# 构建说明

## 必读：原始 APK

本仓库**不包含**原始 CarScanner APK（`original_apk/` 目录已被 `.gitignore` 排除）。

构建 A 端之前，需要手动将原始 APK 放入：

```
NewUI/original_apk/com.companyname.cardemo-Signed.apk
```

> 文件大小约 149MB，包名 `com.companyname.cardemo`，版本 1.121.0。

---

## A 端（Android 模拟器）

### 前提条件

- 已安装：`apktool`、`Android SDK build-tools 34.0.0`、`dotnet SDK`、`Mono（mcs）`、`Node.js`
- 已放入原始 APK（见上方）

### 步骤 1：构建 React UI（仅修改了前端代码时需要）

```bash
cd NewUI/react_ui
npm install
npm run build
cp -r dist/* ../my_source_code/
```

### 步骤 2：构建并注入修改后的 DLL（仅修改了 C# WebSocket 源码时需要）

> 注意：此步骤需要 `temp_verify/unknown/assemblies/` 中的原始 DLL 作为编译参考。
> 这些 DLL 可以通过以下命令从原始 APK 提取：
> ```bash
> mkdir -p NewUI/temp_verify
> apktool d NewUI/original_apk/com.companyname.cardemo-Signed.apk -o NewUI/temp_verify -f
> ```

```bash
cd NewUI/yun/tools
bash build_and_inject.sh
```

### 步骤 3：打包 APK

```bash
cd NewUI
python3 automator.py
```

输出：`NewUI/build_output/Release_com.companyname.cardemo-Signed.apk`

### 安装到模拟器

```bash
adb -s <模拟器ID> install -r NewUI/build_output/Release_com.companyname.cardemo-Signed.apk
```

---

## B 端（React Native，物理设备）

### 前提条件

- 已安装：`Node.js`、`Android SDK`、`JDK 17`
- 物理设备已开启 USB 调试并连接

### 步骤

```bash
cd NewUI/yun/client-app
npm install
npm run android
```

或使用 Metro 热更新模式：

```bash
# 终端 1：启动 Metro
npm start

# 终端 2（如已安装 APK，只需推送 JS 代码）
adb -s <设备ID> reverse tcp:8081 tcp:8081
```

---

## 常用命令速查

```bash
# 查看已连接设备
adb devices

# A 端模拟器日志
adb -s <模拟器ID> logcat -s OBDCloudManager WebSocketServerBridge

# B 端设备日志
adb -s <设备ID> logcat -s ReactNativeJS CloudBridge BluetoothManager
```
