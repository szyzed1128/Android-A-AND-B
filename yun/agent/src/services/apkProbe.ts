/**
 * APK 就绪探针
 *
 * 核心操作：实例就绪流程
 * ① am force-stop → ② am start → ③ 等待WS可连接 → ④ getBrands → ⑤ getProfiles("AITO")
 * → ⑥ applyProfile("AITO","AITO") → ⑦ 断开 → ⑧ 标记 idle
 *
 * 任意步骤失败 → 回到①重试（最多5次，随后标记 bad）
 */

import WebSocket from 'ws';
import { execShell } from './instanceManager';
import { InstanceConfig } from '../config';

// 默认探针配置
const DEFAULT_BRAND = 'AITO';
const DEFAULT_MODEL = 'AITO';
const WS_CONNECT_TIMEOUT_MS = 15000;    // 等待WS连接最长15秒
const WS_CONNECT_POLL_INTERVAL_MS = 500;
const STEP_TIMEOUT_MS = 35000;          // 每步骤超时35秒（APK profiles 加载约需20秒）
const READINESS_TIMEOUT_MS = 120000;    // 冷探针等待默认品牌目录完成加载的最长时间
const MAX_FAILURES_BEFORE_BAD = 5;

export type ProbeStatus = 'idle' | 'probing' | 'bad';

export interface ProbeResult {
  success: boolean;
  error?: string;
}

export interface CatalogProfileItem {
  profileIndex: number;
  name: string;
  description?: string;
}

/**
 * 执行完整就绪探针流程（供外部调用）
 */
export async function runReadinessProbe(instance: InstanceConfig): Promise<ProbeResult> {
  try {
    // Step 0: 确保 adb 连接有效（systemd 环境下 adb server 可能没有设备注册）
    console.log(`[Probe] ${instance.id} Step0: adb connect ${instance.adbTarget}`);
    await execShell(`adb connect ${instance.adbTarget}`);
    // adb daemon 重启后 forward 会消失，每次探针前都重建
    await execShell(`adb -s ${instance.adbTarget} forward tcp:${instance.probePort} tcp:8080`);
    console.log(`[Probe] ${instance.id} Step0: forward ${instance.probePort}→8080 已建立`);

    // Step 1: am force-stop（日常就绪/重置不再清应用数据，避免每次都回到最脆弱的首次初始化路径）
    console.log(`[Probe] ${instance.id} Step1: am force-stop`);
    await stopApkActivity(instance);

    // Step 2: am start（冷启动）
    console.log(`[Probe] ${instance.id} Step2: am start`);
    await startApkActivity(instance);

    // Step 3: 等待 APK 完全就绪（不仅 getBrands 成功，还要默认品牌 profiles 真正可用）
    // 说明：只看到 getBrands 成功并不代表 profiles 已加载完成；过早调用 getProfiles/applyProfile
    //      会得到空列表或 NullReferenceException，导致实例被误判成 idle。
    console.log(`[Probe] ${instance.id} Step3: 等待APK完全就绪（默认品牌 catalog 可用，最多120秒）`);
    const wsUrl = `ws://localhost:${instance.probePort}/ws`;
    await waitForApkReady(wsUrl, READINESS_TIMEOUT_MS, DEFAULT_BRAND);
    console.log(`[Probe] ${instance.id} Step3: APK已就绪，默认品牌 catalog 可响应`);

    // Step 4-6: 应用默认车型（AITO），验证 profiles 已加载
    console.log(`[Probe] ${instance.id} Step4-6: 应用默认车型配置`);
    const applyResult = await applyCarProfile(instance, DEFAULT_BRAND, 0, DEFAULT_MODEL);
    if (!applyResult.success) {
      throw new Error(applyResult.error || `默认车型 ${DEFAULT_BRAND}/${DEFAULT_MODEL} 应用失败`);
    }

    console.log(`[Probe] ${instance.id} ✅ 就绪探针通过`);
    return { success: true };
  } catch (err: any) {
    console.error(`[Probe] ${instance.id} ❌ 探针失败:`, err.message);
    return { success: false, error: err.message };
  }
}

