using System;

namespace SevenZip.CommandLineParser
{
	// Token: 0x0200003C RID: 60
	public class SwitchForm
	{
		// Token: 0x06000184 RID: 388 RVA: 0x0000A892 File Offset: 0x00008A92
		public SwitchForm(string idString, SwitchType type, bool multi, int minLen, int maxLen, string postCharSet)
		{
			this.IDString = idString;
			this.Type = type;
			this.Multi = multi;
			this.MinLen = minLen;
			this.MaxLen = maxLen;
			this.PostCharSet = postCharSet;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000A8C7 File Offset: 0x00008AC7
		public SwitchForm(string idString, SwitchType type, bool multi, int minLen)
			: this(idString, type, multi, minLen, 0, "")
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000A8DA File Offset: 0x00008ADA
		public SwitchForm(string idString, SwitchType type, bool multi)
			: this(idString, type, multi, 0)
		{
		}

		// Token: 0x0400015C RID: 348
		public string IDString;

		// Token: 0x0400015D RID: 349
		public SwitchType Type;

		// Token: 0x0400015E RID: 350
		public bool Multi;

		// Token: 0x0400015F RID: 351
		public int MinLen;

		// Token: 0x04000160 RID: 352
		public int MaxLen;

		// Token: 0x04000161 RID: 353
		public string PostCharSet;
	}
}
