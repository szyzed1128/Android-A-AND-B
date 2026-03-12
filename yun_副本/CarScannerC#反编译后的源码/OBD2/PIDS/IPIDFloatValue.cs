using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003FC RID: 1020
	public interface IPIDFloatValue : IPID, INotifyPropertyChanged
	{
		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x06002903 RID: 10499
		double Value { get; }

		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x06002904 RID: 10500
		UnitsHelper.Units Units { get; }

		// Token: 0x06002905 RID: 10501
		void SetValue(double value);

		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x06002906 RID: 10502
		string TextValueVariants { get; }

		// Token: 0x06002907 RID: 10503
		string GetTextValueVariantOrNull(double value);
	}
}
