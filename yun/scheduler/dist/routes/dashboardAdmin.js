"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const child_process_1 = require("child_process");
const util_1 = require("util");
const express_1 = require("express");
const agentClient_1 = require("../services/agentClient");
const dashboardService_1 = require("../services/dashboardService");
const redis_1 = require("../services/redis");
const scheduler_1 = require("../services/scheduler");
const execFileAsync = (0, util_1.promisify)(child_process_1.execFile);
const router = (0, express_1.Router)();
router.post('/instances/:id/reset', async (req, res) => {
    try {
        const instance = await mustGetInstance(req.params.id);
        ensureNoActiveSession(instance, '实例仍绑定会话，不能直接重置');
        const redis = (0, redis_1.getRedis)();
        await redis.removeFromIdlePool(instance.id);
        await redis.updateInstanceStatus(instance.id, 'probing', {}, ['reservedForDeviceId', 'reservedUntil']);
        const agent = (0, agentClient_1.createAgentClient)(instance.serverIp, instance.agentPort);
        const result = await agent.reset(instance.id);
        if (!result.success) {
            throw new Error(result.error || '实例重置失败');
        }
        return res.json({ success: true, message: '实例重置已触发' });
    }
    catch (err) {
        return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
    }
});
router.post('/instances/:id/restart-runtime', async (req, res) => {
    try {
        const instance = await mustGetInstance(req.params.id);
        ensureNoActiveSession(instance, '实例仍绑定会话，不能直接重启运行时');
        const redis = (0, redis_1.getRedis)();
        await redis.removeFromIdlePool(instance.id);
        await redis.updateInstanceStatus(instance.id, 'probing', {}, ['reservedForDeviceId', 'reservedUntil']);
        const agent = (0, agentClient_1.createAgentClient)(instance.serverIp, instance.agentPort);
        const result = await agent.restartRuntime(instance.id);
        if (!result.success) {
            throw new Error(result.error || '运行时重启失败');
        }
        return res.json({ success: true, message: result.message || '运行时重启完成' });
    }
    catch (err) {
        return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
    }
});
router.post('/instances/:id/disable', async (req, res) => {
    try {
        const instance = await mustGetInstance(req.params.id);
        const redis = (0, redis_1.getRedis)();
        await redis.setInstanceDisabled(instance.id, true);
        await redis.removeFromIdlePool(instance.id);
        return res.json({ success: true, message: `实例 ${instance.id} 已摘除` });
    }
    catch (err) {
        return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
    }
});
router.post('/instances/:id/enable', async (req, res) => {
    try {
        const instance = await mustGetInstance(req.params.id);
        const redis = (0, redis_1.getRedis)();
        await redis.setInstanceDisabled(instance.id, false);
        await (0, scheduler_1.syncInstanceIdlePool)(instance.id);
        return res.json({ success: true, message: `实例 ${instance.id} 已恢复调度` });
    }
    catch (err) {
        return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
    }
});
router.delete('/instances/:id', async (req, res) => {
    try {
        const instance = await mustGetInstance(req.params.id);
        ensureNoActiveSession(instance, '实例仍绑定会话，不能移除');
        const redis = (0, redis_1.getRedis)();
        await redis.setInstanceDisabled(instance.id, false);
        await redis.removeInstance(instance.id);
        return res.json({ success: true, message: '实例已从调度层移除' });
    }
    catch (err) {
        return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
    }
});
router.post('/instances/restart-bad', async (_req, res) => {
    try {
        const redis = (0, redis_1.getRedis)();
        const instances = await (0, dashboardService_1.getDashboardInstances)();
        const badInstances = instances.filter(item => item.status === 'bad');
        const results = [];
        for (const instance of badInstances) {
            try {
                ensureNoActiveSession(instance, '实例仍绑定会话，不能直接重启运行时');
                await redis.removeFromIdlePool(instance.id);
                await redis.updateInstanceStatus(instance.id, 'probing', {}, ['reservedForDeviceId', 'reservedUntil']);
                const agent = (0, agentClient_1.createAgentClient)(instance.serverIp, instance.agentPort);
                const result = await agent.restartRuntime(instance.id);
                results.push({
                    id: instance.id,
                    success: Boolean(result.success),
                    error: result.error,
                });
            }
            catch (err) {
                results.push({ id: instance.id, success: false, error: err.message });
            }
        }
        return res.json({ success: true, data: { total: badInstances.length, results } });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
router.get('/instances/:id/logs', async (req, res) => {
    try {
        const instance = await mustGetInstance(req.params.id);
        const agent = (0, agentClient_1.createAgentClient)(instance.serverIp, instance.agentPort);
        const runtime = await agent.getInstanceLogs(instance.id);
        const scheduler = await buildSchedulerLog(instance.id);
        return res.json({ success: true, data: { instance, scheduler, runtime } });
    }
    catch (err) {
        return res.status(httpStatusFromError(err)).json({ success: false, error: err.message });
    }
});
router.post('/servers/:serverId/disable', async (req, res) => {
    try {
        const redis = (0, redis_1.getRedis)();
        const serverId = req.params.serverId;
        await redis.setServerDisabled(serverId, true);
        const instances = (await (0, dashboardService_1.getDashboardInstances)()).filter(item => item.serverId === serverId);
        for (const instance of instances) {
            await redis.removeFromIdlePool(instance.id);
        }
        return res.json({ success: true, message: `服务器 ${serverId} 已摘除` });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
router.post('/servers/:serverId/enable', async (req, res) => {
    try {
        const redis = (0, redis_1.getRedis)();
        const serverId = req.params.serverId;
        await redis.setServerDisabled(serverId, false);
        const instances = (await (0, dashboardService_1.getDashboardInstances)()).filter(item => item.serverId === serverId);
        for (const instance of instances) {
            await (0, scheduler_1.syncInstanceIdlePool)(instance.id);
        }
        return res.json({ success: true, message: `服务器 ${serverId} 已恢复调度` });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
async function mustGetInstance(id) {
    const redis = (0, redis_1.getRedis)();
    const instance = await redis.getInstance(id);
    if (instance) {
        return instance;
    }
    const err = new Error(`实例 ${id} 不存在`);
    err.statusCode = 404;
    throw err;
}
function ensureNoActiveSession(instance, message) {
    if (!instance.sessionId)
        return;
    const err = new Error(message);
    err.statusCode = 409;
    throw err;
}
function httpStatusFromError(err) {
    return typeof err?.statusCode === 'number' ? err.statusCode : 500;
}
async function buildSchedulerLog(instanceId) {
    const { stdout } = await execFileAsync('/bin/bash', [
        '-lc',
        `journalctl -u obd-scheduler -n 300 --no-pager | grep '${instanceId}' | tail -120 || true`,
    ], {
        timeout: 20000,
        maxBuffer: 1024 * 1024 * 4,
    });
    return {
        title: 'SchedulerLog',
        content: stdout.trim(),
    };
}
exports.default = router;
