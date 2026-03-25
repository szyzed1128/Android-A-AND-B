# 02 Waydroid 安装与配置

> **本文档用途：新 ECS 实例首次配置的操作手册。**
> 已在 121.40.198.17（阿里云 ARM64，Ubuntu 22.04）上实际走通，所有步骤和踩坑均已验证。
> 下次购买新 ECS 做生产部署时，按本文档从头执行即可。
>
> 适用环境：**ARM64 ECS，Ubuntu 22.04 LTS**（阿里云 ARM64 裸金属/ECS 均适用）

---

## 一、前置要求确认

SSH 连接服务器后，先验证环境：

```bash
# 1. 确认架构是 ARM64
uname -m
# 期望输出：aarch64

# 2. 确认内核版本（需要 5.x+）
uname -r
# 实测：5.15.0-173-generic

# 3. 确认 binder 模块可用（Waydroid 必需）
ls /dev/binder /dev/anbox-binder 2>/dev/null
# 阿里云 ARM64 ECS 上 binder 默认可用，无需手动加载
# 如果没有，执行：modprobe binder_linux
```

---

## 二、安装 Waydroid

```bash
# 安装依赖
apt update
apt install -y curl ca-certificates lxc-utils python3-pip python3-dbus socat adb weston

# 安装 Waydroid（官方脚本）
curl -s https://repo.waydro.id | bash
apt install -y waydroid

# 初始化 Waydroid，下载 GAPPS 镜像（需要几分钟，服务器需能访问外网）
waydroid init -s GAPPS -f
```

> **说明：** `-s GAPPS` 包含 Google 服务框架，CarScanner APK 正常运行需要它。
> 初始化完成后 `/var/lib/waydroid/images/` 下会有 `system.img` 和 `vendor.img` 两个文件。

---

## 三、关键补丁（必须执行，否则后续无法正常运行）

以下三个补丁是实际部署过程中发现的坑，**必须在启动容器之前打好**。

### 3.1 修复 ro.adb.secure（否则 ADB 永远 unauthorized）

Waydroid 源码硬编码了 `ro.adb.secure=1`，必须 patch：

```bash
# 备份原文件
cp /usr/lib/waydroid/tools/helpers/lxc.py \
   /usr/lib/waydroid/tools/helpers/lxc.py.bak

# patch：关闭 ADB 授权检查，开启 debuggable
sed -i 's/props.append("ro.adb.secure=1")/props.append("ro.adb.secure=0")/' \
    /usr/lib/waydroid/tools/helpers/lxc.py
sed -i 's/props.append("ro.debuggable=0")/props.append("ro.debuggable=1")/' \
    /usr/lib/waydroid/tools/helpers/lxc.py

# 验证
grep "ro.adb.secure\|ro.debuggable" /usr/lib/waydroid/tools/helpers/lxc.py
# 期望：ro.adb.secure=0，ro.debuggable=1
```

### 3.2 禁用 suspend 自动冻结（否则容器会周期性 FROZEN）

Android 的电源管理会定期发出 suspend 事件，导致容器被 lxc-freeze 冻结，APK 停止响应：

```bash
# 备份
cp /usr/lib/waydroid/tools/services/hardware_manager.py \
   /usr/lib/waydroid/tools/services/hardware_manager.py.bak

# patch：让 suspend() 变成 no-op
python3 << 'PYEOF'
path = "/usr/lib/waydroid/tools/services/hardware_manager.py"
with open(path, "r") as f:
    content = f.read()

old = """    def suspend():
        cfg = tools.config.load(args)
        if cfg["waydroid"]["suspend_action"] == "stop":
            tools.actions.session_manager.stop(args)
        else:
            tools.actions.container_manager.freeze(args)"""

new = """    def suspend():
        logging.debug("suspend() called - headless mode: ignoring suspend request")"""

if old in content:
    content = content.replace(old, new)
    with open(path, "w") as f:
        f.write(content)
    print("patch 成功")
else:
    print("未找到目标代码，请检查 Waydroid 版本")
PYEOF
```

### 3.3 修改 waydroid.cfg（补充属性）

```bash
# 在 waydroid.cfg 的 [waydroid] 段追加 [properties] 节
# 注意：此处配置会被 lxc.py 中的硬编码覆盖，3.1 的 patch 才是真正生效的
cat >> /var/lib/waydroid/waydroid.cfg << 'EOF'

[properties]
ro.adb.secure = 0
ro.debuggable = 1
service.adb.root = 1
EOF
```

---

## 四、启动 waydroid-container 服务

Waydroid 安装后会自动注册 `waydroid-container.service`，只负责 D-Bus daemon，不会自动启动容器：

```bash
systemctl enable waydroid-container
systemctl start waydroid-container
systemctl status waydroid-container
# 期望：active (running)
```

> ⚠️ **不要用 `waydroid session start`**。
> 服务器上没有 Wayland session，这个命令会报 `WAYLAND_DISPLAY is not set` 并退出。
> 正确的启动方式见下一节（通过 D-Bus 直接调用）。

