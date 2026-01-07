import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { NavBar, Cell, Empty, Loading } from 'react-vant';

type FreezeFrameItem = {
  Name: string;
  Value: string | number;
  Units?: string;
};

const FreezeFramePage: React.FC = () => {
  const navigate = useNavigate();
  const [data, setData] = useState<FreezeFrameItem[]>([]);
  const [loading, setLoading] = useState(false);

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
    const handler = (rawData: any) => {
      let parsed = rawData;
      if (typeof rawData === 'string') {
        try {
          parsed = JSON.parse(rawData);
        } catch (e) {
          console.error('Failed to parse freeze frame data:', e);
          parsed = [];
        }
      }

      if (!Array.isArray(parsed)) {
        parsed = [];
      }

      setData(parsed as FreezeFrameItem[]);
      setLoading(false);
    };

    (window as any).onReadFreezeFrameSuccess = handler;

    // 2. 再发起请求
    setLoading(true);
    safeCall('readFreezeFrameAsync', 0);

    // 3. 清理
    return () => {
      (window as any).onReadFreezeFrameSuccess = null;
    };
  }, []);

  const renderContent = () => {
    if (loading) {
      return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', paddingTop: 80 }}>
          <Loading type="spinner" vertical>
            正在读取冻结帧...
          </Loading>
        </div>
      );
    }

    if (!data.length) {
      return <Empty description="未发现冻结帧数据" />;
    }

    return (
      <Cell.Group>
        {data.map((item, index) => {
          const isNaNValue = String(item.Value) === 'NaN';
          const displayValue = `${item.Value ?? ''} ${item.Units ?? ''}`.trim();
          return (
            <Cell
              key={index}
              title={item.Name}
              value={<span style={{ color: isNaNValue ? '#999' : '#323233' }}>{displayValue}</span>}
            />
          );
        })}
      </Cell.Group>
    );
  };

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar title="冻结帧" leftText="返回" onClickLeft={() => navigate(-1)} />
      <div style={{ flex: 1, overflow: 'auto' }}>{renderContent()}</div>
    </div>
  );
};

export default FreezeFramePage;
