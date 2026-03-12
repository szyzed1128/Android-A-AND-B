using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000398 RID: 920
	internal class OBDResponseAnalyzer
	{
		// Token: 0x060026FC RID: 9980 RVA: 0x001DF89C File Offset: 0x001DDA9C
		public static ELMFormat AnalyzeResponse(string cmd, string reply0100)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string reply_marker = OBDResponseAnalyzer.GetResponseMarkerFromCommand(cmd);
			string[] array = (from x in reply0100.Replace(" ", string.Empty).Split('\r', StringSplitOptions.None)
				select OBDDataReader.FilterHexAndNewLineOnly(x.Trim()) into x
				where x.Length > 3 && x.Contains(reply_marker)
				select x).ToArray<string>();
			foreach (string text in array)
			{
				num += OBDResponseAnalyzer.GetCAN11bitPoints(reply_marker, text);
			}
			foreach (string text2 in array)
			{
				num2 += OBDResponseAnalyzer.GetCAN29bitPoints(reply_marker, text2);
			}
			foreach (string text3 in array)
			{
				num3 += OBDResponseAnalyzer.GetKWPPoints(reply_marker, text3);
			}
			if (num > num2 && num > num3)
			{
				return ELMFormat.CAN11bit;
			}
			if (num2 > num && num2 > num3)
			{
				return ELMFormat.CAN29bit;
			}
			if (num3 > num && num3 > num)
			{
				return ELMFormat.KWP;
			}
			return ELMFormat.Unknown;
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x001DF9C0 File Offset: 0x001DDBC0
		private static int GetCAN11bitPoints(string response_marker, string line)
		{
			int num = 0;
			int num2 = line.IndexOf(response_marker);
			if (line[0] == '7')
			{
				num += 2;
			}
			if (line.Length % 2 != 0)
			{
				num += 2;
			}
			if (num2 == 5)
			{
				num += 2;
				int num3 = int.Parse(line.Substring(num2 - 2, 2), NumberStyles.AllowHexSpecifier);
				if ((line.Length - num2) / 2 == num3)
				{
					num += 2;
				}
			}
			if (num2 == 7 && line[3] == '1' && line[4] == '0')
			{
				num += 2;
			}
			if (line[1] == '7')
			{
				if (line[line.Length - 1] == line[0])
				{
					num += 2;
				}
				else
				{
					num++;
				}
				if (num2 == 6)
				{
					num += 2;
				}
				if (num2 == 8 && line[4] == '1' && line[5] == '0')
				{
					num += 2;
				}
			}
			return num;
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x001DFA94 File Offset: 0x001DDC94
		private static int GetCAN29bitPoints(string response_marker, string line)
		{
			int num = 0;
			int num2 = line.IndexOf(response_marker);
			if (line.Length % 2 == 0)
			{
				num++;
			}
			if (num2 == 10)
			{
				num += 2;
				int num3 = int.Parse(line.Substring(num2 - 2, 2), NumberStyles.AllowHexSpecifier);
				if ((line.Length - num2) / 2 == num3)
				{
					num += 2;
				}
			}
			if (num2 == 12 && line[8] == '1' && line[9] == '0')
			{
				num += 2;
			}
			return num;
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x001DFB0C File Offset: 0x001DDD0C
		private static int GetKWPPoints(string response_marker, string line)
		{
			int num = 0;
			int num2 = line.IndexOf(response_marker);
			if (line.Length % 2 == 0)
			{
				num++;
			}
			if (num2 == 6)
			{
				num += 2;
			}
			string text = line.Substring(line.Length - 2);
			if ((BitHelpers.GetCheckSummHex(line.Substring(0, line.Length - 2)) == text) > false)
			{
				num += 2;
			}
			return num;
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x001DFB6C File Offset: 0x001DDD6C
		private static string GetResponseMarkerFromCommand(string cmd)
		{
			int num = BitHelpers.ConvertHexToInt(cmd.Substring(0, 2));
			num += 64;
			if (cmd.Length == 2)
			{
				return num.ToString("X2", CultureInfo.InvariantCulture);
			}
			if (cmd.Length != 6)
			{
				return num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2);
			}
			if (cmd[0] == '0')
			{
				return num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2);
			}
			return num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2, 2);
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x001DFC18 File Offset: 0x001DDE18
		public static ELMFormat GetELMFormatFromProtocolNumber(int ProtocolNumber)
		{
			switch (ProtocolNumber)
			{
			case 0:
				return ELMFormat.Unknown;
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 11:
				return ELMFormat.KWP;
			case 6:
			case 8:
			case 10:
				break;
			case 7:
			case 9:
				return ELMFormat.CAN29bit;
			default:
				if (ProtocolNumber != 43)
				{
					return ELMFormat.Unknown;
				}
				break;
			}
			return ELMFormat.CAN11bit;
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x001DFC6C File Offset: 0x001DDE6C
		public static int GetProtocolNumberFromInitString(string init_string)
		{
			int num = init_string.ToUpperInvariant().LastIndexOf("ATSP");
			if (num == -1)
			{
				return 0;
			}
			if (init_string.Length < num + 5)
			{
				return 0;
			}
			string text = init_string.Substring(num + 4, 1);
			int num2;
			try
			{
				num2 = BitHelpers.ConvertHexToInt(text);
			}
			catch
			{
				num2 = 0;
			}
			return num2;
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x001DFCC8 File Offset: 0x001DDEC8
		public static int ParseProtocolNumberResponseString(string reply)
		{
			if (string.IsNullOrEmpty(reply))
			{
				return 0;
			}
			reply = reply.Replace("SEARCHING", "").Replace("STOPPED", "");
			StringBuilder stringBuilder = new StringBuilder(reply.Length);
			for (int i = 0; i < reply.Length; i++)
			{
				if (char.IsLetterOrDigit(reply, i))
				{
					stringBuilder.Append(reply[i]);
				}
			}
			reply = stringBuilder.ToString();
			return "0123456789ABCDEF".IndexOf(char.ToUpper(reply[reply.Length - 1]));
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x00002050 File Offset: 0x00000250
		public OBDResponseAnalyzer()
		{
		}

		// Token: 0x02000399 RID: 921
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002705 RID: 9989 RVA: 0x001DFD59 File Offset: 0x001DDF59
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002706 RID: 9990 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002707 RID: 9991 RVA: 0x001DFD65 File Offset: 0x001DDF65
			internal string <AnalyzeResponse>b__0_0(string x)
			{
				return OBDDataReader.FilterHexAndNewLineOnly(x.Trim());
			}

			// Token: 0x0400153A RID: 5434
			public static readonly OBDResponseAnalyzer.<>c <>9 = new OBDResponseAnalyzer.<>c();

			// Token: 0x0400153B RID: 5435
			public static Func<string, string> <>9__0_0;
		}

		// Token: 0x0200039A RID: 922
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06002708 RID: 9992 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06002709 RID: 9993 RVA: 0x001DFD72 File Offset: 0x001DDF72
			internal bool <AnalyzeResponse>b__1(string x)
			{
				return x.Length > 3 && x.Contains(this.reply_marker);
			}

			// Token: 0x0400153C RID: 5436
			public string reply_marker;
		}
	}
}
