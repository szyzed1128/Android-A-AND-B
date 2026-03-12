using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000317 RID: 791
	public enum OBDDataReaderStatus
	{
		// Token: 0x04001209 RID: 4617
		Disconnected,
		// Token: 0x0400120A RID: 4618
		ConnectingToELM,
		// Token: 0x0400120B RID: 4619
		ConnectedToELM,
		// Token: 0x0400120C RID: 4620
		ConnectingToECU,
		// Token: 0x0400120D RID: 4621
		ConnectedToECU,
		// Token: 0x0400120E RID: 4622
		Disconnecting
	}
}
