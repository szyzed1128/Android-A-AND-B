using System;
using System.Collections;

namespace SevenZip.CommandLineParser
{
	// Token: 0x0200003D RID: 61
	public class SwitchResult
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000A8E6 File Offset: 0x00008AE6
		public SwitchResult()
		{
			this.ThereIs = false;
		}

		// Token: 0x04000162 RID: 354
		public bool ThereIs;

		// Token: 0x04000163 RID: 355
		public bool WithMinus;

		// Token: 0x04000164 RID: 356
		public ArrayList PostStrings = new ArrayList();

		// Token: 0x04000165 RID: 357
		public int PostCharIndex;
	}
}
