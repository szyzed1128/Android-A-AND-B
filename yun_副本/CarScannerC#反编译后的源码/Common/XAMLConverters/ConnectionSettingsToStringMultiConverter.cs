using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000817 RID: 2071
	internal class ConnectionSettingsToStringMultiConverter : IMultiValueConverter
	{
		// Token: 0x060047DE RID: 18398 RVA: 0x0036FB80 File Offset: 0x0036DD80
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			switch (SharedSettings.Current.ConnectionType)
			{
			case ConnectionTypes.WiFi:
				return "Wi-Fi: " + SharedSettings.Current.WiFiServer + ":" + SharedSettings.Current.WiFiPort;
			case ConnectionTypes.BluetoothLE:
				if (string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceName))
				{
					return "Bluetooth LE (4.0)";
				}
				return "Bluetooth LE (4.0): " + SharedSettings.Current.BTLEDeviceName;
			case ConnectionTypes.Bluetooth:
			case ConnectionTypes.MFI_OBDLinkMXPlus:
				if (string.IsNullOrEmpty(SharedSettings.Current.BTDeviceName))
				{
					return "Bluetooth";
				}
				return "Bluetooth: " + SharedSettings.Current.BTDeviceName;
			}
			return "";
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionSettingsToStringMultiConverter()
		{
		}
	}
}
