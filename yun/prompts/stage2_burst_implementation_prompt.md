# 任务：进入阶段二——基于已确认的阶段一结论，实施 Burst Snapshot Mode 的最小可行版本
# 重要：优先保护现有稳定路径；允许最小必要的底层改造；禁止进入错误的“复制第二套链路”路线

你好，claude code。

阶段一调研已完成，并且其总体方向已经得到确认。
你现在进入阶段二：在当前仓库中实现 Burst Snapshot Mode 的**最小可行版本（MVP）**。

但请注意：
这个任务不是普通的“新页面 + 新接口”开发，而是一个以 **transport owner / 会话仲裁 / 响应组包 / 原版解析链复用** 为核心的功能实现任务。

---

## 一、你现在必须接受的已确认前提

以下前提已经过阶段一确认，除非你在代码中发现新的直接证据，否则实现时应以这些结论为基础：

1. 当前实时 PID 主循环的真实 owner 仍是 A 端原版 `OBDDataReader`，不是 B 端。
2. B 端当前主要负责：
   - WebSocket 桥接
   - 本地蓝牙连接
   - 数据透传
   - UI 展示
3. 当前 A→B→ELM327 与 ELM327→B→A 仍是单通道桥接，没有现成的命令级 request/response correlation 层。
4. 原版请求生成不能简化为“PID → 单条命令”，它依赖：
   - `LiveDataPIDModel.GetRequests(...)`
   - `RequestProducerStatic.UpdateOBDReaderRequests()`
   - `OBDRequestQueueOptimizer.Optimize(...)`
5. 原版解析链不能简化为“只传 `SeqID + RawHexResponse`”，而是依赖完整上下文，至少涉及：
   - `command`
   - `header`
   - `request / request.PIDs`
   - `response_header`
   - `timestamp`
   - 原始响应文本 / 数据
6. Burst 模式最优先的实施方向是：
   - **独占 owner 切换**
   - 而不是一开始就做完整的 broker 多路复用
7. 用户观察：原 CarScanner 连接 ELM327 后 OBD/Host 灯应持续闪烁（持续探活/在线机制）；当前项目只闪几下即停。
   这意味着 keepalive / online 机制在实现 Burst 时必须被认真保护和恢复。
8. `KONNWEI` 是固定经典蓝牙地址，不是随机 BLE 地址，不要误处理其地址语义。

---

## 二、最高优先级红线（凌驾一切）

### 0.1 现有稳定路径保护原则
- 目标是：在**不破坏当前稳定功能**的前提下，实现 Burst 模式
- Burst 模式必须通过**新开的 UI 页面**和**新开的 WebSocket Action / Session 入口**触发
- **禁止无必要地改写**当前正常运行的既有业务路径
- 但如果阶段一结论证明：实现 Burst 模式必须对现有 `transport / session / keepalive / owner` 层做**最小必要改造**，则允许进行**受控、小范围、可解释**的底层调整
- **优先复用原有请求生成、解析、连接管理逻辑**
- **禁止为了“纯加法”而复制出一套与原逻辑脱节的新链路**
- 判断标准：**当 Burst 模式未启用时，现有主路径行为必须保持等价，现有稳定功能不得被破坏**

### 0.2 严禁 A/B 同时主动向同一蓝牙会话发诊断命令
- Burst 期间，禁止出现：
  - A 端仍在同一会话上继续正常 PID 轮询
  - B 端同时自己向 ELM327 下发 Burst 命令
- 必须有**明确的 transport owner**
- 在任一时刻，只允许一方拥有该诊断通道的主动写权限

### 0.3 严禁“只复制逻辑、不处理上下文”
- 不允许通过“自己拼命令 + 自己猜公式 + 自己简化解析”的方式实现 Burst
- 必须复用：
  - 原版请求生成链
  - 原版解析链
  - 原版连接/状态管理语义
- 如果无法直接调用原逻辑，必须说明阻碍点，并采用“最小上下文还原”的方式间接复用

### 0.4 严禁在 B 端直接复制一套完整 CarData/PID 解析体系
- B 端不应复制原版 C# 的完整解析逻辑
- 离线解析优先仍在 A 端完成
- B 端只负责：
  - 采样
  - 组包
  - 暂存
  - 上传
  - 展示

### 0.5 严禁先验假设可以 mock keepalive
- 在没有清楚证明前，不要默认：
  - B 端伪造心跳响应
  - B 端随便返回 `0100` / `ATRV` / `TesterPresent`
- 必须优先采用：
  - owner 切换
  - 受控 keepalive 策略
  - 采样后恢复
- 若必须做最小 keepalive 支持，必须先说明依据

### 0.6 全程中文
- 所有分析、计划、说明、提交说明全部使用中文

### 0.7 修改前先说明并等待确认
- 在真正开始修改代码前，先用中文输出：
  - 拟修改文件列表
  - 每个文件的职责
  - 为什么必须改它
  - 改动顺序
