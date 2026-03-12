using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200005F RID: 95
	public class BoolToSFGaugeOrientationConverter : IValueConverter
	{
		// Token: 0x0600023C RID: 572 RVA: 0x00017A58 File Offset: 0x00015C58
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00002050 File Offset: 0x00000250
		public BoolToSFGaugeOrientationConverter()
		{
		}
	}
}
