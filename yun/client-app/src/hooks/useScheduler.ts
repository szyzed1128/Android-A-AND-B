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
 * 从 cloudHost 推导调度后端地址
 * 调度后端与 APK 共用同一台服务器，通过 Nginx /api/ 路径代理：
 *   cloudHost = "47.x.x.x" → http://47.x.x.x:8080/api
 *   cloudHost = "wss://domain.com" → http://domain.com:8080/api
 *
 * 对以下情况返回空字符串（不触发调度初始化）：
 *   - 默认本地地址 '192.168.1.100'
 *   - 输入中的不完整 IP（如 "47."、"47.1"）
 *   - 少于 4 个字符的任何输入
 */
export function deriveSchedulerUrl(cloudHost: string): string {
  const host = cloudHost.replace(/^wss?:\/\//i, '').replace(/\/.*$/, '').trim();

  // 默认值（空字符串）或太短，不处理
  if (!host || host.length < 4) return '';

  // 如果看起来像 IP 地址，要求格式完整（a.b.c.d，每段1-3位数字）
  const looksLikeIp = /^\d/.test(host);
  if (looksLikeIp) {
    const isCompleteIp = /^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/.test(host);
    if (!isCompleteIp) return ''; // 输入中的不完整 IP，等用户输完再触发
  }

  return `http://${host}:8080/api`;
}

/**
 * useScheduler - 完整 Hook，只在 App 根挂载一次
 * 负责会话初始化、心跳、AppState 监听
 * 同时返回操作方法供调用方便
 */
export function useScheduler() {
  const {
    setSessionId,
    setSelectedProfile,
    setSelectedDevice,
    setSchedulerReady,
    setSchedulerBaseUrl,
    setSchedulerInitializing,
    setSchedulerInitError,
    setSchedulerInitStage,
    setSchedulerInitAttemptAt,
    setSchedulerInitSuccessAt,
    setSchedulerDeviceId,
    setSchedulerHealthChecking,
    setSchedulerHealthMessage,
    setAssignedWsUrl,
    cloudHost,
    schedulerRefreshToken,
  } = useAppContext();
  const heartbeatTimerRef = useRef<ReturnType<typeof setInterval> | null>(null);
  const appStateRef = useRef<AppStateStatus>('active');
  const initializedRef = useRef(false);
  const initializingRef = useRef(false);

  const initSession = useCallback(async (force = false) => {
    if (initializingRef.current) return;
    if (!force && initializedRef.current) return;

    setSchedulerInitAttemptAt(Date.now());
    setSchedulerInitStage('准备调度地址');
    const url = deriveSchedulerUrl(cloudHost);
    setSchedulerBaseUrl(url);

    if (!url) {
      SchedulerClient.setBaseUrl('');
      SchedulerClient.setSessionId(null);
      setSessionId(null);
      setSchedulerReady(false);
      setAssignedWsUrl(null);
      setSchedulerInitializing(false);
      setSchedulerInitStage('等待输入完整 IP');
      initializedRef.current = false;
      const trimmedHost = cloudHost.trim();
      setSchedulerInitError(trimmedHost ? '请输入完整服务器 IP' : null);
      console.log('[useScheduler] 调度后端地址未设置或格式不完整，跳过初始化');
      return;
    }

    SchedulerClient.setBaseUrl(url);
    setSchedulerReady(false);
    setSchedulerInitializing(true);
    setSchedulerInitError(null);
    setSchedulerHealthMessage(null);
    setAssignedWsUrl(null);
    initializingRef.current = true;

    try {
      setSchedulerInitStage('准备 deviceId');
      const deviceId = await getOrCreateDeviceId();
      setSchedulerDeviceId(deviceId);
      setSchedulerInitStage('发送 session/init');
      const sessionId = await SchedulerClient.initSession(deviceId);
      setSessionId(sessionId);
      setSchedulerInitStage('恢复会话配置');
      const sessionState = await SchedulerClient.getSessionState();

      if (
        sessionState.carBrand &&
        typeof sessionState.profileIndex === 'number' &&
        sessionState.profileName
      ) {
        setSelectedProfile({
          brand: sessionState.carBrand,
          name: sessionState.profileName,
          profileIndex: sessionState.profileIndex,
        });
      } else {
        setSelectedProfile(null);
      }

      if (sessionState.btAddress && sessionState.btProtocol) {
        setSelectedDevice({
          name: sessionState.btName || sessionState.btAddress,
          address: sessionState.btAddress,
          protocol: sessionState.btProtocol,
        });
      } else {
        setSelectedDevice(null);
      }

      setSchedulerReady(true);
      setSchedulerInitError(null);
      setSchedulerInitSuccessAt(Date.now());
      setSchedulerInitStage('初始化成功');
      initializedRef.current = true;
      console.log('[useScheduler] 会话初始化成功 sessionId=', sessionId);
    } catch (err: any) {
      SchedulerClient.setSessionId(null);
      setSessionId(null);
      setSchedulerReady(false);
      setSchedulerInitError(err.message || '调度初始化失败');
      setSchedulerInitStage('初始化失败');
      initializedRef.current = false;
      // 调度后端暂不可达（非致命），用 log 不用 warn/error，避免开发模式弹窗。
      // 不自动重试：等待两个时机自然触发 —— ① cloudHost 变化 ② App 回到前台
      console.log('[useScheduler] 调度后端暂不可达，等待重试时机:', err.message);
    } finally {
      setSchedulerInitializing(false);
      initializingRef.current = false;
    }
  }, [
    cloudHost,
    setAssignedWsUrl,
    setSchedulerBaseUrl,
    setSchedulerInitError,
    setSchedulerInitAttemptAt,
    setSchedulerInitStage,
    setSchedulerInitializing,
    setSchedulerInitSuccessAt,
    setSchedulerReady,
    setSchedulerDeviceId,
    setSchedulerHealthMessage,
    setSelectedDevice,
    setSelectedProfile,
    setSessionId,
  ]);

  const checkSchedulerHealth = useCallback(async () => {
    const url = deriveSchedulerUrl(cloudHost);
    setSchedulerBaseUrl(url);

    if (!url) {
      setSchedulerHealthMessage('请输入完整服务器 IP');
      return;
    }

    SchedulerClient.setBaseUrl(url);
    setSchedulerHealthChecking(true);
    setSchedulerHealthMessage(null);
    try {
      const health = await SchedulerClient.checkHealth();
      setSchedulerHealthMessage(`健康检查成功：status=${health.status} redis=${health.redis || '-'}`);
    } catch (err: any) {
      setSchedulerHealthMessage(`健康检查失败：${err.message || '未知错误'}`);
    } finally {
      setSchedulerHealthChecking(false);
    }
  }, [
    cloudHost,
    setSchedulerBaseUrl,
    setSchedulerHealthChecking,
    setSchedulerHealthMessage,
  ]);

  // 监听 cloudHost 变化，自动更新 SchedulerClient 的 baseUrl
  // 加 800ms 防抖：用户在输入框逐字输入时不触发，停止输入后才执行
  // 这样 "47."、"47.1" 等中间状态不会触发网络请求
  useEffect(() => {
    const timer = setTimeout(() => {
      const url = deriveSchedulerUrl(cloudHost);
      setSchedulerBaseUrl(url);
      if (url) {
        console.log('[useScheduler] 调度后端地址已更新:', url);
      }
      initializedRef.current = false;
      initSession(true);
    }, 800); // 800ms 防抖：用户停止输入后再触发

    return () => clearTimeout(timer);
  }, [cloudHost, initSession, setSchedulerBaseUrl]);

  useEffect(() => {
    if (schedulerRefreshToken <= 0) return;
    initializedRef.current = false;
    initSession(true);
  }, [initSession, schedulerRefreshToken]);

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
        // 若会话尚未初始化成功（例如之前服务器不可达），趁此机会重试
        if (!initializedRef.current) {
          initSession(true);
        }
      } else if (nextState === 'background') {
        console.log('[useScheduler] App 进入后台');
      }
    });
    return () => subscription.remove();
  }, [startHeartbeat, initSession]);

  useEffect(() => {
    initSession(true);
    startHeartbeat();
    return () => { stopHeartbeat(); };
  }, [initSession, startHeartbeat, stopHeartbeat]);

  // 返回操作方法（各页面可通过此 hook 调用，但不会重复启动生命周期）
  return {
    syncDevice: useCallback(async (btAddress: string, btProtocol: string, btName?: string) => {
      try {
        await SchedulerClient.setDevice(btAddress, btProtocol, btName);
      } catch (err: any) {
        console.log('[Scheduler] syncDevice 跳过（调度未就绪）:', err.message);
      }
    }, []),

    syncCar: useCallback(async (carBrand: string, profileIndex: number, profileName: string) => {
      try {
        await SchedulerClient.setCar(carBrand, profileIndex, profileName);
      } catch (err: any) {
        console.log('[Scheduler] syncCar 跳过（调度未就绪）:', err.message);
      }
    }, []),

    reserveInstance: useCallback(async (): Promise<string> => {
      const result = await SchedulerClient.reserve();
      console.log('[Scheduler] 实例已分配 wsUrl=', result.wsUrl);
      return result.wsUrl;
    }, []),

    getCatalogBrands: useCallback(async (): Promise<string[]> => {
      return SchedulerClient.getCatalogBrands();
    }, []),

    getCatalogProfiles: useCallback(async (brand: string) => {
      return SchedulerClient.getCatalogProfiles(brand);
    }, []),

    notifyDisconnect: useCallback(async () => {
      await SchedulerClient.disconnect();
    }, []),

    releasePreparedInstance: useCallback(async () => {
      await SchedulerClient.release();
    }, []),

    notifyRunning: useCallback(async () => {
      await SchedulerClient.updateStatus('running');
    }, []),

    checkSchedulerHealth,
  };
}

