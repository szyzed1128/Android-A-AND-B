using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000084 RID: 132
	public class TextSizeToPickerIdxConverter : IValueConverter
	{
		// Token: 0x060002AF RID: 687 RVA: 0x00018C18 File Offset: 0x00016E18
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is double))
			{
				return 2;
			}
			int num = (int)((double)value);
			if (num == TextSizeToPickerIdxConverter.micro)
			{
				return 0;
			}
			if (num == TextSizeToPickerIdxConverter.small)
			{
				return 1;
			}
			if (num == TextSizeToPickerIdxConverter.medium)
			{
				return 2;
			}
			if (num == TextSizeToPickerIdxConverter.large)
			{
				return 3;
			}
			if (num == TextSizeToPickerIdxConverter.large1_5)
			{
				return 4;
			}
			if (num == TextSizeToPickerIdxConverter.large2)
			{
				return 5;
			}
			return 2;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00018C9C File Offset: 0x00016E9C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is int))
			{
				return TextSizeToPickerIdxConverter.medium;
			}
			switch ((int)value)
			{
			case 0:
				return TextSizeToPickerIdxConverter.micro;
			case 1:
				return TextSizeToPickerIdxConverter.small;
			case 2:
				return TextSizeToPickerIdxConverter.medium;
			case 3:
				return TextSizeToPickerIdxConverter.large;
			case 4:
				return TextSizeToPickerIdxConverter.large1_5;
			case 5:
				return TextSizeToPickerIdxConverter.large2;
			default:
				return TextSizeToPickerIdxConverter.medium;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00002050 File Offset: 0x00000250
		public TextSizeToPickerIdxConverter()
		{
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00018D30 File Offset: 0x00016F30
		// Note: this type is marked as 'beforefieldinit'.
		static TextSizeToPickerIdxConverter()
		{
		}

		// Token: 0x040001B4 RID: 436
		private static int micro = (int)Device.GetNamedSize(1, typeof(Label));

		// Token: 0x040001B5 RID: 437
		private static int small = (int)Device.GetNamedSize(2, typeof(Label));

		// Token: 0x040001B6 RID: 438
		private static int medium = (int)Device.GetNamedSize(3, typeof(Label));

		// Token: 0x040001B7 RID: 439
		private static int large = (int)Device.GetNamedSize(4, typeof(Label));

		// Token: 0x040001B8 RID: 440
		private static int large1_5 = (int)(Device.GetNamedSize(4, typeof(Label)) * 1.5);

		// Token: 0x040001B9 RID: 441
		private static int large2 = (int)(Device.GetNamedSize(4, typeof(Label)) * 2.0);
	}
}
