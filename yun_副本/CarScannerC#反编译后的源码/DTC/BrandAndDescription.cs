using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000540 RID: 1344
	public class BrandAndDescription
	{
		// Token: 0x0600323E RID: 12862 RVA: 0x0022D004 File Offset: 0x0022B204
		public BrandAndDescription(string brand, string description)
		{
			this.Brand = brand;
			this.Description = description;
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x0022D01A File Offset: 0x0022B21A
		// (set) Token: 0x06003240 RID: 12864 RVA: 0x0022D022 File Offset: 0x0022B222
		public string Brand
		{
			[CompilerGenerated]
			get
			{
				return this.<Brand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Brand>k__BackingField = value;
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x0022D02B File Offset: 0x0022B22B
		// (set) Token: 0x06003242 RID: 12866 RVA: 0x0022D033 File Offset: 0x0022B233
		public string Description
		{
			[CompilerGenerated]
			get
			{
				return this.<Description>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Description>k__BackingField = value;
			}
		}

		// Token: 0x04001D32 RID: 7474
		[CompilerGenerated]
		private string <Brand>k__BackingField;

		// Token: 0x04001D33 RID: 7475
		[CompilerGenerated]
		private string <Description>k__BackingField;
	}
}
