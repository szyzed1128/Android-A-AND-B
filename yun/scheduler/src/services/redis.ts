/**
 * Redis 操作封装
 * 所有 Redis Key 和 TTL 集中管理
 */

import Redis from 'ioredis';
import { InstanceInfo, SessionInfo, InstanceStatus, SessionStatus } from '../types';

// Redis Key 前缀
const KEYS = {
  instance: (id: string) => `instance:${id}`,
  session: (id: string) => `session:${id}`,
  device: (deviceId: string) => `device:${deviceId}`,
  idlePool: 'instances:idle',         // ZSet：空闲实例池
  allInstances: 'instances:all',      // Set：所有已注册实例ID
};

// TTL 配置（秒）
const TTL = {
  session: 60 * 60 * 24,   // 会话最长保留24小时
  device: 60 * 60 * 24,    // deviceId→sessionId 映射保留24小时
};

export class RedisService {
  private client: Redis;

  constructor(redisUrl: string = 'redis://localhost:6379') {
    this.client = new Redis(redisUrl, {
      lazyConnect: true,
      retryStrategy: (times) => Math.min(times * 100, 3000),
    });
    this.client.on('error', (err) => {
      console.error('[Redis] 连接错误:', err.message);
    });
  }

  async connect(): Promise<void> {
    await this.client.connect();
    console.log('[Redis] 连接成功');
  }

  // ==================== 实例操作 ====================

  async setInstance(instance: InstanceInfo): Promise<void> {
    const key = KEYS.instance(instance.id);
    await this.client.hset(key, this.flatten(instance));
    await this.client.sadd(KEYS.allInstances, instance.id);
  }

  async getInstance(id: string): Promise<InstanceInfo | null> {
    const data = await this.client.hgetall(KEYS.instance(id));
    if (!data || Object.keys(data).length === 0) return null;
    return this.parseInstance(data);
  }

  async updateInstanceStatus(
    id: string,
    status: InstanceStatus,
    extra?: Partial<InstanceInfo>,
    clearFields?: Array<keyof InstanceInfo>
  ): Promise<void> {
    const key = KEYS.instance(id);
    const updates: Record<string, string> = { status };
    if (extra) {
      Object.entries(extra).forEach(([k, v]) => {
        if (v !== undefined && v !== null) updates[k] = String(v);
      });
    }
    await this.client.hset(key, updates);
    // 明确删除需要清除的字段（Redis HDEL），避免脏数据残留
    if (clearFields && clearFields.length > 0) {
      await this.client.hdel(key, ...clearFields);
    }
  }

  async getAllInstanceIds(): Promise<string[]> {
    return this.client.smembers(KEYS.allInstances);
  }

  // ==================== 空闲池操作 ====================

  async addToIdlePool(instanceId: string): Promise<void> {
    await this.client.zadd(KEYS.idlePool, Date.now(), instanceId);
  }

  async removeFromIdlePool(instanceId: string): Promise<void> {
    await this.client.zrem(KEYS.idlePool, instanceId);
  }

  /** 原子性：从空闲池取出一个实例（ZPOPMIN） */
  async popIdleInstance(): Promise<string | null> {
    const result = await this.client.zpopmin(KEYS.idlePool, 1);
    if (!result || result.length === 0) return null;
    return result[0] as string; // [member, score, member, score...]
  }

  async getIdlePoolSize(): Promise<number> {
    return this.client.zcard(KEYS.idlePool);
  }

  // ==================== 会话操作 ====================

  async setSession(session: SessionInfo): Promise<void> {
    const key = KEYS.session(session.sessionId);
    await this.client.hset(key, this.flatten(session));
    await this.client.expire(key, TTL.session);
  }

  async getSession(sessionId: string): Promise<SessionInfo | null> {
    const data = await this.client.hgetall(KEYS.session(sessionId));
    if (!data || Object.keys(data).length === 0) return null;
    return this.parseSession(data);
  }

  async updateSession(
    sessionId: string,
    updates: Partial<SessionInfo>,
    clearFields?: Array<keyof SessionInfo>
  ): Promise<void> {
    const flat: Record<string, string> = {};
    Object.entries(updates).forEach(([k, v]) => {
      if (v !== undefined && v !== null) flat[k] = String(v);
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

  async deleteSession(sessionId: string): Promise<void> {
    await this.client.del(KEYS.session(sessionId));
  }

  // ==================== DeviceId 映射 ====================

  async getSessionIdByDevice(deviceId: string): Promise<string | null> {
    return this.client.get(KEYS.device(deviceId));
  }

  async setDeviceSession(deviceId: string, sessionId: string): Promise<void> {
    await this.client.set(KEYS.device(deviceId), sessionId, 'EX', TTL.device);
  }

  async deleteDeviceSession(deviceId: string): Promise<void> {
    await this.client.del(KEYS.device(deviceId));
  }

  // ==================== 工具方法 ====================

  /** 扁平化对象（去掉 undefined 字段）为 Redis Hash 格式 */
  private flatten(obj: Record<string, any>): Record<string, string> {
    const result: Record<string, string> = {};
    for (const [k, v] of Object.entries(obj)) {
      if (v !== undefined && v !== null) {
        result[k] = String(v);
      }
    }
    return result;
  }

  private parseInstance(data: Record<string, string>): InstanceInfo {
    return {
      id: data.id,
      serverId: data.serverId,
      serverIp: data.serverIp,
      wsPort: parseInt(data.wsPort),
      agentPort: parseInt(data.agentPort),
      status: data.status as InstanceStatus,
      sessionId: data.sessionId || undefined,
      lastHealthAt: data.lastHealthAt ? parseInt(data.lastHealthAt) : undefined,
      reservedForDeviceId: data.reservedForDeviceId || undefined,
      reservedUntil: data.reservedUntil ? parseInt(data.reservedUntil) : undefined,
    };
  }

  private parseSession(data: Record<string, string>): SessionInfo {
    return {
      sessionId: data.sessionId,
      deviceId: data.deviceId,
      userId: data.userId || undefined,
      instanceId: data.instanceId || undefined,
      status: data.status as SessionStatus,
      carBrand: data.carBrand || undefined,
      carModel: data.carModel || undefined,
      btAddress: data.btAddress || undefined,
      btProtocol: (data.btProtocol as any) || undefined,
      lastHeartbeatAt: data.lastHeartbeatAt ? parseInt(data.lastHeartbeatAt) : undefined,
      createdAt: parseInt(data.createdAt),
    };
  }

  async ping(): Promise<boolean> {
    try {
      await this.client.ping();
      return true;
    } catch {
      return false;
    }
  }
}

// 单例
let _redis: RedisService | null = null;

export function getRedis(): RedisService {
  if (!_redis) throw new Error('Redis 未初始化，请先调用 initRedis()');
  return _redis;
}

export async function initRedis(url?: string): Promise<RedisService> {
  _redis = new RedisService(url);
  await _redis.connect();
  return _redis;
}
