/**
 * 蓝牙管理器 - 支持三种蓝牙连接方式
 *
 * 1. 经典蓝牙 (Classic Bluetooth / SPP) - Android上使用
 * 2. BLE (Bluetooth Low Energy) - Android/iOS通用
 * 3. MFi (Made for iPhone) - iOS上的经典蓝牙替代方案
 *
 * 依赖库:
 * - react-native-ble-manager (BLE) - 替代 react-native-ble-plx
 * - react-native-bluetooth-classic (经典蓝牙/Android)
 * - OBDMFiModuleIOS (MFi/iOS 原生桥接)
 */
import { Platform, NativeModules, NativeEventEmitter } from 'react-native';
import { Buffer } from 'buffer';
import { BTDeviceInfo } from '../protocol/MessageProtocol';

// 动态导入 BleManager，防止原生模块未链接时崩溃
let BleManager: any = null;
let bleManagerEmitter: NativeEventEmitter | null = null;
const OBDMFiModuleIOS = NativeModules.OBDMFiModuleIOS;

type MFiAccessoryInfo = {
  name?: string;
  serialNumber?: string;
  connectionID?: string | number;
  protocolStrings?: string[];
};

type MFiNativeEventPayload = {
  sessionId?: string;
  base64Data?: string;
  reason?: string;
};

type MFiNativeModule = {
  isSupported?: () => Promise<boolean>;
  getConnectedAccessories: () => Promise<MFiAccessoryInfo[]>;
  showBluetoothAccessoryPicker?: () => Promise<{ status?: string } | string>;
  openSession: (accessoryId: string, protocol: string) => Promise<string>;
  send: (sessionId: string, base64Data: string) => Promise<void>;
  closeSession: (sessionId: string) => Promise<boolean>;
};

// 调试: 列出所有原生模块
console.log('[BLE] Platform:', Platform.OS);
console.log('[BLE] 可用的原生模块:', Object.keys(NativeModules).filter(k => k.toLowerCase().includes('ble') || k.toLowerCase().includes('bluetooth') || k.toLowerCase().includes('mfi')));
console.log('[BLE] NativeModules.BleManager 存在:', !!NativeModules.BleManager);
console.log('[MFi] NativeModules.OBDMFiModuleIOS 存在:', !!OBDMFiModuleIOS);

if (NativeModules.BleManager) {
  try {
    BleManager = require('react-native-ble-manager').default;
    if (BleManager) {
      // v11: BleManager 本身就是 NativeEventEmitter
      bleManagerEmitter = BleManager;
      console.log('[BLE] react-native-ble-manager 模块加载成功');
    }
  } catch (e) {
    console.warn('[BLE] react-native-ble-manager 模块加载失败:', e);
  }
} else {
  console.warn('[BLE] NativeModules.BleManager 不存在，BLE 功能不可用');
  console.log('[BLE] 所有可用的 NativeModules:', Object.keys(NativeModules).join(', '));
}

// 连接协议类型
export type BluetoothProtocol = 'classic' | 'ble' | 'mfi';

// 设备信息（扩展）
export interface ExtendedBTDeviceInfo extends BTDeviceInfo {
  protocol: BluetoothProtocol;
  serviceUUIDs?: string[];
}

// 事件处理器
export type BluetoothEventHandler = {
  onDeviceDiscovered?: (device: ExtendedBTDeviceInfo) => void;
  onScanFinished?: () => void;
  onDataReceived?: (sessionId: string, base64Data: string) => void;
  onConnectionLost?: (sessionId: string, reason: string) => void;
  onError?: (code: string, message: string) => void;
};

// ELM327 设备常用UUID
const OBD_SERVICE_UUIDS = {
  BLE: [
    'FFF0',
    '0000FFF0-0000-1000-8000-00805F9B34FB',
    'E7810A71-73AE-499D-8C15-FAA9AEF0C3F2', // Vlink
  ],
  CLASSIC_SPP: '00001101-0000-1000-8000-00805F9B34FB',
};

const TX_CHAR_UUID = 'FFF2';
const RX_CHAR_UUID = 'FFF1';
const MFI_PROTOCOL_CANDIDATES = [
  'com.obdlink',
  'com.vgatemall',
  'com.obd2.elm327',
  'com.obdlink.obd',
  'com.vgatemall.obd',
];
const MFI_DATA_EVENT = 'OBDMFiDataReceived';
const MFI_SESSION_CLOSED_EVENT = 'OBDMFiSessionClosed';
const MFI_ACCESSORY_CONNECTED_EVENT = 'OBDMFiAccessoryConnected';
const MFI_ACCESSORY_DISCONNECTED_EVENT = 'OBDMFiAccessoryDisconnected';
const MFI_CONNECT_MATCH_TIMEOUT_MS = 2800;
const MFI_CONNECT_MATCH_INTERVAL_MS = 250;
const MFI_CONNECT_RETRY_DELAY_MS = 250;
const MFI_LOG_PREVIEW_BYTES = 64;
const MFI_ACCESSORY_SUMMARY_LIMIT = 6;
const MFI_RX_FLUSH_TIMEOUT_MS = 60;
const MFI_RX_PROMPT_FLUSH_DELAY_MS = 12;
const MFI_RX_MAX_BUFFER_BYTES = 16 * 1024;

/**
 * BLE连接适配器 - 使用 react-native-ble-manager
 */
class BLEAdapter {
  private connectedPeripheralId: string | null = null;
  private sessionId: string | null = null;
  private eventHandler: BluetoothEventHandler;
  private listeners: any[] = [];
  private discoveredDevices: Map<string, ExtendedBTDeviceInfo> = new Map();
  private txServiceUUID: string | null = null;
  private txCharUUID: string | null = null;
  private useWriteWithoutResponse: boolean = false; // ELM327 设备通常使用无响应写入

  constructor(eventHandler: BluetoothEventHandler) {
    this.eventHandler = eventHandler;
  }

  async initialize(): Promise<boolean> {
    console.log('[BLE] 初始化中...');
    if (!BleManager) {
      console.log('[BLE] BleManager 模块未加载，跳过初始化');
      return false;
    }
    try {
      // 启用详细日志，便于调试
      await BleManager.start({ showAlert: false, verboseLogging: true });
      console.log('[BLE] BleManager 初始化成功');
      return true;
    } catch (error: any) {
      console.log('[BLE] 初始化失败:', error.message);
      return false;
    }
  }

  async startScan(): Promise<void> {
    console.log('[BLE] 开始扫描...');
    if (!BleManager || !bleManagerEmitter) {
      console.log('[BLE] BleManager 未初始化，无法扫描');
      this.eventHandler.onError?.('BLE_NOT_INITIALIZED', 'BLE模块未加载');
      return;
    }
    this.discoveredDevices.clear();

    // 移除旧的监听器
    this.removeListeners();

    // 监听发现设备事件
    const discoverListener = bleManagerEmitter.addListener(
      'BleManagerDiscoverPeripheral',
      (peripheral: any) => {
        this.handleDiscoveredPeripheral(peripheral);
      }
    );
    this.listeners.push(discoverListener);

    // 监听扫描停止事件
    const stopListener = bleManagerEmitter.addListener(
      'BleManagerStopScan',
      () => {
        console.log('[BLE] 扫描已停止');
        this.eventHandler.onScanFinished?.();
      }
    );
    this.listeners.push(stopListener);

    try {
      // 扫描所有设备，持续30秒
      // allowDuplicates=false：iOS 每个设备只上报一次，避免高频广播事件洪泛 JS 线程
      await BleManager.scan([], 30, false);
      console.log('[BLE] 扫描已启动');
    } catch (error: any) {
      console.log('[BLE] 扫描启动失败:', error.message);
      this.eventHandler.onError?.('BLE_SCAN_ERROR', error.message);
    }
  }

