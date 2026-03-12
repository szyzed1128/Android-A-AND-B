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
	// Token: 0x02000A9F RID: 2719
	internal class DPFServiceRegeneration : MQBServiceProcedure
	{
		// Token: 0x060055F4 RID: 22004 RVA: 0x0040F350 File Offset: 0x0040D550
		public DPFServiceRegeneration()
		{
			base.Name = Translate.GetString("codingDB_DpfServiceRegenerationWhileStanding_Name");
			base.InnerDescription = Translate.GetString("codingDB_DpfServiceRegenerationWhileStanding_InnerDescription");
			this.Unit = "01";
			this.startOption = new MQBAdaptationOption(Translate.GetString("codingDB_opt_Start"), "31010305040000");
			this.cancelOption = new MQBAdaptationOption(Translate.GetString("btnCancel.Content"), "31020305");
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
			this.status_string_0102 = "";
			this.status_string_0104 = "";
			this.Password = "27971";
			base.PasswordVisible = true;
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x0040F40C File Offset: 0x0040D60C
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

		// Token: 0x040034CE RID: 13518
		protected MQBAdaptationOption startOption;

		// Token: 0x040034CF RID: 13519
		protected new MQBAdaptationOption cancelOption;

		// Token: 0x040034D0 RID: 13520
		protected string status_string_0102;

		// Token: 0x040034D1 RID: 13521
		protected string status_string_0104;

		// Token: 0x02000AA0 RID: 2720
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x060055F6 RID: 22006 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x060055F7 RID: 22007 RVA: 0x0040F460 File Offset: 0x0040D660
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

			// Token: 0x060055F8 RID: 22008 RVA: 0x0040F598 File Offset: 0x0040D798
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

			// Token: 0x060055F9 RID: 22009 RVA: 0x0040F714 File Offset: 0x0040D914
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

			// Token: 0x060055FA RID: 22010 RVA: 0x0040F77E File Offset: 0x0040D97E
			internal void <OptionExecute>b__4(OBDRequest request, string data)
			{
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x040034D2 RID: 13522
			public CodingRequestResult codingResult;

			// Token: 0x040034D3 RID: 13523
			public SemaphoreSlim semaphore;

			// Token: 0x040034D4 RID: 13524
			public DPFServiceRegeneration <>4__this;

			// Token: 0x040034D5 RID: 13525
			public IProgress<string> progress;
		}

		// Token: 0x02000AA1 RID: 2721
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_1
		{
			// Token: 0x060055FB RID: 22011 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_1()
			{
			}

			// Token: 0x060055FC RID: 22012 RVA: 0x0040F7B4 File Offset: 0x0040D9B4
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

			// Token: 0x040034D6 RID: 13526
			public bool status0102_finished;

			// Token: 0x040034D7 RID: 13527
			public DPFServiceRegeneration.<>c__DisplayClass5_0 CS$<>8__locals1;
		}

		// Token: 0x02000AA2 RID: 2722
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__5 : IAsyncStateMachine
		{
			// Token: 0x060055FD RID: 22013 RVA: 0x0040F888 File Offset: 0x0040DA88
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DPFServiceRegeneration dpfserviceRegeneration = this;
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
						CS$<>8__locals1 = new DPFServiceRegeneration.<>c__DisplayClass5_0();
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
						if (!(optionValue == dpfserviceRegeneration.startOption.Value))
						{
							goto IL_01FB;
						}
						DPFServiceRegeneration.<>c__DisplayClass5_1 CS$<>8__locals2 = new DPFServiceRegeneration.<>c__DisplayClass5_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						OBDRequest obdrequest = new OBDRequest(optionValue, dpfserviceRegeneration.RequestHeader, dpfserviceRegeneration.BeforeCommands, dpfserviceRegeneration.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("220102", dpfserviceRegeneration.RequestHeader, dpfserviceRegeneration.BeforeCommands, dpfserviceRegeneration.AfterCommands, true);
						OBDRequest obdrequest3 = new OBDRequest("220104", dpfserviceRegeneration.RequestHeader, dpfserviceRegeneration.BeforeCommands, dpfserviceRegeneration.AfterCommands, true);
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DPFServiceRegeneration.<OptionExecute>d__5>(ref taskAwaiter, ref this);
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
					if (!(optionValue == dpfserviceRegeneration.cancelOption.Value))
					{
						goto IL_02D5;
					}
					OBDRequest obdrequest4 = new OBDRequest(optionValue, dpfserviceRegeneration.RequestHeader, dpfserviceRegeneration.BeforeCommands, dpfserviceRegeneration.AfterCommands, false);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DPFServiceRegeneration.<OptionExecute>d__5>(ref taskAwaiter, ref this);
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

			// Token: 0x060055FE RID: 22014 RVA: 0x0040FBE0 File Offset: 0x0040DDE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040034D8 RID: 13528
			public int <>1__state;

			// Token: 0x040034D9 RID: 13529
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040034DA RID: 13530
			public DPFServiceRegeneration <>4__this;

			// Token: 0x040034DB RID: 13531
			public IProgress<string> progress;

			// Token: 0x040034DC RID: 13532
			public string optionValue;

			// Token: 0x040034DD RID: 13533
			private DPFServiceRegeneration.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x040034DE RID: 13534
			private ResponseReceivedDelegate <checkReceivedResponseForNegativeResult>5__2;

			// Token: 0x040034DF RID: 13535
			private TaskAwaiter <>u__1;
		}
	}
}
