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

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BA8 RID: 2984
	internal class HyundaiReset4AT : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005ABD RID: 23229 RVA: 0x00434B40 File Offset: 0x00432D40
		public HyundaiReset4AT()
		{
			base.Name = "Reset automatic transmission adaptation (4-speed AT)";
			base.Description = "Compatibility: Hyundai/Kia with 4-speed Automatic Transmission with CAN bus (A4CF0, A4CF1, A4CF2). Compatibility depends on installed transmission control unit.\nRequired conditions: ignition: ON, engine NOT started.";
			base.Translations.Add(new TranslationItem("ru", "Сброс адаптации АКПП (4-ступенчатая АКПП)", "Совместимость: 4АКПП Hyundai/Kia (A4CF0, A4CF1, A4CF2) c CAN шиной. Совместимость зависит от установленного блока управления АКПП, поэтому данная функция работает не на всех автомобилях.\nУсловия для выполнения: зажигание включено, двигатель не запущен!", ""));
			base.RequestHeader = "7E1";
			base.ResponseHeader = "";
			this.BeforeCommands = "1090";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset", "00", new TranslationItem[]
			{
				new TranslationItem("ru", "Сбросить", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			this.HasCurrentState = false;
			base.Group = CodingGroup.EngineAndPowertrain;
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x00434C1C File Offset: 0x00432E1C
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "1090";
			this.AfterCommands = "20";
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x00434C34 File Offset: 0x00432E34
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest("21A0", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("61A0"))
				{
					this.CurrentState = "";
					this.HasCurrentState = false;
				}
				else
				{
					this.CurrentState = Translate.GetString("coding_NotSupported");
					this.HasCurrentState = true;
				}
				semaphore.Release();
			};
			base.CurrentState = "";
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
			await semaphore.WaitAsync();
			return CodingRequestResult.Success;
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x00434C78 File Offset: 0x00432E78
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			OBDRequest obdrequest = new OBDRequest("30E200", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("70E2") || data.Contains("037F7078"))
				{
					IProgress<string> progress2 = progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress3 = progress;
					if (progress3 != null)
					{
						progress3.Report(Translate.GetString("coding_progress_DataRejected"));
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
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
			await semaphore.WaitAsync();
			return codingResult;
		}

		// Token: 0x02000BA9 RID: 2985
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x06005AC1 RID: 23233 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06005AC2 RID: 23234 RVA: 0x00434CC4 File Offset: 0x00432EC4
			internal void <UpdateCurrentState>b__0(OBDRequest request, string data)
			{
				if (data.Contains("61A0"))
				{
					this.<>4__this.CurrentState = "";
					this.<>4__this.HasCurrentState = false;
				}
				else
				{
					this.<>4__this.CurrentState = Translate.GetString("coding_NotSupported");
					this.<>4__this.HasCurrentState = true;
				}
				this.semaphore.Release();
			}

			// Token: 0x0400391F RID: 14623
			public HyundaiReset4AT <>4__this;

			// Token: 0x04003920 RID: 14624
			public SemaphoreSlim semaphore;
		}

		// Token: 0x02000BAA RID: 2986
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06005AC3 RID: 23235 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005AC4 RID: 23236 RVA: 0x00434D2C File Offset: 0x00432F2C
			internal void <Execute>b__0(OBDRequest request, string data)
			{
				if (data.Contains("70E2") || data.Contains("037F7078"))
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

			// Token: 0x04003921 RID: 14625
			public IProgress<string> progress;

			// Token: 0x04003922 RID: 14626
			public CodingRequestResult codingResult;

			// Token: 0x04003923 RID: 14627
			public SemaphoreSlim semaphore;
		}

		// Token: 0x02000BAB RID: 2987
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005AC5 RID: 23237 RVA: 0x00434E34 File Offset: 0x00433034
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				HyundaiReset4AT hyundaiReset4AT = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new HyundaiReset4AT.<>c__DisplayClass3_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						OBDRequest obdrequest = new OBDRequest("30E200", hyundaiReset4AT.RequestHeader, hyundaiReset4AT.BeforeCommands, hyundaiReset4AT.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("70E2") || data.Contains("037F7078"))
							{
								IProgress<string> progress = CS$<>8__locals1.progress;
								if (progress != null)
								{
									progress.Report(Translate.GetString("coding_progress_DataAccepted"));
								}
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
							}
							else
							{
								IProgress<string> progress2 = CS$<>8__locals1.progress;
								if (progress2 != null)
								{
									progress2.Report(Translate.GetString("coding_progress_DataRejected"));
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
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HyundaiReset4AT.<Execute>d__3>(ref taskAwaiter, ref this);
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

			// Token: 0x06005AC6 RID: 23238 RVA: 0x00434FAC File Offset: 0x004331AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003924 RID: 14628
			public int <>1__state;

			// Token: 0x04003925 RID: 14629
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003926 RID: 14630
			public IProgress<string> progress;

			// Token: 0x04003927 RID: 14631
			public HyundaiReset4AT <>4__this;

			// Token: 0x04003928 RID: 14632
			private HyundaiReset4AT.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04003929 RID: 14633
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000BAC RID: 2988
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005AC7 RID: 23239 RVA: 0x00434FBC File Offset: 0x004331BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				HyundaiReset4AT hyundaiReset4AT = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						HyundaiReset4AT.<>c__DisplayClass2_0 CS$<>8__locals1 = new HyundaiReset4AT.<>c__DisplayClass2_0();
						CS$<>8__locals1.<>4__this = this;
						hyundaiReset4AT.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest("21A0", hyundaiReset4AT.RequestHeader, hyundaiReset4AT.BeforeCommands, hyundaiReset4AT.AfterCommands, false);
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("61A0"))
							{
								CS$<>8__locals1.<>4__this.CurrentState = "";
								CS$<>8__locals1.<>4__this.HasCurrentState = false;
							}
							else
							{
								CS$<>8__locals1.<>4__this.CurrentState = Translate.GetString("coding_NotSupported");
								CS$<>8__locals1.<>4__this.HasCurrentState = true;
							}
							CS$<>8__locals1.semaphore.Release();
						};
						hyundaiReset4AT.CurrentState = "";
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HyundaiReset4AT.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
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
					codingRequestResult = CodingRequestResult.Success;
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

			// Token: 0x06005AC8 RID: 23240 RVA: 0x004350F8 File Offset: 0x004332F8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400392A RID: 14634
			public int <>1__state;

			// Token: 0x0400392B RID: 14635
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400392C RID: 14636
			public HyundaiReset4AT <>4__this;

			// Token: 0x0400392D RID: 14637
			private TaskAwaiter <>u__1;
		}
	}
}
