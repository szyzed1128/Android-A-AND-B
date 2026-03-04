/**
 * ECUInfoResultPage - ECU信息结果页
 *
 * 折叠显示各 ECU 的详细信息
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
import { useNavigation, useRoute, RouteProp } from '@react-navigation/native';
import { useCloudBridge } from '../hooks/useCloudBridge';
import { RootStackParamList } from '../App';

type ECUInfoResultRouteProp = RouteProp<RootStackParamList, 'ECUInfoResult'>;

interface InfoItem {
  key: string;
  value: string;
}

interface ECUInfoResult {
  ecuIndex: number;
  ecuName: string;
  infoList: InfoItem[];
  loading: boolean;
  error?: string;
  expanded: boolean;
}

export default function ECUInfoResultPage() {
  const navigation = useNavigation<any>();
  const route = useRoute<ECUInfoResultRouteProp>();
  const { indices, ecuList } = route.params;
  const { readECUInfoAsync } = useCloudBridge();

  const [results, setResults] = useState<ECUInfoResult[]>([]);
  const [loading, setLoading] = useState(true);

  // 初始化并读取
  useEffect(() => {
    initResults();
    startReading();
  }, []);

  const initResults = () => {
    const initialResults: ECUInfoResult[] = indices.map(index => ({
      ecuIndex: index,
      ecuName: ecuList[index]?.name || `ECU ${index}`,
      infoList: [],
      loading: true,
      expanded: true, // 默认展开
    }));
    setResults(initialResults);
  };

  const startReading = async () => {
    setLoading(true);

    readECUInfoAsync(indices, {
      onProgress: (ecuIndex: number, infoList: InfoItem[]) => {
        setResults(prev => prev.map(r => {
          if (r.ecuIndex === ecuIndex) {
            return { ...r, infoList, loading: false };
          }
          return r;
        }));
      },
      onError: (ecuIndex: number, error: string) => {
        setResults(prev => prev.map(r => {
          if (r.ecuIndex === ecuIndex) {
            return { ...r, error, loading: false };
          }
          return r;
        }));
      },
      onFinish: () => {
        setLoading(false);
      },
    });
  };

  // 切换展开
  const toggleExpand = (ecuIndex: number) => {
    setResults(prev => prev.map(r => {
      if (r.ecuIndex === ecuIndex) {
        return { ...r, expanded: !r.expanded };
      }
      return r;
    }));
  };

  // 渲染 ECU 信息
  const renderECUInfo = ({ item }: { item: ECUInfoResult }) => (
    <View style={styles.ecuSection}>
      <TouchableOpacity
        style={styles.ecuHeader}
        onPress={() => toggleExpand(item.ecuIndex)}
      >
        <View style={styles.ecuHeaderLeft}>
          <Icon
            name={item.expanded ? 'chevron-down' : 'chevron-right'}
            size={20}
            color="#969799"
          />
          <Text style={styles.ecuName}>{item.ecuName}</Text>
        </View>
        {item.loading ? (
          <ActivityIndicator size="small" color="#1989fa" />
        ) : (
          <Text style={styles.ecuCount}>
            {item.infoList.length} 项
          </Text>
        )}
      </TouchableOpacity>

      {item.expanded && (
        <View style={styles.ecuContent}>
          {item.error ? (
            <View style={styles.errorContainer}>
              <Icon name="alert-circle-outline" size={20} color="#ee0a24" />
              <Text style={styles.errorText}>{item.error}</Text>
            </View>
          ) : item.loading ? (
            <View style={styles.loadingContainer}>
              <ActivityIndicator size="small" color="#1989fa" />
              <Text style={styles.loadingText}>读取中...</Text>
            </View>
          ) : item.infoList.length === 0 ? (
            <View style={styles.emptyContainer}>
              <Text style={styles.emptyText}>无信息</Text>
            </View>
          ) : (
            item.infoList.map((info, index) => (
              <View key={`${info.key}-${index}`} style={styles.infoItem}>
                <Text style={styles.infoKey}>{info.key}</Text>
                <Text style={styles.infoValue}>{info.value}</Text>
              </View>
            ))
          )}
        </View>
      )}
    </View>
  );

  const loadingCount = results.filter(r => r.loading).length;

  return (
    <SafeAreaView style={styles.container} edges={['bottom']}>
      {/* 标题栏 */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.headerButton}>
          <Icon name="arrow-left" size={24} color="#323233" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>ECU信息结果</Text>
        <TouchableOpacity onPress={startReading} style={styles.headerButton} disabled={loading}>
          <Icon name="refresh" size={24} color={loading ? '#c8c9cc' : '#1989fa'} />
        </TouchableOpacity>
      </View>

      {/* 状态栏 */}
      {loadingCount > 0 && (
        <View style={styles.statusBar}>
          <ActivityIndicator size="small" color="#1989fa" />
          <Text style={styles.statusText}>
            正在读取 {loadingCount} 个ECU...
          </Text>
        </View>
      )}

      {/* 结果列表 */}
      <FlatList
        data={results}
        keyExtractor={(item) => `ecu-${item.ecuIndex}`}
        renderItem={renderECUInfo}
        contentContainerStyle={styles.listContent}
      />
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
    justifyContent: 'center',
    padding: 12,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  statusText: {
    marginLeft: 8,
    fontSize: 14,
    color: '#666',
  },
  listContent: {
    padding: 12,
  },
  ecuSection: {
    backgroundColor: '#fff',
    borderRadius: 8,
    marginBottom: 12,
    overflow: 'hidden',
  },
  ecuHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: 12,
    backgroundColor: '#f5f6f7',
  },
  ecuHeaderLeft: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  ecuName: {
    marginLeft: 8,
    fontSize: 15,
    fontWeight: '500',
    color: '#323233',
  },
  ecuCount: {
    fontSize: 13,
    color: '#969799',
  },
  ecuContent: {
    borderTopWidth: StyleSheet.hairlineWidth,
    borderTopColor: '#eee',
  },
  errorContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
  },
  errorText: {
    marginLeft: 8,
    fontSize: 14,
    color: '#ee0a24',
  },
  loadingContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    padding: 16,
  },
  loadingText: {
    marginLeft: 8,
    fontSize: 14,
    color: '#666',
  },
  emptyContainer: {
    alignItems: 'center',
    justifyContent: 'center',
    padding: 16,
  },
  emptyText: {
    fontSize: 14,
    color: '#969799',
  },
  infoItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    padding: 12,
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  infoKey: {
    flex: 1,
    fontSize: 14,
    color: '#666',
  },
  infoValue: {
    flex: 1,
    fontSize: 14,
    color: '#323233',
    textAlign: 'right',
  },
});
