# CarII 项目记忆

> 此文件在每次对话开始时由 Claude Code 自动加载。
> 包含必须遵守的关键规则和经验教训。

## 0. A/B 端定义（最高优先级，必须牢记）

- **A 端 = 模拟器中运行的 CarScanner APK（WebSocket 服务器端）**
  - 运行在云端 Android 模拟器中
  - 包含完整 OBD 诊断协议解析逻辑
  - 内置 WebSocket 服务器，等待 B 端连接
  - **不是最终产品，用户不直接接触**

- **B 端 = 最终上架商城的客户端软件（WebSocket 客户端端）**
  - 运行在用户手机上，作为蓝牙网关 + UI 展示层
  - **既有 Android 版本（`yun/client-app/android/`）也有 iOS 版本（`yun/client-app/ios/`）**
  - 需要上架 Google Play 和 App Store，面向真实用户
  - 通过 WebSocket 连接到 A 端，通过蓝牙连接到 ELM327

> **一句话总结**：模拟器中的是 A 端（服务器），B 端是客户端，也是最终需要上架商城的软件，既有安卓版本也有 iOS 版本。

---

## 1. 思考原则（必须遵守）

### 1.0 始终使用中文回复
- 所有回复、注释、说明必须使用中文
- 代码中的变量名和函数名保持英文，但注释用中文

### 1.0.1 硬件设备绝对没问题
- **整个项目中硬件设备 ELM327 是绝对不可能出任何问题的**
- 一旦遇到类似蓝牙连接不上等问题，**永远不用考虑硬件设备的问题**
- **肯定是软件方面有问题**，必须从代码层面排查

### 1.1 先回顾再行动
- 在对 IPA 进行任何修改之前，检查类似的修改是否曾导致闪退
- 参考下面的闪退修复历史
- 遇到新问题时，先检查 Android 版本（NewUI/）是如何解决的

### 1.2 参考 Android 成功模式
- Android 版本在 NewUI/ 目录下工作正常
- 始终检查 NewUI/automator.py 了解构建流程
- 始终检查 NewUI/react_ui/ 了解 React 代码
- 始终检查 NewUI/modified_dlls/ 了解修改了哪些 DLL

### 1.3 牢记最终目标
- 用 React UI 替换 CarScanner 的原生界面
- React UI 必须能调用原有业务逻辑（蓝牙、OBD、故障码等）
- 这不仅仅是显示一个 WebView - 而是要实现完整功能

---

## 2. iOS 闪退规则（关键）

### 2.1 核心原则：不是"不能改"，而是"改了需要同步"

之前测试闪退的原因是：**只改了 Info.plist，没有同步其他位置**。

### 2.2 需要同步修改的位置

| 位置 | 内容 | 修改工具 |
|------|------|---------|
| **Info.plist** | CFBundleName, CFBundleVersion, CFBundleIdentifier | 文本编辑器 |
| **CarScannerXamarinForms.dll** | 程序集版本（Assembly Version） | dnSpy (Windows) |
| **embedded.mobileprovision** | Identifier | 重新生成描述文件 |
| **archived-expanded-entitlements.xcent** | application-identifier | 签名时自动生成 |

### 2.3 最可能的闪退原因

```
Info.plist CFBundleVersion = "1.0.0"
DLL Assembly Version = "1.121.0"  ← 不匹配！
结果：Xamarin 运行时抛出 "Version mismatch" 异常 → 闪退
```

### 2.4 正确的修改顺序

1. 用 dnSpy 修改 DLL 程序集版本 → 与 Info.plist 一致
2. 修改 Info.plist 所有版本字段
3. 确保 provisioning profile 的 Identifier 匹配
4. 重新签名

---

## 3. Android vs iOS 架构

### 3.1 APK vs IPA 反编译差异

| 平台 | DLL 内容 | 方法体 | 实际代码位置 |
|------|---------|--------|-------------|
| Android APK | 完整 IL 代码 | 有实现逻辑 | DLL 本身 |
| iOS IPA | 只有元数据 | **空的** | .aotdata.arm64 + 主二进制 |

### 3.2 iOS 上 DLL 替换为什么无效

iOS（Mono AOT 运行时）：
- DLL 只包含元数据（类型信息、方法签名）
- 方法体是空的 - 实际代码在 .aotdata.arm64 和主二进制中
- 替换 DLL 只改变元数据，不改变执行的代码

### 3.3 iOS 解决方案

复用 Android 思路但适配 AOT：
- Android：修改 DLL → 重新打包 APK
- iOS：修改 DLL → **重新 AOT 编译** → 打包 IPA

---

## 4. Android 成功模式详解

### 4.1 构建流程（NewUI/automator.py）

```
1. 反编译 APK (apktool)
2. 注入 React UI → assets/dist/
3. 替换修改过的 DLL:
   - CarScannerXamarinForms.dll → 修改了 App.MainPage 初始化
   - Xamarin.Forms.Core.dll → 禁用了 DisplayAlert
4. 替换 Smali 文件 → FormsWebChromeClient（拦截弹窗）
5. 重编译、签名
```

### 4.2 JSBridge 方法（24 个）

**连接**: connectAsync, disconnectAsync
**配置**: getBrands, getProfiles, applyProfile, getECUList
**诊断**: readECUInfoAsync, readDTCAsync, clearDTCAsync, readFreezeFrameAsync, getPIDList
**实时数据**: startReadPIDs, stopReadPIDs, getAllSensorsAsync
**蓝牙**: startBTScan, stopBTScan, startBLEScan, stopBLEScan
**其他**: callCSharpMethod, setPromiseResult, testSensorsPage, testDTCPage, testFreezeFramePage, testSettingsPage

### 4.3 三层弹窗拦截

