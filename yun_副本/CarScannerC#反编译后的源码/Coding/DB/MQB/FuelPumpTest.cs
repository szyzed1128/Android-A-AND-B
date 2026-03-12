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
	// Token: 0x02000ADE RID: 2782
	internal class FuelPumpTest : MQBServiceProcedure
	{
		// Token: 0x0600574B RID: 22347 RVA: 0x00419E1C File Offset: 0x0041801C
		public FuelPumpTest()
		{
			base.Name = Translate.GetString("codingDB_TransferFuelPumpTest_Name");
			base.Description = Translate.GetString("codingDB_TransferFuelPumpTest_Description");
			base.InnerDescription = Translate.GetString("codingDB_TransferFuelPumpTest_InnerDescription");
			this.Unit = "01";
			this.startOption = new MQBAdaptationOption(Translate.GetString("coding_Start"), "3101030D040000");
			this.stopOption = new MQBAdaptationOption(Translate.GetString("coding_Stop"), "3102030D");
			base.Options.Add(this.startOption);
			base.Options.Add(this.stopOption);
			this.status_string_0102 = "";
			this.status_string_0104 = "";
			this.Password = "27971";
			base.PasswordVisible = true;
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x00419EE8 File Offset: 0x004180E8
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
				OBDRequest obdrequest5 = new OBDRequest(this.stopOption.Value, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
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
			if (optionValue == this.startOption.Value)
			{
				OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest2 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				OBDRequest obdrequest3 = new OBDRequest("220104", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				obdrequest.ResponseReceived += responseReceivedDelegate;
				obdrequest2.ResponseReceived += checkReceivedResponseForNegativeResult;
				obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
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
			if (optionValue == this.stopOption.Value)
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

		// Token: 0x040035C8 RID: 13768
		protected MQBAdaptationOption startOption;

		// Token: 0x040035C9 RID: 13769
		protected MQBAdaptationOption stopOption;

		// Token: 0x040035CA RID: 13770
		private string status_string_0102;

		// Token: 0x040035CB RID: 13771
		private string status_string_0104;

		// Token: 0x02000ADF RID: 2783
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x0600574D RID: 22349 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x0600574E RID: 22350 RVA: 0x00419F3C File Offset: 0x0041813C
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

			// Token: 0x0600574F RID: 22351 RVA: 0x0041A074 File Offset: 0x00418274
			internal void <OptionExecute>b__1(OBDRequest request, string data)
			{
				OBDRequest obdrequest = new OBDRequest(this.<>4__this.stopOption.Value, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false);
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

			// Token: 0x06005750 RID: 22352 RVA: 0x0041A1F0 File Offset: 0x004183F0
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

			// Token: 0x06005751 RID: 22353 RVA: 0x0041A25A File Offset: 0x0041845A
			internal void <OptionExecute>b__4(OBDRequest request, string data)
			{
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x040035CC RID: 13772
			public CodingRequestResult codingResult;

			// Token: 0x040035CD RID: 13773
			public SemaphoreSlim semaphore;

			// Token: 0x040035CE RID: 13774
			public FuelPumpTest <>4__this;

			// Token: 0x040035CF RID: 13775
			public IProgress<string> progress;
		}

		// Token: 0x02000AE0 RID: 2784
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_1
		{
			// Token: 0x06005752 RID: 22354 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_1()
			{
			}

			// Token: 0x06005753 RID: 22355 RVA: 0x0041A290 File Offset: 0x00418490
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

			// Token: 0x040035D0 RID: 13776
			public bool status0102_finished;

			// Token: 0x040035D1 RID: 13777
			public FuelPumpTest.<>c__DisplayClass5_0 CS$<>8__locals1;
		}

		// Token: 0x02000AE1 RID: 2785
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__5 : IAsyncStateMachine
		{
			// Token: 0x06005754 RID: 22356 RVA: 0x0041A364 File Offset: 0x00418564
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FuelPumpTest fuelPumpTest = this;
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
							goto IL_02CE;
						}
						CS$<>8__locals1 = new FuelPumpTest.<>c__DisplayClass5_0();
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
							OBDRequest obdrequest5 = new OBDRequest(CS$<>8__locals1.<>4__this.stopOption.Value, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false);
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
						if (!(optionValue == fuelPumpTest.startOption.Value))
						{
							goto IL_01FB;
						}
						FuelPumpTest.<>c__DisplayClass5_1 CS$<>8__locals2 = new FuelPumpTest.<>c__DisplayClass5_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						OBDRequest obdrequest = new OBDRequest(optionValue, fuelPumpTest.RequestHeader, fuelPumpTest.BeforeCommands, fuelPumpTest.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("220102", fuelPumpTest.RequestHeader, fuelPumpTest.BeforeCommands, fuelPumpTest.AfterCommands, true);
						OBDRequest obdrequest3 = new OBDRequest("220104", fuelPumpTest.RequestHeader, fuelPumpTest.BeforeCommands, fuelPumpTest.AfterCommands, true);
						obdrequest.ResponseReceived += responseReceivedDelegate;
						obdrequest2.ResponseReceived += checkReceivedResponseForNegativeResult;
						obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FuelPumpTest.<OptionExecute>d__5>(ref taskAwaiter, ref this);
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
					IL_01FB:
					if (!(optionValue == fuelPumpTest.stopOption.Value))
					{
						goto IL_02D5;
					}
					OBDRequest obdrequest4 = new OBDRequest(optionValue, fuelPumpTest.RequestHeader, fuelPumpTest.BeforeCommands, fuelPumpTest.AfterCommands, false);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FuelPumpTest.<OptionExecute>d__5>(ref taskAwaiter, ref this);
						return;
					}
					IL_02CE:
					taskAwaiter.GetResult();
					IL_02D5:
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

			// Token: 0x06005755 RID: 22357 RVA: 0x0041A6BC File Offset: 0x004188BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040035D2 RID: 13778
			public int <>1__state;

			// Token: 0x040035D3 RID: 13779
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040035D4 RID: 13780
			public FuelPumpTest <>4__this;

			// Token: 0x040035D5 RID: 13781
			public IProgress<string> progress;

			// Token: 0x040035D6 RID: 13782
			public string optionValue;

			// Token: 0x040035D7 RID: 13783
			private FuelPumpTest.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x040035D8 RID: 13784
			private ResponseReceivedDelegate <checkReceivedResponseForNegativeResult>5__2;

			// Token: 0x040035D9 RID: 13785
			private TaskAwaiter <>u__1;
		}
	}
}
