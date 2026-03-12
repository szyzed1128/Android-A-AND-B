using System;
using CarScannerXamarinForms.PlatformAdapters;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007D1 RID: 2001
	public static class Hardware
	{
		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x060046CA RID: 18122 RVA: 0x0036B769 File Offset: 0x00369969
		public static string DeviceID
		{
			get
			{
				return PlatformHelper.CommonService.DeviceID;
			}
		}
	}
}
