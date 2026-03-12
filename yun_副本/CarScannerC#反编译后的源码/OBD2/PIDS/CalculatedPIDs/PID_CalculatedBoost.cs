using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000446 RID: 1094
	internal class PID_CalculatedBoost : CalculatedPIDV2
	{
		// Token: 0x06002E05 RID: 11781 RVA: 0x00201C18 File Offset: 0x001FFE18
		public PID_CalculatedBoost()
			: base(PID.GetResourceString("PID_CalculatedBoost"), "CALC_BOOST", SharedSettings.Current.Pressure_use_kpa ? UnitsHelper.Units.bar : UnitsHelper.Units.psi, 0.0, 1.0, Roles.None)
		{
			base.Minimum = 0.0;
			base.Maximum = 200.0;
			this.Command = "CALC_BOOST";
			base.Id = 237;
			this.LowPriorityInterval = 300;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x00201CA0 File Offset: 0x001FFEA0
		public override void Initialize()
		{
			List<PID> liveDataPIDs = App.OBDReader.CurrentCarData.LiveDataPIDs;
			this.requiredPIDs.Clear();
			this.lowPriorityRequiredPIDs.Clear();
			if (SharedSettings.Current.Pressure_use_kpa)
			{
				base.Units = UnitsHelper.Units.bar;
			}
			else
			{
				base.Units = UnitsHelper.Units.psi;
			}
			this.BARO_AND_AMBIENT_TEMP_AVAILABLE = false;
			this.air_density = 1.2041;
			this.atmospheric_pressure = 101.325;
			this.VolumetricEfficiency = new Dictionary<int, int>();
			this.VolumetricEfficiency.Add(1000, SharedSettings.Current.VE1000);
			this.VolumetricEfficiency.Add(2000, SharedSettings.Current.VE2000);
			this.VolumetricEfficiency.Add(3000, SharedSettings.Current.VE3000);
			this.VolumetricEfficiency.Add(4000, SharedSettings.Current.VE4000);
			this.VolumetricEfficiency.Add(5000, SharedSettings.Current.VE5000);
			this.VolumetricEfficiency.Add(6000, SharedSettings.Current.VE6000);
			this.VolumetricEfficiency.Add(7000, SharedSettings.Current.VE7000);
			this.VolumetricEfficiency.Add(8000, SharedSettings.Current.VE8000);
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
			this.LOAD_ABS = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.LOAD_ABS);
			if (this.LOAD_ABS == null)
			{
				this.LOAD_ABS = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 91) as IPIDFloatValue;
			}
			if (SharedSettings.Current.BoostCalculationMethod == BoostCalculationMethods.Auto)
			{
				if (this.MAP != null && this.MAP.IsAvailable)
				{
					this.boostCalculationMethod = BoostCalculationMethods.MAP;
				}
				else if (this.LOAD_ABS != null && this.LOAD_ABS.IsAvailable && this.RPM != null && this.RPM.IsAvailable)
				{
					this.boostCalculationMethod = BoostCalculationMethods.LOAD_ABS;
				}
				else
				{
					if (this.MAF == null || !this.MAF.IsAvailable || this.IAT == null || !this.IAT.IsAvailable || this.RPM == null || !this.RPM.IsAvailable)
					{
						this.IsAvailable = false;
						base.Initialize();
						return;
					}
					this.boostCalculationMethod = BoostCalculationMethods.MAF;
				}
			}
			else
			{
				this.boostCalculationMethod = SharedSettings.Current.BoostCalculationMethod;
			}
			switch (this.boostCalculationMethod)
			{
			case BoostCalculationMethods.MAP:
				this.Init_MAP();
				break;
			case BoostCalculationMethods.MAF:
				this.Init_MAF();
				break;
			case BoostCalculationMethods.LOAD_ABS:
				this.Init_LOAD_ABS();
				break;
			}
			base.Initialize();
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x00202194 File Offset: 0x00200394
		private void Init_LOAD_ABS()
		{
			if (this.RPM != null)
			{
				this.requiredPIDs.Add(this.RPM);
			}
			if (this.LOAD_ABS != null)
			{
				this.requiredPIDs.Add(this.LOAD_ABS);
			}
			if (this.BARO != null && this.BARO.IsAvailable && this.AMBIENT_TEMP != null && this.AMBIENT_TEMP.IsAvailable)
			{
				this.lowPriorityRequiredPIDs.Add(this.BARO);
				this.lowPriorityRequiredPIDs.Add(this.AMBIENT_TEMP);
				this.BARO_AND_AMBIENT_TEMP_AVAILABLE = true;
			}
			else
			{
				this.BARO_AND_AMBIENT_TEMP_AVAILABLE = false;
			}
			base.DependencyPID = this.LOAD_ABS;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x0020223C File Offset: 0x0020043C
		private void Init_MAF()
		{
			if (this.MAF != null)
			{
				this.requiredPIDs.Add(this.MAF as PID);
			}
			if (this.RPM != null)
			{
				this.requiredPIDs.Add(this.RPM as PID);
			}
			if (this.IAT != null)
			{
				this.lowPriorityRequiredPIDs.Add(this.IAT);
			}
			base.DependencyPID = this.MAF;
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x002022AC File Offset: 0x002004AC
		private void Init_MAP()
		{
			if (this.MAP != null)
			{
				this.requiredPIDs.Add(this.MAP as PID);
			}
			if (this.BARO != null && this.BARO.IsAvailable)
			{
				this.lowPriorityRequiredPIDs.Add(this.BARO);
			}
			base.DependencyPID = this.MAP;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x0020230C File Offset: 0x0020050C
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

		// Token: 0x06002E0B RID: 11787 RVA: 0x002023D4 File Offset: 0x002005D4
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			switch (this.boostCalculationMethod)
			{
			case BoostCalculationMethods.MAP:
				result = this.GetBoostFromMAP(this.MAP.Value);
				return true;
			case BoostCalculationMethods.MAF:
			{
				double num;
				if (this.MAF.Units == UnitsHelper.Units.kg_h)
				{
					num = this.GetMAPFromMAF(this.MAF.Value * 2.777777777778);
				}
				else
				{
					num = this.GetMAPFromMAF(this.MAF.Value);
				}
				if (double.IsNaN(num))
				{
					result = double.NaN;
				}
				else
				{
					double boostFromMAP = this.GetBoostFromMAP(num);
					result = boostFromMAP;
				}
				return true;
			}
			case BoostCalculationMethods.LOAD_ABS:
			{
				if (this.BARO_AND_AMBIENT_TEMP_AVAILABLE)
				{
					this.air_density = PID_CalculatedPowerFromAcceleration.GetAirDensity(this.BARO.Value, this.AMBIENT_TEMP.Value);
				}
				if (this.air_density == 0.0)
				{
					this.air_density = 1.184;
				}
				double num2 = this.LOAD_ABS.Value / 100.0 * this.air_density / (SharedSettings.Current.EngineDisplacement / (double)SharedSettings.Current.EngineCylinders) * (double)SharedSettings.Current.EngineCylinders / 2.0 * this.RPM.Value / 60.0;
				double mapfromMAF = this.GetMAPFromMAF(num2);
				if (double.IsNaN(mapfromMAF))
				{
					result = double.NaN;
				}
				else
				{
					result = this.GetBoostFromMAP(mapfromMAF);
				}
				return true;
			}
			default:
				result = 0.0;
				return false;
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x00202558 File Offset: 0x00200758
		private double GetBoostFromMAP(double map)
		{
			double num;
			if (this.BARO != null && this.BARO.IsAvailable && this.BARO.Value > 0.0)
			{
				num = map - this.BARO.Value;
			}
			else
			{
				num = map - 101.325;
			}
			double num2 = num;
			if (base.Units == UnitsHelper.Units.psi)
			{
				num2 = num * 0.14503773800722;
			}
			else if (base.Units == UnitsHelper.Units.bar)
			{
				num2 = num * 0.01;
			}
			return num2;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x002025E0 File Offset: 0x002007E0
		private double GetMAPFromMAF(double maf)
		{
			if (this.IAT == null || !this.IAT.IsAvailable || this.RPM == null || !this.RPM.IsAvailable)
			{
				return double.NaN;
			}
			double value = this.IAT.Value;
			double value2 = this.RPM.Value;
			double num = 28.96;
			double num2 = 8.314459848;
			return maf * (value + 273.0) / (num / num2 * (value2 / 60.0) * (SharedSettings.Current.EngineDisplacement / 2.0) * (this.GetVolumetricEfficiency(value2) / 100.0));
		}

		// Token: 0x040019E8 RID: 6632
		private IPIDFloatValue MAP;

		// Token: 0x040019E9 RID: 6633
		private IPIDFloatValue BARO;

		// Token: 0x040019EA RID: 6634
		private IPIDFloatValue IAT;

		// Token: 0x040019EB RID: 6635
		private IPIDFloatValue RPM;

		// Token: 0x040019EC RID: 6636
		private IPIDFloatValue LOAD_ABS;

		// Token: 0x040019ED RID: 6637
		private IPIDFloatValue LOAD_PCT;

		// Token: 0x040019EE RID: 6638
		private IPIDFloatValue MAF;

		// Token: 0x040019EF RID: 6639
		private IPIDFloatValue AMBIENT_TEMP;

		// Token: 0x040019F0 RID: 6640
		private BoostCalculationMethods boostCalculationMethod;

		// Token: 0x040019F1 RID: 6641
		private bool BARO_AND_AMBIENT_TEMP_AVAILABLE;

		// Token: 0x040019F2 RID: 6642
		private double fuel_density;

		// Token: 0x040019F3 RID: 6643
		private double air_density;

		// Token: 0x040019F4 RID: 6644
		private double atmospheric_pressure;

		// Token: 0x040019F5 RID: 6645
		private Dictionary<int, int> VolumetricEfficiency;

		// Token: 0x02000447 RID: 1095
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E0E RID: 11790 RVA: 0x00202691 File Offset: 0x00200891
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E0F RID: 11791 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E10 RID: 11792 RVA: 0x0020269D File Offset: 0x0020089D
			internal bool <Initialize>b__15_0(PID x)
			{
				return x.Command == "221209";
			}

			// Token: 0x06002E11 RID: 11793 RVA: 0x002026AF File Offset: 0x002008AF
			internal bool <Initialize>b__15_1(PID x)
			{
				return x.Id == 16;
			}

			// Token: 0x06002E12 RID: 11794 RVA: 0x002026BB File Offset: 0x002008BB
			internal bool <Initialize>b__15_2(PID x)
			{
				return x.Id == 11;
			}

			// Token: 0x06002E13 RID: 11795 RVA: 0x002026C7 File Offset: 0x002008C7
			internal bool <Initialize>b__15_3(PID x)
			{
				return x.Id == 15;
			}

			// Token: 0x06002E14 RID: 11796 RVA: 0x002026D3 File Offset: 0x002008D3
			internal bool <Initialize>b__15_4(PID x)
			{
				return x.Command == "221201";
			}

			// Token: 0x06002E15 RID: 11797 RVA: 0x002026E5 File Offset: 0x002008E5
			internal bool <Initialize>b__15_5(PID x)
			{
				return x.Id == 12;
			}

			// Token: 0x06002E16 RID: 11798 RVA: 0x002026F1 File Offset: 0x002008F1
			internal bool <Initialize>b__15_6(PID x)
			{
				return x.Id == 67;
			}

			// Token: 0x06002E17 RID: 11799 RVA: 0x002026FD File Offset: 0x002008FD
			internal bool <Initialize>b__15_7(PID x)
			{
				return x.Id == 94;
			}

			// Token: 0x06002E18 RID: 11800 RVA: 0x00202709 File Offset: 0x00200909
			internal bool <Initialize>b__15_8(PID x)
			{
				return x.Id == 91;
			}

			// Token: 0x040019F6 RID: 6646
			public static readonly PID_CalculatedBoost.<>c <>9 = new PID_CalculatedBoost.<>c();

			// Token: 0x040019F7 RID: 6647
			public static Func<PID, bool> <>9__15_0;

			// Token: 0x040019F8 RID: 6648
			public static Func<PID, bool> <>9__15_1;

			// Token: 0x040019F9 RID: 6649
			public static Func<PID, bool> <>9__15_2;

			// Token: 0x040019FA RID: 6650
			public static Func<PID, bool> <>9__15_3;

			// Token: 0x040019FB RID: 6651
			public static Func<PID, bool> <>9__15_4;

			// Token: 0x040019FC RID: 6652
			public static Func<PID, bool> <>9__15_5;

			// Token: 0x040019FD RID: 6653
			public static Func<PID, bool> <>9__15_6;

			// Token: 0x040019FE RID: 6654
			public static Func<PID, bool> <>9__15_7;

			// Token: 0x040019FF RID: 6655
			public static Func<PID, bool> <>9__15_8;
		}
	}
}
