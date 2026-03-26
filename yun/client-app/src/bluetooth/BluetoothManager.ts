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
 * - react-native-external-accessory (MFi/iOS)
 */
import { Platform, NativeModules, NativeEventEmitter } from 'react-native';
import { Buffer } from 'buffer';
import { BTDeviceInfo } from '../protocol/MessageProtocol';

// 动态导入 BleManager，防止原生模块未链接时崩溃
let BleManager: any = null;
let bleManagerEmitter: NativeEventEmitter | null = null;

// 调试: 列出所有原生模块
console.log('[BLE] Platform:', Platform.OS);
console.log('[BLE] 可用的原生模块:', Object.keys(NativeModules).filter(k => k.toLowerCase().includes('ble') || k.toLowerCase().includes('bluetooth')));
console.log('[BLE] NativeModules.BleManager 存在:', !!NativeModules.BleManager);

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

// [FLOW] 全链路追踪日志工具函数
function flowLog(direction: string, description: string, detail?: string): void {
  const d = new Date();
  const ts = `[${String(d.getHours()).padStart(2,'0')}:${String(d.getMinutes()).padStart(2,'0')}:${String(d.getSeconds()).padStart(2,'0')}.${String(d.getMilliseconds()).padStart(3,'0')}]`;
  const dir = direction.padEnd(10);
  const detailStr = detail ? ` | ${String(detail).substring(0, 120)}` : '';
  console.log(`[FLOW] ${ts} ${dir} | ${description}${detailStr}`);
}

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
      // 扫描所有设备，持续30秒；allowDuplicates=false，内置去重逻辑已覆盖更新场景
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
      // 只在有名称时记录，减少日志噪音
      if (deviceName) {
        console.log('[BLE] 跳过不可连接设备:', deviceName);
      }
      return;
    }

    // 调试：只打印可连接设备的详细信息
    console.log('[BLE] 可连接设备:', {
      id: deviceId,
      name: deviceName,
      localName: peripheral.advertising?.localName,
      isConnectable,
      rssi: peripheral.rssi,
      serviceUUIDs: peripheral.advertising?.serviceUUIDs,
    });

    const existingDevice = this.discoveredDevices.get(deviceId);

    // 去重逻辑
    if (existingDevice) {
      // 如果已有设备且名称相同，跳过
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

    this.discoveredDevices.set(deviceId, info);
    console.log('[BLE] 发现设备:', info.name, info.address, 'RSSI:', info.rssi);
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
            flowLog('ELM>B', '(BLE)收到数据', decoded.substring(0, 60).replace(/\r/g, '\\r').replace(/\n/g, '\\n'));

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
    flowLog('B>ELM', '(BLE)发送命令', decoded.replace(/\r/g, '\\r').replace(/\n/g, '\\n'));

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
          flowLog('ELM>B', '(ClassicBT)收到数据', decoded.substring(0, 60).replace(/\r/g, '\\r').replace(/\n/g, '\\n'));

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
    flowLog('B>ELM', '(ClassicBT)发送命令', decoded.replace(/\r/g, '\\r').replace(/\n/g, '\\n'));

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
  private eaManager: any = null;
  private connectedAccessory: any = null;
  private sessionId: string | null = null;
  private eventHandler: BluetoothEventHandler;

  constructor(eventHandler: BluetoothEventHandler) {
    this.eventHandler = eventHandler;
    this.initModule();
  }

  private initModule(): void {
    // MFi 模块未安装，暂时禁用
    console.log('MFi adapter disabled - react-native-external-accessory not installed');
    return;
  }

  async initialize(): Promise<boolean> {
    if (Platform.OS !== 'ios') return false;
    return this.eaManager != null;
  }

  async startScan(): Promise<void> {
    if (!this.eaManager) {
      this.eventHandler.onError?.('MFI_UNAVAILABLE', 'MFi模块未加载');
      return;
    }

    try {
      const accessories = await this.eaManager.getConnectedAccessories();

      for (const accessory of accessories) {
        const info: ExtendedBTDeviceInfo = {
          name: accessory.name || 'MFi Device',
          address: accessory.serialNumber || accessory.connectionID,
          rssi: -50,
          valid: this.isOBDAccessory(accessory),
          paired: true,
          protocol: 'mfi',
        };
        this.eventHandler.onDeviceDiscovered?.(info);
      }

      this.eventHandler.onScanFinished?.();
    } catch (e: any) {
      this.eventHandler.onError?.('MFI_SCAN_ERROR', e.message);
    }
  }

  async stopScan(): Promise<void> {
    this.eventHandler.onScanFinished?.();
  }

  async connect(address: string): Promise<string> {
    if (!this.eaManager) throw new Error('MFi不可用');

    const protocol = 'com.obd2.elm327';
    this.connectedAccessory = await this.eaManager.openSession(address, protocol);
    this.sessionId = `mfi-${Date.now()}`;

    this.connectedAccessory.onDataReceived((data: any) => {
      if (this.sessionId) {
        this.eventHandler.onDataReceived?.(this.sessionId, data);
      }
    });

    return this.sessionId;
  }

  async send(base64Data: string): Promise<void> {
    if (!this.connectedAccessory) throw new Error('MFi未连接');
    await this.connectedAccessory.write(base64Data);
  }

  async disconnect(): Promise<void> {
    if (this.connectedAccessory) {
      await this.connectedAccessory.closeSession();
    }
    this.connectedAccessory = null;
    this.sessionId = null;
  }

  getSessionId(): string | null {
    return this.sessionId;
  }

  private isOBDAccessory(accessory: any): boolean {
    const name = (accessory.name || '').toUpperCase();
    const keywords = ['OBD', 'ELM', 'CAR', 'DIAG'];
    return keywords.some(k => name.includes(k));
  }
}

