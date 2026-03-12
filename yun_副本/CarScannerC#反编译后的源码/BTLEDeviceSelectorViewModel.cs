using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x020001A6 RID: 422
	public class BTLEDeviceSelectorViewModel : IBTLEDeviceSelectorViewModel, INotifyPropertyChanged
	{
		// Token: 0x060016D1 RID: 5841 RVA: 0x000A9128 File Offset: 0x000A7328
		public BTLEDeviceSelectorViewModel()
		{
			this.ble = CrossBluetoothLE.Current;
			this.ble.StateChanged += this.OnStateChanged;
			this.adapter = CrossBluetoothLE.Current.Adapter;
			this.adapter.DeviceDiscovered += this.OnDeviceDiscovered;
			this.adapter.ScanTimeoutElapsed += this.Adapter_ScanTimeoutElapsed;
			this.ErrorMessage = "";
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x000A91C7 File Offset: 0x000A73C7
		private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
		{
			this.OnPropertyChanged("IsScanning");
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060016D3 RID: 5843 RVA: 0x000A91D4 File Offset: 0x000A73D4
		// (remove) Token: 0x060016D4 RID: 5844 RVA: 0x000A920C File Offset: 0x000A740C
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x000A9244 File Offset: 0x000A7444
		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x060016D6 RID: 5846 RVA: 0x000A9268 File Offset: 0x000A7468
		public ObservableCollection<IDevice> DeviceList
		{
			get
			{
				return this._DeviceList;
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x060016D7 RID: 5847 RVA: 0x000A9270 File Offset: 0x000A7470
		public BluetoothState CurrentState
		{
			get
			{
				return this.ble.State;
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x000A927D File Offset: 0x000A747D
		public bool IsBluetoothOn
		{
			get
			{
				return this.ble.State == 4;
			}
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x000A9290 File Offset: 0x000A7490
		public async Task StopDiscovering()
		{
			if (this.ble.Adapter.IsScanning)
			{
				await this.ble.Adapter.StopScanningForDevicesAsync();
			}
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x000A92D4 File Offset: 0x000A74D4
		private void OnStateChanged(object sender, BluetoothStateChangedArgs e)
		{
			if (e.OldState != 4 && e.OldState != null && e.NewState == 4 && this.DeviceList.Count == 0)
			{
				this.DiscoverDevices.Execute(null);
			}
			this.OnPropertyChanged("CurrentState");
			this.OnPropertyChanged("IsBluetoothOn");
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x000A932A File Offset: 0x000A752A
		private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
		{
			BTLEDeviceSelectorViewModel.<>c__DisplayClass17_0 CS$<>8__locals1 = new BTLEDeviceSelectorViewModel.<>c__DisplayClass17_0();
			CS$<>8__locals1.e = e;
			CS$<>8__locals1.<>4__this = this;
			Device.BeginInvokeOnMainThread(delegate
			{
				BTLEDeviceSelectorViewModel.<>c__DisplayClass17_0.<<OnDeviceDiscovered>b__0>d <<OnDeviceDiscovered>b__0>d;
				<<OnDeviceDiscovered>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<OnDeviceDiscovered>b__0>d.<>4__this = CS$<>8__locals1;
				<<OnDeviceDiscovered>b__0>d.<>1__state = -1;
				<<OnDeviceDiscovered>b__0>d.<>t__builder.Start<BTLEDeviceSelectorViewModel.<>c__DisplayClass17_0.<<OnDeviceDiscovered>b__0>d>(ref <<OnDeviceDiscovered>b__0>d);
			});
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x000A934F File Offset: 0x000A754F
		public bool IsScanning
		{
			get
			{
				return this.IsBluetoothOn && this.adapter.IsScanning;
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x000A9366 File Offset: 0x000A7566
		public ICommand DiscoverDevices
		{
			get
			{
				return new Command(async delegate
				{
					this.DeviceList.Clear();
					if (PlatformHelper.IsAndroid && PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
					{
						TaskAwaiter<PermissionStatus> taskAwaiter3 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							await taskAwaiter3;
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						}
						if (taskAwaiter3.GetResult() != 3)
						{
							return;
						}
					}
					if (this.adapter.IsScanning)
					{
						await this.adapter.StopScanningForDevicesAsync();
						this.OnPropertyChanged("IsScanning");
						await Task.Delay(1000);
					}
					this.adapter.StartScanningForDevicesAsync(null, null, false, default(CancellationToken));
					this.OnPropertyChanged("IsScanning");
				});
			}
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x000A9379 File Offset: 0x000A7579
		// (set) Token: 0x060016DF RID: 5855 RVA: 0x000A9381 File Offset: 0x000A7581
		public bool IsTestingDevice
		{
			get
			{
				return this._IsTestingDevice;
			}
			set
			{
				if (value != this._IsTestingDevice)
				{
					this._IsTestingDevice = value;
					this.OnPropertyChanged("IsTestingDevice");
				}
			}
		}

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x000A939E File Offset: 0x000A759E
		// (set) Token: 0x060016E1 RID: 5857 RVA: 0x000A93A6 File Offset: 0x000A75A6
		public string TestingDeviceString
		{
			get
			{
				return this._TestingDeviceString;
			}
			set
			{
				if (value != this._TestingDeviceString)
				{
					this._TestingDeviceString = value;
					this.OnPropertyChanged("TestingDeviceString");
				}
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x000A93C8 File Offset: 0x000A75C8
		// (set) Token: 0x060016E3 RID: 5859 RVA: 0x000A93D0 File Offset: 0x000A75D0
		public string ErrorMessage
		{
			[CompilerGenerated]
			get
			{
				return this.<ErrorMessage>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ErrorMessage>k__BackingField = value;
			}
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x000A93DC File Offset: 0x000A75DC
		public async Task<bool> TestDevice(IDevice device)
		{
			await this.StopDiscovering();
			this.IsTestingDevice = true;
			this.deviceForTest = new BTLEDeviceDescription();
			this.TestingDeviceString = Translate.GetString("ios_BTLE_CheckingDevice");
			try
			{
				Task task = Task.Delay(TimeSpan.FromSeconds(10.0));
				Task connect_task = null;
				if (PlatformHelper.IsAndroid)
				{
					ConnectParameters connectParameters;
					connectParameters..ctor(false, true);
					connect_task = this.adapter.ConnectToDeviceAsync(device, connectParameters, default(CancellationToken));
				}
				if (PlatformHelper.IsiOS)
				{
					connect_task = this.adapter.ConnectToDeviceAsync(device, default(ConnectParameters), default(CancellationToken));
				}
				Task task2 = await Task.WhenAny(new Task[] { task, connect_task });
				if (task2 != connect_task || task2.IsFaulted)
				{
					this.ErrorMessage = "Connection failed";
					this.IsTestingDevice = false;
					return false;
				}
				if (PlatformHelper.IsAndroid)
				{
					await Task.Delay(TimeSpan.FromSeconds(1.0));
				}
				connect_task = null;
			}
			catch (Exception)
			{
				this.ErrorMessage = "Connection failed";
			}
			if (device.State == 2)
			{
				this.deviceForTest.NamePart = device.Name;
				Task task3 = Task.Delay(TimeSpan.FromSeconds(1.0));
				Task connect_task = device.GetServicesAsync(default(CancellationToken));
				Task task4 = await Task.WhenAny(new Task[] { task3, connect_task });
				if (task4 == connect_task && !task4.IsFaulted)
				{
					IReadOnlyList<IService> services = ((Task<IReadOnlyList<IService>>)connect_task).Result;
					for (int i = 0; i < services.Count; i++)
					{
						this.TestingDeviceString = string.Format(Translate.GetString("ios_BTLE_CheckingDeviceStep2"), (i + 1).ToString(), services.Count.ToString());
						TaskAwaiter<bool> taskAwaiter = this.CheckService(services[i]).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (taskAwaiter.GetResult())
						{
							this.deviceForTest.ServiceID = services[i].Id.ToString();
							return true;
						}
					}
					services = null;
				}
				connect_task = null;
			}
			this.IsTestingDevice = false;
			return false;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x000A9428 File Offset: 0x000A7628
		public async Task<bool> CheckService(IService service)
		{
			await this.StopDiscovering();
			Task task = Task.Delay(TimeSpan.FromSeconds(4.0));
			Task service_task = service.GetCharacteristicsAsync();
			Task task2 = await Task.WhenAny(new Task[] { task, service_task });
			if (task2 == service_task && !task2.IsFaulted)
			{
				IReadOnlyList<ICharacteristic> result = ((Task<IReadOnlyList<ICharacteristic>>)service_task).Result;
				if (result.Count >= 1)
				{
					ICharacteristic[] array = result.Where((ICharacteristic x) => x.CanUpdate).ToArray<ICharacteristic>();
					ICharacteristic[] array2 = result.Where((ICharacteristic x) => x.CanWrite).ToArray<ICharacteristic>();
					if (array.Length == 0 || array2.Length == 0)
					{
						return false;
					}
					List<KeyValuePair<ICharacteristic, ICharacteristic>> list = new List<KeyValuePair<ICharacteristic, ICharacteristic>>();
					foreach (ICharacteristic characteristic in array)
					{
						for (int j = 0; j < array2.Length; j++)
						{
							KeyValuePair<ICharacteristic, ICharacteristic> keyValuePair = new KeyValuePair<ICharacteristic, ICharacteristic>(characteristic, array2[j]);
							list.Add(keyValuePair);
						}
					}
					foreach (KeyValuePair<ICharacteristic, ICharacteristic> keyValuePair2 in list)
					{
						ICharacteristic inputchar = keyValuePair2.Key;
						ICharacteristic outputchar = keyValuePair2.Value;
						try
						{
							bool flag = await this.CheckCharacteristics(inputchar, outputchar);
							if (flag)
							{
								this.deviceForTest.InputID = inputchar.Id.ToString();
								this.deviceForTest.OutputID = outputchar.Id.ToString();
							}
							return flag;
						}
						catch (Exception ex)
						{
							this.ErrorMessage = ex.Message;
							return false;
						}
					}
					List<KeyValuePair<ICharacteristic, ICharacteristic>>.Enumerator enumerator = default(List<KeyValuePair<ICharacteristic, ICharacteristic>>.Enumerator);
				}
			}
			return false;
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x000A9474 File Offset: 0x000A7674
		private async Task<bool> CheckCharacteristics(ICharacteristic input, ICharacteristic output)
		{
			if (input.CanUpdate && output.CanWrite)
			{
				this.buffer = new ObservableCollection<byte>();
				input.ValueUpdated += this.OnDataReceived;
				if (input.CanUpdate)
				{
					try
					{
						await input.StartUpdatesAsync(default(CancellationToken));
					}
					catch (Exception ex)
					{
						this.ErrorMessage = ex.Message;
					}
				}
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(4.0));
				try
				{
					await output.WriteAsync(Encoding.ASCII.GetBytes("ATZ\r"), cancellationTokenSource.Token);
					await Task.Delay(1000);
					string response = Encoding.ASCII.GetString(this.buffer.ToArray<byte>());
					if (string.IsNullOrEmpty(response))
					{
						await App.OBDReader.DebugWrite("\nBTLE_EMPTY_RESPONSE\n");
					}
					if (!string.IsNullOrEmpty(response) && (response.ToLowerInvariant().Contains("elm") || response.Contains(">")))
					{
						try
						{
							await input.StopUpdatesAsync(default(CancellationToken));
						}
						catch (Exception)
						{
						}
						input.ValueUpdated -= this.OnDataReceived;
						await App.OBDReader.DebugWrite("\nBTLE_RESPONSE=" + response + "\n");
						return true;
					}
					response = null;
				}
				catch (Exception ex2)
				{
					this.ErrorMessage = ex2.Message;
					return false;
				}
			}
			return false;
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x060016E7 RID: 5863 RVA: 0x000A94C7 File Offset: 0x000A76C7
		public BTLEDeviceDescription DeviceForTest
		{
			get
			{
				return this.deviceForTest;
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x000A94D0 File Offset: 0x000A76D0
		private void OnDataReceived(object sender, CharacteristicUpdatedEventArgs e)
		{
			byte[] value = e.Characteristic.Value;
			if (value != null)
			{
				foreach (byte b in value)
				{
					this.buffer.Add(b);
				}
			}
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x000A950C File Offset: 0x000A770C
		public async Task<string> GetDeviceInfo(IDevice device)
		{
			await this.StopDiscovering();
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Device name: " + device.Name);
			sb.AppendLine("Device name: " + device.Id.ToString());
			this.IsTestingDevice = true;
			this.deviceForTest = new BTLEDeviceDescription();
			this.TestingDeviceString = Translate.GetString("ios_BTLE_CheckingDevice");
			try
			{
				Task task = Task.Delay(TimeSpan.FromSeconds(4.0));
				Task connect_task = null;
				if (PlatformHelper.IsAndroid)
				{
					ConnectParameters connectParameters;
					connectParameters..ctor(false, true);
					connect_task = this.adapter.ConnectToDeviceAsync(device, connectParameters, default(CancellationToken));
				}
				if (PlatformHelper.IsiOS)
				{
					connect_task = this.adapter.ConnectToDeviceAsync(device, default(ConnectParameters), default(CancellationToken));
				}
				Task task2 = await Task.WhenAny(new Task[] { task, connect_task });
				if (task2 != connect_task || task2.IsFaulted)
				{
					this.ErrorMessage = "Connection to device failed";
					this.IsTestingDevice = false;
					return "Connection to device failed";
				}
				if (PlatformHelper.IsAndroid)
				{
					await Task.Delay(TimeSpan.FromSeconds(1.0));
				}
				connect_task = null;
			}
			catch (Exception)
			{
				this.ErrorMessage = "Connection failed";
			}
			if (device.State == 2)
			{
				this.deviceForTest.NamePart = device.Name;
				new CancellationTokenSource();
				Task connect_task = device.GetServicesAsync(default(CancellationToken));
				Task task3 = await Task.WhenAny(new Task[]
				{
					Task.Delay(TimeSpan.FromSeconds(4.0)),
					connect_task
				});
				if (task3 == connect_task && !task3.IsFaulted)
				{
					foreach (IService service in ((Task<IReadOnlyList<IService>>)connect_task).Result)
					{
						sb.AppendLine("Service UUID: " + service.Id.ToString());
						try
						{
							IEnumerable<ICharacteristic> enumerable = await service.GetCharacteristicsAsync();
							sb.AppendLine("Characteristics:");
							foreach (ICharacteristic characteristic in enumerable)
							{
								sb.AppendLine(string.Format("Id: {0}, R={1}, W={2}, U={3}", new object[]
								{
									characteristic.Id,
									characteristic.CanRead.ToString(),
									characteristic.CanWrite.ToString(),
									characteristic.CanUpdate.ToString()
								}));
							}
						}
						catch (Exception ex)
						{
							sb.AppendLine(ex.Message ?? "");
						}
					}
					IEnumerator<IService> enumerator = null;
				}
				connect_task = null;
			}
			this.IsTestingDevice = false;
			return sb.ToString();
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x000A9558 File Offset: 0x000A7758
		public static async Task<bool> FindDeviceWithRandomizedUUID()
		{
			bool flag;
			if (string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceName))
			{
				flag = false;
			}
			else
			{
				IBTLEDeviceSelectorViewModel model = new BTLEDeviceSelectorViewModel();
				for (int i = 0; i < 2; i++)
				{
					model.DiscoverDevices.Execute(null);
					Stopwatch sw = new Stopwatch();
					sw.Start();
					TimeSpan discoverTimeLimit = TimeSpan.FromSeconds(3.0);
					while (sw.Elapsed < discoverTimeLimit)
					{
						IDevice device = model.DeviceList.ToArray<IDevice>().FirstOrDefault((IDevice x) => x.Name == SharedSettings.Current.BTLEDeviceName);
						if (device != null)
						{
							sw.Stop();
							SharedSettings.Current.BTLEDeviceID = device.Id.ToString();
							return true;
						}
						await Task.Delay(50);
					}
					sw = null;
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x000A9594 File Offset: 0x000A7794
		[CompilerGenerated]
		private async void <get_DiscoverDevices>b__21_0()
		{
			this.DeviceList.Clear();
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
			if (this.adapter.IsScanning)
			{
				await this.adapter.StopScanningForDevicesAsync();
				this.OnPropertyChanged("IsScanning");
				await Task.Delay(1000);
			}
			this.adapter.StartScanningForDevicesAsync(null, null, false, default(CancellationToken));
			this.OnPropertyChanged("IsScanning");
		}

		// Token: 0x040009A9 RID: 2473
		private IAdapter adapter;

		// Token: 0x040009AA RID: 2474
		private IBluetoothLE ble;

		// Token: 0x040009AB RID: 2475
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040009AC RID: 2476
		private ObservableCollection<IDevice> _DeviceList = new ObservableCollection<IDevice>();

		// Token: 0x040009AD RID: 2477
		private bool _IsTestingDevice;

		// Token: 0x040009AE RID: 2478
		private string _TestingDeviceString = "";

		// Token: 0x040009AF RID: 2479
		[CompilerGenerated]
		private string <ErrorMessage>k__BackingField;

		// Token: 0x040009B0 RID: 2480
		private ObservableCollection<byte> buffer = new ObservableCollection<byte>();

		// Token: 0x040009B1 RID: 2481
		private BTLEDeviceDescription deviceForTest;

		// Token: 0x020001A7 RID: 423
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_DiscoverDevices>b__21_0>d : IAsyncStateMachine
		{
			// Token: 0x060016EC RID: 5868 RVA: 0x000A95CC File Offset: 0x000A77CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorViewModel btledeviceSelectorViewModel = this;
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
						goto IL_010C;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0173;
					}
					default:
						btledeviceSelectorViewModel.DeviceList.Clear();
						if (!PlatformHelper.IsAndroid || !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
						{
							goto IL_00A3;
						}
						taskAwaiter3 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTLEDeviceSelectorViewModel.<<get_DiscoverDevices>b__21_0>d>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (taskAwaiter3.GetResult() != 3)
					{
						goto IL_01B9;
					}
					IL_00A3:
					if (!btledeviceSelectorViewModel.adapter.IsScanning)
					{
						goto IL_017A;
					}
					taskAwaiter4 = btledeviceSelectorViewModel.adapter.StopScanningForDevicesAsync().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<<get_DiscoverDevices>b__21_0>d>(ref taskAwaiter4, ref this);
						return;
					}
					IL_010C:
					taskAwaiter4.GetResult();
					btledeviceSelectorViewModel.OnPropertyChanged("IsScanning");
					taskAwaiter4 = Task.Delay(1000).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<<get_DiscoverDevices>b__21_0>d>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0173:
					taskAwaiter4.GetResult();
					IL_017A:
					btledeviceSelectorViewModel.adapter.StartScanningForDevicesAsync(null, null, false, default(CancellationToken));
					btledeviceSelectorViewModel.OnPropertyChanged("IsScanning");
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01B9:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060016ED RID: 5869 RVA: 0x000A97C4 File Offset: 0x000A79C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040009B2 RID: 2482
			public int <>1__state;

			// Token: 0x040009B3 RID: 2483
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040009B4 RID: 2484
			public BTLEDeviceSelectorViewModel <>4__this;

			// Token: 0x040009B5 RID: 2485
			private TaskAwaiter<PermissionStatus> <>u__1;

			// Token: 0x040009B6 RID: 2486
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020001A8 RID: 424
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060016EE RID: 5870 RVA: 0x000A97D2 File Offset: 0x000A79D2
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060016EF RID: 5871 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060016F0 RID: 5872 RVA: 0x000A97DE File Offset: 0x000A79DE
			internal bool <CheckService>b__35_0(ICharacteristic x)
			{
				return x.CanUpdate;
			}

			// Token: 0x060016F1 RID: 5873 RVA: 0x000A97E6 File Offset: 0x000A79E6
			internal bool <CheckService>b__35_1(ICharacteristic x)
			{
				return x.CanWrite;
			}

			// Token: 0x060016F2 RID: 5874 RVA: 0x000A97EE File Offset: 0x000A79EE
			internal bool <FindDeviceWithRandomizedUUID>b__43_0(IDevice x)
			{
				return x.Name == SharedSettings.Current.BTLEDeviceName;
			}

			// Token: 0x040009B7 RID: 2487
			public static readonly BTLEDeviceSelectorViewModel.<>c <>9 = new BTLEDeviceSelectorViewModel.<>c();

			// Token: 0x040009B8 RID: 2488
			public static Func<ICharacteristic, bool> <>9__35_0;

			// Token: 0x040009B9 RID: 2489
			public static Func<ICharacteristic, bool> <>9__35_1;

			// Token: 0x040009BA RID: 2490
			public static Func<IDevice, bool> <>9__43_0;
		}

		// Token: 0x020001A9 RID: 425
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x060016F3 RID: 5875 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x060016F4 RID: 5876 RVA: 0x000A9808 File Offset: 0x000A7A08
			internal async void <OnDeviceDiscovered>b__0()
			{
				if (this.e.Device != null)
				{
					await Task.Delay(new Random().Next(70, 300));
					BTLENameToVisibleBoolConverter btlenameToVisibleBoolConverter = new BTLENameToVisibleBoolConverter();
					bool isDeviceValid = (bool)btlenameToVisibleBoolConverter.Convert(this.e.Device.Name, null, null, null);
					int num = 0;
					try
					{
						if (isDeviceValid)
						{
							this.<>4__this.DeviceList.Insert(0, this.e.Device);
						}
						else if (SharedSettings.Current.BTLEShowDevicesWithoutName || (!SharedSettings.Current.BTLEShowDevicesWithoutName && !string.IsNullOrEmpty(this.e.Device.Name)))
						{
							this.<>4__this.DeviceList.Add(this.e.Device);
						}
					}
					catch (Exception obj)
					{
						num = 1;
					}
					if (num == 1)
					{
						object obj;
						Exception ex = (Exception)obj;
						try
						{
							await Task.Delay(new Random().Next(350, 500));
							if (isDeviceValid)
							{
								this.<>4__this.DeviceList.Insert(0, this.e.Device);
							}
							else if (SharedSettings.Current.BTLEShowDevicesWithoutName || (!SharedSettings.Current.BTLEShowDevicesWithoutName && !string.IsNullOrEmpty(this.e.Device.Name)))
							{
								this.<>4__this.DeviceList.Add(this.e.Device);
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}

			// Token: 0x040009BB RID: 2491
			public DeviceEventArgs e;

			// Token: 0x040009BC RID: 2492
			public BTLEDeviceSelectorViewModel <>4__this;

			// Token: 0x020001AA RID: 426
			[StructLayout(LayoutKind.Auto)]
			private struct <<OnDeviceDiscovered>b__0>d : IAsyncStateMachine
			{
				// Token: 0x060016F5 RID: 5877 RVA: 0x000A9840 File Offset: 0x000A7A40
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					BTLEDeviceSelectorViewModel.<>c__DisplayClass17_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter taskAwaiter;
						TaskAwaiter taskAwaiter2;
						if (num != 0)
						{
							if (num == 1)
							{
								goto IL_014B;
							}
							if (CS$<>8__locals1.e.Device == null)
							{
								goto IL_024E;
							}
							taskAwaiter = Task.Delay(new Random().Next(70, 300)).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<>c__DisplayClass17_0.<<OnDeviceDiscovered>b__0>d>(ref taskAwaiter, ref this);
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
						BTLENameToVisibleBoolConverter btlenameToVisibleBoolConverter = new BTLENameToVisibleBoolConverter();
						isDeviceValid = (bool)btlenameToVisibleBoolConverter.Convert(CS$<>8__locals1.e.Device.Name, null, null, null);
						int num3 = 0;
						try
						{
							if (isDeviceValid)
							{
								CS$<>8__locals1.<>4__this.DeviceList.Insert(0, CS$<>8__locals1.e.Device);
							}
							else if (SharedSettings.Current.BTLEShowDevicesWithoutName || (!SharedSettings.Current.BTLEShowDevicesWithoutName && !string.IsNullOrEmpty(CS$<>8__locals1.e.Device.Name)))
							{
								CS$<>8__locals1.<>4__this.DeviceList.Add(CS$<>8__locals1.e.Device);
							}
						}
						catch (Exception obj)
						{
							num3 = 1;
						}
						if (num3 != 1)
						{
							goto IL_0233;
						}
						object obj;
						Exception ex = (Exception)obj;
						IL_014B:
						try
						{
							if (num != 1)
							{
								taskAwaiter = Task.Delay(new Random().Next(350, 500)).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 1);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<>c__DisplayClass17_0.<<OnDeviceDiscovered>b__0>d>(ref taskAwaiter, ref this);
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
							if (isDeviceValid)
							{
								CS$<>8__locals1.<>4__this.DeviceList.Insert(0, CS$<>8__locals1.e.Device);
							}
							else if (SharedSettings.Current.BTLEShowDevicesWithoutName || (!SharedSettings.Current.BTLEShowDevicesWithoutName && !string.IsNullOrEmpty(CS$<>8__locals1.e.Device.Name)))
							{
								CS$<>8__locals1.<>4__this.DeviceList.Add(CS$<>8__locals1.e.Device);
							}
						}
						catch (Exception)
						{
						}
						IL_0233:;
					}
					catch (Exception ex2)
					{
						num2 = -2;
						this.<>t__builder.SetException(ex2);
						return;
					}
					IL_024E:
					num2 = -2;
					this.<>t__builder.SetResult();
				}

				// Token: 0x060016F6 RID: 5878 RVA: 0x000A9AFC File Offset: 0x000A7CFC
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040009BD RID: 2493
				public int <>1__state;

				// Token: 0x040009BE RID: 2494
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x040009BF RID: 2495
				public BTLEDeviceSelectorViewModel.<>c__DisplayClass17_0 <>4__this;

				// Token: 0x040009C0 RID: 2496
				private bool <isDeviceValid>5__2;

				// Token: 0x040009C1 RID: 2497
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x020001AB RID: 427
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckCharacteristics>d__36 : IAsyncStateMachine
		{
			// Token: 0x060016F7 RID: 5879 RVA: 0x000A9B0C File Offset: 0x000A7D0C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorViewModel btledeviceSelectorViewModel = this;
				bool flag;
				try
				{
					if (num != 0)
					{
						if (num - 1 <= 4)
						{
							goto IL_010A;
						}
						if (!input.CanUpdate || !output.CanWrite)
						{
							goto IL_03EF;
						}
						btledeviceSelectorViewModel.buffer = new ObservableCollection<byte>();
						input.ValueUpdated += btledeviceSelectorViewModel.OnDataReceived;
						if (!input.CanUpdate)
						{
							goto IL_00F0;
						}
					}
					TaskAwaiter taskAwaiter2;
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = input.StartUpdatesAsync(default(CancellationToken)).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<CheckCharacteristics>d__36>(ref taskAwaiter, ref this);
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
					catch (Exception ex)
					{
						btledeviceSelectorViewModel.ErrorMessage = ex.Message;
					}
					IL_00F0:
					CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
					cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(4.0));
					IL_010A:
					try
					{
						TaskAwaiter taskAwaiter;
						TaskAwaiter<bool> taskAwaiter3;
						switch (num)
						{
						case 1:
						{
							TaskAwaiter<bool> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<bool>);
							num = (num2 = -1);
							break;
						}
						case 2:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_01FB;
						case 3:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_028A;
						case 4:
							IL_02CD:
							try
							{
								if (num != 4)
								{
									taskAwaiter = input.StopUpdatesAsync(default(CancellationToken)).GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num = (num2 = 4);
										taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<CheckCharacteristics>d__36>(ref taskAwaiter, ref this);
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
							input.ValueUpdated -= btledeviceSelectorViewModel.OnDataReceived;
							taskAwaiter = App.OBDReader.DebugWrite("\nBTLE_RESPONSE=" + response + "\n").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 5);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<CheckCharacteristics>d__36>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_03C8;
						case 5:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_03C8;
						default:
							taskAwaiter3 = output.WriteAsync(Encoding.ASCII.GetBytes("ATZ\r"), cancellationTokenSource.Token).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 1);
								TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, BTLEDeviceSelectorViewModel.<CheckCharacteristics>d__36>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						}
						taskAwaiter3.GetResult();
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 2);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<CheckCharacteristics>d__36>(ref taskAwaiter, ref this);
							return;
						}
						IL_01FB:
						taskAwaiter.GetResult();
						response = Encoding.ASCII.GetString(btledeviceSelectorViewModel.buffer.ToArray<byte>());
						if (!string.IsNullOrEmpty(response))
						{
							goto IL_0291;
						}
						taskAwaiter = App.OBDReader.DebugWrite("\nBTLE_EMPTY_RESPONSE\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 3);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<CheckCharacteristics>d__36>(ref taskAwaiter, ref this);
							return;
						}
						IL_028A:
						taskAwaiter.GetResult();
						IL_0291:
						if (!string.IsNullOrEmpty(response) && (response.ToLowerInvariant().Contains("elm") || response.Contains(">")))
						{
							goto IL_02CD;
						}
						response = null;
						goto IL_03EF;
						IL_03C8:
						taskAwaiter.GetResult();
						flag = true;
						goto IL_040C;
					}
					catch (Exception ex2)
					{
						btledeviceSelectorViewModel.ErrorMessage = ex2.Message;
						flag = false;
						goto IL_040C;
					}
					IL_03EF:
					flag = false;
				}
				catch (Exception ex3)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex3);
					return;
				}
				IL_040C:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060016F8 RID: 5880 RVA: 0x000A9FA0 File Offset: 0x000A81A0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040009C2 RID: 2498
			public int <>1__state;

			// Token: 0x040009C3 RID: 2499
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040009C4 RID: 2500
			public ICharacteristic input;

			// Token: 0x040009C5 RID: 2501
			public ICharacteristic output;

			// Token: 0x040009C6 RID: 2502
			public BTLEDeviceSelectorViewModel <>4__this;

			// Token: 0x040009C7 RID: 2503
			private TaskAwaiter <>u__1;

			// Token: 0x040009C8 RID: 2504
			private string <response>5__2;

			// Token: 0x040009C9 RID: 2505
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x020001AC RID: 428
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckService>d__35 : IAsyncStateMachine
		{
			// Token: 0x060016F9 RID: 5881 RVA: 0x000A9FB0 File Offset: 0x000A81B0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorViewModel btledeviceSelectorViewModel = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<Task> taskAwaiter3;
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
					{
						TaskAwaiter<Task> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Task>);
						num = (num2 = -1);
						goto IL_010C;
					}
					case 2:
						IL_021C:
						try
						{
							if (num != 2)
							{
								goto IL_031F;
							}
							IL_024D:
							try
							{
								TaskAwaiter<bool> taskAwaiter5;
								if (num != 2)
								{
									taskAwaiter5 = btledeviceSelectorViewModel.CheckCharacteristics(inputchar, outputchar).GetAwaiter();
									if (!taskAwaiter5.IsCompleted)
									{
										num = (num2 = 2);
										TaskAwaiter<bool> taskAwaiter6 = taskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, BTLEDeviceSelectorViewModel.<CheckService>d__35>(ref taskAwaiter5, ref this);
										return;
									}
								}
								else
								{
									TaskAwaiter<bool> taskAwaiter6;
									taskAwaiter5 = taskAwaiter6;
									taskAwaiter6 = default(TaskAwaiter<bool>);
									num = (num2 = -1);
								}
								bool result = taskAwaiter5.GetResult();
								if (result)
								{
									btledeviceSelectorViewModel.deviceForTest.InputID = inputchar.Id.ToString();
									btledeviceSelectorViewModel.deviceForTest.OutputID = outputchar.Id.ToString();
								}
								flag = result;
								goto IL_0377;
							}
							catch (Exception ex)
							{
								btledeviceSelectorViewModel.ErrorMessage = ex.Message;
								flag = false;
								goto IL_0377;
							}
							IL_031F:
							if (enumerator.MoveNext())
							{
								KeyValuePair<ICharacteristic, ICharacteristic> keyValuePair = enumerator.Current;
								inputchar = keyValuePair.Key;
								outputchar = keyValuePair.Value;
								goto IL_024D;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = default(List<KeyValuePair<ICharacteristic, ICharacteristic>>.Enumerator);
						goto IL_0353;
					default:
						taskAwaiter = btledeviceSelectorViewModel.StopDiscovering().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<CheckService>d__35>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					Task task = Task.Delay(TimeSpan.FromSeconds(4.0));
					service_task = service.GetCharacteristicsAsync();
					taskAwaiter3 = Task.WhenAny(new Task[] { task, service_task }).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<Task> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, BTLEDeviceSelectorViewModel.<CheckService>d__35>(ref taskAwaiter3, ref this);
						return;
					}
					IL_010C:
					Task result2 = taskAwaiter3.GetResult();
					if (result2 == service_task && !result2.IsFaulted)
					{
						IReadOnlyList<ICharacteristic> result3 = ((Task<IReadOnlyList<ICharacteristic>>)service_task).Result;
						if (result3.Count >= 1)
						{
							ICharacteristic[] array = result3.Where((ICharacteristic x) => x.CanUpdate).ToArray<ICharacteristic>();
							ICharacteristic[] array2 = result3.Where((ICharacteristic x) => x.CanWrite).ToArray<ICharacteristic>();
							if (array.Length == 0 || array2.Length == 0)
							{
								flag = false;
								goto IL_0377;
							}
							List<KeyValuePair<ICharacteristic, ICharacteristic>> list = new List<KeyValuePair<ICharacteristic, ICharacteristic>>();
							foreach (ICharacteristic characteristic in array)
							{
								foreach (ICharacteristic characteristic2 in array2)
								{
									KeyValuePair<ICharacteristic, ICharacteristic> keyValuePair2 = new KeyValuePair<ICharacteristic, ICharacteristic>(characteristic, characteristic2);
									list.Add(keyValuePair2);
								}
							}
							enumerator = list.GetEnumerator();
							goto IL_021C;
						}
					}
					IL_0353:
					flag = false;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					service_task = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_0377:
				num2 = -2;
				service_task = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060016FA RID: 5882 RVA: 0x000AA39C File Offset: 0x000A859C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040009CA RID: 2506
			public int <>1__state;

			// Token: 0x040009CB RID: 2507
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040009CC RID: 2508
			public BTLEDeviceSelectorViewModel <>4__this;

			// Token: 0x040009CD RID: 2509
			public IService service;

			// Token: 0x040009CE RID: 2510
			private Task <service_task>5__2;

			// Token: 0x040009CF RID: 2511
			private TaskAwaiter <>u__1;

			// Token: 0x040009D0 RID: 2512
			private TaskAwaiter<Task> <>u__2;

			// Token: 0x040009D1 RID: 2513
			private List<KeyValuePair<ICharacteristic, ICharacteristic>>.Enumerator <>7__wrap2;

			// Token: 0x040009D2 RID: 2514
			private ICharacteristic <inputchar>5__4;

			// Token: 0x040009D3 RID: 2515
			private ICharacteristic <outputchar>5__5;

			// Token: 0x040009D4 RID: 2516
			private TaskAwaiter<bool> <>u__3;
		}

		// Token: 0x020001AD RID: 429
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <FindDeviceWithRandomizedUUID>d__43 : IAsyncStateMachine
		{
			// Token: 0x060016FB RID: 5883 RVA: 0x000AA3AC File Offset: 0x000A85AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceName))
						{
							flag = false;
							goto IL_01A7;
						}
						model = new BTLEDeviceSelectorViewModel();
						i = 0;
						goto IL_0177;
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					IL_013C:
					taskAwaiter.GetResult();
					IL_0143:
					if (!(sw.Elapsed < discoverTimeLimit))
					{
						sw = null;
						int num3 = i;
						i = num3 + 1;
					}
					else
					{
						IDevice device = model.DeviceList.ToArray<IDevice>().FirstOrDefault((IDevice x) => x.Name == SharedSettings.Current.BTLEDeviceName);
						if (device != null)
						{
							sw.Stop();
							SharedSettings.Current.BTLEDeviceID = device.Id.ToString();
							flag = true;
							goto IL_01A7;
						}
						taskAwaiter = Task.Delay(50).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<FindDeviceWithRandomizedUUID>d__43>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_013C;
					}
					IL_0177:
					if (i < 2)
					{
						model.DiscoverDevices.Execute(null);
						sw = new Stopwatch();
						sw.Start();
						discoverTimeLimit = TimeSpan.FromSeconds(3.0);
						goto IL_0143;
					}
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					model = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01A7:
				num2 = -2;
				model = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060016FC RID: 5884 RVA: 0x000AA598 File Offset: 0x000A8798
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040009D5 RID: 2517
			public int <>1__state;

			// Token: 0x040009D6 RID: 2518
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040009D7 RID: 2519
			private IBTLEDeviceSelectorViewModel <model>5__2;

			// Token: 0x040009D8 RID: 2520
			private int <i>5__3;

			// Token: 0x040009D9 RID: 2521
			private Stopwatch <sw>5__4;

			// Token: 0x040009DA RID: 2522
			private TimeSpan <discoverTimeLimit>5__5;

			// Token: 0x040009DB RID: 2523
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001AE RID: 430
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetDeviceInfo>d__42 : IAsyncStateMachine
		{
			// Token: 0x060016FD RID: 5885 RVA: 0x000AA5A8 File Offset: 0x000A87A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorViewModel btledeviceSelectorViewModel = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<Task> taskAwaiter3;
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
					case 2:
					{
						IL_0101:
						try
						{
							if (num != 1)
							{
								if (num == 2)
								{
									TaskAwaiter taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter);
									num = (num2 = -1);
									goto IL_0282;
								}
								Task task = Task.Delay(TimeSpan.FromSeconds(4.0));
								connect_task = null;
								if (PlatformHelper.IsAndroid)
								{
									ConnectParameters connectParameters;
									connectParameters..ctor(false, true);
									connect_task = btledeviceSelectorViewModel.adapter.ConnectToDeviceAsync(device, connectParameters, default(CancellationToken));
								}
								if (PlatformHelper.IsiOS)
								{
									connect_task = btledeviceSelectorViewModel.adapter.ConnectToDeviceAsync(device, default(ConnectParameters), default(CancellationToken));
								}
								taskAwaiter3 = Task.WhenAny(new Task[] { task, connect_task }).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 1);
									TaskAwaiter<Task> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, BTLEDeviceSelectorViewModel.<GetDeviceInfo>d__42>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter<Task> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<Task>);
								num = (num2 = -1);
							}
							Task result = taskAwaiter3.GetResult();
							if (result != connect_task || result.IsFaulted)
							{
								btledeviceSelectorViewModel.ErrorMessage = "Connection to device failed";
								btledeviceSelectorViewModel.IsTestingDevice = false;
								text = "Connection to device failed";
								goto IL_059F;
							}
							if (!PlatformHelper.IsAndroid)
							{
								goto IL_02A8;
							}
							taskAwaiter = Task.Delay(TimeSpan.FromSeconds(1.0)).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 2);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<GetDeviceInfo>d__42>(ref taskAwaiter, ref this);
								return;
							}
							IL_0282:
							taskAwaiter.GetResult();
							IL_02A8:
							connect_task = null;
						}
						catch (Exception)
						{
							btledeviceSelectorViewModel.ErrorMessage = "Connection failed";
						}
						if (device.State != 2)
						{
							goto IL_056A;
						}
						btledeviceSelectorViewModel.deviceForTest.NamePart = device.Name;
						new CancellationTokenSource();
						connect_task = device.GetServicesAsync(default(CancellationToken));
						Task task2 = Task.Delay(TimeSpan.FromSeconds(4.0));
						taskAwaiter3 = Task.WhenAny(new Task[] { task2, connect_task }).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 3);
							TaskAwaiter<Task> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, BTLEDeviceSelectorViewModel.<GetDeviceInfo>d__42>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0386;
					}
					case 3:
					{
						TaskAwaiter<Task> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Task>);
						num = (num2 = -1);
						goto IL_0386;
					}
					case 4:
						IL_03C7:
						try
						{
							if (num != 4)
							{
								goto IL_0532;
							}
							IL_040A:
							try
							{
								TaskAwaiter<IReadOnlyList<ICharacteristic>> taskAwaiter5;
								if (num != 4)
								{
									IService service;
									taskAwaiter5 = service.GetCharacteristicsAsync().GetAwaiter();
									if (!taskAwaiter5.IsCompleted)
									{
										num = (num2 = 4);
										TaskAwaiter<IReadOnlyList<ICharacteristic>> taskAwaiter6 = taskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IReadOnlyList<ICharacteristic>>, BTLEDeviceSelectorViewModel.<GetDeviceInfo>d__42>(ref taskAwaiter5, ref this);
										return;
									}
								}
								else
								{
									TaskAwaiter<IReadOnlyList<ICharacteristic>> taskAwaiter6;
									taskAwaiter5 = taskAwaiter6;
									taskAwaiter6 = default(TaskAwaiter<IReadOnlyList<ICharacteristic>>);
									num = (num2 = -1);
								}
								IEnumerable<ICharacteristic> result2 = taskAwaiter5.GetResult();
								sb.AppendLine("Characteristics:");
								IEnumerator<ICharacteristic> enumerator2 = result2.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										ICharacteristic characteristic = enumerator2.Current;
										sb.AppendLine(string.Format("Id: {0}, R={1}, W={2}, U={3}", new object[]
										{
											characteristic.Id,
											characteristic.CanRead.ToString(),
											characteristic.CanWrite.ToString(),
											characteristic.CanUpdate.ToString()
										}));
									}
								}
								finally
								{
									if (num < 0 && enumerator2 != null)
									{
										enumerator2.Dispose();
									}
								}
							}
							catch (Exception ex)
							{
								sb.AppendLine(ex.Message ?? "");
							}
							IL_0532:
							if (enumerator.MoveNext())
							{
								IService service = enumerator.Current;
								sb.AppendLine("Service UUID: " + service.Id.ToString());
								goto IL_040A;
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						enumerator = null;
						goto IL_0563;
					default:
						taskAwaiter = btledeviceSelectorViewModel.StopDiscovering().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<GetDeviceInfo>d__42>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					sb = new StringBuilder();
					sb.AppendLine("Device name: " + device.Name);
					sb.AppendLine("Device name: " + device.Id.ToString());
					btledeviceSelectorViewModel.IsTestingDevice = true;
					btledeviceSelectorViewModel.deviceForTest = new BTLEDeviceDescription();
					btledeviceSelectorViewModel.TestingDeviceString = Translate.GetString("ios_BTLE_CheckingDevice");
					goto IL_0101;
					IL_0386:
					Task result3 = taskAwaiter3.GetResult();
					if (result3 == connect_task && !result3.IsFaulted)
					{
						IReadOnlyList<IService> result4 = ((Task<IReadOnlyList<IService>>)connect_task).Result;
						enumerator = result4.GetEnumerator();
						goto IL_03C7;
					}
					IL_0563:
					connect_task = null;
					IL_056A:
					btledeviceSelectorViewModel.IsTestingDevice = false;
					text = sb.ToString();
				}
				catch (Exception ex2)
				{
					num2 = -2;
					sb = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_059F:
				num2 = -2;
				sb = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x060016FE RID: 5886 RVA: 0x000AABEC File Offset: 0x000A8DEC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040009DC RID: 2524
			public int <>1__state;

			// Token: 0x040009DD RID: 2525
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x040009DE RID: 2526
			public BTLEDeviceSelectorViewModel <>4__this;

			// Token: 0x040009DF RID: 2527
			public IDevice device;

			// Token: 0x040009E0 RID: 2528
			private StringBuilder <sb>5__2;

			// Token: 0x040009E1 RID: 2529
			private TaskAwaiter <>u__1;

			// Token: 0x040009E2 RID: 2530
			private Task <connect_task>5__3;

			// Token: 0x040009E3 RID: 2531
			private TaskAwaiter<Task> <>u__2;

			// Token: 0x040009E4 RID: 2532
			private IEnumerator<IService> <>7__wrap3;

			// Token: 0x040009E5 RID: 2533
			private TaskAwaiter<IReadOnlyList<ICharacteristic>> <>u__3;
		}

		// Token: 0x020001AF RID: 431
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StopDiscovering>d__15 : IAsyncStateMachine
		{
			// Token: 0x060016FF RID: 5887 RVA: 0x000AABFC File Offset: 0x000A8DFC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorViewModel btledeviceSelectorViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!btledeviceSelectorViewModel.ble.Adapter.IsScanning)
						{
							goto IL_0085;
						}
						taskAwaiter = btledeviceSelectorViewModel.ble.Adapter.StopScanningForDevicesAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<StopDiscovering>d__15>(ref taskAwaiter, ref this);
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
					IL_0085:;
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

			// Token: 0x06001700 RID: 5888 RVA: 0x000AACCC File Offset: 0x000A8ECC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040009E6 RID: 2534
			public int <>1__state;

			// Token: 0x040009E7 RID: 2535
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040009E8 RID: 2536
			public BTLEDeviceSelectorViewModel <>4__this;

			// Token: 0x040009E9 RID: 2537
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001B0 RID: 432
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <TestDevice>d__34 : IAsyncStateMachine
		{
			// Token: 0x06001701 RID: 5889 RVA: 0x000AACDC File Offset: 0x000A8EDC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorViewModel btledeviceSelectorViewModel = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<Task> taskAwaiter5;
					TaskAwaiter<bool> taskAwaiter7;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					case 2:
					{
						IL_00A5:
						try
						{
							if (num != 1)
							{
								if (num == 2)
								{
									TaskAwaiter taskAwaiter4;
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter);
									num2 = -1;
									goto IL_0226;
								}
								Task task = Task.Delay(TimeSpan.FromSeconds(10.0));
								connect_task = null;
								if (PlatformHelper.IsAndroid)
								{
									ConnectParameters connectParameters;
									connectParameters..ctor(false, true);
									connect_task = btledeviceSelectorViewModel.adapter.ConnectToDeviceAsync(device, connectParameters, default(CancellationToken));
								}
								if (PlatformHelper.IsiOS)
								{
									connect_task = btledeviceSelectorViewModel.adapter.ConnectToDeviceAsync(device, default(ConnectParameters), default(CancellationToken));
								}
								taskAwaiter5 = Task.WhenAny(new Task[] { task, connect_task }).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter<Task> taskAwaiter6 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, BTLEDeviceSelectorViewModel.<TestDevice>d__34>(ref taskAwaiter5, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter<Task> taskAwaiter6;
								taskAwaiter5 = taskAwaiter6;
								taskAwaiter6 = default(TaskAwaiter<Task>);
								num2 = -1;
							}
							Task result = taskAwaiter5.GetResult();
							if (result != connect_task || result.IsFaulted)
							{
								btledeviceSelectorViewModel.ErrorMessage = "Connection failed";
								btledeviceSelectorViewModel.IsTestingDevice = false;
								flag = false;
								goto IL_049D;
							}
							if (!PlatformHelper.IsAndroid)
							{
								goto IL_0248;
							}
							taskAwaiter3 = Task.Delay(TimeSpan.FromSeconds(1.0)).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<TestDevice>d__34>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0226:
							taskAwaiter3.GetResult();
							IL_0248:
							connect_task = null;
						}
						catch (Exception)
						{
							btledeviceSelectorViewModel.ErrorMessage = "Connection failed";
						}
						if (device.State != 2)
						{
							goto IL_0479;
						}
						btledeviceSelectorViewModel.deviceForTest.NamePart = device.Name;
						Task task2 = Task.Delay(TimeSpan.FromSeconds(1.0));
						connect_task = device.GetServicesAsync(default(CancellationToken));
						taskAwaiter5 = Task.WhenAny(new Task[] { task2, connect_task }).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter<Task> taskAwaiter6 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, BTLEDeviceSelectorViewModel.<TestDevice>d__34>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_0320;
					}
					case 3:
					{
						TaskAwaiter<Task> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<Task>);
						num2 = -1;
						goto IL_0320;
					}
					case 4:
						taskAwaiter7 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0406;
					default:
						taskAwaiter3 = btledeviceSelectorViewModel.StopDiscovering().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorViewModel.<TestDevice>d__34>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					btledeviceSelectorViewModel.IsTestingDevice = true;
					btledeviceSelectorViewModel.deviceForTest = new BTLEDeviceDescription();
					btledeviceSelectorViewModel.TestingDeviceString = Translate.GetString("ios_BTLE_CheckingDevice");
					goto IL_00A5;
					IL_0320:
					Task result2 = taskAwaiter5.GetResult();
					if (result2 == connect_task && !result2.IsFaulted)
					{
						services = ((Task<IReadOnlyList<IService>>)connect_task).Result;
						i = 0;
						goto IL_0455;
					}
					goto IL_0472;
					IL_0406:
					if (taskAwaiter7.GetResult())
					{
						btledeviceSelectorViewModel.deviceForTest.ServiceID = services[i].Id.ToString();
						flag = true;
						goto IL_049D;
					}
					int num3 = i;
					i = num3 + 1;
					IL_0455:
					if (i >= services.Count)
					{
						services = null;
					}
					else
					{
						btledeviceSelectorViewModel.TestingDeviceString = string.Format(Translate.GetString("ios_BTLE_CheckingDeviceStep2"), (i + 1).ToString(), services.Count.ToString());
						taskAwaiter7 = btledeviceSelectorViewModel.CheckService(services[i]).GetAwaiter();
						if (!taskAwaiter7.IsCompleted)
						{
							num2 = 4;
							taskAwaiter2 = taskAwaiter7;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, BTLEDeviceSelectorViewModel.<TestDevice>d__34>(ref taskAwaiter7, ref this);
							return;
						}
						goto IL_0406;
					}
					IL_0472:
					connect_task = null;
					IL_0479:
					btledeviceSelectorViewModel.IsTestingDevice = false;
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_049D:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06001702 RID: 5890 RVA: 0x000AB1D0 File Offset: 0x000A93D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040009EA RID: 2538
			public int <>1__state;

			// Token: 0x040009EB RID: 2539
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040009EC RID: 2540
			public BTLEDeviceSelectorViewModel <>4__this;

			// Token: 0x040009ED RID: 2541
			public IDevice device;

			// Token: 0x040009EE RID: 2542
			private TaskAwaiter <>u__1;

			// Token: 0x040009EF RID: 2543
			private Task <connect_task>5__2;

			// Token: 0x040009F0 RID: 2544
			private TaskAwaiter<Task> <>u__2;

			// Token: 0x040009F1 RID: 2545
			private IReadOnlyList<IService> <services>5__3;

			// Token: 0x040009F2 RID: 2546
			private int <i>5__4;

			// Token: 0x040009F3 RID: 2547
			private TaskAwaiter<bool> <>u__3;
		}
	}
}
