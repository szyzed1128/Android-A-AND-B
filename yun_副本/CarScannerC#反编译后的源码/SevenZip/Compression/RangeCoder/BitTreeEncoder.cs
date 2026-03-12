using System;

namespace SevenZip.Compression.RangeCoder
{
	// Token: 0x02000027 RID: 39
	internal struct BitTreeEncoder
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x0000627D File Offset: 0x0000447D
		public BitTreeEncoder(int numBitLevels)
		{
			this.NumBitLevels = numBitLevels;
			this.Models = new BitEncoder[1 << numBitLevels];
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006298 File Offset: 0x00004498
		public void Init()
		{
			uint num = 1U;
			while ((ulong)num < (ulong)(1L << (this.NumBitLevels & 31)))
			{
				this.Models[(int)num].Init();
				num += 1U;
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000062D0 File Offset: 0x000044D0
		public void Encode(Encoder rangeEncoder, uint symbol)
		{
			uint num = 1U;
			int i = this.NumBitLevels;
			while (i > 0)
			{
				i--;
				uint num2 = (symbol >> i) & 1U;
				this.Models[(int)num].Encode(rangeEncoder, num2);
				num = (num << 1) | num2;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006314 File Offset: 0x00004514
		public void ReverseEncode(Encoder rangeEncoder, uint symbol)
		{
			uint num = 1U;
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)this.NumBitLevels))
			{
				uint num3 = symbol & 1U;
				this.Models[(int)num].Encode(rangeEncoder, num3);
				num = (num << 1) | num3;
				symbol >>= 1;
				num2 += 1U;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006358 File Offset: 0x00004558
		public uint GetPrice(uint symbol)
		{
			uint num = 0U;
			uint num2 = 1U;
			int i = this.NumBitLevels;
			while (i > 0)
			{
				i--;
				uint num3 = (symbol >> i) & 1U;
				num += this.Models[(int)num2].GetPrice(num3);
				num2 = (num2 << 1) + num3;
			}
			return num;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000063A0 File Offset: 0x000045A0
		public uint ReverseGetPrice(uint symbol)
		{
			uint num = 0U;
			uint num2 = 1U;
			for (int i = this.NumBitLevels; i > 0; i--)
			{
				uint num3 = symbol & 1U;
				symbol >>= 1;
				num += this.Models[(int)num2].GetPrice(num3);
				num2 = (num2 << 1) | num3;
			}
			return num;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000063E8 File Offset: 0x000045E8
		public static uint ReverseGetPrice(BitEncoder[] Models, uint startIndex, int NumBitLevels, uint symbol)
		{
			uint num = 0U;
			uint num2 = 1U;
			for (int i = NumBitLevels; i > 0; i--)
			{
				uint num3 = symbol & 1U;
				symbol >>= 1;
				num += Models[(int)(startIndex + num2)].GetPrice(num3);
				num2 = (num2 << 1) | num3;
			}
			return num;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006428 File Offset: 0x00004628
		public static void ReverseEncode(BitEncoder[] Models, uint startIndex, Encoder rangeEncoder, int NumBitLevels, uint symbol)
		{
			uint num = 1U;
			for (int i = 0; i < NumBitLevels; i++)
			{
				uint num2 = symbol & 1U;
				Models[(int)(startIndex + num)].Encode(rangeEncoder, num2);
				num = (num << 1) | num2;
				symbol >>= 1;
			}
		}

		// Token: 0x0400009F RID: 159
		private BitEncoder[] Models;

		// Token: 0x040000A0 RID: 160
		private int NumBitLevels;
	}
}
