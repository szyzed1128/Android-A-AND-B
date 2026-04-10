/**
 * Agent HTTP 客户端
 * 调度后端 → 本地Agent 的HTTP调用封装
 */

import axios, { AxiosInstance } from 'axios';

interface ApplyCarRequest {
  carBrand: string;
  carModel: string;
}

interface ApplyCarResponse {
  success: boolean;
  error?: string;
}

interface ResetResponse {
  success: boolean;
  error?: string;
}

interface InstanceStatusResponse {
  id: string;
  status: string;
  health: 'ok' | 'bad';
}

export class AgentClient {
  private http: AxiosInstance;
  private baseUrl: string;

  constructor(serverIp: string, agentPort: number, timeoutMs = 30000) {
    this.baseUrl = `http://${serverIp}:${agentPort}`;
    this.http = axios.create({
      baseURL: this.baseUrl,
      timeout: timeoutMs,
    });
  }

  /**
   * 应用用户车型配置
   * Agent 建立 WebSocket → applyProfile → 断开
   */
  async applyCar(instanceId: string, carBrand: string, carModel: string): Promise<ApplyCarResponse> {
    try {
      const res = await this.http.post<ApplyCarResponse>(
        `/instance/${instanceId}/applyCar`,
        { carBrand, carModel } as ApplyCarRequest
      );
      return res.data;
    } catch (err: any) {
      console.error(`[AgentClient] applyCar 失败 instance=${instanceId}:`, err.message);
      return { success: false, error: err.message };
    }
  }

  /**
   * 触发实例就绪流程（重置 + 探针 + 标记idle）
   * 仅通知，不等待完成（异步执行）
   */
  async reset(instanceId: string): Promise<ResetResponse> {
    try {
      const res = await this.http.post<ResetResponse>(
        `/instance/${instanceId}/reset`,
        {},
        { timeout: 5000 }  // 仅等待确认收到，不等待完成
      );
      return res.data;
    } catch (err: any) {
      console.error(`[AgentClient] reset 失败 instance=${instanceId}:`, err.message);
      return { success: false, error: err.message };
    }
  }

  /**
   * 查询单实例状态
   */
  async getInstanceStatus(instanceId: string): Promise<InstanceStatusResponse | null> {
    try {
      const res = await this.http.get<InstanceStatusResponse>(
        `/instance/${instanceId}/status`,
        { timeout: 5000 }
      );
      return res.data;
    } catch {
      return null;
    }
  }

  /**
   * 检查 Agent 自身是否在线
   */
  async health(): Promise<boolean> {
    try {
      await this.http.get('/health', { timeout: 3000 });
      return true;
    } catch {
      return false;
    }
  }

  getBaseUrl(): string {
    return this.baseUrl;
  }
}

/**
 * 根据实例信息创建 AgentClient
 */
export function createAgentClient(serverIp: string, agentPort: number): AgentClient {
  return new AgentClient(serverIp, agentPort);
}
