using System;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Forms;

namespace CarScannerXamarinForms.InApp
{
	// Token: 0x02000472 RID: 1138
	internal static class InAppManager
	{
		// Token: 0x06002EE2 RID: 12002 RVA: 0x0020777D File Offset: 0x0020597D
		public static Page GetInAppPage()
		{
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				return new InAppPurchasePageRUS();
			}
			return new InAppPurchasePageV2();
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x00207792 File Offset: 0x00205992
		public static ICustomInAppManager GetInstance()
		{
			return PlatformHelper.CommonService.GetNewInAppManagerInstance();
		}
	}
}
