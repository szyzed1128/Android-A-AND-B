using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B05 RID: 2821
	internal class MQBAisinProcedures : MQBServiceProcedure
	{
		// Token: 0x06005827 RID: 22567 RVA: 0x004209E4 File Offset: 0x0041EBE4
		public MQBAisinProcedures()
		{
			base.Name = "AISIN automatic transmission service procedures";
			base.InnerDescription = "Compatibility: AISIN automatic transmissionsWARNING! This features could be dangerous! In some cases you may require to visit service station after using this. Please think twice before doing it!How to do the procedure:\n1) Apply parking brake, set AT selector to P position, turn on ignition without starting the engine (some procedures may require engine running)\n2) Follow instructions on the screen after procedure was started\n3) Wait until finished.";
			this.Unit = "02";
			this.optDrivingRecordDataClear = new MQBAdaptationOption("1. Driving record data clear (reset adaptation to driving style)", "31010364040000", new TranslationItem[]
			{
				new TranslationItem("ru", "1. Сброс накопленных данных (сброс адаптации к стилю вождения)", "", "")
			});
			this.optBasicSettingsClearTemperature = new MQBAdaptationOption("2. Basic setting and clear of maximum oil temperature and T/C oil", "31010318040000", new TranslationItem[]
			{
				new TranslationItem("ru", "2. Базовая установка АКПП и сброс данных о максимальной температуре трансмиссионной жидкости", "", "")
			});
			this.optBasicSettingsClearMileage = new MQBAdaptationOption("3. Basic setting and clear of the mileage information in DSP interface", "31010318040000", new TranslationItem[]
			{
				new TranslationItem("ru", "3. Базовая установка АКПП и сброс данных о пробеге в DSP-интерфейсе", "", "")
			});
			base.Options.Add(this.optDrivingRecordDataClear);
			base.Options.Add(this.optBasicSettingsClearMileage);
			base.Options.Add(this.optBasicSettingsClearTemperature);
			base.Translations.Add(new TranslationItem("ru", "Сервисные процедуры для АКПП AISIN", "", "")
			{
				AdditionalText = "ВНИМАНИЕ! Использование этих возможностей может быть опасно! В некоторых случаях может потребоваться обслуживание в условиях полноценного сервисного центра. Пожалуйста, подумайте дважды, прежде чем запускать эти процедуры!Порядок проведения:\n1) Поднимите (активируйте) стояночный тормоз, переведите селектор АКПП в положение P, включите зажигание, не запуская двигатель (для некоторых процедур может потребоваться запуск двигателя)Запустите двигатель, нажмите запуск.\n2) Следуйте инструкциям на экране после запуска процедуры.\n3) Дождитесь окончания."
			});
			this.status_string_0102 = "";
			this.status_string_0104 = "";
			this.lastStartedOption = "";
			this.Password = "";
			base.PasswordVisible = false;
		}

		// Token: 0x06005828 RID: 22568 RVA: 0x00420B54 File Offset: 0x0041ED54
		protected override async Task<CodingRequestResult> OptionExecute(string optionValue, IProgress<string> progress)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
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
			ResponseReceivedDelegate responseReceivedDelegate2 = delegate(OBDRequest request, string data)
			{
				string text = "3102";
				string text2 = this.lastStartedOption;
				if (!(text2 == "31010364040000"))
				{
					if (!(text2 == "31010318040000"))
					{
						if (text2 == "31010319040000")
						{
							text = "31020319";
						}
					}
					else
					{
						text = "31020318";
					}
				}
				else
				{
					text = "31020364";
				}
				OBDRequest obdrequest4 = new OBDRequest(text, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
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
			if (optionValue == this.optBasicSettingsClearMileage.Value || optionValue == this.optBasicSettingsClearTemperature.Value || optionValue == this.optDrivingRecordDataClear.Value)
			{
				OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				this.lastStartedOption = optionValue;
				OBDRequest obdrequest2 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				OBDRequest obdrequest3 = new OBDRequest("220104", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				obdrequest.ResponseReceived += responseReceivedDelegate2;
				obdrequest2.ResponseReceived += responseReceivedDelegate;
				obdrequest3.ResponseReceived += responseReceivedDelegate;
				bool status0102_finished = false;
				obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
					progress.Report(this.status_string_0102 + "\n" + this.status_string_0104);
					if (data[0] == 16)
					{
						status0102_finished = true;
						progress.Report(Translate.GetString("coding_OperationFinished"));
						codingResult = CodingRequestResult.Success;
						semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					}
				};
				obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					this.status_string_0104 = this.StatusFrom0104ToString(data);
					progress.Report(this.status_string_0102 + "\n" + this.status_string_0104);
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
				await semaphore.WaitAsync();
			}
			return codingResult;
		}

		// Token: 0x040036A0 RID: 13984
		protected MQBAdaptationOption optDrivingRecordDataClear;

		// Token: 0x040036A1 RID: 13985
		protected MQBAdaptationOption optBasicSettingsClearTemperature;

		// Token: 0x040036A2 RID: 13986
		protected MQBAdaptationOption optBasicSettingsClearMileage;

		// Token: 0x040036A3 RID: 13987
		private string status_string_0102;

		// Token: 0x040036A4 RID: 13988
		private string status_string_0104;

		// Token: 0x040036A5 RID: 13989
		private string lastStartedOption;

		// Token: 0x02000B06 RID: 2822
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06005829 RID: 22569 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x0600582A RID: 22570 RVA: 0x00420BA8 File Offset: 0x0041EDA8
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

			// Token: 0x0600582B RID: 22571 RVA: 0x00420CE0 File Offset: 0x0041EEE0
			internal void <OptionExecute>b__1(OBDRequest request, string data)
			{
				string text = "3102";
				string lastStartedOption = this.<>4__this.lastStartedOption;
				if (!(lastStartedOption == "31010364040000"))
				{
					if (!(lastStartedOption == "31010318040000"))
					{
						if (lastStartedOption == "31010319040000")
						{
							text = "31020319";
						}
					}
					else
					{
						text = "31020318";
					}
				}
				else
				{
					text = "31020364";
				}
				OBDRequest obdrequest = new OBDRequest(text, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false);
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

			// Token: 0x0600582C RID: 22572 RVA: 0x00420EA8 File Offset: 0x0041F0A8
			internal void <OptionExecute>b__3(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.codingResult = CodingRequestResult.UnknownError;
					this.semaphore.Release();
					return;
				}
				this.<>4__this.status_string_0104 = this.<>4__this.StatusFrom0104ToString(data);
				this.progress.Report(this.<>4__this.status_string_0102 + "\n" + this.<>4__this.status_string_0104);
			}

			// Token: 0x040036A6 RID: 13990
			public CodingRequestResult codingResult;

			// Token: 0x040036A7 RID: 13991
			public SemaphoreSlim semaphore;

			// Token: 0x040036A8 RID: 13992
			public MQBAisinProcedures <>4__this;

			// Token: 0x040036A9 RID: 13993
			public IProgress<string> progress;
		}

		// Token: 0x02000B07 RID: 2823
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_1
		{
			// Token: 0x0600582D RID: 22573 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_1()
			{
			}

			// Token: 0x0600582E RID: 22574 RVA: 0x00420F14 File Offset: 0x0041F114
			internal void <OptionExecute>b__2(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
					this.CS$<>8__locals1.semaphore.Release();
					return;
				}
				this.CS$<>8__locals1.<>4__this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
				this.CS$<>8__locals1.progress.Report(this.CS$<>8__locals1.<>4__this.status_string_0102 + "\n" + this.CS$<>8__locals1.<>4__this.status_string_0104);
				if (data[0] == 16)
				{
					this.status0102_finished = true;
					this.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
					this.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
					this.CS$<>8__locals1.semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				}
			}

			// Token: 0x040036AA RID: 13994
			public bool status0102_finished;

			// Token: 0x040036AB RID: 13995
			public MQBAisinProcedures.<>c__DisplayClass7_0 CS$<>8__locals1;
		}

		// Token: 0x02000B08 RID: 2824
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__7 : IAsyncStateMachine
		{
			// Token: 0x0600582F RID: 22575 RVA: 0x00420FE8 File Offset: 0x0041F1E8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAisinProcedures mqbaisinProcedures = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBAisinProcedures.<>c__DisplayClass7_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
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
						ResponseReceivedDelegate responseReceivedDelegate2 = delegate(OBDRequest request, string data)
						{
							string text = "3102";
							string lastStartedOption = CS$<>8__locals1.<>4__this.lastStartedOption;
							if (!(lastStartedOption == "31010364040000"))
							{
								if (!(lastStartedOption == "31010318040000"))
								{
									if (lastStartedOption == "31010319040000")
									{
										text = "31020319";
									}
								}
								else
								{
									text = "31020318";
								}
							}
							else
							{
								text = "31020364";
							}
							OBDRequest obdrequest4 = new OBDRequest(text, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false);
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
						if (!(optionValue == mqbaisinProcedures.optBasicSettingsClearMileage.Value) && !(optionValue == mqbaisinProcedures.optBasicSettingsClearTemperature.Value) && !(optionValue == mqbaisinProcedures.optDrivingRecordDataClear.Value))
						{
							goto IL_0220;
						}
						MQBAisinProcedures.<>c__DisplayClass7_1 CS$<>8__locals2 = new MQBAisinProcedures.<>c__DisplayClass7_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						OBDRequest obdrequest = new OBDRequest(optionValue, mqbaisinProcedures.RequestHeader, mqbaisinProcedures.BeforeCommands, mqbaisinProcedures.AfterCommands, false);
						mqbaisinProcedures.lastStartedOption = optionValue;
						OBDRequest obdrequest2 = new OBDRequest("220102", mqbaisinProcedures.RequestHeader, mqbaisinProcedures.BeforeCommands, mqbaisinProcedures.AfterCommands, true);
						OBDRequest obdrequest3 = new OBDRequest("220104", mqbaisinProcedures.RequestHeader, mqbaisinProcedures.BeforeCommands, mqbaisinProcedures.AfterCommands, true);
						obdrequest.ResponseReceived += responseReceivedDelegate2;
						obdrequest2.ResponseReceived += responseReceivedDelegate;
						obdrequest3.ResponseReceived += responseReceivedDelegate;
						CS$<>8__locals2.status0102_finished = false;
						obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								return;
							}
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
							CS$<>8__locals2.CS$<>8__locals1.progress.Report(CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0102 + "\n" + CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0104);
							if (data[0] == 16)
							{
								CS$<>8__locals2.status0102_finished = true;
								CS$<>8__locals2.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								App.OBDReader.ClearRequestQueue();
							}
						};
						obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								return;
							}
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0104 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.StatusFrom0104ToString(data);
							CS$<>8__locals2.CS$<>8__locals1.progress.Report(CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0102 + "\n" + CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0104);
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
						taskAwaiter = CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAisinProcedures.<OptionExecute>d__7>(ref taskAwaiter, ref this);
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
					IL_0220:
					codingResult = CS$<>8__locals1.codingResult;
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
				this.<>t__builder.SetResult(codingResult);
			}

			// Token: 0x06005830 RID: 22576 RVA: 0x0042127C File Offset: 0x0041F47C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040036AC RID: 13996
			public int <>1__state;

			// Token: 0x040036AD RID: 13997
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040036AE RID: 13998
			public MQBAisinProcedures <>4__this;

			// Token: 0x040036AF RID: 13999
			public IProgress<string> progress;

			// Token: 0x040036B0 RID: 14000
			public string optionValue;

			// Token: 0x040036B1 RID: 14001
			private MQBAisinProcedures.<>c__DisplayClass7_0 <>8__1;

			// Token: 0x040036B2 RID: 14002
			private TaskAwaiter <>u__1;
		}
	}
}
