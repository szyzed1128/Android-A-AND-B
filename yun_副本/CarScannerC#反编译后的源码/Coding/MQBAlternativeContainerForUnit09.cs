using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200089D RID: 2205
	internal class MQBAlternativeContainerForUnit09 : MQBAlternativeContainer
	{
		// Token: 0x06004B2A RID: 19242 RVA: 0x0038254C File Offset: 0x0038074C
		public MQBAlternativeContainerForUnit09(bool RequiresNewBCM, string Address, string Password, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned = false, bool ReversedByteSet = false)
			: base(Address, Password, StartByteId, DataLength, Multiplier, Offset, IsSigned, ReversedByteSet)
		{
			this.RequiresNewBCM = RequiresNewBCM;
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x00382574 File Offset: 0x00380774
		public MQBAlternativeContainerForUnit09(bool RequiresNewBCM, string Address, string Password, Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate, Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate)
			: base(Address, Password, ChangeDataDelegate, GetCurrentStateDelegate)
		{
			this.RequiresNewBCM = RequiresNewBCM;
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x00382589 File Offset: 0x00380789
		public MQBAlternativeContainerForUnit09(bool RequiresNewBCM, string Address, Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate, Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate)
			: this(RequiresNewBCM, Address, "", ChangeDataDelegate, GetCurrentStateDelegate)
		{
		}

		// Token: 0x170016F6 RID: 5878
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x0038259B File Offset: 0x0038079B
		// (set) Token: 0x06004B2E RID: 19246 RVA: 0x003825A3 File Offset: 0x003807A3
		public bool RequiresNewBCM
		{
			[CompilerGenerated]
			get
			{
				return this.<RequiresNewBCM>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequiresNewBCM>k__BackingField = value;
			}
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x003825AC File Offset: 0x003807AC
		public override async Task<bool> CheckIsSupported(string requestHeader, string responseHeader, MQBAlternativeCoding coding)
		{
			Tuple<byte[], CodingRequestResult> tuple = await new MQBLongCoding
			{
				Address = "0600",
				RequestHeader = "70E",
				ResponseHeader = "778"
			}.GetCurrentStateRawData("");
			bool flag2;
			if (tuple.Item2 == CodingRequestResult.Success && tuple.Item1 != null && tuple.Item1.Length != 0)
			{
				bool flag = tuple.Item1.Count((byte x) => x == 0) >= tuple.Item1.Length - 1;
				if (this.RequiresNewBCM)
				{
					flag2 = flag;
				}
				else
				{
					flag2 = !flag;
				}
			}
			else if (this.RequiresNewBCM)
			{
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x04002BDF RID: 11231
		[CompilerGenerated]
		private bool <RequiresNewBCM>k__BackingField;

		// Token: 0x0200089E RID: 2206
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004B30 RID: 19248 RVA: 0x003825EF File Offset: 0x003807EF
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004B31 RID: 19249 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004B32 RID: 19250 RVA: 0x0037EDD4 File Offset: 0x0037CFD4
			internal bool <CheckIsSupported>b__7_0(byte x)
			{
				return x == 0;
			}

			// Token: 0x04002BE0 RID: 11232
			public static readonly MQBAlternativeContainerForUnit09.<>c <>9 = new MQBAlternativeContainerForUnit09.<>c();

			// Token: 0x04002BE1 RID: 11233
			public static Func<byte, bool> <>9__7_0;
		}

		// Token: 0x0200089F RID: 2207
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsSupported>d__7 : IAsyncStateMachine
		{
			// Token: 0x06004B33 RID: 19251 RVA: 0x003825FC File Offset: 0x003807FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = this;
				bool flag2;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = new MQBLongCoding
						{
							Address = "0600",
							RequestHeader = "70E",
							ResponseHeader = "778"
						}.GetCurrentStateRawData("").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAlternativeContainerForUnit09.<CheckIsSupported>d__7>(ref taskAwaiter, ref this);
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
					if (result.Item2 == CodingRequestResult.Success && result.Item1 != null && result.Item1.Length != 0)
					{
						bool flag = result.Item1.Count((byte x) => x == 0) >= result.Item1.Length - 1;
						if (mqbalternativeContainerForUnit.RequiresNewBCM)
						{
							flag2 = flag;
						}
						else
						{
							flag2 = !flag;
						}
					}
					else if (mqbalternativeContainerForUnit.RequiresNewBCM)
					{
						flag2 = true;
					}
					else
					{
						flag2 = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag2);
			}

			// Token: 0x06004B34 RID: 19252 RVA: 0x00382768 File Offset: 0x00380968
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002BE2 RID: 11234
			public int <>1__state;

			// Token: 0x04002BE3 RID: 11235
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04002BE4 RID: 11236
			public MQBAlternativeContainerForUnit09 <>4__this;

			// Token: 0x04002BE5 RID: 11237
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
