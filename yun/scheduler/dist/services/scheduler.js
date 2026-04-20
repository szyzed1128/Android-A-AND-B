"use strict";
/**
 * 核心调度逻辑
 * - 实例分配与释放
 * - 心跳超时监控
 * - 5分钟保留逻辑
 */
Object.defineProperty(exports, "__esModule", { value: true });
exports.reserveInstance = reserveInstance;
exports.getCatalogBrands = getCatalogBrands;
exports.getCatalogProfiles = getCatalogProfiles;
exports.releaseInstance = releaseInstance;
exports.disconnectSession = disconnectSession;
exports.startHeartbeatMonitor = startHeartbeatMonitor;
exports.handleAgentHealthReport = handleAgentHealthReport;
exports.isInstanceBlocked = isInstanceBlocked;
exports.isInstanceSchedulableForReserve = isInstanceSchedulableForReserve;
exports.isInstanceUsableForCatalog = isInstanceUsableForCatalog;
exports.syncInstanceIdlePool = syncInstanceIdlePool;
const redis_1 = require("./redis");
const agentClient_1 = require("./agentClient");
const catalogSnapshot_1 = require("./catalogSnapshot");
// 保留时间：用户主动断开后保留实例的时长（毫秒）
const RESERVE_FOR_USER_MS = 5 * 60 * 1000;
// 心跳超时：超过此时间无心跳则释放（毫秒）
const HEARTBEAT_TIMEOUT_MS = 150 * 1000;
// 最大重试次数（reserve时尝试不同实例）
const MAX_RESERVE_RETRIES = 3;
// 目录缓存新鲜度：1小时内直接使用缓存；超过后尝试实时刷新，失败则回退旧缓存
const CATALOG_CACHE_FRESH_MS = 60 * 60 * 1000;
/**
 * 为会话分配实例并应用用户车型配置
 * 只有 applyProfile 成功后才返回 wsUrl
 */
async function reserveInstance(session) {
    const redis = (0, redis_1.getRedis)();
    if (!session.carBrand || session.profileIndex === undefined || !session.btAddress) {
        throw new Error('缺少必要配置：carBrand/profileIndex/btAddress');
    }
    // 检查是否有保留给该用户的实例（主动断开后5分钟内重连）
    const reservedInstance = await findReservedInstanceForUser(session.deviceId);
    if (reservedInstance) {
        console.log(`[Scheduler] 用户 ${session.deviceId} 重连，复用保留实例 ${reservedInstance.id}`);
        // 更新状态
        await redis.updateInstanceStatus(reservedInstance.id, 'reserved', {
            sessionId: session.sessionId,
        });
        await redis.updateSession(session.sessionId, {
            instanceId: reservedInstance.id,
            status: 'reserved',
        });
        // 重新应用车型配置（可能用户换了车型）
        await applyCarOnInstance(reservedInstance, session.carBrand, session.profileIndex, session.profileName || session.carModel);
        const wsUrl = buildWsUrl(reservedInstance);
        return { wsUrl, instanceId: reservedInstance.id };
    }
    // 从空闲池获取实例，最多重试 MAX_RESERVE_RETRIES 次
    let lastError = '';
    for (let attempt = 0; attempt < MAX_RESERVE_RETRIES; attempt++) {
        const instanceId = await redis.popIdleInstance();
        if (!instanceId) {
            throw new Error('当前没有空闲实例，请稍后再试');
        }
        const instance = await redis.getInstance(instanceId);
        if (!instance) {
            console.warn(`[Scheduler] 实例 ${instanceId} 不存在，跳过`);
            continue;
        }
        if (!(await isInstanceSchedulableForReserve(instance))) {
            console.warn(`[Scheduler] 实例 ${instanceId} 当前不可分配，跳过`);
            await syncInstanceIdlePool(instanceId);
            continue;
        }
        try {
            // 标记为 reserved
            await redis.updateInstanceStatus(instanceId, 'reserved', {
                sessionId: session.sessionId,
            });
            await redis.updateSession(session.sessionId, {
                instanceId,
                status: 'reserved',
            });
            // 调用 Agent 应用用户车型配置
            await applyCarOnInstance(instance, session.carBrand, session.profileIndex, session.profileName || session.carModel);
            const wsUrl = buildWsUrl(instance);
            console.log(`[Scheduler] 实例分配成功 instanceId=${instanceId} wsUrl=${wsUrl}`);
            return { wsUrl, instanceId };
        }
        catch (err) {
            console.error(`[Scheduler] 实例 ${instanceId} 应用车型失败 (尝试${attempt + 1}):`, err.message);
            lastError = err.message;
            // 标记为 bad，让 Agent 重新初始化
            await redis.updateInstanceStatus(instanceId, 'bad', {}, ['sessionId']);
            // 清除 session 中指向已损坏实例的 instanceId
            await redis.updateSession(session.sessionId, { status: 'init' }, ['instanceId']);
            // 通知 Agent 重置
            triggerInstanceReset(instance).catch(e => console.error(`[Scheduler] 触发重置失败 ${instanceId}:`, e.message));
        }
    }
    throw new Error(`分配实例失败（重试${MAX_RESERVE_RETRIES}次）: ${lastError}`);
}
/**
 * 调用 Agent 应用车型配置
 */
