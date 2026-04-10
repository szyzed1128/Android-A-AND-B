/**
 * OBD网关客户端应用 - 软件B
 * 主入口文件 - 带导航
 */
import React, { useRef, useCallback } from 'react';
import { StatusBar } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { AppProvider } from './context/AppContext';
import { useBluetoothBridge } from './hooks/useBluetoothBridge';
import { useScheduler } from './hooks/useScheduler';
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

// ELM327 重启大约需要 3-5 秒，在此期间 A端发来的 connect 请求会失败。
// B端只需延迟上报 connectionLost，给 ELM 时间启动，然后让 A端内置重连机制自然接管。
// 不在 B端主动发起 gateway.connect / connectOBD，避免与 A端重连竞争。
const AUTO_RECONNECT_DELAY_MS = 4000;

const BluetoothBridgeBootstrap: React.FC = () => {
  const autoReconnectingRef = useRef(false);

  // 调度系统初始化（会话 + 心跳）
  useScheduler();

  const handleConnectionLost = useCallback(async (sessionId: string, reason: string) => {
    console.warn(`[AutoReconnect] 蓝牙连接丢失: ${reason} (session=${sessionId})`);

    // 用户主动断开：CloudBridge 已清空 sessionId，直接上报，不等待
    if (!CloudBridge.getSessionId()) {
      console.log('[AutoReconnect] 无活跃 sessionId，用户主动断开，立即上报');
      CloudBridge.sendConnectionLost(sessionId, reason);
      return;
    }

    // 防重入
    if (autoReconnectingRef.current) {
      console.warn('[AutoReconnect] 已有等待进行中，忽略');
      return;
    }
    autoReconnectingRef.current = true;

    // 等待 ELM327 重启完成，然后上报 connectionLost
    // A端收到后会自动发起 connect 请求，B端正常处理即可
    console.log(`[AutoReconnect] 等待 ${AUTO_RECONNECT_DELAY_MS}ms（ELM启动时间）后上报断连`);
    await new Promise(resolve => setTimeout(resolve, AUTO_RECONNECT_DELAY_MS));

    // 等待期间检查用户是否主动断开
    if (!CloudBridge.getSessionId()) {
      console.log('[AutoReconnect] 等待期间 sessionId 被清空，用户主动断开，跳过上报');
      autoReconnectingRef.current = false;
      return;
    }

    console.log('[AutoReconnect] 上报 connectionLost，交由 A端重连机制处理');
    CloudBridge.sendConnectionLost(sessionId, reason);
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
