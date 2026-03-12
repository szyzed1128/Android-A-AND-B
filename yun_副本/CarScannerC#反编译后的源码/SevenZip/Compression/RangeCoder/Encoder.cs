using System;
using System.IO;

namespace SevenZip.Compression.RangeCoder
{
	// Token: 0x02000023 RID: 35
	internal class Encoder
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00005B78 File Offset: 0x00003D78
		public void SetStream(Stream stream)
		{
			this.Stream = stream;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005B81 File Offset: 0x00003D81
		public void ReleaseStream()
		{
			this.Stream = null;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005B8A File Offset: 0x00003D8A
		public void Init()
		{
			this.StartPosition = this.Stream.Position;
			this.Low = 0UL;
			this.Range = uint.MaxValue;
			this._cacheSize = 1U;
			this._cache = 0;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005BBC File Offset: 0x00003DBC
		public void FlushData()
		{
			for (int i = 0; i < 5; i++)
			{
				this.ShiftLow();
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005BDB File Offset: 0x00003DDB
		public void FlushStream()
		{
			this.Stream.Flush();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005BE8 File Offset: 0x00003DE8
		public void CloseStream()
		{
			this.Stream.Close();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005BF8 File Offset: 0x00003DF8
		public void Encode(uint start, uint size, uint total)
		{
			this.Low += (ulong)(start * (this.Range /= total));
			this.Range *= size;
			while (this.Range < 16777216U)
			{
				this.Range <<= 8;
				this.ShiftLow();
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005C58 File Offset: 0x00003E58
		public void ShiftLow()
		{
			if ((uint)this.Low < 4278190080U || (uint)(this.Low >> 32) == 1U)
			{
				byte b = this._cache;
				uint num;
				do
				{
					this.Stream.WriteByte((byte)((ulong)b + (this.Low >> 32)));
					b = byte.MaxValue;
					num = this._cacheSize - 1U;
					this._cacheSize = num;
				}
				while (num != 0U);
				this._cache = (byte)((uint)this.Low >> 24);
			}
			this._cacheSize += 1U;
			this.Low = (ulong)((ulong)((uint)this.Low) << 8);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005CE8 File Offset: 0x00003EE8
		public void EncodeDirectBits(uint v, int numTotalBits)
		{
			for (int i = numTotalBits - 1; i >= 0; i--)
			{
				this.Range >>= 1;
				if (((v >> i) & 1U) == 1U)
				{
					this.Low += (ulong)this.Range;
				}
				if (this.Range < 16777216U)
				{
					this.Range <<= 8;
					this.ShiftLow();
				}
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005D54 File Offset: 0x00003F54
		public void EncodeBit(uint size0, int numTotalBits, uint symbol)
		{
			uint num = (this.Range >> numTotalBits) * size0;
			if (symbol == 0U)
			{
				this.Range = num;
			}
			else
			{
				this.Low += (ulong)num;
				this.Range -= num;
			}
			while (this.Range < 16777216U)
			{
				this.Range <<= 8;
				this.ShiftLow();
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005DBB File Offset: 0x00003FBB
		public long GetProcessedSizeAdd()
		{
			return (long)((ulong)this._cacheSize + (ulong)this.Stream.Position - (ulong)this.StartPosition + 4UL);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00002050 File Offset: 0x00000250
		public Encoder()
		{
		}

		// Token: 0x04000089 RID: 137
		public const uint kTopValue = 16777216U;

		// Token: 0x0400008A RID: 138
		private Stream Stream;

		// Token: 0x0400008B RID: 139
		public ulong Low;

		// Token: 0x0400008C RID: 140
		public uint Range;

		// Token: 0x0400008D RID: 141
		private uint _cacheSize;

		// Token: 0x0400008E RID: 142
		private byte _cache;

		// Token: 0x0400008F RID: 143
		private long StartPosition;
	}
}
