#!/usr/bin/env bash
# =============================================================================
# install_persist.sh  —  为已创建的实例安装 systemd 开机持久化
# =============================================================================
# 用法：./install_persist.sh <START_SLOT> [END_SLOT | --count N]
#
# 前提：create_instance.sh 已成功执行（实例目录、Nginx 配置、脚本都在）。
#
# 本脚本做三件事：
#   1. 确认 obd-instance@.service template 存在（由 create_instance.sh 生成）
#   2. systemctl enable obd-instance@<slot>（注册开机启动）
#   3. 验证 systemctl is-enabled 返回 enabled
#
# 开机恢复链路：
#   systemd → obd-instance@N.service → start_instance.sh N
#
# 注意：
#   - 本脚本不启动实例（只注册），如需立即启动请用 start_instance.sh
#   - 实例1（Waydroid 官方）由 cardemo.service 管理，本脚本不处理
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

SYSTEMD_TEMPLATE="/etc/systemd/system/obd-instance@.service"

# ─── systemd template（问题1修复：总是覆盖写，确保唯一来源带 EnvironmentFile）
# 无论是否已存在，都重新写入，防止 create_instance.sh 先写了不带 EnvironmentFile 的旧版本。
log_info "写入 systemd template: ${SYSTEMD_TEMPLATE}"
cat > "$SYSTEMD_TEMPLATE" << UNIT
# /etc/systemd/system/obd-instance@.service
# 由 install_persist.sh 创建
[Unit]
Description=OBD Waydroid Instance slot=%i
After=network.target
# 若 slot_1 由 cardemo.service 管理，可按需追加 After=cardemo.service

[Service]
Type=oneshot
RemainAfterExit=yes
EnvironmentFile=/etc/obd-instance.env
ExecStart=/bin/bash ${SCRIPT_DIR}/start_instance.sh %i
ExecStop=/bin/bash ${SCRIPT_DIR}/stop_instance.sh %i
TimeoutStartSec=300
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
UNIT
systemctl daemon-reload
log_info "systemd template 已写入: ${SYSTEMD_TEMPLATE}"

# ─── /etc/obd-instance.env（给 start_instance.sh 提供必填环境变量）────────
ENV_FILE="/etc/obd-instance.env"
if [[ ! -f "$ENV_FILE" ]]; then
  log_info "创建 ${ENV_FILE}（SERVER_ID / PUBLIC_WS_HOST 必须填写）..."
  cat > "$ENV_FILE" << ENV
# obd-instance 实例环境变量（必填）
# 安装后请修改以下两行为正确的值：
SERVER_ID=${SERVER_ID:-PLEASE_SET_SERVER_ID}
PUBLIC_WS_HOST=${PUBLIC_WS_HOST:-PLEASE_SET_PUBLIC_WS_HOST}
PUBLIC_WS_SCHEME=${PUBLIC_WS_SCHEME:-ws}
ENV
  log_warn "请确认 ${ENV_FILE} 中的 SERVER_ID 和 PUBLIC_WS_HOST 已填写正确值！"
fi

# ─── 逐 slot 注册 ─────────────────────────────────────────────────────────
FAILED_SLOTS=()
for s in $(seq "$SLOT_START" "$SLOT_END"); do
  derive_identity "$s" 2>/dev/null || { log_warn "slot=${s} identity 失败，跳过"; FAILED_SLOTS+=("$s"); continue; }

  # 验证实例目录存在
  if [[ ! -d "${LXC_DIR}/lxc/${LXC_NAME}" ]]; then
    log_error "slot=${s} 实例目录不存在（请先运行 create_instance.sh ${s}）"
    FAILED_SLOTS+=("$s"); continue
  fi

  # enable
  if systemctl enable "obd-instance@${s}" 2>/dev/null; then
    STATUS="$(systemctl is-enabled "obd-instance@${s}" 2>/dev/null || echo unknown)"
    log_info "slot=${s} (${INST_ID}): systemctl enable → ${STATUS}"
  else
    log_error "slot=${s} systemctl enable 失败"
    FAILED_SLOTS+=("$s")
  fi
done

systemctl daemon-reload 2>/dev/null || true

echo ""
echo "═══════════════════════════════════════════════════════════"
if [[ ${#FAILED_SLOTS[@]} -eq 0 ]]; then
  echo "  持久化注册完成，全部成功 ✓"
  echo ""
  echo "  已注册的服务（开机时自动调用 start_instance.sh）："
  for s in $(seq "$SLOT_START" "$SLOT_END"); do
    echo "    obd-instance@${s}.service"
  done
  echo ""
  echo "  如需立即启动（不重启服务器）："
  echo "    SERVER_ID=${SERVER_ID:-<SERVER_ID>} PUBLIC_WS_HOST=${PUBLIC_WS_HOST:-<HOST>} \\"
  printf "    systemctl start obd-instance@{%s..%s}\n" "$SLOT_START" "$SLOT_END"
  exit 0
else
  echo "  以下 slot 注册失败: ${FAILED_SLOTS[*]}" >&2
  exit 1
fi
