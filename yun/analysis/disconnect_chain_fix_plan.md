# 断开连接链路修复计划（冻结版）

更新时间：2026-03-31

## 1. 目的

本文件用于冻结当前对“断开连接链路”问题的理解、修复方向、实施顺序与红线规范，避免后续因长上下文或多轮协作导致目标偏移。

后续围绕断开链路的分析、改码、测试，均应以本文件为准；若实际情况与本文件冲突，应先更新本文件，再继续实施。

---

## 2. 项目阶段边界（必须统一认识）

当前项目不是单一架构，而是经历了三个阶段：

### 阶段一：原 CarScanner 软件

- 本地运行。
- 前端与业务逻辑都在原软件内部。
- 真正的源码核心是 C# / Xamarin / OBDDataReader 等原生逻辑。

### 阶段二：本地 WebView + JSBridge 架构

- 仍然是本地运行。
- UI 改成了 WebView 层。
- WebView 通过 JSBridge 直接调用原软件 C# 异步方法。
- 阶段二的 A 端 UI 断开按钮体验，是本次断开问题的最直接参照系。

### 阶段三：当前云端 A + 本地 B 架构

- 云端 A：是在阶段二软件基础上继续修改得到。
- 本地 B：React Native 客户端。
- A/B 之间通过 WebSocket 通信。
- B 端负责本地蓝牙 transport；A 端负责复用原 CarScanner / 阶段二的诊断逻辑。

### 当前修复的判定基准

- **行为参照优先级：阶段二断开按钮逻辑 > 当前阶段三现状。**
- 阶段一可作为底层语义参考，但本次不能简单按“阶段一 UI 长什么样”来判断对错。

---

## 3. 当前问题现象（已确认）

用户已经确认：

- 实时数据流分页累计问题已基本修复。
- 当前最明显的不一致，转移到了“断开连接”这一步。
- 当前阶段三断开手感、时序与阶段二 / 原版存在差别。

当前差异主要表现为：

- 断开操作的主导权不在 A 端，而是 B 端先本地断蓝牙。
- A 端收到断开请求后，会提前把链路当成“已经断完”。
- 远端 `disconnect` 事件没有优先按当前 `sessionId` 精确匹配。
- 为规避旧断开尾声误伤新连接，系统长出多层时间窗/保护补丁。

---

## 4. 已确认的根因

## 4.1 根因一：B 端先断本地蓝牙，导致断开主导权倒置

当前 B 端首页点击断开时，执行顺序是：

1. `getBluetoothGateway()?.disconnect()`
2. `disconnectOBD()`

这意味着：

- transport 先在 B 端被切断；
- A 端只能在“链路已经断了一半”的前提下补做逻辑收尾；
- 这与阶段二“UI 只发起 `disconnectAsync`，底层自己完成断开”的模型不一致。

这个问题是当前断开体验偏离原版/阶段二的第一根因。

## 4.2 根因二：A 端 `HandleObdDisconnectAsync()` 把“请求断开”当成“已经断完”

当前 A 端断开主逻辑存在以下路径：

1. 先 `ForceReset()`，直接把 `_bluetoothConnection` 内部状态清空；
2. 再调用 `InvokeJsBridge("disconnectAsync")`；
3. 随后立刻 `ResetOBDDataReaderWebSocket()`；
4. 立刻 `_obdConnection.ResetSession()`；
5. 手工向 B 端发送 `OBDStatusChanged("Disconnected")`。

这条链路的问题在于：

- 它不是“等待真实断开完成”后再收口；
- 而是“请求刚发出，就把状态与会话先收掉”；
- 本质上是一种“伪完成态”。

这就是为什么当前系统需要：

- `ForceReset`
- `allowRemoteDisconnectUntil`
- `ignoreDisconnectedUntil`

等一系列补丁来防止旧断开尾声污染新连接。

## 4.3 根因三：B 端远端断开逻辑没有优先按 `sessionId` 精确匹配

当前 `handleBluetoothDisconnect(message)` 的主要判定方式是：

- 只要当前时间还在 `allowRemoteDisconnectUntil` 窗口内，就允许执行本地蓝牙断开。

它的问题是：

- 没有优先检查 `message.sessionId` 是否等于 `currentSessionId`；
- 因此它更像“时间窗放行”，而不是“当前会话精确断开”；
- 这样会加剧旧会话尾声、重连竞态等问题。

## 4.4 根因四：当前阶段三对 `disconnectAsync` 的使用方式，偏离了阶段二语义

需要特别澄清：

- `JSBridge.disconnectAsync` 在桥接暴露层只是“触发断开动作”；
- 阶段二 UI 之所以体验好，不是因为它同步等待函数返回；
- 而是因为它只负责触发，然后等待 `onOBDStatusChanged` 驱动 UI。

