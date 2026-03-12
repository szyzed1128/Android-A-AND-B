using System;
using System.IO;

namespace SevenZip.Buffer
{
	// Token: 0x02000022 RID: 34
	public class OutBuffer
	{
		// Token: 0x060000CB RID: 203 RVA: 0x00005AA9 File Offset: 0x00003CA9
		public OutBuffer(uint bufferSize)
		{
			this.m_Buffer = new byte[bufferSize];
			this.m_BufferSize = bufferSize;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005AC4 File Offset: 0x00003CC4
		public void SetStream(Stream stream)
		{
			this.m_Stream = stream;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005ACD File Offset: 0x00003CCD
		public void FlushStream()
		{
			this.m_Stream.Flush();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005ADA File Offset: 0x00003CDA
		public void CloseStream()
		{
			this.m_Stream.Close();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005AE7 File Offset: 0x00003CE7
		public void ReleaseStream()
		{
			this.m_Stream = null;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005AF0 File Offset: 0x00003CF0
		public void Init()
		{
			this.m_ProcessedSize = 0UL;
			this.m_Pos = 0U;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005B04 File Offset: 0x00003D04
		public void WriteByte(byte b)
		{
			byte[] buffer = this.m_Buffer;
			uint pos = this.m_Pos;
			this.m_Pos = pos + 1U;
			buffer[(int)pos] = b;
			if (this.m_Pos >= this.m_BufferSize)
			{
				this.FlushData();
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005B3E File Offset: 0x00003D3E
		public void FlushData()
		{
			if (this.m_Pos == 0U)
			{
				return;
			}
			this.m_Stream.Write(this.m_Buffer, 0, (int)this.m_Pos);
			this.m_Pos = 0U;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005B68 File Offset: 0x00003D68
		public ulong GetProcessedSize()
		{
			return this.m_ProcessedSize + (ulong)this.m_Pos;
		}

		// Token: 0x04000084 RID: 132
		private byte[] m_Buffer;

		// Token: 0x04000085 RID: 133
		private uint m_Pos;

		// Token: 0x04000086 RID: 134
		private uint m_BufferSize;

		// Token: 0x04000087 RID: 135
		private Stream m_Stream;

		// Token: 0x04000088 RID: 136
		private ulong m_ProcessedSize;
	}
}
