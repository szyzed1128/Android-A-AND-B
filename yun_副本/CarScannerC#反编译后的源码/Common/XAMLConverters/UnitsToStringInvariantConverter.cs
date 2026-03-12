using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2.PIDS;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000847 RID: 2119
	internal class UnitsToStringInvariantConverter : IValueConverter
	{
		// Token: 0x0600486F RID: 18543 RVA: 0x00370B24 File Offset: 0x0036ED24
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
			return UnitsHelper.GetCaptionInvariant(units);
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x00002050 File Offset: 0x00000250
		public UnitsToStringInvariantConverter()
		{
		}
	}
}
