using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x020009F9 RID: 2553
	internal class ESPConfig
	{
		// Token: 0x060051C5 RID: 20933 RVA: 0x003F412C File Offset: 0x003F232C
		public ESPConfig(string ident, params ValueTuple<string, string>[] values)
		{
			this.Ident = ident;
			foreach (ValueTuple<string, string> valueTuple in values)
			{
				this.Data[valueTuple.Item1] = valueTuple.Item2;
			}
		}

		// Token: 0x040031B1 RID: 12721
		public string Ident;

		// Token: 0x040031B2 RID: 12722
		public Dictionary<string, string> Data = new Dictionary<string, string>();
	}
}
