/**
 * Agent HTTP 客户端
 * 调度后端 → 本地Agent 的HTTP调用封装
 */

import axios, { AxiosInstance } from 'axios';

interface ApplyCarRequest {
  carBrand: string;
  profileIndex: number;
  profileName?: string;
}

interface ApplyCarResponse {
  success: boolean;
  error?: string;
}

interface ResetResponse {
  success: boolean;
  error?: string;
}

interface RuntimeRestartResponse {
  success: boolean;
  error?: string;
  message?: string;
}

export interface CatalogProfileResponseItem {
  profileIndex: number;
  name: string;
  description?: string;
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
  async applyCar(
    instanceId: string,
    carBrand: string,
    profileIndex: number,
    profileName?: string
  ): Promise<ApplyCarResponse> {
    try {
      const res = await this.http.post<ApplyCarResponse>(
        `/instance/${instanceId}/applyCar`,
        { carBrand, profileIndex, profileName } as ApplyCarRequest
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

  async getCatalogBrands(instanceId: string): Promise<string[]> {
    const res = await this.http.get<{ success: boolean; data?: string[]; error?: string }>(
      `/instance/${instanceId}/catalog/brands`,
      { timeout: 15000 }
    );
    if (!res.data.success) {
      throw new Error(res.data.error || '读取品牌列表失败');
    }
    return res.data.data || [];
  }

  async getCatalogProfiles(instanceId: string, brand: string): Promise<CatalogProfileResponseItem[]> {
    const res = await this.http.get<{ success: boolean; data?: CatalogProfileResponseItem[]; error?: string }>(
      `/instance/${instanceId}/catalog/profiles`,
      {
        timeout: 15000,
        params: { brand },
      }
    );
    if (!res.data.success) {
      throw new Error(res.data.error || '读取车型配置失败');
    }
    return res.data.data || [];
  }

  getBaseUrl(): string {
    return this.baseUrl;
  }

  async restartRuntime(instanceId: string): Promise<RuntimeRestartResponse> {
    try {
      const res = await this.http.post<RuntimeRestartResponse>(
        `/instance/${instanceId}/restart-runtime`,
        {},
        { timeout: 300000 }
      );
      return res.data;
    } catch (err: any) {
      console.error(`[AgentClient] restartRuntime 失败 instance=${instanceId}:`, err.message);
      return { success: false, error: err.message };
    }
  }

  async getInstanceLogs(instanceId: string): Promise<any> {
    const res = await this.http.get<{ success: boolean; data?: any; error?: string }>(
      `/instance/${instanceId}/logs`,
      { timeout: 30000 }
    );
    if (!res.data.success) {
      throw new Error(res.data.error || '读取实例日志失败');
    }
    return res.data.data;
  }
}

/**
 * 根据实例信息创建 AgentClient
 */
export function createAgentClient(serverIp: string, agentPort: number): AgentClient {
  return new AgentClient(serverIp, agentPort);
}
