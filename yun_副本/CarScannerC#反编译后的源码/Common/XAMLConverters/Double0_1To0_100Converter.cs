using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200081E RID: 2078
	internal class Double0_1To0_100Converter : IValueConverter
	{
		// Token: 0x060047F3 RID: 18419 RVA: 0x0036FE73 File Offset: 0x0036E073
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int)((double)value * 100.0);
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0036FE8B File Offset: 0x0036E08B
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (double)((int)value) / 100.0;
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x00002050 File Offset: 0x00000250
		public Double0_1To0_100Converter()
		{
		}
	}
}
