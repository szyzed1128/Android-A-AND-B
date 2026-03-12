using System;

namespace SevenZip.CommandLineParser
{
	// Token: 0x0200003F RID: 63
	public class CommandForm
	{
		// Token: 0x0600018F RID: 399 RVA: 0x0000AD06 File Offset: 0x00008F06
		public CommandForm(string idString, bool postStringMode)
		{
			this.IDString = idString;
			this.PostStringMode = postStringMode;
		}

		// Token: 0x0400016C RID: 364
		public string IDString = "";

		// Token: 0x0400016D RID: 365
		public bool PostStringMode;
	}
}
