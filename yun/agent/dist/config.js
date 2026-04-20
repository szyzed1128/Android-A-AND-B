"use strict";
/**
 * Agent 本机配置
 * 每台 EC2 部署时修改此文件（或通过环境变量覆盖）
 */
Object.defineProperty(exports, "__esModule", { value: true });
function getEnv(key, fallback) {
    return process.env[key] || fallback;
}
// 默认配置（单台服务器示例）
// 实际部署时通过环境变量或修改此文件配置
//
// 端口说明：
//   wsPort    = Nginx 对外暴露端口（B端连接用），如 8081
//   probePort = adb forward 内部端口（Agent 探针连接用），如 18081
//   多实例时每个实例各自独立：
//     实例1: wsPort=8081, probePort=18081, adb forward tcp:18081 tcp:8080
//     实例2: wsPort=8082, probePort=18082, adb forward tcp:18082 tcp:8080
const config = {
    serverId: getEnv('SERVER_ID', 'server_default'),
    agentPort: parseInt(getEnv('AGENT_PORT', '4000')),
    schedulerUrl: getEnv('SCHEDULER_URL', 'http://scheduler:3000'),
    healthReportIntervalMs: 60 * 1000,
    deploymentScriptsDir: getEnv('DEPLOYMENT_SCRIPTS_DIR', '/opt/cardemo/scripts'),
    serverIpOverride: process.env.SERVER_IP_OVERRIDE || undefined,
    publicWsHost: process.env.PUBLIC_WS_HOST || undefined,
    publicWsScheme: getEnv('PUBLIC_WS_SCHEME', 'ws') === 'wss' ? 'wss' : 'ws',
    instances: JSON.parse(getEnv('INSTANCES_CONFIG', JSON.stringify([
        {
            id: `${getEnv('SERVER_ID', 'server_default')}_inst_1`,
            wsPort: 8081, // Nginx 对外端口，B端连接用
            probePort: 18081, // adb forward 内部端口，Agent 探针用
            adbTarget: 'localhost:5555',
            packageName: 'com.companyname.cardemo',
            activityName: 'crc64b16463db6be126c1.MainActivity',
        },
        // 多实例时追加，例如：
        // { id: '..._inst_2', wsPort: 8082, probePort: 18082, adbTarget: 'localhost:5556', ... }
    ]))),
};
exports.default = config;
