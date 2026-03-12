using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms
{
	// Token: 0x02000180 RID: 384
	public class SettingsPageElement
	{
		// Token: 0x060015A1 RID: 5537 RVA: 0x0009981E File Offset: 0x00097A1E
		public SettingsPageElement(string Text, string Image, Type t)
		{
			this.Text = Text;
			this.Image = Image;
			this.t = t;
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x0009983B File Offset: 0x00097A3B
		// (set) Token: 0x060015A3 RID: 5539 RVA: 0x00099843 File Offset: 0x00097A43
		public string Text
		{
			[CompilerGenerated]
			get
			{
				return this.<Text>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Text>k__BackingField = value;
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x0009984C File Offset: 0x00097A4C
		// (set) Token: 0x060015A5 RID: 5541 RVA: 0x00099854 File Offset: 0x00097A54
		public string Image
		{
			[CompilerGenerated]
			get
			{
				return this.<Image>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Image>k__BackingField = value;
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x0009985D File Offset: 0x00097A5D
		public Type PageType
		{
			get
			{
				return this.t;
			}
		}

		// Token: 0x04000627 RID: 1575
		[CompilerGenerated]
		private string <Text>k__BackingField;

		// Token: 0x04000628 RID: 1576
		[CompilerGenerated]
		private string <Image>k__BackingField;

		// Token: 0x04000629 RID: 1577
		private Type t;
	}
}
