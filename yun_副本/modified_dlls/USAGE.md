# 修改后的DLL使用说明

## 新增字段

在 `AndroidBluetooth2Manager` 类中新增了以下静态字段：

```csharp
public static bool UseWebSocket = false;
public static string WebSocketServerUrl = "ws://0.0.0.0:8080/ws";
```

## 启用WebSocket模式

在应用启动时设置：

```csharp
AndroidBluetooth2Manager.UseWebSocket = true;
AndroidBluetooth2Manager.WebSocketServerUrl = "ws://0.0.0.0:8080/ws";
```

## 部署步骤

1. 将修改后的 `CarDemo.Android.dll` 复制到:
   `debug_decompiled/unknown/assemblies/CarDemo.Android.dll`

2. 运行 `automator.py` 重新打包APK

3. 安装APK到Android模拟器

4. 在手机上启动软件B（蓝牙网关，客户端）

5. 在手机端配置云端A的访问地址（例如 `ws://云端IP:8080/ws`）

## 注意事项

- 当前版本添加了UseWebSocket开关和基础框架
- 完整的WebSocket通信需要配合外部DLL (OBDCloud.WebSocket.dll)
- 后续版本将实现完整的IL代码注入
