using System;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000451 RID: 1105
	internal class PID_CalculatedRpmX1000 : CalculatedPIDV2
	{
		// Token: 0x06002E5A RID: 11866 RVA: 0x00204C79 File Offset: 0x00202E79
		public PID_CalculatedRpmX1000()
			: base(Translate.GetString("PID_010C") + " x1000", "", UnitsHelper.Units.rpm, 0.0, 7.0, Roles.None)
		{
			base.Id = 900;
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x00204CBC File Offset: 0x00202EBC
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (pid != null)
			{
				double num = Math.Round(pid.Value / 1000.0, 1);
				result = num;
				return true;
			}
			result = double.NaN;
			return false;
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x00204CF4 File Offset: 0x00202EF4
		public override void Initialize()
		{
			this.requiredPIDs.Clear();
			IPIDFloatValue ipidfloatValue = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.RPM);
			this.requiredPIDs.Add(ipidfloatValue);
			base.DependencyPID = ipidfloatValue;
			base.Initialize();
		}
	}
}
