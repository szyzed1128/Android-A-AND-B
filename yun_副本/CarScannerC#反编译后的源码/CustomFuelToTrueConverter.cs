using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000068 RID: 104
	public class CustomFuelToTrueConverter : IValueConverter
	{
		// Token: 0x06000257 RID: 599 RVA: 0x00017E31 File Offset: 0x00016031
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is FuelTypes))
			{
				return false;
			}
			if ((FuelTypes)value == FuelTypes.Custom)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00002050 File Offset: 0x00000250
		public CustomFuelToTrueConverter()
		{
		}
	}
}
