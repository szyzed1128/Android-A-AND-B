/**
 * 调度后端入口
 */

import express from 'express';
import fs from 'fs';
import path from 'path';
import { initRedis } from './services/redis';
import { startHeartbeatMonitor, handleAgentHealthReport } from './services/scheduler';
import sessionRouter from './routes/session';
import instanceRouter from './routes/instance';
import dashboardRouter from './routes/dashboard';
import dashboardAdminRouter from './routes/dashboardAdmin';
import catalogRouter from './routes/catalog';

const app = express();
const PORT = process.env.PORT ? parseInt(process.env.PORT) : 3000;
const REDIS_URL = process.env.REDIS_URL || 'redis://localhost:6379';

app.use(express.json());

// 请求日志
app.use((req, res, next) => {
  console.log(`[HTTP] ${req.method} ${req.path}`);
  next();
});

// 路由
app.use('/session', sessionRouter);
app.use('/instance', instanceRouter);
app.use('/catalog', catalogRouter);
app.use('/dashboard/api', dashboardRouter);
app.use('/dashboard/api/admin', dashboardAdminRouter);

const dashboardDir = resolveDashboardDir();
app.use('/dashboard', express.static(dashboardDir));

// Agent 健康上报入口
app.post('/agent/health', async (req, res) => {
  try {
    await handleAgentHealthReport(req.body);
    res.json({ success: true });
  } catch (err: any) {
    console.error('[Scheduler] 处理健康上报失败:', err.message);
    res.status(500).json({ success: false, error: err.message });
  }
});

// 调度后端自身健康检查
app.get('/health', async (req, res) => {
  const redis = await import('./services/redis').then(m => m.getRedis());
  const redisOk = await redis.ping();
  res.json({ status: 'ok', redis: redisOk ? 'ok' : 'error', ts: Date.now() });
});

// 启动
async function main() {
  try {
    await initRedis(REDIS_URL);
    startHeartbeatMonitor();

    app.listen(PORT, () => {
      console.log(`[Scheduler] 调度服务已启动 port=${PORT}`);
      console.log(`[Scheduler] Redis=${REDIS_URL}`);
      console.log(`[Scheduler] DashboardDir=${dashboardDir}`);
    });
  } catch (err: any) {
    console.error('[Scheduler] 启动失败:', err.message);
    process.exit(1);
  }
}

function resolveDashboardDir(): string {
  const configured = (process.env.DASHBOARD_WEB_DIR || '').trim();
  const candidates = Array.from(new Set([
    configured || null,
    path.resolve(process.cwd(), '../dashboard-web'),
    path.resolve(__dirname, '../../dashboard-web'),
  ].filter((item): item is string => Boolean(item))));

  for (const candidate of candidates) {
    const indexFile = path.join(candidate, 'index.html');
    if (fs.existsSync(indexFile)) {
      return candidate;
    }
  }

  throw new Error(`[Scheduler] 未找到 dashboard-web 目录，候选路径: ${candidates.join(', ')}`);
}

main();
