import React, { createContext, useContext, useState, useCallback, ReactNode } from 'react';

type SelectedProfile = { brand: string; name: string } | null;
type SelectedDevice = { name: string; address: string } | null;
export type ScannedDevice = { name: string; address: string; valid?: boolean; paired?: boolean };

type AppContextValue = {
  selectedProfile: SelectedProfile;
  setSelectedProfile: React.Dispatch<React.SetStateAction<SelectedProfile>>;
  selectedDevice: SelectedDevice;
  setSelectedDevice: React.Dispatch<React.SetStateAction<SelectedDevice>>;
  connectionStatus: string;
  setConnectionStatus: React.Dispatch<React.SetStateAction<string>>;
  // 蓝牙扫描相关
  scannedDevices: ScannedDevice[];
  setScannedDevices: React.Dispatch<React.SetStateAction<ScannedDevice[]>>;
  isScanning: boolean;
  setIsScanning: React.Dispatch<React.SetStateAction<boolean>>;
  clearScannedDevices: () => void;
};

export const AppContext = createContext<AppContextValue | undefined>(undefined);

export const AppProvider = ({ children }: { children: ReactNode }) => {
  const [selectedProfile, setSelectedProfile] = useState<SelectedProfile>(null);
  const [selectedDevice, setSelectedDevice] = useState<SelectedDevice>(null);
  const [connectionStatus, setConnectionStatus] = useState<string>('Disconnected');
  // 蓝牙扫描相关状态
  const [scannedDevices, setScannedDevices] = useState<ScannedDevice[]>([]);
  const [isScanning, setIsScanning] = useState<boolean>(false);
  const clearScannedDevices = useCallback(() => setScannedDevices([]), []);

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
        clearScannedDevices
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
