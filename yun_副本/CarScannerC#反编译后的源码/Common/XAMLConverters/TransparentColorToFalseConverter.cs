using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000844 RID: 2116
	internal class TransparentColorToFalseConverter : IValueConverter
	{
		// Token: 0x06004866 RID: 18534 RVA: 0x00370AB3 File Offset: 0x0036ECB3
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Color)value == Color.Transparent)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x00002050 File Offset: 0x00000250
		public TransparentColorToFalseConverter()
		{
		}
	}
}
