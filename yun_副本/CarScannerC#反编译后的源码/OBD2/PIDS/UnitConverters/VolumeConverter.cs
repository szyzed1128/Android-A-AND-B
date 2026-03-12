using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000439 RID: 1081
	internal class VolumeConverter : AbstractUnitConverter
	{
		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06002DBC RID: 11708 RVA: 0x00200840 File Offset: 0x001FEA40
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.liters;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06002DBD RID: 11709 RVA: 0x00200844 File Offset: 0x001FEA44
		// (set) Token: 0x06002DBE RID: 11710 RVA: 0x0020084C File Offset: 0x001FEA4C
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
			UnitsHelper.Units.liters,
			UnitsHelper.Units.gallons,
			UnitsHelper.Units.mm3
		};

		// Token: 0x06002DBF RID: 11711 RVA: 0x00200855 File Offset: 0x001FEA55
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from != UnitsHelper.Units.gallons)
			{
				if (units_from != UnitsHelper.Units.mm3)
				{
					return value;
				}
				return value * 1E-06;
			}
			else
			{
				if (!SharedSettings.Current.UseUSGallon)
				{
					return value * 4.54609;
				}
				return value * 3.78541;
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x00200894 File Offset: 0x001FEA94
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to != UnitsHelper.Units.gallons)
			{
				if (units_to != UnitsHelper.Units.mm3)
				{
					return value;
				}
				return value / 1E-06;
			}
			else
			{
				if (!SharedSettings.Current.UseUSGallon)
				{
					return value / 4.54609;
				}
				return value / 3.78541;
			}
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x002008D3 File Offset: 0x001FEAD3
		public VolumeConverter()
		{
		}

		// Token: 0x040019C6 RID: 6598
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
