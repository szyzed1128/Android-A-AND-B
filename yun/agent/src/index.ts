/**
 * Agent 入口
 */

import express from 'express';
import config from './config';
import { initInstances, getAllInstanceStatuses } from './services/instanceManager';
import { startHealthReporter } from './services/healthReporter';
import instanceRouter from './routes/instance';

const app = express();
app.use(express.json());

// 日志
app.use((req, res, next) => {
  console.log(`[HTTP] ${req.method} ${req.path}`);
  next();
});

// 路由
app.use('/instance', instanceRouter);

// Agent 自身健康检查
app.get('/health', (req, res) => {
  const statuses = getAllInstanceStatuses();
  res.json({
    status: 'ok',
    serverId: config.serverId,
    instances: statuses,
    ts: Date.now(),
  });
});

// 启动
function main() {
  // 初始化所有实例（触发就绪流程）
  initInstances(config.instances);

  // 启动健康上报
  startHealthReporter(config);

  app.listen(config.agentPort, () => {
    console.log(`[Agent] 已启动 serverId=${config.serverId} port=${config.agentPort}`);
    console.log(`[Agent] 管理 ${config.instances.length} 个实例`);
    console.log(`[Agent] 调度后端=${config.schedulerUrl}`);
  });
}

main();
