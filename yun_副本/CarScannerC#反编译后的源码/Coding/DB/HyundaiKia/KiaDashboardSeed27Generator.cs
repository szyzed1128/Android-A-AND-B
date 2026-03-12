using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BBD RID: 3005
	internal static class KiaDashboardSeed27Generator
	{
		// Token: 0x06005AFB RID: 23291 RVA: 0x004365DC File Offset: 0x004347DC
		public static void SetPasswordDecodeFor2701(OBDRequest req)
		{
			if (req.Command == "2701" && req.Header == "7C6")
			{
				req.ResponseMarker = "6701";
				req.ResponseDecoded += KiaDashboardSeed27Generator.Req2701_ResponseDecoded;
				req.DoNotDecode = false;
			}
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x00436634 File Offset: 0x00434834
		private static void Req2701_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			try
			{
				if (data != null && data.Length >= 2)
				{
					OBDRequest obdrequest = App.OBDReader.GetQueueCopy().FirstOrDefault((OBDRequest x) => x.Command == "2702");
					if (obdrequest != null)
					{
						bool flag = false;
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse<byte>(data);
						}
						int num = BitConverter.ToInt32(data, 0);
						if (num == 0)
						{
							flag = true;
						}
						if (!flag)
						{
							string text = (12 - num).ToString("X8");
							obdrequest.Command = "2702" + text;
						}
						else
						{
							obdrequest.Command = "3E";
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x02000BBE RID: 3006
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005AFD RID: 23293 RVA: 0x004366E8 File Offset: 0x004348E8
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005AFE RID: 23294 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005AFF RID: 23295 RVA: 0x004366F4 File Offset: 0x004348F4
			internal bool <Req2701_ResponseDecoded>b__1_0(OBDRequest x)
			{
				return x.Command == "2702";
			}

			// Token: 0x0400395D RID: 14685
			public static readonly KiaDashboardSeed27Generator.<>c <>9 = new KiaDashboardSeed27Generator.<>c();

			// Token: 0x0400395E RID: 14686
			public static Func<OBDRequest, bool> <>9__1_0;
		}
	}
}
