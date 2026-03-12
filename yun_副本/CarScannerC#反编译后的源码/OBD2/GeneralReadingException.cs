using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200031F RID: 799
	public class GeneralReadingException : OBDException
	{
		// Token: 0x06002479 RID: 9337 RVA: 0x001BEF81 File Offset: 0x001BD181
		public GeneralReadingException(string message)
			: base(message)
		{
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x001BEF8A File Offset: 0x001BD18A
		public GeneralReadingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
