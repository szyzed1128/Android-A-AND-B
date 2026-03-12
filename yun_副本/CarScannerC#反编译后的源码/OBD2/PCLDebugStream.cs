using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200039B RID: 923
	public class PCLDebugStream : IDisposable
	{
		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x0600270A RID: 9994 RVA: 0x001DFD8B File Offset: 0x001DDF8B
		// (set) Token: 0x0600270B RID: 9995 RVA: 0x001DFD92 File Offset: 0x001DDF92
		public static PCLDebugStream CurrentInstance
		{
			get
			{
				return PCLDebugStream._CurrentInstance;
			}
			private set
			{
				PCLDebugStream._CurrentInstance = value;
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x001DFD9C File Offset: 0x001DDF9C
		private PCLDebugStream()
		{
			Random random = new Random();
			this._sessionId = random.Next(0, int.MaxValue);
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x001DFDC8 File Offset: 0x001DDFC8
		public void Clear()
		{
			try
			{
				this.Close();
				using (FileStream fileStream = File.Create(FileSystemHelper.GetLocalFilePath("log.txt")))
				{
					fileStream.Write(new byte[] { 239, 191, 190 }, 0, 3);
					fileStream.Flush();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x001DFE38 File Offset: 0x001DE038
		public void Close()
		{
			if (this.stream != null)
			{
				this.stream.Flush();
				this.stream.Close();
				this.stream = null;
			}
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x001DFE60 File Offset: 0x001DE060
		public async Task Flush()
		{
			if (this.stream != null)
			{
				if (this.stream != null)
				{
					try
					{
						await this.stream.FlushAsync();
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x001DFEA3 File Offset: 0x001DE0A3
		public static string GetFilepath()
		{
			return FileSystemHelper.GetLocalFilePath("log.txt");
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x001DFEB0 File Offset: 0x001DE0B0
		private void Open()
		{
			try
			{
				if (this.stream == null)
				{
					this.stream = File.Open(PCLDebugStream.GetFilepath(), FileMode.OpenOrCreate);
					if (this.stream.Length == 0L)
					{
						this.stream.Write(new byte[] { 239, 191, 190 }, 0, 3);
					}
					if (this.stream.Length > 10485760L)
					{
						this.stream.Seek(this.stream.Length - 6291456L, SeekOrigin.Begin);
						string localFilePath = FileSystemHelper.GetLocalFilePath("newlog.txt");
						using (FileStream fileStream = File.Create(localFilePath))
						{
							this.stream.Write(new byte[] { 239, 191, 190 }, 0, 3);
							this.stream.CopyTo(fileStream);
						}
						this.stream.Close();
						this.stream = null;
						File.Delete(PCLDebugStream.GetFilepath());
						File.Move(localFilePath, PCLDebugStream.GetFilepath());
						this.Open();
					}
					else
					{
						this.stream.Seek(this.stream.Length, SeekOrigin.Begin);
					}
				}
			}
			catch (Exception)
			{
				this.stream = null;
			}
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x001E0008 File Offset: 0x001DE208
		public void Write(byte[] data)
		{
			if (this.stream == null)
			{
				this.Open();
			}
			if (this.stream != null)
			{
				this.stream.Write(data, 0, data.Length);
				this.stream.Flush();
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x001E003C File Offset: 0x001DE23C
		public async Task WriteWithFlush(byte[] data)
		{
			if (this.stream == null)
			{
				this.Open();
			}
			if (this.stream != null)
			{
				await this.stream.WriteAsync(data, 0, data.Length);
				await this.stream.FlushAsync();
			}
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x001E0087 File Offset: 0x001DE287
		public Task WriteWithFlush(string data)
		{
			return this.WriteWithFlush(Encoding.UTF8.GetBytes(data));
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x001E009C File Offset: 0x001DE29C
		public async Task WriteAsync(byte[] data)
		{
			if (this.stream == null)
			{
				this.Open();
			}
			if (this.stream != null)
			{
				await this.stream.WriteAsync(data, 0, data.Length);
			}
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x001E00E7 File Offset: 0x001DE2E7
		public void WriteByte(byte b)
		{
			if (this.stream == null)
			{
				this.Open();
			}
			if (this.stream != null)
			{
				this.stream.WriteByte(b);
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x001E010C File Offset: 0x001DE30C
		public async Task WriteAsync(string s)
		{
			if (this.stream == null)
			{
				this.Open();
			}
			if (this.stream != null)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				await this.stream.WriteAsync(bytes, 0, bytes.Length);
			}
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x001E0158 File Offset: 0x001DE358
		public async Task WriteLineAsync(string s)
		{
			if (this.stream == null)
			{
				this.Open();
			}
			if (this.stream != null)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(s + "\r\n");
				await this.stream.WriteAsync(bytes, 0, bytes.Length);
			}
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x001E01A3 File Offset: 0x001DE3A3
		public void Dispose()
		{
			this.Close();
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x001E01AB File Offset: 0x001DE3AB
		// Note: this type is marked as 'beforefieldinit'.
		static PCLDebugStream()
		{
		}

		// Token: 0x0400153D RID: 5437
		private static PCLDebugStream _CurrentInstance = new PCLDebugStream();

		// Token: 0x0400153E RID: 5438
		private int _sessionId;

		// Token: 0x0400153F RID: 5439
		private Stream stream;

		// Token: 0x0200039C RID: 924
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Flush>d__9 : IAsyncStateMachine
		{
			// Token: 0x0600271B RID: 10011 RVA: 0x001E01B8 File Offset: 0x001DE3B8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PCLDebugStream pcldebugStream = this;
				try
				{
					if (num != 0)
					{
						if (pcldebugStream.stream == null)
						{
							goto IL_00A5;
						}
						if (pcldebugStream.stream == null)
						{
							goto IL_008C;
						}
					}
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = pcldebugStream.stream.FlushAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PCLDebugStream.<Flush>d__9>(ref taskAwaiter, ref this);
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
					}
					catch (Exception)
					{
					}
					IL_008C:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00A5:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600271C RID: 10012 RVA: 0x001E029C File Offset: 0x001DE49C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001540 RID: 5440
			public int <>1__state;

			// Token: 0x04001541 RID: 5441
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001542 RID: 5442
			public PCLDebugStream <>4__this;

			// Token: 0x04001543 RID: 5443
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200039D RID: 925
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteAsync>d__15 : IAsyncStateMachine
		{
			// Token: 0x0600271D RID: 10013 RVA: 0x001E02AC File Offset: 0x001DE4AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PCLDebugStream pcldebugStream = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (pcldebugStream.stream == null)
						{
							pcldebugStream.Open();
						}
						if (pcldebugStream.stream == null)
						{
							goto IL_0093;
						}
						taskAwaiter = pcldebugStream.stream.WriteAsync(data, 0, data.Length).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PCLDebugStream.<WriteAsync>d__15>(ref taskAwaiter, ref this);
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
					IL_0093:;
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

			// Token: 0x0600271E RID: 10014 RVA: 0x001E0388 File Offset: 0x001DE588
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001544 RID: 5444
			public int <>1__state;

			// Token: 0x04001545 RID: 5445
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001546 RID: 5446
			public PCLDebugStream <>4__this;

			// Token: 0x04001547 RID: 5447
			public byte[] data;

			// Token: 0x04001548 RID: 5448
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200039E RID: 926
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteAsync>d__17 : IAsyncStateMachine
		{
			// Token: 0x0600271F RID: 10015 RVA: 0x001E0398 File Offset: 0x001DE598
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PCLDebugStream pcldebugStream = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (pcldebugStream.stream == null)
						{
							pcldebugStream.Open();
						}
						if (pcldebugStream.stream == null)
						{
							goto IL_009A;
						}
						byte[] bytes = Encoding.UTF8.GetBytes(s);
						taskAwaiter = pcldebugStream.stream.WriteAsync(bytes, 0, bytes.Length).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PCLDebugStream.<WriteAsync>d__17>(ref taskAwaiter, ref this);
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
					IL_009A:;
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

			// Token: 0x06002720 RID: 10016 RVA: 0x001E0480 File Offset: 0x001DE680
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001549 RID: 5449
			public int <>1__state;

			// Token: 0x0400154A RID: 5450
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400154B RID: 5451
			public PCLDebugStream <>4__this;

			// Token: 0x0400154C RID: 5452
			public string s;

			// Token: 0x0400154D RID: 5453
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200039F RID: 927
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteLineAsync>d__18 : IAsyncStateMachine
		{
			// Token: 0x06002721 RID: 10017 RVA: 0x001E0490 File Offset: 0x001DE690
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PCLDebugStream pcldebugStream = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (pcldebugStream.stream == null)
						{
							pcldebugStream.Open();
						}
						if (pcldebugStream.stream == null)
						{
							goto IL_00A4;
						}
						byte[] bytes = Encoding.UTF8.GetBytes(s + "\r\n");
						taskAwaiter = pcldebugStream.stream.WriteAsync(bytes, 0, bytes.Length).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PCLDebugStream.<WriteLineAsync>d__18>(ref taskAwaiter, ref this);
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
					IL_00A4:;
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

			// Token: 0x06002722 RID: 10018 RVA: 0x001E0580 File Offset: 0x001DE780
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400154E RID: 5454
			public int <>1__state;

			// Token: 0x0400154F RID: 5455
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001550 RID: 5456
			public PCLDebugStream <>4__this;

			// Token: 0x04001551 RID: 5457
			public string s;

			// Token: 0x04001552 RID: 5458
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020003A0 RID: 928
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteWithFlush>d__13 : IAsyncStateMachine
		{
			// Token: 0x06002723 RID: 10019 RVA: 0x001E0590 File Offset: 0x001DE790
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PCLDebugStream pcldebugStream = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00F6;
						}
						if (pcldebugStream.stream == null)
						{
							pcldebugStream.Open();
						}
						if (pcldebugStream.stream == null)
						{
							goto IL_00FD;
						}
						taskAwaiter = pcldebugStream.stream.WriteAsync(data, 0, data.Length).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PCLDebugStream.<WriteWithFlush>d__13>(ref taskAwaiter, ref this);
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
					taskAwaiter = pcldebugStream.stream.FlushAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PCLDebugStream.<WriteWithFlush>d__13>(ref taskAwaiter, ref this);
						return;
					}
					IL_00F6:
					taskAwaiter.GetResult();
					IL_00FD:;
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

			// Token: 0x06002724 RID: 10020 RVA: 0x001E06D8 File Offset: 0x001DE8D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001553 RID: 5459
			public int <>1__state;

			// Token: 0x04001554 RID: 5460
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001555 RID: 5461
			public PCLDebugStream <>4__this;

			// Token: 0x04001556 RID: 5462
			public byte[] data;

			// Token: 0x04001557 RID: 5463
			private TaskAwaiter <>u__1;
		}
	}
}
