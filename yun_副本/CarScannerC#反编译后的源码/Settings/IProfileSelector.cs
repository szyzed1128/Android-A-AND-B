using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000200 RID: 512
	public interface IProfileSelector : INotifyPropertyChanged
	{
		// Token: 0x06001A4A RID: 6730
		void GoBack();

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001A4B RID: 6731
		// (remove) Token: 0x06001A4C RID: 6732
		event EventHandler<bool> WindowCloseRequested;

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x06001A4D RID: 6733
		// (set) Token: 0x06001A4E RID: 6734
		string TitleText { get; set; }

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06001A4F RID: 6735
		// (set) Token: 0x06001A50 RID: 6736
		bool TitleVisible { get; set; }

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06001A51 RID: 6737
		// (set) Token: 0x06001A52 RID: 6738
		bool BackButtonVisible { get; set; }

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06001A53 RID: 6739
		// (set) Token: 0x06001A54 RID: 6740
		bool CreateBackItem { get; set; }
	}
}
