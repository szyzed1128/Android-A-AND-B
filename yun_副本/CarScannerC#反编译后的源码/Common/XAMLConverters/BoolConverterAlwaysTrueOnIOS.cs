using System;
using System.Globalization;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000812 RID: 2066
	public class BoolConverterAlwaysTrueOnIOS : IValueConverter
	{
		// Token: 0x060047C7 RID: 18375 RVA: 0x0036F80E File Offset: 0x0036DA0E
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (PlatformHelper.IsiOS)
			{
				return true;
			}
			return value;
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x00002050 File Offset: 0x00000250
		public BoolConverterAlwaysTrueOnIOS()
		{
		}
	}
}
