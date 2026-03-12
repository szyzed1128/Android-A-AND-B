using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000575 RID: 1397
	internal class VagCodeNode
	{
		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x0024270A File Offset: 0x0024090A
		// (set) Token: 0x06003384 RID: 13188 RVA: 0x00242712 File Offset: 0x00240912
		public Dictionary<string, string> Models
		{
			[CompilerGenerated]
			get
			{
				return this.<Models>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Models>k__BackingField = value;
			}
		} = new Dictionary<string, string>(3);

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x0024271B File Offset: 0x0024091B
		// (set) Token: 0x06003386 RID: 13190 RVA: 0x00242723 File Offset: 0x00240923
		public string SAE
		{
			[CompilerGenerated]
			get
			{
				return this.<SAE>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SAE>k__BackingField = value;
			}
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x0024272C File Offset: 0x0024092C
		public VagCodeNode()
		{
		}

		// Token: 0x04001E41 RID: 7745
		[CompilerGenerated]
		private Dictionary<string, string> <Models>k__BackingField;

		// Token: 0x04001E42 RID: 7746
		[CompilerGenerated]
		private string <SAE>k__BackingField;
	}
}
