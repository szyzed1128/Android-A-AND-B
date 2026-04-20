#!/usr/bin/env bash
# deploy_to_server.sh
# 运行位置：本地 Mac
# 用途：构建 A端 APK 并一键部署到服务器
#
# 使用前修改以下变量：
set -euo pipefail

# ============================================================
# 配置区（根据实际情况修改）
# ============================================================
SERVER_IP="121.40.198.17"
SERVER_USER="root"
SSH_KEY="~/.ssh/aliyun_hangzhou.pem"
# ============================================================

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
NEWUI_DIR="$(cd "${SCRIPT_DIR}/../.." && pwd)"
YUN_TOOLS_DIR="${NEWUI_DIR}/yun/tools"
# automator.py 固定输出此文件名（不可改），用于本地测试
APK_SRC="${NEWUI_DIR}/build_output/Release_com.companyname.cardemo-Signed.apk"
# 服务器专用副本，与本地测试 APK 区分，不会相互覆盖
APK_SERVER_SRC="${NEWUI_DIR}/Release_com.companyname.cardemo-Server-Signed.apk"
APK_REMOTE_DIR="/opt/cardemo"
APK_REMOTE_PATH="${APK_REMOTE_DIR}/Release_com.companyname.cardemo-Server-Signed.apk"

SSH_OPTS="-i ${SSH_KEY} -o StrictHostKeyChecking=no"

log() { echo "[deploy] $*"; }

# --- Step 1: Java 环境 ---
log "设置 JAVA_HOME..."
export JAVA_HOME=/opt/homebrew/Cellar/openjdk@17/17.0.18/libexec/openjdk.jdk/Contents/Home
export PATH="$JAVA_HOME/bin:$PATH"
java -version 2>&1 | head -1

# --- Step 2: 构建 WebSocket 层 + 注入 DLL ---
log "构建 WebSocket 层..."
cd "${YUN_TOOLS_DIR}"
bash build_and_inject.sh

# 验证 OBDCloud.WebSocket.dll 是占位符（约 2KB）
WS_DLL="${NEWUI_DIR}/yun/modified_dlls/OBDCloud.WebSocket.dll"
WS_SIZE=$(stat -f%z "${WS_DLL}" 2>/dev/null || stat -c%s "${WS_DLL}")
if [ "${WS_SIZE}" -gt 10240 ]; then
    echo "❌ 错误：OBDCloud.WebSocket.dll 大小为 ${WS_SIZE} bytes，超过 10KB"
    echo "   这说明 DllInjector 未正确生成占位符，请检查 build_and_inject.sh 的输出"
    exit 1
fi
log "✓ OBDCloud.WebSocket.dll 大小正常：${WS_SIZE} bytes"

# --- Step 3: 打包签名 APK ---
log "打包签名 APK..."
cd "${NEWUI_DIR}"
python3 automator.py

if [ ! -f "${APK_SRC}" ]; then
    echo "❌ 错误：未找到 APK 文件：${APK_SRC}"
    exit 1
fi
log "✓ APK 构建完成：${APK_SRC}"

# 拷贝为服务器专用副本（与本地测试 APK 文件名区分）
cp "${APK_SRC}" "${APK_SERVER_SRC}"
log "✓ 服务器副本：${APK_SERVER_SRC}"

# --- Step 4: 备份服务器上的旧 APK ---
log "备份服务器上的旧 APK..."
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
ssh ${SSH_OPTS} "${SERVER_USER}@${SERVER_IP}" \
    "mkdir -p ${APK_REMOTE_DIR}/backup && \
     [ -f '${APK_REMOTE_PATH}' ] && \
     cp '${APK_REMOTE_PATH}' '${APK_REMOTE_DIR}/backup/Release_com.companyname.cardemo-Server-Signed-${TIMESTAMP}.apk' && \
     echo 'backup done' || echo 'no old apk to backup'"

# --- Step 5: scp 传输 APK ---
log "传输 APK 到服务器..."
scp ${SSH_OPTS} "${APK_SERVER_SRC}" "${SERVER_USER}@${SERVER_IP}:${APK_REMOTE_PATH}"
log "✓ 传输完成"

# --- Step 6: 远程安装 APK ---
log "远程安装 APK..."
ssh ${SSH_OPTS} "${SERVER_USER}@${SERVER_IP}" bash << 'REMOTE_SCRIPT'
set -e
ADB_TARGET="localhost:5555"
APK_PATH="/opt/cardemo/Release_com.companyname.cardemo-Server-Signed.apk"
PACKAGE="com.companyname.cardemo"
ACTIVITY="crc64b16463db6be126c1.MainActivity"

# 安装 APK
echo "安装 APK..."
adb -s "${ADB_TARGET}" install -r "${APK_PATH}"

# 重启 APK
echo "重启 APK..."
adb -s "${ADB_TARGET}" shell am force-stop "${PACKAGE}"
sleep 2
adb -s "${ADB_TARGET}" shell am start -n "${PACKAGE}/${ACTIVITY}"
sleep 5

# 重建端口 forward
echo "重建 adb forward..."
adb -s "${ADB_TARGET}" forward tcp:18080 tcp:8080

echo "✓ 部署完成"
REMOTE_SCRIPT

log "========================================"
log "✓ 部署成功！"
log "A端已更新到服务器：${SERVER_IP}:8080/ws"
log "B端 App 设置：cloudHost = ${SERVER_IP}，port = 8080"
log "========================================"
