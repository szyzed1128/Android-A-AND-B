using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.DataRecorder;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x0200073E RID: 1854
	public class FileToRecordProxy : INotifyPropertyChanged
	{
		// Token: 0x06003EF0 RID: 16112 RVA: 0x0032EB14 File Offset: 0x0032CD14
		public FileToRecordProxy(string filepath)
		{
			this.Path = filepath;
		}

		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x06003EF1 RID: 16113 RVA: 0x0032EB65 File Offset: 0x0032CD65
		// (set) Token: 0x06003EF2 RID: 16114 RVA: 0x0032EB6D File Offset: 0x0032CD6D
		public bool IsSelected
		{
			get
			{
				return this._IsSelected;
			}
			set
			{
				if (this._IsSelected != value)
				{
					this._IsSelected = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged == null)
					{
						return;
					}
					propertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
				}
			}
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0032EB9A File Offset: 0x0032CD9A
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x06003EF4 RID: 16116 RVA: 0x0032EBB3 File Offset: 0x0032CDB3
		// (set) Token: 0x06003EF5 RID: 16117 RVA: 0x0032EBBB File Offset: 0x0032CDBB
		public string Path
		{
			[CompilerGenerated]
			get
			{
				return this.<Path>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Path>k__BackingField = value;
			}
		}

		// Token: 0x17001486 RID: 5254
		// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x0032EBC4 File Offset: 0x0032CDC4
		public string Name
		{
			get
			{
				return global::System.IO.Path.GetFileNameWithoutExtension(this.Path);
			}
		}

		// Token: 0x17001487 RID: 5255
		// (get) Token: 0x06003EF7 RID: 16119 RVA: 0x0032EBD1 File Offset: 0x0032CDD1
		// (set) Token: 0x06003EF8 RID: 16120 RVA: 0x0032EBD9 File Offset: 0x0032CDD9
		public string Size
		{
			get
			{
				return this._Size;
			}
			set
			{
				if (this._Size == value)
				{
					return;
				}
				this._Size = value;
				this.OnPropertyChanged("Size");
			}
		}

		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x06003EF9 RID: 16121 RVA: 0x0032EBFC File Offset: 0x0032CDFC
		// (set) Token: 0x06003EFA RID: 16122 RVA: 0x0032EC04 File Offset: 0x0032CE04
		public string VIN
		{
			get
			{
				return this._VIN;
			}
			set
			{
				if (this._VIN == value)
				{
					return;
				}
				this._VIN = value;
				this.OnPropertyChanged("VIN");
			}
		}

		// Token: 0x17001489 RID: 5257
		// (get) Token: 0x06003EFB RID: 16123 RVA: 0x0032EC27 File Offset: 0x0032CE27
		// (set) Token: 0x06003EFC RID: 16124 RVA: 0x0032EC2F File Offset: 0x0032CE2F
		public string CarName
		{
			get
			{
				return this._CarName;
			}
			set
			{
				if (this._CarName == value)
				{
					return;
				}
				this._CarName = value;
				this.OnPropertyChanged("CarName");
			}
		}

		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x06003EFD RID: 16125 RVA: 0x0032EC52 File Offset: 0x0032CE52
		// (set) Token: 0x06003EFE RID: 16126 RVA: 0x0032EC5A File Offset: 0x0032CE5A
		public string Device
		{
			get
			{
				return this._Device;
			}
			set
			{
				if (this._Device == value)
				{
					return;
				}
				this._Device = value;
				this.OnPropertyChanged("Device");
			}
		}

		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x06003EFF RID: 16127 RVA: 0x0032EC7D File Offset: 0x0032CE7D
		// (set) Token: 0x06003F00 RID: 16128 RVA: 0x0032EC85 File Offset: 0x0032CE85
		public string ConnectionProfile
		{
			get
			{
				return this._ConnectionProfile;
			}
			set
			{
				if (this._ConnectionProfile == value)
				{
					return;
				}
				this._ConnectionProfile = value;
				this.OnPropertyChanged("ConnectionProfile");
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06003F01 RID: 16129 RVA: 0x0032ECA8 File Offset: 0x0032CEA8
		// (remove) Token: 0x06003F02 RID: 16130 RVA: 0x0032ECE0 File Offset: 0x0032CEE0
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

		// Token: 0x1700148C RID: 5260
		// (get) Token: 0x06003F03 RID: 16131 RVA: 0x0032ED15 File Offset: 0x0032CF15
		// (set) Token: 0x06003F04 RID: 16132 RVA: 0x0032ED1D File Offset: 0x0032CF1D
		public bool SizeLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<SizeLoaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SizeLoaded>k__BackingField = value;
			}
		}

		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x06003F05 RID: 16133 RVA: 0x0032ED26 File Offset: 0x0032CF26
		// (set) Token: 0x06003F06 RID: 16134 RVA: 0x0032ED2E File Offset: 0x0032CF2E
		public bool MetaLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<MetaLoaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<MetaLoaded>k__BackingField = value;
			}
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0032ED38 File Offset: 0x0032CF38
		public void LoadSize()
		{
			try
			{
				long length = new FileInfo(this.Path).Length;
				if (length == 0L)
				{
					this.Size = "0 B";
				}
				else if (length < 1024L)
				{
					this.Size = length.ToString() + " B";
				}
				else if (length < 1048576L)
				{
					this.Size = (length / 1024L).ToString() + " KB";
				}
				else
				{
					double num = (double)length / 1048576.0;
					this.Size = Math.Round(num, 2).ToString(CultureInfo.InvariantCulture) + " MB";
				}
				this.SizeLoaded = true;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x0032EE00 File Offset: 0x0032D000
		public void LoadMeta()
		{
			try
			{
				string path = this.Path;
				if (global::System.IO.Path.GetExtension(path).ToLowerInvariant() == ".brc" && BRCHelper.GetVersion(path) == BRCHelper.BRCType.V2)
				{
					try
					{
						DataRecordContainer dataRecordContainer = DataRecordContainer.LoadFromFile(path, true);
						this.VIN = dataRecordContainer.VIN;
						this.CarName = dataRecordContainer.CarName;
						this.Device = dataRecordContainer.Device;
						this.ConnectionProfile = dataRecordContainer.ConnectionProfile;
					}
					catch (Exception)
					{
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x040026A0 RID: 9888
		private bool _IsSelected;

		// Token: 0x040026A1 RID: 9889
		[CompilerGenerated]
		private string <Path>k__BackingField;

		// Token: 0x040026A2 RID: 9890
		private string _Size = "";

		// Token: 0x040026A3 RID: 9891
		private string _VIN = "";

		// Token: 0x040026A4 RID: 9892
		private string _CarName = "";

		// Token: 0x040026A5 RID: 9893
		private string _Device = "";

		// Token: 0x040026A6 RID: 9894
		private string _ConnectionProfile = "";

		// Token: 0x040026A7 RID: 9895
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040026A8 RID: 9896
		[CompilerGenerated]
		private bool <SizeLoaded>k__BackingField;

		// Token: 0x040026A9 RID: 9897
		[CompilerGenerated]
		private bool <MetaLoaded>k__BackingField;
	}
}
