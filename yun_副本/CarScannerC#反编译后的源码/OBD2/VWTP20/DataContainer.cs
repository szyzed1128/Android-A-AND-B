using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.VWTP20
{
	// Token: 0x020003A1 RID: 929
	internal class DataContainer<T>
	{
		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x001E06E6 File Offset: 0x001DE8E6
		// (set) Token: 0x06002726 RID: 10022 RVA: 0x001E06EE File Offset: 0x001DE8EE
		public T Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x001E06F7 File Offset: 0x001DE8F7
		public DataContainer(T val)
		{
			this.Value = val;
		}

		// Token: 0x04001558 RID: 5464
		[CompilerGenerated]
		private T <Value>k__BackingField;
	}
}
