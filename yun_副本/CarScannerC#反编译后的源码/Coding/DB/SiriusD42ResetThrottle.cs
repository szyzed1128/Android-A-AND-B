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
	// Token: 0x020009C5 RID: 2501
	internal class SiriusD42ResetThrottle : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005105 RID: 20741 RVA: 0x003EDF90 File Offset: 0x003EC190
		public SiriusD42ResetThrottle()
		{
			base.Name = "Reset throttle adaptation";
			base.Description = "Operation conditions: ignition: ON, engine: not started. Sometimes it requires several attempts to perform it";
			base.Translations.Add(new TranslationItem("ru", "Сброс адаптации дросселя", "Условие для проведения операции: зажигание включено, двигатель не запущен. В некоторых случаях процедура срабатывает не с первого раза.", ""));
			base.RequestHeader = "8111F1";
			base.ResponseHeader = "";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset", "00", new TranslationItem[]
			{
				new TranslationItem("ru", "Сбросить", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			base.Group = CodingGroup.ServiceProcedures;
			this.HasCurrentState = false;
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x003EE062 File Offset: 0x003EC262
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "ATSP5;ATSH8111F1";
			this.AfterCommands = "ATSH" + App.OBDReader.GetDefaultHeader();
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x003EE08C File Offset: 0x003EC28C
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			OBDRequest req_sendKey = new OBDRequest("2702", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.KWP
			};
			OBDRequest obdrequest = new OBDRequest("2701", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.KWP
			};
			obdrequest.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					string siriusD42Seed_Signed = this.GetSiriusD42Seed_Signed(getSeedData);
					req_sendKey.Command += siriusD42Seed_Signed;
					IProgress<string> progress2 = progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_SendingPassword"));
					}
				}
				catch (Exception)
				{
					if (!SharedSettings.Current.IgnoreCodingErrors)
					{
						codingResult = CodingRequestResult.WrongSeed;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						semaphore.Release();
					}
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
			OBDRequest obdrequest2 = new OBDRequest("305004", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.KWP
			};
			obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("7050") || data.Contains("037F7078"))
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
						if (data.Length >= 12 && data.Contains("7F30"))
						{
							int num = data.IndexOf("7F30");
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

		// Token: 0x06005108 RID: 20744 RVA: 0x003EE0D8 File Offset: 0x003EC2D8
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.CurrentState = "";
			return CodingRequestResult.Success;
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x003EE11C File Offset: 0x003EC31C
		private string GetSiriusD42Seed_Signed(byte[] seed)
		{
			byte b = seed[0];
			byte b2 = seed[1];
			int num = ((int)b2 << 3) & 255;
			num += (b2 >> 5) & 254;
			if (((b2 & 96) == 0 || (b2 & 96) == 96) && (b2 & 1) == 1)
			{
				num++;
			}
			if ((b2 & 96) == 32 && (b2 & 1) == 0)
			{
				num++;
			}
			if ((b2 & 240) == 64 || (b2 & 240) == 80 || (b2 & 240) == 192 || (b2 & 240) == 208)
			{
				num += (int)(1 - (b2 & 1));
			}
			int num2 = ((int)b << 2) & 255;
			num2 += ((b & byte.MaxValue) >> 6) & 254;
			if ((b & 15) == 8 || (b & 15) == 10 || (b & 15) == 12 || (b & 15) == 14)
			{
				num2++;
			}
			if ((b2 & 128) == 128)
			{
				num2 ^= 1;
			}
			if ((b & 15) == 4 || (b & 15) == 6 || (b & 15) == 8 || (b & 15) == 10)
			{
				num ^= 1;
			}
			if ((b & 16) == 16)
			{
				num ^= 1;
			}
			if ((b2 & 8) == 8)
			{
				num2 ^= 1;
			}
			string text = (num2 & 255).ToString("X2");
			string text2 = (num & 255).ToString("X2");
			return text + text2;
		}

		// Token: 0x020009C6 RID: 2502
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600510A RID: 20746 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x0600510B RID: 20747 RVA: 0x003EE268 File Offset: 0x003EC468
			internal void <Execute>b__0(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					string siriusD42Seed_Signed = this.<>4__this.GetSiriusD42Seed_Signed(getSeedData);
					this.req_sendKey.Command = this.req_sendKey.Command + siriusD42Seed_Signed;
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_SendingPassword"));
					}
				}
				catch (Exception)
				{
					if (!SharedSettings.Current.IgnoreCodingErrors)
					{
						this.codingResult = CodingRequestResult.WrongSeed;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						this.semaphore.Release();
					}
				}
			}

			// Token: 0x0600510C RID: 20748 RVA: 0x003EE300 File Offset: 0x003EC500
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

			// Token: 0x0600510D RID: 20749 RVA: 0x003EE38C File Offset: 0x003EC58C
			internal void <Execute>b__2(OBDRequest request, string data)
			{
				if (data.Contains("7050") || data.Contains("037F7078"))
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
						if (data.Length >= 12 && data.Contains("7F30"))
						{
							int num = data.IndexOf("7F30");
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

			// Token: 0x0400310C RID: 12556
			public SiriusD42ResetThrottle <>4__this;

			// Token: 0x0400310D RID: 12557
			public OBDRequest req_sendKey;

			// Token: 0x0400310E RID: 12558
			public IProgress<string> progress;

			// Token: 0x0400310F RID: 12559
			public CodingRequestResult codingResult;

			// Token: 0x04003110 RID: 12560
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020009C7 RID: 2503
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x0600510E RID: 20750 RVA: 0x003EE494 File Offset: 0x003EC694
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SiriusD42ResetThrottle siriusD42ResetThrottle = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new SiriusD42ResetThrottle.<>c__DisplayClass2_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.req_sendKey = new OBDRequest("2702", siriusD42ResetThrottle.RequestHeader, siriusD42ResetThrottle.BeforeCommands, siriusD42ResetThrottle.AfterCommands, false)
						{
							ELMFormat = ELMFormat.KWP
						};
						OBDRequest obdrequest = new OBDRequest("2701", siriusD42ResetThrottle.RequestHeader, siriusD42ResetThrottle.BeforeCommands, siriusD42ResetThrottle.AfterCommands, false)
						{
							ELMFormat = ELMFormat.KWP
						};
						obdrequest.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
						{
							try
							{
								string siriusD42Seed_Signed = CS$<>8__locals1.<>4__this.GetSiriusD42Seed_Signed(getSeedData);
								CS$<>8__locals1.req_sendKey.Command = CS$<>8__locals1.req_sendKey.Command + siriusD42Seed_Signed;
								IProgress<string> progress = CS$<>8__locals1.progress;
								if (progress != null)
								{
									progress.Report(Translate.GetString("coding_progress_SendingPassword"));
								}
							}
							catch (Exception)
							{
								if (!SharedSettings.Current.IgnoreCodingErrors)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongSeed;
									App.OBDReader.ReplaceQueue(new OBDRequest[0]);
									CS$<>8__locals1.semaphore.Release();
								}
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
						OBDRequest obdrequest2 = new OBDRequest("305004", siriusD42ResetThrottle.RequestHeader, siriusD42ResetThrottle.BeforeCommands, siriusD42ResetThrottle.AfterCommands, false)
						{
							ELMFormat = ELMFormat.KWP
						};
						obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("7050") || data.Contains("037F7078"))
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
									if (data.Length >= 12 && data.Contains("7F30"))
									{
										int num3 = data.IndexOf("7F30");
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SiriusD42ResetThrottle.<Execute>d__2>(ref taskAwaiter, ref this);
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

			// Token: 0x0600510F RID: 20751 RVA: 0x003EE6BC File Offset: 0x003EC8BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003111 RID: 12561
			public int <>1__state;

			// Token: 0x04003112 RID: 12562
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003113 RID: 12563
			public SiriusD42ResetThrottle <>4__this;

			// Token: 0x04003114 RID: 12564
			public IProgress<string> progress;

			// Token: 0x04003115 RID: 12565
			private SiriusD42ResetThrottle.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x04003116 RID: 12566
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020009C8 RID: 2504
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005110 RID: 20752 RVA: 0x003EE6CC File Offset: 0x003EC8CC
			void IAsyncStateMachine.MoveNext()
			{
				SiriusD42ResetThrottle siriusD42ResetThrottle = this;
				CodingRequestResult codingRequestResult;
				try
				{
					siriusD42ResetThrottle.CurrentState = "";
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

			// Token: 0x06005111 RID: 20753 RVA: 0x003EE72C File Offset: 0x003EC92C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003117 RID: 12567
			public int <>1__state;

			// Token: 0x04003118 RID: 12568
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003119 RID: 12569
			public SiriusD42ResetThrottle <>4__this;
		}
	}
}
