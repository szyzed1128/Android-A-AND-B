"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = require("express");
const dashboardService_1 = require("../services/dashboardService");
const router = (0, express_1.Router)();
router.get('/overview', async (_req, res) => {
    try {
        const overview = await (0, dashboardService_1.getDashboardOverview)();
        return res.json({ success: true, data: overview });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
router.get('/instances', async (_req, res) => {
    try {
        const instances = await (0, dashboardService_1.getDashboardInstances)();
        return res.json({ success: true, data: instances });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
router.get('/servers', async (_req, res) => {
    try {
        const servers = await (0, dashboardService_1.getDashboardServers)();
        return res.json({ success: true, data: servers });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
router.get('/sessions', async (_req, res) => {
    try {
        const instances = await (0, dashboardService_1.getDashboardInstances)();
        const sessions = await (0, dashboardService_1.getDashboardSessions)(instances);
        return res.json({ success: true, data: sessions });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
router.get('/anomalies', async (_req, res) => {
    try {
        const instances = await (0, dashboardService_1.getDashboardInstances)();
        const sessions = await (0, dashboardService_1.getDashboardSessions)(instances);
        const anomalies = (0, dashboardService_1.getDashboardAnomalies)(instances, sessions);
        return res.json({ success: true, data: anomalies });
    }
    catch (err) {
        return res.status(500).json({ success: false, error: err.message });
    }
});
exports.default = router;
