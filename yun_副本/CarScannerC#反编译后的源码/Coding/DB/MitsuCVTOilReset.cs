using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB
{
	// Token: 0x020009B7 RID: 2487
	internal class MitsuCVTOilReset : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x060050E0 RID: 20704 RVA: 0x003ECAA4 File Offset: 0x003EACA4
		public MitsuCVTOilReset()
		{
			base.Name = "Reset CVT oil degradation level";
			base.Description = "Compatibility: Mitsubishi Outlander II, XL, may be other Mitsubishi CVT models too.\nOperation conditions: ignition: ON, engine: not started.";
			base.Translations.Add(new TranslationItem("ru", "Сброс счетчика деградации масла в вариаторе (CVT)", "Совместимость: Mitsubishi Outlander II, XL. Возможно подходит и для других моделей Mitsubishi с CVT, не проверено.\nУсловия для проведения операции: зажигание включено, двигатель не запущен.", ""));
			base.Group = CodingGroup.ServiceProcedures;
			base.RequestHeader = "7E1";
			base.ResponseHeader = "";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset", "00", new TranslationItem[]
			{
				new TranslationItem("ru", "Сбросить", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			this.HasCurrentState = true;
			base.Group = CodingGroup.ServiceProcedures;
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x003ECB7E File Offset: 0x003EAD7E
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "1092";
			this.AfterCommands = "1081";
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x003ECB98 File Offset: 0x003EAD98
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest("2110", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			byte[] result = null;
			obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					requestResult = CodingRequestResult.NotSupported;
				}
				else
				{
					requestResult = CodingRequestResult.Success;
				}
				result = data;
				semaphore.Release();
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
			await semaphore.WaitAsync();
			if (requestResult == CodingRequestResult.Success && result != null && result.Length >= 30)
			{
				base.CurrentState = ((int)result[28] * 256 + (int)result[29]).ToString();
			}
			else
			{
				base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(requestResult);
			}
			return requestResult;
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x003ECBDC File Offset: 0x003EADDC
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			OBDRequest req_sendKey = new OBDRequest("2702", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			OBDRequest obdrequest = new OBDRequest("2701", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			obdrequest.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					string mitsuSeed = MitsuCVTOilReset.GetMitsuSeed(getSeedData);
					req_sendKey.Command += mitsuSeed;
					IProgress<string> progress2 = progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_SendingPassword"));
					}
				}
				catch (Exception)
				{
					bool ignoreCodingErrors = SharedSettings.Current.IgnoreCodingErrors;
					codingResult = CodingRequestResult.WrongSeed;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
				}
			};
			req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (!data.Contains("6702") && !SharedSettings.Current.IgnoreCodingErrors)
				{
					if (!SharedSettings.Current.IgnoreCodingErrors)
					{
						codingResult = CodingRequestResult.WrongAccessKey;
						IProgress<string> progress3 = progress;
						if (progress3 != null)
						{
							progress3.Report(Translate.GetString("coding_progress_WrongPassword"));
						}
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						semaphore.Release();
					}
					return;
				}
				IProgress<string> progress4 = progress;
				if (progress4 == null)
				{
					return;
				}
				progress4.Report(Translate.GetString("coding_progress_SuccessPassword"));
			};
			OBDRequest obdrequest2 = new OBDRequest("3103", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			new OBDRequest("2110", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false).ELMFormat = ELMFormat.CAN11bit;
			obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("7103") || data.Contains("037F7078"))
				{
					IProgress<string> progress5 = progress;
					if (progress5 != null)
					{
						progress5.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress6 = progress;
					if (progress6 != null)
					{
						progress6.Report(Translate.GetString("coding_progress_DataRejected"));
					}
					if (data == null)
					{
						codingResult = CodingRequestResult.UnknownError;
					}
					else
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Length >= 12 && data.Contains("7F31"))
						{
							int num = data.IndexOf("7F31");
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
						}
						else
						{
							codingResult = CodingRequestResult.UnknownError;
						}
					}
				}
				semaphore.Release();
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, req_sendKey, obdrequest2 });
			await semaphore.WaitAsync();
			return codingResult;
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x003ECC28 File Offset: 0x003EAE28
		private static string GetMitsuSeed(byte[] seed)
		{
			int num = ((((int)seed[0] * 256) | (int)seed[1]) << 3) & 65535;
			int num2 = ((((int)seed[2] * 256) | (int)seed[3]) << 3) & 65535;
			int num3 = (num * 16) & 65535;
			int num4 = (num2 * 16) & 65535;
			num = (num + 4617 + num3) & 65535;
			num2 = (num2 + 4617 + num4) & 65535;
			return num.ToString("X4") + num2.ToString("X4");
		}

		// Token: 0x020009B8 RID: 2488
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060050E5 RID: 20709 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060050E6 RID: 20710 RVA: 0x003ECCB4 File Offset: 0x003EAEB4
			internal void <UpdateCurrentState>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.requestResult = CodingRequestResult.NotSupported;
				}
				else
				{
					this.requestResult = CodingRequestResult.Success;
				}
				this.result = data;
				this.semaphore.Release();
			}

			// Token: 0x040030DD RID: 12509
			public CodingRequestResult requestResult;

			// Token: 0x040030DE RID: 12510
			public byte[] result;

			// Token: 0x040030DF RID: 12511
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020009B9 RID: 2489
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060050E7 RID: 20711 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060050E8 RID: 20712 RVA: 0x003ECCE4 File Offset: 0x003EAEE4
			internal void <Execute>b__0(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					string mitsuSeed = MitsuCVTOilReset.GetMitsuSeed(getSeedData);
					this.req_sendKey.Command = this.req_sendKey.Command + mitsuSeed;
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_SendingPassword"));
					}
				}
				catch (Exception)
				{
					bool ignoreCodingErrors = SharedSettings.Current.IgnoreCodingErrors;
					this.codingResult = CodingRequestResult.WrongSeed;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
				}
			}

			// Token: 0x060050E9 RID: 20713 RVA: 0x003ECD74 File Offset: 0x003EAF74
			internal void <Execute>b__1(OBDRequest request, string data)
			{
				if (!data.Contains("6702") && !SharedSettings.Current.IgnoreCodingErrors)
				{
					if (!SharedSettings.Current.IgnoreCodingErrors)
					{
						this.codingResult = CodingRequestResult.WrongAccessKey;
						IProgress<string> progress = this.progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_WrongPassword"));
						}
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						this.semaphore.Release();
					}
					return;
				}
				IProgress<string> progress2 = this.progress;
				if (progress2 == null)
				{
					return;
				}
				progress2.Report(Translate.GetString("coding_progress_SuccessPassword"));
			}

			// Token: 0x060050EA RID: 20714 RVA: 0x003ECE00 File Offset: 0x003EB000
			internal void <Execute>b__2(OBDRequest request, string data)
			{
				if (data.Contains("7103") || data.Contains("037F7078"))
				{
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					this.codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress2 = this.progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_DataRejected"));
					}
					if (data == null)
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					else
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Length >= 12 && data.Contains("7F31"))
						{
							int num = data.IndexOf("7F31");
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
						}
						else
						{
							this.codingResult = CodingRequestResult.UnknownError;
						}
					}
				}
				this.semaphore.Release();
			}

			// Token: 0x040030E0 RID: 12512
			public OBDRequest req_sendKey;

			// Token: 0x040030E1 RID: 12513
			public IProgress<string> progress;

			// Token: 0x040030E2 RID: 12514
			public CodingRequestResult codingResult;

			// Token: 0x040030E3 RID: 12515
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020009BA RID: 2490
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x060050EB RID: 20715 RVA: 0x003ECF08 File Offset: 0x003EB108
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MitsuCVTOilReset mitsuCVTOilReset = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MitsuCVTOilReset.<>c__DisplayClass3_0();
						CS$<>8__locals1.progress = progress;
						mitsuCVTOilReset.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.req_sendKey = new OBDRequest("2702", mitsuCVTOilReset.RequestHeader, mitsuCVTOilReset.BeforeCommands, mitsuCVTOilReset.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						OBDRequest obdrequest = new OBDRequest("2701", mitsuCVTOilReset.RequestHeader, mitsuCVTOilReset.BeforeCommands, mitsuCVTOilReset.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						obdrequest.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
						{
							try
							{
								string mitsuSeed = MitsuCVTOilReset.GetMitsuSeed(getSeedData);
								CS$<>8__locals1.req_sendKey.Command = CS$<>8__locals1.req_sendKey.Command + mitsuSeed;
								IProgress<string> progress = CS$<>8__locals1.progress;
								if (progress != null)
								{
									progress.Report(Translate.GetString("coding_progress_SendingPassword"));
								}
							}
							catch (Exception)
							{
								bool ignoreCodingErrors = SharedSettings.Current.IgnoreCodingErrors;
								CS$<>8__locals1.codingResult = CodingRequestResult.WrongSeed;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
							}
						};
						CS$<>8__locals1.req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (!data.Contains("6702") && !SharedSettings.Current.IgnoreCodingErrors)
							{
								if (!SharedSettings.Current.IgnoreCodingErrors)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
									IProgress<string> progress2 = CS$<>8__locals1.progress;
									if (progress2 != null)
									{
										progress2.Report(Translate.GetString("coding_progress_WrongPassword"));
									}
									App.OBDReader.ReplaceQueue(new OBDRequest[0]);
									CS$<>8__locals1.semaphore.Release();
								}
								return;
							}
							IProgress<string> progress3 = CS$<>8__locals1.progress;
							if (progress3 == null)
							{
								return;
							}
							progress3.Report(Translate.GetString("coding_progress_SuccessPassword"));
						};
						OBDRequest obdrequest2 = new OBDRequest("3103", mitsuCVTOilReset.RequestHeader, mitsuCVTOilReset.BeforeCommands, mitsuCVTOilReset.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						new OBDRequest("2110", mitsuCVTOilReset.RequestHeader, mitsuCVTOilReset.BeforeCommands, mitsuCVTOilReset.AfterCommands, false).ELMFormat = ELMFormat.CAN11bit;
						obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("7103") || data.Contains("037F7078"))
							{
								IProgress<string> progress4 = CS$<>8__locals1.progress;
								if (progress4 != null)
								{
									progress4.Report(Translate.GetString("coding_progress_DataAccepted"));
								}
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
							}
							else
							{
								IProgress<string> progress5 = CS$<>8__locals1.progress;
								if (progress5 != null)
								{
									progress5.Report(Translate.GetString("coding_progress_DataRejected"));
								}
								if (data == null)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								}
								else
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
									if (data.Length >= 12 && data.Contains("7F31"))
									{
										int num3 = data.IndexOf("7F31");
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
									}
									else
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
									}
								}
							}
							CS$<>8__locals1.semaphore.Release();
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, CS$<>8__locals1.req_sendKey, obdrequest2 });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MitsuCVTOilReset.<Execute>d__3>(ref taskAwaiter, ref this);
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

			// Token: 0x060050EC RID: 20716 RVA: 0x003ED14C File Offset: 0x003EB34C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030E4 RID: 12516
			public int <>1__state;

			// Token: 0x040030E5 RID: 12517
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040030E6 RID: 12518
			public IProgress<string> progress;

			// Token: 0x040030E7 RID: 12519
			public MitsuCVTOilReset <>4__this;

			// Token: 0x040030E8 RID: 12520
			private MitsuCVTOilReset.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x040030E9 RID: 12521
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020009BB RID: 2491
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x060050ED RID: 20717 RVA: 0x003ED15C File Offset: 0x003EB35C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MitsuCVTOilReset mitsuCVTOilReset = this;
				CodingRequestResult requestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MitsuCVTOilReset.<>c__DisplayClass2_0();
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						mitsuCVTOilReset.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest("2110", mitsuCVTOilReset.RequestHeader, mitsuCVTOilReset.BeforeCommands, mitsuCVTOilReset.AfterCommands, false);
						CS$<>8__locals1.result = null;
						obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals1.requestResult = CodingRequestResult.NotSupported;
							}
							else
							{
								CS$<>8__locals1.requestResult = CodingRequestResult.Success;
							}
							CS$<>8__locals1.result = data;
							CS$<>8__locals1.semaphore.Release();
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MitsuCVTOilReset.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.requestResult == CodingRequestResult.Success && CS$<>8__locals1.result != null && CS$<>8__locals1.result.Length >= 30)
					{
						mitsuCVTOilReset.CurrentState = ((int)CS$<>8__locals1.result[28] * 256 + (int)CS$<>8__locals1.result[29]).ToString();
					}
					else
					{
						mitsuCVTOilReset.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(CS$<>8__locals1.requestResult);
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

			// Token: 0x060050EE RID: 20718 RVA: 0x003ED344 File Offset: 0x003EB544
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030EA RID: 12522
			public int <>1__state;

			// Token: 0x040030EB RID: 12523
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040030EC RID: 12524
			public MitsuCVTOilReset <>4__this;

			// Token: 0x040030ED RID: 12525
			private MitsuCVTOilReset.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x040030EE RID: 12526
			private TaskAwaiter <>u__1;
		}
	}
}
