using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000322 RID: 802
	public interface ITCPConnection : IOBDConnection
	{
		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x06002488 RID: 9352
		// (set) Token: 0x06002489 RID: 9353
		bool Bind { get; set; }
	}
}