/**
 * 统一蓝牙网关 - 管理三种蓝牙连接方式
 */
export class BluetoothGateway {
  private bleAdapter: BLEAdapter;
  private classicAdapter: ClassicBluetoothAdapter;
  private mfiAdapter: MFiAdapter;
  private activeAdapter: BLEAdapter | ClassicBluetoothAdapter | MFiAdapter | null = null;
  private activeProtocol: BluetoothProtocol | null = null;
  private eventHandler: BluetoothEventHandler;

  // 额外的数据接收监听器（供蓝牙桥接使用）
  private dataReceivedListeners: Array<(sessionId: string, base64Data: string) => void> = [];
  private deviceDiscoveredListeners: Array<(device: ExtendedBTDeviceInfo) => void> = [];
  private scanFinishedListeners: Array<() => void> = [];
  private connectionLostListeners: Array<(sessionId: string, reason: string) => void> = [];

  constructor(eventHandler: BluetoothEventHandler) {
    // 包装eventHandler，将事件分发给额外的监听器
    this.eventHandler = {
      ...eventHandler,
      onDataReceived: (sessionId: string, base64Data: string) => {
        // 调用原始回调
        eventHandler.onDataReceived?.(sessionId, base64Data);
        // 调用额外的监听器
        this.dataReceivedListeners.forEach(listener => {
          try {
            listener(sessionId, base64Data);
          } catch (e) {
            console.error('[BluetoothGateway] Data listener error:', e);
          }
        });
      },
      onDeviceDiscovered: (device: ExtendedBTDeviceInfo) => {
        eventHandler.onDeviceDiscovered?.(device);
        this.deviceDiscoveredListeners.forEach(listener => {
          try {
            listener(device);
          } catch (e) {
            console.error('[BluetoothGateway] Device listener error:', e);
          }
        });
      },
      onScanFinished: () => {
        eventHandler.onScanFinished?.();
        this.scanFinishedListeners.forEach(listener => {
          try {
            listener();
          } catch (e) {
            console.error('[BluetoothGateway] Scan listener error:', e);
          }
        });
      },
      onConnectionLost: (sessionId: string, reason: string) => {
        eventHandler.onConnectionLost?.(sessionId, reason);
        this.connectionLostListeners.forEach(listener => {
          try {
            listener(sessionId, reason);
          } catch (e) {
            console.error('[BluetoothGateway] ConnectionLost listener error:', e);
          }
        });
      },
    };
    this.bleAdapter = new BLEAdapter(this.eventHandler);
    this.classicAdapter = new ClassicBluetoothAdapter(this.eventHandler);
    this.mfiAdapter = new MFiAdapter(this.eventHandler);
  }

