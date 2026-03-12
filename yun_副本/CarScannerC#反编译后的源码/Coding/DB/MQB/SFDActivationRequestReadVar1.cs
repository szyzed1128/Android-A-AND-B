using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B4B RID: 2891
	internal class SFDActivationRequestReadVar1 : CustomizableCodingTemplate
	{
		// Token: 0x06005993 RID: 22931 RVA: 0x0042B758 File Offset: 0x00429958
		public SFDActivationRequestReadVar1(string title, string requestHeader, string responseHeader, string protocol)
		{
			this.Group = CodingGroup.SFD;
			base.Name = title;
			base.PreReadCommands = "22F199;22F198;22F1A5;2EF199;2EF198";
			base.Protocol = protocol;
			base.OpenSessionCommand = "1003";
			base.RequestHeader = requestHeader;
			base.ResponseHeader = responseHeader;
			this.ReadModeAndAddress = "3101C00803";
			base.WriteModeAndAddress = "";
			this.ValueType = AdaptationValueTypes.InputTextType;
			base.InnerDescription = "Tap on \"Apply\" button to copy result to clipboard. No data would be written to the ECU";
			base.MakeChangesToInitialData = false;
		}

		// Token: 0x06005994 RID: 22932 RVA: 0x0042B7D8 File Offset: 0x004299D8
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(this.ReadModeAndAddress))
			{
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
				CodingRequestResult item = tuple.Item2;
				byte[] item2 = tuple.Item1;
				if (item != CodingRequestResult.Success)
				{
					base.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
					codingRequestResult = item;
				}
				else
				{
					string text = BitHelpers.ByteArrayToHexString(item2);
					base.CurrentState = text;
					codingRequestResult = item;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x0042B824 File Offset: 0x00429A24
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			try
			{
				await Clipboard.SetTextAsync(base.CurrentState);
				codingRequestResult = CodingRequestResult.Success;
			}
			catch (Exception)
			{
				codingRequestResult = CodingRequestResult.UnknownError;
			}
			return codingRequestResult;
		}

		// Token: 0x02000B4C RID: 2892
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005996 RID: 22934 RVA: 0x0042B868 File Offset: 0x00429A68
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SFDActivationRequestReadVar1 sfdactivationRequestReadVar = this;
				CodingRequestResult codingRequestResult;
				try
				{
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = Clipboard.SetTextAsync(sfdactivationRequestReadVar.CurrentState).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SFDActivationRequestReadVar1.<Execute>d__2>(ref taskAwaiter, ref this);
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
						codingRequestResult = CodingRequestResult.Success;
					}
					catch (Exception)
					{
						codingRequestResult = CodingRequestResult.UnknownError;
					}
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

			// Token: 0x06005997 RID: 22935 RVA: 0x0042B938 File Offset: 0x00429B38
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040037F2 RID: 14322
			public int <>1__state;

			// Token: 0x040037F3 RID: 14323
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040037F4 RID: 14324
			public SFDActivationRequestReadVar1 <>4__this;

			// Token: 0x040037F5 RID: 14325
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000B4D RID: 2893
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005998 RID: 22936 RVA: 0x0042B948 File Offset: 0x00429B48
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SFDActivationRequestReadVar1 sfdactivationRequestReadVar = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(sfdactivationRequestReadVar.ReadModeAndAddress))
						{
							codingRequestResult = CodingRequestResult.Success;
							goto IL_00E1;
						}
						taskAwaiter = sfdactivationRequestReadVar.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, SFDActivationRequestReadVar1.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
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
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						sfdactivationRequestReadVar.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						string text = BitHelpers.ByteArrayToHexString(item2);
						sfdactivationRequestReadVar.CurrentState = text;
						codingRequestResult = item;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00E1:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005999 RID: 22937 RVA: 0x0042BA5C File Offset: 0x00429C5C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040037F6 RID: 14326
			public int <>1__state;

			// Token: 0x040037F7 RID: 14327
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040037F8 RID: 14328
			public SFDActivationRequestReadVar1 <>4__this;

			// Token: 0x040037F9 RID: 14329
			public string password;

			// Token: 0x040037FA RID: 14330
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
