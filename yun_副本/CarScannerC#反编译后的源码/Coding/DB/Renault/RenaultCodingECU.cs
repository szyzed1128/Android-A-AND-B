using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x020009FF RID: 2559
	internal class RenaultCodingECU
	{
		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x060051D2 RID: 20946 RVA: 0x003F5646 File Offset: 0x003F3846
		// (set) Token: 0x060051D3 RID: 20947 RVA: 0x003F564E File Offset: 0x003F384E
		public string Filename
		{
			[CompilerGenerated]
			get
			{
				return this.<Filename>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Filename>k__BackingField = value;
			}
		}

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x060051D4 RID: 20948 RVA: 0x003F5657 File Offset: 0x003F3857
		// (set) Token: 0x060051D5 RID: 20949 RVA: 0x003F565F File Offset: 0x003F385F
		public ObservableCollection<RenaultECUIdents> Idents
		{
			[CompilerGenerated]
			get
			{
				return this.<Idents>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Idents>k__BackingField = value;
			}
		}

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x060051D6 RID: 20950 RVA: 0x003F5668 File Offset: 0x003F3868
		// (set) Token: 0x060051D7 RID: 20951 RVA: 0x003F5670 File Offset: 0x003F3870
		public string RequestHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestHeader>k__BackingField = value;
			}
		}

		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x060051D8 RID: 20952 RVA: 0x003F5679 File Offset: 0x003F3879
		// (set) Token: 0x060051D9 RID: 20953 RVA: 0x003F5681 File Offset: 0x003F3881
		public string ResponseHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponseHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResponseHeader>k__BackingField = value;
			}
		}

		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x060051DA RID: 20954 RVA: 0x003F568A File Offset: 0x003F388A
		// (set) Token: 0x060051DB RID: 20955 RVA: 0x003F5692 File Offset: 0x003F3892
		public string ATST
		{
			[CompilerGenerated]
			get
			{
				return this.<ATST>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ATST>k__BackingField = value;
			}
		} = "64";

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x060051DC RID: 20956 RVA: 0x003F569B File Offset: 0x003F389B
		// (set) Token: 0x060051DD RID: 20957 RVA: 0x003F56A3 File Offset: 0x003F38A3
		public string StartCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<StartCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StartCommand>k__BackingField = value;
			}
		} = "10C0";

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x060051DE RID: 20958 RVA: 0x003F56AC File Offset: 0x003F38AC
		// (set) Token: 0x060051DF RID: 20959 RVA: 0x003F56B4 File Offset: 0x003F38B4
		public string RebootCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<RebootCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RebootCommand>k__BackingField = value;
			}
		} = "";

		// Token: 0x060051E0 RID: 20960 RVA: 0x003F56BD File Offset: 0x003F38BD
		public RenaultCodingECU()
		{
		}

		// Token: 0x040031C9 RID: 12745
		[CompilerGenerated]
		private string <Filename>k__BackingField;

		// Token: 0x040031CA RID: 12746
		[CompilerGenerated]
		private ObservableCollection<RenaultECUIdents> <Idents>k__BackingField;

		// Token: 0x040031CB RID: 12747
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x040031CC RID: 12748
		[CompilerGenerated]
		private string <ResponseHeader>k__BackingField;

		// Token: 0x040031CD RID: 12749
		[CompilerGenerated]
		private string <ATST>k__BackingField;

		// Token: 0x040031CE RID: 12750
		[CompilerGenerated]
		private string <StartCommand>k__BackingField;

		// Token: 0x040031CF RID: 12751
		[CompilerGenerated]
		private string <RebootCommand>k__BackingField;
	}
}
