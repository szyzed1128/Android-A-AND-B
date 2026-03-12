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
	// Token: 0x020004E7 RID: 1255
	internal class KWPECU : ObservableCollection<DTCItemV2>, IECU, IEnumerable<DTCItemV2>, IEnumerable, INotifyPropertyChanged
	{
		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x060030E4 RID: 12516 RVA: 0x0021DB88 File Offset: 0x0021BD88
		// (set) Token: 0x060030E5 RID: 12517 RVA: 0x0021DB90 File Offset: 0x0021BD90
		public string ECUInitSequence
		{
			[CompilerGenerated]
			get
			{
				return this.<ECUInitSequence>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ECUInitSequence>k__BackingField = value;
			}
		} = "";

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x0021DB99 File Offset: 0x0021BD99
		// (set) Token: 0x060030E7 RID: 12519 RVA: 0x0021DBA1 File Offset: 0x0021BDA1
		public bool AddReinitAfter
		{
			[CompilerGenerated]
			get
			{
				return this.<AddReinitAfter>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AddReinitAfter>k__BackingField = value;
			}
		}

		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x060030E8 RID: 12520 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public ELMFormat ELMFormat
		{
			get
			{
				return ELMFormat.KWP;
			}
		}

		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x0021DBAA File Offset: 0x0021BDAA
		// (set) Token: 0x060030EA RID: 12522 RVA: 0x0021DBB2 File Offset: 0x0021BDB2
		public int Protocol
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

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x060030EB RID: 12523 RVA: 0x0021DBBB File Offset: 0x0021BDBB
		// (set) Token: 0x060030EC RID: 12524 RVA: 0x0021DBC3 File Offset: 0x0021BDC3
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

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x0021DBCC File Offset: 0x0021BDCC
		// (set) Token: 0x060030EE RID: 12526 RVA: 0x0021DBD4 File Offset: 0x0021BDD4
		public string ShortName
		{
			[CompilerGenerated]
			get
			{
				return this.<ShortName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShortName>k__BackingField = value;
			}
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x060030EF RID: 12527 RVA: 0x0021DBDD File Offset: 0x0021BDDD
		// (set) Token: 0x060030F0 RID: 12528 RVA: 0x0021DBE5 File Offset: 0x0021BDE5
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

		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x060030F1 RID: 12529 RVA: 0x0021DBEE File Offset: 0x0021BDEE
		// (set) Token: 0x060030F2 RID: 12530 RVA: 0x0021DBF6 File Offset: 0x0021BDF6
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

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x060030F3 RID: 12531 RVA: 0x0021DBFF File Offset: 0x0021BDFF
		// (set) Token: 0x060030F4 RID: 12532 RVA: 0x0021DC07 File Offset: 0x0021BE07
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

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x060030F5 RID: 12533 RVA: 0x0021DC10 File Offset: 0x0021BE10
		// (set) Token: 0x060030F6 RID: 12534 RVA: 0x0021DC18 File Offset: 0x0021BE18
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

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x0021DC21 File Offset: 0x0021BE21
		// (set) Token: 0x060030F8 RID: 12536 RVA: 0x0021DC29 File Offset: 0x0021BE29
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

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x0021DC32 File Offset: 0x0021BE32
		// (set) Token: 0x060030FA RID: 12538 RVA: 0x0021DC3A File Offset: 0x0021BE3A
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
		} = new List<string>();

		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x0021DC43 File Offset: 0x0021BE43
		// (set) Token: 0x060030FC RID: 12540 RVA: 0x0021DC4B File Offset: 0x0021BE4B
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
		} = new List<string>();

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x0021DC54 File Offset: 0x0021BE54
		// (set) Token: 0x060030FE RID: 12542 RVA: 0x0021DC5C File Offset: 0x0021BE5C
		public List<string> OpenSessionCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<OpenSessionCommands>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<OpenSessionCommands>k__BackingField = value;
			}
		} = new List<string>();

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x060030FF RID: 12543 RVA: 0x0021DC65 File Offset: 0x0021BE65
		// (set) Token: 0x06003100 RID: 12544 RVA: 0x0021DC6D File Offset: 0x0021BE6D
		public List<string> CloseSessionCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<CloseSessionCommands>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<CloseSessionCommands>k__BackingField = value;
			}
		} = new List<string>();

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x0021DC76 File Offset: 0x0021BE76
		// (set) Token: 0x06003102 RID: 12546 RVA: 0x0021DC7E File Offset: 0x0021BE7E
		public bool Highlighted
		{
			[CompilerGenerated]
			get
			{
				return this.<Highlighted>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Highlighted>k__BackingField = value;
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x0021DC87 File Offset: 0x0021BE87
		// (set) Token: 0x06003104 RID: 12548 RVA: 0x0021DC8F File Offset: 0x0021BE8F
		public virtual bool ECUExists
		{
			get
			{
				return this._ECUExists;
			}
			protected set
			{
				this._ECUExists = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("ECUExists"));
			}
		}

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x0021DCA8 File Offset: 0x0021BEA8
		// (set) Token: 0x06003106 RID: 12550 RVA: 0x0021DCB0 File Offset: 0x0021BEB0
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
					this.OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
					this.OnPropertyChanged(new PropertyChangedEventArgs("ExpandedStateImage"));
					this.UpdateCollection();
				}
			}
		}

		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x0021DCE8 File Offset: 0x0021BEE8
		// (set) Token: 0x06003108 RID: 12552 RVA: 0x0021DCF0 File Offset: 0x0021BEF0
		public string ExpandedStateImage
		{
			[CompilerGenerated]
			get
			{
				return this.<ExpandedStateImage>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ExpandedStateImage>k__BackingField = value;
			}
		} = "";

		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x0021DCF9 File Offset: 0x0021BEF9
		// (set) Token: 0x0600310A RID: 12554 RVA: 0x0021DD01 File Offset: 0x0021BF01
		public bool IsSelected
		{
			get
			{
				return this._IsSelected;
			}
			set
			{
				this._IsSelected = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("IsSelected"));
			}
		}

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x0021DD1A File Offset: 0x0021BF1A
		public List<DTCItemV2> DTCCollection
		{
			[CompilerGenerated]
			get
			{
				return this.<DTCCollection>k__BackingField;
			}
		} = new List<DTCItemV2>();

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x0600310C RID: 12556 RVA: 0x0021DD22 File Offset: 0x0021BF22
		// (set) Token: 0x0600310D RID: 12557 RVA: 0x0021DD2A File Offset: 0x0021BF2A
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
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x0600310E RID: 12558 RVA: 0x0021DD33 File Offset: 0x0021BF33
		// (set) Token: 0x0600310F RID: 12559 RVA: 0x0021DD3B File Offset: 0x0021BF3B
		public bool TestELMDevice
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

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06003110 RID: 12560 RVA: 0x0021DD44 File Offset: 0x0021BF44
		// (set) Token: 0x06003111 RID: 12561 RVA: 0x0021DD4C File Offset: 0x0021BF4C
		public bool CycleThroughOpenSessionCommands
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

		// Token: 0x06003112 RID: 12562 RVA: 0x0021DD58 File Offset: 0x0021BF58
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

		// Token: 0x06003113 RID: 12563 RVA: 0x0021DDE0 File Offset: 0x0021BFE0
		public async Task ClearDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken token)
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
				array = new OBDRequest[] { atshtestRequest }.Concat(array).ToArray<OBDRequest>();
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

		// Token: 0x06003114 RID: 12564 RVA: 0x0021DE3C File Offset: 0x0021C03C
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
				obdrequest.ELMFormat = ELMFormat.CAN11bit;
			}
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			sb.ToString();
			return sb.ToString();
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x0021DE88 File Offset: 0x0021C088
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

		// Token: 0x06003116 RID: 12566 RVA: 0x0021DF28 File Offset: 0x0021C128
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

		// Token: 0x06003117 RID: 12567 RVA: 0x0021DF64 File Offset: 0x0021C164
		public virtual OBDRequest GetRequestForCommand(string cmd)
		{
			string text = this.ECUInitSequence;
			foreach (string text2 in this.OpenSessionCommands)
			{
				text = text + ";" + text2;
			}
			string text3 = "ATSPDEF";
			foreach (string text4 in this.CloseSessionCommands)
			{
				text3 = text3 + text4 + ";";
			}
			if (this.AddReinitAfter)
			{
				text3 += "REINIT;";
			}
			return new OBDRequest(cmd, this.RequestHeader, text, text3, false)
			{
				ELMFormat = ELMFormat.KWP
			};
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x0021E040 File Offset: 0x0021C240
		public async Task ReadDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken token)
		{
			this.cancellationToken = token;
			OBDRequest[] array = this.GetDTCReadRequests();
			if (this.TestELMDevice)
			{
				OBDRequest atshtestRequest = this.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
				array = new OBDRequest[] { atshtestRequest }.Concat(array).ToArray<OBDRequest>();
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

		// Token: 0x06003119 RID: 12569 RVA: 0x0021E09B File Offset: 0x0021C29B
		public void Reset()
		{
			base.Clear();
			this.DTCCollection.Clear();
			this.ECUExists = false;
			this.Highlighted = false;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x0021E0BC File Offset: 0x0021C2BC
		public void UpdateCollection()
		{
			if (this.Expanded)
			{
				List<DTCItemV2> temp_list = new List<DTCItemV2>(this.DTCCollection);
				if (SharedSettings.Current.HideArchiveDTC)
				{
					temp_list.RemoveAll((DTCItemV2 x) => x.IsArchive);
				}
				if (SharedSettings.Current.HideDTCWithUncomplitedTests)
				{
					temp_list.RemoveAll((DTCItemV2 x) => x.OnlyTestNotComplitedDTC);
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

		// Token: 0x0600311B RID: 12571 RVA: 0x0021E1F4 File Offset: 0x0021C3F4
		protected virtual OBDRequest[] GetDTCReadRequests()
		{
			OBDRequest pingRequest = App.OBDReader.GetPingRequest();
			pingRequest.DoNotDecode = true;
			List<OBDRequest> list = this.ReadDTCCommands.Select((string x) => this.GetRequestForCommand(x)).ToList<OBDRequest>();
			OBDRequest[] testECUExistsRequest = this.GetTestECUExistsRequest();
			List<OBDRequest> list2 = new List<OBDRequest>(list.Count + testECUExistsRequest.Length);
			list2.AddRange(testECUExistsRequest);
			foreach (OBDRequest obdrequest in list)
			{
				list2.Add(obdrequest);
			}
			if (SharedSettings.Current.AlwaysPingECU)
			{
				list2.Add(pingRequest);
			}
			foreach (OBDRequest obdrequest2 in list)
			{
				obdrequest2.OBDMode = OBDDataReader.OBDModes.ReadDTC;
				obdrequest2.CheckLength = false;
				obdrequest2.ResponseReceived += this.DtcReadRequest_ResponseReceived;
				obdrequest2.ResponseDecoded += this.DtcReadRequest_ResponseDecoded1;
			}
			return list2.ToArray();
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x0021E314 File Offset: 0x0021C514
		private void DtcReadRequest_ResponseDecoded1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				this.ECUExists = true;
				List<DTCItemV2> dtcs = DTCDecoder.DecodeData(request, request.Header, data);
				Device.BeginInvokeOnMainThread(delegate
				{
					bool flag = false;
					using (List<DTCItemV2>.Enumerator enumerator = dtcs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DTCItemV2 dtc = enumerator.Current;
							if (!this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == dtc.Code))
							{
								dtc.LoadDescription();
								dtc.ECU = this.Name;
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

		// Token: 0x0600311D RID: 12573 RVA: 0x000027D4 File Offset: 0x000009D4
		private void DtcReadRequest_ResponseReceived(OBDRequest request, string data)
		{
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x0021E368 File Offset: 0x0021C568
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

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x0021E3BB File Offset: 0x0021C5BB
		// (set) Token: 0x06003120 RID: 12576 RVA: 0x0021E3C3 File Offset: 0x0021C5C3
		public bool TestIfEcuExists
		{
			[CompilerGenerated]
			get
			{
				return this.<TestIfEcuExists>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TestIfEcuExists>k__BackingField = value;
			}
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x0021E3CC File Offset: 0x0021C5CC
		protected virtual OBDRequest[] GetTestECUExistsRequest()
		{
			if (!this.TestIfEcuExists)
			{
				return new OBDRequest[0];
			}
			List<string> list;
			if (this.OpenSessionCommands.Count > 0)
			{
				list = this.OpenSessionCommands;
			}
			else
			{
				list = new List<string> { "3E00" };
			}
			List<OBDRequest> test_requests = new List<OBDRequest>(list.Count);
			Predicate<OBDRequest> <>9__1;
			Predicate<OBDRequest> <>9__2;
			Predicate<OBDRequest> <>9__3;
			Predicate<OBDRequest> <>9__4;
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
						if (!string.IsNullOrEmpty(data))
						{
							data.Contains("NO DATA");
						}
						string text2 = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (!string.IsNullOrEmpty(this.ResponseHeader) && data.Contains(this.ResponseHeader))
						{
							this.ECUExists = true;
							List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
							List<OBDRequest> list2 = queueCopy;
							Predicate<OBDRequest> predicate2;
							if ((predicate2 = <>9__2) == null)
							{
								predicate2 = (<>9__2 = (OBDRequest x) => test_requests.Contains(x));
							}
							list2.RemoveAll(predicate2);
							App.OBDReader.ReplaceQueue(queueCopy);
							return;
						}
						if (text2.Contains("7F" + request.Command.Substring(0, 2)))
						{
							this.ECUExists = true;
							List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
							List<OBDRequest> list3 = queueCopy2;
							Predicate<OBDRequest> predicate3;
							if ((predicate3 = <>9__3) == null)
							{
								predicate3 = (<>9__3 = (OBDRequest x) => test_requests.Contains(x));
							}
							list3.RemoveAll(predicate3);
							App.OBDReader.ReplaceQueue(queueCopy2);
							return;
						}
						try
						{
							int num = int.Parse(request.Command.Substring(0, 2), NumberStyles.HexNumber);
							string text3 = (num + 64).ToString("X2");
							if (text2.Contains(text3))
							{
								this.ECUExists = true;
								List<OBDRequest> queueCopy3 = App.OBDReader.GetQueueCopy();
								List<OBDRequest> list4 = queueCopy3;
								Predicate<OBDRequest> predicate4;
								if ((predicate4 = <>9__4) == null)
								{
									predicate4 = (<>9__4 = (OBDRequest x) => test_requests.Contains(x));
								}
								list4.RemoveAll(predicate4);
								App.OBDReader.ReplaceQueue(queueCopy3);
								return;
							}
						}
						catch (Exception)
						{
						}
						if (request == test_requests[test_requests.Count - 1] && !this.ECUExists)
						{
							List<OBDRequest> queueCopy4 = App.OBDReader.GetQueueCopy();
							queueCopy4.RemoveAll((OBDRequest x) => x.Header == request.Header);
							App.OBDReader.ReplaceQueue(queueCopy4);
						}
					});
				}
				obdrequest.ResponseReceived += responseReceivedDelegate;
			}
			return test_requests.ToArray();
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x0021E4BC File Offset: 0x0021C6BC
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

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06003123 RID: 12579 RVA: 0x0021E5D2 File Offset: 0x0021C7D2
		// (set) Token: 0x06003124 RID: 12580 RVA: 0x0021E5DA File Offset: 0x0021C7DA
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

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x0021E5E3 File Offset: 0x0021C7E3
		// (set) Token: 0x06003126 RID: 12582 RVA: 0x0021E5EB File Offset: 0x0021C7EB
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

		// Token: 0x06003127 RID: 12583 RVA: 0x0021E5F4 File Offset: 0x0021C7F4
		public static List<KWPECU> BuildDetectedECUs()
		{
			List<string> readDTCCommands = new List<string>
			{
				"03", "03", "07", "07", "0A", "1800FF00", "1802FF00", "1802FFFF", "1800FFFF", "18FF00",
				"17FF00", "13FF00", "1902AF", "1902AC", "19028D", "190223", "190278", "190208", "190FAC", "190F8D",
				"190F23", "19D2FF00"
			};
			List<string> clearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFFFF", "140000" };
			string headerFirstByte = "";
			int currentProtocolNumber = App.OBDReader.CurrentProtocolNumber;
			if (currentProtocolNumber > 0 && currentProtocolNumber < 6)
			{
				switch (currentProtocolNumber)
				{
				case 1:
					headerFirstByte = "61";
					break;
				case 2:
					headerFirstByte = "6C";
					break;
				case 3:
					headerFirstByte = "68";
					break;
				case 4:
				case 5:
					headerFirstByte = "80";
					break;
				}
				return App.OBDReader.ECUHeaders.Select((ECUHeader x) => new KWPECU
				{
					Name = string.Format("ECU ID: {0}", x),
					RequestHeader = string.Format("{0}{1}F1", headerFirstByte, x),
					ReadDTCCommands = readDTCCommands,
					ClearDTCCommands = clearDTCCommands
				}).ToList<KWPECU>();
			}
			return new List<KWPECU>(0);
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x0021E7E8 File Offset: 0x0021C9E8
		public KWPECU()
		{
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x00216CC7 File Offset: 0x00214EC7
		[CompilerGenerated]
		private void <UpdateCollection>b__102_0()
		{
			base.Clear();
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x0021E8A3 File Offset: 0x0021CAA3
		[CompilerGenerated]
		private OBDRequest <GetDTCReadRequests>b__103_0(string x)
		{
			return this.GetRequestForCommand(x);
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x0021E8AC File Offset: 0x0021CAAC
		[CompilerGenerated]
		private OBDRequest <GetDTCClearRequests>b__106_0(string x)
		{
			OBDRequest requestForCommand = this.GetRequestForCommand(x);
			requestForCommand.OBDMode = OBDDataReader.OBDModes.ClearDTC;
			return requestForCommand;
		}

		// Token: 0x04001C3E RID: 7230
		protected CancellationToken cancellationToken = CancellationToken.None;

		// Token: 0x04001C3F RID: 7231
		[CompilerGenerated]
		private string <ECUInitSequence>k__BackingField;

		// Token: 0x04001C40 RID: 7232
		[CompilerGenerated]
		private bool <AddReinitAfter>k__BackingField;

		// Token: 0x04001C41 RID: 7233
		[CompilerGenerated]
		private int <Protocol>k__BackingField;

		// Token: 0x04001C42 RID: 7234
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04001C43 RID: 7235
		[CompilerGenerated]
		private string <ShortName>k__BackingField;

		// Token: 0x04001C44 RID: 7236
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x04001C45 RID: 7237
		[CompilerGenerated]
		private string <ResponseHeader>k__BackingField;

		// Token: 0x04001C46 RID: 7238
		[CompilerGenerated]
		private string <ExtendedAddress>k__BackingField;

		// Token: 0x04001C47 RID: 7239
		[CompilerGenerated]
		private string <TesterAddress>k__BackingField;

		// Token: 0x04001C48 RID: 7240
		[CompilerGenerated]
		private bool <BadELMDetected>k__BackingField;

		// Token: 0x04001C49 RID: 7241
		[CompilerGenerated]
		private List<string> <ReadDTCCommands>k__BackingField;

		// Token: 0x04001C4A RID: 7242
		[CompilerGenerated]
		private List<string> <ClearDTCCommands>k__BackingField;

		// Token: 0x04001C4B RID: 7243
		[CompilerGenerated]
		private List<string> <OpenSessionCommands>k__BackingField;

		// Token: 0x04001C4C RID: 7244
		[CompilerGenerated]
		private List<string> <CloseSessionCommands>k__BackingField;

		// Token: 0x04001C4D RID: 7245
		[CompilerGenerated]
		private bool <Highlighted>k__BackingField;

		// Token: 0x04001C4E RID: 7246
		private bool _ECUExists;

		// Token: 0x04001C4F RID: 7247
		private bool _Expanded = true;

		// Token: 0x04001C50 RID: 7248
		[CompilerGenerated]
		private string <ExpandedStateImage>k__BackingField;

		// Token: 0x04001C51 RID: 7249
		private bool _IsSelected = true;

		// Token: 0x04001C52 RID: 7250
		[CompilerGenerated]
		private readonly List<DTCItemV2> <DTCCollection>k__BackingField;

		// Token: 0x04001C53 RID: 7251
		[CompilerGenerated]
		private bool <RemoveOtherRequestsIfUDSReadResponded>k__BackingField;

		// Token: 0x04001C54 RID: 7252
		[CompilerGenerated]
		private bool <TestELMDevice>k__BackingField;

		// Token: 0x04001C55 RID: 7253
		[CompilerGenerated]
		private bool <CycleThroughOpenSessionCommands>k__BackingField;

		// Token: 0x04001C56 RID: 7254
		[CompilerGenerated]
		private bool <TestIfEcuExists>k__BackingField;

		// Token: 0x04001C57 RID: 7255
		[CompilerGenerated]
		private Dictionary<string, string> <IdentsASCII>k__BackingField;

		// Token: 0x04001C58 RID: 7256
		[CompilerGenerated]
		private Dictionary<string, string> <IdentsHEX>k__BackingField;

		// Token: 0x020004E8 RID: 1256
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600312C RID: 12588 RVA: 0x0021E8BC File Offset: 0x0021CABC
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600312D RID: 12589 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600312E RID: 12590 RVA: 0x00016849 File Offset: 0x00014A49
			internal char <DecodeAsASCII>b__97_0(byte x)
			{
				return (char)x;
			}

			// Token: 0x0600312F RID: 12591 RVA: 0x00216CDB File Offset: 0x00214EDB
			internal bool <DecodeAsASCII>b__97_1(char ch)
			{
				return (byte)ch >= 32 && (byte)ch <= 126;
			}

			// Token: 0x06003130 RID: 12592 RVA: 0x0021E8C8 File Offset: 0x0021CAC8
			internal bool <UpdateCollection>b__102_1(DTCItemV2 x)
			{
				return x.IsArchive;
			}

			// Token: 0x06003131 RID: 12593 RVA: 0x0021E8D0 File Offset: 0x0021CAD0
			internal bool <UpdateCollection>b__102_2(DTCItemV2 x)
			{
				return x.OnlyTestNotComplitedDTC;
			}

			// Token: 0x04001C59 RID: 7257
			public static readonly KWPECU.<>c <>9 = new KWPECU.<>c();

			// Token: 0x04001C5A RID: 7258
			public static Func<byte, char> <>9__97_0;

			// Token: 0x04001C5B RID: 7259
			public static Func<char, bool> <>9__97_1;

			// Token: 0x04001C5C RID: 7260
			public static Predicate<DTCItemV2> <>9__102_1;

			// Token: 0x04001C5D RID: 7261
			public static Predicate<DTCItemV2> <>9__102_2;
		}

		// Token: 0x020004E9 RID: 1257
		[CompilerGenerated]
		private sealed class <>c__DisplayClass102_0
		{
			// Token: 0x06003132 RID: 12594 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass102_0()
			{
			}

			// Token: 0x06003133 RID: 12595 RVA: 0x0021E8D8 File Offset: 0x0021CAD8
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

			// Token: 0x04001C5E RID: 7262
			public List<DTCItemV2> temp_list;

			// Token: 0x04001C5F RID: 7263
			public KWPECU <>4__this;
		}

		// Token: 0x020004EA RID: 1258
		[CompilerGenerated]
		private sealed class <>c__DisplayClass104_0
		{
			// Token: 0x06003134 RID: 12596 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass104_0()
			{
			}

			// Token: 0x06003135 RID: 12597 RVA: 0x0021E94C File Offset: 0x0021CB4C
			internal void <DtcReadRequest_ResponseDecoded1>b__0()
			{
				bool flag = false;
				using (List<DTCItemV2>.Enumerator enumerator = this.dtcs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KWPECU.<>c__DisplayClass104_1 CS$<>8__locals1 = new KWPECU.<>c__DisplayClass104_1();
						CS$<>8__locals1.dtc = enumerator.Current;
						if (!this.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals1.dtc.Code))
						{
							CS$<>8__locals1.dtc.LoadDescription();
							CS$<>8__locals1.dtc.ECU = this.<>4__this.Name;
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

			// Token: 0x04001C60 RID: 7264
			public KWPECU <>4__this;

			// Token: 0x04001C61 RID: 7265
			public string responseHeader;

			// Token: 0x04001C62 RID: 7266
			public List<DTCItemV2> dtcs;
		}

		// Token: 0x020004EB RID: 1259
		[CompilerGenerated]
		private sealed class <>c__DisplayClass104_1
		{
			// Token: 0x06003136 RID: 12598 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass104_1()
			{
			}

			// Token: 0x06003137 RID: 12599 RVA: 0x0021EA38 File Offset: 0x0021CC38
			internal bool <DtcReadRequest_ResponseDecoded1>b__1(DTCItemV2 x)
			{
				return x.Code == this.dtc.Code;
			}

			// Token: 0x04001C63 RID: 7267
			public DTCItemV2 dtc;
		}

		// Token: 0x020004EC RID: 1260
		[CompilerGenerated]
		private sealed class <>c__DisplayClass111_0
		{
			// Token: 0x06003138 RID: 12600 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass111_0()
			{
			}

			// Token: 0x06003139 RID: 12601 RVA: 0x0021EA50 File Offset: 0x0021CC50
			internal void <GetTestECUExistsRequest>b__0(OBDRequest request, string data)
			{
				KWPECU.<>c__DisplayClass111_1 CS$<>8__locals1 = new KWPECU.<>c__DisplayClass111_1();
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
				if (!string.IsNullOrEmpty(data))
				{
					data.Contains("NO DATA");
				}
				string text = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (!string.IsNullOrEmpty(this.<>4__this.ResponseHeader) && data.Contains(this.<>4__this.ResponseHeader))
				{
					this.<>4__this.ECUExists = true;
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					List<OBDRequest> list = queueCopy;
					Predicate<OBDRequest> predicate2;
					if ((predicate2 = this.<>9__2) == null)
					{
						predicate2 = (this.<>9__2 = (OBDRequest x) => this.test_requests.Contains(x));
					}
					list.RemoveAll(predicate2);
					App.OBDReader.ReplaceQueue(queueCopy);
					return;
				}
				if (text.Contains("7F" + CS$<>8__locals1.request.Command.Substring(0, 2)))
				{
					this.<>4__this.ECUExists = true;
					List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
					List<OBDRequest> list2 = queueCopy2;
					Predicate<OBDRequest> predicate3;
					if ((predicate3 = this.<>9__3) == null)
					{
						predicate3 = (this.<>9__3 = (OBDRequest x) => this.test_requests.Contains(x));
					}
					list2.RemoveAll(predicate3);
					App.OBDReader.ReplaceQueue(queueCopy2);
					return;
				}
				try
				{
					int num = int.Parse(CS$<>8__locals1.request.Command.Substring(0, 2), NumberStyles.HexNumber);
					string text2 = (num + 64).ToString("X2");
					if (text.Contains(text2))
					{
						this.<>4__this.ECUExists = true;
						List<OBDRequest> queueCopy3 = App.OBDReader.GetQueueCopy();
						List<OBDRequest> list3 = queueCopy3;
						Predicate<OBDRequest> predicate4;
						if ((predicate4 = this.<>9__4) == null)
						{
							predicate4 = (this.<>9__4 = (OBDRequest x) => this.test_requests.Contains(x));
						}
						list3.RemoveAll(predicate4);
						App.OBDReader.ReplaceQueue(queueCopy3);
						return;
					}
				}
				catch (Exception)
				{
				}
				if (CS$<>8__locals1.request == this.test_requests[this.test_requests.Count - 1] && !this.<>4__this.ECUExists)
				{
					List<OBDRequest> queueCopy4 = App.OBDReader.GetQueueCopy();
					queueCopy4.RemoveAll((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
					App.OBDReader.ReplaceQueue(queueCopy4);
				}
			}

			// Token: 0x0600313A RID: 12602 RVA: 0x0021EC94 File Offset: 0x0021CE94
			internal bool <GetTestECUExistsRequest>b__1(OBDRequest x)
			{
				return this.test_requests.Contains(x);
			}

			// Token: 0x0600313B RID: 12603 RVA: 0x0021EC94 File Offset: 0x0021CE94
			internal bool <GetTestECUExistsRequest>b__2(OBDRequest x)
			{
				return this.test_requests.Contains(x);
			}

			// Token: 0x0600313C RID: 12604 RVA: 0x0021EC94 File Offset: 0x0021CE94
			internal bool <GetTestECUExistsRequest>b__3(OBDRequest x)
			{
				return this.test_requests.Contains(x);
			}

			// Token: 0x0600313D RID: 12605 RVA: 0x0021EC94 File Offset: 0x0021CE94
			internal bool <GetTestECUExistsRequest>b__4(OBDRequest x)
			{
				return this.test_requests.Contains(x);
			}

			// Token: 0x04001C64 RID: 7268
			public KWPECU <>4__this;

			// Token: 0x04001C65 RID: 7269
			public List<OBDRequest> test_requests;

			// Token: 0x04001C66 RID: 7270
			public Predicate<OBDRequest> <>9__1;

			// Token: 0x04001C67 RID: 7271
			public Predicate<OBDRequest> <>9__2;

			// Token: 0x04001C68 RID: 7272
			public Predicate<OBDRequest> <>9__3;

			// Token: 0x04001C69 RID: 7273
			public Predicate<OBDRequest> <>9__4;

			// Token: 0x04001C6A RID: 7274
			public ResponseReceivedDelegate <>9__0;
		}

		// Token: 0x020004ED RID: 1261
		[CompilerGenerated]
		private sealed class <>c__DisplayClass111_1
		{
			// Token: 0x0600313E RID: 12606 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass111_1()
			{
			}

			// Token: 0x0600313F RID: 12607 RVA: 0x0021ECA2 File Offset: 0x0021CEA2
			internal bool <GetTestECUExistsRequest>b__5(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04001C6B RID: 7275
			public OBDRequest request;
		}

		// Token: 0x020004EE RID: 1262
		[CompilerGenerated]
		private sealed class <>c__DisplayClass121_0
		{
			// Token: 0x06003140 RID: 12608 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass121_0()
			{
			}

			// Token: 0x06003141 RID: 12609 RVA: 0x0021ECBC File Offset: 0x0021CEBC
			internal KWPECU <BuildDetectedECUs>b__0(ECUHeader x)
			{
				return new KWPECU
				{
					Name = string.Format("ECU ID: {0}", x),
					RequestHeader = string.Format("{0}{1}F1", this.headerFirstByte, x),
					ReadDTCCommands = this.readDTCCommands,
					ClearDTCCommands = this.clearDTCCommands
				};
			}

			// Token: 0x04001C6C RID: 7276
			public string headerFirstByte;

			// Token: 0x04001C6D RID: 7277
			public List<string> readDTCCommands;

			// Token: 0x04001C6E RID: 7278
			public List<string> clearDTCCommands;
		}

		// Token: 0x020004EF RID: 1263
		[CompilerGenerated]
		private sealed class <>c__DisplayClass94_0
		{
			// Token: 0x06003142 RID: 12610 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass94_0()
			{
			}

			// Token: 0x06003143 RID: 12611 RVA: 0x0021ED10 File Offset: 0x0021CF10
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

			// Token: 0x06003144 RID: 12612 RVA: 0x0021EDE8 File Offset: 0x0021CFE8
			internal bool <GetATSHTestRequest>b__1(OBDRequest x)
			{
				return x.Header == this.<>4__this.RequestHeader;
			}

			// Token: 0x06003145 RID: 12613 RVA: 0x0021EE00 File Offset: 0x0021D000
			internal void <GetATSHTestRequest>b__2()
			{
				Action action = this.badELMDetectedCallback;
				if (action == null)
				{
					return;
				}
				action();
			}

			// Token: 0x04001C6F RID: 7279
			public KWPECU <>4__this;

			// Token: 0x04001C70 RID: 7280
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001C71 RID: 7281
			public Action badELMDetectedCallback;

			// Token: 0x04001C72 RID: 7282
			public Predicate<OBDRequest> <>9__1;

			// Token: 0x04001C73 RID: 7283
			public Action <>9__2;
		}

		// Token: 0x020004F0 RID: 1264
		[CompilerGenerated]
		private sealed class <>c__DisplayClass95_0
		{
			// Token: 0x06003146 RID: 12614 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass95_0()
			{
			}

			// Token: 0x04001C74 RID: 7284
			public bool succcess;
		}

		// Token: 0x020004F1 RID: 1265
		[CompilerGenerated]
		private sealed class <>c__DisplayClass95_1
		{
			// Token: 0x06003147 RID: 12615 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass95_1()
			{
			}

			// Token: 0x06003148 RID: 12616 RVA: 0x0021EE14 File Offset: 0x0021D014
			internal void <ClearDTCAsync>b__0(OBDRequest req, string response)
			{
				string negativeResponseCode = CAN11bitHelper.GetNegativeResponseCode(req, response);
				if ((negativeResponseCode == "0" && response != null && response.Contains(this.request.ResponseMarker)) || negativeResponseCode == "78")
				{
					this.CS$<>8__locals1.succcess = true;
				}
			}

			// Token: 0x04001C75 RID: 7285
			public OBDRequest request;

			// Token: 0x04001C76 RID: 7286
			public KWPECU.<>c__DisplayClass95_0 CS$<>8__locals1;
		}

		// Token: 0x020004F2 RID: 1266
		[CompilerGenerated]
		private sealed class <>c__DisplayClass96_0
		{
			// Token: 0x06003149 RID: 12617 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass96_0()
			{
			}

			// Token: 0x04001C77 RID: 7287
			public KWPECU <>4__this;

			// Token: 0x04001C78 RID: 7288
			public StringBuilder sb;
		}

		// Token: 0x020004F3 RID: 1267
		[CompilerGenerated]
		private sealed class <>c__DisplayClass96_1
		{
			// Token: 0x0600314A RID: 12618 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass96_1()
			{
			}

			// Token: 0x0600314B RID: 12619 RVA: 0x0021EE68 File Offset: 0x0021D068
			internal void <GetECUInformationReportAsync>b__0(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
			{
				string text = this.CS$<>8__locals1.<>4__this.DecodeAsASCII(decodedRequest.Command, data, true);
				if (!string.IsNullOrEmpty(text))
				{
					this.CS$<>8__locals1.sb.AppendLine(this.requestTitle + ": " + text);
				}
				this.CS$<>8__locals1.<>4__this.ECUExists = true;
			}

			// Token: 0x04001C79 RID: 7289
			public string requestTitle;

			// Token: 0x04001C7A RID: 7290
			public KWPECU.<>c__DisplayClass96_0 CS$<>8__locals1;
		}

		// Token: 0x020004F4 RID: 1268
		[CompilerGenerated]
		private sealed class <>c__DisplayClass96_2
		{
			// Token: 0x0600314C RID: 12620 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass96_2()
			{
			}

			// Token: 0x0600314D RID: 12621 RVA: 0x0021EECC File Offset: 0x0021D0CC
			internal void <GetECUInformationReportAsync>b__1(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
			{
				string text = this.CS$<>8__locals2.<>4__this.DecodeAsHEX(decodedRequest.Command, data);
				if (!string.IsNullOrEmpty(text))
				{
					this.CS$<>8__locals2.sb.AppendLine(this.requestTitle + ": " + text);
				}
				this.CS$<>8__locals2.<>4__this.ECUExists = true;
			}

			// Token: 0x04001C7B RID: 7291
			public string requestTitle;

			// Token: 0x04001C7C RID: 7292
			public KWPECU.<>c__DisplayClass96_0 CS$<>8__locals2;
		}

		// Token: 0x020004F5 RID: 1269
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearDTCAsync>d__95 : IAsyncStateMachine
		{
			// Token: 0x0600314E RID: 12622 RVA: 0x0021EF2C File Offset: 0x0021D12C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				KWPECU kwpecu = this;
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
							goto IL_01A1;
						}
						KWPECU.<>c__DisplayClass95_0 CS$<>8__locals1 = new KWPECU.<>c__DisplayClass95_0();
						kwpecu.cancellationToken = token;
						OBDRequest[] array = kwpecu.GetDTCClearRequests();
						CS$<>8__locals1.succcess = false;
						OBDRequest[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							KWPECU.<>c__DisplayClass95_1 CS$<>8__locals2 = new KWPECU.<>c__DisplayClass95_1();
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
						if (kwpecu.TestELMDevice)
						{
							OBDRequest atshtestRequest = kwpecu.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
							array = new OBDRequest[] { atshtestRequest }.Concat(array).ToArray<OBDRequest>();
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, KWPECU.<ClearDTCAsync>d__95>(ref taskAwaiter, ref this);
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
						goto IL_01A8;
					}
					OBDRequest pingRequest = App.OBDReader.GetPingRequest();
					App.OBDReader.ReplaceQueue(pingRequest);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, KWPECU.<ClearDTCAsync>d__95>(ref taskAwaiter, ref this);
						return;
					}
					IL_01A1:
					taskAwaiter.GetResult();
					IL_01A8:
					kwpecu.cancellationToken = CancellationToken.None;
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

			// Token: 0x0600314F RID: 12623 RVA: 0x0021F138 File Offset: 0x0021D338
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C7D RID: 7293
			public int <>1__state;

			// Token: 0x04001C7E RID: 7294
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001C7F RID: 7295
			public KWPECU <>4__this;

			// Token: 0x04001C80 RID: 7296
			public CancellationToken token;

			// Token: 0x04001C81 RID: 7297
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001C82 RID: 7298
			public Action badELMDetectedCallback;

			// Token: 0x04001C83 RID: 7299
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020004F6 RID: 1270
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync>d__96 : IAsyncStateMachine
		{
			// Token: 0x06003150 RID: 12624 RVA: 0x0021F148 File Offset: 0x0021D348
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				KWPECU kwpecu = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new KWPECU.<>c__DisplayClass96_0();
						CS$<>8__locals1.<>4__this = this;
						kwpecu.cancellationToken = token;
						CS$<>8__locals1.sb = new StringBuilder(8);
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(kwpecu.GetTestECUExistsRequest());
						Dictionary<string, string>.Enumerator enumerator = kwpecu.IdentsASCII.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair = enumerator.Current;
								KWPECU.<>c__DisplayClass96_1 CS$<>8__locals2 = new KWPECU.<>c__DisplayClass96_1();
								CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
								string key = keyValuePair.Key;
								CS$<>8__locals2.requestTitle = keyValuePair.Value;
								OBDRequest requestForCommand = kwpecu.GetRequestForCommand(key);
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
						enumerator = kwpecu.IdentsHEX.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair2 = enumerator.Current;
								KWPECU.<>c__DisplayClass96_2 CS$<>8__locals3 = new KWPECU.<>c__DisplayClass96_2();
								CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
								string key2 = keyValuePair2.Key;
								CS$<>8__locals3.requestTitle = keyValuePair2.Value;
								OBDRequest requestForCommand2 = kwpecu.GetRequestForCommand(key2);
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
								obdrequest.ELMFormat = ELMFormat.CAN11bit;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, KWPECU.<GetECUInformationReportAsync>d__96>(ref taskAwaiter, ref this);
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

			// Token: 0x06003151 RID: 12625 RVA: 0x0021F42C File Offset: 0x0021D62C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C84 RID: 7300
			public int <>1__state;

			// Token: 0x04001C85 RID: 7301
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001C86 RID: 7302
			public KWPECU <>4__this;

			// Token: 0x04001C87 RID: 7303
			public CancellationToken token;

			// Token: 0x04001C88 RID: 7304
			private KWPECU.<>c__DisplayClass96_0 <>8__1;

			// Token: 0x04001C89 RID: 7305
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020004F7 RID: 1271
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadDTCAsync>d__100 : IAsyncStateMachine
		{
			// Token: 0x06003152 RID: 12626 RVA: 0x0021F43C File Offset: 0x0021D63C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				KWPECU kwpecu = this;
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
							goto IL_0148;
						}
						kwpecu.cancellationToken = token;
						OBDRequest[] array = kwpecu.GetDTCReadRequests();
						if (kwpecu.TestELMDevice)
						{
							OBDRequest atshtestRequest = kwpecu.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
							array = new OBDRequest[] { atshtestRequest }.Concat(array).ToArray<OBDRequest>();
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, KWPECU.<ReadDTCAsync>d__100>(ref taskAwaiter, ref this);
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
						goto IL_014F;
					}
					OBDRequest pingRequest = App.OBDReader.GetPingRequest();
					App.OBDReader.ReplaceQueue(pingRequest);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, KWPECU.<ReadDTCAsync>d__100>(ref taskAwaiter, ref this);
						return;
					}
					IL_0148:
					taskAwaiter.GetResult();
					IL_014F:
					kwpecu.cancellationToken = CancellationToken.None;
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

			// Token: 0x06003153 RID: 12627 RVA: 0x0021F5F0 File Offset: 0x0021D7F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C8A RID: 7306
			public int <>1__state;

			// Token: 0x04001C8B RID: 7307
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001C8C RID: 7308
			public KWPECU <>4__this;

			// Token: 0x04001C8D RID: 7309
			public CancellationToken token;

			// Token: 0x04001C8E RID: 7310
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001C8F RID: 7311
			public Action badELMDetectedCallback;

			// Token: 0x04001C90 RID: 7312
			private TaskAwaiter <>u__1;
		}
	}
}