async function applyCarOnInstance(instance, carBrand, profileIndex, profileName) {
    const agent = (0, agentClient_1.createAgentClient)(instance.serverIp, instance.agentPort);
    const result = await agent.applyCar(instance.id, carBrand, profileIndex, profileName);
    if (!result.success) {
        throw new Error(`applyCar 失败: ${result.error || '未知错误'}`);
    }
}
async function getCatalogCandidates() {
    const redis = (0, redis_1.getRedis)();
    const allIds = await redis.getAllInstanceIds();
    const candidates = [];
    for (const id of allIds) {
        const instance = await redis.getInstance(id);
        if (!(await isInstanceUsableForCatalog(instance)))
            continue;
        if (!instance)
            continue;
        candidates.push(instance);
    }
    candidates.sort((a, b) => (b.lastHealthAt || 0) - (a.lastHealthAt || 0));
    return candidates;
}
async function getCatalogBrands() {
    const snapshotBrands = await (0, catalogSnapshot_1.getSnapshotBrands)();
    if (snapshotBrands) {
        return snapshotBrands;
    }
    const redis = (0, redis_1.getRedis)();
    return readCatalogWithCache({
        label: 'catalog brands',
        readCache: () => redis.getCatalogBrandsCache(),
        writeCache: (brands) => redis.setCatalogBrandsCache(brands),
        loadFromLive: loadCatalogBrandsFromLive,
    });
}
async function getCatalogProfiles(brand) {
    const snapshot = await (0, catalogSnapshot_1.getCatalogSnapshot)();
    if (snapshot) {
        const profiles = await (0, catalogSnapshot_1.getSnapshotProfiles)(brand);
        if (!profiles) {
            throw new Error(`录制目录中不存在品牌 ${brand}`);
        }
        return profiles;
    }
    const redis = (0, redis_1.getRedis)();
    return readCatalogWithCache({
        label: `catalog profiles brand=${brand}`,
        readCache: () => redis.getCatalogProfilesCache(brand),
        writeCache: (profiles) => redis.setCatalogProfilesCache(brand, profiles),
        loadFromLive: () => loadCatalogProfilesFromLive(brand),
    });
}
async function loadCatalogBrandsFromLive() {
    const candidates = await getCatalogCandidates();
    if (candidates.length === 0) {
        throw new Error('当前没有可用于车型目录查询的空闲实例，且本地无可用缓存');
    }
    let lastError = '读取品牌列表失败';
    for (const instance of candidates) {
        try {
            const agent = (0, agentClient_1.createAgentClient)(instance.serverIp, instance.agentPort);
            return await agent.getCatalogBrands(instance.id);
        }
        catch (err) {
            lastError = err.message || lastError;
            console.warn(`[Scheduler] catalog brands 失败 instance=${instance.id}: ${lastError}`);
        }
    }
    throw new Error(lastError);
}
async function loadCatalogProfilesFromLive(brand) {
    const candidates = await getCatalogCandidates();
    if (candidates.length === 0) {
        throw new Error(`当前没有可用于品牌 ${brand} 目录查询的空闲实例，且本地无可用缓存`);
    }
    let lastError = `读取品牌 ${brand} 的配置失败`;
    for (const instance of candidates) {
        try {
            const agent = (0, agentClient_1.createAgentClient)(instance.serverIp, instance.agentPort);
            return await agent.getCatalogProfiles(instance.id, brand);
        }
        catch (err) {
            lastError = err.message || lastError;
            console.warn(`[Scheduler] catalog profiles 失败 instance=${instance.id} brand=${brand}: ${lastError}`);
        }
    }
    throw new Error(lastError);
}
function isFreshCatalogCache(updatedAt) {
    return typeof updatedAt === 'number' && (Date.now() - updatedAt) < CATALOG_CACHE_FRESH_MS;
}
async function readCatalogWithCache(options) {
    const cached = await options.readCache();
    if (cached && isFreshCatalogCache(cached.updatedAt)) {
        return cached.data;
    }
    try {
        const data = await options.loadFromLive();
        await options.writeCache(data);
        return data;
    }
    catch (err) {
        if (cached) {
            const ageSeconds = Math.max(0, Math.floor((Date.now() - cached.updatedAt) / 1000));
            console.warn(`[Scheduler] ${options.label} 实时刷新失败，回退缓存 age=${ageSeconds}s: ${err.message || '未知错误'}`);
            return cached.data;
        }
        throw err;
    }
}
/**
 * 释放实例（通知 Agent 重置，实例回到就绪流程）
 */
