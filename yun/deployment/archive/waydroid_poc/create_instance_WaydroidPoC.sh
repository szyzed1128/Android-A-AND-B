#!/usr/bin/env bash
# =============================================================================
# create_instance_WaydroidPoC.sh
# =============================================================================
# 用途：在现有 Waydroid 环境基础上克隆一个完全独立的 Android 实例（PoC 版本）
#
# 用法：
#   ./create_instance_WaydroidPoC.sh <N>
#   N = 实例编号（整数，>= 2，实例1 是原始 Waydroid，由 startup.sh 管理）
#
# 示例：
#   ./create_instance_WaydroidPoC.sh 2    # 创建实例2（今日已手工验证通过）
#   ./create_instance_WaydroidPoC.sh 3    # 创建实例3
#
# 端口规划（N >= 2）：
#   adb socat port : 5554 + N   (实例1=5555, 实例2=5556, ...)
#   probePort      : 18080 + N  (实例1=18081, 实例2=18082, ...)
#   wsPort         : 8080 + N   (实例1=8080,  实例2=8082, ...)
#   weston socket  : wayland-N
#
# 前置条件：
#   - 实例1（原始 Waydroid）已经在运行（startup.sh 已执行）
#   - /var/lib/waydroid/images/ 中有 system.img 和 vendor.img
#   - binderfs 已挂载（/dev/binderfs/binder-control 存在）
#   - ADB 已启动
#   - APK 已放置在 /opt/cardemo/ 下
#
# 已知坑（今日 2026-04-14 逐一验证）：
#   坑1 binder 设备权限：创建后默认 crw-------，必须 chmod 666
#        否则容器内非 root 进程无法使用，Android init 反复崩溃
#   坑2 waydroid.prop key 前缀：复制 prop 文件后，绝对不能把 waydroid. 前缀
#        改成 waydroidN.，vendor.waydroid.task 硬编码读 waydroid.* 系列 key，
#        改了之后 XDG_RUNTIME_DIR 读不到，hwcomposer 去 /run/user/1000 找
#        wayland socket，永远找不到，SurfaceFlinger 永远不向 servicemanager 注册
#   坑3 waydroid.prop 修改方式：bind mount 的文件必须用 Python os.open
#        O_WRONLY|O_TRUNC 原地覆盖，不能用 sed -i（会创建新 inode，
#        使 bind mount 断开，容器里看到的仍是旧内容）
#   坑4 wayland socket 路径不可见：宿主机 ls /run/user/0/ 看不到 wayland-0
#        是正常的（被第一个容器的 bind mount 消耗，文件系统路径消失，
#        但 inode 还在）。通过 cat /proc/net/unix 确认 socket 实际存在
#   坑5 rootfs overlay 必须严格按顺序挂载：
#        (1) system.img → rootfs (ext4 ro)
#        (2) overlay → rootfs (lower=overlay/:rootfs/, upper=overlay_rw/system)
#        (3) vendor.img → rootfs/vendor (ext4 ro)
#        (4) overlay → rootfs/vendor (lower=overlay/vendor:rootfs/vendor, upper=overlay_rw/vendor)
#        (5) waydroid.prop → rootfs/vendor/waydroid.prop (bind)
#   坑6 socat 依赖容器 init 的宿主机 PID（CPID），容器重启后 PID 必然改变，
#        socat 必须重建；通过 lxc-info | grep PID 获取最新 CPID
#   坑7 APK 首次启动会因 ProfilesV2 空数据目录崩溃，Android 自动重启 Activity，
#        第二次启动正常，WebSocket 正常监听 :8080
# =============================================================================

set -euo pipefail

# =============================================================================
# 参数与常量
# =============================================================================

N="${1:-}"
if [[ -z "$N" ]] || ! [[ "$N" =~ ^[2-9][0-9]*$ ]]; then
    echo "[ERROR] 用法: $0 <N>  (N 为整数，>= 2)"
    exit 1
fi

# 实例命名
INST_NAME="waydroid${N}"
INST_DIR="/var/lib/${INST_NAME}"
DATA_DIR="/root/.local/share/${INST_NAME}/data"

# binder 设备名（直接使用 binderfs 路径，不通过 symlink）
BINDER_DEV="/dev/binderfs/inst${N}-binder"
HWBINDER_DEV="/dev/binderfs/inst${N}-hwbinder"
VNDBINDER_DEV="/dev/binderfs/inst${N}-vndbinder"

