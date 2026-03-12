using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000837 RID: 2103
	internal class OBDReaderConnectedToECUStatusToTrue : IValueConverter
	{
		// Token: 0x0600483E RID: 18494 RVA: 0x00370767 File Offset: 0x0036E967
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((OBDDataReaderStatus)value == OBDDataReaderStatus.ConnectedToECU)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x00002050 File Offset: 0x00000250
		public OBDReaderConnectedToECUStatusToTrue()
		{
		}
	}
}
