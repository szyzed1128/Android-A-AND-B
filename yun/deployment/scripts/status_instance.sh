#!/usr/bin/env bash
# =============================================================================
# status_instance.sh  —  查看实例运行时状态
# =============================================================================
# 用法：
#   ./status_instance.sh            查看所有已知实例（slot 2 开始扫描）
#   ./status_instance.sh 2          只看 slot=2
#   ./status_instance.sh 2 4        看 slot=2,3,4
#   ./status_instance.sh 2 --count 3  同上
#
# 输出三层状态（对应文档的验收三层）：
#   系统层：容器运行 / ADB 在线 / boot_completed
#   通道层：probePort / wsPort (HTTP 101)
#   业务层：APK 进程 / WebSocket 端口
# =============================================================================

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${SCRIPT_DIR}/lib_instance_identity.sh"

# ─── 参数解析 ──────────────────────────────────────────────────────────────
SLOT_START=2
SLOT_END=9  # 默认扫描 slot 2~9

case "$#" in
  0) : ;;  # 使用默认范围
  1) SLOT_START="$1"; SLOT_END="$1" ;;
  2) SLOT_START="$1"; SLOT_END="$2" ;;
  3) [[ "$2" == "--count" ]] || { echo "用法: $0 [START [END | --count N]]" >&2; exit 1; }
     SLOT_START="$1"; SLOT_END="$(($1 + $3 - 1))" ;;
esac

require_root

# ─── 状态检查函数 ────────────────────────────────────────────────────────
_check_slot() {
  local slot="$1"
  derive_identity "$slot" 2>/dev/null

  local col_id col_lxc col_adb col_boot col_ws col_apk col_url
  col_id="${INST_ID}"

  # 系统层
  if is_container_running 2>/dev/null; then
    col_lxc="RUNNING"
  else
    col_lxc="STOPPED"
  fi

  if is_adb_online 2>/dev/null; then
    col_adb="online"
  else
    col_adb="offline"
  fi

  if [[ "$col_adb" == "online" ]]; then
    BOOT="$(adb -s "${ADB_TARGET}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r\n')"
    col_boot="${BOOT:-?}"
  else
    col_boot="-"
  fi

  # 通道层
  HTTP="$(curl -s -o /dev/null -w "%{http_code}" --max-time 2 \
    -H "Upgrade: websocket" -H "Connection: Upgrade" \
    -H "Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==" \
    -H "Sec-WebSocket-Version: 13" \
    "http://127.0.0.1:${PROBE_PORT}${WS_PATH}" 2>/dev/null || echo "---")"
  col_ws="${HTTP}"

  # 业务层：APK 进程
  if [[ "$col_adb" == "online" ]]; then
    APK_PROC="$(adb -s "${ADB_TARGET}" shell ps 2>/dev/null | grep "${APK_PACKAGE}" | wc -l | tr -d ' ')"
    col_apk="${APK_PROC}proc"
  else
    col_apk="-"
  fi

  # 整体健康
  local health="✓ READY"
  [[ "$col_lxc" == "RUNNING" ]]    || health="✗ NO_CONTAINER"
  [[ "$col_boot" == "1" ]]         || [[ "$health" != "✓ READY" ]] || health="✗ NOT_BOOTED"
  [[ "$col_ws" == "101" ]]         || [[ "$health" != "✓ READY" ]] || health="✗ WS_DOWN"

  printf "  %-28s  %-9s  %-7s  %-5s  %-5s  %-6s  %s\n" \
    "$col_id" "$col_lxc" "$col_adb" "$col_boot" "$col_ws" "$col_apk" "$health"
}

# ─── 输出表头 ────────────────────────────────────────────────────────────
echo ""
echo "═══════════════════════════════════════════════════════════════════════════════"
printf "  %-28s  %-9s  %-7s  %-5s  %-5s  %-6s  %s\n" \
  "INSTANCE_ID" "CONTAINER" "ADB" "BOOT" "WS" "APK" "HEALTH"
echo "───────────────────────────────────────────────────────────────────────────────"

# ─── 实例1（官方 Waydroid）特别显示 ─────────────────────────────────────
derive_identity 1 2>/dev/null
HTTP1="$(curl -s -o /dev/null -w "%{http_code}" --max-time 2 \
  -H "Upgrade: websocket" -H "Connection: Upgrade" \
  -H "Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==" \
  -H "Sec-WebSocket-Version: 13" \
  "http://127.0.0.1:${PROBE_PORT}${WS_PATH}" 2>/dev/null || echo "---")"
ADB1="$(adb devices 2>/dev/null | grep -c "device$" || echo "0")"
LXC1="$(lxc-info -P "${LXC_DIR}/lxc" -n "${LXC_NAME}" -sH 2>/dev/null || echo "?")"
printf "  %-28s  %-9s  %-7s  %-5s  %-5s  %-6s  %s\n" \
  "${INST_ID}(官方)" "${LXC1}" "${ADB1}dev" "?" "${HTTP1}" "-" "(官方管理)"

# ─── 扫描指定范围 ────────────────────────────────────────────────────────
ANY=0
for s in $(seq "$SLOT_START" "$SLOT_END"); do
  derive_identity "$s" 2>/dev/null
  # 跳过不存在的实例（LXC 目录不存在）
  [[ -d "${LXC_DIR}/lxc/${LXC_NAME}" ]] || continue
  ANY=1
  _check_slot "$s"
done

[[ "$ANY" -eq 1 ]] || echo "  (未找到已创建的实例，slot ${SLOT_START}~${SLOT_END})"

echo "═══════════════════════════════════════════════════════════════════════════════"
echo ""

# ─── Scheduler 实例池状态 ────────────────────────────────────────────────
if curl -s --max-time 2 http://127.0.0.1:3000/health &>/dev/null; then
  echo "Scheduler 实例池:"
  curl -s http://127.0.0.1:3000/instance/pool/stats 2>/dev/null \
    | python3 -c "import sys,json;d=json.load(sys.stdin)['data'];print('  ' + ' '.join(f'{k}={v}' for k,v in d.items()))"
  echo ""
fi

# ─── Agent 实例状态 ──────────────────────────────────────────────────────
if curl -s --max-time 2 http://127.0.0.1:4000/health &>/dev/null; then
  echo "Agent 实例状态:"
  curl -s http://127.0.0.1:4000/health 2>/dev/null \
    | python3 -c "
import sys,json
d=json.load(sys.stdin)
for i in d.get('instances',[]):
    err = ('  err='+i['lastError'][:60]) if i.get('lastError') else ''
    print(f\"  {i['id']:30s}  status={i['status']:<10s}  failures={i.get('failureCount',0)}{err}\")
"
  echo ""
fi
