import React, { useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { NavBar, Cell, Button, Loading, Field, Toast, Tag } from 'react-vant';
import { AppContext, ScannedDevice } from '../hooks/AppContext';

const BluetoothPage: React.FC = () => {
  const navigate = useNavigate();
  const appContext = useContext(AppContext);

  if (!appContext) {
    throw new Error('BluetoothPage must be used within AppProvider');
  }

  const {
    selectedDevice,
    setSelectedDevice,
    scannedDevices,
    isScanning,
    setIsScanning,
    clearScannedDevices
  } = appContext;

  const [manualVisible, setManualVisible] = useState(false);
  const [manualAddress, setManualAddress] = useState(selectedDevice?.address || '');

  const callBridge = useCallback((method: 'startBTScan' | 'stopBTScan') => {
    const bridge = (window as any).JSBridge;
    if (bridge && typeof bridge[method] === 'function') {
      try {
        console.log(`Calling JSBridge.${method}()`);
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
    clearScannedDevices();
    callBridge('startBTScan');
    setIsScanning(true);
  }, [callBridge, clearScannedDevices, setIsScanning]);

  const stopScan = useCallback(() => {
    callBridge('stopBTScan');
    setIsScanning(false);
  }, [callBridge, setIsScanning]);

  // 对设备列表排序：valid: true 的设备置顶
  const sortedDevices = useMemo(() => {
    return [...scannedDevices].sort((a, b) => {
      if (a.valid && !b.valid) return -1;
      if (!a.valid && b.valid) return 1;
      return 0;
    });
  }, [scannedDevices]);

  // 进入页面时自动获取一次设备列表
  useEffect(() => {
    startScan();
    return () => {
      stopScan();
    };
  }, []);

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
            <div>正在扫描设备... ({scannedDevices.length} 个)</div>
          </div>
        )}

        <Cell.Group>
          {sortedDevices.map((device, index) => (
            <Cell
              key={device.address || index}
              clickable
              title={
                <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                  <span>{device.name || '未知设备'}</span>
                  {device.valid && <Tag type="primary" size="medium">OBD</Tag>}
                </div>
              }
              label={device.address}
              onClick={() => handleSelect(device)}
            />
          ))}
          {!sortedDevices.length && !isScanning && (
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
