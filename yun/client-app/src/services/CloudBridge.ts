/**
 * CloudBridge - 云端通信服务
 *
 * 用于与云端 A (运行 CarScanner 业务逻辑的 Android 模拟器) 通信
 * 替代原 JSBridge，通过 WebSocket 发送诊断命令并接收结果
 */

import { MessageType, MessageAction, WSMessage, BurstErrorCode } from '../protocol/MessageProtocol';

// 回调类型定义
export interface DTCCallbacks {
  onSuccess?: (data: any) => void;
  onProgress?: (ecuIndex: number, dtcList: any[]) => void;
  onError?: (error: string) => void;
  onFinish?: () => void;
}

export interface ClearDTCCallbacks {
  onSuccess?: (data: any) => void;
  onError?: (error: string) => void;
  onFinish?: () => void;
}

export interface ECUInfoCallbacks {
  onSuccess?: (data: any) => void;
  onProgress?: (ecuIndex: number, infoList: any[]) => void;
  onError?: (error: string) => void;
  onFinish?: () => void;
}

export interface FreezeFrameCallbacks {
  onSuccess?: (data: any[]) => void;
  onError?: (error: string) => void;
  onFinish?: () => void;
}

// Profile 类型
export interface Profile {
  Name: string;
  Description?: string;
}

// ECU 类型
export interface ECUItem {
  name: string;
  selected?: boolean;   // CarScanner 序列化的 ECUExists：true = 该 ECU 在车上存活
}

// PID 类型
export interface PIDItem {
  NM?: string;
  SNM?: string;
  Value?: any;
  Units?: number | string;
  IsAvailable?: boolean;
  Id?: number;
  MIN?: number;
  MAX?: number;
  CMD?: string;
  originalIndex?: number;
  index?: number;
  value?: string;
  pid?: string;
  name?: string;
  unit?: number;
}

// 待处理请求
interface PendingRequest {
  resolve: (value: any) => void;
  reject: (error: any) => void;
  timeout: ReturnType<typeof setTimeout>;
  action: string;
  startTime: number;
}

interface BurstReplayWaiter {
  resolve: (value: { results: any[]; totalCycles: number }) => void;
  reject: (error: any) => void;
  timeout: ReturnType<typeof setTimeout>;
  abortPoll?: ReturnType<typeof setInterval>;
}

interface BurstReplayTerminalEvent {
  kind: 'completed' | 'failed';
  data: any;
}

interface DisconnectHandleResult {
  handled: boolean;
  error?: string;
  sessionId?: string | null;
}

// 事件监听器类型
type EventListener = (data: any) => void;

/**
 * CloudBridge 类
 */
class CloudBridgeService {
  private ws: WebSocket | null = null;
  private pendingRequests: Map<string, PendingRequest> = new Map();
  private eventListeners: Map<string, EventListener[]> = new Map();
  private burstReplayWaiters: Map<string, BurstReplayWaiter> = new Map();
  private burstReplayTerminalEvents: Map<string, BurstReplayTerminalEvent> = new Map();
  private ignoredReplayTerminalSessions: Map<string, ReturnType<typeof setTimeout>> = new Map();
  private isConnected: boolean = false;
  private reconnectAttempts: number = 0;
  private maxReconnectAttempts: number = 5;
  private requestTimeout: number = 30000; // 30 seconds
  private connectionListeners: Set<(connected: boolean) => void> = new Set();
  private obdStatusListeners: Set<(status: string) => void> = new Set();
  // 仅允许在”用户主动断开”窗口内响应 A 端的断开请求，避免状态抖动导致误断开
  private allowRemoteDisconnectUntil: number = 0;
  // 新连接启动后，在此时间窗口内忽略滞后的 OBDStatusChanged: Disconnected。
  // 旧 disconnectAsync 在 fire-and-forget 后约 6 秒才完成，会触发 A 端状态轮询发出 Disconnected。
  private ignoreDisconnectedUntil: number = 0;

  // Burst Snapshot Mode：B 端转发抑制标志
  // true 时 useBluetoothBridge 不调用 sendOBDData
  public burstModeActive: boolean = false;

  // Burst Replay Mode：A 端 replay 完成/失败事件回调
  // useBurstSession.ts 通过 waitForReplayResult(sessionId) 按会话等待终态事件，
  // 其余消费方仍可直接订阅这两个公共回调。
  public onBurstReplayCompleted?: (data: any) => void;
  public onBurstReplayFailed?: (data: any) => void;

  // 当前蓝牙会话ID
  private currentSessionId: string | null = null;

  // 当前 OBD 状态（用于过滤诊断操作触发的中间态）
  private currentOBDStatus: string = '';

  // 诊断日志：时序与序号
  private btSeq = 0;

  // [TIMING] 两段式时序追踪：
  // _btTimingQueue：handleBluetoothSend 写入，sendOBDData 消费（BT 往返段）
  // _pidTimingPending：sendOBDData 写入，pidValueChanged 消费（A处理+WS段）
  // 原因：A端在发下一条命令(send)的同时几乎同步发出上一轮的 pidValueChanged，
  //       因此 send 和 pidValueChanged 不是同一周期首尾，sendOBDData↔pidValueChanged 才是真正的一对。
  private _btTimingQueue: Array<{ cmdText: string; tSend: number; tBtWrite: number }> = [];
  private _pidTimingPending: {
    cmdText: string;
    tSend: number;       // A发AT命令到达B
    tBtWrite: number;    // B写入ELM327完成
    tElmReply: number;   // ELM327回复数据（sendOBDData入口）
    tForwarded: number;  // B转发数据给A（sendMessage后）
  } | null = null;
  private _lastPidT5: Map<string, number> = new Map();  // 各 PID 上次 UI 更新时间戳
  private ts(): string {
    const d = new Date();
    return `[${String(d.getHours()).padStart(2,'0')}:${String(d.getMinutes()).padStart(2,'0')}:${String(d.getSeconds()).padStart(2,'0')}.${String(d.getMilliseconds()).padStart(3,'0')}]`;
  }
  private flowLog(direction: string, description: string, detail?: string): void {
    const dir = direction.padEnd(10);
    const detailStr = detail ? ` | ${String(detail).substring(0, 120)}` : '';
    console.log(`[FLOW] ${this.ts()} ${dir} | ${description}${detailStr}`);
  }
  private decodeB64(b64: string, maxLen = 120): string {
    try {
      const buf = typeof Buffer !== 'undefined'
        ? Buffer.from(b64, 'base64').toString('utf-8')
        : atob(b64);
      return buf.replace(/\r/g, '\\r').replace(/\n/g, '\\n').substring(0, maxLen);
    } catch {
      return `(b64 len=${b64?.length ?? 0})`;
    }
  }

  // 连接状态回调
  public onConnectionChanged?: (connected: boolean) => void;
  public onOBDStatusChanged?: (status: string) => void;
  public onPIDValueChanged?: (data: PIDItem) => void;

  // 蓝牙桥接回调（由 useBluetoothBridge 设置）
  public onBluetoothSendRequest?: (base64Data: string) => Promise<void>;
  public onBluetoothConnectRequest?: (protocol: string, address: string) => Promise<string>;
  public onBluetoothDisconnectRequest?: () => Promise<void>;
  public onBluetoothStartScanRequest?: (protocols?: string[]) => Promise<void>;
  public onBluetoothStopScanRequest?: () => Promise<void>;

  /**
   * 连接到云端 A
   */
  async connect(host: string, port: number): Promise<void> {
    return new Promise((resolve, reject) => {
      const hasProtocol = /^wss?:\/\//i.test(host);
      let base = hasProtocol ? host : `ws://${host}:${port}`;
      const pathPart = base.replace(/^wss?:\/\/[^/]+/i, '');
      if (!pathPart) {
        base = `${base}/ws`;
      }
      const url = base;
      console.log('[CloudBridge] Connecting to:', url);

      try {
        this.ws = new WebSocket(url);

        this.ws.onopen = () => {
          console.log('[CloudBridge] Connected');
          this.isConnected = true;
          this.reconnectAttempts = 0;
          this.emitConnectionChanged(true);
          resolve();
        };

        this.ws.onclose = (event) => {
          console.log('[CloudBridge] Disconnected:', event.code, event.reason);
          this.isConnected = false;
          this.resetTransportScopedOBDState(`ws.onclose:${event.code}:${event.reason || 'no_reason'}`);
          this.emitConnectionChanged(false);
          this.rejectAllPending('Connection closed');
          this.rejectAllReplayWaiters('Connection closed');
          this.burstReplayTerminalEvents.clear();
        };

        this.ws.onerror = (error) => {
          console.error('[CloudBridge] Error:', error);
          if (!this.isConnected) {
            reject(new Error('Connection failed'));
          }
        };

        this.ws.onmessage = (event) => {
          this.handleMessage(event.data);
        };
      } catch (e) {
        reject(e);
      }
    });
  }

