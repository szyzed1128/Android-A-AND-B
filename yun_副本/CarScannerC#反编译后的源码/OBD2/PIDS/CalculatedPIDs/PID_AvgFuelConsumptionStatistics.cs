using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200043F RID: 1087
	internal class PID_AvgFuelConsumptionStatistics : CalculatedPIDV2
	{
		// Token: 0x06002DEA RID: 11754 RVA: 0x002013A4 File Offset: 0x001FF5A4
		public PID_AvgFuelConsumptionStatistics(PID_TotalFuelUsedStatistics fuelUsed, PID_TotalDistanceStatistics distance)
			: base(PID.GetResourceString("PID_AvgFuelConsumption") + PID.GetResourceString("PID_Total"), "TOTAL_FUEL_USED_STATS", UnitsHelper.Units.liters100km, 0.0, 30.0, Roles.None)
		{
			this.FuelUsedPID = fuelUsed;
			this.DistanceTravelledPID = distance;
			base.ShortName = PID.GetResourceString("PID_AvgFuelConsumption_Short") + PID.GetResourceString("PID_Total_Short");
			base.Id = 632;
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x00201422 File Offset: 0x001FF622
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (DriveCycle.Current != null)
			{
				result = this.FuelUsedPID.Value * 100.0 / this.DistanceTravelledPID.Value;
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x0020145C File Offset: 0x001FF65C
		public override void Initialize()
		{
			base.SetValue(DriveCycleViewModel.Current.TotalDistance);
			this.FuelUsedPID = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalFuelUsedStatistics) as IPIDFloatValue;
			this.requiredPIDs.Clear();
			if (this.FuelUsedPID != null)
			{
				this.requiredPIDs.Add(this.FuelUsedPID);
			}
			this.DistanceTravelledPID = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalDistanceStatistics) as IPIDFloatValue;
			if (this.DistanceTravelledPID != null)
			{
				this.requiredPIDs.Add(this.DistanceTravelledPID);
			}
			base.DependencyPID = this.FuelUsedPID;
			base.Initialize();
		}

		// Token: 0x040019D5 RID: 6613
		private IPIDFloatValue FuelUsedPID;

		// Token: 0x040019D6 RID: 6614
		private IPIDFloatValue DistanceTravelledPID;

		// Token: 0x02000440 RID: 1088
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002DED RID: 11757 RVA: 0x00201544 File Offset: 0x001FF744
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002DEE RID: 11758 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002DEF RID: 11759 RVA: 0x00201550 File Offset: 0x001FF750
			internal bool <Initialize>b__4_0(PID x)
			{
				return x is PID_TotalFuelUsedStatistics;
			}

			// Token: 0x06002DF0 RID: 11760 RVA: 0x0020155B File Offset: 0x001FF75B
			internal bool <Initialize>b__4_1(PID x)
			{
				return x is PID_TotalDistanceStatistics;
			}

			// Token: 0x040019D7 RID: 6615
			public static readonly PID_AvgFuelConsumptionStatistics.<>c <>9 = new PID_AvgFuelConsumptionStatistics.<>c();

			// Token: 0x040019D8 RID: 6616
			public static Func<PID, bool> <>9__4_0;

			// Token: 0x040019D9 RID: 6617
			public static Func<PID, bool> <>9__4_1;
		}
	}
}
