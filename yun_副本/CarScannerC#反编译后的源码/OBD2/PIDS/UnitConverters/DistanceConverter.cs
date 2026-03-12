using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x0200042B RID: 1067
	internal class DistanceConverter : AbstractUnitConverter
	{
		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06002D6C RID: 11628 RVA: 0x001FFD0E File Offset: 0x001FDF0E
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.meters;
			}
		}

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06002D6D RID: 11629 RVA: 0x001FFD12 File Offset: 0x001FDF12
		// (set) Token: 0x06002D6E RID: 11630 RVA: 0x001FFD1A File Offset: 0x001FDF1A
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
			UnitsHelper.Units.meters,
			UnitsHelper.Units.km,
			UnitsHelper.Units.miles,
			UnitsHelper.Units.cm,
			UnitsHelper.Units.mm,
			UnitsHelper.Units.feet,
			UnitsHelper.Units.inch
		};

		// Token: 0x06002D6F RID: 11631 RVA: 0x001FFD24 File Offset: 0x001FDF24
		protected override double ConvertToBaseUnit(double original_value, UnitsHelper.Units units_from)
		{
			if (units_from <= UnitsHelper.Units.feet)
			{
				if (units_from == UnitsHelper.Units.km)
				{
					return original_value * 1000.0;
				}
				if (units_from == UnitsHelper.Units.miles)
				{
					return original_value * 1609.34;
				}
				if (units_from == UnitsHelper.Units.feet)
				{
					return original_value * 0.3048;
				}
			}
			else
			{
				if (units_from == UnitsHelper.Units.mm)
				{
					return original_value * 0.001;
				}
				if (units_from == UnitsHelper.Units.cm)
				{
					return original_value * 0.01;
				}
				if (units_from == UnitsHelper.Units.inch)
				{
					return original_value * 0.0254;
				}
			}
			return original_value;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x001FFDA0 File Offset: 0x001FDFA0
		protected override double ConvertFromBaseUnit(double baseValue, UnitsHelper.Units units_to)
		{
			if (units_to <= UnitsHelper.Units.feet)
			{
				if (units_to == UnitsHelper.Units.km)
				{
					return baseValue / 1000.0;
				}
				if (units_to == UnitsHelper.Units.miles)
				{
					return baseValue / 1609.34;
				}
				if (units_to == UnitsHelper.Units.feet)
				{
					return baseValue / 0.3048;
				}
			}
			else
			{
				if (units_to == UnitsHelper.Units.mm)
				{
					return baseValue / 0.001;
				}
				if (units_to == UnitsHelper.Units.cm)
				{
					return baseValue / 0.01;
				}
				if (units_to == UnitsHelper.Units.inch)
				{
					return baseValue / 0.0254;
				}
			}
			return baseValue;
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x001FFE1B File Offset: 0x001FE01B
		public DistanceConverter()
		{
		}

		// Token: 0x040019B9 RID: 6585
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
