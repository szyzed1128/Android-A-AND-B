import React, { createContext, useContext, useState, useCallback, ReactNode } from 'react';

// 类型定义
type SelectedProfile = { brand: string; name: string } | null;
type SelectedDevice = { name: string; address: string; protocol: 'ble' | 'classic' | 'mfi' } | null;
export type ScannedDevice = {
  name: string;
  address: string;
  valid?: boolean;
  paired?: boolean;
  protocol: 'ble' | 'classic' | 'mfi';
  rssi?: number;
};

// 连接状态类型
export type ConnectionStatus =
  | 'Disconnected'
  | 'ConnectingToELM'
  | 'ConnectedToELM'
  | 'ConnectingToECU'
  | 'ConnectedToECU';

// Context 值类型
type AppContextValue = {
  // 车辆配置
  selectedProfile: SelectedProfile;
  setSelectedProfile: React.Dispatch<React.SetStateAction<SelectedProfile>>;

  // 蓝牙设备
  selectedDevice: SelectedDevice;
  setSelectedDevice: React.Dispatch<React.SetStateAction<SelectedDevice>>;

  // OBD 连接状态
  connectionStatus: ConnectionStatus;
  setConnectionStatus: React.Dispatch<React.SetStateAction<ConnectionStatus>>;

  // 蓝牙扫描
  scannedDevices: ScannedDevice[];
  setScannedDevices: React.Dispatch<React.SetStateAction<ScannedDevice[]>>;
  isScanning: boolean;
  setIsScanning: React.Dispatch<React.SetStateAction<boolean>>;
  clearScannedDevices: () => void;
  addScannedDevice: (device: ScannedDevice) => void;

  // 云端连接状态
  cloudConnected: boolean;
  setCloudConnected: React.Dispatch<React.SetStateAction<boolean>>;
  cloudHost: string;
  setCloudHost: React.Dispatch<React.SetStateAction<string>>;
  cloudPort: number;
  setCloudPort: React.Dispatch<React.SetStateAction<number>>;

  // 后端初始化状态
  isBackendReady: boolean;
  setIsBackendReady: React.Dispatch<React.SetStateAction<boolean>>;

  // App 启动时间
  appStartTime: number;

  // ELM327 超时覆盖（ms）：400=默认不拦截，800/1000/1020=覆盖 ATST 命令
  elmTimeoutMs: number;
  setElmTimeoutMs: React.Dispatch<React.SetStateAction<number>>;
};

export const AppContext = createContext<AppContextValue | undefined>(undefined);

export const AppProvider = ({ children }: { children: ReactNode }) => {
  // 车辆配置
  const [selectedProfile, setSelectedProfile] = useState<SelectedProfile>(null);

  // 蓝牙设备
  const [selectedDevice, setSelectedDevice] = useState<SelectedDevice>(null);

  // OBD 连接状态
  const [connectionStatus, setConnectionStatus] = useState<ConnectionStatus>('Disconnected');

  // 蓝牙扫描
  const [scannedDevices, setScannedDevices] = useState<ScannedDevice[]>([]);
  const [isScanning, setIsScanning] = useState<boolean>(false);

  const clearScannedDevices = useCallback(() => {
    setScannedDevices([]);
  }, []);

  const addScannedDevice = useCallback((device: ScannedDevice) => {
    setScannedDevices(prev => {
      // 去重：如果已存在相同地址的设备，更新信息
      const existing = prev.findIndex(d => d.address === device.address);
      if (existing >= 0) {
        const updated = [...prev];
        // 只有当新设备有名称或名称更好时才更新
        if (device.name && !device.name.startsWith('BLE-')) {
          updated[existing] = device;
        }
        return updated;
      }
      return [...prev, device];
    });
  }, []);

  // 云端连接
  const [cloudConnected, setCloudConnected] = useState<boolean>(false);
  const [cloudHost, setCloudHost] = useState<string>('192.168.1.100');
  const [cloudPort, setCloudPort] = useState<number>(8080);

  // 后端初始化状态
  const [isBackendReady, setIsBackendReady] = useState<boolean>(false);

  // App 启动时间
  const [appStartTime] = useState<number>(() => Date.now());

  // ELM327 ATST 超时覆盖（ms），400=默认不拦截
  const [elmTimeoutMs, setElmTimeoutMs] = useState<number>(400);

  return (
    <AppContext.Provider
      value={{
        selectedProfile,
        setSelectedProfile,
        selectedDevice,
        setSelectedDevice,
        connectionStatus,
        setConnectionStatus,
        scannedDevices,
        setScannedDevices,
        isScanning,
        setIsScanning,
        clearScannedDevices,
        addScannedDevice,
        cloudConnected,
        setCloudConnected,
        cloudHost,
        setCloudHost,
        cloudPort,
        setCloudPort,
        isBackendReady,
        setIsBackendReady,
        appStartTime,
        elmTimeoutMs,
        setElmTimeoutMs,
      }}
    >
      {children}
    </AppContext.Provider>
  );
};

export const useAppContext = () => {
  const context = useContext(AppContext);
  if (!context) {
    throw new Error('useAppContext must be used within an AppProvider');
  }
  return context;
};
