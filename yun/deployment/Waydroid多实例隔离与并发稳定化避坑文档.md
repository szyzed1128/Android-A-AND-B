# Waydroid多实例隔离与并发稳定化避坑文档

## 1. 文档目的

这份文档不是讲“理想架构”，而是记录这次真实落地过程中：

- 从“假多实例”到“真隔离实例”的完整修复链路
- 中间实际改过什么
- 为什么会踩这些坑
- 哪些地方是单实例默认假设，到了多实例一定会出错
- 未来在**新 EC2** 上批量化创建 `inst_01 ~ inst_N` 时，脚本必须如何参数化，才能避免再次出错

这份文档的目标读者是后续要执行自动化的 agent（例如 Claude Code / Codex / 其他脚本生成器）。

---

## 2. 本次阶段最终达成的结果

这次阶段最终不是“端口看起来不同”，而是实现了**真正可并发运行的两个隔离实例**。

已经验证过的事实包括：

- `waydroid` 与 `waydroid2` 是两个独立 LXC 容器
- 两个实例拥有不同的 binder 设备
- 两个实例拥有不同的 `/data`，文件写入互不影响
- 两个实例拥有不同的 ADB 入口：`5555` / `5556`
- 两个实例拥有不同的业务探针端口：`18081` / `18082`
- 两个实例拥有不同的对外 WebSocket 入口：`8080` / `8082`
- 两个实例中的 APK 进程彼此独立
- 双实机并发测试已成功，说明问题最终不在客户端平台，而在实例 2 的运行时和 readiness 判定

也就是说，本次修复的核心不是“客户端修好”，而是：

> **把实例 2 从“看起来像实例”修成了“真的可以被调度、被初始化、被业务使用的独立实例”。**

---

## 3. 这次真实经历的时间线

## 3.1 第一阶段：发现原先的“多实例”是假的

最早表面现象是：

- Agent 配了两个实例
- `adbTarget` 不同
- `probePort` 不同
- `wsPort` 不同

但实际运行里，两条链路都映射到了同一个 Waydroid / 同一个 APK 进程。

典型假象是：

- `127.0.0.1:5555`
- `127.0.0.1:5556`

表面看是两个设备，实际可能只是两个不同的 ADB / socat / forward 入口，最终仍指向同一底层容器。

### 根因

**端口不同不等于实例不同。**

如果没有以下资源一起隔离：

- 独立 LXC 容器
- 独立 binder 设备
- 独立 Wayland socket
- 独立 `/data`
- 独立 ADB 入口
- 独立 APK 进程

那么“多实例”只是端口分流，不是真正的运行时隔离。

---

## 3.2 第二阶段：手工做出真正隔离的实例 2

为了把 `inst_2` 做成真隔离实例，这次实际做过的事情包括：

### A. 克隆第二套 LXC 运行目录

- 新建 `/var/lib/waydroid2/`
- 新建 `/var/lib/waydroid2/lxc/waydroid2/`
- 复制第一实例的 LXC 配置作为模板
- 把 rootfs、host-permissions、data、config_session、config_nodes 等路径全部改成实例 2 自己的路径

### B. 创建第二套 binder 设备

在 binderfs 下新增：

- `inst2-binder`
- `inst2-hwbinder`
- `inst2-vndbinder`

并把实例 2 的 `config_nodes` 从：

- `/dev/anbox-binder`
- `/dev/anbox-hwbinder`
- `/dev/anbox-vndbinder`

改为挂载实例 2 自己的 binder 设备。

### C. 创建第二个 headless Weston / Wayland socket

实例 1 默认用的是：

- `wayland-0`

实例 2 如果继续复用 `wayland-0`，就不是完整隔离，而且还会和实例 1 抢同一个显示 socket。

因此必须额外启动：

- `wayland-1`

并让实例 2 的 `config_session` 明确绑定 `wayland-1`。

### D. 创建第二套网络入口

