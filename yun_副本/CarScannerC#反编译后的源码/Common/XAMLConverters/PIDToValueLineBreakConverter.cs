using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2.PIDS;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200083F RID: 2111
	public class PIDToValueLineBreakConverter : IValueConverter
	{
		// Token: 0x06004857 RID: 18519 RVA: 0x00370928 File Offset: 0x0036EB28
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return 0;
			}
			IPIDFloatValue ipidfloatValue = value as IPIDFloatValue;
			if (ipidfloatValue == null)
			{
				return 1;
			}
			if (string.IsNullOrEmpty(ipidfloatValue.TextValueVariants))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x00002050 File Offset: 0x00000250
		public PIDToValueLineBreakConverter()
		{
		}
	}
}
