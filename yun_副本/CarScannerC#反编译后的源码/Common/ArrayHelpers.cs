using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007B9 RID: 1977
	internal static class ArrayHelpers
	{
		// Token: 0x0600463A RID: 17978 RVA: 0x00369364 File Offset: 0x00367564
		public static bool ArrayEquals<T>(T[] arr1, T[] arr2)
		{
			if (arr1 == null && arr2 == null)
			{
				return true;
			}
			if ((arr1 == null) | (arr2 == null))
			{
				return false;
			}
			if (arr1.Length != arr2.Length)
			{
				return false;
			}
			for (int i = 0; i < arr1.Length; i++)
			{
				T t = arr1[i];
				T t2 = arr2[i];
				if (t != null || t2 != null)
				{
					if ((arr1[i] == null) | (arr2[i] == null))
					{
						return false;
					}
					if (!t.Equals(t2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x003693FC File Offset: 0x003675FC
		public static bool ArrayEquals<T>(Collection<T> arr1, Collection<T> arr2)
		{
			if (arr1 == null && arr2 == null)
			{
				return true;
			}
			if ((arr1 == null) | (arr2 == null))
			{
				return false;
			}
			if (arr1.Count != arr2.Count)
			{
				return false;
			}
			for (int i = 0; i < arr1.Count; i++)
			{
				T t = arr1[i];
				T t2 = arr2[i];
				if (t != null || t2 != null)
				{
					if ((arr1[i] == null) | (arr2[i] == null))
					{
						return false;
					}
					if (!t.Equals(t2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0036949C File Offset: 0x0036769C
		public static bool ListEquals<T>(List<T> arr1, List<T> arr2)
		{
			if (arr1 == null && arr2 == null)
			{
				return true;
			}
			if ((arr1 == null) | (arr2 == null))
			{
				return false;
			}
			if (arr1.Count != arr2.Count)
			{
				return false;
			}
			for (int i = 0; i < arr1.Count; i++)
			{
				T t = arr1[i];
				T t2 = arr2[i];
				if (t != null || t2 != null)
				{
					if ((arr1[i] == null) | (arr2[i] == null))
					{
						return false;
					}
					if (!t.Equals(t2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x0036953A File Offset: 0x0036773A
		public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
		{
			ArrayHelpers.<Split>d__3<T> <Split>d__ = new ArrayHelpers.<Split>d__3<T>(-2);
			<Split>d__.<>3__array = array;
			<Split>d__.<>3__size = size;
			return <Split>d__;
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x00369551 File Offset: 0x00367751
		public static IEnumerable<T[]> SplitToArray<T>(this T[] array, int size)
		{
			ArrayHelpers.<SplitToArray>d__4<T> <SplitToArray>d__ = new ArrayHelpers.<SplitToArray>d__4<T>(-2);
			<SplitToArray>d__.<>3__array = array;
			<SplitToArray>d__.<>3__size = size;
			return <SplitToArray>d__;
		}

		// Token: 0x020007BA RID: 1978
		[CompilerGenerated]
		private sealed class <Split>d__3<T> : IEnumerable<IEnumerable<T>>, IEnumerable, IEnumerator<IEnumerable<T>>, IEnumerator, IDisposable
		{
			// Token: 0x0600463F RID: 17983 RVA: 0x00369568 File Offset: 0x00367768
			[DebuggerHidden]
			public <Split>d__3(int <>1__state)
			{
				this.<>1__state = <>1__state;
				this.<>l__initialThreadId = Environment.CurrentManagedThreadId;
			}

			// Token: 0x06004640 RID: 17984 RVA: 0x00369582 File Offset: 0x00367782
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				this.<>1__state = -2;
			}

			// Token: 0x06004641 RID: 17985 RVA: 0x0036958C File Offset: 0x0036778C
			bool IEnumerator.MoveNext()
			{
				int num = this.<>1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					this.<>1__state = -1;
					int num2 = this.<i>5__2;
					this.<i>5__2 = num2 + 1;
				}
				else
				{
					this.<>1__state = -1;
					this.<i>5__2 = 0;
				}
				if ((float)this.<i>5__2 >= (float)array.Length / (float)size)
				{
					return false;
				}
				this.<>2__current = array.Skip(this.<i>5__2 * size).Take(size);
				this.<>1__state = 1;
				return true;
			}

			// Token: 0x17001600 RID: 5632
			// (get) Token: 0x06004642 RID: 17986 RVA: 0x0036961D File Offset: 0x0036781D
			IEnumerable<T> IEnumerator<IEnumerable<T>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.<>2__current;
				}
			}

			// Token: 0x06004643 RID: 17987 RVA: 0x000D712D File Offset: 0x000D532D
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x17001601 RID: 5633
			// (get) Token: 0x06004644 RID: 17988 RVA: 0x0036961D File Offset: 0x0036781D
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.<>2__current;
				}
			}

			// Token: 0x06004645 RID: 17989 RVA: 0x00369628 File Offset: 0x00367828
			[DebuggerHidden]
			IEnumerator<IEnumerable<T>> IEnumerable<IEnumerable<T>>.GetEnumerator()
			{
				ArrayHelpers.<Split>d__3<T> <Split>d__;
				if (this.<>1__state == -2 && this.<>l__initialThreadId == Environment.CurrentManagedThreadId)
				{
					this.<>1__state = 0;
					<Split>d__ = this;
				}
				else
				{
					<Split>d__ = new ArrayHelpers.<Split>d__3<T>(0);
				}
				<Split>d__.array = array;
				<Split>d__.size = size;
				return <Split>d__;
			}

			// Token: 0x06004646 RID: 17990 RVA: 0x00369677 File Offset: 0x00367877
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>>.GetEnumerator();
			}

			// Token: 0x040028DC RID: 10460
			private int <>1__state;

			// Token: 0x040028DD RID: 10461
			private IEnumerable<T> <>2__current;

			// Token: 0x040028DE RID: 10462
			private int <>l__initialThreadId;

			// Token: 0x040028DF RID: 10463
			private T[] array;

			// Token: 0x040028E0 RID: 10464
			public T[] <>3__array;

			// Token: 0x040028E1 RID: 10465
			private int size;

			// Token: 0x040028E2 RID: 10466
			public int <>3__size;

			// Token: 0x040028E3 RID: 10467
			private int <i>5__2;
		}

		// Token: 0x020007BB RID: 1979
		[CompilerGenerated]
		private sealed class <SplitToArray>d__4<T> : IEnumerable<T[]>, IEnumerable, IEnumerator<T[]>, IEnumerator, IDisposable
		{
			// Token: 0x06004647 RID: 17991 RVA: 0x0036967F File Offset: 0x0036787F
			[DebuggerHidden]
			public <SplitToArray>d__4(int <>1__state)
			{
				this.<>1__state = <>1__state;
				this.<>l__initialThreadId = Environment.CurrentManagedThreadId;
			}

			// Token: 0x06004648 RID: 17992 RVA: 0x00369699 File Offset: 0x00367899
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				this.<>1__state = -2;
			}

			// Token: 0x06004649 RID: 17993 RVA: 0x003696A4 File Offset: 0x003678A4
			bool IEnumerator.MoveNext()
			{
				int num = this.<>1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					this.<>1__state = -1;
					int num2 = this.<i>5__2;
					this.<i>5__2 = num2 + 1;
				}
				else
				{
					this.<>1__state = -1;
					this.<i>5__2 = 0;
				}
				if ((float)this.<i>5__2 >= (float)array.Length / (float)size)
				{
					return false;
				}
				this.<>2__current = array.Skip(this.<i>5__2 * size).Take(size).ToArray<T>();
				this.<>1__state = 1;
				return true;
			}

			// Token: 0x17001602 RID: 5634
			// (get) Token: 0x0600464A RID: 17994 RVA: 0x0036973A File Offset: 0x0036793A
			T[] IEnumerator<T[]>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.<>2__current;
				}
			}

			// Token: 0x0600464B RID: 17995 RVA: 0x000D712D File Offset: 0x000D532D
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x17001603 RID: 5635
			// (get) Token: 0x0600464C RID: 17996 RVA: 0x0036973A File Offset: 0x0036793A
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.<>2__current;
				}
			}

			// Token: 0x0600464D RID: 17997 RVA: 0x00369744 File Offset: 0x00367944
			[DebuggerHidden]
			IEnumerator<T[]> IEnumerable<T[]>.GetEnumerator()
			{
				ArrayHelpers.<SplitToArray>d__4<T> <SplitToArray>d__;
				if (this.<>1__state == -2 && this.<>l__initialThreadId == Environment.CurrentManagedThreadId)
				{
					this.<>1__state = 0;
					<SplitToArray>d__ = this;
				}
				else
				{
					<SplitToArray>d__ = new ArrayHelpers.<SplitToArray>d__4<T>(0);
				}
				<SplitToArray>d__.array = array;
				<SplitToArray>d__.size = size;
				return <SplitToArray>d__;
			}

			// Token: 0x0600464E RID: 17998 RVA: 0x00369793 File Offset: 0x00367993
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T[]>.GetEnumerator();
			}

			// Token: 0x040028E4 RID: 10468
			private int <>1__state;

			// Token: 0x040028E5 RID: 10469
			private T[] <>2__current;

			// Token: 0x040028E6 RID: 10470
			private int <>l__initialThreadId;

			// Token: 0x040028E7 RID: 10471
			private T[] array;

			// Token: 0x040028E8 RID: 10472
			public T[] <>3__array;

			// Token: 0x040028E9 RID: 10473
			private int size;

			// Token: 0x040028EA RID: 10474
			public int <>3__size;

			// Token: 0x040028EB RID: 10475
			private int <i>5__2;
		}
	}
}