为实例 2 增加：

- 独立 bridge
- 独立 ADB 入口
- 独立 socat 代理

这里最终采用的是：

- 容器内 adbd 仍监听 `5555`
- 宿主机用 `socat + nsenter` 把容器 2 暴露到 `127.0.0.1:5556`

这一步的关键不是“端口换一个”，而是：

> `socat` 必须进入实例 2 的**网络 namespace**，而不是连接到实例 1。

### E. 创建第二条业务链路

实例 2 还需要自己独立的：

- `adb forward tcp:18082 tcp:8080`
- Nginx 对外暴露 `8082 -> 18082`
- 防火墙放行 `8082/tcp`

这样双客户端申请实例时，第二个客户端拿到的才是**独立实例 2 的业务入口**。

---

## 3.3 第三阶段：实例 2 看起来“活着”，但仍然不能并发使用

做到上一步后，会出现一种非常迷惑的状态：

- `lxc-info` 显示 `RUNNING`
- `adb devices` 里能看到 `localhost:5556`
- `boot_completed` 可能已经是 `1`
- `curl ws://127.0.0.1:18082/ws` 甚至能拿到 `101`

但真实并发测试时：

- 调度到 `inst_1` 成功
- 调度到 `inst_2` 失败
- Dashboard 状态会从 `reserved` 变成 `bad`
- 过一段时间实例又回到 `idle`

### 根因

这是**多实例稳定性问题**，不是“有无实例”问题。

也就是说：

- 实例 2 已经是真实例
- 但它还没有达到“可稳定被业务分配”的程度

---

## 3.4 第四阶段：为并发稳定化做的关键修复

真正让双实例并发跑成功的，不只是“做出实例 2”，而是下面这些修复。

---

## 4. 本次阶段实际做过的关键修改

## 4.1 持久化修改：Waydroid 系统行为修正

这些改动不是实例 2 独有，而是当前环境为了让整个方案能跑通必须保留的基础修正。

### 修改 1：禁用 suspend 导致的冻结

文件：

- `/usr/lib/waydroid/tools/services/hardware_manager.py`

修改内容：

- `suspend()` 改成 no-op

原因：

- 原始逻辑会触发 `lxc-freeze`
- 多实例实验阶段一旦被冻结，很难判断是容器挂了、显示挂了、还是调度挂了
- 这会极大干扰实例就绪判定和并发排障

### 修改 2：放开 ADB

文件：

- `/usr/lib/waydroid/tools/helpers/lxc.py`

修改内容：

- `ro.adb.secure=0`
- `ro.debuggable=1`

原因：

- 否则容器内 ADB 需要额外授权
- 自动化脚本无法稳定完成 `adb connect / adb forward / install / am start / dumpsys`

---

## 4.2 运行时修改：实例 2 真隔离所需的资源拆分

这部分是未来批量脚本必须自动完成的内容。

### 修改 1：实例 2 使用自己的 binder 设备

真实修复：

- 创建 `inst2-binder/hwbinder/vndbinder`
- 修正其权限，不能保留 root-only 权限

踩坑原因：

- 仅仅创建 binder 设备还不够
- 如果权限不对，Android init / servicemanager / hwcomposer 会异常
- 结果是容器看似启动，实际服务起不来

### 修改 2：实例 2 使用自己的 Wayland socket

真实修复：

- 额外启动 `weston --socket=wayland-1`
- 把实例 2 的 `config_session` 改为绑定 `wayland-1`

踩坑原因：

- Waydroid 默认心智是单实例
- 实例 1 用掉 `wayland-0` 后，实例 2 如果沿用默认值，会导致图形栈异常

### 修改 3：实例 2 使用自己的 prop 文件配置，但 key 前缀不能乱改

真实修复：

- `waydroid2.prop` 中错误使用了 `waydroid2.*`
- 后来修回 `waydroid.*`

踩坑原因：

