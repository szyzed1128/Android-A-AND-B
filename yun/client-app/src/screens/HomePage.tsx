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
  Modal,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import { useNavigation } from '@react-navigation/native';
import { useAppContext, ConnectionStatus } from '../context/AppContext';
import { useCloudBridge } from '../hooks/useCloudBridge';
import { useSchedulerActions } from '../hooks/useScheduler';
import { getBluetoothGateway } from '../hooks/useLocalBluetooth';
import { CONNECTION_STATUS_COLORS, CONNECTION_STATUS_TEXT } from '../constants/units';

type MenuItemType = {
  text: string;
  icon: string;
  disabled: boolean;
  screen: string;
};

type GlobalConnectAttempt = {
  id: number;
  startedAt: number;
};

let activeGlobalConnectAttempt: GlobalConnectAttempt | null = null;
let nextGlobalConnectAttemptId = 1;

function hasGlobalConnectAttempt(): boolean {
  return activeGlobalConnectAttempt !== null;
}

function beginGlobalConnectAttempt(): GlobalConnectAttempt | null {
  if (activeGlobalConnectAttempt) {
    return null;
  }

  const attempt = {
    id: nextGlobalConnectAttemptId++,
    startedAt: Date.now(),
  };
  activeGlobalConnectAttempt = attempt;
  console.log(`[HomePage] 创建全局连接尝试 id=${attempt.id}`);
  return attempt;
}

