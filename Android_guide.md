# NewUI Android 项目说明文档

> 本文档帮助你快速了解这个项目在 Android 方向的工作流程和文件结构。

## 项目概述

这是一个 **OBD 汽车诊断 App** 的 UI 替换项目。核心工作是：
1. 反编译原始 APK
2. 使用 React + Vant 开发新的 WebView UI
3. 注入修改后的 DLL 文件（禁用原生弹窗等）
4. 重新编译、签名 APK

---

## 核心文件结构

```
NewUI/
├── automator.py          # 🔧 核心自动化构建脚本
├── release.keystore      # 🔐 APK 签名密钥
├── original_apk/         # 📦 原始 APK 存放目录
├── my_source_code/       # 📤 构建产物（会被注入到 APK）
├── build_output/         # 📱 最终输出的 APK
├── react_ui/             # ⚛️ React 源码目录
├── modified_dlls/        # 🔨 修改后的 DLL 文件
├── debug_decompiled/     # 🔍 反编译后的 APK 结构参考
└── tools/                # 🛠️ 工具（dnSpy 等）
```

---

## 关键文件详解

### `automator.py` - 自动化构建脚本

**这是最核心的文件**，执行整个 APK 修改流程：

```
工作流程:
1. find_apk()          → 从 original_apk/ 找到原始 APK
2. apktool d           → 反编译 APK 到 temp_decompiled/
3. 注入 UI             → 将 my_source_code/ 复制到 assets/dist/
4. 注入 DLL            → 复制修改后的 DLL 文件
5. 注入 smali          → 复制修改后的 smali（拦截 alert 弹窗）
6. apktool b           → 回编译 APK
7. zipalign            → 内存对齐
8. apksigner           → 签名
9. 输出                → build_output/Release_xxx.apk
```

**依赖工具配置：**
```python
CMD_APKTOOL = ["java", "-jar", "C:\\apktool\\apktool.jar"]
CMD_ZIPALIGN = "C:\\...\\build-tools\\36.1.0\\zipalign.exe"
CMD_APKSIGNER = "C:\\...\\build-tools\\36.1.0\\apksigner.bat"
```

---

### `react_ui/` - React 前端源码

使用 **React 18 + Vant 3 + TypeScript** 开发的移动端 UI。

**技术栈：**
- React 18.2
- React Router 6
- Vant 3 (移动端 UI 组件库)
- Vite 4 (构建工具，支持 legacy 模式兼容 Android 5+)

**目录结构：**
```
react_ui/
├── src/
│   ├── App.tsx              # 路由配置和主组件
│   ├── main.tsx             # 入口文件
│   ├── hooks/
│   │   ├── AppContext.tsx   # 全局状态管理
│   │   └── useOBD.ts        # OBD 通信 Hook
│   └── pages/
│       ├── BluetoothPage.tsx       # 蓝牙连接页面
│       ├── VehicleConfig.tsx       # 车辆配置页面
│       ├── DTCSelectionPage.tsx    # DTC 故障码选择
│       ├── DTCResultPage.tsx       # DTC 结果展示
│       ├── ECUInfoSelectionPage.tsx # ECU 信息选择
│       ├── ECUInfoResultPage.tsx   # ECU 信息结果
│       ├── LiveDataPage.tsx        # 实时数据流页面
│       └── FreezeFramePage.tsx     # 冻结帧页面
├── vite.config.ts           # Vite 配置
└── package.json             # 依赖配置
```

**构建命令：**
```bash
cd react_ui
npm install
npm run build    # 输出到 ../my_source_code/
```

**Vite 配置要点：**
```typescript
export default defineConfig({
  base: './',                    // 使用相对路径（WebView 必须）
  build: {
    outDir: '../my_source_code', // 直接输出到注入目录
  },
  plugins: [
    legacy({ targets: ['android >= 5'] })  // 兼容低版本 Android
  ]
})
```

---

### `my_source_code/` - 构建产物

`npm run build` 后的输出目录，包含：
- `index.html` - 入口 HTML
- `assets/` - JS、CSS 等静态资源

这个目录会被 `automator.py` 注入到 APK 的 `assets/dist/` 目录。

---

### `original_apk/` - 原始 APK

