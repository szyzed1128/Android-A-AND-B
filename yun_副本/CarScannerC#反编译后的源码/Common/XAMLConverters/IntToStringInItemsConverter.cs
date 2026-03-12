using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200082E RID: 2094
	internal class IntToStringInItemsConverter : IValueConverter
	{
		// Token: 0x06004822 RID: 18466 RVA: 0x003704DC File Offset: 0x0036E6DC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IEnumerable<string> enumerable = (IEnumerable<string>)parameter;
			int num = (int)value;
			return enumerable.ElementAt(num);
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x003704FC File Offset: 0x0036E6FC
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IEnumerable<string> enumerable = (IEnumerable<string>)parameter;
			string text = (string)value;
			return enumerable.ToList<string>().IndexOf(text);
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x00002050 File Offset: 0x00000250
		public IntToStringInItemsConverter()
		{
		}
	}
}
