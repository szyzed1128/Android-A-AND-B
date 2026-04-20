"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getDashboardOverview = getDashboardOverview;
exports.getDashboardInstances = getDashboardInstances;
exports.getDashboardServers = getDashboardServers;
exports.getDashboardSessions = getDashboardSessions;
exports.getDashboardAnomalies = getDashboardAnomalies;
const redis_1 = require("./redis");
const HEALTH_STALE_MS = 120 * 1000;
const SESSION_HEARTBEAT_STALE_MS = 150 * 1000;
const PROBING_STUCK_MS = 120 * 1000;
async function getDashboardOverview() {
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
        staleHealthInstances: instances.filter(item => typeof item.healthAgeMs === 'number' && item.healthAgeMs > HEALTH_STALE_MS).length,
        activeSessions: sessions.filter(item => item.status === 'running').length,
        reservedSessions: sessions.filter(item => item.status === 'reserved').length,
        disconnectedSessions: sessions.filter(item => item.status === 'disconnected').length,
        onlineDevices: sessions.filter(item => item.status !== 'disconnected' && item.heartbeatAgeMs <= SESSION_HEARTBEAT_STALE_MS).length,
        anomalyCount: anomalies.length,
    };
}
async function getDashboardInstances() {
    const redis = (0, redis_1.getRedis)();
    const now = Date.now();
    const ids = await redis.getAllInstanceIds();
    const disabledServers = new Set(await redis.getDisabledServerIds());
    const rawInstances = await Promise.all(ids.map(id => redis.getInstance(id)));
    return rawInstances
        .filter((item) => item !== null)
        .map(item => ({
        ...item,
        healthAgeMs: typeof item.lastHealthAt === 'number' ? now - item.lastHealthAt : undefined,
        statusAgeMs: typeof item.statusSince === 'number' ? now - item.statusSince : undefined,
        agentStatusAgeMs: typeof item.agentStatusSince === 'number' ? now - item.agentStatusSince : undefined,
        reservedRemainingMs: typeof item.reservedUntil === 'number' ? item.reservedUntil - now : undefined,
        serverDisabled: disabledServers.has(item.serverId),
    }))
        .sort((left, right) => left.id.localeCompare(right.id));
}
async function getDashboardServers() {
    const instances = await getDashboardInstances();
    const grouped = new Map();
    for (const instance of instances) {
        const current = grouped.get(instance.serverId) || {
            serverId: instance.serverId,
            serverIp: instance.serverIp,
            publicWsHost: instance.publicWsHost,
            agentPort: instance.agentPort,
            totalInstances: 0,
            idleInstances: 0,
            badInstances: 0,
            reservedInstances: 0,
            busyInstances: 0,
            probingInstances: 0,
            disabled: Boolean(instance.serverDisabled),
            cpu: instance.cpu,
            memory: instance.memory,
            lastHealthAt: instance.lastHealthAt,
            healthAgeMs: instance.healthAgeMs,
        };
        current.totalInstances += 1;
        current.disabled = current.disabled || Boolean(instance.serverDisabled);
        if (instance.status === 'idle')
            current.idleInstances += 1;
        if (instance.status === 'bad')
            current.badInstances += 1;
        if (instance.status === 'reserved' || instance.status === 'reserved_for_user')
            current.reservedInstances += 1;
        if (instance.status === 'busy')
            current.busyInstances += 1;
        if (instance.status === 'probing' || instance.agentStatus === 'probing')
            current.probingInstances += 1;
        if ((instance.lastHealthAt || 0) > (current.lastHealthAt || 0)) {
            current.serverIp = instance.serverIp;
            current.publicWsHost = instance.publicWsHost;
            current.agentPort = instance.agentPort;
            current.cpu = instance.cpu;
            current.memory = instance.memory;
            current.lastHealthAt = instance.lastHealthAt;
            current.healthAgeMs = instance.healthAgeMs;
        }
        grouped.set(instance.serverId, current);
    }
    return Array.from(grouped.values()).sort((left, right) => left.serverId.localeCompare(right.serverId));
}
async function getDashboardSessions(instancesArg) {
    const redis = (0, redis_1.getRedis)();
    const now = Date.now();
    const ids = await redis.getAllSessionIds();
    const rawSessions = await Promise.all(ids.map(id => redis.getSession(id)));
    const sessions = rawSessions.filter((item) => item !== null);
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
function getDashboardAnomalies(instances, sessions) {
    const anomalies = [];
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
        if (instance.agentStatus === 'probing' &&
            typeof instance.agentStatusAgeMs === 'number' &&
            instance.agentStatusAgeMs > PROBING_STUCK_MS) {
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
