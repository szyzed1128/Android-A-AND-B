#!/usr/bin/env bash
# =============================================================================
# bootstrap_scheduler_host.sh  —  独立调度层宿主机基线安装
# =============================================================================
# 用法：
#   ./bootstrap_scheduler_host.sh
#
# 目标：
#   - 在独立调度层 EC2 上部署 Redis + Scheduler + dashboard-web + Nginx:8080
#   - 产出给实例层使用的固定入口：http://<scheduler-host>:8080/api
#   - 自动放行调度层公网入口 tcp/8080（ufw / AWS Security Group）
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${SCRIPT_DIR}/lib_instance_identity.sh"

require_root

YUN_DIR="$(cd "${SCRIPT_DIR}/../.." && pwd)"
SCHEDULER_SRC="${YUN_DIR}/scheduler"
DASHBOARD_SRC="${YUN_DIR}/dashboard-web"

DEPLOY_ROOT="/opt/cardemo"
DEPLOY_SCRIPTS_DIR="${DEPLOY_ROOT}/scripts"

log_step "bootstrap_scheduler_host: 独立调度层宿主机安装"
log_info "Ubuntu: $(lsb_release -rs 2>/dev/null || echo unknown)  arch=$(uname -m)"

install_optional_awscli() {
  if command -v aws >/dev/null 2>&1; then
    log_info "aws CLI 已存在: $(aws --version 2>&1 | head -1)"
    return 0
  fi

  if DEBIAN_FRONTEND=noninteractive apt-get install -y -qq awscli >/dev/null 2>&1; then
    log_info "aws CLI 安装完成: $(aws --version 2>&1 | head -1)"
    return 0
  fi

  log_warn "awscli 安装失败或当前仓库无候选包；仅在 AWS 自动配置 Security Group 时需要，当前继续执行"
}

log_step "步骤1: 安装系统依赖包"
apt-get update -qq
DEBIAN_FRONTEND=noninteractive apt-get install -y -qq \
  curl ca-certificates git jq \
  nginx \
  redis-server \
  nodejs \
  python3 \
  lsb-release gnupg
install_optional_awscli

if ! node --version 2>/dev/null | grep -qE '^v(2[0-9]|[3-9][0-9])'; then
  log_step "步骤2: 安装 Node.js >= 20"
  curl -fsSL https://deb.nodesource.com/setup_20.x | bash - >/dev/null 2>&1
  apt-get install -y -qq nodejs
fi
log_info "Node.js 已就绪: $(node --version)"

log_step "步骤3: 创建部署目录"
mkdir -p "${DEPLOY_ROOT}" "${DEPLOY_SCRIPTS_DIR}"
mkdir -p /var/log/obd-scheduler

log_step "步骤4: 构建并部署 scheduler + dashboard-web"
[[ -d "${SCHEDULER_SRC}" ]] || die "scheduler 源码不存在: ${SCHEDULER_SRC}"
[[ -d "${DASHBOARD_SRC}" ]] || die "dashboard-web 目录不存在: ${DASHBOARD_SRC}"

cd "${SCHEDULER_SRC}"
npm ci --quiet 2>/dev/null || npm install --quiet 2>/dev/null
npm run build >/dev/null

mkdir -p "${DEPLOY_ROOT}/scheduler"
rm -rf "${DEPLOY_ROOT}/scheduler/dist" "${DEPLOY_ROOT}/dashboard-web"
cp -R dist "${DEPLOY_ROOT}/scheduler/"
cp package.json "${DEPLOY_ROOT}/scheduler/"
cp package-lock.json "${DEPLOY_ROOT}/scheduler/" 2>/dev/null || true
cp -R "${DASHBOARD_SRC}" "${DEPLOY_ROOT}/dashboard-web"
(
  cd "${DEPLOY_ROOT}/scheduler"
  npm ci --production --quiet >/dev/null 2>&1 || npm install --production --quiet >/dev/null 2>&1
)

