using System;
using System.IO;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007FD RID: 2045
	internal class ShiftStream : Stream
	{
		// Token: 0x0600475D RID: 18269 RVA: 0x0036E223 File Offset: 0x0036C423
		public ShiftStream(Stream stream)
		{
			this.baseStream = stream;
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x0036E232 File Offset: 0x0036C432
		private byte ShiftWriteByte(byte b)
		{
			b += 56;
			b += (byte)this.baseStream.Position;
			return b;
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x0036E24D File Offset: 0x0036C44D
		private byte ShiftWriteByte(byte b, long position)
		{
			b += 56;
			b += (byte)position;
			return b;
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x0036E25E File Offset: 0x0036C45E
		private byte ShiftReadByte(byte b)
		{
			b -= 56;
			b -= (byte)this.baseStream.Position;
			return b;
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x0036E279 File Offset: 0x0036C479
		private byte ShiftReadByte(byte b, long position)
		{
			b -= 56;
			b -= (byte)position;
			return b;
		}

		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06004762 RID: 18274 RVA: 0x0036E28A File Offset: 0x0036C48A
		public override bool CanRead
		{
			get
			{
				return this.baseStream.CanRead;
			}
		}

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x06004763 RID: 18275 RVA: 0x0036E297 File Offset: 0x0036C497
		public override bool CanSeek
		{
			get
			{
				return this.baseStream.CanSeek;
			}
		}

		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x06004764 RID: 18276 RVA: 0x0036E2A4 File Offset: 0x0036C4A4
		public override bool CanWrite
		{
			get
			{
				return this.baseStream.CanWrite;
			}
		}

		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x06004765 RID: 18277 RVA: 0x0036E2B1 File Offset: 0x0036C4B1
		public override long Length
		{
			get
			{
				return this.baseStream.Length;
			}
		}

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06004766 RID: 18278 RVA: 0x0036E2BE File Offset: 0x0036C4BE
		// (set) Token: 0x06004767 RID: 18279 RVA: 0x0036E2CB File Offset: 0x0036C4CB
		public override long Position
		{
			get
			{
				return this.baseStream.Position;
			}
			set
			{
				this.baseStream.Position = value;
			}
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0036E2D9 File Offset: 0x0036C4D9
		public override void Flush()
		{
			this.baseStream.Flush();
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x0036E2E8 File Offset: 0x0036C4E8
		public override int Read(byte[] buffer, int offset, int count)
		{
			byte[] array = new byte[count];
			long position = this.baseStream.Position;
			int num = this.baseStream.Read(array, 0, count);
			int num2 = 0;
			while (num2 < num && num2 < array.Length)
			{
				byte b = this.ShiftReadByte(array[num2], position + (long)num2);
				buffer[offset + num2] = b;
				num2++;
			}
			return num;
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x0036E340 File Offset: 0x0036C540
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.baseStream.Seek(offset, origin);
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0036E34F File Offset: 0x0036C54F
		public override void SetLength(long value)
		{
			this.baseStream.SetLength(value);
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x0036E360 File Offset: 0x0036C560
		public override void Write(byte[] buffer, int offset, int count)
		{
			byte[] array = new byte[buffer.Length];
			long position = this.baseStream.Position;
			for (int i = offset; i < offset + count; i++)
			{
				array[i] = this.ShiftWriteByte(buffer[i], position + (long)i);
			}
			this.baseStream.Write(array, offset, count);
		}

		// Token: 0x04002995 RID: 10645
		private readonly Stream baseStream;
	}
}
