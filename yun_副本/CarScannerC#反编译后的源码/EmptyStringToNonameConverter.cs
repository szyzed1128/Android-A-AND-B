using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000072 RID: 114
	public class EmptyStringToNonameConverter : IValueConverter
	{
		// Token: 0x06000277 RID: 631 RVA: 0x00018344 File Offset: 0x00016544
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string))
			{
				return string.Empty;
			}
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return Translate.GetString("ios_Noname");
			}
			return text;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00002050 File Offset: 0x00000250
		public EmptyStringToNonameConverter()
		{
		}
	}
}
