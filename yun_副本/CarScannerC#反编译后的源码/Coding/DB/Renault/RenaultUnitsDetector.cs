using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A0B RID: 2571
	internal class RenaultUnitsDetector
	{
		// Token: 0x06005213 RID: 21011 RVA: 0x003F6AAE File Offset: 0x003F4CAE
		public RenaultUnitsDetector()
		{
			this.LoadECUCollection();
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x003F6ABC File Offset: 0x003F4CBC
		private void LoadECUCollection()
		{
			this.ECUs = PackageFileReader.DeserilzeFromEmbeddedFileUsingStream<List<RenaultCodingECU>>("Renault.renault.db");
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x003F6AD0 File Offset: 0x003F4CD0
		private Dictionary<string, string> GetCANIdDictionary(List<RenaultCodingECU> ecus)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (RenaultCodingECU renaultCodingECU in ecus)
			{
				dictionary[renaultCodingECU.RequestHeader] = renaultCodingECU.ResponseHeader;
			}
			return dictionary;
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x003F6B30 File Offset: 0x003F4D30
		public async Task<string[]> GetFileNames(IProgress<string> progress)
		{
			List<RenaultCodingECU> found_ecus = new List<RenaultCodingECU>();
			RenaultECUIdents[] array = await this.GetECUIdents(progress);
			for (int i = 0; i < array.Length; i++)
			{
				RenaultECUIdents ident = array[i];
				ident.RequestHeader = ident.RequestHeader.ToUpper();
				ident.soft = ident.soft.ToUpper();
				ident.supplier = ident.supplier.ToUpper();
				ident.version = ident.version.ToUpper();
				Func<RenaultECUIdents, bool> <>9__3;
				List<RenaultCodingECU> list = this.ECUs.Where((RenaultCodingECU x) => x.RequestHeader == ident.RequestHeader).ToList<RenaultCodingECU>().Where(delegate(RenaultCodingECU ecu)
				{
					IEnumerable<RenaultECUIdents> idents2 = ecu.Idents;
					Func<RenaultECUIdents, bool> func3;
					if ((func3 = <>9__3) == null)
					{
						func3 = (<>9__3 = (RenaultECUIdents x) => x.PartiallyEquals(ident));
					}
					return idents2.Any(func3);
				})
					.ToList<RenaultCodingECU>();
				if (ident.RequestHeader == "7E0")
				{
					RenaultCodingECU renaultCodingECU = list.FirstOrDefault((RenaultCodingECU x) => x.Filename == "RenaultFapVer1");
					if (renaultCodingECU != null)
					{
						found_ecus.Add(renaultCodingECU);
						list.Remove(renaultCodingECU);
					}
					renaultCodingECU = list.FirstOrDefault((RenaultCodingECU x) => x.Filename == "RenaultFapVer2");
					if (renaultCodingECU != null)
					{
						found_ecus.Add(renaultCodingECU);
						list.Remove(renaultCodingECU);
					}
					renaultCodingECU = list.FirstOrDefault((RenaultCodingECU x) => x.Filename == "RenaultFapVer3");
					if (renaultCodingECU != null)
					{
						found_ecus.Add(renaultCodingECU);
						list.Remove(renaultCodingECU);
					}
				}
				RenaultCodingECU renaultCodingECU2 = list.FirstOrDefault<RenaultCodingECU>();
				if (renaultCodingECU2 != null)
				{
					if (list.Count > 1)
					{
						int num = renaultCodingECU2.Idents.Max((RenaultECUIdents x) => x.PartiallyEqualsPoints(ident));
						Func<RenaultECUIdents, int> <>9__8;
						for (int j = 1; j < list.Count; j++)
						{
							IEnumerable<RenaultECUIdents> idents = list[j].Idents;
							Func<RenaultECUIdents, int> func;
							if ((func = <>9__8) == null)
							{
								Func<RenaultECUIdents, int> func2 = (RenaultECUIdents x) => x.PartiallyEqualsPoints(ident);
								<>9__8 = func2;
								func = func2;
							}
							int num2 = idents.Max(func);
							if (num2 > num)
							{
								num = num2;
								renaultCodingECU2 = list[j];
							}
						}
					}
					found_ecus.Add(renaultCodingECU2);
				}
			}
			return found_ecus.Select((RenaultCodingECU x) => x.Filename).ToArray<string>();
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x003F6B7C File Offset: 0x003F4D7C
		private async Task<RenaultECUIdents[]> GetECUIdents(IProgress<string> progress)
		{
			Dictionary<string, string> headers = this.GetCANIdDictionary(this.ECUs);
			headers["7E0"] = "7E8";
			headers["7E1"] = "7E9";
			List<RenaultECUIdents> idents = new List<RenaultECUIdents>();
			int total = headers.Keys.Count;
			int current = 0;
			foreach (string text in headers.Keys)
			{
				RenaultUnitsDetector.<>c__DisplayClass5_0 CS$<>8__locals1 = new RenaultUnitsDetector.<>c__DisplayClass5_0();
				CS$<>8__locals1.requestHeader = text;
				int num = current;
				current = num + 1;
				progress.Report(string.Format("[{0}/{1}]", current, total));
				CS$<>8__locals1.responseHeader = headers[CS$<>8__locals1.requestHeader];
				string text2 = "ATFCSH" + CS$<>8__locals1.requestHeader + ";ATFCSD300000;ATFCSM1;ATCRA" + CS$<>8__locals1.responseHeader;
				string text3 = "ATAR;ATFCSM0";
				if (CS$<>8__locals1.requestHeader == "740" || CS$<>8__locals1.requestHeader == "744")
				{
					text2 = string.Concat(new string[] { "ATFCSH", CS$<>8__locals1.requestHeader, ";ATFCSD300000;ATFCSM1;ATCRA", CS$<>8__locals1.responseHeader, ";ATSTFF" });
					text3 = "ATAR;ATFCSM0;ATSTDEF";
				}
				if (CS$<>8__locals1.requestHeader != null && CS$<>8__locals1.requestHeader.StartsWith("7E"))
				{
					text2 = "ATFCSM0;ATCRA" + CS$<>8__locals1.responseHeader;
				}
				if (CS$<>8__locals1.requestHeader.Length == 3)
				{
					text2 = "ATSP6;" + text2;
					text3 += ";ATSPDEF;";
				}
				if (CS$<>8__locals1.requestHeader.Length == 8 || (CS$<>8__locals1.requestHeader.Length == 6 && CS$<>8__locals1.requestHeader.StartsWith("DA")))
				{
					text2 = "ATSP7;" + text2;
					text3 += ";ATSPDEF;";
				}
				if (CS$<>8__locals1.requestHeader.Length == 8)
				{
					string text4 = ";ATCP" + CS$<>8__locals1.requestHeader.Substring(0, 2) + ";";
					CS$<>8__locals1.requestHeader = CS$<>8__locals1.requestHeader.Substring(2);
					text2 += text4;
				}
				CS$<>8__locals1.currentIdent = new RenaultECUIdents();
				OBDRequest obdrequest = new OBDRequest("220121", CS$<>8__locals1.requestHeader, text2, text3, false);
				obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
				{
					if (data != null && data.Length != 0)
					{
						CS$<>8__locals1.currentIdent = RenaultECUIdents.FromLada220121(CS$<>8__locals1.requestHeader, data);
					}
				};
				OBDRequest obdrequest2 = new OBDRequest("220125", CS$<>8__locals1.requestHeader, text2, text3, false);
				obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
				{
					if (data != null && data.Length != 0)
					{
						CS$<>8__locals1.currentIdent.soft = RenaultECUIdents.GetSoftFromLada220125(data);
					}
				};
				OBDRequest obdrequest3 = new OBDRequest("22F1A0", CS$<>8__locals1.requestHeader, text2, text3, false);
				obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
				{
					if (data != null && data.Length != 0)
					{
						CS$<>8__locals1.currentIdent.diagversion = (int)data[0];
					}
				};
				OBDRequest obdrequest4 = new OBDRequest("22F18A", CS$<>8__locals1.requestHeader, text2, text3, false);
				obdrequest4.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
				{
					if (data != null && data.Length != 0)
					{
						CS$<>8__locals1.currentIdent.supplier = Encoding.ASCII.GetString(data).Trim();
					}
				};
				OBDRequest obdrequest5 = new OBDRequest("22F194", CS$<>8__locals1.requestHeader, text2, text3, false);
				obdrequest5.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
				{
					if (data != null && data.Length != 0)
					{
						CS$<>8__locals1.currentIdent.soft = Encoding.ASCII.GetString(data).Trim();
					}
				};
				OBDRequest obdrequest6 = new OBDRequest("22F195", CS$<>8__locals1.requestHeader, text2, text3, false);
				obdrequest6.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
				{
					if (data != null && data.Length != 0)
					{
						CS$<>8__locals1.currentIdent.version = Encoding.ASCII.GetString(data).Trim();
					}
				};
				OBDRequest obdrequest7 = new OBDRequest("2180", CS$<>8__locals1.requestHeader, text2, text3, false)
				{
					ELMFormat = ELMFormat.CAN11bit,
					ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
				};
				obdrequest7.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
				{
					if (data != null && data.Length != 0)
					{
						RenaultECUIdents renaultECUIdents = RenaultECUIdents.From2180(CS$<>8__locals1.requestHeader, data);
						if (renaultECUIdents != null && !renaultECUIdents.IsEmpty)
						{
							CS$<>8__locals1.currentIdent = renaultECUIdents;
							App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == request.Header);
						}
					}
				};
				OBDRequest obdrequest8 = new OBDRequest("10C0", CS$<>8__locals1.requestHeader, text2, text3, false);
				obdrequest8.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (request.Command == "10C0" && data != null && data.Contains("NO DATA") && (request.Header == "744" || request.Header == "74D" || request.Header == "742" || request.Header == "7E0" || request.Header == "DAF115" || request.Header == "18DAF115"))
					{
						request.Command = "1003";
						App.OBDReader.InsertRequestInQueue(request);
						return;
					}
					if (string.IsNullOrEmpty(data) || data.Contains("NO DATA") || data.Contains("ERROR") || !data.Contains(CS$<>8__locals1.responseHeader))
					{
						App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == request.Header);
					}
				};
				List<OBDRequest> requests = new List<OBDRequest> { obdrequest8, obdrequest7, obdrequest3, obdrequest4, obdrequest5, obdrequest6 };
				if (CS$<>8__locals1.requestHeader == "743")
				{
					string selectedBrand = SharedSettings.Current.SelectedBrand;
					if (selectedBrand == "Lada" || selectedBrand == "VAZ" || selectedBrand == "Лада" || selectedBrand == "ВАЗ")
					{
						requests.Add(obdrequest);
						requests.Add(obdrequest2);
					}
				}
				foreach (OBDRequest obdrequest9 in requests)
				{
					obdrequest9.ELMFormat = ELMFormat.CAN11bit;
					obdrequest9.ForceManualFlowControl = false;
				}
				App.OBDReader.ReplaceQueue(requests);
				await App.OBDReader.WaitForCommandQueue();
				if (!CS$<>8__locals1.currentIdent.IsEmpty)
				{
					CS$<>8__locals1.currentIdent.RequestHeader = CS$<>8__locals1.requestHeader;
					idents.Add(CS$<>8__locals1.currentIdent);
				}
				else
				{
					foreach (OBDRequest obdrequest10 in requests)
					{
						obdrequest10.ELMFormat = ELMFormat.CAN11bit;
						obdrequest10.ForceManualFlowControl = true;
					}
					requests.Insert(0, (!string.IsNullOrEmpty(SharedSettings.Current.DetectECUConnectionPID)) ? new OBDRequest(SharedSettings.Current.DetectECUConnectionPID, false) : App.OBDReader.GetDefaultPidRequest());
					App.OBDReader.ReplaceQueue(requests);
					await App.OBDReader.WaitForCommandQueue();
					if (!CS$<>8__locals1.currentIdent.IsEmpty)
					{
						CS$<>8__locals1.currentIdent.RequestHeader = CS$<>8__locals1.requestHeader;
						idents.Add(CS$<>8__locals1.currentIdent);
					}
				}
				CS$<>8__locals1 = null;
				requests = null;
			}
			Dictionary<string, string>.KeyCollection.Enumerator enumerator = default(Dictionary<string, string>.KeyCollection.Enumerator);
			return idents.ToArray();
		}

		// Token: 0x040031FA RID: 12794
		private List<RenaultCodingECU> ECUs;

		// Token: 0x02000A0C RID: 2572
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005218 RID: 21016 RVA: 0x003F6BC7 File Offset: 0x003F4DC7
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005219 RID: 21017 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600521A RID: 21018 RVA: 0x003F6BD3 File Offset: 0x003F4DD3
			internal bool <GetFileNames>b__4_4(RenaultCodingECU x)
			{
				return x.Filename == "RenaultFapVer1";
			}

			// Token: 0x0600521B RID: 21019 RVA: 0x003F6BE5 File Offset: 0x003F4DE5
			internal bool <GetFileNames>b__4_5(RenaultCodingECU x)
			{
				return x.Filename == "RenaultFapVer2";
			}

			// Token: 0x0600521C RID: 21020 RVA: 0x003F6BF7 File Offset: 0x003F4DF7
			internal bool <GetFileNames>b__4_6(RenaultCodingECU x)
			{
				return x.Filename == "RenaultFapVer3";
			}

			// Token: 0x0600521D RID: 21021 RVA: 0x003F6C09 File Offset: 0x003F4E09
			internal string <GetFileNames>b__4_0(RenaultCodingECU x)
			{
				return x.Filename;
			}

			// Token: 0x040031FB RID: 12795
			public static readonly RenaultUnitsDetector.<>c <>9 = new RenaultUnitsDetector.<>c();

			// Token: 0x040031FC RID: 12796
			public static Func<RenaultCodingECU, bool> <>9__4_4;

			// Token: 0x040031FD RID: 12797
			public static Func<RenaultCodingECU, bool> <>9__4_5;

			// Token: 0x040031FE RID: 12798
			public static Func<RenaultCodingECU, bool> <>9__4_6;

			// Token: 0x040031FF RID: 12799
			public static Func<RenaultCodingECU, string> <>9__4_0;
		}

		// Token: 0x02000A0D RID: 2573
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x0600521E RID: 21022 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x0600521F RID: 21023 RVA: 0x003F6C11 File Offset: 0x003F4E11
			internal bool <GetFileNames>b__1(RenaultCodingECU x)
			{
				return x.RequestHeader == this.ident.RequestHeader;
			}

			// Token: 0x06005220 RID: 21024 RVA: 0x003F6C2C File Offset: 0x003F4E2C
			internal bool <GetFileNames>b__2(RenaultCodingECU ecu)
			{
				IEnumerable<RenaultECUIdents> idents = ecu.Idents;
				Func<RenaultECUIdents, bool> func;
				if ((func = this.<>9__3) == null)
				{
					func = (this.<>9__3 = (RenaultECUIdents x) => x.PartiallyEquals(this.ident));
				}
				return idents.Any(func);
			}

			// Token: 0x06005221 RID: 21025 RVA: 0x003F6C63 File Offset: 0x003F4E63
			internal bool <GetFileNames>b__3(RenaultECUIdents x)
			{
				return x.PartiallyEquals(this.ident);
			}

			// Token: 0x06005222 RID: 21026 RVA: 0x003F6C71 File Offset: 0x003F4E71
			internal int <GetFileNames>b__7(RenaultECUIdents x)
			{
				return x.PartiallyEqualsPoints(this.ident);
			}

			// Token: 0x06005223 RID: 21027 RVA: 0x003F6C71 File Offset: 0x003F4E71
			internal int <GetFileNames>b__8(RenaultECUIdents x)
			{
				return x.PartiallyEqualsPoints(this.ident);
			}

			// Token: 0x04003200 RID: 12800
			public RenaultECUIdents ident;

			// Token: 0x04003201 RID: 12801
			public Func<RenaultECUIdents, bool> <>9__3;

			// Token: 0x04003202 RID: 12802
			public Func<RenaultECUIdents, int> <>9__8;
		}

		// Token: 0x02000A0E RID: 2574
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x06005224 RID: 21028 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x06005225 RID: 21029 RVA: 0x003F6C7F File Offset: 0x003F4E7F
			internal void <GetECUIdents>b__0(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.currentIdent = RenaultECUIdents.FromLada220121(this.requestHeader, data);
				}
			}

			// Token: 0x06005226 RID: 21030 RVA: 0x003F6C9A File Offset: 0x003F4E9A
			internal void <GetECUIdents>b__1(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.currentIdent.soft = RenaultECUIdents.GetSoftFromLada220125(data);
				}
			}

			// Token: 0x06005227 RID: 21031 RVA: 0x003F6CB4 File Offset: 0x003F4EB4
			internal void <GetECUIdents>b__2(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.currentIdent.diagversion = (int)data[0];
				}
			}

			// Token: 0x06005228 RID: 21032 RVA: 0x003F6CCB File Offset: 0x003F4ECB
			internal void <GetECUIdents>b__3(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.currentIdent.supplier = Encoding.ASCII.GetString(data).Trim();
				}
			}

			// Token: 0x06005229 RID: 21033 RVA: 0x003F6CEF File Offset: 0x003F4EEF
			internal void <GetECUIdents>b__4(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.currentIdent.soft = Encoding.ASCII.GetString(data).Trim();
				}
			}

			// Token: 0x0600522A RID: 21034 RVA: 0x003F6D13 File Offset: 0x003F4F13
			internal void <GetECUIdents>b__5(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.currentIdent.version = Encoding.ASCII.GetString(data).Trim();
				}
			}

			// Token: 0x0600522B RID: 21035 RVA: 0x003F6D38 File Offset: 0x003F4F38
			internal void <GetECUIdents>b__6(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				RenaultUnitsDetector.<>c__DisplayClass5_1 CS$<>8__locals1 = new RenaultUnitsDetector.<>c__DisplayClass5_1();
				CS$<>8__locals1.request = request;
				if (data != null && data.Length != 0)
				{
					RenaultECUIdents renaultECUIdents = RenaultECUIdents.From2180(this.requestHeader, data);
					if (renaultECUIdents != null && !renaultECUIdents.IsEmpty)
					{
						this.currentIdent = renaultECUIdents;
						App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
					}
				}
			}

			// Token: 0x0600522C RID: 21036 RVA: 0x003F6D90 File Offset: 0x003F4F90
			internal void <GetECUIdents>b__7(OBDRequest request, string data)
			{
				RenaultUnitsDetector.<>c__DisplayClass5_2 CS$<>8__locals1 = new RenaultUnitsDetector.<>c__DisplayClass5_2();
				CS$<>8__locals1.request = request;
				if (CS$<>8__locals1.request.Command == "10C0" && data != null && data.Contains("NO DATA") && (CS$<>8__locals1.request.Header == "744" || CS$<>8__locals1.request.Header == "74D" || CS$<>8__locals1.request.Header == "742" || CS$<>8__locals1.request.Header == "7E0" || CS$<>8__locals1.request.Header == "DAF115" || CS$<>8__locals1.request.Header == "18DAF115"))
				{
					CS$<>8__locals1.request.Command = "1003";
					App.OBDReader.InsertRequestInQueue(CS$<>8__locals1.request);
					return;
				}
				if (string.IsNullOrEmpty(data) || data.Contains("NO DATA") || data.Contains("ERROR") || !data.Contains(this.responseHeader))
				{
					App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
				}
			}

			// Token: 0x04003203 RID: 12803
			public RenaultECUIdents currentIdent;

			// Token: 0x04003204 RID: 12804
			public string requestHeader;

			// Token: 0x04003205 RID: 12805
			public string responseHeader;
		}

		// Token: 0x02000A0F RID: 2575
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_1
		{
			// Token: 0x0600522D RID: 21037 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_1()
			{
			}

			// Token: 0x0600522E RID: 21038 RVA: 0x003F6ECB File Offset: 0x003F50CB
			internal bool <GetECUIdents>b__8(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04003206 RID: 12806
			public OBDRequest request;
		}

		// Token: 0x02000A10 RID: 2576
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_2
		{
			// Token: 0x0600522F RID: 21039 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_2()
			{
			}

			// Token: 0x06005230 RID: 21040 RVA: 0x003F6EE3 File Offset: 0x003F50E3
			internal bool <GetECUIdents>b__9(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04003207 RID: 12807
			public OBDRequest request;
		}

		// Token: 0x02000A11 RID: 2577
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUIdents>d__5 : IAsyncStateMachine
		{
			// Token: 0x06005231 RID: 21041 RVA: 0x003F6EFC File Offset: 0x003F50FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RenaultUnitsDetector renaultUnitsDetector = this;
				RenaultECUIdents[] array;
				try
				{
					if (num > 1)
					{
						headers = renaultUnitsDetector.GetCANIdDictionary(renaultUnitsDetector.ECUs);
						headers["7E0"] = "7E8";
						headers["7E1"] = "7E9";
						idents = new List<RenaultECUIdents>();
						total = headers.Keys.Count;
						current = 0;
						enumerator = headers.Keys.GetEnumerator();
					}
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							if (num != 1)
							{
								goto IL_0797;
							}
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_073F;
						}
						else
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						IL_0606:
						taskAwaiter.GetResult();
						if (!CS$<>8__locals1.currentIdent.IsEmpty)
						{
							CS$<>8__locals1.currentIdent.RequestHeader = CS$<>8__locals1.requestHeader;
							idents.Add(CS$<>8__locals1.currentIdent);
							goto IL_0789;
						}
						List<OBDRequest>.Enumerator enumerator2 = requests.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								OBDRequest obdrequest = enumerator2.Current;
								obdrequest.ELMFormat = ELMFormat.CAN11bit;
								obdrequest.ForceManualFlowControl = true;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						OBDRequest obdrequest2;
						if (string.IsNullOrEmpty(SharedSettings.Current.DetectECUConnectionPID))
						{
							obdrequest2 = App.OBDReader.GetDefaultPidRequest();
						}
						else
						{
							obdrequest2 = new OBDRequest(SharedSettings.Current.DetectECUConnectionPID, false);
						}
						requests.Insert(0, obdrequest2);
						App.OBDReader.ReplaceQueue(requests);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultUnitsDetector.<GetECUIdents>d__5>(ref taskAwaiter, ref this);
							return;
						}
						IL_073F:
						taskAwaiter.GetResult();
						if (!CS$<>8__locals1.currentIdent.IsEmpty)
						{
							CS$<>8__locals1.currentIdent.RequestHeader = CS$<>8__locals1.requestHeader;
							idents.Add(CS$<>8__locals1.currentIdent);
						}
						IL_0789:
						CS$<>8__locals1 = null;
						requests = null;
						IL_0797:
						if (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							CS$<>8__locals1 = new RenaultUnitsDetector.<>c__DisplayClass5_0();
							CS$<>8__locals1.requestHeader = text;
							int num3 = current;
							current = num3 + 1;
							progress.Report(string.Format("[{0}/{1}]", current, total));
							CS$<>8__locals1.responseHeader = headers[CS$<>8__locals1.requestHeader];
							string text2 = "ATFCSH" + CS$<>8__locals1.requestHeader + ";ATFCSD300000;ATFCSM1;ATCRA" + CS$<>8__locals1.responseHeader;
							string text3 = "ATAR;ATFCSM0";
							if (CS$<>8__locals1.requestHeader == "740" || CS$<>8__locals1.requestHeader == "744")
							{
								text2 = string.Concat(new string[] { "ATFCSH", CS$<>8__locals1.requestHeader, ";ATFCSD300000;ATFCSM1;ATCRA", CS$<>8__locals1.responseHeader, ";ATSTFF" });
								text3 = "ATAR;ATFCSM0;ATSTDEF";
							}
							if (CS$<>8__locals1.requestHeader != null && CS$<>8__locals1.requestHeader.StartsWith("7E"))
							{
								text2 = "ATFCSM0;ATCRA" + CS$<>8__locals1.responseHeader;
							}
							if (CS$<>8__locals1.requestHeader.Length == 3)
							{
								text2 = "ATSP6;" + text2;
								text3 += ";ATSPDEF;";
							}
							if (CS$<>8__locals1.requestHeader.Length == 8 || (CS$<>8__locals1.requestHeader.Length == 6 && CS$<>8__locals1.requestHeader.StartsWith("DA")))
							{
								text2 = "ATSP7;" + text2;
								text3 += ";ATSPDEF;";
							}
							if (CS$<>8__locals1.requestHeader.Length == 8)
							{
								string text4 = ";ATCP" + CS$<>8__locals1.requestHeader.Substring(0, 2) + ";";
								CS$<>8__locals1.requestHeader = CS$<>8__locals1.requestHeader.Substring(2);
								text2 += text4;
							}
							CS$<>8__locals1.currentIdent = new RenaultECUIdents();
							OBDRequest obdrequest3 = new OBDRequest("220121", CS$<>8__locals1.requestHeader, text2, text3, false);
							obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
							{
								if (data != null && data.Length != 0)
								{
									CS$<>8__locals1.currentIdent = RenaultECUIdents.FromLada220121(CS$<>8__locals1.requestHeader, data);
								}
							};
							OBDRequest obdrequest4 = new OBDRequest("220125", CS$<>8__locals1.requestHeader, text2, text3, false);
							obdrequest4.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
							{
								if (data != null && data.Length != 0)
								{
									CS$<>8__locals1.currentIdent.soft = RenaultECUIdents.GetSoftFromLada220125(data);
								}
							};
							OBDRequest obdrequest5 = new OBDRequest("22F1A0", CS$<>8__locals1.requestHeader, text2, text3, false);
							obdrequest5.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
							{
								if (data != null && data.Length != 0)
								{
									CS$<>8__locals1.currentIdent.diagversion = (int)data[0];
								}
							};
							OBDRequest obdrequest6 = new OBDRequest("22F18A", CS$<>8__locals1.requestHeader, text2, text3, false);
							obdrequest6.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
							{
								if (data != null && data.Length != 0)
								{
									CS$<>8__locals1.currentIdent.supplier = Encoding.ASCII.GetString(data).Trim();
								}
							};
							OBDRequest obdrequest7 = new OBDRequest("22F194", CS$<>8__locals1.requestHeader, text2, text3, false);
							obdrequest7.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
							{
								if (data != null && data.Length != 0)
								{
									CS$<>8__locals1.currentIdent.soft = Encoding.ASCII.GetString(data).Trim();
								}
							};
							OBDRequest obdrequest8 = new OBDRequest("22F195", CS$<>8__locals1.requestHeader, text2, text3, false);
							obdrequest8.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
							{
								if (data != null && data.Length != 0)
								{
									CS$<>8__locals1.currentIdent.version = Encoding.ASCII.GetString(data).Trim();
								}
							};
							OBDRequest obdrequest9 = new OBDRequest("2180", CS$<>8__locals1.requestHeader, text2, text3, false)
							{
								ELMFormat = ELMFormat.CAN11bit,
								ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
							};
							obdrequest9.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
							{
								RenaultUnitsDetector.<>c__DisplayClass5_1 CS$<>8__locals1 = new RenaultUnitsDetector.<>c__DisplayClass5_1();
								CS$<>8__locals1.request = request;
								if (data != null && data.Length != 0)
								{
									RenaultECUIdents renaultECUIdents = RenaultECUIdents.From2180(CS$<>8__locals1.requestHeader, data);
									if (renaultECUIdents != null && !renaultECUIdents.IsEmpty)
									{
										CS$<>8__locals1.currentIdent = renaultECUIdents;
										App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
									}
								}
							};
							OBDRequest obdrequest10 = new OBDRequest("10C0", CS$<>8__locals1.requestHeader, text2, text3, false);
							obdrequest10.ResponseReceived += delegate(OBDRequest request, string data)
							{
								RenaultUnitsDetector.<>c__DisplayClass5_2 CS$<>8__locals2 = new RenaultUnitsDetector.<>c__DisplayClass5_2();
								CS$<>8__locals2.request = request;
								if (CS$<>8__locals2.request.Command == "10C0" && data != null && data.Contains("NO DATA") && (CS$<>8__locals2.request.Header == "744" || CS$<>8__locals2.request.Header == "74D" || CS$<>8__locals2.request.Header == "742" || CS$<>8__locals2.request.Header == "7E0" || CS$<>8__locals2.request.Header == "DAF115" || CS$<>8__locals2.request.Header == "18DAF115"))
								{
									CS$<>8__locals2.request.Command = "1003";
									App.OBDReader.InsertRequestInQueue(CS$<>8__locals2.request);
									return;
								}
								if (string.IsNullOrEmpty(data) || data.Contains("NO DATA") || data.Contains("ERROR") || !data.Contains(CS$<>8__locals1.responseHeader))
								{
									App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == CS$<>8__locals2.request.Header);
								}
							};
							requests = new List<OBDRequest> { obdrequest10, obdrequest9, obdrequest5, obdrequest6, obdrequest7, obdrequest8 };
							if (CS$<>8__locals1.requestHeader == "743")
							{
								string selectedBrand = SharedSettings.Current.SelectedBrand;
								if (selectedBrand == "Lada" || selectedBrand == "VAZ" || selectedBrand == "Лада" || selectedBrand == "ВАЗ")
								{
									requests.Add(obdrequest3);
									requests.Add(obdrequest4);
								}
							}
							enumerator2 = requests.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									OBDRequest obdrequest11 = enumerator2.Current;
									obdrequest11.ELMFormat = ELMFormat.CAN11bit;
									obdrequest11.ForceManualFlowControl = false;
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator2).Dispose();
								}
							}
							App.OBDReader.ReplaceQueue(requests);
							taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultUnitsDetector.<GetECUIdents>d__5>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0606;
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					enumerator = default(Dictionary<string, string>.KeyCollection.Enumerator);
					array = idents.ToArray();
				}
				catch (Exception ex)
				{
					num2 = -2;
					headers = null;
					idents = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				headers = null;
				idents = null;
				this.<>t__builder.SetResult(array);
			}

			// Token: 0x06005232 RID: 21042 RVA: 0x003F7790 File Offset: 0x003F5990
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003208 RID: 12808
			public int <>1__state;

			// Token: 0x04003209 RID: 12809
			public AsyncTaskMethodBuilder<RenaultECUIdents[]> <>t__builder;

			// Token: 0x0400320A RID: 12810
			public RenaultUnitsDetector <>4__this;

			// Token: 0x0400320B RID: 12811
			public IProgress<string> progress;

			// Token: 0x0400320C RID: 12812
			private RenaultUnitsDetector.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x0400320D RID: 12813
			private Dictionary<string, string> <headers>5__2;

			// Token: 0x0400320E RID: 12814
			private List<RenaultECUIdents> <idents>5__3;

			// Token: 0x0400320F RID: 12815
			private int <total>5__4;

			// Token: 0x04003210 RID: 12816
			private int <current>5__5;

			// Token: 0x04003211 RID: 12817
			private Dictionary<string, string>.KeyCollection.Enumerator <>7__wrap5;

			// Token: 0x04003212 RID: 12818
			private List<OBDRequest> <requests>5__7;

			// Token: 0x04003213 RID: 12819
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000A12 RID: 2578
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetFileNames>d__4 : IAsyncStateMachine
		{
			// Token: 0x06005233 RID: 21043 RVA: 0x003F77A0 File Offset: 0x003F59A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RenaultUnitsDetector renaultUnitsDetector = this;
				string[] array;
				try
				{
					TaskAwaiter<RenaultECUIdents[]> taskAwaiter;
					if (num != 0)
					{
						found_ecus = new List<RenaultCodingECU>();
						taskAwaiter = renaultUnitsDetector.GetECUIdents(progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<RenaultECUIdents[]> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RenaultECUIdents[]>, RenaultUnitsDetector.<GetFileNames>d__4>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<RenaultECUIdents[]> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<RenaultECUIdents[]>);
						num2 = -1;
					}
					RenaultECUIdents[] result = taskAwaiter.GetResult();
					for (int i = 0; i < result.Length; i++)
					{
						RenaultUnitsDetector.<>c__DisplayClass4_0 CS$<>8__locals1 = new RenaultUnitsDetector.<>c__DisplayClass4_0();
						CS$<>8__locals1.ident = result[i];
						CS$<>8__locals1.ident.RequestHeader = CS$<>8__locals1.ident.RequestHeader.ToUpper();
						CS$<>8__locals1.ident.soft = CS$<>8__locals1.ident.soft.ToUpper();
						CS$<>8__locals1.ident.supplier = CS$<>8__locals1.ident.supplier.ToUpper();
						CS$<>8__locals1.ident.version = CS$<>8__locals1.ident.version.ToUpper();
						List<RenaultCodingECU> list = renaultUnitsDetector.ECUs.Where((RenaultCodingECU x) => x.RequestHeader == CS$<>8__locals1.ident.RequestHeader).ToList<RenaultCodingECU>().Where(delegate(RenaultCodingECU ecu)
						{
							IEnumerable<RenaultECUIdents> idents2 = ecu.Idents;
							Func<RenaultECUIdents, bool> func2;
							if ((func2 = CS$<>8__locals1.<>9__3) == null)
							{
								func2 = (CS$<>8__locals1.<>9__3 = (RenaultECUIdents x) => x.PartiallyEquals(CS$<>8__locals1.ident));
							}
							return idents2.Any(func2);
						})
							.ToList<RenaultCodingECU>();
						if (CS$<>8__locals1.ident.RequestHeader == "7E0")
						{
							RenaultCodingECU renaultCodingECU = list.FirstOrDefault((RenaultCodingECU x) => x.Filename == "RenaultFapVer1");
							if (renaultCodingECU != null)
							{
								found_ecus.Add(renaultCodingECU);
								list.Remove(renaultCodingECU);
							}
							renaultCodingECU = list.FirstOrDefault((RenaultCodingECU x) => x.Filename == "RenaultFapVer2");
							if (renaultCodingECU != null)
							{
								found_ecus.Add(renaultCodingECU);
								list.Remove(renaultCodingECU);
							}
							renaultCodingECU = list.FirstOrDefault((RenaultCodingECU x) => x.Filename == "RenaultFapVer3");
							if (renaultCodingECU != null)
							{
								found_ecus.Add(renaultCodingECU);
								list.Remove(renaultCodingECU);
							}
						}
						RenaultCodingECU renaultCodingECU2 = list.FirstOrDefault<RenaultCodingECU>();
						if (renaultCodingECU2 != null)
						{
							if (list.Count > 1)
							{
								int num3 = renaultCodingECU2.Idents.Max((RenaultECUIdents x) => x.PartiallyEqualsPoints(CS$<>8__locals1.ident));
								for (int j = 1; j < list.Count; j++)
								{
									IEnumerable<RenaultECUIdents> idents = list[j].Idents;
									Func<RenaultECUIdents, int> func;
									if ((func = CS$<>8__locals1.<>9__8) == null)
									{
										func = (CS$<>8__locals1.<>9__8 = (RenaultECUIdents x) => x.PartiallyEqualsPoints(CS$<>8__locals1.ident));
									}
									int num4 = idents.Max(func);
									if (num4 > num3)
									{
										num3 = num4;
										renaultCodingECU2 = list[j];
									}
								}
							}
							found_ecus.Add(renaultCodingECU2);
						}
					}
					array = found_ecus.Select((RenaultCodingECU x) => x.Filename).ToArray<string>();
				}
				catch (Exception ex)
				{
					num2 = -2;
					found_ecus = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				found_ecus = null;
				this.<>t__builder.SetResult(array);
			}

			// Token: 0x06005234 RID: 21044 RVA: 0x003F7B1C File Offset: 0x003F5D1C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003214 RID: 12820
			public int <>1__state;

			// Token: 0x04003215 RID: 12821
			public AsyncTaskMethodBuilder<string[]> <>t__builder;

			// Token: 0x04003216 RID: 12822
			public RenaultUnitsDetector <>4__this;

			// Token: 0x04003217 RID: 12823
			public IProgress<string> progress;

			// Token: 0x04003218 RID: 12824
			private List<RenaultCodingECU> <found_ecus>5__2;

			// Token: 0x04003219 RID: 12825
			private TaskAwaiter<RenaultECUIdents[]> <>u__1;
		}
	}
}
