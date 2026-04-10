/**
 * 共享类型定义
 */

// 实例状态
export type InstanceStatus = 'idle' | 'reserved' | 'busy' | 'bad' | 'reserved_for_user';

// 会话状态
export type SessionStatus = 'init' | 'reserved' | 'running' | 'disconnected';

// 实例信息（Redis存储）
export interface InstanceInfo {
  id: string;
  serverId: string;
  serverIp: string;
  wsPort: number;          // APK WebSocket端口
  agentPort: number;       // 本地Agent HTTP端口
  status: InstanceStatus;
  sessionId?: string;      // 当前占用的会话ID
  lastHealthAt?: number;   // 上次健康上报时间戳
  reservedForDeviceId?: string;  // reserved_for_user时记录设备ID
  reservedUntil?: number;  // reserved_for_user的到期时间戳
}

// 会话信息（Redis存储）
export interface SessionInfo {
  sessionId: string;
  deviceId: string;
  userId?: string;         // 预留：未来账号系统
  instanceId?: string;     // 分配到的实例ID
  status: SessionStatus;
  carBrand?: string;
  carModel?: string;
  btAddress?: string;
  btProtocol?: 'ble' | 'classic' | 'mfi';
  lastHeartbeatAt?: number;
  createdAt: number;
}

// Agent健康上报数据
export interface AgentHealthReport {
  serverId: string;
  serverIp: string;
  agentPort: number;
  instances: AgentInstanceHealth[];
  reportedAt: number;
}

export interface AgentInstanceHealth {
  id: string;
  wsPort: number;
  status: InstanceStatus;
  cpu?: number;       // CPU使用率(%)
  memory?: number;    // 内存使用MB
  health: 'ok' | 'bad';
}

// API请求/响应类型

export interface InitSessionRequest {
  deviceId: string;
}

export interface InitSessionResponse {
  sessionId: string;
}

export interface SetDeviceRequest {
  btAddress: string;
  btProtocol: 'ble' | 'classic' | 'mfi';
  btName?: string;
}

export interface SetCarRequest {
  carBrand: string;
  carModel: string;
}

export interface ReserveInstanceResponse {
  wsUrl: string;
  instanceId: string;
}

export interface ApiResponse<T = void> {
  success: boolean;
  data?: T;
  error?: string;
}
