# NewUI 交付与构建说明

## 1. 项目组成

本仓库目前包含四类内容：

1. **A 端服务器 APK 构建链路**
   - 基于原始 CarScanner APK 进行反编译、资源替换、DLL 注入、重新打包与签名。
   - 主要入口：
     - `automator.py`
     - `yun/tools/build_and_inject.sh`
     - `yun/modified_dlls/`

2. **Android / iOS 客户端（软件 B）**
   - React Native 工程目录：`yun/client-app`
   - Android 与 iOS 共用一套业务代码。

3. **调度层 / 实例层 / 部署链路**
   - 调度层：`yun/scheduler`
   - 实例 Agent：`yun/agent`
   - 部署脚本：`yun/deployment`

4. **A 端前端 UI 资源**
   - 源码：`react_ui`
   - 当前已构建并提交的注入资源：`my_source_code`

---

## 2. 克隆仓库后，必须额外准备的东西

### 2.1 必须额外提供

1. **原始 CarScanner APK**
   - 必须放到：
   - `original_apk/com.companyname.cardemo-Signed.apk`
   - 该文件不会提交到仓库。

2. **iOS 真机签名能力（如需真机/ipa）**
   - Apple Developer Team
   - 证书
   - Provisioning Profile

### 2.2 只在特定场景才需要额外准备

1. **如果要重新编译 DLL 注入层**
   - 需要先从原始 APK 中解出：
   - `temp_verify/unknown/assemblies`

2. **如果要部署完整调度系统**
   - 需要 Redis
   - 需要实例层机器环境
   - 需要部署时环境变量配置

---

## 3. 不需要交付、也不建议提交的文件

以下内容通常是本地生成物、缓存或机器相关配置，不需要额外交付：

- `temp_verify/`
- `temp_decompiled/`
- `temp_inspect/`
- `build_output/`
- `yun/client-app/node_modules/`
- `yun/client-app/ios/Pods/`
- `yun/client-app/android/local.properties`
- iOS / Android 的构建缓存、中间产物、DerivedData

---

## 4. 当前仓库已经包含的关键内容

当前仓库已经包含：

1. **A 端 DLL 修补产物**
   - `yun/modified_dlls/CarDemo.Android.dll`
   - `yun/modified_dlls/CarDemo.dll`
   - `yun/modified_dlls/CarScannerXamarinForms.patched.dll`
   - `yun/modified_dlls/OBDCloud.WebSocket.dll`
   - `yun/modified_dlls/WebSocketIOBDConnectionProxy.dll`

2. **A 端当前前端注入资源**
   - `my_source_code/`

3. **Android / iOS 客户端源码**
   - `yun/client-app/`

4. **A 端当前签名文件**
   - `release.keystore`

5. **Android 客户端当前 debug 签名**
   - `yun/client-app/android/app/debug.keystore`

这意味着：

- 如果只是构建**当前版本 A 端 APK**，不需要重新生成 DLL。
- 如果只是构建**当前 Android / iOS 客户端**，仓库代码本身已经足够。

---

## 5. 第一次拿到代码后，必须修改的硬编码

## 5.1 A 端构建机工具路径

必须修改 `automator.py` 中的本机绝对路径：

1. `automator.py`
   - `CMD_ZIPALIGN`
   - `CMD_APKSIGNER`

请改成你自己电脑上的 Android SDK Build-Tools 实际路径。

例如 macOS 常见位置：

```bash
/Users/<你的用户名>/Library/Android/sdk/build-tools/34.0.0/zipalign
/Users/<你的用户名>/Library/Android/sdk/build-tools/34.0.0/apksigner
```

## 5.2 A 端签名配置

如果你不想沿用当前仓库的签名，请修改 `automator.py` 中这些项：

- `KEYSTORE_PATH`
- `KEY_ALIAS`
- `KEY_STORE_PASS`
- `KEY_PASS`

如果要换签名文件，请同时替换根目录下的 `release.keystore`。

## 5.3 Android 客户端本地 SDK 路径

需要在本机创建：

- `yun/client-app/android/local.properties`

内容示例：

```properties
sdk.dir=/Users/<你的用户名>/Library/Android/sdk
```

该文件不提交，每个人都应该配置成自己机器的路径。

## 5.4 iOS 客户端 Debug 调试 IP

如果要做 iOS 真机 Debug，需要修改：

- `yun/client-app/ios/OBDGatewayClient/AppDelegate.mm`

其中当前写死了开发机局域网 IP，需要改成你自己的电脑 IP。

如果只构建 Release 包，该项不会影响最终打包。

## 5.5 iOS 签名信息

如果要做真机安装或导出 ipa，需要修改：

- `yun/client-app/ios/OBDGatewayClient.xcodeproj/project.pbxproj`

重点检查：

