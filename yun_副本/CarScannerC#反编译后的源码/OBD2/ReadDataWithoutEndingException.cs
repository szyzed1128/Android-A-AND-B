using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200031E RID: 798
	public class ReadDataWithoutEndingException : OBDException
	{
		// Token: 0x06002477 RID: 9335 RVA: 0x001BEF81 File Offset: 0x001BD181
		public ReadDataWithoutEndingException(string message)
			: base(message)
		{
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x001BEF8A File Offset: 0x001BD18A
		public ReadDataWithoutEndingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
