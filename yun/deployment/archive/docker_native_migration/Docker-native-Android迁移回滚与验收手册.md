# Docker-native-Android迁移回滚与验收手册

> 本文档定义 Docker-native Android 迁移中的回滚规范与验收规范。
> 这是执行迁移时必须同时维护的配套文档。
> 相关文档：
> - `NewUI/yun/deployment/archive/docker_native_migration/Docker-native-Android迁移总说明.md`
> - `NewUI/yun/deployment/archive/docker_native_migration/Docker-native-Android迁移执行清单.md`

---

## 1. 回滚总原则

1. 每次修改前必须先备份
2. 每次修改后必须有验证命令
3. 每一步修改必须能独立撤销
4. 不允许在未验证前叠加第二个高风险修改
5. 任何时候都必须保留回到“当前单实例稳定状态”的能力

---

## 2. 原子变更粒度

建议按以下粒度推进，不得合并成一个大改动：

1. 单实例 Docker 验证脚本
2. 真双实例 Docker 验证脚本
3. runtime manager 初版
4. Agent 配置改造
5. Agent 探针改造
6. Nginx/端口改造
7. 文档更新

每一步都必须满足：

- 改动范围清晰
- 备份对象清晰
- 回滚动作清晰

---

## 3. 回滚优先级

迁移过程中如失败，必须按以下顺序回滚：

1. 先停新容器
2. 再恢复旧端口
3. 再恢复 Agent 配置
4. 再恢复旧脚本
5. 最后清理新 volume / image / network

禁止顺序：

- 先删 volume，再导出日志
- 先恢复代码，再保留错误运行态
- 先改动 Waydroid 旧环境，再确认 Docker 新环境已失败

---

## 4. 代码层回滚要求

### 4.1 每次代码修改前

必须记录：

- 改动文件列表
- 旧配置备份位置
- 旧脚本备份位置
- 当前 Git 工作区状态

### 4.2 每次代码修改后

必须记录：

- 当前变更文件清单
- 新增文件清单
- 可逆操作说明
- 验证命令

### 4.3 建议执行方式

- 一次只修改一个主题
- 不跨主题混改
- 不把“代码改动”和“服务器危险动作”捆绑到同一步

---

## 5. 服务器层回滚要求

### 5.1 任何配置改动前必须备份

包括但不限于：

- systemd unit
- nginx 配置
- agent `.env`
- 实例清单 JSON
- docker compose 或实例脚本

备份要求：

- 带时间戳
- 原路径旁边可追溯
- 能直接恢复

### 5.2 任何新增对象都必须可单独删除

包括但不限于：

- Docker 容器
- Docker volume
- Docker network
- systemd service
- 新目录
- 新生成配置文件

---

## 6. Docker 迁移的验收标准

### 6.1 单实例验收

必须同时满足：

- Docker Android 容器成功启动
- `adb devices` 可见该实例
- `getprop sys.boot_completed = 1`
- APK 安装成功
- APK 启动成功
- `getBrands` 成功
- `ws://127.0.0.1:<probePort>/ws` 可连

### 6.2 真双实例验收

必须同时满足：

- `adb devices` 可见两个不同 ADB 入口
- 两个实例使用不同数据卷
- 两个实例能分别安装并启动 APK
- 两个实例文件系统写入互不影响
- 两个实例 WebSocket 独立响应
- 一个实例重置不影响另一个

### 6.3 Agent 联调验收

必须同时满足：

- Agent 能读取 Docker 实例清单
- Agent 能识别两个实例
- 两个实例状态可从 `probing -> idle`
- Agent 健康上报成功

### 6.4 Scheduler 联调验收

必须同时满足：

- Scheduler 能看到两个实例
- 实例池数量正确
- reserve/release 正常
- 心跳回收正常
- 一个实例异常不污染另一个实例

---

## 7. 建议的验收命令清单

以下命令类别必须出现在每个阶段的交付结果里：

- 运行时状态
  - `docker ps`
  - `docker inspect`
  - `docker logs`
- ADB 状态
  - `adb devices`
  - `adb -s <target> shell getprop sys.boot_completed`
- APK 行为
  - `pm list packages`
  - `am start`
  - readiness probe
- WebSocket
  - 探针连接 `/ws`
  - 协议方法调用结果
- Agent/Scheduler
  - `/health`
  - `/instance/pool/stats`
  - 日志检查

---

## 8. 失败场景与处理原则

### 场景 A：单实例 Docker Android 无法启动

处理原则：

- 停止继续迁移
- 不改 Agent/Scheduler
- 先定位 runtime/内核兼容问题

### 场景 B：单实例可运行，但 APK 不稳定

处理原则：

- 不进入双实例阶段
- 先确认 APK 与 Docker-native Android runtime 的兼容性

### 场景 C：双实例能起来，但文件系统或 WebSocket 串线

处理原则：

- 不进入 Agent 接回阶段
- 优先查 volume、端口映射、实例配置生成逻辑

### 场景 D：Agent 接回后状态错误

处理原则：

- 保留 Docker 双实例验证环境
- 回滚 Agent 改动
- 不动 Scheduler 核心状态机

### 场景 E：Scheduler 接回后分配异常

处理原则：

- 回滚 Scheduler 联调改动
- 保留 Agent 和 Docker 运行时
- 分层定位问题

---

## 9. Claude Code 执行口径

下面内容可直接喂给 Claude Code：

---

你要把当前项目的实例运行时从 Waydroid 迁移到 Docker-native Android，但必须遵守以下硬约束：

1. 不要先改 B 端 UI 和业务协议
2. 不要重写 Scheduler 核心分配逻辑
3. 优先复用现有 Agent/Scheduler 接口
4. 只把实例层 runtime 从 Waydroid 替换为 Docker-native Android
5. 任何一步修改都必须先给出：
   - 修改范围
   - 备份方式
   - 验证方法
   - 回滚方案
6. 必须先完成：
   - 单实例 Docker Android 验证
   - 真双实例隔离验证
   再接回 Agent 和 Scheduler
7. 迁移时按三类处理：
   - 几乎不动：B端、Scheduler 核心、WebSocket 协议
   - 需要调整：Agent 配置、探针、健康上报、Nginx 端口、运维手册
   - 需要重做：实例生命周期管理、宿主机脚本、运行时部署文档、Waydroid/LXC 相关方案
8. 最终验收必须包含：
   - 两个实例独立 ADB
   - 两个实例独立数据卷
   - 两个实例独立 APK WebSocket
   - Agent 能上报两个实例
   - Scheduler 能分配两个实例
9. 任何时候都必须保留回到当前代码状态和当前单实例运行状态的能力

---

## 10. 最终回滚目标

无论迁移进行到哪一步，都必须能恢复到以下状态：

- 当前代码状态可恢复
- 当前单实例运行链路可恢复
- 当前 Agent/Scheduler 单实例联调可恢复
- 当前 Nginx 端口策略可恢复
- 旧 Waydroid 方案至少还能用于紧急回退
