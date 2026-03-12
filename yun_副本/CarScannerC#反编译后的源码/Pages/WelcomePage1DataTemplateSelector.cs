using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x0200066B RID: 1643
	internal class WelcomePage1DataTemplateSelector : DataTemplateSelector
	{
		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x06003862 RID: 14434 RVA: 0x002A9A86 File Offset: 0x002A7C86
		// (set) Token: 0x06003863 RID: 14435 RVA: 0x002A9A8E File Offset: 0x002A7C8E
		public DataTemplate Item0
		{
			[CompilerGenerated]
			get
			{
				return this.<Item0>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Item0>k__BackingField = value;
			}
		}

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06003864 RID: 14436 RVA: 0x002A9A97 File Offset: 0x002A7C97
		// (set) Token: 0x06003865 RID: 14437 RVA: 0x002A9A9F File Offset: 0x002A7C9F
		public DataTemplate Item1
		{
			[CompilerGenerated]
			get
			{
				return this.<Item1>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Item1>k__BackingField = value;
			}
		}

		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06003866 RID: 14438 RVA: 0x002A9AA8 File Offset: 0x002A7CA8
		// (set) Token: 0x06003867 RID: 14439 RVA: 0x002A9AB0 File Offset: 0x002A7CB0
		public DataTemplate Item2
		{
			[CompilerGenerated]
			get
			{
				return this.<Item2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Item2>k__BackingField = value;
			}
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x002A9ABC File Offset: 0x002A7CBC
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			switch ((int)item)
			{
			case 0:
				return this.Item0;
			case 1:
				return this.Item1;
			case 2:
				return this.Item2;
			default:
				return null;
			}
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x00016A2D File Offset: 0x00014C2D
		public WelcomePage1DataTemplateSelector()
		{
		}

		// Token: 0x04002248 RID: 8776
		[CompilerGenerated]
		private DataTemplate <Item0>k__BackingField;

		// Token: 0x04002249 RID: 8777
		[CompilerGenerated]
		private DataTemplate <Item1>k__BackingField;

		// Token: 0x0400224A RID: 8778
		[CompilerGenerated]
		private DataTemplate <Item2>k__BackingField;
	}
}
