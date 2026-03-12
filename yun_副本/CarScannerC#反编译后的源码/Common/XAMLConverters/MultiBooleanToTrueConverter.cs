using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000833 RID: 2099
	internal class MultiBooleanToTrueConverter : IMultiValueConverter
	{
		// Token: 0x06004831 RID: 18481 RVA: 0x0037068B File Offset: 0x0036E88B
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.All((object x) => x is bool && (bool)x))
			{
				return true;
			}
			return false;
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x00002050 File Offset: 0x00000250
		public MultiBooleanToTrueConverter()
		{
		}

		// Token: 0x02000834 RID: 2100
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004834 RID: 18484 RVA: 0x003706C1 File Offset: 0x0036E8C1
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004835 RID: 18485 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004836 RID: 18486 RVA: 0x003706CD File Offset: 0x0036E8CD
			internal bool <Convert>b__0_0(object x)
			{
				return x is bool && (bool)x;
			}

			// Token: 0x040029D9 RID: 10713
			public static readonly MultiBooleanToTrueConverter.<>c <>9 = new MultiBooleanToTrueConverter.<>c();

			// Token: 0x040029DA RID: 10714
			public static Func<object, bool> <>9__0_0;
		}
	}
}
