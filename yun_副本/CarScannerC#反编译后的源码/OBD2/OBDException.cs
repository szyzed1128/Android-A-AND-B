using System;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200031B RID: 795
	public class OBDException : OperationCanceledException
	{
		// Token: 0x06002471 RID: 9329 RVA: 0x001BEF6E File Offset: 0x001BD16E
		protected OBDException(string message)
			: base(message)
		{
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x001BEF77 File Offset: 0x001BD177
		protected OBDException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
