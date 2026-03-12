using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000818 RID: 2072
	internal class ConnectionTypeToBTVisibleBoolConverterOnlyForAndroid : IValueConverter
	{
		// Token: 0x060047E1 RID: 18401 RVA: 0x0036FC35 File Offset: 0x0036DE35
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (Device.RuntimePlatform != "Android")
			{
				return false;
			}
			if ((ConnectionTypes)value == ConnectionTypes.Bluetooth)
			{
				return true;
			}
			return false;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionTypeToBTVisibleBoolConverterOnlyForAndroid()
		{
		}
	}
}
