#!/usr/bin/env bash
# =============================================================================
# status_instance.sh  —  查看实例运行时状态
# =============================================================================
# 用法：
#   ./status_instance.sh            查看所有已知实例（默认自动发现）
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
EXPLICIT_RANGE=0
SLOT_START=2
SLOT_END=2

case "$#" in
  0) : ;;
  1) EXPLICIT_RANGE=1; SLOT_START="$1"; SLOT_END="$1" ;;
  2) EXPLICIT_RANGE=1; SLOT_START="$1"; SLOT_END="$2" ;;
  3) [[ "$2" == "--count" ]] || { echo "用法: $0 [START [END | --count N]]" >&2; exit 1; }
     EXPLICIT_RANGE=1; SLOT_START="$1"; SLOT_END="$(($1 + $3 - 1))" ;;
esac

require_root

collect_known_slots_from_agent() {
  local env_path="/opt/cardemo/agent/.env"
  [[ -f "${env_path}" ]] || return 0
  python3 - "$env_path" <<'PYEOF'
import json, pathlib, re, sys

env_path = pathlib.Path(sys.argv[1])
text = env_path.read_text()
m = re.search(r'^INSTANCES_CONFIG=(.+)$', text, re.MULTILINE)
if not m:
    raise SystemExit(0)

instances = json.loads(m.group(1))
slots = set()
for item in instances:
    adb_target = str(item.get("adbTarget") or "")
    mm = re.search(r':(\d+)$', adb_target)
    if not mm:
        continue
    slot = int(mm.group(1)) - 5554
    if slot >= 2:
        slots.add(slot)

for slot in sorted(slots):
    print(slot)
PYEOF
}

collect_known_slots_from_fs() {
  find /var/lib -maxdepth 1 -type d -name 'waydroid*' 2>/dev/null \
    | sed 's#/$##' \
    | while read -r path; do
        base="$(basename "${path}")"
        [[ "${base}" == "waydroid" ]] && continue
        [[ "${base}" =~ ^waydroid([0-9]+)$ ]] || continue
        echo "${BASH_REMATCH[1]}"
      done | sort -n | uniq
}

TARGET_SLOTS=()
if [[ "${EXPLICIT_RANGE}" -eq 1 ]]; then
  for s in $(seq "$SLOT_START" "$SLOT_END"); do
    TARGET_SLOTS+=("$s")
  done
else
  while IFS= read -r s; do
    [[ -n "${s}" ]] && TARGET_SLOTS+=("${s}")
  done < <(collect_known_slots_from_agent)
  if [[ ${#TARGET_SLOTS[@]} -eq 0 ]]; then
    while IFS= read -r s; do
      [[ -n "${s}" ]] && TARGET_SLOTS+=("${s}")
    done < <(collect_known_slots_from_fs)
  fi
fi

read_agent_env_value() {
  local key="$1"
  local env_path="/opt/cardemo/agent/.env"
  [[ -f "${env_path}" ]] || return 1
  grep -E "^${key}=" "${env_path}" | head -1 | cut -d= -f2-
}

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
    "http://127.0.0.1:${PROBE_PORT}${WS_PATH}" 2>/dev/null || true)"
  [[ -n "$HTTP" ]] || HTTP="---"
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

# ─── 实例1（官方 slot_1）特别显示 ───────────────────────────────────────
derive_identity 1 2>/dev/null
HTTP1="$(curl -s -o /dev/null -w "%{http_code}" --max-time 2 \
  -H "Upgrade: websocket" -H "Connection: Upgrade" \
  -H "Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==" \
  -H "Sec-WebSocket-Version: 13" \
  "http://127.0.0.1:${PROBE_PORT}${WS_PATH}" 2>/dev/null || true)"
[[ -n "${HTTP1}" ]] || HTTP1="---"
if adb devices 2>/dev/null | grep -q "^${ADB_TARGET}[[:space:]].*device$"; then
  ADB1="online"
  BOOT1="$(adb -s "${ADB_TARGET}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r\n')"
  APK1="$(adb -s "${ADB_TARGET}" shell ps 2>/dev/null | grep "${APK_PACKAGE}" | wc -l | tr -d ' ')proc"
else
  ADB1="offline"
  BOOT1="-"
  APK1="-"
fi
LXC1="$(lxc-info -P "${LXC_DIR}/lxc" -n "${LXC_NAME}" -sH 2>/dev/null || echo "?")"
HEALTH1="✓ READY"
[[ "${LXC1}" == "RUNNING" ]] || HEALTH1="✗ NO_CONTAINER"
[[ "${BOOT1}" == "1" ]] || [[ "${HEALTH1}" != "✓ READY" ]] || HEALTH1="✗ NOT_BOOTED"
[[ "${HTTP1}" == "101" ]] || [[ "${HEALTH1}" != "✓ READY" ]] || HEALTH1="✗ WS_DOWN"
printf "  %-28s  %-9s  %-7s  %-5s  %-5s  %-6s  %s\n" \
  "${INST_ID}(slot_1)" "${LXC1}" "${ADB1}" "${BOOT1}" "${HTTP1}" "${APK1}" "${HEALTH1}"

# ─── 扫描指定范围 ────────────────────────────────────────────────────────
ANY=0
for s in "${TARGET_SLOTS[@]}"; do
  derive_identity "$s" 2>/dev/null
  # 跳过不存在的实例（LXC 目录不存在）
  [[ -d "${LXC_DIR}/lxc/${LXC_NAME}" ]] || continue
  ANY=1
  _check_slot "$s"
done

if [[ "$ANY" -eq 0 ]]; then
  if [[ "${EXPLICIT_RANGE}" -eq 1 ]]; then
    echo "  (未找到已创建的实例，slot ${SLOT_START}~${SLOT_END})"
  else
    echo "  (未发现 slot_2+ 实例；Agent/.env 与 /var/lib 下均未检测到已创建实例)"
  fi
fi

echo "═══════════════════════════════════════════════════════════════════════════════"
echo ""

# ─── 远端 Scheduler 实例池状态 ─────────────────────────────────────────────
SCHEDULER_API_URL="$(read_agent_env_value SCHEDULER_URL || true)"
if [[ -n "${SCHEDULER_API_URL}" ]] && curl -s --max-time 3 "${SCHEDULER_API_URL%/}/health" &>/dev/null; then
  echo "远端 Scheduler 实例池:"
  curl -s "${SCHEDULER_API_URL%/}/instance/pool/stats" 2>/dev/null \
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
