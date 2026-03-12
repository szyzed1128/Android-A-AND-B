using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BC1 RID: 3009
	internal class ResetEngineAdaptation : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005B07 RID: 23303 RVA: 0x00436880 File Offset: 0x00434A80
		public ResetEngineAdaptation()
		{
			base.Name = "Reset engine control unit adaptations";
			base.Description = "Compatibility confirmed with: Kia Picanto TA.";
			base.InnerDescription = "With high possibility this would be compatible with most of Hyundai/Kia cars from ~2009 to ~2015." + SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.Translations.Add(new TranslationItem("ru", "Сброс адаптаций блока управления двигателем", "Совместимость подтверждена с Kia Picanto TA", "С большой долей вероятности совместимо с большинством моделей Hyundai/Kia ~2009 .. ~2015 гг." + SupportedItemsDetectorBase.GetAccessKeyWarningRu));
			base.RequestHeader = "7E0";
			base.ResponseHeader = "7E8";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset adaptations (var.1)", "11FA", new TranslationItem[]
			{
				new TranslationItem("ru", "Сброс адаптаций (вар.1)", "", "")
			}));
			base.Options.Add(new MQBAdaptationOption("Reset adaptations (var.2)", "117A", new TranslationItem[]
			{
				new TranslationItem("ru", "Сброс адаптаций (вар.2)", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			base.Group = CodingGroup.EngineAndPowertrain;
			this.HasCurrentState = false;
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x004369AC File Offset: 0x00434BAC
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "ATSP6;ATSH7E0;ATCRA7E8";
			this.AfterCommands = "ATAR;ATSH7DF";
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x004369C4 File Offset: 0x00434BC4
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			return CodingRequestResult.Success;
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x00436A00 File Offset: 0x00434C00
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest(value, base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseMarker = "51";
			obdrequest.ResponseReceived += delegate(OBDRequest request2, string data)
			{
				if (data == null)
				{
					requestResult = CodingRequestResult.NoData;
				}
				if (data.Contains("NO DATA"))
				{
					requestResult = CodingRequestResult.NoData;
				}
				string text = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (text.Contains("7F1178") || text.Contains("51"))
				{
					requestResult = CodingRequestResult.Success;
				}
			};
			OBDRequest obdrequest2 = new OBDRequest("1003", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			OBDRequest req_openSession1090 = new OBDRequest("1090", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			obdrequest2.ResponseMarker = "50";
			req_openSession1090.ResponseMarker = "50";
			obdrequest2.ResponseReceived += delegate(OBDRequest req1, string data1)
			{
				if (!CAN11bitHelper.IsNoDataOrNegativeResponse(req1, data1))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.Remove(req_openSession1090);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, req_openSession1090, obdrequest });
			await App.OBDReader.WaitForCommandQueue();
			return requestResult;
		}

		// Token: 0x02000BC2 RID: 3010
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06005B0B RID: 23307 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005B0C RID: 23308 RVA: 0x00436A4C File Offset: 0x00434C4C
			internal void <Execute>b__0(OBDRequest request2, string data)
			{
				if (data == null)
				{
					this.requestResult = CodingRequestResult.NoData;
				}
				if (data.Contains("NO DATA"))
				{
					this.requestResult = CodingRequestResult.NoData;
				}
				string text = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (text.Contains("7F1178") || text.Contains("51"))
				{
					this.requestResult = CodingRequestResult.Success;
				}
			}

			// Token: 0x06005B0D RID: 23309 RVA: 0x00436AA4 File Offset: 0x00434CA4
			internal void <Execute>b__1(OBDRequest req1, string data1)
			{
				if (!CAN11bitHelper.IsNoDataOrNegativeResponse(req1, data1))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.Remove(this.req_openSession1090);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x04003961 RID: 14689
			public CodingRequestResult requestResult;

			// Token: 0x04003962 RID: 14690
			public OBDRequest req_openSession1090;
		}

		// Token: 0x02000BC3 RID: 3011
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005B0E RID: 23310 RVA: 0x00436AE0 File Offset: 0x00434CE0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ResetEngineAdaptation resetEngineAdaptation = this;
				CodingRequestResult requestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new ResetEngineAdaptation.<>c__DisplayClass3_0();
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						resetEngineAdaptation.BuildDefaultBeforeAndAfterCommands();
						new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest(value, resetEngineAdaptation.RequestHeader, resetEngineAdaptation.BeforeCommands, resetEngineAdaptation.AfterCommands, false);
						obdrequest.ResponseMarker = "51";
						obdrequest.ResponseReceived += delegate(OBDRequest request2, string data)
						{
							if (data == null)
							{
								CS$<>8__locals1.requestResult = CodingRequestResult.NoData;
							}
							if (data.Contains("NO DATA"))
							{
								CS$<>8__locals1.requestResult = CodingRequestResult.NoData;
							}
							string text = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (text.Contains("7F1178") || text.Contains("51"))
							{
								CS$<>8__locals1.requestResult = CodingRequestResult.Success;
							}
						};
						OBDRequest obdrequest2 = new OBDRequest("1003", resetEngineAdaptation.RequestHeader, resetEngineAdaptation.BeforeCommands, resetEngineAdaptation.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						CS$<>8__locals1.req_openSession1090 = new OBDRequest("1090", resetEngineAdaptation.RequestHeader, resetEngineAdaptation.BeforeCommands, resetEngineAdaptation.AfterCommands, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						obdrequest2.ResponseMarker = "50";
						CS$<>8__locals1.req_openSession1090.ResponseMarker = "50";
						obdrequest2.ResponseReceived += delegate(OBDRequest req1, string data1)
						{
							if (!CAN11bitHelper.IsNoDataOrNegativeResponse(req1, data1))
							{
								List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
								queueCopy.Remove(CS$<>8__locals1.req_openSession1090);
								App.OBDReader.ReplaceQueue(queueCopy);
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, CS$<>8__locals1.req_openSession1090, obdrequest });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ResetEngineAdaptation.<Execute>d__3>(ref taskAwaiter, ref this);
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

			// Token: 0x06005B0F RID: 23311 RVA: 0x00436CE0 File Offset: 0x00434EE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003963 RID: 14691
			public int <>1__state;

			// Token: 0x04003964 RID: 14692
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003965 RID: 14693
			public ResetEngineAdaptation <>4__this;

			// Token: 0x04003966 RID: 14694
			public string value;

			// Token: 0x04003967 RID: 14695
			private ResetEngineAdaptation.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04003968 RID: 14696
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000BC4 RID: 3012
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005B10 RID: 23312 RVA: 0x00436CF0 File Offset: 0x00434EF0
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

			// Token: 0x06005B11 RID: 23313 RVA: 0x00436D3C File Offset: 0x00434F3C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003969 RID: 14697
			public int <>1__state;

			// Token: 0x0400396A RID: 14698
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;
		}
	}
}
