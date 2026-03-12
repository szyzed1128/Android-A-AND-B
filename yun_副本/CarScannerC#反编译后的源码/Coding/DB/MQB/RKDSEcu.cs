using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B43 RID: 2883
	public class RKDSEcu
	{
		// Token: 0x0600594E RID: 22862 RVA: 0x00429F7B File Offset: 0x0042817B
		private RKDSEcu(string name, string address, string version, byte type)
		{
			this.Name = name;
			this.Address = address;
			this.Version = version;
			this.Type = type;
		}

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x0600594F RID: 22863 RVA: 0x00429FA0 File Offset: 0x004281A0
		// (set) Token: 0x06005950 RID: 22864 RVA: 0x00429FA8 File Offset: 0x004281A8
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x06005951 RID: 22865 RVA: 0x00429FB1 File Offset: 0x004281B1
		// (set) Token: 0x06005952 RID: 22866 RVA: 0x00429FB9 File Offset: 0x004281B9
		public string Address
		{
			[CompilerGenerated]
			get
			{
				return this.<Address>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Address>k__BackingField = value;
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x06005953 RID: 22867 RVA: 0x00429FC2 File Offset: 0x004281C2
		// (set) Token: 0x06005954 RID: 22868 RVA: 0x00429FCA File Offset: 0x004281CA
		public string Version
		{
			[CompilerGenerated]
			get
			{
				return this.<Version>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Version>k__BackingField = value;
			}
		}

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x06005955 RID: 22869 RVA: 0x00429FD3 File Offset: 0x004281D3
		// (set) Token: 0x06005956 RID: 22870 RVA: 0x00429FDB File Offset: 0x004281DB
		public byte Type
		{
			[CompilerGenerated]
			get
			{
				return this.<Type>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Type>k__BackingField = value;
			}
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x00429FE4 File Offset: 0x004281E4
		public static RKDSEcu[] GetECUs()
		{
			if (SharedSettings.Current.ShowExperimental)
			{
				return new RKDSEcu[]
				{
					new RKDSEcu("3AA907273D", "7FC000", "2B", 116),
					new RKDSEcu("3AA907273F", "7E0800", "24", 162),
					new RKDSEcu("3AA907273H", "7E0800", "2B", 116),
					new RKDSEcu("5Q0907273B", "7E0800", "22", 34),
					new RKDSEcu("5Q0907273F", "7E0800", "22", 34)
				};
			}
			return new RKDSEcu[]
			{
				new RKDSEcu("3AA907273D", "7FC000", "2B", 116),
				new RKDSEcu("3AA907273F", "7E0800", "24", 162),
				new RKDSEcu("3AA907273H", "7E0800", "2B", 116),
				new RKDSEcu("5Q0907273B", "7E0800", "22", 34)
			};
		}

		// Token: 0x040037CB RID: 14283
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x040037CC RID: 14284
		[CompilerGenerated]
		private string <Address>k__BackingField;

		// Token: 0x040037CD RID: 14285
		[CompilerGenerated]
		private string <Version>k__BackingField;

		// Token: 0x040037CE RID: 14286
		[CompilerGenerated]
		private byte <Type>k__BackingField;
	}
}
