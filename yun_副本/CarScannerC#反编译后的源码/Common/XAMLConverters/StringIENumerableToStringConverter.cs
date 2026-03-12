using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000841 RID: 2113
	internal class StringIENumerableToStringConverter : IValueConverter
	{
		// Token: 0x0600485D RID: 18525 RVA: 0x003709B0 File Offset: 0x0036EBB0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = "; ";
			if (parameter != null && parameter is string)
			{
				text = (string)parameter;
			}
			IEnumerable<string> enumerable = value as IEnumerable<string>;
			if (enumerable != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text2 in enumerable)
				{
					stringBuilder.Append(text2);
					stringBuilder.Append(text);
				}
				return stringBuilder.ToString();
			}
			return "";
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x00002050 File Offset: 0x00000250
		public StringIENumerableToStringConverter()
		{
		}
	}
}
