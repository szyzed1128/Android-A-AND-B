using System;
using System.Globalization;
using CarScannerXamarinForms.Coding;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000830 RID: 2096
	internal class LightControlToIntConverter : IValueConverter
	{
		// Token: 0x06004828 RID: 18472 RVA: 0x0036FE59 File Offset: 0x0036E059
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int)value;
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x0037053D File Offset: 0x0036E73D
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (MQB_LightConfiguration.LightControl)value;
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x00002050 File Offset: 0x00000250
		public LightControlToIntConverter()
		{
		}
	}
}
