using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000073 RID: 115
	public class EmptyStringToTrueConverter : IValueConverter
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0001837A File Offset: 0x0001657A
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return false;
			}
			if (!(value is string))
			{
				return false;
			}
			if (string.IsNullOrEmpty(value as string))
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00002050 File Offset: 0x00000250
		public EmptyStringToTrueConverter()
		{
		}
	}
}
