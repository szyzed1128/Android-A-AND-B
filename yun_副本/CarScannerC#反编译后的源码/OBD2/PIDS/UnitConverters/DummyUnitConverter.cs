using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x0200042C RID: 1068
	internal class DummyUnitConverter : IUnitConverter
	{
		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06002D72 RID: 11634 RVA: 0x001FFE3A File Offset: 0x001FE03A
		// (set) Token: 0x06002D73 RID: 11635 RVA: 0x001FFE42 File Offset: 0x001FE042
		public UnitsHelper.Units[] GetUnits
		{
			[CompilerGenerated]
			get
			{
				return this.<GetUnits>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<GetUnits>k__BackingField = value;
			}
		} = new UnitsHelper.Units[0];

		// Token: 0x06002D74 RID: 11636 RVA: 0x00002076 File Offset: 0x00000276
		public bool CanConvert(UnitsHelper.Units unit)
		{
			return false;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x00016849 File Offset: 0x00014A49
		public double Convert(double original_value, UnitsHelper.Units units_from, UnitsHelper.Units units_to)
		{
			return original_value;
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x001FFE4B File Offset: 0x001FE04B
		public DummyUnitConverter()
		{
		}

		// Token: 0x040019BA RID: 6586
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
