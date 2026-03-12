using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000845 RID: 2117
	internal class TransparentColorToTransparentImagePathConverter : IValueConverter
	{
		// Token: 0x06004869 RID: 18537 RVA: 0x00370AD4 File Offset: 0x0036ECD4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Color)value == Color.Transparent)
			{
				return "transparent.png";
			}
			return null;
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x00002050 File Offset: 0x00000250
		public TransparentColorToTransparentImagePathConverter()
		{
		}
	}
}
