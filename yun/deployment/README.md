# deployment 目录说明

当前正式部署链路只包含以下内容：

- `总体架构说明文档.md`
- `调度层大规模部署说明文档.md`
- `实例层大规模部署说明文档.md`
- `Waydroid多实例隔离与并发稳定化避坑文档.md`
- `scripts/bootstrap_scheduler_host.sh`
- `scripts/bootstrap_host.sh`
- `scripts/bootstrap_slot1.sh`
- `scripts/create_instance.sh`
- `scripts/start_instance.sh`
- `scripts/stop_instance.sh`
- `scripts/reset_instance.sh`
- `scripts/status_instance.sh`
- `scripts/install_persist.sh`
- `scripts/lib_instance_identity.sh`
- `scripts/lib_runtime_common.sh`
- `scripts/run_weston_slot.sh`

配套自动化 skill（仓库外安装态）：

- `~/.codex/skills/obd-deployment/SKILL.md`

读取顺序建议：

1. `总体架构说明文档.md`
2. `调度层大规模部署说明文档.md`
3. `实例层大规模部署说明文档.md`
4. `Waydroid多实例隔离与并发稳定化避坑文档.md`
5. `scripts/`

明确边界：

- 当前正式生产链路是 **独立调度层 EC2 + 实例层 EC2**
- 实例层机器 **不应再部署本机 scheduler**
- 实例层机器的公网地址默认由 Agent 自动探测；`PUBLIC_WS_HOST` 只作为显式覆盖项
- `deployment/archive/` 下均为历史 PoC、迁移草案、审计材料或旧运行时残留
- 自动化 agent / skill 不应把 `deployment/archive/` 当作正式入口
- 自动化 agent / skill 应优先使用 `~/.codex/skills/obd-deployment`，并只读取本目录下的正式文档与脚本
