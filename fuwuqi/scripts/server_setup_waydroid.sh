#!/usr/bin/env bash
# server_setup_waydroid.sh
# 运行位置：服务器（ARM64 Ubuntu 22.04）
# 用途：首次安装 Waydroid 及所有依赖，仅运行一次
#
set -euo pipefail

log() { echo "[setup] $*"; }

# 检查是否是 ARM64
ARCH=$(uname -m)
if [ "${ARCH}" != "aarch64" ]; then
    echo "❌ 错误：此脚本只能在 ARM64 (aarch64) 服务器上运行"
    echo "   当前架构：${ARCH}"
    exit 1
fi
log "✓ 架构确认：${ARCH}"

# --- Step 1: 系统依赖 ---
log "安装系统依赖..."
apt update
apt install -y curl ca-certificates lxc-utils python3-pip adb nginx

# --- Step 2: 确认 binder 内核模块 ---
log "检查 binder 内核模块..."
if [ ! -e /dev/binder ]; then
    log "加载 binder 模块..."
    modprobe binder_linux || modprobe binder
    sleep 2
fi

if [ ! -e /dev/binder ]; then
    echo "⚠️  警告：/dev/binder 不存在"
    echo "   Waydroid 需要 Linux binder 支持"
    echo "   请确认内核已编译 binder 模块：grep CONFIG_ANDROID_BINDER_IPC /boot/config-\$(uname -r)"
    echo "   如果没有，可能需要安装自定义内核"
fi

# --- Step 3: 安装 Waydroid ---
log "安装 Waydroid..."
if ! command -v waydroid &>/dev/null; then
    curl -s https://repo.waydro.id | bash
    apt install -y waydroid
    log "✓ Waydroid 已安装"
else
    log "✓ Waydroid 已存在，跳过安装"
fi

# --- Step 4: 初始化 Waydroid ---
log "初始化 Waydroid（首次下载 Android 镜像，可能需要几分钟）..."
if [ ! -d /var/lib/waydroid/images ]; then
    waydroid init -s GAPPS -f
    log "✓ Waydroid 初始化完成"
else
    log "✓ Waydroid 已初始化，跳过"
fi

# --- Step 5: 配置 systemd 服务 ---
log "配置 waydroid-session systemd 服务..."
cat > /etc/systemd/system/waydroid-session.service << 'EOF'
[Unit]
Description=Waydroid Android Container Session
After=network.target

[Service]
Type=simple
ExecStart=/usr/bin/waydroid session start
ExecStop=/usr/bin/waydroid session stop
Restart=on-failure
RestartSec=10
User=root
Environment=XDG_RUNTIME_DIR=/run/user/0

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable waydroid-session
log "✓ systemd 服务已配置并启用"

# --- Step 6: 创建目录结构 ---
log "创建目录结构..."
mkdir -p /opt/cardemo/backup
mkdir -p /opt/cardemo/scripts
log "✓ 目录创建完成：/opt/cardemo/"

# --- Step 7: 启动 Waydroid ---
log "启动 Waydroid session..."
systemctl start waydroid-session

log "等待 Waydroid 启动（最多 120 秒）..."
for i in $(seq 1 24); do
    STATUS=$(waydroid status 2>/dev/null | grep "Session:" | awk '{print $2}')
    if [ "${STATUS}" = "RUNNING" ]; then
        log "✓ Waydroid session 已启动"
        break
    fi
    echo "  等待中... ${i}/24"
    sleep 5
done

# --- Step 8: 连接 ADB ---
log "连接 ADB..."
adb connect 192.168.240.112:5555 || true
sleep 3

DEVICES=$(adb devices | grep "192.168.240.112")
if echo "${DEVICES}" | grep -q "device"; then
    log "✓ ADB 连接成功"
else
    echo "⚠️  ADB 连接失败，请手动运行：adb connect 192.168.240.112:5555"
fi

log "=========================================="
log "✓ 服务器初始化完成！"
log ""
log "下一步："
log "  1. 将 APK 上传到 /opt/cardemo/Release_com.companyname.cardemo-Signed.apk"
log "     （或从本地 Mac 运行 fuwuqi/scripts/deploy_to_server.sh）"
log "  2. 运行 bash /opt/cardemo/scripts/server_start.sh"
log "=========================================="
