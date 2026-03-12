using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B9F RID: 2975
	internal class HyundaiKiaCyclingProcedurePrimitive : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005AA4 RID: 23204 RVA: 0x004340EC File Offset: 0x004322EC
		public HyundaiKiaCyclingProcedurePrimitive(string[] cycle_commands, string request_header, string response_header, string open_session_commands, int delay_between_ms, int total_time_sec, bool finishWithLast)
		{
			base.RequestHeader = request_header;
			base.ResponseHeader = response_header;
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Start", "", new TranslationItem[]
			{
				new TranslationItem("ru", "Запуск", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			base.Group = CodingGroup.ServiceProcedures;
			this.HasCurrentState = false;
			this.openSessionCommands = open_session_commands;
			this.cycleCommands = cycle_commands;
			this.delayBetweenRequestsMs = delay_between_ms;
			this.totalTimeSec = total_time_sec;
			this.finishWithLast = finishWithLast;
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x004341CC File Offset: 0x004323CC
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = string.Concat(new string[] { "ATSP6;ATSH", base.RequestHeader, ";ATFCSH", base.RequestHeader, ";ATFCSD300000;ATFCSM1;ATCRA", base.ResponseHeader, ";", this.openSessionCommands });
			this.AfterCommands = this.closeSessionCommands + ";ATAR;ATSH7DF;ATFCSM1";
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x00434244 File Offset: 0x00432444
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			return CodingRequestResult.Success;
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x00434280 File Offset: 0x00432480
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult requestResult = CodingRequestResult.Success;
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			DateTime timeStarted = DateTimeNowHelper.NowSafe;
			List<OBDRequest> cycle_requests = new List<OBDRequest>(this.cycleCommands.Length);
			string[] array = this.cycleCommands;
			ResponseReceivedDelegate <>9__0;
			for (int i = 0; i < array.Length; i++)
			{
				OBDRequest obdrequest = new OBDRequest(array[i], base.RequestHeader, this.BeforeCommands, this.AfterCommands, true, new List<PID>(0))
				{
					DoNotDecode = true
				};
				OBDRequest obdrequest2 = obdrequest;
				ResponseReceivedDelegate responseReceivedDelegate;
				if ((responseReceivedDelegate = <>9__0) == null)
				{
					responseReceivedDelegate = (<>9__0 = delegate(OBDRequest request, string data)
					{
						for (int j = 0; j < 5; j++)
						{
							Task.Run(async delegate
							{
								await Task.Delay(1000);
								await App.OBDReader.SendString("3E00");
								await App.OBDReader.ReadData(2500, null, -1);
							}).Wait();
						}
						long num = DateTimeNowHelper.NowSafe.Ticks - timeStarted.Ticks;
						TimeSpan timeSpan = new TimeSpan(num);
						if (timeSpan.TotalSeconds >= (double)this.totalTimeSec && ((this.finishWithLast && request == cycle_requests[cycle_requests.Count - 1]) || (!this.finishWithLast && request == cycle_requests[0])))
						{
							progress.Report(string.Format("{0}/{1} sec.", (int)timeSpan.TotalSeconds, this.totalTimeSec));
							App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							semaphore.Release();
							return;
						}
						progress.Report(string.Format("{0}/{1} sec.", (int)timeSpan.TotalSeconds, this.totalTimeSec));
					});
				}
				obdrequest2.ResponseReceived += responseReceivedDelegate;
				cycle_requests.Add(obdrequest);
			}
			App.OBDReader.ReplaceQueue(cycle_requests);
			await semaphore.WaitAsync();
			return requestResult;
		}

		// Token: 0x040038FA RID: 14586
		protected bool finishWithLast = true;

		// Token: 0x040038FB RID: 14587
		protected string openSessionCommands = "";

		// Token: 0x040038FC RID: 14588
		protected string closeSessionCommands = "";

		// Token: 0x040038FD RID: 14589
		protected string[] cycleCommands = new string[0];

		// Token: 0x040038FE RID: 14590
		private int delayBetweenRequestsMs;

		// Token: 0x040038FF RID: 14591
		private int totalTimeSec;

		// Token: 0x02000BA0 RID: 2976
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005AA8 RID: 23208 RVA: 0x004342CC File Offset: 0x004324CC
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005AA9 RID: 23209 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005AAA RID: 23210 RVA: 0x004342D8 File Offset: 0x004324D8
			internal async Task <Execute>b__9_1()
			{
				await Task.Delay(1000);
				await App.OBDReader.SendString("3E00");
				await App.OBDReader.ReadData(2500, null, -1);
			}

			// Token: 0x04003900 RID: 14592
			public static readonly HyundaiKiaCyclingProcedurePrimitive.<>c <>9 = new HyundaiKiaCyclingProcedurePrimitive.<>c();

			// Token: 0x04003901 RID: 14593
			public static Func<Task> <>9__9_1;

			// Token: 0x02000BA1 RID: 2977
			[StructLayout(LayoutKind.Auto)]
			private struct <<Execute>b__9_1>d : IAsyncStateMachine
			{
				// Token: 0x06005AAB RID: 23211 RVA: 0x00434314 File Offset: 0x00432514
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					try
					{
						TaskAwaiter taskAwaiter;
						TaskAwaiter<string> taskAwaiter3;
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
							goto IL_00D5;
						}
						case 2:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_0138;
						}
						default:
							taskAwaiter = Task.Delay(1000).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HyundaiKiaCyclingProcedurePrimitive.<>c.<<Execute>b__9_1>d>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						taskAwaiter.GetResult();
						taskAwaiter = App.OBDReader.SendString("3E00").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HyundaiKiaCyclingProcedurePrimitive.<>c.<<Execute>b__9_1>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_00D5:
						taskAwaiter.GetResult();
						taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, HyundaiKiaCyclingProcedurePrimitive.<>c.<<Execute>b__9_1>d>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0138:
						taskAwaiter3.GetResult();
					}
					catch (Exception ex)
					{
						num2 = -2;
						this.<>t__builder.SetException(ex);
						return;
					}
					num2 = -2;
					this.<>t__builder.SetResult();
				}

				// Token: 0x06005AAC RID: 23212 RVA: 0x004344AC File Offset: 0x004326AC
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04003902 RID: 14594
				public int <>1__state;

				// Token: 0x04003903 RID: 14595
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04003904 RID: 14596
				private TaskAwaiter <>u__1;

				// Token: 0x04003905 RID: 14597
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x02000BA2 RID: 2978
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x06005AAD RID: 23213 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x06005AAE RID: 23214 RVA: 0x004344BC File Offset: 0x004326BC
			internal void <Execute>b__0(OBDRequest request, string data)
			{
				for (int i = 0; i < 5; i++)
				{
					Task.Run(async delegate
					{
						await Task.Delay(1000);
						await App.OBDReader.SendString("3E00");
						await App.OBDReader.ReadData(2500, null, -1);
					}).Wait();
				}
				long num = DateTimeNowHelper.NowSafe.Ticks - this.timeStarted.Ticks;
				TimeSpan timeSpan = new TimeSpan(num);
				if (timeSpan.TotalSeconds >= (double)this.<>4__this.totalTimeSec && ((this.<>4__this.finishWithLast && request == this.cycle_requests[this.cycle_requests.Count - 1]) || (!this.<>4__this.finishWithLast && request == this.cycle_requests[0])))
				{
					this.progress.Report(string.Format("{0}/{1} sec.", (int)timeSpan.TotalSeconds, this.<>4__this.totalTimeSec));
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
					return;
				}
				this.progress.Report(string.Format("{0}/{1} sec.", (int)timeSpan.TotalSeconds, this.<>4__this.totalTimeSec));
			}

			// Token: 0x04003906 RID: 14598
			public DateTime timeStarted;

			// Token: 0x04003907 RID: 14599
			public HyundaiKiaCyclingProcedurePrimitive <>4__this;

			// Token: 0x04003908 RID: 14600
			public List<OBDRequest> cycle_requests;

			// Token: 0x04003909 RID: 14601
			public IProgress<string> progress;

			// Token: 0x0400390A RID: 14602
			public SemaphoreSlim semaphore;

			// Token: 0x0400390B RID: 14603
			public ResponseReceivedDelegate <>9__0;
		}

		// Token: 0x02000BA3 RID: 2979
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__9 : IAsyncStateMachine
		{
			// Token: 0x06005AAF RID: 23215 RVA: 0x00434600 File Offset: 0x00432800
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				HyundaiKiaCyclingProcedurePrimitive hyundaiKiaCyclingProcedurePrimitive = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						HyundaiKiaCyclingProcedurePrimitive.<>c__DisplayClass9_0 CS$<>8__locals1 = new HyundaiKiaCyclingProcedurePrimitive.<>c__DisplayClass9_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						requestResult = CodingRequestResult.Success;
						hyundaiKiaCyclingProcedurePrimitive.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.timeStarted = DateTimeNowHelper.NowSafe;
						CS$<>8__locals1.cycle_requests = new List<OBDRequest>(hyundaiKiaCyclingProcedurePrimitive.cycleCommands.Length);
						string[] cycleCommands = hyundaiKiaCyclingProcedurePrimitive.cycleCommands;
						for (int i = 0; i < cycleCommands.Length; i++)
						{
							OBDRequest obdrequest = new OBDRequest(cycleCommands[i], hyundaiKiaCyclingProcedurePrimitive.RequestHeader, hyundaiKiaCyclingProcedurePrimitive.BeforeCommands, hyundaiKiaCyclingProcedurePrimitive.AfterCommands, true, new List<PID>(0))
							{
								DoNotDecode = true
							};
							OBDRequest obdrequest2 = obdrequest;
							ResponseReceivedDelegate responseReceivedDelegate;
							if ((responseReceivedDelegate = CS$<>8__locals1.<>9__0) == null)
							{
								responseReceivedDelegate = (CS$<>8__locals1.<>9__0 = delegate(OBDRequest request, string data)
								{
									for (int j = 0; j < 5; j++)
									{
										Task.Run(async delegate
										{
											await Task.Delay(1000);
											await App.OBDReader.SendString("3E00");
											await App.OBDReader.ReadData(2500, null, -1);
										}).Wait();
									}
									long num3 = DateTimeNowHelper.NowSafe.Ticks - CS$<>8__locals1.timeStarted.Ticks;
									TimeSpan timeSpan = new TimeSpan(num3);
									if (timeSpan.TotalSeconds >= (double)CS$<>8__locals1.<>4__this.totalTimeSec && ((CS$<>8__locals1.<>4__this.finishWithLast && request == CS$<>8__locals1.cycle_requests[CS$<>8__locals1.cycle_requests.Count - 1]) || (!CS$<>8__locals1.<>4__this.finishWithLast && request == CS$<>8__locals1.cycle_requests[0])))
									{
										CS$<>8__locals1.progress.Report(string.Format("{0}/{1} sec.", (int)timeSpan.TotalSeconds, CS$<>8__locals1.<>4__this.totalTimeSec));
										App.OBDReader.ReplaceQueue(new OBDRequest[0]);
										CS$<>8__locals1.semaphore.Release();
										return;
									}
									CS$<>8__locals1.progress.Report(string.Format("{0}/{1} sec.", (int)timeSpan.TotalSeconds, CS$<>8__locals1.<>4__this.totalTimeSec));
								});
							}
							obdrequest2.ResponseReceived += responseReceivedDelegate;
							CS$<>8__locals1.cycle_requests.Add(obdrequest);
						}
						App.OBDReader.ReplaceQueue(CS$<>8__locals1.cycle_requests);
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HyundaiKiaCyclingProcedurePrimitive.<Execute>d__9>(ref taskAwaiter, ref this);
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
					codingRequestResult = requestResult;
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

			// Token: 0x06005AB0 RID: 23216 RVA: 0x004347B8 File Offset: 0x004329B8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400390C RID: 14604
			public int <>1__state;

			// Token: 0x0400390D RID: 14605
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400390E RID: 14606
			public HyundaiKiaCyclingProcedurePrimitive <>4__this;

			// Token: 0x0400390F RID: 14607
			public IProgress<string> progress;

			// Token: 0x04003910 RID: 14608
			private CodingRequestResult <requestResult>5__2;

			// Token: 0x04003911 RID: 14609
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000BA4 RID: 2980
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__8 : IAsyncStateMachine
		{
			// Token: 0x06005AB1 RID: 23217 RVA: 0x004347C8 File Offset: 0x004329C8
			void IAsyncStateMachine.MoveNext()
			{
				CodingRequestResult codingRequestResult;
				try
				{
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

			// Token: 0x06005AB2 RID: 23218 RVA: 0x00434814 File Offset: 0x00432A14
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003912 RID: 14610
			public int <>1__state;

			// Token: 0x04003913 RID: 14611
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;
		}
	}
}
