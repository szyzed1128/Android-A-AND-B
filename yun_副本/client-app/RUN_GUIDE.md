# 软件B运行指南（B 为 WebSocket 客户端）

## 架构说明

```
云端A (Android模拟器)          手机B (本应用)           ELM327
┌─────────────────┐         ┌─────────────────┐      ┌─────────┐
│ CarScanner APK  │◄──WS───►│  React Native   │◄─BT─►│ 适配器  │
│ + WebSocket层   │         │  蓝牙网关       │      │         │
└─────────────────┘         └─────────────────┘      └─────────┘
     服务器                        客户端
```

- **云端A**：运行修改后的CarScanner APK，作为WebSocket服务器
- **手机B**：本应用，作为WebSocket客户端 + 蓝牙桥接
- **ELM327**：通过蓝牙连接到手机B

## 环境要求

- Node.js 18+
- Android Studio (带SDK)
- Android手机（开启USB调试）
- JDK 17+

## 快速开始

### 1. 安装依赖

```bash
cd yun/client-app
npm install
```

### 2. Android真机运行

**步骤1**：确保手机开启USB调试并连接电脑

```bash
adb devices
# 应该显示你的设备
```

**步骤2**：启动Metro Bundler

```bash
npm start
```

**步骤3**：安装到设备（另开终端）

```bash
npm run android
```

### 3. Android模拟器运行

**步骤1**：启动Android模拟器（从Android Studio）

**步骤2**：运行

```bash
npm start   # 终端1
npm run android   # 终端2
```

## 端到端测试流程

### 前置条件
1. 云端A已部署（Android模拟器 + 修改后的CarScanner APK）
2. 手机B已安装本应用
3. ELM327蓝牙适配器可用

### 测试步骤
1. 启动云端A的CarScanner应用（WebSocket服务器自动启动）
2. 在手机B的App中输入云端A的IP地址连接
3. 扫描蓝牙设备，选择ELM327
4. 连接ELM327设备
5. 在云端A的CarScanner中进行诊断操作
6. 验证数据通过 A→B→ELM327→B→A 链路正确传输

## 常见问题

### Q: Gradle构建失败

下载gradle-wrapper.jar：
```bash
cd android
# Windows
gradlew.bat wrapper --gradle-version=8.3
# Linux/Mac
./gradlew wrapper --gradle-version=8.3
```

### Q: SDK未找到

在 `android/local.properties` 中设置：
```
sdk.dir=C:\\Users\\你的用户名\\AppData\\Local\\Android\\Sdk
```

### Q: 蓝牙权限问题

确保在手机设置中授予应用蓝牙权限。

### Q: 连接云端A失败

1. 确保云端A和手机B在同一网络
2. 检查云端A的WebSocket服务器是否正常启动
3. 检查防火墙设置

## 功能验证清单

- [ ] 连接云端A的WebSocket服务器
- [ ] 蓝牙扫描（BLE/经典蓝牙）
- [ ] 连接ELM327设备
- [ ] 诊断数据收发（A→B→ELM327）
- [ ] 响应数据回传（ELM327→B→A）

## 项目结构

```
client-app/
├── src/
│   ├── App.tsx                    # 主界面
│   ├── bluetooth/                 # 蓝牙管理（三协议）
│   │   └── BluetoothManager.ts
│   ├── services/                  # 云端通信
│   │   └── CloudBridge.ts         # WebSocket客户端
│   ├── hooks/                     # React Hooks
│   │   ├── useBluetoothBridge.ts  # 蓝牙桥接
│   │   └── useLocalBluetooth.ts   # 本地蓝牙
│   └── protocol/                  # 消息协议
│       └── MessageProtocol.ts
└── android/                       # Android原生项目
```
