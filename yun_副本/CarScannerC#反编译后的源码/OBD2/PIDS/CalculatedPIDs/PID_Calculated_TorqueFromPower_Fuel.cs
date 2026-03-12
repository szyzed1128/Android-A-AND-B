using System;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000453 RID: 1107
	public class PID_Calculated_TorqueFromPower_Fuel : CalculatedPIDV2
	{
		// Token: 0x06002E60 RID: 11872 RVA: 0x00204E5C File Offset: 0x0020305C
		public PID_Calculated_TorqueFromPower_Fuel(PID_CalculatedPowerFromFuelConsumption power)
			: base(PID.GetResourceString("PID_TORQUE_FUEL"), "POWER_FROM_FUEL", UnitsHelper.Units.Nm, 0.0, 300.0, Roles.None)
		{
			base.ShortName = PID.GetResourceString("PID_TORQUE_FUEL_SHORT");
			base.Minimum = 0.0;
			base.Maximum = 300.0;
			this.Command = "POWER_FROM_FUEL";
			this.POWER = power;
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x00204ED4 File Offset: 0x002030D4
		public override void Initialize()
		{
			this.RPM = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.RPM);
			this.requiredPIDs.Add(this.POWER);
			this.requiredPIDs.Add(this.RPM);
			base.DependencyPID = this.POWER;
			this.ResetValues();
			base.Initialize();
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x00204F34 File Offset: 0x00203134
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			double num = this.POWER.Value * 0.73549875 / 1.35962 / (6.28318 * this.RPM.Value / 60.0) * 1000.0;
			result = num;
			return true;
		}

		// Token: 0x04001A40 RID: 6720
		private IPIDFloatValue POWER;

		// Token: 0x04001A41 RID: 6721
		private IPIDFloatValue RPM;
	}
}
