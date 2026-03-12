using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200040C RID: 1036
	internal class PIDOverride : INotifyPropertyChanged
	{
		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x001F2679 File Offset: 0x001F0879
		// (set) Token: 0x06002A7B RID: 10875 RVA: 0x001F2681 File Offset: 0x001F0881
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("_ID"));
			}
		}

		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x001F26A5 File Offset: 0x001F08A5
		// (set) Token: 0x06002A7D RID: 10877 RVA: 0x001F26AD File Offset: 0x001F08AD
		public int SkipCycles
		{
			get
			{
				return this._SkipCycles;
			}
			set
			{
				this._SkipCycles = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("SkipCycles"));
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x06002A7E RID: 10878 RVA: 0x001F26D1 File Offset: 0x001F08D1
		// (set) Token: 0x06002A7F RID: 10879 RVA: 0x001F26D9 File Offset: 0x001F08D9
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x001F26FD File Offset: 0x001F08FD
		// (set) Token: 0x06002A81 RID: 10881 RVA: 0x001F2705 File Offset: 0x001F0905
		public string ShortName
		{
			get
			{
				return this._ShortName;
			}
			set
			{
				this._ShortName = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("ShortName"));
			}
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x001F2729 File Offset: 0x001F0929
		// (set) Token: 0x06002A83 RID: 10883 RVA: 0x001F2731 File Offset: 0x001F0931
		public Roles Role
		{
			get
			{
				return this._Role;
			}
			set
			{
				this._Role = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Role"));
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x001F2755 File Offset: 0x001F0955
		// (set) Token: 0x06002A85 RID: 10885 RVA: 0x001F275D File Offset: 0x001F095D
		public UnitsHelper.Units Unit
		{
			get
			{
				return this._Unit;
			}
			set
			{
				this._Unit = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Unit"));
			}
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06002A86 RID: 10886 RVA: 0x001F2781 File Offset: 0x001F0981
		// (set) Token: 0x06002A87 RID: 10887 RVA: 0x001F2789 File Offset: 0x001F0989
		public string TextValueVariants
		{
			[CompilerGenerated]
			get
			{
				return this.<TextValueVariants>k__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				this.<TextValueVariants>k__BackingField = value;
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06002A88 RID: 10888 RVA: 0x001F2794 File Offset: 0x001F0994
		// (remove) Token: 0x06002A89 RID: 10889 RVA: 0x001F27CC File Offset: 0x001F09CC
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x001F2801 File Offset: 0x001F0A01
		public PIDOverride()
		{
		}

		// Token: 0x040017F5 RID: 6133
		private int _ID = int.MinValue;

		// Token: 0x040017F6 RID: 6134
		private int _SkipCycles;

		// Token: 0x040017F7 RID: 6135
		private string _Name = "";

		// Token: 0x040017F8 RID: 6136
		private string _ShortName = "";

		// Token: 0x040017F9 RID: 6137
		private Roles _Role = Roles.UNDEFINED;

		// Token: 0x040017FA RID: 6138
		[CompilerGenerated]
		private string <TextValueVariants>k__BackingField;

		// Token: 0x040017FB RID: 6139
		private UnitsHelper.Units _Unit;

		// Token: 0x040017FC RID: 6140
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;
	}
}
