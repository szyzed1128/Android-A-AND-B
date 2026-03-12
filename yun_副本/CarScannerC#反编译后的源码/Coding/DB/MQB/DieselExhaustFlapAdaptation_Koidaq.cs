using System;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A9A RID: 2714
	internal class DieselExhaustFlapAdaptation_Koidaq : DPFServiceRegeneration
	{
		// Token: 0x060055B8 RID: 21944 RVA: 0x0040D710 File Offset: 0x0040B910
		public DieselExhaustFlapAdaptation_Koidaq()
		{
			base.Name = "Exhaust flap adaptation (Diesel)";
			base.InnerDescription = "Warning! Not tested!\nCompatibility: Kodiaq (2016-), Tiguan (2016-), etc. 2.0 Diesel";
			this.Unit = "01";
			this.startOption = new MQBAdaptationOption(Translate.GetString("codingDB_opt_Start"), "31010433040000");
			this.cancelOption = new MQBAdaptationOption(Translate.GetString("btnCancel.Content"), "31020433");
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
			this.status_string_0102 = "";
			this.status_string_0104 = "";
			this.Password = "27971";
			base.PasswordVisible = true;
		}
	}
}