  /**
   * 断开连接
   */
  disconnect(): void {
    if (this.ws) {
      this.ws.close();
      this.ws = null;
    }
    this.isConnected = false;
    this.resetTransportScopedOBDState('manual_disconnect');
    this.rejectAllPending('Disconnected');
    this.rejectAllReplayWaiters('Disconnected');
    this.burstReplayTerminalEvents.clear();
  }

  addConnectionListener(listener: (connected: boolean) => void): () => void {
    this.connectionListeners.add(listener);
    return () => {
      this.connectionListeners.delete(listener);
    };
  }

  addOBDStatusListener(listener: (status: string) => void): () => void {
    this.obdStatusListeners.add(listener);
    return () => {
      this.obdStatusListeners.delete(listener);
    };
  }

  /**
   * 获取连接状态
   */
  getIsConnected(): boolean {
    return this.isConnected;
  }

  // ===== 车辆配置命令 =====

  /**
   * 获取车辆品牌列表
   */
  async getBrands(): Promise<string[]> {
    const result = await this.sendRequest(MessageAction.GetBrands);
    return this.parseResult(result, []);
  }

  /**
   * 获取品牌下的配置列表
   */
  async getProfiles(brand: string): Promise<Profile[]> {
    const result = await this.sendRequest(MessageAction.GetProfiles, { brand });
    return this.parseResult(result, []);
  }

  /**
   * 应用车辆配置
   */
  async applyProfile(brand: string, profileIndex: number): Promise<void> {
    await this.sendRequest(MessageAction.ApplyProfile, { brand, profileIndex });
  }

  // ===== ECU 命令 =====

  /**
   * 获取 ECU 列表
   */
  async getECUList(): Promise<ECUItem[]> {
    const result = await this.sendRequest(MessageAction.GetECUList);
    return this.parseResult(result, []);
  }

  /**
   * 读取 ECU 信息（异步回调）
   */
  readECUInfoAsync(indices: number[], callbacks: ECUInfoCallbacks): void {
    this.registerCallbacks('ecuInfo', callbacks);
    this.pendingMeta.set('ecuInfo', { indices });
    this.sendMessage({
      type: MessageType.Request,
      action: MessageAction.ReadECUInfo,
      requestId: this.generateId(),
      data: { indices },
      timestamp: Date.now(),
    });
  }

  // ===== 故障码命令 =====

  /**
   * 读取故障码（异步回调）
   */
  readDTCAsync(indices: number[], callbacks: DTCCallbacks): void {
    this.registerCallbacks('dtc', callbacks);
    this.pendingMeta.set('dtc', { indices });
    this.sendMessage({
      type: MessageType.Request,
      action: MessageAction.ReadDTC,
      requestId: this.generateId(),
      data: { indices },
      timestamp: Date.now(),
    });
  }

  /**
   * 清除故障码（异步回调）
   */
  clearDTCAsync(indices: number[], callbacks: ClearDTCCallbacks): void {
    this.registerCallbacks('clearDtc', callbacks);
    this.pendingMeta.set('clearDtc', { indices });
    this.sendMessage({
      type: MessageType.Request,
      action: MessageAction.ClearDTC,
      requestId: this.generateId(),
      data: { indices },
      timestamp: Date.now(),
    });
  }

  // ===== 冻结帧命令 =====

  /**
   * 读取冻结帧（异步回调）
   */
  readFreezeFrameAsync(frameIndex: number, callbacks: FreezeFrameCallbacks): void {
    this.registerCallbacks('freezeFrame', callbacks);
    this.pendingMeta.set('freezeFrame', { frameIndex });
    this.sendMessage({
      type: MessageType.Request,
      action: MessageAction.ReadFreezeFrame,
      requestId: this.generateId(),
      data: { frameIndex },
      timestamp: Date.now(),
    });
  }

  // ===== 实时数据命令 =====

  /**
   * 获取 PID 列表
   */
  async getPIDList(): Promise<PIDItem[]> {
    const result = await this.sendRequest(MessageAction.GetPIDList);
    const parsed = this.parseResult(result, []);
    return this.normalizePidList(parsed);
  }

  /**
   * 开始读取 PID（实时流）
   */
  startReadPIDs(indices: number[]): void {
    this.sendMessage({
      type: MessageType.Request,
      action: MessageAction.StartReadPIDs,
      requestId: this.generateId(),
      data: { indices },
      timestamp: Date.now(),
    });
  }

  /**
   * 停止读取 PID（等待 A 端确认，3000ms 超时后静默失败）
   */
  async stopReadPIDs(): Promise<void> {
    try {
      await this.sendRequest(MessageAction.StopReadPIDs, undefined, 3000);
    } catch (e) {
      // 超时或失败时静默处理（不阻塞 UI 状态更新）
      console.warn('[CloudBridge] stopReadPIDs 未收到确认:', e);
    }
  }

  // ===== Burst Snapshot Mode 命令 =====

  /**
   * 请求 A 端准备 Burst Session
   * 返回 { sessionId, descriptors, pingCommandText } 成功
   * 失败时 error.errorCode 为 BurstErrorCode.LowPriorityConflict 或 BurstErrorCode.BypassConflict，
   * error.responseData 含 conflictingPids[] 或 conflictDetails[] 结构化信息
   */
  async prepareBurstSession(pidIndices: number[]): Promise<any> {
    return await this.sendRequest(MessageAction.PrepareBurstSession, { pidIndices }, 30000);
  }

  /**
   * 提交 Burst 采样数据给 A 端，并在 final chunk 时触发 replay
   * 非 final 返回累计 ACK；final 返回 { sessionId, replayStarted }
   */
  async commitBurstSamples(data: {
    sessionId: string;
    samples: any[];
    chunkIndex: number;
    totalChunks: number;
    isFinal: boolean;
    totalCycles?: number;
  }): Promise<any> {
    return await this.sendRequest(MessageAction.CommitBurstSamples, data, 60000);
  }

  /**
   * 中止 Burst Session
   */
  async abortBurstSession(data: { sessionId: string; reason?: string }): Promise<any> {
    return await this.sendRequest(MessageAction.AbortBurstSession, data, 10000);
  }

  async waitForReplayResult(
    sessionId: string,
    abortSignal?: { aborted?: boolean } | null,
  ): Promise<{ results: any[]; totalCycles: number }> {
    if (!sessionId) {
      throw new Error('sessionId 为空');
    }

    if (abortSignal?.aborted) {
      this.markReplayResultIgnored(sessionId, 'aborted-before-wait');
      throw new Error('用户取消');
    }

    const terminalEvent = this.burstReplayTerminalEvents.get(sessionId);
    if (terminalEvent) {
      this.burstReplayTerminalEvents.delete(sessionId);
      if (terminalEvent.kind === 'completed') {
        console.log(`[CloudBridge] waitForReplayResult 命中缓存完成 sessionId=${sessionId}`);
        return this.normalizeReplayCompletedData(terminalEvent.data);
      }

      console.warn(`[CloudBridge] waitForReplayResult 命中缓存失败 sessionId=${sessionId}`);
      const err = new Error(terminalEvent.data?.reason || 'Burst Replay 失败') as any;
      err.errorCode = 'BURST_REPLAY_FAILED';
      err.responseData = terminalEvent.data;
      throw err;
    }

    this.clearIgnoredReplaySession(sessionId);

    return await new Promise((resolve, reject) => {
      const existing = this.burstReplayWaiters.get(sessionId);
      if (existing) {
        console.warn(`[CloudBridge] waitForReplayResult 覆盖旧等待器 sessionId=${sessionId}`);
        this.clearReplayWaiter(sessionId, 'replaced');
        try { existing.reject(new Error('已有同 sessionId 的 replay 等待器')); } catch { }
      }

      const timeout = setTimeout(() => {
        console.warn(`[CloudBridge] waitForReplayResult 超时 sessionId=${sessionId}`);
        this.clearReplayWaiter(sessionId, 'timeout');
        this.markReplayResultIgnored(sessionId, 'timeout');
        reject(new Error('等待 Burst Replay 结果超时'));
      }, 5 * 60 * 1000);

      const waiter: BurstReplayWaiter = {
        resolve,
        reject,
        timeout,
      };

      if (abortSignal) {
        waiter.abortPoll = setInterval(() => {
          if (abortSignal.aborted) {
            console.warn(`[CloudBridge] waitForReplayResult 用户取消 sessionId=${sessionId}`);
            this.clearReplayWaiter(sessionId, 'abort');
            this.markReplayResultIgnored(sessionId, 'abort');
            reject(new Error('用户取消'));
          }
        }, 100);
      }

      this.burstReplayWaiters.set(sessionId, waiter);
      console.log(`[CloudBridge] waitForReplayResult 注册 sessionId=${sessionId}`);
    });
  }

