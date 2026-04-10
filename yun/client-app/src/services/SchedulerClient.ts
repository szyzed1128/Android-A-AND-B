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

class SchedulerClientService {
  private baseUrl: string = SCHEDULER_BASE_URL;
  private sessionId: string | null = null;

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
    const url = `${this.baseUrl}${path}`;
    const options: RequestInit = {
      method,
      headers: { 'Content-Type': 'application/json' },
    };
    if (body) options.body = JSON.stringify(body);

    const res = await fetch(url, options);
    const json = await res.json() as any;

    if (!json.success) {
      throw new Error(json.error || `请求失败: ${path}`);
    }
    return json.data as T;
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
  async setCar(carBrand: string, carModel: string): Promise<void> {
    if (!this.sessionId) throw new Error('未初始化会话');
    await this.request('PUT', `/session/${this.sessionId}/car`, {
      carBrand, carModel,
    });
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
      console.warn('[SchedulerClient] disconnect 失败:', e.message);
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
      console.warn('[SchedulerClient] updateStatus 失败:', e.message);
    }
  }
}

const SchedulerClient = new SchedulerClientService();
export default SchedulerClient;
