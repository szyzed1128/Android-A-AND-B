using System;
using System.IO;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007FF RID: 2047
	internal class ShiftStream3 : Stream
	{
		// Token: 0x0600477B RID: 18299 RVA: 0x0036E530 File Offset: 0x0036C730
		public ShiftStream3(Stream stream, byte[] key)
		{
			this.baseStream = stream;
			int num = (int)(key[0] + key[3]);
			int num2 = (int)(key[1] + key[2]);
			if (num > num2)
			{
				this.b1 = key[0];
				this.b2 = key[3];
				return;
			}
			this.b1 = key[1];
			this.b2 = key[2];
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x0036E581 File Offset: 0x0036C781
		private byte ShiftWriteByte(byte b, long position)
		{
			if (position % 2L == 0L)
			{
				b += 88;
				b += this.b1;
				b += (byte)position;
			}
			else
			{
				b -= 69;
				b -= this.b2;
				b -= (byte)position;
			}
			return b;
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0036E5BE File Offset: 0x0036C7BE
		private byte ShiftReadByte(byte b, long position)
		{
			if (position % 2L == 0L)
			{
				b -= 88;
				b -= this.b1;
				b -= (byte)position;
			}
			else
			{
				b += 69;
				b += this.b2;
				b += (byte)position;
			}
			return b;
		}

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x0600477E RID: 18302 RVA: 0x0036E5FB File Offset: 0x0036C7FB
		public override bool CanRead
		{
			get
			{
				return this.baseStream.CanRead;
			}
		}

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x0600477F RID: 18303 RVA: 0x0036E608 File Offset: 0x0036C808
		public override bool CanSeek
		{
			get
			{
				return this.baseStream.CanSeek;
			}
		}

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x06004780 RID: 18304 RVA: 0x0036E615 File Offset: 0x0036C815
		public override bool CanWrite
		{
			get
			{
				return this.baseStream.CanWrite;
			}
		}

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x06004781 RID: 18305 RVA: 0x0036E622 File Offset: 0x0036C822
		public override long Length
		{
			get
			{
				return this.baseStream.Length;
			}
		}

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x06004782 RID: 18306 RVA: 0x0036E62F File Offset: 0x0036C82F
		// (set) Token: 0x06004783 RID: 18307 RVA: 0x0036E63C File Offset: 0x0036C83C
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

		// Token: 0x06004784 RID: 18308 RVA: 0x0036E64A File Offset: 0x0036C84A
		public override void Flush()
		{
			this.baseStream.Flush();
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0036E658 File Offset: 0x0036C858
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

		// Token: 0x06004786 RID: 18310 RVA: 0x0036E6B0 File Offset: 0x0036C8B0
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.baseStream.Seek(offset, origin);
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0036E6BF File Offset: 0x0036C8BF
		public override void SetLength(long value)
		{
			this.baseStream.SetLength(value);
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0036E6D0 File Offset: 0x0036C8D0
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

		// Token: 0x04002997 RID: 10647
		private byte b1;

		// Token: 0x04002998 RID: 10648
		private byte b2;

		// Token: 0x04002999 RID: 10649
		private readonly Stream baseStream;
	}
}
