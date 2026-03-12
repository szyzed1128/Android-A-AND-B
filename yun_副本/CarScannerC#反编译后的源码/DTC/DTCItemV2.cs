using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000544 RID: 1348
	public class DTCItemV2 : INotifyPropertyChanged
	{
		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06003268 RID: 12904 RVA: 0x0022F4B3 File Offset: 0x0022D6B3
		// (set) Token: 0x06003269 RID: 12905 RVA: 0x0022F4BB File Offset: 0x0022D6BB
		public string Code
		{
			[CompilerGenerated]
			get
			{
				return this.<Code>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Code>k__BackingField = value;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x0600326A RID: 12906 RVA: 0x0022F4C4 File Offset: 0x0022D6C4
		// (set) Token: 0x0600326B RID: 12907 RVA: 0x0022F4CC File Offset: 0x0022D6CC
		public string RawCode
		{
			[CompilerGenerated]
			get
			{
				return this.<RawCode>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<RawCode>k__BackingField = value;
			}
		}

		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x0600326C RID: 12908 RVA: 0x0022F4D5 File Offset: 0x0022D6D5
		// (set) Token: 0x0600326D RID: 12909 RVA: 0x0022F4DD File Offset: 0x0022D6DD
		public DTCItemV2.DTCItemType ItemType
		{
			[CompilerGenerated]
			get
			{
				return this.<ItemType>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ItemType>k__BackingField = value;
			}
		}

		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x0600326E RID: 12910 RVA: 0x0022F4E6 File Offset: 0x0022D6E6
		// (set) Token: 0x0600326F RID: 12911 RVA: 0x0022F4EE File Offset: 0x0022D6EE
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

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x06003270 RID: 12912 RVA: 0x0022F4F7 File Offset: 0x0022D6F7
		// (set) Token: 0x06003271 RID: 12913 RVA: 0x0022F4FF File Offset: 0x0022D6FF
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

		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x06003272 RID: 12914 RVA: 0x0022F508 File Offset: 0x0022D708
		// (set) Token: 0x06003273 RID: 12915 RVA: 0x0022F510 File Offset: 0x0022D710
		public string RequestCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestCommand>k__BackingField = value;
			}
		}

		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x0022F519 File Offset: 0x0022D719
		// (set) Token: 0x06003275 RID: 12917 RVA: 0x0022F521 File Offset: 0x0022D721
		public string Payload
		{
			get
			{
				return this._Payload;
			}
			set
			{
				this._Payload = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Payload"));
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x0022F548 File Offset: 0x0022D748
		public static string GetVAGCode(string code)
		{
			string text;
			try
			{
				byte[] array = BitHelpers.HexStringToByteArray(OBDDataReader.FilterHexAndNewLineOnly(code));
				int num = 0;
				Array.Reverse<byte>(array);
				for (int i = 0; i < array.Length; i++)
				{
					num += (int)array[i] * (int)Math.Pow(256.0, (double)i);
				}
				text = num.ToString("00000");
			}
			catch (Exception)
			{
				text = "";
			}
			return text;
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x06003277 RID: 12919 RVA: 0x0022F5B8 File Offset: 0x0022D7B8
		[JsonIgnore]
		public bool IsVAG
		{
			get
			{
				string text = ((!string.IsNullOrEmpty(this.SelectedBrand)) ? this.SelectedBrand : SharedSettings.Current.SelectedBrand);
				return text == "Audi" || text == "Volkswagen" || text == "Seat" || text == "Skoda" || text == "Porsche" || text == "Cupra" || text == "Bentley" || text == "Bugatti";
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x0022F650 File Offset: 0x0022D850
		[JsonIgnore]
		public bool IsBMW
		{
			get
			{
				string text = ((!string.IsNullOrEmpty(this.SelectedBrand)) ? this.SelectedBrand : SharedSettings.Current.SelectedBrand);
				return text == "BMW" || text == "Mini";
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06003279 RID: 12921 RVA: 0x0022F69A File Offset: 0x0022D89A
		// (set) Token: 0x0600327A RID: 12922 RVA: 0x0022F6A2 File Offset: 0x0022D8A2
		public string SelectedBrand
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedBrand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SelectedBrand>k__BackingField = value;
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x0600327B RID: 12923 RVA: 0x0022F6AB File Offset: 0x0022D8AB
		// (set) Token: 0x0600327C RID: 12924 RVA: 0x0022F6B3 File Offset: 0x0022D8B3
		public ObservableCollection<BrandAndDescription> Descriptions
		{
			[CompilerGenerated]
			get
			{
				return this.<Descriptions>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Descriptions>k__BackingField = value;
			}
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x0600327D RID: 12925 RVA: 0x0022F6BC File Offset: 0x0022D8BC
		// (set) Token: 0x0600327E RID: 12926 RVA: 0x0022F6C4 File Offset: 0x0022D8C4
		public List<DTCStatusHelper.DTCStatus> Statuses
		{
			[CompilerGenerated]
			get
			{
				return this.<Statuses>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Statuses>k__BackingField = value;
			}
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x0600327F RID: 12927 RVA: 0x0022F6CD File Offset: 0x0022D8CD
		// (set) Token: 0x06003280 RID: 12928 RVA: 0x0022F6D5 File Offset: 0x0022D8D5
		public List<byte> PayloadData
		{
			[CompilerGenerated]
			get
			{
				return this.<PayloadData>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PayloadData>k__BackingField = value;
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x0022F6DE File Offset: 0x0022D8DE
		// (set) Token: 0x06003282 RID: 12930 RVA: 0x0022F6E6 File Offset: 0x0022D8E6
		public bool OnlyTestNotComplitedDTC
		{
			[CompilerGenerated]
			get
			{
				return this.<OnlyTestNotComplitedDTC>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OnlyTestNotComplitedDTC>k__BackingField = value;
			}
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x06003283 RID: 12931 RVA: 0x0022F6EF File Offset: 0x0022D8EF
		// (set) Token: 0x06003284 RID: 12932 RVA: 0x0022F6F7 File Offset: 0x0022D8F7
		public bool IsArchive
		{
			[CompilerGenerated]
			get
			{
				return this.<IsArchive>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsArchive>k__BackingField = value;
			}
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x06003285 RID: 12933 RVA: 0x0022F700 File Offset: 0x0022D900
		[JsonIgnore]
		public bool StatusVisible
		{
			get
			{
				return !App.OBDReader.IsNissanConsult2Protocol || App.OBDReader.CurrentELMFormat != ELMFormat.KWP;
			}
		}

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x0022F71E File Offset: 0x0022D91E
		[JsonIgnore]
		public bool DescriptionsVisible
		{
			get
			{
				return this.Descriptions.Count > 0;
			}
		}

		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x06003287 RID: 12935 RVA: 0x0022F731 File Offset: 0x0022D931
		// (set) Token: 0x06003288 RID: 12936 RVA: 0x0022F739 File Offset: 0x0022D939
		public string ECU
		{
			[CompilerGenerated]
			get
			{
				return this.<ECU>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ECU>k__BackingField = value;
			}
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x0022F742 File Offset: 0x0022D942
		// (set) Token: 0x0600328A RID: 12938 RVA: 0x0022F74A File Offset: 0x0022D94A
		public string ExtendedAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<ExtendedAddress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ExtendedAddress>k__BackingField = value;
			}
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x0022F753 File Offset: 0x0022D953
		public DTCItemV2(byte[] rawCode, string requestHeader, string responseHeader, string requestCommand, DTCStatusHelper.DTCStatus status, string extendedAddress)
			: this(rawCode, requestHeader, responseHeader, requestCommand, new List<DTCStatusHelper.DTCStatus> { status }, extendedAddress)
		{
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x0022F76F File Offset: 0x0022D96F
		public DTCItemV2(byte[] rawCode, string requestHeader, string responseHeader, string requestCommand, byte statusByte, string extendedAddress)
			: this(rawCode, requestHeader, responseHeader, requestCommand, DTCStatusHelper.GetStatuses(statusByte, requestCommand), extendedAddress)
		{
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x0022F788 File Offset: 0x0022D988
		private DTCItemV2(byte[] rawCode, string requestHeader, string responseHeader, string requestCommand, List<DTCStatusHelper.DTCStatus> statuses, string extendedAddress)
		{
			this.Code = "";
			this.RawCode = "";
			this.RequestHeader = "";
			this.ResponseHeader = "";
			this.RequestCommand = "";
			this._Payload = "";
			this.SelectedBrand = "";
			this.Descriptions = new ObservableCollection<BrandAndDescription>();
			this.Statuses = new List<DTCStatusHelper.DTCStatus>(8);
			this.PayloadData = new List<byte>(0);
			this.ECU = "";
			this.ExtendedAddress = "";
			base..ctor();
			this.RawCode = "";
			foreach (byte b in rawCode)
			{
				this.RawCode += b.ToString("X2");
			}
			this.Code = DTCItemV2.DecodeRawCode(rawCode);
			this.RequestHeader = requestHeader;
			this.ResponseHeader = responseHeader;
			this.RequestCommand = requestCommand;
			this.Statuses = statuses;
			this.ExtendedAddress = extendedAddress;
			if (this.RawCode.Length == 6)
			{
				this.ItemType = DTCItemV2.DTCItemType.UDS;
			}
			else if (requestCommand.StartsWith("18") || requestCommand.StartsWith("17"))
			{
				this.ItemType = DTCItemV2.DTCItemType.KWP2000;
			}
			else if (requestCommand.StartsWith("19"))
			{
				this.ItemType = DTCItemV2.DTCItemType.UDS;
			}
			else if (requestCommand == "03" || requestCommand == "07" || requestCommand == "0A")
			{
				this.ItemType = DTCItemV2.DTCItemType.OBDII;
			}
			else
			{
				this.ItemType = DTCItemV2.DTCItemType.Unknown;
			}
			if (requestCommand == "03A98102" || requestCommand == "03A98102")
			{
				this.ItemType = DTCItemV2.DTCItemType.GMA9;
			}
			if (requestCommand == "1201")
			{
				this.ItemType = DTCItemV2.DTCItemType.GM1201;
			}
			if (requestCommand.StartsWith("190F"))
			{
				this.IsArchive = true;
			}
			if (requestCommand.StartsWith("1902") && this.ItemType == DTCItemV2.DTCItemType.UDS && (this.Statuses.Contains(DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC) || this.Statuses.Contains(DTCStatusHelper.DTCStatus.uds_bit5_testFailedSinceLastClear)) && !this.Statuses.Contains(DTCStatusHelper.DTCStatus.uds_bit0_testFailed) && !this.Statuses.Contains(DTCStatusHelper.DTCStatus.uds_bit1_testFailedThisOperationCycle) && !this.Statuses.Contains(DTCStatusHelper.DTCStatus.uds_bit7_warningIndicatorRequested) && !this.Statuses.Contains(DTCStatusHelper.DTCStatus.uds_bit2_pendingDTC))
			{
				this.IsArchive = true;
			}
			if (this.ItemType == DTCItemV2.DTCItemType.KWP2000 && this.Statuses.Count == 1 && this.Statuses[0] == DTCStatusHelper.DTCStatus.kwp2000_bit5_DTCStorageState)
			{
				this.IsArchive = true;
			}
			if (this.ItemType == DTCItemV2.DTCItemType.OBDII && requestCommand == "0A")
			{
				if (this.Statuses.All((DTCStatusHelper.DTCStatus x) => x == DTCStatusHelper.DTCStatus.obd_0A_permanent))
				{
					this.IsArchive = true;
				}
			}
			if (statuses.Count > 0 && statuses.ToList<DTCStatusHelper.DTCStatus>().Except(DTCItemV2.TestNotComplitedStatuses).ToList<DTCStatusHelper.DTCStatus>()
				.Count == 0)
			{
				this.OnlyTestNotComplitedDTC = true;
			}
			this.SelectedBrand = SharedSettings.Current.SelectedBrand;
			if (this.ItemType == DTCItemV2.DTCItemType.UDS && this.IsVAG)
			{
				string vagcode = DTCItemV2.GetVAGCode(this.RawCode);
				if (!string.IsNullOrEmpty(vagcode))
				{
					this.Code = vagcode;
				}
			}
			if ((this.ItemType == DTCItemV2.DTCItemType.UDS || this.ItemType == DTCItemV2.DTCItemType.KWP2000) && this.IsBMW)
			{
				this.Code = this.RawCode;
			}
			if (responseHeader.Length == 3)
			{
				this.ECU = CAN11bitHelper.GetECUName(responseHeader, SharedSettings.Current.SelectedBrand, extendedAddress);
				return;
			}
			this.ECU = responseHeader;
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x0022FB0C File Offset: 0x0022DD0C
		[JsonConstructor]
		public DTCItemV2(string Code, string RawCode, DTCItemV2.DTCItemType ItemType, string RequestHeader, string ResponseHeader, string RequestCommand, string ECU, string SelectedBrand, string ExtendedAddress = "")
		{
			this.Code = "";
			this.RawCode = "";
			this.RequestHeader = "";
			this.ResponseHeader = "";
			this.RequestCommand = "";
			this._Payload = "";
			this.SelectedBrand = "";
			this.Descriptions = new ObservableCollection<BrandAndDescription>();
			this.Statuses = new List<DTCStatusHelper.DTCStatus>(8);
			this.PayloadData = new List<byte>(0);
			this.ECU = "";
			this.ExtendedAddress = "";
			base..ctor();
			this.Code = Code;
			this.RawCode = RawCode;
			this.ItemType = ItemType;
			this.RequestHeader = RequestHeader;
			this.ResponseHeader = ResponseHeader;
			this.RequestCommand = RequestCommand;
			this.SelectedBrand = SelectedBrand;
			this.ExtendedAddress = ExtendedAddress;
			if (ECU == null)
			{
				ECU = "";
			}
			if (SelectedBrand == null)
			{
				SelectedBrand = "";
			}
			this.SelectedBrand = SelectedBrand;
			if (this.IsVAG && char.IsLetter(Code[0]) && Code.Length == 7)
			{
				this.Code = DTCItemV2.GetVAGCode(RawCode);
			}
			if (this.IsBMW && (ItemType == DTCItemV2.DTCItemType.KWP2000 || ItemType == DTCItemV2.DTCItemType.UDS))
			{
				this.Code = RawCode;
			}
			this.ECU = ECU;
			if (string.IsNullOrEmpty(ECU))
			{
				if (ResponseHeader != null && ResponseHeader.Length == 3)
				{
					this.ECU = CAN11bitHelper.GetECUName(ResponseHeader, (!string.IsNullOrEmpty(this.SelectedBrand)) ? this.SelectedBrand : SharedSettings.Current.SelectedBrand, ExtendedAddress);
					return;
				}
				this.ECU = ResponseHeader;
			}
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x0022FC98 File Offset: 0x0022DE98
		public void LoadDescription()
		{
			string text = this.Code;
			if (this.IsVAG)
			{
				while (text.Length < 5)
				{
					text = "0" + text;
				}
			}
			List<BrandAndDescription> descriptionsForCode = DescriptionLoader.GetDescriptionsForCode(text, this.SelectedBrand);
			this.Descriptions.Clear();
			foreach (BrandAndDescription brandAndDescription in descriptionsForCode)
			{
				brandAndDescription.Description = brandAndDescription.Description.Replace("\\n", "\n").Replace("\\r", "\r");
				this.Descriptions.Add(brandAndDescription);
			}
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs("Descriptions"));
			}
			PropertyChangedEventHandler propertyChanged2 = this.PropertyChanged;
			if (propertyChanged2 == null)
			{
				return;
			}
			propertyChanged2(this, new PropertyChangedEventArgs("DescriptionsVisible"));
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06003290 RID: 12944 RVA: 0x0022FD88 File Offset: 0x0022DF88
		// (remove) Token: 0x06003291 RID: 12945 RVA: 0x0022FDC0 File Offset: 0x0022DFC0
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

		// Token: 0x06003292 RID: 12946 RVA: 0x0022FDF8 File Offset: 0x0022DFF8
		public static string DecodeRawCode(byte[] data)
		{
			if (data.Length < 2)
			{
				return "";
			}
			string text = string.Empty;
			bool bit_1_ = BitHelpers.GetBit_1_8(data[0], 8);
			bool bit_1_2 = BitHelpers.GetBit_1_8(data[0], 7);
			if (!bit_1_ && !bit_1_2)
			{
				text = "P";
			}
			if (!bit_1_ && bit_1_2)
			{
				text = "C";
			}
			if (bit_1_ && !bit_1_2)
			{
				text = "B";
			}
			if (bit_1_ && bit_1_2)
			{
				text = "U";
			}
			bool bit_1_3 = BitHelpers.GetBit_1_8(data[0], 6);
			bool bit_1_4 = BitHelpers.GetBit_1_8(data[0], 5);
			if (!bit_1_3 && !bit_1_4)
			{
				text += "0";
			}
			if (!bit_1_3 && bit_1_4)
			{
				text += "1";
			}
			if (bit_1_3 && !bit_1_4)
			{
				text += "2";
			}
			if (bit_1_3 && bit_1_4)
			{
				text += "3";
			}
			int num = (int)(data[0] & 15);
			int num2 = (data[1] & 240) >> 4;
			int num3 = (int)(data[1] & 15);
			if (data.Length == 2)
			{
				text = text + num.ToString("X1", CultureInfo.InvariantCulture) + num2.ToString("X1", CultureInfo.InvariantCulture) + num3.ToString("X1", CultureInfo.InvariantCulture);
			}
			else
			{
				int num4 = (data[2] & 240) >> 4;
				int num5 = (int)(data[2] & 15);
				text = string.Concat(new string[]
				{
					text,
					num.ToString("X1", CultureInfo.InvariantCulture),
					num2.ToString("X1", CultureInfo.InvariantCulture),
					num3.ToString("X1", CultureInfo.InvariantCulture),
					num4.ToString("X1", CultureInfo.InvariantCulture),
					num5.ToString("X1", CultureInfo.InvariantCulture)
				});
			}
			return text;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x0022FFA0 File Offset: 0x0022E1A0
		public override string ToString()
		{
			return this.Code;
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x0022FFA8 File Offset: 0x0022E1A8
		public override bool Equals(object obj)
		{
			DTCItemV2 dtcitemV = obj as DTCItemV2;
			return dtcitemV != null && this.Code == dtcitemV.Code && this.RawCode == dtcitemV.RawCode && this.ItemType == dtcitemV.ItemType && this.ResponseHeader == dtcitemV.ResponseHeader && this.Statuses.SequenceEqual(dtcitemV.Statuses);
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x0023001C File Offset: 0x0022E21C
		public override int GetHashCode()
		{
			return (((((((((-875794960 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Code)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.RawCode)) * -1521134295 + this.ItemType.GetHashCode()) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.RequestHeader)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.ResponseHeader)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.RequestCommand)) * -1521134295 + EqualityComparer<List<DTCStatusHelper.DTCStatus>>.Default.GetHashCode(this.Statuses)) * -1521134295 + EqualityComparer<List<byte>>.Default.GetHashCode(this.PayloadData)) * -1521134295 + this.OnlyTestNotComplitedDTC.GetHashCode()) * -1521134295 + this.IsArchive.GetHashCode();
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00230114 File Offset: 0x0022E314
		public OBDRequest[] GetRequestForClear()
		{
			if (App.OBDSimulator.IsActive)
			{
				return new OBDRequest[0];
			}
			if (App.OBDReader.IsNissanConsult2Protocol && App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
			{
				return new OBDRequest[]
				{
					new OBDRequest("14", false),
					new OBDRequest("14FF00", false),
					new OBDRequest("14FFFF", false),
					new OBDRequest("14FFFFFF", false),
					new OBDRequest("140000", false),
					new OBDRequest("1400FF00", false)
				};
			}
			if (this.IsBMW && App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && !string.IsNullOrEmpty(this.RequestHeader) && (this.ItemType == DTCItemV2.DTCItemType.UDS || this.ItemType == DTCItemV2.DTCItemType.KWP2000))
			{
				string[] array = new string[] { "14FFFFFF" };
				if (this.ItemType == DTCItemV2.DTCItemType.UDS)
				{
					array = new string[] { "14FFFFFF" };
				}
				else if (this.ItemType == DTCItemV2.DTCItemType.KWP2000)
				{
					array = new string[] { "14", "14FF00", "14FFFF", "140000" };
				}
				string ecu = this.ResponseHeader.Substring(1);
				return array.Select((string cmd) => new OBDRequest(cmd, "6F1", string.Concat(new string[] { "ATPBC101;ATSPB;ATCEA", ecu, ";ATFCSD", ecu, "300010;ATSH6F1;ATFCSH6F1;ATFCSM1;ATCRA6", ecu }), "ATD;ATSP6;ATD0;ATE0;ATH1;ATS0;ATSTDEF", false)
				{
					DoNotDecode = true
				}).ToArray<OBDRequest>();
			}
			if (this.ItemType == DTCItemV2.DTCItemType.OBDII)
			{
				return new OBDRequest[]
				{
					new OBDRequest("04", false),
					new OBDRequest("04", false),
					new OBDRequest("04", false),
					new OBDRequest("04", false)
				};
			}
			if (this.ItemType == DTCItemV2.DTCItemType.UDS)
			{
				return new OBDRequest[]
				{
					new OBDRequest("14FFFFFF", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
					new OBDRequest("04", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false)
				};
			}
			if (this.ItemType == DTCItemV2.DTCItemType.KWP2000)
			{
				return new OBDRequest[]
				{
					new OBDRequest("14", this.RequestHeader, "", "", false),
					new OBDRequest("14FFFF", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
					new OBDRequest("14FF00", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
					new OBDRequest("140000", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
					new OBDRequest("04", this.RequestHeader, "", "", false)
				};
			}
			return new OBDRequest[]
			{
				new OBDRequest("04", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
				new OBDRequest("14FFFFFF", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
				new OBDRequest("14", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
				new OBDRequest("14FFFF", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
				new OBDRequest("14FF00", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
				new OBDRequest("140000", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false),
				new OBDRequest("04", this.RequestHeader, this.Request.BeforeCommands, this.Request.AfterCommands, false)
			};
		}

		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x00230515 File Offset: 0x0022E715
		// (set) Token: 0x06003298 RID: 12952 RVA: 0x0023051D File Offset: 0x0022E71D
		[JsonIgnore]
		public OBDRequest Request
		{
			[CompilerGenerated]
			get
			{
				return this.<Request>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Request>k__BackingField = value;
			}
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x00230526 File Offset: 0x0022E726
		// Note: this type is marked as 'beforefieldinit'.
		static DTCItemV2()
		{
		}

		// Token: 0x04001D3F RID: 7487
		[CompilerGenerated]
		private string <Code>k__BackingField;

		// Token: 0x04001D40 RID: 7488
		[CompilerGenerated]
		private string <RawCode>k__BackingField;

		// Token: 0x04001D41 RID: 7489
		[CompilerGenerated]
		private DTCItemV2.DTCItemType <ItemType>k__BackingField;

		// Token: 0x04001D42 RID: 7490
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x04001D43 RID: 7491
		[CompilerGenerated]
		private string <ResponseHeader>k__BackingField;

		// Token: 0x04001D44 RID: 7492
		[CompilerGenerated]
		private string <RequestCommand>k__BackingField;

		// Token: 0x04001D45 RID: 7493
		private string _Payload;

		// Token: 0x04001D46 RID: 7494
		[CompilerGenerated]
		private string <SelectedBrand>k__BackingField;

		// Token: 0x04001D47 RID: 7495
		[CompilerGenerated]
		private ObservableCollection<BrandAndDescription> <Descriptions>k__BackingField;

		// Token: 0x04001D48 RID: 7496
		[CompilerGenerated]
		private List<DTCStatusHelper.DTCStatus> <Statuses>k__BackingField;

		// Token: 0x04001D49 RID: 7497
		[CompilerGenerated]
		private List<byte> <PayloadData>k__BackingField;

		// Token: 0x04001D4A RID: 7498
		[CompilerGenerated]
		private bool <OnlyTestNotComplitedDTC>k__BackingField;

		// Token: 0x04001D4B RID: 7499
		[CompilerGenerated]
		private bool <IsArchive>k__BackingField;

		// Token: 0x04001D4C RID: 7500
		[CompilerGenerated]
		private string <ECU>k__BackingField;

		// Token: 0x04001D4D RID: 7501
		[CompilerGenerated]
		private string <ExtendedAddress>k__BackingField;

		// Token: 0x04001D4E RID: 7502
		private static DTCStatusHelper.DTCStatus[] TestNotComplitedStatuses = new DTCStatusHelper.DTCStatus[]
		{
			DTCStatusHelper.DTCStatus.uds_bit4_testNotCompletedSinceLastClear,
			DTCStatusHelper.DTCStatus.uds_bit6_testNotCompletedThisOperationCycle,
			DTCStatusHelper.DTCStatus.kwp2000_bit2_testRunning,
			DTCStatusHelper.DTCStatus.kwp2000_bit4_testReadiness,
			DTCStatusHelper.DTCStatus.GM_bit2_testNotPassedSinceDTCCleared,
			DTCStatusHelper.DTCStatus.GM_bit5_testNotPassedSinceCurrentPowerUp
		};

		// Token: 0x04001D4F RID: 7503
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04001D50 RID: 7504
		[CompilerGenerated]
		private OBDRequest <Request>k__BackingField;

		// Token: 0x02000545 RID: 1349
		public enum DTCItemType
		{
			// Token: 0x04001D52 RID: 7506
			OBDII,
			// Token: 0x04001D53 RID: 7507
			KWP2000,
			// Token: 0x04001D54 RID: 7508
			UDS,
			// Token: 0x04001D55 RID: 7509
			Unknown,
			// Token: 0x04001D56 RID: 7510
			GMA9,
			// Token: 0x04001D57 RID: 7511
			GM1201
		}

		// Token: 0x02000546 RID: 1350
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600329A RID: 12954 RVA: 0x0023053E File Offset: 0x0022E73E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600329B RID: 12955 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600329C RID: 12956 RVA: 0x0023054A File Offset: 0x0022E74A
			internal bool <.ctor>b__73_0(DTCStatusHelper.DTCStatus x)
			{
				return x == DTCStatusHelper.DTCStatus.obd_0A_permanent;
			}

			// Token: 0x04001D58 RID: 7512
			public static readonly DTCItemV2.<>c <>9 = new DTCItemV2.<>c();

			// Token: 0x04001D59 RID: 7513
			public static Func<DTCStatusHelper.DTCStatus, bool> <>9__73_0;
		}

		// Token: 0x02000547 RID: 1351
		[CompilerGenerated]
		private sealed class <>c__DisplayClass83_0
		{
			// Token: 0x0600329D RID: 12957 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass83_0()
			{
			}

			// Token: 0x0600329E RID: 12958 RVA: 0x00230550 File Offset: 0x0022E750
			internal OBDRequest <GetRequestForClear>b__0(string cmd)
			{
				return new OBDRequest(cmd, "6F1", string.Concat(new string[] { "ATPBC101;ATSPB;ATCEA", this.ecu, ";ATFCSD", this.ecu, "300010;ATSH6F1;ATFCSH6F1;ATFCSM1;ATCRA6", this.ecu }), "ATD;ATSP6;ATD0;ATE0;ATH1;ATS0;ATSTDEF", false)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x04001D5A RID: 7514
			public string ecu;
		}
	}
}
