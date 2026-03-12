using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200082D RID: 2093
	internal class IntToStringConverter : IValueConverter
	{
		// Token: 0x0600481F RID: 18463 RVA: 0x00370484 File Offset: 0x0036E684
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((double)((int)value)).ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x003704A8 File Offset: 0x0036E6A8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num;
			if (int.TryParse(value as string, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				return num;
			}
			return 0;
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x00002050 File Offset: 0x00000250
		public IntToStringConverter()
		{
		}
	}
}
