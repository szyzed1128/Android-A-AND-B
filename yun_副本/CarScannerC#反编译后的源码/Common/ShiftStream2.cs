using System;
using System.IO;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007FE RID: 2046
	internal class ShiftStream2 : Stream
	{
		// Token: 0x0600476D RID: 18285 RVA: 0x0036E3AE File Offset: 0x0036C5AE
		public ShiftStream2(Stream stream)
		{
			this.baseStream = stream;
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x0036E3BD File Offset: 0x0036C5BD
		private byte ShiftWriteByte(byte b, long position)
		{
			if (position % 2L == 0L)
			{
				b += 88;
				b += (byte)position;
			}
			else
			{
				b -= 69;
				b -= (byte)position;
			}
			return b;
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x0036E3E4 File Offset: 0x0036C5E4
		private byte ShiftReadByte(byte b, long position)
		{
			if (position % 2L == 0L)
			{
				b -= 88;
				b -= (byte)position;
			}
			else
			{
				b += 69;
				b += (byte)position;
			}
			return b;
		}

		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x06004770 RID: 18288 RVA: 0x0036E40B File Offset: 0x0036C60B
		public override bool CanRead
		{
			get
			{
				return this.baseStream.CanRead;
			}
		}

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x06004771 RID: 18289 RVA: 0x0036E418 File Offset: 0x0036C618
		public override bool CanSeek
		{
			get
			{
				return this.baseStream.CanSeek;
			}
		}

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x06004772 RID: 18290 RVA: 0x0036E425 File Offset: 0x0036C625
		public override bool CanWrite
		{
			get
			{
				return this.baseStream.CanWrite;
			}
		}

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06004773 RID: 18291 RVA: 0x0036E432 File Offset: 0x0036C632
		public override long Length
		{
			get
			{
				return this.baseStream.Length;
			}
		}

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x06004774 RID: 18292 RVA: 0x0036E43F File Offset: 0x0036C63F
		// (set) Token: 0x06004775 RID: 18293 RVA: 0x0036E44C File Offset: 0x0036C64C
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

		// Token: 0x06004776 RID: 18294 RVA: 0x0036E45A File Offset: 0x0036C65A
		public override void Flush()
		{
			this.baseStream.Flush();
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0036E468 File Offset: 0x0036C668
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

		// Token: 0x06004778 RID: 18296 RVA: 0x0036E4C0 File Offset: 0x0036C6C0
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.baseStream.Seek(offset, origin);
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0036E4CF File Offset: 0x0036C6CF
		public override void SetLength(long value)
		{
			this.baseStream.SetLength(value);
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0036E4E0 File Offset: 0x0036C6E0
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

		// Token: 0x04002996 RID: 10646
		private readonly Stream baseStream;
	}
}
