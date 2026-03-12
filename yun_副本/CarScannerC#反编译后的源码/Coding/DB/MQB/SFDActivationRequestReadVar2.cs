using System;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B4E RID: 2894
	internal class SFDActivationRequestReadVar2 : SFDActivationRequestReadVar1
	{
		// Token: 0x0600599A RID: 22938 RVA: 0x0042BA6A File Offset: 0x00429C6A
		public SFDActivationRequestReadVar2(string title, string requestHeader, string responseHeader, string protocol)
			: base(title, requestHeader, responseHeader, protocol)
		{
			this.ReadModeAndAddress = "3101C00802";
		}
	}
}
