using System;

namespace CarScannerXamarinForms.OBD2.PIDS.UnitConverters
{
	// Token: 0x02000430 RID: 1072
	internal interface IUnitConverter
	{
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06002D89 RID: 11657
		UnitsHelper.Units[] GetUnits { get; }

		// Token: 0x06002D8A RID: 11658
		bool CanConvert(UnitsHelper.Units unit);

		// Token: 0x06002D8B RID: 11659
		double Convert(double original_value, UnitsHelper.Units units_from, UnitsHelper.Units units_to);
	}
}