function finishGlobalConnectAttempt(id: number, reason: string): void {
  if (!activeGlobalConnectAttempt || activeGlobalConnectAttempt.id !== id) {
    return;
  }

  const ageMs = Date.now() - activeGlobalConnectAttempt.startedAt;
  console.log(`[HomePage] 结束全局连接尝试 id=${id} reason=${reason} ageMs=${ageMs}`);
  activeGlobalConnectAttempt = null;
}

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
    schedulerReady,
    setAssignedWsUrl,
  } = useAppContext();

  const { connectOBD, disconnectOBD, connectCloud, disconnectCloud, getOBDSessionId } = useCloudBridge();
  const { reserveInstance, notifyDisconnect, notifyRunning, syncDevice, syncCar } = useSchedulerActions();

  // 启用蓝牙桥接（连接CloudBridge和本地蓝牙）

  // 开发模式：云端连接状态
  const [cloudConnecting, setCloudConnecting] = useState(false);
  // OBD 连接中状态（包含调度预留阶段）
  const [obdConnecting, setObdConnecting] = useState(false);
  // 调度预留中（正在向调度后端申请实例）
  const [reserving, setReserving] = useState(false);
  // 断开中遮罩（等待 A 端真实 Disconnected 后才消失）
  const [disconnecting, setDisconnecting] = useState(false);
  const connectInFlightRef = useRef(false);
  const userCancelledRef = useRef(false);
  const ownedConnectAttemptIdRef = useRef<number | null>(null);
  const disconnectingTimerRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const disconnectPollRef = useRef<ReturnType<typeof setInterval> | null>(null);

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
    if (connectionStatus === 'ConnectingToECU') return '连接中';  // A端初始连接状态，ELM尚未连接
    if (connectionStatus === 'ConnectingToELM') return '连接中';
    if (connectionStatus === 'Disconnecting') return '断开中';
    if (['ConnectedToELM', 'ConnectedToECU'].includes(connectionStatus)) {
      return '已连接';
    }
    return '未连接';
  };

  const getEcuStatus = () => {
    if (connectionStatus === 'ConnectedToELM') return '连接中';  // ELM已连接，ECU连接中
    if (connectionStatus === 'Disconnecting') return '断开中';
    if (connectionStatus === 'ConnectedToECU') return '已连接';
    return '未连接';
  };

  const elmStatus = getElmStatus();
  const ecuStatus = getEcuStatus();

  const getStatusColor = (status: string) => {
    if (status === '已连接') return '#07c160';
    if (status === '连接中') return '#1989fa';
    if (status === '断开中') return '#ff976a';  // 橙色表示断开中
    return '#969799';
  };

  // 菜单项
  const menuItems: MenuItemType[] = [
    { text: '实时数据', icon: 'play-circle', disabled: !isFullyConnected, screen: 'LiveData' },
    { text: 'Burst采样', icon: 'lightning-bolt', disabled: !isFullyConnected, screen: 'BurstSnapshot' },
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
    const localConnecting = connectInFlightRef.current || obdConnecting;

    // 断开连接（连接中再次点击视为取消连接）
    if (isConnected || localConnecting) {
      userCancelledRef.current = true;  // 标记用户主动取消
      const ownedAttemptId = ownedConnectAttemptIdRef.current;
      connectInFlightRef.current = false;
      setObdConnecting(false);  // 连接中断开时必须清除，否则 obdConnecting 永久残留，下次无法重连

      const bluetoothGateway = getBluetoothGateway();
      const hasCloudSession = !!getOBDSessionId();
      const hasLocalSession = bluetoothGateway?.hasActiveSession?.() ?? false;

      try {
        if (hasCloudSession) {
          setDisconnecting(true);  // 显示"正在断开..."遮罩，直到前后端状态都真正可重连才消失
          if (disconnectingTimerRef.current) {
            clearTimeout(disconnectingTimerRef.current);
            disconnectingTimerRef.current = null;
          }
          if (disconnectPollRef.current) {
            clearInterval(disconnectPollRef.current);
            disconnectPollRef.current = null;
          }
          // 10 秒保险：若状态机异常卡住，至少不让 UI 永久被遮住
          disconnectingTimerRef.current = setTimeout(() => {
            disconnectingTimerRef.current = null;
            if (disconnectPollRef.current) {
              clearInterval(disconnectPollRef.current);
              disconnectPollRef.current = null;
            }
            console.warn('[HomePage] 断开遮罩超时，强制收起');
            setDisconnecting(false);
          }, 10000);
          await disconnectOBD();
          // 调度模式：通知调度后端保留实例5分钟
          if (schedulerReady) {
            notifyDisconnect().catch(e =>
              console.warn('[HomePage] notifyDisconnect 失败:', e.message)
            );
          }
        } else if (hasLocalSession) {
          console.log('[HomePage] 取消连接：仅断开本地蓝牙，会话尚未同步到云端');
          await bluetoothGateway?.disconnect();
          if (ownedAttemptId !== null) {
            finishGlobalConnectAttempt(ownedAttemptId, '取消连接：仅断开本地蓝牙');
            ownedConnectAttemptIdRef.current = null;
          }
        } else {
          console.log('[HomePage] 取消连接：当前无可断开的云端/本地会话');
          if (ownedAttemptId !== null) {
            finishGlobalConnectAttempt(ownedAttemptId, '取消连接：当前无会话');
            ownedConnectAttemptIdRef.current = null;
          }
        }
      } catch (e) {
        console.error('Disconnect error:', e);
        if (hasCloudSession) {
          if (disconnectingTimerRef.current) {
            clearTimeout(disconnectingTimerRef.current);
            disconnectingTimerRef.current = null;
          }
          if (disconnectPollRef.current) {
            clearInterval(disconnectPollRef.current);
            disconnectPollRef.current = null;
          }
          setDisconnecting(false);  // 出错时立即收起遮罩
        }
        if (ownedAttemptId !== null) {
          finishGlobalConnectAttempt(ownedAttemptId, `取消连接失败:${(e as any)?.message || 'unknown'}`);
          ownedConnectAttemptIdRef.current = null;
        }
      }
      return;
    }

    // 其他实例/重复回调导致的重复进入：直接忽略，不把它当成"取消连接"
    if (hasGlobalConnectAttempt()) {
      console.log('[HomePage] 忽略重复开始连接：已有全局连接尝试进行中');
      return;
    }

    // 防止重复点击"开始连接"
    if (obdConnecting) {
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

    // 强制走调度模式：必须等 schedulerReady = true 才能连接
    // （手动模式降级暂时注释掉，确保始终走新调度框架）
    const useSchedulerMode = true; // schedulerReady;

    if (!schedulerReady) {
      Alert.alert('提示', '调度服务尚未就绪，请稍候或检查服务器 IP 是否正确');
      return;
    }

    /* 手动模式降级（暂时关闭）
    if (!useSchedulerMode && !cloudConnected) {
      Alert.alert('提示', '请先连接云端服务');
      return;
    }
    */

    const attempt = beginGlobalConnectAttempt();
    if (!attempt) {
      console.log('[HomePage] 创建全局连接尝试失败：已有进行中的连接');
      return;
    }

    // 开始连接
    ownedConnectAttemptIdRef.current = attempt.id;
    userCancelledRef.current = false;
    connectInFlightRef.current = true;
    setObdConnecting(true);

    try {
      // 【调度模式】步骤0：向调度后端申请实例，获取 wsUrl
      let assignedUrl: string | undefined;
      if (useSchedulerMode) {
        console.log('[HomePage] 调度模式：向调度后端申请实例...');
        setReserving(true);
        try {
          assignedUrl = await reserveInstance();
          setAssignedWsUrl(assignedUrl);
          console.log('[HomePage] 调度分配成功 wsUrl=', assignedUrl);
        } finally {
          setReserving(false);
        }

        if (userCancelledRef.current) {
          console.log('[HomePage] 调度完成但用户已取消，释放已分配实例');
          // 通知调度后端释放刚分配的实例（保留5分钟，以防用户立刻重试）
          notifyDisconnect().catch(e =>
            console.warn('[HomePage] 取消后 notifyDisconnect 失败:', e.message)
          );
          setAssignedWsUrl(null);
          finishGlobalConnectAttempt(attempt.id, '用户取消：调度完成后');
          ownedConnectAttemptIdRef.current = null;
          connectInFlightRef.current = false;
          setObdConnecting(false);
          return;
        }

        // 连接到调度分配的 CloudBridge ws 地址
        const cloudOk = await connectCloud(assignedUrl);
        if (!cloudOk) throw new Error('连接调度实例失败');
      }

      // 【蓝牙连接】B 端先本地连接蓝牙，获取 sessionId 后再通知 A 端
      console.log('[HomePage] 开始本地蓝牙连接:', selectedDevice.protocol, selectedDevice.address);
      const gateway = getBluetoothGateway();
      if (!gateway) {
        throw new Error('蓝牙未初始化，请重启应用');
      }
      const sessionId = await gateway.connect(
        selectedDevice.protocol as 'ble' | 'classic' | 'mfi',
        selectedDevice.address
      );

      if (userCancelledRef.current) {
        console.log('[HomePage] 本地蓝牙连接完成，但用户已取消，立即断开并停止后续初始化');
        await gateway.disconnect();
        finishGlobalConnectAttempt(attempt.id, '用户取消：本地蓝牙连接完成后立即断开');
        ownedConnectAttemptIdRef.current = null;
        return;
      }

      console.log('[HomePage] 本地蓝牙已连接, sessionId:', sessionId);
      console.log('[HomePage] 通知云端开始初始化:', selectedDevice.protocol, selectedDevice.address);

      // 通知云端开始初始化（A 端直接使用 sessionId，不再发起 Connect 请求）
      await connectOBD(selectedDevice.protocol, selectedDevice.address, sessionId);

      // 调度模式：通知调度后端进入 running 状态
      if (useSchedulerMode) {
        await notifyRunning();
      }

      console.log('[HomePage] 云端已通知，等待初始化结果...');
    } catch (e: any) {
      // 用户主动取消时不显示错误提示
      if (!userCancelledRef.current) {
        console.error('[HomePage] 连接失败:', e);
        Alert.alert('连接失败', e.message || '连接过程出错');
      }
      userCancelledRef.current = false;
      // 断开蓝牙
      await getBluetoothGateway()?.disconnect();
      // 调度模式：连接失败时通知调度后端释放实例（不保留，直接归池）
      if (schedulerReady) {
        notifyDisconnect().catch(e =>
          console.warn('[HomePage] 连接失败后 notifyDisconnect 失败:', e.message)
        );
        setAssignedWsUrl(null);
      }
      finishGlobalConnectAttempt(attempt.id, `连接失败:${e?.message || 'unknown'}`);
      ownedConnectAttemptIdRef.current = null;
      connectInFlightRef.current = false;
      setObdConnecting(false);
    }
  };


  useEffect(() => {
    if (!connectInFlightRef.current && ownedConnectAttemptIdRef.current === null) return;
    if (connectionStatus === 'ConnectedToECU' || connectionStatus === 'Disconnected') {
      connectInFlightRef.current = false;
      setObdConnecting(false);
      if (ownedConnectAttemptIdRef.current !== null) {
        finishGlobalConnectAttempt(ownedConnectAttemptIdRef.current, `状态收口:${connectionStatus}`);
        ownedConnectAttemptIdRef.current = null;
      }
    }
  }, [connectionStatus]);

  // 断开遮罩只在"真正可重连"时消失，而不是固定延时。
  // 需要同时满足：
  // 1) A 端状态已到 Disconnected
  // 2) CloudBridge 已清空 sessionId
  // 3) 本地蓝牙网关已无活动会话/挂起连接
  // 4) HomePage 自己的连接门禁已完全收口
  useEffect(() => {
    if (!disconnecting) {
      if (disconnectPollRef.current) {
        clearInterval(disconnectPollRef.current);
        disconnectPollRef.current = null;
      }
      return;
    }

    const checkDisconnectReady = () => {
      const bluetoothGateway = getBluetoothGateway();
      const hasCloudSession = !!getOBDSessionId();
      const localReady =
        bluetoothGateway?.isReadyForConnect?.() ??
        !(bluetoothGateway?.hasActiveSession?.() ?? false);
      const ready =
        connectionStatus === 'Disconnected' &&
        !hasCloudSession &&
        localReady &&
        !connectInFlightRef.current &&
        !obdConnecting &&
        ownedConnectAttemptIdRef.current === null &&
        !hasGlobalConnectAttempt();

      if (!ready) {
        return;
      }

      console.log('[HomePage] 断开流程已完全收口，关闭断开遮罩');
      if (disconnectingTimerRef.current) {
        clearTimeout(disconnectingTimerRef.current);
        disconnectingTimerRef.current = null;
      }
      if (disconnectPollRef.current) {
        clearInterval(disconnectPollRef.current);
        disconnectPollRef.current = null;
      }
      setDisconnecting(false);
    };

    checkDisconnectReady();

    if (!disconnectPollRef.current) {
      disconnectPollRef.current = setInterval(checkDisconnectReady, 100);
    }

    return () => {
      if (disconnectPollRef.current) {
        clearInterval(disconnectPollRef.current);
        disconnectPollRef.current = null;
      }
    };
  }, [connectionStatus, disconnecting, getOBDSessionId, obdConnecting]);

  useEffect(() => {
    return () => {
      if (disconnectingTimerRef.current) {
        clearTimeout(disconnectingTimerRef.current);
        disconnectingTimerRef.current = null;
      }
      if (disconnectPollRef.current) {
        clearInterval(disconnectPollRef.current);
        disconnectPollRef.current = null;
      }
    };
  }, []);

  // 监控 UI 状态变化（用于诊断）
  useEffect(() => {
    console.log(`[HomePage UI] ========== 状态变化 ==========`);
    console.log(`[HomePage UI] connectionStatus: ${connectionStatus}`);
    console.log(`[HomePage UI] → ELM状态: ${elmStatus}, ECU状态: ${ecuStatus}`);
    console.log(`[HomePage UI] → isConnected: ${isConnected}, isFullyConnected: ${isFullyConnected}`);
    console.log(`[HomePage UI] → 按钮状态: obdConnecting=${obdConnecting}, connectInFlight=${connectInFlightRef.current}`);
    console.log(`[HomePage UI] =====================================`);
  }, [connectionStatus, elmStatus, ecuStatus, isConnected, isFullyConnected, obdConnecting]);

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
            (isConnected || obdConnecting) && styles.disconnectButton,
          ]}
          onPress={handleConnect}
          activeOpacity={0.8}
        >
          <Text style={styles.connectButtonText}>
            {isConnected ? '断开连接' : (reserving ? '分配实例中...' : obdConnecting ? '取消连接' : '开始连接')}
          </Text>
        </TouchableOpacity>
      </View>

      {/* 正在断开遮罩：阻止一切操作，等 A 端确认断开后消失 */}
      <Modal visible={disconnecting} transparent animationType="fade">
        <View style={styles.disconnectingOverlay}>
          <View style={styles.disconnectingCard}>
            <ActivityIndicator size="large" color="#1989fa" />
            <Text style={styles.disconnectingText}>正在断开...</Text>
          </View>
        </View>
      </Modal>
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
  disconnectingOverlay: {
    flex: 1,
    backgroundColor: 'rgba(0,0,0,0.45)',
    justifyContent: 'center',
    alignItems: 'center',
  },
  disconnectingCard: {
    backgroundColor: '#fff',
    borderRadius: 12,
    paddingVertical: 32,
    paddingHorizontal: 44,
    alignItems: 'center',
  },
  disconnectingText: {
    marginTop: 14,
    fontSize: 16,
    color: '#323233',
    fontWeight: '500',
  },
});
