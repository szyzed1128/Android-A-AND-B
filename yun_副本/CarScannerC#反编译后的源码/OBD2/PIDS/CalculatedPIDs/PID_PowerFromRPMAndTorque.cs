using System;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000466 RID: 1126
	internal class PID_PowerFromRPMAndTorque : CalculatedPIDV2
	{
		// Token: 0x06002EA2 RID: 11938 RVA: 0x00206488 File Offset: 0x00204688
		public PID_PowerFromRPMAndTorque()
			: base("Power (from Torque and RPM)", "POWER_FROM_TORQUE", UnitsHelper.Units.hp, 0.0, 300.0, Roles.CALC_PowerFromRPMAndTorque)
		{
			base.Id = 902;
			base.ShortName = "Power from Torque";
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x002064C8 File Offset: 0x002046C8
		public override void Initialize()
		{
			this.TORQUE = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.EngineTorque);
			this.RPM = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.RPM);
			this.requiredPIDs.Clear();
			this.requiredPIDs.Add(this.TORQUE);
			this.requiredPIDs.Add(this.RPM);
			base.DependencyPID = this.TORQUE;
			base.Initialize();
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x00206544 File Offset: 0x00204744
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (this.TORQUE == null || this.RPM == null)
			{
				result = double.NaN;
				return false;
			}
			double num = this.TORQUE.Value;
			if (this.TORQUE.Units != UnitsHelper.Units.lbf_ft)
			{
				num *= 1.355818;
			}
			double value = this.RPM.Value;
			if (value == 0.0)
			{
				result = 0.0;
				return true;
			}
			double num2 = value * num / 9549.0 * 1.36;
			result = num2;
			return true;
		}

		// Token: 0x04001A6D RID: 6765
		private IPIDFloatValue TORQUE;

		// Token: 0x04001A6E RID: 6766
		private IPIDFloatValue RPM;
	}
}