当前阶段三的问题，不是 `disconnectAsync` 本身不对，而是：

- B 端先断了；
- A 端又在调用 `disconnectAsync` 后提前伪造完成态；
- 最终使断开链路不再是“原版式的真实完成收口”。

---

## 5. 原版 / 阶段二 的目标语义（修复目标）

本次修复要回到下面这个模型：

1. **B 端点击断开时，不先主动本地断蓝牙。**
2. **B 端只向 A 端发起 `OBDDisconnect` 请求。**
3. **A 端进入原有断开逻辑。**
4. **当 A 端底层 transport 真的需要断开时，再向 B 端发送带 `sessionId` 的 `disconnect` 请求。**
5. **B 端只对当前会话执行本地断开。**
6. **UI 主要依赖真实的 `OBDStatusChanged("Disconnected")` 来收口。**

一句话概括：

> 恢复“A 端主导断开，B 端按当前会话执行 transport 断开，UI 等真实状态完成”的模型。

---

## 6. 修改策略（已定）

本次采用 **方案二**，而不是直接大回退。

### 方案二的核心思想

- 纠正断开主导权；
- 尽量恢复阶段二行为；
- 但暂时保留保护补丁，避免一次性把竞态保护全部拆掉；
- 先把主路径改对，再根据实测决定哪些保护补丁可以收缩。

### 为什么不用“直接回退原版”

因为当前阶段三已经存在以下额外复杂性：

- A/B 分离；
- sessionId 会话管理；
- WebSocket transport 代理层；
- 旧断开尾声可能晚于新连接建立；
- replay / burst 等附加流程。

因此不能简单地：

- 直接删 `ForceReset`；
- 直接删 `ignoreDisconnectedUntil`；
- 直接删 `allowRemoteDisconnectUntil`；
- 直接认为“只要改回 `disconnectAsync` 就万事大吉”。

---

## 7. 第一批修复内容（必须一起改）

第一批是当前最关键的一次原子修复，三个落点需要一起改，不能只改其中一个。

## 7.1 B 端首页断开入口改顺序

目标：

- 用户点击断开时，不再先本地断蓝牙；
- 改成只请求 A 端断开。

修改落点：

- `NewUI/yun/client-app/src/screens/HomePage.tsx`

计划动作：

- 删除“先 `getBluetoothGateway()?.disconnect()` 再 `disconnectOBD()`”的当前顺序；
- 改成只 `await disconnectOBD()`；
- 保留现有“正在断开”遮罩与保险计时器，先不改 UX 收口逻辑。

## 7.2 B 端远端 `disconnect` 改成优先按 `sessionId` 精确匹配

目标：

- 远端断开必须优先针对当前会话；
- 时间窗从“主判定”降级为“辅助保护”。

修改落点：

- `NewUI/yun/client-app/src/services/CloudBridge.ts`

计划动作：

- 在 `handleBluetoothDisconnect(message)` 中优先判断：
  - `message.sessionId` 是否存在；
  - 是否等于 `currentSessionId`。
- 只有匹配当前会话时，才执行本地蓝牙断开。
- 若 `sessionId` 不匹配，应记录日志并忽略。
- `allowRemoteDisconnectUntil` 暂时保留，但不再作为唯一放行依据。

## 7.3 A 端断开收口从“提前伪完成”改为“真实断开完成后收口”

目标：

- 正常断开路径不再一上来就 `ForceReset()`；
- 不再刚发起断开就手工宣布 `Disconnected`；
- 把真正的收口动作尽量放到真实断开完成点上。

修改落点：

- `NewUI/yun/websocket_layer/src/OBDCloudManager.cs`

计划动作：

- 调整 `HandleObdDisconnectAsync()`：
  - 不再沿用“`ForceReset()` + 手工发 `Disconnected`”的主路径；
  - 先以阶段二语义触发断开流程；
  - 收口逻辑尽量转移到真实状态变更上。
- 评估利用 `OBDDataReader.StatusChanged` 的 `Disconnected` 作为统一收口点：
  - 到了真实 `Disconnected` 后，再做 `ResetOBDDataReaderWebSocket()`；
  - 再做 `_obdConnection.ResetSession()`；
  - 再结束相关断开流程。

---

## 8. 第二批修复内容（第一批稳定后再做）

第二批不是立即动，而是第一批实机验证通过后再评估。

## 8.1 重新评估 `allowRemoteDisconnectUntil`

如果第一批完成后，`sessionId` 精确匹配已经足以约束远端断开，则：

- 可以考虑缩短时间窗；
- 甚至把它降级为更弱的兼容保护。

