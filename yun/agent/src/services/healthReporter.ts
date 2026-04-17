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
      const publicWsHost = await resolvePublicWsHost(config);
      const serverIp = await resolveServerIp(config);
      const statuses = getAllInstanceStatuses();
      const instances = statuses.map(s => ({
        id: s.id,
        wsPort: s.wsPort,
        agentStatus: s.status,
        health: s.status === 'bad' ? 'bad' : 'ok',
        failureCount: s.failureCount,
        lastError: s.lastError,
        statusChangedAt: s.statusChangedAt,
        ...getResourceUsage(),
      }));

      await axios.post(`${config.schedulerUrl}/agent/health`, {
        serverId: config.serverId,
        serverIp,
        publicWsHost,
        publicWsScheme: config.publicWsScheme,
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

let cachedPublicWsHost: string | null = null;
let cachedServerIp: string | null = null;

async function resolvePublicWsHost(config: AgentConfig): Promise<string> {
  if (config.publicWsHost) {
    cachedPublicWsHost = config.publicWsHost;
    return config.publicWsHost;
  }

  if (cachedPublicWsHost) {
    return cachedPublicWsHost;
  }

  const candidates = [
    'http://100.100.100.200/latest/meta-data/eipv4',
    'http://100.100.100.200/latest/meta-data/public-ipv4',
    'http://169.254.169.254/latest/meta-data/public-ipv4',
  ];

  for (const url of candidates) {
    try {
      const res = await axios.get(url, { timeout: 1200, responseType: 'text' });
      const value = String(res.data || '').trim();
      if (value) {
        cachedPublicWsHost = value;
        return value;
      }
    } catch {
      // ignore and try next candidate
    }
  }

  const fallback = getServerIp();
  cachedPublicWsHost = fallback;
  return fallback;
}

async function resolveServerIp(config: AgentConfig): Promise<string> {
  if (config.serverIpOverride) {
    cachedServerIp = config.serverIpOverride;
    return config.serverIpOverride;
  }

  if (cachedServerIp) {
    return cachedServerIp;
  }

  const candidates = [
    'http://100.100.100.200/latest/meta-data/inner-ipv4',
    'http://169.254.169.254/latest/meta-data/local-ipv4',
  ];

  for (const url of candidates) {
    try {
      const res = await axios.get(url, { timeout: 1200, responseType: 'text' });
      const value = String(res.data || '').trim();
      if (value) {
        cachedServerIp = value;
        return value;
      }
    } catch {
      // ignore and try next candidate
    }
  }

  const fallback = getServerIp();
  cachedServerIp = fallback;
  return fallback;
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
