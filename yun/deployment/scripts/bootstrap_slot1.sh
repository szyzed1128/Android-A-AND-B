#!/usr/bin/env bash
# =============================================================================
# bootstrap_slot1.sh  —  新 EC2 上初始化官方 slot_1（Waydroid 基线实例）
# =============================================================================
# 用法：
#   export SERVER_ID=ec2-apne1-a01 PUBLIC_WS_HOST=1.2.3.4
#   export PUBLIC_WS_SCHEME=ws   # 可选，默认 ws
#   ./bootstrap_slot1.sh
#
# 作用：
#   - 执行 waydroid init（若尚未初始化）
#   - 准备 slot_1 所需 binder 设备 / symlink
#   - 写 weston.service / cardemo.service / startup.sh
#   - 配置 slot_1 的 Nginx 端口（默认 8081 → 18081）
#   - 启动 slot_1，并等待 Agent 将其实例状态收敛为 idle
#
# 注意：
#   - 本脚本面向“新 EC2 从零部署”路径
#   - 如果当前机器已经是旧式单实例 8080 生产机，请勿直接执行
# =============================================================================

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${SCRIPT_DIR}/lib_instance_identity.sh"

require_root
_require_env SERVER_ID
_require_env PUBLIC_WS_HOST
: "${PUBLIC_WS_SCHEME:=ws}"

derive_identity 1

DEPLOY_ROOT="/opt/cardemo"
STARTUP_SH="${DEPLOY_ROOT}/startup.sh"
LEGACY_NGINX_CONF="/etc/nginx/conf.d/cardemo.conf"

detect_waydroid_net_script() {
  local candidates=(
    "/usr/lib/waydroid/data/scripts/waydroid-net.sh"
    "/usr/libexec/waydroid/waydroid-net.sh"
  )
  for path in "${candidates[@]}"; do
    if [[ -f "${path}" ]]; then
      echo "${path}"
      return 0
    fi
  done
  if command -v waydroid-net.sh >/dev/null 2>&1; then
    command -v waydroid-net.sh
    return 0
  fi
  return 1
}

ensure_slot1_binder_links() {
  log_step "步骤1: 准备 slot_1 binder 设备"
  [[ -c /dev/binderfs/binder-control ]] || die "binderfs 未就绪，请先执行 bootstrap_host.sh"

  for dev in "${BINDER_NAME}" "${HWBINDER_NAME}" "${VNDBINDER_NAME}"; do
    create_binder_device "${dev}"
  done

  [[ -e /dev/anbox-binder && ! -L /dev/anbox-binder && ! -c /dev/anbox-binder ]] && die "/dev/anbox-binder 已存在且不是字符设备/符号链接"
  [[ -e /dev/anbox-hwbinder && ! -L /dev/anbox-hwbinder && ! -c /dev/anbox-hwbinder ]] && die "/dev/anbox-hwbinder 已存在且不是字符设备/符号链接"
  [[ -e /dev/anbox-vndbinder && ! -L /dev/anbox-vndbinder && ! -c /dev/anbox-vndbinder ]] && die "/dev/anbox-vndbinder 已存在且不是字符设备/符号链接"

  [[ -c /dev/anbox-binder ]] || ln -sfn "/dev/binderfs/${BINDER_NAME}" /dev/anbox-binder
  [[ -c /dev/anbox-hwbinder ]] || ln -sfn "/dev/binderfs/${HWBINDER_NAME}" /dev/anbox-hwbinder
  [[ -c /dev/anbox-vndbinder ]] || ln -sfn "/dev/binderfs/${VNDBINDER_NAME}" /dev/anbox-vndbinder

  log_info "slot_1 binder 设备就绪"
}

write_waydroid_container_service_if_missing() {
  if systemctl cat waydroid-container.service >/dev/null 2>&1; then
    log_info "waydroid-container.service 已存在，沿用官方服务"
    return 0
  fi

  log_warn "waydroid-container.service 不存在，写入兼容版本"
  cat > /etc/systemd/system/waydroid-container.service <<'UNIT'
[Unit]
Description=Waydroid Container Service
After=network.target obd-binderfs.service
Requires=obd-binderfs.service

[Service]
Type=simple
Environment=XDG_RUNTIME_DIR=/run/user/0
ExecStart=/usr/bin/python3 /usr/bin/waydroid container start
ExecStop=/usr/bin/python3 /usr/bin/waydroid container stop
Restart=on-failure
RestartSec=5

[Install]
WantedBy=multi-user.target
UNIT
}

