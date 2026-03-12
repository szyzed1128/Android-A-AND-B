using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000829 RID: 2089
	internal class FormatStringConverter : IValueConverter
	{
		// Token: 0x06004813 RID: 18451 RVA: 0x0037034C File Offset: 0x0036E54C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				string text = value as string;
				if (text != null)
				{
					if (parameter != null)
					{
						string text2 = parameter as string;
						if (text2 != null)
						{
							return string.Format(text2, text);
						}
					}
					return value;
				}
			}
			return "";
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x00002050 File Offset: 0x00000250
		public FormatStringConverter()
		{
		}
	}
}
