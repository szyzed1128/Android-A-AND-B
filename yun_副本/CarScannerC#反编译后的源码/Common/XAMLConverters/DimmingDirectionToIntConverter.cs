using System;
using System.Globalization;
using CarScannerXamarinForms.Coding;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200081D RID: 2077
	internal class DimmingDirectionToIntConverter : IValueConverter
	{
		// Token: 0x060047F0 RID: 18416 RVA: 0x0036FE59 File Offset: 0x0036E059
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int)value;
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x0036FE66 File Offset: 0x0036E066
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (MQB_LightConfiguration.DimmingDirection)value;
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x00002050 File Offset: 0x00000250
		public DimmingDirectionToIntConverter()
		{
		}
	}
}
