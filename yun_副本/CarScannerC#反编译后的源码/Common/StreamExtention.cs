using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x02000800 RID: 2048
	public static class StreamExtention
	{
		// Token: 0x06004789 RID: 18313 RVA: 0x0036E720 File Offset: 0x0036C920
		public static async Task<int> ReadAsyncWithTimeout(this Stream stream, byte[] buffer, int offset, int count, int timeoutMilliseconds)
		{
			if (stream.CanRead)
			{
				Task<int> readTask = Task.Run<int>(() => stream.Read(buffer, offset, count));
				Task task = Task.Delay(timeoutMilliseconds);
				TaskAwaiter<Task> taskAwaiter = Task.WhenAny(new Task[] { readTask, task }).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<Task> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<Task>);
				}
				if (taskAwaiter.GetResult() == readTask)
				{
					return await readTask;
				}
				readTask = null;
			}
			return 0;
		}

		// Token: 0x02000801 RID: 2049
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x0600478A RID: 18314 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x0600478B RID: 18315 RVA: 0x0036E784 File Offset: 0x0036C984
			internal int <ReadAsyncWithTimeout>b__0()
			{
				return this.stream.Read(this.buffer, this.offset, this.count);
			}

			// Token: 0x0400299A RID: 10650
			public Stream stream;

			// Token: 0x0400299B RID: 10651
			public byte[] buffer;

			// Token: 0x0400299C RID: 10652
			public int offset;

			// Token: 0x0400299D RID: 10653
			public int count;
		}

		// Token: 0x02000802 RID: 2050
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadAsyncWithTimeout>d__0 : IAsyncStateMachine
		{
			// Token: 0x0600478C RID: 18316 RVA: 0x0036E7A4 File Offset: 0x0036C9A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				int num3;
				try
				{
					TaskAwaiter<int> taskAwaiter3;
					TaskAwaiter<Task> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<int> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<int>);
							num2 = -1;
							goto IL_0149;
						}
						StreamExtention.<>c__DisplayClass0_0 CS$<>8__locals1 = new StreamExtention.<>c__DisplayClass0_0();
						CS$<>8__locals1.stream = stream;
						CS$<>8__locals1.buffer = buffer;
						CS$<>8__locals1.offset = offset;
						CS$<>8__locals1.count = count;
						if (!CS$<>8__locals1.stream.CanRead)
						{
							goto IL_015A;
						}
						readTask = Task.Run<int>(() => CS$<>8__locals1.stream.Read(CS$<>8__locals1.buffer, CS$<>8__locals1.offset, CS$<>8__locals1.count));
						Task task = Task.Delay(timeoutMilliseconds);
						taskAwaiter5 = Task.WhenAny(new Task[] { readTask, task }).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, StreamExtention.<ReadAsyncWithTimeout>d__0>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Task>);
						num2 = -1;
					}
					if (taskAwaiter5.GetResult() != readTask)
					{
						readTask = null;
						goto IL_015A;
					}
					taskAwaiter3 = readTask.GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<int> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, StreamExtention.<ReadAsyncWithTimeout>d__0>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0149:
					num3 = taskAwaiter3.GetResult();
					goto IL_0177;
					IL_015A:
					num3 = 0;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0177:
				num2 = -2;
				this.<>t__builder.SetResult(num3);
			}

			// Token: 0x0600478D RID: 18317 RVA: 0x0036E958 File Offset: 0x0036CB58
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400299E RID: 10654
			public int <>1__state;

			// Token: 0x0400299F RID: 10655
			public AsyncTaskMethodBuilder<int> <>t__builder;

			// Token: 0x040029A0 RID: 10656
			public Stream stream;

			// Token: 0x040029A1 RID: 10657
			public byte[] buffer;

			// Token: 0x040029A2 RID: 10658
			public int offset;

			// Token: 0x040029A3 RID: 10659
			public int count;

			// Token: 0x040029A4 RID: 10660
			public int timeoutMilliseconds;

			// Token: 0x040029A5 RID: 10661
			private Task<int> <readTask>5__2;

			// Token: 0x040029A6 RID: 10662
			private TaskAwaiter<Task> <>u__1;

			// Token: 0x040029A7 RID: 10663
			private TaskAwaiter<int> <>u__2;
		}
	}
}
