/**
 * OBD网关客户端应用 - 软件B
 * 主入口文件 - 带导航
 */
import React, { useRef, useCallback } from 'react';
import { StatusBar } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { AppProvider, useAppContext } from './context/AppContext';
import { useBluetoothBridge } from './hooks/useBluetoothBridge';
import { useCloudBridge } from './hooks/useCloudBridge';
import { getBluetoothGateway } from './hooks/useLocalBluetooth';
import CloudBridge from './services/CloudBridge';

// 页面
import HomePage from './screens/HomePage';
import BluetoothPage from './screens/BluetoothPage';
import VehicleConfigPage from './screens/VehicleConfigPage';
import DTCSelectionPage from './screens/DTCSelectionPage';
import DTCResultPage from './screens/DTCResultPage';
import ECUInfoSelectionPage from './screens/ECUInfoSelectionPage';
import ECUInfoResultPage from './screens/ECUInfoResultPage';
import FreezeFramePage from './screens/FreezeFramePage';
import LiveDataPage from './screens/LiveDataPage';
import BurstSnapshotPage from './screens/BurstSnapshotPage';

// 导航参数类型
export type RootStackParamList = {
  Home: undefined;
  Bluetooth: undefined;
  VehicleConfig: undefined;
  DTCSelection: undefined;
  DTCResult: { indices: number[]; ecuList: any[] };
  ECUInfoSelection: undefined;
  ECUInfoResult: { indices: number[]; ecuList: any[] };
  FreezeFrame: undefined;
  LiveData: undefined;
  BurstSnapshot: undefined;
};

const Stack = createNativeStackNavigator<RootStackParamList>();

const AUTO_RECONNECT_MAX_ATTEMPTS = 5;
const AUTO_RECONNECT_INTERVAL_MS = 3000;   // ELM327 重启约需 3-5s
const AUTO_RECONNECT_SETTLE_MS = 800;      // sendConnectionLost 后等 A 端开始清理

