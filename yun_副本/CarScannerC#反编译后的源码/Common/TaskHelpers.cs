using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x02000807 RID: 2055
	public static class TaskHelpers
	{
		// Token: 0x0600479F RID: 18335 RVA: 0x0036EBA0 File Offset: 0x0036CDA0
		public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
		{
			TResult tresult;
			using (CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource())
			{
				TaskAwaiter<Task> taskAwaiter = Task.WhenAny(new Task[]
				{
					task,
					Task.Delay(timeout, timeoutCancellationTokenSource.Token)
				}).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<Task> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<Task>);
				}
				if (taskAwaiter.GetResult() != task)
				{
					throw new TimeoutException("The operation has timed out.");
				}
				timeoutCancellationTokenSource.Cancel();
				tresult = await task;
			}
			return tresult;
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x0036EBEC File Offset: 0x0036CDEC
		public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
		{
			using (CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource())
			{
				TaskAwaiter<Task> taskAwaiter = Task.WhenAny(new Task[]
				{
					task,
					Task.Delay(timeout, timeoutCancellationTokenSource.Token)
				}).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<Task> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<Task>);
				}
				if (taskAwaiter.GetResult() != task)
				{
					throw new TimeoutException("The operation has timed out.");
				}
				timeoutCancellationTokenSource.Cancel();
				await task;
			}
			CancellationTokenSource timeoutCancellationTokenSource = null;
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x0036EC38 File Offset: 0x0036CE38
		public static async Task<Task> WhenAnyCompletedSuccessfulyWithTimeout(IEnumerable<Task> tasks, int timeoutMs, int delayMs = 100)
		{
			int i = 0;
			while (i < timeoutMs)
			{
				foreach (Task task in tasks)
				{
					if (task.IsCompletedSuccessfully)
					{
						return task;
					}
				}
				if (!tasks.All((Task x) => x.IsFaulted))
				{
					await Task.Delay(delayMs);
					i += delayMs;
					continue;
				}
				return null;
			}
			return null;
		}

		// Token: 0x02000808 RID: 2056
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060047A2 RID: 18338 RVA: 0x0036EC8B File Offset: 0x0036CE8B
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060047A3 RID: 18339 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060047A4 RID: 18340 RVA: 0x0036EC97 File Offset: 0x0036CE97
			internal bool <WhenAnyCompletedSuccessfulyWithTimeout>b__2_0(Task x)
			{
				return x.IsFaulted;
			}

			// Token: 0x040029AF RID: 10671
			public static readonly TaskHelpers.<>c <>9 = new TaskHelpers.<>c();

			// Token: 0x040029B0 RID: 10672
			public static Func<Task, bool> <>9__2_0;
		}

		// Token: 0x02000809 RID: 2057
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <TimeoutAfter>d__0<TResult> : IAsyncStateMachine
		{
			// Token: 0x060047A5 RID: 18341 RVA: 0x0036ECA0 File Offset: 0x0036CEA0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TResult result;
				try
				{
					if (num > 1)
					{
						timeoutCancellationTokenSource = new CancellationTokenSource();
					}
					try
					{
						TaskAwaiter<TResult> taskAwaiter3;
						TaskAwaiter<Task> taskAwaiter5;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter<TResult> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<TResult>);
								num = (num2 = -1);
								goto IL_0107;
							}
							taskAwaiter5 = Task.WhenAny(new Task[]
							{
								task,
								Task.Delay(timeout, timeoutCancellationTokenSource.Token)
							}).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, TaskHelpers.<TimeoutAfter>d__0<TResult>>(ref taskAwaiter5, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Task>);
							num = (num2 = -1);
						}
						if (taskAwaiter5.GetResult() != task)
						{
							throw new TimeoutException("The operation has timed out.");
						}
						timeoutCancellationTokenSource.Cancel();
						taskAwaiter3 = task.GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter<TResult> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TResult>, TaskHelpers.<TimeoutAfter>d__0<TResult>>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0107:
						result = taskAwaiter3.GetResult();
					}
					finally
					{
						if (num < 0 && timeoutCancellationTokenSource != null)
						{
							((IDisposable)timeoutCancellationTokenSource).Dispose();
						}
					}
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

			// Token: 0x060047A6 RID: 18342 RVA: 0x0036EE44 File Offset: 0x0036D044
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040029B1 RID: 10673
			public int <>1__state;

			// Token: 0x040029B2 RID: 10674
			public AsyncTaskMethodBuilder<TResult> <>t__builder;

			// Token: 0x040029B3 RID: 10675
			public Task<TResult> task;

			// Token: 0x040029B4 RID: 10676
			public TimeSpan timeout;

			// Token: 0x040029B5 RID: 10677
			private CancellationTokenSource <timeoutCancellationTokenSource>5__2;

			// Token: 0x040029B6 RID: 10678
			private TaskAwaiter<Task> <>u__1;

			// Token: 0x040029B7 RID: 10679
			private TaskAwaiter<TResult> <>u__2;
		}

		// Token: 0x0200080A RID: 2058
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <TimeoutAfter>d__1 : IAsyncStateMachine
		{
			// Token: 0x060047A7 RID: 18343 RVA: 0x0036EE54 File Offset: 0x0036D054
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					if (num > 1)
					{
						timeoutCancellationTokenSource = new CancellationTokenSource();
					}
					try
					{
						TaskAwaiter taskAwaiter3;
						TaskAwaiter<Task> taskAwaiter5;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0107;
							}
							taskAwaiter5 = Task.WhenAny(new Task[]
							{
								task,
								Task.Delay(timeout, timeoutCancellationTokenSource.Token)
							}).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, TaskHelpers.<TimeoutAfter>d__1>(ref taskAwaiter5, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Task>);
							num = (num2 = -1);
						}
						if (taskAwaiter5.GetResult() != task)
						{
							throw new TimeoutException("The operation has timed out.");
						}
						timeoutCancellationTokenSource.Cancel();
						taskAwaiter3 = task.GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TaskHelpers.<TimeoutAfter>d__1>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0107:
						taskAwaiter3.GetResult();
					}
					finally
					{
						if (num < 0 && timeoutCancellationTokenSource != null)
						{
							((IDisposable)timeoutCancellationTokenSource).Dispose();
						}
					}
					timeoutCancellationTokenSource = null;
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

			// Token: 0x060047A8 RID: 18344 RVA: 0x0036F000 File Offset: 0x0036D200
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040029B8 RID: 10680
			public int <>1__state;

			// Token: 0x040029B9 RID: 10681
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040029BA RID: 10682
			public Task task;

			// Token: 0x040029BB RID: 10683
			public TimeSpan timeout;

			// Token: 0x040029BC RID: 10684
			private CancellationTokenSource <timeoutCancellationTokenSource>5__2;

			// Token: 0x040029BD RID: 10685
			private TaskAwaiter<Task> <>u__1;

			// Token: 0x040029BE RID: 10686
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200080B RID: 2059
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WhenAnyCompletedSuccessfulyWithTimeout>d__2 : IAsyncStateMachine
		{
			// Token: 0x060047A9 RID: 18345 RVA: 0x0036F010 File Offset: 0x0036D210
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Task task;
				try
				{
					if (num != 0)
					{
						i = 0;
						goto IL_00FB;
					}
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num = (num2 = -1);
					IL_00E1:
					taskAwaiter.GetResult();
					i += delayMs;
					IL_00FB:
					if (i >= timeoutMs)
					{
						task = null;
					}
					else
					{
						IEnumerator<Task> enumerator = tasks.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								Task task2 = enumerator.Current;
								if (task2.IsCompletedSuccessfully)
								{
									task = task2;
									goto IL_0129;
								}
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						if (tasks.All((Task x) => x.IsFaulted))
						{
							task = null;
						}
						else
						{
							taskAwaiter = Task.Delay(delayMs).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TaskHelpers.<WhenAnyCompletedSuccessfulyWithTimeout>d__2>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_00E1;
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0129:
				num2 = -2;
				this.<>t__builder.SetResult(task);
			}

			// Token: 0x060047AA RID: 18346 RVA: 0x0036F190 File Offset: 0x0036D390
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040029BF RID: 10687
			public int <>1__state;

			// Token: 0x040029C0 RID: 10688
			public AsyncTaskMethodBuilder<Task> <>t__builder;

			// Token: 0x040029C1 RID: 10689
			public IEnumerable<Task> tasks;

			// Token: 0x040029C2 RID: 10690
			public int delayMs;

			// Token: 0x040029C3 RID: 10691
			public int timeoutMs;

			// Token: 0x040029C4 RID: 10692
			private int <i>5__2;

			// Token: 0x040029C5 RID: 10693
			private TaskAwaiter <>u__1;
		}
	}
}
