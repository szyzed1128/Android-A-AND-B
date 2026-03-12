using System;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AAA RID: 2730
	internal class EA288_LowTemperatureCircuitBleeding : DPFServiceRegeneration
	{
		// Token: 0x0600560C RID: 22028 RVA: 0x00410484 File Offset: 0x0040E684
		public EA288_LowTemperatureCircuitBleeding()
		{
			base.Name = "EA288 Bleeding of Low Temperature Circuit Cooling (255 sec.)";
			base.InnerDescription = "Compatibility: EA288 Diesel";
			this.Unit = "01";
			this.startOption = new MQBAdaptationOption(Translate.GetString("codingDB_opt_Start"), "310103FD0400FF");
			this.cancelOption = new MQBAdaptationOption(Translate.GetString("btnCancel.Content"), "310203FD");
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
			this.status_string_0102 = "";
			this.status_string_0104 = "";
			this.Password = "27971";
			base.PasswordVisible = true;
		}
	}
}
