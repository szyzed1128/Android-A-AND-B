/**
 * BurstSampler - Burst 采样引擎
 *
 * 按 descriptor 顺序向蓝牙发命令，用 ElmResponseAssembler 组包，
 * 生成 BurstSampleEnvelope[]。
 *
 * header 回放使用 descriptor.effectiveHeader（由 A 端解析 GetDefaultHeader 结果下发），
 * B 端不自行猜测默认 header。
 *
 * normalizedPayloadHex 仅作 B 端 best-effort 提取，A 端以 rawElmText 为权威输入，
 * 并在同一个 OBDDataReader 会话内通过 Burst Replay 驱动原始解析链。
 */

import { Buffer } from 'buffer';
import { BluetoothGateway } from '../bluetooth/BluetoothManager';
import { ElmResponseAssembler } from './ElmResponseAssembler';
import {
  BurstRequestDescriptor,
  BurstSampleEnvelope,
} from './BurstTypes';

export interface BurstSamplerOptions {
  sessionId: string;
  descriptors: BurstRequestDescriptor[];
  durationMs: number;
  gateway: BluetoothGateway;
  commandTimeoutMs?: number;
  onCycleComplete?: (cycleIndex: number, samples: BurstSampleEnvelope[]) => void;
}

function formatWireText(text: string, maxLen = 600): string {
  const normalized = (text || '').replace(/\r/g, '\\r').replace(/\n/g, '\\n');
  return normalized.length > maxLen ? normalized.slice(0, maxLen) + '…' : normalized;
}

function buildExchangeKey(command: string, effectiveHeader?: string): string {
  return command + '|' + (effectiveHeader || '');
}

/**
 * 发送一条命令并等待完整响应（beginWait → send → wait），并输出诊断日志。
 */
async function sendAndWaitLogged(
  gateway: BluetoothGateway,
  assembler: ElmResponseAssembler,
  command: string,
  timeoutMs: number,
  context: {
    sessionId: string;
    cycleIndex: number;
    stage: 'atsh' | 'before' | 'main' | 'after';
    seqId?: number;
    effectiveHeader?: string;
    note?: string;
  },
): Promise<{
  rawElmText: string;
  promptSeen: boolean;
  timeout: boolean;
  completedBy: 'prompt' | 'timeout' | 'abort';
  chunkCount: number;
  sentAtMs: number;
  completedAtMs: number;
  elapsedMs: number;
}> {
  const exchangeKey = buildExchangeKey(command, context.effectiveHeader);
  const note = context.note ? ` note=${context.note}` : '';
  const seqText = context.seqId ?? '-';
  const headerText = context.effectiveHeader || '-';

  console.log(
    `[BurstWire][TX] session=${context.sessionId} cycle=${context.cycleIndex} stage=${context.stage} ` +
    `seq=${seqText} header=${headerText} key=${exchangeKey} timeoutMs=${timeoutMs} cmd=${command}${note}`
  );

  const waitPromise = assembler.beginWait(timeoutMs);
  const cmdB64 = Buffer.from(command + '\r', 'ascii').toString('base64');
  const sentAtMs = Date.now();
  await gateway.send(cmdB64);
  const resp = await waitPromise;
  const completedAtMs = Date.now();
  const elapsedMs = completedAtMs - sentAtMs;

  console.log(
    `[BurstWire][RX] session=${context.sessionId} cycle=${context.cycleIndex} stage=${context.stage} ` +
    `seq=${seqText} header=${headerText} key=${exchangeKey} elapsedMs=${elapsedMs} ` +
    `completedBy=${resp.completedBy} promptSeen=${resp.promptSeen} timeout=${resp.timeout} ` +
    `chunks=${resp.chunkCount} raw=${formatWireText(resp.rawElmText)}${note}`
  );

  return {
    ...resp,
    sentAtMs,
    completedAtMs,
    elapsedMs,
  };
}

/**
 * best-effort 提取 responseHeader 和 normalizedPayloadHex。
 * 这些字段仅用于 B 端显示/诊断。A 端仍以 rawElmText 为权威输入，并在 replay 会话内消费。
 *
 * 使用 descriptor.responseMarker 定位 payload 起点（优先于 mode+0x40 猜测）。
 */