  // ===== ELM327 连接命令 =====

  /**
   * 连接 ELM327 设备（通过云端）
   */
  async connectOBD(protocol: string, address: string, sessionId?: string): Promise<void> {
    // 关闭旧断开窗口：防止前一次 disconnectAsync 的滞后 Disconnect 动作断掉新会话
    this.allowRemoteDisconnectUntil = 0;
    // 开启"忽略滞后 Disconnected"窗口（10s）：
    // A 端 fire-and-forget 的 disconnectAsync 约 6s 后完成，完成时 A 端状态轮询会发出
    // OBDStatusChanged: Disconnected，若不过滤会打断正在进行的新连接流程。
    this.ignoreDisconnectedUntil = Date.now() + 10000;

    const optimisticSessionId = sessionId ?? null;
    if (optimisticSessionId) {
      this.currentSessionId = optimisticSessionId;
    }

    try {
      const result = await this.sendRequest(MessageAction.OBDConnect, { protocol, address, sessionId });
      const responseSessionId = result?.sessionId ?? result?.SessionId ?? optimisticSessionId;
      if (responseSessionId) {
        this.currentSessionId = responseSessionId;
      }
    } catch (e) {
      if (optimisticSessionId && this.currentSessionId === optimisticSessionId) {
        this.currentSessionId = null;
      }
      throw e;
    }
  }

  /**
   * 断开 ELM327 设备
   */
  async disconnectOBD(): Promise<void> {
    if (!this.currentSessionId) {
      console.log('[CloudBridge] 跳过 obdDisconnect：当前无活动 sessionId');
      return;
    }

    // 用户主动断开：关闭”忽略滞后 Disconnected”窗口，允许真实的 Disconnected 状态通过
    this.ignoreDisconnectedUntil = 0;
    // 标记”允许远端断开”的短窗口（用户主动断开）
    this.allowRemoteDisconnectUntil = Date.now() + 15000;
    await this.sendRequest(MessageAction.OBDDisconnect);
  }

  // ===== 蓝牙桥接方法 =====

  /**
   * 发送诊断数据到云端A（蓝牙接收到数据后调用）
   */
  sendOBDData(sessionId: string, base64Data: string): void {
    if (!this.isConnected || !this.ws) {
      console.warn('[CloudBridge] Not connected, cannot send OBD data');
      return;
    }

    const timingT2 = Date.now();                                                    // [TIMING] t2: ELM327回复数据（B收到）
    // 从队列取出对应的 BT 计时，组合成完整的 pending 条目供 pidValueChanged 消费
    const btEntry = this._btTimingQueue.shift();
    if (btEntry) {
      this._pidTimingPending = { ...btEntry, tElmReply: timingT2, tForwarded: 0 };
    }

    const message: WSMessage = {
      type: MessageType.Event,
      action: MessageAction.OBDData,
      sessionId,
      data: base64Data,
      timestamp: Date.now(),
    };

    const preview = this.decodeB64(base64Data);
    // 检查完整响应中是否包含 ELM327 '>' 终结符
    let hasPrompt = false;
    try {
      const full = typeof Buffer !== 'undefined'
        ? Buffer.from(base64Data, 'base64').toString('ascii')
        : atob(base64Data);
      hasPrompt = full.includes('>');
    } catch {}
    console.log(`[CloudBridge] ${this.ts()} ◀◀ ELM327→B→A session=${sessionId} has_prompt=${hasPrompt} (b64len=${base64Data?.length || 0}): [${preview}]`);
    this.flowLog('B>A', 'ELM数据转发', `session=${sessionId} len=${base64Data.length} has_prompt=${hasPrompt}`);
    this.sendMessage(message);
    if (this._pidTimingPending && this._pidTimingPending.tForwarded === 0) {
      this._pidTimingPending.tForwarded = Date.now();                               // [TIMING] t3: B转发数据给A完成
    }
  }

  /**
   * 通知云端A：蓝牙连接已丢失
   */
  sendConnectionLost(sessionId: string, reason: string): void {
    if (!this.isConnected || !this.ws) {
      console.warn('[CloudBridge] Not connected, cannot send ConnectionLost');
      return;
    }

    const message: WSMessage = {
      type: MessageType.Event,
      action: MessageAction.ConnectionLost,
      sessionId,
      data: { reason },
      timestamp: Date.now(),
    };

    console.warn('[CloudBridge] Sending ConnectionLost to cloud:', reason);
    this.sendMessage(message);
    if (this.currentSessionId === sessionId) {
      this.currentSessionId = null;
    }
  }

  /**
   * 发送扫描设备事件给云端A
   */
  sendDeviceDiscovered(device: any): void {
    if (!this.isConnected || !this.ws) return;
    this.sendMessage({
      type: MessageType.Event,
      action: MessageAction.DeviceDiscovered,
      data: device,
      timestamp: Date.now(),
    });
  }

  /**
   * 发送扫描结束事件给云端A
   */
  sendScanFinished(): void {
    if (!this.isConnected || !this.ws) return;
    this.sendMessage({
      type: MessageType.Event,
      action: MessageAction.ScanFinished,
      timestamp: Date.now(),
    });
  }

  /**
   * 设置当前蓝牙会话ID
   */
  setSessionId(sessionId: string | null): void {
    this.currentSessionId = sessionId;
  }

  /**
   * 获取当前蓝牙会话ID
   */
  getSessionId(): string | null {
    return this.currentSessionId;
  }

  // ===== 私有方法 =====

  /**
   * 发送请求并等待响应
   */
  private sendRequest(action: string, data?: any, timeoutMs?: number): Promise<any> {
    return new Promise((resolve, reject) => {
      if (!this.isConnected || !this.ws) {
        reject(new Error('Not connected to cloud'));
        return;
      }

      const requestId = this.generateId();
      const actualTimeout = timeoutMs ?? this.requestTimeout;
      const timeout = setTimeout(() => {
        this.pendingRequests.delete(requestId);
        console.warn(`[CloudBridge] TIMEOUT action=${action} req=${requestId} after ${actualTimeout}ms`);
        reject(new Error('Request timeout'));
      }, actualTimeout);

      this.pendingRequests.set(requestId, {
        resolve,
        reject,
        timeout,
        action,
        startTime: Date.now(),
      });

      const message: WSMessage = {
        type: MessageType.Request,
        action,
        requestId,
        data,
        timestamp: Date.now(),
      };

      this.flowLog('B>A', `发出请求: ${action}`);
      this.sendMessage(message);
    });
  }

  /**
   * 发送消息
   */
  private sendMessage(message: WSMessage): void {
    if (this.ws && this.isConnected) {
      const json = JSON.stringify(message);
      console.log(`[CloudBridge] TX type=${message.type} action=${message.action} req=${message.requestId || '-'} session=${message.sessionId || '-'} data=${this.describeData(message.data)}`);
      this.ws.send(json);
    } else {
      console.warn(`[CloudBridge] ⚠️ 消息被丢弃(未连接) type=${message.type} action=${message.action} ws=${!!this.ws} connected=${this.isConnected}`);
    }
  }

