using System;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007E7 RID: 2023
	public interface IUMPConsent
	{
		// Token: 0x0600470E RID: 18190
		void DisplayConsentIfRequired();

		// Token: 0x0600470F RID: 18191
		void ForceDisplayConsentForm();

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06004710 RID: 18192
		bool CanDisplayAds { get; }
	}
}
