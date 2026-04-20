using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// WebSocket服务器桥接层 - 在A端监听，B端作为客户端连接
    /// </summary>
    public class WebSocketServerBridge : IWebSocketBridge, IDisposable
    {
        private TcpListener _listener;
        private TcpClient _client;
        private NetworkStream _stream;
        private CancellationTokenSource _cts;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<WSMessage>> _pendingRequests = new();
        private readonly int _responseTimeoutMs;
        private bool _disposed;
        private WebSocketState _state = WebSocketState.None;
        private readonly object _clientLock = new object();
        private DateTime _lastClientSeenUtc = DateTime.UtcNow;
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);
        private volatile bool _clientReady;

        private const int HEADER_READ_BUFFER = 4096;
        private const int HEARTBEAT_INTERVAL_MS = 10000;

        public event EventHandler<WSMessage> MessageReceived;
        public event EventHandler<string> RawMessageReceived;
        public event EventHandler<WebSocketState> StateChanged;
        public event EventHandler<string> ConnectionClosed;

        public WebSocketState State => _state;
        public bool IsConnected => _clientReady && _client != null && _client.Connected && _stream != null;

        public WebSocketServerBridge(int responseTimeoutMs = 10000)
        {
            _responseTimeoutMs = responseTimeoutMs;
        }

        /// <summary>
        /// 启动WebSocket服务器监听
        /// serverUrl 用于解析端口，例如 ws://0.0.0.0:8080/ws
        /// </summary>
        public async Task ConnectAsync(string serverUrl)
        {
            var (ip, port) = ParseListenEndpoint(serverUrl);
            Log($"[WS-Server] Start listen {ip}:{port}");

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            _listener?.Stop();
            _listener = new TcpListener(ip, port);
            _listener.Start();

            _state = WebSocketState.Open;
            StateChanged?.Invoke(this, _state);

            // 后台接受连接
            _ = AcceptLoopAsync(_cts.Token);

            await Task.CompletedTask.ConfigureAwait(false);
        }

        private async Task AcceptLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                TcpClient client = null;
                try
                {
                    client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                }
                catch
                {
                    if (ct.IsCancellationRequested) break;
                }

                if (client == null) continue;
                var remote = client.Client?.RemoteEndPoint?.ToString() ?? "unknown";
                Log($"[WS-Server] Client accepted {remote}");

                // 只保留一个客户端连接，新的连接覆盖旧的
                var hadExistingClient = false;
                lock (_clientLock)
                {
                    hadExistingClient = _client != null || _stream != null || _clientReady;
                }
                if (hadExistingClient)
                {
                    CloseClient("Client replaced by new connection", notify: true);
                }

                lock (_clientLock)
                {
                    _client = client;
                    _stream = client.GetStream();
                    try
                    {
                        _client.Client.NoDelay = true;
                        _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    }
                    catch { }
                }

                var ok = await HandleHandshakeAsync(_stream, ct).ConfigureAwait(false);
                if (!ok)
                {
                    Log("[WS-Server] Handshake failed");
                    CloseClient();
                    continue;
                }
                Log("[WS-Server] Handshake OK");
                _clientReady = true;

                _ = ReceiveLoopAsync(_stream, ct);
                _ = HeartbeatLoopAsync(ct);
            }
        }

        private async Task<bool> HandleHandshakeAsync(NetworkStream stream, CancellationToken ct)
        {
            string header;
            try
            {
                header = await ReadHttpHeaderAsync(stream, ct).ConfigureAwait(false);
            }
            catch
            {
                return false;
            }

            var key = ExtractWebSocketKey(header);
            if (string.IsNullOrEmpty(key))
            {
                Log("[WS-Server] Missing Sec-WebSocket-Key");
                return false;
            }

            var accept = ComputeAcceptKey(key);
            var response = new StringBuilder();
            response.Append("HTTP/1.1 101 Switching Protocols\r\n");
            response.Append("Upgrade: websocket\r\n");
            response.Append("Connection: Upgrade\r\n");
            response.Append("Sec-WebSocket-Accept: ").Append(accept).Append("\r\n");
            response.Append("\r\n");

            var bytes = Encoding.ASCII.GetBytes(response.ToString());
            await stream.WriteAsync(bytes, 0, bytes.Length, ct).ConfigureAwait(false);
            return true;
        }

        private async Task<string> ReadHttpHeaderAsync(NetworkStream stream, CancellationToken ct)
        {
            var buffer = new byte[HEADER_READ_BUFFER];
            var sb = new StringBuilder();

            while (!ct.IsCancellationRequested)
            {
                var read = await stream.ReadAsync(buffer, 0, buffer.Length, ct).ConfigureAwait(false);
                if (read <= 0) break;

                sb.Append(Encoding.ASCII.GetString(buffer, 0, read));
                if (sb.ToString().Contains("\r\n\r\n"))
                    break;
            }

            return sb.ToString();
        }

        private static string ExtractWebSocketKey(string header)
        {
            var lines = header.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.StartsWith("Sec-WebSocket-Key:", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Substring("Sec-WebSocket-Key:".Length).Trim();
                }
            }
            return null;
        }

        private static string ComputeAcceptKey(string key)
        {
            const string magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(key + magic));
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// 接收消息循环
        /// </summary>
        private async Task ReceiveLoopAsync(NetworkStream stream, CancellationToken ct)
        {
            try
            {
                byte currentOpcode = 0;
                using var fragmentBuffer = new System.IO.MemoryStream();
                var hasFragments = false;
                while (!ct.IsCancellationRequested && _client != null && _client.Connected)
                {
                    var frame = await ReadFrameAsync(stream, ct).ConfigureAwait(false);
                    if (frame == null)
                    {
                        if (!ct.IsCancellationRequested)
                        {
                            CloseClientForStream(stream, "Client disconnected (EOF)", notify: true);
                        }
                        break;
                    }
                    _lastClientSeenUtc = DateTime.UtcNow;

                    if (frame.Opcode == 0x08)
                    {
                        CloseClientForStream(stream, "Client closed connection", notify: true);
                        break;
                    }

                    if (frame.Opcode == 0x09)
                    {
                        await SendControlFrameAsync(0x0A, frame.Payload, ct).ConfigureAwait(false);
                        continue;
                    }

                    if (frame.Opcode == 0x0A)
                    {
                        // Pong
                        Log("[WS-Server] PONG received");
                        continue;
                    }

                    if (frame.Opcode == 0x00)
                    {
                        // continuation
                        if (!hasFragments)
                        {
                            // unexpected continuation; ignore
                            continue;
                        }
                        fragmentBuffer.Write(frame.Payload, 0, frame.Payload.Length);
                        if (frame.Fin)
                        {
                            if (currentOpcode == 0x01)
                            {
                                var json = Encoding.UTF8.GetString(fragmentBuffer.ToArray());
                                ProcessMessage(json);
                            }
                            fragmentBuffer.SetLength(0);
                            hasFragments = false;
                            currentOpcode = 0;
                        }
                        continue;
                    }

                    if (frame.Opcode == 0x01)
                    {
                        if (frame.Fin)
                        {
                            var json = Encoding.UTF8.GetString(frame.Payload);
                            ProcessMessage(json);
                        }
                        else
                        {
                            fragmentBuffer.SetLength(0);
                            fragmentBuffer.Write(frame.Payload, 0, frame.Payload.Length);
                            hasFragments = true;
                            currentOpcode = 0x01;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                if (!ct.IsCancellationRequested)
                {
                    CloseClientForStream(stream, $"WebSocket receive error: {ex.Message}", notify: true);
                }
            }
        }

        private async Task HeartbeatLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(HEARTBEAT_INTERVAL_MS, ct).ConfigureAwait(false);
                    if (ct.IsCancellationRequested) break;
                    if (!IsConnected || _stream == null) continue;

                    // 若长时间未收到任何帧，也主动发 Ping
                    var idleMs = (DateTime.UtcNow - _lastClientSeenUtc).TotalMilliseconds;
                    if (idleMs >= HEARTBEAT_INTERVAL_MS)
                    {
                        await SendControlFrameAsync(0x09, Array.Empty<byte>(), ct).ConfigureAwait(false);
                        Log($"[WS-Server] PING sent (idle {idleMs:0}ms)");
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    Log($"[WS-Server] Heartbeat error: {ex.Message}");
                }
            }
        }

        private void ProcessMessage(string json)
        {
            try
            {
                RawMessageReceived?.Invoke(this, json);
            }
            catch
            {
                // ignore
            }

            try
            {
                var message = WSMessage.FromJson(json);
                Log($"[WS-Server] RX type={message.Type} action={message.Action} req={message.RequestId ?? "-"} session={message.SessionId ?? "-"} data={DescribeData(message.Data)}");
                if (message.Type == MessageType.Response && !string.IsNullOrEmpty(message.RequestId))
                {
                    if (_pendingRequests.TryRemove(message.RequestId, out var tcs))
                    {
                        tcs.TrySetResult(message);
                        return;
                    }
                }

                MessageReceived?.Invoke(this, message);
            }
            catch (JsonException)
            {
                // ignore parse error
            }
        }

        public async Task SendAsync(WSMessage message)
        {
            if (!IsConnected) throw new InvalidOperationException("WebSocket客户端未连接");
            var json = message.ToJson();
            Log($"[WS-Server] SendAsync type={message.Type} action={message.Action} json.len={json.Length}");
            await SendTextAsync(json).ConfigureAwait(false);
            Log($"[WS-Server] SendAsync 完成 type={message.Type} action={message.Action}");
        }

        public async Task SendRawAsync(string json)
        {
            if (!IsConnected) throw new InvalidOperationException("WebSocket客户端未连接");
            if (string.IsNullOrWhiteSpace(json)) return;
            await SendTextAsync(json).ConfigureAwait(false);
        }

        public async Task<WSMessage> SendAndWaitAsync(WSMessage request)
        {
            if (string.IsNullOrEmpty(request.RequestId))
                request.RequestId = Guid.NewGuid().ToString("N").Substring(0, 8);

            var tcs = new TaskCompletionSource<WSMessage>();
            _pendingRequests[request.RequestId] = tcs;
            var start = DateTime.UtcNow;
            Log($"[WS-Server] TX-REQ action={request.Action} req={request.RequestId} session={request.SessionId ?? "-"} data={DescribeData(request.Data)}");

            try
            {
                await SendAsync(request).ConfigureAwait(false);
                using var timeoutCts = new CancellationTokenSource(_responseTimeoutMs);
                timeoutCts.Token.Register(() =>
                {
                    Log($"[WS-Server] TIMEOUT action={request.Action} req={request.RequestId} after {_responseTimeoutMs}ms");
                    tcs.TrySetException(new TimeoutException($"等待响应超时: {request.Action}"));
                });
                var response = await tcs.Task.ConfigureAwait(false);
                var elapsedMs = (DateTime.UtcNow - start).TotalMilliseconds;
                Log($"[WS-Server] RX-RESP action={request.Action} req={request.RequestId} ok={response.Success} ms={elapsedMs:0}");
                return response;
            }
            finally
            {
                _pendingRequests.TryRemove(request.RequestId, out _);
            }
        }

        private async Task SendTextAsync(string text)
        {
            var payload = Encoding.UTF8.GetBytes(text);
            await SendFrameAsync(0x01, payload, _cts?.Token ?? CancellationToken.None).ConfigureAwait(false);
        }

        private async Task SendControlFrameAsync(byte opcode, byte[] payload, CancellationToken ct)
        {
            await SendFrameAsync(opcode, payload ?? Array.Empty<byte>(), ct).ConfigureAwait(false);
        }

        private async Task SendFrameAsync(byte opcode, byte[] payload, CancellationToken ct)
        {
            if (!IsConnected || _stream == null) return;

            var lockTaken = false;
            try
            {
                await _sendLock.WaitAsync(ct).ConfigureAwait(false);
                lockTaken = true;

                var header = BuildFrameHeader(opcode, payload.Length);
                // 合并 header 和 payload 为单次写入，避免 TCP 分片问题
                var frame = new byte[header.Length + payload.Length];
                Buffer.BlockCopy(header, 0, frame, 0, header.Length);
                if (payload.Length > 0)
                    Buffer.BlockCopy(payload, 0, frame, header.Length, payload.Length);

                Console.WriteLine($"[WS-Server] SendFrame opcode={opcode} payload.len={payload.Length} frame.len={frame.Length}");
                await _stream.WriteAsync(frame, 0, frame.Length, ct).ConfigureAwait(false);
                await _stream.FlushAsync(ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WS-Server] SendFrame error: {ex.Message}");
                CloseClient($"WebSocket send error: {ex.Message}", notify: true);
            }
            finally
            {
                if (lockTaken)
                    _sendLock.Release();
            }
        }

        private static byte[] BuildFrameHeader(byte opcode, int length)
        {
            if (length < 126)
            {
                return new[] { (byte)(0x80 | opcode), (byte)length };
            }
            if (length <= 65535)
            {
                var header = new byte[4];
                header[0] = (byte)(0x80 | opcode);
                header[1] = 126;
                header[2] = (byte)((length >> 8) & 0xFF);
                header[3] = (byte)(length & 0xFF);
                return header;
            }

            var longHeader = new byte[10];
            longHeader[0] = (byte)(0x80 | opcode);
            longHeader[1] = 127;
            // 只支持 32 位长度
            longHeader[6] = (byte)((length >> 24) & 0xFF);
            longHeader[7] = (byte)((length >> 16) & 0xFF);
            longHeader[8] = (byte)((length >> 8) & 0xFF);
            longHeader[9] = (byte)(length & 0xFF);
            return longHeader;
        }

        private async Task<WebSocketFrame> ReadFrameAsync(NetworkStream stream, CancellationToken ct)
        {
            var header = new byte[2];
            if (!await ReadExactAsync(stream, header, 0, 2, ct).ConfigureAwait(false))
                return null;

            var first = header[0];
            var second = header[1];

            var fin = (first & 0x80) != 0;
            var opcode = (byte)(first & 0x0F);
            var masked = (second & 0x80) != 0;
            var payloadLen = (ulong)(second & 0x7F);

            if (payloadLen == 126)
            {
                var ext = new byte[2];
                if (!await ReadExactAsync(stream, ext, 0, 2, ct).ConfigureAwait(false))
                    return null;
                payloadLen = (ulong)((ext[0] << 8) | ext[1]);
            }
            else if (payloadLen == 127)
            {
                var ext = new byte[8];
                if (!await ReadExactAsync(stream, ext, 0, 8, ct).ConfigureAwait(false))
                    return null;
                payloadLen = (ulong)((ext[4] << 24) | (ext[5] << 16) | (ext[6] << 8) | ext[7]);
            }

            byte[] mask = null;
            if (masked)
            {
                mask = new byte[4];
                if (!await ReadExactAsync(stream, mask, 0, 4, ct).ConfigureAwait(false))
                    return null;
            }

            var payload = new byte[payloadLen];
            if (payloadLen > 0)
            {
                if (!await ReadExactAsync(stream, payload, 0, (int)payloadLen, ct).ConfigureAwait(false))
                    return null;
            }

            if (masked && mask != null)
            {
                for (var i = 0; i < payload.Length; i++)
                {
                    payload[i] ^= mask[i % 4];
                }
            }

            return new WebSocketFrame
            {
                Fin = fin,
                Opcode = opcode,
                Payload = payload
            };
        }

        private static async Task<bool> ReadExactAsync(NetworkStream stream, byte[] buffer, int offset, int count, CancellationToken ct)
        {
            var total = 0;
            while (total < count)
            {
                var read = await stream.ReadAsync(buffer, offset + total, count - total, ct).ConfigureAwait(false);
                if (read <= 0) return false;
                total += read;
            }
            return true;
        }

        private static (IPAddress, int) ParseListenEndpoint(string serverUrl)
        {
            var port = 8080;
            IPAddress ip = IPAddress.Any;

            if (Uri.TryCreate(serverUrl, UriKind.Absolute, out var uri))
            {
                if (uri.Port > 0) port = uri.Port;
                if (!string.IsNullOrEmpty(uri.Host) && IPAddress.TryParse(uri.Host, out var parsed))
                    ip = parsed;
            }

            return (ip, port);
        }

        public async Task CloseAsync()
        {
            _cts?.Cancel();
            CloseClient();
            _listener?.Stop();
            _listener = null;
            _state = WebSocketState.Closed;
            StateChanged?.Invoke(this, _state);
            await Task.CompletedTask.ConfigureAwait(false);
        }

        private void CloseClientForStream(NetworkStream stream, string reason, bool notify)
        {
            bool isCurrentStream;
            lock (_clientLock)
            {
                isCurrentStream = ReferenceEquals(_stream, stream);
            }

            if (!isCurrentStream)
            {
                Log($"[WS-Server] Ignore close for stale stream reason={reason ?? "(空)"}");
                return;
            }

            CloseClient(reason, notify);
        }

        private void CloseClient(string reason = null, bool notify = false)
        {
            var hadClient = _stream != null || _client != null || _clientReady || !_pendingRequests.IsEmpty;
            if (hadClient)
            {
                Log($"[WS-Server] Client closed reason={reason ?? "(空)"} notify={notify}");
            }

            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _stream = null;
            _client = null;
            _clientReady = false;
            foreach (var kvp in _pendingRequests)
            {
                kvp.Value.TrySetException(new Exception("WebSocket客户端断开"));
            }
            _pendingRequests.Clear();

            if (notify && hadClient)
            {
                try
                {
                    ConnectionClosed?.Invoke(this, string.IsNullOrWhiteSpace(reason) ? "Client disconnected" : reason);
                }
                catch { }
            }
        }

        private static string DescribeData(object data)
        {
            if (data == null) return "null";
            if (data is string s) return $"string(len={s.Length})";
            return data.GetType().Name;
        }

        private static void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _cts?.Cancel();
            CloseClient();
            _listener?.Stop();
            _cts?.Dispose();
        }

        private class WebSocketFrame
        {
            public bool Fin { get; set; }
            public byte Opcode { get; set; }
            public byte[] Payload { get; set; }
        }
    }
}
