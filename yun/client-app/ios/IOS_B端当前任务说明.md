# iOS B 端当前任务说明

## 1. 文档目的
这份文档用于让新加入当前任务的开发者快速理解我们现在正在做什么，尤其是让 Claude Code、Gemini 或其他协作代理在最短时间内进入上下文。

当前任务不是从零开始开发 iOS，而是在已经完成一部分 iOS B 端基础能力的前提下，继续推进 `MFi -> ELM327 -> ECU 初始化` 这条链路，并定位为什么现在 MFi 已经能连上 ELM，但 ECU 初始化仍然失败。

---

## 2. 当前任务目标
当前重点目标只有一个：

- 把 iOS B 端的 `MFi -> ELM -> ECU 初始化` 链路打通

更具体地说：

1. iOS B 端已经可以发现并连接 MFi 设备
2. WebSocket 桥接已经可以把 A 端命令转发给 B 端
3. B 端已经可以把 ELM 的响应回传给 A 端
4. 当前真正卡住的是 ECU 初始化阶段，而不是发现设备、建立 MFi session、或 WebSocket 整体断链

当前不以 UI 为重点，不优先做页面样式、交互细节或功能扩展。

---

## 3. 当前系统结构
### 3.1 A / B 角色
- A 端：注入后的 CarScanner 侧，当前主要跑在安卓模拟器中，用于发起初始化、读写 OBD 命令、承接原有诊断逻辑
- B 端：React Native 客户端，当前 iOS 真机运行，用于本地连接蓝牙设备（BLE / MFi），并通过 WebSocket 与 A 端通信

### 3.2 通信路径
当前总体数据路径如下：

`A(注入版 CarScanner) -> WebSocket -> B(iOS RN) -> MFi -> ELM327 -> ECU`

响应路径反向返回：

`ECU -> ELM327 -> MFi -> B(iOS RN) -> WebSocket -> A`

### 3.3 当前 iOS 连接方式结论
在 iOS 侧，本期只要求关注：

- BLE
- MFi

当前重点是 MFi，不是 BLE，也不是 UI 页面统一。

---

## 4. 当前代码状态
### 4.1 已经存在的 iOS 关键实现
当前 iOS B 端并不是空白，而是已经具备下列基础：

1. iOS 原生 MFi 模块已存在
2. RN 侧 `BluetoothManager.ios.ts` 已承接 MFi 扫描、连接、发送、接收逻辑
3. WebSocket 云桥接链路已经接上
4. iOS 真机上已经能扫描到并连接 MFi 设备

### 4.2 已确认已经回退的临时诊断层
之前为了诊断问题，曾临时新增过以下 iOS 专属文件：

- `useBluetoothBridge.ios.ts`
- `HomePage.ios.tsx`
- `BridgeDebugStore.ios.ts`

这些临时诊断文件已经整批完整回退，当前不要把它们当作现状，也不要再假定它们仍然存在。

### 4.3 当前应以什么状态为准
当前应以“回退后的干净状态”为准：

- iOS 没有额外的 HomePage 诊断 UI
- iOS 没有单独的桥接调试 Hook 覆盖层
- 之前加到 `BluetoothManager.ios.ts` 里的那批临时诊断上报也已经撤掉

---

## 5. 已完成的事情
截至目前，以下事项已经完成：

1. iOS B 端项目已能在真机运行
2. Metro 连接问题已梳理清楚，开发服务器可通过局域网 IP 连接
3. MFi 设备扫描流程已可用
4. 至少一类 MFi 设备可以在 B 端页面被发现并点击连接
5. MFi session 已能够建立
6. A/B WebSocket 连接已经可用
7. A 端能把初始化命令发到 B 端
8. B 端能把 ELM 返回值带回 A 端
9. A 端已经明确可到 `ConnectedToELM`

这说明：

- WebSocket 通道不是完全坏的
- MFi 会话不是完全没建立
- ELM 适配器并非完全无响应

---

## 6. 当前卡点
当前卡点非常明确：

- `ELM 已连接，但 ECU 初始化失败`

表现为：

1. A 端先到 `ConnectedToELM`
2. 随后进入 `ConnectingToECU`
3. 基础 AT 命令可通
4. 一旦进入 ECU 探测（尤其是 `0100`）就反复失败

换句话说，问题不在“连接上适配器”，而在“连接上适配器后，没能进一步把 ECU 会话建立起来”。

---

## 7. 通过 A / B 日志已确认的问题
下面是当前最关键的事实结论，这部分是后续所有分析的基础。

### 7.1 已确认：A 发命令正常
A 端日志显示，初始化阶段命令确实已经发出，包括：

- `ATE0`
- `ATAT1`
- `ATDPN`
- `0100`

因此不能再把问题归因成“A 没有发命令”。

### 7.2 已确认：B 有回传，不是无响应
A 端日志里能看到 `ELM327->B->A` 的回包内容，说明 B 端确实把本地适配器收到的数据带回来了。

这意味着：

- 不是 B 完全没收到数据
- 也不是 B 收到后完全没回给 A

### 7.3 已确认：ELM 层是活的
日志中明确出现过：

- `ELM327 v1.4b`
- `OBDDataReader.StatusChanged -> ConnectedToELM`

这说明至少 ELM 这一层是活的。

### 7.4 已确认：失败点在 ECU 初始化
当前最核心的失败发生在 `0100` 这类 ECU 探测命令上。

多次抓到的结果包括：

- `UNABLE TO CONNECT`
- `BUS INIT: `
- `ERROR`

这三个字符串不是推测，而是已经在 A 端日志中明确出现。

