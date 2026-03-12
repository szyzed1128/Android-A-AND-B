using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200030C RID: 780
	internal class ConnectionSettingsTester
	{
		// Token: 0x06002404 RID: 9220 RVA: 0x001BD4A4 File Offset: 0x001BB6A4
		public async Task<bool> CheckConnection(ConnectionTypes conType, string address)
		{
			this.debugId = new Random().Next();
			bool result = false;
			bool flag;
			if (App.OBDReader != null && App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
			{
				flag = false;
			}
			else
			{
				IOBDConnection connection = null;
				try
				{
					connection = this.GetConnectionFromType(conType);
					TaskAwaiter<bool> taskAwaiter = connection.ConnectAsync(address, PCLDebugStream.CurrentInstance).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						await Task.Delay(300);
						result = await this.CheckForELMResponse(connection);
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					if (connection != null && connection.Connected)
					{
						try
						{
							connection.Disconect();
						}
						catch (Exception)
						{
						}
					}
				}
				flag = result;
			}
			return flag;
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x001BD4F8 File Offset: 0x001BB6F8
		private async Task<bool> CheckForELMResponse(IOBDConnection connection)
		{
			await this.SendString(connection, "ATZ\rATE0\r\r\r\r\r\r\r\r\r\r");
			string text = await this.ReadData(connection);
			bool flag;
			if (text != null && text.Contains('>'))
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x001BD544 File Offset: 0x001BB744
		private async Task SendString(IOBDConnection connection, string data)
		{
			byte[] array = new byte[data.Length + 1];
			Encoding.ASCII.GetBytes(data, 0, data.Length, array, 0);
			array[array.Length - 1] = 13;
			TimeSpan timeout = new TimeSpan(0, 0, 2);
			await connection.WriteBytesAsync(array).TimeoutAfter(timeout);
			await connection.FlushAsync().TimeoutAfter(timeout);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x001BD590 File Offset: 0x001BB790
		private async Task<string> ReadData(IOBDConnection connection)
		{
			int timeout_ms = 2500;
			StringBuilder sb = new StringBuilder();
			int dataCount = 0;
			bool hasFinishCharacter = false;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			int loopcounter = 0;
			while (sw.ElapsedMilliseconds < (long)timeout_ms)
			{
				int num = loopcounter;
				loopcounter = num + 1;
				byte[] array = await connection.ReadBytesAsync();
				if (array.Length == 0)
				{
					await Task.Delay(10);
				}
				else
				{
					bool flag = false;
					char[] array2 = (from b in array
						where b > 0
						select b into x
						select (char)x).ToArray<char>();
					if (array2 != null && array2.Length != 0)
					{
						string text = new string(array2);
						dataCount += text.Length;
						sb.Append(text);
						flag = true;
						if (text.EndsWith('>'))
						{
							hasFinishCharacter = true;
							sw.Stop();
							break;
						}
					}
					if (flag)
					{
						sw.Restart();
					}
				}
			}
			string text2 = sb.ToString();
			sw.Stop();
			string text3;
			if (hasFinishCharacter)
			{
				text3 = text2;
			}
			else
			{
				text3 = "";
			}
			return text3;
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x001BD5D4 File Offset: 0x001BB7D4
		private IOBDConnection GetConnectionFromType(ConnectionTypes conType)
		{
			IOBDConnection iobdconnection = null;
			switch (conType)
			{
			case ConnectionTypes.WiFi:
				if (PlatformHelper.IsAndroid)
				{
					iobdconnection = DependencyService.Get<ITCPConnectionV2Droid>(1);
					(iobdconnection as ITCPConnectionV2Droid).Bind = true;
				}
				else
				{
					iobdconnection = DependencyService.Get<ITCPConnection>(1);
				}
				break;
			case ConnectionTypes.BluetoothLE:
				iobdconnection = new BTLEConnection(new Guid(SharedSettings.Current.BTLEServiceID), new Guid(SharedSettings.Current.BTLEInputID), new Guid(SharedSettings.Current.BTLEOutputID));
				break;
			case ConnectionTypes.Bluetooth:
				iobdconnection = DependencyService.Get<IBluetoothConnection>(1);
				if (PlatformHelper.IsAndroid)
				{
					(iobdconnection as IBluetoothConnection).ConnectionTimeoutSeconds = 4;
				}
				break;
			case ConnectionTypes.MFI_OBDLinkMXPlus:
				iobdconnection = DependencyService.Get<IBluetoothConnection>(1);
				break;
			}
			return iobdconnection;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionSettingsTester()
		{
		}

		// Token: 0x040011B3 RID: 4531
		private int debugId;

		// Token: 0x0200030D RID: 781
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600240A RID: 9226 RVA: 0x001BD67D File Offset: 0x001BB87D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600240B RID: 9227 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600240C RID: 9228 RVA: 0x001BD689 File Offset: 0x001BB889
			internal bool <ReadData>b__4_0(byte b)
			{
				return b > 0;
			}

			// Token: 0x0600240D RID: 9229 RVA: 0x00016849 File Offset: 0x00014A49
			internal char <ReadData>b__4_1(byte x)
			{
				return (char)x;
			}

			// Token: 0x040011B4 RID: 4532
			public static readonly ConnectionSettingsTester.<>c <>9 = new ConnectionSettingsTester.<>c();

			// Token: 0x040011B5 RID: 4533
			public static Func<byte, bool> <>9__4_0;

			// Token: 0x040011B6 RID: 4534
			public static Func<byte, char> <>9__4_1;
		}

		// Token: 0x0200030E RID: 782
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckConnection>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600240E RID: 9230 RVA: 0x001BD690 File Offset: 0x001BB890
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ConnectionSettingsTester connectionSettingsTester = this;
				bool flag;
				try
				{
					if (num > 2)
					{
						connectionSettingsTester.debugId = new Random().Next();
						result = false;
						if (App.OBDReader != null && App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
						{
							flag = false;
							goto IL_0206;
						}
						connection = null;
					}
					try
					{
						TaskAwaiter<bool> taskAwaiter3;
						TaskAwaiter taskAwaiter4;
						switch (num)
						{
						case 0:
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num = (num2 = -1);
							break;
						case 1:
						{
							TaskAwaiter taskAwaiter5;
							taskAwaiter4 = taskAwaiter5;
							taskAwaiter5 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_013A;
						}
						case 2:
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num = (num2 = -1);
							goto IL_019B;
						default:
							connection = connectionSettingsTester.GetConnectionFromType(conType);
							taskAwaiter3 = connection.ConnectAsync(address, PCLDebugStream.CurrentInstance).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ConnectionSettingsTester.<CheckConnection>d__1>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						}
						if (!taskAwaiter3.GetResult())
						{
							goto IL_01AC;
						}
						taskAwaiter4 = Task.Delay(300).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionSettingsTester.<CheckConnection>d__1>(ref taskAwaiter4, ref this);
							return;
						}
						IL_013A:
						taskAwaiter4.GetResult();
						taskAwaiter3 = connectionSettingsTester.CheckForELMResponse(connection).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 2);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ConnectionSettingsTester.<CheckConnection>d__1>(ref taskAwaiter3, ref this);
							return;
						}
						IL_019B:
						bool result2 = taskAwaiter3.GetResult();
						result = result2;
						IL_01AC:;
					}
					catch (Exception)
					{
					}
					finally
					{
						if (num < 0 && connection != null && connection.Connected)
						{
							try
							{
								connection.Disconect();
							}
							catch (Exception)
							{
							}
						}
					}
					flag = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					connection = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0206:
				num2 = -2;
				connection = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600240F RID: 9231 RVA: 0x001BD924 File Offset: 0x001BBB24
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040011B7 RID: 4535
			public int <>1__state;

			// Token: 0x040011B8 RID: 4536
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040011B9 RID: 4537
			public ConnectionSettingsTester <>4__this;

			// Token: 0x040011BA RID: 4538
			public ConnectionTypes conType;

			// Token: 0x040011BB RID: 4539
			public string address;

			// Token: 0x040011BC RID: 4540
			private bool <result>5__2;

			// Token: 0x040011BD RID: 4541
			private IOBDConnection <connection>5__3;

			// Token: 0x040011BE RID: 4542
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040011BF RID: 4543
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200030F RID: 783
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckForELMResponse>d__2 : IAsyncStateMachine
		{
			// Token: 0x06002410 RID: 9232 RVA: 0x001BD934 File Offset: 0x001BBB34
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ConnectionSettingsTester connectionSettingsTester = this;
				bool flag;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
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
						taskAwaiter3 = connectionSettingsTester.SendString(connection, "ATZ\rATE0\r\r\r\r\r\r\r\r\r\r").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionSettingsTester.<CheckForELMResponse>d__2>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = connectionSettingsTester.ReadData(connection).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, ConnectionSettingsTester.<CheckForELMResponse>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_00DB:
					string result = taskAwaiter.GetResult();
					if (result != null && result.Contains('>'))
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002411 RID: 9233 RVA: 0x001BDA78 File Offset: 0x001BBC78
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040011C0 RID: 4544
			public int <>1__state;

			// Token: 0x040011C1 RID: 4545
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040011C2 RID: 4546
			public ConnectionSettingsTester <>4__this;

			// Token: 0x040011C3 RID: 4547
			public IOBDConnection connection;

			// Token: 0x040011C4 RID: 4548
			private TaskAwaiter <>u__1;

			// Token: 0x040011C5 RID: 4549
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x02000310 RID: 784
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadData>d__4 : IAsyncStateMachine
		{
			// Token: 0x06002412 RID: 9234 RVA: 0x001BDA88 File Offset: 0x001BBC88
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string text3;
				try
				{
					TaskAwaiter taskAwaiter;
					ValueTaskAwaiter<byte[]> valueTaskAwaiter;
					if (num != 0)
					{
						if (num != 1)
						{
							timeout_ms = 2500;
							sb = new StringBuilder();
							dataCount = 0;
							hasFinishCharacter = false;
							sw = new Stopwatch();
							sw.Start();
							loopcounter = 0;
							goto IL_01F4;
						}
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0130;
					}
					else
					{
						ValueTaskAwaiter<byte[]> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<byte[]>);
						num2 = -1;
					}
					IL_00CC:
					byte[] result = valueTaskAwaiter.GetResult();
					if (result.Length == 0)
					{
						taskAwaiter = Task.Delay(10).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionSettingsTester.<ReadData>d__4>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						bool flag = false;
						char[] array = (from b in result
							where b > 0
							select b into x
							select (char)x).ToArray<char>();
						if (array != null && array.Length != 0)
						{
							string text = new string(array);
							dataCount += text.Length;
							sb.Append(text);
							flag = true;
							if (text.EndsWith('>'))
							{
								hasFinishCharacter = true;
								sw.Stop();
								goto IL_020B;
							}
						}
						if (flag)
						{
							sw.Restart();
							goto IL_01F4;
						}
						goto IL_01F4;
					}
					IL_0130:
					taskAwaiter.GetResult();
					IL_01F4:
					if (sw.ElapsedMilliseconds < (long)timeout_ms)
					{
						int num3 = loopcounter;
						loopcounter = num3 + 1;
						valueTaskAwaiter = connection.ReadBytesAsync().GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num2 = 0;
							ValueTaskAwaiter<byte[]> valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<byte[]>, ConnectionSettingsTester.<ReadData>d__4>(ref valueTaskAwaiter, ref this);
							return;
						}
						goto IL_00CC;
					}
					IL_020B:
					string text2 = sb.ToString();
					sw.Stop();
					if (hasFinishCharacter)
					{
						text3 = text2;
					}
					else
					{
						text3 = "";
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					sb = null;
					sw = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				sb = null;
				sw = null;
				this.<>t__builder.SetResult(text3);
			}

			// Token: 0x06002413 RID: 9235 RVA: 0x001BDD30 File Offset: 0x001BBF30
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040011C6 RID: 4550
			public int <>1__state;

			// Token: 0x040011C7 RID: 4551
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x040011C8 RID: 4552
			public IOBDConnection connection;

			// Token: 0x040011C9 RID: 4553
			private int <timeout_ms>5__2;

			// Token: 0x040011CA RID: 4554
			private StringBuilder <sb>5__3;

			// Token: 0x040011CB RID: 4555
			private int <dataCount>5__4;

			// Token: 0x040011CC RID: 4556
			private bool <hasFinishCharacter>5__5;

			// Token: 0x040011CD RID: 4557
			private Stopwatch <sw>5__6;

			// Token: 0x040011CE RID: 4558
			private int <loopcounter>5__7;

			// Token: 0x040011CF RID: 4559
			private ValueTaskAwaiter<byte[]> <>u__1;

			// Token: 0x040011D0 RID: 4560
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000311 RID: 785
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendString>d__3 : IAsyncStateMachine
		{
			// Token: 0x06002414 RID: 9236 RVA: 0x001BDD40 File Offset: 0x001BBF40
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
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
							goto IL_012A;
						}
						byte[] array = new byte[data.Length + 1];
						Encoding.ASCII.GetBytes(data, 0, data.Length, array, 0);
						array[array.Length - 1] = 13;
						timeout = new TimeSpan(0, 0, 2);
						taskAwaiter = connection.WriteBytesAsync(array).TimeoutAfter(timeout).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionSettingsTester.<SendString>d__3>(ref taskAwaiter, ref this);
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
					taskAwaiter = connection.FlushAsync().TimeoutAfter(timeout).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionSettingsTester.<SendString>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_012A:
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

			// Token: 0x06002415 RID: 9237 RVA: 0x001BDEC8 File Offset: 0x001BC0C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040011D1 RID: 4561
			public int <>1__state;

			// Token: 0x040011D2 RID: 4562
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040011D3 RID: 4563
			public string data;

			// Token: 0x040011D4 RID: 4564
			public IOBDConnection connection;

			// Token: 0x040011D5 RID: 4565
			private TimeSpan <timeout>5__2;

			// Token: 0x040011D6 RID: 4566
			private TaskAwaiter <>u__1;
		}
	}
}
