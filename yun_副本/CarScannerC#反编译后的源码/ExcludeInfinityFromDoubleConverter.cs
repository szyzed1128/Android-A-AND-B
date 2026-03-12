using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.DashboardGauges;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000075 RID: 117
	public class ExcludeInfinityFromDoubleConverter : IValueConverter
	{
		// Token: 0x06000280 RID: 640 RVA: 0x00018424 File Offset: 0x00016624
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is double))
			{
				return 0;
			}
			double num = (double)value;
			if (parameter != null && (parameter is GaugeItem || parameter is GaugeItemVar2 || parameter is GaugeItemVar3))
			{
				DashboardItem dashboardItem = (parameter as ContentView).BindingContext as DashboardItem;
				if (dashboardItem == null)
				{
					return 0.0;
				}
				if (double.IsInfinity(num))
				{
					return dashboardItem.Maximum;
				}
				if (double.IsNaN(num))
				{
					return dashboardItem.Minimum;
				}
				if (num < dashboardItem.Minimum)
				{
					return dashboardItem.Minimum;
				}
				if (num > dashboardItem.Maximum)
				{
					return dashboardItem.Maximum;
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

		// Token: 0x06000281 RID: 641 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00002050 File Offset: 0x00000250
		public ExcludeInfinityFromDoubleConverter()
		{
		}
	}
}
