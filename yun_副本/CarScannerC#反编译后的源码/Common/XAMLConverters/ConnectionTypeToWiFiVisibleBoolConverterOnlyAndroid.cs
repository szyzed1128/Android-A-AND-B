using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000819 RID: 2073
	internal class ConnectionTypeToWiFiVisibleBoolConverterOnlyAndroid : IValueConverter
	{
		// Token: 0x060047E4 RID: 18404 RVA: 0x0036FC65 File Offset: 0x0036DE65
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (Device.RuntimePlatform != "Android")
			{
				return false;
			}
			if ((ConnectionTypes)value == ConnectionTypes.WiFi)
			{
				return true;
			}
			return false;
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionTypeToWiFiVisibleBoolConverterOnlyAndroid()
		{
		}
	}
}
