using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000843 RID: 2115
	internal class TranparentColorToTrueConverter : IValueConverter
	{
		// Token: 0x06004863 RID: 18531 RVA: 0x00370A92 File Offset: 0x0036EC92
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Color)value == Color.Transparent)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x00002050 File Offset: 0x00000250
		public TranparentColorToTrueConverter()
		{
		}
	}
}
