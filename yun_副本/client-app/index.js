/**
 * OBD蓝牙网关客户端入口点
 * @format
 */

// Buffer polyfill for React Native
import { Buffer } from 'buffer';
global.Buffer = Buffer;
// btoa/atob polyfill for React Native
if (typeof global.btoa !== 'function') {
  global.btoa = (data) => Buffer.from(data, 'binary').toString('base64');
}
if (typeof global.atob !== 'function') {
  global.atob = (base64) => Buffer.from(base64, 'base64').toString('binary');
}

import { AppRegistry, NativeModules } from 'react-native';
import App from './src/App';

const appName = 'OBDGatewayClient';

const setupLogBridge = () => {
  if (global.__RN_LOG_BRIDGE_PATCHED__) return;
  global.__RN_LOG_BRIDGE_PATCHED__ = true;

  const logBridge = NativeModules?.LogBridge;
  const hasBridge = !!logBridge?.log;
  const maxLen = 3800;

  const formatArg = (arg) => {
    if (arg === null) return 'null';
    if (arg === undefined) return 'undefined';
    if (arg instanceof Error) return arg.stack || `${arg.name}: ${arg.message}`;
    if (typeof arg === 'string') return arg;
    try {
      return JSON.stringify(arg);
    } catch {
      return String(arg);
    }
  };

  const emit = (level, args) => {
    if (!hasBridge) return;
    try {
      let msg = args.map(formatArg).join(' ');
      if (msg.length > maxLen) {
        msg = `${msg.slice(0, maxLen)}...`;
      }
      logBridge.log(level, msg);
    } catch {
      // ignore logging errors
    }
  };

  const original = {
    log: console.log?.bind(console),
    info: console.info?.bind(console),
    warn: console.warn?.bind(console),
    error: console.error?.bind(console),
    debug: console.debug?.bind(console),
  };

  console.log = (...args) => {
    emit('info', args);
    original.log?.(...args);
  };
  console.info = (...args) => {
    emit('info', args);
    original.info?.(...args);
  };
  console.warn = (...args) => {
    emit('warn', args);
    original.warn?.(...args);
  };
  console.error = (...args) => {
    emit('error', args);
    original.error?.(...args);
  };
  console.debug = (...args) => {
    emit('info', args);
    original.debug?.(...args);
  };

  if (hasBridge) {
    original.log?.('[LogBridge] native log bridge enabled');
  }
};

setupLogBridge();

AppRegistry.registerComponent(appName, () => App);
