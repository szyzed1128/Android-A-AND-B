/**
 * FreezeFramePage - 冻结帧页
 *
 * 显示故障发生时的快照数据
 */

import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  FlatList,
  ActivityIndicator,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import { useNavigation } from '@react-navigation/native';
import { useCloudBridge } from '../hooks/useCloudBridge';
import { useAppContext } from '../context/AppContext';
import { UNITS_MAP } from '../constants/units';

interface FreezeFrameItem {
  pid: string;
  name: string;
  value: string;
  unit: number | string;  // A 端可能返回字符串单位（如 "%"、"kPa"）或数字代码
}

export default function FreezeFramePage() {
  const navigation = useNavigation<any>();
  const { cloudConnected } = useAppContext();
  const { readFreezeFrameAsync } = useCloudBridge();

  const [frameData, setFrameData] = useState<FreezeFrameItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [frameIndex, setFrameIndex] = useState(0);

  // 加载冻结帧数据
  useEffect(() => {
    if (cloudConnected) {
      loadFreezeFrame(frameIndex);
    }
  }, [cloudConnected, frameIndex]);

  const loadFreezeFrame = async (frame: number) => {
    setLoading(true);
    setError(null);

    readFreezeFrameAsync(frame, {
      onSuccess: (data: FreezeFrameItem[]) => {
        setFrameData(data || []);
      },
      onError: (err: string) => {
        setError(err);
        setFrameData([]);
      },
      onFinish: () => {
        setLoading(false);
      },
    });
  };

  // 获取单位文本：A 端返回字符串时直接用，数字时查 UNITS_MAP
  const getUnitText = (unitCode: number | string): string => {
    if (typeof unitCode === 'string') return unitCode;
    return UNITS_MAP[unitCode] || '';
  };

  // 渲染数据项
  const renderItem = ({ item }: { item: FreezeFrameItem }) => (
    <View style={styles.dataItem}>
      <View style={styles.dataInfo}>
        <Text style={styles.dataName}>{item.name}</Text>
        <Text style={styles.dataPID}>{item.pid}</Text>
      </View>
      <View style={styles.dataValueContainer}>
        <Text style={styles.dataValue}>{item.value}</Text>
        <Text style={styles.dataUnit}>{getUnitText(item.unit)}</Text>
      </View>
    </View>
  );

  return (
    <SafeAreaView style={styles.container} edges={['bottom']}>
      {/* 标题栏 */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.headerButton}>
          <Icon name="arrow-left" size={24} color="#323233" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>冻结帧</Text>
        <TouchableOpacity
          onPress={() => loadFreezeFrame(frameIndex)}
          style={styles.headerButton}
          disabled={loading}
        >
          <Icon name="refresh" size={24} color={loading ? '#c8c9cc' : '#1989fa'} />
        </TouchableOpacity>
      </View>

      {/* 帧选择器 */}
      <View style={styles.frameSelector}>
        <TouchableOpacity
          style={[styles.frameButton, frameIndex === 0 && styles.frameButtonDisabled]}
          onPress={() => setFrameIndex(prev => Math.max(0, prev - 1))}
          disabled={frameIndex === 0 || loading}
        >
          <Icon name="chevron-left" size={24} color={frameIndex === 0 ? '#c8c9cc' : '#1989fa'} />
        </TouchableOpacity>
        <Text style={styles.frameText}>帧 #{frameIndex}</Text>
        <TouchableOpacity
          style={styles.frameButton}
          onPress={() => setFrameIndex(prev => prev + 1)}
          disabled={loading}
        >
          <Icon name="chevron-right" size={24} color={loading ? '#c8c9cc' : '#1989fa'} />
        </TouchableOpacity>
      </View>

      {/* 内容区域 */}
      {loading ? (
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color="#1989fa" />
          <Text style={styles.loadingText}>正在读取冻结帧数据...</Text>
        </View>
      ) : error ? (
        <View style={styles.errorContainer}>
          <Icon name="alert-circle-outline" size={48} color="#ee0a24" />
          <Text style={styles.errorText}>{error}</Text>
          <TouchableOpacity
            style={styles.retryButton}
            onPress={() => loadFreezeFrame(frameIndex)}
          >
            <Text style={styles.retryButtonText}>重试</Text>
          </TouchableOpacity>
        </View>
      ) : frameData.length === 0 ? (
        <View style={styles.emptyContainer}>
          <Icon name="snowflake" size={48} color="#c8c9cc" />
          <Text style={styles.emptyText}>无冻结帧数据</Text>
          <Text style={styles.emptyHint}>当前帧没有记录快照数据</Text>
        </View>
      ) : (
        <FlatList
          data={frameData}
          keyExtractor={(item, index) => `${item.pid}-${index}`}
          renderItem={renderItem}
          contentContainerStyle={styles.listContent}
        />
      )}
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
  frameSelector: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    padding: 12,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  frameButton: {
    padding: 8,
  },
  frameButtonDisabled: {
    opacity: 0.5,
  },
  frameText: {
    marginHorizontal: 20,
    fontSize: 16,
    fontWeight: '500',
    color: '#323233',
  },
  loadingContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  loadingText: {
    marginTop: 16,
    fontSize: 14,
    color: '#666',
  },
  errorContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: 40,
  },
  errorText: {
    marginTop: 16,
    fontSize: 14,
    color: '#ee0a24',
    textAlign: 'center',
  },
  retryButton: {
    marginTop: 20,
    paddingHorizontal: 24,
    paddingVertical: 10,
    backgroundColor: '#1989fa',
    borderRadius: 8,
  },
  retryButtonText: {
    color: '#fff',
    fontSize: 14,
    fontWeight: '500',
  },
  emptyContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: 40,
  },
  emptyText: {
    marginTop: 16,
    fontSize: 16,
    color: '#969799',
  },
  emptyHint: {
    marginTop: 8,
    fontSize: 13,
    color: '#c8c9cc',
  },
  listContent: {
    padding: 12,
  },
  dataItem: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: 12,
    backgroundColor: '#fff',
    borderRadius: 8,
    marginBottom: 8,
  },
  dataInfo: {
    flex: 1,
  },
  dataName: {
    fontSize: 14,
    color: '#323233',
  },
  dataPID: {
    marginTop: 2,
    fontSize: 11,
    color: '#c8c9cc',
  },
  dataValueContainer: {
    flexDirection: 'row',
    alignItems: 'baseline',
  },
  dataValue: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1989fa',
  },
  dataUnit: {
    marginLeft: 4,
    fontSize: 12,
    color: '#969799',
  },
});
