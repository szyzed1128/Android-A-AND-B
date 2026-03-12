using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000433 RID: 1075
	internal class PowerConverter : AbstractUnitConverter
	{
		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06002D98 RID: 11672 RVA: 0x00200322 File Offset: 0x001FE522
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.W;
			}
		}

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06002D99 RID: 11673 RVA: 0x00200326 File Offset: 0x001FE526
		// (set) Token: 0x06002D9A RID: 11674 RVA: 0x0020032E File Offset: 0x001FE52E
		public override UnitsHelper.Units[] GetUnits
		{
			[CompilerGenerated]
			get
			{
				return this.<GetUnits>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<GetUnits>k__BackingField = value;
			}
		} = new UnitsHelper.Units[]
		{
			UnitsHelper.Units.W,
			UnitsHelper.Units.kW,
			UnitsHelper.Units.hp
		};

		// Token: 0x06002D9B RID: 11675 RVA: 0x00200337 File Offset: 0x001FE537
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from == UnitsHelper.Units.hp)
			{
				return value * 745.7;
			}
			if (units_from != UnitsHelper.Units.kW)
			{
				return value;
			}
			return value * 1000.0;
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x0020035E File Offset: 0x001FE55E
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to == UnitsHelper.Units.hp)
			{
				return value / 745.7;
			}
			if (units_to == UnitsHelper.Units.kW)
			{
				return value / 1000.0;
			}
			return value;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x00200383 File Offset: 0x001FE583
		public PowerConverter()
		{
		}

		// Token: 0x040019C0 RID: 6592
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
