# Docker-native-Android迁移总说明

> 本文档用于指导把当前项目的实例层运行时从 `Waydroid` 迁移到 `Docker-native Android`。
> 这不是当前正式生产部署链路；当前正式链路仍是 `bootstrap_scheduler_host.sh + bootstrap_host.sh + bootstrap_slot1.sh + create_instance.sh` 这一套独立调度层 + Waydroid 多实例脚本体系。
> 本文档只讨论迁移范围、模块边界、保留项与重做项，不直接给出逐步命令。
> 执行顺序见：
> - `NewUI/yun/deployment/archive/docker_native_migration/Docker-native-Android迁移执行清单.md`
> - `NewUI/yun/deployment/archive/docker_native_migration/Docker-native-Android迁移回滚与验收手册.md`

---

## 1. 迁移目标

把当前项目从：

- Waydroid 单实例 / 伪多实例运行时

迁移为：

- Docker-native Android 多容器运行时

迁移后保持以下目标不变：

- B 端 App 仍然通过调度层申请实例
- Agent 仍然向 Scheduler 上报实例健康状态
- 每个实例仍对外暴露 `ws://<host>:<port>/ws`
- APK 仍是 Android 容器内部的业务执行体
- Scheduler 仍按实例池分配、回收和保活
- 一台 EC2 能同时承载多个彼此独立的真实 Android 实例

---

## 2. 非目标

本次迁移不做以下事情：

- 不重写 B 端 UI
- 不重写 WebSocket 业务协议
- 不重写 Scheduler 核心状态机
- 不同步优化所有历史文档
- 不兼容保留 Waydroid 与 Docker 双运行时长期并行

---

## 3. 迁移范围总览

| 模块 | 几乎不动 | 需要调整 | 需要重做 |
|---|---|---|---|
| B端 UI/业务流程 | ✅ |  |  |
| B端 Scheduler 调用链 | ✅ | ⚠️ 少量配置/容错 |  |
| WebSocket 业务协议 | ✅ |  |  |
| Scheduler 核心分配逻辑 | ✅ | ⚠️ 实例字段来源/部署说明 |  |
| Scheduler API 路由 | ✅ |  |  |
| Agent 上报协议 | ✅ | ⚠️ 字段来源与实例发现方式 |  |
| Agent 探针逻辑 |  | ⚠️ 需要适配 Docker 容器启动/ADB/端口 |  |
| Agent 实例配置模型 |  | ⚠️ 需要从 Waydroid 槽位模型改成 Docker 容器模型 |  |
| 实例生命周期管理 |  |  | ❌ 必须重做 |
| 宿主机部署脚本 |  |  | ❌ 必须重做 |
| Waydroid/LXC 相关脚本与文档 |  |  | ❌ 必须废弃/重写 |
| ADB 暴露方式 |  | ⚠️ 需要重构 |  |
| APK 安装/重置/清数据流程 |  | ⚠️ 需要改成容器内执行模型 |  |
| Nginx 端口/反向代理 |  | ⚠️ 需要按 Docker 实例重新映射 |  |
| 监控/运维手册 |  | ⚠️ 需要更新命令与故障判断 |  |
| 调度层 Redis/会话模型 | ✅ |  |  |
| Dashboard | ✅ | ⚠️ 如展示运行时细节则要加字段 |  |
| Docker / 镜像 / volume / network 体系 |  |  | ❌ 必须新建 |

---

## 4. 几乎不动的部分

### 4.1 B 端 App 的用户流程

保留原因：

- 当前 B 端已经围绕“调度层先分配，再连接实例 WebSocket”的流程实现
- 这个流程与底层 runtime 是 Waydroid 还是 Docker 无直接关系

可直接保留的行为：

- 用户输入 `cloudHost/cloudPort`
- App 自动推导 Scheduler API 地址
- App 初始化 session
- 选择品牌/车型
- 申请实例
- 连接实例 WebSocket
- 发送 OBD 指令、读取 ECU/DTC/冻结帧/PID

关键文件：

- `NewUI/yun/client-app/src/services/SchedulerClient.ts:5`
- `NewUI/yun/client-app/src/services/SchedulerClient.ts:6`
- `NewUI/yun/client-app/src/services/SchedulerClient.ts:7`

保留原则：

