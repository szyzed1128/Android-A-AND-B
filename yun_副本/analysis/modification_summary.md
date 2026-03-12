# WebSocket改造点摘要

## 需要修改的关键位置

### 1. 蓝牙扫描改造点
- 原始: Plugin.BLE.Adapter.StartScanningForDevicesAsync()
- 改为: WebSocketBridge.SendAsync({action: 'startScan'})

### 2. 蓝牙连接改造点
- 原始: Plugin.BLE.Adapter.ConnectToKnownDeviceAsync()
- 改为: WebSocketBridge.SendAsync({action: 'connect', address: xxx})

### 3. 数据发送改造点
- 原始: ICharacteristic.WriteAsync(bytes)
- 改为: WebSocketBridge.SendAsync({action: 'send', data: base64})

### 4. 数据接收改造点
- 原始: ICharacteristic.ValueUpdated += handler
- 改为: WebSocketBridge.MessageReceived += handler