- 直觉上，第二实例应该把配置 key 也改成 `waydroid2.*`
- 但 `vendor.waydroid.task` 实际是按固定 key `waydroid.*` 读取
- 一旦改成 `waydroid2.*`，就会读不到 `xdg_runtime_dir / wayland_display`
- 结果表现为图形栈启动异常、服务迟迟不就绪，实例 2 业务不可用

这类坑很典型，说明：

> **“实例名称可参数化”不等于“配置 key 名称也可参数化”。**

后续脚本必须明确区分：

- 哪些是**路径 / 资源名**，可以按实例编号生成
- 哪些是**协议字段 / 系统约定 key**，必须保持固定

### 修改 4：实例 2 的 socat 必须绑定“当前容器 PID”

真实修复：

- 容器重启后，旧 socat 仍指向旧 PID
- 导致 ADB 入口存在，但设备实际 `offline` 或错误指向旧 namespace
- 最后必须重建 socat

踩坑原因：

- `socat` 的目标不是“实例名”，而是 `nsenter -t <container_pid> -n`
- PID 是动态值，不是实例静态身份的一部分
- 因此未来脚本绝不能写死：
  - `nsenter -t 50539`
  - 或任何一次实验中的 PID

正确做法必须是：

1. 先通过 `lxc-info` 查出当前容器 PID
2. 再动态生成 socat 进程
3. 容器重启后必须同步重建 socat

### 修改 5：实例 2 暴露第二条完整业务链路

真实修复：

- `adb forward tcp:18082 tcp:8080`
- Nginx 新增 `8082 -> 18082`
- 防火墙放行 `8082/tcp`

踩坑原因：

- 如果只在宿主机本地能连，外部客户端仍然申请不到实例
- 尤其是此前已经验证过 `8080` 可用，很容易误判成“客户端问题”
- 实际上只是第二实例的公网入口没暴露完整

---

## 4.3 代码修改：把 Agent 的 readiness 从“单实例心智”修成“多实例可分配”

### 修改文件

- `NewUI/yun/agent/src/services/apkProbe.ts`

### 实际改动

#### 改动 1：`applyCar` 前强制拉起 APK

当前代码里，`applyCarProfile()` 在真正执行业务前会先：

- `startApkActivity(instance)`

原因：

- 多实例环境下，`idle` 只表示“探针曾经成功过”
- 不表示“此刻 APK 进程仍然活着”
- 尤其是实例 2，之前出现过：
  - 探针通过
  - 进程退出 / 重启
  - Scheduler 仍把它当成可分配实例
  - 真正分配时才发现业务端口不可用

#### 改动 2：`applyCar` 前重新等待业务就绪，而不是只相信之前的探针结果

当前代码里，`applyCarProfile()` 在真正调用业务探针前会重新：

- `waitForApkReady(wsUrl, 45000, carBrand)`

原因：

- 单实例时代容易假设：探针通过一次，后面就一直可用
- 多实例并发时这个假设不成立
- 容器 / APK / profiles 加载状态都可能在实例 idle 期间发生变化

#### 改动 3：readiness 判定从“WS 可连”升级为“目标品牌 catalog 真可用”

根因不是 WebSocket 不通，而是：

- `getBrands` 成功
- 但 `getProfiles(brand)` 仍可能失败、为空，或触发异常

也就是说，以下状态都不足以判定实例可分配：

- `boot_completed=1`
- ADB 在线
- `/ws` 返回 `101`
- `getBrands` 成功

真正需要的判定至少应当包括：

- APK 已被拉起
- WebSocket 已可连
- `getBrands` 正常
- **目标品牌**的 `getProfiles(brand)` 正常
- `applyProfile(brand, profileIndex)` 正常

---

## 5. 这次踩过的核心坑，以及为什么会踩

## 5.1 坑一：把“端口不同”误当成“实例不同”

### 错误理解

