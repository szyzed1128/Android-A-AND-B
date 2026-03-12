using System;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200084D RID: 2125
	public class WrongDeviceException : CodingException
	{
		// Token: 0x06004899 RID: 18585 RVA: 0x0037100E File Offset: 0x0036F20E
		public WrongDeviceException(string Message, ExceptionConsequences ExceptionConsequence)
			: base(Message, ExceptionConsequence)
		{
		}
	}
}