- 然后停住，等待我确认
- 只有在我确认后，才开始实际修改代码

---

## 三、你要实现的目标（阶段二范围）

你的目标不是一次性做全功能终态，而是实现**最小可行且可验证的 Burst Snapshot Mode**。

### MVP 必须具备
1. A 端能够生成用于 Burst 的**完整请求描述**
2. B 端能够在 Burst 会话期间成为**唯一主动写 owner**
3. B 端能够按请求描述顺序执行命令，并完成**完整 ELM 响应组包**
4. B 端能够本地缓存样本，并将样本批量回传给 A 端
5. A 端能够基于原版解析链，尽可能复用现有逻辑完成离线解析
6. B 端能够把解析结果与本地时间戳合并并显示
7. Burst 结束、异常中断、超时、蓝牙断开后，系统能恢复 owner 和状态

### 本阶段先不追求
- 完整 broker 多路复用
- B 端独立解析能力
- 复杂图表动画
- 所有边界条件一次性全部覆盖
- 对现有实时页做大规模重构

---

## 四、优先实施策略（必须遵守）

### 策略 1：采用“独占 owner 切换”而不是 broker 多路复用
优先实现：

1. Burst 会话开始前：
   - B 端请求 A 端准备 Burst Session
   - A 端生成 BurstRequestDescriptor 列表
   - A 端进入 Burst 会话态
   - A 端暂停“正常业务请求写入”或切换诊断 owner
2. Burst 会话期间：
   - B 端成为唯一主动发送 Burst 诊断命令的一方
   - A 端不得继续在同一会话上主动下发普通实时 PID 请求
3. Burst 结束后：
   - B 端提交样本
   - A 端离线解析
   - A 端恢复原 owner / keepalive / 正常状态

注意：
- 这里说的是“暂停正常业务请求写入 / 切换 owner”
- **不要先入为主地把它实现成“强行彻底停掉 OBDDataReader 主循环”**
- 除非代码证明这是必要且更安全的

### 策略 2：先补“完整响应组包层”，再做 B 端 Burst 轮询
当前 B 端没有现成的“按命令等待完整响应直到 `>`”能力。
因此必须先实现一个**受控的响应组包层**，至少要支持：

- 从本地蓝牙收包中累计原始文本
- 正确判断一条 ELM 响应何时完整
- 识别终结符 `>`
- 处理超时
- 保留原始文本 `rawElmText`
- 生成可上传的 `normalizedPayloadHex` / `responseHeader`

禁止假设：
- 一次蓝牙事件就是一条完整 ELM 响应
- 一次 WebSocket 消息就是一条完整命令响应

### 策略 3：离线解析优先仍在 A 端
A 端必须尽可能复用原版解析链。
优先思路：

- 用 A 端生成的请求描述来保存解析上下文
- 在离线解析阶段，根据 descriptor + sample 重建最小必要的请求上下文
- 尽可能调用原版 `CarData.Decode(...)` / `PID.Decode(...)` 路径
- 不允许简单根据 PID ID 自己手写公式

### 策略 4：keepalive 采用“受控策略”，不要随意伪造
你需要为 Burst 会话设计一个明确的 keepalive policy，例如：

- `paused`
- `minimal`
- `normal`

但不要先验假设一定能 mock。
实现时优先考虑：
- Burst 窗口足够短
- owner 切换期间正常轮询不写入
- 会话结束后恢复原行为

---

## 五、建议新增的协议 / 会话动作（可在实现时微调命名）

建议使用会话语义，而不是简单动作语义。

### A/B 间新增 action（建议）
- `prepareBurstSession`
- `commitBurstSamples`
- `abortBurstSession`

如你认为还需要更细的动作，可补充，但必须保持命名清晰、职责单一。

### 不建议
- 直接沿用 `startReadPIDs` 做变体分支
- 在现有实时路径里大量散落 `if (isBurstMode)`

对外入口可以是新 action，
但内部实现优先使用“集中式 session / owner 状态”管理。

---

## 六、你要落地的对象模型（必须实现）

以下对象以阶段一 DTO 设计为基础实现，允许你在不违背原语义的前提下补充字段。

### 1. `BurstRequestDescriptor`
至少要覆盖：
- `seqId`
- `command`
- `header`
- `pidIds`
- `pidNames`
- `beforeCommands`
- `afterCommands`
- `checkLength`
- `responseMarker`
- `skipCyclesTarget`
- `isMultiRequest`
- `source`

建议补充：
- `obdMode`
- `timeoutMs`
- `descriptorVersion`
- `constituentPidIds`

### 2. `BurstSampleEnvelope`
至少要覆盖：
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

建议补充：
- `chunkCount`
- `completedBy`（如 `prompt` / `timeout` / `abort`）
- `parseHint`

