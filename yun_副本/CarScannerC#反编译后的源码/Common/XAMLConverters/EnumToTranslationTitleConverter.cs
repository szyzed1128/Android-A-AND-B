using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000823 RID: 2083
	internal class EnumToTranslationTitleConverter : IValueConverter
	{
		// Token: 0x06004801 RID: 18433 RVA: 0x0036FF94 File Offset: 0x0036E194
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Enum))
			{
				return value.ToString();
			}
			Type type = value.GetType();
			string name = type.Name;
			string text = Enum.GetName(type, value).ToString();
			string @string = Translate.GetString(name + "." + text);
			if (string.IsNullOrEmpty(@string))
			{
				return text;
			}
			return @string;
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x00002050 File Offset: 0x00000250
		public EnumToTranslationTitleConverter()
		{
		}
	}
}
