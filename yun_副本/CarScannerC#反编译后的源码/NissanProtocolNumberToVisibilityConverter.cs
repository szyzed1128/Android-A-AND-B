using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200007D RID: 125
	public class NissanProtocolNumberToVisibilityConverter : IValueConverter
	{
		// Token: 0x06000297 RID: 663 RVA: 0x000187C2 File Offset: 0x000169C2
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is int))
			{
				return false;
			}
			if ((int)value == 11)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00002050 File Offset: 0x00000250
		public NissanProtocolNumberToVisibilityConverter()
		{
		}
	}
}