- `5555 != 5556`
- `18081 != 18082`
- `8080 != 8082`

所以以为已经是多实例。

### 真实情况

只要底层仍是：

- 同一 LXC
- 同一 binder
- 同一 `/data`
- 同一 APK 进程

那就仍然是假多实例。

### 为什么会踩

因为调度层对外返回的实例地址，确实主要是靠 `wsPort` 区分。

这会让人天然产生一个误解：

> 既然调度层靠端口区分，那实例层也只要换端口就行。

这个结论是错的。

**端口只是外部入口，不是实例身份证。**

---

## 5.2 坑二：Waydroid 默认是单实例心智

Waydroid 官方路径、配置和工具链普遍假设：

- 一个默认工作目录
- 一组默认 binder 设备
- 一个默认 weston / wayland socket
- 一条默认网络链路

### 为什么会踩

因为“复制目录再启动一个容器”看起来像是多实例，但实际上这些系统资源仍然是 singleton。

所以未来脚本必须明确：

> **任何默认值，只要它是全局资源，就必须为实例槽位显式参数化。**

至少包括：

- binder 设备名
- Wayland display
- weston socket
- LXC 名称
- rootfs / host-permissions / data 目录
- bridge 名称
- adb 入口
- probePort
- wsPort

---

## 5.3 坑三：配置项名字不能想当然跟着实例名一起改

典型案例就是：

- `waydroid2.prop`
- 里面把 `waydroid.*` 改成了 `waydroid2.*`

### 为什么会踩

这是非常自然的直觉：既然实例名改了，配置 key 前缀也应该改。

但系统内部读取逻辑不是按实例名推导，而是写死读：

- `waydroid.*`

所以会出现“看起来很规范，实际上把系统读配置这件事改坏了”的情况。

### 结论

后续脚本必须给每个参数打标签：

- **可参数化命名项**
- **不可参数化协议项**

不能只做字符串替换。

---

## 5.4 坑四：容器 PID 是动态值，不是静态配置

### 典型问题

- 实例 2 容器重启后
- ADB 入口还在
- 但 socat 还指向旧 PID
- 最后设备 `offline` 或业务异常

### 为什么会踩

因为很多一次性调试命令天然会写成：

```bash
nsenter -t 50539 -n ...
```

调试时这样写没问题，但一旦进入脚本，这就变成了炸弹。

### 结论

后续所有与 namespace 绑定的动作都必须走：

1. 查当前 PID
2. 用当前 PID 生成进程
3. PID 变化时重建依赖它的代理

---

## 5.5 坑五：`boot_completed=1` 不是“实例可分配”

### 为什么会踩

单实例里，`boot_completed=1` 往往已经很接近“可用”。

但多实例里，这只说明：

- Android 系统启动完成

不说明：

- APK 已拉起
- WebSocket 已可用
- 默认品牌 profiles 已加载
- 目标车型 apply 能成功

### 结论

批量脚本不能把 `boot_completed=1` 当作最终验收，只能当作**中间步骤**。

---

## 5.6 坑六：`/ws` 返回 `101` 不是“业务完全可用”

### 为什么会踩

`101 Switching Protocols` 只能证明：

- 端口通
- 反代通
- APK 里有 WS 服务在监听

但不证明：

- `getBrands` 正常
- `getProfiles` 正常
- `applyProfile` 正常

### 结论

未来自动化验收必须分三层：

1. **系统层**
   - 容器运行
   - ADB 在线
   - boot_completed=1

2. **通道层**
   - probePort 通
   - wsPort 通
   - `/ws` 返回 `101`

3. **业务层**
   - `getBrands`
   - `getProfiles(brand)`
   - `applyProfile(brand, profileIndex)`

真正可分配的标准，至少要达到第三层。

---

## 5.7 坑七：单实例 probe 心智会让 Agent 错判实例已就绪

### 原始错误模式

实例探针通过一次后，Agent 把实例标成 `idle`。

