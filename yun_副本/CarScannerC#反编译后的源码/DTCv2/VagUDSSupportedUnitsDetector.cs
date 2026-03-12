using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.ECUModels;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.VWTP20;

namespace CarScannerXamarinForms.DTCv2
{
	// Token: 0x020005B2 RID: 1458
	internal class VagUDSSupportedUnitsDetector : ISupportedUnitsDetector
	{
		// Token: 0x060034D2 RID: 13522 RVA: 0x002601A4 File Offset: 0x0025E3A4
		public async Task<List<IECU>> DetectSupported(IEnumerable<IECU> allUnits)
		{
			TaskAwaiter<bool> taskAwaiter = this.CheckForVWTPGate().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			List<IECU> list;
			if (taskAwaiter.GetResult())
			{
				list = new List<IECU>(0);
			}
			else
			{
				List<IECU> result = new List<IECU>();
				result.AddRange(allUnits.Where((IECU x) => x is OBD2Can11bitECU || x is OBD2Can29bitECU));
				List<IECU> list2 = await this.DetectSupportedFrom222A2A(allUnits);
				result.AddRange(list2.Distinct<IECU>());
				if (list2.Count > 0)
				{
					foreach (IECU iecu in list2)
					{
						IVagECU vagECU = iecu as IVagECU;
						if (vagECU != null)
						{
							vagECU.IsDetectedAsExisting = true;
						}
					}
					list = list2;
				}
				else
				{
					List<IECU> list3 = await this.DetectSupportedMQB(allUnits);
					result.AddRange(list3.Distinct<IECU>());
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
					{
						result = (from x in result
							orderby x is OBD2Can11bitECU || x is OBD2Can29bitECU descending, x is VAG29bitECU descending, x.Name
							select x).ToList<IECU>();
					}
					else
					{
						result = (from x in result
							orderby x is OBD2Can11bitECU || x is OBD2Can29bitECU descending, x is VAGECU descending, x.Name
							select x).ToList<IECU>();
					}
					foreach (IECU iecu2 in result)
					{
						IVagECU vagECU2 = iecu2 as IVagECU;
						if (vagECU2 != null)
						{
							vagECU2.IsDetectedAsExisting = true;
						}
					}
					list = result;
				}
			}
			return list;
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x002601F0 File Offset: 0x0025E3F0
		public async Task<List<IECU>> DetectSupportedFrom222A2A(IEnumerable<IECU> allUnits)
		{
			List<IECU> result = new List<IECU>();
			OBDRequest obdrequest = new OBDRequest("222A2A", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
			obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					for (int i = 0; i < data.Length; i++)
					{
						byte b = data[i];
						string ident = b.ToString("X2") + ".";
						IEnumerable<IECU> enumerable = allUnits.Where((IECU x) => x.Name.StartsWith(ident, StringComparison.OrdinalIgnoreCase));
						if (enumerable != null)
						{
							result.AddRange(enumerable);
						}
					}
				}
			};
			App.OBDReader.ReplaceQueue(obdrequest);
			await App.OBDReader.WaitForCommandQueue();
			await Task.Delay(200);
			result = (from x in result.Distinct<IECU>()
				orderby x is VAGECU descending, x.Name
				select x).ToList<IECU>();
			return result;
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x00260234 File Offset: 0x0025E434
		public async Task<List<IECU>> DetectSupportedMQB(IEnumerable<IECU> allUnits)
		{
			List<IECU> result = new List<IECU>();
			byte[] installedUnitsRawData = null;
			byte[] codedUnitsRawData = null;
			byte[] eventsUnitsRawData = null;
			byte[] mebUnitsRawData = null;
			OBDRequest obdrequest = new OBDRequest("222A26", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
			obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					installedUnitsRawData = data;
					IECU iecu3 = allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
					if (iecu3 != null)
					{
						result.Add(iecu3);
					}
				}
			};
			OBDRequest obdrequest2 = new OBDRequest("2204A3", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
			obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					codedUnitsRawData = data;
					IECU iecu4 = allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
					if (iecu4 != null)
					{
						result.Add(iecu4);
					}
				}
			};
			OBDRequest obdrequest3 = new OBDRequest("222A28", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
			obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					eventsUnitsRawData = data;
					IECU iecu5 = allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
					if (iecu5 != null)
					{
						result.Add(iecu5);
					}
				}
			};
			OBDRequest obdrequest4 = new OBDRequest("22F1B7", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
			obdrequest4.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					mebUnitsRawData = data;
					IECU iecu6 = allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
					if (iecu6 != null)
					{
						result.Add(iecu6);
					}
				}
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3, obdrequest4 });
			await App.OBDReader.WaitForCommandQueue();
			await Task.Delay(200);
			List<IECU> list;
			if (installedUnitsRawData == null && codedUnitsRawData == null && mebUnitsRawData == null)
			{
				list = result;
			}
			else
			{
				if (mebUnitsRawData != null)
				{
					using (MemoryStream memoryStream = new MemoryStream(mebUnitsRawData))
					{
						while (memoryStream.Position < memoryStream.Length)
						{
							int num = memoryStream.ReadByte() * 256 + memoryStream.ReadByte();
							string address11bit2 = (1792 + num).ToString("X3");
							string address29bit2 = (16515072 + num).ToString("X6");
							IEnumerable<IECU> enumerable = allUnits.Where((IECU x) => x.RequestHeader == address11bit2 || x.RequestHeader == address29bit2);
							if (enumerable != null)
							{
								foreach (IECU iecu in enumerable)
								{
									if (!result.Contains(iecu))
									{
										result.Add(iecu);
									}
								}
							}
						}
					}
				}
				foreach (byte[] array in new List<byte[]>(3) { installedUnitsRawData, codedUnitsRawData, eventsUnitsRawData })
				{
					if (array != null)
					{
						List<bool> list2 = new List<bool>();
						foreach (byte b in array)
						{
							for (int j = 0; j < 8; j++)
							{
								list2.Add(BitHelpers.GetBit_0_7(b, j));
							}
						}
						for (int k = 0; k < list2.Count; k++)
						{
							if (list2[k])
							{
								string address11bit = (1792 + k).ToString("X3");
								string address29bit = (16515072 + k).ToString("X6");
								if (k == 118)
								{
									address11bit = "7E0";
								}
								if (k == 119)
								{
									address11bit = "7E1";
								}
								if (k == 0)
								{
									address11bit = "710";
								}
								IEnumerable<IECU> enumerable2 = allUnits.Where((IECU x) => x.RequestHeader == address11bit || x.RequestHeader == address29bit);
								if (enumerable2 != null)
								{
									foreach (IECU iecu2 in enumerable2)
									{
										if (!result.Contains(iecu2))
										{
											result.Add(iecu2);
										}
										if (array == eventsUnitsRawData)
										{
											iecu2.Highlighted = true;
										}
									}
								}
							}
						}
					}
				}
				list = result;
			}
			return list;
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x00260278 File Offset: 0x0025E478
		private async Task<bool> CheckForVWTPGate()
		{
			bool result = false;
			OBDRequest obdrequest = VWTPManager.BuildRequest("1089", "19", false);
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if ((data != null && data.Contains("5089")) || data.Contains("7F10"))
				{
					result = true;
				}
			};
			App.OBDReader.ReplaceQueue(obdrequest);
			await App.OBDReader.WaitForCommandQueue();
			await Task.Delay(100);
			return result;
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x00002050 File Offset: 0x00000250
		public VagUDSSupportedUnitsDetector()
		{
		}

		// Token: 0x020005B3 RID: 1459
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060034D7 RID: 13527 RVA: 0x002602B3 File Offset: 0x0025E4B3
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060034D8 RID: 13528 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060034D9 RID: 13529 RVA: 0x002602BF File Offset: 0x0025E4BF
			internal bool <DetectSupported>b__0_0(IECU x)
			{
				return x is OBD2Can11bitECU || x is OBD2Can29bitECU;
			}

			// Token: 0x060034DA RID: 13530 RVA: 0x002602BF File Offset: 0x0025E4BF
			internal bool <DetectSupported>b__0_1(IECU x)
			{
				return x is OBD2Can11bitECU || x is OBD2Can29bitECU;
			}

			// Token: 0x060034DB RID: 13531 RVA: 0x002602D4 File Offset: 0x0025E4D4
			internal bool <DetectSupported>b__0_2(IECU x)
			{
				return x is VAG29bitECU;
			}

			// Token: 0x060034DC RID: 13532 RVA: 0x002602DF File Offset: 0x0025E4DF
			internal string <DetectSupported>b__0_3(IECU x)
			{
				return x.Name;
			}

			// Token: 0x060034DD RID: 13533 RVA: 0x002602BF File Offset: 0x0025E4BF
			internal bool <DetectSupported>b__0_4(IECU x)
			{
				return x is OBD2Can11bitECU || x is OBD2Can29bitECU;
			}

			// Token: 0x060034DE RID: 13534 RVA: 0x002602E7 File Offset: 0x0025E4E7
			internal bool <DetectSupported>b__0_5(IECU x)
			{
				return x is VAGECU;
			}

			// Token: 0x060034DF RID: 13535 RVA: 0x002602DF File Offset: 0x0025E4DF
			internal string <DetectSupported>b__0_6(IECU x)
			{
				return x.Name;
			}

			// Token: 0x060034E0 RID: 13536 RVA: 0x002602E7 File Offset: 0x0025E4E7
			internal bool <DetectSupportedFrom222A2A>b__1_1(IECU x)
			{
				return x is VAGECU;
			}

			// Token: 0x060034E1 RID: 13537 RVA: 0x002602DF File Offset: 0x0025E4DF
			internal string <DetectSupportedFrom222A2A>b__1_2(IECU x)
			{
				return x.Name;
			}

			// Token: 0x060034E2 RID: 13538 RVA: 0x002602F2 File Offset: 0x0025E4F2
			internal bool <DetectSupportedMQB>b__2_4(IECU x)
			{
				return x.RequestHeader == "710";
			}

			// Token: 0x060034E3 RID: 13539 RVA: 0x002602F2 File Offset: 0x0025E4F2
			internal bool <DetectSupportedMQB>b__2_5(IECU x)
			{
				return x.RequestHeader == "710";
			}

			// Token: 0x060034E4 RID: 13540 RVA: 0x002602F2 File Offset: 0x0025E4F2
			internal bool <DetectSupportedMQB>b__2_6(IECU x)
			{
				return x.RequestHeader == "710";
			}

			// Token: 0x060034E5 RID: 13541 RVA: 0x002602F2 File Offset: 0x0025E4F2
			internal bool <DetectSupportedMQB>b__2_7(IECU x)
			{
				return x.RequestHeader == "710";
			}

			// Token: 0x04001F61 RID: 8033
			public static readonly VagUDSSupportedUnitsDetector.<>c <>9 = new VagUDSSupportedUnitsDetector.<>c();

			// Token: 0x04001F62 RID: 8034
			public static Func<IECU, bool> <>9__0_0;

			// Token: 0x04001F63 RID: 8035
			public static Func<IECU, bool> <>9__0_1;

			// Token: 0x04001F64 RID: 8036
			public static Func<IECU, bool> <>9__0_2;

			// Token: 0x04001F65 RID: 8037
			public static Func<IECU, string> <>9__0_3;

			// Token: 0x04001F66 RID: 8038
			public static Func<IECU, bool> <>9__0_4;

			// Token: 0x04001F67 RID: 8039
			public static Func<IECU, bool> <>9__0_5;

			// Token: 0x04001F68 RID: 8040
			public static Func<IECU, string> <>9__0_6;

			// Token: 0x04001F69 RID: 8041
			public static Func<IECU, bool> <>9__1_1;

			// Token: 0x04001F6A RID: 8042
			public static Func<IECU, string> <>9__1_2;

			// Token: 0x04001F6B RID: 8043
			public static Func<IECU, bool> <>9__2_4;

			// Token: 0x04001F6C RID: 8044
			public static Func<IECU, bool> <>9__2_5;

			// Token: 0x04001F6D RID: 8045
			public static Func<IECU, bool> <>9__2_6;

			// Token: 0x04001F6E RID: 8046
			public static Func<IECU, bool> <>9__2_7;
		}

		// Token: 0x020005B4 RID: 1460
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x060034E6 RID: 13542 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x060034E7 RID: 13543 RVA: 0x00260304 File Offset: 0x0025E504
			internal void <DetectSupportedFrom222A2A>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					foreach (byte b in data)
					{
						VagUDSSupportedUnitsDetector.<>c__DisplayClass1_1 CS$<>8__locals1 = new VagUDSSupportedUnitsDetector.<>c__DisplayClass1_1();
						CS$<>8__locals1.ident = b.ToString("X2") + ".";
						IEnumerable<IECU> enumerable = this.allUnits.Where((IECU x) => x.Name.StartsWith(CS$<>8__locals1.ident, StringComparison.OrdinalIgnoreCase));
						if (enumerable != null)
						{
							this.result.AddRange(enumerable);
						}
					}
				}
			}

			// Token: 0x04001F6F RID: 8047
			public IEnumerable<IECU> allUnits;

			// Token: 0x04001F70 RID: 8048
			public List<IECU> result;
		}

		// Token: 0x020005B5 RID: 1461
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_1
		{
			// Token: 0x060034E8 RID: 13544 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_1()
			{
			}

			// Token: 0x060034E9 RID: 13545 RVA: 0x00260378 File Offset: 0x0025E578
			internal bool <DetectSupportedFrom222A2A>b__3(IECU x)
			{
				return x.Name.StartsWith(this.ident, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x04001F71 RID: 8049
			public string ident;
		}

		// Token: 0x020005B6 RID: 1462
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060034EA RID: 13546 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060034EB RID: 13547 RVA: 0x0026038C File Offset: 0x0025E58C
			internal void <DetectSupportedMQB>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.installedUnitsRawData = data;
					IECU iecu = this.allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
					if (iecu != null)
					{
						this.result.Add(iecu);
					}
				}
			}

			// Token: 0x060034EC RID: 13548 RVA: 0x002603E4 File Offset: 0x0025E5E4
			internal void <DetectSupportedMQB>b__1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.codedUnitsRawData = data;
					IECU iecu = this.allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
					if (iecu != null)
					{
						this.result.Add(iecu);
					}
				}
			}

			// Token: 0x060034ED RID: 13549 RVA: 0x0026043C File Offset: 0x0025E63C
			internal void <DetectSupportedMQB>b__2(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.eventsUnitsRawData = data;
					IECU iecu = this.allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
					if (iecu != null)
					{
						this.result.Add(iecu);
					}
				}
			}

			// Token: 0x060034EE RID: 13550 RVA: 0x00260494 File Offset: 0x0025E694
			internal void <DetectSupportedMQB>b__3(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.mebUnitsRawData = data;
					IECU iecu = this.allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
					if (iecu != null)
					{
						this.result.Add(iecu);
					}
				}
			}

			// Token: 0x04001F72 RID: 8050
			public byte[] installedUnitsRawData;

			// Token: 0x04001F73 RID: 8051
			public IEnumerable<IECU> allUnits;

			// Token: 0x04001F74 RID: 8052
			public List<IECU> result;

			// Token: 0x04001F75 RID: 8053
			public byte[] codedUnitsRawData;

			// Token: 0x04001F76 RID: 8054
			public byte[] eventsUnitsRawData;

			// Token: 0x04001F77 RID: 8055
			public byte[] mebUnitsRawData;
		}

		// Token: 0x020005B7 RID: 1463
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_1
		{
			// Token: 0x060034EF RID: 13551 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_1()
			{
			}

			// Token: 0x060034F0 RID: 13552 RVA: 0x002604E9 File Offset: 0x0025E6E9
			internal bool <DetectSupportedMQB>b__8(IECU x)
			{
				return x.RequestHeader == this.address11bit || x.RequestHeader == this.address29bit;
			}

			// Token: 0x04001F78 RID: 8056
			public string address11bit;

			// Token: 0x04001F79 RID: 8057
			public string address29bit;
		}

		// Token: 0x020005B8 RID: 1464
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_2
		{
			// Token: 0x060034F1 RID: 13553 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_2()
			{
			}

			// Token: 0x060034F2 RID: 13554 RVA: 0x00260511 File Offset: 0x0025E711
			internal bool <DetectSupportedMQB>b__9(IECU x)
			{
				return x.RequestHeader == this.address11bit || x.RequestHeader == this.address29bit;
			}

			// Token: 0x04001F7A RID: 8058
			public string address11bit;

			// Token: 0x04001F7B RID: 8059
			public string address29bit;
		}

		// Token: 0x020005B9 RID: 1465
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060034F3 RID: 13555 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060034F4 RID: 13556 RVA: 0x00260539 File Offset: 0x0025E739
			internal void <CheckForVWTPGate>b__0(OBDRequest request, string data)
			{
				if ((data != null && data.Contains("5089")) || data.Contains("7F10"))
				{
					this.result = true;
				}
			}

			// Token: 0x04001F7C RID: 8060
			public bool result;
		}

		// Token: 0x020005BA RID: 1466
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckForVWTPGate>d__3 : IAsyncStateMachine
		{
			// Token: 0x060034F5 RID: 13557 RVA: 0x00260560 File Offset: 0x0025E760
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool result;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_010F;
						}
						CS$<>8__locals1 = new VagUDSSupportedUnitsDetector.<>c__DisplayClass3_0();
						CS$<>8__locals1.result = false;
						OBDRequest obdrequest = VWTPManager.BuildRequest("1089", "19", false);
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if ((data != null && data.Contains("5089")) || data.Contains("7F10"))
							{
								CS$<>8__locals1.result = true;
							}
						};
						App.OBDReader.ReplaceQueue(obdrequest);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagUDSSupportedUnitsDetector.<CheckForVWTPGate>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					taskAwaiter = Task.Delay(100).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagUDSSupportedUnitsDetector.<CheckForVWTPGate>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_010F:
					taskAwaiter.GetResult();
					result = CS$<>8__locals1.result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x060034F6 RID: 13558 RVA: 0x002606E8 File Offset: 0x0025E8E8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F7D RID: 8061
			public int <>1__state;

			// Token: 0x04001F7E RID: 8062
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001F7F RID: 8063
			private VagUDSSupportedUnitsDetector.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04001F80 RID: 8064
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005BB RID: 1467
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DetectSupported>d__0 : IAsyncStateMachine
		{
			// Token: 0x060034F7 RID: 13559 RVA: 0x002606F8 File Offset: 0x0025E8F8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VagUDSSupportedUnitsDetector vagUDSSupportedUnitsDetector = this;
				List<IECU> list;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter<List<IECU>> taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						break;
					case 1:
					{
						TaskAwaiter<List<IECU>> taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter<List<IECU>>);
						num = (num2 = -1);
						goto IL_0129;
					}
					case 2:
					{
						TaskAwaiter<List<IECU>> taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter<List<IECU>>);
						num = (num2 = -1);
						goto IL_01F0;
					}
					default:
						taskAwaiter3 = vagUDSSupportedUnitsDetector.CheckForVWTPGate().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VagUDSSupportedUnitsDetector.<DetectSupported>d__0>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (taskAwaiter3.GetResult())
					{
						list = new List<IECU>(0);
						goto IL_0383;
					}
					result = new List<IECU>();
					result.AddRange(allUnits.Where((IECU x) => x is OBD2Can11bitECU || x is OBD2Can29bitECU));
					taskAwaiter4 = vagUDSSupportedUnitsDetector.DetectSupportedFrom222A2A(allUnits).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<List<IECU>> taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<IECU>>, VagUDSSupportedUnitsDetector.<DetectSupported>d__0>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0129:
					List<IECU> result2 = taskAwaiter4.GetResult();
					result.AddRange(result2.Distinct<IECU>());
					List<IECU>.Enumerator enumerator;
					if (result2.Count > 0)
					{
						enumerator = result2.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								IECU iecu = enumerator.Current;
								IVagECU vagECU = iecu as IVagECU;
								if (vagECU != null)
								{
									vagECU.IsDetectedAsExisting = true;
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						list = result2;
						goto IL_0383;
					}
					taskAwaiter4 = vagUDSSupportedUnitsDetector.DetectSupportedMQB(allUnits).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter<List<IECU>> taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<IECU>>, VagUDSSupportedUnitsDetector.<DetectSupported>d__0>(ref taskAwaiter4, ref this);
						return;
					}
					IL_01F0:
					List<IECU> result3 = taskAwaiter4.GetResult();
					result.AddRange(result3.Distinct<IECU>());
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
					{
						result = (from x in result
							orderby x is OBD2Can11bitECU || x is OBD2Can29bitECU descending, x is VAG29bitECU descending, x.Name
							select x).ToList<IECU>();
					}
					else
					{
						result = (from x in result
							orderby x is OBD2Can11bitECU || x is OBD2Can29bitECU descending, x is VAGECU descending, x.Name
							select x).ToList<IECU>();
					}
					enumerator = result.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IECU iecu2 = enumerator.Current;
							IVagECU vagECU2 = iecu2 as IVagECU;
							if (vagECU2 != null)
							{
								vagECU2.IsDetectedAsExisting = true;
							}
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					list = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					result = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0383:
				num2 = -2;
				result = null;
				this.<>t__builder.SetResult(list);
			}

			// Token: 0x060034F8 RID: 13560 RVA: 0x00260AF0 File Offset: 0x0025ECF0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F81 RID: 8065
			public int <>1__state;

			// Token: 0x04001F82 RID: 8066
			public AsyncTaskMethodBuilder<List<IECU>> <>t__builder;

			// Token: 0x04001F83 RID: 8067
			public VagUDSSupportedUnitsDetector <>4__this;

			// Token: 0x04001F84 RID: 8068
			public IEnumerable<IECU> allUnits;

			// Token: 0x04001F85 RID: 8069
			private List<IECU> <result>5__2;

			// Token: 0x04001F86 RID: 8070
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001F87 RID: 8071
			private TaskAwaiter<List<IECU>> <>u__2;
		}

		// Token: 0x020005BC RID: 1468
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DetectSupportedFrom222A2A>d__1 : IAsyncStateMachine
		{
			// Token: 0x060034F9 RID: 13561 RVA: 0x00260B00 File Offset: 0x0025ED00
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				List<IECU> result;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0134;
						}
						CS$<>8__locals1 = new VagUDSSupportedUnitsDetector.<>c__DisplayClass1_0();
						CS$<>8__locals1.allUnits = allUnits;
						CS$<>8__locals1.result = new List<IECU>();
						OBDRequest obdrequest = new OBDRequest("222A2A", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
						obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								foreach (byte b in data)
								{
									VagUDSSupportedUnitsDetector.<>c__DisplayClass1_1 CS$<>8__locals1 = new VagUDSSupportedUnitsDetector.<>c__DisplayClass1_1();
									CS$<>8__locals1.ident = b.ToString("X2") + ".";
									IEnumerable<IECU> enumerable = CS$<>8__locals1.allUnits.Where((IECU x) => x.Name.StartsWith(CS$<>8__locals1.ident, StringComparison.OrdinalIgnoreCase));
									if (enumerable != null)
									{
										CS$<>8__locals1.result.AddRange(enumerable);
									}
								}
							}
						};
						App.OBDReader.ReplaceQueue(obdrequest);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagUDSSupportedUnitsDetector.<DetectSupportedFrom222A2A>d__1>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					taskAwaiter = Task.Delay(200).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagUDSSupportedUnitsDetector.<DetectSupportedFrom222A2A>d__1>(ref taskAwaiter, ref this);
						return;
					}
					IL_0134:
					taskAwaiter.GetResult();
					CS$<>8__locals1.result = (from x in CS$<>8__locals1.result.Distinct<IECU>()
						orderby x is VAGECU descending, x.Name
						select x).ToList<IECU>();
					result = CS$<>8__locals1.result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x060034FA RID: 13562 RVA: 0x00260D18 File Offset: 0x0025EF18
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F88 RID: 8072
			public int <>1__state;

			// Token: 0x04001F89 RID: 8073
			public AsyncTaskMethodBuilder<List<IECU>> <>t__builder;

			// Token: 0x04001F8A RID: 8074
			public IEnumerable<IECU> allUnits;

			// Token: 0x04001F8B RID: 8075
			private VagUDSSupportedUnitsDetector.<>c__DisplayClass1_0 <>8__1;

			// Token: 0x04001F8C RID: 8076
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005BD RID: 1469
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DetectSupportedMQB>d__2 : IAsyncStateMachine
		{
			// Token: 0x060034FB RID: 13563 RVA: 0x00260D28 File Offset: 0x0025EF28
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				List<IECU> list;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_021B;
						}
						CS$<>8__locals1 = new VagUDSSupportedUnitsDetector.<>c__DisplayClass2_0();
						CS$<>8__locals1.allUnits = allUnits;
						CS$<>8__locals1.result = new List<IECU>();
						CS$<>8__locals1.installedUnitsRawData = null;
						CS$<>8__locals1.codedUnitsRawData = null;
						CS$<>8__locals1.eventsUnitsRawData = null;
						CS$<>8__locals1.mebUnitsRawData = null;
						OBDRequest obdrequest = new OBDRequest("222A26", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
						obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								CS$<>8__locals1.installedUnitsRawData = data;
								IECU iecu3 = CS$<>8__locals1.allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
								if (iecu3 != null)
								{
									CS$<>8__locals1.result.Add(iecu3);
								}
							}
						};
						OBDRequest obdrequest2 = new OBDRequest("2204A3", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
						obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								CS$<>8__locals1.codedUnitsRawData = data;
								IECU iecu4 = CS$<>8__locals1.allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
								if (iecu4 != null)
								{
									CS$<>8__locals1.result.Add(iecu4);
								}
							}
						};
						OBDRequest obdrequest3 = new OBDRequest("222A28", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
						obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								CS$<>8__locals1.eventsUnitsRawData = data;
								IECU iecu5 = CS$<>8__locals1.allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
								if (iecu5 != null)
								{
									CS$<>8__locals1.result.Add(iecu5);
								}
							}
						};
						OBDRequest obdrequest4 = new OBDRequest("22F1B7", "710", "ATSP6;ATFCSH710;ATFCSD300000;ATFCSM1;ATCRA77A", "ATAR;ATFCSM0;ATSPDEF", false);
						obdrequest4.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								CS$<>8__locals1.mebUnitsRawData = data;
								IECU iecu6 = CS$<>8__locals1.allUnits.FirstOrDefault((IECU x) => x.RequestHeader == "710");
								if (iecu6 != null)
								{
									CS$<>8__locals1.result.Add(iecu6);
								}
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3, obdrequest4 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagUDSSupportedUnitsDetector.<DetectSupportedMQB>d__2>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					taskAwaiter = Task.Delay(200).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagUDSSupportedUnitsDetector.<DetectSupportedMQB>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_021B:
					taskAwaiter.GetResult();
					if (CS$<>8__locals1.installedUnitsRawData == null && CS$<>8__locals1.codedUnitsRawData == null && CS$<>8__locals1.mebUnitsRawData == null)
					{
						list = CS$<>8__locals1.result;
					}
					else
					{
						if (CS$<>8__locals1.mebUnitsRawData != null)
						{
							MemoryStream memoryStream = new MemoryStream(CS$<>8__locals1.mebUnitsRawData);
							try
							{
								while (memoryStream.Position < memoryStream.Length)
								{
									VagUDSSupportedUnitsDetector.<>c__DisplayClass2_1 CS$<>8__locals2 = new VagUDSSupportedUnitsDetector.<>c__DisplayClass2_1();
									int num3 = memoryStream.ReadByte() * 256 + memoryStream.ReadByte();
									CS$<>8__locals2.address11bit = (1792 + num3).ToString("X3");
									CS$<>8__locals2.address29bit = (16515072 + num3).ToString("X6");
									IEnumerable<IECU> enumerable = CS$<>8__locals1.allUnits.Where((IECU x) => x.RequestHeader == CS$<>8__locals2.address11bit || x.RequestHeader == CS$<>8__locals2.address29bit);
									if (enumerable != null)
									{
										IEnumerator<IECU> enumerator = enumerable.GetEnumerator();
										try
										{
											while (enumerator.MoveNext())
											{
												IECU iecu = enumerator.Current;
												if (!CS$<>8__locals1.result.Contains(iecu))
												{
													CS$<>8__locals1.result.Add(iecu);
												}
											}
										}
										finally
										{
											if (num < 0 && enumerator != null)
											{
												enumerator.Dispose();
											}
										}
									}
								}
							}
							finally
							{
								if (num < 0 && memoryStream != null)
								{
									((IDisposable)memoryStream).Dispose();
								}
							}
						}
						List<byte[]>.Enumerator enumerator2 = new List<byte[]>(3) { CS$<>8__locals1.installedUnitsRawData, CS$<>8__locals1.codedUnitsRawData, CS$<>8__locals1.eventsUnitsRawData }.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								byte[] array = enumerator2.Current;
								if (array != null)
								{
									List<bool> list2 = new List<bool>();
									foreach (byte b in array)
									{
										for (int j = 0; j < 8; j++)
										{
											list2.Add(BitHelpers.GetBit_0_7(b, j));
										}
									}
									for (int k = 0; k < list2.Count; k++)
									{
										if (list2[k])
										{
											VagUDSSupportedUnitsDetector.<>c__DisplayClass2_2 CS$<>8__locals3 = new VagUDSSupportedUnitsDetector.<>c__DisplayClass2_2();
											CS$<>8__locals3.address11bit = (1792 + k).ToString("X3");
											CS$<>8__locals3.address29bit = (16515072 + k).ToString("X6");
											if (k == 118)
											{
												CS$<>8__locals3.address11bit = "7E0";
											}
											if (k == 119)
											{
												CS$<>8__locals3.address11bit = "7E1";
											}
											if (k == 0)
											{
												CS$<>8__locals3.address11bit = "710";
											}
											IEnumerable<IECU> enumerable2 = CS$<>8__locals1.allUnits.Where((IECU x) => x.RequestHeader == CS$<>8__locals3.address11bit || x.RequestHeader == CS$<>8__locals3.address29bit);
											if (enumerable2 != null)
											{
												IEnumerator<IECU> enumerator = enumerable2.GetEnumerator();
												try
												{
													while (enumerator.MoveNext())
													{
														IECU iecu2 = enumerator.Current;
														if (!CS$<>8__locals1.result.Contains(iecu2))
														{
															CS$<>8__locals1.result.Add(iecu2);
														}
														if (array == CS$<>8__locals1.eventsUnitsRawData)
														{
															iecu2.Highlighted = true;
														}
													}
												}
												finally
												{
													if (num < 0 && enumerator != null)
													{
														enumerator.Dispose();
													}
												}
											}
										}
									}
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						list = CS$<>8__locals1.result;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(list);
			}

			// Token: 0x060034FC RID: 13564 RVA: 0x00261354 File Offset: 0x0025F554
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F8D RID: 8077
			public int <>1__state;

			// Token: 0x04001F8E RID: 8078
			public AsyncTaskMethodBuilder<List<IECU>> <>t__builder;

			// Token: 0x04001F8F RID: 8079
			public IEnumerable<IECU> allUnits;

			// Token: 0x04001F90 RID: 8080
			private VagUDSSupportedUnitsDetector.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x04001F91 RID: 8081
			private TaskAwaiter <>u__1;
		}
	}
}
