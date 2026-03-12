using System;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A07 RID: 2567
	internal class RenaultFapVer3 : RenaultFapVer2
	{
		// Token: 0x06005207 RID: 20999 RVA: 0x003F6789 File Offset: 0x003F4989
		public RenaultFapVer3()
		{
			this.opt_start.Value = "3101020D00";
			this.opt_stop.Value = "3102020D00";
			this.regenStateCommand = "3103020D00";
		}
	}
}