## 8.2 重新评估 `ignoreDisconnectedUntil`

如果 A 端不再提前制造“伪完成态”，则：

- 旧 `disconnectAsync` 尾声污染新连接的概率会下降；
- 这时再评估是否仍需保持当前 10s 窗口；
- 或者是否能缩短为更保守的窗口。

## 8.3 重新评估 `ForceReset`

注意：

- `ForceReset` 当前不是毫无意义；
- 它主要用于防止旧断开尾声在更晚时刻清掉新 session。

因此：

- 第一批修复前，不能贸然删除它；
- 第一批稳定后，才评估它是否应该只留在 replay / 异常恢复路径，退出普通用户断开主路径。

---

## 9. 红线规范（实施时必须遵守）

以下内容属于本轮修复的硬性红线：

### 9.1 不能直接删保护补丁

在第一批修复验证前，**禁止**直接删除：

- `ForceReset`
- `ignoreDisconnectedUntil`
- `allowRemoteDisconnectUntil`

原因：

- 它们虽然丑，但当前承担着竞态保护责任；
- 必须先把主路径纠正，再决定是否收缩。

### 9.2 不能把“请求返回”当成“断开完成”

后续实现中，**禁止**把以下任一事件直接当成断开完成：

- `disconnectOBD()` Promise 返回；
- `InvokeJsBridge("disconnectAsync")` 调用成功；
- A 端收到 `OBDDisconnect` 请求。

断开完成的主判据应该是：

- 真实 `Disconnected` 状态；
- 或底层真实 transport / reader 完成断开后的统一收口点。

### 9.3 不能继续让 B 端先本地断蓝牙再通知 A 端

这是本轮要修复的核心偏差之一。

### 9.4 不能把 `KONNWEI` 当作随机 BLE 地址处理

项目约定已明确：

- `KONNWEI` 为固定经典蓝牙地址；
- 不能按 BLE 随机扫描地址、临时地址或会变地址来处理。

### 9.5 本轮不要混入“持续探活/OBD灯闪烁”问题一起改

用户已明确观察到：

- 原 CarScanner 连接 ELM327 后，OBD / Host 灯应持续闪烁；
- 当前只闪几下即停；
- 后续需要单独研究 CarScanner C# 中的探活逻辑。

但本轮断开修复中：

- **不要把持续探活问题混入同一批改动。**

### 9.6 每次实际改码前，先说明再执行

项目约定要求：

- 用户已允许修改代码；
- 但每次修改前，仍需先说明方案并获得确认。

---

## 10. 验证标准

第一批修复后，至少验证以下场景：

## 10.1 正常连接后主动断开

预期：

- B 端点击断开，不立即先断本地蓝牙；
- A 端进入断开流程；
- 当 A 端真实要求 transport 断开时，B 端收到带当前 `sessionId` 的 `disconnect`；
- B 端执行本地断开；
- 最终收到 `Disconnected` 并收口 UI。

## 10.2 断开后立即重连

预期：

- 旧断开尾声不会误断新会话；
- 不会出现“刚连上又被旧断开打掉”；
- `sessionId` 精确匹配应该能起作用。

## 10.3 连接中途点击取消 / 断开

预期：

- 不残留半断开态；
- 不出现“B 端蓝牙断了，但 A 端状态还卡着”的问题；
- 遮罩最终能正确收口。

## 10.4 iOS / Android 都要验证

其中 Android 还要注意：

- USB 连接到 Mac；
- Metro 常驻；
- 端口代理正常；
- 避免把代理/Metro 问题误判为断开逻辑问题。

---

## 11. 本轮不处理的内容

以下内容不属于本文件对应的第一批断开修复范围：

- 实时数据流分页累计问题（已基本修复，不在本轮继续展开）；
- OBD / Host 灯持续闪烁的探活逻辑；
- 更大范围的 replay / burst 重构；
- 蓝牙扫描策略调整；
- Metro / adb reverse 之外的开发环境问题。

---

## 12. 当前建议的实施顺序

1. 先按本文件完成第一批修复的代码改动。
2. 实机验证：正常断开、断开后立刻重连、连接中取消。
3. 如果第一批稳定，再进入第二批：缩减时间窗/保护补丁。
4. 断开链路稳定后，再单独研究“持续探活/OBD灯闪烁”问题。

---

## 13. 一句话冻结结论

当前阶段三断开问题的核心，不是“不会断开”，而是：

> **断开主导权被倒置，A 端又提前伪造了完成态，B 端还没有按当前 `sessionId` 精确执行 transport 断开。**

因此本轮修复的主目标不是“再加补丁”，而是：

> **恢复 A 端主导断开、B 端按当前会话执行断开、UI 等真实断开完成状态收口。**
