using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.InApp
{
	// Token: 0x0200049B RID: 1179
	public class Response
	{
		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06002F68 RID: 12136 RVA: 0x002110CE File Offset: 0x0020F2CE
		// (set) Token: 0x06002F69 RID: 12137 RVA: 0x002110D6 File Offset: 0x0020F2D6
		public string message
		{
			[CompilerGenerated]
			get
			{
				return this.<message>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<message>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06002F6A RID: 12138 RVA: 0x002110DF File Offset: 0x0020F2DF
		// (set) Token: 0x06002F6B RID: 12139 RVA: 0x002110E7 File Offset: 0x0020F2E7
		public string orderId
		{
			[CompilerGenerated]
			get
			{
				return this.<orderId>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<orderId>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06002F6C RID: 12140 RVA: 0x002110F0 File Offset: 0x0020F2F0
		// (set) Token: 0x06002F6D RID: 12141 RVA: 0x002110F8 File Offset: 0x0020F2F8
		public int result
		{
			[CompilerGenerated]
			get
			{
				return this.<result>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<result>k__BackingField = value;
			}
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x00211101 File Offset: 0x0020F301
		public Response()
		{
		}

		// Token: 0x04001B6A RID: 7018
		[CompilerGenerated]
		private string <message>k__BackingField;

		// Token: 0x04001B6B RID: 7019
		[CompilerGenerated]
		private string <orderId>k__BackingField;

		// Token: 0x04001B6C RID: 7020
		[CompilerGenerated]
		private int <result>k__BackingField;
	}
}
