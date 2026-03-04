/**
 * WebSocket消息协议定义
 * 与C#端 OBDCloud.WebSocket.MessageProtocol 保持一致
 */

// 消息类型
export const MessageType = {
  Request: 'request',
  Response: 'response',
  Event: 'event',
} as const;

// 操作类型
export const MessageAction = {
  // 基础消息
  Ping: 'ping',
  Pong: 'pong',

  // 蓝牙扫描（本地执行）
  StartScan: 'startScan',
  StopScan: 'stopScan',
  DeviceDiscovered: 'deviceDiscovered',
  ScanFinished: 'scanFinished',

  // 蓝牙连接
  Connect: 'connect',
  Disconnect: 'disconnect',
  Send: 'send',
  OBDData: 'obdData',
  ConnectionLost: 'connectionLost',
  Error: 'error',

  // ===== 诊断命令（发送到云端A）=====

  // 车辆配置
  GetBrands: 'getBrands',
  GetProfiles: 'getProfiles',
  ApplyProfile: 'applyProfile',

  // ECU 操作
  GetECUList: 'getECUList',
  ReadECUInfo: 'readECUInfo',

  // 故障码
  ReadDTC: 'readDTC',
  ClearDTC: 'clearDTC',

  // 冻结帧
  ReadFreezeFrame: 'readFreezeFrame',

  // 实时数据
  GetPIDList: 'getPIDList',
  StartReadPIDs: 'startReadPIDs',
  StopReadPIDs: 'stopReadPIDs',

  // OBD 连接（通过云端）
  OBDConnect: 'obdConnect',
  OBDDisconnect: 'obdDisconnect',

  // ===== 云端事件（从云端A接收）=====
  OBDStatusChanged: 'obdStatusChanged',
  DTCResult: 'dtcResult',
  DTCCleared: 'dtcCleared',
  ECUInfoResult: 'ecuInfoResult',
  FreezeFrameResult: 'freezeFrameResult',
  PIDValueChanged: 'pidValueChanged',
  BrandsResult: 'brandsResult',
  ProfilesResult: 'profilesResult',
  ECUListResult: 'ecuListResult',
  PIDListResult: 'pidListResult',
} as const;

// 错误码
export const ErrorCode = {
  BT_UNAVAILABLE: 'BT_UNAVAILABLE',
  BT_DISABLED: 'BT_DISABLED',
  DEVICE_NOT_FOUND: 'DEVICE_NOT_FOUND',
  CONNECTION_TIMEOUT: 'CONNECTION_TIMEOUT',
  CONNECTION_LOST: 'CONNECTION_LOST',
  SEND_FAILED: 'SEND_FAILED',
  INVALID_SESSION: 'INVALID_SESSION',
  PERMISSION_DENIED: 'PERMISSION_DENIED',
} as const;

// 消息结构
export interface WSMessage {
  type: string;
  action: string;
  requestId?: string;
  sessionId?: string;
  data?: any;
  success?: boolean;
  error?: string;
  timestamp?: number;
}

// 蓝牙设备信息
export interface BTDeviceInfo {
  name: string;
  address: string;
  rssi?: number;
  valid: boolean;
  paired: boolean;
  protocol?: 'classic' | 'ble' | 'mfi';  // 蓝牙协议类型
  serviceUUIDs?: string[];               // BLE服务UUID列表
}

// 连接请求数据
export interface ConnectRequestData {
  protocol: 'classic' | 'ble' | 'mfi';  // 蓝牙协议类型
  address: string;
}

// 错误数据
export interface ErrorData {
  code: string;
  message: string;
}

/**
 * 生成短UUID
 */
function shortId(): string {
  return Math.random().toString(36).substring(2, 10);
}

/**
 * 消息构建工厂 - B端使用（响应和事件消息）
 */
export const WSMessageFactory = {
  // --- 响应消息 ---

  createResponse(requestId: string, action: string, success: boolean, error?: string): WSMessage {
    return {
      type: MessageType.Response,
      action,
      requestId,
      success,
      error,
      timestamp: Date.now(),
    };
  },

  createConnectResponse(requestId: string, success: boolean, sessionId?: string, error?: string): WSMessage {
    return {
      type: MessageType.Response,
      action: MessageAction.Connect,
      requestId,
      success,
      sessionId,
      error,
      timestamp: Date.now(),
    };
  },

  createPongResponse(requestId: string): WSMessage {
    return {
      type: MessageType.Response,
      action: MessageAction.Pong,
      requestId,
      success: true,
      timestamp: Date.now(),
    };
  },

  // --- 事件消息 ---

  createDeviceDiscoveredEvent(device: BTDeviceInfo): WSMessage {
    return {
      type: MessageType.Event,
      action: MessageAction.DeviceDiscovered,
      data: device,
      timestamp: Date.now(),
    };
  },

  createScanFinishedEvent(): WSMessage {
    return {
      type: MessageType.Event,
      action: MessageAction.ScanFinished,
      timestamp: Date.now(),
    };
  },

  createOBDDataEvent(sessionId: string, base64Data: string): WSMessage {
    return {
      type: MessageType.Event,
      action: MessageAction.OBDData,
      sessionId,
      data: base64Data,
      timestamp: Date.now(),
    };
  },

  createConnectionLostEvent(sessionId: string, reason: string): WSMessage {
    return {
      type: MessageType.Event,
      action: MessageAction.ConnectionLost,
      sessionId,
      data: { reason },
      timestamp: Date.now(),
    };
  },

  createErrorEvent(code: string, message: string): WSMessage {
    return {
      type: MessageType.Event,
      action: MessageAction.Error,
      data: { code, message },
      timestamp: Date.now(),
    };
  },
};
