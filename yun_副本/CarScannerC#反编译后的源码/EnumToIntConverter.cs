using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000074 RID: 116
	public class EnumToIntConverter : IValueConverter
	{
		// Token: 0x0600027D RID: 637 RVA: 0x000183B0 File Offset: 0x000165B0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				obj = (int)value;
			}
			catch
			{
				obj = 0;
			}
			return obj;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000183E8 File Offset: 0x000165E8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				int num = (int)value;
				obj = Enum.ToObject(targetType, num);
			}
			catch
			{
				obj = Enum.ToObject(targetType, 0);
			}
			return obj;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00002050 File Offset: 0x00000250
		public EnumToIntConverter()
		{
		}
	}
}
