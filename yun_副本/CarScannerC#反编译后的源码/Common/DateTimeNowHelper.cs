using System;
using CarScannerXamarinForms.PlatformAdapters;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007C6 RID: 1990
	public static class DateTimeNowHelper
	{
		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x0600469A RID: 18074 RVA: 0x0036B0C8 File Offset: 0x003692C8
		public static DateTime NowSafe
		{
			get
			{
				DateTime dateTime;
				try
				{
					dateTime = DateTime.Now;
				}
				catch (Exception)
				{
					dateTime = PlatformHelper.CommonService.PlatformNowDateTime;
				}
				return dateTime;
			}
		}
	}
}
