using System;
using CarScannerXamarinForms.Coding.DB.MQB;

namespace CarScannerXamarinForms.Coding.DB.VAG_OTHER
{
	// Token: 0x020009D8 RID: 2520
	internal class MLB_DPF_ServiceReset : DPFServiceRegeneration
	{
		// Token: 0x0600514E RID: 20814 RVA: 0x003F12E4 File Offset: 0x003EF4E4
		public MLB_DPF_ServiceReset()
		{
			base.Name = "Resettings of DPF learned values";
			base.InnerDescription = "Use this feature wisely!";
			this.startOption = new MQBAdaptationOption(Translate.GetString("codingDB_opt_Start"), "31010302040000");
			this.cancelOption = new MQBAdaptationOption(Translate.GetString("btnCancel.Content"), "31020302");
			base.Options.Clear();
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
		}
	}
}
