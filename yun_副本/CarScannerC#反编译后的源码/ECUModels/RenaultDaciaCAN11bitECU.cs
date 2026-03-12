using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB.Renault;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x02000509 RID: 1289
	internal class RenaultDaciaCAN11bitECU : CAN11bitECU
	{
		// Token: 0x0600318D RID: 12685 RVA: 0x00223364 File Offset: 0x00221564
		public RenaultDaciaCAN11bitECU(string name, string request, string response)
		{
			this.Name = name + " (11 bit)";
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "10C0" };
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC", "17FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00" };
			base.CloseSessionCommands = new List<string>();
			base.IdentsASCII.TryAdd("22F18A", "Supplier code");
			base.IdentsASCII.TryAdd("22F194", "Soft version");
			base.IdentsASCII.TryAdd("22F195", "Version");
			base.IdentsASCII.TryAdd("22F1A0", "Diag. ver.");
			base.IdentsHEX.TryAdd("2180", "ECU Ident");
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x00223488 File Offset: 0x00221688
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("7E0", "7E8", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string, string>("7E1", "7E9", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string, string>("740", "760", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string, string>("752", "772", "SRS/Airbags"),
				new ValueTuple<string, string, string>("742", "762", "Power steering"),
				new ValueTuple<string, string, string>("743", "763", "Dashboard/Instrument cluster"),
				new ValueTuple<string, string, string>("744", "764", "Heater & air conditioning"),
				new ValueTuple<string, string, string>("745", "765", "BCM"),
				new ValueTuple<string, string, string>("748", "768", "All wheel drive system (AWD/4WD)"),
				new ValueTuple<string, string, string>("74D", "76D", "EMM"),
				new ValueTuple<string, string, string>("74E", "76E", "Sonar"),
				new ValueTuple<string, string, string>("710", "730", "CAN network gateway"),
				new ValueTuple<string, string, string>("755", "775", "Hill start assist"),
				new ValueTuple<string, string, string>("799", "7B9", "Key hands free"),
				new ValueTuple<string, string, string>("747", "767", "Multimedia system"),
				new ValueTuple<string, string, string>("7CA", "7DA", "Emergency Call and Communication Module"),
				new ValueTuple<string, string, string>("75D", "77D", "Front radar"),
				new ValueTuple<string, string, string>("756", "776", "DC-DC"),
				new ValueTuple<string, string, string>("712", "732", "Radio"),
				new ValueTuple<string, string, string>("79B", "7BB", "BMS"),
				new ValueTuple<string, string, string>("7E4", "7EC", "EVC"),
				new ValueTuple<string, string, string>("75A", "77E", "PEB"),
				new ValueTuple<string, string, string>("796", "7B6", "LBC2"),
				new ValueTuple<string, string, string>("758", "778", "TPMS")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902A7", "1902AC", "17FF00" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				RenaultDaciaCAN11bitECU renaultDaciaCAN11bitECU = new RenaultDaciaCAN11bitECU(valueTuple.Item3, item, item2);
				list.Add(renaultDaciaCAN11bitECU);
				if (item == "740")
				{
					renaultDaciaCAN11bitECU.CloseSessionCommands = new List<string>(1) { "1081" };
					renaultDaciaCAN11bitECU.ClearDTCCommands.Insert(0, "14FFFFFF");
					renaultDaciaCAN11bitECU.ClearDTCCommands.Add("14FF00");
				}
			}
			return list;
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x00223876 File Offset: 0x00221A76
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return RenaultDaciaCAN11bitECU.BuildList();
			}
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x00223880 File Offset: 0x00221A80
		public override OBDRequest GetRequestForCommand(string cmd)
		{
			OBDRequest requestForCommand = base.GetRequestForCommand(cmd);
			if (requestForCommand.Header == "740")
			{
				List<string> list = requestForCommand.BeforeCommands.ToList<string>();
				List<string> list2 = requestForCommand.AfterCommands.ToList<string>();
				list.Add("ATSTFF");
				list2.Add("ATST" + SharedSettings.Current.GetATST());
				requestForCommand.BeforeCommands = list.ToArray();
				requestForCommand.AfterCommands = list2.ToArray();
			}
			return requestForCommand;
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x00223900 File Offset: 0x00221B00
		public override async Task<string> GetECUInformationReportAsync(CancellationToken token)
		{
			this.cancellationToken = token;
			StringBuilder sb = new StringBuilder(8);
			List<OBDRequest> list = new List<OBDRequest>();
			list.AddRange(this.GetTestECUExistsRequest());
			OBDRequest requestForCommand = this.GetRequestForCommand("2180");
			requestForCommand.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					RenaultECUIdents renaultECUIdents = RenaultECUIdents.From2180(request.Header, data);
					if (renaultECUIdents != null && !renaultECUIdents.IsEmpty)
					{
						string text = renaultECUIdents.supplier;
						byte[] array = BitHelpers.ConvertHexToBytesX(text);
						string text2 = this.DecodeAsASCII(request.Command, array, true);
						if (text2.Length == 3)
						{
							text = text2 + " (" + text + ")";
						}
						sb.AppendLine("Software: " + renaultECUIdents.soft);
						sb.AppendLine("Diag. version: " + renaultECUIdents.diagversion.ToString());
						sb.AppendLine("Supplier: " + text);
						sb.AppendLine("Version: " + renaultECUIdents.version);
						App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Command == "22F18A" || x.Command == "22F194" || x.Command == "22F195" || x.Command == "22F1A0");
					}
				}
			};
			list.Add(requestForCommand);
			foreach (KeyValuePair<string, string> keyValuePair in base.IdentsASCII)
			{
				string key = keyValuePair.Key;
				string requestTitle2 = keyValuePair.Value;
				OBDRequest requestForCommand2 = this.GetRequestForCommand(key);
				requestForCommand2.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
				{
					string text3 = this.DecodeAsASCII(decodedRequest.Command, data, true);
					if (!string.IsNullOrEmpty(text3))
					{
						sb.AppendLine(requestTitle2 + ": " + text3);
					}
				};
				list.Add(requestForCommand2);
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in base.IdentsHEX)
			{
				string key2 = keyValuePair2.Key;
				string requestTitle = keyValuePair2.Value;
				OBDRequest requestForCommand3 = this.GetRequestForCommand(key2);
				requestForCommand3.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
				{
					string text4 = this.DecodeAsHEX(decodedRequest.Command, data);
					if (!string.IsNullOrEmpty(text4))
					{
						sb.AppendLine(requestTitle + ": " + text4);
					}
				};
				list.Add(requestForCommand3);
			}
			foreach (OBDRequest obdrequest in list)
			{
				obdrequest.CheckLength = true;
				obdrequest.ELMFormat = ELMFormat.CAN11bit;
			}
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			return sb.ToString();
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x0022394C File Offset: 0x00221B4C
		public override async Task ClearDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken token)
		{
			this.cancellationToken = token;
			OBDRequest[] array = this.GetDTCClearRequests();
			foreach (OBDRequest obdrequest in array)
			{
				if (obdrequest.Command != null && obdrequest.Command == "14FFFFFF")
				{
					obdrequest.ResponseReceived += this.DtcReadOrClearRequest_ResponseReceived;
				}
			}
			if (this.TestELMDevice)
			{
				OBDRequest atshtestRequest = this.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
				OBDRequest atfctestRequest = this.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
				array = new OBDRequest[] { atshtestRequest, atfctestRequest }.Concat(array).ToArray<OBDRequest>();
			}
			App.OBDReader.ReplaceQueue(array);
			await App.OBDReader.WaitForCommandQueue();
			this.cancellationToken = CancellationToken.None;
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x002239A8 File Offset: 0x00221BA8
		public override async Task ReadDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken token)
		{
			this.cancellationToken = token;
			OBDRequest[] array = this.GetDTCReadRequests();
			foreach (OBDRequest obdrequest in array)
			{
				if (obdrequest.Command != null && obdrequest.Command.StartsWith("19"))
				{
					obdrequest.ResponseReceived += this.DtcReadOrClearRequest_ResponseReceived;
				}
			}
			if (this.TestELMDevice)
			{
				OBDRequest atshtestRequest = this.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
				OBDRequest atfctestRequest = this.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
				array = new OBDRequest[] { atshtestRequest, atfctestRequest }.Concat(array).ToArray<OBDRequest>();
			}
			App.OBDReader.ReplaceQueue(array);
			await App.OBDReader.WaitForCommandQueue();
			this.cancellationToken = CancellationToken.None;
			await this.RequestFreezeFrames();
			if (!SharedSettings.Current.UseDefaultInit)
			{
				OBDRequest pingRequest = App.OBDReader.GetPingRequest();
				App.OBDReader.ReplaceQueue(pingRequest);
				await App.OBDReader.WaitForCommandQueue();
			}
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x00223A04 File Offset: 0x00221C04
		private void DtcReadOrClearRequest_ResponseReceived(OBDRequest request, string data)
		{
			if (data == null)
			{
				return;
			}
			if (data.Contains("NO DATA"))
			{
				return;
			}
			if (request.Command == null)
			{
				return;
			}
			if (request.Command.Length < 2)
			{
				return;
			}
			string text = request.Command.Substring(0, 2);
			if (data.Contains("037F" + text + "7F"))
			{
				if (request.BeforeCommands.Any((string x) => x == "10C0"))
				{
					for (int i = 0; i < request.BeforeCommands.Length; i++)
					{
						if (request.BeforeCommands[i] == "10C0")
						{
							request.BeforeCommands[i] = "1003";
						}
					}
					request.BeforeCommands = request.BeforeCommands.Distinct<string>().ToArray<string>();
					App.OBDReader.InsertRequestInQueue(request);
				}
			}
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x00223AE8 File Offset: 0x00221CE8
		protected override void DtcReadRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				this.ECUExists = true;
				List<DTCItemV2> dtcs = DTCDecoder.DecodeData(request, request.Header, data);
				if (base.RemoveOtherRequestsIfUDSReadResponded && !string.IsNullOrEmpty(request.Header) && dtcs.Count > 0 && request.Command.StartsWith("19"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.Header == request.Header && !request.Command.StartsWith("19"));
					App.OBDReader.ReplaceQueue(queueCopy);
				}
				Device.BeginInvokeOnMainThread(delegate
				{
					bool flag = false;
					using (List<DTCItemV2>.Enumerator enumerator = dtcs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DTCItemV2 dtc = enumerator.Current;
							if (dtc != null && !this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == dtc.Code) && (!(this.RequestHeader == "740") || !(dtc.RawCode == "505004")))
							{
								dtc.LoadDescription();
								dtc.ECU = this.Name;
								this.DTCCollection.Add(dtc);
								DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
								if (recorder != null)
								{
									recorder.Record(dtc);
								}
								flag = true;
							}
						}
					}
					if (flag)
					{
						this.UpdateCollection();
					}
				});
			}
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x00223BE4 File Offset: 0x00221DE4
		public async Task RequestFreezeFrames()
		{
			if (base.Count != 0)
			{
				List<OBDRequest> list = new List<OBDRequest>();
				using (IEnumerator<DTCItemV2> enumerator = base.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DTCItemV2 dtc = enumerator.Current;
						if (dtc.RawCode.Length == 6)
						{
							dtc.Payload = "";
							OBDRequest requestForCommand = this.GetRequestForCommand("1906" + dtc.RawCode + "80");
							OBDRequest requestForCommand2 = this.GetRequestForCommand("1906" + dtc.RawCode + "81");
							OBDRequest requestForCommand3 = this.GetRequestForCommand("1906" + dtc.RawCode + "82");
							ResponseDecodedDelegate responseDecodedDelegate = delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
							{
								if (data != null && data.Length >= 2)
								{
									switch (data[0])
									{
									case 128:
										if (data.Length >= 4)
										{
											int mileage = (int)data[1] * 256 * 256 + (int)data[2] * 256 + (int)data[3];
											MainThreadHelper.InvokeOnMainThread(delegate
											{
												if (dtc.Payload != "")
												{
													dtc.Payload += "\n";
												}
												dtc.Payload = string.Concat(new string[]
												{
													dtc.Payload,
													"Odometer: ",
													UnitsHelper.GetValue((double)mileage, UnitsHelper.Units.km).ToString(),
													" ",
													UnitsHelper.GetCaption(UnitsHelper.Units.km)
												});
												DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
												if (recorder == null)
												{
													return;
												}
												recorder.Record(dtc);
											});
											return;
										}
										break;
									case 129:
										if (data.Length >= 2)
										{
											MainThreadHelper.InvokeOnMainThread(delegate
											{
												if (dtc.Payload != "")
												{
													dtc.Payload += "\n";
												}
												dtc.Payload = dtc.Payload + "Aging counter: " + data[1].ToString();
												DataRecorderV2 recorder2 = App.OBDReader.CurrentCarData.Recorder;
												if (recorder2 == null)
												{
													return;
												}
												recorder2.Record(dtc);
											});
											return;
										}
										break;
									case 130:
										if (data.Length >= 2)
										{
											MainThreadHelper.InvokeOnMainThread(delegate
											{
												if (dtc.Payload != "")
												{
													dtc.Payload += "\n";
												}
												dtc.Payload = dtc.Payload + "Occurrence counter: " + data[1].ToString();
												DataRecorderV2 recorder3 = App.OBDReader.CurrentCarData.Recorder;
												if (recorder3 == null)
												{
													return;
												}
												recorder3.Record(dtc);
											});
										}
										break;
									default:
										return;
									}
								}
							};
							requestForCommand.ResponseDecoded += responseDecodedDelegate;
							requestForCommand2.ResponseDecoded += responseDecodedDelegate;
							requestForCommand3.ResponseDecoded += responseDecodedDelegate;
							requestForCommand.ResponseReceived += base.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
							requestForCommand2.ResponseReceived += base.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
							requestForCommand3.ResponseReceived += base.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
							list.Add(requestForCommand);
							list.Add(requestForCommand2);
							list.Add(requestForCommand3);
						}
					}
				}
				if (list.Count != 0)
				{
					App.OBDReader.ReplaceQueue(list);
					await App.OBDReader.WaitForCommandQueue();
				}
			}
		}

		// Token: 0x0200050A RID: 1290
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003197 RID: 12695 RVA: 0x00223C27 File Offset: 0x00221E27
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003198 RID: 12696 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003199 RID: 12697 RVA: 0x00223C34 File Offset: 0x00221E34
			internal bool <GetECUInformationReportAsync>b__5_1(OBDRequest x)
			{
				return x.Command == "22F18A" || x.Command == "22F194" || x.Command == "22F195" || x.Command == "22F1A0";
			}

			// Token: 0x0600319A RID: 12698 RVA: 0x00223C89 File Offset: 0x00221E89
			internal bool <DtcReadOrClearRequest_ResponseReceived>b__8_0(string x)
			{
				return x == "10C0";
			}

			// Token: 0x04001C9C RID: 7324
			public static readonly RenaultDaciaCAN11bitECU.<>c <>9 = new RenaultDaciaCAN11bitECU.<>c();

			// Token: 0x04001C9D RID: 7325
			public static Predicate<OBDRequest> <>9__5_1;

			// Token: 0x04001C9E RID: 7326
			public static Func<string, bool> <>9__8_0;
		}

		// Token: 0x0200050B RID: 1291
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_0
		{
			// Token: 0x0600319B RID: 12699 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_0()
			{
			}

			// Token: 0x0600319C RID: 12700 RVA: 0x00223C98 File Offset: 0x00221E98
			internal void <RequestFreezeFrames>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				RenaultDaciaCAN11bitECU.<>c__DisplayClass10_1 CS$<>8__locals1 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass10_1();
				CS$<>8__locals1.CS$<>8__locals1 = this;
				CS$<>8__locals1.data = data;
				if (CS$<>8__locals1.data != null && CS$<>8__locals1.data.Length >= 2)
				{
					switch (CS$<>8__locals1.data[0])
					{
					case 128:
						if (CS$<>8__locals1.data.Length >= 4)
						{
							RenaultDaciaCAN11bitECU.<>c__DisplayClass10_2 CS$<>8__locals2 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass10_2();
							CS$<>8__locals2.CS$<>8__locals2 = CS$<>8__locals1;
							CS$<>8__locals2.mileage = (int)CS$<>8__locals2.CS$<>8__locals2.data[1] * 256 * 256 + (int)CS$<>8__locals2.CS$<>8__locals2.data[2] * 256 + (int)CS$<>8__locals2.CS$<>8__locals2.data[3];
							MainThreadHelper.InvokeOnMainThread(delegate
							{
								if (CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload != "")
								{
									CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload + "\n";
								}
								CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = string.Concat(new string[]
								{
									CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload,
									"Odometer: ",
									UnitsHelper.GetValue((double)CS$<>8__locals2.mileage, UnitsHelper.Units.km).ToString(),
									" ",
									UnitsHelper.GetCaption(UnitsHelper.Units.km)
								});
								DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
								if (recorder == null)
								{
									return;
								}
								recorder.Record(CS$<>8__locals2.CS$<>8__locals2.CS$<>8__locals1.dtc);
							});
							return;
						}
						break;
					case 129:
						if (CS$<>8__locals1.data.Length >= 2)
						{
							MainThreadHelper.InvokeOnMainThread(delegate
							{
								if (CS$<>8__locals1.CS$<>8__locals1.dtc.Payload != "")
								{
									CS$<>8__locals1.CS$<>8__locals1.dtc.Payload = CS$<>8__locals1.CS$<>8__locals1.dtc.Payload + "\n";
								}
								CS$<>8__locals1.CS$<>8__locals1.dtc.Payload = CS$<>8__locals1.CS$<>8__locals1.dtc.Payload + "Aging counter: " + CS$<>8__locals1.data[1].ToString();
								DataRecorderV2 recorder2 = App.OBDReader.CurrentCarData.Recorder;
								if (recorder2 == null)
								{
									return;
								}
								recorder2.Record(CS$<>8__locals1.CS$<>8__locals1.dtc);
							});
							return;
						}
						break;
					case 130:
						if (CS$<>8__locals1.data.Length >= 2)
						{
							MainThreadHelper.InvokeOnMainThread(delegate
							{
								if (CS$<>8__locals1.CS$<>8__locals1.dtc.Payload != "")
								{
									CS$<>8__locals1.CS$<>8__locals1.dtc.Payload = CS$<>8__locals1.CS$<>8__locals1.dtc.Payload + "\n";
								}
								CS$<>8__locals1.CS$<>8__locals1.dtc.Payload = CS$<>8__locals1.CS$<>8__locals1.dtc.Payload + "Occurrence counter: " + CS$<>8__locals1.data[1].ToString();
								DataRecorderV2 recorder3 = App.OBDReader.CurrentCarData.Recorder;
								if (recorder3 == null)
								{
									return;
								}
								recorder3.Record(CS$<>8__locals1.CS$<>8__locals1.dtc);
							});
						}
						break;
					default:
						return;
					}
				}
			}

			// Token: 0x04001C9F RID: 7327
			public DTCItemV2 dtc;
		}

		// Token: 0x0200050C RID: 1292
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_1
		{
			// Token: 0x0600319D RID: 12701 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_1()
			{
			}

			// Token: 0x0600319E RID: 12702 RVA: 0x00223D9C File Offset: 0x00221F9C
			internal void <RequestFreezeFrames>b__1()
			{
				if (this.CS$<>8__locals1.dtc.Payload != "")
				{
					this.CS$<>8__locals1.dtc.Payload = this.CS$<>8__locals1.dtc.Payload + "\n";
				}
				this.CS$<>8__locals1.dtc.Payload = this.CS$<>8__locals1.dtc.Payload + "Aging counter: " + this.data[1].ToString();
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				if (recorder == null)
				{
					return;
				}
				recorder.Record(this.CS$<>8__locals1.dtc);
			}

			// Token: 0x0600319F RID: 12703 RVA: 0x00223E50 File Offset: 0x00222050
			internal void <RequestFreezeFrames>b__2()
			{
				if (this.CS$<>8__locals1.dtc.Payload != "")
				{
					this.CS$<>8__locals1.dtc.Payload = this.CS$<>8__locals1.dtc.Payload + "\n";
				}
				this.CS$<>8__locals1.dtc.Payload = this.CS$<>8__locals1.dtc.Payload + "Occurrence counter: " + this.data[1].ToString();
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				if (recorder == null)
				{
					return;
				}
				recorder.Record(this.CS$<>8__locals1.dtc);
			}

			// Token: 0x04001CA0 RID: 7328
			public byte[] data;

			// Token: 0x04001CA1 RID: 7329
			public RenaultDaciaCAN11bitECU.<>c__DisplayClass10_0 CS$<>8__locals1;
		}

		// Token: 0x0200050D RID: 1293
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_2
		{
			// Token: 0x060031A0 RID: 12704 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_2()
			{
			}

			// Token: 0x060031A1 RID: 12705 RVA: 0x00223F04 File Offset: 0x00222104
			internal void <RequestFreezeFrames>b__3()
			{
				if (this.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload != "")
				{
					this.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = this.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload + "\n";
				}
				this.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = string.Concat(new string[]
				{
					this.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload,
					"Odometer: ",
					UnitsHelper.GetValue((double)this.mileage, UnitsHelper.Units.km).ToString(),
					" ",
					UnitsHelper.GetCaption(UnitsHelper.Units.km)
				});
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				if (recorder == null)
				{
					return;
				}
				recorder.Record(this.CS$<>8__locals2.CS$<>8__locals1.dtc);
			}

			// Token: 0x04001CA2 RID: 7330
			public int mileage;

			// Token: 0x04001CA3 RID: 7331
			public RenaultDaciaCAN11bitECU.<>c__DisplayClass10_1 CS$<>8__locals2;
		}

		// Token: 0x0200050E RID: 1294
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x060031A2 RID: 12706 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x060031A3 RID: 12707 RVA: 0x00223FF8 File Offset: 0x002221F8
			internal void <GetECUInformationReportAsync>b__0(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					RenaultECUIdents renaultECUIdents = RenaultECUIdents.From2180(request.Header, data);
					if (renaultECUIdents != null && !renaultECUIdents.IsEmpty)
					{
						string text = renaultECUIdents.supplier;
						byte[] array = BitHelpers.ConvertHexToBytesX(text);
						string text2 = this.<>4__this.DecodeAsASCII(request.Command, array, true);
						if (text2.Length == 3)
						{
							text = text2 + " (" + text + ")";
						}
						this.sb.AppendLine("Software: " + renaultECUIdents.soft);
						this.sb.AppendLine("Diag. version: " + renaultECUIdents.diagversion.ToString());
						this.sb.AppendLine("Supplier: " + text);
						this.sb.AppendLine("Version: " + renaultECUIdents.version);
						App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Command == "22F18A" || x.Command == "22F194" || x.Command == "22F195" || x.Command == "22F1A0");
					}
				}
			}

			// Token: 0x04001CA4 RID: 7332
			public RenaultDaciaCAN11bitECU <>4__this;

			// Token: 0x04001CA5 RID: 7333
			public StringBuilder sb;
		}

		// Token: 0x0200050F RID: 1295
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_1
		{
			// Token: 0x060031A4 RID: 12708 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_1()
			{
			}

			// Token: 0x060031A5 RID: 12709 RVA: 0x0022410C File Offset: 0x0022230C
			internal void <GetECUInformationReportAsync>b__2(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
			{
				string text = this.CS$<>8__locals1.<>4__this.DecodeAsASCII(decodedRequest.Command, data, true);
				if (!string.IsNullOrEmpty(text))
				{
					this.CS$<>8__locals1.sb.AppendLine(this.requestTitle + ": " + text);
				}
			}

			// Token: 0x04001CA6 RID: 7334
			public string requestTitle;

			// Token: 0x04001CA7 RID: 7335
			public RenaultDaciaCAN11bitECU.<>c__DisplayClass5_0 CS$<>8__locals1;
		}

		// Token: 0x02000510 RID: 1296
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_2
		{
			// Token: 0x060031A6 RID: 12710 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_2()
			{
			}

			// Token: 0x060031A7 RID: 12711 RVA: 0x0022415C File Offset: 0x0022235C
			internal void <GetECUInformationReportAsync>b__3(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
			{
				string text = this.CS$<>8__locals2.<>4__this.DecodeAsHEX(decodedRequest.Command, data);
				if (!string.IsNullOrEmpty(text))
				{
					this.CS$<>8__locals2.sb.AppendLine(this.requestTitle + ": " + text);
				}
			}

			// Token: 0x04001CA8 RID: 7336
			public string requestTitle;

			// Token: 0x04001CA9 RID: 7337
			public RenaultDaciaCAN11bitECU.<>c__DisplayClass5_0 CS$<>8__locals2;
		}

		// Token: 0x02000511 RID: 1297
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x060031A8 RID: 12712 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x060031A9 RID: 12713 RVA: 0x002241AB File Offset: 0x002223AB
			internal bool <DtcReadRequest_ResponseDecoded>b__1(OBDRequest x)
			{
				return x.Header == this.request.Header && !this.request.Command.StartsWith("19");
			}

			// Token: 0x04001CAA RID: 7338
			public OBDRequest request;

			// Token: 0x04001CAB RID: 7339
			public RenaultDaciaCAN11bitECU <>4__this;

			// Token: 0x04001CAC RID: 7340
			public string responseHeader;
		}

		// Token: 0x02000512 RID: 1298
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_1
		{
			// Token: 0x060031AA RID: 12714 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_1()
			{
			}

			// Token: 0x060031AB RID: 12715 RVA: 0x002241E0 File Offset: 0x002223E0
			internal void <DtcReadRequest_ResponseDecoded>b__0()
			{
				bool flag = false;
				using (List<DTCItemV2>.Enumerator enumerator = this.dtcs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RenaultDaciaCAN11bitECU.<>c__DisplayClass9_2 CS$<>8__locals1 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass9_2();
						CS$<>8__locals1.dtc = enumerator.Current;
						if (CS$<>8__locals1.dtc != null && !this.CS$<>8__locals1.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals1.dtc.Code) && (!(this.CS$<>8__locals1.<>4__this.RequestHeader == "740") || !(CS$<>8__locals1.dtc.RawCode == "505004")))
						{
							CS$<>8__locals1.dtc.LoadDescription();
							CS$<>8__locals1.dtc.ECU = this.CS$<>8__locals1.<>4__this.Name;
							this.CS$<>8__locals1.<>4__this.DTCCollection.Add(CS$<>8__locals1.dtc);
							DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
							if (recorder != null)
							{
								recorder.Record(CS$<>8__locals1.dtc);
							}
							flag = true;
						}
					}
				}
				if (flag)
				{
					this.CS$<>8__locals1.<>4__this.UpdateCollection();
				}
			}

			// Token: 0x04001CAD RID: 7341
			public List<DTCItemV2> dtcs;

			// Token: 0x04001CAE RID: 7342
			public RenaultDaciaCAN11bitECU.<>c__DisplayClass9_0 CS$<>8__locals1;
		}

		// Token: 0x02000513 RID: 1299
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_2
		{
			// Token: 0x060031AC RID: 12716 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_2()
			{
			}

			// Token: 0x060031AD RID: 12717 RVA: 0x00224320 File Offset: 0x00222520
			internal bool <DtcReadRequest_ResponseDecoded>b__2(DTCItemV2 x)
			{
				return x.Code == this.dtc.Code;
			}

			// Token: 0x04001CAF RID: 7343
			public DTCItemV2 dtc;
		}

		// Token: 0x02000514 RID: 1300
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearDTCAsync>d__6 : IAsyncStateMachine
		{
			// Token: 0x060031AE RID: 12718 RVA: 0x00224338 File Offset: 0x00222538
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RenaultDaciaCAN11bitECU renaultDaciaCAN11bitECU = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						renaultDaciaCAN11bitECU.cancellationToken = token;
						OBDRequest[] array = renaultDaciaCAN11bitECU.GetDTCClearRequests();
						foreach (OBDRequest obdrequest in array)
						{
							if (obdrequest.Command != null && obdrequest.Command == "14FFFFFF")
							{
								obdrequest.ResponseReceived += renaultDaciaCAN11bitECU.DtcReadOrClearRequest_ResponseReceived;
							}
						}
						if (renaultDaciaCAN11bitECU.TestELMDevice)
						{
							OBDRequest atshtestRequest = renaultDaciaCAN11bitECU.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
							OBDRequest atfctestRequest = renaultDaciaCAN11bitECU.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
							array = new OBDRequest[] { atshtestRequest, atfctestRequest }.Concat(array).ToArray<OBDRequest>();
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultDaciaCAN11bitECU.<ClearDTCAsync>d__6>(ref taskAwaiter, ref this);
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
					renaultDaciaCAN11bitECU.cancellationToken = CancellationToken.None;
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

			// Token: 0x060031AF RID: 12719 RVA: 0x002244C0 File Offset: 0x002226C0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001CB0 RID: 7344
			public int <>1__state;

			// Token: 0x04001CB1 RID: 7345
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001CB2 RID: 7346
			public RenaultDaciaCAN11bitECU <>4__this;

			// Token: 0x04001CB3 RID: 7347
			public CancellationToken token;

			// Token: 0x04001CB4 RID: 7348
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001CB5 RID: 7349
			public Action badELMDetectedCallback;

			// Token: 0x04001CB6 RID: 7350
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000515 RID: 1301
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync>d__5 : IAsyncStateMachine
		{
			// Token: 0x060031B0 RID: 12720 RVA: 0x002244D0 File Offset: 0x002226D0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RenaultDaciaCAN11bitECU renaultDaciaCAN11bitECU = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass5_0();
						CS$<>8__locals1.<>4__this = this;
						renaultDaciaCAN11bitECU.cancellationToken = token;
						CS$<>8__locals1.sb = new StringBuilder(8);
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(renaultDaciaCAN11bitECU.GetTestECUExistsRequest());
						OBDRequest requestForCommand = renaultDaciaCAN11bitECU.GetRequestForCommand("2180");
						requestForCommand.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
						{
							if (data != null && data.Length != 0)
							{
								RenaultECUIdents renaultECUIdents = RenaultECUIdents.From2180(request.Header, data);
								if (renaultECUIdents != null && !renaultECUIdents.IsEmpty)
								{
									string text2 = renaultECUIdents.supplier;
									byte[] array = BitHelpers.ConvertHexToBytesX(text2);
									string text3 = CS$<>8__locals1.<>4__this.DecodeAsASCII(request.Command, array, true);
									if (text3.Length == 3)
									{
										text2 = text3 + " (" + text2 + ")";
									}
									CS$<>8__locals1.sb.AppendLine("Software: " + renaultECUIdents.soft);
									CS$<>8__locals1.sb.AppendLine("Diag. version: " + renaultECUIdents.diagversion.ToString());
									CS$<>8__locals1.sb.AppendLine("Supplier: " + text2);
									CS$<>8__locals1.sb.AppendLine("Version: " + renaultECUIdents.version);
									App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Command == "22F18A" || x.Command == "22F194" || x.Command == "22F195" || x.Command == "22F1A0");
								}
							}
						};
						list.Add(requestForCommand);
						Dictionary<string, string>.Enumerator enumerator = renaultDaciaCAN11bitECU.IdentsASCII.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair = enumerator.Current;
								RenaultDaciaCAN11bitECU.<>c__DisplayClass5_1 CS$<>8__locals2 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass5_1();
								CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
								string key = keyValuePair.Key;
								CS$<>8__locals2.requestTitle = keyValuePair.Value;
								OBDRequest requestForCommand2 = renaultDaciaCAN11bitECU.GetRequestForCommand(key);
								requestForCommand2.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
								{
									string text4 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.DecodeAsASCII(decodedRequest.Command, data, true);
									if (!string.IsNullOrEmpty(text4))
									{
										CS$<>8__locals2.CS$<>8__locals1.sb.AppendLine(CS$<>8__locals2.requestTitle + ": " + text4);
									}
								};
								list.Add(requestForCommand2);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = renaultDaciaCAN11bitECU.IdentsHEX.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair2 = enumerator.Current;
								RenaultDaciaCAN11bitECU.<>c__DisplayClass5_2 CS$<>8__locals3 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass5_2();
								CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
								string key2 = keyValuePair2.Key;
								CS$<>8__locals3.requestTitle = keyValuePair2.Value;
								OBDRequest requestForCommand3 = renaultDaciaCAN11bitECU.GetRequestForCommand(key2);
								requestForCommand3.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
								{
									string text5 = CS$<>8__locals3.CS$<>8__locals2.<>4__this.DecodeAsHEX(decodedRequest.Command, data);
									if (!string.IsNullOrEmpty(text5))
									{
										CS$<>8__locals3.CS$<>8__locals2.sb.AppendLine(CS$<>8__locals3.requestTitle + ": " + text5);
									}
								};
								list.Add(requestForCommand3);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						List<OBDRequest>.Enumerator enumerator2 = list.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								OBDRequest obdrequest = enumerator2.Current;
								obdrequest.CheckLength = true;
								obdrequest.ELMFormat = ELMFormat.CAN11bit;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultDaciaCAN11bitECU.<GetECUInformationReportAsync>d__5>(ref taskAwaiter, ref this);
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
					text = CS$<>8__locals1.sb.ToString();
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
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x060031B1 RID: 12721 RVA: 0x002247CC File Offset: 0x002229CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001CB7 RID: 7351
			public int <>1__state;

			// Token: 0x04001CB8 RID: 7352
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001CB9 RID: 7353
			public RenaultDaciaCAN11bitECU <>4__this;

			// Token: 0x04001CBA RID: 7354
			public CancellationToken token;

			// Token: 0x04001CBB RID: 7355
			private RenaultDaciaCAN11bitECU.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x04001CBC RID: 7356
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000516 RID: 1302
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadDTCAsync>d__7 : IAsyncStateMachine
		{
			// Token: 0x060031B2 RID: 12722 RVA: 0x002247DC File Offset: 0x002229DC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RenaultDaciaCAN11bitECU renaultDaciaCAN11bitECU = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0197;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_021A;
					}
					default:
					{
						renaultDaciaCAN11bitECU.cancellationToken = token;
						OBDRequest[] array = renaultDaciaCAN11bitECU.GetDTCReadRequests();
						foreach (OBDRequest obdrequest in array)
						{
							if (obdrequest.Command != null && obdrequest.Command.StartsWith("19"))
							{
								obdrequest.ResponseReceived += renaultDaciaCAN11bitECU.DtcReadOrClearRequest_ResponseReceived;
							}
						}
						if (renaultDaciaCAN11bitECU.TestELMDevice)
						{
							OBDRequest atshtestRequest = renaultDaciaCAN11bitECU.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
							OBDRequest atfctestRequest = renaultDaciaCAN11bitECU.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
							array = new OBDRequest[] { atshtestRequest, atfctestRequest }.Concat(array).ToArray<OBDRequest>();
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultDaciaCAN11bitECU.<ReadDTCAsync>d__7>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					renaultDaciaCAN11bitECU.cancellationToken = CancellationToken.None;
					taskAwaiter = renaultDaciaCAN11bitECU.RequestFreezeFrames().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultDaciaCAN11bitECU.<ReadDTCAsync>d__7>(ref taskAwaiter, ref this);
						return;
					}
					IL_0197:
					taskAwaiter.GetResult();
					if (SharedSettings.Current.UseDefaultInit)
					{
						goto IL_0221;
					}
					OBDRequest pingRequest = App.OBDReader.GetPingRequest();
					App.OBDReader.ReplaceQueue(pingRequest);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultDaciaCAN11bitECU.<ReadDTCAsync>d__7>(ref taskAwaiter, ref this);
						return;
					}
					IL_021A:
					taskAwaiter.GetResult();
					IL_0221:;
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

			// Token: 0x060031B3 RID: 12723 RVA: 0x00224A54 File Offset: 0x00222C54
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001CBD RID: 7357
			public int <>1__state;

			// Token: 0x04001CBE RID: 7358
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001CBF RID: 7359
			public RenaultDaciaCAN11bitECU <>4__this;

			// Token: 0x04001CC0 RID: 7360
			public CancellationToken token;

			// Token: 0x04001CC1 RID: 7361
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001CC2 RID: 7362
			public Action badELMDetectedCallback;

			// Token: 0x04001CC3 RID: 7363
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000517 RID: 1303
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestFreezeFrames>d__10 : IAsyncStateMachine
		{
			// Token: 0x060031B4 RID: 12724 RVA: 0x00224A64 File Offset: 0x00222C64
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RenaultDaciaCAN11bitECU renaultDaciaCAN11bitECU = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (renaultDaciaCAN11bitECU.Count == 0)
						{
							goto IL_0200;
						}
						List<OBDRequest> list = new List<OBDRequest>();
						IEnumerator<DTCItemV2> enumerator = renaultDaciaCAN11bitECU.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								RenaultDaciaCAN11bitECU.<>c__DisplayClass10_0 CS$<>8__locals1 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass10_0();
								CS$<>8__locals1.dtc = enumerator.Current;
								if (CS$<>8__locals1.dtc.RawCode.Length == 6)
								{
									CS$<>8__locals1.dtc.Payload = "";
									OBDRequest requestForCommand = renaultDaciaCAN11bitECU.GetRequestForCommand("1906" + CS$<>8__locals1.dtc.RawCode + "80");
									OBDRequest requestForCommand2 = renaultDaciaCAN11bitECU.GetRequestForCommand("1906" + CS$<>8__locals1.dtc.RawCode + "81");
									OBDRequest requestForCommand3 = renaultDaciaCAN11bitECU.GetRequestForCommand("1906" + CS$<>8__locals1.dtc.RawCode + "82");
									ResponseDecodedDelegate responseDecodedDelegate = delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
									{
										RenaultDaciaCAN11bitECU.<>c__DisplayClass10_1 CS$<>8__locals2 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass10_1();
										CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
										CS$<>8__locals2.data = data;
										if (CS$<>8__locals2.data != null && CS$<>8__locals2.data.Length >= 2)
										{
											switch (CS$<>8__locals2.data[0])
											{
											case 128:
												if (CS$<>8__locals2.data.Length >= 4)
												{
													RenaultDaciaCAN11bitECU.<>c__DisplayClass10_2 CS$<>8__locals3 = new RenaultDaciaCAN11bitECU.<>c__DisplayClass10_2();
													CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
													CS$<>8__locals3.mileage = (int)CS$<>8__locals3.CS$<>8__locals2.data[1] * 256 * 256 + (int)CS$<>8__locals3.CS$<>8__locals2.data[2] * 256 + (int)CS$<>8__locals3.CS$<>8__locals2.data[3];
													MainThreadHelper.InvokeOnMainThread(delegate
													{
														if (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload != "")
														{
															CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload + "\n";
														}
														CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = string.Concat(new string[]
														{
															CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.dtc.Payload,
															"Odometer: ",
															UnitsHelper.GetValue((double)CS$<>8__locals3.mileage, UnitsHelper.Units.km).ToString(),
															" ",
															UnitsHelper.GetCaption(UnitsHelper.Units.km)
														});
														DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
														if (recorder == null)
														{
															return;
														}
														recorder.Record(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.dtc);
													});
													return;
												}
												break;
											case 129:
												if (CS$<>8__locals2.data.Length >= 2)
												{
													MainThreadHelper.InvokeOnMainThread(delegate
													{
														if (CS$<>8__locals2.CS$<>8__locals1.dtc.Payload != "")
														{
															CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = CS$<>8__locals2.CS$<>8__locals1.dtc.Payload + "\n";
														}
														CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = CS$<>8__locals2.CS$<>8__locals1.dtc.Payload + "Aging counter: " + CS$<>8__locals2.data[1].ToString();
														DataRecorderV2 recorder2 = App.OBDReader.CurrentCarData.Recorder;
														if (recorder2 == null)
														{
															return;
														}
														recorder2.Record(CS$<>8__locals2.CS$<>8__locals1.dtc);
													});
													return;
												}
												break;
											case 130:
												if (CS$<>8__locals2.data.Length >= 2)
												{
													MainThreadHelper.InvokeOnMainThread(delegate
													{
														if (CS$<>8__locals2.CS$<>8__locals1.dtc.Payload != "")
														{
															CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = CS$<>8__locals2.CS$<>8__locals1.dtc.Payload + "\n";
														}
														CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = CS$<>8__locals2.CS$<>8__locals1.dtc.Payload + "Occurrence counter: " + CS$<>8__locals2.data[1].ToString();
														DataRecorderV2 recorder3 = App.OBDReader.CurrentCarData.Recorder;
														if (recorder3 == null)
														{
															return;
														}
														recorder3.Record(CS$<>8__locals2.CS$<>8__locals1.dtc);
													});
												}
												break;
											default:
												return;
											}
										}
									};
									requestForCommand.ResponseDecoded += responseDecodedDelegate;
									requestForCommand2.ResponseDecoded += responseDecodedDelegate;
									requestForCommand3.ResponseDecoded += responseDecodedDelegate;
									requestForCommand.ResponseReceived += renaultDaciaCAN11bitECU.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
									requestForCommand2.ResponseReceived += renaultDaciaCAN11bitECU.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
									requestForCommand3.ResponseReceived += renaultDaciaCAN11bitECU.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
									list.Add(requestForCommand);
									list.Add(requestForCommand2);
									list.Add(requestForCommand3);
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
						if (list.Count == 0)
						{
							goto IL_0200;
						}
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultDaciaCAN11bitECU.<RequestFreezeFrames>d__10>(ref taskAwaiter, ref this);
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
				IL_0200:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060031B5 RID: 12725 RVA: 0x00224CB8 File Offset: 0x00222EB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001CC4 RID: 7364
			public int <>1__state;

			// Token: 0x04001CC5 RID: 7365
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001CC6 RID: 7366
			public RenaultDaciaCAN11bitECU <>4__this;

			// Token: 0x04001CC7 RID: 7367
			private TaskAwaiter <>u__1;
		}
	}
}
