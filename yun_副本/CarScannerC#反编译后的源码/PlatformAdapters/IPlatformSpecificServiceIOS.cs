using System;
using System.Threading.Tasks;
using CarScannerXamarinForms.Bluetooth2;

namespace CarScannerXamarinForms.PlatformAdapters
{
	// Token: 0x020002DD RID: 733
	public interface IPlatformSpecificServiceIOS
	{
		// Token: 0x06002314 RID: 8980
		void StoreReceiptParser_LoadReceipt();

		// Token: 0x06002315 RID: 8981
		bool IsCoreBluetoothAuthorizationStatusDeniedOrRestricted();

		// Token: 0x06002316 RID: 8982
		void OpenSystemSettings();

		// Token: 0x06002317 RID: 8983
		void LocalNetworkPermissionService_EasyWayRequest();

		// Token: 0x06002318 RID: 8984
		Task<bool> SendDebugEmail(string path, string text, string address, string subject);

		// Token: 0x06002319 RID: 8985
		bool TrackingRequestHelper_ShouldRequestTracking();

		// Token: 0x0600231A RID: 8986
		Task<bool> TrackingRequestHelper_DisplayTrackingRequest();

		// Token: 0x0600231B RID: 8987
		void SetStatusBarStyle_DarkContent();

		// Token: 0x0600231C RID: 8988
		void SetStatusBarStyle_LightContent();

		// Token: 0x0600231D RID: 8989
		IBluetooth2Device MFIDevicesManager_FindFirstSupportedMFIDevice();

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x0600231E RID: 8990
		bool IsiOSApplicationOnMac { get; }

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x0600231F RID: 8991
		string AppDelegate_StartupLog { get; }
	}
}
