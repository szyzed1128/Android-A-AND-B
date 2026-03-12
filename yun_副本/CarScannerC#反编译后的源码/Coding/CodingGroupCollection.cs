using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000850 RID: 2128
	public class CodingGroupCollection : ObservableCollection<ICodingContainer>
	{
		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x0600489C RID: 18588 RVA: 0x00371018 File Offset: 0x0036F218
		public string Name
		{
			get
			{
				return Translate.GetString("coding_Group_" + this.Group.ToString());
			}
		}

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x0600489D RID: 18589 RVA: 0x00371048 File Offset: 0x0036F248
		// (set) Token: 0x0600489E RID: 18590 RVA: 0x00371050 File Offset: 0x0036F250
		public CodingGroup Group
		{
			[CompilerGenerated]
			get
			{
				return this.<Group>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Group>k__BackingField = value;
			}
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x00371059 File Offset: 0x0036F259
		public CodingGroupCollection()
		{
		}

		// Token: 0x040029E8 RID: 10728
		[CompilerGenerated]
		private CodingGroup <Group>k__BackingField;
	}
}
