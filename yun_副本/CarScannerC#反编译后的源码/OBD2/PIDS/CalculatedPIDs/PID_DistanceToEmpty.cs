using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000454 RID: 1108
	internal class PID_DistanceToEmpty : CalculatedPIDV2
	{
		// Token: 0x06002E63 RID: 11875 RVA: 0x00204F90 File Offset: 0x00203190
		public PID_DistanceToEmpty()
			: base(PID.GetResourceString("PID_DistanceToEmpty"), "DISTANCE_TO_EMPTY", UnitsHelper.Units.km, 0.0, 1.0, Roles.None)
		{
			base.Minimum = 0.0;
			base.Maximum = 100000.0;
			this.Command = "DISTANCE_TO_EMPTY";
			base.ShortName = PID.GetResourceString("PID_DistanceToEmpty_Short");
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x00205000 File Offset: 0x00203200
		public override void Initialize()
		{
			this.requiredPIDs.Clear();
			this.FUEL_LEVEL = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.FuelLevelInputLiters);
			if (this.FUEL_LEVEL == null)
			{
				this.FUEL_LEVEL = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.FuelLevelInputPercent);
			}
			if (this.FUEL_LEVEL == null)
			{
				this.FUEL_LEVEL = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 63) as IPIDFloatValue;
			}
			this.requiredPIDs.Add(this.FUEL_LEVEL as PID);
			this.AVG_CONSUMTPION_STATS = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_AvgFuelConsumptionStatistics) as IPIDFloatValue;
			if (this.AVG_CONSUMTPION_STATS != null)
			{
				this.requiredPIDs.Add(this.AVG_CONSUMTPION_STATS as PID);
			}
			this.CURRENT_DISTANCE = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalDistance) as IPIDFloatValue;
			this.CURRENT_AVG_CONSUMPTION = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption) as IPIDFloatValue;
			this.TOTAL_DISTANCE_STATS = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalDistanceStatistics) as IPIDFloatValue;
			base.DependencyPID = this.AVG_CONSUMTPION_STATS;
			if (this.FUEL_LEVEL != null && this.AVG_CONSUMTPION_STATS != null && this.FUEL_LEVEL.IsAvailable && this.AVG_CONSUMTPION_STATS.IsAvailable)
			{
				this.IsAvailable = true;
			}
			else
			{
				this.IsAvailable = false;
			}
			base.Initialize();
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x0020520C File Offset: 0x0020340C
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (this.AVG_CONSUMTPION_STATS == null || this.FUEL_LEVEL == null)
			{
				result = 0.0;
				return false;
			}
			double num = ((this.TOTAL_DISTANCE_STATS != null) ? this.TOTAL_DISTANCE_STATS.Value : 0.0);
			if (!double.IsFinite(num) || num < 1.0)
			{
				result = double.NaN;
				return true;
			}
			double num2;
			if (this.CURRENT_DISTANCE != null && this.CURRENT_AVG_CONSUMPTION != null && this.CURRENT_DISTANCE.Value >= 20.0)
			{
				double value = this.CURRENT_AVG_CONSUMPTION.Value;
				num2 = 100.0 / value;
			}
			else
			{
				double value2 = this.AVG_CONSUMTPION_STATS.Value;
				num2 = 100.0 / value2;
			}
			double num3 = 0.0;
			if (this.FUEL_LEVEL.Role == Roles.FuelLevelInputLiters)
			{
				num3 = this.FUEL_LEVEL.Value;
			}
			else if (this.FUEL_LEVEL.Role == Roles.FuelLevelInputPercent)
			{
				double num4;
				if (SharedSettings.Current.UseLitersForVolume)
				{
					num4 = SharedSettings.Current.FuelTankCapacity;
				}
				else if (SharedSettings.Current.UseUSGallon)
				{
					num4 = SharedSettings.Current.FuelTankCapacity * 3.78541178;
				}
				else
				{
					num4 = SharedSettings.Current.FuelTankCapacity * 4.5461;
				}
				num3 = num4 * this.FUEL_LEVEL.Value / 100.0;
			}
			result = num2 * num3;
			return true;
		}

		// Token: 0x04001A42 RID: 6722
		private IPIDFloatValue FUEL_LEVEL;

		// Token: 0x04001A43 RID: 6723
		private IPIDFloatValue AVG_CONSUMTPION_STATS;

		// Token: 0x04001A44 RID: 6724
		private IPIDFloatValue CURRENT_DISTANCE;

		// Token: 0x04001A45 RID: 6725
		private IPIDFloatValue CURRENT_AVG_CONSUMPTION;

		// Token: 0x04001A46 RID: 6726
		private IPIDFloatValue TOTAL_DISTANCE_STATS;

		// Token: 0x02000455 RID: 1109
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E66 RID: 11878 RVA: 0x00205383 File Offset: 0x00203583
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E67 RID: 11879 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E68 RID: 11880 RVA: 0x00204B35 File Offset: 0x00202D35
			internal bool <Initialize>b__6_0(PID x)
			{
				return x.Id == 63;
			}

			// Token: 0x06002E69 RID: 11881 RVA: 0x0020538F File Offset: 0x0020358F
			internal bool <Initialize>b__6_1(PID x)
			{
				return x is PID_AvgFuelConsumptionStatistics;
			}

			// Token: 0x06002E6A RID: 11882 RVA: 0x000AC03D File Offset: 0x000AA23D
			internal bool <Initialize>b__6_2(PID x)
			{
				return x is PID_TotalDistance;
			}

			// Token: 0x06002E6B RID: 11883 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <Initialize>b__6_3(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x06002E6C RID: 11884 RVA: 0x0020155B File Offset: 0x001FF75B
			internal bool <Initialize>b__6_4(PID x)
			{
				return x is PID_TotalDistanceStatistics;
			}

			// Token: 0x04001A47 RID: 6727
			public static readonly PID_DistanceToEmpty.<>c <>9 = new PID_DistanceToEmpty.<>c();

			// Token: 0x04001A48 RID: 6728
			public static Func<PID, bool> <>9__6_0;

			// Token: 0x04001A49 RID: 6729
			public static Func<PID, bool> <>9__6_1;

			// Token: 0x04001A4A RID: 6730
			public static Func<PID, bool> <>9__6_2;

			// Token: 0x04001A4B RID: 6731
			public static Func<PID, bool> <>9__6_3;

			// Token: 0x04001A4C RID: 6732
			public static Func<PID, bool> <>9__6_4;
		}
	}
}