  private handleDiscoveredPeripheral(peripheral: any): void {
    const deviceId = peripheral.id;
    const isConnectable = peripheral.advertising?.isConnectable;
    const deviceName = peripheral.name || peripheral.advertising?.localName || null;

    // 过滤：跳过不可连接的设备（如 AirTag、Find My 网络设备等）
    if (isConnectable === 0 || isConnectable === false) {
      return;
    }

    const existingDevice = this.discoveredDevices.get(deviceId);

    // 去重逻辑
    if (existingDevice) {
      // 如果已有设备且名称相同，跳过（即使 RSSI 变化也不更新，避免触发 state update）
      if (existingDevice.name === deviceName) return;
      // 如果已有真实名称，新的没有名称，跳过
      if (!deviceName && !existingDevice.name.startsWith('BLE-')) return;
    }

    const info: ExtendedBTDeviceInfo = {
      name: deviceName || `BLE-${deviceId.substring(0, 8)}`,
      address: deviceId,
      rssi: peripheral.rssi ?? -100,
      valid: this.isOBDDevice(deviceName || ''),
      paired: false,
      protocol: 'ble',
      serviceUUIDs: peripheral.advertising?.serviceUUIDs || [],
    };

    // 只在真正新增/更新设备时打印（去重后），避免日志洪泛
    console.log('[BLE] 发现设备:', info.name, info.address, 'RSSI:', info.rssi);

    this.discoveredDevices.set(deviceId, info);
    this.eventHandler.onDeviceDiscovered?.(info);
  }

  async stopScan(): Promise<void> {
    try {
      await BleManager.stopScan();
    } catch (error) {
      // 忽略停止扫描的错误
    }
    this.eventHandler.onScanFinished?.();
  }

  async connect(address: string): Promise<string> {
    console.log('[BLE] 连接到设备:', address);

    // 连接设备
    await BleManager.connect(address);
    console.log('[BLE] 已连接');

    // 发现服务和特征
    const peripheralInfo = await BleManager.retrieveServices(address);
    console.log('[BLE] 服务已发现:', peripheralInfo);

    // 查找可写特征和可通知特征
    let txCharacteristic: any = null;
    let rxCharacteristic: any = null;

    if (peripheralInfo.characteristics) {
      for (const char of peripheralInfo.characteristics) {
        const props = char.properties;
        // 查找可写特征（TX）- 优先使用 WriteWithoutResponse（ELM327 设备通常使用）
        if (props.WriteWithoutResponse) {
          txCharacteristic = char;
          this.txServiceUUID = char.service;
          this.txCharUUID = char.characteristic;
          this.useWriteWithoutResponse = true;
          console.log('[BLE] 找到TX特征 (WriteWithoutResponse):', char.characteristic);
        } else if (props.Write && !txCharacteristic) {
          txCharacteristic = char;
          this.txServiceUUID = char.service;
          this.txCharUUID = char.characteristic;
          this.useWriteWithoutResponse = false;
          console.log('[BLE] 找到TX特征 (Write):', char.characteristic);
        }
        // 查找可通知特征（RX）
        if (props.Notify || props.Indicate) {
          rxCharacteristic = char;
          console.log('[BLE] 找到RX特征:', char.characteristic);
        }
      }
    }

    // 启动通知监听
    if (rxCharacteristic) {
      await BleManager.startNotification(
        address,
        rxCharacteristic.service,
        rxCharacteristic.characteristic
      );

      // 监听数据
      const dataListener = bleManagerEmitter.addListener(
        'BleManagerDidUpdateValueForCharacteristic',
        (data: any) => {
          if (data.peripheral === address && this.sessionId) {
            // data.value 是字节数组，转为 base64
            const bytes = new Uint8Array(data.value);
            const base64 = this.bytesToBase64(bytes);

            // 调试：打印收到的原始数据（解码后的文本）+ 时间戳
            const decoded = this.base64ToString(base64);
            const recvTs = new Date().toISOString().slice(11, 23);
            console.log(`[BLE] ${recvTs} ◀ ELM327响应: [${decoded.substring(0, 160).replace(/\r/g, '\\r').replace(/\n/g, '\\n')}] (len=${decoded.length})`);

            this.eventHandler.onDataReceived?.(this.sessionId, base64);
          }
        }
      );
      this.listeners.push(dataListener);
    }

    // 监听断开连接
    const disconnectListener = bleManagerEmitter.addListener(
      'BleManagerDisconnectPeripheral',
      (data: any) => {
        if (data.peripheral === address && this.sessionId) {
          this.eventHandler.onConnectionLost?.(this.sessionId, 'BLE device disconnected');
        }
      }
    );
    this.listeners.push(disconnectListener);

    this.connectedPeripheralId = address;
    this.sessionId = `ble-${Date.now()}`;

    return this.sessionId;
  }

  async send(base64Data: string): Promise<void> {
    if (!this.connectedPeripheralId || !this.txServiceUUID || !this.txCharUUID) {
      throw new Error('BLE not connected');
    }

    // base64 转字节数组
    const bytes = this.base64ToBytes(base64Data);

    // 调试：打印发送的数据（解码后的文本）+ 时间戳
    const decoded = this.base64ToString(base64Data);
    const tsStr = new Date().toISOString().slice(11, 23);
    console.log(`[BLE] ${tsStr} ▶ AT命令: [${decoded.replace(/\r/g, '\\r').replace(/\n/g, '\\n')}] (len=${decoded.length})`);

    // BLE MTU 限制，通常为 20 字节，分块发送
    const MTU = 20;
    for (let i = 0; i < bytes.length; i += MTU) {
      const chunk = bytes.slice(i, Math.min(i + MTU, bytes.length));

      if (this.useWriteWithoutResponse) {
        // 使用无响应写入（更快，ELM327 设备常用）
        await BleManager.writeWithoutResponse(
          this.connectedPeripheralId,
          this.txServiceUUID,
          this.txCharUUID,
          Array.from(chunk)
        );
      } else {
        // 使用标准写入（需要设备确认）
        await BleManager.write(
          this.connectedPeripheralId,
          this.txServiceUUID,
          this.txCharUUID,
          Array.from(chunk)
        );
      }
    }
  }

  async disconnect(): Promise<void> {
    if (this.connectedPeripheralId) {
      try {
        await BleManager.disconnect(this.connectedPeripheralId);
      } catch (error) {
        // 忽略断开连接的错误
      }
    }
    this.removeListeners();
    this.connectedPeripheralId = null;
    this.sessionId = null;
    this.txServiceUUID = null;
    this.txCharUUID = null;
    this.useWriteWithoutResponse = false;
  }

  getSessionId(): string | null {
    return this.sessionId;
  }

  private removeListeners(): void {
    for (const listener of this.listeners) {
      listener.remove();
    }
    this.listeners = [];
  }

  private isOBDDevice(name: string): boolean {
    const upper = name.toUpperCase();
    const keywords = ['OBD', 'ELM', 'VLINK', 'VGATE', 'SCAN', 'CAR', 'DIAG'];
    return keywords.some(k => upper.includes(k));
  }

  private bytesToBase64(bytes: Uint8Array): string {
    return Buffer.from(bytes).toString('base64');
  }

  private base64ToBytes(base64: string): Uint8Array {
    const buffer = Buffer.from(base64, 'base64');
    return new Uint8Array(buffer);
  }

  private base64ToString(base64: string): string {
    try {
      return Buffer.from(base64, 'base64').toString('utf-8');
    } catch {
      return '[无法解码]';
    }
  }

  destroy(): void {
    this.removeListeners();
  }
}

/**
 * 经典蓝牙适配器 (Android SPP)
 * 使用 react-native-bluetooth-classic 库
 */
class ClassicBluetoothAdapter {
  private btClassic: any = null;
  private connectedDevice: any = null;
  private connectedAddress: string | null = null;
  private sessionId: string | null = null;
  private eventHandler: BluetoothEventHandler;
  private isScanning: boolean = false;
  private scanTimeoutHandle: any = null;
  private lastAddressHint: string = '';
  private isConnecting: boolean = false;
  private connectingAddress: string | null = null;
  private connectPromise: Promise<string> | null = null;

  // 事件订阅
  private disconnectSubscription: any = null;
  private discoverySubscription: any = null;
  private dataSubscription: any = null;

  constructor(eventHandler: BluetoothEventHandler) {
    this.eventHandler = eventHandler;
    this.initModule();
  }

  private initModule(): void {
    // 经典蓝牙只在 Android 上可用，iOS 不支持
    if (Platform.OS !== 'android') {
      console.log('[ClassicBT] iOS 不支持经典蓝牙，跳过');
      return;
    }

    try {
      // 加载 react-native-bluetooth-classic (它导出的是 BluetoothModule 实例)
      this.btClassic = require('react-native-bluetooth-classic').default;
      console.log('[ClassicBT] 模块加载成功');
    } catch (e) {
      console.warn('[ClassicBT] react-native-bluetooth-classic 未安装');
    }
  }

