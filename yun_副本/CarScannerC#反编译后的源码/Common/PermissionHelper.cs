using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007F7 RID: 2039
	internal static class PermissionHelper
	{
		// Token: 0x0600473C RID: 18236 RVA: 0x0036D6BE File Offset: 0x0036B8BE
		public static void OpenPermissionsSettings()
		{
			PlatformHelper.CommonService.OpenPermissionsSettings();
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0036D6CC File Offset: 0x0036B8CC
		public static async Task CheckLocationPermissionStatus(Action<PermissionStatus> callback)
		{
			PermissionStatus permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
			callback(permissionStatus);
		}

		// Token: 0x020007F8 RID: 2040
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckLocationPermissionStatus>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600473E RID: 18238 RVA: 0x0036D710 File Offset: 0x0036B910
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<PermissionStatus> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, PermissionHelper.<CheckLocationPermissionStatus>d__1>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
					}
					PermissionStatus result = taskAwaiter.GetResult();
					callback(result);
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600473F RID: 18239 RVA: 0x0036D7C8 File Offset: 0x0036B9C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400298B RID: 10635
			public int <>1__state;

			// Token: 0x0400298C RID: 10636
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400298D RID: 10637
			public Action<PermissionStatus> callback;

			// Token: 0x0400298E RID: 10638
			private TaskAwaiter<PermissionStatus> <>u__1;
		}
	}
}
