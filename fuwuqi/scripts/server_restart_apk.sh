#!/usr/bin/env bash
# server_restart_apk.sh
# 运行位置：服务器
# 用途：仅重启 APK（不重启 Waydroid），秒级操作，日常最常用
#
# 实际部署说明：
#   ADB 通过 socat+nsenter 代理连接，目标为 localhost:5555
#   adb forward 使用内部端口 18080（避免与 Nginx 监听的 8080 冲突）
#
set -euo pipefail

ADB_TARGET="localhost:5555"
PACKAGE="com.companyname.cardemo"
ACTIVITY="crc64b16463db6be126c1.MainActivity"

log() { echo "[restart-apk] $(date '+%H:%M:%S') $*"; }

# 确保 ADB 已连接
log "确认 ADB 连接..."
adb connect "${ADB_TARGET}" > /dev/null 2>&1 || true

DEVICE_STATE=$(adb -s "${ADB_TARGET}" get-state 2>/dev/null | tr -d '\r' || echo "unknown")
if [ "${DEVICE_STATE}" != "device" ]; then
    echo "❌ ADB 未连接（状态：${DEVICE_STATE}）"
    echo "   请确认容器运行中：lxc-info -n waydroid -P /var/lib/waydroid/lxc/"
    echo "   如容器 FROZEN，先解冻：lxc-unfreeze -n waydroid -P /var/lib/waydroid/lxc/"
    echo "   如 socat 未运行，重启 socat 代理再试"
    exit 1
fi
log "✓ ADB 已连接"

# 强制停止 APK
log "强制停止 APK..."
adb -s "${ADB_TARGET}" shell am force-stop "${PACKAGE}"
sleep 2

# 启动 APK
log "启动 APK..."
adb -s "${ADB_TARGET}" shell am start -n "${PACKAGE}/${ACTIVITY}"
sleep 5

# 等待 WebSocket 端口就绪
log "等待 WebSocket :8080..."
for i in $(seq 1 6); do
    WS=$(timeout 3 adb -s "${ADB_TARGET}" shell ss -tlnp 2>/dev/null | grep ":8080" | head -1 || echo "")
    if [ -n "${WS}" ]; then
        log "✓ WebSocket :8080 就绪"
        break
    fi
    sleep 3
done

# 重建 adb forward（内部端口 18080，Nginx 监听 8080 并代理到 18080）
log "重建 adb forward tcp:18080 → tcp:8080..."
adb -s "${ADB_TARGET}" forward tcp:18080 tcp:8080 > /dev/null

log "✓ APK 已重启"
log "  内部地址：ws://127.0.0.1:18080/ws（adb forward 直连）"
log "  对外地址：ws://$(hostname -I | awk '{print $1}'):8080/ws（经 Nginx 代理）"
