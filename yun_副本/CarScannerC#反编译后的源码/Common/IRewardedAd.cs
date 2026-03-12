using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007E6 RID: 2022
	public interface IRewardedAd : INotifyPropertyChanged
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060046FD RID: 18173
		// (remove) Token: 0x060046FE RID: 18174
		event EventHandler<KeyValuePair<string, int>> Rewarded;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x060046FF RID: 18175
		// (remove) Token: 0x06004700 RID: 18176
		event EventHandler RewardedVideoAdClosed;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06004701 RID: 18177
		// (remove) Token: 0x06004702 RID: 18178
		event EventHandler<int> RewardedVideoAdFailedToLoad;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06004703 RID: 18179
		// (remove) Token: 0x06004704 RID: 18180
		event EventHandler RewardedVideoAdLeftApplication;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06004705 RID: 18181
		// (remove) Token: 0x06004706 RID: 18182
		event EventHandler RewardedVideoAdLoaded;

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06004707 RID: 18183
		// (remove) Token: 0x06004708 RID: 18184
		event EventHandler RewardedVideoAdOpened;

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06004709 RID: 18185
		// (remove) Token: 0x0600470A RID: 18186
		event EventHandler RewardedVideoStarted;

		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x0600470B RID: 18187
		bool IsLoaded { get; }

		// Token: 0x0600470C RID: 18188
		void LoadAd();

		// Token: 0x0600470D RID: 18189
		void ShowAd();
	}
}