- `DEVELOPMENT_TEAM`
- `PRODUCT_BUNDLE_IDENTIFIER`

你必须改成自己 Apple 账号下可用的 Team 和包名。

## 5.6 调度后端地址规则

客户端当前默认把用户输入的 `cloudHost` 推导成：

```text
http://<cloudHost>:8080/api
```

如果你的调度后端不是这个地址规则，需要修改：

- `yun/client-app/src/hooks/useScheduler.ts`

## 5.7 Agent / 部署默认配置

如果你要部署实例层，需要检查：

- `yun/agent/src/config.ts`

至少需要根据实际环境修改或通过环境变量覆盖：

- `SERVER_ID`
- `AGENT_PORT`
- `SCHEDULER_URL`
- `PUBLIC_WS_HOST`
- `PUBLIC_WS_SCHEME`
- `INSTANCES_CONFIG`

---

## 6. 构建环境要求

## 6.1 A 端当前版本出包

需要：

- Python 3
- Java JDK 17+
- apktool
- Android SDK
- Android SDK Build-Tools 34.0.0

## 6.2 A 端未来重编 DLL 注入层

在上面基础上，还需要：

- .NET SDK
- Mono（需要 `mcs`）

## 6.3 A 端前端 UI 重建

如果你修改了 `react_ui`，还需要：

- Node.js
- npm

## 6.4 Android 客户端

需要：

- Node.js 18+
- npm
- JDK 17+
- Android Studio / Android SDK
- adb

## 6.5 iOS 客户端

需要：

- macOS
- Xcode
- CocoaPods

如果要真机/ipa，还需要：

- Apple Developer 签名环境

## 6.6 调度层 / 实例层

### 调度层

需要：

- Node.js
- npm
- Redis

### 实例层

按当前正式链路，建议准备：

- ARM64 Ubuntu
- Waydroid
- ADB
- Nginx

---

## 7. A 端当前版本如何构建（不重新改 DLL）

如果你只是想构建**当前仓库对应的 A 端 APK**，按下面做：

### 第 1 步：准备原始 APK

把原始 APK 放到：

- `original_apk/com.companyname.cardemo-Signed.apk`

### 第 2 步：检查工具路径

修改 `automator.py` 中的：

- `CMD_ZIPALIGN`
- `CMD_APKSIGNER`

### 第 3 步：如需换签名，修改签名配置

检查 `automator.py` 中的：

- `KEYSTORE_PATH`
- `KEY_ALIAS`
- `KEY_STORE_PASS`
- `KEY_PASS`

### 第 4 步：执行打包

```bash
python3 automator.py
```

### 第 5 步：查看产物

输出一般位于：

- `build_output/Release_com.companyname.cardemo-Signed.apk`

### 当前流程自动完成的事情

`automator.py` 会自动做：

1. 反编译原始 APK
2. 把 `my_source_code/` 注入到 APK 的 `assets/dist/`
3. 把 `yun/modified_dlls/` 中的关键 DLL 注入到 `unknown/assemblies/`
4. 重新打包 APK
5. 执行 `zipalign`
6. 执行签名

---

## 8. Android 客户端构建步骤

### 第 1 步：安装依赖

```bash
cd yun/client-app
npm install
```

### 第 2 步：配置 Android SDK 路径

创建：

- `yun/client-app/android/local.properties`

示例：

```properties
sdk.dir=/Users/<你的用户名>/Library/Android/sdk
```

### 第 3 步：构建 APK

```bash
cd yun/client-app/android
./gradlew assembleRelease
```

产物一般位于：

- `yun/client-app/android/app/build/outputs/apk/release/app-release.apk`

### 说明

当前 Android Release 默认仍使用 debug 签名，适合开发验证，不是正式商店发布配置。

---

## 9. iOS 客户端构建步骤

### 第 1 步：安装依赖

```bash
cd yun/client-app
npm install
```

### 第 2 步：安装 Pods

```bash
cd yun/client-app/ios
pod install
```

### 第 3 步：如需真机 Debug，修改开发机局域网 IP

修改：

- `yun/client-app/ios/OBDGatewayClient/AppDelegate.mm`

### 第 4 步：如需真机或 ipa，修改签名信息

修改：

- `yun/client-app/ios/OBDGatewayClient.xcodeproj/project.pbxproj`

### 第 5 步：在 Xcode 中构建

打开：

- `yun/client-app/ios/OBDGatewayClient.xcworkspace`

然后在 Xcode 中选择：

- Debug / Release
- Simulator / Device
- 对应签名 Team

---

## 10. 未来如果要修改 A 端“服务器 APK 的行为”，当前仓库怎么做

你当前理解的“反编译 + DLL 注入”是核心，但完整链路通常不止这两步。

按当前仓库的能力，A 端改造一般分成下面几类：

