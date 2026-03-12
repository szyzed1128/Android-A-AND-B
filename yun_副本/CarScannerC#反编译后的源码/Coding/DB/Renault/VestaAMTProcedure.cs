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
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A13 RID: 2579
	internal class VestaAMTProcedure : ICodingContainer, INotifyPropertyChanged, IServiceProcedure
	{
		// Token: 0x06005235 RID: 21045 RVA: 0x003F7B2C File Offset: 0x003F5D2C
		public VestaAMTProcedure(string id, string name, string description, string innerDescription)
		{
			this.Group = CodingGroup.ServiceProcedures;
			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.InnerDescription = innerDescription;
			this.startOption = new MQBAdaptationOption("1. Start", "31" + this.ID + "00", new TranslationItem[]
			{
				new TranslationItem("ru", "1. Запуск", "", "")
			});
			this.stopOption = new MQBAdaptationOption("1. Start", "32" + this.ID, new TranslationItem[]
			{
				new TranslationItem("ru", "2. Прервать", "", "")
			});
			this.Options.Add(this.startOption);
			this.Options.Add(this.stopOption);
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x06005236 RID: 21046 RVA: 0x003F7C58 File Offset: 0x003F5E58
		// (set) Token: 0x06005237 RID: 21047 RVA: 0x003F7C60 File Offset: 0x003F5E60
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
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x06005238 RID: 21048 RVA: 0x003F7C69 File Offset: 0x003F5E69
		// (set) Token: 0x06005239 RID: 21049 RVA: 0x003F7C71 File Offset: 0x003F5E71
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

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x0600523A RID: 21050 RVA: 0x003F7C7A File Offset: 0x003F5E7A
		// (set) Token: 0x0600523B RID: 21051 RVA: 0x003F7C82 File Offset: 0x003F5E82
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

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x0600523C RID: 21052 RVA: 0x003F7C8B File Offset: 0x003F5E8B
		// (set) Token: 0x0600523D RID: 21053 RVA: 0x003F7C93 File Offset: 0x003F5E93
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

		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x0600523E RID: 21054 RVA: 0x003F7C9C File Offset: 0x003F5E9C
		// (set) Token: 0x0600523F RID: 21055 RVA: 0x003F7CA4 File Offset: 0x003F5EA4
		public string CurrentState
		{
			get
			{
				return this._CurrenState;
			}
			set
			{
				this._CurrenState = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("CurrentState"));
			}
		}

		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x06005240 RID: 21056 RVA: 0x00002076 File Offset: 0x00000276
		public bool PasswordVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x06005241 RID: 21057 RVA: 0x003F7CC8 File Offset: 0x003F5EC8
		// (set) Token: 0x06005242 RID: 21058 RVA: 0x003F7CD0 File Offset: 0x003F5ED0
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

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x06005243 RID: 21059 RVA: 0x003F7CD9 File Offset: 0x003F5ED9
		// (set) Token: 0x06005244 RID: 21060 RVA: 0x003F7CE1 File Offset: 0x003F5EE1
		public string Password
		{
			[CompilerGenerated]
			get
			{
				return this.<Password>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Password>k__BackingField = value;
			}
		}

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x06005245 RID: 21061 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string PasswordHint
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x06005246 RID: 21062 RVA: 0x003F7CEA File Offset: 0x003F5EEA
		// (set) Token: 0x06005247 RID: 21063 RVA: 0x003F7CF2 File Offset: 0x003F5EF2
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

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x06005248 RID: 21064 RVA: 0x003F7CFB File Offset: 0x003F5EFB
		// (set) Token: 0x06005249 RID: 21065 RVA: 0x003F7D03 File Offset: 0x003F5F03
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

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x0600524A RID: 21066 RVA: 0x00002076 File Offset: 0x00000276
		public AdaptationValueTypes ValueType
		{
			get
			{
				return AdaptationValueTypes.OptionType;
			}
		}

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x0600524B RID: 21067 RVA: 0x003F7D0C File Offset: 0x003F5F0C
		// (remove) Token: 0x0600524C RID: 21068 RVA: 0x003F7D44 File Offset: 0x003F5F44
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

		// Token: 0x0600524D RID: 21069 RVA: 0x003F7D7C File Offset: 0x003F5F7C
		public async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult result = CodingRequestResult.UnknownError;
			string text = string.Concat(new string[] { "ATSH", this.RequestHeader, ";ATFCSH", this.RequestHeader, ";ATFCSD300000;ATFCSM1;ATST", this.ATST, ";ATCRA", this.ResponseHeader });
			string text2 = "ATAR;ATFCSM0";
			OBDRequest openSessionReq = new OBDRequest("10C0", this.RequestHeader, text, text2, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			OBDRequest resetReq = new OBDRequest("1102", this.RequestHeader, text, text2, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			resetReq.ResponseReceived += this.ResetReq_ResponseReceived;
			OBDRequest stopReq = new OBDRequest("32" + this.ID, this.RequestHeader, text, text2, false);
			if (value == this.startOption.Value)
			{
				this.StopRequested = false;
				OBDRequest obdrequest = new OBDRequest(value, this.RequestHeader, text, text2, false)
				{
					ELMFormat = ELMFormat.CAN11bit
				};
				OBDRequest continueReq = new OBDRequest("31" + this.ID + "01", this.RequestHeader, text, text2, false)
				{
					ELMFormat = ELMFormat.CAN11bit
				};
				string negativePattern = "037F" + obdrequest.Command.Substring(0, 2);
				negativePattern + "78";
				string continueWithRepeatPattern = "0371" + this.ID + "01";
				string finishedSuccessPattern = "0271" + this.ID;
				string negativeCode03Pattern = "0371" + this.ID + "03";
				string finishedWithCode02Pattern = "0371" + this.ID + "02";
				Func<string, bool> <>9__1;
				Func<string, bool> <>9__2;
				Func<string, bool> <>9__3;
				Func<string, bool> <>9__4;
				Func<string, bool> <>9__5;
				ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest senderRequest, string data)
				{
					if (this.StopRequested)
					{
						return;
					}
					if (senderRequest.Command.EndsWith("01"))
					{
						Task.Delay(250).Wait();
					}
					if (data == null)
					{
						result = CodingRequestResult.NoData;
					}
					if (data.Contains("NO DATA") || data.Contains("ERROR"))
					{
						result = CodingRequestResult.NoData;
					}
					string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					IEnumerable<string> enumerable = array;
					Func<string, bool> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = (string line) => line.Contains(finishedSuccessPattern) || line.Contains(finishedWithCode02Pattern));
					}
					if (enumerable.Any(func))
					{
						IProgress<string> progress2 = progress;
						if (progress2 != null)
						{
							progress2.Report("Операция завершена");
						}
						result = CodingRequestResult.Success;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { stopReq, resetReq, resetReq });
						return;
					}
					IEnumerable<string> enumerable2 = array;
					Func<string, bool> func2;
					if ((func2 = <>9__2) == null)
					{
						func2 = (<>9__2 = (string line) => line.Contains(continueWithRepeatPattern));
					}
					if (enumerable2.Any(func2))
					{
						IProgress<string> progress3 = progress;
						if (progress3 != null)
						{
							progress3.Report("Операция выполняется");
						}
						App.OBDReader.ReplaceQueue(new OBDRequest[] { continueReq });
						return;
					}
					IEnumerable<string> enumerable3 = array;
					Func<string, bool> func3;
					if ((func3 = <>9__3) == null)
					{
						func3 = (<>9__3 = (string line) => line.Contains(negativeCode03Pattern));
					}
					if (enumerable3.Any(func3))
					{
						IProgress<string> progress4 = progress;
						if (progress4 != null)
						{
							progress4.Report("Выполнение операции невозможно");
						}
						result = CodingRequestResult.UnknownError;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						return;
					}
					IEnumerable<string> enumerable4 = array;
					Func<string, bool> func4;
					if ((func4 = <>9__4) == null)
					{
						func4 = (<>9__4 = (string line) => line.Contains(negativePattern));
					}
					if (enumerable4.Any(func4))
					{
						IEnumerable<string> enumerable5 = array;
						Func<string, bool> func5;
						if ((func5 = <>9__5) == null)
						{
							func5 = (<>9__5 = (string line) => line.Contains(negativePattern));
						}
						string text3 = enumerable5.FirstOrDefault(func5);
						int num = text3.IndexOf(negativePattern);
						string text4 = text3.Substring(num, 8);
						string text5 = text4.Substring(text4.Length - 2);
						IProgress<string> progress5 = progress;
						if (progress5 != null)
						{
							progress5.Report("Выполнение операции невозможно\nКод ошибки: " + text5);
						}
						result = CodingRequestResult.UnknownError;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					}
				};
				obdrequest.ResponseReceived += responseReceivedDelegate;
				continueReq.ResponseReceived += responseReceivedDelegate;
				App.OBDReader.ReplaceQueue(new OBDRequest[] { openSessionReq, obdrequest });
				await App.OBDReader.WaitForCommandQueue();
			}
			if (value == this.stopOption.Value)
			{
				this.StopRequested = true;
				App.OBDReader.ReplaceQueue(new OBDRequest[] { openSessionReq, stopReq });
				await App.OBDReader.WaitForCommandQueue();
				result = CodingRequestResult.Success;
			}
			return result;
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x003F7DD0 File Offset: 0x003F5FD0
		private void ResetReq_ResponseReceived(OBDRequest request, string data)
		{
			Task.Delay(1000).Wait();
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x003F7DE4 File Offset: 0x003F5FE4
		public virtual async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			return CodingRequestResult.Success;
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x003F7E20 File Offset: 0x003F6020
		public static ICodingContainer[] VestaAMTProcedures()
		{
			string text = "\nВНИМАНИЕ! Помните, что все процедуры вы выполняете на свой страх и риск и помните, что ответственность за все негативные последствия несете только Вы!";
			VestaAMTProcedure vestaAMTProcedure = new VestaAMTProcedure("22", "1. Процедура обучения коробки передач (для коробки передач АМТ)", "", "Функция обучения коробки передач измеряет и сохраняет значения конечных положений актуатора переключения передачи для каждой передачи и положений актуатора выбора передачи для каждой нейтрали линии выбора передачи.\nВ процессе обучения также осуществляется проверка достоверности конечных положений актуатора.\nСтарт процедуры обучения проводится независимо от фактического положения актуатора.\nДлительность процедуры: около 1 минуты.\nПосле завершения процедуры необходимо выключить зажигание и подождать не менее 60 секунд (до отключения главного реле) для корректного сохранения результатов в памяти блока управления!\nУсловия для выполнения процедуры:\n-напряжение АКБ > 10 В;\n-температура электроприводов актуатора< 130 oC;\n-зажигание включено;\n-автомобиль находится в неподвижном состоянии;\n-коробка передач в нейтральном положении;\n-двигатель не работает." + text);
			VestaClutchKissPointAdaptation vestaClutchKissPointAdaptation = new VestaClutchKissPointAdaptation();
			vestaClutchKissPointAdaptation.InnerDescription += text;
			VestaAMTProcedure vestaAMTProcedure2 = new VestaAMTProcedure("18", "2. Процедура обучения конечных положений сцепления (для коробки передач АМТ)", "", "Функция обучения конечных положений сцепления измеряет и сохраняет значения положений актуатора выключения сцепления, соответствующие выключенному и полностью включенному сцеплению.\nДлительность процедуры: около 5 секунд.\nПосле завершения процедуры необходимо выключить зажигание и подождать не менее 60 секунд (до отключения главного реле) для корректного сохранения результатов в памяти блока управления!\nУсловия для выполнения процедуры:\n-напряжение АКБ > 10 В;\n-зажигание включено;\n-автомобиль находится в неподвижном состоянии;\n-коробка передач в нейтральном положении;\n-двигатель не работает." + text);
			VestaAMTProcedure vestaAMTProcedure3 = new VestaAMTProcedure("25", "Сброс счетчиков защиты сцепления", "", text);
			CustomizableCodingTemplate customizableCodingTemplate = new CustomizableCodingTemplate
			{
				Name = "Перезагрузка блока управления роботизированной коробки передач",
				Description = "",
				InnerDescription = text,
				Options = 
				{
					new MQBAdaptationOption("Сброс", "02")
				},
				ReadModeAndAddress = "",
				WriteModeAndAddress = "11",
				HasCurrentState = false,
				PreWriteCommands = "10C0",
				RequestHeader = "7E1",
				ResponseHeader = "7E9",
				PasswordVisible = false,
				ValueType = AdaptationValueTypes.OptionType
			};
			return new ICodingContainer[] { vestaAMTProcedure, vestaClutchKissPointAdaptation, vestaAMTProcedure2, vestaAMTProcedure3, customizableCodingTemplate };
		}

		// Token: 0x0400321A RID: 12826
		protected string RequestHeader = "7E1";

		// Token: 0x0400321B RID: 12827
		protected string ResponseHeader = "7E9";

		// Token: 0x0400321C RID: 12828
		protected string ATST = "FA";

		// Token: 0x0400321D RID: 12829
		protected string ID = "";

		// Token: 0x0400321E RID: 12830
		private MQBAdaptationOption startOption;

		// Token: 0x0400321F RID: 12831
		private MQBAdaptationOption stopOption;

		// Token: 0x04003220 RID: 12832
		[CompilerGenerated]
		private CodingGroup <Group>k__BackingField;

		// Token: 0x04003221 RID: 12833
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04003222 RID: 12834
		[CompilerGenerated]
		private string <Description>k__BackingField;

		// Token: 0x04003223 RID: 12835
		[CompilerGenerated]
		private string <InnerDescription>k__BackingField;

		// Token: 0x04003224 RID: 12836
		private string _CurrenState = "";

		// Token: 0x04003225 RID: 12837
		[CompilerGenerated]
		private bool <HasCurrentState>k__BackingField;

		// Token: 0x04003226 RID: 12838
		[CompilerGenerated]
		private string <Password>k__BackingField;

		// Token: 0x04003227 RID: 12839
		protected bool StopRequested;

		// Token: 0x04003228 RID: 12840
		[CompilerGenerated]
		private ObservableCollection<MQBAdaptationOption> <Options>k__BackingField;

		// Token: 0x04003229 RID: 12841
		[CompilerGenerated]
		private bool <RequiresPro>k__BackingField;

		// Token: 0x0400322A RID: 12842
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x02000A14 RID: 2580
		[CompilerGenerated]
		private sealed class <>c__DisplayClass53_0
		{
			// Token: 0x06005251 RID: 21073 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass53_0()
			{
			}

			// Token: 0x06005252 RID: 21074 RVA: 0x003F7F48 File Offset: 0x003F6148
			internal void <Execute>b__0(OBDRequest senderRequest, string data)
			{
				if (this.<>4__this.StopRequested)
				{
					return;
				}
				if (senderRequest.Command.EndsWith("01"))
				{
					Task.Delay(250).Wait();
				}
				if (data == null)
				{
					this.result = CodingRequestResult.NoData;
				}
				if (data.Contains("NO DATA") || data.Contains("ERROR"))
				{
					this.result = CodingRequestResult.NoData;
				}
				string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				IEnumerable<string> enumerable = array;
				Func<string, bool> func;
				if ((func = this.<>9__1) == null)
				{
					func = (this.<>9__1 = (string line) => line.Contains(this.finishedSuccessPattern) || line.Contains(this.finishedWithCode02Pattern));
				}
				if (enumerable.Any(func))
				{
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report("Операция завершена");
					}
					this.result = CodingRequestResult.Success;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.stopReq, this.resetReq, this.resetReq });
					return;
				}
				IEnumerable<string> enumerable2 = array;
				Func<string, bool> func2;
				if ((func2 = this.<>9__2) == null)
				{
					func2 = (this.<>9__2 = (string line) => line.Contains(this.continueWithRepeatPattern));
				}
				if (enumerable2.Any(func2))
				{
					IProgress<string> progress2 = this.progress;
					if (progress2 != null)
					{
						progress2.Report("Операция выполняется");
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.continueReq });
					return;
				}
				IEnumerable<string> enumerable3 = array;
				Func<string, bool> func3;
				if ((func3 = this.<>9__3) == null)
				{
					func3 = (this.<>9__3 = (string line) => line.Contains(this.negativeCode03Pattern));
				}
				if (enumerable3.Any(func3))
				{
					IProgress<string> progress3 = this.progress;
					if (progress3 != null)
					{
						progress3.Report("Выполнение операции невозможно");
					}
					this.result = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					return;
				}
				IEnumerable<string> enumerable4 = array;
				Func<string, bool> func4;
				if ((func4 = this.<>9__4) == null)
				{
					func4 = (this.<>9__4 = (string line) => line.Contains(this.negativePattern));
				}
				if (enumerable4.Any(func4))
				{
					IEnumerable<string> enumerable5 = array;
					Func<string, bool> func5;
					if ((func5 = this.<>9__5) == null)
					{
						func5 = (this.<>9__5 = (string line) => line.Contains(this.negativePattern));
					}
					string text = enumerable5.FirstOrDefault(func5);
					int num = text.IndexOf(this.negativePattern);
					string text2 = text.Substring(num, 8);
					string text3 = text2.Substring(text2.Length - 2);
					IProgress<string> progress4 = this.progress;
					if (progress4 != null)
					{
						progress4.Report("Выполнение операции невозможно\nКод ошибки: " + text3);
					}
					this.result = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			}

			// Token: 0x06005253 RID: 21075 RVA: 0x003F818B File Offset: 0x003F638B
			internal bool <Execute>b__1(string line)
			{
				return line.Contains(this.finishedSuccessPattern) || line.Contains(this.finishedWithCode02Pattern);
			}

			// Token: 0x06005254 RID: 21076 RVA: 0x003F81A9 File Offset: 0x003F63A9
			internal bool <Execute>b__2(string line)
			{
				return line.Contains(this.continueWithRepeatPattern);
			}

			// Token: 0x06005255 RID: 21077 RVA: 0x003F81B7 File Offset: 0x003F63B7
			internal bool <Execute>b__3(string line)
			{
				return line.Contains(this.negativeCode03Pattern);
			}

			// Token: 0x06005256 RID: 21078 RVA: 0x003F81C5 File Offset: 0x003F63C5
			internal bool <Execute>b__4(string line)
			{
				return line.Contains(this.negativePattern);
			}

			// Token: 0x06005257 RID: 21079 RVA: 0x003F81C5 File Offset: 0x003F63C5
			internal bool <Execute>b__5(string line)
			{
				return line.Contains(this.negativePattern);
			}

			// Token: 0x0400322B RID: 12843
			public VestaAMTProcedure <>4__this;

			// Token: 0x0400322C RID: 12844
			public CodingRequestResult result;

			// Token: 0x0400322D RID: 12845
			public IProgress<string> progress;

			// Token: 0x0400322E RID: 12846
			public OBDRequest stopReq;

			// Token: 0x0400322F RID: 12847
			public OBDRequest resetReq;

			// Token: 0x04003230 RID: 12848
			public string finishedSuccessPattern;

			// Token: 0x04003231 RID: 12849
			public string finishedWithCode02Pattern;

			// Token: 0x04003232 RID: 12850
			public string continueWithRepeatPattern;

			// Token: 0x04003233 RID: 12851
			public OBDRequest continueReq;

			// Token: 0x04003234 RID: 12852
			public string negativeCode03Pattern;

			// Token: 0x04003235 RID: 12853
			public string negativePattern;

			// Token: 0x04003236 RID: 12854
			public Func<string, bool> <>9__1;

			// Token: 0x04003237 RID: 12855
			public Func<string, bool> <>9__2;

			// Token: 0x04003238 RID: 12856
			public Func<string, bool> <>9__3;

			// Token: 0x04003239 RID: 12857
			public Func<string, bool> <>9__4;

			// Token: 0x0400323A RID: 12858
			public Func<string, bool> <>9__5;
		}

		// Token: 0x02000A15 RID: 2581
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__53 : IAsyncStateMachine
		{
			// Token: 0x06005258 RID: 21080 RVA: 0x003F81D4 File Offset: 0x003F63D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VestaAMTProcedure vestaAMTProcedure = this;
				CodingRequestResult result;
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
							goto IL_03B4;
						}
						CS$<>8__locals1 = new VestaAMTProcedure.<>c__DisplayClass53_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.result = CodingRequestResult.UnknownError;
						string text = string.Concat(new string[] { "ATSH", vestaAMTProcedure.RequestHeader, ";ATFCSH", vestaAMTProcedure.RequestHeader, ";ATFCSD300000;ATFCSM1;ATST", vestaAMTProcedure.ATST, ";ATCRA", vestaAMTProcedure.ResponseHeader });
						string text2 = "ATAR;ATFCSM0";
						openSessionReq = new OBDRequest("10C0", vestaAMTProcedure.RequestHeader, text, text2, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						CS$<>8__locals1.resetReq = new OBDRequest("1102", vestaAMTProcedure.RequestHeader, text, text2, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						CS$<>8__locals1.resetReq.ResponseReceived += vestaAMTProcedure.ResetReq_ResponseReceived;
						CS$<>8__locals1.stopReq = new OBDRequest("32" + vestaAMTProcedure.ID, vestaAMTProcedure.RequestHeader, text, text2, false);
						if (!(value == vestaAMTProcedure.startOption.Value))
						{
							goto IL_0310;
						}
						vestaAMTProcedure.StopRequested = false;
						OBDRequest obdrequest = new OBDRequest(value, vestaAMTProcedure.RequestHeader, text, text2, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						CS$<>8__locals1.continueReq = new OBDRequest("31" + vestaAMTProcedure.ID + "01", vestaAMTProcedure.RequestHeader, text, text2, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						CS$<>8__locals1.negativePattern = "037F" + obdrequest.Command.Substring(0, 2);
						CS$<>8__locals1.negativePattern + "78";
						CS$<>8__locals1.continueWithRepeatPattern = "0371" + vestaAMTProcedure.ID + "01";
						CS$<>8__locals1.finishedSuccessPattern = "0271" + vestaAMTProcedure.ID;
						CS$<>8__locals1.negativeCode03Pattern = "0371" + vestaAMTProcedure.ID + "03";
						CS$<>8__locals1.finishedWithCode02Pattern = "0371" + vestaAMTProcedure.ID + "02";
						ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest senderRequest, string data)
						{
							if (CS$<>8__locals1.<>4__this.StopRequested)
							{
								return;
							}
							if (senderRequest.Command.EndsWith("01"))
							{
								Task.Delay(250).Wait();
							}
							if (data == null)
							{
								CS$<>8__locals1.result = CodingRequestResult.NoData;
							}
							if (data.Contains("NO DATA") || data.Contains("ERROR"))
							{
								CS$<>8__locals1.result = CodingRequestResult.NoData;
							}
							string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
							IEnumerable<string> enumerable = array;
							Func<string, bool> func;
							if ((func = CS$<>8__locals1.<>9__1) == null)
							{
								func = (CS$<>8__locals1.<>9__1 = (string line) => line.Contains(CS$<>8__locals1.finishedSuccessPattern) || line.Contains(CS$<>8__locals1.finishedWithCode02Pattern));
							}
							if (enumerable.Any(func))
							{
								IProgress<string> progress = CS$<>8__locals1.progress;
								if (progress != null)
								{
									progress.Report("Операция завершена");
								}
								CS$<>8__locals1.result = CodingRequestResult.Success;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals1.stopReq, CS$<>8__locals1.resetReq, CS$<>8__locals1.resetReq });
								return;
							}
							IEnumerable<string> enumerable2 = array;
							Func<string, bool> func2;
							if ((func2 = CS$<>8__locals1.<>9__2) == null)
							{
								func2 = (CS$<>8__locals1.<>9__2 = (string line) => line.Contains(CS$<>8__locals1.continueWithRepeatPattern));
							}
							if (enumerable2.Any(func2))
							{
								IProgress<string> progress2 = CS$<>8__locals1.progress;
								if (progress2 != null)
								{
									progress2.Report("Операция выполняется");
								}
								App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals1.continueReq });
								return;
							}
							IEnumerable<string> enumerable3 = array;
							Func<string, bool> func3;
							if ((func3 = CS$<>8__locals1.<>9__3) == null)
							{
								func3 = (CS$<>8__locals1.<>9__3 = (string line) => line.Contains(CS$<>8__locals1.negativeCode03Pattern));
							}
							if (enumerable3.Any(func3))
							{
								IProgress<string> progress3 = CS$<>8__locals1.progress;
								if (progress3 != null)
								{
									progress3.Report("Выполнение операции невозможно");
								}
								CS$<>8__locals1.result = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								return;
							}
							IEnumerable<string> enumerable4 = array;
							Func<string, bool> func4;
							if ((func4 = CS$<>8__locals1.<>9__4) == null)
							{
								func4 = (CS$<>8__locals1.<>9__4 = (string line) => line.Contains(CS$<>8__locals1.negativePattern));
							}
							if (enumerable4.Any(func4))
							{
								IEnumerable<string> enumerable5 = array;
								Func<string, bool> func5;
								if ((func5 = CS$<>8__locals1.<>9__5) == null)
								{
									func5 = (CS$<>8__locals1.<>9__5 = (string line) => line.Contains(CS$<>8__locals1.negativePattern));
								}
								string text3 = enumerable5.FirstOrDefault(func5);
								int num3 = text3.IndexOf(CS$<>8__locals1.negativePattern);
								string text4 = text3.Substring(num3, 8);
								string text5 = text4.Substring(text4.Length - 2);
								IProgress<string> progress4 = CS$<>8__locals1.progress;
								if (progress4 != null)
								{
									progress4.Report("Выполнение операции невозможно\nКод ошибки: " + text5);
								}
								CS$<>8__locals1.result = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							}
						};
						obdrequest.ResponseReceived += responseReceivedDelegate;
						CS$<>8__locals1.continueReq.ResponseReceived += responseReceivedDelegate;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { openSessionReq, obdrequest });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VestaAMTProcedure.<Execute>d__53>(ref taskAwaiter, ref this);
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
					IL_0310:
					if (!(value == vestaAMTProcedure.stopOption.Value))
					{
						goto IL_03C7;
					}
					vestaAMTProcedure.StopRequested = true;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { openSessionReq, CS$<>8__locals1.stopReq });
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VestaAMTProcedure.<Execute>d__53>(ref taskAwaiter, ref this);
						return;
					}
					IL_03B4:
					taskAwaiter.GetResult();
					CS$<>8__locals1.result = CodingRequestResult.Success;
					IL_03C7:
					result = CS$<>8__locals1.result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					openSessionReq = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				openSessionReq = null;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x06005259 RID: 21081 RVA: 0x003F861C File Offset: 0x003F681C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400323B RID: 12859
			public int <>1__state;

			// Token: 0x0400323C RID: 12860
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400323D RID: 12861
			public VestaAMTProcedure <>4__this;

			// Token: 0x0400323E RID: 12862
			public IProgress<string> progress;

			// Token: 0x0400323F RID: 12863
			public string value;

			// Token: 0x04003240 RID: 12864
			private VestaAMTProcedure.<>c__DisplayClass53_0 <>8__1;

			// Token: 0x04003241 RID: 12865
			private OBDRequest <openSessionReq>5__2;

			// Token: 0x04003242 RID: 12866
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000A16 RID: 2582
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__55 : IAsyncStateMachine
		{
			// Token: 0x0600525A RID: 21082 RVA: 0x003F862C File Offset: 0x003F682C
			void IAsyncStateMachine.MoveNext()
			{
				CodingRequestResult codingRequestResult;
				try
				{
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

			// Token: 0x0600525B RID: 21083 RVA: 0x003F8678 File Offset: 0x003F6878
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003243 RID: 12867
			public int <>1__state;

			// Token: 0x04003244 RID: 12868
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;
		}
	}
}
