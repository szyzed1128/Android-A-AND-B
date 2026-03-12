using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007EB RID: 2027
	public static class LinkedListPool<T>
	{
		// Token: 0x0600471B RID: 18203 RVA: 0x0036CF74 File Offset: 0x0036B174
		public static LinkedList<T> Rent()
		{
			LinkedListPool<T>.RentCounter++;
			LinkedList<T> linkedList;
			if (LinkedListPool<T>._pool.TryTake(out linkedList))
			{
				linkedList.Clear();
				return linkedList;
			}
			LinkedListPool<T>.NewListCounter++;
			return new LinkedList<T>();
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x0036CFB4 File Offset: 0x0036B1B4
		public static void Return(LinkedList<T> list)
		{
			if (list == null)
			{
				return;
			}
			list.Clear();
			LinkedListPool<T>._pool.Add(list);
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x0036CFCC File Offset: 0x0036B1CC
		public static void Clear()
		{
			LinkedList<T> linkedList;
			while (LinkedListPool<T>._pool.TryTake(out linkedList))
			{
			}
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x0036CFE7 File Offset: 0x0036B1E7
		// Note: this type is marked as 'beforefieldinit'.
		static LinkedListPool()
		{
		}

		// Token: 0x0400296F RID: 10607
		private static readonly ConcurrentBag<LinkedList<T>> _pool = new ConcurrentBag<LinkedList<T>>();

		// Token: 0x04002970 RID: 10608
		private static int NewListCounter = 0;

		// Token: 0x04002971 RID: 10609
		private static int RentCounter = 0;
	}
}
