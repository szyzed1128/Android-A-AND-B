using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// 基于WebSocket的蓝牙连接适配器
    ///
    /// 此类的方法签名与 IOBDConnection 接口完全匹配：
    /// - ReadBytesAsync() 返回 ValueTask<byte[]>
    /// - WriteBytesAsync(byte[] data) 返回 Task
    /// - ConnectAsync(string, object) 返回 Task<bool>
    /// - Disconect() 返回 void
    /// - 属性：KeepAlive, NoDelay, Connected, NeedFlush
    ///
    /// 通过 DLL 修补，将此对象赋值给 OBDDataReader.Connection 字段，
    /// 使所有AT命令通过WebSocket转发到手机B。
    ///
    /// 数据流：
    /// OBDDataReader.SendString("ATZ\r")
    ///   → WebSocketBluetoothConnection.WriteBytesAsync()
    ///   → WebSocket → 手机B
    ///   → 蓝牙 → ELM327
    ///   → ELM327响应 → 蓝牙 → 手机B
    ///   → WebSocket → WebSocketBluetoothConnection.ReadBytesAsync()
    ///   → OBDDataReader.ReadData()
    /// </summary>
    public class WebSocketBluetoothConnection : IDisposable
    {
        #region Android 日志

        private static System.Reflection.MethodInfo? _androidLogMethod;
        private static bool _androidLogInitialized;

        private static void Log(string message)
        {
            // 同时输出到 Console 和 Debug
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);

            // 尝试通过反射调用 Android.Util.Log
            if (!_androidLogInitialized)
            {
                _androidLogInitialized = true;
                try
                {
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var logType = asm.GetType("Android.Util.Log");
                        if (logType != null)
                        {
                            _androidLogMethod = logType.GetMethod("Info",
                                new Type[] { typeof(string), typeof(string) });
                            break;
                        }
                    }
                }
                catch { }
            }

            if (_androidLogMethod != null)
            {
                try
                {
                    _androidLogMethod.Invoke(null, new object[] { "WebSocketBT", message });
                }
                catch { }
            }
        }

        #endregion

        private readonly IWebSocketBridge _bridge;
        private string _sessionId;
        private bool _isConnected;
        private bool _disposed;

        // 数据接收缓冲区（模拟原始蓝牙的数据接收）
        // 存储 (数据, 入队时间戳) 用于计算延迟
        private readonly ConcurrentQueue<(byte[] data, DateTime enqueueTime)> _receiveBuffer = new ConcurrentQueue<(byte[], DateTime)>();

        // 超时设置（仅 WriteBytesAsync 使用）
        private int _writeTimeoutMs = 5000;

        // 诊断日志：时序追踪（相对会话开始的毫秒数 + 操作序号）
        private readonly DateTime _sessionStart = DateTime.UtcNow;
        private int _writeSeq;
        private int _readSeq;

        // 监测：空读取计数（每 50 次记录一次）
        private int _emptyReadCount;
        private int _emptyReadTotal;

        // 诊断：上条 AT 命令完成情况追踪（用于定位 ELMStuck 根因）
        private string _diagCmdText = "";
        private DateTime _diagWriteTime = DateTime.MinValue;
        private bool _diagDataReceived;
        private bool _diagPromptReceived;
        private int _diagBytesReceived;

        #region IOBDConnection 接口属性

        /// <summary>
        /// 是否保持连接活跃
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// 是否禁用 Nagle 算法
        /// </summary>
        public bool NoDelay { get; set; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool Connected => _isConnected && _bridge?.IsConnected == true;

        /// <summary>
        /// 是否需要刷新（WebSocket 不需要手动刷新）
        /// </summary>
        public bool NeedFlush => false;

        /// <summary>
        /// 连接超时秒数（IBluetoothConnection 接口）
        /// </summary>
        public int ConnectionTimeoutSeconds { get; set; } = 5;

        #endregion

        public WebSocketBluetoothConnection(IWebSocketBridge bridge)
        {
            _bridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
            _bridge.MessageReceived += OnBridgeMessageReceived;
        }

        #region IOBDConnection 接口方法

        /// <summary>
        /// 连接到 ELM327 设备
        /// 签名匹配 IOBDConnection.ConnectAsync(string host_port, object debugStream)
        ///
        /// host_port 格式:
        /// - 蓝牙: MAC地址 "00:1D:A5:68:98:8B" 或 GUID
        /// - WiFi: "192.168.0.10:35000"
        /// </summary>
        public async Task<bool> ConnectAsync(string deviceAddress, object? debugStream)
        {
            if (_isConnected && !string.IsNullOrWhiteSpace(_sessionId))
            {
                if (_bridge == null || !_bridge.IsConnected)
                {
                    // Bridge 已断开（B 端重连），必须重置后重新建立会话
                    Log("[WebSocketBT] Bridge 已断开，重置状态后重新连接");
                    _isConnected = false;
                    _sessionId = null;
                    // 继续执行连接逻辑
                }
                else
                {
                    Log("[WebSocketBT] 已绑定会话，跳过重新连接");
                    return true;
                }
            }

            if (string.IsNullOrWhiteSpace(deviceAddress))
                return false;

            // 清空接收缓冲区和监测计数器
            while (_receiveBuffer.TryDequeue(out _)) { }
            _emptyReadCount = 0;
            _emptyReadTotal = 0;

            try
            {
                // 判断连接协议
                string protocol = DetectProtocol(deviceAddress);

                Log($"[WebSocketBT] 连接 ELM327: {protocol} -> {deviceAddress}");

                // 发送连接请求到手机B
                var request = WSMessageFactory.CreateConnectRequest(protocol, deviceAddress);
                var response = await _bridge.SendAndWaitAsync(request).ConfigureAwait(false);

                if (response.Success == true)
                {
                    _sessionId = response.SessionId;
                    _isConnected = true;
                    Log($"[WebSocketBT] 连接成功, sessionId={_sessionId}");
                    return true;
                }
                else
                {
                    Log($"[WebSocketBT] 连接失败: {response.Error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log($"[WebSocketBT] 连接异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 发送数据到 ELM327
        /// 原始: Task WriteBytesAsync(byte[] data)
        /// </summary>
        public async Task WriteBytesAsync(byte[] data)
        {
            if (!Connected)
                throw new InvalidOperationException("ELM327 未连接");

            if (data == null || data.Length == 0)
                return;

            // 解码命令文本（提取到 try 外，供诊断和日志共用）
            string cmdText;
            try { cmdText = Encoding.ASCII.GetString(data).Replace("\r", "\\r").Replace("\n", "\\n"); }
            catch { cmdText = $"[binary len={data.Length}]"; }

            // 调试：显示发送的命令（带序号和时间戳）
            var wSeq = System.Threading.Interlocked.Increment(ref _writeSeq);
            Log($"[WebSocketBT] {T()} ▶▶ W#{wSeq} AT命令: [{cmdText}] (len={data.Length})");

            // ---- 诊断：打印上条 AT 命令的完成情况，用于识别 ELMStuck 根因 ----
            // 有数据=False → 纯超时（ReadDataTimeoutException）
            // 有数据=True 有'>'=False → 数据不完整（GeneralReadingException）
            if (_diagWriteTime != DateTime.MinValue)
            {
                var prevElapsed = (DateTime.UtcNow - _diagWriteTime).TotalMilliseconds;
                Log($"[WebSocketBT-DIAG] ↑ 上条命令: cmd=[{_diagCmdText}] elapsed={prevElapsed:0}ms 有数据={_diagDataReceived} 有'>'={_diagPromptReceived} 总字节={_diagBytesReceived}");
            }
            _diagCmdText = cmdText;
            _diagWriteTime = DateTime.UtcNow;
            _diagDataReceived = false;
            _diagPromptReceived = false;
            _diagBytesReceived = 0;
            // ---- 诊断结束 ----

            // 发送数据到手机B，记录往返延迟（A→WebSocket→B→蓝牙→ELM327→蓝牙→B→WebSocket→A 全链路）
            var wStart = DateTime.UtcNow;
            var request = WSMessageFactory.CreateSendRequest(_sessionId, data);
            var response = await _bridge.SendAndWaitAsync(request).ConfigureAwait(false);
            var wRtt = (DateTime.UtcNow - wStart).TotalMilliseconds;

            if (response.Success != true)
            {
                Log($"[WebSocketBT] {T()} ✗ W#{wSeq} 发送失败 (RTT={wRtt:0}ms): {response.Error}");
                throw new Exception($"发送失败: {response.Error}");
            }
            Log($"[WebSocketBT] {T()} ✓ W#{wSeq} 发送成功 (RTT={wRtt:0}ms)");
        }

        /// <summary>
        /// 读取 ELM327 响应数据
        /// 注意：返回 Task<byte[]> 而不是 ValueTask<byte[]>
        /// 在运行时通过 DllPatcher 注入的反射调用来绕过类型不匹配问题
        /// </summary>
        public async Task<byte[]> ReadBytesAsync()
        {
            var rSeq = System.Threading.Interlocked.Increment(ref _readSeq);
            // 与原版 BTLEConnection.ReadBytesAsync 完全一致：非阻塞立即返回
            // 有数据就返回数据，没数据立即返回空数组
            // OBDDataReader 的 ReadData() 有自己的 2500ms Stopwatch + Task.Delay(10) 轮询循环
            if (_receiveBuffer.TryDequeue(out var item))
            {
                var latencyMs = (DateTime.UtcNow - item.enqueueTime).TotalMilliseconds;
                // 重置空读取计数
                if (_emptyReadCount > 0)
                {
                    Log($"[WebSocketBT] {T()} ◀◀ R#{rSeq} 结束空轮询 (空读次数={_emptyReadCount})");
                    _emptyReadCount = 0;
                }
                Log($"[WebSocketBT] {T()} ◀◀ R#{rSeq} 收到数据 (len={item.data.Length}, 队列延迟={latencyMs:0}ms)");
                LogReceivedData(item.data);
                return item.data;
            }

            // 无数据，记录空读取次数（每 50 次记录一次，避免刷屏）
            _emptyReadCount++;
            _emptyReadTotal++;
            if (_emptyReadCount % 50 == 0)
            {
                Log($"[WebSocketBT] {T()} ◀◀ R#{rSeq} 空轮询中... (连续={_emptyReadCount}, 总计={_emptyReadTotal})");
            }

            // 立即返回空数组（让上层 OBDDataReader 控制超时和轮询）
            return Array.Empty<byte>();
        }

        /// <summary>
        /// 刷新发送缓冲区（WebSocket 不需要手动刷新）
        /// 原始: Task FlushAsync()
        /// </summary>
        public Task FlushAsync()
        {
            // WebSocket 是实时发送的，不需要手动刷新
            return Task.CompletedTask;
        }

        /// <summary>
        /// 断开连接
        /// 原始: void Disconect() - 注意原始接口拼写错误
        /// </summary>
        public void Disconect()
        {
            DisconnectAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// 异步断开连接
        /// </summary>
        public async Task DisconnectAsync()
        {
            if (!_isConnected)
                return;

            try
            {
                Log($"[WebSocketBT] 断开连接");
                var request = WSMessageFactory.CreateDisconnectRequest(_sessionId);
                await _bridge.SendAndWaitAsync(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log($"[WebSocketBT] 断开异常: {ex.Message}");
            }
            finally
            {
                _isConnected = false;
                _sessionId = null;
            }
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 绑定已建立的会话（B端先连接蓝牙的场景）
        /// </summary>
        public void BindSession(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return;

            _sessionId = sessionId;
            _isConnected = true;

            // 清空接收缓冲区，避免旧会话残留数据干扰新会话的 Initialize() 握手
            while (_receiveBuffer.TryDequeue(out _)) { }
            _emptyReadCount = 0;
            _emptyReadTotal = 0;

            Log($"[WebSocketBT] 绑定会话: {sessionId}（已清空接收缓冲区）");
        }

        /// <summary>
        /// 设置写入超时时间
        /// </summary>
        public void SetWriteTimeout(int timeoutMs)
        {
            _writeTimeoutMs = timeoutMs > 0 ? timeoutMs : 5000;
        }

        /// <summary>
        /// 当前会话ID
        /// </summary>
        public string SessionId => _sessionId;

        #endregion

        #region 私有方法

        /// <summary>
        /// 返回相对会话开始的毫秒数，用于日志时序对齐
        /// </summary>
        private string T() => $"[+{(DateTime.UtcNow - _sessionStart).TotalMilliseconds:0}ms]";

        /// <summary>
        /// 检测连接协议类型
        /// </summary>
        private string DetectProtocol(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return "ble";

            // WiFi: 包含 : 和 . 的是 IP:Port 格式
            if (address.Contains(".") && address.Contains(":"))
                return "wifi";

            // 经典蓝牙: MAC 地址格式 XX:XX:XX:XX:XX:XX
            if (address.Length == 17 && address.Split(':').Length == 6)
                return "classic";

            // GUID 格式通常是 BLE
            if (Guid.TryParse(address, out _))
                return "ble";

            // 默认使用经典蓝牙
            return "classic";
        }

        /// <summary>
        /// 处理来自手机B的消息
        /// </summary>
        private void OnBridgeMessageReceived(object sender, WSMessage message)
        {
            // 只处理当前会话的数据
            if (!string.IsNullOrEmpty(message.SessionId) && message.SessionId != _sessionId)
                return;

            if (message.Action == MessageAction.OBDData)
            {
                try
                {
                    byte[] data = null;

                    // 解析 base64 数据
                    if (message.Data is string base64)
                    {
                        data = Convert.FromBase64String(base64);
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

                    if (data != null && data.Length > 0)
                    {
                        // ---- 诊断：追踪数据到达情况和 '>' 终结符 ----
                        _diagDataReceived = true;
                        _diagBytesReceived += data.Length;
                        if (!_diagPromptReceived)
                        {
                            try
                            {
                                var responseText = Encoding.ASCII.GetString(data);
                                if (responseText.IndexOf('>') >= 0)
                                {
                                    _diagPromptReceived = true;
                                    var sinceSend = _diagWriteTime != DateTime.MinValue
                                        ? (DateTime.UtcNow - _diagWriteTime).TotalMilliseconds
                                        : -1;
                                    Log($"[WebSocketBT-DIAG] ✓ 收到'>'终结符 cmd=[{_diagCmdText}] 距发送={sinceSend:0}ms");
                                }
                            }
                            catch { }
                        }
                        // ---- 诊断结束 ----

                        // 入队前先解码预览，方便日志直观显示 ELM327 响应内容
                        string preview = "(二进制)";
                        try { preview = Encoding.ASCII.GetString(data).Replace("\r", "\\r").Replace("\n", "\\n"); } catch { }
                        if (preview.Length > 100) preview = preview.Substring(0, 100) + "...";
                        Log($"[WebSocketBT] {T()} ← ELM327→B→A (len={data.Length}): [{preview}]");
                        // 放入接收缓冲区（OBDDataReader 轮询 ReadBytesAsync 会取走）
                        _receiveBuffer.Enqueue((data, DateTime.UtcNow));
                        Log($"[WebSocketBT] {T()} ← 数据已入队 (队列≈{_receiveBuffer.Count}, 总W={_writeSeq} 总R={_readSeq})");
                    }
                }
                catch (Exception ex)
                {
                    Log($"[WebSocketBT] 解析数据失败: {ex.Message}");
                }
            }
            else if (message.Action == MessageAction.ConnectionLost)
            {
                Log($"[WebSocketBT] 连接丢失");
                _isConnected = false;
                _sessionId = null;
            }
        }

        /// <summary>
        /// 记录接收到的数据（调试用）
        /// </summary>
        private void LogReceivedData(byte[] data)
        {
            if (data == null || data.Length == 0)
                return;

            try
            {
                var text = Encoding.ASCII.GetString(data).Replace("\r", "\\r").Replace("\n", "\\n");
                Log($"[WebSocketBT] {T()} 响应内容: [{Truncate(text, 160)}] (len={data.Length})");
            }
            catch { }
        }

        private static string Truncate(string text, int maxLen)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLen) return text ?? string.Empty;
            return text.Substring(0, maxLen);
        }

        #endregion

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _bridge.MessageReceived -= OnBridgeMessageReceived;
        }
    }
}
