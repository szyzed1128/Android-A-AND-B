/**
 * ECUInfoResultPage - ECU信息结果页
 *
 * 用 SectionList 按 ECU 分组显示，同组内条目无分隔线，整体如连续文本
 */

import React, { useState, useEffect, useCallback } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  SectionList,
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

interface Section {
  title: string;
  data: InfoItem[];
}

export default function ECUInfoResultPage() {
  const navigation = useNavigation<any>();
  const route = useRoute<ECUInfoResultRouteProp>();
  const { indices } = route.params;
  const { readECUInfoAsync } = useCloudBridge();

  const [sections, setSections] = useState<Section[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const startReading = useCallback(() => {
    setLoading(true);
    setError(null);
    setSections([]);

    readECUInfoAsync(indices, {
      onSuccess: (data: any) => {
        if (Array.isArray(data)) {
          setSections(data as Section[]);
        }
      },
      onError: (_ecuIndex: number, err: string) => {
        setError(err);
      },
      onFinish: () => {
        setLoading(false);
      },
    });
  }, [indices]);

  useEffect(() => {
    startReading();
  }, []);

  const renderSectionHeader = ({ section }: { section: Section }) => (
    section.title ? (
      <View style={styles.sectionHeader}>
        <Text style={styles.sectionTitle}>{section.title}</Text>
      </View>
    ) : null
  );

  const renderItem = ({ item, index, section }: { item: InfoItem; index: number; section: Section }) => {
    const isLast = index === section.data.length - 1;
    return (
      <View style={[styles.row, isLast && styles.rowLast]}>
        {item.value ? (
          <Text style={styles.rowText}>
            <Text style={styles.rowKey}>{item.key}</Text>
            <Text style={styles.separator}>: </Text>
            <Text style={styles.rowValue}>{item.value}</Text>
          </Text>
        ) : (
          <Text style={styles.rowTextDesc}>{item.key}</Text>
        )}
      </View>
    );
  };

  const totalCount = sections.reduce((sum, s) => sum + s.data.length, 0);

  return (
    <SafeAreaView style={styles.container} edges={['bottom']}>
      {/* 标题栏 */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.headerButton}>
          <Icon name="arrow-left" size={24} color="#323233" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>ECU信息</Text>
        <TouchableOpacity onPress={startReading} style={styles.headerButton} disabled={loading}>
          <Icon name="refresh" size={24} color={loading ? '#c8c9cc' : '#1989fa'} />
        </TouchableOpacity>
      </View>

      {loading ? (
        <View style={styles.centerBox}>
          <ActivityIndicator size="large" color="#1989fa" />
          <Text style={styles.loadingText}>正在读取 ECU 信息...</Text>
        </View>
      ) : error ? (
        <View style={styles.centerBox}>
          <Icon name="alert-circle-outline" size={32} color="#ee0a24" />
          <Text style={styles.errorText}>{error}</Text>
          <TouchableOpacity style={styles.retryButton} onPress={startReading}>
            <Text style={styles.retryText}>重试</Text>
          </TouchableOpacity>
        </View>
      ) : totalCount === 0 ? (
        <View style={styles.centerBox}>
          <Text style={styles.emptyText}>未读取到 ECU 信息</Text>
        </View>
      ) : (
        <SectionList
          sections={sections}
          keyExtractor={(item, idx) => `${item.key}-${idx}`}
          renderItem={renderItem}
          renderSectionHeader={renderSectionHeader}
          contentContainerStyle={styles.listContent}
          stickySectionHeadersEnabled={false}
        />
      )}
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f0f1f3',
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
  listContent: {
    paddingVertical: 8,
    paddingHorizontal: 12,
  },
  /* -------- 分组标题 -------- */
  sectionHeader: {
    marginTop: 12,
    marginBottom: 0,
    paddingHorizontal: 12,
    paddingVertical: 8,
    backgroundColor: '#fff',
    borderTopLeftRadius: 8,
    borderTopRightRadius: 8,
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#e8e8e8',
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: '700',
    color: '#1a1a1a',
  },
  /* -------- 数据行（同一 ECU 组内无分隔线） -------- */
  row: {
    paddingHorizontal: 12,
    paddingVertical: 8,
    backgroundColor: '#fff',
  },
  rowLast: {
    borderBottomLeftRadius: 8,
    borderBottomRightRadius: 8,
    paddingBottom: 12,
  },
  rowText: {
    fontSize: 13,
    lineHeight: 19,
    color: '#323233',
    flexShrink: 1,
  },
  rowKey: {
    color: '#646566',
  },
  separator: {
    color: '#969799',
  },
  rowValue: {
    color: '#323233',
  },
  /* 无 value 的描述行（较长的说明文字等） */
  rowTextDesc: {
    fontSize: 13,
    lineHeight: 19,
    color: '#969799',
    fontStyle: 'italic',
  },
  /* -------- 公共状态 -------- */
  centerBox: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: 32,
  },
  loadingText: {
    marginTop: 12,
    fontSize: 14,
    color: '#666',
  },
  errorText: {
    marginTop: 8,
    fontSize: 14,
    color: '#ee0a24',
    textAlign: 'center',
  },
  emptyText: {
    fontSize: 14,
    color: '#969799',
  },
  retryButton: {
    marginTop: 16,
    paddingHorizontal: 24,
    paddingVertical: 10,
    backgroundColor: '#1989fa',
    borderRadius: 6,
  },
  retryText: {
    fontSize: 14,
    color: '#fff',
    fontWeight: '500',
  },
});
