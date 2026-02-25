# WebSocket 层集成指南

本文档说明如何将 WebSocket 层集成到原始 CarScanner 应用中。

## 架构概述

```
云端A (Android模拟器)                      手机B                    ELM327
┌─────────────────────────┐         ┌─────────────────┐      ┌─────────┐
│ CarScanner APK          │         │  React Native   │      │         │
│ ┌─────────────────────┐ │         │  蓝牙网关       │      │ 适配器  │
│ │ OBDDataReader       │ │         │                 │      │         │
│ │   ↓                 │ │◄──WS───►│                 │◄─BT─►│         │
│ │ WebSocketBluetooth  │ │         │                 │      │         │
│ │ Connection          │ │         │                 │      │         │
│ └─────────────────────┘ │         └─────────────────┘      └─────────┘
└─────────────────────────┘
```

## 步骤1：运行 DllPatcher

```bash
cd tools/DllPatcher
dotnet run -- <原始DLL路径> <输出DLL路径>

# 示例
dotnet run -- ../../CarDemo.Android.dll ../../modified_dlls/CarDemo.Android.patched.dll
```

DllPatcher 会：
1. 添加 `UseWebSocketConnection` 和 `WebSocketConnection` 静态字段
2. 添加 `InitializeWebSocket()`、`DisableWebSocket()`、`IsWebSocketEnabled()` 方法
3. 修改 `Connect()` 方法的 IL 代码，在每个 Connection 赋值前添加条件判断

## 步骤2：在 JSBridge 中初始化

在 JSBridge 构造函数或应用启动时添加以下代码：

```csharp
using OBDCloud.WebSocket;

// 在 JSBridge 构造函数中
public JSBridge(/* ... */)
{
    // 初始化 WebSocket 云端管理器
    InitializeWebSocketCloud();
}

private async void InitializeWebSocketCloud()
{
    try
    {
        // 设置 WebSocket 服务器监听地址
        OBDCloudManager.SetServerUrl("ws://0.0.0.0:8080/ws");

        // 初始化管理器
        await OBDCloudManager.InitializeIfNeededAsync("ws://0.0.0.0:8080/ws");

        // 注册 JSBridge 实例以启用 UI 桥接
        OBDCloudManager.Instance.RegisterJSBridge(this);

        // 将 WebSocketBluetoothConnection 注入到 OBDDataReader
        // 这会让所有蓝牙操作通过 WebSocket 转发到手机B
        var bluetoothConnection = OBDCloudManager.Instance.BluetoothConnection;

        // 使用反射调用注入的方法（因为编译时 OBDDataReader 还没有这个方法）
        var odrType = typeof(OBDDataReader);
        var initMethod = odrType.GetMethod("InitializeWebSocket",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

        if (initMethod != null)
        {
            initMethod.Invoke(null, new object[] { bluetoothConnection });
            System.Diagnostics.Debug.WriteLine("[JSBridge] WebSocket 蓝牙连接已注入");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("[JSBridge] 警告: 未找到 InitializeWebSocket 方法");
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"[JSBridge] WebSocket 初始化失败: {ex.Message}");
    }
}
```

## 步骤3：处理连接请求

当用户在 UI 中选择连接 ELM327 设备时，`OBDDataReader.Connect()` 方法会被调用。
由于 DllPatcher 已修改了该方法，它会：

1. 检查 `UseWebSocketConnection` 是否为 true
2. 检查 `WebSocketConnection` 是否不为 null
3. 如果两者都满足，使用 `WebSocketBluetoothConnection` 作为连接
4. 否则使用原始的蓝牙连接逻辑

## 数据流

### 发送命令（A → B → ELM327）
```
OBDDataReader.SendString("ATZ\r")
  → WebSocketBluetoothConnection.WriteBytesAsync()
  → WebSocket 发送到手机B
  → 手机B 通过蓝牙发送到 ELM327
```

### 接收响应（ELM327 → B → A）
```
ELM327 响应 "ELM327 v1.5\r\n>"
  → 手机B 蓝牙接收
  → 手机B 通过 WebSocket 发送到云端A
  → WebSocketBluetoothConnection.ReadBytesAsync() 返回数据
  → OBDDataReader 处理响应
```

## 故障排除

### WebSocket 服务器未启动
检查日志中是否有 `[OBDCloudManager] WebSocket服务器已启动` 消息。

### 连接仍使用本地蓝牙
确保：
1. DllPatcher 正确运行并生成了修改后的 DLL
2. 修改后的 DLL 已被打包到 APK 中
3. `InitializeWebSocket()` 在 `Connect()` 之前被调用

### 检查注入状态
```csharp
// 使用反射检查
var odrType = typeof(OBDDataReader);
var checkMethod = odrType.GetMethod("IsWebSocketEnabled",
    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

if (checkMethod != null)
{
    bool enabled = (bool)checkMethod.Invoke(null, null);
    System.Diagnostics.Debug.WriteLine($"WebSocket 已启用: {enabled}");
}
```

## 禁用 WebSocket 模式

如果需要切换回本地蓝牙模式：

```csharp
var odrType = typeof(OBDDataReader);
var disableMethod = odrType.GetMethod("DisableWebSocket",
    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

if (disableMethod != null)
{
    disableMethod.Invoke(null, null);
}
```