/**
 * Agent 重启后的“接管”路径：
 * 优先复用当前已经跑起来的实例，不主动 force-stop / am start。
 * 只要本机对外 wsPort 已经能完成 getBrands + getProfiles + applyProfile，
 * 就直接把实例标回 idle，避免每次 Agent 重启都把全部实例重新冷启动一遍。
 */
export async function tryAdoptReadyInstance(instance: InstanceConfig): Promise<ProbeResult> {
  try {
    console.log(`[Probe] ${instance.id} adopt: adb connect ${instance.adbTarget}`);
    await execShell(`adb connect ${instance.adbTarget}`);
    await execShell(`adb -s ${instance.adbTarget} forward tcp:${instance.probePort} tcp:8080`);
    const wsUrl = `ws://127.0.0.1:${instance.probePort}/ws`;
    console.log(`[Probe] ${instance.id} adopt: 尝试接管现成运行态 ${wsUrl}`);
    await runBusinessProbeOnUrl(instance.id, wsUrl, DEFAULT_BRAND, 0, DEFAULT_MODEL);
    console.log(`[Probe] ${instance.id} adopt: 接管成功`);
    return { success: true };
  } catch (err: any) {
    console.warn(`[Probe] ${instance.id} adopt 失败: ${err.message}`);
    return { success: false, error: err.message };
  }
}

/**
 * 应用指定车型配置（供 applyCar 接口调用）
 * 注意：A端协议 applyProfile 参数是 { brand, profileIndex: number }
 * profileIndex 是 getProfiles(brand) 返回数组的下标
 */
export async function applyCarProfile(
  instance: InstanceConfig,
  carBrand: string,
  profileIndex: number,
  profileName?: string
): Promise<ProbeResult> {
  try {
    console.log(`[Probe] ${instance.id} applyCar: ${carBrand} index=${profileIndex} name=${profileName || '-'}`);
    // 多实例下，idle 并不等于 APK 进程仍然存活。分配前显式拉起 Activity，
    // 避免实例2这类“探针通过后进程已退出”导致的分配成功但 ws 不可连。
    await startApkActivity(instance);
    // 这里不能只等 getBrands 成功；目标品牌的 profiles 也必须可用，否则 applyCar 仍会在
    // getProfiles/applyProfile 阶段失败。
    const wsUrl = `ws://localhost:${instance.probePort}/ws`;
    await waitForApkReady(wsUrl, 45000, carBrand);
    await runBusinessProbe(instance, carBrand, profileIndex, profileName);
    return { success: true };
  } catch (err: any) {
    console.error(`[Probe] ${instance.id} applyCar 失败:`, err.message);
    return { success: false, error: err.message };
  }
}

export async function getCatalogBrands(instance: InstanceConfig): Promise<string[]> {
  return withProbeSocket(instance, async (ws) => {
    const rawBrands = await sendRequest(ws, 'getBrands', {});
    const brands = parseResponseData(rawBrands);
    if (!Array.isArray(brands)) {
      throw new Error('getBrands 返回格式异常');
    }
    return brands
      .map((item) => String(item ?? '').trim())
      .filter(Boolean);
  });
}

export async function getCatalogProfiles(
  instance: InstanceConfig,
  brand: string
): Promise<CatalogProfileItem[]> {
  return withProbeSocket(instance, async (ws) => {
    const rawProfiles = await sendRequest(ws, 'getProfiles', { brand });
    const profiles = parseResponseData(rawProfiles);
    if (!Array.isArray(profiles)) {
      throw new Error(`getProfiles("${brand}") 返回格式异常`);
    }

    return profiles.map((profile: any, index: number) => ({
      profileIndex: index,
      name: String(profile?.Name ?? profile?.name ?? `配置${index + 1}`),
      description: profile?.Description ?? profile?.description ?? undefined,
    }));
  });
}

