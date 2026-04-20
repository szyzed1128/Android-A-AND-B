#!/usr/bin/env bash
# =============================================================================
# lib_instance_identity.sh  —  Waydroid 多实例身份证派生函数库
# =============================================================================
# 用法：source lib_instance_identity.sh
#       derive_identity <SLOT_INDEX>
#       echo "$INST_ID / $LXC_NAME / $PROBE_PORT ..."
#
# ─────────────────────────────────────────────────────────────────────────────
# ⚠️  当前脚本集的明确边界（问题4）：
#
#   本库及配套脚本（create/start/stop/reset/status/install_persist）
#   只负责 slot_index >= 2 的实例管理。
#
#   slot_1（Waydroid 官方实例）不由本脚本集管理：
#     - 它由 Waydroid 官方工具链（waydroid init / waydroid session start）初始化
#     - 由 /etc/systemd/system/cardemo.service + /opt/cardemo/startup.sh 启动
#     - 本脚本只做"兼容识别"，不负责创建或管理
#
#   因此，当前脚本集的真实定位是：
#     "在已有官方 slot_1 基线的前提下，创建与管理 slot_2+"
#
#   如需在新 EC2 上从零部署完整 slot_1~N 系统，正式入口为：
#     - bootstrap_scheduler_host.sh : 独立调度层基线（Redis/Scheduler/Nginx:8080）
#     - bootstrap_host.sh           : 实例层宿主机基线（Waydroid/ADB/Node.js/Nginx）
#     - bootstrap_slot1.sh          : 官方实例初始化（waydroid init + APK + cardemo.service）
# ─────────────────────────────────────────────────────────────────────────────
#
# 所有其他脚本（create/start/stop/reset/status）都应该 source 本文件，
# 然后调用 derive_identity 获取完整字段，不得在脚本正文里硬写字面量。
#
# ─────────────────────────────────────────────────────────────────────────────
# 设计原则（来自避坑文档）：
#   A. 所有可参数化的资源名（路径/端口/设备名）都按公式派生
#   B. 协议固定字段（waydroid.* key、内部端口 8080、/ws 路径）永远不随实例号改变
#   C. slot_index=1 是 Waydroid 官方实例，由脚本特判兼容，不由本库管理创建
#   D. 容器 PID 是动态值，每次运行时通过 get_container_pid 探测，绝不硬写
# =============================================================================

# ─── 必填/可选环境变量（问题7：不能有危险的硬编码默认值）────────────────────
# 必填：
#   export SERVER_ID=ec2-apne1-a01
# 可选（显式覆盖 Agent 自动探测的公网地址）：
#   export PUBLIC_WS_HOST=1.2.3.4
# 若 SERVER_ID 未设置，derive_identity 会直接报错退出。
# PUBLIC_WS_HOST 留空时，仅影响脚本打印出来的对外地址展示；真正运行时由 Agent 自动探测。
__IDENTITY_ENV_LOADED=0
_load_identity_env_from_file_once() {
  [[ "${__IDENTITY_ENV_LOADED}" == "1" ]] && return 0
  __IDENTITY_ENV_LOADED=1

  local env_file="${OBD_INSTANCE_ENV_FILE:-/etc/obd-instance.env}"
  [[ -f "${env_file}" ]] || return 0

  set -a
  # shellcheck disable=SC1090
  source "${env_file}"
  set +a
}

_require_env() {
  local var="$1"
  _load_identity_env_from_file_once
  [[ -n "${!var:-}" ]] || {
    echo "[identity] 错误: 环境变量 ${var} 未设置。" >&2
    echo "[identity] 用法示例: SERVER_ID=my-server $0 ..." >&2
    exit 1
  }
  [[ "${!var}" != PLEASE_SET_* ]] || {
    echo "[identity] 错误: 环境变量 ${var} 未设置。" >&2
    echo "[identity] 用法示例: SERVER_ID=my-server $0 ..." >&2
    exit 1
  }
}