  async initialize(): Promise<boolean> {
    if (Platform.OS !== 'android') return false;
    if (!this.btClassic) return false;

    try {
      // 请求 Android 运行时权限
      const granted = await this.requestPermissions();
      if (!granted) {
        console.warn('[ClassicBT] 蓝牙权限被拒绝');
        return false;
      }

      // 检查蓝牙是否启用
      const enabled = await this.btClassic.isBluetoothEnabled();
      if (!enabled) {
        await this.btClassic.requestBluetoothEnabled();
      }
      console.log('[ClassicBT] 初始化成功');
      return true;
    } catch (e: any) {
      console.error('[ClassicBT] 初始化失败:', e.message);
      this.eventHandler.onError?.('CLASSIC_BT_ERROR', e.message);
      return false;
    }
  }

  /**
   * 请求 Android 蓝牙运行时权限
   */
  private async requestPermissions(): Promise<boolean> {
    if (Platform.OS !== 'android') return true;

    try {
      const { PermissionsAndroid } = require('react-native');

      // Android 12+ (API 31+) 需要新的蓝牙权限
      const permissions = [
        PermissionsAndroid.PERMISSIONS.BLUETOOTH_SCAN,
        PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT,
        PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION,
      ].filter(Boolean); // 过滤掉不存在的权限（旧版Android）

      const results = await PermissionsAndroid.requestMultiple(permissions);

      // 检查所有权限是否授予
      const allGranted = Object.values(results).every(
        (result) => result === PermissionsAndroid.RESULTS.GRANTED
      );

      console.log('[ClassicBT] 权限请求结果:', results, '全部授予:', allGranted);
      return allGranted;
    } catch (e: any) {
      console.error('[ClassicBT] 权限请求失败:', e.message);
      return false;
    }
  }

  async startScan(): Promise<void> {
    if (!this.btClassic) {
      this.eventHandler.onError?.('CLASSIC_BT_UNAVAILABLE', '经典蓝牙模块未加载');
      return;
    }

    try {
      if (this.isScanning) {
        console.log('[ClassicBT] 扫描已在进行中，忽略重复请求');
        return;
      }
      this.isScanning = true;

      // 先请求权限
      const granted = await this.requestPermissions();
      if (!granted) {
        this.eventHandler.onError?.('PERMISSION_DENIED', '蓝牙权限被拒绝');
        this.isScanning = false;
        return;
      }

      // 确保不处于发现中（避免 "already in discovery mode"）
      try {
        await this.btClassic.cancelDiscovery();
      } catch {
        // ignore
      }

      // 1. 获取已配对设备
      console.log('[ClassicBT] 获取已配对设备...');
      const paired = await this.btClassic.getBondedDevices();
      console.log('[ClassicBT] 已配对设备数量:', paired.length);
      if (paired.length > 0) {
        console.log('[ClassicBT] 已配对设备(前8):', this.formatBondedDevices(paired, 8));
      }

      for (const device of paired) {
        const info: ExtendedBTDeviceInfo = {
          name: device.name || 'Unknown',
          address: device.address,
          rssi: -50,
          valid: this.isELM327Device(device.name),
          paired: true,
          protocol: 'classic',
        };
        this.eventHandler.onDeviceDiscovered?.(info);
      }

      // 2. 设置设备发现监听器
      this.removeDiscoverySubscription();
      this.discoverySubscription = this.btClassic.onDeviceDiscovered((event: any) => {
        console.log('[ClassicBT] 发现新设备:', event.device?.name, event.device?.address);
        const device = event.device;
        if (device) {
          const info: ExtendedBTDeviceInfo = {
            name: device.name || 'Unknown',
            address: device.address,
            rssi: device.rssi ?? -70,
            valid: this.isELM327Device(device.name),
            paired: false,
            protocol: 'classic',
          };
          this.eventHandler.onDeviceDiscovered?.(info);
        }
      });

      // 3. 开始发现新设备
      console.log('[ClassicBT] 开始扫描周围设备...');
      await this.btClassic.startDiscovery();

      // 扫描超时 30 秒
      if (this.scanTimeoutHandle) {
        clearTimeout(this.scanTimeoutHandle);
      }
      this.scanTimeoutHandle = setTimeout(() => this.stopScan(), 30000);
    } catch (e: any) {
      console.error('[ClassicBT] 扫描错误:', e.message);
      this.eventHandler.onError?.('CLASSIC_SCAN_ERROR', e.message);
      this.isScanning = false;
    }
  }

  async stopScan(): Promise<void> {
    this.removeDiscoverySubscription();

    if (this.scanTimeoutHandle) {
      clearTimeout(this.scanTimeoutHandle);
      this.scanTimeoutHandle = null;
    }

    try {
      if (this.btClassic) {
        await this.btClassic.cancelDiscovery();
        console.log('[ClassicBT] 扫描已停止');
      }
    } catch {
      // 忽略取消扫描的错误
    } finally {
      this.isScanning = false;
    }
    this.eventHandler.onScanFinished?.();
  }

  async connect(address: string): Promise<string> {
    if (!this.btClassic) throw new Error('经典蓝牙不可用');

    const normalized = String(address || '').trim().toUpperCase();
    if (this.connectedAddress && this.connectedAddress.toUpperCase() === normalized && this.sessionId) {
      console.log('[ClassicBT] 已连接到该地址，复用 sessionId:', this.sessionId);
      return this.sessionId;
    }

    if (this.isConnecting) {
      if (this.connectingAddress && this.connectingAddress.toUpperCase() === normalized && this.connectPromise) {
        console.log('[ClassicBT] 同地址连接进行中，复用连接 Promise');
        return this.connectPromise;
      }
      throw new Error('正在连接其他设备，请稍后重试');
    }

    this.isConnecting = true;
    this.connectingAddress = normalized;
    this.connectPromise = this.connectInternal(address);
    try {
      return await this.connectPromise;
    } finally {
      this.isConnecting = false;
      this.connectingAddress = null;
      this.connectPromise = null;
    }
  }

