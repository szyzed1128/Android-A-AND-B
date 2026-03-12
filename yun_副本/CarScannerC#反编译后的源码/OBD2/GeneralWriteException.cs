using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000320 RID: 800
	public class GeneralWriteException : OBDException
	{
		// Token: 0x0600247B RID: 9339 RVA: 0x001BEF81 File Offset: 0x001BD181
		public GeneralWriteException(string message)
			: base(message)
		{
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x001BEF8A File Offset: 0x001BD18A
		public GeneralWriteException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
