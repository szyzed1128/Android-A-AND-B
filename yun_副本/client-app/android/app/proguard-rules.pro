# React Native ProGuard Rules

-keep class com.facebook.react.** { *; }
-keep class com.facebook.hermes.** { *; }

# 保留蓝牙相关类
-keep class com.polidea.reactnativeble.** { *; }
-keep class com.rusel.RCTBluetoothSerial.** { *; }

# 保留OBD网关类
-keep class com.obdgatewayclient.** { *; }
