using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000085 RID: 133
	public class TimeSpanTotalIntSecondsToDoubleConverter : IValueConverter
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x00018DD8 File Offset: 0x00016FD8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is TimeSpan)
			{
				return ((TimeSpan)value).TotalSeconds;
			}
			return 15;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00018E08 File Offset: 0x00017008
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double)
			{
				return TimeSpan.FromSeconds((double)value);
			}
			return 1;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00002050 File Offset: 0x00000250
		public TimeSpanTotalIntSecondsToDoubleConverter()
		{
		}
	}
}
