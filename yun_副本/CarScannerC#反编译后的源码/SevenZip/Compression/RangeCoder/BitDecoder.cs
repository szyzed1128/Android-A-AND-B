using System;

namespace SevenZip.Compression.RangeCoder
{
	// Token: 0x02000026 RID: 38
	internal struct BitDecoder
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x0000614B File Offset: 0x0000434B
		public void UpdateModel(int numMoveBits, uint symbol)
		{
			if (symbol == 0U)
			{
				this.Prob += 2048U - this.Prob >> numMoveBits;
				return;
			}
			this.Prob -= this.Prob >> numMoveBits;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006187 File Offset: 0x00004387
		public void Init()
		{
			this.Prob = 1024U;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006194 File Offset: 0x00004394
		public uint Decode(Decoder rangeDecoder)
		{
			uint num = (rangeDecoder.Range >> 11) * this.Prob;
			if (rangeDecoder.Code < num)
			{
				rangeDecoder.Range = num;
				this.Prob += 2048U - this.Prob >> 5;
				if (rangeDecoder.Range < 16777216U)
				{
					rangeDecoder.Code = (rangeDecoder.Code << 8) | (uint)((byte)rangeDecoder.Stream.ReadByte());
					rangeDecoder.Range <<= 8;
				}
				return 0U;
			}
			rangeDecoder.Range -= num;
			rangeDecoder.Code -= num;
			this.Prob -= this.Prob >> 5;
			if (rangeDecoder.Range < 16777216U)
			{
				rangeDecoder.Code = (rangeDecoder.Code << 8) | (uint)((byte)rangeDecoder.Stream.ReadByte());
				rangeDecoder.Range <<= 8;
			}
			return 1U;
		}

		// Token: 0x0400009B RID: 155
		public const int kNumBitModelTotalBits = 11;

		// Token: 0x0400009C RID: 156
		public const uint kBitModelTotal = 2048U;

		// Token: 0x0400009D RID: 157
		private const int kNumMoveBits = 5;

		// Token: 0x0400009E RID: 158
		private uint Prob;
	}
}
