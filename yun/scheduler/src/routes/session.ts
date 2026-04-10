/**
 * 会话管理路由
 * B端调用的所有 /session/* 接口
 */

import { Router, Request, Response } from 'express';
import { initSession, validateSession, updateHeartbeat } from '../services/tokenService';
import { reserveInstance, disconnectSession, releaseInstance } from '../services/scheduler';
import { getRedis } from '../services/redis';
import { ApiResponse, ReserveInstanceResponse } from '../types';

const router = Router();

/**
 * POST /session/init
 * App 启动时建立调度会话
 */
router.post('/init', async (req: Request, res: Response) => {
  try {
    const { deviceId } = req.body;
    if (!deviceId || typeof deviceId !== 'string') {
      return res.status(400).json({ success: false, error: 'deviceId 必填' });
    }

    const session = await initSession(deviceId);
    const resp: ApiResponse<{ sessionId: string }> = {
      success: true,
      data: { sessionId: session.sessionId },
    };
    return res.json(resp);
  } catch (err: any) {
    console.error('[Session] init 失败:', err.message);
    return res.status(500).json({ success: false, error: err.message });
  }
});

/**
 * PUT /session/:id/device
 * 保存蓝牙设备信息
 */
router.put('/:id/device', async (req: Request, res: Response) => {
  try {
    const session = await validateSession(req.params.id);
    if (!session) return res.status(404).json({ success: false, error: '会话不存在' });

    const { btAddress, btProtocol, btName } = req.body;
    if (!btAddress || !btProtocol) {
      return res.status(400).json({ success: false, error: 'btAddress/btProtocol 必填' });
    }

    const redis = getRedis();
    await redis.updateSession(session.sessionId, { btAddress, btProtocol });

    return res.json({ success: true });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

/**
 * PUT /session/:id/car
 * 保存车型配置
 */
router.put('/:id/car', async (req: Request, res: Response) => {
  try {
    const session = await validateSession(req.params.id);
    if (!session) return res.status(404).json({ success: false, error: '会话不存在' });

    const { carBrand, carModel } = req.body;
    if (!carBrand || !carModel) {
      return res.status(400).json({ success: false, error: 'carBrand/carModel 必填' });
    }

    const redis = getRedis();
    await redis.updateSession(session.sessionId, { carBrand, carModel });

    return res.json({ success: true });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

/**
 * POST /session/:id/reserve
 * 准备连接：分配实例 + 应用车型配置
 * 返回 wsUrl（只有成功才返回）
 */
router.post('/:id/reserve', async (req: Request, res: Response) => {
  try {
    const session = await validateSession(req.params.id);
    if (!session) return res.status(404).json({ success: false, error: '会话不存在' });

    if (!session.carBrand || !session.carModel) {
      return res.status(400).json({ success: false, error: '请先选择车型配置' });
    }
    if (!session.btAddress) {
      return res.status(400).json({ success: false, error: '请先选择蓝牙设备' });
    }

    const { wsUrl, instanceId } = await reserveInstance(session);

    const resp: ApiResponse<ReserveInstanceResponse> = {
      success: true,
      data: { wsUrl, instanceId },
    };
    return res.json(resp);
  } catch (err: any) {
    console.error('[Session] reserve 失败:', err.message);
    return res.status(503).json({ success: false, error: err.message });
  }
});

/**
 * POST /session/:id/heartbeat
 * B端定期发送心跳（每30秒）
 */
router.post('/:id/heartbeat', async (req: Request, res: Response) => {
  try {
    const ok = await updateHeartbeat(req.params.id);
    if (!ok) return res.status(404).json({ success: false, error: '会话不存在' });
    return res.json({ success: true });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

/**
 * POST /session/:id/disconnect
 * 用户主动断开：实例保留5分钟
 */
router.post('/:id/disconnect', async (req: Request, res: Response) => {
  try {
    const session = await validateSession(req.params.id);
    if (!session) return res.status(404).json({ success: false, error: '会话不存在' });

    await disconnectSession(session);
    return res.json({ success: true });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

/**
 * PUT /session/:id/status
 * 更新会话运行状态（running等）
 */
router.put('/:id/status', async (req: Request, res: Response) => {
  try {
    const session = await validateSession(req.params.id);
    if (!session) return res.status(404).json({ success: false, error: '会话不存在' });

    const { status } = req.body;
    const redis = getRedis();

    if (status === 'running' && session.instanceId) {
      await redis.updateSession(session.sessionId, { status: 'running' });
      await redis.updateInstanceStatus(session.instanceId, 'busy');
    }

    return res.json({ success: true });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

export default router;
