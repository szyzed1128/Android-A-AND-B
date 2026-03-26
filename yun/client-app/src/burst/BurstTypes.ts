/**
 * Burst Snapshot Mode - DTO 类型定义
 *
 * 纯类型文件，不包含任何运行时逻辑。
 * 与 A 端 OBDCloudManager 的 Burst 序列化格式保持一致。
 */

export interface BurstRequestDescriptor {
  seqId: number;
  command: string;
  header: string;
  effectiveHeader: string;
  skipATSH: boolean;
  elmFormat: string;
  responseMarker: string;
  pidIds: number[];
  pidNames: string[];
  constituentPidIds?: number[][];
  subRequests?: Array<{
    command: string;
    pidIds: number[];
    expectedDataLength: number;
  }>;
  beforeCommands?: string[];
  afterCommands?: string[];
  checkLength?: boolean;
  skipCyclesTarget?: number;
  isMultiRequest: boolean;
  source: string;
  expectedResponseBytes?: number;
  timeoutMs?: number;
  /** OBDRequest.Repeat：true 时 StartLoopV3 会自动将该请求重入队，replay 时必须设 SkipCyclesTarget=0 */
  repeat: boolean;
  /** OBDRequest.DoNotDecode：true 时 DecodeData 跳过解析 */
  doNotDecode: boolean;
}

export interface BurstSampleEnvelope {
  sessionId: string;
  seqId: number;
  cycleIndex: number;
  sampleOrdinal: number;
  command: string;
  header: string;
  sentAtMs: number;
  completedAtMs: number;
  timestampMs: number;
  elapsedMs: number;
  rawElmText: string;
  normalizedPayloadHex: string;
  responseHeader: string;
  promptSeen: boolean;
  timeout: boolean;
  transportError?: string;
  completedBy: 'prompt' | 'timeout' | 'abort';
  /** ReplayConnection 使用的匹配键：command + "|" + effectiveHeader */
  exchangeKey: string;
  parseHint?: {
    pidIds: number[];
    isNoData: boolean;
    isError: boolean;
  };
}

export interface BurstParsedResult {
  sessionId: string;
  seqId: number;
  cycleIndex: number;
  sampleOrdinal: number;
  pidId: number;
  pidName: string;
  value: number | string | object;
  rawValue?: string;
  unit: number;
  displayText: string;
  timestampMs: number;
  parseOk: boolean;
  error?: string;
}

export type BurstSessionMode =
  | 'idle'
  | 'preparing'
  | 'sampling'
  | 'committing'
  | 'finished'
  | 'aborted'
  | 'error';

export type BurstSessionStatus =
  | 'ok'
  | 'timeout'
  | 'transport_error'
  | 'parse_error'
  | 'aborted';

export type BurstKeepalivePolicy = 'paused';

export interface BurstSessionState {
  sessionId: string;
  mode: BurstSessionMode;
  owner: 'A' | 'B';
  keepalivePolicy: BurstKeepalivePolicy;
  startedAt: number;
  finishedAt?: number;
  status: BurstSessionStatus;
  totalCycles: number;
  descriptorCount: number;
  sampleCount: number;
  parseSuccessCount: number;
  parseFailCount: number;
  avgCycleDurationMs?: number;
  lastCycleStartMs?: number;
  error?: string;
  abortReason?: string;
}
