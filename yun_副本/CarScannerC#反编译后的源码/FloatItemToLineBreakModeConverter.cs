using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000076 RID: 118
	public class FloatItemToLineBreakModeConverter : IValueConverter
	{
		// Token: 0x06000283 RID: 643 RVA: 0x00018515 File Offset: 0x00016715
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
			{
				return 1;
			}
			if ((bool)value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00002050 File Offset: 0x00000250
		public FloatItemToLineBreakModeConverter()
		{
		}
	}
}
