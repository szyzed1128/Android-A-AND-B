using System;
using System.IO;
using System.Text;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007C9 RID: 1993
	public class DiffDecodeStream : Stream
	{
		// Token: 0x060046A0 RID: 18080 RVA: 0x0036B182 File Offset: 0x00369382
		public DiffDecodeStream(Stream oldDataStream, Stream newDataStream)
		{
			this.oldDataStream = oldDataStream;
			this.newDataStream = newDataStream;
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x0036B198 File Offset: 0x00369398
		public void Init()
		{
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(this.newDataStream, Encoding.Default, true))
				{
					this.ms = new MemoryStream();
					byte[] array = binaryReader.ReadBytes(4);
					long num = binaryReader.ReadInt64();
					this.ms = new MemoryStream((int)num);
					this.oldDataStream.Seek(0L, SeekOrigin.Begin);
					if (num < this.oldDataStream.Length)
					{
						byte[] array2 = new byte[num];
						this.oldDataStream.Read(array2, 0, array2.Length);
						this.ms.Write(array2, 0, array2.Length);
					}
					else
					{
						this.oldDataStream.CopyTo(this.ms);
					}
					this.ms.Seek(0L, SeekOrigin.Begin);
					while (this.newDataStream.Position < this.newDataStream.Length)
					{
						uint num2 = binaryReader.ReadUInt32();
						uint num3 = binaryReader.ReadUInt32();
						if (this.newDataStream.Length - this.newDataStream.Position < (long)((ulong)num3))
						{
							throw new Exception();
						}
						byte[] array3 = binaryReader.ReadBytes((int)num3);
						this.ms.Seek((long)((ulong)num2), SeekOrigin.Begin);
						this.ms.Write(array3, 0, array3.Length);
					}
					this.ms.Seek(0L, SeekOrigin.Begin);
					byte[] array4 = Crc32.Calculate(this.ms);
					if (array[0] != array4[0] || array[1] != array4[1] || array[2] != array4[2] || array[3] != array4[3])
					{
						throw new Exception();
					}
					if (this.ms.Length != num)
					{
						throw new Exception();
					}
				}
				this.ms.Seek(0L, SeekOrigin.Begin);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x00017A6F File Offset: 0x00015C6F
		public override void Flush()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x0036B36C File Offset: 0x0036956C
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.ms.Read(buffer, offset, count);
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x0036B37C File Offset: 0x0036957C
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.ms.Seek((long)((int)offset), origin);
		}

		// Token: 0x060046A5 RID: 18085 RVA: 0x00017A6F File Offset: 0x00015C6F
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x00017A6F File Offset: 0x00015C6F
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x060046A7 RID: 18087 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x060046A8 RID: 18088 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x060046A9 RID: 18089 RVA: 0x00002076 File Offset: 0x00000276
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x060046AA RID: 18090 RVA: 0x0036B38D File Offset: 0x0036958D
		public override long Length
		{
			get
			{
				return this.ms.Length;
			}
		}

		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x060046AB RID: 18091 RVA: 0x0036B39A File Offset: 0x0036959A
		// (set) Token: 0x060046AC RID: 18092 RVA: 0x0036B3A7 File Offset: 0x003695A7
		public override long Position
		{
			get
			{
				return this.ms.Position;
			}
			set
			{
				this.ms.Position = value;
			}
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x0036B3B8 File Offset: 0x003695B8
		protected override void Dispose(bool disposing)
		{
			try
			{
				this.ms.Dispose();
			}
			catch (Exception)
			{
			}
			base.Dispose(disposing);
		}

		// Token: 0x04002903 RID: 10499
		private Stream oldDataStream;

		// Token: 0x04002904 RID: 10500
		private Stream newDataStream;

		// Token: 0x04002905 RID: 10501
		private MemoryStream ms;
	}
}