merge_slot1_into_agent_env() {
  local env_path="${DEPLOY_ROOT}/agent/.env"
  [[ -f "${env_path}" ]] || die "Agent 环境文件不存在：${env_path}，请先执行 bootstrap_host.sh"
  python3 - "$env_path" "$INST_ID" "$WS_PORT" "$PROBE_PORT" "$ADB_TARGET" "$APK_PACKAGE" "$APK_ACTIVITY" <<'PYEOF'
import json
import pathlib
import re
import sys

env_path = pathlib.Path(sys.argv[1])
inst_id = sys.argv[2]
ws_port = int(sys.argv[3])
probe_port = int(sys.argv[4])
adb_target = sys.argv[5]
package_name = sys.argv[6]
activity_name = sys.argv[7]

content = env_path.read_text()
m = re.search(r'^INSTANCES_CONFIG=(.+)$', content, re.MULTILINE)
if not m:
    print("INSTANCES_CONFIG 缺失", file=sys.stderr)
    sys.exit(1)

instances = json.loads(m.group(1))
slot1 = {
    "id": inst_id,
    "wsPort": ws_port,
    "probePort": probe_port,
    "adbTarget": adb_target,
    "packageName": package_name,
    "activityName": activity_name,
}

updated = False
for idx, item in enumerate(instances):
    if item.get("id") == inst_id:
        instances[idx] = {**item, **slot1}
        updated = True
        break
if not updated:
    instances.insert(0, slot1)

new_line = "INSTANCES_CONFIG=" + json.dumps(instances, separators=(",", ":"))
content = re.sub(r'^INSTANCES_CONFIG=.+$', new_line, content, flags=re.MULTILINE)
env_path.write_text(content)
print("Agent slot_1 身份已写回")
PYEOF
}

read_agent_env_value() {
  local key="$1"
  local env_path="${DEPLOY_ROOT}/agent/.env"
  [[ -f "${env_path}" ]] || die "Agent 环境文件不存在：${env_path}"
  grep -E "^${key}=" "${env_path}" | head -1 | cut -d= -f2-
}

log_step "bootstrap_slot1: 初始化官方 slot_1"
log_info "INST_ID=${INST_ID}  wsPort=${WS_PORT}  probePort=${PROBE_PORT}  adbTarget=${ADB_TARGET}"

[[ -d "${DEPLOY_ROOT}/agent" ]] || die "未检测到 /opt/cardemo/agent，请先执行 bootstrap_host.sh"
[[ -f "${APK_PATH}" ]] || die "APK 不存在：${APK_PATH}"
command -v waydroid >/dev/null 2>&1 || die "Waydroid 未安装，请先执行 bootstrap_host.sh"
python3 -c 'import dbus, websockets' >/dev/null 2>&1 || die "缺少 python3-dbus / python3-websockets，请先执行 bootstrap_host.sh"

if [[ -f "${LEGACY_NGINX_CONF}" && ! -f "${NGINX_CONF}" ]]; then
  die "检测到旧式单实例 Nginx 配置 ${LEGACY_NGINX_CONF}。本脚本面向新 EC2，不应直接套到旧生产机。"
fi

WAYDROID_NET_SH="$(detect_waydroid_net_script)" || die "未找到 waydroid-net.sh"
ensure_slot1_binder_links

log_step "步骤2: 执行 waydroid init（如需要）"
if [[ ! -d /var/lib/waydroid/images || ! -f /var/lib/waydroid/waydroid.prop ]]; then
  waydroid init -s GAPPS -f
  log_info "waydroid init 完成"
else
  log_info "Waydroid 已初始化，跳过"
fi

write_waydroid_container_service_if_missing

log_step "步骤3: 写 weston.service"
cat > /etc/systemd/system/weston.service <<EOF
[Unit]
Description=Headless Weston for Waydroid slot_1
After=network.target

