# 车辆诊断云端化项目 - Agent指引

## 项目概述

这是一个车辆诊断应用云端化改造项目（支持 OBD-II 和 UDS 协议）：
- **软件A（云端）**：Android模拟器中运行原始诊断应用，蓝牙层改造为WebSocket
- **软件B（手机）**：React Native应用，作为蓝牙网关桥接WebSocket与 ELM327

## 架构

```
云端A (Android模拟器) ←WebSocket→ 手机B ←蓝牙→ ELM327
```

## 蓝牙支持

三种协议：BLE、经典蓝牙(SPP)、MFi

## 关键目录

- `analysis/` - DLL分析工具和报告
- `websocket_layer/` - C# WebSocket层（A端）
- `client-app/` - React Native客户端（B端）
- `tools/DllPatcher/` - DLL修补工具
- `deployment/` - 云端部署配置

## DLL改造点

改造 `CarDemo.Android.dll` 中的：
- `AndroidBluetooth2Manager.StartDiscoveringDevices/StopDiscoveringDevices`
- `BluetoothConnectionV3.ConnectAsync`

## 重要文档

- `README.md` - 项目说明
- `PROGRESS.md` - 进度跟踪
- `websocket_layer/PROTOCOL.md` - 通信协议
- `modified_dlls/PATCH_GUIDE.md` - DLL改造指南