| 层级 | 位置 | 方法 |
|------|------|------|
| 第1层 | index.html | 拦截 window.alert/confirm |
| 第2层 | FormsWebChromeClient.smali | 拦截 WebView 的 onJsAlert/onJsConfirm |
| 第3层 | 修改后的 DLL | 禁用 DisplayAlert 方法 |

---

## 5. iOS 项目已完成的代码

### 5.1 CarDemo.iOS 桥接代码（1087 行）

| 文件 | 行数 | 用途 |
|------|------|------|
| MainWebViewRenderer.cs | 446 | WKWebView 渲染器 |
| JSBridge.cs | 417 | 23+ 个方法映射 |
| MainWebViewRenderer_URLScheme.cs | 224 | 备选 URL Scheme 方案 |

### 5.2 React UI

- 源码: ios_project/react_ui/
- 构建产物: ios_project/my_source_code/
- 与 Android 版本相同

---

## 6. 测试 IPA 历史记录

| IPA | 改了什么 | 结果 | 原因 |
|-----|----------|------|------|
| CarII_OriginalResigned | 仅重新签名 | ✓ 正常 | 基线 |
| CarII_Test3_Both | 添加了 www + DLL | ✓ 正常 | 安全操作 |
| CarII_Test5_NameOnly | 改了 CFBundleName | ✗ 闪退 | 未同步 DLL 版本 |
| CarII_Test6_VersionOnly | 改了 CFBundleVersion | ✗ 闪退 | 未同步 DLL 版本 |
| CarII_Test8_DisplayNameOnly | 改了 CFBundleDisplayName | ✓ 正常 | 无需同步 |
| CarII_WithPatchedDLL | 替换了主 DLL | ✗ 闪退 | AOT：方法体为空 |

---

## 7. 当前可工作的 IPA 配置

```
# Info.plist 值
CFBundleIdentifier: com.zed.carii
CFBundleDisplayName: CarII
CFBundleName: Car Scanner          # 保持原值（或同步修改 DLL）
CFBundleVersion: 1.121.0           # 保持原值（或同步修改 DLL）

# 已注入的文件（安全操作）
www/                              # React UI
CarDemo.dll                       # 跨平台逻辑
CarDemo.iOS.dll                   # iOS 桥接
```

---

## 8. 可用工具

### 8.1 Apple Developer Documentation MCP

已激活，可用于查询 Apple API 文档：
- `mcp__apple-docs__search_apple_docs` - 搜索文档
- `mcp__apple-docs__get_apple_doc_content` - 获取文档内容
- `mcp__apple-docs__list_technologies` - 浏览技术框架
- `mcp__apple-docs__search_framework_symbols` - 搜索框架符号
- `mcp__apple-docs__search_wwdc_content` - 搜索 WWDC 内容

### 8.2 常用场景

- 查询 WKWebView、WKScriptMessageHandler 用法
- 查询代码签名要求
- 查询 Info.plist 键值定义
- 查询 Xamarin.iOS 相关的 Apple API

---

## 9. 下一步待办

- [ ] 阶段 1：验证 AOT+JIT 混合模式（dnSpy 修改 DLL 后测试）
- [ ] 阶段 2：使用 Xamarin.iOS 编译（生成 AOT 产物）
- [ ] 阶段 3：注入和测试完整 IPA

---

## 10. 技术踩坑记录

> 记录曾经踩过的坑和解决方案，供以后遇到类似问题时参考。

### 10.1 DLL Patch：async 状态机中 `ldarg.0` 与 `ldloc.1` 的陷阱

**场景**：用 dnlib 向 `CarScannerXamarinForms.dll` 注入 IL，拦截 `OBDDataReader.Connection` 字段赋值。

**症状**：WSProxy 构造函数日志出现（说明对象被创建），但 `ConnectAsync` / `WriteBytesAsync` 从未被调用，连接逻辑走的仍是原始蓝牙分支。

**根因**：`OBDDataReader.Connect()` 是 async 方法，编译器生成的状态机为 `<Connect>d__131.MoveNext()`。在 `MoveNext()` 内部：
- `ldarg.0` 加载的是**状态机结构体自身**（`<Connect>d__131`），不是 `OBDDataReader`
- Mono 编译器会在 `MoveNext()` 入口将 `this`（OBDDataReader）缓存到局部变量，所有 `this.Connection = ...` 均编译为 `ldloc.1`（直接读局部变量），而非 `ldarg.0 + ldfld <>4__this`
- 原 patch 硬编码 `Ldarg_0`，导致 `stfld Connection` 把 WSProxy 写入了状态机的内存区域，`OBDDataReader.Connection` 实际上从未被修改

**正确做法**：不要假设加载方式，直接复制 `instructions[startIndex]`（即 DllPatcher 扫描到的赋值起始指令），它本身就是正确的对象加载指令：

```csharp
// 错误：硬编码 Ldarg_0
newInstructions.Add(OpCodes.Ldarg_0.ToInstruction());

// 正确：复制原指令（可能是 ldloc.1 或其他形式，取决于编译器）
var loadObjInstr = instructions[startIndex];
newInstructions.Add(new Instruction(loadObjInstr.OpCode, loadObjInstr.Operand));
```

**附带问题**：dnlib 对注入后的复杂方法体重新计算 MaxStack 会报错，需要在写入选项中保留原始值：

```csharp
options.MetadataOptions.Flags |= dnlib.DotNet.Writer.MetadataFlags.KeepOldMaxStack;
```

**验证结果**：修复后全链路贯通——AT 命令经 WebSocket → B 端 → ELM327，响应原路返回，OBD 初始化状态依次经过 `ConnectingToECU → ConnectingToELM → ConnectedToELM → Successful connection!`。
