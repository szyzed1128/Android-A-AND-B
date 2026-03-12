using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000824 RID: 2084
	internal class EnumValueToFalseConverter : IValueConverter
	{
		// Token: 0x06004804 RID: 18436 RVA: 0x0036FFE8 File Offset: 0x0036E1E8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = (int)value;
			int num2 = 0;
			if (parameter is string)
			{
				num2 = int.Parse(parameter as string);
			}
			else if (parameter is int)
			{
				num2 = (int)parameter;
			}
			else if (parameter is double)
			{
				num2 = (int)((double)parameter);
			}
			if (num == num2)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x00002050 File Offset: 0x00000250
		public EnumValueToFalseConverter()
		{
		}
	}
}