  /**
   * 处理接收到的消息
   */
  private handleMessage(data: string): void {
    // 诊断：记录原始数据（解析前）
    const preview = data.length > 200 ? data.substring(0, 200) + '...' : data;
    console.log(`[CloudBridge] RAW-RX len=${data.length} preview=${preview}`);

    try {
      const raw = JSON.parse(data);
      const message: WSMessage = {
        type: raw?.type ?? raw?.Type,
        action: raw?.action ?? raw?.Action,
        requestId: raw?.requestId ?? raw?.RequestId,
        sessionId: raw?.sessionId ?? raw?.SessionId,
        success: raw?.success ?? raw?.Success,
        error: raw?.error ?? raw?.Error,
        data: raw?.data ?? raw?.Data,
        timestamp: raw?.timestamp ?? raw?.Timestamp,
      };

      if (typeof message.data === 'string') {
        const trimmed = message.data.trim();
        if ((trimmed.startsWith('{') && trimmed.endsWith('}')) ||
            (trimmed.startsWith('[') && trimmed.endsWith(']'))) {
          try {
            message.data = JSON.parse(trimmed);
          } catch {
            // ignore parse failure, keep original string
          }
        }
      }
      console.log(`[CloudBridge] RX type=${message.type} action=${message.action} req=${message.requestId || '-'} session=${message.sessionId || '-'} data=${this.describeData(message.data)}`);

      // 处理响应
      if (message.type === MessageType.Response && message.requestId) {
        const pending = this.pendingRequests.get(message.requestId);
        if (pending) {
          clearTimeout(pending.timeout);
          this.pendingRequests.delete(message.requestId);

          const elapsed = Date.now() - pending.startTime;
          console.log(`[CloudBridge] RESP action=${pending.action} req=${message.requestId} ok=${message.success} ms=${elapsed}`);

          if (message.success) {
            pending.resolve(message.data);
          } else {
            // 附加 errorCode 和 responseData，供调用方区分结构化错误（如 Burst 冲突）
            const err = new Error(message.error || 'Request failed') as any;
            err.errorCode = message.error;
            err.responseData = message.data;
            pending.reject(err);
          }
        }
        return;
      }

      // 处理来自云端A的蓝牙桥接请求
      if (message.type === MessageType.Request) {
        this.handleBluetoothRequest(message);
        return;
      }

      // 处理事件
      if (message.type === MessageType.Event) {
        this.handleEvent(message);
        return;
      }

      // 忽略A端UI回调转发（B端通过自己的请求-响应通道独立接收诊断数据）
      if (message.type === 'ui_event') {
        return;
      }

      console.warn('[CloudBridge] Unknown message type:', message.type);
    } catch (e) {
      console.error('[CloudBridge] Parse error:', e);
    }
  }

  /**
   * 处理来自云端A的蓝牙桥接请求
   */
  private async handleBluetoothRequest(message: WSMessage): Promise<void> {
    const requestId = message.requestId;

    try {
      console.log(`[CloudBridge] BT-REQ action=${message.action} req=${requestId || '-'} data=${this.describeData(message.data)}`);
      switch (message.action) {
        case MessageAction.Send:
          await this.handleBluetoothSend(message);
          this.sendResponse(requestId, message.action, true);
          break;

        case MessageAction.Connect:
          const sessionId = await this.handleBluetoothConnect(message);
          this.sendResponse(requestId, message.action, true, { sessionId }, undefined, sessionId);
          break;

        case MessageAction.Disconnect:
          const disconnectResult = await this.handleBluetoothDisconnect(message);
          if (disconnectResult.handled) {
            this.sendResponse(requestId, message.action, true, undefined, undefined, disconnectResult.sessionId || undefined);
          } else {
            this.sendResponse(
              requestId,
              message.action,
              false,
              { ignored: true, sessionId: disconnectResult.sessionId ?? null },
              disconnectResult.error || 'Disconnect ignored',
              disconnectResult.sessionId || undefined
            );
          }
          break;

        case MessageAction.StartScan:
          await this.handleBluetoothStartScan(message);
          this.sendResponse(requestId, message.action, true);
          break;

        case MessageAction.StopScan:
          await this.handleBluetoothStopScan(message);
          this.sendResponse(requestId, message.action, true);
          break;

        case MessageAction.Ping:
          this.sendMessage({
            type: MessageType.Response,
            action: MessageAction.Pong,
            requestId,
            success: true,
            timestamp: Date.now(),
          });
          break;

        default:
          console.log('[CloudBridge] Unhandled request:', message.action);
          this.sendResponse(requestId, message.action, false, null, 'Unknown action');
      }
    } catch (e: any) {
      console.error('[CloudBridge] Bluetooth request error:', e);
      this.sendResponse(requestId, message.action, false, null, e.message || 'Request failed');
    }
  }

  /**
   * 处理蓝牙发送请求（A发送AT命令到B，B转发到蓝牙）
   */
  private async handleBluetoothSend(message: WSMessage): Promise<void> {
    const base64Data = message.data as string;

    if (!this.onBluetoothSendRequest) {
      throw new Error('Bluetooth bridge not initialized');
    }

    const seq = ++this.btSeq;
    const preview = this.decodeB64(base64Data);
    const timingT0 = Date.now();
    let cmdText = '?';
    try {
      cmdText = Buffer.from(base64Data, 'base64').slice(0, 8).toString('ascii')
        .replace(/\r/g, '\\r').replace(/\n/g, '\\n').replace(/[^\x20-\x7E\\]/g, '?');
    } catch {}
    // 在 await 前入队，防止 await 期间 sendOBDData 被调用时队列为空
    const btEntry = { cmdText, tSend: timingT0, tBtWrite: 0 };
    this._btTimingQueue.push(btEntry);
    console.log(`[CloudBridge] ${this.ts()} ▶▶ BT#${seq} A→B→ELM327: [${preview}] (b64len=${base64Data?.length || 0})`);
    await this.onBluetoothSendRequest(base64Data);
    btEntry.tBtWrite = Date.now();                                                  // [TIMING] t1: B写入ELM327完成（原地更新）
    console.log(`[CloudBridge] ${this.ts()} ✓ BT#${seq} 转发完成 (耗时=${btEntry.tBtWrite - timingT0}ms)`);
  }

  /**
   * 处理蓝牙连接请求
   */
  private async handleBluetoothConnect(message: WSMessage): Promise<string> {
    const data = (message.data || {}) as any;
    const protocol = data.protocol ?? data.Protocol;
    const address = data.address ?? data.Address;

    if (!this.onBluetoothConnectRequest) {
      throw new Error('Bluetooth bridge not initialized');
    }

    const connStart = Date.now();
    console.log(`[CloudBridge] ${this.ts()} 蓝牙连接请求: protocol=${protocol} address=${address}`);
    const sessionId = await this.onBluetoothConnectRequest(protocol, address);
    this.currentSessionId = sessionId;
    console.log(`[CloudBridge] ${this.ts()} 蓝牙连接成功: sessionId=${sessionId} (耗时=${Date.now()-connStart}ms)`);
    return sessionId;
  }

  /**
   * 处理蓝牙断开请求
   */
  private async handleBluetoothDisconnect(message: WSMessage): Promise<DisconnectHandleResult> {
    if (!this.onBluetoothDisconnectRequest) {
      throw new Error('Bluetooth bridge not initialized');
    }

    const remoteSessionId = message.sessionId ?? null;
    const currentSessionId = this.currentSessionId;

    if (remoteSessionId) {
      if (!currentSessionId) {
        const error = `忽略远端断开请求：当前无活动会话 remote=${remoteSessionId}`;
        console.warn(`[CloudBridge] ${error}`);
        return { handled: false, error, sessionId: remoteSessionId };
      }

      if (remoteSessionId !== currentSessionId) {
        const error = `忽略远端断开请求：sessionId 不匹配 remote=${remoteSessionId} current=${currentSessionId}`;
        console.warn(`[CloudBridge] ${error}`);
        return { handled: false, error, sessionId: remoteSessionId };
      }

      console.log(`[CloudBridge] Disconnecting Bluetooth by matched sessionId=${remoteSessionId}`);
      this.allowRemoteDisconnectUntil = 0;
      await this.onBluetoothDisconnectRequest();
      if (this.currentSessionId === remoteSessionId) {
        this.currentSessionId = null;
      }
      return { handled: true, sessionId: remoteSessionId };
    }

    const now = Date.now();
    if (now > this.allowRemoteDisconnectUntil) {
      const error = '忽略远端断开请求：缺少 sessionId 且未在允许窗口内';
      console.warn(`[CloudBridge] ${error}`);
      return { handled: false, error, sessionId: null };
    }

    console.log('[CloudBridge] Disconnecting Bluetooth (fallback allow window)');
    this.allowRemoteDisconnectUntil = 0;
    await this.onBluetoothDisconnectRequest();
    this.currentSessionId = null;
    return { handled: true, sessionId: null };
  }

