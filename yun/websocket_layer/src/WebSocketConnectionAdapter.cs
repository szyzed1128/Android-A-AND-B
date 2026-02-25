using System;
using System.Threading.Tasks;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// WebSocket 连接适配器
    ///
    /// 这个类充当 WebSocketBluetoothConnection 和原软件 IOBDConnection 接口之间的桥梁。
    /// 它通过反射调用 WebSocketBluetoothConnection 的方法，避免了类型签名不匹配的问题。
    ///
    /// 关键问题：
    /// - IOBDConnection.ReadBytesAsync() 返回 ValueTask<byte[]>
    /// - WebSocketBluetoothConnection.ReadBytesAsync() 返回 Task<byte[]>
    /// - 由于 ValueTask 在 Mono/mscorlib 和 .NET Standard 中定义不同，
    ///   直接实现接口会导致 VTable 设置失败
    ///
    /// 解决方案：
    /// - 这个适配器使用反射来调用方法
    /// - 通过 dynamic 类型绕过编译时类型检查
    /// </summary>
    public class WebSocketConnectionAdapter
    {
        private readonly WebSocketBluetoothConnection _connection;

        // 缓存的方法信息
        private System.Reflection.MethodInfo _readBytesAsyncMethod;
        private System.Reflection.MethodInfo _writeBytesAsyncMethod;
        private System.Reflection.MethodInfo _flushAsyncMethod;
        private System.Reflection.MethodInfo _connectAsyncMethod;
        private System.Reflection.MethodInfo _disconectMethod;
        private System.Reflection.PropertyInfo _keepAliveProperty;
        private System.Reflection.PropertyInfo _noDelayProperty;
        private System.Reflection.PropertyInfo _connectedProperty;
        private System.Reflection.PropertyInfo _needFlushProperty;
        private System.Reflection.PropertyInfo _connectionTimeoutSecondsProperty;

        public WebSocketConnectionAdapter(WebSocketBluetoothConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            CacheMethodInfo();
        }

        private void CacheMethodInfo()
        {
            var type = typeof(WebSocketBluetoothConnection);
            _readBytesAsyncMethod = type.GetMethod("ReadBytesAsync");
            _writeBytesAsyncMethod = type.GetMethod("WriteBytesAsync");
            _flushAsyncMethod = type.GetMethod("FlushAsync");
            _connectAsyncMethod = type.GetMethod("ConnectAsync");
            _disconectMethod = type.GetMethod("Disconect");
            _keepAliveProperty = type.GetProperty("KeepAlive");
            _noDelayProperty = type.GetProperty("NoDelay");
            _connectedProperty = type.GetProperty("Connected");
            _needFlushProperty = type.GetProperty("NeedFlush");
            _connectionTimeoutSecondsProperty = type.GetProperty("ConnectionTimeoutSeconds");
        }

        /// <summary>
        /// 直接获取底层连接（用于需要完整功能的场景）
        /// </summary>
        public WebSocketBluetoothConnection UnderlyingConnection => _connection;

        #region IOBDConnection 接口方法（通过反射调用）

        public bool KeepAlive
        {
            get => (bool)_keepAliveProperty.GetValue(_connection);
            set => _keepAliveProperty.SetValue(_connection, value);
        }

        public bool NoDelay
        {
            get => (bool)_noDelayProperty.GetValue(_connection);
            set => _noDelayProperty.SetValue(_connection, value);
        }

        public bool Connected => (bool)_connectedProperty.GetValue(_connection);

        public bool NeedFlush => (bool)_needFlushProperty.GetValue(_connection);

        public int ConnectionTimeoutSeconds
        {
            get => (int)_connectionTimeoutSecondsProperty.GetValue(_connection);
            set => _connectionTimeoutSecondsProperty.SetValue(_connection, value);
        }

        /// <summary>
        /// 读取数据 - 返回 object 以便调用者可以 await
        /// 实际返回类型是 Task<byte[]>
        /// </summary>
        public object ReadBytesAsync()
        {
            return _readBytesAsyncMethod.Invoke(_connection, null);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        public object WriteBytesAsync(byte[] data)
        {
            return _writeBytesAsyncMethod.Invoke(_connection, new object[] { data });
        }

        /// <summary>
        /// 刷新缓冲区
        /// </summary>
        public object FlushAsync()
        {
            return _flushAsyncMethod.Invoke(_connection, null);
        }

        /// <summary>
        /// 连接到设备
        /// </summary>
        public object ConnectAsync(string deviceAddress, object debugStream = null)
        {
            return _connectAsyncMethod.Invoke(_connection, new object[] { deviceAddress, debugStream });
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconect()
        {
            _disconectMethod.Invoke(_connection, null);
        }

        #endregion
    }
}
