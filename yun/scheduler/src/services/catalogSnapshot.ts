import fs from 'fs/promises';
import path from 'path';
import { CatalogProfileSummary, CatalogSnapshot } from '../types';

let cachedSnapshot: CatalogSnapshot | null = null;
let cachedSnapshotMtimeMs = -1;

function getSnapshotPath(): string {
  return process.env.CATALOG_SNAPSHOT_PATH || path.resolve(process.cwd(), 'data/catalog_snapshot.json');
}

async function loadSnapshotFromDisk(): Promise<CatalogSnapshot | null> {
  const snapshotPath = getSnapshotPath();

  try {
    const stat = await fs.stat(snapshotPath);
    if (cachedSnapshot && cachedSnapshotMtimeMs === stat.mtimeMs) {
      return cachedSnapshot;
    }

    const raw = await fs.readFile(snapshotPath, 'utf8');
    const parsed = JSON.parse(raw) as CatalogSnapshot;
    validateSnapshot(parsed);

    cachedSnapshot = parsed;
    cachedSnapshotMtimeMs = stat.mtimeMs;
    return parsed;
  } catch (err: any) {
    if (err?.code === 'ENOENT') {
      cachedSnapshot = null;
      cachedSnapshotMtimeMs = -1;
      return null;
    }
    throw err;
  }
}

function validateSnapshot(snapshot: CatalogSnapshot): void {
  if (!snapshot || !Array.isArray(snapshot.brands) || typeof snapshot.profilesByBrand !== 'object' || !snapshot.profilesByBrand) {
    throw new Error('目录快照格式无效');
  }
}

export async function getCatalogSnapshot(): Promise<CatalogSnapshot | null> {
  return loadSnapshotFromDisk();
}

export async function getSnapshotBrands(): Promise<string[] | null> {
  const snapshot = await loadSnapshotFromDisk();
  return snapshot ? snapshot.brands : null;
}

export async function getSnapshotProfiles(brand: string): Promise<CatalogProfileSummary[] | null> {
  const snapshot = await loadSnapshotFromDisk();
  if (!snapshot) {
    return null;
  }
  return snapshot.profilesByBrand[brand] || null;
}

export async function writeCatalogSnapshot(snapshot: CatalogSnapshot): Promise<string> {
  validateSnapshot(snapshot);

  const snapshotPath = getSnapshotPath();
  await fs.mkdir(path.dirname(snapshotPath), { recursive: true });
  await fs.writeFile(snapshotPath, JSON.stringify(snapshot, null, 2), 'utf8');

  cachedSnapshot = snapshot;
  cachedSnapshotMtimeMs = -1;

  return snapshotPath;
}

export function buildCatalogSnapshot(
  brands: string[],
  profilesByBrand: Record<string, CatalogProfileSummary[]>,
  source?: CatalogSnapshot['source']
): CatalogSnapshot {
  return {
    recordedAt: Date.now(),
    brands,
    profilesByBrand,
    source,
  };
}
