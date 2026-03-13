/**
 * CloudBridge - 云端通信服务
 *
 * 用于与云端 A (运行 CarScanner 业务逻辑的 Android 模拟器) 通信
 * 替代原 JSBridge，通过 WebSocket 发送诊断命令并接收结果
 */

import { MessageType, MessageAction, WSMessage } from '../protocol/MessageProtocol';

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
  selected?: boolean;
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

// 事件监听器类型
type EventListener = (data: any) => void;

/**
 * CloudBridge 类
 */
class CloudBridgeService {
  private ws: WebSocket | null = null;
  private pendingRequests: Map<string, PendingRequest> = new Map();
  private eventListeners: Map<string, EventListener[]> = new Map();
  private isConnected: boolean = false;
  private reconnectAttempts: number = 0;
  private maxReconnectAttempts: number = 5;
  private requestTimeout: number = 30000; // 30 seconds
  private connectionListeners: Set<(connected: boolean) => void> = new Set();
  private obdStatusListeners: Set<(status: string) => void> = new Set();
  // 仅允许在“用户主动断开”窗口内响应 A 端的断开请求，避免状态抖动导致误断开
  private allowRemoteDisconnectUntil: number = 0;

  // 当前蓝牙会话ID
  private currentSessionId: string | null = null;

  // 当前 OBD 状态（用于过滤诊断操作触发的中间态）
  private currentOBDStatus: string = '';

  // 诊断日志：时序与序号
  private btSeq = 0;
  private ts(): string {
    const d = new Date();
    return `[${String(d.getHours()).padStart(2,'0')}:${String(d.getMinutes()).padStart(2,'0')}:${String(d.getSeconds()).padStart(2,'0')}.${String(d.getMilliseconds()).padStart(3,'0')}]`;
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
          this.emitConnectionChanged(false);
          this.rejectAllPending('Connection closed');
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
    this.rejectAllPending('Disconnected');
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

  // ===== ELM327 连接命令 =====

  /**
   * 连接 ELM327 设备（通过云端）
   */
  async connectOBD(protocol: string, address: string, sessionId?: string): Promise<void> {
    await this.sendRequest(MessageAction.OBDConnect, { protocol, address, sessionId });
  }

  /**
   * 断开 ELM327 设备
   */
  async disconnectOBD(): Promise<void> {
    // 标记“允许远端断开”的短窗口（用户主动断开）
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
    this.sendMessage(message);
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
  private sendRequest(action: string, data?: any): Promise<any> {
    return new Promise((resolve, reject) => {
      if (!this.isConnected || !this.ws) {
        reject(new Error('Not connected to cloud'));
        return;
      }

      const requestId = this.generateId();
      const timeout = setTimeout(() => {
        this.pendingRequests.delete(requestId);
        console.warn(`[CloudBridge] TIMEOUT action=${action} req=${requestId} after ${this.requestTimeout}ms`);
        reject(new Error('Request timeout'));
      }, this.requestTimeout);

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
            pending.reject(new Error(message.error || 'Request failed'));
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
          await this.handleBluetoothDisconnect(message);
          this.sendResponse(requestId, message.action, true);
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
    const t0 = Date.now();
    console.log(`[CloudBridge] ${this.ts()} ▶▶ BT#${seq} A→B→ELM327: [${preview}] (b64len=${base64Data?.length || 0})`);
    await this.onBluetoothSendRequest(base64Data);
    console.log(`[CloudBridge] ${this.ts()} ✓ BT#${seq} 转发完成 (耗时=${Date.now()-t0}ms)`);
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
  private async handleBluetoothDisconnect(message: WSMessage): Promise<void> {
    if (!this.onBluetoothDisconnectRequest) {
      throw new Error('Bluetooth bridge not initialized');
    }

    const now = Date.now();
    if (now > this.allowRemoteDisconnectUntil) {
      console.warn('[CloudBridge] 忽略远端断开请求：未在允许窗口内');
      return;
    }

    console.log('[CloudBridge] Disconnecting Bluetooth (allowed)');
    this.allowRemoteDisconnectUntil = 0;
    await this.onBluetoothDisconnectRequest();
    this.currentSessionId = null;
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

        // 过滤延迟到达的 Disconnecting（已经 Disconnected 后不再接受 Disconnecting）
        if (this.currentOBDStatus === 'Disconnected' && newStatus === 'Disconnecting') {
          console.log(`[CloudBridge] 过滤延迟的 Disconnecting（当前已 Disconnected）`);
          break;
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
        const normalized = this.normalizePidEvent(message.data);
        if (normalized) {
          const idx = normalized.originalIndex ?? normalized.index;
          const val = normalized.Value;
          console.log(`[CloudBridge] PIDValueChanged idx=${idx} NM=${normalized.NM} val=${typeof val === 'object' ? '[object]' : val}`);
          this.onPIDValueChanged?.(normalized);
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
        const parsed = this.parseResult(message.data, message.data as any);
        console.log(`[CloudBridge] ECUInfoResult received (${Array.isArray(parsed) ? 'array' : typeof parsed})`);
        const indices = this.pendingMeta.get('ecuInfo')?.indices as number[] | undefined;
        if (callbacks?.onProgress) {
          if (Array.isArray(parsed) && Array.isArray(parsed[0])) {
            parsed.forEach((item: any[], idx: number) => {
              const ecuIndex = indices?.[idx] ?? idx;
              callbacks.onProgress(ecuIndex, this.normalizeEcuInfoList(item));
            });
          } else {
            const normalized = this.normalizeEcuInfoList(parsed);
            const targets = indices && indices.length > 0 ? indices : [0];
            targets.forEach((ecuIndex: number, idx: number) => {
              callbacks.onProgress(ecuIndex, idx === 0 ? normalized : []);
            });
          }
          callbacks.onFinish?.();
        } else {
          callbacks?.onSuccess?.(this.normalizeEcuInfoList(parsed));
          callbacks?.onFinish?.();
        }
        this.pendingMeta.delete('ecuInfo');
        break;
      }

      case MessageAction.FreezeFrameResult: {
        const parsed = this.parseResult(message.data, message.data);
        console.log(`[CloudBridge] FreezeFrameResult received (${Array.isArray(parsed) ? 'array' : typeof parsed})`);
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
        console.log('[CloudBridge] Unhandled event:', message.action);
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
      const parsed = this.parseResult(raw, raw);
      if (parsed !== raw) return this.normalizeEcuInfoList(parsed);
      return raw
        .split(/\r?\n/)
        .map(line => line.trim())
        .filter(Boolean)
        .map(line => {
          const parts = line.split(':');
          if (parts.length > 1) {
            return {
              key: parts[0].trim(),
              value: parts.slice(1).join(':').trim(),
            };
          }
          return { key: line, value: '' };
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
      const lines = raw
        .split(/\r?\n/)
        .map(line => line.trim())
        .filter(Boolean);

      if (lines.length === 1 && /no freeze frame/i.test(lines[0])) {
        return [];
      }

      return lines.map(line => {
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
