using System;

namespace CarScannerXamarinForms
{
	// Token: 0x02000052 RID: 82
	public interface IScreenKeeper
	{
		// Token: 0x060001F0 RID: 496
		void KeepScreenOn();

		// Token: 0x060001F1 RID: 497
		void LetScreenOff();

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001F2 RID: 498
		bool IsScreenForcedOn { get; }
	}
}
