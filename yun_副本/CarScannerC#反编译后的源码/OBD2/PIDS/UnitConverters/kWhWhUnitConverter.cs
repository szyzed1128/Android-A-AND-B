using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000431 RID: 1073
	internal class kWhWhUnitConverter : AbstractUnitConverter
	{
		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06002D8C RID: 11660 RVA: 0x00200135 File Offset: 0x001FE335
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.Wh;
			}
		}

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06002D8D RID: 11661 RVA: 0x00200139 File Offset: 0x001FE339
		// (set) Token: 0x06002D8E RID: 11662 RVA: 0x00200141 File Offset: 0x001FE341
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
			UnitsHelper.Units.Wh,
			UnitsHelper.Units.kWh
		};

		// Token: 0x06002D8F RID: 11663 RVA: 0x0020014A File Offset: 0x001FE34A
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from != UnitsHelper.Units.kWh)
			{
				if (units_from != UnitsHelper.Units.Wh)
				{
				}
				return value;
			}
			return value * 1000.0;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x00200165 File Offset: 0x001FE365
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to != UnitsHelper.Units.kWh)
			{
				if (units_to != UnitsHelper.Units.Wh)
				{
				}
				return value;
			}
			return value / 1000.0;
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x00200180 File Offset: 0x001FE380
		public kWhWhUnitConverter()
		{
		}

		// Token: 0x040019BE RID: 6590
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
