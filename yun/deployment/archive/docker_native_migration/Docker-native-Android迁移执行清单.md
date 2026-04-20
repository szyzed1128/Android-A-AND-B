# Docker-native-Android迁移执行清单

> 本文档是迁移执行顺序清单。
> 这不是当前正式生产部署链路；当前正式链路仍是 `bootstrap_scheduler_host.sh + bootstrap_host.sh + bootstrap_slot1.sh + create_instance.sh` 这一套独立调度层 + Waydroid 多实例脚本体系。
> 目标是让 Claude Code 按固定阶段推进，避免跳步骤、混步骤、漏步骤。
> 总说明见：
> - `NewUI/yun/deployment/archive/docker_native_migration/Docker-native-Android迁移总说明.md`
> 回滚与验收见：
> - `NewUI/yun/deployment/archive/docker_native_migration/Docker-native-Android迁移回滚与验收手册.md`

---

## 1. 执行总原则

1. 先验证运行时，再接业务层
2. 先单实例，再双实例，再接 Agent/Scheduler
3. 一次只改一个层级
4. 每一步都必须可回滚
5. 没通过本阶段验收，不得进入下一阶段

---

## 2. 阶段拆分

### 阶段 1：冻结边界

目标：

- 不让迁移范围失控

必须确认：

- 不改 B 端 UI
- 不改 WebSocket 业务协议
- 不重写 Scheduler 核心状态机
- 只替换实例层 runtime

输出物：

- 迁移边界确认
- 文档路径确认
- 回滚规则确认

---

### 阶段 2：验证单 Docker-native Android 实例

目标：

- 先证明单实例可运行、可调试、可跑 APK

任务清单：

1. 宿主机内核能力检查
   - binder/binderfs
   - ashmem 或等价能力
   - Docker 可运行
2. 拉起 1 个 Docker-native Android 容器
3. 验证 ADB 连通
4. 安装 APK
5. 启动 APK
6. 验证业务探针
   - `getBrands`
   - `getProfiles`
   - `applyProfile`
7. 验证 APK WebSocket
   - `ws://127.0.0.1:<probePort>/ws`

必须产出：

- 单实例启动脚本
- 单实例状态检查命令
- 单实例回滚命令

通过标准：

- `boot_completed=1`
- `adb devices` 能看到实例
- APK 可安装、可启动
- `getBrands` 成功
- WebSocket 可达

---

### 阶段 3：验证真双实例

目标：

- 证明同一 EC2 上 2 个 Android 容器真正彼此独立

任务清单：

1. 启动第二个 Docker-native Android 容器
2. 为两个容器分配不同 ADB 端口
3. 为两个容器分配不同 WebSocket 端口
4. 为两个容器分配不同数据卷
5. 在两个容器中分别安装 APK
6. 对两个容器分别执行：
   - `pm clear`
   - `am start`
   - `getBrands`
7. 做文件系统隔离验证
8. 做 APK WebSocket 独立验证
9. 做单实例重置不影响另一实例的验证

必须产出：

- 双实例启动脚本
- 双实例状态检查脚本
- 双实例清理脚本

通过标准：

- `adb devices` 同时看到两个实例
- 两个实例 `/data` 写入互不影响
- 两个实例 WebSocket 都独立响应
- 一个实例重启/清数据不影响另一个

---

### 阶段 4：抽象 runtime manager

目标：

- 把“手工 docker run”固化成可维护的实例编排层

任务清单：

1. 定义实例配置模板
2. 创建 runtime manager 目录结构
3. 实现脚本或服务：
   - create
   - start
   - stop
   - restart
   - reset
   - remove
   - inspect
4. 支持为 Agent 生成实例清单
5. 输出固定配置文件：
   - `/opt/cardemo/agent/.env` 中的 `INSTANCES_CONFIG`

建议产物：

