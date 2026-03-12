using System;
using System.Globalization;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000065 RID: 101
	public class ConnectionTypeToIntConverter : IValueConverter
	{
		// Token: 0x0600024E RID: 590 RVA: 0x00017D04 File Offset: 0x00015F04
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ConnectionTypes connectionTypes = (ConnectionTypes)value;
			if (PlatformHelper.IsiOS)
			{
				switch (connectionTypes)
				{
				case ConnectionTypes.BluetoothLE:
					return 1;
				case ConnectionTypes.MFI_OBDLinkMXPlus:
					return 2;
				}
				return 0;
			}
			if (PlatformHelper.IsAndroid)
			{
				switch (connectionTypes)
				{
				case ConnectionTypes.WiFi:
					return 0;
				case ConnectionTypes.BluetoothLE:
					return 1;
				}
				return 2;
			}
			return (int)connectionTypes;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00017D88 File Offset: 0x00015F88
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = (int)value;
			if (PlatformHelper.IsiOS)
			{
				switch (num)
				{
				case 0:
					return ConnectionTypes.WiFi;
				case 1:
					return ConnectionTypes.BluetoothLE;
				case 2:
					return ConnectionTypes.MFI_OBDLinkMXPlus;
				}
			}
			else if (PlatformHelper.IsAndroid)
			{
				switch (num)
				{
				case 0:
					return ConnectionTypes.WiFi;
				case 1:
					return ConnectionTypes.BluetoothLE;
				}
				return ConnectionTypes.Bluetooth;
			}
			return (ConnectionTypes)num;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionTypeToIntConverter()
		{
		}
	}
}
