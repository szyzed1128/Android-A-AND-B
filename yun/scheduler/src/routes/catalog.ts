import { Router, Request, Response } from 'express';
import { getCatalogBrands, getCatalogProfiles } from '../services/scheduler';

const router = Router();

router.get('/brands', async (_req: Request, res: Response) => {
  try {
    const brands = await getCatalogBrands();
    return res.json({ success: true, data: brands });
  } catch (err: any) {
    console.error('[Catalog] 读取品牌列表失败:', err.message);
    return res.status(503).json({ success: false, error: err.message });
  }
});

router.get('/profiles', async (req: Request, res: Response) => {
  try {
    const { brand } = req.query;
    if (!brand || typeof brand !== 'string') {
      return res.status(400).json({ success: false, error: 'brand 必填' });
    }

    const profiles = await getCatalogProfiles(brand);
    return res.json({ success: true, data: profiles });
  } catch (err: any) {
    console.error('[Catalog] 读取车型配置失败:', err.message);
    return res.status(503).json({ success: false, error: err.message });
  }
});

export default router;
