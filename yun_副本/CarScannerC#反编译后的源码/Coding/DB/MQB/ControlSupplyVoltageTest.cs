using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A93 RID: 2707
	internal class ControlSupplyVoltageTest : MQBServiceProcedure
	{
		// Token: 0x0600558D RID: 21901 RVA: 0x0040BF50 File Offset: 0x0040A150
		public ControlSupplyVoltageTest()
		{
			base.Name = "Easy open: Control supply voltage VIP test";
			base.Group = CodingGroup.Boot;
			this.startOption = new MQBAdaptationOption("Start", "2F040502");
			this.cancelOption = new MQBAdaptationOption("Stop", "2F040500");
			this.Password = "20103";
			this.Unit = "05";
			base.Options.Clear();
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
		}

		// Token: 0x0600558E RID: 21902 RVA: 0x0040BFE4 File Offset: 0x0040A1E4
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
				OBDRequest obdrequest5 = new OBDRequest(this.cancelOption.Value, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				if (data == null)
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
					semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
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
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
					semaphore.Release();
				}
			};
			if (optionValue != this.cancelOption.Value)
			{
				bool waitingForFinishedState = true;
				OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest2 = new OBDRequest("2F040503FFFFFFFF", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest3 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				obdrequest.ResponseReceived += responseReceivedDelegate;
				obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
				obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
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
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
				await semaphore.WaitAsync();
			}
			if (optionValue == this.cancelOption.Value)
			{
				OBDRequest obdrequest4 = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest4.ResponseReceived += checkReceivedResponseForNegativeResult;
				obdrequest4.ResponseReceived += delegate(OBDRequest request, string data)
				{
					progress.Report(Translate.GetString("coding_OperationFinished"));
					codingResult = CodingRequestResult.Success;
					semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
				await semaphore.WaitAsync();
			}
			return codingResult;
		}

		// Token: 0x04003489 RID: 13449
		private MQBAdaptationOption startOption;

		// Token: 0x02000A94 RID: 2708
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600558F RID: 21903 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06005590 RID: 21904 RVA: 0x0040C038 File Offset: 0x0040A238
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

			// Token: 0x06005591 RID: 21905 RVA: 0x0040C170 File Offset: 0x0040A370
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

			// Token: 0x06005592 RID: 21906 RVA: 0x0040C2EB File Offset: 0x0040A4EB
			internal void <OptionExecute>b__3(OBDRequest request, string data)
			{
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x0400348A RID: 13450
			public CodingRequestResult codingResult;

			// Token: 0x0400348B RID: 13451
			public SemaphoreSlim semaphore;

			// Token: 0x0400348C RID: 13452
			public ControlSupplyVoltageTest <>4__this;

			// Token: 0x0400348D RID: 13453
			public IProgress<string> progress;
		}

		// Token: 0x02000A95 RID: 2709
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_1
		{
			// Token: 0x06005593 RID: 21907 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_1()
			{
			}

			// Token: 0x06005594 RID: 21908 RVA: 0x0040C320 File Offset: 0x0040A520
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

			// Token: 0x0400348E RID: 13454
			public bool waitingForFinishedState;

			// Token: 0x0400348F RID: 13455
			public ControlSupplyVoltageTest.<>c__DisplayClass2_0 CS$<>8__locals1;
		}

		// Token: 0x02000A96 RID: 2710
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005595 RID: 21909 RVA: 0x0040C3BC File Offset: 0x0040A5BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ControlSupplyVoltageTest controlSupplyVoltageTest = this;
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
							goto IL_02A8;
						}
						CS$<>8__locals1 = new ControlSupplyVoltageTest.<>c__DisplayClass2_0();
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
							OBDRequest obdrequest5 = new OBDRequest(CS$<>8__locals1.<>4__this.cancelOption.Value, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false);
							if (data == null)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							if (data.Contains("NO DATA"))
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
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
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
								CS$<>8__locals1.semaphore.Release();
							}
						};
						if (!(optionValue != controlSupplyVoltageTest.cancelOption.Value))
						{
							goto IL_01D5;
						}
						ControlSupplyVoltageTest.<>c__DisplayClass2_1 CS$<>8__locals2 = new ControlSupplyVoltageTest.<>c__DisplayClass2_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.waitingForFinishedState = true;
						OBDRequest obdrequest = new OBDRequest(optionValue, controlSupplyVoltageTest.RequestHeader, controlSupplyVoltageTest.BeforeCommands, controlSupplyVoltageTest.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("2F040503FFFFFFFF", controlSupplyVoltageTest.RequestHeader, controlSupplyVoltageTest.BeforeCommands, controlSupplyVoltageTest.AfterCommands, false);
						OBDRequest obdrequest3 = new OBDRequest("220102", controlSupplyVoltageTest.RequestHeader, controlSupplyVoltageTest.BeforeCommands, controlSupplyVoltageTest.AfterCommands, true);
						obdrequest.ResponseReceived += responseReceivedDelegate;
						obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
						obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
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
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
						taskAwaiter = CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ControlSupplyVoltageTest.<OptionExecute>d__2>(ref taskAwaiter, ref this);
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
					IL_01D5:
					if (!(optionValue == controlSupplyVoltageTest.cancelOption.Value))
					{
						goto IL_02AF;
					}
					OBDRequest obdrequest4 = new OBDRequest(optionValue, controlSupplyVoltageTest.RequestHeader, controlSupplyVoltageTest.BeforeCommands, controlSupplyVoltageTest.AfterCommands, false);
					obdrequest4.ResponseReceived += checkReceivedResponseForNegativeResult;
					obdrequest4.ResponseReceived += delegate(OBDRequest request, string data)
					{
						CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
						CS$<>8__locals1.codingResult = CodingRequestResult.Success;
						CS$<>8__locals1.semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					};
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
					taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ControlSupplyVoltageTest.<OptionExecute>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_02A8:
					taskAwaiter.GetResult();
					IL_02AF:
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

			// Token: 0x06005596 RID: 21910 RVA: 0x0040C6EC File Offset: 0x0040A8EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003490 RID: 13456
			public int <>1__state;

			// Token: 0x04003491 RID: 13457
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003492 RID: 13458
			public ControlSupplyVoltageTest <>4__this;

			// Token: 0x04003493 RID: 13459
			public IProgress<string> progress;

			// Token: 0x04003494 RID: 13460
			public string optionValue;

			// Token: 0x04003495 RID: 13461
			private ControlSupplyVoltageTest.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x04003496 RID: 13462
			private ResponseReceivedDelegate <checkReceivedResponseForNegativeResult>5__2;

			// Token: 0x04003497 RID: 13463
			private TaskAwaiter <>u__1;
		}
	}
}
