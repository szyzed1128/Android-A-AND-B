/**
 * VehicleConfigPage - 车辆配置页
 *
 * 新架构：
 * - 品牌/车型目录来自调度层 catalog API
 * - 页面只保存用户选择，不直接对 A 端执行 applyProfile
 * - 真正申请实例时，由调度层把选择应用到分配到的实例
 */

import React, { useState, useEffect } from 'react';
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
import { useSchedulerActions } from '../hooks/useScheduler';
import { SchedulerCatalogProfile } from '../services/SchedulerClient';

export default function VehicleConfigPage() {
  const navigation = useNavigation<any>();
  const { setSelectedProfile, schedulerReady } = useAppContext();
  const { syncCar, getCatalogBrands, getCatalogProfiles } = useSchedulerActions();

  const [brands, setBrands] = useState<string[]>([]);
  const [selectedBrand, setSelectedBrand] = useState<string | null>(null);
  const [profiles, setProfiles] = useState<SchedulerCatalogProfile[]>([]);
  const [searchKeyword, setSearchKeyword] = useState('');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!schedulerReady) {
      setBrands([]);
      setProfiles([]);
      setSelectedBrand(null);
      return;
    }

    loadBrands();
  }, [schedulerReady]);

  const loadBrands = async () => {
    setLoading(true);
    try {
      const data = await getCatalogBrands();
      setBrands(data || []);
    } catch (err: any) {
      console.error('[VehicleConfig] loadBrands 失败:', err);
      Alert.alert('错误', err.message || '读取品牌列表失败');
    } finally {
      setLoading(false);
    }
  };

  const loadProfiles = async (brand: string) => {
    setLoading(true);
    try {
      const data = await getCatalogProfiles(brand);
      setProfiles(data || []);
    } catch (err: any) {
      console.error('[VehicleConfig] loadProfiles 失败:', err);
      Alert.alert('错误', err.message || '读取车型配置失败');
    } finally {
      setLoading(false);
    }
  };

  const handleBrandSelect = (brand: string) => {
    setSelectedBrand(brand);
    setSearchKeyword('');
    loadProfiles(brand);
  };

  const handleProfileSelect = async (profile: SchedulerCatalogProfile) => {
    if (!selectedBrand) return;

    setLoading(true);
    try {
      await syncCar(selectedBrand, profile.profileIndex, profile.name);
      setSelectedProfile({
        brand: selectedBrand,
        name: profile.name,
        profileIndex: profile.profileIndex,
      });
      Alert.alert('成功', '车型已保存，申请实例时会自动应用到分配的 A 端实例');
      setTimeout(() => navigation.goBack(), 300);
    } catch (err: any) {
      Alert.alert('错误', err.message || '保存车型失败');
    } finally {
      setLoading(false);
    }
  };

  const handleBack = () => {
    if (selectedBrand) {
      setSelectedBrand(null);
      setProfiles([]);
      return;
    }
    navigation.goBack();
  };

  const filteredBrands = brands.filter((brand) =>
    brand.toLowerCase().includes(searchKeyword.toLowerCase())
  );

  if (!schedulerReady) {
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
          <Icon name="server-network-off" size={48} color="#c8c9cc" />
          <Text style={styles.emptyText}>调度层尚未就绪</Text>
          <Text style={styles.emptyHint}>请先回首页输入服务器 IP 并完成调度初始化</Text>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.container} edges={['bottom']}>
      <View style={styles.header}>
        <TouchableOpacity onPress={handleBack} style={styles.headerButton}>
          <Icon name="arrow-left" size={24} color="#323233" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>
          {selectedBrand ? `选择配置 - ${selectedBrand}` : '选择品牌'}
        </Text>
        <View style={styles.headerButton} />
      </View>

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

      {loading && (
        <View style={styles.loadingBar}>
          <ActivityIndicator size="small" color="#1989fa" />
          <Text style={styles.loadingText}>加载中...</Text>
        </View>
      )}

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
                <Text style={styles.emptyText}>暂无品牌数据</Text>
              </View>
            ) : null
          }
        />
      )}

      {selectedBrand && (
        <FlatList
          data={profiles}
          keyExtractor={(item) => `${item.profileIndex}-${item.name}`}
          renderItem={({ item }) => (
            <TouchableOpacity
              style={styles.listItem}
              onPress={() => handleProfileSelect(item)}
            >
              <View style={styles.profileInfo}>
                <Text style={styles.listItemText}>{item.name}</Text>
                {item.description ? (
                  <Text style={styles.profileDesc}>{item.description}</Text>
                ) : null}
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
    color: '#969799',
    fontSize: 13,
  },
  listItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    backgroundColor: '#fff',
    paddingHorizontal: 16,
    paddingVertical: 14,
    borderBottomWidth: StyleSheet.hairlineWidth,
    borderBottomColor: '#f2f3f5',
  },
  listItemText: {
    fontSize: 14,
    color: '#323233',
  },
  profileInfo: {
    flex: 1,
    marginRight: 12,
  },
  profileDesc: {
    marginTop: 4,
    fontSize: 12,
    color: '#969799',
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingVertical: 80,
  },
  emptyText: {
    marginTop: 12,
    fontSize: 16,
    color: '#969799',
  },
  emptyHint: {
    marginTop: 8,
    fontSize: 13,
    color: '#c8c9cc',
    textAlign: 'center',
    paddingHorizontal: 32,
    lineHeight: 20,
  },
});
