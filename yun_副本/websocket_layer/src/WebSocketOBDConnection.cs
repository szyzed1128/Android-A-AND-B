using System;
using System.Threading;
using System.Threading.Tasks;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// 基于WebSocket的 ELM327 连接管理
    /// 替换原始应用中的 BluetoothConnectionV3.ConnectAsync / WriteAsync
    /// 以及 Plugin.BLE 的 ICharacteristic 读写操作
    /// </summary>
    public class WebSocketOBDConnection : IDisposable
    {
        private readonly IWebSocketBridge _bridge;
        private string _sessionId;
        private bool _isConnected;
        private bool _disposed;

        // 用于同步读取模式（替换 BluetoothConnectionV3.ReadBytesAsync）
        private TaskCompletionSource<byte[]> _readTcs;
        private readonly object _readLock = new object();

        /// <summary>
        /// 收到诊断数据时触发
        /// 对应原始: ICharacteristic.ValueUpdated 事件
        /// </summary>
        public event EventHandler<byte[]> DataReceived;

        /// <summary>
        /// 连接丢失时触发
        /// 对应原始: IAdapter.DeviceConnectionLost 事件
        /// </summary>
        public event EventHandler<string> ConnectionLost;

        /// <summary>
        /// 当前会话ID
        /// </summary>
        public string SessionId => _sessionId;

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// 绑定来自B端的会话ID（B先连接蓝牙的场景）
        /// </summary>
        public void BindSession(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return;
            _sessionId = sessionId;
            _isConnected = true;
        }

        /// <summary>
        /// 清理当前会话（不发送断开请求）
        /// </summary>
        public void ResetSession()
        {
            _sessionId = null;
            _isConnected = false;
        }

        public WebSocketOBDConnection(IWebSocketBridge bridge)
        {
            _bridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
            _bridge.MessageReceived += OnBridgeMessageReceived;
        }

        /// <summary>
        /// 连接到 ELM327 设备
        /// 对应原始: BluetoothConnectionV3.ConnectAsync(device_id, debugStream)
        /// </summary>
        /// <param name="protocol">连接协议: "bt" (蓝牙) 或 "wifi"</param>
        /// <param name="address">设备地址 (MAC地址或IP:端口)</param>
        public async Task<bool> ConnectAsync(string protocol, string address)
        {
            System.Diagnostics.Debug.WriteLine($"[WebSocketOBD] ConnectAsync: {protocol} {address}");
            var request = WSMessageFactory.CreateConnectRequest(protocol, address);
            var response = await _bridge.SendAndWaitAsync(request).ConfigureAwait(false);

            if (response.Success == true)
            {
                _sessionId = response.SessionId;
                _isConnected = true;
                System.Diagnostics.Debug.WriteLine($"[WebSocketOBD] Connected session={_sessionId}");
                return true;
            }
            else
            {
                throw new Exception($"ELM327 连接失败: {response.Error}");
            }
        }

        /// <summary>
        /// 发送数据到 ELM327 设备
        /// 对应原始: ICharacteristic.WriteAsync(bytes)
        /// 或 DebugStream.WriteAsync(bytes)
        /// </summary>
        public async Task SendAsync(byte[] data)
        {
            if (!_isConnected)
                throw new InvalidOperationException("ELM327 未连接");

            System.Diagnostics.Debug.WriteLine($"[WebSocketOBD] Send len={data?.Length ?? 0}");
            var request = WSMessageFactory.CreateSendRequest(_sessionId, data);
            var response = await _bridge.SendAndWaitAsync(request).ConfigureAwait(false);

            if (response.Success != true)
            {
                throw new Exception($"数据发送失败: {response.Error}");
            }
        }

        /// <summary>
        /// 发送字符串命令到 ELM327 设备（便捷方法）
        /// 例如: SendCommandAsync("010D\r")
        /// </summary>
        public async Task SendCommandAsync(string command)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(command);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// 等待下一个诊断数据到达（同步读取模式）
        /// 替换原始: BluetoothConnectionV3.ReadBytesAsync()
        /// </summary>
        /// <param name="timeoutMs">超时时间（毫秒），默认10秒</param>
        /// <returns>接收到的数据</returns>
        public async Task<byte[]> ReadNextDataAsync(int timeoutMs = 10000)
        {
            TaskCompletionSource<byte[]> tcs;

            lock (_readLock)
            {
                // 取消之前的等待
                _readTcs?.TrySetCanceled();
                _readTcs = new TaskCompletionSource<byte[]>();
                tcs = _readTcs;
            }

            using (var cts = new CancellationTokenSource(timeoutMs))
            {
                cts.Token.Register(() => tcs.TrySetException(
                    new TimeoutException("等待诊断数据超时")));

                try
                {
                    System.Diagnostics.Debug.WriteLine($"[WebSocketOBD] ReadNextDataAsync timeout={timeoutMs}ms");
                    return await tcs.Task.ConfigureAwait(false);
                }
                finally
                {
                    lock (_readLock)
                    {
                        if (_readTcs == tcs)
                            _readTcs = null;
                    }
                }
            }
        }

        /// <summary>
        /// 断开 ELM327 连接
        /// 对应原始: IAdapter.DisconnectDeviceAsync(device)
        /// </summary>
        public async Task DisconnectAsync()
        {
            if (!_isConnected) return;

            try
            {
                var request = WSMessageFactory.CreateDisconnectRequest(_sessionId);
                await _bridge.SendAndWaitAsync(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"断开连接异常: {ex.Message}");
            }
            finally
            {
                _isConnected = false;
                _sessionId = null;
            }
        }

        /// <summary>
        /// 处理来自B端的消息
        /// </summary>
        private void OnBridgeMessageReceived(object sender, WSMessage message)
        {
            // 只处理当前会话的消息
            if (!string.IsNullOrEmpty(message.SessionId) && message.SessionId != _sessionId)
                return;

            switch (message.Action)
            {
                case MessageAction.OBDData:
                    byte[] data = null;
                    try
                    {
                        // obdData 事件的 data 可能是:
                        // 1. 直接的 base64 字符串
                        // 2. 嵌套对象 { sessionId, data: base64 }
                        if (message.Data is string directBase64)
                        {
                            data = Convert.FromBase64String(directBase64);
                        }
                        else
                        {
                            // 尝试从嵌套结构获取
                            var dataObj = message.GetData<Newtonsoft.Json.Linq.JObject>();
                            if (dataObj != null)
                            {
                                var innerData = dataObj.Value<string>("data");
                                if (!string.IsNullOrEmpty(innerData))
                                {
                                    data = Convert.FromBase64String(innerData);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[ELM327Connection] 解析诊断数据失败: {ex.Message}");
                    }

                    if (data != null)
                    {
                        // 先尝试设置同步读取的结果
                        lock (_readLock)
                        {
                            _readTcs?.TrySetResult(data);
                        }
                        // 同时触发事件（异步模式）
                        DataReceived?.Invoke(this, data);
                    }
                    break;

                case MessageAction.ConnectionLost:
                    _isConnected = false;
                    _sessionId = null;
                    var reason = "Unknown";
                    try
                    {
                        var errorData = message.GetData<dynamic>();
                        reason = errorData?.reason?.ToString() ?? "Unknown";
                    }
                    catch { }
                    ConnectionLost?.Invoke(this, reason);
                    break;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _bridge.MessageReceived -= OnBridgeMessageReceived;
        }
    }
}
