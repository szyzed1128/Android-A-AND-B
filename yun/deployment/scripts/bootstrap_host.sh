#!/usr/bin/env bash
# =============================================================================
# bootstrap_host.sh  —  新 EC2 实例层宿主机基线安装
# =============================================================================
# 用法：
#   export SERVER_ID=ec2-apne1-a01
#   export PUBLIC_WS_HOST=1.2.3.4
#   export SCHEDULER_URL=http://<scheduler-host>:8080/api
#   export SERVER_IP_OVERRIDE=10.0.1.23   # 可选，强制 Agent 上报给 Scheduler 的控制面地址
#   export PUBLIC_WS_SCHEME=ws   # 可选，默认 ws
#   ./bootstrap_host.sh
#
# 目标：把一台全新的 Ubuntu 22.04 LTS (ARM64) 变成实例层宿主机。
#
# 执行后保证：
#   - Waydroid / ADB / Weston / LXC / Nginx / Node.js 已安装
#   - ip / iptables / ss / nc / nsenter 等实例脚本依赖已安装
#   - python3-dbus / python3-websockets 已安装
#   - binder 模块开机自动加载，binderfs 开机自动挂载
#   - Waydroid 关键补丁已打并校验成功
#   - Agent / deployment scripts 已部署到 /opt/cardemo
#   - Agent systemd 已启动
#   - Agent 已显式指向远端独立调度层
#
# 不覆盖：
#   - 独立调度层部署（由 bootstrap_scheduler_host.sh 负责）
#   - waydroid init（由 bootstrap_slot1.sh 负责）
#   - slot_1 运行时（由 bootstrap_slot1.sh 负责）
#   - slot_2+ 实例创建（由 create_instance.sh 负责）
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${SCRIPT_DIR}/lib_instance_identity.sh"

require_root
_require_env SERVER_ID
_require_env PUBLIC_WS_HOST
_require_env SCHEDULER_URL
: "${PUBLIC_WS_SCHEME:=ws}"
: "${SERVER_IP_OVERRIDE:=}"

derive_identity 1

YUN_DIR="$(cd "${SCRIPT_DIR}/../.." && pwd)"
AGENT_SRC="${YUN_DIR}/agent"
DEPLOY_ROOT="/opt/cardemo"
DEPLOY_SCRIPTS_DIR="${DEPLOY_ROOT}/scripts"
APK_DEST="${APK_PATH}"

SLOT1_INST_ID="${INST_ID}"
SLOT1_WS_PORT="${WS_PORT}"
SLOT1_PROBE_PORT="${PROBE_PORT}"
SLOT1_ADB_TARGET="${ADB_TARGET}"

