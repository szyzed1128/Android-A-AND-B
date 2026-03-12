using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Bluetooth2;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000300 RID: 768
	public class ConnectionDetector
	{
		// Token: 0x060023DC RID: 9180 RVA: 0x001BC43A File Offset: 0x001BA63A
		public ConnectionDetector()
		{
			this.FinishedWithSuccess += this.ConnectionDetector_FinishedWithSuccess;
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x001BC454 File Offset: 0x001BA654
		private void ConnectionDetector_FinishedWithSuccess(object sender, EventArgs e)
		{
			this.Found = true;
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x001BC460 File Offset: 0x001BA660
		public async Task DetectAndSetSettings()
		{
			this.Found = false;
			List<Task> list = new List<Task>();
			list.Add(this.CheckAndSetWiFi());
			if (PlatformHelper.IsiOS)
			{
				list.Add(this.CheckAndSetBluetoothLE());
			}
			if (PlatformHelper.IsAndroid)
			{
				list.Add(this.CheckAndSetBluetooth());
			}
			await Task.WhenAll(list);
			if (PlatformHelper.IsAndroid && !this.Found)
			{
				await this.CheckAndSetBluetoothLE();
			}
			if (PlatformHelper.IsiOS && !this.Found)
			{
				await this.CheckAndSetBluetooth();
			}
			if (!this.Found)
			{
				EventHandler nothingFound = this.NothingFound;
				if (nothingFound != null)
				{
					nothingFound(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060023DF RID: 9183 RVA: 0x001BC4A4 File Offset: 0x001BA6A4
		// (remove) Token: 0x060023E0 RID: 9184 RVA: 0x001BC4DC File Offset: 0x001BA6DC
		public event EventHandler NothingFound
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.NothingFound;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.NothingFound, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.NothingFound;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.NothingFound, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060023E1 RID: 9185 RVA: 0x001BC514 File Offset: 0x001BA714
		// (remove) Token: 0x060023E2 RID: 9186 RVA: 0x001BC54C File Offset: 0x001BA74C
		public event EventHandler FinishedWithSuccess
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.FinishedWithSuccess;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.FinishedWithSuccess, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.FinishedWithSuccess;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.FinishedWithSuccess, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x001BC584 File Offset: 0x001BA784
		private async Task CheckAndSetWiFi()
		{
			string[] array = new string[] { "192.168.0.10:35000", "192.168.1.10:35000" };
			for (int i = 0; i < array.Length; i++)
			{
				string server = array[i];
				Task.Run(() => this.CheckWifiAddress(server));
			}
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x001BC5C8 File Offset: 0x001BA7C8
		private async Task CheckWifiAddress(string host_port)
		{
			TaskAwaiter<bool> taskAwaiter = new ConnectionSettingsTester().CheckConnection(ConnectionTypes.WiFi, host_port).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult() && !this.Found)
			{
				string[] items = host_port.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				MainThreadHelper.InvokeOnMainThread(delegate
				{
					SharedSettings.Current.WiFiServer = items[0];
					SharedSettings.Current.WiFiPort = items[1];
					SharedSettings.Current.ConnectionType = ConnectionTypes.WiFi;
					EventHandler finishedWithSuccess = this.FinishedWithSuccess;
					if (finishedWithSuccess == null)
					{
						return;
					}
					finishedWithSuccess(this, EventArgs.Empty);
				});
			}
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x001BC614 File Offset: 0x001BA814
		private async Task CheckAndSetBluetoothLE()
		{
			if (PlatformHelper.IsAndroid)
			{
				if (PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
				{
					TaskAwaiter<PermissionStatus> taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
					}
					if (taskAwaiter.GetResult() != 3)
					{
						return;
					}
				}
				else
				{
					TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
					}
					if (taskAwaiter.GetResult() != 3)
					{
						return;
					}
				}
			}
			BTLEDeviceSelectorViewModel btle = new BTLEDeviceSelectorViewModel();
			btle.DiscoverDevices.Execute(null);
			await Task.Delay(2000);
			List<IDevice> temp_devices = this.FilterBTLEDevices(btle.DeviceList);
			btle.DiscoverDevices.Execute(null);
			await Task.Delay(4000);
			temp_devices.AddRange(this.FilterBTLEDevices(btle.DeviceList));
			(from x in temp_devices
				group x by x.Id into y
				select y.First<IDevice>()).ToList<IDevice>();
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x001BC657 File Offset: 0x001BA857
		private List<IDevice> FilterBTLEDevices(IEnumerable<IDevice> devices)
		{
			return devices.Where((IDevice x) => !string.IsNullOrEmpty(x.Name)).ToList<IDevice>();
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x001BC684 File Offset: 0x001BA884
		private async Task CheckAndSetBluetooth()
		{
			try
			{
				if (PlatformHelper.IsAndroid && PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
				{
					TaskAwaiter<PermissionStatus> taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
					}
					if (taskAwaiter.GetResult() != 3)
					{
						return;
					}
				}
				BTDeviceSelectorViewModel bt = new BTDeviceSelectorViewModel();
				bt.DiscoverDevices.Execute(null);
				await Task.Delay(100);
				if (bt.DeviceList.Count == 0)
				{
					await Task.Delay(2000);
					bt.DiscoverDevices.Execute(null);
					await Task.Delay(500);
				}
				List<IBluetooth2Device> list = this.FilterBTDevices(bt.DeviceList);
				if (list != null && list.Count == 1)
				{
					IBluetooth2Device onlyDevice = list[0];
					if (onlyDevice != null)
					{
						MainThreadHelper.InvokeOnMainThread(delegate
						{
							SharedSettings.Current.BTDeviceID = onlyDevice.Id;
							SharedSettings.Current.BTDeviceName = onlyDevice.Name;
						});
					}
				}
				bt.StopDiscoveringDevices.Execute(null);
				bt = null;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x001BC6C8 File Offset: 0x001BA8C8
		private async Task CheckBluetoothDevice(IBluetooth2Device device)
		{
			ConnectionSettingsTester tester = new ConnectionSettingsTester();
			bool flag = false;
			if (PlatformHelper.IsAndroid)
			{
				flag = await tester.CheckConnection(ConnectionTypes.Bluetooth, device.Id);
			}
			if (PlatformHelper.IsiOS)
			{
				flag = await tester.CheckConnection(ConnectionTypes.MFI_OBDLinkMXPlus, device.Id);
			}
			if (flag && !this.Found)
			{
				MainThreadHelper.InvokeOnMainThread(delegate
				{
					SharedSettings.Current.BTDeviceID = device.Id;
					SharedSettings.Current.BTDeviceName = device.Name;
					if (PlatformHelper.IsAndroid)
					{
						SharedSettings.Current.ConnectionType = ConnectionTypes.Bluetooth;
					}
					else if (PlatformHelper.IsiOS)
					{
						SharedSettings.Current.ConnectionType = ConnectionTypes.MFI_OBDLinkMXPlus;
					}
					EventHandler finishedWithSuccess = this.FinishedWithSuccess;
					if (finishedWithSuccess == null)
					{
						return;
					}
					finishedWithSuccess(this, EventArgs.Empty);
				});
			}
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x001BC714 File Offset: 0x001BA914
		private List<IBluetooth2Device> FilterBTDevices(IEnumerable<IBluetooth2Device> deviceList)
		{
			if (PlatformHelper.IsiOS)
			{
				return deviceList.ToList<IBluetooth2Device>();
			}
			if (PlatformHelper.IsAndroid)
			{
				return deviceList.Where((IBluetooth2Device x) => x.IsValid).ToList<IBluetooth2Device>();
			}
			throw new NotImplementedException("ConnectionDetector.FilterBTDevices->Platform=" + Device.RuntimePlatform);
		}

		// Token: 0x04001183 RID: 4483
		private volatile bool Found;

		// Token: 0x04001184 RID: 4484
		[CompilerGenerated]
		private EventHandler NothingFound;

		// Token: 0x04001185 RID: 4485
		[CompilerGenerated]
		private EventHandler FinishedWithSuccess;

		// Token: 0x02000301 RID: 769
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060023EA RID: 9194 RVA: 0x001BC775 File Offset: 0x001BA975
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060023EB RID: 9195 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060023EC RID: 9196 RVA: 0x001BC781 File Offset: 0x001BA981
			internal Guid <CheckAndSetBluetoothLE>b__12_0(IDevice x)
			{
				return x.Id;
			}

			// Token: 0x060023ED RID: 9197 RVA: 0x001BC789 File Offset: 0x001BA989
			internal IDevice <CheckAndSetBluetoothLE>b__12_1(IGrouping<Guid, IDevice> y)
			{
				return y.First<IDevice>();
			}

			// Token: 0x060023EE RID: 9198 RVA: 0x001BC791 File Offset: 0x001BA991
			internal bool <FilterBTLEDevices>b__13_0(IDevice x)
			{
				return !string.IsNullOrEmpty(x.Name);
			}

			// Token: 0x060023EF RID: 9199 RVA: 0x001BC7A1 File Offset: 0x001BA9A1
			internal bool <FilterBTDevices>b__16_0(IBluetooth2Device x)
			{
				return x.IsValid;
			}

			// Token: 0x04001186 RID: 4486
			public static readonly ConnectionDetector.<>c <>9 = new ConnectionDetector.<>c();

			// Token: 0x04001187 RID: 4487
			public static Func<IDevice, Guid> <>9__12_0;

			// Token: 0x04001188 RID: 4488
			public static Func<IGrouping<Guid, IDevice>, IDevice> <>9__12_1;

			// Token: 0x04001189 RID: 4489
			public static Func<IDevice, bool> <>9__13_0;

			// Token: 0x0400118A RID: 4490
			public static Func<IBluetooth2Device, bool> <>9__16_0;
		}

		// Token: 0x02000302 RID: 770
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_0
		{
			// Token: 0x060023F0 RID: 9200 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_0()
			{
			}

			// Token: 0x060023F1 RID: 9201 RVA: 0x001BC7A9 File Offset: 0x001BA9A9
			internal Task <CheckAndSetWiFi>b__0()
			{
				return this.<>4__this.CheckWifiAddress(this.server);
			}

			// Token: 0x0400118B RID: 4491
			public string server;

			// Token: 0x0400118C RID: 4492
			public ConnectionDetector <>4__this;
		}

		// Token: 0x02000303 RID: 771
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x060023F2 RID: 9202 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x060023F3 RID: 9203 RVA: 0x001BC7BC File Offset: 0x001BA9BC
			internal void <CheckWifiAddress>b__0()
			{
				SharedSettings.Current.WiFiServer = this.items[0];
				SharedSettings.Current.WiFiPort = this.items[1];
				SharedSettings.Current.ConnectionType = ConnectionTypes.WiFi;
				EventHandler finishedWithSuccess = this.<>4__this.FinishedWithSuccess;
				if (finishedWithSuccess == null)
				{
					return;
				}
				finishedWithSuccess(this.<>4__this, EventArgs.Empty);
			}

			// Token: 0x0400118D RID: 4493
			public ConnectionDetector <>4__this;

			// Token: 0x0400118E RID: 4494
			public string[] items;
		}

		// Token: 0x02000304 RID: 772
		[CompilerGenerated]
		private sealed class <>c__DisplayClass14_0
		{
			// Token: 0x060023F4 RID: 9204 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass14_0()
			{
			}

			// Token: 0x060023F5 RID: 9205 RVA: 0x001BC818 File Offset: 0x001BAA18
			internal void <CheckAndSetBluetooth>b__0()
			{
				SharedSettings.Current.BTDeviceID = this.onlyDevice.Id;
				SharedSettings.Current.BTDeviceName = this.onlyDevice.Name;
			}

			// Token: 0x0400118F RID: 4495
			public IBluetooth2Device onlyDevice;
		}

		// Token: 0x02000305 RID: 773
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			// Token: 0x060023F6 RID: 9206 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_0()
			{
			}

			// Token: 0x060023F7 RID: 9207 RVA: 0x001BC844 File Offset: 0x001BAA44
			internal void <CheckBluetoothDevice>b__0()
			{
				SharedSettings.Current.BTDeviceID = this.device.Id;
				SharedSettings.Current.BTDeviceName = this.device.Name;
				if (PlatformHelper.IsAndroid)
				{
					SharedSettings.Current.ConnectionType = ConnectionTypes.Bluetooth;
				}
				else if (PlatformHelper.IsiOS)
				{
					SharedSettings.Current.ConnectionType = ConnectionTypes.MFI_OBDLinkMXPlus;
				}
				EventHandler finishedWithSuccess = this.<>4__this.FinishedWithSuccess;
				if (finishedWithSuccess == null)
				{
					return;
				}
				finishedWithSuccess(this.<>4__this, EventArgs.Empty);
			}

			// Token: 0x04001190 RID: 4496
			public IBluetooth2Device device;

			// Token: 0x04001191 RID: 4497
			public ConnectionDetector <>4__this;
		}

		// Token: 0x02000306 RID: 774
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckAndSetBluetooth>d__14 : IAsyncStateMachine
		{
			// Token: 0x060023F8 RID: 9208 RVA: 0x001BC8C4 File Offset: 0x001BAAC4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ConnectionDetector connectionDetector = this;
				try
				{
					try
					{
						TaskAwaiter<PermissionStatus> taskAwaiter3;
						TaskAwaiter taskAwaiter4;
						switch (num)
						{
						case 0:
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
							num2 = -1;
							break;
						case 1:
						{
							TaskAwaiter taskAwaiter5;
							taskAwaiter4 = taskAwaiter5;
							taskAwaiter5 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0115;
						}
						case 2:
						{
							TaskAwaiter taskAwaiter5;
							taskAwaiter4 = taskAwaiter5;
							taskAwaiter5 = default(TaskAwaiter);
							num2 = -1;
							goto IL_018C;
						}
						case 3:
						{
							TaskAwaiter taskAwaiter5;
							taskAwaiter4 = taskAwaiter5;
							taskAwaiter5 = default(TaskAwaiter);
							num2 = -1;
							goto IL_01FF;
						}
						default:
							if (!PlatformHelper.IsAndroid || !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
							{
								goto IL_00A1;
							}
							taskAwaiter3 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, ConnectionDetector.<CheckAndSetBluetooth>d__14>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						}
						if (taskAwaiter3.GetResult() != 3)
						{
							goto IL_028C;
						}
						IL_00A1:
						bt = new BTDeviceSelectorViewModel();
						bt.DiscoverDevices.Execute(null);
						taskAwaiter4 = Task.Delay(100).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionDetector.<CheckAndSetBluetooth>d__14>(ref taskAwaiter4, ref this);
							return;
						}
						IL_0115:
						taskAwaiter4.GetResult();
						if (bt.DeviceList.Count != 0)
						{
							goto IL_0206;
						}
						taskAwaiter4 = Task.Delay(2000).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionDetector.<CheckAndSetBluetooth>d__14>(ref taskAwaiter4, ref this);
							return;
						}
						IL_018C:
						taskAwaiter4.GetResult();
						bt.DiscoverDevices.Execute(null);
						taskAwaiter4 = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionDetector.<CheckAndSetBluetooth>d__14>(ref taskAwaiter4, ref this);
							return;
						}
						IL_01FF:
						taskAwaiter4.GetResult();
						IL_0206:
						List<IBluetooth2Device> list = connectionDetector.FilterBTDevices(bt.DeviceList);
						if (list != null && list.Count == 1)
						{
							ConnectionDetector.<>c__DisplayClass14_0 CS$<>8__locals1 = new ConnectionDetector.<>c__DisplayClass14_0();
							CS$<>8__locals1.onlyDevice = list[0];
							if (CS$<>8__locals1.onlyDevice != null)
							{
								MainThreadHelper.InvokeOnMainThread(delegate
								{
									SharedSettings.Current.BTDeviceID = CS$<>8__locals1.onlyDevice.Id;
									SharedSettings.Current.BTDeviceName = CS$<>8__locals1.onlyDevice.Name;
								});
							}
						}
						bt.StopDiscoveringDevices.Execute(null);
						bt = null;
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_028C:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060023F9 RID: 9209 RVA: 0x001BCBA4 File Offset: 0x001BADA4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001192 RID: 4498
			public int <>1__state;

			// Token: 0x04001193 RID: 4499
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001194 RID: 4500
			public ConnectionDetector <>4__this;

			// Token: 0x04001195 RID: 4501
			private BTDeviceSelectorViewModel <bt>5__2;

			// Token: 0x04001196 RID: 4502
			private TaskAwaiter<PermissionStatus> <>u__1;

			// Token: 0x04001197 RID: 4503
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000307 RID: 775
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckAndSetBluetoothLE>d__12 : IAsyncStateMachine
		{
			// Token: 0x060023FA RID: 9210 RVA: 0x001BCBB4 File Offset: 0x001BADB4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ConnectionDetector connectionDetector = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
						break;
					case 1:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
						goto IL_00F2;
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0175;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01FC;
					}
					default:
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_0101;
						}
						if (PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
						{
							taskAwaiter3 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, ConnectionDetector.<CheckAndSetBluetoothLE>d__12>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, ConnectionDetector.<CheckAndSetBluetoothLE>d__12>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_00F2;
						}
						break;
					}
					if (taskAwaiter3.GetResult() != 3)
					{
						goto IL_029C;
					}
					goto IL_0101;
					IL_00F2:
					if (taskAwaiter3.GetResult() != 3)
					{
						goto IL_029C;
					}
					IL_0101:
					btle = new BTLEDeviceSelectorViewModel();
					btle.DiscoverDevices.Execute(null);
					taskAwaiter4 = Task.Delay(2000).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionDetector.<CheckAndSetBluetoothLE>d__12>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0175:
					taskAwaiter4.GetResult();
					temp_devices = connectionDetector.FilterBTLEDevices(btle.DeviceList);
					btle.DiscoverDevices.Execute(null);
					taskAwaiter4 = Task.Delay(4000).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionDetector.<CheckAndSetBluetoothLE>d__12>(ref taskAwaiter4, ref this);
						return;
					}
					IL_01FC:
					taskAwaiter4.GetResult();
					temp_devices.AddRange(connectionDetector.FilterBTLEDevices(btle.DeviceList));
					(from x in temp_devices
						group x by x.Id into y
						select y.First<IDevice>()).ToList<IDevice>();
				}
				catch (Exception ex)
				{
					num2 = -2;
					btle = null;
					temp_devices = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_029C:
				num2 = -2;
				btle = null;
				temp_devices = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060023FB RID: 9211 RVA: 0x001BCE9C File Offset: 0x001BB09C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001198 RID: 4504
			public int <>1__state;

			// Token: 0x04001199 RID: 4505
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400119A RID: 4506
			public ConnectionDetector <>4__this;

			// Token: 0x0400119B RID: 4507
			private BTLEDeviceSelectorViewModel <btle>5__2;

			// Token: 0x0400119C RID: 4508
			private List<IDevice> <temp_devices>5__3;

			// Token: 0x0400119D RID: 4509
			private TaskAwaiter<PermissionStatus> <>u__1;

			// Token: 0x0400119E RID: 4510
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000308 RID: 776
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckAndSetWiFi>d__10 : IAsyncStateMachine
		{
			// Token: 0x060023FC RID: 9212 RVA: 0x001BCEAC File Offset: 0x001BB0AC
			void IAsyncStateMachine.MoveNext()
			{
				ConnectionDetector connectionDetector = this;
				try
				{
					string[] array = new string[] { "192.168.0.10:35000", "192.168.1.10:35000" };
					for (int i = 0; i < array.Length; i++)
					{
						Task.Run(new Func<Task>(new ConnectionDetector.<>c__DisplayClass10_0
						{
							<>4__this = connectionDetector,
							server = array[i]
						}.<CheckAndSetWiFi>b__0));
					}
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

			// Token: 0x060023FD RID: 9213 RVA: 0x001BCF48 File Offset: 0x001BB148
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400119F RID: 4511
			public int <>1__state;

			// Token: 0x040011A0 RID: 4512
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040011A1 RID: 4513
			public ConnectionDetector <>4__this;
		}

		// Token: 0x02000309 RID: 777
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckBluetoothDevice>d__15 : IAsyncStateMachine
		{
			// Token: 0x060023FE RID: 9214 RVA: 0x001BCF58 File Offset: 0x001BB158
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ConnectionDetector connectionDetector = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					bool flag;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_013F;
						}
						CS$<>8__locals1 = new ConnectionDetector.<>c__DisplayClass15_0();
						CS$<>8__locals1.device = device;
						CS$<>8__locals1.<>4__this = this;
						tester = new ConnectionSettingsTester();
						flag = false;
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_00CE;
						}
						taskAwaiter = tester.CheckConnection(ConnectionTypes.Bluetooth, CS$<>8__locals1.device.Id).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ConnectionDetector.<CheckBluetoothDevice>d__15>(ref taskAwaiter, ref this);
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
					flag = taskAwaiter.GetResult();
					IL_00CE:
					if (!PlatformHelper.IsiOS)
					{
						goto IL_0147;
					}
					taskAwaiter = tester.CheckConnection(ConnectionTypes.MFI_OBDLinkMXPlus, CS$<>8__locals1.device.Id).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ConnectionDetector.<CheckBluetoothDevice>d__15>(ref taskAwaiter, ref this);
						return;
					}
					IL_013F:
					flag = taskAwaiter.GetResult();
					IL_0147:
					if (flag && !connectionDetector.Found)
					{
						MainThreadHelper.InvokeOnMainThread(delegate
						{
							SharedSettings.Current.BTDeviceID = CS$<>8__locals1.device.Id;
							SharedSettings.Current.BTDeviceName = CS$<>8__locals1.device.Name;
							if (PlatformHelper.IsAndroid)
							{
								SharedSettings.Current.ConnectionType = ConnectionTypes.Bluetooth;
							}
							else if (PlatformHelper.IsiOS)
							{
								SharedSettings.Current.ConnectionType = ConnectionTypes.MFI_OBDLinkMXPlus;
							}
							EventHandler finishedWithSuccess = CS$<>8__locals1.<>4__this.FinishedWithSuccess;
							if (finishedWithSuccess == null)
							{
								return;
							}
							finishedWithSuccess(CS$<>8__locals1.<>4__this, EventArgs.Empty);
						});
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					tester = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				tester = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060023FF RID: 9215 RVA: 0x001BD138 File Offset: 0x001BB338
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040011A2 RID: 4514
			public int <>1__state;

			// Token: 0x040011A3 RID: 4515
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040011A4 RID: 4516
			public IBluetooth2Device device;

			// Token: 0x040011A5 RID: 4517
			public ConnectionDetector <>4__this;

			// Token: 0x040011A6 RID: 4518
			private ConnectionDetector.<>c__DisplayClass15_0 <>8__1;

			// Token: 0x040011A7 RID: 4519
			private ConnectionSettingsTester <tester>5__2;

			// Token: 0x040011A8 RID: 4520
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200030A RID: 778
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckWifiAddress>d__11 : IAsyncStateMachine
		{
			// Token: 0x06002400 RID: 9216 RVA: 0x001BD148 File Offset: 0x001BB348
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ConnectionDetector connectionDetector = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						CS$<>8__locals1 = new ConnectionDetector.<>c__DisplayClass11_0();
						CS$<>8__locals1.<>4__this = this;
						taskAwaiter3 = new ConnectionSettingsTester().CheckConnection(ConnectionTypes.WiFi, host_port).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ConnectionDetector.<CheckWifiAddress>d__11>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult() && !connectionDetector.Found)
					{
						CS$<>8__locals1.items = host_port.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
						MainThreadHelper.InvokeOnMainThread(delegate
						{
							SharedSettings.Current.WiFiServer = CS$<>8__locals1.items[0];
							SharedSettings.Current.WiFiPort = CS$<>8__locals1.items[1];
							SharedSettings.Current.ConnectionType = ConnectionTypes.WiFi;
							EventHandler finishedWithSuccess = CS$<>8__locals1.<>4__this.FinishedWithSuccess;
							if (finishedWithSuccess == null)
							{
								return;
							}
							finishedWithSuccess(CS$<>8__locals1.<>4__this, EventArgs.Empty);
						});
					}
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
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002401 RID: 9217 RVA: 0x001BD278 File Offset: 0x001BB478
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040011A9 RID: 4521
			public int <>1__state;

			// Token: 0x040011AA RID: 4522
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040011AB RID: 4523
			public ConnectionDetector <>4__this;

			// Token: 0x040011AC RID: 4524
			public string host_port;

			// Token: 0x040011AD RID: 4525
			private ConnectionDetector.<>c__DisplayClass11_0 <>8__1;

			// Token: 0x040011AE RID: 4526
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200030B RID: 779
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DetectAndSetSettings>d__3 : IAsyncStateMachine
		{
			// Token: 0x06002402 RID: 9218 RVA: 0x001BD288 File Offset: 0x001BB488
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ConnectionDetector connectionDetector = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0121;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_018A;
					}
					default:
					{
						connectionDetector.Found = false;
						List<Task> list = new List<Task>();
						list.Add(connectionDetector.CheckAndSetWiFi());
						if (PlatformHelper.IsiOS)
						{
							list.Add(connectionDetector.CheckAndSetBluetoothLE());
						}
						if (PlatformHelper.IsAndroid)
						{
							list.Add(connectionDetector.CheckAndSetBluetooth());
						}
						taskAwaiter = Task.WhenAll(list).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionDetector.<DetectAndSetSettings>d__3>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					if (!PlatformHelper.IsAndroid || connectionDetector.Found)
					{
						goto IL_0128;
					}
					taskAwaiter = connectionDetector.CheckAndSetBluetoothLE().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionDetector.<DetectAndSetSettings>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_0121:
					taskAwaiter.GetResult();
					IL_0128:
					if (!PlatformHelper.IsiOS || connectionDetector.Found)
					{
						goto IL_0191;
					}
					taskAwaiter = connectionDetector.CheckAndSetBluetooth().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ConnectionDetector.<DetectAndSetSettings>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_018A:
					taskAwaiter.GetResult();
					IL_0191:
					if (!connectionDetector.Found)
					{
						EventHandler nothingFound = connectionDetector.NothingFound;
						if (nothingFound != null)
						{
							nothingFound(connectionDetector, EventArgs.Empty);
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

			// Token: 0x06002403 RID: 9219 RVA: 0x001BD494 File Offset: 0x001BB694
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040011AF RID: 4527
			public int <>1__state;

			// Token: 0x040011B0 RID: 4528
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040011B1 RID: 4529
			public ConnectionDetector <>4__this;

			// Token: 0x040011B2 RID: 4530
			private TaskAwaiter <>u__1;
		}
	}
}
