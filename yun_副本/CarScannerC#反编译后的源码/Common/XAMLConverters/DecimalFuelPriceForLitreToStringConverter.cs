using System;
using System.Globalization;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200081C RID: 2076
	internal class DecimalFuelPriceForLitreToStringConverter : IValueConverter
	{
		// Token: 0x060047ED RID: 18413 RVA: 0x0036FD24 File Offset: 0x0036DF24
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			decimal num = (decimal)value;
			if (SharedSettings.Current.UseLitersForVolume)
			{
				return num.ToString("0.####");
			}
			decimal num2 = (SharedSettings.Current.UseUSGallon ? 0.264172m : 0.2199692m);
			return (num / num2).ToString("0.####");
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x0036FD90 File Offset: 0x0036DF90
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = (string)value;
			text = text.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			if (string.IsNullOrEmpty(text) || text.EndsWith(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator))
			{
				return Binding.DoNothing;
			}
			decimal num;
			if (!decimal.TryParse(text, out num))
			{
				return Binding.DoNothing;
			}
			decimal num2 = num;
			if (SharedSettings.Current.UseLitersForVolume)
			{
				return num2;
			}
			decimal num3 = (SharedSettings.Current.UseUSGallon ? 0.264172m : 0.2199692m);
			return num2 * num3;
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x00002050 File Offset: 0x00000250
		public DecimalFuelPriceForLitreToStringConverter()
		{
		}
	}
}
