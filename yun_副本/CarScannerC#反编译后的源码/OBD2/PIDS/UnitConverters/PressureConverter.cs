using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000434 RID: 1076
	internal class PressureConverter : AbstractUnitConverter
	{
		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06002D9E RID: 11678 RVA: 0x002003A2 File Offset: 0x001FE5A2
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.Pa;
			}
		}

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06002D9F RID: 11679 RVA: 0x002003A6 File Offset: 0x001FE5A6
		// (set) Token: 0x06002DA0 RID: 11680 RVA: 0x002003AE File Offset: 0x001FE5AE
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
			UnitsHelper.Units.Pa,
			UnitsHelper.Units.kPa,
			UnitsHelper.Units.MPa,
			UnitsHelper.Units.hPa,
			UnitsHelper.Units.psi,
			UnitsHelper.Units.bar,
			UnitsHelper.Units.mbar
		};

		// Token: 0x06002DA1 RID: 11681 RVA: 0x002003B8 File Offset: 0x001FE5B8
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from <= UnitsHelper.Units.mbar)
			{
				if (units_from <= UnitsHelper.Units.psi)
				{
					if (units_from == UnitsHelper.Units.kPa)
					{
						return value * 1000.0;
					}
					if (units_from != UnitsHelper.Units.psi)
					{
						return value;
					}
					return value / 0.000145038;
				}
				else
				{
					if (units_from == UnitsHelper.Units.bar)
					{
						return value * 100000.0;
					}
					if (units_from != UnitsHelper.Units.mbar)
					{
						return value;
					}
				}
			}
			else if (units_from <= UnitsHelper.Units.MPa)
			{
				if (units_from != UnitsHelper.Units.hPa)
				{
					if (units_from != UnitsHelper.Units.MPa)
					{
						return value;
					}
					return value * 1000000.0;
				}
			}
			else
			{
				if (units_from == UnitsHelper.Units.atm)
				{
					return value * 101325.0;
				}
				if (units_from == UnitsHelper.Units.InHg)
				{
					return value * 3386.39;
				}
				if (units_from != UnitsHelper.Units.mmHg)
				{
					return value;
				}
				return value * 133.322;
			}
			return value * 100.0;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x00200470 File Offset: 0x001FE670
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to <= UnitsHelper.Units.mbar)
			{
				if (units_to <= UnitsHelper.Units.psi)
				{
					if (units_to == UnitsHelper.Units.kPa)
					{
						return value / 1000.0;
					}
					if (units_to != UnitsHelper.Units.psi)
					{
						return value;
					}
					return value * 0.000145038;
				}
				else
				{
					if (units_to == UnitsHelper.Units.bar)
					{
						return value / 100000.0;
					}
					if (units_to != UnitsHelper.Units.mbar)
					{
						return value;
					}
				}
			}
			else if (units_to <= UnitsHelper.Units.MPa)
			{
				if (units_to != UnitsHelper.Units.hPa)
				{
					if (units_to != UnitsHelper.Units.MPa)
					{
						return value;
					}
					return value / 1000000.0;
				}
			}
			else
			{
				if (units_to == UnitsHelper.Units.atm)
				{
					return value / 101325.0;
				}
				if (units_to == UnitsHelper.Units.InHg)
				{
					return value / 3386.39;
				}
				if (units_to != UnitsHelper.Units.mmHg)
				{
					return value;
				}
				return value / 133.322;
			}
			return value / 100.0;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x00200527 File Offset: 0x001FE727
		public PressureConverter()
		{
		}

		// Token: 0x040019C1 RID: 6593
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
