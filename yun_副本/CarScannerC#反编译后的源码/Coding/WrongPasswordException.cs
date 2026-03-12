using System;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200084F RID: 2127
	public class WrongPasswordException : CodingException
	{
		// Token: 0x0600489B RID: 18587 RVA: 0x0037100E File Offset: 0x0036F20E
		public WrongPasswordException(string Message, ExceptionConsequences ExceptionConsequence)
			: base(Message, ExceptionConsequence)
		{
		}
	}
}