- 不改 UI 流程
- 不改 API 调用顺序
- 不改 B 端页面组织
- 不改 CloudBridge/WebSocket 上层业务调用方式

### 4.2 WebSocket 业务协议

可直接保留：

- `getBrands`
- `getProfiles`
- `applyProfile`
- `connectOBD`
- `readECUInfo`
- `readDTC`
- `readFreezeFrame`
- `startReadPIDs`
- `stopReadPIDs`

关键文件：

- `NewUI/yun/agent/src/services/apkProbe.ts:36`

保留原则：

- 不改协议名
- 不改请求/响应格式
- 不改 APK 内业务协议

### 4.3 Scheduler 核心分配逻辑

可直接保留的逻辑：

- `reserveInstance`
- `releaseInstance`
- `disconnectSession`
- `heartbeat monitor`
- `idle / reserved / busy / bad / reserved_for_user`

关键文件：

- `NewUI/yun/scheduler/src/services/scheduler.ts:25`
- `NewUI/yun/scheduler/src/services/scheduler.ts:189`
- `NewUI/yun/scheduler/src/services/scheduler.ts:277`
- `NewUI/yun/scheduler/src/services/scheduler.ts:308`

保留原则：

- 不重写 Redis 状态机
- 不重写 session 生命周期
- 不重写调度 API 路由
- 只调整实例来源与部署说明

### 4.4 Scheduler API 与 Redis/Dashboard 模型

可以保持不变或只做兼容扩展：

- `/health`
- `/session/init`
- `/session/:id/reserve`
- `/session/:id/heartbeat`
- `/session/:id/release`
- `/instance/pool/stats`
- `/catalog/brands`
- `/catalog/profiles`
- `InstanceInfo`
- `SessionInfo`

关键文件：

- `NewUI/yun/scheduler/src/types/index.ts:17`
- `NewUI/yun/scheduler/src/services/dashboardService.ts`
- `NewUI/yun/dashboard-web/app.js`

---

## 5. 需要调整的部分

### 5.1 Agent 配置模型

当前问题：

- `InstanceConfig` 明显围绕 Waydroid 设计
- 默认假设通过 `adbTarget + adb forward` 访问 Android Worker

关键文件：

- `NewUI/yun/agent/src/config.ts:6`
- `NewUI/yun/agent/src/config.ts:38`

迁移后建议字段：

```ts
interface InstanceConfig {
  id: string;
  containerName: string;
  runtimeType: 'docker-android';
  adbTarget: string;
  wsPort: number;
  probePort: number;
  packageName: string;
  activityName: string;
  host: string;
  dockerImage?: string;
  dockerDataVolume?: string;
}
```

调整原则：

- 保留 `id/wsPort/probePort/packageName/activityName`
- 新增 Docker 运行时字段
- 不再在配置里出现 `waydroid` 概念

### 5.2 Agent 实例发现方式

当前问题：

- 主要依赖 `.env` 中的 `INSTANCES_CONFIG`
- 不适合容器动态创建和重建

迁移后建议：

- 优先仍由部署层统一生成 `INSTANCES_CONFIG`
  - 落点仍写回 `/opt/cardemo/agent/.env`
- 可选再升级为 runtime manager HTTP 输出

### 5.3 Agent 健康上报

当前逻辑可以保留，调整字段来源：

- `serverId`
- `serverIp`
- `publicWsHost`
- `agentPort`
- `instances[]`

关键文件：

- `NewUI/yun/agent/src/services/healthReporter.ts:11`
- `NewUI/yun/agent/src/services/healthReporter.ts:27`

需要调整：

- `instances[]` 来自 Docker 容器元数据
- `publicWsHost` 生产环境建议强制配置，不再依赖自动探测
- 可选补充：
  - `runtimeType`
  - `containerName`
  - `containerStatus`
  - `imageTag`

### 5.4 Agent 探针逻辑

当前逻辑：

- `adb connect <adbTarget>`
- `adb forward tcp:<probePort> tcp:8080`
- `pm clear`
- `am start`
- `getBrands`
- `getProfiles`
- `applyProfile`

关键文件：

- `NewUI/yun/agent/src/services/apkProbe.ts:39`
- `NewUI/yun/agent/src/services/apkProbe.ts:48`
- `NewUI/yun/agent/src/services/apkProbe.ts:62`

迁移后保留：

