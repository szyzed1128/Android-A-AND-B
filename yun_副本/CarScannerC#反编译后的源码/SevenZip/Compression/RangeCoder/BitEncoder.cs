using System;

namespace SevenZip.Compression.RangeCoder
{
	// Token: 0x02000025 RID: 37
	internal struct BitEncoder
	{
		// Token: 0x060000EA RID: 234 RVA: 0x00005FC8 File Offset: 0x000041C8
		public void Init()
		{
			this.Prob = 1024U;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005FD5 File Offset: 0x000041D5
		public void UpdateModel(uint symbol)
		{
			if (symbol == 0U)
			{
				this.Prob += 2048U - this.Prob >> 5;
				return;
			}
			this.Prob -= this.Prob >> 5;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000600C File Offset: 0x0000420C
		public void Encode(Encoder encoder, uint symbol)
		{
			uint num = (encoder.Range >> 11) * this.Prob;
			if (symbol == 0U)
			{
				encoder.Range = num;
				this.Prob += 2048U - this.Prob >> 5;
			}
			else
			{
				encoder.Low += (ulong)num;
				encoder.Range -= num;
				this.Prob -= this.Prob >> 5;
			}
			if (encoder.Range < 16777216U)
			{
				encoder.Range <<= 8;
				encoder.ShiftLow();
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000060A4 File Offset: 0x000042A4
		static BitEncoder()
		{
			for (int i = 8; i >= 0; i--)
			{
				uint num = 1U << 9 - i - 1;
				uint num2 = 1U << 9 - i;
				for (uint num3 = num; num3 < num2; num3 += 1U)
				{
					BitEncoder.ProbPrices[(int)num3] = (uint)((i << 6) + (int)(num2 - num3 << 6 >> 9 - i - 1));
				}
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006106 File Offset: 0x00004306
		public uint GetPrice(uint symbol)
		{
			checked
			{
				return BitEncoder.ProbPrices[(int)((IntPtr)((unchecked((ulong)(this.Prob - symbol) ^ (ulong)((long)(-(long)symbol))) & 2047UL) >> 2))];
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006125 File Offset: 0x00004325
		public uint GetPrice0()
		{
			return BitEncoder.ProbPrices[(int)(this.Prob >> 2)];
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006135 File Offset: 0x00004335
		public uint GetPrice1()
		{
			return BitEncoder.ProbPrices[(int)(2048U - this.Prob >> 2)];
		}

		// Token: 0x04000094 RID: 148
		public const int kNumBitModelTotalBits = 11;

		// Token: 0x04000095 RID: 149
		public const uint kBitModelTotal = 2048U;

		// Token: 0x04000096 RID: 150
		private const int kNumMoveBits = 5;

		// Token: 0x04000097 RID: 151
		private const int kNumMoveReducingBits = 2;

		// Token: 0x04000098 RID: 152
		public const int kNumBitPriceShiftBits = 6;

		// Token: 0x04000099 RID: 153
		private uint Prob;

		// Token: 0x0400009A RID: 154
		private static uint[] ProbPrices = new uint[512];
	}
}
