using System;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AA9 RID: 2729
	internal class DSGDQ381AdaptationRoutine : DSGAdaptationRoutine
	{
		// Token: 0x0600560B RID: 22027 RVA: 0x0041044A File Offset: 0x0040E64A
		public DSGDQ381AdaptationRoutine()
		{
			this.startOption.Value = "310103800000";
			base.Name = Translate.GetString("codingDB_DsgAdaptationDq200Dq250Dq500_Name").Replace("(DQ200, DQ250, DQ500)", "(DQ381)");
		}
	}
}
