using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x020002E3 RID: 739
	public class BTLEConnection : IOBDConnection
	{
		// Token: 0x06002337 RID: 9015 RVA: 0x001AE1F9 File Offset: 0x001AC3F9
		public BTLEConnection(Guid service, Guid input, Guid output)
		{
			this.adapter = CrossBluetoothLE.Current.Adapter;
			this.g_service = service;
			this.g_input = input;
			this.g_output = output;
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x06002338 RID: 9016 RVA: 0x001AE238 File Offset: 0x001AC438
		public bool Connected
		{
			get
			{
				return this.device != null && this.device.State == 2;
			}
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x001AE255 File Offset: 0x001AC455
		// (set) Token: 0x0600233A RID: 9018 RVA: 0x001AE25D File Offset: 0x001AC45D
		public bool KeepAlive
		{
			[CompilerGenerated]
			get
			{
				return this.<KeepAlive>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<KeepAlive>k__BackingField = value;
			}
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x0600233B RID: 9019 RVA: 0x001AE266 File Offset: 0x001AC466
		// (set) Token: 0x0600233C RID: 9020 RVA: 0x001AE26E File Offset: 0x001AC46E
		public bool NoDelay
		{
			[CompilerGenerated]
			get
			{
				return this.<NoDelay>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NoDelay>k__BackingField = value;
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x001AE278 File Offset: 0x001AC478
		public async Task<bool> ConnectAsync(string device_guid, PCLDebugStream debugStream)
		{
			this.CurrentState = BTLEConnection.State.None;
			this.buffer.Clear();
			bool flag;
			if (Guid.TryParse(device_guid, out this.g_device))
			{
				Task[] tasks = new Task[]
				{
					this.EstablishConnectionAsync(),
					Task.Delay(TimeSpan.FromSeconds(5.0))
				};
				TaskAwaiter<Task> taskAwaiter = Task.WhenAny(tasks).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<Task> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<Task>);
				}
				if (taskAwaiter.GetResult() == tasks[1])
				{
					flag = false;
				}
				else
				{
					Task<bool> task = (Task<bool>)tasks[0];
					if (task.IsFaulted || task.IsCanceled)
					{
						flag = false;
					}
					else
					{
						flag = task.Result;
					}
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x001AE2C4 File Offset: 0x001AC4C4
		private async Task<bool> EstablishConnectionAsync()
		{
			this.CurrentState = BTLEConnection.State.None;
			if (this.adapter.IsScanning)
			{
				await this.adapter.StopScanningForDevicesAsync();
			}
			if (PlatformHelper.IsAndroid)
			{
				try
				{
					ConnectParameters connectParameters;
					connectParameters..ctor(false, true);
					this.device = await this.adapter.ConnectToKnownDeviceAsync(this.g_device, connectParameters, default(CancellationToken));
				}
				catch (Exception)
				{
				}
			}
			if (PlatformHelper.IsiOS)
			{
				this.device = await this.adapter.ConnectToKnownDeviceAsync(this.g_device, default(ConnectParameters), default(CancellationToken));
			}
			if (this.device != null)
			{
				IService service = await this.device.GetServiceAsync(this.g_service, default(CancellationToken));
				if (service != null)
				{
					if (this.char_input != null)
					{
						this.char_input.ValueUpdated -= this.OnDataReceived;
					}
					this.char_input = await service.GetCharacteristicAsync(this.g_input);
					this.char_output = await service.GetCharacteristicAsync(this.g_output);
					if (this.char_input != null && this.char_output != null)
					{
						this.char_input.ValueUpdated += this.OnDataReceived;
						try
						{
							await this.char_input.StartUpdatesAsync(default(CancellationToken));
						}
						catch (Exception)
						{
						}
						return true;
					}
				}
				service = null;
			}
			return false;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x001AE308 File Offset: 0x001AC508
		private void OnDataReceived(object sender, CharacteristicUpdatedEventArgs e)
		{
			byte[] value = e.Characteristic.Value;
			this.buffer.Enqueue(value);
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x001AE330 File Offset: 0x001AC530
		public async void Disconect()
		{
			try
			{
				if (this.device != null)
				{
					if (this.char_input != null)
					{
						try
						{
							await this.char_input.StopUpdatesAsync(default(CancellationToken));
						}
						catch (Exception)
						{
						}
						this.char_input.ValueUpdated -= this.OnDataReceived;
					}
					await this.adapter.DisconnectDeviceAsync(this.device);
					this.device = null;
				}
			}
			catch
			{
			}
			finally
			{
				this.device = null;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x00002076 File Offset: 0x00000276
		public bool NeedFlush
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x001AE368 File Offset: 0x001AC568
		public async Task FlushAsync()
		{
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x001AE3A4 File Offset: 0x001AC5A4
		public async ValueTask<byte[]> ReadBytesAsync()
		{
			if (this.device == null)
			{
				throw new GeneralWriteException("BTLE device not connected!");
			}
			if (this.device.State != 2)
			{
				throw new GeneralWriteException("BTLE device not connected!");
			}
			byte[] array;
			byte[] array2;
			if (this.buffer.TryDequeue(out array))
			{
				array2 = array;
			}
			else
			{
				array2 = EmptyArrays.EmptyByteArray;
			}
			return array2;
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x001AE3E8 File Offset: 0x001AC5E8
		public async Task WriteBytesAsync(byte[] data)
		{
			if (data.Length > 17)
			{
				for (int i = 0; i < data.Length; i += 17)
				{
					int num = data.Length - i;
					if (num > 17)
					{
						num = 17;
					}
					byte[] array = new byte[num];
					Array.Copy(data, i, array, 0, num);
					await this.WriteBytesAsync(array);
				}
			}
			else if (this.device != null)
			{
				if (this.device.State != 2)
				{
					throw new GeneralWriteException("BTLE device not connected!");
				}
				await this.char_output.WriteAsync(data, default(CancellationToken));
			}
		}

		// Token: 0x040010F6 RID: 4342
		private BTLEConnection.State CurrentState = BTLEConnection.State.None;

		// Token: 0x040010F7 RID: 4343
		protected Guid g_device;

		// Token: 0x040010F8 RID: 4344
		protected Guid g_service;

		// Token: 0x040010F9 RID: 4345
		protected Guid g_input;

		// Token: 0x040010FA RID: 4346
		protected Guid g_output;

		// Token: 0x040010FB RID: 4347
		private IDevice device;

		// Token: 0x040010FC RID: 4348
		private IAdapter adapter;

		// Token: 0x040010FD RID: 4349
		private ICharacteristic char_input;

		// Token: 0x040010FE RID: 4350
		private ICharacteristic char_output;

		// Token: 0x040010FF RID: 4351
		[CompilerGenerated]
		private bool <KeepAlive>k__BackingField;

		// Token: 0x04001100 RID: 4352
		[CompilerGenerated]
		private bool <NoDelay>k__BackingField;

		// Token: 0x04001101 RID: 4353
		private ConcurrentQueue<byte[]> buffer = new ConcurrentQueue<byte[]>();

		// Token: 0x020002E4 RID: 740
		private enum State
		{
			// Token: 0x04001103 RID: 4355
			Reading,
			// Token: 0x04001104 RID: 4356
			Writing,
			// Token: 0x04001105 RID: 4357
			None
		}

		// Token: 0x020002E5 RID: 741
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ConnectAsync>d__21 : IAsyncStateMachine
		{
			// Token: 0x06002345 RID: 9029 RVA: 0x001AE434 File Offset: 0x001AC634
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEConnection btleconnection = this;
				bool flag;
				try
				{
					TaskAwaiter<Task> taskAwaiter3;
					if (num != 0)
					{
						btleconnection.CurrentState = BTLEConnection.State.None;
						btleconnection.buffer.Clear();
						if (!Guid.TryParse(device_guid, out btleconnection.g_device))
						{
							flag = false;
							goto IL_012B;
						}
						tasks = new Task[2];
						tasks[0] = btleconnection.EstablishConnectionAsync();
						tasks[1] = Task.Delay(TimeSpan.FromSeconds(5.0));
						taskAwaiter3 = Task.WhenAny(tasks).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, BTLEConnection.<ConnectAsync>d__21>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Task>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult() == tasks[1])
					{
						flag = false;
					}
					else
					{
						Task<bool> task = (Task<bool>)tasks[0];
						if (task.IsFaulted || task.IsCanceled)
						{
							flag = false;
						}
						else
						{
							flag = task.Result;
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_012B:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002346 RID: 9030 RVA: 0x001AE59C File Offset: 0x001AC79C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001106 RID: 4358
			public int <>1__state;

			// Token: 0x04001107 RID: 4359
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001108 RID: 4360
			public BTLEConnection <>4__this;

			// Token: 0x04001109 RID: 4361
			public string device_guid;

			// Token: 0x0400110A RID: 4362
			private Task[] <tasks>5__2;

			// Token: 0x0400110B RID: 4363
			private TaskAwaiter<Task> <>u__1;
		}

		// Token: 0x020002E6 RID: 742
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Disconect>d__25 : IAsyncStateMachine
		{
			// Token: 0x06002347 RID: 9031 RVA: 0x001AE5AC File Offset: 0x001AC7AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEConnection btleconnection = this;
				try
				{
					try
					{
						TaskAwaiter taskAwaiter2;
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							if (num == 1)
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0118;
							}
							if (btleconnection.device == null)
							{
								goto IL_0126;
							}
							if (btleconnection.char_input == null)
							{
								goto IL_00BC;
							}
						}
						try
						{
							if (num != 0)
							{
								taskAwaiter = btleconnection.char_input.StopUpdatesAsync(default(CancellationToken)).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 0);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEConnection.<Disconect>d__25>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter.GetResult();
						}
						catch (Exception)
						{
						}
						btleconnection.char_input.ValueUpdated -= btleconnection.OnDataReceived;
						IL_00BC:
						taskAwaiter = btleconnection.adapter.DisconnectDeviceAsync(btleconnection.device).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEConnection.<Disconect>d__25>(ref taskAwaiter, ref this);
							return;
						}
						IL_0118:
						taskAwaiter.GetResult();
						btleconnection.device = null;
						IL_0126:;
					}
					catch
					{
					}
					finally
					{
						if (num < 0)
						{
							btleconnection.device = null;
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
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002348 RID: 9032 RVA: 0x001AE784 File Offset: 0x001AC984
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400110C RID: 4364
			public int <>1__state;

			// Token: 0x0400110D RID: 4365
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400110E RID: 4366
			public BTLEConnection <>4__this;

			// Token: 0x0400110F RID: 4367
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020002E7 RID: 743
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <EstablishConnectionAsync>d__22 : IAsyncStateMachine
		{
			// Token: 0x06002349 RID: 9033 RVA: 0x001AE794 File Offset: 0x001AC994
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEConnection btleconnection = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<IDevice> taskAwaiter3;
					IDevice device;
					TaskAwaiter<IService> taskAwaiter5;
					TaskAwaiter<ICharacteristic> taskAwaiter7;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
						IL_00AE:
						try
						{
							if (num != 1)
							{
								ConnectParameters connectParameters;
								connectParameters..ctor(false, true);
								taskAwaiter3 = btleconnection.adapter.ConnectToKnownDeviceAsync(btleconnection.g_device, connectParameters, default(CancellationToken)).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 1);
									TaskAwaiter<IDevice> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IDevice>, BTLEConnection.<EstablishConnectionAsync>d__22>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter<IDevice> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<IDevice>);
								num = (num2 = -1);
							}
							device = taskAwaiter3.GetResult();
							btleconnection.device = device;
						}
						catch (Exception)
						{
						}
						goto IL_0140;
					case 2:
					{
						TaskAwaiter<IDevice> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<IDevice>);
						num = (num2 = -1);
						goto IL_01C0;
					}
					case 3:
					{
						TaskAwaiter<IService> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<IService>);
						num = (num2 = -1);
						goto IL_0248;
					}
					case 4:
					{
						TaskAwaiter<ICharacteristic> taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter<ICharacteristic>);
						num = (num2 = -1);
						goto IL_02E5;
					}
					case 5:
					{
						TaskAwaiter<ICharacteristic> taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter<ICharacteristic>);
						num = (num2 = -1);
						goto IL_0358;
					}
					case 6:
						IL_0396:
						try
						{
							if (num != 6)
							{
								taskAwaiter = btleconnection.char_input.StartUpdatesAsync(default(CancellationToken)).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 6);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEConnection.<EstablishConnectionAsync>d__22>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter.GetResult();
						}
						catch (Exception)
						{
						}
						flag = true;
						goto IL_042F;
					default:
						btleconnection.CurrentState = BTLEConnection.State.None;
						if (!btleconnection.adapter.IsScanning)
						{
							goto IL_00A4;
						}
						taskAwaiter = btleconnection.adapter.StopScanningForDevicesAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEConnection.<EstablishConnectionAsync>d__22>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_00A4:
					if (PlatformHelper.IsAndroid)
					{
						goto IL_00AE;
					}
					IL_0140:
					if (!PlatformHelper.IsiOS)
					{
						goto IL_01D1;
					}
					taskAwaiter3 = btleconnection.adapter.ConnectToKnownDeviceAsync(btleconnection.g_device, default(ConnectParameters), default(CancellationToken)).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter<IDevice> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IDevice>, BTLEConnection.<EstablishConnectionAsync>d__22>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01C0:
					device = taskAwaiter3.GetResult();
					btleconnection.device = device;
					IL_01D1:
					if (btleconnection.device == null)
					{
						goto IL_0412;
					}
					taskAwaiter5 = btleconnection.device.GetServiceAsync(btleconnection.g_service, default(CancellationToken)).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num = (num2 = 3);
						TaskAwaiter<IService> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IService>, BTLEConnection.<EstablishConnectionAsync>d__22>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0248:
					IService result = taskAwaiter5.GetResult();
					service = result;
					if (service == null)
					{
						goto IL_040B;
					}
					if (btleconnection.char_input != null)
					{
						btleconnection.char_input.ValueUpdated -= btleconnection.OnDataReceived;
					}
					taskAwaiter7 = service.GetCharacteristicAsync(btleconnection.g_input).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 4);
						TaskAwaiter<ICharacteristic> taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ICharacteristic>, BTLEConnection.<EstablishConnectionAsync>d__22>(ref taskAwaiter7, ref this);
						return;
					}
					IL_02E5:
					ICharacteristic characteristic = taskAwaiter7.GetResult();
					btleconnection.char_input = characteristic;
					taskAwaiter7 = service.GetCharacteristicAsync(btleconnection.g_output).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 5);
						TaskAwaiter<ICharacteristic> taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ICharacteristic>, BTLEConnection.<EstablishConnectionAsync>d__22>(ref taskAwaiter7, ref this);
						return;
					}
					IL_0358:
					characteristic = taskAwaiter7.GetResult();
					btleconnection.char_output = characteristic;
					if (btleconnection.char_input != null && btleconnection.char_output != null)
					{
						btleconnection.char_input.ValueUpdated += btleconnection.OnDataReceived;
						goto IL_0396;
					}
					IL_040B:
					service = null;
					IL_0412:
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_042F:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600234A RID: 9034 RVA: 0x001AEC30 File Offset: 0x001ACE30
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001110 RID: 4368
			public int <>1__state;

			// Token: 0x04001111 RID: 4369
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001112 RID: 4370
			public BTLEConnection <>4__this;

			// Token: 0x04001113 RID: 4371
			private TaskAwaiter <>u__1;

			// Token: 0x04001114 RID: 4372
			private TaskAwaiter<IDevice> <>u__2;

			// Token: 0x04001115 RID: 4373
			private IService <service>5__2;

			// Token: 0x04001116 RID: 4374
			private TaskAwaiter<IService> <>u__3;

			// Token: 0x04001117 RID: 4375
			private TaskAwaiter<ICharacteristic> <>u__4;
		}

		// Token: 0x020002E8 RID: 744
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <FlushAsync>d__28 : IAsyncStateMachine
		{
			// Token: 0x0600234B RID: 9035 RVA: 0x001AEC40 File Offset: 0x001ACE40
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600234C RID: 9036 RVA: 0x001AEC8C File Offset: 0x001ACE8C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001118 RID: 4376
			public int <>1__state;

			// Token: 0x04001119 RID: 4377
			public AsyncTaskMethodBuilder <>t__builder;
		}

		// Token: 0x020002E9 RID: 745
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadBytesAsync>d__29 : IAsyncStateMachine
		{
			// Token: 0x0600234D RID: 9037 RVA: 0x001AEC9C File Offset: 0x001ACE9C
			void IAsyncStateMachine.MoveNext()
			{
				BTLEConnection btleconnection = this;
				byte[] array2;
				try
				{
					if (btleconnection.device == null)
					{
						throw new GeneralWriteException("BTLE device not connected!");
					}
					if (btleconnection.device.State != 2)
					{
						throw new GeneralWriteException("BTLE device not connected!");
					}
					byte[] array;
					if (btleconnection.buffer.TryDequeue(out array))
					{
						array2 = array;
					}
					else
					{
						array2 = EmptyArrays.EmptyByteArray;
					}
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult(array2);
			}

			// Token: 0x0600234E RID: 9038 RVA: 0x001AED34 File Offset: 0x001ACF34
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400111A RID: 4378
			public int <>1__state;

			// Token: 0x0400111B RID: 4379
			public AsyncValueTaskMethodBuilder<byte[]> <>t__builder;

			// Token: 0x0400111C RID: 4380
			public BTLEConnection <>4__this;
		}

		// Token: 0x020002EA RID: 746
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteBytesAsync>d__30 : IAsyncStateMachine
		{
			// Token: 0x0600234F RID: 9039 RVA: 0x001AED44 File Offset: 0x001ACF44
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEConnection btleconnection = this;
				try
				{
					if (num != 0)
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 1)
						{
							if (data.Length > 17)
							{
								i = 0;
								goto IL_00D7;
							}
							if (btleconnection.device == null)
							{
								goto IL_0184;
							}
							if (btleconnection.device.State != 2)
							{
								throw new GeneralWriteException("BTLE device not connected!");
							}
							taskAwaiter = btleconnection.char_output.WriteAsync(data, default(CancellationToken)).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, BTLEConnection.<WriteBytesAsync>d__30>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_0184;
					}
					TaskAwaiter taskAwaiter4;
					TaskAwaiter taskAwaiter3 = taskAwaiter4;
					taskAwaiter4 = default(TaskAwaiter);
					num2 = -1;
					IL_00C1:
					taskAwaiter3.GetResult();
					i += 17;
					IL_00D7:
					if (i < data.Length)
					{
						int num3 = data.Length - i;
						if (num3 > 17)
						{
							num3 = 17;
						}
						byte[] array = new byte[num3];
						Array.Copy(data, i, array, 0, num3);
						taskAwaiter3 = btleconnection.WriteBytesAsync(array).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEConnection.<WriteBytesAsync>d__30>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_00C1;
					}
					IL_0184:;
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

			// Token: 0x06002350 RID: 9040 RVA: 0x001AEF20 File Offset: 0x001AD120
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400111D RID: 4381
			public int <>1__state;

			// Token: 0x0400111E RID: 4382
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400111F RID: 4383
			public byte[] data;

			// Token: 0x04001120 RID: 4384
			public BTLEConnection <>4__this;

			// Token: 0x04001121 RID: 4385
			private int <i>5__2;

			// Token: 0x04001122 RID: 4386
			private TaskAwaiter <>u__1;

			// Token: 0x04001123 RID: 4387
			private TaskAwaiter<bool> <>u__2;
		}
	}
}
