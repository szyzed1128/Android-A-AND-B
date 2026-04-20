#!/usr/bin/env bash
# =============================================================================
# run_weston_slot.sh  —  为指定 slot 启动受 systemd 托管的 weston
# =============================================================================

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${SCRIPT_DIR}/lib_instance_identity.sh"
source "${SCRIPT_DIR}/lib_runtime_common.sh"

SLOT="${1:?用法: $0 <SLOT_INDEX>}"
require_root
derive_identity "${SLOT}"
_LOG_PREFIX="[obd-weston:${SLOT_INDEX}]"

prepare_runtime_dirs
cleanup_stale_wayland_socket "${WAYLAND_DISPLAY}"
log_info "启动 weston: slot=${SLOT_INDEX} display=${WAYLAND_DISPLAY}"

exec env XDG_RUNTIME_DIR=/run/user/0 \
  weston \
  --backend=headless-backend.so \
  --no-config \
  --socket="${WAYLAND_DISPLAY}"
