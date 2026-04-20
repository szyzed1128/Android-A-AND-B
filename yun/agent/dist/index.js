"use strict";
/**
 * Agent 入口
 */
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const config_1 = __importDefault(require("./config"));
const instanceManager_1 = require("./services/instanceManager");
const healthReporter_1 = require("./services/healthReporter");
const instance_1 = __importDefault(require("./routes/instance"));
const app = (0, express_1.default)();
app.use(express_1.default.json());
// 日志
app.use((req, res, next) => {
    console.log(`[HTTP] ${req.method} ${req.path}`);
    next();
});
// 路由
app.use('/instance', instance_1.default);
// Agent 自身健康检查
app.get('/health', (req, res) => {
    const statuses = (0, instanceManager_1.getAllInstanceStatuses)();
    res.json({
        status: 'ok',
        serverId: config_1.default.serverId,
        instances: statuses,
        ts: Date.now(),
    });
});
// 启动
function main() {
    // 初始化所有实例（触发就绪流程）
    (0, instanceManager_1.initInstances)(config_1.default.instances);
    // 启动健康上报
    (0, healthReporter_1.startHealthReporter)(config_1.default);
    app.listen(config_1.default.agentPort, () => {
        console.log(`[Agent] 已启动 serverId=${config_1.default.serverId} port=${config_1.default.agentPort}`);
        console.log(`[Agent] 管理 ${config_1.default.instances.length} 个实例`);
        console.log(`[Agent] 调度后端=${config_1.default.schedulerUrl}`);
    });
}
main();
