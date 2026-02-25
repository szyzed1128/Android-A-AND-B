using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// WebSocket桥接层 - 封装WebSocket客户端连接和通信
    /// 用于替换原始应用中的蓝牙通信层
    /// </summary>
    public class WebSocketBridge : IWebSocketBridge, IDisposable
    {
        private ClientWebSocket _wsClient;
        private CancellationTokenSource _cts;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<WSMessage>> _pendingRequests = new();
        private readonly int _responseTimeoutMs;
        private string _serverUrl;
        private bool _disposed;
        private int _reconnectAttempts;
        private const int MAX_RECONNECT_ATTEMPTS = 10;
        private const int RECEIVE_BUFFER_SIZE = 8192;

        /// <summary>
        /// 收到消息时触发
        /// </summary>
        public event EventHandler<WSMessage> MessageReceived;

        /// <summary>
        /// 收到原始文本消息时触发
        /// </summary>
        public event EventHandler<string> RawMessageReceived;

        /// <summary>
        /// 连接状态变更时触发
        /// </summary>
        public event EventHandler<WebSocketState> StateChanged;

        /// <summary>
        /// 连接关闭时触发
        /// </summary>
        public event EventHandler<string> ConnectionClosed;

        /// <summary>
        /// 当前连接状态
        /// </summary>
        public WebSocketState State => _wsClient?.State ?? WebSocketState.None;

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _wsClient?.State == WebSocketState.Open;

        public WebSocketBridge(int responseTimeoutMs = 10000)
        {
            _responseTimeoutMs = responseTimeoutMs;
        }

        /// <summary>
        /// 连接到WebSocket服务器
        /// </summary>
        public async Task ConnectAsync(string url)
        {
            _serverUrl = url;
            _reconnectAttempts = 0;
            await ConnectInternalAsync().ConfigureAwait(false);
        }

        private async Task ConnectInternalAsync()
        {
            _wsClient?.Dispose();
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            _wsClient = new ClientWebSocket();
            _wsClient.Options.KeepAliveInterval = TimeSpan.FromSeconds(30);

            try
            {
                await _wsClient.ConnectAsync(new Uri(_serverUrl), _cts.Token).ConfigureAwait(false);
                OnStateChanged(WebSocketState.Open);
                _reconnectAttempts = 0;
                // 启动消息接收循环
                _ = ReceiveLoopAsync(_cts.Token);
                // 启动心跳
                _ = HeartbeatLoopAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                OnStateChanged(WebSocketState.Closed);
                throw new Exception($"WebSocket连接失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 发送消息（无需等待响应）
        /// </summary>
        public async Task SendAsync(WSMessage message)
        {
            if (!IsConnected)
                throw new InvalidOperationException("WebSocket未连接");

            var json = message.ToJson();
            var bytes = Encoding.UTF8.GetBytes(json);
            await _wsClient.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                _cts.Token);
        }

        /// <summary>
        /// 发送原始JSON消息（用于UI桥接）
        /// </summary>
        public async Task SendRawAsync(string json)
        {
            if (!IsConnected)
                throw new InvalidOperationException("WebSocket未连接");
            if (string.IsNullOrWhiteSpace(json))
                return;

            var bytes = Encoding.UTF8.GetBytes(json);
            await _wsClient.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                _cts.Token);
        }

        /// <summary>
        /// 发送请求并等待对应的响应
        /// </summary>
        public async Task<WSMessage> SendAndWaitAsync(WSMessage request)
        {
            if (string.IsNullOrEmpty(request.RequestId))
                request.RequestId = Guid.NewGuid().ToString("N").Substring(0, 8);

            var tcs = new TaskCompletionSource<WSMessage>();
            _pendingRequests[request.RequestId] = tcs;

            try
            {
                await SendAsync(request).ConfigureAwait(false);
                using var timeoutCts = new CancellationTokenSource(_responseTimeoutMs);
                timeoutCts.Token.Register(() => tcs.TrySetException(
                    new TimeoutException($"等待响应超时: {request.Action}")));
                return await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                _pendingRequests.TryRemove(request.RequestId, out _);
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public async Task CloseAsync()
        {
            if (_wsClient?.State == WebSocketState.Open)
            {
                try
                {
                    await _wsClient.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closing",
                        CancellationToken.None);
                }
                catch { /* 忽略关闭时的错误 */ }
            }

            _cts?.Cancel();
            OnStateChanged(WebSocketState.Closed);
        }

        /// <summary>
        /// 消息接收循环
        /// </summary>
        private async Task ReceiveLoopAsync(CancellationToken ct)
        {
            var buffer = new byte[RECEIVE_BUFFER_SIZE];
            var messageBuffer = new StringBuilder();

            try
            {
                while (!ct.IsCancellationRequested && _wsClient.State == WebSocketState.Open)
                {
                    var result = await _wsClient.ReceiveAsync(
                        new ArraySegment<byte>(buffer), ct).ConfigureAwait(false);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        ConnectionClosed?.Invoke(this, "Server closed connection");
                        await TryReconnectAsync().ConfigureAwait(false);
                        return;
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        messageBuffer.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                        if (result.EndOfMessage)
                        {
                            var json = messageBuffer.ToString();
                            messageBuffer.Clear();
                            ProcessMessage(json);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消
            }
            catch (WebSocketException ex)
            {
                ConnectionClosed?.Invoke(this, $"WebSocket error: {ex.Message}");
                await TryReconnectAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 处理收到的消息
        /// </summary>
        private void ProcessMessage(string json)
        {
            try
            {
                RawMessageReceived?.Invoke(this, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"原始消息回调异常: {ex.Message}");
            }

            try
            {
                var message = WSMessage.FromJson(json);

                // 如果是响应消息，匹配到对应的请求
                if (message.Type == MessageType.Response && !string.IsNullOrEmpty(message.RequestId))
                {
                    if (_pendingRequests.TryRemove(message.RequestId, out var tcs))
                    {
                        tcs.TrySetResult(message);
                        return;
                    }
                }

                // 其他消息通过事件通知
                MessageReceived?.Invoke(this, message);
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"JSON解析错误: {ex.Message}, 原始数据: {json}");
            }
        }

        /// <summary>
        /// 心跳循环
        /// </summary>
        private async Task HeartbeatLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested && IsConnected)
            {
                try
                {
                    await Task.Delay(30000, ct).ConfigureAwait(false);
                    if (IsConnected)
                    {
                        var ping = WSMessageFactory.CreatePingRequest();
                        await SendAsync(ping).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException) { break; }
                catch { /* 心跳失败不中断 */ }
            }
        }

        /// <summary>
        /// 尝试自动重连（指数退避）
        /// </summary>
        private async Task TryReconnectAsync()
        {
            if (_disposed || string.IsNullOrEmpty(_serverUrl))
                return;

            while (_reconnectAttempts < MAX_RECONNECT_ATTEMPTS && !_disposed)
            {
                _reconnectAttempts++;
                var delay = Math.Min(1000 * (int)Math.Pow(2, _reconnectAttempts - 1), 16000);
                System.Diagnostics.Debug.WriteLine(
                    $"WebSocket重连尝试 {_reconnectAttempts}/{MAX_RECONNECT_ATTEMPTS}, {delay}ms后重试");

                await Task.Delay(delay).ConfigureAwait(false);

                try
                {
                    await ConnectInternalAsync().ConfigureAwait(false);
                    System.Diagnostics.Debug.WriteLine("WebSocket重连成功");
                    return;
                }
                catch
                {
                    // 继续重试
                }
            }

            System.Diagnostics.Debug.WriteLine("WebSocket重连失败，已达最大重试次数");
            OnStateChanged(WebSocketState.Closed);
        }

        private void OnStateChanged(WebSocketState state)
        {
            StateChanged?.Invoke(this, state);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _cts?.Cancel();
            _wsClient?.Dispose();
            _cts?.Dispose();
        }
    }
}
