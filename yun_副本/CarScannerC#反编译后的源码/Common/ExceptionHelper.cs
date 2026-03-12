using System;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007CD RID: 1997
	public static class ExceptionHelper
	{
		// Token: 0x060046B6 RID: 18102 RVA: 0x0036B4A8 File Offset: 0x003696A8
		public static string ToStringWithInnerExceptions(this Exception exc)
		{
			if (exc == null)
			{
				return "";
			}
			string text = exc.ToString();
			if (exc.InnerException != null)
			{
				string text2 = exc.InnerException.ToStringWithInnerExceptions();
				text = text + "\n" + text2;
			}
			return text;
		}
	}
}
