using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000432 RID: 1074
	internal class MassFlowConverter : AbstractUnitConverter
	{
		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06002D92 RID: 11666 RVA: 0x0020019E File Offset: 0x001FE39E
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.grams_sec;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06002D93 RID: 11667 RVA: 0x002001A1 File Offset: 0x001FE3A1
		// (set) Token: 0x06002D94 RID: 11668 RVA: 0x002001A9 File Offset: 0x001FE3A9
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
			UnitsHelper.Units.grams_sec,
			UnitsHelper.Units.kg_h,
			UnitsHelper.Units.kg_min,
			UnitsHelper.Units.grams_min,
			UnitsHelper.Units.lb_min,
			UnitsHelper.Units.lb_sec,
			UnitsHelper.Units.lb_h,
			UnitsHelper.Units.mg_hour
		};

		// Token: 0x06002D95 RID: 11669 RVA: 0x002001B4 File Offset: 0x001FE3B4
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from <= UnitsHelper.Units.kg_h)
			{
				if (units_from == UnitsHelper.Units.kg_min)
				{
					return value * 16.6667;
				}
				if (units_from == UnitsHelper.Units.kg_h)
				{
					return value / 3.6;
				}
			}
			else
			{
				switch (units_from)
				{
				case UnitsHelper.Units.grams_min:
					return value / 60.0;
				case UnitsHelper.Units.lb_min:
					return value * 7.55987;
				case UnitsHelper.Units.lb_sec:
					return value * 453.592;
				case UnitsHelper.Units.lb_h:
					return value * 0.125998;
				default:
					if (units_from == UnitsHelper.Units.mg_hour)
					{
						return value / 3600000.0;
					}
					if (units_from == UnitsHelper.Units.grams_hour)
					{
						return value / 3600.0;
					}
					break;
				}
			}
			return value;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x0020025C File Offset: 0x001FE45C
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to <= UnitsHelper.Units.kg_h)
			{
				if (units_to == UnitsHelper.Units.kg_min)
				{
					return value / 16.6667;
				}
				if (units_to == UnitsHelper.Units.kg_h)
				{
					return value * 3.6;
				}
			}
			else
			{
				switch (units_to)
				{
				case UnitsHelper.Units.grams_min:
					return value * 60.0;
				case UnitsHelper.Units.lb_min:
					return value / 7.55987;
				case UnitsHelper.Units.lb_sec:
					return value / 453.592;
				case UnitsHelper.Units.lb_h:
					return value / 0.125998;
				default:
					if (units_to == UnitsHelper.Units.mg_hour)
					{
						return value * 3600000.0;
					}
					if (units_to == UnitsHelper.Units.grams_hour)
					{
						return value * 3600.0;
					}
					break;
				}
			}
			return value;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x00200303 File Offset: 0x001FE503
		public MassFlowConverter()
		{
		}

		// Token: 0x040019BF RID: 6591
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