[Service]
Type=simple
Environment=XDG_RUNTIME_DIR=/run/user/0
ExecStartPre=/bin/mkdir -p /run/user/0
ExecStart=/usr/bin/weston --backend=headless-backend.so --no-config --socket=${WAYLAND_DISPLAY}
Restart=always
RestartSec=2
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
EOF

log_step "步骤4: 写 /opt/cardemo/startup.sh"
cat > "${STARTUP_SH}" <<EOF
#!/usr/bin/env bash
set -euo pipefail

ADB_TARGET="${ADB_TARGET}"
PROBE_PORT="${PROBE_PORT}"
PACKAGE="${APK_PACKAGE}"
ACTIVITY="${APK_ACTIVITY}"
APK_PATH="${APK_PATH}"
LXC_NAME="${LXC_NAME}"
LXC_BASE="${LXC_DIR}/lxc"
BRIDGE_NAME="${BRIDGE_NAME}"
WAYLAND_DISPLAY="${WAYLAND_DISPLAY}"
WAYDROID_NET_SH="${WAYDROID_NET_SH}"
DEFAULT_BRAND="${DEFAULT_BRAND}"

log() { echo "[slot1] \$(date '+%H:%M:%S') \$*"; }
die() { echo "[slot1] ERROR: \$*" >&2; exit 1; }

is_wayland_ready() {
  grep -q "\${WAYLAND_DISPLAY}\$" /proc/net/unix 2>/dev/null
}

container_running() {
  lxc-info -P "\${LXC_BASE}" -n "\${LXC_NAME}" -sH 2>/dev/null | grep -q "RUNNING"
}

get_cpid() {
  lxc-info -P "\${LXC_BASE}" -n "\${LXC_NAME}" 2>/dev/null | grep '^PID:' | awk '{print \$2}' | head -1
}

mkdir -p /run/user/0 /run/user/0/pulse
touch /run/user/0/pulse/native
mkdir -p /root/.local/share/waydroid/data

systemctl is-active weston.service >/dev/null 2>&1 || systemctl start weston.service
for i in \$(seq 1 15); do
  is_wayland_ready && break
  sleep 1
  [[ \$i -eq 15 ]] && die "weston socket \${WAYLAND_DISPLAY} 未就绪"
done
log "weston 已就绪"

sh "\${WAYDROID_NET_SH}" start >/dev/null 2>&1 || die "waydroid-net.sh start 失败"
ip link show "\${BRIDGE_NAME}" >/dev/null 2>&1 || die "waydroid bridge \${BRIDGE_NAME} 不存在"
ip -o link show "\${BRIDGE_NAME}" | grep -q 'UP' || die "waydroid bridge \${BRIDGE_NAME} 未处于 UP 状态"
ip -4 addr show "\${BRIDGE_NAME}" | grep -q 'inet ' || die "waydroid bridge \${BRIDGE_NAME} 没有 IPv4 地址"
log "waydroid 网络已就绪"

systemctl is-active waydroid-container.service >/dev/null 2>&1 || systemctl start waydroid-container.service

dbus_start_container() {
  python3 - <<'PYEOF'
import os
import sys
sys.path.insert(0, "/usr/lib/waydroid")
import dbus

session = {
    "user_name": "root",
    "user_id": "0",
    "group_id": "0",
    "host_user": "/root",
    "pid": str(os.getpid()),
    "xdg_data_home": "/root/.local/share",
    "xdg_runtime_dir": "/run/user/0",
    "wayland_display": "${WAYLAND_DISPLAY}",
    "pulse_runtime_path": "/run/user/0/pulse",
    "state": "STOPPED",
    "lcd_density": "0",
    "background_start": "true",
    "waydroid_user_state": "/root/.local/share/waydroid",
    "waydroid_data": "/root/.local/share/waydroid/data",
}

try:
    bus = dbus.SystemBus()
    obj = bus.get_object("id.waydro.Container", "/ContainerManager")
    mgr = dbus.Interface(obj, "id.waydro.ContainerManager")
    mgr.Start(session)
except Exception as exc:
    print(f"D-Bus Start(session) 失败: {exc}", file=sys.stderr)
    sys.exit(1)
PYEOF
}

