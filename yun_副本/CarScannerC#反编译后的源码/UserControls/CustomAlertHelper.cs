using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Forms;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005C8 RID: 1480
	public static class CustomAlertHelper
	{
		// Token: 0x0600354B RID: 13643 RVA: 0x00263920 File Offset: 0x00261B20
		public static async Task<string> DisplayActionSheetCustom(this Page page, string title, string cancel, string destruction, params string[] buttons)
		{
			string text;
			if (page == null)
			{
				text = cancel;
			}
			else if (PlatformHelper.IsiOS && PlatformHelper.IsPlatformVersionNewerOrEqual(14, 0) && PlatformHelper.IOSService.IsiOSApplicationOnMac)
			{
				text = await CustomAlertPage.DisplayActionSheetCustom(page, title, cancel, destruction, buttons);
			}
			else
			{
				text = await page.DisplayActionSheet(title, cancel, destruction, buttons);
			}
			return text;
		}

		// Token: 0x020005C9 RID: 1481
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DisplayActionSheetCustom>d__0 : IAsyncStateMachine
		{
			// Token: 0x0600354C RID: 13644 RVA: 0x00263984 File Offset: 0x00261B84
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string text;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter<string> taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (page == null)
							{
								text = cancel;
								goto IL_0158;
							}
							if (PlatformHelper.IsiOS && PlatformHelper.IsPlatformVersionNewerOrEqual(14, 0) && PlatformHelper.IOSService.IsiOSApplicationOnMac)
							{
								taskAwaiter = CustomAlertPage.DisplayActionSheetCustom(page, title, cancel, destruction, buttons).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, CustomAlertHelper.<DisplayActionSheetCustom>d__0>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00BC;
							}
							else
							{
								taskAwaiter = page.DisplayActionSheet(title, cancel, destruction, buttons).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, CustomAlertHelper.<DisplayActionSheetCustom>d__0>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
						}
						text = taskAwaiter.GetResult();
						goto IL_0158;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<string>);
					num2 = -1;
					IL_00BC:
					text = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0158:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x0600354D RID: 13645 RVA: 0x00263B1C File Offset: 0x00261D1C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FB4 RID: 8116
			public int <>1__state;

			// Token: 0x04001FB5 RID: 8117
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001FB6 RID: 8118
			public Page page;

			// Token: 0x04001FB7 RID: 8119
			public string cancel;

			// Token: 0x04001FB8 RID: 8120
			public string title;

			// Token: 0x04001FB9 RID: 8121
			public string destruction;

			// Token: 0x04001FBA RID: 8122
			public string[] buttons;

			// Token: 0x04001FBB RID: 8123
			private TaskAwaiter<string> <>u__1;
		}
	}
}