validate_scheduler_url() {
  [[ "${SCHEDULER_URL}" =~ /api/?$ ]] || die "SCHEDULER_URL 必须指向调度层 /api 入口，例如 http://scheduler-host:8080/api"
  [[ ! "${SCHEDULER_URL}" =~ ^https?://(127\.0\.0\.1|localhost)(:[0-9]+)?/ ]] || die "SCHEDULER_URL 不能指向本机回环地址，必须是独立调度层地址"
}

resolve_apk_src_path() {
  if [[ -n "${APK_SRC_PATH:-}" ]]; then
    [[ -f "${APK_SRC_PATH}" ]] && echo "${APK_SRC_PATH}" && return 0
    return 1
  fi

  local candidates=(
    "${YUN_DIR}/../Release_com.companyname.cardemo-Server-Signed.apk"
    "${YUN_DIR}/../build_output/Release_com.companyname.cardemo-Signed.apk"
  )
  local path
  for path in "${candidates[@]}"; do
    [[ -f "${path}" ]] && echo "${path}" && return 0
  done
  return 1
}

detect_binder_module() {
  if modinfo binder_linux >/dev/null 2>&1; then
    echo "binder_linux"
    return 0
  fi
  if modinfo binder >/dev/null 2>&1; then
    echo "binder"
    return 0
  fi
  die "未找到 binder 内核模块（binder_linux / binder），当前内核不满足 Waydroid 要求"
}

patch_hardware_manager() {
  local path="$1"
  [[ -f "$path" ]] || die "Waydroid 文件不存在: ${path}"
  python3 - "$path" <<'PYEOF'
import pathlib
import re
import sys

path = pathlib.Path(sys.argv[1])
text = path.read_text()
marker = 'headless mode: ignoring suspend request'
if marker in text:
    print("  hardware_manager.py 已有补丁，跳过")
    sys.exit(0)

pattern = r'^(?P<indent>[ \t]*)def suspend\(\):\n(?:(?P=indent)[ \t]+.*\n)+'
def replacement(match):
    indent = match.group("indent")
    return (
        f'{indent}def suspend():\n'
        f'{indent}    logging.debug("suspend() called - headless mode: ignoring suspend request")\n'
        f'{indent}    return\n'
    )
new_text, count = re.subn(pattern, replacement, text, count=1, flags=re.MULTILINE)
if count != 1:
    print("无法定位 hardware_manager.py 中的 suspend() 定义", file=sys.stderr)
    sys.exit(1)

path.write_text(new_text)
if marker not in new_text:
    print("hardware_manager.py 补丁校验失败", file=sys.stderr)
    sys.exit(1)
print("  hardware_manager.py suspend → no-op 补丁已打")
PYEOF
}

patch_lxc_py() {
  local path="$1"
  [[ -f "$path" ]] || die "Waydroid 文件不存在: ${path}"
  python3 - "$path" <<'PYEOF'
import pathlib
import sys

path = pathlib.Path(sys.argv[1])
text = path.read_text()
orig = text

if 'props.append("ro.adb.secure=0")' not in text:
    if 'props.append("ro.adb.secure=1")' not in text:
        print("未找到 ro.adb.secure 配置，无法自动打补丁", file=sys.stderr)
        sys.exit(1)
    text = text.replace('props.append("ro.adb.secure=1")', 'props.append("ro.adb.secure=0")')

if 'props.append("ro.debuggable=1")' not in text:
    if 'props.append("ro.debuggable=0")' in text:
        text = text.replace('props.append("ro.debuggable=0")', 'props.append("ro.debuggable=1")')
    else:
        needle = 'props.append("ro.adb.secure=0")'
        if needle not in text:
            print("未找到 adb.secure 行，无法插入 ro.debuggable=1", file=sys.stderr)
            sys.exit(1)
        text = text.replace(needle, needle + '\n    props.append("ro.debuggable=1")', 1)

if text != orig:
    path.write_text(text)

if 'props.append("ro.adb.secure=0")' not in text or 'props.append("ro.debuggable=1")' not in text:
    print("lxc.py 补丁校验失败", file=sys.stderr)
    sys.exit(1)

print("  lxc.py ro.adb.secure=0 + ro.debuggable=1 就绪")
PYEOF
}

merge_agent_env() {
  local env_path="$1"
  python3 - "$env_path" "$SERVER_ID" "$PUBLIC_WS_HOST" "$PUBLIC_WS_SCHEME" \
    "$SERVER_IP_OVERRIDE" "$SCHEDULER_URL" \
    "$SLOT1_INST_ID" "$SLOT1_WS_PORT" "$SLOT1_PROBE_PORT" "$SLOT1_ADB_TARGET" \
    "$APK_PACKAGE" "$APK_ACTIVITY" <<'PYEOF'
import json
import pathlib
import sys

env_path = pathlib.Path(sys.argv[1])
server_id = sys.argv[2]
public_ws_host = sys.argv[3]
public_ws_scheme = sys.argv[4]
server_ip_override = sys.argv[5]
scheduler_url = sys.argv[6]
slot1_id = sys.argv[7]
slot1_ws_port = int(sys.argv[8])
slot1_probe_port = int(sys.argv[9])
slot1_adb_target = sys.argv[10]
package_name = sys.argv[11]
activity_name = sys.argv[12]

existing_lines = env_path.read_text().splitlines() if env_path.exists() else []
env_map = {}
order = []
for line in existing_lines:
    if not line or line.lstrip().startswith("#") or "=" not in line:
        continue
    key, value = line.split("=", 1)
    env_map[key] = value
    if key not in order:
        order.append(key)

instances = []
if env_map.get("INSTANCES_CONFIG"):
    try:
        instances = json.loads(env_map["INSTANCES_CONFIG"])
    except Exception as exc:
        print(f"现有 INSTANCES_CONFIG 解析失败: {exc}", file=sys.stderr)
        sys.exit(1)

existing_slot1 = next((item for item in instances if item.get("id") == slot1_id), None)
slot1_identity = {
    "id": slot1_id,
    "wsPort": existing_slot1.get("wsPort", slot1_ws_port) if existing_slot1 else slot1_ws_port,
    "probePort": existing_slot1.get("probePort", slot1_probe_port) if existing_slot1 else slot1_probe_port,
    "adbTarget": existing_slot1.get("adbTarget", slot1_adb_target) if existing_slot1 else slot1_adb_target,
    "packageName": package_name,
    "activityName": activity_name,
}

updated = False
for index, item in enumerate(instances):
    if item.get("id") == slot1_id:
        instances[index] = {**item, **slot1_identity}
        updated = True
        break

if not updated:
    instances.insert(0, slot1_identity)

env_map["SERVER_ID"] = server_id
env_map["PUBLIC_WS_HOST"] = public_ws_host
env_map["PUBLIC_WS_SCHEME"] = public_ws_scheme
env_map["AGENT_PORT"] = "4000"
if server_ip_override:
    env_map["SERVER_IP_OVERRIDE"] = server_ip_override
else:
    env_map.pop("SERVER_IP_OVERRIDE", None)
env_map["SCHEDULER_URL"] = scheduler_url
env_map["INSTANCES_CONFIG"] = json.dumps(instances, separators=(",", ":"))

preferred = [
    "SERVER_ID",
    "PUBLIC_WS_HOST",
    "PUBLIC_WS_SCHEME",
    "AGENT_PORT",
    "SERVER_IP_OVERRIDE",
    "SCHEDULER_URL",
    "INSTANCES_CONFIG",
]
seen = set()
lines = []
for key in preferred + order + sorted(env_map.keys()):
    if key in env_map and key not in seen:
        lines.append(f"{key}={env_map[key]}")
        seen.add(key)

env_path.write_text("\n".join(lines) + "\n")
print(f"Agent 环境文件已更新: {env_path}")
PYEOF
}

log_step "bootstrap_host: 实例层宿主机基线安装"
validate_scheduler_url
APK_SRC_PATH="$(resolve_apk_src_path || true)"
log_info "SERVER_ID=${SERVER_ID}  PUBLIC_WS_HOST=${PUBLIC_WS_HOST}  PUBLIC_WS_SCHEME=${PUBLIC_WS_SCHEME}"
log_info "SCHEDULER_URL=${SCHEDULER_URL}"
log_info "Ubuntu: $(lsb_release -rs 2>/dev/null || echo unknown)  arch=$(uname -m)"

if [[ "$(uname -m)" != "aarch64" ]]; then
  die "当前脚本只支持 ARM64/aarch64 宿主机，当前架构为 $(uname -m)"
fi

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
  android-tools-adb \
  weston \
  lxc lxcfs \
  iproute2 iptables netcat-openbsd util-linux \
  nginx \
  socat \
  dnsmasq-base \
  python3 python3-pip python3-dbus python3-websockets \
  lsb-release gnupg
install_optional_awscli
log_info "系统包安装完成"

log_step "步骤2: 安装 Node.js >= 20"
if node --version 2>/dev/null | grep -qE '^v(2[0-9]|[3-9][0-9])'; then
  log_info "Node.js 已满足版本要求: $(node --version)"
else
  curl -fsSL https://deb.nodesource.com/setup_20.x | bash - >/dev/null 2>&1
  apt-get install -y -qq nodejs
  log_info "Node.js 安装完成: $(node --version)"
fi

log_step "步骤3: 安装 Waydroid"
if command -v waydroid >/dev/null 2>&1; then
  log_info "Waydroid 已安装: $(waydroid --version 2>/dev/null || echo unknown)"
else
  curl -fsSL https://repo.waydro.id | bash - >/dev/null 2>&1
  apt-get install -y -qq waydroid
  log_info "Waydroid 安装完成: $(waydroid --version 2>/dev/null || echo unknown)"
fi

log_step "步骤4: 配置 binder 模块和 binderfs"
BINDER_MODULE="$(detect_binder_module)"
OPTIONAL_MODULES=()
if modinfo ashmem_linux >/dev/null 2>&1; then
  OPTIONAL_MODULES+=("ashmem_linux")
fi

{
  echo "# OBD Waydroid 所需内核模块"
  echo "${BINDER_MODULE}"
  for mod in "${OPTIONAL_MODULES[@]}"; do
    echo "${mod}"
  done
} > /etc/modules-load.d/obd-android.conf

lsmod | grep -q "^${BINDER_MODULE}" || modprobe "${BINDER_MODULE}" || die "加载模块 ${BINDER_MODULE} 失败"
for mod in "${OPTIONAL_MODULES[@]}"; do
  lsmod | grep -q "^${mod}" || modprobe "${mod}" || log_warn "模块 ${mod} 加载失败（Android 12+ 通常可忽略）"
done

cat > /etc/systemd/system/obd-binderfs.service <<EOF
[Unit]
Description=Ensure binderfs is mounted for Waydroid
DefaultDependencies=no
After=local-fs.target systemd-modules-load.service
Before=multi-user.target

[Service]
Type=oneshot
RemainAfterExit=yes
ExecStart=/bin/bash -lc 'modprobe ${BINDER_MODULE} && mkdir -p /dev/binderfs && (mountpoint -q /dev/binderfs || mount -t binder binder /dev/binderfs) && test -c /dev/binderfs/binder-control'
ExecStop=/bin/bash -lc 'mountpoint -q /dev/binderfs && umount /dev/binderfs || true'

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable --now obd-binderfs.service >/dev/null 2>&1 || die "obd-binderfs.service 启动失败"
[[ -c /dev/binderfs/binder-control ]] || die "binderfs 未就绪：/dev/binderfs/binder-control 不存在"
log_info "binderfs 已就绪"

log_step "步骤5: 打 Waydroid 关键补丁"
patch_hardware_manager "/usr/lib/waydroid/tools/services/hardware_manager.py"
patch_lxc_py "/usr/lib/waydroid/tools/helpers/lxc.py"

log_step "步骤6: 创建部署目录"
mkdir -p "${DEPLOY_ROOT}" "${DEPLOY_SCRIPTS_DIR}"
mkdir -p /var/log/obd-agent
mkdir -p /etc/obd-agent
log_info "部署目录已创建"

log_step "步骤7: 放置 APK"
if [[ -n "${APK_SRC_PATH}" && -f "${APK_SRC_PATH}" ]]; then
  if [[ -f "${APK_DEST}" ]] && cmp -s "${APK_SRC_PATH}" "${APK_DEST}"; then
    log_info "APK 已存在且与源一致: ${APK_DEST}"
  else
    cp -f "${APK_SRC_PATH}" "${APK_DEST}"
    log_info "APK 已复制/更新: ${APK_DEST}  (src=${APK_SRC_PATH})"
  fi
elif [[ -f "${APK_DEST}" ]]; then
  log_info "APK 已存在: ${APK_DEST}"
else
  log_warn "APK 未找到。已检查默认候选："
  log_warn "  - ${YUN_DIR}/../Release_com.companyname.cardemo-Server-Signed.apk"
  log_warn "  - ${YUN_DIR}/../build_output/Release_com.companyname.cardemo-Signed.apk"
  log_warn "后续执行 bootstrap_slot1.sh 前必须先放置到 ${APK_DEST}"
fi

log_step "步骤8: 构建并部署应用文件"
[[ -d "${AGENT_SRC}" ]] || die "agent 源码不存在: ${AGENT_SRC}"
log_info "构建 agent..."
cd "${AGENT_SRC}"
npm ci --quiet 2>/dev/null || npm install --quiet 2>/dev/null
npm run build >/dev/null

mkdir -p "${DEPLOY_ROOT}/agent"
rm -rf "${DEPLOY_ROOT}/agent/dist"
cp -R dist "${DEPLOY_ROOT}/agent/"
cp package.json "${DEPLOY_ROOT}/agent/"
cp package-lock.json "${DEPLOY_ROOT}/agent/" 2>/dev/null || true
(
  cd "${DEPLOY_ROOT}/agent"
  npm ci --production --quiet >/dev/null 2>&1 || npm install --production --quiet >/dev/null 2>&1
)
log_info "agent 已部署到 ${DEPLOY_ROOT}/agent"

for file in "${SCRIPT_DIR}"/*.sh; do
  cp -f "${file}" "${DEPLOY_SCRIPTS_DIR}/"
done
chmod +x "${DEPLOY_SCRIPTS_DIR}"/*.sh
log_info "deployment scripts 已部署到 ${DEPLOY_SCRIPTS_DIR}"

log_step "步骤9: 写环境文件"
merge_agent_env "${DEPLOY_ROOT}/agent/.env"

cat > /etc/obd-instance.env <<EOF
# 由 bootstrap_host.sh 生成
SERVER_ID=${SERVER_ID}
PUBLIC_WS_HOST=${PUBLIC_WS_HOST}
PUBLIC_WS_SCHEME=${PUBLIC_WS_SCHEME}
AGENT_PORT=4000
SCHEDULER_URL=${SCHEDULER_URL}
SERVER_IP_OVERRIDE=${SERVER_IP_OVERRIDE}
EOF
log_info "环境文件已写入"

log_step "步骤10: 配置 Agent systemd 服务"
cat > /etc/systemd/system/obd-agent.service <<'UNIT'
[Unit]
Description=OBD Agent Service
After=network.target cardemo.service
Wants=cardemo.service

[Service]
Type=simple
WorkingDirectory=/opt/cardemo/agent
EnvironmentFile=/opt/cardemo/agent/.env
ExecStart=/usr/bin/node dist/index.js
Restart=always
RestartSec=5
StandardOutput=append:/var/log/obd-agent/agent.log
StandardError=append:/var/log/obd-agent/agent.err.log

[Install]
WantedBy=multi-user.target
UNIT

systemctl disable --now obd-scheduler >/dev/null 2>&1 || true
rm -f /etc/systemd/system/obd-scheduler.service
rm -f /etc/nginx/conf.d/obd-scheduler.conf

systemctl daemon-reload
systemctl enable --now obd-agent >/dev/null 2>&1 || die "obd-agent 启动失败"
systemctl enable --now nginx >/dev/null 2>&1 || die "nginx 启动失败"
nginx -t || die "Nginx 配置错误"
nginx -s reload >/dev/null 2>&1 || systemctl restart nginx || die "nginx reload 失败"
ensure_scheduler_agent_tcp_port_open 4000 "${SCHEDULER_URL}" "实例层 Agent 控制面"
log_info "nginx 已启动（实例 wsPort 入口由 bootstrap_slot1.sh / create_instance.sh 继续生成）"

log_step "验收: 实例层宿主机基线"
ERRORS=0

for cmd in waydroid adb weston node ip iptables ss nc nsenter; do
  command -v "${cmd}" >/dev/null 2>&1 && log_info "✓ ${cmd}" || { log_error "✗ ${cmd} 未安装"; ERRORS=$((ERRORS + 1)); }
done

python3 -c 'import dbus, websockets' >/dev/null 2>&1 && \
  log_info "✓ python3-dbus / python3-websockets" || \
  { log_error "✗ Python 依赖缺失（dbus / websockets）"; ERRORS=$((ERRORS + 1)); }

[[ -c /dev/binderfs/binder-control ]] && log_info "✓ binderfs" || { log_error "✗ binderfs 未就绪"; ERRORS=$((ERRORS + 1)); }
[[ -x "${DEPLOY_SCRIPTS_DIR}/create_instance.sh" ]] && log_info "✓ deployment scripts" || { log_error "✗ deployment scripts 未部署"; ERRORS=$((ERRORS + 1)); }
[[ -f /etc/obd-instance.env ]] && log_info "✓ /etc/obd-instance.env" || { log_error "✗ /etc/obd-instance.env 缺失"; ERRORS=$((ERRORS + 1)); }

systemctl is-active obd-agent >/dev/null 2>&1 && log_info "✓ obd-agent" || { log_error "✗ obd-agent 未运行"; ERRORS=$((ERRORS + 1)); }
systemctl is-active nginx >/dev/null 2>&1 && log_info "✓ nginx" || { log_error "✗ nginx 未运行"; ERRORS=$((ERRORS + 1)); }
curl -fsS --max-time 5 "${SCHEDULER_URL%/}/health" >/dev/null && log_info "✓ 远端 Scheduler API" || { log_error "✗ 远端 Scheduler API 不可用: ${SCHEDULER_URL%/}/health"; ERRORS=$((ERRORS + 1)); }

[[ ${ERRORS} -eq 0 ]] || die "实例层宿主机基线验收失败（${ERRORS} 项），请检查上方错误"

echo ""
echo "═══════════════════════════════════════════════════════════"
echo "  bootstrap_host 完成 ✓"
echo "  下一步:"
echo "    SERVER_ID=${SERVER_ID} PUBLIC_WS_HOST=${PUBLIC_WS_HOST} SCHEDULER_URL=${SCHEDULER_URL} PUBLIC_WS_SCHEME=${PUBLIC_WS_SCHEME} \\"
echo "    ${DEPLOY_SCRIPTS_DIR}/bootstrap_slot1.sh"
echo "═══════════════════════════════════════════════════════════"
