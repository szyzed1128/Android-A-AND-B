using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200006B RID: 107
	public class DoubleToStringConverter : IValueConverter
	{
		// Token: 0x06000260 RID: 608 RVA: 0x00017F08 File Offset: 0x00016108
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = "0.00";
			if (parameter != null && parameter is string)
			{
				text = (string)parameter;
			}
			if (value is double)
			{
				return ((double)value).ToString(text);
			}
			return 0.0;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00017F54 File Offset: 0x00016154
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string))
			{
				return BindableProperty.UnsetValue;
			}
			double num = 0.0;
			string text = value as string;
			text = text.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			if (string.IsNullOrEmpty(text) || text.EndsWith(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator))
			{
				return Binding.DoNothing;
			}
			if (double.TryParse(text, out num))
			{
				return num;
			}
			return BindableProperty.UnsetValue;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00002050 File Offset: 0x00000250
		public DoubleToStringConverter()
		{
		}
	}
}
