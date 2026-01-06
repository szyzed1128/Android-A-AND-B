import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { NavBar, Cell, Loading, Empty, Toast } from 'react-vant';

type LocationState = {
  indices?: number[];
  ecuList?: any[];
};

const DTCResultPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { indices = [], ecuList = [] } = (location.state as LocationState) || {};

  const [results, setResults] = useState<any[][]>([]);
  const [reading, setReading] = useState(false);

  const safeCall = (method: string, ...args: any[]) => {
    const bridge = (window as any).JSBridge;
    if (bridge && typeof bridge[method] === 'function') {
      try {
        return bridge[method](...args);
      } catch (e) {
        console.error(`JSBridge.${method} error:`, e);
      }
    } else {
      console.warn(`JSBridge.${method} not available.`);
    }
    return null;
  };

  useEffect(() => {
    setReading(true);
    safeCall('readDTCAsync', indices);
  }, [indices]);

  useEffect(() => {
    const handler = (rawData: any) => {
      let parsed = rawData;
      if (typeof rawData === 'string') {
        try {
          parsed = JSON.parse(rawData);
        } catch (e) {
          console.error('Failed to parse DTC data:', e);
        }
      }

      if (!Array.isArray(parsed)) {
        parsed = [];
      }

      setResults(parsed as any[][]);
      setReading(false);
    };

    (window as any).onReadDTCSuccess = handler;
    return () => {
      (window as any).onReadDTCSuccess = null;
    };
  }, []);

  const hasAnyCodes =
    Array.isArray(results) &&
    results.some(item => Array.isArray(item) && item.length > 0);

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar title="故障码诊断" leftText="返回" onClickLeft={() => navigate(-1)} />

      <div style={{ flex: 1, overflow: 'auto', padding: '8px 0' }}>
        {reading ? (
          <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', paddingTop: 80 }}>
            <Loading type="spinner" vertical>
              正在读取故障码...
            </Loading>
          </div>
        ) : hasAnyCodes ? (
          indices.map((idx, i) => {
            const ecuName = ecuList[idx]?.name || `ECU ${idx}`;
            const codes = Array.isArray(results[i]) ? results[i] : [];
            if (!codes.length) {
              return null;
            }
            return (
              <Cell.Group key={idx} title={ecuName}>
                {codes.map((code: any, ci: number) => (
                  <Cell
                    key={ci}
                    title={code?.Code || '未知故障码'}
                    label={code?.Description || '无描述'}
                  />
                ))}
              </Cell.Group>
            );
          })
        ) : (
          <Empty description="未发现故障码" />
        )}
      </div>
    </div>
  );
};

export default DTCResultPage;
