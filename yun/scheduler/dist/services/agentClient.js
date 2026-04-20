"use strict";
/**
 * Agent HTTP 客户端
 * 调度后端 → 本地Agent 的HTTP调用封装
 */
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.AgentClient = void 0;
exports.createAgentClient = createAgentClient;
const axios_1 = __importDefault(require("axios"));
class AgentClient {
    constructor(serverIp, agentPort, timeoutMs = 30000) {
        this.baseUrl = `http://${serverIp}:${agentPort}`;
        this.http = axios_1.default.create({
            baseURL: this.baseUrl,
            timeout: timeoutMs,
        });
    }
    /**
     * 应用用户车型配置
     * Agent 建立 WebSocket → applyProfile → 断开
     */
    async applyCar(instanceId, carBrand, profileIndex, profileName) {
        try {
            const res = await this.http.post(`/instance/${instanceId}/applyCar`, { carBrand, profileIndex, profileName });
            return res.data;
        }
        catch (err) {
            console.error(`[AgentClient] applyCar 失败 instance=${instanceId}:`, err.message);
            return { success: false, error: err.message };
        }
    }
    /**
     * 触发实例就绪流程（重置 + 探针 + 标记idle）
     * 仅通知，不等待完成（异步执行）
     */
    async reset(instanceId) {
        try {
            const res = await this.http.post(`/instance/${instanceId}/reset`, {}, { timeout: 5000 } // 仅等待确认收到，不等待完成
            );
            return res.data;
        }
        catch (err) {
            console.error(`[AgentClient] reset 失败 instance=${instanceId}:`, err.message);
            return { success: false, error: err.message };
        }
    }
    /**
     * 查询单实例状态
     */
    async getInstanceStatus(instanceId) {
        try {
            const res = await this.http.get(`/instance/${instanceId}/status`, { timeout: 5000 });
            return res.data;
        }
        catch {
            return null;
        }
    }
    /**
     * 检查 Agent 自身是否在线
     */
    async health() {
        try {
            await this.http.get('/health', { timeout: 3000 });
            return true;
        }
        catch {
            return false;
        }
    }
    async getCatalogBrands(instanceId) {
        const res = await this.http.get(`/instance/${instanceId}/catalog/brands`, { timeout: 15000 });
        if (!res.data.success) {
            throw new Error(res.data.error || '读取品牌列表失败');
        }
        return res.data.data || [];
    }
    async getCatalogProfiles(instanceId, brand) {
        const res = await this.http.get(`/instance/${instanceId}/catalog/profiles`, {
            timeout: 15000,
            params: { brand },
        });
        if (!res.data.success) {
            throw new Error(res.data.error || '读取车型配置失败');
        }
        return res.data.data || [];
    }
    getBaseUrl() {
        return this.baseUrl;
    }
    async restartRuntime(instanceId) {
        try {
            const res = await this.http.post(`/instance/${instanceId}/restart-runtime`, {}, { timeout: 300000 });
            return res.data;
        }
        catch (err) {
            console.error(`[AgentClient] restartRuntime 失败 instance=${instanceId}:`, err.message);
            return { success: false, error: err.message };
        }
    }
    async getInstanceLogs(instanceId) {
        const res = await this.http.get(`/instance/${instanceId}/logs`, { timeout: 30000 });
        if (!res.data.success) {
            throw new Error(res.data.error || '读取实例日志失败');
        }
        return res.data.data;
    }
}
exports.AgentClient = AgentClient;
/**
 * 根据实例信息创建 AgentClient
 */
function createAgentClient(serverIp, agentPort) {
    return new AgentClient(serverIp, agentPort);
}
