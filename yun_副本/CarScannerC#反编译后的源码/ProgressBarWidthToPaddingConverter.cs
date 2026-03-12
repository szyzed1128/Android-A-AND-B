using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000082 RID: 130
	public class ProgressBarWidthToPaddingConverter : IValueConverter
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x000189A0 File Offset: 0x00016BA0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double && parameter is double)
			{
				double num = (double)value;
				double num2 = (double)parameter;
				return new Thickness(num * num2 / 100.0, 0.0);
			}
			return default(Thickness);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00002050 File Offset: 0x00000250
		public ProgressBarWidthToPaddingConverter()
		{
		}
	}
}