  /**
   * 添加数据接收监听器（供蓝牙桥接使用）
   * @returns 取消订阅函数
   */
  addDataReceivedListener(listener: (sessionId: string, base64Data: string) => void): () => void {
    this.dataReceivedListeners.push(listener);
    console.log('[BluetoothGateway] Data listener added, total:', this.dataReceivedListeners.length);
    return () => {
      this.dataReceivedListeners = this.dataReceivedListeners.filter(l => l !== listener);
      console.log('[BluetoothGateway] Data listener removed, total:', this.dataReceivedListeners.length);
    };
  }

  addDeviceDiscoveredListener(listener: (device: ExtendedBTDeviceInfo) => void): () => void {
    this.deviceDiscoveredListeners.push(listener);
    console.log('[BluetoothGateway] Device listener added, total:', this.deviceDiscoveredListeners.length);
    return () => {
      this.deviceDiscoveredListeners = this.deviceDiscoveredListeners.filter(l => l !== listener);
      console.log('[BluetoothGateway] Device listener removed, total:', this.deviceDiscoveredListeners.length);
    };
  }

  addScanFinishedListener(listener: () => void): () => void {
    this.scanFinishedListeners.push(listener);
    console.log('[BluetoothGateway] Scan listener added, total:', this.scanFinishedListeners.length);
    return () => {
      this.scanFinishedListeners = this.scanFinishedListeners.filter(l => l !== listener);
      console.log('[BluetoothGateway] Scan listener removed, total:', this.scanFinishedListeners.length);
    };
  }

  addConnectionLostListener(listener: (sessionId: string, reason: string) => void): () => void {
    this.connectionLostListeners.push(listener);
    console.log('[BluetoothGateway] ConnectionLost listener added, total:', this.connectionLostListeners.length);
    return () => {
      this.connectionLostListeners = this.connectionLostListeners.filter(l => l !== listener);
      console.log('[BluetoothGateway] ConnectionLost listener removed, total:', this.connectionLostListeners.length);
    };
  }

  /**
   * 初始化所有可用的蓝牙适配器
   */
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

  /**
   * 开始扫描（扫描所有可用协议的设备）
   */
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

  /**
   * 停止扫描
   */
  async stopScan(): Promise<void> {
    await Promise.allSettled([
      this.bleAdapter.stopScan(),
      this.classicAdapter.stopScan(),
      this.mfiAdapter.stopScan(),
    ]);
  }

  /**
   * 连接到设备
   * @param protocol 连接协议
   * @param address 设备地址
   */
  async connect(protocol: BluetoothProtocol, address: string): Promise<string> {
    // 连接前先停止扫描，避免蓝牙发现影响 RFCOMM 稳定性
    await this.stopScan();

    console.log(`[BluetoothGateway] connect protocol=${protocol} address=${address}`);

    if (protocol === 'classic' && this.classicAdapter.isConnectedTo(address)) {
      const existing = this.classicAdapter.getSessionId();
      if (existing) {
        console.log('[BluetoothGateway] 已连接目标经典蓝牙，跳过重连');
        this.activeAdapter = this.classicAdapter;
        this.activeProtocol = 'classic';
        return existing;
      }
    }

    // 断开已有连接
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
    console.log(`[BluetoothGateway] connect success protocol=${protocol} session=${sessionId}`);
    return sessionId;
  }

  /**
   * 发送数据
   */
  async send(base64Data: string): Promise<void> {
    if (!this.activeAdapter) {
      throw new Error('未连接到设备');
    }
    console.log(`[BluetoothGateway] send protocol=${this.activeProtocol ?? 'unknown'} len=${base64Data?.length ?? 0}`);
    await this.activeAdapter.send(base64Data);
  }

  /**
   * 断开连接
   */
  async disconnect(): Promise<void> {
    if (this.activeAdapter) {
      console.log(`[BluetoothGateway] disconnect protocol=${this.activeProtocol ?? 'unknown'}`);
      await this.activeAdapter.disconnect();
    }
    this.activeAdapter = null;
    this.activeProtocol = null;
  }

  /**
   * 获取当前活动协议
   */
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

  /**
   * 销毁所有适配器
   */
  destroy(): void {
    this.bleAdapter.destroy();
  }
}