  private async connectInternal(address: string): Promise<string> {
    if (!this.btClassic) throw new Error('经典蓝牙不可用');

    const addressInfo = this.getAddressInfo(address);
    console.log('[ClassicBT] 连接设备:', addressInfo.normalized);
    console.log('[ClassicBT] 地址信息:', addressInfo.description);
    this.lastAddressHint = '';
    if (!addressInfo.isValid) {
      throw new Error('蓝牙地址格式无效，请确认地址形如 AA:BB:CC:DD:EE:FF');
    }
    if (addressInfo.isMulticast) {
      throw new Error('蓝牙地址为组播地址，无法用于经典蓝牙连接');
    }
    // 校验是否在已配对列表（经典蓝牙通常应为已配对）
    let isBonded = false;
    try {
      const bonded = await this.btClassic.getBondedDevices();
      const bondedSet = new Set(
        bonded.map((d: any) => String(d.address || '').toUpperCase())
      );
      isBonded = bondedSet.has(addressInfo.normalized);
      console.log('[ClassicBT] 已配对设备数量:', bonded.length, '当前地址已配对:', isBonded);
      if (!isBonded) {
        const hint = '当前地址不在系统已配对列表';
        this.lastAddressHint = this.lastAddressHint
          ? `${this.lastAddressHint}；${hint}`
          : hint;
        console.warn('[ClassicBT] 警告:', hint);
      }
    } catch (e: any) {
      console.warn('[ClassicBT] 获取已配对设备失败:', e?.message || e);
    }

    if (addressInfo.isLocallyAdmin) {
      if (isBonded) {
        console.log('[ClassicBT] 本地管理地址但已配对，按固定地址处理');
      } else {
        this.lastAddressHint = this.lastAddressHint
          ? `${this.lastAddressHint}；地址为本地管理地址（不一定是随机）`
          : '地址为本地管理地址（不一定是随机）';
        console.warn('[ClassicBT] 警告:', this.lastAddressHint);
      }
    }

    // 关键：连接前必须先取消蓝牙发现，否则 Android 蓝牙栈会干扰连接
    if (this.isScanning) {
      await this.stopScan();
    }
    try {
      await this.btClassic.cancelDiscovery();
      console.log('[ClassicBT] 已取消蓝牙发现');
    } catch (e) {
      // 忽略取消发现的错误
    }

    // 等待蓝牙栈稳定
    await new Promise(r => setTimeout(r, 300));

    // 连接设备 - 使用纯二进制模式，不设置分隔符
    // OBD 响应可能包含多个 \r，使用分隔符会导致数据被错误分割
    // 尝试多种连接方式，解决 "read failed, socket might closed or timeout" 错误
    let device: any = null;

    // 优化连接顺序：先尝试 insecure（大多数 ELM327 克隆设备使用）
    // 注意：某些华为/荣耀手机需要使用 secure 连接
    const attempts = [
      { secureSocket: false, label: 'insecure default' },  // 最常见的成功方式
      { secureSocket: true, label: 'secure default' },
      { secureSocket: false, connectorType: 'rfcomm', label: 'insecure rfcomm' },
      { secureSocket: true, connectorType: 'rfcomm', label: 'secure rfcomm' },
    ];

    for (const attempt of attempts) {
      try {
        console.log(`[ClassicBT] 尝试连接: ${attempt.label}`);
        device = await this.btClassic.connectToDevice(address, {
          connectionType: 'binary',
          secureSocket: attempt.secureSocket,
          readTimeout: 8000, // 提高读取超时，避免弱链路下过早断开
          ...(attempt.connectorType ? { connectorType: attempt.connectorType } : {}),
        });
        console.log(`[ClassicBT] 连接成功: ${attempt.label}`);
        break;
      } catch (e: any) {
        console.log(`[ClassicBT] ${attempt.label} 失败: ${e.message}`);
        device = null;
        // 等待蓝牙栈恢复后再尝试下一种方式（增加到 1.5 秒）
        await new Promise(r => setTimeout(r, 1500));
      }
    }

    if (!device) {
      const hint = this.lastAddressHint
        ? `（${this.lastAddressHint}）`
        : '';
      throw new Error(`蓝牙连接失败，请尝试在系统设置中重新配对设备${hint}`);
    }

    this.connectedDevice = device;
    this.connectedAddress = address;
    this.sessionId = `classic-${Date.now()}`;

    // 监听数据接收（使用设备的 onDataReceived 方法）
    if (device && typeof device.onDataReceived === 'function') {
      this.dataSubscription = device.onDataReceived((event: any) => {
        if (this.sessionId && event?.data) {
          // binary 模式下，data 可能是 Buffer/Uint8Array 或 base64 字符串
          let base64: string;
          if (typeof event.data === 'string') {
            // 如果是字符串，可能已经是 base64 或原始文本
            if (this.isBase64(event.data)) {
              base64 = this.normalizeBase64(event.data);
            } else {
              base64 = Buffer.from(event.data, 'utf-8').toString('base64');
            }
          } else if (event.data instanceof Uint8Array || Buffer.isBuffer(event.data)) {
            // 如果是字节数组，直接转 base64
            base64 = Buffer.from(event.data).toString('base64');
          } else {
            // 其他情况，尝试转换
            base64 = Buffer.from(String(event.data)).toString('base64');
          }

          // 调试：打印收到的原始数据（解码后的文本）+ 时间戳
          const decoded = Buffer.from(base64, 'base64').toString('utf-8');
          const recvTs = new Date().toISOString().slice(11, 23);
          console.log(`[ClassicBT] ${recvTs} ◀ ELM327响应: [${decoded.substring(0, 160).replace(/\r/g, '\\r').replace(/\n/g, '\\n')}] (len=${decoded.length})`);

          this.eventHandler.onDataReceived?.(this.sessionId, base64);
        }
      });
      console.log('[ClassicBT] 数据监听已设置');
    }

    // 监听断开连接（使用全局模块事件）
    this.removeDisconnectSubscription();
    this.disconnectSubscription = this.btClassic.onDeviceDisconnected((event: any) => {
      if (event.device?.address === this.connectedAddress && this.sessionId) {
        console.log('[ClassicBT] 设备已断开:', event.device.address);
        this.eventHandler.onConnectionLost?.(this.sessionId, 'Classic BT disconnected');
        this.cleanup();
      }
    });

    console.log('[ClassicBT] 连接成功, sessionId:', this.sessionId);
    return this.sessionId;
  }

  isConnectedTo(address: string): boolean {
    if (!this.connectedAddress) return false;
    return this.connectedAddress.toUpperCase() === String(address || '').trim().toUpperCase();
  }

  getSessionId(): string | null {
    return this.sessionId;
  }

  async send(base64Data: string): Promise<void> {
    if (!this.connectedAddress || !this.btClassic) {
      throw new Error('未连接到设备');
    }

    // 调试：打印发送的数据（解码后的文本）+ 时间戳
    const decoded = Buffer.from(base64Data, 'base64').toString('utf-8');
    const tsStr = new Date().toISOString().slice(11, 23);
    console.log(`[ClassicBT] ${tsStr} ▶ AT命令: [${decoded.replace(/\r/g, '\\r').replace(/\n/g, '\\n')}] (len=${decoded.length})`);

    // 使用模块方法发送数据（更可靠）
    const writeStart = Date.now();
    await this.btClassic.writeToDevice(this.connectedAddress, base64Data, 'base64');
    console.log(`[ClassicBT] ${new Date().toISOString().slice(11, 23)} ✓ 发送完成 (耗时=${Date.now()-writeStart}ms)`);
  }

  async disconnect(): Promise<void> {
    if (this.connectedAddress && this.btClassic) {
      try {
        await this.btClassic.disconnectFromDevice(this.connectedAddress);
        console.log('[ClassicBT] 已断开连接');
      } catch (e) {
        // 忽略断开连接的错误
      }
    }
    this.cleanup();
  }

  private cleanup(): void {
    this.removeDataSubscription();
    this.removeDisconnectSubscription();
    this.connectedDevice = null;
    this.connectedAddress = null;
    this.sessionId = null;
  }

  private removeDiscoverySubscription(): void {
    if (this.discoverySubscription) {
      this.discoverySubscription.remove();
      this.discoverySubscription = null;
    }
  }

  private removeDisconnectSubscription(): void {
    if (this.disconnectSubscription) {
      this.disconnectSubscription.remove();
      this.disconnectSubscription = null;
    }
  }

  private removeDataSubscription(): void {
    if (this.dataSubscription) {
      this.dataSubscription.remove();
      this.dataSubscription = null;
    }
  }

  private isBase64(str: string): boolean {
    if (!str || str.length === 0) return false;
    const cleaned = str.replace(/\s+/g, '');
    if (cleaned.length < 4) return false; // 4 是含 = 填充的有效 base64 最小长度（如 DT4= 即 \r>）
    if (!/^[A-Za-z0-9+/=]+$/.test(cleaned)) return false;
    try {
      const padded = this.normalizeBase64(cleaned);
      const buf = Buffer.from(padded, 'base64');
      if (!buf || buf.length === 0) return false;
      const roundtrip = Buffer.from(buf).toString('base64').replace(/=+$/, '');
      return roundtrip === cleaned.replace(/=+$/, '');
    } catch {
      return false;
    }
  }

  private normalizeBase64(str: string): string {
    const cleaned = str.replace(/\s+/g, '');
    const mod = cleaned.length % 4;
    if (mod === 0) return cleaned;
    return cleaned + '='.repeat(4 - mod);
  }

  private getAddressInfo(address: string): {
    normalized: string;
    isValid: boolean;
    isLocallyAdmin: boolean;
    isMulticast: boolean;
    description: string;
  } {
    const normalized = String(address || '').trim().toUpperCase();
    const isValid = /^([0-9A-F]{2}:){5}[0-9A-F]{2}$/.test(normalized);
    if (!isValid) {
      return {
        normalized,
        isValid: false,
        isLocallyAdmin: false,
        isMulticast: false,
        description: `${normalized}（无效格式）`,
      };
    }
    const firstByte = parseInt(normalized.split(':')[0], 16);
    const isMulticast = (firstByte & 0x01) !== 0;
    const isLocallyAdmin = (firstByte & 0x02) !== 0;
    let typeLabel = '公共地址';
    if (isMulticast) typeLabel = '组播地址';
    else if (isLocallyAdmin) typeLabel = '本地管理地址(不一定随机)';
    const description = `${normalized} / ${typeLabel}`;
    return {
      normalized,
      isValid: true,
      isLocallyAdmin,
      isMulticast,
      description,
    };
  }

  private formatBondedDevices(devices: any[], limit: number): string {
    const list = devices.slice(0, limit).map((d: any) => {
      const name = d?.name || 'Unknown';
      const addr = String(d?.address || '').toUpperCase();
      return `${name}(${addr})`;
    });
    return list.join(', ');
  }