: "${PUBLIC_WS_SCHEME:=ws}"
: "${APK_PACKAGE:=com.companyname.cardemo}"
: "${APK_ACTIVITY:=crc64b16463db6be126c1.MainActivity}"
: "${APK_PATH:=/opt/cardemo/Release_com.companyname.cardemo-Server-Signed.apk}"
: "${DEFAULT_BRAND:=AITO}"
: "${PUBLIC_INGRESS_CIDRS:=0.0.0.0/0}"
: "${AGENT_INGRESS_CIDRS:=}"
: "${FIREWALL_AUTOMATION_STRICT:=1}"
: "${WAYDROID_BASE_DIR:=/var/lib}"   # Waydroid LXC 目录根，固定在 /var/lib
: "${DATA_BASE_DIR:=/root/.local/share}"

# 端口基址（slot_N 的端口 = base + slot_index）
: "${ADB_BASE_PORT:=5554}"       # slot=1→5555, slot=2→5556, slot=3→5557
: "${PROBE_BASE_PORT:=18080}"    # slot=1→18081, slot=2→18082
: "${WS_BASE_PORT:=8080}"        # slot=1→8081*, slot=2→8082
# *注意：实际部署中 inst_1 用 8080（历史原因）；新 EC2 上建议统一从 8081 开始

# ─── 派生函数 ─────────────────────────────────────────────────────────────────

