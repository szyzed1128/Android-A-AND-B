using System;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000465 RID: 1125
	internal class PID_PowerFromAir : CalculatedPIDV2
	{
		// Token: 0x06002E9F RID: 11935 RVA: 0x002063D6 File Offset: 0x002045D6
		public PID_PowerFromAir()
			: base("Power from MAF", "POWER_FROM_AIR", UnitsHelper.Units.hp, 0.0, 500.0, Roles.None)
		{
			base.Id = 807;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x00206408 File Offset: 0x00204608
		public override void Initialize()
		{
			this.maf = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.MAF);
			base.DependencyPID = this.maf;
			this.requiredPIDs.Clear();
			this.requiredPIDs.Add(this.maf);
			base.Initialize();
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x00206459 File Offset: 0x00204659
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (this.maf != null)
			{
				result = this.maf.Value * 1.2;
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x04001A6C RID: 6764
		private IPIDFloatValue maf;
	}
}
