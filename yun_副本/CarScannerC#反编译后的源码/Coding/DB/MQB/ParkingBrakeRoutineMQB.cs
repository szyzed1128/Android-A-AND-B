using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B31 RID: 2865
	internal class ParkingBrakeRoutineMQB : MQBServiceProcedure
	{
		// Token: 0x0600590B RID: 22795 RVA: 0x00427A84 File Offset: 0x00425C84
		public ParkingBrakeRoutineMQB()
		{
			base.Name = Translate.GetString("codingDB_ElectricParkingBrakeServiceRoutines_Name");
			base.InnerDescription = Translate.GetString("codingDB_ElectricParkingBrakeServiceRoutines_InnerDescription");
			this.Unit = "03";
			this.OpenOption = new MQBAdaptationOption(Translate.GetString("codingDB_1OpenForBrakePadsReplacement_Name"), "310103A1");
			this.CloseOption = new MQBAdaptationOption(Translate.GetString("codingDB_2CloseAfterPadsReplacement_Name"), "310103A0");
			this.CheckOption = new MQBAdaptationOption(Translate.GetString("codingDB_3TestParkingBrake_Name"), "31010410");
			this.ClearDTCCodesOption = new MQBAdaptationOption(Translate.GetString("codingDB_4ClearDtcCodes_Name"), "14FFFFFF");
			base.Options.Add(this.OpenOption);
			base.Options.Add(this.CloseOption);
			base.Options.Add(this.CheckOption);
			base.Options.Add(this.ClearDTCCodesOption);
		}

		// Token: 0x0600590C RID: 22796 RVA: 0x00427B70 File Offset: 0x00425D70
		protected override async Task<CodingRequestResult> OptionExecute(string optionValue, IProgress<string> progress)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
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
			if (optionValue == this.ClearDTCCodesOption.Value)
			{
				OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Contains("7F1478") || data.Contains("54"))
						{
							progress.Report(MQBAdaptationTemplate.CodingRequestResultToString(CodingRequestResult.Success));
							codingResult = CodingRequestResult.Success;
						}
					}
					semaphore.Release();
				};
				App.OBDReader.AddRequestToQueue(obdrequest);
				await semaphore.WaitAsync();
			}
			else if (optionValue == this.CheckOption.Value)
			{
				bool waitingForNonZeroState2 = true;
				OBDRequest obdrequest2 = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest3 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				obdrequest2.ResponseReceived += responseReceivedDelegate;
				obdrequest3.ResponseReceived += responseReceivedDelegate;
				obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					ParkingBrakeRoutineMQB.ReportCurrentState(progress, data);
					if (waitingForNonZeroState2)
					{
						if (data[0] != 0)
						{
							waitingForNonZeroState2 = false;
							return;
						}
					}
					else if (data[0] == 0)
					{
						progress.Report(Translate.GetString("coding_OperationFinished"));
						codingResult = CodingRequestResult.Success;
						semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					}
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, obdrequest3 });
				await semaphore.WaitAsync();
			}
			else if (optionValue == this.OpenOption.Value)
			{
				bool waitingForNonZeroState = true;
				OBDRequest obdrequest4 = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest5 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				OBDRequest finish_open_request = new OBDRequest("310203A1", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				finish_open_request.ResponseReceived += delegate(OBDRequest request, string data)
				{
					codingResult = CodingRequestResult.Success;
					semaphore.Release();
					progress.Report(Translate.GetString("coding_OperationFinished"));
				};
				DateTime firstResult2 = DateTimeNowHelper.NowSafe;
				obdrequest4.ResponseReceived += responseReceivedDelegate;
				obdrequest5.ResponseReceived += responseReceivedDelegate;
				obdrequest5.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					if ((DateTimeNowHelper.NowSafe - firstResult2).TotalSeconds > 120.0)
					{
						progress.Report(Translate.GetString("coding_OperationFinished"));
						App.OBDReader.ReplaceQueue(new OBDRequest[] { finish_open_request });
						return;
					}
					ParkingBrakeRoutineMQB.ReportCurrentState(progress, data);
					if (waitingForNonZeroState)
					{
						if (data[0] != 0)
						{
							waitingForNonZeroState = false;
							return;
						}
					}
					else if (data[0] == 0)
					{
						progress.Report(Translate.GetString("coding_OperationFinished"));
						App.OBDReader.ReplaceQueue(new OBDRequest[] { finish_open_request });
					}
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4, obdrequest5 });
				await semaphore.WaitAsync();
			}
			else if (optionValue == this.CloseOption.Value)
			{
				bool waitingForClosedState = true;
				OBDRequest obdrequest6 = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest7 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				OBDRequest finish_close_request = new OBDRequest("310203A0", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				finish_close_request.ResponseReceived += delegate(OBDRequest request, string data)
				{
					codingResult = CodingRequestResult.Success;
					semaphore.Release();
					progress.Report(Translate.GetString("coding_OperationFinished"));
				};
				DateTime firstResult = DateTimeNowHelper.NowSafe;
				obdrequest6.ResponseReceived += responseReceivedDelegate;
				obdrequest7.ResponseReceived += responseReceivedDelegate;
				obdrequest7.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					if ((DateTimeNowHelper.NowSafe - firstResult).TotalSeconds > 120.0)
					{
						progress.Report("Finishing!");
						App.OBDReader.ReplaceQueue(new OBDRequest[] { finish_close_request });
						return;
					}
					ParkingBrakeRoutineMQB.ReportCurrentState(progress, data);
					if (waitingForClosedState)
					{
						if (data[0] == 16)
						{
							waitingForClosedState = false;
							return;
						}
					}
					else
					{
						progress.Report(Translate.GetString("coding_OperationFinished"));
						App.OBDReader.ReplaceQueue(new OBDRequest[] { finish_close_request });
					}
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest6, obdrequest7 });
				await semaphore.WaitAsync();
			}
			return codingResult;
		}

		// Token: 0x0600590D RID: 22797 RVA: 0x00427BC4 File Offset: 0x00425DC4
		private static void ReportCurrentState(IProgress<string> progress, byte[] data)
		{
			if (data[0] == 192)
			{
				progress.Report(Translate.GetString("coding_OperationInProgress") + "\nParking brake released");
				return;
			}
			if (data[0] == 16)
			{
				progress.Report(Translate.GetString("coding_OperationInProgress") + " \nParking brake applied");
				return;
			}
			progress.Report(Translate.GetString("coding_OperationInProgress") + "\n" + data[0].ToString("X2"));
		}

		// Token: 0x04003777 RID: 14199
		protected MQBAdaptationOption CheckOption;

		// Token: 0x04003778 RID: 14200
		protected MQBAdaptationOption OpenOption;

		// Token: 0x04003779 RID: 14201
		protected MQBAdaptationOption CloseOption;

		// Token: 0x0400377A RID: 14202
		protected MQBAdaptationOption ClearDTCCodesOption;

		// Token: 0x02000B32 RID: 2866
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x0600590E RID: 22798 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x0600590F RID: 22799 RVA: 0x00427C44 File Offset: 0x00425E44
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

			// Token: 0x06005910 RID: 22800 RVA: 0x00427D7C File Offset: 0x00425F7C
			internal void <OptionExecute>b__1(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (data.Contains("7F1478") || data.Contains("54"))
					{
						this.progress.Report(MQBAdaptationTemplate.CodingRequestResultToString(CodingRequestResult.Success));
						this.codingResult = CodingRequestResult.Success;
					}
				}
				this.semaphore.Release();
			}

			// Token: 0x06005911 RID: 22801 RVA: 0x00427DD2 File Offset: 0x00425FD2
			internal void <OptionExecute>b__3(OBDRequest request, string data)
			{
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
			}

			// Token: 0x06005912 RID: 22802 RVA: 0x00427DD2 File Offset: 0x00425FD2
			internal void <OptionExecute>b__5(OBDRequest request, string data)
			{
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
			}

			// Token: 0x0400377B RID: 14203
			public CodingRequestResult codingResult;

			// Token: 0x0400377C RID: 14204
			public SemaphoreSlim semaphore;

			// Token: 0x0400377D RID: 14205
			public IProgress<string> progress;
		}

		// Token: 0x02000B33 RID: 2867
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_1
		{
			// Token: 0x06005913 RID: 22803 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_1()
			{
			}

			// Token: 0x06005914 RID: 22804 RVA: 0x00427DFC File Offset: 0x00425FFC
			internal void <OptionExecute>b__2(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
					this.CS$<>8__locals1.semaphore.Release();
					return;
				}
				ParkingBrakeRoutineMQB.ReportCurrentState(this.CS$<>8__locals1.progress, data);
				if (this.waitingForNonZeroState)
				{
					if (data[0] != 0)
					{
						this.waitingForNonZeroState = false;
						return;
					}
				}
				else if (data[0] == 0)
				{
					this.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
					this.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
					this.CS$<>8__locals1.semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				}
			}

			// Token: 0x0400377E RID: 14206
			public bool waitingForNonZeroState;

			// Token: 0x0400377F RID: 14207
			public ParkingBrakeRoutineMQB.<>c__DisplayClass5_0 CS$<>8__locals1;
		}

		// Token: 0x02000B34 RID: 2868
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_2
		{
			// Token: 0x06005915 RID: 22805 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_2()
			{
			}

			// Token: 0x06005916 RID: 22806 RVA: 0x00427E9C File Offset: 0x0042609C
			internal void <OptionExecute>b__4(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.CS$<>8__locals2.codingResult = CodingRequestResult.UnknownError;
					this.CS$<>8__locals2.semaphore.Release();
					return;
				}
				if ((DateTimeNowHelper.NowSafe - this.firstResult).TotalSeconds > 120.0)
				{
					this.CS$<>8__locals2.progress.Report(Translate.GetString("coding_OperationFinished"));
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.finish_open_request });
					return;
				}
				ParkingBrakeRoutineMQB.ReportCurrentState(this.CS$<>8__locals2.progress, data);
				if (this.waitingForNonZeroState)
				{
					if (data[0] != 0)
					{
						this.waitingForNonZeroState = false;
						return;
					}
				}
				else if (data[0] == 0)
				{
					this.CS$<>8__locals2.progress.Report(Translate.GetString("coding_OperationFinished"));
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.finish_open_request });
				}
			}

			// Token: 0x04003780 RID: 14208
			public DateTime firstResult;

			// Token: 0x04003781 RID: 14209
			public OBDRequest finish_open_request;

			// Token: 0x04003782 RID: 14210
			public bool waitingForNonZeroState;

			// Token: 0x04003783 RID: 14211
			public ParkingBrakeRoutineMQB.<>c__DisplayClass5_0 CS$<>8__locals2;
		}

		// Token: 0x02000B35 RID: 2869
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_3
		{
			// Token: 0x06005917 RID: 22807 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_3()
			{
			}

			// Token: 0x06005918 RID: 22808 RVA: 0x00427F84 File Offset: 0x00426184
			internal void <OptionExecute>b__6(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.CS$<>8__locals3.codingResult = CodingRequestResult.UnknownError;
					this.CS$<>8__locals3.semaphore.Release();
					return;
				}
				if ((DateTimeNowHelper.NowSafe - this.firstResult).TotalSeconds > 120.0)
				{
					this.CS$<>8__locals3.progress.Report("Finishing!");
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.finish_close_request });
					return;
				}
				ParkingBrakeRoutineMQB.ReportCurrentState(this.CS$<>8__locals3.progress, data);
				if (this.waitingForClosedState)
				{
					if (data[0] == 16)
					{
						this.waitingForClosedState = false;
						return;
					}
				}
				else
				{
					this.CS$<>8__locals3.progress.Report(Translate.GetString("coding_OperationFinished"));
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.finish_close_request });
				}
			}

			// Token: 0x04003784 RID: 14212
			public DateTime firstResult;

			// Token: 0x04003785 RID: 14213
			public OBDRequest finish_close_request;

			// Token: 0x04003786 RID: 14214
			public bool waitingForClosedState;

			// Token: 0x04003787 RID: 14215
			public ParkingBrakeRoutineMQB.<>c__DisplayClass5_0 CS$<>8__locals3;
		}

		// Token: 0x02000B36 RID: 2870
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__5 : IAsyncStateMachine
		{
			// Token: 0x06005919 RID: 22809 RVA: 0x00428064 File Offset: 0x00426264
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ParkingBrakeRoutineMQB parkingBrakeRoutineMQB = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0252;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03C2;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_052F;
					}
					default:
					{
						CS$<>8__locals1 = new ParkingBrakeRoutineMQB.<>c__DisplayClass5_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
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
						if (optionValue == parkingBrakeRoutineMQB.ClearDTCCodesOption.Value)
						{
							OBDRequest obdrequest = new OBDRequest(optionValue, parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, false);
							obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
							{
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
									if (data.Contains("7F1478") || data.Contains("54"))
									{
										CS$<>8__locals1.progress.Report(MQBAdaptationTemplate.CodingRequestResultToString(CodingRequestResult.Success));
										CS$<>8__locals1.codingResult = CodingRequestResult.Success;
									}
								}
								CS$<>8__locals1.semaphore.Release();
							};
							App.OBDReader.AddRequestToQueue(obdrequest);
							taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ParkingBrakeRoutineMQB.<OptionExecute>d__5>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (optionValue == parkingBrakeRoutineMQB.CheckOption.Value)
						{
							ParkingBrakeRoutineMQB.<>c__DisplayClass5_1 CS$<>8__locals2 = new ParkingBrakeRoutineMQB.<>c__DisplayClass5_1();
							CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
							CS$<>8__locals2.waitingForNonZeroState = true;
							OBDRequest obdrequest2 = new OBDRequest(optionValue, parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, false);
							OBDRequest obdrequest3 = new OBDRequest("220102", parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, true);
							obdrequest2.ResponseReceived += responseReceivedDelegate;
							obdrequest3.ResponseReceived += responseReceivedDelegate;
							obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
							{
								if (data == null || data.Length == 0)
								{
									CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
									CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
									return;
								}
								ParkingBrakeRoutineMQB.ReportCurrentState(CS$<>8__locals2.CS$<>8__locals1.progress, data);
								if (CS$<>8__locals2.waitingForNonZeroState)
								{
									if (data[0] != 0)
									{
										CS$<>8__locals2.waitingForNonZeroState = false;
										return;
									}
								}
								else if (data[0] == 0)
								{
									CS$<>8__locals2.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
									CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
									CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
									App.OBDReader.ClearRequestQueue();
								}
							};
							App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, obdrequest3 });
							taskAwaiter = CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ParkingBrakeRoutineMQB.<OptionExecute>d__5>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0252;
						}
						else if (optionValue == parkingBrakeRoutineMQB.OpenOption.Value)
						{
							ParkingBrakeRoutineMQB.<>c__DisplayClass5_2 CS$<>8__locals3 = new ParkingBrakeRoutineMQB.<>c__DisplayClass5_2();
							CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
							CS$<>8__locals3.waitingForNonZeroState = true;
							OBDRequest obdrequest4 = new OBDRequest(optionValue, parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, false);
							OBDRequest obdrequest5 = new OBDRequest("220102", parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, true);
							CS$<>8__locals3.finish_open_request = new OBDRequest("310203A1", parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, false);
							CS$<>8__locals3.finish_open_request.ResponseReceived += delegate(OBDRequest request, string data)
							{
								CS$<>8__locals3.CS$<>8__locals2.codingResult = CodingRequestResult.Success;
								CS$<>8__locals3.CS$<>8__locals2.semaphore.Release();
								CS$<>8__locals3.CS$<>8__locals2.progress.Report(Translate.GetString("coding_OperationFinished"));
							};
							CS$<>8__locals3.firstResult = DateTimeNowHelper.NowSafe;
							obdrequest4.ResponseReceived += responseReceivedDelegate;
							obdrequest5.ResponseReceived += responseReceivedDelegate;
							obdrequest5.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
							{
								if (data == null || data.Length == 0)
								{
									CS$<>8__locals3.CS$<>8__locals2.codingResult = CodingRequestResult.UnknownError;
									CS$<>8__locals3.CS$<>8__locals2.semaphore.Release();
									return;
								}
								if ((DateTimeNowHelper.NowSafe - CS$<>8__locals3.firstResult).TotalSeconds > 120.0)
								{
									CS$<>8__locals3.CS$<>8__locals2.progress.Report(Translate.GetString("coding_OperationFinished"));
									App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals3.finish_open_request });
									return;
								}
								ParkingBrakeRoutineMQB.ReportCurrentState(CS$<>8__locals3.CS$<>8__locals2.progress, data);
								if (CS$<>8__locals3.waitingForNonZeroState)
								{
									if (data[0] != 0)
									{
										CS$<>8__locals3.waitingForNonZeroState = false;
										return;
									}
								}
								else if (data[0] == 0)
								{
									CS$<>8__locals3.CS$<>8__locals2.progress.Report(Translate.GetString("coding_OperationFinished"));
									App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals3.finish_open_request });
								}
							};
							App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4, obdrequest5 });
							taskAwaiter = CS$<>8__locals3.CS$<>8__locals2.semaphore.WaitAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ParkingBrakeRoutineMQB.<OptionExecute>d__5>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_03C2;
						}
						else
						{
							if (!(optionValue == parkingBrakeRoutineMQB.CloseOption.Value))
							{
								goto IL_0536;
							}
							ParkingBrakeRoutineMQB.<>c__DisplayClass5_3 CS$<>8__locals4 = new ParkingBrakeRoutineMQB.<>c__DisplayClass5_3();
							CS$<>8__locals4.CS$<>8__locals3 = CS$<>8__locals1;
							CS$<>8__locals4.waitingForClosedState = true;
							OBDRequest obdrequest6 = new OBDRequest(optionValue, parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, false);
							OBDRequest obdrequest7 = new OBDRequest("220102", parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, true);
							CS$<>8__locals4.finish_close_request = new OBDRequest("310203A0", parkingBrakeRoutineMQB.RequestHeader, parkingBrakeRoutineMQB.BeforeCommands, parkingBrakeRoutineMQB.AfterCommands, false);
							CS$<>8__locals4.finish_close_request.ResponseReceived += delegate(OBDRequest request, string data)
							{
								CS$<>8__locals4.CS$<>8__locals3.codingResult = CodingRequestResult.Success;
								CS$<>8__locals4.CS$<>8__locals3.semaphore.Release();
								CS$<>8__locals4.CS$<>8__locals3.progress.Report(Translate.GetString("coding_OperationFinished"));
							};
							CS$<>8__locals4.firstResult = DateTimeNowHelper.NowSafe;
							obdrequest6.ResponseReceived += responseReceivedDelegate;
							obdrequest7.ResponseReceived += responseReceivedDelegate;
							obdrequest7.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
							{
								if (data == null || data.Length == 0)
								{
									CS$<>8__locals4.CS$<>8__locals3.codingResult = CodingRequestResult.UnknownError;
									CS$<>8__locals4.CS$<>8__locals3.semaphore.Release();
									return;
								}
								if ((DateTimeNowHelper.NowSafe - CS$<>8__locals4.firstResult).TotalSeconds > 120.0)
								{
									CS$<>8__locals4.CS$<>8__locals3.progress.Report("Finishing!");
									App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals4.finish_close_request });
									return;
								}
								ParkingBrakeRoutineMQB.ReportCurrentState(CS$<>8__locals4.CS$<>8__locals3.progress, data);
								if (CS$<>8__locals4.waitingForClosedState)
								{
									if (data[0] == 16)
									{
										CS$<>8__locals4.waitingForClosedState = false;
										return;
									}
								}
								else
								{
									CS$<>8__locals4.CS$<>8__locals3.progress.Report(Translate.GetString("coding_OperationFinished"));
									App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals4.finish_close_request });
								}
							};
							App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest6, obdrequest7 });
							taskAwaiter = CS$<>8__locals4.CS$<>8__locals3.semaphore.WaitAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 3;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ParkingBrakeRoutineMQB.<OptionExecute>d__5>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_052F;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					goto IL_0536;
					IL_0252:
					taskAwaiter.GetResult();
					goto IL_0536;
					IL_03C2:
					taskAwaiter.GetResult();
					goto IL_0536;
					IL_052F:
					taskAwaiter.GetResult();
					IL_0536:
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

			// Token: 0x0600591A RID: 22810 RVA: 0x0042860C File Offset: 0x0042680C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003788 RID: 14216
			public int <>1__state;

			// Token: 0x04003789 RID: 14217
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400378A RID: 14218
			public IProgress<string> progress;

			// Token: 0x0400378B RID: 14219
			public string optionValue;

			// Token: 0x0400378C RID: 14220
			public ParkingBrakeRoutineMQB <>4__this;

			// Token: 0x0400378D RID: 14221
			private ParkingBrakeRoutineMQB.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x0400378E RID: 14222
			private TaskAwaiter <>u__1;
		}
	}
}
