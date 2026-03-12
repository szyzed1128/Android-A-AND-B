using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Toyota
{
	// Token: 0x020009EA RID: 2538
	internal class ToyotaUnitsDetector
	{
		// Token: 0x060051A1 RID: 20897 RVA: 0x003F2BF8 File Offset: 0x003F0DF8
		public async Task<List<CustomizableCodingTemplate>> CheckAvailableCodings(List<CustomizableCodingTemplate> codings, IProgress<string> progress)
		{
			List<CustomizableCodingTemplate> result = codings.ToList<CustomizableCodingTemplate>();
			try
			{
				List<KeyValuePair<string, string>> headers = this.GetHeaders(codings);
				List<OBDRequest> requests = new List<OBDRequest>(headers.Count);
				int progressCounter = 1;
				foreach (KeyValuePair<string, string> keyValuePair in headers)
				{
					string key = keyValuePair.Key;
					string extAddr2 = keyValuePair.Value;
					OBDRequest obdrequestForUnit = ToyotaUnitsDetector.GetOBDRequestForUnit("3E", key, extAddr2);
					obdrequestForUnit.ResponseReceived += delegate(OBDRequest req, string response)
					{
						IProgress<string> progress2 = progress;
						if (progress2 != null)
						{
							progress2.Report(string.Format("1/2 ({0}%)", progressCounter * 100 / headers.Count));
						}
						int progressCounter3 = progressCounter;
						progressCounter = progressCounter3 + 1;
						if (!response.Contains("7F3E") && !response.Contains("7E") && !response.Contains("7F 3E"))
						{
							result.RemoveAll((CustomizableCodingTemplate x) => x.GetRequestHeaderForELM327() == req.Header && x.ExtendedAddress == extAddr2);
						}
					};
					requests.Add(obdrequestForUnit);
				}
				App.OBDReader.ReplaceQueue(requests);
				await App.OBDReader.WaitForCommandQueue();
				headers = this.GetHeaders(result);
				requests.Clear();
				List<ToyotaA8Item> A8items = new List<ToyotaA8Item>();
				progressCounter = 1;
				foreach (KeyValuePair<string, string> keyValuePair2 in headers)
				{
					try
					{
						string key2 = keyValuePair2.Key;
						string extAddr = keyValuePair2.Value;
						string[] array = new string[] { "A801", "A803" };
						ResponseReceivedDelegate <>9__4;
						for (int i = 0; i < array.Length; i++)
						{
							OBDRequest obdrequestForUnit2 = ToyotaUnitsDetector.GetOBDRequestForUnit(array[i], key2, extAddr);
							obdrequestForUnit2.ForceManualFlowControl = false;
							ResponseReceivedDelegate responseReceivedDelegate;
							if ((responseReceivedDelegate = <>9__4) == null)
							{
								ResponseReceivedDelegate responseReceivedDelegate2 = delegate(OBDRequest req, string response)
								{
									IProgress<string> progress3 = progress;
									if (progress3 != null)
									{
										progress3.Report(string.Format("2/2 ({0}%)", progressCounter * 100 / (headers.Count * 2)));
									}
									int progressCounter2 = progressCounter;
									progressCounter = progressCounter2 + 1;
									List<ToyotaA8Item> list2 = ToyotaA8Parser.ParseResponse(req, extAddr, response);
									A8items.AddRange(list2);
								};
								<>9__4 = responseReceivedDelegate2;
								responseReceivedDelegate = responseReceivedDelegate2;
							}
							obdrequestForUnit2.ResponseReceived += responseReceivedDelegate;
							requests.Add(obdrequestForUnit2);
						}
					}
					catch (Exception)
					{
					}
				}
				App.OBDReader.ReplaceQueue(requests);
				await App.OBDReader.WaitForCommandQueue();
				using (List<CustomizableCodingTemplate>.Enumerator enumerator2 = result.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CustomizableCodingTemplate coding = enumerator2.Current;
						try
						{
							string codingId = coding.WriteModeAndAddress.Substring(2);
							ToyotaA8Item toyotaA8Item = A8items.FirstOrDefault((ToyotaA8Item x) => x.Type == ToyotaA8Item.ItemTypes.WriteA803 && x.CanIdHex == coding.GetRequestHeaderForELM327() && x.ExtendedAddress == coding.ExtendedAddress && x.IdHex == codingId);
							if (toyotaA8Item != null)
							{
								coding.PreWriteDataProcessor = toyotaA8Item;
							}
						}
						catch (Exception)
						{
						}
					}
				}
				List<CustomizableCodingTemplate> list = result.Where((CustomizableCodingTemplate x) => x.PreWriteDataProcessor != null && x.PreWriteDataProcessor is ToyotaA8Item).ToList<CustomizableCodingTemplate>();
				result = list.Where((CustomizableCodingTemplate x) => (x.PreWriteDataProcessor as ToyotaA8Item).CheckIfCodingIsSupported(x)).ToList<CustomizableCodingTemplate>();
				requests = null;
			}
			catch (Exception ex)
			{
				App.OBDReader.DebugWrite(ex.ToString());
			}
			return result;
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x003F2C4C File Offset: 0x003F0E4C
		public async Task ReadAllUnits(IProgress<string> progress)
		{
			List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("700", ""),
				new KeyValuePair<string, string>("750", "B5"),
				new KeyValuePair<string, string>("7C0", ""),
				new KeyValuePair<string, string>("750", "40"),
				new KeyValuePair<string, string>("7C4", ""),
				new KeyValuePair<string, string>("750", "67"),
				new KeyValuePair<string, string>("750", "B6"),
				new KeyValuePair<string, string>("750", "41"),
				new KeyValuePair<string, string>("750", "E0"),
				new KeyValuePair<string, string>("750", "90"),
				new KeyValuePair<string, string>("750", "91"),
				new KeyValuePair<string, string>("750", "92"),
				new KeyValuePair<string, string>("750", "93"),
				new KeyValuePair<string, string>("750", "B9"),
				new KeyValuePair<string, string>("750", "A1"),
				new KeyValuePair<string, string>("750", "A2"),
				new KeyValuePair<string, string>("750", "A4"),
				new KeyValuePair<string, string>("750", "A7"),
				new KeyValuePair<string, string>("750", "B8"),
				new KeyValuePair<string, string>("750", "AB"),
				new KeyValuePair<string, string>("750", "80"),
				new KeyValuePair<string, string>("750", "83"),
				new KeyValuePair<string, string>("750", "85"),
				new KeyValuePair<string, string>("750", "8A"),
				new KeyValuePair<string, string>("750", "8B"),
				new KeyValuePair<string, string>("750", "8C"),
				new KeyValuePair<string, string>("750", "8D"),
				new KeyValuePair<string, string>("750", "DC"),
				new KeyValuePair<string, string>("750", "C7"),
				new KeyValuePair<string, string>("750", "70"),
				new KeyValuePair<string, string>("750", "36"),
				new KeyValuePair<string, string>("750", "44"),
				new KeyValuePair<string, string>("791", ""),
				new KeyValuePair<string, string>("799", ""),
				new KeyValuePair<string, string>("781", ""),
				new KeyValuePair<string, string>("789", ""),
				new KeyValuePair<string, string>("750", "A8"),
				new KeyValuePair<string, string>("750", "4F"),
				new KeyValuePair<string, string>("750", "B0"),
				new KeyValuePair<string, string>("750", "AE"),
				new KeyValuePair<string, string>("750", "69"),
				new KeyValuePair<string, string>("750", "D3"),
				new KeyValuePair<string, string>("750", "F3"),
				new KeyValuePair<string, string>("7B0", ""),
				new KeyValuePair<string, string>("7E0", ""),
				new KeyValuePair<string, string>("750", "2A")
			};
			List<OBDRequest> list = new List<OBDRequest>(headers.Count * 3);
			List<ToyotaA8Item> A8items = new List<ToyotaA8Item>();
			int progressCounter = 1;
			foreach (KeyValuePair<string, string> keyValuePair in headers)
			{
				try
				{
					string key = keyValuePair.Key;
					string extAddr = keyValuePair.Value;
					string[] array = new string[] { "A801", "A803", "222000" };
					ResponseReceivedDelegate <>9__0;
					for (int i = 0; i < array.Length; i++)
					{
						OBDRequest obdrequestForUnit = ToyotaUnitsDetector.GetOBDRequestForUnit(array[i], key, extAddr);
						obdrequestForUnit.ForceManualFlowControl = false;
						OBDRequest obdrequest = obdrequestForUnit;
						ResponseReceivedDelegate responseReceivedDelegate;
						if ((responseReceivedDelegate = <>9__0) == null)
						{
							responseReceivedDelegate = (<>9__0 = delegate(OBDRequest req, string response)
							{
								IProgress<string> progress2 = progress;
								if (progress2 != null)
								{
									progress2.Report(string.Format("{0}%", progressCounter * 100 / (headers.Count * 3)));
								}
								int progressCounter2 = progressCounter;
								progressCounter = progressCounter2 + 1;
								List<ToyotaA8Item> list2 = ToyotaA8Parser.ParseResponse(req, extAddr, response);
								A8items.AddRange(list2);
							});
						}
						obdrequest.ResponseReceived += responseReceivedDelegate;
						list.Add(obdrequestForUnit);
					}
				}
				catch (Exception)
				{
				}
			}
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x003F2C90 File Offset: 0x003F0E90
		private List<KeyValuePair<string, string>> GetHeaders(List<CustomizableCodingTemplate> codings)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (CustomizableCodingTemplate customizableCodingTemplate in codings)
			{
				string requestHeaderForELM = customizableCodingTemplate.GetRequestHeaderForELM327();
				string extendedAddress = customizableCodingTemplate.ExtendedAddress;
				KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(requestHeaderForELM, extendedAddress);
				if (!list.Any((KeyValuePair<string, string> x) => x.Key == kvp.Key && x.Value == kvp.Value))
				{
					list.Add(kvp);
				}
			}
			return list;
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x003F2D20 File Offset: 0x003F0F20
		internal static OBDRequest GetOBDRequestForUnit(string cmd, string requestHeader, string extenededAddress)
		{
			string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(requestHeader, "Toyota", null);
			string text;
			string text2;
			if (string.IsNullOrEmpty(extenededAddress))
			{
				text = "ATFCSH" + requestHeader + ";ATFCSD300010;ATFCSM1;ATAL;ATCRA" + possibleResponseHeader;
				text2 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF";
			}
			else
			{
				text = string.Concat(new string[] { "ATFCSH", requestHeader, ";ATFCSD", extenededAddress, "300010;ATFCSM1;ATAL;ATCRA", possibleResponseHeader, ";ATCEA", extenededAddress, ";ATTA", extenededAddress });
				text2 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF";
			}
			return new OBDRequest(cmd, requestHeader, text, text2, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x00002050 File Offset: 0x00000250
		public ToyotaUnitsDetector()
		{
		}

		// Token: 0x020009EB RID: 2539
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060051A6 RID: 20902 RVA: 0x003F2DC9 File Offset: 0x003F0FC9
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060051A7 RID: 20903 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060051A8 RID: 20904 RVA: 0x003F2DD5 File Offset: 0x003F0FD5
			internal bool <CheckAvailableCodings>b__0_0(CustomizableCodingTemplate x)
			{
				return x.PreWriteDataProcessor != null && x.PreWriteDataProcessor is ToyotaA8Item;
			}

			// Token: 0x060051A9 RID: 20905 RVA: 0x003F2DEF File Offset: 0x003F0FEF
			internal bool <CheckAvailableCodings>b__0_1(CustomizableCodingTemplate x)
			{
				return (x.PreWriteDataProcessor as ToyotaA8Item).CheckIfCodingIsSupported(x);
			}

			// Token: 0x04003184 RID: 12676
			public static readonly ToyotaUnitsDetector.<>c <>9 = new ToyotaUnitsDetector.<>c();

			// Token: 0x04003185 RID: 12677
			public static Func<CustomizableCodingTemplate, bool> <>9__0_0;

			// Token: 0x04003186 RID: 12678
			public static Func<CustomizableCodingTemplate, bool> <>9__0_1;
		}

		// Token: 0x020009EC RID: 2540
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060051AA RID: 20906 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x04003187 RID: 12679
			public IProgress<string> progress;

			// Token: 0x04003188 RID: 12680
			public List<CustomizableCodingTemplate> result;

			// Token: 0x04003189 RID: 12681
			public int progressCounter;

			// Token: 0x0400318A RID: 12682
			public List<KeyValuePair<string, string>> headers;

			// Token: 0x0400318B RID: 12683
			public List<ToyotaA8Item> A8items;
		}

		// Token: 0x020009ED RID: 2541
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_1
		{
			// Token: 0x060051AB RID: 20907 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_1()
			{
			}

			// Token: 0x060051AC RID: 20908 RVA: 0x003F2E04 File Offset: 0x003F1004
			internal void <CheckAvailableCodings>b__2(OBDRequest req, string response)
			{
				ToyotaUnitsDetector.<>c__DisplayClass0_2 CS$<>8__locals1 = new ToyotaUnitsDetector.<>c__DisplayClass0_2();
				CS$<>8__locals1.CS$<>8__locals2 = this;
				CS$<>8__locals1.req = req;
				IProgress<string> progress = this.CS$<>8__locals1.progress;
				if (progress != null)
				{
					progress.Report(string.Format("1/2 ({0}%)", this.CS$<>8__locals1.progressCounter * 100 / this.CS$<>8__locals1.headers.Count));
				}
				int progressCounter = this.CS$<>8__locals1.progressCounter;
				this.CS$<>8__locals1.progressCounter = progressCounter + 1;
				if (!response.Contains("7F3E") && !response.Contains("7E") && !response.Contains("7F 3E"))
				{
					this.CS$<>8__locals1.result.RemoveAll((CustomizableCodingTemplate x) => x.GetRequestHeaderForELM327() == CS$<>8__locals1.req.Header && x.ExtendedAddress == CS$<>8__locals1.CS$<>8__locals2.extAddr);
				}
			}

			// Token: 0x0400318C RID: 12684
			public string extAddr;

			// Token: 0x0400318D RID: 12685
			public ToyotaUnitsDetector.<>c__DisplayClass0_0 CS$<>8__locals1;
		}

		// Token: 0x020009EE RID: 2542
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_2
		{
			// Token: 0x060051AD RID: 20909 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_2()
			{
			}

			// Token: 0x060051AE RID: 20910 RVA: 0x003F2EC7 File Offset: 0x003F10C7
			internal bool <CheckAvailableCodings>b__3(CustomizableCodingTemplate x)
			{
				return x.GetRequestHeaderForELM327() == this.req.Header && x.ExtendedAddress == this.CS$<>8__locals2.extAddr;
			}

			// Token: 0x0400318E RID: 12686
			public OBDRequest req;

			// Token: 0x0400318F RID: 12687
			public ToyotaUnitsDetector.<>c__DisplayClass0_1 CS$<>8__locals2;
		}

		// Token: 0x020009EF RID: 2543
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_3
		{
			// Token: 0x060051AF RID: 20911 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_3()
			{
			}

			// Token: 0x060051B0 RID: 20912 RVA: 0x003F2EFC File Offset: 0x003F10FC
			internal void <CheckAvailableCodings>b__4(OBDRequest req, string response)
			{
				IProgress<string> progress = this.CS$<>8__locals3.progress;
				if (progress != null)
				{
					progress.Report(string.Format("2/2 ({0}%)", this.CS$<>8__locals3.progressCounter * 100 / (this.CS$<>8__locals3.headers.Count * 2)));
				}
				int progressCounter = this.CS$<>8__locals3.progressCounter;
				this.CS$<>8__locals3.progressCounter = progressCounter + 1;
				List<ToyotaA8Item> list = ToyotaA8Parser.ParseResponse(req, this.extAddr, response);
				this.CS$<>8__locals3.A8items.AddRange(list);
			}

			// Token: 0x04003190 RID: 12688
			public string extAddr;

			// Token: 0x04003191 RID: 12689
			public ToyotaUnitsDetector.<>c__DisplayClass0_0 CS$<>8__locals3;

			// Token: 0x04003192 RID: 12690
			public ResponseReceivedDelegate <>9__4;
		}

		// Token: 0x020009F0 RID: 2544
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_4
		{
			// Token: 0x060051B1 RID: 20913 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_4()
			{
			}

			// Token: 0x060051B2 RID: 20914 RVA: 0x003F2F88 File Offset: 0x003F1188
			internal bool <CheckAvailableCodings>b__5(ToyotaA8Item x)
			{
				return x.Type == ToyotaA8Item.ItemTypes.WriteA803 && x.CanIdHex == this.coding.GetRequestHeaderForELM327() && x.ExtendedAddress == this.coding.ExtendedAddress && x.IdHex == this.codingId;
			}

			// Token: 0x04003193 RID: 12691
			public CustomizableCodingTemplate coding;

			// Token: 0x04003194 RID: 12692
			public string codingId;
		}

		// Token: 0x020009F1 RID: 2545
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x060051B3 RID: 20915 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x04003195 RID: 12693
			public IProgress<string> progress;

			// Token: 0x04003196 RID: 12694
			public int progressCounter;

			// Token: 0x04003197 RID: 12695
			public List<KeyValuePair<string, string>> headers;

			// Token: 0x04003198 RID: 12696
			public List<ToyotaA8Item> A8items;
		}

		// Token: 0x020009F2 RID: 2546
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_1
		{
			// Token: 0x060051B4 RID: 20916 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_1()
			{
			}

			// Token: 0x060051B5 RID: 20917 RVA: 0x003F2FE4 File Offset: 0x003F11E4
			internal void <ReadAllUnits>b__0(OBDRequest req, string response)
			{
				IProgress<string> progress = this.CS$<>8__locals1.progress;
				if (progress != null)
				{
					progress.Report(string.Format("{0}%", this.CS$<>8__locals1.progressCounter * 100 / (this.CS$<>8__locals1.headers.Count * 3)));
				}
				int progressCounter = this.CS$<>8__locals1.progressCounter;
				this.CS$<>8__locals1.progressCounter = progressCounter + 1;
				List<ToyotaA8Item> list = ToyotaA8Parser.ParseResponse(req, this.extAddr, response);
				this.CS$<>8__locals1.A8items.AddRange(list);
			}

			// Token: 0x04003199 RID: 12697
			public string extAddr;

			// Token: 0x0400319A RID: 12698
			public ToyotaUnitsDetector.<>c__DisplayClass1_0 CS$<>8__locals1;

			// Token: 0x0400319B RID: 12699
			public ResponseReceivedDelegate <>9__0;
		}

		// Token: 0x020009F3 RID: 2547
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060051B6 RID: 20918 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060051B7 RID: 20919 RVA: 0x003F3070 File Offset: 0x003F1270
			internal bool <GetHeaders>b__0(KeyValuePair<string, string> x)
			{
				return x.Key == this.kvp.Key && x.Value == this.kvp.Value;
			}

			// Token: 0x0400319C RID: 12700
			public KeyValuePair<string, string> kvp;
		}

		// Token: 0x020009F4 RID: 2548
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckAvailableCodings>d__0 : IAsyncStateMachine
		{
			// Token: 0x060051B8 RID: 20920 RVA: 0x003F30A4 File Offset: 0x003F12A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ToyotaUnitsDetector toyotaUnitsDetector = this;
				List<CustomizableCodingTemplate> result;
				try
				{
					if (num > 1)
					{
						CS$<>8__locals1 = new ToyotaUnitsDetector.<>c__DisplayClass0_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.result = codings.ToList<CustomizableCodingTemplate>();
					}
					try
					{
						TaskAwaiter taskAwaiter;
						List<KeyValuePair<string, string>>.Enumerator enumerator;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_033B;
							}
							CS$<>8__locals1.headers = toyotaUnitsDetector.GetHeaders(codings);
							requests = new List<OBDRequest>(CS$<>8__locals1.headers.Count);
							CS$<>8__locals1.progressCounter = 1;
							enumerator = CS$<>8__locals1.headers.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<string, string> keyValuePair = enumerator.Current;
									ToyotaUnitsDetector.<>c__DisplayClass0_1 CS$<>8__locals2 = new ToyotaUnitsDetector.<>c__DisplayClass0_1();
									CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
									string key = keyValuePair.Key;
									CS$<>8__locals2.extAddr = keyValuePair.Value;
									OBDRequest obdrequestForUnit = ToyotaUnitsDetector.GetOBDRequestForUnit("3E", key, CS$<>8__locals2.extAddr);
									obdrequestForUnit.ResponseReceived += delegate(OBDRequest req, string response)
									{
										ToyotaUnitsDetector.<>c__DisplayClass0_2 CS$<>8__locals5 = new ToyotaUnitsDetector.<>c__DisplayClass0_2();
										CS$<>8__locals5.CS$<>8__locals2 = CS$<>8__locals2;
										CS$<>8__locals5.req = req;
										IProgress<string> progress = CS$<>8__locals2.CS$<>8__locals1.progress;
										if (progress != null)
										{
											progress.Report(string.Format("1/2 ({0}%)", CS$<>8__locals2.CS$<>8__locals1.progressCounter * 100 / CS$<>8__locals2.CS$<>8__locals1.headers.Count));
										}
										int progressCounter = CS$<>8__locals2.CS$<>8__locals1.progressCounter;
										CS$<>8__locals2.CS$<>8__locals1.progressCounter = progressCounter + 1;
										if (!response.Contains("7F3E") && !response.Contains("7E") && !response.Contains("7F 3E"))
										{
											CS$<>8__locals2.CS$<>8__locals1.result.RemoveAll((CustomizableCodingTemplate x) => x.GetRequestHeaderForELM327() == CS$<>8__locals5.req.Header && x.ExtendedAddress == CS$<>8__locals5.CS$<>8__locals2.extAddr);
										}
									};
									requests.Add(obdrequestForUnit);
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							App.OBDReader.ReplaceQueue(requests);
							taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ToyotaUnitsDetector.<CheckAvailableCodings>d__0>(ref taskAwaiter, ref this);
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
						CS$<>8__locals1.headers = toyotaUnitsDetector.GetHeaders(CS$<>8__locals1.result);
						requests.Clear();
						CS$<>8__locals1.A8items = new List<ToyotaA8Item>();
						CS$<>8__locals1.progressCounter = 1;
						enumerator = CS$<>8__locals1.headers.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair2 = enumerator.Current;
								try
								{
									ToyotaUnitsDetector.<>c__DisplayClass0_3 CS$<>8__locals3 = new ToyotaUnitsDetector.<>c__DisplayClass0_3();
									CS$<>8__locals3.CS$<>8__locals3 = CS$<>8__locals1;
									string key2 = keyValuePair2.Key;
									CS$<>8__locals3.extAddr = keyValuePair2.Value;
									string[] array = new string[] { "A801", "A803" };
									for (int i = 0; i < array.Length; i++)
									{
										OBDRequest obdrequestForUnit2 = ToyotaUnitsDetector.GetOBDRequestForUnit(array[i], key2, CS$<>8__locals3.extAddr);
										obdrequestForUnit2.ForceManualFlowControl = false;
										OBDRequest obdrequest = obdrequestForUnit2;
										ResponseReceivedDelegate responseReceivedDelegate;
										if ((responseReceivedDelegate = CS$<>8__locals3.<>9__4) == null)
										{
											responseReceivedDelegate = (CS$<>8__locals3.<>9__4 = delegate(OBDRequest req, string response)
											{
												IProgress<string> progress2 = CS$<>8__locals3.CS$<>8__locals3.progress;
												if (progress2 != null)
												{
													progress2.Report(string.Format("2/2 ({0}%)", CS$<>8__locals3.CS$<>8__locals3.progressCounter * 100 / (CS$<>8__locals3.CS$<>8__locals3.headers.Count * 2)));
												}
												int progressCounter2 = CS$<>8__locals3.CS$<>8__locals3.progressCounter;
												CS$<>8__locals3.CS$<>8__locals3.progressCounter = progressCounter2 + 1;
												List<ToyotaA8Item> list2 = ToyotaA8Parser.ParseResponse(req, CS$<>8__locals3.extAddr, response);
												CS$<>8__locals3.CS$<>8__locals3.A8items.AddRange(list2);
											});
										}
										obdrequest.ResponseReceived += responseReceivedDelegate;
										requests.Add(obdrequestForUnit2);
									}
								}
								catch (Exception)
								{
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
						App.OBDReader.ReplaceQueue(requests);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ToyotaUnitsDetector.<CheckAvailableCodings>d__0>(ref taskAwaiter, ref this);
							return;
						}
						IL_033B:
						taskAwaiter.GetResult();
						List<CustomizableCodingTemplate>.Enumerator enumerator2 = CS$<>8__locals1.result.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								ToyotaUnitsDetector.<>c__DisplayClass0_4 CS$<>8__locals4 = new ToyotaUnitsDetector.<>c__DisplayClass0_4();
								CS$<>8__locals4.coding = enumerator2.Current;
								try
								{
									CS$<>8__locals4.codingId = CS$<>8__locals4.coding.WriteModeAndAddress.Substring(2);
									ToyotaA8Item toyotaA8Item = CS$<>8__locals1.A8items.FirstOrDefault((ToyotaA8Item x) => x.Type == ToyotaA8Item.ItemTypes.WriteA803 && x.CanIdHex == CS$<>8__locals4.coding.GetRequestHeaderForELM327() && x.ExtendedAddress == CS$<>8__locals4.coding.ExtendedAddress && x.IdHex == CS$<>8__locals4.codingId);
									if (toyotaA8Item != null)
									{
										CS$<>8__locals4.coding.PreWriteDataProcessor = toyotaA8Item;
									}
								}
								catch (Exception)
								{
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
						List<CustomizableCodingTemplate> list = CS$<>8__locals1.result.Where((CustomizableCodingTemplate x) => x.PreWriteDataProcessor != null && x.PreWriteDataProcessor is ToyotaA8Item).ToList<CustomizableCodingTemplate>();
						CS$<>8__locals1.result = list.Where((CustomizableCodingTemplate x) => (x.PreWriteDataProcessor as ToyotaA8Item).CheckIfCodingIsSupported(x)).ToList<CustomizableCodingTemplate>();
						requests = null;
					}
					catch (Exception ex)
					{
						App.OBDReader.DebugWrite(ex.ToString());
					}
					result = CS$<>8__locals1.result;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x060051B9 RID: 20921 RVA: 0x003F3608 File Offset: 0x003F1808
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400319D RID: 12701
			public int <>1__state;

			// Token: 0x0400319E RID: 12702
			public AsyncTaskMethodBuilder<List<CustomizableCodingTemplate>> <>t__builder;

			// Token: 0x0400319F RID: 12703
			public IProgress<string> progress;

			// Token: 0x040031A0 RID: 12704
			public List<CustomizableCodingTemplate> codings;

			// Token: 0x040031A1 RID: 12705
			public ToyotaUnitsDetector <>4__this;

			// Token: 0x040031A2 RID: 12706
			private ToyotaUnitsDetector.<>c__DisplayClass0_0 <>8__1;

			// Token: 0x040031A3 RID: 12707
			private List<OBDRequest> <requests>5__2;

			// Token: 0x040031A4 RID: 12708
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020009F5 RID: 2549
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadAllUnits>d__1 : IAsyncStateMachine
		{
			// Token: 0x060051BA RID: 20922 RVA: 0x003F3618 File Offset: 0x003F1818
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						ToyotaUnitsDetector.<>c__DisplayClass1_0 CS$<>8__locals1 = new ToyotaUnitsDetector.<>c__DisplayClass1_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.headers = new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>("700", ""),
							new KeyValuePair<string, string>("750", "B5"),
							new KeyValuePair<string, string>("7C0", ""),
							new KeyValuePair<string, string>("750", "40"),
							new KeyValuePair<string, string>("7C4", ""),
							new KeyValuePair<string, string>("750", "67"),
							new KeyValuePair<string, string>("750", "B6"),
							new KeyValuePair<string, string>("750", "41"),
							new KeyValuePair<string, string>("750", "E0"),
							new KeyValuePair<string, string>("750", "90"),
							new KeyValuePair<string, string>("750", "91"),
							new KeyValuePair<string, string>("750", "92"),
							new KeyValuePair<string, string>("750", "93"),
							new KeyValuePair<string, string>("750", "B9"),
							new KeyValuePair<string, string>("750", "A1"),
							new KeyValuePair<string, string>("750", "A2"),
							new KeyValuePair<string, string>("750", "A4"),
							new KeyValuePair<string, string>("750", "A7"),
							new KeyValuePair<string, string>("750", "B8"),
							new KeyValuePair<string, string>("750", "AB"),
							new KeyValuePair<string, string>("750", "80"),
							new KeyValuePair<string, string>("750", "83"),
							new KeyValuePair<string, string>("750", "85"),
							new KeyValuePair<string, string>("750", "8A"),
							new KeyValuePair<string, string>("750", "8B"),
							new KeyValuePair<string, string>("750", "8C"),
							new KeyValuePair<string, string>("750", "8D"),
							new KeyValuePair<string, string>("750", "DC"),
							new KeyValuePair<string, string>("750", "C7"),
							new KeyValuePair<string, string>("750", "70"),
							new KeyValuePair<string, string>("750", "36"),
							new KeyValuePair<string, string>("750", "44"),
							new KeyValuePair<string, string>("791", ""),
							new KeyValuePair<string, string>("799", ""),
							new KeyValuePair<string, string>("781", ""),
							new KeyValuePair<string, string>("789", ""),
							new KeyValuePair<string, string>("750", "A8"),
							new KeyValuePair<string, string>("750", "4F"),
							new KeyValuePair<string, string>("750", "B0"),
							new KeyValuePair<string, string>("750", "AE"),
							new KeyValuePair<string, string>("750", "69"),
							new KeyValuePair<string, string>("750", "D3"),
							new KeyValuePair<string, string>("750", "F3"),
							new KeyValuePair<string, string>("7B0", ""),
							new KeyValuePair<string, string>("7E0", ""),
							new KeyValuePair<string, string>("750", "2A")
						};
						List<OBDRequest> list = new List<OBDRequest>(CS$<>8__locals1.headers.Count * 3);
						CS$<>8__locals1.A8items = new List<ToyotaA8Item>();
						CS$<>8__locals1.progressCounter = 1;
						List<KeyValuePair<string, string>>.Enumerator enumerator = CS$<>8__locals1.headers.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair = enumerator.Current;
								try
								{
									ToyotaUnitsDetector.<>c__DisplayClass1_1 CS$<>8__locals2 = new ToyotaUnitsDetector.<>c__DisplayClass1_1();
									CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
									string key = keyValuePair.Key;
									CS$<>8__locals2.extAddr = keyValuePair.Value;
									string[] array = new string[] { "A801", "A803", "222000" };
									for (int i = 0; i < array.Length; i++)
									{
										OBDRequest obdrequestForUnit = ToyotaUnitsDetector.GetOBDRequestForUnit(array[i], key, CS$<>8__locals2.extAddr);
										obdrequestForUnit.ForceManualFlowControl = false;
										OBDRequest obdrequest = obdrequestForUnit;
										ResponseReceivedDelegate responseReceivedDelegate;
										if ((responseReceivedDelegate = CS$<>8__locals2.<>9__0) == null)
										{
											responseReceivedDelegate = (CS$<>8__locals2.<>9__0 = delegate(OBDRequest req, string response)
											{
												IProgress<string> progress = CS$<>8__locals2.CS$<>8__locals1.progress;
												if (progress != null)
												{
													progress.Report(string.Format("{0}%", CS$<>8__locals2.CS$<>8__locals1.progressCounter * 100 / (CS$<>8__locals2.CS$<>8__locals1.headers.Count * 3)));
												}
												int progressCounter = CS$<>8__locals2.CS$<>8__locals1.progressCounter;
												CS$<>8__locals2.CS$<>8__locals1.progressCounter = progressCounter + 1;
												List<ToyotaA8Item> list2 = ToyotaA8Parser.ParseResponse(req, CS$<>8__locals2.extAddr, response);
												CS$<>8__locals2.CS$<>8__locals1.A8items.AddRange(list2);
											});
										}
										obdrequest.ResponseReceived += responseReceivedDelegate;
										list.Add(obdrequestForUnit);
									}
								}
								catch (Exception)
								{
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
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ToyotaUnitsDetector.<ReadAllUnits>d__1>(ref taskAwaiter, ref this);
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
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060051BB RID: 20923 RVA: 0x003F3C08 File Offset: 0x003F1E08
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040031A5 RID: 12709
			public int <>1__state;

			// Token: 0x040031A6 RID: 12710
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040031A7 RID: 12711
			public IProgress<string> progress;

			// Token: 0x040031A8 RID: 12712
			private TaskAwaiter <>u__1;
		}
	}
}
