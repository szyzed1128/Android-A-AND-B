import React, { createContext, useContext, useState, ReactNode } from 'react';

type SelectedProfile = { brand: string; name: string } | null;
type SelectedDevice = { name: string; address: string } | null;

type AppContextValue = {
  selectedProfile: SelectedProfile;
  setSelectedProfile: React.Dispatch<React.SetStateAction<SelectedProfile>>;
  selectedDevice: SelectedDevice;
  setSelectedDevice: React.Dispatch<React.SetStateAction<SelectedDevice>>;
  connectionStatus: string;
  setConnectionStatus: React.Dispatch<React.SetStateAction<string>>;
};

export const AppContext = createContext<AppContextValue | undefined>(undefined);

export const AppProvider = ({ children }: { children: ReactNode }) => {
  const [selectedProfile, setSelectedProfile] = useState<SelectedProfile>(null);
  const [selectedDevice, setSelectedDevice] = useState<SelectedDevice>(null);
  const [connectionStatus, setConnectionStatus] = useState<string>('Disconnected');

  return (
    <AppContext.Provider
      value={{ 
        selectedProfile, 
        setSelectedProfile, 
        selectedDevice, 
        setSelectedDevice,
        connectionStatus,
        setConnectionStatus
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
