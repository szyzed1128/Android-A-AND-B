#!/usr/bin/env bash
# =============================================================================
# stop_instance.sh  —  停止实例运行时资源（逆序清理）
# =============================================================================
# 用法：./stop_instance.sh <START_SLOT> [END_SLOT | --count N]
#
# 停止顺序（create 的逆序）：
#   1. socat
#   2. adb forward 清除
#   3. APK 强制停止
#   4. LXC 容器
#   5. weston
#   6. bridge
#   7. rootfs umount
#
# 不删除任何持久化文件（LXC config / prop / Nginx conf），只清理运行时状态。
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
  for s in $(seq "$SLOT_END" -1 "$SLOT_START"); do
    bash "${BASH_SOURCE[0]}" "$s" || log_warn "slot=${s} 停止失败，继续"
  done
  exit 0
fi

SLOT="$SLOT_START"
derive_identity "$SLOT"
log_info "停止实例: slot=${SLOT} / ${INST_ID}"

# 1. socat
pkill -f "socat.*${ADB_PORT}" 2>/dev/null && log_info "socat 已停止" || true

# 2. adb forward 清除
adb -s "${ADB_TARGET}" forward --remove "tcp:${PROBE_PORT}" >/dev/null 2>&1 || true
adb disconnect "${ADB_TARGET}" >/dev/null 2>&1 || true
log_info "adb forward 已清除"

# 3. APK 停止（容器还活着时）
if is_container_running; then
  adb -s "${ADB_TARGET}" shell am force-stop "${APK_PACKAGE}" >/dev/null 2>&1 || true
  log_info "APK 已停止"
fi

# 4. LXC 容器
if is_container_running; then
  lxc-stop -P "${LXC_DIR}/lxc" -n "${LXC_NAME}" -k 2>/dev/null || true
  log_info "容器 ${LXC_NAME} 已停止"
fi

# 5. weston（只停属于本实例的 weston）
if is_wayland_ready; then
  pkill -f "weston.*${WAYLAND_DISPLAY}" 2>/dev/null || true
  log_info "weston (${WAYLAND_DISPLAY}) 已停止"
fi

# 6. bridge
if ip link show "${BRIDGE_NAME}" &>/dev/null; then
  ip link set "${BRIDGE_NAME}" down 2>/dev/null || true
  ip link delete "${BRIDGE_NAME}" 2>/dev/null || true
  log_info "bridge ${BRIDGE_NAME} 已删除"
fi

# 7. rootfs umount（逆序卸载 4 层）
if is_rootfs_mounted; then
  umount "${ROOTFS_DIR}/vendor/waydroid.prop" 2>/dev/null || true
  umount "${ROOTFS_DIR}/vendor" 2>/dev/null || true  # overlay
  umount "${ROOTFS_DIR}/vendor" 2>/dev/null || true  # ext4
  umount "${ROOTFS_DIR}" 2>/dev/null || true          # overlay
  umount "${ROOTFS_DIR}" 2>/dev/null || true          # ext4
  log_info "rootfs 已卸载"
fi

log_info "实例 ${INST_ID} 已完全停止"