  private isELM327Device(name: string): boolean {
    if (!name) return false;
    const upper = name.toUpperCase();
    const keywords = ['OBD', 'ELM', 'VLINK', 'VGATE', 'SCAN', 'CAR', 'DIAG'];
    return keywords.some(k => upper.includes(k));
  }
}

/**
 * MFi适配器 (iOS External Accessory)
 */
class MFiAdapter {
  private nativeModule: MFiNativeModule | null = null;
  private nativeEmitter: NativeEventEmitter | null = null;
  private sessionId: string | null = null;
  private nativeSessionId: string | null = null;
  private connectedAccessoryId: string | null = null;
  private selectedAddress: string | null = null;
  private eventHandler: BluetoothEventHandler;
  private dataSubscription: any = null;
  private closeSubscription: any = null;
  private accessoryConnectSubscription: any = null;
  private accessoryDisconnectSubscription: any = null;
  private listenersAttached: boolean = false;
  private currentProtocol: string | null = null;
  private sendQueue: Promise<void> = Promise.resolve();
  private sendQueueError: string | null = null;
  private lastActivityAt: number = 0;
  private connectionOpenedAt: number = 0;
  private txPackets: number = 0;
  private rxPackets: number = 0;
  private accessoryHintsByAddress: Map<string, { name: string; serial: string; connectionId: string }> = new Map();
  private rxChunkBuffers: Buffer[] = [];
  private rxBufferedBytes: number = 0;
  private rxFlushTimer: ReturnType<typeof setTimeout> | null = null;
  private txSequence: number = 0;
  private rxChunkSequence: number = 0;
  private rxResponseSequence: number = 0;

  constructor(eventHandler: BluetoothEventHandler) {
    this.eventHandler = eventHandler;
    this.initModule();
  }

  private initModule(): void {
    if (Platform.OS !== 'ios') return;

    if (!OBDMFiModuleIOS) {
      this.log('init', 'OBDMFiModuleIOS 未链接，MFi功能不可用');
      return;
    }

    this.nativeModule = OBDMFiModuleIOS as MFiNativeModule;
    this.nativeEmitter = new NativeEventEmitter(OBDMFiModuleIOS);
    this.log('init', 'OBDMFiModuleIOS 已加载');
  }

  async initialize(): Promise<boolean> {
    if (Platform.OS !== 'ios') return false;
    if (!this.nativeModule) {
      this.log('initialize', 'native module unavailable');
      return false;
    }

    try {
      this.log('initialize', 'start');
      const supported = this.nativeModule.isSupported
        ? await this.nativeModule.isSupported()
        : true;

      if (!supported) {
        this.log('initialize', '当前设备不支持 ExternalAccessory');
        return false;
      }

      this.attachListeners();
      this.log('initialize', 'success', { supported });
      return true;
    } catch (e: any) {
      this.log('initialize', 'failed', { error: this.extractErrorMessage(e) });
      return false;
    }
  }

  async startScan(): Promise<void> {
    if (!this.nativeModule) {
      this.eventHandler.onError?.('MFI_UNAVAILABLE', 'MFi模块未加载');
      this.log('scan', 'skip: native module unavailable');
      return;
    }

    try {
      this.log('scan', 'start');
      const accessories = await this.nativeModule.getConnectedAccessories();
      this.log('scan', 'connected accessories fetched', { count: accessories.length });

      for (const accessory of accessories) {
        this.cacheAccessoryHint(accessory);
        const info = this.buildAccessoryInfo(accessory);
        if (!info) continue;
        this.eventHandler.onDeviceDiscovered?.(info);
        this.log('scan', 'discovered accessory', {
          name: info.name,
          address: info.address,
          protocols: accessory.protocolStrings || [],
          valid: info.valid,
        });
      }

      this.eventHandler.onScanFinished?.();
      this.log('scan', 'finished', { count: accessories.length });
    } catch (e: any) {
      this.eventHandler.onError?.('MFI_SCAN_ERROR', e.message);
      this.log('scan', 'failed', { error: this.extractErrorMessage(e) });
    }
  }

  async stopScan(): Promise<void> {
    this.log('scan', 'stop requested');
    this.eventHandler.onScanFinished?.();
  }

  async connect(address: string): Promise<string> {
    if (!this.nativeModule) throw new Error('MFi不可用');
    const normalizedAddress = this.normalizeAddress(address);
    this.selectedAddress = normalizedAddress || address;
    this.log('connect', 'request', { address, normalizedAddress });

    // 清理旧会话，避免旧流事件干扰当前连接
    if (this.nativeSessionId) {
      try {
        await this.nativeModule.closeSession(this.nativeSessionId);
      } catch (e: any) {
        console.warn('[MFi] 关闭旧会话失败:', e?.message || e);
      }
      this.cleanupSessionState();
    }

    const accessories = await this.waitForAccessories(
      MFI_CONNECT_MATCH_TIMEOUT_MS,
      MFI_CONNECT_MATCH_INTERVAL_MS
    );

    const match = this.findAccessory(accessories, normalizedAddress);
    let targetAccessory = match.accessory;
    let matchReason = match.reason;

    if (!targetAccessory) {
      const fallback = this.pickFallbackAccessory(accessories);
      if (fallback) {
        targetAccessory = fallback.accessory;
        matchReason = fallback.reason;
      }
    }

    if (!targetAccessory) {
      const available = this.summarizeAccessories(accessories);
      this.log('connect', 'target accessory not found', {
        address: normalizedAddress || address,
        availableCount: accessories.length,
        available,
      });
      throw new Error(`未找到 MFi 设备: ${address || 'empty'}; 可用设备=${available || 'none'}`);
    }

    const accessoryId = this.getAccessoryAddress(targetAccessory);
    if (!accessoryId) {
      throw new Error('MFi设备标识无效，无法建立会话');
    }

    this.cacheAccessoryHint(targetAccessory);
    this.log('connect', 'target accessory matched', {
      selected: normalizedAddress || address,
      resolvedAccessoryId: accessoryId,
      reason: matchReason,
      accessoryName: targetAccessory.name || 'MFi Device',
      availableCount: accessories.length,
    });

    const protocolCandidates = this.getProtocolCandidates(targetAccessory.protocolStrings);
    if (protocolCandidates.length === 0) {
      this.log('connect', 'no protocol candidates', { address, protocols: targetAccessory.protocolStrings || [] });
      throw new Error('设备未暴露受支持的 OBD 协议，请检查配件协议白名单');
    }
    this.log('connect', 'protocol candidates prepared', {
      address: accessoryId,
      candidates: protocolCandidates,
      accessoryName: targetAccessory.name || 'MFi Device',
    });

    let lastError: unknown = null;
    for (let i = 0; i < protocolCandidates.length; i += 1) {
      const protocol = protocolCandidates[i];
      try {
        this.log('connect', 'attempt openSession', { protocol, attempt: i + 1, total: protocolCandidates.length });
        this.nativeSessionId = await this.nativeModule.openSession(accessoryId, protocol);
        this.currentProtocol = protocol;
        this.connectedAccessoryId = accessoryId;
        this.sessionId = `mfi-${Date.now()}`;
        this.lastActivityAt = Date.now();
        this.connectionOpenedAt = Date.now();
        this.txPackets = 0;
        this.rxPackets = 0;
        this.sendQueue = Promise.resolve();
        this.sendQueueError = null;
        this.log('connect', 'success', {
          protocol,
          attempt: i + 1,
          total: protocolCandidates.length,
          matchReason,
        });
        return this.sessionId;
      } catch (e: any) {
        lastError = e;
        this.log('connect', 'attempt failed', {
          protocol,
          attempt: i + 1,
          total: protocolCandidates.length,
          error: this.extractErrorMessage(e),
        });
        if (i < protocolCandidates.length - 1) {
          await this.sleep(MFI_CONNECT_RETRY_DELAY_MS);
        }
      }
    }

    this.log('connect', 'all attempts failed', { address: accessoryId, error: this.extractErrorMessage(lastError) });
    throw new Error(`MFi连接失败: ${this.extractErrorMessage(lastError)}`);
  }

