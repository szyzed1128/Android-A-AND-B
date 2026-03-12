using System;
using System.IO;

namespace SevenZip.Compression.LZ
{
	// Token: 0x02000039 RID: 57
	public class InWindow
	{
		// Token: 0x0600016E RID: 366 RVA: 0x0000A33C File Offset: 0x0000853C
		public void MoveBlock()
		{
			uint num = this._bufferOffset + this._pos - this._keepSizeBefore;
			if (num > 0U)
			{
				num -= 1U;
			}
			uint num2 = this._bufferOffset + this._streamPos - num;
			for (uint num3 = 0U; num3 < num2; num3 += 1U)
			{
				this._bufferBase[(int)num3] = this._bufferBase[(int)(num + num3)];
			}
			this._bufferOffset -= num;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A3A4 File Offset: 0x000085A4
		public virtual void ReadBlock()
		{
			if (this._streamEndWasReached)
			{
				return;
			}
			for (;;)
			{
				int num = (int)(0U - this._bufferOffset + this._blockSize - this._streamPos);
				if (num == 0)
				{
					break;
				}
				int num2 = this._stream.Read(this._bufferBase, (int)(this._bufferOffset + this._streamPos), num);
				if (num2 == 0)
				{
					goto Block_3;
				}
				this._streamPos += (uint)num2;
				if (this._streamPos >= this._pos + this._keepSizeAfter)
				{
					this._posLimit = this._streamPos - this._keepSizeAfter;
				}
			}
			return;
			Block_3:
			this._posLimit = this._streamPos;
			if (this._bufferOffset + this._posLimit > this._pointerToLastSafePosition)
			{
				this._posLimit = this._pointerToLastSafePosition - this._bufferOffset;
			}
			this._streamEndWasReached = true;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A471 File Offset: 0x00008671
		private void Free()
		{
			this._bufferBase = null;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A47C File Offset: 0x0000867C
		public void Create(uint keepSizeBefore, uint keepSizeAfter, uint keepSizeReserv)
		{
			this._keepSizeBefore = keepSizeBefore;
			this._keepSizeAfter = keepSizeAfter;
			uint num = keepSizeBefore + keepSizeAfter + keepSizeReserv;
			if (this._bufferBase == null || this._blockSize != num)
			{
				this.Free();
				this._blockSize = num;
				this._bufferBase = new byte[this._blockSize];
			}
			this._pointerToLastSafePosition = this._blockSize - keepSizeAfter;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A4DA File Offset: 0x000086DA
		public void SetStream(Stream stream)
		{
			this._stream = stream;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A4E3 File Offset: 0x000086E3
		public void ReleaseStream()
		{
			this._stream = null;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A4EC File Offset: 0x000086EC
		public void Init()
		{
			this._bufferOffset = 0U;
			this._pos = 0U;
			this._streamPos = 0U;
			this._streamEndWasReached = false;
			this.ReadBlock();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A510 File Offset: 0x00008710
		public void MovePos()
		{
			this._pos += 1U;
			if (this._pos > this._posLimit)
			{
				if (this._bufferOffset + this._pos > this._pointerToLastSafePosition)
				{
					this.MoveBlock();
				}
				this.ReadBlock();
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A54F File Offset: 0x0000874F
		public byte GetIndexByte(int index)
		{
			checked
			{
				return this._bufferBase[(int)((IntPtr)(unchecked((ulong)(this._bufferOffset + this._pos) + (ulong)((long)index))))];
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A56C File Offset: 0x0000876C
		public uint GetMatchLen(int index, uint distance, uint limit)
		{
			if (this._streamEndWasReached && (ulong)this._pos + (ulong)((long)index) + (ulong)limit > (ulong)this._streamPos)
			{
				limit = this._streamPos - (uint)((ulong)this._pos + (ulong)((long)index));
			}
			distance += 1U;
			uint num = this._bufferOffset + this._pos + (uint)index;
			uint num2 = 0U;
			while (num2 < limit && this._bufferBase[(int)(num + num2)] == this._bufferBase[(int)(num + num2 - distance)])
			{
				num2 += 1U;
			}
			return num2;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A5E5 File Offset: 0x000087E5
		public uint GetNumAvailableBytes()
		{
			return this._streamPos - this._pos;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A5F4 File Offset: 0x000087F4
		public void ReduceOffsets(int subValue)
		{
			this._bufferOffset += (uint)subValue;
			this._posLimit -= (uint)subValue;
			this._pos -= (uint)subValue;
			this._streamPos -= (uint)subValue;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00002050 File Offset: 0x00000250
		public InWindow()
		{
		}

		// Token: 0x04000145 RID: 325
		public byte[] _bufferBase;

		// Token: 0x04000146 RID: 326
		private Stream _stream;

		// Token: 0x04000147 RID: 327
		private uint _posLimit;

		// Token: 0x04000148 RID: 328
		private bool _streamEndWasReached;

		// Token: 0x04000149 RID: 329
		private uint _pointerToLastSafePosition;

		// Token: 0x0400014A RID: 330
		public uint _bufferOffset;

		// Token: 0x0400014B RID: 331
		public uint _blockSize;

		// Token: 0x0400014C RID: 332
		public uint _pos;

		// Token: 0x0400014D RID: 333
		private uint _keepSizeBefore;

		// Token: 0x0400014E RID: 334
		private uint _keepSizeAfter;

		// Token: 0x0400014F RID: 335
		public uint _streamPos;
	}
}
