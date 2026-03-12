using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000649 RID: 1609
	internal static class PIDSelector
	{
		// Token: 0x060037CC RID: 14284 RVA: 0x002A05A4 File Offset: 0x0029E7A4
		public static async Task<IPID> SelectPIDAsync(IPID preSelectedPid = null, Func<PID, bool> additionalFilter = null)
		{
			IPID ipid;
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected || !SharedSettings.Current.PIDSelectorWithValuePreview)
			{
				ipid = await PIDSelectorV2.SelectPIDAsync(preSelectedPid, additionalFilter);
			}
			else
			{
				ipid = await PIDSelectorV3.SelectPIDAsync(preSelectedPid, additionalFilter);
			}
			return ipid;
		}

		// Token: 0x0200064A RID: 1610
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectPIDAsync>d__0 : IAsyncStateMachine
		{
			// Token: 0x060037CD RID: 14285 RVA: 0x002A05F0 File Offset: 0x0029E7F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				IPID ipid;
				try
				{
					TaskAwaiter<IPID> taskAwaiter;
					TaskAwaiter<IPID> taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected || !SharedSettings.Current.PIDSelectorWithValuePreview)
							{
								taskAwaiter = PIDSelectorV2.SelectPIDAsync(preSelectedPid, additionalFilter).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IPID>, PIDSelector.<SelectPIDAsync>d__0>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0088;
							}
							else
							{
								taskAwaiter = PIDSelectorV3.SelectPIDAsync(preSelectedPid, additionalFilter).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IPID>, PIDSelector.<SelectPIDAsync>d__0>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<IPID>);
							num2 = -1;
						}
						ipid = taskAwaiter.GetResult();
						goto IL_010F;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<IPID>);
					num2 = -1;
					IL_0088:
					ipid = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_010F:
				num2 = -2;
				this.<>t__builder.SetResult(ipid);
			}

			// Token: 0x060037CE RID: 14286 RVA: 0x002A0730 File Offset: 0x0029E930
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040021C8 RID: 8648
			public int <>1__state;

			// Token: 0x040021C9 RID: 8649
			public AsyncTaskMethodBuilder<IPID> <>t__builder;

			// Token: 0x040021CA RID: 8650
			public IPID preSelectedPid;

			// Token: 0x040021CB RID: 8651
			public Func<PID, bool> additionalFilter;

			// Token: 0x040021CC RID: 8652
			private TaskAwaiter<IPID> <>u__1;
		}
	}
}
