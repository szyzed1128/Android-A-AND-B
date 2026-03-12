using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.CarPlay
{
	// Token: 0x02000BF2 RID: 3058
	public interface ICarPlayDelegate
	{
		// Token: 0x06005BD8 RID: 23512
		CarPlayPages GetCurrentPage();

		// Token: 0x06005BD9 RID: 23513
		Task DisplayAlert(params string[] messageVariants);

		// Token: 0x06005BDA RID: 23514
		Task ShowNonDismissableAlert(string message);

		// Token: 0x06005BDB RID: 23515
		Task HideNonDismissableAlert();

		// Token: 0x06005BDC RID: 23516
		void UpdateStatusTab(KeyValuePair<string, string>[] newValues, ConnectButtonVisible button);

		// Token: 0x06005BDD RID: 23517
		void UpdateDashboardValues(KeyValuePair<string, string>[] newDashboardValues);

		// Token: 0x06005BDE RID: 23518
		void UpdateAcceleration(List<KeyValuePair<string, string>> newAccelerationValues, string speedTitle);

		// Token: 0x06005BDF RID: 23519
		void UpdateDashboardPagesList(List<KeyValuePair<string, int>> pages);

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06005BE0 RID: 23520
		// (remove) Token: 0x06005BE1 RID: 23521
		event EventHandler<int> UserSelectedDashboardPageIndex;

		// Token: 0x06005BE2 RID: 23522
		void OnUserSelectedDashboardPageIndex(int idx);

		// Token: 0x06005BE3 RID: 23523
		void OpenDashboard(string title);

		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x06005BE4 RID: 23524
		int MaxListItems { get; }

		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x06005BE5 RID: 23525
		int MaxTextItems { get; }
	}
}
