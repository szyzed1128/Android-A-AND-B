#!/usr/bin/env bash
# server_start.sh
# 运行位置：服务器
# 用途：启动 Waydroid session + APK + 建立 adb forward
#       服务器重启后，或 Waydroid session 异常退出后运行
#
set -euo pipefail

ADB_TARGET="192.168.240.112:5555"
PACKAGE="com.companyname.cardemo"
ACTIVITY="crc64b16463db6be126c1.MainActivity"
APK_PATH="/opt/cardemo/Release_com.companyname.cardemo-Server-Signed.apk"

log() { echo "[start] $(date '+%H:%M:%S') $*"; }

# --- Step 1: 启动 Waydroid session ---
log "检查 Waydroid session 状态..."
SESSION_STATUS=$(waydroid status 2>/dev/null | grep "Session:" | awk '{print $2}' || echo "STOPPED")
CONTAINER_STATUS=$(waydroid status 2>/dev/null | grep "Container:" | awk '{print $2}' || echo "STOPPED")

if [ "${SESSION_STATUS}" != "RUNNING" ] || [ "${CONTAINER_STATUS}" != "RUNNING" ]; then
    log "启动 Waydroid session..."
    waydroid session start &
    WAYDROID_PID=$!

    log "等待 Waydroid 完全启动（最多 120 秒）..."
    for i in $(seq 1 24); do
        sleep 5
        S=$(waydroid status 2>/dev/null | grep "Session:" | awk '{print $2}' || echo "")
        C=$(waydroid status 2>/dev/null | grep "Container:" | awk '{print $2}' || echo "")
        if [ "${S}" = "RUNNING" ] && [ "${C}" = "RUNNING" ]; then
            log "✓ Waydroid 已启动"
            break
        fi
        echo "  等待中... ${i}/24 (Session=${S} Container=${C})"
    done
else
    log "✓ Waydroid 已在运行中"
fi

# 等待 Android 系统完全启动
log "等待 Android 系统 boot_completed..."
adb connect "${ADB_TARGET}" || true
sleep 5

for i in $(seq 1 20); do
    BOOT=$(adb -s "${ADB_TARGET}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r' || echo "0")
    if [ "${BOOT}" = "1" ]; then
        log "✓ Android 系统启动完成"
        break
    fi
    echo "  等待 Android 启动... ${i}/20"
    sleep 3
done

# --- Step 2: 安装 APK（如果还未安装）---
log "检查 APK 是否已安装..."
INSTALLED=$(adb -s "${ADB_TARGET}" shell pm list packages 2>/dev/null | grep "${PACKAGE}" || echo "")
if [ -z "${INSTALLED}" ]; then
    if [ -f "${APK_PATH}" ]; then
        log "安装 APK..."
        adb -s "${ADB_TARGET}" install -r "${APK_PATH}"
        log "✓ APK 安装完成"
    else
        echo "⚠️  APK 未安装，且未找到 APK 文件：${APK_PATH}"
        echo "   请先从本地 Mac 运行 fuwuqi/scripts/deploy_to_server.sh"
        exit 1
    fi
else
    log "✓ APK 已安装"
fi

# --- Step 3: 启动 APK ---
log "启动 APK..."
adb -s "${ADB_TARGET}" shell am force-stop "${PACKAGE}" 2>/dev/null || true
sleep 2
adb -s "${ADB_TARGET}" shell am start -n "${PACKAGE}/${ACTIVITY}"
log "✓ APK 已启动"

# --- Step 4: 等待 WebSocket 服务就绪 ---
log "等待 WebSocket 服务就绪（最多 30 秒）..."
sleep 5

for i in $(seq 1 6); do
    WS_STATUS=$(adb -s "${ADB_TARGET}" shell ss -tlnp 2>/dev/null | grep ":8080" | head -1 || echo "")
    if [ -n "${WS_STATUS}" ]; then
        log "✓ WebSocket 端口 8080 已监听"
        break
    fi
    echo "  等待 WebSocket 启动... ${i}/6"
    sleep 5
done

# --- Step 5: 建立 adb forward ---
log "建立 adb forward tcp:8080 → Waydroid:8080..."
adb -s "${ADB_TARGET}" forward tcp:8080 tcp:8080
log "✓ 端口 forward 已建立"

# --- Step 6: 验证 WebSocket 可达性 ---
log "验证 WebSocket 可达性..."
WS_TEST=$(curl -s -o /dev/null -w "%{http_code}" \
    --max-time 5 \
    -i -N -H "Connection: Upgrade" -H "Upgrade: websocket" \
    -H "Sec-WebSocket-Version: 13" \
    -H "Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==" \
    "http://127.0.0.1:8080/ws" 2>/dev/null || echo "000")

if [ "${WS_TEST}" = "101" ]; then
    log "✓ WebSocket 响应正常（101 Switching Protocols）"
else
    echo "⚠️  WebSocket 测试返回：${WS_TEST}（不是 101，但 APK 可能仍在初始化）"
fi

log "=========================================="
log "✓ A端启动完成！"
log "  Waydroid 容器：192.168.240.112"
log "  WebSocket：ws://$(hostname -I | awk '{print $1}'):8080/ws"
log "  B端设置 cloudHost = $(hostname -I | awk '{print $1}')，port = 8080"
log "=========================================="
