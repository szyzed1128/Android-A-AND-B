using System;
using System.IO;

namespace SevenZip.Compression.LZ
{
	// Token: 0x0200003A RID: 58
	public class OutWindow
	{
		// Token: 0x0600017B RID: 379 RVA: 0x0000A62E File Offset: 0x0000882E
		public void Create(uint windowSize)
		{
			if (this._windowSize != windowSize)
			{
				this._buffer = new byte[windowSize];
			}
			this._windowSize = windowSize;
			this._pos = 0U;
			this._streamPos = 0U;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A65A File Offset: 0x0000885A
		public void Init(Stream stream, bool solid)
		{
			this.ReleaseStream();
			this._stream = stream;
			if (!solid)
			{
				this._streamPos = 0U;
				this._pos = 0U;
				this.TrainSize = 0U;
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A684 File Offset: 0x00008884
		public bool Train(Stream stream)
		{
			long length = stream.Length;
			uint num = ((length < (long)((ulong)this._windowSize)) ? ((uint)length) : this._windowSize);
			this.TrainSize = num;
			stream.Position = length - (long)((ulong)num);
			this._streamPos = (this._pos = 0U);
			while (num > 0U)
			{
				uint num2 = this._windowSize - this._pos;
				if (num < num2)
				{
					num2 = num;
				}
				int num3 = stream.Read(this._buffer, (int)this._pos, (int)num2);
				if (num3 == 0)
				{
					return false;
				}
				num -= (uint)num3;
				this._pos += (uint)num3;
				this._streamPos += (uint)num3;
				if (this._pos == this._windowSize)
				{
					this._streamPos = (this._pos = 0U);
				}
			}
			return true;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A745 File Offset: 0x00008945
		public void ReleaseStream()
		{
			this.Flush();
			this._stream = null;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000A754 File Offset: 0x00008954
		public void Flush()
		{
			uint num = this._pos - this._streamPos;
			if (num == 0U)
			{
				return;
			}
			this._stream.Write(this._buffer, (int)this._streamPos, (int)num);
			if (this._pos >= this._windowSize)
			{
				this._pos = 0U;
			}
			this._streamPos = this._pos;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000A7AC File Offset: 0x000089AC
		public void CopyBlock(uint distance, uint len)
		{
			uint num = this._pos - distance - 1U;
			if (num >= this._windowSize)
			{
				num += this._windowSize;
			}
			while (len > 0U)
			{
				if (num >= this._windowSize)
				{
					num = 0U;
				}
				byte[] buffer = this._buffer;
				uint pos = this._pos;
				this._pos = pos + 1U;
				buffer[(int)pos] = this._buffer[(int)num++];
				if (this._pos >= this._windowSize)
				{
					this.Flush();
				}
				len -= 1U;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000A824 File Offset: 0x00008A24
		public void PutByte(byte b)
		{
			byte[] buffer = this._buffer;
			uint pos = this._pos;
			this._pos = pos + 1U;
			buffer[(int)pos] = b;
			if (this._pos >= this._windowSize)
			{
				this.Flush();
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A860 File Offset: 0x00008A60
		public byte GetByte(uint distance)
		{
			uint num = this._pos - distance - 1U;
			if (num >= this._windowSize)
			{
				num += this._windowSize;
			}
			return this._buffer[(int)num];
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00002050 File Offset: 0x00000250
		public OutWindow()
		{
		}

		// Token: 0x04000150 RID: 336
		private byte[] _buffer;

		// Token: 0x04000151 RID: 337
		private uint _pos;

		// Token: 0x04000152 RID: 338
		private uint _windowSize;

		// Token: 0x04000153 RID: 339
		private uint _streamPos;

		// Token: 0x04000154 RID: 340
		private Stream _stream;

		// Token: 0x04000155 RID: 341
		public uint TrainSize;
	}
}