derive_identity() {
  local slot="${1:?用法: derive_identity <SLOT_INDEX>}"

  if ! [[ "$slot" =~ ^[1-9][0-9]*$ ]]; then
    echo "[identity] 错误: SLOT_INDEX 必须是正整数，收到: $slot" >&2
    return 1
  fi

  # 必填环境变量校验（问题7）
  _require_env SERVER_ID
  _load_identity_env_from_file_once
  [[ "${PUBLIC_WS_HOST:-}" == PLEASE_SET_* ]] && PUBLIC_WS_HOST=""

  # ── 静态派生字段 ──────────────────────────────────────────────────────────

  SLOT_INDEX="$slot"
  SLOT_2D="$(printf '%02d' "$slot")"

  # 调度层全局实例 ID
  INST_ID="${SERVER_ID}_inst_${SLOT_2D}"

  # LXC 容器名（slot=1 为官方默认名 waydroid，slot>=2 追加编号）
  if [[ "$slot" -eq 1 ]]; then
    LXC_NAME="waydroid"
  else
    LXC_NAME="waydroid${slot}"
  fi

  # 目录结构
  LXC_DIR="${WAYDROID_BASE_DIR}/${LXC_NAME}"       # /var/lib/waydroid2
  ROOTFS_DIR="${LXC_DIR}/rootfs"
  HOST_PERMS_DIR="${LXC_DIR}/host-permissions"
  OVERLAY_RW_DIR="${LXC_DIR}/overlay_rw"
  OVERLAY_WORK_DIR="${LXC_DIR}/overlay_work"
  DATA_DIR="${DATA_BASE_DIR}/${LXC_NAME}/data"     # /root/.local/share/waydroid2/data
  PROP_FILE="${LXC_DIR}/waydroid.prop"

  # binder 设备名（slot=1 保持 Waydroid 官方名，slot>=2 按编号）
  if [[ "$slot" -eq 1 ]]; then
    BINDER_NAME="anbox-binder"
    HWBINDER_NAME="anbox-hwbinder"
    VNDBINDER_NAME="anbox-vndbinder"
    BINDER_DEV="/dev/anbox-binder"
    HWBINDER_DEV="/dev/anbox-hwbinder"
    VNDBINDER_DEV="/dev/anbox-vndbinder"
  else
    BINDER_NAME="inst${slot}-binder"
    HWBINDER_NAME="inst${slot}-hwbinder"
    VNDBINDER_NAME="inst${slot}-vndbinder"
    BINDER_DEV="/dev/binderfs/${BINDER_NAME}"
    HWBINDER_DEV="/dev/binderfs/${HWBINDER_NAME}"
    VNDBINDER_DEV="/dev/binderfs/${VNDBINDER_NAME}"
  fi

  # Wayland socket 名：slot=N → wayland-{N-1}
  WAYLAND_DISPLAY="wayland-$((slot - 1))"

  # bridge 名：slot=N → waydroid{N-1}  (slot=1→waydroid0, slot=2→waydroid1)
  BRIDGE_NAME="waydroid$((slot - 1))"

  # bridge 地址（问题2修复：严格拆分主机地址与网段，不能混用）
  # BRIDGE_ADDR：网卡上实际配置的地址（含前缀长度），用于 ip addr add
  # BRIDGE_SUBNET：网络段地址，用于路由/iptables 规则引用
  # 10.200.x.0/24 网段支持 slot=2..254（253 个实例），无溢出风险
  BRIDGE_ADDR="10.200.${slot}.1/24"    # 配置到网卡的地址+前缀
  BRIDGE_SUBNET="10.200.${slot}.0/24"  # 网段（路由/规则用）
  BRIDGE_IP="10.200.${slot}.1"         # 纯 IP（检查存在性时用）

  # bridge MAC（00:16:3e:f9:d3:0N，N=slot+2，与 live 实验一致）
  BRIDGE_MAC="$(printf '00:16:3e:f9:d3:%02x' $((slot + 2)))"

  # ADB / 端口
  ADB_PORT="$((ADB_BASE_PORT + slot))"           # 5555, 5556, 5557...
  ADB_TARGET="localhost:${ADB_PORT}"             # Agent 用的 adb 地址
  PROBE_PORT="$((PROBE_BASE_PORT + slot))"       # 18081, 18082...
  WS_PORT="$((WS_BASE_PORT + slot))"             # 8081, 8082...  (slot=1→8081)
  # 注意：live 环境 inst_1 实际用 8080，因为它是 Waydroid 官方实例直接暴露的。
  # 新 EC2 上所有实例统一走此公式，不存在该不一致。

  # 对外 WebSocket URL（供脚本展示/提示）
  # 真正运行时若未显式设置 PUBLIC_WS_HOST，则由 Agent 自动探测公网地址并上报给 Scheduler。
  if [[ -n "${PUBLIC_WS_HOST:-}" ]]; then
    INST_PUBLIC_HOST_DISPLAY="${PUBLIC_WS_HOST}"
  else
    INST_PUBLIC_HOST_DISPLAY="<agent-auto-detect-public-host>"
  fi
  INST_WS_URL="${PUBLIC_WS_SCHEME}://${INST_PUBLIC_HOST_DISPLAY}:${WS_PORT}/ws"

  # ── 协议固定字段（不随实例号变化，坑三的教训）──────────────────────────
  APK_INTERNAL_PORT=8080       # APK 内部监听端口，永远是 8080
  WS_PATH="/ws"                # WebSocket 路径，永远是 /ws
  # waydroid.* 配置 key 也属于协议固定字段，在 generate_prop_content() 中保证

  # ── Nginx 配置文件路径 ────────────────────────────────────────────────────
  NGINX_CONF="/etc/nginx/conf.d/obd-inst-${SLOT_2D}.conf"

  # ── systemd 服务名 ────────────────────────────────────────────────────────
  SYSTEMD_SERVICE="obd-instance@${SLOT_INDEX}.service"

  export SLOT_INDEX SLOT_2D INST_ID LXC_NAME
  export LXC_DIR ROOTFS_DIR HOST_PERMS_DIR OVERLAY_RW_DIR OVERLAY_WORK_DIR
  export DATA_DIR PROP_FILE
  export BINDER_NAME HWBINDER_NAME VNDBINDER_NAME
  export BINDER_DEV HWBINDER_DEV VNDBINDER_DEV
  export WAYLAND_DISPLAY BRIDGE_NAME BRIDGE_IP BRIDGE_ADDR BRIDGE_SUBNET BRIDGE_MAC
  export ADB_PORT ADB_TARGET PROBE_PORT WS_PORT INST_PUBLIC_HOST_DISPLAY INST_WS_URL
  export APK_INTERNAL_PORT WS_PATH NGINX_CONF SYSTEMD_SERVICE
}

