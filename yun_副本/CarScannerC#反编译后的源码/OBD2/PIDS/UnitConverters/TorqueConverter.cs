using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000438 RID: 1080
	internal class TorqueConverter : AbstractUnitConverter
	{
		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x00019412 File Offset: 0x00017612
		protected override UnitsHelper.Units BaseUnit
		{
			get
			{
				return UnitsHelper.Units.Nm;
			}
		}

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x002007E9 File Offset: 0x001FE9E9
		// (set) Token: 0x06002DB8 RID: 11704 RVA: 0x002007F1 File Offset: 0x001FE9F1
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
			UnitsHelper.Units.Nm,
			UnitsHelper.Units.lbf_ft
		};

		// Token: 0x06002DB9 RID: 11705 RVA: 0x002007FA File Offset: 0x001FE9FA
		protected override double ConvertToBaseUnit(double value, UnitsHelper.Units units_from)
		{
			if (units_from == UnitsHelper.Units.lbf_ft)
			{
				return value * 1.355818;
			}
			return value;
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x0020080E File Offset: 0x001FEA0E
		protected override double ConvertFromBaseUnit(double value, UnitsHelper.Units units_to)
		{
			if (units_to == UnitsHelper.Units.lbf_ft)
			{
				return value / 1.355818;
			}
			return value;
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x00200822 File Offset: 0x001FEA22
		public TorqueConverter()
		{
		}

		// Token: 0x040019C5 RID: 6597
		[CompilerGenerated]
		private UnitsHelper.Units[] <GetUnits>k__BackingField;
	}
}
