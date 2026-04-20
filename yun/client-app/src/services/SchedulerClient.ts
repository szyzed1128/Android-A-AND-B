/**
 * SchedulerClient - 调度后端 HTTP 调用封装
 * B端与调度后端通信的统一入口
 *
 * 调度后端地址通过 setBaseUrl() 动态设置，由 useScheduler 从 cloudHost 推导：
 *   cloudHost = "47.110.246.8" → schedulerUrl = "http://47.110.246.8:8080/api"
 * 调度后端通过 Nginx /api/ 路径代理，与 APK WebSocket /ws 共用 8080 端口。
 */

// 默认无地址，必须在 useScheduler 初始化时通过 setBaseUrl() 设置
const SCHEDULER_BASE_URL = '';
const LOG_BODY_PREVIEW_MAX = 300;

export interface SchedulerCatalogProfile {
  profileIndex: number;
  name: string;
  description?: string;
}

export interface SchedulerSessionState {
  sessionId: string;
  status: 'init' | 'reserved' | 'running' | 'disconnected';
  carBrand?: string;
  carModel?: string;
  profileIndex?: number;
  profileName?: string;
  btAddress?: string;
  btName?: string;
  btProtocol?: 'ble' | 'classic' | 'mfi';
  instanceId?: string;
}

class SchedulerClientService {
  private baseUrl: string = SCHEDULER_BASE_URL;
  private sessionId: string | null = null;

  private buildUrl(path: string): string {
    if (!this.baseUrl) {
      throw new Error('调度后端地址为空');
    }
    return `${this.baseUrl}${path}`;
  }

  private async fetchJson<T>(
    method: 'GET' | 'POST' | 'PUT',
    path: string,
    body?: any,
    timeoutMs: number = 8000
  ): Promise<{ status: number; payload: T }> {
    const url = this.buildUrl(path);
    const startedAt = Date.now();
    const bodyText = body === undefined ? undefined : JSON.stringify(body);

    try {
      const options: RequestInit = { method };
      if (bodyText !== undefined) {
        options.headers = {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
        };
        options.body = bodyText;
      }

      console.log('[SchedulerClient] 请求开始', JSON.stringify({
        method,
        path,
        url,
        timeoutMs,
        sessionId: this.sessionId,
        hasBody: bodyText !== undefined,
        bodyPreview: bodyText ? bodyText.slice(0, LOG_BODY_PREVIEW_MAX) : '',
      }));

      const res = await Promise.race([
        fetch(url, options),
        new Promise<Response>((_, reject) => {
          setTimeout(() => reject(new Error(`调度请求超时（${timeoutMs}ms）: ${path}`)), timeoutMs);
        }),
      ]);
      const text = await res.text();
      console.log('[SchedulerClient] 收到响应', JSON.stringify({
        method,
        path,
        url,
        status: res.status,
        elapsedMs: Date.now() - startedAt,
        bodyPreview: text.slice(0, LOG_BODY_PREVIEW_MAX),
      }));
      let payload: T;
      try {
        payload = text ? JSON.parse(text) as T : ({} as T);
      } catch {
        throw new Error(`调度响应不是 JSON（HTTP ${res.status}）`);
      }

      return { status: res.status, payload };
    } catch (err: any) {
      console.error('[SchedulerClient] 请求失败', JSON.stringify({
        method,
        path,
        url,
        elapsedMs: Date.now() - startedAt,
        error: this.describeError(err),
      }));
      if (err instanceof Error) {
        throw err;
      }
      throw new Error(String(err));
    }
  }

  private describeError(err: any) {
    if (!err) {
      return { raw: 'unknown error' };
    }

    const result: Record<string, any> = {
      type: typeof err,
      constructor: err?.constructor?.name,
      name: err?.name,
      message: err?.message,
      code: err?.code,
      domain: err?.domain,
      stack: err?.stack,
    };

    for (const key of Object.getOwnPropertyNames(err)) {
      if (!(key in result)) {
        result[key] = err[key];
      }
    }

    return result;
  }

  setBaseUrl(url: string) {
    this.baseUrl = url;
  }

  getBaseUrl(): string {
    return this.baseUrl;
  }

  setSessionId(id: string | null) {
    this.sessionId = id;
  }

  getSessionId(): string | null {
    return this.sessionId;
  }

  private async request<T>(
    method: 'GET' | 'POST' | 'PUT',
    path: string,
    body?: any
  ): Promise<T> {
    const { status, payload } = await this.fetchJson<any>(method, path, body);

    if (status < 200 || status >= 300) {
      throw new Error(payload?.error || `调度请求失败（HTTP ${status}）: ${path}`);
    }

    if (!payload?.success) {
      throw new Error(payload?.error || `调度请求失败: ${path}`);
    }
    return payload.data as T;
  }

