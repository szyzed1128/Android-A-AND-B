/**
 * LiveDataPage - 实时数据页
 *
 * 分页显示实时 PID 数据
 */

import React, { useState, useEffect, useCallback, useRef } from 'react';
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
import { useNavigation, useFocusEffect } from '@react-navigation/native';
import { useCloudBridge } from '../hooks/useCloudBridge';
import { useAppContext } from '../context/AppContext';
import { UNITS_MAP } from '../constants/units';

interface PIDItem {
  index: number;
  pid: string;
  name: string;
  value: string;
  unit: number;
}

const PAGE_SIZE = 10;

export default function LiveDataPage() {
  const navigation = useNavigation<any>();
  const { cloudConnected } = useAppContext();
  const { getPIDList, startReadPIDs, stopReadPIDs, onPIDValueChanged } = useCloudBridge();

  const [pidList, setPIDList] = useState<PIDItem[]>([]);
  const [currentPage, setCurrentPage] = useState(0);
  const [loading, setLoading] = useState(true);
  const [reading, setReading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const pidValuesRef = useRef<Map<number, string>>(new Map());

  // 加载 PID 列表
  useEffect(() => {
    if (cloudConnected) {
      loadPIDList();
    }
  }, [cloudConnected]);

  // 页面失焦时停止读取
  useFocusEffect(
    useCallback(() => {
      return () => {
        if (reading) {
          stopReadPIDs();
          setReading(false);
        }
      };
    }, [reading])
  );

  // 监听 PID 值变化
  useEffect(() => {
    const unsubscribe = onPIDValueChanged((pidIndex: number, value: string) => {
      pidValuesRef.current.set(pidIndex, value);

      // 更新显示的 PID 值
      setPIDList(prev => prev.map(p => {
        if (p.index === pidIndex) {
          return { ...p, value };
        }
        return p;
      }));
    });

    return () => {
      unsubscribe?.();
    };
  }, [onPIDValueChanged]);

  const loadPIDList = async () => {
    setLoading(true);
    setError(null);

    try {
      const data = await getPIDList();
      if (data && data.length > 0) {
        setPIDList(data.map((item: any, index: number) => ({
          index,
          pid: item.pid || `PID_${index}`,
          name: item.name || `参数 ${index}`,
          value: '-',
          unit: item.unit || 0,
        })));
      } else {
        setPIDList([]);
      }
    } catch (e: any) {
      setError(e.message || '加载失败');
    }

    setLoading(false);
  };

  // 计算分页
  const totalPages = Math.ceil(pidList.length / PAGE_SIZE);
  const startIndex = currentPage * PAGE_SIZE;
  const endIndex = Math.min(startIndex + PAGE_SIZE, pidList.length);
  const currentPageData = pidList.slice(startIndex, endIndex);
  const currentPageIndices = currentPageData.map(p => p.index);

  // 切换页面时更新读取的 PID
  useEffect(() => {
    if (reading && currentPageIndices.length > 0) {
      startReadPIDs(currentPageIndices);
    }
  }, [currentPage, reading]);

  // 开始/停止读取
  const handleToggleReading = async () => {
    if (reading) {
      await stopReadPIDs();
      setReading(false);
    } else {
      setReading(true);
      // useEffect([currentPage, reading]) 会监听 reading 变化并调用 startReadPIDs，无需此处重复调用
    }
  };

  // 上一页
  const handlePrevPage = () => {
    if (currentPage > 0) {
      setCurrentPage(prev => prev - 1);
    }
  };

  // 下一页
  const handleNextPage = () => {
    if (currentPage < totalPages - 1) {
      setCurrentPage(prev => prev + 1);
    }
  };

  // 获取单位文本
  const getUnitText = (unitCode: number): string => {
    return UNITS_MAP[unitCode] || '';
  };

  // 渲染数据项
  const renderItem = ({ item }: { item: PIDItem }) => (
    <View style={styles.dataItem}>
      <View style={styles.dataInfo}>
        <Text style={styles.dataName} numberOfLines={1}>{item.name}</Text>
        <Text style={styles.dataPID}>{item.pid}</Text>
      </View>
      <View style={styles.dataValueContainer}>
        <Text style={[
          styles.dataValue,
          item.value === '-' && styles.dataValuePending
        ]}>
          {item.value}
        </Text>
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
        <Text style={styles.headerTitle}>实时数据</Text>
        <TouchableOpacity
          onPress={loadPIDList}
          style={styles.headerButton}
          disabled={loading || reading}
        >
          <Icon
            name="refresh"
            size={24}
            color={(loading || reading) ? '#c8c9cc' : '#1989fa'}
          />
        </TouchableOpacity>
      </View>

      {/* 内容区域 */}
      {loading ? (
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color="#1989fa" />
          <Text style={styles.loadingText}>正在加载参数列表...</Text>
        </View>
      ) : error ? (
        <View style={styles.errorContainer}>
          <Icon name="alert-circle-outline" size={48} color="#ee0a24" />
          <Text style={styles.errorText}>{error}</Text>
          <TouchableOpacity
            style={styles.retryButton}
            onPress={loadPIDList}
          >
            <Text style={styles.retryButtonText}>重试</Text>
          </TouchableOpacity>
        </View>
      ) : pidList.length === 0 ? (
        <View style={styles.emptyContainer}>
          <Icon name="gauge-empty" size={48} color="#c8c9cc" />
          <Text style={styles.emptyText}>无可用参数</Text>
        </View>
      ) : (
        <>
          {/* 状态栏 */}
          <View style={styles.statusBar}>
            <View style={styles.statusInfo}>
              <View style={[
                styles.statusDot,
                { backgroundColor: reading ? '#07c160' : '#969799' }
              ]} />
              <Text style={styles.statusText}>
                {reading ? '正在读取...' : '已停止'}
              </Text>
            </View>
            <Text style={styles.pageInfo}>
              {startIndex + 1}-{endIndex} / {pidList.length}
            </Text>
          </View>

          {/* 数据列表 */}
          <FlatList
            data={currentPageData}
            keyExtractor={(item) => `pid-${item.index}`}
            renderItem={renderItem}
            contentContainerStyle={styles.listContent}
          />

          {/* 分页控制 */}
          <View style={styles.pagination}>
            <TouchableOpacity
              style={[styles.pageButton, currentPage === 0 && styles.pageButtonDisabled]}
              onPress={handlePrevPage}
              disabled={currentPage === 0}
            >
              <Icon
                name="chevron-left"
                size={24}
                color={currentPage === 0 ? '#c8c9cc' : '#323233'}
              />
              <Text style={[
                styles.pageButtonText,
                currentPage === 0 && styles.pageButtonTextDisabled
              ]}>
                上一页
              </Text>
            </TouchableOpacity>

            <Text style={styles.pageNumber}>
              {currentPage + 1} / {totalPages}
            </Text>

            <TouchableOpacity
              style={[styles.pageButton, currentPage >= totalPages - 1 && styles.pageButtonDisabled]}
              onPress={handleNextPage}
              disabled={currentPage >= totalPages - 1}
            >
              <Text style={[
                styles.pageButtonText,
                currentPage >= totalPages - 1 && styles.pageButtonTextDisabled
              ]}>
                下一页
              </Text>
              <Icon
                name="chevron-right"
                size={24}
                color={currentPage >= totalPages - 1 ? '#c8c9cc' : '#323233'}
              />
            </TouchableOpacity>
          </View>
        </>
      )}

      {/* 底部按钮 */}
      {!loading && !error && pidList.length > 0 && (
        <View style={styles.bottomBar}>
          <TouchableOpacity
            style={[styles.controlButton, reading && styles.stopButton]}
            onPress={handleToggleReading}
          >
            <Icon
              name={reading ? 'stop' : 'play'}
              size={20}
              color="#fff"
            />
            <Text style={styles.controlButtonText}>
              {reading ? '停止读取' : '开始读取'}
            </Text>
          </TouchableOpacity>
        </View>
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
  statusBar: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: 12,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  statusInfo: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  statusDot: {
    width: 8,
    height: 8,
    borderRadius: 4,
    marginRight: 8,
  },
  statusText: {
    fontSize: 14,
    color: '#666',
  },
  pageInfo: {
    fontSize: 13,
    color: '#969799',
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
    marginRight: 12,
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
    fontSize: 20,
    fontWeight: '600',
    color: '#1989fa',
  },
  dataValuePending: {
    color: '#c8c9cc',
  },
  dataUnit: {
    marginLeft: 4,
    fontSize: 12,
    color: '#969799',
  },
  pagination: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: 12,
    backgroundColor: '#fff',
    borderTopWidth: StyleSheet.hairlineWidth,
    borderTopColor: '#eee',
  },
  pageButton: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 8,
  },
  pageButtonDisabled: {
    opacity: 0.5,
  },
  pageButtonText: {
    fontSize: 14,
    color: '#323233',
  },
  pageButtonTextDisabled: {
    color: '#c8c9cc',
  },
  pageNumber: {
    fontSize: 14,
    color: '#666',
  },
  bottomBar: {
    padding: 16,
    backgroundColor: '#fff',
    borderTopWidth: StyleSheet.hairlineWidth,
    borderTopColor: '#eee',
  },
  controlButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#1989fa',
    paddingVertical: 14,
    borderRadius: 8,
  },
  stopButton: {
    backgroundColor: '#ee0a24',
  },
  controlButtonText: {
    marginLeft: 8,
    color: '#fff',
    fontSize: 16,
    fontWeight: '500',
  },
});
