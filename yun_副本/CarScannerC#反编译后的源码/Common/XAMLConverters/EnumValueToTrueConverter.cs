using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000825 RID: 2085
	internal class EnumValueToTrueConverter : IValueConverter
	{
		// Token: 0x06004807 RID: 18439 RVA: 0x00370044 File Offset: 0x0036E244
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
				return true;
			}
			return false;
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x00002050 File Offset: 0x00000250
		public EnumValueToTrueConverter()
		{
		}
	}
}
