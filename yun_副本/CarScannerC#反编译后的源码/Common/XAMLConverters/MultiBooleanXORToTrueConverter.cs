using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000835 RID: 2101
	internal class MultiBooleanXORToTrueConverter : IMultiValueConverter
	{
		// Token: 0x06004837 RID: 18487 RVA: 0x003706E0 File Offset: 0x0036E8E0
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.All((object x) => x is bool))
			{
				if (values.Any((object x) => (bool)x))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00002050 File Offset: 0x00000250
		public MultiBooleanXORToTrueConverter()
		{
		}

		// Token: 0x02000836 RID: 2102
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600483A RID: 18490 RVA: 0x00370748 File Offset: 0x0036E948
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600483B RID: 18491 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600483C RID: 18492 RVA: 0x00370754 File Offset: 0x0036E954
			internal bool <Convert>b__0_0(object x)
			{
				return x is bool;
			}

			// Token: 0x0600483D RID: 18493 RVA: 0x0037075F File Offset: 0x0036E95F
			internal bool <Convert>b__0_1(object x)
			{
				return (bool)x;
			}

			// Token: 0x040029DB RID: 10715
			public static readonly MultiBooleanXORToTrueConverter.<>c <>9 = new MultiBooleanXORToTrueConverter.<>c();

			// Token: 0x040029DC RID: 10716
			public static Func<object, bool> <>9__0_0;

			// Token: 0x040029DD RID: 10717
			public static Func<object, bool> <>9__0_1;
		}
	}
}
