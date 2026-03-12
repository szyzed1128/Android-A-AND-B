using System;
using System.Collections.Generic;
using System.ComponentModel;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.CarPlay
{
	// Token: 0x02000BF5 RID: 3061
	public interface ICarPlayManager : INotifyPropertyChanged
	{
		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x06005BE6 RID: 23526
		CarPlayPages CurrentPage { get; }

		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x06005BE7 RID: 23527
		bool IsConnected { get; }

		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x06005BE8 RID: 23528
		bool IsDelegateConnected { get; }

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06005BE9 RID: 23529
		// (remove) Token: 0x06005BEA RID: 23530
		event EventHandler Connected;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06005BEB RID: 23531
		// (remove) Token: 0x06005BEC RID: 23532
		event EventHandler Disconnected;

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06005BED RID: 23533
		// (remove) Token: 0x06005BEE RID: 23534
		event EventHandler<CarPlayPages> CarPlayPageChanged;

		// Token: 0x06005BEF RID: 23535
		void OnAccelerationConfigurationChanged();

		// Token: 0x06005BF0 RID: 23536
		void OnDashboardConfigurationUpdated();

		// Token: 0x06005BF1 RID: 23537
		void OnOBDStatusChanged(OBDDataReaderStatus status);

		// Token: 0x06005BF2 RID: 23538
		void UpdateRequests(List<OBDRequest> requests);

		// Token: 0x06005BF3 RID: 23539
		void OnDisconnectTapped();

		// Token: 0x06005BF4 RID: 23540
		void OnConnectTapped();

		// Token: 0x06005BF5 RID: 23541
		void ResetAcceleration();

		// Token: 0x06005BF6 RID: 23542
		void DisplayAlert(string message);

		// Token: 0x06005BF7 RID: 23543
		void OnDashboardPageSelectedIndex(object sender, int idx);

		// Token: 0x06005BF8 RID: 23544
		void OnVINLoaded(string VIN);

		// Token: 0x06005BF9 RID: 23545
		void DisplayNonDismissableAlert(string message);

		// Token: 0x06005BFA RID: 23546
		void HideNonDismissableAlert();

		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x06005BFB RID: 23547
		ICarPlayDelegate CarPlayDelegate { get; }

		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x06005BFC RID: 23548
		CarPlayDashboardModel DashboardModel { get; }

		// Token: 0x06005BFD RID: 23549
		KeyValuePair<string, string>[] GetDashboardValues();
	}
}
