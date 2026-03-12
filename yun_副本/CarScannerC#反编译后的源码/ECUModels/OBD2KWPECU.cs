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
	// Token: 0x02000505 RID: 1285
	internal class OBD2KWPECU : KWPECU
	{
		// Token: 0x06003181 RID: 12673 RVA: 0x00222ACC File Offset: 0x00220CCC
		public OBD2KWPECU()
		{
			base.RequestHeader = "";
			base.Protocol = 0;
			base.Name = "OBD-II";
			base.ReadDTCCommands = new List<string>
			{
				"03", "03", "07", "07", "0A", "1800FF00", "1802FF00", "1802FFFF", "1800FFFF", "18FF00",
				"17FF00", "13FF00", "1902AF", "1902AC", "19028D", "190223", "190278", "190208", "190FAC", "190F8D",
				"190F23", "19D2FF00"
			};
			base.ClearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFFFF", "140000" };
			base.RemoveOtherRequestsIfUDSReadResponded = false;
			base.TestELMDevice = false;
			this.ECUExists = true;
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x00218FB1 File Offset: 0x002171B1
		protected override OBDRequest[] GetTestECUExistsRequest()
		{
			return new OBDRequest[0];
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x00222C5C File Offset: 0x00220E5C
		public override async Task<string> GetECUInformationReportAsync(CancellationToken token)
		{
			return await new OBD2InfoModelReportGenerator().GetReport();
		}

		// Token: 0x02000506 RID: 1286
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync>d__2 : IAsyncStateMachine
		{
			// Token: 0x06003184 RID: 12676 RVA: 0x00222C98 File Offset: 0x00220E98
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBD2KWPECU.<GetECUInformationReportAsync>d__2>(ref taskAwaiter, ref this);
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

			// Token: 0x06003185 RID: 12677 RVA: 0x00222D4C File Offset: 0x00220F4C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C97 RID: 7319
			public int <>1__state;

			// Token: 0x04001C98 RID: 7320
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001C99 RID: 7321
			private TaskAwaiter<string> <>u__1;
		}
	}
}
