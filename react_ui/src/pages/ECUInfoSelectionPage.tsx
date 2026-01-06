import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { NavBar, Cell, Checkbox, Button, Loading, Toast } from 'react-vant';

type ECUItem = {
  name: string;
  selected?: boolean;
  [key: string]: any;
};

const ECUInfoSelectionPage: React.FC = () => {
  const navigate = useNavigate();
  const [ecuList, setEcuList] = useState<ECUItem[]>([]);
  const [selectedIndices, setSelectedIndices] = useState<number[]>([]);
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
    setLoading(true);
    try {
      const result = safeCall('getECUList');
      if (result) {
        const data: ECUItem[] = typeof result === 'string' ? JSON.parse(result) : result;
        setEcuList(data);
        const defaults = data
          .map((item, index) => (item.selected ? index : -1))
          .filter(index => index !== -1);
        setSelectedIndices(defaults);
      }
    } catch (e) {
      console.error('Failed to load ECU list:', e);
      Toast.fail('加载ECU列表失败');
    } finally {
      setLoading(false);
    }
  }, []);

  const handleToggle = (index: number) => {
    setSelectedIndices(prev =>
      prev.includes(index) ? prev.filter(i => i !== index) : [...prev, index]
    );
  };

  const handleStart = () => {
    navigate('/ecu-info-result', { state: { indices: selectedIndices, ecuList } });
  };

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar title="ECU 标识信息" leftText="返回" onClickLeft={() => navigate(-1)} />

      <div style={{ flex: 1, overflow: 'auto' }}>
        {loading ? (
          <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', padding: 40, color: '#666' }}>
            <Loading type="spinner" />
            <div style={{ marginTop: 12 }}>正在加载ECU列表...</div>
          </div>
        ) : (
          <Checkbox.Group value={selectedIndices} onChange={value => setSelectedIndices(value as number[])}>
            <Cell.Group>
              {ecuList.map((item, index) => (
                <Cell
                  key={index}
                  clickable
                  title={item.name}
                  icon={
                    <Checkbox
                      name={index}
                      shape="square"
                      checked={selectedIndices.includes(index)}
                      onClick={event => {
                        event.stopPropagation();
                        handleToggle(index);
                      }}
                    />
                  }
                  onClick={() => handleToggle(index)}
                />
              ))}
            </Cell.Group>
          </Checkbox.Group>
        )}
      </div>

      <div style={{ padding: 16, background: '#fff' }}>
        <Button block type="primary" onClick={handleStart}>
          读取信息
        </Button>
      </div>
    </div>
  );
};

export default ECUInfoSelectionPage;
