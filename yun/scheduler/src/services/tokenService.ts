/**
 * Token / Session 管理
 * 处理 deviceId → sessionId 的绑定与失效
 */

import { v4 as uuidv4 } from 'uuid';
import { getRedis } from './redis';
import { SessionInfo } from '../types';
import { releaseInstance } from './scheduler';

/**
 * 初始化会话
 *
 * 处理两种情况：
 * 1. 旧实例处于 reserved_for_user（用户主动断开后5分钟内重开 App）
 *    → 不释放实例，保留5分钟保留状态
 *    → 将旧 session 的车型/蓝牙配置复制到新 session（"沿用上次选择"）
 *    → 用户重新调用 reserve() 时 findReservedInstanceForUser 会找到实例复用
 *
 * 2. 旧实例处于其他状态（busy/reserved 等，即 App 在运行中被杀）
 *    → 立即释放实例，重置入池
 */
export async function initSession(deviceId: string): Promise<SessionInfo> {
  const redis = getRedis();

  const oldSessionId = await redis.getSessionIdByDevice(deviceId);

  // 保存旧 session 配置，用于5分钟重连时沿用
  let inheritedConfig: Pick<
    SessionInfo,
    'carBrand' | 'carModel' | 'profileIndex' | 'profileName' | 'btAddress' | 'btName' | 'btProtocol'
  > = {};

  if (oldSessionId) {
    console.log(`[Token] deviceId=${deviceId} 存在旧session=${oldSessionId}，处理旧状态`);
    const oldSession = await redis.getSession(oldSessionId);

    if (oldSession?.instanceId) {
      const oldInstance = await redis.getInstance(oldSession.instanceId);

      // 判断旧实例是否是"用户主动断开后的保留实例"
      const isReservedForThisUser =
        oldInstance?.status === 'reserved_for_user' &&
        oldInstance.reservedForDeviceId === deviceId &&
        oldInstance.reservedUntil !== undefined &&
        oldInstance.reservedUntil > Date.now();

      if (isReservedForThisUser) {
        // 情况1：保留实例不释放，让 findReservedInstanceForUser 在 reserve() 时复用
        // 同时保存旧配置，让新 session 继承，避免用户需要重新选车型/蓝牙
        console.log(`[Token] 旧实例 ${oldSession.instanceId} 处于5分钟保留中，不释放，继承配置`);
        inheritedConfig = {
          carBrand: oldSession.carBrand,
          carModel: oldSession.carModel,
          profileIndex: oldSession.profileIndex,
          profileName: oldSession.profileName,
          btAddress: oldSession.btAddress,
          btName: oldSession.btName,
          btProtocol: oldSession.btProtocol,
        };
      } else {
        // 情况2：App 运行中被杀/其他状态，立即释放实例
        console.log(`[Token] 旧实例 ${oldSession.instanceId} 状态=${oldInstance?.status}，立即释放`);
        await releaseInstance(oldSession.instanceId, '新session替换');
      }
    }

    await redis.deleteSession(oldSessionId);
    await redis.deleteDeviceSession(deviceId);
  }

  // 生成新 session，继承旧配置（若有）
  const sessionId = uuidv4();
  const now = Date.now();
  const session: SessionInfo = {
    sessionId,
    deviceId,
    status: 'init',
    createdAt: now,
    lastHeartbeatAt: now,
    // 沿用上次连接的车型/蓝牙选择（5分钟重连场景）
    ...inheritedConfig,
  };

  await redis.setSession(session);
  await redis.setDeviceSession(deviceId, sessionId);

  const inheritedMsg = inheritedConfig.carBrand
    ? ` (继承配置: ${inheritedConfig.carBrand}/${inheritedConfig.profileName || inheritedConfig.carModel})`
    : '';
  console.log(`[Token] 新session已创建 sessionId=${sessionId} deviceId=${deviceId}${inheritedMsg}`);
  return session;
}

/**
 * 验证 sessionId 是否有效
 */
export async function validateSession(sessionId: string): Promise<SessionInfo | null> {
  const redis = getRedis();
  return redis.getSession(sessionId);
}

/**
 * 更新心跳时间
 */
export async function updateHeartbeat(sessionId: string): Promise<boolean> {
  const redis = getRedis();
  const session = await redis.getSession(sessionId);
  if (!session) return false;

  await redis.updateSession(sessionId, { lastHeartbeatAt: Date.now() });
  return true;
}
