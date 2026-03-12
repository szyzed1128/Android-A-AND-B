using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007EC RID: 2028
	public static class ListPool<T>
	{
		// Token: 0x0600471F RID: 18207 RVA: 0x0036D000 File Offset: 0x0036B200
		public static List<T> Rent()
		{
			ListPool<T>.RentCounter++;
			List<T> list;
			if (ListPool<T>._pool.TryTake(out list))
			{
				list.Clear();
				return list;
			}
			ListPool<T>.NewListCounter++;
			return new List<T>(128);
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x0036D045 File Offset: 0x0036B245
		public static void Return(List<T> list)
		{
			if (list == null)
			{
				return;
			}
			list.Clear();
			ListPool<T>._pool.Add(list);
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x0036D05C File Offset: 0x0036B25C
		public static void Clear()
		{
			List<T> list;
			while (ListPool<T>._pool.TryTake(out list))
			{
			}
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x0036D077 File Offset: 0x0036B277
		// Note: this type is marked as 'beforefieldinit'.
		static ListPool()
		{
		}

		// Token: 0x04002972 RID: 10610
		private static readonly ConcurrentBag<List<T>> _pool = new ConcurrentBag<List<T>>();

		// Token: 0x04002973 RID: 10611
		private static int NewListCounter = 0;

		// Token: 0x04002974 RID: 10612
		private static int RentCounter = 0;
	}
}
