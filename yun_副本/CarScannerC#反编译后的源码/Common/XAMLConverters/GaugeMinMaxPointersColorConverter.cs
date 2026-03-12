using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200082A RID: 2090
	public class GaugeMinMaxPointersColorConverter : IValueConverter
	{
		// Token: 0x06004816 RID: 18454 RVA: 0x00370384 File Offset: 0x0036E584
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null)
			{
				return value;
			}
			ContentView contentView = (ContentView)parameter;
			if (contentView.BindingContext == null)
			{
				return value;
			}
			if (!((DashboardItem)contentView.BindingContext).ShowMinMaxPointers)
			{
				return Color.Transparent;
			}
			return value;
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x00002050 File Offset: 0x00000250
		public GaugeMinMaxPointersColorConverter()
		{
		}
	}
}
