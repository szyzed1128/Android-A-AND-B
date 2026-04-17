#!/usr/bin/env bash
# =============================================================================
# start_instance.sh  —  服务器重启后恢复实例运行时状态
# =============================================================================
# 用法：./start_instance.sh <START_SLOT> [END_SLOT | --count N]
#
# 只恢复"临时运行时"资源，不重新安装/配置静态文件：
#   - binder 设备（重启后消失）
#   - rootfs overlay 挂载（重启后消失）
#   - weston 进程
#   - bridge
#   - LXC 容器
#   - socat ADB 代理（坑四：必须用当前新 PID）
#   - adb forward
#
# 供 systemd obd-instance@N.service 的 ExecStart 调用。
# =============================================================================

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${SCRIPT_DIR}/lib_instance_identity.sh"

resolve_agent_instance_id() {
  local health_url="http://127.0.0.1:${AGENT_PORT}/health"
  curl -s --max-time 3 "${health_url}" 2>/dev/null | \
    python3 -c '
import json
import sys

expected_id = sys.argv[1]
expected_ws_port = int(sys.argv[2])

payload = json.load(sys.stdin)
rows = payload.get("instances") or []

exact = next((row for row in rows if row.get("id") == expected_id), None)
if exact:
    print(exact.get("id") or "")
    raise SystemExit(0)

matched = next((row for row in rows if int(row.get("wsPort") or -1) == expected_ws_port), None)
if matched:
    print(matched.get("id") or "")
' "${INST_ID}" "${WS_PORT}" 2>/dev/null || true
}

_parse_args() {
  local start="" end=""
  case "$#" in
    0) echo "用法: $0 <START_SLOT> [END_SLOT | --count N]" >&2; exit 1 ;;
    1) start="$1"; end="$1" ;;
    2) start="$1"; end="$2" ;;
    3) [[ "$2" == "--count" ]] || { echo "用法: $0 <START> --count N" >&2; exit 1; }
       start="$1"; end="$((start + $3 - 1))" ;;
    *) echo "参数过多" >&2; exit 1 ;;
  esac
  [[ "$start" -ge 2 ]] 2>/dev/null || { echo "SLOT >= 2" >&2; exit 1; }
  SLOT_START="$start"; SLOT_END="$end"
}

_parse_args "$@"
require_root