# 网络
BRIDGE_NAME="waydroid-br${N}"   # 注意：避免与容器名 waydroidN 冲突
BRIDGE_IP="192.168.25${N}.1"    # N=2 → 192.168.252.1，N=3 → 192.168.253.1 etc.
# ⚠️  N >= 10 时 IP 段会出问题，PoC 阶段 N <= 9

# Wayland
WAYLAND_SOCK="wayland-${N}"

# ADB / 端口
ADB_SOCAT_PORT=$((5554 + N))    # 实例2=5556, 实例3=5557
PROBE_PORT=$((18080 + N))        # 实例2=18082, 实例3=18083
WS_PORT=$((8080 + N))            # 实例2=8082,  实例3=8083

# 原始实例路径（共享只读 images）
BASE_DIR="/var/lib/waydroid"
IMAGES_DIR="${BASE_DIR}/images"
OVERLAY_DIR="${BASE_DIR}/overlay"   # 共享 Waydroid patch 层（只读）

# APK 信息
APK_PATH="/opt/cardemo/Release_com.companyname.cardemo-Server-Signed.apk"
APK_PKG="com.companyname.cardemo"
APK_ACTIVITY="crc64b16463db6be126c1.MainActivity"

# =============================================================================
# 工具函数
# =============================================================================

log()  { echo "[$(date '+%H:%M:%S')] [INFO ] $*"; }
warn() { echo "[$(date '+%H:%M:%S')] [WARN ] $*" >&2; }
err()  { echo "[$(date '+%H:%M:%S')] [ERROR] $*" >&2; exit 1; }

check_root() {
    [[ $EUID -eq 0 ]] || err "必须以 root 运行"
}

check_prereqs() {
    log "检查前置条件..."
    [[ -f "${IMAGES_DIR}/system.img" ]] || err "system.img 不存在: ${IMAGES_DIR}/system.img"
    [[ -f "${IMAGES_DIR}/vendor.img" ]] || err "vendor.img 不存在: ${IMAGES_DIR}/vendor.img"
    [[ -c "/dev/binderfs/binder-control" ]] || err "binderfs binder-control 不存在，请确认 binder_linux 模块已加载"
    lxc-info -P "${BASE_DIR}/lxc" -n waydroid -sH 2>/dev/null | grep -q "RUNNING" || \
        err "实例1（waydroid）未在运行，请先执行 startup.sh"
    command -v adb >/dev/null 2>&1 || err "adb 未安装"
    [[ -f "$APK_PATH" ]] || err "APK 不存在: $APK_PATH"
    log "前置条件检查通过"
}

# =============================================================================
# 步骤 1：创建 binderfs 设备
# 坑1：创建后必须 chmod 666
# =============================================================================

create_binder_devices() {
    log "步骤1: 创建 binderfs 设备..."

    python3 << PYEOF
import fcntl, os, sys

# BINDER_CTL_ADD = _IOWR('b', 1, struct binderfs_device)
# sizeof(struct binderfs_device) = 256 (name) + 4 (major) + 4 (minor) = 264
BINDER_CTL_ADD = (3 << 30) | (ord('b') << 8) | 1 | (264 << 16)  # = 0xc1086201

def create_device(name):
    path = f"/dev/binderfs/{name}"
    if os.path.exists(path):
        print(f"  已存在，跳过: {path}")
        os.chmod(path, 0o666)
        return
    buf = bytearray(264)
    buf[:len(name)] = name.encode()
    try:
        fd = os.open('/dev/binderfs/binder-control', os.O_RDWR)
        fcntl.ioctl(fd, BINDER_CTL_ADD, buf)
        os.close(fd)
        # 坑1：创建后默认 crw-------，必须 chmod 666
        os.chmod(path, 0o666)
        print(f"  创建成功: {path}")
    except OSError as e:
        print(f"  创建失败: {path}: {e}", file=sys.stderr)
        sys.exit(1)

create_device("inst${N}-binder")
create_device("inst${N}-hwbinder")
create_device("inst${N}-vndbinder")
PYEOF

    log "binder 设备就绪: inst${N}-{binder,hwbinder,vndbinder}"
}

# =============================================================================
# 步骤 2：创建目录结构
# =============================================================================

