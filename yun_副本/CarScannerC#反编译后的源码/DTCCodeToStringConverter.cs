using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200006C RID: 108
	internal class DTCCodeToStringConverter : IValueConverter
	{
		// Token: 0x06000263 RID: 611 RVA: 0x00017FF4 File Offset: 0x000161F4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (text == null)
			{
				return string.Empty;
			}
			if (text.Length == 7 && char.IsLetter(text[0]))
			{
				return text.Insert(5, "(") + ")";
			}
			return text;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00002050 File Offset: 0x00000250
		public DTCCodeToStringConverter()
		{
		}
	}
}
