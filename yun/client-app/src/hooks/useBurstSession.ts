/**
 * useBurstSession - Burst Snapshot Mode 会话管理 Hook
 *
 * 封装 prepare → sample → commit → finish/abort 全生命周期。
 * 严格执行两阶段 owner 切换：
 *   阶段 1：prepare 期间 B 继续正常转发（burstModeActive=false）
 *   阶段 2：收到 prepared 后才开启 burstModeActive 并开始采样
 *
 * abort 机制：
 *   abortBurst() 设置 signal + 中断 assembler
 *   executeBurst 在 prepare/sampling/committing 各阶段检查 signal
 *   abortBurst 返回一个 Promise，resolve 时机 = executeBurst 的 finally 执行完
 *   页面可 await abortBurst() 来确认"Burst 已真正结束，可以安全离开"
 */

import { useState, useCallback, useRef } from 'react';
import CloudBridge from '../services/CloudBridge';
import { getBluetoothGateway } from './useLocalBluetooth';
import {
  BurstRequestDescriptor,
  BurstSampleEnvelope,
  BurstParsedResult,
  BurstSessionMode,
} from '../burst/BurstTypes';
import { BurstErrorCode } from '../protocol/MessageProtocol';
import { runBurstSampler, splitIntoChunks } from '../burst/BurstSampler';
import { ElmResponseAssembler } from '../burst/ElmResponseAssembler';

export interface BurstSessionResult {
  sessionId: string;
  descriptors: BurstRequestDescriptor[];
  samples: BurstSampleEnvelope[];
  results: BurstParsedResult[];
  totalCycles: number;
}

