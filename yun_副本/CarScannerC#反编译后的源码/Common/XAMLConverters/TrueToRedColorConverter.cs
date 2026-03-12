using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000846 RID: 2118
	internal class TrueToRedColorConverter : IValueConverter
	{
		// Token: 0x0600486C RID: 18540 RVA: 0x00370AEF File Offset: 0x0036ECEF
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return Application.Current.Resources["ButtonRedColor"];
			}
			return Application.Current.Resources["TextColor"];
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x00002050 File Offset: 0x00000250
		public TrueToRedColorConverter()
		{
		}
	}
}
