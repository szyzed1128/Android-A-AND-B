using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200082C RID: 2092
	internal class IntToHexConverter : IValueConverter
	{
		// Token: 0x0600481C RID: 18460 RVA: 0x00370430 File Offset: 0x0036E630
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value).ToString("X2");
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x00370450 File Offset: 0x0036E650
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = (string)value;
			int num = 0;
			int.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num);
			return num;
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x00002050 File Offset: 0x00000250
		public IntToHexConverter()
		{
		}
	}
}
