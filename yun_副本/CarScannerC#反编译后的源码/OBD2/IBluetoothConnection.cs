using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000323 RID: 803
	public interface IBluetoothConnection : IOBDConnection
	{
		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x0600248A RID: 9354
		// (set) Token: 0x0600248B RID: 9355
		int ConnectionTimeoutSeconds { get; set; }
	}
}
