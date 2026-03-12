using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x02000803 RID: 2051
	public static class StringExtensions
	{
		// Token: 0x0600478E RID: 18318 RVA: 0x0036E966 File Offset: 0x0036CB66
		public static StringExtensions.LineSplitEnumerator SplitLines(this string str)
		{
			return new StringExtensions.LineSplitEnumerator(str.AsSpan());
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0036E973 File Offset: 0x0036CB73
		public static bool EqualsWithoutSpacesAndPunctuationTo(this string one, string other)
		{
			return StringExtensions.IsStringsEqualsWithoutSpacesAndPunctuation(one, other);
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0036E97C File Offset: 0x0036CB7C
		public static bool IsStringsEqualsWithoutSpacesAndPunctuation(string one, string two)
		{
			if (one == null && two == null)
			{
				return true;
			}
			if (one == null || two == null)
			{
				return false;
			}
			if (one.Length == two.Length)
			{
				return one.Equals(two, StringComparison.OrdinalIgnoreCase);
			}
			string text = new string(one.Where((char x) => !char.IsPunctuation(x) && !char.IsSeparator(x) && !char.IsControl(x) && x != ' ').ToArray<char>());
			string text2 = new string(two.Where((char x) => !char.IsPunctuation(x) && !char.IsSeparator(x) && !char.IsControl(x) && x != ' ').ToArray<char>());
			return text.Equals(text2, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x02000804 RID: 2052
		public ref struct LineSplitEnumerator
		{
			// Token: 0x06004791 RID: 18321 RVA: 0x0036EA1C File Offset: 0x0036CC1C
			public LineSplitEnumerator(ReadOnlySpan<char> str)
			{
				this._str = str;
				this.Current = default(StringExtensions.LineSplitEntry);
			}

			// Token: 0x06004792 RID: 18322 RVA: 0x0036EA3F File Offset: 0x0036CC3F
			public StringExtensions.LineSplitEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x06004793 RID: 18323 RVA: 0x0036EA48 File Offset: 0x0036CC48
			public unsafe bool MoveNext()
			{
				ReadOnlySpan<char> str = this._str;
				if (str.Length == 0)
				{
					return false;
				}
				int num = str.IndexOfAny('\r', '\n');
				if (num == -1)
				{
					this._str = ReadOnlySpan<char>.Empty;
					this.Current = new StringExtensions.LineSplitEntry(str, ReadOnlySpan<char>.Empty);
					return true;
				}
				if (num < str.Length - 1 && *str[num] == 13 && *str[num + 1] == 10)
				{
					this.Current = new StringExtensions.LineSplitEntry(str.Slice(0, num), str.Slice(num, 2));
					this._str = str.Slice(num + 2);
					return true;
				}
				this.Current = new StringExtensions.LineSplitEntry(str.Slice(0, num), str.Slice(num, 1));
				this._str = str.Slice(num + 1);
				return true;
			}

			// Token: 0x17001633 RID: 5683
			// (get) Token: 0x06004794 RID: 18324 RVA: 0x0036EB19 File Offset: 0x0036CD19
			// (set) Token: 0x06004795 RID: 18325 RVA: 0x0036EB21 File Offset: 0x0036CD21
			public StringExtensions.LineSplitEntry Current
			{
				[CompilerGenerated]
				readonly get
				{
					return this.<Current>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<Current>k__BackingField = value;
				}
			}

			// Token: 0x040029A8 RID: 10664
			private ReadOnlySpan<char> _str;

			// Token: 0x040029A9 RID: 10665
			[CompilerGenerated]
			private StringExtensions.LineSplitEntry <Current>k__BackingField;
		}

		// Token: 0x02000805 RID: 2053
		public readonly ref struct LineSplitEntry
		{
			// Token: 0x06004796 RID: 18326 RVA: 0x0036EB2A File Offset: 0x0036CD2A
			public LineSplitEntry(ReadOnlySpan<char> line, ReadOnlySpan<char> separator)
			{
				this.Line = line;
				this.Separator = separator;
			}

			// Token: 0x17001634 RID: 5684
			// (get) Token: 0x06004797 RID: 18327 RVA: 0x0036EB3A File Offset: 0x0036CD3A
			public ReadOnlySpan<char> Line
			{
				[CompilerGenerated]
				get
				{
					return this.<Line>k__BackingField;
				}
			}

			// Token: 0x17001635 RID: 5685
			// (get) Token: 0x06004798 RID: 18328 RVA: 0x0036EB42 File Offset: 0x0036CD42
			public ReadOnlySpan<char> Separator
			{
				[CompilerGenerated]
				get
				{
					return this.<Separator>k__BackingField;
				}
			}

			// Token: 0x06004799 RID: 18329 RVA: 0x0036EB4A File Offset: 0x0036CD4A
			public void Deconstruct(out ReadOnlySpan<char> line, out ReadOnlySpan<char> separator)
			{
				line = this.Line;
				separator = this.Separator;
			}

			// Token: 0x0600479A RID: 18330 RVA: 0x0036EB64 File Offset: 0x0036CD64
			public static implicit operator ReadOnlySpan<char>(StringExtensions.LineSplitEntry entry)
			{
				return entry.Line;
			}

			// Token: 0x040029AA RID: 10666
			[CompilerGenerated]
			private readonly ReadOnlySpan<char> <Line>k__BackingField;

			// Token: 0x040029AB RID: 10667
			[CompilerGenerated]
			private readonly ReadOnlySpan<char> <Separator>k__BackingField;
		}

		// Token: 0x02000806 RID: 2054
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600479B RID: 18331 RVA: 0x0036EB6D File Offset: 0x0036CD6D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600479C RID: 18332 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600479D RID: 18333 RVA: 0x0036EB79 File Offset: 0x0036CD79
			internal bool <IsStringsEqualsWithoutSpacesAndPunctuation>b__4_0(char x)
			{
				return !char.IsPunctuation(x) && !char.IsSeparator(x) && !char.IsControl(x) && x != ' ';
			}

			// Token: 0x0600479E RID: 18334 RVA: 0x0036EB79 File Offset: 0x0036CD79
			internal bool <IsStringsEqualsWithoutSpacesAndPunctuation>b__4_1(char x)
			{
				return !char.IsPunctuation(x) && !char.IsSeparator(x) && !char.IsControl(x) && x != ' ';
			}

			// Token: 0x040029AC RID: 10668
			public static readonly StringExtensions.<>c <>9 = new StringExtensions.<>c();

			// Token: 0x040029AD RID: 10669
			public static Func<char, bool> <>9__4_0;

			// Token: 0x040029AE RID: 10670
			public static Func<char, bool> <>9__4_1;
		}
	}
}
