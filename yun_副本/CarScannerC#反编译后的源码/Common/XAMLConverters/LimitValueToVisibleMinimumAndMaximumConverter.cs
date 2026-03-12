using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.DashboardGauges;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000831 RID: 2097
	public class LimitValueToVisibleMinimumAndMaximumConverter : IValueConverter
	{
		// Token: 0x0600482B RID: 18475 RVA: 0x0037054C File Offset: 0x0036E74C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double num = (double)value;
			if (parameter is GaugeItem || parameter is LinearProgressItem)
			{
				DashboardItem dashboardItem = ((ContentView)parameter).BindingContext as DashboardItem;
				if (dashboardItem != null)
				{
					double num2 = dashboardItem.Maximum;
					double num3 = dashboardItem.Minimum;
					if (!double.IsFinite(num3))
					{
						num3 = 0.0;
					}
					if (!double.IsFinite(num2))
					{
						num2 = 0.0;
					}
					if (double.IsNaN(num))
					{
						return num3;
					}
					if (double.IsInfinity(num))
					{
						return num2;
					}
					if (num > num2)
					{
						return num2;
					}
					if (num < num3)
					{
						return num3;
					}
					return num;
				}
			}
			return num;
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x00002050 File Offset: 0x00000250
		public LimitValueToVisibleMinimumAndMaximumConverter()
		{
		}
	}
}
