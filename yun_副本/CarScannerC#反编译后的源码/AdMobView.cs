using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200018D RID: 397
	public class AdMobView : ContentView
	{
		// Token: 0x06001607 RID: 5639 RVA: 0x0000ADAB File Offset: 0x00008FAB
		public AdMobView()
		{
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0009BB21 File Offset: 0x00099D21
		public void Show()
		{
			base.IsVisible = true;
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0009BB2A File Offset: 0x00099D2A
		public void Hide()
		{
			base.IsVisible = false;
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x0009BB33 File Offset: 0x00099D33
		// (set) Token: 0x0600160B RID: 5643 RVA: 0x0009BB3B File Offset: 0x00099D3B
		public bool AdsLoaded
		{
			get
			{
				return this._AdsLoaded;
			}
			set
			{
				this._AdsLoaded = value;
				this.OnPropertyChanged("AdsLoaded");
			}
		}

		// Token: 0x0400065E RID: 1630
		private bool _AdsLoaded;
	}
}
