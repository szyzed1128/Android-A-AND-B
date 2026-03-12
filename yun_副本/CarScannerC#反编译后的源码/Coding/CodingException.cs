using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200084C RID: 2124
	public class CodingException : Exception
	{
		// Token: 0x06004896 RID: 18582 RVA: 0x00370FED File Offset: 0x0036F1ED
		protected CodingException(string Message, ExceptionConsequences ExceptionConsequence)
			: base(Message)
		{
			this.ExceptionConsequence = ExceptionConsequence;
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x06004897 RID: 18583 RVA: 0x00370FFD File Offset: 0x0036F1FD
		// (set) Token: 0x06004898 RID: 18584 RVA: 0x00371005 File Offset: 0x0036F205
		public ExceptionConsequences ExceptionConsequence
		{
			[CompilerGenerated]
			get
			{
				return this.<ExceptionConsequence>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<ExceptionConsequence>k__BackingField = value;
			}
		}

		// Token: 0x040029E7 RID: 10727
		[CompilerGenerated]
		private ExceptionConsequences <ExceptionConsequence>k__BackingField;
	}
}