create_directories() {
    log "步骤2: 创建目录结构..."
    mkdir -p "${INST_DIR}/lxc/${INST_NAME}"
    mkdir -p "${INST_DIR}/overlay_rw/system" "${INST_DIR}/overlay_rw/vendor"
    mkdir -p "${INST_DIR}/overlay_work/system" "${INST_DIR}/overlay_work/vendor"
    mkdir -p "${INST_DIR}/overlay/system" "${INST_DIR}/overlay/vendor"
    mkdir -p "${INST_DIR}/rootfs"
    mkdir -p "${INST_DIR}/host-permissions"
    mkdir -p "${DATA_DIR}"
    log "目录结构创建完成: ${INST_DIR}/"
}

# =============================================================================
# 步骤 3：复制并修改 LXC 配置
# =============================================================================

setup_lxc_config() {
    log "步骤3: 配置 LXC..."

    # 3.1 主 config（修改路径和标识）
    cp "${BASE_DIR}/lxc/waydroid/config" "${INST_DIR}/lxc/${INST_NAME}/config"
    cp "${BASE_DIR}/lxc/waydroid/waydroid.seccomp" "${INST_DIR}/lxc/${INST_NAME}/waydroid.seccomp"

    local cfg="${INST_DIR}/lxc/${INST_NAME}/config"
    sed -i "s|lxc.rootfs.path = ${BASE_DIR}/rootfs|lxc.rootfs.path = ${INST_DIR}/rootfs|" "$cfg"
    sed -i "s|lxc.seccomp.profile = ${BASE_DIR}/lxc/waydroid/waydroid.seccomp|lxc.seccomp.profile = ${INST_DIR}/lxc/${INST_NAME}/waydroid.seccomp|" "$cfg"
    sed -i "s|lxc.include = ${BASE_DIR}/lxc/waydroid/config_nodes|lxc.include = ${INST_DIR}/lxc/${INST_NAME}/config_nodes|" "$cfg"
    sed -i "s|lxc.include = ${BASE_DIR}/lxc/waydroid/config_session|lxc.include = ${INST_DIR}/lxc/${INST_NAME}/config_session|" "$cfg"
    sed -i "s|lxc.uts.name = waydroid|lxc.uts.name = ${INST_NAME}|" "$cfg"
    sed -i "s|lxc.net.0.link = waydroid0|lxc.net.0.link = ${BRIDGE_NAME}|" "$cfg"
    # MAC 地址末位加 N，避免冲突
    local old_mac
    old_mac=$(grep "lxc.net.0.hwaddr" "$cfg" | awk '{print $3}')
    local new_mac
    new_mac=$(printf '%s:%02x' "${old_mac%:*}" "$((N + 3))")  # 末位 = 03 + N
    sed -i "s|lxc.net.0.hwaddr = .*|lxc.net.0.hwaddr = ${new_mac}|" "$cfg"

    # 3.2 config_nodes（替换 binder 设备路径）
    # 实例1 用 /dev/anbox-binder（symlink），我们直接写 binderfs 路径，等价
    sed 's|/dev/anbox-binder|/dev/binderfs/inst'"${N}"'-binder|g
         s|/dev/anbox-hwbinder|/dev/binderfs/inst'"${N}"'-hwbinder|g
         s|/dev/anbox-vndbinder|/dev/binderfs/inst'"${N}"'-vndbinder|g
         s|'"${BASE_DIR}"'/host-permissions|'"${INST_DIR}"'/host-permissions|g' \
        "${BASE_DIR}/lxc/waydroid/config_nodes" > "${INST_DIR}/lxc/${INST_NAME}/config_nodes"

    # 3.3 config_session（独立 wayland socket 和 data 目录）
    cat > "${INST_DIR}/lxc/${INST_NAME}/config_session" << SESS
lxc.mount.entry = tmpfs /run/xdg none create=dir 0 0
lxc.mount.entry = /run/user/0/${WAYLAND_SOCK} run/xdg/wayland-0 none rbind,create=file 0 0
lxc.mount.entry = /run/user/0/pulse/native run/xdg/pulse/native none rbind,create=file,optional 0 0
lxc.mount.entry = ${DATA_DIR} data none rbind 0 0
SESS

    log "LXC 配置完成"
}

# =============================================================================
# 步骤 4：挂载 rootfs
# 坑5：必须严格按顺序，且 bind mount 不能卸载后重挂
# =============================================================================

