using System;
using System.Globalization;
using System.Text;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000832 RID: 2098
	internal class LiveDataPidModelToStringNameValueUnits : IValueConverter
	{
		// Token: 0x0600482E RID: 18478 RVA: 0x003705FC File Offset: 0x0036E7FC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null)
			{
				return "";
			}
			LiveDataPIDModel liveDataPIDModel = (LiveDataPIDModel)parameter;
			StringBuilder stringBuilder = new StringBuilder(6);
			stringBuilder.Append(liveDataPIDModel.SelectedPID.ShortName);
			stringBuilder.Append(": ");
			stringBuilder.Append(liveDataPIDModel.FloatValue.ToString());
			if (!string.IsNullOrEmpty(liveDataPIDModel.Units))
			{
				stringBuilder.Append("[");
				stringBuilder.Append(liveDataPIDModel.Units);
				stringBuilder.Append("]");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x00002050 File Offset: 0x00000250
		public LiveDataPidModelToStringNameValueUnits()
		{
		}
	}
}
