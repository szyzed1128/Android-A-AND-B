using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000842 RID: 2114
	internal class ThicknessToDoubleConverter : IValueConverter
	{
		// Token: 0x06004860 RID: 18528 RVA: 0x00370A38 File Offset: 0x0036EC38
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((Thickness)value).Left;
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x00370A58 File Offset: 0x0036EC58
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double)
			{
				return new Thickness((double)value);
			}
			if (value is int)
			{
				return new Thickness((double)((int)value));
			}
			return BindableProperty.UnsetValue;
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x00002050 File Offset: 0x00000250
		public ThicknessToDoubleConverter()
		{
		}
	}
}
