/**
 * BurstSnapshotPage - Burst 采样模式页面
 *
 * PID 选择 → 采样时长输入 → 启动/中止 → 结果表格
 * 页面离开时自动 abort，防止后台遗留 Burst 会话。
 */

import React, { useState, useEffect, useCallback, useRef } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  TextInput,
  ActivityIndicator,
  ScrollView,
  Alert,
  BackHandler,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import { useNavigation, useFocusEffect } from '@react-navigation/native';
import { useCloudBridge } from '../hooks/useCloudBridge';
import { useAppContext } from '../context/AppContext';
import { useBurstSession, BurstSessionResult } from '../hooks/useBurstSession';
import { UNITS_MAP } from '../constants/units';

interface PIDOption {
  index: number;
  name: string;
  pid: string;
  selected: boolean;
}

export default function BurstSnapshotPage() {
  const navigation = useNavigation<any>();
  const { cloudConnected } = useAppContext();
  const { getPIDList } = useCloudBridge();
  const { executeBurst, abortBurst, abortBurstAndWait, mode, error, progress } = useBurstSession();

  const [pidOptions, setPidOptions] = useState<PIDOption[]>([]);
  const [durationSec, setDurationSec] = useState('10');
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState<BurstSessionResult | null>(null);

  // 追踪 busy 状态，供生命周期钩子使用（避免闭包陷阱）
  const isBusy = mode === 'preparing' || mode === 'sampling' || mode === 'committing';
  const isBusyRef = useRef(isBusy);
  useEffect(() => { isBusyRef.current = isBusy; }, [isBusy]);

  // canStart：只有空闲态（idle/finished/aborted/error）且有选中 PID 时才可开始
  const selectedIndices = pidOptions.filter(p => p.selected).map(p => p.index);
  const canStart = selectedIndices.length > 0 && !isBusy;
  const durationMs = Math.max(1, Math.min(60, parseInt(durationSec, 10) || 10)) * 1000;

  // ── 加载 PID 列表 ──
  useEffect(() => {
    if (cloudConnected) { loadPIDList(); }
  }, [cloudConnected]);

  const loadPIDList = async () => {
    setLoading(true);
    try {
      const data = await getPIDList();
      if (data && data.length > 0) {
        const filtered = data
          .filter((item: any) => item.NM !== 'Not selected' && item.SNM !== 'Not selected' && item.name !== 'Not selected')
          .map((item: any) => ({
            index: item.originalIndex ?? 0,
            name: item.NM ?? item.name ?? `参数 ${item.originalIndex}`,
            pid: item.CMD ?? item.pid ?? '',
            selected: false,
          }));
        setPidOptions(filtered);
      }
    } catch (e: any) {
      Alert.alert('错误', e.message || '加载 PID 列表失败');
    }
    setLoading(false);
  };

  const togglePid = (index: number) => {
    setPidOptions(prev => prev.map(p =>
      p.index === index ? { ...p, selected: !p.selected } : p
    ));
  };

  const selectAll = () => setPidOptions(prev => prev.map(p => ({ ...p, selected: true })));
  const deselectAll = () => setPidOptions(prev => prev.map(p => ({ ...p, selected: false })));

  // ── 开始采样 ──
  const handleStart = async () => {
    if (!canStart) return;
    setResult(null);
    const res = await executeBurst(selectedIndices, durationMs);
    if (res) setResult(res);
  };

  // ── 离开保护：统一拦截所有导航离开，确保 abort 完成后才放行 ──
  const [leaving, setLeaving] = useState(false);
  const abortInProgressRef = useRef<Promise<void> | null>(null);

  // 统一 abort + 等待逻辑，多次调用复用同一个 Promise
  const ensureAbortComplete = useCallback((): Promise<void> => {
    if (abortInProgressRef.current) return abortInProgressRef.current;
    if (!isBusyRef.current) return Promise.resolve();

    const p = abortBurstAndWait().finally(() => {
      abortInProgressRef.current = null;
    });
    abortInProgressRef.current = p;
    return p;
  }, [abortBurstAndWait]);

  // 路径 1+2+3 统一入口：beforeRemove 拦截所有导航离开
  // 覆盖：顶部返回按钮 goBack、Android 返回键、程序化 navigate、路由移除
  const allowNextRemoveRef = useRef(false);

  useEffect(() => {
    const unsubscribe = navigation.addListener('beforeRemove', (e: any) => {
      // replay 放行：abort 完成后的 dispatch 触发的第二次 beforeRemove，直接放行
      if (allowNextRemoveRef.current) {
        allowNextRemoveRef.current = false;
        return;
      }

      if (!isBusyRef.current) return; // 非 busy 直接放行

      e.preventDefault(); // 阻止离开
      setLeaving(true);
      ensureAbortComplete().then(() => {
        setLeaving(false);
        allowNextRemoveRef.current = true; // 标记下一次 beforeRemove 放行
        navigation.dispatch(e.data.action); // replay 离开动作
      });
    });
    return unsubscribe;
  }, [navigation, ensureAbortComplete]);

  // 顶部返回按钮：直接 goBack，beforeRemove 会拦截
  const handleBack = useCallback(() => {
    navigation.goBack();
  }, [navigation]);

  // Android 返回键：转发到 goBack，beforeRemove 会拦截
  useFocusEffect(
    useCallback(() => {
      const onBackPress = () => {
        navigation.goBack();
        return true;
      };
      BackHandler.addEventListener('hardwareBackPress', onBackPress);
      return () => BackHandler.removeEventListener('hardwareBackPress', onBackPress);
    }, [navigation])
  );

  // 兜底：极端情况下组件卸载但 beforeRemove 未触发（如热重载），同步触发 abort signal
  useEffect(() => {
    return () => {
      if (isBusyRef.current) {
        abortBurst();
      }
    };
  }, [abortBurst]);

  // ── 结果表格 ──
  const getResultTable = () => {
    if (!result || result.results.length === 0) return null;

    const pidNameSet = new Set<string>();
    const cycleSet = new Set<number>();
    for (const r of result.results) {
      pidNameSet.add(r.pidName);
      cycleSet.add(r.cycleIndex);
    }
    const pidNames = Array.from(pidNameSet);
    const cycles = Array.from(cycleSet).sort((a, b) => a - b);

    const rows = cycles.map(c => {
      const row: Record<string, string> = { _cycle: String(c) };
      const cycleResults = result.results.filter(r => r.cycleIndex === c);
      const ts = cycleResults.length > 0 ? cycleResults[0].timestampMs : 0;
      row._time = ts > 0 ? new Date(ts).toISOString().substring(11, 23) : '';
      for (const pn of pidNames) {
        const pr = cycleResults.find(r => r.pidName === pn);
        if (pr && pr.parseOk) {
          const unitText = typeof pr.unit === 'number' ? (UNITS_MAP[pr.unit] || '') : '';
          row[pn] = `${pr.displayText}${unitText ? ' ' + unitText : ''}`;
        } else if (pr && !pr.parseOk) {
          row[pn] = pr.error || '解析失败';
        } else {
          row[pn] = '-';
        }
      }
      return row;
    });

    return { pidNames, rows };
  };

  const table = result ? getResultTable() : null;

  return (
    <SafeAreaView style={styles.container}>
      {/* 顶栏：返回按钮走 goBack，beforeRemove 拦截处理 abort */}
      <View style={styles.header}>
        <TouchableOpacity onPress={handleBack} style={styles.backBtn}>
          <Icon name="arrow-left" size={24} color="#333" />
        </TouchableOpacity>
        <Text style={styles.title}>Burst 采样</Text>
        <View style={{ width: 40 }} />
      </View>

      {loading ? (
        <ActivityIndicator size="large" style={{ marginTop: 40 }} />
      ) : (
        <ScrollView style={styles.content}>
          {/* PID 选择 */}
          <View style={styles.section}>
            <View style={styles.sectionHeader}>
              <Text style={styles.sectionTitle}>选择 PID ({selectedIndices.length}/{pidOptions.length})</Text>
              <View style={{ flexDirection: 'row' }}>
                <TouchableOpacity onPress={selectAll} style={styles.smallBtn}><Text style={styles.smallBtnText}>全选</Text></TouchableOpacity>
                <TouchableOpacity onPress={deselectAll} style={styles.smallBtn}><Text style={styles.smallBtnText}>清空</Text></TouchableOpacity>
              </View>
            </View>
            <View style={styles.pidGrid}>
              {pidOptions.map(p => (
                <TouchableOpacity
                  key={p.index}
                  style={[styles.pidChip, p.selected && styles.pidChipSelected]}
                  onPress={() => togglePid(p.index)}
                  disabled={isBusy}
                >
                  <Text style={[styles.pidChipText, p.selected && styles.pidChipTextSelected]} numberOfLines={1}>{p.name}</Text>
                </TouchableOpacity>
              ))}
            </View>
          </View>

          {/* 时长输入 */}
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>采样时长（秒，1-60）</Text>
            <TextInput style={styles.input} value={durationSec} onChangeText={setDurationSec} keyboardType="numeric" editable={!isBusy} />
          </View>

          {/* 控制按钮 */}
          <View style={styles.buttonRow}>
            {!isBusy ? (
              <TouchableOpacity style={[styles.startBtn, !canStart && styles.btnDisabled]} onPress={handleStart} disabled={!canStart}>
                <Icon name="play" size={20} color="#fff" />
                <Text style={styles.btnText}>开始采样</Text>
              </TouchableOpacity>
            ) : (
              <TouchableOpacity style={styles.abortBtn} onPress={abortBurst}>
                <Icon name="stop" size={20} color="#fff" />
                <Text style={styles.btnText}>中止</Text>
              </TouchableOpacity>
            )}
          </View>

          {/* 状态 */}
          {(isBusy || leaving) && (
            <View style={styles.statusBox}>
              <ActivityIndicator size="small" />
              <Text style={styles.statusText}>
                {leaving && '正在中止...'}
                {!leaving && mode === 'preparing' && '准备中...'}
                {!leaving && mode === 'sampling' && `采样中 - 周期 ${progress.cycle}`}
                {!leaving && mode === 'committing' && '提交解析中...'}
              </Text>
            </View>
          )}

          {error && <View style={styles.errorBox}><Text style={styles.errorText}>{error}</Text></View>}

          {/* 结果表格 */}
          {table && table.rows.length > 0 && (
            <View style={styles.section}>
              <Text style={styles.sectionTitle}>结果（{result!.totalCycles} 周期，{result!.results.length} 条解析）</Text>
              <ScrollView horizontal>
                <View>
                  <View style={styles.tableRow}>
                    <Text style={[styles.tableCell, styles.tableHeader, { width: 40 }]}>#</Text>
                    <Text style={[styles.tableCell, styles.tableHeader, { width: 100 }]}>时间</Text>
                    {table.pidNames.map(pn => (
                      <Text key={pn} style={[styles.tableCell, styles.tableHeader, { width: 120 }]} numberOfLines={1}>{pn}</Text>
                    ))}
                  </View>
                  {table.rows.map((row, idx) => (
                    <View key={idx} style={[styles.tableRow, idx % 2 === 1 && styles.tableRowAlt]}>
                      <Text style={[styles.tableCell, { width: 40 }]}>{row._cycle}</Text>
                      <Text style={[styles.tableCell, { width: 100 }]}>{row._time}</Text>
                      {table.pidNames.map(pn => (
                        <Text key={pn} style={[styles.tableCell, { width: 120 }]} numberOfLines={1}>{row[pn] || '-'}</Text>
                      ))}
                    </View>
                  ))}
                </View>
              </ScrollView>
            </View>
          )}

          {result && (!table || table.rows.length === 0) && (
            <View style={styles.section}><Text style={styles.statusText}>采样完成但无解析结果</Text></View>
          )}

          <View style={{ height: 40 }} />
        </ScrollView>
      )}
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f5f5f5' },
  header: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingHorizontal: 16, paddingVertical: 12, backgroundColor: '#fff', borderBottomWidth: 1, borderBottomColor: '#e0e0e0' },
  backBtn: { width: 40, alignItems: 'flex-start' },
  title: { fontSize: 18, fontWeight: '600', color: '#333' },
  content: { flex: 1, padding: 16 },
  section: { backgroundColor: '#fff', borderRadius: 8, padding: 12, marginBottom: 12 },
  sectionHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 },
  sectionTitle: { fontSize: 14, fontWeight: '600', color: '#333', marginBottom: 4 },
  smallBtn: { paddingHorizontal: 10, paddingVertical: 4, marginLeft: 8, backgroundColor: '#e8e8e8', borderRadius: 4 },
  smallBtnText: { fontSize: 12, color: '#555' },
  pidGrid: { flexDirection: 'row', flexWrap: 'wrap' },
  pidChip: { paddingHorizontal: 10, paddingVertical: 6, margin: 3, backgroundColor: '#f0f0f0', borderRadius: 16, borderWidth: 1, borderColor: '#ddd' },
  pidChipSelected: { backgroundColor: '#2196F3', borderColor: '#1976D2' },
  pidChipText: { fontSize: 12, color: '#555' },
  pidChipTextSelected: { color: '#fff' },
  input: { borderWidth: 1, borderColor: '#ddd', borderRadius: 6, padding: 10, fontSize: 16, backgroundColor: '#fafafa' },
  buttonRow: { flexDirection: 'row', justifyContent: 'center', marginBottom: 12 },
  startBtn: { flexDirection: 'row', alignItems: 'center', backgroundColor: '#4CAF50', paddingHorizontal: 24, paddingVertical: 12, borderRadius: 8 },
  abortBtn: { flexDirection: 'row', alignItems: 'center', backgroundColor: '#f44336', paddingHorizontal: 24, paddingVertical: 12, borderRadius: 8 },
  btnDisabled: { backgroundColor: '#ccc' },
  btnText: { color: '#fff', fontSize: 16, fontWeight: '600', marginLeft: 8 },
  statusBox: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', padding: 12, backgroundColor: '#E3F2FD', borderRadius: 8, marginBottom: 12 },
  statusText: { fontSize: 14, color: '#555', marginLeft: 8 },
  errorBox: { padding: 12, backgroundColor: '#FFEBEE', borderRadius: 8, marginBottom: 12 },
  errorText: { fontSize: 14, color: '#D32F2F' },
  tableRow: { flexDirection: 'row', borderBottomWidth: 1, borderBottomColor: '#eee' },
  tableRowAlt: { backgroundColor: '#fafafa' },
  tableCell: { paddingHorizontal: 6, paddingVertical: 8, fontSize: 12, color: '#333' },
  tableHeader: { fontWeight: '600', backgroundColor: '#f0f0f0' },
});
