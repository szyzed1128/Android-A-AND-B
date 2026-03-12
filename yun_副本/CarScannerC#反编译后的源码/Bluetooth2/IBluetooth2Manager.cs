using System;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Bluetooth2
{
	// Token: 0x02000BFA RID: 3066
	public interface IBluetooth2Manager
	{
		// Token: 0x06005C0F RID: 23567
		Task StartDiscoveringDevices();

		// Token: 0x06005C10 RID: 23568
		Task StopDiscoveringDevices();

		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x06005C11 RID: 23569
		bool IsOn { get; }

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x06005C12 RID: 23570
		bool IsAvailable { get; }

		// Token: 0x06005C13 RID: 23571
		Task PowerOn();

		// Token: 0x06005C14 RID: 23572
		Task PowerOff();

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x06005C15 RID: 23573
		bool CanChangeState { get; }

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06005C16 RID: 23574
		// (remove) Token: 0x06005C17 RID: 23575
		event DeviceDiscoveredDelegate OnDeviceDiscovered;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06005C18 RID: 23576
		// (remove) Token: 0x06005C19 RID: 23577
		event BluetoothStateChangedDelegate OnStateChanged;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06005C1A RID: 23578
		// (remove) Token: 0x06005C1B RID: 23579
		event BluetoothScanFinishedDelegate OnScanFinished;
	}
}
