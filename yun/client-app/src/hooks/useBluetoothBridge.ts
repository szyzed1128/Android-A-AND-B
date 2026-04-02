/**
 * useBluetoothBridge - 蓝牙桥接 Hook
 *
 * 连接 CloudBridge 和本地蓝牙：
 * - 当云端A发送蓝牙命令时，转发到本地蓝牙
 * - 当本地蓝牙收到数据时，发送给云端A
 *
 * 数据流：
 * A(云端) → WebSocket → B(CloudBridge) → 本地蓝牙 → ELM327
 * ELM327 响应 → 本地蓝牙 → B(CloudBridge) → WebSocket → A(云端)
 */

import { useEffect, useRef } from 'react';
import { Buffer } from 'buffer';
import CloudBridge from '../services/CloudBridge';
import { useLocalBluetooth, getBluetoothGateway } from './useLocalBluetooth';
import { useAppContext } from '../context/AppContext';

/**
 * ELM327 命令覆盖：拦截 ATST 和 ATAT，解决 NRC 0x78 (ResponsePending) 问题
 *
 * 根因：ELM327 v1.5 + ATAT1（自适应超时）会根据之前的快速响应（如 0100 约50ms）
 * 缩短有效等待窗口。当 ECU 在 ~80ms 发出 NRC 0x78 后，ELM327 不等实际 DTC
 * 数据（通常需要 300-500ms），直接将 NRC 返回给主机，导致故障码读取为空。
 *
 * 修复方案（两步缺一不可）：
 *  1. ATST：替换为更大的超时值，让 ELM327 有足够时间等待 ECU 的延迟响应
 *  2. ATAT1 → ATAT0：关闭自适应超时，使 ELM327 严格使用固定 ATST 值
 *     （若保留 ATAT1，自适应机制会把有效超时缩短到 ~100ms，ATST 增大无效）
 *
 * @param base64Data  原始 base64 编码的 AT 命令
 * @param minTimeoutMs  最小超时（ms），≤400 表示不覆盖
 * @returns 可能已替换的 base64 数据
 */
function applyElmTimeoutOverride(base64Data: string, minTimeoutMs: number): string {
  // 400ms 以下不拦截（即"默认"档）
  if (minTimeoutMs <= 400) return base64Data;

  try {
    const decoded = Buffer.from(base64Data, 'base64').toString('ascii');

    // --- 拦截 ATST：将过小的超时值替换为配置的最小值 ---
    const ststMatch = decoded.match(/^AT\s*ST\s*([0-9A-Fa-f]{1,2})\s*\r?$/i);
    if (ststMatch) {
      const currentValue = parseInt(ststMatch[1], 16);
      const targetValue = Math.min(0xFF, Math.ceil(minTimeoutMs / 4));
      if (currentValue < targetValue) {
        const hexStr = targetValue.toString(16).toUpperCase().padStart(2, '0');
        const newCmd = `ATST${hexStr}\r`;
        console.log(
          `[ATSTIntercept] ${decoded.replace(/\r/g, '\\r')} → ${newCmd.replace(/\r/g, '\\r')} ` +
          `(${currentValue * 4}ms → ${targetValue * 4}ms)`
        );
        return Buffer.from(newCmd, 'ascii').toString('base64');
      }
    }

    // --- 拦截 ATAT1/ATAT2 → ATAT0：关闭自适应超时 ---
    // 必须与 ATST 增大配合使用：若保留 ATAT1，自适应机制会把有效等待窗口
    // 缩短到 ~100ms，使增大 ATST 的效果完全失效
    const atatMatch = decoded.match(/^AT\s*AT\s*([12])\s*\r?$/i);
    if (atatMatch) {
      const newCmd = `ATAT0\r`;
      console.log(
        `[ATATIntercept] ${decoded.replace(/\r/g, '\\r')} → ${newCmd.replace(/\r/g, '\\r')} ` +
        `(关闭自适应超时，配合 ATST 修复 NRC 0x78 问题)`
      );
      return Buffer.from(newCmd, 'ascii').toString('base64');
    }
  } catch {
    // 解析失败，返回原始数据
  }
  return base64Data;
}

let bridgeWireTxCounter = 0;
let bridgeWireRxCounter = 0;

function formatBridgeWireText(text: string, maxLen = 600): string {
  const normalized = (text || '').replace(/\r/g, '\\r').replace(/\n/g, '\\n');
  return normalized.length > maxLen ? normalized.slice(0, maxLen) + '…' : normalized;
}

