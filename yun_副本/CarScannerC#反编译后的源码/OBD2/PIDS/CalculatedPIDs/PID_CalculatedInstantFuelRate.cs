using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200044C RID: 1100
	public class PID_CalculatedInstantFuelRate : CalculatedPIDV2
	{
		// Token: 0x06002E28 RID: 11816 RVA: 0x00202CFC File Offset: 0x00200EFC
		public PID_CalculatedInstantFuelRate(PID_EconomizerFSSandThrottlePosition economizer)
			: base(PID.GetResourceString("PID_CalculatedInstantFuelRate"), "INSTANT_FUEL_RATE", UnitsHelper.Units.Lh, 0.0, 30.0, Roles.CALC_InstantFuelRate)
		{
			base.ShortName = Translate.GetString("PID_CalculatedInstantFuelRate_Short");
			base.Id = 230;
			this.Economizer = economizer;
			this.LowPriorityInterval = 300;
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x00202D70 File Offset: 0x00200F70
		private double GetVolumetricEfficiency(double RPM)
		{
			if (RPM >= (double)this.VolumetricEfficiency.Keys.Last<int>())
			{
				return (double)this.VolumetricEfficiency.Values.Last<int>();
			}
			if (RPM <= (double)this.VolumetricEfficiency.Keys.First<int>())
			{
				return (double)this.VolumetricEfficiency.Values.First<int>();
			}
			int num = (int)RPM;
			int num2 = 0;
			int maxValue = int.MaxValue;
			foreach (int num3 in this.VolumetricEfficiency.Keys)
			{
				if (Math.Abs(num - num3) < maxValue)
				{
					num2 = num3;
				}
			}
			return (double)this.VolumetricEfficiency[num2];
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x00202E38 File Offset: 0x00201038
		public override void Initialize()
		{
			this.ResetValues();
			this.requiredPIDs.Clear();
			this.lowPriorityRequiredPIDs.Clear();
			this.air_density = 1.2041;
			this.atmospheric_pressure = 101.325;
			List<PID> liveDataPIDs = App.OBDReader.CurrentCarData.LiveDataPIDs;
			this.FuelRate = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.InstantFuelRate);
			if (this.FuelRate == null)
			{
				this.FuelRate = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 123) as IPIDFloatValue;
			}
			this.MAF = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.MAF);
			if (this.MAF == null)
			{
				if (App.OBDReader.IsNissanConsult2Protocol)
				{
					this.MAF = liveDataPIDs.FirstOrDefault((PID x) => x.Command == "221209") as IPIDFloatValue;
				}
				else
				{
					this.MAF = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 16) as IPIDFloatValue;
				}
			}
			this.MAP = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.MAP);
			if (this.MAP == null)
			{
				this.MAP = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 11) as IPIDFloatValue;
			}
			this.IAT = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.IAT);
			if (this.IAT == null)
			{
				this.IAT = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 15) as IPIDFloatValue;
			}
			this.RPM = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.RPM);
			if (this.RPM == null)
			{
				if (App.OBDReader.IsNissanConsult2Protocol)
				{
					this.RPM = liveDataPIDs.FirstOrDefault((PID x) => x.Command == "221201") as IPIDFloatValue;
				}
				else
				{
					this.RPM = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 12) as IPIDFloatValue;
				}
			}
			this.AFR_LAMBDA = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.LAMBDA);
			if (this.AFR_LAMBDA == null)
			{
				this.AFR_LAMBDA = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 92) as IPIDFloatValue;
			}
			this.Economizer = liveDataPIDs.FirstOrDefault((PID x) => x.Command == "ECONOMIZER") as IPIDFloatValue;
			this.LOAD_ABS = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.LOAD_ABS);
			if (this.LOAD_ABS == null)
			{
				this.LOAD_ABS = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 91) as IPIDFloatValue;
			}
			this.LOAD_PCT = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.LOAD_PCT);
			if (this.LOAD_PCT == null)
			{
				this.LOAD_PCT = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 4) as IPIDFloatValue;
			}
			this.BARO = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.BARO);
			if (this.BARO == null)
			{
				this.BARO = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 67) as IPIDFloatValue;
			}
			this.AMBIENT_TEMP = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.AMBIENT_TEMP);
			if (this.AMBIENT_TEMP == null)
			{
				this.AMBIENT_TEMP = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 94) as IPIDFloatValue;
			}
			this.INJECTOR = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Injection);
			if (this.INJECTOR == null)
			{
				this.INJECTOR = SharedSettings.Current.GetInjectorPID() as IPIDFloatValue;
			}
			this.CYCLE_CONSUMPTION = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.CycleFuelConsumption);
			if (this.AMBIENT_TEMP != null && this.BARO != null && this.AMBIENT_TEMP.IsAvailable && this.BARO.IsAvailable)
			{
				this.BARO_AND_AMBIENT_TEMP_AVAILABLE = true;
			}
			else
			{
				this.BARO_AND_AMBIENT_TEMP_AVAILABLE = false;
			}
			if (SharedSettings.Current.FuelType == FuelTypes.FlexFuelOBDII)
			{
				this.FUEL_TYPE_PID = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 106) as IPIDWithStringValue;
				if (!this.FUEL_TYPE_PID.IsAvailable)
				{
					this.FUEL_TYPE_PID = null;
				}
			}
			else
			{
				this.FUEL_TYPE_PID = null;
			}
			this.VolumetricEfficiency = new Dictionary<int, int>();
			this.VolumetricEfficiency.Add(1000, SharedSettings.Current.VE1000);
			this.VolumetricEfficiency.Add(2000, SharedSettings.Current.VE2000);
			this.VolumetricEfficiency.Add(3000, SharedSettings.Current.VE3000);
			this.VolumetricEfficiency.Add(4000, SharedSettings.Current.VE4000);
			this.VolumetricEfficiency.Add(5000, SharedSettings.Current.VE5000);
			this.VolumetricEfficiency.Add(6000, SharedSettings.Current.VE6000);
			this.VolumetricEfficiency.Add(7000, SharedSettings.Current.VE7000);
			this.VolumetricEfficiency.Add(8000, SharedSettings.Current.VE8000);
			this.SetFuelProperties();
			if (SharedSettings.Current.FuelFlowCalculationScheme == FuelFlowCalculationSchemes.Auto)
			{
				if (this.FuelRate != null && this.FuelRate.IsAvailable)
				{
					this.requiredPIDs.Add(this.FuelRate);
					base.DependencyPID = this.FuelRate;
					this.CurrentCalculationScheme = FuelFlowCalculationSchemes.FuelRate;
				}
				else if (this.CYCLE_CONSUMPTION != null && this.CYCLE_CONSUMPTION.IsAvailable && this.RPM != null && this.RPM.IsAvailable)
				{
					this.Init_CycleConsumption();
					this.CurrentCalculationScheme = FuelFlowCalculationSchemes.CycleConsumption;
				}
				else if (this.MAF != null && this.MAF.IsAvailable)
				{
					this.Init_MAF();
					this.CurrentCalculationScheme = FuelFlowCalculationSchemes.MAF;
				}
				else if (this.MAP != null && this.IAT != null && this.RPM != null && this.MAP.IsAvailable && this.IAT.IsAvailable && this.RPM.IsAvailable)
				{
					this.Init_MAP();
					this.CurrentCalculationScheme = FuelFlowCalculationSchemes.MAP;
				}
				else if (this.LOAD_ABS != null && this.RPM != null && this.LOAD_ABS.IsAvailable && this.RPM.IsAvailable)
				{
					this.Init_Load_ABS();
					this.CurrentCalculationScheme = FuelFlowCalculationSchemes.LOAD_ABS;
				}
				else if (this.INJECTOR != null && this.INJECTOR.IsAvailable && this.RPM != null && this.RPM.IsAvailable)
				{
					this.Init_Injector();
					this.CurrentCalculationScheme = FuelFlowCalculationSchemes.Injector;
				}
				else
				{
					this.IsAvailable = false;
				}
			}
			else
			{
				this.CurrentCalculationScheme = SharedSettings.Current.FuelFlowCalculationScheme;
				switch (this.CurrentCalculationScheme)
				{
				case FuelFlowCalculationSchemes.MAF:
					this.Init_MAF();
					break;
				case FuelFlowCalculationSchemes.LOAD_ABS:
					this.Init_Load_ABS();
					break;
				case FuelFlowCalculationSchemes.MAP:
					this.Init_MAP();
					break;
				case FuelFlowCalculationSchemes.FuelRate:
					this.requiredPIDs.Add(this.FuelRate);
					base.DependencyPID = this.FuelRate;
					break;
				case FuelFlowCalculationSchemes.Injector:
					this.Init_Injector();
					break;
				case FuelFlowCalculationSchemes.CycleConsumption:
					this.Init_CycleConsumption();
					break;
				}
			}
			if (SharedSettings.Current.FilterRPM300 && this.RPM != null && this.RPM.IsAvailable && base.RequiredPIDs.Contains(this.RPM))
			{
				this.checkRPM300 = true;
			}
			else
			{
				this.checkRPM300 = false;
			}
			this.PrintToDebugLog();
			base.Initialize();
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x0020365C File Offset: 0x0020185C
		private void PrintToDebugLog()
		{
			if (App.OBDSimulator.IsActive)
			{
				return;
			}
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				return;
			}
			try
			{
				IPID[] array = new IPIDFloatValue[]
				{
					this.FuelRate, this.MAF, this.MAP, this.IAT, this.RPM, this.AFR_LAMBDA, this.FSS, this.LOAD_ABS, this.LOAD_PCT, this.Economizer,
					this.BARO, this.AMBIENT_TEMP, this.O2_V1, this.O2_V2, this.INJECTOR, this.CYCLE_CONSUMPTION
				};
				IPID[] array2 = array;
				string[] array3 = new string[]
				{
					"FuelRate", "MAF", "MAP", "IAT", "RPM", "AFR_LAMBDA", "FSS", "LOAD_ABS", "LOAD_PCT", "Economizer",
					"BARO", "AMBIENT_TEMP", "O2_V1", "O2_V2", "INJECTOR", "CYCLE_CONSUMPTION"
				};
				StringBuilder stringBuilder = new StringBuilder(6 + array2.Length);
				stringBuilder.Append("\n[FUELRATE: ");
				stringBuilder.Append(this.CurrentCalculationScheme.ToString());
				stringBuilder.Append(";");
				for (int i = 0; i < array2.Length; i++)
				{
					stringBuilder.Append(this.PidToDebugString(array3[i], array2[i]));
				}
				stringBuilder.Append("]\n");
				App.OBDReader.DebugWrite(stringBuilder.ToString());
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x00203854 File Offset: 0x00201A54
		private string PidToDebugString(string name, IPID pid)
		{
			if (pid == null)
			{
				return name + "=null;";
			}
			return string.Concat(new string[]
			{
				name,
				"=",
				pid.Id.ToString(),
				":",
				pid.IsAvailable.ToString(),
				":",
				pid.Name.ToString(),
				";"
			});
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x002038CF File Offset: 0x00201ACF
		private void Init_CycleConsumption()
		{
			if (this.RPM != null)
			{
				this.requiredPIDs.Add(this.RPM);
			}
			if (this.CYCLE_CONSUMPTION != null)
			{
				this.requiredPIDs.Add(this.CYCLE_CONSUMPTION);
			}
			base.DependencyPID = this.CYCLE_CONSUMPTION;
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x00203910 File Offset: 0x00201B10
		private void SetFuelProperties()
		{
			FuelTypes fuelTypes = SharedSettings.Current.FuelType;
			if (fuelTypes == FuelTypes.FlexFuelOBDII && this.FUEL_TYPE_PID != null)
			{
				switch (this.FUEL_TYPE_PID.IntValue)
				{
				case 2:
				case 10:
					fuelTypes = FuelTypes.Methanol;
					goto IL_00AB;
				case 3:
				case 11:
				case 18:
					fuelTypes = FuelTypes.Ethanol;
					goto IL_00AB;
				case 4:
				case 19:
				case 23:
					fuelTypes = FuelTypes.Diesel;
					goto IL_00AB;
				case 5:
				case 12:
					fuelTypes = FuelTypes.Propan;
					goto IL_00AB;
				case 6:
				case 13:
					fuelTypes = FuelTypes.Propan;
					goto IL_00AB;
				case 7:
				case 14:
					fuelTypes = FuelTypes.Propan;
					goto IL_00AB;
				}
				fuelTypes = FuelTypes.Gasoline;
			}
			IL_00AB:
			switch (fuelTypes)
			{
			case FuelTypes.Diesel:
				this.Stechiometric = 15.0;
				this.fuel_density = 0.832;
				return;
			case FuelTypes.Ethanol:
				this.Stechiometric = 9.0;
				this.fuel_density = 0.789;
				return;
			case FuelTypes.Methanol:
				this.Stechiometric = 6.4;
				this.fuel_density = 0.792;
				return;
			case FuelTypes.Propan:
				this.Stechiometric = 15.5;
				this.fuel_density = 0.493;
				return;
			case FuelTypes.Methan:
				this.Stechiometric = 17.2;
				this.fuel_density = 0.656;
				return;
			case FuelTypes.Custom:
				this.Stechiometric = SharedSettings.Current.CustomFuelAF;
				this.fuel_density = SharedSettings.Current.CustomFuelDensity;
				return;
			case FuelTypes.EvNoFuel:
				this.Stechiometric = 1.0;
				this.fuel_density = 1.0;
				return;
			}
			this.Stechiometric = 14.7;
			this.fuel_density = 0.73;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x00203AF0 File Offset: 0x00201CF0
		private void Init_Injector()
		{
			if (this.INJECTOR == null)
			{
				this.IsAvailable = false;
				return;
			}
			this.requiredPIDs.Add(this.INJECTOR);
			this.requiredPIDs.Add(this.RPM);
			if (SharedSettings.Current.DetectZeroFuelConsumption && this.Economizer.IsAvailable)
			{
				this.requiredPIDs.Add(this.Economizer);
			}
			base.DependencyPID = this.INJECTOR;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x00203B68 File Offset: 0x00201D68
		private void Init_MAP()
		{
			this.requiredPIDs.Add(this.MAP);
			if ((this.AFR_LAMBDA as PID).IsAvailable && !SharedSettings.Current.FuelFlowUseFixedAFR)
			{
				this.requiredPIDs.Add(this.AFR_LAMBDA);
			}
			if (SharedSettings.Current.DetectZeroFuelConsumption && (this.Economizer as PID).IsAvailable)
			{
				this.requiredPIDs.Add(this.Economizer);
			}
			this.lowPriorityRequiredPIDs.Add(this.IAT);
			this.Init_Diesel();
			this.requiredPIDs.Add(this.RPM);
			base.DependencyPID = this.RPM;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x00203C18 File Offset: 0x00201E18
		private void Init_MAF()
		{
			if ((this.AFR_LAMBDA as PID).IsAvailable && !SharedSettings.Current.FuelFlowUseFixedAFR)
			{
				this.requiredPIDs.Add(this.AFR_LAMBDA);
			}
			if (SharedSettings.Current.DetectZeroFuelConsumption && (this.Economizer as PID).IsAvailable)
			{
				this.requiredPIDs.Add(this.Economizer);
			}
			this.Init_Diesel();
			this.requiredPIDs.Add(this.MAF);
			if (SharedSettings.Current.FilterRPM300 && this.RPM != null && this.RPM.IsAvailable)
			{
				this.requiredPIDs.Add(this.RPM);
			}
			if (this.MAF != null)
			{
				this.maf_units = this.MAF.Units;
			}
			base.DependencyPID = this.MAF;
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x00203CF4 File Offset: 0x00201EF4
		private void Init_Load_ABS()
		{
			this.requiredPIDs.Add(this.RPM);
			if ((this.AFR_LAMBDA as PID).IsAvailable && !SharedSettings.Current.FuelFlowUseFixedAFR)
			{
				this.requiredPIDs.Add(this.AFR_LAMBDA);
			}
			if (SharedSettings.Current.DetectZeroFuelConsumption && (this.Economizer as PID).IsAvailable)
			{
				this.requiredPIDs.Add(this.Economizer);
			}
			if (this.BARO_AND_AMBIENT_TEMP_AVAILABLE)
			{
				this.lowPriorityRequiredPIDs.Add(this.BARO);
				this.lowPriorityRequiredPIDs.Add(this.AMBIENT_TEMP);
			}
			this.Init_Diesel();
			this.requiredPIDs.Add(this.LOAD_ABS);
			base.DependencyPID = this.RPM;
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x00203DBD File Offset: 0x00201FBD
		private void Init_Diesel()
		{
			if (SharedSettings.Current.FuelType == FuelTypes.Diesel && this.CurrentCalculationScheme != FuelFlowCalculationSchemes.CycleConsumption && (this.LOAD_PCT as PID).IsAvailable)
			{
				this.requiredPIDs.Add(this.LOAD_PCT);
			}
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x00203DF8 File Offset: 0x00201FF8
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (this.checkRPM300 && this.RPM.Value <= 300.0)
			{
				result = 0.0;
				return true;
			}
			switch (this.CurrentCalculationScheme)
			{
			case FuelFlowCalculationSchemes.MAF:
			{
				if (SharedSettings.Current.DetectZeroFuelConsumption && (this.Economizer as PID).IsAvailable && this.Economizer.Value == 0.0)
				{
					result = 0.0;
					return true;
				}
				double num;
				if (this.maf_units == UnitsHelper.Units.kg_h)
				{
					num = this.MAF.Value;
				}
				else
				{
					num = this.MAF.Value * 3.6;
				}
				result = this.GetFuelFromAir(num) * SharedSettings.Current.FuelFlowCorrectionFactor;
				return true;
			}
			case FuelFlowCalculationSchemes.LOAD_ABS:
			{
				if (SharedSettings.Current.DetectZeroFuelConsumption && (this.Economizer as PID).IsAvailable && this.Economizer.Value == 0.0)
				{
					result = 0.0;
					return true;
				}
				double num2 = 1.184;
				if (this.BARO_AND_AMBIENT_TEMP_AVAILABLE)
				{
					double num3 = this.BARO.Value;
					if (this.BARO.Units == UnitsHelper.Units.mbar)
					{
						num3 *= 0.1;
					}
					else if (this.BARO.Units == UnitsHelper.Units.bar)
					{
						num3 *= 100.0;
					}
					num2 = PID_CalculatedPowerFromAcceleration.GetAirDensity(num3, this.AMBIENT_TEMP.Value);
				}
				if (num2 == 0.0)
				{
					num2 = 1.184;
				}
				double num4 = this.LOAD_ABS.Value / 100.0 * num2 / (SharedSettings.Current.EngineDisplacement / (double)SharedSettings.Current.EngineCylinders) * (double)SharedSettings.Current.EngineCylinders / 2.0 * this.RPM.Value / 60.0;
				result = this.GetFuelFromAir(num4) * SharedSettings.Current.FuelFlowCorrectionFactor;
				return true;
			}
			case FuelFlowCalculationSchemes.MAP:
			{
				if (SharedSettings.Current.DetectZeroFuelConsumption && (this.Economizer as PID).IsAvailable && this.Economizer.Value == 0.0)
				{
					result = 0.0;
					return true;
				}
				double num5 = this.MAP.Value;
				if (this.MAP.Units == UnitsHelper.Units.mbar)
				{
					num5 *= 0.1;
				}
				else if (this.MAP.Units == UnitsHelper.Units.bar)
				{
					num5 *= 100.0;
				}
				else if (this.MAP.Units == UnitsHelper.Units.hPa)
				{
					num5 *= 0.1;
				}
				else if (this.MAP.Units == UnitsHelper.Units.MPa)
				{
					num5 *= 1000.0;
				}
				else if (this.MAP.Units == UnitsHelper.Units.Pa)
				{
					num5 *= 0.001;
				}
				double num6 = this.RPM.Value * num5 / (this.IAT.Value + 273.15) * 2.0 / 60.0 * (this.GetVolumetricEfficiency(this.RPM.Value) / 100.0) * SharedSettings.Current.EngineDisplacement * 28.97 / 8.3144598;
				result = this.GetFuelFromAir(num6) * SharedSettings.Current.FuelFlowCorrectionFactor;
				return true;
			}
			case FuelFlowCalculationSchemes.FuelRate:
				if (this.FuelRate.Units == UnitsHelper.Units.Lh)
				{
					result = this.FuelRate.Value * SharedSettings.Current.FuelFlowCorrectionFactor;
					return true;
				}
				if (this.FuelRate.Units == UnitsHelper.Units.grams_sec)
				{
					double num7 = this.FuelRate.Value / this.fuel_density;
					double num8 = 3.6 * num7;
					result = num8 * SharedSettings.Current.FuelFlowCorrectionFactor;
					return true;
				}
				if (this.FuelRate.Units == UnitsHelper.Units.kg_h)
				{
					double num9 = this.FuelRate.Value * 1000.0 / 3600.0 / this.fuel_density;
					double num10 = 3.6 * num9;
					result = num10 * SharedSettings.Current.FuelFlowCorrectionFactor;
					return true;
				}
				result = double.NaN;
				return false;
			case FuelFlowCalculationSchemes.Injector:
			{
				if (this.last_tick.Ticks == 0L)
				{
					this.last_tick = new TimeSpan(DateTimeNowHelper.NowSafe.Ticks);
					result = 0.0;
					return false;
				}
				if (SharedSettings.Current.DetectZeroFuelConsumption && (this.Economizer as PID).IsAvailable && this.Economizer.Value == 0.0)
				{
					result = 0.0;
					return true;
				}
				double num11 = this.RPM.Value / 60.0 * ((double)(SharedSettings.Current.EngineCylinders / 2) * this.INJECTOR.Value / 1000.0) * (SharedSettings.Current.InjectorFlow / 1000.0 / 60.0) * 3600.0;
				result = num11 * SharedSettings.Current.FuelFlowCorrectionFactor;
				this.last_tick = new TimeSpan(DateTimeNowHelper.NowSafe.Ticks);
				return true;
			}
			case FuelFlowCalculationSchemes.CycleConsumption:
			{
				double value = this.RPM.Value;
				double num12 = this.CYCLE_CONSUMPTION.Value;
				UnitsHelper.Units units = this.CYCLE_CONSUMPTION.Units;
				if (units <= UnitsHelper.Units.mgpc)
				{
					if (units != UnitsHelper.Units.g_stroke)
					{
						if (units == UnitsHelper.Units.mgpc)
						{
							num12 = num12 / 1000.0 / this.fuel_density / 1000.0;
						}
					}
					else
					{
						num12 = num12 / this.fuel_density / 1000.0;
					}
				}
				else if (units != UnitsHelper.Units.mg_stroke)
				{
					if (units == UnitsHelper.Units.mm3_stroke)
					{
						num12 /= 1000000.0;
					}
				}
				else
				{
					num12 = num12 / 1000.0 / this.fuel_density / 1000.0;
				}
				double num13 = num12 * 2.0 * value * 60.0;
				result = num13 * SharedSettings.Current.FuelFlowCorrectionFactor;
				return true;
			}
			default:
				result = 0.0;
				return false;
			}
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x00204444 File Offset: 0x00202644
		private double GetFuelFromAir(double MAF)
		{
			if (this.FUEL_TYPE_PID != null)
			{
				this.SetFuelProperties();
			}
			if (SharedSettings.Current.FuelType == FuelTypes.Diesel)
			{
				MAF = MAF * this.LOAD_PCT.Value / 100.0;
			}
			return MAF / this.GetAFRLambdaValue() / this.fuel_density;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x00204494 File Offset: 0x00202694
		private double GetAFRLambdaValue()
		{
			if (SharedSettings.Current.FuelFlowUseFixedAFR)
			{
				return this.Stechiometric;
			}
			if (!(this.AFR_LAMBDA as PID).IsAvailable || this.AFR_LAMBDA.Value == 0.0 || !double.IsFinite(this.AFR_LAMBDA.Value))
			{
				return this.Stechiometric;
			}
			if (SharedSettings.Current.FuelType == FuelTypes.Gasoline && SharedSettings.Current.ShowAirFuelBasedOnStoichiometric)
			{
				if (!SharedSettings.Current.FilterWrongAFValues)
				{
					return this.AFR_LAMBDA.Value;
				}
				double value = this.AFR_LAMBDA.Value;
				if (value > 29.4 || value < 1.47)
				{
					return this.Stechiometric;
				}
				return value;
			}
			else
			{
				double num = this.AFR_LAMBDA.Value * this.Stechiometric;
				if (!SharedSettings.Current.FilterWrongAFValues)
				{
					return num;
				}
				if (num > 29.4 || num < 1.47)
				{
					return this.Stechiometric;
				}
				return num;
			}
		}

		// Token: 0x04001A09 RID: 6665
		private Dictionary<int, int> VolumetricEfficiency;

		// Token: 0x04001A0A RID: 6666
		private double Stechiometric;

		// Token: 0x04001A0B RID: 6667
		private double fuel_density;

		// Token: 0x04001A0C RID: 6668
		private double air_density;

		// Token: 0x04001A0D RID: 6669
		private double atmospheric_pressure;

		// Token: 0x04001A0E RID: 6670
		private UnitsHelper.Units maf_units = UnitsHelper.Units.grams_sec;

		// Token: 0x04001A0F RID: 6671
		private bool checkRPM300;

		// Token: 0x04001A10 RID: 6672
		private FuelFlowCalculationSchemes CurrentCalculationScheme = FuelFlowCalculationSchemes.FuelRate;

		// Token: 0x04001A11 RID: 6673
		private IPIDFloatValue FuelRate;

		// Token: 0x04001A12 RID: 6674
		private IPIDFloatValue MAF;

		// Token: 0x04001A13 RID: 6675
		private IPIDFloatValue MAP;

		// Token: 0x04001A14 RID: 6676
		private IPIDFloatValue IAT;

		// Token: 0x04001A15 RID: 6677
		private IPIDFloatValue RPM;

		// Token: 0x04001A16 RID: 6678
		private IPIDFloatValue AFR_LAMBDA;

		// Token: 0x04001A17 RID: 6679
		private IPIDFloatValue FSS;

		// Token: 0x04001A18 RID: 6680
		private IPIDFloatValue LOAD_ABS;

		// Token: 0x04001A19 RID: 6681
		private IPIDFloatValue LOAD_PCT;

		// Token: 0x04001A1A RID: 6682
		private IPIDFloatValue Economizer;

		// Token: 0x04001A1B RID: 6683
		private IPIDFloatValue BARO;

		// Token: 0x04001A1C RID: 6684
		private IPIDFloatValue AMBIENT_TEMP;

		// Token: 0x04001A1D RID: 6685
		private IPIDFloatValue O2_V1;

		// Token: 0x04001A1E RID: 6686
		private IPIDFloatValue O2_V2;

		// Token: 0x04001A1F RID: 6687
		private IPIDFloatValue INJECTOR;

		// Token: 0x04001A20 RID: 6688
		private IPIDFloatValue CYCLE_CONSUMPTION;

		// Token: 0x04001A21 RID: 6689
		private IPIDWithStringValue FUEL_TYPE_PID;

		// Token: 0x04001A22 RID: 6690
		private bool BARO_AND_AMBIENT_TEMP_AVAILABLE;

		// Token: 0x04001A23 RID: 6691
		private TimeSpan last_tick;

		// Token: 0x0200044D RID: 1101
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E37 RID: 11831 RVA: 0x0020459C File Offset: 0x0020279C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E38 RID: 11832 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E39 RID: 11833 RVA: 0x002045A8 File Offset: 0x002027A8
			internal bool <Initialize>b__29_0(PID x)
			{
				return x.Id == 123;
			}

			// Token: 0x06002E3A RID: 11834 RVA: 0x0020269D File Offset: 0x0020089D
			internal bool <Initialize>b__29_1(PID x)
			{
				return x.Command == "221209";
			}

			// Token: 0x06002E3B RID: 11835 RVA: 0x002026AF File Offset: 0x002008AF
			internal bool <Initialize>b__29_2(PID x)
			{
				return x.Id == 16;
			}

			// Token: 0x06002E3C RID: 11836 RVA: 0x002026BB File Offset: 0x002008BB
			internal bool <Initialize>b__29_3(PID x)
			{
				return x.Id == 11;
			}

			// Token: 0x06002E3D RID: 11837 RVA: 0x002026C7 File Offset: 0x002008C7
			internal bool <Initialize>b__29_4(PID x)
			{
				return x.Id == 15;
			}

			// Token: 0x06002E3E RID: 11838 RVA: 0x002026D3 File Offset: 0x002008D3
			internal bool <Initialize>b__29_5(PID x)
			{
				return x.Command == "221201";
			}

			// Token: 0x06002E3F RID: 11839 RVA: 0x002026E5 File Offset: 0x002008E5
			internal bool <Initialize>b__29_6(PID x)
			{
				return x.Id == 12;
			}

			// Token: 0x06002E40 RID: 11840 RVA: 0x002045B4 File Offset: 0x002027B4
			internal bool <Initialize>b__29_7(PID x)
			{
				return x.Id == 92;
			}

			// Token: 0x06002E41 RID: 11841 RVA: 0x002045C0 File Offset: 0x002027C0
			internal bool <Initialize>b__29_8(PID x)
			{
				return x.Command == "ECONOMIZER";
			}

			// Token: 0x06002E42 RID: 11842 RVA: 0x00202709 File Offset: 0x00200909
			internal bool <Initialize>b__29_9(PID x)
			{
				return x.Id == 91;
			}

			// Token: 0x06002E43 RID: 11843 RVA: 0x002045D2 File Offset: 0x002027D2
			internal bool <Initialize>b__29_10(PID x)
			{
				return x.Id == 4;
			}

			// Token: 0x06002E44 RID: 11844 RVA: 0x002026F1 File Offset: 0x002008F1
			internal bool <Initialize>b__29_11(PID x)
			{
				return x.Id == 67;
			}

			// Token: 0x06002E45 RID: 11845 RVA: 0x002026FD File Offset: 0x002008FD
			internal bool <Initialize>b__29_12(PID x)
			{
				return x.Id == 94;
			}

			// Token: 0x06002E46 RID: 11846 RVA: 0x002045DD File Offset: 0x002027DD
			internal bool <Initialize>b__29_13(PID x)
			{
				return x.Id == 106;
			}

			// Token: 0x04001A24 RID: 6692
			public static readonly PID_CalculatedInstantFuelRate.<>c <>9 = new PID_CalculatedInstantFuelRate.<>c();

			// Token: 0x04001A25 RID: 6693
			public static Func<PID, bool> <>9__29_0;

			// Token: 0x04001A26 RID: 6694
			public static Func<PID, bool> <>9__29_1;

			// Token: 0x04001A27 RID: 6695
			public static Func<PID, bool> <>9__29_2;

			// Token: 0x04001A28 RID: 6696
			public static Func<PID, bool> <>9__29_3;

			// Token: 0x04001A29 RID: 6697
			public static Func<PID, bool> <>9__29_4;

			// Token: 0x04001A2A RID: 6698
			public static Func<PID, bool> <>9__29_5;

			// Token: 0x04001A2B RID: 6699
			public static Func<PID, bool> <>9__29_6;

			// Token: 0x04001A2C RID: 6700
			public static Func<PID, bool> <>9__29_7;

			// Token: 0x04001A2D RID: 6701
			public static Func<PID, bool> <>9__29_8;

			// Token: 0x04001A2E RID: 6702
			public static Func<PID, bool> <>9__29_9;

			// Token: 0x04001A2F RID: 6703
			public static Func<PID, bool> <>9__29_10;

			// Token: 0x04001A30 RID: 6704
			public static Func<PID, bool> <>9__29_11;

			// Token: 0x04001A31 RID: 6705
			public static Func<PID, bool> <>9__29_12;

			// Token: 0x04001A32 RID: 6706
			public static Func<PID, bool> <>9__29_13;
		}
	}
}
