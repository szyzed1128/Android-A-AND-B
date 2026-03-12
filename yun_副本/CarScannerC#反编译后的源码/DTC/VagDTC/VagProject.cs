using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.DTC.VagDTC
{
	// Token: 0x0200058B RID: 1419
	internal class VagProject
	{
		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x060033DA RID: 13274 RVA: 0x00243FE7 File Offset: 0x002421E7
		// (set) Token: 0x060033DB RID: 13275 RVA: 0x00243FEF File Offset: 0x002421EF
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Name>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x060033DC RID: 13276 RVA: 0x00243FF8 File Offset: 0x002421F8
		// (set) Token: 0x060033DD RID: 13277 RVA: 0x00244000 File Offset: 0x00242200
		public int StartYear
		{
			[CompilerGenerated]
			get
			{
				return this.<StartYear>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StartYear>k__BackingField = value;
			}
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x060033DE RID: 13278 RVA: 0x00244009 File Offset: 0x00242209
		// (set) Token: 0x060033DF RID: 13279 RVA: 0x00244011 File Offset: 0x00242211
		public int EndYear
		{
			[CompilerGenerated]
			get
			{
				return this.<EndYear>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EndYear>k__BackingField = value;
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x060033E0 RID: 13280 RVA: 0x0024401A File Offset: 0x0024221A
		// (set) Token: 0x060033E1 RID: 13281 RVA: 0x00244022 File Offset: 0x00242222
		public List<string> ModelCodes
		{
			[CompilerGenerated]
			get
			{
				return this.<ModelCodes>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ModelCodes>k__BackingField = value;
			}
		} = new List<string>();

		// Token: 0x060033E2 RID: 13282 RVA: 0x0024402C File Offset: 0x0024222C
		public VagProject(string name, int startYear, int endYear, params string[] modelCodes)
		{
			this.Name = name;
			this.StartYear = startYear;
			this.EndYear = endYear;
			if (modelCodes != null)
			{
				this.ModelCodes.AddRange(modelCodes);
			}
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x0024407C File Offset: 0x0024227C
		public bool FitsModelCode(string modelCode)
		{
			return this.ModelCodes.Any((string x) => x == modelCode);
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x002440B2 File Offset: 0x002422B2
		public bool FitsModelYear(int year)
		{
			return year >= this.StartYear && year <= this.EndYear;
		}

		// Token: 0x04001E89 RID: 7817
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04001E8A RID: 7818
		[CompilerGenerated]
		private int <StartYear>k__BackingField;

		// Token: 0x04001E8B RID: 7819
		[CompilerGenerated]
		private int <EndYear>k__BackingField;

		// Token: 0x04001E8C RID: 7820
		[CompilerGenerated]
		private List<string> <ModelCodes>k__BackingField;

		// Token: 0x0200058C RID: 1420
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x060033E5 RID: 13285 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x060033E6 RID: 13286 RVA: 0x002440C9 File Offset: 0x002422C9
			internal bool <FitsModelCode>b__0(string x)
			{
				return x == this.modelCode;
			}

			// Token: 0x04001E8D RID: 7821
			public string modelCode;
		}
	}
}