# ─── 动态运行时探测函数 ───────────────────────────────────────────────────────

# 获取容器当前 init 进程的宿主机 PID（坑四：必须每次运行时探测，绝不硬写）
get_container_pid() {
  lxc-info -P "${LXC_DIR}/lxc" -n "${LXC_NAME}" 2>/dev/null \
    | grep "^PID:" | awk '{print $2}' | head -1
}

# 检查容器是否正在运行
is_container_running() {
  lxc-info -P "${LXC_DIR}/lxc" -n "${LXC_NAME}" -sH 2>/dev/null | grep -q "RUNNING"
}

# 检查 binder 设备是否存在且权限正确（坑一的检查）
check_binder_devices() {
  local ok=0
  for dev in "$BINDER_DEV" "$HWBINDER_DEV" "$VNDBINDER_DEV"; do
    if [[ ! -c "$dev" ]]; then
      echo "[identity] binder 设备不存在: $dev" >&2
      ok=1
    elif [[ "$(stat -c '%a' "$dev")" != "666" ]]; then
      echo "[identity] binder 设备权限不正确（应为 666）: $dev" >&2
      ok=1
    fi
  done
  return $ok
}

# 检查 rootfs 是否已挂载（需要 4 层都在）
is_rootfs_mounted() {
  mountpoint -q "${ROOTFS_DIR}" 2>/dev/null && \
  mountpoint -q "${ROOTFS_DIR}/vendor" 2>/dev/null
}

# 检查 weston 对应 wayland socket 是否存在
is_wayland_ready() {
  grep -q "${WAYLAND_DISPLAY}$" /proc/net/unix 2>/dev/null
}

# 检查 ADB 设备在线
is_adb_online() {
  adb devices 2>/dev/null | grep -q "^${ADB_TARGET}.*device$"
}

# ─── waydroid.prop 内容生成 ───────────────────────────────────────────────────
# 警告：key 前缀永远是 waydroid.*，绝不改成 waydroid2.* 等（坑三）
generate_prop_content() {
  local src_prop="${WAYDROID_BASE_DIR}/waydroid/waydroid.prop"
  python3 - << PYEOF
import os
with open("${src_prop}", "r") as f:
    lines = f.readlines()
result = []
for line in lines:
    line = line.rstrip('\n')
    if "=" in line:
        key, _, val = line.partition("=")
        # 只更新 value 中的路径，key 前缀保持 waydroid.*（坑三的关键）
        if key == "waydroid.host_data_path":
            val = "${DATA_DIR}"
        # xdg_runtime_dir / wayland_display / pulse_runtime_path 保持原值不变
        result.append(f"{key}={val}")
    else:
        result.append(line)
print('\n'.join(result))
PYEOF
}

# ─── 日志工具 ─────────────────────────────────────────────────────────────────
_LOG_PREFIX="[obd-inst]"
log_info()  { echo "${_LOG_PREFIX} [INFO ] $*"; }
log_warn()  { echo "${_LOG_PREFIX} [WARN ] $*" >&2; }
log_error() { echo "${_LOG_PREFIX} [ERROR] $*" >&2; }
log_step()  { echo ""; echo "${_LOG_PREFIX} ── $* ──"; }

die() { log_error "$*"; exit 1; }

# 要求以 root 运行
require_root() {
  [[ $EUID -eq 0 ]] || die "必须以 root 运行"
}

_split_list_items() {
  local raw="${1:-}"
  python3 - "$raw" <<'PYEOF'
import sys
raw = sys.argv[1]
for part in raw.replace(",", " ").split():
    item = part.strip()
    if item:
        print(item)
PYEOF
}

extract_url_host() {
  local url="${1:-}"
  python3 - "$url" <<'PYEOF'
import sys
from urllib.parse import urlparse

url = sys.argv[1].strip()
parsed = urlparse(url)
host = parsed.hostname or ""
if not host:
    raise SystemExit(1)
print(host)
PYEOF
}

