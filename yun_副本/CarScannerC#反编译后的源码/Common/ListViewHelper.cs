using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007ED RID: 2029
	public static class ListViewHelper
	{
		// Token: 0x06004723 RID: 18211 RVA: 0x0036D090 File Offset: 0x0036B290
		public static ITemplatedItemsList<Cell> GetCells(this ListView lv)
		{
			PropertyInfo propertyInfo = lv.GetType().GetRuntimeProperties().FirstOrDefault((PropertyInfo info) => info.Name == "TemplatedItems");
			if (propertyInfo != null)
			{
				return propertyInfo.GetValue(lv) as ITemplatedItemsList<Cell>;
			}
			return null;
		}

		// Token: 0x020007EE RID: 2030
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004724 RID: 18212 RVA: 0x0036D0E4 File Offset: 0x0036B2E4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004725 RID: 18213 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004726 RID: 18214 RVA: 0x0036D0F0 File Offset: 0x0036B2F0
			internal bool <GetCells>b__0_0(PropertyInfo info)
			{
				return info.Name == "TemplatedItems";
			}

			// Token: 0x04002975 RID: 10613
			public static readonly ListViewHelper.<>c <>9 = new ListViewHelper.<>c();

			// Token: 0x04002976 RID: 10614
			public static Func<PropertyInfo, bool> <>9__0_0;
		}
	}
}