  async send(base64Data: string): Promise<void> {
    if (!this.nativeModule || !this.nativeSessionId) {
      throw new Error('MFi未连接');
    }
    if (this.sendQueueError) {
      this.log('send', 'queue had previous error', { error: this.sendQueueError });
    }

    const sessionSnapshot = this.nativeSessionId;
    const queuePendingAt = Date.now();
    const preview = this.previewBase64(base64Data);
    const payloadBytes = this.base64ByteLength(base64Data);
    const txSeq = this.txSequence + 1;
    this.txSequence = txSeq;
    this.log('send', 'enqueue', {
      seq: txSeq,
      bytes: payloadBytes,
      preview,
    });
    const sendTask = this.sendQueue.then(async () => {
      if (!this.nativeModule || !this.nativeSessionId || this.nativeSessionId !== sessionSnapshot) {
        throw new Error('MFi会话已关闭或已切换');
      }
      await this.nativeModule.send(sessionSnapshot, base64Data);
      this.lastActivityAt = Date.now();
      this.txPackets += 1;
      this.log('send', 'sent', {
        seq: txSeq,
        waitMs: Date.now() - queuePendingAt,
        bytes: payloadBytes,
        preview,
      });
    });

    this.sendQueue = sendTask.then(
      () => undefined,
      (error) => {
        this.sendQueueError = this.extractErrorMessage(error);
        this.log('send', 'failed', {
          seq: txSeq,
          error: this.sendQueueError,
          waitMs: Date.now() - queuePendingAt,
          bytes: payloadBytes,
          preview,
        });
      }
    );

    await sendTask;
  }

  async disconnect(): Promise<void> {
    this.log('disconnect', 'request');
    this.flushRxBuffer('disconnect');
    if (this.nativeModule && this.nativeSessionId) {
      await this.nativeModule.closeSession(this.nativeSessionId);
    }
    this.cleanupSessionState();
  }

  getSessionId(): string | null {
    return this.sessionId;
  }

  destroy(): void {
    this.log('destroy', 'adapter destroy');
    if (this.dataSubscription) {
      this.dataSubscription.remove();
      this.dataSubscription = null;
    }
    if (this.closeSubscription) {
      this.closeSubscription.remove();
      this.closeSubscription = null;
    }
    if (this.accessoryConnectSubscription) {
      this.accessoryConnectSubscription.remove();
      this.accessoryConnectSubscription = null;
    }
    if (this.accessoryDisconnectSubscription) {
      this.accessoryDisconnectSubscription.remove();
      this.accessoryDisconnectSubscription = null;
    }
    this.listenersAttached = false;
    this.cleanupSessionState();
  }

  private attachListeners(): void {
    if (!this.nativeEmitter || this.listenersAttached) return;
    this.listenersAttached = true;
    this.log('listener', 'attach native listeners');

    this.dataSubscription = this.nativeEmitter.addListener(
      MFI_DATA_EVENT,
      (payload: MFiNativeEventPayload) => {
        if (!this.sessionId || !this.nativeSessionId) return;
        if (!payload?.sessionId || payload.sessionId !== this.nativeSessionId) return;
        this.handleNativeDataChunk(payload.base64Data || '');
      }
    );

    this.closeSubscription = this.nativeEmitter.addListener(
      MFI_SESSION_CLOSED_EVENT,
      (payload: MFiNativeEventPayload) => {
        if (!this.sessionId || !this.nativeSessionId) return;
        if (!payload?.sessionId || payload.sessionId !== this.nativeSessionId) return;

        const reason = payload.reason || 'MFi session closed';
        const closedSession = this.sessionId;
        this.flushRxBuffer('session_closed');
        this.log('disconnect', 'session closed event', { reason });
        this.cleanupSessionState();
        this.eventHandler.onConnectionLost?.(closedSession, reason);
      }
    );

    this.accessoryConnectSubscription = this.nativeEmitter.addListener(
      MFI_ACCESSORY_CONNECTED_EVENT,
      (accessory: MFiAccessoryInfo) => {
        const info = this.buildAccessoryInfo(accessory);
        this.cacheAccessoryHint(accessory);
        this.log('scan', 'accessory connected event', {
          address: info?.address || null,
          name: info?.name || null,
          protocols: accessory?.protocolStrings || [],
        });
        if (info) {
          this.eventHandler.onDeviceDiscovered?.(info);
        }
      }
    );

    this.accessoryDisconnectSubscription = this.nativeEmitter.addListener(
      MFI_ACCESSORY_DISCONNECTED_EVENT,
      (accessory: MFiAccessoryInfo) => {
        this.log('scan', 'accessory disconnected event', {
          address: this.getAccessoryAddress(accessory),
          name: accessory?.name || null,
          protocols: accessory?.protocolStrings || [],
        });
      }
    );
  }

  private cleanupSessionState(): void {
    const hadSession = !!this.sessionId || !!this.nativeSessionId;
    if (hadSession) {
      const uptimeMs = this.connectionOpenedAt > 0 ? Date.now() - this.connectionOpenedAt : 0;
      this.log('session', 'cleanup', {
        uptimeMs,
        idleMs: this.lastActivityAt > 0 ? Date.now() - this.lastActivityAt : null,
        bufferedBytes: this.rxBufferedBytes,
      });
    }
    this.clearRxAggregation('session_cleanup');
    this.nativeSessionId = null;
    this.sessionId = null;
    this.connectedAccessoryId = null;
    this.selectedAddress = null;
    this.currentProtocol = null;
    this.sendQueue = Promise.resolve();
    this.sendQueueError = null;
    this.lastActivityAt = 0;
    this.connectionOpenedAt = 0;
    this.txPackets = 0;
    this.rxPackets = 0;
    this.txSequence = 0;
    this.rxChunkSequence = 0;
    this.rxResponseSequence = 0;
  }

  private handleNativeDataChunk(base64Data: string): void {
    if (!this.sessionId) return;

    const chunk = this.decodeBase64(base64Data);
    const chunkSeq = this.rxChunkSequence + 1;
    this.rxChunkSequence = chunkSeq;
    this.lastActivityAt = Date.now();
    this.rxPackets += 1;

    const hasPrompt = chunk.includes(0x3e);
    const hasCR = chunk.includes(0x0d);
    const hasLF = chunk.includes(0x0a);
    const chunkBytes = chunk.length;
    const flushDelayMs = hasPrompt ? MFI_RX_PROMPT_FLUSH_DELAY_MS : MFI_RX_FLUSH_TIMEOUT_MS;

    if (chunkBytes > 0) {
      this.rxChunkBuffers.push(chunk);
      this.rxBufferedBytes += chunkBytes;
    }

    this.log('rx', 'chunk received', {
      chunkSeq,
      chunkBytes,
      bufferedBytes: this.rxBufferedBytes,
      hasPrompt,
      hasCR,
      hasLF,
      preview: this.previewBuffer(chunk),
      flushDelayMs,
    });

    if (this.rxBufferedBytes >= MFI_RX_MAX_BUFFER_BYTES) {
      this.flushRxBuffer('max_buffer');
      return;
    }

    this.scheduleRxFlush(flushDelayMs);
  }

  private scheduleRxFlush(delayMs: number): void {
    if (this.rxFlushTimer) {
      clearTimeout(this.rxFlushTimer);
    }
    this.rxFlushTimer = setTimeout(() => {
      this.rxFlushTimer = null;
      this.flushRxBuffer('idle_or_prompt');
    }, delayMs);
  }

  private flushRxBuffer(reason: string): void {
    if (this.rxFlushTimer) {
      clearTimeout(this.rxFlushTimer);
      this.rxFlushTimer = null;
    }
    if (!this.sessionId || this.rxChunkBuffers.length === 0 || this.rxBufferedBytes === 0) {
      return;
    }

    const chunkCount = this.rxChunkBuffers.length;
    const response = Buffer.concat(this.rxChunkBuffers, this.rxBufferedBytes);
    this.rxChunkBuffers = [];
    this.rxBufferedBytes = 0;

    const responseSeq = this.rxResponseSequence + 1;
    this.rxResponseSequence = responseSeq;
    this.lastActivityAt = Date.now();

    this.log('rx', 'flush aggregated response', {
      responseSeq,
      reason,
      chunks: chunkCount,
      bytes: response.length,
      hasPrompt: response.includes(0x3e),
      hasCR: response.includes(0x0d),
      hasLF: response.includes(0x0a),
      preview: this.previewBuffer(response),
      fullText: this.formatBufferForLog(response),
    });
    this.eventHandler.onDataReceived?.(this.sessionId, response.toString('base64'));
  }

