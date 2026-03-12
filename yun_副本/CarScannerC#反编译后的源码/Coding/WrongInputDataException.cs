using System;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200084E RID: 2126
	public class WrongInputDataException : CodingException
	{
		// Token: 0x0600489A RID: 18586 RVA: 0x0037100E File Offset: 0x0036F20E
		public WrongInputDataException(string Message, ExceptionConsequences ExceptionConsequence)
			: base(Message, ExceptionConsequence)
		{
		}
	}
}
