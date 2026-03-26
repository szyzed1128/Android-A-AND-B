/**
 * OBD网关客户端应用 - 软件B
 * 主入口文件 - 带导航
 */
import React from 'react';
import { StatusBar } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { AppProvider } from './context/AppContext';
import { useBluetoothBridge } from './hooks/useBluetoothBridge';

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

const BluetoothBridgeBootstrap: React.FC = () => {
  useBluetoothBridge();
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
