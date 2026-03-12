using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000574 RID: 1396
	internal class VagCode
	{
		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x0600337E RID: 13182 RVA: 0x002426D4 File Offset: 0x002408D4
		// (set) Token: 0x0600337F RID: 13183 RVA: 0x002426DC File Offset: 0x002408DC
		public int Code
		{
			[CompilerGenerated]
			get
			{
				return this.<Code>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Code>k__BackingField = value;
			}
		}

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x06003380 RID: 13184 RVA: 0x002426E5 File Offset: 0x002408E5
		// (set) Token: 0x06003381 RID: 13185 RVA: 0x002426ED File Offset: 0x002408ED
		public List<VagCodeNode> Nodes
		{
			[CompilerGenerated]
			get
			{
				return this.<Nodes>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Nodes>k__BackingField = value;
			}
		} = new List<VagCodeNode>(1);

		// Token: 0x06003382 RID: 13186 RVA: 0x002426F6 File Offset: 0x002408F6
		public VagCode()
		{
		}

		// Token: 0x04001E3F RID: 7743
		[CompilerGenerated]
		private int <Code>k__BackingField;

		// Token: 0x04001E40 RID: 7744
		[CompilerGenerated]
		private List<VagCodeNode> <Nodes>k__BackingField;
	}
}
