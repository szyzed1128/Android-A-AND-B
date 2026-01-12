import React, { useContext, useEffect } from 'react';
import { HashRouter, Routes, Route, useNavigate } from 'react-router-dom';
import { ConfigProvider, NavBar, Grid, Button, Toast } from 'react-vant';
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
  const { safeCall } = useOBD();
  const navigate = useNavigate();
  const appContext = useContext(AppContext);

  if (!appContext) {
    throw new Error('HomePage must be used within AppProvider');
  }

  const { selectedProfile, selectedDevice, connectionStatus: status } = appContext;
  const profileDisplay = selectedProfile
    ? `${selectedProfile.brand} ${selectedProfile.name}`
    : '未选择车型';
  const deviceDisplay = selectedDevice ? selectedDevice.name : '未选择设备';
  
  const isConnected =
    status === 'ConnectedToECU' ||
    status === 'ConnectedToELM' ||
    status === 'ConnectingToELM' ||
    status === 'ConnectingToECU';
  const isFullyConnected = status === 'ConnectedToECU';

  const elmStatus =
    status === 'ConnectingToELM'
      ? '连接中'
      : status === 'ConnectedToELM' || status === 'ConnectingToECU' || status === 'ConnectedToECU'
      ? '已连接'
      : '未连接';
  const ecuStatus =
    status === 'ConnectingToECU' ? '连接中' : status === 'ConnectedToECU' ? '已连接' : '未连接';
  const elmColor =
    elmStatus === '已连接' ? '#07c160' : elmStatus === '连接中' ? '#1989fa' : '#969799';
  const ecuColor =
    ecuStatus === '已连接' ? '#07c160' : ecuStatus === '连接中' ? '#1989fa' : '#969799';
  const statusItemStyle = { display: 'flex', alignItems: 'center', gap: 4, fontSize: 12 };

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
        return;
      }
      Toast.info('请先连接车辆');
      return;
    }
    navigate(item.path);
  };

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar title="OBD 智能终端" leftArrow={false} />
      
      <div style={{ padding: 16, flex: 1 }}>
        {/* 状态卡片 */}
        <div
          style={{
            background: '#fff',
            padding: 16,
            borderRadius: 8,
            marginBottom: 16,
            display: 'flex',
            justifyContent: 'flex-start',
            alignItems: 'center',
          }}
        >
          <span style={{ fontSize: 14 }}>当前车型: {profileDisplay}</span>
        </div>

        {/* 6宫格菜单 */}
        <Grid columnNum={2} gutter={10} key={status}>
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
        <div
          style={{
            display: 'flex',
            justifyContent: 'space-around',
            marginBottom: 16,
            padding: '10px',
            background: '#f5f6f7',
            borderRadius: '8px',
          }}
        >
          <div style={statusItemStyle}>
            <span>ELM: </span>
            <span style={{ color: elmColor }}>{elmStatus}</span>
          </div>
          <div style={statusItemStyle}>
            <span>ECU: </span>
            <span style={{ color: ecuColor }}>{ecuStatus}</span>
          </div>
        </div>

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