但在多实例环境里，“通过过一次”不等于“此刻仍然能用”。

之前真实发生过：

- 探针结束
- A 端触发断开处理
- APK 重启 / 重新加载
- Scheduler 立刻拿去 `applyCar`
- 结果实例被打成 `bad`

### 修复原则

`applyCar` 必须是**重入式安全**的：

- 先拉起 APK
- 再重新等待 brand-ready
- 再做业务 apply

不能假设 idle 期间业务状态静止不变。

---

## 5.8 坑八：实例 2 不是特例，所有 inst_N 都必须按同一套规则生成

这次很多问题是先在 `inst_2` 暴露出来的，但它不是“实例 2 特有问题”。

本质上是因为此前只有单实例，很多逻辑都偷偷依赖了：

- 默认路径
- 默认端口
- 默认实例名
- 默认系统资源

一旦开始做 `inst_2`，这些隐藏假设才全部暴露。

### 结论

后续任何脚本、代码、配置都不能写成：

- “给实例 2 特判一下”

而必须写成：

- “对任意 `slot_index` 按同一规则生成和验收”

---

## 6. 未来批量脚本必须使用的“实例身份证”

后续自动化的最重要原则是：

> **脚本输入不应该是几个散落的端口号，而应该是一个完整的实例身份对象。**

## 6.1 静态身份字段

这些字段由“服务器级配置 + 槽位号”稳定派生，不应该手写散落在脚本各处。

| 字段 | 含义 |
|---|---|
| `server_id` | 当前实例层服务器唯一标识 |
| `slot_index` | 槽位序号，从 `1..N` |
| `slot_2d` | 两位槽位号，如 `01/02/03` |
| `instance_id` | 调度层全局实例 ID |
| `lxc_name` | LXC 容器名 |
| `runtime_name` | 运行时实例名 |
| `work_dir` | 实例工作目录根 |
| `rootfs_dir` | rootfs 目录 |
| `host_permissions_dir` | host-permissions 目录 |
| `data_dir` | 实例数据目录 |
| `prop_file` | 实例 prop 文件路径 |
| `binder_name` | binder 设备名 |
| `hwbinder_name` | hwbinder 设备名 |
| `vndbinder_name` | vndbinder 设备名 |
| `wayland_display` | 实例使用的 wayland socket 名 |
| `bridge_name` | 实例网络 bridge 名 |
| `adb_port` | 宿主机 ADB 代理端口 |
| `adb_target` | Agent 用的 ADB 地址 |
| `probe_port` | 宿主机本地探针端口 |
| `ws_port` | 对外业务端口 |
| `public_ws_host` | 对外公网域名或 IP（可选覆盖；默认由 Agent 自动探测） |

## 6.2 运行时动态字段

这些字段绝不能写死，必须每次运行时探测。

| 字段 | 含义 |
|---|---|
| `container_pid` | 当前容器 init PID |
| `weston_pid` | 当前实例对应的 Weston PID |
| `socat_pid` | 当前 ADB 代理 PID |
| `adb_state` | `device/offline` 等状态 |
| `boot_completed` | Android 是否完成启动 |
| `apk_pid` | APK 当前进程 PID |
| `ws_101_ok` | 对外 WebSocket 握手是否成功 |
| `brand_ready` | 指定品牌 catalog 是否可用 |
| `apply_ready` | `applyProfile` 是否可成功 |

---

## 6.3 推荐的派生规则

以下规则的重点不是“数值必须是这些”，而是：

> **必须通过统一公式派生，不能再手写 `5556/8082/waydroid2` 这种字面量。**

