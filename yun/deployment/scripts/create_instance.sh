#!/usr/bin/env bash
# =============================================================================
# create_instance.sh  —  Waydroid 实例完整创建 + 业务验收
# =============================================================================
# 用法：./create_instance.sh <SLOT_INDEX>
#
# 覆盖三类能力：
#   A. create_runtime(identity)         —— 步骤 1–13
#   B. verify_business_readiness(identity) —— 步骤 14–18
#   C. 生成持久化所需的 Nginx conf / systemd service 文件 —— 步骤 19
#
# 注意：
#   - 本脚本不执行 enable/start systemd 服务，只生成配置文件；
#     使用 install_persist.sh <SLOT_INDEX> 完成持久化注册。
#   - slot_index=1 是 Waydroid 官方实例，本脚本拒绝操作。
#   - 所有幂等步骤可重复执行。
# =============================================================================

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${SCRIPT_DIR}/lib_instance_identity.sh"

# ─── 参数解析 ──────────────────────────────────────────────────────────────
# 用法（三种等价写法）：
#   ./create_instance.sh 2           仅创建 slot=2
#   ./create_instance.sh 2 4         创建 slot=2,3,4（从2到4）
#   ./create_instance.sh 2 --count 3 同上，从2开始创建3个
#
# slot_index=1 是 Waydroid 官方实例，本脚本拒绝操作。

_parse_args() {
  local start="" end="" count=""
  case "$#" in
    0) echo "用法: $0 <START_SLOT> [END_SLOT | --count N]" >&2; exit 1 ;;
    1) start="$1"; end="$1" ;;
    2) start="$1"; end="$2" ;;
    3) [[ "$2" == "--count" ]] || { echo "用法: $0 <START> --count N" >&2; exit 1; }
       start="$1"; count="$3"; end="$((start + count - 1))" ;;
    *) echo "参数过多" >&2; exit 1 ;;
  esac
  [[ "$start" =~ ^[2-9][0-9]*$ ]] || { echo "START_SLOT 必须 >= 2" >&2; exit 1; }
  [[ "$end" =~ ^[2-9][0-9]*$ ]]   || { echo "END_SLOT 必须 >= 2" >&2; exit 1; }
  [[ "$end" -ge "$start" ]]        || { echo "END_SLOT 必须 >= START_SLOT" >&2; exit 1; }
  SLOT_START="$start"
  SLOT_END="$end"
}

_parse_args "$@"
require_root

