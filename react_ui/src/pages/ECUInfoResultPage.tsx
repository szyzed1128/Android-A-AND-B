import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { NavBar, Collapse, Loading, Empty } from 'react-vant';

type LocationState = {
  indices?: number[];
  ecuList?: any[];
};

type ECUInfoItem = {
  name: string;
  info: string;
};

const ECUInfoResultPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { indices = [] } = (location.state as LocationState) || {};

  const [reading, setReading] = useState(false);
  const [items, setItems] = useState<ECUInfoItem[]>([]);

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
    // 1. 先注册回调
    const successHandler = (rawData: any) => {
      let data = rawData;
      if (typeof rawData === 'string') {
        try {
          data = JSON.parse(rawData);
        } catch (e) {
          console.error('Failed to parse ECU info data:', e);
        }
      }

      if (!Array.isArray(data)) {
        data = [];
      }

      const formatted: ECUInfoItem[] = [];
      for (let i = 0; i < data.length; i += 2) {
        formatted.push({
          name: (data[i] ?? '').toString().replace(/\n/g, ''),
          info: (data[i + 1] ?? '').toString().replace(/\u0000/g, ''),
        });
      }

      setItems(formatted);
      setReading(false);
    };

    (window as any).onReadECUInfoSuccess = successHandler;

    // 2. 再发起请求
    setReading(true);
    safeCall('readECUInfoAsync', JSON.stringify(indices));

    // 3. 清理
    return () => {
      (window as any).onReadECUInfoSuccess = null;
    };
  }, [indices]);

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar title="ECU 标识信息" leftText="返回" onClickLeft={() => navigate(-1)} />

      <div style={{ flex: 1, overflow: 'auto', padding: '8px 12px' }}>
        {reading ? (
          <div style={{ display: 'flex', justifyContent: 'center', paddingTop: 80 }}>
            <Loading type="spinner" vertical>
              正在读取ECU信息...
            </Loading>
          </div>
        ) : items.length === 0 ? (
          <Empty description="未获取到ECU信息" />
        ) : (
          <Collapse>
            {items.map((item, index) => (
              <Collapse.Item key={index} title={item.name}>
                <div style={{ whiteSpace: 'pre-wrap', fontSize: 14, color: '#333' }}>{item.info}</div>
              </Collapse.Item>
            ))}
          </Collapse>
        )}
      </div>
    </div>
  );
};

export default ECUInfoResultPage;
