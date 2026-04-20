"use strict";
/**
 * 调度后端入口
 */
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || (function () {
    var ownKeys = function(o) {
        ownKeys = Object.getOwnPropertyNames || function (o) {
            var ar = [];
            for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) ar[ar.length] = k;
            return ar;
        };
        return ownKeys(o);
    };
    return function (mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k = ownKeys(mod), i = 0; i < k.length; i++) if (k[i] !== "default") __createBinding(result, mod, k[i]);
        __setModuleDefault(result, mod);
        return result;
    };
})();
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const fs_1 = __importDefault(require("fs"));
const path_1 = __importDefault(require("path"));
const redis_1 = require("./services/redis");
const scheduler_1 = require("./services/scheduler");
const session_1 = __importDefault(require("./routes/session"));
const instance_1 = __importDefault(require("./routes/instance"));
const dashboard_1 = __importDefault(require("./routes/dashboard"));
const dashboardAdmin_1 = __importDefault(require("./routes/dashboardAdmin"));
const catalog_1 = __importDefault(require("./routes/catalog"));
const app = (0, express_1.default)();
const PORT = process.env.PORT ? parseInt(process.env.PORT) : 3000;
const REDIS_URL = process.env.REDIS_URL || 'redis://localhost:6379';
app.use(express_1.default.json());
// 请求日志
app.use((req, res, next) => {
    console.log(`[HTTP] ${req.method} ${req.path}`);
    next();
});
// 路由
app.use('/session', session_1.default);
app.use('/instance', instance_1.default);
app.use('/catalog', catalog_1.default);
app.use('/dashboard/api', dashboard_1.default);
app.use('/dashboard/api/admin', dashboardAdmin_1.default);
const dashboardDir = resolveDashboardDir();
app.use('/dashboard', express_1.default.static(dashboardDir));
// Agent 健康上报入口
app.post('/agent/health', async (req, res) => {
    try {
        await (0, scheduler_1.handleAgentHealthReport)(req.body);
        res.json({ success: true });
    }
    catch (err) {
        console.error('[Scheduler] 处理健康上报失败:', err.message);
        res.status(500).json({ success: false, error: err.message });
    }
});
// 调度后端自身健康检查
app.get('/health', async (req, res) => {
    const redis = await Promise.resolve().then(() => __importStar(require('./services/redis'))).then(m => m.getRedis());
    const redisOk = await redis.ping();
    res.json({ status: 'ok', redis: redisOk ? 'ok' : 'error', ts: Date.now() });
});
// 启动
async function main() {
    try {
        await (0, redis_1.initRedis)(REDIS_URL);
        (0, scheduler_1.startHeartbeatMonitor)();
        app.listen(PORT, () => {
            console.log(`[Scheduler] 调度服务已启动 port=${PORT}`);
            console.log(`[Scheduler] Redis=${REDIS_URL}`);
            console.log(`[Scheduler] DashboardDir=${dashboardDir}`);
        });
    }
    catch (err) {
        console.error('[Scheduler] 启动失败:', err.message);
        process.exit(1);
    }
}
function resolveDashboardDir() {
    const configured = (process.env.DASHBOARD_WEB_DIR || '').trim();
    const candidates = Array.from(new Set([
        configured || null,
        path_1.default.resolve(process.cwd(), '../dashboard-web'),
        path_1.default.resolve(__dirname, '../../dashboard-web'),
    ].filter((item) => Boolean(item))));
    for (const candidate of candidates) {
        const indexFile = path_1.default.join(candidate, 'index.html');
        if (fs_1.default.existsSync(indexFile)) {
            return candidate;
        }
    }
    throw new Error(`[Scheduler] 未找到 dashboard-web 目录，候选路径: ${candidates.join(', ')}`);
}
main();
