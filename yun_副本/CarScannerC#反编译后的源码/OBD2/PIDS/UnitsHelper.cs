using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS.UnitConverters;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000426 RID: 1062
	public static class UnitsHelper
	{
		// Token: 0x06002D51 RID: 11601 RVA: 0x001FEE7C File Offset: 0x001FD07C
		public static string GetCaptionInvariant(UnitsHelper.Units unit)
		{
			switch (unit)
			{
			case UnitsHelper.Units.None:
				return "";
			case UnitsHelper.Units.kmh:
				return "km/h";
			case UnitsHelper.Units.mph:
				return "mph";
			case UnitsHelper.Units.km:
			case UnitsHelper.Units.miles:
			case UnitsHelper.Units.kPa:
			case UnitsHelper.Units.rpm:
			case UnitsHelper.Units.Pa:
			case UnitsHelper.Units.Nm:
			case UnitsHelper.Units.mA:
			case UnitsHelper.Units.meters:
			case UnitsHelper.Units.Ohm:
			case UnitsHelper.Units.mm:
			case UnitsHelper.Units.ppm:
			case UnitsHelper.Units.ms:
			case UnitsHelper.Units.psi:
			case UnitsHelper.Units.bar:
			case UnitsHelper.Units.mV:
			case UnitsHelper.Units.mbar:
			case UnitsHelper.Units.W:
			case UnitsHelper.Units.h:
			case UnitsHelper.Units.days:
			case UnitsHelper.Units.A:
			case UnitsHelper.Units.kWh:
			case UnitsHelper.Units.Wh:
			case UnitsHelper.Units.Ah:
			case UnitsHelper.Units.Hours:
			case UnitsHelper.Units.μs:
			case UnitsHelper.Units.g_L:
			case UnitsHelper.Units.kPsi:
			case UnitsHelper.Units.mm3:
			case UnitsHelper.Units.mg_stroke:
			case UnitsHelper.Units.hPa:
			case UnitsHelper.Units.revs:
			case UnitsHelper.Units.kHz:
			case UnitsHelper.Units.MPa:
			case UnitsHelper.Units.cm:
			case UnitsHelper.Units.mg_rev:
			case UnitsHelper.Units.mg:
			case UnitsHelper.Units.mOhm:
			case UnitsHelper.Units.mg_cyl:
			case UnitsHelper.Units.grads_CS:
			case UnitsHelper.Units.months:
			case UnitsHelper.Units.lbf_ft:
				break;
			case UnitsHelper.Units.grads:
				return "°";
			case UnitsHelper.Units.grams_sec:
				return "g/sec";
			case UnitsHelper.Units.kg_min:
				return "kg/min";
			case UnitsHelper.Units.volts:
				return "V";
			case UnitsHelper.Units.Lh:
				return "L/h";
			case UnitsHelper.Units.percent:
				return "%";
			case UnitsHelper.Units.celicium:
				return "℃";
			case UnitsHelper.Units.fahrengheit:
				return "℉";
			case UnitsHelper.Units.seconds:
				return "sec.";
			case UnitsHelper.Units.minutes:
				return "min";
			case UnitsHelper.Units.feet:
				return "ft.";
			case UnitsHelper.Units.liters100km:
				return "L/100km";
			case UnitsHelper.Units.MPG:
				return "MPG";
			case UnitsHelper.Units.liters:
				return "L";
			case UnitsHelper.Units.gallons:
				return "gal.";
			case UnitsHelper.Units.kOhm:
				return "kOhm";
			case UnitsHelper.Units.MOhm:
				return "MOhm";
			case UnitsHelper.Units.MHz:
				return "MHz";
			case UnitsHelper.Units.Hz:
				return "Hz";
			case UnitsHelper.Units.Vms:
				return "V/msec.";
			case UnitsHelper.Units.Pa_sec:
				return "Pa/sec.";
			case UnitsHelper.Units.kg_h:
				return "kg/h";
			case UnitsHelper.Units.g_cyl:
				return "g/cylinder";
			case UnitsHelper.Units.g_stroke:
				return "g/stroke";
			case UnitsHelper.Units.lbs:
				return "lbs";
			case UnitsHelper.Units.gramms:
				return "g.";
			case UnitsHelper.Units.mV_sec:
				return "mV/sec.";
			case UnitsHelper.Units.gpm:
				return "gpm";
			case UnitsHelper.Units.g:
				return "g";
			case UnitsHelper.Units.m_sec2:
				return "m/s²";
			case UnitsHelper.Units.hp:
				return "hp";
			case UnitsHelper.Units.kW:
				return "kW";
			case UnitsHelper.Units.km_liter:
				return "km/L";
			case UnitsHelper.Units.microseconds:
				return "μs";
			case UnitsHelper.Units.m3_hour:
				return "m^3/h.";
			case UnitsHelper.Units.mgpc:
				return "mg/c";
			case UnitsHelper.Units.money:
				return SharedSettings.Current.Currency;
			case UnitsHelper.Units.grads_sec:
				return "°/s";
			case UnitsHelper.Units.mm3_stroke:
				return "mm³/str.";
			case UnitsHelper.Units.mg_m3:
				return "mg/m³";
			case UnitsHelper.Units.m_s:
				return "m/sec.";
			default:
				switch (unit)
				{
				case UnitsHelper.Units.km_kwh:
					return "km/kWh";
				case UnitsHelper.Units.kWh_100km:
					return "kWh/100km";
				case UnitsHelper.Units.miles_per_kwh:
					return "mi/kWh";
				case UnitsHelper.Units.kwH_100miles:
					return "kWh/100mi";
				case UnitsHelper.Units.mg_hour:
					return "mg/hour";
				case UnitsHelper.Units.per_second:
					return "/sec.";
				case UnitsHelper.Units.l_per_mm:
					return "l/mm";
				case UnitsHelper.Units.per_minute:
					return "/min.";
				case UnitsHelper.Units.L_sec:
					return "L/sec.";
				case UnitsHelper.Units.L_min:
					return "L/min.";
				}
				break;
			}
			return unit.ToString();
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x001FF180 File Offset: 0x001FD380
		public static string GetCaption(UnitsHelper.Units unit)
		{
			switch (unit)
			{
			case UnitsHelper.Units.None:
				return "";
			case UnitsHelper.Units.kmh:
			case UnitsHelper.Units.mph:
				if (SharedSettings.Current.Use_km)
				{
					return "km/h";
				}
				return "mph";
			case UnitsHelper.Units.km:
				if (SharedSettings.Current.Use_km)
				{
					return "km";
				}
				return "miles";
			case UnitsHelper.Units.miles:
			case UnitsHelper.Units.rpm:
			case UnitsHelper.Units.Pa:
			case UnitsHelper.Units.mA:
			case UnitsHelper.Units.MPG:
			case UnitsHelper.Units.Ohm:
			case UnitsHelper.Units.mm:
			case UnitsHelper.Units.gpm:
			case UnitsHelper.Units.ppm:
			case UnitsHelper.Units.ms:
			case UnitsHelper.Units.bar:
			case UnitsHelper.Units.mV:
			case UnitsHelper.Units.mbar:
			case UnitsHelper.Units.W:
			case UnitsHelper.Units.h:
			case UnitsHelper.Units.m3_hour:
			case UnitsHelper.Units.mgpc:
			case UnitsHelper.Units.days:
			case UnitsHelper.Units.A:
			case UnitsHelper.Units.kWh:
			case UnitsHelper.Units.Wh:
			case UnitsHelper.Units.Ah:
			case UnitsHelper.Units.Hours:
			case UnitsHelper.Units.μs:
			case UnitsHelper.Units.g_L:
			case UnitsHelper.Units.kPsi:
			case UnitsHelper.Units.hPa:
			case UnitsHelper.Units.revs:
			case UnitsHelper.Units.kHz:
			case UnitsHelper.Units.MPa:
			case UnitsHelper.Units.cm:
			case UnitsHelper.Units.mg_rev:
			case UnitsHelper.Units.mg:
			case UnitsHelper.Units.mOhm:
			case UnitsHelper.Units.mg_cyl:
			case UnitsHelper.Units.grads_CS:
			case UnitsHelper.Units.months:
			case UnitsHelper.Units.Kelvin:
			case UnitsHelper.Units.inch:
			case UnitsHelper.Units.atm:
			case UnitsHelper.Units.gph:
			case UnitsHelper.Units.grams_min:
			case UnitsHelper.Units.lb_min:
			case UnitsHelper.Units.lb_sec:
			case UnitsHelper.Units.lb_h:
			case UnitsHelper.Units.InHg:
			case UnitsHelper.Units.mmHg:
				break;
			case UnitsHelper.Units.kPa:
				if (SharedSettings.Current.Pressure_use_kpa)
				{
					return "kPa";
				}
				return "psi";
			case UnitsHelper.Units.grads:
				return "°";
			case UnitsHelper.Units.grams_sec:
				if (SharedSettings.Current.Flow_use_grams_sec)
				{
					return "g/sec";
				}
				return "kg/hour";
			case UnitsHelper.Units.kg_min:
				if (SharedSettings.Current.Flow_use_grams_sec)
				{
					return "g/sec";
				}
				return "kg/hour";
			case UnitsHelper.Units.volts:
				return "V";
			case UnitsHelper.Units.Lh:
				if (SharedSettings.Current.UseLitersForVolume)
				{
					return "L/h";
				}
				return "gal./h";
			case UnitsHelper.Units.Nm:
			case UnitsHelper.Units.lbf_ft:
				if (SharedSettings.Current.UseNmForTorque)
				{
					return "N⋅m";
				}
				return "lbf⋅ft";
			case UnitsHelper.Units.percent:
				return "%";
			case UnitsHelper.Units.celicium:
				if (SharedSettings.Current.Use_celcium)
				{
					return "℃";
				}
				return "℉";
			case UnitsHelper.Units.fahrengheit:
				if (SharedSettings.Current.Use_celcium)
				{
					return "℃";
				}
				return "℉";
			case UnitsHelper.Units.seconds:
				return "sec.";
			case UnitsHelper.Units.minutes:
				return "min";
			case UnitsHelper.Units.meters:
				if (SharedSettings.Current.Use_km)
				{
					return "m";
				}
				return "feet";
			case UnitsHelper.Units.feet:
				return "ft.";
			case UnitsHelper.Units.liters100km:
				switch (SharedSettings.Current.FuelConsumptionUnit)
				{
				case FuelConsumptionUnits.KmPerLiter:
					return "km/L";
				case FuelConsumptionUnits.MilesPerGallon:
					return "MPG";
				}
				return "L/100km";
			case UnitsHelper.Units.liters:
				if (SharedSettings.Current.UseLitersForVolume)
				{
					return "L";
				}
				return "gallon";
			case UnitsHelper.Units.gallons:
				return "gal.";
			case UnitsHelper.Units.kOhm:
				return "kOhm";
			case UnitsHelper.Units.MOhm:
				return "MOhm";
			case UnitsHelper.Units.MHz:
				return "MHz";
			case UnitsHelper.Units.Hz:
				return "Hz";
			case UnitsHelper.Units.Vms:
				return "V/msec.";
			case UnitsHelper.Units.Pa_sec:
				return "Pa/sec.";
			case UnitsHelper.Units.kg_h:
				if (SharedSettings.Current.Flow_use_grams_sec)
				{
					return "g/sec";
				}
				return "kg/hour";
			case UnitsHelper.Units.g_cyl:
				return "g/cylinder";
			case UnitsHelper.Units.g_stroke:
				return "g/stroke";
			case UnitsHelper.Units.lbs:
				return "lbs";
			case UnitsHelper.Units.gramms:
				return "g.";
			case UnitsHelper.Units.mV_sec:
				return "mV/sec.";
			case UnitsHelper.Units.g:
			case UnitsHelper.Units.m_sec2:
				if (SharedSettings.Current.AccelerationUseG)
				{
					return "g";
				}
				return "m/s²";
			case UnitsHelper.Units.hp:
				if (SharedSettings.Current.UseHoursePower)
				{
					return "hp";
				}
				return "kW";
			case UnitsHelper.Units.kW:
				if (SharedSettings.Current.UseHoursePower)
				{
					return "hp";
				}
				return "kW";
			case UnitsHelper.Units.psi:
				if (SharedSettings.Current.Pressure_use_kpa)
				{
					return "kPa";
				}
				return "psi";
			case UnitsHelper.Units.km_liter:
				return "km/L";
			case UnitsHelper.Units.microseconds:
				return "μs";
			case UnitsHelper.Units.money:
				return SharedSettings.Current.Currency;
			case UnitsHelper.Units.mm3:
				return "mm^3";
			case UnitsHelper.Units.mg_stroke:
				return "mg/str.";
			case UnitsHelper.Units.grads_sec:
				return "°/s";
			case UnitsHelper.Units.mm3_stroke:
				return "mm^3/str.";
			case UnitsHelper.Units.mg_m3:
				return "mg/m^3";
			case UnitsHelper.Units.m_s:
				return "m/sec.";
			case UnitsHelper.Units.km_kwh:
				return "km/kWh";
			case UnitsHelper.Units.kWh_100km:
				if (SharedSettings.Current.Use_km)
				{
					return "kWh/100km";
				}
				return "mi/kWh";
			case UnitsHelper.Units.miles_per_kwh:
				if (SharedSettings.Current.Use_km)
				{
					return "kWh/100km";
				}
				return "mi/kWh";
			default:
				if (unit == UnitsHelper.Units.L_min)
				{
					return "L/min.";
				}
				break;
			}
			return unit.ToString();
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x001FF5D0 File Offset: 0x001FD7D0
		public static double GetValue(double original_value, UnitsHelper.Units unit)
		{
			if (unit <= UnitsHelper.Units.psi)
			{
				switch (unit)
				{
				case UnitsHelper.Units.kmh:
					if (SharedSettings.Current.Use_km)
					{
						return original_value;
					}
					return original_value / 1.609344;
				case UnitsHelper.Units.mph:
					if (!SharedSettings.Current.Use_km)
					{
						return original_value;
					}
					return original_value * 1.609344;
				case UnitsHelper.Units.km:
					if (SharedSettings.Current.Use_km)
					{
						return original_value;
					}
					return original_value / 1.609344;
				case UnitsHelper.Units.miles:
				case UnitsHelper.Units.rpm:
				case UnitsHelper.Units.grads:
				case UnitsHelper.Units.volts:
				case UnitsHelper.Units.Pa:
				case UnitsHelper.Units.percent:
				case UnitsHelper.Units.seconds:
				case UnitsHelper.Units.minutes:
				case UnitsHelper.Units.mA:
				case UnitsHelper.Units.feet:
				case UnitsHelper.Units.MPG:
					break;
				case UnitsHelper.Units.kPa:
					if (SharedSettings.Current.Pressure_use_kpa)
					{
						return original_value;
					}
					return original_value * 0.145037738007;
				case UnitsHelper.Units.grams_sec:
					if (SharedSettings.Current.Flow_use_grams_sec)
					{
						return original_value;
					}
					return original_value * 3.6;
				case UnitsHelper.Units.kg_min:
					if (SharedSettings.Current.Flow_use_grams_sec)
					{
						return original_value * 16.6666667;
					}
					return original_value * 60.0;
				case UnitsHelper.Units.Lh:
					if (SharedSettings.Current.UseLitersForVolume)
					{
						return original_value;
					}
					if (SharedSettings.Current.UseUSGallon)
					{
						return original_value * 0.26417205124156;
					}
					return original_value * 0.21996924829909;
				case UnitsHelper.Units.Nm:
					if (SharedSettings.Current.UseNmForTorque)
					{
						return original_value;
					}
					return original_value * 0.73756214927727;
				case UnitsHelper.Units.celicium:
					if (SharedSettings.Current.Use_celcium)
					{
						return original_value;
					}
					return original_value * 1.8 + 32.0;
				case UnitsHelper.Units.fahrengheit:
					if (SharedSettings.Current.Use_celcium)
					{
						return (original_value - 32.0) / 1.8;
					}
					return original_value;
				case UnitsHelper.Units.meters:
					if (SharedSettings.Current.Use_km)
					{
						return original_value;
					}
					return original_value * 3.28084;
				case UnitsHelper.Units.liters100km:
					if (!double.IsPositiveInfinity(original_value))
					{
						switch (SharedSettings.Current.FuelConsumptionUnit)
						{
						case FuelConsumptionUnits.KmPerLiter:
							return 100.0 / original_value;
						case FuelConsumptionUnits.MilesPerGallon:
							if (!SharedSettings.Current.UseUSGallon)
							{
								return 282.481 / original_value;
							}
							return 235.215 / original_value;
						}
						return original_value;
					}
					if (SharedSettings.Current.FuelConsumptionUnit == FuelConsumptionUnits.LitersPer100km)
					{
						return original_value;
					}
					return 0.0;
				case UnitsHelper.Units.liters:
					if (SharedSettings.Current.UseLitersForVolume)
					{
						return original_value;
					}
					if (SharedSettings.Current.UseUSGallon)
					{
						return original_value * 0.264172;
					}
					return original_value * 0.219969;
				default:
					if (unit != UnitsHelper.Units.kg_h)
					{
						switch (unit)
						{
						case UnitsHelper.Units.g:
							if (SharedSettings.Current.AccelerationUseG)
							{
								return original_value;
							}
							return original_value * 9.80665;
						case UnitsHelper.Units.m_sec2:
							if (!SharedSettings.Current.AccelerationUseG)
							{
								return original_value;
							}
							return original_value / 9.80665;
						case UnitsHelper.Units.hp:
							if (SharedSettings.Current.UseHoursePower)
							{
								return original_value;
							}
							return original_value * 0.73549875;
						case UnitsHelper.Units.kW:
							if (SharedSettings.Current.UseHoursePower)
							{
								return original_value / 0.73549875;
							}
							return original_value;
						case UnitsHelper.Units.psi:
							if (SharedSettings.Current.Pressure_use_kpa)
							{
								return original_value * 6.89476;
							}
							return original_value;
						}
					}
					else
					{
						if (SharedSettings.Current.Flow_use_grams_sec)
						{
							return original_value * 0.277778;
						}
						return original_value;
					}
					break;
				}
			}
			else if (unit != UnitsHelper.Units.lbf_ft)
			{
				if (unit != UnitsHelper.Units.kWh_100km)
				{
					if (unit == UnitsHelper.Units.miles_per_kwh)
					{
						if (SharedSettings.Current.Use_km)
						{
							return UnitsHelper.Converters.FirstOrDefault((IUnitConverter x) => x is EnergyConsumptionConverter).Convert(original_value, UnitsHelper.Units.miles_per_kwh, UnitsHelper.Units.kWh_100km);
						}
						return original_value;
					}
				}
				else
				{
					if (SharedSettings.Current.Use_km)
					{
						return original_value;
					}
					return UnitsHelper.Converters.FirstOrDefault((IUnitConverter x) => x is EnergyConsumptionConverter).Convert(original_value, UnitsHelper.Units.kWh_100km, UnitsHelper.Units.miles_per_kwh);
				}
			}
			else
			{
				if (SharedSettings.Current.UseNmForTorque)
				{
					return original_value * 1.35582;
				}
				return original_value;
			}
			return original_value;
		}

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06002D54 RID: 11604 RVA: 0x001FF9CC File Offset: 0x001FDBCC
		public static List<string> UnitsCaptionCollection
		{
			get
			{
				if (UnitsHelper._UnitsCaptionCollection == null)
				{
					UnitsHelper._UnitsCaptionCollection = (from Enum x in Enum.GetValues(typeof(UnitsHelper.Units))
						select UnitsHelper.GetCaptionInvariant((UnitsHelper.Units)x)).ToList<string>();
					UnitsHelper._UnitsCaptionCollection[0] = "None";
				}
				return UnitsHelper._UnitsCaptionCollection;
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06002D55 RID: 11605 RVA: 0x001FFA38 File Offset: 0x001FDC38
		public static List<string> UnitsNamesCollection
		{
			get
			{
				if (UnitsHelper._UnitsNamesCollection == null)
				{
					UnitsHelper._UnitsNamesCollection = (from Enum x in Enum.GetValues(typeof(UnitsHelper.Units))
						select x.ToString()).ToList<string>();
				}
				return UnitsHelper._UnitsNamesCollection;
			}
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x001FFA94 File Offset: 0x001FDC94
		public static UnitsHelper.Units TryGetUnitsFromText(string s)
		{
			int num = UnitsHelper.UnitsCaptionCollection.FindIndex((string x) => x.ToLowerInvariant() == s.ToLowerInvariant());
			if (num < 0)
			{
				num = UnitsHelper.UnitsNamesCollection.FindIndex((string x) => x.ToLowerInvariant() == s.ToLowerInvariant());
			}
			if (num >= 0)
			{
				return (UnitsHelper.Units)num;
			}
			return UnitsHelper.Units.None;
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x001FFAE8 File Offset: 0x001FDCE8
		public static UnitsHelper.Units[] GetPossibleConversionsForUnit(UnitsHelper.Units unit)
		{
			IUnitConverter converterForUnit = UnitsHelper.GetConverterForUnit(unit);
			if (converterForUnit == null)
			{
				return new UnitsHelper.Units[] { unit };
			}
			return converterForUnit.GetUnits;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x001FFB10 File Offset: 0x001FDD10
		internal static IUnitConverter GetConverterForUnit(UnitsHelper.Units unit)
		{
			foreach (IUnitConverter unitConverter in UnitsHelper.Converters)
			{
				if (unitConverter.CanConvert(unit))
				{
					return unitConverter;
				}
			}
			return null;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x001FFB44 File Offset: 0x001FDD44
		public static double Convert(double value, UnitsHelper.Units units_from, UnitsHelper.Units units_to)
		{
			IUnitConverter unitConverter = null;
			foreach (IUnitConverter unitConverter2 in UnitsHelper.Converters)
			{
				if (unitConverter2.CanConvert(units_from) && unitConverter2.CanConvert(units_to))
				{
					unitConverter = unitConverter2;
					break;
				}
			}
			if (unitConverter == null)
			{
				return value;
			}
			return unitConverter.Convert(value, units_from, units_to);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x001FFB90 File Offset: 0x001FDD90
		// Note: this type is marked as 'beforefieldinit'.
		static UnitsHelper()
		{
		}

		// Token: 0x0400193E RID: 6462
		private static List<string> _UnitsCaptionCollection = null;

		// Token: 0x0400193F RID: 6463
		private static List<string> _UnitsNamesCollection = null;

		// Token: 0x04001940 RID: 6464
		internal static IUnitConverter[] Converters = new IUnitConverter[]
		{
			new DistanceConverter(),
			new FuelConsumptionConverter(),
			new MassFlowConverter(),
			new PowerConverter(),
			new PressureConverter(),
			new SpeedConverter(),
			new TemperatureConverter(),
			new TimeConverter(),
			new TorqueConverter(),
			new VolumeConverter(),
			new VolumeFlowConverter(),
			new EnergyConsumptionConverter(),
			new kWhWhUnitConverter(),
			new ElectricalResistanceUnitConverter()
		};

		// Token: 0x02000427 RID: 1063
		public enum Units
		{
			// Token: 0x04001942 RID: 6466
			None,
			// Token: 0x04001943 RID: 6467
			kmh,
			// Token: 0x04001944 RID: 6468
			mph,
			// Token: 0x04001945 RID: 6469
			km,
			// Token: 0x04001946 RID: 6470
			miles,
			// Token: 0x04001947 RID: 6471
			kPa,
			// Token: 0x04001948 RID: 6472
			rpm,
			// Token: 0x04001949 RID: 6473
			grads,
			// Token: 0x0400194A RID: 6474
			grams_sec,
			// Token: 0x0400194B RID: 6475
			kg_min,
			// Token: 0x0400194C RID: 6476
			volts,
			// Token: 0x0400194D RID: 6477
			Pa,
			// Token: 0x0400194E RID: 6478
			Lh,
			// Token: 0x0400194F RID: 6479
			Nm,
			// Token: 0x04001950 RID: 6480
			percent,
			// Token: 0x04001951 RID: 6481
			celicium,
			// Token: 0x04001952 RID: 6482
			fahrengheit,
			// Token: 0x04001953 RID: 6483
			seconds,
			// Token: 0x04001954 RID: 6484
			minutes,
			// Token: 0x04001955 RID: 6485
			mA,
			// Token: 0x04001956 RID: 6486
			meters,
			// Token: 0x04001957 RID: 6487
			feet,
			// Token: 0x04001958 RID: 6488
			liters100km,
			// Token: 0x04001959 RID: 6489
			MPG,
			// Token: 0x0400195A RID: 6490
			liters,
			// Token: 0x0400195B RID: 6491
			gallons,
			// Token: 0x0400195C RID: 6492
			Ohm,
			// Token: 0x0400195D RID: 6493
			kOhm,
			// Token: 0x0400195E RID: 6494
			MOhm,
			// Token: 0x0400195F RID: 6495
			MHz,
			// Token: 0x04001960 RID: 6496
			Hz,
			// Token: 0x04001961 RID: 6497
			Vms,
			// Token: 0x04001962 RID: 6498
			Pa_sec,
			// Token: 0x04001963 RID: 6499
			kg_h,
			// Token: 0x04001964 RID: 6500
			g_cyl,
			// Token: 0x04001965 RID: 6501
			g_stroke,
			// Token: 0x04001966 RID: 6502
			mm,
			// Token: 0x04001967 RID: 6503
			lbs,
			// Token: 0x04001968 RID: 6504
			gramms,
			// Token: 0x04001969 RID: 6505
			mV_sec,
			// Token: 0x0400196A RID: 6506
			gpm,
			// Token: 0x0400196B RID: 6507
			ppm,
			// Token: 0x0400196C RID: 6508
			g,
			// Token: 0x0400196D RID: 6509
			m_sec2,
			// Token: 0x0400196E RID: 6510
			ms,
			// Token: 0x0400196F RID: 6511
			hp,
			// Token: 0x04001970 RID: 6512
			kW,
			// Token: 0x04001971 RID: 6513
			psi,
			// Token: 0x04001972 RID: 6514
			bar,
			// Token: 0x04001973 RID: 6515
			mV,
			// Token: 0x04001974 RID: 6516
			km_liter,
			// Token: 0x04001975 RID: 6517
			microseconds,
			// Token: 0x04001976 RID: 6518
			mbar,
			// Token: 0x04001977 RID: 6519
			W,
			// Token: 0x04001978 RID: 6520
			h,
			// Token: 0x04001979 RID: 6521
			m3_hour,
			// Token: 0x0400197A RID: 6522
			mgpc,
			// Token: 0x0400197B RID: 6523
			days,
			// Token: 0x0400197C RID: 6524
			money,
			// Token: 0x0400197D RID: 6525
			A,
			// Token: 0x0400197E RID: 6526
			kWh,
			// Token: 0x0400197F RID: 6527
			Wh,
			// Token: 0x04001980 RID: 6528
			Ah,
			// Token: 0x04001981 RID: 6529
			Hours,
			// Token: 0x04001982 RID: 6530
			μs,
			// Token: 0x04001983 RID: 6531
			g_L,
			// Token: 0x04001984 RID: 6532
			kPsi,
			// Token: 0x04001985 RID: 6533
			mm3,
			// Token: 0x04001986 RID: 6534
			mg_stroke,
			// Token: 0x04001987 RID: 6535
			hPa,
			// Token: 0x04001988 RID: 6536
			revs,
			// Token: 0x04001989 RID: 6537
			grads_sec,
			// Token: 0x0400198A RID: 6538
			mm3_stroke,
			// Token: 0x0400198B RID: 6539
			kHz,
			// Token: 0x0400198C RID: 6540
			MPa,
			// Token: 0x0400198D RID: 6541
			cm,
			// Token: 0x0400198E RID: 6542
			mg_rev,
			// Token: 0x0400198F RID: 6543
			mg,
			// Token: 0x04001990 RID: 6544
			mg_m3,
			// Token: 0x04001991 RID: 6545
			mOhm,
			// Token: 0x04001992 RID: 6546
			mg_cyl,
			// Token: 0x04001993 RID: 6547
			grads_CS,
			// Token: 0x04001994 RID: 6548
			months,
			// Token: 0x04001995 RID: 6549
			lbf_ft,
			// Token: 0x04001996 RID: 6550
			m_s,
			// Token: 0x04001997 RID: 6551
			Kelvin,
			// Token: 0x04001998 RID: 6552
			inch,
			// Token: 0x04001999 RID: 6553
			atm,
			// Token: 0x0400199A RID: 6554
			gph,
			// Token: 0x0400199B RID: 6555
			grams_min,
			// Token: 0x0400199C RID: 6556
			lb_min,
			// Token: 0x0400199D RID: 6557
			lb_sec,
			// Token: 0x0400199E RID: 6558
			lb_h,
			// Token: 0x0400199F RID: 6559
			InHg,
			// Token: 0x040019A0 RID: 6560
			mmHg,
			// Token: 0x040019A1 RID: 6561
			km_kwh,
			// Token: 0x040019A2 RID: 6562
			kWh_100km,
			// Token: 0x040019A3 RID: 6563
			miles_per_kwh,
			// Token: 0x040019A4 RID: 6564
			kwH_100miles,
			// Token: 0x040019A5 RID: 6565
			MPGe,
			// Token: 0x040019A6 RID: 6566
			ATDC,
			// Token: 0x040019A7 RID: 6567
			BTDC,
			// Token: 0x040019A8 RID: 6568
			mg_hour,
			// Token: 0x040019A9 RID: 6569
			per_second,
			// Token: 0x040019AA RID: 6570
			l_per_mm,
			// Token: 0x040019AB RID: 6571
			per_minute,
			// Token: 0x040019AC RID: 6572
			db,
			// Token: 0x040019AD RID: 6573
			L_sec,
			// Token: 0x040019AE RID: 6574
			L_min,
			// Token: 0x040019AF RID: 6575
			mg_km,
			// Token: 0x040019B0 RID: 6576
			mg_sec,
			// Token: 0x040019B1 RID: 6577
			grams_hour
		}

		// Token: 0x02000428 RID: 1064
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002D5B RID: 11611 RVA: 0x001FFC2A File Offset: 0x001FDE2A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002D5C RID: 11612 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002D5D RID: 11613 RVA: 0x001FFC36 File Offset: 0x001FDE36
			internal bool <GetValue>b__3_0(IUnitConverter x)
			{
				return x is EnergyConsumptionConverter;
			}

			// Token: 0x06002D5E RID: 11614 RVA: 0x001FFC36 File Offset: 0x001FDE36
			internal bool <GetValue>b__3_1(IUnitConverter x)
			{
				return x is EnergyConsumptionConverter;
			}

			// Token: 0x06002D5F RID: 11615 RVA: 0x001FFC41 File Offset: 0x001FDE41
			internal string <get_UnitsCaptionCollection>b__6_0(Enum x)
			{
				return UnitsHelper.GetCaptionInvariant((UnitsHelper.Units)x);
			}

			// Token: 0x06002D60 RID: 11616 RVA: 0x001FFC4E File Offset: 0x001FDE4E
			internal string <get_UnitsNamesCollection>b__9_0(Enum x)
			{
				return x.ToString();
			}

			// Token: 0x040019B2 RID: 6578
			public static readonly UnitsHelper.<>c <>9 = new UnitsHelper.<>c();

			// Token: 0x040019B3 RID: 6579
			public static Func<IUnitConverter, bool> <>9__3_0;

			// Token: 0x040019B4 RID: 6580
			public static Func<IUnitConverter, bool> <>9__3_1;

			// Token: 0x040019B5 RID: 6581
			public static Func<Enum, string> <>9__6_0;

			// Token: 0x040019B6 RID: 6582
			public static Func<Enum, string> <>9__9_0;
		}

		// Token: 0x02000429 RID: 1065
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_0
		{
			// Token: 0x06002D61 RID: 11617 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_0()
			{
			}

			// Token: 0x06002D62 RID: 11618 RVA: 0x001FFC56 File Offset: 0x001FDE56
			internal bool <TryGetUnitsFromText>b__0(string x)
			{
				return x.ToLowerInvariant() == this.s.ToLowerInvariant();
			}

			// Token: 0x06002D63 RID: 11619 RVA: 0x001FFC56 File Offset: 0x001FDE56
			internal bool <TryGetUnitsFromText>b__1(string x)
			{
				return x.ToLowerInvariant() == this.s.ToLowerInvariant();
			}

			// Token: 0x040019B7 RID: 6583
			public string s;
		}
	}
}
