using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000077 RID: 119
	public class FloatValueToColorConverter : IValueConverter
	{
		// Token: 0x06000286 RID: 646 RVA: 0x0001853C File Offset: 0x0001673C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DashboardItem dashboardItem = (parameter as ContentView).BindingContext as DashboardItem;
			if (dashboardItem.GaugeShowRedLine && dashboardItem.Model != null && dashboardItem.Model.ChartVisible && dashboardItem.Model.FloatValue >= dashboardItem.GaugeRedLineStart)
			{
				return dashboardItem.GaugeRedLineColor;
			}
			return dashboardItem.ValueNormalTextColor;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00002050 File Offset: 0x00000250
		public FloatValueToColorConverter()
		{
		}
	}
}
