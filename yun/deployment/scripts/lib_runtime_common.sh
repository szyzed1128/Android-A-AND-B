#!/usr/bin/env bash
# =============================================================================
# lib_runtime_common.sh  —  Android/Wayland 运行时公共辅助函数
# =============================================================================
# 目标：
#   - 统一 slot_1 与 slot_2+ 的 weston / wayland socket 管理
#   - 统一 Android readiness 判定（boot_completed + framework services）
#   - 避免 create/start/bootstrap 各自复制一份脆弱时序逻辑
# =============================================================================

_RUNTIME_USER_DIR="/run/user/0"
_RUNTIME_PULSE_DIR="${_RUNTIME_USER_DIR}/pulse"

wayland_socket_path() {
  local display="${1:?display 不能为空}"
  echo "${_RUNTIME_USER_DIR}/${display}"
}

weston_log_path() {
  local display="${1:?display 不能为空}"
  echo "/tmp/weston-${display}.log"
}

prepare_runtime_dirs() {
  mkdir -p "${_RUNTIME_USER_DIR}" "${_RUNTIME_PULSE_DIR}"
  touch "${_RUNTIME_PULSE_DIR}/native"
}

wayland_socket_ready() {
  local display="${1:?display 不能为空}"
  grep -q "${display}$" /proc/net/unix 2>/dev/null
}

wait_wayland_socket() {
  local display="${1:?display 不能为空}"
  local timeout="${2:-15}"
  for _ in $(seq 1 "${timeout}"); do
    wayland_socket_ready "${display}" && return 0
    sleep 1
  done
  return 1
}

cleanup_stale_wayland_socket() {
  local display="${1:?display 不能为空}"
  if wayland_socket_ready "${display}"; then
    return 0
  fi
  rm -f "$(wayland_socket_path "${display}")" "$(wayland_socket_path "${display}").lock" 2>/dev/null || true
}

start_ephemeral_weston() {
  local display="${1:?display 不能为空}"
  prepare_runtime_dirs
  cleanup_stale_wayland_socket "${display}"
  XDG_RUNTIME_DIR="${_RUNTIME_USER_DIR}" nohup weston \
    --backend=headless-backend.so \
    --no-config \
    --socket="${display}" \
    > "$(weston_log_path "${display}")" 2>&1 &
}

slot_weston_service_name() {
  local slot="${1:?slot 不能为空}"
  if [[ "${slot}" -eq 1 ]]; then
    echo "weston.service"
  else
    echo "obd-weston@${slot}.service"
  fi
}

ensure_slot_wayland_socket() {
  local slot="${1:?slot 不能为空}"
  local display="${2:?display 不能为空}"
  local timeout="${3:-15}"
  local service="${4:-$(slot_weston_service_name "${slot}")}"

  prepare_runtime_dirs
  if wayland_socket_ready "${display}"; then
    return 0
  fi

  cleanup_stale_wayland_socket "${display}"
  if [[ -n "${service}" ]] && systemctl cat "${service}" >/dev/null 2>&1; then
    log_info "wayland socket ${display} 缺失，重启 ${service}"
    systemctl restart "${service}" >/dev/null 2>&1 || return 1
  else
    log_info "wayland socket ${display} 缺失，直接拉起 weston"
    start_ephemeral_weston "${display}"
  fi

  wait_wayland_socket "${display}" "${timeout}"
}

stop_slot_weston() {
  local slot="${1:?slot 不能为空}"
  local display="${2:?display 不能为空}"
  local service="${3:-$(slot_weston_service_name "${slot}")}"

  if [[ -n "${service}" ]] && systemctl cat "${service}" >/dev/null 2>&1; then
    systemctl stop "${service}" >/dev/null 2>&1 || true
  fi
  pkill -f "weston.*${display}" 2>/dev/null || true
  cleanup_stale_wayland_socket "${display}"
}

android_framework_ready() {
  local adb_target="${1:?adb_target 不能为空}"
  local boot
  local package_service
  local activity_service

  boot="$(adb -s "${adb_target}" shell getprop sys.boot_completed 2>/dev/null | tr -d '\r\n')"
  [[ "${boot}" == "1" ]] || return 1

  package_service="$(adb -s "${adb_target}" shell service check package 2>/dev/null | tr -d '\r')"
  activity_service="$(adb -s "${adb_target}" shell service check activity 2>/dev/null | tr -d '\r')"
  [[ "${package_service}" == *"found"* ]] || return 1
  [[ "${activity_service}" == *"found"* ]] || return 1
  return 0
}

wait_for_android_framework_ready() {
  local adb_target="${1:?adb_target 不能为空}"
  local timeout="${2:-180}"
  local interval="${3:-3}"
  local rounds="$(( (timeout + interval - 1) / interval ))"

  for _ in $(seq 1 "${rounds}"); do
    android_framework_ready "${adb_target}" && return 0
    sleep "${interval}"
  done
  return 1
}