  /**
   * 处理蓝牙开始扫描请求
   */
  private async handleBluetoothStartScan(message: WSMessage): Promise<void> {
    if (!this.onBluetoothStartScanRequest) {
      throw new Error('Bluetooth bridge not initialized');
    }
    const protocols = message.data?.protocols as string[] | undefined;
    await this.onBluetoothStartScanRequest(protocols);
  }

  /**
   * 处理蓝牙停止扫描请求
   */
  private async handleBluetoothStopScan(message: WSMessage): Promise<void> {
    if (!this.onBluetoothStopScanRequest) {
      throw new Error('Bluetooth bridge not initialized');
    }
    await this.onBluetoothStopScanRequest();
  }

  private describeData(data: any): string {
    if (data == null) return 'null';
    if (typeof data === 'string') return `string(len=${data.length})`;
    if (Array.isArray(data)) return `array(len=${data.length})`;
    try {
      const json = JSON.stringify(data);
      return `json(len=${json?.length || 0})`;
    } catch {
      return typeof data;
    }
  }

  /**
   * 发送响应到云端A
   */
  private sendResponse(
    requestId: string | undefined,
    action: string,
    success: boolean,
    data?: any,
    error?: string,
    sessionId?: string
  ): void {
    if (!requestId) return;

    const response: WSMessage = {
      type: MessageType.Response,
      action,
      requestId,
      success,
      data,
      error,
      sessionId,
      timestamp: Date.now(),
    };

    this.sendMessage(response);
  }

