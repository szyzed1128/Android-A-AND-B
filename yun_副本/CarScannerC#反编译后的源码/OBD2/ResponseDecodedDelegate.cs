using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200038E RID: 910
	// (Invoke) Token: 0x06002666 RID: 9830
	public delegate void ResponseDecodedDelegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader);
}
