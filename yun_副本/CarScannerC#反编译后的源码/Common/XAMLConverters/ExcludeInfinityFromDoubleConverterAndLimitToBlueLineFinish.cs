using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000826 RID: 2086
	internal class ExcludeInfinityFromDoubleConverterAndLimitToBlueLineFinish : IValueConverter
	{
		// Token: 0x0600480A RID: 18442 RVA: 0x003700A0 File Offset: 0x0036E2A0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is double))
			{
				return 0;
			}
			double num = (double)value;
			if (parameter != null && parameter is ContentView)
			{
				DashboardItem dashboardItem = (parameter as ContentView).BindingContext as DashboardItem;
				if (dashboardItem == null)
				{
					return 0.0;
				}
				double num2 = dashboardItem.Minimum;
				if (dashboardItem.GaugeShowBlueLine)
				{
					num2 = dashboardItem.GaugeBlueLineFinish;
				}
				if (double.IsInfinity(num))
				{
					return num2;
				}
				if (double.IsNaN(num))
				{
					return dashboardItem.Minimum;
				}
				if (num < dashboardItem.Minimum)
				{
					return dashboardItem.Minimum;
				}
				if (num > num2)
				{
					return num2;
				}
				return num;
			}
			else
			{
				if (double.IsInfinity(num) || double.IsNaN(num) || double.IsPositiveInfinity(num) || double.IsNegativeInfinity(num))
				{
					return int.MaxValue;
				}
				return num;
			}
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x00002050 File Offset: 0x00000250
		public ExcludeInfinityFromDoubleConverterAndLimitToBlueLineFinish()
		{
		}
	}
}
