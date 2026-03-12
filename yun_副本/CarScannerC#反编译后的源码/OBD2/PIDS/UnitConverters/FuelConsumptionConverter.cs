using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x0200042F RID: 1071
	internal class FuelConsumptionConverter : AbstractUnitConverter
	{
		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06002D83 RID: 11651 RVA: 0x0020001C File Offset: 0x001FE21C
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.liters100km;
			}
		}

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06002D84 RID: 11652 RVA: 0x00200020 File Offset: 0x001FE220
		// (set) Token: 0x06002D85 RID: 11653 RVA: 0x00200028 File Offset: 0x001FE228
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
			UnitsHelper.Units.liters100km,
			UnitsHelper.Units.MPG,
			UnitsHelper.Units.km_liter
		};

		// Token: 0x06002D86 RID: 11654 RVA: 0x00200034 File Offset: 0x001FE234
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from != UnitsHelper.Units.MPG)
			{
				if (units_from != UnitsHelper.Units.km_liter)
				{
					return value;
				}
				if (value == 0.0)
				{
					return double.PositiveInfinity;
				}
				return 100.0 / value;
			}
			else
			{
				if (value == 0.0)
				{
					return double.PositiveInfinity;
				}
				if (!SharedSettings.Current.UseUSGallon)
				{
					return 282.481 / value;
				}
				return 235.215 / value;
			}
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x002000A8 File Offset: 0x001FE2A8
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to != UnitsHelper.Units.MPG)
			{
				if (units_to != UnitsHelper.Units.km_liter)
				{
					return value;
				}
				if (double.IsInfinity(value))
				{
					return 0.0;
				}
				return 100.0 * value;
			}
			else
			{
				if (double.IsInfinity(value))
				{
					return 0.0;
				}
				if (!SharedSettings.Current.UseUSGallon)
				{
					return 282.481 * value;
				}
				return 235.215 * value;
			}
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x00200116 File Offset: 0x001FE316
		public FuelConsumptionConverter()
		{
		}

		// Token: 0x040019BD RID: 6589
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
