using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000838 RID: 2104
	internal class OBDReaderDisconnectedOrConnectedToELMorECUStatusToTrue : IValueConverter
	{
		// Token: 0x06004841 RID: 18497 RVA: 0x00370780 File Offset: 0x0036E980
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			OBDDataReaderStatus obddataReaderStatus = (OBDDataReaderStatus)value;
			if (obddataReaderStatus == OBDDataReaderStatus.ConnectedToECU || obddataReaderStatus == OBDDataReaderStatus.ConnectedToELM || obddataReaderStatus == OBDDataReaderStatus.Disconnected)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x00002050 File Offset: 0x00000250
		public OBDReaderDisconnectedOrConnectedToELMorECUStatusToTrue()
		{
		}
	}
}
