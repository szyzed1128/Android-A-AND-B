using System;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200083A RID: 2106
	internal class PermissionStatusToColorConverter : IValueConverter
	{
		// Token: 0x06004847 RID: 18503 RVA: 0x003707C3 File Offset: 0x0036E9C3
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((PermissionStatus)value == 3)
			{
				return Application.Current.Resources["GreenTextColor"];
			}
			return Application.Current.Resources["RedTextColor"];
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x00002050 File Offset: 0x00000250
		public PermissionStatusToColorConverter()
		{
		}
	}
}
