using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using Syncfusion.SfGauge.XForms;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000828 RID: 2088
	internal class FalseToVerticalMarginConverter : IValueConverter
	{
		// Token: 0x06004810 RID: 18448 RVA: 0x00370278 File Offset: 0x0036E478
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
			{
				return new Thickness(0.0, 0.0, 0.0, 0.0);
			}
			if ((bool)value)
			{
				return new Thickness(0.0, 0.0, 0.0, 0.0);
			}
			SfLinearGauge sfLinearGauge = parameter as SfLinearGauge;
			if (sfLinearGauge.BindingContext != null)
			{
				DashboardItem dashboardItem = sfLinearGauge.BindingContext as DashboardItem;
				double valueFontSize = dashboardItem.ValueFontSize;
				double titleFontSize = dashboardItem.TitleFontSize;
				return new Thickness(0.0, titleFontSize, 0.0, valueFontSize);
			}
			return default(Thickness);
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x00002050 File Offset: 0x00000250
		public FalseToVerticalMarginConverter()
		{
		}
	}
}
