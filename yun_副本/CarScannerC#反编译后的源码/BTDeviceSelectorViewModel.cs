using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using CarScannerXamarinForms.Bluetooth2;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x020001A3 RID: 419
	internal class BTDeviceSelectorViewModel : INotifyPropertyChanged
	{
		// Token: 0x060016BB RID: 5819 RVA: 0x000A8CEC File Offset: 0x000A6EEC
		public BTDeviceSelectorViewModel()
		{
			this.adapter = PlatformHelper.CommonService.GetNewBluetooth2Manager();
			if (this.adapter != null)
			{
				this.adapter.OnStateChanged += this.Adapter_OnStateChanged;
				this.adapter.OnScanFinished += this.Adapter_OnScanFinished;
				this.adapter.OnDeviceDiscovered += this.Adapter_OnDeviceDiscovered;
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x060016BC RID: 5820 RVA: 0x000A8D67 File Offset: 0x000A6F67
		public ObservableCollection<IBluetooth2Device> DeviceList
		{
			get
			{
				return this._DeviceList;
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x060016BD RID: 5821 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public bool IsAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x060016BE RID: 5822 RVA: 0x000A8D72 File Offset: 0x000A6F72
		public bool IsBluetoothOn
		{
			get
			{
				return this.adapter.IsOn;
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x000A8D7F File Offset: 0x000A6F7F
		// (set) Token: 0x060016C0 RID: 5824 RVA: 0x000A8D87 File Offset: 0x000A6F87
		public bool IsScanning
		{
			get
			{
				return this._IsScanning;
			}
			set
			{
				if (value != this._IsScanning)
				{
					this._IsScanning = value;
					this.OnPropertyChanged("IsScanning");
				}
			}
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x060016C1 RID: 5825 RVA: 0x000A8DA4 File Offset: 0x000A6FA4
		// (set) Token: 0x060016C2 RID: 5826 RVA: 0x000A8DAC File Offset: 0x000A6FAC
		public bool IsPairing
		{
			get
			{
				return this._IsPairing;
			}
			set
			{
				if (value != this._IsPairing)
				{
					this._IsPairing = value;
					this.OnPropertyChanged("IsPairing");
				}
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060016C3 RID: 5827 RVA: 0x000A8DCC File Offset: 0x000A6FCC
		// (remove) Token: 0x060016C4 RID: 5828 RVA: 0x000A8E04 File Offset: 0x000A7004
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

		// Token: 0x060016C5 RID: 5829 RVA: 0x000A8E3C File Offset: 0x000A703C
		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x000A8E60 File Offset: 0x000A7060
		private void Adapter_OnDeviceDiscovered(IBluetooth2Manager manager, IBluetooth2Device device)
		{
			if (device == null)
			{
				return;
			}
			if (!this.DeviceList.Any((IBluetooth2Device x) => x.Id == device.Id && x.Name == device.Name))
			{
				if (device.IsValid)
				{
					this.DeviceList.Insert(0, device);
				}
				else
				{
					this.DeviceList.Add(device);
				}
				OBDDataReader obdreader = App.OBDReader;
				if (obdreader == null)
				{
					return;
				}
				obdreader.DebugWrite(string.Concat(new string[] { "\r\n[BTDeviceDiscovered: ", device.Name, " | ", device.Id, "]\r\n" }));
			}
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x000A8F20 File Offset: 0x000A7120
		private void Adapter_OnScanFinished(IBluetooth2Manager manager)
		{
			this.IsScanning = false;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x000A8F29 File Offset: 0x000A7129
		private void Adapter_OnStateChanged(IBluetooth2Manager manager)
		{
			this.OnPropertyChanged("IsAvailable");
			this.OnPropertyChanged("IsBluetoothOn");
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x000A8F41 File Offset: 0x000A7141
		public ICommand DiscoverDevices
		{
			get
			{
				return new Command(async delegate
				{
					this.DeviceList.Clear();
					if (!this.IsBluetoothOn)
					{
						await this.adapter.PowerOn();
					}
					await this.adapter.StartDiscoveringDevices();
				});
			}
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x060016CA RID: 5834 RVA: 0x000A8F54 File Offset: 0x000A7154
		public ICommand StopDiscoveringDevices
		{
			get
			{
				return new Command(delegate
				{
					IBluetooth2Manager bluetooth2Manager = this.adapter;
					if (bluetooth2Manager == null)
					{
						return;
					}
					bluetooth2Manager.StopDiscoveringDevices();
				});
			}
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x000A8F68 File Offset: 0x000A7168
		[CompilerGenerated]
		private async void <get_DiscoverDevices>b__25_0()
		{
			this.DeviceList.Clear();
			if (!this.IsBluetoothOn)
			{
				await this.adapter.PowerOn();
			}
			await this.adapter.StartDiscoveringDevices();
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x000A8F9F File Offset: 0x000A719F
		[CompilerGenerated]
		private void <get_StopDiscoveringDevices>b__27_0()
		{
			IBluetooth2Manager bluetooth2Manager = this.adapter;
			if (bluetooth2Manager == null)
			{
				return;
			}
			bluetooth2Manager.StopDiscoveringDevices();
		}

		// Token: 0x0400099F RID: 2463
		private ObservableCollection<IBluetooth2Device> _DeviceList = new ObservableCollection<IBluetooth2Device>();

		// Token: 0x040009A0 RID: 2464
		private bool _IsScanning;

		// Token: 0x040009A1 RID: 2465
		private bool _IsPairing;

		// Token: 0x040009A2 RID: 2466
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040009A3 RID: 2467
		private IBluetooth2Manager adapter;

		// Token: 0x020001A4 RID: 420
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_DiscoverDevices>b__25_0>d : IAsyncStateMachine
		{
			// Token: 0x060016CD RID: 5837 RVA: 0x000A8FB4 File Offset: 0x000A71B4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTDeviceSelectorViewModel btdeviceSelectorViewModel = this;
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
							goto IL_00E1;
						}
						btdeviceSelectorViewModel.DeviceList.Clear();
						if (btdeviceSelectorViewModel.IsBluetoothOn)
						{
							goto IL_008B;
						}
						taskAwaiter = btdeviceSelectorViewModel.adapter.PowerOn().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorViewModel.<<get_DiscoverDevices>b__25_0>d>(ref taskAwaiter, ref this);
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
					IL_008B:
					taskAwaiter = btdeviceSelectorViewModel.adapter.StartDiscoveringDevices().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorViewModel.<<get_DiscoverDevices>b__25_0>d>(ref taskAwaiter, ref this);
						return;
					}
					IL_00E1:
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

			// Token: 0x060016CE RID: 5838 RVA: 0x000A90E8 File Offset: 0x000A72E8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040009A4 RID: 2468
			public int <>1__state;

			// Token: 0x040009A5 RID: 2469
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040009A6 RID: 2470
			public BTDeviceSelectorViewModel <>4__this;

			// Token: 0x040009A7 RID: 2471
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001A5 RID: 421
		[CompilerGenerated]
		private sealed class <>c__DisplayClass20_0
		{
			// Token: 0x060016CF RID: 5839 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass20_0()
			{
			}

			// Token: 0x060016D0 RID: 5840 RVA: 0x000A90F6 File Offset: 0x000A72F6
			internal bool <Adapter_OnDeviceDiscovered>b__0(IBluetooth2Device x)
			{
				return x.Id == this.device.Id && x.Name == this.device.Name;
			}

			// Token: 0x040009A8 RID: 2472
			public IBluetooth2Device device;
		}
	}
}
