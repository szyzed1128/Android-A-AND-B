/**
 * 实例操作路由
 * 被调度后端 HTTP 调用
 */

import { Router, Request, Response } from 'express';
import { getInstanceStatus, triggerReset, getAllInstanceStatuses } from '../services/instanceManager';
import { applyCarProfile } from '../services/apkProbe';
import config from '../config';

const router = Router();

/**
 * POST /instance/:id/applyCar
 * 应用用户选择的车型配置
 * 调度后端在分配实例给用户时调用
 */
router.post('/:id/applyCar', async (req: Request, res: Response) => {
  const { id } = req.params;
  const { carBrand, carModel } = req.body;

  if (!carBrand || !carModel) {
    return res.status(400).json({ success: false, error: 'carBrand/carModel 必填' });
  }

  const instanceConfig = config.instances.find(i => i.id === id);
  if (!instanceConfig) {
    return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
  }

  const status = getInstanceStatus(id);
  if (status !== 'idle') {
    return res.status(409).json({
      success: false,
      error: `实例 ${id} 状态为 ${status}，无法应用配置`,
    });
  }

  console.log(`[Agent] applyCar ${id}: ${carBrand}/${carModel}`);
  const result = await applyCarProfile(instanceConfig, carBrand, carModel);

  return res.json(result);
});

/**
 * POST /instance/:id/reset
 * 触发实例就绪流程（重置后重新探针直到idle）
 * 调度后端释放实例时调用
 */
router.post('/:id/reset', async (req: Request, res: Response) => {
  const { id } = req.params;

  const instanceConfig = config.instances.find(i => i.id === id);
  if (!instanceConfig) {
    return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
  }

  console.log(`[Agent] 触发重置 ${id}`);
  triggerReset(id);  // 异步执行，立即返回

  return res.json({ success: true, message: '重置已触发，正在后台执行' });
});

/**
 * GET /instance/:id/status
 * 查询单实例状态
 */
router.get('/:id/status', (req: Request, res: Response) => {
  const { id } = req.params;
  const status = getInstanceStatus(id);

  if (status === null) {
    return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
  }

  return res.json({
    success: true,
    data: { id, status, health: status === 'bad' ? 'bad' : 'ok' },
  });
});

/**
 * GET /instance/all/status
 * 查询所有实例状态（运维用）
 */
router.get('/all/status', (req: Request, res: Response) => {
  const statuses = getAllInstanceStatuses();
  return res.json({ success: true, data: statuses });
});

export default router;
