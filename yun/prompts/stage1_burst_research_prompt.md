# 任务：仅执行阶段一——源码调研、并发风险排查、Owner 仲裁方案设计、DTO 设计
# 严禁进入实现阶段，严禁改代码

你好，claude code。

你现在只允许执行“阶段一：调研与设计”，禁止进入任何代码实现、文件修改、补丁、重构、依赖安装、页面开发、协议落地、接口新增等动作。

---

## 一、你的唯一目标

基于当前仓库的真实代码状态，完成以下四件事：

1. 还原当前 A/B 双端实时数据链路的真实运行结构；
2. 排查 Burst Snapshot Mode 设想在当前仓库下会遇到的并发/串包/上下文丢失风险；
3. 设计一个适合当前仓库现状的 transport owner / 仲裁方案；
4. 设计离线解析所需的完整 DTO 结构。

你现在**不是来实现功能的**，而是来做：
- 源码调研
- 运行链路分析
- 风险识别
- 方案设计
- DTO 设计
- 文件级改动计划草案

完成后必须停住，等待确认。

---

## 二、最高优先级红线

### 红线 1：全程中文
你的分析、结论、风险说明、设计方案、计划，全部使用中文。

### 红线 2：绝对禁止改代码
本阶段严禁：
- 修改任何文件
- 新建任何文件
- 删除任何文件
- 执行任何补丁
- 生成任何实现代码
- 提交任何 diff
- 安装任何依赖

你只能读取、检索、分析、汇报。

### 红线 3：以代码为准，不以想象为准
仓库中的说明文档可能过期。
如果文档与代码不一致，必须：
1. 明确指出不一致点；
2. 说明应以哪段代码为准；
3. 把该差异列入风险或备注。

### 红线 4：不要把问题误判为“页面开发问题”
本任务首先是：
- transport owner / 仲裁问题
- 单通道蓝牙桥接并发问题
- OBD/ELM327 会话层响应边界问题
- 原版请求生成与解析上下文复用问题

其次才是 UI 问题。
你必须始终按“底层链路问题优先”来分析。

### 红线 5：不要假设可只靠 `SeqID + RawHexResponse` 离线解析
在未证明之前，禁止假设只上传：
- `SeqID`
- `RawHexResponse`

就能在 A 端正确复用原版解析链。
你必须先通过代码证明原版解析最小依赖输入是什么。

### 红线 6：不要先验假设可以 mock keepalive
在没有明确定位原版 keepalive / tester present / online 机制前，
禁止直接建议：
- “B 端 mock 心跳给 A 端”
- “随便回一个 ATRV/0100”
- “本地伪造在线响应”

必须先定位真实机制，再给出方案比较。

### 红线 7：KONNWEI 地址语义不可被改写
`KONNWEI` 是固定经典蓝牙地址，不是随机 BLE 地址。
不要把它当成临时扫描地址或随机地址模型来分析。

### 红线 8：必须等待确认后再进入下一阶段
本轮输出结束后，必须显式停止，并等待我确认。
不允许顺手补充任何实现代码、伪代码实现、文件 patch 建议。

---

## 三、仓库真实路径（必须先核对）

请以以下路径为主进行调研：

### A 端 / WebSocket 桥接层
- `NewUI/yun/websocket_layer/src/OBDCloudManager.cs`
- `NewUI/yun/websocket_layer/src/WebSocketBluetoothConnection.cs`
- `NewUI/yun/websocket_layer/src/WebSocketUIBridge.cs`
- `NewUI/yun/websocket_layer/src/MessageProtocol.cs`

### B 端 / React Native 客户端
- `NewUI/yun/client-app/src/services/CloudBridge.ts`
- `NewUI/yun/client-app/src/hooks/useBluetoothBridge.ts`
- `NewUI/yun/client-app/src/hooks/useCloudBridge.ts`
- `NewUI/yun/client-app/src/hooks/useLocalBluetooth.ts`
- `NewUI/yun/client-app/src/bluetooth/BluetoothManager.ts`
- `NewUI/yun/client-app/src/screens/LiveDataPage.tsx`
- `NewUI/yun/client-app/src/App.tsx`

