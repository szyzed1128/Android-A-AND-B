# 车辆诊断应用云端化改造

## 项目目标

将当前的 Android 车辆诊断应用改造成云端架构，实现：
- **软件A（云端）**：运行在云服务器 Android 模拟器中，包含完整的诊断业务逻辑和协议解析（支持 OBD-II 和 UDS 协议）
- **软件B（客户端）**：运行在用户手机上的跨平台应用，作为蓝牙网关和UI展示层

**核心改造**：A 端内置 WebSocket 服务器，B 作为客户端连接；A 的蓝牙通信层改造成 WebSocket 通信层，B 通过 WebSocket 转发蓝牙数据给 A。

---

## 架构设计

```
用户 ← UI展示 ← 软件B(手机) ← 蓝牙(经典/BLE/MFi) → ELM327
                    ↑
                    ↓ WebSocket (JSON + Base64)
软件A(云端Android模拟器)
              - React UI (业务逻辑)
              - C# 诊断协议解析 (OBD-II + UDS)
              - 蓝牙层 → WebSocket层 (改造点)
              - **WebSocket Server**
```

### 数据流

> 连接方向：**手机B(客户端) → 云端A(服务器)**

1. **云端A → 手机B**：A发送诊断命令请求（如读取车速），通过WebSocket传给B
2. **手机B → ELM327**：B通过蓝牙将命令发送给 ELM327 适配器
3. **ELM327 → 手机B**：ELM327 返回数据（如"41 0D 3C"）
4. **手机B → 云端A**：B将数据通过WebSocket回传给A，A进行协议解析和UI展示

---

## 蓝牙协议支持

软件B支持三种蓝牙连接方式：

| 协议 | 平台 | 依赖库 | 典型场景 |
|------|------|--------|----------|
| BLE | Android + iOS | react-native-ble-plx | 新款低功耗 ELM327 适配器 |
| 经典蓝牙 (SPP) | Android | react-native-bluetooth-classic | 传统 ELM327 适配器 |
| MFi | iOS | react-native-external-accessory | iOS 认证的经典蓝牙 ELM327 |

---

## 技术方案

### WebSocket通信协议

B与A之间使用JSON格式通信，二进制数据用Base64编码：

```json
{
  "type": "request|response|event",
  "action": "startScan|connect|send|obdData|...",
  "sessionId": "会话ID",
  "data": "Base64编码的诊断数据"
}
```

### DLL改造策略

采用**运行时替换**方案：
- 在 `AndroidBluetooth2Manager` 和 `BluetoothConnectionV3` 中，将蓝牙API调用替换为WebSocket调用
- 保持原有类接口不变，OBD协议解析层完全不动
- 通过静态开关 `UseWebSocket` 可在蓝牙/WebSocket模式间切换

### 改造点（已通过dnlib分析确认）

| 类名 | 方法名 | 改造内容 |
|------|--------|----------|
| JSBridge | callCSharpMethod | 添加WebSocket初始化 |
| AndroidBluetooth2Manager | StartDiscoveringDevices | 替换为WebSocket扫描请求 |
| AndroidBluetooth2Manager | StopDiscoveringDevices | 替换为WebSocket停止扫描 |
| AndroidBluetooth2Manager | DeviceDiscovered | 改为接收WebSocket事件 |
| BluetoothConnectionV3 | ConnectAsync | 替换为WebSocket连接请求 |

---

## 目录结构

```
yun/
├── analysis/                    # DLL分析工具
│   ├── Program.cs               # dnlib分析器
│   └── bluetooth_analysis_report.md
│
├── websocket_layer/             # C# WebSocket层（云端A使用）
│   ├── PROTOCOL.md              # 通信协议文档
│   └── src/
│       ├── WebSocketBridge.cs   # WebSocket客户端
│       ├── WebSocketDeviceDiscovery.cs
│       ├── WebSocketELM327Connection.cs
│       └── CloudManager.cs      # 统一入口
│
├── client-app/                  # React Native客户端（软件B）
│   └── src/
│       ├── bluetooth/BluetoothManager.ts  # 三协议蓝牙管理
│       ├── websocket/WebSocketServer.ts
│       └── gateway/ELM327Gateway.ts       # 核心网关
│
├── tools/DllPatcher/            # DLL修补工具
│
├── modified_dlls/               # 修补输出和指南
│   └── PATCH_GUIDE.md
│
└── deployment/                  # 云端部署配置
    ├── Dockerfile
    ├── docker-compose.yml
    └── nginx.conf
```

---

## 关键设计决策

1. **为什么用WebSocket而不是HTTP？**
   - 诊断通信是双向实时的，WebSocket支持服务端主动推送数据
   - 保持长连接，避免频繁建立连接的开销

2. **为什么A端运行WebSocket服务器，B作为客户端？**
   - B在用户手机上，IP不固定且经常处于NAT后；A在云端有固定地址
   - 由B主动连接A可避免穿透问题，且连接稳定

3. **为什么保留原有DLL结构而不是重写？**
   - CarScannerXamarinForms.dll包含大量诊断协议解析逻辑（OBD-II + UDS），重写成本高
   - 只改造通信层，最小化改动范围，降低风险
