using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200045C RID: 1116
	internal class PID_EV_InstantConsumption_KWH_100KM : CalculatedPIDV2
	{
		// Token: 0x06002E85 RID: 11909 RVA: 0x00205C21 File Offset: 0x00203E21
		public PID_EV_InstantConsumption_KWH_100KM()
			: base("EV Instant Energy Consumption [kWh/100km]", "EV_INST_CONSUMPTION_KWH_100KM", UnitsHelper.Units.kWh_100km, -20.0, 20.0, Roles.EV_INSTANT_CONSUMPTION_KWH_100KM)
		{
			base.Id = 815;
			base.ShortName = base.Name;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x00205C60 File Offset: 0x00203E60
		public override void Initialize()
		{
			this.EV_INSTANT_CONSUMPTION_KM_KWH = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Role == Roles.EV_INSTANT_CONSUMPTION_KM_KWH);
			if (this.EV_INSTANT_CONSUMPTION_KM_KWH == null)
			{
				this.EV_INSTANT_CONSUMPTION_KM_KWH = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.EV_INSTANT_CONSUMPTION_KM_KWH);
			}
			this.requiredPIDs.Clear();
			this.requiredPIDs.Add(this.EV_INSTANT_CONSUMPTION_KM_KWH);
			base.DependencyPID = this.EV_INSTANT_CONSUMPTION_KM_KWH;
			base.Initialize();
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x00205CEE File Offset: 0x00203EEE
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			result = 100.0 / this.EV_INSTANT_CONSUMPTION_KM_KWH.Value;
			return true;
		}

		// Token: 0x04001A61 RID: 6753
		private IPIDFloatValue EV_INSTANT_CONSUMPTION_KM_KWH;

		// Token: 0x0200045D RID: 1117
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E88 RID: 11912 RVA: 0x00205D08 File Offset: 0x00203F08
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E89 RID: 11913 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E8A RID: 11914 RVA: 0x00205D14 File Offset: 0x00203F14
			internal bool <Initialize>b__2_0(CustomPID x)
			{
				return x.Role == Roles.EV_INSTANT_CONSUMPTION_KM_KWH;
			}

			// Token: 0x04001A62 RID: 6754
			public static readonly PID_EV_InstantConsumption_KWH_100KM.<>c <>9 = new PID_EV_InstantConsumption_KWH_100KM.<>c();

			// Token: 0x04001A63 RID: 6755
			public static Func<CustomPID, bool> <>9__2_0;
		}
	}
}
