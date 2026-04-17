#!/usr/bin/env bash
# =============================================================================
# reset_instance.sh  —  重置实例到干净就绪状态（pm clear + 重新探针）
# =============================================================================
# 用法：./reset_instance.sh <START_SLOT> [END_SLOT | --count N]
#
# 用于：
#   - 业务结束后释放实例，恢复为可分配状态
#   - 实例 bad 后手动触发恢复
#   - 不重建底层运行时（容器/rootfs 保持不变），只重置 APK 状态
#
# 执行顺序：
#   1. 确认容器和 ADB 在线（若不在线则先调 start_instance.sh）
#   2. APK pm clear（清除残留数据和状态）
#   3. APK am start（冷启动）
#   4. 等待 WebSocket 就绪
#   5. adb forward 重建
#   6. 业务验收（getBrands + getProfiles）
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
  FAILED_SLOTS=()
  for s in $(seq "$SLOT_START" "$SLOT_END"); do
    bash "${BASH_SOURCE[0]}" "$s" || { log_warn "slot=${s} reset 失败，继续"; FAILED_SLOTS+=("$s"); }
  done
  [[ ${#FAILED_SLOTS[@]} -eq 0 ]] || die "以下 slot reset 失败: ${FAILED_SLOTS[*]}"
  exit 0
fi

SLOT="$SLOT_START"
derive_identity "$SLOT"
log_info "重置实例: slot=${SLOT} / ${INST_ID}"

# ─── 确认运行时在线（若不在线则先恢复）──────────────────────────────────
if ! is_container_running || ! is_adb_online; then
  log_info "实例运行时不在线，先执行 start_instance.sh..."
  bash "${SCRIPT_DIR}/start_instance.sh" "${SLOT}"
fi

# ─── pm clear（清除 APK 所有数据，确保干净启动）─────────────────────────
log_info "pm clear ${APK_PACKAGE}..."
adb -s "${ADB_TARGET}" shell pm clear "${APK_PACKAGE}" 2>/dev/null || \
  log_warn "pm clear 失败（可能 APK 未安装），继续..."

# ─── am start（冷启动）───────────────────────────────────────────────────
log_info "am start..."
adb -s "${ADB_TARGET}" shell am start -n "${APK_PACKAGE}/${APK_ACTIVITY}" >/dev/null 2>&1

# ─── 等待 WebSocket 就绪 ─────────────────────────────────────────────────
for i in $(seq 1 40); do
  WS="$(adb -s "${ADB_TARGET}" shell ss -tlnp 2>/dev/null | grep ":${APK_INTERNAL_PORT}" || true)"
  [[ -n "$WS" ]] && break; sleep 5
  [[ $i -eq 40 ]] && die "APK WebSocket 超时"
done
log_info "APK WebSocket 就绪"

# ─── adb forward 重建 ────────────────────────────────────────────────────
adb -s "${ADB_TARGET}" forward "tcp:${PROBE_PORT}" "tcp:${APK_INTERNAL_PORT}"

# ─── 业务验收（必须验到 applyProfile，保持与 create_instance 对称）───────
log_info "验收: getBrands + getProfiles(${DEFAULT_BRAND}) + applyProfile..."
python3 - << PYEOF
import asyncio, websockets, json, time, sys

async def check():
    ws_url = "ws://127.0.0.1:${PROBE_PORT}${WS_PATH}"
    deadline = time.time() + 60
    while time.time() < deadline:
        ws = None
        try:
            ws = await asyncio.wait_for(
                websockets.connect(ws_url, ping_interval=None, open_timeout=5), timeout=6)
            # getBrands
            rid1 = "reset_brands"
            await ws.send(json.dumps({"type":"request","action":"getBrands","requestId":rid1,"data":{}}))
            brands = None
            for _ in range(20):
                raw = await asyncio.wait_for(ws.recv(), timeout=5)
                m = json.loads(raw)
                if (m.get("RequestId") or m.get("requestId")) == rid1:
                    d = m.get("Data") or m.get("data") or []
                    if isinstance(d, str): d = json.loads(d)
                    brands = d; break
            if not brands:
                raise Exception("getBrands 返回空")
            # getProfiles（问题4的关键补充）
            rid2 = "reset_profiles"
            await ws.send(json.dumps({"type":"request","action":"getProfiles","requestId":rid2,"data":{"brand":"${DEFAULT_BRAND}"}}))
            for _ in range(20):
                raw = await asyncio.wait_for(ws.recv(), timeout=5)
                m = json.loads(raw)
                if (m.get("RequestId") or m.get("requestId")) == rid2:
                    d = m.get("Data") or m.get("data") or []
                    if isinstance(d, str): d = json.loads(d)
                    if not d: raise Exception(f"getProfiles(${DEFAULT_BRAND}) 返回空")
                    return True
            raise Exception("getProfiles 超时")
        except Exception as e:
            pass
        finally:
            try:
                if ws: ws.close()
            except: pass
        await asyncio.sleep(3)
    return False

if not asyncio.run(check()):
    print("业务验收失败：getBrands 或 getProfiles 未就绪", file=sys.stderr)
    sys.exit(1)
print(f"  getBrands + getProfiles(${DEFAULT_BRAND}) ✓")

async def check_apply():
    ws_url = "ws://127.0.0.1:${PROBE_PORT}${WS_PATH}"
    ws = await asyncio.wait_for(
        websockets.connect(ws_url, ping_interval=None, open_timeout=5), timeout=6)
    try:
        rid = "reset_apply"
        await ws.send(json.dumps({"type":"request","action":"applyProfile","requestId":rid,"data":{"brand":"${DEFAULT_BRAND}","profileIndex":0}}))
        for _ in range(30):
            raw = await asyncio.wait_for(ws.recv(), timeout=5)
            m = json.loads(raw)
            if (m.get("RequestId") or m.get("requestId")) == rid:
                ok_flag = m.get("Success") if m.get("Success") is not None else m.get("success")
                if ok_flag is False:
                    raise Exception(m.get("Error") or m.get("error") or "applyProfile 返回失败")
                return True
        raise Exception("applyProfile 超时")
    finally:
        try:
            await ws.close()
        except Exception:
            pass

try:
    asyncio.run(check_apply())
except Exception as exc:
    print(f"业务验收失败：applyProfile 异常: {exc}", file=sys.stderr)
    sys.exit(1)
print(f"  applyProfile(${DEFAULT_BRAND}, 0) ✓")
PYEOF

log_info "实例 ${INST_ID} 已重置并就绪 ✓"