resolve_host_ipv4s() {
  local host="${1:-}"
  python3 - "$host" <<'PYEOF'
import socket
import sys

host = sys.argv[1].strip()
seen = []
for info in socket.getaddrinfo(host, None, socket.AF_INET, socket.SOCK_STREAM):
    ip = info[4][0]
    if ip not in seen:
        seen.append(ip)
for ip in seen:
    print(ip)
PYEOF
}

_firewall_fail() {
  local message="$1"
  if [[ "${FIREWALL_AUTOMATION_STRICT}" == "1" ]]; then
    die "${message}"
  fi
  log_warn "${message}"
}

_aws_imds_get() {
  local path="$1"
  local base="${AWS_IMDS_BASE_URL:-http://169.254.169.254}"
  local token
  token="$(curl -fsS -m 1 -X PUT "${base}/latest/api/token" \
    -H "X-aws-ec2-metadata-token-ttl-seconds: 60" 2>/dev/null || true)"
  if [[ -n "${token}" ]]; then
    curl -fsS -m 1 -H "X-aws-ec2-metadata-token: ${token}" \
      "${base}/latest/${path}" 2>/dev/null
    return $?
  fi
  curl -fsS -m 1 "${base}/latest/${path}" 2>/dev/null
}

is_aws_ec2() {
  _aws_imds_get "meta-data/instance-id" >/dev/null 2>&1
}

detect_aws_region() {
  if [[ -n "${AWS_REGION:-}" ]]; then
    echo "${AWS_REGION}"
    return 0
  fi
  if [[ -n "${AWS_DEFAULT_REGION:-}" ]]; then
    echo "${AWS_DEFAULT_REGION}"
    return 0
  fi

  local doc
  doc="$(_aws_imds_get "dynamic/instance-identity/document" || true)"
  [[ -n "${doc}" ]] || return 1
  python3 - <<'PYEOF' <<<"${doc}"
import json
import sys

payload = json.load(sys.stdin)
region = str(payload.get("region") or "").strip()
if not region:
    raise SystemExit(1)
print(region)
PYEOF
}

detect_aws_security_group_ids() {
  if [[ -n "${AWS_SECURITY_GROUP_IDS:-}" ]]; then
    _split_list_items "${AWS_SECURITY_GROUP_IDS}"
    return 0
  fi

  local macs mac
  macs="$(_aws_imds_get "meta-data/network/interfaces/macs/" || true)"
  [[ -n "${macs}" ]] || return 1
  while read -r mac; do
    [[ -n "${mac}" ]] || continue
    _aws_imds_get "meta-data/network/interfaces/macs/${mac}security-group-ids" 2>/dev/null || true
  done <<<"${macs}" | awk 'NF {print $1}' | sort -u
}

_aws_authorize_cidr() {
  local sg_id="$1"
  local region="$2"
  local port="$3"
  local cidr="$4"
  local output rc=0

  output="$(aws ec2 authorize-security-group-ingress \
    --region "${region}" \
    --group-id "${sg_id}" \
    --protocol tcp \
    --port "${port}" \
    --cidr "${cidr}" 2>&1)" || rc=$?

  if [[ ${rc} -eq 0 ]] || [[ "${output}" == *"InvalidPermission.Duplicate"* ]]; then
    return 0
  fi

  log_error "AWS Security Group 放行失败 sg=${sg_id} port=${port} cidr=${cidr}: ${output}"
  return 1
}

_ensure_ufw_public_tcp_port_open() {
  local port="$1"
  local label="$2"
  local cidr count=0
  while read -r cidr; do
    [[ -n "${cidr}" ]] || continue
    ufw allow proto tcp from "${cidr}" to any port "${port}" >/dev/null 2>&1 || \
      _firewall_fail "${label} 的 ufw 放行失败: tcp/${port} from ${cidr}"
    count=$((count + 1))
  done < <(_split_list_items "${PUBLIC_INGRESS_CIDRS}")
  [[ ${count} -gt 0 ]] || _firewall_fail "${label} 的 PUBLIC_INGRESS_CIDRS 为空，未生成任何 ufw 规则"
  log_info "${label}: ufw 已放行 tcp/${port}"
}