if ! container_running; then
  for i in \$(seq 1 12); do
    if dbus_start_container; then
      log "D-Bus Start(session) 已提交，attempt=\${i}"
      break
    fi
    sleep 5
    [[ \$i -eq 12 ]] && die "D-Bus 容器启动连续失败，ContainerManager 可能未就绪"
    log "D-Bus ContainerManager 未就绪，5 秒后重试..."
  done
fi

for i in \$(seq 1 30); do
  container_running && break
  sleep 1
  [[ \$i -eq 30 ]] && die "Waydroid 容器启动超时"
done
log "Waydroid 容器已运行"

CPID="\$(get_cpid)"
[[ -n "\${CPID}" ]] || die "无法获取容器 PID"
nsenter -t "\${CPID}" -n -- ip link add dummy0 type dummy 2>/dev/null || true
nsenter -t "\${CPID}" -n -- ip link set dummy0 up
iptables -C FORWARD -i "\${BRIDGE_NAME}" -j ACCEPT 2>/dev/null || iptables -I FORWARD -i "\${BRIDGE_NAME}" -j ACCEPT
iptables -C FORWARD -o "\${BRIDGE_NAME}" -j ACCEPT 2>/dev/null || iptables -I FORWARD -o "\${BRIDGE_NAME}" -j ACCEPT
log "容器网络补丁已完成"

for i in \$(seq 1 30); do
  nsenter -t "\${CPID}" -n -- ss -tlnp 2>/dev/null | grep -q ':5555' && break
  sleep 2
  [[ \$i -eq 30 ]] && die "容器内 adbd 未监听 5555"
done
pkill -f 'socat.*5555' 2>/dev/null || true
sleep 1
nohup socat \
  "TCP-LISTEN:5555,bind=127.0.0.1,reuseaddr,fork" \
  "EXEC:nsenter -t \${CPID} -n -- nc 127.0.0.1 5555" \
  >/dev/null 2>&1 &
sleep 2
log "socat ADB 代理已启动"

adb disconnect "\${ADB_TARGET}" >/dev/null 2>&1 || true
adb connect "\${ADB_TARGET}" >/dev/null 2>&1 || true
adb devices | grep -q "^\${ADB_TARGET}[[:space:]].*device\$" || die "ADB 未在线：\${ADB_TARGET}"
log "ADB 已连接"

for i in \$(seq 1 60); do
  BOOT="\$(adb -s "\${ADB_TARGET}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\\r\\n')"
  [[ "\${BOOT}" == "1" ]] && break
  sleep 3
  [[ \$i -eq 60 ]] && die "Android boot_completed 超时"
done
log "Android boot_completed=1"

if ! adb -s "\${ADB_TARGET}" shell pm list packages 2>/dev/null | grep -q "\${PACKAGE}"; then
  adb -s "\${ADB_TARGET}" install -r "\${APK_PATH}" 2>&1 | tail -3
  log "APK 安装完成"
else
  log "APK 已安装"
fi

adb -s "\${ADB_TARGET}" shell am force-stop "\${PACKAGE}" >/dev/null 2>&1 || true
sleep 2
adb -s "\${ADB_TARGET}" shell am start -n "\${PACKAGE}/\${ACTIVITY}" >/dev/null 2>&1
log "APK 已启动"

for i in \$(seq 1 40); do
  WS="\$(adb -s "\${ADB_TARGET}" shell ss -tlnp 2>/dev/null | grep ':8080' || true)"
  [[ -n "\${WS}" ]] && break
  sleep 5
  [[ \$i -eq 40 ]] && die "APK WebSocket :8080 启动超时"
done
log "APK WebSocket 已就绪"

adb -s "\${ADB_TARGET}" forward --remove "tcp:\${PROBE_PORT}" >/dev/null 2>&1 || true
adb -s "\${ADB_TARGET}" forward "tcp:\${PROBE_PORT}" tcp:8080 >/dev/null
log "adb forward tcp:\${PROBE_PORT} → tcp:8080 完成"

HTTP_STATUS="\$(curl -s -o /dev/null -w '%{http_code}' --max-time 5 \
  -H 'Upgrade: websocket' -H 'Connection: Upgrade' \
  -H 'Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==' \
  -H 'Sec-WebSocket-Version: 13' \
  "http://127.0.0.1:\${PROBE_PORT}/ws" 2>/dev/null || true)"
