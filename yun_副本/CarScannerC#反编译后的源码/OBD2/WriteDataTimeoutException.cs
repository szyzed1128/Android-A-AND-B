using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200031D RID: 797
	public class WriteDataTimeoutException : OBDException
	{
		// Token: 0x06002475 RID: 9333 RVA: 0x001BEF81 File Offset: 0x001BD181
		public WriteDataTimeoutException(string message)
			: base(message)
		{
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x001BEF8A File Offset: 0x001BD18A
		public WriteDataTimeoutException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
