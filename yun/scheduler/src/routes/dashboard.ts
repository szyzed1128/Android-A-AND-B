import { Router, Request, Response } from 'express';
import {
  getDashboardAnomalies,
  getDashboardInstances,
  getDashboardOverview,
  getDashboardSessions,
} from '../services/dashboardService';

const router = Router();

router.get('/overview', async (_req: Request, res: Response) => {
  try {
    const overview = await getDashboardOverview();
    return res.json({ success: true, data: overview });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

router.get('/instances', async (_req: Request, res: Response) => {
  try {
    const instances = await getDashboardInstances();
    return res.json({ success: true, data: instances });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

router.get('/sessions', async (_req: Request, res: Response) => {
  try {
    const instances = await getDashboardInstances();
    const sessions = await getDashboardSessions(instances);
    return res.json({ success: true, data: sessions });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

router.get('/anomalies', async (_req: Request, res: Response) => {
  try {
    const instances = await getDashboardInstances();
    const sessions = await getDashboardSessions(instances);
    const anomalies = getDashboardAnomalies(instances, sessions);
    return res.json({ success: true, data: anomalies });
  } catch (err: any) {
    return res.status(500).json({ success: false, error: err.message });
  }
});

export default router;
