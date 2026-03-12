using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2.PIDS;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000086 RID: 134
	public class UnitsToIntConverter : IValueConverter
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00018E29 File Offset: 0x00017029
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			return (int)((UnitsHelper.Units)value);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00018E3B File Offset: 0x0001703B
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			return (UnitsHelper.Units)((int)value);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00002050 File Offset: 0x00000250
		public UnitsToIntConverter()
		{
		}
	}
}
