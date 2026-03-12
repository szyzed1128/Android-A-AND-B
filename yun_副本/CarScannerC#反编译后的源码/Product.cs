using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms
{
	// Token: 0x0200009F RID: 159
	public class Product : IProduct
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0001D7AE File Offset: 0x0001B9AE
		// (set) Token: 0x0600031F RID: 799 RVA: 0x0001D7B6 File Offset: 0x0001B9B6
		public string ProductID
		{
			[CompilerGenerated]
			get
			{
				return this.<ProductID>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ProductID>k__BackingField = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0001D7BF File Offset: 0x0001B9BF
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0001D7C7 File Offset: 0x0001B9C7
		public string ProductName
		{
			[CompilerGenerated]
			get
			{
				return this.<ProductName>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ProductName>k__BackingField = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0001D7D0 File Offset: 0x0001B9D0
		// (set) Token: 0x06000323 RID: 803 RVA: 0x0001D7D8 File Offset: 0x0001B9D8
		public string ProductDescription
		{
			[CompilerGenerated]
			get
			{
				return this.<ProductDescription>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ProductDescription>k__BackingField = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0001D7E1 File Offset: 0x0001B9E1
		// (set) Token: 0x06000325 RID: 805 RVA: 0x0001D7E9 File Offset: 0x0001B9E9
		public string LocalizedPriceString
		{
			[CompilerGenerated]
			get
			{
				return this.<LocalizedPriceString>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LocalizedPriceString>k__BackingField = value;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0001D7F2 File Offset: 0x0001B9F2
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0001D7FA File Offset: 0x0001B9FA
		public string CurrencyCode
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrencyCode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CurrencyCode>k__BackingField = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0001D803 File Offset: 0x0001BA03
		// (set) Token: 0x06000329 RID: 809 RVA: 0x0001D80B File Offset: 0x0001BA0B
		public long Price
		{
			[CompilerGenerated]
			get
			{
				return this.<Price>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Price>k__BackingField = value;
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0001D814 File Offset: 0x0001BA14
		public Product(string productID, string productName, string productDescription, string localizedPriceString, long price, string currencyCode)
		{
			this.ProductID = productID;
			this.ProductName = productName;
			this.ProductDescription = productDescription;
			this.LocalizedPriceString = localizedPriceString;
			this.Price = price;
			this.CurrencyCode = currencyCode;
		}

		// Token: 0x04000206 RID: 518
		[CompilerGenerated]
		private string <ProductID>k__BackingField;

		// Token: 0x04000207 RID: 519
		[CompilerGenerated]
		private string <ProductName>k__BackingField;

		// Token: 0x04000208 RID: 520
		[CompilerGenerated]
		private string <ProductDescription>k__BackingField;

		// Token: 0x04000209 RID: 521
		[CompilerGenerated]
		private string <LocalizedPriceString>k__BackingField;

		// Token: 0x0400020A RID: 522
		[CompilerGenerated]
		private string <CurrencyCode>k__BackingField;

		// Token: 0x0400020B RID: 523
		[CompilerGenerated]
		private long <Price>k__BackingField;
	}
}
