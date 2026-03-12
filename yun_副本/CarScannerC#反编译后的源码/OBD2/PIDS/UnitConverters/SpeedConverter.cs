using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000435 RID: 1077
	internal class SpeedConverter : AbstractUnitConverter
	{
		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x00200546 File Offset: 0x001FE746
		// (set) Token: 0x06002DA5 RID: 11685 RVA: 0x0020054E File Offset: 0x001FE74E
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
			UnitsHelper.Units.kmh,
			UnitsHelper.Units.mph,
			UnitsHelper.Units.m_s
		};

		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06002DA6 RID: 11686 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.kmh;
			}
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x00200557 File Offset: 0x001FE757
		protected override double ConvertToBaseUnit(double original_value, UnitsHelper.Units units_from)
		{
			if (units_from == UnitsHelper.Units.mph)
			{
				return original_value * 1.60934;
			}
			if (units_from != UnitsHelper.Units.m_s)
			{
				return original_value;
			}
			return original_value * 3.6;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x0020057D File Offset: 0x001FE77D
		protected override double ConvertFromBaseUnit(double original_value, UnitsHelper.Units units_to)
		{
			if (units_to == UnitsHelper.Units.mph)
			{
				return original_value / 1.60934;
			}
			if (units_to != UnitsHelper.Units.m_s)
			{
				return original_value;
			}
			return original_value / 3.6;
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x002005A3 File Offset: 0x001FE7A3
		public SpeedConverter()
		{
		}

		// Token: 0x040019C2 RID: 6594
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
