using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200006A RID: 106
	public class DoubleToPercentConverter : IValueConverter
	{
		// Token: 0x0600025D RID: 605 RVA: 0x00017EBC File Offset: 0x000160BC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double && parameter is double)
			{
				double num = (double)value;
				double num2 = (double)parameter;
				return num * num2 / 100.0;
			}
			return 0.0;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00002050 File Offset: 0x00000250
		public DoubleToPercentConverter()
		{
		}
	}
}
