using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000083 RID: 131
	public class TextSizeToIdxConverter : IValueConverter
	{
		// Token: 0x060002AB RID: 683 RVA: 0x000189F8 File Offset: 0x00016BF8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is double))
			{
				return 2;
			}
			double num = (double)value;
			if (num.Equals3DigitPrecision(TextSizeToIdxConverter.micro))
			{
				return 0;
			}
			if (num.Equals3DigitPrecision(TextSizeToIdxConverter.small))
			{
				return 1;
			}
			if (num.Equals3DigitPrecision(TextSizeToIdxConverter.medium))
			{
				return 2;
			}
			if (num.Equals3DigitPrecision(TextSizeToIdxConverter.large))
			{
				return 3;
			}
			if (num.Equals3DigitPrecision(TextSizeToIdxConverter.large2))
			{
				return 4;
			}
			if (num.Equals3DigitPrecision(TextSizeToIdxConverter.large4))
			{
				return 5;
			}
			return 2;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00018A9C File Offset: 0x00016C9C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is double))
			{
				return TextSizeToIdxConverter.medium;
			}
			double num = (double)value;
			if (num.Equals3DigitPrecision(0.0))
			{
				return TextSizeToIdxConverter.micro;
			}
			if (num.Equals3DigitPrecision(1.0))
			{
				return TextSizeToIdxConverter.small;
			}
			if (num.Equals3DigitPrecision(2.0))
			{
				return TextSizeToIdxConverter.medium;
			}
			if (num.Equals3DigitPrecision(3.0))
			{
				return TextSizeToIdxConverter.large;
			}
			if (num.Equals3DigitPrecision(4.0))
			{
				return TextSizeToIdxConverter.large2;
			}
			if (num.Equals3DigitPrecision(5.0))
			{
				return TextSizeToIdxConverter.large4;
			}
			return TextSizeToIdxConverter.medium;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00002050 File Offset: 0x00000250
		public TextSizeToIdxConverter()
		{
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00018B78 File Offset: 0x00016D78
		// Note: this type is marked as 'beforefieldinit'.
		static TextSizeToIdxConverter()
		{
		}

		// Token: 0x040001AE RID: 430
		private static double micro = Device.GetNamedSize(1, typeof(Label));

		// Token: 0x040001AF RID: 431
		private static double small = Device.GetNamedSize(2, typeof(Label));

		// Token: 0x040001B0 RID: 432
		private static double medium = Device.GetNamedSize(3, typeof(Label));

		// Token: 0x040001B1 RID: 433
		private static double large = Device.GetNamedSize(4, typeof(Label));

		// Token: 0x040001B2 RID: 434
		private static double large2 = Device.GetNamedSize(4, typeof(Label)) * 1.5;

		// Token: 0x040001B3 RID: 435
		private static double large4 = Device.GetNamedSize(4, typeof(Label)) * 2.0;
	}
}
