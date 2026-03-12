using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200006E RID: 110
	public class DTCReadingModeToIntConverter : IValueConverter
	{
		// Token: 0x06000269 RID: 617 RVA: 0x00002050 File Offset: 0x00000250
		public DTCReadingModeToIntConverter()
		{
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000180E3 File Offset: 0x000162E3
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DTCReadingMode)
			{
				return (int)((DTCReadingMode)value);
			}
			return 0;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000180FF File Offset: 0x000162FF
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int)
			{
				return (DTCReadingMode)value;
			}
			return DTCReadingMode.Auto;
		}
	}
}
