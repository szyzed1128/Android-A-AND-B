using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008E8 RID: 2280
	internal class MQB_LightConfigurationCoding : MQBAdaptationTemplate
	{
		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x06004D08 RID: 19720 RVA: 0x0038B29D File Offset: 0x0038949D
		// (set) Token: 0x06004D09 RID: 19721 RVA: 0x0038B2A5 File Offset: 0x003894A5
		public MQB_LightConfiguration LightConfiguration
		{
			[CompilerGenerated]
			get
			{
				return this.<LightConfiguration>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<LightConfiguration>k__BackingField = value;
			}
		} = new MQB_LightConfiguration();

		// Token: 0x06004D0A RID: 19722 RVA: 0x0038B2B0 File Offset: 0x003894B0
		public MQB_LightConfigurationCoding(string name, string description, string address, params TranslationItem[] translations)
		{
			base.ValueType = AdaptationValueTypes.MQBLightConfiguration;
			base.Address = address;
			base.RequestHeader = "70E";
			base.ResponseHeader = "778";
			this.Password = "31347";
			base.Name = name;
			base.Description = description;
			base.ValueType = AdaptationValueTypes.MQBLightConfiguration;
			base.Group = CodingGroup.ManualLightConfiguration;
			foreach (TranslationItem translationItem in translations)
			{
				base.Translations.Add(translationItem);
			}
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x0038B33C File Offset: 0x0038953C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			CodingRequestResult codingRequestResult = await base.UpdateCurrentState(password, null);
			if (codingRequestResult == CodingRequestResult.Success)
			{
				try
				{
					this.LightConfiguration.LoadFromData(base.CurrentState);
				}
				catch (Exception)
				{
					return CodingRequestResult.InitialDataIncorrect;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x0038B388 File Offset: 0x00389588
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			string text = BitHelpers.ByteArrayToHexString(this.LightConfiguration.ApplyToData());
			return await base.Execute(password, text, UserFriendlyValue, progress, originalData, skipIfTheSameData);
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x0038B3F6 File Offset: 0x003895F6
		public static MQB_LightConfigurationCoding GetForTest()
		{
			return new MQB_LightConfigurationCoding("lamp 0", "lamp 0", "0550", Array.Empty<TranslationItem>());
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x0038B414 File Offset: 0x00389614
		public static MQB_LightConfigurationCoding[] GetLightConfigurationMQB()
		{
			return new MQB_LightConfigurationCoding[]
			{
				new MQB_LightConfigurationCoding("0 BLK VL B36", "Usually front left turn signal", "0550", new TranslationItem[]
				{
					new TranslationItem("ru", "0 BLK VL B36", "Обычно передний левый указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("1 BLK VR B20", "Usually front right turn signal", "0551", new TranslationItem[]
				{
					new TranslationItem("ru", "1 BLK VR B20", "Обычно передний правый указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("2 SL VLB10", "Usually front left side light", "0552", new TranslationItem[]
				{
					new TranslationItem("ru", "2 SL VLB10", "Обычно передний левый габарит", "")
				}),
				new MQB_LightConfigurationCoding("3 SL VRB21", "Usually front right side light", "0553", new TranslationItem[]
				{
					new TranslationItem("ru", "3 SL VRB21", "Обычно передний правый габарит", "")
				}),
				new MQB_LightConfigurationCoding("4 TFL LB4", "Usually front left DRL", "0554", new TranslationItem[]
				{
					new TranslationItem("ru", "4 TFL LB4", "Обычно передний левый ДХО", "")
				}),
				new MQB_LightConfigurationCoding("5 TFL RB32", "Usually front right DRL", "0555", new TranslationItem[]
				{
					new TranslationItem("ru", "5 TFL RB32", "Обычно передний правый ДХО", "")
				}),
				new MQB_LightConfigurationCoding("6 ABL LC5", "Usually low beam left", "0556", new TranslationItem[]
				{
					new TranslationItem("ru", "6 ABL LC5", "Обычно ближний свет слева", "")
				}),
				new MQB_LightConfigurationCoding("7 ABL RB1", "Usually low beam right", "0557", new TranslationItem[]
				{
					new TranslationItem("ru", "7 ABL RB1", "Обычно ближний свет справа", "")
				}),
				new MQB_LightConfigurationCoding("8 FL LB39", "Usually high beam left", "0558", new TranslationItem[]
				{
					new TranslationItem("ru", "8 FL LB39", "Обычно дальний свет слева", "")
				}),
				new MQB_LightConfigurationCoding("9 FL RB2", "Usually high beam right", "0559", new TranslationItem[]
				{
					new TranslationItem("ru", "9 FL RB2", "Обычно дальний свет справа", "")
				}),
				new MQB_LightConfigurationCoding("10 SHUTTER LB23", "Usually low beam left diagnostics", "055A", new TranslationItem[]
				{
					new TranslationItem("ru", "10 SHUTTER LB23", "Обычно диагностика ближнего света слева", "")
				}),
				new MQB_LightConfigurationCoding("11 SHUTTER RB22", "Usually low beam right diagnostics", "055B", new TranslationItem[]
				{
					new TranslationItem("ru", "11 SHUTTER RB22", "Обычно диагностика ближнего света справа", "")
				}),
				new MQB_LightConfigurationCoding("12 NL LB45", "Usually left fog light", "055C", new TranslationItem[]
				{
					new TranslationItem("ru", "12 NL LB45", "Обычно левая ПТФ", "")
				}),
				new MQB_LightConfigurationCoding("13 NL RB5", "Usually right fog light", "055D", new TranslationItem[]
				{
					new TranslationItem("ru", "13 NL RB5", "Обычно правая ПТФ", "")
				}),
				new MQB_LightConfigurationCoding("14 LOCKUNLOCK61", "", "055E", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("15 35 LED Warnblinktaster C48", "", "055F", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("16 BLK SLB35BLK SL KC9", "Usually left part of front DRL (stripes, etc.)", "0560", new TranslationItem[]
				{
					new TranslationItem("ru", "16 BLK SLB35BLK SL KC9", "Обычно левая часть передних ДХО (реснички и т.п.)", "")
				}),
				new MQB_LightConfigurationCoding("17 TFL R BLK SRB3TFL R BLK SR KC3", "Usually right part of front DRL (stripes, etc.)", "0561", new TranslationItem[]
				{
					new TranslationItem("ru", "17 TFL R BLK SRB3TFL R BLK SR KC3", "Обычно правая часть передних ДХО (реснички и т.п.)", "")
				}),
				new MQB_LightConfigurationCoding("18 BLK HLA60", "Usually rear left turn light", "0562", new TranslationItem[]
				{
					new TranslationItem("ru", "18 BLK HLA60", "Обычно задний левый указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("19 BLK HRC31", "Usually rear right turn light", "0563", new TranslationItem[]
				{
					new TranslationItem("ru", "19 BLK HRC31", "Обычно задний правый указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("20 BR LA71", "Usually left stop-signal", "0564", new TranslationItem[]
				{
					new TranslationItem("ru", "20 BR LA71", "Обычно левый стоп-сигнал", "")
				}),
				new MQB_LightConfigurationCoding("21 BR RC8", "Usually right stop-signal", "0565", new TranslationItem[]
				{
					new TranslationItem("ru", "21 BR RC8", "Обычно правый стоп-сигнал", "")
				}),
				new MQB_LightConfigurationCoding("22 BR MA57", "", "0566", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("23 SL HLC10", "Usually rear left side light", "0567", new TranslationItem[]
				{
					new TranslationItem("ru", "23 SL HLC10", "Обычно левый задний габарит", "")
				}),
				new MQB_LightConfigurationCoding("24 SL HRA65", "Usually rear right side light", "0568", new TranslationItem[]
				{
					new TranslationItem("ru", "24 SL HRA65", "Обычно правый задний габарит", "")
				}),
				new MQB_LightConfigurationCoding("25 KZL HA59", "Usually number plate light", "0569", new TranslationItem[]
				{
					new TranslationItem("ru", "25 KZL HA59", "Обычно подсветка номерного знака", "")
				}),
				new MQB_LightConfigurationCoding("26 NSL LA72", "Usually rear left fog light", "056A", new TranslationItem[]
				{
					new TranslationItem("ru", "26 NSL LA72", "Обычно левая задняя ПТФ", "")
				}),
				new MQB_LightConfigurationCoding("27 NSL RC6", "Usually rear right fog light", "056B", new TranslationItem[]
				{
					new TranslationItem("ru", "27 NSL RC6", "Обычно правая задняя ПТФ", "")
				}),
				new MQB_LightConfigurationCoding("28 RFL LC11", "Usually rear left reverse light", "056C", new TranslationItem[]
				{
					new TranslationItem("ru", "28 RFL LC11", "Обычно левый фонарь заднего хода", "")
				}),
				new MQB_LightConfigurationCoding("29 RFL RA64", "Usually rear right reverse light", "056D", new TranslationItem[]
				{
					new TranslationItem("ru", "29 RFL RA64", "Обычно правый фонарь заднего хода", "")
				}),
				new MQB_LightConfigurationCoding("30 FR LC72", "Usually foot light", "056E", new TranslationItem[]
				{
					new TranslationItem("ru", "30 FR LC72", "Обычно освещение пространства для ног", "")
				}),
				new MQB_LightConfigurationCoding("31 AMBL 1C61", "", "056F", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("32 AMBL 2C35", "", "0570", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("33 AMBL 3C36", "", "0571", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("34 AMBL 4C37", "", "0572", Array.Empty<TranslationItem>())
			};
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x0038BB44 File Offset: 0x00389D44
		public static MQB_LightConfigurationCoding[] GetLightConfigurationPQ26()
		{
			return new MQB_LightConfigurationCoding[]
			{
				new MQB_LightConfigurationCoding("0 BLK VL B35", "Usually front left turn signal", "0550", new TranslationItem[]
				{
					new TranslationItem("ru", "0 BLK VL B35", "Обычно передний левый указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("1 BLK VR B23", "Usually front right turn signal", "0551", new TranslationItem[]
				{
					new TranslationItem("ru", "1 BLK VR B23", "Обычно передний правый указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("2 SL VL B22", "Usually front left side light", "0552", new TranslationItem[]
				{
					new TranslationItem("ru", "2 SL VL B22", "Обычно передний левый габарит", "")
				}),
				new MQB_LightConfigurationCoding("3 SL VRB36", "Usually front right side light", "0553", new TranslationItem[]
				{
					new TranslationItem("ru", "3 SL VRB36", "Обычно передний правый габарит", "")
				}),
				new MQB_LightConfigurationCoding("4 TFL LB43", "Usually front left DRL", "0554", new TranslationItem[]
				{
					new TranslationItem("ru", "4 TFL LB43", "Обычно передний левый ДХО", "")
				}),
				new MQB_LightConfigurationCoding("5 TFL RB6", "Usually front right DRL", "0555", new TranslationItem[]
				{
					new TranslationItem("ru", "5 TFL RB6", "Обычно передний правый ДХО", "")
				}),
				new MQB_LightConfigurationCoding("6 ABL LB44", "Usually low beam left", "0556", new TranslationItem[]
				{
					new TranslationItem("ru", "6 ABL LB44", "Обычно ближний свет слева", "")
				}),
				new MQB_LightConfigurationCoding("7 ABL RB5", "Usually low beam right", "0557", new TranslationItem[]
				{
					new TranslationItem("ru", "7 ABL RB5", "Обычно ближний свет справа", "")
				}),
				new MQB_LightConfigurationCoding("8 FL LB42", "Usually high beam left", "0558", new TranslationItem[]
				{
					new TranslationItem("ru", "8 FL L42", "Обычно дальний свет слева", "")
				}),
				new MQB_LightConfigurationCoding("9 FL RB7", "Usually high beam right", "0559", new TranslationItem[]
				{
					new TranslationItem("ru", "9 FL RB7", "Обычно дальний свет справа", "")
				}),
				new MQB_LightConfigurationCoding("10", "", "055A", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("11 WARNBLK TASTER C54", "", "055B", new TranslationItem[]
				{
					new TranslationItem("ru", "11 WARNBLK TASTER C54", "Световая сигнализация", "")
				}),
				new MQB_LightConfigurationCoding("12 NL LB40", "Usually left fog light", "055C", new TranslationItem[]
				{
					new TranslationItem("ru", "12 NL LB40", "Обычно левая ПТФ", "")
				}),
				new MQB_LightConfigurationCoding("13 NL RB3", "Usually right fog light", "055D", new TranslationItem[]
				{
					new TranslationItem("ru", "13 NL RB3", "Обычно правая ПТФ", "")
				}),
				new MQB_LightConfigurationCoding("14 LOCKUNLOCK61", "", "055E", new TranslationItem[]
				{
					new TranslationItem("ru", "14 LOCKUNLOCK61", "Обычно светодиод открытия/закрытия центрального замка ", "")
				}),
				new MQB_LightConfigurationCoding("15 SAFE LED C55", "", "055F", new TranslationItem[]
				{
					new TranslationItem("ru", "15 SAFE LED C55", "Светодиод охраны на двери", "")
				}),
				new MQB_LightConfigurationCoding("16 BLK SLC11", "Left side turn light", "0580", new TranslationItem[]
				{
					new TranslationItem("ru", "16 BLK SLC11", "Левый боковой указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("17 BLK SR A72", "Right side turn light", "0582", new TranslationItem[]
				{
					new TranslationItem("ru", "17 BLK SR A72", "Правый боковой указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("18 BLK HLA71", "Usually rear left turn light", "0583", new TranslationItem[]
				{
					new TranslationItem("ru", "18 BLK HLA71", "Обычно задний левый указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("19 BLK HRC8", "Usually rear right turn light", "0581", new TranslationItem[]
				{
					new TranslationItem("ru", "19 BLK HRC8", "Обычно задний правый указатель поворота", "")
				}),
				new MQB_LightConfigurationCoding("20 BR LA70", "Usually left stop-signal", "0564", new TranslationItem[]
				{
					new TranslationItem("ru", "20 BR LA70", "Обычно левый стоп-сигнал", "")
				}),
				new MQB_LightConfigurationCoding("21 BR RC8", "Usually right stop-signal", "0565", new TranslationItem[]
				{
					new TranslationItem("ru", "21 BR RC8", "Обычно правый стоп-сигнал", "")
				}),
				new MQB_LightConfigurationCoding("22 BR MC9", "Usually 3rd stop-signal", "0566", new TranslationItem[]
				{
					new TranslationItem("ru", "22 BR MC9", "Обычно дополнительный стоп-сигнал", "")
				}),
				new MQB_LightConfigurationCoding("23 SL HLC7", "Usually rear left side light", "0567", new TranslationItem[]
				{
					new TranslationItem("ru", "23 SL HLC7", "Обычно левый задний габарит", "")
				}),
				new MQB_LightConfigurationCoding("24 SL HRA69", "Usually rear right side light", "0568", new TranslationItem[]
				{
					new TranslationItem("ru", "24 SL HRA69", "Обычно правый задний габарит", "")
				}),
				new MQB_LightConfigurationCoding("25 KZL HA60", "Usually number plate light", "0569", new TranslationItem[]
				{
					new TranslationItem("ru", "25 KZL HA60", "Обычно подсветка номерного знака", "")
				}),
				new MQB_LightConfigurationCoding("26 NSL A65", "Usually rear left fog light", "056A", new TranslationItem[]
				{
					new TranslationItem("ru", "26 NSL A65", "Обычно левая задняя ПТФ", "")
				}),
				new MQB_LightConfigurationCoding("27 KL58XS C67", "Terminal 58xs dimmer ", "056B", new TranslationItem[]
				{
					new TranslationItem("ru", "27 KL58XS C67", "Обычно  Terminal 58xs dimmer ", "")
				}),
				new MQB_LightConfigurationCoding("28 RFL C3", "Usually rear left reverse light", "056C", new TranslationItem[]
				{
					new TranslationItem("ru", "28 RFL C3", "Обычно левый фонарь заднего хода", "")
				}),
				new MQB_LightConfigurationCoding("29 KL30G A69", "Usually inside locker lights", "056D", new TranslationItem[]
				{
					new TranslationItem("ru", "29 KL30G A69", "Обычно плафон освещения вещевого ящика", "")
				}),
				new MQB_LightConfigurationCoding("30 INNENLICHT A68", "Usually foot light", "056E", new TranslationItem[]
				{
					new TranslationItem("ru", "30 INNENLICHT A68", "Обычно освещение пространства для ног", "")
				}),
				new MQB_LightConfigurationCoding("31 AMBL 1C65", "", "056F", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("32 AMBL 2C64", "", "0570", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("33 AMBL 3C72", "", "0571", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("34 AMBL 4C71", "", "0572", Array.Empty<TranslationItem>()),
				new MQB_LightConfigurationCoding("35 LED Warnblinktaster C48", "", "055F", Array.Empty<TranslationItem>())
			};
		}

		// Token: 0x06004D10 RID: 19728 RVA: 0x0038C2C9 File Offset: 0x0038A4C9
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<CodingRequestResult> <>n__0(string password, IProgress<string> progress = null)
		{
			return base.UpdateCurrentState(password, progress);
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x003838BA File Offset: 0x00381ABA
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<CodingRequestResult> <>n__1(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			return base.Execute(password, value, UserFriendlyValue, progress, originalData, skipIfTheSameData);
		}

		// Token: 0x04002D77 RID: 11639
		[CompilerGenerated]
		private MQB_LightConfiguration <LightConfiguration>k__BackingField;

		// Token: 0x020008E9 RID: 2281
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__6 : IAsyncStateMachine
		{
			// Token: 0x06004D12 RID: 19730 RVA: 0x0038C2D4 File Offset: 0x0038A4D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQB_LightConfigurationCoding mqb_LightConfigurationCoding = this;
				CodingRequestResult result;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						string text = BitHelpers.ByteArrayToHexString(mqb_LightConfigurationCoding.LightConfiguration.ApplyToData());
						taskAwaiter = mqb_LightConfigurationCoding.<>n__1(password, text, UserFriendlyValue, progress, originalData, skipIfTheSameData).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQB_LightConfigurationCoding.<Execute>d__6>(ref taskAwaiter, ref this);
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
					result = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x06004D13 RID: 19731 RVA: 0x0038C3C0 File Offset: 0x0038A5C0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D78 RID: 11640
			public int <>1__state;

			// Token: 0x04002D79 RID: 11641
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002D7A RID: 11642
			public MQB_LightConfigurationCoding <>4__this;

			// Token: 0x04002D7B RID: 11643
			public string password;

			// Token: 0x04002D7C RID: 11644
			public string UserFriendlyValue;

			// Token: 0x04002D7D RID: 11645
			public IProgress<string> progress;

			// Token: 0x04002D7E RID: 11646
			public byte[] originalData;

			// Token: 0x04002D7F RID: 11647
			public bool skipIfTheSameData;

			// Token: 0x04002D80 RID: 11648
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x020008EA RID: 2282
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__5 : IAsyncStateMachine
		{
			// Token: 0x06004D14 RID: 19732 RVA: 0x0038C3D0 File Offset: 0x0038A5D0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQB_LightConfigurationCoding mqb_LightConfigurationCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = mqb_LightConfigurationCoding.<>n__0(password, null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQB_LightConfigurationCoding.<UpdateCurrentState>d__5>(ref taskAwaiter, ref this);
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
					if (result == CodingRequestResult.Success)
					{
						try
						{
							mqb_LightConfigurationCoding.LightConfiguration.LoadFromData(mqb_LightConfigurationCoding.CurrentState);
						}
						catch (Exception)
						{
							codingRequestResult = CodingRequestResult.InitialDataIncorrect;
							goto IL_00AC;
						}
					}
					codingRequestResult = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00AC:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004D15 RID: 19733 RVA: 0x0038C4BC File Offset: 0x0038A6BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D81 RID: 11649
			public int <>1__state;

			// Token: 0x04002D82 RID: 11650
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002D83 RID: 11651
			public MQB_LightConfigurationCoding <>4__this;

			// Token: 0x04002D84 RID: 11652
			public string password;

			// Token: 0x04002D85 RID: 11653
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}
	}
}
