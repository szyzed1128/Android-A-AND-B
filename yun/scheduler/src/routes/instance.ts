/**
 * 实例查询路由 + Agent健康上报入口
 */

import { Router, Request, Response } from 'express';
import { getRedis } from '../services/redis';
import { handleAgentHealthReport } from '../services/scheduler';

const router = Router();

/**
 * GET /instance/:id/status
 * 查询单实例状态
 */
router.get('/:id/status', async (req: Request, res: Response) => {
  try {
    const redis = getRedis();
    const instance = await redis.getInstance(req.params.id);
    if (!instance) return res.status(404).json({ success: false, error: '实例不存在' });
    return res.json({ success: true, data: instance });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

/**
 * GET /instance/pool/stats
 * 查询实例池状态（运维用）
 */
router.get('/pool/stats', async (req: Request, res: Response) => {
  try {
    const redis = getRedis();
    const idleCount = await redis.getIdlePoolSize();
    const allIds = await redis.getAllInstanceIds();

    const stats = { total: allIds.length, idle: idleCount, busy: 0, bad: 0, reserved: 0, probing: 0 };
    for (const id of allIds) {
      const inst = await redis.getInstance(id);
      if (inst?.status === 'busy') stats.busy++;
      else if (inst?.status === 'bad') stats.bad++;
      else if (inst?.status === 'probing') stats.probing++;
      else if (inst?.status === 'reserved' || inst?.status === 'reserved_for_user') stats.reserved++;
    }

    return res.json({ success: true, data: stats });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

export default router;
