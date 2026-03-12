using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000071 RID: 113
	public class EmptyStringToFalseConverter : IValueConverter
	{
		// Token: 0x06000274 RID: 628 RVA: 0x0001830E File Offset: 0x0001650E
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return false;
			}
			if (!(value is string))
			{
				return true;
			}
			if (string.IsNullOrEmpty(value as string))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00002050 File Offset: 0x00000250
		public EmptyStringToFalseConverter()
		{
		}
	}
}
