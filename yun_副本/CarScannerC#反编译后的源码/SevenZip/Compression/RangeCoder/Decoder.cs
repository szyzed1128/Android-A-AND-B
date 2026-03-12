using System;
using System.IO;

namespace SevenZip.Compression.RangeCoder
{
	// Token: 0x02000024 RID: 36
	internal class Decoder
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00005DDC File Offset: 0x00003FDC
		public void Init(Stream stream)
		{
			this.Stream = stream;
			this.Code = 0U;
			this.Range = uint.MaxValue;
			for (int i = 0; i < 5; i++)
			{
				this.Code = (this.Code << 8) | (uint)((byte)this.Stream.ReadByte());
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005E25 File Offset: 0x00004025
		public void ReleaseStream()
		{
			this.Stream = null;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005E2E File Offset: 0x0000402E
		public void CloseStream()
		{
			this.Stream.Close();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005E3B File Offset: 0x0000403B
		public void Normalize()
		{
			while (this.Range < 16777216U)
			{
				this.Code = (this.Code << 8) | (uint)((byte)this.Stream.ReadByte());
				this.Range <<= 8;
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005E75 File Offset: 0x00004075
		public void Normalize2()
		{
			if (this.Range < 16777216U)
			{
				this.Code = (this.Code << 8) | (uint)((byte)this.Stream.ReadByte());
				this.Range <<= 8;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005EB0 File Offset: 0x000040B0
		public uint GetThreshold(uint total)
		{
			return this.Code / (this.Range /= total);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005ED5 File Offset: 0x000040D5
		public void Decode(uint start, uint size, uint total)
		{
			this.Code -= start * this.Range;
			this.Range *= size;
			this.Normalize();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005F00 File Offset: 0x00004100
		public uint DecodeDirectBits(int numTotalBits)
		{
			uint num = this.Range;
			uint num2 = this.Code;
			uint num3 = 0U;
			for (int i = numTotalBits; i > 0; i--)
			{
				num >>= 1;
				uint num4 = num2 - num >> 31;
				num2 -= num & (num4 - 1U);
				num3 = (num3 << 1) | (1U - num4);
				if (num < 16777216U)
				{
					num2 = (num2 << 8) | (uint)((byte)this.Stream.ReadByte());
					num <<= 8;
				}
			}
			this.Range = num;
			this.Code = num2;
			return num3;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005F74 File Offset: 0x00004174
		public uint DecodeBit(uint size0, int numTotalBits)
		{
			uint num = (this.Range >> numTotalBits) * size0;
			uint num2;
			if (this.Code < num)
			{
				num2 = 0U;
				this.Range = num;
			}
			else
			{
				num2 = 1U;
				this.Code -= num;
				this.Range -= num;
			}
			this.Normalize();
			return num2;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00002050 File Offset: 0x00000250
		public Decoder()
		{
		}

		// Token: 0x04000090 RID: 144
		public const uint kTopValue = 16777216U;

		// Token: 0x04000091 RID: 145
		public uint Range;

		// Token: 0x04000092 RID: 146
		public uint Code;

		// Token: 0x04000093 RID: 147
		public Stream Stream;
	}
}
