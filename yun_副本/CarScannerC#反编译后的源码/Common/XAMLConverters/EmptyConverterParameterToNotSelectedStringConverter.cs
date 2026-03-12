using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000821 RID: 2081
	internal class EmptyConverterParameterToNotSelectedStringConverter : IMultiValueConverter
	{
		// Token: 0x060047FB RID: 18427 RVA: 0x0036FF59 File Offset: 0x0036E159
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values[1] == null || values[1] as string == string.Empty)
			{
				return Translate.GetString("settings_NotSelected");
			}
			return values[0];
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x00002050 File Offset: 0x00000250
		public EmptyConverterParameterToNotSelectedStringConverter()
		{
		}
	}
}
