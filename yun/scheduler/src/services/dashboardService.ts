import { getRedis } from './redis';
import { InstanceInfo, SessionInfo } from '../types';

const HEALTH_STALE_MS = 120 * 1000;
const SESSION_HEARTBEAT_STALE_MS = 150 * 1000;
const PROBING_STUCK_MS = 120 * 1000;

export interface DashboardOverview {
  totalInstances: number;
  idleInstances: number;
  reservedInstances: number;
  busyInstances: number;
  reservedForUserInstances: number;
  badInstances: number;
  probingInstances: number;
  staleHealthInstances: number;
  activeSessions: number;
  reservedSessions: number;
  disconnectedSessions: number;
  onlineDevices: number;
  anomalyCount: number;
}

export interface DashboardInstanceRow extends InstanceInfo {
  healthAgeMs?: number;
  statusAgeMs?: number;
  agentStatusAgeMs?: number;
  reservedRemainingMs?: number;
}

export interface DashboardSessionRow extends SessionInfo {
  durationMs: number;
  heartbeatAgeMs: number;
  instanceStatus?: string;
  instanceHealth?: string;
}

export interface DashboardAnomaly {
  type:
    | 'SESSION_RUNNING_NO_INSTANCE'
    | 'SESSION_RESERVED_NO_INSTANCE'
    | 'SESSION_HEARTBEAT_TIMEOUT'
    | 'INSTANCE_BUSY_NO_SESSION'
    | 'INSTANCE_RESERVED_NO_SESSION'
    | 'INSTANCE_PROBING_TOO_LONG'
    | 'INSTANCE_HEALTH_STALE'
    | 'INSTANCE_HEALTH_BAD';
  severity: 'warning' | 'critical';
  instanceId?: string;
  sessionId?: string;
  deviceId?: string;
  message: string;
  ageMs?: number;
}

export async function getDashboardOverview(): Promise<DashboardOverview> {
  const instances = await getDashboardInstances();
  const sessions = await getDashboardSessions(instances);
  const anomalies = getDashboardAnomalies(instances, sessions);

  return {
    totalInstances: instances.length,
    idleInstances: instances.filter(item => item.status === 'idle').length,
    reservedInstances: instances.filter(item => item.status === 'reserved').length,
    busyInstances: instances.filter(item => item.status === 'busy').length,
    reservedForUserInstances: instances.filter(item => item.status === 'reserved_for_user').length,
    badInstances: instances.filter(item => item.status === 'bad').length,
    probingInstances: instances.filter(item => item.agentStatus === 'probing' || item.status === 'probing').length,
    staleHealthInstances: instances.filter(item =>
      typeof item.healthAgeMs === 'number' && item.healthAgeMs > HEALTH_STALE_MS
    ).length,
    activeSessions: sessions.filter(item => item.status === 'running').length,
    reservedSessions: sessions.filter(item => item.status === 'reserved').length,
    disconnectedSessions: sessions.filter(item => item.status === 'disconnected').length,
    onlineDevices: sessions.filter(item =>
      item.status !== 'disconnected' && item.heartbeatAgeMs <= SESSION_HEARTBEAT_STALE_MS
    ).length,
    anomalyCount: anomalies.length,
  };
}

export async function getDashboardInstances(): Promise<DashboardInstanceRow[]> {
  const redis = getRedis();
  const now = Date.now();
  const ids = await redis.getAllInstanceIds();
  const rawInstances = await Promise.all(ids.map(id => redis.getInstance(id)));

  return rawInstances
    .filter((item): item is InstanceInfo => item !== null)
    .map(item => ({
      ...item,
      healthAgeMs: typeof item.lastHealthAt === 'number' ? now - item.lastHealthAt : undefined,
      statusAgeMs: typeof item.statusSince === 'number' ? now - item.statusSince : undefined,
      agentStatusAgeMs: typeof item.agentStatusSince === 'number' ? now - item.agentStatusSince : undefined,
      reservedRemainingMs: typeof item.reservedUntil === 'number' ? item.reservedUntil - now : undefined,
    }))
    .sort((left, right) => left.id.localeCompare(right.id));
}

