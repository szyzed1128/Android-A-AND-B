using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200031C RID: 796
	public class ReadDataTimeoutException : OBDException
	{
		// Token: 0x06002473 RID: 9331 RVA: 0x001BEF81 File Offset: 0x001BD181
		public ReadDataTimeoutException(string message)
			: base(message)
		{
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x001BEF8A File Offset: 0x001BD18A
		public ReadDataTimeoutException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
