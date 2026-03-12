using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x0200042A RID: 1066
	internal abstract class AbstractUnitConverter : IUnitConverter
	{
		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06002D64 RID: 11620 RVA: 0x001FFC6E File Offset: 0x001FDE6E
		// (set) Token: 0x06002D65 RID: 11621 RVA: 0x001FFC76 File Offset: 0x001FDE76
		public virtual UnitsHelper.Units[] GetUnits
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
		} = new UnitsHelper.Units[0];

		// Token: 0x06002D66 RID: 11622 RVA: 0x001FFC80 File Offset: 0x001FDE80
		public bool CanConvert(UnitsHelper.Units unit)
		{
			UnitsHelper.Units[] getUnits = this.GetUnits;
			for (int i = 0; i < getUnits.Length; i++)
			{
				if (getUnits[i] == unit)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x001FFCAC File Offset: 0x001FDEAC
		public virtual double Convert(double original_value, UnitsHelper.Units units_from, UnitsHelper.Units units_to)
		{
			if (units_to == units_from)
			{
				return original_value;
			}
			if (!this.CanConvert(units_from) || !this.CanConvert(units_to))
			{
				return original_value;
			}
			double num = original_value;
			if (units_from != this.BaseUnit)
			{
				num = this.ConvertToBaseUnit(original_value, units_from);
			}
			if (units_to == this.BaseUnit)
			{
				return num;
			}
			return this.ConvertFromBaseUnit(num, units_to);
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06002D68 RID: 11624 RVA: 0x00002076 File Offset: 0x00000276
		protected virtual UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.None;
			}
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x00017A6F File Offset: 0x00015C6F
		protected virtual double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x00017A6F File Offset: 0x00015C6F
		protected virtual double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x001FFCFA File Offset: 0x001FDEFA
		protected AbstractUnitConverter()
		{
		}

		// Token: 0x040019B8 RID: 6584
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