[[ -n "\${HTTP_STATUS}" ]] || HTTP_STATUS="000"
[[ "\${HTTP_STATUS}" == "101" ]] || die "WebSocket 握手失败：\${HTTP_STATUS}"
log "通道层验证通过（101）"

python3 - "\${PROBE_PORT}" "\${DEFAULT_BRAND}" <<'PYEOF'
import asyncio
import json
import sys
import time
import websockets

probe_port = sys.argv[1]
brand = sys.argv[2]
ws_url = f"ws://127.0.0.1:{probe_port}/ws"

async def wait_catalog():
    deadline = time.time() + 60
    while time.time() < deadline:
        ws = None
        try:
            ws = await asyncio.wait_for(
                websockets.connect(ws_url, ping_interval=None, open_timeout=5),
                timeout=6,
            )
            await ws.send(json.dumps({"type": "request", "action": "getBrands", "requestId": "brands", "data": {}}))
            for _ in range(20):
                raw = await asyncio.wait_for(ws.recv(), timeout=5)
                message = json.loads(raw)
                if (message.get("RequestId") or message.get("requestId")) == "brands":
                    data = message.get("Data") or message.get("data")
                    if isinstance(data, str):
                        data = json.loads(data)
                    if not isinstance(data, list) or not data:
                        raise RuntimeError("getBrands 返回空")
                    break
            else:
                raise RuntimeError("getBrands 超时")

            await ws.send(json.dumps({"type": "request", "action": "getProfiles", "requestId": "profiles", "data": {"brand": brand}}))
            for _ in range(20):
                raw = await asyncio.wait_for(ws.recv(), timeout=5)
                message = json.loads(raw)
                if (message.get("RequestId") or message.get("requestId")) == "profiles":
                    data = message.get("Data") or message.get("data")
                    if isinstance(data, str):
                        data = json.loads(data)
                    if not isinstance(data, list) or not data:
                        raise RuntimeError("getProfiles 返回空")
                    return
            raise RuntimeError("getProfiles 超时")
        except Exception:
            await asyncio.sleep(3)
        finally:
            if ws is not None:
                try:
                    await ws.close()
                except Exception:
                    pass
    raise SystemExit(f"业务验收失败：{brand} catalog 60s 内未就绪")

async def apply_default():
    ws = await asyncio.wait_for(
        websockets.connect(ws_url, ping_interval=None, open_timeout=5),
        timeout=6,
    )
    try:
        await ws.send(json.dumps({"type": "request", "action": "applyProfile", "requestId": "apply", "data": {"brand": brand, "profileIndex": 0}}))
        for _ in range(30):
            raw = await asyncio.wait_for(ws.recv(), timeout=5)
            message = json.loads(raw)
            if (message.get("RequestId") or message.get("requestId")) == "apply":
                success = message.get("Success") if message.get("Success") is not None else message.get("success")
                if success is False:
                    raise SystemExit(f"applyProfile 失败: {message.get('Error') or message.get('error')}")
                return
        raise SystemExit("applyProfile 超时")
    finally:
        try:
            await ws.close()
        except Exception:
            pass

asyncio.run(wait_catalog())
asyncio.run(apply_default())
print("业务层验证通过")
PYEOF

log "slot_1 启动链路完成"
EOF
chmod +x "${STARTUP_SH}"

log_step "步骤5: 写 cardemo.service"
cat > /etc/systemd/system/cardemo.service <<EOF
[Unit]
Description=CarDemo slot_1 bootstrap service
After=network.target weston.service waydroid-container.service
Requires=weston.service waydroid-container.service
Before=obd-agent.service

[Service]
Type=oneshot
RemainAfterExit=yes
ExecStart=/bin/bash ${STARTUP_SH}
TimeoutStartSec=600
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
EOF

log_step "步骤6: 写 slot_1 Nginx 配置"
cat > "${NGINX_CONF}" <<EOF
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
        return 200 "OK\n";
        add_header Content-Type text/plain;
    }
}
EOF

nginx -t || die "Nginx 配置检查失败：${NGINX_CONF}"
nginx -s reload >/dev/null 2>&1 || systemctl restart nginx || die "nginx reload 失败"
ensure_public_tcp_port_open "${WS_PORT}" "slot_1 WebSocket 入口"