mount_rootfs() {
    log "步骤4: 挂载 rootfs..."

    # 检查是否已挂载，幂等处理
    if mountpoint -q "${INST_DIR}/rootfs" 2>/dev/null; then
        log "rootfs 已挂载，跳过"
        return
    fi

    # (1) system.img → rootfs (ext4 ro)
    mount -o ro "${IMAGES_DIR}/system.img" "${INST_DIR}/rootfs"

    # (2) overlay 覆盖 rootfs
    #     lowerdir = 共享 Waydroid patch 层（只读） + rootfs
    #     upperdir = 本实例独立可写层
    mount -t overlay overlay \
        -o "lowerdir=${OVERLAY_DIR}:${INST_DIR}/rootfs,upperdir=${INST_DIR}/overlay_rw/system,workdir=${INST_DIR}/overlay_work/system" \
        "${INST_DIR}/rootfs"

    # (3) vendor.img → rootfs/vendor (ext4 ro)
    mount -o ro "${IMAGES_DIR}/vendor.img" "${INST_DIR}/rootfs/vendor"

    # (4) overlay 覆盖 vendor
    mount -t overlay overlay \
        -o "lowerdir=${OVERLAY_DIR}/vendor:${INST_DIR}/rootfs/vendor,upperdir=${INST_DIR}/overlay_rw/vendor,workdir=${INST_DIR}/overlay_work/vendor" \
        "${INST_DIR}/rootfs/vendor"

    # (5) waydroid.prop bind mount（将在步骤5生成后挂载）
    log "rootfs 挂载完成（4层 overlay）"
}

# =============================================================================
# 步骤 5：生成 waydroid.prop
# 坑2：绝对不能改 waydroid.* 前缀为 waydroidN.*
# 坑3：已 bind mount 的文件必须 Python 原地写，不能 sed -i
# =============================================================================

setup_waydroid_prop() {
    log "步骤5: 生成 waydroid.prop..."

    local src_prop="${BASE_DIR}/waydroid.prop"
    local dst_prop="${INST_DIR}/waydroid.prop"
    local mounted_prop="${INST_DIR}/rootfs/vendor/waydroid.prop"

    # 生成新 prop：只替换 value 中的实例路径，不改 key 前缀
    python3 << PYEOF
import os

with open("${src_prop}", "r") as f:
    lines = f.readlines()

result = []
for line in lines:
    line = line.rstrip('\n')
    # 坑2：只替换 value 部分中的路径，key 的 waydroid. 前缀绝对不动
    if "=" in line:
        key, _, val = line.partition("=")
        # host_data_path: 指向本实例 data 目录
        if key == "waydroid.host_data_path":
            val = "${DATA_DIR}"
        # system_ota / vendor_ota：保持原值（只读 image 共享）
        # xdg_runtime_dir, wayland_display, pulse_runtime_path：保持原值
        # 这些 key 必须保持 waydroid.* 前缀，vendor.waydroid.task 硬编码读取
        result.append(f"{key}={val}")
    else:
        result.append(line)

content = '\n'.join(result) + '\n'

# 先写独立副本
with open("${dst_prop}", "w") as f:
    f.write(content)
print(f"  写入: ${dst_prop}")
PYEOF

    # bind mount 到 rootfs/vendor/waydroid.prop
    if mountpoint -q "${mounted_prop}" 2>/dev/null; then
        # 坑3：已 bind mount，用 Python 原地写
        log "waydroid.prop 已 bind mount，原地更新..."
        python3 << PYEOF
import os

with open("${dst_prop}", "r") as f:
    content = f.read()

# O_WRONLY | O_TRUNC 原地覆盖，不改 inode（bind mount 继续有效）
fd = os.open("${mounted_prop}", os.O_WRONLY | os.O_TRUNC)
os.write(fd, content.encode())
os.close(fd)
print(f"  原地更新: ${mounted_prop}")
PYEOF
    else
        mount --bind "${dst_prop}" "${mounted_prop}"
        log "waydroid.prop bind mount 完成"
    fi
}

# =============================================================================
# 步骤 6：网络配置（bridge）
# =============================================================================

