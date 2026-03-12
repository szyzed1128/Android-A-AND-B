using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B52 RID: 2898
	internal class SFDLockUnit : SFDActivationWriteToken
	{
		// Token: 0x060059A2 RID: 22946 RVA: 0x0042BDE0 File Offset: 0x00429FE0
		public SFDLockUnit(string title, string requestHeader, string responseHeader, string protocol)
			: base(title, requestHeader, responseHeader, protocol)
		{
			this.ValueType = AdaptationValueTypes.OptionType;
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption("Lock", "3101C005");
			this.Options.Add(mqbadaptationOption);
		}

		// Token: 0x060059A3 RID: 22947 RVA: 0x0042BE1C File Offset: 0x0042A01C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
			CodingRequestResult codingRequestResult = tuple.Item2;
			byte[] item = tuple.Item1;
			CodingRequestResult codingRequestResult2;
			if (codingRequestResult != CodingRequestResult.Success)
			{
				base.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(codingRequestResult);
				codingRequestResult2 = codingRequestResult;
			}
			else if (item == null || item.Length < 4)
			{
				codingRequestResult = CodingRequestResult.UnknownError;
				base.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(codingRequestResult);
				codingRequestResult2 = codingRequestResult;
			}
			else
			{
				if (item[3] == 0)
				{
					base.CurrentState = "No activation active";
				}
				else
				{
					base.CurrentState = string.Format("Activation time left: {0} min", item[3]);
				}
				codingRequestResult2 = CodingRequestResult.Success;
			}
			return codingRequestResult2;
		}

		// Token: 0x060059A4 RID: 22948 RVA: 0x0042BE68 File Offset: 0x0042A068
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			return await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, value);
		}

		// Token: 0x02000B53 RID: 2899
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x060059A5 RID: 22949 RVA: 0x0042BED8 File Offset: 0x0042A0D8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SFDLockUnit sfdlockUnit = this;
				CodingRequestResult result;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						string text = value;
						taskAwaiter = sfdlockUnit.WriteDataToECU(password, UserFriendlyValue, progress, originalData, text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, SFDLockUnit.<Execute>d__2>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					result = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x060059A6 RID: 22950 RVA: 0x0042BFB4 File Offset: 0x0042A1B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003809 RID: 14345
			public int <>1__state;

			// Token: 0x0400380A RID: 14346
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400380B RID: 14347
			public string value;

			// Token: 0x0400380C RID: 14348
			public SFDLockUnit <>4__this;

			// Token: 0x0400380D RID: 14349
			public string password;

			// Token: 0x0400380E RID: 14350
			public string UserFriendlyValue;

			// Token: 0x0400380F RID: 14351
			public IProgress<string> progress;

			// Token: 0x04003810 RID: 14352
			public byte[] originalData;

			// Token: 0x04003811 RID: 14353
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000B54 RID: 2900
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x060059A7 RID: 22951 RVA: 0x0042BFC4 File Offset: 0x0042A1C4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SFDLockUnit sfdlockUnit = this;
				CodingRequestResult codingRequestResult2;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = sfdlockUnit.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, SFDLockUnit.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult codingRequestResult = result.Item2;
					byte[] item = result.Item1;
					if (codingRequestResult != CodingRequestResult.Success)
					{
						sfdlockUnit.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(codingRequestResult);
						codingRequestResult2 = codingRequestResult;
					}
					else if (item == null || item.Length < 4)
					{
						codingRequestResult = CodingRequestResult.UnknownError;
						sfdlockUnit.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(codingRequestResult);
						codingRequestResult2 = codingRequestResult;
					}
					else
					{
						if (item[3] == 0)
						{
							sfdlockUnit.CurrentState = "No activation active";
						}
						else
						{
							sfdlockUnit.CurrentState = string.Format("Activation time left: {0} min", item[3]);
						}
						codingRequestResult2 = CodingRequestResult.Success;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult2);
			}

			// Token: 0x060059A8 RID: 22952 RVA: 0x0042C0F4 File Offset: 0x0042A2F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003812 RID: 14354
			public int <>1__state;

			// Token: 0x04003813 RID: 14355
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003814 RID: 14356
			public SFDLockUnit <>4__this;

			// Token: 0x04003815 RID: 14357
			public string password;

			// Token: 0x04003816 RID: 14358
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
