using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200004D RID: 77
	public class DebugConverter : IValueConverter
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x00002050 File Offset: 0x00000250
		public DebugConverter()
		{
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00016849 File Offset: 0x00014A49
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00016849 File Offset: 0x00014A49
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
