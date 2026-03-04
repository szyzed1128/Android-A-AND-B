module.exports = {
  dependencies: {
    // 禁用 react-native-bluetooth-classic 在 iOS 上的自动链接
    // iOS 不支持经典蓝牙 (SPP)，只在 Android 上使用
    'react-native-bluetooth-classic': {
      platforms: {
        ios: null, // 禁用 iOS
      },
    },
    // 禁用旧的 ble-plx 库（已切换到 ble-manager）
    'react-native-ble-plx': {
      platforms: {
        ios: null,
      },
    },
  },
};
