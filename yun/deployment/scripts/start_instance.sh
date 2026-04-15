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
  for s in $(seq "$SLOT_START" "$SLOT_END"); do
    bash "${BASH_SOURCE[0]}" "$s" || log_warn "slot=${s} 启动失败，继续"
  done
  exit 0
fi

SLOT="$SLOT_START"
derive_identity "$SLOT"
log_info "恢复实例运行时: slot=${SLOT} / ${INST_ID}"

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
adb -s "${ADB_TARGET}" forward "tcp:${PROBE_PORT}" "tcp:${APK_INTERNAL_PORT}"

log_info "实例 ${INST_ID} 恢复完成 ✓  WS: ${INST_WS_URL}"
