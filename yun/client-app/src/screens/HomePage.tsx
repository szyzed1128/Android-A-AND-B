/**
 * HomePage - 首页
 *
 * 6按钮菜单 + 连接状态 + 设备信息
 */

import React, { useEffect, useRef, useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  Alert,
  ScrollView,
  TextInput,
  ActivityIndicator,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import { useNavigation } from '@react-navigation/native';
import { useAppContext, ConnectionStatus } from '../context/AppContext';
import { useCloudBridge } from '../hooks/useCloudBridge';
import { getBluetoothGateway } from '../hooks/useLocalBluetooth';
import { CONNECTION_STATUS_COLORS, CONNECTION_STATUS_TEXT } from '../constants/units';

type MenuItemType = {
  text: string;
  icon: string;
  disabled: boolean;
  screen: string;
};

export default function HomePage() {
  const navigation = useNavigation<any>();
  const {
    selectedProfile,
    selectedDevice,
    connectionStatus,
    cloudConnected,
    cloudHost,
    setCloudHost,
    elmTimeoutMs,
    setElmTimeoutMs,
  } = useAppContext();

  const { connectOBD, disconnectOBD, connectCloud, disconnectCloud } = useCloudBridge();

  // 启用蓝牙桥接（连接CloudBridge和本地蓝牙）

  // 开发模式：云端连接状态
  const [cloudConnecting, setCloudConnecting] = useState(false);
  // OBD 连接中状态
  const [obdConnecting, setObdConnecting] = useState(false);
  const connectInFlightRef = useRef(false);

  // 显示文本
  const profileDisplay = selectedProfile
    ? `${selectedProfile.brand} ${selectedProfile.name}`
    : '未选择车型';
  const deviceDisplay = selectedDevice ? selectedDevice.name : '未选择设备';

  // 连接状态判断
  const isConnected =
    connectionStatus === 'ConnectedToECU' ||
    connectionStatus === 'ConnectedToELM' ||
    connectionStatus === 'ConnectingToELM' ||
    connectionStatus === 'ConnectingToECU';
  const isFullyConnected = connectionStatus === 'ConnectedToECU';

  // ELM/ECU 状态
  const getElmStatus = () => {
    if (connectionStatus === 'ConnectingToELM') return '连接中';
    if (['ConnectedToELM', 'ConnectingToECU', 'ConnectedToECU'].includes(connectionStatus)) {
      return '已连接';
    }
    return '未连接';
  };

  const getEcuStatus = () => {
    if (connectionStatus === 'ConnectingToECU') return '连接中';
    if (connectionStatus === 'ConnectedToECU') return '已连接';
    return '未连接';
  };

  const elmStatus = getElmStatus();
  const ecuStatus = getEcuStatus();

  const getStatusColor = (status: string) => {
    if (status === '已连接') return '#07c160';
    if (status === '连接中') return '#1989fa';
    return '#969799';
  };

  // 菜单项
  const menuItems: MenuItemType[] = [
    { text: '实时数据', icon: 'play-circle', disabled: !isFullyConnected, screen: 'LiveData' },
    { text: '故障码', icon: 'alert-circle', disabled: !isFullyConnected, screen: 'DTCSelection' },
    { text: '冻结帧', icon: 'snowflake', disabled: !isFullyConnected, screen: 'FreezeFrame' },
    { text: 'ECU信息', icon: 'information', disabled: !isFullyConnected, screen: 'ECUInfoSelection' },
    { text: '车辆配置', icon: 'cog', disabled: isConnected, screen: 'VehicleConfig' },
    { text: '蓝牙连接', icon: 'bluetooth', disabled: isConnected, screen: 'Bluetooth' },
  ];

  // 菜单点击
  const handleMenuClick = (item: MenuItemType) => {
    if (item.disabled) {
      if (isConnected && (item.text === '车辆配置' || item.text === '蓝牙连接')) {
        Alert.alert('提示', '请先断开连接');
        return;
      }
      Alert.alert('提示', '请先连接车辆');
      return;
    }
    navigation.navigate(item.screen);
  };

  // 连接/断开
  const handleConnect = async () => {
    if (obdConnecting || connectInFlightRef.current) {
      console.log('[HomePage] 连接流程进行中，忽略重复请求');
      return;
    }

    // 断开连接
    if (isConnected) {
      connectInFlightRef.current = false;
      setObdConnecting(true);
      try {
        await getBluetoothGateway()?.disconnect();
        await disconnectOBD();
      } catch (e) {
        console.error('Disconnect error:', e);
      }
      setObdConnecting(false);
      return;
    }

    // 检查前置条件
    if (!selectedDevice) {
      Alert.alert('提示', '请先选择蓝牙设备', [
        { text: '去选择', onPress: () => navigation.navigate('Bluetooth') },
        { text: '取消', style: 'cancel' },
      ]);
      return;
    }

    if (!cloudConnected) {
      Alert.alert('提示', '请先连接云端服务');
      return;
    }

    // 开始连接
    connectInFlightRef.current = true;
    setObdConnecting(true);

    try {
      // 新流程：B 端先本地连接蓝牙，获取 sessionId 后再通知 A 端
      console.log('[HomePage] 开始本地蓝牙连接:', selectedDevice.protocol, selectedDevice.address);
      const gateway = getBluetoothGateway();
      if (!gateway) {
        throw new Error('蓝牙未初始化，请重启应用');
      }
      const sessionId = await gateway.connect(
        selectedDevice.protocol as 'ble' | 'classic' | 'mfi',
        selectedDevice.address
      );

      console.log('[HomePage] 本地蓝牙已连接, sessionId:', sessionId);
      console.log('[HomePage] 通知云端开始初始化:', selectedDevice.protocol, selectedDevice.address);

      // 步骤2: 通知云端开始初始化（A 端直接使用 sessionId，不再发起 Connect 请求）
      await connectOBD(selectedDevice.protocol, selectedDevice.address, sessionId);

      console.log('[HomePage] 云端已通知，等待初始化结果...');
    } catch (e: any) {
      console.error('[HomePage] 连接失败:', e);
      Alert.alert('连接失败', e.message || '连接过程出错');
      // 断开蓝牙
      await getBluetoothGateway()?.disconnect();
      connectInFlightRef.current = false;
      setObdConnecting(false);
    }
  };

  useEffect(() => {
    if (!connectInFlightRef.current) return;
    if (connectionStatus === 'ConnectedToECU' || connectionStatus === 'Disconnected') {
      connectInFlightRef.current = false;
      setObdConnecting(false);
    }
  }, [connectionStatus]);

  // 开发模式：连接/断开云端
  const handleCloudConnect = async () => {
    if (cloudConnected) {
      disconnectCloud();
      return;
    }

    if (!cloudHost.trim()) {
      Alert.alert('提示', '请输入云端IP地址');
      return;
    }

    setCloudConnecting(true);
    try {
      const success = await connectCloud();
      if (!success) {
        Alert.alert('连接失败', '无法连接到云端服务，请检查IP地址和网络');
      }
    } catch (e) {
      Alert.alert('连接失败', '连接出错');
    }
    setCloudConnecting(false);
  };

  return (
    <SafeAreaView style={styles.container}>
      {/* 标题栏 */}
      <View style={styles.header}>
        <Text style={styles.headerTitle}>OBD 智能终端</Text>
        <View style={[styles.cloudIndicator, { backgroundColor: cloudConnected ? '#07c160' : '#969799' }]} />
      </View>

      <ScrollView style={styles.content}>
        {/* 状态卡片 */}
        <View style={styles.statusCard}>
          <Text style={styles.statusText}>当前车型: {profileDisplay}</Text>
        </View>

        {/* 6宫格菜单 */}
        <View style={styles.menuGrid}>
          {menuItems.map((item, index) => (
            <TouchableOpacity
              key={index}
              style={[styles.menuItem, item.disabled && styles.menuItemDisabled]}
              onPress={() => handleMenuClick(item)}
              activeOpacity={0.7}
            >
              <Icon
                name={item.icon}
                size={32}
                color={item.disabled ? '#c8c9cc' : '#1989fa'}
              />
              <Text style={[styles.menuText, item.disabled && styles.menuTextDisabled]}>
                {item.text}
              </Text>
            </TouchableOpacity>
          ))}
        </View>

        {/* 开发模式：云端连接 */}
        <View style={styles.devSection}>
          <Text style={styles.devSectionTitle}>开发调试</Text>
          <View style={styles.cloudConnectRow}>
            <TextInput
              style={styles.cloudHostInput}
              value={cloudHost}
              onChangeText={setCloudHost}
              placeholder="云端IP地址"
              placeholderTextColor="#c8c9cc"
              editable={!cloudConnected && !cloudConnecting}
            />
            <TouchableOpacity
              style={[
                styles.cloudConnectButton,
                cloudConnected && styles.cloudDisconnectButton,
              ]}
              onPress={handleCloudConnect}
              disabled={cloudConnecting}
            >
              {cloudConnecting ? (
                <ActivityIndicator size="small" color="#fff" />
              ) : (
                <Text style={styles.cloudConnectButtonText}>
                  {cloudConnected ? '断开' : '连接'}
                </Text>
              )}
            </TouchableOpacity>
          </View>
          <Text style={styles.cloudStatusText}>
            云端状态: {cloudConnected ? '已连接' : '未连接'}
          </Text>

          {/* ELM327 ATST 超时覆盖 */}
          <View style={styles.timeoutRow}>
            <Text style={styles.timeoutLabel}>ELM超时:</Text>
            {([400, 800, 1000, 1020] as const).map((ms) => (
              <TouchableOpacity
                key={ms}
                style={[styles.timeoutOption, elmTimeoutMs === ms && styles.timeoutOptionActive]}
                onPress={() => setElmTimeoutMs(ms)}
              >
                <Text style={[styles.timeoutOptionText, elmTimeoutMs === ms && styles.timeoutOptionTextActive]}>
                  {ms === 400 ? '默认' : ms === 1020 ? '最大' : `${ms}ms`}
                </Text>
              </TouchableOpacity>
            ))}
          </View>
          <Text style={styles.timeoutHint}>
            {elmTimeoutMs <= 400
              ? '400ms（原始值，适合直连蓝牙）'
              : `覆盖 ATST → ${Math.min(255, Math.ceil(elmTimeoutMs / 4)).toString(16).toUpperCase()}（${Math.min(255, Math.ceil(elmTimeoutMs / 4)) * 4}ms，适合 WebSocket 桥接）`}
          </Text>
        </View>
      </ScrollView>

      {/* 底部控制栏 */}
      <View style={styles.bottomBar}>
        {/* 状态指示 */}
        <View style={styles.statusRow}>
          <View style={styles.statusItem}>
            <Text style={styles.statusLabel}>ELM: </Text>
            <Text style={[styles.statusValue, { color: getStatusColor(elmStatus) }]}>
              {elmStatus}
            </Text>
          </View>
          <View style={styles.statusItem}>
            <Text style={styles.statusLabel}>ECU: </Text>
            <Text style={[styles.statusValue, { color: getStatusColor(ecuStatus) }]}>
              {ecuStatus}
            </Text>
          </View>
        </View>

        {/* 当前设备 */}
        <Text style={styles.deviceText}>当前设备: {deviceDisplay}</Text>

        {/* 连接按钮 */}
        <TouchableOpacity
          style={[
            styles.connectButton,
            isConnected && styles.disconnectButton,
            obdConnecting && styles.connectingButton,
          ]}
          onPress={handleConnect}
          activeOpacity={0.8}
          disabled={obdConnecting}
        >
          {obdConnecting ? (
            <View style={styles.connectingRow}>
              <ActivityIndicator size="small" color="#fff" />
              <Text style={styles.connectButtonText}>
                {isConnected ? ' 断开中...' : ' 连接中...'}
              </Text>
            </View>
          ) : (
            <Text style={styles.connectButtonText}>
              {isConnected ? '断开连接' : '开始连接'}
            </Text>
          )}
        </TouchableOpacity>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f7f8fa',
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 16,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  headerTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#323233',
  },
  cloudIndicator: {
    width: 8,
    height: 8,
    borderRadius: 4,
    marginLeft: 8,
  },
  content: {
    flex: 1,
    padding: 16,
  },
  statusCard: {
    backgroundColor: '#fff',
    padding: 16,
    borderRadius: 8,
    marginBottom: 16,
  },
  statusText: {
    fontSize: 14,
    color: '#323233',
  },
  menuGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
  },
  menuItem: {
    width: '48%',
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 8,
    alignItems: 'center',
    marginBottom: 12,
  },
  menuItemDisabled: {
    opacity: 0.5,
  },
  menuText: {
    marginTop: 8,
    fontSize: 14,
    color: '#323233',
  },
  menuTextDisabled: {
    color: '#c8c9cc',
  },
  bottomBar: {
    padding: 16,
    backgroundColor: '#fff',
    borderTopWidth: StyleSheet.hairlineWidth,
    borderTopColor: '#eee',
  },
  statusRow: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    padding: 10,
    backgroundColor: '#f5f6f7',
    borderRadius: 8,
    marginBottom: 12,
  },
  statusItem: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  statusLabel: {
    fontSize: 12,
    color: '#666',
  },
  statusValue: {
    fontSize: 12,
    fontWeight: '500',
  },
  deviceText: {
    textAlign: 'center',
    color: '#666',
    fontSize: 12,
    marginBottom: 12,
  },
  connectButton: {
    backgroundColor: '#1989fa',
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
  },
  disconnectButton: {
    backgroundColor: '#ee0a24',
  },
  connectingButton: {
    backgroundColor: '#969799',
  },
  connectingRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
  },
  connectButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '500',
  },
  // 开发模式样式
  devSection: {
    backgroundColor: '#fff',
    padding: 16,
    borderRadius: 8,
    marginTop: 4,
    borderWidth: 1,
    borderColor: '#ff976a',
    borderStyle: 'dashed',
  },
  devSectionTitle: {
    fontSize: 12,
    color: '#ff976a',
    marginBottom: 12,
    fontWeight: '500',
  },
  cloudConnectRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  cloudHostInput: {
    flex: 1,
    borderWidth: 1,
    borderColor: '#ebedf0',
    borderRadius: 6,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 14,
    color: '#323233',
  },
  cloudConnectButton: {
    backgroundColor: '#07c160',
    paddingHorizontal: 20,
    paddingVertical: 10,
    borderRadius: 6,
    minWidth: 70,
    alignItems: 'center',
  },
  cloudDisconnectButton: {
    backgroundColor: '#ee0a24',
  },
  cloudConnectButtonText: {
    color: '#fff',
    fontSize: 14,
    fontWeight: '500',
  },
  cloudStatusText: {
    marginTop: 8,
    fontSize: 12,
    color: '#969799',
  },
  timeoutRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 12,
    gap: 6,
  },
  timeoutLabel: {
    fontSize: 12,
    color: '#ff976a',
    marginRight: 4,
  },
  timeoutOption: {
    paddingHorizontal: 10,
    paddingVertical: 5,
    borderRadius: 4,
    borderWidth: 1,
    borderColor: '#ff976a',
  },
  timeoutOptionActive: {
    backgroundColor: '#ff976a',
  },
  timeoutOptionText: {
    fontSize: 12,
    color: '#ff976a',
  },
  timeoutOptionTextActive: {
    color: '#fff',
  },
  timeoutHint: {
    marginTop: 4,
    fontSize: 11,
    color: '#c8c9cc',
  },
});
