using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x020002EE RID: 750
	internal static class CAN29bitHelper
	{
		// Token: 0x06002368 RID: 9064 RVA: 0x001B18C8 File Offset: 0x001AFAC8
		public static string GetPossibleResponseHeaderFilter(string requestHeader, string brand, OBDRequest request)
		{
			if (request != null && request.BeforeCommands != null && request.BeforeCommands.Length != 0)
			{
				string text = request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase));
				if (text != null)
				{
					return text.ToUpperInvariant().Replace(" ", "").Replace("ATCRA", "")
						.Substring(6, 2);
				}
				if (request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCF", StringComparison.OrdinalIgnoreCase)) != null)
				{
					if (request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCM", StringComparison.OrdinalIgnoreCase)) != null)
					{
						return "??";
					}
				}
			}
			if (requestHeader.Length == 6)
			{
				return requestHeader.Substring(2, 2);
			}
			if (requestHeader.Length == 8)
			{
				return requestHeader.Substring(4, 2);
			}
			return "";
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x001B19D4 File Offset: 0x001AFBD4
		public static string GetPossibleResponseHeader(string requestHeader, string brand, OBDRequest request)
		{
			if (request != null && request.BeforeCommands != null && request.BeforeCommands.Length != 0)
			{
				string text = request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase));
				if (text != null)
				{
					return text.ToUpperInvariant().Replace(" ", "").Replace("ATCRA", "");
				}
				if (request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCF", StringComparison.OrdinalIgnoreCase)) != null)
				{
					if (request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCM", StringComparison.OrdinalIgnoreCase)) != null)
					{
						return "??";
					}
				}
			}
			if (requestHeader.Length == 6)
			{
				return App.OBDReader.ELMStatus.CAN29bitPriority + requestHeader.Substring(0, 2) + requestHeader.Substring(4, 2) + requestHeader.Substring(2, 2);
			}
			if (requestHeader.Length == 8)
			{
				return requestHeader.Substring(0, 4) + requestHeader.Substring(6, 2) + requestHeader.Substring(4, 2);
			}
			return "";
		}

		// Token: 0x020002EF RID: 751
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600236A RID: 9066 RVA: 0x001B1B11 File Offset: 0x001AFD11
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600236B RID: 9067 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600236C RID: 9068 RVA: 0x001B17AB File Offset: 0x001AF9AB
			internal bool <GetPossibleResponseHeaderFilter>b__0_0(string x)
			{
				return x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600236D RID: 9069 RVA: 0x001B1B1D File Offset: 0x001AFD1D
			internal bool <GetPossibleResponseHeaderFilter>b__0_1(string x)
			{
				return x.StartsWith("ATCF", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600236E RID: 9070 RVA: 0x001B1B2B File Offset: 0x001AFD2B
			internal bool <GetPossibleResponseHeaderFilter>b__0_2(string x)
			{
				return x.StartsWith("ATCM", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600236F RID: 9071 RVA: 0x001B17AB File Offset: 0x001AF9AB
			internal bool <GetPossibleResponseHeader>b__1_0(string x)
			{
				return x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06002370 RID: 9072 RVA: 0x001B1B1D File Offset: 0x001AFD1D
			internal bool <GetPossibleResponseHeader>b__1_1(string x)
			{
				return x.StartsWith("ATCF", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06002371 RID: 9073 RVA: 0x001B1B2B File Offset: 0x001AFD2B
			internal bool <GetPossibleResponseHeader>b__1_2(string x)
			{
				return x.StartsWith("ATCM", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x04001133 RID: 4403
			public static readonly CAN29bitHelper.<>c <>9 = new CAN29bitHelper.<>c();

			// Token: 0x04001134 RID: 4404
			public static Func<string, bool> <>9__0_0;

			// Token: 0x04001135 RID: 4405
			public static Func<string, bool> <>9__0_1;

			// Token: 0x04001136 RID: 4406
			public static Func<string, bool> <>9__0_2;

			// Token: 0x04001137 RID: 4407
			public static Func<string, bool> <>9__1_0;

			// Token: 0x04001138 RID: 4408
			public static Func<string, bool> <>9__1_1;

			// Token: 0x04001139 RID: 4409
			public static Func<string, bool> <>9__1_2;
		}
	}
}
