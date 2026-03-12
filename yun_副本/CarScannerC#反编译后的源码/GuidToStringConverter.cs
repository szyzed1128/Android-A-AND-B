using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000078 RID: 120
	public class GuidToStringConverter : IValueConverter
	{
		// Token: 0x06000289 RID: 649 RVA: 0x000185A4 File Offset: 0x000167A4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Guid)
			{
				return ((Guid)value).ToString();
			}
			return null;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00002050 File Offset: 0x00000250
		public GuidToStringConverter()
		{
		}
	}
}
