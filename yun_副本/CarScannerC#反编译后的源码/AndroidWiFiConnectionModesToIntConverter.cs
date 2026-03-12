using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200005C RID: 92
	internal class AndroidWiFiConnectionModesToIntConverter : IValueConverter
	{
		// Token: 0x06000231 RID: 561 RVA: 0x0001796E File Offset: 0x00015B6E
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int)((AndroidWiFiConnectionModes)value);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0001797B File Offset: 0x00015B7B
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (AndroidWiFiConnectionModes)((int)value);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00002050 File Offset: 0x00000250
		public AndroidWiFiConnectionModesToIntConverter()
		{
		}
	}
}