/**
 * WebSocket 业务探针：getBrands → getProfiles → applyProfile
 *
 * A端协议说明：
 *   - getProfiles({ brand }) → 返回 Profile[] 数组
 *   - applyProfile({ brand, profileIndex }) → profileIndex 是上述数组的下标
 *
 * 探针默认车型 AITO 只有一个配置，profileIndex = 0
 * 用户指定车型时，需先 getProfiles 找到对应 profileIndex
 */
async function runBusinessProbe(
  instance: InstanceConfig,
  brand: string,
  profileIndex: number,
  profileName?: string
): Promise<void> {
  const wsUrl = `ws://localhost:${instance.probePort}/ws`;
  await runBusinessProbeOnUrl(instance.id, wsUrl, brand, profileIndex, profileName);
}

async function runBusinessProbeOnUrl(
  instanceId: string,
  wsUrl: string,
  brand: string,
  profileIndex: number,
  profileName?: string
): Promise<void> {
  const ws = await connectWebSocket(wsUrl);

  try {
    const rawBrands = await sendRequest(ws, 'getBrands', {});
    const brands = parseResponseData(rawBrands);
    if (!Array.isArray(brands) || brands.length === 0) {
      throw new Error('getBrands 返回空列表');
    }
    console.log(`[Probe] ${instanceId} getBrands 成功，共${brands.length}个品牌`);

    const rawProfiles = await sendRequest(ws, 'getProfiles', { brand });
    const profiles = parseResponseData(rawProfiles);
    if (!Array.isArray(profiles) || profiles.length === 0) {
      throw new Error(`getProfiles("${brand}") 返回空列表`);
    }
    console.log(`[Probe] ${instanceId} getProfiles 成功，共${profiles.length}个配置`);

    if (profileIndex < 0 || profileIndex >= profiles.length) {
      throw new Error(
        `品牌 ${brand} 的配置下标越界: index=${profileIndex}, count=${profiles.length}, name=${profileName || '-'}`
      );
    }

    await sendRequest(ws, 'applyProfile', { brand, profileIndex });
    console.log(`[Probe] ${instanceId} applyProfile("${brand}", index=${profileIndex}) 成功`);
  } finally {
    try {
      ws.close();
    } catch {
      // ignore
    }
  }
}

async function withProbeSocket<T>(
  instance: InstanceConfig,
  runner: (ws: WebSocket) => Promise<T>
): Promise<T> {
  const wsUrl = `ws://localhost:${instance.probePort}/ws`;
  const ws = await connectWebSocket(wsUrl);

  try {
    return await runner(ws);
  } finally {
    ws.close();
  }
}

async function startApkActivity(instance: InstanceConfig): Promise<void> {
  await execShell(
    `adb -s ${instance.adbTarget} shell am start -n ` +
    `${instance.packageName}/${instance.activityName}`
  );
}

async function stopApkActivity(instance: InstanceConfig): Promise<void> {
  await execShell(
    `adb -s ${instance.adbTarget} shell am force-stop ${instance.packageName}`
  );
}

/**
 * 解析 A端响应的 Data 字段
 * A端返回的 Data 可能是 JSON 字符串（如 "[\"Acura\",...]"）或已解析的对象/数组
 */
function parseResponseData(data: any): any {
  if (typeof data === 'string') {
    try { return JSON.parse(data); } catch { return data; }
  }
  return data;
}

/**
 * 轮询等待 APK 完全就绪
 *
 * 仅检查 TCP 连通是不够的：APK 的 WebSocket 端口约 2 秒就会监听，
 * 但 JSBridge / ProfilesV2 初始化可能更晚完成。过早调用 getBrands/getProfiles
 * 会返回空列表或 NullReferenceException。
 *
 * 当传入 brand 时，除了 getBrands 成功，还要求 getProfiles(brand) 返回非空列表。
 */
