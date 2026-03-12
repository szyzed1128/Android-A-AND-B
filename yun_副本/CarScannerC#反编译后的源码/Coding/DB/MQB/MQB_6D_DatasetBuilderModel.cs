using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B24 RID: 2852
	internal class MQB_6D_DatasetBuilderModel : INotifyPropertyChanged
	{
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06005878 RID: 22648 RVA: 0x00423A00 File Offset: 0x00421C00
		// (remove) Token: 0x06005879 RID: 22649 RVA: 0x00423A38 File Offset: 0x00421C38
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

		// Token: 0x0600587A RID: 22650 RVA: 0x00423A6D File Offset: 0x00421C6D
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x00423A88 File Offset: 0x00421C88
		public void LoadFromBytes(byte[] data, string unitId, byte[] longCoding)
		{
			this.originalData = data;
			this.Version = Encoding.ASCII.GetString(data, 0, 4);
			this.unitId = unitId;
			this.longCoding = longCoding;
			byte b = data[100];
			if (b != 17)
			{
				if (b != 26)
				{
					if (b != 27)
					{
						this.Byte64 = Byte64Values.Unknown;
					}
					else
					{
						this.Byte64 = Byte64Values.OpenCloseFromRemote;
					}
				}
				else
				{
					this.Byte64 = Byte64Values.OnlyOpenFromRemote;
				}
			}
			else
			{
				this.Byte64 = Byte64Values.NoEasyClose;
			}
			byte b2 = data[101];
			this.IsClosingForbiddenWhenKeyIsNear = BitHelpers.GetBit_0_7(b2, 3);
			this.IsRemoteClosingForbidden = BitHelpers.GetBit_0_7(b2, 6);
			this.IsOpeningForbiddenWhileIgnitionTurnedOn = BitHelpers.GetBit_0_7(b2, 7);
			int num = (int)(data[102] & 15);
			switch (num)
			{
			case 3:
				this.Byte66InteriorButton = Byte66LowNibblePartValues.AllowOnlyOpeningWhileIgnitionTurnedOff;
				goto IL_00F5;
			case 4:
				this.Byte66InteriorButton = Byte66LowNibblePartValues.AllowOnlyOpeningWhileIgnitionTurnedOnOrOff;
				goto IL_00F5;
			case 5:
			case 6:
				break;
			case 7:
				this.Byte66InteriorButton = Byte66LowNibblePartValues.AllowOnlyOpeningWhileIgnitionTurnedOn;
				goto IL_00F5;
			default:
				if (num == 11)
				{
					this.Byte66InteriorButton = Byte66LowNibblePartValues.AllowOpenAndCloseWhileIgnitionOnAndOff;
					goto IL_00F5;
				}
				if (num == 13)
				{
					this.Byte66InteriorButton = Byte66LowNibblePartValues.AllowClosingWhileEngineRunning;
					goto IL_00F5;
				}
				break;
			}
			this.Byte66InteriorButton = Byte66LowNibblePartValues.Unknown;
			IL_00F5:
			this.InteriorButtonOnlyUnlocksTrunk = BitHelpers.GetBit_0_7(data[103], 0);
			this.InteriorButtonCloseByHolding = BitHelpers.GetBit_0_7(data[103], 6);
			this.InteriorButtonRestricted = BitHelpers.GetBit_0_7(data[103], 7);
			this.KickCloseEnabled = BitHelpers.GetBit_0_7(data[107], 7);
			b = data[108];
			if (b != 175)
			{
				if (b == 223)
				{
					this.EasyCloseEnabled = EasyCloseModes.Disabled;
				}
				else
				{
					this.EasyCloseEnabled = EasyCloseModes.Unknown;
				}
			}
			else
			{
				this.EasyCloseEnabled = EasyCloseModes.Enabled;
			}
			this.OnPropertyChanged("IsByte62EqualsToLongCodingFirstByte");
			this.UseUserDefinedBytes = false;
			this.Bytes64_67 = data[100].ToString("X2") + data[101].ToString("X2") + data[102].ToString("X2") + data[103].ToString("X2");
			this.Bytes6B_6C = data[107].ToString("X2") + data[108].ToString("X2");
			this.IsLoaded = true;
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x0600587C RID: 22652 RVA: 0x00423C9B File Offset: 0x00421E9B
		// (set) Token: 0x0600587D RID: 22653 RVA: 0x00423CA3 File Offset: 0x00421EA3
		public string Version
		{
			[CompilerGenerated]
			get
			{
				return this.<Version>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Version>k__BackingField = value;
			}
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x00423CAC File Offset: 0x00421EAC
		public byte[] GetOriginalData()
		{
			byte[] array = new byte[this.originalData.Length];
			Array.Copy(this.originalData, array, array.Length);
			return array;
		}

		// Token: 0x0600587F RID: 22655 RVA: 0x00423CD8 File Offset: 0x00421ED8
		public byte[] BuildNewData()
		{
			byte[] array = new byte[this.originalData.Length - 4];
			Array.Copy(this.originalData, 0, array, 0, array.Length);
			if (this.ShouldSetByte62FromLongCoding)
			{
				array[98] = this.longCoding[0];
			}
			if (this.UseUserDefinedBytes)
			{
				byte[] array2 = BitHelpers.ConvertHexToBytesX(this.Bytes64_67.Replace(" ", ""));
				byte[] array3 = BitHelpers.ConvertHexToBytesX(this.Bytes6B_6C.Replace(" ", ""));
				if (array2.Length != 4 || array3.Length != 2)
				{
					throw new ArgumentException();
				}
				Array.Copy(array2, 0, array, 100, array2.Length);
				Array.Copy(array3, 0, array, 107, array3.Length);
			}
			else
			{
				if (this.Byte64 != Byte64Values.Unknown)
				{
					array[100] = (byte)this.Byte64;
				}
				byte b = array[101];
				BitHelpers.SwitchBitInByte(array, 101, 3, this.IsClosingForbiddenWhenKeyIsNear);
				BitHelpers.SwitchBitInByte(array, 101, 6, this.IsRemoteClosingForbidden);
				BitHelpers.SwitchBitInByte(array, 101, 7, this.IsOpeningForbiddenWhileIgnitionTurnedOn);
				if (this.Byte66InteriorButton != Byte66LowNibblePartValues.Unknown)
				{
					int num = (int)array[102];
					num &= 240;
					num ^= (int)((byte)this.Byte66InteriorButton);
					array[102] = (byte)num;
				}
				BitHelpers.SwitchBitInByte(array, 103, 0, this.InteriorButtonOnlyUnlocksTrunk);
				BitHelpers.SwitchBitInByte(array, 103, 6, this.InteriorButtonCloseByHolding);
				BitHelpers.SwitchBitInByte(array, 103, 7, this.InteriorButtonRestricted);
				BitHelpers.SwitchBitInByte(array, 107, 7, this.KickCloseEnabled);
				if (this.EasyCloseEnabled != EasyCloseModes.Unknown)
				{
					array[108] = (byte)this.EasyCloseEnabled;
				}
			}
			byte[] array4 = Crc32.Calculate(array);
			Array.Reverse<byte>(array4);
			return array.Concat(array4).ToArray<byte>();
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x06005880 RID: 22656 RVA: 0x00423E61 File Offset: 0x00422061
		public bool IsByte62EqualsToLongCodingFirstByte
		{
			get
			{
				return this.originalData[98] == this.longCoding[0];
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x06005881 RID: 22657 RVA: 0x00423E79 File Offset: 0x00422079
		// (set) Token: 0x06005882 RID: 22658 RVA: 0x00423E81 File Offset: 0x00422081
		public bool ShouldSetByte62FromLongCoding
		{
			get
			{
				return this._ShouldSetByte62FromLongCoding;
			}
			set
			{
				if (value != this._ShouldSetByte62FromLongCoding)
				{
					this._ShouldSetByte62FromLongCoding = value;
					this.OnPropertyChanged("ShouldSetByte62FromLongCoding");
				}
			}
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06005883 RID: 22659 RVA: 0x00423E9E File Offset: 0x0042209E
		// (set) Token: 0x06005884 RID: 22660 RVA: 0x00423EA6 File Offset: 0x004220A6
		public Byte64Values Byte64
		{
			get
			{
				return this._Byte64;
			}
			set
			{
				if (this._Byte64 != value)
				{
					this._Byte64 = value;
					this.OnPropertyChanged("Byte64");
				}
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06005885 RID: 22661 RVA: 0x00423EC3 File Offset: 0x004220C3
		// (set) Token: 0x06005886 RID: 22662 RVA: 0x00423ECB File Offset: 0x004220CB
		public bool IsClosingForbiddenWhenKeyIsNear
		{
			get
			{
				return this._IsClosingForbiddenWhenKeyIsNear;
			}
			set
			{
				if (this._IsClosingForbiddenWhenKeyIsNear != value)
				{
					this._IsClosingForbiddenWhenKeyIsNear = value;
					this.OnPropertyChanged("IsClosingForbiddenWhenKeyIsNear");
				}
			}
		}

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x06005887 RID: 22663 RVA: 0x00423EE8 File Offset: 0x004220E8
		// (set) Token: 0x06005888 RID: 22664 RVA: 0x00423EF0 File Offset: 0x004220F0
		public bool IsRemoteClosingForbidden
		{
			get
			{
				return this._IsRemoteClosingForbidden;
			}
			set
			{
				if (this._IsRemoteClosingForbidden != value)
				{
					this._IsRemoteClosingForbidden = value;
					this.OnPropertyChanged("IsRemoteClosingForbidden");
				}
			}
		}

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x06005889 RID: 22665 RVA: 0x00423F0D File Offset: 0x0042210D
		// (set) Token: 0x0600588A RID: 22666 RVA: 0x00423F15 File Offset: 0x00422115
		public bool IsOpeningForbiddenWhileIgnitionTurnedOn
		{
			get
			{
				return this._IsOpeningForbiddenWhileIgnitionTurnedOn;
			}
			set
			{
				if (this._IsOpeningForbiddenWhileIgnitionTurnedOn != value)
				{
					this._IsOpeningForbiddenWhileIgnitionTurnedOn = value;
					this.OnPropertyChanged("IsOpeningForbiddenWhileIgnitionTurnedOn");
				}
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x0600588B RID: 22667 RVA: 0x00423F32 File Offset: 0x00422132
		// (set) Token: 0x0600588C RID: 22668 RVA: 0x00423F3A File Offset: 0x0042213A
		public Byte66LowNibblePartValues Byte66InteriorButton
		{
			get
			{
				return this._Byte66InteriorButton;
			}
			set
			{
				if (value != this._Byte66InteriorButton)
				{
					this._Byte66InteriorButton = value;
					this.OnPropertyChanged("Byte66InteriorButton");
				}
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x0600588D RID: 22669 RVA: 0x00423F57 File Offset: 0x00422157
		// (set) Token: 0x0600588E RID: 22670 RVA: 0x00423F5F File Offset: 0x0042215F
		public bool InteriorButtonOnlyUnlocksTrunk
		{
			get
			{
				return this._InteriorButtonOnlyUnlocksTrunk;
			}
			set
			{
				if (value != this._InteriorButtonOnlyUnlocksTrunk)
				{
					this._InteriorButtonOnlyUnlocksTrunk = value;
					this.OnPropertyChanged("InteriorButtonOnlyUnlocksTrunk");
				}
			}
		}

		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x0600588F RID: 22671 RVA: 0x00423F7C File Offset: 0x0042217C
		// (set) Token: 0x06005890 RID: 22672 RVA: 0x00423F84 File Offset: 0x00422184
		public bool InteriorButtonCloseByHolding
		{
			get
			{
				return this._InteriorButtonCloseByHolding4;
			}
			set
			{
				if (value != this._InteriorButtonCloseByHolding4)
				{
					this._InteriorButtonCloseByHolding4 = value;
					this.OnPropertyChanged("InteriorButtonCloseByHolding");
				}
			}
		}

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x06005891 RID: 22673 RVA: 0x00423FA1 File Offset: 0x004221A1
		// (set) Token: 0x06005892 RID: 22674 RVA: 0x00423FA9 File Offset: 0x004221A9
		public bool InteriorButtonRestricted
		{
			get
			{
				return this._InteriorButtonRestricted;
			}
			set
			{
				if (value != this._InteriorButtonRestricted)
				{
					this._InteriorButtonRestricted = value;
					this.OnPropertyChanged("InteriorButtonRestricted");
				}
			}
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x06005893 RID: 22675 RVA: 0x00423FC6 File Offset: 0x004221C6
		// (set) Token: 0x06005894 RID: 22676 RVA: 0x00423FCE File Offset: 0x004221CE
		public bool KickCloseEnabled
		{
			get
			{
				return this._KickCloseEnabled;
			}
			set
			{
				if (this._KickCloseEnabled != value)
				{
					this._KickCloseEnabled = value;
					this.OnPropertyChanged("KickCloseEnabled");
				}
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x06005895 RID: 22677 RVA: 0x00423FEB File Offset: 0x004221EB
		// (set) Token: 0x06005896 RID: 22678 RVA: 0x00423FF3 File Offset: 0x004221F3
		public EasyCloseModes EasyCloseEnabled
		{
			get
			{
				return this._EasyCloseEnabled;
			}
			set
			{
				if (value != this._EasyCloseEnabled)
				{
					this._EasyCloseEnabled = value;
					this.OnPropertyChanged("EasyCloseEnabled");
				}
			}
		}

		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x06005897 RID: 22679 RVA: 0x00424010 File Offset: 0x00422210
		// (set) Token: 0x06005898 RID: 22680 RVA: 0x00424018 File Offset: 0x00422218
		public bool UseUserDefinedBytes
		{
			get
			{
				return this._UseUserDefinedBytes;
			}
			set
			{
				if (this._UseUserDefinedBytes != value)
				{
					this._UseUserDefinedBytes = value;
					this.OnPropertyChanged("UseUserDefinedBytes");
				}
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x06005899 RID: 22681 RVA: 0x00424035 File Offset: 0x00422235
		// (set) Token: 0x0600589A RID: 22682 RVA: 0x0042403D File Offset: 0x0042223D
		public string Bytes64_67
		{
			get
			{
				return this._Bytes64_67;
			}
			set
			{
				if (value != this._Bytes64_67)
				{
					this._Bytes64_67 = value;
					this.OnPropertyChanged("Bytes64_67");
				}
			}
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x0600589B RID: 22683 RVA: 0x0042405F File Offset: 0x0042225F
		// (set) Token: 0x0600589C RID: 22684 RVA: 0x00424067 File Offset: 0x00422267
		public string Bytes6B_6C
		{
			get
			{
				return this._Bytes6B_6C;
			}
			set
			{
				if (value != this._Bytes6B_6C)
				{
					this._Bytes6B_6C = value;
					this.OnPropertyChanged("Bytes6B_6C");
				}
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x0600589D RID: 22685 RVA: 0x00424089 File Offset: 0x00422289
		// (set) Token: 0x0600589E RID: 22686 RVA: 0x00424091 File Offset: 0x00422291
		public bool IsLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<IsLoaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsLoaded>k__BackingField = value;
			}
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x0042409C File Offset: 0x0042229C
		public void LoadFromUserBytes()
		{
			byte[] array = this.originalData.ToArray<byte>();
			try
			{
				byte[] array2 = BitHelpers.ConvertHexToBytesX(this.Bytes64_67.Replace(" ", ""));
				byte[] array3 = BitHelpers.ConvertHexToBytesX(this.Bytes6B_6C.Replace(" ", ""));
				if (array2.Length != 4 || array3.Length != 2)
				{
					throw new ArgumentException();
				}
				Array.Copy(array2, 0, array, 100, array2.Length);
				Array.Copy(array3, 0, array, 107, array3.Length);
				this.LoadFromBytes(array, this.unitId, this.longCoding);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x00424144 File Offset: 0x00422344
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.Version,
				";",
				this.originalData[98].ToString("X2"),
				";",
				this.originalData[100].ToString("X2"),
				";",
				this.originalData[101].ToString("X2"),
				";",
				this.originalData[102].ToString("X2"),
				";",
				this.originalData[103].ToString("X2"),
				";",
				this.originalData[107].ToString("X2"),
				";",
				string.Format("IsByte62EqualsToLongCodingFirstByte={0};", this.IsByte62EqualsToLongCodingFirstByte),
				string.Format("Byte64={0};", this.Byte64),
				string.Format("IsClosingForbiddenWhenKeyIsNear={0};", this.IsClosingForbiddenWhenKeyIsNear),
				string.Format("IsRemoteClosingForbidden={0};", this.IsRemoteClosingForbidden),
				string.Format("IsOpeningForbiddenWhileIgnitionTurnedOn={0};", this.IsOpeningForbiddenWhileIgnitionTurnedOn),
				string.Format("Byte66InteriorButton={0};", this.Byte66InteriorButton),
				string.Format("InteriorButtonOnlyUnlocksTrunk={0};", this.InteriorButtonOnlyUnlocksTrunk),
				string.Format("InteriorButtonCloseByHolding={0};", this.InteriorButtonCloseByHolding),
				string.Format("InteriorButtonRestricted={0};", this.InteriorButtonRestricted),
				string.Format("KickCloseEnabled={0};", this.KickCloseEnabled),
				string.Format("EasyCloseEnabled={0}", this.EasyCloseEnabled)
			});
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x00424354 File Offset: 0x00422554
		public MQB_6D_DatasetBuilderModel()
		{
		}

		// Token: 0x04003721 RID: 14113
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04003722 RID: 14114
		private byte[] originalData = new byte[116];

		// Token: 0x04003723 RID: 14115
		private byte[] longCoding = new byte[2];

		// Token: 0x04003724 RID: 14116
		private string unitId = "";

		// Token: 0x04003725 RID: 14117
		[CompilerGenerated]
		private string <Version>k__BackingField;

		// Token: 0x04003726 RID: 14118
		private bool _ShouldSetByte62FromLongCoding;

		// Token: 0x04003727 RID: 14119
		private Byte64Values _Byte64;

		// Token: 0x04003728 RID: 14120
		private bool _IsClosingForbiddenWhenKeyIsNear;

		// Token: 0x04003729 RID: 14121
		private bool _IsRemoteClosingForbidden;

		// Token: 0x0400372A RID: 14122
		private bool _IsOpeningForbiddenWhileIgnitionTurnedOn;

		// Token: 0x0400372B RID: 14123
		private Byte66LowNibblePartValues _Byte66InteriorButton;

		// Token: 0x0400372C RID: 14124
		private bool _InteriorButtonOnlyUnlocksTrunk;

		// Token: 0x0400372D RID: 14125
		private bool _InteriorButtonCloseByHolding4;

		// Token: 0x0400372E RID: 14126
		private bool _InteriorButtonRestricted;

		// Token: 0x0400372F RID: 14127
		private bool _KickCloseEnabled;

		// Token: 0x04003730 RID: 14128
		private EasyCloseModes _EasyCloseEnabled;

		// Token: 0x04003731 RID: 14129
		private bool _UseUserDefinedBytes;

		// Token: 0x04003732 RID: 14130
		private string _Bytes64_67 = "";

		// Token: 0x04003733 RID: 14131
		[CompilerGenerated]
		private bool <IsLoaded>k__BackingField;

		// Token: 0x04003734 RID: 14132
		private string _Bytes6B_6C = "";
	}
}