- `pm clear`
- `am start`
- `getBrands/getProfiles/applyProfile`
- readiness 判断标准

迁移后调整：

- `adb connect` 的目标端口来源
- `adb forward` 的建立方式
- 容器重启后端口重建
- 容器级重置与应用级重置的先后顺序

### 5.5 Nginx 端口策略

当前问题：

- 大量脚本默认单实例 `8080`
- 默认 `18080 -> 8080`

关键文件：

- `NewUI/fuwuqi/03_端口与代理配置.md:103`
- `NewUI/fuwuqi/scripts/server_start.sh:95`
- `NewUI/fuwuqi/08_调度系统部署.md:224`

迁移后建议：

- 每实例一个 `hostWsPort`
- 初期不要做统一 `/ws` 多路复用
- 先明确：
  - `8081 -> inst_01`
  - `8082 -> inst_02`
  - `18081 -> probe inst_01`
  - `18082 -> probe inst_02`

### 5.6 运维与监控命令

需要从：

- `waydroid status`
- `lxc-info`
- `lxc-unfreeze`

迁移到：

- `docker ps`
- `docker inspect`
- `docker logs`
- `docker exec`
- `docker restart`

需要更新的文档/脚本：

- `NewUI/fuwuqi/05_运维脚本手册.md`
- `NewUI/fuwuqi/08_调度系统部署.md`
- `NewUI/fuwuqi/scripts/server_status.sh`

---

## 6. 需要重做的部分

### 6.1 实例运行时管理层

当前状态：

- 项目没有独立的 runtime manager
- 当前“实例管理”本质上是 Agent 对固定配置做探针

迁移后必须新增：

- `docker-android-manager`
  - create instance
  - remove instance
  - restart instance
  - reset instance data
  - inspect instance
  - generate agent config

### 6.2 宿主机部署脚本

当前脚本：

- `NewUI/fuwuqi/scripts/server_setup_waydroid.sh`
- `NewUI/fuwuqi/scripts/server_start.sh`
- `NewUI/fuwuqi/scripts/server_restart_apk.sh`

迁移后建议替换为：

- `server_setup_docker_android.sh`
- `docker_instance_create.sh`
- `docker_instance_start.sh`
- `docker_instance_reset.sh`
- `docker_instance_status.sh`
- `docker_instance_remove.sh`

### 6.3 实例层部署文档

当前问题：

- “唯一 `adbTarget/probePort/wsPort`” 被当成多实例成立条件
- 但未保证底层容器独立

需要重写内容：

- Docker 镜像选择
- 容器实例目录 / volume 规划
- ADB 暴露端口规则
- 每实例数据卷规则
- 端口分配规则
- 容器重建和回滚
- 多实例验收标准

### 6.4 Waydroid/LXC 相关运维体系

需要整体废弃或标记废弃：

- `waydroid status`
- `lxc-info -n waydroid`
- `lxc-unfreeze`
- `/var/lib/waydroid`
- `waydroid0`
- `waydroid session start`（历史 Waydroid 命令，迁移后不再使用）

### 6.5 实例初始化与重置策略

当前策略过度依赖 Android 内部命令：

- `pm clear`
- `am start`
- 探针循环

迁移后建议分层重置：

- 轻重置：
  - `pm clear`
  - 进程重启
  - readiness probe
- 中重置：
  - `docker restart <container>`
- 重重置：
  - 删除实例数据卷并重建容器

---

## 7. Claude Code 执行原则

1. 不先改 B 端 UI 和业务协议
2. 不重写 Scheduler 核心状态机
3. 优先复用现有 Agent/Scheduler 接口
4. 只替换实例层 runtime
5. 必须先完成：
   - 单实例 Docker Android 验证
   - 真双实例隔离验证
   再接回 Agent 和 Scheduler
6. 任何一步修改前必须先给出：
   - 修改范围
   - 备份方式
   - 验证方法
   - 回滚方案

---

## 8. 结论

本次迁移不是推倒重做，也不是小修小补。

它的本质是：

- 保留业务层和调度层
- 替换实例层 runtime
- 重建宿主机运维与实例生命周期管理

短期复杂度主要集中在：

- 实例运行时
- 运维脚本
- Agent 配置和探针

不是集中在：

- B 端
- Scheduler 核心
- WebSocket 业务协议
