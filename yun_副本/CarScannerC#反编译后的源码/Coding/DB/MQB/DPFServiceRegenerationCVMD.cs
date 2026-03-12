using System;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AA3 RID: 2723
	internal class DPFServiceRegenerationCVMD : DPFServiceRegeneration
	{
		// Token: 0x060055FF RID: 22015 RVA: 0x0040FBEE File Offset: 0x0040DDEE
		public DPFServiceRegenerationCVMD()
		{
			this.startOption.Value = "31010305040000";
			this.cancelOption.Value = "31020305";
			base.Name += " (EA839)";
		}
	}
}
