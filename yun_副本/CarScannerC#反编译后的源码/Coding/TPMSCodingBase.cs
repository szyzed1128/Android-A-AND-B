using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000914 RID: 2324
	internal abstract class TPMSCodingBase : CustomizableCodingTemplate
	{
		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x003A0573 File Offset: 0x0039E773
		// (set) Token: 0x06004DA0 RID: 19872 RVA: 0x003A057B File Offset: 0x0039E77B
		public override CodingGroup Group
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
		} = CodingGroup.TPMS;

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06004DA1 RID: 19873 RVA: 0x003A0584 File Offset: 0x0039E784
		// (set) Token: 0x06004DA2 RID: 19874 RVA: 0x003A058C File Offset: 0x0039E78C
		public override AdaptationValueTypes ValueType
		{
			[CompilerGenerated]
			get
			{
				return this.<ValueType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ValueType>k__BackingField = value;
			}
		} = AdaptationValueTypes.TPMS;

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x003A0595 File Offset: 0x0039E795
		// (set) Token: 0x06004DA4 RID: 19876 RVA: 0x003A059D File Offset: 0x0039E79D
		public string ID1
		{
			get
			{
				return this._ID1;
			}
			set
			{
				this._ID1 = value;
				base.OnPropertyChanged("ID1");
			}
		}

		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x06004DA5 RID: 19877 RVA: 0x003A05B1 File Offset: 0x0039E7B1
		// (set) Token: 0x06004DA6 RID: 19878 RVA: 0x003A05B9 File Offset: 0x0039E7B9
		public string ID2
		{
			get
			{
				return this._ID2;
			}
			set
			{
				this._ID2 = value;
				base.OnPropertyChanged("ID2");
			}
		}

		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x06004DA7 RID: 19879 RVA: 0x003A05CD File Offset: 0x0039E7CD
		// (set) Token: 0x06004DA8 RID: 19880 RVA: 0x003A05D5 File Offset: 0x0039E7D5
		public string ID3
		{
			get
			{
				return this._ID3;
			}
			set
			{
				this._ID3 = value;
				base.OnPropertyChanged("ID3");
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x06004DA9 RID: 19881 RVA: 0x003A05E9 File Offset: 0x0039E7E9
		// (set) Token: 0x06004DAA RID: 19882 RVA: 0x003A05F1 File Offset: 0x0039E7F1
		public string ID4
		{
			get
			{
				return this._ID4;
			}
			set
			{
				this._ID4 = value;
				base.OnPropertyChanged("ID4");
			}
		}

		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x06004DAB RID: 19883 RVA: 0x003A0605 File Offset: 0x0039E805
		// (set) Token: 0x06004DAC RID: 19884 RVA: 0x003A060D File Offset: 0x0039E80D
		public string ID5
		{
			get
			{
				return this._ID5;
			}
			set
			{
				this._ID5 = value;
				base.OnPropertyChanged("ID5");
			}
		}

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x06004DAD RID: 19885 RVA: 0x003A0621 File Offset: 0x0039E821
		// (set) Token: 0x06004DAE RID: 19886 RVA: 0x003A0629 File Offset: 0x0039E829
		public virtual string ID1Title
		{
			[CompilerGenerated]
			get
			{
				return this.<ID1Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ID1Title>k__BackingField = value;
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x06004DAF RID: 19887 RVA: 0x003A0632 File Offset: 0x0039E832
		// (set) Token: 0x06004DB0 RID: 19888 RVA: 0x003A063A File Offset: 0x0039E83A
		public virtual string ID2Title
		{
			[CompilerGenerated]
			get
			{
				return this.<ID2Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ID2Title>k__BackingField = value;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x06004DB1 RID: 19889 RVA: 0x003A0643 File Offset: 0x0039E843
		// (set) Token: 0x06004DB2 RID: 19890 RVA: 0x003A064B File Offset: 0x0039E84B
		public virtual string ID3Title
		{
			[CompilerGenerated]
			get
			{
				return this.<ID3Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ID3Title>k__BackingField = value;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x003A0654 File Offset: 0x0039E854
		// (set) Token: 0x06004DB4 RID: 19892 RVA: 0x003A065C File Offset: 0x0039E85C
		public virtual string ID4Title
		{
			[CompilerGenerated]
			get
			{
				return this.<ID4Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ID4Title>k__BackingField = value;
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x06004DB5 RID: 19893 RVA: 0x003A0665 File Offset: 0x0039E865
		// (set) Token: 0x06004DB6 RID: 19894 RVA: 0x003A066D File Offset: 0x0039E86D
		public virtual string ID5Title
		{
			[CompilerGenerated]
			get
			{
				return this.<ID5Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ID5Title>k__BackingField = value;
			}
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x003A0676 File Offset: 0x0039E876
		// (set) Token: 0x06004DB8 RID: 19896 RVA: 0x003A067E File Offset: 0x0039E87E
		public virtual bool ID1Visible
		{
			get
			{
				return this._ID1Visible;
			}
			set
			{
				if (this._ID1Visible != value)
				{
					this._ID1Visible = value;
					base.OnPropertyChanged("ID1Visible");
				}
			}
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x06004DB9 RID: 19897 RVA: 0x003A069B File Offset: 0x0039E89B
		// (set) Token: 0x06004DBA RID: 19898 RVA: 0x003A06A3 File Offset: 0x0039E8A3
		public virtual bool ID2Visible
		{
			get
			{
				return this._ID2Visible;
			}
			set
			{
				if (this._ID2Visible != value)
				{
					this._ID2Visible = value;
					base.OnPropertyChanged("ID2Visible");
				}
			}
		}

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x06004DBB RID: 19899 RVA: 0x003A06C0 File Offset: 0x0039E8C0
		// (set) Token: 0x06004DBC RID: 19900 RVA: 0x003A06C8 File Offset: 0x0039E8C8
		public virtual bool ID3Visible
		{
			get
			{
				return this._ID3Visible;
			}
			set
			{
				if (this._ID3Visible != value)
				{
					this._ID3Visible = value;
					base.OnPropertyChanged("ID3Visible");
				}
			}
		}

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x06004DBD RID: 19901 RVA: 0x003A06E5 File Offset: 0x0039E8E5
		// (set) Token: 0x06004DBE RID: 19902 RVA: 0x003A06ED File Offset: 0x0039E8ED
		public virtual bool ID4Visible
		{
			get
			{
				return this._ID4Visible;
			}
			set
			{
				if (this._ID4Visible != value)
				{
					this._ID4Visible = value;
					base.OnPropertyChanged("ID4Visible");
				}
			}
		}

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x06004DBF RID: 19903 RVA: 0x003A070A File Offset: 0x0039E90A
		// (set) Token: 0x06004DC0 RID: 19904 RVA: 0x003A0712 File Offset: 0x0039E912
		public virtual bool ID5Visible
		{
			get
			{
				return this._ID5Visible;
			}
			set
			{
				if (this._ID5Visible != value)
				{
					this._ID5Visible = value;
					base.OnPropertyChanged("ID5Visible");
				}
			}
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x003A0730 File Offset: 0x0039E930
		protected TPMSCodingBase()
		{
		}

		// Token: 0x04002E30 RID: 11824
		[CompilerGenerated]
		private CodingGroup <Group>k__BackingField;

		// Token: 0x04002E31 RID: 11825
		[CompilerGenerated]
		private AdaptationValueTypes <ValueType>k__BackingField;

		// Token: 0x04002E32 RID: 11826
		private string _ID1 = "";

		// Token: 0x04002E33 RID: 11827
		private string _ID2 = "";

		// Token: 0x04002E34 RID: 11828
		private string _ID3 = "";

		// Token: 0x04002E35 RID: 11829
		private string _ID4 = "";

		// Token: 0x04002E36 RID: 11830
		private string _ID5 = "";

		// Token: 0x04002E37 RID: 11831
		[CompilerGenerated]
		private string <ID1Title>k__BackingField;

		// Token: 0x04002E38 RID: 11832
		[CompilerGenerated]
		private string <ID2Title>k__BackingField;

		// Token: 0x04002E39 RID: 11833
		[CompilerGenerated]
		private string <ID3Title>k__BackingField;

		// Token: 0x04002E3A RID: 11834
		[CompilerGenerated]
		private string <ID4Title>k__BackingField;

		// Token: 0x04002E3B RID: 11835
		[CompilerGenerated]
		private string <ID5Title>k__BackingField;

		// Token: 0x04002E3C RID: 11836
		private bool _ID1Visible = true;

		// Token: 0x04002E3D RID: 11837
		private bool _ID2Visible = true;

		// Token: 0x04002E3E RID: 11838
		private bool _ID3Visible = true;

		// Token: 0x04002E3F RID: 11839
		private bool _ID4Visible = true;

		// Token: 0x04002E40 RID: 11840
		private bool _ID5Visible = true;
	}
}
