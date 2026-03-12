using System;
using System.IO;

namespace SevenZip.Compression.LZ
{
	// Token: 0x02000038 RID: 56
	public class BinTree : InWindow, IMatchFinder, IInWindowStream
	{
		// Token: 0x0600015F RID: 351 RVA: 0x00009A08 File Offset: 0x00007C08
		public void SetType(int numHashBytes)
		{
			this.HASH_ARRAY = numHashBytes > 2;
			if (this.HASH_ARRAY)
			{
				this.kNumHashDirectBytes = 0U;
				this.kMinMatchCheck = 4U;
				this.kFixHashSize = 66560U;
				return;
			}
			this.kNumHashDirectBytes = 2U;
			this.kMinMatchCheck = 3U;
			this.kFixHashSize = 0U;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00009A56 File Offset: 0x00007C56
		public new void SetStream(Stream stream)
		{
			base.SetStream(stream);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009A5F File Offset: 0x00007C5F
		public new void ReleaseStream()
		{
			base.ReleaseStream();
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00009A68 File Offset: 0x00007C68
		public new void Init()
		{
			base.Init();
			for (uint num = 0U; num < this._hashSizeSum; num += 1U)
			{
				this._hash[(int)num] = 0U;
			}
			this._cyclicBufferPos = 0U;
			base.ReduceOffsets(-1);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009AA4 File Offset: 0x00007CA4
		public new void MovePos()
		{
			uint num = this._cyclicBufferPos + 1U;
			this._cyclicBufferPos = num;
			if (num >= this._cyclicBufferSize)
			{
				this._cyclicBufferPos = 0U;
			}
			base.MovePos();
			if (this._pos == 2147483647U)
			{
				this.Normalize();
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009AEA File Offset: 0x00007CEA
		public new byte GetIndexByte(int index)
		{
			return base.GetIndexByte(index);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009AF3 File Offset: 0x00007CF3
		public new uint GetMatchLen(int index, uint distance, uint limit)
		{
			return base.GetMatchLen(index, distance, limit);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009AFE File Offset: 0x00007CFE
		public new uint GetNumAvailableBytes()
		{
			return base.GetNumAvailableBytes();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009B08 File Offset: 0x00007D08
		public void Create(uint historySize, uint keepAddBufferBefore, uint matchMaxLen, uint keepAddBufferAfter)
		{
			if (historySize > 2147483391U)
			{
				throw new Exception();
			}
			this._cutValue = 16U + (matchMaxLen >> 1);
			uint num = (historySize + keepAddBufferBefore + matchMaxLen + keepAddBufferAfter) / 2U + 256U;
			base.Create(historySize + keepAddBufferBefore, matchMaxLen + keepAddBufferAfter, num);
			this._matchMaxLen = matchMaxLen;
			uint num2 = historySize + 1U;
			if (this._cyclicBufferSize != num2)
			{
				this._son = new uint[(this._cyclicBufferSize = num2) * 2U];
			}
			uint num3 = 65536U;
			if (this.HASH_ARRAY)
			{
				num3 = historySize - 1U;
				num3 |= num3 >> 1;
				num3 |= num3 >> 2;
				num3 |= num3 >> 4;
				num3 |= num3 >> 8;
				num3 >>= 1;
				num3 |= 65535U;
				if (num3 > 16777216U)
				{
					num3 >>= 1;
				}
				this._hashMask = num3;
				num3 += 1U;
				num3 += this.kFixHashSize;
			}
			if (num3 != this._hashSizeSum)
			{
				this._hash = new uint[this._hashSizeSum = num3];
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00009BF0 File Offset: 0x00007DF0
		public uint GetMatches(uint[] distances)
		{
			uint num;
			if (this._pos + this._matchMaxLen <= this._streamPos)
			{
				num = this._matchMaxLen;
			}
			else
			{
				num = this._streamPos - this._pos;
				if (num < this.kMinMatchCheck)
				{
					this.MovePos();
					return 0U;
				}
			}
			uint num2 = 0U;
			uint num3 = ((this._pos > this._cyclicBufferSize) ? (this._pos - this._cyclicBufferSize) : 0U);
			uint num4 = this._bufferOffset + this._pos;
			uint num5 = 1U;
			uint num6 = 0U;
			uint num7 = 0U;
			uint num10;
			if (this.HASH_ARRAY)
			{
				uint num8 = CRC.Table[(int)this._bufferBase[(int)num4]] ^ (uint)this._bufferBase[(int)(num4 + 1U)];
				num6 = num8 & 1023U;
				uint num9 = num8 ^ (uint)((uint)this._bufferBase[(int)(num4 + 2U)] << 8);
				num7 = num9 & 65535U;
				num10 = (num9 ^ (CRC.Table[(int)this._bufferBase[(int)(num4 + 3U)]] << 5)) & this._hashMask;
			}
			else
			{
				num10 = (uint)((int)this._bufferBase[(int)num4] ^ ((int)this._bufferBase[(int)(num4 + 1U)] << 8));
			}
			uint num11 = this._hash[(int)(this.kFixHashSize + num10)];
			if (this.HASH_ARRAY)
			{
				uint num12 = this._hash[(int)num6];
				uint num13 = this._hash[(int)(1024U + num7)];
				this._hash[(int)num6] = this._pos;
				this._hash[(int)(1024U + num7)] = this._pos;
				if (num12 > num3 && this._bufferBase[(int)(this._bufferOffset + num12)] == this._bufferBase[(int)num4])
				{
					num5 = (distances[(int)num2++] = 2U);
					distances[(int)num2++] = this._pos - num12 - 1U;
				}
				if (num13 > num3 && this._bufferBase[(int)(this._bufferOffset + num13)] == this._bufferBase[(int)num4])
				{
					if (num13 == num12)
					{
						num2 -= 2U;
					}
					num5 = (distances[(int)num2++] = 3U);
					distances[(int)num2++] = this._pos - num13 - 1U;
					num12 = num13;
				}
				if (num2 != 0U && num12 == num11)
				{
					num2 -= 2U;
					num5 = 1U;
				}
			}
			this._hash[(int)(this.kFixHashSize + num10)] = this._pos;
			uint num14 = (this._cyclicBufferPos << 1) + 1U;
			uint num15 = this._cyclicBufferPos << 1;
			uint num17;
			uint num16 = (num17 = this.kNumHashDirectBytes);
			if (this.kNumHashDirectBytes != 0U && num11 > num3 && this._bufferBase[(int)(this._bufferOffset + num11 + this.kNumHashDirectBytes)] != this._bufferBase[(int)(num4 + this.kNumHashDirectBytes)])
			{
				num5 = (distances[(int)num2++] = this.kNumHashDirectBytes);
				distances[(int)num2++] = this._pos - num11 - 1U;
			}
			uint cutValue = this._cutValue;
			while (num11 > num3 && cutValue-- != 0U)
			{
				uint num18 = this._pos - num11;
				uint num19 = ((num18 <= this._cyclicBufferPos) ? (this._cyclicBufferPos - num18) : (this._cyclicBufferPos - num18 + this._cyclicBufferSize)) << 1;
				uint num20 = this._bufferOffset + num11;
				uint num21 = Math.Min(num17, num16);
				if (this._bufferBase[(int)(num20 + num21)] == this._bufferBase[(int)(num4 + num21)])
				{
					while ((num21 += 1U) != num && this._bufferBase[(int)(num20 + num21)] == this._bufferBase[(int)(num4 + num21)])
					{
					}
					if (num5 < num21)
					{
						num5 = (distances[(int)num2++] = num21);
						distances[(int)num2++] = num18 - 1U;
						if (num21 == num)
						{
							this._son[(int)num15] = this._son[(int)num19];
							this._son[(int)num14] = this._son[(int)(num19 + 1U)];
							IL_03D1:
							this.MovePos();
							return num2;
						}
					}
				}
				if (this._bufferBase[(int)(num20 + num21)] < this._bufferBase[(int)(num4 + num21)])
				{
					this._son[(int)num15] = num11;
					num15 = num19 + 1U;
					num11 = this._son[(int)num15];
					num16 = num21;
				}
				else
				{
					this._son[(int)num14] = num11;
					num14 = num19;
					num11 = this._son[(int)num14];
					num17 = num21;
				}
			}
			this._son[(int)num14] = (this._son[(int)num15] = 0U);
			goto IL_03D1;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00009FD8 File Offset: 0x000081D8
		public void Skip(uint num)
		{
			for (;;)
			{
				uint num2;
				if (this._pos + this._matchMaxLen <= this._streamPos)
				{
					num2 = this._matchMaxLen;
					goto IL_0040;
				}
				num2 = this._streamPos - this._pos;
				if (num2 >= this.kMinMatchCheck)
				{
					goto IL_0040;
				}
				this.MovePos();
				IL_029A:
				if ((num -= 1U) == 0U)
				{
					break;
				}
				continue;
				IL_0040:
				uint num3 = ((this._pos > this._cyclicBufferSize) ? (this._pos - this._cyclicBufferSize) : 0U);
				uint num4 = this._bufferOffset + this._pos;
				uint num9;
				if (this.HASH_ARRAY)
				{
					uint num5 = CRC.Table[(int)this._bufferBase[(int)num4]] ^ (uint)this._bufferBase[(int)(num4 + 1U)];
					uint num6 = num5 & 1023U;
					this._hash[(int)num6] = this._pos;
					uint num7 = num5 ^ (uint)((uint)this._bufferBase[(int)(num4 + 2U)] << 8);
					uint num8 = num7 & 65535U;
					this._hash[(int)(1024U + num8)] = this._pos;
					num9 = (num7 ^ (CRC.Table[(int)this._bufferBase[(int)(num4 + 3U)]] << 5)) & this._hashMask;
				}
				else
				{
					num9 = (uint)((int)this._bufferBase[(int)num4] ^ ((int)this._bufferBase[(int)(num4 + 1U)] << 8));
				}
				uint num10 = this._hash[(int)(this.kFixHashSize + num9)];
				this._hash[(int)(this.kFixHashSize + num9)] = this._pos;
				uint num11 = (this._cyclicBufferPos << 1) + 1U;
				uint num12 = this._cyclicBufferPos << 1;
				uint num14;
				uint num13 = (num14 = this.kNumHashDirectBytes);
				uint cutValue = this._cutValue;
				while (num10 > num3 && cutValue-- != 0U)
				{
					uint num15 = this._pos - num10;
					uint num16 = ((num15 <= this._cyclicBufferPos) ? (this._cyclicBufferPos - num15) : (this._cyclicBufferPos - num15 + this._cyclicBufferSize)) << 1;
					uint num17 = this._bufferOffset + num10;
					uint num18 = Math.Min(num14, num13);
					if (this._bufferBase[(int)(num17 + num18)] == this._bufferBase[(int)(num4 + num18)])
					{
						while ((num18 += 1U) != num2 && this._bufferBase[(int)(num17 + num18)] == this._bufferBase[(int)(num4 + num18)])
						{
						}
						if (num18 == num2)
						{
							this._son[(int)num12] = this._son[(int)num16];
							this._son[(int)num11] = this._son[(int)(num16 + 1U)];
							IL_0294:
							this.MovePos();
							goto IL_029A;
						}
					}
					if (this._bufferBase[(int)(num17 + num18)] < this._bufferBase[(int)(num4 + num18)])
					{
						this._son[(int)num12] = num10;
						num12 = num16 + 1U;
						num10 = this._son[(int)num12];
						num13 = num18;
					}
					else
					{
						this._son[(int)num11] = num10;
						num11 = num16;
						num10 = this._son[(int)num11];
						num14 = num18;
					}
				}
				this._son[(int)num11] = (this._son[(int)num12] = 0U);
				goto IL_0294;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000A28C File Offset: 0x0000848C
		private void NormalizeLinks(uint[] items, uint numItems, uint subValue)
		{
			for (uint num = 0U; num < numItems; num += 1U)
			{
				uint num2 = items[(int)num];
				if (num2 <= subValue)
				{
					num2 = 0U;
				}
				else
				{
					num2 -= subValue;
				}
				items[(int)num] = num2;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000A2BC File Offset: 0x000084BC
		private void Normalize()
		{
			uint num = this._pos - this._cyclicBufferSize;
			this.NormalizeLinks(this._son, this._cyclicBufferSize * 2U, num);
			this.NormalizeLinks(this._hash, this._hashSizeSum, num);
			base.ReduceOffsets((int)num);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A306 File Offset: 0x00008506
		public void SetCutValue(uint cutValue)
		{
			this._cutValue = cutValue;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A30F File Offset: 0x0000850F
		public BinTree()
		{
		}

		// Token: 0x04000132 RID: 306
		private uint _cyclicBufferPos;

		// Token: 0x04000133 RID: 307
		private uint _cyclicBufferSize;

		// Token: 0x04000134 RID: 308
		private uint _matchMaxLen;

		// Token: 0x04000135 RID: 309
		private uint[] _son;

		// Token: 0x04000136 RID: 310
		private uint[] _hash;

		// Token: 0x04000137 RID: 311
		private uint _cutValue = 255U;

		// Token: 0x04000138 RID: 312
		private uint _hashMask;

		// Token: 0x04000139 RID: 313
		private uint _hashSizeSum;

		// Token: 0x0400013A RID: 314
		private bool HASH_ARRAY = true;

		// Token: 0x0400013B RID: 315
		private const uint kHash2Size = 1024U;

		// Token: 0x0400013C RID: 316
		private const uint kHash3Size = 65536U;

		// Token: 0x0400013D RID: 317
		private const uint kBT2HashSize = 65536U;

		// Token: 0x0400013E RID: 318
		private const uint kStartMaxLen = 1U;

		// Token: 0x0400013F RID: 319
		private const uint kHash3Offset = 1024U;

		// Token: 0x04000140 RID: 320
		private const uint kEmptyHashValue = 0U;

		// Token: 0x04000141 RID: 321
		private const uint kMaxValForNormalize = 2147483647U;

		// Token: 0x04000142 RID: 322
		private uint kNumHashDirectBytes;

		// Token: 0x04000143 RID: 323
		private uint kMinMatchCheck = 4U;

		// Token: 0x04000144 RID: 324
		private uint kFixHashSize = 66560U;
	}
}