function parseElmResponse(rawElmText: string, descriptor: BurstRequestDescriptor): {
  responseHeader: string;
  normalizedPayloadHex: string;
  isNoData: boolean;
  isError: boolean;
} {
  const trimmed = rawElmText.replace(/>/g, '').trim();

  if (/NO DATA/i.test(trimmed)) {
    return { responseHeader: '', normalizedPayloadHex: '', isNoData: true, isError: false };
  }
  if (/ERROR|UNABLE|BUFFER FULL|\?/i.test(trimmed)) {
    return { responseHeader: '', normalizedPayloadHex: '', isNoData: false, isError: true };
  }

  const lines = trimmed.split(/[\r\n]+/)
    .map(l => l.trim())
    .filter(l => l.length > 0 && /^[0-9A-Fa-f\s]+$/.test(l));

  if (lines.length === 0) {
    return { responseHeader: '', normalizedPayloadHex: '', isNoData: false, isError: true };
  }

  // 使用 descriptor.responseMarker（由 A 端 GetResponseMarkerFromCommand 生成）
  const marker = (descriptor.responseMarker || '').toUpperCase();

  let responseHeader = '';
  const payloadParts: string[] = [];

  for (const line of lines) {
    const tokens = line.replace(/\s+/g, ' ').trim().split(' ');
    if (tokens.length === 0) continue;

    let dataStartIdx = 0;

    // CAN header 检测：首 token 为 3 字符 hex（如 7E8）
    if (tokens[0].length === 3 && /^[0-9A-Fa-f]{3}$/.test(tokens[0])) {
      if (!responseHeader) responseHeader = tokens[0].toUpperCase();
      dataStartIdx = 1;
    }

    const dataHex = tokens.slice(dataStartIdx).join('').toUpperCase();

    if (marker && dataHex.includes(marker)) {
      const markerIdx = dataHex.indexOf(marker);
      payloadParts.push(dataHex.substring(markerIdx));
    } else {
      // 无 marker 的行：可能是 ISO-TP 连续帧或非标格式
      // 标记为 best-effort，A 端 DecodeData 会重新解析 rawElmText
      payloadParts.push(dataHex);
    }
  }

  return {
    responseHeader,
    normalizedPayloadHex: payloadParts.join(''),
    isNoData: false,
    isError: false,
  };
}

