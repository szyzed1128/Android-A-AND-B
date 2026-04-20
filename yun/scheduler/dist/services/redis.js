"use strict";
/**
 * Redis 操作封装
 * 所有 Redis Key 和 TTL 集中管理
 */
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.RedisService = void 0;
exports.getRedis = getRedis;
exports.initRedis = initRedis;
const ioredis_1 = __importDefault(require("ioredis"));
// Redis Key 前缀
const KEYS = {
    instance: (id) => `instance:${id}`,
    session: (id) => `session:${id}`,
    device: (deviceId) => `device:${deviceId}`,
    catalogBrands: 'catalog:brands',
    catalogProfiles: (brand) => `catalog:profiles:${encodeURIComponent(brand)}`,
    idlePool: 'instances:idle', // ZSet：空闲实例池
    allInstances: 'instances:all', // Set：所有已注册实例ID
    disabledServers: 'servers:disabled',
    disabledInstances: 'instances:disabled',
};
// TTL 配置（秒）
const TTL = {
    session: 60 * 60 * 24, // 会话最长保留24小时
    device: 60 * 60 * 24, // deviceId→sessionId 映射保留24小时
    catalogRetention: 60 * 60 * 24 * 7, // 目录缓存保留7天，调度层自己判断新鲜度
};
class RedisService {
    constructor(redisUrl = 'redis://localhost:6379') {
        this.client = new ioredis_1.default(redisUrl, {
            lazyConnect: true,
            retryStrategy: (times) => Math.min(times * 100, 3000),
        });
        this.client.on('error', (err) => {
            console.error('[Redis] 连接错误:', err.message);
        });
    }
    async connect() {
        await this.client.connect();
        console.log('[Redis] 连接成功');
    }
    // ==================== 实例操作 ====================
    async setInstance(instance) {
        const key = KEYS.instance(instance.id);
        await this.client.hset(key, this.flatten(instance));
        await this.client.sadd(KEYS.allInstances, instance.id);
    }
    async getInstance(id) {
        const data = await this.client.hgetall(KEYS.instance(id));
        if (!data || Object.keys(data).length === 0)
            return null;
        return this.parseInstance(data);
    }
    async updateInstanceStatus(id, status, extra, clearFields) {
        const key = KEYS.instance(id);
        const updates = { status };
        if (extra) {
            Object.entries(extra).forEach(([k, v]) => {
                if (v !== undefined && v !== null)
                    updates[k] = String(v);
            });
        }
        await this.client.hset(key, updates);
        // 明确删除需要清除的字段（Redis HDEL），避免脏数据残留
        if (clearFields && clearFields.length > 0) {
            await this.client.hdel(key, ...clearFields);
        }
    }
    async getAllInstanceIds() {
        return this.client.smembers(KEYS.allInstances);
    }
    async removeInstance(id) {
        await this.client.zrem(KEYS.idlePool, id);
        await this.client.srem(KEYS.allInstances, id);
        await this.client.del(KEYS.instance(id));
    }
    async getAllSessionIds() {
        const ids = [];
        let cursor = '0';
        do {
            const [nextCursor, keys] = await this.client.scan(cursor, 'MATCH', KEYS.session('*'), 'COUNT', '200');
            cursor = nextCursor;
            for (const key of keys) {
                ids.push(key.replace(/^session:/, ''));
            }
        } while (cursor !== '0');
        return ids;
    }
    // ==================== 空闲池操作 ====================
    async addToIdlePool(instanceId) {
        await this.client.zadd(KEYS.idlePool, Date.now(), instanceId);
    }
    async removeFromIdlePool(instanceId) {
        await this.client.zrem(KEYS.idlePool, instanceId);
    }
    /** 原子性：从空闲池取出一个实例（ZPOPMIN） */
    async popIdleInstance() {
        const result = await this.client.zpopmin(KEYS.idlePool, 1);
        if (!result || result.length === 0)
            return null;
        return result[0]; // [member, score, member, score...]
    }
    async getIdlePoolSize() {
        return this.client.zcard(KEYS.idlePool);
    }
    async getDisabledServerIds() {
        return this.client.smembers(KEYS.disabledServers);
    }
    async isServerDisabled(serverId) {
        const result = await this.client.sismember(KEYS.disabledServers, serverId);
        return result === 1;
    }
    async setServerDisabled(serverId, disabled) {
        if (disabled) {
            await this.client.sadd(KEYS.disabledServers, serverId);
            return;
        }
        await this.client.srem(KEYS.disabledServers, serverId);
    }
    async getDisabledInstanceIds() {
        return this.client.smembers(KEYS.disabledInstances);
    }
    async isInstanceDisabled(instanceId) {
        const result = await this.client.sismember(KEYS.disabledInstances, instanceId);
        return result === 1;
    }
    async setInstanceDisabled(instanceId, disabled) {
        if (disabled) {
            await this.client.sadd(KEYS.disabledInstances, instanceId);
            await this.client.hset(KEYS.instance(instanceId), { disabled: 'true' });
            return;
        }
        await this.client.srem(KEYS.disabledInstances, instanceId);
        await this.client.hdel(KEYS.instance(instanceId), 'disabled');
    }
    // ==================== 会话操作 ====================
    async setSession(session) {
        const key = KEYS.session(session.sessionId);
        await this.client.hset(key, this.flatten(session));
        await this.client.expire(key, TTL.session);
    }
    async getSession(sessionId) {
        const data = await this.client.hgetall(KEYS.session(sessionId));
        if (!data || Object.keys(data).length === 0)
            return null;
        return this.parseSession(data);
    }
    async updateSession(sessionId, updates, clearFields) {
        const flat = {};
        Object.entries(updates).forEach(([k, v]) => {
            if (v !== undefined && v !== null)
                flat[k] = String(v);
        });
        if (Object.keys(flat).length > 0) {
            await this.client.hset(KEYS.session(sessionId), flat);
        }
        // 明确删除需要清除的字段（Redis HDEL）
        if (clearFields && clearFields.length > 0) {
            await this.client.hdel(KEYS.session(sessionId), ...clearFields);
        }
        await this.client.expire(KEYS.session(sessionId), TTL.session);
    }
    async deleteSession(sessionId) {
        await this.client.del(KEYS.session(sessionId));
    }
    // ==================== DeviceId 映射 ====================
    async getSessionIdByDevice(deviceId) {
        return this.client.get(KEYS.device(deviceId));
    }
    async setDeviceSession(deviceId, sessionId) {
        await this.client.set(KEYS.device(deviceId), sessionId, 'EX', TTL.device);
    }
    async deleteDeviceSession(deviceId) {
        await this.client.del(KEYS.device(deviceId));
    }
    // ==================== 车辆目录缓存 ====================
    async getCatalogBrandsCache() {
        return this.getJson(KEYS.catalogBrands);
    }
    async setCatalogBrandsCache(brands) {
        await this.setJson(KEYS.catalogBrands, { data: brands, updatedAt: Date.now() }, TTL.catalogRetention);
    }
    async getCatalogProfilesCache(brand) {
        return this.getJson(KEYS.catalogProfiles(brand));
    }
    async setCatalogProfilesCache(brand, profiles) {
        await this.setJson(KEYS.catalogProfiles(brand), { data: profiles, updatedAt: Date.now() }, TTL.catalogRetention);
    }
    // ==================== 工具方法 ====================
    async getJson(key) {
        const raw = await this.client.get(key);
        if (!raw)
            return null;
        try {
            return JSON.parse(raw);
        }
        catch (err) {
            console.warn(`[Redis] JSON 缓存解析失败，已清理 key=${key}: ${err.message}`);
            await this.client.del(key);
            return null;
        }
    }
    async setJson(key, value, ttlSec) {
        await this.client.set(key, JSON.stringify(value), 'EX', ttlSec);
    }
    /** 扁平化对象（去掉 undefined 字段）为 Redis Hash 格式 */
    flatten(obj) {
        const result = {};
        for (const [k, v] of Object.entries(obj)) {
            if (v !== undefined && v !== null) {
                result[k] = String(v);
            }
        }
        return result;
    }
    parseInstance(data) {
        return {
            id: data.id,
            serverId: data.serverId,
            serverIp: data.serverIp,
            publicWsHost: data.publicWsHost || undefined,
            publicWsScheme: data.publicWsScheme || undefined,
            disabled: data.disabled === 'true' ? true : undefined,
            wsPort: parseInt(data.wsPort),
            agentPort: parseInt(data.agentPort),
            status: data.status,
            statusSince: data.statusSince ? parseInt(data.statusSince) : undefined,
            sessionId: data.sessionId || undefined,
            lastHealthAt: data.lastHealthAt ? parseInt(data.lastHealthAt) : undefined,
            reservedForDeviceId: data.reservedForDeviceId || undefined,
            reservedUntil: data.reservedUntil ? parseInt(data.reservedUntil) : undefined,
            health: data.health || undefined,
            cpu: data.cpu ? parseFloat(data.cpu) : undefined,
            memory: data.memory ? parseInt(data.memory) : undefined,
            agentStatus: data.agentStatus || undefined,
            agentStatusSince: data.agentStatusSince ? parseInt(data.agentStatusSince) : undefined,
            failureCount: data.failureCount ? parseInt(data.failureCount) : undefined,
            lastError: data.lastError || undefined,
        };
    }
    parseSession(data) {
        return {
            sessionId: data.sessionId,
            deviceId: data.deviceId,
            userId: data.userId || undefined,
            instanceId: data.instanceId || undefined,
            status: data.status,
            carBrand: data.carBrand || undefined,
            carModel: data.carModel || undefined,
            profileIndex: data.profileIndex ? parseInt(data.profileIndex) : undefined,
            profileName: data.profileName || undefined,
            btAddress: data.btAddress || undefined,
            btName: data.btName || undefined,
            btProtocol: data.btProtocol || undefined,
            lastHeartbeatAt: data.lastHeartbeatAt ? parseInt(data.lastHeartbeatAt) : undefined,
            createdAt: parseInt(data.createdAt),
        };
    }
    async ping() {
        try {
            await this.client.ping();
            return true;
        }
        catch {
            return false;
        }
    }
}
exports.RedisService = RedisService;
// 单例
let _redis = null;
function getRedis() {
    if (!_redis)
        throw new Error('Redis 未初始化，请先调用 initRedis()');
    return _redis;
}
async function initRedis(url) {
    _redis = new RedisService(url);
    await _redis.connect();
    return _redis;
}
