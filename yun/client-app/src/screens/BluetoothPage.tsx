/**
 * BluetoothPage - 蓝牙扫描页
 *
 * 三种协议选择 + 设备列表 + 手动MAC输入
 */

import React, { useEffect, useState, useCallback, useMemo } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  FlatList,
  ActivityIndicator,
  TextInput,
  Alert,
  Platform,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import { useNavigation } from '@react-navigation/native';
import { useLocalBluetooth } from '../hooks/useLocalBluetooth';
import { ScannedDevice } from '../context/AppContext';

type ProtocolTab = 'ble' | 'classic' | 'mfi';

export default function BluetoothPage() {
  const navigation = useNavigation<any>();
  const {
    scannedDevices,
    getFilteredDevices,
    isScanning,
    selectedDevice,
    startScan,
    stopScan,
    selectDevice,
  } = useLocalBluetooth();

  // 当前选中的协议标签
  const [activeProtocol, setActiveProtocol] = useState<ProtocolTab>('ble');
  // 手动输入
  const [manualVisible, setManualVisible] = useState(false);
  const [manualAddress, setManualAddress] = useState(selectedDevice?.address || '');
  // 正在选择的设备（防止多次点击）
  const [selectingAddress, setSelectingAddress] = useState<string | null>(null);

  // 协议标签配置
  const protocolTabs = useMemo(() => {
    const tabs: { key: ProtocolTab; label: string; icon: string; available: boolean }[] = [
      { key: 'ble', label: 'BLE', icon: 'bluetooth', available: true },
      { key: 'classic', label: '经典蓝牙', icon: 'bluetooth-connect', available: Platform.OS === 'android' },
      { key: 'mfi', label: 'MFi', icon: 'apple', available: Platform.OS === 'ios' },
    ];
    return tabs;
  }, []);

  // 过滤后的设备列表
  const filteredDevices = useMemo(() => {
    return getFilteredDevices(activeProtocol);
  }, [getFilteredDevices, activeProtocol]);

  // 进入页面时自动扫描当前协议
  useEffect(() => {
    startScan([activeProtocol]);
    return () => {
      stopScan();
    };
  }, []);

  // 切换协议时重新扫描
  const handleProtocolChange = useCallback((protocol: ProtocolTab) => {
    setActiveProtocol(protocol);
    stopScan();
    setTimeout(() => startScan([protocol]), 300);
  }, [stopScan, startScan]);

  // 刷新
  const handleRefresh = useCallback(() => {
    stopScan();
    setTimeout(() => startScan([activeProtocol]), 300);
  }, [stopScan, startScan, activeProtocol]);

  // 选择设备（带加载状态）
  const handleSelect = useCallback(async (device: ScannedDevice) => {
    // 防止重复点击
    if (selectingAddress === device.address) return;

    setSelectingAddress(device.address);
    stopScan();

    // 保存选择的设备
    selectDevice(device);

    // 短暂延迟让用户看到反馈
    setTimeout(() => {
      setSelectingAddress(null);
      Alert.alert('成功', `已选择设备: ${device.name}`, [
        { text: '确定', onPress: () => navigation.goBack() }
      ]);
    }, 200);
  }, [selectingAddress, stopScan, selectDevice, navigation]);

  // 手动确认
  const handleManualConfirm = useCallback(() => {
    const mac = manualAddress.trim();
    if (!mac) {
      Alert.alert('提示', '请输入MAC地址');
      return;
    }

    stopScan();
    selectDevice({
      name: '手动配置设备',
      address: mac,
      protocol: activeProtocol,
    });
    Alert.alert('成功', '已保存连接地址', [
      { text: '确定', onPress: () => navigation.goBack() }
    ]);
  }, [manualAddress, stopScan, selectDevice, navigation, activeProtocol]);

  // 渲染协议选择按钮
  const renderProtocolTabs = () => (
    <View style={styles.protocolTabs}>
      {protocolTabs.map((tab) => (
        <TouchableOpacity
          key={tab.key}
          style={[
            styles.protocolTab,
            activeProtocol === tab.key && styles.protocolTabActive,
            !tab.available && styles.protocolTabDisabled,
          ]}
          onPress={() => tab.available && handleProtocolChange(tab.key)}
          disabled={!tab.available}
          activeOpacity={0.7}
        >
          <Icon
            name={tab.icon}
            size={20}
            color={
              !tab.available
                ? '#c8c9cc'
                : activeProtocol === tab.key
                ? '#fff'
                : '#1989fa'
            }
          />
          <Text
            style={[
              styles.protocolTabText,
              activeProtocol === tab.key && styles.protocolTabTextActive,
              !tab.available && styles.protocolTabTextDisabled,
            ]}
          >
            {tab.label}
          </Text>
          {!tab.available && (
            <Text style={styles.protocolTabHint}>
              {Platform.OS === 'ios' ? '仅Android' : '仅iOS'}
            </Text>
          )}
        </TouchableOpacity>
      ))}
    </View>
  );

  // 渲染设备项
  const renderDevice = ({ item }: { item: ScannedDevice }) => {
    const isSelecting = selectingAddress === item.address;
    const isSelected = selectedDevice?.address === item.address;

    return (
      <TouchableOpacity
        style={[
          styles.deviceItem,
          isSelected && styles.deviceItemSelected,
        ]}
        onPress={() => handleSelect(item)}
        activeOpacity={0.7}
        disabled={isSelecting}
      >
        <View style={styles.deviceInfo}>
          <View style={styles.deviceNameRow}>
            <Text style={styles.deviceName}>{item.name || '未知设备'}</Text>
            {item.valid && (
              <View style={styles.obdTag}>
                <Text style={styles.obdTagText}>OBD</Text>
              </View>
            )}
            {item.paired && (
              <View style={styles.pairedTag}>
                <Text style={styles.pairedTagText}>已配对</Text>
              </View>
            )}
            {isSelected && (
              <View style={styles.selectedTag}>
                <Text style={styles.selectedTagText}>当前</Text>
              </View>
            )}
          </View>
          <Text style={styles.deviceAddress}>{item.address}</Text>
          {item.rssi !== undefined && (
            <Text style={styles.deviceRssi}>信号: {item.rssi} dBm</Text>
          )}
        </View>
        {isSelecting ? (
          <ActivityIndicator size="small" color="#1989fa" />
        ) : (
          <Icon name="chevron-right" size={20} color="#c8c9cc" />
        )}
      </TouchableOpacity>
    );
  };

  // 空列表
  const renderEmpty = () => {
    if (isScanning) return null;
    return (
      <View style={styles.emptyContainer}>
        <Icon name="bluetooth-off" size={48} color="#c8c9cc" />
        <Text style={styles.emptyText}>暂无设备</Text>
        <Text style={styles.emptyHint}>请确认蓝牙已开启后点击刷新</Text>
      </View>
    );
  };

  return (
    <SafeAreaView style={styles.container} edges={['bottom']}>
      {/* 标题栏 */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.headerButton}>
          <Icon name="arrow-left" size={24} color="#323233" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>选择蓝牙设备</Text>
        <TouchableOpacity onPress={handleRefresh} style={styles.headerButton}>
          <Icon name="refresh" size={24} color="#1989fa" />
        </TouchableOpacity>
      </View>

      {/* 协议选择按钮 */}
      {renderProtocolTabs()}

      {/* 扫描状态 */}
      {isScanning && (
        <View style={styles.scanningBar}>
          <ActivityIndicator size="small" color="#1989fa" />
          <Text style={styles.scanningText}>
            正在扫描 {activeProtocol.toUpperCase()} 设备... ({filteredDevices.length} 个)
          </Text>
        </View>
      )}

      {/* 设备列表 */}
      <FlatList
        data={filteredDevices}
        renderItem={renderDevice}
        keyExtractor={(item) => `${item.protocol}-${item.address}`}
        ListEmptyComponent={renderEmpty}
        contentContainerStyle={styles.listContent}
        extraData={selectingAddress}
      />

      {/* 手动输入区域 */}
      <View style={styles.manualSection}>
        {manualVisible ? (
          <View style={styles.manualForm}>
            <Text style={styles.manualLabel}>
              MAC地址 ({activeProtocol === 'ble' ? 'UUID' : 'MAC'})
            </Text>
            <TextInput
              style={styles.manualInput}
              value={manualAddress}
              onChangeText={setManualAddress}
              placeholder={activeProtocol === 'ble' ? '例如: 12345678-1234-1234-1234-123456789ABC' : '例如: AA:BB:CC:DD:EE:FF'}
              placeholderTextColor="#c8c9cc"
              autoCapitalize="characters"
            />
            <View style={styles.manualButtons}>
              <TouchableOpacity
                style={[styles.manualButton, styles.manualButtonPrimary]}
                onPress={handleManualConfirm}
              >
                <Text style={styles.manualButtonTextPrimary}>确认</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={styles.manualButton}
                onPress={() => setManualVisible(false)}
              >
                <Text style={styles.manualButtonText}>取消</Text>
              </TouchableOpacity>
            </View>
          </View>
        ) : (
          <TouchableOpacity
            style={styles.manualTrigger}
            onPress={() => setManualVisible(true)}
          >
            <Text style={styles.manualTriggerText}>手动输入地址</Text>
          </TouchableOpacity>
        )}
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
    justifyContent: 'space-between',
    paddingHorizontal: 4,
    paddingVertical: 12,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  headerButton: {
    padding: 8,
  },
  headerTitle: {
    fontSize: 17,
    fontWeight: '600',
    color: '#323233',
  },
  // 协议选择按钮样式
  protocolTabs: {
    flexDirection: 'row',
    padding: 12,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
    gap: 8,
  },
  protocolTab: {
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 10,
    paddingHorizontal: 8,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: '#1989fa',
    gap: 4,
  },
  protocolTabActive: {
    backgroundColor: '#1989fa',
    borderColor: '#1989fa',
  },
  protocolTabDisabled: {
    borderColor: '#c8c9cc',
    backgroundColor: '#f7f8fa',
  },
  protocolTabText: {
    fontSize: 13,
    color: '#1989fa',
    fontWeight: '500',
  },
  protocolTabTextActive: {
    color: '#fff',
  },
  protocolTabTextDisabled: {
    color: '#c8c9cc',
  },
  protocolTabHint: {
    fontSize: 9,
    color: '#c8c9cc',
    position: 'absolute',
    bottom: 2,
    right: 4,
  },
  // 扫描状态
  scanningBar: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 12,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  scanningText: {
    marginLeft: 8,
    fontSize: 14,
    color: '#666',
  },
  // 设备列表
  listContent: {
    flexGrow: 1,
  },
  deviceItem: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  deviceItemSelected: {
    backgroundColor: '#e6f7ff',
  },
  deviceInfo: {
    flex: 1,
  },
  deviceNameRow: {
    flexDirection: 'row',
    alignItems: 'center',
    flexWrap: 'wrap',
  },
  deviceName: {
    fontSize: 15,
    color: '#323233',
    marginRight: 8,
  },
  obdTag: {
    backgroundColor: '#1989fa',
    paddingHorizontal: 6,
    paddingVertical: 2,
    borderRadius: 4,
    marginRight: 4,
  },
  obdTagText: {
    fontSize: 10,
    color: '#fff',
    fontWeight: '500',
  },
  pairedTag: {
    backgroundColor: '#07c160',
    paddingHorizontal: 6,
    paddingVertical: 2,
    borderRadius: 4,
    marginRight: 4,
  },
  pairedTagText: {
    fontSize: 10,
    color: '#fff',
    fontWeight: '500',
  },
  selectedTag: {
    backgroundColor: '#ff976a',
    paddingHorizontal: 6,
    paddingVertical: 2,
    borderRadius: 4,
  },
  selectedTagText: {
    fontSize: 10,
    color: '#fff',
    fontWeight: '500',
  },
  deviceAddress: {
    fontSize: 12,
    color: '#969799',
    marginTop: 4,
  },
  deviceRssi: {
    fontSize: 11,
    color: '#c8c9cc',
    marginTop: 2,
  },
  // 空列表
  emptyContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 60,
  },
  emptyText: {
    fontSize: 14,
    color: '#969799',
    marginTop: 12,
  },
  emptyHint: {
    fontSize: 12,
    color: '#c8c9cc',
    marginTop: 4,
  },
  // 手动输入
  manualSection: {
    padding: 16,
    backgroundColor: '#fff',
    borderTopWidth: StyleSheet.hairlineWidth,
    borderTopColor: '#eee',
  },
  manualTrigger: {
    borderWidth: 1,
    borderColor: '#1989fa',
    borderRadius: 8,
    paddingVertical: 12,
    alignItems: 'center',
  },
  manualTriggerText: {
    color: '#1989fa',
    fontSize: 14,
    fontWeight: '500',
  },
  manualForm: {
    gap: 12,
  },
  manualLabel: {
    fontSize: 14,
    color: '#323233',
    marginBottom: 4,
  },
  manualInput: {
    borderWidth: 1,
    borderColor: '#ebedf0',
    borderRadius: 8,
    padding: 12,
    fontSize: 14,
    color: '#323233',
  },
  manualButtons: {
    flexDirection: 'row',
    gap: 8,
  },
  manualButton: {
    flex: 1,
    paddingVertical: 12,
    borderRadius: 8,
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#ebedf0',
  },
  manualButtonPrimary: {
    backgroundColor: '#1989fa',
    borderColor: '#1989fa',
  },
  manualButtonText: {
    fontSize: 14,
    color: '#323233',
  },
  manualButtonTextPrimary: {
    fontSize: 14,
    color: '#fff',
  },
});
