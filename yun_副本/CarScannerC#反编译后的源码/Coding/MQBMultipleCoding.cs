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
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008B6 RID: 2230
	public class MQBMultipleCoding : ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06004B8D RID: 19341 RVA: 0x003848AC File Offset: 0x00382AAC
		public MQBMultipleCoding(CodingGroup Group, string Name, string Description, bool OnlyOnOption, params ICodingContainer[] Codings)
		{
			this.Group = Group;
			this.Name = Name;
			this.Description = Description;
			this.codings = Codings;
			this.Options.Add(MQBAdaptationTemplate.EnableOption);
			if (!OnlyOnOption)
			{
				this.Options.Add(MQBAdaptationTemplate.DisableOption);
			}
		}

		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x06004B8E RID: 19342 RVA: 0x00384966 File Offset: 0x00382B66
		// (set) Token: 0x06004B8F RID: 19343 RVA: 0x0038496E File Offset: 0x00382B6E
		public bool ShouldUpdateInternalNames
		{
			[CompilerGenerated]
			get
			{
				return this.<ShouldUpdateInternalNames>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShouldUpdateInternalNames>k__BackingField = value;
			}
		} = true;

		// Token: 0x06004B90 RID: 19344 RVA: 0x00384978 File Offset: 0x00382B78
		private void UpdateInternalNames()
		{
			if (this.ShouldUpdateInternalNames)
			{
				for (int i = 0; i < this.codings.Length; i++)
				{
					this.codings[i].Name = string.Concat(new string[]
					{
						this.Name,
						" ",
						Translate.GetString("coding_Step"),
						" #",
						(i + 1).ToString()
					});
				}
			}
		}

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06004B91 RID: 19345 RVA: 0x003849EC File Offset: 0x00382BEC
		// (remove) Token: 0x06004B92 RID: 19346 RVA: 0x00384A24 File Offset: 0x00382C24
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

		// Token: 0x06004B93 RID: 19347 RVA: 0x00384A59 File Offset: 0x00382C59
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x06004B94 RID: 19348 RVA: 0x00384A74 File Offset: 0x00382C74
		// (set) Token: 0x06004B95 RID: 19349 RVA: 0x00384ABC File Offset: 0x00382CBC
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

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x06004B96 RID: 19350 RVA: 0x00384AD0 File Offset: 0x00382CD0
		// (set) Token: 0x06004B97 RID: 19351 RVA: 0x00384B18 File Offset: 0x00382D18
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

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x06004B98 RID: 19352 RVA: 0x00384B2C File Offset: 0x00382D2C
		// (set) Token: 0x06004B99 RID: 19353 RVA: 0x00384B74 File Offset: 0x00382D74
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

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x06004B9A RID: 19354 RVA: 0x00384B88 File Offset: 0x00382D88
		// (set) Token: 0x06004B9B RID: 19355 RVA: 0x00384B90 File Offset: 0x00382D90
		public List<TranslationItem> Translations
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
		} = new List<TranslationItem>();

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x06004B9C RID: 19356 RVA: 0x00384B99 File Offset: 0x00382D99
		// (set) Token: 0x06004B9D RID: 19357 RVA: 0x00384BA1 File Offset: 0x00382DA1
		public string CurrentState
		{
			get
			{
				return this._CurrentState;
			}
			protected set
			{
				this._CurrentState = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("CurrentState"));
			}
		}

		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x06004B9E RID: 19358 RVA: 0x00384BC5 File Offset: 0x00382DC5
		// (set) Token: 0x06004B9F RID: 19359 RVA: 0x00384BCD File Offset: 0x00382DCD
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

		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x00384BD6 File Offset: 0x00382DD6
		// (set) Token: 0x06004BA1 RID: 19361 RVA: 0x00384BDE File Offset: 0x00382DDE
		public string Password
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

		// Token: 0x1700170B RID: 5899
		// (get) Token: 0x06004BA2 RID: 19362 RVA: 0x00384BF2 File Offset: 0x00382DF2
		// (set) Token: 0x06004BA3 RID: 19363 RVA: 0x00384BFA File Offset: 0x00382DFA
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
		} = "";

		// Token: 0x1700170C RID: 5900
		// (get) Token: 0x06004BA4 RID: 19364 RVA: 0x00002076 File Offset: 0x00000276
		public AdaptationValueTypes ValueType
		{
			get
			{
				return AdaptationValueTypes.OptionType;
			}
		}

		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x06004BA5 RID: 19365 RVA: 0x00384C03 File Offset: 0x00382E03
		// (set) Token: 0x06004BA6 RID: 19366 RVA: 0x00384C0B File Offset: 0x00382E0B
		public ObservableCollection<MQBAdaptationOption> Options
		{
			[CompilerGenerated]
			get
			{
				return this.<Options>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Options>k__BackingField = value;
			}
		} = new ObservableCollection<MQBAdaptationOption>();

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x06004BA7 RID: 19367 RVA: 0x00384C14 File Offset: 0x00382E14
		// (set) Token: 0x06004BA8 RID: 19368 RVA: 0x00384C1C File Offset: 0x00382E1C
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

		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x06004BA9 RID: 19369 RVA: 0x00384C25 File Offset: 0x00382E25
		// (set) Token: 0x06004BAA RID: 19370 RVA: 0x00384C2D File Offset: 0x00382E2D
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
		}

		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x06004BAB RID: 19371 RVA: 0x00384C36 File Offset: 0x00382E36
		// (set) Token: 0x06004BAC RID: 19372 RVA: 0x00384C3E File Offset: 0x00382E3E
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
		} = true;

		// Token: 0x06004BAD RID: 19373 RVA: 0x00384C48 File Offset: 0x00382E48
		public virtual async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			this.UpdateInternalNames();
			int step = 1;
			ICodingContainer[] array = this.codings;
			for (int i = 0; i < array.Length; i++)
			{
				ICodingContainer coding = array[i];
				string step_string = string.Format(Translate.GetString("coding_progress_step"), step, this.codings.Length);
				step_string = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + step_string;
				IProgress<string> progress2 = progress;
				if (progress2 != null)
				{
					progress2.Report(step_string);
				}
				Progress<string> progress3 = new Progress<string>();
				progress3.ProgressChanged += delegate(object sender, string internalString)
				{
					string text2 = step_string + "\n" + internalString;
					IProgress<string> progress4 = progress;
					if (progress4 == null)
					{
						return;
					}
					progress4.Report(text2);
				};
				string text = "";
				if (!string.IsNullOrEmpty(coding.Password))
				{
					text = coding.Password;
				}
				else if (!string.IsNullOrEmpty(password))
				{
					text = password;
				}
				CodingRequestResult codingRequestResult = await coding.Execute(text, value, UserFriendlyValue, progress3, null, skipIfTheSameData);
				if (codingRequestResult != CodingRequestResult.Success && (codingRequestResult != CodingRequestResult.WrongDevice || !(coding is MQBEasyCodingItem) || !(coding as MQBEasyCodingItem).SkipOnWrongDevice) && (codingRequestResult == CodingRequestResult.Success || !(coding is MQBEasyCodingItem) || !(coding as MQBEasyCodingItem).SkipOnWrongDevice))
				{
					return codingRequestResult;
				}
				step++;
				await Task.Delay(3500);
				coding = null;
			}
			array = null;
			return CodingRequestResult.Success;
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x00384CB8 File Offset: 0x00382EB8
		public async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.UpdateInternalNames();
			int counter = 1;
			ICodingContainer[] array = this.codings;
			int i = 0;
			while (i < array.Length)
			{
				ICodingContainer coding = array[i];
				string step_password = "";
				if (!string.IsNullOrEmpty(coding.Password))
				{
					step_password = coding.Password;
				}
				else if (!string.IsNullOrEmpty(password))
				{
					step_password = password;
				}
				CodingRequestResult codingRequestResult = await coding.UpdateCurrentState("", null);
				if (codingRequestResult == CodingRequestResult.WrongAccessKey)
				{
					codingRequestResult = await coding.UpdateCurrentState(step_password, null);
				}
				CodingRequestResult codingRequestResult2;
				if (codingRequestResult != CodingRequestResult.Success)
				{
					if ((codingRequestResult != CodingRequestResult.WrongDevice || !(coding is MQBEasyCodingItem) || !(coding as MQBEasyCodingItem).SkipOnWrongDevice) && (codingRequestResult == CodingRequestResult.Success || !(coding is MQBEasyCodingItem) || !(coding as MQBEasyCodingItem).SkipOnWrongDevice))
					{
						if (codingRequestResult == CodingRequestResult.NotSupported || codingRequestResult == CodingRequestResult.InitialDataIncorrect || codingRequestResult == CodingRequestResult.InitialDataEmpty)
						{
							this.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						}
						else
						{
							this.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						}
					}
					codingRequestResult2 = codingRequestResult;
				}
				else
				{
					if (!(coding.CurrentState == MQBAdaptationTemplate.DisableOption.Title))
					{
						counter++;
						step_password = null;
						coding = null;
						i++;
						continue;
					}
					this.CurrentState = MQBAdaptationTemplate.DisableOption.Title;
					codingRequestResult2 = CodingRequestResult.Success;
				}
				return codingRequestResult2;
			}
			array = null;
			this.CurrentState = MQBAdaptationTemplate.EnableOption.Title;
			return CodingRequestResult.Success;
		}

		// Token: 0x04002C43 RID: 11331
		[CompilerGenerated]
		private bool <ShouldUpdateInternalNames>k__BackingField;

		// Token: 0x04002C44 RID: 11332
		private ICodingContainer[] codings;

		// Token: 0x04002C45 RID: 11333
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002C46 RID: 11334
		private string _Name = "";

		// Token: 0x04002C47 RID: 11335
		private string _Description = "";

		// Token: 0x04002C48 RID: 11336
		private string _InnerDescription = "";

		// Token: 0x04002C49 RID: 11337
		[CompilerGenerated]
		private List<TranslationItem> <Translations>k__BackingField;

		// Token: 0x04002C4A RID: 11338
		private string _CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;

		// Token: 0x04002C4B RID: 11339
		[CompilerGenerated]
		private bool <PasswordVisible>k__BackingField;

		// Token: 0x04002C4C RID: 11340
		private string _Password = "";

		// Token: 0x04002C4D RID: 11341
		[CompilerGenerated]
		private string <PasswordHint>k__BackingField;

		// Token: 0x04002C4E RID: 11342
		[CompilerGenerated]
		private ObservableCollection<MQBAdaptationOption> <Options>k__BackingField;

		// Token: 0x04002C4F RID: 11343
		[CompilerGenerated]
		private CodingGroup <Group>k__BackingField;

		// Token: 0x04002C50 RID: 11344
		[CompilerGenerated]
		private bool <RequiresPro>k__BackingField;

		// Token: 0x04002C51 RID: 11345
		[CompilerGenerated]
		private bool <HasCurrentState>k__BackingField;

		// Token: 0x020008B7 RID: 2231
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004BAF RID: 19375 RVA: 0x00384D03 File Offset: 0x00382F03
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004BB0 RID: 19376 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004BB1 RID: 19377 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Name>b__12_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004BB2 RID: 19378 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Description>b__16_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004BB3 RID: 19379 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_InnerDescription>b__20_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x04002C52 RID: 11346
			public static readonly MQBMultipleCoding.<>c <>9 = new MQBMultipleCoding.<>c();

			// Token: 0x04002C53 RID: 11347
			public static Func<TranslationItem, bool> <>9__12_0;

			// Token: 0x04002C54 RID: 11348
			public static Func<TranslationItem, bool> <>9__16_0;

			// Token: 0x04002C55 RID: 11349
			public static Func<TranslationItem, bool> <>9__20_0;
		}

		// Token: 0x020008B8 RID: 2232
		[CompilerGenerated]
		private sealed class <>c__DisplayClass61_0
		{
			// Token: 0x06004BB4 RID: 19380 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass61_0()
			{
			}

			// Token: 0x04002C56 RID: 11350
			public IProgress<string> progress;
		}

		// Token: 0x020008B9 RID: 2233
		[CompilerGenerated]
		private sealed class <>c__DisplayClass61_1
		{
			// Token: 0x06004BB5 RID: 19381 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass61_1()
			{
			}

			// Token: 0x06004BB6 RID: 19382 RVA: 0x00384D10 File Offset: 0x00382F10
			internal void <Execute>b__0(object sender, string internalString)
			{
				string text = this.step_string + "\n" + internalString;
				IProgress<string> progress = this.CS$<>8__locals1.progress;
				if (progress == null)
				{
					return;
				}
				progress.Report(text);
			}

			// Token: 0x04002C57 RID: 11351
			public string step_string;

			// Token: 0x04002C58 RID: 11352
			public MQBMultipleCoding.<>c__DisplayClass61_0 CS$<>8__locals1;
		}

		// Token: 0x020008BA RID: 2234
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__61 : IAsyncStateMachine
		{
			// Token: 0x06004BB7 RID: 19383 RVA: 0x00384D48 File Offset: 0x00382F48
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBMultipleCoding mqbmultipleCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<CodingRequestResult> taskAwaiter3;
					if (num != 0)
					{
						if (num != 1)
						{
							CS$<>8__locals1 = new MQBMultipleCoding.<>c__DisplayClass61_0();
							IProgress<string> progress;
							CS$<>8__locals1.progress = progress;
							mqbmultipleCoding.UpdateInternalNames();
							step = 1;
							array = mqbmultipleCoding.codings;
							i = 0;
							goto IL_0294;
						}
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0278;
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					IL_01AF:
					CodingRequestResult result = taskAwaiter3.GetResult();
					if (result != CodingRequestResult.Success && (result != CodingRequestResult.WrongDevice || !(coding is MQBEasyCodingItem) || !(coding as MQBEasyCodingItem).SkipOnWrongDevice) && (result == CodingRequestResult.Success || !(coding is MQBEasyCodingItem) || !(coding as MQBEasyCodingItem).SkipOnWrongDevice))
					{
						codingRequestResult = result;
						goto IL_02D2;
					}
					int num3 = step;
					step = num3 + 1;
					taskAwaiter = Task.Delay(3500).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBMultipleCoding.<Execute>d__61>(ref taskAwaiter, ref this);
						return;
					}
					IL_0278:
					taskAwaiter.GetResult();
					coding = null;
					i++;
					IL_0294:
					if (i >= array.Length)
					{
						array = null;
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						coding = array[i];
						MQBMultipleCoding.<>c__DisplayClass61_1 CS$<>8__locals2 = new MQBMultipleCoding.<>c__DisplayClass61_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.step_string = string.Format(Translate.GetString("coding_progress_step"), step, mqbmultipleCoding.codings.Length);
						CS$<>8__locals2.step_string = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + CS$<>8__locals2.step_string;
						IProgress<string> progress = CS$<>8__locals2.CS$<>8__locals1.progress;
						if (progress != null)
						{
							progress.Report(CS$<>8__locals2.step_string);
						}
						Progress<string> progress2 = new Progress<string>();
						progress2.ProgressChanged += delegate(object sender, string internalString)
						{
							string text2 = CS$<>8__locals2.step_string + "\n" + internalString;
							IProgress<string> progress3 = CS$<>8__locals2.CS$<>8__locals1.progress;
							if (progress3 == null)
							{
								return;
							}
							progress3.Report(text2);
						};
						string text = "";
						if (!string.IsNullOrEmpty(coding.Password))
						{
							text = coding.Password;
						}
						else if (!string.IsNullOrEmpty(password))
						{
							text = password;
						}
						taskAwaiter3 = coding.Execute(text, value, UserFriendlyValue, progress2, null, skipIfTheSameData).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBMultipleCoding.<Execute>d__61>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_01AF;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_02D2:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004BB8 RID: 19384 RVA: 0x00385060 File Offset: 0x00383260
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002C59 RID: 11353
			public int <>1__state;

			// Token: 0x04002C5A RID: 11354
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002C5B RID: 11355
			public IProgress<string> progress;

			// Token: 0x04002C5C RID: 11356
			public MQBMultipleCoding <>4__this;

			// Token: 0x04002C5D RID: 11357
			private MQBMultipleCoding.<>c__DisplayClass61_0 <>8__1;

			// Token: 0x04002C5E RID: 11358
			public string password;

			// Token: 0x04002C5F RID: 11359
			public string value;

			// Token: 0x04002C60 RID: 11360
			public string UserFriendlyValue;

			// Token: 0x04002C61 RID: 11361
			public bool skipIfTheSameData;

			// Token: 0x04002C62 RID: 11362
			private int <step>5__2;

			// Token: 0x04002C63 RID: 11363
			private ICodingContainer[] <>7__wrap2;

			// Token: 0x04002C64 RID: 11364
			private int <>7__wrap3;

			// Token: 0x04002C65 RID: 11365
			private ICodingContainer <coding>5__5;

			// Token: 0x04002C66 RID: 11366
			private TaskAwaiter<CodingRequestResult> <>u__1;

			// Token: 0x04002C67 RID: 11367
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020008BB RID: 2235
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__62 : IAsyncStateMachine
		{
			// Token: 0x06004BB9 RID: 19385 RVA: 0x00385070 File Offset: 0x00383270
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBMultipleCoding mqbmultipleCoding = this;
				CodingRequestResult codingRequestResult2;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (num != 1)
						{
							mqbmultipleCoding.UpdateInternalNames();
							counter = 1;
							array = mqbmultipleCoding.codings;
							i = 0;
							goto IL_0247;
						}
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_016D;
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					IL_00FE:
					CodingRequestResult codingRequestResult = taskAwaiter.GetResult();
					if (codingRequestResult != CodingRequestResult.WrongAccessKey)
					{
						goto IL_0175;
					}
					taskAwaiter = coding.UpdateCurrentState(step_password, null).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBMultipleCoding.<UpdateCurrentState>d__62>(ref taskAwaiter, ref this);
						return;
					}
					IL_016D:
					codingRequestResult = taskAwaiter.GetResult();
					IL_0175:
					if (codingRequestResult != CodingRequestResult.Success)
					{
						if ((codingRequestResult != CodingRequestResult.WrongDevice || !(coding is MQBEasyCodingItem) || !(coding as MQBEasyCodingItem).SkipOnWrongDevice) && (codingRequestResult == CodingRequestResult.Success || !(coding is MQBEasyCodingItem) || !(coding as MQBEasyCodingItem).SkipOnWrongDevice))
						{
							if (codingRequestResult == CodingRequestResult.NotSupported || codingRequestResult == CodingRequestResult.InitialDataIncorrect || codingRequestResult == CodingRequestResult.InitialDataEmpty)
							{
								mqbmultipleCoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							}
							else
							{
								mqbmultipleCoding.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
							}
						}
						codingRequestResult2 = codingRequestResult;
						goto IL_028E;
					}
					if (coding.CurrentState == MQBAdaptationTemplate.DisableOption.Title)
					{
						mqbmultipleCoding.CurrentState = MQBAdaptationTemplate.DisableOption.Title;
						codingRequestResult2 = CodingRequestResult.Success;
						goto IL_028E;
					}
					int num3 = counter;
					counter = num3 + 1;
					step_password = null;
					coding = null;
					i++;
					IL_0247:
					if (i >= array.Length)
					{
						array = null;
						mqbmultipleCoding.CurrentState = MQBAdaptationTemplate.EnableOption.Title;
						codingRequestResult2 = CodingRequestResult.Success;
					}
					else
					{
						coding = array[i];
						step_password = "";
						if (!string.IsNullOrEmpty(coding.Password))
						{
							step_password = coding.Password;
						}
						else if (!string.IsNullOrEmpty(password))
						{
							step_password = password;
						}
						taskAwaiter = coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBMultipleCoding.<UpdateCurrentState>d__62>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_00FE;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_028E:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult2);
			}

			// Token: 0x06004BBA RID: 19386 RVA: 0x0038533C File Offset: 0x0038353C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002C68 RID: 11368
			public int <>1__state;

			// Token: 0x04002C69 RID: 11369
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002C6A RID: 11370
			public MQBMultipleCoding <>4__this;

			// Token: 0x04002C6B RID: 11371
			public string password;

			// Token: 0x04002C6C RID: 11372
			private int <counter>5__2;

			// Token: 0x04002C6D RID: 11373
			private ICodingContainer[] <>7__wrap2;

			// Token: 0x04002C6E RID: 11374
			private int <>7__wrap3;

			// Token: 0x04002C6F RID: 11375
			private ICodingContainer <coding>5__5;

			// Token: 0x04002C70 RID: 11376
			private string <step_password>5__6;

			// Token: 0x04002C71 RID: 11377
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}
	}
}