# ─── 批量循环（问题5：失败不吞掉，最终返回非零退出码）──────────────────
if [[ "$SLOT_START" -ne "$SLOT_END" ]]; then
  echo "═══════════════════════════════════════════════════════════"
  echo "  批量创建 slot ${SLOT_START} ~ ${SLOT_END}"
  echo "  共 $((SLOT_END - SLOT_START + 1)) 个实例"
  echo "═══════════════════════════════════════════════════════════"
  FAILED_SLOTS=()
  for s in $(seq "$SLOT_START" "$SLOT_END"); do
    echo ""
    echo "▶▶▶ 开始创建 slot=${s} ..."
    if ! bash "${BASH_SOURCE[0]}" "$s"; then
      echo "[ERROR] slot=${s} 创建失败" >&2
      FAILED_SLOTS+=("$s")
    fi
  done
  echo ""
  echo "═══════════════════════════════════════════════════════════"
  if [[ ${#FAILED_SLOTS[@]} -eq 0 ]]; then
    echo "  批量创建完成，全部成功 ✓"
    exit 0
  else
    echo "  批量创建完成，以下 slot 失败: ${FAILED_SLOTS[*]}" >&2
    exit 1
  fi
fi

SLOT="$SLOT_START"
derive_identity "$SLOT"
print_identity

# ─── 前置检查 ──────────────────────────────────────────────────────────────
log_step "前置条件检查"
[[ -f "${APK_PATH}" ]]              || die "APK 不存在: ${APK_PATH}"
[[ -c /dev/binderfs/binder-control ]] || die "binderfs 未挂载（binder_linux 模块未加载）"
is_container_running && { log_warn "容器 ${LXC_NAME} 已在运行，请先 stop_instance.sh ${SLOT}"; exit 1; } || true
[[ -d "${WAYDROID_BASE_DIR}/waydroid/images" ]] || die "找不到原始实例 images 目录，无法克隆"
log_info "前置条件通过"

# =============================================================================
# A. create_runtime
# =============================================================================

# ─── 步骤 1：创建目录结构 ─────────────────────────────────────────────────
log_step "步骤1: 创建目录结构"
mkdir -p "${LXC_DIR}/lxc/${LXC_NAME}"
mkdir -p "${OVERLAY_RW_DIR}/system" "${OVERLAY_RW_DIR}/vendor"
mkdir -p "${OVERLAY_WORK_DIR}/system" "${OVERLAY_WORK_DIR}/vendor"
mkdir -p "${LXC_DIR}/overlay/system" "${LXC_DIR}/overlay/vendor"
mkdir -p "${ROOTFS_DIR}"
mkdir -p "${HOST_PERMS_DIR}"
mkdir -p "${DATA_DIR}"
log_info "目录结构已创建"

# ─── 步骤 2：创建 binder 设备并校正权限（坑一：权限不对容器启不来）────────
log_step "步骤2: 创建 binder 设备"
create_binder_device "${BINDER_NAME}"
create_binder_device "${HWBINDER_NAME}"
create_binder_device "${VNDBINDER_NAME}"
check_binder_devices || die "binder 设备创建或权限有误"
log_info "binder 设备就绪: ${BINDER_DEV} ${HWBINDER_DEV} ${VNDBINDER_DEV}"

# ─── 步骤 3：生成 LXC config ──────────────────────────────────────────────
log_step "步骤3: 生成 LXC 配置"
BASE_LXC_DIR="${WAYDROID_BASE_DIR}/waydroid/lxc/waydroid"

# 3.1 主 config
cp "${BASE_LXC_DIR}/config" "${LXC_DIR}/lxc/${LXC_NAME}/config"
cp "${BASE_LXC_DIR}/waydroid.seccomp" "${LXC_DIR}/lxc/${LXC_NAME}/waydroid.seccomp"

sed -i "s|lxc\.rootfs\.path = .*/rootfs|lxc.rootfs.path = ${ROOTFS_DIR}|" \
    "${LXC_DIR}/lxc/${LXC_NAME}/config"
sed -i "s|lxc\.seccomp\.profile = .*/waydroid\.seccomp|lxc.seccomp.profile = ${LXC_DIR}/lxc/${LXC_NAME}/waydroid.seccomp|" \
    "${LXC_DIR}/lxc/${LXC_NAME}/config"
sed -i "s|lxc\.include = .*/config_nodes|lxc.include = ${LXC_DIR}/lxc/${LXC_NAME}/config_nodes|" \
    "${LXC_DIR}/lxc/${LXC_NAME}/config"
sed -i "s|lxc\.include = .*/config_session|lxc.include = ${LXC_DIR}/lxc/${LXC_NAME}/config_session|" \
    "${LXC_DIR}/lxc/${LXC_NAME}/config"
sed -i "s|lxc\.uts\.name = .*|lxc.uts.name = ${LXC_NAME}|" \
    "${LXC_DIR}/lxc/${LXC_NAME}/config"
sed -i "s|lxc\.net\.0\.link = .*|lxc.net.0.link = ${BRIDGE_NAME}|" \
    "${LXC_DIR}/lxc/${LXC_NAME}/config"
sed -i "s|lxc\.net\.0\.hwaddr = .*|lxc.net.0.hwaddr = ${BRIDGE_MAC}|" \
    "${LXC_DIR}/lxc/${LXC_NAME}/config"

# 3.2 config_nodes（替换 binder 设备路径，坑一的完整修复）
sed "s|/dev/anbox-binder|${BINDER_DEV}|g
     s|/dev/anbox-hwbinder|${HWBINDER_DEV}|g
     s|/dev/anbox-vndbinder|${VNDBINDER_DEV}|g
     s|${WAYDROID_BASE_DIR}/waydroid/host-permissions|${HOST_PERMS_DIR}|g" \
    "${BASE_LXC_DIR}/config_nodes" > "${LXC_DIR}/lxc/${LXC_NAME}/config_nodes"

# 3.3 config_session（绑定实例独立的 wayland socket，坑二的修复）
cat > "${LXC_DIR}/lxc/${LXC_NAME}/config_session" << SESS
lxc.mount.entry = tmpfs /run/xdg none create=dir 0 0
lxc.mount.entry = /run/user/0/${WAYLAND_DISPLAY} run/xdg/wayland-0 none rbind,create=file 0 0
lxc.mount.entry = /run/user/0/pulse/native run/xdg/pulse/native none rbind,create=file,optional 0 0
lxc.mount.entry = ${DATA_DIR} data none rbind 0 0
SESS

log_info "LXC 配置已生成"

# ─── 步骤 4：挂载 rootfs（4 层 overlay，坑五中的底层保障）─────────────────
log_step "步骤4: 挂载 rootfs"
if is_rootfs_mounted; then
  log_info "rootfs 已挂载，跳过"
else
  BASE_IMAGES="${WAYDROID_BASE_DIR}/waydroid/images"
  BASE_OVERLAY="${WAYDROID_BASE_DIR}/waydroid/overlay"

  # 层1: system.img → rootfs (ext4 ro)
  mount -o ro "${BASE_IMAGES}/system.img" "${ROOTFS_DIR}"
  # 层2: overlay (lower=shared_patch:rootfs, upper=本实例 rw 层)
  mount -t overlay overlay \
    -o "lowerdir=${BASE_OVERLAY}:${ROOTFS_DIR},upperdir=${OVERLAY_RW_DIR}/system,workdir=${OVERLAY_WORK_DIR}/system" \
    "${ROOTFS_DIR}"
  # 层3: vendor.img → rootfs/vendor (ext4 ro)
  mount -o ro "${BASE_IMAGES}/vendor.img" "${ROOTFS_DIR}/vendor"
  # 层4: overlay vendor
  mount -t overlay overlay \
    -o "lowerdir=${BASE_OVERLAY}/vendor:${ROOTFS_DIR}/vendor,upperdir=${OVERLAY_RW_DIR}/vendor,workdir=${OVERLAY_WORK_DIR}/vendor" \
    "${ROOTFS_DIR}/vendor"

  log_info "rootfs 4 层挂载完成"
fi

# ─── 步骤 5：生成 waydroid.prop（坑三：key 前缀必须是 waydroid.*）──────────
log_step "步骤5: 生成 waydroid.prop"
generate_prop_content > "${PROP_FILE}"
# 写入 bind mount（先挂载才能原地写）
if mountpoint -q "${ROOTFS_DIR}/vendor/waydroid.prop" 2>/dev/null; then
  # 已 bind mount，必须用 Python 原地写（sed -i 会破坏 inode）
  python3 -c "
import os
content = open('${PROP_FILE}').read()
fd = os.open('${ROOTFS_DIR}/vendor/waydroid.prop', os.O_WRONLY | os.O_TRUNC)
os.write(fd, content.encode())
os.close(fd)
"
  log_info "waydroid.prop 已原地更新（bind mount 安全）"
else
  mount --bind "${PROP_FILE}" "${ROOTFS_DIR}/vendor/waydroid.prop"
  log_info "waydroid.prop bind mount 完成"
fi

# ─── 步骤 6：配置网络 bridge ───────────────────────────────────────────────
log_step "步骤6: 配置网络 bridge"
if ! ip link show "${BRIDGE_NAME}" &>/dev/null; then
  ip link add "${BRIDGE_NAME}" type bridge
  log_info "bridge ${BRIDGE_NAME} 已创建"
fi
ip link set "${BRIDGE_NAME}" up
ip addr show "${BRIDGE_NAME}" | grep -q "${BRIDGE_IP}" || \
  ip addr add "${BRIDGE_ADDR}" dev "${BRIDGE_NAME}"
# iptables FORWARD 规则
iptables -C FORWARD -i "${BRIDGE_NAME}" -j ACCEPT 2>/dev/null || \
  iptables -I FORWARD -i "${BRIDGE_NAME}" -j ACCEPT
iptables -C FORWARD -o "${BRIDGE_NAME}" -j ACCEPT 2>/dev/null || \
  iptables -I FORWARD -o "${BRIDGE_NAME}" -j ACCEPT
log_info "bridge ${BRIDGE_NAME} (${BRIDGE_ADDR}) 就绪"

# ─── 步骤 7：启动 weston（为实例提供独立 wayland socket，坑二的修复）─────
log_step "步骤7: 启动 weston (${WAYLAND_DISPLAY})"
mkdir -p /run/user/0
if ! is_wayland_ready; then
  mkdir -p /run/user/0
  XDG_RUNTIME_DIR=/run/user/0 nohup weston \
    --backend=headless-backend.so \
    --no-config \
    --socket="${WAYLAND_DISPLAY}" \
    > "/tmp/weston-${WAYLAND_DISPLAY}.log" 2>&1 &
  # 等待 socket 出现（最多 15 秒）
  for i in $(seq 1 15); do
    is_wayland_ready && break
    sleep 1
    [[ $i -eq 15 ]] && die "weston ${WAYLAND_DISPLAY} 启动超时"
  done
  log_info "weston 已启动 (${WAYLAND_DISPLAY})"
else
  log_info "wayland socket ${WAYLAND_DISPLAY} 已存在"
fi

# ─── 步骤 8：准备 pulse 占位文件 ──────────────────────────────────────────
mkdir -p /run/user/0/pulse
touch /run/user/0/pulse/native

# ─── 步骤 9：启动 LXC 容器 ────────────────────────────────────────────────
log_step "步骤9: 启动 LXC 容器 ${LXC_NAME}"
lxc-start -P "${LXC_DIR}/lxc" -n "${LXC_NAME}" -- /init &
for i in $(seq 1 30); do
  is_container_running && break
  sleep 1
  [[ $i -eq 30 ]] && die "容器 ${LXC_NAME} 启动超时"
done
log_info "容器 ${LXC_NAME} 已进入 RUNNING 状态"

# ─── 步骤 10：获取容器 PID（坑四：动态探测，不硬写）─────────────────────
log_step "步骤10: 获取容器 PID"
CPID="$(get_container_pid)"
[[ -n "$CPID" ]] || die "无法获取容器 PID"
log_info "容器 init 宿主机 PID = ${CPID}"

# ─── 步骤 11：启动 socat ADB 代理（坑四：用当前 CPID，进入容器 namespace）
log_step "步骤11: 启动 socat ADB 代理 (:${ADB_PORT})"
pkill -f "socat.*${ADB_PORT}" 2>/dev/null || true
sleep 1
# 等待容器内 adbd 监听（最多 60 秒）
for i in $(seq 1 30); do
  nsenter -t "${CPID}" -n -- ss -tlnp 2>/dev/null | grep -q ":5555" && break
  sleep 2
  [[ $i -eq 30 ]] && die "容器内 adbd 未在 :5555 启动"
done
nohup socat \
  "TCP-LISTEN:${ADB_PORT},bind=127.0.0.1,reuseaddr,fork" \
  "EXEC:nsenter -t ${CPID} -n -- nc 127.0.0.1 5555" \
  >/dev/null 2>&1 &
sleep 2
log_info "socat 已启动 (localhost:${ADB_PORT} → 容器:5555)"

# ─── 步骤 12：ADB 连接 ────────────────────────────────────────────────────
log_step "步骤12: ADB 连接"
adb disconnect "${ADB_TARGET}" >/dev/null 2>&1 || true
adb connect "${ADB_TARGET}" >/dev/null 2>&1
sleep 2
is_adb_online || die "ADB 连接 ${ADB_TARGET} 失败"
log_info "ADB 已在线: ${ADB_TARGET}"

# ─── 步骤 13：等待 Android 完全启动（坑五：boot_completed 是必要非充分条件）
log_step "步骤13: 等待 Android boot_completed"
for i in $(seq 1 60); do
  BOOT="$(adb -s "${ADB_TARGET}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r\n')"
  [[ "$BOOT" == "1" ]] && break
  sleep 3
  [[ $i -eq 60 ]] && die "Android boot_completed 超时"
done
log_info "Android 启动完成 (boot_completed=1)"

# ─── 步骤 14：安装 APK ────────────────────────────────────────────────────
log_step "步骤14: 安装 APK"
INSTALLED="$(adb -s "${ADB_TARGET}" shell pm list packages 2>/dev/null | grep "${APK_PACKAGE}" || true)"
if [[ -z "$INSTALLED" ]]; then
  adb -s "${ADB_TARGET}" install -r "${APK_PATH}" 2>&1 | tail -3
  log_info "APK 安装完成"
else
  log_info "APK 已安装，跳过"
fi

# ─── 步骤 15：启动 APK ────────────────────────────────────────────────────
log_step "步骤15: 启动 APK"
adb -s "${ADB_TARGET}" shell am force-stop "${APK_PACKAGE}" >/dev/null 2>&1 || true
sleep 2
adb -s "${ADB_TARGET}" shell am start -n "${APK_PACKAGE}/${APK_ACTIVITY}" >/dev/null 2>&1
log_info "APK 已启动（首次启动可能因 ProfilesV2 崩溃后自动重启，属正常现象）"

# ─── 步骤 16：等待 APK WebSocket 就绪（坑六：101 不等于业务可用）──────────
log_step "步骤16: 等待 APK WebSocket :${APK_INTERNAL_PORT} 就绪"
for i in $(seq 1 40); do
  WS="$(adb -s "${ADB_TARGET}" shell ss -tlnp 2>/dev/null | grep ":${APK_INTERNAL_PORT}" || true)"
  [[ -n "$WS" ]] && break
  sleep 5
  [[ $i -eq 40 ]] && die "APK WebSocket 启动超时"
done
log_info "APK WebSocket 已监听 :${APK_INTERNAL_PORT}"

# ─── 步骤 17：adb forward ─────────────────────────────────────────────────
log_step "步骤17: adb forward tcp:${PROBE_PORT} → tcp:${APK_INTERNAL_PORT}"
adb -s "${ADB_TARGET}" forward "tcp:${PROBE_PORT}" "tcp:${APK_INTERNAL_PORT}" >/dev/null
log_info "adb forward 完成 (${PROBE_PORT} → 容器内 ${APK_INTERNAL_PORT})"

# =============================================================================
# B. verify_business_readiness — 三层验收（坑五/六/七 的完整修复）
# =============================================================================

# ─── 步骤 18：业务三层验收 ───────────────────────────────────────────────
log_step "步骤18: 业务验收"

# 层1：系统层
log_info "[验收] 系统层: container=RUNNING boot=1 adb=online ✓"

# 层2：通道层 — probePort + wsPort
HTTP_STATUS="$(curl -s -o /dev/null -w "%{http_code}" --max-time 5 \
  -H "Upgrade: websocket" -H "Connection: Upgrade" \
  -H "Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==" \
  -H "Sec-WebSocket-Version: 13" \
  "http://127.0.0.1:${PROBE_PORT}${WS_PATH}" 2>/dev/null || true)"
[[ -n "${HTTP_STATUS}" ]] || HTTP_STATUS="000"
[[ "$HTTP_STATUS" == "101" ]] || die "[验收] 通道层失败: probePort ${PROBE_PORT} 返回 ${HTTP_STATUS}（期望 101）"
log_info "[验收] 通道层: probe_port ${PROBE_PORT} → 101 ✓"

# 层3：业务层 — getBrands + getProfiles + applyProfile
# 通过 waitForApkReady（轮询，等待 getBrands 和默认品牌 getProfiles 都成功）
log_info "[验收] 业务层: 等待 ${DEFAULT_BRAND} catalog 就绪（最多 60 秒）..."
python3 - << PYEOF
import asyncio, websockets, json, time, sys

WS_URL = "ws://127.0.0.1:${PROBE_PORT}${WS_PATH}"
BRAND = "${DEFAULT_BRAND}"
TIMEOUT = 60

async def check():
    deadline = time.time() + TIMEOUT
    while time.time() < deadline:
        ws = None
        try:
            ws = await asyncio.wait_for(
                websockets.connect(WS_URL, ping_interval=None, open_timeout=5),
                timeout=6)
            rid = "verify_001"
            # getBrands
            await ws.send(json.dumps({"type":"request","action":"getBrands","requestId":rid,"data":{}}))
            for _ in range(20):
                raw = await asyncio.wait_for(ws.recv(), timeout=5)
                m = json.loads(raw)
                if (m.get("RequestId") or m.get("requestId")) == rid:
                    brands = m.get("Data") or m.get("data")
                    if isinstance(brands, str):
                        import json as j2; brands = j2.loads(brands)
                    if isinstance(brands, list) and len(brands) > 0:
                        break
            else:
                raise Exception("getBrands 超时或返回空")
            # getProfiles
            rid2 = "verify_002"
            await ws.send(json.dumps({"type":"request","action":"getProfiles","requestId":rid2,"data":{"brand":BRAND}}))
            for _ in range(20):
                raw = await asyncio.wait_for(ws.recv(), timeout=5)
                m = json.loads(raw)
                if (m.get("RequestId") or m.get("requestId")) == rid2:
                    profiles = m.get("Data") or m.get("data")
                    if isinstance(profiles, str):
                        import json as j2; profiles = j2.loads(profiles)
                    if isinstance(profiles, list) and len(profiles) > 0:
                        return True
                    raise Exception(f"getProfiles({BRAND}) 返回空或异常")
            raise Exception("getProfiles 超时")
        except Exception as e:
            pass
        finally:
            try:
                if ws: ws.close()
            except: pass
        await asyncio.sleep(3)
    return False

ok = asyncio.run(check())
if not ok:
    print(f"[验收] 业务层失败: {BRAND} catalog 在 {TIMEOUT}s 内未就绪", file=sys.stderr)
    sys.exit(1)
print(f"[验收] 业务层: getBrands + getProfiles({BRAND}) ✓")

# 问题3：补上 applyProfile 验收，覆盖"能连但 apply 失败"的坑
async def check_apply():
    ws_url = "ws://127.0.0.1:${PROBE_PORT}${WS_PATH}"
    ws = await asyncio.wait_for(
        websockets.connect(ws_url, ping_interval=None, open_timeout=5), timeout=6)
    try:
        rid = "apply_verify"
        # getProfiles 取 index=0
        await ws.send(json.dumps({"type":"request","action":"getProfiles","requestId":rid+"_p","data":{"brand":"${DEFAULT_BRAND}"}}))
        profiles_resp = None
        for _ in range(30):
            raw = await asyncio.wait_for(ws.recv(), timeout=5)
            m = json.loads(raw)
            if (m.get("RequestId") or m.get("requestId")) == rid+"_p":
                d = m.get("Data") or m.get("data") or []
                if isinstance(d, str): d = json.loads(d)
                profiles_resp = d; break
        if not profiles_resp:
            raise Exception("getProfiles 响应为空")
        # applyProfile index=0
        await ws.send(json.dumps({"type":"request","action":"applyProfile","requestId":rid,"data":{"brand":"${DEFAULT_BRAND}","profileIndex":0}}))
        for _ in range(30):
            raw = await asyncio.wait_for(ws.recv(), timeout=5)
            m = json.loads(raw)
            if (m.get("RequestId") or m.get("requestId")) == rid:
                ok_flag = m.get("Success") if m.get("Success") is not None else m.get("success")
                if ok_flag == False:
                    raise Exception(f"applyProfile 返回失败: {m.get('Error') or m.get('error')}")
                return True
        raise Exception("applyProfile 超时")
    finally:
        try: ws.close()
        except: pass

try:
    asyncio.run(check_apply())
    print("[验收] 业务层: applyProfile(${DEFAULT_BRAND}, 0) ✓")
except Exception as e:
    print(f"[验收] applyProfile 失败: {e}", file=sys.stderr)
    sys.exit(1)
PYEOF

log_info "[验收] 三层验收全部通过 ✓"

# =============================================================================
# C. 生成持久化配置文件
# =============================================================================

# ─── 步骤 19：生成 Nginx 配置 ─────────────────────────────────────────────
log_step "步骤19: 生成 Nginx 配置"
cat > "${NGINX_CONF}" << NGINX
# obd-inst-${SLOT_2D} — 自动生成，slot=${SLOT_INDEX}
server {
    listen ${WS_PORT};
    server_name _;

    location ${WS_PATH} {
        proxy_pass http://127.0.0.1:${PROBE_PORT};
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_connect_timeout 60s;
        proxy_send_timeout 60s;
        proxy_read_timeout 3600s;
        proxy_buffering off;
    }

    location /health {
        return 200 "OK\\n";
        add_header Content-Type text/plain;
    }
}
NGINX

# 问题8修复：Nginx 配置/重载失败是致命错误，不能只告警
nginx -t || die "Nginx 配置检查失败，请检查 ${NGINX_CONF}"
nginx -s reload || die "Nginx reload 失败，请检查 Nginx 状态"
log_info "Nginx 已配置并重载 (port ${WS_PORT} → ${PROBE_PORT})"

# 防火墙放行（问题8：失败必须明确提示，但脚本无法处理 AWS Security Group）
ensure_public_tcp_port_open "${WS_PORT}" "slot_${SLOT_INDEX} WebSocket 入口"

# ─── 步骤 20：持久化模板统一由 install_persist.sh 管理 ───────────────────
# 问题1修复：不在 create_instance.sh 中写 systemd 模板。
# create 此前写的模板不带 EnvironmentFile，与 install_persist.sh 的版本冲突，
# 导致开机恢复时缺少 SERVER_ID / PUBLIC_WS_HOST 环境变量而失败。
# 现在统一由 install_persist.sh 覆盖写唯一版本（含 EnvironmentFile）。
log_step "步骤20: systemd 持久化说明"
log_info "实例创建完成，如需开机自动恢复，运行:"
log_info "  SERVER_ID=${SERVER_ID} PUBLIC_WS_HOST=${PUBLIC_WS_HOST} ${SCRIPT_DIR}/install_persist.sh ${SLOT_INDEX}"

# ─── 步骤21：自动接入 Agent（问题2）──────────────────────────────────────
# 问题A修复：成功横幅移到 Agent 注册完成之后，防止 Step21 失败时终端已显示"成功"而误导运维
# 不只打印示例，而是实际修改 INSTANCES_CONFIG 并重启 obd-agent。
log_step "步骤21: 注册实例到 Agent"
AGENT_ENV="/opt/cardemo/agent/.env"
[[ -f "$AGENT_ENV" ]] || die "Agent 环境文件不存在: ${AGENT_ENV}"

python3 - << PYEOF
import json, re, sys, os

env_path = "${AGENT_ENV}"
inst_id = "${INST_ID}"
new_entry = {
    "id": inst_id,
    "wsPort": ${WS_PORT},
    "probePort": ${PROBE_PORT},
    "adbTarget": "${ADB_TARGET}",
    "packageName": "${APK_PACKAGE}",
    "activityName": "${APK_ACTIVITY}"
}

with open(env_path, "r") as f:
    content = f.read()

# 找 INSTANCES_CONFIG 行
m = re.search(r'^INSTANCES_CONFIG=(.+)$', content, re.MULTILINE)
if not m:
    print(f"[Agent] INSTANCES_CONFIG 未找到于 {env_path}", file=sys.stderr)
    sys.exit(1)

raw = m.group(1).strip()
try:
    instances = json.loads(raw)
except json.JSONDecodeError as e:
    print(f"[Agent] INSTANCES_CONFIG JSON 解析失败: {e}", file=sys.stderr)
    sys.exit(1)

result = "created"
updated = False
for index, item in enumerate(instances):
    if item.get("id") == inst_id:
        merged = {**item, **new_entry}
        instances[index] = merged
        updated = True
        result = "unchanged" if item == merged else "updated"
        break

if not updated:
    instances.append(new_entry)

new_line = f"INSTANCES_CONFIG={json.dumps(instances, separators=(',', ':'))}"
new_content = re.sub(r'^INSTANCES_CONFIG=.+$', new_line, content, flags=re.MULTILINE)

# 原地写（不用 sed -i 避免 inode 问题）
fd = os.open(env_path, os.O_WRONLY | os.O_TRUNC)
os.write(fd, new_content.encode())
os.close(fd)
print(f"[Agent] {inst_id} -> INSTANCES_CONFIG ({result})")
PYEOF

# 重启 obd-agent（问题3修复：失败必须 die，不能只 warn）
log_info "重启 obd-agent..."
systemctl restart obd-agent 2>/dev/null || die "obd-agent 重启失败，请检查: journalctl -u obd-agent -n 20"
log_info "obd-agent 已重启"

# 等待 Agent 探针完成并将实例标记为 idle（问题3：超时必须 die）
log_info "等待 ${INST_ID} 进入 idle 状态（最多 120 秒）..."
AGENT_FINAL_STATUS=""
for i in $(seq 1 24); do
  AGENT_FINAL_STATUS="$(curl -s --max-time 3 http://127.0.0.1:4000/health 2>/dev/null | \
    python3 -c "
import sys,json
try:
    d=json.load(sys.stdin)
    inst=[i for i in d['instances'] if i['id']=='${INST_ID}']
    print(inst[0]['status'] if inst else 'not_found')
except: print('err')
" 2>/dev/null || echo "err")"
  if [[ "$AGENT_FINAL_STATUS" == "idle" ]]; then
    log_info "Agent 已将 ${INST_ID} 标记为 idle ✓"
    break
  fi
  if [[ $i -eq 24 ]]; then
    die "等待实例进入 idle 超时（当前状态: ${AGENT_FINAL_STATUS}），实例未成功接入调度池。\n  查看详情: journalctl -u obd-agent -n 50"
  fi
  sleep 5
done

# ─── 最终汇总（问题A修复：移至 Agent 成功接入之后）────────────────────────
echo ""
echo "═══════════════════════════════════════════════════════════"
echo "  实例 ${INST_ID} 全流程完成 ✓"
echo "  · 三层业务验收通过"
echo "  · Agent 已接入，状态: idle"
echo "═══════════════════════════════════════════════════════════"
echo ""
echo "  对外 WebSocket: ${INST_WS_URL}"
echo "  本地探针端口:   127.0.0.1:${PROBE_PORT}"
echo "  ADB 入口:       ${ADB_TARGET}"
echo ""
echo "  下一步——启用开机持久化（问题B修复：使用 install_persist.sh）："
echo "    SERVER_ID=${SERVER_ID} PUBLIC_WS_HOST=${PUBLIC_WS_HOST} ${SCRIPT_DIR}/install_persist.sh ${SLOT_INDEX}"
echo ""
echo "  AWS 环境需额外手动放行安全组端口 ${WS_PORT}/tcp"
echo "═══════════════════════════════════════════════════════════"
