using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007C7 RID: 1991
	public class DictionaryAsArrayResolver2 : DefaultContractResolver
	{
		// Token: 0x0600469B RID: 18075 RVA: 0x0036B0FC File Offset: 0x003692FC
		protected override JsonContract CreateContract(Type objectType)
		{
			if (objectType.GetInterfaces().Any((Type i) => i == typeof(IDictionary) || (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<, >))))
			{
				return base.CreateArrayContract(objectType);
			}
			return base.CreateContract(objectType);
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x0036B139 File Offset: 0x00369339
		public DictionaryAsArrayResolver2()
		{
		}

		// Token: 0x020007C8 RID: 1992
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600469D RID: 18077 RVA: 0x0036B141 File Offset: 0x00369341
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600469E RID: 18078 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600469F RID: 18079 RVA: 0x0036B14D File Offset: 0x0036934D
			internal bool <CreateContract>b__0_0(Type i)
			{
				return i == typeof(IDictionary) || (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<, >));
			}

			// Token: 0x04002901 RID: 10497
			public static readonly DictionaryAsArrayResolver2.<>c <>9 = new DictionaryAsArrayResolver2.<>c();

			// Token: 0x04002902 RID: 10498
			public static Func<Type, bool> <>9__0_0;
		}
	}
}
