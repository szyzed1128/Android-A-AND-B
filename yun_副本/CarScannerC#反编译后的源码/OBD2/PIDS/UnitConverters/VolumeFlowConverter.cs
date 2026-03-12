using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x0200043A RID: 1082
	internal class VolumeFlowConverter : AbstractUnitConverter
	{
		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06002DC2 RID: 11714 RVA: 0x0001941D File Offset: 0x0001761D
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.Lh;
			}
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x002008F2 File Offset: 0x001FEAF2
		// (set) Token: 0x06002DC4 RID: 11716 RVA: 0x002008FA File Offset: 0x001FEAFA
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
			UnitsHelper.Units.Lh,
			UnitsHelper.Units.gpm,
			UnitsHelper.Units.gph,
			UnitsHelper.Units.lb_min,
			UnitsHelper.Units.L_sec
		};

		// Token: 0x06002DC5 RID: 11717 RVA: 0x00200904 File Offset: 0x001FEB04
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from <= UnitsHelper.Units.gph)
			{
				if (units_from != UnitsHelper.Units.gpm)
				{
					if (units_from == UnitsHelper.Units.gph)
					{
						if (SharedSettings.Current.UseUSGallon)
						{
							return value * 3.785412;
						}
						return value * 4.54609;
					}
				}
				else
				{
					if (SharedSettings.Current.UseUSGallon)
					{
						return value * 227.124707;
					}
					return value * 272.7654;
				}
			}
			else
			{
				if (units_from == UnitsHelper.Units.L_sec)
				{
					return value * 3600.0;
				}
				if (units_from == UnitsHelper.Units.L_min)
				{
					return value * 60.0;
				}
			}
			return value;
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x00200990 File Offset: 0x001FEB90
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to <= UnitsHelper.Units.gph)
			{
				if (units_to != UnitsHelper.Units.gpm)
				{
					if (units_to == UnitsHelper.Units.gph)
					{
						if (SharedSettings.Current.UseUSGallon)
						{
							return value / 3.785412;
						}
						return value / 4.54609;
					}
				}
				else
				{
					if (SharedSettings.Current.UseUSGallon)
					{
						return value / 227.124707;
					}
					return value / 272.7654;
				}
			}
			else
			{
				if (units_to == UnitsHelper.Units.L_sec)
				{
					return value / 3600.0;
				}
				if (units_to == UnitsHelper.Units.L_min)
				{
					return value / 60.0;
				}
			}
			return value;
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x00200A1B File Offset: 0x001FEC1B
		public VolumeFlowConverter()
		{
		}

		// Token: 0x040019C7 RID: 6599
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
