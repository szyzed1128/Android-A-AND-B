using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007CA RID: 1994
	public struct DoubleValueItem
	{
		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x060046AE RID: 18094 RVA: 0x0036B3EC File Offset: 0x003695EC
		// (set) Token: 0x060046AF RID: 18095 RVA: 0x0036B3F4 File Offset: 0x003695F4
		public TimeSpan TimeAdded
		{
			[CompilerGenerated]
			readonly get
			{
				return this.<TimeAdded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<TimeAdded>k__BackingField = value;
			}
		}

		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x060046B0 RID: 18096 RVA: 0x0036B400 File Offset: 0x00369600
		public double SecondsAdded
		{
			get
			{
				return this.TimeAdded.TotalSeconds;
			}
		}

		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x060046B1 RID: 18097 RVA: 0x0036B41B File Offset: 0x0036961B
		// (set) Token: 0x060046B2 RID: 18098 RVA: 0x0036B423 File Offset: 0x00369623
		public double Value
		{
			[CompilerGenerated]
			readonly get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x0036B42C File Offset: 0x0036962C
		public DoubleValueItem(double Value, TimeSpan TimeAdded)
		{
			this.Value = Value;
			this.TimeAdded = TimeAdded;
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x0036B43C File Offset: 0x0036963C
		public override string ToString()
		{
			return this.Value.ToString(CultureInfo.InvariantCulture) + " (" + this.TimeAdded.ToString("c", CultureInfo.InvariantCulture) + ")";
		}

		// Token: 0x04002906 RID: 10502
		[CompilerGenerated]
		private TimeSpan <TimeAdded>k__BackingField;

		// Token: 0x04002907 RID: 10503
		[CompilerGenerated]
		private double <Value>k__BackingField;
	}
}
