using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.PSA
{
	// Token: 0x02000A20 RID: 2592
	internal static class PSA_SeedKeyGenerator
	{
		// Token: 0x06005290 RID: 21136 RVA: 0x003F9768 File Offset: 0x003F7968
		public static string getKey(string seedTXT, string appKeyTXT)
		{
			string[] array = new string[]
			{
				seedTXT.Substring(0, 2),
				seedTXT.Substring(2, 2),
				seedTXT.Substring(4, 2),
				seedTXT.Substring(6, 2)
			};
			string[] array2 = new string[]
			{
				appKeyTXT.Substring(0, 2),
				appKeyTXT.Substring(2, 2)
			};
			long num = (long)int.Parse(array2[0] + array2[1], NumberStyles.HexNumber);
			long num2 = (long)(int.Parse(array2[1] + "00" + array2[0] + array2[1], NumberStyles.HexNumber) * 170);
			long num3;
			if (num > 32767L)
			{
				num3 = (long)((ulong)(-1206451487) * (ulong)(281474976645120L | num) >> 32);
				num3 = (long)((((ulong)(-65536) | (ulong)(num3 & 65535L)) >> 7) + (ulong)(-33554432));
			}
			else
			{
				num3 = (long)((ulong)(-1206451487) * (ulong)num >> 32 >> 7);
			}
			long num4 = ((num3 + (num3 >> 31)) & 65535L) * 30323L;
			long num5 = num2 - num4;
			if ((num5 & 65535L) > 32767L)
			{
				num5 += 30323L;
			}
			long num6 = num5 & 65535L;
			num = (long)int.Parse(array[0] + array[3], NumberStyles.HexNumber);
			long num7 = num * 171L;
			if (num > 32767L)
			{
				num3 = (long)((ulong)(-1189002245) * (ulong)(281474976645120L | num) >> 32);
				num3 = (long)((((ulong)(-65536) | (ulong)(num3 & 65535L)) >> 7) + (ulong)(-33554432));
			}
			else
			{
				num3 = (long)((ulong)(-1189002245) * (ulong)num >> 32 >> 7);
			}
			num4 = ((num3 + (num3 >> 31)) & 65535L) * 30269L;
			num5 = num7 - num4;
			if ((num5 & 65535L) > 32767L)
			{
				num5 += 30269L;
			}
			num5 &= 65535L;
			long num8 = num5 | num6;
			num = (long)int.Parse(array[1] + array[2], NumberStyles.HexNumber);
			long num9 = num * 170L;
			if (num > 32767L)
			{
				num3 = (long)((ulong)(-1206451487) * (ulong)(281474976645120L | num) >> 32);
				num3 = (long)((((ulong)(-65536) | (ulong)(num3 & 65535L)) >> 7) + (ulong)(-33554432));
			}
			else
			{
				num3 = (long)((ulong)(-1206451487) * (ulong)num >> 32 >> 7);
			}
			num4 = ((num3 + (num3 >> 31)) & 65535L) * 30323L;
			num5 = num9 - num4;
			if ((num5 & 65535L) > 32767L)
			{
				num5 += 30323L;
			}
			num5 &= 65535L;
			long num10 = num5;
			num = num8 & 65535L;
			long num11 = num * 171L;
			if (num > 32767L)
			{
				num3 = (long)((ulong)(-1189002245) * (ulong)(281474976645120L | num) >> 32);
				num3 = (long)((((ulong)(-65536) | (ulong)(num3 & 65535L)) >> 7) + (ulong)(-33554432));
			}
			else
			{
				num3 = (long)((ulong)(-1189002245) * (ulong)num >> 32 >> 7);
			}
			num4 = ((num3 + (num3 >> 31)) & 65535L) * 30269L;
			num5 = num11 - num4;
			if ((num5 & 65535L) > 32767L)
			{
				num5 += 30269L;
			}
			num5 &= 65535L;
			long num12 = num10 | num5;
			return num8.ToString("X4") + num12.ToString("X4");
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x003F9ABD File Offset: 0x003F7CBD
		public static void SetPasswordDecodeFor2703(OBDRequest req, string password)
		{
			PSA_SeedKeyGenerator._password = password;
			if (req.Command == "2703")
			{
				req.ResponseMarker = "6703";
				req.ResponseDecoded += PSA_SeedKeyGenerator.Req2703_ResponseDecoded;
				req.DoNotDecode = false;
			}
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x003F9AFC File Offset: 0x003F7CFC
		private static void Req2703_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			try
			{
				if (data != null && data.Length != 0)
				{
					OBDRequest obdrequest = App.OBDReader.GetQueueCopy().FirstOrDefault((OBDRequest x) => x.Command == "2704");
					if (obdrequest != null)
					{
						string text = BitHelpers.ByteArrayToHexString(data);
						try
						{
							string key = PSA_SeedKeyGenerator.getKey(text, PSA_SeedKeyGenerator._password);
							obdrequest.Command = "2704" + key;
						}
						catch (Exception)
						{
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x003F9B8C File Offset: 0x003F7D8C
		// Note: this type is marked as 'beforefieldinit'.
		static PSA_SeedKeyGenerator()
		{
		}

		// Token: 0x04003279 RID: 12921
		private static string _password = "";

		// Token: 0x02000A21 RID: 2593
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005294 RID: 21140 RVA: 0x003F9B98 File Offset: 0x003F7D98
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005295 RID: 21141 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005296 RID: 21142 RVA: 0x003F9BA4 File Offset: 0x003F7DA4
			internal bool <Req2703_ResponseDecoded>b__3_0(OBDRequest x)
			{
				return x.Command == "2704";
			}

			// Token: 0x0400327A RID: 12922
			public static readonly PSA_SeedKeyGenerator.<>c <>9 = new PSA_SeedKeyGenerator.<>c();

			// Token: 0x0400327B RID: 12923
			public static Func<OBDRequest, bool> <>9__3_0;
		}
	}
}
