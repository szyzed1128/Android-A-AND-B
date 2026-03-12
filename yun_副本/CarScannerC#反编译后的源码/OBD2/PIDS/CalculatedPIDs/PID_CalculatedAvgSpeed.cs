using System;
using System.Diagnostics;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000444 RID: 1092
	internal class PID_CalculatedAvgSpeed : CalculatedPIDV2
	{
		// Token: 0x06002DFB RID: 11771 RVA: 0x00201928 File Offset: 0x001FFB28
		public PID_CalculatedAvgSpeed(IPIDFloatValue TotalDistancePID)
			: base(PID.GetResourceString("PID_CalculatedAvgSpeed"), "AVG_SPEED", UnitsHelper.Units.kmh, 0.0, 1.0, Roles.None)
		{
			this.TotalDistancePID = TotalDistancePID;
			base.Minimum = 0.0;
			base.Maximum = 150.0;
			base.Id = 233;
			this.sw = new Stopwatch();
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x00201999 File Offset: 0x001FFB99
		public override void ResetValues()
		{
			this.sw.Restart();
			this.total_time_ticks = 0L;
			base.ResetValues();
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x002019B4 File Offset: 0x001FFBB4
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (!this.sw.IsRunning)
			{
				this.sw.Start();
				result = 0.0;
				return false;
			}
			long ticks = this.sw.Elapsed.Ticks;
			if (ticks > 150000000L)
			{
				result = base.Value;
				this.sw.Restart();
				return false;
			}
			this.sw.Restart();
			this.total_time_ticks += ticks;
			TimeSpan timeSpan = new TimeSpan(this.total_time_ticks);
			double num = this.TotalDistancePID.Value / timeSpan.TotalHours;
			result = num;
			return true;
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x00201A56 File Offset: 0x001FFC56
		public override void Initialize()
		{
			Stopwatch stopwatch = this.sw;
			if (stopwatch != null)
			{
				stopwatch.Stop();
			}
			this.sw = new Stopwatch();
			this.total_time_ticks = 0L;
			base.DependencyPID = this.TotalDistancePID;
			base.Initialize();
		}

		// Token: 0x040019E0 RID: 6624
		private Stopwatch sw;

		// Token: 0x040019E1 RID: 6625
		private long total_time_ticks;

		// Token: 0x040019E2 RID: 6626
		private IPIDFloatValue TotalDistancePID;
	}
}
