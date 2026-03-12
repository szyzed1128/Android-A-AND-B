using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms
{
	// Token: 0x02000157 RID: 343
	public class BTLEDeviceDescription
	{
		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x000817CE File Offset: 0x0007F9CE
		// (set) Token: 0x060014EB RID: 5355 RVA: 0x000817D6 File Offset: 0x0007F9D6
		public string NamePart
		{
			[CompilerGenerated]
			get
			{
				return this.<NamePart>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NamePart>k__BackingField = value;
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x000817DF File Offset: 0x0007F9DF
		// (set) Token: 0x060014ED RID: 5357 RVA: 0x000817E7 File Offset: 0x0007F9E7
		public string ServiceID
		{
			[CompilerGenerated]
			get
			{
				return this.<ServiceID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ServiceID>k__BackingField = value;
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x000817F0 File Offset: 0x0007F9F0
		// (set) Token: 0x060014EF RID: 5359 RVA: 0x000817F8 File Offset: 0x0007F9F8
		public string InputID
		{
			[CompilerGenerated]
			get
			{
				return this.<InputID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InputID>k__BackingField = value;
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x00081801 File Offset: 0x0007FA01
		// (set) Token: 0x060014F1 RID: 5361 RVA: 0x00081809 File Offset: 0x0007FA09
		public string OutputID
		{
			[CompilerGenerated]
			get
			{
				return this.<OutputID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OutputID>k__BackingField = value;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00081812 File Offset: 0x0007FA12
		private static BTLEDeviceDescription Viecar
		{
			get
			{
				return new BTLEDeviceDescription
				{
					NamePart = "viecar",
					ServiceID = "{0000fff0-0000-1000-8000-00805f9b34fb}",
					OutputID = "{0000fff2-0000-1000-8000-00805f9b34fb}",
					InputID = "{0000fff1-0000-1000-8000-00805f9b34fb}"
				};
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x00081845 File Offset: 0x0007FA45
		private static BTLEDeviceDescription Kiwi3
		{
			get
			{
				return new BTLEDeviceDescription
				{
					NamePart = "kiwi",
					ServiceID = "{e47c8027-cca1-4e3b-981f-bdc47abeb5b5}",
					OutputID = "{1cce1ea8-bd34-4813-a00a-c76e028fadcb}",
					InputID = "{cacc07ff-ffff-4c48-8fae-a9ef71b75e26}"
				};
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x00081878 File Offset: 0x0007FA78
		private static BTLEDeviceDescription Veepeak
		{
			get
			{
				return new BTLEDeviceDescription
				{
					NamePart = "veepeak",
					ServiceID = "{0000fff0-0000-1000-8000-00805f9b34fb}",
					OutputID = "{0000fff2-0000-1000-8000-00805f9b34fb}",
					InputID = "{0000fff1-0000-1000-8000-00805f9b34fb}"
				};
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x000818AB File Offset: 0x0007FAAB
		public static IReadOnlyList<BTLEDeviceDescription> GetKnownDeviceDefinitions
		{
			get
			{
				return new List<BTLEDeviceDescription>
				{
					BTLEDeviceDescription.Kiwi3,
					BTLEDeviceDescription.Viecar,
					BTLEDeviceDescription.Veepeak
				};
			}
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x000818D3 File Offset: 0x0007FAD3
		public static string GetBTLEGuidStringFromBytes(string hex_data)
		{
			if (hex_data.Length < 4)
			{
				throw new ArgumentException("Need 2 meaning bytes for BTLE GUID");
			}
			return string.Format("0000{0}-0000-1000-8000-00805f9b34fb", hex_data.Substring(0, 4));
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x000818FB File Offset: 0x0007FAFB
		public static Guid GetBTLEGuidFromBytes(string hex_data)
		{
			if (hex_data.Length < 4)
			{
				throw new ArgumentException("Need 2 meaning bytes for BTLE GUID");
			}
			return Guid.Parse(string.Format("0000{0}-0000-1000-8000-00805f9b34fb", hex_data.Substring(0, 4)));
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00002050 File Offset: 0x00000250
		public BTLEDeviceDescription()
		{
		}

		// Token: 0x04000556 RID: 1366
		[CompilerGenerated]
		private string <NamePart>k__BackingField;

		// Token: 0x04000557 RID: 1367
		[CompilerGenerated]
		private string <ServiceID>k__BackingField;

		// Token: 0x04000558 RID: 1368
		[CompilerGenerated]
		private string <InputID>k__BackingField;

		// Token: 0x04000559 RID: 1369
		[CompilerGenerated]
		private string <OutputID>k__BackingField;
	}
}
