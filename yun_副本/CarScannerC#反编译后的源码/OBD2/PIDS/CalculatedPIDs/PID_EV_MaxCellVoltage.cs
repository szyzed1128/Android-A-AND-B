using System;
using System.Collections.Generic;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200045E RID: 1118
	internal class PID_EV_MaxCellVoltage : CalculatedPIDV2
	{
		// Token: 0x06002E8B RID: 11915 RVA: 0x00205D20 File Offset: 0x00203F20
		public PID_EV_MaxCellVoltage()
			: base("Max Cell Voltage", "EV_MAX_CELL_VOLTAGE", UnitsHelper.Units.volts, 0.0, 800.0, Roles.EV_MaxCellVoltage)
		{
			base.Id = 802;
			base.ShortName = base.Name;
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x00205D78 File Offset: 0x00203F78
		public override void Initialize()
		{
			this.cells.Clear();
			this.cells.AddRange(App.OBDReader.CurrentCarData.FindPIDsByRole(Roles.EV_CellVoltage));
			foreach (IPIDFloatValue ipidfloatValue in this.cells)
			{
				this.requiredPIDs.Add(ipidfloatValue);
			}
			if (this.cells.Count > 0)
			{
				base.DependencyPID = this.cells[this.cells.Count - 1];
			}
			if (SharedSettings.Current.ProfileUpdateAlias == "cc94f7e67ba44df38627d945c6e74220")
			{
				this.IsAvailable = false;
				this.requiredPIDs.Clear();
			}
			base.Initialize();
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x00205E54 File Offset: 0x00204054
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (pid == base.DependencyPID)
			{
				double num = double.NaN;
				foreach (IPIDFloatValue ipidfloatValue in this.cells)
				{
					double num2 = ipidfloatValue.Value;
					if (ipidfloatValue.Units == UnitsHelper.Units.mV)
					{
						num2 /= 1000.0;
					}
					if (num2 != 0.0 && num2 < 4.89)
					{
						if (double.IsNaN(num))
						{
							num = num2;
						}
						else if (double.IsFinite(num2) && num2 > num)
						{
							num = num2;
						}
					}
				}
				result = num;
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x04001A64 RID: 6756
		private List<IPIDFloatValue> cells = new List<IPIDFloatValue>();
	}
}