export function useBurstSession() {
  const [mode, setMode] = useState<BurstSessionMode>('idle');
  const [error, setError] = useState<string | null>(null);
  const [progress, setProgress] = useState({ cycle: 0, sample: 0 });
  const sessionIdRef = useRef<string | null>(null);
  const abortSignalRef = useRef({ aborted: false });
  const assemblerRef = useRef<ElmResponseAssembler | null>(null);
  // executeBurst 完成的 Promise，abort 时可以 await 它来确认 Burst 真正结束
  const executionPromiseRef = useRef<Promise<BurstSessionResult | null> | null>(null);

  const executeBurst = useCallback((
    pidIndices: number[],
    durationMs: number,
  ): Promise<BurstSessionResult | null> => {
    const promise = executeBurstInternal(pidIndices, durationMs);
    executionPromiseRef.current = promise;
    // promise settle 后清除引用
    promise.finally(() => { executionPromiseRef.current = null; });
    return promise;
  }, []);

  async function executeBurstInternal(
    pidIndices: number[],
    durationMs: number,
  ): Promise<BurstSessionResult | null> {
    const gateway = getBluetoothGateway();
    if (!gateway) {
      setError('蓝牙网关未初始化');
      setMode('error');
      return null;
    }

    abortSignalRef.current = { aborted: false };
    assemblerRef.current = null;
    setError(null);
    setMode('preparing');

    let sessionId = '';
    let descriptors: BurstRequestDescriptor[] = [];

    try {
      // ── 阶段 1：prepare（burstModeActive 未开启）──
      const prepareResult = await CloudBridge.prepareBurstSession(pidIndices);
      if (!prepareResult || !prepareResult.descriptors) {
        throw new Error(prepareResult?.error || 'prepare 返回无效');
      }

      sessionId = prepareResult.sessionId || '';
      sessionIdRef.current = sessionId;

      const rawDesc = prepareResult.descriptors;
      descriptors = Array.isArray(rawDesc) ? rawDesc : (typeof rawDesc === 'string' ? JSON.parse(rawDesc) : []);

      if (descriptors.length === 0) {
        throw new Error('descriptor 为空');
      }

      console.log(`[BurstSession] prepared: sessionId=${sessionId} descriptors=${descriptors.length}`);

      if (abortSignalRef.current.aborted) throw new Error('用户取消');

      // ── 阶段 2：收到 prepared 后才开启 burstModeActive ──
      CloudBridge.burstModeActive = true;
      console.log('[BurstSession] burstModeActive = true');

      // 采样
      setMode('sampling');

      const assembler = new ElmResponseAssembler();
      assemblerRef.current = assembler;

      const unsubData = gateway.addDataReceivedListener(
        (_sid: string, base64Data: string) => {
          assembler.feed(base64Data);
        }
      );

      let samples: BurstSampleEnvelope[];
      try {
        samples = await runBurstSampler(
          {
            sessionId,
            descriptors,
            durationMs,
            gateway,
            onCycleComplete: (cycleIdx, cycleSamples) => {
              setProgress({ cycle: cycleIdx + 1, sample: cycleSamples.length });
            },
          },
          abortSignalRef.current,
          assembler,
        );
      } finally {
        unsubData();
        assembler.destroy();
        assemblerRef.current = null;
      }

      console.log(`[BurstSession] sampling done: ${samples.length} samples`);

      if (abortSignalRef.current.aborted) throw new Error('用户取消');

      // ── 提交：每轮检查 abort ──
      setMode('committing');
      const chunks = splitIntoChunks(samples, descriptors.length);
      const totalCycles = samples.reduce((m, s) => Math.max(m, s.cycleIndex), -1) + 1;
      let replayResult: { results: BurstParsedResult[]; totalCycles: number } | null = null;

      for (let i = 0; i < chunks.length; i++) {
        if (abortSignalRef.current.aborted) {
          console.log(`[BurstSession] commit 阶段 abort，已提交 ${i}/${chunks.length} chunks`);
          throw new Error('用户取消');
        }

        const isFinalChunk = i === chunks.length - 1;
        const resp = await CloudBridge.commitBurstSamples({
          sessionId,
          samples: chunks[i],
          chunkIndex: i,
          totalChunks: chunks.length,
          isFinal: isFinalChunk,
          totalCycles: isFinalChunk ? totalCycles : undefined,
        });

        if (!isFinalChunk) {
          if (typeof resp?.accumulated !== 'number') {
            throw new Error('A 端返回的 commit ACK 无效');
          }
          continue;
        }

        if (!resp?.replayStarted) {
          throw new Error('A 端未进入 Burst Replay');
        }

        console.log(`[BurstSession] wait replay result sessionId=${sessionId}`);
        replayResult = await CloudBridge.waitForReplayResult(sessionId, abortSignalRef.current) as {
          results: BurstParsedResult[];
          totalCycles: number;
        };
      }

      const finalResults = replayResult?.results ?? [];
      const finalTotalCycles = replayResult?.totalCycles ?? totalCycles;
      console.log(`[BurstSession] replay done: ${finalResults.length} results totalCycles=${finalTotalCycles}`);

      setMode('finished');
      return {
        sessionId,
        descriptors,
        samples,
        results: finalResults,
        totalCycles: finalTotalCycles,
      };

    } catch (err: any) {
      const errorCode: string = err?.errorCode || '';
      const responseData: any = err?.responseData;

      // Burst Prepare 阶段的结构化冲突错误，提供可读信息
      let msg: string;
      if (errorCode === BurstErrorCode.LowPriorityConflict) {
        if (responseData?.failClosed) {
          msg = `所选 PID 含 CalculatedPID，但 lowPriority 依赖检测失败（fail-closed）：${responseData?.reason || '未知原因'}。请换选不含积分型 CalculatedPID 的参数。`;
        } else {
          const conflicts: any[] = responseData?.conflictingPids || [];
          const names = conflicts.map((c: any) =>
            `${c.selectedPidName}（通过 ${c.conflictSourcePidName} 触发 LP 插队）`
          ).join('、');
          msg = `以下 PID 会在 replay 期间动态插队（MVP 不支持）：${names || '详见日志'}。请取消选择这些 PID。`;
        }
      } else if (errorCode === BurstErrorCode.BypassConflict) {
        const conflicts: any[] = responseData?.conflictDetails || [];
        const cmds = conflicts.map((c: any) => `${c.command}（${c.conflictSource}）`).join('、');
        msg = `以下命令与 bypass 集合冲突：${cmds || '详见日志'}。这些命令会被 ReplayConnection 旁路，无法正常录制。`;
      } else if (errorCode === BurstErrorCode.PingGenerationFailed) {
        msg = `Burst 初始化失败：无法从 SharedSettings 读取连接配置（${responseData?.reason || 'UseDefaultInit/Mode01Prefix/TesterPresentCommand 不可读'}）。请重新连接 OBD 后重试。`;
      } else if (errorCode === BurstErrorCode.SkipATSHUnsupported) {
        msg = `所选 PID 包含 VwTp20/Header=000 协议（seqId=${responseData?.rejectedSeqId ?? '?'}），该协议在当前版本不支持 Burst Replay。请取消选择该 PID 后重试。`;
      } else if (errorCode === BurstErrorCode.MultiRequestUnsupported) {
        msg = `所选 PID 包含多帧请求命令 ${responseData?.rejectedCommand ?? '?'}（seqId=${responseData?.rejectedSeqId ?? '?'}），该类型在当前版本不支持 Burst Replay。请取消选择该 PID 后重试。`;
      } else {
        msg = err?.message || String(err);
      }

      console.warn(`[BurstSession] error errorCode=${errorCode}: ${msg}`);
      setError(msg);
      setMode(abortSignalRef.current.aborted ? 'aborted' : 'error');

      if (sessionId) {
        try {
          await CloudBridge.abortBurstSession({ sessionId, reason: msg });
        } catch (abortErr) {
          console.warn('[BurstSession] abort 请求失败:', abortErr);
        }
      }

      return null;

    } finally {
      CloudBridge.burstModeActive = false;
      console.log('[BurstSession] burstModeActive = false');
      sessionIdRef.current = null;
    }
  }

  /**
   * 中止 Burst 并等待 executeBurst 真正结束。
   * 返回的 Promise 在 executeBurst 的 finally 执行完后 resolve。
   */
  const abortBurstAndWait = useCallback(async (): Promise<void> => {
    abortSignalRef.current.aborted = true;
    assemblerRef.current?.abort();
    console.log('[BurstSession] abortBurstAndWait: signal + assembler.abort()');

    // 等待 executeBurst 的 promise settle（含 catch + finally）
    if (executionPromiseRef.current) {
      try { await executionPromiseRef.current; } catch { /* 忽略，已在内部处理 */ }
    }
    console.log('[BurstSession] abortBurstAndWait: 完成');
  }, []);

  // 保留同步版本供 unmount cleanup 使用（useEffect cleanup 不能 async）
  const abortBurst = useCallback(() => {
    abortSignalRef.current.aborted = true;
    assemblerRef.current?.abort();
    console.log('[BurstSession] abortBurst: signal + assembler.abort()');
  }, []);

  return {
    executeBurst,
    abortBurst,
    abortBurstAndWait,
    mode,
    error,
    progress,
  };
}
