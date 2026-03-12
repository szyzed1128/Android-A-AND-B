using System;
using System.IO;

namespace SevenZip.Buffer
{
	// Token: 0x02000021 RID: 33
	public class InBuffer
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00005964 File Offset: 0x00003B64
		public InBuffer(uint bufferSize)
		{
			this.m_Buffer = new byte[bufferSize];
			this.m_BufferSize = bufferSize;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000597F File Offset: 0x00003B7F
		public void Init(Stream stream)
		{
			this.m_Stream = stream;
			this.m_ProcessedSize = 0UL;
			this.m_Limit = 0U;
			this.m_Pos = 0U;
			this.m_StreamWasExhausted = false;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000059A8 File Offset: 0x00003BA8
		public bool ReadBlock()
		{
			if (this.m_StreamWasExhausted)
			{
				return false;
			}
			this.m_ProcessedSize += (ulong)this.m_Pos;
			int num = this.m_Stream.Read(this.m_Buffer, 0, (int)this.m_BufferSize);
			this.m_Pos = 0U;
			this.m_Limit = (uint)num;
			this.m_StreamWasExhausted = num == 0;
			return !this.m_StreamWasExhausted;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005A0D File Offset: 0x00003C0D
		public void ReleaseStream()
		{
			this.m_Stream = null;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005A18 File Offset: 0x00003C18
		public bool ReadByte(byte b)
		{
			if (this.m_Pos >= this.m_Limit && !this.ReadBlock())
			{
				return false;
			}
			byte[] buffer = this.m_Buffer;
			uint pos = this.m_Pos;
			this.m_Pos = pos + 1U;
			b = buffer[(int)pos];
			return true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005A58 File Offset: 0x00003C58
		public byte ReadByte()
		{
			if (this.m_Pos >= this.m_Limit && !this.ReadBlock())
			{
				return byte.MaxValue;
			}
			byte[] buffer = this.m_Buffer;
			uint pos = this.m_Pos;
			this.m_Pos = pos + 1U;
			return buffer[(int)pos];
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005A99 File Offset: 0x00003C99
		public ulong GetProcessedSize()
		{
			return this.m_ProcessedSize + (ulong)this.m_Pos;
		}

		// Token: 0x0400007D RID: 125
		private byte[] m_Buffer;

		// Token: 0x0400007E RID: 126
		private uint m_Pos;

		// Token: 0x0400007F RID: 127
		private uint m_Limit;

		// Token: 0x04000080 RID: 128
		private uint m_BufferSize;

		// Token: 0x04000081 RID: 129
		private Stream m_Stream;

		// Token: 0x04000082 RID: 130
		private bool m_StreamWasExhausted;

		// Token: 0x04000083 RID: 131
		private ulong m_ProcessedSize;
	}
}
