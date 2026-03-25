# 04 APK 构建与部署

> 本文描述：在本地修改 A端代码后，如何一键构建并部署到服务器的 Waydroid 容器中。

---

## 一、整体流程图

```
本地 Mac
┌─────────────────────────────────────────────────────────┐
│  1. 修改 A端代码（websocket_layer/src/*.cs 等）           │
│  2. cd NewUI/yun/tools && bash build_and_inject.sh      │
│     → 编译 OBDCloud.WebSocket.dll                       │
│     → 注入 CarDemo.Android.dll                          │
│     → 输出到 NewUI/yun/modified_dlls/                   │
│  3. cd NewUI && python3 automator.py                           │
│     → 打包 APK                                               │
│     → 签名                                                   │
│     → 输出：Release_com.companyname.cardemo-Signed.apk       │
│             （本地测试用，automator.py 固定此名，不可改）        │
│  4. bash fuwuqi/scripts/deploy_to_server.sh                  │
│     → cp 为服务器专用副本：                                   │
│         Release_com.companyname.cardemo-Server-Signed.apk    │
│     → scp 服务器副本到服务器                                   │
│     → SSH 远程安装 APK                                        │
│     → SSH 远程重启 APK                                        │
└─────────────────────────────────────────────────────────┘

服务器
┌─────────────────────────────────────────────────────────┐
│  自动执行（由 deploy_to_server.sh 触发）：               │
│  - adb connect 192.168.240.112:5555                     │
│  - adb install -r xxx.apk                              │
│  - am force-stop com.companyname.cardemo               │
│  - am start -n com.companyname.cardemo/.MainActivity   │
│  - adb forward tcp:8080 tcp:8080                       │
└─────────────────────────────────────────────────────────┘
```

---

## 二、前提条件

### 2.1 本地环境（Mac）

确认以下命令可用：

```bash
# Java（用于 apksigner）
java -version  # 需要 Java 17
# 如果报错：
export JAVA_HOME=/opt/homebrew/Cellar/openjdk@17/17.0.18/libexec/openjdk.jdk/Contents/Home
export PATH="$JAVA_HOME/bin:$PATH"

# .NET SDK（用于编译 C# 项目）
dotnet --version  # 需要 .NET 6+

# Mono（用于 mcs 编译器）
mcs --version

# ADB（可选，本地测试用）
adb version

# SSH 密钥（访问服务器）
ls ~/.ssh/  # 确认有服务器的私钥文件
```

### 2.2 服务器端（首次配置）

```bash
# 以下命令在服务器上执行一次即可
apt install -y adb
mkdir -p /opt/cardemo
```

### 2.3 deploy_to_server.sh 的配置变量

编辑 `fuwuqi/scripts/deploy_to_server.sh`，修改顶部的变量：

```bash
SERVER_IP="你的服务器公网IP"
SERVER_USER="root"                    # 或其他 SSH 用户名
SSH_KEY="~/.ssh/your_key.pem"        # SSH 私钥路径
```

---

## 三、本地构建步骤（详细）

### Step 1：修改代码

只有修改了以下路径的代码才需要重新构建：
- `NewUI/yun/websocket_layer/src/` — A端 C# 逻辑
- `NewUI/yun/tools/DllPatcher/` — DLL 修补逻辑

如果只修改了 B端代码（`yun/client-app/src/`），不需要重新构建 APK。

### Step 2：构建 WebSocket 层 + 注入 DLL

```bash
cd /Users/zed/Documents/Programe/CarUI/NewUI/yun/tools
bash build_and_inject.sh
```

**预期输出**：
```
Building WebSocket layer...
Building WebSocketIOBDConnectionProxy with Mono mcs...
Building injector...
Running injector...
Building patcher...
Running patcher on CarScannerXamarinForms.dll...
Copying WebSocketIOBDConnectionProxy.dll to output...
Done. Output in .../modified_dlls
Files:
-rw-r--r-- 1 ... 2048 ... OBDCloud.WebSocket.dll   ← 重要：必须是 ~2KB 占位符！
-rw-r--r-- 1 ... xxxx ... CarDemo.Android.dll
-rw-r--r-- 1 ... xxxx ... CarScannerXamarinForms.patched.dll
```

> ⚠️ 红线：`OBDCloud.WebSocket.dll` 必须是约 2048 bytes 的占位符。
> 如果看到它是几 MB，说明构建有问题（见 MEMORY.md 陷阱1）。

### Step 3：打包并签名 APK

```bash
cd /Users/zed/Documents/Programe/CarUI/NewUI

# 如果 apksigner 找不到 Java，设置 JAVA_HOME
export JAVA_HOME=/opt/homebrew/Cellar/openjdk@17/17.0.18/libexec/openjdk.jdk/Contents/Home
export PATH="$JAVA_HOME/bin:$PATH"

python3 automator.py
```

**预期输出文件**：
```
NewUI/Release_com.companyname.cardemo-Signed.apk        ← 本地测试用（automator.py 固定名称）
```

> 说明：`automator.py` 是现有构建脚本，输出文件名固定，不修改它。
> `deploy_to_server.sh` 在 Step 4 会将其 `cp` 为 `*-Server-Signed.apk`，二者并存，互不覆盖。

### Step 4：一键部署到服务器

```bash
cd /Users/zed/Documents/Programe/CarUI/NewUI
bash fuwuqi/scripts/deploy_to_server.sh
```

---

## 四、APK 文件命名与存放路径

### 本地 Mac（NewUI/）

```
NewUI/
├── Release_com.companyname.cardemo-Signed.apk         ← 本地测试用（automator.py 输出）
└── Release_com.companyname.cardemo-Server-Signed.apk  ← 服务器专用副本（deploy_to_server.sh 生成）
```

两个文件由同一次构建产生，内容相同，名称不同。**运行 `deploy_to_server.sh` 时两个文件都会被更新**，本地测试 APK 不会因为服务器部署而"消失"，只是内容被刷新（和你本地安装到模拟器时一样的版本）。

### 服务器（/opt/cardemo/）

```
服务器: /opt/cardemo/
├── Release_com.companyname.cardemo-Server-Signed.apk   ← 当前版本（每次部署覆盖）
└── backup/
    └── Release_com.companyname.cardemo-Server-Signed-<timestamp>.apk  ← 上一个版本备份
```

`deploy_to_server.sh` 脚本会自动将旧 APK 备份后再上传新版本。

---

## 五、回滚

如果新版本有问题，通过 SSH 手动回滚：

```bash
ssh -i ~/.ssh/your_key.pem root@服务器IP

# 列出备份
ls -la /opt/cardemo/backup/

# 恢复备份（将 <timestamp> 替换为实际时间戳）
cp /opt/cardemo/backup/Release_com.companyname.cardemo-Server-Signed-<timestamp>.apk \
   /opt/cardemo/Release_com.companyname.cardemo-Server-Signed.apk

# 重新安装
adb -s 192.168.240.112:5555 install -r \
  /opt/cardemo/Release_com.companyname.cardemo-Server-Signed.apk

# 重启 APK
bash /opt/cardemo/scripts/server_restart_apk.sh
```

---

## 六、只更新 B端（React Native App）

B端代码修改后，不需要重新构建 APK，只需重新构建 React Native：

- **Android B端**：在本地用 `npm run android` 或 Android Studio 打包新的 APK
- **iOS B端**：在本地用 Xcode / xcodebuild 重新签名并 install

B端连接 A端的地址在 App 运行时由用户在设置界面填写，切换本地/服务器只需修改设置，不需要重新打包。