async function waitForApkReady(wsUrl: string, timeoutMs: number, brand?: string): Promise<void> {
  const deadline = Date.now() + timeoutMs;
  let lastError = '';

  while (Date.now() < deadline) {
    let ws: WebSocket | null = null;
    try {
      ws = await connectWebSocket(wsUrl, 3000);
      const raw = await sendRequest(ws, 'getBrands', {});
      const brands = parseResponseData(raw);
      if (!Array.isArray(brands) || brands.length === 0) {
        throw new Error('getBrands 返回空列表');
      }

      if (brand) {
        const rawProfiles = await sendRequest(ws, 'getProfiles', { brand });
        const profiles = parseResponseData(rawProfiles);
        if (!Array.isArray(profiles) || profiles.length === 0) {
          throw new Error(`getProfiles("${brand}") 返回空列表`);
        }
      }
      return;
    } catch (err: any) {
      lastError = err?.message || '未知错误';
      // 未就绪（连接失败或 getBrands/getProfiles 出错），等待后重试
    } finally {
      try { ws?.close(); } catch { /* ignore */ }
    }
    await sleep(3000);
  }

  if (brand) {
    throw new Error(`APK 初始化超时（${timeoutMs}ms），${brand} 的 profiles 始终未就绪: ${lastError}`);
  }
  throw new Error(`APK 初始化超时（${timeoutMs}ms），getBrands 始终未成功: ${lastError}`);
}

/**
 * 轮询等待 WebSocket 端口可连接（仅 TCP 层，用于 applyCar 前的快速检查）
 */
async function waitForWebSocket(url: string, timeoutMs: number): Promise<void> {
  const deadline = Date.now() + timeoutMs;
  let lastError = '';

  while (Date.now() < deadline) {
    try {
      const ws = await connectWebSocket(url, 2000);
      ws.close();
      return;
    } catch (err: any) {
      lastError = err.message;
      await sleep(WS_CONNECT_POLL_INTERVAL_MS);
    }
  }

  throw new Error(`等待WebSocket超时(${timeoutMs}ms): ${lastError}`);
}

/**
 * 建立 WebSocket 连接
 */
function connectWebSocket(url: string, timeoutMs = 5000): Promise<WebSocket> {
  return new Promise((resolve, reject) => {
    const ws = new WebSocket(url);
    const timer = setTimeout(() => {
      ws.terminate();
      reject(new Error(`WebSocket连接超时: ${url}`));
    }, timeoutMs);

    ws.on('open', () => {
      clearTimeout(timer);
      resolve(ws);
    });
    ws.on('error', (err) => {
      clearTimeout(timer);
      reject(err);
    });
  });
}

/**
 * 通过 WebSocket 发送请求并等待响应
 * 使用与现有协议一致的 request/response 格式
 */
function sendRequest(ws: WebSocket, action: string, data: any): Promise<any> {
  return new Promise((resolve, reject) => {
    const requestId = `probe_${Date.now()}_${Math.random().toString(36).slice(2, 6)}`;
    const timer = setTimeout(() => {
      ws.off('message', handler);
      reject(new Error(`${action} 超时（${STEP_TIMEOUT_MS}ms）`));
    }, STEP_TIMEOUT_MS);

    const handler = (raw: WebSocket.RawData) => {
      try {
        const msg = JSON.parse(raw.toString());
        // A端协议用 PascalCase（RequestId），兼容 camelCase（requestId）
        const msgRequestId = msg.RequestId ?? msg.requestId;
        if (msgRequestId !== requestId) return;

        clearTimeout(timer);
        ws.off('message', handler);

        // 兼容 PascalCase（Success）和 camelCase（success）
        const isSuccess = msg.Success ?? msg.success;
        if (isSuccess === false) {
          reject(new Error(`${action} 失败: ${msg.Error ?? msg.error ?? '未知错误'}`));
        } else {
          resolve(msg.Data ?? msg.data);
        }
      } catch {
        // 解析失败，等下一条消息
      }
    };

    ws.on('message', handler);
    ws.send(JSON.stringify({
      type: 'request',
      action,
      requestId,
      data,
    }));
  });
}

function sleep(ms: number): Promise<void> {
  return new Promise(resolve => setTimeout(resolve, ms));
}
