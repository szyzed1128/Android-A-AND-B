using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Haval
{
	// Token: 0x02000BD5 RID: 3029
	internal class DashboardPasswordGenerator
	{
		// Token: 0x06005B3B RID: 23355 RVA: 0x00437D14 File Offset: 0x00435F14
		public static void SetPasswordDecodeFor2701(OBDRequest req)
		{
			if (req.Command == "2701" && (req.Header == "766" || req.Header == "7E0" || req.Header == "765"))
			{
				req.ResponseMarker = "6701";
				req.ResponseDecoded += DashboardPasswordGenerator.Req_ResponseDecoded;
				req.DoNotDecode = false;
			}
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x00437D90 File Offset: 0x00435F90
		private static void Req_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			try
			{
				if (data != null && data.Length >= 2)
				{
					OBDRequest obdrequest = App.OBDReader.GetQueueCopy().FirstOrDefault((OBDRequest x) => x.Command == "2702");
					if (obdrequest != null)
					{
						obdrequest.Command = "2702" + data[1].ToString("X2") + data[0].ToString("X2");
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x00002050 File Offset: 0x00000250
		public DashboardPasswordGenerator()
		{
		}

		// Token: 0x02000BD6 RID: 3030
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005B3E RID: 23358 RVA: 0x00437E20 File Offset: 0x00436020
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005B3F RID: 23359 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005B40 RID: 23360 RVA: 0x004366F4 File Offset: 0x004348F4
			internal bool <Req_ResponseDecoded>b__1_0(OBDRequest x)
			{
				return x.Command == "2702";
			}

			// Token: 0x0400398A RID: 14730
			public static readonly DashboardPasswordGenerator.<>c <>9 = new DashboardPasswordGenerator.<>c();

			// Token: 0x0400398B RID: 14731
			public static Func<OBDRequest, bool> <>9__1_0;
		}
	}
}
