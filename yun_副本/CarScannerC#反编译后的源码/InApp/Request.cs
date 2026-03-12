using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.InApp
{
	// Token: 0x0200049A RID: 1178
	public class Request
	{
		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06002F5F RID: 12127 RVA: 0x0021105A File Offset: 0x0020F25A
		// (set) Token: 0x06002F60 RID: 12128 RVA: 0x00211062 File Offset: 0x0020F262
		public int iap
		{
			[CompilerGenerated]
			get
			{
				return this.<iap>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<iap>k__BackingField = value;
			}
		} = -1;

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06002F61 RID: 12129 RVA: 0x0021106B File Offset: 0x0020F26B
		// (set) Token: 0x06002F62 RID: 12130 RVA: 0x00211073 File Offset: 0x0020F273
		public string packageName
		{
			[CompilerGenerated]
			get
			{
				return this.<packageName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<packageName>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06002F63 RID: 12131 RVA: 0x0021107C File Offset: 0x0020F27C
		// (set) Token: 0x06002F64 RID: 12132 RVA: 0x00211084 File Offset: 0x0020F284
		public string productId
		{
			[CompilerGenerated]
			get
			{
				return this.<productId>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<productId>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06002F65 RID: 12133 RVA: 0x0021108D File Offset: 0x0020F28D
		// (set) Token: 0x06002F66 RID: 12134 RVA: 0x00211095 File Offset: 0x0020F295
		public string purchaseToken
		{
			[CompilerGenerated]
			get
			{
				return this.<purchaseToken>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<purchaseToken>k__BackingField = value;
			}
		} = "";

		// Token: 0x06002F67 RID: 12135 RVA: 0x0021109E File Offset: 0x0020F29E
		public Request()
		{
		}

		// Token: 0x04001B66 RID: 7014
		[CompilerGenerated]
		private int <iap>k__BackingField;

		// Token: 0x04001B67 RID: 7015
		[CompilerGenerated]
		private string <packageName>k__BackingField;

		// Token: 0x04001B68 RID: 7016
		[CompilerGenerated]
		private string <productId>k__BackingField;

		// Token: 0x04001B69 RID: 7017
		[CompilerGenerated]
		private string <purchaseToken>k__BackingField;
	}
}
