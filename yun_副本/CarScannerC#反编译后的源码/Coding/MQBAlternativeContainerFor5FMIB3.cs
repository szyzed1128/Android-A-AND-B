using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008A0 RID: 2208
	internal class MQBAlternativeContainerFor5FMIB3 : MQBAlternativeContainer
	{
		// Token: 0x06004B35 RID: 19253 RVA: 0x00382778 File Offset: 0x00380978
		public MQBAlternativeContainerFor5FMIB3(string Address, string Password, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned = false, bool ReversedByteSet = false)
			: base(Address, Password, StartByteId, DataLength, Multiplier, Offset, IsSigned, ReversedByteSet)
		{
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x00382798 File Offset: 0x00380998
		public MQBAlternativeContainerFor5FMIB3(string Address, string Password, Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate, Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate)
			: base(Address, Password, ChangeDataDelegate, GetCurrentStateDelegate)
		{
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x003827A5 File Offset: 0x003809A5
		public MQBAlternativeContainerFor5FMIB3(string Address, Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate, Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate)
			: this(Address, "", ChangeDataDelegate, GetCurrentStateDelegate)
		{
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x003827B8 File Offset: 0x003809B8
		public override async Task<bool> CheckIsSupported(string requestHeader, string responseHeader, MQBAlternativeCoding coding)
		{
			if (string.IsNullOrEmpty(coding.ECU) || string.IsNullOrEmpty(coding.Device) || string.IsNullOrEmpty(coding.ASAM))
			{
				await coding.RequestDeviceIdentsAsync();
			}
			bool flag;
			if (coding.IsMIB3())
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x020008A1 RID: 2209
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsSupported>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004B39 RID: 19257 RVA: 0x003827FC File Offset: 0x003809FC
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAlternativeContainerFor5FMIB3.<CheckIsSupported>d__3>(ref taskAwaiter, ref this);
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
						flag = true;
					}
					else
					{
						flag = false;
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

			// Token: 0x06004B3A RID: 19258 RVA: 0x003828F8 File Offset: 0x00380AF8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002BE6 RID: 11238
			public int <>1__state;

			// Token: 0x04002BE7 RID: 11239
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04002BE8 RID: 11240
			public MQBAlternativeCoding coding;

			// Token: 0x04002BE9 RID: 11241
			private TaskAwaiter <>u__1;
		}
	}
}
