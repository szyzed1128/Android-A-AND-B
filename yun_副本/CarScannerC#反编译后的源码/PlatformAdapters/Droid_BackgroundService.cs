using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms.PlatformAdapters
{
	// Token: 0x020002D8 RID: 728
	internal static class Droid_BackgroundService
	{
		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x001ADFEE File Offset: 0x001AC1EE
		public static Droid_IBackgroundService Instance
		{
			get
			{
				if (Device.RuntimePlatform == "Android" && Droid_BackgroundService._instance == null)
				{
					Droid_BackgroundService._instance = DependencyService.Get<Droid_IBackgroundService>(0);
					return Droid_BackgroundService._instance;
				}
				return null;
			}
		}

		// Token: 0x040010E4 RID: 4324
		private static Droid_IBackgroundService _instance;
	}
}
