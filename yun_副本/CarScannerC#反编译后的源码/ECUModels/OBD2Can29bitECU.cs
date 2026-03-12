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
	// Token: 0x02000501 RID: 1281
	internal class OBD2Can29bitECU : CAN29bitECU
	{
		// Token: 0x0600316F RID: 12655 RVA: 0x00222764 File Offset: 0x00220964
		public OBD2Can29bitECU()
			: base("OBD-II", "", "")
		{
			this.Protocol = 7;
			base.ReadDTCCommands = new List<string> { "03", "07", "0A" };
			base.ClearDTCCommands = new List<string> { "04" };
			base.RemoveOtherRequestsIfUDSReadResponded = false;
			this.TestELMDevice = false;
			base.IdentsASCII.TryAdd("090A", "ECU name");
			base.IdentsASCII.TryAdd("0902", "VIN");
			base.IdentsASCII.TryAdd("0904", "Calibration ID");
			base.IdentsASCII.TryAdd("0906", "Calibration Verification Numbers (CVN)");
			this.ECUExists = true;
		}

		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06003171 RID: 12657 RVA: 0x0022283C File Offset: 0x00220A3C
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

		// Token: 0x06003172 RID: 12658 RVA: 0x00218FB1 File Offset: 0x002171B1
		protected override OBDRequest[] GetTestECUExistsRequest()
		{
			return new OBDRequest[0];
		}

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x06003174 RID: 12660 RVA: 0x00222845 File Offset: 0x00220A45
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

		// Token: 0x06003175 RID: 12661 RVA: 0x00222850 File Offset: 0x00220A50
		public override async Task<string> GetECUInformationReportAsync(CancellationToken token)
		{
			return await new OBD2InfoModelReportGenerator().GetReport();
		}

		// Token: 0x02000502 RID: 1282
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync>d__8 : IAsyncStateMachine
		{
			// Token: 0x06003176 RID: 12662 RVA: 0x0022288C File Offset: 0x00220A8C
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBD2Can29bitECU.<GetECUInformationReportAsync>d__8>(ref taskAwaiter, ref this);
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

			// Token: 0x06003177 RID: 12663 RVA: 0x00222940 File Offset: 0x00220B40
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001C91 RID: 7313
			public int <>1__state;

			// Token: 0x04001C92 RID: 7314
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001C93 RID: 7315
			private TaskAwaiter<string> <>u__1;
		}
	}
}
