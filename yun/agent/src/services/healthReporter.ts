/**
 * 健康状态主动上报
 * 每60秒向调度后端推送本机所有实例状态
 */

import axios from 'axios';
import { execSync } from 'child_process';
import { getAllInstanceStatuses, RuntimeStatus } from './instanceManager';
import { AgentConfig } from '../config';

export function startHealthReporter(config: AgentConfig): void {
  const report = async () => {
    try {
      const statuses = getAllInstanceStatuses();
      const instances = statuses.map(s => ({
        id: s.id,
        wsPort: s.wsPort,
        status: s.status,
        health: s.status === 'bad' ? 'bad' : 'ok',
        ...getResourceUsage(),
      }));

      await axios.post(`${config.schedulerUrl}/agent/health`, {
        serverId: config.serverId,
        serverIp: getServerIp(),
        agentPort: config.agentPort,
        instances,
        reportedAt: Date.now(),
      }, { timeout: 5000 });

    } catch (err: any) {
      console.error('[HealthReporter] 上报失败:', err.message);
    }
  };

  // 启动时立即上报一次
  report();
  setInterval(report, config.healthReportIntervalMs);
  console.log(`[HealthReporter] 已启动，每${config.healthReportIntervalMs / 1000}秒上报`);
}

function getResourceUsage(): { cpu?: number; memory?: number } {
  try {
    // 简单获取系统负载
    const loadAvg = parseFloat(
      execSync("cat /proc/loadavg 2>/dev/null | awk '{print $1}'").toString().trim()
    );
    const memInfo = execSync("free -m 2>/dev/null | awk 'NR==2{print $3}'").toString().trim();
    return {
      cpu: Math.round(loadAvg * 100) / 100,
      memory: parseInt(memInfo) || undefined,
    };
  } catch {
    return {};
  }
}

function getServerIp(): string {
  try {
    return execSync("hostname -I 2>/dev/null | awk '{print $1}'").toString().trim() || 'unknown';
  } catch {
    return 'unknown';
  }
}
