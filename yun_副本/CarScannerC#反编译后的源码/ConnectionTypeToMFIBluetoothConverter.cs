using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000066 RID: 102
	public class ConnectionTypeToMFIBluetoothConverter : IValueConverter
	{
		// Token: 0x06000251 RID: 593 RVA: 0x00017E02 File Offset: 0x00016002
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((ConnectionTypes)value == ConnectionTypes.MFI_OBDLinkMXPlus)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00002050 File Offset: 0x00000250
		public ConnectionTypeToMFIBluetoothConverter()
		{
		}
	}
}
