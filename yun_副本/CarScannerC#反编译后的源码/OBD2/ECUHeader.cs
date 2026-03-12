using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000312 RID: 786
	public class ECUHeader
	{
		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x001BDED6 File Offset: 0x001BC0D6
		// (set) Token: 0x06002417 RID: 9239 RVA: 0x001BDEDE File Offset: 0x001BC0DE
		public string Id
		{
			[CompilerGenerated]
			get
			{
				return this.<Id>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Id>k__BackingField = value;
			}
		}

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x001BDEE7 File Offset: 0x001BC0E7
		public string FriendlyName
		{
			get
			{
				return "ECU ID: " + this.Id;
			}
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x001BDEF9 File Offset: 0x001BC0F9
		public override string ToString()
		{
			return this.Id;
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x001BDF01 File Offset: 0x001BC101
		public ECUHeader(string id)
		{
			this.Id = id;
		}

		// Token: 0x040011D7 RID: 4567
		[CompilerGenerated]
		private string <Id>k__BackingField;
	}
}
