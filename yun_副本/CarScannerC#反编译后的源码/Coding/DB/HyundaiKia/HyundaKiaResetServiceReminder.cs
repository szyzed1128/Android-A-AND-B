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
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BB2 RID: 2994
	internal class HyundaKiaResetServiceReminder : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005AD8 RID: 23256 RVA: 0x00435A90 File Offset: 0x00433C90
		public HyundaKiaResetServiceReminder()
		{
			base.Name = "Reset service reminder";
			base.Description = "";
			base.InnerDescription = SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.Translations.Add(new TranslationItem("ru", "Сброс уведомления о сервисе", SupportedItemsDetectorBase.GetAccessKeyWarningRu, ""));
			base.RequestHeader = "7C6";
			base.ResponseHeader = "7CE";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset", "2E0071", new TranslationItem[]
			{
				new TranslationItem("ru", "Сброс", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			base.Group = CodingGroup.ServiceProcedures;
			this.HasCurrentState = false;
		}

		// Token: 0x06005AD9 RID: 23257 RVA: 0x00435B6D File Offset: 0x00433D6D
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "ATSP6;ATSH7C6;ATCRA7CE;20;1003;";
			this.AfterCommands = "ATAR;ATSH7DF";
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x00435B88 File Offset: 0x00433D88
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			return CodingRequestResult.Success;
		}

		// Token: 0x06005ADB RID: 23259 RVA: 0x00435BC4 File Offset: 0x00433DC4
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult requestResult = CodingRequestResult.Success;
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest("2E0071", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest[] accessKeysRequests = this.GetAccessKeysRequests(password, progress, delegate
			{
				if (!SharedSettings.Current.IgnoreCodingErrors)
				{
					requestResult = CodingRequestResult.WrongAccessKey;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
				}
			});
			List<OBDRequest> list = new List<OBDRequest>();
			list.AddRange(accessKeysRequests);
			list.Add(obdrequest);
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			return requestResult;
		}

		// Token: 0x06005ADC RID: 23260 RVA: 0x00435C18 File Offset: 0x00433E18
		protected OBDRequest[] GetAccessKeysRequests(string password, IProgress<string> progress, Action wrongPasswordCallback)
		{
			bool zero_seed = false;
			OBDRequest req_sendKey = new OBDRequest("2702", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest = new OBDRequest("2701", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(getSeedData);
					}
					int num = BitConverter.ToInt32(getSeedData, 0);
					if (num == 0)
					{
						zero_seed = true;
					}
					if (!zero_seed)
					{
						string text = (12 - num).ToString("X8");
						req_sendKey.Command = "2702" + text;
						IProgress<string> progress2 = progress;
						if (progress2 != null)
						{
							progress2.Report(Translate.GetString("coding_progress_SendingPassword"));
						}
					}
					else
					{
						req_sendKey.Command = "3E";
					}
				}
				catch (Exception)
				{
					App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
					req_sendKey.Command = "3E";
				}
			};
			req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (request.Command != "3E")
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if ((data != null && data.Contains("6702")) || SharedSettings.Current.IgnoreCodingErrors)
					{
						IProgress<string> progress3 = progress;
						if (progress3 == null)
						{
							return;
						}
						progress3.Report(Translate.GetString("coding_progress_SuccessPassword"));
						return;
					}
					else
					{
						Action wrongPasswordCallback2 = wrongPasswordCallback;
						if (wrongPasswordCallback2 == null)
						{
							return;
						}
						wrongPasswordCallback2();
					}
				}
			};
			return new OBDRequest[] { obdrequest, req_sendKey };
		}

		// Token: 0x02000BB3 RID: 2995
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06005ADD RID: 23261 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005ADE RID: 23262 RVA: 0x00435CBD File Offset: 0x00433EBD
			internal void <Execute>b__0()
			{
				if (!SharedSettings.Current.IgnoreCodingErrors)
				{
					this.requestResult = CodingRequestResult.WrongAccessKey;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
				}
			}

			// Token: 0x04003943 RID: 14659
			public CodingRequestResult requestResult;

			// Token: 0x04003944 RID: 14660
			public SemaphoreSlim semaphore;
		}

		// Token: 0x02000BB4 RID: 2996
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06005ADF RID: 23263 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06005AE0 RID: 23264 RVA: 0x00435CF0 File Offset: 0x00433EF0
			internal void <GetAccessKeysRequests>b__0(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(getSeedData);
					}
					int num = BitConverter.ToInt32(getSeedData, 0);
					if (num == 0)
					{
						this.zero_seed = true;
					}
					if (!this.zero_seed)
					{
						string text = (12 - num).ToString("X8");
						this.req_sendKey.Command = "2702" + text;
						IProgress<string> progress = this.progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_SendingPassword"));
						}
					}
					else
					{
						this.req_sendKey.Command = "3E";
					}
				}
				catch (Exception)
				{
					App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
					this.req_sendKey.Command = "3E";
				}
			}

			// Token: 0x06005AE1 RID: 23265 RVA: 0x00435DB0 File Offset: 0x00433FB0
			internal void <GetAccessKeysRequests>b__1(OBDRequest request, string data)
			{
				if (request.Command != "3E")
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if ((data != null && data.Contains("6702")) || SharedSettings.Current.IgnoreCodingErrors)
					{
						IProgress<string> progress = this.progress;
						if (progress == null)
						{
							return;
						}
						progress.Report(Translate.GetString("coding_progress_SuccessPassword"));
						return;
					}
					else
					{
						Action action = this.wrongPasswordCallback;
						if (action == null)
						{
							return;
						}
						action();
					}
				}
			}

			// Token: 0x04003945 RID: 14661
			public bool zero_seed;

			// Token: 0x04003946 RID: 14662
			public OBDRequest req_sendKey;

			// Token: 0x04003947 RID: 14663
			public IProgress<string> progress;

			// Token: 0x04003948 RID: 14664
			public Action wrongPasswordCallback;
		}

		// Token: 0x02000BB5 RID: 2997
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005AE2 RID: 23266 RVA: 0x00435E24 File Offset: 0x00434024
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				HyundaKiaResetServiceReminder hyundaKiaResetServiceReminder = this;
				CodingRequestResult requestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new HyundaKiaResetServiceReminder.<>c__DisplayClass3_0();
						CS$<>8__locals1.requestResult = CodingRequestResult.Success;
						hyundaKiaResetServiceReminder.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest("2E0071", hyundaKiaResetServiceReminder.RequestHeader, hyundaKiaResetServiceReminder.BeforeCommands, hyundaKiaResetServiceReminder.AfterCommands, false);
						OBDRequest[] accessKeysRequests = hyundaKiaResetServiceReminder.GetAccessKeysRequests(password, progress, delegate
						{
							if (!SharedSettings.Current.IgnoreCodingErrors)
							{
								CS$<>8__locals1.requestResult = CodingRequestResult.WrongAccessKey;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
							}
						});
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(accessKeysRequests);
						list.Add(obdrequest);
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HyundaKiaResetServiceReminder.<Execute>d__3>(ref taskAwaiter, ref this);
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

			// Token: 0x06005AE3 RID: 23267 RVA: 0x00435FA0 File Offset: 0x004341A0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003949 RID: 14665
			public int <>1__state;

			// Token: 0x0400394A RID: 14666
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400394B RID: 14667
			public HyundaKiaResetServiceReminder <>4__this;

			// Token: 0x0400394C RID: 14668
			public string password;

			// Token: 0x0400394D RID: 14669
			public IProgress<string> progress;

			// Token: 0x0400394E RID: 14670
			private HyundaKiaResetServiceReminder.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x0400394F RID: 14671
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000BB6 RID: 2998
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005AE4 RID: 23268 RVA: 0x00435FB0 File Offset: 0x004341B0
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

			// Token: 0x06005AE5 RID: 23269 RVA: 0x00435FFC File Offset: 0x004341FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003950 RID: 14672
			public int <>1__state;

			// Token: 0x04003951 RID: 14673
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;
		}
	}
}
