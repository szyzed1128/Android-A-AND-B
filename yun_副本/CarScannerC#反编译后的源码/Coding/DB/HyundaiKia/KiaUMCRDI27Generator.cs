using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BBF RID: 3007
	internal static class KiaUMCRDI27Generator
	{
		// Token: 0x06005B00 RID: 23296 RVA: 0x00436708 File Offset: 0x00434908
		private static int calc(int seed)
		{
			for (int i = 0; i <= 34; i++)
			{
				if (((long)seed & (long)((ulong)(-2147483648))) == (long)((ulong)(-2147483648)))
				{
					seed = (seed << 1) ^ 2006460816;
				}
				else
				{
					seed <<= 1;
				}
			}
			return seed;
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x00436748 File Offset: 0x00434948
		private static string GetKey(string seed)
		{
			byte[] bytes = BitConverter.GetBytes(KiaUMCRDI27Generator.calc(BitHelpers.ConvertHexToInt(seed)));
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(bytes);
			}
			return BitHelpers.ByteArrayToHexString(bytes);
		}

		// Token: 0x06005B02 RID: 23298 RVA: 0x0043677C File Offset: 0x0043497C
		public static void SetPasswordDecodeFor2701(OBDRequest req)
		{
			if (req.Command == "2701" && (req.Header == "7E0" || req.Header == "7E1"))
			{
				req.ResponseMarker = "6701";
				req.ResponseDecoded += KiaUMCRDI27Generator.Req2701_ResponseDecoded;
				req.DoNotDecode = false;
			}
		}

		// Token: 0x06005B03 RID: 23299 RVA: 0x004367E4 File Offset: 0x004349E4
		private static void Req2701_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			try
			{
				if (data != null && data.Length >= 4)
				{
					OBDRequest obdrequest = App.OBDReader.GetQueueCopy().FirstOrDefault((OBDRequest x) => x.Command == "2702");
					if (obdrequest != null)
					{
						string text = BitHelpers.ByteArrayToHexString(data);
						if (text.Length > 8)
						{
							text = text.Substring(0, 8);
						}
						string key = KiaUMCRDI27Generator.GetKey(text);
						obdrequest.Command = "2702" + key;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x02000BC0 RID: 3008
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005B04 RID: 23300 RVA: 0x00436874 File Offset: 0x00434A74
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005B05 RID: 23301 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005B06 RID: 23302 RVA: 0x004366F4 File Offset: 0x004348F4
			internal bool <Req2701_ResponseDecoded>b__3_0(OBDRequest x)
			{
				return x.Command == "2702";
			}

			// Token: 0x0400395F RID: 14687
			public static readonly KiaUMCRDI27Generator.<>c <>9 = new KiaUMCRDI27Generator.<>c();

			// Token: 0x04003960 RID: 14688
			public static Func<OBDRequest, bool> <>9__3_0;
		}
	}
}
