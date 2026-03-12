using System;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000452 RID: 1106
	internal class PID_Calculated_EV_CharghingPower : CalculatedPIDV2
	{
		// Token: 0x06002E5D RID: 11869 RVA: 0x00204D36 File Offset: 0x00202F36
		public PID_Calculated_EV_CharghingPower()
			: base("Hybrid/EV Charger Power", "EV_CHARGER_POWER", UnitsHelper.Units.kW, -50.0, 50.0, Roles.EV_ChargerPower)
		{
			base.Id = 805;
			base.ShortName = "EV Charger Power";
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x00204D74 File Offset: 0x00202F74
		public override void Initialize()
		{
			this.VOLTAGE = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.EV_ChargerVoltage);
			this.CURRENT = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.EV_ChargerCurrent);
			this.requiredPIDs.Clear();
			this.requiredPIDs.Add(this.VOLTAGE);
			this.requiredPIDs.Add(this.CURRENT);
			base.DependencyPID = this.VOLTAGE;
			base.Initialize();
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x00204DF0 File Offset: 0x00202FF0
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			double num = this.VOLTAGE.Value;
			if (this.VOLTAGE.Units == UnitsHelper.Units.mV)
			{
				num /= 1000.0;
			}
			double num2 = this.CURRENT.Value;
			if (this.CURRENT.Units == UnitsHelper.Units.mA)
			{
				num2 /= 1000.0;
			}
			result = num2 * num / 1000.0;
			return true;
		}

		// Token: 0x04001A3E RID: 6718
		private IPIDFloatValue VOLTAGE;

		// Token: 0x04001A3F RID: 6719
		private IPIDFloatValue CURRENT;
	}
}
