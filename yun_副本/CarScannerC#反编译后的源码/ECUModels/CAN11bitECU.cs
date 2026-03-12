using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004B5 RID: 1205
	internal class CAN11bitECU : ObservableCollection<DTCItemV2>, IECU, IEnumerable<DTCItemV2>, IEnumerable, INotifyPropertyChanged
	{
		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x00215431 File Offset: 0x00213631
		public static string ECU_ENGINE_NAME
		{
			get
			{
				return Translate.GetString("ecu_Engine");
			}
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x0021543D File Offset: 0x0021363D
		public static string ECU_TRANSMISSION_NAME
		{
			get
			{
				return Translate.GetString("ecu_Transmission");
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x00215449 File Offset: 0x00213649
		public static string ECU_ABS_NAME
		{
			get
			{
				return Translate.GetString("ecu_Abs");
			}
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06002FE2 RID: 12258 RVA: 0x00215458 File Offset: 0x00213658
		// (remove) Token: 0x06002FE3 RID: 12259 RVA: 0x00215490 File Offset: 0x00213690
		public new event PropertyChangedEventHandler PropertyChanged
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

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public virtual ELMFormat ELMFormat
		{
			get
			{
				return ELMFormat.CAN11bit;
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x002154C5 File Offset: 0x002136C5
		// (set) Token: 0x06002FE6 RID: 12262 RVA: 0x002154CD File Offset: 0x002136CD
		public virtual int Protocol
		{
			[CompilerGenerated]
			get
			{
				return this.<Protocol>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Protocol>k__BackingField = value;
			}
		}

		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x002154D6 File Offset: 0x002136D6
		// (set) Token: 0x06002FE8 RID: 12264 RVA: 0x002154DE File Offset: 0x002136DE
		public virtual string Name
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
		} = "";

		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x002154E7 File Offset: 0x002136E7
		public virtual string ShortName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06002FEA RID: 12266 RVA: 0x002154EF File Offset: 0x002136EF
		// (set) Token: 0x06002FEB RID: 12267 RVA: 0x002154F7 File Offset: 0x002136F7
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
		} = "";

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x00215500 File Offset: 0x00213700
		// (set) Token: 0x06002FED RID: 12269 RVA: 0x00215508 File Offset: 0x00213708
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
		} = "";

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x00215511 File Offset: 0x00213711
		// (set) Token: 0x06002FEF RID: 12271 RVA: 0x00215519 File Offset: 0x00213719
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
		} = "";

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06002FF0 RID: 12272 RVA: 0x00215522 File Offset: 0x00213722
		// (set) Token: 0x06002FF1 RID: 12273 RVA: 0x0021552A File Offset: 0x0021372A
		public string TesterAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<TesterAddress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TesterAddress>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x00215533 File Offset: 0x00213733
		// (set) Token: 0x06002FF3 RID: 12275 RVA: 0x0021553B File Offset: 0x0021373B
		public List<string> ReadDTCCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<ReadDTCCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ReadDTCCommands>k__BackingField = value;
			}
		} = CAN11bitECU.DEFAULT_DTC_READ_COMMANDS.ToList<string>();

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x00215544 File Offset: 0x00213744
		// (set) Token: 0x06002FF5 RID: 12277 RVA: 0x0021554C File Offset: 0x0021374C
		public List<string> ClearDTCCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<ClearDTCCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ClearDTCCommands>k__BackingField = value;
			}
		} = CAN11bitECU.DEFAULT_DTC_CLEAR_COMMANDS.ToList<string>();

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x00215555 File Offset: 0x00213755
		// (set) Token: 0x06002FF7 RID: 12279 RVA: 0x0021555D File Offset: 0x0021375D
		public List<string> OpenSessionCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<OpenSessionCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OpenSessionCommands>k__BackingField = value;
			}
		} = new List<string>(0);

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x00215566 File Offset: 0x00213766
		// (set) Token: 0x06002FF9 RID: 12281 RVA: 0x0021556E File Offset: 0x0021376E
		public List<string> CloseSessionCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<CloseSessionCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CloseSessionCommands>k__BackingField = value;
			}
		} = new List<string>(0);

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x00215577 File Offset: 0x00213777
		// (set) Token: 0x06002FFB RID: 12283 RVA: 0x0021557F File Offset: 0x0021377F
		public string AdditionalPreInit
		{
			[CompilerGenerated]
			get
			{
				return this.<AdditionalPreInit>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AdditionalPreInit>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x00215588 File Offset: 0x00213788
		// (set) Token: 0x06002FFD RID: 12285 RVA: 0x00215590 File Offset: 0x00213790
		public string AdditionalPostInit
		{
			[CompilerGenerated]
			get
			{
				return this.<AdditionalPostInit>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AdditionalPostInit>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x00215599 File Offset: 0x00213799
		// (set) Token: 0x06002FFF RID: 12287 RVA: 0x002155A1 File Offset: 0x002137A1
		public Dictionary<string, string> IdentsASCII
		{
			[CompilerGenerated]
			get
			{
				return this.<IdentsASCII>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IdentsASCII>k__BackingField = value;
			}
		} = new Dictionary<string, string>();

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x002155AA File Offset: 0x002137AA
		// (set) Token: 0x06003001 RID: 12289 RVA: 0x002155B2 File Offset: 0x002137B2
		public Dictionary<string, string> IdentsHEX
		{
			[CompilerGenerated]
			get
			{
				return this.<IdentsHEX>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IdentsHEX>k__BackingField = value;
			}
		} = new Dictionary<string, string>();

		// Token: 0x06003002 RID: 12290 RVA: 0x002155BC File Offset: 0x002137BC
		public virtual void AddKWP2000Idents()
		{
			this.IdentsASCII.TryAdd("1A90", "VIN");
			this.IdentsASCII.TryAdd("1A91", Translate.GetString("PID_1A91"));
			this.IdentsASCII.TryAdd("1A92", Translate.GetString("PID_1A92"));
			this.IdentsASCII.TryAdd("1A93", Translate.GetString("PID_1A93"));
			this.IdentsASCII.TryAdd("1A94", Translate.GetString("PID_1A94"));
			this.IdentsASCII.TryAdd("1A95", Translate.GetString("PID_1A95"));
			this.IdentsASCII.TryAdd("1A96", Translate.GetString("PID_1A96"));
			this.IdentsASCII.TryAdd("1A97", Translate.GetString("PID_1A97"));
			this.IdentsASCII.TryAdd("1A98", Translate.GetString("PID_1A98"));
			this.IdentsHEX.TryAdd("1A99", Translate.GetString("PID_1A99"));
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x002156D4 File Offset: 0x002138D4
		public virtual void AddUDSIdents()
		{
			this.IdentsASCII.TryAdd("22F180", "Boot software identification");
			this.IdentsASCII.TryAdd("22F181", "Application software identification");
			this.IdentsASCII.TryAdd("22F182", "Application data identification");
			this.IdentsASCII.TryAdd("22F183", "Boot software fingerprint");
			this.IdentsASCII.TryAdd("22F184", "Application software fingerprintr");
			this.IdentsASCII.TryAdd("22F185", "Application data fingerprint");
			this.IdentsASCII.TryAdd("22F187", "Manufacturer spare part number");
			this.IdentsASCII.TryAdd("22F188", "Manufacturer ECU software number");
			this.IdentsASCII.TryAdd("22F189", "Manufacturer ECU software version");
			this.IdentsASCII.TryAdd("22F18A", "System supplier identifier");
			this.IdentsASCII.TryAdd("22F18B", "ECU manufacturing date (ASCII)");
			this.IdentsHEX.TryAdd("22F18B", "ECU manufacturing date (HEX)");
			this.IdentsASCII.TryAdd("22F18C", "ECU serial number");
			this.IdentsASCII.TryAdd("22F18E", "Manufacturer kit assembly part number");
			this.IdentsASCII.TryAdd("22F190", "VIN");
			this.IdentsASCII.TryAdd("22F191", "Manufacturer ECU hardware number");
			this.IdentsASCII.TryAdd("22F192", "System supplier ECU hardware number");
			this.IdentsASCII.TryAdd("22F193", "System supplier ECU hardware version");
			this.IdentsASCII.TryAdd("22F194", "System supplier ECU software number");
			this.IdentsASCII.TryAdd("22F195", "System supplier ECU software version");
			this.IdentsASCII.TryAdd("22F196", "Exhaust regulation or type approval number");
			this.IdentsASCII.TryAdd("22F197", "System name");
			this.IdentsASCII.TryAdd("22F198", "Repair shop code or tester serial number");
			this.IdentsASCII.TryAdd("22F191", "Programming date (ASCII)");
			this.IdentsHEX.TryAdd("22F191", "Programming date (HEX)");
			this.IdentsASCII.TryAdd("22F19A", "Calibration repair shop code or equipment serial number");
			this.IdentsASCII.TryAdd("22F19B", "Calibration date (ASCII)");
			this.IdentsHEX.TryAdd("22F19B", "Calibration date (HEX)");
			this.IdentsASCII.TryAdd("22F19C", "Calibration equipment software number");
			this.IdentsASCII.TryAdd("22F19D", "ECU Installation date (ASCII)");
			this.IdentsHEX.TryAdd("22F19D", "ECU Installation date (HEX)");
			this.IdentsASCII.TryAdd("22F19E", "ODX file id");
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x002159A1 File Offset: 0x00213BA1
		public virtual void Reset()
		{
			base.Clear();
			this.DTCCollection.Clear();
			this.ECUExists = false;
			this.Highlighted = false;
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x002159C4 File Offset: 0x00213BC4
		protected virtual OBDRequest[] GetDTCReadRequests()
		{
			List<OBDRequest> list = this.ReadDTCCommands.Select((string x) => this.GetRequestForCommand(x)).ToList<OBDRequest>();
			OBDRequest[] testECUExistsRequest = this.GetTestECUExistsRequest();
			List<OBDRequest> list2 = new List<OBDRequest>(list.Count + testECUExistsRequest.Length);
			list2.AddRange(testECUExistsRequest);
			list2.AddRange(list);
			foreach (OBDRequest obdrequest in list)
			{
				obdrequest.OBDMode = OBDDataReader.OBDModes.ReadDTC;
				obdrequest.CheckLength = true;
				obdrequest.ResponseReceived += this.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
				obdrequest.ResponseDecoded += this.DtcReadRequest_ResponseDecoded;
			}
			return list2.ToArray();
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x00215A84 File Offset: 0x00213C84
		protected virtual OBDRequest[] GetDTCClearRequests()
		{
			List<OBDRequest> list = this.ClearDTCCommands.Select(delegate(string x)
			{
				OBDRequest requestForCommand = this.GetRequestForCommand(x);
				requestForCommand.OBDMode = OBDDataReader.OBDModes.ClearDTC;
				return requestForCommand;
			}).ToList<OBDRequest>();
			OBDRequest[] testECUExistsRequest = this.GetTestECUExistsRequest();
			List<OBDRequest> list2 = new List<OBDRequest>(list.Count + testECUExistsRequest.Length);
			list2.AddRange(testECUExistsRequest);
			list2.AddRange(list);
			return list2.ToArray();
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x00215AD8 File Offset: 0x00213CD8
		protected virtual void DtcReadRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				this.ECUExists = true;
				List<DTCItemV2> dtcs = DTCDecoder.DecodeData(request, request.Header, data);
				if (this is SubaruCANECU || (this is OBD2Can11bitECU && SharedSettings.Current.SelectedBrand == "Subaru"))
				{
					SubaruCANECU.FixSubaruDTCItems(dtcs, request, this);
				}
				if (this.RemoveOtherRequestsIfUDSReadResponded && !string.IsNullOrEmpty(request.Header) && dtcs.Count > 0 && request.Command.StartsWith("19"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					List<OBDRequest> list = new List<OBDRequest>(queueCopy.Count);
					foreach (OBDRequest obdrequest in queueCopy)
					{
						if (obdrequest.Header == request.Header && obdrequest.Command != null && !obdrequest.Command.StartsWith("19"))
						{
							list.Add(obdrequest);
						}
					}
					foreach (OBDRequest obdrequest2 in list)
					{
						queueCopy.Remove(obdrequest2);
					}
					App.OBDReader.ReplaceQueue(queueCopy);
				}
				if (dtcs != null && dtcs.Count > 0)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						bool flag = false;
						using (List<DTCItemV2>.Enumerator enumerator2 = dtcs.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								DTCItemV2 dtc = enumerator2.Current;
								if (dtc != null && !this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == dtc.Code))
								{
									dtc.LoadDescription();
									if (!(this is OBD2Can11bitECU))
									{
										dtc.ECU = this.Name;
									}
									else
									{
										dtc.ECU = CAN11bitHelper.GetECUName(responseHeader, SharedSettings.Current.SelectedBrand, "");
									}
									this.DTCCollection.Add(dtc);
									DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
									if (recorder != null)
									{
										recorder.Record(dtc);
									}
									flag = true;
								}
							}
						}
						if (flag)
						{
							this.UpdateCollection();
						}
					});
				}
			}
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x00215C94 File Offset: 0x00213E94
		protected virtual string DecodeAsASCII(string command, byte[] data, bool filterPrintable)
		{
			if (data == null)
			{
				return "";
			}
			if (filterPrintable)
			{
				string text = new string((from x in data
					select (char)x into ch
					where (byte)ch >= 32 && (byte)ch <= 126
					select ch).ToArray<char>());
				if (text == null)
				{
					text = "";
				}
				text = text.Trim();
				if (string.IsNullOrEmpty(text))
				{
					text = BitHelpers.ByteArrayToHexString(data);
				}
				return text;
			}
			return Encoding.ASCII.GetString(data).Trim();
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x00215D34 File Offset: 0x00213F34
		protected virtual string DecodeAsHEX(string command, byte[] data)
		{
			if (data == null)
			{
				return "";
			}
			string text;
			try
			{
				text = BitHelpers.ByteArrayToHexString(data);
			}
			catch (Exception)
			{
				text = "";
			}
			return text;
		}

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x00215D70 File Offset: 0x00213F70
		// (set) Token: 0x0600300B RID: 12299 RVA: 0x00215D78 File Offset: 0x00213F78
		public List<string> DefaultTestEcuExistsCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<DefaultTestEcuExistsCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DefaultTestEcuExistsCommands>k__BackingField = value;
			}
		} = new List<string> { "3E", "3E01", "3E00" };

		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x00215D81 File Offset: 0x00213F81
		// (set) Token: 0x0600300D RID: 12301 RVA: 0x00215D89 File Offset: 0x00213F89
		public bool RemoveAllOnCANError
		{
			[CompilerGenerated]
			get
			{
				return this.<RemoveAllOnCANError>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RemoveAllOnCANError>k__BackingField = value;
			}
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x00215D94 File Offset: 0x00213F94
		protected virtual OBDRequest[] GetTestECUExistsRequest()
		{
			List<string> list;
			if (this.OpenSessionCommands.Count > 0)
			{
				list = this.OpenSessionCommands;
			}
			else
			{
				list = this.DefaultTestEcuExistsCommands;
			}
			List<OBDRequest> test_requests = new List<OBDRequest>(list.Count);
			Predicate<OBDRequest> <>9__1;
			Predicate<OBDRequest> <>9__3;
			Predicate<OBDRequest> <>9__4;
			Predicate<OBDRequest> <>9__5;
			ResponseReceivedDelegate <>9__0;
			foreach (string text in list)
			{
				OBDRequest requestForCommand = this.GetRequestForCommand(text);
				requestForCommand.DoNotDecode = true;
				test_requests.Add(requestForCommand);
				OBDRequest obdrequest = requestForCommand;
				ResponseReceivedDelegate responseReceivedDelegate;
				if ((responseReceivedDelegate = <>9__0) == null)
				{
					responseReceivedDelegate = (<>9__0 = delegate(OBDRequest request, string data)
					{
						if (this.ECUExists)
						{
							OBDDataReader obdreader = App.OBDReader;
							Predicate<OBDRequest> predicate;
							if ((predicate = <>9__1) == null)
							{
								predicate = (<>9__1 = (OBDRequest x) => test_requests.Contains(x));
							}
							obdreader.RemoveFromQueue(predicate);
							return;
						}
						if (!string.IsNullOrEmpty(data) && !data.Contains("NO DATA") && data.Contains("CAN ERROR"))
						{
							if (this.RemoveAllOnCANError)
							{
								List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
								queueCopy.RemoveAll((OBDRequest x) => x.ELMFormat == ELMFormat.CAN11bit);
								App.OBDReader.ReplaceQueue(queueCopy);
								return;
							}
							return;
						}
						else
						{
							string text2 = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (!string.IsNullOrEmpty(this.ResponseHeader) && data.Contains(this.ResponseHeader))
							{
								this.ECUExists = true;
								List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
								List<OBDRequest> list2 = queueCopy2;
								Predicate<OBDRequest> predicate2;
								if ((predicate2 = <>9__3) == null)
								{
									predicate2 = (<>9__3 = (OBDRequest x) => test_requests.Contains(x));
								}
								list2.RemoveAll(predicate2);
								App.OBDReader.ReplaceQueue(queueCopy2);
								return;
							}
							if (text2.Contains("7F" + request.Command.Substring(0, 2)))
							{
								this.ECUExists = true;
								List<OBDRequest> queueCopy3 = App.OBDReader.GetQueueCopy();
								List<OBDRequest> list3 = queueCopy3;
								Predicate<OBDRequest> predicate3;
								if ((predicate3 = <>9__4) == null)
								{
									predicate3 = (<>9__4 = (OBDRequest x) => test_requests.Contains(x));
								}
								list3.RemoveAll(predicate3);
								App.OBDReader.ReplaceQueue(queueCopy3);
								return;
							}
							try
							{
								int num = int.Parse(request.Command.Substring(0, 2), NumberStyles.HexNumber);
								string text3 = (num + 64).ToString("X2");
								if (text2.Contains(text3))
								{
									this.ECUExists = true;
									List<OBDRequest> queueCopy4 = App.OBDReader.GetQueueCopy();
									List<OBDRequest> list4 = queueCopy4;
									Predicate<OBDRequest> predicate4;
									if ((predicate4 = <>9__5) == null)
									{
										predicate4 = (<>9__5 = (OBDRequest x) => test_requests.Contains(x));
									}
									list4.RemoveAll(predicate4);
									App.OBDReader.ReplaceQueue(queueCopy4);
									return;
								}
							}
							catch (Exception)
							{
							}
							if (request == test_requests[test_requests.Count - 1] && !this.ECUExists)
							{
								List<OBDRequest> queueCopy5 = App.OBDReader.GetQueueCopy();
								queueCopy5.RemoveAll((OBDRequest x) => x.Header == request.Header);
								App.OBDReader.ReplaceQueue(queueCopy5);
							}
							return;
						}
					});
				}
				obdrequest.ResponseReceived += responseReceivedDelegate;
			}
			return test_requests.ToArray();
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x00215E68 File Offset: 0x00214068
		protected virtual void Last_ResponseReceived(OBDRequest request, string data)
		{
			if (!this.ECUExists)
			{
				List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
				queueCopy.RemoveAll((OBDRequest x) => x.Header == request.Header);
				App.OBDReader.ReplaceQueue(queueCopy);
			}
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x00215EB3 File Offset: 0x002140B3
		public override int GetHashCode()
		{
			return CAN11bitECU.GetDeterministicHashCode(this.RequestHeader) + CAN11bitECU.GetDeterministicHashCode(this.Name) + CAN11bitECU.GetDeterministicHashCode(this.ExtendedAddress);
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x00215ED8 File Offset: 0x002140D8
		protected static int GetDeterministicHashCode(string str)
		{
			int num = 352654597;
			int num2 = num;
			for (int i = 0; i < str.Length; i += 2)
			{
				num = ((num << 5) + num) ^ (int)str[i];
				if (i == str.Length - 1)
				{
					break;
				}
				num2 = ((num2 << 5) + num2) ^ (int)str[i + 1];
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x00215F30 File Offset: 0x00214130
		// (set) Token: 0x06003013 RID: 12307 RVA: 0x00215F38 File Offset: 0x00214138
		public virtual bool ECUExists
		{
			get
			{
				return this._ECUExists;
			}
			protected set
			{
				this._ECUExists = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("ECUExists"));
			}
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x00215F5C File Offset: 0x0021415C
		public virtual OBDRequest GetRequestForCommand(string cmd)
		{
			string text = "";
			if (string.IsNullOrEmpty(this.ResponseHeader) || this.RequestHeader == "7DF")
			{
				if (SharedSettings.Current.UseDefaultInit || !SharedSettings.Current.CustomInitString.Contains("ATCRA"))
				{
					text = "ATAR;";
				}
			}
			else
			{
				text = "ATCRA" + this.ResponseHeader + ";";
			}
			string text2;
			if (string.IsNullOrEmpty(this.RequestHeader) || this.RequestHeader == "7DF")
			{
				text2 = "ATFCSM0;";
			}
			else if (string.IsNullOrEmpty(this.ExtendedAddress) || this is VAGECU)
			{
				text2 = "ATFCSH" + this.RequestHeader + ";ATFCSD300005;ATFCSM1;";
			}
			else
			{
				text2 = string.Concat(new string[] { "ATFCSH", this.RequestHeader, ";ATFCSD", this.ExtendedAddress, "300005;ATFCSM1;" });
			}
			string text3 = "";
			if (!string.IsNullOrEmpty(this.ExtendedAddress) && !(this is VAGECU))
			{
				text3 = "ATCEA" + this.ExtendedAddress + ";";
			}
			string text4 = "";
			if (!string.IsNullOrEmpty(this.TesterAddress))
			{
				text4 = "ATTA" + this.TesterAddress + ";";
			}
			string text5 = "";
			if (!string.IsNullOrEmpty(this.RequestHeader))
			{
				text5 = "ATSP" + this.Protocol.ToString("X1") + ";";
			}
			string text6 = string.Concat(new string[] { text5, this.AdditionalPreInit, ";", text2, text, text3, text4, ";" });
			foreach (string text7 in this.OpenSessionCommands)
			{
				text6 = text6 + text7 + ";";
			}
			string text8 = this.AdditionalPostInit + ";";
			foreach (string text9 in this.CloseSessionCommands)
			{
				text8 = text8 + text9 + ";";
			}
			text8 += "ATAR;ATFCSM0;ATCEA;ATSTDEF";
			OBDRequest obdrequest = new OBDRequest(cmd, this.RequestHeader, text6, text8, false)
			{
				ELMFormat = ELMFormat.CAN11bit,
				MaxLines = 1000
			};
			if (this.CycleThroughOpenSessionCommands)
			{
				if (this.OpenSessionCommands.Count == 0)
				{
					return obdrequest;
				}
				if (this.currentOpenSession == "")
				{
					this.currentOpenSession = this.OpenSessionCommands[0];
				}
				List<string> list = obdrequest.BeforeCommands.ToList<string>();
				list.RemoveAll((string x) => this.OpenSessionCommands.Contains(x) && x != this.currentOpenSession);
				obdrequest.BeforeCommands = list.ToArray();
				if (this.OpenSessionCommands.Count > 1)
				{
					obdrequest.ResponseReceived += this.Request_ResponseReceived;
				}
			}
			return obdrequest;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x002162A4 File Offset: 0x002144A4
		private void Request_ResponseReceived(OBDRequest request, string data)
		{
			if (request.DoNotDecode)
			{
				return;
			}
			string negativeResponseCode = CAN11bitHelper.GetNegativeResponseCode(request, data);
			if (negativeResponseCode == "7F" || negativeResponseCode == "7f")
			{
				int num = 0;
				int num2 = -1;
				for (int i = 0; i < request.BeforeCommands.Length; i++)
				{
					string text = request.BeforeCommands[i];
					if (this.OpenSessionCommands.IndexOf(text) >= 0)
					{
						num = this.OpenSessionCommands.IndexOf(text);
						num2 = i;
						break;
					}
				}
				string text2 = this.OpenSessionCommands[num];
				num++;
				if (num > 0 && num < this.OpenSessionCommands.Count)
				{
					string text3 = this.OpenSessionCommands[num];
					this.currentOpenSession = text3;
					request.BeforeCommands[num2] = text3;
					App.OBDReader.InsertRequestInQueue(request);
					foreach (OBDRequest obdrequest in App.OBDReader.GetQueueCopy())
					{
						for (int j = 0; j < obdrequest.BeforeCommands.Length; j++)
						{
							if (obdrequest.BeforeCommands[j] == text2)
							{
								obdrequest.BeforeCommands[j] = text3;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x002163F8 File Offset: 0x002145F8
		public void Request_ResponseReceivedCheckForNegativeResponseAndForCancellation(OBDRequest request, string data)
		{
			if (this.cancellationToken.IsCancellationRequested)
			{
				App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				return;
			}
			if (data == null)
			{
				return;
			}
			string text = OBDDataReader.FilterHexAndNewLineOnly(data);
			string service = request.Command.Substring(0, 2);
			string text2 = "037F" + service + "11";
			string nr78_string = "037F" + service + "78";
			if (text.Contains(text2))
			{
				App.OBDReader.GetQueueCopy().RemoveAll((OBDRequest x) => x.Header == request.Header && x.Command.StartsWith(service));
				return;
			}
			if (text.IndexOf(nr78_string) >= 0)
			{
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
				{
					string[] array = new string[request.BeforeCommands.Length + 2];
					Array.Copy(request.BeforeCommands, array, request.BeforeCommands.Length);
					array[array.Length - 2] = "ATAT0";
					array[array.Length - 1] = "ATSTFF";
					string[] array2 = new string[request.AfterCommands.Length + 2];
					Array.Copy(request.AfterCommands, array2, request.AfterCommands.Length);
					array2[array2.Length - 2] = "ATAT" + SharedSettings.Current.AdaptiveTimings.ToString();
					string text3 = SharedSettings.Current.GetATST();
					if (string.IsNullOrEmpty(text3))
					{
						text3 = "32";
					}
					array2[array2.Length - 1] = "ATST" + text3;
					request.BeforeCommands = array;
					request.AfterCommands = array2;
					request.ResponseReceived -= this.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
					request.ResponseReceived += delegate(OBDRequest request2, string response)
					{
						if (response == null || request2.NR78RepeatCounter > 3)
						{
							return;
						}
						bool flag = false;
						bool flag2 = false;
						if (response != null && response.Contains("NO DATA"))
						{
							flag = true;
						}
						if (!flag)
						{
							string text4 = OBDDataReader.FilterHexAndNewLineOnly(response);
							string[] array3 = text4.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
							if (text4.Contains(nr78_string) && array3.Length == 1)
							{
								flag2 = true;
							}
						}
						if (flag || flag2)
						{
							int nr78RepeatCounter = request2.NR78RepeatCounter;
							request2.NR78RepeatCounter = nr78RepeatCounter + 1;
							App.OBDReader.InsertRequestInQueue(request2);
						}
					};
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.Insert(0, request);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}
		}

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x00216627 File Offset: 0x00214827
		// (set) Token: 0x06003018 RID: 12312 RVA: 0x00216630 File Offset: 0x00214830
		public bool Expanded
		{
			get
			{
				return this._Expanded;
			}
			set
			{
				if (this._Expanded != value)
				{
					this._Expanded = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged(this, new PropertyChangedEventArgs("Expanded"));
					}
					PropertyChangedEventHandler propertyChanged2 = this.PropertyChanged;
					if (propertyChanged2 != null)
					{
						propertyChanged2(this, new PropertyChangedEventArgs("ExpandedStateImage"));
					}
					this.UpdateCollection();
				}
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x0021668B File Offset: 0x0021488B
		public string ExpandedStateImage
		{
			get
			{
				if (this.Expanded)
				{
					return "icons8_chevron_up_white.png";
				}
				return "icons8_chevron_down_white.png";
			}
		}

		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x002166A0 File Offset: 0x002148A0
		public List<DTCItemV2> DTCCollection
		{
			[CompilerGenerated]
			get
			{
				return this.<DTCCollection>k__BackingField;
			}
		} = new List<DTCItemV2>();

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x002166A8 File Offset: 0x002148A8
		// (set) Token: 0x0600301C RID: 12316 RVA: 0x002166B0 File Offset: 0x002148B0
		public bool IsSelected
		{
			get
			{
				return this._IsSelected;
			}
			set
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

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x002166D4 File Offset: 0x002148D4
		// (set) Token: 0x0600301E RID: 12318 RVA: 0x002166DC File Offset: 0x002148DC
		public bool RemoveOtherRequestsIfUDSReadResponded
		{
			[CompilerGenerated]
			get
			{
				return this.<RemoveOtherRequestsIfUDSReadResponded>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RemoveOtherRequestsIfUDSReadResponded>k__BackingField = value;
			}
		} = true;

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x002166E5 File Offset: 0x002148E5
		// (set) Token: 0x06003020 RID: 12320 RVA: 0x002166ED File Offset: 0x002148ED
		public virtual bool TestELMDevice
		{
			[CompilerGenerated]
			get
			{
				return this.<TestELMDevice>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TestELMDevice>k__BackingField = value;
			}
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x002166F8 File Offset: 0x002148F8
		public virtual async Task ReadDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken token)
		{
			this.cancellationToken = token;
			OBDRequest[] array = this.GetDTCReadRequests();
			if (this.TestELMDevice)
			{
				OBDRequest atshtestRequest = this.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
				OBDRequest atfctestRequest = this.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
				array = new OBDRequest[] { atshtestRequest, atfctestRequest }.Concat(array).ToArray<OBDRequest>();
			}
			App.OBDReader.ReplaceQueue(array);
			await App.OBDReader.WaitForCommandQueue();
			if (!SharedSettings.Current.UseDefaultInit)
			{
				OBDRequest pingRequest = App.OBDReader.GetPingRequest();
				App.OBDReader.ReplaceQueue(pingRequest);
				await App.OBDReader.WaitForCommandQueue();
			}
			this.cancellationToken = CancellationToken.None;
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x00216754 File Offset: 0x00214954
		public virtual async Task ClearDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken token)
		{
			this.cancellationToken = token;
			OBDRequest[] array = this.GetDTCClearRequests();
			bool succcess = false;
			OBDRequest[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				OBDRequest request = array2[i];
				request.ResponseReceived += delegate(OBDRequest req, string response)
				{
					string negativeResponseCode = CAN11bitHelper.GetNegativeResponseCode(req, response);
					if ((negativeResponseCode == "0" && response != null && response.Contains(request.ResponseMarker)) || negativeResponseCode == "78")
					{
						succcess = true;
					}
				};
			}
			if (this.TestELMDevice)
			{
				OBDRequest atshtestRequest = this.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
				OBDRequest atfctestRequest = this.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
				array = new OBDRequest[] { atshtestRequest, atfctestRequest }.Concat(array).ToArray<OBDRequest>();
			}
			App.OBDReader.ReplaceQueue(array);
			await App.OBDReader.WaitForCommandQueue();
			if (!SharedSettings.Current.UseDefaultInit)
			{
				OBDRequest pingRequest = App.OBDReader.GetPingRequest();
				App.OBDReader.ReplaceQueue(pingRequest);
				await App.OBDReader.WaitForCommandQueue();
			}
			this.cancellationToken = CancellationToken.None;
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x002167B0 File Offset: 0x002149B0
		public virtual async Task<string> GetECUInformationReportAsync(CancellationToken token)
		{
			this.cancellationToken = token;
			StringBuilder sb = new StringBuilder(8);
			List<OBDRequest> list = new List<OBDRequest>();
			list.AddRange(this.GetTestECUExistsRequest());
			foreach (KeyValuePair<string, string> keyValuePair in this.IdentsASCII)
			{
				string key = keyValuePair.Key;
				string requestTitle2 = keyValuePair.Value;
				OBDRequest requestForCommand = this.GetRequestForCommand(key);
				requestForCommand.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
				{
					string text = this.DecodeAsASCII(decodedRequest.Command, data, true);
					if (!string.IsNullOrEmpty(text))
					{
						sb.AppendLine(requestTitle2 + ": " + text);
					}
					this.ECUExists = true;
				};
				list.Add(requestForCommand);
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in this.IdentsHEX)
			{
				string key2 = keyValuePair2.Key;
				string requestTitle = keyValuePair2.Value;
				OBDRequest requestForCommand2 = this.GetRequestForCommand(key2);
				requestForCommand2.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
				{
					string text2 = this.DecodeAsHEX(decodedRequest.Command, data);
					if (!string.IsNullOrEmpty(text2))
					{
						sb.AppendLine(requestTitle + ": " + text2);
					}
					this.ECUExists = true;
				};
				list.Add(requestForCommand2);
			}
			foreach (OBDRequest obdrequest in list)
			{
				obdrequest.CheckLength = true;
				obdrequest.ELMFormat = this.ELMFormat;
			}
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			sb.ToString();
			return sb.ToString();
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x002167FC File Offset: 0x002149FC
		public void UpdateCollection()
		{
			if (this.Expanded)
			{
				List<DTCItemV2> temp_list = new List<DTCItemV2>(this.DTCCollection);
				if (SharedSettings.Current.HideArchiveDTC)
				{
					temp_list = temp_list.Where((DTCItemV2 x) => !x.IsArchive).ToList<DTCItemV2>();
				}
				if (SharedSettings.Current.HideDTCWithUncomplitedTests)
				{
					temp_list = temp_list.Where((DTCItemV2 x) => !x.OnlyTestNotComplitedDTC).ToList<DTCItemV2>();
				}
				if (MainThread.IsMainThread)
				{
					base.Clear();
					using (List<DTCItemV2>.Enumerator enumerator = temp_list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DTCItemV2 dtcitemV = enumerator.Current;
							try
							{
								base.Add(dtcitemV);
							}
							catch (Exception)
							{
							}
						}
						return;
					}
				}
				MainThread.BeginInvokeOnMainThread(delegate
				{
					this.Clear();
					foreach (DTCItemV2 dtcitemV2 in temp_list)
					{
						try
						{
							this.Add(dtcitemV2);
						}
						catch (Exception)
						{
						}
					}
				});
				return;
			}
			if (MainThread.IsMainThread)
			{
				base.Clear();
				return;
			}
			MainThread.BeginInvokeOnMainThread(delegate
			{
				base.Clear();
			});
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x00216948 File Offset: 0x00214B48
		protected virtual OBDRequest GetATFCTestRequest(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback)
		{
			OBDRequest obdrequest = new OBDRequest("ATFCSH" + this.RequestHeader, "", new string[] { "ATSP" + this.Protocol.ToString("X1") }, new string[0], false);
			obdrequest.DoNotDecode = true;
			Predicate<OBDRequest> <>9__1;
			Action <>9__2;
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					OBDDataReader obdreader = App.OBDReader;
					Predicate<OBDRequest> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = (OBDRequest x) => x.Header == this.RequestHeader);
					}
					obdreader.RemoveFromQueue(predicate);
					foreach (IECU iecu in otherECUs)
					{
						if (((!string.IsNullOrEmpty(iecu.RequestHeader) && iecu.GetRequestForCommand("03").BeforeCommands.Contains("ATFCSH" + iecu.RequestHeader)) || !string.IsNullOrEmpty(iecu.ExtendedAddress)) && !(iecu is OBD2Can11bitECU))
						{
							iecu.IsSelected = false;
							iecu.Reset();
							this.BadELMDetected = true;
						}
					}
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate
						{
							Action badELMDetectedCallback2 = badELMDetectedCallback;
							if (badELMDetectedCallback2 == null)
							{
								return;
							}
							badELMDetectedCallback2();
						});
					}
					Device.BeginInvokeOnMainThread(action);
				}
			};
			return obdrequest;
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x002169D0 File Offset: 0x00214BD0
		protected virtual OBDRequest GetATSHTestRequest(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback)
		{
			OBDRequest obdrequest = new OBDRequest("ATSH" + this.RequestHeader, "", new string[] { "ATSP" + this.Protocol.ToString("X1") }, new string[0], false);
			obdrequest.DoNotDecode = true;
			Predicate<OBDRequest> <>9__1;
			Action <>9__2;
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					OBDDataReader obdreader = App.OBDReader;
					Predicate<OBDRequest> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = (OBDRequest x) => x.Header == this.RequestHeader);
					}
					obdreader.RemoveFromQueue(predicate);
					foreach (IECU iecu in otherECUs)
					{
						if (!string.IsNullOrEmpty(iecu.RequestHeader) || !string.IsNullOrEmpty(iecu.ExtendedAddress))
						{
							iecu.IsSelected = false;
							iecu.Reset();
							this.BadELMDetected = true;
						}
					}
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate
						{
							Action badELMDetectedCallback2 = badELMDetectedCallback;
							if (badELMDetectedCallback2 == null)
							{
								return;
							}
							badELMDetectedCallback2();
						});
					}
					Device.BeginInvokeOnMainThread(action);
				}
			};
			return obdrequest;
		}

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x00216A58 File Offset: 0x00214C58
		// (set) Token: 0x06003028 RID: 12328 RVA: 0x00216A60 File Offset: 0x00214C60
		protected bool BadELMDetected
		{
			[CompilerGenerated]
			get
			{
				return this.<BadELMDetected>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BadELMDetected>k__BackingField = value;
			}
		}

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x00216A69 File Offset: 0x00214C69
		// (set) Token: 0x0600302A RID: 12330 RVA: 0x00216A71 File Offset: 0x00214C71
		public bool Highlighted
		{
			get
			{
				return this._Highlighted;
			}
			set
			{
				if (value != this._Highlighted)
				{
					this._Highlighted = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Highlighted"));
				}
			}
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x00216A93 File Offset: 0x00214C93
		// (set) Token: 0x0600302C RID: 12332 RVA: 0x00216A9B File Offset: 0x00214C9B
		public virtual bool CycleThroughOpenSessionCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<CycleThroughOpenSessionCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CycleThroughOpenSessionCommands>k__BackingField = value;
			}
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x00216AA4 File Offset: 0x00214CA4
		public CAN11bitECU()
		{
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x00216BB4 File Offset: 0x00214DB4
		// Note: this type is marked as 'beforefieldinit'.
		static CAN11bitECU()
		{
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x00216C90 File Offset: 0x00214E90
		[CompilerGenerated]
		private OBDRequest <GetDTCReadRequests>b__74_0(string x)
		{
			return this.GetRequestForCommand(x);
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x00216C99 File Offset: 0x00214E99
		[CompilerGenerated]
		private OBDRequest <GetDTCClearRequests>b__75_0(string x)
		{
			OBDRequest requestForCommand = this.GetRequestForCommand(x);
			requestForCommand.OBDMode = OBDDataReader.OBDModes.ClearDTC;
			return requestForCommand;
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x00216CA9 File Offset: 0x00214EA9
		[CompilerGenerated]
		private bool <GetRequestForCommand>b__95_0(string x)
		{
			return this.OpenSessionCommands.Contains(x) && x != this.currentOpenSession;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x00216CC7 File Offset: 0x00214EC7
		[CompilerGenerated]
		private void <UpdateCollection>b__124_0()
		{
			base.Clear();
		}

		// Token: 0x04001BCA RID: 7114
		private static string[] DEFAULT_DTC_CLEAR_COMMANDS = new string[] { "14", "14FF00", "14FFFFFF", "140000", "14FFFF" };

		// Token: 0x04001BCB RID: 7115
		private static string[] DEFAULT_DTC_READ_COMMANDS = new string[]
		{
			"1800FF00", "1802FF00", "1802FFFF", "1800FFFF", "18FF00", "17FF00", "13FF00", "1902AF", "1902AC", "19028D",
			"190223", "190278", "190208", "190FAC", "190F8D", "190F23", "19D2FF00"
		};

		// Token: 0x04001BCC RID: 7116
		[CompilerGenerated]
		private new PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04001BCD RID: 7117
		[CompilerGenerated]
		private int <Protocol>k__BackingField;

		// Token: 0x04001BCE RID: 7118
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04001BCF RID: 7119
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x04001BD0 RID: 7120
		[CompilerGenerated]
		private string <ResponseHeader>k__BackingField;

		// Token: 0x04001BD1 RID: 7121
		[CompilerGenerated]
		private string <ExtendedAddress>k__BackingField;

		// Token: 0x04001BD2 RID: 7122
		[CompilerGenerated]
		private string <TesterAddress>k__BackingField;

		// Token: 0x04001BD3 RID: 7123
		[CompilerGenerated]
		private List<string> <ReadDTCCommands>k__BackingField;

		// Token: 0x04001BD4 RID: 7124
		[CompilerGenerated]
		private List<string> <ClearDTCCommands>k__BackingField;

		// Token: 0x04001BD5 RID: 7125
		[CompilerGenerated]
		private List<string> <OpenSessionCommands>k__BackingField;

		// Token: 0x04001BD6 RID: 7126
		[CompilerGenerated]
		private List<string> <CloseSessionCommands>k__BackingField;

		// Token: 0x04001BD7 RID: 7127
		[CompilerGenerated]
		private string <AdditionalPreInit>k__BackingField;

		// Token: 0x04001BD8 RID: 7128
		[CompilerGenerated]
		private string <AdditionalPostInit>k__BackingField;

		// Token: 0x04001BD9 RID: 7129
		[CompilerGenerated]
		private Dictionary<string, string> <IdentsASCII>k__BackingField;

		// Token: 0x04001BDA RID: 7130
		[CompilerGenerated]
		private Dictionary<string, string> <IdentsHEX>k__BackingField;

		// Token: 0x04001BDB RID: 7131
		[CompilerGenerated]
		private List<string> <DefaultTestEcuExistsCommands>k__BackingField;

		// Token: 0x04001BDC RID: 7132
		[CompilerGenerated]
		private bool <RemoveAllOnCANError>k__BackingField;

		// Token: 0x04001BDD RID: 7133
		private bool _ECUExists;

		// Token: 0x04001BDE RID: 7134
		protected string currentOpenSession = "";

		// Token: 0x04001BDF RID: 7135
		private bool _Expanded = true;

		// Token: 0x04001BE0 RID: 7136
		[CompilerGenerated]
		private readonly List<DTCItemV2> <DTCCollection>k__BackingField;

		// Token: 0x04001BE1 RID: 7137
		private bool _IsSelected = true;

		// Token: 0x04001BE2 RID: 7138
		[CompilerGenerated]
		private bool <RemoveOtherRequestsIfUDSReadResponded>k__BackingField;

		// Token: 0x04001BE3 RID: 7139
		[CompilerGenerated]
		private bool <TestELMDevice>k__BackingField;

		// Token: 0x04001BE4 RID: 7140
		protected CancellationToken cancellationToken = CancellationToken.None;

		// Token: 0x04001BE5 RID: 7141
		[CompilerGenerated]
		private bool <BadELMDetected>k__BackingField;

		// Token: 0x04001BE6 RID: 7142
		[CompilerGenerated]
		private bool <CycleThroughOpenSessionCommands>k__BackingField;

		// Token: 0x04001BE7 RID: 7143
		private bool _Highlighted;

		// Token: 0x020004B6 RID: 1206
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003033 RID: 12339 RVA: 0x00216CCF File Offset: 0x00214ECF
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003034 RID: 12340 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003035 RID: 12341 RVA: 0x00016849 File Offset: 0x00014A49
			internal char <DecodeAsASCII>b__77_0(byte x)
			{
				return (char)x;
			}

			// Token: 0x06003036 RID: 12342 RVA: 0x00216CDB File Offset: 0x00214EDB
			internal bool <DecodeAsASCII>b__77_1(char ch)
			{
				return (byte)ch >= 32 && (byte)ch <= 126;
			}

			// Token: 0x06003037 RID: 12343 RVA: 0x00216CEE File Offset: 0x00214EEE
			internal bool <GetTestECUExistsRequest>b__87_2(OBDRequest x)
			{
				return x.ELMFormat == ELMFormat.CAN11bit;
			}

			// Token: 0x06003038 RID: 12344 RVA: 0x00216CF9 File Offset: 0x00214EF9
			internal bool <UpdateCollection>b__124_1(DTCItemV2 x)
			{
				return !x.IsArchive;
			}

			// Token: 0x06003039 RID: 12345 RVA: 0x00216D04 File Offset: 0x00214F04
			internal bool <UpdateCollection>b__124_2(DTCItemV2 x)
			{
				return !x.OnlyTestNotComplitedDTC;
			}

			// Token: 0x04001BE8 RID: 7144
			public static readonly CAN11bitECU.<>c <>9 = new CAN11bitECU.<>c();

			// Token: 0x04001BE9 RID: 7145
			public static Func<byte, char> <>9__77_0;

			// Token: 0x04001BEA RID: 7146
			public static Func<char, bool> <>9__77_1;

			// Token: 0x04001BEB RID: 7147
			public static Predicate<OBDRequest> <>9__87_2;

			// Token: 0x04001BEC RID: 7148
			public static Func<DTCItemV2, bool> <>9__124_1;

			// Token: 0x04001BED RID: 7149
			public static Func<DTCItemV2, bool> <>9__124_2;
		}

		// Token: 0x020004B7 RID: 1207
		[CompilerGenerated]
		private sealed class <>c__DisplayClass122_0
		{
			// Token: 0x0600303A RID: 12346 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass122_0()
			{
			}

			// Token: 0x04001BEE RID: 7150
			public bool succcess;
		}

		// Token: 0x020004B8 RID: 1208
		[CompilerGenerated]
		private sealed class <>c__DisplayClass122_1
		{
			// Token: 0x0600303B RID: 12347 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass122_1()
			{
			}

			// Token: 0x0600303C RID: 12348 RVA: 0x00216D10 File Offset: 0x00214F10
			internal void <ClearDTCAsync>b__0(OBDRequest req, string response)
			{
				string negativeResponseCode = CAN11bitHelper.GetNegativeResponseCode(req, response);
				if ((negativeResponseCode == "0" && response != null && response.Contains(this.request.ResponseMarker)) || negativeResponseCode == "78")
				{
					this.CS$<>8__locals1.succcess = true;
				}
			}

			// Token: 0x04001BEF RID: 7151
			public OBDRequest request;

			// Token: 0x04001BF0 RID: 7152
			public CAN11bitECU.<>c__DisplayClass122_0 CS$<>8__locals1;
		}

		// Token: 0x020004B9 RID: 1209
		[CompilerGenerated]
		private sealed class <>c__DisplayClass123_0
		{
			// Token: 0x0600303D RID: 12349 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass123_0()
			{
			}

			// Token: 0x04001BF1 RID: 7153
			public CAN11bitECU <>4__this;

			// Token: 0x04001BF2 RID: 7154
			public StringBuilder sb;
		}

		// Token: 0x020004BA RID: 1210
		[CompilerGenerated]
		private sealed class <>c__DisplayClass123_1
		{
			// Token: 0x0600303E RID: 12350 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass123_1()
			{
			}

			// Token: 0x0600303F RID: 12351 RVA: 0x00216D64 File Offset: 0x00214F64
			internal void <GetECUInformationReportAsync>b__0(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
			{
				string text = this.CS$<>8__locals1.<>4__this.DecodeAsASCII(decodedRequest.Command, data, true);
				if (!string.IsNullOrEmpty(text))
				{
					this.CS$<>8__locals1.sb.AppendLine(this.requestTitle + ": " + text);
				}
				this.CS$<>8__locals1.<>4__this.ECUExists = true;
			}

			// Token: 0x04001BF3 RID: 7155
			public string requestTitle;

			// Token: 0x04001BF4 RID: 7156
			public CAN11bitECU.<>c__DisplayClass123_0 CS$<>8__locals1;
		}

		// Token: 0x020004BB RID: 1211
		[CompilerGenerated]
		private sealed class <>c__DisplayClass123_2
		{
			// Token: 0x06003040 RID: 12352 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass123_2()
			{
			}

			// Token: 0x06003041 RID: 12353 RVA: 0x00216DC8 File Offset: 0x00214FC8
			internal void <GetECUInformationReportAsync>b__1(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
			{
				string text = this.CS$<>8__locals2.<>4__this.DecodeAsHEX(decodedRequest.Command, data);
				if (!string.IsNullOrEmpty(text))
				{
					this.CS$<>8__locals2.sb.AppendLine(this.requestTitle + ": " + text);
				}
				this.CS$<>8__locals2.<>4__this.ECUExists = true;
			}

			// Token: 0x04001BF5 RID: 7157
			public string requestTitle;

			// Token: 0x04001BF6 RID: 7158
			public CAN11bitECU.<>c__DisplayClass123_0 CS$<>8__locals2;
		}

		// Token: 0x020004BC RID: 1212
		[CompilerGenerated]
		private sealed class <>c__DisplayClass124_0
		{
			// Token: 0x06003042 RID: 12354 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass124_0()
			{
			}

			// Token: 0x06003043 RID: 12355 RVA: 0x00216E28 File Offset: 0x00215028
			internal void <UpdateCollection>b__3()
			{
				this.<>4__this.Clear();
				foreach (DTCItemV2 dtcitemV in this.temp_list)
				{
					try
					{
						this.<>4__this.Add(dtcitemV);
					}
					catch (Exception)
					{
					}
				}
			}

			// Token: 0x04001BF7 RID: 7159
			public List<DTCItemV2> temp_list;

			// Token: 0x04001BF8 RID: 7160
			public CAN11bitECU <>4__this;
		}

		// Token: 0x020004BD RID: 1213
		[CompilerGenerated]
		private sealed class <>c__DisplayClass125_0
		{
			// Token: 0x06003044 RID: 12356 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass125_0()
			{
			}

			// Token: 0x06003045 RID: 12357 RVA: 0x00216E9C File Offset: 0x0021509C
			internal void <GetATFCTestRequest>b__0(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					OBDDataReader obdreader = App.OBDReader;
					Predicate<OBDRequest> predicate;
					if ((predicate = this.<>9__1) == null)
					{
						predicate = (this.<>9__1 = (OBDRequest x) => x.Header == this.<>4__this.RequestHeader);
					}
					obdreader.RemoveFromQueue(predicate);
					foreach (IECU iecu in this.otherECUs)
					{
						if (((!string.IsNullOrEmpty(iecu.RequestHeader) && iecu.GetRequestForCommand("03").BeforeCommands.Contains("ATFCSH" + iecu.RequestHeader)) || !string.IsNullOrEmpty(iecu.ExtendedAddress)) && !(iecu is OBD2Can11bitECU))
						{
							iecu.IsSelected = false;
							iecu.Reset();
							this.<>4__this.BadELMDetected = true;
						}
					}
					Action action;
					if ((action = this.<>9__2) == null)
					{
						action = (this.<>9__2 = delegate
						{
							Action action2 = this.badELMDetectedCallback;
							if (action2 == null)
							{
								return;
							}
							action2();
						});
					}
					Device.BeginInvokeOnMainThread(action);
				}
			}

			// Token: 0x06003046 RID: 12358 RVA: 0x00216FA4 File Offset: 0x002151A4
			internal bool <GetATFCTestRequest>b__1(OBDRequest x)
			{
				return x.Header == this.<>4__this.RequestHeader;
			}

			// Token: 0x06003047 RID: 12359 RVA: 0x00216FBC File Offset: 0x002151BC
			internal void <GetATFCTestRequest>b__2()
			{
				Action action = this.badELMDetectedCallback;
				if (action == null)
				{
					return;
				}
				action();
			}

			// Token: 0x04001BF9 RID: 7161
			public CAN11bitECU <>4__this;

			// Token: 0x04001BFA RID: 7162
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001BFB RID: 7163
			public Action badELMDetectedCallback;

			// Token: 0x04001BFC RID: 7164
			public Predicate<OBDRequest> <>9__1;

			// Token: 0x04001BFD RID: 7165
			public Action <>9__2;
		}

		// Token: 0x020004BE RID: 1214
		[CompilerGenerated]
		private sealed class <>c__DisplayClass126_0
		{
			// Token: 0x06003048 RID: 12360 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass126_0()
			{
			}

			// Token: 0x06003049 RID: 12361 RVA: 0x00216FD0 File Offset: 0x002151D0
			internal void <GetATSHTestRequest>b__0(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					OBDDataReader obdreader = App.OBDReader;
					Predicate<OBDRequest> predicate;
					if ((predicate = this.<>9__1) == null)
					{
						predicate = (this.<>9__1 = (OBDRequest x) => x.Header == this.<>4__this.RequestHeader);
					}
					obdreader.RemoveFromQueue(predicate);
					foreach (IECU iecu in this.otherECUs)
					{
						if (!string.IsNullOrEmpty(iecu.RequestHeader) || !string.IsNullOrEmpty(iecu.ExtendedAddress))
						{
							iecu.IsSelected = false;
							iecu.Reset();
							this.<>4__this.BadELMDetected = true;
						}
					}
					Action action;
					if ((action = this.<>9__2) == null)
					{
						action = (this.<>9__2 = delegate
						{
							Action action2 = this.badELMDetectedCallback;
							if (action2 == null)
							{
								return;
							}
							action2();
						});
					}
					Device.BeginInvokeOnMainThread(action);
				}
			}

			// Token: 0x0600304A RID: 12362 RVA: 0x002170A8 File Offset: 0x002152A8
			internal bool <GetATSHTestRequest>b__1(OBDRequest x)
			{
				return x.Header == this.<>4__this.RequestHeader;
			}

			// Token: 0x0600304B RID: 12363 RVA: 0x002170C0 File Offset: 0x002152C0
			internal void <GetATSHTestRequest>b__2()
			{
				Action action = this.badELMDetectedCallback;
				if (action == null)
				{
					return;
				}
				action();
			}

			// Token: 0x04001BFE RID: 7166
			public CAN11bitECU <>4__this;

			// Token: 0x04001BFF RID: 7167
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001C00 RID: 7168
			public Action badELMDetectedCallback;

			// Token: 0x04001C01 RID: 7169
			public Predicate<OBDRequest> <>9__1;

			// Token: 0x04001C02 RID: 7170
			public Action <>9__2;
		}

		// Token: 0x020004BF RID: 1215
		[CompilerGenerated]
		private sealed class <>c__DisplayClass76_0
		{
			// Token: 0x0600304C RID: 12364 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass76_0()
			{
			}

			// Token: 0x0600304D RID: 12365 RVA: 0x002170D4 File Offset: 0x002152D4
			internal void <DtcReadRequest_ResponseDecoded>b__0()
			{
				bool flag = false;
				using (List<DTCItemV2>.Enumerator enumerator = this.dtcs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CAN11bitECU.<>c__DisplayClass76_1 CS$<>8__locals1 = new CAN11bitECU.<>c__DisplayClass76_1();
						CS$<>8__locals1.dtc = enumerator.Current;
						if (CS$<>8__locals1.dtc != null && !this.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals1.dtc.Code))
						{
							CS$<>8__locals1.dtc.LoadDescription();
							if (!(this.<>4__this is OBD2Can11bitECU))
							{
								CS$<>8__locals1.dtc.ECU = this.<>4__this.Name;
							}
							else
							{
								CS$<>8__locals1.dtc.ECU = CAN11bitHelper.GetECUName(this.responseHeader, SharedSettings.Current.SelectedBrand, "");
							}
							this.<>4__this.DTCCollection.Add(CS$<>8__locals1.dtc);
							DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
							if (recorder != null)
							{
								recorder.Record(CS$<>8__locals1.dtc);
							}
							flag = true;
						}
					}
				}
				if (flag)
				{
					this.<>4__this.UpdateCollection();
				}
			}

			// Token: 0x04001C03 RID: 7171
			public CAN11bitECU <>4__this;

			// Token: 0x04001C04 RID: 7172
			public string responseHeader;

			// Token: 0x04001C05 RID: 7173
			public List<DTCItemV2> dtcs;
		}

		// Token: 0x020004C0 RID: 1216
		[CompilerGenerated]
		private sealed class <>c__DisplayClass76_1
		{
			// Token: 0x0600304E RID: 12366 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass76_1()
			{
			}

			// Token: 0x0600304F RID: 12367 RVA: 0x00217200 File Offset: 0x00215400
			internal bool <DtcReadRequest_ResponseDecoded>b__1(DTCItemV2 x)
			{
				return x.Code == this.dtc.Code;
			}

			// Token: 0x04001C06 RID: 7174
			public DTCItemV2 dtc;
		}

		// Token: 0x020004C1 RID: 1217
		[CompilerGenerated]
		private sealed class <>c__DisplayClass87_0
		{
			// Token: 0x06003050 RID: 12368 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass87_0()
			{
			}

			// Token: 0x06003051 RID: 12369 RVA: 0x00217218 File Offset: 0x00215418
			internal void <GetTestECUExistsRequest>b__0(OBDRequest request, string data)
			{
				CAN11bitECU.<>c__DisplayClass87_1 CS$<>8__locals1 = new CAN11bitECU.<>c__DisplayClass87_1();
				CS$<>8__locals1.request = request;
				if (this.<>4__this.ECUExists)
				{
					OBDDataReader obdreader = App.OBDReader;
					Predicate<OBDRequest> predicate;
					if ((predicate = this.<>9__1) == null)
					{
						predicate = (this.<>9__1 = (OBDRequest x) => this.test_requests.Contains(x));
					}
					obdreader.RemoveFromQueue(predicate);
					return;
				}
				if (!string.IsNullOrEmpty(data) && !data.Contains("NO DATA") && data.Contains("CAN ERROR"))
				{
					if (this.<>4__this.RemoveAllOnCANError)
					{
						List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
						queueCopy.RemoveAll((OBDRequest x) => x.ELMFormat == ELMFormat.CAN11bit);
						App.OBDReader.ReplaceQueue(queueCopy);
						return;
					}
					return;
				}
				else
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (!string.IsNullOrEmpty(this.<>4__this.ResponseHeader) && data.Contains(this.<>4__this.ResponseHeader))
					{
						this.<>4__this.ECUExists = true;
						List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
						List<OBDRequest> list = queueCopy2;
						Predicate<OBDRequest> predicate2;
						if ((predicate2 = this.<>9__3) == null)
						{
							predicate2 = (this.<>9__3 = (OBDRequest x) => this.test_requests.Contains(x));
						}
						list.RemoveAll(predicate2);
						App.OBDReader.ReplaceQueue(queueCopy2);
						return;
					}
					if (text.Contains("7F" + CS$<>8__locals1.request.Command.Substring(0, 2)))
					{
						this.<>4__this.ECUExists = true;
						List<OBDRequest> queueCopy3 = App.OBDReader.GetQueueCopy();
						List<OBDRequest> list2 = queueCopy3;
						Predicate<OBDRequest> predicate3;
						if ((predicate3 = this.<>9__4) == null)
						{
							predicate3 = (this.<>9__4 = (OBDRequest x) => this.test_requests.Contains(x));
						}
						list2.RemoveAll(predicate3);
						App.OBDReader.ReplaceQueue(queueCopy3);
						return;
					}
					try
					{
						int num = int.Parse(CS$<>8__locals1.request.Command.Substring(0, 2), NumberStyles.HexNumber);
						string text2 = (num + 64).ToString("X2");
						if (text.Contains(text2))
						{
							this.<>4__this.ECUExists = true;
							List<OBDRequest> queueCopy4 = App.OBDReader.GetQueueCopy();
							List<OBDRequest> list3 = queueCopy4;
							Predicate<OBDRequest> predicate4;
							if ((predicate4 = this.<>9__5) == null)
							{
								predicate4 = (this.<>9__5 = (OBDRequest x) => this.test_requests.Contains(x));
							}
							list3.RemoveAll(predicate4);
							App.OBDReader.ReplaceQueue(queueCopy4);
							return;
						}
					}
					catch (Exception)
					{
					}
					if (CS$<>8__locals1.request == this.test_requests[this.test_requests.Count - 1] && !this.<>4__this.ECUExists)
					{
						List<OBDRequest> queueCopy5 = App.OBDReader.GetQueueCopy();
						queueCopy5.RemoveAll((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
						App.OBDReader.ReplaceQueue(queueCopy5);
					}
					return;
				}
			}

			// Token: 0x06003052 RID: 12370 RVA: 0x002174B8 File Offset: 0x002156B8
			internal bool <GetTestECUExistsRequest>b__1(OBDRequest x)
			{
				return this.test_requests.Contains(x);
			}

			// Token: 0x06003053 RID: 12371 RVA: 0x002174B8 File Offset: 0x002156B8
			internal bool <GetTestECUExistsRequest>b__3(OBDRequest x)
			{
				return this.test_requests.Contains(x);
			}

			// Token: 0x06003054 RID: 12372 RVA: 0x002174B8 File Offset: 0x002156B8
			internal bool <GetTestECUExistsRequest>b__4(OBDRequest x)
			{
				return this.test_requests.Contains(x);
			}

			// Token: 0x06003055 RID: 12373 RVA: 0x002174B8 File Offset: 0x002156B8
			internal bool <GetTestECUExistsRequest>b__5(OBDRequest x)
			{
				return this.test_requests.Contains(x);
			}

			// Token: 0x04001C07 RID: 7175
			public CAN11bitECU <>4__this;

			// Token: 0x04001C08 RID: 7176
			public List<OBDRequest> test_requests;

			// Token: 0x04001C09 RID: 7177
			public Predicate<OBDRequest> <>9__1;

			// Token: 0x04001C0A RID: 7178
			public Predicate<OBDRequest> <>9__3;

			// Token: 0x04001C0B RID: 7179
			public Predicate<OBDRequest> <>9__4;

			// Token: 0x04001C0C RID: 7180
			public Predicate<OBDRequest> <>9__5;

			// Token: 0x04001C0D RID: 7181
			public ResponseReceivedDelegate <>9__0;
		}

		// Token: 0x020004C2 RID: 1218
		[CompilerGenerated]
		private sealed class <>c__DisplayClass87_1
		{
			// Token: 0x06003056 RID: 12374 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass87_1()
			{
			}

			// Token: 0x06003057 RID: 12375 RVA: 0x002174C6 File Offset: 0x002156C6
			internal bool <GetTestECUExistsRequest>b__6(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04001C0E RID: 7182
			public OBDRequest request;
		}

		// Token: 0x020004C3 RID: 1219
		[CompilerGenerated]
		private sealed class <>c__DisplayClass88_0
		{
			// Token: 0x06003058 RID: 12376 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass88_0()
			{
			}

			// Token: 0x06003059 RID: 12377 RVA: 0x002174DE File Offset: 0x002156DE
			internal bool <Last_ResponseReceived>b__0(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04001C0F RID: 7183
			public OBDRequest request;
		}

		// Token: 0x020004C4 RID: 1220
		[CompilerGenerated]
		private sealed class <>c__DisplayClass98_0
		{
			// Token: 0x0600305A RID: 12378 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass98_0()
			{
			}

			// Token: 0x0600305B RID: 12379 RVA: 0x002174F6 File Offset: 0x002156F6
			internal bool <Request_ResponseReceivedCheckForNegativeResponseAndForCancellation>b__0(OBDRequest x)
			{
				return x.Header == this.request.Header && x.Command.StartsWith(this.service);
			}

			// Token: 0x0600305C RID: 12380 RVA: 0x00217524 File Offset: 0x00215724
			internal void <Request_ResponseReceivedCheckForNegativeResponseAndForCancellation>b__1(OBDRequest request2, string response)
			{
				if (response == null || request2.NR78RepeatCounter > 3)
				{
					return;
				}
				bool flag = false;
				bool flag2 = false;
				if (response != null && response.Contains("NO DATA"))
				{
					flag = true;
				}
				if (!flag)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(response);
					string[] array = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					if (text.Contains(this.nr78_string) && array.Length == 1)
					{
						flag2 = true;
					}
				}
				if (flag || flag2)
				{
					int nr78RepeatCounter = request2.NR78RepeatCounter;
					request2.NR78RepeatCounter = nr78RepeatCounter + 1;
					App.OBDReader.InsertRequestInQueue(request2);
				}
			}

			// Token: 0x04001C10 RID: 7184
			public OBDRequest request;

			// Token: 0x04001C11 RID: 7185
			public string service;

			// Token: 0x04001C12 RID: 7186
			public string nr78_string;
		}

		// Token: 0x020004C5 RID: 1221
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearDTCAsync>d__122 : IAsyncStateMachine
		{
			// Token: 0x0600305D RID: 12381 RVA: 0x002175B0 File Offset: 0x002157B0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CAN11bitECU can11bitECU = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_01BA;
						}
						CAN11bitECU.<>c__DisplayClass122_0 CS$<>8__locals1 = new CAN11bitECU.<>c__DisplayClass122_0();
						can11bitECU.cancellationToken = token;
						OBDRequest[] array = can11bitECU.GetDTCClearRequests();
						CS$<>8__locals1.succcess = false;
						OBDRequest[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							CAN11bitECU.<>c__DisplayClass122_1 CS$<>8__locals2 = new CAN11bitECU.<>c__DisplayClass122_1();
							CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
							CS$<>8__locals2.request = array2[i];
							CS$<>8__locals2.request.ResponseReceived += delegate(OBDRequest req, string response)
							{
								string negativeResponseCode = CAN11bitHelper.GetNegativeResponseCode(req, response);
								if ((negativeResponseCode == "0" && response != null && response.Contains(CS$<>8__locals2.request.ResponseMarker)) || negativeResponseCode == "78")
								{
									CS$<>8__locals2.CS$<>8__locals1.succcess = true;
								}
							};
						}
						if (can11bitECU.TestELMDevice)
						{
							OBDRequest atshtestRequest = can11bitECU.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
							OBDRequest atfctestRequest = can11bitECU.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
							array = new OBDRequest[] { atshtestRequest, atfctestRequest }.Concat(array).ToArray<OBDRequest>();
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CAN11bitECU.<ClearDTCAsync>d__122>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					if (SharedSettings.Current.UseDefaultInit)
					{
						goto IL_01C1;
					}
					OBDRequest pingRequest = App.OBDReader.GetPingRequest();
					App.OBDReader.ReplaceQueue(pingRequest);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CAN11bitECU.<ClearDTCAsync>d__122>(ref taskAwaiter, ref this);
						return;
					}
					IL_01BA:
					taskAwaiter.GetResult();
					IL_01C1:
					can11bitECU.cancellationToken = CancellationToken.None;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600305E RID: 12382 RVA: 0x002177D4 File Offset: 0x002159D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C13 RID: 7187
			public int <>1__state;

			// Token: 0x04001C14 RID: 7188
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001C15 RID: 7189
			public CAN11bitECU <>4__this;

			// Token: 0x04001C16 RID: 7190
			public CancellationToken token;

			// Token: 0x04001C17 RID: 7191
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001C18 RID: 7192
			public Action badELMDetectedCallback;

			// Token: 0x04001C19 RID: 7193
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020004C6 RID: 1222
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync>d__123 : IAsyncStateMachine
		{
			// Token: 0x0600305F RID: 12383 RVA: 0x002177E4 File Offset: 0x002159E4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CAN11bitECU can11bitECU = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new CAN11bitECU.<>c__DisplayClass123_0();
						CS$<>8__locals1.<>4__this = this;
						can11bitECU.cancellationToken = token;
						CS$<>8__locals1.sb = new StringBuilder(8);
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(can11bitECU.GetTestECUExistsRequest());
						Dictionary<string, string>.Enumerator enumerator = can11bitECU.IdentsASCII.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair = enumerator.Current;
								CAN11bitECU.<>c__DisplayClass123_1 CS$<>8__locals2 = new CAN11bitECU.<>c__DisplayClass123_1();
								CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
								string key = keyValuePair.Key;
								CS$<>8__locals2.requestTitle = keyValuePair.Value;
								OBDRequest requestForCommand = can11bitECU.GetRequestForCommand(key);
								requestForCommand.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
								{
									string text2 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.DecodeAsASCII(decodedRequest.Command, data, true);
									if (!string.IsNullOrEmpty(text2))
									{
										CS$<>8__locals2.CS$<>8__locals1.sb.AppendLine(CS$<>8__locals2.requestTitle + ": " + text2);
									}
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.ECUExists = true;
								};
								list.Add(requestForCommand);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = can11bitECU.IdentsHEX.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair2 = enumerator.Current;
								CAN11bitECU.<>c__DisplayClass123_2 CS$<>8__locals3 = new CAN11bitECU.<>c__DisplayClass123_2();
								CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
								string key2 = keyValuePair2.Key;
								CS$<>8__locals3.requestTitle = keyValuePair2.Value;
								OBDRequest requestForCommand2 = can11bitECU.GetRequestForCommand(key2);
								requestForCommand2.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
								{
									string text3 = CS$<>8__locals3.CS$<>8__locals2.<>4__this.DecodeAsHEX(decodedRequest.Command, data);
									if (!string.IsNullOrEmpty(text3))
									{
										CS$<>8__locals3.CS$<>8__locals2.sb.AppendLine(CS$<>8__locals3.requestTitle + ": " + text3);
									}
									CS$<>8__locals3.CS$<>8__locals2.<>4__this.ECUExists = true;
								};
								list.Add(requestForCommand2);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						List<OBDRequest>.Enumerator enumerator2 = list.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								OBDRequest obdrequest = enumerator2.Current;
								obdrequest.CheckLength = true;
								obdrequest.ELMFormat = can11bitECU.ELMFormat;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CAN11bitECU.<GetECUInformationReportAsync>d__123>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					CS$<>8__locals1.sb.ToString();
					text = CS$<>8__locals1.sb.ToString();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06003060 RID: 12384 RVA: 0x00217ACC File Offset: 0x00215CCC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C1A RID: 7194
			public int <>1__state;

			// Token: 0x04001C1B RID: 7195
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001C1C RID: 7196
			public CAN11bitECU <>4__this;

			// Token: 0x04001C1D RID: 7197
			public CancellationToken token;

			// Token: 0x04001C1E RID: 7198
			private CAN11bitECU.<>c__DisplayClass123_0 <>8__1;

			// Token: 0x04001C1F RID: 7199
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020004C7 RID: 1223
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadDTCAsync>d__121 : IAsyncStateMachine
		{
			// Token: 0x06003061 RID: 12385 RVA: 0x00217ADC File Offset: 0x00215CDC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CAN11bitECU can11bitECU = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0161;
						}
						can11bitECU.cancellationToken = token;
						OBDRequest[] array = can11bitECU.GetDTCReadRequests();
						if (can11bitECU.TestELMDevice)
						{
							OBDRequest atshtestRequest = can11bitECU.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
							OBDRequest atfctestRequest = can11bitECU.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
							array = new OBDRequest[] { atshtestRequest, atfctestRequest }.Concat(array).ToArray<OBDRequest>();
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CAN11bitECU.<ReadDTCAsync>d__121>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					if (SharedSettings.Current.UseDefaultInit)
					{
						goto IL_0168;
					}
					OBDRequest pingRequest = App.OBDReader.GetPingRequest();
					App.OBDReader.ReplaceQueue(pingRequest);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CAN11bitECU.<ReadDTCAsync>d__121>(ref taskAwaiter, ref this);
						return;
					}
					IL_0161:
					taskAwaiter.GetResult();
					IL_0168:
					can11bitECU.cancellationToken = CancellationToken.None;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003062 RID: 12386 RVA: 0x00217CA8 File Offset: 0x00215EA8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C20 RID: 7200
			public int <>1__state;

			// Token: 0x04001C21 RID: 7201
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001C22 RID: 7202
			public CAN11bitECU <>4__this;

			// Token: 0x04001C23 RID: 7203
			public CancellationToken token;

			// Token: 0x04001C24 RID: 7204
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001C25 RID: 7205
			public Action badELMDetectedCallback;

			// Token: 0x04001C26 RID: 7206
			private TaskAwaiter <>u__1;
		}
	}
}
