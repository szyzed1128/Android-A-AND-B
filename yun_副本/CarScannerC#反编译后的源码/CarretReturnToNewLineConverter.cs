using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000061 RID: 97
	public class CarretReturnToNewLineConverter : IValueConverter
	{
		// Token: 0x06000242 RID: 578 RVA: 0x00017BD3 File Offset: 0x00015DD3
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((string)value).Replace('\r', '\n');
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00002050 File Offset: 0x00000250
		public CarretReturnToNewLineConverter()
		{
		}
	}
}