  private clearRxAggregation(reason: string): void {
    if (this.rxFlushTimer) {
      clearTimeout(this.rxFlushTimer);
      this.rxFlushTimer = null;
    }
    if (this.rxChunkBuffers.length > 0 && this.rxBufferedBytes > 0) {
      const pending = Buffer.concat(this.rxChunkBuffers, this.rxBufferedBytes);
      this.log('rx', 'drop buffered response', {
        reason,
        chunks: this.rxChunkBuffers.length,
        bytes: pending.length,
        preview: this.previewBuffer(pending),
      });
    }
    this.rxChunkBuffers = [];
    this.rxBufferedBytes = 0;
  }

  private async waitForAccessories(timeoutMs: number, intervalMs: number): Promise<MFiAccessoryInfo[]> {
    if (!this.nativeModule) return [];

    const startedAt = Date.now();
    let accessories: MFiAccessoryInfo[] = [];

    while (Date.now() - startedAt <= timeoutMs) {
      accessories = await this.nativeModule.getConnectedAccessories();
      for (const accessory of accessories) {
        this.cacheAccessoryHint(accessory);
      }

      if (accessories.length > 0) {
        const match = this.findAccessory(accessories, this.normalizeAddress(this.selectedAddress || ''));
        if (match.accessory) {
          return accessories;
        }
      }

      await this.sleep(intervalMs);
    }

    return accessories;
  }

  private getAccessoryAddress(accessory: MFiAccessoryInfo): string {
    const serial = String(accessory.serialNumber || '').trim();
    if (serial.length > 0) return serial;
    const connectionId = String(accessory.connectionID ?? '').trim();
    return connectionId;
  }

  private buildAccessoryInfo(accessory: MFiAccessoryInfo): ExtendedBTDeviceInfo | null {
    const address = this.getAccessoryAddress(accessory);
    if (!address) return null;
    return {
      name: accessory.name || 'MFi Device',
      address,
      rssi: -50,
      valid: this.isOBDAccessory(accessory),
      paired: true,
      protocol: 'mfi',
    };
  }

  private findAccessory(accessories: MFiAccessoryInfo[], address: string): { accessory: MFiAccessoryInfo | null; reason: string } {
    const target = this.normalizeAddress(address);
    if (!target) return { accessory: null, reason: 'empty_target' };

    const bySerial = accessories.find((item) => this.normalizeAddress(item.serialNumber || '') === target);
    if (bySerial) return { accessory: bySerial, reason: 'serial_exact' };

    const byConn = accessories.find((item) => this.normalizeAddress(item.connectionID ?? '') === target);
    if (byConn) return { accessory: byConn, reason: 'connection_id_exact' };

    const hint = this.accessoryHintsByAddress.get(target);
    if (hint) {
      const hintedSerial = this.normalizeAddress(hint.serial);
      if (hintedSerial) {
        const byHintSerial = accessories.find((item) => this.normalizeAddress(item.serialNumber || '') === hintedSerial);
        if (byHintSerial) return { accessory: byHintSerial, reason: 'hint_serial' };
      }

      const hintedConn = this.normalizeAddress(hint.connectionId);
      if (hintedConn) {
        const byHintConn = accessories.find((item) => this.normalizeAddress(item.connectionID ?? '') === hintedConn);
        if (byHintConn) return { accessory: byHintConn, reason: 'hint_connection_id' };
      }

      const hintedName = this.normalizeName(hint.name);
      if (hintedName) {
        const sameName = accessories.filter((item) => this.normalizeName(item.name || '') === hintedName);
        if (sameName.length === 1) {
          return { accessory: sameName[0], reason: 'hint_name_unique' };
        }
      }
    }

    return { accessory: null, reason: 'not_found' };
  }

  private pickFallbackAccessory(accessories: MFiAccessoryInfo[]): { accessory: MFiAccessoryInfo; reason: string } | null {
    if (!accessories || accessories.length === 0) return null;

    const supported = accessories.filter((item) => this.isOBDAccessory(item));
    if (supported.length === 1) {
      return { accessory: supported[0], reason: 'single_supported_fallback' };
    }

    if (accessories.length === 1) {
      return { accessory: accessories[0], reason: 'single_accessory_fallback' };
    }

    return null;
  }

  private cacheAccessoryHint(accessory: MFiAccessoryInfo): void {
    const address = this.normalizeAddress(this.getAccessoryAddress(accessory));
    if (!address) return;

    const hint = {
      name: String(accessory.name || '').trim(),
      serial: String(accessory.serialNumber || '').trim(),
      connectionId: String(accessory.connectionID ?? '').trim(),
    };
    this.accessoryHintsByAddress.set(address, hint);
  }

  private summarizeAccessories(accessories: MFiAccessoryInfo[]): string {
    if (!accessories || accessories.length === 0) return '';
    return accessories
      .slice(0, MFI_ACCESSORY_SUMMARY_LIMIT)
      .map((item) => {
        const name = String(item.name || 'MFi Device').trim();
        const serial = String(item.serialNumber || '').trim();
        const conn = String(item.connectionID ?? '').trim();
        return `${name}{serial=${serial || '-'},conn=${conn || '-'}}`;
      })
      .join(', ');
  }

  private normalizeAddress(value: unknown): string {
    return String(value ?? '').trim().toUpperCase();
  }

  private normalizeName(value: string): string {
    return String(value || '').trim().toUpperCase();
  }

  private getProtocolCandidates(protocolStrings?: string[]): string[] {
    const protocols = (protocolStrings || [])
      .map(p => String(p || '').trim())
      .filter(p => p.length > 0);

    if (protocols.length === 0) return [];

    const uniqueProtocols = Array.from(new Set(protocols));
    const candidates: string[] = [];

    for (const preferred of MFI_PROTOCOL_CANDIDATES) {
      const matched = uniqueProtocols.find(p => p.toLowerCase() === preferred.toLowerCase());
      if (matched && !candidates.includes(matched)) {
        candidates.push(matched);
      }
    }

    for (const p of uniqueProtocols) {
      if (p.toLowerCase().includes('obd') && !candidates.includes(p)) {
        candidates.push(p);
      }
    }

    for (const p of uniqueProtocols) {
      if (!candidates.includes(p)) {
        candidates.push(p);
      }
    }

    return candidates;
  }

  private extractErrorMessage(error: unknown): string {
    if (error instanceof Error) return error.message;
    if (typeof error === 'string') return error;
    if (error && typeof error === 'object') {
      const maybeMessage = (error as any).message;
      if (typeof maybeMessage === 'string' && maybeMessage.length > 0) {
        return maybeMessage;
      }
    }
    return 'unknown_error';
  }

  private async sleep(ms: number): Promise<void> {
    await new Promise(resolve => setTimeout(resolve, ms));
  }

  private log(stage: string, message: string, extra?: Record<string, unknown>): void {
    const context: Record<string, unknown> = {
      sessionId: this.sessionId || null,
      nativeSessionId: this.nativeSessionId || null,
      accessoryId: this.connectedAccessoryId || null,
      protocol: this.currentProtocol || null,
      txPackets: this.txPackets,
      rxPackets: this.rxPackets,
      ...extra,
    };
    console.log(`[MFi][${stage}] ${message} ${this.safeJson(context)}`);
  }

  private safeJson(value: unknown): string {
    try {
      return JSON.stringify(value);
    } catch {
      return '{"serialize":"failed"}';
    }
  }

  private decodeBase64(base64: string): Buffer {
    if (!base64) return Buffer.alloc(0);
    try {
      return Buffer.from(base64, 'base64');
    } catch {
      return Buffer.alloc(0);
    }
  }

  private base64ByteLength(base64: string): number {
    return this.decodeBase64(base64).length;
  }

  private formatBufferForLog(buffer: Buffer, maxBytes?: number): string {
    if (!buffer || buffer.length === 0) return '';
    try {
      const text = buffer
        .toString('utf-8')
        .replace(/\r/g, '\\r')
        .replace(/\n/g, '\\n');
      if (typeof maxBytes === 'number' && text.length > maxBytes) {
        return `${text.slice(0, maxBytes)}...`;
      }
      return text;
    } catch {
      return '[decode_failed]';
    }
  }

  private previewBuffer(buffer: Buffer): string {
    return this.formatBufferForLog(buffer, MFI_LOG_PREVIEW_BYTES);
  }

  private previewBase64(base64: string): string {
    return this.previewBuffer(this.decodeBase64(base64));
  }

