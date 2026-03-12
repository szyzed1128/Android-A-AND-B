using System;
using System.IO;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007C1 RID: 1985
	public static class Crc32
	{
		// Token: 0x06004680 RID: 18048 RVA: 0x0036A3F8 File Offset: 0x003685F8
		public static byte[] Calculate(byte[] source)
		{
			uint[] array = new uint[256];
			uint num2;
			for (uint num = 0U; num < 256U; num += 1U)
			{
				num2 = num;
				for (uint num3 = 0U; num3 < 8U; num3 += 1U)
				{
					num2 = (((num2 & 1U) != 0U) ? ((num2 >> 1) ^ 3988292384U) : (num2 >> 1));
				}
				array[(int)num] = num2;
			}
			num2 = uint.MaxValue;
			foreach (byte b in source)
			{
				num2 = array[(int)((num2 ^ (uint)b) & 255U)] ^ (num2 >> 8);
			}
			num2 ^= uint.MaxValue;
			byte[] bytes = BitConverter.GetBytes(num2);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(bytes);
			}
			return bytes;
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x0036A494 File Offset: 0x00368694
		public static byte[] Calculate(Stream stream)
		{
			uint[] array = new uint[256];
			uint num2;
			for (uint num = 0U; num < 256U; num += 1U)
			{
				num2 = num;
				for (uint num3 = 0U; num3 < 8U; num3 += 1U)
				{
					num2 = (((num2 & 1U) != 0U) ? ((num2 >> 1) ^ 3988292384U) : (num2 >> 1));
				}
				array[(int)num] = num2;
			}
			num2 = uint.MaxValue;
			while (stream.Position < stream.Length)
			{
				int num4 = stream.ReadByte();
				if (num4 >= 0)
				{
					byte b = (byte)num4;
					num2 = array[(int)((num2 ^ (uint)b) & 255U)] ^ (num2 >> 8);
				}
			}
			num2 ^= uint.MaxValue;
			byte[] bytes = BitConverter.GetBytes(num2);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(bytes);
			}
			return bytes;
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x0036A538 File Offset: 0x00368738
		public static uint CalculateUint(byte[] source)
		{
			byte[] array = Crc32.Calculate(source);
			return (uint)array[0] * 256U * 256U * 256U + (uint)array[1] * 256U * 256U + (uint)array[2] * 256U + (uint)array[3];
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x0036A580 File Offset: 0x00368780
		public static int CalculateInt(byte[] source)
		{
			byte[] array = Crc32.Calculate(source);
			return (int)array[0] * 256 * 256 * 256 + (int)array[1] * 256 * 256 + (int)array[2] * 256 + (int)array[3];
		}
	}
}
