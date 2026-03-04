# 构建云端化 APK 指南

## 前提条件

1. 已安装 Java JDK 17+
2. 已安装 Android SDK
3. 已安装 apktool

## 步骤 1：准备修补后的 DLL

DllPatcher 已成功修补 `CarScannerXamarinForms.dll`，添加了 WebSocket 连接支持。

修补后的文件：
- `yun/modified_dlls/CarScannerXamarinForms.patched.dll`

## 步骤 2：运行 automator.py

```bash
cd /Users/zed/Documents/Programe/CarUI/NewUI
python3 automator.py
```

automator.py 会：
1. 反编译原始 APK
2. 注入 React UI 到 `assets/dist/`
3. 注入修补后的 DLL：
   - `CarScannerXamarinForms.patched.dll` → `CarScannerXamarinForms.dll`（WebSocket 连接钩子）
   - `CarDemo.Android.dll`（WebSocket 初始化）
   - `CarDemo.dll`（UI 钩子）
4. 重新打包并签名 APK

## 步骤 3：安装到设备

```bash
adb install -r build_output/cloud-a-signed.apk
```

## 步骤 4：启动软件 B（蓝牙网关）

在手机 B 上运行 React Native 应用：

```bash
cd yun/client-app
npm start        # 终端 1
npm run android  # 终端 2
```

## 步骤 5：连接测试

1. 启动云端 A 的 CarScanner 应用
2. 在手机 B 的网关应用中连接云端 A 的 IP
3. 扫描并连接 ELM327 设备
4. 在云端 A 进行诊断操作

## 注入的 WebSocket 功能

修补后的 `CarScannerXamarinForms.dll` 包含：

| 字段/方法 | 类型 | 用途 |
|----------|------|------|
| `OBDDataReader.UseWebSocketConnection` | `static bool` | 启用/禁用 WebSocket 模式 |
| `OBDDataReader.WebSocketConnection` | `static object` | 存储 WebSocket 连接实例 |
| `OBDDataReader.InitializeWebSocket(connection)` | `static void` | 初始化 WebSocket 连接 |
| `OBDDataReader.DisableWebSocket()` | `static void` | 禁用 WebSocket 模式 |
| `OBDDataReader.IsWebSocketEnabled()` | `static bool` | 检查是否启用 |

当 `UseWebSocketConnection=true` 且 `WebSocketConnection!=null` 时，
所有 ELM327 连接操作会通过 WebSocket 转发到手机 B。

## 故障排除

### 问题：APK 安装失败
- 确保已卸载旧版本
- 检查签名是否正确

### 问题：WebSocket 连接失败
- 确保云端 A 和手机 B 在同一网络
- 检查防火墙设置
- 查看日志：`adb logcat | grep WebSocket`

### 问题：蓝牙连接失败
- 确保手机 B 已授予蓝牙权限
- 确保 ELM327 处于配对模式
