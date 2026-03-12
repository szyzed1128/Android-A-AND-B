using System;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.PlatformAdapters
{
	// Token: 0x020002D9 RID: 729
	public interface Droid_IBackgroundService
	{
		// Token: 0x060022DF RID: 8927
		void UpdateStatus(OBDDataReaderStatus status);

		// Token: 0x060022E0 RID: 8928
		void StopService();
	}
}
