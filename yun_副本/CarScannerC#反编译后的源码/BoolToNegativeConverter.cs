using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200005D RID: 93
	public class BoolToNegativeConverter : IValueConverter
	{
		// Token: 0x06000234 RID: 564 RVA: 0x00017988 File Offset: 0x00015B88
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				return !(bool)value;
			}
			return false;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00017988 File Offset: 0x00015B88
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				return !(bool)value;
			}
			return false;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00002050 File Offset: 0x00000250
		public BoolToNegativeConverter()
		{
		}
	}
}