export async function runBurstSampler(
  options: BurstSamplerOptions,
  abortSignal?: { aborted: boolean },
  externalAssembler?: ElmResponseAssembler,
): Promise<BurstSampleEnvelope[]> {
  const {
    sessionId,
    descriptors,
    durationMs,
    gateway,
    commandTimeoutMs = 3000,
    onCycleComplete,
  } = options;

  // 如果调用方传入了外部 assembler（用于 abort 能直接中断），则使用它
  // 调用方负责注册 feed 监听和最终 destroy
  const assembler = externalAssembler ?? new ElmResponseAssembler();
  const ownsAssembler = !externalAssembler;
  const allSamples: BurstSampleEnvelope[] = [];
  let sampleOrdinal = 0;
  let cycleIndex = 0;
  let lastSentEffectiveHeader = '';

  // 仅当自建 assembler 时注册监听
  let unsubData: (() => void) | null = null;
  if (ownsAssembler) {
    unsubData = gateway.addDataReceivedListener(
      (_sessionId: string, base64Data: string) => {
        assembler.feed(base64Data);
      }
    );
  }

  const startTime = Date.now();

  function isAborted(): boolean {
    return abortSignal?.aborted === true;
  }

  try {
    while (!isAborted() && (Date.now() - startTime) < durationMs) {
      const cycleSamples: BurstSampleEnvelope[] = [];
      console.log(`[BurstWire][CYCLE] session=${sessionId} cycle=${cycleIndex} descriptors=${descriptors.length} start`);

      for (const desc of descriptors) {
        if (isAborted() || (Date.now() - startTime) >= durationMs) break;

        // ── header 回放：使用 descriptor.effectiveHeader ──
        // effectiveHeader 由 A 端解析（含 GetDefaultHeader 逻辑），B 端不自行猜测
        if (!desc.skipATSH) {
          const targetHeader = desc.effectiveHeader || '';
          if (targetHeader && targetHeader !== lastSentEffectiveHeader) {
            await sendAndWaitLogged(gateway, assembler, 'ATSH' + targetHeader, 2000, {
              sessionId,
              cycleIndex,
              stage: 'atsh',
              seqId: desc.seqId,
              effectiveHeader: targetHeader,
              note: 'header-switch',
            });
            lastSentEffectiveHeader = targetHeader;
          }
        }

        // 发送 beforeCommands
        if (desc.beforeCommands) {
          for (let bcIndex = 0; bcIndex < desc.beforeCommands.length; bcIndex++) {
            if (isAborted()) break;
            const bc = desc.beforeCommands[bcIndex];
            await sendAndWaitLogged(gateway, assembler, bc, 2000, {
              sessionId,
              cycleIndex,
              stage: 'before',
              seqId: desc.seqId,
              effectiveHeader: desc.effectiveHeader || '',
              note: `before#${bcIndex + 1}/${desc.beforeCommands.length}`,
            });
          }
        }

        if (isAborted()) break;

        // 发送主命令
        const resp = await sendAndWaitLogged(gateway, assembler, desc.command, desc.timeoutMs ?? commandTimeoutMs, {
          sessionId,
          cycleIndex,
          stage: 'main',
          seqId: desc.seqId,
          effectiveHeader: desc.effectiveHeader || '',
          note: `pidIds=${(desc.pidIds || []).join(',') || '-'}`,
        });
        const sentAtMs = resp.sentAtMs;
        const completedAtMs = resp.completedAtMs;

        const parsed = parseElmResponse(resp.rawElmText, desc);

        const envelope: BurstSampleEnvelope = {
          sessionId,
          seqId: desc.seqId,
          cycleIndex,
          sampleOrdinal: sampleOrdinal++,
          command: desc.command,
          header: desc.header,
          sentAtMs,
          completedAtMs,
          timestampMs: sentAtMs,
          elapsedMs: completedAtMs - sentAtMs,
          rawElmText: resp.rawElmText,
          normalizedPayloadHex: parsed.normalizedPayloadHex,
          responseHeader: parsed.responseHeader,
          promptSeen: resp.promptSeen,
          timeout: resp.timeout,
          transportError: undefined,
          completedBy: resp.completedBy,
          // ReplayConnection 匹配键：与 A 端 ReplayConnection 计算方式严格一致
          exchangeKey: desc.command + '|' + (desc.effectiveHeader || ''),
          parseHint: {
            pidIds: desc.pidIds,
            isNoData: parsed.isNoData,
            isError: parsed.isError,
          },
        };

        console.log(
          `[BurstWire][PARSE] session=${sessionId} cycle=${cycleIndex} seq=${desc.seqId} ` +
          `key=${envelope.exchangeKey} responseHeader=${parsed.responseHeader || '-'} ` +
          `isNoData=${parsed.isNoData} isError=${parsed.isError} payload=${formatWireText(parsed.normalizedPayloadHex, 240)}`
        );

        cycleSamples.push(envelope);
        allSamples.push(envelope);

        // 发送 afterCommands
        if (desc.afterCommands) {
          for (let acIndex = 0; acIndex < desc.afterCommands.length; acIndex++) {
            if (isAborted()) break;
            const ac = desc.afterCommands[acIndex];
            await sendAndWaitLogged(gateway, assembler, ac, 2000, {
              sessionId,
              cycleIndex,
              stage: 'after',
              seqId: desc.seqId,
              effectiveHeader: desc.effectiveHeader || '',
              note: `after#${acIndex + 1}/${desc.afterCommands.length}`,
            });
          }
        }
      }

      console.log(`[BurstWire][CYCLE] session=${sessionId} cycle=${cycleIndex} samples=${cycleSamples.length} end`);
      onCycleComplete?.(cycleIndex, cycleSamples);
      cycleIndex++;
    }
  } finally {
    if (ownsAssembler) {
      unsubData?.();
      assembler.destroy();
    }
  }

  return allSamples;
}

/**
 * 按周期边界分 chunk，受双限制：样本数上限 + JSON UTF-8 字节上限
 */
export function splitIntoChunks(
  samples: BurstSampleEnvelope[],
  descriptorCount: number,
  maxSamplesPerChunk = 200,
  maxBytesPerChunk = 512 * 1024,
): BurstSampleEnvelope[][] {
  if (samples.length === 0) return [[]];

  const cycles = new Map<number, BurstSampleEnvelope[]>();
  for (const s of samples) {
    const arr = cycles.get(s.cycleIndex) ?? [];
    arr.push(s);
    cycles.set(s.cycleIndex, arr);
  }

  const sortedCycleKeys = Array.from(cycles.keys()).sort((a, b) => a - b);
  const chunks: BurstSampleEnvelope[][] = [];
  let currentChunk: BurstSampleEnvelope[] = [];
  let currentBytes = 0;

  for (const ck of sortedCycleKeys) {
    const cycleSamples = cycles.get(ck)!;
    const cycleBytes = Buffer.byteLength(JSON.stringify(cycleSamples), 'utf8');
    const cycleSampleCount = cycleSamples.length;

    if (
      currentChunk.length > 0 &&
      (currentChunk.length + cycleSampleCount > maxSamplesPerChunk ||
       currentBytes + cycleBytes > maxBytesPerChunk)
    ) {
      chunks.push(currentChunk);
      currentChunk = [];
      currentBytes = 0;
    }

    currentChunk.push(...cycleSamples);
    currentBytes += cycleBytes;
  }

  if (currentChunk.length > 0 || chunks.length === 0) {
    chunks.push(currentChunk);
  }

  return chunks;
}
