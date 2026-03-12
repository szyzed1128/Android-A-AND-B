using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008CD RID: 2253
	internal class MQBServiceProcedure : IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x06004C35 RID: 19509 RVA: 0x00387F36 File Offset: 0x00386136
		// (set) Token: 0x06004C36 RID: 19510 RVA: 0x00387F3E File Offset: 0x0038613E
		public MQBAdaptationOption CancelOption
		{
			get
			{
				return this.cancelOption;
			}
			set
			{
				this.cancelOption = value;
			}
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00387F47 File Offset: 0x00386147
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x06004C38 RID: 19512 RVA: 0x00387F60 File Offset: 0x00386160
		// (set) Token: 0x06004C39 RID: 19513 RVA: 0x00387F68 File Offset: 0x00386168
		public CodingGroup Group
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
		} = CodingGroup.ServiceProcedures;

		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x06004C3A RID: 19514 RVA: 0x00387F74 File Offset: 0x00386174
		// (set) Token: 0x06004C3B RID: 19515 RVA: 0x00387FBC File Offset: 0x003861BC
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

		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x06004C3C RID: 19516 RVA: 0x00387FD0 File Offset: 0x003861D0
		// (set) Token: 0x06004C3D RID: 19517 RVA: 0x00388018 File Offset: 0x00386218
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

		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x06004C3E RID: 19518 RVA: 0x0038802C File Offset: 0x0038622C
		// (set) Token: 0x06004C3F RID: 19519 RVA: 0x00388074 File Offset: 0x00386274
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

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x06004C40 RID: 19520 RVA: 0x00388088 File Offset: 0x00386288
		// (set) Token: 0x06004C41 RID: 19521 RVA: 0x00388090 File Offset: 0x00386290
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

		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x06004C42 RID: 19522 RVA: 0x003880A4 File Offset: 0x003862A4
		// (set) Token: 0x06004C43 RID: 19523 RVA: 0x003880AC File Offset: 0x003862AC
		public bool PasswordVisible
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
		}

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x06004C44 RID: 19524 RVA: 0x003880B5 File Offset: 0x003862B5
		// (set) Token: 0x06004C45 RID: 19525 RVA: 0x003880BD File Offset: 0x003862BD
		public bool HasCurrentState
		{
			[CompilerGenerated]
			get
			{
				return this.<HasCurrentState>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HasCurrentState>k__BackingField = value;
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x06004C46 RID: 19526 RVA: 0x003880C6 File Offset: 0x003862C6
		// (set) Token: 0x06004C47 RID: 19527 RVA: 0x003880CE File Offset: 0x003862CE
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

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x06004C48 RID: 19528 RVA: 0x003880E2 File Offset: 0x003862E2
		// (set) Token: 0x06004C49 RID: 19529 RVA: 0x003880EA File Offset: 0x003862EA
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

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x06004C4A RID: 19530 RVA: 0x003880FE File Offset: 0x003862FE
		// (set) Token: 0x06004C4B RID: 19531 RVA: 0x00388106 File Offset: 0x00386306
		public virtual string Unit
		{
			[CompilerGenerated]
			get
			{
				return this.<Unit>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Unit>k__BackingField = value;
			}
		}

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06004C4C RID: 19532 RVA: 0x00388110 File Offset: 0x00386310
		// (remove) Token: 0x06004C4D RID: 19533 RVA: 0x00388148 File Offset: 0x00386348
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

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x06004C4E RID: 19534 RVA: 0x0038817D File Offset: 0x0038637D
		// (set) Token: 0x06004C4F RID: 19535 RVA: 0x00388185 File Offset: 0x00386385
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

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x0038818E File Offset: 0x0038638E
		// (set) Token: 0x06004C51 RID: 19537 RVA: 0x00388196 File Offset: 0x00386396
		public ObservableCollection<MQBAdaptationOption> Options
		{
			[CompilerGenerated]
			get
			{
				return this.<Options>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Options>k__BackingField = value;
			}
		} = new ObservableCollection<MQBAdaptationOption>();

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x06004C52 RID: 19538 RVA: 0x0038819F File Offset: 0x0038639F
		// (set) Token: 0x06004C53 RID: 19539 RVA: 0x003881A7 File Offset: 0x003863A7
		public bool RequiresPro
		{
			[CompilerGenerated]
			get
			{
				return this.<RequiresPro>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequiresPro>k__BackingField = value;
			}
		} = true;

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x06004C54 RID: 19540 RVA: 0x00002076 File Offset: 0x00000276
		public AdaptationValueTypes ValueType
		{
			get
			{
				return AdaptationValueTypes.OptionType;
			}
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x06004C55 RID: 19541 RVA: 0x003881B0 File Offset: 0x003863B0
		// (set) Token: 0x06004C56 RID: 19542 RVA: 0x003881B8 File Offset: 0x003863B8
		public virtual string GetStateMode
		{
			[CompilerGenerated]
			get
			{
				return this.<GetStateMode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GetStateMode>k__BackingField = value;
			}
		} = "22";

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x06004C57 RID: 19543 RVA: 0x003881C1 File Offset: 0x003863C1
		// (set) Token: 0x06004C58 RID: 19544 RVA: 0x003881C9 File Offset: 0x003863C9
		public virtual string GetStateAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<GetStateAddress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GetStateAddress>k__BackingField = value;
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x06004C59 RID: 19545 RVA: 0x003881D2 File Offset: 0x003863D2
		// (set) Token: 0x06004C5A RID: 19546 RVA: 0x003881DA File Offset: 0x003863DA
		public bool SkipOnWrongDevice
		{
			[CompilerGenerated]
			get
			{
				return this.<SkipOnWrongDevice>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SkipOnWrongDevice>k__BackingField = value;
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x06004C5B RID: 19547 RVA: 0x003881E3 File Offset: 0x003863E3
		// (set) Token: 0x06004C5C RID: 19548 RVA: 0x003881EB File Offset: 0x003863EB
		public string RequiredDeviceOrECUItemCode
		{
			[CompilerGenerated]
			get
			{
				return this.<RequiredDeviceOrECUItemCode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequiredDeviceOrECUItemCode>k__BackingField = value;
			}
		} = "";

		// Token: 0x06004C5D RID: 19549 RVA: 0x00017A6F File Offset: 0x00015C6F
		protected virtual string StateToString(string state)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x003881F4 File Offset: 0x003863F4
		protected virtual async Task<CodingRequestResult> OptionExecute(string optionValue, IProgress<string> progress)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			ResponseReceivedDelegate checkReceivedResponseForNegativeResult = delegate(OBDRequest request, string data)
			{
				if (data == null)
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
					return;
				}
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
				{
					int num = data.IndexOf("7F2E");
					int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
					if (num2 >= 128 || num2 == 34)
					{
						codingResult = CodingRequestResult.WrongConditions;
					}
					else if (num2 == 51)
					{
						codingResult = CodingRequestResult.WrongAccessKey;
					}
					else if (num2 == 49)
					{
						codingResult = CodingRequestResult.NotSupported;
					}
					else
					{
						codingResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
				}
			};
			ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
			{
				OBDRequest obdrequest4 = new OBDRequest(this.cancelOption.Value, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				if (data == null)
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
					semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
					semaphore.Release();
					return;
				}
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
				{
					int num3 = data.IndexOf("7F2E");
					int num4 = int.Parse(data.Substring(num3 + 4, 2), NumberStyles.HexNumber);
					if (num4 >= 128 || num4 == 34)
					{
						codingResult = CodingRequestResult.WrongConditions;
					}
					else if (num4 == 51)
					{
						codingResult = CodingRequestResult.WrongAccessKey;
					}
					else if (num4 == 49)
					{
						codingResult = CodingRequestResult.NotSupported;
					}
					else
					{
						codingResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
					semaphore.Release();
				}
			};
			if (optionValue != this.cancelOption.Value)
			{
				bool waitingForFinishedState = true;
				OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest2 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				obdrequest.ResponseReceived += responseReceivedDelegate;
				obdrequest2.ResponseReceived += checkReceivedResponseForNegativeResult;
				obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					progress.Report(MQBServiceProcedure.Status0102ByteToString(data[0]));
					if (waitingForFinishedState && data[0] == 16)
					{
						progress.Report(Translate.GetString("coding_OperationFinished"));
						codingResult = CodingRequestResult.Success;
						semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					}
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
				await semaphore.WaitAsync();
			}
			if (optionValue == this.cancelOption.Value)
			{
				OBDRequest obdrequest3 = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
				obdrequest3.ResponseReceived += delegate(OBDRequest request, string data)
				{
					progress.Report(Translate.GetString("coding_OperationFinished"));
					codingResult = CodingRequestResult.Success;
					semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest3 });
				await semaphore.WaitAsync();
			}
			return codingResult;
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x00388248 File Offset: 0x00386448
		public virtual async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
			return CodingRequestResult.Success;
		}

		// Token: 0x06004C60 RID: 19552 RVA: 0x0038828B File Offset: 0x0038648B
		protected virtual void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "ATFCSH" + VagUnitHelper.GetRequestHeaderForMQBUnit(this.Unit) + ";ATFCSD300000;ATFCSM1;ATAL;ATCRA" + VagUnitHelper.GetResponseHeaderForMQBUnit(this.Unit);
			this.AfterCommands = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0";
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x06004C61 RID: 19553 RVA: 0x003882C3 File Offset: 0x003864C3
		// (set) Token: 0x06004C62 RID: 19554 RVA: 0x003882CB File Offset: 0x003864CB
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

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x06004C63 RID: 19555 RVA: 0x003882D4 File Offset: 0x003864D4
		// (set) Token: 0x06004C64 RID: 19556 RVA: 0x003882DC File Offset: 0x003864DC
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

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x06004C65 RID: 19557 RVA: 0x003882E5 File Offset: 0x003864E5
		public virtual string RequestHeader
		{
			get
			{
				return VagUnitHelper.GetRequestHeaderForMQBUnit(this.Unit);
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x06004C66 RID: 19558 RVA: 0x003882F2 File Offset: 0x003864F2
		// (set) Token: 0x06004C67 RID: 19559 RVA: 0x003882FA File Offset: 0x003864FA
		public string Device
		{
			[CompilerGenerated]
			get
			{
				return this.<Device>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Device>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x06004C68 RID: 19560 RVA: 0x00388303 File Offset: 0x00386503
		// (set) Token: 0x06004C69 RID: 19561 RVA: 0x0038830B File Offset: 0x0038650B
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
		} = "";

		// Token: 0x06004C6A RID: 19562 RVA: 0x00388314 File Offset: 0x00386514
		public async Task RequestDeviceIdentsAsync()
		{
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest("22F187", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			obdrequest.ResponseDecoded += delegate(OBDRequest device_request2, byte[] data, bool result, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					string @string = Encoding.ASCII.GetString(data);
					StringBuilder stringBuilder = new StringBuilder(@string.Length);
					foreach (char c in @string)
					{
						if (char.IsLetterOrDigit(c))
						{
							stringBuilder.Append(c);
						}
					}
					this.Device = stringBuilder.ToString().ToUpperInvariant();
				}
			};
			OBDRequest obdrequest2 = new OBDRequest("22F189", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest2.ResponseDecoded += delegate(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					string string2 = Encoding.ASCII.GetString(data);
					StringBuilder stringBuilder2 = new StringBuilder(string2.Length);
					foreach (char c2 in string2)
					{
						if (char.IsLetterOrDigit(c2))
						{
							stringBuilder2.Append(c2);
						}
					}
					this.ECU = stringBuilder2.ToString().ToUpperInvariant();
				}
				semaphore.Release();
			};
			OBDRequest[] array = new OBDRequest[] { obdrequest, obdrequest2 };
			OBDRequest[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].ELMFormat = ELMFormat.CAN11bit;
			}
			App.OBDReader.ReplaceQueue(array);
			await semaphore.WaitAsync();
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x00388358 File Offset: 0x00386558
		private bool MatchPattern(string str, string pattern)
		{
			return new Regex("^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$", RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(str);
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x003883A8 File Offset: 0x003865A8
		public bool CheckDevice(string device)
		{
			if (string.IsNullOrEmpty(device))
			{
				return true;
			}
			device = "*" + device.ToUpperInvariant() + "*";
			return this.MatchPattern(this.ECU.ToUpperInvariant(), device) || this.MatchPattern(this.Device.ToUpperInvariant(), device);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x00388404 File Offset: 0x00386604
		public async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			MQBServiceProcedure.<>c__DisplayClass103_0 CS$<>8__locals1 = new MQBServiceProcedure.<>c__DisplayClass103_0();
			CS$<>8__locals1.progress = progress;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.value = value;
			this.BuildDefaultBeforeAndAfterCommands();
			await this.RequestDeviceIdentsAsync();
			CodingRequestResult codingRequestResult;
			if (!string.IsNullOrEmpty(this.RequiredDeviceOrECUItemCode) && !this.CheckDevice(this.RequiredDeviceOrECUItemCode))
			{
				codingRequestResult = CodingRequestResult.WrongDevice;
			}
			else
			{
				CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
				this.BuildDefaultBeforeAndAfterCommands();
				CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
				CS$<>8__locals1.req_setDate = new OBDRequest(string.Format("2EF199{0}{1}{2}", DateTimeNowHelper.NowSafe.Year - 2000, DateTimeNowHelper.NowSafe.Month.ToString("00"), DateTimeNowHelper.NowSafe.Day.ToString("00")), this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				CS$<>8__locals1.req_setDate.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if (data != null && !data.Contains("6EF199"))
					{
						App.OBDReader.DebugWrite("\n[2EF199_error]\n");
					}
				};
				OBDRequest obdrequest = new OBDRequest("22F199", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest.ResponseDecoded += delegate(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
				{
					if (getDateData != null && getDateData.Length >= 3)
					{
						string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
						CS$<>8__locals1.req_setDate.Command = text;
					}
				};
				CS$<>8__locals1.req_setCodingSequence = new OBDRequest("2EF198", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				CS$<>8__locals1.req_setCodingSequence.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if (data != null && data.Contains("6EF198"))
					{
						IProgress<string> progress2 = CS$<>8__locals1.progress;
						if (progress2 == null)
						{
							return;
						}
						progress2.Report(Translate.GetString("coding_StartingOperation"));
						return;
					}
					else
					{
						App.OBDReader.DebugWrite("\n[2EF198_error]\n");
						IProgress<string> progress3 = CS$<>8__locals1.progress;
						if (progress3 == null)
						{
							return;
						}
						progress3.Report(Translate.GetString("coding_StartingOperation"));
						return;
					}
				};
				OBDRequest obdrequest2 = new OBDRequest("22F1A5", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest2.ResponseDecoded += delegate(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
				{
					if (getCodingData != null && getCodingData.Length != 0)
					{
						string text2 = BitHelpers.ByteArrayToHexString(getCodingData);
						string text3 = "2EF198" + text2;
						CS$<>8__locals1.req_setCodingSequence.Command = text3;
						return;
					}
					string text4 = "2EF1980181C8F63039";
					CS$<>8__locals1.req_setCodingSequence.Command = text4;
				};
				CS$<>8__locals1.zero_seed = false;
				OBDRequest obdrequest3 = null;
				CS$<>8__locals1.req_sendKey = null;
				CS$<>8__locals1.uint_pass = 0U;
				if (!string.IsNullOrEmpty(password) && uint.TryParse(password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out CS$<>8__locals1.uint_pass))
				{
					CS$<>8__locals1.req_sendKey = new OBDRequest("2704", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
					obdrequest3 = new OBDRequest("2703", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
					obdrequest3.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
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
								CS$<>8__locals1.zero_seed = true;
							}
							if (!CS$<>8__locals1.zero_seed)
							{
								string text5 = (num + CS$<>8__locals1.uint_pass).ToString("X8");
								CS$<>8__locals1.req_sendKey.Command = "2704" + text5;
								IProgress<string> progress4 = CS$<>8__locals1.progress;
								if (progress4 != null)
								{
									progress4.Report(Translate.GetString("coding_progress_SendingPassword"));
								}
							}
							else
							{
								CS$<>8__locals1.req_sendKey.Command = "3E";
							}
						}
						catch (Exception)
						{
							App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
							CS$<>8__locals1.req_sendKey.Command = "3E";
						}
					};
					CS$<>8__locals1.req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if (request.Command != "3E")
						{
							if (data != null)
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
							}
							if ((data != null && data.Contains("6704")) || SharedSettings.Current.IgnoreCodingErrors)
							{
								IProgress<string> progress5 = CS$<>8__locals1.progress;
								if (progress5 == null)
								{
									return;
								}
								progress5.Report(Translate.GetString("coding_progress_SuccessPassword"));
								return;
							}
							else if (!(SharedSettings.Current.IgnoreCodingErrors | CS$<>8__locals1.zero_seed))
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
								IProgress<string> progress6 = CS$<>8__locals1.progress;
								if (progress6 != null)
								{
									progress6.Report(Translate.GetString("coding_progress_WrongPassword"));
								}
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
							}
						}
					};
				}
				OBDRequest obdrequest4 = new OBDRequest("03C00010000301", "200", "ATAL", "", false)
				{
					DoNotDecode = true
				};
				OBDRequest obdrequest5 = new OBDRequest("1003", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest6 = new OBDRequest("1003", "710", "ATFCSH710;ATFCSD300000;ATFCSM1;ATAL;ATCRA77A", this.AfterCommands, false)
				{
					DoNotDecode = true
				};
				List<OBDRequest> list = new List<OBDRequest>();
				list.Add(obdrequest4);
				list.Add(obdrequest6);
				list.Add(obdrequest5);
				list.Add(obdrequest);
				list.Add(obdrequest2);
				if (obdrequest3 != null && CS$<>8__locals1.req_sendKey != null)
				{
					list.Add(obdrequest3);
					list.Add(CS$<>8__locals1.req_sendKey);
				}
				list.Add(CS$<>8__locals1.req_setDate);
				list.Add(CS$<>8__locals1.req_setCodingSequence);
				list[list.Count - 1].ResponseReceived += delegate(OBDRequest request, string data)
				{
					MQBServiceProcedure.<>c__DisplayClass103_0.<<Execute>b__6>d <<Execute>b__6>d;
					<<Execute>b__6>d.<>t__builder = AsyncVoidMethodBuilder.Create();
					<<Execute>b__6>d.<>4__this = CS$<>8__locals1;
					<<Execute>b__6>d.<>1__state = -1;
					<<Execute>b__6>d.<>t__builder.Start<MQBServiceProcedure.<>c__DisplayClass103_0.<<Execute>b__6>d>(ref <<Execute>b__6>d);
				};
				App.OBDReader.ReplaceQueue(list);
				await CS$<>8__locals1.semaphore.WaitAsync();
				codingRequestResult = CS$<>8__locals1.codingResult;
			}
			return codingRequestResult;
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x00388460 File Offset: 0x00386660
		protected string StatusFrom0104ToString(byte[] data)
		{
			if (data == null || data.Length < 2)
			{
				return Translate.GetString("coding_UnknownState");
			}
			string text = ((int)data[0] * 256 + (int)data[1]).ToString("X4");
			string text2 = "coding_mqb_0104_" + text;
			string @string = Translate.GetString(text2);
			if (string.IsNullOrEmpty(@string) || @string == text2)
			{
				return Translate.GetString("coding_UnknownState") + " (" + text + ")";
			}
			return @string;
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x003884DC File Offset: 0x003866DC
		protected static string Status0102ByteToString(byte b)
		{
			if (b <= 64)
			{
				if (b == 0)
				{
					return Translate.GetString("coding_OperationNotStarted");
				}
				if (b == 16)
				{
					return Translate.GetString("coding_OperationFinished");
				}
				if (b == 64)
				{
					return Translate.GetString("coding_OperationCancelledDueToSecurityReasons");
				}
			}
			else
			{
				if (b == 96)
				{
					return Translate.GetString("coding_OperationCancelledDueToMalfunction");
				}
				if (b == 128)
				{
					return Translate.GetString("coding_FinishedTimeout");
				}
				if (b == 192)
				{
					return Translate.GetString("coding_OperationInProgress");
				}
			}
			return Translate.GetString("coding_UnknownState") + " " + b.ToString("X2");
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x00388578 File Offset: 0x00386778
		public MQBServiceProcedure()
		{
		}

		// Token: 0x04002CEB RID: 11499
		protected MQBAdaptationOption cancelOption;

		// Token: 0x04002CEC RID: 11500
		[CompilerGenerated]
		private CodingGroup <Group>k__BackingField;

		// Token: 0x04002CED RID: 11501
		private string _Name = "";

		// Token: 0x04002CEE RID: 11502
		private string _Description = "";

		// Token: 0x04002CEF RID: 11503
		private string _InnerDescription = "";

		// Token: 0x04002CF0 RID: 11504
		private string _CurrentState = "";

		// Token: 0x04002CF1 RID: 11505
		[CompilerGenerated]
		private bool <PasswordVisible>k__BackingField;

		// Token: 0x04002CF2 RID: 11506
		[CompilerGenerated]
		private bool <HasCurrentState>k__BackingField;

		// Token: 0x04002CF3 RID: 11507
		private string _Password = "";

		// Token: 0x04002CF4 RID: 11508
		private string _PasswordHint = "";

		// Token: 0x04002CF5 RID: 11509
		[CompilerGenerated]
		private string <Unit>k__BackingField;

		// Token: 0x04002CF6 RID: 11510
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002CF7 RID: 11511
		[CompilerGenerated]
		private ObservableCollection<TranslationItem> <Translations>k__BackingField;

		// Token: 0x04002CF8 RID: 11512
		[CompilerGenerated]
		private ObservableCollection<MQBAdaptationOption> <Options>k__BackingField;

		// Token: 0x04002CF9 RID: 11513
		[CompilerGenerated]
		private bool <RequiresPro>k__BackingField;

		// Token: 0x04002CFA RID: 11514
		[CompilerGenerated]
		private string <GetStateMode>k__BackingField;

		// Token: 0x04002CFB RID: 11515
		[CompilerGenerated]
		private string <GetStateAddress>k__BackingField;

		// Token: 0x04002CFC RID: 11516
		[CompilerGenerated]
		private bool <SkipOnWrongDevice>k__BackingField;

		// Token: 0x04002CFD RID: 11517
		[CompilerGenerated]
		private string <RequiredDeviceOrECUItemCode>k__BackingField;

		// Token: 0x04002CFE RID: 11518
		[CompilerGenerated]
		private string <BeforeCommands>k__BackingField;

		// Token: 0x04002CFF RID: 11519
		[CompilerGenerated]
		private string <AfterCommands>k__BackingField;

		// Token: 0x04002D00 RID: 11520
		[CompilerGenerated]
		private string <Device>k__BackingField;

		// Token: 0x04002D01 RID: 11521
		[CompilerGenerated]
		private string <ECU>k__BackingField;

		// Token: 0x020008CE RID: 2254
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004C71 RID: 19569 RVA: 0x0038861E File Offset: 0x0038681E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004C72 RID: 19570 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004C73 RID: 19571 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Name>b__10_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004C74 RID: 19572 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Description>b__14_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004C75 RID: 19573 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_InnerDescription>b__18_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004C76 RID: 19574 RVA: 0x0038862A File Offset: 0x0038682A
			internal void <Execute>b__103_0(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && !data.Contains("6EF199"))
				{
					App.OBDReader.DebugWrite("\n[2EF199_error]\n");
				}
			}

			// Token: 0x04002D02 RID: 11522
			public static readonly MQBServiceProcedure.<>c <>9 = new MQBServiceProcedure.<>c();

			// Token: 0x04002D03 RID: 11523
			public static Func<TranslationItem, bool> <>9__10_0;

			// Token: 0x04002D04 RID: 11524
			public static Func<TranslationItem, bool> <>9__14_0;

			// Token: 0x04002D05 RID: 11525
			public static Func<TranslationItem, bool> <>9__18_0;

			// Token: 0x04002D06 RID: 11526
			public static ResponseReceivedDelegate <>9__103_0;
		}

		// Token: 0x020008CF RID: 2255
		[CompilerGenerated]
		private sealed class <>c__DisplayClass100_0
		{
			// Token: 0x06004C77 RID: 19575 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass100_0()
			{
			}

			// Token: 0x06004C78 RID: 19576 RVA: 0x00388658 File Offset: 0x00386858
			internal void <RequestDeviceIdentsAsync>b__0(OBDRequest device_request2, byte[] data, bool result, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					string @string = Encoding.ASCII.GetString(data);
					StringBuilder stringBuilder = new StringBuilder(@string.Length);
					foreach (char c in @string)
					{
						if (char.IsLetterOrDigit(c))
						{
							stringBuilder.Append(c);
						}
					}
					this.<>4__this.Device = stringBuilder.ToString().ToUpperInvariant();
				}
			}

			// Token: 0x06004C79 RID: 19577 RVA: 0x003886C4 File Offset: 0x003868C4
			internal void <RequestDeviceIdentsAsync>b__1(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					string @string = Encoding.ASCII.GetString(data);
					StringBuilder stringBuilder = new StringBuilder(@string.Length);
					foreach (char c in @string)
					{
						if (char.IsLetterOrDigit(c))
						{
							stringBuilder.Append(c);
						}
					}
					this.<>4__this.ECU = stringBuilder.ToString().ToUpperInvariant();
				}
				this.semaphore.Release();
			}

			// Token: 0x04002D07 RID: 11527
			public MQBServiceProcedure <>4__this;

			// Token: 0x04002D08 RID: 11528
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020008D0 RID: 2256
		[CompilerGenerated]
		private sealed class <>c__DisplayClass103_0
		{
			// Token: 0x06004C7A RID: 19578 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass103_0()
			{
			}

			// Token: 0x06004C7B RID: 19579 RVA: 0x0038873C File Offset: 0x0038693C
			internal void <Execute>b__1(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
			{
				if (getDateData != null && getDateData.Length >= 3)
				{
					string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
					this.req_setDate.Command = text;
				}
			}

			// Token: 0x06004C7C RID: 19580 RVA: 0x0038879C File Offset: 0x0038699C
			internal void <Execute>b__2(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && data.Contains("6EF198"))
				{
					IProgress<string> progress = this.progress;
					if (progress == null)
					{
						return;
					}
					progress.Report(Translate.GetString("coding_StartingOperation"));
					return;
				}
				else
				{
					App.OBDReader.DebugWrite("\n[2EF198_error]\n");
					IProgress<string> progress2 = this.progress;
					if (progress2 == null)
					{
						return;
					}
					progress2.Report(Translate.GetString("coding_StartingOperation"));
					return;
				}
			}

			// Token: 0x06004C7D RID: 19581 RVA: 0x0038880C File Offset: 0x00386A0C
			internal void <Execute>b__3(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
			{
				if (getCodingData != null && getCodingData.Length != 0)
				{
					string text = BitHelpers.ByteArrayToHexString(getCodingData);
					string text2 = "2EF198" + text;
					this.req_setCodingSequence.Command = text2;
					return;
				}
				string text3 = "2EF1980181C8F63039";
				this.req_setCodingSequence.Command = text3;
			}

			// Token: 0x06004C7E RID: 19582 RVA: 0x00388854 File Offset: 0x00386A54
			internal void <Execute>b__4(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
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
						this.zero_seed = true;
					}
					if (!this.zero_seed)
					{
						string text = (num + this.uint_pass).ToString("X8");
						this.req_sendKey.Command = "2704" + text;
						IProgress<string> progress = this.progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_SendingPassword"));
						}
					}
					else
					{
						this.req_sendKey.Command = "3E";
					}
				}
				catch (Exception)
				{
					App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
					this.req_sendKey.Command = "3E";
				}
			}

			// Token: 0x06004C7F RID: 19583 RVA: 0x00388918 File Offset: 0x00386B18
			internal void <Execute>b__5(OBDRequest request, string data)
			{
				if (request.Command != "3E")
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if ((data != null && data.Contains("6704")) || SharedSettings.Current.IgnoreCodingErrors)
					{
						IProgress<string> progress = this.progress;
						if (progress == null)
						{
							return;
						}
						progress.Report(Translate.GetString("coding_progress_SuccessPassword"));
						return;
					}
					else if (!(SharedSettings.Current.IgnoreCodingErrors | this.zero_seed))
					{
						this.codingResult = CodingRequestResult.WrongAccessKey;
						IProgress<string> progress2 = this.progress;
						if (progress2 != null)
						{
							progress2.Report(Translate.GetString("coding_progress_WrongPassword"));
						}
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						this.semaphore.Release();
					}
				}
			}

			// Token: 0x06004C80 RID: 19584 RVA: 0x003889D0 File Offset: 0x00386BD0
			internal async void <Execute>b__6(OBDRequest request, string data)
			{
				CodingRequestResult codingRequestResult = await this.<>4__this.OptionExecute(this.value, this.progress);
				this.codingResult = codingRequestResult;
				this.semaphore.Release();
			}

			// Token: 0x04002D09 RID: 11529
			public OBDRequest req_setDate;

			// Token: 0x04002D0A RID: 11530
			public IProgress<string> progress;

			// Token: 0x04002D0B RID: 11531
			public OBDRequest req_setCodingSequence;

			// Token: 0x04002D0C RID: 11532
			public bool zero_seed;

			// Token: 0x04002D0D RID: 11533
			public uint uint_pass;

			// Token: 0x04002D0E RID: 11534
			public OBDRequest req_sendKey;

			// Token: 0x04002D0F RID: 11535
			public CodingRequestResult codingResult;

			// Token: 0x04002D10 RID: 11536
			public SemaphoreSlim semaphore;

			// Token: 0x04002D11 RID: 11537
			public MQBServiceProcedure <>4__this;

			// Token: 0x04002D12 RID: 11538
			public string value;

			// Token: 0x020008D1 RID: 2257
			[StructLayout(LayoutKind.Auto)]
			private struct <<Execute>b__6>d : IAsyncStateMachine
			{
				// Token: 0x06004C81 RID: 19585 RVA: 0x00388A08 File Offset: 0x00386C08
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					MQBServiceProcedure.<>c__DisplayClass103_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = CS$<>8__locals1.<>4__this.OptionExecute(CS$<>8__locals1.value, CS$<>8__locals1.progress).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBServiceProcedure.<>c__DisplayClass103_0.<<Execute>b__6>d>(ref taskAwaiter, ref this);
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
						CS$<>8__locals1.codingResult = result;
						CS$<>8__locals1.semaphore.Release();
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

				// Token: 0x06004C82 RID: 19586 RVA: 0x00388AE4 File Offset: 0x00386CE4
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04002D13 RID: 11539
				public int <>1__state;

				// Token: 0x04002D14 RID: 11540
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x04002D15 RID: 11541
				public MQBServiceProcedure.<>c__DisplayClass103_0 <>4__this;

				// Token: 0x04002D16 RID: 11542
				private TaskAwaiter<CodingRequestResult> <>u__1;
			}
		}

		// Token: 0x020008D2 RID: 2258
		[CompilerGenerated]
		private sealed class <>c__DisplayClass79_0
		{
			// Token: 0x06004C83 RID: 19587 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass79_0()
			{
			}

			// Token: 0x06004C84 RID: 19588 RVA: 0x00388AF4 File Offset: 0x00386CF4
			internal void <OptionExecute>b__0(OBDRequest request, string data)
			{
				if (data == null)
				{
					this.codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					this.codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
					return;
				}
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
				{
					int num = data.IndexOf("7F2E");
					int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
					if (num2 >= 128 || num2 == 34)
					{
						this.codingResult = CodingRequestResult.WrongConditions;
					}
					else if (num2 == 51)
					{
						this.codingResult = CodingRequestResult.WrongAccessKey;
					}
					else if (num2 == 49)
					{
						this.codingResult = CodingRequestResult.NotSupported;
					}
					else
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
				}
			}

			// Token: 0x06004C85 RID: 19589 RVA: 0x00388C2C File Offset: 0x00386E2C
			internal void <OptionExecute>b__1(OBDRequest request, string data)
			{
				OBDRequest obdrequest = new OBDRequest(this.<>4__this.cancelOption.Value, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false);
				if (data == null)
				{
					this.codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
					this.semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					this.codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
					this.semaphore.Release();
					return;
				}
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
				{
					int num = data.IndexOf("7F2E");
					int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
					if (num2 >= 128 || num2 == 34)
					{
						this.codingResult = CodingRequestResult.WrongConditions;
					}
					else if (num2 == 51)
					{
						this.codingResult = CodingRequestResult.WrongAccessKey;
					}
					else if (num2 == 49)
					{
						this.codingResult = CodingRequestResult.NotSupported;
					}
					else
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
					this.semaphore.Release();
				}
			}

			// Token: 0x06004C86 RID: 19590 RVA: 0x00388DA7 File Offset: 0x00386FA7
			internal void <OptionExecute>b__3(OBDRequest request, string data)
			{
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x04002D17 RID: 11543
			public CodingRequestResult codingResult;

			// Token: 0x04002D18 RID: 11544
			public SemaphoreSlim semaphore;

			// Token: 0x04002D19 RID: 11545
			public MQBServiceProcedure <>4__this;

			// Token: 0x04002D1A RID: 11546
			public IProgress<string> progress;
		}

		// Token: 0x020008D3 RID: 2259
		[CompilerGenerated]
		private sealed class <>c__DisplayClass79_1
		{
			// Token: 0x06004C87 RID: 19591 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass79_1()
			{
			}

			// Token: 0x06004C88 RID: 19592 RVA: 0x00388DDC File Offset: 0x00386FDC
			internal void <OptionExecute>b__2(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
					this.CS$<>8__locals1.semaphore.Release();
					return;
				}
				this.CS$<>8__locals1.progress.Report(MQBServiceProcedure.Status0102ByteToString(data[0]));
				if (this.waitingForFinishedState && data[0] == 16)
				{
					this.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
					this.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
					this.CS$<>8__locals1.semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				}
			}

			// Token: 0x04002D1B RID: 11547
			public bool waitingForFinishedState;

			// Token: 0x04002D1C RID: 11548
			public MQBServiceProcedure.<>c__DisplayClass79_0 CS$<>8__locals1;
		}

		// Token: 0x020008D4 RID: 2260
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__103 : IAsyncStateMachine
		{
			// Token: 0x06004C89 RID: 19593 RVA: 0x00388E78 File Offset: 0x00387078
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBServiceProcedure mqbserviceProcedure = this;
				CodingRequestResult codingRequestResult;
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
							goto IL_04C0;
						}
						CS$<>8__locals1 = new MQBServiceProcedure.<>c__DisplayClass103_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.value = value;
						mqbserviceProcedure.BuildDefaultBeforeAndAfterCommands();
						taskAwaiter = mqbserviceProcedure.RequestDeviceIdentsAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBServiceProcedure.<Execute>d__103>(ref taskAwaiter, ref this);
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
					if (!string.IsNullOrEmpty(mqbserviceProcedure.RequiredDeviceOrECUItemCode) && !mqbserviceProcedure.CheckDevice(mqbserviceProcedure.RequiredDeviceOrECUItemCode))
					{
						codingRequestResult = CodingRequestResult.WrongDevice;
						goto IL_04F5;
					}
					CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
					mqbserviceProcedure.BuildDefaultBeforeAndAfterCommands();
					CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
					CS$<>8__locals1.req_setDate = new OBDRequest(string.Format("2EF199{0}{1}{2}", DateTimeNowHelper.NowSafe.Year - 2000, DateTimeNowHelper.NowSafe.Month.ToString("00"), DateTimeNowHelper.NowSafe.Day.ToString("00")), mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
					CS$<>8__locals1.req_setDate.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if (data != null)
						{
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
						}
						if (data != null && !data.Contains("6EF199"))
						{
							App.OBDReader.DebugWrite("\n[2EF199_error]\n");
						}
					};
					OBDRequest obdrequest = new OBDRequest("22F199", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
					obdrequest.ResponseDecoded += delegate(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
					{
						if (getDateData != null && getDateData.Length >= 3)
						{
							string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
							CS$<>8__locals1.req_setDate.Command = text;
						}
					};
					CS$<>8__locals1.req_setCodingSequence = new OBDRequest("2EF198", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
					CS$<>8__locals1.req_setCodingSequence.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if (data != null)
						{
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
						}
						if (data != null && data.Contains("6EF198"))
						{
							IProgress<string> progress = CS$<>8__locals1.progress;
							if (progress == null)
							{
								return;
							}
							progress.Report(Translate.GetString("coding_StartingOperation"));
							return;
						}
						else
						{
							App.OBDReader.DebugWrite("\n[2EF198_error]\n");
							IProgress<string> progress2 = CS$<>8__locals1.progress;
							if (progress2 == null)
							{
								return;
							}
							progress2.Report(Translate.GetString("coding_StartingOperation"));
							return;
						}
					};
					OBDRequest obdrequest2 = new OBDRequest("22F1A5", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
					obdrequest2.ResponseDecoded += delegate(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
					{
						if (getCodingData != null && getCodingData.Length != 0)
						{
							string text2 = BitHelpers.ByteArrayToHexString(getCodingData);
							string text3 = "2EF198" + text2;
							CS$<>8__locals1.req_setCodingSequence.Command = text3;
							return;
						}
						string text4 = "2EF1980181C8F63039";
						CS$<>8__locals1.req_setCodingSequence.Command = text4;
					};
					CS$<>8__locals1.zero_seed = false;
					OBDRequest obdrequest3 = null;
					CS$<>8__locals1.req_sendKey = null;
					CS$<>8__locals1.uint_pass = 0U;
					if (!string.IsNullOrEmpty(password) && uint.TryParse(password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out CS$<>8__locals1.uint_pass))
					{
						CS$<>8__locals1.req_sendKey = new OBDRequest("2704", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
						obdrequest3 = new OBDRequest("2703", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
						obdrequest3.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
						{
							try
							{
								if (BitConverter.IsLittleEndian)
								{
									Array.Reverse<byte>(getSeedData);
								}
								uint num3 = BitConverter.ToUInt32(getSeedData, 0);
								if (num3 == 0U)
								{
									CS$<>8__locals1.zero_seed = true;
								}
								if (!CS$<>8__locals1.zero_seed)
								{
									string text5 = (num3 + CS$<>8__locals1.uint_pass).ToString("X8");
									CS$<>8__locals1.req_sendKey.Command = "2704" + text5;
									IProgress<string> progress3 = CS$<>8__locals1.progress;
									if (progress3 != null)
									{
										progress3.Report(Translate.GetString("coding_progress_SendingPassword"));
									}
								}
								else
								{
									CS$<>8__locals1.req_sendKey.Command = "3E";
								}
							}
							catch (Exception)
							{
								App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
								CS$<>8__locals1.req_sendKey.Command = "3E";
							}
						};
						CS$<>8__locals1.req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (request.Command != "3E")
							{
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
								}
								if ((data != null && data.Contains("6704")) || SharedSettings.Current.IgnoreCodingErrors)
								{
									IProgress<string> progress4 = CS$<>8__locals1.progress;
									if (progress4 == null)
									{
										return;
									}
									progress4.Report(Translate.GetString("coding_progress_SuccessPassword"));
									return;
								}
								else if (!(SharedSettings.Current.IgnoreCodingErrors | CS$<>8__locals1.zero_seed))
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
									IProgress<string> progress5 = CS$<>8__locals1.progress;
									if (progress5 != null)
									{
										progress5.Report(Translate.GetString("coding_progress_WrongPassword"));
									}
									App.OBDReader.ReplaceQueue(new OBDRequest[0]);
									CS$<>8__locals1.semaphore.Release();
								}
							}
						};
					}
					OBDRequest obdrequest4 = new OBDRequest("03C00010000301", "200", "ATAL", "", false)
					{
						DoNotDecode = true
					};
					OBDRequest obdrequest5 = new OBDRequest("1003", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
					OBDRequest obdrequest6 = new OBDRequest("1003", "710", "ATFCSH710;ATFCSD300000;ATFCSM1;ATAL;ATCRA77A", mqbserviceProcedure.AfterCommands, false)
					{
						DoNotDecode = true
					};
					List<OBDRequest> list = new List<OBDRequest>();
					list.Add(obdrequest4);
					list.Add(obdrequest6);
					list.Add(obdrequest5);
					list.Add(obdrequest);
					list.Add(obdrequest2);
					if (obdrequest3 != null && CS$<>8__locals1.req_sendKey != null)
					{
						list.Add(obdrequest3);
						list.Add(CS$<>8__locals1.req_sendKey);
					}
					list.Add(CS$<>8__locals1.req_setDate);
					list.Add(CS$<>8__locals1.req_setCodingSequence);
					list[list.Count - 1].ResponseReceived += delegate(OBDRequest request, string data)
					{
						MQBServiceProcedure.<>c__DisplayClass103_0.<<Execute>b__6>d <<Execute>b__6>d;
						<<Execute>b__6>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<Execute>b__6>d.<>4__this = CS$<>8__locals1;
						<<Execute>b__6>d.<>1__state = -1;
						<<Execute>b__6>d.<>t__builder.Start<MQBServiceProcedure.<>c__DisplayClass103_0.<<Execute>b__6>d>(ref <<Execute>b__6>d);
					};
					App.OBDReader.ReplaceQueue(list);
					taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBServiceProcedure.<Execute>d__103>(ref taskAwaiter, ref this);
						return;
					}
					IL_04C0:
					taskAwaiter.GetResult();
					codingRequestResult = CS$<>8__locals1.codingResult;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04F5:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004C8A RID: 19594 RVA: 0x003893B4 File Offset: 0x003875B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D1D RID: 11549
			public int <>1__state;

			// Token: 0x04002D1E RID: 11550
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002D1F RID: 11551
			public IProgress<string> progress;

			// Token: 0x04002D20 RID: 11552
			public MQBServiceProcedure <>4__this;

			// Token: 0x04002D21 RID: 11553
			public string value;

			// Token: 0x04002D22 RID: 11554
			private MQBServiceProcedure.<>c__DisplayClass103_0 <>8__1;

			// Token: 0x04002D23 RID: 11555
			public string password;

			// Token: 0x04002D24 RID: 11556
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008D5 RID: 2261
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__79 : IAsyncStateMachine
		{
			// Token: 0x06004C8B RID: 19595 RVA: 0x003893C4 File Offset: 0x003875C4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBServiceProcedure mqbserviceProcedure = this;
				CodingRequestResult codingResult;
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
							goto IL_0284;
						}
						CS$<>8__locals1 = new MQBServiceProcedure.<>c__DisplayClass79_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						checkReceivedResponseForNegativeResult = delegate(OBDRequest request, string data)
						{
							if (data == null)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							if (data.Contains("NO DATA"))
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
							{
								int num3 = data.IndexOf("7F2E");
								int num4 = int.Parse(data.Substring(num3 + 4, 2), NumberStyles.HexNumber);
								if (num4 >= 128 || num4 == 34)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
								}
								else if (num4 == 51)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
								}
								else if (num4 == 49)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
								}
								else
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								}
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
							}
						};
						ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
						{
							OBDRequest obdrequest4 = new OBDRequest(CS$<>8__locals1.<>4__this.cancelOption.Value, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false);
							if (data == null)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							if (data.Contains("NO DATA"))
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
							{
								int num5 = data.IndexOf("7F2E");
								int num6 = int.Parse(data.Substring(num5 + 4, 2), NumberStyles.HexNumber);
								if (num6 >= 128 || num6 == 34)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
								}
								else if (num6 == 51)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
								}
								else if (num6 == 49)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
								}
								else
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								}
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
								CS$<>8__locals1.semaphore.Release();
							}
						};
						if (!(optionValue != mqbserviceProcedure.cancelOption.Value))
						{
							goto IL_01B1;
						}
						MQBServiceProcedure.<>c__DisplayClass79_1 CS$<>8__locals2 = new MQBServiceProcedure.<>c__DisplayClass79_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.waitingForFinishedState = true;
						OBDRequest obdrequest = new OBDRequest(optionValue, mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("220102", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, true);
						obdrequest.ResponseReceived += responseReceivedDelegate;
						obdrequest2.ResponseReceived += checkReceivedResponseForNegativeResult;
						obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								return;
							}
							CS$<>8__locals2.CS$<>8__locals1.progress.Report(MQBServiceProcedure.Status0102ByteToString(data[0]));
							if (CS$<>8__locals2.waitingForFinishedState && data[0] == 16)
							{
								CS$<>8__locals2.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								App.OBDReader.ClearRequestQueue();
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
						taskAwaiter = CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBServiceProcedure.<OptionExecute>d__79>(ref taskAwaiter, ref this);
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
					IL_01B1:
					if (!(optionValue == mqbserviceProcedure.cancelOption.Value))
					{
						goto IL_028B;
					}
					OBDRequest obdrequest3 = new OBDRequest(optionValue, mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
					obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
					obdrequest3.ResponseReceived += delegate(OBDRequest request, string data)
					{
						CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
						CS$<>8__locals1.codingResult = CodingRequestResult.Success;
						CS$<>8__locals1.semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					};
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest3 });
					taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBServiceProcedure.<OptionExecute>d__79>(ref taskAwaiter, ref this);
						return;
					}
					IL_0284:
					taskAwaiter.GetResult();
					IL_028B:
					codingResult = CS$<>8__locals1.codingResult;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					checkReceivedResponseForNegativeResult = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				checkReceivedResponseForNegativeResult = null;
				this.<>t__builder.SetResult(codingResult);
			}

			// Token: 0x06004C8C RID: 19596 RVA: 0x003896D0 File Offset: 0x003878D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D25 RID: 11557
			public int <>1__state;

			// Token: 0x04002D26 RID: 11558
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002D27 RID: 11559
			public MQBServiceProcedure <>4__this;

			// Token: 0x04002D28 RID: 11560
			public IProgress<string> progress;

			// Token: 0x04002D29 RID: 11561
			public string optionValue;

			// Token: 0x04002D2A RID: 11562
			private MQBServiceProcedure.<>c__DisplayClass79_0 <>8__1;

			// Token: 0x04002D2B RID: 11563
			private ResponseReceivedDelegate <checkReceivedResponseForNegativeResult>5__2;

			// Token: 0x04002D2C RID: 11564
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008D6 RID: 2262
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestDeviceIdentsAsync>d__100 : IAsyncStateMachine
		{
			// Token: 0x06004C8D RID: 19597 RVA: 0x003896E0 File Offset: 0x003878E0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBServiceProcedure mqbserviceProcedure = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						MQBServiceProcedure.<>c__DisplayClass100_0 CS$<>8__locals1 = new MQBServiceProcedure.<>c__DisplayClass100_0();
						CS$<>8__locals1.<>4__this = this;
						mqbserviceProcedure.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest("22F187", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						obdrequest.ResponseDecoded += delegate(OBDRequest device_request2, byte[] data, bool result, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								string @string = Encoding.ASCII.GetString(data);
								StringBuilder stringBuilder = new StringBuilder(@string.Length);
								foreach (char c in @string)
								{
									if (char.IsLetterOrDigit(c))
									{
										stringBuilder.Append(c);
									}
								}
								CS$<>8__locals1.<>4__this.Device = stringBuilder.ToString().ToUpperInvariant();
							}
						};
						OBDRequest obdrequest2 = new OBDRequest("22F189", mqbserviceProcedure.RequestHeader, mqbserviceProcedure.BeforeCommands, mqbserviceProcedure.AfterCommands, false);
						obdrequest2.ResponseDecoded += delegate(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								string string2 = Encoding.ASCII.GetString(data);
								StringBuilder stringBuilder2 = new StringBuilder(string2.Length);
								foreach (char c2 in string2)
								{
									if (char.IsLetterOrDigit(c2))
									{
										stringBuilder2.Append(c2);
									}
								}
								CS$<>8__locals1.<>4__this.ECU = stringBuilder2.ToString().ToUpperInvariant();
							}
							CS$<>8__locals1.semaphore.Release();
						};
						OBDRequest[] array = new OBDRequest[] { obdrequest, obdrequest2 };
						OBDRequest[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							array2[i].ELMFormat = ELMFormat.CAN11bit;
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBServiceProcedure.<RequestDeviceIdentsAsync>d__100>(ref taskAwaiter, ref this);
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

			// Token: 0x06004C8E RID: 19598 RVA: 0x00389874 File Offset: 0x00387A74
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D2D RID: 11565
			public int <>1__state;

			// Token: 0x04002D2E RID: 11566
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002D2F RID: 11567
			public MQBServiceProcedure <>4__this;

			// Token: 0x04002D30 RID: 11568
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008D7 RID: 2263
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__80 : IAsyncStateMachine
		{
			// Token: 0x06004C8F RID: 19599 RVA: 0x00389884 File Offset: 0x00387A84
			void IAsyncStateMachine.MoveNext()
			{
				MQBServiceProcedure mqbserviceProcedure = this;
				CodingRequestResult codingRequestResult;
				try
				{
					mqbserviceProcedure.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004C90 RID: 19600 RVA: 0x003898E4 File Offset: 0x00387AE4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D31 RID: 11569
			public int <>1__state;

			// Token: 0x04002D32 RID: 11570
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002D33 RID: 11571
			public MQBServiceProcedure <>4__this;
		}
	}
}