_resolve_agent_ingress_cidrs() {
  local scheduler_url="$1"
  if [[ -n "${AGENT_INGRESS_CIDRS:-}" ]]; then
    _split_list_items "${AGENT_INGRESS_CIDRS}"
    return 0
  fi

  local host
  host="$(extract_url_host "${scheduler_url}" 2>/dev/null)" || return 1
  resolve_host_ipv4s "${host}" 2>/dev/null | sed 's#$#/32#'
}

_ensure_ufw_scheduler_tcp_port_open() {
  local port="$1"
  local scheduler_url="$2"
  local label="$3"
  local cidr count=0

  while read -r cidr; do
    [[ -n "${cidr}" ]] || continue
    ufw allow proto tcp from "${cidr}" to any port "${port}" >/dev/null 2>&1 || \
      _firewall_fail "${label} 的 ufw 放行失败: tcp/${port} from ${cidr}"
    count=$((count + 1))
  done < <(_resolve_agent_ingress_cidrs "${scheduler_url}")

  [[ ${count} -gt 0 ]] || _firewall_fail "${label} 无法从 SCHEDULER_URL / AGENT_INGRESS_CIDRS 推导来源地址"
  log_info "${label}: ufw 已按调度层来源放行 tcp/${port}"
}

_ensure_aws_public_tcp_port_open() {
  local port="$1"
  local label="$2"
  local region sg_id cidr sg_count=0 cidr_count=0

  region="$(detect_aws_region)" || {
    _firewall_fail "无法自动识别 AWS region，无法为 ${label} 配置 Security Group"
    return 1
  }
  while read -r sg_id; do
    [[ -n "${sg_id}" ]] || continue
    sg_count=$((sg_count + 1))
    while read -r cidr; do
      [[ -n "${cidr}" ]] || continue
      cidr_count=$((cidr_count + 1))
      _aws_authorize_cidr "${sg_id}" "${region}" "${port}" "${cidr}" || \
        _firewall_fail "${label} 的 AWS Security Group 放行失败: sg=${sg_id} tcp/${port} ${cidr}"
    done < <(_split_list_items "${PUBLIC_INGRESS_CIDRS}")
  done < <(detect_aws_security_group_ids)
  [[ ${sg_count} -gt 0 ]] || _firewall_fail "${label} 无法发现当前 EC2 的 Security Group"
  [[ ${cidr_count} -gt 0 ]] || _firewall_fail "${label} 的 PUBLIC_INGRESS_CIDRS 为空，未生成任何 AWS 规则"
  log_info "${label}: AWS Security Group 已放行 tcp/${port}"
}

_ensure_aws_scheduler_tcp_port_open() {
  local port="$1"
  local scheduler_url="$2"
  local label="$3"
  local region sg_id cidr sg_count=0 cidr_count=0

  region="$(detect_aws_region)" || {
    _firewall_fail "无法自动识别 AWS region，无法为 ${label} 配置 Security Group"
    return 1
  }
  while read -r sg_id; do
    [[ -n "${sg_id}" ]] || continue
    sg_count=$((sg_count + 1))
    while read -r cidr; do
      [[ -n "${cidr}" ]] || continue
      cidr_count=$((cidr_count + 1))
      _aws_authorize_cidr "${sg_id}" "${region}" "${port}" "${cidr}" || \
        _firewall_fail "${label} 的 AWS Security Group 放行失败: sg=${sg_id} tcp/${port} ${cidr}"
    done < <(_resolve_agent_ingress_cidrs "${scheduler_url}")
  done < <(detect_aws_security_group_ids)
  [[ ${sg_count} -gt 0 ]] || _firewall_fail "${label} 无法发现当前 EC2 的 Security Group"
  [[ ${cidr_count} -gt 0 ]] || _firewall_fail "${label} 无法从 SCHEDULER_URL / AGENT_INGRESS_CIDRS 推导来源地址"
  log_info "${label}: AWS Security Group 已按调度层来源放行 tcp/${port}"
}

