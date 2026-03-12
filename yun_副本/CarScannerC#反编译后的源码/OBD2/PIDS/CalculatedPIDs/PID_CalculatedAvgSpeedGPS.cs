using System;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000445 RID: 1093
	internal class PID_CalculatedAvgSpeedGPS : CalculatedPIDV2
	{
		// Token: 0x06002DFF RID: 11775 RVA: 0x00201A90 File Offset: 0x001FFC90
		public PID_CalculatedAvgSpeedGPS(IPIDFloatValue GPSSpeedPID)
			: base(PID.GetResourceString("PID_CalculatedAvgSpeedGPS"), "AVG_SPEED_GPS", UnitsHelper.Units.kmh, 0.0, 1.0, Roles.None)
		{
			this.GPSSpeedPID = GPSSpeedPID;
			base.Minimum = 0.0;
			base.Maximum = 150.0;
			this.Command = "AVG_SPEED_GPS";
			base.Id = 543;
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x00201B01 File Offset: 0x001FFD01
		// (set) Token: 0x06002E01 RID: 11777 RVA: 0x001E8DEC File Offset: 0x001E6FEC
		public override bool IsAvailable
		{
			get
			{
				return this.GPSSpeedPID != null && this.GPSSpeedPID.IsAvailable;
			}
			set
			{
				base.IsAvailable = value;
			}
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x00201B18 File Offset: 0x001FFD18
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (this.TicksLastRead == 0L)
			{
				this.TicksLastRead = DateTimeNowHelper.NowSafe.Ticks;
				result = 0.0;
				return false;
			}
			long ticks = DateTimeNowHelper.NowSafe.Ticks;
			TimeSpan timeSpan = new TimeSpan(ticks - this.TicksLastRead);
			this.TicksLastRead = ticks;
			double num = this.GPSSpeedPID.Value * timeSpan.TotalHours;
			this.S += num;
			this.TimeSinceStart += timeSpan.Ticks;
			result = this.S / new TimeSpan(this.TimeSinceStart).TotalHours;
			return true;
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x00201BC5 File Offset: 0x001FFDC5
		public override void Initialize()
		{
			this.ResetValues();
			base.DependencyPID = this.GPSSpeedPID;
			base.Initialize();
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x00201BDF File Offset: 0x001FFDDF
		public override void ResetValues()
		{
			base.SetValue(0.0);
			this.TicksLastRead = 0L;
			this.speed_sum = 0L;
			this.S = 0.0;
			this.TimeSinceStart = 0L;
		}

		// Token: 0x040019E3 RID: 6627
		private IPIDFloatValue GPSSpeedPID;

		// Token: 0x040019E4 RID: 6628
		private long speed_sum;

		// Token: 0x040019E5 RID: 6629
		private long TicksLastRead;

		// Token: 0x040019E6 RID: 6630
		private double S;

		// Token: 0x040019E7 RID: 6631
		private long TimeSinceStart;
	}
}
