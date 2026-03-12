using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x02000503 RID: 1283
	internal class OBD2Can11bitECU : CAN11bitECU
	{
		// Token: 0x06003178 RID: 12664 RVA: 0x00222950 File Offset: 0x00220B50
		public OBD2Can11bitECU()
		{
			this.Protocol = 0;
			this.Name = "OBD-II";
			base.ReadDTCCommands = new List<string> { "03", "07", "0A" };
			base.ClearDTCCommands = new List<string> { "04" };
			base.RemoveOtherRequestsIfUDSReadResponded = false;
			this.TestELMDevice = false;
			this.ECUExists = true;
		}

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x0600317A RID: 12666 RVA: 0x00222845 File Offset: 0x00220A45
		public override bool ECUExists
		{
			get
			{
				return true;
			}
			protected set
			{
				base.ECUExists = value;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x0600317B RID: 12667 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x0600317C RID: 12668 RVA: 0x0022283C File Offset: 0x00220A3C
		public override bool TestELMDevice
		{
			get
			{
				return false;
			}
			set
			{
				base.TestELMDevice = value;
			}
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x00218FB1 File Offset: 0x002171B1
		protected override OBDRequest[] GetTestECUExistsRequest()
		{
			return new OBDRequest[0];
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x002229CC File Offset: 0x00220BCC
		public override async Task<string> GetECUInformationReportAsync(CancellationToken token)
		{
			return await new OBD2InfoModelReportGenerator().GetReport();
		}

		// Token: 0x02000504 RID: 1284
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync>d__8 : IAsyncStateMachine
		{
			// Token: 0x0600317F RID: 12671 RVA: 0x00222A08 File Offset: 0x00220C08
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string result;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = new OBD2InfoModelReportGenerator().GetReport().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBD2Can11bitECU.<GetECUInformationReportAsync>d__8>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
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

			// Token: 0x06003180 RID: 12672 RVA: 0x00222ABC File Offset: 0x00220CBC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C94 RID: 7316
			public int <>1__state;

			// Token: 0x04001C95 RID: 7317
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001C96 RID: 7318
			private TaskAwaiter<string> <>u__1;
		}
	}
}