### 原版 / 反编译源码
- `NewUI/yun/CarScannerC#反编译后的源码/OBD2/OBDDataReader.cs`
- `NewUI/yun/CarScannerC#反编译后的源码/OBD2/OBDRequest.cs`
- `NewUI/yun/CarScannerC#反编译后的源码/OBD2/CarData.cs`
- `NewUI/yun/CarScannerC#反编译后的源码/OBD2/RequestProducers/RequestProducerStatic.cs`
- `NewUI/yun/CarScannerC#反编译后的源码/OBD2/RequestProducers/MainAppRequestProducer.cs`
- `NewUI/yun/CarScannerC#反编译后的源码/ViewModels/LiveDataPIDModel.cs`
- `NewUI/yun/CarScannerC#反编译后的源码/LiveDataListPage.cs`
- `NewUI/yun/CarScannerC#反编译后的源码/LiveDataChartPage.cs`

### 注入 / 补丁工具
- `NewUI/yun/tools/JSBridgePatcher`
- `NewUI/yun/tools/DllInjector`

### 参考文档（仅辅助）
- `NewUI/yun/关于实时数据流架构的分析.md`
- `NewUI/yun/当前实时数据流架构.md`

注意：
文档只作辅助，不得替代代码事实。

---

## 四、你必须先确认的现实前提

在开始分析前，请先基于代码验证并接受以下现实前提；如果其中任何一条不成立，请指出并纠正：

1. 当前实时 PID 主循环的真正 owner 仍是 A 端原版 `OBDDataReader`，不是 B 端。
2. B 端当前主要承担：
   - WebSocket 桥接
   - 本地蓝牙连接
   - 数据透传
   - UI 展示
3. 当前 A→B→ELM327 与 ELM327→B→A 是单通道桥接，不存在现成的命令级 request/response correlation 层。
4. 原版请求生成不是“PID -> 单条 Hex”的简单映射，而可能经过：
   - `LiveDataPIDModel.GetRequests(...)`
   - `RequestProducerStatic.UpdateOBDReaderRequests()`
   - `OBDRequestQueueOptimizer.Optimize(...)`
5. 原版解析链并不一定支持“只给原始响应 Hex 即可离线解析”，很可能依赖 `command / header / request / pids / timeStamp / response_header` 等上下文。
6. 当前项目中 keepalive / 在线探活行为很可能仍和 `OBDDataReader` 主循环有关，而不是普通页面逻辑。
7. 用户观察到原 CarScanner 连接 ELM327 后，OBD/Host 灯应持续闪烁；当前项目只闪几下即停，这说明 keepalive / online 机制很可能尚未完整保留。

---

## 五、调研任务（必须逐项完成）

你必须完成以下 6 个主题的调研，并给出清晰结论。

---

### 主题 1：当前实时数据链路真实路径还原

请还原当前“实时数据”从 B 端发起到最终回显的真实路径，回答：

1. B 端 `LiveDataPage` 当前如何发起“开始读 PID”？
2. A 端收到后如何进入原版读取逻辑？
3. 原版数据回调如何重新转成 WebSocket 事件？
4. B 端如何接收并更新 UI？
5. 当前 `startReadPIDs` / `stopReadPIDs` 路径中，A/B 双方各自真正负责什么？

要求：
- 用“文字版链路图”清晰描述
- 标明关键文件与方法
- 明确指出当前架构和直觉上“B 本地直接读 ECU”有何不同

---

### 主题 2：命令生成链调研

请定位原版实时 PID 请求是如何生成的，回答：

1. 当前 PID 列表选择最终如何变成 `OBDRequest` 列表？
2. 哪些字段会参与请求定义？
   - command
   - header
   - beforeCommands
   - afterCommands
   - repeat
   - checkLength
   - pids
   - responseMarker
   - skipCycles
