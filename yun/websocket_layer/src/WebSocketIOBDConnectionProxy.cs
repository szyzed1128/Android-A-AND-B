using System;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// 基于 WebSocket 的蓝牙连接代理
    /// 实现 IOBDConnection 接口，使原软件的所有 AT 命令通过 WebSocket 转发到手机 B
    /// </summary>
    public class WebSocketIOBDConnectionProxy : IOBDConnection
    {
        // 内部使用反射调用的目标对象
        private readonly object _target;
        private readonly Type _targetType;

        // 缓存的方法和属性反射信息
        private readonly System.Reflection.MethodInfo _readBytesAsyncMethod;
        private readonly System.Reflection.MethodInfo _writeBytesAsyncMethod;
        private readonly System.Reflection.MethodInfo _flushAsyncMethod;
        private readonly System.Reflection.MethodInfo _connectAsyncMethod;
        private readonly System.Reflection.MethodInfo _disconnectMethod;
        private readonly System.Reflection.PropertyInfo _keepAliveProperty;
        private readonly System.Reflection.PropertyInfo _noDelayProperty;
        private readonly System.Reflection.PropertyInfo _connectedProperty;
        private readonly System.Reflection.PropertyInfo _needFlushProperty;

        // ── Android logcat 支持 ──────────────────────────────────────
        private static System.Reflection.MethodInfo _androidLogMethod;
        private static bool _androidLogInitialized;
        private const string TAG = "WSProxy";

        private static void Log(string message)
        {
            Console.WriteLine($"[{TAG}] {message}");
            System.Diagnostics.Debug.WriteLine($"[{TAG}] {message}");

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
                                new[] { typeof(string), typeof(string) });
                            break;
                        }
                    }
                }
                catch { }
            }

            try { _androidLogMethod?.Invoke(null, new object[] { TAG, message }); }
            catch { }
        }
        // ────────────────────────────────────────────────────────────

        public WebSocketIOBDConnectionProxy(object target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _targetType = target.GetType();

            // 缓存反射信息并逐一记录是否找到
            _readBytesAsyncMethod  = _targetType.GetMethod("ReadBytesAsync");
            _writeBytesAsyncMethod = _targetType.GetMethod("WriteBytesAsync");
            _flushAsyncMethod      = _targetType.GetMethod("FlushAsync");
            _connectAsyncMethod    = _targetType.GetMethod("ConnectAsync");
            _disconnectMethod      = _targetType.GetMethod("Disconect");   // 原始拼写
            _keepAliveProperty     = _targetType.GetProperty("KeepAlive");
            _noDelayProperty       = _targetType.GetProperty("NoDelay");
            _connectedProperty     = _targetType.GetProperty("Connected");
            _needFlushProperty     = _targetType.GetProperty("NeedFlush");

            Log($"[构造] target={_targetType.FullName}");
            Log($"[构造] ReadBytesAsync   = {(_readBytesAsyncMethod  != null ? "✓" : "✗ NULL")}");
            Log($"[构造] WriteBytesAsync  = {(_writeBytesAsyncMethod != null ? "✓" : "✗ NULL")}");
            Log($"[构造] FlushAsync       = {(_flushAsyncMethod      != null ? "✓" : "✗ NULL")}");
            Log($"[构造] ConnectAsync     = {(_connectAsyncMethod    != null ? "✓" : "✗ NULL")}");
            Log($"[构造] Disconect        = {(_disconnectMethod      != null ? "✓" : "✗ NULL")}");
            Log($"[构造] Connected(prop)  = {(_connectedProperty     != null ? "✓" : "✗ NULL")}");
            Log($"[构造] KeepAlive(prop)  = {(_keepAliveProperty     != null ? "✓" : "✗ NULL")}");
            Log($"[构造] NoDelay(prop)    = {(_noDelayProperty       != null ? "✓" : "✗ NULL")}");
            Log($"[构造] NeedFlush(prop)  = {(_needFlushProperty     != null ? "✓" : "✗ NULL")}");
        }

        #region IOBDConnection 接口实现

        public bool KeepAlive
        {
            get
            {
                try { return (bool)_keepAliveProperty.GetValue(_target); }
                catch (Exception ex) { Log($"[KeepAlive.get] 异常: {ex.Message}"); return false; }
            }
            set
            {
                try { _keepAliveProperty.SetValue(_target, value); }
                catch (Exception ex) { Log($"[KeepAlive.set] 异常: {ex.Message}"); }
            }
        }

        public bool NoDelay
        {
            get
            {
                try { return (bool)_noDelayProperty.GetValue(_target); }
                catch (Exception ex) { Log($"[NoDelay.get] 异常: {ex.Message}"); return false; }
            }
            set
            {
                try { _noDelayProperty.SetValue(_target, value); }
                catch (Exception ex) { Log($"[NoDelay.set] 异常: {ex.Message}"); }
            }
        }

        public bool Connected
        {
            get
            {
                try
                {
                    if (_connectedProperty == null)
                    {
                        Log("[Connected] ✗ _connectedProperty 为 null，返回 false");
                        return false;
                    }
                    var val = (bool)_connectedProperty.GetValue(_target);
                    Log($"[Connected] = {val}");
                    return val;
                }
                catch (Exception ex)
                {
                    Log($"[Connected] 异常: {ex.Message}");
                    return false;
                }
            }
        }

        public bool NeedFlush
        {
            get
            {
                try
                {
                    if (_needFlushProperty == null) return false;
                    return (bool)_needFlushProperty.GetValue(_target);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 读取数据 — 关键：将 Task&lt;byte[]&gt; 转换为 ValueTask&lt;byte[]&gt;
        /// </summary>
        public ValueTask<byte[]> ReadBytesAsync()
        {
            Log("[ReadBytesAsync] ▶ 调用");
            try
            {
                if (_readBytesAsyncMethod == null)
                {
                    Log("[ReadBytesAsync] ✗ 方法为 null，返回空数组");
                    return new ValueTask<byte[]>(Array.Empty<byte>());
                }
                var task = (Task<byte[]>)_readBytesAsyncMethod.Invoke(_target, null);
                Log("[ReadBytesAsync] ✓ 已委托给 target");
                return new ValueTask<byte[]>(task);
            }
            catch (Exception ex)
            {
                Log($"[ReadBytesAsync] ✗ 异常: {ex.GetType().Name}: {ex.Message}");
                return new ValueTask<byte[]>(Array.Empty<byte>());
            }
        }

        /// <summary>
        /// 写入数据（AT 命令）
        /// </summary>
        public Task WriteBytesAsync(byte[] data)
        {
            string preview = "(null)";
            if (data != null)
            {
                try { preview = Encoding.ASCII.GetString(data).Replace("\r", "\\r").Replace("\n", "\\n"); }
                catch { preview = $"(bytes len={data.Length})"; }
            }
            Log($"[WriteBytesAsync] ▶ 调用 data=[{preview}] len={data?.Length ?? 0}");

            try
            {
                if (_writeBytesAsyncMethod == null)
                {
                    Log("[WriteBytesAsync] ✗ 方法为 null");
                    return Task.CompletedTask;
                }
                var result = (Task)_writeBytesAsyncMethod.Invoke(_target, new object[] { data });
                Log("[WriteBytesAsync] ✓ 已委托给 target");
                return result;
            }
            catch (Exception ex)
            {
                Log($"[WriteBytesAsync] ✗ 异常: {ex.GetType().Name}: {ex.Message}");
                // 返回已完成的 Task，避免 OBDDataReader 因异常而中断状态机
                return Task.FromException(ex);
            }
        }

        /// <summary>
        /// 刷新缓冲区
        /// </summary>
        public Task FlushAsync()
        {
            try
            {
                if (_flushAsyncMethod == null) return Task.CompletedTask;
                return (Task)_flushAsyncMethod.Invoke(_target, null);
            }
            catch (Exception ex)
            {
                Log($"[FlushAsync] 异常: {ex.Message}");
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 连接到设备
        /// </summary>
        public Task<bool> ConnectAsync(string host_port, PCLDebugStream debugStream)
        {
            Log($"[ConnectAsync] ▶ 调用 host_port={host_port}");
            try
            {
                if (_connectAsyncMethod == null)
                {
                    Log("[ConnectAsync] ✗ 方法为 null，返回 false");
                    return Task.FromResult(false);
                }
                var result = (Task<bool>)_connectAsyncMethod.Invoke(_target, new object[] { host_port, debugStream });
                Log("[ConnectAsync] ✓ 已委托给 target，等待结果...");
                // 包装结果以便记录返回值
                return LogConnectResult(result);
            }
            catch (Exception ex)
            {
                Log($"[ConnectAsync] ✗ 异常: {ex.GetType().Name}: {ex.Message}");
                return Task.FromResult(false);
            }
        }

        private async Task<bool> LogConnectResult(Task<bool> inner)
        {
            try
            {
                var ok = await inner.ConfigureAwait(false);
                Log($"[ConnectAsync] ◀ 结果={ok}");
                return ok;
            }
            catch (Exception ex)
            {
                Log($"[ConnectAsync] ◀ 内部异常: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconect()
        {
            Log("[Disconect] ▶ 调用");
            try
            {
                if (_disconnectMethod == null) { Log("[Disconect] ✗ 方法为 null"); return; }
                _disconnectMethod.Invoke(_target, null);
                Log("[Disconect] ✓ 完成");
            }
            catch (Exception ex)
            {
                Log($"[Disconect] ✗ 异常: {ex.GetType().Name}: {ex.Message}");
            }
        }

        #endregion
    }
}
