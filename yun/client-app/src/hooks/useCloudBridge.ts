/**
 * useCloudBridge - 云端通信 Hook
 *
 * 提供与云端 A 通信的便捷方法
 */

import { useCallback, useEffect } from 'react';
import { useAppContext } from '../context/AppContext';
import CloudBridge, {
  Profile,
  ECUItem,
  PIDItem,
  DTCCallbacks,
  ClearDTCCallbacks,
  ECUInfoCallbacks,
  FreezeFrameCallbacks,
} from '../services/CloudBridge';

export function useCloudBridge() {
  const { setConnectionStatus, setCloudConnected, cloudHost, cloudPort } = useAppContext();

  // 设置状态变化回调
  useEffect(() => {
    const unsubscribeConnection = CloudBridge.addConnectionListener((connected) => {
      setCloudConnected(connected);
    });
    const unsubscribeObd = CloudBridge.addOBDStatusListener((status) => {
      console.log(`[useCloudBridge] OBD 状态回调触发: "${status}"`);
      setConnectionStatus(status as any);
    });

    return () => {
      unsubscribeConnection();
      unsubscribeObd();
    };
  }, [setCloudConnected, setConnectionStatus]);

  // 连接云端
  const connectCloud = useCallback(async () => {
    try {
      await CloudBridge.connect(cloudHost, cloudPort);
      return true;
    } catch (e) {
      console.error('[useCloudBridge] Connect failed:', e);
      return false;
    }
  }, [cloudHost, cloudPort]);

  // 断开云端
  const disconnectCloud = useCallback(() => {
    CloudBridge.disconnect();
  }, []);

  // 安全调用方法
  const safeCall = useCallback(async <T>(
    method: () => Promise<T>,
    defaultValue: T
  ): Promise<T> => {
    if (!CloudBridge.getIsConnected()) {
      console.warn('[useCloudBridge] Not connected');
      return defaultValue;
    }
    try {
      return await method();
    } catch (e) {
      console.error('[useCloudBridge] Call failed:', e);
      return defaultValue;
    }
  }, []);

  // 车辆配置
  const getBrands = useCallback(() => safeCall(() => CloudBridge.getBrands(), []), [safeCall]);

  const getProfiles = useCallback(
    (brand: string) => safeCall(() => CloudBridge.getProfiles(brand), []),
    [safeCall]
  );

  const applyProfile = useCallback(
    (brand: string, index: number) => safeCall(() => CloudBridge.applyProfile(brand, index), undefined),
    [safeCall]
  );

  // ECU
  const getECUList = useCallback(() => safeCall(() => CloudBridge.getECUList(), []), [safeCall]);

  const readECUInfoAsync = useCallback((indices: number[], callbacks: ECUInfoCallbacks) => {
    CloudBridge.readECUInfoAsync(indices, callbacks);
  }, []);

  // 故障码
  const readDTCAsync = useCallback((indices: number[], callbacks: DTCCallbacks) => {
    CloudBridge.readDTCAsync(indices, callbacks);
  }, []);

  const clearDTCAsync = useCallback((indices: number[], callbacks: ClearDTCCallbacks) => {
    CloudBridge.clearDTCAsync(indices, callbacks);
  }, []);

  // 冻结帧
  const readFreezeFrameAsync = useCallback((frameIndex: number, callbacks: FreezeFrameCallbacks) => {
    CloudBridge.readFreezeFrameAsync(frameIndex, callbacks);
  }, []);

  // PID
  const getPIDList = useCallback(() => safeCall(() => CloudBridge.getPIDList(), []), [safeCall]);

  const startReadPIDs = useCallback((indices: number[]) => {
    CloudBridge.startReadPIDs(indices);
  }, []);

  const stopReadPIDs = useCallback(async () => {
    await CloudBridge.stopReadPIDs();
  }, []);

  // OBD 连接
  const connectOBD = useCallback(
    (protocol: string, address: string, sessionId?: string) =>
      safeCall(() => CloudBridge.connectOBD(protocol, address, sessionId), undefined),
    [safeCall]
  );

  const disconnectOBD = useCallback(
    () => safeCall(() => CloudBridge.disconnectOBD(), undefined),
    [safeCall]
  );

  // 清除回调
  const clearCallbacks = useCallback((key: string) => {
    CloudBridge.clearCallbacks(key);
  }, []);

  return {
    // 连接状态
    isConnected: CloudBridge.getIsConnected(),

    // 云端连接
    connectCloud,
    disconnectCloud,

    // 车辆配置
    getBrands,
    getProfiles,
    applyProfile,

    // ECU
    getECUList,
    readECUInfoAsync,

    // 故障码
    readDTCAsync,
    clearDTCAsync,

    // 冻结帧
    readFreezeFrameAsync,

    // PID
    getPIDList,
    startReadPIDs,
    stopReadPIDs,

    // OBD 连接
    connectOBD,
    disconnectOBD,

    // 工具
    clearCallbacks,

    // PID 值变化回调订阅
    onPIDValueChanged: (callback: (data: PIDItem) => void) => {
      CloudBridge.onPIDValueChanged = callback;
      // 返回取消订阅函数
      return () => {
        CloudBridge.onPIDValueChanged = undefined;
      };
    },
  };
}

export default useCloudBridge;
