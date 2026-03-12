using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x0200042D RID: 1069
	internal class ElectricalResistanceUnitConverter : AbstractUnitConverter
	{
		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06002D77 RID: 11639 RVA: 0x001FFE5F File Offset: 0x001FE05F
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.Ohm;
			}
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06002D78 RID: 11640 RVA: 0x001FFE63 File Offset: 0x001FE063
		// (set) Token: 0x06002D79 RID: 11641 RVA: 0x001FFE6B File Offset: 0x001FE06B
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
			UnitsHelper.Units.Ohm,
			UnitsHelper.Units.kOhm,
			UnitsHelper.Units.mOhm,
			UnitsHelper.Units.MOhm
		};

		// Token: 0x06002D7A RID: 11642 RVA: 0x001FFE74 File Offset: 0x001FE074
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			switch (units_from)
			{
			case UnitsHelper.Units.Ohm:
				break;
			case UnitsHelper.Units.kOhm:
				return value * 1000.0;
			case UnitsHelper.Units.MOhm:
				return value * 1000000.0;
			default:
				if (units_from == UnitsHelper.Units.mOhm)
				{
					return value / 1000.0;
				}
				break;
			}
			return value;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x001FFEC4 File Offset: 0x001FE0C4
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			switch (units_to)
			{
			case UnitsHelper.Units.Ohm:
				break;
			case UnitsHelper.Units.kOhm:
				return value / 1000.0;
			case UnitsHelper.Units.MOhm:
				return value / 1000000.0;
			default:
				if (units_to == UnitsHelper.Units.mOhm)
				{
					return value * 1000.0;
				}
				break;
			}
			return value;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x001FFF12 File Offset: 0x001FE112
		public ElectricalResistanceUnitConverter()
		{
		}

		// Token: 0x040019BB RID: 6587
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