export function useBluetoothBridge(options?: {
  onConnectionLost?: (sessionId: string, reason: string) => void;
}) {
  const { connectDevice, disconnectDevice, sendData, startScan, stopScan } = useLocalBluetooth();
  const { elmTimeoutMs } = useAppContext();
  const unsubscribeRef = useRef<Array<() => void>>([]);

  // 用 ref 持有最新的超时值，避免 useEffect 的 stale closure 问题
  const elmTimeoutMsRef = useRef(elmTimeoutMs);
  useEffect(() => {
    elmTimeoutMsRef.current = elmTimeoutMs;
    console.log(`[BluetoothBridge] ELM 超时已更新: ${elmTimeoutMs}ms`);
  }, [elmTimeoutMs]);

  useEffect(() => {
    console.log('[BluetoothBridge] 初始化蓝牙桥接');

    // 设置 CloudBridge 的蓝牙回调

    // 1. 处理云端A发送的数据转发请求（A→B→ELM327）
    CloudBridge.onBluetoothSendRequest = async (base64Data: string) => {
      // 应用 ATST 超时覆盖（在日志和发送之前）
      const overridden = applyElmTimeoutOverride(base64Data, elmTimeoutMsRef.current);

      const txSeq = ++bridgeWireTxCounter;

      // 调试：解码并显示发送的 AT 命令
      try {
        const decoded = Buffer.from(overridden, 'base64').toString('utf-8');
        const displayText = formatBridgeWireText(decoded);
        console.log(`[BluetoothBridge] A→B→ELM327: ${displayText} (len=${overridden.length})`);
        console.log(
          `[BridgeWire][TX#${txSeq}] burst=${CloudBridge.burstModeActive} ` +
          `timeoutOverrideMs=${elmTimeoutMsRef.current} raw=${displayText}`
        );
      } catch {
        console.log(`[BluetoothBridge] A→B→ELM327: [二进制数据] (len=${overridden.length})`);
        console.log(`[BridgeWire][TX#${txSeq}] burst=${CloudBridge.burstModeActive} raw=[二进制数据]`);
      }

      await sendData(overridden);
    };

    // 2. 处理云端A发送的蓝牙连接请求
    CloudBridge.onBluetoothConnectRequest = async (protocol: string, address: string) => {
      console.log('[BluetoothBridge] 云端请求连接蓝牙:', protocol, address);
      const result = await connectDevice(protocol as 'ble' | 'classic' | 'mfi', address);
      if (!result.success) {
        throw new Error(result.error || '蓝牙连接失败');
      }
      return result.sessionId || '';
    };

    // 3. 处理云端A发送的蓝牙断开请求
    CloudBridge.onBluetoothDisconnectRequest = async () => {
      console.log('[BluetoothBridge] 云端请求断开蓝牙');
      await disconnectDevice();
    };

    // 4. 处理云端A发送的扫描请求
    CloudBridge.onBluetoothStartScanRequest = async (protocols?: string[]) => {
      console.log('[BluetoothBridge] 云端请求开始扫描:', protocols || 'all');
      const normalized = (protocols || []).filter(Boolean) as Array<'ble' | 'classic' | 'mfi'>;
      await startScan(normalized.length > 0 ? normalized : undefined);
    };

    CloudBridge.onBluetoothStopScanRequest = async () => {
      console.log('[BluetoothBridge] 云端请求停止扫描');
      await stopScan();
    };

    // 5. 监听本地蓝牙数据，转发给云端A（ELM327→B→A）
    // 注意：不需要转发设备扫描事件给 A 端，A 端不需要这些信息
    const bluetoothGateway = getBluetoothGateway();
    if (bluetoothGateway) {
      const dataUnsub = bluetoothGateway.addDataReceivedListener(
        (sessionId: string, base64Data: string) => {
          const rxSeq = ++bridgeWireRxCounter;

          // 调试：解码并显示收到的 ELM327 响应
          try {
            const decoded = Buffer.from(base64Data, 'base64').toString('utf-8');
            const displayText = formatBridgeWireText(decoded);
            console.log(`[BluetoothBridge] ELM327→B→A: ${displayText} (len=${base64Data.length})`);
            console.log(
              `[BridgeWire][RX#${rxSeq}] session=${sessionId} burst=${CloudBridge.burstModeActive} ` +
              `forwarded=${!CloudBridge.burstModeActive} raw=${displayText}`
            );
          } catch {
            console.log(`[BluetoothBridge] ELM327→B→A: [二进制数据] (len=${base64Data.length})`);
            console.log(
              `[BridgeWire][RX#${rxSeq}] session=${sessionId} burst=${CloudBridge.burstModeActive} ` +
              `forwarded=${!CloudBridge.burstModeActive} raw=[二进制数据]`
            );
          }

          // 发送数据给云端A（Burst 期间抑制：B 端独占蓝牙通道，不向 A 转发）
          if (!CloudBridge.burstModeActive) {
            CloudBridge.sendOBDData(sessionId, base64Data);
          }
        }
      );
      const lostUnsub = bluetoothGateway.addConnectionLostListener(
        (sessionId: string, reason: string) => {
          console.warn(`[BluetoothBridge] 蓝牙连接丢失: ${reason} (session=${sessionId})`);
          if (options?.onConnectionLost) {
            // 外部处理（含自动重连逻辑）
            options.onConnectionLost(sessionId, reason);
          } else {
            // 兜底：原有行为
            CloudBridge.sendConnectionLost(sessionId, reason);
          }
        }
      );
      // 移除设备扫描事件转发 - A 端不需要这些信息，B 端本地处理即可
      unsubscribeRef.current.push(dataUnsub);
      unsubscribeRef.current.push(lostUnsub);
      console.log('[BluetoothBridge] 数据监听已注册（不转发扫描事件给云端）');
    } else {
      console.warn('[BluetoothBridge] 蓝牙网关未初始化，数据监听未注册');
    }

    // 清理
    return () => {
      console.log('[BluetoothBridge] 清理蓝牙桥接');
      CloudBridge.onBluetoothSendRequest = undefined;
      CloudBridge.onBluetoothConnectRequest = undefined;
      CloudBridge.onBluetoothDisconnectRequest = undefined;
      CloudBridge.onBluetoothStartScanRequest = undefined;
      CloudBridge.onBluetoothStopScanRequest = undefined;

      if (unsubscribeRef.current.length > 0) {
        unsubscribeRef.current.forEach((unsub) => {
          try { unsub(); } catch { }
        });
        unsubscribeRef.current = [];
      }
    };
  }, [connectDevice, disconnectDevice, sendData, startScan, stopScan]);
}

export default useBluetoothBridge;
