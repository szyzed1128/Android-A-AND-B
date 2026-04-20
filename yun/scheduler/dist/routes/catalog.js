"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = require("express");
const scheduler_1 = require("../services/scheduler");
const router = (0, express_1.Router)();
router.get('/brands', async (_req, res) => {
    try {
        const brands = await (0, scheduler_1.getCatalogBrands)();
        return res.json({ success: true, data: brands });
    }
    catch (err) {
        console.error('[Catalog] 读取品牌列表失败:', err.message);
        return res.status(503).json({ success: false, error: err.message });
    }
});
router.get('/profiles', async (req, res) => {
    try {
        const { brand } = req.query;
        if (!brand || typeof brand !== 'string') {
            return res.status(400).json({ success: false, error: 'brand 必填' });
        }
        const profiles = await (0, scheduler_1.getCatalogProfiles)(brand);
        return res.json({ success: true, data: profiles });
    }
    catch (err) {
        console.error('[Catalog] 读取车型配置失败:', err.message);
        return res.status(503).json({ success: false, error: err.message });
    }
});
exports.default = router;
