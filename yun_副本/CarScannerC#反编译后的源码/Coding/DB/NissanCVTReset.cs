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
	// Token: 0x020009BC RID: 2492
	internal class NissanCVTReset : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x060050EF RID: 20719 RVA: 0x003ED354 File Offset: 0x003EB554
		public NissanCVTReset()
		{
			base.Name = "Reset CVT oil degradation level";
			base.Description = "Experimental! Compatibility not guaranteed!\nRequired conditions: ignition: ON, engine NOT started.";
			base.Translations.Add(new TranslationItem("ru", "Сброс счетчика деградации масла в вариаторе (CVT)", "Экспериментальная возможность, работоспособность не гарантируется.\nУсловия для выполнения: зажигание включено, двигатель не запущен!", ""));
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

		// Token: 0x060050F0 RID: 20720 RVA: 0x003ED42E File Offset: 0x003EB62E
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "ATSP6;ATSH7E1;ATFCSH7E1;ATFCSD300000;ATFCSM1;ATCRA7E9";
			this.AfterCommands = "ATSH7DF;ATAR;ATFCSM0;ATSPDEF";
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x003ED448 File Offset: 0x003EB648
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest("2103", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
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
			if (requestResult == CodingRequestResult.Success && result != null && result.Length >= 4)
			{
				base.CurrentState = ((long)((int)result[0] * 16777216 + (int)result[1] * 65536 + (int)result[2] * 256 + (int)result[3])).ToString();
			}
			else
			{
				base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(requestResult);
			}
			return requestResult;
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x003ED48C File Offset: 0x003EB68C
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			OBDRequest obdrequest = new OBDRequest("10C0", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			OBDRequest obdrequest2 = new OBDRequest("3B0200000000", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("7B02") || data.Contains("037F3178"))
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
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
			await semaphore.WaitAsync();
			await Task.Delay(500);
			return codingResult;
		}

		// Token: 0x020009BD RID: 2493
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060050F3 RID: 20723 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060050F4 RID: 20724 RVA: 0x003ED4D8 File Offset: 0x003EB6D8
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

			// Token: 0x040030EF RID: 12527
			public CodingRequestResult requestResult;

			// Token: 0x040030F0 RID: 12528
			public byte[] result;

			// Token: 0x040030F1 RID: 12529
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020009BE RID: 2494
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060050F5 RID: 20725 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060050F6 RID: 20726 RVA: 0x003ED508 File Offset: 0x003EB708
			internal void <Execute>b__0(OBDRequest request, string data)
			{
				if (data.Contains("7B02") || data.Contains("037F3178"))
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

			// Token: 0x040030F2 RID: 12530
			public IProgress<string> progress;

			// Token: 0x040030F3 RID: 12531
			public CodingRequestResult codingResult;

			// Token: 0x040030F4 RID: 12532
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020009BF RID: 2495
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x060050F7 RID: 20727 RVA: 0x003ED610 File Offset: 0x003EB810
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				NissanCVTReset nissanCVTReset = this;
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
							goto IL_0197;
						}
						CS$<>8__locals1 = new NissanCVTReset.<>c__DisplayClass3_0();
						CS$<>8__locals1.progress = progress;
						nissanCVTReset.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						OBDRequest obdrequest = new OBDRequest("10C0", nissanCVTReset.RequestHeader, nissanCVTReset.BeforeCommands, nissanCVTReset.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						OBDRequest obdrequest2 = new OBDRequest("3B0200000000", nissanCVTReset.RequestHeader, nissanCVTReset.BeforeCommands, nissanCVTReset.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("7B02") || data.Contains("037F3178"))
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
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, NissanCVTReset.<Execute>d__3>(ref taskAwaiter, ref this);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, NissanCVTReset.<Execute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_0197:
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

			// Token: 0x060050F8 RID: 20728 RVA: 0x003ED820 File Offset: 0x003EBA20
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030F5 RID: 12533
			public int <>1__state;

			// Token: 0x040030F6 RID: 12534
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040030F7 RID: 12535
			public IProgress<string> progress;

			// Token: 0x040030F8 RID: 12536
			public NissanCVTReset <>4__this;

			// Token: 0x040030F9 RID: 12537
			private NissanCVTReset.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x040030FA RID: 12538
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020009C0 RID: 2496
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x060050F9 RID: 20729 RVA: 0x003ED830 File Offset: 0x003EBA30
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				NissanCVTReset nissanCVTReset = this;
				CodingRequestResult requestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new NissanCVTReset.<>c__DisplayClass2_0();
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						nissanCVTReset.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest("2103", nissanCVTReset.RequestHeader, nissanCVTReset.BeforeCommands, nissanCVTReset.AfterCommands, false);
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, NissanCVTReset.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.requestResult == CodingRequestResult.Success && CS$<>8__locals1.result != null && CS$<>8__locals1.result.Length >= 4)
					{
						nissanCVTReset.CurrentState = ((long)((int)CS$<>8__locals1.result[0] * 16777216 + (int)CS$<>8__locals1.result[1] * 65536 + (int)CS$<>8__locals1.result[2] * 256 + (int)CS$<>8__locals1.result[3])).ToString();
					}
					else
					{
						nissanCVTReset.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(CS$<>8__locals1.requestResult);
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

			// Token: 0x060050FA RID: 20730 RVA: 0x003EDA40 File Offset: 0x003EBC40
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030FB RID: 12539
			public int <>1__state;

			// Token: 0x040030FC RID: 12540
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040030FD RID: 12541
			public NissanCVTReset <>4__this;

			// Token: 0x040030FE RID: 12542
			private NissanCVTReset.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x040030FF RID: 12543
			private TaskAwaiter <>u__1;
		}
	}
}