```yaml
server_id: ${SERVER_ID}
slot_index: ${SLOT_INDEX}
slot_2d: ${pad2(slot_index)}

instance_id: ${server_id}_inst_${slot_2d}
lxc_name: waydroid${slot_index == 1 ? '' : slot_index}
runtime_name: android-${slot_2d}

work_dir: /srv/obd/workers/${slot_2d}
rootfs_dir: /var/lib/${lxc_name}/rootfs
host_permissions_dir: /var/lib/${lxc_name}/host-permissions
data_dir: /root/.local/share/${lxc_name}/data
prop_file: /var/lib/${lxc_name}/waydroid.prop

binder_name: ${slot_index == 1 ? 'anbox-binder' : 'inst' + slot_index + '-binder'}
hwbinder_name: ${slot_index == 1 ? 'anbox-hwbinder' : 'inst' + slot_index + '-hwbinder'}
vndbinder_name: ${slot_index == 1 ? 'anbox-vndbinder' : 'inst' + slot_index + '-vndbinder'}

wayland_display: wayland-${slot_index - 1}
bridge_name: waydroid${slot_index - 1}

adb_port: ${ADB_BASE_PORT + slot_index - 1}
adb_target: 127.0.0.1:${adb_port}

probe_port: ${PROBE_BASE_PORT + slot_index - 1}
ws_port: ${WS_BASE_PORT + slot_index - 1}
public_ws_host: ${PUBLIC_WS_HOST:-<agent-auto-detect-public-host>}
```

---

## 6.4 必须保持固定、不能跟实例编号一起改名的内容

这一条必须单独强调。

以下内容属于**系统协议字段 / 固定约定**，不要随实例号改名：

- `waydroid.*` 配置 key
- APK 内部监听端口 `8080`
- WebSocket 路径 `/ws`
- Agent / Scheduler 对实例状态字段的协议字段名

也就是说：

- 路径可以按实例号区分
- 端口可以按实例号区分
- 设备名可以按实例号区分
- **协议字段名不要按实例号区分**

---

## 7. 给未来自动化脚本的明确要求

## 7.1 创建顺序不能乱

未来脚本在创建 `inst_N` 时，至少应按下面顺序执行：

1. 计算实例身份证
2. 创建工作目录
3. 创建 / 校验 binder 设备，并修正权限
4. 生成 LXC 配置
5. 生成 prop 文件，但保持 `waydroid.*` 固定 key
6. 挂载 rootfs / overlay / host-permissions
7. 启动对应的 Weston / Wayland socket
8. 创建 bridge
9. 启动 LXC 容器
10. 查询当前 `container_pid`
11. 按当前 PID 启动 socat
12. `adb connect`
13. 等待 `boot_completed=1`
14. 安装 / 启动 APK
15. `adb forward tcp:<probe_port> tcp:8080`
16. 配置 Nginx `ws_port -> probe_port`
17. 放行防火墙端口
18. 执行业务 readiness 验收
19. 验证通过后再注册给 Agent / Scheduler

## 7.2 验收顺序也不能乱

脚本在每个实例创建完成后，应按下面顺序验收：

### 第一步：运行时验收

- `lxc-info` 为 `RUNNING`
- `adb devices` 能看到对应 `adb_target`
- `boot_completed=1`

### 第二步：通道验收

- `probe_port` 已建立
- `curl /ws` 返回 `101`
- 对外 `ws_port` 从公网可访问

### 第三步：业务验收

- `getBrands`
- `getProfiles(default_brand)`
- `applyProfile(default_brand, default_index)`

### 第四步：并发验收

至少抽样验证：

- 两个实例同时被分配
- 两个实例都能完成 `applyCar`
- 两个实例互不影响

---

## 7.3 未来脚本必须避免的错误写法

不要再出现以下写法：

- 写死 `waydroid2`
- 写死 `5556`
- 写死 `18082`
- 写死 `8082`
- 写死 `inst2-binder`
- 写死 `wayland-1`
- 写死 `nsenter -t 50539`
- 对 `inst_2` 单独分支处理

正确写法必须都是：

- 输入 `slot_index`
- 派生实例身份证
- 所有动作都引用身份证对象
- 所有动态值都运行时探测

