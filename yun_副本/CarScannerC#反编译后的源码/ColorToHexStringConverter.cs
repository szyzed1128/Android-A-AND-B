using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000062 RID: 98
	public class ColorToHexStringConverter : IValueConverter
	{
		// Token: 0x06000245 RID: 581 RVA: 0x00017BE4 File Offset: 0x00015DE4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Color color;
			if (value is Color)
			{
				color = (Color)value;
			}
			else
			{
				color = Color.Default;
			}
			int num = (int)(color.R * 255.0);
			int num2 = (int)(color.G * 255.0);
			int num3 = (int)(color.B * 255.0);
			return num.ToString("X2", CultureInfo.InvariantCulture) + num2.ToString("X2", CultureInfo.InvariantCulture) + num3.ToString("X2", CultureInfo.InvariantCulture);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00017C7C File Offset: 0x00015E7C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string))
			{
				return default(Color);
			}
			uint num;
			if (uint.TryParse((string)value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
			{
				return Color.FromUint(num);
			}
			return default(Color);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00002050 File Offset: 0x00000250
		public ColorToHexStringConverter()
		{
		}
	}
}