### 7.5 已抓到的关键日志结论
已抓到的典型现象如下：

1. `ATAT1` 返回 `OK`
2. `ATDPN` 返回 `A0`
3. `0100` 多次返回 `UNABLE TO CONNECT`
4. 后续又多次出现 `BUS INIT: `
5. 某些轮次还夹杂 `ERROR`

这说明当前链路不是完全死掉，而是“正在尝试建立 ECU 总线，但没有成功进入稳定有效会话”。

---

## 8. 当前最可靠的结论
当前问题应被严格表述为：

- `MFi 会话是通的`
- `ELM 也是活的`
- `WebSocket 桥接也能来回传输数据`
- `真正失败的是 ECU 初始化 / 总线建立`

所以不要再把问题误判为：

- B 端没有回传数据
- WebSocket 整体断链
- MFi 压根没有连上
- ELM 完全没响应

这些都不符合当前日志证据。

---

## 9. 一个非常容易误判的点
A 端日志里如果看到 `ATAT1`，**不能直接证明** B 端最终也真的向 ELM 发出了 `ATAT1` 原文。

原因：

- A 端 `WebSocketBluetoothConnection` / `WebSocketIOBDConnectionProxy` 的发送日志是在发给 B 之前打的
- 所以如果未来需要验证“B 端有没有改写 AT 命令”，不能只靠 A 端发送日志下结论

这点非常重要。

不过在当前这轮问题里，即便先不讨论命令改写，单看返回值也已经足够确认：当前 ECU 初始化仍然失败。

---

## 10. 用户观察到的一个重要现象
用户明确观察到：

- 原版 CarScanner 连接 ELM327 后，OBD/Host 灯会持续闪烁
- 我们当前实现中，灯只是闪几下就停

这提示一个高价值方向：

- 原 CarScanner 很可能存在持续探活 / 保活 / 在线机制
- 而我们当前 WebSocket / OBDDataReader 对接可能没有完整复现这部分逻辑

后续分析时，不能只看初始化那几条命令，还要研究原 CarScanner 是否在连接建立后持续发某类探活命令。

---

## 11. 当前有效的开发边界
以下边界是当前有效版本：

1. iOS 开发不能破坏安卓端
2. 新增 iOS 文件要带明显 iOS 标签
3. 尽量优先用 `.ios.ts / .ios.tsx` 做覆盖
4. 能共享的逻辑最终还是尽量共享
5. 当前重点不是 UI，而是先把 `MFi -> ELM -> ECU 初始化` 这条链打通

---

## 12. Claude Code / Gemini 进入任务前必须主动读取的文件
### 12.1 必读文件
1. `ios/DEVELOPMENT_RULES_IOS.md`
2. `ios/MFI_REAL_DEVICE_VALIDATION_IOS.md`
3. `src/bluetooth/BluetoothManager.ios.ts`
4. `src/screens/BluetoothPage.ios.tsx`
5. `ios/OBDGatewayClient/OBDMFiModuleIOS.mm`
6. `ios/OBDGatewayClient/OBDMFiModuleIOS.h`
7. `src/hooks/useBluetoothBridge.ts`
8. `src/hooks/useLocalBluetooth.ts`
9. `src/services/CloudBridge.ts`
10. `src/context/AppContext.tsx`
11. `../websocket_layer/src/OBDCloudManager.cs`
12. `../websocket_layer/src/WebSocketBluetoothConnection.cs`
13. `../websocket_layer/src/WebSocketIOBDConnectionProxy.cs`

### 12.2 补充读文件
1. `src/screens/HomePage.tsx`
2. `src/protocol/MessageProtocol.ts`
3. `ios/OBDGatewayClient/Info.plist`
4. `ios/OBDGatewayClient.xcodeproj/project.pbxproj`
5. `../../CarScannerC#反编译后的源码/PIDScannerPage.cs`

---

## 13. 建议 Claude Code 按什么顺序理解项目
建议阅读顺序如下：

1. 先读 iOS 规则文档，明确开发边界
2. 再读 iOS MFi 实现链路
3. 再读 A 端 WebSocket 注入链路
4. 最后去看原 CarScanner 反编译源码里与初始化、保活、PID 探测有关的部分

这样能最快进入当前任务上下文。

---

## 14. 后续最值得优先分析的三个问题
如果要继续推进问题定位，优先级最高的是：

1. `BluetoothManager.ios.ts` 当前 MFi 数据接收聚合策略，是否破坏了 ECU 初始化阶段的响应边界
2. 原版 CarScanner 在 `0100` 前后的初始化节奏、AT 序列、等待时间，与我们当前实现到底有哪些差异
3. 原版 CarScanner 是否存在持续探活 / 保活逻辑，而我们当前 WebSocket / OBDDataReader 对接没有复现

---

## 15. 当前不要再重复走的弯路
下面这些方向，当前证据已经说明它们不是主要根因，至少不是第一优先级：

1. “是不是 A 没发命令”
2. “是不是 B 完全没回数据”
3. “是不是 MFi session 压根没建立”
4. “是不是 ELM 完全没反应”

这些判断都已经被现有日志基本否定。

---

## 16. 当前一句话总结
当前 iOS B 端 MFi 路径已经打通到 ELM 层，但 ECU 初始化仍失败；失败表现为 `0100` 等探测命令返回 `UNABLE TO CONNECT / BUS INIT: / ERROR`。下一步的核心不是重做 UI，也不是怀疑 WebSocket 整体断链，而是要对照原 CarScanner 继续研究 `MFi -> ELM -> ECU 初始化 / 探活` 这条链路在时序、聚合、保活上的差异。
