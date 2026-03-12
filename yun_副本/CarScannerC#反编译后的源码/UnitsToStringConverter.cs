using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2.PIDS;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000087 RID: 135
	public class UnitsToStringConverter : IValueConverter
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x00018E50 File Offset: 0x00017050
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is UnitsHelper.Units))
			{
				return null;
			}
			UnitsHelper.Units units = (UnitsHelper.Units)value;
			if (units == UnitsHelper.Units.None)
			{
				return Translate.GetString("pid_Units_None");
			}
			return UnitsHelper.GetCaption(units);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00002050 File Offset: 0x00000250
		public UnitsToStringConverter()
		{
		}
	}
}
