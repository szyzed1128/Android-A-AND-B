import { createAgentClient } from '../services/agentClient';
import { buildCatalogSnapshot, writeCatalogSnapshot } from '../services/catalogSnapshot';
import { initRedis, getRedis } from '../services/redis';
import { CatalogProfileSummary, InstanceInfo } from '../types';

const REDIS_URL = process.env.REDIS_URL || 'redis://localhost:6379';
const RETRY_PER_BRAND = 3;

function sortInstances(instances: InstanceInfo[]): InstanceInfo[] {
  return [...instances].sort((a, b) => (b.lastHealthAt || 0) - (a.lastHealthAt || 0));
}

async function getIdleCatalogCandidates(): Promise<InstanceInfo[]> {
  const redis = getRedis();
  const ids = await redis.getAllInstanceIds();
  const instances: InstanceInfo[] = [];

  for (const id of ids) {
    const instance = await redis.getInstance(id);
    if (!instance) {
      continue;
    }
    if (instance.status !== 'idle') {
      continue;
    }
    if (instance.health === 'bad') {
      continue;
    }
    instances.push(instance);
  }

  return sortInstances(instances);
}

async function fetchBrands(candidates: InstanceInfo[]): Promise<{ brands: string[]; source: InstanceInfo }> {
  let lastError = '读取品牌列表失败';

  for (const instance of candidates) {
    try {
      const agent = createAgentClient(instance.serverIp, instance.agentPort);
      const brands = await agent.getCatalogBrands(instance.id);
      if (!brands.length) {
        throw new Error(`实例 ${instance.id} 返回空品牌列表`);
      }
      return { brands, source: instance };
    } catch (err: any) {
      lastError = err.message || lastError;
      console.warn(`[CatalogRecord] getBrands 失败 instance=${instance.id}: ${lastError}`);
    }
  }

  throw new Error(lastError);
}

async function fetchProfilesForBrand(
  brand: string,
  candidates: InstanceInfo[]
): Promise<CatalogProfileSummary[]> {
  let lastError = `读取品牌 ${brand} 的配置失败`;

  for (let attempt = 1; attempt <= RETRY_PER_BRAND; attempt++) {
    for (const instance of candidates) {
      try {
        const agent = createAgentClient(instance.serverIp, instance.agentPort);
        const profiles = await agent.getCatalogProfiles(instance.id, brand);
        if (!profiles.length) {
          throw new Error(`品牌 ${brand} 返回空配置列表`);
        }
        return profiles;
      } catch (err: any) {
        lastError = err.message || lastError;
        console.warn(`[CatalogRecord] getProfiles 失败 brand=${brand} instance=${instance.id} attempt=${attempt}: ${lastError}`);
      }
    }
  }

  throw new Error(lastError);
}

async function main(): Promise<void> {
  await initRedis(REDIS_URL);

  const candidates = await getIdleCatalogCandidates();
  if (!candidates.length) {
    throw new Error('当前没有可用于录制目录快照的 idle 实例');
  }

  const { brands, source } = await fetchBrands(candidates);
  const profilesByBrand: Record<string, CatalogProfileSummary[]> = {};

  console.log(`[CatalogRecord] 开始录制目录，共 ${brands.length} 个品牌`);
  for (let index = 0; index < brands.length; index++) {
    const brand = brands[index];
    const profiles = await fetchProfilesForBrand(brand, candidates);
    profilesByBrand[brand] = profiles.map((profile, profileIndex) => ({
      profileIndex,
      name: profile.name,
      description: profile.description,
    }));
    console.log(`[CatalogRecord] ${index + 1}/${brands.length} ${brand} -> ${profiles.length} 个配置`);
  }

  const snapshot = buildCatalogSnapshot(brands, profilesByBrand, {
    instanceId: source.id,
    serverIp: source.serverIp,
    agentPort: source.agentPort,
  });
  const snapshotPath = await writeCatalogSnapshot(snapshot);

  const redis = getRedis();
  await redis.setCatalogBrandsCache(brands);
  for (const brand of brands) {
    await redis.setCatalogProfilesCache(brand, profilesByBrand[brand]);
  }

  console.log(`[CatalogRecord] 录制完成 path=${snapshotPath} brands=${brands.length}`);
}

main().catch((err) => {
  console.error('[CatalogRecord] 录制失败:', err.message);
  process.exit(1);
});
