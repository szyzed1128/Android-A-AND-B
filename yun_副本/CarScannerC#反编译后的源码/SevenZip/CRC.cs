using System;

namespace SevenZip
{
	// Token: 0x02000018 RID: 24
	internal class CRC
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00005854 File Offset: 0x00003A54
		static CRC()
		{
			for (uint num = 0U; num < 256U; num += 1U)
			{
				uint num2 = num;
				for (int i = 0; i < 8; i++)
				{
					if ((num2 & 1U) != 0U)
					{
						num2 = (num2 >> 1) ^ 3988292384U;
					}
					else
					{
						num2 >>= 1;
					}
				}
				CRC.Table[(int)num] = num2;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000058AB File Offset: 0x00003AAB
		public void Init()
		{
			this._value = uint.MaxValue;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000058B4 File Offset: 0x00003AB4
		public void UpdateByte(byte b)
		{
			this._value = CRC.Table[(int)((byte)this._value ^ b)] ^ (this._value >> 8);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000058D4 File Offset: 0x00003AD4
		public void Update(byte[] data, uint offset, uint size)
		{
			for (uint num = 0U; num < size; num += 1U)
			{
				this._value = CRC.Table[(int)((byte)this._value ^ data[(int)(offset + num)])] ^ (this._value >> 8);
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000590F File Offset: 0x00003B0F
		public uint GetDigest()
		{
			return this._value ^ uint.MaxValue;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005919 File Offset: 0x00003B19
		private static uint CalculateDigest(byte[] data, uint offset, uint size)
		{
			CRC crc = new CRC();
			crc.Update(data, offset, size);
			return crc.GetDigest();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000592E File Offset: 0x00003B2E
		private static bool VerifyDigest(uint digest, byte[] data, uint offset, uint size)
		{
			return CRC.CalculateDigest(data, offset, size) == digest;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000593B File Offset: 0x00003B3B
		public CRC()
		{
		}

		// Token: 0x0400006B RID: 107
		public static readonly uint[] Table = new uint[256];

		// Token: 0x0400006C RID: 108
		private uint _value = uint.MaxValue;
	}
}