  /**
   * App 启动时初始化会话
   * 若已有旧 session，服务端会立即失效并重新分配
   */
  async initSession(deviceId: string): Promise<string> {
    const data = await this.request<{ sessionId: string }>(
      'POST', '/session/init', { deviceId }
    );
    this.sessionId = data.sessionId;
    return data.sessionId;
  }

  /**
   * 保存蓝牙设备信息
   */
  async setDevice(btAddress: string, btProtocol: string, btName?: string): Promise<void> {
    if (!this.sessionId) throw new Error('未初始化会话');
    await this.request('PUT', `/session/${this.sessionId}/device`, {
      btAddress, btProtocol, btName,
    });
  }

  /**
   * 保存车型配置
   */
  async setCar(carBrand: string, profileIndex: number, profileName: string): Promise<void> {
    if (!this.sessionId) throw new Error('未初始化会话');
    await this.request('PUT', `/session/${this.sessionId}/car`, {
      carBrand,
      profileIndex,
      profileName,
      carModel: profileName,
    });
  }

  async getSessionState(): Promise<SchedulerSessionState> {
    if (!this.sessionId) throw new Error('未初始化会话');
    return this.request('GET', `/session/${this.sessionId}`);
  }

  async getCatalogBrands(): Promise<string[]> {
    return this.request('GET', '/catalog/brands');
  }

  async getCatalogProfiles(brand: string): Promise<SchedulerCatalogProfile[]> {
    const encodedBrand = encodeURIComponent(brand);
    return this.request('GET', `/catalog/profiles?brand=${encodedBrand}`);
  }

  /**
   * 准备连接：分配实例 + 应用车型配置
   * 返回 wsUrl（只有完全成功才返回）
   */
  async reserve(): Promise<{ wsUrl: string; instanceId: string }> {
    if (!this.sessionId) throw new Error('未初始化会话');
    return this.request('POST', `/session/${this.sessionId}/reserve`);
  }

  /**
   * 取消尚未正式连接的预留实例
   * 仅用于“申请实例/等待实例”阶段的用户取消
   */
  async cancelReserve(): Promise<void> {
    if (!this.sessionId) return;
    try {
      await this.request('POST', `/session/${this.sessionId}/cancel-reserve`);
    } catch (e: any) {
      console.log('[SchedulerClient] cancelReserve 跳过（非致命）:', e.message);
    }
  }

  /**
   * 发送心跳
   */
  async heartbeat(): Promise<void> {
    if (!this.sessionId) return;
    try {
      await this.request('POST', `/session/${this.sessionId}/heartbeat`);
    } catch {
      // 心跳失败不抛出，静默处理
    }
  }

  /**
   * 用户主动断开连接
   */
  async disconnect(): Promise<void> {
    if (!this.sessionId) return;
    try {
      await this.request('POST', `/session/${this.sessionId}/disconnect`);
    } catch (e: any) {
      console.log('[SchedulerClient] disconnect 跳过（非致命）:', e.message);
    }
  }

  /**
   * 连接失败或用户放弃后，立即释放实例并重置
   */
  async release(): Promise<void> {
    if (!this.sessionId) return;
    try {
      await this.request('POST', `/session/${this.sessionId}/release`);
    } catch (e: any) {
      console.log('[SchedulerClient] release 跳过（非致命）:', e.message);
    }
  }

  /**
   * 更新运行状态
   */
  async updateStatus(status: 'running' | 'init'): Promise<void> {
    if (!this.sessionId) return;
    try {
      await this.request('PUT', `/session/${this.sessionId}/status`, { status });
    } catch (e: any) {
      console.log('[SchedulerClient] updateStatus 跳过（非致命）:', e.message);
    }
  }

  async checkHealth(): Promise<{ status: string; redis?: string; ts?: number }> {
    const url = this.buildUrl('/health');
    const startedAt = Date.now();
    console.log('[SchedulerClient] 健康检查开始', JSON.stringify({ url }));

    try {
      const res = await fetch(url);
      const text = await res.text();
      console.log('[SchedulerClient] 健康检查响应', JSON.stringify({
        url,
        status: res.status,
        elapsedMs: Date.now() - startedAt,
        bodyPreview: text.slice(0, LOG_BODY_PREVIEW_MAX),
      }));

      if (res.status < 200 || res.status >= 300) {
        throw new Error(`调度健康检查失败（HTTP ${res.status}）`);
      }

      return text ? JSON.parse(text) as { status: string; redis?: string; ts?: number } : { status: 'unknown' };
    } catch (err: any) {
      console.error('[SchedulerClient] 健康检查失败', JSON.stringify({
        url,
        elapsedMs: Date.now() - startedAt,
        error: this.describeError(err),
      }));
      if (err instanceof Error) {
        throw err;
      }
      throw new Error(String(err));
    }
  }
}

const SchedulerClient = new SchedulerClientService();
export default SchedulerClient;
