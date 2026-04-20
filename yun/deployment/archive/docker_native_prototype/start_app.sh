#!/usr/bin/env bash
#
# LEGACY：仅供 Docker / Android 模拟器 PoC 使用。
# 当前正式生产链路是 Waydroid 多实例脚本，不使用本文件。
# 本文件保留的唯一目的，是让 `deployment/Dockerfile` 的遗留 PoC 链路仍可启动。
#

set -euo pipefail

echo "=== OBD 云端服务启动（LEGACY Docker/模拟器链路）==="

echo "等待模拟器启动..."
adb wait-for-device
sleep 10

until adb shell getprop sys.boot_completed 2>/dev/null | tr -d '\r' | grep -q "^1$"; do
  echo "等待模拟器完全启动..."
  sleep 5
done

echo "模拟器已启动"

APK_PATH="/app/obd-app.apk"
PACKAGE="com.companyname.cardemo"

echo "安装 OBD 应用..."
if [[ -f "${APK_PATH}" ]]; then
  adb install -r "${APK_PATH}"
  echo "APK 安装完成"
else
  echo "错误: 未找到 APK 文件: ${APK_PATH}" >&2
  exit 1
fi

echo "授予应用权限..."
adb shell pm grant "${PACKAGE}" android.permission.INTERNET || true
adb shell pm grant "${PACKAGE}" android.permission.ACCESS_NETWORK_STATE || true
adb shell pm grant "${PACKAGE}" android.permission.ACCESS_WIFI_STATE || true

echo "启动 OBD 应用..."
adb shell am start -n "${PACKAGE}/.MainActivity"

echo "应用已启动，开始监控日志..."
echo "WebSocket 服务应在 Android 内部 8080 端口监听"
echo ""

while IFS= read -r line; do
  echo "[$(date +%H:%M:%S)] ${line}"
done < <(adb logcat -s CarDemo:* JSBridge:* OBDCloud:* WebSocket:*)