if [[ "$SLOT_START" -ne "$SLOT_END" ]]; then
  FAILED_SLOTS=()
  for s in $(seq "$SLOT_START" "$SLOT_END"); do
    bash "${BASH_SOURCE[0]}" "$s" || { log_warn "slot=${s} 启动失败，继续"; FAILED_SLOTS+=("$s"); }
  done
  [[ ${#FAILED_SLOTS[@]} -eq 0 ]] || die "以下 slot 启动失败: ${FAILED_SLOTS[*]}"
  exit 0
fi

SLOT="$SLOT_START"
derive_identity "$SLOT"
log_info "恢复实例运行时: slot=${SLOT} / ${INST_ID}"
: "${AGENT_PORT:=4000}"

# ─── 前置：目录必须存在（create_instance.sh 已创建过）────────────────────
[[ -d "${LXC_DIR}/lxc/${LXC_NAME}" ]] || \
  die "实例目录不存在: ${LXC_DIR}，请先运行 create_instance.sh ${SLOT}"

# ─── 1. binder 设备（重启后消失，必须重建）──────────────────────────────
if ! check_binder_devices 2>/dev/null; then
  log_info "重建 binder 设备..."
  create_binder_device "${BINDER_NAME}"
  create_binder_device "${HWBINDER_NAME}"
  create_binder_device "${VNDBINDER_NAME}"
else
  log_info "binder 设备已存在"
fi

# ─── 2. rootfs overlay 挂载 ──────────────────────────────────────────────
if ! is_rootfs_mounted; then
  log_info "重挂 rootfs..."
  BASE_IMAGES="${WAYDROID_BASE_DIR}/waydroid/images"
  BASE_OVERLAY="${WAYDROID_BASE_DIR}/waydroid/overlay"
  mount -o ro "${BASE_IMAGES}/system.img" "${ROOTFS_DIR}"
  mount -t overlay overlay \
    -o "lowerdir=${BASE_OVERLAY}:${ROOTFS_DIR},upperdir=${OVERLAY_RW_DIR}/system,workdir=${OVERLAY_WORK_DIR}/system" \
    "${ROOTFS_DIR}"
  mount -o ro "${BASE_IMAGES}/vendor.img" "${ROOTFS_DIR}/vendor"
  mount -t overlay overlay \
    -o "lowerdir=${BASE_OVERLAY}/vendor:${ROOTFS_DIR}/vendor,upperdir=${OVERLAY_RW_DIR}/vendor,workdir=${OVERLAY_WORK_DIR}/vendor" \
    "${ROOTFS_DIR}/vendor"
  # prop bind mount
  if [[ -f "${PROP_FILE}" ]]; then
    mountpoint -q "${ROOTFS_DIR}/vendor/waydroid.prop" 2>/dev/null || \
      mount --bind "${PROP_FILE}" "${ROOTFS_DIR}/vendor/waydroid.prop"
  fi
  log_info "rootfs 已挂载"
else
  log_info "rootfs 已挂载，跳过"
fi

# ─── 3. bridge ────────────────────────────────────────────────────────────
if ! ip link show "${BRIDGE_NAME}" &>/dev/null; then
  ip link add "${BRIDGE_NAME}" type bridge
fi
ip link set "${BRIDGE_NAME}" up
ip addr show "${BRIDGE_NAME}" | grep -q "${BRIDGE_IP}" || \
  ip addr add "${BRIDGE_ADDR}" dev "${BRIDGE_NAME}"
iptables -C FORWARD -i "${BRIDGE_NAME}" -j ACCEPT 2>/dev/null || \
  iptables -I FORWARD -i "${BRIDGE_NAME}" -j ACCEPT
iptables -C FORWARD -o "${BRIDGE_NAME}" -j ACCEPT 2>/dev/null || \
  iptables -I FORWARD -o "${BRIDGE_NAME}" -j ACCEPT
log_info "bridge ${BRIDGE_NAME} 就绪"

# ─── 4. weston ────────────────────────────────────────────────────────────
mkdir -p /run/user/0
if ! is_wayland_ready; then
  XDG_RUNTIME_DIR=/run/user/0 nohup weston \
    --backend=headless-backend.so --no-config \
    --socket="${WAYLAND_DISPLAY}" \
    > "/tmp/weston-${WAYLAND_DISPLAY}.log" 2>&1 &
  for i in $(seq 1 15); do
    is_wayland_ready && break; sleep 1
    [[ $i -eq 15 ]] && die "weston ${WAYLAND_DISPLAY} 启动超时"
  done
  log_info "weston (${WAYLAND_DISPLAY}) 已启动"
fi

mkdir -p /run/user/0/pulse; touch /run/user/0/pulse/native

# ─── 5. LXC 容器 ─────────────────────────────────────────────────────────
if ! is_container_running; then
  lxc-start -P "${LXC_DIR}/lxc" -n "${LXC_NAME}" -- /init &
  for i in $(seq 1 30); do
    is_container_running && break; sleep 1
    [[ $i -eq 30 ]] && die "容器 ${LXC_NAME} 启动超时"
  done
  log_info "容器 ${LXC_NAME} 已启动"
fi

# ─── 6. socat（坑四：必须用当前新 PID）──────────────────────────────────
CPID="$(get_container_pid)"
[[ -n "$CPID" ]] || die "无法获取容器 PID"
pkill -f "socat.*${ADB_PORT}" 2>/dev/null || true
sleep 1
for i in $(seq 1 30); do
  nsenter -t "${CPID}" -n -- ss -tlnp 2>/dev/null | grep -q ":5555" && break
  sleep 2; [[ $i -eq 30 ]] && die "adbd 未启动"
done
nohup socat \
  "TCP-LISTEN:${ADB_PORT},bind=127.0.0.1,reuseaddr,fork" \
  "EXEC:nsenter -t ${CPID} -n -- nc 127.0.0.1 5555" \
  >/dev/null 2>&1 &
sleep 2
log_info "socat 已启动 (PID_ref=${CPID})"

# ─── 7. ADB ──────────────────────────────────────────────────────────────
adb disconnect "${ADB_TARGET}" >/dev/null 2>&1 || true
adb connect "${ADB_TARGET}" >/dev/null 2>&1
sleep 2
is_adb_online || die "ADB ${ADB_TARGET} 连接失败"

# ─── 8. 等待 boot + 启动 APK ─────────────────────────────────────────────
for i in $(seq 1 60); do
  BOOT="$(adb -s "${ADB_TARGET}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r\n')"
  [[ "$BOOT" == "1" ]] && break; sleep 3
  [[ $i -eq 60 ]] && die "boot_completed 超时"
done

adb -s "${ADB_TARGET}" shell am force-stop "${APK_PACKAGE}" >/dev/null 2>&1 || true
sleep 2
adb -s "${ADB_TARGET}" shell am start -n "${APK_PACKAGE}/${APK_ACTIVITY}" >/dev/null 2>&1

for i in $(seq 1 40); do
  WS="$(adb -s "${ADB_TARGET}" shell ss -tlnp 2>/dev/null | grep ":${APK_INTERNAL_PORT}" || true)"
  [[ -n "$WS" ]] && break; sleep 5
  [[ $i -eq 40 ]] && die "APK WebSocket 超时"
done

# ─── 9. adb forward ───────────────────────────────────────────────────────
adb -s "${ADB_TARGET}" forward --remove "tcp:${PROBE_PORT}" >/dev/null 2>&1 || true
adb -s "${ADB_TARGET}" forward "tcp:${PROBE_PORT}" "tcp:${APK_INTERNAL_PORT}" >/dev/null

FORWARD_ROW="$(adb forward --list 2>/dev/null | \
  grep -E "^${ADB_TARGET}[[:space:]]+tcp:${PROBE_PORT}[[:space:]]+tcp:${APK_INTERNAL_PORT}$" || true)"
[[ -n "${FORWARD_ROW}" ]] || die "adb forward 校验失败: ${ADB_TARGET} tcp:${PROBE_PORT} -> tcp:${APK_INTERNAL_PORT}"

HTTP_STATUS="$(
  curl -s -o /dev/null -w '%{http_code}' \
    --http1.1 \
    --max-time 5 \
    -H 'Connection: Upgrade' \
    -H 'Upgrade: websocket' \
    -H 'Sec-WebSocket-Version: 13' \
    -H 'Sec-WebSocket-Key: restart-check==' \
    "http://127.0.0.1:${PROBE_PORT}${WS_PATH}" 2>/dev/null || true
)"
[[ -n "${HTTP_STATUS}" ]] || HTTP_STATUS="000"
[[ "${HTTP_STATUS}" == "101" ]] || die "probePort ${PROBE_PORT} WebSocket 校验失败: HTTP ${HTTP_STATUS}"
log_info "adb forward 已恢复 (${PROBE_PORT} → ${APK_INTERNAL_PORT})"

if curl -s --max-time 3 "http://127.0.0.1:${AGENT_PORT}/health" >/dev/null 2>&1; then
  AGENT_INSTANCE_ID="${INST_ID}"
  RESOLVED_AGENT_INSTANCE_ID="$(resolve_agent_instance_id)"
  if [[ -n "${RESOLVED_AGENT_INSTANCE_ID}" ]]; then
    AGENT_INSTANCE_ID="${RESOLVED_AGENT_INSTANCE_ID}"
    [[ "${AGENT_INSTANCE_ID}" == "${INST_ID}" ]] || \
      log_info "检测到 Agent 实例 ID 与正式身份证不一致，已按运行时 ID 收敛: ${AGENT_INSTANCE_ID}"
  fi

  log_info "通知 Agent 重新收敛实例状态..."
  curl -fsS --max-time 5 -X POST "http://127.0.0.1:${AGENT_PORT}/instance/${AGENT_INSTANCE_ID}/reset" >/dev/null || \
    die "通知 Agent 重置实例失败: ${AGENT_INSTANCE_ID}"

  for i in $(seq 1 24); do
    AGENT_STATUS="$(curl -s --max-time 3 "http://127.0.0.1:${AGENT_PORT}/health" 2>/dev/null | \
      python3 -c "
import sys, json
try:
    payload = json.load(sys.stdin)
    rows = payload.get('instances') or []
    row = next((item for item in rows if item.get('id') == '${AGENT_INSTANCE_ID}'), None)
    print((row or {}).get('status') or 'not_found')
except Exception:
    print('err')
" 2>/dev/null || echo 'err')"
    if [[ "${AGENT_STATUS}" == "idle" ]]; then
      log_info "Agent 已将 ${AGENT_INSTANCE_ID} 收敛为 idle"
      break
    fi
    [[ $i -eq 24 ]] && die "等待 Agent 将 ${AGENT_INSTANCE_ID} 收敛为 idle 超时（当前=${AGENT_STATUS}）"
    sleep 5
  done
else
  log_warn "obd-agent 未在线，跳过自动收敛；实例运行态已恢复，但调度池状态可能滞后"
fi

log_info "实例 ${INST_ID} 恢复完成 ✓  WS: ${INST_WS_URL}"
