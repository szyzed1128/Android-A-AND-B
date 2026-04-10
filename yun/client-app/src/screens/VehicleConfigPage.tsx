/**
 * VehicleConfigPage - 车辆配置页
 *
 * 品牌选择 → 配置选择两步流程
 */

import React, { useState, useEffect, useCallback } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  FlatList,
  TextInput,
  ActivityIndicator,
  Alert,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import { useNavigation } from '@react-navigation/native';
import { useAppContext } from '../context/AppContext';
import { useCloudBridge } from '../hooks/useCloudBridge';
import { useSchedulerActions } from '../hooks/useScheduler';
import { Profile } from '../services/CloudBridge';

export default function VehicleConfigPage() {
  const navigation = useNavigation<any>();
  const { setSelectedProfile, cloudConnected, appStartTime } = useAppContext();
  const { getBrands, getProfiles, applyProfile } = useCloudBridge();
  const { syncCar } = useSchedulerActions();

  const [brands, setBrands] = useState<string[]>([]);
  const [selectedBrand, setSelectedBrand] = useState<string | null>(null);
  const [profiles, setProfiles] = useState<Profile[]>([]);
  const [searchKeyword, setSearchKeyword] = useState('');
  const [loading, setLoading] = useState(false);
  const [isInitializing, setIsInitializing] = useState(true);
  const [initProgress, setInitProgress] = useState(0);

  // 初始化等待（后端需要约20秒加载）
  useEffect(() => {
    const SAFE_THRESHOLD = 20 * 1000;
    const elapsed = Date.now() - appStartTime;
    const remaining = Math.max(0, SAFE_THRESHOLD - elapsed);

    if (remaining > 0) {
      const interval = setInterval(() => {
        const now = Date.now() - appStartTime;
        const progress = Math.min(100, (now / SAFE_THRESHOLD) * 100);
        setInitProgress(progress);

        if (progress >= 100) {
          clearInterval(interval);
          setIsInitializing(false);
        }
      }, 100);

      return () => clearInterval(interval);
    } else {
      setIsInitializing(false);
    }
  }, [appStartTime]);

  // 加载品牌列表
  useEffect(() => {
    if (!isInitializing && cloudConnected) {
      loadBrands();
    }
  }, [isInitializing, cloudConnected]);

  const loadBrands = async () => {
    setLoading(true);
    try {
      const data = await getBrands();
      setBrands(data || []);
    } catch (e) {
      console.error('Load brands error:', e);
    }
    setLoading(false);
  };

  const loadProfiles = async (brand: string) => {
    setLoading(true);
    try {
      const data = await getProfiles(brand);
      setProfiles(data || []);
    } catch (e) {
      console.error('Load profiles error:', e);
    }
    setLoading(false);
  };

  // 选择品牌
  const handleBrandSelect = (brand: string) => {
    setSelectedBrand(brand);
    setSearchKeyword('');
    loadProfiles(brand);
  };

  // 选择配置
  const handleProfileSelect = async (profile: Profile, index: number) => {
    if (!selectedBrand) return;

    setLoading(true);
    try {
      await applyProfile(selectedBrand, index);
      setSelectedProfile({ brand: selectedBrand, name: profile.Name });
      // 同步车型到调度后端（异步，不阻塞 UI）
      syncCar(selectedBrand, profile.Name).catch(e =>
        console.warn('[VehicleConfig] syncCar 失败:', e.message)
      );
      Alert.alert('成功', '配置已应用');
      setTimeout(() => navigation.goBack(), 500);
    } catch (e) {
      Alert.alert('错误', '应用配置失败');
    }
    setLoading(false);
  };

  // 返回处理
  const handleBack = () => {
    if (selectedBrand) {
      setSelectedBrand(null);
      setProfiles([]);
    } else {
      navigation.goBack();
    }
  };

  // 过滤品牌
  const filteredBrands = brands.filter(brand =>
    brand.toLowerCase().includes(searchKeyword.toLowerCase())
  );

  // 初始化中
  if (isInitializing) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.header}>
          <TouchableOpacity onPress={() => navigation.goBack()} style={styles.headerButton}>
            <Icon name="arrow-left" size={24} color="#323233" />
          </TouchableOpacity>
          <Text style={styles.headerTitle}>车辆配置</Text>
          <View style={styles.headerButton} />
        </View>
        <View style={styles.initContainer}>
          <ActivityIndicator size="large" color="#1989fa" />
          <Text style={styles.initText}>正在初始化后端...</Text>
          <View style={styles.progressBar}>
            <View style={[styles.progressFill, { width: `${initProgress}%` }]} />
          </View>
          <Text style={styles.initHint}>{Math.round(initProgress)}%</Text>
        </View>
      </SafeAreaView>
    );
  }

  // 未连接云端
  if (!cloudConnected) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.header}>
          <TouchableOpacity onPress={() => navigation.goBack()} style={styles.headerButton}>
            <Icon name="arrow-left" size={24} color="#323233" />
          </TouchableOpacity>
          <Text style={styles.headerTitle}>车辆配置</Text>
          <View style={styles.headerButton} />
        </View>
        <View style={styles.emptyContainer}>
          <Icon name="cloud-off-outline" size={48} color="#c8c9cc" />
          <Text style={styles.emptyText}>未连接云端服务</Text>
          <Text style={styles.emptyHint}>请先在首页连接云端</Text>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.container} edges={['bottom']}>
      {/* 标题栏 */}
      <View style={styles.header}>
        <TouchableOpacity onPress={handleBack} style={styles.headerButton}>
          <Icon name="arrow-left" size={24} color="#323233" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>
          {selectedBrand ? `选择配置 - ${selectedBrand}` : '选择品牌'}
        </Text>
        <View style={styles.headerButton} />
      </View>

      {/* 搜索栏（仅品牌页） */}
      {!selectedBrand && (
        <View style={styles.searchBar}>
          <Icon name="magnify" size={20} color="#969799" />
          <TextInput
            style={styles.searchInput}
            value={searchKeyword}
            onChangeText={setSearchKeyword}
            placeholder="搜索品牌"
            placeholderTextColor="#c8c9cc"
          />
          {searchKeyword.length > 0 && (
            <TouchableOpacity onPress={() => setSearchKeyword('')}>
              <Icon name="close-circle" size={18} color="#c8c9cc" />
            </TouchableOpacity>
          )}
        </View>
      )}

      {/* 加载中 */}
      {loading && (
        <View style={styles.loadingBar}>
          <ActivityIndicator size="small" color="#1989fa" />
          <Text style={styles.loadingText}>加载中...</Text>
        </View>
      )}

      {/* 品牌列表 */}
      {!selectedBrand && (
        <FlatList
          data={filteredBrands}
          keyExtractor={(item) => item}
          renderItem={({ item }) => (
            <TouchableOpacity
              style={styles.listItem}
              onPress={() => handleBrandSelect(item)}
            >
              <Text style={styles.listItemText}>{item}</Text>
              <Icon name="chevron-right" size={20} color="#c8c9cc" />
            </TouchableOpacity>
          )}
          ListEmptyComponent={
            !loading ? (
              <View style={styles.emptyContainer}>
                <Text style={styles.emptyText}>暂无数据</Text>
              </View>
            ) : null
          }
        />
      )}

      {/* 配置列表 */}
      {selectedBrand && (
        <FlatList
          data={profiles}
          keyExtractor={(item, index) => `${item.Name}-${index}`}
          renderItem={({ item, index }) => (
            <TouchableOpacity
              style={styles.listItem}
              onPress={() => handleProfileSelect(item, index)}
            >
              <View style={styles.profileInfo}>
                <Text style={styles.listItemText}>{item.Name}</Text>
                {item.Description && (
                  <Text style={styles.profileDesc}>{item.Description}</Text>
                )}
              </View>
              <Icon name="chevron-right" size={20} color="#c8c9cc" />
            </TouchableOpacity>
          )}
          ListEmptyComponent={
            !loading ? (
              <View style={styles.emptyContainer}>
                <Text style={styles.emptyText}>暂无配置</Text>
              </View>
            ) : null
          }
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
  searchBar: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#fff',
    paddingHorizontal: 12,
    paddingVertical: 8,
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  searchInput: {
    flex: 1,
    marginLeft: 8,
    fontSize: 14,
    color: '#323233',
    padding: 8,
  },
  loadingBar: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    padding: 12,
    backgroundColor: '#fff',
  },
  loadingText: {
    marginLeft: 8,
    fontSize: 14,
    color: '#666',
  },
  listItem: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: 16,
    backgroundColor: '#fff',
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#eee',
  },
  listItemText: {
    fontSize: 15,
    color: '#323233',
  },
  profileInfo: {
    flex: 1,
  },
  profileDesc: {
    fontSize: 12,
    color: '#969799',
    marginTop: 4,
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
    marginTop: 12,
  },
  emptyHint: {
    fontSize: 12,
    color: '#c8c9cc',
    marginTop: 4,
  },
  initContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: 40,
  },
  initText: {
    fontSize: 16,
    color: '#323233',
    marginTop: 16,
  },
  initHint: {
    fontSize: 14,
    color: '#969799',
    marginTop: 8,
  },
  progressBar: {
    width: '80%',
    height: 4,
    backgroundColor: '#ebedf0',
    borderRadius: 2,
    marginTop: 16,
    overflow: 'hidden',
  },
  progressFill: {
    height: '100%',
    backgroundColor: '#1989fa',
  },
});
