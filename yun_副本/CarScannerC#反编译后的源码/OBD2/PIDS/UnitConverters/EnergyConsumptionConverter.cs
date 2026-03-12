using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x0200042E RID: 1070
	internal class EnergyConsumptionConverter : AbstractUnitConverter
	{
		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06002D7D RID: 11645 RVA: 0x001FFF31 File Offset: 0x001FE131
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.km_kwh;
			}
		}

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06002D7E RID: 11646 RVA: 0x001FFF35 File Offset: 0x001FE135
		// (set) Token: 0x06002D7F RID: 11647 RVA: 0x001FFF3D File Offset: 0x001FE13D
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
			UnitsHelper.Units.km_kwh,
			UnitsHelper.Units.kWh_100km,
			UnitsHelper.Units.miles_per_kwh,
			UnitsHelper.Units.kwH_100miles,
			UnitsHelper.Units.MPGe
		};

		// Token: 0x06002D80 RID: 11648 RVA: 0x001FFF48 File Offset: 0x001FE148
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			switch (units_from)
			{
			case UnitsHelper.Units.kWh_100km:
				return 100.0 / value;
			case UnitsHelper.Units.miles_per_kwh:
				return 1.609344 * value;
			case UnitsHelper.Units.kwH_100miles:
				return 160.9344 / value;
			case UnitsHelper.Units.MPGe:
				return 0.047748 * value;
			default:
				return value;
			}
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x001FFFA4 File Offset: 0x001FE1A4
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			switch (units_to)
			{
			case UnitsHelper.Units.kWh_100km:
				return 100.0 / value;
			case UnitsHelper.Units.miles_per_kwh:
				return 0.621371 * value;
			case UnitsHelper.Units.kwH_100miles:
				return 160.9344 / value;
			case UnitsHelper.Units.MPGe:
				return 20.943316 * value;
			default:
				return value;
			}
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x001FFFFD File Offset: 0x001FE1FD
		public EnergyConsumptionConverter()
		{
		}

		// Token: 0x040019BC RID: 6588
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