---

## 8. 未来在新 EC2 上最容易漏掉的点

## 8.1 只做容器克隆，不做系统资源隔离

会得到“假多实例”。

## 8.2 只看 `boot_completed`

会得到“系统活着但业务不可分配”的实例。

## 8.3 只看 `101`

会得到“端口通但车型 apply 失败”的实例。

## 8.4 忘记重建 socat

容器重启后就会出现 ADB offline / 指向旧 namespace。

## 8.5 把配置 key 一并重命名

会出现实例看似启动、实际关键服务读不到配置。

## 8.6 忘记开放第二实例端口

会被误判成客户端问题，实际是入口没暴露完整。

## 8.7 继续沿用单实例 readiness 逻辑

会出现：

- Dashboard 看上去有 idle
- 真分配时还是失败

## 8.8 把 `create_instance.sh` 误当成“新机从零部署脚本”

会出现：

- 一上来就报 `/var/lib/waydroid/images` 不存在
- 或 `slot_1` 不存在
- 或 Agent 环境文件不存在

因为：

- `create_instance.sh` 只负责 `slot_2+`
- 新机从零部署必须先跑：
  - `bootstrap_host.sh`
  - `bootstrap_slot1.sh`

## 8.9 忽略宿主机内核前置条件

会出现：

- Waydroid 包安装成功
- 但 `/dev/binderfs/binder-control` 不存在
- 后续所有实例脚本都无法真正启动

因为：

- 用户态包可以通过 `apt` 安装
- 但 `binder / binderfs` 是宿主机内核能力，不是脚本能补出来的

所以新 EC2 部署前必须先确认：

- ARM64 / aarch64
- Ubuntu 22.04 LTS
- 内核支持 `binder_linux` 或 `binder`
- 内核支持 `binderfs`

---

## 9. 这次阶段对未来脚本设计的直接结论

未来批量化脚本不应该只做“创建容器”。

它必须至少拆成三部分：

### A. `generate_identity(slot_index)`

负责生成实例身份证对象。

### B. `create_runtime(identity)`

负责：

- binder
- Wayland
- rootfs
- bridge
- LXC
- socat
- ADB
- forward
- Nginx
- 防火墙

### C. `verify_business_readiness(identity)`

负责：

- boot_completed
- WS 101
- getBrands
- getProfiles
- applyProfile

只有第三步通过，实例才能被 Agent 注册为 `idle`。

### D. `bootstrap_host() / bootstrap_slot1()`

未来已经不能再把“宿主机基线”和“slot_1 基线”默认为人工步骤。

它们也必须显式脚本化，并承担下面的职责：

- 安装宿主机依赖包
- 校验 binder / binderfs
- 部署 Agent / Scheduler / dashboard-web
- 完成 `waydroid init`
- 建立官方 `slot_1`
- 再把 `slot_2+` 交给 `create_instance.sh`

---

## 10. 建议与现有文档的关系

这份文档应与以下文档配合使用：

- `NewUI/yun/deployment/总体架构说明文档.md`
- `NewUI/yun/deployment/实例层大规模部署说明文档.md`
- `NewUI/yun/deployment/archive/audits/服务器审计报告_20260414.md`

三者职责不同：

- **总体架构说明文档**：讲生产架构边界
- **实例层大规模部署说明文档**：讲理想化部署结构
- **服务器审计报告**：讲当前服务器到底被改成了什么
- **本文件**：讲这次真实排障过程中，哪些坑会让“看起来对了”的多实例最后仍然失败

---

## 11. 一句话总括

这次最重要的经验不是“Waydroid 能多开”，而是：

> **多实例成功的关键不在“多开出几个容器”，而在“把实例的系统资源、入口链路、就绪判定、业务验收全部参数化并落实到同一个实例身份证对象上”。**

如果未来脚本只做到“复制一个 waydroid2”，那一定还会重踩这次的坑。
