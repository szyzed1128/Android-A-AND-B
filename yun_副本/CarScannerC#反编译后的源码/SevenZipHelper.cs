using System;
using System.IO;
using SevenZip;
using SevenZip.Compression.LZMA;

// Token: 0x02000003 RID: 3
public static class SevenZipHelper
{
	// Token: 0x06000005 RID: 5 RVA: 0x00002080 File Offset: 0x00000280
	public static byte[] Compress(byte[] inputBytes)
	{
		MemoryStream memoryStream = new MemoryStream(inputBytes);
		MemoryStream memoryStream2 = new MemoryStream();
		Encoder encoder = new Encoder();
		encoder.SetCoderProperties(SevenZipHelper.propIDs, SevenZipHelper.properties);
		encoder.WriteCoderProperties(memoryStream2);
		long length = memoryStream.Length;
		for (int i = 0; i < 8; i++)
		{
			memoryStream2.WriteByte((byte)(length >> 8 * i));
		}
		encoder.Code(memoryStream, memoryStream2, -1L, -1L, null);
		return memoryStream2.ToArray();
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020F4 File Offset: 0x000002F4
	public static void Compress(Stream inStream, Stream outStream)
	{
		Encoder encoder = new Encoder();
		encoder.SetCoderProperties(SevenZipHelper.propIDs, SevenZipHelper.properties);
		encoder.WriteCoderProperties(outStream);
		long length = inStream.Length;
		for (int i = 0; i < 8; i++)
		{
			outStream.WriteByte((byte)(length >> 8 * i));
		}
		encoder.Code(inStream, outStream, -1L, -1L, null);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002150 File Offset: 0x00000350
	public static byte[] Decompress(byte[] inputBytes)
	{
		MemoryStream memoryStream = new MemoryStream(inputBytes);
		Decoder decoder = new Decoder();
		MemoryStream memoryStream2 = new MemoryStream();
		byte[] array = new byte[5];
		if (memoryStream.Read(array, 0, 5) != 5)
		{
			throw new Exception("input .lzma is too short");
		}
		long num = 0L;
		for (int i = 0; i < 8; i++)
		{
			int num2 = memoryStream.ReadByte();
			if (num2 < 0)
			{
				throw new Exception("Can't Read 1");
			}
			num |= (long)((long)((ulong)((byte)num2)) << 8 * i);
		}
		decoder.SetDecoderProperties(array);
		long num3 = memoryStream.Length - memoryStream.Position;
		decoder.Code(memoryStream, memoryStream2, num3, num, null);
		return memoryStream2.ToArray();
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000021F4 File Offset: 0x000003F4
	public static void Decompress(Stream newInStream, Stream newOutStream)
	{
		Decoder decoder = new Decoder();
		byte[] array = new byte[5];
		if (newInStream.Read(array, 0, 5) != 5)
		{
			throw new Exception("input .lzma is too short");
		}
		long num = 0L;
		for (int i = 0; i < 8; i++)
		{
			int num2 = newInStream.ReadByte();
			if (num2 < 0)
			{
				throw new Exception("Can't Read 1");
			}
			num |= (long)((long)((ulong)((byte)num2)) << 8 * i);
		}
		decoder.SetDecoderProperties(array);
		long num3 = newInStream.Length - newInStream.Position;
		decoder.Code(newInStream, newOutStream, num3, num, null);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002280 File Offset: 0x00000480
	// Note: this type is marked as 'beforefieldinit'.
	static SevenZipHelper()
	{
	}

	// Token: 0x04000001 RID: 1
	private static int dictionary = 8388608;

	// Token: 0x04000002 RID: 2
	private static bool eos = false;

	// Token: 0x04000003 RID: 3
	private static CoderPropID[] propIDs = new CoderPropID[]
	{
		CoderPropID.DictionarySize,
		CoderPropID.PosStateBits,
		CoderPropID.LitContextBits,
		CoderPropID.LitPosBits,
		CoderPropID.Algorithm,
		CoderPropID.NumFastBytes,
		CoderPropID.MatchFinder,
		CoderPropID.EndMarker
	};

	// Token: 0x04000004 RID: 4
	private static object[] properties = new object[]
	{
		SevenZipHelper.dictionary,
		2,
		3,
		0,
		2,
		128,
		"bt4",
		SevenZipHelper.eos
	};
}
