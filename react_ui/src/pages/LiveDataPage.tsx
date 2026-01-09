import React, { useEffect, useState, useCallback, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { NavBar, Cell, Loading, Empty, Button } from 'react-vant';
import { Arrow, ArrowLeft } from '@react-vant/icons';

// Unit code to string mapping based on UnitsHelper.cs
const UNITS_MAP: Record<number, string> = {
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

const getUnitString = (units: number | string | undefined): string => {
  if (typeof units === 'string') return units;
  if (typeof units === 'number') return UNITS_MAP[units] || '';
  return '';
};

type ECUTest = {
  Name: string;
  Available: boolean;
  Complete: boolean;
  Cycle?: number;
};

type MonitorStatusValue = {
  MIL_ON: boolean;
  DTCs: number;
  ECUTests: ECUTest[];
  VehicleType?: number;
};

type PIDItem = {
  NM: string;
  SNM: string;
  Value: number | string | MonitorStatusValue | any;
  Units?: number | string;
  IsAvailable: boolean;
  Id?: number;
  MIN?: number;
  MAX?: number;
  CMD?: string;
  originalIndex?: number;  // Original index in the full PID list
};

const ITEMS_PER_PAGE = 10;

const LiveDataPage: React.FC = () => {
  const navigate = useNavigate();
  const [pidList, setPidList] = useState<PIDItem[]>([]);
  const [currentPage, setCurrentPage] = useState(0);
  const [pidValues, setPidValues] = useState<Map<string, PIDItem>>(new Map());
  const [loading, setLoading] = useState(true);
  const [reading, setReading] = useState(false);
  const isReadingRef = useRef(false);
  const scrollContainerRef = useRef<HTMLDivElement>(null);

  const safeCall = useCallback((method: string, ...args: any[]) => {
    const bridge = (window as any).JSBridge;
    if (bridge && typeof bridge[method] === 'function') {
      try {
        return bridge[method](...args);
      } catch (e) {
        console.error(`JSBridge.${method} error:`, e);
      }
    } else {
      console.warn(`JSBridge.${method} not available.`);
    }
    return null;
  }, []);

  const totalPages = Math.ceil(pidList.length / ITEMS_PER_PAGE);

  const getCurrentPageIndices = useCallback((page: number, list: PIDItem[]) => {
    const start = page * ITEMS_PER_PAGE;
    const end = Math.min(start + ITEMS_PER_PAGE, list.length);
    const indices: number[] = [];
    for (let i = start; i < end; i++) {
      // Use originalIndex if available, otherwise use the array index
      const originalIdx = list[i].originalIndex ?? i;
      indices.push(originalIdx);
    }
    return indices;
  }, []);

  const startReading = useCallback((indices: number[]) => {
    if (indices.length === 0) return;
    console.log('Starting read PIDs:', indices);
    setReading(true);
    isReadingRef.current = true;
    safeCall('startReadPIDs', JSON.stringify(indices));
  }, [safeCall]);

  const stopReading = useCallback(() => {
    if (isReadingRef.current) {
      console.log('Stopping read PIDs');
      safeCall('stopReadPIDs');
      isReadingRef.current = false;
      setReading(false);
    }
  }, [safeCall]);

  useEffect(() => {
    const pidValueHandler = (data: any) => {
      let parsed = data;
      if (typeof data === 'string') {
        try {
          parsed = JSON.parse(data);
        } catch (e) {
          console.error('Failed to parse PID data:', e);
          return;
        }
      }

      // Use NM (name) as key to match PID
      if (parsed && parsed.NM) {
        setPidValues(prev => {
          const newMap = new Map(prev);
          newMap.set(parsed.NM, parsed);
          return newMap;
        });
      }
    };

    (window as any).onPIDValueChanged = pidValueHandler;

    const result = safeCall('getPIDList');
    let list: PIDItem[] = [];
    if (result) {
      if (typeof result === 'string') {
        try {
          list = JSON.parse(result);
        } catch (e) {
          console.error('Failed to parse PID list:', e);
        }
      } else if (Array.isArray(result)) {
        list = result;
      }
    }

    // Add original index and filter out "Not selected" PIDs
    list = list
      .map((item, index) => ({ ...item, originalIndex: index }))
      .filter(item => item.NM !== 'Not selected' && item.SNM !== 'Not selected');

    if (list.length > 0) {
      setPidList(list);
      setLoading(false);
      const indices = getCurrentPageIndices(0, list);
      startReading(indices);
    } else {
      setLoading(false);
    }

    return () => {
      stopReading();
      (window as any).onPIDValueChanged = null;
    };
  }, [safeCall, getCurrentPageIndices, startReading, stopReading]);

  const handlePageChange = (newPage: number) => {
    if (newPage < 0 || newPage >= totalPages || newPage === currentPage) return;

    stopReading();
    setCurrentPage(newPage);
    const indices = getCurrentPageIndices(newPage, pidList);
    startReading(indices);

    // Scroll to top
    if (scrollContainerRef.current) {
      scrollContainerRef.current.scrollTop = 0;
    }
  };

  const getCurrentPageItems = () => {
    const start = currentPage * ITEMS_PER_PAGE;
    const end = Math.min(start + ITEMS_PER_PAGE, pidList.length);
    return pidList.slice(start, end).map((item, idx) => ({
      ...item,
      globalIndex: start + idx
    }));
  };

  const formatValue = (item: PIDItem, liveData?: PIDItem): string => {
    const data = liveData || item;
    const value = data.Value;

    if (value === null || value === undefined) return '--';
    if (typeof value === 'number' && isNaN(value)) return '--';
    if (String(value) === 'NaN') return '--';

    if (typeof value === 'object') {
      return '';
    }

    // Round numbers with more than 4 decimal places
    let displayValue = value;
    if (typeof value === 'number') {
      const decimalPlaces = (value.toString().split('.')[1] || '').length;
      if (decimalPlaces > 4) {
        displayValue = Math.round(value * 10000) / 10000;
      }
    }

    const unitStr = getUnitString(data.Units);
    return `${displayValue}${unitStr ? ' ' + unitStr : ''}`;
  };

  const isMonitorStatus = (value: any): value is MonitorStatusValue => {
    return value && typeof value === 'object' && 'ECUTests' in value;
  };

  const renderMonitorStatus = (value: MonitorStatusValue) => {
    const lines: string[] = [];
    lines.push(`MIL:${value.MIL_ON ? 'ON' : 'OFF'}`);
    lines.push(`DTC count:${value.DTCs}`);

    if (value.ECUTests && Array.isArray(value.ECUTests)) {
      value.ECUTests.forEach(test => {
        const availableStr = test.Available ? 'Available' : 'Not available';
        const completeStr = test.Complete ? 'Completed' : 'Not completed';
        lines.push(`${test.Name}: ${availableStr}/${completeStr}`);
      });
    }

    return (
      <div style={{ textAlign: 'right', fontSize: 12, lineHeight: 1.6 }}>
        {lines.map((line, idx) => (
          <div key={idx}>{line}</div>
        ))}
      </div>
    );
  };

  const renderPIDItem = (item: PIDItem & { globalIndex: number }) => {
    // Use NM to get live data
    const liveData = pidValues.get(item.NM);
    const displayData = liveData || item;
    const value = displayData.Value;
    const isAvailable = displayData.IsAvailable !== false;

    if (isMonitorStatus(value)) {
      return (
        <Cell
          key={item.globalIndex}
          title={<span style={{ color: isAvailable ? '#323233' : '#969799' }}>{item.NM}</span>}
          label={renderMonitorStatus(value)}
          style={{ opacity: isAvailable ? 1 : 0.6 }}
        />
      );
    }

    const formattedValue = formatValue(item, liveData);

    return (
      <Cell
        key={item.globalIndex}
        title={<span style={{ color: isAvailable ? '#323233' : '#969799' }}>{item.NM}</span>}
        value={
          <span style={{ color: isAvailable ? '#323233' : '#969799' }}>
            {formattedValue || '--'}
          </span>
        }
        style={{ opacity: isAvailable ? 1 : 0.6 }}
      />
    );
  };

  const renderContent = () => {
    if (loading) {
      return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', paddingTop: 80 }}>
          <Loading type="spinner" vertical>
            正在加载数据项...
          </Loading>
        </div>
      );
    }

    if (pidList.length === 0) {
      return <Empty description="暂无数据" />;
    }

    const items = getCurrentPageItems();

    return (
      <Cell.Group>
        {items.map(item => renderPIDItem(item))}
      </Cell.Group>
    );
  };

  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column', background: '#f7f8fa' }}>
      <NavBar
        title="实时数据"
        leftText="返回"
        onClickLeft={() => navigate(-1)}
      />

      <div ref={scrollContainerRef} style={{ flex: 1, overflow: 'auto' }}>
        {renderContent()}
      </div>

      {pidList.length > 0 && (
        <div style={{
          padding: 16,
          background: '#fff',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          borderTop: '1px solid #ebedf0'
        }}>
          <Button
            size="small"
            disabled={currentPage === 0}
            onClick={() => handlePageChange(currentPage - 1)}
            icon={<ArrowLeft />}
          >
            上一页
          </Button>
          <span style={{ fontSize: 14, color: '#646566' }}>
            第 {currentPage + 1} / {totalPages} 页
            {reading && <span style={{ marginLeft: 8, color: '#07c160' }}>读取中...</span>}
          </span>
          <Button
            size="small"
            disabled={currentPage >= totalPages - 1}
            onClick={() => handlePageChange(currentPage + 1)}
            iconPosition="right"
            icon={<Arrow />}
          >
            下一页
          </Button>
        </div>
      )}
    </div>
  );
};

export default LiveDataPage;
