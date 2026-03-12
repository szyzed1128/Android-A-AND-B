using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000840 RID: 2112
	internal class SkipCyclesIntToPriorityStringConverter : IValueConverter
	{
		// Token: 0x0600485A RID: 18522 RVA: 0x0037096C File Offset: 0x0036EB6C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return "1/" + ((int)value + 1).ToString();
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x00370993 File Offset: 0x0036EB93
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return int.Parse(((string)value).Substring(2)) - 1;
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x00002050 File Offset: 0x00000250
		public SkipCyclesIntToPriorityStringConverter()
		{
		}
	}
}