async function releaseInstance(instanceId, reason) {
    const redis = (0, redis_1.getRedis)();
    const instance = await redis.getInstance(instanceId);
    if (!instance) {
        console.warn(`[Scheduler] 释放实例 ${instanceId} 不存在，跳过`);
        return;
    }
    console.log(`[Scheduler] 释放实例 ${instanceId}，原因: ${reason}`);
    // 先从空闲池移除（防重复）
    await redis.removeFromIdlePool(instanceId);
    // 更新状态，明确清除关联字段（HDEL，不留脏数据）
    await redis.updateInstanceStatus(instanceId, 'bad', {}, ['sessionId', 'reservedForDeviceId', 'reservedUntil']);
    // 通知 Agent 重置（异步，不阻塞）
    triggerInstanceReset(instance).catch(e => console.error(`[Scheduler] 触发重置失败 ${instanceId}:`, e.message));
}
/**
 * 用户主动断开：保留实例给该用户5分钟
 */
async function disconnectSession(session) {
    const redis = (0, redis_1.getRedis)();
    if (!session.instanceId)
        return;
    const instance = await redis.getInstance(session.instanceId);
    if (!instance)
        return;
    const reservedUntil = Date.now() + RESERVE_FOR_USER_MS;
    // 保留实例给该用户5分钟，明确清除 sessionId（实例不再属于任何活跃 session）
    // reservedUntil 写入 Redis，服务重启后 checkHeartbeats 仍可检测到期
    await redis.updateInstanceStatus(session.instanceId, 'reserved_for_user', {
        reservedForDeviceId: session.deviceId,
        reservedUntil,
    }, ['sessionId']); // HDEL sessionId，避免脏数据
    await redis.updateSession(session.sessionId, { status: 'disconnected' });
    console.log(`[Scheduler] 实例 ${session.instanceId} 保留给用户 ${session.deviceId} 5分钟（到期: ${new Date(reservedUntil).toISOString()}）`);
    // 不使用 setTimeout，到期检测由 checkHeartbeats 每60秒扫描完成，重启后仍有效
}
/**
 * 查找保留给指定用户的实例
 */
async function findReservedInstanceForUser(deviceId) {
    const redis = (0, redis_1.getRedis)();
    const allIds = await redis.getAllInstanceIds();
    const now = Date.now();
    for (const id of allIds) {
        const instance = await redis.getInstance(id);
        if (instance?.status === 'reserved_for_user' &&
            instance.reservedForDeviceId === deviceId &&
            instance.reservedUntil &&
            instance.reservedUntil > now &&
            !(await isInstanceBlocked(instance))) {
            return instance;
        }
    }
    return null;
}
/**
 * 异步触发 Agent 重置实例
 */