### 3. `BurstParsedResult`
至少要覆盖：
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

### 4. `BurstSessionState`
至少要覆盖：
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

建议补充：
- `abortReason`
- `ownerBeforeBurst`
- `ownerAfterBurst`

---

## 七、A 端实现要求

A 端实现重点在于：**会话控制、请求描述生成、离线解析复用、状态恢复**

### 必做项
1. 新增 Burst Session 状态管理
2. 新增 Burst 相关 action 处理
3. 在准备阶段生成 `BurstRequestDescriptor[]`
4. 在 Burst 会话期间明确切换 owner / 写权限
5. 在提交阶段接收 `BurstSampleEnvelope[]`
6. 尽可能复用原版解析链进行离线解析
7. 结束或异常时恢复原 owner 和状态

### 必须优先复用的东西
- 原版请求生成链
- 原版解析链
- 原版连接状态与 OBDDataReader 语义

### 禁止事项
- 不要自己手写 PID 公式替代原版解析
- 不要为了 Burst 复制第二套“伪 OBD 内核”
- 不要偷偷改掉现有实时数据主路径的语义

---

## 八、B 端实现要求

B 端实现重点在于：**Burst 会话控制、本地顺序采样、响应组包、本地缓存、结果展示**

### 必做项
1. 新增 Burst 模式页面
2. 支持选择 PID、输入采样时长或采样轮数
3. 请求 A 端准备 Burst Session
4. 接收 `BurstRequestDescriptor[]`
5. 在 Burst 会话内顺序执行命令
6. 完成原始 ELM 响应组包
7. 生成并暂存 `BurstSampleEnvelope[]`
8. 将样本批量提交给 A 端
9. 接收 `BurstParsedResult[]`
10. 将解析结果与本地时间戳合并显示

### UI 范围
优先实现：
- PID 选择
- 时长输入
- 采样状态
- 结果表格
- 基础回放视图

如果当前仓库没有图表依赖：
- **不要擅自安装图表库**
- 先把表格 + 基础回放做通
- 若后续要加图表，需先说明并等待确认

### 禁止事项
- 不要绕开现有蓝牙网关另起第二条蓝牙通路
- 不要假设蓝牙原生事件天然等于完整响应
- 不要把解析逻辑复制到 B 端

---

## 九、日志与可观测性要求

这次实现必须带足够日志，便于我们定位会话问题。

至少记录：
- Burst session 创建 / 开始 / 结束 / 中止
- owner 切换
- keepalive policy 变化
- descriptor 数量
- sample 数量
- parse 成功数 / 失败数
- 单条命令耗时
- 响应是否完整（是否见到 `>`）
- 超时 / 传输错误 / 恢复路径

但注意：
- 日志要聚焦关键状态
- 不要把正常高频路径刷成不可读

---

## 十、验证要求

在完成修改后，你必须验证以下内容：

### 1. Burst 未启用时
- 现有 `LiveDataPage` 主路径行为保持等价
- 现有实时数据功能未被破坏

### 2. Burst 启用时
- 能成功创建 Burst Session
- B 端能顺序执行 Burst 请求
- B 端能正确识别完整响应边界
- 样本能成功上传
- A 端能完成离线解析
- B 端能展示结果

### 3. 异常场景
至少验证：
- Burst 期间超时
- Burst 期间蓝牙断开
- Burst 中止
- Burst 结束后 owner 恢复

如果某些验证你无法完全执行，也必须在最终说明中明确指出。

---

## 十一、工作方式（必须遵守）

### 第一步：先输出实施计划，不改代码
先用中文输出：
1. 你准备修改哪些文件
2. 每个文件修改目的是什么
3. 为什么必须改它
4. 改动顺序是什么
5. 哪些属于新增文件
6. 哪些属于最小必要底层改动

然后停住，等待我确认。

### 第二步：收到我确认后再开始修改
只有在我明确回复确认后，才开始实际修改文件。

### 第三步：每完成一组关键改动，给出中文进度说明
重点说明：
- 已完成什么
- 还剩什么
- 是否触及底层 transport / owner / keepalive

### 第四步：完成后给出总结
总结必须包括：
- 修改文件列表
- 核心实现点
- 对现有主路径的影响
- 已验证内容
- 未验证风险

---

## 十二、禁止事项（再次强调）

在本轮实现中，严禁：
- 把问题降格成“只是新页面开发”
- 同时允许 A/B 主动写同一蓝牙会话
- 复制第二套与原逻辑脱节的请求生成 / 解析逻辑
- 用手写公式替代原版解析链
- 未经说明擅自安装新依赖
- 未经确认直接开始修改代码

---

## 十三、现在开始

请先基于上述约束，**只输出第一步实施计划**：
- 拟改文件
- 改动目的
- 改动顺序
- 为什么这些改动是最小必要的

此时不要改代码，等待我的确认。