  private isOBDAccessory(accessory: any): boolean {
    const name = (accessory.name || '').toUpperCase();
    const protocols = (accessory.protocolStrings || []).map((p: string) => String(p).toUpperCase());
    const keywords = ['OBD', 'ELM', 'CAR', 'DIAG'];
    const protocolHints = ['OBD', 'ELM', 'VGATE', 'OBDLINK'];
    return keywords.some(k => name.includes(k)) || protocols.some((p: string) => protocolHints.some(h => p.includes(h)));
  }
}

export class BluetoothGateway {
  private bleAdapter: BLEAdapter;
  private classicAdapter: ClassicBluetoothAdapter;
  private mfiAdapter: MFiAdapter;
  private activeAdapter: BLEAdapter | ClassicBluetoothAdapter | MFiAdapter | null = null;
  private activeProtocol: BluetoothProtocol | null = null;
  private eventHandler: BluetoothEventHandler;

  private dataReceivedListeners: Array<(sessionId: string, base64Data: string) => void> = [];
  private deviceDiscoveredListeners: Array<(device: ExtendedBTDeviceInfo) => void> = [];
  private scanFinishedListeners: Array<() => void> = [];
  private connectionLostListeners: Array<(sessionId: string, reason: string) => void> = [];

  constructor(eventHandler: BluetoothEventHandler) {
    this.eventHandler = {
      ...eventHandler,
      onDataReceived: (sessionId: string, base64Data: string) => {
        eventHandler.onDataReceived?.(sessionId, base64Data);
        this.dataReceivedListeners.forEach(listener => {
          try {
            listener(sessionId, base64Data);
          } catch (e) {
            console.error('[BluetoothGateway][iOS] Data listener error:', e);
          }
        });
      },
      onDeviceDiscovered: (device: ExtendedBTDeviceInfo) => {
        eventHandler.onDeviceDiscovered?.(device);
        this.deviceDiscoveredListeners.forEach(listener => {
          try {
            listener(device);
          } catch (e) {
            console.error('[BluetoothGateway][iOS] Device listener error:', e);
          }
        });
      },
      onScanFinished: () => {
        eventHandler.onScanFinished?.();
        this.scanFinishedListeners.forEach(listener => {
          try {
            listener();
          } catch (e) {
            console.error('[BluetoothGateway][iOS] Scan listener error:', e);
          }
        });
      },
      onConnectionLost: (sessionId: string, reason: string) => {
        eventHandler.onConnectionLost?.(sessionId, reason);
        this.connectionLostListeners.forEach(listener => {
          try {
            listener(sessionId, reason);
          } catch (e) {
            console.error('[BluetoothGateway][iOS] ConnectionLost listener error:', e);
          }
        });
      },
    };

    this.bleAdapter = new BLEAdapter(this.eventHandler);
    this.classicAdapter = new ClassicBluetoothAdapter(this.eventHandler);
    this.mfiAdapter = new MFiAdapter(this.eventHandler);
  }

  addDataReceivedListener(listener: (sessionId: string, base64Data: string) => void): () => void {
    this.dataReceivedListeners.push(listener);
    console.log('[BluetoothGateway][iOS] Data listener added, total:', this.dataReceivedListeners.length);
    return () => {
      this.dataReceivedListeners = this.dataReceivedListeners.filter(l => l !== listener);
      console.log('[BluetoothGateway][iOS] Data listener removed, total:', this.dataReceivedListeners.length);
    };
  }

  addDeviceDiscoveredListener(listener: (device: ExtendedBTDeviceInfo) => void): () => void {
    this.deviceDiscoveredListeners.push(listener);
    console.log('[BluetoothGateway][iOS] Device listener added, total:', this.deviceDiscoveredListeners.length);
    return () => {
      this.deviceDiscoveredListeners = this.deviceDiscoveredListeners.filter(l => l !== listener);
      console.log('[BluetoothGateway][iOS] Device listener removed, total:', this.deviceDiscoveredListeners.length);
    };
  }

  addScanFinishedListener(listener: () => void): () => void {
    this.scanFinishedListeners.push(listener);
    console.log('[BluetoothGateway][iOS] Scan listener added, total:', this.scanFinishedListeners.length);
    return () => {
      this.scanFinishedListeners = this.scanFinishedListeners.filter(l => l !== listener);
      console.log('[BluetoothGateway][iOS] Scan listener removed, total:', this.scanFinishedListeners.length);
    };
  }

  addConnectionLostListener(listener: (sessionId: string, reason: string) => void): () => void {
    this.connectionLostListeners.push(listener);
    console.log('[BluetoothGateway][iOS] ConnectionLost listener added, total:', this.connectionLostListeners.length);
    return () => {
      this.connectionLostListeners = this.connectionLostListeners.filter(l => l !== listener);
      console.log('[BluetoothGateway][iOS] ConnectionLost listener removed, total:', this.connectionLostListeners.length);
    };
  }

  async initialize(): Promise<{ ble: boolean; classic: boolean; mfi: boolean }> {
    const results = await Promise.all([
      this.bleAdapter.initialize(),
      this.classicAdapter.initialize(),
      this.mfiAdapter.initialize(),
    ]);

    return {
      ble: results[0],
      classic: results[1],
      mfi: results[2],
    };
  }

  async startScan(protocols?: BluetoothProtocol[]): Promise<void> {
    const scanProtocols = protocols || ['ble', 'classic', 'mfi'];
    const promises: Promise<void>[] = [];

    if (scanProtocols.includes('ble')) {
      promises.push(this.bleAdapter.startScan());
    }
    if (scanProtocols.includes('classic') && Platform.OS === 'android') {
      promises.push(this.classicAdapter.startScan());
    }
    if (scanProtocols.includes('mfi') && Platform.OS === 'ios') {
      promises.push(this.mfiAdapter.startScan());
    }

    await Promise.allSettled(promises);
  }

  async stopScan(): Promise<void> {
    await Promise.allSettled([
      this.bleAdapter.stopScan(),
      this.classicAdapter.stopScan(),
      this.mfiAdapter.stopScan(),
    ]);
  }

  async connect(protocol: BluetoothProtocol, address: string): Promise<string> {
    await this.stopScan();

    console.log(`[BluetoothGateway][iOS] connect protocol=${protocol} address=${address}`);

    if (protocol === 'classic' && this.classicAdapter.isConnectedTo(address)) {
      const existing = this.classicAdapter.getSessionId();
      if (existing) {
        console.log('[BluetoothGateway][iOS] 已连接目标经典蓝牙，跳过重连');
        this.activeAdapter = this.classicAdapter;
        this.activeProtocol = 'classic';
        return existing;
      }
    }

    await this.disconnect();

    let sessionId: string;
    switch (protocol) {
      case 'ble':
        sessionId = await this.bleAdapter.connect(address);
        this.activeAdapter = this.bleAdapter;
        break;
      case 'classic':
        sessionId = await this.classicAdapter.connect(address);
        this.activeAdapter = this.classicAdapter;
        break;
      case 'mfi':
        sessionId = await this.mfiAdapter.connect(address);
        this.activeAdapter = this.mfiAdapter;
        break;
      default:
        throw new Error(`未知协议: ${protocol}`);
    }

    this.activeProtocol = protocol;
    console.log(`[BluetoothGateway][iOS] connect success protocol=${protocol} session=${sessionId}`);
    return sessionId;
  }

  async send(base64Data: string): Promise<void> {
    if (!this.activeAdapter) {
      throw new Error('未连接到设备');
    }
    console.log(`[BluetoothGateway][iOS] send protocol=${this.activeProtocol ?? 'unknown'} len=${base64Data?.length ?? 0}`);
    await this.activeAdapter.send(base64Data);
  }

  async disconnect(): Promise<void> {
    if (this.activeAdapter) {
      console.log(`[BluetoothGateway][iOS] disconnect protocol=${this.activeProtocol ?? 'unknown'}`);
      await this.activeAdapter.disconnect();
    }
    this.activeAdapter = null;
    this.activeProtocol = null;
  }

  getActiveProtocol(): BluetoothProtocol | null {
    return this.activeProtocol;
  }

  hasActiveSession(): boolean {
    return !!(
      this.classicAdapter.getSessionId?.() ||
      this.bleAdapter.getSessionId?.() ||
      this.mfiAdapter.getSessionId?.()
    );
  }

  destroy(): void {
    this.bleAdapter.destroy();
    this.mfiAdapter.destroy();
    this.activeAdapter = null;
    this.activeProtocol = null;
  }
}