/**
 * useSchedulerActions - 轻量版，仅返回操作方法
 * 供所有子页面（HomePage、VehicleConfigPage、BluetoothPage）使用
 * 不启动任何定时器或生命周期监听（避免多实例问题）
 * 只有 App.tsx 的 BluetoothBridgeBootstrap 调用完整的 useScheduler()
 */
export function useSchedulerActions() {
  const {
    cloudHost,
    setSchedulerBaseUrl,
    setSchedulerHealthChecking,
    setSchedulerHealthMessage,
  } = useAppContext();

  const checkSchedulerHealth = async () => {
    const url = deriveSchedulerUrl(cloudHost);
    setSchedulerBaseUrl(url);

    if (!url) {
      setSchedulerHealthMessage('请输入完整服务器 IP');
      return;
    }

    SchedulerClient.setBaseUrl(url);
    setSchedulerHealthChecking(true);
    setSchedulerHealthMessage(null);
    try {
      const health = await SchedulerClient.checkHealth();
      setSchedulerHealthMessage(`健康检查成功：status=${health.status} redis=${health.redis || '-'}`);
    } catch (err: any) {
      setSchedulerHealthMessage(`健康检查失败：${err.message || '未知错误'}`);
    } finally {
      setSchedulerHealthChecking(false);
    }
  };

  return {
    syncDevice: async (btAddress: string, btProtocol: string, btName?: string) => {
      try {
        await SchedulerClient.setDevice(btAddress, btProtocol, btName);
      } catch (err: any) {
        console.log('[Scheduler] syncDevice 跳过（调度未就绪）:', err.message);
      }
    },

    syncCar: async (carBrand: string, profileIndex: number, profileName: string) => {
      try {
        await SchedulerClient.setCar(carBrand, profileIndex, profileName);
      } catch (err: any) {
        console.log('[Scheduler] syncCar 跳过（调度未就绪）:', err.message);
      }
    },

    reserveInstance: async (): Promise<string> => {
      const result = await SchedulerClient.reserve();
      console.log('[Scheduler] 实例已分配 wsUrl=', result.wsUrl);
      return result.wsUrl;
    },

    cancelReserve: async () => {
      await SchedulerClient.cancelReserve();
    },

    getCatalogBrands: async (): Promise<string[]> => {
      return SchedulerClient.getCatalogBrands();
    },

    getCatalogProfiles: async (brand: string) => {
      return SchedulerClient.getCatalogProfiles(brand);
    },

    notifyDisconnect: async () => {
      await SchedulerClient.disconnect();
    },

    releasePreparedInstance: async () => {
      await SchedulerClient.release();
    },

    notifyRunning: async () => {
      await SchedulerClient.updateStatus('running');
    },

    checkSchedulerHealth,
  };
}
