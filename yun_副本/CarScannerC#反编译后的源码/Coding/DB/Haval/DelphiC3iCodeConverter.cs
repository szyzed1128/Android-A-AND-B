using System;
using System.Collections.Generic;
using System.Text;

namespace CarScannerXamarinForms.Coding.DB.Haval
{
	// Token: 0x02000BD7 RID: 3031
	internal static class DelphiC3iCodeConverter
	{
		// Token: 0x06005B41 RID: 23361 RVA: 0x00437E2C File Offset: 0x0043602C
		private static byte CharToCode(char c)
		{
			byte b;
			if (DelphiC3iCodeConverter.dict.TryGetValue(c, out b))
			{
				return b;
			}
			return byte.MaxValue;
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x00437E50 File Offset: 0x00436050
		private static char ByteToChar(byte b)
		{
			foreach (KeyValuePair<char, byte> keyValuePair in DelphiC3iCodeConverter.dict)
			{
				if (keyValuePair.Value == b)
				{
					return keyValuePair.Key;
				}
			}
			return '-';
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x00437EB4 File Offset: 0x004360B4
		public static string BytesToCode(byte[] data)
		{
			StringBuilder stringBuilder = new StringBuilder(data.Length);
			for (int i = 0; i < data.Length; i++)
			{
				stringBuilder.Append(DelphiC3iCodeConverter.ByteToChar(data[i]));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x00437EF0 File Offset: 0x004360F0
		public static byte[] CodeToBytes(string code)
		{
			byte[] array = new byte[code.Length];
			for (int i = 0; i < code.Length; i++)
			{
				byte b = DelphiC3iCodeConverter.CharToCode(code[i]);
				array[i] = b;
			}
			return array;
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x00437F2C File Offset: 0x0043612C
		// Note: this type is marked as 'beforefieldinit'.
		static DelphiC3iCodeConverter()
		{
		}

		// Token: 0x0400398C RID: 14732
		private static Dictionary<char, byte> dict = new Dictionary<char, byte>
		{
			{ '0', 0 },
			{ '1', 1 },
			{ '2', 2 },
			{ '3', 3 },
			{ '4', 4 },
			{ '5', 5 },
			{ '6', 6 },
			{ '7', 7 },
			{ '8', 8 },
			{ '9', 9 },
			{ 'A', 10 },
			{ 'B', 11 },
			{ 'C', 12 },
			{ 'D', 13 },
			{ 'E', 14 },
			{ 'F', 15 },
			{ 'G', 16 },
			{ 'J', 18 },
			{ 'K', 19 },
			{ 'L', 20 },
			{ 'M', 21 },
			{ 'N', 22 },
			{ 'P', 23 },
			{ 'R', 24 },
			{ 'S', 25 },
			{ 'T', 26 },
			{ 'U', 27 },
			{ 'W', 28 },
			{ 'X', 29 },
			{ 'Y', 30 },
			{ 'Z', 31 }
		};
	}
}
