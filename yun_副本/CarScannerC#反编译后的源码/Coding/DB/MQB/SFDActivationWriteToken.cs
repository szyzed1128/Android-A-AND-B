using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B4F RID: 2895
	internal class SFDActivationWriteToken : CustomizableCodingTemplate
	{
		// Token: 0x0600599B RID: 22939 RVA: 0x0042BA84 File Offset: 0x00429C84
		public SFDActivationWriteToken(string title, string requestHeader, string responseHeader, string protocol)
		{
			this.Group = CodingGroup.SFD;
			base.Name = title;
			base.PreWriteCommands = "22F199;22F198;22F1A5;2EF199;2EF198";
			base.Protocol = protocol;
			base.OpenSessionCommand = "1003";
			base.RequestHeader = requestHeader;
			base.ResponseHeader = responseHeader;
			this.ReadModeAndAddress = "220174";
			base.WriteModeAndAddress = "3101C004";
			this.ValueType = AdaptationValueTypes.InputTextType;
			base.MakeChangesToInitialData = false;
		}

		// Token: 0x0600599C RID: 22940 RVA: 0x0042BAF8 File Offset: 0x00429CF8
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

		// Token: 0x0600599D RID: 22941 RVA: 0x0042BB44 File Offset: 0x00429D44
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			return await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, value);
		}

		// Token: 0x02000B50 RID: 2896
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x0600599E RID: 22942 RVA: 0x0042BBB4 File Offset: 0x00429DB4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SFDActivationWriteToken sfdactivationWriteToken = this;
				CodingRequestResult result;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						string text = value;
						taskAwaiter = sfdactivationWriteToken.WriteDataToECU(password, UserFriendlyValue, progress, originalData, text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, SFDActivationWriteToken.<Execute>d__2>(ref taskAwaiter, ref this);
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

			// Token: 0x0600599F RID: 22943 RVA: 0x0042BC90 File Offset: 0x00429E90
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040037FB RID: 14331
			public int <>1__state;

			// Token: 0x040037FC RID: 14332
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040037FD RID: 14333
			public string value;

			// Token: 0x040037FE RID: 14334
			public SFDActivationWriteToken <>4__this;

			// Token: 0x040037FF RID: 14335
			public string password;

			// Token: 0x04003800 RID: 14336
			public string UserFriendlyValue;

			// Token: 0x04003801 RID: 14337
			public IProgress<string> progress;

			// Token: 0x04003802 RID: 14338
			public byte[] originalData;

			// Token: 0x04003803 RID: 14339
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000B51 RID: 2897
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x060059A0 RID: 22944 RVA: 0x0042BCA0 File Offset: 0x00429EA0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SFDActivationWriteToken sfdactivationWriteToken = this;
				CodingRequestResult codingRequestResult2;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = sfdactivationWriteToken.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, SFDActivationWriteToken.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
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
						sfdactivationWriteToken.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(codingRequestResult);
						codingRequestResult2 = codingRequestResult;
					}
					else if (item == null || item.Length < 4)
					{
						codingRequestResult = CodingRequestResult.UnknownError;
						sfdactivationWriteToken.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(codingRequestResult);
						codingRequestResult2 = codingRequestResult;
					}
					else
					{
						if (item[3] == 0)
						{
							sfdactivationWriteToken.CurrentState = "No activation active";
						}
						else
						{
							sfdactivationWriteToken.CurrentState = string.Format("Activation time left: {0} min", item[3]);
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

			// Token: 0x060059A1 RID: 22945 RVA: 0x0042BDD0 File Offset: 0x00429FD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003804 RID: 14340
			public int <>1__state;

			// Token: 0x04003805 RID: 14341
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003806 RID: 14342
			public SFDActivationWriteToken <>4__this;

			// Token: 0x04003807 RID: 14343
			public string password;

			// Token: 0x04003808 RID: 14344
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
