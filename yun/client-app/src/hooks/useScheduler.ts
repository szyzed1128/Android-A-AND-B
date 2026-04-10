/**
 * useScheduler - 调度会话生命周期管理
 *
 * 设计说明：
 * - useScheduler() 完整 Hook 只应在 App 根组件（BluetoothBridgeBootstrap）挂载一次
 *   负责：会话初始化、心跳定时器、AppState 生命周期
 *
 * - 子页面（VehicleConfigPage、BluetoothPage）不调用 useScheduler()
 *   而是直接调用 SchedulerClient 的方法，或者使用下方导出的独立函数
 */

import { useEffect, useRef, useCallback } from 'react';
import { AppState, AppStateStatus } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import SchedulerClient from '../services/SchedulerClient';
import { useAppContext } from '../context/AppContext';

const HEARTBEAT_INTERVAL_MS = 30 * 1000;
const DEVICE_ID_KEY = '@obd_device_id';

/**
 * 获取或生成持久化 deviceId（不依赖原生模块）
 */
async function getOrCreateDeviceId(): Promise<string> {
  try {
    const stored = await AsyncStorage.getItem(DEVICE_ID_KEY);
    if (stored) return stored;
    const newId = `device_${Date.now()}_${Math.random().toString(36).slice(2, 10)}`;
    await AsyncStorage.setItem(DEVICE_ID_KEY, newId);
    return newId;
  } catch {
    return `device_temp_${Date.now()}`;
  }
}

/**
 * useScheduler - 完整 Hook，只在 App 根挂载一次
 * 负责会话初始化、心跳、AppState 监听
 * 同时返回操作方法供调用方便
 */
export function useScheduler() {
  const { setSessionId, setSchedulerReady } = useAppContext();
  const heartbeatTimerRef = useRef<ReturnType<typeof setInterval> | null>(null);
  const appStateRef = useRef<AppStateStatus>('active');
  const initializedRef = useRef(false);

  const initSession = useCallback(async () => {
    if (initializedRef.current) return;
    try {
      const deviceId = await getOrCreateDeviceId();
      const sessionId = await SchedulerClient.initSession(deviceId);
      setSessionId(sessionId);
      setSchedulerReady(true);
      initializedRef.current = true;
      console.log('[useScheduler] 会话初始化成功 sessionId=', sessionId);
    } catch (err: any) {
      console.error('[useScheduler] 会话初始化失败:', err.message);
      setTimeout(initSession, 5000);
    }
  }, [setSessionId, setSchedulerReady]);

  const startHeartbeat = useCallback(() => {
    if (heartbeatTimerRef.current) return;
    heartbeatTimerRef.current = setInterval(async () => {
      await SchedulerClient.heartbeat();
    }, HEARTBEAT_INTERVAL_MS);
    console.log('[useScheduler] 心跳已启动（每30秒）');
  }, []);

  const stopHeartbeat = useCallback(() => {
    if (heartbeatTimerRef.current) {
      clearInterval(heartbeatTimerRef.current);
      heartbeatTimerRef.current = null;
    }
  }, []);

  useEffect(() => {
    const subscription = AppState.addEventListener('change', (nextState: AppStateStatus) => {
      const prev = appStateRef.current;
      appStateRef.current = nextState;
      if (nextState === 'active' && prev !== 'active') {
        console.log('[useScheduler] App 回到前台，恢复心跳');
        startHeartbeat();
      } else if (nextState === 'background') {
        console.log('[useScheduler] App 进入后台');
      }
    });
    return () => subscription.remove();
  }, [startHeartbeat]);

  useEffect(() => {
    initSession();
    startHeartbeat();
    return () => { stopHeartbeat(); };
  }, [initSession, startHeartbeat, stopHeartbeat]);

  // 返回操作方法（各页面可通过此 hook 调用，但不会重复启动生命周期）
  return {
    syncDevice: useCallback(async (btAddress: string, btProtocol: string, btName?: string) => {
      try {
        await SchedulerClient.setDevice(btAddress, btProtocol, btName);
      } catch (err: any) {
        console.warn('[Scheduler] syncDevice 失败:', err.message);
      }
    }, []),

    syncCar: useCallback(async (carBrand: string, carModel: string) => {
      try {
        await SchedulerClient.setCar(carBrand, carModel);
      } catch (err: any) {
        console.warn('[Scheduler] syncCar 失败:', err.message);
      }
    }, []),

    reserveInstance: useCallback(async (): Promise<string> => {
      const result = await SchedulerClient.reserve();
      console.log('[Scheduler] 实例已分配 wsUrl=', result.wsUrl);
      return result.wsUrl;
    }, []),

    notifyDisconnect: useCallback(async () => {
      await SchedulerClient.disconnect();
    }, []),

    notifyRunning: useCallback(async () => {
      await SchedulerClient.updateStatus('running');
    }, []),
  };
}

/**
 * useSchedulerActions - 轻量版，仅返回操作方法
 * 供所有子页面（HomePage、VehicleConfigPage、BluetoothPage）使用
 * 不启动任何定时器或生命周期监听（避免多实例问题）
 * 只有 App.tsx 的 BluetoothBridgeBootstrap 调用完整的 useScheduler()
 */
export function useSchedulerActions() {
  return {
    syncDevice: async (btAddress: string, btProtocol: string, btName?: string) => {
      try {
        await SchedulerClient.setDevice(btAddress, btProtocol, btName);
      } catch (err: any) {
        console.warn('[Scheduler] syncDevice 失败:', err.message);
      }
    },

    syncCar: async (carBrand: string, carModel: string) => {
      try {
        await SchedulerClient.setCar(carBrand, carModel);
      } catch (err: any) {
        console.warn('[Scheduler] syncCar 失败:', err.message);
      }
    },

    reserveInstance: async (): Promise<string> => {
      const result = await SchedulerClient.reserve();
      console.log('[Scheduler] 实例已分配 wsUrl=', result.wsUrl);
      return result.wsUrl;
    },

    notifyDisconnect: async () => {
      await SchedulerClient.disconnect();
    },

    notifyRunning: async () => {
      await SchedulerClient.updateStatus('running');
    },
  };
}
