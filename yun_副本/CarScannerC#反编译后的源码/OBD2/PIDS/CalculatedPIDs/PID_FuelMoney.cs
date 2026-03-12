using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000461 RID: 1121
	internal class PID_FuelMoney : CalculatedPIDV2
	{
		// Token: 0x06002E92 RID: 11922 RVA: 0x00206110 File Offset: 0x00204310
		public PID_FuelMoney(PID_TotalFuelUsed FuelUsedPID)
			: base(PID.GetResourceString("PID_FuelMoney"), "FUEL_MONEY", UnitsHelper.Units.money, 0.0, 1.0, Roles.None)
		{
			base.Minimum = 0.0;
			base.Maximum = 100000.0;
			this.Command = "FUEL_MONEY";
			base.ShortName = PID.GetResourceString("PID_FuelMoney");
			this.FuelUsedPID = FuelUsedPID;
			base.Id = 633;
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x00206192 File Offset: 0x00204392
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			result = this.FuelUsedPID.Value * (double)SharedSettings.Current.FuelPriceForLitre;
			return true;
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x002061B4 File Offset: 0x002043B4
		public override void Initialize()
		{
			base.SetValue(0.0);
			this.FuelUsedPID = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalFuelUsed) as IPIDFloatValue;
			this.requiredPIDs.Clear();
			if (this.FuelUsedPID != null)
			{
				this.requiredPIDs.Add(this.FuelUsedPID);
				base.DependencyPID = this.FuelUsedPID;
			}
			base.Initialize();
		}

		// Token: 0x04001A66 RID: 6758
		private IPIDFloatValue FuelUsedPID;

		// Token: 0x02000462 RID: 1122
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E95 RID: 11925 RVA: 0x00206244 File Offset: 0x00204444
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E96 RID: 11926 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E97 RID: 11927 RVA: 0x000AC048 File Offset: 0x000AA248
			internal bool <Initialize>b__3_0(PID x)
			{
				return x is PID_TotalFuelUsed;
			}

			// Token: 0x04001A67 RID: 6759
			public static readonly PID_FuelMoney.<>c <>9 = new PID_FuelMoney.<>c();

			// Token: 0x04001A68 RID: 6760
			public static Func<PID, bool> <>9__3_0;
		}
	}
}
