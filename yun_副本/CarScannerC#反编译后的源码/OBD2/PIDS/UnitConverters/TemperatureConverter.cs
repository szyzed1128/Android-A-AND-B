using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000436 RID: 1078
	internal class TemperatureConverter : AbstractUnitConverter
	{
		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06002DAA RID: 11690 RVA: 0x002005C2 File Offset: 0x001FE7C2
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.celicium;
			}
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06002DAB RID: 11691 RVA: 0x002005C6 File Offset: 0x001FE7C6
		// (set) Token: 0x06002DAC RID: 11692 RVA: 0x002005CE File Offset: 0x001FE7CE
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
			UnitsHelper.Units.celicium,
			UnitsHelper.Units.fahrengheit,
			UnitsHelper.Units.Kelvin
		};

		// Token: 0x06002DAD RID: 11693 RVA: 0x002005D7 File Offset: 0x001FE7D7
		protected override double ConvertToBaseUnit(double original_value, UnitsHelper.Units units_from)
		{
			if (units_from == UnitsHelper.Units.fahrengheit)
			{
				return (original_value - 32.0) * 5.0 / 9.0;
			}
			if (units_from != UnitsHelper.Units.Kelvin)
			{
				return original_value;
			}
			return original_value - 273.15;
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x00200612 File Offset: 0x001FE812
		protected override double ConvertFromBaseUnit(double original_value, UnitsHelper.Units units_to)
		{
			if (units_to == UnitsHelper.Units.fahrengheit)
			{
				return original_value * 9.0 / 5.0 + 32.0;
			}
			if (units_to != UnitsHelper.Units.Kelvin)
			{
				return original_value;
			}
			return original_value + 273.15;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x0020064D File Offset: 0x001FE84D
		public TemperatureConverter()
		{
		}

		// Token: 0x040019C3 RID: 6595
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