存放未修改的原始 APK 文件。`automator.py` 会自动查找此目录下的 `.apk` 文件。

当前文件：`com.companyname.cardemo-Signed.apk` (~156MB)

---

### `build_output/` - 输出目录

构建完成后的最终 APK 存放位置。

输出文件格式：`Release_{原始APK名}.apk`

---

### `modified_dlls/` - 修改后的 DLL

存放被修改的 .NET DLL 文件（Xamarin 应用的核心程序集）：

| 文件 | 用途 |
|------|------|
| `CarScannerXamarinForms.dll` | 禁用 DisplayAlert 弹窗 |
| `Xamarin.Forms.Core.dll` | 禁用所有 DisplayAlert |

这些 DLL 会被注入到 APK 的 `unknown/assemblies/` 目录。

---

### `debug_decompiled/` - 反编译参考

反编译后的 APK 结构，用于参考和修改：

```
debug_decompiled/
├── AndroidManifest.xml    # 清单文件
├── apktool.yml           # apktool 配置
├── assets/               # 资源文件（包含原始 WebView UI）
├── res/                  # Android 资源
├── smali/                # Dalvik 字节码
├── lib/                  # Native 库
└── unknown/              # 未知文件（包含 .NET assemblies）
    └── assemblies/       # Xamarin DLL 文件
```

**重要：** `smali/crc643f46942d9dd1fff9/FormsWebChromeClient.smali` 被修改用于拦截 JavaScript alert 弹窗。

---

### `tools/` - 开发工具

| 工具 | 用途 |
|------|------|
| `dnSpy/` | .NET 反编译和修改工具，用于修改 DLL |

---

### `release.keystore` - 签名密钥

APK 签名所需的密钥库文件。

配置信息（在 automator.py 中）：
```python
KEY_ALIAS = "my_alias"
KEY_STORE_PASS = "123456"
KEY_PASS = "123456"
```

---

## 临时目录（可忽略）

以下目录是分析/调试过程中产生的临时文件，不是核心流程必需：

| 目录 | 说明 |
|------|------|
| `temp_decompiled/` | automator.py 临时工作目录（自动清理） |
| `temp_analyzer/` | DLL 分析临时文件 |
| `temp_check/` | 检查临时文件 |
| `temp_extract/` | 提取临时文件 |
| `temp_inspect/` | 检查临时文件 |
| `temp_verify/` | 验证临时文件 |
| `temp_verify_new/` | 新验证临时文件 |
| `apk_analysis/` | APK 分析结果 |
| `ipa_analysis/` | iOS IPA 分析结果 |
| `dll_modifier/` | DLL 修改工具项目 |
| `dll_patcher/` | DLL 补丁工具 |

---

## 完整构建流程

```bash
# 1. 开发/修改 React UI
cd react_ui
npm install
npm run dev      # 开发模式预览

# 2. 构建 React UI
npm run build    # 输出到 my_source_code/

# 3. 构建 APK（在 NewUI 目录下）
cd ..
python automator.py

# 4. 输出
# → build_output/Release_com.companyname.cardemo-Signed.apk
```

---

## WebView 与原生通信

React UI 通过全局 `window.obdBridge` 对象与原生 App 通信：

```typescript
// hooks/useOBD.ts
declare global {
  interface Window {
    obdBridge?: {
      sendCommand: (cmd: string) => void;
      // ... 其他方法
    };
  }
}
```

---

## 注意事项

1. **路径问题**：`automator.py` 中的工具路径是 Windows 绝对路径，Mac 上需要修改
2. **签名**：每次重新签名后需要卸载旧版本再安装
3. **Vite base 路径**：必须使用 `'./'` 相对路径，否则 WebView 无法加载资源
4. **Legacy 模式**：为兼容低版本 Android WebView，启用了 legacy 插件

---

## 版本历史

| 版本 | 说明 |
|------|------|
| v01.11 | 优化蓝牙界面 |
| v01.10 | 顶部蓝色部分替换成白色 |
| v01.9 | WebView 蓝色框值从60改到0 |
| v01.8 | 实时数据流UI翻译，连接状态表示，ECUInfo折叠面板优化 |
| v01.7 | 实时数据流功能完成 |

---

*文档生成时间：2026-01-20*