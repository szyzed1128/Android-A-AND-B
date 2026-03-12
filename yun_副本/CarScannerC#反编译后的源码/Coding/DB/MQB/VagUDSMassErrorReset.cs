using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B68 RID: 2920
	internal class VagUDSMassErrorReset : MQBServiceProcedure
	{
		// Token: 0x060059F3 RID: 23027 RVA: 0x0042EC4C File Offset: 0x0042CE4C
		public VagUDSMassErrorReset()
		{
			base.Name = Translate.GetString("codingDB_DtcClearInMostOfUnits_Name");
			base.Description = Translate.GetString("codingDB_DtcClearInMostOfUnits_Description");
			base.InnerDescription = Translate.GetString("codingDB_DtcClearInMostOfUnits_InnerDescription");
			this.Unit = "01";
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_ClearDtcInAllUnits_Name"), "14");
			base.Group = CodingGroup.ServiceProcedures;
			base.Options.Add(mqbadaptationOption);
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x0042ECC4 File Offset: 0x0042CEC4
		protected override async Task<CodingRequestResult> OptionExecute(string optionValue, IProgress<string> progress)
		{
			new OBDRequest("1083", "700", "", "", false);
			OBDRequest obdrequest = new OBDRequest("3E80", "700", "", "", false);
			OBDRequest obdrequest2 = new OBDRequest("04", "700", "", "", false);
			OBDRequest obdrequest3 = new OBDRequest("14FFFFFF", "700", "", "", false);
			App.OBDReader.ReplaceQueue(new OBDRequest[]
			{
				obdrequest2, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest,
				obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest3
			});
			await App.OBDReader.WaitForCommandQueue();
			return CodingRequestResult.Success;
		}

		// Token: 0x02000B69 RID: 2921
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__1 : IAsyncStateMachine
		{
			// Token: 0x060059F5 RID: 23029 RVA: 0x0042ED00 File Offset: 0x0042CF00
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						new OBDRequest("1083", "700", "", "", false);
						OBDRequest obdrequest = new OBDRequest("3E80", "700", "", "", false);
						OBDRequest obdrequest2 = new OBDRequest("04", "700", "", "", false);
						OBDRequest obdrequest3 = new OBDRequest("14FFFFFF", "700", "", "", false);
						App.OBDReader.ReplaceQueue(new OBDRequest[]
						{
							obdrequest2, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest,
							obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest3
						});
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagUDSMassErrorReset.<OptionExecute>d__1>(ref taskAwaiter, ref this);
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
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060059F6 RID: 23030 RVA: 0x0042EE94 File Offset: 0x0042D094
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400385C RID: 14428
			public int <>1__state;

			// Token: 0x0400385D RID: 14429
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400385E RID: 14430
			private TaskAwaiter <>u__1;
		}
	}
}
