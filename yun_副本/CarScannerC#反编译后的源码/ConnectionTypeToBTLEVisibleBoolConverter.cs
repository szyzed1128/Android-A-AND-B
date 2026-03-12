using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000063 RID: 99
	public class ConnectionTypeToBTLEVisibleBoolConverter : IValueConverter
	{
		// Token: 0x06000248 RID: 584 RVA: 0x00017CD3 File Offset: 0x00015ED3
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((ConnectionTypes)value == ConnectionTypes.BluetoothLE)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionTypeToBTLEVisibleBoolConverter()
		{
		}
	}
}
