/**
 * useLocalBluetooth - 本地蓝牙 Hook
 *
 * 封装本地蓝牙扫描功能，使用现有的 BluetoothManager
 */

import { useCallback, useEffect, useRef } from 'react';
import { useAppContext, ScannedDevice } from '../context/AppContext';
import { BluetoothGateway, BluetoothProtocol, ExtendedBTDeviceInfo } from '../bluetooth/BluetoothManager';

// 蓝牙网关单例 - 确保所有 useLocalBluetooth 调用共享同一个实例
let bluetoothGateway: BluetoothGateway | null = null;
let gatewayRefCount = 0; // 引用计数，最后一个卸载时销毁
let isScanningShared = false; // 模块级扫描状态，所有实例共享，防止多实例 stale closure
let pendingDevicesBuffer: ScannedDevice[] = []; // 待批量刷新的设备缓冲
let flushIntervalId: ReturnType<typeof setInterval> | null = null; // 刷新定时器

export function useLocalBluetooth() {
  const {
    scannedDevices,
    setScannedDevices,
    isScanning,
    setIsScanning,
    clearScannedDevices,
    addScannedDevice,
    selectedDevice,
    setSelectedDevice,
  } = useAppContext();

  const gatewayRef = useRef<BluetoothGateway | null>(null);
  // 扫描结束回调 ref，供网关单例访问（绕过 closure 陷阱）
  const onScanFinishedRef = useRef<() => void>(() => {});
  // 每次渲染时更新回调，确保能访问到最新的 setIsScanning
  onScanFinishedRef.current = () => {
    setIsScanning(false);
  };

  // 批量刷新函数 ref，供定时器和 onScanFinished 访问（绕过 closure 陷阱）
  const flushDevicesRef = useRef<() => void>(() => {});
  // 每次渲染时更新，确保总是使用最新的 setScannedDevices
  flushDevicesRef.current = () => {
    const pending = pendingDevicesBuffer.splice(0);
    if (!pending.length) return;
    // 单次 setScannedDevices 调用合并所有待更新设备（与 addScannedDevice 去重逻辑一致）
    setScannedDevices(prev => {
      const next = [...prev];
      for (const device of pending) {
        const idx = next.findIndex(d => d.address === device.address);
        if (idx >= 0) {
          // 仅当名称更好时才覆盖（与原 addScannedDevice 逻辑一致）
          if (device.name && !device.name.startsWith('BLE-')) next[idx] = device;
        } else {
          next.push(device);
        }
      }
      // BLE 开头的设备排到最后，非 BLE 设备保持原有相对顺序
      next.sort((a, b) => {
        const aIsBLE = (a.name ?? '').startsWith('BLE');
        const bIsBLE = (b.name ?? '').startsWith('BLE');
        if (aIsBLE === bIsBLE) return 0;
        return aIsBLE ? 1 : -1;
      });
      return next;
    });
  };

  // 初始化蓝牙网关（单例模式）
  useEffect(() => {
    if (!bluetoothGateway) {
      console.log('[LocalBluetooth] 创建蓝牙网关单例');
      bluetoothGateway = new BluetoothGateway({
        onDeviceDiscovered: (device: ExtendedBTDeviceInfo) => {
          // 不再立即触发 state update，推入缓冲等待批量刷新
          pendingDevicesBuffer.push({
            name: device.name,
            address: device.address,
            valid: device.valid,
            paired: device.paired,
            protocol: device.protocol,
            rssi: device.rssi,
          });
        },
        onScanFinished: () => {
          // guard：多个 adapter 都会触发 onScanFinished（BLE x2 + Classic + MFi = 4次），只处理第一次
          if (!isScanningShared) return;
          isScanningShared = false;
          // 停止刷新定时器，做最终一次批量刷新，确保扫描结束前发现的设备全部写入
          if (flushIntervalId) { clearInterval(flushIntervalId); flushIntervalId = null; }
          flushDevicesRef.current();
          onScanFinishedRef.current();
        },
        onError: (code, message) => {
          console.error('[LocalBluetooth] Error:', code, message);
        },
      });

      // 初始化
      bluetoothGateway.initialize().then((result) => {
        console.log('[LocalBluetooth] Initialized:', result);
      });
    }

    gatewayRef.current = bluetoothGateway;
    gatewayRefCount++;
    console.log('[LocalBluetooth] Hook 挂载, 引用计数:', gatewayRefCount);

    return () => {
      gatewayRefCount--;
      console.log('[LocalBluetooth] Hook 卸载, 引用计数:', gatewayRefCount);
      if (gatewayRefCount <= 0 && bluetoothGateway) {
        if (bluetoothGateway.hasActiveSession()) {
          console.log('[LocalBluetooth] 存在活动连接，保持蓝牙网关实例');
          gatewayRefCount = 0;
        } else {
          console.log('[LocalBluetooth] 销毁蓝牙网关单例');
          bluetoothGateway.destroy();
          bluetoothGateway = null;
          gatewayRefCount = 0;
        }
      }
      gatewayRef.current = null;
    };
  }, [setIsScanning]);

  // 开始扫描
  const startScan = useCallback(async (protocols?: BluetoothProtocol[]) => {
    if (!gatewayRef.current) {
      console.warn('[LocalBluetooth] Gateway not initialized');
      return;
    }
    // 用模块级变量，所有实例共享，避免多实例 stale closure 导致守卫失效
    if (isScanningShared) {
      console.log('[LocalBluetooth] 已在扫描中，忽略重复请求');
      return;
    }

    isScanningShared = true;
    pendingDevicesBuffer = []; // 清空旧缓冲，避免上次扫描残留
    clearScannedDevices();
    setIsScanning(true);

    // 启动批量刷新定时器（每 300ms 将缓冲的设备批量写入 state）
    if (flushIntervalId) clearInterval(flushIntervalId);
    flushIntervalId = setInterval(() => {
      flushDevicesRef.current();
    }, 300);

    try {
      await gatewayRef.current.startScan(protocols);
    } catch (e) {
      console.error('[LocalBluetooth] Scan error:', e);
      isScanningShared = false;
      if (flushIntervalId) { clearInterval(flushIntervalId); flushIntervalId = null; }
      setIsScanning(false);
    }
  }, [clearScannedDevices, setIsScanning]);

  // 停止扫描
  const stopScan = useCallback(async () => {
    if (!gatewayRef.current) return;

    isScanningShared = false;
    // 停止刷新定时器，做最终一次批量刷新
    if (flushIntervalId) { clearInterval(flushIntervalId); flushIntervalId = null; }
    flushDevicesRef.current();
    try {
      await gatewayRef.current.stopScan();
    } catch (e) {
      console.error('[LocalBluetooth] Stop scan error:', e);
    }
    setIsScanning(false);
  }, [setIsScanning]);

  // 选择设备
  const selectDevice = useCallback((device: ScannedDevice) => {
    setSelectedDevice({
      name: device.name,
      address: device.address,
      protocol: device.protocol,
    });
  }, [setSelectedDevice]);

  // 连接设备（快速返回，不阻塞等待 ATZ 验证）
  const connectDevice = useCallback(async (
    protocol: 'ble' | 'classic' | 'mfi',
    address: string
  ): Promise<{ success: boolean; sessionId?: string; error?: string }> => {
    if (!gatewayRef.current) {
      return { success: false, error: '蓝牙未初始化' };
    }

    try {
      console.log('[LocalBluetooth] ========================================');
      console.log('[LocalBluetooth] 开始连接设备:', protocol, address);

      const sessionId = await gatewayRef.current.connect(protocol, address);
      console.log('[LocalBluetooth] 蓝牙连接成功, sessionId:', sessionId);
      console.log('[LocalBluetooth] ========================================');

      // 立即返回成功，不阻塞等待 ATZ 验证
      // ATZ 初始化将由 A 端（云端）自己发送和处理
      return { success: true, sessionId };
    } catch (e: any) {
      console.error('[LocalBluetooth] 连接失败:', e);
      console.log('[LocalBluetooth] ========================================');
      return { success: false, error: e.message || '连接失败' };
    }
  }, []);

  // 断开设备
  const disconnectDevice = useCallback(async (): Promise<void> => {
    if (!gatewayRef.current) return;
    try {
      await gatewayRef.current.disconnect();
    } catch (e) {
      console.error('[LocalBluetooth] Disconnect error:', e);
    }
  }, []);

  // 发送数据到 ELM327 设备（供蓝牙桥接使用）
  const sendData = useCallback(async (base64Data: string): Promise<void> => {
    if (!gatewayRef.current) {
      throw new Error('蓝牙未初始化');
    }
    await gatewayRef.current.send(base64Data);
  }, []);

  // 检查设备是否为 ELM327 设备
  const isOBDDevice = useCallback((name: string): boolean => {
    if (!name) return false;
    const upper = name.toUpperCase();
    const keywords = ['OBD', 'ELM', 'VLINK', 'VGATE', 'SCAN', 'CAR', 'DIAG'];
    return keywords.some(k => upper.includes(k));
  }, []);

  // 获取设备列表（按协议过滤，保持发现顺序）
  const getFilteredDevices = useCallback((protocol?: 'ble' | 'classic' | 'mfi'): ScannedDevice[] => {
    if (!protocol) {
      return scannedDevices;
    }
    return scannedDevices.filter(d => d.protocol === protocol);
  }, [scannedDevices]);

  return {
    // 状态
    scannedDevices,
    getFilteredDevices,
    isScanning,
    selectedDevice,

    // 操作
    startScan,
    stopScan,
    selectDevice,
    clearScannedDevices,
    connectDevice,
    disconnectDevice,
    sendData,

    // 工具
    isOBDDevice,
  };
}

// 导出蓝牙网关实例（供其他组件使用）
export function getBluetoothGateway(): BluetoothGateway | null {
  return bluetoothGateway;
}

export default useLocalBluetooth;
