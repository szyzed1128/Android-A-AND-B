"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.getCatalogSnapshot = getCatalogSnapshot;
exports.getSnapshotBrands = getSnapshotBrands;
exports.getSnapshotProfiles = getSnapshotProfiles;
exports.writeCatalogSnapshot = writeCatalogSnapshot;
exports.buildCatalogSnapshot = buildCatalogSnapshot;
const promises_1 = __importDefault(require("fs/promises"));
const path_1 = __importDefault(require("path"));
let cachedSnapshot = null;
let cachedSnapshotMtimeMs = -1;
function getSnapshotPath() {
    return process.env.CATALOG_SNAPSHOT_PATH || path_1.default.resolve(process.cwd(), 'data/catalog_snapshot.json');
}
async function loadSnapshotFromDisk() {
    const snapshotPath = getSnapshotPath();
    try {
        const stat = await promises_1.default.stat(snapshotPath);
        if (cachedSnapshot && cachedSnapshotMtimeMs === stat.mtimeMs) {
            return cachedSnapshot;
        }
        const raw = await promises_1.default.readFile(snapshotPath, 'utf8');
        const parsed = JSON.parse(raw);
        validateSnapshot(parsed);
        cachedSnapshot = parsed;
        cachedSnapshotMtimeMs = stat.mtimeMs;
        return parsed;
    }
    catch (err) {
        if (err?.code === 'ENOENT') {
            cachedSnapshot = null;
            cachedSnapshotMtimeMs = -1;
            return null;
        }
        throw err;
    }
}
function validateSnapshot(snapshot) {
    if (!snapshot || !Array.isArray(snapshot.brands) || typeof snapshot.profilesByBrand !== 'object' || !snapshot.profilesByBrand) {
        throw new Error('目录快照格式无效');
    }
}
async function getCatalogSnapshot() {
    return loadSnapshotFromDisk();
}
async function getSnapshotBrands() {
    const snapshot = await loadSnapshotFromDisk();
    return snapshot ? snapshot.brands : null;
}
async function getSnapshotProfiles(brand) {
    const snapshot = await loadSnapshotFromDisk();
    if (!snapshot) {
        return null;
    }
    return snapshot.profilesByBrand[brand] || null;
}
async function writeCatalogSnapshot(snapshot) {
    validateSnapshot(snapshot);
    const snapshotPath = getSnapshotPath();
    await promises_1.default.mkdir(path_1.default.dirname(snapshotPath), { recursive: true });
    await promises_1.default.writeFile(snapshotPath, JSON.stringify(snapshot, null, 2), 'utf8');
    cachedSnapshot = snapshot;
    cachedSnapshotMtimeMs = -1;
    return snapshotPath;
}
function buildCatalogSnapshot(brands, profilesByBrand, source) {
    return {
        recordedAt: Date.now(),
        brands,
        profilesByBrand,
        source,
    };
}
