/**
 * 实例管理器
 * 执行 adb shell 命令，管理实例就绪流程（无限重试循环）
 */

import { exec } from 'child_process';
import { promisify } from 'util';
import { InstanceConfig } from '../config';
import { runReadinessProbe, tryAdoptReadyInstance } from './apkProbe';

const execAsync = promisify(exec);

// 实例运行时状态（内存中）
export type RuntimeStatus = 'idle' | 'probing' | 'busy' | 'bad';

interface InstanceRuntime {
  config: InstanceConfig;
  status: RuntimeStatus;
  statusChangedAt: number;
  failureCount: number;
  lastError?: string;
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
      statusChangedAt: Date.now(),
      failureCount: 0,
    });
  }
  bootstrapInstancesOnStartup(configs).catch(e =>
    console.error(`[InstanceManager] 启动接管失败:`, e.message)
  );
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
export function getAllInstanceStatuses(): Array<{
  id: string;
  status: RuntimeStatus;
  wsPort: number;
  statusChangedAt: number;
  failureCount: number;
  lastError?: string;
}> {
  return Array.from(instances.entries()).map(([id, rt]) => ({
    id,
    status: rt.status,
    wsPort: rt.config.wsPort,
    statusChangedAt: rt.statusChangedAt,
    failureCount: rt.failureCount,
    lastError: rt.lastError,
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

  setRuntimeStatus(rt, 'probing');
  rt.failureCount = 0;
  rt.lastError = undefined;
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
  if (rt) setRuntimeStatus(rt, 'busy');
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
        setRuntimeStatus(rt, 'bad');
        rt.probingPromise = undefined;
        return;
      }

      console.log(`[InstanceManager] ${instanceId} 开始就绪探针（第${rt.failureCount + 1}次）`);
      const result = await runReadinessProbe(rt.config);

      if (result.success) {
        setRuntimeStatus(rt, 'idle');
        rt.failureCount = 0;
        rt.lastError = undefined;
        rt.probingPromise = undefined;
        console.log(`[InstanceManager] ${instanceId} ✅ 就绪，状态=idle`);
        return;
      } else {
        rt.failureCount++;
        rt.lastError = result.error;
        console.warn(`[InstanceManager] ${instanceId} 探针失败(${rt.failureCount}/${MAX_FAILURES_BEFORE_BAD}): ${result.error}`);
        if (rt.failureCount >= MAX_FAILURES_BEFORE_BAD) {
          setRuntimeStatus(rt, 'bad');
        }
        await sleep(3000); // 失败后等3秒再重试
      }
    }
  })();

  rt.probingPromise = promise;
  await promise;
}

async function bootstrapInstancesOnStartup(configs: InstanceConfig[]): Promise<void> {
  const coldProbeQueue: string[] = [];

  for (const cfg of configs) {
    const rt = instances.get(cfg.id);
    if (!rt) continue;

    console.log(`[InstanceManager] ${cfg.id} 启动接管：优先复用现成运行态`);
    const adoptResult = await tryAdoptReadyInstance(cfg);
    if (adoptResult.success) {
      setRuntimeStatus(rt, 'idle');
      rt.failureCount = 0;
      rt.lastError = undefined;
      console.log(`[InstanceManager] ${cfg.id} ✅ 已接管现成运行态，状态=idle`);
      continue;
    }

    rt.lastError = adoptResult.error;
    console.log(`[InstanceManager] ${cfg.id} 启动接管失败，加入冷探针队列: ${adoptResult.error}`);
    coldProbeQueue.push(cfg.id);
  }

  for (const instanceId of coldProbeQueue) {
    console.log(`[InstanceManager] ${instanceId} 开始启动后冷探针`);
    await triggerReadinessLoop(instanceId);
  }
}

function setRuntimeStatus(rt: InstanceRuntime, status: RuntimeStatus): void {
  if (rt.status === status) {
    return;
  }

  rt.status = status;
  rt.statusChangedAt = Date.now();
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
