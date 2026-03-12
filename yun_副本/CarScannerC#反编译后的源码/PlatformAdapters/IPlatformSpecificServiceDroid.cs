using System;
using System.Net.Http;
using System.Threading.Tasks;
using CarScannerXamarinForms.InApp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.PlatformAdapters
{
	// Token: 0x020002DC RID: 732
	public interface IPlatformSpecificServiceDroid
	{
		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x060022F1 RID: 8945
		int NavigationBarColor { get; }

		// Token: 0x060022F2 RID: 8946
		void SetNavigationBarColor(Color forms_color);

		// Token: 0x060022F3 RID: 8947
		void SetNavigationBarColor(int icolor);

		// Token: 0x060022F4 RID: 8948
		void FileMigrator_MigrateBackups(IProgress<string> progress);

		// Token: 0x060022F5 RID: 8949
		void FileMigrator_MigrateRecords(IProgress<string> progress);

		// Token: 0x060022F6 RID: 8950
		void Log_Debug(string tag, string message);

		// Token: 0x060022F7 RID: 8951
		void AndroidHelper_StopService();

		// Token: 0x060022F8 RID: 8952
		void AndroidHelper_StartService();

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x060022F9 RID: 8953
		string GetExternalStoragePath { get; }

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x060022FA RID: 8954
		string Android_OS_Environment_MediaMounted { get; }

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x060022FB RID: 8955
		string Android_OS_Environment_ExternalStorageState { get; }

		// Token: 0x060022FC RID: 8956
		HttpMessageHandler GetNewDroid_http_BypassSslValidationClientHandler();

		// Token: 0x060022FD RID: 8957
		void KillApp();

		// Token: 0x060022FE RID: 8958
		Task<PermissionStatus> GetBluetoothStatusAndroid12Async();

		// Token: 0x060022FF RID: 8959
		Task<PermissionStatus> RequestBluetoothPermissionAndroid12Async();

		// Token: 0x06002300 RID: 8960
		Task AndroidHelper_RequestBluetoothPowerOn();

		// Token: 0x06002301 RID: 8961
		Task AndroidHelper_RequestBluetoothPowerOff();

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x06002302 RID: 8962
		int Window_NavigationBarColor { get; }

		// Token: 0x06002303 RID: 8963
		void Window_SetNavigationBarColor(Color xfcolor);

		// Token: 0x06002304 RID: 8964
		void Window_SetFullscreenOn();

		// Token: 0x06002305 RID: 8965
		void Window_SetFullscreenOff();

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x06002306 RID: 8966
		int Window_StatusBarColor { get; }

		// Token: 0x06002307 RID: 8967
		void Window_SetStatusBarColor(Color xfcolor);

		// Token: 0x06002308 RID: 8968
		void Window_SetStatusBarColor(int icolor);

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06002309 RID: 8969
		string DevicePermanentID_DeviceID { get; }

		// Token: 0x0600230A RID: 8970
		void Droid_InApp_RuKeyActivator_CheckKeyAsync(string key, bool register);

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x0600230B RID: 8971
		string Resources_Configuration_Locale_Country { get; }

		// Token: 0x0600230C RID: 8972
		bool PowerHacks_CanShowPowerManagerActivity();

		// Token: 0x0600230D RID: 8973
		void PowerHacks_LaunchPowerManagerActivity();

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x0600230E RID: 8974
		bool HasLocationPermission { get; }

		// Token: 0x0600230F RID: 8975
		void CheckLocationPermission();

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x06002310 RID: 8976
		bool HasStoragePermission { get; }

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06002311 RID: 8977
		bool HasBluetoothPermission { get; }

		// Token: 0x06002312 RID: 8978
		Task CustomKeyActivator_CheckKeyAsync(string key, Action success, Action fail);

		// Token: 0x06002313 RID: 8979
		Task<ValueTuple<ActivationRequestResult, int>> RuKeyActivator_CheckKeyAsync(string key, bool registerNewKey, string device = "");
	}
}
