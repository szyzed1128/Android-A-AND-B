using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000827 RID: 2087
	internal class ExcludeInfinityFromDoubleConverterAndLimitToRedLineStart : IValueConverter
	{
		// Token: 0x0600480D RID: 18445 RVA: 0x0037018C File Offset: 0x0036E38C
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
				double num2 = dashboardItem.Maximum;
				if (dashboardItem.GaugeShowRedLine)
				{
					num2 = dashboardItem.GaugeRedLineStart;
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

		// Token: 0x0600480E RID: 18446 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x00002050 File Offset: 0x00000250
		public ExcludeInfinityFromDoubleConverterAndLimitToRedLineStart()
		{
		}
	}
}
