using System;
using System.Collections.Generic;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200045F RID: 1119
	internal class PID_EV_MinCellVoltage : CalculatedPIDV2
	{
		// Token: 0x06002E8E RID: 11918 RVA: 0x00205F14 File Offset: 0x00204114
		public PID_EV_MinCellVoltage()
			: base("Min Cell Voltage", "EV_MIN_CELL_VOLTAGE", UnitsHelper.Units.volts, 0.0, 800.0, Roles.EV_MinCellVoltage)
		{
			base.Id = 803;
			base.ShortName = base.Name;
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x00205F6C File Offset: 0x0020416C
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

		// Token: 0x06002E90 RID: 11920 RVA: 0x00206048 File Offset: 0x00204248
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
					if (num2 != 0.0 && num2 < 4.89 && double.IsFinite(num2))
					{
						if (double.IsNaN(num))
						{
							num = num2;
						}
						else if (double.IsFinite(num2) && num2 < num)
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

		// Token: 0x04001A65 RID: 6757
		private List<IPIDFloatValue> cells = new List<IPIDFloatValue>();
	}
}
