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
	// Token: 0x020009B2 RID: 2482
	internal class GMResetServiceCANVar1 : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x060050D4 RID: 20692 RVA: 0x003EC438 File Offset: 0x003EA638
		public GMResetServiceCANVar1()
		{
			base.Name = "Reset service reminder";
			base.Description = "Compatibility confirmed with: Chevrolet Captiva/Opel Antara/Daewoo Windstorm/Holden Captiva gen. 1 (2006-2018). Other GM-Opel based models not tested, but could be compatible too.";
			base.Translations.Add(new TranslationItem("ru", "Сброс межсервисного интервала", "Совместимость подтверждена с Chevrolet Captiva, Opel Antara первого поколения (2006-2018). Может подойти и для других моделей на платформах GM-Opel.", ""));
			base.RequestHeader = "";
			base.ResponseHeader = "";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset full length (100%)", "FF", new TranslationItem[]
			{
				new TranslationItem("ru", "Сбросить интервал на 100%", "", "")
			}));
			base.Options.Add(new MQBAdaptationOption("Reset 1/2 length (50%)", "80", new TranslationItem[]
			{
				new TranslationItem("ru", "Сбросить на 1/2 (50%)", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
			base.Group = CodingGroup.ServiceProcedures;
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x003EC53F File Offset: 0x003EA73F
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "ATSP6;ATSH7DF";
			this.AfterCommands = "";
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x003EC558 File Offset: 0x003EA758
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest("1A6D", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
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
			if (requestResult == CodingRequestResult.Success && result != null && result.Length != 0)
			{
				base.CurrentState = ((double)result[0] * 100.0 / 255.0).ToString("0");
			}
			else
			{
				base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(requestResult);
			}
			return requestResult;
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x003EC59C File Offset: 0x003EA79C
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest("3B6D" + value, base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest2 = new OBDRequest("1A6D", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			byte[] result = null;
			obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0 && (int)data[0] == int.Parse(value, NumberStyles.HexNumber))
				{
					result = data;
					requestResult = CodingRequestResult.Success;
				}
				else
				{
					requestResult = CodingRequestResult.NotSupported;
				}
				semaphore.Release();
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
			await semaphore.WaitAsync();
			if (requestResult == CodingRequestResult.Success && result != null && result.Length != 0)
			{
				base.CurrentState = ((double)result[0] * 100.0 / 255.0).ToString("0");
			}
			else
			{
				base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(requestResult);
			}
			return requestResult;
		}

		// Token: 0x020009B3 RID: 2483
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060050D8 RID: 20696 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060050D9 RID: 20697 RVA: 0x003EC5E7 File Offset: 0x003EA7E7
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

			// Token: 0x040030CB RID: 12491
			public CodingRequestResult requestResult;

			// Token: 0x040030CC RID: 12492
			public byte[] result;

			// Token: 0x040030CD RID: 12493
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020009B4 RID: 2484
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060050DA RID: 20698 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060050DB RID: 20699 RVA: 0x003EC614 File Offset: 0x003EA814
			internal void <Execute>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0 && (int)data[0] == int.Parse(this.value, NumberStyles.HexNumber))
				{
					this.result = data;
					this.requestResult = CodingRequestResult.Success;
				}
				else
				{
					this.requestResult = CodingRequestResult.NotSupported;
				}
				this.semaphore.Release();
			}

			// Token: 0x040030CE RID: 12494
			public string value;

			// Token: 0x040030CF RID: 12495
			public byte[] result;

			// Token: 0x040030D0 RID: 12496
			public CodingRequestResult requestResult;

			// Token: 0x040030D1 RID: 12497
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020009B5 RID: 2485
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x060050DC RID: 20700 RVA: 0x003EC664 File Offset: 0x003EA864
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				GMResetServiceCANVar1 gmresetServiceCANVar = this;
				CodingRequestResult requestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new GMResetServiceCANVar1.<>c__DisplayClass3_0();
						CS$<>8__locals1.value = value;
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						gmresetServiceCANVar.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest("3B6D" + CS$<>8__locals1.value, gmresetServiceCANVar.RequestHeader, gmresetServiceCANVar.BeforeCommands, gmresetServiceCANVar.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("1A6D", gmresetServiceCANVar.RequestHeader, gmresetServiceCANVar.BeforeCommands, gmresetServiceCANVar.AfterCommands, false);
						CS$<>8__locals1.result = null;
						obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0 && (int)data[0] == int.Parse(CS$<>8__locals1.value, NumberStyles.HexNumber))
							{
								CS$<>8__locals1.result = data;
								CS$<>8__locals1.requestResult = CodingRequestResult.Success;
							}
							else
							{
								CS$<>8__locals1.requestResult = CodingRequestResult.NotSupported;
							}
							CS$<>8__locals1.semaphore.Release();
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GMResetServiceCANVar1.<Execute>d__3>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.requestResult == CodingRequestResult.Success && CS$<>8__locals1.result != null && CS$<>8__locals1.result.Length != 0)
					{
						double num3 = (double)CS$<>8__locals1.result[0];
						gmresetServiceCANVar.CurrentState = (num3 * 100.0 / 255.0).ToString("0");
					}
					else
					{
						gmresetServiceCANVar.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(CS$<>8__locals1.requestResult);
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

			// Token: 0x060050DD RID: 20701 RVA: 0x003EC898 File Offset: 0x003EAA98
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030D2 RID: 12498
			public int <>1__state;

			// Token: 0x040030D3 RID: 12499
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040030D4 RID: 12500
			public string value;

			// Token: 0x040030D5 RID: 12501
			public GMResetServiceCANVar1 <>4__this;

			// Token: 0x040030D6 RID: 12502
			private GMResetServiceCANVar1.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x040030D7 RID: 12503
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020009B6 RID: 2486
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x060050DE RID: 20702 RVA: 0x003EC8A8 File Offset: 0x003EAAA8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				GMResetServiceCANVar1 gmresetServiceCANVar = this;
				CodingRequestResult requestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new GMResetServiceCANVar1.<>c__DisplayClass2_0();
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						gmresetServiceCANVar.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest("1A6D", gmresetServiceCANVar.RequestHeader, gmresetServiceCANVar.BeforeCommands, gmresetServiceCANVar.AfterCommands, false);
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GMResetServiceCANVar1.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.requestResult == CodingRequestResult.Success && CS$<>8__locals1.result != null && CS$<>8__locals1.result.Length != 0)
					{
						double num3 = (double)CS$<>8__locals1.result[0];
						gmresetServiceCANVar.CurrentState = (num3 * 100.0 / 255.0).ToString("0");
					}
					else
					{
						gmresetServiceCANVar.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(CS$<>8__locals1.requestResult);
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

			// Token: 0x060050DF RID: 20703 RVA: 0x003ECA94 File Offset: 0x003EAC94
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030D8 RID: 12504
			public int <>1__state;

			// Token: 0x040030D9 RID: 12505
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040030DA RID: 12506
			public GMResetServiceCANVar1 <>4__this;

			// Token: 0x040030DB RID: 12507
			private GMResetServiceCANVar1.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x040030DC RID: 12508
			private TaskAwaiter <>u__1;
		}
	}
}
