using System;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.PlatformAdapters
{
	// Token: 0x020002DA RID: 730
	public interface IAndroidHelperImplementation
	{
		// Token: 0x060022E1 RID: 8929
		void StartService();

		// Token: 0x060022E2 RID: 8930
		void StopService();

		// Token: 0x060022E3 RID: 8931
		Task RequestBluetoothPowerOn();

		// Token: 0x060022E4 RID: 8932
		Task RequestBluetoothPowerOff();
	}
}
