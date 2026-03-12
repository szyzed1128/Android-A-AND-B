using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200045A RID: 1114
	internal class PID_EV_InstantConsumptioKMKWH : CalculatedPIDV2
	{
		// Token: 0x06002E7E RID: 11902 RVA: 0x00205A8A File Offset: 0x00203C8A
		public PID_EV_InstantConsumptioKMKWH()
			: base("EV Instant Energy Consumption", "EV_INST_CONSUMPTION_KM_KWH", UnitsHelper.Units.km_kwh, -20.0, 20.0, Roles.EV_INSTANT_CONSUMPTION_KM_KWH)
		{
			base.Id = 814;
			base.ShortName = "EV Instant Consumption";
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x00205AC8 File Offset: 0x00203CC8
		public override void Initialize()
		{
			this.BATT_POWER = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Role == Roles.EV_BatteryPower);
			this.SPEED = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Role == Roles.Speed);
			if (this.BATT_POWER == null)
			{
				this.BATT_POWER = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.EV_BatteryPower);
			}
			if (this.SPEED == null)
			{
				this.SPEED = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed);
			}
			this.requiredPIDs.Clear();
			this.requiredPIDs.Add(this.BATT_POWER);
			this.requiredPIDs.Add(this.SPEED);
			base.DependencyPID = this.SPEED;
			base.Initialize();
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x00205BBC File Offset: 0x00203DBC
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			double num = this.SPEED.Value;
			if (this.SPEED.Units == UnitsHelper.Units.mph)
			{
				num = UnitsHelper.Convert(num, UnitsHelper.Units.mph, UnitsHelper.Units.kmh);
			}
			double value = this.BATT_POWER.Value;
			result = num / value;
			return true;
		}

		// Token: 0x04001A5C RID: 6748
		private IPIDFloatValue BATT_POWER;

		// Token: 0x04001A5D RID: 6749
		private IPIDFloatValue SPEED;

		// Token: 0x0200045B RID: 1115
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E81 RID: 11905 RVA: 0x00205BFE File Offset: 0x00203DFE
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E82 RID: 11906 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E83 RID: 11907 RVA: 0x00205C0A File Offset: 0x00203E0A
			internal bool <Initialize>b__3_0(CustomPID x)
			{
				return x.Role == Roles.EV_BatteryPower;
			}

			// Token: 0x06002E84 RID: 11908 RVA: 0x00205C16 File Offset: 0x00203E16
			internal bool <Initialize>b__3_1(CustomPID x)
			{
				return x.Role == Roles.Speed;
			}

			// Token: 0x04001A5E RID: 6750
			public static readonly PID_EV_InstantConsumptioKMKWH.<>c <>9 = new PID_EV_InstantConsumptioKMKWH.<>c();

			// Token: 0x04001A5F RID: 6751
			public static Func<CustomPID, bool> <>9__3_0;

			// Token: 0x04001A60 RID: 6752
			public static Func<CustomPID, bool> <>9__3_1;
		}
	}
}
