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
	// Token: 0x02000B5E RID: 2910
	internal class ThrottleAdaptationRoutine : MQBServiceProcedure
	{
		// Token: 0x060059CF RID: 22991 RVA: 0x0042CCC8 File Offset: 0x0042AEC8
		public ThrottleAdaptationRoutine()
		{
			base.Name = Translate.GetString("codingDB_ThrottleAdaptation_Name");
			base.Description = Translate.GetString("codingDB_ThrottleAdaptation_Description");
			base.InnerDescription = "";
			this.Unit = "01";
			this.startOption = new MQBAdaptationOption(Translate.GetString("coding_Start"), "3101032E040000");
			this.cancelOption = new MQBAdaptationOption(Translate.GetString("coding_Stop"), "3102032E");
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
			base.Translations.Add(new TranslationItem("ru", "Адаптация дроссельной заслонки", "", "")
			{
				ShortName = "Эта процедура может улучшить реакцию на педаль акселератора на некоторое время"
			});
			base.Group = CodingGroup.EngineAndPowertrain;
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x0042CDA0 File Offset: 0x0042AFA0
		protected override async Task<CodingRequestResult> OptionExecute(string optionValue, IProgress<string> progress)
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
			if (optionValue == this.startOption.Value)
			{
				bool waitingForFinishedState = true;
				OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest break_routine_request = new OBDRequest(this.cancelOption.Value, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
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
						App.OBDReader.ReplaceQueue(new OBDRequest[] { break_routine_request });
						semaphore.Release();
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

		// Token: 0x04003833 RID: 14387
		protected MQBAdaptationOption startOption;

		// Token: 0x04003834 RID: 14388
		protected new MQBAdaptationOption cancelOption;

		// Token: 0x02000B5F RID: 2911
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060059D1 RID: 22993 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060059D2 RID: 22994 RVA: 0x0042CDF4 File Offset: 0x0042AFF4
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

			// Token: 0x060059D3 RID: 22995 RVA: 0x0042CF2C File Offset: 0x0042B12C
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

			// Token: 0x060059D4 RID: 22996 RVA: 0x0042D0A7 File Offset: 0x0042B2A7
			internal void <OptionExecute>b__3(OBDRequest request, string data)
			{
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x04003835 RID: 14389
			public CodingRequestResult codingResult;

			// Token: 0x04003836 RID: 14390
			public SemaphoreSlim semaphore;

			// Token: 0x04003837 RID: 14391
			public ThrottleAdaptationRoutine <>4__this;

			// Token: 0x04003838 RID: 14392
			public IProgress<string> progress;
		}

		// Token: 0x02000B60 RID: 2912
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_1
		{
			// Token: 0x060059D5 RID: 22997 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_1()
			{
			}

			// Token: 0x060059D6 RID: 22998 RVA: 0x0042D0DC File Offset: 0x0042B2DC
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
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.break_routine_request });
					this.CS$<>8__locals1.semaphore.Release();
				}
			}

			// Token: 0x04003839 RID: 14393
			public bool waitingForFinishedState;

			// Token: 0x0400383A RID: 14394
			public OBDRequest break_routine_request;

			// Token: 0x0400383B RID: 14395
			public ThrottleAdaptationRoutine.<>c__DisplayClass3_0 CS$<>8__locals1;
		}

		// Token: 0x02000B61 RID: 2913
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__3 : IAsyncStateMachine
		{
			// Token: 0x060059D7 RID: 22999 RVA: 0x0042D188 File Offset: 0x0042B388
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ThrottleAdaptationRoutine throttleAdaptationRoutine = this;
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
							goto IL_02AE;
						}
						CS$<>8__locals1 = new ThrottleAdaptationRoutine.<>c__DisplayClass3_0();
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
						if (!(optionValue == throttleAdaptationRoutine.startOption.Value))
						{
							goto IL_01DB;
						}
						ThrottleAdaptationRoutine.<>c__DisplayClass3_1 CS$<>8__locals2 = new ThrottleAdaptationRoutine.<>c__DisplayClass3_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.waitingForFinishedState = true;
						OBDRequest obdrequest = new OBDRequest(optionValue, throttleAdaptationRoutine.RequestHeader, throttleAdaptationRoutine.BeforeCommands, throttleAdaptationRoutine.AfterCommands, false);
						CS$<>8__locals2.break_routine_request = new OBDRequest(throttleAdaptationRoutine.cancelOption.Value, throttleAdaptationRoutine.RequestHeader, throttleAdaptationRoutine.BeforeCommands, throttleAdaptationRoutine.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("220102", throttleAdaptationRoutine.RequestHeader, throttleAdaptationRoutine.BeforeCommands, throttleAdaptationRoutine.AfterCommands, true);
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
								App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals2.break_routine_request });
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
						taskAwaiter = CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ThrottleAdaptationRoutine.<OptionExecute>d__3>(ref taskAwaiter, ref this);
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
					IL_01DB:
					if (!(optionValue == throttleAdaptationRoutine.cancelOption.Value))
					{
						goto IL_02B5;
					}
					OBDRequest obdrequest3 = new OBDRequest(optionValue, throttleAdaptationRoutine.RequestHeader, throttleAdaptationRoutine.BeforeCommands, throttleAdaptationRoutine.AfterCommands, false);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ThrottleAdaptationRoutine.<OptionExecute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_02AE:
					taskAwaiter.GetResult();
					IL_02B5:
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

			// Token: 0x060059D8 RID: 23000 RVA: 0x0042D4C0 File Offset: 0x0042B6C0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400383C RID: 14396
			public int <>1__state;

			// Token: 0x0400383D RID: 14397
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400383E RID: 14398
			public ThrottleAdaptationRoutine <>4__this;

			// Token: 0x0400383F RID: 14399
			public IProgress<string> progress;

			// Token: 0x04003840 RID: 14400
			public string optionValue;

			// Token: 0x04003841 RID: 14401
			private ThrottleAdaptationRoutine.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04003842 RID: 14402
			private ResponseReceivedDelegate <checkReceivedResponseForNegativeResult>5__2;

			// Token: 0x04003843 RID: 14403
			private TaskAwaiter <>u__1;
		}
	}
}