log_step "步骤7: 对齐 Agent 中的 slot_1 身份"
merge_slot1_into_agent_env

log_step "步骤8: 启动 systemd 服务"
systemctl daemon-reload
systemctl enable waydroid-container.service >/dev/null 2>&1 || die "enable waydroid-container.service 失败"
systemctl enable weston.service >/dev/null 2>&1 || die "enable weston.service 失败"
systemctl enable cardemo.service >/dev/null 2>&1 || die "enable cardemo.service 失败"

systemctl restart waydroid-container.service >/dev/null 2>&1 || die "waydroid-container.service 启动失败"
systemctl restart weston.service >/dev/null 2>&1 || die "weston.service 启动失败"
systemctl restart cardemo.service >/dev/null 2>&1 || die "cardemo.service 启动失败"

systemctl restart obd-agent >/dev/null 2>&1 || die "obd-agent 重启失败"
log_info "slot_1 相关服务已启动"

log_step "步骤9: 等待 Agent 将 slot_1 收敛为 idle"
SCHEDULER_API_URL="$(read_agent_env_value SCHEDULER_URL)"
[[ -n "${SCHEDULER_API_URL}" ]] || die "Agent .env 中缺少 SCHEDULER_URL"
SCHEDULER_DASHBOARD_INSTANCES_URL="$(python3 - "$SCHEDULER_API_URL" <<'PYEOF'
import sys
url = sys.argv[1].rstrip("/")
if url.endswith("/api"):
    print(url[:-4] + "/dashboard/api/instances")
elif url.endswith("/api/"):
    print(url[:-5] + "/dashboard/api/instances")
else:
    print(url + "/dashboard/api/instances")
PYEOF
)"

curl -fsS --max-time 5 "${SCHEDULER_API_URL%/}/health" >/dev/null || die "远端调度层不可用：${SCHEDULER_API_URL%/}/health"

python3 - "$INST_ID" "$SCHEDULER_DASHBOARD_INSTANCES_URL" <<'PYEOF'
import json
import sys
import time
from urllib.request import urlopen

inst_id = sys.argv[1]
dashboard_instances_url = sys.argv[2]
deadline = time.time() + 120
last_status = None

while time.time() < deadline:
    try:
        with urlopen(dashboard_instances_url, timeout=5) as resp:
            payload = json.loads(resp.read().decode())
        rows = payload.get("data") or []
        row = next((item for item in rows if item.get("id") == inst_id), None)
        if row:
            last_status = row.get("status")
            agent_status = row.get("agentStatus")
            if last_status == "idle":
                print(f"实例 {inst_id} 已 idle")
                sys.exit(0)
            print(f"等待中: status={last_status} agentStatus={agent_status}")
    except Exception:
        pass
    time.sleep(5)

raise SystemExit(f"实例 {inst_id} 在 120s 内未进入 idle，最后状态={last_status}")
PYEOF

log_step "验收: slot_1"
systemctl is-active weston.service >/dev/null 2>&1 || die "weston.service 未运行"
systemctl is-active waydroid-container.service >/dev/null 2>&1 || die "waydroid-container.service 未运行"
systemctl is-active cardemo.service >/dev/null 2>&1 || die "cardemo.service 未处于 active"
adb devices | grep -q "^${ADB_TARGET}[[:space:]].*device$" || die "ADB 设备未在线: ${ADB_TARGET}"
curl -s --max-time 5 "http://127.0.0.1:${WS_PORT}/health" >/dev/null || die "slot_1 Nginx 健康检查失败"

echo ""
echo "═══════════════════════════════════════════════════════════"
echo "  bootstrap_slot1 完成 ✓"
echo "  slot_1:"
echo "    INST_ID   = ${INST_ID}"
echo "    wsPort    = ${WS_PORT}"
echo "    probePort = ${PROBE_PORT}"
echo "    adbTarget = ${ADB_TARGET}"
echo "  下一步:"
echo "    SERVER_ID=${SERVER_ID} PUBLIC_WS_HOST=${PUBLIC_WS_HOST} SCHEDULER_URL=${SCHEDULER_API_URL} ${DEPLOY_ROOT}/scripts/create_instance.sh 2"
echo "═══════════════════════════════════════════════════════════"
