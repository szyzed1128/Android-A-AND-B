using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BB7 RID: 2999
	internal class KiaBCM27Generator
	{
		// Token: 0x06005AE6 RID: 23270 RVA: 0x0043600C File Offset: 0x0043420C
		private static void calc(byte[] inBuf8, byte[] outBuf8)
		{
			byte[] array = new byte[8];
			Array.Copy(inBuf8, array, 8);
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				num ^= (int)inBuf8[i];
			}
			for (int j = 0; j < 382; j++)
			{
				byte b = KiaBCM27Generator.rotate((int)KiaBCM27Generator.summ(array[1], array[0]), 1) ^ array[2];
				byte b2 = KiaBCM27Generator.rotate((int)KiaBCM27Generator.summ(array[1], b), 2) ^ array[3];
				byte b3 = KiaBCM27Generator.rotate((int)KiaBCM27Generator.summ(b2, b), 5) ^ array[4];
				byte b4 = KiaBCM27Generator.rotate((int)KiaBCM27Generator.summ(b3, b2), 7) ^ array[5];
				byte b5 = KiaBCM27Generator.rotate((int)KiaBCM27Generator.summ(b4, b3), 3) ^ array[6];
				byte b6 = KiaBCM27Generator.rotate((int)KiaBCM27Generator.summ(b5, b4), 6) ^ array[7];
				byte b7 = KiaBCM27Generator.rotate((int)KiaBCM27Generator.summ(b6, b5), 4) ^ array[0];
				byte b8 = KiaBCM27Generator.rotate((int)KiaBCM27Generator.summ(b2, b7), 3) ^ array[1];
				array[0] = b6;
				array[1] = b7;
				array[2] = b8;
				array[3] = b;
				array[4] = b2;
				array[5] = b3;
				array[6] = b4;
				array[7] = b5;
				if (j == num + 52 - 1)
				{
					Array.Copy(array, outBuf8, 8);
				}
			}
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x0043613E File Offset: 0x0043433E
		private static byte summ(byte b1, byte b2)
		{
			return b1 + b2;
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x00436144 File Offset: 0x00434344
		private static byte rotate(int val, int shift)
		{
			return (byte)((val >> 8 - shift) | (val << shift));
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x00436158 File Offset: 0x00434358
		private static string GetKey(string seed)
		{
			byte[] array = BitHelpers.ConvertHexToBytesX(seed);
			byte[] array2 = new byte[8];
			KiaBCM27Generator.calc(array, array2);
			return BitHelpers.ByteArrayToHexString(array2);
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x00436180 File Offset: 0x00434380
		public static void SetPasswordDecodeFor2711(OBDRequest req)
		{
			if (req.Command == "2711" && req.Header == "7A0")
			{
				req.ResponseMarker = "6711";
				req.ResponseDecoded += KiaBCM27Generator.Req2711_ResponseDecoded;
				req.DoNotDecode = false;
			}
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x004361D8 File Offset: 0x004343D8
		private static void Req2711_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			try
			{
				if (data != null && data.Length >= 8)
				{
					OBDRequest obdrequest = App.OBDReader.GetQueueCopy().FirstOrDefault((OBDRequest x) => x.Command == "2712");
					if (obdrequest != null)
					{
						string text = BitHelpers.ByteArrayToHexString(data);
						if (text.Length > 16)
						{
							text = text.Substring(0, 16);
						}
						string key = KiaBCM27Generator.GetKey(text);
						obdrequest.Command = "2712" + key;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x00436268 File Offset: 0x00434468
		public static void Test()
		{
			byte[] array = new byte[] { 94, 254, 237, 227, 253, 205, 225, 189 };
			byte[] array2 = new byte[8];
			KiaBCM27Generator.calc(array, array2);
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x00002050 File Offset: 0x00000250
		public KiaBCM27Generator()
		{
		}

		// Token: 0x02000BB8 RID: 3000
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005AEE RID: 23278 RVA: 0x00436293 File Offset: 0x00434493
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005AEF RID: 23279 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005AF0 RID: 23280 RVA: 0x0043629F File Offset: 0x0043449F
			internal bool <Req2711_ResponseDecoded>b__5_0(OBDRequest x)
			{
				return x.Command == "2712";
			}

			// Token: 0x04003952 RID: 14674
			public static readonly KiaBCM27Generator.<>c <>9 = new KiaBCM27Generator.<>c();

			// Token: 0x04003953 RID: 14675
			public static Func<OBDRequest, bool> <>9__5_0;
		}
	}
}
