using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200082B RID: 2091
	internal class IntMsToStringSecondsConverterWithNegativeWrongValue : IValueConverter
	{
		// Token: 0x06004819 RID: 18457 RVA: 0x003703C8 File Offset: 0x0036E5C8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value / 100).ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x003703EC File Offset: 0x0036E5EC
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return -2;
			}
			int num;
			if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
			{
				return num * 100;
			}
			return -1;
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x00002050 File Offset: 0x00000250
		public IntMsToStringSecondsConverterWithNegativeWrongValue()
		{
		}
	}
}
