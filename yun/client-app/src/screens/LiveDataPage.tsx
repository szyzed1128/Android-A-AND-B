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
  rawData?: any;  // 保存 A 端原始数据（含 Value 对象、Units 等）
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

  const readingRef = useRef(false);  // 用于 useFocusEffect 避免闭包陷阱

  // 同步 readingRef
  useEffect(() => {
    readingRef.current = reading;
  }, [reading]);

  // 加载 PID 列表
  useEffect(() => {
    if (cloudConnected) {
      loadPIDList();
    }
  }, [cloudConnected]);

  // 页面失焦时停止读取（用 ref 避免闭包陷阱）
  useFocusEffect(
    useCallback(() => {
      return () => {
        if (readingRef.current) {
          stopReadPIDs();
          setReading(false);
          readingRef.current = false;
        }
      };
    }, [stopReadPIDs])  // 依赖 stopReadPIDs，确保 cleanup 中可用
  );

  // 监听 PID 值变化
  useEffect(() => {
    const unsubscribe = onPIDValueChanged((data: any) => {
      // A 端发来的是完整 PIDItem 对象（含 NM, Value, Units 等）
      const pidName = data.NM ?? data.name;
      if (!pidName) return;

      // 格式化 Value 字段
      let formattedValue = '-';
      const rawValue = data.Value ?? data.value;

      if (rawValue !== null && rawValue !== undefined) {
        if (typeof rawValue === 'object') {
          // 对象类型（如 MonitorStatus）：转为多行文本
          if (rawValue.MIL_ON !== undefined) {
            // MonitorStatus 类型
            const lines = [`MIL:${rawValue.MIL_ON ? 'ON' : 'OFF'}`, `DTC:${rawValue.DTCs}`];
            if (rawValue.ECUTests && Array.isArray(rawValue.ECUTests)) {
              rawValue.ECUTests.forEach((test: any) => {
                lines.push(`${test.Name}: ${test.Available ? 'Avail' : 'N/A'}/${test.Complete ? 'Done' : 'Pending'}`);
              });
            }
            formattedValue = lines.join('\n');
          } else {
            formattedValue = JSON.stringify(rawValue);
          }
        } else if (typeof rawValue === 'number') {
          // 数字：保留最多 4 位小数
          const decimalPlaces = (rawValue.toString().split('.')[1] || '').length;
          formattedValue = decimalPlaces > 4
            ? (Math.round(rawValue * 10000) / 10000).toString()
            : rawValue.toString();
        } else {
          formattedValue = String(rawValue);
        }
      }

      // 更新显示的 PID 值（用 name 匹配）
      setPIDList(prev => prev.map(p => {
        if (p.name === pidName) {
          return {
            ...p,
            value: formattedValue,
            rawData: data,  // 保存原始数据
          };
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
        // 过滤掉 "Not selected" 占位符，用 NM 作为 name，保留原始索引
        const filtered = data
          .filter((item: any) => item.NM !== 'Not selected' && item.SNM !== 'Not selected' && item.name !== 'Not selected')
          .map((item: any) => ({
            index: item.originalIndex ?? 0,
            pid: item.CMD ?? item.pid ?? `PID_${item.originalIndex}`,
            name: item.NM ?? item.name ?? `参数 ${item.originalIndex}`,
            value: '-',
            unit: item.Units ?? item.unit ?? 0,
          }));
        setPIDList(filtered);
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

  // 切换页面时更新读取的 PID（200ms 防抖，合并快速翻页，避免短时多次发送）
  useEffect(() => {
    if (!reading || currentPageIndices.length === 0) return;
    const timer = setTimeout(() => {
      startReadPIDs(currentPageIndices);
    }, 200);
    return () => clearTimeout(timer);
  }, [currentPage, reading]);

  // 开始/停止读取
  const handleToggleReading = async () => {
    if (reading) {
      setReading(false);  // 立即更新 UI，不等待 A 端确认（避免按钮卡死 30s）
      stopReadPIDs();     // 后台发送停止命令（不阻塞）
    } else {
      setReading(true);
      // useEffect([currentPage, reading]) 会监听 reading 变化并调用 startReadPIDs，无需此处重复调用
    }
  };

  // 上一页
  const handlePrevPage = () => {
    if (currentPage > 0) {
      if (reading) stopReadPIDs();  // 先停止当前页，useEffect 200ms 后再 start 新页
      setCurrentPage(prev => prev - 1);
    }
  };

  // 下一页
  const handleNextPage = () => {
    if (currentPage < totalPages - 1) {
      if (reading) stopReadPIDs();  // 先停止当前页，useEffect 200ms 后再 start 新页
      setCurrentPage(prev => prev + 1);
    }
  };

  // 获取单位文本
  const getUnitText = (unitCode: number): string => {
    return UNITS_MAP[unitCode] || '';
  };

  // 渲染数据项
  const renderItem = ({ item }: { item: PIDItem }) => {
    const isMultiline = item.value.includes('\n');
    const unitText = getUnitText(item.unit);

    return (
      <View style={styles.dataItem}>
        <View style={styles.dataInfo}>
          <Text style={styles.dataName} numberOfLines={2}>{item.name}</Text>
          <Text style={styles.dataPID}>{item.pid}</Text>
        </View>
        <View style={styles.dataValueContainer}>
          {isMultiline ? (
            <View style={styles.multilineValue}>
              {item.value.split('\n').map((line, idx) => (
                <Text key={idx} style={[styles.dataValue, styles.multilineText]}>
                  {line}
                </Text>
              ))}
            </View>
          ) : (
            <>
              <Text style={[
                styles.dataValue,
                item.value === '-' && styles.dataValuePending
              ]}>
                {item.value}
              </Text>
              {unitText && <Text style={styles.dataUnit}>{unitText}</Text>}
            </>
          )}
        </View>
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
    alignItems: 'flex-start',  // 改为 flex-start 支持多行内容
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
  multilineValue: {
    alignItems: 'flex-end',
  },
  multilineText: {
    fontSize: 12,
    lineHeight: 18,
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
