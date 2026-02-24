import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { NavBar, Cell, Loading, Empty } from 'react-vant';

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
    // 监听全局 OBD CustomEvent（由 App.tsx OBDCallbackBridge 派发）
    const handleSuccess = (e: Event) => {
      const rawData = (e as CustomEvent).detail;
      console.log('onReadDTCSuccess called:', rawData);
      let parsed = rawData;
      if (typeof rawData === 'string') {
        try {
          parsed = JSON.parse(rawData);
        } catch (err) {
          console.error('Failed to parse DTC data:', err);
        }
      }
      if (!Array.isArray(parsed)) parsed = [];
      setResults(parsed as any[][]);
    };

    const handleError = (e: Event) => {
      console.error('onReadDTCError:', (e as CustomEvent).detail);
    };

    const handleFinish = () => {
      console.log('onReadDTCFinish called');
      setReading(false);
    };

    window.addEventListener('obd:onReadDTCSuccess', handleSuccess);
    window.addEventListener('obd:onReadDTCError', handleError);
    window.addEventListener('obd:onReadDTCFinish', handleFinish);

    // 发起请求
    setReading(true);
    safeCall('readDTCAsync', JSON.stringify(indices));

    return () => {
      window.removeEventListener('obd:onReadDTCSuccess', handleSuccess);
      window.removeEventListener('obd:onReadDTCError', handleError);
      window.removeEventListener('obd:onReadDTCFinish', handleFinish);
    };
  }, [indices]);

  const hasAnyCodes =
    Array.isArray(results) &&
    results.some(item => Array.isArray(item) && item.length > 0);

  // 格式化故障码显示：P007312 -> P0073(12)
  const formatDTCCode = (code: string | undefined): string => {
    if (!code) return '未知故障码';
    // 标准 DTC 格式是 5 位（如 P0073），超过的部分是状态码
    if (code.length > 5) {
      const baseCode = code.substring(0, 5);
      const suffix = code.substring(5);
      return `${baseCode}(${suffix})`;
    }
    return code;
  };

  // 获取故障码描述
  const getDescription = (code: any): string => {
    if (!code) return '无描述';
    // 优先使用 Descriptions 数组中的描述
    if (code.Descriptions && Array.isArray(code.Descriptions) && code.Descriptions.length > 0) {
      return code.Descriptions[0].Description || '无描述';
    }
    return code.Description || '无描述';
  };

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
                    title={formatDTCCode(code?.Code)}
                    label={getDescription(code)}
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
