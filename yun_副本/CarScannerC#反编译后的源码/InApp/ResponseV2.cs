using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.InApp
{
	// Token: 0x0200049C RID: 1180
	public class ResponseV2
	{
		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06002F6F RID: 12143 RVA: 0x0021111F File Offset: 0x0020F31F
		// (set) Token: 0x06002F70 RID: 12144 RVA: 0x00211127 File Offset: 0x0020F327
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

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06002F71 RID: 12145 RVA: 0x00211130 File Offset: 0x0020F330
		// (set) Token: 0x06002F72 RID: 12146 RVA: 0x00211138 File Offset: 0x0020F338
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

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06002F73 RID: 12147 RVA: 0x00211141 File Offset: 0x0020F341
		// (set) Token: 0x06002F74 RID: 12148 RVA: 0x00211149 File Offset: 0x0020F349
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

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06002F75 RID: 12149 RVA: 0x00211152 File Offset: 0x0020F352
		// (set) Token: 0x06002F76 RID: 12150 RVA: 0x0021115A File Offset: 0x0020F35A
		public long expirityTime
		{
			[CompilerGenerated]
			get
			{
				return this.<expirityTime>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<expirityTime>k__BackingField = value;
			}
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x00211163 File Offset: 0x0020F363
		public ResponseV2()
		{
		}

		// Token: 0x04001B6D RID: 7021
		[CompilerGenerated]
		private string <message>k__BackingField;

		// Token: 0x04001B6E RID: 7022
		[CompilerGenerated]
		private string <orderId>k__BackingField;

		// Token: 0x04001B6F RID: 7023
		[CompilerGenerated]
		private int <result>k__BackingField;

		// Token: 0x04001B70 RID: 7024
		[CompilerGenerated]
		private long <expirityTime>k__BackingField;
	}
}
