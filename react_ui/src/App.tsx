import React, { useContext } from 'react';
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
import './App.css';

const HomePage = () => {
  const { status, safeCall } = useOBD();
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
  
  const isConnected = status === 'ConnectedToECU' || status === 'ConnectedToELM';

  const menuItems = [
    { text: '实时数据', icon: <PlayCircle />, disabled: !isConnected, path: '/live-data' },
    { text: '故障码', icon: <Warning />, disabled: !isConnected, path: '/dtc' },
    { text: '冻结帧', icon: <InfoO />, disabled: !isConnected, path: '/freeze-frame' },
    { text: 'ECU信息', icon: <InfoO />, disabled: !isConnected, path: '/ecu-info' },
    { text: '车辆配置', icon: <Setting />, disabled: false, path: '/config' }, // 始终可用
    { text: '蓝牙连接', icon: <Exchange />, disabled: false, path: '/bluetooth' }, // 始终可用
  ];

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar title="OBD 智能终端" />
      
      <div style={{ padding: 16, flex: 1 }}>
        {/* 状态卡片 */}
        <div style={{ background: '#fff', padding: 16, borderRadius: 8, marginBottom: 16, display: 'flex', justifyContent: 'space-between' }}>
            <span>当前车型: {profileDisplay}</span>
            <Badge content={status} color={isConnected ? '#07c160' : '#ee0a24'} />
        </div>

        {/* 6宫格菜单 */}
        <Grid columnNum={2} gutter={10}>
          {menuItems.map((item, index) => (
            <Grid.Item 
              key={index} 
              icon={item.icon} 
              text={item.text} 
              style={{ opacity: item.disabled ? 0.5 : 1 }}
              onClick={() => !item.disabled && navigate(item.path)}
            />
          ))}
        </Grid>
      </div>

      {/* 底部控制栏 */}
      <div style={{ padding: 16, background: '#fff' }}>
        <div style={{ marginBottom: 8, textAlign: 'center', color: '#666' }}>
          当前设备: {deviceDisplay}
        </div>
        <Button 
          block 
          type={isConnected ? 'danger' : 'primary'} 
          onClick={() => {
            if (isConnected) {
              safeCall('disconnectAsync');
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
          </Routes>
        </HashRouter>
      </ConfigProvider>
    </AppProvider>
  );
}
