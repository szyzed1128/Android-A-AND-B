using System;

namespace CarScannerXamarinForms
{
	// Token: 0x02000050 RID: 80
	public interface IKeyPairStorage
	{
		// Token: 0x060001E6 RID: 486
		bool KeyExists(string key);

		// Token: 0x060001E7 RID: 487
		bool GetBool(string key);

		// Token: 0x060001E8 RID: 488
		void SetBool(string key, bool b);

		// Token: 0x060001E9 RID: 489
		string GetString(string key);

		// Token: 0x060001EA RID: 490
		void SetString(string key, string s);

		// Token: 0x060001EB RID: 491
		long GetLong(string key);

		// Token: 0x060001EC RID: 492
		void SetLong(string key, long l);

		// Token: 0x060001ED RID: 493
		double GetDouble(string key);

		// Token: 0x060001EE RID: 494
		void SetDouble(string key, double d);
	}
}
