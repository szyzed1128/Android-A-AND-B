using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000067 RID: 103
	public class ConnectionTypeToWiFiVisibleBoolConverter : IValueConverter
	{
		// Token: 0x06000254 RID: 596 RVA: 0x00017E1A File Offset: 0x0001601A
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((ConnectionTypes)value == ConnectionTypes.WiFi)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionTypeToWiFiVisibleBoolConverter()
		{
		}
	}
}
