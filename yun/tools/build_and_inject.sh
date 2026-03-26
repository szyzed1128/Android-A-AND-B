#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
YUN_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"
ROOT_DIR="$(cd "${YUN_DIR}/.." && pwd)"

WS_PROJECT="${YUN_DIR}/websocket_layer/OBDCloud.WebSocket.csproj"
INJECTOR_PROJECT="${YUN_DIR}/tools/DllInjector/DllInjector.csproj"
PATCHER_PROJECT="${YUN_DIR}/tools/DllPatcher/DllPatcher.csproj"
OUTPUT_DIR="${YUN_DIR}/modified_dlls"
ASSEMBLIES_DIR="${ROOT_DIR}/temp_verify/unknown/assemblies"

CARDEMO_ANDROID="${ASSEMBLIES_DIR}/CarDemo.Android.dll"
CARDEMO="${ASSEMBLIES_DIR}/CarDemo.dll"
CARSCANNER="${ASSEMBLIES_DIR}/CarScannerXamarinForms.dll"
WS_DLL="${YUN_DIR}/websocket_layer/bin/Release/netstandard2.0/OBDCloud.WebSocket.dll"

# 代理 DLL 的路径（使用 Mono 编译器编译）
PROXY_DLL="${YUN_DIR}/websocket_layer/bin/WebSocketIOBDConnectionProxy.dll"

printf "Building WebSocket layer...\n"
dotnet build "${WS_PROJECT}" -c Release

# 编译 WebSocketIOBDConnectionProxy.dll (需要 Mono 编译器)
printf "\nBuilding WebSocketIOBDConnectionProxy with Mono mcs...\n"
mcs -target:library -nostdlib \
    -r:"${ASSEMBLIES_DIR}/mscorlib.dll" \
    -r:"${ASSEMBLIES_DIR}/netstandard.dll" \
    -r:"${ASSEMBLIES_DIR}/CarScannerXamarinForms.dll" \
    -r:"${ASSEMBLIES_DIR}/System.dll" \
    -r:"${ASSEMBLIES_DIR}/System.Core.dll" \
    -r:"${ASSEMBLIES_DIR}/Newtonsoft.Json.dll" \
    -out:"${PROXY_DLL}" \
    "${YUN_DIR}/websocket_layer/src/WebSocketIOBDConnectionProxy.cs" \
    "${YUN_DIR}/websocket_layer/src/ReplayConnection.cs"

printf "Building injector...\n"
dotnet build "${INJECTOR_PROJECT}" -c Release

printf "Running injector...\n"
dotnet run --project "${INJECTOR_PROJECT}" -c Release -- \
  "${CARDEMO_ANDROID}" \
  "${CARDEMO}" \
  "${WS_DLL}" \
  "${OUTPUT_DIR}" \
  "${PROXY_DLL}"

# 构建并运行 DllPatcher（修改 CarScannerXamarinForms.dll，添加 WebSocket 字段）
printf "\nBuilding patcher...\n"
dotnet build "${PATCHER_PROJECT}" -c Release

printf "Running patcher on CarScannerXamarinForms.dll...\n"
dotnet run --project "${PATCHER_PROJECT}" -c Release -- \
  "${CARSCANNER}" \
  "${OUTPUT_DIR}/CarScannerXamarinForms.patched.dll" \
  "${WS_DLL}"

# 复制代理 DLL 到输出目录
printf "\nCopying WebSocketIOBDConnectionProxy.dll to output...\n"
cp "${PROXY_DLL}" "${OUTPUT_DIR}/"

printf "Done. Output in %s\n" "${OUTPUT_DIR}"
printf "Files:\n"
ls -la "${OUTPUT_DIR}"/*.dll
