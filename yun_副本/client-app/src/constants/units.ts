/**
 * 单位码映射
 * 基于 UnitsHelper.cs 的单位定义
 */

export const UNITS_MAP: Record<number, string> = {
  0: '',           // None
  1: 'km/h',       // kmh
  2: 'mph',        // mph
  3: 'km',         // km
  4: 'miles',      // miles
  5: 'kPa',        // kPa
  6: 'rpm',        // rpm
  7: '°',          // grads
  8: 'g/sec',      // grams_sec
  9: 'kg/min',     // kg_min
  10: 'V',         // volts
  11: 'Pa',        // Pa
  12: 'L/h',       // Lh
  13: 'Nm',        // Nm
  14: '%',         // percent
  15: '℃',         // celicium
  16: '℉',         // fahrengheit
  17: 'sec.',      // seconds
  18: 'min',       // minutes
  19: 'mA',        // mA
  20: 'm',         // meters
  21: 'ft.',       // feet
  22: 'L/100km',   // liters100km
  23: 'MPG',       // MPG
  24: 'L',         // liters
  25: 'gal.',      // gallons
  26: 'Ohm',       // Ohm
  27: 'kOhm',      // kOhm
  28: 'MOhm',      // MOhm
  29: 'MHz',       // MHz
  30: 'Hz',        // Hz
  31: 'V/msec.',   // Vms
  32: 'Pa/sec.',   // Pa_sec
  33: 'kg/h',      // kg_h
  34: 'g/cyl',     // g_cyl
  35: 'g/stroke',  // g_stroke
  36: 'mm',        // mm
  37: 'lbs',       // lbs
  38: 'g.',        // gramms
  39: 'mV/sec.',   // mV_sec
  40: 'gpm',       // gpm
  41: 'ppm',       // ppm
  42: 'g',         // g
  43: 'm/s²',      // m_sec2
  44: 'ms',        // ms
  45: 'hp',        // hp
  46: 'kW',        // kW
  47: 'psi',       // psi
  48: 'bar',       // bar
  49: 'mV',        // mV
  50: 'km/L',      // km_liter
  51: 'μs',        // microseconds
  52: 'mbar',      // mbar
  53: 'W',         // W
  54: 'h',         // h
  55: 'm³/h',      // m3_hour
  56: 'mg/c',      // mgpc
  57: 'days',      // days
  58: '$',         // money
  59: 'A',         // A
  60: 'kWh',       // kWh
  61: 'Wh',        // Wh
  62: 'Ah',        // Ah
  63: 'Hours',     // Hours
  64: 'μs',        // μs
  65: 'g/L',       // g_L
  66: 'kPsi',      // kPsi
  67: 'mm³',       // mm3
  68: 'mg/str.',   // mg_stroke
  69: 'hPa',       // hPa
  70: 'revs',      // revs
  71: '°/s',       // grads_sec
  72: 'mm³/str.',  // mm3_stroke
  73: 'kHz',       // kHz
  74: 'MPa',       // MPa
  75: 'cm',        // cm
  76: 'mg/rev',    // mg_rev
  77: 'mg',        // mg
  78: 'mg/m³',     // mg_m3
  79: 'mOhm',      // mOhm
  80: 'mg/cyl',    // mg_cyl
  81: '°CS',       // grads_CS
  82: 'months',    // months
  83: 'lbf⋅ft',    // lbf_ft
  84: 'm/sec.',    // m_s
};

/**
 * 获取单位字符串
 */
export const getUnitString = (units: number | string | undefined): string => {
  if (typeof units === 'string') return units;
  if (typeof units === 'number') return UNITS_MAP[units] || '';
  return '';
};

/**
 * 格式化数值显示
 */
export const formatValue = (value: any, units?: number | string): string => {
  // 处理空值
  if (value === null || value === undefined) return '--';

  // 处理 NaN
  if (typeof value === 'number' && isNaN(value)) return '--';
  if (value === 'NaN') return '--';

  // 处理对象类型（如 MonitorStatusValue）
  if (typeof value === 'object') return '';

  // 处理数字
  if (typeof value === 'number') {
    // 小数位数过多时截断
    const str = value.toString();
    const decimalIndex = str.indexOf('.');
    if (decimalIndex >= 0 && str.length - decimalIndex > 5) {
      value = value.toFixed(4);
    }
  }

  // 拼接单位
  const unitStr = getUnitString(units);
  if (unitStr) {
    return `${value} ${unitStr}`;
  }
  return String(value);
};

/**
 * 连接状态颜色映射
 */
export const CONNECTION_STATUS_COLORS = {
  Disconnected: '#969799',
  ConnectingToELM: '#1989fa',
  ConnectedToELM: '#1989fa',
  ConnectingToECU: '#1989fa',
  ConnectedToECU: '#07c160',
} as const;

/**
 * 连接状态显示文本
 */
export const CONNECTION_STATUS_TEXT = {
  Disconnected: '未连接',
  ConnectingToELM: '连接ELM中...',
  ConnectedToELM: 'ELM已连接',
  ConnectingToECU: '连接ECU中...',
  ConnectedToECU: '已连接',
} as const;
