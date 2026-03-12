using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200082F RID: 2095
	internal class IntZeroToBoolTrueConverter : IValueConverter
	{
		// Token: 0x06004825 RID: 18469 RVA: 0x00370526 File Offset: 0x0036E726
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((int)value == 0)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x00002050 File Offset: 0x00000250
		public IntZeroToBoolTrueConverter()
		{
		}
	}
}
