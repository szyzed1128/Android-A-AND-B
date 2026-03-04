#!/bin/bash
#
# OBD云端服务启动脚本
# 在Android模拟器中启动OBD诊断应用
#

echo "=== OBD云端服务启动 ==="

# 等待模拟器启动
echo "等待模拟器启动..."
adb wait-for-device
sleep 10

# 检查模拟器状态
adb shell getprop sys.boot_completed | grep -q "1"
while [ $? -ne 0 ]; do
    echo "等待模拟器完全启动..."
    sleep 5
    adb shell getprop sys.boot_completed | grep -q "1"
done

echo "模拟器已启动"

# 安装APK
echo "安装OBD应用..."
APK_PATH="/app/obd-app.apk"
if [ -f "$APK_PATH" ]; then
    adb install -r "$APK_PATH"
    echo "APK安装完成"
else
    echo "错误: 未找到APK文件: $APK_PATH"
    exit 1
fi

# 授予权限
echo "授予应用权限..."
PACKAGE="com.companyname.cardemo"
adb shell pm grant $PACKAGE android.permission.INTERNET
adb shell pm grant $PACKAGE android.permission.ACCESS_NETWORK_STATE
adb shell pm grant $PACKAGE android.permission.ACCESS_WIFI_STATE

# 启动应用
echo "启动OBD应用..."
adb shell am start -n $PACKAGE/.MainActivity

# 监控日志
echo "应用已启动, 开始监控日志..."
echo "WebSocket服务器应在端口8080上监听"
echo ""

# 持续监控
adb logcat -s CarDemo:* JSBridge:* OBDCloud:* WebSocket:* | while read line; do
    echo "[$(date +%H:%M:%S)] $line"
done
