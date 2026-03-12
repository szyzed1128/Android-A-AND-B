/**
 * DTCResultPage - 故障码结果页
 *
 * 按 ECU 分组显示故障码
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

type DTCResultRouteProp = RouteProp<RootStackParamList, 'DTCResult'>;

interface DTCItem {
  code: string;
  description: string;
  status?: string;
}

interface ECUResult {
  ecuIndex: number;
  ecuName: string;
  dtcList: DTCItem[];
  loading: boolean;
  error?: string;
}


export default function DTCResultPage() {
  const navigation = useNavigation<any>();
  const route = useRoute<DTCResultRouteProp>();
  const { indices, ecuList } = route.params;
  const { readDTCAsync } = useCloudBridge();

  const [results, setResults] = useState<ECUResult[]>([]);
  const [loading, setLoading] = useState(true);

  // 初始化并读取
  useEffect(() => {
    initResults();
    startReading();
  }, []);

  const initResults = () => {
    const initialResults: ECUResult[] = indices.map(index => ({
      ecuIndex: index,
      ecuName: ecuList[index]?.name || `ECU ${index}`,
      dtcList: [],
      loading: true,
    }));
    setResults(initialResults);
  };

  const startReading = async () => {
    setLoading(true);

    readDTCAsync(indices, {
      onProgress: (ecuIndex: number, dtcList: DTCItem[]) => {
        setResults(prev => prev.map(r => {
          if (r.ecuIndex === ecuIndex) {
            return { ...r, dtcList, loading: false };
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

  // 计算总数
  const totalDTC = results.reduce((sum, r) => sum + r.dtcList.length, 0);
  const loadingCount = results.filter(r => r.loading).length;

  // 渲染 ECU 结果
  const renderECUResult = ({ item }: { item: ECUResult }) => (
    <View style={styles.ecuSection}>
      <View style={styles.ecuHeader}>
        <Text style={styles.ecuName}>{item.ecuName}</Text>
        {item.loading ? (
          <ActivityIndicator size="small" color="#1989fa" />
        ) : (
          <Text style={styles.ecuCount}>
            {item.dtcList.length} 个故障码
          </Text>
        )}
      </View>

      {item.error ? (
        <View style={styles.errorContainer}>
          <Icon name="alert-circle-outline" size={20} color="#ee0a24" />
          <Text style={styles.errorText}>{item.error}</Text>
        </View>
      ) : item.dtcList.length === 0 && !item.loading ? (
        <View style={styles.noDTCContainer}>
          <Icon name="check-circle-outline" size={20} color="#07c160" />
          <Text style={styles.noDTCText}>无故障码</Text>
        </View>
      ) : (
        item.dtcList.map((dtc, index) => (
          <View key={`${dtc.code}-${index}`} style={styles.dtcItem}>
            <View style={styles.dtcCodeRow}>
              <Text style={styles.dtcCode}>{dtc.code}</Text>
              {dtc.status && (
                <View style={styles.statusTag}>
                  <Text style={styles.statusText}>{dtc.status}</Text>
                </View>
              )}
            </View>
            <Text style={styles.dtcDesc}>{dtc.description || '未知故障'}</Text>
          </View>
        ))
      )}
    </View>
  );

  return (
    <SafeAreaView style={styles.container} edges={['bottom']}>
      {/* 标题栏 */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.headerButton}>
          <Icon name="arrow-left" size={24} color="#323233" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>故障码结果</Text>
        <TouchableOpacity onPress={startReading} style={styles.headerButton} disabled={loading}>
          <Icon name="refresh" size={24} color={loading ? '#c8c9cc' : '#1989fa'} />
        </TouchableOpacity>
      </View>

      {/* 统计栏 */}
      <View style={styles.summaryBar}>
        <Text style={styles.summaryText}>
          共 {totalDTC} 个故障码
          {loadingCount > 0 && ` (${loadingCount} 个ECU读取中...)`}
        </Text>
      </View>

      {/* 结果列表 */}
      <FlatList
        data={results}
        keyExtractor={(item) => `ecu-${item.ecuIndex}`}
        renderItem={renderECUResult}
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
  summaryBar: {
    padding: 12,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  summaryText: {
    fontSize: 14,
    color: '#666',
    textAlign: 'center',
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
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  ecuName: {
    fontSize: 15,
    fontWeight: '500',
    color: '#323233',
  },
  ecuCount: {
    fontSize: 13,
    color: '#969799',
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
  noDTCContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    padding: 16,
  },
  noDTCText: {
    marginLeft: 8,
    fontSize: 14,
    color: '#07c160',
  },
  dtcItem: {
    padding: 12,
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  dtcCodeRow: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  dtcCode: {
    fontSize: 16,
    fontWeight: '600',
    color: '#ee0a24',
  },
  statusTag: {
    marginLeft: 8,
    paddingHorizontal: 6,
    paddingVertical: 2,
    backgroundColor: '#fff7cc',
    borderRadius: 4,
  },
  statusText: {
    fontSize: 10,
    color: '#ff976a',
  },
  dtcDesc: {
    marginTop: 4,
    fontSize: 13,
    color: '#666',
    lineHeight: 18,
  },
});
