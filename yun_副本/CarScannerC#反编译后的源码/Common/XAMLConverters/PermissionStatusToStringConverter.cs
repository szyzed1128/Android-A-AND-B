using System;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200083B RID: 2107
	internal class PermissionStatusToStringConverter : IValueConverter
	{
		// Token: 0x0600484A RID: 18506 RVA: 0x003707F7 File Offset: 0x0036E9F7
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((PermissionStatus)value == 3)
			{
				return Translate.GetString("settings_Granted");
			}
			return Translate.GetString("settings_NotGranted");
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x00002050 File Offset: 0x00000250
		public PermissionStatusToStringConverter()
		{
		}
	}
}
