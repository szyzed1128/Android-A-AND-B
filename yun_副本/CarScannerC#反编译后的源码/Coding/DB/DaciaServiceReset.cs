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

namespace CarScannerXamarinForms.Coding.DB
{
	// Token: 0x020009C1 RID: 2497
	internal class DaciaServiceReset : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x060050FB RID: 20731 RVA: 0x003EDA50 File Offset: 0x003EBC50
		public DaciaServiceReset()
		{
			base.Name = "Reset service reminder";
			base.Description = "Compatibility: Dacia / Dacia based Renault\nWARNING! This service operation wasn't tested on a real car! Only on emulator! Do this at your own risk!";
			base.Translations.Add(new TranslationItem("ru", "Сброс межсервисного интервала", "Совместимость: Dacia, Renault (на платформе Dacia).\nВНИМАНИЕ! Эта операция не была проверена на живом автомобиле, только на эмуляторе! Используйте на свой страх и риск! Буду благодарен, если сообщите о результатах!", ""));
			base.RequestHeader = "743";
			base.ResponseHeader = "763";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset", "00", new TranslationItem[]
			{
				new TranslationItem("ru", "Сбросить", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			this.HasCurrentState = false;
			base.Group = CodingGroup.ServiceProcedures;
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x003EDB22 File Offset: 0x003EBD22
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "ATSP6;ATSH743;ATFCSH743;ATFCSD300000;ATFCSM1;ATCRA763";
			this.AfterCommands = "ATSH7DF;ATAR;ATFCSM0";
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x003EDB3C File Offset: 0x003EBD3C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.CurrentState = "";
			return CodingRequestResult.Success;
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x003EDB80 File Offset: 0x003EBD80
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			OBDRequest obdrequest = new OBDRequest("10C0", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			OBDRequest obdrequest2 = new OBDRequest("310200", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("710200") || data.Contains("037F3178"))
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
						if (data.Length >= 12 && data.Contains("7F2F"))
						{
							int num = data.IndexOf("7F2F");
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
			OBDRequest obdrequest3 = new OBDRequest("1103", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
			await semaphore.WaitAsync();
			await Task.Delay(500);
			return codingResult;
		}

		// Token: 0x020009C2 RID: 2498
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060050FF RID: 20735 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005100 RID: 20736 RVA: 0x003EDBCC File Offset: 0x003EBDCC
			internal void <Execute>b__0(OBDRequest request, string data)
			{
				if (data.Contains("710200") || data.Contains("037F3178"))
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
						if (data.Length >= 12 && data.Contains("7F2F"))
						{
							int num = data.IndexOf("7F2F");
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

			// Token: 0x04003100 RID: 12544
			public IProgress<string> progress;

			// Token: 0x04003101 RID: 12545
			public CodingRequestResult codingResult;

			// Token: 0x04003102 RID: 12546
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020009C3 RID: 2499
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005101 RID: 20737 RVA: 0x003EDCD4 File Offset: 0x003EBED4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DaciaServiceReset daciaServiceReset = this;
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
							goto IL_01C2;
						}
						CS$<>8__locals1 = new DaciaServiceReset.<>c__DisplayClass3_0();
						CS$<>8__locals1.progress = progress;
						daciaServiceReset.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						OBDRequest obdrequest = new OBDRequest("10C0", daciaServiceReset.RequestHeader, daciaServiceReset.BeforeCommands, daciaServiceReset.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						OBDRequest obdrequest2 = new OBDRequest("310200", daciaServiceReset.RequestHeader, daciaServiceReset.BeforeCommands, daciaServiceReset.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("710200") || data.Contains("037F3178"))
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
									if (data.Length >= 12 && data.Contains("7F2F"))
									{
										int num3 = data.IndexOf("7F2F");
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
						OBDRequest obdrequest3 = new OBDRequest("1103", daciaServiceReset.RequestHeader, daciaServiceReset.BeforeCommands, daciaServiceReset.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DaciaServiceReset.<Execute>d__3>(ref taskAwaiter, ref this);
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
					taskAwaiter = Task.Delay(500).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DaciaServiceReset.<Execute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_01C2:
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

			// Token: 0x06005102 RID: 20738 RVA: 0x003EDF10 File Offset: 0x003EC110
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003103 RID: 12547
			public int <>1__state;

			// Token: 0x04003104 RID: 12548
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003105 RID: 12549
			public IProgress<string> progress;

			// Token: 0x04003106 RID: 12550
			public DaciaServiceReset <>4__this;

			// Token: 0x04003107 RID: 12551
			private DaciaServiceReset.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04003108 RID: 12552
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020009C4 RID: 2500
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005103 RID: 20739 RVA: 0x003EDF20 File Offset: 0x003EC120
			void IAsyncStateMachine.MoveNext()
			{
				DaciaServiceReset daciaServiceReset = this;
				CodingRequestResult codingRequestResult;
				try
				{
					daciaServiceReset.CurrentState = "";
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

			// Token: 0x06005104 RID: 20740 RVA: 0x003EDF80 File Offset: 0x003EC180
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003109 RID: 12553
			public int <>1__state;

			// Token: 0x0400310A RID: 12554
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400310B RID: 12555
			public DaciaServiceReset <>4__this;
		}
	}
}