---

## 五、无头启动流程（每次服务器重启后执行）

服务器没有桌面/GPU，启动链路比普通 Waydroid 更复杂，需要按顺序执行：

### Step 1：启动 weston headless（提供虚拟 Wayland 显示）

**这是关键步骤**。Waydroid 的 hwcomposer 需要连接真实的 Wayland socket，否则 SurfaceFlinger 无法启动，Android 永远无法完成 boot（`sys.boot_completed` 永远不会变成 1）。

```bash
mkdir -p /run/user/0
XDG_RUNTIME_DIR=/run/user/0 nohup weston \
    --backend=headless-backend.so \
    --no-config \
    --socket=wayland-0 \
    > /tmp/weston.log 2>&1 &

sleep 3
# 验证：必须是 socket 类型文件
file /run/user/0/wayland-0
# 期望：/run/user/0/wayland-0: socket
```

### Step 2：启动 waydroid 网络桥

```bash
sh /usr/lib/waydroid/data/scripts/waydroid-net.sh start
# 验证
ip link show waydroid0
```

### Step 3：准备 bind mount 所需文件

```bash
mkdir -p /run/user/0/pulse
touch /run/user/0/pulse/native
mkdir -p /root/.local/share/waydroid/data
```

### Step 4：通过 D-Bus 启动容器

```bash
python3 << 'PYEOF'
import sys, os
sys.path.insert(0, "/usr/lib/waydroid")
import dbus

bus = dbus.SystemBus()
obj = bus.get_object("id.waydro.Container", "/ContainerManager")
mgr = dbus.Interface(obj, "id.waydro.ContainerManager")

session = {
    "user_name": "root", "user_id": "0", "group_id": "0",
    "host_user": "/root", "pid": str(os.getpid()),
    "xdg_data_home": "/root/.local/share",
    "xdg_runtime_dir": "/run/user/0",
    "wayland_display": "wayland-0",       # 指向 weston 创建的 socket
    "pulse_runtime_path": "/run/user/0/pulse",
    "state": "STOPPED", "lcd_density": "0",
    "background_start": "true",
    "waydroid_user_state": "/root/.local/share/waydroid",
    "waydroid_data": "/root/.local/share/waydroid/data",
}
mgr.Start(session)
print("容器启动完成")
PYEOF

# 验证容器状态
sleep 5
lxc-info -n waydroid -P /var/lib/waydroid/lxc/
# 期望：State: RUNNING
```

### Step 5：在容器网络命名空间内创建 dummy0

```bash
CPID=$(lxc-info -n waydroid -P /var/lib/waydroid/lxc/ | grep PID | awk '{print $2}' | head -1)
nsenter -t $CPID -n -- ip link add dummy0 type dummy 2>/dev/null || true
nsenter -t $CPID -n -- ip link set dummy0 up
```

### Step 6：配置 UFW 允许容器流量转发

```bash
iptables -I FORWARD -i waydroid0 -j ACCEPT
iptables -I FORWARD -o waydroid0 -j ACCEPT
```

---

## 六、ADB 连接（socat 代理方式）

> ⚠️ **不能直连 192.168.240.112:5555**。
> 容器内 netd 崩溃导致容器网络不通，需要通过 socat+nsenter 代理到宿主机端口。

```bash
CPID=$(lxc-info -n waydroid -P /var/lib/waydroid/lxc/ | grep PID | awk '{print $2}' | head -1)

# 启动 socat 代理（把宿主机 localhost:5555 转发进容器的 127.0.0.1:5555）
pkill -f "socat.*5555" 2>/dev/null || true
sleep 1
nohup socat TCP-LISTEN:5555,bind=127.0.0.1,reuseaddr,fork \
    "EXEC:nsenter -t ${CPID} -n -- nc 127.0.0.1 5555" \
    > /dev/null 2>&1 &

sleep 3

# 等待 Android 启动完成（boot_completed=1）
for i in $(seq 1 40); do
    adb connect localhost:5555 > /dev/null 2>&1 || true
    BOOT=$(timeout 3 adb -s localhost:5555 shell getprop sys.boot_completed 2>/dev/null | tr -d '\r' || echo "0")
    echo "等待 Android 启动... $i/40: boot_completed=$BOOT"
    if [ "$BOOT" = "1" ]; then
        echo "✓ Android 启动完成"
        break
    fi
    sleep 5
done
```

---

## 七、安装 APK

```bash
# 从本地 Mac 传输 APK（在本地 Mac 上执行）
scp -i ~/.ssh/your_key.pem \
    /path/to/Release_com.companyname.cardemo-Signed.apk \
    root@服务器IP:/opt/cardemo/Release_com.companyname.cardemo-Server-Signed.apk

# 在服务器上安装
mkdir -p /opt/cardemo
adb -s localhost:5555 install -r \
    /opt/cardemo/Release_com.companyname.cardemo-Server-Signed.apk

# 验证
adb -s localhost:5555 shell pm list packages | grep cardemo
# 期望：package:com.companyname.cardemo
```

