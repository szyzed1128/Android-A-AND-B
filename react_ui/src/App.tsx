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

    // 触发 WebSocket 初始化（云端模式）
    // 这会让 C# 侧的 OBDCloudManager 初始化 WebSocket 连接
    const bridge = (window as any).JSBridge;
    if (bridge && typeof bridge.callCSharpMethod === 'function') {
      console.log('Triggering WebSocket initialization...');
      try {
        bridge.callCSharpMethod('initWebSocket');
        // WebSocket 初始化成功只意味着网络层就绪，不是 OBD 设备已连接
        // 真正的连接状态由后端 OBDDataReader.CurrentStatus 决定
        // 这里不再设置 ConnectedToELM 状态
        console.log('WebSocket initialization triggered');
      } catch (e) {
        console.warn('initWebSocket call failed (expected in non-cloud mode):', e);
      }
    }

    return () => {
      console.log('GlobalListener unmounted');
      (window as any).onOBDStatusChanged = null;
    };
  }, [setConnectionStatus]);

  return null;
};

// 蓝牙扫描全局监听器：负责监听蓝牙扫描事件并同步到 Context
const BTScanListener = () => {
  const context = useContext(AppContext);
  if (!context) return null;
  const { setScannedDevices } = context;

  useEffect(() => {
    console.log('BTScanListener mounted');
    (window as any).onBTScanEvent = (data: any) => {
      console.log('onBTScanEvent', data);

      let payload: { type?: string; name?: string; address?: string } | null = null;
      try {
        payload = typeof data === 'string' ? JSON.parse(data) : data;
      } catch (e) {
        console.error('Failed to parse BT scan data:', e, data);
        return;
      }

      if (!payload || payload.type !== 'discovered' || !payload.address) {
        return;
      }

      const newDevice = {
        name: payload.name || '未知设备',
        address: payload.address,
        valid: payload.valid,
        paired: payload.paired
      };
      setScannedDevices(prev =>
        prev.some(d => d.address === newDevice.address) ? prev : [...prev, newDevice]
      );
    };

    return () => {
      console.log('BTScanListener unmounted');
      (window as any).onBTScanEvent = null;
    };
  }, [setScannedDevices]);

  return null;
};

// OBD回调全局桥接：持久化注册所有CarScanner OBD回调
// 解决问题：C#调用 window.onReadFreezeFrameSuccess() 时页面可能已卸载导致 ReferenceError
// 方案：全局回调派发 CustomEvent，各页面监听事件而非直接赋值 window.onXxx
const OBDCallbackBridge = () => {
  useEffect(() => {
    const OBD_CALLBACKS = [
      'onReadDTCSuccess', 'onReadDTCError', 'onReadDTCFinish',
      'onClearDTCSuccess', 'onClearDTCError', 'onClearDTCFinish',
      'onReadECUInfoSuccess', 'onReadECUInfoError',
      'onReadFreezeFrameSuccess', 'onReadFreezeFrameError', 'onReadFreezeFrameFinish',
      'onPIDValueChanged',
    ];

    OBD_CALLBACKS.forEach(name => {
      (window as any)[name] = (...args: any[]) => {
        const data = args[0] ?? null;
        // 1. 派发 CustomEvent，当前已挂载的页面组件可监听
        window.dispatchEvent(new CustomEvent(`obd:${name}`, { detail: data }));
        // 2. 转发给 OBDCloudManager → WebSocket → B端（云端模式下使用）
        try {
          const b = (window as any).JSBridge;
          if (b && typeof b.uiCallback === 'function') {
            b.uiCallback(name, JSON.stringify(data));
          }
        } catch (_) {
          // 非云端模式下 uiCallback 可能不存在，忽略
        }
      };
    });

    // 这些是持久回调，不在 cleanup 中移除
  }, []);

  return null;
};

// 后端就绪检测器：App 启动时检测后端是否初始化完成
const BackendReadyChecker = () => {
  const context = useContext(AppContext);
  if (!context) return null;
  const { setIsBackendReady } = context;

  useEffect(() => {
    console.log('BackendReadyChecker started');
    let retryCount = 0;
    const maxRetries = 60; // 最多检测60秒
    let timer: ReturnType<typeof setInterval> | null = null;

    const checkBackend = (): boolean => {
      try {
        const bridge = (window as any).JSBridge;
        if (bridge && typeof bridge.getBrands === 'function') {
          const result = bridge.getBrands();
          if (result) {
            const data = typeof result === 'string' ? JSON.parse(result) : result;
            if (data && data.length > 0) {
              console.log('Backend is ready!');
              setIsBackendReady(true);
              return true;
            }
          }
        }
      } catch (e) {
        console.log(`Backend check attempt ${retryCount + 1} failed:`, e);
      }
      return false;
    };

    // 第一次立即检测
    if (checkBackend()) return;

    // 开始轮询检测
    timer = setInterval(() => {
      retryCount++;
      if (checkBackend()) {
        if (timer) clearInterval(timer);
        return;
      }
      if (retryCount >= maxRetries) {
        if (timer) clearInterval(timer);
        console.log('Backend ready check timeout');
      }
    }, 1000);

    return () => {
      if (timer) clearInterval(timer);
    };
  }, [setIsBackendReady]);

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
      <BTScanListener />
      <OBDCallbackBridge />
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