3. `RequestProducerStatic` 的职责是什么？
4. `MainAppRequestProducer` 的职责是什么？
5. `LiveDataPIDModel.GetRequests(...)` 在这条链路中的作用是什么？
6. `OBDRequestQueueOptimizer.Optimize(...)` 会对 Burst 模式的设计产生什么影响？
7. 如果未来要把请求描述从 A 端发给 B 端，本阶段你认定的“最小完整请求描述”必须包含哪些字段？

重点：
- 不要停留在“找到一个方法名”
- 要说明“请求上下文”是如何形成的

---

### 主题 3：解析链调研

请定位原版从 ECU 响应到 PID 值变化的完整解析链，回答：

1. `OBDDataReader` 在什么层调用解析？
2. `CarData.Decode(...)` 在整体链路中的角色是什么？
3. `OBDRequest.PIDs` 在解析中是否关键？
4. `request.Command`、`request.Header`、`response_header`、`timeStamp` 是否参与解析或分发？
5. 解析最终是统一在一个总入口完成，还是仍会分发到各 PID 的 `Decode(...)`？
6. 如果只提供 `SeqID + RawHexResponse`，为什么不可靠？
7. 你认定的“离线复用解析链”的最小必要输入是什么？

要求：
- 给出你认为最适合后续离线复用的解析切入层
- 但本阶段不要写任何实现代码
- 必须明确指出“哪些字段绝不能丢”

---

### 主题 4：并发、串包与 ELM327 会话层风险

请基于当前桥接实现，分析以下问题：

1. 当前 A 端写蓝牙命令到 B 的路径是什么？
2. 当前 B 端如何把 A 下发的命令写到本地蓝牙？
3. 当前 ELM327 返回数据后，是如何回到 A 端并进入读取缓冲的？
4. 当前系统里是否存在命令级 request/response correlation？
5. 如果在 Burst 模式下让 B 端自己额外直接发命令，会发生哪些具体冲突？
6. 当前 B 端是否已经具备：
   - 按命令等待完整响应直到 `>`
   - 响应组包
   - 每条命令的独立超时
   - 多路请求的可靠分发
7. 如果这些能力没有现成实现，缺口具体在哪里？

必须特别说明：
- 为什么这个任务首先是 transport owner / broker / 会话层问题
- 而不是“新建一个页面 + 多发几条命令”这么简单

---

### 主题 5：keepalive / online 机制调研

请重点核查原版 keepalive / online 机制，回答：

1. `OBDDataReader` 在请求队列为空时会做什么？
2. 当前 ping / tester present / online probe 的实际候选命令是什么？
3. 它们是在什么条件下触发？
4. 这一机制是否与用户观察到的“原版连接后 OBD/Host 灯持续闪烁”一致？
5. 当前 A/B WebSocket 桥接是否完整保留了该机制？
6. 如果没有完整保留，缺口在哪里？
7. 在 Burst 模式期间，最安全的处理方向是：
   - A 继续作为唯一 owner 并统一调度？
   - 明确 owner 切换并暂停 A 的正常请求？
   - 还是别的方案？
8. 为什么不能轻率地建议“B 端直接 mock 一个心跳回复”？

---

### 主题 6：Owner 仲裁方案与 DTO 设计

#### Part A：Owner / 仲裁方案比较
请至少比较以下两类方案：

##### 方案 A：独占 owner 切换
- Burst 会话开始前，明确切换 transport owner
- 在 Burst 窗口中，只允许一方主动下发诊断命令
- 采样结束后再归还 owner

##### 方案 B：单通道 broker 多路复用
- 所有请求仍走统一发送器
- 由 broker 负责：
  - 请求排队
  - 响应边界识别
  - keepalive 仲裁
  - 命令级关联

请对两种方案给出：
1. 适配当前仓库的可行性
2. 所需底层改动量
3. 风险大小
4. 对现有稳定路径的影响
5. 你最终推荐哪一种
6. 推荐理由是什么
7. 当前阶段下，最小可行设计应优先保住什么，不该一开始追求什么