const BluetoothBridgeBootstrap: React.FC = () => {
  const { selectedDevice, cloudConnected } = useAppContext();
  const { connectOBD } = useCloudBridge();
  const autoReconnectingRef = useRef(false);

  // 用 ref 持有最新值，避免 useEffect/callback stale closure
  const selectedDeviceRef = useRef(selectedDevice);
  const cloudConnectedRef = useRef(cloudConnected);
  React.useEffect(() => { selectedDeviceRef.current = selectedDevice; }, [selectedDevice]);
  React.useEffect(() => { cloudConnectedRef.current = cloudConnected; }, [cloudConnected]);
  const connectOBDRef = useRef(connectOBD);
  React.useEffect(() => { connectOBDRef.current = connectOBD; }, [connectOBD]);

  const handleConnectionLost = useCallback(async (sessionId: string, reason: string) => {
    console.warn(`[AutoReconnect] 蓝牙连接丢失: ${reason} (session=${sessionId})`);

    // 用户主动断开时 CloudBridge 已清空 sessionId，此时不重连
    if (!CloudBridge.getSessionId()) {
      console.log('[AutoReconnect] 无活跃 sessionId，判断为用户主动断开，跳过重连');
      CloudBridge.sendConnectionLost(sessionId, reason);
      return;
    }

    // 前置条件检查
    const device = selectedDeviceRef.current;
    if (!device || !cloudConnectedRef.current) {
      console.warn('[AutoReconnect] 无设备或云端未连接，跳过重连');
      CloudBridge.sendConnectionLost(sessionId, reason);
      return;
    }

    // 防重入
    if (autoReconnectingRef.current) {
      console.warn('[AutoReconnect] 已有重连进行中，忽略');
      return;
    }
    autoReconnectingRef.current = true;

    const gateway = getBluetoothGateway();
    if (!gateway) {
      console.warn('[AutoReconnect] 蓝牙网关未初始化，跳过重连');
      CloudBridge.sendConnectionLost(sessionId, reason);
      autoReconnectingRef.current = false;
      return;
    }

    let reconnected = false;
    let newSessionId = '';

    for (let attempt = 1; attempt <= AUTO_RECONNECT_MAX_ATTEMPTS; attempt++) {
      console.log(`[AutoReconnect] 第 ${attempt}/${AUTO_RECONNECT_MAX_ATTEMPTS} 次尝试，等待 ${AUTO_RECONNECT_INTERVAL_MS}ms...`);
      await new Promise(resolve => setTimeout(resolve, AUTO_RECONNECT_INTERVAL_MS));

      // 每次重试前再检查一次用户是否主动断开
      if (!CloudBridge.getSessionId()) {
        console.log('[AutoReconnect] 重连等待期间 sessionId 被清空，用户主动断开，终止重连');
        autoReconnectingRef.current = false;
        return;
      }

      try {
        console.log(`[AutoReconnect] 尝试重连 BLE: ${device.protocol} ${device.address}`);
        newSessionId = await gateway.connect(
          device.protocol as 'ble' | 'classic' | 'mfi',
          device.address
        );
        console.log(`[AutoReconnect] BLE 重连成功，newSessionId=${newSessionId}`);
        reconnected = true;
        break;
      } catch (e: any) {
        console.warn(`[AutoReconnect] 第 ${attempt} 次重连失败: ${e?.message || e}`);
      }
    }

    if (reconnected) {
      // 通知 A 旧会话结束
      CloudBridge.sendConnectionLost(sessionId, reason);
      // 等待 A 端开始清理旧会话
      await new Promise(resolve => setTimeout(resolve, AUTO_RECONNECT_SETTLE_MS));
      // 检查用户是否在等待期间主动断开
      if (!CloudBridge.getSessionId() && !newSessionId) {
        console.log('[AutoReconnect] settle 期间用户主动断开，终止重连');
        autoReconnectingRef.current = false;
        return;
      }
      try {
        console.log(`[AutoReconnect] 通知 A 端重新初始化 OBD，session=${newSessionId}`);
        await connectOBDRef.current(device.protocol, device.address, newSessionId);
        console.log('[AutoReconnect] 自动重连成功');
      } catch (e: any) {
        console.error(`[AutoReconnect] connectOBD 失败: ${e?.message || e}`);
      }
    } else {
      console.warn('[AutoReconnect] 所有重连尝试失败，上报断连');
      CloudBridge.sendConnectionLost(sessionId, reason);
    }

    autoReconnectingRef.current = false;
  }, []);

  useBluetoothBridge({ onConnectionLost: handleConnectionLost });
  return null;
};

const App: React.FC = () => {
  return (
    <SafeAreaProvider>
      <AppProvider>
        <BluetoothBridgeBootstrap />
        <StatusBar barStyle="dark-content" backgroundColor="#fff" />
        <NavigationContainer>
          <Stack.Navigator
            initialRouteName="Home"
            screenOptions={{
              headerShown: false,
              animation: 'slide_from_right',
            }}
          >
            <Stack.Screen name="Home" component={HomePage} />
            <Stack.Screen name="Bluetooth" component={BluetoothPage} />
            <Stack.Screen name="VehicleConfig" component={VehicleConfigPage} />
            <Stack.Screen name="DTCSelection" component={DTCSelectionPage} />
            <Stack.Screen name="DTCResult" component={DTCResultPage} />
            <Stack.Screen name="ECUInfoSelection" component={ECUInfoSelectionPage} />
            <Stack.Screen name="ECUInfoResult" component={ECUInfoResultPage} />
            <Stack.Screen name="FreezeFrame" component={FreezeFramePage} />
            <Stack.Screen name="LiveData" component={LiveDataPage} />
            <Stack.Screen name="BurstSnapshot" component={BurstSnapshotPage} />
          </Stack.Navigator>
        </NavigationContainer>
      </AppProvider>
    </SafeAreaProvider>
  );
};

export default App;
