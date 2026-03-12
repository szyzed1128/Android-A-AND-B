using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.BLE.Abstractions.Contracts;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x0200073F RID: 1855
	internal interface IBTLEDeviceSelectorViewModel
	{
		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x06003F09 RID: 16137
		BluetoothState CurrentState { get; }

		// Token: 0x1700148F RID: 5263
		// (get) Token: 0x06003F0A RID: 16138
		BTLEDeviceDescription DeviceForTest { get; }

		// Token: 0x17001490 RID: 5264
		// (get) Token: 0x06003F0B RID: 16139
		ObservableCollection<IDevice> DeviceList { get; }

		// Token: 0x17001491 RID: 5265
		// (get) Token: 0x06003F0C RID: 16140
		ICommand DiscoverDevices { get; }

		// Token: 0x17001492 RID: 5266
		// (get) Token: 0x06003F0D RID: 16141
		string ErrorMessage { get; }

		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x06003F0E RID: 16142
		bool IsBluetoothOn { get; }

		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x06003F0F RID: 16143
		// (set) Token: 0x06003F10 RID: 16144
		bool IsTestingDevice { get; set; }

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x06003F11 RID: 16145
		// (set) Token: 0x06003F12 RID: 16146
		string TestingDeviceString { get; set; }

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06003F13 RID: 16147
		// (remove) Token: 0x06003F14 RID: 16148
		event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06003F15 RID: 16149
		Task<string> GetDeviceInfo(IDevice device);

		// Token: 0x06003F16 RID: 16150
		Task StopDiscovering();

		// Token: 0x06003F17 RID: 16151
		Task<bool> TestDevice(IDevice device);
	}
}
