using System;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000648 RID: 1608
	internal interface IPIDSelector
	{
		// Token: 0x060037CA RID: 14282
		Task WaitSemaphoreAsync();

		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x060037CB RID: 14283
		IPID SelectedPID { get; }
	}
}