---

## 八、启动 APK 并配置端口转发

```bash
PACKAGE="com.companyname.cardemo"
ACTIVITY="crc64b16463db6be126c1.MainActivity"

# 启动 APK
adb -s localhost:5555 shell am force-stop "$PACKAGE" 2>/dev/null || true
sleep 2
adb -s localhost:5555 shell am start -n "$PACKAGE/$ACTIVITY"
sleep 8

# 等待 WebSocket 端口就绪
for i in $(seq 1 12); do
    WS=$(timeout 3 adb -s localhost:5555 shell ss -tlnp 2>/dev/null | grep ":8080" | head -1 || echo "")
    [ -n "$WS" ] && echo "✓ WebSocket :8080 就绪" && break
    echo "等待... $i/12"
    sleep 5
done

# 建立 adb forward（内部端口 18080，避免与 Nginx 的 8080 冲突）
adb -s localhost:5555 forward tcp:18080 tcp:8080
adb -s localhost:5555 forward --list
```

---

## 九、配置 Nginx（对外暴露 WebSocket）

`adb forward` 只绑定 `127.0.0.1`，外网无法直接访问，必须用 Nginx 代理：

```bash
apt install -y nginx

# 创建配置（测试阶段，无 SSL）
cat > /etc/nginx/conf.d/cardemo.conf << 'EOF'
server {
    listen 8080;
    server_name _;

    location /ws {
        proxy_pass http://127.0.0.1:18080;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_read_timeout 3600s;
        proxy_send_timeout 3600s;
    }
}
EOF

# 删除默认站点避免冲突
rm -f /etc/nginx/sites-enabled/default

nginx -t && systemctl restart nginx

# 开放防火墙
ufw allow 8080/tcp
```

---

## 十、配置开机自启动

上面五到九的所有步骤都需要每次服务器重启后重新执行，因此封装成脚本和 systemd 服务：

```bash
# 将 startup.sh 放到服务器（从本地 Mac 传输）
scp -i ~/.ssh/your_key.pem \
    /path/to/fuwuqi/scripts/startup_template.sh \
    root@服务器IP:/opt/cardemo/startup.sh
chmod +x /opt/cardemo/startup.sh

# 创建 systemd 服务
cat > /etc/systemd/system/cardemo.service << 'EOF'
[Unit]
Description=CarDemo A端启动服务 (Waydroid + APK)
After=waydroid-container.service network.target
Requires=waydroid-container.service

[Service]
Type=oneshot
RemainAfterExit=yes
ExecStart=/bin/bash /opt/cardemo/startup.sh
TimeoutStartSec=300

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable cardemo.service
```

---

## 十一、验证完整链路

```bash
# 从宿主机测试 WebSocket（返回 HTTP 101 = 成功）
curl -s -o /dev/null -w "%{http_code}" --max-time 5 \
  -H "Connection: Upgrade" -H "Upgrade: websocket" \
  -H "Sec-WebSocket-Version: 13" \
  -H "Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==" \
  "http://127.0.0.1:8080/ws"
# 期望输出：101

# 查看 APK 日志
adb -s localhost:5555 logcat -v time | awk '/\[FLOW\]/ { print; fflush(); }'
```

B端输入：`Host = 服务器公网IP`，`Port = 8080`

---

## 十二、已知踩坑汇总

| 问题 | 现象 | 根因 | 解法 |
|------|------|------|------|
| ADB unauthorized | `adb -s 192.168.240.112:5555` 永远 unauthorized | `lxc.py` 硬编码 `ro.adb.secure=1` | patch lxc.py（见第三节） |
| ADB 无法直连容器 IP | `adb connect 192.168.240.112:5555` connection refused | 容器内 netd 崩溃，网络不通 | socat+nsenter 代理（见第六节） |
| boot_completed 永远不变成 1 | Android 卡在启动，SurfaceFlinger 反复崩溃 | hwcomposer 需要 Wayland 连接 | 先启动 weston headless（见第五节） |
| 容器周期性 FROZEN | APK 停止响应，adb 超时 | Android suspend 事件触发 lxc-freeze | patch hardware_manager.py（见第三节） |
| overlay build.prop → Zygote 崩溃 | Zygote 启动失败，APK 无法运行 | overlay 文件会完全替换 Android 的 build.prop | 绝对不要在 overlay 里放 build.prop |
| sed -i 破坏 bind mount | 修改 waydroid.prop 后容器看不到变化 | sed -i 创建新 inode，bind mount 指向旧 inode | 用 Python `os.open(O_WRONLY|O_TRUNC)` 原地写入 |
| adb forward 不对外暴露 | B端连不上 | adb forward 只绑 127.0.0.1 | 加 Nginx 代理（见第九节） |