ensure_public_tcp_port_open() {
  local port="$1"
  local label="${2:-端口 ${port}}"
  local configured=0

  if command -v ufw >/dev/null 2>&1; then
    _ensure_ufw_public_tcp_port_open "${port}" "${label}"
    configured=1
  fi

  if is_aws_ec2; then
    command -v aws >/dev/null 2>&1 || {
      _firewall_fail "当前是 AWS EC2，但未安装 aws CLI，无法自动放行 ${label}"
      return 1
    }
    _ensure_aws_public_tcp_port_open "${port}" "${label}"
    configured=1
  fi

  [[ ${configured} -eq 1 ]] || \
    log_warn "${label}: 未检测到 ufw / AWS EC2，未自动配置防火墙，请手工确认 tcp/${port}"
}

ensure_scheduler_agent_tcp_port_open() {
  local port="$1"
  local scheduler_url="$2"
  local label="${3:-Agent 控制面端口}"
  local configured=0

  if command -v ufw >/dev/null 2>&1; then
    _ensure_ufw_scheduler_tcp_port_open "${port}" "${scheduler_url}" "${label}"
    configured=1
  fi

  if is_aws_ec2; then
    command -v aws >/dev/null 2>&1 || {
      _firewall_fail "当前是 AWS EC2，但未安装 aws CLI，无法自动放行 ${label}"
      return 1
    }
    _ensure_aws_scheduler_tcp_port_open "${port}" "${scheduler_url}" "${label}"
    configured=1
  fi

  [[ ${configured} -eq 1 ]] || \
    log_warn "${label}: 未检测到 ufw / AWS EC2，未自动配置防火墙，请手工确认 tcp/${port}"
}

# 创建 binderfs 设备（slot>=2 调用，slot=1 不需要）
create_binder_device() {
  local name="$1"
  local path="/dev/binderfs/${name}"
  if [[ -c "$path" ]]; then
    log_info "binder 设备已存在，校正权限: $path"
    chmod 666 "$path"
    return 0
  fi
  python3 - << PYEOF
import fcntl, os, sys
BINDER_CTL_ADD = (3 << 30) | (ord('b') << 8) | 1 | (264 << 16)
buf = bytearray(264)
name = "${name}".encode()
buf[:len(name)] = name
try:
    fd = os.open('/dev/binderfs/binder-control', os.O_RDWR)
    fcntl.ioctl(fd, BINDER_CTL_ADD, buf)
    os.close(fd)
    os.chmod("${path}", 0o666)
    print(f"  created: ${path}")
except Exception as e:
    print(f"  failed: ${path}: {e}", file=sys.stderr)
    sys.exit(1)
PYEOF
}

# 打印当前实例身份证（调试用）
print_identity() {
  echo "─────────────────────────────────────────"
  echo "  实例身份证 slot=${SLOT_INDEX}"
  echo "─────────────────────────────────────────"
  echo "  INST_ID         = ${INST_ID}"
  echo "  LXC_NAME        = ${LXC_NAME}"
  echo "  LXC_DIR         = ${LXC_DIR}"
  echo "  DATA_DIR        = ${DATA_DIR}"
  echo "  binder          = ${BINDER_NAME} / ${HWBINDER_NAME} / ${VNDBINDER_NAME}"
  echo "  WAYLAND_DISPLAY = ${WAYLAND_DISPLAY}"
  echo "  BRIDGE_NAME     = ${BRIDGE_NAME}  addr=${BRIDGE_ADDR}  subnet=${BRIDGE_SUBNET}"
  echo "  ADB_TARGET      = ${ADB_TARGET}"
  echo "  PROBE_PORT      = ${PROBE_PORT}"
  echo "  WS_PORT         = ${WS_PORT}"
  echo "  PUBLIC_WS_HOST  = ${PUBLIC_WS_HOST:-<agent-auto-detect-public-host>}"
  echo "  INST_WS_URL     = ${INST_WS_URL}"
  echo "  NGINX_CONF      = ${NGINX_CONF}"
  echo "─────────────────────────────────────────"
}
