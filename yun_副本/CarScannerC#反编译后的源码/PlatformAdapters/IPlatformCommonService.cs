using System;
using CarScannerXamarinForms.Bluetooth2;
using Sounds.FormsPlugin.Abstractions;

namespace CarScannerXamarinForms.PlatformAdapters
{
	// Token: 0x020002DB RID: 731
	public interface IPlatformCommonService
	{
		// Token: 0x060022E5 RID: 8933
		bool IsVersionEqualsOrHigher(int major, int minor);

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x060022E6 RID: 8934
		string AppVersion { get; }

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x060022E7 RID: 8935
		string AppBuild { get; }

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x060022E8 RID: 8936
		string DeviceID { get; }

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x060022E9 RID: 8937
		DateTime PlatformNowDateTime { get; }

		// Token: 0x060022EA RID: 8938
		void HideKeyboard();

		// Token: 0x060022EB RID: 8939
		void OpenPermissionsSettings();

		// Token: 0x060022EC RID: 8940
		string[] GetAvailableSounds();

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x060022ED RID: 8941
		ISoundManager SoundManager { get; }

		// Token: 0x060022EE RID: 8942
		ICustomInAppManager GetNewInAppManagerInstance();

		// Token: 0x060022EF RID: 8943
		void QuitApp();

		// Token: 0x060022F0 RID: 8944
		IBluetooth2Manager GetNewBluetooth2Manager();
	}
}
