using System;
using System.Threading.Tasks;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// 基于WebSocket的蓝牙设备发现服务
    /// 替换原始应用中的 AndroidBluetooth2Manager.StartDiscoveringDevices
    /// </summary>
    public class WebSocketDeviceDiscovery
    {
        private readonly IWebSocketBridge _bridge;
        private bool _isScanning;

        /// <summary>
        /// 发现设备时触发
        /// </summary>
        public event EventHandler<BTDeviceInfo> DeviceDiscovered;

        /// <summary>
        /// 扫描结束时触发
        /// </summary>
        public event EventHandler ScanFinished;

        /// <summary>
        /// 是否正在扫描
        /// </summary>
        public bool IsScanning => _isScanning;

        public WebSocketDeviceDiscovery(IWebSocketBridge bridge)
        {
            _bridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
            _bridge.MessageReceived += OnBridgeMessageReceived;
        }

        /// <summary>
        /// 开始扫描蓝牙设备
        /// 对应原始: IAdapter.StartScanningForDevicesAsync()
        /// </summary>
        public async Task StartScanAsync()
        {
            if (_isScanning) return;

            var request = WSMessageFactory.CreateStartScanRequest();
            var response = await _bridge.SendAndWaitAsync(request).ConfigureAwait(false);

            if (response.Success == true)
            {
                _isScanning = true;
            }
            else
            {
                throw new Exception($"启动扫描失败: {response.Error}");
            }
        }

        /// <summary>
        /// 停止扫描
        /// 对应原始: IAdapter.StopScanningForDevicesAsync()
        /// </summary>
        public async Task StopScanAsync()
        {
            if (!_isScanning) return;

            var request = WSMessageFactory.CreateStopScanRequest();
            var response = await _bridge.SendAndWaitAsync(request).ConfigureAwait(false);

            _isScanning = false;

            if (response.Success != true)
            {
                System.Diagnostics.Debug.WriteLine($"停止扫描警告: {response.Error}");
            }
        }

        /// <summary>
        /// 处理来自B端的消息
        /// </summary>
        private void OnBridgeMessageReceived(object sender, WSMessage message)
        {
            switch (message.Action)
            {
                case MessageAction.DeviceDiscovered:
                    var device = message.GetData<BTDeviceInfo>();
                    if (device != null)
                    {
                        DeviceDiscovered?.Invoke(this, device);
                    }
                    break;

                case MessageAction.ScanFinished:
                    _isScanning = false;
                    ScanFinished?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        /// <summary>
        /// 清理事件订阅
        /// </summary>
        public void Dispose()
        {
            _bridge.MessageReceived -= OnBridgeMessageReceived;
        }
    }
}
