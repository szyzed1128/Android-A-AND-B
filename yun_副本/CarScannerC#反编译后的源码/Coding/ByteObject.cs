using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000849 RID: 2121
	internal class ByteObject : INotifyPropertyChanged
	{
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06004875 RID: 18549 RVA: 0x00370C5C File Offset: 0x0036EE5C
		// (remove) Token: 0x06004876 RID: 18550 RVA: 0x00370C94 File Offset: 0x0036EE94
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

		// Token: 0x06004877 RID: 18551 RVA: 0x00370CC9 File Offset: 0x0036EEC9
		public ByteObject(byte b)
		{
			this.B = b;
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x00370CD8 File Offset: 0x0036EED8
		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x06004879 RID: 18553 RVA: 0x00370CF1 File Offset: 0x0036EEF1
		// (set) Token: 0x0600487A RID: 18554 RVA: 0x00370CFC File Offset: 0x0036EEFC
		public byte B
		{
			get
			{
				return this._B;
			}
			set
			{
				this._B = value;
				this.OnPropertyChanged("Hex");
				this.OnPropertyChanged("Bin");
				this.OnPropertyChanged("Bit0");
				this.OnPropertyChanged("Bit1");
				this.OnPropertyChanged("Bit2");
				this.OnPropertyChanged("Bit3");
				this.OnPropertyChanged("Bit4");
				this.OnPropertyChanged("Bit5");
				this.OnPropertyChanged("Bit6");
				this.OnPropertyChanged("Bit7");
			}
		}

		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x0600487B RID: 18555 RVA: 0x00370D7E File Offset: 0x0036EF7E
		// (set) Token: 0x0600487C RID: 18556 RVA: 0x00370D8C File Offset: 0x0036EF8C
		public bool Bit0
		{
			get
			{
				return BitHelpers.GetBit_0_7(this.B, 0);
			}
			set
			{
				this.B = BitHelpers.SetBit(this.B, 0, value);
			}
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x0600487D RID: 18557 RVA: 0x00370DA1 File Offset: 0x0036EFA1
		// (set) Token: 0x0600487E RID: 18558 RVA: 0x00370DAF File Offset: 0x0036EFAF
		public bool Bit1
		{
			get
			{
				return BitHelpers.GetBit_0_7(this.B, 1);
			}
			set
			{
				this.B = BitHelpers.SetBit(this.B, 1, value);
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x0600487F RID: 18559 RVA: 0x00370DC4 File Offset: 0x0036EFC4
		// (set) Token: 0x06004880 RID: 18560 RVA: 0x00370DD2 File Offset: 0x0036EFD2
		public bool Bit2
		{
			get
			{
				return BitHelpers.GetBit_0_7(this.B, 2);
			}
			set
			{
				this.B = BitHelpers.SetBit(this.B, 2, value);
			}
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x06004881 RID: 18561 RVA: 0x00370DE7 File Offset: 0x0036EFE7
		// (set) Token: 0x06004882 RID: 18562 RVA: 0x00370DF5 File Offset: 0x0036EFF5
		public bool Bit3
		{
			get
			{
				return BitHelpers.GetBit_0_7(this.B, 3);
			}
			set
			{
				this.B = BitHelpers.SetBit(this.B, 3, value);
			}
		}

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x00370E0A File Offset: 0x0036F00A
		// (set) Token: 0x06004884 RID: 18564 RVA: 0x00370E18 File Offset: 0x0036F018
		public bool Bit4
		{
			get
			{
				return BitHelpers.GetBit_0_7(this.B, 4);
			}
			set
			{
				this.B = BitHelpers.SetBit(this.B, 4, value);
			}
		}

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x06004885 RID: 18565 RVA: 0x00370E2D File Offset: 0x0036F02D
		// (set) Token: 0x06004886 RID: 18566 RVA: 0x00370E3B File Offset: 0x0036F03B
		public bool Bit5
		{
			get
			{
				return BitHelpers.GetBit_0_7(this.B, 5);
			}
			set
			{
				this.B = BitHelpers.SetBit(this.B, 5, value);
			}
		}

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x06004887 RID: 18567 RVA: 0x00370E50 File Offset: 0x0036F050
		// (set) Token: 0x06004888 RID: 18568 RVA: 0x00370E5E File Offset: 0x0036F05E
		public bool Bit6
		{
			get
			{
				return BitHelpers.GetBit_0_7(this.B, 6);
			}
			set
			{
				this.B = BitHelpers.SetBit(this.B, 6, value);
			}
		}

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x06004889 RID: 18569 RVA: 0x00370E73 File Offset: 0x0036F073
		// (set) Token: 0x0600488A RID: 18570 RVA: 0x00370E81 File Offset: 0x0036F081
		public bool Bit7
		{
			get
			{
				return BitHelpers.GetBit_0_7(this.B, 7);
			}
			set
			{
				this.B = BitHelpers.SetBit(this.B, 7, value);
			}
		}

		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x0600488B RID: 18571 RVA: 0x00370E98 File Offset: 0x0036F098
		// (set) Token: 0x0600488C RID: 18572 RVA: 0x00370EB8 File Offset: 0x0036F0B8
		public string Hex
		{
			get
			{
				return this.B.ToString("X2");
			}
			set
			{
				byte b;
				if (byte.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out b))
				{
					this.B = b;
				}
			}
		}

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x00370EE5 File Offset: 0x0036F0E5
		// (set) Token: 0x0600488E RID: 18574 RVA: 0x00370EFC File Offset: 0x0036F0FC
		public string Bin
		{
			get
			{
				return Convert.ToString(this.B, 2).PadLeft(8, '0');
			}
			set
			{
				try
				{
					this.B = Convert.ToByte(value, 2);
				}
				catch (Exception)
				{
					this.B = this.B;
				}
			}
		}

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x0600488F RID: 18575 RVA: 0x00370F38 File Offset: 0x0036F138
		// (set) Token: 0x06004890 RID: 18576 RVA: 0x00370F40 File Offset: 0x0036F140
		public int ByteIdx
		{
			get
			{
				return this._ByteIdx;
			}
			set
			{
				this._ByteIdx = value;
				this.OnPropertyChanged("ByteIdx");
			}
		}

		// Token: 0x040029DF RID: 10719
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040029E0 RID: 10720
		private byte _B;

		// Token: 0x040029E1 RID: 10721
		private int _ByteIdx;
	}
}
