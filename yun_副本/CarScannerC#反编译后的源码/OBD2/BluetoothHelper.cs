using System;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x020002E2 RID: 738
	internal static class BluetoothHelper
	{
		// Token: 0x06002336 RID: 9014 RVA: 0x001AE1A8 File Offset: 0x001AC3A8
		public static void TurnOffBluetoothIfNeedTo()
		{
			if (PlatformHelper.IsAndroid && App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected && BluetoothHelper.BluetoothWasTurnedOnByTheApp && SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth && SharedSettings.Current.TurnOffBluetoothIfWasTurnedOn)
			{
				PlatformHelper.DroidService.AndroidHelper_RequestBluetoothPowerOff();
				BluetoothHelper.BluetoothWasTurnedOnByTheApp = false;
			}
		}

		// Token: 0x040010F5 RID: 4341
		public static bool BluetoothWasTurnedOnByTheApp;
	}
}
