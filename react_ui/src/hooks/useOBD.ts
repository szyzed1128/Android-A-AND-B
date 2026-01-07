import { useEffect, useState, useCallback, useContext } from 'react';
import { AppContext } from './AppContext';

export const useOBD = () => {
  const context = useContext(AppContext);
  const status = context?.connectionStatus || 'Disconnected';
  const [pidData, setPidData] = useState<any>(null);

  useEffect(() => {
    // Only listen for PID data here. Status is handled globally in App.tsx
    (window as any).onPIDValueChanged = (data: any) => {
      if (typeof data === 'string') {
        try {
          setPidData(JSON.parse(data));
        } catch (e) {
          setPidData(data);
        }
      } else {
        setPidData(data);
      }
    };

    return () => {
      (window as any).onPIDValueChanged = null;
    };
  }, []);

  const safeCall = useCallback((method: string, ...args: any[]) => {
    const bridge = (window as any).JSBridge;
    if (bridge && typeof bridge[method] === 'function') {
      try {
        return bridge[method](...args);
      } catch (e) {
        console.error(`Error calling ${method}:`, e);
      }
    } else {
      console.warn(`JSBridge.${method} not available.`);
    }
  }, []);

  return { status, pidData, safeCall };
};