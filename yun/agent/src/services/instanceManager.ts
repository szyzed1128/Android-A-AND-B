/**
 * 实例管理器
 * 执行 adb shell 命令，管理实例就绪流程（无限重试循环）
 */

import { exec } from 'child_process';
import { promisify } from 'util';
import { InstanceConfig } from '../config';
import { runReadinessProbe } from './apkProbe';

const execAsync = promisify(exec);

// 实例运行时状态（内存中）
export type RuntimeStatus = 'idle' | 'probing' | 'busy' | 'bad';

interface InstanceRuntime {
  config: InstanceConfig;
  status: RuntimeStatus;
  failureCount: number;
  probingPromise?: Promise<void>;  // 防止并发触发多次探针
}

const instances = new Map<string, InstanceRuntime>();

const MAX_FAILURES_BEFORE_BAD = 5;

/**
 * 初始化所有实例（服务启动时调用）
 */
export function initInstances(configs: InstanceConfig[]): void {
  for (const cfg of configs) {
    instances.set(cfg.id, {
      config: cfg,
      status: 'probing',
      failureCount: 0,
    });
    // 异步启动就绪流程
    triggerReadinessLoop(cfg.id).catch(e =>
      console.error(`[InstanceManager] 初始化失败 ${cfg.id}:`, e.message)
    );
  }
  console.log(`[InstanceManager] 初始化 ${configs.length} 个实例`);
}

/**
 * 获取实例状态
 */
export function getInstanceStatus(instanceId: string): RuntimeStatus | null {
  return instances.get(instanceId)?.status ?? null;
}

/**
 * 获取所有实例状态
 */
export function getAllInstanceStatuses(): Array<{ id: string; status: RuntimeStatus; wsPort: number }> {
  return Array.from(instances.entries()).map(([id, rt]) => ({
    id,
    status: rt.status,
    wsPort: rt.config.wsPort,
  }));
}

/**
 * 触发实例重置（由调度后端调用，释放实例后）
 * 异步执行，立即返回
 */
export function triggerReset(instanceId: string): void {
  const rt = instances.get(instanceId);
  if (!rt) {
    console.warn(`[InstanceManager] triggerReset: 实例 ${instanceId} 不存在`);
    return;
  }

  // 如果已在探针中，不重复触发
  if (rt.status === 'probing' && rt.probingPromise) {
    console.log(`[InstanceManager] ${instanceId} 已在就绪流程中，忽略重复触发`);
    return;
  }

  rt.status = 'probing';
  rt.failureCount = 0;
  console.log(`[InstanceManager] ${instanceId} 触发重置`);

  triggerReadinessLoop(instanceId).catch(e =>
    console.error(`[InstanceManager] 就绪循环异常 ${instanceId}:`, e.message)
  );
}

/**
 * 标记实例为 busy（用户已连接）
 */
export function markBusy(instanceId: string): void {
  const rt = instances.get(instanceId);
  if (rt) rt.status = 'busy';
}

/**
 * 核心：实例就绪循环（无限重试直到成功或达到失败上限）
 */
async function triggerReadinessLoop(instanceId: string): Promise<void> {
  const rt = instances.get(instanceId);
  if (!rt) return;

  // 防止并发
  if (rt.probingPromise) {
    await rt.probingPromise;
    return;
  }

  const promise = (async () => {
    while (true) {
      if (rt.failureCount >= MAX_FAILURES_BEFORE_BAD) {
        console.error(`[InstanceManager] ${instanceId} 连续失败${rt.failureCount}次，标记为 bad`);
        rt.status = 'bad';
        rt.probingPromise = undefined;
        return;
      }

      console.log(`[InstanceManager] ${instanceId} 开始就绪探针（第${rt.failureCount + 1}次）`);
      const result = await runReadinessProbe(rt.config);

      if (result.success) {
        rt.status = 'idle';
        rt.failureCount = 0;
        rt.probingPromise = undefined;
        console.log(`[InstanceManager] ${instanceId} ✅ 就绪，状态=idle`);
        return;
      } else {
        rt.failureCount++;
        console.warn(`[InstanceManager] ${instanceId} 探针失败(${rt.failureCount}/${MAX_FAILURES_BEFORE_BAD}): ${result.error}`);
        await sleep(3000); // 失败后等3秒再重试
      }
    }
  })();

  rt.probingPromise = promise;
  await promise;
}

/**
 * 执行 shell 命令（供探针使用）
 */
export async function execShell(cmd: string): Promise<string> {
  try {
    const { stdout, stderr } = await execAsync(cmd, { timeout: 30000 });
    if (stderr && !stderr.includes('Warning')) {
      console.warn(`[Shell] stderr: ${stderr.trim()}`);
    }
    return stdout.trim();
  } catch (err: any) {
    throw new Error(`命令失败: ${cmd}\n${err.message}`);
  }
}

function sleep(ms: number): Promise<void> {
  return new Promise(resolve => setTimeout(resolve, ms));
}
