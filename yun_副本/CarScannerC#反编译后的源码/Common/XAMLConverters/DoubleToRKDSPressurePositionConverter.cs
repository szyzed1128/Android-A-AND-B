using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200081F RID: 2079
	internal class DoubleToRKDSPressurePositionConverter : IValueConverter
	{
		// Token: 0x060047F6 RID: 18422 RVA: 0x0036FEA4 File Offset: 0x0036E0A4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string s = ((double)value).ToString("0.0", CultureInfo.InvariantCulture);
			string[] array = (string[])parameter;
			if (s == "25.5")
			{
				return 0;
			}
			return EnumerableExtensions.IndexOf<string>(array, (string x) => x == s);
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x0036FF0C File Offset: 0x0036E10C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = (int)value;
			string[] array = (string[])parameter;
			if (num == 0)
			{
				return 25.5;
			}
			return double.Parse(array[num], CultureInfo.InvariantCulture);
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x00002050 File Offset: 0x00000250
		public DoubleToRKDSPressurePositionConverter()
		{
		}

		// Token: 0x02000820 RID: 2080
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060047F9 RID: 18425 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060047FA RID: 18426 RVA: 0x0036FF4B File Offset: 0x0036E14B
			internal bool <Convert>b__0(string x)
			{
				return x == this.s;
			}

			// Token: 0x040029D8 RID: 10712
			public string s;
		}
	}
}
