using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008D8 RID: 2264
	internal class MQBUnitBackupCreator : ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06004C91 RID: 19601 RVA: 0x003898F4 File Offset: 0x00387AF4
		public static List<ICodingContainer> BuildBackupCreators(string platform)
		{
			MQBUnitBackupCreator[] array = (from unit in (from x in PackageFileReader.GetFilesInDirectory("vag." + platform + ".backup")
					orderby x
					select x).ToArray<string>()
				select new MQBUnitBackupCreator(unit, platform)).ToArray<MQBUnitBackupCreator>();
			foreach (MQBUnitBackupCreator mqbunitBackupCreator in array)
			{
				string text = mqbunitBackupCreator.unit;
				if (text != null)
				{
					int length = text.Length;
					if (length == 2)
					{
						char c = text[1];
						switch (c)
						{
						case '0':
							if (!(text == "10"))
							{
								goto IL_01F1;
							}
							mqbunitBackupCreator.Password = "71679";
							goto IL_01F1;
						case '1':
							if (!(text == "01"))
							{
								goto IL_01F1;
							}
							mqbunitBackupCreator.Password = "27971";
							goto IL_01F1;
						case '2':
						case '4':
							goto IL_01F1;
						case '3':
							if (!(text == "03"))
							{
								goto IL_01F1;
							}
							mqbunitBackupCreator.Password = "11966";
							mqbunitBackupCreator.PasswordHint = "20103, 40168, 11966, 25004";
							goto IL_01F1;
						case '5':
							if (!(text == "A5"))
							{
								goto IL_01F1;
							}
							break;
						case '6':
							if (!(text == "16"))
							{
								goto IL_01F1;
							}
							break;
						case '7':
							if (!(text == "17"))
							{
								goto IL_01F1;
							}
							break;
						case '8':
							if (!(text == "18"))
							{
								goto IL_01F1;
							}
							mqbunitBackupCreator.Password = "80782";
							goto IL_01F1;
						case '9':
							if (!(text == "09"))
							{
								goto IL_01F1;
							}
							mqbunitBackupCreator.Password = "31347";
							goto IL_01F1;
						default:
							if (c != 'F')
							{
								goto IL_01F1;
							}
							if (!(text == "5F"))
							{
								goto IL_01F1;
							}
							break;
						}
						mqbunitBackupCreator.Password = "20103";
					}
				}
				IL_01F1:;
			}
			ICodingContainer codingContainer = MQBUnitBackupCreator.BuildMultiBackupCreator(array);
			List<ICodingContainer> list = new List<ICodingContainer>(array.Length + 1);
			list.Add(codingContainer);
			list.AddRange(array);
			return list;
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x00389B24 File Offset: 0x00387D24
		public static ICodingContainer BuildMultiBackupCreator(IEnumerable<ICodingContainer> unitBackupCreators)
		{
			MQBMultipleCoding mqbmultipleCoding = new MQBMultipleCoding(CodingGroup.BackupCreator, "All units backup", "Creates full backup of all long coding and all adaptation values. Takes a very long time!", true, unitBackupCreators.ToArray<ICodingContainer>());
			mqbmultipleCoding.ShouldUpdateInternalNames = false;
			mqbmultipleCoding.HasCurrentState = false;
			mqbmultipleCoding.Options.Clear();
			mqbmultipleCoding.Options.Add(new MQBAdaptationOption(Translate.GetString("coding_Start"), "00"));
			mqbmultipleCoding.Translations.Add(new TranslationItem("ru", "Резервная копия всех блоков", "Создается резервная копия длинного кодирования и всех адаптаций всех блоков. Занимает очень много времени!", ""));
			return mqbmultipleCoding;
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x00389BA8 File Offset: 0x00387DA8
		public MQBUnitBackupCreator(string unit, string platform)
		{
			this.unit = unit;
			this.requestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit(unit);
			this.responseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit(unit);
			this.Name = string.Format(Translate.GetString("coding_BackupUnit"), unit);
			this.InnerDescription = Translate.GetString("coding_BackupUnit_InnerDescription");
			string text = PackageFileReader.ReadFileToString("vag." + platform + ".backup." + unit);
			this.commands = text.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
			this.commands = this.commands.Distinct<string>().ToArray<string>();
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x00389C58 File Offset: 0x00387E58
		public MQBUnitBackupCreator(string unit, string[] commands)
		{
			this.commands = commands;
			this.unit = unit;
			this.requestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit(unit);
			this.responseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit(unit);
			this.Name = string.Format(Translate.GetString("coding_BackupUnit"), unit);
			this.InnerDescription = Translate.GetString("coding_BackupUnit_InnerDescription");
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x06004C95 RID: 19605 RVA: 0x0033BB94 File Offset: 0x00339D94
		// (set) Token: 0x06004C96 RID: 19606 RVA: 0x000027D4 File Offset: 0x000009D4
		public CodingGroup Group
		{
			get
			{
				return CodingGroup.BackupCreator;
			}
			set
			{
			}
		}

		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x00389CC2 File Offset: 0x00387EC2
		// (set) Token: 0x06004C98 RID: 19608 RVA: 0x00389CCA File Offset: 0x00387ECA
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

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x06004C99 RID: 19609 RVA: 0x00389CD3 File Offset: 0x00387ED3
		// (set) Token: 0x06004C9A RID: 19610 RVA: 0x00389CDB File Offset: 0x00387EDB
		public string Description
		{
			[CompilerGenerated]
			get
			{
				return this.<Description>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Description>k__BackingField = value;
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x06004C9B RID: 19611 RVA: 0x00389CE4 File Offset: 0x00387EE4
		// (set) Token: 0x06004C9C RID: 19612 RVA: 0x00389CEC File Offset: 0x00387EEC
		public string InnerDescription
		{
			[CompilerGenerated]
			get
			{
				return this.<InnerDescription>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InnerDescription>k__BackingField = value;
			}
		}

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x06004C9D RID: 19613 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string CurrentState
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public bool PasswordVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x06004C9F RID: 19615 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06004CA0 RID: 19616 RVA: 0x000027D4 File Offset: 0x000009D4
		public bool HasCurrentState
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x06004CA1 RID: 19617 RVA: 0x00389CF5 File Offset: 0x00387EF5
		// (set) Token: 0x06004CA2 RID: 19618 RVA: 0x00389CFD File Offset: 0x00387EFD
		public virtual string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				this.OnPropertyChanged("Password");
			}
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x00389D11 File Offset: 0x00387F11
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x00389D2A File Offset: 0x00387F2A
		// (set) Token: 0x06004CA5 RID: 19621 RVA: 0x00389D32 File Offset: 0x00387F32
		public string PasswordHint
		{
			[CompilerGenerated]
			get
			{
				return this.<PasswordHint>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PasswordHint>k__BackingField = value;
			}
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x06004CA6 RID: 19622 RVA: 0x00389D3B File Offset: 0x00387F3B
		public ObservableCollection<MQBAdaptationOption> Options
		{
			get
			{
				return new ObservableCollection<MQBAdaptationOption>
				{
					new MQBAdaptationOption(Translate.GetString("coding_Start"), "00")
				};
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x06004CA7 RID: 19623 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06004CA8 RID: 19624 RVA: 0x000027D4 File Offset: 0x000009D4
		public bool RequiresPro
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x00002076 File Offset: 0x00000276
		public AdaptationValueTypes ValueType
		{
			get
			{
				return AdaptationValueTypes.OptionType;
			}
		}

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06004CAA RID: 19626 RVA: 0x00389D5C File Offset: 0x00387F5C
		// (remove) Token: 0x06004CAB RID: 19627 RVA: 0x00389D94 File Offset: 0x00387F94
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

		// Token: 0x06004CAC RID: 19628 RVA: 0x00389DCC File Offset: 0x00387FCC
		public async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			List<CodingLogItem> logItems = new List<CodingLogItem>(this.commands.Length);
			List<OBDRequest> list = this.BuildRequests(progress, logItems);
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			CodingLogItem.RecordToLog(logItems);
			return CodingRequestResult.Success;
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x00389E18 File Offset: 0x00388018
		private List<OBDRequest> BuildRequests(IProgress<string> progress, List<CodingLogItem> logItems)
		{
			List<OBDRequest> list = new List<OBDRequest>(this.commands.Length);
			string text = string.Concat(new string[] { "ATFCSH", this.requestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", this.responseHeader, ";1003" });
			string text2 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0";
			string text3 = this.requestHeader;
			if (this.requestHeader.Length == 8)
			{
				text3 = this.requestHeader.Substring(2);
				text = string.Concat(new string[]
				{
					"ATSP7;ATCP",
					this.requestHeader.Substring(0, 2),
					";ATFCSH",
					this.requestHeader,
					";ATFCSD300000;ATFCSM1;ATAL;ATCRA",
					this.responseHeader,
					";1003"
				});
				text2 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0";
			}
			if (string.IsNullOrEmpty(this.Password))
			{
				List<OBDRequest> requestsFromStringCommands = MQBParametrizeBase.GetRequestsFromStringCommands("2703;2704", this.Password, text3, text, text2);
				list.AddRange(requestsFromStringCommands);
			}
			for (int i = 0; i < this.commands.Length; i++)
			{
				try
				{
					int count = i;
					OBDRequest obdrequest = new OBDRequest(this.commands[i], text3, text, text2, false);
					string id = obdrequest.Command.Substring(2);
					string vin = "";
					if (CarInfoViewModel.Instance != null && CarInfoViewModel.Instance.IsVINAvailable && !string.IsNullOrEmpty(CarInfoViewModel.Instance.VIN))
					{
						vin = CarInfoViewModel.Instance.VIN;
					}
					obdrequest.ResponseReceived += delegate(OBDRequest req2, string response)
					{
						IProgress<string> progress2 = progress;
						if (progress2 == null)
						{
							return;
						}
						progress2.Report(string.Format("{0} / {1}", count + 1, this.commands.Length));
					};
					obdrequest.CheckLength = true;
					obdrequest.ELMFormat = ELMFormat.CAN11bit;
					obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string respHeader)
					{
						if (data != null && data.Length != 0)
						{
							string text4 = BitHelpers.ByteArrayToHexString(data);
							CodingLogItem codingLogItem = new CodingLogItem(CodingLogItem.CodingTypes.MQB, DateTimeNowHelper.NowSafe.Ticks, this.Name + " $" + id, "BACKUP", id, text4, "", this.Password, this.requestHeader, this.responseHeader, vin, "", "", "", "96", "");
							if (this.requestHeader.Length == 8)
							{
								codingLogItem = new CodingLogItem(CodingLogItem.CodingTypes.CustomizableCodingTemplate, DateTimeNowHelper.NowSafe.Ticks, this.Name + " $" + id, "BACKUP", "22" + id, text4, "", this.Password, this.requestHeader, this.responseHeader, vin, "1003", string.IsNullOrEmpty(this.Password) ? "22F199;22F1A5;22F1A0;22F1A1;2EF199;2EF198;2EF1A0;2EF1A1" : "22F199;22F1A5;22F1A0;22F1A1;2EF199;2EF198;2EF1A0;2EF1A1;2703;2704", "", "32", string.IsNullOrEmpty(this.Password) ? "22F199;22F1A5;22F1A0;22F1A1;2EF199;2EF198;2EF1A0;2EF1A1" : "22F199;22F1A5;22F1A0;22F1A1;2EF199;2EF198;2EF1A0;2EF1A1;2703;2704");
								codingLogItem.Values["Protocol"] = "7";
							}
							logItems.Add(codingLogItem);
						}
					};
					list.Add(obdrequest);
				}
				catch (Exception)
				{
				}
			}
			return list;
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x0038A024 File Offset: 0x00388224
		public async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			string text = "ATFCSH" + this.requestHeader + ";ATFCSD300000;ATFCSM1;ATAL;ATCRA" + this.responseHeader;
			string text2 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0";
			OBDRequest obdrequest = new OBDRequest("1003", this.requestHeader, text, text2, false);
			obdrequest.ResponseMarker = "50";
			bool unitExists = false;
			obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					unitExists = true;
				}
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
			await App.OBDReader.WaitForCommandQueue();
			CodingRequestResult codingRequestResult;
			if (unitExists)
			{
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			return codingRequestResult;
		}

		// Token: 0x04002D34 RID: 11572
		private string[] commands;

		// Token: 0x04002D35 RID: 11573
		private string unit;

		// Token: 0x04002D36 RID: 11574
		private string requestHeader;

		// Token: 0x04002D37 RID: 11575
		private string responseHeader;

		// Token: 0x04002D38 RID: 11576
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04002D39 RID: 11577
		[CompilerGenerated]
		private string <Description>k__BackingField;

		// Token: 0x04002D3A RID: 11578
		[CompilerGenerated]
		private string <InnerDescription>k__BackingField;

		// Token: 0x04002D3B RID: 11579
		private string _Password = "";

		// Token: 0x04002D3C RID: 11580
		[CompilerGenerated]
		private string <PasswordHint>k__BackingField;

		// Token: 0x04002D3D RID: 11581
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x020008D9 RID: 2265
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004CAF RID: 19631 RVA: 0x0038A067 File Offset: 0x00388267
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004CB0 RID: 19632 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004CB1 RID: 19633 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <BuildBackupCreators>b__0_0(string x)
			{
				return x;
			}

			// Token: 0x04002D3E RID: 11582
			public static readonly MQBUnitBackupCreator.<>c <>9 = new MQBUnitBackupCreator.<>c();

			// Token: 0x04002D3F RID: 11583
			public static Func<string, string> <>9__0_0;
		}

		// Token: 0x020008DA RID: 2266
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06004CB2 RID: 19634 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06004CB3 RID: 19635 RVA: 0x0038A073 File Offset: 0x00388273
			internal MQBUnitBackupCreator <BuildBackupCreators>b__1(string unit)
			{
				return new MQBUnitBackupCreator(unit, this.platform);
			}

			// Token: 0x04002D40 RID: 11584
			public string platform;
		}

		// Token: 0x020008DB RID: 2267
		[CompilerGenerated]
		private sealed class <>c__DisplayClass50_0
		{
			// Token: 0x06004CB4 RID: 19636 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass50_0()
			{
			}

			// Token: 0x04002D41 RID: 11585
			public IProgress<string> progress;

			// Token: 0x04002D42 RID: 11586
			public MQBUnitBackupCreator <>4__this;

			// Token: 0x04002D43 RID: 11587
			public List<CodingLogItem> logItems;
		}

		// Token: 0x020008DC RID: 2268
		[CompilerGenerated]
		private sealed class <>c__DisplayClass50_1
		{
			// Token: 0x06004CB5 RID: 19637 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass50_1()
			{
			}

			// Token: 0x06004CB6 RID: 19638 RVA: 0x0038A084 File Offset: 0x00388284
			internal void <BuildRequests>b__0(OBDRequest req2, string response)
			{
				IProgress<string> progress = this.CS$<>8__locals1.progress;
				if (progress == null)
				{
					return;
				}
				progress.Report(string.Format("{0} / {1}", this.count + 1, this.CS$<>8__locals1.<>4__this.commands.Length));
			}

			// Token: 0x06004CB7 RID: 19639 RVA: 0x0038A0D4 File Offset: 0x003882D4
			internal void <BuildRequests>b__1(OBDRequest request, byte[] data, bool decodeResult, string respHeader)
			{
				if (data != null && data.Length != 0)
				{
					string text = BitHelpers.ByteArrayToHexString(data);
					CodingLogItem codingLogItem = new CodingLogItem(CodingLogItem.CodingTypes.MQB, DateTimeNowHelper.NowSafe.Ticks, this.CS$<>8__locals1.<>4__this.Name + " $" + this.id, "BACKUP", this.id, text, "", this.CS$<>8__locals1.<>4__this.Password, this.CS$<>8__locals1.<>4__this.requestHeader, this.CS$<>8__locals1.<>4__this.responseHeader, this.vin, "", "", "", "96", "");
					if (this.CS$<>8__locals1.<>4__this.requestHeader.Length == 8)
					{
						codingLogItem = new CodingLogItem(CodingLogItem.CodingTypes.CustomizableCodingTemplate, DateTimeNowHelper.NowSafe.Ticks, this.CS$<>8__locals1.<>4__this.Name + " $" + this.id, "BACKUP", "22" + this.id, text, "", this.CS$<>8__locals1.<>4__this.Password, this.CS$<>8__locals1.<>4__this.requestHeader, this.CS$<>8__locals1.<>4__this.responseHeader, this.vin, "1003", string.IsNullOrEmpty(this.CS$<>8__locals1.<>4__this.Password) ? "22F199;22F1A5;22F1A0;22F1A1;2EF199;2EF198;2EF1A0;2EF1A1" : "22F199;22F1A5;22F1A0;22F1A1;2EF199;2EF198;2EF1A0;2EF1A1;2703;2704", "", "32", string.IsNullOrEmpty(this.CS$<>8__locals1.<>4__this.Password) ? "22F199;22F1A5;22F1A0;22F1A1;2EF199;2EF198;2EF1A0;2EF1A1" : "22F199;22F1A5;22F1A0;22F1A1;2EF199;2EF198;2EF1A0;2EF1A1;2703;2704");
						codingLogItem.Values["Protocol"] = "7";
					}
					this.CS$<>8__locals1.logItems.Add(codingLogItem);
				}
			}

			// Token: 0x04002D44 RID: 11588
			public int count;

			// Token: 0x04002D45 RID: 11589
			public string id;

			// Token: 0x04002D46 RID: 11590
			public string vin;

			// Token: 0x04002D47 RID: 11591
			public MQBUnitBackupCreator.<>c__DisplayClass50_0 CS$<>8__locals1;
		}

		// Token: 0x020008DD RID: 2269
		[CompilerGenerated]
		private sealed class <>c__DisplayClass51_0
		{
			// Token: 0x06004CB8 RID: 19640 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass51_0()
			{
			}

			// Token: 0x06004CB9 RID: 19641 RVA: 0x0038A2A4 File Offset: 0x003884A4
			internal void <UpdateCurrentState>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.unitExists = true;
				}
			}

			// Token: 0x04002D48 RID: 11592
			public bool unitExists;
		}

		// Token: 0x020008DE RID: 2270
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__49 : IAsyncStateMachine
		{
			// Token: 0x06004CBA RID: 19642 RVA: 0x0038A2B4 File Offset: 0x003884B4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBUnitBackupCreator mqbunitBackupCreator = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						logItems = new List<CodingLogItem>(mqbunitBackupCreator.commands.Length);
						List<OBDRequest> list = mqbunitBackupCreator.BuildRequests(progress, logItems);
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBUnitBackupCreator.<Execute>d__49>(ref taskAwaiter, ref this);
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
					CodingLogItem.RecordToLog(logItems);
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					num2 = -2;
					logItems = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				logItems = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004CBB RID: 19643 RVA: 0x0038A3BC File Offset: 0x003885BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D49 RID: 11593
			public int <>1__state;

			// Token: 0x04002D4A RID: 11594
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002D4B RID: 11595
			public MQBUnitBackupCreator <>4__this;

			// Token: 0x04002D4C RID: 11596
			public IProgress<string> progress;

			// Token: 0x04002D4D RID: 11597
			private List<CodingLogItem> <logItems>5__2;

			// Token: 0x04002D4E RID: 11598
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008DF RID: 2271
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__51 : IAsyncStateMachine
		{
			// Token: 0x06004CBC RID: 19644 RVA: 0x0038A3CC File Offset: 0x003885CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBUnitBackupCreator mqbunitBackupCreator = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBUnitBackupCreator.<>c__DisplayClass51_0();
						string text = "ATFCSH" + mqbunitBackupCreator.requestHeader + ";ATFCSD300000;ATFCSM1;ATAL;ATCRA" + mqbunitBackupCreator.responseHeader;
						string text2 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0";
						OBDRequest obdrequest = new OBDRequest("1003", mqbunitBackupCreator.requestHeader, text, text2, false);
						obdrequest.ResponseMarker = "50";
						CS$<>8__locals1.unitExists = false;
						obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								CS$<>8__locals1.unitExists = true;
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBUnitBackupCreator.<UpdateCurrentState>d__51>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.unitExists)
					{
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						codingRequestResult = CodingRequestResult.NotSupported;
					}
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
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004CBD RID: 19645 RVA: 0x0038A544 File Offset: 0x00388744
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D4F RID: 11599
			public int <>1__state;

			// Token: 0x04002D50 RID: 11600
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002D51 RID: 11601
			public MQBUnitBackupCreator <>4__this;

			// Token: 0x04002D52 RID: 11602
			private MQBUnitBackupCreator.<>c__DisplayClass51_0 <>8__1;

			// Token: 0x04002D53 RID: 11603
			private TaskAwaiter <>u__1;
		}
	}
}
