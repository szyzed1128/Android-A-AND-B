using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200046E RID: 1134
	internal class PID_TotalFuelUsedStatistics : CalculatedPIDV2
	{
		// Token: 0x06002EDC RID: 11996 RVA: 0x0020762C File Offset: 0x0020582C
		public PID_TotalFuelUsedStatistics(PID_TotalFuelUsed FuelUsedPID)
			: base(PID.GetResourceString("PID_TotalFuelUsed") + PID.GetResourceString("PID_Total"), "TOTAL_FUEL_USED_STATS", UnitsHelper.Units.liters, 0.0, 1.0, Roles.None)
		{
			base.Minimum = 0.0;
			base.Maximum = 100000.0;
			this.Command = "TOTAL_FUEL_USED_STATS";
			base.ShortName = PID.GetResourceString("PID_TotalFuelUsed") + PID.GetResourceString("PID_Total_Short");
			this.FuelUsedPID = FuelUsedPID;
			base.Id = 631;
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x002076CC File Offset: 0x002058CC
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (DriveCycle.Current != null)
			{
				result = DriveCycleViewModel.Current.TotalFuelUsed + DriveCycle.Current.FuelUsed;
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x002076FC File Offset: 0x002058FC
		public override void Initialize()
		{
			base.SetValue(DriveCycleViewModel.Current.TotalFuelUsed);
			this.FuelUsedPID = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalFuelUsed) as IPIDFloatValue;
			if (this.FuelUsedPID != null)
			{
				base.DependencyPID = this.FuelUsedPID;
			}
			base.Initialize();
		}

		// Token: 0x04001A97 RID: 6807
		private IPIDFloatValue FuelUsedPID;

		// Token: 0x0200046F RID: 1135
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002EDF RID: 11999 RVA: 0x00207771 File Offset: 0x00205971
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002EE0 RID: 12000 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002EE1 RID: 12001 RVA: 0x000AC048 File Offset: 0x000AA248
			internal bool <Initialize>b__3_0(PID x)
			{
				return x is PID_TotalFuelUsed;
			}

			// Token: 0x04001A98 RID: 6808
			public static readonly PID_TotalFuelUsedStatistics.<>c <>9 = new PID_TotalFuelUsedStatistics.<>c();

			// Token: 0x04001A99 RID: 6809
			public static Func<PID, bool> <>9__3_0;
		}
	}
}
