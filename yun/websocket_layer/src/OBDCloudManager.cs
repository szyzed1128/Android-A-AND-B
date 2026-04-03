using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
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

        /// <summary>
        /// [FLOW] 全链路追踪日志 - 供本类及同命名空间类调用
        /// </summary>
        internal static void FlowLog(string direction, string description, string detail = null)
        {
            var ts = DateTime.Now.ToString("HH:mm:ss.fff");
            var dir = direction.PadRight(10);
            var msg = detail != null
                ? $"[FLOW] [{ts}] {dir} | {description} | {detail.Substring(0, Math.Min(120, detail.Length))}"
                : $"[FLOW] [{ts}] {dir} | {description}";
            Log(msg);
        }

        #endregion

        #region 单例模式（供 DLL 注入使用）

        private static OBDCloudManager _instance;
        private static readonly object _instanceLock = new object();
        private static bool _initializationStarted;
        private static string _pendingServerUrl = "ws://0.0.0.0:8080/ws";  // 默认监听地址

        /// <summary>
        /// B端最后一次 OBD 数据入队时间，由 WebSocketBluetoothConnection 更新，
        /// 用于计算 A端处理耗时（obdData到达 → onPIDValueChanged触发）
        /// </summary>
        internal static DateTime LastObdDataReceivedAt;

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
        private readonly object _disconnectCleanupLock = new object();
        private Delegate _obdStatusChangedDelegate;
        private object _obdReaderInstance;
        private Type _cachedObdReaderType;
        private Type _cachedAppType;
        private bool _disconnectCleanupPending;
        private string _disconnectCleanupSessionId;
        private DateTime _disconnectCleanupMarkedUtc = DateTime.MinValue;
        private readonly object _connectAttemptLock = new object();
        private bool _connectAttemptInProgress;
        private int _connectAttemptGeneration;
        private string _connectAttemptSessionId;
        private string _connectAttemptAddress;
        private string _connectAttemptProtocol;
        private DateTime _connectAttemptStartedUtc = DateTime.MinValue;

        // Burst Snapshot Mode 状态
        private volatile bool _isBurstMode;
        private volatile bool _burstRecoveryInProgress;
        private string _burstSessionId;
        private JArray _burstDescriptorsJson;

        // Burst Replay Mode 状态（第二批新增）
        private System.Collections.Generic.List<JObject> _burstPendingSamples;
        private TaskCompletionSource<string> _replayTcs;
        private IOBDConnection _originalConnection; // 热切换前保存的真实 connection
        // replay 时使用的 descriptors（Prepare 阶段保存，commit 时 _burstDescriptorsJson 已清空）
        private JArray _burstDescriptorsJsonForReplay;
        // replay 生命周期守卫：Task.Run 启动后置 true，finally 中置 false
        // HandlePrepareBurstSessionAsync / HandleAbortBurstSessionAsync 均需检查此标志
        private volatile bool _isReplayActive;
        // Abort 竞态记账：_isReplayActive=true 但 _replayTcs 尚未创建时，Abort 先记账；
        // ExecuteReplayAsync 一进 try 就检查，若为 true 则直接短路退出
        private volatile bool _pendingReplayAbort;
        private readonly object _replayResultLock = new object();
        private JArray _replayCollectedResults;
        private int _replayTotalCycles;
        private Dictionary<int, ReplayDescriptorBinding> _replayDescriptorBindings;
        private List<ReplayPendingEntry> _replayOpenEntries;

        private sealed class ReplayPidBinding
        {
            public int PidId { get; set; }
            public string PidName { get; set; }
            public string MatchName { get; set; }
            public int Unit { get; set; }
        }

        private sealed class ReplayDescriptorBinding
        {
            public int SeqId { get; set; }
            public bool DoNotDecode { get; set; }
            public List<ReplayPidBinding> Pids { get; } = new List<ReplayPidBinding>();
        }

        private sealed class ReplayPendingEntry
        {
            public JObject Entry { get; set; }
            public ReplayDescriptorBinding Descriptor { get; set; }
            public Dictionary<int, JObject> CapturedEventsByPidId { get; } = new Dictionary<int, JObject>();
            public Dictionary<string, JObject> CapturedEventsByName { get; } = new Dictionary<string, JObject>(StringComparer.OrdinalIgnoreCase);
            public TaskCompletionSource<bool> CompletionTcs { get; } = new TaskCompletionSource<bool>();
            public bool ForcedIncomplete { get; set; }
        }

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
            _bridge.ConnectionClosed += (s, reason) =>
            {
                Log($"[OBDCloudManager] Bridge connection closed: {reason}");
                HandleBridgeConnectionClosed(reason);
            };
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
                FinalizePendingDisconnectCleanupIfNeeded($"ConnectionLost:{reason}");
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
            FlowLog("A>B", $"JSBridge回调: {methodName}");
            // 只处理关心的 UI 回调，避免噪音
            switch (methodName)
            {
                case "onPIDValueChanged":
                    // 计算 A端处理耗时：从最后一次 obdData 入队到此回调触发的时间差
                    var aProcessingMs = (long)(DateTime.UtcNow - LastObdDataReceivedAt).TotalMilliseconds;
                    var pidRaw = NormalizeArg(args, 0);
                    var pidJo = (pidRaw as JObject) ?? JObject.FromObject(pidRaw ?? new object());
                    pidJo["_aMs"] = aProcessingMs;
                    TryCaptureReplayPidValueChanged(pidJo);
                    SendEvent(MessageAction.PIDValueChanged, pidJo);
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
                FlowLog("A>B", $"发送结果事件: {action}");

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
            FlowLog("B>A", $"收到请求: {message.Action}", $"req={message.RequestId ?? "-"}");

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
                    await HandleStopReadPidsAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.PrepareBurstSession:
                    await HandlePrepareBurstSessionAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.CommitBurstSamples:
                    await HandleCommitBurstSamplesAsync(message).ConfigureAwait(false);
                    break;
                case MessageAction.AbortBurstSession:
                    await HandleAbortBurstSessionAsync(message).ConfigureAwait(false);
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

                ClearPendingDisconnectCleanup($"new connect sessionId={sessionId}");

                var isNewAttempt = TryBeginConnectAttempt(sessionId, address, protocol, out var connectGeneration);

                // 绑定 sessionId 到 WebSocketBluetoothConnection
                _bluetoothConnection.BindSession(sessionId);
                _obdConnection.BindSession(sessionId);
                _bluetoothConnection.RememberConnectionHint(protocol, address);
                Log($"[OBDCloudManager] 绑定 sessionId: {sessionId}");

                // 配置 OBDDataReader 的 WebSocket 连接
                ConfigureOBDDataReaderForWebSocket();

                if (!isNewAttempt)
                {
                    Log($"[OBDCloudManager] 连接初始化已在进行中，复用当前尝试 generation={connectGeneration} sessionId={sessionId}");
                    await SendResponseAsync(message, true, new { connecting = true, sessionId, reused = true }).ConfigureAwait(false);
                    return;
                }

                Log($"[OBDCloudManager] 开始新的连接尝试 generation={connectGeneration} sessionId={sessionId}");

                // 先返回响应，避免 B 端等待超时（初始化异步执行）
                await SendResponseAsync(message, true, new { connecting = true, sessionId }).ConfigureAwait(false);

                var initAddress = address;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 触发原 CarScanner 的 OBDDataReader.Initialize 流程（完全使用原逻辑）
                        var initOk = await TryInitializeObdReaderAsync(initAddress, protocol).ConfigureAwait(false);
                        Log($"[OBDCloudManager] OBDDataReader.Initialize 触发完成 (ok={initOk}, generation={connectGeneration})");

                        if (!initOk && IsCurrentConnectAttempt(connectGeneration))
                        {
                            await _bridge.SendAsync(WSMessageFactory.CreateOBDStatusChangedEvent("Disconnected"))
                                .ConfigureAwait(false);
                        }
                    }
                    catch (Exception initEx)
                    {
                        Log($"[OBDCloudManager] OBD 初始化异常 generation={connectGeneration}: {initEx.Message}");
                        if (IsCurrentConnectAttempt(connectGeneration))
                        {
                            await _bridge.SendAsync(WSMessageFactory.CreateOBDStatusChangedEvent("Disconnected"))
                                .ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        CompleteConnectAttempt(connectGeneration, "Initialize task finished");
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

        private async Task<bool> TryInitializeObdReaderAsync(string deviceAddress, string protocol)
        {
            var initTs = DateTime.UtcNow;
            string InitElapsed() => $"[INIT +{(DateTime.UtcNow - initTs).TotalMilliseconds:0}ms]";

            try
            {
                Log($"[OBDCloudManager] {InitElapsed()} ▶ TryInitializeObdReaderAsync 开始 device={deviceAddress}");
                ConfigureOBDDataReaderForWebSocket();
                Log($"[OBDCloudManager] {InitElapsed()} ① WebSocket连接配置完成");
                TryConfigureSharedSettingsForWebSocket(deviceAddress, protocol);
                Log($"[OBDCloudManager] {InitElapsed()} ② SharedSettings配置完成");

                if (!TryResolveObdReader(out var obdReader, out var obdReaderType))
                {
                    Log($"[OBDCloudManager] {InitElapsed()} ✗ 无法解析 OBDDataReader，跳过 Initialize");
                    return false;
                }

                Log($"[OBDCloudManager] {InitElapsed()} ③ OBDDataReader实例已获取: {obdReader?.GetType().FullName ?? "null"}");

                // 原版 SimpleMainPage.StartConnection()（第883行）的行为：
                // 连接前重置 DisconnectRequested = false，否则 Connect()/Initialize() 会立即返回 false
                var disconnectReqField = obdReaderType.GetField("DisconnectRequested",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (disconnectReqField != null)
                {
                    disconnectReqField.SetValue(obdReader, false);
                    Log($"[OBDCloudManager] {InitElapsed()} ③+ DisconnectRequested = false 已重置");
                }

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

        private void TryConfigureSharedSettingsForWebSocket(string deviceAddress, string protocol)
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
                    var normalizedProtocol = (protocol ?? string.Empty).Trim().ToLowerInvariant();
                    string connectionTypeName;
                    switch (normalizedProtocol)
                    {
                        case "mfi":
                            connectionTypeName = "MFI_OBDLinkMXPlus";
                            break;
                        case "ble":
                            connectionTypeName = "BluetoothLE";
                            break;
                        case "wifi":
                            connectionTypeName = "WiFi";
                            break;
                        case "classic":
                        case "bt":
                        case "bluetooth":
                        default:
                            connectionTypeName = "Bluetooth";
                            break;
                    }

                    var connectionTypeValue = Enum.Parse(connEnumType, connectionTypeName);
                    connectionTypeProp.SetValue(current, connectionTypeValue);
                    Log($"[OBDCloudManager] 已设置 SharedSettings.ConnectionType = {connectionTypeName} (protocol={protocol ?? "(空)"})");
                }

                if (!string.IsNullOrWhiteSpace(deviceAddress) &&
                    !string.Equals(deviceAddress, "WEBSOCKET", StringComparison.OrdinalIgnoreCase))
                {
                    var normalizedProtocol = (protocol ?? string.Empty).Trim().ToLowerInvariant();
                    if (normalizedProtocol == "ble")
                    {
                        var btleIdProp = settingsType.GetProperty("BTLEDeviceID",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        btleIdProp?.SetValue(current, deviceAddress);
                        Log($"[OBDCloudManager] 已设置 SharedSettings.BTLEDeviceID = {deviceAddress}");
                    }
                    else
                    {
                        var btIdProp = settingsType.GetProperty("BTDeviceID",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        btIdProp?.SetValue(current, deviceAddress);
                        Log($"[OBDCloudManager] 已设置 SharedSettings.BTDeviceID = {deviceAddress}");
                    }
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

            NotifyUIStatusChanged(statusText);

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

            if (string.Equals(statusText, "ConnectedToECU", StringComparison.OrdinalIgnoreCase))
            {
                CompleteConnectAttempt(_connectAttemptGeneration, "StatusChanged:ConnectedToECU");
            }

            if (string.Equals(statusText, "Disconnected", StringComparison.OrdinalIgnoreCase))
            {
                FinalizePendingDisconnectCleanupIfNeeded("StatusChanged:Disconnected");
            }
        }


        private bool TryBeginConnectAttempt(string sessionId, string address, string protocol, out int generation)
        {
            lock (_connectAttemptLock)
            {
                if (_connectAttemptInProgress &&
                    string.Equals(_connectAttemptAddress, address, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(_connectAttemptProtocol, protocol, StringComparison.OrdinalIgnoreCase))
                {
                    _connectAttemptSessionId = sessionId;
                    generation = _connectAttemptGeneration;
                    return false;
                }

                _connectAttemptInProgress = true;
                _connectAttemptGeneration++;
                _connectAttemptSessionId = sessionId;
                _connectAttemptAddress = address;
                _connectAttemptProtocol = protocol;
                _connectAttemptStartedUtc = DateTime.UtcNow;
                generation = _connectAttemptGeneration;
                return true;
            }
        }

        private void CancelConnectAttempt(string reason)
        {
            bool hadAttempt;
            string sessionId;
            string address;
            string protocol;
            double ageMs;

            lock (_connectAttemptLock)
            {
                hadAttempt = _connectAttemptInProgress;
                sessionId = _connectAttemptSessionId;
                address = _connectAttemptAddress;
                protocol = _connectAttemptProtocol;
                ageMs = _connectAttemptStartedUtc == DateTime.MinValue
                    ? -1
                    : (DateTime.UtcNow - _connectAttemptStartedUtc).TotalMilliseconds;

                _connectAttemptInProgress = false;
                _connectAttemptGeneration++;
                _connectAttemptSessionId = null;
                _connectAttemptAddress = null;
                _connectAttemptProtocol = null;
                _connectAttemptStartedUtc = DateTime.MinValue;
            }

            if (hadAttempt)
            {
                Log($"[OBDCloudManager] 取消连接尝试 reason={reason} sessionId={sessionId ?? "(空)"} address={address ?? "(空)"} protocol={protocol ?? "(空)"} ageMs={ageMs:0}");
            }
        }

        private void CompleteConnectAttempt(int generation, string reason)
        {
            string sessionId;
            string address;
            string protocol;
            double ageMs;

            lock (_connectAttemptLock)
            {
                if (!_connectAttemptInProgress || generation != _connectAttemptGeneration)
                {
                    return;
                }

                sessionId = _connectAttemptSessionId;
                address = _connectAttemptAddress;
                protocol = _connectAttemptProtocol;
                ageMs = _connectAttemptStartedUtc == DateTime.MinValue
                    ? -1
                    : (DateTime.UtcNow - _connectAttemptStartedUtc).TotalMilliseconds;

                _connectAttemptInProgress = false;
                _connectAttemptSessionId = null;
                _connectAttemptAddress = null;
                _connectAttemptProtocol = null;
                _connectAttemptStartedUtc = DateTime.MinValue;
            }

            Log($"[OBDCloudManager] 完成连接尝试 generation={generation} reason={reason} sessionId={sessionId ?? "(空)"} address={address ?? "(空)"} protocol={protocol ?? "(空)"} ageMs={ageMs:0}");
        }

        private bool IsCurrentConnectAttempt(int generation)
        {
            lock (_connectAttemptLock)
            {
                return _connectAttemptInProgress && generation == _connectAttemptGeneration;
            }
        }

        private void MarkPendingDisconnectCleanup(string sessionId)
        {
            lock (_disconnectCleanupLock)
            {
                _disconnectCleanupPending = true;
                _disconnectCleanupSessionId = sessionId;
                _disconnectCleanupMarkedUtc = DateTime.UtcNow;
            }

            Log($"[OBDCloudManager] 标记待收口断开 sessionId={sessionId ?? "(空)"}");
        }

        private void ClearPendingDisconnectCleanup(string reason)
        {
            bool hadPending;
            string sessionId;
            double ageMs;

            lock (_disconnectCleanupLock)
            {
                hadPending = _disconnectCleanupPending || !string.IsNullOrWhiteSpace(_disconnectCleanupSessionId);
                sessionId = _disconnectCleanupSessionId;
                ageMs = _disconnectCleanupMarkedUtc == DateTime.MinValue
                    ? -1
                    : (DateTime.UtcNow - _disconnectCleanupMarkedUtc).TotalMilliseconds;
                _disconnectCleanupPending = false;
                _disconnectCleanupSessionId = null;
                _disconnectCleanupMarkedUtc = DateTime.MinValue;
            }

            if (hadPending)
            {
                Log($"[OBDCloudManager] 清除待收口断开 reason={reason} sessionId={sessionId ?? "(空)"} ageMs={ageMs:0}");
            }
        }

        private void TrySetObdReaderDisconnectRequested(bool requested, string reason)
        {
            try
            {
                if (!TryResolveObdReader(out var obdReader, out var obdReaderType))
                {
                    Log($"[OBDCloudManager] 设置 DisconnectRequested 失败：无法解析 OBDDataReader reason={reason}");
                    return;
                }

                var disconnectReqField = obdReaderType.GetField("DisconnectRequested",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (disconnectReqField == null)
                {
                    Log($"[OBDCloudManager] 设置 DisconnectRequested 失败：未找到字段 reason={reason}");
                    return;
                }

                disconnectReqField.SetValue(obdReader, requested);
                Log($"[OBDCloudManager] 设置 DisconnectRequested = {requested} reason={reason}");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 设置 DisconnectRequested 异常 reason={reason}: {ex.Message}");
            }
        }

        private void HandleBridgeConnectionClosed(string reason)
        {
            var normalizedReason = string.IsNullOrWhiteSpace(reason) ? "unknown" : reason;
            var activeSessionId = _bluetoothConnection?.SessionId ?? _obdConnection?.SessionId;

            CancelConnectAttempt($"BridgeConnectionClosed:{normalizedReason}");
            ClearPendingDisconnectCleanup($"BridgeConnectionClosed:{normalizedReason}");

            if (TryInvokeStage2DisconnectOnBridgeLoss(normalizedReason, activeSessionId))
            {
                return;
            }

            try
            {
                ResetOBDDataReaderWebSocket();
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Bridge closed ResetOBDDataReaderWebSocket 失败: {ex.Message}");
            }

            try
            {
                _bluetoothConnection?.ForceReset(reason: $"BridgeConnectionClosed:{normalizedReason}");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Bridge closed ForceReset 失败: {ex.Message}");
            }

            try
            {
                _obdConnection?.ResetSession();
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Bridge closed ResetSession 失败: {ex.Message}");
            }

            Log($"[OBDCloudManager] Bridge 真失联兜底收口完成 reason={normalizedReason} sessionId={activeSessionId ?? "(空)"}");
        }

        private bool TryInvokeStage2DisconnectOnBridgeLoss(string reason, string sessionId)
        {
            try
            {
                Log($"[OBDCloudManager] Bridge 真失联 -> 触发阶段二同源断开 reason={reason} sessionId={sessionId ?? "(空)"}");
                InvokeJsBridge("disconnectAsync");
                Log($"[OBDCloudManager] Bridge 真失联 -> 已触发 A 端断开按钮同源链 reason={reason} sessionId={sessionId ?? "(空)"}");
                return true;
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Bridge 真失联 -> 触发阶段二同源断开失败，转兜底收口 reason={reason} sessionId={sessionId ?? "(空)"} err={ex.Message}");
            }

            TrySetObdReaderDisconnectRequested(true, $"BridgeConnectionClosedFallback:{reason}");
            TryForceObdReaderStatus("Disconnected", $"BridgeConnectionClosedFallback:{reason}");
            NotifyUIStatusChanged("Disconnected");
            return false;
        }

        private void TryForceObdReaderStatus(string statusName, string reason)
        {
            try
            {
                if (!TryResolveObdReader(out var obdReader, out var obdReaderType))
                {
                    Log($"[OBDCloudManager] 强制设置 CurrentStatus 失败：无法解析 OBDDataReader reason={reason}");
                    return;
                }

                var setStatusMethod = obdReaderType.GetMethod("SetStatusForTest",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (setStatusMethod != null)
                {
                    var parameters = setStatusMethod.GetParameters();
                    if (parameters.Length == 1)
                    {
                        var targetStatus = Enum.Parse(parameters[0].ParameterType, statusName);
                        setStatusMethod.Invoke(obdReader, new object[] { targetStatus });
                        Log($"[OBDCloudManager] 强制设置 CurrentStatus = {statusName} reason={reason} via SetStatusForTest");
                        return;
                    }
                }

                var currentStatusProperty = obdReaderType.GetProperty("CurrentStatus",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (currentStatusProperty?.CanWrite == true)
                {
                    var targetStatus = Enum.Parse(currentStatusProperty.PropertyType, statusName);
                    currentStatusProperty.SetValue(obdReader, targetStatus);
                    Log($"[OBDCloudManager] 强制设置 CurrentStatus = {statusName} reason={reason} via Property");
                    return;
                }

                var currentStatusField = obdReaderType.GetField("CurrentStatus",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (currentStatusField != null)
                {
                    var targetStatus = Enum.Parse(currentStatusField.FieldType, statusName);
                    currentStatusField.SetValue(obdReader, targetStatus);
                    Log($"[OBDCloudManager] 强制设置 CurrentStatus = {statusName} reason={reason} via Field");
                    return;
                }

                Log($"[OBDCloudManager] 强制设置 CurrentStatus 失败：未找到可写入口 reason={reason}");
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                Log($"[OBDCloudManager] 强制设置 CurrentStatus 异常 reason={reason}: {message}");
            }
        }

        private void FinalizePendingDisconnectCleanupIfNeeded(string trigger)
        {
            bool pending;
            string expectedSessionId;
            double ageMs;

            lock (_disconnectCleanupLock)
            {
                pending = _disconnectCleanupPending;
                expectedSessionId = _disconnectCleanupSessionId;
                ageMs = _disconnectCleanupMarkedUtc == DateTime.MinValue
                    ? -1
                    : (DateTime.UtcNow - _disconnectCleanupMarkedUtc).TotalMilliseconds;

                if (!pending)
                {
                    return;
                }

                _disconnectCleanupPending = false;
                _disconnectCleanupSessionId = null;
                _disconnectCleanupMarkedUtc = DateTime.MinValue;
            }

            var activeSessionId = _bluetoothConnection?.SessionId ?? _obdConnection?.SessionId;
            if (!string.IsNullOrWhiteSpace(expectedSessionId) &&
                !string.IsNullOrWhiteSpace(activeSessionId) &&
                !string.Equals(expectedSessionId, activeSessionId, StringComparison.Ordinal))
            {
                Log($"[OBDCloudManager] 跳过断开收口 trigger={trigger} pending={expectedSessionId} current={activeSessionId} ageMs={ageMs:0}");
                return;
            }

            try
            {
                ResetOBDDataReaderWebSocket();
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 断开收口 ResetOBDDataReaderWebSocket 失败: {ex.Message}");
            }

            try
            {
                _bluetoothConnection.ForceReset(reason: $"FinalizePendingDisconnectCleanupIfNeeded:{trigger}");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 断开收口 ForceReset 失败: {ex.Message}");
            }

            try
            {
                _obdConnection.ResetSession();
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] 断开收口 ResetSession 失败: {ex.Message}");
            }

            Log($"[OBDCloudManager] 完成断开收口 trigger={trigger} sessionId={expectedSessionId ?? "(空)"} ageMs={ageMs:0}");
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
            bool disconnectRequestedSet = false;
            bool jsBridgeInvoked = false;

            try
            {
                if (_uiBridge == null)
                {
                    throw new InvalidOperationException("JSBridge未注册");
                }

                var activeSessionId = _bluetoothConnection.SessionId ?? _obdConnection.SessionId;
                CancelConnectAttempt("HandleObdDisconnectAsync");
                TrySetObdReaderDisconnectRequested(true, "HandleObdDisconnectAsync");
                disconnectRequestedSet = true;
                MarkPendingDisconnectCleanup(activeSessionId);
                _bluetoothConnection.EnqueueDisconnectSession(activeSessionId);

                InvokeJsBridge("disconnectAsync");
                jsBridgeInvoked = true;
                _bluetoothConnection.ForceReset(preserveQueuedDisconnects: true, reason: "HandleObdDisconnectAsync");
                _obdConnection.ResetSession();
                Log($"[OBDCloudManager] disconnectAsync 调用成功 sessionId={activeSessionId ?? "(空)"}");

                await SendResponseAsync(message, true, new { disconnecting = true, sessionId = activeSessionId })
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var activeSessionId = _bluetoothConnection.SessionId ?? _obdConnection.SessionId;
                _bluetoothConnection.CancelQueuedDisconnectSession(activeSessionId, $"HandleObdDisconnectAsync failed: {ex.Message}");
                ClearPendingDisconnectCleanup($"HandleObdDisconnectAsync failed: {ex.Message}");
                if (disconnectRequestedSet && !jsBridgeInvoked)
                {
                    TrySetObdReaderDisconnectRequested(false, $"HandleObdDisconnectAsync rollback: {ex.Message}");
                }
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

                var wsConnField = obdDataReaderType.GetField("WebSocketConnection",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                wsConnField?.SetValue(null, null);

                Log("[OBDCloudManager] 重置 OBDDataReader.UseWebSocketConnection = false, WebSocketConnection = null");
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

                // ── 方案A：OBD mode01 批量合并优化 ──────────────────────────────────────
                // 原版 CarScanner 的真实入口是 RequestProducerStatic.UpdateOBDReaderRequests()，
                // 该方法内部调用 OBDRequestQueueOptimizer.Optimize()，将同 Header 下的 mode01
                // 命令合并为 OBDMultiRequest（最多6条/帧），一次 AT 命令读多个 PID，大幅提速。
                // 我们的 JSBridge startReadPIDs 绕过了这一步，导致每个 PID 单独发命令。
                // 修复：在调用 startReadPIDs 前，用反射走完整的优化路径并写入 CommandQueue。
                //
                // UDS mode22 优化（TODO）：
                //   将 SharedSettings.CANOptimizeMode22 设为 true 即可启用，
                //   但 UDS 需要自学习数据长度字典（CANOptimizeMode22DataLengthDictionary）
                //   积累足够数据后才能合并，首次连接会自然退化为逐条发送，无风险。
                //   启用时在下方 SetOptimizeFlags 处增加一行：
                //     canOptimizeMode22Prop?.SetValue(settings, true);
                // ────────────────────────────────────────────────────────────────────────
                bool optimizeApplied = false;
                try
                {
                    int[] pidIndices = indices != null
                        ? indices.ToObject<int[]>() ?? Array.Empty<int>()
                        : Array.Empty<int>();

                    if (pidIndices.Length > 0 && TryResolveObdReader(out var obdReader, out var obdReaderType))
                    {
                        optimizeApplied = TryApplyOptimizedQueue(obdReader, obdReaderType, pidIndices);
                    }
                }
                catch (Exception optEx)
                {
                    Log($"[OBDCloudManager] ⚠ StartReadPIDs: 优化队列失败，回退到 JSBridge: {optEx.Message}");
                }

                // 无论优化是否成功，始终调用 startReadPIDs 确保 CarScanner 内部状态（Running 标志等）正确启动
                // 注意：optimizeApplied=true 时队列已由 ReplaceQueue+StartLoopV3 写入并启动，
                //       不再需要调用 JSBridge startReadPIDs（否则会覆盖已优化的队列）
                if (!optimizeApplied)
                {
                    try
                    {
                        InvokeJsBridge("startReadPIDs", json);
                        Log($"[OBDCloudManager] ✓ StartReadPIDs: InvokeJsBridge 成功 indices={json}");
                    }
                    catch (Exception jex)
                    {
                        Log($"[OBDCloudManager] ⚠ StartReadPIDs: JSBridge 调用失败 {jex.GetType().Name}: {jex.Message}");
                    }
                }
                else
                {
                    Log($"[OBDCloudManager] ✓ StartReadPIDs: 优化队列已写入并启动，跳过 JSBridge indices={json}");
                }

                await SendResponseAsync(message, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 按 pidIndices 走原版优化路径：GetRequests → Optimize → ReplaceQueue。
        /// 复刻 RequestProducerStatic.UpdateOBDReaderRequests() 的核心逻辑。
        /// OBD mode01 命令会被合并为 OBDMultiRequest（最多6条/帧）；
        /// UDS mode22 命令在 CANOptimizeMode22=false 时原样保留，不受影响。
        /// 返回 true 表示优化后的队列已写入，startReadPIDs 仍会被调用以启动 Running 状态。
        /// </summary>
        private bool TryApplyOptimizedQueue(object obdReader, Type obdReaderType, int[] pidIndices)
        {
            // 1. 获取 LiveDataPIDModel 类型及 _PIDCollection 字段
            var ldpmType = FindType("CarScannerXamarinForms.ViewModels.LiveDataPIDModel");
            if (ldpmType == null) { Log("[OBDCloudManager] OptQueue: LiveDataPIDModel 类型未找到"); return false; }

            var pidCollectionField = ldpmType.GetField("_PIDCollection",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
                | System.Reflection.BindingFlags.Public);
            if (pidCollectionField == null) { Log("[OBDCloudManager] OptQueue: _PIDCollection 字段未找到"); return false; }

            var pidCollection = pidCollectionField.GetValue(null);
            if (pidCollection == null) { Log("[OBDCloudManager] OptQueue: _PIDCollection 为 null"); return false; }

            var collectionType = pidCollection.GetType();
            var countProp = collectionType.GetProperty("Count");
            var itemProp  = collectionType.GetProperty("Item");
            int totalPids = (int)countProp.GetValue(pidCollection);

            // 2. 构建 List<OBDRequest>，对每个 index 调用 GetRequests
            var obdRequestType  = FindType("CarScannerXamarinForms.OBD2.OBDRequest");
            if (obdRequestType == null) { Log("[OBDCloudManager] OptQueue: OBDRequest 类型未找到"); return false; }
            var requestListType = typeof(System.Collections.Generic.List<>).MakeGenericType(obdRequestType);
            var requestList     = Activator.CreateInstance(requestListType);

            var getRequestsMethod = ldpmType.GetMethod("GetRequests",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                null, new Type[]
                {
                    FindType("CarScannerXamarinForms.OBD2.PIDS.IPID"),
                    requestListType,
                    typeof(string),
                    typeof(string)
                }, null);
            if (getRequestsMethod == null) { Log("[OBDCloudManager] OptQueue: GetRequests 方法未找到"); return false; }

            foreach (var idx in pidIndices)
            {
                if (idx < 0 || idx >= totalPids) continue;
                var pid = itemProp.GetValue(pidCollection, new object[] { idx });
                if (pid == null) continue;
                getRequestsMethod.Invoke(null, new object[] { pid, requestList, null, "" });
            }

            int rawCount = (int)requestListType.GetProperty("Count").GetValue(requestList);
            Log($"[OBDCloudManager] OptQueue: GetRequests 生成 {rawCount} 条原始请求");
            if (rawCount == 0) return false;

            // 3. 开启 CANOptimizeRequests 并调用 Optimize()
            //    仅在本次调用前临时设置，Optimize 内部读取该值后立即生效。
            //    TODO（UDS）：若需启用 mode22 合并，在此处增加：
            //      canOptimizeMode22Prop?.SetValue(settings, true);
            var settingsType = FindType("CarScannerXamarinForms.Settings.SharedSettings");
            var settingsCurrentProp = settingsType?.GetProperty("Current",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var settings = settingsCurrentProp?.GetValue(null);
            var canOptimizeProp = settingsType?.GetProperty("CANOptimizeRequests");
            if (settings != null && canOptimizeProp != null)
                canOptimizeProp.SetValue(settings, true);

            IEnumerable<object> optimized = null;
            var optimizerType  = FindType("CarScannerXamarinForms.OBD2.OBDRequestQueueOptimizer");
            var optimizeMethod = optimizerType?.GetMethod("Optimize",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (optimizeMethod != null)
            {
                var result = optimizeMethod.Invoke(null, new object[] { requestList });
                if (result != null)
                {
                    optimized = result as IEnumerable<object>
                                ?? (result as System.Collections.IEnumerable)?.Cast<object>();
                    Log("[OBDCloudManager] OptQueue: Optimize 完成");
                }
            }
            else
            {
                Log("[OBDCloudManager] OptQueue: Optimize 方法未找到，使用未优化队列");
            }

            // 4. 调用 ReplaceQueue 写入已优化的队列
            var replaceQueueMethod = obdReaderType.GetMethod("ReplaceQueue",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (replaceQueueMethod == null) { Log("[OBDCloudManager] OptQueue: ReplaceQueue 方法未找到"); return false; }

            replaceQueueMethod.Invoke(obdReader, new object[] { optimized ?? requestList });
            int finalCount = optimized != null ? optimized.Count() : rawCount;
            Log($"[OBDCloudManager] OptQueue: ReplaceQueue 写入 {finalCount} 条优化后请求（原始 {rawCount} 条）");

            // 5. 直接调用 StartLoopV3 启动轮询（对应原版 SetRunning(true)）
            //    不经过 JSBridge startReadPIDs，避免其内部再次 ReplaceQueue 覆盖优化结果
            var startLoopMethod = obdReaderType.GetMethod("StartLoopV3",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (startLoopMethod != null)
            {
                startLoopMethod.Invoke(obdReader, new object[] { "OptimizedQueue" });
                Log("[OBDCloudManager] OptQueue: StartLoopV3 已启动");
            }
            else
            {
                Log("[OBDCloudManager] OptQueue: StartLoopV3 方法未找到，回退由 JSBridge 启动");
                return false; // 让 HandleStartReadPidsAsync 回退到 JSBridge
            }

            return true;
        }

        private async Task HandleStopReadPidsAsync(WSMessage message)
        {
            try
            {
                Log("[OBDCloudManager] ▶ HandleStopReadPidsAsync 开始");

                // 直接在 WS 后台线程调用 _uiBridge.InvokeMethod，绕过 InvokeJsBridgeOnMainThread。
                // 原因：StopReadPIDs() 内部有 ClearRequestQueue().Wait()，若从主线程调用，
                // Task.Delay 续体会被派发回主线程，但主线程正被 .Wait() 阻塞 → 死锁。
                // WS 后台线程无 SynchronizationContext，续体回到线程池，.Wait() 可正常完成。
                WebSocketUIBridge bridge;
                lock (_uiBridgeLock) { bridge = _uiBridge; }

                if (bridge != null)
                {
                    try
                    {
                        bridge.InvokeMethod("stopReadPIDs");
                        Log("[OBDCloudManager] ✓ StopReadPIDs: InvokeMethod 完成，modelPIDList 已清空");
                    }
                    catch (Exception jex)
                    {
                        Log($"[OBDCloudManager] ⚠ StopReadPIDs: InvokeMethod 失败 {jex.GetType().Name}: {jex.Message}");
                    }
                }
                else
                {
                    Log("[OBDCloudManager] ⚠ StopReadPIDs: _uiBridge 未初始化");
                }

                await SendResponseAsync(message, true).ConfigureAwait(false);
                Log("[OBDCloudManager] ◀ HandleStopReadPidsAsync 完成");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] ✗ HandleStopReadPidsAsync 异常: {ex.GetType().Name}: {ex.Message}");
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

        #region Burst Snapshot Mode

        private async Task HandlePrepareBurstSessionAsync(WSMessage message)
        {
            try
            {
                Log("[OBDCloudManager] ▶ HandlePrepareBurstSessionAsync 开始");

                if (_burstRecoveryInProgress)
                {
                    await SendResponseAsync(message, false, null, "上次 Burst 恢复仍在进行中，请稍后重试").ConfigureAwait(false);
                    return;
                }
                if (_isReplayActive)
                {
                    await SendResponseAsync(message, false, null, "Burst Replay 正在进行中，请等待完成或先 Abort").ConfigureAwait(false);
                    return;
                }
                if (_isBurstMode)
                {
                    await SendResponseAsync(message, false, null, "已有 Burst 会话正在进行").ConfigureAwait(false);
                    return;
                }

                var obj = message.GetData<JObject>();
                var pidIndicesToken = obj?["pidIndices"];
                if (pidIndicesToken == null)
                {
                    await SendResponseAsync(message, false, null, "缺少 pidIndices").ConfigureAwait(false);
                    return;
                }
                var pidIndices = pidIndicesToken.ToObject<int[]>();
                if (pidIndices == null || pidIndices.Length == 0)
                {
                    await SendResponseAsync(message, false, null, "pidIndices 为空").ConfigureAwait(false);
                    return;
                }

                Log($"[OBDCloudManager] Burst: pidIndices=[{string.Join(",", pidIndices)}]");

                if (!TryResolveObdReader(out var obdReader, out var obdReaderType))
                {
                    await SendResponseAsync(message, false, null, "无法解析 OBDDataReader").ConfigureAwait(false);
                    return;
                }

                // ① ClearRequestQueue
                var clearMethod = obdReaderType.GetMethod("ClearRequestQueue",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                    null, Type.EmptyTypes, null);
                if (clearMethod != null)
                {
                    var taskObj = clearMethod.Invoke(obdReader, null);
                    if (taskObj is Task t) await t.ConfigureAwait(false);
                }
                Log("[OBDCloudManager] Burst: ① ClearRequestQueue 完成");

                // ② 孤立 loopId
                var loopIdField = obdReaderType.GetField("loopId",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (loopIdField != null)
                {
                    loopIdField.SetValue(obdReader, new Random().Next());
                    Log("[OBDCloudManager] Burst: ② loopId 已孤立");
                }
                else
                {
                    Log("[OBDCloudManager] Burst: ⚠ loopId 字段未找到");
                }

                // ③ SetRunning(false)
                var setRunningMethod = obdReaderType.GetMethod("SetRunning",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (setRunningMethod != null)
                {
                    setRunningMethod.Invoke(obdReader, new object[] { false, "BurstMode" });
                    Log("[OBDCloudManager] Burst: ③ SetRunning(false) 完成");
                }
                else
                {
                    Log("[OBDCloudManager] Burst: ⚠ SetRunning 方法未找到");
                }

                // ④ 轮询停稳
                var quiesced = await PollForQuiescence(obdReader, obdReaderType, 20000).ConfigureAwait(false);
                if (!quiesced)
                {
                    Log("[OBDCloudManager] Burst: ✗ 停稳超时，启动后台延迟恢复");
                    StartDeferredRecovery(obdReader, obdReaderType);
                    try { await SendResponseAsync(message, false, null, "主循环停稳超时").ConfigureAwait(false); }
                    catch (Exception respEx) { Log($"[OBDCloudManager] Burst: 超时响应发送失败（恢复已启动）: {respEx.Message}"); }
                    return;
                }
                Log("[OBDCloudManager] Burst: ④ 停稳确认");

                // ⑤ 开启 A 端守卫
                _bluetoothConnection.SetBurstMode(true);
                _obdConnection?.SetBurstMode(true);

                // ⑥ 清缓冲
                _bluetoothConnection.ClearReceiveBuffer();
                _obdConnection?.ClearReceiveBuffer();
                Log("[OBDCloudManager] Burst: ⑤⑥ guard + buffer 完成");

                // ⑦ 生成 descriptors
                var descriptors = GenerateBurstDescriptors(obdReader, obdReaderType, pidIndices);
                if (descriptors == null)
                {
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    RestoreLoopSafe(obdReader, obdReaderType);
                    await SendResponseAsync(message, false, null, "descriptor 生成失败").ConfigureAwait(false);
                    return;
                }

                // ⑧ 生成 pingCommandText（按 StartLoopV3 内联 4-branch，不使用 GetPingRequest()）
                // 三个 Settings 属性必须从 SharedSettings.Current 读取；返回 null = fail-closed
                string pingCommandText = GeneratePingCommandText(obdReader, obdReaderType);
                if (pingCommandText == null)
                {
                    Log("[OBDCloudManager] Burst: ✗ pingCommandText 生成失败 → Prepare fail-closed");
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    RestoreLoopSafe(obdReader, obdReaderType);
                    await SendResponseAsync(message, false,
                        new JObject { ["failClosed"] = true, ["reason"] = "pingCommandText 生成失败（SharedSettings 不可读）" },
                        BurstErrorCode.PingGenerationFailed).ConfigureAwait(false);
                    return;
                }
                Log($"[OBDCloudManager] Burst: ⑧ pingCommandText={pingCommandText}");

                // ⑨ Bypass 冲突检测：ping / beforeCommands / afterCommands 不能与 descriptor.command 重叠
                var bypassConflictData = CheckBypassConflicts(descriptors, pingCommandText);
                if (bypassConflictData != null)
                {
                    Log($"[OBDCloudManager] Burst: ✗ bypass 冲突 → Prepare 失败");
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    RestoreLoopSafe(obdReader, obdReaderType);
                    await SendResponseAsync(message, false, bypassConflictData, BurstErrorCode.BypassConflict).ConfigureAwait(false);
                    return;
                }
                Log("[OBDCloudManager] Burst: ⑨ bypass 冲突检测通过");

                // ⑩ lowPriority 冲突检测（运行时递归，全量 BFS，visited set，fail-closed）
                // 获取 _PIDCollection 用于检测（Prepare 阶段允许使用）
                var ldpmType = FindType("CarScannerXamarinForms.ViewModels.LiveDataPIDModel");
                var pidCollectionField = ldpmType?.GetField("_PIDCollection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
                    | System.Reflection.BindingFlags.Public);
                var pidCollection = pidCollectionField?.GetValue(null);

                if (pidCollection == null)
                {
                    Log("[OBDCloudManager] Burst: ⚠ _PIDCollection 不可用，lowPriority 检测 fail-closed → Prepare 失败");
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    RestoreLoopSafe(obdReader, obdReaderType);
                    await SendResponseAsync(message, false,
                        new JObject { ["failClosed"] = true, ["reason"] = "_PIDCollection 不可用" },
                        BurstErrorCode.LowPriorityConflict).ConfigureAwait(false);
                    return;
                }

                var lpConflictData = CheckLowPriorityConflicts(pidIndices, pidCollection, pidCollection.GetType());
                if (lpConflictData != null)
                {
                    bool isFailClosed = lpConflictData.Value<bool>("failClosed");
                    Log($"[OBDCloudManager] Burst: ✗ lowPriority 冲突 failClosed={isFailClosed} → Prepare 失败");
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    RestoreLoopSafe(obdReader, obdReaderType);
                    await SendResponseAsync(message, false, lpConflictData, BurstErrorCode.LowPriorityConflict).ConfigureAwait(false);
                    return;
                }
                Log("[OBDCloudManager] Burst: ⑩ lowPriority 冲突检测通过");

                // ⑪ MVP 前置拒绝：skipATSH=true（VwTp20 + Header=="000"）
                // ReplayConnection 依赖 ATSH 命令更新 _currentHeader；skipATSH 时不发 ATSH，
                // 导致 _currentHeader 不更新但 exchangeKey 仍含 effectiveHeader → 必然 mismatch。
                // MVP 阶段直接拒绝，待 header 状态同步方案补完后再开放。
                var skipATSHDesc = descriptors.OfType<JObject>()
                    .FirstOrDefault(d => d.Value<bool>("skipATSH"));
                if (skipATSHDesc != null)
                {
                    var rejectedSeqId = skipATSHDesc.Value<string>("seqId") ?? "?";
                    Log($"[OBDCloudManager] Burst: ✗ skipATSH=true 不支持（seqId={rejectedSeqId}）→ Prepare 拒绝");
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    RestoreLoopSafe(obdReader, obdReaderType);
                    await SendResponseAsync(message, false,
                        new JObject { ["rejectedSeqId"] = rejectedSeqId, ["reason"] = "skipATSH=true 协议（VwTp20/Header=000）MVP 阶段不支持 Burst Replay" },
                        BurstErrorCode.SkipATSHUnsupported).ConfigureAwait(false);
                    return;
                }
                Log("[OBDCloudManager] Burst: ⑪ skipATSH 检测通过");

                // ⑫ MVP 前置拒绝：isMultiRequest=true
                // BuildReplayQueue 当前只重建 OBDRequest，OBDMultiRequest 语义不等价，带错误语义进入 replay 风险高。
                // MVP 阶段直接拒绝，待 OBDMultiRequest 重建方案审批后再开放。
                var multiReqDesc = descriptors.OfType<JObject>()
                    .FirstOrDefault(d => d.Value<bool>("isMultiRequest"));
                if (multiReqDesc != null)
                {
                    var rejectedSeqId = multiReqDesc.Value<int?>("seqId");
                    var rejectedCommand = multiReqDesc.Value<string>("command") ?? "?";
                    Log($"[OBDCloudManager] Burst: ✗ isMultiRequest=true 不支持（seqId={rejectedSeqId} command={rejectedCommand}）→ Prepare 拒绝");
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    RestoreLoopSafe(obdReader, obdReaderType);
                    await SendResponseAsync(message, false,
                        new JObject
                        {
                            ["rejectedSeqId"] = rejectedSeqId,
                            ["rejectedCommand"] = rejectedCommand,
                            ["reason"] = "isMultiRequest=true 协议 MVP 阶段不支持 Burst Replay"
                        },
                        BurstErrorCode.MultiRequestUnsupported).ConfigureAwait(false);
                    return;
                }
                Log("[OBDCloudManager] Burst: ⑫ multi-request 检测通过");

                _burstSessionId = Guid.NewGuid().ToString("N").Substring(0, 12);
                _isBurstMode = true;
                _burstDescriptorsJson = descriptors;
                // 同时保存到 replay 专用字段，防止 isFinal 时 _burstDescriptorsJson 已被清空
                _burstDescriptorsJsonForReplay = descriptors;
                _burstPendingSamples = null; // 清空上次残留

                Log($"[OBDCloudManager] Burst: 生成 {descriptors.Count} 个 descriptor, sessionId={_burstSessionId}");

                await SendResponseAsync(message, true, new JObject
                {
                    ["sessionId"] = _burstSessionId,
                    ["descriptors"] = descriptors,
                    // pingCommandText 供审计；ReplayConnection 在 A 端自行重新计算，不依赖此值
                    ["pingCommandText"] = pingCommandText
                }).ConfigureAwait(false);

                Log("[OBDCloudManager] ◀ HandlePrepareBurstSessionAsync 成功");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] ✗ HandlePrepareBurstSessionAsync 异常: {ex.Message}");
                _isBurstMode = false;
                _burstDescriptorsJson = null;
                _bluetoothConnection.SetBurstMode(false);
                _obdConnection?.SetBurstMode(false);

                // 如果主循环已被停掉（步骤①-③已执行），统一走后台延迟恢复
                // 不在 catch 中直接 RestoreLoopSafe，因为无法保证 ATRV 尾部 ReadData 已结束
                if (TryResolveObdReader(out var exObdReader, out var exObdReaderType))
                {
                    var exRunningField = exObdReaderType.GetField("_Running",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    bool exRunning = true;
                    if (exRunningField != null) exRunning = (bool)exRunningField.GetValue(exObdReader);

                    if (!exRunning)
                    {
                        Log("[OBDCloudManager] Burst: catch 中主循环已停，启动后台延迟恢复");
                        StartDeferredRecovery(exObdReader, exObdReaderType);
                    }
                }

                try { await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false); }
                catch (Exception respEx) { Log($"[OBDCloudManager] Burst: catch 响应发送失败: {respEx.Message}"); }
            }
        }

        private async Task HandleCommitBurstSamplesAsync(WSMessage message)
        {
            try
            {
                var obj = message.GetData<JObject>();
                var sessionId = obj?.Value<string>("sessionId") ?? "";
                var isFinal = obj?.Value<bool>("isFinal") ?? true;
                var chunkIndex = obj?.Value<int>("chunkIndex") ?? 0;
                var totalChunks = obj?.Value<int>("totalChunks") ?? 1;
                var samplesToken = obj?["samples"];

                Log($"[OBDCloudManager] ▶ CommitBurstSamples chunk={chunkIndex}/{totalChunks} isFinal={isFinal}");

                if (sessionId != _burstSessionId || !_isBurstMode)
                {
                    await SendResponseAsync(message, false, null, $"无效 sessionId 或 Burst 未激活: {sessionId}").ConfigureAwait(false);
                    return;
                }

                // 累积 samples
                var samplesArray = samplesToken as JArray ?? new JArray();
                if (_burstPendingSamples == null)
                    _burstPendingSamples = new System.Collections.Generic.List<JObject>();
                foreach (var t in samplesArray)
                    if (t is JObject jo) _burstPendingSamples.Add(jo);

                Log($"[OBDCloudManager] Burst: 累积 samples={_burstPendingSamples.Count} isFinal={isFinal}");

                if (!isFinal)
                {
                    // 非 final chunk：只确认收到
                    await SendResponseAsync(message, true, new JObject
                    {
                        ["sessionId"] = sessionId,
                        ["chunkIndex"] = chunkIndex,
                        ["accumulated"] = _burstPendingSamples.Count
                    }).ConfigureAwait(false);
                    return;
                }

                // ── isFinal=true：启动 replay ──────────────────────────────────
                // 按 sampleOrdinal 升序排列，构建顺序指针消费所需的 entries 数组
                var rawList = _burstPendingSamples;
                _burstPendingSamples = null;
                rawList.Sort((a, b) =>
                    (a.Value<int>("sampleOrdinal")).CompareTo(b.Value<int>("sampleOrdinal")));
                var allSamples = new JArray(rawList);
                var computedTotalCycles = rawList.Count == 0
                    ? 0
                    : rawList.Max(s => s.Value<int?>("cycleIndex") ?? -1) + 1;
                _replayTotalCycles = obj?.Value<int?>("totalCycles") ?? computedTotalCycles;
                Log($"[OBDCloudManager] Burst: final totalCycles={_replayTotalCycles}");

                // 先返回 ACK，replay 结果通过异步事件推送
                await SendResponseAsync(message, true, new JObject
                {
                    ["sessionId"] = sessionId,
                    ["replayStarted"] = true,
                    ["sampleCount"] = allSamples.Count
                }).ConfigureAwait(false);

                // 关闭 Burst 采样守卫（replay 期间不再需要）
                _bluetoothConnection.SetBurstMode(false);
                _obdConnection?.SetBurstMode(false);
                _bluetoothConnection.ClearReceiveBuffer();
                _obdConnection?.ClearReceiveBuffer();
                _isBurstMode = false;
                _burstDescriptorsJson = null;

                // 置守卫：replay 生命周期开始，阻止新 Prepare / 重复 Commit
                _bluetoothConnection.SetReplayRecoveryGuard(true);
                _isReplayActive = true;

                // 后台启动 replay（不 await，避免阻塞消息循环）
                _ = Task.Run(() => ExecuteReplayAsync(sessionId, allSamples));
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] ✗ CommitBurstSamples 异常: {ex.Message}");
                _burstPendingSamples = null;

                if (_isBurstMode)
                {
                    _isBurstMode = false;
                    _burstDescriptorsJson = null;
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    _bluetoothConnection.ClearReceiveBuffer();
                    _obdConnection?.ClearReceiveBuffer();

                    if (TryResolveObdReader(out var exReader, out var exType))
                    {
                        RestoreLoopSafe(exReader, exType);
                        Log("[OBDCloudManager] Burst: CommitBurstSamples catch 中已恢复主循环");
                    }
                }

                try { await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false); }
                catch (Exception respEx) { Log($"[OBDCloudManager] Burst: commit 错误响应发送失败: {respEx.Message}"); }
            }
        }

        /// <summary>
        /// Burst Replay 核心逻辑（后台运行，不阻塞消息循环）。
        ///
        /// 红线约束：
        /// - ReplayConnection / Connection 热切换 / NoDataLimit 修改，全部在此方法生命周期内生效
        /// - finally 中必须彻底恢复，不允许副作用泄漏到主路径
        /// - 不改变现有 pidValueChanged 实时链路行为
        /// - 不影响非 Burst 页面、非 Burst 诊断、非 Burst WebSocket 流程
        /// </summary>
        private async Task ExecuteReplayAsync(string sessionId, JArray allSamples)
        {
            Log($"[OBDCloudManager] Replay: ▶ 开始 sessionId={sessionId} samples={allSamples.Count}");

            object obdReader = null;
            Type obdReaderType = null;
            IOBDConnection savedConnection = null;
            System.Reflection.FieldInfo connectionField = null;
            System.Reflection.FieldInfo useWsStaticField = null;
            System.Reflection.FieldInfo wsStaticField = null;
            object savedStaticWsConnection = null;
            bool? savedUseWsConnection = null;
            int savedNoDataLimit = -1;
            object settingsCurrent = null;
            System.Reflection.PropertyInfo noDataLimitProp = null;
            IOBDConnection replayConn = null;
            Type replayConnType = null;
            System.Reflection.MethodInfo replayAbortMethod = null;
            System.Reflection.MethodInfo replaySetStatusForTestMethod = null;
            System.Reflection.FieldInfo replayLastActionField = null;
            System.Reflection.FieldInfo replayLastTimeConnectedField = null;
            System.Reflection.FieldInfo replayRecoveryGuardField = null;
            System.Reflection.MethodInfo replayRecoveryGuardSetter = null;
            bool? savedReplayRecoveryGuard = null;
            bool replayRecoveryGuardEnabled = false;
            bool hotSwapped = false;
            bool staticHotSwapped = false;

            try
            {
                // ── 1. 获取 OBDDataReader 实例 ────────────────────────────────
                if (!TryResolveObdReader(out obdReader, out obdReaderType))
                {
                    Log("[OBDCloudManager] Replay: ✗ OBDDataReader 不可用 → 放弃 replay");
                    await SendReplayFailedEventAsync(sessionId, "OBDDataReader 不可用").ConfigureAwait(false);
                    return;
                }

                // ── 1a. pendingAbort 短路：Abort 落在 _isReplayActive=true → _replayTcs 创建之间的竞态窗口
                // 必须在 TryResolveObdReader 之后检查，确保 obdReader 非 null，finally 可完整执行 RestoreLoopSafe
                // 不发送 BurstReplayFailed，安静退出，由 finally 统一恢复主循环
                if (_pendingReplayAbort)
                {
                    Log($"[OBDCloudManager] Replay: ⚠ pendingAbort 已记账，obdReader 已就绪，短路退出 sessionId={sessionId}");
                    return; // finally 持有 obdReader，可完整执行 F1/F4/F5
                }

                // ── 1b. 开启 OBDDataReader 自身恢复链隔离，防止 replay 期间进入 OnDisconnectDetected ──
                replayRecoveryGuardField = obdReaderType.GetField("ReplayRecoveryGuard",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                replayRecoveryGuardSetter = obdReaderType.GetMethod("SetReplayRecoveryGuard",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (replayRecoveryGuardField == null && replayRecoveryGuardSetter == null)
                {
                    Log("[OBDCloudManager] Replay: ✗ ReplayRecoveryGuard 不可用 → 放弃 replay");
                    await SendReplayFailedEventAsync(sessionId, "ReplayRecoveryGuard 不可用").ConfigureAwait(false);
                    return;
                }
                try
                {
                    if (replayRecoveryGuardField != null)
                    {
                        savedReplayRecoveryGuard = replayRecoveryGuardField.GetValue(null) as bool?;
                        if (!savedReplayRecoveryGuard.HasValue)
                        {
                            var rawGuard = replayRecoveryGuardField.GetValue(null);
                            if (rawGuard is bool boolGuard)
                            {
                                savedReplayRecoveryGuard = boolGuard;
                            }
                        }
                    }

                    if (replayRecoveryGuardSetter != null)
                    {
                        replayRecoveryGuardSetter.Invoke(null, new object[] { true });
                    }
                    else
                    {
                        replayRecoveryGuardField?.SetValue(null, true);
                    }
                    replayRecoveryGuardEnabled = true;
                    Log($"[OBDCloudManager] Replay: ✓ ReplayRecoveryGuard 已开启 original={savedReplayRecoveryGuard?.ToString() ?? "(unknown)"}");
                }
                catch (Exception guardEx)
                {
                    var guardMsg = guardEx.InnerException?.Message ?? guardEx.Message;
                    Log($"[OBDCloudManager] Replay: ✗ ReplayRecoveryGuard 开启失败: {guardMsg}");
                    await SendReplayFailedEventAsync(sessionId, $"ReplayRecoveryGuard 开启失败: {guardMsg}").ConfigureAwait(false);
                    return;
                }

                // ── 2. 重新生成 pingCommandText 用于 bypassSet ──────────────
                var savedDescriptors = _burstDescriptorsJsonForReplay;
                lock (_replayResultLock)
                {
                    _replayCollectedResults = new JArray();
                    _replayOpenEntries = new List<ReplayPendingEntry>();
                    _replayDescriptorBindings = BuildReplayDescriptorBindings(obdReader, obdReaderType, savedDescriptors);
                }
                Log($"[OBDCloudManager] Replay: 结果绑定 descriptors={_replayDescriptorBindings?.Count ?? 0}");
                string pingCommandText = GeneratePingCommandText(obdReader, obdReaderType) ?? "";
                Log($"[OBDCloudManager] Replay: pingCommandText={pingCommandText}");

                // ── 3. 构建 bypassSet ──────────────────────────────────────
                var bypassSet = BuildReplayBypassSet(savedDescriptors, pingCommandText);
                Log($"[OBDCloudManager] Replay: bypassSet={bypassSet.Count} items");

                // ── 4. 构建 descriptor-derived OBDRequest queue ────────────
                if (savedDescriptors == null || savedDescriptors.Count == 0)
                {
                    Log("[OBDCloudManager] Replay: ✗ replay descriptors 缺失 → 放弃 replay");
                    await SendReplayFailedEventAsync(sessionId, "Replay descriptors 不可用").ConfigureAwait(false);
                    return;
                }

                object replayQueue = BuildReplayQueue(obdReader, obdReaderType, savedDescriptors);
                if (replayQueue == null)
                {
                    Log("[OBDCloudManager] Replay: ✗ replay queue 构建失败 → 放弃 replay");
                    await SendReplayFailedEventAsync(sessionId, "Replay queue 构建失败").ConfigureAwait(false);
                    return;
                }

                // ── 5. 保存并提升 SharedSettings.Current.NoDataLimit ────────
                var settingsType = FindType("CarScannerXamarinForms.Settings.SharedSettings");
                var currentProp = settingsType?.GetProperty("Current",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                settingsCurrent = currentProp?.GetValue(null);
                noDataLimitProp = settingsCurrent?.GetType().GetProperty("NoDataLimit");
                if (noDataLimitProp != null && settingsCurrent != null)
                {
                    savedNoDataLimit = (int)(noDataLimitProp.GetValue(settingsCurrent) ?? 40);
                    noDataLimitProp.SetValue(settingsCurrent, int.MaxValue);
                    Log($"[OBDCloudManager] Replay: NoDataLimit {savedNoDataLimit} → int.MaxValue");
                }

                // ── 6. 获取 Connection 字段，热切换到 ReplayConnection ────
                connectionField = obdReaderType.GetField("Connection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.FlattenHierarchy);
                if (connectionField == null)
                {
                    Log("[OBDCloudManager] Replay: ✗ Connection 字段未找到 → 放弃 replay");
                    await SendReplayFailedEventAsync(sessionId, "Connection 字段未找到").ConfigureAwait(false);
                    return;
                }

                savedConnection = (IOBDConnection)connectionField.GetValue(obdReader);
                _originalConnection = savedConnection;

                // allSamples 已在 HandleCommitBurstSamplesAsync 中按 sampleOrdinal 升序排列
                var sortedEntries = allSamples.OfType<JObject>().ToArray();

                _replayTcs = new TaskCompletionSource<string>();
                replayConnType = FindType("OBDCloud.WebSocket.ReplayConnection");
                if (replayConnType == null)
                {
                    try
                    {
                        System.Reflection.Assembly.Load("WebSocketIOBDConnectionProxy");
                        replayConnType = FindType("OBDCloud.WebSocket.ReplayConnection");
                    }
                    catch (Exception loadEx)
                    {
                        Log($"[OBDCloudManager] Replay: ⚠ 加载 WebSocketIOBDConnectionProxy 失败: {loadEx.Message}");
                    }
                }

                if (replayConnType == null)
                {
                    Log("[OBDCloudManager] Replay: ✗ ReplayConnection 类型未找到 → 放弃 replay");
                    await SendReplayFailedEventAsync(sessionId, "ReplayConnection 类型未找到").ConfigureAwait(false);
                    return;
                }

                try
                {
                    replayConn = Activator.CreateInstance(
                        replayConnType,
                        sortedEntries,
                        bypassSet,
                        _replayTcs,
                        (Action<string>)Log,
                        (Action<JObject, int>)((entry, pointer) => OnReplayEntryConsumed(sessionId, entry, pointer)),
                        sessionId) as IOBDConnection;
                }
                catch (Exception createEx)
                {
                    var createMsg = createEx.InnerException?.Message ?? createEx.Message;
                    Log($"[OBDCloudManager] Replay: ✗ ReplayConnection 创建失败: {createMsg}");
                    await SendReplayFailedEventAsync(sessionId, $"ReplayConnection 创建失败: {createMsg}").ConfigureAwait(false);
                    return;
                }

                if (replayConn == null)
                {
                    Log("[OBDCloudManager] Replay: ✗ ReplayConnection 创建结果为 null → 放弃 replay");
                    await SendReplayFailedEventAsync(sessionId, "ReplayConnection 创建失败").ConfigureAwait(false);
                    return;
                }

                replayAbortMethod = replayConnType.GetMethod("Abort",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                connectionField.SetValue(obdReader, replayConn);
                hotSwapped = true;
                Log($"[OBDCloudManager] Replay: ✓ Connection 热切换完成 original={savedConnection?.GetType().Name} entries={sortedEntries.Length}");

                useWsStaticField = obdReaderType.GetField("UseWebSocketConnection",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                wsStaticField = obdReaderType.GetField("WebSocketConnection",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                if (useWsStaticField != null)
                {
                    try
                    {
                        savedUseWsConnection = (bool?)useWsStaticField.GetValue(null);
                        useWsStaticField.SetValue(null, true);
                        Log($"[OBDCloudManager] Replay: ✓ UseWebSocketConnection 热切换完成 original={savedUseWsConnection}");
                    }
                    catch (Exception useWsEx)
                    {
                        Log($"[OBDCloudManager] Replay: ✗ UseWebSocketConnection 热切换失败: {useWsEx.Message}");
                    }
                }
                else
                {
                    Log("[OBDCloudManager] Replay: ⚠ 未找到静态 UseWebSocketConnection 字段");
                }

                if (wsStaticField != null)
                {
                    try
                    {
                        savedStaticWsConnection = wsStaticField.GetValue(null);
                        wsStaticField.SetValue(null, replayConn);
                        staticHotSwapped = true;
                        Log($"[OBDCloudManager] Replay: ✓ 静态 WebSocketConnection 热切换完成 original={savedStaticWsConnection?.GetType().Name ?? "null"} replay={replayConn.GetType().Name}");
                    }
                    catch (Exception wsEx)
                    {
                        Log($"[OBDCloudManager] Replay: ✗ 静态 WebSocketConnection 热切换失败: {wsEx.Message}");
                        await SendReplayFailedEventAsync(sessionId, $"静态 WebSocketConnection 热切换失败: {wsEx.Message}").ConfigureAwait(false);
                        return;
                    }
                }
                else
                {
                    Log("[OBDCloudManager] Replay: ⚠ 未找到静态 WebSocketConnection 字段");
                }

                // ── 7. 确保 DisconnectRequested = false，并钉住 replay 上下文 ──
                var drField = obdReaderType.GetField("DisconnectRequested",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                drField?.SetValue(obdReader, false);

                replaySetStatusForTestMethod = obdReaderType.GetMethod("SetStatusForTest",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (replaySetStatusForTestMethod != null)
                {
                    try
                    {
                        var statusParams = replaySetStatusForTestMethod.GetParameters();
                        if (statusParams.Length == 1)
                        {
                            var connectedToEcu = Enum.Parse(statusParams[0].ParameterType, "ConnectedToECU");
                            replaySetStatusForTestMethod.Invoke(obdReader, new object[] { connectedToEcu });
                            Log("[OBDCloudManager] Replay: ✓ CurrentStatus 已钉住为 ConnectedToECU");
                        }
                    }
                    catch (Exception statusEx)
                    {
                        var statusMsg = statusEx.InnerException?.Message ?? statusEx.Message;
                        Log($"[OBDCloudManager] Replay: ⚠ CurrentStatus 钉住失败: {statusMsg}");
                    }
                }
                else
                {
                    Log("[OBDCloudManager] Replay: ⚠ 未找到 SetStatusForTest，跳过 CurrentStatus 钉住");
                }

                replayLastActionField = obdReaderType.GetField("LastAction",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (replayLastActionField != null)
                {
                    try
                    {
                        var readAction = Enum.Parse(replayLastActionField.FieldType, "Read");
                        replayLastActionField.SetValue(obdReader, readAction);
                        Log("[OBDCloudManager] Replay: ✓ LastAction 已钉住为 Read");
                    }
                    catch (Exception lastActionEx)
                    {
                        var lastActionMsg = lastActionEx.InnerException?.Message ?? lastActionEx.Message;
                        Log($"[OBDCloudManager] Replay: ⚠ LastAction 钉住失败: {lastActionMsg}");
                    }
                }
                else
                {
                    Log("[OBDCloudManager] Replay: ⚠ 未找到 LastAction 字段，跳过 LastAction 钉住");
                }

                replayLastTimeConnectedField = obdReaderType.GetField("lastTimeConnected",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (replayLastTimeConnectedField != null)
                {
                    try
                    {
                        var stopwatchProp = obdReaderType.GetProperty("stopwatch",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        var replayStopwatch = stopwatchProp?.GetValue(obdReader) as System.Diagnostics.Stopwatch;
                        if (replayStopwatch != null)
                        {
                            replayLastTimeConnectedField.SetValue(obdReader, replayStopwatch.ElapsedTicks);
                            Log($"[OBDCloudManager] Replay: ✓ lastTimeConnected 已刷新为 {replayStopwatch.ElapsedTicks}");
                        }
                        else
                        {
                            Log("[OBDCloudManager] Replay: ⚠ stopwatch 不可用，跳过 lastTimeConnected 刷新");
                        }
                    }
                    catch (Exception lastTimeEx)
                    {
                        var lastTimeMsg = lastTimeEx.InnerException?.Message ?? lastTimeEx.Message;
                        Log($"[OBDCloudManager] Replay: ⚠ lastTimeConnected 刷新失败: {lastTimeMsg}");
                    }
                }
                else
                {
                    Log("[OBDCloudManager] Replay: ⚠ 未找到 lastTimeConnected 字段，跳过连接时间刷新");
                }

                // ── 8. 替换 CommandQueue（descriptor-derived） ───────────
                if (replayQueue != null)
                {
                    var replaceQueueMethods = obdReaderType.GetMethods(
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    System.Reflection.MethodInfo replaceMethod = null;
                    foreach (var m in replaceQueueMethods)
                    {
                        if (m.Name == "ReplaceQueue")
                        {
                            var ps = m.GetParameters();
                            if (ps.Length == 1 && !ps[0].ParameterType.IsArray
                                && !ps[0].ParameterType.Name.StartsWith("OBDRequest"))
                            {
                                replaceMethod = m;
                                break;
                            }
                        }
                    }
                    if (replaceMethod == null)
                    {
                        Log("[OBDCloudManager] Replay: ✗ ReplaceQueue(IEnumerable) 方法未找到 → 放弃 replay");
                        await SendReplayFailedEventAsync(sessionId, "ReplaceQueue(IEnumerable) 方法未找到").ConfigureAwait(false);
                        return;
                    }

                    try
                    {
                        replaceMethod.Invoke(obdReader, new object[] { replayQueue });
                        Log("[OBDCloudManager] Replay: ✓ ReplaceQueue 完成");
                        LogReplayQueueSnapshot(obdReader, obdReaderType, "after ReplaceQueue", 3);
                    }
                    catch (Exception replaceEx)
                    {
                        var replaceMsg = replaceEx.InnerException?.Message ?? replaceEx.Message;
                        Log($"[OBDCloudManager] Replay: ✗ ReplaceQueue 调用失败: {replaceMsg}");
                        await SendReplayFailedEventAsync(sessionId, $"ReplaceQueue 失败: {replaceMsg}").ConfigureAwait(false);
                        return;
                    }
                }

                // ── 9. 孤立 loopId，防止旧循环触发 OnDisconnectDetected ──
                var loopIdField = obdReaderType.GetField("loopId",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                loopIdField?.SetValue(obdReader, new Random().Next());

                var setRunningMethod = obdReaderType.GetMethod("SetRunning",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (setRunningMethod != null)
                {
                    setRunningMethod.Invoke(obdReader, new object[] { false, "BurstReplay" });
                    Log("[OBDCloudManager] Replay: ✓ SetRunning(false) 完成");
                }
                else
                {
                    Log("[OBDCloudManager] Replay: ⚠ SetRunning 方法未找到");
                }

                // ── 10. 启动 StartLoopV3("BurstReplay") ────────────────────
                var startLoopMethod = obdReaderType.GetMethod("StartLoopV3",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (startLoopMethod == null)
                {
                    Log("[OBDCloudManager] Replay: ✗ StartLoopV3 未找到 → 放弃 replay");
                    await SendReplayFailedEventAsync(sessionId, "StartLoopV3 未找到").ConfigureAwait(false);
                    return;
                }
                LogReplayLoopState(obdReader, obdReaderType, "before StartLoopV3");
                startLoopMethod.Invoke(obdReader, new object[] { "BurstReplay" });
                Log("[OBDCloudManager] Replay: ✓ StartLoopV3(BurstReplay) 已调用");
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(300).ConfigureAwait(false);
                        LogReplayLoopState(obdReader, obdReaderType, "after StartLoopV3 +300ms");
                    }
                    catch (Exception diagEx)
                    {
                        Log($"[OBDCloudManager] ReplayDiag: 延迟状态采样异常: {diagEx.Message}");
                    }
                });

                // ── 11. 等待 replay 完成（含超时 5 分钟） ─────────────────
                var timeoutTask = Task.Delay(TimeSpan.FromMinutes(5));
                var completedTask = await Task.WhenAny(_replayTcs.Task, timeoutTask).ConfigureAwait(false);

                string terminationReason;
                if (completedTask == timeoutTask)
                {
                    terminationReason = "timeout";
                    Log("[OBDCloudManager] Replay: ✗ 等待超时（5 分钟）");
                    replayAbortMethod?.Invoke(replayConn, null);
                }
                else
                {
                    terminationReason = await _replayTcs.Task.ConfigureAwait(false);
                    Log($"[OBDCloudManager] Replay: 终止原因={terminationReason}");
                }

                // ── 12. 推送 replay 完成 / 失败事件 ───────────────────────
                if (terminationReason == "exhausted")
                {
                    await SettleReplayOpenEntriesAsync(sessionId, TimeSpan.FromSeconds(1), terminationReason).ConfigureAwait(false);

                    JArray resultsSnapshot;
                    int totalCyclesSnapshot;
                    lock (_replayResultLock)
                    {
                        resultsSnapshot = _replayCollectedResults != null
                            ? new JArray(_replayCollectedResults)
                            : new JArray();
                        totalCyclesSnapshot = _replayTotalCycles;
                    }

                    Log($"[OBDCloudManager] Replay: 完成事件 results={resultsSnapshot.Count} totalCycles={totalCyclesSnapshot}");
                    await SendReplayCompletedEventAsync(sessionId, terminationReason, resultsSnapshot, totalCyclesSnapshot).ConfigureAwait(false);
                }
                else
                {
                    await SendReplayFailedEventAsync(sessionId, terminationReason).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Replay: ✗ 异常 {ex.GetType().Name}: {ex.Message}");
                try { await SendReplayFailedEventAsync(sessionId, $"exception: {ex.Message}").ConfigureAwait(false); }
                catch { /* 忽略推送失败 */ }
            }
            finally
            {
                // ── finally：严格恢复所有临时修改 ────────────────────────
                Log("[OBDCloudManager] Replay: finally 开始恢复");

                // F1. 孤立 loopId（让 StartLoopV3 退出），同时设 Running = false
                if (obdReader != null && obdReaderType != null)
                {
                    try
                    {
                        var loopIdField = obdReaderType.GetField("loopId",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        loopIdField?.SetValue(obdReader, new Random().Next());

                        var setRunningMethod = obdReaderType.GetMethod("SetRunning",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        setRunningMethod?.Invoke(obdReader, new object[] { false, "BurstReplayFinally" });
                    }
                    catch (Exception fe) { Log($"[OBDCloudManager] Replay: finally loopId/Running 异常: {fe.Message}"); }
                }

                // F2. 恢复 NoDataLimit
                if (savedNoDataLimit >= 0 && noDataLimitProp != null && settingsCurrent != null)
                {
                    try
                    {
                        noDataLimitProp.SetValue(settingsCurrent, savedNoDataLimit);
                        Log($"[OBDCloudManager] Replay: NoDataLimit 恢复为 {savedNoDataLimit}");
                    }
                    catch (Exception fe) { Log($"[OBDCloudManager] Replay: finally NoDataLimit 异常: {fe.Message}"); }
                }

                // F3. 先恢复静态 WebSocketConnection / UseWebSocketConnection，再恢复实例 Connection
                if (staticHotSwapped && wsStaticField != null)
                {
                    try
                    {
                        wsStaticField.SetValue(null, savedStaticWsConnection);
                        Log($"[OBDCloudManager] Replay: 静态 WebSocketConnection 已恢复为 {savedStaticWsConnection?.GetType().Name ?? "null"}");
                    }
                    catch (Exception fe) { Log($"[OBDCloudManager] Replay: finally 静态 WebSocketConnection 恢复异常: {fe.Message}"); }
                }

                if (useWsStaticField != null && savedUseWsConnection.HasValue)
                {
                    try
                    {
                        useWsStaticField.SetValue(null, savedUseWsConnection.Value);
                        Log($"[OBDCloudManager] Replay: UseWebSocketConnection 已恢复为 {savedUseWsConnection.Value}");
                    }
                    catch (Exception fe) { Log($"[OBDCloudManager] Replay: finally UseWebSocketConnection 恢复异常: {fe.Message}"); }
                }

                if (hotSwapped && connectionField != null && obdReader != null && savedConnection != null)
                {
                    try
                    {
                        connectionField.SetValue(obdReader, savedConnection);
                        _originalConnection = null;
                        Log($"[OBDCloudManager] Replay: Connection 热切换回 {savedConnection.GetType().Name}");
                    }
                    catch (Exception fe) { Log($"[OBDCloudManager] Replay: finally Connection 恢复异常: {fe.Message}"); }
                }

                // F4. 清理 replay 状态
                _replayTcs = null;
                _originalConnection = null;
                _burstRecoveryInProgress = false;
                _burstDescriptorsJsonForReplay = null;
                _pendingReplayAbort = false; // 清竞态记账标志
                _isReplayActive = false; // 守卫清除，允许新 Prepare
                lock (_replayResultLock)
                {
                    _replayCollectedResults = null;
                    _replayDescriptorBindings = null;
                    _replayOpenEntries = null;
                    _replayTotalCycles = 0;
                }

                // F5. 恢复真实主循环（包含 UpdateOBDReaderRequests + StartLoopV3("BurstRestore")）
                if (obdReader != null && obdReaderType != null)
                {
                    try
                    {
                        // 短暂等待 StartLoopV3(BurstReplay) 退出
                        await Task.Delay(300).ConfigureAwait(false);
                        RestoreLoopSafe(obdReader, obdReaderType);
                        Log("[OBDCloudManager] Replay: ✓ RestoreLoopSafe 完成");
                    }
                    catch (Exception fe) { Log($"[OBDCloudManager] Replay: finally RestoreLoopSafe 异常: {fe.Message}"); }
                }

                if (replayRecoveryGuardEnabled)
                {
                    try
                    {
                        if (replayRecoveryGuardSetter != null)
                        {
                            replayRecoveryGuardSetter.Invoke(null, new object[] { savedReplayRecoveryGuard ?? false });
                        }
                        else
                        {
                            replayRecoveryGuardField?.SetValue(null, savedReplayRecoveryGuard ?? false);
                        }
                        Log($"[OBDCloudManager] Replay: ReplayRecoveryGuard 已恢复为 {savedReplayRecoveryGuard ?? false}");
                    }
                    catch (Exception fe)
                    {
                        Log($"[OBDCloudManager] Replay: finally ReplayRecoveryGuard 恢复异常: {fe.Message}");
                    }
                }

                _bluetoothConnection.SetReplayRecoveryGuard(false);
                Log("[OBDCloudManager] Replay: ◀ finally 完成");
            }
        }

        /// <summary>
        /// 基于 CurrentCarData.LiveDataPIDs 构建 pidId → PID 对象 映射。
        /// </summary>
        private Dictionary<int, object> BuildReplayPidLookup(object obdReader, Type obdReaderType)
        {
            var pidLookup = new Dictionary<int, object>();

            try
            {
                var carDataProp = obdReaderType.GetProperty("CurrentCarData",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var carData = carDataProp?.GetValue(obdReader);
                var liveDataPids = carData?.GetType().GetProperty("LiveDataPIDs")?.GetValue(carData);

                if (liveDataPids is System.Collections.IEnumerable liveEnum)
                {
                    foreach (var pid in liveEnum)
                    {
                        if (pid == null) continue;
                        var idProp = pid.GetType().GetProperty("Id");
                        if (idProp == null) continue;

                        try
                        {
                            pidLookup[Convert.ToInt32(idProp.GetValue(pid) ?? -1)] = pid;
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Replay: BuildReplayPidLookup 异常: {ex.Message}");
            }

            Log($"[OBDCloudManager] Replay: LiveDataPIDs 共 {pidLookup.Count} 个");
            return pidLookup;
        }

        private Dictionary<int, ReplayDescriptorBinding> BuildReplayDescriptorBindings(object obdReader, Type obdReaderType, JArray descriptors)
        {
            var bindings = new Dictionary<int, ReplayDescriptorBinding>();
            var pidLookup = BuildReplayPidLookup(obdReader, obdReaderType);

            if (descriptors == null)
                return bindings;

            foreach (var descToken in descriptors)
            {
                if (!(descToken is JObject desc)) continue;

                var seqId = desc.Value<int?>("seqId") ?? -1;
                if (seqId < 0) continue;

                var binding = new ReplayDescriptorBinding
                {
                    SeqId = seqId,
                    DoNotDecode = desc.Value<bool>("doNotDecode")
                };

                var pidIds = desc["pidIds"] as JArray ?? new JArray();
                var pidNames = desc["pidNames"] as JArray ?? new JArray();
                for (var index = 0; index < pidIds.Count; index++)
                {
                    var pidId = pidIds[index].Value<int>();
                    var hasPidObj = pidLookup.TryGetValue(pidId, out var pidObj);
                    string pidName = pidNames.Count > index
                        ? (pidNames[index]?.ToString() ?? $"PID_{pidId}")
                        : $"PID_{pidId}";
                    int unit = 0;
                    if (hasPidObj)
                    {
                        try
                        {
                            var pidType = pidObj.GetType();
                            pidName = (pidType.GetProperty("Name") ?? pidType.GetProperty("NM"))?.GetValue(pidObj)?.ToString() ?? pidName;
                            unit = Convert.ToInt32(pidType.GetProperty("Units")?.GetValue(pidObj) ?? 0);
                        }
                        catch { }
                    }
                    else
                    {
                        Log($"[OBDCloudManager] Replay: seqId={seqId} pidId={pidId} 未找到 PID 对象，退回 descriptor 名称绑定");
                    }

                    binding.Pids.Add(new ReplayPidBinding
                    {
                        PidId = pidId,
                        PidName = pidName,
                        MatchName = NormalizeReplayPidName(pidName),
                        Unit = unit,
                    });
                }

                bindings[seqId] = binding;
            }

            return bindings;
        }

        private void OnReplayEntryConsumed(string sessionId, JObject entry, int pointer)
        {
            if (entry == null)
                return;

            lock (_replayResultLock)
            {
                ForceFlushReplayEntriesBeforeAdvanceLocked(sessionId, $"advance:{pointer}");

                var seqId = entry.Value<int?>("seqId") ?? -1;
                ReplayDescriptorBinding descriptorBinding = null;
                _replayDescriptorBindings?.TryGetValue(seqId, out descriptorBinding);
                var pendingEntry = new ReplayPendingEntry
                {
                    Entry = entry,
                    Descriptor = descriptorBinding,
                };

                var entryTimeout = entry.Value<bool?>("timeout") ?? false;
                var completedBy = entry.Value<string>("completedBy") ?? "";
                if (descriptorBinding == null
                    || descriptorBinding.DoNotDecode
                    || descriptorBinding.Pids.Count == 0
                    || entryTimeout
                    || string.Equals(completedBy, "abort", StringComparison.OrdinalIgnoreCase))
                    pendingEntry.CompletionTcs.TrySetResult(true);

                _replayOpenEntries ??= new List<ReplayPendingEntry>();
                _replayOpenEntries.Add(pendingEntry);

                Log($"[OBDCloudManager] Replay: entry 命中 pointer={pointer} seqId={seqId} cycle={entry.Value<int?>("cycleIndex") ?? -1} ordinal={entry.Value<int?>("sampleOrdinal") ?? -1}");

                if (descriptorBinding == null)
                    Log($"[OBDCloudManager] Replay: ⚠ seqId={seqId} 缺少 descriptor 绑定");

                FlushReadyReplayEntriesLocked(sessionId, $"advance:{pointer}");
            }
        }

        private async Task SettleReplayOpenEntriesAsync(string sessionId, TimeSpan perEntryTimeout, string terminationReason)
        {
            while (true)
            {
                ReplayPendingEntry headEntry = null;

                lock (_replayResultLock)
                {
                    FlushReadyReplayEntriesLocked(sessionId, $"settle:{terminationReason}:ready");
                    headEntry = _replayOpenEntries != null && _replayOpenEntries.Count > 0
                        ? _replayOpenEntries[0]
                        : null;
                }

                if (headEntry == null)
                    return;

                var waitTask = headEntry.CompletionTcs.Task;
                var completedTask = await Task.WhenAny(waitTask, Task.Delay(perEntryTimeout)).ConfigureAwait(false);
                var settled = completedTask == waitTask;

                lock (_replayResultLock)
                {
                    FlushReadyReplayEntriesLocked(sessionId, settled
                        ? $"settle:{terminationReason}:captured"
                        : $"settle:{terminationReason}:timeout");

                    if (_replayOpenEntries != null
                        && _replayOpenEntries.Count > 0
                        && ReferenceEquals(_replayOpenEntries[0], headEntry))
                    {
                        if (!settled)
                            headEntry.ForcedIncomplete = true;

                        FlushReplayPendingEntryLocked(headEntry, sessionId, settled
                            ? $"settle:{terminationReason}:forced-complete"
                            : $"settle:{terminationReason}:forced-timeout");
                    }
                }
            }
        }

        private void TryCaptureReplayPidValueChanged(object normalizedPidEvent)
        {
            if (!_isReplayActive || normalizedPidEvent == null)
                return;

            var eventObj = normalizedPidEvent as JObject;
            if (eventObj == null && normalizedPidEvent is string eventStr)
            {
                try { eventObj = JObject.Parse(eventStr); } catch { }
            }
            if (eventObj == null)
                return;

            int? eventPidId = TryGetReplayEventPidId(eventObj);
            var eventPidName = NormalizeReplayPidName(
                eventObj.Value<string>("NM")
                ?? eventObj.Value<string>("name")
                ?? eventObj.Value<string>("Name")
                ?? eventObj.Value<string>("SNM")
                ?? eventObj.Value<string>("ShortName"));

            if (eventPidId == null && string.IsNullOrEmpty(eventPidName))
                return;

            lock (_replayResultLock)
            {
                if (_replayOpenEntries == null || _replayOpenEntries.Count == 0)
                    return;

                foreach (var pendingEntry in _replayOpenEntries)
                {
                    if (pendingEntry?.Descriptor == null)
                        continue;

                    ReplayPidBinding matchedBinding = null;
                    foreach (var pidBinding in pendingEntry.Descriptor.Pids)
                    {
                        if (IsReplayPidBindingMatch(pidBinding, eventPidId, eventPidName))
                        {
                            matchedBinding = pidBinding;
                            break;
                        }
                    }

                    if (matchedBinding == null)
                        continue;

                    if (pendingEntry.CapturedEventsByPidId.ContainsKey(matchedBinding.PidId))
                        continue;

                    pendingEntry.CapturedEventsByPidId[matchedBinding.PidId] = eventObj;
                    if (!string.IsNullOrEmpty(matchedBinding.MatchName))
                        pendingEntry.CapturedEventsByName[matchedBinding.MatchName] = eventObj;

                    Log($"[OBDCloudManager] Replay: 捕获 PID 回调 seqId={pendingEntry.Entry?.Value<int?>("seqId") ?? -1} pidId={matchedBinding.PidId} pidName={matchedBinding.PidName}");

                    if (IsReplayEntryComplete(pendingEntry))
                        pendingEntry.CompletionTcs.TrySetResult(true);

                    FlushReadyReplayEntriesLocked(pendingEntry.Entry?.Value<string>("sessionId") ?? "", $"pid:{matchedBinding.PidId}");
                    break;
                }
            }
        }

        private bool IsReplayPidBindingMatch(ReplayPidBinding pidBinding, int? eventPidId, string eventPidName)
        {
            if (pidBinding == null)
                return false;

            if (eventPidId.HasValue && pidBinding.PidId == eventPidId.Value)
                return true;

            return !string.IsNullOrEmpty(eventPidName)
                && !string.IsNullOrEmpty(pidBinding.MatchName)
                && string.Equals(pidBinding.MatchName, eventPidName, StringComparison.OrdinalIgnoreCase);
        }

        private int? TryGetReplayEventPidId(JObject eventObj)
        {
            if (eventObj == null)
                return null;

            int intVal;
            var idToken = eventObj["Id"] ?? eventObj["id"] ?? eventObj["PID"] ?? eventObj["pidId"];
            if (idToken == null)
                return null;

            if (idToken.Type == JTokenType.Integer)
                return idToken.Value<int>();

            return int.TryParse(idToken.ToString(), out intVal) ? intVal : (int?)null;
        }

        private string NormalizeReplayPidName(string name)
        {
            return string.IsNullOrWhiteSpace(name)
                ? ""
                : name.Trim().ToUpperInvariant();
        }

        private bool IsReplayEntryComplete(ReplayPendingEntry pendingEntry)
        {
            if (pendingEntry == null)
                return true;

            if (pendingEntry.CompletionTcs.Task.IsCompleted)
                return true;

            if (pendingEntry.Descriptor == null || pendingEntry.Descriptor.DoNotDecode)
                return true;

            if (pendingEntry.Descriptor.Pids.Count == 0)
                return true;

            foreach (var pidBinding in pendingEntry.Descriptor.Pids)
            {
                if (!pendingEntry.CapturedEventsByPidId.ContainsKey(pidBinding.PidId))
                    return false;
            }
            return true;
        }

        private void ForceFlushReplayEntriesBeforeAdvanceLocked(string sessionId, string reason)
        {
            if (_replayOpenEntries == null)
                return;

            while (_replayOpenEntries.Count > 0)
            {
                var headEntry = _replayOpenEntries[0];
                if (!IsReplayEntryComplete(headEntry))
                    headEntry.ForcedIncomplete = true;

                FlushReplayPendingEntryLocked(headEntry, sessionId, reason);
            }
        }

        private void FlushReadyReplayEntriesLocked(string sessionId, string reason)
        {
            if (_replayOpenEntries == null)
                return;

            while (_replayOpenEntries.Count > 0)
            {
                var headEntry = _replayOpenEntries[0];
                if (!IsReplayEntryComplete(headEntry))
                    break;

                FlushReplayPendingEntryLocked(headEntry, sessionId, reason);
            }
        }

        private void FlushReplayPendingEntryLocked(ReplayPendingEntry pendingEntry, string sessionId, string reason)
        {
            if (pendingEntry?.Entry == null)
                return;

            if (_replayOpenEntries != null)
                _replayOpenEntries.Remove(pendingEntry);

            var entry = pendingEntry.Entry;
            var descriptor = pendingEntry.Descriptor;
            var seqId = entry.Value<int?>("seqId") ?? -1;
            var cycleIndex = entry.Value<int?>("cycleIndex") ?? -1;
            var sampleOrdinal = entry.Value<int?>("sampleOrdinal") ?? -1;
            var timestampMs = entry.Value<long?>("timestampMs") ?? 0L;
            var rawElmText = entry.Value<string>("rawElmText") ?? "";
            var entrySessionId = entry.Value<string>("sessionId") ?? sessionId ?? "";
            var completedBy = entry.Value<string>("completedBy") ?? "";
            var timeout = entry.Value<bool?>("timeout") ?? false;

            if (descriptor == null || descriptor.Pids.Count == 0)
            {
                _replayCollectedResults?.Add(new JObject
                {
                    ["sessionId"] = entrySessionId,
                    ["seqId"] = seqId,
                    ["cycleIndex"] = cycleIndex,
                    ["sampleOrdinal"] = sampleOrdinal,
                    ["pidId"] = -1,
                    ["pidName"] = "",
                    ["value"] = JValue.CreateNull(),
                    ["rawValue"] = rawElmText,
                    ["unit"] = 0,
                    ["displayText"] = "",
                    ["timestampMs"] = timestampMs,
                    ["parseOk"] = false,
                    ["error"] = descriptor == null ? "missing_descriptor_binding" : "missing_pid_binding",
                });
                Log($"[OBDCloudManager] Replay: flush seqId={seqId} 无 descriptor/PID 绑定 reason={reason}");
                return;
            }

            string transportError = null;
            if (descriptor.DoNotDecode) transportError = "do_not_decode";
            else if (string.Equals(completedBy, "abort", StringComparison.OrdinalIgnoreCase)) transportError = "abort";
            else if (timeout) transportError = "timeout";

            foreach (var pidBinding in descriptor.Pids)
            {
                pendingEntry.CapturedEventsByPidId.TryGetValue(pidBinding.PidId, out var capturedEvent);

                var parseOk = capturedEvent != null
                    && string.IsNullOrEmpty(transportError);
                var parseError = parseOk ? null : (transportError
                    ?? (pendingEntry.ForcedIncomplete ? "missing_pid_update" : "missing_pid_event"));
                var displayText = "";
                JToken valueToken = JValue.CreateNull();
                var unit = pidBinding.Unit;

                if (parseOk)
                {
                    var rawValueToken = capturedEvent["Value"] ?? capturedEvent["value"] ?? capturedEvent["val"];
                    if (rawValueToken != null)
                    {
                        valueToken = rawValueToken.DeepClone();
                        displayText = rawValueToken.Type == JTokenType.String
                            ? rawValueToken.Value<string>() ?? ""
                            : rawValueToken.ToString(Formatting.None);
                    }

                    if (capturedEvent["Units"] != null || capturedEvent["unit"] != null || capturedEvent["Unit"] != null)
                    {
                        try
                        {
                            unit = Convert.ToInt32((capturedEvent["Units"] ?? capturedEvent["unit"] ?? capturedEvent["Unit"]).ToString());
                        }
                        catch { }
                    }
                }

                _replayCollectedResults?.Add(new JObject
                {
                    ["sessionId"] = entrySessionId,
                    ["seqId"] = seqId,
                    ["cycleIndex"] = cycleIndex,
                    ["sampleOrdinal"] = sampleOrdinal,
                    ["pidId"] = pidBinding.PidId,
                    ["pidName"] = pidBinding.PidName,
                    ["value"] = valueToken,
                    ["rawValue"] = rawElmText,
                    ["unit"] = unit,
                    ["displayText"] = displayText,
                    ["timestampMs"] = timestampMs,
                    ["parseOk"] = parseOk,
                    ["error"] = parseOk ? null : parseError,
                });
            }

            Log($"[OBDCloudManager] Replay: flush seqId={seqId} cycle={cycleIndex} ordinal={sampleOrdinal} pids={descriptor.Pids.Count} captured={pendingEntry.CapturedEventsByPidId.Count} forcedIncomplete={pendingEntry.ForcedIncomplete} reason={reason}");
        }

        /// <summary>
        /// 构建 replay bypassSet：ping + 所有 descriptor 的 beforeCommands + afterCommands
        /// （bypassSet 在初始化时一次性构建，不允许运行时动态扩充）
        /// </summary>
        private HashSet<string> BuildReplayBypassSet(JArray descriptors, string pingCommandText)
        {
            var bypassSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(pingCommandText))
                bypassSet.Add(pingCommandText.Trim());

            if (descriptors != null)
            {
                foreach (JObject desc in descriptors)
                {
                    var before = desc["beforeCommands"] as JArray;
                    var after = desc["afterCommands"] as JArray;
                    if (before != null)
                        foreach (var c in before) { var s = c.Value<string>()?.Trim(); if (!string.IsNullOrEmpty(s)) bypassSet.Add(s); }
                    if (after != null)
                        foreach (var c in after) { var s = c.Value<string>()?.Trim(); if (!string.IsNullOrEmpty(s)) bypassSet.Add(s); }
                }
            }
            return bypassSet;
        }

        /// <summary>
        /// 从 descriptor JSON 重建 OBDRequest 列表（descriptor-derived queue）。
        /// PID 对象来源：App.OBDReader.CurrentCarData.LiveDataPIDs（修正 4/Q1）。
        /// 所有 OBDRequest 的 SkipCyclesTarget = 0（MVP 扁平化）。
        /// </summary>
        private object BuildReplayQueue(object obdReader, Type obdReaderType, JArray descriptors)
        {
            try
            {
                var obdRequestType = FindType("CarScannerXamarinForms.OBD2.OBDRequest");
                var pidBaseType = FindType("CarScannerXamarinForms.OBD2.PIDS.PID");
                if (obdRequestType == null || pidBaseType == null)
                {
                    Log("[OBDCloudManager] Replay: BuildReplayQueue 关键类型未找到");
                    return null;
                }

                // LiveDataPIDs 来自 CurrentCarData（Q1）
                var carDataProp = obdReaderType.GetProperty("CurrentCarData",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var carData = carDataProp?.GetValue(obdReader);
                var liveDataPids = carData?.GetType().GetProperty("LiveDataPIDs")?.GetValue(carData);

                // 构建 pidId → PID 对象 查找字典
                var pidLookup = new System.Collections.Generic.Dictionary<int, object>();
                if (liveDataPids is System.Collections.IEnumerable liveEnum)
                {
                    foreach (var pid in liveEnum)
                    {
                        if (pid == null) continue;
                        var idP = pid.GetType().GetProperty("Id");
                        if (idP != null) pidLookup[(int)idP.GetValue(pid)] = pid;
                    }
                }
                Log($"[OBDCloudManager] Replay: LiveDataPIDs 共 {pidLookup.Count} 个");

                var pidListType = typeof(System.Collections.Generic.List<>).MakeGenericType(pidBaseType);
                var pidListAdd = pidListType.GetMethod("Add");
                var reqListType = typeof(System.Collections.Generic.List<>).MakeGenericType(obdRequestType);
                var reqList = Activator.CreateInstance(reqListType);
                var reqListAdd = reqListType.GetMethod("Add");

                // OBDRequest(Command, Header, BeforeCommands[], AfterCommands[], Repeat, List<PID>)
                var obdReqCtor = obdRequestType.GetConstructor(new Type[]
                {
                    typeof(string), typeof(string), typeof(string[]), typeof(string[]), typeof(bool), pidListType
                });
                if (obdReqCtor == null)
                {
                    Log("[OBDCloudManager] Replay: OBDRequest ctor 未找到");
                    return null;
                }

                var skipCyclesProp = obdRequestType.GetProperty("SkipCyclesTarget");
                var doNotDecodeProp = obdRequestType.GetProperty("DoNotDecode");
                var reqRespMarkerProp = obdRequestType.GetProperty("ResponseMarker");
                var reqCheckLenProp = obdRequestType.GetProperty("CheckLength");
                var reqElmFmtProp = obdRequestType.GetProperty("ELMFormat");
                var elmFormatEnum = FindType("CarScannerXamarinForms.OBD2.ELMFormat");

                foreach (JObject desc in descriptors)
                {
                    var command = desc.Value<string>("command") ?? "";
                    var header = desc.Value<string>("header") ?? "";
                    var repeat = desc.Value<bool>("repeat");
                    var doNotDecode = desc.Value<bool>("doNotDecode");
                    var beforeCmds = desc["beforeCommands"]?.ToObject<string[]>() ?? new string[0];
                    var afterCmds = desc["afterCommands"]?.ToObject<string[]>() ?? new string[0];
                    var pidIds = desc["pidIds"] as JArray ?? new JArray();
                    var descCheckLen = desc.Value<bool>("checkLength");
                    var descRespMarker = desc.Value<string>("responseMarker") ?? "";
                    var descElmFormat = desc.Value<string>("elmFormat") ?? "Unknown";

                    // 构建 PID 列表
                    var pidList = Activator.CreateInstance(pidListType);
                    foreach (var pidIdToken in pidIds)
                    {
                        int pidId = pidIdToken.Value<int>();
                        if (pidLookup.TryGetValue(pidId, out var pidObj))
                            pidListAdd.Invoke(pidList, new object[] { pidObj });
                    }

                    // 构造 OBDRequest
                    var req = obdReqCtor.Invoke(new object[] { command, header, beforeCmds, afterCmds, repeat, pidList });
                    if (req == null) continue;

                    // SkipCyclesTarget = 0（MVP 扁平化）
                    skipCyclesProp?.SetValue(req, 0);
                    doNotDecodeProp?.SetValue(req, doNotDecode);
                    if (descCheckLen) reqCheckLenProp?.SetValue(req, true);
                    if (!string.IsNullOrEmpty(descRespMarker)) reqRespMarkerProp?.SetValue(req, descRespMarker);
                    if (elmFormatEnum != null)
                    {
                        try
                        {
                            var fmtVal = Enum.Parse(elmFormatEnum, descElmFormat, true);
                            reqElmFmtProp?.SetValue(req, fmtVal);
                        }
                        catch { /* 忽略无法解析的 ELMFormat */ }
                    }

                    reqListAdd.Invoke(reqList, new object[] { req });
                }

                var reqCount = (int)reqListType.GetProperty("Count").GetValue(reqList);
                Log($"[OBDCloudManager] Replay: BuildReplayQueue 构建 {reqCount} 个 OBDRequest");
                return reqList;
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Replay: BuildReplayQueue 异常: {ex.Message}");
                return null;
            }
        }

        private void LogReplayQueueSnapshot(object obdReader, Type obdReaderType, string stage, int maxEntries)
        {
            try
            {
                var queueProp = obdReaderType.GetProperty("CommandQueue",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var queueObj = queueProp?.GetValue(obdReader);
                if (queueObj == null)
                {
                    Log($"[OBDCloudManager] ReplayDiag: {stage} CommandQueue=null");
                    return;
                }

                string countText = "?";
                var countProp = queueObj.GetType().GetProperty("Count");
                if (countProp != null)
                {
                    try
                    {
                        countText = (countProp.GetValue(queueObj)?.ToString()) ?? "?";
                    }
                    catch { }
                }

                Log($"[OBDCloudManager] ReplayDiag: {stage} queueType={queueObj.GetType().FullName} count={countText}");

                if (!(queueObj is System.Collections.IEnumerable enumerable))
                {
                    Log($"[OBDCloudManager] ReplayDiag: {stage} CommandQueue 不可枚举");
                    return;
                }

                int index = 0;
                foreach (var item in enumerable)
                {
                    if (index >= maxEntries) break;
                    if (item == null)
                    {
                        Log($"[OBDCloudManager] ReplayDiag: {stage} item[{index}]=null");
                        index++;
                        continue;
                    }

                    var itemType = item.GetType();
                    var command = itemType.GetProperty("Command")?.GetValue(item) as string ?? "";
                    var header = itemType.GetProperty("Header")?.GetValue(item) as string ?? "";
                    var repeat = itemType.GetProperty("Repeat")?.GetValue(item)?.ToString() ?? "?";
                    var doNotDecode = itemType.GetProperty("DoNotDecode")?.GetValue(item)?.ToString() ?? "?";
                    var before = itemType.GetProperty("BeforeCommands")?.GetValue(item) as string[] ?? Array.Empty<string>();
                    var after = itemType.GetProperty("AfterCommands")?.GetValue(item) as string[] ?? Array.Empty<string>();
                    var pidCount = "?";
                    var pidsObj = itemType.GetProperty("PIDs")?.GetValue(item);
                    var pidsCountProp = pidsObj?.GetType().GetProperty("Count");
                    if (pidsCountProp != null)
                    {
                        try
                        {
                            pidCount = pidsCountProp.GetValue(pidsObj)?.ToString() ?? "?";
                        }
                        catch { }
                    }

                    Log($"[OBDCloudManager] ReplayDiag: {stage} item[{index}] cmd=[{command}] header=[{header}] repeat={repeat} doNotDecode={doNotDecode} before={before.Length} after={after.Length} pidCount={pidCount}");
                    index++;
                }

                if (index == 0)
                {
                    Log($"[OBDCloudManager] ReplayDiag: {stage} queue is empty");
                }
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] ReplayDiag: {stage} queue snapshot 异常: {ex.Message}");
            }
        }

        private void LogReplayLoopState(object obdReader, Type obdReaderType, string stage)
        {
            try
            {
                string running = "?";
                var runningProp = obdReaderType.GetProperty("Running",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (runningProp != null)
                {
                    try { running = runningProp.GetValue(obdReader)?.ToString() ?? "?"; } catch { }
                }
                else
                {
                    var runningField = obdReaderType.GetField("_Running",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (runningField != null)
                    {
                        try { running = runningField.GetValue(obdReader)?.ToString() ?? "?"; } catch { }
                    }
                }

                string disconnectRequested = "?";
                var disconnectField = obdReaderType.GetField("DisconnectRequested",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (disconnectField != null)
                {
                    try { disconnectRequested = disconnectField.GetValue(obdReader)?.ToString() ?? "?"; } catch { }
                }

                var currentStatus = obdReaderType.GetProperty("CurrentStatus",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.GetValue(obdReader)?.ToString() ?? "?";
                var lastAction = obdReaderType.GetField("LastAction",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(obdReader)?.ToString() ?? "?";
                var connection = obdReaderType.GetField("Connection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(obdReader);
                var connectionType = connection?.GetType().FullName ?? "(null)";
                var queueProp = obdReaderType.GetProperty("CommandQueue",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var queueObj = queueProp?.GetValue(obdReader);
                var queueCount = queueObj?.GetType().GetProperty("Count")?.GetValue(queueObj)?.ToString() ?? "?";

                Log($"[OBDCloudManager] ReplayDiag: {stage} Running={running} DisconnectRequested={disconnectRequested} CurrentStatus={currentStatus} LastAction={lastAction} Connection={connectionType} QueueCount={queueCount}");
                LogReplayQueueSnapshot(obdReader, obdReaderType, stage, 3);
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] ReplayDiag: {stage} loop state 异常: {ex.Message}");
            }
        }

        private async Task SendReplayCompletedEventAsync(string sessionId, string reason, JArray results, int totalCycles)
        {
            try
            {
                await _bridge.SendAsync(new WSMessage
                {
                    Type = MessageType.Event,
                    Action = MessageAction.BurstReplayCompleted,
                    SessionId = sessionId,
                    Data = new JObject
                    {
                        ["sessionId"] = sessionId,
                        ["reason"] = reason,
                        ["results"] = results ?? new JArray(),
                        ["totalCycles"] = totalCycles,
                    }
                }).ConfigureAwait(false);
                Log($"[OBDCloudManager] Replay: ◀ BurstReplayCompleted 事件已推送 reason={reason} results={results?.Count ?? 0} totalCycles={totalCycles}");
            }
            catch (Exception ex) { Log($"[OBDCloudManager] Replay: BurstReplayCompleted 推送失败: {ex.Message}"); }
        }

        private async Task SendReplayFailedEventAsync(string sessionId, string reason)
        {
            try
            {
                await _bridge.SendAsync(new WSMessage
                {
                    Type = MessageType.Event,
                    Action = MessageAction.BurstReplayFailed,
                    SessionId = sessionId,
                    Data = new JObject { ["sessionId"] = sessionId, ["reason"] = reason }
                }).ConfigureAwait(false);
                Log($"[OBDCloudManager] Replay: ◀ BurstReplayFailed 事件已推送 reason={reason}");
            }
            catch (Exception ex) { Log($"[OBDCloudManager] Replay: BurstReplayFailed 推送失败: {ex.Message}"); }
        }


        private async Task HandleAbortBurstSessionAsync(WSMessage message)
        {
            try
            {
                var obj = message.GetData<JObject>();
                var sessionId = obj?.Value<string>("sessionId") ?? "";
                var reason = obj?.Value<string>("reason") ?? "unknown";

                Log($"[OBDCloudManager] ▶ AbortBurstSession reason={reason}");

                // 如果 replay 正在进行（_isReplayActive 或 _replayTcs 非空），通过 TCS 中止
                // replay 的 finally 负责恢复 Connection / NoDataLimit / loop，这里只发 ACK
                if (_isReplayActive || _replayTcs != null)
                {
                    if (string.IsNullOrEmpty(sessionId) || !string.Equals(sessionId, _burstSessionId, StringComparison.Ordinal))
                    {
                        Log($"[OBDCloudManager] Burst: ✗ Abort replay sessionId 不匹配 current={_burstSessionId ?? "<null>"} incoming={sessionId ?? "<null>"}");
                        await SendResponseAsync(message, false,
                            new JObject { ["currentSessionId"] = _burstSessionId ?? "" },
                            "sessionId 与当前 Burst Replay 会话不匹配").ConfigureAwait(false);
                        return;
                    }

                    Log("[OBDCloudManager] Burst: Abort 触发 replay 中止");
                    // 先记账：若 _replayTcs 尚未创建（2109→2223 竞态窗口），ExecuteReplayAsync 进入 try 后会检查此标志并短路
                    _pendingReplayAbort = true;
                    _replayTcs?.TrySetResult("user_abort");
                    await SendResponseAsync(message, true, new JObject { ["sessionId"] = sessionId, ["replayAborted"] = true }).ConfigureAwait(false);
                    return;
                }

                if (!_isBurstMode)
                {
                    await SendResponseAsync(message, true, new JObject { ["sessionId"] = sessionId, ["restored"] = true }).ConfigureAwait(false);
                    return;
                }

                if (string.IsNullOrEmpty(sessionId) || !string.Equals(sessionId, _burstSessionId, StringComparison.Ordinal))
                {
                    Log($"[OBDCloudManager] Burst: ✗ Abort snapshot sessionId 不匹配 current={_burstSessionId ?? "<null>"} incoming={sessionId ?? "<null>"}");
                    await SendResponseAsync(message, false,
                        new JObject { ["currentSessionId"] = _burstSessionId ?? "" },
                        "sessionId 与当前 Burst 会话不匹配").ConfigureAwait(false);
                    return;
                }

                _isBurstMode = false;
                _burstDescriptorsJson = null;
                _burstPendingSamples = null;
                _bluetoothConnection.SetBurstMode(false);
                _obdConnection?.SetBurstMode(false);
                _bluetoothConnection.ClearReceiveBuffer();
                _obdConnection?.ClearReceiveBuffer();

                try
                {
                    await SendResponseAsync(message, true, new JObject { ["sessionId"] = sessionId, ["restored"] = true }).ConfigureAwait(false);
                }
                catch (Exception respEx)
                {
                    Log($"[OBDCloudManager] Burst: abort 响应发送失败（继续恢复循环）: {respEx.Message}");
                }
                finally
                {
                    if (TryResolveObdReader(out var obdReader, out var obdReaderType))
                    {
                        RestoreLoopSafe(obdReader, obdReaderType);
                    }
                }

                Log("[OBDCloudManager] ◀ AbortBurstSession 完成");
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] ✗ AbortBurstSession 异常: {ex.Message}");

                if (_isBurstMode)
                {
                    _isBurstMode = false;
                    _burstDescriptorsJson = null;
                    _burstPendingSamples = null;
                    _bluetoothConnection.SetBurstMode(false);
                    _obdConnection?.SetBurstMode(false);
                    _bluetoothConnection.ClearReceiveBuffer();
                    _obdConnection?.ClearReceiveBuffer();

                    if (TryResolveObdReader(out var exReader, out var exType))
                    {
                        RestoreLoopSafe(exReader, exType);
                        Log("[OBDCloudManager] Burst: AbortBurstSession catch 中已恢复主循环");
                    }
                }

                try { await SendResponseAsync(message, false, null, ex.Message).ConfigureAwait(false); }
                catch (Exception respEx) { Log($"[OBDCloudManager] Burst: abort 错误响应发送失败: {respEx.Message}"); }
            }
        }

        /// <summary>
        /// 轮询停稳：硬门槛 Running==false && RWCycleEnded==true + 辅助 readSeq 停转 + IsQuiesced
        /// </summary>
        private async Task<bool> PollForQuiescence(object obdReader, Type obdReaderType, int timeoutMs)
        {
            var runningField = obdReaderType.GetField("_Running",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var rwCycleEndedField = obdReaderType.GetField("RWCycleEnded",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public);

            if (runningField == null || rwCycleEndedField == null)
            {
                Log($"[OBDCloudManager] Burst: 停稳轮询字段缺失 running={runningField != null} rwEnded={rwCycleEndedField != null}");
                return false;
            }

            var conn = _bluetoothConnection;
            int prevReadSeq = conn.ReadSeq;
            int readStableCount = 0;
            var sw = System.Diagnostics.Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                await Task.Delay(200).ConfigureAwait(false);

                var running = (bool)runningField.GetValue(obdReader);
                var rwEnded = (bool)rwCycleEndedField.GetValue(obdReader);

                // 硬门槛
                if (running || !rwEnded)
                {
                    readStableCount = 0;
                    prevReadSeq = conn.ReadSeq;
                    continue;
                }

                // 辅助确认：readSeq 停转
                int curReadSeq = conn.ReadSeq;
                if (curReadSeq == prevReadSeq)
                    readStableCount++;
                else
                {
                    readStableCount = 0;
                    prevReadSeq = curReadSeq;
                }

                // 四条件合并判定：硬门槛(running==F, rwEnded==T) + readSeq 稳定≥3次(600ms) + IsQuiesced(500)
                if (readStableCount >= 3 && conn.IsQuiesced(500))
                {
                    Log($"[OBDCloudManager] Burst: 停稳确认 elapsed={sw.ElapsedMilliseconds}ms readStable={readStableCount}");
                    return true;
                }
            }

            Log($"[OBDCloudManager] Burst: 停稳超时 {timeoutMs}ms");
            return false;
        }

        /// <summary>
        /// 后台延迟恢复：prepare 超时后不立即重启，等真正静默后再恢复
        /// </summary>
        private void StartDeferredRecovery(object obdReader, Type obdReaderType)
        {
            _burstRecoveryInProgress = true;
            _ = Task.Run(async () =>
            {
                try
                {
                    Log("[OBDCloudManager] Burst: 后台延迟恢复开始");

                    var runningField = obdReaderType.GetField("_Running",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var rwCycleEndedField = obdReaderType.GetField("RWCycleEnded",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                        | System.Reflection.BindingFlags.Public);

                    var conn = _bluetoothConnection;
                    int prevReadSeq = conn.ReadSeq;
                    int readStableCount = 0;
                    var sw = System.Diagnostics.Stopwatch.StartNew();

                    while (sw.ElapsedMilliseconds < 60000)
                    {
                        await Task.Delay(500).ConfigureAwait(false);

                        // 硬门槛：Running == false && RWCycleEnded == true
                        bool running = true;
                        bool rwEnded = false;
                        if (runningField != null) running = (bool)runningField.GetValue(obdReader);
                        if (rwCycleEndedField != null) rwEnded = (bool)rwCycleEndedField.GetValue(obdReader);

                        if (running || !rwEnded)
                        {
                            readStableCount = 0;
                            prevReadSeq = conn.ReadSeq;
                            continue;
                        }

                        // 辅助确认：readSeq 停转 + IsQuiesced
                        int curReadSeq = conn.ReadSeq;
                        if (curReadSeq == prevReadSeq)
                            readStableCount++;
                        else
                        {
                            readStableCount = 0;
                            prevReadSeq = curReadSeq;
                        }

                        if (readStableCount >= 2 && conn.IsQuiesced(500))
                        {
                            Log($"[OBDCloudManager] Burst: 延迟恢复 - 停稳确认 elapsed={sw.ElapsedMilliseconds}ms");
                            RestoreLoopSafe(obdReader, obdReaderType);
                            _burstRecoveryInProgress = false;
                            Log("[OBDCloudManager] Burst: 延迟恢复成功");
                            return;
                        }
                    }

                    // 60 秒仍未恢复 → 强制断开
                    Log("[OBDCloudManager] Burst: 延迟恢复超时 60s，强制断开");
                    try
                    {
                        var disconnectMethod = obdReaderType.GetMethod("Disconnect",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                            null, new Type[] { typeof(string) }, null);
                        if (disconnectMethod != null)
                        {
                            var taskObj = disconnectMethod.Invoke(obdReader, new object[] { "BurstTimeoutForceDisconnect" });
                            if (taskObj is Task t) await t.ConfigureAwait(false);
                        }
                    }
                    catch (Exception dex)
                    {
                        Log($"[OBDCloudManager] Burst: 强制断开异常: {dex.Message}");
                    }

                    _ = _bridge.SendAsync(WSMessageFactory.CreateOBDStatusChangedEvent("Disconnected"));
                    _burstRecoveryInProgress = false;
                    Log("[OBDCloudManager] Burst: 延迟恢复 → 已强制断开");
                }
                catch (Exception ex)
                {
                    Log($"[OBDCloudManager] Burst: 延迟恢复异常: {ex.Message}");
                    _burstRecoveryInProgress = false;
                }
            });
        }

        /// <summary>
        /// 恢复主循环：UpdateOBDReaderRequests + StartLoopV3
        /// </summary>
        private void RestoreLoopSafe(object obdReader, Type obdReaderType)
        {
            try
            {
                // UpdateOBDReaderRequests
                var rpsType = FindType("CarScannerXamarinForms.OBD2.RequestProducers.RequestProducerStatic");
                if (rpsType != null)
                {
                    var updateMethod = rpsType.GetMethod("UpdateOBDReaderRequests",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (updateMethod != null)
                    {
                        updateMethod.Invoke(null, null);
                        Log("[OBDCloudManager] Burst: UpdateOBDReaderRequests 完成");
                    }
                }

                // StartLoopV3
                var startLoopMethod = obdReaderType.GetMethod("StartLoopV3",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (startLoopMethod != null)
                {
                    startLoopMethod.Invoke(obdReader, new object[] { "BurstRestore" });
                    Log("[OBDCloudManager] Burst: StartLoopV3(BurstRestore) 调用完成");
                }
                else
                {
                    Log("[OBDCloudManager] Burst: ⚠ StartLoopV3 方法未找到");
                }
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Burst: RestoreLoopSafe 异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 按 pidIndices 从原版请求生成链生成 BurstRequestDescriptor[]
        /// 路径: _PIDCollection -> GetRequests -> Optimize -> 序列化
        /// </summary>
        private JArray GenerateBurstDescriptors(object obdReader, Type obdReaderType, int[] pidIndices)
        {
            try
            {
                // 获取 LiveDataPIDModel._PIDCollection
                var ldpmType = FindType("CarScannerXamarinForms.ViewModels.LiveDataPIDModel");
                if (ldpmType == null)
                {
                    Log("[OBDCloudManager] Burst: LiveDataPIDModel 类型未找到");
                    return null;
                }

                var pidCollectionField = ldpmType.GetField("_PIDCollection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
                    | System.Reflection.BindingFlags.Public);
                if (pidCollectionField == null)
                {
                    Log("[OBDCloudManager] Burst: _PIDCollection 字段未找到");
                    return null;
                }

                var pidCollection = pidCollectionField.GetValue(null);
                if (pidCollection == null)
                {
                    Log("[OBDCloudManager] Burst: _PIDCollection 为 null");
                    return null;
                }

                // pidCollection 是 ObservableCollection<PID>，通过索引器访问
                var collectionType = pidCollection.GetType();
                var countProp = collectionType.GetProperty("Count");
                var itemProp = collectionType.GetProperty("Item");
                int totalPids = (int)countProp.GetValue(pidCollection);
                Log($"[OBDCloudManager] Burst: PIDCollection 总数={totalPids}");

                // 构建请求列表 (List<OBDRequest>)
                var obdRequestType = FindType("CarScannerXamarinForms.OBD2.OBDRequest");
                if (obdRequestType == null)
                {
                    Log("[OBDCloudManager] Burst: OBDRequest 类型未找到");
                    return null;
                }
                var requestListType = typeof(System.Collections.Generic.List<>).MakeGenericType(obdRequestType);
                var requestList = Activator.CreateInstance(requestListType);

                // 对每个 pidIndex 调用 GetRequests
                var getRequestsMethod = ldpmType.GetMethod("GetRequests",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                    null, new Type[] {
                        FindType("CarScannerXamarinForms.OBD2.PIDS.IPID"),
                        requestListType,
                        typeof(string),
                        typeof(string)
                    }, null);

                if (getRequestsMethod == null)
                {
                    Log("[OBDCloudManager] Burst: GetRequests 方法未找到");
                    return null;
                }

                foreach (var idx in pidIndices)
                {
                    if (idx < 0 || idx >= totalPids) continue;
                    var pid = itemProp.GetValue(pidCollection, new object[] { idx });
                    if (pid == null) continue;
                    getRequestsMethod.Invoke(null, new object[] { pid, requestList, null, "" });
                }

                var listCount = (int)requestListType.GetProperty("Count").GetValue(requestList);
                Log($"[OBDCloudManager] Burst: GetRequests 生成 {listCount} 条原始请求");

                if (listCount == 0) return new JArray();

                // 调用 Optimize
                var optimizerType = FindType("CarScannerXamarinForms.OBD2.OBDRequestQueueOptimizer");
                if (optimizerType != null)
                {
                    var optimizeMethod = optimizerType.GetMethod("Optimize",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (optimizeMethod != null)
                    {
                        var optimized = optimizeMethod.Invoke(null, new object[] { requestList });
                        if (optimized != null)
                        {
                            requestList = optimized;
                            Log("[OBDCloudManager] Burst: Optimize 完成");
                        }
                    }
                }

                // 序列化为 JArray（传入 obdReader 用于获取 effectiveHeader / elmFormat）
                return SerializeDescriptors(requestList, obdRequestType, obdReader, obdReaderType);
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Burst: GenerateBurstDescriptors 异常: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        private JArray SerializeDescriptors(object requestEnumerable, Type obdRequestType, object obdReader, Type obdReaderType)
        {
            var result = new JArray();
            int seqId = 0;

            var commandProp = obdRequestType.GetProperty("Command");
            var headerProp = obdRequestType.GetProperty("Header");
            var beforeProp = obdRequestType.GetProperty("BeforeCommands");
            var afterProp = obdRequestType.GetProperty("AfterCommands");
            var checkLenProp = obdRequestType.GetProperty("CheckLength");
            var respMarkerProp = obdRequestType.GetProperty("ResponseMarker");
            var skipCyclesProp = obdRequestType.GetProperty("SkipCyclesTarget");
            var pidsProp = obdRequestType.GetProperty("PIDs");
            var elmFormatProp = obdRequestType.GetProperty("ELMFormat");
            // Replay 重建时必须的字段：Repeat 决定请求是否循环，DoNotDecode 跳过解码
            var repeatProp = obdRequestType.GetProperty("Repeat");
            var doNotDecodeProp = obdRequestType.GetProperty("DoNotDecode");

            var multiRequestType = FindType("CarScannerXamarinForms.OBD2.OBDMultiRequest");

            // 获取 GetDefaultHeader() 和 CurrentELMFormat 用于 descriptor 补充
            string defaultHeader = "";
            string currentElmFormat = "Unknown";
            try
            {
                var getDefaultHeaderMethod = obdReaderType.GetMethod("GetDefaultHeader",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (getDefaultHeaderMethod != null)
                    defaultHeader = getDefaultHeaderMethod.Invoke(obdReader, null) as string ?? "";

                var elmFmtProp = obdReaderType.GetProperty("CurrentELMFormat",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (elmFmtProp != null)
                    currentElmFormat = elmFmtProp.GetValue(obdReader)?.ToString() ?? "Unknown";
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Burst: 获取 defaultHeader/elmFormat 异常: {ex.Message}");
            }

            foreach (var request in (System.Collections.IEnumerable)requestEnumerable)
            {
                var command = commandProp?.GetValue(request) as string ?? "";
                var header = headerProp?.GetValue(request) as string ?? "";
                var before = beforeProp?.GetValue(request) as string[];
                var after = afterProp?.GetValue(request) as string[];
                var checkLen = checkLenProp?.GetValue(request) as bool? ?? false;
                var respMarker = respMarkerProp?.GetValue(request) as string ?? "";
                var skipCycles = (int)(skipCyclesProp?.GetValue(request) ?? 0);
                // repeat=true 的请求会被 StartLoopV3 自动重入队；DoNotDecode 跳过 DecodeData
                var repeat = repeatProp != null ? (bool)(repeatProp.GetValue(request) ?? false) : false;
                var doNotDecode = doNotDecodeProp != null ? (bool)(doNotDecodeProp.GetValue(request) ?? false) : false;

                // ELMFormat：优先取 request 级别，fallback 到全局
                string reqElmFormat = currentElmFormat;
                if (elmFormatProp != null)
                {
                    var reqFmt = elmFormatProp.GetValue(request)?.ToString() ?? "Unknown";
                    if (reqFmt != "Unknown") reqElmFormat = reqFmt;
                }

                bool isMulti = multiRequestType != null && multiRequestType.IsInstanceOfType(request);

                // effectiveHeader：空 header 时使用 GetDefaultHeader() 的结果
                string effectiveHeader = string.IsNullOrEmpty(header) ? defaultHeader : header;

                // skipATSH：VwTp20 + Header=="000" 时不发 ATSH
                bool skipATSH = reqElmFormat == "VwTp20" && header == "000";

                // 提取 PID 信息
                var pidsObj = pidsProp?.GetValue(request);
                var pidIds = new JArray();
                var pidNames = new JArray();
                var constituentPidIds = new JArray();

                if (pidsObj is System.Collections.IEnumerable pidList)
                {
                    foreach (var pid in pidList)
                    {
                        if (pid == null) continue;
                        var pidType = pid.GetType();
                        var idProp = pidType.GetProperty("Id");
                        var nameProp = pidType.GetProperty("Name") ?? pidType.GetProperty("NM");
                        if (idProp != null) pidIds.Add((int)idProp.GetValue(pid));
                        if (nameProp != null) pidNames.Add(nameProp.GetValue(pid)?.ToString() ?? "");
                    }
                }

                if (isMulti)
                {
                    // OBDMultiRequest: 提取 Commands dict（subCmd→dataLength）和 PIDs dict（subCmd→PID list）
                    var commandsDictProp = multiRequestType.GetProperty("Commands");
                    var multiPidsDictProp = multiRequestType.GetProperty("PIDs",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
                        | System.Reflection.BindingFlags.DeclaredOnly);
                    var expLenProp = multiRequestType.GetProperty("ExpectedDataLength");

                    var commandsDict = commandsDictProp?.GetValue(request);
                    var multiPidsDict = multiPidsDictProp?.GetValue(request);

                    var subRequestsArr = new JArray();

                    if (commandsDict is System.Collections.IDictionary cmdsD && multiPidsDict is System.Collections.IDictionary pidsD)
                    {
                        foreach (System.Collections.DictionaryEntry entry in cmdsD)
                        {
                            var subCmd = entry.Key?.ToString() ?? "";
                            var subDataLen = Convert.ToInt32(entry.Value);
                            var subPidArr = new JArray();

                            if (pidsD.Contains(subCmd))
                            {
                                var subPidList = pidsD[subCmd] as System.Collections.IEnumerable;
                                if (subPidList != null)
                                {
                                    foreach (var sp in subPidList)
                                    {
                                        var idP = sp?.GetType().GetProperty("Id");
                                        var nmP = sp?.GetType().GetProperty("Name") ?? sp?.GetType().GetProperty("NM");
                                        if (idP != null)
                                        {
                                            int spId = (int)idP.GetValue(sp);
                                            subPidArr.Add(spId);
                                            // 同时填充顶层 pidIds / pidNames（基类 _PIDs 在 OBDMultiRequest 中为空）
                                            if (!pidIds.Any(t => t.Value<int>() == spId))
                                            {
                                                pidIds.Add(spId);
                                                pidNames.Add(nmP?.GetValue(sp)?.ToString() ?? "");
                                            }
                                        }
                                    }
                                }
                            }

                            subRequestsArr.Add(new JObject
                            {
                                ["command"] = subCmd,
                                ["pidIds"] = subPidArr,
                                ["expectedDataLength"] = subDataLen
                            });

                            constituentPidIds.Add(subPidArr);
                        }
                    }

                    // subRequestsArr 和 expLen 暂存，等 desc 创建后再赋值
                    var multiSubRequests = subRequestsArr;
                    int multiExpLen = 0;
                    if (expLenProp != null)
                        multiExpLen = Convert.ToInt32(expLenProp.GetValue(request));

                    var desc = new JObject
                    {
                        ["seqId"] = seqId++,
                        ["command"] = command,
                        ["header"] = header,
                        ["effectiveHeader"] = effectiveHeader,
                        ["skipATSH"] = skipATSH,
                        ["elmFormat"] = reqElmFormat,
                        ["responseMarker"] = respMarker,
                        ["pidIds"] = pidIds,
                        ["pidNames"] = pidNames,
                        ["isMultiRequest"] = true,
                        ["repeat"] = repeat,
                        ["doNotDecode"] = doNotDecode,
                        ["source"] = "getRequests+optimize"
                    };

                    if (before != null && before.Length > 0) desc["beforeCommands"] = new JArray(before);
                    if (after != null && after.Length > 0) desc["afterCommands"] = new JArray(after);
                    if (checkLen) desc["checkLength"] = true;
                    if (skipCycles > 0) desc["skipCyclesTarget"] = skipCycles;
                    if (constituentPidIds.Count > 0) desc["constituentPidIds"] = constituentPidIds;
                    if (multiSubRequests.Count > 0) desc["subRequests"] = multiSubRequests;
                    if (multiExpLen > 0) desc["expectedResponseBytes"] = multiExpLen;

                    result.Add(desc);
                }
                else
                {
                    var desc = new JObject
                    {
                        ["seqId"] = seqId++,
                        ["command"] = command,
                        ["header"] = header,
                        ["effectiveHeader"] = effectiveHeader,
                        ["skipATSH"] = skipATSH,
                        ["elmFormat"] = reqElmFormat,
                        ["responseMarker"] = respMarker,
                        ["pidIds"] = pidIds,
                        ["pidNames"] = pidNames,
                        ["isMultiRequest"] = false,
                        ["repeat"] = repeat,
                        ["doNotDecode"] = doNotDecode,
                        ["source"] = "getRequests+optimize"
                    };

                    if (before != null && before.Length > 0) desc["beforeCommands"] = new JArray(before);
                    if (after != null && after.Length > 0) desc["afterCommands"] = new JArray(after);
                    if (checkLen) desc["checkLength"] = true;
                    if (skipCycles > 0) desc["skipCyclesTarget"] = skipCycles;

                    result.Add(desc);
                }
            }

            return result;
        }

        /// <summary>
        /// 历史遗留 helper：按旧方案离线重建 OBDRequest/OBDMultiRequest 后调用 DecodeData。
        /// 当前 Burst Replay MVP 主链不再使用该方法，仅保留作对照/排查。
        /// </summary>
        private JArray ParseBurstSamples(JArray samples)
        {
            var results = new JArray();

            if (!TryResolveObdReader(out var obdReader, out var obdReaderType))
                return results;

            var carDataProp = obdReaderType.GetProperty("CurrentCarData",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var carData = carDataProp?.GetValue(obdReader);
            if (carData == null) { Log("[OBDCloudManager] Burst: CurrentCarData 为 null"); return results; }

            var liveDataPids = carData.GetType().GetProperty("LiveDataPIDs")?.GetValue(carData);
            if (liveDataPids == null) return results;

            var obdRequestType = FindType("CarScannerXamarinForms.OBD2.OBDRequest");
            var multiRequestType = FindType("CarScannerXamarinForms.OBD2.OBDMultiRequest");
            var pidBaseType = FindType("CarScannerXamarinForms.OBD2.PIDS.PID");
            var elmFormatEnum = FindType("CarScannerXamarinForms.OBD2.ELMFormat");
            if (obdRequestType == null || pidBaseType == null) { Log("[OBDCloudManager] Burst: 关键类型未找到"); return results; }

            // DecodeData(string data, OBDRequest request, string override_header)
            var decodeDataMethod = obdReaderType.GetMethod("DecodeData",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                null, new Type[] { typeof(string), obdRequestType, typeof(string) }, null);
            if (decodeDataMethod == null) { Log("[OBDCloudManager] Burst: DecodeData 未找到"); return results; }

            var pidListType = typeof(System.Collections.Generic.List<>).MakeGenericType(pidBaseType);
            var pidListAddMethod = pidListType.GetMethod("Add");

            // OBDRequest 构造: (string Command, bool Repeat, List<PID> pids)
            var obdReqCtor = obdRequestType.GetConstructor(new Type[] { typeof(string), typeof(bool), pidListType });
            // OBDMultiRequest 构造: (List<OBDRequest>, bool, int)
            var obdReqListType = typeof(System.Collections.Generic.List<>).MakeGenericType(obdRequestType);
            System.Reflection.ConstructorInfo multiReqCtor = null;
            if (multiRequestType != null)
                multiReqCtor = multiRequestType.GetConstructor(new Type[] { obdReqListType, typeof(bool), typeof(int) });

            // 属性 setter 缓存
            var reqHeaderProp = obdRequestType.GetProperty("Header");
            var reqBeforeProp = obdRequestType.GetProperty("BeforeCommands");
            var reqAfterProp = obdRequestType.GetProperty("AfterCommands");
            var reqElmFmtProp = obdRequestType.GetProperty("ELMFormat");
            var reqCheckLenProp = obdRequestType.GetProperty("CheckLength");
            var reqRespMarkerProp = obdRequestType.GetProperty("ResponseMarker");

            foreach (var sampleToken in samples)
            {
                try
                {
                    var sample = sampleToken as JObject;
                    if (sample == null) continue;

                    var seqId = sample.Value<int>("seqId");
                    var cycleIndex = sample.Value<int>("cycleIndex");
                    var sampleOrdinal = sample.Value<int>("sampleOrdinal");
                    var rawElmText = sample.Value<string>("rawElmText") ?? "";
                    var timestampMs = sample.Value<long>("timestampMs");
                    var isTimeout = sample.Value<bool>("timeout");
                    if (isTimeout || string.IsNullOrEmpty(rawElmText)) continue;

                    JObject desc = null;
                    if (_burstDescriptorsJson != null && seqId < _burstDescriptorsJson.Count)
                        desc = _burstDescriptorsJson[seqId] as JObject;
                    if (desc == null) continue;

                    var command = desc.Value<string>("command") ?? "";
                    var header = desc.Value<string>("header") ?? "";
                    var isMulti = desc.Value<bool>("isMultiRequest");
                    var descElmFormat = desc.Value<string>("elmFormat") ?? "Unknown";
                    var descRespMarker = desc.Value<string>("responseMarker") ?? "";
                    var descCheckLen = desc.Value<bool>("checkLength");
                    var descBefore = desc["beforeCommands"]?.ToObject<string[]>();
                    var descAfter = desc["afterCommands"]?.ToObject<string[]>();
                    var descPidIds = desc["pidIds"] as JArray;
                    var descSubRequests = desc["subRequests"] as JArray;

                    // multi-request 的 PID 集合来自 subRequests 并集，不依赖顶层 pidIds
                    // 普通 request 的 PID 集合来自顶层 pidIds
                    var allPidInfos = new System.Collections.Generic.List<(int id, string name, int unit)>();

                    if (isMulti && descSubRequests != null && descSubRequests.Count > 0)
                    {
                        // 从 subRequests 收集所有 PID（并集去重）
                        var seenIds = new System.Collections.Generic.HashSet<int>();
                        foreach (var srToken in descSubRequests)
                        {
                            var srPidIds = (srToken as JObject)?["pidIds"] as JArray;
                            if (srPidIds == null) continue;
                            foreach (var spIdToken in srPidIds)
                            {
                                var spId = spIdToken.Value<int>();
                                if (!seenIds.Add(spId)) continue;
                                var pid = FindPidById(liveDataPids, spId);
                                if (pid == null) continue;
                                var pt = pid.GetType();
                                var nm = (pt.GetProperty("Name") ?? pt.GetProperty("NM"))?.GetValue(pid)?.ToString() ?? "";
                                int un = 0;
                                try { un = Convert.ToInt32(pt.GetProperty("Units")?.GetValue(pid) ?? 0); } catch { }
                                allPidInfos.Add((spId, nm, un));
                            }
                        }
                    }
                    else
                    {
                        // 普通 request：从顶层 pidIds
                        if (descPidIds == null || descPidIds.Count == 0) continue;
                        foreach (var pidIdToken in descPidIds)
                        {
                            var pidId = pidIdToken.Value<int>();
                            var pid = FindPidById(liveDataPids, pidId);
                            if (pid == null) continue;
                            var pt = pid.GetType();
                            var nm = (pt.GetProperty("Name") ?? pt.GetProperty("NM"))?.GetValue(pid)?.ToString() ?? "";
                            int un = 0;
                            try { un = Convert.ToInt32(pt.GetProperty("Units")?.GetValue(pid) ?? 0); } catch { }
                            allPidInfos.Add((pidId, nm, un));
                        }
                    }

                    if (allPidInfos.Count == 0) continue;
                    Log($"[OBDCloudManager] Burst: allPidInfos=[{string.Join(",", allPidInfos.Select(p => $"{p.id}:{p.name}"))}]");

                    // 解析 ELMFormat 枚举值
                    object elmFmtValue = null;
                    if (elmFormatEnum != null)
                    {
                        try { elmFmtValue = Enum.Parse(elmFormatEnum, descElmFormat, true); }
                        catch { try { elmFmtValue = Enum.Parse(elmFormatEnum, "Unknown", true); } catch { } }
                    }

                    object tempRequest = null;

                    if (isMulti && multiReqCtor != null)
                    {
                        // ── 重建 OBDMultiRequest ──
                        var subReqsJson = desc["subRequests"] as JArray;
                        if (subReqsJson != null && subReqsJson.Count > 0)
                        {
                            var subReqList = Activator.CreateInstance(obdReqListType);
                            var subListAddMethod = obdReqListType.GetMethod("Add");

                            foreach (var srToken in subReqsJson)
                            {
                                var srObj = srToken as JObject;
                                if (srObj == null) continue;

                                var subCmd = srObj.Value<string>("command") ?? "";
                                var subPidIdsArr = srObj["pidIds"] as JArray;

                                var subPidList = Activator.CreateInstance(pidListType);
                                if (subPidIdsArr != null)
                                {
                                    foreach (var spId in subPidIdsArr)
                                    {
                                        var pid = FindPidById(liveDataPids, spId.Value<int>());
                                        if (pid != null) pidListAddMethod.Invoke(subPidList, new object[] { pid });
                                    }
                                }

                                // 构造子请求 OBDRequest(subCmd, false, subPidList)
                                object subReq = null;
                                if (obdReqCtor != null)
                                    subReq = obdReqCtor.Invoke(new object[] { subCmd, false, subPidList });
                                if (subReq == null) continue;

                                reqHeaderProp?.SetValue(subReq, header);
                                if (descBefore != null) reqBeforeProp?.SetValue(subReq, descBefore);
                                if (descAfter != null) reqAfterProp?.SetValue(subReq, descAfter);
                                subListAddMethod.Invoke(subReqList, new object[] { subReq });
                            }

                            int expLen = desc.Value<int?>("expectedResponseBytes") ?? 1;
                            try
                            {
                                tempRequest = multiReqCtor.Invoke(new object[] { subReqList, false, expLen });
                            }
                            catch (Exception mrEx)
                            {
                                Log($"[OBDCloudManager] Burst: OBDMultiRequest 构造异常: {mrEx.InnerException?.Message ?? mrEx.Message}");
                            }
                        }
                    }

                    if (tempRequest == null)
                    {
                        // ── 重建普通 OBDRequest ──
                        var pidList = Activator.CreateInstance(pidListType);
                        foreach (var pi in allPidInfos)
                        {
                            var pid = FindPidById(liveDataPids, pi.id);
                            if (pid != null) pidListAddMethod.Invoke(pidList, new object[] { pid });
                        }

                        if (obdReqCtor != null)
                            tempRequest = obdReqCtor.Invoke(new object[] { command, false, pidList });
                        if (tempRequest == null) continue;
                    }

                    // ── 补齐所有影响 DecodeData 的属性 ──
                    reqHeaderProp?.SetValue(tempRequest, header);
                    if (descBefore != null) reqBeforeProp?.SetValue(tempRequest, descBefore);
                    if (descAfter != null) reqAfterProp?.SetValue(tempRequest, descAfter);
                    if (elmFmtValue != null) reqElmFmtProp?.SetValue(tempRequest, elmFmtValue);
                    if (descCheckLen) reqCheckLenProp?.SetValue(tempRequest, true);
                    if (!string.IsNullOrEmpty(descRespMarker)) reqRespMarkerProp?.SetValue(tempRequest, descRespMarker);

                    // ── 调用 DecodeData(rawElmText, request, null) ──
                    var cleanedElmText = rawElmText.Replace(">", "").TrimEnd();
                    bool decodeOk = false;
                    try
                    {
                        var taskResult = decodeDataMethod.Invoke(obdReader, new object[] { cleanedElmText, tempRequest, null });
                        if (taskResult != null)
                        {
                            var asTaskMethod = taskResult.GetType().GetMethod("AsTask");
                            if (asTaskMethod != null)
                            {
                                var task = asTaskMethod.Invoke(taskResult, null) as Task<bool>;
                                if (task != null) decodeOk = task.GetAwaiter().GetResult();
                            }
                        }
                    }
                    catch (Exception dex)
                    {
                        Log($"[OBDCloudManager] Burst: DecodeData 异常: {dex.InnerException?.Message ?? dex.Message}");
                        foreach (var pi in allPidInfos)
                        {
                            results.Add(new JObject
                            {
                                ["sessionId"] = _burstSessionId, ["seqId"] = seqId,
                                ["cycleIndex"] = cycleIndex, ["sampleOrdinal"] = sampleOrdinal,
                                ["pidId"] = pi.id, ["pidName"] = pi.name,
                                ["value"] = null, ["unit"] = pi.unit, ["displayText"] = "",
                                ["timestampMs"] = timestampMs, ["parseOk"] = false,
                                ["error"] = dex.InnerException?.Message ?? dex.Message
                            });
                        }
                        continue;
                    }

                    // ── 读取解析后的 PID Value ──
                    foreach (var pi in allPidInfos)
                    {
                        var pid = FindPidById(liveDataPids, pi.id);
                        if (pid == null)
                        {
                            Log($"[OBDCloudManager] Burst: FindPidById null pidId={pi.id} pidName={pi.name}");
                            continue;
                        }
                        var pt = pid.GetType();
                        var valueObj = pt.GetProperty("Value")?.GetValue(pid);
                        var displayText = valueObj?.ToString() ?? "";

                        JToken valueToken;
                        if (valueObj is double d) valueToken = d;
                        else if (valueObj is float f) valueToken = f;
                        else if (valueObj is int iv) valueToken = iv;
                        else if (valueObj is long lv) valueToken = lv;
                        else valueToken = displayText;

                        results.Add(new JObject
                        {
                            ["sessionId"] = _burstSessionId, ["seqId"] = seqId,
                            ["cycleIndex"] = cycleIndex, ["sampleOrdinal"] = sampleOrdinal,
                            ["pidId"] = pi.id, ["pidName"] = pi.name,
                            ["value"] = valueToken, ["rawValue"] = rawElmText,
                            ["unit"] = pi.unit, ["displayText"] = displayText,
                            ["timestampMs"] = timestampMs, ["parseOk"] = decodeOk
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log($"[OBDCloudManager] Burst: sample 解析异常: {ex.Message}");
                }
            }

            return results;
        }

        private object FindPidById(object liveDataPids, int pidId)
        {
            if (liveDataPids is System.Collections.IEnumerable enumerable)
            {
                foreach (var pid in enumerable)
                {
                    if (pid == null) continue;
                    var idProp = pid.GetType().GetProperty("Id");
                    if (idProp != null && (int)idProp.GetValue(pid) == pidId)
                        return pid;
                }
            }
            return null;
        }

        /// <summary>
        /// 按 StartLoopV3 内联逻辑（4-branch）生成 pingCommandText。
        /// 注意：不使用 GetPingRequest()，因为 StartLoopV3 内联不含 DaihatsuKLine 后缀。
        ///
        /// UseDefaultInit / Mode01Prefix / TesterPresentCommand 必须从 SharedSettings.Current 读取，
        /// 不得从 OBDDataReader 实例属性读取（OBDDataReader 上的同名属性可能是派生缓存，
        /// Settings 才是权威数据源）。
        ///
        /// 返回 null 表示生成失败（Prepare 必须 fail-closed，不能继续）。
        /// </summary>
        private string GeneratePingCommandText(object obdReader, Type obdReaderType)
        {
            try
            {
                // ── 1. 获取 SharedSettings.Current ──────────────────────────────
                var settingsType = FindType("CarScannerXamarinForms.Settings.SharedSettings");
                if (settingsType == null)
                {
                    Log("[OBDCloudManager] Burst: ✗ SharedSettings 类型未找到 → pingCommandText fail-closed");
                    return null;
                }

                var currentProp = settingsType.GetProperty("Current",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                var current = currentProp?.GetValue(null);
                if (current == null)
                {
                    Log("[OBDCloudManager] Burst: ✗ SharedSettings.Current 为 null → pingCommandText fail-closed");
                    return null;
                }

                var currentType = current.GetType();

                // ── 2. UseDefaultInit from SharedSettings.Current ────────────────
                var useDefaultInitProp = currentType.GetProperty("UseDefaultInit");
                if (useDefaultInitProp == null)
                {
                    Log("[OBDCloudManager] Burst: ✗ SharedSettings.Current.UseDefaultInit 未找到 → fail-closed");
                    return null;
                }
                bool useDefaultInit = (bool)(useDefaultInitProp.GetValue(current) ?? true);

                // ── 3. Mode01Prefix from SharedSettings.Current ──────────────────
                var mode01Prop = currentType.GetProperty("Mode01Prefix");
                if (mode01Prop == null)
                {
                    Log("[OBDCloudManager] Burst: ✗ SharedSettings.Current.Mode01Prefix 未找到 → fail-closed");
                    return null;
                }
                string mode01Prefix = (mode01Prop.GetValue(current) as string)?.Trim() ?? "";
                if (string.IsNullOrEmpty(mode01Prefix)) mode01Prefix = "01";

                // ── 4. TesterPresentCommand from SharedSettings.Current ───────────
                var testerProp = currentType.GetProperty("TesterPresentCommand");
                if (testerProp == null)
                {
                    Log("[OBDCloudManager] Burst: ✗ SharedSettings.Current.TesterPresentCommand 未找到 → fail-closed");
                    return null;
                }
                string testerPresent = (testerProp.GetValue(current) as string)?.Trim() ?? "";

                // ── 5. IsNissanConsult2Protocol from OBDDataReader（实例属性，不在 SharedSettings 中）
                bool isNissan = false;
                var isNissanProp = obdReaderType.GetProperty("IsNissanConsult2Protocol",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (isNissanProp != null)
                    isNissan = (bool)(isNissanProp.GetValue(obdReader) ?? false);

                // ── 6. 4-branch（与 StartLoopV3 内联逻辑一致，无 DaihatsuKLine 后缀）──
                if (useDefaultInit)
                {
                    string ping = isNissan ? "221201" : (mode01Prefix + "00");
                    Log($"[OBDCloudManager] Burst: pingCommandText={ping} (useDefaultInit={useDefaultInit} isNissan={isNissan} prefix={mode01Prefix})");
                    return ping;
                }
                else
                {
                    if (!string.IsNullOrEmpty(testerPresent))
                    {
                        Log($"[OBDCloudManager] Burst: pingCommandText={testerPresent} (TesterPresentCommand)");
                        return testerPresent;
                    }

                    // 分支 4：GetDefaultPidCommand()（OBDDataReader 方法，不是设置项）
                    var getDefaultPidMethod = obdReaderType.GetMethod("GetDefaultPidCommand",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic
                        | System.Reflection.BindingFlags.Instance);
                    string defaultPid = "";
                    if (getDefaultPidMethod != null)
                        defaultPid = (getDefaultPidMethod.Invoke(obdReader, null) as string)?.Trim() ?? "";

                    string fallback = string.IsNullOrEmpty(defaultPid) ? (mode01Prefix + "00") : defaultPid;
                    Log($"[OBDCloudManager] Burst: pingCommandText={fallback} (GetDefaultPidCommand fallback)");
                    return fallback;
                }
            }
            catch (Exception ex)
            {
                Log($"[OBDCloudManager] Burst: GeneratePingCommandText 异常: {ex.Message}");
                return null; // null = fail-closed，不能继续
            }
        }

        /// <summary>
        /// Bypass 冲突检测：检查 bypassSet（ping + 所有 beforeCommands + afterCommands）
        /// 是否与任何 descriptor.command 重叠。
        /// 返回 null 表示无冲突；返回 JObject 表示冲突详情（调用方填入 SendResponseAsync 的 data）。
        /// </summary>
        private JObject CheckBypassConflicts(JArray descriptors, string pingCommandText)
        {
            // 构建 bypassSet（一次性，不允许运行时扩充）
            var bypassSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(pingCommandText))
                bypassSet.Add(pingCommandText.Trim());

            foreach (JObject desc in descriptors)
            {
                var before = desc["beforeCommands"] as JArray;
                var after = desc["afterCommands"] as JArray;
                if (before != null)
                    foreach (var c in before) { var s = c.Value<string>()?.Trim(); if (!string.IsNullOrEmpty(s)) bypassSet.Add(s); }
                if (after != null)
                    foreach (var c in after) { var s = c.Value<string>()?.Trim(); if (!string.IsNullOrEmpty(s)) bypassSet.Add(s); }
            }

            var conflicts = new JArray();
            foreach (JObject desc in descriptors)
            {
                var cmd = desc.Value<string>("command")?.Trim();
                if (string.IsNullOrEmpty(cmd)) continue;
                if (!bypassSet.Contains(cmd)) continue;

                // 确定冲突来源
                string source;
                if (!string.IsNullOrEmpty(pingCommandText) &&
                    string.Equals(cmd, pingCommandText.Trim(), StringComparison.OrdinalIgnoreCase))
                    source = "pingCommandText";
                else
                    source = "beforeAfterCommands";

                conflicts.Add(new JObject
                {
                    ["seqId"] = desc.Value<int>("seqId"),
                    ["command"] = cmd,
                    ["conflictSource"] = source
                });
                Log($"[OBDCloudManager] Burst: bypass 冲突 seqId={desc.Value<int>("seqId")} cmd={cmd} src={source}");
            }

            if (conflicts.Count == 0) return null;

            return new JObject
            {
                ["conflictDetails"] = conflicts,
                ["bypassSetSample"] = new JArray(bypassSet.Take(8).Cast<object>().ToArray())
            };
        }

        /// <summary>
        /// lowPriority 冲突检测（全量 BFS + visited set + fail-closed）。
        /// 对 pidIndices 中每个 PID，递归遍历其 RequiredPIDs 树，
        /// 若找到 CalculatedPIDV2 有非空 LowPriorityRequiredPIDs 则记录冲突。
        /// 若遇到 CalculatedPIDV2 但无法读取其 LP 字段（属性+私有字段均失败），则 fail-closed。
        /// 返回 null = 无冲突可继续；返回含 "failClosed"=true 的对象 = 检测失败需 Prepare 拒绝；
        /// 返回含 "conflictingPids"=[] 的对象 = 有实际冲突。
        /// </summary>
        private JObject CheckLowPriorityConflicts(int[] pidIndices, object pidCollection, Type collectionType)
        {
            var calcV2Type = FindType("CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs.CalculatedPIDV2");
            if (calcV2Type == null)
            {
                // 找不到 CalculatedPIDV2 类型 → 系统中不存在此类 PID，无冲突
                Log("[OBDCloudManager] Burst: CalculatedPIDV2 类型未找到，跳过 lowPriority 检测");
                return null;
            }

            var countProp = collectionType.GetProperty("Count");
            var itemProp = collectionType.GetProperty("Item");
            int totalPids = countProp != null ? (int)countProp.GetValue(pidCollection) : 0;

            // 收集选中的 PID 对象
            var selectedPids = new System.Collections.Generic.List<(int rootIdx, object pid)>();
            foreach (var idx in pidIndices)
            {
                if (idx < 0 || idx >= totalPids) continue;
                var pid = itemProp?.GetValue(pidCollection, new object[] { idx });
                if (pid != null) selectedPids.Add((idx, pid));
            }

            var conflictingPids = new JArray();
            bool failClosed = false;

            foreach (var (rootIdx, rootPid) in selectedPids)
            {
                // 为每个根 PID 做独立的 BFS（公用 visited 防止跨根 PID 的重复遍历）
                var visited = new HashSet<object>();

                CollectLpConflictsForPid(
                    rootPid, rootPid, calcV2Type,
                    visited, conflictingPids, ref failClosed);

                if (failClosed)
                {
                    Log("[OBDCloudManager] Burst: lowPriority 检测 fail-closed（CalculatedPIDV2 LP 字段不可读）");
                    return new JObject { ["failClosed"] = true };
                }
            }

            if (conflictingPids.Count == 0) return null;

            return new JObject { ["conflictingPids"] = conflictingPids };
        }

        /// <summary>
        /// BFS 递归辅助：从 currentPid 出发，遍历 RequiredPIDs 树，
        /// 记录所有 CalculatedPIDV2 节点的 lowPriorityRequiredPIDs 冲突。
        /// failClosed = true 时立即停止（上层会处理）。
        /// </summary>
        private void CollectLpConflictsForPid(
            object rootPid, object currentPid, Type calcV2Type,
            HashSet<object> visited, JArray conflictingPids, ref bool failClosed)
        {
            if (failClosed) return;
            if (currentPid == null) return;
            if (!visited.Add(currentPid)) return; // 已访问

            var currentType = currentPid.GetType();
            bool isCalcV2 = calcV2Type.IsAssignableFrom(currentType);

            if (isCalcV2)
            {
                bool detectionAvailable = false;
                var lpPids = GetPidLowPriorityRequiredPIDs(currentPid, currentType, calcV2Type, out detectionAvailable);

                if (!detectionAvailable)
                {
                    // CalculatedPIDV2 但无法读取 LP 字段 → fail-closed
                    failClosed = true;
                    return;
                }

                if (lpPids.Count > 0)
                {
                    var lpInsertions = new JArray();
                    foreach (var lpPid in lpPids)
                    {
                        var lpType = lpPid.GetType();
                        var lpIdProp = lpType.GetProperty("Id");
                        var lpNameProp = lpType.GetProperty("Name") ?? lpType.GetProperty("NM");
                        var lpCmdProp = lpType.GetProperty("CMD") ?? lpType.GetProperty("Command");
                        lpInsertions.Add(new JObject
                        {
                            ["pidId"] = lpIdProp != null ? (int)lpIdProp.GetValue(lpPid) : -1,
                            ["pidName"] = lpNameProp?.GetValue(lpPid)?.ToString() ?? "",
                            ["command"] = lpCmdProp?.GetValue(lpPid)?.ToString() ?? ""
                        });
                        Log($"[OBDCloudManager] Burst: lowPriority 冲突 → LP pidId={lpIdProp?.GetValue(lpPid)} name={lpNameProp?.GetValue(lpPid)}");
                    }

                    var rootType = rootPid.GetType();
                    var srcType = currentType;
                    conflictingPids.Add(new JObject
                    {
                        ["selectedPidId"] = JToken.FromObject(rootType.GetProperty("Id")?.GetValue(rootPid) ?? -1),
                        ["selectedPidName"] = (rootType.GetProperty("Name") ?? rootType.GetProperty("NM"))?.GetValue(rootPid)?.ToString() ?? "",
                        ["conflictSourcePidId"] = JToken.FromObject(srcType.GetProperty("Id")?.GetValue(currentPid) ?? -1),
                        ["conflictSourcePidName"] = (srcType.GetProperty("Name") ?? srcType.GetProperty("NM"))?.GetValue(currentPid)?.ToString() ?? "",
                        ["lowPriorityInsertions"] = lpInsertions
                    });
                }
            }

            // 递归遍历 RequiredPIDs（CalculatedPIDV2 节点才有依赖树）
            // 若 CalculatedPIDV2 节点的 RequiredPIDs 两条路径均失败 → fail-closed
            var requiredPids = GetPidRequiredPIDs(currentPid, currentType, calcV2Type, out bool reqDetectionAvailable);
            if (!reqDetectionAvailable)
            {
                // CalculatedPIDV2 节点但依赖树不可读，无法保证检测完整性 → fail-closed
                failClosed = true;
                return;
            }
            foreach (var req in requiredPids)
            {
                if (failClosed) return;
                CollectLpConflictsForPid(rootPid, req, calcV2Type, visited, conflictingPids, ref failClosed);
            }
        }

        /// <summary>
        /// 获取 PID 的 lowPriorityRequiredPIDs（先公共属性，再私有字段 fallback）。
        /// detectionAvailable = false 时表示节点是 CalculatedPIDV2 但两条路径都失败（需 fail-closed）。
        /// </summary>
        private System.Collections.Generic.List<object> GetPidLowPriorityRequiredPIDs(
            object pid, Type pidType, Type calcV2Type, out bool detectionAvailable)
        {
            detectionAvailable = false;
            var result = new System.Collections.Generic.List<object>();

            // 1. 公共属性优先（LowPriorityRequiredPIDs）
            var lpProp = calcV2Type.GetProperty("LowPriorityRequiredPIDs",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.FlattenHierarchy);
            if (lpProp != null)
            {
                detectionAvailable = true;
                var val = lpProp.GetValue(pid);
                if (val is System.Collections.IEnumerable ie)
                    foreach (var item in ie) if (item != null) result.Add(item);
                return result;
            }

            // 2. Fallback：私有字段反射（lowPriorityRequiredPIDs）
            var lpField = calcV2Type.GetField("lowPriorityRequiredPIDs",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.FlattenHierarchy);
            if (lpField != null)
            {
                detectionAvailable = true;
                var val = lpField.GetValue(pid);
                if (val is System.Collections.IEnumerable ie)
                    foreach (var item in ie) if (item != null) result.Add(item);
                return result;
            }

            // 3. 两条路径均失败：detectionAvailable = false（调用方 fail-closed）
            Log("[OBDCloudManager] Burst: ⚠ CalculatedPIDV2.lowPriorityRequiredPIDs 公共属性与私有字段均未找到 → fail-closed");
            return result;
        }

        /// <summary>
        /// 获取 CalculatedPIDV2 节点的 RequiredPIDs 依赖列表（用于递归遍历）。
        /// 非 CalculatedPIDV2 节点：detectionAvailable = true，返回空列表（无依赖，正常）。
        /// CalculatedPIDV2 节点但两条路径均失败：detectionAvailable = false → 调用方 fail-closed。
        /// </summary>
        private System.Collections.Generic.List<object> GetPidRequiredPIDs(
            object pid, Type pidType, Type calcV2Type, out bool detectionAvailable)
        {
            var result = new System.Collections.Generic.List<object>();

            // 非 CalculatedPIDV2 节点：无依赖树，检测不适用，视为"可用且无依赖"
            if (!calcV2Type.IsAssignableFrom(pidType))
            {
                detectionAvailable = true;
                return result;
            }

            // 1. 公共属性优先（RequiredPIDs）
            var reqProp = calcV2Type.GetProperty("RequiredPIDs",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.FlattenHierarchy);
            if (reqProp != null)
            {
                detectionAvailable = true;
                var val = reqProp.GetValue(pid);
                if (val is System.Collections.IEnumerable ie)
                    foreach (var item in ie) if (item != null) result.Add(item);
                return result;
            }

            // 2. Fallback：私有字段（requiredPIDs）
            var reqField = calcV2Type.GetField("requiredPIDs",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.FlattenHierarchy);
            if (reqField != null)
            {
                detectionAvailable = true;
                var val = reqField.GetValue(pid);
                if (val is System.Collections.IEnumerable ie)
                    foreach (var item in ie) if (item != null) result.Add(item);
                return result;
            }

            // 3. 两条路径均失败：CalculatedPIDV2 节点但依赖树不可读 → fail-closed
            Log("[OBDCloudManager] Burst: ⚠ CalculatedPIDV2.RequiredPIDs 公共属性与私有字段均未找到 → fail-closed");
            detectionAvailable = false;
            return result;
        }

        private Type FindType(string fullName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var t = asm.GetType(fullName);
                    if (t != null) return t;
                }
                catch { }
            }
            return null;
        }

        private static byte[] HexToBytes(string hex)
        {
            hex = hex.Replace(" ", "").Replace("-", "");
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }

        #endregion
    }
}