export async function getDashboardSessions(
  instancesArg?: DashboardInstanceRow[]
): Promise<DashboardSessionRow[]> {
  const redis = getRedis();
  const now = Date.now();
  const ids = await redis.getAllSessionIds();
  const rawSessions = await Promise.all(ids.map(id => redis.getSession(id)));
  const sessions = rawSessions.filter((item): item is SessionInfo => item !== null);
  const instances = instancesArg ?? await getDashboardInstances();
  const instanceMap = new Map(instances.map(item => [item.id, item]));

  return sessions
    .map(item => {
      const instance = item.instanceId ? instanceMap.get(item.instanceId) : undefined;
      const lastHeartbeatAt = item.lastHeartbeatAt ?? item.createdAt;
      return {
        ...item,
        durationMs: now - item.createdAt,
        heartbeatAgeMs: now - lastHeartbeatAt,
        instanceStatus: instance?.status,
        instanceHealth: instance?.health,
      };
    })
    .sort((left, right) => right.createdAt - left.createdAt);
}

export function getDashboardAnomalies(
  instances: DashboardInstanceRow[],
  sessions: DashboardSessionRow[]
): DashboardAnomaly[] {
  const anomalies: DashboardAnomaly[] = [];

  for (const session of sessions) {
    if (session.status === 'running' && !session.instanceId) {
      anomalies.push({
        type: 'SESSION_RUNNING_NO_INSTANCE',
        severity: 'critical',
        sessionId: session.sessionId,
        deviceId: session.deviceId,
        message: '运行中会话缺少实例绑定',
      });
    }

    if (session.status === 'reserved' && !session.instanceId) {
      anomalies.push({
        type: 'SESSION_RESERVED_NO_INSTANCE',
        severity: 'warning',
        sessionId: session.sessionId,
        deviceId: session.deviceId,
        message: '已预留会话尚未绑定实例',
      });
    }

    if (session.status !== 'disconnected' && session.heartbeatAgeMs > SESSION_HEARTBEAT_STALE_MS) {
      anomalies.push({
        type: 'SESSION_HEARTBEAT_TIMEOUT',
        severity: 'critical',
        sessionId: session.sessionId,
        deviceId: session.deviceId,
        ageMs: session.heartbeatAgeMs,
        message: '会话心跳超时',
      });
    }
  }

  for (const instance of instances) {
    if (instance.status === 'busy' && !instance.sessionId) {
      anomalies.push({
        type: 'INSTANCE_BUSY_NO_SESSION',
        severity: 'critical',
        instanceId: instance.id,
        message: '实例处于 busy 但没有 sessionId',
      });
    }

    if (instance.status === 'reserved' && !instance.sessionId) {
      anomalies.push({
        type: 'INSTANCE_RESERVED_NO_SESSION',
        severity: 'warning',
        instanceId: instance.id,
        message: '实例处于 reserved 但没有 sessionId',
      });
    }

    if (
      instance.agentStatus === 'probing' &&
      typeof instance.agentStatusAgeMs === 'number' &&
      instance.agentStatusAgeMs > PROBING_STUCK_MS
    ) {
      anomalies.push({
        type: 'INSTANCE_PROBING_TOO_LONG',
        severity: 'critical',
        instanceId: instance.id,
        ageMs: instance.agentStatusAgeMs,
        message: '实例处于 probing 过久',
      });
    }

    if (typeof instance.healthAgeMs === 'number' && instance.healthAgeMs > HEALTH_STALE_MS) {
      anomalies.push({
        type: 'INSTANCE_HEALTH_STALE',
        severity: 'warning',
        instanceId: instance.id,
        ageMs: instance.healthAgeMs,
        message: '实例健康上报过期',
      });
    }

    if (instance.health === 'bad' || instance.status === 'bad') {
      anomalies.push({
        type: 'INSTANCE_HEALTH_BAD',
        severity: 'critical',
        instanceId: instance.id,
        message: instance.lastError
          ? `实例健康异常：${instance.lastError}`
          : '实例健康异常',
      });
    }
  }

  return anomalies.sort((left, right) => {
    if (left.severity !== right.severity) {
      return left.severity === 'critical' ? -1 : 1;
    }
    return (right.ageMs ?? 0) - (left.ageMs ?? 0);
  });
}
