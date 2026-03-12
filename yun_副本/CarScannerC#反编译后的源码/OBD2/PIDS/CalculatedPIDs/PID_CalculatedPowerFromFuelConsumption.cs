using System;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000450 RID: 1104
	public class PID_CalculatedPowerFromFuelConsumption : CalculatedPIDV2
	{
		// Token: 0x06002E57 RID: 11863 RVA: 0x00204B44 File Offset: 0x00202D44
		public PID_CalculatedPowerFromFuelConsumption(PID_CalculatedInstantFuelRate CALC_FUEL_RATE)
			: base(PID.GetResourceString("PID_POWER_FROM_FUEL_CONSUMPTION"), "POWER_FROM_FUEL", UnitsHelper.Units.kW, 0.0, 1.0, Roles.None)
		{
			base.ShortName = PID.GetResourceString("PID_POWER_FROM_FUEL_CONSUMPTION_SHORT");
			base.Minimum = 0.0;
			base.Maximum = 300.0;
			this.Command = "POWER_FROM_FUEL";
			this.CALC_FUEL_RATE = CALC_FUEL_RATE;
			base.Id = 238;
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x00204BC6 File Offset: 0x00202DC6
		public override void Initialize()
		{
			this.requiredPIDs.Clear();
			base.DependencyPID = this.CALC_FUEL_RATE;
			this.ResetValues();
			base.Initialize();
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x00204BEC File Offset: 0x00202DEC
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			double value = this.CALC_FUEL_RATE.Value;
			double num = ((SharedSettings.Current.FuelType == FuelTypes.Gasoline) ? 0.73 : 0.832);
			double num2 = value * num * 1000.0 / 60.0 / 60.0;
			double num3 = ((SharedSettings.Current.FuelType == FuelTypes.Gasoline) ? 43.0 : 42.7);
			double num4 = 0.33;
			result = num2 * num4 * num3;
			return true;
		}

		// Token: 0x04001A3D RID: 6717
		private PID_CalculatedInstantFuelRate CALC_FUEL_RATE;
	}
}