async function triggerInstanceReset(instance) {
    const agent = (0, agentClient_1.createAgentClient)(instance.serverIp, instance.agentPort);
    const result = await agent.reset(instance.id);
    if (!result.success) {
        console.error(`[Scheduler] Agent 重置失败 instance=${instance.id}: ${result.error}`);
    }
}
/**
 * 构建 WebSocket URL
 */
function buildWsUrl(instance) {
    const scheme = instance.publicWsScheme || 'ws';
    const host = instance.publicWsHost || instance.serverIp;
    return `${scheme}://${host}:${instance.wsPort}/ws`;
}
/**
 * 心跳监控：每60秒扫描，超过150秒无心跳则释放
 */
function startHeartbeatMonitor() {
    setInterval(async () => {
        await checkHeartbeats();
    }, 60 * 1000);
    console.log('[Scheduler] 心跳监控已启动（每60秒扫描，150秒超时）');
}
async function checkHeartbeats() {
    const redis = (0, redis_1.getRedis)();
    const allIds = await redis.getAllInstanceIds();
    const now = Date.now();
    for (const instanceId of allIds) {
        const instance = await redis.getInstance(instanceId);
        if (!instance)
            continue;
        // ① 检查 busy 实例的心跳超时（用户正在使用中）
        if (instance.sessionId && instance.status === 'busy') {
            const session = await redis.getSession(instance.sessionId);
            if (session) {
                const lastHeartbeat = session.lastHeartbeatAt || session.createdAt;
                const elapsed = now - lastHeartbeat;
                if (elapsed > HEARTBEAT_TIMEOUT_MS) {
                    console.warn(`[Scheduler] busy 心跳超时 sessionId=${session.sessionId} elapsed=${elapsed}ms，释放实例`);
                    await releaseInstance(instanceId, `心跳超时 ${elapsed}ms`);
                    await redis.deleteSession(session.sessionId);
                    await redis.deleteDeviceSession(session.deviceId);
                }
            }
        }
        // ② 检查 reserved 实例的心跳超时
        // 场景：applyCar 成功返回 wsUrl 后，用户 App 崩溃，从未调用蓝牙连接
        // 此时实例是 reserved，session 是 reserved，心跳断了但状态机卡住
        if (instance.sessionId && instance.status === 'reserved') {
            const session = await redis.getSession(instance.sessionId);
            if (session) {
                const lastHeartbeat = session.lastHeartbeatAt || session.createdAt;
                const elapsed = now - lastHeartbeat;
                if (elapsed > HEARTBEAT_TIMEOUT_MS) {
                    console.warn(`[Scheduler] reserved 心跳超时 sessionId=${session.sessionId} elapsed=${elapsed}ms，释放实例`);
                    await releaseInstance(instanceId, `reserved状态心跳超时 ${elapsed}ms`);
                    await redis.deleteSession(session.sessionId);
                    await redis.deleteDeviceSession(session.deviceId);
                }
            }
        }
        // ③ 检查 reserved_for_user 实例的5分钟保留到期
        // 用 Redis 持久化的 reservedUntil，服务重启后依然有效
        if (instance.status === 'reserved_for_user' && instance.reservedUntil) {
            if (now > instance.reservedUntil) {
                console.log(`[Scheduler] 5分钟保留到期，释放实例 ${instanceId}`);
                await releaseInstance(instanceId, '5分钟保留到期');
            }
        }
    }
}
/**
 * 处理 Agent 健康上报
 * 将 idle 状态的实例加入空闲池
 */
