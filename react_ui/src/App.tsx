import React, { useContext, useEffect } from 'react';
import { HashRouter, Routes, Route, useNavigate } from 'react-router-dom';
import { ConfigProvider, NavBar, Grid, Button, Toast, Badge } from 'react-vant';
import { Exchange, Warning, PlayCircle, InfoO, Setting, WapHome } from '@react-vant/icons';
import { useOBD } from './hooks/useOBD';
import { AppProvider, AppContext } from './hooks/AppContext';
import VehicleConfig from './pages/VehicleConfig';
import BluetoothPage from './pages/BluetoothPage';
import DTCSelectionPage from './pages/DTCSelectionPage';
import DTCResultPage from './pages/DTCResultPage';
import ECUInfoSelectionPage from './pages/ECUInfoSelectionPage';
import ECUInfoResultPage from './pages/ECUInfoResultPage';
import FreezeFramePage from './pages/FreezeFramePage';
import LiveDataPage from './pages/LiveDataPage';
import './App.css';

// 全局监听器组件：负责监听 Bridge 事件并同步到 Context
const GlobalListener = () => {
  const context = useContext(AppContext);
  if (!context) return null;
  const { setConnectionStatus } = context;

  useEffect(() => {
    console.log('GlobalListener mounted');
    (window as any).onOBDStatusChanged = (newStatus: string) => {
      console.log('Global OBD Status Changed:', newStatus);
      setConnectionStatus(newStatus);
    };

    return () => {
      console.log('GlobalListener unmounted');
      (window as any).onOBDStatusChanged = null;
    };
  }, [setConnectionStatus]);

  return null;
};

const HomePage = () => {
  const { status, safeCall } = useOBD(); // status now comes from AppContext
  const navigate = useNavigate();
  const appContext = useContext(AppContext);

  if (!appContext) {
    throw new Error('HomePage must be used within AppProvider');
  }

  const { selectedProfile, selectedDevice } = appContext;
  const profileDisplay = selectedProfile
    ? `${selectedProfile.brand} ${selectedProfile.name}`
    : '未选择车型';
  const deviceDisplay = selectedDevice ? selectedDevice.name : '未选择设备';
  
  const isConnected = status === 'ConnectedToECU' || status === 'ConnectedToELM' || status === 'ConnectingToELM' || status === 'ConnectingToECU';
  const isFullyConnected = status === 'ConnectedToECU';

  const menuItems = [
    { text: '实时数据', icon: <PlayCircle />, disabled: !isFullyConnected, path: '/live-data' },
    { text: '故障码', icon: <Warning />, disabled: !isFullyConnected, path: '/dtc' },
    { text: '冻结帧', icon: <InfoO />, disabled: !isFullyConnected, path: '/freeze-frame' },
    { text: 'ECU信息', icon: <InfoO />, disabled: !isFullyConnected, path: '/ecu-info' },
    // 连接时禁用配置功能，强制用户先断开
    { text: '车辆配置', icon: <Setting />, disabled: isConnected, path: '/config' }, 
    { text: '蓝牙连接', icon: <Exchange />, disabled: isConnected, path: '/bluetooth' },
  ];

  const handleMenuClick = (item: any) => {
    if (item.disabled) {
      if (isConnected && (item.text === '车辆配置' || item.text === '蓝牙连接')) {
        Toast.info('请先断开连接');
      }
      return;
    }
    navigate(item.path);
  };

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar title="OBD 智能终端" leftArrow={false} />
      
      <div style={{ padding: 16, flex: 1 }}>
        {/* 状态卡片 */}
        <div style={{ background: '#fff', padding: 16, borderRadius: 8, marginBottom: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <span style={{ fontSize: 14 }}>当前车型: {profileDisplay}</span>
            <Badge content={status} color={isFullyConnected ? '#07c160' : (isConnected ? '#1989fa' : '#ee0a24')} />
        </div>

        {/* 6宫格菜单 */}
        <Grid columnNum={2} gutter={10}>
          {menuItems.map((item, index) => (
            <Grid.Item 
              key={index} 
              icon={item.icon} 
              text={item.text} 
              style={{ opacity: item.disabled ? 0.5 : 1 }}
              onClick={() => handleMenuClick(item)}
            />
          ))}
        </Grid>
      </div>

      {/* 底部控制栏 */}
      <div style={{ padding: 16, background: '#fff' }}>
        <div style={{ marginBottom: 8, textAlign: 'center', color: '#666', fontSize: 12 }}>
          当前设备: {deviceDisplay}
        </div>
        <Button 
          block 
          type={isConnected ? 'danger' : 'primary'} 
          onClick={() => {
            if (isConnected) {
              safeCall('disconnectAsync');
              // 乐观更新，虽然 GlobalListener 会处理，但这里可以立即给反馈
              // 实际通常等待 onOBDStatusChanged 回调
              return;
            }
            if (!selectedDevice) {
              Toast.info('请先选择蓝牙设备');
              return;
            }
            safeCall('connectAsync', 'bt', selectedDevice.address);
          }}
        >
          {isConnected ? '断开连接' : '开始连接'}
        </Button>
      </div>
    </div>
  );
};

export default function App() {
  return (
    <AppProvider>
      <GlobalListener />
      <ConfigProvider>
        <HashRouter>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/dtc" element={<DTCSelectionPage />} />
            <Route path="/dtc-result" element={<DTCResultPage />} />
            <Route path="/config" element={<VehicleConfig />} />
            <Route path="/bluetooth" element={<BluetoothPage />} />
            <Route path="/ecu-info" element={<ECUInfoSelectionPage />} />
            <Route path="/ecu-info-result" element={<ECUInfoResultPage />} />
            <Route path="/freeze-frame" element={<FreezeFramePage />} />
            <Route path="/live-data" element={<LiveDataPage />} />
          </Routes>
        </HashRouter>
      </ConfigProvider>
    </AppProvider>
  );
}
