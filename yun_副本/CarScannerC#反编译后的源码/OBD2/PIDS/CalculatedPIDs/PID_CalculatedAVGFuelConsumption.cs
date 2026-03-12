using System;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000443 RID: 1091
	internal class PID_CalculatedAVGFuelConsumption : CalculatedPIDV2
	{
		// Token: 0x06002DF8 RID: 11768 RVA: 0x0020183C File Offset: 0x001FFA3C
		public PID_CalculatedAVGFuelConsumption(PID_TotalDistance distancePID, PID_TotalFuelUsed fuelUsedPID)
			: base(PID.GetResourceString("PID_AvgFuelConsumption"), "AVG_FUEL_CONSUMPTION", UnitsHelper.Units.liters100km, 0.0, 1.0, Roles.None)
		{
			this.distancePID = distancePID;
			this.fuelUsedPID = fuelUsedPID;
			base.Maximum = 50.0;
			base.Minimum = 0.0;
			this.Command = "AVG_FUEL_CONSUMPTION";
			base.ShortName = Translate.GetString("PID_AvgFuelConsumption_Short");
			base.Id = 234;
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x002018C5 File Offset: 0x001FFAC5
		public override void Initialize()
		{
			this.ResetValues();
			this.requiredPIDs.Add(this.distancePID);
			this.requiredPIDs.Add(this.fuelUsedPID);
			base.DependencyPID = this.fuelUsedPID;
			base.Initialize();
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x00201901 File Offset: 0x001FFB01
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			result = this.fuelUsedPID.Value / this.distancePID.Value * 100.0;
			return true;
		}

		// Token: 0x040019DE RID: 6622
		private PID_TotalDistance distancePID;

		// Token: 0x040019DF RID: 6623
		private PID_TotalFuelUsed fuelUsedPID;
	}
}
