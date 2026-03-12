using System;

namespace SevenZip.Compression.RangeCoder
{
	// Token: 0x02000028 RID: 40
	internal struct BitTreeDecoder
	{
		// Token: 0x060000FC RID: 252 RVA: 0x00006464 File Offset: 0x00004664
		public BitTreeDecoder(int numBitLevels)
		{
			this.NumBitLevels = numBitLevels;
			this.Models = new BitDecoder[1 << numBitLevels];
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006480 File Offset: 0x00004680
		public void Init()
		{
			uint num = 1U;
			while ((ulong)num < (ulong)(1L << (this.NumBitLevels & 31)))
			{
				this.Models[(int)num].Init();
				num += 1U;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000064B8 File Offset: 0x000046B8
		public uint Decode(Decoder rangeDecoder)
		{
			uint num = 1U;
			for (int i = this.NumBitLevels; i > 0; i--)
			{
				num = (num << 1) + this.Models[(int)num].Decode(rangeDecoder);
			}
			return num - (1U << this.NumBitLevels);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000064FC File Offset: 0x000046FC
		public uint ReverseDecode(Decoder rangeDecoder)
		{
			uint num = 1U;
			uint num2 = 0U;
			for (int i = 0; i < this.NumBitLevels; i++)
			{
				uint num3 = this.Models[(int)num].Decode(rangeDecoder);
				num <<= 1;
				num += num3;
				num2 |= num3 << i;
			}
			return num2;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006544 File Offset: 0x00004744
		public static uint ReverseDecode(BitDecoder[] Models, uint startIndex, Decoder rangeDecoder, int NumBitLevels)
		{
			uint num = 1U;
			uint num2 = 0U;
			for (int i = 0; i < NumBitLevels; i++)
			{
				uint num3 = Models[(int)(startIndex + num)].Decode(rangeDecoder);
				num <<= 1;
				num += num3;
				num2 |= num3 << i;
			}
			return num2;
		}

		// Token: 0x040000A1 RID: 161
		private BitDecoder[] Models;

		// Token: 0x040000A2 RID: 162
		private int NumBitLevels;
	}
}