setup_network() {
    log "步骤6: 配置网络..."

    # 创建 bridge（幂等）
    ip link show "${BRIDGE_NAME}" >/dev/null 2>&1 || {
        ip link add "${BRIDGE_NAME}" type bridge
        log "bridge 已创建: ${BRIDGE_NAME}"
    }
    ip link set "${BRIDGE_NAME}" up
    ip addr show "${BRIDGE_NAME}" | grep -q "${BRIDGE_IP}" || {
        ip addr add "${BRIDGE_IP}/24" dev "${BRIDGE_NAME}"
        log "bridge IP 已配置: ${BRIDGE_IP}/24"
    }

    # iptables FORWARD 规则
    iptables -C FORWARD -i "${BRIDGE_NAME}" -j ACCEPT 2>/dev/null || \
        iptables -I FORWARD -i "${BRIDGE_NAME}" -j ACCEPT
    iptables -C FORWARD -o "${BRIDGE_NAME}" -j ACCEPT 2>/dev/null || \
        iptables -I FORWARD -o "${BRIDGE_NAME}" -j ACCEPT

    log "网络配置完成: bridge=${BRIDGE_NAME} IP=${BRIDGE_IP}"
}

# =============================================================================
# 步骤 7：启动 weston（独立 wayland socket）
# 坑4：socket 创建后在当前 shell 的 mount namespace 里不可见，但实际存在
# =============================================================================

start_weston() {
    log "步骤7: 启动 weston (${WAYLAND_SOCK})..."

    # 检查 socket 是否已存在（通过 /proc/net/unix，不通过 ls）
    if cat /proc/net/unix 2>/dev/null | grep -q "${WAYLAND_SOCK}$"; then
        log "wayland socket ${WAYLAND_SOCK} 已存在，跳过 weston 启动"
        return
    fi

    mkdir -p /run/user/0
    XDG_RUNTIME_DIR=/run/user/0 nohup weston \
        --backend=headless-backend.so \
        --no-config \
        --socket="${WAYLAND_SOCK}" \
        > "/tmp/weston-${WAYLAND_SOCK}.log" 2>&1 &
    local weston_pid=$!

    # 等待 socket 出现（最多 10 秒）
    local timeout=10
    while ! cat /proc/net/unix 2>/dev/null | grep -q "${WAYLAND_SOCK}$"; do
        sleep 1
        timeout=$((timeout - 1))
        [[ $timeout -le 0 ]] && err "weston 启动超时，socket ${WAYLAND_SOCK} 未出现"
    done

    log "weston 已启动 (PID=${weston_pid}, socket=${WAYLAND_SOCK})"
}

# =============================================================================
# 步骤 8：确保 pulse 目录存在
# =============================================================================

setup_pulse() {
    mkdir -p /run/user/0/pulse
    touch /run/user/0/pulse/native
}

# =============================================================================
# 步骤 9：启动 LXC 容器
# =============================================================================

start_container() {
    log "步骤9: 启动 LXC 容器 ${INST_NAME}..."

    # 检查是否已运行
    if lxc-info -P "${INST_DIR}/lxc" -n "${INST_NAME}" -sH 2>/dev/null | grep -q "RUNNING"; then
        log "容器 ${INST_NAME} 已在运行"
        return
    fi

    lxc-start -P "${INST_DIR}/lxc" -n "${INST_NAME}" -- /init &

    # 等待容器进入 RUNNING 状态（最多 30 秒）
    local timeout=30
    while ! lxc-info -P "${INST_DIR}/lxc" -n "${INST_NAME}" -sH 2>/dev/null | grep -q "RUNNING"; do
        sleep 1
        timeout=$((timeout - 1))
        [[ $timeout -le 0 ]] && err "容器 ${INST_NAME} 启动超时"
    done

    log "容器 ${INST_NAME} 进入 RUNNING 状态"
}

# =============================================================================
# 步骤 10：获取容器 PID 并配置 socat ADB 代理
# 坑6：CPID 是宿主机 PID，不是容器内 PID；重启后必须重新获取
# =============================================================================

