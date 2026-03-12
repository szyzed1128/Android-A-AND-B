using System;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000740 RID: 1856
	// (Invoke) Token: 0x06003F19 RID: 16153
	public delegate void PIDChangedEvent(IPID NewPID, LiveDataPIDModel Model);
}
