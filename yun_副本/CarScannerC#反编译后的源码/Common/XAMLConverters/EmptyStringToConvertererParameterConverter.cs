using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000822 RID: 2082
	internal class EmptyStringToConvertererParameterConverter : IValueConverter
	{
		// Token: 0x060047FE RID: 18430 RVA: 0x0036FF82 File Offset: 0x0036E182
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(value as string))
			{
				return parameter;
			}
			return value;
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x00002050 File Offset: 0x00000250
		public EmptyStringToConvertererParameterConverter()
		{
		}
	}
}
