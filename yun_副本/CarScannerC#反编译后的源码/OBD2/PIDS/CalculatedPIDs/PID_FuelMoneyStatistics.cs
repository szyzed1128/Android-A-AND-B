using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000463 RID: 1123
	internal class PID_FuelMoneyStatistics : CalculatedPIDV2
	{
		// Token: 0x06002E98 RID: 11928 RVA: 0x00206250 File Offset: 0x00204450
		public PID_FuelMoneyStatistics(PID_FuelMoney FuelMoneyPID)
			: base(PID.GetResourceString("PID_FuelMoney") + PID.GetResourceString("PID_Total"), "FUEL_MONEY_STATISTICS", UnitsHelper.Units.money, 0.0, 1.0, Roles.None)
		{
			base.Minimum = 0.0;
			base.Maximum = 100000.0;
			this.Command = "FUEL_MONEY_STATISTICS";
			base.ShortName = PID.GetResourceString("PID_FuelMoney") + PID.GetResourceString("PID_Total");
			this.FuelMoneyPID = FuelMoneyPID;
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x002062E5 File Offset: 0x002044E5
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (DriveCycle.Current != null)
			{
				result = (double)(DriveCycleViewModel.Current.TotalFuelPrice + DriveCycle.Current.TotalFuelPrice);
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x00206320 File Offset: 0x00204520
		public override void Initialize()
		{
			this.ResetValues();
			this.FuelMoneyPID = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_FuelMoney) as IPIDFloatValue;
			this.requiredPIDs.Clear();
			if (this.FuelMoneyPID != null)
			{
				this.requiredPIDs.Add(this.FuelMoneyPID);
				base.DependencyPID = this.FuelMoneyPID;
			}
			base.Initialize();
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x002063A7 File Offset: 0x002045A7
		public override void ResetValues()
		{
			base.SetValue((double)DriveCycleViewModel.Current.TotalFuelPrice);
		}

		// Token: 0x04001A69 RID: 6761
		private IPIDFloatValue FuelMoneyPID;

		// Token: 0x02000464 RID: 1124
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E9C RID: 11932 RVA: 0x002063BF File Offset: 0x002045BF
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E9D RID: 11933 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E9E RID: 11934 RVA: 0x002063CB File Offset: 0x002045CB
			internal bool <Initialize>b__3_0(PID x)
			{
				return x is PID_FuelMoney;
			}

			// Token: 0x04001A6A RID: 6762
			public static readonly PID_FuelMoneyStatistics.<>c <>9 = new PID_FuelMoneyStatistics.<>c();

			// Token: 0x04001A6B RID: 6763
			public static Func<PID, bool> <>9__3_0;
		}
	}
}
