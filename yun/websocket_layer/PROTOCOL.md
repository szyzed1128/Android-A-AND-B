# 车辆诊断云端化 WebSocket通信协议

## 概述

软件A（云端Android模拟器中运行的原始APK，**WebSocket服务器**）与软件B（手机蓝牙网关，**WebSocket客户端**）之间的通信协议。

## 连接方式

- 协议: `ws://` (开发) / `wss://` (生产)
- 默认端口: `8080`
- 路径: `/ws`（A端服务器监听）
- 数据格式: JSON
  
> 连接方向：**B(客户端) → A(服务器)**。  
> 连接建立后，A 可主动下发请求，B 负责响应与事件回传。

## 消息基本格式

```json
{
  "type": "request|response|event",
  "action": "string",
  "requestId": "uuid",
  "sessionId": "uuid",
  "data": {},
  "success": true,
  "error": "string",
  "timestamp": 1234567890
}
```

### 字段说明

| 字段 | 类型 | 必填 | 说明 |
|------|------|------|------|
| type | string | 是 | 消息类型: request/response/event |
| action | string | 是 | 操作名称 |
| requestId | string | 条件 | 请求ID，用于匹配request和response |
| sessionId | string | 条件 | 蓝牙连接会话ID |
| data | object/string | 条件 | 载荷数据（对象或Base64编码的二进制） |
| success | boolean | response时 | 操作是否成功 |
| error | string | 失败时 | 错误描述 |
| timestamp | number | 否 | Unix毫秒时间戳 |

---

## 操作列表

### 1. 心跳 (ping/pong)

**A(服务器) → B(客户端):**
```json
{"type": "request", "action": "ping", "requestId": "req-001"}
```

**B(客户端) → A(服务器):**
```json
{"type": "response", "action": "pong", "requestId": "req-001", "success": true}
```

### 2. 蓝牙扫描

**A(服务器) → B(客户端): 开始扫描**
```json
{"type": "request", "action": "startScan", "requestId": "req-002"}
```

**B(客户端) → A(服务器): 确认扫描开始**
```json
{"type": "response", "action": "startScan", "requestId": "req-002", "success": true}
```

**B(客户端) → A(服务器): 发现设备事件（多次）**
```json
{
  "type": "event",
  "action": "deviceDiscovered",
  "data": {
    "name": "OBDII",
    "address": "00:1D:A5:68:98:8B",
    "rssi": -65,
    "valid": true,
    "paired": false
  }
}
```

**A(服务器) → B(客户端): 停止扫描**
```json
{"type": "request", "action": "stopScan", "requestId": "req-003"}
```

**B(客户端) → A(服务器): 确认停止**
```json
{"type": "response", "action": "stopScan", "requestId": "req-003", "success": true}
```

**B(客户端) → A(服务器): 扫描完成事件**
```json
{"type": "event", "action": "scanFinished"}
```

### 3. 蓝牙连接

**A(服务器) → B(客户端): 连接设备**
```json
{
  "type": "request",
  "action": "connect",
  "requestId": "req-004",
  "data": {
    "protocol": "bt",
    "address": "00:1D:A5:68:98:8B"
  }
}
```

**B(客户端) → A(服务器): 连接成功**
```json
{
  "type": "response",
  "action": "connect",
  "requestId": "req-004",
  "success": true,
  "sessionId": "session-abc-123"
}
```

**B(客户端) → A(服务器): 连接失败**
```json
{
  "type": "response",
  "action": "connect",
  "requestId": "req-004",
  "success": false,
  "error": "Device not found or connection timeout"
}
```

### 4. 断开连接

**A(服务器) → B(客户端):**
```json
{
  "type": "request",
  "action": "disconnect",
  "requestId": "req-005",
  "sessionId": "session-abc-123"
}
```

**B(客户端) → A(服务器):**
```json
{
  "type": "response",
  "action": "disconnect",
  "requestId": "req-005",
  "success": true
}
```

### 5. 发送诊断数据

**A(服务器) → B(客户端): 发送命令**
```json
{
  "type": "request",
  "action": "send",
  "requestId": "req-006",
  "sessionId": "session-abc-123",
  "data": "MDEwRA0="
}
```
> data 字段为 Base64 编码的字节流, 例如 `"010D\r"` → `"MDEwRA0="`

**B(客户端) → A(服务器): 发送确认**
```json
{
  "type": "response",
  "action": "send",
  "requestId": "req-006",
  "success": true
}
```

### 6. 接收诊断数据

**B(客户端) → A(服务器): ELM327 返回数据事件**
```json
{
  "type": "event",
  "action": "obdData",
  "sessionId": "session-abc-123",
  "data": "NDEgMEQgM0MNCj4="
}
```
> data 字段为 Base64 编码的响应, 例如 `"41 0D 3C\r>"` → `"NDEgMEQgM0MNCj4="`

### 7. 连接状态变更

**B(客户端) → A(服务器): 设备断开事件**
```json
{
  "type": "event",
  "action": "connectionLost",
  "sessionId": "session-abc-123",
  "data": {
    "reason": "Device disconnected"
  }
}
```

### 8. 错误事件

**B(客户端) → A(服务器):**
```json
{
  "type": "event",
  "action": "error",
  "data": {
    "code": "BT_UNAVAILABLE",
    "message": "Bluetooth is not available on this device"
  }
}
```

---

## 错误码

| 错误码 | 说明 |
|--------|------|
| BT_UNAVAILABLE | 蓝牙不可用 |
| BT_DISABLED | 蓝牙未开启 |
| DEVICE_NOT_FOUND | 设备未找到 |
| CONNECTION_TIMEOUT | 连接超时 |
| CONNECTION_LOST | 连接丢失 |
| SEND_FAILED | 发送失败 |
| INVALID_SESSION | 无效会话 |
| PERMISSION_DENIED | 权限不足 |

---

## 重连策略

1. WebSocket断开后，A端自动尝试重连
2. 重试间隔: 1s → 2s → 4s → 8s → 16s (指数退避，最大16s)
3. 重连成功后，需要重新建立蓝牙会话
4. B端保持蓝牙连接，等待A端重连
