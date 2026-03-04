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

  // 初始化蓝牙网关（单例模式）
  useEffect(() => {
    if (!bluetoothGateway) {
      console.log('[LocalBluetooth] 创建蓝牙网关单例');
      bluetoothGateway = new BluetoothGateway({
        onDeviceDiscovered: (device: ExtendedBTDeviceInfo) => {
          const scannedDevice: ScannedDevice = {
            name: device.name,
            address: device.address,
            valid: device.valid,
            paired: device.paired,
            protocol: device.protocol,
            rssi: device.rssi,
          };
          addScannedDevice(scannedDevice);
        },
        onScanFinished: () => {
          setIsScanning(false);
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
  }, [addScannedDevice, setIsScanning]);

  // 开始扫描
  const startScan = useCallback(async (protocols?: BluetoothProtocol[]) => {
    if (!gatewayRef.current) {
      console.warn('[LocalBluetooth] Gateway not initialized');
      return;
    }
    if (isScanning) {
      console.log('[LocalBluetooth] 已在扫描中，忽略重复请求');
      return;
    }

    clearScannedDevices();
    setIsScanning(true);

    try {
      await gatewayRef.current.startScan(protocols);
    } catch (e) {
      console.error('[LocalBluetooth] Scan error:', e);
      setIsScanning(false);
    }
  }, [clearScannedDevices, setIsScanning]);

  // 停止扫描
  const stopScan = useCallback(async () => {
    if (!gatewayRef.current) return;

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
