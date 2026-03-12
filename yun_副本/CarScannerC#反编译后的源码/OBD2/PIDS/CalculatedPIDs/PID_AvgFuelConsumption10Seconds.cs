using System;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200043E RID: 1086
	internal class PID_AvgFuelConsumption10Seconds : CalculatedPIDV2
	{
		// Token: 0x06002DE7 RID: 11751 RVA: 0x0020129C File Offset: 0x001FF49C
		public PID_AvgFuelConsumption10Seconds(PID_CalculatedAVGFuelConsumption avgConsumption)
			: base(PID.GetResourceString("PID_AvgFuelConsumption") + " 10 sec", "AVG_FUEL_CONSUMPTION_10SEC", UnitsHelper.Units.liters100km, 0.0, 1.0, Roles.None)
		{
			this.avgConsumption = avgConsumption;
			base.Maximum = 50.0;
			base.Minimum = 0.0;
			this.Command = "AVG_FUEL_CONSUMPTION_10SEC";
			base.ShortName = Translate.GetString("PID_AvgFuelConsumption_Short") + " 10 sec";
			base.Id = 804;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x0020134B File Offset: 0x001FF54B
		public override void Initialize()
		{
			this.ResetValues();
			this.requiredPIDs.Add(this.avgConsumption);
			base.DependencyPID = this.avgConsumption;
			base.Initialize();
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x00201376 File Offset: 0x001FF576
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			this.valueContainer.AddLast(new DoubleValueItem(pid.Value, pid.TimeStamp));
			result = this.valueContainer.GetAverage();
			return true;
		}

		// Token: 0x040019D3 RID: 6611
		private PID_CalculatedAVGFuelConsumption avgConsumption;

		// Token: 0x040019D4 RID: 6612
		private DoubleValueTimeLimitedList valueContainer = new DoubleValueTimeLimitedList(TimeSpan.FromSeconds(10.0));
	}
}