async function handleAgentHealthReport(report) {
    const redis = (0, redis_1.getRedis)();
    const now = Date.now();
    const serverDisabled = await redis.isServerDisabled(report.serverId);
    for (const inst of report.instances) {
        // 注册或更新实例信息
        const existing = await redis.getInstance(inst.id);
        // 状态权威性规则：
        // - Agent 报告 bad → 无条件信任（硬件/APK 真的坏了）
        // - Agent 报告 idle → 但调度器侧有更高优先级的状态时，以调度器为准
        //   reserved_for_user: 用户5分钟保留期，Agent 不知道，不能被覆盖
        //   reserved/busy: 正在服务用户，Agent 也可能不知道，不能被覆盖
        // - 其他情况：信任 Agent 上报
        let effectiveStatus;
        if (inst.health === 'bad') {
            effectiveStatus = 'bad';
        }
        else if (existing &&
            (existing.status === 'reserved_for_user' ||
                existing.status === 'reserved' ||
                existing.status === 'busy')) {
            // 调度器侧状态优先，不被 Agent 的 idle 覆盖
            effectiveStatus = existing.status;
        }
        else {
            effectiveStatus = inst.agentStatus;
        }
        const statusSince = existing?.status === effectiveStatus
            ? existing.statusSince ?? now
            : now;
        const agentStatusSince = typeof inst.statusChangedAt === 'number'
            ? inst.statusChangedAt
            : existing?.agentStatus === inst.agentStatus
                ? existing.agentStatusSince ?? now
                : now;
        const instanceInfo = {
            id: inst.id,
            serverId: report.serverId,
            serverIp: report.serverIp,
            publicWsHost: report.publicWsHost,
            publicWsScheme: report.publicWsScheme,
            disabled: existing?.disabled ?? false,
            wsPort: inst.wsPort,
            agentPort: report.agentPort,
            status: effectiveStatus,
            statusSince,
            sessionId: existing?.sessionId,
            // 保留调度器侧的预留字段，防止 Agent 上报时丢失
            reservedForDeviceId: existing?.reservedForDeviceId,
            reservedUntil: existing?.reservedUntil,
            lastHealthAt: now,
            health: inst.health,
            cpu: inst.cpu,
            memory: inst.memory,
            agentStatus: inst.agentStatus,
            agentStatusSince,
            failureCount: inst.failureCount,
            lastError: inst.lastError,
        };
        await redis.setInstance(instanceInfo);
        // idle 实例加入空闲池的条件：
        // 1. Agent 报告 idle + health=ok
        // 2. 调度器未将其分配给任何 session（无 sessionId）
        // 3. 未处于调度器侧的保留/使用状态
        const schedulerOwnedStatuses = ['reserved_for_user', 'reserved', 'busy'];
        const isSchedulerOwned = existing ? schedulerOwnedStatuses.includes(existing.status) : false;
        if (serverDisabled || instanceInfo.disabled) {
            await redis.removeFromIdlePool(inst.id);
            continue;
        }
        if (effectiveStatus === 'idle' && inst.health === 'ok' && !existing?.sessionId && !isSchedulerOwned) {
            await redis.addToIdlePool(inst.id);
        }
        else {
            await redis.removeFromIdlePool(inst.id);
        }
    }
}
async function isInstanceBlocked(instance) {
    if (!instance)
        return true;
    const redis = (0, redis_1.getRedis)();
    if (instance.disabled)
        return true;
    return redis.isServerDisabled(instance.serverId);
}
async function isInstanceSchedulableForReserve(instance) {
    if (!instance)
        return false;
    if (await isInstanceBlocked(instance))
        return false;
    return instance.status === 'idle' && instance.health === 'ok' && !instance.sessionId;
}
async function isInstanceUsableForCatalog(instance) {
    if (!instance)
        return false;
    if (await isInstanceBlocked(instance))
        return false;
    if (instance.status !== 'idle' && instance.status !== 'reserved_for_user')
        return false;
    if (instance.health === 'bad')
        return false;
    return true;
}
async function syncInstanceIdlePool(instanceId) {
    const redis = (0, redis_1.getRedis)();
    const instance = await redis.getInstance(instanceId);
    const schedulable = await isInstanceSchedulableForReserve(instance);
    if (schedulable) {
        await redis.addToIdlePool(instanceId);
        return true;
    }
    await redis.removeFromIdlePool(instanceId);
    return false;
}
