using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000064 RID: 100
	public class ConnectionTypeToBTVisibleBoolConverter : IValueConverter
	{
		// Token: 0x0600024B RID: 587 RVA: 0x00017CEB File Offset: 0x00015EEB
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((ConnectionTypes)value == ConnectionTypes.Bluetooth)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionTypeToBTVisibleBoolConverter()
		{
		}
	}
}
