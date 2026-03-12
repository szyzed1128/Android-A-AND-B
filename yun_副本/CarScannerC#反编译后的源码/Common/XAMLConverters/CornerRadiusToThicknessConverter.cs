using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200081A RID: 2074
	internal class CornerRadiusToThicknessConverter : IValueConverter
	{
		// Token: 0x060047E7 RID: 18407 RVA: 0x0036FC94 File Offset: 0x0036DE94
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Thickness thickness = (Thickness)value;
			return new CornerRadius(thickness.Top, thickness.Right, thickness.Left, thickness.Bottom);
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0036FCD0 File Offset: 0x0036DED0
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			CornerRadius cornerRadius = (CornerRadius)value;
			return new Thickness(cornerRadius.BottomLeft, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight);
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x00002050 File Offset: 0x00000250
		public CornerRadiusToThicknessConverter()
		{
		}
	}
}
