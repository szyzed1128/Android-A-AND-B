using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008A2 RID: 2210
	internal class MQBAlternativeContainerFor5FMIB25 : MQBAlternativeContainer
	{
		// Token: 0x06004B3B RID: 19259 RVA: 0x00382908 File Offset: 0x00380B08
		public MQBAlternativeContainerFor5FMIB25(string Address, string Password, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned = false, bool ReversedByteSet = false)
			: base(Address, Password, StartByteId, DataLength, Multiplier, Offset, IsSigned, ReversedByteSet)
		{
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x00382798 File Offset: 0x00380998
		public MQBAlternativeContainerFor5FMIB25(string Address, string Password, Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate, Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate)
			: base(Address, Password, ChangeDataDelegate, GetCurrentStateDelegate)
		{
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x00382928 File Offset: 0x00380B28
		public MQBAlternativeContainerFor5FMIB25(string Address, Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate, Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate)
			: this(Address, "", ChangeDataDelegate, GetCurrentStateDelegate)
		{
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x00382938 File Offset: 0x00380B38
		public override async Task<bool> CheckIsSupported(string requestHeader, string responseHeader, MQBAlternativeCoding coding)
		{
			if (string.IsNullOrEmpty(coding.ECU) || string.IsNullOrEmpty(coding.Device) || string.IsNullOrEmpty(coding.ASAM))
			{
				await coding.RequestDeviceIdentsAsync();
			}
			bool flag;
			if (coding.IsMIB3())
			{
				flag = false;
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x020008A3 RID: 2211
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsSupported>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004B3F RID: 19263 RVA: 0x0038297C File Offset: 0x00380B7C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!string.IsNullOrEmpty(coding.ECU) && !string.IsNullOrEmpty(coding.Device) && !string.IsNullOrEmpty(coding.ASAM))
						{
							goto IL_009D;
						}
						taskAwaiter = coding.RequestDeviceIdentsAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAlternativeContainerFor5FMIB25.<CheckIsSupported>d__3>(ref taskAwaiter, ref this);
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
					IL_009D:
					if (coding.IsMIB3())
					{
						flag = false;
					}
					else
					{
						flag = true;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06004B40 RID: 19264 RVA: 0x00382A78 File Offset: 0x00380C78
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002BEA RID: 11242
			public int <>1__state;

			// Token: 0x04002BEB RID: 11243
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04002BEC RID: 11244
			public MQBAlternativeCoding coding;

			// Token: 0x04002BED RID: 11245
			private TaskAwaiter <>u__1;
		}
	}
}