- `docker-android-manager/`
- `config/templates/`
- `bin/create_instance.sh`
- `bin/reset_instance.sh`

通过标准：

- 运行时不依赖人工手敲长命令
- 能按实例 ID 进行生命周期管理
- 能稳定输出 Agent 所需配置

---

### 阶段 5：接回 Agent

目标：

- 让 Agent 在不推翻现有协议的前提下管理 Docker 实例

任务清单：

1. 修改 Agent 配置模型
2. 改成从生成文件读取实例列表
3. 适配新的：
   - `adbTarget`
   - `probePort`
   - `wsPort`
   - `containerName`
4. 跑通 readiness probe
5. 上报 Scheduler

关键文件：

- `NewUI/yun/agent/src/config.ts`
- `NewUI/yun/agent/src/services/instanceManager.ts`
- `NewUI/yun/agent/src/services/apkProbe.ts`
- `NewUI/yun/agent/src/services/healthReporter.ts`

通过标准：

- Agent 启动成功
- Agent 能识别两个 Docker 实例
- 两个实例状态能正确从 `probing -> idle`
- Scheduler 能收到健康上报

---

### 阶段 6：接回 Scheduler

目标：

- 让现有调度层重新对接新实例层

任务清单：

1. 验证 Scheduler 的实例池统计
2. 验证 reserve/release
3. 验证 session 分配
4. 验证心跳超时回收
5. 验证同一台服务器下多个实例同时存在

关键文件：

- `NewUI/yun/scheduler/src/services/scheduler.ts`
- `NewUI/yun/scheduler/src/services/agentClient.ts`

通过标准：

- `/instance/pool/stats` 数量正确
- 两个实例都可分配
- 会话释放后实例回到 `idle`
- 一个实例 `bad` 不影响另一个

---

### 阶段 7：更新 Nginx 与端口策略

目标：

- 让外部访问与内部探针都使用新的端口规划

任务清单：

1. 定义端口表
2. 更新 Nginx 反代
3. 更新本地/线上健康检查命令
4. 校验 B 端通过调度层拿到的 wsUrl

建议初始端口：

- `8081 -> inst_01 ws`
- `8082 -> inst_02 ws`
- `18081 -> inst_01 probe`
- `18082 -> inst_02 probe`

通过标准：

- 端口不冲突
- Nginx 配置通过
- `ws://<host>:8081/ws` 与 `ws://<host>:8082/ws` 指向不同实例

---

### 阶段 8：文档与运维收口

目标：

- 让 Docker 路线可交接、可维护

任务清单：

1. 更新实例层部署文档
2. 更新调度层对接文档
3. 更新运维脚本手册
4. 标记旧 Waydroid 文档废弃
5. 写清楚故障排查和回滚

必须覆盖：

- 实例创建
- 实例删除
- 数据重置
- APK 安装
- ADB 调试
- WebSocket 检查
- Agent/Scheduler 检查

---

## 3. Claude Code 执行顺序约束

Claude Code 必须严格按以下顺序执行：

1. 单实例运行时验证
2. 真双实例隔离验证
3. runtime manager 初版
4. Agent 配置改造
5. Agent 探针改造
6. Scheduler 联调
7. Nginx/端口改造
8. 文档与运维收口

禁止顺序：

- 先改 Agent，再验证 Docker 单实例
- 先改 Scheduler，再验证真双实例
- 先改 B 端，再验证 runtime
- 一次性同时改 runtime + agent + scheduler + nginx

---

## 4. 每个阶段必须交付的内容

每个阶段结束都必须提供：

- 变更文件清单
- 服务器改动清单
- 验证命令
- 验证结果
- 回滚步骤
- 当前阻塞项

---

## 5. 阶段完成定义

只有满足以下条件，才允许进入下一阶段：

- 本阶段所有关键验证通过
- 本阶段回滚方案实际可执行
- 没有遗留“手工记忆型步骤”
- 所有新参数都已文档化
