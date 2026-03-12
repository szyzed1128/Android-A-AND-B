# MFi 实机验证清单（iOS）

适用范围：`yun/client-app` iOS 端 B 软件（RN + MFi 原生桥接）。
目标：用统一日志快速确认「扫描 -> 连接 -> 收发 -> 断连」全链路是否稳定。

## 1. 准备条件

- iPhone 已开启蓝牙。
- OBD 适配器已上电，且在 iOS 系统中可被识别为 External Accessory。
- App 已包含 `UISupportedExternalAccessoryProtocols` 对应协议（当前已加：`com.obd2.elm327` / `com.obdlink.obd` / `com.vgatemall.obd`）。
- 使用 Debug 构建运行，打开 Xcode 控制台或 RN 日志。

## 2. 关键日志标签

- JS 层：`[MFi][...]`
- 原生层：`[MFiNative][...]`

建议重点观察以下阶段：

- `init / initialize`
- `scan`
- `connect`
- `tx`（发送）
- `rx`（接收）
- `disconnect / session`

## 3. 用例清单

### 用例 A：扫描已连接配件

步骤：

1. 打开连接页，触发 MFi 扫描。
2. 确认设备列表出现目标配件。

通过标准：

- 出现 JS 日志：
  - `[MFi][scan] start`
  - `[MFi][scan] discovered accessory ...`
  - `[MFi][scan] finished`
- 出现原生日志：
  - `[MFiNative][scan] connected accessories fetched ...`

### 用例 B：协议回退连接

步骤：

1. 选择目标设备并点击连接。
2. 观察协议尝试顺序与最终成功协议。

通过标准：

- JS 日志可见：
  - `[MFi][connect] protocol candidates prepared ...`
  - `[MFi][connect] attempt openSession ...`
  - 若某协议失败：`attempt failed`
  - 最终：`success`
- 原生日志可见：
  - `[MFiNative][connect] openSession request ...`
  - 成功时：`openSession success`

### 用例 C：串行发送与响应

步骤：

1. 连接成功后，连续快速发送多条 AT/OBD 命令。
2. 观察发送是否按序完成，且每条均有响应。

通过标准：

- JS 日志可见：
  - `send enqueue -> send sent` 成对出现
  - `txPackets/rxPackets` 持续增加
- 原生日志可见：
  - `[MFiNative][tx] send success ...`
  - `[MFiNative][rx] chunk received ...`

### 用例 D：断连与恢复

步骤：

1. 连接状态下拔掉适配器电源或关闭蓝牙。
2. 观察 App 是否收到断连原因，并可再次连接。

通过标准：

- JS 日志可见：
  - `[MFi][disconnect] session closed event ...`
  - `[MFi][session] cleanup ...`
- 原生日志可见：
  - `[MFiNative][disconnect] accessory did disconnect ...`
  - `[MFiNative][disconnect] session closed ...`

## 4. 快速排障映射

- 症状：扫描不到设备
  - 看 `MFiNative scan connected accessories fetched count=0`
  - 优先检查：配件是否真的属于 External Accessory、协议白名单是否匹配。

- 症状：能扫到但连不上
  - 看 `MFi connect attempt failed` 与 `MFiNative connect openSession ...` 的错误信息。
  - 常见原因：协议字符串不匹配、设备已被其他会话占用。

- 症状：连接后偶发写失败
  - 看 `MFiNative tx wait writable timeout / write timeout / write failed`。
  - 重点检查：命令发送频率是否过高、设备端是否阻塞。

- 症状：连接后无响应
  - 看是否有 `MFiNative rx chunk received`。
  - 若无：优先检查会话协议、命令格式、设备是否需要初始化命令序列。

## 5. 测试记录建议

每次实机测试至少记录：

- 设备名称/型号（如能获取）
- 连接时最终协议
- 连接建立耗时
- 5 分钟内 `txPackets/rxPackets`
- 异常断连次数与原因

