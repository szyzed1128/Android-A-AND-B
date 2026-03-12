using System;

namespace CarScannerXamarinForms.InApp
{
	// Token: 0x02000499 RID: 1177
	public interface IPurchaseInfo
	{
		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06002F5C RID: 12124
		string ProductId { get; }

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06002F5D RID: 12125
		DateTime PurchaseTime { get; }

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06002F5E RID: 12126
		string TransactionId { get; }
	}
}
