import { useEffect, useState, useCallback } from 'react';

export const useOBD = () => {
  const [status, setStatus] = useState<string>('Disconnected');
  const [pidData, setPidData] = useState<any>(null);

  useEffect(() => {
    (window as any).onOBDStatusChanged = (newStatus: string) => {
      console.log('OBD Status:', newStatus);
      setStatus(newStatus);
    };

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
      (window as any).onOBDStatusChanged = null;
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