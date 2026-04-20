/**
 * 共享类型定义
 */

// 调度侧实例状态
export type InstanceStatus = 'idle' | 'reserved' | 'busy' | 'bad' | 'reserved_for_user' | 'probing';

// Agent 原始运行时状态
export type AgentRuntimeStatus = 'idle' | 'probing' | 'busy' | 'bad';

// 健康状态
export type HealthStatus = 'ok' | 'bad';

// 会话状态
export type SessionStatus = 'init' | 'reserved' | 'running' | 'disconnected';

// 实例信息（Redis存储）
export interface InstanceInfo {
  id: string;
  serverId: string;
  serverIp: string;        // 调度层访问 Agent 的控制地址（可为内网）
  publicWsHost?: string;   // B 端连接实例 WebSocket 的对外地址
  publicWsScheme?: 'ws' | 'wss';
  disabled?: boolean;      // 实例级摘除开关；true 时不再进入调度池
  wsPort: number;          // APK WebSocket端口
  agentPort: number;       // 本地Agent HTTP端口
  status: InstanceStatus;
  statusSince?: number;    // 当前调度状态开始时间
  sessionId?: string;      // 当前占用的会话ID
  lastHealthAt?: number;   // 上次健康上报时间戳
  reservedForDeviceId?: string;  // reserved_for_user时记录设备ID
  reservedUntil?: number;  // reserved_for_user的到期时间戳
  health?: HealthStatus;
  cpu?: number;      // 宿主机 CPU 使用率(%)
  memory?: number;   // 宿主机内存使用率(%)
  agentStatus?: AgentRuntimeStatus;
  agentStatusSince?: number;
  failureCount?: number;
  lastError?: string;
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
  profileIndex?: number;
  profileName?: string;
  btAddress?: string;
  btName?: string;
  btProtocol?: 'ble' | 'classic' | 'mfi';
  lastHeartbeatAt?: number;
  createdAt: number;
}

// Agent健康上报数据
export interface AgentHealthReport {
  serverId: string;
  serverIp: string;          // 调度层访问 Agent 的控制地址
  publicWsHost?: string;     // B 端连接实例 WebSocket 的对外地址
  publicWsScheme?: 'ws' | 'wss';
  agentPort: number;
  instances: AgentInstanceHealth[];
  reportedAt: number;
}

export interface AgentInstanceHealth {
  id: string;
  wsPort: number;
  agentStatus: AgentRuntimeStatus;
  cpu?: number;       // CPU使用率(%)
  memory?: number;    // 内存使用MB
  health: HealthStatus;
  failureCount?: number;
  lastError?: string;
  statusChangedAt?: number;
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
  profileIndex: number;
  profileName: string;
}

export interface CatalogProfileSummary {
  profileIndex: number;
  name: string;
  description?: string;
}

export interface CatalogSnapshot {
  recordedAt: number;
  brands: string[];
  profilesByBrand: Record<string, CatalogProfileSummary[]>;
  source?: {
    instanceId: string;
    serverIp: string;
    agentPort: number;
  };
}

export interface ReserveInstanceResponse {
  wsUrl: string;
  instanceId: string;
}

export interface SessionStateResponse {
  sessionId: string;
  status: SessionStatus;
  carBrand?: string;
  carModel?: string;
  profileIndex?: number;
  profileName?: string;
  btAddress?: string;
  btName?: string;
  btProtocol?: 'ble' | 'classic' | 'mfi';
  instanceId?: string;
}

export interface ApiResponse<T = void> {
  success: boolean;
  data?: T;
  error?: string;
}
