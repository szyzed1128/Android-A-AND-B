#!/usr/bin/env bash
# server_status.sh
# 运行位置：服务器
# 用途：一键检查所有组件状态
#
# 实际部署说明：
#   ADB 通过 socat+nsenter 代理连接，目标为 localhost:5555
#   adb forward 使用内部端口 18080，Nginx 代理外网 8080 → 内网 18080
#
set -uo pipefail

ADB_TARGET="localhost:5555"
PACKAGE="com.companyname.cardemo"

OK="✓"
FAIL="✗"
WARN="⚠"

echo ""
echo "=== CarII A端服务器状态检查 ==="
echo "时间：$(date '+%Y-%m-%d %H:%M:%S')"
echo ""

ALL_OK=true

# --- 1. weston headless ---
echo "[1] weston headless (Wayland 显示服务)"
if pgrep -x weston > /dev/null; then
    echo "  weston RUNNING ${OK}"
    ls /run/user/0/wayland-0 > /dev/null 2>&1 \
        && echo "  /run/user/0/wayland-0 socket ${OK}" \
        || echo "  wayland-0 socket 不存在 ${FAIL}"
else
    echo "  weston 未运行 ${FAIL}"
    ALL_OK=false
fi

# --- 2. LXC 容器状态 ---
echo ""
echo "[2] Waydroid LXC 容器"
CONTAINER_STATE=$(lxc-info -n waydroid -P /var/lib/waydroid/lxc/ 2>/dev/null | grep "State:" | awk '{print $2}' || echo "UNKNOWN")

if [ "${CONTAINER_STATE}" = "RUNNING" ]; then
    echo "  State: RUNNING ${OK}"
    CPID=$(lxc-info -n waydroid -P /var/lib/waydroid/lxc/ | grep "PID:" | awk '{print $2}' | head -1)
    echo "  PID: ${CPID}"
elif [ "${CONTAINER_STATE}" = "FROZEN" ]; then
    echo "  State: FROZEN ${FAIL}（需要解冻：lxc-unfreeze -n waydroid -P /var/lib/waydroid/lxc/）"
    ALL_OK=false
else
    echo "  State: ${CONTAINER_STATE} ${FAIL}"
    ALL_OK=false
fi

# --- 3. socat ADB 代理 ---
echo ""
echo "[3] socat ADB 代理 (localhost:5555)"
if pgrep -f "socat.*5555" > /dev/null; then
    echo "  socat 运行中 ${OK}"
else
    echo "  socat 未运行 ${FAIL}（ADB 代理未启动）"
    ALL_OK=false
fi

# --- 4. ADB 连接 ---
echo ""
echo "[4] ADB 连接"
adb connect "${ADB_TARGET}" > /dev/null 2>&1 || true
DEVICE_STATE=$(adb -s "${ADB_TARGET}" get-state 2>/dev/null | tr -d '\r' || echo "unknown")

if [ "${DEVICE_STATE}" = "device" ]; then
    echo "  ${ADB_TARGET}  device ${OK}"
else
    echo "  ${ADB_TARGET}  ${DEVICE_STATE} ${FAIL}"
    ALL_OK=false
fi

# --- 5. APK 进程 ---
echo ""
echo "[5] APK 进程"
APK_PID=$(timeout 5 adb -s "${ADB_TARGET}" shell pidof "${PACKAGE}" 2>/dev/null | tr -d '\r' || echo "")

if [ -n "${APK_PID}" ]; then
    echo "  ${PACKAGE}  PID=${APK_PID} ${OK}"
else
    echo "  ${PACKAGE}  未运行 ${FAIL}"
    ALL_OK=false
fi

# --- 6. WebSocket 端口（容器内）---
echo ""
echo "[6] WebSocket 端口（容器内 :8080）"
WS_LISTEN=$(timeout 5 adb -s "${ADB_TARGET}" shell ss -tlnp 2>/dev/null | grep ":8080" | head -1 || echo "")

if [ -n "${WS_LISTEN}" ]; then
    echo "  :8080 LISTEN ${OK}"
else
    echo "  :8080 未监听 ${FAIL}"
    echo "  （APK 可能还在初始化中，等待 5-10 秒再检查）"
    ALL_OK=false
fi

# --- 7. adb forward ---
echo ""
echo "[7] adb forward (tcp:18080 → tcp:8080)"
FORWARD=$(adb -s "${ADB_TARGET}" forward --list 2>/dev/null | grep "tcp:18080" || echo "")

if [ -n "${FORWARD}" ]; then
    echo "  ${FORWARD} ${OK}"
else
    echo "  tcp:18080 forward 未配置 ${FAIL}"
    echo "  修复：adb -s ${ADB_TARGET} forward tcp:18080 tcp:8080"
    ALL_OK=false
fi

# --- 8. Nginx ---
echo ""
echo "[8] Nginx (外网 :8080 → 内网 :18080)"
if systemctl is-active nginx > /dev/null 2>&1; then
    echo "  nginx 运行中 ${OK}"
    ss -tlnp | grep ":8080" | grep nginx > /dev/null \
        && echo "  :8080 监听 ${OK}" \
        || echo "  :8080 未监听 ${WARN}"
else
    echo "  nginx 未运行 ${FAIL}"
    ALL_OK=false
fi

# --- 9. WebSocket 可达性（从宿主机经 Nginx）---
echo ""
echo "[9] WebSocket 可达性测试（127.0.0.1:8080/ws）"
HTTP_CODE=$(curl -s -o /dev/null -w "%{http_code}" \
    --max-time 5 \
    -H "Connection: Upgrade" -H "Upgrade: websocket" \
    -H "Sec-WebSocket-Version: 13" \
    -H "Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==" \
    "http://127.0.0.1:8080/ws" 2>/dev/null || echo "000")

if [ "${HTTP_CODE}" = "101" ]; then
    echo "  HTTP 101 Switching Protocols ${OK}"
elif [ "${HTTP_CODE}" = "000" ]; then
    echo "  连接拒绝 / 超时 ${FAIL}"
    ALL_OK=false
else
    echo "  HTTP ${HTTP_CODE} ${WARN}（非标准响应，可能仍在初始化）"
fi

# --- 总结 ---
echo ""
echo "=================================="
if [ "${ALL_OK}" = "true" ]; then
    echo "✓ 状态：全部正常"
    echo "  WebSocket 地址：ws://$(hostname -I | awk '{print $1}'):8080/ws"
    echo "  B端输入：Host=$(hostname -I | awk '{print $1}')  Port=8080"
else
    echo "${FAIL} 状态：有异常，请根据上方提示排查"
    echo "  完整重启：bash /opt/cardemo/startup.sh"
    echo "  仅重启APK：bash /opt/cardemo/restart_apk.sh"
fi
echo "=================================="
echo ""