setup_adb() {
    log "步骤10: 配置 socat ADB 代理 (port=${ADB_SOCAT_PORT})..."

    # 获取容器 init 的宿主机 PID
    local cpid
    cpid=$(lxc-info -P "${INST_DIR}/lxc" -n "${INST_NAME}" | grep "^PID:" | awk '{print $2}' | head -1)
    [[ -z "$cpid" ]] && err "无法获取容器 ${INST_NAME} 的 PID"
    log "容器 init 宿主机 PID: ${cpid}"

    # 等待容器内 adbd 监听（最多 60 秒）
    log "等待容器内 adbd 在 :5555 监听..."
    local timeout=60
    while ! nsenter -t "${cpid}" -n -- ss -tlnp 2>/dev/null | grep -q ":5555"; do
        sleep 2
        timeout=$((timeout - 2))
        [[ $timeout -le 0 ]] && err "adbd 启动超时"
    done
    log "adbd 已在容器内 :5555 监听"

    # 清理旧 socat
    pkill -f "socat.*${ADB_SOCAT_PORT}" 2>/dev/null || true
    sleep 1

    # 启动 socat：宿主机 ADB_SOCAT_PORT → 容器 network namespace 内的 :5555
    nohup socat \
        "TCP-LISTEN:${ADB_SOCAT_PORT},bind=127.0.0.1,reuseaddr,fork" \
        "EXEC:nsenter -t ${cpid} -n -- nc 127.0.0.1 5555" \
        > /dev/null 2>&1 &

    sleep 2

    # 连接 ADB
    adb disconnect "localhost:${ADB_SOCAT_PORT}" >/dev/null 2>&1 || true
    adb connect "localhost:${ADB_SOCAT_PORT}" >/dev/null 2>&1
    sleep 1

    # 验证连接
    if ! adb devices | grep -q "localhost:${ADB_SOCAT_PORT}.*device"; then
        err "ADB 连接 localhost:${ADB_SOCAT_PORT} 失败"
    fi
    log "ADB 已连接: localhost:${ADB_SOCAT_PORT}"
}

# =============================================================================
# 步骤 11：等待 Android 完全启动
# =============================================================================

