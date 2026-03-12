using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200040E RID: 1038
	public class PIDWithFloatValueFormula : PID, IPIDFloatValue, IPID, INotifyPropertyChanged
	{
		// Token: 0x06002A93 RID: 10899 RVA: 0x001F2A53 File Offset: 0x001F0C53
		public PIDWithFloatValueFormula(string Name, string Command, Func<byte[], double> Formula, UnitsHelper.Units unit)
			: base(Name, Command)
		{
			this.Formula = Formula;
			this.Units = unit;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x001F2A78 File Offset: 0x001F0C78
		public PIDWithFloatValueFormula(string Command, Func<byte[], double> Formula, UnitsHelper.Units unit)
			: base(PID.GetResourceString("PID_" + Command), Command)
		{
			this.Formula = Formula;
			this.Units = unit;
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x001F2AAB File Offset: 0x001F0CAB
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = timeStamp;
			this.Value = this.Formula(data);
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x001F2AC6 File Offset: 0x001F0CC6
		public virtual Func<byte[], double> GetFormula()
		{
			return this.Formula;
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06002A97 RID: 10903 RVA: 0x001F2ACE File Offset: 0x001F0CCE
		// (set) Token: 0x06002A98 RID: 10904 RVA: 0x001F2AD6 File Offset: 0x001F0CD6
		public UnitsHelper.Units Units
		{
			[CompilerGenerated]
			get
			{
				return this.<Units>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Units>k__BackingField = value;
			}
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06002A99 RID: 10905 RVA: 0x001F2ADF File Offset: 0x001F0CDF
		// (set) Token: 0x06002A9A RID: 10906 RVA: 0x001F2AE7 File Offset: 0x001F0CE7
		public double Value
		{
			get
			{
				return this._Value;
			}
			protected set
			{
				this._Value = value;
				this.OnValueChanged();
			}
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x001F2AF6 File Offset: 0x001F0CF6
		public void SetValue(double d)
		{
			if (App.OBDReader != null)
			{
				base.TimeStamp = App.OBDReader.stopwatch.Elapsed;
			}
			else
			{
				base.TimeStamp = TimeSpan.Zero;
			}
			this.Value = d;
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x001F2B28 File Offset: 0x001F0D28
		public void SendNaN()
		{
			base.TimeStamp = App.OBDReader.stopwatch.Elapsed;
			this.Value = double.NaN;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x001F2B50 File Offset: 0x001F0D50
		public static void ResetScalingToDefaults()
		{
			PIDWithFloatValueFormula.EquivalenceRatioScalingPerBit = 3.05E-05;
			PIDWithFloatValueFormula.OxygenSensorVoltageScalingPerBit = 0.000122;
			PIDWithFloatValueFormula.OxygenSensorCurrentScalingPerBit = 0.00390625;
			PIDWithFloatValueFormula.IntakeManifoldAbsolutPressureScalingPerBit = 1.0;
			PIDWithFloatValueFormula.MAFScalingPerBit = 0.01;
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06002A9E RID: 10910 RVA: 0x001F2BA3 File Offset: 0x001F0DA3
		// (set) Token: 0x06002A9F RID: 10911 RVA: 0x001F2BAA File Offset: 0x001F0DAA
		public static double EquivalenceRatioScalingPerBit
		{
			[CompilerGenerated]
			get
			{
				return PIDWithFloatValueFormula.<EquivalenceRatioScalingPerBit>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				PIDWithFloatValueFormula.<EquivalenceRatioScalingPerBit>k__BackingField = value;
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x001F2BB2 File Offset: 0x001F0DB2
		// (set) Token: 0x06002AA1 RID: 10913 RVA: 0x001F2BB9 File Offset: 0x001F0DB9
		public static double OxygenSensorVoltageScalingPerBit
		{
			[CompilerGenerated]
			get
			{
				return PIDWithFloatValueFormula.<OxygenSensorVoltageScalingPerBit>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				PIDWithFloatValueFormula.<OxygenSensorVoltageScalingPerBit>k__BackingField = value;
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x06002AA2 RID: 10914 RVA: 0x001F2BC1 File Offset: 0x001F0DC1
		// (set) Token: 0x06002AA3 RID: 10915 RVA: 0x001F2BC8 File Offset: 0x001F0DC8
		public static double OxygenSensorCurrentScalingPerBit
		{
			[CompilerGenerated]
			get
			{
				return PIDWithFloatValueFormula.<OxygenSensorCurrentScalingPerBit>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				PIDWithFloatValueFormula.<OxygenSensorCurrentScalingPerBit>k__BackingField = value;
			}
		}

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x06002AA4 RID: 10916 RVA: 0x001F2BD0 File Offset: 0x001F0DD0
		// (set) Token: 0x06002AA5 RID: 10917 RVA: 0x001F2BD7 File Offset: 0x001F0DD7
		public static double IntakeManifoldAbsolutPressureScalingPerBit
		{
			[CompilerGenerated]
			get
			{
				return PIDWithFloatValueFormula.<IntakeManifoldAbsolutPressureScalingPerBit>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				PIDWithFloatValueFormula.<IntakeManifoldAbsolutPressureScalingPerBit>k__BackingField = value;
			}
		}

		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x06002AA6 RID: 10918 RVA: 0x001F2BDF File Offset: 0x001F0DDF
		// (set) Token: 0x06002AA7 RID: 10919 RVA: 0x001F2BE6 File Offset: 0x001F0DE6
		public static double MAFScalingPerBit
		{
			[CompilerGenerated]
			get
			{
				return PIDWithFloatValueFormula.<MAFScalingPerBit>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				PIDWithFloatValueFormula.<MAFScalingPerBit>k__BackingField = value;
			}
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x06002AA8 RID: 10920 RVA: 0x001F2BEE File Offset: 0x001F0DEE
		// (set) Token: 0x06002AA9 RID: 10921 RVA: 0x001F2BF6 File Offset: 0x001F0DF6
		public string TextValueVariants
		{
			get
			{
				return this._TextValueVariants;
			}
			set
			{
				if (this._TextValueVariants != value)
				{
					this._TextValueVariants = value;
					this.NotifyPropertyChanged("TextValueVariants");
				}
			}
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x001F2C18 File Offset: 0x001F0E18
		public string GetTextValueVariantOrNull(double value)
		{
			if (this._TextValuesDict == null || this._TextValuesDict.Count == 0)
			{
				return null;
			}
			int num = (int)value;
			string text;
			if (this._TextValuesDict.TryGetValue(num, out text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x001F2C54 File Offset: 0x001F0E54
		public static PIDWithFloatValueFormula PID0104_CalculatedEngineLoadValue()
		{
			return new PIDWithFloatValueFormula("0104", (byte[] data) => (double)(data[0] * 100) / 255.0, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 4,
				Role = Roles.LOAD_PCT
			};
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x001F2CBC File Offset: 0x001F0EBC
		public static PIDWithFloatValueFormula PID0105_EngineCoolantTemperature()
		{
			return new PIDWithFloatValueFormula("0105", PIDWithFloatValueFormula.TemperatureFormula, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 5,
				Role = Roles.Coolant,
				SkipCycles = 1
			};
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x001F2D0D File Offset: 0x001F0F0D
		public static PIDWithFloatValueFormula PID0106_STFTB1()
		{
			return new PIDWithFloatValueFormula("0106", PIDWithFloatValueFormula.TrimFormula, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Id = 6
			};
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x001F2D45 File Offset: 0x001F0F45
		public static PIDWithFloatValueFormula PID0107_LTFTB1()
		{
			return new PIDWithFloatValueFormula("0107", PIDWithFloatValueFormula.TrimFormula, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Id = 7
			};
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x001F2D7D File Offset: 0x001F0F7D
		public static PIDWithFloatValueFormula PID0108_STFTB2()
		{
			return new PIDWithFloatValueFormula("0108", PIDWithFloatValueFormula.TrimFormula, UnitsHelper.Units.percent)
			{
				Minimum = 50.0,
				Maximum = 50.0,
				Id = 8
			};
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x001F2DB5 File Offset: 0x001F0FB5
		public static PIDWithFloatValueFormula PID0109_LTFTB2()
		{
			return new PIDWithFloatValueFormula("0109", PIDWithFloatValueFormula.TrimFormula, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Id = 9
			};
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x001F2DF0 File Offset: 0x001F0FF0
		public static PIDWithFloatValueFormula PID010A_FuelPressure()
		{
			return new PIDWithFloatValueFormula("010A", (byte[] data) => (double)data[0] * 3.0, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 765.0,
				Id = 10
			};
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x001F2E50 File Offset: 0x001F1050
		public static PIDWithFloatValueFormula PID010B_IntakeManifoldAbsolutePressure()
		{
			return new PIDWithFloatValueFormula("010B", (byte[] data) => (double)data[0] * PIDWithFloatValueFormula.IntakeManifoldAbsolutPressureScalingPerBit, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 255.0,
				Id = 11,
				Role = Roles.MAP
			};
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x001F2EB4 File Offset: 0x001F10B4
		public static PIDWithFloatValueFormula PID010C_EngineRPM()
		{
			return new PIDWithFloatValueFormula("010C", delegate(byte[] data)
			{
				int num = (int)data[0] * 256;
				num += (int)data[1];
				if (!SharedSettings.Current.UseRPMFix)
				{
					num /= 4;
				}
				return (double)num;
			}, UnitsHelper.Units.rpm)
			{
				Minimum = 0.0,
				Maximum = 7000.0,
				Id = 12,
				Role = Roles.RPM
			};
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x001F2F18 File Offset: 0x001F1118
		public static PIDWithFloatValueFormula PID010D_VehicleSpeed()
		{
			return new PIDWithFloatValueFormula("010D", delegate(byte[] data)
			{
				if (SharedSettings.Current.SpeedPID2Bytes && data.Length >= 2)
				{
					return (double)((int)data[0] * 256 + (int)data[1]) * SharedSettings.Current.SpeedCorrectionFactor;
				}
				return (double)data[0] * SharedSettings.Current.SpeedCorrectionFactor;
			}, UnitsHelper.Units.kmh)
			{
				Minimum = 0.0,
				Maximum = 220.0,
				Id = 13,
				Role = Roles.Speed
			};
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x001F2F7C File Offset: 0x001F117C
		public static PIDWithFloatValueFormula PID010E_TimingAdvance()
		{
			return new PIDWithFloatValueFormula("010E", (byte[] data) => (double)((data[0] - 128) / 2), UnitsHelper.Units.grads)
			{
				Minimum = -64.0,
				Maximum = 64.0,
				ShortName = PID.GetResourceString("PID_010E_Short"),
				Id = 14
			};
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x001F2FE9 File Offset: 0x001F11E9
		public static PIDWithFloatValueFormula PID010F_IntakeAirTemperature()
		{
			return new PIDWithFloatValueFormula("010F", PIDWithFloatValueFormula.TemperatureFormula, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 15,
				Role = Roles.IAT
			};
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x001F302C File Offset: 0x001F122C
		public static PIDWithFloatValueFormula PID0110_MAFAirFlowRate()
		{
			return new PIDWithFloatValueFormula("0110", (byte[] data) => ((double)((int)data[0] * 256) + (double)data[1]) * PIDWithFloatValueFormula.MAFScalingPerBit, UnitsHelper.Units.grams_sec)
			{
				Minimum = 0.0,
				Maximum = 655.35,
				Id = 16,
				Role = Roles.MAF
			};
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x001F3090 File Offset: 0x001F1290
		public static PIDWithFloatValueFormula PID0111_ThrottlePosition()
		{
			return new PIDWithFloatValueFormula("0111", (byte[] data) => (double)data[0] * 100.0 / 255.0, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 17,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x001F30F5 File Offset: 0x001F12F5
		public static PIDWithFloatValueFormula PID0121_DistanceTraveledWithMILLampOn()
		{
			return new PIDWithFloatValueFormula("0121", PIDWithFloatValueFormula.ABFormula, UnitsHelper.Units.km)
			{
				Minimum = 0.0,
				Maximum = 65535.0,
				Id = 41
			};
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x001F3130 File Offset: 0x001F1330
		public static PIDWithFloatValueFormula PID0122_FuelRailPressure_RelativeToManifoldVacuum()
		{
			return new PIDWithFloatValueFormula("0122", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.07900000363588333, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 5177.265,
				Id = 42
			};
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x001F3190 File Offset: 0x001F1390
		public static PIDWithFloatValueFormula PID0123_FuelRailPressure_DieselOrGasolineDirectInject()
		{
			return new PIDWithFloatValueFormula("0123", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 10.0, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 655.35,
				Id = 43
			};
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x001F31F0 File Offset: 0x001F13F0
		public static PIDWithFloatValueFormula PID012C_CommandedEGR()
		{
			return new PIDWithFloatValueFormula("012C", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 60
			};
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x001F324E File Offset: 0x001F144E
		public static PIDWithFloatValueFormula PID012D_EGRError()
		{
			return new PIDWithFloatValueFormula("012D", (byte[] data) => (double)((data[0] - 128) * 100 / 128), UnitsHelper.Units.percent)
			{
				Id = 61
			};
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x001F3283 File Offset: 0x001F1483
		public static PIDWithFloatValueFormula PID012E_CommandedEvaporativePurge()
		{
			return new PIDWithFloatValueFormula("012E", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 62
			};
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x001F32B8 File Offset: 0x001F14B8
		public static PIDWithFloatValueFormula PID012F_FuelLevelInput()
		{
			return new PIDWithFloatValueFormula("012F", delegate(byte[] data)
			{
				if (SharedSettings.Current.UseLitersForVolume)
				{
					double fuelTankCapacity = SharedSettings.Current.FuelTankCapacity;
				}
				else if (SharedSettings.Current.UseUSGallon)
				{
					double fuelTankCapacity2 = SharedSettings.Current.FuelTankCapacity;
				}
				else
				{
					double fuelTankCapacity3 = SharedSettings.Current.FuelTankCapacity;
				}
				return (double)(data[0] * 100 / byte.MaxValue);
			}, UnitsHelper.Units.percent)
			{
				Id = 806,
				Role = Roles.FuelLevelInputPercent,
				Name = Translate.GetString("PID_012F") + " (%)",
				ShortName = Translate.GetString("PID_012F") + " (%)"
			};
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x001F3337 File Offset: 0x001F1537
		public static PIDWithFloatValueFormula PID0130_WarmupsSinceCodesCleared()
		{
			return new PIDWithFloatValueFormula("0130", (byte[] data) => (double)data[0], UnitsHelper.Units.None)
			{
				Id = 64
			};
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x001F336B File Offset: 0x001F156B
		public static PIDWithFloatValueFormula PID0131_DistanceTraveledSinceCodesCleared()
		{
			return new PIDWithFloatValueFormula("0131", PIDWithFloatValueFormula.ABFormula, UnitsHelper.Units.km)
			{
				Id = 65
			};
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x001F3385 File Offset: 0x001F1585
		public static PIDWithFloatValueFormula PID0132_EvapSystemVaporPressure()
		{
			return new PIDWithFloatValueFormula("0132", delegate(byte[] data)
			{
				byte[] array = new byte[]
				{
					data[0],
					data[1]
				};
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				return (double)BitConverter.ToInt16(array, 0) * 0.25;
			}, UnitsHelper.Units.Pa)
			{
				Id = 66
			};
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x001F33BA File Offset: 0x001F15BA
		public static PIDWithFloatValueFormula PID0133_BarometricPressure()
		{
			return new PIDWithFloatValueFormula("0133", (byte[] data) => (double)data[0], UnitsHelper.Units.kPa)
			{
				Id = 67,
				Role = Roles.BARO
			};
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x001F33F6 File Offset: 0x001F15F6
		public static PIDWithFloatValueFormula PID013C_CatalystTemperatureB1S1()
		{
			return new PIDWithFloatValueFormula("013C", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 10.0 - 40.0, UnitsHelper.Units.celicium)
			{
				Id = 84
			};
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x001F342B File Offset: 0x001F162B
		public static PIDWithFloatValueFormula PID013D_CatalystTemperatureB2S1()
		{
			return new PIDWithFloatValueFormula("013D", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 10.0 - 40.0, UnitsHelper.Units.celicium)
			{
				Id = 85
			};
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x001F3460 File Offset: 0x001F1660
		public static PIDWithFloatValueFormula PID013E_CatalystTemperatureB1S2()
		{
			return new PIDWithFloatValueFormula("013E", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 10.0 - 40.0, UnitsHelper.Units.celicium)
			{
				Id = 86
			};
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x001F3495 File Offset: 0x001F1695
		public static PIDWithFloatValueFormula PID013F_CatalystTemperatureB2S2()
		{
			return new PIDWithFloatValueFormula("013F", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 10.0 - 40.0, UnitsHelper.Units.celicium)
			{
				Id = 87
			};
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x001F34CA File Offset: 0x001F16CA
		public static PIDWithFloatValueFormula PID0142_ControlModuleVoltage()
		{
			return new PIDWithFloatValueFormula("0142", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 1000.0, UnitsHelper.Units.volts)
			{
				Id = 90
			};
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x001F34FF File Offset: 0x001F16FF
		public static PIDWithFloatValueFormula PID0143_AbsoluteLoadValue()
		{
			return new PIDWithFloatValueFormula("0143", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 100.0 / 255.0, UnitsHelper.Units.percent)
			{
				Id = 91,
				Role = Roles.LOAD_ABS
			};
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x001F353C File Offset: 0x001F173C
		public static PIDWithFloatValueFormula PID0144_FuelAirCommandedEquivalenceRatio()
		{
			return new PIDWithFloatValueFormula("0144", delegate(byte[] data)
			{
				double num = PIDWithFloatValueFormula.ABFormula(data) * PIDWithFloatValueFormula.EquivalenceRatioScalingPerBit;
				if (SharedSettings.Current.ShowAirFuelBasedOnStoichiometric && SharedSettings.Current.FuelType == FuelTypes.Gasoline)
				{
					num *= 14.64;
				}
				return num;
			}, UnitsHelper.Units.None)
			{
				Id = 92,
				Role = Roles.LAMBDA
			};
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x001F3578 File Offset: 0x001F1778
		public static PIDWithFloatValueFormula PID0145_RelativeThrottlePosition()
		{
			return new PIDWithFloatValueFormula("0145", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 93,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x001F35B4 File Offset: 0x001F17B4
		public static PIDWithFloatValueFormula PID0146_AmbientAirTemperature()
		{
			return new PIDWithFloatValueFormula("0146", PIDWithFloatValueFormula.TemperatureFormula, UnitsHelper.Units.celicium)
			{
				Id = 94,
				Role = Roles.AMBIENT_TEMP
			};
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x001F35D7 File Offset: 0x001F17D7
		public static PIDWithFloatValueFormula PID0147_AbsoluteThrottlePositionB()
		{
			return new PIDWithFloatValueFormula("0147", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 95,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x001F3613 File Offset: 0x001F1813
		public static PIDWithFloatValueFormula PID0148_AbsoluteThrottlePositionC()
		{
			return new PIDWithFloatValueFormula("0148", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 96,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x001F364F File Offset: 0x001F184F
		public static PIDWithFloatValueFormula PID0149_AbsolutePedalPositionD()
		{
			return new PIDWithFloatValueFormula("0149", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 97,
				Role = Roles.AcceleratorPedalPosition
			};
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x001F368C File Offset: 0x001F188C
		public static PIDWithFloatValueFormula PID014A_AbsolutePedalPositionE()
		{
			return new PIDWithFloatValueFormula("014A", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 98,
				Role = Roles.AcceleratorPedalPosition
			};
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x001F36C9 File Offset: 0x001F18C9
		public static PIDWithFloatValueFormula PID014B_AbsolutePedalPositionF()
		{
			return new PIDWithFloatValueFormula("014B", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 99,
				Role = Roles.AcceleratorPedalPosition
			};
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x001F3706 File Offset: 0x001F1906
		public static PIDWithFloatValueFormula PID014C_CommandedThrottleActuator()
		{
			return new PIDWithFloatValueFormula("014C", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 100
			};
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x001F373B File Offset: 0x001F193B
		public static PIDWithFloatValueFormula PID0150_MaximumValueForAirFlowRateFromMassAirFlowSensor()
		{
			return new PIDWithFloatValueFormula("0150", (byte[] data) => (double)data[0] * 10.0 / 65535.0, UnitsHelper.Units.grams_sec)
			{
				Id = 105
			};
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x001F376F File Offset: 0x001F196F
		public static PIDWithFloatValueFormula PID0152_EthanolFuelPercent()
		{
			return new PIDWithFloatValueFormula("0152", (byte[] data) => (double)data[0] * 100.0 / 255.0, UnitsHelper.Units.percent)
			{
				Id = 107
			};
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x001F37A4 File Offset: 0x001F19A4
		public static PIDWithFloatValueFormula PID0153_AbsoluteEvapSystemVaporPressure()
		{
			return new PIDWithFloatValueFormula("0153", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 200.0, UnitsHelper.Units.kPa)
			{
				Id = 108
			};
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x001F37D8 File Offset: 0x001F19D8
		public static PIDWithFloatValueFormula PID0154_EvapSystemVaporPressure()
		{
			return new PIDWithFloatValueFormula("0154", delegate(byte[] data)
			{
				byte[] array = new byte[]
				{
					data[0],
					data[1]
				};
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				return (double)BitConverter.ToInt16(data, 0);
			}, UnitsHelper.Units.Pa)
			{
				Id = 109
			};
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x001F380D File Offset: 0x001F1A0D
		public static PIDWithFloatValueFormula PID0159_FuelRailPressureAbsolute()
		{
			return new PIDWithFloatValueFormula("0159", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 10.0, UnitsHelper.Units.kPa)
			{
				Id = 118
			};
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x001F3841 File Offset: 0x001F1A41
		public static PIDWithFloatValueFormula PID015A_RelativeAcceleratorPedalPosition()
		{
			return new PIDWithFloatValueFormula("015A", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 119
			};
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x001F3876 File Offset: 0x001F1A76
		public static PIDWithFloatValueFormula PID015B_HybridBatteryPackRemainingLife()
		{
			return new PIDWithFloatValueFormula("015B", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Id = 120
			};
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x001F38AB File Offset: 0x001F1AAB
		public static PIDWithFloatValueFormula PID015C_EngineOilTemperature()
		{
			return new PIDWithFloatValueFormula("015C", PIDWithFloatValueFormula.TemperatureFormula, UnitsHelper.Units.celicium)
			{
				Id = 121
			};
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x001F38C6 File Offset: 0x001F1AC6
		public static PIDWithFloatValueFormula PID015D_FuelInjectionTiming()
		{
			return new PIDWithFloatValueFormula("015D", (byte[] data) => (PIDWithFloatValueFormula.ABFormula(data) - 26880.0) / 128.0, UnitsHelper.Units.grads)
			{
				Id = 122,
				Role = Roles.None
			};
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x001F3901 File Offset: 0x001F1B01
		public static PIDWithFloatValueFormula PID015E_EngineFuelRate()
		{
			return new PIDWithFloatValueFormula("015E", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.05000000074505806, UnitsHelper.Units.Lh)
			{
				Id = 123,
				Role = Roles.InstantFuelRate
			};
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x001F393E File Offset: 0x001F1B3E
		public static PIDWithFloatValueFormula PID0161_DriversDemandEnginePercentTorque()
		{
			return new PIDWithFloatValueFormula("0161", (byte[] data) => (double)(data[0] - 125), UnitsHelper.Units.percent)
			{
				Id = 125
			};
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x001F3973 File Offset: 0x001F1B73
		public static PIDWithFloatValueFormula PID0162_ActualEnginePercentTorque()
		{
			return new PIDWithFloatValueFormula("0162", (byte[] data) => (double)(data[0] - 125), UnitsHelper.Units.percent)
			{
				Id = 126
			};
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x001F39A8 File Offset: 0x001F1BA8
		public static PIDWithFloatValueFormula PID0163_EngineReferenceTorque()
		{
			return new PIDWithFloatValueFormula("0163", PIDWithFloatValueFormula.ABFormula, UnitsHelper.Units.Nm)
			{
				Id = 127,
				Role = Roles.EngineTorque
			};
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x001F39CB File Offset: 0x001F1BCB
		public static PIDWithFloatValueFormula PIDATRV_OBDVoltage()
		{
			return new PID_ATRV();
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x001F39D4 File Offset: 0x001F1BD4
		public static PIDWithFloatValueFormula PID0114_O2S1B1_Voltage()
		{
			string text = "0114";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaVoltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 20,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x001F3A60 File Offset: 0x001F1C60
		public static PIDWithFloatValueFormula PID0114_O2S1B1_Trim()
		{
			string text = "0114";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaTrim, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_STFT"),
				Id = 21,
				ShortName = PID.GetResourceString("PID_" + text + "_Trim_Short")
			};
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x001F3AEC File Offset: 0x001F1CEC
		public static PIDWithFloatValueFormula PID0115_O2S2B1_Voltage()
		{
			string text = "0115";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaVoltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 22,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x001F3B78 File Offset: 0x001F1D78
		public static PIDWithFloatValueFormula PID0115_O2S2B1_Trim()
		{
			string text = "0115";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaTrim, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_STFT"),
				Id = 23,
				ShortName = PID.GetResourceString("PID_" + text + "_Trim_Short")
			};
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x001F3C04 File Offset: 0x001F1E04
		public static PIDWithFloatValueFormula PID0116_O2S3B1_Voltage()
		{
			string text = "0116";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaVoltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 24,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x001F3C90 File Offset: 0x001F1E90
		public static PIDWithFloatValueFormula PID0116_O2S3B1_Trim()
		{
			string text = "0116";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaTrim, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_STFT"),
				Id = 25,
				ShortName = PID.GetResourceString("PID_" + text + "_Trim_Short")
			};
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x001F3D1C File Offset: 0x001F1F1C
		public static PIDWithFloatValueFormula PID0117_O2S4B1_Voltage()
		{
			string text = "0117";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaVoltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 26,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x001F3DA8 File Offset: 0x001F1FA8
		public static PIDWithFloatValueFormula PID0117_O2S4B1_Trim()
		{
			string text = "0117";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaTrim, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_STFT"),
				Id = 27,
				ShortName = PID.GetResourceString("PID_" + text + "_Trim_Short")
			};
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x001F3E34 File Offset: 0x001F2034
		public static PIDWithFloatValueFormula PID0118_O2S1B2_Voltage()
		{
			string text = "0118";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaVoltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 28,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x001F3EC0 File Offset: 0x001F20C0
		public static PIDWithFloatValueFormula PID0118_O2S1B2_Trim()
		{
			string text = "0118";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaTrim, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_STFT"),
				Id = 29,
				ShortName = PID.GetResourceString("PID_" + text + "_Trim_Short")
			};
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x001F3F4C File Offset: 0x001F214C
		public static PIDWithFloatValueFormula PID0119_O2S2B2_Voltage()
		{
			string text = "0119";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaVoltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 30,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x001F3FD8 File Offset: 0x001F21D8
		public static PIDWithFloatValueFormula PID0119_O2S2B2_Trim()
		{
			string text = "0119";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaTrim, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_STFT"),
				Id = 31,
				ShortName = PID.GetResourceString("PID_" + text + "_Trim_Short")
			};
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x001F4064 File Offset: 0x001F2264
		public static PIDWithFloatValueFormula PID011A_O2S3B2_Voltage()
		{
			string text = "011A";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaVoltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 32,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x001F40F0 File Offset: 0x001F22F0
		public static PIDWithFloatValueFormula PID011A_O2S3B2_Trim()
		{
			string text = "011A";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaTrim, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_STFT"),
				Id = 33,
				ShortName = PID.GetResourceString("PID_" + text + "_Trim_Short")
			};
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x001F417C File Offset: 0x001F237C
		public static PIDWithFloatValueFormula PID011B_O2S4B2_Voltage()
		{
			string text = "011B";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaVoltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 34,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x001F4208 File Offset: 0x001F2408
		public static PIDWithFloatValueFormula PID011B_O2S4B2_Trim()
		{
			string text = "011B";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2EasyFormulaTrim, UnitsHelper.Units.percent)
			{
				Minimum = -50.0,
				Maximum = 50.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_STFT"),
				Id = 35,
				ShortName = PID.GetResourceString("PID_" + text + "_Trim_Short")
			};
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x001F4294 File Offset: 0x001F2494
		public static PIDWithFloatValueFormula PID0124_O2S1WR_EqRatio()
		{
			string text = "0124";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 44,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x001F4328 File Offset: 0x001F2528
		public static PIDWithFloatValueFormula PID0124_O2S1WR_Voltage()
		{
			string text = "0124";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_Voltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 8.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 45,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x001F43B4 File Offset: 0x001F25B4
		public static PIDWithFloatValueFormula PID0125_O2S2WR_EqRatio()
		{
			string text = "0125";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 46,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x001F4448 File Offset: 0x001F2648
		public static PIDWithFloatValueFormula PID0125_O2S2WR_Voltage()
		{
			string text = "0125";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_Voltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 8.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 47,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x001F44D4 File Offset: 0x001F26D4
		public static PIDWithFloatValueFormula PID0126_O2S3WR_EqRatio()
		{
			string text = "0126";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 48,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x001F4568 File Offset: 0x001F2768
		public static PIDWithFloatValueFormula PID0126_O2S3WR_Voltage()
		{
			string text = "0126";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_Voltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 8.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 49,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x001F45F4 File Offset: 0x001F27F4
		public static PIDWithFloatValueFormula PID0127_O2S4WR_EqRatio()
		{
			string text = "0127";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 50,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x001F4688 File Offset: 0x001F2888
		public static PIDWithFloatValueFormula PID0127_O2S4WR_Voltage()
		{
			string text = "0127";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_Voltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 8.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 51,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x001F4714 File Offset: 0x001F2914
		public static PIDWithFloatValueFormula PID0128_O2S5WR_EqRatio()
		{
			string text = "0128";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 52,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x001F47A8 File Offset: 0x001F29A8
		public static PIDWithFloatValueFormula PID0128_O2S5WR_Voltage()
		{
			string text = "0128";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_Voltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 8.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 53,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x001F4834 File Offset: 0x001F2A34
		public static PIDWithFloatValueFormula PID0129_O2S4WR_EqRatio()
		{
			string text = "0129";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 54,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x001F48C8 File Offset: 0x001F2AC8
		public static PIDWithFloatValueFormula PID0129_O2S4WR_Voltage()
		{
			string text = "0129";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_Voltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 8.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 55,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x001F4954 File Offset: 0x001F2B54
		public static PIDWithFloatValueFormula PID012A_O2S7WR_EqRatio()
		{
			string text = "012A";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 56,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x001F49E8 File Offset: 0x001F2BE8
		public static PIDWithFloatValueFormula PID012A_O2S7WR_Voltage()
		{
			string text = "012A";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_Voltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 8.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 57,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x001F4A74 File Offset: 0x001F2C74
		public static PIDWithFloatValueFormula PID012B_O2S8WR_EqRatio()
		{
			string text = "012B";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 58,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x001F4B08 File Offset: 0x001F2D08
		public static PIDWithFloatValueFormula PID012B_O2S8WR_Voltage()
		{
			string text = "012B";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula_Voltage, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 8.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_Voltage"),
				Id = 59,
				ShortName = PID.GetResourceString("PID_" + text + "_Volt_Short")
			};
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x001F4B94 File Offset: 0x001F2D94
		public static PIDWithFloatValueFormula PID0134_O2S1WR_CurrentmA()
		{
			string text = "0134";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_Current, UnitsHelper.Units.mA)
			{
				Minimum = -128.0,
				Maximum = 128.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_CurrentmA"),
				Id = 68,
				ShortName = PID.GetResourceString("PID_" + text + "_Current_Short")
			};
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x001F4C20 File Offset: 0x001F2E20
		public static PIDWithFloatValueFormula PID0134_O2S1WR_EqRatio()
		{
			string text = "0134";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 69,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x001F4CB4 File Offset: 0x001F2EB4
		public static PIDWithFloatValueFormula PID0135_O2S2WR_CurrentmA()
		{
			string text = "0135";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_Current, UnitsHelper.Units.mA)
			{
				Minimum = -128.0,
				Maximum = 128.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_CurrentmA"),
				Id = 70,
				ShortName = PID.GetResourceString("PID_" + text + "_Current_Short")
			};
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x001F4D40 File Offset: 0x001F2F40
		public static PIDWithFloatValueFormula PID0135_O2S2WR_EqRatio()
		{
			string text = "0135";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 71,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x001F4DD4 File Offset: 0x001F2FD4
		public static PIDWithFloatValueFormula PID0136_O2S3WR_CurrentmA()
		{
			string text = "0136";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_Current, UnitsHelper.Units.mA)
			{
				Minimum = -128.0,
				Maximum = 128.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_CurrentmA"),
				Id = 72,
				ShortName = PID.GetResourceString("PID_" + text + "_Current_Short")
			};
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x001F4E60 File Offset: 0x001F3060
		public static PIDWithFloatValueFormula PID0136_O2S3WR_EqRatio()
		{
			string text = "0136";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 73,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x001F4EF4 File Offset: 0x001F30F4
		public static PIDWithFloatValueFormula PID0137_O2S4WR_CurrentmA()
		{
			string text = "0137";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_Current, UnitsHelper.Units.mA)
			{
				Minimum = -128.0,
				Maximum = 128.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_CurrentmA"),
				Id = 74,
				ShortName = PID.GetResourceString("PID_" + text + "_Current_Short")
			};
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x001F4F80 File Offset: 0x001F3180
		public static PIDWithFloatValueFormula PID0137_O2S4WR_EqRatio()
		{
			string text = "0137";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 75,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x001F5014 File Offset: 0x001F3214
		public static PIDWithFloatValueFormula PID0138_O2S5WR_CurrentmA()
		{
			string text = "0138";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_Current, UnitsHelper.Units.mA)
			{
				Minimum = -128.0,
				Maximum = 128.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_CurrentmA"),
				Id = 76,
				ShortName = PID.GetResourceString("PID_" + text + "_Current_Short")
			};
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x001F50A0 File Offset: 0x001F32A0
		public static PIDWithFloatValueFormula PID0138_O2S5WR_EqRatio()
		{
			string text = "0138";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 77,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x001F5134 File Offset: 0x001F3334
		public static PIDWithFloatValueFormula PID0139_O2S6WR_CurrentmA()
		{
			string text = "0139";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_Current, UnitsHelper.Units.mA)
			{
				Minimum = -128.0,
				Maximum = 128.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_CurrentmA"),
				Id = 78,
				ShortName = PID.GetResourceString("PID_" + text + "_Current_Short")
			};
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x001F51C0 File Offset: 0x001F33C0
		public static PIDWithFloatValueFormula PID0139_O2S6WR_EqRatio()
		{
			string text = "0139";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 79,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x001F5254 File Offset: 0x001F3454
		public static PIDWithFloatValueFormula PID013A_O2S7WR_CurrentmA()
		{
			string text = "013A";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_Current, UnitsHelper.Units.mA)
			{
				Minimum = -128.0,
				Maximum = 128.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_CurrentmA"),
				Id = 80,
				ShortName = PID.GetResourceString("PID_" + text + "_Current_Short")
			};
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x001F52E0 File Offset: 0x001F34E0
		public static PIDWithFloatValueFormula PID013A_O2S7WR_EqRatio()
		{
			string text = "013A";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 81,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x001F5374 File Offset: 0x001F3574
		public static PIDWithFloatValueFormula PID013B_O2S8WR_CurrentmA()
		{
			string text = "013B";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_Current, UnitsHelper.Units.mA)
			{
				Minimum = -128.0,
				Maximum = 128.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_CurrentmA"),
				Id = 82,
				ShortName = PID.GetResourceString("PID_" + text + "_Current_Short")
			};
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x001F5400 File Offset: 0x001F3600
		public static PIDWithFloatValueFormula PID013B_O2S8WR_EqRatio()
		{
			string text = "013B";
			return new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.O2_WR_ABCD_Formula2_EqRatio, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 2.0,
				Name = PID.GetResourceString("PID_" + text) + " " + PID.GetResourceString("PID_EquivalenceRatio"),
				Id = 83,
				Role = Roles.LAMBDA,
				ShortName = PID.GetResourceString("PID_" + text + "_Eq_Short")
			};
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x001F5494 File Offset: 0x001F3694
		public static PIDWithFloatValueFormula PID0155_ShortTermSecondaryOxygenSensorTrimB1()
		{
			string text = "0155";
			PIDWithFloatValueFormula pidwithFloatValueFormula = new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.SecondaryOxygenSensorTrimBank1And2, UnitsHelper.Units.percent);
			pidwithFloatValueFormula.Minimum = -100.0;
			pidwithFloatValueFormula.Maximum = 100.0;
			pidwithFloatValueFormula.Name = PID.GetResourceString("PID_" + text) + " Bank 1";
			pidwithFloatValueFormula.Id = 110;
			pidwithFloatValueFormula.ShortName = pidwithFloatValueFormula.Name;
			return pidwithFloatValueFormula;
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x001F5508 File Offset: 0x001F3708
		public static PIDWithFloatValueFormula PID0155_ShortTermSecondaryOxygenSensorTrimB3()
		{
			string text = "0155";
			PIDWithFloatValueFormula pidwithFloatValueFormula = new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.SecondaryOxygenSensorTrimBank3And4, UnitsHelper.Units.percent);
			pidwithFloatValueFormula.Minimum = -100.0;
			pidwithFloatValueFormula.Maximum = 100.0;
			pidwithFloatValueFormula.Name = PID.GetResourceString("PID_" + text) + " Bank 3";
			pidwithFloatValueFormula.Id = 111;
			pidwithFloatValueFormula.ShortName = pidwithFloatValueFormula.Name;
			return pidwithFloatValueFormula;
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x001F557C File Offset: 0x001F377C
		public static PIDWithFloatValueFormula PID0156_LongTermSecondaryOxygenSensorTrimB1()
		{
			string text = "0156";
			PIDWithFloatValueFormula pidwithFloatValueFormula = new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.SecondaryOxygenSensorTrimBank1And2, UnitsHelper.Units.percent);
			pidwithFloatValueFormula.Minimum = -100.0;
			pidwithFloatValueFormula.Maximum = 100.0;
			pidwithFloatValueFormula.Name = PID.GetResourceString("PID_" + text) + " Bank 1";
			pidwithFloatValueFormula.Id = 112;
			pidwithFloatValueFormula.ShortName = pidwithFloatValueFormula.Name;
			return pidwithFloatValueFormula;
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x001F55F0 File Offset: 0x001F37F0
		public static PIDWithFloatValueFormula PID0156_LongTermSecondaryOxygenSensorTrimB3()
		{
			string text = "0156";
			PIDWithFloatValueFormula pidwithFloatValueFormula = new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.SecondaryOxygenSensorTrimBank3And4, UnitsHelper.Units.percent);
			pidwithFloatValueFormula.Minimum = -100.0;
			pidwithFloatValueFormula.Maximum = 100.0;
			pidwithFloatValueFormula.Name = PID.GetResourceString("PID_" + text) + " Bank 3";
			pidwithFloatValueFormula.Id = 113;
			pidwithFloatValueFormula.ShortName = pidwithFloatValueFormula.Name;
			return pidwithFloatValueFormula;
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x001F5664 File Offset: 0x001F3864
		public static PIDWithFloatValueFormula PID0157_ShortTermSecondaryOxygenSensorTrimB2()
		{
			string text = "0157";
			PIDWithFloatValueFormula pidwithFloatValueFormula = new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.SecondaryOxygenSensorTrimBank1And2, UnitsHelper.Units.percent);
			pidwithFloatValueFormula.Minimum = -100.0;
			pidwithFloatValueFormula.Maximum = 100.0;
			pidwithFloatValueFormula.Name = PID.GetResourceString("PID_" + text) + " Bank 2";
			pidwithFloatValueFormula.Id = 114;
			pidwithFloatValueFormula.ShortName = pidwithFloatValueFormula.Name;
			return pidwithFloatValueFormula;
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x001F56D8 File Offset: 0x001F38D8
		public static PIDWithFloatValueFormula PID0157_ShortTermSecondaryOxygenSensorTrimB4()
		{
			string text = "0157";
			PIDWithFloatValueFormula pidwithFloatValueFormula = new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.SecondaryOxygenSensorTrimBank3And4, UnitsHelper.Units.percent);
			pidwithFloatValueFormula.Minimum = -100.0;
			pidwithFloatValueFormula.Maximum = 100.0;
			pidwithFloatValueFormula.Name = PID.GetResourceString("PID_" + text) + " Bank 4";
			pidwithFloatValueFormula.Id = 115;
			pidwithFloatValueFormula.ShortName = pidwithFloatValueFormula.Name;
			return pidwithFloatValueFormula;
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x001F574C File Offset: 0x001F394C
		public static PIDWithFloatValueFormula PID0158_LongTermSecondaryOxygenSensorTrimB2()
		{
			string text = "0158";
			PIDWithFloatValueFormula pidwithFloatValueFormula = new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.SecondaryOxygenSensorTrimBank1And2, UnitsHelper.Units.percent);
			pidwithFloatValueFormula.Minimum = -100.0;
			pidwithFloatValueFormula.Maximum = 100.0;
			pidwithFloatValueFormula.Name = PID.GetResourceString("PID_" + text) + " Bank 2";
			pidwithFloatValueFormula.Id = 116;
			pidwithFloatValueFormula.ShortName = pidwithFloatValueFormula.Name;
			return pidwithFloatValueFormula;
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x001F57C0 File Offset: 0x001F39C0
		public static PIDWithFloatValueFormula PID0158_LongTermSecondaryOxygenSensorTrimB4()
		{
			string text = "0158";
			PIDWithFloatValueFormula pidwithFloatValueFormula = new PIDWithFloatValueFormula(text, PIDWithFloatValueFormula.SecondaryOxygenSensorTrimBank3And4, UnitsHelper.Units.percent);
			pidwithFloatValueFormula.Minimum = -100.0;
			pidwithFloatValueFormula.Maximum = 100.0;
			pidwithFloatValueFormula.Name = PID.GetResourceString("PID_" + text) + " Bank 4";
			pidwithFloatValueFormula.Id = 117;
			pidwithFloatValueFormula.ShortName = pidwithFloatValueFormula.Name;
			return pidwithFloatValueFormula;
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x001F5834 File Offset: 0x001F3A34
		public static PIDWithFloatValueFormula PID0164_EnginePercentTorqueDataAtIdle()
		{
			return new PIDWithFloatValueFormula("0164", (byte[] data) => (double)(data[0] - 125), UnitsHelper.Units.percent)
			{
				Minimum = -125.0,
				Maximum = 125.0,
				Name = PID.GetResourceString("PID_0164_1"),
				ShortName = PID.GetResourceString("PID_0164_1_Short"),
				Id = 128
			};
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x001F58B8 File Offset: 0x001F3AB8
		public static PIDWithFloatValueFormula PID0164_EnginePercentTorqueDataAtPoint2()
		{
			return new PIDWithFloatValueFormula("0164", (byte[] data) => (double)(data[1] - 125), UnitsHelper.Units.percent)
			{
				Minimum = -125.0,
				Maximum = 125.0,
				Name = PID.GetResourceString("PID_0164_2"),
				ShortName = PID.GetResourceString("PID_0164_2_Short"),
				Id = 129
			};
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x001F593C File Offset: 0x001F3B3C
		public static PIDWithFloatValueFormula PID0164_EnginePercentTorqueDataAtPoint3()
		{
			return new PIDWithFloatValueFormula("0164", (byte[] data) => (double)(data[2] - 125), UnitsHelper.Units.percent)
			{
				Minimum = -125.0,
				Maximum = 125.0,
				Name = PID.GetResourceString("PID_0164_3"),
				ShortName = PID.GetResourceString("PID_0164_3_Short"),
				Id = 130
			};
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x001F59C0 File Offset: 0x001F3BC0
		public static PIDWithFloatValueFormula PID0164_EnginePercentTorqueDataAtPoint4()
		{
			return new PIDWithFloatValueFormula("0164", (byte[] data) => (double)(data[3] - 125), UnitsHelper.Units.percent)
			{
				Minimum = -125.0,
				Maximum = 125.0,
				Name = PID.GetResourceString("PID_0164_4"),
				ShortName = PID.GetResourceString("PID_0164_4_Short"),
				Id = 131
			};
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x001F5A44 File Offset: 0x001F3C44
		public static PIDWithFloatValueFormula PID0164_EnginePercentTorqueDataAtPoint5()
		{
			return new PIDWithFloatValueFormula("0164", (byte[] data) => (double)(data[4] - 125), UnitsHelper.Units.percent)
			{
				Minimum = -125.0,
				Maximum = 125.0,
				Name = PID.GetResourceString("PID_0164_5"),
				ShortName = PID.GetResourceString("PID_0164_5_Short"),
				Id = 132
			};
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x001F5AC8 File Offset: 0x001F3CC8
		public static PIDWithFloatValueFormula PID0166_MAFSensorA()
		{
			return new PIDWithFloatValueFormula("0166", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.03125;
				}
				return double.NaN;
			}, UnitsHelper.Units.grams_sec)
			{
				Minimum = 0.0,
				Maximum = 2050.0,
				Name = PID.GetResourceString("PID_0166_A"),
				ShortName = PID.GetResourceString("PID_0166_A_Short"),
				Id = 133,
				Role = Roles.MAF
			};
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x001F5B50 File Offset: 0x001F3D50
		public static PIDWithFloatValueFormula PID0166_MAFSensorB()
		{
			return new PIDWithFloatValueFormula("0166", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.03125;
				}
				return double.NaN;
			}, UnitsHelper.Units.grams_sec)
			{
				Minimum = 0.0,
				Maximum = 2050.0,
				Name = PID.GetResourceString("PID_0166_B"),
				ShortName = PID.GetResourceString("PID_0166_B_Short"),
				Id = 134,
				Role = Roles.MAF
			};
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x001F5BD8 File Offset: 0x001F3DD8
		public static PIDWithFloatValueFormula PID0167_ECTSensorA()
		{
			return new PIDWithFloatValueFormula("0167", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Name = PID.GetResourceString("PID_0105") + " (A)",
				ShortName = PID.GetResourceString("PID_0105") + " (A)",
				Id = 135,
				Role = Roles.Coolant
			};
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x001F5C74 File Offset: 0x001F3E74
		public static PIDWithFloatValueFormula PID0167_ECTSensorB()
		{
			return new PIDWithFloatValueFormula("0167", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Name = PID.GetResourceString("PID_0105") + " (B)",
				ShortName = PID.GetResourceString("PID_0105") + " (B)",
				Id = 136,
				Role = Roles.Coolant
			};
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x001F5D10 File Offset: 0x001F3F10
		public static PIDWithFloatValueFormula PID0168_IATSensorB1S1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_010F") + " B1S1", "0168", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 137,
				Role = Roles.IAT
			};
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x001F5D8C File Offset: 0x001F3F8C
		public static PIDWithFloatValueFormula PID0168_IATSensorB1S2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_010F") + " B1S2", "0168", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 138,
				Role = Roles.IAT
			};
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x001F5E08 File Offset: 0x001F4008
		public static PIDWithFloatValueFormula PID0168_IATSensorB1S3()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_010F") + " B1S3", "0168", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 139,
				Role = Roles.IAT
			};
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x001F5E84 File Offset: 0x001F4084
		public static PIDWithFloatValueFormula PID0168_IATSensorB2S1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_010F") + " B2S1", "0168", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 140,
				Role = Roles.IAT
			};
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x001F5F00 File Offset: 0x001F4100
		public static PIDWithFloatValueFormula PID0168_IATSensorB2S2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_010F") + " B2S2", "0168", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 5)) && data.Length > 5)
				{
					return (double)(data[5] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 141,
				Role = Roles.IAT
			};
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x001F5F7C File Offset: 0x001F417C
		public static PIDWithFloatValueFormula PID0168_IATSensorB2S3()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_010F") + " B2S3", "0168", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 6)) && data.Length > 6)
				{
					return (double)(data[6] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 142,
				Role = Roles.IAT
			};
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x001F5FF8 File Offset: 0x001F41F8
		public static PIDWithFloatValueFormula PID0169_CommandedEGRDutyCycleA()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_012C") + " (A)", "0169", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 143
			};
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x001F6070 File Offset: 0x001F4270
		public static PIDWithFloatValueFormula PID0169_ActualEGRDutyCycleA()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0169_ActualEGR") + " (A)", "0169", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_0_7(data[0], 1)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 144
			};
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x001F60E8 File Offset: 0x001F42E8
		public static PIDWithFloatValueFormula PID0169_EGRErrorA()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_012D") + " (A)", "0169", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)((data[3] - 128) * 100 / 128);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = -100.0,
				Maximum = 100.0,
				Id = 145
			};
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x001F6160 File Offset: 0x001F4360
		public static PIDWithFloatValueFormula PID0169_CommandedEGRDutyCycleB()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_012C") + " (B)", "0169", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 146
			};
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x001F61D8 File Offset: 0x001F43D8
		public static PIDWithFloatValueFormula PID0169_ActualEGRDutyCycleB()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0169_ActualEGR") + " (B)", "0169", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 5)) && data.Length > 5)
				{
					return (double)(data[5] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 147
			};
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x001F6250 File Offset: 0x001F4450
		public static PIDWithFloatValueFormula PID0169_EGRErrorB()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_012D") + " (B)", "0169", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 6)) && data.Length > 6)
				{
					return (double)((data[6] - 128) * 100 / 128);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = -100.0,
				Maximum = 100.0,
				Id = 148
			};
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x001F62C8 File Offset: 0x001F44C8
		public static PIDWithFloatValueFormula PID016A_CommandedIntakeAirFlowAControl()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016A_1"), "016A", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 149
			};
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x001F6334 File Offset: 0x001F4534
		public static PIDWithFloatValueFormula PID016A_RelativeIntakeAirFlowAPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016A_2"), "016A", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 150
			};
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x001F63A0 File Offset: 0x001F45A0
		public static PIDWithFloatValueFormula PID016A_CommandedIntakeAirFlowBControl()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016A_3"), "016A", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 151
			};
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x001F640C File Offset: 0x001F460C
		public static PIDWithFloatValueFormula PID016A_RelativeIntakeAirFlowBPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016A_4"), "016A", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 152
			};
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x001F6478 File Offset: 0x001F4678
		public static PIDWithFloatValueFormula PID016B_ExhaustGasRecirculationTempBank1Sensor1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016B_1"), "016B", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 153
			};
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x001F64E4 File Offset: 0x001F46E4
		public static PIDWithFloatValueFormula PID016B_ExhaustGasRecirculationTempBank1Sensor2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016B_2"), "016B", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 154
			};
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x001F6550 File Offset: 0x001F4750
		public static PIDWithFloatValueFormula PID016B_ExhaustGasRecirculationTempBank2Sensor1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016B_3"), "016B", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 155
			};
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x001F65BC File Offset: 0x001F47BC
		public static PIDWithFloatValueFormula PID016B_ExhaustGasRecirculationTempBank2Sensor2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016B_4"), "016B", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 156
			};
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x001F6628 File Offset: 0x001F4828
		public static PIDWithFloatValueFormula PID016C_CommandedThrottleActuatorAControl()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016C_1"), "016C", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 157
			};
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x001F6694 File Offset: 0x001F4894
		public static PIDWithFloatValueFormula PID016C_RelativeThrottleAPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016C_2"), "016C", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 158,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x001F6708 File Offset: 0x001F4908
		public static PIDWithFloatValueFormula PID016C_CommandedThrottleActuatorBControl()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016C_3"), "016C", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 159
			};
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x001F6774 File Offset: 0x001F4974
		public static PIDWithFloatValueFormula PID016C_RelativeThrottleBPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016C_4"), "016C", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 160,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x001F67E8 File Offset: 0x001F49E8
		public static PIDWithFloatValueFormula PID016D_CommandedFuelRailPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016D_1"), "016D", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)(((int)data[1] * 256 + (int)data[2]) * 10);
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 655350.0,
				Id = 161
			};
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x001F6854 File Offset: 0x001F4A54
		public static PIDWithFloatValueFormula PID016D_FuelRailPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016D_2"), "016D", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)(((int)data[3] * 256 + (int)data[4]) * 10);
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 655350.0,
				Id = 162
			};
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x001F68C0 File Offset: 0x001F4AC0
		public static PIDWithFloatValueFormula PID016D_FuelRailTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016D_3"), "016D", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 5)
				{
					return (double)(data[5] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 163
			};
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x001F692C File Offset: 0x001F4B2C
		public static PIDWithFloatValueFormula PID016E_CommandedInjectionControlPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016E_1"), "016E", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)(((int)data[1] * 256 + (int)data[2]) * 10);
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 655350.0,
				Id = 164
			};
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x001F6998 File Offset: 0x001F4B98
		public static PIDWithFloatValueFormula PID016E_InjectionControlPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016E_2"), "016E", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)(((int)data[3] * 256 + (int)data[4]) * 10);
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 655350.0,
				Id = 165
			};
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x001F6A04 File Offset: 0x001F4C04
		public static PIDWithFloatValueFormula PID016F_TurbochargerCompressorInletPressureSensorA()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016F_1"), "016F", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)data[1];
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 255.0,
				Id = 166
			};
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x001F6A70 File Offset: 0x001F4C70
		public static PIDWithFloatValueFormula PID016F_TurbochargerCompressorInletPressureSensorB()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_016F_2"), "016F", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)data[2];
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 255.0,
				Id = 167
			};
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x001F6ADC File Offset: 0x001F4CDC
		public static PIDWithFloatValueFormula PID0170_CommandedBoostPressureA()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0170_1"), "0170", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.03125;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 2050.0,
				Id = 168
			};
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x001F6B48 File Offset: 0x001F4D48
		public static PIDWithFloatValueFormula PID0170_BoostPressureSensorA()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0170_2"), "0170", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.03125;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 2050.0,
				Id = 169
			};
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x001F6BB4 File Offset: 0x001F4DB4
		public static PIDWithFloatValueFormula PID0170_CommandedBoostPressureB()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0170_3"), "0170", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.03125;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 2050.0,
				Id = 170
			};
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x001F6C20 File Offset: 0x001F4E20
		public static PIDWithFloatValueFormula PID0170_BoostPressureSensorB()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0170_4"), "0170", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 8)
				{
					return (double)((int)data[7] * 256 + (int)data[8]) * 0.03125;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 2050.0,
				Id = 171
			};
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x001F6C8C File Offset: 0x001F4E8C
		public static PIDWithFloatValueFormula PID0171_CommandedVariableGeometryTurboAPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0171_1"), "0171", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 172
			};
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x001F6CF8 File Offset: 0x001F4EF8
		public static PIDWithFloatValueFormula PID0171_VariableGeometryTurboAPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0171_2"), "0171", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 173
			};
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x001F6D64 File Offset: 0x001F4F64
		public static PIDWithFloatValueFormula PID0171_CommandedVariableGeometryTurboBPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0171_3"), "0171", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 174
			};
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x001F6DD0 File Offset: 0x001F4FD0
		public static PIDWithFloatValueFormula PID0171_VariableGeometryTurboBPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0171_4"), "0171", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 175
			};
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x001F6E3C File Offset: 0x001F503C
		public static PIDWithFloatValueFormula PID0172_CommandedWastegateAPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0172_1"), "0172", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 176
			};
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x001F6EA8 File Offset: 0x001F50A8
		public static PIDWithFloatValueFormula PID0172_WastegateAPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0172_2"), "0172", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 177
			};
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x001F6F14 File Offset: 0x001F5114
		public static PIDWithFloatValueFormula PID0172_CommandedWastegateBPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0172_3"), "0172", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 178
			};
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x001F6F80 File Offset: 0x001F5180
		public static PIDWithFloatValueFormula PID0172_WastegateBPosition()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0172_4"), "0172", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 179
			};
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x001F6FEC File Offset: 0x001F51EC
		public static PIDWithFloatValueFormula PID0173_ExhaustPressureSensorBank1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0173_1"), "0173", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length >= 3)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.01;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 660.0,
				Id = 180
			};
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x001F7058 File Offset: 0x001F5258
		public static PIDWithFloatValueFormula PID0173_ExhaustPressureSensorBank2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0173_2"), "0173", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length >= 5)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.01;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 660.0,
				Id = 181
			};
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x001F70C4 File Offset: 0x001F52C4
		public static PIDWithFloatValueFormula PID0174_TurbochargerARPM()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0174_1"), "0174", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]);
				}
				return double.NaN;
			}, UnitsHelper.Units.rpm)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				Id = 182
			};
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x001F7130 File Offset: 0x001F5330
		public static PIDWithFloatValueFormula PID0174_TurbochargerBRPM()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0174_2"), "0174", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]);
				}
				return double.NaN;
			}, UnitsHelper.Units.rpm)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				Id = 183
			};
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x001F719C File Offset: 0x001F539C
		public static PIDWithFloatValueFormula PID0175_TurbochargerACompressorInletTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0175_1"), "0175", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 184
			};
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x001F7208 File Offset: 0x001F5408
		public static PIDWithFloatValueFormula PID0175_TurbochargerACompressorOutletTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0175_2"), "0175", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 185
			};
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x001F7274 File Offset: 0x001F5474
		public static PIDWithFloatValueFormula PID0175_TurbochargerATurbineInletTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0175_3"), "0175", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 186
			};
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x001F72E0 File Offset: 0x001F54E0
		public static PIDWithFloatValueFormula PID0175_TurbochargerATurbineOutletTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0175_4"), "0175", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 187
			};
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x001F734C File Offset: 0x001F554C
		public static PIDWithFloatValueFormula PID0176_TurbochargerBCompressorInletTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0176_1"), "0176", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 188
			};
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x001F73B8 File Offset: 0x001F55B8
		public static PIDWithFloatValueFormula PID0176_TurbochargerBCompressorOutletTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0176_2"), "0176", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 215.0,
				Id = 189
			};
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x001F7424 File Offset: 0x001F5624
		public static PIDWithFloatValueFormula PID0176_TurbochargerBTurbineInletTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0176_3"), "0176", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 190
			};
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x001F7490 File Offset: 0x001F5690
		public static PIDWithFloatValueFormula PID0176_TurbochargerBTurbineOutletTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0176_4"), "0176", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 191
			};
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x001F74FC File Offset: 0x001F56FC
		public static PIDWithFloatValueFormula PID0177_ChargeAirCoolerTemperatureBank1Sensor1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0177_1"), "0177", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 192
			};
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x001F7568 File Offset: 0x001F5768
		public static PIDWithFloatValueFormula PID0177_ChargeAirCoolerTemperatureBank1Sensor2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0177_2"), "0177", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 193
			};
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x001F75D4 File Offset: 0x001F57D4
		public static PIDWithFloatValueFormula PID0177_ChargeAirCoolerTemperatureBank2Sensor1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0177_3"), "0177", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 194
			};
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x001F7640 File Offset: 0x001F5840
		public static PIDWithFloatValueFormula PID0177_ChargeAirCoolerTemperatureBank2Sensor2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0177_4"), "0177", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] - 40);
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 195
			};
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x001F76AC File Offset: 0x001F58AC
		public static PIDWithFloatValueFormula PID0178_ExhaustGasTemperatureBank1Sensor1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0178_1"), "0178", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 196
			};
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x001F7718 File Offset: 0x001F5918
		public static PIDWithFloatValueFormula PID0178_ExhaustGasTemperatureBank1Sensor2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0178_2"), "0178", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 197
			};
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x001F7784 File Offset: 0x001F5984
		public static PIDWithFloatValueFormula PID0178_ExhaustGasTemperatureBank1Sensor3()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0178_3"), "0178", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 198
			};
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x001F77F0 File Offset: 0x001F59F0
		public static PIDWithFloatValueFormula PID0178_ExhaustGasTemperatureBank1Sensor4()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0178_4"), "0178", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 8)
				{
					return (double)((int)data[7] * 256 + (int)data[8]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 199
			};
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x001F785C File Offset: 0x001F5A5C
		public static PIDWithFloatValueFormula PID0179_ExhaustGasTemperatureBank2Sensor1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0179_1"), "0179", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 3)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 200
			};
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x001F78C8 File Offset: 0x001F5AC8
		public static PIDWithFloatValueFormula PID0179_ExhaustGasTemperatureBank2Sensor2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0179_2"), "0179", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 5)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 201
			};
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x001F7934 File Offset: 0x001F5B34
		public static PIDWithFloatValueFormula PID0179_ExhaustGasTemperatureBank2Sensor3()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0179_3"), "0179", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 7)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 202
			};
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x001F79A0 File Offset: 0x001F5BA0
		public static PIDWithFloatValueFormula PID0179_ExhaustGasTemperatureBank2Sensor4()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0179_4"), "0179", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 9)
				{
					return (double)((int)data[7] * 256 + (int)data[8]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 203
			};
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x001F7A0C File Offset: 0x001F5C0C
		public static PIDWithFloatValueFormula PID017A_DieselParticulateFilterBank1DeltaPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017A_1"), "017A", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((short)((int)data[1] * 256 + (int)data[2])) * 0.01;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = -330.0,
				Maximum = 330.0,
				Id = 204
			};
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x001F7A78 File Offset: 0x001F5C78
		public static PIDWithFloatValueFormula PID017A_DieselParticulateFilterBank1InletPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017A_2"), "017A", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.01;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 660.0,
				Id = 205
			};
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x001F7AE4 File Offset: 0x001F5CE4
		public static PIDWithFloatValueFormula PID017A_DieselParticulateFilterBank1OutletPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017A_3"), "017A", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.01;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 660.0,
				Id = 206
			};
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x001F7B50 File Offset: 0x001F5D50
		public static PIDWithFloatValueFormula PID017B_DieselParticulateFilterBank2DeltaPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017B_1"), "017B", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((short)((int)data[1] * 256 + (int)data[2])) * 0.01;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = -330.0,
				Maximum = 330.0,
				Id = 207
			};
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x001F7BBC File Offset: 0x001F5DBC
		public static PIDWithFloatValueFormula PID017B_DieselParticulateFilterBank2InletPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017B_2"), "017B", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.01;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 660.0,
				Id = 208
			};
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x001F7C28 File Offset: 0x001F5E28
		public static PIDWithFloatValueFormula PID017B_DieselParticulateFilterBank2OutletPressure()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017B_3"), "017B", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.01;
				}
				return double.NaN;
			}, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 660.0,
				Id = 209
			};
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x001F7C94 File Offset: 0x001F5E94
		public static PIDWithFloatValueFormula PID017C_DPFBank1InletTemperatureSensor()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017C_1"), "017C", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 210
			};
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x001F7D00 File Offset: 0x001F5F00
		public static PIDWithFloatValueFormula PID017C_DPFBank1OutletTemperatureSensor()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017C_2"), "017C", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 211
			};
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x001F7D6C File Offset: 0x001F5F6C
		public static PIDWithFloatValueFormula PID017C_DPFBank2InletTemperatureSensor()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017C_3"), "017C", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0,
				Id = 212
			};
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x001F7DD8 File Offset: 0x001F5FD8
		public static PIDWithFloatValueFormula PID017C_DPFBank2OutletTemperatureSensor()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_017C_4"), "017C", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 8)
				{
					return (double)((int)data[7] * 256 + (int)data[8]) * 0.1 - 40.0;
				}
				return double.NaN;
			}, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 6500.0
			};
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x001F7E38 File Offset: 0x001F6038
		public static PIDWithFloatValueFormula GetPID0908(int id)
		{
			string text = "PID_0908_" + id.ToString();
			return new PIDWithFloatValueFormula(PID.GetResourceString(text), "0908", delegate(byte[] data)
			{
				if (data.Length >= id + 2)
				{
					return (double)((int)data[id + 1] * 256 + (int)data[id + 2]);
				}
				return double.NaN;
			}, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				ShortName = PID.GetResourceString(text + "_Short"),
				HasAnnotation = true,
				Id = -10 - id
			};
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x001F7ED0 File Offset: 0x001F60D0
		public static PIDWithFloatValueFormula GetPID090B(int id)
		{
			string text = "PID_090B_" + id.ToString();
			return new PIDWithFloatValueFormula(PID.GetResourceString(text), "090B", delegate(byte[] data)
			{
				if (data.Length >= id + 2)
				{
					return (double)((int)data[id + 1] * 256 + (int)data[id + 2]);
				}
				return double.NaN;
			}, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				ShortName = PID.GetResourceString(text + "_Short"),
				HasAnnotation = true,
				Id = -65 - id
			};
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x001F7F68 File Offset: 0x001F6168
		public static PIDWithFloatValueFormula PID0183_1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0183_1"), "0183", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]);
				}
				return double.NaN;
			}, UnitsHelper.Units.ppm)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				Id = 217
			};
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x001F7FD4 File Offset: 0x001F61D4
		public static PIDWithFloatValueFormula PID0183_2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0183_2"), "0183", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]);
				}
				return double.NaN;
			}, UnitsHelper.Units.ppm)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				Id = 218
			};
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x001F8040 File Offset: 0x001F6240
		public static PIDWithFloatValueFormula PID0183_3()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0183_3"), "0183", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 4)
				{
					return (double)((int)data[5] * 256 + (int)data[6]);
				}
				return double.NaN;
			}, UnitsHelper.Units.ppm)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				Id = 650
			};
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x001F80AC File Offset: 0x001F62AC
		public static PIDWithFloatValueFormula PID0183_4()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0183_4"), "0183", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)((int)data[7] * 256 + (int)data[8]);
				}
				return double.NaN;
			}, UnitsHelper.Units.ppm)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				Id = 651
			};
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x001F8118 File Offset: 0x001F6318
		public static PIDWithFloatValueFormula PID0184_ManifoldSurfaceTemperature()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0184"), "0184", (byte[] data) => (double)(data[0] - 40), UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 652
			};
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x001F8184 File Offset: 0x001F6384
		public static PIDWithFloatValueFormula PID0185_1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0185_1"), "0185", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.005;
				}
				return double.NaN;
			}, UnitsHelper.Units.Lh)
			{
				Minimum = 0.0,
				Maximum = 300.0,
				Id = 653
			};
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x001F81F0 File Offset: 0x001F63F0
		public static PIDWithFloatValueFormula PID018D()
		{
			return new PIDWithFloatValueFormula("018D", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 654
			};
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x001F8254 File Offset: 0x001F6454
		public static PIDWithFloatValueFormula PID018E()
		{
			return new PIDWithFloatValueFormula("018E", (byte[] data) => (double)(data[0] - 125), UnitsHelper.Units.percent)
			{
				Minimum = -100.0,
				Maximum = 100.0,
				Id = 655
			};
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x001F82B8 File Offset: 0x001F64B8
		public static PIDWithFloatValueFormula PID0190_2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0190_2"), "0190", delegate(byte[] data)
			{
				if (BitHelpers.GetBit_1_8(data[0], 7))
				{
					return 1.0;
				}
				return 0.0;
			}, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Id = 656
			};
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x001F8324 File Offset: 0x001F6524
		public static PIDWithFloatValueFormula PID0190_3()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0190_3"), "0190", (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]), UnitsHelper.Units.Hours)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 657
			};
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x001F8390 File Offset: 0x001F6590
		public static PIDWithFloatValueFormula PID0191_2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0191_2"), "0191", (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]), UnitsHelper.Units.Hours)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 658
			};
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x001F83FC File Offset: 0x001F65FC
		public static PIDWithFloatValueFormula PID0191_3()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_0191_3"), "0191", (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]), UnitsHelper.Units.Hours)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 659
			};
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x001F8468 File Offset: 0x001F6668
		public static PIDWithFloatValueFormula PID019D_1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_019D_1"), "019D", (byte[] data) => (double)((int)data[0] * 256 + (int)data[1]) * 0.02, UnitsHelper.Units.grams_sec)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 660,
				Role = Roles.InstantFuelRate
			};
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x001F84DC File Offset: 0x001F66DC
		public static PIDWithFloatValueFormula PID019D_2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_019D_2"), "019D", (byte[] data) => (double)((int)data[2] * 256 + (int)data[3]) * 0.02, UnitsHelper.Units.grams_sec)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 661,
				Role = Roles.InstantFuelRate
			};
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x001F8550 File Offset: 0x001F6750
		public static PIDWithFloatValueFormula PID019E()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_019E"), "019E", (byte[] data) => (double)((int)data[0] * 256 + (int)data[1]) * 0.02, UnitsHelper.Units.kg_h)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 662
			};
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x001F85BC File Offset: 0x001F67BC
		public static PIDWithFloatValueFormula PID01A2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_01A2"), "01A2", (byte[] data) => (double)((int)data[0] * 256) + (double)data[1] * 0.03125, UnitsHelper.Units.mg_stroke)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Id = 663
			};
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x001F8628 File Offset: 0x001F6828
		public static PIDWithFloatValueFormula PID01A6()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_01A6"), "01A6", (byte[] data) => (double)((int)data[0] * 256 * 256 * 256 + (int)data[1] * 256 * 256 + (int)data[2] * 256 + (int)data[3]) * 0.1, UnitsHelper.Units.km)
			{
				Minimum = 0.0,
				Maximum = 200000.0,
				Id = 664
			};
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x001F8694 File Offset: 0x001F6894
		public static PIDWithFloatValueFormula PID_01A4_1()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_01A4_1"), "01A4", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[0] >> 4);
				}
				return double.NaN;
			}, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 300.0,
				Id = 796
			};
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x001F8700 File Offset: 0x001F6900
		public static PIDWithFloatValueFormula PID_01A4_2()
		{
			return new PIDWithFloatValueFormula(PID.GetResourceString("PID_01A4_2"), "01A4", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 3)
				{
					return (double)((int)data[2] * 256 + (int)data[3]) * 0.001;
				}
				return double.NaN;
			}, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 300.0,
				Id = 797
			};
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x001F876C File Offset: 0x001F696C
		public static PIDWithFloatValueFormula PID01AA_VehicleSpeedLimit()
		{
			return new PIDWithFloatValueFormula("01AA", (byte[] data) => (double)data[0], UnitsHelper.Units.kmh)
			{
				Minimum = 0.0,
				Maximum = 255.0,
				Name = PID.GetResourceString("PID_0164_2"),
				ShortName = PID.GetResourceString("PID_0164_2"),
				Id = 565
			};
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x001E4D06 File Offset: 0x001E2F06
		public static double Signed16Bit(byte A, byte B)
		{
			return (double)((short)((int)A * 256 + (int)B));
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x001F87EC File Offset: 0x001F69EC
		public static PIDWithFloatValueFormula PID01AF_CommandedFreshAirFlow()
		{
			return new PIDWithFloatValueFormula("01AF", (byte[] data) => (double)((int)data[0] * 256 + (int)data[1]) * 0.05, UnitsHelper.Units.kg_h)
			{
				Minimum = 0.0,
				Maximum = 3000.0,
				Name = PID.GetResourceString("PID_01AF"),
				ShortName = PID.GetResourceString("PID_01AF"),
				Id = 577
			};
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x001F8870 File Offset: 0x001F6A70
		public static PIDWithFloatValueFormula PID01B2_TractionBatteryPackPerformanceRetentionRate()
		{
			return new PIDWithFloatValueFormula("01B2", (byte[] data) => (double)data[0] * 100.0 / 255.0, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 3000.0,
				Name = PID.GetResourceString("PID_01B2"),
				ShortName = PID.GetResourceString("PID_01B2"),
				Id = 590
			};
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x001F88F4 File Offset: 0x001F6AF4
		public static PIDWithFloatValueFormula PID01B8_TimeSinceLastCellBalancing()
		{
			return new PIDWithFloatValueFormula("01B8", (byte[] data) => (double)((int)data[0] * 256 + (int)data[1]), UnitsHelper.Units.minutes)
			{
				Minimum = 0.0,
				Maximum = 65000.0,
				Name = PID.GetResourceString("PID_01B8"),
				ShortName = PID.GetResourceString("PID_01B8"),
				Id = 823
			};
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x001F8978 File Offset: 0x001F6B78
		public static PIDWithFloatValueFormula PID01B9_BatteryMinCellVoltage()
		{
			return new PIDWithFloatValueFormula("01B9", (byte[] data) => (double)((int)data[0] * 256 + (int)data[1]) * 0.001, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 5.0,
				Name = PID.GetResourceString("PID_01B9_1"),
				ShortName = PID.GetResourceString("PID_01B9_1"),
				Id = 824
			};
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x001F89FC File Offset: 0x001F6BFC
		public static PIDWithFloatValueFormula PID01B9_BatteryMaxCellVoltage()
		{
			return new PIDWithFloatValueFormula("01B9", (byte[] data) => (double)((int)data[2] * 256 + (int)data[3]) * 0.001, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 5.0,
				Name = PID.GetResourceString("PID_01B9_2"),
				ShortName = PID.GetResourceString("PID_01B9_2"),
				Id = 825
			};
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x001F8A80 File Offset: 0x001F6C80
		public static PIDWithFloatValueFormula PID01BA_1()
		{
			return new PIDWithFloatValueFormula("01BA", (byte[] data) => (double)(data[0] * 100 / byte.MaxValue), UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Name = PID.GetResourceString("PID_01BA_1"),
				ShortName = PID.GetResourceString("PID_01BA_1"),
				Id = 826
			};
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x001F8B04 File Offset: 0x001F6D04
		public static PIDWithFloatValueFormula PID01BA_2()
		{
			return new PIDWithFloatValueFormula("01BA", (byte[] data) => PIDWithFloatValueFormula.Signed16Bit(data[1], data[2]) * 0.1, UnitsHelper.Units.A)
			{
				Minimum = -3000.0,
				Maximum = 3000.0,
				Name = PID.GetResourceString("PID_01BA_2"),
				ShortName = PID.GetResourceString("PID_01BA_2"),
				Id = 827
			};
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x001F8B88 File Offset: 0x001F6D88
		public static PIDWithFloatValueFormula PID01BA_3()
		{
			return new PIDWithFloatValueFormula("01BA", (byte[] data) => PIDWithFloatValueFormula.Signed16Bit(data[3], data[4]) * 0.1, UnitsHelper.Units.A)
			{
				Minimum = -3000.0,
				Maximum = 3000.0,
				Name = PID.GetResourceString("PID_01BA_3"),
				ShortName = PID.GetResourceString("PID_01BA_3"),
				Id = 828
			};
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x001F8C0C File Offset: 0x001F6E0C
		public static PIDWithFloatValueFormula PID01C4_1()
		{
			return new PIDWithFloatValueFormula("01C4", (byte[] data) => (double)((int)data[0] * 256 * 256 * 256 + (int)data[1] * 256 * 256 + (int)data[2] * 256 + (int)data[3]), UnitsHelper.Units.seconds)
			{
				Minimum = 0.0,
				Maximum = 10000.0,
				Name = PID.GetResourceString("PID_01C4_1"),
				ShortName = PID.GetResourceString("PID_01C4_1"),
				Id = 893
			};
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x001F8C90 File Offset: 0x001F6E90
		public static PIDWithFloatValueFormula PID01C4_2()
		{
			return new PIDWithFloatValueFormula("01C4", (byte[] data) => (double)((int)data[4] * 256 * 256 * 256 + (int)data[5] * 256 * 256 + (int)data[6] * 256 + (int)data[7]), UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 10000.0,
				Name = PID.GetResourceString("PID_01C4_2"),
				ShortName = PID.GetResourceString("PID_01C4_2"),
				Id = 894
			};
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x001F8D10 File Offset: 0x001F6F10
		public static PIDWithFloatValueFormula PID0188_A0()
		{
			return new PIDWithFloatValueFormula("0188", (byte[] data) => BitHelpers.GetBit_0_7(data[0], 0) > false, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 10000.0,
				Name = PID.GetResourceString("PID_0188_A0"),
				ShortName = PID.GetResourceString("PID_0188_A0"),
				Id = 905
			};
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x001F8D90 File Offset: 0x001F6F90
		public static PIDWithFloatValueFormula PID0188_A1()
		{
			return new PIDWithFloatValueFormula("0188", (byte[] data) => BitHelpers.GetBit_0_7(data[0], 0) > false, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 10000.0,
				Name = PID.GetResourceString("PID_0188_A1"),
				ShortName = PID.GetResourceString("PID_0188_A1"),
				Id = 906
			};
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x001F8E10 File Offset: 0x001F7010
		public static PIDWithFloatValueFormula PID0188_A2()
		{
			return new PIDWithFloatValueFormula("0188", (byte[] data) => BitHelpers.GetBit_0_7(data[0], 0) > false, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 10000.0,
				Name = PID.GetResourceString("PID_0188_A2"),
				ShortName = PID.GetResourceString("PID_0188_A2"),
				Id = 907
			};
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x001F8E90 File Offset: 0x001F7090
		public static PIDWithFloatValueFormula PID0188_A3()
		{
			return new PIDWithFloatValueFormula("0188", (byte[] data) => BitHelpers.GetBit_0_7(data[0], 0) > false, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 10000.0,
				Name = PID.GetResourceString("PID_0188_A3"),
				ShortName = PID.GetResourceString("PID_0188_A3"),
				Id = 908
			};
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x001F8F10 File Offset: 0x001F7110
		public static PIDWithFloatValueFormula PID0188_A7()
		{
			return new PIDWithFloatValueFormula("0188", (byte[] data) => BitHelpers.GetBit_0_7(data[0], 0) > false, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 10000.0,
				Name = PID.GetResourceString("PID_0188_A7"),
				ShortName = PID.GetResourceString("PID_0188_A7"),
				Id = 909
			};
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x001F8F90 File Offset: 0x001F7190
		public static NC_PIDWithFloatValueFormula PID_Nissan_221101_EngineCoolantTemperature()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0105"), "221101", (byte[] data) => (double)(data[0] - 50), UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Id = 245,
				Role = Roles.Coolant
			};
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x001F9004 File Offset: 0x001F7204
		public static NC_PIDWithFloatValueFormula PID_Nissan_221102_VehicleSpeed()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010D"), "221102", (byte[] data) => (double)(data[0] * 2) * SharedSettings.Current.SpeedCorrectionFactor, UnitsHelper.Units.kmh)
			{
				Minimum = 0.0,
				Maximum = 220.0,
				Role = Roles.Speed
			};
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x001F906C File Offset: 0x001F726C
		public static NC_PIDWithFloatValueFormula PID_Nissan_221103_ControlModuleVoltage()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0142"), "221103", (byte[] data) => (double)data[0] * 0.08, UnitsHelper.Units.volts)
			{
				Minimum = 9.0,
				Maximum = 15.0
			};
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x001F90CC File Offset: 0x001F72CC
		public static NC_PIDWithFloatValueFormula PID_Nissan_221104_FuelTemperature()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221104"), "221104", (byte[] data) => (double)(data[0] - 50), UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0
			};
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x001F912C File Offset: 0x001F732C
		public static NC_PIDWithFloatValueFormula PID_Nissan_221105_EGRTemperature()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221105"), "221105", (byte[] data) => (double)data[0] * 0.02, UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0
			};
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x001F918C File Offset: 0x001F738C
		public static NC_PIDWithFloatValueFormula PID_Nissan_221106_IntakeAirTemperature()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010F"), "221106", (byte[] data) => (double)(data[0] - 50), UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0,
				Role = Roles.IAT
			};
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x001F91F4 File Offset: 0x001F73F4
		public static NC_PIDWithFloatValueFormula PID_Nissan_221107_TimingAdvance2()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010E") + " #2", "221107", (byte[] data) => (double)(50 - data[0]), UnitsHelper.Units.grads)
			{
				Minimum = -45.0,
				Maximum = 45.0
			};
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x001F9260 File Offset: 0x001F7460
		public static NC_PIDWithFloatValueFormula PID_Nissan_221108_TimingAdvance3()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010E") + " #3", "221108", (byte[] data) => (double)(70 - data[0]), UnitsHelper.Units.grads)
			{
				Minimum = -45.0,
				Maximum = 45.0
			};
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x001F92CC File Offset: 0x001F74CC
		public static NC_PIDWithFloatValueFormula PID_Nissan_221109_TimingAdvance4()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010E") + " #4", "221109", (byte[] data) => (double)(80 - data[0]), UnitsHelper.Units.grads)
			{
				Minimum = -45.0,
				Maximum = 45.0
			};
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x001F9338 File Offset: 0x001F7538
		public static NC_PIDWithFloatValueFormula PID_Nissan_22110A_TimingAdvance1()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010E") + " #1", "22110A", (byte[] data) => (double)(110 - data[0]), UnitsHelper.Units.grads)
			{
				Minimum = -45.0,
				Maximum = 45.0
			};
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x001F93A4 File Offset: 0x001F75A4
		public static NC_PIDWithFloatValueFormula PID_Nissan_22110B_IdleValve()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22110B_IdleValve"), "22110B", (byte[] data) => (double)data[0] * 0.5, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0
			};
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x001F9404 File Offset: 0x001F7604
		public static NC_PIDWithFloatValueFormula PID_Nissan_22110C_IdleValveSteps()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22110B_IdleValveSteps"), "22110C", (byte[] data) => (double)data[0] * 0.5, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 255.0
			};
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x001F9464 File Offset: 0x001F7664
		public static NC_PIDWithFloatValueFormula PID_Nissan_22110D_IdleRPMSetpoint()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22110D_IdleRPMSetpoint"), "22110D", (byte[] data) => (double)data[0] * 12.5, UnitsHelper.Units.rpm)
			{
				Minimum = 0.0,
				Maximum = 3000.0
			};
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x001F94C4 File Offset: 0x001F76C4
		public static NC_PIDWithFloatValueFormula PID_Nissan_22110E_MAPSensorVoltage()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22110E_MAPSensorVoltage"), "22110E", (byte[] data) => (double)data[0] * 0.02, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 3000.0
			};
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x001F9524 File Offset: 0x001F7724
		public static NC_PIDWithFloatValueFormula PID_Nissan_22110F_EVAPSteps()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22110F_EVAPSteps"), "22110F", (byte[] data) => (double)data[0], UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 255.0
			};
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x001F9584 File Offset: 0x001F7784
		public static NC_PIDWithFloatValueFormula PID_Nissan_221110_EVAPPercent()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221110_EVAPPercent"), "221110", (byte[] data) => (double)data[0] * 0.5, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0
			};
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x001F95E4 File Offset: 0x001F77E4
		public static NC_PIDWithFloatValueFormula PID_Nissan_221111_FuelTemperature()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221104"), "221111", (byte[] data) => (double)(data[0] - 50), UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0
			};
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x001F9644 File Offset: 0x001F7844
		public static NC_PIDWithFloatValueFormula PID_Nissan_221112_EGRSteps()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221112_EGRSteps"), "221112", (byte[] data) => (double)data[0] * 0.5, UnitsHelper.Units.None)
			{
				Minimum = 0.0,
				Maximum = 255.0
			};
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x001F96A4 File Offset: 0x001F78A4
		public static NC_PIDWithFloatValueFormula PID_Nissan_221114_FuelLevel()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_012F"), "221114", (byte[] data) => (double)data[0] * 0.04, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Role = Roles.FuelLevelInputPercent
			};
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x001F970C File Offset: 0x001F790C
		public static NC_PIDWithFloatValueFormula PID_Nissan_221117_CalculatedLoadValue()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0104"), "221117", (byte[] data) => (double)data[0] * 100.0 / 256.0, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Role = Roles.LOAD_PCT
			};
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x001F9774 File Offset: 0x001F7974
		public static NC_PIDWithFloatValueFormula PID_Nissan_221118_O2B1S1()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0114"), "221118", (byte[] data) => (double)data[0] / 100.0, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0
			};
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x001F97D4 File Offset: 0x001F79D4
		public static NC_PIDWithFloatValueFormula PID_Nissan_221119_O2B2S1()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0118"), "221119", (byte[] data) => (double)data[0] / 100.0, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0
			};
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x001F9834 File Offset: 0x001F7A34
		public static NC_PIDWithFloatValueFormula PID_Nissan_22111A_O2B1S2()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0115"), "22111A", (byte[] data) => (double)data[0] / 100.0, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0
			};
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x001F9894 File Offset: 0x001F7A94
		public static NC_PIDWithFloatValueFormula PID_Nissan_22111B_O2B2S2()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0119"), "22111B", (byte[] data) => (double)data[0] / 100.0, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 1.0
			};
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x001F98F4 File Offset: 0x001F7AF4
		public static NC_PIDWithFloatValueFormula PID_Nissan_22111C_ThrottlePosition1()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0111") + " #1", "22111C", (byte[] data) => (double)data[0] * 0.02, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x001F9968 File Offset: 0x001F7B68
		public static NC_PIDWithFloatValueFormula PID_Nissan_22111D_ThrottlePosition2()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0111") + " #2", "22111D", (byte[] data) => (double)data[0] * 0.02, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x001F99DC File Offset: 0x001F7BDC
		public static NC_PIDWithFloatValueFormula PID_Nissan_22111E_ThrottlePosition3()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0111") + " #3", "22111E", (byte[] data) => (double)data[0] * 0.035, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Role = Roles.Throttle
			};
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x001F9A50 File Offset: 0x001F7C50
		public static NC_PIDWithFloatValueFormula PID_Nissan_22111F_EngingeOilTemperature()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_015C"), "22111F", (byte[] data) => (double)(data[0] - 50), UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 150.0
			};
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x001F9AB0 File Offset: 0x001F7CB0
		public static NC_PIDWithFloatValueFormula PID_Nissan_221123_STFTBank1()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0106"), "221123", (byte[] data) => (double)(data[0] - 100), UnitsHelper.Units.percent)
			{
				Minimum = -25.0,
				Maximum = 25.0
			};
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x001F9B10 File Offset: 0x001F7D10
		public static NC_PIDWithFloatValueFormula PID_Nissan_221124_STFTBank2()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0108"), "221124", (byte[] data) => (double)(data[0] - 100), UnitsHelper.Units.percent)
			{
				Minimum = -25.0,
				Maximum = 25.0
			};
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x001F9B70 File Offset: 0x001F7D70
		public static NC_PIDWithFloatValueFormula PID_Nissan_221125_LTFTBank1()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0107"), "221125", (byte[] data) => (double)(data[0] - 100), UnitsHelper.Units.percent)
			{
				Minimum = -25.0,
				Maximum = 25.0
			};
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x001F9BD0 File Offset: 0x001F7DD0
		public static NC_PIDWithFloatValueFormula PID_Nissan_221126_LTFTBank2()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0109"), "221126", (byte[] data) => (double)(data[0] - 100), UnitsHelper.Units.percent)
			{
				Minimum = -25.0,
				Maximum = 25.0
			};
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x001F9C30 File Offset: 0x001F7E30
		public static NC_PIDWithFloatValueFormula PID_Nissan_221150_O2SensorHeaterDuty()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221150_O2SensorHeaterDuty"), "221150", (byte[] data) => (double)data[0] * 10.0, UnitsHelper.Units.percent)
			{
				Minimum = 0.0,
				Maximum = 100.0
			};
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x001F9C90 File Offset: 0x001F7E90
		public static NC_PIDWithFloatValueFormula PID_Nissan_221201_EngineRPM()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010C"), "221201", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 12.5, UnitsHelper.Units.rpm)
			{
				Minimum = 0.0,
				Maximum = 6000.0,
				Role = Roles.RPM
			};
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x001F9CF8 File Offset: 0x001F7EF8
		public static NC_PIDWithFloatValueFormula PID_Nissan_221203_DistanceWithMIL()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0121"), "221203", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data), UnitsHelper.Units.km)
			{
				Minimum = 0.0,
				Maximum = 66000.0
			};
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x001F9D58 File Offset: 0x001F7F58
		public static NC_PIDWithFloatValueFormula PID_Nissan_221204_MAFVoltageB1()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221204_MAFVoltage") + "#1", "221204", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.005, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 66000.0
			};
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x001F9DC4 File Offset: 0x001F7FC4
		public static NC_PIDWithFloatValueFormula PID_Nissan_221205_MAFVoltageB2()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221204_MAFVoltage") + "#2", "221205", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.005, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 66000.0
			};
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x001F9E30 File Offset: 0x001F8030
		public static NC_PIDWithFloatValueFormula PID_Nissan_221206_InjectionTimingB1()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221206_InjectionTiming") + "#1", "221206", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.01, UnitsHelper.Units.ms)
			{
				Minimum = 0.0,
				Maximum = 66000.0,
				Role = Roles.Injection
			};
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x001F9EA4 File Offset: 0x001F80A4
		public static NC_PIDWithFloatValueFormula PID_Nissan_221207_InjectionTimingB2()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221206_InjectionTiming") + "#2", "221207", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.01, UnitsHelper.Units.ms)
			{
				Minimum = 0.0,
				Maximum = 10.0,
				Role = Roles.Injection
			};
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x001F9F18 File Offset: 0x001F8118
		public static NC_PIDWithFloatValueFormula PID_Nissan_221208_BaseInjectionTiming()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221208_BaseInjectionTiming"), "221208", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 2048.0, UnitsHelper.Units.ms)
			{
				Minimum = 0.0,
				Maximum = 10.0,
				Role = Roles.Injection
			};
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x001F9F80 File Offset: 0x001F8180
		public static NC_PIDWithFloatValueFormula PID_Nissan_221209_MAF()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_0110"), "221209", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.01, UnitsHelper.Units.grams_sec)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Role = Roles.MAF
			};
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x001F9FE8 File Offset: 0x001F81E8
		public static NC_PIDWithFloatValueFormula PID_Nissan_22120C_FuelPressure()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010A"), "22120C", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 20.0 / 65536.0, UnitsHelper.Units.kPa)
			{
				Minimum = 0.0,
				Maximum = 100.0
			};
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x001FA048 File Offset: 0x001F8248
		public static NC_PIDWithFloatValueFormula PID_Nissan_22120D_AcceleratorPedalPositionSensor1Volts()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22120D_AcceleratorPedalPositionSensor1Volts"), "22120D", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.005, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 10.0
			};
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x001FA0A8 File Offset: 0x001F82A8
		public static NC_PIDWithFloatValueFormula PID_Nissan_22120E_AcceleratorPedalPositionSensor2Volts()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22120E_AcceleratorPedalPositionSensor2Volts"), "22120E", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.005, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 10.0
			};
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x001FA108 File Offset: 0x001F8308
		public static NC_PIDWithFloatValueFormula PID_Nissan_22120F_ThrottlePositionSensor1Volts()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22120F_ThrottlePositionSensor1Volts"), "22120F", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.005, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 10.0
			};
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x001FA168 File Offset: 0x001F8368
		public static NC_PIDWithFloatValueFormula PID_Nissan_221210_ThrottlePositionSensor2Volts()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_221210_ThrottlePositionSensor2Volts"), "221210", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) * 0.005, UnitsHelper.Units.volts)
			{
				Minimum = 0.0,
				Maximum = 10.0
			};
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x001FA1C8 File Offset: 0x001F83C8
		public static NC_PIDWithFloatValueFormula PID_Nissan_2212A1_VehicleSpeed()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_010D"), "2212A1", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 10.0, UnitsHelper.Units.kmh)
			{
				Minimum = 0.0,
				Maximum = 200.0,
				Role = Roles.Speed
			};
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x001FA230 File Offset: 0x001F8430
		public static NC_PIDWithFloatValueFormula PID_Nissan_22121B_BaseInjectionTimingLowThreshold()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22121B_BaseInjectionTimingLowThreshold"), "22121B", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 2048.0, UnitsHelper.Units.ms)
			{
				Minimum = 0.0,
				Maximum = 10.0
			};
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x001FA290 File Offset: 0x001F8490
		public static NC_PIDWithFloatValueFormula PID_Nissan_22121C_BaseInjectionTimingHighThreshold()
		{
			return new NC_PIDWithFloatValueFormula(PID.GetResourceString("PID_Nissan_22121C_BaseInjectionTimingHighThreshold"), "22121C", (byte[] data) => PIDWithFloatValueFormula.ABFormula(data) / 2048.0, UnitsHelper.Units.ms)
			{
				Minimum = 0.0,
				Maximum = 10.0
			};
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x001FA2F0 File Offset: 0x001F84F0
		// Note: this type is marked as 'beforefieldinit'.
		static PIDWithFloatValueFormula()
		{
		}

		// Token: 0x040017FE RID: 6142
		protected Func<byte[], double> Formula;

		// Token: 0x040017FF RID: 6143
		[CompilerGenerated]
		private UnitsHelper.Units <Units>k__BackingField;

		// Token: 0x04001800 RID: 6144
		private double _Value;

		// Token: 0x04001801 RID: 6145
		[CompilerGenerated]
		private static double <EquivalenceRatioScalingPerBit>k__BackingField;

		// Token: 0x04001802 RID: 6146
		[CompilerGenerated]
		private static double <OxygenSensorVoltageScalingPerBit>k__BackingField;

		// Token: 0x04001803 RID: 6147
		[CompilerGenerated]
		private static double <OxygenSensorCurrentScalingPerBit>k__BackingField;

		// Token: 0x04001804 RID: 6148
		[CompilerGenerated]
		private static double <IntakeManifoldAbsolutPressureScalingPerBit>k__BackingField;

		// Token: 0x04001805 RID: 6149
		[CompilerGenerated]
		private static double <MAFScalingPerBit>k__BackingField;

		// Token: 0x04001806 RID: 6150
		private string _TextValueVariants;

		// Token: 0x04001807 RID: 6151
		protected Dictionary<int, string> _TextValuesDict = new Dictionary<int, string>(0);

		// Token: 0x04001808 RID: 6152
		protected static Func<byte[], double> TemperatureFormula = (byte[] data) => (double)(data[0] - 40);

		// Token: 0x04001809 RID: 6153
		protected static Func<byte[], double> TrimFormula = (byte[] data) => (double)(data[0] - 128) * 100.0 / 128.0;

		// Token: 0x0400180A RID: 6154
		internal static Func<byte[], double> ABFormula = (byte[] data) => (double)((int)data[0] * 256 + (int)data[1]);

		// Token: 0x0400180B RID: 6155
		protected static Func<byte[], double> PercentFormula = (byte[] data) => (double)data[0] * 100.0 / 255.0;

		// Token: 0x0400180C RID: 6156
		private static Func<byte[], double> O2EasyFormulaVoltage = (byte[] data) => (double)((float)data[0]) * 0.005;

		// Token: 0x0400180D RID: 6157
		private static Func<byte[], double> O2EasyFormulaTrim = delegate(byte[] data)
		{
			if (data[1] == 255)
			{
				return double.NaN;
			}
			return (double)(data[1] - 128) * 100.0 / 128.0;
		};

		// Token: 0x0400180E RID: 6158
		private static Func<byte[], double> O2_WR_ABCD_Formula_EqRatio = delegate(byte[] data)
		{
			double num = (double)data[0];
			num *= 256.0;
			num += (double)data[1];
			num *= PIDWithFloatValueFormula.EquivalenceRatioScalingPerBit;
			if (SharedSettings.Current.ShowAirFuelBasedOnStoichiometric && SharedSettings.Current.FuelType == FuelTypes.Gasoline)
			{
				num *= 14.7;
			}
			return num;
		};

		// Token: 0x0400180F RID: 6159
		private static Func<byte[], double> O2_WR_ABCD_Formula_Voltage = (byte[] data) => ((double)data[2] * 256.0 + (double)data[3]) * PIDWithFloatValueFormula.OxygenSensorVoltageScalingPerBit;

		// Token: 0x04001810 RID: 6160
		private static Func<byte[], double> O2_WR_ABCD_Formula2_EqRatio = delegate(byte[] data)
		{
			double num2 = (double)data[0];
			num2 *= 256.0;
			num2 += (double)data[1];
			num2 *= PIDWithFloatValueFormula.EquivalenceRatioScalingPerBit;
			if (SharedSettings.Current.ShowAirFuelBasedOnStoichiometric && SharedSettings.Current.FuelType == FuelTypes.Gasoline)
			{
				num2 *= 14.7;
			}
			return num2;
		};

		// Token: 0x04001811 RID: 6161
		private static Func<byte[], double> O2_WR_ABCD_Formula2_Current = (byte[] data) => ((double)data[2] * 256.0 + (double)data[3] - 32768.0) * PIDWithFloatValueFormula.OxygenSensorCurrentScalingPerBit;

		// Token: 0x04001812 RID: 6162
		private static Func<byte[], double> SecondaryOxygenSensorTrimBank1And2 = (byte[] data) => (double)((data[0] - 128) * 100 / 128);

		// Token: 0x04001813 RID: 6163
		private static Func<byte[], double> SecondaryOxygenSensorTrimBank3And4 = delegate(byte[] data)
		{
			if (data.Length >= 2)
			{
				return (double)((data[1] - 128) * 100 / 128);
			}
			return double.NaN;
		};

		// Token: 0x04001814 RID: 6164
		protected const double NissanRPMMultiplier = 12.5;

		// Token: 0x0200040F RID: 1039
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002BC7 RID: 11207 RVA: 0x001FA3F9 File Offset: 0x001F85F9
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002BC8 RID: 11208 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002BC9 RID: 11209 RVA: 0x001FA405 File Offset: 0x001F8605
			internal double <PID0104_CalculatedEngineLoadValue>b__54_0(byte[] data)
			{
				return (double)(data[0] * 100) / 255.0;
			}

			// Token: 0x06002BCA RID: 11210 RVA: 0x001FA418 File Offset: 0x001F8618
			internal double <PID010A_FuelPressure>b__60_0(byte[] data)
			{
				return (double)data[0] * 3.0;
			}

			// Token: 0x06002BCB RID: 11211 RVA: 0x001FA428 File Offset: 0x001F8628
			internal double <PID010B_IntakeManifoldAbsolutePressure>b__61_0(byte[] data)
			{
				return (double)data[0] * PIDWithFloatValueFormula.IntakeManifoldAbsolutPressureScalingPerBit;
			}

			// Token: 0x06002BCC RID: 11212 RVA: 0x001FA434 File Offset: 0x001F8634
			internal double <PID010C_EngineRPM>b__62_0(byte[] data)
			{
				int num = (int)data[0] * 256;
				num += (int)data[1];
				if (!SharedSettings.Current.UseRPMFix)
				{
					num /= 4;
				}
				return (double)num;
			}

			// Token: 0x06002BCD RID: 11213 RVA: 0x001FA463 File Offset: 0x001F8663
			internal double <PID010D_VehicleSpeed>b__63_0(byte[] data)
			{
				if (SharedSettings.Current.SpeedPID2Bytes && data.Length >= 2)
				{
					return (double)((int)data[0] * 256 + (int)data[1]) * SharedSettings.Current.SpeedCorrectionFactor;
				}
				return (double)data[0] * SharedSettings.Current.SpeedCorrectionFactor;
			}

			// Token: 0x06002BCE RID: 11214 RVA: 0x001FA4A0 File Offset: 0x001F86A0
			internal double <PID010E_TimingAdvance>b__64_0(byte[] data)
			{
				return (double)((data[0] - 128) / 2);
			}

			// Token: 0x06002BCF RID: 11215 RVA: 0x001FA4AE File Offset: 0x001F86AE
			internal double <PID0110_MAFAirFlowRate>b__66_0(byte[] data)
			{
				return ((double)((int)data[0] * 256) + (double)data[1]) * PIDWithFloatValueFormula.MAFScalingPerBit;
			}

			// Token: 0x06002BD0 RID: 11216 RVA: 0x001FA4C5 File Offset: 0x001F86C5
			internal double <PID0111_ThrottlePosition>b__67_0(byte[] data)
			{
				return (double)data[0] * 100.0 / 255.0;
			}

			// Token: 0x06002BD1 RID: 11217 RVA: 0x001FA4DF File Offset: 0x001F86DF
			internal double <PID0122_FuelRailPressure_RelativeToManifoldVacuum>b__69_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.07900000363588333;
			}

			// Token: 0x06002BD2 RID: 11218 RVA: 0x001FA4F6 File Offset: 0x001F86F6
			internal double <PID0123_FuelRailPressure_DieselOrGasolineDirectInject>b__70_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 10.0;
			}

			// Token: 0x06002BD3 RID: 11219 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID012C_CommandedEGR>b__71_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BD4 RID: 11220 RVA: 0x001FA51C File Offset: 0x001F871C
			internal double <PID012D_EGRError>b__72_0(byte[] data)
			{
				return (double)((data[0] - 128) * 100 / 128);
			}

			// Token: 0x06002BD5 RID: 11221 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID012E_CommandedEvaporativePurge>b__73_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BD6 RID: 11222 RVA: 0x001FA534 File Offset: 0x001F8734
			internal double <PID012F_FuelLevelInput>b__74_0(byte[] data)
			{
				if (SharedSettings.Current.UseLitersForVolume)
				{
					double fuelTankCapacity = SharedSettings.Current.FuelTankCapacity;
				}
				else if (SharedSettings.Current.UseUSGallon)
				{
					double fuelTankCapacity2 = SharedSettings.Current.FuelTankCapacity;
				}
				else
				{
					double fuelTankCapacity3 = SharedSettings.Current.FuelTankCapacity;
				}
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BD7 RID: 11223 RVA: 0x001FA58B File Offset: 0x001F878B
			internal double <PID0130_WarmupsSinceCodesCleared>b__75_0(byte[] data)
			{
				return (double)data[0];
			}

			// Token: 0x06002BD8 RID: 11224 RVA: 0x001FA594 File Offset: 0x001F8794
			internal double <PID0132_EvapSystemVaporPressure>b__77_0(byte[] data)
			{
				byte[] array = new byte[]
				{
					data[0],
					data[1]
				};
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				return (double)BitConverter.ToInt16(array, 0) * 0.25;
			}

			// Token: 0x06002BD9 RID: 11225 RVA: 0x001FA58B File Offset: 0x001F878B
			internal double <PID0133_BarometricPressure>b__78_0(byte[] data)
			{
				return (double)data[0];
			}

			// Token: 0x06002BDA RID: 11226 RVA: 0x001FA5D3 File Offset: 0x001F87D3
			internal double <PID013C_CatalystTemperatureB1S1>b__79_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 10.0 - 40.0;
			}

			// Token: 0x06002BDB RID: 11227 RVA: 0x001FA5D3 File Offset: 0x001F87D3
			internal double <PID013D_CatalystTemperatureB2S1>b__80_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 10.0 - 40.0;
			}

			// Token: 0x06002BDC RID: 11228 RVA: 0x001FA5D3 File Offset: 0x001F87D3
			internal double <PID013E_CatalystTemperatureB1S2>b__81_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 10.0 - 40.0;
			}

			// Token: 0x06002BDD RID: 11229 RVA: 0x001FA5D3 File Offset: 0x001F87D3
			internal double <PID013F_CatalystTemperatureB2S2>b__82_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 10.0 - 40.0;
			}

			// Token: 0x06002BDE RID: 11230 RVA: 0x001FA5F4 File Offset: 0x001F87F4
			internal double <PID0142_ControlModuleVoltage>b__83_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 1000.0;
			}

			// Token: 0x06002BDF RID: 11231 RVA: 0x001FA60B File Offset: 0x001F880B
			internal double <PID0143_AbsoluteLoadValue>b__84_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 100.0 / 255.0;
			}

			// Token: 0x06002BE0 RID: 11232 RVA: 0x001FA62C File Offset: 0x001F882C
			internal double <PID0144_FuelAirCommandedEquivalenceRatio>b__85_0(byte[] data)
			{
				double num = PIDWithFloatValueFormula.ABFormula(data) * PIDWithFloatValueFormula.EquivalenceRatioScalingPerBit;
				if (SharedSettings.Current.ShowAirFuelBasedOnStoichiometric && SharedSettings.Current.FuelType == FuelTypes.Gasoline)
				{
					num *= 14.64;
				}
				return num;
			}

			// Token: 0x06002BE1 RID: 11233 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID0145_RelativeThrottlePosition>b__86_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BE2 RID: 11234 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID0147_AbsoluteThrottlePositionB>b__88_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BE3 RID: 11235 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID0148_AbsoluteThrottlePositionC>b__89_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BE4 RID: 11236 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID0149_AbsolutePedalPositionD>b__90_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BE5 RID: 11237 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID014A_AbsolutePedalPositionE>b__91_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BE6 RID: 11238 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID014B_AbsolutePedalPositionF>b__92_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BE7 RID: 11239 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID014C_CommandedThrottleActuator>b__93_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BE8 RID: 11240 RVA: 0x001FA670 File Offset: 0x001F8870
			internal double <PID0150_MaximumValueForAirFlowRateFromMassAirFlowSensor>b__94_0(byte[] data)
			{
				return (double)data[0] * 10.0 / 65535.0;
			}

			// Token: 0x06002BE9 RID: 11241 RVA: 0x001FA4C5 File Offset: 0x001F86C5
			internal double <PID0152_EthanolFuelPercent>b__95_0(byte[] data)
			{
				return (double)data[0] * 100.0 / 255.0;
			}

			// Token: 0x06002BEA RID: 11242 RVA: 0x001FA68A File Offset: 0x001F888A
			internal double <PID0153_AbsoluteEvapSystemVaporPressure>b__96_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 200.0;
			}

			// Token: 0x06002BEB RID: 11243 RVA: 0x001FA6A4 File Offset: 0x001F88A4
			internal double <PID0154_EvapSystemVaporPressure>b__97_0(byte[] data)
			{
				byte[] array = new byte[]
				{
					data[0],
					data[1]
				};
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				return (double)BitConverter.ToInt16(data, 0);
			}

			// Token: 0x06002BEC RID: 11244 RVA: 0x001FA4F6 File Offset: 0x001F86F6
			internal double <PID0159_FuelRailPressureAbsolute>b__98_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 10.0;
			}

			// Token: 0x06002BED RID: 11245 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID015A_RelativeAcceleratorPedalPosition>b__99_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BEE RID: 11246 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID015B_HybridBatteryPackRemainingLife>b__100_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002BEF RID: 11247 RVA: 0x001FA6D9 File Offset: 0x001F88D9
			internal double <PID015D_FuelInjectionTiming>b__102_0(byte[] data)
			{
				return (PIDWithFloatValueFormula.ABFormula(data) - 26880.0) / 128.0;
			}

			// Token: 0x06002BF0 RID: 11248 RVA: 0x001FA6FA File Offset: 0x001F88FA
			internal double <PID015E_EngineFuelRate>b__103_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.05000000074505806;
			}

			// Token: 0x06002BF1 RID: 11249 RVA: 0x001FA711 File Offset: 0x001F8911
			internal double <PID0161_DriversDemandEnginePercentTorque>b__104_0(byte[] data)
			{
				return (double)(data[0] - 125);
			}

			// Token: 0x06002BF2 RID: 11250 RVA: 0x001FA711 File Offset: 0x001F8911
			internal double <PID0162_ActualEnginePercentTorque>b__105_0(byte[] data)
			{
				return (double)(data[0] - 125);
			}

			// Token: 0x06002BF3 RID: 11251 RVA: 0x001FA711 File Offset: 0x001F8911
			internal double <PID0164_EnginePercentTorqueDataAtIdle>b__164_0(byte[] data)
			{
				return (double)(data[0] - 125);
			}

			// Token: 0x06002BF4 RID: 11252 RVA: 0x001FA71A File Offset: 0x001F891A
			internal double <PID0164_EnginePercentTorqueDataAtPoint2>b__165_0(byte[] data)
			{
				return (double)(data[1] - 125);
			}

			// Token: 0x06002BF5 RID: 11253 RVA: 0x001FA723 File Offset: 0x001F8923
			internal double <PID0164_EnginePercentTorqueDataAtPoint3>b__166_0(byte[] data)
			{
				return (double)(data[2] - 125);
			}

			// Token: 0x06002BF6 RID: 11254 RVA: 0x001FA72C File Offset: 0x001F892C
			internal double <PID0164_EnginePercentTorqueDataAtPoint4>b__167_0(byte[] data)
			{
				return (double)(data[3] - 125);
			}

			// Token: 0x06002BF7 RID: 11255 RVA: 0x001FA735 File Offset: 0x001F8935
			internal double <PID0164_EnginePercentTorqueDataAtPoint5>b__168_0(byte[] data)
			{
				return (double)(data[4] - 125);
			}

			// Token: 0x06002BF8 RID: 11256 RVA: 0x001FA740 File Offset: 0x001F8940
			internal double <PID0166_MAFSensorA>b__169_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.03125;
				}
				return double.NaN;
			}

			// Token: 0x06002BF9 RID: 11257 RVA: 0x001FA794 File Offset: 0x001F8994
			internal double <PID0166_MAFSensorB>b__170_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.03125;
				}
				return double.NaN;
			}

			// Token: 0x06002BFA RID: 11258 RVA: 0x001FA7E8 File Offset: 0x001F89E8
			internal double <PID0167_ECTSensorA>b__171_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002BFB RID: 11259 RVA: 0x001FA828 File Offset: 0x001F8A28
			internal double <PID0167_ECTSensorB>b__172_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002BFC RID: 11260 RVA: 0x001FA868 File Offset: 0x001F8A68
			internal double <PID0168_IATSensorB1S1>b__173_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002BFD RID: 11261 RVA: 0x001FA8A8 File Offset: 0x001F8AA8
			internal double <PID0168_IATSensorB1S2>b__174_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002BFE RID: 11262 RVA: 0x001FA8E8 File Offset: 0x001F8AE8
			internal double <PID0168_IATSensorB1S3>b__175_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002BFF RID: 11263 RVA: 0x001FA928 File Offset: 0x001F8B28
			internal double <PID0168_IATSensorB2S1>b__176_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C00 RID: 11264 RVA: 0x001FA968 File Offset: 0x001F8B68
			internal double <PID0168_IATSensorB2S2>b__177_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 5)) && data.Length > 5)
				{
					return (double)(data[5] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C01 RID: 11265 RVA: 0x001FA9A8 File Offset: 0x001F8BA8
			internal double <PID0168_IATSensorB2S3>b__178_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 6)) && data.Length > 6)
				{
					return (double)(data[6] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C02 RID: 11266 RVA: 0x001FA9E8 File Offset: 0x001F8BE8
			internal double <PID0169_CommandedEGRDutyCycleA>b__179_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C03 RID: 11267 RVA: 0x001FAA30 File Offset: 0x001F8C30
			internal double <PID0169_ActualEGRDutyCycleA>b__180_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_0_7(data[0], 1)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C04 RID: 11268 RVA: 0x001FAA78 File Offset: 0x001F8C78
			internal double <PID0169_EGRErrorA>b__181_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)((data[3] - 128) * 100 / 128);
				}
				return double.NaN;
			}

			// Token: 0x06002C05 RID: 11269 RVA: 0x001FAAC4 File Offset: 0x001F8CC4
			internal double <PID0169_CommandedEGRDutyCycleB>b__182_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C06 RID: 11270 RVA: 0x001FAB0C File Offset: 0x001F8D0C
			internal double <PID0169_ActualEGRDutyCycleB>b__183_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 5)) && data.Length > 5)
				{
					return (double)(data[5] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C07 RID: 11271 RVA: 0x001FAB54 File Offset: 0x001F8D54
			internal double <PID0169_EGRErrorB>b__184_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 6)) && data.Length > 6)
				{
					return (double)((data[6] - 128) * 100 / 128);
				}
				return double.NaN;
			}

			// Token: 0x06002C08 RID: 11272 RVA: 0x001FABA0 File Offset: 0x001F8DA0
			internal double <PID016A_CommandedIntakeAirFlowAControl>b__185_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C09 RID: 11273 RVA: 0x001FABE8 File Offset: 0x001F8DE8
			internal double <PID016A_RelativeIntakeAirFlowAPosition>b__186_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C0A RID: 11274 RVA: 0x001FAC30 File Offset: 0x001F8E30
			internal double <PID016A_CommandedIntakeAirFlowBControl>b__187_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C0B RID: 11275 RVA: 0x001FAC78 File Offset: 0x001F8E78
			internal double <PID016A_RelativeIntakeAirFlowBPosition>b__188_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C0C RID: 11276 RVA: 0x001FACC0 File Offset: 0x001F8EC0
			internal double <PID016B_ExhaustGasRecirculationTempBank1Sensor1>b__189_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C0D RID: 11277 RVA: 0x001FAD00 File Offset: 0x001F8F00
			internal double <PID016B_ExhaustGasRecirculationTempBank1Sensor2>b__190_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C0E RID: 11278 RVA: 0x001FAD40 File Offset: 0x001F8F40
			internal double <PID016B_ExhaustGasRecirculationTempBank2Sensor1>b__191_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C0F RID: 11279 RVA: 0x001FAD80 File Offset: 0x001F8F80
			internal double <PID016B_ExhaustGasRecirculationTempBank2Sensor2>b__192_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C10 RID: 11280 RVA: 0x001FADC0 File Offset: 0x001F8FC0
			internal double <PID016C_CommandedThrottleActuatorAControl>b__193_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C11 RID: 11281 RVA: 0x001FAE08 File Offset: 0x001F9008
			internal double <PID016C_RelativeThrottleAPosition>b__194_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C12 RID: 11282 RVA: 0x001FAE50 File Offset: 0x001F9050
			internal double <PID016C_CommandedThrottleActuatorBControl>b__195_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C13 RID: 11283 RVA: 0x001FAE98 File Offset: 0x001F9098
			internal double <PID016C_RelativeThrottleBPosition>b__196_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C14 RID: 11284 RVA: 0x001FAEE0 File Offset: 0x001F90E0
			internal double <PID016D_CommandedFuelRailPressure>b__197_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)(((int)data[1] * 256 + (int)data[2]) * 10);
				}
				return double.NaN;
			}

			// Token: 0x06002C15 RID: 11285 RVA: 0x001FAF2C File Offset: 0x001F912C
			internal double <PID016D_FuelRailPressure>b__198_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)(((int)data[3] * 256 + (int)data[4]) * 10);
				}
				return double.NaN;
			}

			// Token: 0x06002C16 RID: 11286 RVA: 0x001FAF78 File Offset: 0x001F9178
			internal double <PID016D_FuelRailTemperature>b__199_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 5)
				{
					return (double)(data[5] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C17 RID: 11287 RVA: 0x001FAFB8 File Offset: 0x001F91B8
			internal double <PID016E_CommandedInjectionControlPressure>b__200_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)(((int)data[1] * 256 + (int)data[2]) * 10);
				}
				return double.NaN;
			}

			// Token: 0x06002C18 RID: 11288 RVA: 0x001FB004 File Offset: 0x001F9204
			internal double <PID016E_InjectionControlPressure>b__201_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)(((int)data[3] * 256 + (int)data[4]) * 10);
				}
				return double.NaN;
			}

			// Token: 0x06002C19 RID: 11289 RVA: 0x001FB050 File Offset: 0x001F9250
			internal double <PID016F_TurbochargerCompressorInletPressureSensorA>b__202_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)data[1];
				}
				return double.NaN;
			}

			// Token: 0x06002C1A RID: 11290 RVA: 0x001FB090 File Offset: 0x001F9290
			internal double <PID016F_TurbochargerCompressorInletPressureSensorB>b__203_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)data[2];
				}
				return double.NaN;
			}

			// Token: 0x06002C1B RID: 11291 RVA: 0x001FB0D0 File Offset: 0x001F92D0
			internal double <PID0170_CommandedBoostPressureA>b__204_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.03125;
				}
				return double.NaN;
			}

			// Token: 0x06002C1C RID: 11292 RVA: 0x001FB124 File Offset: 0x001F9324
			internal double <PID0170_BoostPressureSensorA>b__205_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.03125;
				}
				return double.NaN;
			}

			// Token: 0x06002C1D RID: 11293 RVA: 0x001FB178 File Offset: 0x001F9378
			internal double <PID0170_CommandedBoostPressureB>b__206_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.03125;
				}
				return double.NaN;
			}

			// Token: 0x06002C1E RID: 11294 RVA: 0x001FB1CC File Offset: 0x001F93CC
			internal double <PID0170_BoostPressureSensorB>b__207_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 8)
				{
					return (double)((int)data[7] * 256 + (int)data[8]) * 0.03125;
				}
				return double.NaN;
			}

			// Token: 0x06002C1F RID: 11295 RVA: 0x001FB220 File Offset: 0x001F9420
			internal double <PID0171_CommandedVariableGeometryTurboAPosition>b__208_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C20 RID: 11296 RVA: 0x001FB268 File Offset: 0x001F9468
			internal double <PID0171_VariableGeometryTurboAPosition>b__209_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C21 RID: 11297 RVA: 0x001FB2B0 File Offset: 0x001F94B0
			internal double <PID0171_CommandedVariableGeometryTurboBPosition>b__210_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C22 RID: 11298 RVA: 0x001FB2F8 File Offset: 0x001F94F8
			internal double <PID0171_VariableGeometryTurboBPosition>b__211_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C23 RID: 11299 RVA: 0x001FB340 File Offset: 0x001F9540
			internal double <PID0172_CommandedWastegateAPosition>b__212_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C24 RID: 11300 RVA: 0x001FB388 File Offset: 0x001F9588
			internal double <PID0172_WastegateAPosition>b__213_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C25 RID: 11301 RVA: 0x001FB3D0 File Offset: 0x001F95D0
			internal double <PID0172_CommandedWastegateBPosition>b__214_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C26 RID: 11302 RVA: 0x001FB418 File Offset: 0x001F9618
			internal double <PID0172_WastegateBPosition>b__215_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] * 100 / byte.MaxValue);
				}
				return double.NaN;
			}

			// Token: 0x06002C27 RID: 11303 RVA: 0x001FB460 File Offset: 0x001F9660
			internal double <PID0173_ExhaustPressureSensorBank1>b__216_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length >= 3)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.01;
				}
				return double.NaN;
			}

			// Token: 0x06002C28 RID: 11304 RVA: 0x001FB4B4 File Offset: 0x001F96B4
			internal double <PID0173_ExhaustPressureSensorBank2>b__217_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length >= 5)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.01;
				}
				return double.NaN;
			}

			// Token: 0x06002C29 RID: 11305 RVA: 0x001FB508 File Offset: 0x001F9708
			internal double <PID0174_TurbochargerARPM>b__218_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]);
				}
				return double.NaN;
			}

			// Token: 0x06002C2A RID: 11306 RVA: 0x001FB550 File Offset: 0x001F9750
			internal double <PID0174_TurbochargerBRPM>b__219_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]);
				}
				return double.NaN;
			}

			// Token: 0x06002C2B RID: 11307 RVA: 0x001FB598 File Offset: 0x001F9798
			internal double <PID0175_TurbochargerACompressorInletTemperature>b__220_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C2C RID: 11308 RVA: 0x001FB5D8 File Offset: 0x001F97D8
			internal double <PID0175_TurbochargerACompressorOutletTemperature>b__221_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C2D RID: 11309 RVA: 0x001FB618 File Offset: 0x001F9818
			internal double <PID0175_TurbochargerATurbineInletTemperature>b__222_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C2E RID: 11310 RVA: 0x001FB674 File Offset: 0x001F9874
			internal double <PID0175_TurbochargerATurbineOutletTemperature>b__223_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C2F RID: 11311 RVA: 0x001FB6D0 File Offset: 0x001F98D0
			internal double <PID0176_TurbochargerBCompressorInletTemperature>b__224_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C30 RID: 11312 RVA: 0x001FB710 File Offset: 0x001F9910
			internal double <PID0176_TurbochargerBCompressorOutletTemperature>b__225_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C31 RID: 11313 RVA: 0x001FB750 File Offset: 0x001F9950
			internal double <PID0176_TurbochargerBTurbineInletTemperature>b__226_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C32 RID: 11314 RVA: 0x001FB7AC File Offset: 0x001F99AC
			internal double <PID0176_TurbochargerBTurbineOutletTemperature>b__227_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C33 RID: 11315 RVA: 0x001FB808 File Offset: 0x001F9A08
			internal double <PID0177_ChargeAirCoolerTemperatureBank1Sensor1>b__228_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[1] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C34 RID: 11316 RVA: 0x001FB848 File Offset: 0x001F9A48
			internal double <PID0177_ChargeAirCoolerTemperatureBank1Sensor2>b__229_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 2)
				{
					return (double)(data[2] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C35 RID: 11317 RVA: 0x001FB888 File Offset: 0x001F9A88
			internal double <PID0177_ChargeAirCoolerTemperatureBank2Sensor1>b__230_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 3)
				{
					return (double)(data[3] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C36 RID: 11318 RVA: 0x001FB8C8 File Offset: 0x001F9AC8
			internal double <PID0177_ChargeAirCoolerTemperatureBank2Sensor2>b__231_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)(data[4] - 40);
				}
				return double.NaN;
			}

			// Token: 0x06002C37 RID: 11319 RVA: 0x001FB908 File Offset: 0x001F9B08
			internal double <PID0178_ExhaustGasTemperatureBank1Sensor1>b__232_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C38 RID: 11320 RVA: 0x001FB964 File Offset: 0x001F9B64
			internal double <PID0178_ExhaustGasTemperatureBank1Sensor2>b__233_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C39 RID: 11321 RVA: 0x001FB9C0 File Offset: 0x001F9BC0
			internal double <PID0178_ExhaustGasTemperatureBank1Sensor3>b__234_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C3A RID: 11322 RVA: 0x001FBA1C File Offset: 0x001F9C1C
			internal double <PID0178_ExhaustGasTemperatureBank1Sensor4>b__235_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 8)
				{
					return (double)((int)data[7] * 256 + (int)data[8]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C3B RID: 11323 RVA: 0x001FBA78 File Offset: 0x001F9C78
			internal double <PID0179_ExhaustGasTemperatureBank2Sensor1>b__236_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 3)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C3C RID: 11324 RVA: 0x001FBAD4 File Offset: 0x001F9CD4
			internal double <PID0179_ExhaustGasTemperatureBank2Sensor2>b__237_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 5)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C3D RID: 11325 RVA: 0x001FBB30 File Offset: 0x001F9D30
			internal double <PID0179_ExhaustGasTemperatureBank2Sensor3>b__238_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 7)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C3E RID: 11326 RVA: 0x001FBB8C File Offset: 0x001F9D8C
			internal double <PID0179_ExhaustGasTemperatureBank2Sensor4>b__239_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 9)
				{
					return (double)((int)data[7] * 256 + (int)data[8]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C3F RID: 11327 RVA: 0x001FBBE8 File Offset: 0x001F9DE8
			internal double <PID017A_DieselParticulateFilterBank1DeltaPressure>b__240_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((short)((int)data[1] * 256 + (int)data[2])) * 0.01;
				}
				return double.NaN;
			}

			// Token: 0x06002C40 RID: 11328 RVA: 0x001FBC3C File Offset: 0x001F9E3C
			internal double <PID017A_DieselParticulateFilterBank1InletPressure>b__241_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.01;
				}
				return double.NaN;
			}

			// Token: 0x06002C41 RID: 11329 RVA: 0x001FBC90 File Offset: 0x001F9E90
			internal double <PID017A_DieselParticulateFilterBank1OutletPressure>b__242_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.01;
				}
				return double.NaN;
			}

			// Token: 0x06002C42 RID: 11330 RVA: 0x001FBCE4 File Offset: 0x001F9EE4
			internal double <PID017B_DieselParticulateFilterBank2DeltaPressure>b__243_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((short)((int)data[1] * 256 + (int)data[2])) * 0.01;
				}
				return double.NaN;
			}

			// Token: 0x06002C43 RID: 11331 RVA: 0x001FBD38 File Offset: 0x001F9F38
			internal double <PID017B_DieselParticulateFilterBank2InletPressure>b__244_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.01;
				}
				return double.NaN;
			}

			// Token: 0x06002C44 RID: 11332 RVA: 0x001FBD8C File Offset: 0x001F9F8C
			internal double <PID017B_DieselParticulateFilterBank2OutletPressure>b__245_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.01;
				}
				return double.NaN;
			}

			// Token: 0x06002C45 RID: 11333 RVA: 0x001FBDE0 File Offset: 0x001F9FE0
			internal double <PID017C_DPFBank1InletTemperatureSensor>b__246_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C46 RID: 11334 RVA: 0x001FBE3C File Offset: 0x001FA03C
			internal double <PID017C_DPFBank1OutletTemperatureSensor>b__247_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C47 RID: 11335 RVA: 0x001FBE98 File Offset: 0x001FA098
			internal double <PID017C_DPFBank2InletTemperatureSensor>b__248_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 6)
				{
					return (double)((int)data[5] * 256 + (int)data[6]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C48 RID: 11336 RVA: 0x001FBEF4 File Offset: 0x001FA0F4
			internal double <PID017C_DPFBank2OutletTemperatureSensor>b__249_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 8)
				{
					return (double)((int)data[7] * 256 + (int)data[8]) * 0.1 - 40.0;
				}
				return double.NaN;
			}

			// Token: 0x06002C49 RID: 11337 RVA: 0x001FBF50 File Offset: 0x001FA150
			internal double <PID0183_1>b__252_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]);
				}
				return double.NaN;
			}

			// Token: 0x06002C4A RID: 11338 RVA: 0x001FBF98 File Offset: 0x001FA198
			internal double <PID0183_2>b__253_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 4)
				{
					return (double)((int)data[3] * 256 + (int)data[4]);
				}
				return double.NaN;
			}

			// Token: 0x06002C4B RID: 11339 RVA: 0x001FBFE0 File Offset: 0x001FA1E0
			internal double <PID0183_3>b__254_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3)) && data.Length > 4)
				{
					return (double)((int)data[5] * 256 + (int)data[6]);
				}
				return double.NaN;
			}

			// Token: 0x06002C4C RID: 11340 RVA: 0x001FC028 File Offset: 0x001FA228
			internal double <PID0183_4>b__255_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 4)) && data.Length > 4)
				{
					return (double)((int)data[7] * 256 + (int)data[8]);
				}
				return double.NaN;
			}

			// Token: 0x06002C4D RID: 11341 RVA: 0x001FC06F File Offset: 0x001FA26F
			internal double <PID0184_ManifoldSurfaceTemperature>b__256_0(byte[] data)
			{
				return (double)(data[0] - 40);
			}

			// Token: 0x06002C4E RID: 11342 RVA: 0x001FC078 File Offset: 0x001FA278
			internal double <PID0185_1>b__257_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 2)
				{
					return (double)((int)data[1] * 256 + (int)data[2]) * 0.005;
				}
				return double.NaN;
			}

			// Token: 0x06002C4F RID: 11343 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID018D>b__258_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002C50 RID: 11344 RVA: 0x001FA711 File Offset: 0x001F8911
			internal double <PID018E>b__259_0(byte[] data)
			{
				return (double)(data[0] - 125);
			}

			// Token: 0x06002C51 RID: 11345 RVA: 0x001FC0C9 File Offset: 0x001FA2C9
			internal double <PID0190_2>b__260_0(byte[] data)
			{
				if (BitHelpers.GetBit_1_8(data[0], 7))
				{
					return 1.0;
				}
				return 0.0;
			}

			// Token: 0x06002C52 RID: 11346 RVA: 0x001F0E7C File Offset: 0x001EF07C
			internal double <PID0190_3>b__261_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]);
			}

			// Token: 0x06002C53 RID: 11347 RVA: 0x001F0E7C File Offset: 0x001EF07C
			internal double <PID0191_2>b__262_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]);
			}

			// Token: 0x06002C54 RID: 11348 RVA: 0x001F0E7C File Offset: 0x001EF07C
			internal double <PID0191_3>b__263_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]);
			}

			// Token: 0x06002C55 RID: 11349 RVA: 0x001FC0E9 File Offset: 0x001FA2E9
			internal double <PID019D_1>b__264_0(byte[] data)
			{
				return (double)((int)data[0] * 256 + (int)data[1]) * 0.02;
			}

			// Token: 0x06002C56 RID: 11350 RVA: 0x001FC103 File Offset: 0x001FA303
			internal double <PID019D_2>b__265_0(byte[] data)
			{
				return (double)((int)data[2] * 256 + (int)data[3]) * 0.02;
			}

			// Token: 0x06002C57 RID: 11351 RVA: 0x001FC0E9 File Offset: 0x001FA2E9
			internal double <PID019E>b__266_0(byte[] data)
			{
				return (double)((int)data[0] * 256 + (int)data[1]) * 0.02;
			}

			// Token: 0x06002C58 RID: 11352 RVA: 0x001FC11D File Offset: 0x001FA31D
			internal double <PID01A2>b__267_0(byte[] data)
			{
				return (double)((int)data[0] * 256) + (double)data[1] * 0.03125;
			}

			// Token: 0x06002C59 RID: 11353 RVA: 0x001FC138 File Offset: 0x001FA338
			internal double <PID01A6>b__268_0(byte[] data)
			{
				return (double)((int)data[0] * 256 * 256 * 256 + (int)data[1] * 256 * 256 + (int)data[2] * 256 + (int)data[3]) * 0.1;
			}

			// Token: 0x06002C5A RID: 11354 RVA: 0x001FC178 File Offset: 0x001FA378
			internal double <PID_01A4_1>b__269_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length > 1)
				{
					return (double)(data[0] >> 4);
				}
				return double.NaN;
			}

			// Token: 0x06002C5B RID: 11355 RVA: 0x001FC1B8 File Offset: 0x001FA3B8
			internal double <PID_01A4_2>b__270_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2)) && data.Length > 3)
				{
					return (double)((int)data[2] * 256 + (int)data[3]) * 0.001;
				}
				return double.NaN;
			}

			// Token: 0x06002C5C RID: 11356 RVA: 0x001FA58B File Offset: 0x001F878B
			internal double <PID01AA_VehicleSpeedLimit>b__271_0(byte[] data)
			{
				return (double)data[0];
			}

			// Token: 0x06002C5D RID: 11357 RVA: 0x001FC209 File Offset: 0x001FA409
			internal double <PID01AF_CommandedFreshAirFlow>b__273_0(byte[] data)
			{
				return (double)((int)data[0] * 256 + (int)data[1]) * 0.05;
			}

			// Token: 0x06002C5E RID: 11358 RVA: 0x001FA4C5 File Offset: 0x001F86C5
			internal double <PID01B2_TractionBatteryPackPerformanceRetentionRate>b__274_0(byte[] data)
			{
				return (double)data[0] * 100.0 / 255.0;
			}

			// Token: 0x06002C5F RID: 11359 RVA: 0x001FC223 File Offset: 0x001FA423
			internal double <PID01B8_TimeSinceLastCellBalancing>b__275_0(byte[] data)
			{
				return (double)((int)data[0] * 256 + (int)data[1]);
			}

			// Token: 0x06002C60 RID: 11360 RVA: 0x001FC233 File Offset: 0x001FA433
			internal double <PID01B9_BatteryMinCellVoltage>b__276_0(byte[] data)
			{
				return (double)((int)data[0] * 256 + (int)data[1]) * 0.001;
			}

			// Token: 0x06002C61 RID: 11361 RVA: 0x001FC24D File Offset: 0x001FA44D
			internal double <PID01B9_BatteryMaxCellVoltage>b__277_0(byte[] data)
			{
				return (double)((int)data[2] * 256 + (int)data[3]) * 0.001;
			}

			// Token: 0x06002C62 RID: 11362 RVA: 0x001FA50D File Offset: 0x001F870D
			internal double <PID01BA_1>b__278_0(byte[] data)
			{
				return (double)(data[0] * 100 / byte.MaxValue);
			}

			// Token: 0x06002C63 RID: 11363 RVA: 0x001FC267 File Offset: 0x001FA467
			internal double <PID01BA_2>b__279_0(byte[] data)
			{
				return PIDWithFloatValueFormula.Signed16Bit(data[1], data[2]) * 0.1;
			}

			// Token: 0x06002C64 RID: 11364 RVA: 0x001FC27E File Offset: 0x001FA47E
			internal double <PID01BA_3>b__280_0(byte[] data)
			{
				return PIDWithFloatValueFormula.Signed16Bit(data[3], data[4]) * 0.1;
			}

			// Token: 0x06002C65 RID: 11365 RVA: 0x001FC295 File Offset: 0x001FA495
			internal double <PID01C4_1>b__281_0(byte[] data)
			{
				return (double)((int)data[0] * 256 * 256 * 256 + (int)data[1] * 256 * 256 + (int)data[2] * 256 + (int)data[3]);
			}

			// Token: 0x06002C66 RID: 11366 RVA: 0x001FC2CB File Offset: 0x001FA4CB
			internal double <PID01C4_2>b__282_0(byte[] data)
			{
				return (double)((int)data[4] * 256 * 256 * 256 + (int)data[5] * 256 * 256 + (int)data[6] * 256 + (int)data[7]);
			}

			// Token: 0x06002C67 RID: 11367 RVA: 0x001FC301 File Offset: 0x001FA501
			internal double <PID0188_A0>b__283_0(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[0], 0) > false;
			}

			// Token: 0x06002C68 RID: 11368 RVA: 0x001FC301 File Offset: 0x001FA501
			internal double <PID0188_A1>b__284_0(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[0], 0) > false;
			}

			// Token: 0x06002C69 RID: 11369 RVA: 0x001FC301 File Offset: 0x001FA501
			internal double <PID0188_A2>b__285_0(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[0], 0) > false;
			}

			// Token: 0x06002C6A RID: 11370 RVA: 0x001FC301 File Offset: 0x001FA501
			internal double <PID0188_A3>b__286_0(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[0], 0) > false;
			}

			// Token: 0x06002C6B RID: 11371 RVA: 0x001FC301 File Offset: 0x001FA501
			internal double <PID0188_A7>b__287_0(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[0], 0) > false;
			}

			// Token: 0x06002C6C RID: 11372 RVA: 0x001FC310 File Offset: 0x001FA510
			internal double <PID_Nissan_221101_EngineCoolantTemperature>b__289_0(byte[] data)
			{
				return (double)(data[0] - 50);
			}

			// Token: 0x06002C6D RID: 11373 RVA: 0x001FC319 File Offset: 0x001FA519
			internal double <PID_Nissan_221102_VehicleSpeed>b__290_0(byte[] data)
			{
				return (double)(data[0] * 2) * SharedSettings.Current.SpeedCorrectionFactor;
			}

			// Token: 0x06002C6E RID: 11374 RVA: 0x001FC32C File Offset: 0x001FA52C
			internal double <PID_Nissan_221103_ControlModuleVoltage>b__291_0(byte[] data)
			{
				return (double)data[0] * 0.08;
			}

			// Token: 0x06002C6F RID: 11375 RVA: 0x001FC310 File Offset: 0x001FA510
			internal double <PID_Nissan_221104_FuelTemperature>b__292_0(byte[] data)
			{
				return (double)(data[0] - 50);
			}

			// Token: 0x06002C70 RID: 11376 RVA: 0x001FC33C File Offset: 0x001FA53C
			internal double <PID_Nissan_221105_EGRTemperature>b__293_0(byte[] data)
			{
				return (double)data[0] * 0.02;
			}

			// Token: 0x06002C71 RID: 11377 RVA: 0x001FC310 File Offset: 0x001FA510
			internal double <PID_Nissan_221106_IntakeAirTemperature>b__294_0(byte[] data)
			{
				return (double)(data[0] - 50);
			}

			// Token: 0x06002C72 RID: 11378 RVA: 0x001FC34C File Offset: 0x001FA54C
			internal double <PID_Nissan_221107_TimingAdvance2>b__295_0(byte[] data)
			{
				return (double)(50 - data[0]);
			}

			// Token: 0x06002C73 RID: 11379 RVA: 0x001FC355 File Offset: 0x001FA555
			internal double <PID_Nissan_221108_TimingAdvance3>b__296_0(byte[] data)
			{
				return (double)(70 - data[0]);
			}

			// Token: 0x06002C74 RID: 11380 RVA: 0x001FC35E File Offset: 0x001FA55E
			internal double <PID_Nissan_221109_TimingAdvance4>b__297_0(byte[] data)
			{
				return (double)(80 - data[0]);
			}

			// Token: 0x06002C75 RID: 11381 RVA: 0x001FC367 File Offset: 0x001FA567
			internal double <PID_Nissan_22110A_TimingAdvance1>b__298_0(byte[] data)
			{
				return (double)(110 - data[0]);
			}

			// Token: 0x06002C76 RID: 11382 RVA: 0x001FC370 File Offset: 0x001FA570
			internal double <PID_Nissan_22110B_IdleValve>b__299_0(byte[] data)
			{
				return (double)data[0] * 0.5;
			}

			// Token: 0x06002C77 RID: 11383 RVA: 0x001FC370 File Offset: 0x001FA570
			internal double <PID_Nissan_22110C_IdleValveSteps>b__300_0(byte[] data)
			{
				return (double)data[0] * 0.5;
			}

			// Token: 0x06002C78 RID: 11384 RVA: 0x001FC380 File Offset: 0x001FA580
			internal double <PID_Nissan_22110D_IdleRPMSetpoint>b__301_0(byte[] data)
			{
				return (double)data[0] * 12.5;
			}

			// Token: 0x06002C79 RID: 11385 RVA: 0x001FC33C File Offset: 0x001FA53C
			internal double <PID_Nissan_22110E_MAPSensorVoltage>b__302_0(byte[] data)
			{
				return (double)data[0] * 0.02;
			}

			// Token: 0x06002C7A RID: 11386 RVA: 0x001FA58B File Offset: 0x001F878B
			internal double <PID_Nissan_22110F_EVAPSteps>b__303_0(byte[] data)
			{
				return (double)data[0];
			}

			// Token: 0x06002C7B RID: 11387 RVA: 0x001FC370 File Offset: 0x001FA570
			internal double <PID_Nissan_221110_EVAPPercent>b__304_0(byte[] data)
			{
				return (double)data[0] * 0.5;
			}

			// Token: 0x06002C7C RID: 11388 RVA: 0x001FC310 File Offset: 0x001FA510
			internal double <PID_Nissan_221111_FuelTemperature>b__305_0(byte[] data)
			{
				return (double)(data[0] - 50);
			}

			// Token: 0x06002C7D RID: 11389 RVA: 0x001FC370 File Offset: 0x001FA570
			internal double <PID_Nissan_221112_EGRSteps>b__306_0(byte[] data)
			{
				return (double)data[0] * 0.5;
			}

			// Token: 0x06002C7E RID: 11390 RVA: 0x001FC390 File Offset: 0x001FA590
			internal double <PID_Nissan_221114_FuelLevel>b__307_0(byte[] data)
			{
				return (double)data[0] * 0.04;
			}

			// Token: 0x06002C7F RID: 11391 RVA: 0x001FC3A0 File Offset: 0x001FA5A0
			internal double <PID_Nissan_221117_CalculatedLoadValue>b__308_0(byte[] data)
			{
				return (double)data[0] * 100.0 / 256.0;
			}

			// Token: 0x06002C80 RID: 11392 RVA: 0x001FC3BA File Offset: 0x001FA5BA
			internal double <PID_Nissan_221118_O2B1S1>b__309_0(byte[] data)
			{
				return (double)data[0] / 100.0;
			}

			// Token: 0x06002C81 RID: 11393 RVA: 0x001FC3BA File Offset: 0x001FA5BA
			internal double <PID_Nissan_221119_O2B2S1>b__310_0(byte[] data)
			{
				return (double)data[0] / 100.0;
			}

			// Token: 0x06002C82 RID: 11394 RVA: 0x001FC3BA File Offset: 0x001FA5BA
			internal double <PID_Nissan_22111A_O2B1S2>b__311_0(byte[] data)
			{
				return (double)data[0] / 100.0;
			}

			// Token: 0x06002C83 RID: 11395 RVA: 0x001FC3BA File Offset: 0x001FA5BA
			internal double <PID_Nissan_22111B_O2B2S2>b__312_0(byte[] data)
			{
				return (double)data[0] / 100.0;
			}

			// Token: 0x06002C84 RID: 11396 RVA: 0x001FC33C File Offset: 0x001FA53C
			internal double <PID_Nissan_22111C_ThrottlePosition1>b__313_0(byte[] data)
			{
				return (double)data[0] * 0.02;
			}

			// Token: 0x06002C85 RID: 11397 RVA: 0x001FC33C File Offset: 0x001FA53C
			internal double <PID_Nissan_22111D_ThrottlePosition2>b__314_0(byte[] data)
			{
				return (double)data[0] * 0.02;
			}

			// Token: 0x06002C86 RID: 11398 RVA: 0x001FC3CA File Offset: 0x001FA5CA
			internal double <PID_Nissan_22111E_ThrottlePosition3>b__315_0(byte[] data)
			{
				return (double)data[0] * 0.035;
			}

			// Token: 0x06002C87 RID: 11399 RVA: 0x001FC310 File Offset: 0x001FA510
			internal double <PID_Nissan_22111F_EngingeOilTemperature>b__316_0(byte[] data)
			{
				return (double)(data[0] - 50);
			}

			// Token: 0x06002C88 RID: 11400 RVA: 0x001FC3DA File Offset: 0x001FA5DA
			internal double <PID_Nissan_221123_STFTBank1>b__317_0(byte[] data)
			{
				return (double)(data[0] - 100);
			}

			// Token: 0x06002C89 RID: 11401 RVA: 0x001FC3DA File Offset: 0x001FA5DA
			internal double <PID_Nissan_221124_STFTBank2>b__318_0(byte[] data)
			{
				return (double)(data[0] - 100);
			}

			// Token: 0x06002C8A RID: 11402 RVA: 0x001FC3DA File Offset: 0x001FA5DA
			internal double <PID_Nissan_221125_LTFTBank1>b__319_0(byte[] data)
			{
				return (double)(data[0] - 100);
			}

			// Token: 0x06002C8B RID: 11403 RVA: 0x001FC3DA File Offset: 0x001FA5DA
			internal double <PID_Nissan_221126_LTFTBank2>b__320_0(byte[] data)
			{
				return (double)(data[0] - 100);
			}

			// Token: 0x06002C8C RID: 11404 RVA: 0x001FC3E3 File Offset: 0x001FA5E3
			internal double <PID_Nissan_221150_O2SensorHeaterDuty>b__321_0(byte[] data)
			{
				return (double)data[0] * 10.0;
			}

			// Token: 0x06002C8D RID: 11405 RVA: 0x001FC3F3 File Offset: 0x001FA5F3
			internal double <PID_Nissan_221201_EngineRPM>b__322_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 12.5;
			}

			// Token: 0x06002C8E RID: 11406 RVA: 0x001FC40A File Offset: 0x001FA60A
			internal double <PID_Nissan_221203_DistanceWithMIL>b__323_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data);
			}

			// Token: 0x06002C8F RID: 11407 RVA: 0x001FC417 File Offset: 0x001FA617
			internal double <PID_Nissan_221204_MAFVoltageB1>b__324_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.005;
			}

			// Token: 0x06002C90 RID: 11408 RVA: 0x001FC417 File Offset: 0x001FA617
			internal double <PID_Nissan_221205_MAFVoltageB2>b__325_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.005;
			}

			// Token: 0x06002C91 RID: 11409 RVA: 0x001FC42E File Offset: 0x001FA62E
			internal double <PID_Nissan_221206_InjectionTimingB1>b__326_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.01;
			}

			// Token: 0x06002C92 RID: 11410 RVA: 0x001FC42E File Offset: 0x001FA62E
			internal double <PID_Nissan_221207_InjectionTimingB2>b__327_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.01;
			}

			// Token: 0x06002C93 RID: 11411 RVA: 0x001FC445 File Offset: 0x001FA645
			internal double <PID_Nissan_221208_BaseInjectionTiming>b__328_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 2048.0;
			}

			// Token: 0x06002C94 RID: 11412 RVA: 0x001FC42E File Offset: 0x001FA62E
			internal double <PID_Nissan_221209_MAF>b__329_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.01;
			}

			// Token: 0x06002C95 RID: 11413 RVA: 0x001FC45C File Offset: 0x001FA65C
			internal double <PID_Nissan_22120C_FuelPressure>b__330_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 20.0 / 65536.0;
			}

			// Token: 0x06002C96 RID: 11414 RVA: 0x001FC417 File Offset: 0x001FA617
			internal double <PID_Nissan_22120D_AcceleratorPedalPositionSensor1Volts>b__331_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.005;
			}

			// Token: 0x06002C97 RID: 11415 RVA: 0x001FC417 File Offset: 0x001FA617
			internal double <PID_Nissan_22120E_AcceleratorPedalPositionSensor2Volts>b__332_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.005;
			}

			// Token: 0x06002C98 RID: 11416 RVA: 0x001FC417 File Offset: 0x001FA617
			internal double <PID_Nissan_22120F_ThrottlePositionSensor1Volts>b__333_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.005;
			}

			// Token: 0x06002C99 RID: 11417 RVA: 0x001FC417 File Offset: 0x001FA617
			internal double <PID_Nissan_221210_ThrottlePositionSensor2Volts>b__334_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) * 0.005;
			}

			// Token: 0x06002C9A RID: 11418 RVA: 0x001FC47D File Offset: 0x001FA67D
			internal double <PID_Nissan_2212A1_VehicleSpeed>b__335_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 10.0;
			}

			// Token: 0x06002C9B RID: 11419 RVA: 0x001FC445 File Offset: 0x001FA645
			internal double <PID_Nissan_22121B_BaseInjectionTimingLowThreshold>b__336_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 2048.0;
			}

			// Token: 0x06002C9C RID: 11420 RVA: 0x001FC445 File Offset: 0x001FA645
			internal double <PID_Nissan_22121C_BaseInjectionTimingHighThreshold>b__337_0(byte[] data)
			{
				return PIDWithFloatValueFormula.ABFormula(data) / 2048.0;
			}

			// Token: 0x06002C9D RID: 11421 RVA: 0x001FC06F File Offset: 0x001FA26F
			internal double <.cctor>b__338_0(byte[] data)
			{
				return (double)(data[0] - 40);
			}

			// Token: 0x06002C9E RID: 11422 RVA: 0x001FC494 File Offset: 0x001FA694
			internal double <.cctor>b__338_1(byte[] data)
			{
				return (double)(data[0] - 128) * 100.0 / 128.0;
			}

			// Token: 0x06002C9F RID: 11423 RVA: 0x001FC223 File Offset: 0x001FA423
			internal double <.cctor>b__338_2(byte[] data)
			{
				return (double)((int)data[0] * 256 + (int)data[1]);
			}

			// Token: 0x06002CA0 RID: 11424 RVA: 0x001FA4C5 File Offset: 0x001F86C5
			internal double <.cctor>b__338_3(byte[] data)
			{
				return (double)data[0] * 100.0 / 255.0;
			}

			// Token: 0x06002CA1 RID: 11425 RVA: 0x001FC4B4 File Offset: 0x001FA6B4
			internal double <.cctor>b__338_4(byte[] data)
			{
				return (double)((float)data[0]) * 0.005;
			}

			// Token: 0x06002CA2 RID: 11426 RVA: 0x001FC4C5 File Offset: 0x001FA6C5
			internal double <.cctor>b__338_5(byte[] data)
			{
				if (data[1] == 255)
				{
					return double.NaN;
				}
				return (double)(data[1] - 128) * 100.0 / 128.0;
			}

			// Token: 0x06002CA3 RID: 11427 RVA: 0x001FC4FC File Offset: 0x001FA6FC
			internal double <.cctor>b__338_6(byte[] data)
			{
				double num = (double)data[0];
				num *= 256.0;
				num += (double)data[1];
				num *= PIDWithFloatValueFormula.EquivalenceRatioScalingPerBit;
				if (SharedSettings.Current.ShowAirFuelBasedOnStoichiometric && SharedSettings.Current.FuelType == FuelTypes.Gasoline)
				{
					num *= 14.7;
				}
				return num;
			}

			// Token: 0x06002CA4 RID: 11428 RVA: 0x001FC54E File Offset: 0x001FA74E
			internal double <.cctor>b__338_7(byte[] data)
			{
				return ((double)data[2] * 256.0 + (double)data[3]) * PIDWithFloatValueFormula.OxygenSensorVoltageScalingPerBit;
			}

			// Token: 0x06002CA5 RID: 11429 RVA: 0x001FC56C File Offset: 0x001FA76C
			internal double <.cctor>b__338_8(byte[] data)
			{
				double num = (double)data[0];
				num *= 256.0;
				num += (double)data[1];
				num *= PIDWithFloatValueFormula.EquivalenceRatioScalingPerBit;
				if (SharedSettings.Current.ShowAirFuelBasedOnStoichiometric && SharedSettings.Current.FuelType == FuelTypes.Gasoline)
				{
					num *= 14.7;
				}
				return num;
			}

			// Token: 0x06002CA6 RID: 11430 RVA: 0x001FC5BE File Offset: 0x001FA7BE
			internal double <.cctor>b__338_9(byte[] data)
			{
				return ((double)data[2] * 256.0 + (double)data[3] - 32768.0) * PIDWithFloatValueFormula.OxygenSensorCurrentScalingPerBit;
			}

			// Token: 0x06002CA7 RID: 11431 RVA: 0x001FA51C File Offset: 0x001F871C
			internal double <.cctor>b__338_10(byte[] data)
			{
				return (double)((data[0] - 128) * 100 / 128);
			}

			// Token: 0x06002CA8 RID: 11432 RVA: 0x001FC5E3 File Offset: 0x001FA7E3
			internal double <.cctor>b__338_11(byte[] data)
			{
				if (data.Length >= 2)
				{
					return (double)((data[1] - 128) * 100 / 128);
				}
				return double.NaN;
			}

			// Token: 0x04001815 RID: 6165
			public static readonly PIDWithFloatValueFormula.<>c <>9 = new PIDWithFloatValueFormula.<>c();

			// Token: 0x04001816 RID: 6166
			public static Func<byte[], double> <>9__54_0;

			// Token: 0x04001817 RID: 6167
			public static Func<byte[], double> <>9__60_0;

			// Token: 0x04001818 RID: 6168
			public static Func<byte[], double> <>9__61_0;

			// Token: 0x04001819 RID: 6169
			public static Func<byte[], double> <>9__62_0;

			// Token: 0x0400181A RID: 6170
			public static Func<byte[], double> <>9__63_0;

			// Token: 0x0400181B RID: 6171
			public static Func<byte[], double> <>9__64_0;

			// Token: 0x0400181C RID: 6172
			public static Func<byte[], double> <>9__66_0;

			// Token: 0x0400181D RID: 6173
			public static Func<byte[], double> <>9__67_0;

			// Token: 0x0400181E RID: 6174
			public static Func<byte[], double> <>9__69_0;

			// Token: 0x0400181F RID: 6175
			public static Func<byte[], double> <>9__70_0;

			// Token: 0x04001820 RID: 6176
			public static Func<byte[], double> <>9__71_0;

			// Token: 0x04001821 RID: 6177
			public static Func<byte[], double> <>9__72_0;

			// Token: 0x04001822 RID: 6178
			public static Func<byte[], double> <>9__73_0;

			// Token: 0x04001823 RID: 6179
			public static Func<byte[], double> <>9__74_0;

			// Token: 0x04001824 RID: 6180
			public static Func<byte[], double> <>9__75_0;

			// Token: 0x04001825 RID: 6181
			public static Func<byte[], double> <>9__77_0;

			// Token: 0x04001826 RID: 6182
			public static Func<byte[], double> <>9__78_0;

			// Token: 0x04001827 RID: 6183
			public static Func<byte[], double> <>9__79_0;

			// Token: 0x04001828 RID: 6184
			public static Func<byte[], double> <>9__80_0;

			// Token: 0x04001829 RID: 6185
			public static Func<byte[], double> <>9__81_0;

			// Token: 0x0400182A RID: 6186
			public static Func<byte[], double> <>9__82_0;

			// Token: 0x0400182B RID: 6187
			public static Func<byte[], double> <>9__83_0;

			// Token: 0x0400182C RID: 6188
			public static Func<byte[], double> <>9__84_0;

			// Token: 0x0400182D RID: 6189
			public static Func<byte[], double> <>9__85_0;

			// Token: 0x0400182E RID: 6190
			public static Func<byte[], double> <>9__86_0;

			// Token: 0x0400182F RID: 6191
			public static Func<byte[], double> <>9__88_0;

			// Token: 0x04001830 RID: 6192
			public static Func<byte[], double> <>9__89_0;

			// Token: 0x04001831 RID: 6193
			public static Func<byte[], double> <>9__90_0;

			// Token: 0x04001832 RID: 6194
			public static Func<byte[], double> <>9__91_0;

			// Token: 0x04001833 RID: 6195
			public static Func<byte[], double> <>9__92_0;

			// Token: 0x04001834 RID: 6196
			public static Func<byte[], double> <>9__93_0;

			// Token: 0x04001835 RID: 6197
			public static Func<byte[], double> <>9__94_0;

			// Token: 0x04001836 RID: 6198
			public static Func<byte[], double> <>9__95_0;

			// Token: 0x04001837 RID: 6199
			public static Func<byte[], double> <>9__96_0;

			// Token: 0x04001838 RID: 6200
			public static Func<byte[], double> <>9__97_0;

			// Token: 0x04001839 RID: 6201
			public static Func<byte[], double> <>9__98_0;

			// Token: 0x0400183A RID: 6202
			public static Func<byte[], double> <>9__99_0;

			// Token: 0x0400183B RID: 6203
			public static Func<byte[], double> <>9__100_0;

			// Token: 0x0400183C RID: 6204
			public static Func<byte[], double> <>9__102_0;

			// Token: 0x0400183D RID: 6205
			public static Func<byte[], double> <>9__103_0;

			// Token: 0x0400183E RID: 6206
			public static Func<byte[], double> <>9__104_0;

			// Token: 0x0400183F RID: 6207
			public static Func<byte[], double> <>9__105_0;

			// Token: 0x04001840 RID: 6208
			public static Func<byte[], double> <>9__164_0;

			// Token: 0x04001841 RID: 6209
			public static Func<byte[], double> <>9__165_0;

			// Token: 0x04001842 RID: 6210
			public static Func<byte[], double> <>9__166_0;

			// Token: 0x04001843 RID: 6211
			public static Func<byte[], double> <>9__167_0;

			// Token: 0x04001844 RID: 6212
			public static Func<byte[], double> <>9__168_0;

			// Token: 0x04001845 RID: 6213
			public static Func<byte[], double> <>9__169_0;

			// Token: 0x04001846 RID: 6214
			public static Func<byte[], double> <>9__170_0;

			// Token: 0x04001847 RID: 6215
			public static Func<byte[], double> <>9__171_0;

			// Token: 0x04001848 RID: 6216
			public static Func<byte[], double> <>9__172_0;

			// Token: 0x04001849 RID: 6217
			public static Func<byte[], double> <>9__173_0;

			// Token: 0x0400184A RID: 6218
			public static Func<byte[], double> <>9__174_0;

			// Token: 0x0400184B RID: 6219
			public static Func<byte[], double> <>9__175_0;

			// Token: 0x0400184C RID: 6220
			public static Func<byte[], double> <>9__176_0;

			// Token: 0x0400184D RID: 6221
			public static Func<byte[], double> <>9__177_0;

			// Token: 0x0400184E RID: 6222
			public static Func<byte[], double> <>9__178_0;

			// Token: 0x0400184F RID: 6223
			public static Func<byte[], double> <>9__179_0;

			// Token: 0x04001850 RID: 6224
			public static Func<byte[], double> <>9__180_0;

			// Token: 0x04001851 RID: 6225
			public static Func<byte[], double> <>9__181_0;

			// Token: 0x04001852 RID: 6226
			public static Func<byte[], double> <>9__182_0;

			// Token: 0x04001853 RID: 6227
			public static Func<byte[], double> <>9__183_0;

			// Token: 0x04001854 RID: 6228
			public static Func<byte[], double> <>9__184_0;

			// Token: 0x04001855 RID: 6229
			public static Func<byte[], double> <>9__185_0;

			// Token: 0x04001856 RID: 6230
			public static Func<byte[], double> <>9__186_0;

			// Token: 0x04001857 RID: 6231
			public static Func<byte[], double> <>9__187_0;

			// Token: 0x04001858 RID: 6232
			public static Func<byte[], double> <>9__188_0;

			// Token: 0x04001859 RID: 6233
			public static Func<byte[], double> <>9__189_0;

			// Token: 0x0400185A RID: 6234
			public static Func<byte[], double> <>9__190_0;

			// Token: 0x0400185B RID: 6235
			public static Func<byte[], double> <>9__191_0;

			// Token: 0x0400185C RID: 6236
			public static Func<byte[], double> <>9__192_0;

			// Token: 0x0400185D RID: 6237
			public static Func<byte[], double> <>9__193_0;

			// Token: 0x0400185E RID: 6238
			public static Func<byte[], double> <>9__194_0;

			// Token: 0x0400185F RID: 6239
			public static Func<byte[], double> <>9__195_0;

			// Token: 0x04001860 RID: 6240
			public static Func<byte[], double> <>9__196_0;

			// Token: 0x04001861 RID: 6241
			public static Func<byte[], double> <>9__197_0;

			// Token: 0x04001862 RID: 6242
			public static Func<byte[], double> <>9__198_0;

			// Token: 0x04001863 RID: 6243
			public static Func<byte[], double> <>9__199_0;

			// Token: 0x04001864 RID: 6244
			public static Func<byte[], double> <>9__200_0;

			// Token: 0x04001865 RID: 6245
			public static Func<byte[], double> <>9__201_0;

			// Token: 0x04001866 RID: 6246
			public static Func<byte[], double> <>9__202_0;

			// Token: 0x04001867 RID: 6247
			public static Func<byte[], double> <>9__203_0;

			// Token: 0x04001868 RID: 6248
			public static Func<byte[], double> <>9__204_0;

			// Token: 0x04001869 RID: 6249
			public static Func<byte[], double> <>9__205_0;

			// Token: 0x0400186A RID: 6250
			public static Func<byte[], double> <>9__206_0;

			// Token: 0x0400186B RID: 6251
			public static Func<byte[], double> <>9__207_0;

			// Token: 0x0400186C RID: 6252
			public static Func<byte[], double> <>9__208_0;

			// Token: 0x0400186D RID: 6253
			public static Func<byte[], double> <>9__209_0;

			// Token: 0x0400186E RID: 6254
			public static Func<byte[], double> <>9__210_0;

			// Token: 0x0400186F RID: 6255
			public static Func<byte[], double> <>9__211_0;

			// Token: 0x04001870 RID: 6256
			public static Func<byte[], double> <>9__212_0;

			// Token: 0x04001871 RID: 6257
			public static Func<byte[], double> <>9__213_0;

			// Token: 0x04001872 RID: 6258
			public static Func<byte[], double> <>9__214_0;

			// Token: 0x04001873 RID: 6259
			public static Func<byte[], double> <>9__215_0;

			// Token: 0x04001874 RID: 6260
			public static Func<byte[], double> <>9__216_0;

			// Token: 0x04001875 RID: 6261
			public static Func<byte[], double> <>9__217_0;

			// Token: 0x04001876 RID: 6262
			public static Func<byte[], double> <>9__218_0;

			// Token: 0x04001877 RID: 6263
			public static Func<byte[], double> <>9__219_0;

			// Token: 0x04001878 RID: 6264
			public static Func<byte[], double> <>9__220_0;

			// Token: 0x04001879 RID: 6265
			public static Func<byte[], double> <>9__221_0;

			// Token: 0x0400187A RID: 6266
			public static Func<byte[], double> <>9__222_0;

			// Token: 0x0400187B RID: 6267
			public static Func<byte[], double> <>9__223_0;

			// Token: 0x0400187C RID: 6268
			public static Func<byte[], double> <>9__224_0;

			// Token: 0x0400187D RID: 6269
			public static Func<byte[], double> <>9__225_0;

			// Token: 0x0400187E RID: 6270
			public static Func<byte[], double> <>9__226_0;

			// Token: 0x0400187F RID: 6271
			public static Func<byte[], double> <>9__227_0;

			// Token: 0x04001880 RID: 6272
			public static Func<byte[], double> <>9__228_0;

			// Token: 0x04001881 RID: 6273
			public static Func<byte[], double> <>9__229_0;

			// Token: 0x04001882 RID: 6274
			public static Func<byte[], double> <>9__230_0;

			// Token: 0x04001883 RID: 6275
			public static Func<byte[], double> <>9__231_0;

			// Token: 0x04001884 RID: 6276
			public static Func<byte[], double> <>9__232_0;

			// Token: 0x04001885 RID: 6277
			public static Func<byte[], double> <>9__233_0;

			// Token: 0x04001886 RID: 6278
			public static Func<byte[], double> <>9__234_0;

			// Token: 0x04001887 RID: 6279
			public static Func<byte[], double> <>9__235_0;

			// Token: 0x04001888 RID: 6280
			public static Func<byte[], double> <>9__236_0;

			// Token: 0x04001889 RID: 6281
			public static Func<byte[], double> <>9__237_0;

			// Token: 0x0400188A RID: 6282
			public static Func<byte[], double> <>9__238_0;

			// Token: 0x0400188B RID: 6283
			public static Func<byte[], double> <>9__239_0;

			// Token: 0x0400188C RID: 6284
			public static Func<byte[], double> <>9__240_0;

			// Token: 0x0400188D RID: 6285
			public static Func<byte[], double> <>9__241_0;

			// Token: 0x0400188E RID: 6286
			public static Func<byte[], double> <>9__242_0;

			// Token: 0x0400188F RID: 6287
			public static Func<byte[], double> <>9__243_0;

			// Token: 0x04001890 RID: 6288
			public static Func<byte[], double> <>9__244_0;

			// Token: 0x04001891 RID: 6289
			public static Func<byte[], double> <>9__245_0;

			// Token: 0x04001892 RID: 6290
			public static Func<byte[], double> <>9__246_0;

			// Token: 0x04001893 RID: 6291
			public static Func<byte[], double> <>9__247_0;

			// Token: 0x04001894 RID: 6292
			public static Func<byte[], double> <>9__248_0;

			// Token: 0x04001895 RID: 6293
			public static Func<byte[], double> <>9__249_0;

			// Token: 0x04001896 RID: 6294
			public static Func<byte[], double> <>9__252_0;

			// Token: 0x04001897 RID: 6295
			public static Func<byte[], double> <>9__253_0;

			// Token: 0x04001898 RID: 6296
			public static Func<byte[], double> <>9__254_0;

			// Token: 0x04001899 RID: 6297
			public static Func<byte[], double> <>9__255_0;

			// Token: 0x0400189A RID: 6298
			public static Func<byte[], double> <>9__256_0;

			// Token: 0x0400189B RID: 6299
			public static Func<byte[], double> <>9__257_0;

			// Token: 0x0400189C RID: 6300
			public static Func<byte[], double> <>9__258_0;

			// Token: 0x0400189D RID: 6301
			public static Func<byte[], double> <>9__259_0;

			// Token: 0x0400189E RID: 6302
			public static Func<byte[], double> <>9__260_0;

			// Token: 0x0400189F RID: 6303
			public static Func<byte[], double> <>9__261_0;

			// Token: 0x040018A0 RID: 6304
			public static Func<byte[], double> <>9__262_0;

			// Token: 0x040018A1 RID: 6305
			public static Func<byte[], double> <>9__263_0;

			// Token: 0x040018A2 RID: 6306
			public static Func<byte[], double> <>9__264_0;

			// Token: 0x040018A3 RID: 6307
			public static Func<byte[], double> <>9__265_0;

			// Token: 0x040018A4 RID: 6308
			public static Func<byte[], double> <>9__266_0;

			// Token: 0x040018A5 RID: 6309
			public static Func<byte[], double> <>9__267_0;

			// Token: 0x040018A6 RID: 6310
			public static Func<byte[], double> <>9__268_0;

			// Token: 0x040018A7 RID: 6311
			public static Func<byte[], double> <>9__269_0;

			// Token: 0x040018A8 RID: 6312
			public static Func<byte[], double> <>9__270_0;

			// Token: 0x040018A9 RID: 6313
			public static Func<byte[], double> <>9__271_0;

			// Token: 0x040018AA RID: 6314
			public static Func<byte[], double> <>9__273_0;

			// Token: 0x040018AB RID: 6315
			public static Func<byte[], double> <>9__274_0;

			// Token: 0x040018AC RID: 6316
			public static Func<byte[], double> <>9__275_0;

			// Token: 0x040018AD RID: 6317
			public static Func<byte[], double> <>9__276_0;

			// Token: 0x040018AE RID: 6318
			public static Func<byte[], double> <>9__277_0;

			// Token: 0x040018AF RID: 6319
			public static Func<byte[], double> <>9__278_0;

			// Token: 0x040018B0 RID: 6320
			public static Func<byte[], double> <>9__279_0;

			// Token: 0x040018B1 RID: 6321
			public static Func<byte[], double> <>9__280_0;

			// Token: 0x040018B2 RID: 6322
			public static Func<byte[], double> <>9__281_0;

			// Token: 0x040018B3 RID: 6323
			public static Func<byte[], double> <>9__282_0;

			// Token: 0x040018B4 RID: 6324
			public static Func<byte[], double> <>9__283_0;

			// Token: 0x040018B5 RID: 6325
			public static Func<byte[], double> <>9__284_0;

			// Token: 0x040018B6 RID: 6326
			public static Func<byte[], double> <>9__285_0;

			// Token: 0x040018B7 RID: 6327
			public static Func<byte[], double> <>9__286_0;

			// Token: 0x040018B8 RID: 6328
			public static Func<byte[], double> <>9__287_0;

			// Token: 0x040018B9 RID: 6329
			public static Func<byte[], double> <>9__289_0;

			// Token: 0x040018BA RID: 6330
			public static Func<byte[], double> <>9__290_0;

			// Token: 0x040018BB RID: 6331
			public static Func<byte[], double> <>9__291_0;

			// Token: 0x040018BC RID: 6332
			public static Func<byte[], double> <>9__292_0;

			// Token: 0x040018BD RID: 6333
			public static Func<byte[], double> <>9__293_0;

			// Token: 0x040018BE RID: 6334
			public static Func<byte[], double> <>9__294_0;

			// Token: 0x040018BF RID: 6335
			public static Func<byte[], double> <>9__295_0;

			// Token: 0x040018C0 RID: 6336
			public static Func<byte[], double> <>9__296_0;

			// Token: 0x040018C1 RID: 6337
			public static Func<byte[], double> <>9__297_0;

			// Token: 0x040018C2 RID: 6338
			public static Func<byte[], double> <>9__298_0;

			// Token: 0x040018C3 RID: 6339
			public static Func<byte[], double> <>9__299_0;

			// Token: 0x040018C4 RID: 6340
			public static Func<byte[], double> <>9__300_0;

			// Token: 0x040018C5 RID: 6341
			public static Func<byte[], double> <>9__301_0;

			// Token: 0x040018C6 RID: 6342
			public static Func<byte[], double> <>9__302_0;

			// Token: 0x040018C7 RID: 6343
			public static Func<byte[], double> <>9__303_0;

			// Token: 0x040018C8 RID: 6344
			public static Func<byte[], double> <>9__304_0;

			// Token: 0x040018C9 RID: 6345
			public static Func<byte[], double> <>9__305_0;

			// Token: 0x040018CA RID: 6346
			public static Func<byte[], double> <>9__306_0;

			// Token: 0x040018CB RID: 6347
			public static Func<byte[], double> <>9__307_0;

			// Token: 0x040018CC RID: 6348
			public static Func<byte[], double> <>9__308_0;

			// Token: 0x040018CD RID: 6349
			public static Func<byte[], double> <>9__309_0;

			// Token: 0x040018CE RID: 6350
			public static Func<byte[], double> <>9__310_0;

			// Token: 0x040018CF RID: 6351
			public static Func<byte[], double> <>9__311_0;

			// Token: 0x040018D0 RID: 6352
			public static Func<byte[], double> <>9__312_0;

			// Token: 0x040018D1 RID: 6353
			public static Func<byte[], double> <>9__313_0;

			// Token: 0x040018D2 RID: 6354
			public static Func<byte[], double> <>9__314_0;

			// Token: 0x040018D3 RID: 6355
			public static Func<byte[], double> <>9__315_0;

			// Token: 0x040018D4 RID: 6356
			public static Func<byte[], double> <>9__316_0;

			// Token: 0x040018D5 RID: 6357
			public static Func<byte[], double> <>9__317_0;

			// Token: 0x040018D6 RID: 6358
			public static Func<byte[], double> <>9__318_0;

			// Token: 0x040018D7 RID: 6359
			public static Func<byte[], double> <>9__319_0;

			// Token: 0x040018D8 RID: 6360
			public static Func<byte[], double> <>9__320_0;

			// Token: 0x040018D9 RID: 6361
			public static Func<byte[], double> <>9__321_0;

			// Token: 0x040018DA RID: 6362
			public static Func<byte[], double> <>9__322_0;

			// Token: 0x040018DB RID: 6363
			public static Func<byte[], double> <>9__323_0;

			// Token: 0x040018DC RID: 6364
			public static Func<byte[], double> <>9__324_0;

			// Token: 0x040018DD RID: 6365
			public static Func<byte[], double> <>9__325_0;

			// Token: 0x040018DE RID: 6366
			public static Func<byte[], double> <>9__326_0;

			// Token: 0x040018DF RID: 6367
			public static Func<byte[], double> <>9__327_0;

			// Token: 0x040018E0 RID: 6368
			public static Func<byte[], double> <>9__328_0;

			// Token: 0x040018E1 RID: 6369
			public static Func<byte[], double> <>9__329_0;

			// Token: 0x040018E2 RID: 6370
			public static Func<byte[], double> <>9__330_0;

			// Token: 0x040018E3 RID: 6371
			public static Func<byte[], double> <>9__331_0;

			// Token: 0x040018E4 RID: 6372
			public static Func<byte[], double> <>9__332_0;

			// Token: 0x040018E5 RID: 6373
			public static Func<byte[], double> <>9__333_0;

			// Token: 0x040018E6 RID: 6374
			public static Func<byte[], double> <>9__334_0;

			// Token: 0x040018E7 RID: 6375
			public static Func<byte[], double> <>9__335_0;

			// Token: 0x040018E8 RID: 6376
			public static Func<byte[], double> <>9__336_0;

			// Token: 0x040018E9 RID: 6377
			public static Func<byte[], double> <>9__337_0;
		}

		// Token: 0x02000410 RID: 1040
		[CompilerGenerated]
		private sealed class <>c__DisplayClass250_0
		{
			// Token: 0x06002CA9 RID: 11433 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass250_0()
			{
			}

			// Token: 0x06002CAA RID: 11434 RVA: 0x001FC608 File Offset: 0x001FA808
			internal double <GetPID0908>b__0(byte[] data)
			{
				if (data.Length >= this.id + 2)
				{
					return (double)((int)data[this.id + 1] * 256 + (int)data[this.id + 2]);
				}
				return double.NaN;
			}

			// Token: 0x040018EA RID: 6378
			public int id;
		}

		// Token: 0x02000411 RID: 1041
		[CompilerGenerated]
		private sealed class <>c__DisplayClass251_0
		{
			// Token: 0x06002CAB RID: 11435 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass251_0()
			{
			}

			// Token: 0x06002CAC RID: 11436 RVA: 0x001FC642 File Offset: 0x001FA842
			internal double <GetPID090B>b__0(byte[] data)
			{
				if (data.Length >= this.id + 2)
				{
					return (double)((int)data[this.id + 1] * 256 + (int)data[this.id + 2]);
				}
				return double.NaN;
			}

			// Token: 0x040018EB RID: 6379
			public int id;
		}
	}
}
