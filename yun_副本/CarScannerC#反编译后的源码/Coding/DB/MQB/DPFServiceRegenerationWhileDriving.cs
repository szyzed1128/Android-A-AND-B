using System;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AA4 RID: 2724
	internal class DPFServiceRegenerationWhileDriving : DPFServiceRegeneration
	{
		// Token: 0x06005600 RID: 22016 RVA: 0x0040FC2C File Offset: 0x0040DE2C
		public DPFServiceRegenerationWhileDriving(string addTextToName = "")
		{
			this.startOption.Value = "3101053D040000";
			this.cancelOption.Value = "3102053D";
			base.Name = Translate.GetString("codingDB_DpfServiceRegenerationWhileDriving_Name") + addTextToName;
			base.InnerDescription = Translate.GetString("codingDB_DpfServiceRegenerationWhileDriving_InnerDescription");
		}
	}
}