#### Part B：DTO 设计
请给出你建议的 DTO 结构定义，只需设计字段和含义，不要写实现代码。

至少设计以下对象：

##### 1. `BurstRequestDescriptor`
至少考虑字段：
- `seqId`
- `command`
- `header`
- `beforeCommands`
- `afterCommands`
- `checkLength`
- `responseMarker`
- `skipCyclesTarget`
- `pidIds`
- `pidNames`
- `source`
- 你认为必需的其他上下文

##### 2. `BurstSampleEnvelope`
至少考虑字段：
- `sessionId`
- `seqId`
- `command`
- `header`
- `rawElmText`
- `normalizedPayloadHex`
- `responseHeader`
- `timestampMs`
- `elapsedMs`
- `promptSeen`
- `timeout`
- `transportError`
- `parseHint`
- 你认为后续排障必需的字段

##### 3. `BurstParsedResult`
至少考虑字段：
- `sessionId`
- `seqId`
- `pidId`
- `pidName`
- `value`
- `rawValue`
- `unit`
- `displayText`
- `timestampMs`
- `parseOk`
- `error`

##### 4. `BurstSessionState`
至少考虑字段：
- `sessionId`
- `mode`
- `owner`
- `keepalivePolicy`
- `startedAt`
- `finishedAt`
- `status`
- `sampleCount`
- `parseSuccessCount`
- `parseFailCount`
- `error`

同时请回答：
1. 哪些字段绝不能省？
2. 哪些字段虽然看起来冗余，但对排障和离线复用至关重要？
3. 哪些字段绝不能等到实现阶段再临时补？

---

## 六、输出要求（必须严格按此结构）

你最终必须按下面结构输出，禁止自由发挥成散文。

### 1. 代码现实结论
- 用中文总结当前仓库真实架构
- 指出哪些点和直觉想象不同

### 2. 当前实时链路图
- 用文字链路图描述
- 标注关键方法 / 类 / 文件

### 3. 命令生成链结论
- 定位关键方法
- 说明请求描述最小必要字段
- 指出优化器对设计的影响

### 4. 解析链结论
- 定位关键方法
- 说明最适合复用的离线解析切入层
- 明确说明为什么不能只传 `SeqID + RawHexResponse`

### 5. 并发与串包风险清单
- 列出当前最危险的冲突点
- 指出是哪个层面的问题：
  - transport
  - broker
  - session
  - parsing context
  - keepalive

### 6. keepalive / online 机制结论
- 说明原版机制
- 说明当前桥接是否完整保留
- 说明对 Burst 设计的约束

### 7. Owner 仲裁方案比较
- 比较方案 A / 方案 B
- 必须给出最终推荐
- 说明推荐理由与保守性考虑

### 8. DTO 设计
- 给出对象列表
- 给出字段说明
- 明确哪些字段不可省略

### 9. 文档与代码不一致点
- 若发现过期文档或错误描述，逐项列出
- 明确说明应以哪段代码为准

### 10. 第二阶段前的待确认问题
- 列出需要我确认的点
- 仅列问题，不要开始实现

### 11. 停止
最后必须明确写出：
- “我已完成阶段一调研与设计，当前不进入实现阶段，等待你的确认。”

---

## 七、禁止事项（再次强调）

在本轮中，严禁：
- 写实现代码
- 写伪实现代码
- 写 patch
- 写 diff
- 新增接口
- 新增页面
- 修改协议
- 安装依赖
- 输出“顺手帮你实现一版”
- 进入任何第二阶段内容

如果你分析完后认为后续需要实现，也只能写在：
- “第二阶段前的待确认问题”
里，绝不能直接动手。

---

## 八、现在开始

请现在开始执行：
- 仓库源码调研
- 并发风险排查
- Owner 仲裁方案比较
- DTO 设计

完成后严格按“六、输出要求”汇报，并停住等待确认。
