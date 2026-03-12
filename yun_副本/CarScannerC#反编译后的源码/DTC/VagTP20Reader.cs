using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000576 RID: 1398
	internal class VagTP20Reader
	{
		// Token: 0x06003388 RID: 13192 RVA: 0x00002050 File Offset: 0x00000250
		public VagTP20Reader()
		{
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x00242740 File Offset: 0x00240940
		public async Task<string> GetChannelForDestination(byte destination)
		{
			OBDRequest obdrequest = new OBDRequest(destination.ToString("X2") + "C00010000301", "200", "", "", false);
			obdrequest.ELMFormat = ELMFormat.CAN11bit;
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			string channel = null;
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("NO DATA"))
				{
					semaphore.Release();
					return;
				}
				string text = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (text.Length < 12)
				{
					semaphore.Release();
					return;
				}
				string text2 = text.Substring(8, 2);
				string text3 = text.Substring(10, 2);
				channel = text3 + text2;
				semaphore.Release();
			};
			App.OBDReader.AddRequestToQueue(obdrequest);
			await semaphore.WaitAsync();
			return channel;
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x00242784 File Offset: 0x00240984
		public async Task SetChannelParams(string channel)
		{
			OBDRequest obdrequest = new OBDRequest("A00F8AFF32FF", channel, "", "", false);
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				semaphore.Release();
			};
			await semaphore.WaitAsync();
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x002427C8 File Offset: 0x002409C8
		public async Task<string> SendRequestToChannel(string message, string channel)
		{
			OBDRequest obdrequest = new OBDRequest("1000" + message.Length.ToString("X2") + message, channel, "", "", false);
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			string response = null;
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				response = data;
				semaphore.Release();
			};
			await semaphore.WaitAsync();
			return response;
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x00242814 File Offset: 0x00240A14
		public async Task GetDTCFromChannel(string channel)
		{
			await this.SendRequestToChannel("1089", channel);
			await this.SendRequestToChannel("A8", channel);
		}

		// Token: 0x02000577 RID: 1399
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x0600338D RID: 13197 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x0600338E RID: 13198 RVA: 0x00242860 File Offset: 0x00240A60
			internal void <GetChannelForDestination>b__0(OBDRequest request, string data)
			{
				if (data.Contains("NO DATA"))
				{
					this.semaphore.Release();
					return;
				}
				string text = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (text.Length < 12)
				{
					this.semaphore.Release();
					return;
				}
				string text2 = text.Substring(8, 2);
				string text3 = text.Substring(10, 2);
				this.channel = text3 + text2;
				this.semaphore.Release();
			}

			// Token: 0x04001E43 RID: 7747
			public SemaphoreSlim semaphore;

			// Token: 0x04001E44 RID: 7748
			public string channel;
		}

		// Token: 0x02000578 RID: 1400
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600338F RID: 13199 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06003390 RID: 13200 RVA: 0x002428D1 File Offset: 0x00240AD1
			internal void <SetChannelParams>b__0(OBDRequest request, string data)
			{
				this.semaphore.Release();
			}

			// Token: 0x04001E45 RID: 7749
			public SemaphoreSlim semaphore;
		}

		// Token: 0x02000579 RID: 1401
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06003391 RID: 13201 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06003392 RID: 13202 RVA: 0x002428DF File Offset: 0x00240ADF
			internal void <SendRequestToChannel>b__0(OBDRequest request, string data)
			{
				this.response = data;
				this.semaphore.Release();
			}

			// Token: 0x04001E46 RID: 7750
			public string response;

			// Token: 0x04001E47 RID: 7751
			public SemaphoreSlim semaphore;
		}

		// Token: 0x0200057A RID: 1402
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetChannelForDestination>d__1 : IAsyncStateMachine
		{
			// Token: 0x06003393 RID: 13203 RVA: 0x002428F4 File Offset: 0x00240AF4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string channel;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new VagTP20Reader.<>c__DisplayClass1_0();
						OBDRequest obdrequest = new OBDRequest(destination.ToString("X2") + "C00010000301", "200", "", "", false);
						obdrequest.ELMFormat = ELMFormat.CAN11bit;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.channel = null;
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("NO DATA"))
							{
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							string text = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (text.Length < 12)
							{
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							string text2 = text.Substring(8, 2);
							string text3 = text.Substring(10, 2);
							CS$<>8__locals1.channel = text3 + text2;
							CS$<>8__locals1.semaphore.Release();
						};
						App.OBDReader.AddRequestToQueue(obdrequest);
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagTP20Reader.<GetChannelForDestination>d__1>(ref taskAwaiter, ref this);
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
					channel = CS$<>8__locals1.channel;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(channel);
			}

			// Token: 0x06003394 RID: 13204 RVA: 0x00242A4C File Offset: 0x00240C4C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001E48 RID: 7752
			public int <>1__state;

			// Token: 0x04001E49 RID: 7753
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001E4A RID: 7754
			public byte destination;

			// Token: 0x04001E4B RID: 7755
			private VagTP20Reader.<>c__DisplayClass1_0 <>8__1;

			// Token: 0x04001E4C RID: 7756
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200057B RID: 1403
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetDTCFromChannel>d__4 : IAsyncStateMachine
		{
			// Token: 0x06003395 RID: 13205 RVA: 0x00242A5C File Offset: 0x00240C5C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VagTP20Reader vagTP20Reader = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_00DB;
						}
						taskAwaiter = vagTP20Reader.SendRequestToChannel("1089", channel).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VagTP20Reader.<GetDTCFromChannel>d__4>(ref taskAwaiter, ref this);
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
					taskAwaiter.GetResult();
					taskAwaiter = vagTP20Reader.SendRequestToChannel("A8", channel).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VagTP20Reader.<GetDTCFromChannel>d__4>(ref taskAwaiter, ref this);
						return;
					}
					IL_00DB:
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

			// Token: 0x06003396 RID: 13206 RVA: 0x00242B88 File Offset: 0x00240D88
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001E4D RID: 7757
			public int <>1__state;

			// Token: 0x04001E4E RID: 7758
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001E4F RID: 7759
			public VagTP20Reader <>4__this;

			// Token: 0x04001E50 RID: 7760
			public string channel;

			// Token: 0x04001E51 RID: 7761
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x0200057C RID: 1404
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendRequestToChannel>d__3 : IAsyncStateMachine
		{
			// Token: 0x06003397 RID: 13207 RVA: 0x00242B98 File Offset: 0x00240D98
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string response;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new VagTP20Reader.<>c__DisplayClass3_0();
						OBDRequest obdrequest = new OBDRequest("1000" + message.Length.ToString("X2") + message, channel, "", "", false);
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.response = null;
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							CS$<>8__locals1.response = data;
							CS$<>8__locals1.semaphore.Release();
						};
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagTP20Reader.<SendRequestToChannel>d__3>(ref taskAwaiter, ref this);
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
					response = CS$<>8__locals1.response;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(response);
			}

			// Token: 0x06003398 RID: 13208 RVA: 0x00242CEC File Offset: 0x00240EEC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001E52 RID: 7762
			public int <>1__state;

			// Token: 0x04001E53 RID: 7763
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001E54 RID: 7764
			public string message;

			// Token: 0x04001E55 RID: 7765
			public string channel;

			// Token: 0x04001E56 RID: 7766
			private VagTP20Reader.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04001E57 RID: 7767
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200057D RID: 1405
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SetChannelParams>d__2 : IAsyncStateMachine
		{
			// Token: 0x06003399 RID: 13209 RVA: 0x00242CFC File Offset: 0x00240EFC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						VagTP20Reader.<>c__DisplayClass2_0 CS$<>8__locals1 = new VagTP20Reader.<>c__DisplayClass2_0();
						OBDRequest obdrequest = new OBDRequest("A00F8AFF32FF", channel, "", "", false);
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							CS$<>8__locals1.semaphore.Release();
						};
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagTP20Reader.<SetChannelParams>d__2>(ref taskAwaiter, ref this);
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
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600339A RID: 13210 RVA: 0x00242DEC File Offset: 0x00240FEC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001E58 RID: 7768
			public int <>1__state;

			// Token: 0x04001E59 RID: 7769
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001E5A RID: 7770
			public string channel;

			// Token: 0x04001E5B RID: 7771
			private TaskAwaiter <>u__1;
		}
	}
}
