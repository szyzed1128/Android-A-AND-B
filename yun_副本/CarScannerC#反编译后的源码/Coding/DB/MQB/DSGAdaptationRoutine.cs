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
	// Token: 0x02000AA5 RID: 2725
	internal class DSGAdaptationRoutine : MQBServiceProcedure
	{
		// Token: 0x06005601 RID: 22017 RVA: 0x0040FC88 File Offset: 0x0040DE88
		public DSGAdaptationRoutine()
		{
			base.Name = Translate.GetString("codingDB_DsgAdaptationDq200Dq250Dq500_Name");
			base.InnerDescription = Translate.GetString("codingDB_DsgAdaptationDq200Dq250Dq500_InnerDescription");
			this.Unit = "02";
			this.startOption = new MQBAdaptationOption("1. Start", "31010380040000", new TranslationItem[]
			{
				new TranslationItem("ru", "1. Запуск", "", "")
			});
			this.cancelOption = new MQBAdaptationOption("2. Cancel", "31020380", new TranslationItem[]
			{
				new TranslationItem("ru", "2. Прервать", "", "")
			});
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
		}

		// Token: 0x06005602 RID: 22018 RVA: 0x0040FD58 File Offset: 0x0040DF58
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
						semaphore.Release();
						App.OBDReader.ClearRequestQueue();
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

		// Token: 0x040034E0 RID: 13536
		protected MQBAdaptationOption startOption;

		// Token: 0x040034E1 RID: 13537
		protected new MQBAdaptationOption cancelOption;

		// Token: 0x02000AA6 RID: 2726
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06005603 RID: 22019 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005604 RID: 22020 RVA: 0x0040FDAC File Offset: 0x0040DFAC
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

			// Token: 0x06005605 RID: 22021 RVA: 0x0040FEE4 File Offset: 0x0040E0E4
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

			// Token: 0x06005606 RID: 22022 RVA: 0x0041005F File Offset: 0x0040E25F
			internal void <OptionExecute>b__3(OBDRequest request, string data)
			{
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x040034E2 RID: 13538
			public CodingRequestResult codingResult;

			// Token: 0x040034E3 RID: 13539
			public SemaphoreSlim semaphore;

			// Token: 0x040034E4 RID: 13540
			public DSGAdaptationRoutine <>4__this;

			// Token: 0x040034E5 RID: 13541
			public IProgress<string> progress;
		}

		// Token: 0x02000AA7 RID: 2727
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_1
		{
			// Token: 0x06005607 RID: 22023 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_1()
			{
			}

			// Token: 0x06005608 RID: 22024 RVA: 0x00410094 File Offset: 0x0040E294
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
					this.CS$<>8__locals1.semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				}
			}

			// Token: 0x040034E6 RID: 13542
			public bool waitingForFinishedState;

			// Token: 0x040034E7 RID: 13543
			public DSGAdaptationRoutine.<>c__DisplayClass3_0 CS$<>8__locals1;
		}

		// Token: 0x02000AA8 RID: 2728
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005609 RID: 22025 RVA: 0x00410130 File Offset: 0x0040E330
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DSGAdaptationRoutine dsgadaptationRoutine = this;
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
							goto IL_0284;
						}
						CS$<>8__locals1 = new DSGAdaptationRoutine.<>c__DisplayClass3_0();
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
						if (!(optionValue == dsgadaptationRoutine.startOption.Value))
						{
							goto IL_01B1;
						}
						DSGAdaptationRoutine.<>c__DisplayClass3_1 CS$<>8__locals2 = new DSGAdaptationRoutine.<>c__DisplayClass3_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.waitingForFinishedState = true;
						OBDRequest obdrequest = new OBDRequest(optionValue, dsgadaptationRoutine.RequestHeader, dsgadaptationRoutine.BeforeCommands, dsgadaptationRoutine.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("220102", dsgadaptationRoutine.RequestHeader, dsgadaptationRoutine.BeforeCommands, dsgadaptationRoutine.AfterCommands, true);
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
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								App.OBDReader.ClearRequestQueue();
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
						taskAwaiter = CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DSGAdaptationRoutine.<OptionExecute>d__3>(ref taskAwaiter, ref this);
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
					IL_01B1:
					if (!(optionValue == dsgadaptationRoutine.cancelOption.Value))
					{
						goto IL_028B;
					}
					OBDRequest obdrequest3 = new OBDRequest(optionValue, dsgadaptationRoutine.RequestHeader, dsgadaptationRoutine.BeforeCommands, dsgadaptationRoutine.AfterCommands, false);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DSGAdaptationRoutine.<OptionExecute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_0284:
					taskAwaiter.GetResult();
					IL_028B:
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

			// Token: 0x0600560A RID: 22026 RVA: 0x0041043C File Offset: 0x0040E63C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040034E8 RID: 13544
			public int <>1__state;

			// Token: 0x040034E9 RID: 13545
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040034EA RID: 13546
			public DSGAdaptationRoutine <>4__this;

			// Token: 0x040034EB RID: 13547
			public IProgress<string> progress;

			// Token: 0x040034EC RID: 13548
			public string optionValue;

			// Token: 0x040034ED RID: 13549
			private DSGAdaptationRoutine.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x040034EE RID: 13550
			private ResponseReceivedDelegate <checkReceivedResponseForNegativeResult>5__2;

			// Token: 0x040034EF RID: 13551
			private TaskAwaiter <>u__1;
		}
	}
}
