using System;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// 车辆诊断云端管理器 - 统一管理WebSocket连接、设备发现和 ELM327 通信
    /// 这是云端A使用的主入口类，替换原始蓝牙管理逻辑
    /// </summary>
    public class OBDCloudManager : IDisposable
    {
        #region Android 日志输出

        private static System.Reflection.MethodInfo _androidLogMethod;
        private static bool _androidLogInitialized;

        /// <summary>
        /// 输出日志到 Android logcat（通过反射调用 Android.Util.Log.Info）
        /// </summary>
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
                            // 立即测试日志输出
                            if (_androidLogMethod != null)
                            {
                                _androidLogMethod.Invoke(null, new object[] { "OBDCloudManager", "[OBDCloudManager] 日志系统初始化成功" });
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[OBDCloudManager] 日志初始化异常: {ex.Message}");
                }
            }

            if (_androidLogMethod != null)
            {
                try
                {
                    _androidLogMethod.Invoke(null, new object[] { "OBDCloudManager", message });
                }
                catch { }
            }
        }

        /// <summary>
        /// 静态构造函数 - 在类首次使用时初始化日志
        /// </summary>
        static OBDCloudManager()
        {
            Log("[OBDCloudManager] 静态构造函数被调用");
        }

        #endregion

        #region 单例模式（供 DLL 注入使用）

        private static OBDCloudManager _instance;
        private static readonly object _instanceLock = new object();
        private static bool _initializationStarted;
        private static string _pendingServerUrl = "ws://0.0.0.0:8080/ws";  // 默认监听地址

        /// <summary>
        /// 设置服务器URL（在首次访问 Instance 之前调用）
        /// </summary>
        public static void SetServerUrl(string url)
        {
            _pendingServerUrl = url;
        }

        /// <summary>
        /// 全局单例实例（懒加载，首次访问时同步初始化）
        /// </summary>
        public static OBDCloudManager Instance
        {
            get
            {
                Log("[OBDCloudManager] Instance getter 被访问");

                // 快速路径：如果已初始化，直接返回
                if (_instance != null && _instance._bridge?.State == WebSocketState.Open)
                {
                    Log("[OBDCloudManager] 返回已初始化的实例");
                    return _instance;
                }

                lock (_instanceLock)
                {
                    // 双重检查
                    if (_instance != null && _instance._bridge?.State == WebSocketState.Open)
                        return _instance;

                    if (!_initializationStarted)
                    {
                        _initializationStarted = true;
                        Log("[OBDCloudManager] 开始初始化...");
                        try
                        {
                            Log("[OBDCloudManager] 准备创建 OBDCloudManager 实例...");
                            _instance = new OBDCloudManager();
                            Log("[OBDCloudManager] OBDCloudManager 实例创建成功");
                        }
                        catch (Exception ex)
                        {
                            Log($"[OBDCloudManager] 创建实例失败: {ex.GetType().Name}: {ex.Message}");
                            Log($"[OBDCloudManager] 堆栈: {ex.StackTrace}");
                            _initializationStarted = false;
                            throw;
                        }
                        try
                        {
                            // 同步阻塞等待连接完成
                            Log($"[OBDCloudManager] 启动服务器监听 {_pendingServerUrl}");
                            _instance.InitializeAsync(_pendingServerUrl).GetAwaiter().GetResult();
                            Log($"[OBDCloudManager] 服务器已启动 {_pendingServerUrl}");
                        }
                        catch (Exception ex)
                        {
                            Log($"[OBDCloudManager] 启动失败: {ex.Message}");
                            _initializationStarted = false;  // 允许重试
                            _instance = null;
                            throw;
                        }
                    }
                    else
                    {
                        Log("[OBDCloudManager] 初始化正在进行中...");
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// 是否已初始化
        /// </summary>
        // 仅表示服务器已开始监听（不要求已有客户端连接）
        public static bool IsInitialized => _instance != null && _instance._bridge?.State == WebSocketState.Open;

        /// <summary>
        /// 是否已有客户端连接
        /// </summary>
        public static bool IsClientConnected => _instance != null && _instance._bridge?.IsConnected == true;

        /// <summary>
        /// 初始化单例（如果尚未初始化）- 异步版本
        /// </summary>
        /// <param name="serverUrl">WebSocket服务器地址</param>
        public static async Task InitializeIfNeededAsync(string serverUrl)
        {
            _pendingServerUrl = serverUrl;  // 保存 URL

            if (_instance != null && _instance._bridge?.State == WebSocketState.Open)
                return;

            lock (_instanceLock)
            {
                if (_instance == null && !_initializationStarted)
                {
                    _initializationStarted = true;
                    _instance = new OBDCloudManager();
                }
            }

            if (_instance != null && !(_instance._bridge?.State == WebSocketState.Open))
            {
                try
                {
                    await _instance.InitializeAsync(serverUrl).ConfigureAwait(false);
                    Log($"[OBDCloudManager] 服务器已启动 {serverUrl}");
                }
                catch (Exception ex)
                {
                    Log($"[OBDCloudManager] 启动失败: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// 设备发现回调（用于转发到 AndroidBluetooth2Manager）
        /// </summary>
        public static Action<string, string, int> OnDeviceFoundCallback;

        #endregion

        private IWebSocketBridge _bridge;
        private WebSocketDeviceDiscovery _discovery;
        private WebSocketOBDConnection _obdConnection;
        private WebSocketBluetoothConnection _bluetoothConnection;
        private WebSocketUIBridge _uiBridge;
        private string _serverUrl;
        private bool _disposed;
        private readonly object _uiBridgeLock = new object();
        private readonly ManualResetEventSlim _uiBridgeReady = new ManualResetEventSlim(false);
        private const int UiBridgeWaitMs = 5000;
        private readonly object _obdReaderEventLock = new object();
        private Delegate _obdStatusChangedDelegate;
        private object _obdReaderInstance;
        private Type _cachedObdReaderType;
        private Type _cachedAppType;

        /// <summary>
        /// 设备发现服务
        /// </summary>
        public WebSocketDeviceDiscovery Discovery => _discovery;

        /// <summary>
        /// ELM327 连接管理（旧接口，保留兼容）
        /// </summary>
        public WebSocketOBDConnection OBDConnection => _obdConnection;

        /// <summary>
        /// ELM327 蓝牙连接（实现 IOBDConnection 接口，供原软件使用）
        /// 用于替换原始 BluetoothConnectionV3，使所有AT命令通过WebSocket转发
        /// </summary>
        public WebSocketBluetoothConnection BluetoothConnection => _bluetoothConnection;

        /// <summary>
        /// WebSocket底层桥接
        /// </summary>
        public IWebSocketBridge Bridge => _bridge;

        /// <summary>
        /// WebSocket连接状态变更事件
        /// </summary>
        public event EventHandler<WebSocketState> ConnectionStateChanged;

        /// <summary>
        /// 发现设备事件（转发自 Discovery）
        /// </summary>
        public event EventHandler<BTDeviceInfo> DeviceDiscovered;

        /// <summary>
        /// 诊断数据接收事件（转发自 OBDConnection）
        /// </summary>
        public event EventHandler<byte[]> OBDDataReceived;

        /// <summary>
        /// ELM327 连接丢失事件（转发自 OBDConnection）
        /// </summary>
        public event EventHandler<string> OBDConnectionLost;

        public OBDCloudManager()
        {
            System.Diagnostics.Debug.WriteLine("[OBDCloudManager] >>> 构造函数入口");
            Console.WriteLine("[OBDCloudManager] >>> 构造函数入口 (Console)");
            Log("[OBDCloudManager] 构造函数开始");
            try
            {
                System.Diagnostics.Debug.WriteLine("[OBDCloudManager] >>> 准备创建 WebSocketServerBridge");
                _bridge = new WebSocketServerBridge(responseTimeoutMs: 30000);
                Log("[OBDCloudManager] WebSocketServerBridge 创建成功");
                _discovery = new WebSocketDeviceDiscovery(_bridge);
                Log("[OBDCloudManager] WebSocketDeviceDiscovery 创建成功");
                _obdConnection = new WebSocketOBDConnection(_bridge);
                Log("[OBDCloudManager] WebSocketOBDConnection 创建成功");
                _bluetoothConnection = new WebSocketBluetoothConnection(_bridge);
                Log("[OBDCloudManager] WebSocketBluetoothConnection 创建成功");

            // 转发事件
            _bridge.StateChanged += (s, state) => ConnectionStateChanged?.Invoke(this, state);
            // 注意：设备发现事件不再转发给 JS，B 端会自己处理蓝牙扫描
            // A 端（云端）不需要知道 B 端扫描到了哪些设备
            _discovery.DeviceDiscovered += (s, device) =>
            {
                DeviceDiscovered?.Invoke(this, device);
                OnDeviceFoundCallback?.Invoke(device.Name, device.Address, device.Rssi);
                // 静默处理，不再输出日志（减少噪音）
            };
            _obdConnection.DataReceived += (s, data) => OBDDataReceived?.Invoke(this, data);
            _obdConnection.ConnectionLost += (s, reason) =>
            {
                OBDConnectionLost?.Invoke(this, reason);
                Log($"[OBDCloudManager] OBD connection lost: {reason}");
                try
                {
                    _ = _bridge.SendAsync(WSMessageFactory.CreateOBDStatusChangedEvent("Disconnected"));
                }
                catch (Exception ex)
                {
                    Log($"[OBDCloudManager] Send OBDStatusChanged failed: {ex.Message}");
                }
            };

            _bridge.MessageReceived += OnBridgeMessageReceived;
            Log("[OBDCloudManager] 构造函数完成");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 构造函数异常: {ex.Message}");
                Log($"[OBDCloudManager] 堆栈: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// 捕获 UI 侧 JS 回调（从 MainWebView.InvokeAsync 注入调用）
        /// </summary>
        public static void HandleUiInvoke(string methodName, object[] args)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                return;

            var mgr = _instance;
            if (mgr == null || mgr._bridge == null || !mgr._bridge.IsConnected)
                return;

            mgr.HandleUiInvokeInternal(methodName, args);
        }

        /// <summary>
        /// 初始化并连接到网关B
        /// </summary>
        /// <param name="serverUrl">WebSocket服务器地址, 例如 "ws://192.168.1.100:8080/ws"</param>
        public async Task InitializeAsync(string serverUrl)
        {
            _serverUrl = serverUrl;
            await _bridge.ConnectAsync(serverUrl).ConfigureAwait(false);
            Log($"[OBDCloudManager] WebSocket服务器已启动 {serverUrl}");
        }

        /// <summary>
        /// 通知 UI 连接状态变化
        /// </summary>
        private void NotifyUIStatusChanged(string status)
        {
            try
            {
                var jsScript = $"if(window.onOBDStatusChanged){{window.onOBDStatusChanged('{status}')}}";
                Log($"[OBDCloudManager] 通知 UI 状态: {status}");
                WebSocketUIBridge.HandleOutgoingJS(jsScript);
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 通知 UI 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 注册JSBridge实例，启用UI桥接通道
        /// 同时预配置 WebSocket 连接，使原软件的 Connect() 方法使用 WebSocket
        /// </summary>
        public void RegisterJSBridge(object jsBridge)
        {
            if (jsBridge == null) throw new ArgumentNullException(nameof(jsBridge));

            Log("[OBDCloudManager] ========================================");
            Log("[OBDCloudManager] RegisterJSBridge 被调用");

            lock (_uiBridgeLock)
            {
                _uiBridge = new WebSocketUIBridge(jsBridge);
                _uiBridgeReady.Set();
            }

            WebSocketUIBridge.OutgoingMessage -= OnUiOutgoingMessage;
            WebSocketUIBridge.OutgoingMessage += OnUiOutgoingMessage;

            _bridge.RawMessageReceived -= OnRawMessageReceived;
            _bridge.RawMessageReceived += OnRawMessageReceived;

            // 关键：在注册 JSBridge 时就预配置 WebSocket 连接
            // 这样当原软件调用 connectAsync 时，WebSocket 连接就已经准备好了
            Log("[OBDCloudManager] 预配置 OBDDataReader WebSocket 连接...");
            ConfigureOBDDataReaderForWebSocket();

            Log("[OBDCloudManager] RegisterJSBridge 完成");
            Log("[OBDCloudManager] ========================================");
        }

        /// <summary>
        /// 开始扫描 ELM327 设备
        /// 替代: JSBridge.startBTScan() → AndroidBluetooth2Manager.StartDiscoveringDevices()
        /// </summary>
        public async Task StartScanAsync()
        {
            await _discovery.StartScanAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 停止扫描
        /// 替代: JSBridge.stopBTScan() → AndroidBluetooth2Manager.StopDiscoveringDevices()
        /// </summary>
        public async Task StopScanAsync()
        {
            await _discovery.StopScanAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 连接 ELM327 设备
        /// 替代: JSBridge.connectAsync(type, address) → BluetoothConnectionV3.ConnectAsync()
        /// </summary>
        public async Task<bool> ConnectToDeviceAsync(string protocol, string address)
        {
            return await _obdConnection.ConnectAsync(protocol, address).ConfigureAwait(false);
        }

        /// <summary>
        /// 断开 ELM327 连接
        /// 替代: JSBridge.disconnectAsync()
        /// </summary>
        public async Task DisconnectAsync()
        {
            await _obdConnection.DisconnectAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 发送诊断命令
        /// 替代: DebugStream.WriteAsync() / ICharacteristic.WriteAsync()
        /// </summary>
        public async Task SendOBDCommandAsync(byte[] data)
        {
            await _obdConnection.SendAsync(data).ConfigureAwait(false);
        }

        /// <summary>
        /// 发送诊断文本命令
        /// </summary>
        public async Task SendOBDCommandAsync(string command)
        {
            await _obdConnection.SendCommandAsync(command).ConfigureAwait(false);
        }

        /// <summary>
        /// 关闭所有连接
        /// </summary>
        public async Task ShutdownAsync()
        {
            if (_obdConnection.IsConnected)
            {
                await _obdConnection.DisconnectAsync().ConfigureAwait(false);
            }
            await _bridge.CloseAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            WebSocketUIBridge.OutgoingMessage -= OnUiOutgoingMessage;
            _bridge.RawMessageReceived -= OnRawMessageReceived;
            _bridge.MessageReceived -= OnBridgeMessageReceived;
            UnsubscribeObdReaderStatusChanged();

            _bluetoothConnection?.Dispose();
            _obdConnection?.Dispose();
            _discovery?.Dispose();
            _bridge?.Dispose();
        }

        private void OnRawMessageReceived(object sender, string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            if (_uiBridge == null)
            {
                Log("[OBDCloudManager] ⚠ 收到 UI 命令但 JSBridge 未注册");
                return;
            }

            try
            {
                var obj = JObject.Parse(json);
                var type = obj.Value<string>("type");
                if (!string.Equals(type, "ui_command", StringComparison.OrdinalIgnoreCase))
                    return;

                var method = obj.Value<string>("method") ?? "-";
                Log($"[OBDCloudManager] UI命令: {method}");
            }
            catch (JsonException)
            {
                return;
            }

            lock (_uiBridgeLock)
            {
                _uiBridge?.HandleIncomingMessage(json);
            }
        }

        private async void OnUiOutgoingMessage(string json)
        {
            // 诊断：记录入口（含连接状态）
            var preview2 = (json?.Length ?? 0) > 100 ? json.Substring(0, 100) : (json ?? "null");
            Console.WriteLine($"[OnUiOutgoingMessage] json.len={json?.Length ?? 0} connected={_bridge?.IsConnected} preview={preview2}");
            if (string.IsNullOrWhiteSpace(json) || _bridge == null || !_bridge.IsConnected)
                return;

            try
            {
                var obj = JObject.Parse(json);
                var type = obj.GetValue("type", StringComparison.OrdinalIgnoreCase)?.Value<string>();
                if (string.Equals(type, "ui_event", StringComparison.OrdinalIgnoreCase))
                {
                    var eventName = obj.GetValue("event", StringComparison.OrdinalIgnoreCase)?.Value<string>();
                    if (!string.IsNullOrWhiteSpace(eventName))
                    {
                        Log($"[OBDCloudManager] UI事件: {eventName}");
                        var dataToken = obj.GetValue("data", StringComparison.OrdinalIgnoreCase);
                        HandleUiInvokeInternal(eventName, dataToken == null ? null : new object[] { dataToken });
                    }
                    return;
                }
            }
            catch (JsonException)
            {
                // ignore and fallback to raw forwarding
            }

            try
            {
                await _bridge.SendRawAsync(json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log($"发送UI事件失败: {ex.Message}");
            }
        }

        private void HandleUiInvokeInternal(string methodName, object[] args)
        {
            // 诊断：记录所有 UI 回调事件名
            Log($"[OBDCloudManager] UI回调: {methodName} args={args?.Length ?? 0}");
            // 只处理关心的 UI 回调，避免噪音
            switch (methodName)
            {
                case "onOBDStatusChanged":
                    SendEvent(MessageAction.OBDStatusChanged, new { status = ToStringArg(args, 0) });
                    break;
                case "onPIDValueChanged":
                    SendEvent(MessageAction.PIDValueChanged, NormalizeArg(args, 0));
                    break;

                case "onReadDTCSuccess":
                    var dtcRaw = args != null && args.Length > 0 ? args[0] : null;
                    var dtcRawStr = dtcRaw?.ToString() ?? "";
                    SendEvent(MessageAction.DTCResult, ParseJsonArg(dtcRawStr));
                    break;
                case "onReadDTCError":
                    SendError("DTC_ERROR", ToStringArg(args, 0));
                    break;

                case "onClearDTCSuccess":
                    SendEvent(MessageAction.DTCCleared, NormalizeArg(args, 0));
                    break;
                case "onClearDTCError":
                    SendError("CLEAR_DTC_ERROR", ToStringArg(args, 0));
                    break;

                case "onReadECUInfoSuccess":
                    SendEvent(MessageAction.ECUInfoResult, NormalizeArg(args, 0));
                    break;
                case "onReadECUInfoError":
                    SendError("ECU_INFO_ERROR", ToStringArg(args, 0));
                    break;

                case "onReadFreezeFrameSuccess":
                    SendEvent(MessageAction.FreezeFrameResult, NormalizeArg(args, 0));
                    break;
                case "onReadFreezeFrameError":
                    SendError("FREEZE_FRAME_ERROR", ToStringArg(args, 0));
                    break;
            }
        }

        private static string ToStringArg(object[] args, int index)
        {
            if (args == null || args.Length <= index || args[index] == null)
                return string.Empty;
            return args[index].ToString();
        }

        /// <summary>
        /// 将 CarScanner WebView 传出的字符串解析为 JToken。
        /// CarScanner 对 DTC 等 JSON 结果做了双重转义：原本 {"Code":"X"} 变成
        /// {\"Code\":\"X\"} 作为字符串字面量传出。直接 JToken.Parse 会失败，
        /// 需要先把字面量反斜杠引号 \" 还原为 " 再解析。
        /// </summary>
        private static object ParseJsonArg(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return raw;
            var trimmed = raw.Trim();
            if (!trimmed.StartsWith("[") && !trimmed.StartsWith("{"))
                return raw;

            // 第一次尝试：直接解析（数据已是正常 JSON）
            try { return JToken.Parse(trimmed); } catch { }

            // 第二次尝试：去除 CarScanner 的字面量 \" 转义后再解析
            var unescaped = trimmed.Replace("\\\"", "\"");
            try { return JToken.Parse(unescaped); } catch { }

            return raw;
        }

        private static object NormalizeArg(object[] args, int index)
        {
            if (args == null || args.Length <= index)
                return null;
            var arg = args[index];
            if (arg == null)
                return null;

            // 已经是结构化 JSON，直接返回
            if (arg is JArray || arg is JObject)
                return arg;

            // 提取字符串表示：C# string、JValue（任意子类型）均支持
            string strValue = null;
            if (arg is string s)
            {
                strValue = s;
            }
            else if (arg is JValue jv)
            {
                // JTokenType.String 用 Value<string>() 取原始内容，避免多余引号
                // 其他类型（Raw、Integer 等）用 ToString(None) 取 JSON 文本表示
                strValue = jv.Type == JTokenType.String
                    ? jv.Value<string>()
                    : jv.ToString(Formatting.None);
            }
            else
            {
                strValue = arg.ToString();
            }

            if (!string.IsNullOrEmpty(strValue))
            {
                var trimmed = strValue.Trim();
                if ((trimmed.StartsWith("{") && trimmed.EndsWith("}")) ||
                    (trimmed.StartsWith("[") && trimmed.EndsWith("]")))
                {
                    try { return JToken.Parse(trimmed); } catch { }
                }
                return strValue;
            }
            return arg;
        }

        private void SendEvent(string action, object data)
        {
            var connected = _bridge?.IsConnected == true;
            Log($"[OBDCloudManager] SendEvent action={action} connected={connected}");
            if (_bridge == null || !connected)
                return;

            var message = new WSMessage
            {
                Type = MessageType.Event,
                Action = action,
                Data = data
            };

            try
            {
                // 诊断：记录即将发送的 JSON
                var json = message.ToJson();
                var preview = json.Length > 150 ? json.Substring(0, 150) + "..." : json;
                Log($"[OBDCloudManager] TX event action={action} json.len={json.Length} preview={preview}");

                var task = _bridge.SendAsync(message);
                task.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        Log($"[OBDCloudManager] SendEvent {action} 发送失败: {t.Exception?.GetBaseException()?.Message}");
                    else
                        Log($"[OBDCloudManager] SendEvent {action} 发送完成");
                });
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] SendEvent {action} 同步异常: {ex.Message}");
            }
        }

        private void SendError(string code, string message)
        {
            SendEvent(MessageAction.Error, new ErrorData { Code = code, Message = message ?? string.Empty });
        }

        private async void OnBridgeMessageReceived(object sender, WSMessage message)
        {
            if (message == null || message.Type != MessageType.Request)
                return;

            Log($"[OBDCloudManager] RX request action={message.Action} req={message.RequestId ?? "-"} session={message.SessionId ?? "-"}");

            // 仅处理来自B端的业务请求
            switch (message.Action)
            {
                case MessageAction.OBDConnect:
                    await HandleObdConnectAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.OBDDisconnect:
                    await HandleObdDisconnectAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.GetBrands:
                    await HandleInvokeStringResultAsync(message, "getBrands").ConfigureAwait(false);
                    break;
                case MessageAction.GetProfiles:
                    await HandleGetProfilesAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.ApplyProfile:
                    await HandleApplyProfileAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.GetECUList:
                    await HandleInvokeStringResultAsync(message, "getECUList").ConfigureAwait(false);
                    break;
                case MessageAction.ReadECUInfo:
                    await HandleInvokeAsyncCommand(message, "readECUInfoAsync", "indices").ConfigureAwait(false);
                    break;
                case MessageAction.ReadDTC:
                    await HandleInvokeAsyncCommand(message, "readDTCAsync", "indices").ConfigureAwait(false);
                    break;
                case MessageAction.ClearDTC:
                    await HandleInvokeAsyncCommand(message, "clearDTCAsync", "indices").ConfigureAwait(false);
                    break;
                case MessageAction.ReadFreezeFrame:
                    await HandleReadFreezeFrameAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.GetPIDList:
                    await HandleInvokeStringResultAsync(message, "getPIDList").ConfigureAwait(false);
                    break;
                case MessageAction.StartReadPIDs:
                    await HandleStartReadPidsAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.StopReadPIDs:
                    await HandleInvokeVoidResultAsync(message, "stopReadPIDs").ConfigureAwait(false);
                    break;
            }
        }

        private async Task HandleObdConnectAsync(WSMessage message)
        {
            try
            {
                var data = message.GetData<OBDConnectRequestData>();
                var sessionId = data?.SessionId;
                var protocol = data?.Protocol ?? "BT";
                var address = data?.Address ?? "WEBSOCKET";
                var sessionProvided = !string.IsNullOrWhiteSpace(sessionId);

                Log($"[OBDCloudManager] HandleObdConnectAsync: protocol={protocol}, address={address}, sessionId={sessionId ?? "(空)"}");

                // 如果 B 端没有提供 sessionId，说明 B 端还没有连接蓝牙
                // 我们需要发送 Connect 请求让 B 端连接蓝牙
                if (!sessionProvided)
                {
                    Log("[OBDCloudManager] B 端未提供 sessionId，发送 Connect 请求让 B 端连接蓝牙...");

                    try
                    {
                        // 发送 Connect 请求到 B 端
                        var connectRequest = WSMessageFactory.CreateConnectRequest(protocol, address);
                        var response = await _bridge.SendAndWaitAsync(connectRequest).ConfigureAwait(false);

                        if (response == null || response.Success != true)
                        {
                            var errorMsg = response?.Error ?? "未知错误";
                            Log($"[OBDCloudManager] B 端蓝牙连接失败：{errorMsg}");
                            await SendResponseAsync(message, false, null, $"B 端蓝牙连接失败: {errorMsg}").ConfigureAwait(false);
                            return;
                        }

                        // 从响应中获取 sessionId
                        sessionId = response.SessionId;
                        if (string.IsNullOrWhiteSpace(sessionId))
                        {
                            // 尝试从 Data 中获取
                            var connectResult = response.GetData<ConnectResult>();
                            sessionId = connectResult?.SessionId;
                        }

                        if (string.IsNullOrWhiteSpace(sessionId))
                        {
                            Log("[OBDCloudManager] B 端蓝牙连接失败：未返回 sessionId");
                            await SendResponseAsync(message, false, null, "B 端蓝牙连接失败：未返回 sessionId").ConfigureAwait(false);
                            return;
                        }

                        Log($"[OBDCloudManager] B 端蓝牙连接成功，sessionId: {sessionId}");
                    }
                    catch (TimeoutException)
                    {
                        Log("[OBDCloudManager] B 端蓝牙连接超时");
                        await SendResponseAsync(message, false, null, "B 端蓝牙连接超时").ConfigureAwait(false);
                        return;
                    }
                    catch (Exception connEx)
                    {
                        Log($"[OBDCloudManager] B 端蓝牙连接异常: {connEx.Message}");
                        await SendResponseAsync(message, false, null, $"B 端蓝牙连接失败: {connEx.Message}").ConfigureAwait(false);
                        return;
                    }
                }

                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    Log("[OBDCloudManager] sessionId 为空，无法继续初始化");
                    await SendResponseAsync(message, false, null, "sessionId 为空").ConfigureAwait(false);
                    return;
                }

                // 绑定 sessionId 到 WebSocketBluetoothConnection
                _bluetoothConnection.BindSession(sessionId);
                _obdConnection.BindSession(sessionId);
                Log($"[OBDCloudManager] 绑定 sessionId: {sessionId}");

                // 配置 OBDDataReader 的 WebSocket 连接
                ConfigureOBDDataReaderForWebSocket();

                // 先返回响应，避免 B 端等待超时（初始化异步执行）
                await SendResponseAsync(message, true, new { connecting = true, sessionId }).ConfigureAwait(false);

                var initAddress = address;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 触发原 CarScanner 的 OBDDataReader.Initialize 流程（完全使用原逻辑）
                        var initOk = await TryInitializeObdReaderAsync(initAddress).ConfigureAwait(false);
                        Log($"[OBDCloudManager] OBDDataReader.Initialize 触发完成 (ok={initOk})");

                        if (!initOk)
                        {
                            await _bridge.SendAsync(WSMessageFactory.CreateOBDStatusChangedEvent("Disconnected"))
                                .ConfigureAwait(false);
                        }
                    }
                    catch (Exception initEx)
                    {
                        Log($"[OBDCloudManager] OBD 初始化异常: {initEx.Message}");
                        await _bridge.SendAsync(WSMessageFactory.CreateOBDStatusChangedEvent("Disconnected"))
                            .ConfigureAwait(false);
                    }
                });
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] HandleObdConnectAsync 异常: {ex.Message}");
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 通过反射配置 OBDDataReader 使用 WebSocket 连接
        /// DllPatcher 已经在 OBDDataReader 中添加了以下静态字段：
        /// - UseWebSocketConnection (bool): 标记是否使用 WebSocket
        /// - WebSocketConnection (object): 存储 WebSocket 连接实例
        /// </summary>
        private void ConfigureOBDDataReaderForWebSocket()
        {
            Log("[OBDCloudManager] ConfigureOBDDataReaderForWebSocket 开始");

            try
            {
                // 列出所有已加载的程序集（调试用）
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Log($"[OBDCloudManager] 已加载 {assemblies.Length} 个程序集");

                // 查找 OBDDataReader 类型
                Type obdDataReaderType = null;
                string foundInAssembly = null;

                foreach (var assembly in assemblies)
                {
                    try
                    {
                        // 只检查可能包含 OBDDataReader 的程序集
                        var asmName = assembly.GetName().Name;
                        if (asmName.Contains("CarScanner") || asmName.Contains("CarDemo"))
                        {
                            Log($"[OBDCloudManager] 检查程序集: {asmName}");
                        }

                        obdDataReaderType = assembly.GetType("CarScannerXamarinForms.OBD2.OBDDataReader");
                        if (obdDataReaderType != null)
                        {
                            foundInAssembly = assembly.GetName().Name;
                            break;
                        }
                    }
                    catch { }
                }

                if (obdDataReaderType == null)
                {
                    Log("[OBDCloudManager] ✗ 未找到 OBDDataReader 类型");
                    Log("[OBDCloudManager] 提示: 检查 CarScannerXamarinForms.dll 是否已加载");
                    return;
                }

                Log($"[OBDCloudManager] ✓ 找到 OBDDataReader，在程序集: {foundInAssembly}");

                // 列出 OBDDataReader 的所有静态字段
                var staticFields = obdDataReaderType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                Log($"[OBDCloudManager] OBDDataReader 静态字段数: {staticFields.Length}");
                foreach (var field in staticFields)
                {
                    Log($"[OBDCloudManager]   - {field.Name}: {field.FieldType.Name}");
                }

                // 设置 UseWebSocketConnection = true
                var useWsField = obdDataReaderType.GetField("UseWebSocketConnection",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (useWsField != null)
                {
                    useWsField.SetValue(null, true);
                    Log("[OBDCloudManager] ✓ 设置 UseWebSocketConnection = true");
                }
                else
                {
                    Log("[OBDCloudManager] ✗ 未找到 UseWebSocketConnection 字段（DLL可能未被patch）");
                }

                // 设置 WebSocketConnection = WebSocketIOBDConnectionProxy(_bluetoothConnection)
                // 使用代理类包装 _bluetoothConnection，解决 Task<byte[]> vs ValueTask<byte[]> 类型不匹配问题
                var wsConnField = obdDataReaderType.GetField("WebSocketConnection",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (wsConnField != null)
                {
                    // 先尝试显式加载 WebSocketIOBDConnectionProxy 程序集
                    System.Reflection.Assembly proxyAssembly = null;
                    try
                    {
                        Log("[OBDCloudManager] 尝试加载 WebSocketIOBDConnectionProxy 程序集...");
                        proxyAssembly = System.Reflection.Assembly.Load("WebSocketIOBDConnectionProxy");
                        Log($"[OBDCloudManager] ✓ 成功加载程序集: {proxyAssembly.GetName().Name}");
                    }
                    catch (Exception loadEx)
                    {
                        Log($"[OBDCloudManager] ⚠ 加载程序集失败: {loadEx.Message}");
                    }

                    // 查找 WebSocketIOBDConnectionProxy 类型
                    Type proxyType = null;

                    // 先从刚加载的程序集中查找
                    if (proxyAssembly != null)
                    {
                        try
                        {
                            proxyType = proxyAssembly.GetType("OBDCloud.WebSocket.WebSocketIOBDConnectionProxy");
                            if (proxyType != null)
                            {
                                Log($"[OBDCloudManager] ✓ 在显式加载的程序集中找到 proxy 类型");
                            }
                        }
                        catch { }
                    }

                    // 如果还没找到，遍历所有已加载的程序集
                    if (proxyType == null)
                    {
                        foreach (var asm in assemblies)
                        {
                            try
                            {
                                proxyType = asm.GetType("OBDCloud.WebSocket.WebSocketIOBDConnectionProxy");
                                if (proxyType != null) break;
                            }
                            catch { }
                        }
                    }

                    if (proxyType != null)
                    {
                        // 创建代理实例，包装 _bluetoothConnection
                        var proxy = Activator.CreateInstance(proxyType, new object[] { _bluetoothConnection });
                        wsConnField.SetValue(null, proxy);
                        Log("[OBDCloudManager] ✓ 设置 WebSocketConnection = WebSocketIOBDConnectionProxy(BluetoothConnection)");
                    }
                    else
                    {
                        // 如果找不到代理类，尝试直接设置（可能导致运行时错误）
                        Log("[OBDCloudManager] ⚠ 未找到 WebSocketIOBDConnectionProxy，尝试直接设置");
                        wsConnField.SetValue(null, _bluetoothConnection);
                        Log("[OBDCloudManager] ✓ 设置 WebSocketConnection = BluetoothConnection (直接)");
                    }
                }
                else
                {
                    Log("[OBDCloudManager] ✗ 未找到 WebSocketConnection 字段（DLL可能未被patch）");
                }

                Log("[OBDCloudManager] ConfigureOBDDataReaderForWebSocket 完成");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] ✗ 配置 OBDDataReader 失败: {ex.Message}");
                Log($"[OBDCloudManager] 堆栈: {ex.StackTrace}");
            }
        }

        private async Task<bool> TryInitializeObdReaderAsync(string deviceAddress)
        {
            var initTs = DateTime.UtcNow;
            string InitElapsed() => $"[INIT +{(DateTime.UtcNow - initTs).TotalMilliseconds:0}ms]";

            try
            {
                Log($"[OBDCloudManager] {InitElapsed()} ▶ TryInitializeObdReaderAsync 开始 device={deviceAddress}");
                ConfigureOBDDataReaderForWebSocket();
                Log($"[OBDCloudManager] {InitElapsed()} ① WebSocket连接配置完成");
                TryConfigureSharedSettingsForWebSocket(deviceAddress);
                Log($"[OBDCloudManager] {InitElapsed()} ② SharedSettings配置完成");

                if (!TryResolveObdReader(out var obdReader, out var obdReaderType))
                {
                    Log($"[OBDCloudManager] {InitElapsed()} ✗ 无法解析 OBDDataReader，跳过 Initialize");
                    return false;
                }

                Log($"[OBDCloudManager] {InitElapsed()} ③ OBDDataReader实例已获取: {obdReader?.GetType().FullName ?? "null"}");
                EnsureObdReaderStatusSubscription(obdReader, obdReaderType);
                Log($"[OBDCloudManager] {InitElapsed()} ④ StatusChanged事件订阅完成");

                var initModesType = obdReaderType.GetNestedType("InitModes",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (initModesType == null)
                {
                    Log($"[OBDCloudManager] {InitElapsed()} ✗ 未找到 OBDDataReader.InitModes");
                    return false;
                }

                Log($"[OBDCloudManager] {InitElapsed()} ⑤ 查找 Initialize 方法...");
                var initMethod = obdReaderType.GetMethod("Initialize",
                    new[] { typeof(int), initModesType, typeof(IProgress<string>) });
                if (initMethod == null)
                {
                    Log($"[OBDCloudManager] {InitElapsed()} ✗ 未找到 OBDDataReader.Initialize(int, InitModes, IProgress<string>)");
                    return false;
                }

                var progress = new Progress<string>(msg =>
                {
                    if (!string.IsNullOrWhiteSpace(msg))
                        Log($"[OBDCloudManager] {InitElapsed()} ⑥-进度: [{msg}]");
                });

                var initMode = Enum.Parse(initModesType, "Default");
                Log($"[OBDCloudManager] {InitElapsed()} ⑥ 调用 OBDDataReader.Initialize(retries=3, mode=Default)...");
                var taskObj = initMethod.Invoke(obdReader, new object[] { 3, initMode, progress });

                if (taskObj is Task<bool> boolTask)
                {
                    var ok = await boolTask.ConfigureAwait(false);
                    Log($"[OBDCloudManager] {InitElapsed()} ⑦ OBDDataReader.Initialize 完成，结果={ok}");
                    return ok;
                }

                if (taskObj is Task task)
                {
                    await task.ConfigureAwait(false);
                    Log($"[OBDCloudManager] {InitElapsed()} ⑦ OBDDataReader.Initialize 完成 (void Task)");
                    return true;
                }

                Log($"[OBDCloudManager] {InitElapsed()} ✗ OBDDataReader.Initialize 未返回 Task");
                return false;
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] {InitElapsed()} ✗ OBDDataReader.Initialize 异常: {ex.GetType().Name}: {ex.Message}");
                Log($"[OBDCloudManager] {InitElapsed()} StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        private void TryConfigureSharedSettingsForWebSocket(string deviceAddress)
        {
            try
            {
                var settingsType = FindTypeByName("CarScannerXamarinForms.Settings.SharedSettings");
                if (settingsType == null)
                {
                    Log("[OBDCloudManager] ⚠ 未找到 SharedSettings 类型");
                    return;
                }

                var currentProp = settingsType.GetProperty("Current",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                var current = currentProp?.GetValue(null);
                if (current == null)
                {
                    Log("[OBDCloudManager] ⚠ SharedSettings.Current 为空");
                    return;
                }

                var connectionTypesType = FindTypeByName("CarScannerXamarinForms.Settings.ConnectionTypes");
                var connectionTypeProp = settingsType.GetProperty("ConnectionType",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (connectionTypeProp != null)
                {
                    var connEnumType = connectionTypesType ?? connectionTypeProp.PropertyType;
                    var bluetoothValue = Enum.Parse(connEnumType, "Bluetooth");
                    connectionTypeProp.SetValue(current, bluetoothValue);
                    Log("[OBDCloudManager] 已设置 SharedSettings.ConnectionType = Bluetooth");
                }

                if (!string.IsNullOrWhiteSpace(deviceAddress) &&
                    !string.Equals(deviceAddress, "WEBSOCKET", StringComparison.OrdinalIgnoreCase))
                {
                    var btIdProp = settingsType.GetProperty("BTDeviceID",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    btIdProp?.SetValue(current, deviceAddress);
                    Log($"[OBDCloudManager] 已设置 SharedSettings.BTDeviceID = {deviceAddress}");
                }
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 配置 SharedSettings 失败: {ex.Message}");
            }
        }

        private void EnsureObdReaderStatusSubscription(object obdReader, Type obdReaderType)
        {
            if (obdReader == null || obdReaderType == null)
                return;

            var eventInfo = obdReaderType.GetEvent("StatusChanged",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (eventInfo == null)
            {
                Log("[OBDCloudManager] ⚠ 未找到 OBDDataReader.StatusChanged 事件");
                return;
            }

            lock (_obdReaderEventLock)
            {
                if (_obdStatusChangedDelegate != null && ReferenceEquals(_obdReaderInstance, obdReader))
                    return;

                if (_obdStatusChangedDelegate != null && _obdReaderInstance != null)
                {
                    try
                    {
                        eventInfo.RemoveEventHandler(_obdReaderInstance, _obdStatusChangedDelegate);
                    }
                    catch { }
                }

                var delegateType = eventInfo.EventHandlerType;
                var invoke = delegateType?.GetMethod("Invoke");
                var paramType = invoke?.GetParameters().Length == 1 ? invoke.GetParameters()[0].ParameterType : null;
                if (paramType == null)
                {
                    Log("[OBDCloudManager] ⚠ StatusChanged 事件签名异常");
                    return;
                }

                var statusParam = Expression.Parameter(paramType, "status");
                var handlerMethod = GetType().GetMethod(nameof(HandleObdReaderStatusChanged),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var body = Expression.Call(Expression.Constant(this), handlerMethod,
                    Expression.Convert(statusParam, typeof(object)));
                var lambda = Expression.Lambda(delegateType, body, statusParam);

                _obdStatusChangedDelegate = lambda.Compile();
                eventInfo.AddEventHandler(obdReader, _obdStatusChangedDelegate);
                _obdReaderInstance = obdReader;
                Log("[OBDCloudManager] ✓ 已订阅 OBDDataReader.StatusChanged");
            }
        }

        private void UnsubscribeObdReaderStatusChanged()
        {
            lock (_obdReaderEventLock)
            {
                if (_obdStatusChangedDelegate == null || _obdReaderInstance == null)
                    return;

                var obdReaderType = _cachedObdReaderType ?? FindObdReaderType();
                if (obdReaderType == null)
                    return;

                var eventInfo = obdReaderType.GetEvent("StatusChanged",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (eventInfo == null)
                    return;

                try
                {
                    eventInfo.RemoveEventHandler(_obdReaderInstance, _obdStatusChangedDelegate);
                    Log("[OBDCloudManager] 已取消订阅 OBDDataReader.StatusChanged");
                }
                catch { }
                finally
                {
                    _obdStatusChangedDelegate = null;
                    _obdReaderInstance = null;
                }
            }
        }

        private void HandleObdReaderStatusChanged(object statusObj)
        {
            var statusText = statusObj?.ToString() ?? "Unknown";
            Log($"[OBDCloudManager] OBDDataReader.StatusChanged -> {statusText}");

            try
            {
                if (_bridge?.IsConnected == true)
                {
                    _ = _bridge.SendAsync(WSMessageFactory.CreateOBDStatusChangedEvent(statusText));
                }
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 转发状态事件失败: {ex.Message}");
            }
        }

        private bool TryResolveObdReader(out object obdReader, out Type obdReaderType)
        {
            obdReader = null;
            obdReaderType = _cachedObdReaderType ?? FindObdReaderType();

            if (obdReaderType == null)
                return false;

            _cachedObdReaderType = obdReaderType;

            var appType = _cachedAppType ?? FindAppType(obdReaderType);
            if (appType == null)
                return false;

            _cachedAppType = appType;

            var readerField = appType.GetField("OBDReader",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (readerField == null)
            {
                Log("[OBDCloudManager] ✗ App.OBDReader 字段不存在");
                return false;
            }

            obdReader = readerField.GetValue(null);
            if (obdReader == null)
            {
                obdReader = Activator.CreateInstance(obdReaderType);
                readerField.SetValue(null, obdReader);
                Log("[OBDCloudManager] ✓ 创建新的 App.OBDReader 实例");
            }

            return true;
        }

        private Type FindObdReaderType()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var type = assembly.GetType("CarScannerXamarinForms.OBD2.OBDDataReader");
                    if (type != null)
                        return type;
                }
                catch { }
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (string.Equals(type.Name, "OBDDataReader", StringComparison.Ordinal) &&
                            type.Namespace != null && type.Namespace.EndsWith(".OBD2", StringComparison.Ordinal))
                        {
                            return type;
                        }
                    }
                }
                catch { }
            }

            return null;
        }

        private Type FindAppType(Type obdReaderType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var type = assembly.GetType("CarScannerXamarinForms.App");
                    if (type != null)
                        return type;
                }
                catch { }
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        var field = type.GetField("OBDReader",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        if (field != null && field.FieldType == obdReaderType)
                            return type;
                    }
                }
                catch { }
            }

            return null;
        }

        private Type FindTypeByName(string fullName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var type = assembly.GetType(fullName);
                    if (type != null)
                        return type;
                }
                catch { }
            }
            return null;
        }

        private async Task HandleObdDisconnectAsync(WSMessage message)
        {
            try
            {
                // 调用原软件的断开连接方法
                if (_uiBridge != null)
                {
                    try
                    {
                        InvokeJsBridge("disconnectAsync");
                        Log("[OBDCloudManager] disconnectAsync 调用成功");
                    }
                    catch (Exception ex)
                    {
                        Log($"[OBDCloudManager] disconnectAsync 调用失败: {ex.Message}");
                    }
                }

                // 重置 WebSocket 连接状态
                ResetOBDDataReaderWebSocket();

                _obdConnection.ResetSession();
                await _bridge.SendAsync(WSMessageFactory.CreateOBDStatusChangedEvent("Disconnected"))
                    .ConfigureAwait(false);
                await SendResponseAsync(message, true, new { disconnected = true }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 重置 OBDDataReader 的 WebSocket 配置
        /// </summary>
        private void ResetOBDDataReaderWebSocket()
        {
            try
            {
                UnsubscribeObdReaderStatusChanged();

                Type obdDataReaderType = null;
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        obdDataReaderType = assembly.GetType("CarScannerXamarinForms.OBD2.OBDDataReader");
                        if (obdDataReaderType != null)
                            break;
                    }
                    catch { }
                }

                if (obdDataReaderType == null)
                    return;

                // 重置 UseWebSocketConnection = false
                var useWsField = obdDataReaderType.GetField("UseWebSocketConnection",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                useWsField?.SetValue(null, false);

                Log("[OBDCloudManager] 重置 OBDDataReader.UseWebSocketConnection = false");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 重置 OBDDataReader 失败: {ex.Message}");
            }
        }

        private async Task HandleGetProfilesAsync(WSMessage message)
        {
            try
            {
                var obj = message.GetData<JObject>();
                var brand = obj?.Value<string>("brand");
                var result = InvokeJsBridge("getProfiles", brand);
                await SendResponseAsync(message, true, result).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        private async Task HandleApplyProfileAsync(WSMessage message)
        {
            try
            {
                var obj = message.GetData<JObject>();
                var brand = obj?.Value<string>("brand");
                var index = obj?.Value<int?>("profileIndex") ?? 0;
                InvokeJsBridge("applyProfile", brand, index.ToString());
                await SendResponseAsync(message, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        private async Task HandleReadFreezeFrameAsync(WSMessage message)
        {
            try
            {
                var obj = message.GetData<JObject>();
                var frameIndex = obj?.Value<int?>("frameIndex") ?? 0;
                InvokeJsBridge("readFreezeFrameAsync", frameIndex.ToString());
                await SendResponseAsync(message, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        private async Task HandleStartReadPidsAsync(WSMessage message)
        {
            try
            {
                var obj = message.GetData<JObject>();
                var indices = obj?["indices"];
                var json = indices != null ? indices.ToString(Formatting.None) : "[]";
                InvokeJsBridge("startReadPIDs", json);
                await SendResponseAsync(message, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        private async Task HandleInvokeStringResultAsync(WSMessage message, string methodName)
        {
            try
            {
                var result = InvokeJsBridge(methodName);
                await SendResponseAsync(message, true, result).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        private async Task HandleInvokeVoidResultAsync(WSMessage message, string methodName)
        {
            try
            {
                InvokeJsBridge(methodName);
                await SendResponseAsync(message, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        private async Task HandleInvokeAsyncCommand(WSMessage message, string methodName, string arrayField)
        {
            try
            {
                var obj = message.GetData<JObject>();
                var indices = obj?[arrayField];
                var json = indices != null ? indices.ToString(Formatting.None) : "[]";
                InvokeJsBridge(methodName, json);
                await SendResponseAsync(message, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        private object InvokeJsBridge(string methodName, params string[] args)
        {
            if (_uiBridge == null)
            {
                // 启动阶段可能尚未注册，等待一小段时间避免误判
                if (!IsOnMainThread())
                {
                    _uiBridgeReady.Wait(UiBridgeWaitMs);
                }
            }

            if (_uiBridge == null)
            {
                Log("[OBDCloudManager] ⚠ JSBridge 未注册（等待超时）");
                throw new InvalidOperationException("JSBridge未注册");
            }

            try
            {
                return InvokeJsBridgeOnMainThread(methodName, args);
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                var inner = tie.InnerException;
                var innerMsg = inner?.Message ?? tie.Message;
                Log($"[OBDCloudManager] JSBridge 调用异常: method={methodName}, inner={inner?.GetType().Name}: {innerMsg}");
                if (inner != null)
                    throw new Exception(innerMsg);
                throw;
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] JSBridge 调用异常: method={methodName}, ex={ex.GetType().Name}: {ex.Message}");
                throw;
            }
        }

        private object InvokeJsBridgeOnMainThread(string methodName, params string[] args)
        {
            if (IsOnMainThread())
            {
                return _uiBridge.InvokeMethod(methodName, args);
            }

            if (!TryBeginInvokeOnMainThread(() => { }))
            {
                // 无法切换线程，直接调用（可能抛异常）
                return _uiBridge.InvokeMethod(methodName, args);
            }

            var tcs = new TaskCompletionSource<object>();
            TryBeginInvokeOnMainThread(() =>
            {
                try
                {
                    var result = _uiBridge.InvokeMethod(methodName, args);
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task.GetAwaiter().GetResult();
        }

        private bool IsOnMainThread()
        {
            try
            {
                var essentialsType = FindTypeByName("Xamarin.Essentials.MainThread");
                if (essentialsType != null)
                {
                    var prop = essentialsType.GetProperty("IsMainThread",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (prop?.GetValue(null) is bool isMain)
                        return isMain;
                }

                var deviceType = FindTypeByName("Xamarin.Forms.Device");
                if (deviceType != null)
                {
                    var prop = deviceType.GetProperty("IsInvokeRequired",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (prop?.GetValue(null) is bool isInvokeRequired)
                        return !isInvokeRequired;
                }
            }
            catch { }

            return false;
        }

        private bool TryBeginInvokeOnMainThread(Action action)
        {
            try
            {
                var essentialsType = FindTypeByName("Xamarin.Essentials.MainThread");
                if (essentialsType != null)
                {
                    var method = essentialsType.GetMethod("BeginInvokeOnMainThread",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (method != null)
                    {
                        method.Invoke(null, new object[] { action });
                        return true;
                    }
                }

                var deviceType = FindTypeByName("Xamarin.Forms.Device");
                if (deviceType != null)
                {
                    var method = deviceType.GetMethod("BeginInvokeOnMainThread",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (method != null)
                    {
                        method.Invoke(null, new object[] { action });
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }

        private async Task SendResponseAsync(WSMessage request, bool success, object data = null, string error = null)
        {
            if (request == null || string.IsNullOrEmpty(request.RequestId))
                return;

            try
            {
                var reqId = request.RequestId ?? "-";
                var action = request.Action ?? "-";
                var dataDesc = data == null ? "null" : data.GetType().Name;
                var errDesc = string.IsNullOrWhiteSpace(error) ? "-" : error;
                Log($"[OBDCloudManager] TX response action={action} req={reqId} ok={success} data={dataDesc} err={errDesc}");
            }
            catch { }

            var response = new WSMessage
            {
                Type = MessageType.Response,
                Action = request.Action,
                RequestId = request.RequestId,
                Success = success,
                Error = error,
                Data = data,
                SessionId = request.SessionId
            };

            await _bridge.SendAsync(response).ConfigureAwait(false);
        }
    }
}