  /**
   * 处理事件消息
   */
  private handleEvent(message: WSMessage): void {
    switch (message.action) {
      case MessageAction.OBDStatusChanged: {
        const newStatus = message.data?.status ?? message.data?.Status ?? message.data;
        console.log('[CloudBridge] OBDStatusChanged:', newStatus, '(当前:', this.currentOBDStatus, ')');
        this.flowLog('B<A', `OBD状态: ${newStatus}`);

        // 过滤延迟到达的 Disconnecting（已经 Disconnected 后不再接受 Disconnecting）
        if (this.currentOBDStatus === 'Disconnected' && newStatus === 'Disconnecting') {
          console.log(`[CloudBridge] 过滤延迟的 Disconnecting（当前已 Disconnected）`);
          break;
        }

        // 过滤新连接启动后滞后到达的 Disconnected：
        // A 端 disconnectAsync 为 fire-and-forget，约 6s 后完成，完成时状态轮询会发出
        // OBDStatusChanged: Disconnected，会打断正在进行的新连接流程。
        if (newStatus === 'Disconnected' && Date.now() < this.ignoreDisconnectedUntil) {
          console.log(`[CloudBridge] 过滤滞后的 Disconnected（新连接窗口内，忽略旧 disconnectAsync 尾声）`);
          break;
        }
        // 一旦收到真正的 Disconnected（窗口外）或连接成功，清除忽略窗口
        if (newStatus === 'Disconnected' || newStatus === 'ConnectedToECU') {
          this.ignoreDisconnectedUntil = 0;
        }

        if (newStatus === 'Disconnected') {
          this.currentSessionId = null;
          this.allowRemoteDisconnectUntil = 0;
        }

        // 若当前已完全连接到 ECU，过滤掉诊断操作触发的中间过渡状态
        // 允许：Disconnected（断开）、Disconnecting（断开中）、ConnectingToECU（重连）
        const allowedFromConnectedToECU = ['Disconnected', 'Disconnecting', 'ConnectingToECU'];
        if (this.currentOBDStatus === 'ConnectedToECU' &&
            !allowedFromConnectedToECU.includes(newStatus)) {
          console.log(`[CloudBridge] 过滤中间状态: ${newStatus}（当前已 ConnectedToECU）`);
          break;
        }

        this.currentOBDStatus = newStatus ?? '';
        this.emitOBDStatusChanged(newStatus);
        break;
      }

      case MessageAction.PIDValueChanged: {
        const timingT4 = Date.now();                                                // [TIMING] t4: B收到A解析结果
        // 在 normalize 之前提取 A端处理耗时字段（normalize 可能丢弃未知字段）
        const aMs: number = (message.data as any)?._aMs ?? -1;
        const normalized = this.normalizePidEvent(message.data);
        if (normalized) {
          const idx = normalized.originalIndex ?? normalized.index;
          const val = normalized.Value;
          console.log(`[CloudBridge] PIDValueChanged idx=${idx} NM=${normalized.NM} val=${typeof val === 'object' ? '[object]' : val}`);
          this.flowLog('B<A', `PID值变化: ${normalized.NM}=${typeof val === 'object' ? '[obj]' : val}`);
          this.onPIDValueChanged?.(normalized);
          const timingT5 = Date.now();

          // 虚拟 PID 过滤：Current time 由 CarScanner 内部计算，没有对应的 AT 命令，
          // 不消费 _pidTimingPending，避免错位污染其他真实 PID 的计时
          const isVirtualPid = normalized.NM === 'Current time';
          if (isVirtualPid) break;

          // 输出 [TIMING] 日志
          const entry = this._pidTimingPending;
          if (entry && entry.tElmReply > 0) {
            this._pidTimingPending = null;
            const name = normalized.NM ?? '?';
            const total = timingT5 - entry.tSend;
            const btRtt = entry.tElmReply - entry.tSend;
            const aWs = aMs >= 0 ? aMs : (timingT4 - entry.tForwarded);
            const lastT5 = this._lastPidT5.get(name);
            const gap = lastT5 !== undefined ? (timingT5 - lastT5) : -1;
            const d = new Date(entry.tSend);
            const ts = `${String(d.getHours()).padStart(2,'0')}:${String(d.getMinutes()).padStart(2,'0')}:${String(d.getSeconds()).padStart(2,'0')}.${String(d.getMilliseconds()).padStart(3,'0')}`;
            const gapStr = gap >= 0 ? `  距上次: ${gap}ms` : '';
            console.log(
              `[TIMING] ${name}  ${entry.cmdText}  ${ts}  总耗时: ${total}ms${gapStr}\n` +
              `  +0ms    A发AT命令到达B\n` +
              `  +${entry.tBtWrite - entry.tSend}ms    B写入ELM327完成\n` +
              `  +${entry.tElmReply - entry.tSend}ms   ELM327回复数据              BT往返: ${btRtt}ms\n` +
              `  +${entry.tForwarded - entry.tSend}ms   B转发数据给A\n` +
              `  +${timingT4 - entry.tSend}ms  B收到A解析结果              A处理+WS: ${aWs}ms\n` +
              `  +${timingT5 - entry.tSend}ms  UI数字更新`
            );
            this._lastPidT5.set(name, timingT5);
          }
        }
        break;
      }

      case MessageAction.DTCResult: {
        const callbacks = this.callbacks.get('dtc');
        let parsed: any = this.parseResult(message.data, message.data as any);
        // 若解析结果仍为字符串（A 端 JSON.stringify 双重编码），再解析一次
        if (typeof parsed === 'string') {
          console.log(`[CloudBridge] DTCResult raw string (first 120): ${parsed.substring(0, 120)}`);
          try { parsed = JSON.parse(parsed); } catch {}
        }
        console.log(`[CloudBridge] DTCResult received (${Array.isArray(parsed) ? 'array' : typeof parsed})`);
        this.flowLog('B<A', `DTC结果收到`, `count=${Array.isArray(parsed) ? parsed.length : typeof parsed}`);
        const indices = this.pendingMeta.get('dtc')?.indices as number[] | undefined;
        if (callbacks?.onProgress && Array.isArray(parsed)) {
          if (Array.isArray(parsed[0])) {
            parsed.forEach((list: any[], idx: number) => {
              const ecuIndex = indices?.[idx] ?? idx;
              callbacks.onProgress(ecuIndex, this.normalizeDtcList(list));
            });
          } else {
            const normalized = this.normalizeDtcList(parsed);
            const targets = indices && indices.length > 0 ? indices : [0];
            targets.forEach((ecuIndex: number, idx: number) => {
              callbacks.onProgress(ecuIndex, idx === 0 ? normalized : []);
            });
          }
          callbacks.onFinish?.();
        } else if (callbacks?.onProgress) {
          // 非数组情况（字符串等）：通过 normalizeDtcList 尝试解析，路由到 onProgress
          const normalized = this.normalizeDtcList(parsed);
          const targets = indices && indices.length > 0 ? indices : [0];
          targets.forEach((ecuIndex: number, idx: number) => {
            callbacks.onProgress(ecuIndex, idx === 0 ? normalized : []);
          });
          callbacks.onFinish?.();
        } else {
          callbacks?.onSuccess?.(this.normalizeDtcList(parsed));
          callbacks?.onFinish?.();
        }
        this.pendingMeta.delete('dtc');
        break;
      }

      case MessageAction.DTCCleared:
        this.triggerCallbacks('clearDtc', 'onSuccess', message.data);
        this.triggerCallbacks('clearDtc', 'onFinish');
        this.pendingMeta.delete('clearDtc');
        break;

      case MessageAction.ECUInfoResult: {
        const callbacks = this.callbacks.get('ecuInfo');
        // 诊断日志：打印原始 message.data 类型和前200字符
        const rawDataType = typeof message.data;
        const rawDataPreview = typeof message.data === 'string'
          ? message.data.substring(0, 200)
          : Array.isArray(message.data)
            ? `Array[${(message.data as any[]).length}] first=${JSON.stringify((message.data as any[])[0])?.substring(0, 100)}`
            : JSON.stringify(message.data)?.substring(0, 200);
        console.log(`[ECUInfo-RAW] type=${rawDataType} data=${rawDataPreview}`);

        let parsed: any = this.parseResult(message.data, message.data as any);
        // 若解析结果仍为字符串，尝试多种方式解析（A端数据含 \" 转义 + 裸换行符）
        if (typeof parsed === 'string') {
          // 尝试1：直接 JSON.parse
          try { parsed = JSON.parse(parsed); } catch {}
          if (typeof parsed === 'string') {
            // 尝试2：替换 \" → "，同时把裸 CR/LF/Tab 转为 JSON 合法转义序列
            try {
              const cleaned = parsed
                .replace(/\\"/g, '"')        // \" → "
                .replace(/\r\n/g, '\\r\\n')  // 真实 CR+LF → JSON 转义
                .replace(/\r/g, '\\r')       // 真实 CR → JSON 转义
                .replace(/\n/g, '\\n')       // 真实 LF → JSON 转义
                .replace(/\t/g, '\\t');      // 真实 Tab → JSON 转义
              parsed = JSON.parse(cleaned);
            } catch {}
          }
          // 尝试3：字符级扫描，专门处理 A端真实格式 [\n \"str1\",\n \"str2\"]
          // A端数据中字符串用 \" (反斜杠+引号) 作为边界，非标准JSON，导致上述所有解析失败
          if (typeof parsed === 'string') {
            try {
              const s = parsed;
              const items: string[] = [];
              let ci = 0;
              while (ci < s.length) {
                // 找开始的 \" (反斜杠+引号)
                if (s.charAt(ci) === '\\' && ci + 1 < s.length && s.charAt(ci + 1) === '"') {
                  ci += 2; // 跳过开始的 \"
                  let seg = '';
                  while (ci < s.length) {
                    // 找结束的 \" (反斜杠+引号)
                    if (s.charAt(ci) === '\\' && ci + 1 < s.length && s.charAt(ci + 1) === '"') {
                      ci += 2; // 跳过结束的 \"
                      break;
                    }
                    // 其他转义序列：原样保留，交给 cleanStr / normalizeEcuInfoList 处理
                    if (s.charAt(ci) === '\\' && ci + 1 < s.length) {
                      seg += s.charAt(ci);
                      seg += s.charAt(ci + 1);
                      ci += 2;
                    } else {
                      seg += s.charAt(ci);
                      ci++;
                    }
                  }
                  items.push(seg);
                } else {
                  ci++;
                }
              }
              if (items.length >= 2) {
                parsed = items;
              }
            } catch (_e3) { /* ignore */ }
          }
        }
        console.log(`[ECUInfo] parsed type=${Array.isArray(parsed) ? `array(${parsed.length})` : typeof parsed}`);
        this.flowLog('B<A', 'ECU信息结果收到');

        // 构建分组数据：[{ title: ECU名, items: [{key,value},...] }, ...]
        // A 端格式：交替数组 [ECU名0, ECU数据文本0, ECU名1, ECU数据文本1, ...]
        const cleanStr = (s: string) => s
          .replace(/\u0000/g, '').replace(/\\u0000/g, '')  // 去空字节
          .replace(/\\r\\n|\\r|\\n/g, '')                   // 去字面量转义换行
          .replace(/[\r\n]/g, '')                            // 去实际换行
          .replace(/\\/g, '')                                // 去残留反斜杠
          .trim();

        const sections: { title: string; data: { key: string; value: string }[] }[] = [];

        if (Array.isArray(parsed) && parsed.length >= 2) {
          const pairCount = Math.floor(parsed.length / 2);
          for (let pairIdx = 0; pairIdx < pairCount; pairIdx++) {
            const ecuName = cleanStr((parsed[pairIdx * 2] ?? '').toString());
            const infoText = (parsed[pairIdx * 2 + 1] ?? '').toString();
            const infoItems = this.normalizeEcuInfoList(infoText);
            // 如果数据第一行与 ECU 名重复，跳过它
            const filtered = infoItems.filter((item, idx) =>
              !(idx === 0 && !item.value && cleanStr(item.key) === ecuName)
            );
            sections.push({ title: ecuName, data: filtered });
          }
        } else {
          // 降级：整体作为一个无名分组
          const infoItems = this.normalizeEcuInfoList(parsed);
          sections.push({ title: '', data: infoItems });
        }

        // 诊断：输出每个 section 的 title 和前3项数据
        sections.forEach((sec, i) => {
          const preview = sec.data.slice(0, 3).map(d => `${d.key}|${d.value}`).join(' || ');
          console.log(`[ECUInfo-SECTIONS] [${i}] title="${sec.title}" items=${sec.data.length} preview: ${preview}`);
        });

        callbacks?.onSuccess?.(sections);
        callbacks?.onFinish?.();
        this.pendingMeta.delete('ecuInfo');
        break;
      }

      case MessageAction.FreezeFrameResult: {
        let parsed: any = this.parseResult(message.data, message.data);
        // 若解析结果仍为字符串（A 端 JSON 双重编码），再解析一次
        if (typeof parsed === 'string') {
          console.log(`[CloudBridge] FreezeFrameResult raw string (first 120): ${parsed.substring(0, 120)}`);
          try { parsed = JSON.parse(parsed); } catch {}
        }
        console.log(`[CloudBridge] FreezeFrameResult received (${Array.isArray(parsed) ? 'array' : typeof parsed})`);
        this.flowLog('B<A', '冻结帧结果收到');
        this.triggerCallbacks('freezeFrame', 'onSuccess', this.normalizeFreezeFrame(parsed));
        this.triggerCallbacks('freezeFrame', 'onFinish');
        this.pendingMeta.delete('freezeFrame');
        break;
      }

      case MessageAction.Error: {
        const errorCode = message.data?.code || message.data?.Code || '';
        const errorMsg = typeof message.data === 'string'
          ? message.data
          : (message.data?.message || message.data?.Message || 'Unknown error');
        console.warn(`[CloudBridge] Error event code=${errorCode} msg=${errorMsg}`);
        const target = this.resolveErrorTarget(errorCode);
        if (target) {
          this.triggerCallbacks(target, 'onError', errorMsg);
          this.triggerCallbacks(target, 'onFinish');
          this.pendingMeta.delete(target);
        } else {
          this.triggerCallbacks('dtc', 'onError', errorMsg);
          this.triggerCallbacks('clearDtc', 'onError', errorMsg);
          this.triggerCallbacks('ecuInfo', 'onError', errorMsg);
          this.triggerCallbacks('freezeFrame', 'onError', errorMsg);
          this.triggerCallbacks('dtc', 'onFinish');
          this.triggerCallbacks('clearDtc', 'onFinish');
          this.triggerCallbacks('ecuInfo', 'onFinish');
          this.triggerCallbacks('freezeFrame', 'onFinish');
          this.pendingMeta.delete('dtc');
          this.pendingMeta.delete('clearDtc');
          this.pendingMeta.delete('ecuInfo');
          this.pendingMeta.delete('freezeFrame');
        }
        break;
      }

      default:
        if (message.action === MessageAction.BurstReplayCompleted) {
          console.log('[CloudBridge] BurstReplayCompleted event received');
          this.flowLog('B<A', 'Burst回放完成');
          this.handleReplayCompletedEvent(message.data);
          this.onBurstReplayCompleted?.(message.data);
          break;
        }
        if (message.action === MessageAction.BurstReplayFailed) {
          console.warn('[CloudBridge] BurstReplayFailed event received', message.data);
          this.handleReplayFailedEvent(message.data);
          this.onBurstReplayFailed?.(message.data);
          break;
        }
        console.log('[CloudBridge] Unhandled event:', message.action);
    }
  }

  private handleReplayCompletedEvent(data: any): void {
    const sessionId = data?.sessionId;
    if (!sessionId) return;

    const waiter = this.burstReplayWaiters.get(sessionId);
    if (waiter) {
      this.clearReplayWaiter(sessionId, 'completed');
      waiter.resolve(this.normalizeReplayCompletedData(data));
      console.log(`[CloudBridge] BurstReplayCompleted resolve waiter sessionId=${sessionId}`);
      return;
    }

    if (this.consumeIgnoredReplaySession(sessionId)) {
      console.log(`[CloudBridge] BurstReplayCompleted drop ignored sessionId=${sessionId}`);
      this.burstReplayTerminalEvents.delete(sessionId);
      return;
    }

    this.burstReplayTerminalEvents.set(sessionId, { kind: 'completed', data });
    console.log(`[CloudBridge] BurstReplayCompleted cached sessionId=${sessionId}`);
  }

  private handleReplayFailedEvent(data: any): void {
    const sessionId = data?.sessionId;
    if (!sessionId) return;

    const waiter = this.burstReplayWaiters.get(sessionId);
    if (waiter) {
      this.clearReplayWaiter(sessionId, 'failed');
      const err = new Error(data?.reason || 'Burst Replay 失败') as any;
      err.errorCode = 'BURST_REPLAY_FAILED';
      err.responseData = data;
      waiter.reject(err);
      console.warn(`[CloudBridge] BurstReplayFailed reject waiter sessionId=${sessionId}`);
      return;
    }

    if (this.consumeIgnoredReplaySession(sessionId)) {
      console.warn(`[CloudBridge] BurstReplayFailed drop ignored sessionId=${sessionId}`);
      this.burstReplayTerminalEvents.delete(sessionId);
      return;
    }

    this.burstReplayTerminalEvents.set(sessionId, { kind: 'failed', data });
    console.warn(`[CloudBridge] BurstReplayFailed cached sessionId=${sessionId}`);
  }

  private normalizeReplayCompletedData(data: any): { results: any[]; totalCycles: number } {
    const results = Array.isArray(data?.results) ? data.results : [];
    const totalCycles = typeof data?.totalCycles === 'number' ? data.totalCycles : 0;
    return { results, totalCycles };
  }

  private clearReplayWaiter(sessionId: string, reason: string = 'unknown'): void {
    const waiter = this.burstReplayWaiters.get(sessionId);
    if (!waiter) return;

    clearTimeout(waiter.timeout);
    if (waiter.abortPoll) clearInterval(waiter.abortPoll);
    this.burstReplayWaiters.delete(sessionId);
    console.log(`[CloudBridge] clearReplayWaiter sessionId=${sessionId} reason=${reason}`);
  }

  private clearIgnoredReplaySession(sessionId: string): void {
    const cleanup = this.ignoredReplayTerminalSessions.get(sessionId);
    if (!cleanup) return;

    clearTimeout(cleanup);
    this.ignoredReplayTerminalSessions.delete(sessionId);
    console.log(`[CloudBridge] clearIgnoredReplaySession sessionId=${sessionId}`);
  }

  private markReplayResultIgnored(sessionId: string, reason: string): void {
    if (!sessionId) return;

    this.clearIgnoredReplaySession(sessionId);
    this.burstReplayTerminalEvents.delete(sessionId);
    const cleanup = setTimeout(() => {
      this.ignoredReplayTerminalSessions.delete(sessionId);
      console.log(`[CloudBridge] ignored replay session TTL expired sessionId=${sessionId}`);
    }, 10 * 60 * 1000);
    this.ignoredReplayTerminalSessions.set(sessionId, cleanup);
    console.log(`[CloudBridge] markReplayResultIgnored sessionId=${sessionId} reason=${reason}`);
  }

  private consumeIgnoredReplaySession(sessionId: string): boolean {
    const cleanup = this.ignoredReplayTerminalSessions.get(sessionId);
    if (!cleanup) return false;

    clearTimeout(cleanup);
    this.ignoredReplayTerminalSessions.delete(sessionId);
    return true;
  }

  private rejectAllReplayWaiters(reason: string): void {
    const pending = Array.from(this.burstReplayWaiters.entries());
    if (pending.length > 0) {
      console.warn(`[CloudBridge] rejectAllReplayWaiters count=${pending.length} reason=${reason}`);
    }

    for (const [sessionId, waiter] of pending) {
      clearTimeout(waiter.timeout);
      if (waiter.abortPoll) clearInterval(waiter.abortPoll);
      try {
        waiter.reject(new Error(reason));
      } catch {}
      this.burstReplayWaiters.delete(sessionId);
      this.markReplayResultIgnored(sessionId, `rejectAll:${reason}`);
      console.warn(`[CloudBridge] rejectAllReplayWaiters rejected sessionId=${sessionId}`);
    }
  }

  private emitConnectionChanged(connected: boolean): void {
    try {
      this.onConnectionChanged?.(connected);
    } catch (e) {
      console.error('[CloudBridge] onConnectionChanged error:', e);
    }

    this.connectionListeners.forEach((listener) => {
      try {
        listener(connected);
      } catch (e) {
        console.error('[CloudBridge] connection listener error:', e);
      }
    });
  }

  private resetTransportScopedOBDState(reason: string): void {
    const prevStatus = this.currentOBDStatus || '(empty)';
    this.currentSessionId = null;
    this.allowRemoteDisconnectUntil = 0;
    this.ignoreDisconnectedUntil = 0;

    if (this.currentOBDStatus !== 'Disconnected') {
      console.log(`[CloudBridge] transport closed → 强制回收 OBD 状态 prev=${prevStatus} reason=${reason}`);
      this.currentOBDStatus = 'Disconnected';
      this.emitOBDStatusChanged('Disconnected');
      return;
    }

    this.currentOBDStatus = 'Disconnected';
    console.log(`[CloudBridge] transport closed → OBD 状态已是 Disconnected reason=${reason}`);
  }

  private emitOBDStatusChanged(status: string): void {
    try {
      this.onOBDStatusChanged?.(status);
    } catch (e) {
      console.error('[CloudBridge] onOBDStatusChanged error:', e);
    }

    this.obdStatusListeners.forEach((listener) => {
      try {
        listener(status);
      } catch (e) {
        console.error('[CloudBridge] OBD status listener error:', e);
      }
    });
  }

  private normalizeDtcList(raw: any): any[] {
    if (raw === null || raw === undefined) return [];

    if (typeof raw === 'string') {
      const parsed = this.parseResult(raw, raw);
      if (parsed !== raw) return this.normalizeDtcList(parsed);
      return [];
    }

    if (!Array.isArray(raw)) return [];

    return raw.map((item: any) => {
      if (!item || typeof item !== 'object') {
        return { code: String(item ?? ''), description: '', status: '' };
      }

      const code = item.code ?? item.Code ?? item.RawCode ?? item.rawCode ?? '';
      let description = item.description ?? item.Description ?? '';
      if (!description && Array.isArray(item.Descriptions) && item.Descriptions.length > 0) {
        const first = item.Descriptions[0];
        description = first?.Description ?? first?.description ?? '';
      }

      let status = item.status ?? item.Status ?? '';
      if (!status && Array.isArray(item.Statuses) && item.Statuses.length > 0) {
        status = item.Statuses.map((s: any) => String(s)).join(', ');
      }

      return {
        code: code ? String(code) : '',
        description: description ? String(description) : '',
        status: status ? String(status) : undefined,
      };
    });
  }

  private normalizeEcuInfoList(raw: any): { key: string; value: string }[] {
    if (raw === null || raw === undefined) return [];

    if (typeof raw === 'string') {
      // 先尝试 JSON 解析（可能是双重编码字符串）
      const jsonParsed = this.parseResult(raw, raw);
      if (jsonParsed !== raw) return this.normalizeEcuInfoList(jsonParsed);

      // 清理各种形式的无效字符，并统一换行符
      const text = raw
        .replace(/\u0000/g, '')        // 真实 null 字节
        .replace(/\\u0000/g, '')       // 字面量 \u0000（6字符）
        .replace(/\\r\\n/g, '\n')      // 字面量 \r\n → 真实换行
        .replace(/\\n/g, '\n')         // 字面量 \n → 真实换行
        .replace(/\\r/g, '')           // 字面量 \r → 删除
        .replace(/\r\n/g, '\n')        // 真实 CR+LF → LF
        .replace(/\r/g, '\n')          // 真实 CR → LF
        .replace(/\\t/g, '\t');        // 字面量 \t → 真实 Tab

      return text
        .split('\n')
        .map((line: string) => line.trim())
        .filter(Boolean)
        .map((line: string) => {
          // 优先用 Tab 分隔（ECU 数据的标准格式），再尝试冒号
          const tabIdx = line.indexOf('\t');
          if (tabIdx > -1) {
            return {
              key: line.substring(0, tabIdx).replace(/\\/g, '').trim(),
              value: line.substring(tabIdx + 1).replace(/\\/g, '').trim(),
            };
          }
          const colonIdx = line.indexOf(':');
          if (colonIdx > -1) {
            return {
              key: line.substring(0, colonIdx).replace(/\\/g, '').trim(),
              value: line.substring(colonIdx + 1).replace(/\\/g, '').trim(),
            };
          }
          // 无分隔符的行：整行作为标题/描述文本，去掉残留反斜杠
          return { key: line.replace(/\\/g, '').trim(), value: '' };
        });
    }

    if (Array.isArray(raw)) {
      if (raw.length === 0) return [];
      const first = raw[0];
      if (first && typeof first === 'object' && ('key' in first || 'Key' in first || 'value' in first || 'Value' in first)) {
        return raw.map(item => ({
          key: String(item?.key ?? item?.Key ?? ''),
          value: String(item?.value ?? item?.Value ?? ''),
        }));
      }

      return raw.map(item => {
        if (typeof item === 'string') {
          const parts = item.split(':');
          if (parts.length > 1) {
            return { key: parts[0].trim(), value: parts.slice(1).join(':').trim() };
          }
          return { key: item, value: '' };
        }
        return { key: String(item ?? ''), value: '' };
      });
    }

    if (typeof raw === 'object') {
      if ('key' in raw || 'Key' in raw || 'value' in raw || 'Value' in raw) {
        return [{
          key: String((raw as any).key ?? (raw as any).Key ?? ''),
          value: String((raw as any).value ?? (raw as any).Value ?? ''),
        }];
      }
    }

    return [];
  }

  private normalizeFreezeFrame(raw: any): any[] {
    if (raw === null || raw === undefined) return [];

    if (Array.isArray(raw)) {
      return raw.map(item => {
        if (item && typeof item === 'object') {
          // 兼容C#端PascalCase字段名（Name/Value/Units）和JS端camelCase（name/value/unit）
          return {
            pid: item.pid ?? item.PID ?? item.Pid ?? '',
            name: item.name ?? item.Name ?? '',
            value: String(item.value ?? item.Value ?? ''),
            unit: item.unit ?? item.Unit ?? item.Units ?? item.units ?? 0,
          };
        }
        if (typeof item === 'string') {
          const parts = item.split(':');
          const name = parts[0]?.trim() || '';
          const value = parts.slice(1).join(':').trim();
          return { pid: name, name, value, unit: 0 };
        }
        return { pid: '', name: '', value: String(item ?? ''), unit: 0 };
      });
    }

    if (typeof raw === 'string') {
      // 先尝试直接 JSON 解析
      try {
        const p = JSON.parse(raw);
        if (Array.isArray(p) || (p && typeof p === 'object')) {
          return this.normalizeFreezeFrame(p);
        }
      } catch {}

      // 尝试去除 CarScanner 的 \" 字面量转义后解析（A 端 WebView JSBridge 会产生此格式）
      try {
        const unescaped = raw.replace(/\\"/g, '"');
        const p = JSON.parse(unescaped);
        if (Array.isArray(p) || (p && typeof p === 'object')) {
          return this.normalizeFreezeFrame(p);
        }
      } catch {}

      // 降级：按行分割处理
      const lines = raw
        .split(/\r?\n/)
        .map((line: string) => line.trim())
        .filter(Boolean);

      if (lines.length === 1 && /no freeze frame/i.test(lines[0])) {
        return [];
      }

      return lines.map((line: string) => {
        const parts = line.split(':');
        const name = parts[0]?.trim() || '';
        const value = parts.slice(1).join(':').trim();
        return { pid: name, name, value, unit: 0 };
      });
    }

    return [];
  }

  private normalizePidEvent(data: any): PIDItem | null {
    if (!data || typeof data !== 'object') return null;

    // A 端发来的是完整的 PIDItem 对象（含 NM, Value, Units 等）
    // 返回完整对象而不是只提取 index 和 value
    return {
      ...data,
      NM: data.NM ?? data.name ?? data.Name,
      SNM: data.SNM ?? data.shortName ?? data.ShortName,
      Value: data.Value ?? data.value ?? data.val,
      Units: data.Units ?? data.unit ?? data.Unit,
      originalIndex: data.originalIndex ?? data.OriginalIndex ?? data.index ?? data.Index,
    } as PIDItem;
  }

  private normalizePidList(raw: any): PIDItem[] {
    if (!Array.isArray(raw)) return [];

    this.pidIndexMap.clear();

    return raw.map((item: any, index: number) => {
      const pid = item?.pid ?? item?.PID ?? item?.Id ?? item?.ID ?? item?.Cmd ?? item?.CMD ?? item?.NM ?? `PID_${index}`;
      const name = item?.name ?? item?.NM ?? item?.Name ?? item?.SNM ?? pid;
      let unit: any = item?.unit ?? item?.Unit ?? item?.Units ?? 0;

      if (typeof unit === 'string') {
        const parsed = parseInt(unit, 10);
        unit = Number.isFinite(parsed) ? parsed : 0;
      }

      const mapKey = item?.pid ?? item?.PID ?? item?.Id ?? item?.ID ?? item?.Cmd ?? item?.CMD ?? item?.NM ?? item?.Name ?? pid;
      if (mapKey !== undefined && mapKey !== null) {
        this.pidIndexMap.set(String(mapKey), index);
      }

      return {
        ...item,
        pid: String(pid),
        name: String(name),
        unit: typeof unit === 'number' ? unit : 0,
        originalIndex: index,  // 保留 A 端原始 array index
      } as PIDItem;
    });
  }

  private resolveErrorTarget(code: string): string | null {
    if (!code) return null;
    const upper = String(code).toUpperCase();
    if (upper.includes('CLEAR_DTC')) return 'clearDtc';
    if (upper.includes('DTC')) return 'dtc';
    if (upper.includes('ECU_INFO')) return 'ecuInfo';
    if (upper.includes('FREEZE_FRAME')) return 'freezeFrame';
    return null;
  }

  // 回调存储
  private callbacks: Map<string, any> = new Map();
  private pendingMeta: Map<string, any> = new Map();
  private pidIndexMap: Map<string, number> = new Map();

  /**
   * 注册回调
   */
  private registerCallbacks(key: string, callbacks: any): void {
    this.callbacks.set(key, callbacks);
  }

  /**
   * 触发回调
   */
  private triggerCallbacks(key: string, method: string, data?: any): void {
    const callbacks = this.callbacks.get(key);
    if (callbacks && callbacks[method]) {
      callbacks[method](data);
    }
  }

  /**
   * 清除回调
   */
  clearCallbacks(key: string): void {
    this.callbacks.delete(key);
    this.pendingMeta.delete(key);
  }

  /**
   * 生成唯一 ID
   */
  private generateId(): string {
    return Math.random().toString(36).substring(2, 10);
  }

  /**
   * 解析结果（处理字符串 JSON）
   */
  private parseResult<T>(data: any, defaultValue: T): T {
    if (data === null || data === undefined) {
      return defaultValue;
    }
    if (typeof data === 'string') {
      try {
        return JSON.parse(data) as T;
      } catch {
        return defaultValue;
      }
    }
    return data as T;
  }

  /**
   * 拒绝所有待处理请求
   */
  private rejectAllPending(reason: string): void {
    for (const [requestId, pending] of this.pendingRequests) {
      clearTimeout(pending.timeout);
      pending.reject(new Error(reason));
    }
    this.pendingRequests.clear();
  }
}

// 导出单例
export const CloudBridge = new CloudBridgeService();
export default CloudBridge;
