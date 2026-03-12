using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Nissan
{
	// Token: 0x02000A70 RID: 2672
	internal class NissanLeafBatterySoHCountersReset
	{
		// Token: 0x06005480 RID: 21632 RVA: 0x00403178 File Offset: 0x00401378
		public static void SetPasswordDecodeFor2701(OBDRequest req)
		{
			if (req.Command == "2765" && req.Header == "79B")
			{
				req.ResponseMarker = "6765";
				req.ResponseDecoded += NissanLeafBatterySoHCountersReset.Req_ResponseDecoded;
				req.DoNotDecode = false;
			}
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x004031D0 File Offset: 0x004013D0
		private static void Req_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			try
			{
				if (data != null && data.Length >= 4)
				{
					OBDRequest obdrequest = App.OBDReader.GetQueueCopy().FirstOrDefault((OBDRequest x) => x.Command == "2766");
					if (obdrequest != null)
					{
						byte[] array = NissanLeafBatterySoHCountersReset.KeyCalculator(data);
						obdrequest.Command = "2766" + BitHelpers.ByteArrayToHexString(array);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005482 RID: 21634 RVA: 0x0040324C File Offset: 0x0040144C
		internal static byte[] KeyCalculator(byte[] seedBytes)
		{
			int num = (int)seedBytes[0] * 256 * 256 * 256 + (int)seedBytes[1] * 256 * 256 + (int)seedBytes[2] * 256 + (int)seedBytes[3];
			byte[] array = new byte[8];
			NissanLeafBatterySoHCountersReset.genkeytobuf(num, array, 33825);
			return array;
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x004032A0 File Offset: 0x004014A0
		private static int genkeytobuf(int seed, byte[] resp, int K)
		{
			int num = 1424571133;
			int num2 = NissanLeafBatterySoHCountersReset.funcgen(num >> 16, num & 65535, seed >> 16, K);
			int num3 = NissanLeafBatterySoHCountersReset.funcgen(seed & 65535, seed >> 16, num >> 16, K);
			resp[0] = (byte)(num2 & 255);
			resp[1] = (byte)(num3 & 255);
			resp[2] = (byte)((num3 >> 8) & 255);
			resp[3] = (byte)((num2 >> 8) & 255);
			resp[4] = (byte)((num3 >> 16) & 255);
			resp[5] = (byte)((num2 >> 16) & 255);
			resp[6] = (byte)((num3 >> 24) & 255);
			resp[7] = (byte)((num2 >> 24) & 255);
			return 0;
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x00403348 File Offset: 0x00401548
		private static int funcgen(int b1, int b2, int b3, int k)
		{
			int num = NissanLeafBatterySoHCountersReset.stage1(b2, b3);
			int num2 = NissanLeafBatterySoHCountersReset.stage2(b2, b3);
			num = NissanLeafBatterySoHCountersReset.stage3(b1, num, num2);
			num2 = NissanLeafBatterySoHCountersReset.stage3(b1, num2, num);
			int num3 = NissanLeafBatterySoHCountersReset.stage4(num, k);
			return NissanLeafBatterySoHCountersReset.stage4(num2, k) | (num3 << 16);
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x0040338C File Offset: 0x0040158C
		private static int stage1(int b1, int b2)
		{
			int num = (6 | b1) & b2 & 15;
			int num2 = ((b1 >> num) | (b1 << 16 - num)) & 65535;
			int num3 = ((b2 << num) | (b2 >> 16 - num)) & 65535;
			return (num2 * num3) & 65535;
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x004033D8 File Offset: 0x004015D8
		private static int stage2(int b1, int b2)
		{
			int num = (b1 * 6 + b2) & 255;
			return ((b1 + num) * (b2 + num)) & 65535;
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x00403400 File Offset: 0x00401600
		private static int stage3(int b1, int b2, int b3)
		{
			int num = ((b2 ^ 36839) | (b3 ^ 32648)) & 65535;
			int num2 = (b1 ^ (b1 >> 8)) & 255;
			return (num * num2) & 65535;
		}

		// Token: 0x06005488 RID: 21640 RVA: 0x00403438 File Offset: 0x00401638
		private static int stage4(int b1, int b2)
		{
			int num = 65535;
			for (int i = 0; i < 16; i++)
			{
				int num2 = b1 & 1 & 65535;
				b1 >>= 1;
				num = (num << 1) & 65535;
				if (num2 == 1)
				{
					num ^= b1;
				}
				else
				{
					num ^= b2;
				}
			}
			return num & 65535;
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x00002050 File Offset: 0x00000250
		public NissanLeafBatterySoHCountersReset()
		{
		}

		// Token: 0x0400339C RID: 13212
		private const int X2766 = 33825;

		// Token: 0x0400339D RID: 13213
		private const int MAGIC_N = 6;

		// Token: 0x02000A71 RID: 2673
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600548A RID: 21642 RVA: 0x00403484 File Offset: 0x00401684
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600548B RID: 21643 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600548C RID: 21644 RVA: 0x00403490 File Offset: 0x00401690
			internal bool <Req_ResponseDecoded>b__1_0(OBDRequest x)
			{
				return x.Command == "2766";
			}

			// Token: 0x0400339E RID: 13214
			public static readonly NissanLeafBatterySoHCountersReset.<>c <>9 = new NissanLeafBatterySoHCountersReset.<>c();

			// Token: 0x0400339F RID: 13215
			public static Func<OBDRequest, bool> <>9__1_0;
		}
	}
}
