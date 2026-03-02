#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
YUN_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"
ROOT_DIR="$(cd "${YUN_DIR}/.." && pwd)"

WS_PROJECT="${YUN_DIR}/websocket_layer/OBDCloud.WebSocket.csproj"
INJECTOR_PROJECT="${YUN_DIR}/tools/DllInjector/DllInjector.csproj"
OUTPUT_DIR="${YUN_DIR}/modified_dlls"
ASSEMBLIES_DIR="${ROOT_DIR}/temp_verify/unknown/assemblies"

CARDEMO_ANDROID="${ASSEMBLIES_DIR}/CarDemo.Android.dll"
CARDEMO="${ASSEMBLIES_DIR}/CarDemo.dll"
WS_DLL="${YUN_DIR}/websocket_layer/bin/Release/netstandard2.0/OBDCloud.WebSocket.dll"

# 代理 DLL 的路径（使用 Mono 编译器编译）
PROXY_DLL="${YUN_DIR}/websocket_layer/bin/WebSocketIOBDConnectionProxy.dll"

printf "Building WebSocket layer...\n"
dotnet build "${WS_PROJECT}" -c Release

# 编译 WebSocketIOBDConnectionProxy.dll (需要 Mono 编译器)
printf "\nBuilding WebSocketIOBDConnectionProxy with Mono mcs...\n"
mcs -target:library -nostdlib \
    -r:"${ASSEMBLIES_DIR}/mscorlib.dll" \
    -r:"${ASSEMBLIES_DIR}/CarScannerXamarinForms.dll" \
    -r:"${ASSEMBLIES_DIR}/System.dll" \
    -out:"${PROXY_DLL}" \
    "${YUN_DIR}/websocket_layer/src/WebSocketIOBDConnectionProxy.cs"

printf "Building injector...\n"
dotnet build "${INJECTOR_PROJECT}" -c Release

printf "Running injector...\n"
dotnet run --project "${INJECTOR_PROJECT}" -c Release -- \
  "${CARDEMO_ANDROID}" \
  "${CARDEMO}" \
  "${WS_DLL}" \
  "${OUTPUT_DIR}"

# 复制代理 DLL 到输出目录
printf "\nCopying WebSocketIOBDConnectionProxy.dll to output...\n"
cp "${PROXY_DLL}" "${OUTPUT_DIR}/"

printf "Done. Output in %s\n" "${OUTPUT_DIR}"
printf "Files:\n"
ls -la "${OUTPUT_DIR}"/*.dll
