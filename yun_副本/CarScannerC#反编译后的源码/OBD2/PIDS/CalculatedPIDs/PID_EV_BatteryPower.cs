using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000458 RID: 1112
	internal class PID_EV_BatteryPower : CalculatedPIDV2
	{
		// Token: 0x06002E77 RID: 11895 RVA: 0x002058A1 File Offset: 0x00203AA1
		public PID_EV_BatteryPower()
			: base("HV EV Battery Power", "EV_BATTERY_POWER", UnitsHelper.Units.kW, -50.0, 50.0, Roles.EV_BatteryPower)
		{
			base.Id = 801;
			base.ShortName = "HV EV Battery Power";
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x002058E0 File Offset: 0x00203AE0
		public override void Initialize()
		{
			this.VOLTAGE = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Role == Roles.EV_BatteryVoltage);
			this.CURRENT = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Role == Roles.EV_BatteryCurrent);
			if (this.VOLTAGE == null)
			{
				this.VOLTAGE = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.EV_BatteryVoltage);
			}
			if (this.CURRENT == null)
			{
				this.CURRENT = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.EV_BatteryCurrent);
			}
			this.requiredPIDs.Clear();
			this.requiredPIDs.Add(this.VOLTAGE);
			this.requiredPIDs.Add(this.CURRENT);
			base.DependencyPID = this.VOLTAGE;
			base.Initialize();
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x002059D4 File Offset: 0x00203BD4
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			double num = this.VOLTAGE.Value;
			if (this.VOLTAGE.Units == UnitsHelper.Units.mV)
			{
				num /= 1000.0;
			}
			double num2 = this.CURRENT.Value;
			if (this.CURRENT.Units == UnitsHelper.Units.mA)
			{
				num2 /= 1000.0;
			}
			if (SharedSettings.Current.EV_Power_InvertValue)
			{
				result = num2 * num / 1000.0 * -1.0;
			}
			else
			{
				result = num2 * num / 1000.0;
			}
			return true;
		}

		// Token: 0x04001A57 RID: 6743
		private IPIDFloatValue VOLTAGE;

		// Token: 0x04001A58 RID: 6744
		private IPIDFloatValue CURRENT;

		// Token: 0x02000459 RID: 1113
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E7A RID: 11898 RVA: 0x00205A66 File Offset: 0x00203C66
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E7B RID: 11899 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E7C RID: 11900 RVA: 0x00205A72 File Offset: 0x00203C72
			internal bool <Initialize>b__3_0(CustomPID x)
			{
				return x.Role == Roles.EV_BatteryVoltage;
			}

			// Token: 0x06002E7D RID: 11901 RVA: 0x00205A7E File Offset: 0x00203C7E
			internal bool <Initialize>b__3_1(CustomPID x)
			{
				return x.Role == Roles.EV_BatteryCurrent;
			}

			// Token: 0x04001A59 RID: 6745
			public static readonly PID_EV_BatteryPower.<>c <>9 = new PID_EV_BatteryPower.<>c();

			// Token: 0x04001A5A RID: 6746
			public static Func<CustomPID, bool> <>9__3_0;

			// Token: 0x04001A5B RID: 6747
			public static Func<CustomPID, bool> <>9__3_1;
		}
	}
}
