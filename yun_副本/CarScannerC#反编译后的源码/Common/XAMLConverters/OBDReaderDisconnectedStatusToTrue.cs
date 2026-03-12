using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000839 RID: 2105
	internal class OBDReaderDisconnectedStatusToTrue : IValueConverter
	{
		// Token: 0x06004844 RID: 18500 RVA: 0x003707AC File Offset: 0x0036E9AC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((OBDDataReaderStatus)value == OBDDataReaderStatus.Disconnected)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x00002050 File Offset: 0x00000250
		public OBDReaderDisconnectedStatusToTrue()
		{
		}
	}
}
