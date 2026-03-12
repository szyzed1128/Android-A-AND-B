using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000437 RID: 1079
	internal class TimeConverter : AbstractUnitConverter
	{
		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06002DB0 RID: 11696 RVA: 0x0020066C File Offset: 0x001FE86C
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.seconds;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06002DB1 RID: 11697 RVA: 0x00200670 File Offset: 0x001FE870
		// (set) Token: 0x06002DB2 RID: 11698 RVA: 0x00200678 File Offset: 0x001FE878
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
			UnitsHelper.Units.μs,
			UnitsHelper.Units.microseconds,
			UnitsHelper.Units.ms,
			UnitsHelper.Units.seconds,
			UnitsHelper.Units.minutes,
			UnitsHelper.Units.Hours,
			UnitsHelper.Units.days,
			UnitsHelper.Units.months,
			UnitsHelper.Units.h
		};

		// Token: 0x06002DB3 RID: 11699 RVA: 0x00200684 File Offset: 0x001FE884
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from <= UnitsHelper.Units.h)
			{
				if (units_from <= UnitsHelper.Units.ms)
				{
					if (units_from == UnitsHelper.Units.minutes)
					{
						return value * 60.0;
					}
					if (units_from != UnitsHelper.Units.ms)
					{
						return value;
					}
					return value * 0.001;
				}
				else
				{
					if (units_from == UnitsHelper.Units.microseconds)
					{
						return value / 1000000.0;
					}
					if (units_from != UnitsHelper.Units.h)
					{
						return value;
					}
				}
			}
			else if (units_from <= UnitsHelper.Units.Hours)
			{
				if (units_from == UnitsHelper.Units.days)
				{
					return value * 86400.0;
				}
				if (units_from != UnitsHelper.Units.Hours)
				{
					return value;
				}
			}
			else
			{
				if (units_from == UnitsHelper.Units.μs)
				{
					return value * 1E-09;
				}
				if (units_from != UnitsHelper.Units.months)
				{
					return value;
				}
				return value * 2678400.0;
			}
			return value * 3600.0;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x00200728 File Offset: 0x001FE928
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to <= UnitsHelper.Units.h)
			{
				if (units_to <= UnitsHelper.Units.ms)
				{
					if (units_to == UnitsHelper.Units.minutes)
					{
						return value / 60.0;
					}
					if (units_to != UnitsHelper.Units.ms)
					{
						return value;
					}
					return value / 0.001;
				}
				else
				{
					if (units_to == UnitsHelper.Units.microseconds)
					{
						return value * 1000000.0;
					}
					if (units_to != UnitsHelper.Units.h)
					{
						return value;
					}
				}
			}
			else if (units_to <= UnitsHelper.Units.Hours)
			{
				if (units_to == UnitsHelper.Units.days)
				{
					return value / 86400.0;
				}
				if (units_to != UnitsHelper.Units.Hours)
				{
					return value;
				}
			}
			else
			{
				if (units_to == UnitsHelper.Units.μs)
				{
					return value / 1E-09;
				}
				if (units_to != UnitsHelper.Units.months)
				{
					return value;
				}
				return value / 2678400.0;
			}
			return value / 3600.0;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x002007C9 File Offset: 0x001FE9C9
		public TimeConverter()
		{
		}

		// Token: 0x040019C4 RID: 6596
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