wait_boot_completed() {
    log "步骤11: 等待 Android boot_completed..."
    local timeout=120
    while true; do
        local boot
        boot=$(adb -s "localhost:${ADB_SOCAT_PORT}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r\n')
        if [[ "$boot" == "1" ]]; then
            log "Android 启动完成 (boot_completed=1)"
            return
        fi
        sleep 3
        timeout=$((timeout - 3))
        [[ $timeout -le 0 ]] && err "Android 启动超时（boot_completed 一直为空）"
    done
}

# =============================================================================
# 步骤 12：安装并启动 APK
# 坑7：首次启动可能因 ProfilesV2 空数据目录崩溃，Android 会自动重启 Activity
# =============================================================================

setup_apk() {
    log "步骤12: 安装并启动 APK..."

    # 安装（幂等）
    local installed
    installed=$(adb -s "localhost:${ADB_SOCAT_PORT}" shell pm list packages 2>/dev/null | grep "${APK_PKG}" || true)
    if [[ -z "$installed" ]]; then
        log "正在安装 APK..."
        adb -s "localhost:${ADB_SOCAT_PORT}" install -r "${APK_PATH}" 2>&1 | tail -3
    else
        log "APK 已安装，跳过"
    fi

    # 强制停止（确保干净启动）
    adb -s "localhost:${ADB_SOCAT_PORT}" shell am force-stop "${APK_PKG}" 2>/dev/null || true
    sleep 2

    # 启动
    adb -s "localhost:${ADB_SOCAT_PORT}" shell am start -n "${APK_PKG}/${APK_ACTIVITY}" 2>/dev/null
    log "APK 已启动（首次启动可能崩溃并自动重启，属正常现象）"
}

# =============================================================================
# 步骤 13：adb forward + 等待 WebSocket 就绪
# =============================================================================

wait_websocket() {
    log "步骤13: 等待 APK WebSocket :8080 就绪..."

    local timeout=120
    while true; do
        local ws
        ws=$(adb -s "localhost:${ADB_SOCAT_PORT}" shell ss -tlnp 2>/dev/null | grep ":8080" || true)
        if [[ -n "$ws" ]]; then
            log "WebSocket :8080 已就绪"
            break
        fi
        sleep 5
        timeout=$((timeout - 5))
        [[ $timeout -le 0 ]] && err "WebSocket :8080 启动超时"
    done

    # adb forward
    adb -s "localhost:${ADB_SOCAT_PORT}" forward "tcp:${PROBE_PORT}" tcp:8080
    log "adb forward tcp:${PROBE_PORT} → tcp:8080 完成"
}

# =============================================================================
# 步骤 14：最终验证
# =============================================================================

verify() {
    log "步骤14: 最终验证..."

    # WebSocket 握手测试（HTTP 101）
    local status
    status=$(curl -s -o /dev/null -w "%{http_code}" --max-time 5 \
        -H "Upgrade: websocket" \
        -H "Connection: Upgrade" \
        -H "Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==" \
        -H "Sec-WebSocket-Version: 13" \
        "http://127.0.0.1:${PROBE_PORT}/ws" 2>/dev/null || echo "000")

    if [[ "$status" == "101" ]]; then
        log "WebSocket 握手验证通过 (HTTP ${status})"
    else
        warn "WebSocket 握手返回 ${status}（预期 101），可能 APK 还在重启中"
    fi
}

# =============================================================================
# 输出实例信息
# =============================================================================

print_summary() {
    echo ""
    echo "======================================================================"
    echo "  实例 ${N} 创建完成"
    echo "======================================================================"
    echo "  容器名称  : ${INST_NAME}"
    echo "  工作目录  : ${INST_DIR}"
    echo "  Data 目录 : ${DATA_DIR}"
    echo ""
    echo "  binder    : /dev/binderfs/inst${N}-{binder,hwbinder,vndbinder}"
    echo "  wayland   : ${WAYLAND_SOCK}"
    echo "  bridge    : ${BRIDGE_NAME} (${BRIDGE_IP}/24)"
    echo ""
    echo "  ADB socat port : localhost:${ADB_SOCAT_PORT}"
    echo "  probe port     : 127.0.0.1:${PROBE_PORT}  (adb forward)"
    echo "  ws port        : ${WS_PORT} (需要 Nginx 配置，见下方)"
    echo ""
    echo "  Nginx 配置（需手动添加到 /etc/nginx/conf.d/）："
    echo "    server {"
    echo "        listen ${WS_PORT};"
    echo "        location /ws {"
    echo "            proxy_pass http://127.0.0.1:${PROBE_PORT};"
    echo "            proxy_http_version 1.1;"
    echo "            proxy_set_header Upgrade \$http_upgrade;"
    echo "            proxy_set_header Connection \"upgrade\";"
    echo "            proxy_read_timeout 3600s;"
    echo "        }"
    echo "    }"
    echo ""
    echo "  Agent INSTANCES_CONFIG 追加条目："
    echo "    {"
    echo "      \"id\":\"server_01_inst_${N}\","
    echo "      \"wsPort\":${WS_PORT},"
    echo "      \"probePort\":${PROBE_PORT},"
    echo "      \"adbTarget\":\"localhost:${ADB_SOCAT_PORT}\","
    echo "      \"packageName\":\"${APK_PKG}\","
    echo "      \"activityName\":\"${APK_ACTIVITY}\""
    echo "    }"
    echo "======================================================================"
    echo ""
    echo "  WebSocket 快速验证："
    echo "    curl -s -o /dev/null -w \"%{http_code}\" --max-time 3 \\"
    echo "      -H \"Upgrade: websocket\" -H \"Connection: Upgrade\" \\"
    echo "      -H \"Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==\" \\"
    echo "      -H \"Sec-WebSocket-Version: 13\" \\"
    echo "      http://127.0.0.1:${PROBE_PORT}/ws"
    echo "    # 预期返回: 101"
    echo "======================================================================"
    echo ""
    echo "  重要：此实例在服务器重启后需要重建（临时状态）"
    echo "  待办：编写 start_instance.sh 实现持久化启动"
    echo "======================================================================"
}

# =============================================================================
# 主流程
# =============================================================================

main() {
    echo "======================================================================"
    echo "  create_instance_WaydroidPoC.sh  N=${N}"
    echo "  $(date)"
    echo "======================================================================"

    check_root
    check_prereqs
    create_binder_devices   # 步骤1
    create_directories      # 步骤2
    setup_lxc_config        # 步骤3
    mount_rootfs            # 步骤4
    setup_waydroid_prop     # 步骤5
    setup_network           # 步骤6
    start_weston            # 步骤7
    setup_pulse             # 步骤8
    start_container         # 步骤9
    setup_adb               # 步骤10
    wait_boot_completed     # 步骤11
    setup_apk               # 步骤12
    wait_websocket          # 步骤13
    verify                  # 步骤14
    print_summary
}

main "$@"
