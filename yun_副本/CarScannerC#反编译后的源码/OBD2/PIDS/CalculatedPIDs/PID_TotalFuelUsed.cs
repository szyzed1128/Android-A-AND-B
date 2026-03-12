using System;
using System.Diagnostics;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200046D RID: 1133
	internal class PID_TotalFuelUsed : CalculatedPIDV2
	{
		// Token: 0x06002ED8 RID: 11992 RVA: 0x002074A4 File Offset: 0x002056A4
		public PID_TotalFuelUsed(PID_CalculatedInstantFuelRate PID_FuelRate)
			: base(PID.GetResourceString("PID_TotalFuelUsed"), "TOTAL_FUEL_USED", UnitsHelper.Units.liters, 0.0, 1.0, Roles.None)
		{
			this.PID_FuelRate = PID_FuelRate;
			base.Minimum = 0.0;
			base.Maximum = 40.0;
			this.Command = "TOTAL_FUEL_USED";
			this.sw = new Stopwatch();
			base.Id = 232;
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x00207521 File Offset: 0x00205721
		public override void ResetValues()
		{
			this.S = 0.0;
			this.sw.Restart();
			base.ResetValues();
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x00207544 File Offset: 0x00205744
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (!this.sw.IsRunning)
			{
				this.sw.Start();
				result = 0.0;
				return false;
			}
			result = 0.0;
			TimeSpan elapsed = this.sw.Elapsed;
			this.sw.Restart();
			if (elapsed.TotalSeconds > 15.0)
			{
				return false;
			}
			double num = this.PID_FuelRate.Value * elapsed.TotalHours;
			if (double.IsFinite(num))
			{
				this.S += num;
				DriveCycle.Current.RecordFuel(num, pid.TimeStamp);
			}
			result = this.S;
			return true;
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x002075F0 File Offset: 0x002057F0
		public override void Initialize()
		{
			this.requiredPIDs.Clear();
			base.DependencyPID = this.PID_FuelRate;
			this.S = 0.0;
			this.sw.Reset();
			base.Initialize();
		}

		// Token: 0x04001A94 RID: 6804
		private PID_CalculatedInstantFuelRate PID_FuelRate;

		// Token: 0x04001A95 RID: 6805
		private Stopwatch sw;

		// Token: 0x04001A96 RID: 6806
		private double S;
	}
}
