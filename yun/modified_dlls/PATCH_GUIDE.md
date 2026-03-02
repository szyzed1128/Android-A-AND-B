# DLL修补指南

## 概述

本指南描述如何将WebSocket通信层注入到原始CarDemo应用中，
使其以**A端服务器**方式运行，并通过WebSocket与手机端网关(软件B，客户端)通信，而非直接使用蓝牙。

## 改造文件

1. **CarDemo.Android.dll** - Android平台层，包含JSBridge和蓝牙管理器
2. **CarDemo.dll** - 跨平台业务逻辑（可能不需要修改）

## 改造方案

### 方案A: 运行时替换（推荐）

使用配置开关，在运行时决定使用蓝牙还是WebSocket：

```csharp
// 在AndroidBluetooth2Manager中
public static bool UseWebSocket { get; set; } = false;
public static string WebSocketServerUrl { get; set; } = "ws://0.0.0.0:8080/ws"; // A端监听地址

public async Task StartDiscoveringDevices()
{
    if (UseWebSocket)
    {
        await WebSocketDeviceDiscovery.StartScanAsync();
    }
    else
    {
        // 原始蓝牙实现
        await OriginalStartDiscoveringDevices();
    }
}
```

### 方案B: 完全替换

直接修改方法体，将所有蓝牙调用替换为WebSocket调用。
这种方案侵入性更大，但实现更简单。

## 具体修改点

### 1. JSBridge.callCSharpMethod

添加WebSocket初始化逻辑：

```csharp
public void callCSharpMethod(string data)
{
    // 新增: 初始化WebSocket连接
    if (!OBDCloudManager.IsInitialized)
    {
        // A端作为服务器启动监听
        OBDCloudManager.Initialize(WebSocketServerUrl);
    }

    // 原有逻辑...
    MainThread.BeginInvokeOnMainThread(() => {
        // ...
    });
}
```

### 2. AndroidBluetooth2Manager

替换扫描方法：

```csharp
// 原始
private BluetoothAdapter adapter;
public void StartDiscoveringDevices()
{
    adapter.StartDiscovery();
}

// 改造后
private OBDCloudManager cloudManager;
public async void StartDiscoveringDevices()
{
    cloudManager.DeviceDiscovered += OnDeviceDiscovered;
    await cloudManager.StartScanAsync();
}
```

### 3. BluetoothConnectionV3

替换连接和数据发送：

```csharp
// 原始
public async Task<bool> ConnectAsync(string device_id, PCLDebugStream debugStream)
{
    socket = device.CreateRfcommSocketToServiceRecord(SPP_UUID);
    await socket.ConnectAsync();
}

// 改造后
public async Task<bool> ConnectAsync(string device_id, PCLDebugStream debugStream)
{
    return await cloudManager.ConnectToDeviceAsync("bt", device_id);
}
```

## IL代码修改示例

使用dnlib修改IL代码：

```csharp
// 找到目标方法
var method = type.Methods.First(m => m.Name == "StartDiscoveringDevices");

// 清除原有指令
method.Body.Instructions.Clear();

// 插入新指令
method.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
method.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(cloudManagerField));
method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(startScanMethod));
method.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
```

## 依赖注入

修改后的DLL需要依赖：
- System.Net.WebSockets.dll
- Newtonsoft.Json.dll (已存在)

## 测试验证

1. 使用dnSpy打开修改后的DLL
2. 验证方法体是否正确修改
3. 部署到模拟器中测试

## 注意事项

- 修改前备份原始DLL
- 保持方法签名不变
- 确保异步方法的状态机正确生成
- 测试所有OBD功能路径
