/**
 * ECUInfoSelectionPage - ECU信息选择页
 *
 * ECU 多选，读取 ECU 信息
 */

import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  FlatList,
  ActivityIndicator,
  Alert,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import { useNavigation } from '@react-navigation/native';
import { useCloudBridge } from '../hooks/useCloudBridge';
import { useAppContext } from '../context/AppContext';
import { ECUItem } from '../services/CloudBridge';

export default function ECUInfoSelectionPage() {
  const navigation = useNavigation<any>();
  const { cloudConnected } = useAppContext();
  const { getECUList } = useCloudBridge();

  const [ecuList, setEcuList] = useState<ECUItem[]>([]);
  const [selectedIndices, setSelectedIndices] = useState<number[]>([]);
  const [loading, setLoading] = useState(true);

  // 加载 ECU 列表
  useEffect(() => {
    if (cloudConnected) {
      loadECUList();
    }
  }, [cloudConnected]);

  const loadECUList = async () => {
    setLoading(true);
    try {
      const data = await getECUList();
      setEcuList(data || []);
      // 默认全选
      setSelectedIndices(data?.map((_, i) => i) || []);
    } catch (e) {
      console.error('Load ECU list error:', e);
    }
    setLoading(false);
  };

  // 切换选择
  const handleToggle = (index: number) => {
    setSelectedIndices(prev => {
      if (prev.includes(index)) {
        return prev.filter(i => i !== index);
      }
      return [...prev, index];
    });
  };

  // 全选/取消全选
  const handleSelectAll = () => {
    if (selectedIndices.length === ecuList.length) {
      setSelectedIndices([]);
    } else {
      setSelectedIndices(ecuList.map((_, i) => i));
    }
  };

  // 开始读取
  const handleStart = () => {
    if (selectedIndices.length === 0) {
      Alert.alert('提示', '请至少选择一个ECU');
      return;
    }
    navigation.navigate('ECUInfoResult', { indices: selectedIndices, ecuList });
  };

  const isAllSelected = selectedIndices.length === ecuList.length && ecuList.length > 0;

  return (
    <SafeAreaView style={styles.container} edges={['bottom']}>
      {/* 标题栏 */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.headerButton}>
          <Icon name="arrow-left" size={24} color="#323233" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>ECU信息</Text>
        <View style={styles.headerButton} />
      </View>

      {/* 全选按钮 */}
      <TouchableOpacity style={styles.selectAllRow} onPress={handleSelectAll}>
        <Icon
          name={isAllSelected ? 'checkbox-marked' : 'checkbox-blank-outline'}
          size={24}
          color={isAllSelected ? '#1989fa' : '#c8c9cc'}
        />
        <Text style={styles.selectAllText}>全选</Text>
      </TouchableOpacity>

      {/* ECU 列表 */}
      {loading ? (
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color="#1989fa" />
        </View>
      ) : (
        <FlatList
          data={ecuList}
          keyExtractor={(item, index) => `${item.name}-${index}`}
          renderItem={({ item, index }) => (
            <TouchableOpacity
              style={styles.ecuItem}
              onPress={() => handleToggle(index)}
            >
              <Icon
                name={selectedIndices.includes(index) ? 'checkbox-marked' : 'checkbox-blank-outline'}
                size={24}
                color={selectedIndices.includes(index) ? '#1989fa' : '#c8c9cc'}
              />
              <Text style={styles.ecuName}>{item.name}</Text>
            </TouchableOpacity>
          )}
          ListEmptyComponent={
            <View style={styles.emptyContainer}>
              <Text style={styles.emptyText}>暂无ECU数据</Text>
            </View>
          }
        />
      )}

      {/* 底部按钮 */}
      <View style={styles.bottomBar}>
        <TouchableOpacity
          style={styles.button}
          onPress={handleStart}
        >
          <Text style={styles.buttonText}>开始读取</Text>
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
    justifyContent: 'space-between',
    paddingHorizontal: 4,
    paddingVertical: 12,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  headerButton: {
    padding: 8,
    width: 40,
  },
  headerTitle: {
    fontSize: 17,
    fontWeight: '600',
    color: '#323233',
  },
  selectAllRow: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  selectAllText: {
    marginLeft: 12,
    fontSize: 15,
    color: '#323233',
  },
  loadingContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  ecuItem: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  ecuName: {
    marginLeft: 12,
    fontSize: 15,
    color: '#323233',
  },
  emptyContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 60,
  },
  emptyText: {
    fontSize: 14,
    color: '#969799',
  },
  bottomBar: {
    padding: 16,
    backgroundColor: '#fff',
    borderTopWidth: StyleSheet.hairlineWidth,
    borderTopColor: '#eee',
  },
  button: {
    backgroundColor: '#1989fa',
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '500',
  },
});