### 10.1 修改 Web UI

目录：

- `react_ui`

流程：

1. 修改 `react_ui` 源码
2. 执行：

```bash
cd react_ui
npm install
npm run build
cp -r dist/* ../my_source_code/
```

3. 再运行：

```bash
python3 automator.py
```

### 10.2 修改 C# WebSocket 层 / 注入逻辑

目录：

- `yun/websocket_layer`
- `yun/tools/DllInjector`
- `yun/tools/DllPatcher`
- `yun/tools/JSBridgePatcher`
- `yun/tools/InterfacePatcher`
- `yun/tools/ProxyGenerator`

这类修改的典型流程：

1. 准备原始 APK
2. 用 `apktool d` 提取 `temp_verify/unknown/assemblies`
3. 修改 C# 逻辑
4. 执行：

```bash
cd yun/tools
bash build_and_inject.sh
```

5. 再执行：

```bash
cd ../..
python3 automator.py
```

### 10.3 修改 APK 打包逻辑

当前主要由：

- `automator.py`

负责。

如果你未来需要加入更多处理步骤，例如：

- 替换更多资源文件
- 修改额外目录
- 加入更多自动修补逻辑

通常就是扩展 `automator.py`。

### 10.4 当前自动化里，除了“反编译 + DLL 注入”还有什么

以当前代码为准，已经具备或部分具备的操作包括：

1. **反编译 APK**
2. **替换前端 UI 资源**
3. **注入多个修补后的 DLL**
4. **重新打包 APK**
5. **zipalign**
6. **APK 签名**
7. **部分资源替换**
   - 当前脚本中已包含对 `styles.xml` 的可选替换入口
8. **C# 层桥接/代理/字段注入**
   - 通过 `DllInjector`、`DllPatcher`、`InterfacePatcher` 等工具完成

所以准确地说，当前仓库支持的是：

- APK 反编译
- Web UI 注入
- DLL 层修补与注入
- APK 重打包与签名

而不是只有“反编译 + DLL 注入”两件事。

---

## 11. 当前仓库已经能支持哪些 A 端改造

按当前工具链，已经比较适合做的事情有：

1. 修改 Web UI
2. 修改托管 DLL 中的字段、方法、桥接逻辑
3. 修改 WebSocket 通信层
4. 修改部分资源替换逻辑
5. 重新打包并签名 APK

也就是说，如果未来要修改的是：

- CarDemo.Android.dll
- CarDemo.dll
- CarScannerXamarinForms.dll
- WebSocket 桥接逻辑
- A 端 Web UI

那么**当前代码是能够支持的**。

---

## 12. 当前仓库还不属于“开箱即用”的深层改造范围

如果未来改动涉及下面这些内容，当前仓库不能算完全现成，但可以继续扩展：

1. 大规模 `AndroidManifest.xml` 修改
2. 大规模 `smali` 修改
3. 原生 `.so` 库修改
4. 更复杂的资源系统改造
5. 包名、Activity、安装结构的大范围切换
6. 多套正式签名体系并行管理

这些情况下，通常要继续扩展：

- `automator.py`
- 或新增单独脚本 / 工具

---

## 13. 对后来接手同事的建议

### 如果你只想快速出当前版本

按顺序做：

1. 准备原始 APK
2. 改 `automator.py` 中本机工具路径
3. 跑 `python3 automator.py`
4. Android / iOS 客户端分别按各自工程正常构建

### 如果你要继续开发 A 端功能

建议按下面原则：

1. **只改前端 UI**
   - 改 `react_ui`
   - 重新 build
   - 再跑 `automator.py`

2. **改 DLL 逻辑**
   - 先准备 `temp_verify`
   - 再跑 `yun/tools/build_and_inject.sh`
   - 最后跑 `automator.py`

3. **改 APK 结构或打包流程**
   - 直接改 `automator.py`

4. **改调度/实例部署**
   - 优先阅读：
     - `yun/deployment/README.md`
     - `yun/deployment/总体架构说明文档.md`

---

## 14. 仓库内已有文档

如果你需要更细节的说明，可继续阅读：

- `BUILD.md`
- `Android_guide.md`
- `yun/README.md`
- `yun/BUILD_CLOUD_APK.md`
- `yun/client-app/RUN_GUIDE.md`
- `yun/deployment/README.md`

---

## 15. 最终结论

当前仓库对后来接手者来说：

1. **不缺核心源码**
2. **缺的主要是原始 CarScanner APK 这个外部输入**
3. **iOS 真机/ipa 还需要接手者自己的 Apple 签名能力**
4. **A 端未来继续改行为是可行的**
5. **当前最成熟的改造层是**
   - Web UI
   - 托管 DLL
   - WebSocket 层
   - APK 打包层
6. **更深层的 Android 原生改造仍需继续扩展工具链**

