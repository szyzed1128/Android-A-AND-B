import { execFile } from 'child_process';
import { promisify } from 'util';
import { Router, Request, Response } from 'express';
import { createAgentClient } from '../services/agentClient';
import { getDashboardInstances } from '../services/dashboardService';
import { getRedis } from '../services/redis';
import { syncInstanceIdlePool } from '../services/scheduler';
import { InstanceInfo } from '../types';

const execFileAsync = promisify(execFile);
const router = Router();

router.post('/instances/:id/reset', async (req: Request, res: Response) => {
  try {
    const instance = await mustGetInstance(req.params.id);
    ensureNoActiveSession(instance, '实例仍绑定会话，不能直接重置');

    const redis = getRedis();
    await redis.removeFromIdlePool(instance.id);
    await redis.updateInstanceStatus(instance.id, 'probing', {}, ['reservedForDeviceId', 'reservedUntil']);

    const agent = createAgentClient(instance.serverIp, instance.agentPort);
    const result = await agent.reset(instance.id);
    if (!result.success) {
      throw new Error(result.error || '实例重置失败');
    }

    return res.json({ success: true, message: '实例重置已触发' });
  } catch (err: any) {
    return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
  }
});

router.post('/instances/:id/restart-runtime', async (req: Request, res: Response) => {
  try {
    const instance = await mustGetInstance(req.params.id);
    ensureNoActiveSession(instance, '实例仍绑定会话，不能直接重启运行时');

    const redis = getRedis();
    await redis.removeFromIdlePool(instance.id);
    await redis.updateInstanceStatus(instance.id, 'probing', {}, ['reservedForDeviceId', 'reservedUntil']);

    const agent = createAgentClient(instance.serverIp, instance.agentPort);
    const result = await agent.restartRuntime(instance.id);
    if (!result.success) {
      throw new Error(result.error || '运行时重启失败');
    }

    return res.json({ success: true, message: result.message || '运行时重启完成' });
  } catch (err: any) {
    return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
  }
});

router.post('/instances/:id/disable', async (req: Request, res: Response) => {
  try {
    const instance = await mustGetInstance(req.params.id);
    const redis = getRedis();
    await redis.setInstanceDisabled(instance.id, true);
    await redis.removeFromIdlePool(instance.id);

    return res.json({ success: true, message: `实例 ${instance.id} 已摘除` });
  } catch (err: any) {
    return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
  }
});

router.post('/instances/:id/enable', async (req: Request, res: Response) => {
  try {
    const instance = await mustGetInstance(req.params.id);
    const redis = getRedis();
    await redis.setInstanceDisabled(instance.id, false);
    await syncInstanceIdlePool(instance.id);

    return res.json({ success: true, message: `实例 ${instance.id} 已恢复调度` });
  } catch (err: any) {
    return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
  }
});

router.delete('/instances/:id', async (req: Request, res: Response) => {
  try {
    const instance = await mustGetInstance(req.params.id);
    ensureNoActiveSession(instance, '实例仍绑定会话，不能移除');

    const redis = getRedis();
    await redis.setInstanceDisabled(instance.id, false);
    await redis.removeInstance(instance.id);

    return res.json({ success: true, message: '实例已从调度层移除' });
  } catch (err: any) {
    return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
  }
});

router.post('/instances/restart-bad', async (_req: Request, res: Response) => {
  try {
    const redis = getRedis();
    const instances = await getDashboardInstances();
    const badInstances = instances.filter(item => item.status === 'bad');
    const results: Array<{ id: string; success: boolean; error?: string }> = [];

    for (const instance of badInstances) {
      try {
        ensureNoActiveSession(instance, '实例仍绑定会话，不能直接重启运行时');
        await redis.removeFromIdlePool(instance.id);
        await redis.updateInstanceStatus(instance.id, 'probing', {}, ['reservedForDeviceId', 'reservedUntil']);
        const agent = createAgentClient(instance.serverIp, instance.agentPort);
        const result = await agent.restartRuntime(instance.id);
        results.push({
          id: instance.id,
          success: Boolean(result.success),
          error: result.error,
        });
      } catch (err: any) {
        results.push({ id: instance.id, success: false, error: err.message });
      }
    }

    return res.json({ success: true, data: { total: badInstances.length, results } });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

router.get('/instances/:id/logs', async (req: Request, res: Response) => {
  try {
    const instance = await mustGetInstance(req.params.id);
    const agent = createAgentClient(instance.serverIp, instance.agentPort);
    const runtime = await agent.getInstanceLogs(instance.id);
    const scheduler = await buildSchedulerLog(instance.id);

    return res.json({ success: true, data: { instance, scheduler, runtime } });
  } catch (err: any) {
    return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
  }
});

router.post('/servers/:serverId/disable', async (req: Request, res: Response) => {
  try {
    const redis = getRedis();
    const serverId = req.params.serverId;
    await redis.setServerDisabled(serverId, true);

    const instances = (await getDashboardInstances()).filter(item => item.serverId === serverId);
    for (const instance of instances) {
      await redis.removeFromIdlePool(instance.id);
    }

    return res.json({ success: true, message: `服务器 ${serverId} 已摘除` });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

router.post('/servers/:serverId/enable', async (req: Request, res: Response) => {
  try {
    const redis = getRedis();
    const serverId = req.params.serverId;
    await redis.setServerDisabled(serverId, false);

    const instances = (await getDashboardInstances()).filter(item => item.serverId === serverId);
    for (const instance of instances) {
      await syncInstanceIdlePool(instance.id);
    }

    return res.json({ success: true, message: `服务器 ${serverId} 已恢复调度` });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

async function mustGetInstance(id: string): Promise<InstanceInfo> {
  const redis = getRedis();
  const instance = await redis.getInstance(id);
  if (instance) {
    return instance;
  }

  const err = new Error(`实例 ${id} 不存在`);
  (err as any).statusCode = 404;
  throw err;
}

function ensureNoActiveSession(instance: InstanceInfo, message: string): void {
  if (!instance.sessionId) return;
  const err = new Error(message);
  (err as any).statusCode = 409;
  throw err;
}

function httpStatusFromError(err: any): number {
  return typeof err?.statusCode === 'number' ? err.statusCode : 500;
}

async function buildSchedulerLog(instanceId: string): Promise<{ title: string; content: string }> {
  const { stdout } = await execFileAsync('/bin/bash', [
    '-lc',
    `journalctl -u obd-scheduler -n 300 --no-pager | grep '${instanceId}' | tail -120 || true`,
  ], {
    timeout: 20000,
    maxBuffer: 1024 * 1024 * 4,
  });

  return {
    title: 'SchedulerLog',
    content: stdout.trim(),
  };
}

export default router;
