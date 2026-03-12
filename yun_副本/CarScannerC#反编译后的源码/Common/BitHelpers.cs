using System;
using System.Globalization;
using System.Text;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007BE RID: 1982
	public static class BitHelpers
	{
		// Token: 0x0600466C RID: 18028 RVA: 0x00369F78 File Offset: 0x00368178
		public static byte ReverseByte(this byte inByte)
		{
			return (byte)((((ulong)inByte * 8623620610UL) & 1136090292240UL) % 1023UL);
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x00369F98 File Offset: 0x00368198
		public static void SwitchBitInByte(byte[] data, int byteIdx, int bitIdx, bool bitValue)
		{
			data[byteIdx] = BitHelpers.SetBit(data[byteIdx], bitIdx, bitValue);
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x00369FA7 File Offset: 0x003681A7
		public static void SwitchBitInByte(byte[] data, int byteIdx, int bitIdx, int bitValue)
		{
			data[byteIdx] = BitHelpers.SetBit(data[byteIdx], bitIdx, bitValue != 0);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x001ECAC3 File Offset: 0x001EACC3
		public static bool GetBit_0_7(byte b, int bitNumber)
		{
			return ((int)b & (1 << bitNumber)) != 0;
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x00369FB9 File Offset: 0x003681B9
		public static bool GetBit_1_8(byte b, int bitNumber)
		{
			return ((int)b & (1 << bitNumber - 1)) != 0;
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x00369FC8 File Offset: 0x003681C8
		private static int HexToInt(char c)
		{
			switch (c)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			case ':':
			case ';':
			case '<':
			case '=':
			case '>':
			case '?':
			case '@':
				goto IL_00AF;
			case 'A':
				break;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			default:
				switch (c)
				{
				case 'a':
					break;
				case 'b':
					return 11;
				case 'c':
					return 12;
				case 'd':
					return 13;
				case 'e':
					return 14;
				case 'f':
					return 15;
				default:
					goto IL_00AF;
				}
				break;
			}
			return 10;
			IL_00AF:
			throw new FormatException("Unrecognized hex char " + c.ToString());
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x0036A09C File Offset: 0x0036829C
		public unsafe static byte[] ConvertHexToBytesX(string input, int startIdx, int length)
		{
			ReadOnlySpan<char> readOnlySpan = input.AsSpan().Slice(startIdx, length);
			byte[] array = new byte[readOnlySpan.Length + 1 >> 1];
			int num = array.Length - 1;
			int num2 = readOnlySpan.Length - 1;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				byte[] array2 = array;
				int num3 = num - (i >> 1);
				array2[num3] |= BitHelpers.ByteLookup[i & 1, BitHelpers.HexToInt((char)(*readOnlySpan[num2 - i]))];
			}
			return array;
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x0036A124 File Offset: 0x00368324
		public static byte[] ConvertHexToBytesX(string input)
		{
			byte[] array = new byte[input.Length + 1 >> 1];
			int num = array.Length - 1;
			int num2 = input.Length - 1;
			for (int i = 0; i < input.Length; i++)
			{
				byte[] array2 = array;
				int num3 = num - (i >> 1);
				array2[num3] |= BitHelpers.ByteLookup[i & 1, BitHelpers.HexToInt(input[num2 - i])];
			}
			return array;
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x0036A18D File Offset: 0x0036838D
		public static int ConvertHexToInt(string input)
		{
			return int.Parse(input, NumberStyles.HexNumber);
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x0036A19C File Offset: 0x0036839C
		public static int GetChecksumm(byte[] data)
		{
			byte b = 0;
			for (int i = 0; i < data.Length; i++)
			{
				b += data[i];
			}
			return (int)b;
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x0036A1C1 File Offset: 0x003683C1
		public static int GetChecksumm(string HexString)
		{
			return BitHelpers.GetChecksumm(BitHelpers.ConvertHexToBytesX(HexString));
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x0036A1D0 File Offset: 0x003683D0
		public static string GetCheckSummHex(string HexString)
		{
			return BitHelpers.GetChecksumm(HexString).ToString("X2", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x0036A1F5 File Offset: 0x003683F5
		public static byte SetBit(byte aByte, int pos, bool value)
		{
			if (value)
			{
				aByte = (byte)((int)aByte | (1 << pos));
				return aByte;
			}
			aByte = (byte)((int)aByte & ~(1 << pos));
			return aByte;
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x0036A214 File Offset: 0x00368414
		public static short BytesToShort(byte[] bytes)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(bytes);
			}
			return BitConverter.ToInt16(bytes, 0);
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x0036A22C File Offset: 0x0036842C
		public static string ByteArrayToHexString(byte[] Bytes)
		{
			StringBuilder stringBuilder = new StringBuilder(Bytes.Length * 2);
			string text = "0123456789ABCDEF";
			foreach (byte b in Bytes)
			{
				stringBuilder.Append(text[b >> 4]);
				stringBuilder.Append(text[(int)(b & 15)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x0036A288 File Offset: 0x00368488
		public static byte[] HexStringToByteArray(string Hex)
		{
			byte[] array = new byte[Hex.Length / 2];
			int[] array2 = new int[]
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				0, 0, 0, 0, 0, 0, 0, 10, 11, 12,
				13, 14, 15
			};
			int num = 0;
			int i = 0;
			while (i < Hex.Length)
			{
				array[num] = (byte)((array2[(int)(char.ToUpper(Hex[i]) - '0')] << 4) | array2[(int)(char.ToUpper(Hex[i + 1]) - '0')]);
				i += 2;
				num++;
			}
			return array;
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x0036A2F9 File Offset: 0x003684F9
		// Note: this type is marked as 'beforefieldinit'.
		static BitHelpers()
		{
		}

		// Token: 0x040028F1 RID: 10481
		private static readonly byte[,] ByteLookup = new byte[,]
		{
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				10, 11, 12, 13, 14, 15
			},
			{
				0, 16, 32, 48, 64, 80, 96, 112, 128, 144,
				160, 176, 192, 208, 224, 240
			}
		};
	}
}
