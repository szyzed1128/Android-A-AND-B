"use strict";
/**
 * 实例操作路由
 * 被调度后端 HTTP 调用
 */
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = require("express");
const instanceManager_1 = require("../services/instanceManager");
const apkProbe_1 = require("../services/apkProbe");
const runtimeAdmin_1 = require("../services/runtimeAdmin");
const config_1 = __importDefault(require("../config"));
const router = (0, express_1.Router)();
/**
 * POST /instance/:id/applyCar
 * 应用用户选择的车型配置
 * 调度后端在分配实例给用户时调用
 */
router.post('/:id/applyCar', async (req, res) => {
    const { id } = req.params;
    const { carBrand, profileIndex, profileName } = req.body;
    if (!carBrand || !Number.isInteger(profileIndex)) {
        return res.status(400).json({ success: false, error: 'carBrand/profileIndex 必填' });
    }
    const instanceConfig = config_1.default.instances.find(i => i.id === id);
    if (!instanceConfig) {
        return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
    }
    const status = (0, instanceManager_1.getInstanceStatus)(id);
    if (status !== 'idle') {
        return res.status(409).json({
            success: false,
            error: `实例 ${id} 状态为 ${status}，无法应用配置`,
        });
    }
    console.log(`[Agent] applyCar ${id}: ${carBrand} index=${profileIndex} name=${profileName || '-'}`);
    const result = await (0, apkProbe_1.applyCarProfile)(instanceConfig, carBrand, profileIndex, profileName);
    return res.json(result);
});
router.get('/:id/catalog/brands', async (req, res) => {
    const { id } = req.params;
    const instanceConfig = config_1.default.instances.find(i => i.id === id);
    if (!instanceConfig) {
        return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
    }
    const status = (0, instanceManager_1.getInstanceStatus)(id);
    if (status !== 'idle') {
        return res.status(409).json({ success: false, error: `实例 ${id} 状态为 ${status}，无法读取目录` });
    }
    try {
        const brands = await (0, apkProbe_1.getCatalogBrands)(instanceConfig);
        return res.json({ success: true, data: brands });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
router.get('/:id/catalog/profiles', async (req, res) => {
    const { id } = req.params;
    const { brand } = req.query;
    if (!brand || typeof brand !== 'string') {
        return res.status(400).json({ success: false, error: 'brand 必填' });
    }
    const instanceConfig = config_1.default.instances.find(i => i.id === id);
    if (!instanceConfig) {
        return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
    }
    const status = (0, instanceManager_1.getInstanceStatus)(id);
    if (status !== 'idle') {
        return res.status(409).json({ success: false, error: `实例 ${id} 状态为 ${status}，无法读取目录` });
    }
    try {
        const profiles = await (0, apkProbe_1.getCatalogProfiles)(instanceConfig, brand);
        return res.json({ success: true, data: profiles });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
/**
 * POST /instance/:id/reset
 * 触发实例就绪流程（重置后重新探针直到idle）
 * 调度后端释放实例时调用
 */
router.post('/:id/reset', async (req, res) => {
    const { id } = req.params;
    const instanceConfig = config_1.default.instances.find(i => i.id === id);
    if (!instanceConfig) {
        return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
    }
    console.log(`[Agent] 触发重置 ${id}`);
    (0, instanceManager_1.triggerReset)(id); // 异步执行，立即返回
    return res.json({ success: true, message: '重置已触发，正在后台执行' });
});
router.post('/:id/restart-runtime', async (req, res) => {
    const { id } = req.params;
    const instanceConfig = config_1.default.instances.find(i => i.id === id);
    if (!instanceConfig) {
        return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
    }
    try {
        await (0, runtimeAdmin_1.restartInstanceRuntime)(instanceConfig);
        return res.json({ success: true, message: '运行时重启完成' });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
router.get('/:id/logs', async (req, res) => {
    const { id } = req.params;
    const instanceConfig = config_1.default.instances.find(i => i.id === id);
    if (!instanceConfig) {
        return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
    }
    try {
        const data = await (0, runtimeAdmin_1.collectInstanceRuntimeLogs)(instanceConfig);
        return res.json({ success: true, data });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
/**
 * GET /instance/:id/status
 * 查询单实例状态
 */
router.get('/:id/status', (req, res) => {
    const { id } = req.params;
    const status = (0, instanceManager_1.getInstanceStatus)(id);
    if (status === null) {
        return res.status(404).json({ success: false, error: `实例 ${id} 不存在` });
    }
    return res.json({
        success: true,
        data: { id, status, health: status === 'bad' ? 'bad' : 'ok' },
    });
});
/**
 * GET /instance/all/status
 * 查询所有实例状态（运维用）
 */
router.get('/all/status', (req, res) => {
    const statuses = (0, instanceManager_1.getAllInstanceStatuses)();
    return res.json({ success: true, data: statuses });
});
exports.default = router;
