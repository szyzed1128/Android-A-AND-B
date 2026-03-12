using System;

namespace CarScannerXamarinForms
{
	// Token: 0x0200009E RID: 158
	public interface IProduct
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000318 RID: 792
		string ProductID { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000319 RID: 793
		string ProductName { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600031A RID: 794
		string ProductDescription { get; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600031B RID: 795
		string LocalizedPriceString { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600031C RID: 796
		long Price { get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600031D RID: 797
		string CurrencyCode { get; }
	}
}
