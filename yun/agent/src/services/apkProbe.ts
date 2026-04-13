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
const STEP_TIMEOUT_MS = 35000;          // 每步骤超时35秒（APK profiles 加载约需20秒）
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
    // Step 0: 确保 adb 连接有效（systemd 环境下 adb server 可能没有设备注册）
    console.log(`[Probe] ${instance.id} Step0: adb connect ${instance.adbTarget}`);
    await execShell(`adb connect ${instance.adbTarget}`);
    // adb daemon 重启后 forward 会消失，每次探针前都重建
    await execShell(`adb -s ${instance.adbTarget} forward tcp:${instance.probePort} tcp:8080`);
    console.log(`[Probe] ${instance.id} Step0: forward ${instance.probePort}→8080 已建立`);

    // Step 1: pm clear（清除残留数据）
    console.log(`[Probe] ${instance.id} Step1: pm clear`);
    await execShell(`adb -s ${instance.adbTarget} shell pm clear ${instance.packageName}`);

    // Step 2: am start（冷启动）
    console.log(`[Probe] ${instance.id} Step2: am start`);
    await execShell(
      `adb -s ${instance.adbTarget} shell am start -n ` +
      `${instance.packageName}/${instance.activityName}`
    );

    // Step 3: 等待 APK 完全就绪（轮询 getBrands 直到成功，最多等 60 秒）
    // 说明：APK 的 WebSocket 端口约 2 秒可连，但 JSBridge 初始化约需 20 秒
    //       只检查 TCP 连通不够，必须等 getBrands 成功才算真正就绪
    console.log(`[Probe] ${instance.id} Step3: 等待APK完全就绪（最多60秒）`);
    const wsUrl = `ws://localhost:${instance.probePort}/ws`;
    await waitForApkReady(wsUrl, 60000);
    console.log(`[Probe] ${instance.id} Step3: APK已就绪，getBrands 可响应`);

    // Step 4-6: 应用默认车型（AITO），验证 profiles 已加载
    console.log(`[Probe] ${instance.id} Step4-6: 应用默认车型配置`);
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
    const rawBrands = await sendRequest(ws, 'getBrands', {});
    const brands = parseResponseData(rawBrands);
    if (!Array.isArray(brands) || brands.length === 0) {
      throw new Error('getBrands 返回空列表');
    }
    console.log(`[Probe] ${instance.id} getBrands 成功，共${brands.length}个品牌`);

    // Step 5: getProfiles —— 验证指定品牌的配置已加载，同时获取 profileIndex
    const rawProfiles = await sendRequest(ws, 'getProfiles', { brand });
    const profiles = parseResponseData(rawProfiles);
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
 * 轮询等待 APK 完全就绪（WebSocket 可连 + getBrands 成功响应）
 *
 * 仅检查 TCP 连通是不够的：APK 的 WebSocket 端口约 2 秒就会监听，
 * 但 JSBridge 初始化约需 20-30 秒，过早调用 getBrands 会返回 NullReferenceException。
 * 此函数每 3 秒尝试一次 getBrands，直到成功或超时。
 */
async function waitForApkReady(wsUrl: string, timeoutMs: number): Promise<void> {
  const deadline = Date.now() + timeoutMs;

  while (Date.now() < deadline) {
    let ws: WebSocket | null = null;
    try {
      ws = await connectWebSocket(wsUrl, 3000);
      const raw = await sendRequest(ws, 'getBrands', {});
      const brands = parseResponseData(raw);
      if (Array.isArray(brands) && brands.length > 0) {
        return; // APK 完全就绪
      }
    } catch {
      // 未就绪（连接失败或 getBrands 出错），等待后重试
    } finally {
      try { ws?.close(); } catch { /* ignore */ }
    }
    await sleep(3000);
  }

  throw new Error(`APK 初始化超时（${timeoutMs}ms），getBrands 始终未成功`);
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
