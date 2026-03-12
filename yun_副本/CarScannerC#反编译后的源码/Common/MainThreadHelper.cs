using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007F0 RID: 2032
	public static class MainThreadHelper
	{
		// Token: 0x0600472D RID: 18221 RVA: 0x0036D25C File Offset: 0x0036B45C
		public static async Task OnMainThreadAsync(Func<Task> action)
		{
			MainThreadHelper.<>c__DisplayClass0_0 CS$<>8__locals1 = new MainThreadHelper.<>c__DisplayClass0_0();
			CS$<>8__locals1.action = action;
			CS$<>8__locals1.tcs = new TaskCompletionSource<object>();
			Device.BeginInvokeOnMainThread(delegate
			{
				MainThreadHelper.<>c__DisplayClass0_0.<<OnMainThreadAsync>b__0>d <<OnMainThreadAsync>b__0>d;
				<<OnMainThreadAsync>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<OnMainThreadAsync>b__0>d.<>4__this = CS$<>8__locals1;
				<<OnMainThreadAsync>b__0>d.<>1__state = -1;
				<<OnMainThreadAsync>b__0>d.<>t__builder.Start<MainThreadHelper.<>c__DisplayClass0_0.<<OnMainThreadAsync>b__0>d>(ref <<OnMainThreadAsync>b__0>d);
			});
			await CS$<>8__locals1.tcs.Task;
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x0036D29F File Offset: 0x0036B49F
		public static void InvokeOnMainThread(Action a)
		{
			if (MainThread.IsMainThread)
			{
				a();
				return;
			}
			MainThread.InvokeOnMainThreadAsync(a).Wait();
		}

		// Token: 0x020007F1 RID: 2033
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x0600472F RID: 18223 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06004730 RID: 18224 RVA: 0x0036D2BC File Offset: 0x0036B4BC
			internal async void <OnMainThreadAsync>b__0()
			{
				try
				{
					await this.action();
					this.tcs.SetResult(new { });
				}
				catch (Exception ex)
				{
					this.tcs.SetException(ex);
				}
			}

			// Token: 0x0400297B RID: 10619
			public Func<Task> action;

			// Token: 0x0400297C RID: 10620
			public TaskCompletionSource<object> tcs;

			// Token: 0x020007F2 RID: 2034
			[StructLayout(LayoutKind.Auto)]
			private struct <<OnMainThreadAsync>b__0>d : IAsyncStateMachine
			{
				// Token: 0x06004731 RID: 18225 RVA: 0x0036D2F4 File Offset: 0x0036B4F4
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					MainThreadHelper.<>c__DisplayClass0_0 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 0)
							{
								taskAwaiter = CS$<>8__locals1.action().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MainThreadHelper.<>c__DisplayClass0_0.<<OnMainThreadAsync>b__0>d>(ref taskAwaiter, ref this);
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
							CS$<>8__locals1.tcs.SetResult(new { });
						}
						catch (Exception ex)
						{
							CS$<>8__locals1.tcs.SetException(ex);
						}
					}
					catch (Exception ex2)
					{
						num2 = -2;
						this.<>t__builder.SetException(ex2);
						return;
					}
					num2 = -2;
					this.<>t__builder.SetResult();
				}

				// Token: 0x06004732 RID: 18226 RVA: 0x0036D3E0 File Offset: 0x0036B5E0
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400297D RID: 10621
				public int <>1__state;

				// Token: 0x0400297E RID: 10622
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x0400297F RID: 10623
				public MainThreadHelper.<>c__DisplayClass0_0 <>4__this;

				// Token: 0x04002980 RID: 10624
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x020007F3 RID: 2035
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnMainThreadAsync>d__0 : IAsyncStateMachine
		{
			// Token: 0x06004733 RID: 18227 RVA: 0x0036D3F0 File Offset: 0x0036B5F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<object> taskAwaiter;
					if (num != 0)
					{
						MainThreadHelper.<>c__DisplayClass0_0 CS$<>8__locals1 = new MainThreadHelper.<>c__DisplayClass0_0();
						CS$<>8__locals1.action = action;
						CS$<>8__locals1.tcs = new TaskCompletionSource<object>();
						Device.BeginInvokeOnMainThread(delegate
						{
							MainThreadHelper.<>c__DisplayClass0_0.<<OnMainThreadAsync>b__0>d <<OnMainThreadAsync>b__0>d;
							<<OnMainThreadAsync>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<OnMainThreadAsync>b__0>d.<>4__this = CS$<>8__locals1;
							<<OnMainThreadAsync>b__0>d.<>1__state = -1;
							<<OnMainThreadAsync>b__0>d.<>t__builder.Start<MainThreadHelper.<>c__DisplayClass0_0.<<OnMainThreadAsync>b__0>d>(ref <<OnMainThreadAsync>b__0>d);
						});
						taskAwaiter = CS$<>8__locals1.tcs.Task.GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<object> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, MainThreadHelper.<OnMainThreadAsync>d__0>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<object> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<object>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
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

			// Token: 0x06004734 RID: 18228 RVA: 0x0036D4D0 File Offset: 0x0036B6D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002981 RID: 10625
			public int <>1__state;

			// Token: 0x04002982 RID: 10626
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002983 RID: 10627
			public Func<Task> action;

			// Token: 0x04002984 RID: 10628
			private TaskAwaiter<object> <>u__1;
		}
	}
}
