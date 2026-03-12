using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008BC RID: 2236
	internal class MQBParametrizeBase : ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06004BBB RID: 19387 RVA: 0x0038534A File Offset: 0x0038354A
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06004BBC RID: 19388 RVA: 0x00385364 File Offset: 0x00383564
		// (remove) Token: 0x06004BBD RID: 19389 RVA: 0x0038539C File Offset: 0x0038359C
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

		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x06004BBE RID: 19390 RVA: 0x003853D1 File Offset: 0x003835D1
		// (set) Token: 0x06004BBF RID: 19391 RVA: 0x003853D9 File Offset: 0x003835D9
		public CodingGroup Group
		{
			get
			{
				return this._Group;
			}
			set
			{
				this._Group = value;
				this.OnPropertyChanged("Group");
			}
		}

		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x06004BC0 RID: 19392 RVA: 0x003853F0 File Offset: 0x003835F0
		// (set) Token: 0x06004BC1 RID: 19393 RVA: 0x00385438 File Offset: 0x00383638
		public string Name
		{
			get
			{
				TranslationItem translationItem = this.Translations.FirstOrDefault((TranslationItem x) => x.Language == App.CurrentLanguageCode);
				if (translationItem != null)
				{
					return translationItem.Name;
				}
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.OnPropertyChanged("Name");
			}
		}

		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x0038544C File Offset: 0x0038364C
		// (set) Token: 0x06004BC3 RID: 19395 RVA: 0x00385494 File Offset: 0x00383694
		public string Description
		{
			get
			{
				TranslationItem translationItem = this.Translations.FirstOrDefault((TranslationItem x) => x.Language == App.CurrentLanguageCode);
				if (translationItem != null)
				{
					return translationItem.ShortName;
				}
				return this._Description;
			}
			set
			{
				this._Description = value;
				this.OnPropertyChanged("Description");
			}
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x06004BC4 RID: 19396 RVA: 0x003854A8 File Offset: 0x003836A8
		// (set) Token: 0x06004BC5 RID: 19397 RVA: 0x003854F0 File Offset: 0x003836F0
		public string InnerDescription
		{
			get
			{
				TranslationItem translationItem = this.Translations.FirstOrDefault((TranslationItem x) => x.Language == App.CurrentLanguageCode);
				if (translationItem != null)
				{
					return translationItem.AdditionalText;
				}
				return this._InnerDescription;
			}
			set
			{
				this._InnerDescription = value;
				this.OnPropertyChanged("InnerDescription");
			}
		}

		// Token: 0x17001715 RID: 5909
		// (get) Token: 0x06004BC6 RID: 19398 RVA: 0x00385504 File Offset: 0x00383704
		// (set) Token: 0x06004BC7 RID: 19399 RVA: 0x0038550C File Offset: 0x0038370C
		public string CurrentState
		{
			get
			{
				return this._CurrentState;
			}
			set
			{
				this._CurrentState = value;
				this.OnPropertyChanged("CurrentState");
			}
		}

		// Token: 0x17001716 RID: 5910
		// (get) Token: 0x06004BC8 RID: 19400 RVA: 0x00385520 File Offset: 0x00383720
		// (set) Token: 0x06004BC9 RID: 19401 RVA: 0x00385528 File Offset: 0x00383728
		public virtual bool PasswordVisible
		{
			[CompilerGenerated]
			get
			{
				return this.<PasswordVisible>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PasswordVisible>k__BackingField = value;
			}
		} = true;

		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x06004BCA RID: 19402 RVA: 0x00385531 File Offset: 0x00383731
		// (set) Token: 0x06004BCB RID: 19403 RVA: 0x00385539 File Offset: 0x00383739
		public virtual bool HasCurrentState
		{
			get
			{
				return this._HasCurrentState;
			}
			set
			{
				this._HasCurrentState = value;
				this.OnPropertyChanged("HasCurrentState");
			}
		}

		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x06004BCC RID: 19404 RVA: 0x0038554D File Offset: 0x0038374D
		// (set) Token: 0x06004BCD RID: 19405 RVA: 0x00385555 File Offset: 0x00383755
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

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x06004BCE RID: 19406 RVA: 0x00385569 File Offset: 0x00383769
		// (set) Token: 0x06004BCF RID: 19407 RVA: 0x00385571 File Offset: 0x00383771
		public string PasswordHint
		{
			get
			{
				return this._PasswordHint;
			}
			set
			{
				this._PasswordHint = value;
				this.OnPropertyChanged("PasswordHint");
			}
		}

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x06004BD0 RID: 19408 RVA: 0x00385585 File Offset: 0x00383785
		// (set) Token: 0x06004BD1 RID: 19409 RVA: 0x0038558D File Offset: 0x0038378D
		public ObservableCollection<MQBAdaptationOption> Options
		{
			[CompilerGenerated]
			get
			{
				return this.<Options>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Options>k__BackingField = value;
			}
		} = new ObservableCollection<MQBAdaptationOption>();

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x00385596 File Offset: 0x00383796
		// (set) Token: 0x06004BD3 RID: 19411 RVA: 0x0038559E File Offset: 0x0038379E
		public ObservableCollection<TranslationItem> Translations
		{
			[CompilerGenerated]
			get
			{
				return this.<Translations>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Translations>k__BackingField = value;
			}
		} = new ObservableCollection<TranslationItem>();

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x06004BD4 RID: 19412 RVA: 0x003855A7 File Offset: 0x003837A7
		// (set) Token: 0x06004BD5 RID: 19413 RVA: 0x003855AF File Offset: 0x003837AF
		public virtual bool RequiresPro
		{
			get
			{
				return this._RequiresPro;
			}
			set
			{
				this._RequiresPro = value;
				this.OnPropertyChanged("RequiresPro");
			}
		}

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x06004BD6 RID: 19414 RVA: 0x003855C3 File Offset: 0x003837C3
		// (set) Token: 0x06004BD7 RID: 19415 RVA: 0x003855CB File Offset: 0x003837CB
		public AdaptationValueTypes ValueType
		{
			get
			{
				return this._ValueType;
			}
			set
			{
				this._ValueType = value;
				this.OnPropertyChanged("ValueType");
			}
		}

		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x003855DF File Offset: 0x003837DF
		// (set) Token: 0x06004BD9 RID: 19417 RVA: 0x003855E7 File Offset: 0x003837E7
		public string RequestHeader
		{
			get
			{
				return this._RequestHeader;
			}
			set
			{
				this._RequestHeader = value;
				this.OnPropertyChanged("RequestHeader");
			}
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x003855FB File Offset: 0x003837FB
		// (set) Token: 0x06004BDB RID: 19419 RVA: 0x00385603 File Offset: 0x00383803
		public string ResponseHeader
		{
			get
			{
				return this._ResponseHeader;
			}
			set
			{
				this._ResponseHeader = value;
				this.OnPropertyChanged("ResponseHeader");
			}
		}

		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x00385617 File Offset: 0x00383817
		// (set) Token: 0x06004BDD RID: 19421 RVA: 0x0038561F File Offset: 0x0038381F
		protected virtual string BeforeCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<BeforeCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BeforeCommands>k__BackingField = value;
			}
		}

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x00385628 File Offset: 0x00383828
		// (set) Token: 0x06004BDF RID: 19423 RVA: 0x00385630 File Offset: 0x00383830
		protected virtual string AfterCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<AfterCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AfterCommands>k__BackingField = value;
			}
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0038563C File Offset: 0x0038383C
		protected virtual void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = string.Concat(new string[] { "ATFCSH", this.RequestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", this.ResponseHeader, ";ATST", this.ATST });
			this.AfterCommands = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF";
		}

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x06004BE1 RID: 19425 RVA: 0x00385698 File Offset: 0x00383898
		// (set) Token: 0x06004BE2 RID: 19426 RVA: 0x003856A0 File Offset: 0x003838A0
		public string PreReadCommands
		{
			get
			{
				return this._PreReadCommands;
			}
			set
			{
				this._PreReadCommands = value;
				this.OnPropertyChanged("_PreReadCommands");
			}
		}

		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x06004BE3 RID: 19427 RVA: 0x003856B4 File Offset: 0x003838B4
		// (set) Token: 0x06004BE4 RID: 19428 RVA: 0x003856BC File Offset: 0x003838BC
		public string PreWriteCommands
		{
			get
			{
				return this._PreWriteCommands;
			}
			set
			{
				this._PreWriteCommands = value;
				this.OnPropertyChanged("PreWriteCommands");
			}
		}

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x06004BE5 RID: 19429 RVA: 0x003856D0 File Offset: 0x003838D0
		// (set) Token: 0x06004BE6 RID: 19430 RVA: 0x003856D8 File Offset: 0x003838D8
		public string PostWriteCommands
		{
			get
			{
				return this._PostWriteCommands;
			}
			set
			{
				this._PostWriteCommands = value;
				this.OnPropertyChanged("PostWriteCommands");
			}
		}

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x003856EC File Offset: 0x003838EC
		// (set) Token: 0x06004BE8 RID: 19432 RVA: 0x003856F4 File Offset: 0x003838F4
		public int AddressFormatLength
		{
			get
			{
				return this._AddressFormatLength;
			}
			set
			{
				this._AddressFormatLength = value;
				this.OnPropertyChanged("AddressFormatLength");
			}
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x00385708 File Offset: 0x00383908
		// (set) Token: 0x06004BEA RID: 19434 RVA: 0x00385710 File Offset: 0x00383910
		public int DataLengthFormatLength
		{
			get
			{
				return this._DataLengthFormatLength;
			}
			set
			{
				this._DataLengthFormatLength = value;
				this.OnPropertyChanged("DataLengthFormatLength");
			}
		}

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x06004BEB RID: 19435 RVA: 0x00385724 File Offset: 0x00383924
		// (set) Token: 0x06004BEC RID: 19436 RVA: 0x0038572C File Offset: 0x0038392C
		public virtual string ATST
		{
			get
			{
				return this._ATST;
			}
			set
			{
				this._ATST = value;
				this.OnPropertyChanged("ATST");
			}
		}

		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x06004BED RID: 19437 RVA: 0x00385740 File Offset: 0x00383940
		// (set) Token: 0x06004BEE RID: 19438 RVA: 0x00385748 File Offset: 0x00383948
		public int Address
		{
			get
			{
				return this._Address;
			}
			set
			{
				this._Address = value;
				this.OnPropertyChanged("Address");
			}
		}

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x06004BEF RID: 19439 RVA: 0x0038575C File Offset: 0x0038395C
		// (set) Token: 0x06004BF0 RID: 19440 RVA: 0x00385764 File Offset: 0x00383964
		public int DataLength
		{
			get
			{
				return this._DataLength;
			}
			set
			{
				this._DataLength = value;
				this.OnPropertyChanged("DataLength");
			}
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x06004BF1 RID: 19441 RVA: 0x00385778 File Offset: 0x00383978
		public bool DeveloperMode
		{
			get
			{
				return SharedSettings.Current.DeveloperMode;
			}
		}

		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x00385784 File Offset: 0x00383984
		// (set) Token: 0x06004BF3 RID: 19443 RVA: 0x0038578C File Offset: 0x0038398C
		public string LogComment
		{
			[CompilerGenerated]
			get
			{
				return this.<LogComment>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LogComment>k__BackingField = value;
			}
		} = "";

		// Token: 0x06004BF4 RID: 19444 RVA: 0x00385798 File Offset: 0x00383998
		public virtual async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			if (originalData == null && !(this.RequestHeader == VagUnitHelper.GetRequestHeaderForMQBUnit("19")) && !(this.RequestHeader == VagUnitHelper.GetRequestHeaderForMQBUnit("65")))
			{
				CodingRequestResult codingRequestResult = await this.UpdateCurrentState(password, progress);
				if (codingRequestResult != CodingRequestResult.Success)
				{
					return codingRequestResult;
				}
				originalData = BitHelpers.ConvertHexToBytesX(this.CurrentState);
			}
			CodingRequestResult codingRequestResult2;
			if (skipIfTheSameData && originalData != null && ArrayHelpers.ArrayEquals<byte>(BitHelpers.ConvertHexToBytesX(value), originalData))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("AddressFormatLength", this.AddressFormatLength.ToString("X2"));
				dictionary.Add("DataLengthFormatLength", this.DataLengthFormatLength.ToString("X2"));
				dictionary.Add("DataLength", this.DataLength.ToString("X2"));
				CodingLogItem.RecordToLog(this.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQBParametrizeBase, this.Address.ToString("X2"), BitHelpers.ByteArrayToHexString(originalData), value, password, this.RequestHeader, this.ResponseHeader, this.PreReadCommands, this.PreWriteCommands, this.PostWriteCommands, this.ATST, dictionary, "");
				codingRequestResult2 = CodingRequestResult.Success;
			}
			else
			{
				codingRequestResult2 = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, value);
			}
			return codingRequestResult2;
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x00385810 File Offset: 0x00383A10
		public virtual async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			if (string.IsNullOrEmpty(password))
			{
				password = this.Password;
			}
			if (this.LogComment != null)
			{
				await App.OBDReader.DebugWrite("\nLogComment\n");
			}
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password, progress);
			CodingRequestResult item = tuple.Item2;
			byte[] item2 = tuple.Item1;
			CodingRequestResult codingRequestResult;
			if (item != CodingRequestResult.Success)
			{
				this.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
				codingRequestResult = item;
			}
			else
			{
				this.CurrentState = BitHelpers.ByteArrayToHexString(item2);
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x00385863 File Offset: 0x00383A63
		protected virtual bool CheckResponseForNullOrNoData(string response)
		{
			return response == null || response.Contains("NO DATA");
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x00385878 File Offset: 0x00383A78
		public virtual async Task<Tuple<byte[], CodingRequestResult>> GetCurrentStateRawData(string password, IProgress<string> progress)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			List<OBDRequest> list = new List<OBDRequest>();
			list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(this.PreReadCommands, password, this.RequestHeader, this.BeforeCommands, this.AfterCommands));
			string text = this.Address.ToString("X" + (this.AddressFormatLength * 2).ToString());
			string text2 = this.DataLength.ToString("X" + (this.DataLengthFormatLength * 2).ToString());
			string text3 = string.Concat(new string[]
			{
				"3500",
				(this.DataLengthFormatLength & 15).ToString("X1"),
				(this.AddressFormatLength & 15).ToString("X1"),
				text,
				text2
			});
			List<byte> resultBytes = new List<byte>(this.DataLength);
			int bytesReceived = 0;
			int counter = 1;
			OBDRequest obdrequest = new OBDRequest(text3, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseMarker = "75";
			Action<string> <>9__3;
			obdrequest.ResponseReceived += delegate(OBDRequest req, string initiateReadRequestData)
			{
				if (initiateReadRequestData != null && (initiateReadRequestData.Contains("75") || initiateReadRequestData.Contains("037F3578")))
				{
					OBDRequest getDataRequest = new OBDRequest("36" + counter.ToString("X2"), this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
					getDataRequest.ResponseMarker = "76" + counter.ToString("X2");
					getDataRequest.CheckLength = true;
					getDataRequest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
					getDataRequest.ResponseReceived += delegate(OBDRequest getDataRequest3, string getDataRequestResponse)
					{
					};
					getDataRequest.ResponseDecoded += delegate(OBDRequest getDataRequest2, byte[] getDataRequestData, bool decodeResult, string responseHeader)
					{
						if (getDataRequestData != null)
						{
							resultBytes.AddRange(getDataRequestData);
							bytesReceived += getDataRequestData.Length;
							int counter2 = counter;
							counter = counter2 + 1;
							string text4 = string.Format("{0}/{1}", bytesReceived, this.DataLength);
							IProgress<string> progress2 = progress;
							if (progress2 != null)
							{
								progress2.Report(text4);
							}
							if (bytesReceived < this.DataLength)
							{
								getDataRequest.FailCounter = 0;
								getDataRequest.Payload = "";
								getDataRequest.SkippedCycles = 0;
								getDataRequest.Command = "36" + counter.ToString("X2");
								getDataRequest.CheckLength = true;
								getDataRequest.ResponseMarker = "76" + counter.ToString("X2");
								App.OBDReader.InsertRequestInQueue(getDataRequest);
								return;
							}
							requestResult = CodingRequestResult.Success;
						}
					};
					OBDRequest getDataRequest4 = getDataRequest;
					Action<string> action;
					if ((action = <>9__3) == null)
					{
						action = (<>9__3 = delegate(string s)
						{
							string text5 = string.Format("{0}/{1}\n{2}", bytesReceived, this.DataLength, s);
							IProgress<string> progress3 = progress;
							if (progress3 == null)
							{
								return;
							}
							progress3.Report(text5);
						});
					}
					getDataRequest4.Progress = new Progress<string>(action);
					App.OBDReader.InsertRequestInQueue(getDataRequest);
				}
			};
			OBDRequest obdrequest2 = new OBDRequest("37", this.RequestHeader, this.BeforeCommands + ";ATAT0;ATSTFF", this.AfterCommands + string.Format(";ATAT{0};ATST{1}", SharedSettings.Current.AdaptiveTimings, this.ATST), false);
			list.Add(obdrequest);
			list.Add(obdrequest2);
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			return new Tuple<byte[], CodingRequestResult>(resultBytes.ToArray(), requestResult);
		}

		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x003858CB File Offset: 0x00383ACB
		// (set) Token: 0x06004BF9 RID: 19449 RVA: 0x003858D3 File Offset: 0x00383AD3
		public virtual uint DefaultMaxBlockSize
		{
			[CompilerGenerated]
			get
			{
				return this.<DefaultMaxBlockSize>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DefaultMaxBlockSize>k__BackingField = value;
			}
		} = 4095U;

		// Token: 0x06004BFA RID: 19450 RVA: 0x003858DC File Offset: 0x00383ADC
		protected virtual async Task<CodingRequestResult> WriteDataToECU(string password, string UserFriendlyValue, IProgress<string> progress, byte[] originalData, string checked_value)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			byte[] dataToWrite = BitHelpers.ConvertHexToBytesX(checked_value);
			bool writeWasStarted = false;
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			List<OBDRequest> list = new List<OBDRequest>();
			list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(this.PreWriteCommands, password, this.RequestHeader, this.BeforeCommands, this.AfterCommands));
			string text = this.Address.ToString("X" + (this.AddressFormatLength * 2).ToString());
			string text2 = this.DataLength.ToString("X" + (this.DataLengthFormatLength * 2).ToString());
			OBDRequest obdrequest = new OBDRequest(string.Concat(new string[]
			{
				"3400",
				(this.DataLengthFormatLength & 15).ToString("X1"),
				(this.AddressFormatLength & 15).ToString("X1"),
				text,
				text2
			}), this.RequestHeader, this.BeforeCommands + ";ATAT0;ATSTFF", this.AfterCommands + ";ATAT1;ATST" + this.ATST, false);
			obdrequest.ResponseMarker = "74";
			uint maxBlockSize = 0U;
			bool hasNR78initiate = false;
			obdrequest.ResponseReceived += delegate(OBDRequest initiateWriteRequest3, string response)
			{
				if (response != null && OBDDataReader.FilterHexAndNewLineOnly(response).Contains("037F3478"))
				{
					hasNR78initiate = true;
				}
			};
			ResponseReceivedDelegate <>9__5;
			obdrequest.ResponseDecoded += delegate(OBDRequest initiateWriteRequest2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null && !hasNR78initiate)
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					return;
				}
				try
				{
					byte b = data[0];
					byte[] array = new byte[data.Length - 1];
					Array.Copy(data, 1, array, 0, data.Length - 1);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					for (int i = 0; i < array.Length; i++)
					{
						maxBlockSize += (uint)array[i] * (uint)Math.Pow(256.0, (double)i);
					}
				}
				catch (Exception)
				{
					maxBlockSize = this.DefaultMaxBlockSize;
				}
				maxBlockSize -= 2U;
				if (maxBlockSize > this.DatasetUploadMaxBlockSize)
				{
					maxBlockSize = this.DatasetUploadMaxBlockSize;
				}
				List<OBDRequest> list2 = new List<OBDRequest>();
				MemoryStream memoryStream = new MemoryStream(dataToWrite);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				int num = 1;
				OBDRequest tpReq = new OBDRequest("3E80", "700", "", "", false)
				{
					DoNotDecode = true
				};
				tpReq.ResponseDecoded += this.TpReq_ResponseDecoded;
				while (memoryStream.Position < memoryStream.Length)
				{
					long position = memoryStream.Position;
					uint num2 = maxBlockSize;
					if ((ulong)num2 > (ulong)(memoryStream.Length - memoryStream.Position))
					{
						num2 = (uint)memoryStream.Length - (uint)memoryStream.Position;
					}
					byte[] array2 = new byte[num2];
					memoryStream.Read(array2, 0, array2.Length);
					long positionFinish = memoryStream.Position;
					long totalLength = memoryStream.Length;
					OBDRequest obdrequest4 = new OBDRequest("36" + num.ToString("X2") + BitHelpers.ByteArrayToHexString(array2), this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
					obdrequest4.ResponseReceived += delegate(OBDRequest stepreq2, string stepdata)
					{
						App.OBDReader.NO_DATA_Counter = 0;
						writeWasStarted = true;
						if (stepdata != null)
						{
							string text3 = OBDDataReader.FilterHexAndNewLineOnly(stepdata);
							if (text3.Contains("7F3678") && !text3.Contains("76"))
							{
								Task.Delay(500).Wait();
							}
							if ((!text3.Contains("7F3678") && text3.Contains("037F36")) || stepdata.Contains("NO DATA"))
							{
								requestResult = CodingRequestResult.UnknownError;
								List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
								queueCopy2.RemoveAll((OBDRequest x) => x.Command.StartsWith("36"));
								App.OBDReader.ReplaceQueue(queueCopy2);
							}
						}
						IProgress<string> progress3 = progress;
						if (progress3 == null)
						{
							return;
						}
						progress3.Report(Translate.GetString("coding_progress_ApplyingNewData") + string.Format("\n{0} / {1}", positionFinish, totalLength));
					};
					string progressString = Translate.GetString("coding_progress_ApplyingNewData") + string.Format("\n{0} / {1}\n", position, totalLength);
					obdrequest4.Progress = new Progress<string>(delegate(string writeProgress)
					{
						IProgress<string> progress4 = progress;
						if (progress4 == null)
						{
							return;
						}
						progress4.Report(progressString + writeProgress);
					});
					obdrequest4.Keys.Add("TesterPresent", "ATSH700;023E80;ATSH" + obdrequest4.Header);
					list2.Add(obdrequest4);
					list2.Add(tpReq);
					num++;
				}
				OBDRequest obdrequest5 = list2.LastOrDefault((OBDRequest x) => x != tpReq);
				ResponseReceivedDelegate responseReceivedDelegate;
				if ((responseReceivedDelegate = <>9__5) == null)
				{
					responseReceivedDelegate = (<>9__5 = delegate(OBDRequest lastRequest2, string lastRequestData)
					{
						App.OBDReader.NO_DATA_Counter = 0;
						if (lastRequestData != null)
						{
							string text4 = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
							if (text4.Contains("7F3678") || text4.Contains("76"))
							{
								IProgress<string> progress5 = progress;
								if (progress5 != null)
								{
									progress5.Report(Translate.GetString("coding_progress_DataAccepted"));
								}
								requestResult = CodingRequestResult.Success;
								return;
							}
							if (!text4.Contains("76") && !text4.Contains("037F3478"))
							{
								if (text4.Length >= 11 && text4.Contains("7F36"))
								{
									int num3 = text4.IndexOf("7F36");
									int num4 = int.Parse(text4.Substring(num3 + 4, 2), NumberStyles.HexNumber);
									if (num4 >= 128 || num4 == 34)
									{
										requestResult = CodingRequestResult.WrongConditions;
										return;
									}
									if (num4 == 51)
									{
										requestResult = CodingRequestResult.WrongAccessKey;
										return;
									}
									if (num4 == 49)
									{
										requestResult = CodingRequestResult.NotSupported;
										return;
									}
									requestResult = CodingRequestResult.UnknownError;
									return;
								}
								else
								{
									requestResult = CodingRequestResult.UnknownError;
								}
							}
						}
					});
				}
				obdrequest5.ResponseReceived += responseReceivedDelegate;
				List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
				list2.AddRange(queueCopy);
				IProgress<string> progress2 = progress;
				if (progress2 != null)
				{
					progress2.Report(Translate.GetString("coding_progress_ApplyingNewData"));
				}
				App.OBDReader.ReplaceQueue(list2);
			};
			obdrequest.ResponseReceived += delegate(OBDRequest initiateWriteRequest2, string response)
			{
				if (response == null || response.Contains("NO DATA") || response.Contains("ERROR"))
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					requestResult = CodingRequestResult.NoData;
					return;
				}
				string text5 = OBDDataReader.FilterHexAndNewLineOnly(response);
				if (!text5.Contains("74") && !text5.Contains("037F3478"))
				{
					if (text5.Length >= 11 && text5.Contains("7F34"))
					{
						int num5 = text5.IndexOf("7F34");
						int num6 = int.Parse(text5.Substring(num5 + 4, 2), NumberStyles.HexNumber);
						if (num6 >= 128 || num6 == 34)
						{
							requestResult = CodingRequestResult.WrongConditions;
						}
						else if (num6 == 51)
						{
							requestResult = CodingRequestResult.WrongAccessKey;
						}
						else if (num6 == 49)
						{
							requestResult = CodingRequestResult.NotSupported;
						}
						else
						{
							requestResult = CodingRequestResult.UnknownError;
						}
					}
					else
					{
						requestResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			};
			list.Add(obdrequest);
			OBDRequest obdrequest2 = new OBDRequest("37", this.RequestHeader, this.BeforeCommands + ";ATAT0;ATSTFF", this.AfterCommands + string.Format(";ATAT{0};ATST{1}", SharedSettings.Current.AdaptiveTimings, this.ATST), false);
			obdrequest2.ResponseReceived += delegate(OBDRequest finishTransferRequest2, string response)
			{
				if (response != null)
				{
					string[] array3 = OBDDataReader.FilterHexAndNewLineOnly(response).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length == 1 && array3[0].Contains("037F3778"))
					{
						Task.Delay(1000).Wait();
					}
				}
			};
			list.Add(obdrequest2);
			obdrequest2.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
			obdrequest2.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
			List<OBDRequest> requestsFromStringCommands = MQBParametrizeBase.GetRequestsFromStringCommands(this.PostWriteCommands, password, this.RequestHeader, this.BeforeCommands, this.AfterCommands);
			foreach (OBDRequest obdrequest3 in requestsFromStringCommands)
			{
				if (obdrequest3.Command.StartsWith("2E"))
				{
					obdrequest3.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
					obdrequest3.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				}
			}
			list.AddRange(requestsFromStringCommands);
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			if ((requestResult == CodingRequestResult.Success) | writeWasStarted)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("AddressFormatLength", this.AddressFormatLength.ToString("X2"));
				dictionary.Add("DataLengthFormatLength", this.DataLengthFormatLength.ToString("X2"));
				dictionary.Add("DataLength", this.DataLength.ToString("X2"));
				if (originalData != null)
				{
					CodingLogItem.RecordToLog(this.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQBParametrizeBase, this.Address.ToString("X2"), BitHelpers.ByteArrayToHexString(originalData), checked_value, password, this.RequestHeader, this.ResponseHeader, this.PreReadCommands, this.PreWriteCommands, this.PostWriteCommands, this.ATST, dictionary, "");
				}
			}
			return requestResult;
		}

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x06004BFB RID: 19451 RVA: 0x00385949 File Offset: 0x00383B49
		public virtual uint DatasetUploadMaxBlockSize
		{
			get
			{
				return (uint)SharedSettings.Current.DatasetUploadMaxBlockSize;
			}
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00385955 File Offset: 0x00383B55
		private void TpReq_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			App.OBDReader.NO_DATA_Counter = 0;
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x00385964 File Offset: 0x00383B64
		public static List<OBDRequest> GetRequestsFromStringCommands(string commandsString, string password, string RequestHeader, string BeforeCommands, string AfterCommands)
		{
			bool zero_seed = false;
			uint uint_pass = 0U;
			if (!string.IsNullOrEmpty(password))
			{
				uint.TryParse(password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out uint_pass);
			}
			if (commandsString == null)
			{
				return new List<OBDRequest>(0);
			}
			commandsString = commandsString.ToUpperInvariant();
			string[] array = CustomPID.StringToCommands(commandsString);
			List<OBDRequest> list = new List<OBDRequest>();
			foreach (string text in array)
			{
				if (text.Contains(':'))
				{
					string[] array3 = text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length == 1)
					{
						OBDRequest obdrequest = new OBDRequest(array3[0], RequestHeader, BeforeCommands, AfterCommands, false);
						list.Add(obdrequest);
					}
					else if (array3.Length > 1)
					{
						OBDRequest obdrequest2 = new OBDRequest(array3[1], array3[0], BeforeCommands, AfterCommands, false);
						list.Add(obdrequest2);
					}
				}
				else
				{
					OBDRequest req = new OBDRequest(text, RequestHeader, BeforeCommands, AfterCommands, false);
					if (text == "2EF198")
					{
						OBDRequest obdrequest3 = new OBDRequest("22F1A5", RequestHeader, BeforeCommands, AfterCommands, false);
						obdrequest3.ResponseDecoded += delegate(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
						{
							if (getCodingData != null && getCodingData.Length != 0)
							{
								string text2 = BitHelpers.ByteArrayToHexString(getCodingData);
								if (getCodingData.All((byte x) => x == 0))
								{
									text2 = "0181C8F63039";
								}
								string text3 = "2EF198" + text2;
								req.Command = text3;
								return;
							}
							string text4 = "2EF1980181C8F63039";
							req.Command = text4;
						};
						list.Add(obdrequest3);
					}
					else if (text == "2EF199")
					{
						req.Command = string.Format("2EF199{0}{1}{2}", DateTimeNowHelper.NowSafe.Year - 2000, DateTimeNowHelper.NowSafe.Month.ToString("00"), DateTimeNowHelper.NowSafe.Day.ToString("00"));
						req.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data != null)
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
							}
							if (data != null && !data.Contains("6EF199"))
							{
								App.OBDReader.DebugWrite("\n2EF199_error\n");
							}
						};
						OBDRequest obdrequest4 = new OBDRequest("22F199", RequestHeader, BeforeCommands, AfterCommands, false);
						obdrequest4.ResponseDecoded += delegate(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
						{
							if (getDateData != null && getDateData.Length >= 3)
							{
								string text5 = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
								req.Command = text5;
							}
						};
						list.Add(obdrequest4);
					}
					else if (text.Length == 6 && text.StartsWith("22"))
					{
						req.CheckLength = true;
						req.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
						req.ResponseDecoded += delegate(OBDRequest req22, byte[] mode22Data, bool getDateDecodeResult, string responseHeader)
						{
							if (mode22Data != null && mode22Data.Length >= 0)
							{
								string text6 = BitHelpers.ByteArrayToHexString(mode22Data);
								string writeCommand = "2E" + req.Command.Substring(2, 4);
								OBDRequest obdrequest6 = App.OBDReader.GetQueueCopy().FirstOrDefault((OBDRequest x) => x.Command == writeCommand);
								if (obdrequest6 != null)
								{
									obdrequest6.Command += text6;
								}
							}
						};
					}
					else if (text == "2704")
					{
						OBDRequest obdrequest5 = new OBDRequest("2703", RequestHeader, BeforeCommands, AfterCommands, false);
						obdrequest5.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
						{
							try
							{
								if (BitConverter.IsLittleEndian)
								{
									Array.Reverse<byte>(getSeedData);
								}
								uint num = BitConverter.ToUInt32(getSeedData, 0);
								if (num == 0U)
								{
									zero_seed = true;
								}
								if (!zero_seed)
								{
									string text7 = (num + uint_pass).ToString("X8");
									req.Command = "2704" + text7;
								}
								else
								{
									req.Command = "3E";
								}
							}
							catch (Exception)
							{
								App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
								req.Command = "3E";
							}
						};
						req.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (request.Command != "3E")
							{
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
								}
								if (data == null || !data.Contains("6704"))
								{
									bool ignoreCodingErrors = SharedSettings.Current.IgnoreCodingErrors;
								}
							}
						};
						list.Add(obdrequest5);
					}
					else if (text == "1102")
					{
						req.ResponseReceived += delegate(OBDRequest req2, string data)
						{
							int num2 = 20000;
							if (req.Header == VagUnitHelper.GetRequestHeaderForMQBUnit("A5"))
							{
								num2 = 5000;
							}
							Task.Delay(num2).Wait();
						};
					}
					else if (text == "31030300" && RequestHeader == VagUnitHelper.GetRequestHeaderForMQBUnit("A5"))
					{
						req.ResponseReceived += delegate(OBDRequest req2, string data)
						{
							OBDRequest obdrequest7 = new OBDRequest("3E80", "700", "", "", false)
							{
								DoNotDecode = true
							};
							DateTime dateTime = DateTimeNowHelper.NowSafe.AddSeconds(10.0);
							obdrequest7.Keys.Add("DELAYUNTIL", dateTime.Ticks.ToString());
							obdrequest7.ResponseReceived += delegate(OBDRequest tpreq2, string emptyData)
							{
								long num3 = long.Parse(tpreq2.Keys["DELAYUNTIL"]);
								if (DateTimeNowHelper.NowSafe.Ticks < num3)
								{
									App.OBDReader.InsertRequestInQueue(tpreq2);
								}
							};
							App.OBDReader.InsertRequestInQueue(obdrequest7);
						};
					}
					else if (text == "ATSTDEF" || text == "atstdef")
					{
						req.Command = "ATST" + SharedSettings.Current.GetATST();
					}
					else if (text.StartsWith("31"))
					{
						req.ResponseReceived += delegate(OBDRequest req2, string data)
						{
							if (data != null)
							{
								string text8 = OBDDataReader.FilterHexAndNewLineOnly(data);
								string text9 = "71" + req2.Command.Substring(2, 4);
								if (text8.Contains("7F3178") && !text8.Contains(text9))
								{
									OBDRequest obdrequest8 = new OBDRequest("3E80", "700", "", "", false)
									{
										DoNotDecode = true
									};
									DateTime dateTime2 = DateTimeNowHelper.NowSafe.AddSeconds(5.0);
									obdrequest8.Keys.Add("DELAYUNTIL", dateTime2.Ticks.ToString());
									obdrequest8.ResponseReceived += delegate(OBDRequest tpreq2, string emptyData)
									{
										long num4 = long.Parse(tpreq2.Keys["DELAYUNTIL"]);
										if (DateTimeNowHelper.NowSafe.Ticks < num4)
										{
											App.OBDReader.InsertRequestInQueue(tpreq2);
										}
									};
									App.OBDReader.InsertRequestInQueue(obdrequest8);
								}
							}
						};
					}
					list.Add(req);
				}
			}
			return list;
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00385D4C File Offset: 0x00383F4C
		public MQBParametrizeBase()
		{
		}

		// Token: 0x04002C72 RID: 11378
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002C73 RID: 11379
		private CodingGroup _Group;

		// Token: 0x04002C74 RID: 11380
		private string _Name = "";

		// Token: 0x04002C75 RID: 11381
		private string _Description = "";

		// Token: 0x04002C76 RID: 11382
		private string _InnerDescription = "";

		// Token: 0x04002C77 RID: 11383
		private string _CurrentState = "";

		// Token: 0x04002C78 RID: 11384
		[CompilerGenerated]
		private bool <PasswordVisible>k__BackingField;

		// Token: 0x04002C79 RID: 11385
		private bool _HasCurrentState = true;

		// Token: 0x04002C7A RID: 11386
		private string _Password = "";

		// Token: 0x04002C7B RID: 11387
		private string _PasswordHint = "";

		// Token: 0x04002C7C RID: 11388
		[CompilerGenerated]
		private ObservableCollection<MQBAdaptationOption> <Options>k__BackingField;

		// Token: 0x04002C7D RID: 11389
		[CompilerGenerated]
		private ObservableCollection<TranslationItem> <Translations>k__BackingField;

		// Token: 0x04002C7E RID: 11390
		private bool _RequiresPro = true;

		// Token: 0x04002C7F RID: 11391
		private AdaptationValueTypes _ValueType;

		// Token: 0x04002C80 RID: 11392
		private string _RequestHeader = "";

		// Token: 0x04002C81 RID: 11393
		private string _ResponseHeader = "";

		// Token: 0x04002C82 RID: 11394
		[CompilerGenerated]
		private string <BeforeCommands>k__BackingField;

		// Token: 0x04002C83 RID: 11395
		[CompilerGenerated]
		private string <AfterCommands>k__BackingField;

		// Token: 0x04002C84 RID: 11396
		private string _PreReadCommands = "1003;2703";

		// Token: 0x04002C85 RID: 11397
		private string _PreWriteCommands = "";

		// Token: 0x04002C86 RID: 11398
		private string _PostWriteCommands = "";

		// Token: 0x04002C87 RID: 11399
		private int _AddressFormatLength = 3;

		// Token: 0x04002C88 RID: 11400
		private int _DataLengthFormatLength = 3;

		// Token: 0x04002C89 RID: 11401
		private string _ATST = "64";

		// Token: 0x04002C8A RID: 11402
		private int _Address;

		// Token: 0x04002C8B RID: 11403
		private int _DataLength = 1;

		// Token: 0x04002C8C RID: 11404
		[CompilerGenerated]
		private string <LogComment>k__BackingField;

		// Token: 0x04002C8D RID: 11405
		[CompilerGenerated]
		private uint <DefaultMaxBlockSize>k__BackingField;

		// Token: 0x020008BD RID: 2237
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004BFF RID: 19455 RVA: 0x00385E39 File Offset: 0x00384039
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004C00 RID: 19456 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004C01 RID: 19457 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Name>b__9_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004C02 RID: 19458 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Description>b__13_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004C03 RID: 19459 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_InnerDescription>b__17_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004C04 RID: 19460 RVA: 0x000027D4 File Offset: 0x000009D4
			internal void <GetCurrentStateRawData>b__114_1(OBDRequest getDataRequest3, string getDataRequestResponse)
			{
			}

			// Token: 0x06004C05 RID: 19461 RVA: 0x00385E45 File Offset: 0x00384045
			internal bool <WriteDataToECU>b__119_8(OBDRequest x)
			{
				return x.Command.StartsWith("36");
			}

			// Token: 0x06004C06 RID: 19462 RVA: 0x00385E58 File Offset: 0x00384058
			internal void <WriteDataToECU>b__119_3(OBDRequest finishTransferRequest2, string response)
			{
				if (response != null)
				{
					string[] array = OBDDataReader.FilterHexAndNewLineOnly(response).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length == 1 && array[0].Contains("037F3778"))
					{
						Task.Delay(1000).Wait();
					}
				}
			}

			// Token: 0x06004C07 RID: 19463 RVA: 0x0037EDD4 File Offset: 0x0037CFD4
			internal bool <GetRequestsFromStringCommands>b__123_5(byte x)
			{
				return x == 0;
			}

			// Token: 0x06004C08 RID: 19464 RVA: 0x00385EA9 File Offset: 0x003840A9
			internal void <GetRequestsFromStringCommands>b__123_6(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && !data.Contains("6EF199"))
				{
					App.OBDReader.DebugWrite("\n2EF199_error\n");
				}
			}

			// Token: 0x06004C09 RID: 19465 RVA: 0x00385ED6 File Offset: 0x003840D6
			internal void <GetRequestsFromStringCommands>b__123_10(OBDRequest request, string data)
			{
				if (request.Command != "3E")
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if (data == null || !data.Contains("6704"))
					{
						bool ignoreCodingErrors = SharedSettings.Current.IgnoreCodingErrors;
					}
				}
			}

			// Token: 0x06004C0A RID: 19466 RVA: 0x00385F10 File Offset: 0x00384110
			internal void <GetRequestsFromStringCommands>b__123_2(OBDRequest req2, string data)
			{
				OBDRequest obdrequest = new OBDRequest("3E80", "700", "", "", false)
				{
					DoNotDecode = true
				};
				DateTime dateTime = DateTimeNowHelper.NowSafe.AddSeconds(10.0);
				obdrequest.Keys.Add("DELAYUNTIL", dateTime.Ticks.ToString());
				obdrequest.ResponseReceived += delegate(OBDRequest tpreq2, string emptyData)
				{
					long num = long.Parse(tpreq2.Keys["DELAYUNTIL"]);
					if (DateTimeNowHelper.NowSafe.Ticks < num)
					{
						App.OBDReader.InsertRequestInQueue(tpreq2);
					}
				};
				App.OBDReader.InsertRequestInQueue(obdrequest);
			}

			// Token: 0x06004C0B RID: 19467 RVA: 0x00385FA8 File Offset: 0x003841A8
			internal void <GetRequestsFromStringCommands>b__123_11(OBDRequest tpreq2, string emptyData)
			{
				long num = long.Parse(tpreq2.Keys["DELAYUNTIL"]);
				if (DateTimeNowHelper.NowSafe.Ticks < num)
				{
					App.OBDReader.InsertRequestInQueue(tpreq2);
				}
			}

			// Token: 0x06004C0C RID: 19468 RVA: 0x00385FE8 File Offset: 0x003841E8
			internal void <GetRequestsFromStringCommands>b__123_3(OBDRequest req2, string data)
			{
				if (data != null)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(data);
					string text2 = "71" + req2.Command.Substring(2, 4);
					if (text.Contains("7F3178") && !text.Contains(text2))
					{
						OBDRequest obdrequest = new OBDRequest("3E80", "700", "", "", false)
						{
							DoNotDecode = true
						};
						DateTime dateTime = DateTimeNowHelper.NowSafe.AddSeconds(5.0);
						obdrequest.Keys.Add("DELAYUNTIL", dateTime.Ticks.ToString());
						obdrequest.ResponseReceived += delegate(OBDRequest tpreq2, string emptyData)
						{
							long num = long.Parse(tpreq2.Keys["DELAYUNTIL"]);
							if (DateTimeNowHelper.NowSafe.Ticks < num)
							{
								App.OBDReader.InsertRequestInQueue(tpreq2);
							}
						};
						App.OBDReader.InsertRequestInQueue(obdrequest);
					}
				}
			}

			// Token: 0x06004C0D RID: 19469 RVA: 0x003860C0 File Offset: 0x003842C0
			internal void <GetRequestsFromStringCommands>b__123_12(OBDRequest tpreq2, string emptyData)
			{
				long num = long.Parse(tpreq2.Keys["DELAYUNTIL"]);
				if (DateTimeNowHelper.NowSafe.Ticks < num)
				{
					App.OBDReader.InsertRequestInQueue(tpreq2);
				}
			}

			// Token: 0x04002C8E RID: 11406
			public static readonly MQBParametrizeBase.<>c <>9 = new MQBParametrizeBase.<>c();

			// Token: 0x04002C8F RID: 11407
			public static Func<TranslationItem, bool> <>9__9_0;

			// Token: 0x04002C90 RID: 11408
			public static Func<TranslationItem, bool> <>9__13_0;

			// Token: 0x04002C91 RID: 11409
			public static Func<TranslationItem, bool> <>9__17_0;

			// Token: 0x04002C92 RID: 11410
			public static ResponseReceivedDelegate <>9__114_1;

			// Token: 0x04002C93 RID: 11411
			public static Predicate<OBDRequest> <>9__119_8;

			// Token: 0x04002C94 RID: 11412
			public static ResponseReceivedDelegate <>9__119_3;

			// Token: 0x04002C95 RID: 11413
			public static Func<byte, bool> <>9__123_5;

			// Token: 0x04002C96 RID: 11414
			public static ResponseReceivedDelegate <>9__123_6;

			// Token: 0x04002C97 RID: 11415
			public static ResponseReceivedDelegate <>9__123_10;

			// Token: 0x04002C98 RID: 11416
			public static ResponseReceivedDelegate <>9__123_11;

			// Token: 0x04002C99 RID: 11417
			public static ResponseReceivedDelegate <>9__123_2;

			// Token: 0x04002C9A RID: 11418
			public static ResponseReceivedDelegate <>9__123_12;

			// Token: 0x04002C9B RID: 11419
			public static ResponseReceivedDelegate <>9__123_3;
		}

		// Token: 0x020008BE RID: 2238
		[CompilerGenerated]
		private sealed class <>c__DisplayClass114_0
		{
			// Token: 0x06004C0E RID: 19470 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass114_0()
			{
			}

			// Token: 0x06004C0F RID: 19471 RVA: 0x00386100 File Offset: 0x00384300
			internal void <GetCurrentStateRawData>b__0(OBDRequest req, string initiateReadRequestData)
			{
				if (initiateReadRequestData != null && (initiateReadRequestData.Contains("75") || initiateReadRequestData.Contains("037F3578")))
				{
					MQBParametrizeBase.<>c__DisplayClass114_1 CS$<>8__locals1 = new MQBParametrizeBase.<>c__DisplayClass114_1();
					CS$<>8__locals1.CS$<>8__locals1 = this;
					CS$<>8__locals1.getDataRequest = new OBDRequest("36" + this.counter.ToString("X2"), this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false);
					CS$<>8__locals1.getDataRequest.ResponseMarker = "76" + this.counter.ToString("X2");
					CS$<>8__locals1.getDataRequest.CheckLength = true;
					CS$<>8__locals1.getDataRequest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
					CS$<>8__locals1.getDataRequest.ResponseReceived += delegate(OBDRequest getDataRequest3, string getDataRequestResponse)
					{
					};
					CS$<>8__locals1.getDataRequest.ResponseDecoded += delegate(OBDRequest getDataRequest2, byte[] getDataRequestData, bool decodeResult, string responseHeader)
					{
						if (getDataRequestData != null)
						{
							CS$<>8__locals1.CS$<>8__locals1.resultBytes.AddRange(getDataRequestData);
							CS$<>8__locals1.CS$<>8__locals1.bytesReceived = CS$<>8__locals1.CS$<>8__locals1.bytesReceived + getDataRequestData.Length;
							int num = CS$<>8__locals1.CS$<>8__locals1.counter;
							CS$<>8__locals1.CS$<>8__locals1.counter = num + 1;
							string text = string.Format("{0}/{1}", CS$<>8__locals1.CS$<>8__locals1.bytesReceived, CS$<>8__locals1.CS$<>8__locals1.<>4__this.DataLength);
							IProgress<string> progress = CS$<>8__locals1.CS$<>8__locals1.progress;
							if (progress != null)
							{
								progress.Report(text);
							}
							if (CS$<>8__locals1.CS$<>8__locals1.bytesReceived < CS$<>8__locals1.CS$<>8__locals1.<>4__this.DataLength)
							{
								CS$<>8__locals1.getDataRequest.FailCounter = 0;
								CS$<>8__locals1.getDataRequest.Payload = "";
								CS$<>8__locals1.getDataRequest.SkippedCycles = 0;
								CS$<>8__locals1.getDataRequest.Command = "36" + CS$<>8__locals1.CS$<>8__locals1.counter.ToString("X2");
								CS$<>8__locals1.getDataRequest.CheckLength = true;
								CS$<>8__locals1.getDataRequest.ResponseMarker = "76" + CS$<>8__locals1.CS$<>8__locals1.counter.ToString("X2");
								App.OBDReader.InsertRequestInQueue(CS$<>8__locals1.getDataRequest);
								return;
							}
							CS$<>8__locals1.CS$<>8__locals1.requestResult = CodingRequestResult.Success;
						}
					};
					OBDRequest getDataRequest = CS$<>8__locals1.getDataRequest;
					Action<string> action;
					if ((action = this.<>9__3) == null)
					{
						action = (this.<>9__3 = delegate(string s)
						{
							string text2 = string.Format("{0}/{1}\n{2}", this.bytesReceived, this.<>4__this.DataLength, s);
							IProgress<string> progress2 = this.progress;
							if (progress2 == null)
							{
								return;
							}
							progress2.Report(text2);
						});
					}
					getDataRequest.Progress = new Progress<string>(action);
					App.OBDReader.InsertRequestInQueue(CS$<>8__locals1.getDataRequest);
				}
			}

			// Token: 0x06004C10 RID: 19472 RVA: 0x0038624C File Offset: 0x0038444C
			internal void <GetCurrentStateRawData>b__3(string s)
			{
				string text = string.Format("{0}/{1}\n{2}", this.bytesReceived, this.<>4__this.DataLength, s);
				IProgress<string> progress = this.progress;
				if (progress == null)
				{
					return;
				}
				progress.Report(text);
			}

			// Token: 0x04002C9C RID: 11420
			public int counter;

			// Token: 0x04002C9D RID: 11421
			public MQBParametrizeBase <>4__this;

			// Token: 0x04002C9E RID: 11422
			public List<byte> resultBytes;

			// Token: 0x04002C9F RID: 11423
			public int bytesReceived;

			// Token: 0x04002CA0 RID: 11424
			public IProgress<string> progress;

			// Token: 0x04002CA1 RID: 11425
			public CodingRequestResult requestResult;

			// Token: 0x04002CA2 RID: 11426
			public Action<string> <>9__3;
		}

		// Token: 0x020008BF RID: 2239
		[CompilerGenerated]
		private sealed class <>c__DisplayClass114_1
		{
			// Token: 0x06004C11 RID: 19473 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass114_1()
			{
			}

			// Token: 0x06004C12 RID: 19474 RVA: 0x00386294 File Offset: 0x00384494
			internal void <GetCurrentStateRawData>b__2(OBDRequest getDataRequest2, byte[] getDataRequestData, bool decodeResult, string responseHeader)
			{
				if (getDataRequestData != null)
				{
					this.CS$<>8__locals1.resultBytes.AddRange(getDataRequestData);
					this.CS$<>8__locals1.bytesReceived = this.CS$<>8__locals1.bytesReceived + getDataRequestData.Length;
					int counter = this.CS$<>8__locals1.counter;
					this.CS$<>8__locals1.counter = counter + 1;
					string text = string.Format("{0}/{1}", this.CS$<>8__locals1.bytesReceived, this.CS$<>8__locals1.<>4__this.DataLength);
					IProgress<string> progress = this.CS$<>8__locals1.progress;
					if (progress != null)
					{
						progress.Report(text);
					}
					if (this.CS$<>8__locals1.bytesReceived < this.CS$<>8__locals1.<>4__this.DataLength)
					{
						this.getDataRequest.FailCounter = 0;
						this.getDataRequest.Payload = "";
						this.getDataRequest.SkippedCycles = 0;
						this.getDataRequest.Command = "36" + this.CS$<>8__locals1.counter.ToString("X2");
						this.getDataRequest.CheckLength = true;
						this.getDataRequest.ResponseMarker = "76" + this.CS$<>8__locals1.counter.ToString("X2");
						App.OBDReader.InsertRequestInQueue(this.getDataRequest);
						return;
					}
					this.CS$<>8__locals1.requestResult = CodingRequestResult.Success;
				}
			}

			// Token: 0x04002CA3 RID: 11427
			public OBDRequest getDataRequest;

			// Token: 0x04002CA4 RID: 11428
			public MQBParametrizeBase.<>c__DisplayClass114_0 CS$<>8__locals1;
		}

		// Token: 0x020008C0 RID: 2240
		[CompilerGenerated]
		private sealed class <>c__DisplayClass119_0
		{
			// Token: 0x06004C13 RID: 19475 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass119_0()
			{
			}

			// Token: 0x06004C14 RID: 19476 RVA: 0x003863F8 File Offset: 0x003845F8
			internal void <WriteDataToECU>b__0(OBDRequest initiateWriteRequest3, string response)
			{
				if (response != null && OBDDataReader.FilterHexAndNewLineOnly(response).Contains("037F3478"))
				{
					this.hasNR78initiate = true;
				}
			}

			// Token: 0x06004C15 RID: 19477 RVA: 0x00386418 File Offset: 0x00384618
			internal void <WriteDataToECU>b__1(OBDRequest initiateWriteRequest2, byte[] data, bool decodeResult, string responseHeader)
			{
				MQBParametrizeBase.<>c__DisplayClass119_1 CS$<>8__locals1 = new MQBParametrizeBase.<>c__DisplayClass119_1();
				CS$<>8__locals1.CS$<>8__locals1 = this;
				if (data == null && !this.hasNR78initiate)
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					return;
				}
				try
				{
					byte b = data[0];
					byte[] array = new byte[data.Length - 1];
					Array.Copy(data, 1, array, 0, data.Length - 1);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					for (int i = 0; i < array.Length; i++)
					{
						this.maxBlockSize += (uint)array[i] * (uint)Math.Pow(256.0, (double)i);
					}
				}
				catch (Exception)
				{
					this.maxBlockSize = this.<>4__this.DefaultMaxBlockSize;
				}
				this.maxBlockSize -= 2U;
				if (this.maxBlockSize > this.<>4__this.DatasetUploadMaxBlockSize)
				{
					this.maxBlockSize = this.<>4__this.DatasetUploadMaxBlockSize;
				}
				List<OBDRequest> list = new List<OBDRequest>();
				MemoryStream memoryStream = new MemoryStream(this.dataToWrite);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				int num = 1;
				CS$<>8__locals1.tpReq = new OBDRequest("3E80", "700", "", "", false)
				{
					DoNotDecode = true
				};
				CS$<>8__locals1.tpReq.ResponseDecoded += this.<>4__this.TpReq_ResponseDecoded;
				while (memoryStream.Position < memoryStream.Length)
				{
					MQBParametrizeBase.<>c__DisplayClass119_2 CS$<>8__locals2 = new MQBParametrizeBase.<>c__DisplayClass119_2();
					CS$<>8__locals2.CS$<>8__locals2 = CS$<>8__locals1;
					long position = memoryStream.Position;
					uint num2 = this.maxBlockSize;
					if ((ulong)num2 > (ulong)(memoryStream.Length - memoryStream.Position))
					{
						num2 = (uint)memoryStream.Length - (uint)memoryStream.Position;
					}
					byte[] array2 = new byte[num2];
					memoryStream.Read(array2, 0, array2.Length);
					CS$<>8__locals2.positionFinish = memoryStream.Position;
					CS$<>8__locals2.totalLength = memoryStream.Length;
					OBDRequest obdrequest = new OBDRequest("36" + num.ToString("X2") + BitHelpers.ByteArrayToHexString(array2), this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false);
					obdrequest.ResponseReceived += delegate(OBDRequest stepreq2, string stepdata)
					{
						App.OBDReader.NO_DATA_Counter = 0;
						CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.writeWasStarted = true;
						if (stepdata != null)
						{
							string text = OBDDataReader.FilterHexAndNewLineOnly(stepdata);
							if (text.Contains("7F3678") && !text.Contains("76"))
							{
								Task.Delay(500).Wait();
							}
							if ((!text.Contains("7F3678") && text.Contains("037F36")) || stepdata.Contains("NO DATA"))
							{
								CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
								List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
								queueCopy2.RemoveAll((OBDRequest x) => x.Command.StartsWith("36"));
								App.OBDReader.ReplaceQueue(queueCopy2);
							}
						}
						IProgress<string> progress2 = CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.progress;
						if (progress2 == null)
						{
							return;
						}
						progress2.Report(Translate.GetString("coding_progress_ApplyingNewData") + string.Format("\n{0} / {1}", CS$<>8__locals2.positionFinish, CS$<>8__locals2.totalLength));
					};
					CS$<>8__locals2.progressString = Translate.GetString("coding_progress_ApplyingNewData") + string.Format("\n{0} / {1}\n", position, CS$<>8__locals2.totalLength);
					obdrequest.Progress = new Progress<string>(delegate(string writeProgress)
					{
						IProgress<string> progress3 = CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.progress;
						if (progress3 == null)
						{
							return;
						}
						progress3.Report(CS$<>8__locals2.progressString + writeProgress);
					});
					obdrequest.Keys.Add("TesterPresent", "ATSH700;023E80;ATSH" + obdrequest.Header);
					list.Add(obdrequest);
					list.Add(CS$<>8__locals2.CS$<>8__locals2.tpReq);
					num++;
				}
				OBDRequest obdrequest2 = list.LastOrDefault((OBDRequest x) => x != CS$<>8__locals1.tpReq);
				ResponseReceivedDelegate responseReceivedDelegate;
				if ((responseReceivedDelegate = this.<>9__5) == null)
				{
					responseReceivedDelegate = (this.<>9__5 = delegate(OBDRequest lastRequest2, string lastRequestData)
					{
						App.OBDReader.NO_DATA_Counter = 0;
						if (lastRequestData != null)
						{
							string text2 = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
							if (text2.Contains("7F3678") || text2.Contains("76"))
							{
								IProgress<string> progress4 = this.progress;
								if (progress4 != null)
								{
									progress4.Report(Translate.GetString("coding_progress_DataAccepted"));
								}
								this.requestResult = CodingRequestResult.Success;
								return;
							}
							if (!text2.Contains("76") && !text2.Contains("037F3478"))
							{
								if (text2.Length >= 11 && text2.Contains("7F36"))
								{
									int num3 = text2.IndexOf("7F36");
									int num4 = int.Parse(text2.Substring(num3 + 4, 2), NumberStyles.HexNumber);
									if (num4 >= 128 || num4 == 34)
									{
										this.requestResult = CodingRequestResult.WrongConditions;
										return;
									}
									if (num4 == 51)
									{
										this.requestResult = CodingRequestResult.WrongAccessKey;
										return;
									}
									if (num4 == 49)
									{
										this.requestResult = CodingRequestResult.NotSupported;
										return;
									}
									this.requestResult = CodingRequestResult.UnknownError;
									return;
								}
								else
								{
									this.requestResult = CodingRequestResult.UnknownError;
								}
							}
						}
					});
				}
				obdrequest2.ResponseReceived += responseReceivedDelegate;
				List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
				list.AddRange(queueCopy);
				IProgress<string> progress = this.progress;
				if (progress != null)
				{
					progress.Report(Translate.GetString("coding_progress_ApplyingNewData"));
				}
				App.OBDReader.ReplaceQueue(list);
			}

			// Token: 0x06004C16 RID: 19478 RVA: 0x00386750 File Offset: 0x00384950
			internal void <WriteDataToECU>b__5(OBDRequest lastRequest2, string lastRequestData)
			{
				App.OBDReader.NO_DATA_Counter = 0;
				if (lastRequestData != null)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
					if (text.Contains("7F3678") || text.Contains("76"))
					{
						IProgress<string> progress = this.progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_DataAccepted"));
						}
						this.requestResult = CodingRequestResult.Success;
						return;
					}
					if (!text.Contains("76") && !text.Contains("037F3478"))
					{
						if (text.Length >= 11 && text.Contains("7F36"))
						{
							int num = text.IndexOf("7F36");
							int num2 = int.Parse(text.Substring(num + 4, 2), NumberStyles.HexNumber);
							if (num2 >= 128 || num2 == 34)
							{
								this.requestResult = CodingRequestResult.WrongConditions;
								return;
							}
							if (num2 == 51)
							{
								this.requestResult = CodingRequestResult.WrongAccessKey;
								return;
							}
							if (num2 == 49)
							{
								this.requestResult = CodingRequestResult.NotSupported;
								return;
							}
							this.requestResult = CodingRequestResult.UnknownError;
							return;
						}
						else
						{
							this.requestResult = CodingRequestResult.UnknownError;
						}
					}
				}
			}

			// Token: 0x06004C17 RID: 19479 RVA: 0x00386848 File Offset: 0x00384A48
			internal void <WriteDataToECU>b__2(OBDRequest initiateWriteRequest2, string response)
			{
				if (response == null || response.Contains("NO DATA") || response.Contains("ERROR"))
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.requestResult = CodingRequestResult.NoData;
					return;
				}
				string text = OBDDataReader.FilterHexAndNewLineOnly(response);
				if (!text.Contains("74") && !text.Contains("037F3478"))
				{
					if (text.Length >= 11 && text.Contains("7F34"))
					{
						int num = text.IndexOf("7F34");
						int num2 = int.Parse(text.Substring(num + 4, 2), NumberStyles.HexNumber);
						if (num2 >= 128 || num2 == 34)
						{
							this.requestResult = CodingRequestResult.WrongConditions;
						}
						else if (num2 == 51)
						{
							this.requestResult = CodingRequestResult.WrongAccessKey;
						}
						else if (num2 == 49)
						{
							this.requestResult = CodingRequestResult.NotSupported;
						}
						else
						{
							this.requestResult = CodingRequestResult.UnknownError;
						}
					}
					else
					{
						this.requestResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			}

			// Token: 0x04002CA5 RID: 11429
			public bool hasNR78initiate;

			// Token: 0x04002CA6 RID: 11430
			public uint maxBlockSize;

			// Token: 0x04002CA7 RID: 11431
			public MQBParametrizeBase <>4__this;

			// Token: 0x04002CA8 RID: 11432
			public byte[] dataToWrite;

			// Token: 0x04002CA9 RID: 11433
			public bool writeWasStarted;

			// Token: 0x04002CAA RID: 11434
			public CodingRequestResult requestResult;

			// Token: 0x04002CAB RID: 11435
			public IProgress<string> progress;

			// Token: 0x04002CAC RID: 11436
			public ResponseReceivedDelegate <>9__5;
		}

		// Token: 0x020008C1 RID: 2241
		[CompilerGenerated]
		private sealed class <>c__DisplayClass119_1
		{
			// Token: 0x06004C18 RID: 19480 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass119_1()
			{
			}

			// Token: 0x06004C19 RID: 19481 RVA: 0x0038693D File Offset: 0x00384B3D
			internal bool <WriteDataToECU>b__4(OBDRequest x)
			{
				return x != this.tpReq;
			}

			// Token: 0x04002CAD RID: 11437
			public OBDRequest tpReq;

			// Token: 0x04002CAE RID: 11438
			public MQBParametrizeBase.<>c__DisplayClass119_0 CS$<>8__locals1;
		}

		// Token: 0x020008C2 RID: 2242
		[CompilerGenerated]
		private sealed class <>c__DisplayClass119_2
		{
			// Token: 0x06004C1A RID: 19482 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass119_2()
			{
			}

			// Token: 0x06004C1B RID: 19483 RVA: 0x0038694C File Offset: 0x00384B4C
			internal void <WriteDataToECU>b__6(OBDRequest stepreq2, string stepdata)
			{
				App.OBDReader.NO_DATA_Counter = 0;
				this.CS$<>8__locals2.CS$<>8__locals1.writeWasStarted = true;
				if (stepdata != null)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(stepdata);
					if (text.Contains("7F3678") && !text.Contains("76"))
					{
						Task.Delay(500).Wait();
					}
					if ((!text.Contains("7F3678") && text.Contains("037F36")) || stepdata.Contains("NO DATA"))
					{
						this.CS$<>8__locals2.CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
						queueCopy.RemoveAll((OBDRequest x) => x.Command.StartsWith("36"));
						App.OBDReader.ReplaceQueue(queueCopy);
					}
				}
				IProgress<string> progress = this.CS$<>8__locals2.CS$<>8__locals1.progress;
				if (progress == null)
				{
					return;
				}
				progress.Report(Translate.GetString("coding_progress_ApplyingNewData") + string.Format("\n{0} / {1}", this.positionFinish, this.totalLength));
			}

			// Token: 0x06004C1C RID: 19484 RVA: 0x00386A68 File Offset: 0x00384C68
			internal void <WriteDataToECU>b__7(string writeProgress)
			{
				IProgress<string> progress = this.CS$<>8__locals2.CS$<>8__locals1.progress;
				if (progress == null)
				{
					return;
				}
				progress.Report(this.progressString + writeProgress);
			}

			// Token: 0x04002CAF RID: 11439
			public long positionFinish;

			// Token: 0x04002CB0 RID: 11440
			public long totalLength;

			// Token: 0x04002CB1 RID: 11441
			public string progressString;

			// Token: 0x04002CB2 RID: 11442
			public MQBParametrizeBase.<>c__DisplayClass119_1 CS$<>8__locals2;
		}

		// Token: 0x020008C3 RID: 2243
		[CompilerGenerated]
		private sealed class <>c__DisplayClass123_0
		{
			// Token: 0x06004C1D RID: 19485 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass123_0()
			{
			}

			// Token: 0x04002CB3 RID: 11443
			public bool zero_seed;

			// Token: 0x04002CB4 RID: 11444
			public uint uint_pass;
		}

		// Token: 0x020008C4 RID: 2244
		[CompilerGenerated]
		private sealed class <>c__DisplayClass123_1
		{
			// Token: 0x06004C1E RID: 19486 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass123_1()
			{
			}

			// Token: 0x06004C1F RID: 19487 RVA: 0x00386A90 File Offset: 0x00384C90
			internal void <GetRequestsFromStringCommands>b__4(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
			{
				if (getCodingData != null && getCodingData.Length != 0)
				{
					string text = BitHelpers.ByteArrayToHexString(getCodingData);
					if (getCodingData.All((byte x) => x == 0))
					{
						text = "0181C8F63039";
					}
					string text2 = "2EF198" + text;
					this.req.Command = text2;
					return;
				}
				string text3 = "2EF1980181C8F63039";
				this.req.Command = text3;
			}

			// Token: 0x06004C20 RID: 19488 RVA: 0x00386B04 File Offset: 0x00384D04
			internal void <GetRequestsFromStringCommands>b__7(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
			{
				if (getDateData != null && getDateData.Length >= 3)
				{
					string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
					this.req.Command = text;
				}
			}

			// Token: 0x06004C21 RID: 19489 RVA: 0x00386B64 File Offset: 0x00384D64
			internal void <GetRequestsFromStringCommands>b__0(OBDRequest req22, byte[] mode22Data, bool getDateDecodeResult, string responseHeader)
			{
				if (mode22Data != null && mode22Data.Length >= 0)
				{
					MQBParametrizeBase.<>c__DisplayClass123_2 CS$<>8__locals1 = new MQBParametrizeBase.<>c__DisplayClass123_2();
					string text = BitHelpers.ByteArrayToHexString(mode22Data);
					CS$<>8__locals1.writeCommand = "2E" + this.req.Command.Substring(2, 4);
					OBDRequest obdrequest = App.OBDReader.GetQueueCopy().FirstOrDefault((OBDRequest x) => x.Command == CS$<>8__locals1.writeCommand);
					if (obdrequest != null)
					{
						obdrequest.Command += text;
					}
				}
			}

			// Token: 0x06004C22 RID: 19490 RVA: 0x00386BDC File Offset: 0x00384DDC
			internal void <GetRequestsFromStringCommands>b__9(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(getSeedData);
					}
					uint num = BitConverter.ToUInt32(getSeedData, 0);
					if (num == 0U)
					{
						this.CS$<>8__locals1.zero_seed = true;
					}
					if (!this.CS$<>8__locals1.zero_seed)
					{
						string text = (num + this.CS$<>8__locals1.uint_pass).ToString("X8");
						this.req.Command = "2704" + text;
					}
					else
					{
						this.req.Command = "3E";
					}
				}
				catch (Exception)
				{
					App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
					this.req.Command = "3E";
				}
			}

			// Token: 0x06004C23 RID: 19491 RVA: 0x00386C94 File Offset: 0x00384E94
			internal void <GetRequestsFromStringCommands>b__1(OBDRequest req2, string data)
			{
				int num = 20000;
				if (this.req.Header == VagUnitHelper.GetRequestHeaderForMQBUnit("A5"))
				{
					num = 5000;
				}
				Task.Delay(num).Wait();
			}

			// Token: 0x04002CB5 RID: 11445
			public OBDRequest req;

			// Token: 0x04002CB6 RID: 11446
			public MQBParametrizeBase.<>c__DisplayClass123_0 CS$<>8__locals1;
		}

		// Token: 0x020008C5 RID: 2245
		[CompilerGenerated]
		private sealed class <>c__DisplayClass123_2
		{
			// Token: 0x06004C24 RID: 19492 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass123_2()
			{
			}

			// Token: 0x06004C25 RID: 19493 RVA: 0x00386CD4 File Offset: 0x00384ED4
			internal bool <GetRequestsFromStringCommands>b__8(OBDRequest x)
			{
				return x.Command == this.writeCommand;
			}

			// Token: 0x04002CB7 RID: 11447
			public string writeCommand;
		}

		// Token: 0x020008C6 RID: 2246
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__111 : IAsyncStateMachine
		{
			// Token: 0x06004C26 RID: 19494 RVA: 0x00386CE8 File Offset: 0x00384EE8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBParametrizeBase mqbparametrizeBase = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
							goto IL_025B;
						}
						if (originalData != null || mqbparametrizeBase.RequestHeader == VagUnitHelper.GetRequestHeaderForMQBUnit("19") || mqbparametrizeBase.RequestHeader == VagUnitHelper.GetRequestHeaderForMQBUnit("65"))
						{
							goto IL_00E2;
						}
						taskAwaiter = mqbparametrizeBase.UpdateCurrentState(password, progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBParametrizeBase.<Execute>d__111>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					CodingRequestResult result = taskAwaiter.GetResult();
					if (result != CodingRequestResult.Success)
					{
						codingRequestResult = result;
						goto IL_027E;
					}
					originalData = BitHelpers.ConvertHexToBytesX(mqbparametrizeBase.CurrentState);
					IL_00E2:
					if (skipIfTheSameData && originalData != null && ArrayHelpers.ArrayEquals<byte>(BitHelpers.ConvertHexToBytesX(value), originalData))
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("AddressFormatLength", mqbparametrizeBase.AddressFormatLength.ToString("X2"));
						dictionary.Add("DataLengthFormatLength", mqbparametrizeBase.DataLengthFormatLength.ToString("X2"));
						dictionary.Add("DataLength", mqbparametrizeBase.DataLength.ToString("X2"));
						CodingLogItem.RecordToLog(mqbparametrizeBase.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQBParametrizeBase, mqbparametrizeBase.Address.ToString("X2"), BitHelpers.ByteArrayToHexString(originalData), value, password, mqbparametrizeBase.RequestHeader, mqbparametrizeBase.ResponseHeader, mqbparametrizeBase.PreReadCommands, mqbparametrizeBase.PreWriteCommands, mqbparametrizeBase.PostWriteCommands, mqbparametrizeBase.ATST, dictionary, "");
						codingRequestResult = CodingRequestResult.Success;
						goto IL_027E;
					}
					taskAwaiter = mqbparametrizeBase.WriteDataToECU(password, UserFriendlyValue, progress, originalData, value).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBParametrizeBase.<Execute>d__111>(ref taskAwaiter, ref this);
						return;
					}
					IL_025B:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_027E:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004C27 RID: 19495 RVA: 0x00386FA4 File Offset: 0x003851A4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002CB8 RID: 11448
			public int <>1__state;

			// Token: 0x04002CB9 RID: 11449
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002CBA RID: 11450
			public byte[] originalData;

			// Token: 0x04002CBB RID: 11451
			public MQBParametrizeBase <>4__this;

			// Token: 0x04002CBC RID: 11452
			public string password;

			// Token: 0x04002CBD RID: 11453
			public IProgress<string> progress;

			// Token: 0x04002CBE RID: 11454
			public bool skipIfTheSameData;

			// Token: 0x04002CBF RID: 11455
			public string value;

			// Token: 0x04002CC0 RID: 11456
			public string UserFriendlyValue;

			// Token: 0x04002CC1 RID: 11457
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x020008C7 RID: 2247
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentStateRawData>d__114 : IAsyncStateMachine
		{
			// Token: 0x06004C28 RID: 19496 RVA: 0x00386FB4 File Offset: 0x003851B4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBParametrizeBase mqbparametrizeBase = this;
				Tuple<byte[], CodingRequestResult> tuple;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBParametrizeBase.<>c__DisplayClass114_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						mqbparametrizeBase.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(mqbparametrizeBase.PreReadCommands, password, mqbparametrizeBase.RequestHeader, mqbparametrizeBase.BeforeCommands, mqbparametrizeBase.AfterCommands));
						string text = mqbparametrizeBase.Address.ToString("X" + (mqbparametrizeBase.AddressFormatLength * 2).ToString());
						string text2 = mqbparametrizeBase.DataLength.ToString("X" + (mqbparametrizeBase.DataLengthFormatLength * 2).ToString());
						string text3 = string.Concat(new string[]
						{
							"3500",
							(mqbparametrizeBase.DataLengthFormatLength & 15).ToString("X1"),
							(mqbparametrizeBase.AddressFormatLength & 15).ToString("X1"),
							text,
							text2
						});
						CS$<>8__locals1.resultBytes = new List<byte>(mqbparametrizeBase.DataLength);
						CS$<>8__locals1.bytesReceived = 0;
						CS$<>8__locals1.counter = 1;
						OBDRequest obdrequest = new OBDRequest(text3, mqbparametrizeBase.RequestHeader, mqbparametrizeBase.BeforeCommands, mqbparametrizeBase.AfterCommands, false);
						obdrequest.ResponseMarker = "75";
						obdrequest.ResponseReceived += delegate(OBDRequest req, string initiateReadRequestData)
						{
							if (initiateReadRequestData != null && (initiateReadRequestData.Contains("75") || initiateReadRequestData.Contains("037F3578")))
							{
								MQBParametrizeBase.<>c__DisplayClass114_1 CS$<>8__locals1 = new MQBParametrizeBase.<>c__DisplayClass114_1();
								CS$<>8__locals1.CS$<>8__locals1 = CS$<>8__locals1;
								CS$<>8__locals1.getDataRequest = new OBDRequest("36" + CS$<>8__locals1.counter.ToString("X2"), CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false);
								CS$<>8__locals1.getDataRequest.ResponseMarker = "76" + CS$<>8__locals1.counter.ToString("X2");
								CS$<>8__locals1.getDataRequest.CheckLength = true;
								CS$<>8__locals1.getDataRequest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
								CS$<>8__locals1.getDataRequest.ResponseReceived += delegate(OBDRequest getDataRequest3, string getDataRequestResponse)
								{
								};
								CS$<>8__locals1.getDataRequest.ResponseDecoded += delegate(OBDRequest getDataRequest2, byte[] getDataRequestData, bool decodeResult, string responseHeader)
								{
									if (getDataRequestData != null)
									{
										CS$<>8__locals1.CS$<>8__locals1.resultBytes.AddRange(getDataRequestData);
										CS$<>8__locals1.CS$<>8__locals1.bytesReceived = CS$<>8__locals1.CS$<>8__locals1.bytesReceived + getDataRequestData.Length;
										int counter = CS$<>8__locals1.CS$<>8__locals1.counter;
										CS$<>8__locals1.CS$<>8__locals1.counter = counter + 1;
										string text4 = string.Format("{0}/{1}", CS$<>8__locals1.CS$<>8__locals1.bytesReceived, CS$<>8__locals1.CS$<>8__locals1.<>4__this.DataLength);
										IProgress<string> progress = CS$<>8__locals1.CS$<>8__locals1.progress;
										if (progress != null)
										{
											progress.Report(text4);
										}
										if (CS$<>8__locals1.CS$<>8__locals1.bytesReceived < CS$<>8__locals1.CS$<>8__locals1.<>4__this.DataLength)
										{
											CS$<>8__locals1.getDataRequest.FailCounter = 0;
											CS$<>8__locals1.getDataRequest.Payload = "";
											CS$<>8__locals1.getDataRequest.SkippedCycles = 0;
											CS$<>8__locals1.getDataRequest.Command = "36" + CS$<>8__locals1.CS$<>8__locals1.counter.ToString("X2");
											CS$<>8__locals1.getDataRequest.CheckLength = true;
											CS$<>8__locals1.getDataRequest.ResponseMarker = "76" + CS$<>8__locals1.CS$<>8__locals1.counter.ToString("X2");
											App.OBDReader.InsertRequestInQueue(CS$<>8__locals1.getDataRequest);
											return;
										}
										CS$<>8__locals1.CS$<>8__locals1.requestResult = CodingRequestResult.Success;
									}
								};
								OBDRequest getDataRequest = CS$<>8__locals1.getDataRequest;
								Action<string> action;
								if ((action = CS$<>8__locals1.<>9__3) == null)
								{
									action = (CS$<>8__locals1.<>9__3 = delegate(string s)
									{
										string text5 = string.Format("{0}/{1}\n{2}", CS$<>8__locals1.bytesReceived, CS$<>8__locals1.<>4__this.DataLength, s);
										IProgress<string> progress2 = CS$<>8__locals1.progress;
										if (progress2 == null)
										{
											return;
										}
										progress2.Report(text5);
									});
								}
								getDataRequest.Progress = new Progress<string>(action);
								App.OBDReader.InsertRequestInQueue(CS$<>8__locals1.getDataRequest);
							}
						};
						OBDRequest obdrequest2 = new OBDRequest("37", mqbparametrizeBase.RequestHeader, mqbparametrizeBase.BeforeCommands + ";ATAT0;ATSTFF", mqbparametrizeBase.AfterCommands + string.Format(";ATAT{0};ATST{1}", SharedSettings.Current.AdaptiveTimings, mqbparametrizeBase.ATST), false);
						list.Add(obdrequest);
						list.Add(obdrequest2);
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBParametrizeBase.<GetCurrentStateRawData>d__114>(ref taskAwaiter, ref this);
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
					tuple = new Tuple<byte[], CodingRequestResult>(CS$<>8__locals1.resultBytes.ToArray(), CS$<>8__locals1.requestResult);
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
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x06004C29 RID: 19497 RVA: 0x0038729C File Offset: 0x0038549C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002CC2 RID: 11458
			public int <>1__state;

			// Token: 0x04002CC3 RID: 11459
			public AsyncTaskMethodBuilder<Tuple<byte[], CodingRequestResult>> <>t__builder;

			// Token: 0x04002CC4 RID: 11460
			public MQBParametrizeBase <>4__this;

			// Token: 0x04002CC5 RID: 11461
			public IProgress<string> progress;

			// Token: 0x04002CC6 RID: 11462
			public string password;

			// Token: 0x04002CC7 RID: 11463
			private MQBParametrizeBase.<>c__DisplayClass114_0 <>8__1;

			// Token: 0x04002CC8 RID: 11464
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008C8 RID: 2248
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__112 : IAsyncStateMachine
		{
			// Token: 0x06004C2A RID: 19498 RVA: 0x003872AC File Offset: 0x003854AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBParametrizeBase mqbparametrizeBase = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
							num2 = -1;
							goto IL_0103;
						}
						if (string.IsNullOrEmpty(password))
						{
							password = mqbparametrizeBase.Password;
						}
						if (mqbparametrizeBase.LogComment == null)
						{
							goto IL_00A0;
						}
						taskAwaiter3 = App.OBDReader.DebugWrite("\nLogComment\n").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBParametrizeBase.<UpdateCurrentState>d__112>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					IL_00A0:
					taskAwaiter = mqbparametrizeBase.GetCurrentStateRawData(password, progress).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBParametrizeBase.<UpdateCurrentState>d__112>(ref taskAwaiter, ref this);
						return;
					}
					IL_0103:
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						mqbparametrizeBase.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						mqbparametrizeBase.CurrentState = BitHelpers.ByteArrayToHexString(item2);
						codingRequestResult = CodingRequestResult.Success;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004C2B RID: 19499 RVA: 0x00387440 File Offset: 0x00385640
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002CC9 RID: 11465
			public int <>1__state;

			// Token: 0x04002CCA RID: 11466
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002CCB RID: 11467
			public string password;

			// Token: 0x04002CCC RID: 11468
			public MQBParametrizeBase <>4__this;

			// Token: 0x04002CCD RID: 11469
			public IProgress<string> progress;

			// Token: 0x04002CCE RID: 11470
			private TaskAwaiter <>u__1;

			// Token: 0x04002CCF RID: 11471
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__2;
		}

		// Token: 0x020008C9 RID: 2249
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteDataToECU>d__119 : IAsyncStateMachine
		{
			// Token: 0x06004C2C RID: 19500 RVA: 0x00387450 File Offset: 0x00385650
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBParametrizeBase mqbparametrizeBase = this;
				CodingRequestResult requestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBParametrizeBase.<>c__DisplayClass119_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						mqbparametrizeBase.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.dataToWrite = BitHelpers.ConvertHexToBytesX(checked_value);
						CS$<>8__locals1.writeWasStarted = false;
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(mqbparametrizeBase.PreWriteCommands, password, mqbparametrizeBase.RequestHeader, mqbparametrizeBase.BeforeCommands, mqbparametrizeBase.AfterCommands));
						string text = mqbparametrizeBase.Address.ToString("X" + (mqbparametrizeBase.AddressFormatLength * 2).ToString());
						string text2 = mqbparametrizeBase.DataLength.ToString("X" + (mqbparametrizeBase.DataLengthFormatLength * 2).ToString());
						OBDRequest obdrequest = new OBDRequest(string.Concat(new string[]
						{
							"3400",
							(mqbparametrizeBase.DataLengthFormatLength & 15).ToString("X1"),
							(mqbparametrizeBase.AddressFormatLength & 15).ToString("X1"),
							text,
							text2
						}), mqbparametrizeBase.RequestHeader, mqbparametrizeBase.BeforeCommands + ";ATAT0;ATSTFF", mqbparametrizeBase.AfterCommands + ";ATAT1;ATST" + mqbparametrizeBase.ATST, false);
						obdrequest.ResponseMarker = "74";
						CS$<>8__locals1.maxBlockSize = 0U;
						CS$<>8__locals1.hasNR78initiate = false;
						obdrequest.ResponseReceived += delegate(OBDRequest initiateWriteRequest3, string response)
						{
							if (response != null && OBDDataReader.FilterHexAndNewLineOnly(response).Contains("037F3478"))
							{
								CS$<>8__locals1.hasNR78initiate = true;
							}
						};
						obdrequest.ResponseDecoded += delegate(OBDRequest initiateWriteRequest2, byte[] data, bool decodeResult, string responseHeader)
						{
							MQBParametrizeBase.<>c__DisplayClass119_1 CS$<>8__locals1 = new MQBParametrizeBase.<>c__DisplayClass119_1();
							CS$<>8__locals1.CS$<>8__locals1 = CS$<>8__locals1;
							if (data == null && !CS$<>8__locals1.hasNR78initiate)
							{
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								return;
							}
							try
							{
								byte b = data[0];
								byte[] array = new byte[data.Length - 1];
								Array.Copy(data, 1, array, 0, data.Length - 1);
								if (BitConverter.IsLittleEndian)
								{
									Array.Reverse<byte>(array);
								}
								for (int i = 0; i < array.Length; i++)
								{
									CS$<>8__locals1.maxBlockSize += (uint)array[i] * (uint)Math.Pow(256.0, (double)i);
								}
							}
							catch (Exception)
							{
								CS$<>8__locals1.maxBlockSize = CS$<>8__locals1.<>4__this.DefaultMaxBlockSize;
							}
							CS$<>8__locals1.maxBlockSize -= 2U;
							if (CS$<>8__locals1.maxBlockSize > CS$<>8__locals1.<>4__this.DatasetUploadMaxBlockSize)
							{
								CS$<>8__locals1.maxBlockSize = CS$<>8__locals1.<>4__this.DatasetUploadMaxBlockSize;
							}
							List<OBDRequest> list2 = new List<OBDRequest>();
							MemoryStream memoryStream = new MemoryStream(CS$<>8__locals1.dataToWrite);
							memoryStream.Seek(0L, SeekOrigin.Begin);
							int num3 = 1;
							CS$<>8__locals1.tpReq = new OBDRequest("3E80", "700", "", "", false)
							{
								DoNotDecode = true
							};
							CS$<>8__locals1.tpReq.ResponseDecoded += CS$<>8__locals1.<>4__this.TpReq_ResponseDecoded;
							while (memoryStream.Position < memoryStream.Length)
							{
								MQBParametrizeBase.<>c__DisplayClass119_2 CS$<>8__locals2 = new MQBParametrizeBase.<>c__DisplayClass119_2();
								CS$<>8__locals2.CS$<>8__locals2 = CS$<>8__locals1;
								long position = memoryStream.Position;
								uint num4 = CS$<>8__locals1.maxBlockSize;
								if ((ulong)num4 > (ulong)(memoryStream.Length - memoryStream.Position))
								{
									num4 = (uint)memoryStream.Length - (uint)memoryStream.Position;
								}
								byte[] array2 = new byte[num4];
								memoryStream.Read(array2, 0, array2.Length);
								CS$<>8__locals2.positionFinish = memoryStream.Position;
								CS$<>8__locals2.totalLength = memoryStream.Length;
								OBDRequest obdrequest4 = new OBDRequest("36" + num3.ToString("X2") + BitHelpers.ByteArrayToHexString(array2), CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false);
								obdrequest4.ResponseReceived += delegate(OBDRequest stepreq2, string stepdata)
								{
									App.OBDReader.NO_DATA_Counter = 0;
									CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.writeWasStarted = true;
									if (stepdata != null)
									{
										string text3 = OBDDataReader.FilterHexAndNewLineOnly(stepdata);
										if (text3.Contains("7F3678") && !text3.Contains("76"))
										{
											Task.Delay(500).Wait();
										}
										if ((!text3.Contains("7F3678") && text3.Contains("037F36")) || stepdata.Contains("NO DATA"))
										{
											CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
											List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
											queueCopy2.RemoveAll((OBDRequest x) => x.Command.StartsWith("36"));
											App.OBDReader.ReplaceQueue(queueCopy2);
										}
									}
									IProgress<string> progress2 = CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.progress;
									if (progress2 == null)
									{
										return;
									}
									progress2.Report(Translate.GetString("coding_progress_ApplyingNewData") + string.Format("\n{0} / {1}", CS$<>8__locals2.positionFinish, CS$<>8__locals2.totalLength));
								};
								CS$<>8__locals2.progressString = Translate.GetString("coding_progress_ApplyingNewData") + string.Format("\n{0} / {1}\n", position, CS$<>8__locals2.totalLength);
								obdrequest4.Progress = new Progress<string>(delegate(string writeProgress)
								{
									IProgress<string> progress3 = CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.progress;
									if (progress3 == null)
									{
										return;
									}
									progress3.Report(CS$<>8__locals2.progressString + writeProgress);
								});
								obdrequest4.Keys.Add("TesterPresent", "ATSH700;023E80;ATSH" + obdrequest4.Header);
								list2.Add(obdrequest4);
								list2.Add(CS$<>8__locals2.CS$<>8__locals2.tpReq);
								num3++;
							}
							OBDRequest obdrequest5 = list2.LastOrDefault((OBDRequest x) => x != CS$<>8__locals1.tpReq);
							ResponseReceivedDelegate responseReceivedDelegate;
							if ((responseReceivedDelegate = CS$<>8__locals1.<>9__5) == null)
							{
								responseReceivedDelegate = (CS$<>8__locals1.<>9__5 = delegate(OBDRequest lastRequest2, string lastRequestData)
								{
									App.OBDReader.NO_DATA_Counter = 0;
									if (lastRequestData != null)
									{
										string text4 = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
										if (text4.Contains("7F3678") || text4.Contains("76"))
										{
											IProgress<string> progress4 = CS$<>8__locals1.progress;
											if (progress4 != null)
											{
												progress4.Report(Translate.GetString("coding_progress_DataAccepted"));
											}
											CS$<>8__locals1.requestResult = CodingRequestResult.Success;
											return;
										}
										if (!text4.Contains("76") && !text4.Contains("037F3478"))
										{
											if (text4.Length >= 11 && text4.Contains("7F36"))
											{
												int num5 = text4.IndexOf("7F36");
												int num6 = int.Parse(text4.Substring(num5 + 4, 2), NumberStyles.HexNumber);
												if (num6 >= 128 || num6 == 34)
												{
													CS$<>8__locals1.requestResult = CodingRequestResult.WrongConditions;
													return;
												}
												if (num6 == 51)
												{
													CS$<>8__locals1.requestResult = CodingRequestResult.WrongAccessKey;
													return;
												}
												if (num6 == 49)
												{
													CS$<>8__locals1.requestResult = CodingRequestResult.NotSupported;
													return;
												}
												CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
												return;
											}
											else
											{
												CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
											}
										}
									}
								});
							}
							obdrequest5.ResponseReceived += responseReceivedDelegate;
							List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
							list2.AddRange(queueCopy);
							IProgress<string> progress = CS$<>8__locals1.progress;
							if (progress != null)
							{
								progress.Report(Translate.GetString("coding_progress_ApplyingNewData"));
							}
							App.OBDReader.ReplaceQueue(list2);
						};
						obdrequest.ResponseReceived += delegate(OBDRequest initiateWriteRequest2, string response)
						{
							if (response == null || response.Contains("NO DATA") || response.Contains("ERROR"))
							{
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.requestResult = CodingRequestResult.NoData;
								return;
							}
							string text5 = OBDDataReader.FilterHexAndNewLineOnly(response);
							if (!text5.Contains("74") && !text5.Contains("037F3478"))
							{
								if (text5.Length >= 11 && text5.Contains("7F34"))
								{
									int num7 = text5.IndexOf("7F34");
									int num8 = int.Parse(text5.Substring(num7 + 4, 2), NumberStyles.HexNumber);
									if (num8 >= 128 || num8 == 34)
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.WrongConditions;
									}
									else if (num8 == 51)
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.WrongAccessKey;
									}
									else if (num8 == 49)
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.NotSupported;
									}
									else
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
									}
								}
								else
								{
									CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
								}
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							}
						};
						list.Add(obdrequest);
						OBDRequest obdrequest2 = new OBDRequest("37", mqbparametrizeBase.RequestHeader, mqbparametrizeBase.BeforeCommands + ";ATAT0;ATSTFF", mqbparametrizeBase.AfterCommands + string.Format(";ATAT{0};ATST{1}", SharedSettings.Current.AdaptiveTimings, mqbparametrizeBase.ATST), false);
						obdrequest2.ResponseReceived += delegate(OBDRequest finishTransferRequest2, string response)
						{
							if (response != null)
							{
								string[] array3 = OBDDataReader.FilterHexAndNewLineOnly(response).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
								if (array3.Length == 1 && array3[0].Contains("037F3778"))
								{
									Task.Delay(1000).Wait();
								}
							}
						};
						list.Add(obdrequest2);
						obdrequest2.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
						obdrequest2.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
						List<OBDRequest> requestsFromStringCommands = MQBParametrizeBase.GetRequestsFromStringCommands(mqbparametrizeBase.PostWriteCommands, password, mqbparametrizeBase.RequestHeader, mqbparametrizeBase.BeforeCommands, mqbparametrizeBase.AfterCommands);
						List<OBDRequest>.Enumerator enumerator = requestsFromStringCommands.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								OBDRequest obdrequest3 = enumerator.Current;
								if (obdrequest3.Command.StartsWith("2E"))
								{
									obdrequest3.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
									obdrequest3.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						list.AddRange(requestsFromStringCommands);
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBParametrizeBase.<WriteDataToECU>d__119>(ref taskAwaiter, ref this);
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
					if ((CS$<>8__locals1.requestResult == CodingRequestResult.Success) | CS$<>8__locals1.writeWasStarted)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("AddressFormatLength", mqbparametrizeBase.AddressFormatLength.ToString("X2"));
						dictionary.Add("DataLengthFormatLength", mqbparametrizeBase.DataLengthFormatLength.ToString("X2"));
						dictionary.Add("DataLength", mqbparametrizeBase.DataLength.ToString("X2"));
						if (originalData != null)
						{
							CodingLogItem.RecordToLog(mqbparametrizeBase.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQBParametrizeBase, mqbparametrizeBase.Address.ToString("X2"), BitHelpers.ByteArrayToHexString(originalData), checked_value, password, mqbparametrizeBase.RequestHeader, mqbparametrizeBase.ResponseHeader, mqbparametrizeBase.PreReadCommands, mqbparametrizeBase.PreWriteCommands, mqbparametrizeBase.PostWriteCommands, mqbparametrizeBase.ATST, dictionary, "");
						}
					}
					requestResult = CS$<>8__locals1.requestResult;
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
				this.<>t__builder.SetResult(requestResult);
			}

			// Token: 0x06004C2D RID: 19501 RVA: 0x0038796C File Offset: 0x00385B6C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002CD0 RID: 11472
			public int <>1__state;

			// Token: 0x04002CD1 RID: 11473
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002CD2 RID: 11474
			public MQBParametrizeBase <>4__this;

			// Token: 0x04002CD3 RID: 11475
			public IProgress<string> progress;

			// Token: 0x04002CD4 RID: 11476
			public string checked_value;

			// Token: 0x04002CD5 RID: 11477
			public string password;

			// Token: 0x04002CD6 RID: 11478
			private MQBParametrizeBase.<>c__DisplayClass119_0 <>8__1;

			// Token: 0x04002CD7 RID: 11479
			public byte[] originalData;

			// Token: 0x04002CD8 RID: 11480
			public string UserFriendlyValue;

			// Token: 0x04002CD9 RID: 11481
			private TaskAwaiter <>u__1;
		}
	}
}
