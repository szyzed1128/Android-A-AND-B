import React, { useCallback, useContext, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { NavBar, Cell, Button, Loading, Field, Toast } from 'react-vant';
import { AppContext } from '../hooks/AppContext';

interface ScanEvent {
  type: string;
  name?: string;
  address: string;
}

const BluetoothPage: React.FC = () => {
  const navigate = useNavigate();
  const appContext = useContext(AppContext);

  if (!appContext) {
    throw new Error('BluetoothPage must be used within AppProvider');
  }

  const { selectedDevice, setSelectedDevice } = appContext;
  const [devices, setDevices] = useState<ScanEvent[]>([]);
  const [isScanning, setIsScanning] = useState(false);
  const [manualVisible, setManualVisible] = useState(false);
  const [manualAddress, setManualAddress] = useState(selectedDevice?.address || '');

  const callBridge = useCallback((method: 'startBTScan' | 'stopBTScan') => {
    const bridge = (window as any).JSBridge;
    if (bridge && typeof bridge[method] === 'function') {
      try {
        return bridge[method]();
      } catch (e) {
        console.error(`JSBridge.${method} error:`, e);
      }
    } else {
      console.warn(`JSBridge.${method} not available.`);
    }
    return null;
  }, []);

  const startScan = useCallback(() => {
    setDevices([]);
    callBridge('startBTScan');
    setIsScanning(true);
  }, [callBridge]);

  const stopScan = useCallback(() => {
    callBridge('stopBTScan');
    setIsScanning(false);
  }, [callBridge]);

  useEffect(() => {
    const handleScan = (data: any) => {
      let payload: ScanEvent | null = null;
      try {
        payload = typeof data === 'string' ? JSON.parse(data) : data;
      } catch (e) {
        console.error('Failed to parse scan data:', e, data);
      }

      if (!payload || payload.type !== 'discovered' || !payload.address) {
        return;
      }

      setDevices(prev =>
        prev.some(item => item.address === payload!.address)
          ? prev
          : [...prev, { name: payload!.name || '未知设备', address: payload!.address }]
      );
    };

    (window as any).onBTScanEvent = handleScan;
    startScan();

    return () => {
      stopScan();
      (window as any).onBTScanEvent = null;
    };
  }, [startScan, stopScan]);

  const handleRefresh = () => {
    stopScan();
    startScan();
  };

  const handleSelect = (device: { name?: string; address: string }) => {
    stopScan();
    setSelectedDevice({ name: device.name || '未知设备', address: device.address });
    Toast.success('已选择设备');
    navigate('/');
  };

  const handleManualConfirm = () => {
    const mac = manualAddress.trim();
    if (!mac) {
      Toast.fail('请输入MAC地址');
      return;
    }

    stopScan();
    setSelectedDevice({ name: '手动配置设备', address: mac });
    Toast.success('已保存连接地址');
    navigate('/');
  };

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar
        title="配置连接地址"
        leftText="返回"
        rightText="刷新"
        onClickLeft={() => navigate(-1)}
        onClickRight={handleRefresh}
      />

      <div style={{ flex: 1, overflow: 'auto' }}>
        {isScanning && (
          <div style={{ display: 'flex', alignItems: 'center', padding: '12px 16px', gap: 8, color: '#666' }}>
            <Loading type="spinner" />
            <div>正在扫描设备...</div>
          </div>
        )}

        <Cell.Group>
          {devices.map((device, index) => (
            <Cell
              key={device.address || index}
              clickable
              title={device.name || '未知设备'}
              label={device.address}
              onClick={() => handleSelect(device)}
            />
          ))}
          {!devices.length && !isScanning && (
            <Cell title="暂无设备" label="请确认蓝牙已开启后点击刷新" />
          )}
        </Cell.Group>

        <div style={{ padding: 16, background: '#fff' }}>
          {manualVisible ? (
            <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
              <Field
                value={manualAddress}
                onChange={val => setManualAddress(val)}
                label="MAC地址"
                placeholder="例如: AA:BB:CC:DD:EE:FF"
                clearable
              />
              <div style={{ display: 'flex', gap: 8 }}>
                <Button type="primary" block onClick={handleManualConfirm}>
                  确认
                </Button>
                <Button block onClick={() => setManualVisible(false)}>
                  取消
                </Button>
              </div>
            </div>
          ) : (
            <Button block type="primary" plain onClick={() => setManualVisible(true)}>
              手动输入地址
            </Button>
          )}
        </div>
      </div>
    </div>
  );
};

export default BluetoothPage;