for file in "${SCRIPT_DIR}"/*.sh; do
  cp -f "${file}" "${DEPLOY_SCRIPTS_DIR}/"
done
chmod +x "${DEPLOY_SCRIPTS_DIR}"/*.sh
log_info "调度层应用与脚本已部署到 ${DEPLOY_ROOT}"

log_step "步骤5: 写环境文件"
cat > "${DEPLOY_ROOT}/scheduler/.env" <<'EOF'
PORT=3000
REDIS_URL=redis://127.0.0.1:6379
EOF

log_step "步骤6: 配置 systemd 服务"
cat > /etc/systemd/system/obd-scheduler.service <<'UNIT'
[Unit]
Description=OBD Scheduler Service
After=network.target redis-server.service
Requires=redis-server.service

[Service]
Type=simple
WorkingDirectory=/opt/cardemo/scheduler
EnvironmentFile=/opt/cardemo/scheduler/.env
ExecStart=/usr/bin/node dist/index.js
Restart=always
RestartSec=5
StandardOutput=append:/var/log/obd-scheduler/scheduler.log
StandardError=append:/var/log/obd-scheduler/scheduler.err.log

[Install]
WantedBy=multi-user.target
UNIT

systemctl daemon-reload
systemctl enable --now redis-server >/dev/null 2>&1 || die "redis-server 启动失败"
systemctl enable --now obd-scheduler >/dev/null 2>&1 || die "obd-scheduler 启动失败"

log_step "步骤7: 配置 Nginx 调度入口"
cat > /etc/nginx/conf.d/obd-scheduler.conf <<'NGINX'
server {
    listen 8080;
    server_name _;

    location /api/ {
        proxy_pass http://127.0.0.1:3000/;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_read_timeout 60s;
    }

    location /dashboard/ {
        proxy_pass http://127.0.0.1:3000/dashboard/;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
    }

    location = /health {
        proxy_pass http://127.0.0.1:3000/health;
    }
}
NGINX

systemctl enable --now nginx >/dev/null 2>&1 || die "nginx 启动失败"
nginx -t || die "Nginx 配置错误"
nginx -s reload >/dev/null 2>&1 || systemctl restart nginx || die "nginx reload 失败"
ensure_public_tcp_port_open 8080 "调度层 API / Dashboard 入口"

log_step "验收: 独立调度层"
ERRORS=0
systemctl is-active redis-server >/dev/null 2>&1 && log_info "✓ redis-server" || { log_error "✗ redis-server 未运行"; ERRORS=$((ERRORS + 1)); }
systemctl is-active obd-scheduler >/dev/null 2>&1 && log_info "✓ obd-scheduler" || { log_error "✗ obd-scheduler 未运行"; ERRORS=$((ERRORS + 1)); }
systemctl is-active nginx >/dev/null 2>&1 && log_info "✓ nginx" || { log_error "✗ nginx 未运行"; ERRORS=$((ERRORS + 1)); }
[[ -d "${DEPLOY_ROOT}/dashboard-web" ]] && log_info "✓ dashboard-web" || { log_error "✗ dashboard-web 未部署"; ERRORS=$((ERRORS + 1)); }
curl -fsS --max-time 3 http://127.0.0.1:3000/health >/dev/null && log_info "✓ Scheduler Node" || { log_error "✗ Scheduler Node 不可用"; ERRORS=$((ERRORS + 1)); }
curl -fsS --max-time 3 http://127.0.0.1:8080/health >/dev/null && log_info "✓ Scheduler Nginx 入口" || { log_error "✗ Scheduler Nginx 入口不可用"; ERRORS=$((ERRORS + 1)); }

[[ ${ERRORS} -eq 0 ]] || die "调度层宿主机验收失败（${ERRORS} 项）"

echo ""
echo "═══════════════════════════════════════════════════════════"
echo "  bootstrap_scheduler_host 完成 ✓"
echo "  实例层应把 SCHEDULER_URL 指向："
echo "    http://<scheduler-host>:8080/api"
echo "═══════════════════════════════════════════════════════════"
