using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200046B RID: 1131
	internal class PID_TotalDistanceStatistics : CalculatedPIDV2
	{
		// Token: 0x06002ED1 RID: 11985 RVA: 0x00207370 File Offset: 0x00205570
		public PID_TotalDistanceStatistics()
			: base(PID.GetResourceString("PID_TotalDistance") + PID.GetResourceString("PID_Total"), "TOTAL_DISTANCE_STATS", UnitsHelper.Units.km, 0.0, 10000.0, Roles.CALC_DistanceStatistics)
		{
			base.ShortName = PID.GetResourceString("PID_TotalDistance_Short") + PID.GetResourceString("PID_Total_Short");
			base.Id = 630;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x002073E0 File Offset: 0x002055E0
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (DriveCycle.Current != null)
			{
				result = DriveCycleViewModel.Current.TotalDistance + DriveCycle.Current.Distance;
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x00207410 File Offset: 0x00205610
		public override void Initialize()
		{
			this.ResetValues();
			this.DistancePID = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalDistance) as IPIDFloatValue;
			if (this.DistancePID != null)
			{
				this.requiredPIDs.Clear();
				base.DependencyPID = this.DistancePID;
			}
			base.Initialize();
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x00207486 File Offset: 0x00205686
		public override void ResetValues()
		{
			base.SetValue(DriveCycleViewModel.Current.TotalDistance);
		}

		// Token: 0x04001A91 RID: 6801
		private IPIDFloatValue DistancePID;

		// Token: 0x0200046C RID: 1132
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002ED5 RID: 11989 RVA: 0x00207498 File Offset: 0x00205698
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002ED6 RID: 11990 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002ED7 RID: 11991 RVA: 0x000AC03D File Offset: 0x000AA23D
			internal bool <Initialize>b__3_0(PID x)
			{
				return x is PID_TotalDistance;
			}

			// Token: 0x04001A92 RID: 6802
			public static readonly PID_TotalDistanceStatistics.<>c <>9 = new PID_TotalDistanceStatistics.<>c();

			// Token: 0x04001A93 RID: 6803
			public static Func<PID, bool> <>9__3_0;
		}
	}
}
