/**
 * 健康状态主动上报
 * 每60秒向调度后端推送本机所有实例状态
 */

import axios from 'axios';
import { execSync } from 'child_process';
import os from 'os';
import { promises as fs } from 'fs';
import { getAllInstanceStatuses, RuntimeStatus } from './instanceManager';
import { AgentConfig } from '../config';

export function startHealthReporter(config: AgentConfig): void {
  const report = async () => {
    try {
      const publicWsHost = await resolvePublicWsHost(config);
      const serverIp = await resolveServerIp(config);
      const statuses = getAllInstanceStatuses();
      const resourceUsage = await getResourceUsage();
      const instances = statuses.map(s => ({
        id: s.id,
        wsPort: s.wsPort,
        agentStatus: s.status,
        health: s.status === 'bad' ? 'bad' : 'ok',
        failureCount: s.failureCount,
        lastError: s.lastError,
        statusChangedAt: s.statusChangedAt,
        ...resourceUsage,
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

async function getResourceUsage(): Promise<{ cpu?: number; memory?: number }> {
  try {
    const cpu = await getCpuPercent();
    const totalMb = os.totalmem() / 1024 / 1024;
    const freeMb = os.freemem() / 1024 / 1024;
    const usedPercent = totalMb > 0 ? ((totalMb - freeMb) / totalMb) * 100 : undefined;
    return {
      cpu: typeof cpu === 'number' ? Math.round(cpu * 100) / 100 : undefined,
      memory: typeof usedPercent === 'number' ? Math.round(usedPercent * 100) / 100 : undefined,
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

async function getCpuPercent(): Promise<number | undefined> {
  const first = await readCpuSample();
  await new Promise(resolve => setTimeout(resolve, 120));
  const second = await readCpuSample();
  const idleDiff = second.idle - first.idle;
  const totalDiff = second.total - first.total;
  if (totalDiff <= 0) return undefined;
  return (1 - idleDiff / totalDiff) * 100;
}

async function readCpuSample(): Promise<{ idle: number; total: number }> {
  const stat = await fs.readFile('/proc/stat', 'utf8');
  const line = stat.split('\n').find(item => item.startsWith('cpu '));
  if (!line) {
    throw new Error('cpu stat 缺失');
  }
  const values = line.trim().split(/\s+/).slice(1).map(item => parseInt(item, 10));
  const idle = (values[3] || 0) + (values[4] || 0);
  const total = values.reduce((sum, value) => sum + value, 0);
  return { idle, total };
}
