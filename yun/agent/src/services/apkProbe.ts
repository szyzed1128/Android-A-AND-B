/**
 * APK 就绪探针
 *
 * 核心操作：实例就绪流程
 * ① pm clear → ② am start → ③ 等待WS可连接 → ④ getBrands → ⑤ getProfiles("AITO")
 * → ⑥ applyProfile("AITO","AITO") → ⑦ 断开 → ⑧ 标记 idle
 *
 * 任意步骤失败 → 回到①重试（无限重试，连续5次失败标记 bad）
 */

import WebSocket from 'ws';
import { execShell } from './instanceManager';
import { InstanceConfig } from '../config';

// 默认探针配置
const DEFAULT_BRAND = 'AITO';
const DEFAULT_MODEL = 'AITO';
const WS_CONNECT_TIMEOUT_MS = 15000;    // 等待WS连接最长15秒
const WS_CONNECT_POLL_INTERVAL_MS = 500;
const STEP_TIMEOUT_MS = 10000;          // 每步骤超时10秒
const MAX_FAILURES_BEFORE_BAD = 5;

export type ProbeStatus = 'idle' | 'probing' | 'bad';

export interface ProbeResult {
  success: boolean;
  error?: string;
}

/**
 * 执行完整就绪探针流程（供外部调用）
 */
export async function runReadinessProbe(instance: InstanceConfig): Promise<ProbeResult> {
  try {
    // Step 1: pm clear（清除残留数据）
    console.log(`[Probe] ${instance.id} Step1: pm clear`);
    await execShell(`adb -s ${instance.adbTarget} shell pm clear ${instance.packageName}`);

    // Step 2: am start（冷启动）
    console.log(`[Probe] ${instance.id} Step2: am start`);
    await execShell(
      `adb -s ${instance.adbTarget} shell am start -n ` +
      `${instance.packageName}/${instance.activityName}`
    );

    // Step 3: 等待 WebSocket 可连接（使用 adb forward 的内部端口）
    console.log(`[Probe] ${instance.id} Step3: 等待WebSocket (probePort=${instance.probePort})`);
    await waitForWebSocket(`ws://localhost:${instance.probePort}/ws`, WS_CONNECT_TIMEOUT_MS);

    // Step 4-6: WebSocket 业务探针
    console.log(`[Probe] ${instance.id} Step4-6: 业务探针`);
    await runBusinessProbe(instance, DEFAULT_BRAND, DEFAULT_MODEL);

    console.log(`[Probe] ${instance.id} ✅ 就绪探针通过`);
    return { success: true };
  } catch (err: any) {
    console.error(`[Probe] ${instance.id} ❌ 探针失败:`, err.message);
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
  carModel: string
): Promise<ProbeResult> {
  try {
    console.log(`[Probe] ${instance.id} applyCar: ${carBrand}/${carModel}`);
    await runBusinessProbe(instance, carBrand, carModel);
    return { success: true };
  } catch (err: any) {
    console.error(`[Probe] ${instance.id} applyCar 失败:`, err.message);
    return { success: false, error: err.message };
  }
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
  model: string
): Promise<void> {
  const wsUrl = `ws://localhost:${instance.probePort}/ws`;
  const ws = await connectWebSocket(wsUrl);

  try {
    // Step 4: getBrands —— 验证 APK 已加载完毕
    const brands = await sendRequest(ws, 'getBrands', {});
    if (!Array.isArray(brands) || brands.length === 0) {
      throw new Error('getBrands 返回空列表');
    }
    console.log(`[Probe] ${instance.id} getBrands 成功，共${brands.length}个品牌`);

    // Step 5: getProfiles —— 验证指定品牌的配置已加载，同时获取 profileIndex
    const profiles = await sendRequest(ws, 'getProfiles', { brand });
    if (!Array.isArray(profiles) || profiles.length === 0) {
      throw new Error(`getProfiles("${brand}") 返回空列表`);
    }
    console.log(`[Probe] ${instance.id} getProfiles 成功，共${profiles.length}个配置`);

    // 找到目标车型的 profileIndex
    // 默认探针(AITO)只有一个配置，直接用 index=0
    // 用户选择的车型通过名字匹配（Profile.Name 字段）
    let profileIndex = 0;
    if (model !== DEFAULT_MODEL || brand !== DEFAULT_BRAND) {
      // 用户车型：在列表中查找匹配的名字
      const idx = profiles.findIndex((p: any) =>
        (p.Name ?? p.name ?? '') === model
      );
      if (idx < 0) {
        throw new Error(`在 ${brand} 的配置列表中找不到车型 "${model}"`);
      }
      profileIndex = idx;
    }

    // Step 6: applyProfile —— 参数：brand + profileIndex（数字下标）
    await sendRequest(ws, 'applyProfile', { brand, profileIndex });
    console.log(`[Probe] ${instance.id} applyProfile("${brand}", index=${profileIndex}) 成功`);
  } finally {
    ws.close();
  }
}

/**
 * 轮询等待 WebSocket 端口可连接
 */
async function waitForWebSocket(url: string, timeoutMs: number): Promise<void> {
  const deadline = Date.now() + timeoutMs;
  let lastError = '';

  while (Date.now() < deadline) {
    try {
      const ws = await connectWebSocket(url, 2000);
      ws.close();
      return; // 连接成功
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
        if (msg.requestId !== requestId) return;

        clearTimeout(timer);
        ws.off('message', handler);

        if (msg.success === false) {
          reject(new Error(`${action} 失败: ${msg.error || '未知错误'}`));
        } else {
          resolve(msg.data);
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
