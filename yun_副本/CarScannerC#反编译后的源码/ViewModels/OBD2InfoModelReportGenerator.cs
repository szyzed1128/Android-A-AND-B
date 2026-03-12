using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x0200074D RID: 1869
	internal class OBD2InfoModelReportGenerator
	{
		// Token: 0x06003FA0 RID: 16288 RVA: 0x00331608 File Offset: 0x0032F808
		private async Task<Dictionary<string, List<string>>> GetSupported()
		{
			OBD2InfoModelReportGenerator.<>c__DisplayClass0_0 CS$<>8__locals1 = new OBD2InfoModelReportGenerator.<>c__DisplayClass0_0();
			string[] pids = new string[] { "0900", "0920", "0940", "0960", "0980" };
			Dictionary<string, List<string>> dictionary2;
			if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
			{
				pids = new string[] { "0900" };
				List<string> list = new List<string> { "0902", "0904", "0906", "090A" };
				Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
				foreach (ECUHeader ecuheader in App.OBDReader.ECUHeaders)
				{
					dictionary.Add(ecuheader.Id, list);
				}
				dictionary2 = dictionary;
			}
			else
			{
				if (SharedSettings.Current.Mode01Prefix == "22F4")
				{
					for (int i = 0; i < pids.Length; i++)
					{
						pids[i] = "22F8" + pids[i].Substring(2, 2);
					}
				}
				CS$<>8__locals1.responseData = new Dictionary<string, string>();
				List<OBDRequest> list2 = new List<OBDRequest>();
				for (int j = 0; j < pids.Length; j++)
				{
					OBDRequest obdrequest = new OBDRequest(pids[j], false);
					OBDRequest obdrequest2 = obdrequest;
					ResponseReceivedDelegate responseReceivedDelegate;
					if ((responseReceivedDelegate = CS$<>8__locals1.<>9__0) == null)
					{
						responseReceivedDelegate = (CS$<>8__locals1.<>9__0 = delegate(OBDRequest sender, string e)
						{
							if (e == null)
							{
								return;
							}
							if (e.Contains("NO DATA"))
							{
								return;
							}
							if (e.Contains(sender.ResponseMarker))
							{
								CS$<>8__locals1.responseData.Add(sender.Command, e);
							}
						});
					}
					obdrequest2.ResponseReceived += responseReceivedDelegate;
					list2.Add(obdrequest);
				}
				App.OBDReader.ReplaceQueue(list2);
				await App.OBDReader.WaitForCommandQueue();
				CS$<>8__locals1.dictHeadersSupportedPids = new Dictionary<string, List<string>>();
				foreach (ECUHeader header in App.OBDReader.ECUHeaders)
				{
					CS$<>8__locals1.dictHeadersSupportedPids.Add(header.Id, new List<string>());
					foreach (string text in pids)
					{
						OBDRequest obdrequest3 = new OBDRequest(text, false, new PID(text, text));
						obdrequest3.Payload = header.Id;
						ResponseDecodedDelegate responseDecodedDelegate;
						if ((responseDecodedDelegate = CS$<>8__locals1.<>9__1) == null)
						{
							OBD2InfoModelReportGenerator.<>c__DisplayClass0_0 CS$<>8__locals2 = CS$<>8__locals1;
							ResponseDecodedDelegate responseDecodedDelegate2 = delegate(OBDRequest dummyReq2, byte[] data, bool decodeResult, string responseHeader)
							{
								if (data != null && data.Length != 0)
								{
									try
									{
										List<string> supported = new SupportedPID(dummyReq2.Command).GetSupported(data);
										CS$<>8__locals1.dictHeadersSupportedPids[dummyReq2.Payload].AddRange(supported);
									}
									catch (Exception)
									{
									}
								}
							};
							CS$<>8__locals2.<>9__1 = responseDecodedDelegate2;
							responseDecodedDelegate = responseDecodedDelegate2;
						}
						obdrequest3.ResponseDecoded += responseDecodedDelegate;
						await App.OBDReader.DecodeData(CS$<>8__locals1.responseData[text], obdrequest3, header.Id);
					}
					string[] array = null;
					header = null;
				}
				IEnumerator<ECUHeader> enumerator2 = null;
				foreach (KeyValuePair<string, List<string>> keyValuePair in CS$<>8__locals1.dictHeadersSupportedPids)
				{
					if (keyValuePair.Value.Count == 0 && App.OBDReader.CurrentProtocolNumber > 2)
					{
						keyValuePair.Value.AddRange(new string[] { "0902", "0904", "0906", "090A" });
					}
				}
				dictionary2 = CS$<>8__locals1.dictHeadersSupportedPids;
			}
			return dictionary2;
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x00331644 File Offset: 0x0032F844
		private async Task<List<string>> GetSupportedV2()
		{
			List<string> list;
			if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
			{
				list = new List<string> { "0902", "0904", "0906", "090A" };
			}
			else
			{
				string pid_template = "09{0}";
				if (SharedSettings.Current.Mode01Prefix == "22F4")
				{
					pid_template = "22F4{0}";
				}
				bool finished = false;
				int idx = 0;
				PID_SupportedPids pid = null;
				List<string> resultPids = new List<string>();
				List<PID> emptyList = new List<PID>(0);
				do
				{
					OBD2InfoModelReportGenerator.<>c__DisplayClass1_0 CS$<>8__locals1 = new OBD2InfoModelReportGenerator.<>c__DisplayClass1_0();
					string text = string.Format(pid_template, idx.ToString("X2"));
					pid = new PID_SupportedPids(text, emptyList, "");
					OBDRequest req = new OBDRequest(text, false, pid);
					CS$<>8__locals1.data = "";
					req.ResponseReceived += delegate(OBDRequest sender, string response)
					{
						CS$<>8__locals1.data = response;
					};
					App.OBDReader.ReplaceQueue(req);
					await App.OBDReader.WaitForCommandQueue();
					string[] array = OBDDataReader.FilterHexAndNewLineOnly(CS$<>8__locals1.data).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					List<string> headers = App.OBDReader.GetHeaders(req.ResponseMarker, array, ELMFormat.Unknown);
					finished = true;
					foreach (string text2 in headers)
					{
						await App.OBDReader.DecodeData(CS$<>8__locals1.data, req, text2);
						if (pid.Value[pid.Value.Length - 1])
						{
							finished = false;
						}
						for (int i = 1; i < pid.Value.Length; i++)
						{
							if (pid.Value[i])
							{
								string text3 = string.Format(pid_template, (idx + i + 1).ToString("X2"));
								if (!resultPids.Contains(text3))
								{
									resultPids.Add(text3);
								}
							}
						}
					}
					List<string>.Enumerator enumerator = default(List<string>.Enumerator);
					idx += 32;
					CS$<>8__locals1 = null;
					req = null;
				}
				while (!finished);
				list = resultPids;
			}
			return list;
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x00331680 File Offset: 0x0032F880
		public async Task<Dictionary<string, List<InformationItem>>> GetInformationItems()
		{
			OBD2InfoModelReportGenerator.<>c__DisplayClass2_0 CS$<>8__locals1 = new OBD2InfoModelReportGenerator.<>c__DisplayClass2_0();
			List<string> list = await this.GetSupportedV2();
			List<string> supportedPids = list;
			supportedPids.AddRange(new string[] { "1A90", "22F190" });
			List<OBDRequest> list2 = new List<OBDRequest>();
			CS$<>8__locals1.responseData = new Dictionary<string, string>();
			foreach (string text in supportedPids)
			{
				OBDRequest obdrequest = new OBDRequest(text, false);
				ResponseReceivedDelegate responseReceivedDelegate;
				if ((responseReceivedDelegate = CS$<>8__locals1.<>9__0) == null)
				{
					OBD2InfoModelReportGenerator.<>c__DisplayClass2_0 CS$<>8__locals2 = CS$<>8__locals1;
					ResponseReceivedDelegate responseReceivedDelegate2 = delegate(OBDRequest sender, string e)
					{
						if (e == null)
						{
							return;
						}
						if (e.Contains("NO DATA"))
						{
							return;
						}
						CS$<>8__locals1.responseData.Add(sender.Command, e);
					};
					CS$<>8__locals2.<>9__0 = responseReceivedDelegate2;
					responseReceivedDelegate = responseReceivedDelegate2;
				}
				obdrequest.ResponseReceived += responseReceivedDelegate;
				list2.Add(obdrequest);
			}
			App.OBDReader.ReplaceQueue(list2);
			await App.OBDReader.WaitForCommandQueue();
			new Dictionary<string, Dictionary<string, byte[]>>();
			CS$<>8__locals1.result = new Dictionary<string, List<InformationItem>>();
			using (IEnumerator<ECUHeader> enumerator2 = App.OBDReader.ECUHeaders.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					OBD2InfoModelReportGenerator.<>c__DisplayClass2_1 CS$<>8__locals3 = new OBD2InfoModelReportGenerator.<>c__DisplayClass2_1();
					CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals3.header = enumerator2.Current;
					CS$<>8__locals3.CS$<>8__locals1.result.Add(CS$<>8__locals3.header.Id, new List<InformationItem>());
					using (List<string>.Enumerator enumerator3 = supportedPids.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string pid = enumerator3.Current;
							if (CS$<>8__locals3.CS$<>8__locals1.responseData.ContainsKey(pid))
							{
								PID pid2 = new PID(pid, pid);
								OBDRequest obdrequest2 = new OBDRequest(pid, false, pid2);
								obdrequest2.Payload = CS$<>8__locals3.header.Id;
								obdrequest2.ResponseDecoded += delegate(OBDRequest dummyRequest2, byte[] data, bool decodeResult, string responseHeader)
								{
									try
									{
										if (data != null && data.Length != 0)
										{
											InformationItem informationItem = new InformationItem
											{
												PID = pid,
												RawData = data,
												Header = dummyRequest2.Payload
											};
											informationItem.Decode();
											if (!string.IsNullOrEmpty(informationItem.Title) && !string.IsNullOrEmpty(informationItem.Value))
											{
												CS$<>8__locals3.CS$<>8__locals1.result[CS$<>8__locals3.header.Id].Add(informationItem);
											}
										}
									}
									catch (Exception)
									{
									}
								};
								await App.OBDReader.DecodeData(CS$<>8__locals3.CS$<>8__locals1.responseData[pid], obdrequest2, CS$<>8__locals3.header.Id);
							}
						}
					}
					List<string>.Enumerator enumerator3 = default(List<string>.Enumerator);
					CS$<>8__locals3 = null;
				}
			}
			IEnumerator<ECUHeader> enumerator2 = null;
			return CS$<>8__locals1.result;
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x003316C4 File Offset: 0x0032F8C4
		public async Task<string> GetReport()
		{
			Dictionary<string, List<InformationItem>> dictionary = await this.GetInformationItems();
			StringBuilder stringBuilder = new StringBuilder(32);
			stringBuilder.Append(Translate.GetString("Settings_Control_tbECUProtocol.Text"));
			stringBuilder.Append(' ');
			string text = StaticLists.Protocols[App.OBDReader.CurrentProtocolNumber];
			stringBuilder.Append(text);
			stringBuilder.Append('\n');
			foreach (string text2 in dictionary.Keys)
			{
				if (dictionary[text2].Count != 0)
				{
					stringBuilder.Append("ECU address/CAN Id: ");
					stringBuilder.Append(text2);
					stringBuilder.Append("\n");
					foreach (InformationItem informationItem in (from x in dictionary[text2]
						orderby x.PID == "0908" || x.PID == "090B", x.PID descending
						select x).ToArray<InformationItem>())
					{
						stringBuilder.Append(informationItem.Title);
						stringBuilder.Append(": ");
						if (informationItem.Value.Contains('\n'))
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append(informationItem.Value);
						stringBuilder.Append("\n");
					}
					stringBuilder.Append("\n");
				}
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x00002050 File Offset: 0x00000250
		public OBD2InfoModelReportGenerator()
		{
		}

		// Token: 0x0200074E RID: 1870
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003FA5 RID: 16293 RVA: 0x00331707 File Offset: 0x0032F907
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003FA6 RID: 16294 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003FA7 RID: 16295 RVA: 0x00331713 File Offset: 0x0032F913
			internal bool <GetReport>b__3_0(InformationItem x)
			{
				return x.PID == "0908" || x.PID == "090B";
			}

			// Token: 0x06003FA8 RID: 16296 RVA: 0x00331739 File Offset: 0x0032F939
			internal string <GetReport>b__3_1(InformationItem x)
			{
				return x.PID;
			}

			// Token: 0x040026E7 RID: 9959
			public static readonly OBD2InfoModelReportGenerator.<>c <>9 = new OBD2InfoModelReportGenerator.<>c();

			// Token: 0x040026E8 RID: 9960
			public static Func<InformationItem, bool> <>9__3_0;

			// Token: 0x040026E9 RID: 9961
			public static Func<InformationItem, string> <>9__3_1;
		}

		// Token: 0x0200074F RID: 1871
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06003FA9 RID: 16297 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06003FAA RID: 16298 RVA: 0x00331741 File Offset: 0x0032F941
			internal void <GetSupported>b__0(OBDRequest sender, string e)
			{
				if (e == null)
				{
					return;
				}
				if (e.Contains("NO DATA"))
				{
					return;
				}
				if (e.Contains(sender.ResponseMarker))
				{
					this.responseData.Add(sender.Command, e);
				}
			}

			// Token: 0x06003FAB RID: 16299 RVA: 0x00331778 File Offset: 0x0032F978
			internal void <GetSupported>b__1(OBDRequest dummyReq2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					try
					{
						List<string> supported = new SupportedPID(dummyReq2.Command).GetSupported(data);
						this.dictHeadersSupportedPids[dummyReq2.Payload].AddRange(supported);
					}
					catch (Exception)
					{
					}
				}
			}

			// Token: 0x040026EA RID: 9962
			public Dictionary<string, string> responseData;

			// Token: 0x040026EB RID: 9963
			public Dictionary<string, List<string>> dictHeadersSupportedPids;

			// Token: 0x040026EC RID: 9964
			public ResponseReceivedDelegate <>9__0;

			// Token: 0x040026ED RID: 9965
			public ResponseDecodedDelegate <>9__1;
		}

		// Token: 0x02000750 RID: 1872
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06003FAC RID: 16300 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06003FAD RID: 16301 RVA: 0x003317CC File Offset: 0x0032F9CC
			internal void <GetSupportedV2>b__0(OBDRequest sender, string response)
			{
				this.data = response;
			}

			// Token: 0x040026EE RID: 9966
			public string data;
		}

		// Token: 0x02000751 RID: 1873
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x06003FAE RID: 16302 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06003FAF RID: 16303 RVA: 0x003317D5 File Offset: 0x0032F9D5
			internal void <GetInformationItems>b__0(OBDRequest sender, string e)
			{
				if (e == null)
				{
					return;
				}
				if (e.Contains("NO DATA"))
				{
					return;
				}
				this.responseData.Add(sender.Command, e);
			}

			// Token: 0x040026EF RID: 9967
			public Dictionary<string, string> responseData;

			// Token: 0x040026F0 RID: 9968
			public Dictionary<string, List<InformationItem>> result;

			// Token: 0x040026F1 RID: 9969
			public ResponseReceivedDelegate <>9__0;
		}

		// Token: 0x02000752 RID: 1874
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_1
		{
			// Token: 0x06003FB0 RID: 16304 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_1()
			{
			}

			// Token: 0x040026F2 RID: 9970
			public ECUHeader header;

			// Token: 0x040026F3 RID: 9971
			public OBD2InfoModelReportGenerator.<>c__DisplayClass2_0 CS$<>8__locals1;
		}

		// Token: 0x02000753 RID: 1875
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_2
		{
			// Token: 0x06003FB1 RID: 16305 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_2()
			{
			}

			// Token: 0x06003FB2 RID: 16306 RVA: 0x003317FC File Offset: 0x0032F9FC
			internal void <GetInformationItems>b__1(OBDRequest dummyRequest2, byte[] data, bool decodeResult, string responseHeader)
			{
				try
				{
					if (data != null && data.Length != 0)
					{
						InformationItem informationItem = new InformationItem
						{
							PID = this.pid,
							RawData = data,
							Header = dummyRequest2.Payload
						};
						informationItem.Decode();
						if (!string.IsNullOrEmpty(informationItem.Title) && !string.IsNullOrEmpty(informationItem.Value))
						{
							this.CS$<>8__locals2.CS$<>8__locals1.result[this.CS$<>8__locals2.header.Id].Add(informationItem);
						}
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x040026F4 RID: 9972
			public string pid;

			// Token: 0x040026F5 RID: 9973
			public OBD2InfoModelReportGenerator.<>c__DisplayClass2_1 CS$<>8__locals2;
		}

		// Token: 0x02000754 RID: 1876
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetInformationItems>d__2 : IAsyncStateMachine
		{
			// Token: 0x06003FB3 RID: 16307 RVA: 0x00331898 File Offset: 0x0032FA98
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBD2InfoModelReportGenerator obd2InfoModelReportGenerator = this;
				Dictionary<string, List<InformationItem>> result;
				try
				{
					TaskAwaiter<List<string>> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<List<string>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<List<string>>);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_01AA;
					}
					case 2:
						IL_01DC:
						try
						{
							if (num != 2)
							{
								goto IL_03CC;
							}
							IL_0253:
							try
							{
								ValueTaskAwaiter<bool> valueTaskAwaiter;
								if (num == 2)
								{
									ValueTaskAwaiter<bool> valueTaskAwaiter2;
									valueTaskAwaiter = valueTaskAwaiter2;
									valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
									num = (num2 = -1);
									goto IL_0389;
								}
								IL_0391:
								while (enumerator3.MoveNext())
								{
									OBD2InfoModelReportGenerator.<>c__DisplayClass2_2 CS$<>8__locals4 = new OBD2InfoModelReportGenerator.<>c__DisplayClass2_2();
									CS$<>8__locals4.CS$<>8__locals2 = CS$<>8__locals3;
									CS$<>8__locals4.pid = enumerator3.Current;
									if (CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.responseData.ContainsKey(CS$<>8__locals4.pid))
									{
										PID pid = new PID(CS$<>8__locals4.pid, CS$<>8__locals4.pid);
										OBDRequest obdrequest = new OBDRequest(CS$<>8__locals4.pid, false, pid);
										obdrequest.Payload = CS$<>8__locals4.CS$<>8__locals2.header.Id;
										obdrequest.ResponseDecoded += delegate(OBDRequest dummyRequest2, byte[] data, bool decodeResult, string responseHeader)
										{
											try
											{
												if (data != null && data.Length != 0)
												{
													InformationItem informationItem = new InformationItem
													{
														PID = CS$<>8__locals4.pid,
														RawData = data,
														Header = dummyRequest2.Payload
													};
													informationItem.Decode();
													if (!string.IsNullOrEmpty(informationItem.Title) && !string.IsNullOrEmpty(informationItem.Value))
													{
														CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.result[CS$<>8__locals4.CS$<>8__locals2.header.Id].Add(informationItem);
													}
												}
											}
											catch (Exception)
											{
											}
										};
										valueTaskAwaiter = App.OBDReader.DecodeData(CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.responseData[CS$<>8__locals4.pid], obdrequest, CS$<>8__locals4.CS$<>8__locals2.header.Id).GetAwaiter();
										if (!valueTaskAwaiter.IsCompleted)
										{
											num = (num2 = 2);
											ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
											this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBD2InfoModelReportGenerator.<GetInformationItems>d__2>(ref valueTaskAwaiter, ref this);
											return;
										}
										goto IL_0389;
									}
								}
								goto IL_03B9;
								IL_0389:
								valueTaskAwaiter.GetResult();
								goto IL_0391;
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator3).Dispose();
								}
							}
							IL_03B9:
							enumerator3 = default(List<string>.Enumerator);
							CS$<>8__locals3 = null;
							IL_03CC:
							if (enumerator2.MoveNext())
							{
								CS$<>8__locals3 = new OBD2InfoModelReportGenerator.<>c__DisplayClass2_1();
								CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
								CS$<>8__locals3.header = enumerator2.Current;
								CS$<>8__locals3.CS$<>8__locals1.result.Add(CS$<>8__locals3.header.Id, new List<InformationItem>());
								enumerator3 = supportedPids.GetEnumerator();
								goto IL_0253;
							}
						}
						finally
						{
							if (num < 0 && enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
						enumerator2 = null;
						result = CS$<>8__locals1.result;
						goto IL_0432;
					default:
						CS$<>8__locals1 = new OBD2InfoModelReportGenerator.<>c__DisplayClass2_0();
						taskAwaiter = obd2InfoModelReportGenerator.GetSupportedV2().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<List<string>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<string>>, OBD2InfoModelReportGenerator.<GetInformationItems>d__2>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					List<string> result2 = taskAwaiter.GetResult();
					supportedPids = result2;
					supportedPids.AddRange(new string[] { "1A90", "22F190" });
					List<OBDRequest> list = new List<OBDRequest>();
					CS$<>8__locals1.responseData = new Dictionary<string, string>();
					List<string>.Enumerator enumerator4 = supportedPids.GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							string text = enumerator4.Current;
							OBDRequest obdrequest2 = new OBDRequest(text, false);
							OBDRequest obdrequest3 = obdrequest2;
							ResponseReceivedDelegate responseReceivedDelegate;
							if ((responseReceivedDelegate = CS$<>8__locals1.<>9__0) == null)
							{
								responseReceivedDelegate = (CS$<>8__locals1.<>9__0 = delegate(OBDRequest sender, string e)
								{
									if (e == null)
									{
										return;
									}
									if (e.Contains("NO DATA"))
									{
										return;
									}
									CS$<>8__locals1.responseData.Add(sender.Command, e);
								});
							}
							obdrequest3.ResponseReceived += responseReceivedDelegate;
							list.Add(obdrequest2);
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator4).Dispose();
						}
					}
					App.OBDReader.ReplaceQueue(list);
					taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBD2InfoModelReportGenerator.<GetInformationItems>d__2>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01AA:
					taskAwaiter3.GetResult();
					new Dictionary<string, Dictionary<string, byte[]>>();
					CS$<>8__locals1.result = new Dictionary<string, List<InformationItem>>();
					enumerator2 = App.OBDReader.ECUHeaders.GetEnumerator();
					goto IL_01DC;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					supportedPids = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0432:
				num2 = -2;
				CS$<>8__locals1 = null;
				supportedPids = null;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x06003FB4 RID: 16308 RVA: 0x00331D60 File Offset: 0x0032FF60
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040026F6 RID: 9974
			public int <>1__state;

			// Token: 0x040026F7 RID: 9975
			public AsyncTaskMethodBuilder<Dictionary<string, List<InformationItem>>> <>t__builder;

			// Token: 0x040026F8 RID: 9976
			public OBD2InfoModelReportGenerator <>4__this;

			// Token: 0x040026F9 RID: 9977
			private OBD2InfoModelReportGenerator.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x040026FA RID: 9978
			private OBD2InfoModelReportGenerator.<>c__DisplayClass2_1 <>8__2;

			// Token: 0x040026FB RID: 9979
			private List<string> <supportedPids>5__2;

			// Token: 0x040026FC RID: 9980
			private TaskAwaiter<List<string>> <>u__1;

			// Token: 0x040026FD RID: 9981
			private TaskAwaiter <>u__2;

			// Token: 0x040026FE RID: 9982
			private IEnumerator<ECUHeader> <>7__wrap2;

			// Token: 0x040026FF RID: 9983
			private List<string>.Enumerator <>7__wrap3;

			// Token: 0x04002700 RID: 9984
			private ValueTaskAwaiter<bool> <>u__3;
		}

		// Token: 0x02000755 RID: 1877
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetReport>d__3 : IAsyncStateMachine
		{
			// Token: 0x06003FB5 RID: 16309 RVA: 0x00331D70 File Offset: 0x0032FF70
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBD2InfoModelReportGenerator obd2InfoModelReportGenerator = this;
				string text3;
				try
				{
					TaskAwaiter<Dictionary<string, List<InformationItem>>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = obd2InfoModelReportGenerator.GetInformationItems().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<Dictionary<string, List<InformationItem>>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Dictionary<string, List<InformationItem>>>, OBD2InfoModelReportGenerator.<GetReport>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Dictionary<string, List<InformationItem>>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Dictionary<string, List<InformationItem>>>);
						num = (num2 = -1);
					}
					Dictionary<string, List<InformationItem>> result = taskAwaiter.GetResult();
					StringBuilder stringBuilder = new StringBuilder(32);
					stringBuilder.Append(Translate.GetString("Settings_Control_tbECUProtocol.Text"));
					stringBuilder.Append(' ');
					string text = StaticLists.Protocols[App.OBDReader.CurrentProtocolNumber];
					stringBuilder.Append(text);
					stringBuilder.Append('\n');
					Dictionary<string, List<InformationItem>>.KeyCollection.Enumerator enumerator = result.Keys.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string text2 = enumerator.Current;
							if (result[text2].Count != 0)
							{
								stringBuilder.Append("ECU address/CAN Id: ");
								stringBuilder.Append(text2);
								stringBuilder.Append("\n");
								foreach (InformationItem informationItem in (from x in result[text2]
									orderby x.PID == "0908" || x.PID == "090B", x.PID descending
									select x).ToArray<InformationItem>())
								{
									stringBuilder.Append(informationItem.Title);
									stringBuilder.Append(": ");
									if (informationItem.Value.Contains('\n'))
									{
										stringBuilder.Append("\n");
									}
									stringBuilder.Append(informationItem.Value);
									stringBuilder.Append("\n");
								}
								stringBuilder.Append("\n");
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
					text3 = stringBuilder.ToString().Trim();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(text3);
			}

			// Token: 0x06003FB6 RID: 16310 RVA: 0x00331FF0 File Offset: 0x003301F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002701 RID: 9985
			public int <>1__state;

			// Token: 0x04002702 RID: 9986
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04002703 RID: 9987
			public OBD2InfoModelReportGenerator <>4__this;

			// Token: 0x04002704 RID: 9988
			private TaskAwaiter<Dictionary<string, List<InformationItem>>> <>u__1;
		}

		// Token: 0x02000756 RID: 1878
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetSupported>d__0 : IAsyncStateMachine
		{
			// Token: 0x06003FB7 RID: 16311 RVA: 0x00332000 File Offset: 0x00330200
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Dictionary<string, List<string>> dictionary2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							goto IL_025D;
						}
						CS$<>8__locals1 = new OBD2InfoModelReportGenerator.<>c__DisplayClass0_0();
						pids = new string[] { "0900", "0920", "0940", "0960", "0980" };
						if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
						{
							pids = new string[] { "0900" };
							List<string> list = new List<string> { "0902", "0904", "0906", "090A" };
							Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
							IEnumerator<ECUHeader> enumerator3 = App.OBDReader.ECUHeaders.GetEnumerator();
							try
							{
								while (enumerator3.MoveNext())
								{
									ECUHeader ecuheader = enumerator3.Current;
									dictionary.Add(ecuheader.Id, list);
								}
							}
							finally
							{
								if (num < 0 && enumerator3 != null)
								{
									enumerator3.Dispose();
								}
							}
							dictionary2 = dictionary;
							goto IL_04C4;
						}
						if (SharedSettings.Current.Mode01Prefix == "22F4")
						{
							for (int i = 0; i < pids.Length; i++)
							{
								pids[i] = "22F8" + pids[i].Substring(2, 2);
							}
						}
						CS$<>8__locals1.responseData = new Dictionary<string, string>();
						List<OBDRequest> list2 = new List<OBDRequest>();
						for (int j = 0; j < pids.Length; j++)
						{
							OBDRequest obdrequest = new OBDRequest(pids[j], false);
							OBDRequest obdrequest2 = obdrequest;
							ResponseReceivedDelegate responseReceivedDelegate;
							if ((responseReceivedDelegate = CS$<>8__locals1.<>9__0) == null)
							{
								responseReceivedDelegate = (CS$<>8__locals1.<>9__0 = delegate(OBDRequest sender, string e)
								{
									if (e == null)
									{
										return;
									}
									if (e.Contains("NO DATA"))
									{
										return;
									}
									if (e.Contains(sender.ResponseMarker))
									{
										CS$<>8__locals1.responseData.Add(sender.Command, e);
									}
								});
							}
							obdrequest2.ResponseReceived += responseReceivedDelegate;
							list2.Add(obdrequest);
						}
						App.OBDReader.ReplaceQueue(list2);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBD2InfoModelReportGenerator.<GetSupported>d__0>(ref taskAwaiter, ref this);
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
					CS$<>8__locals1.dictHeadersSupportedPids = new Dictionary<string, List<string>>();
					enumerator2 = App.OBDReader.ECUHeaders.GetEnumerator();
					IL_025D:
					try
					{
						if (num != 1)
						{
							goto IL_03D7;
						}
						ValueTaskAwaiter<bool> valueTaskAwaiter2;
						ValueTaskAwaiter<bool> valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						num = (num2 = -1);
						IL_03A0:
						valueTaskAwaiter.GetResult();
						k++;
						IL_03B6:
						if (k >= array.Length)
						{
							array = null;
							header = null;
						}
						else
						{
							string text = array[k];
							PID pid = new PID(text, text);
							OBDRequest obdrequest3 = new OBDRequest(text, false, pid);
							obdrequest3.Payload = header.Id;
							OBDRequest obdrequest4 = obdrequest3;
							ResponseDecodedDelegate responseDecodedDelegate;
							if ((responseDecodedDelegate = CS$<>8__locals1.<>9__1) == null)
							{
								responseDecodedDelegate = (CS$<>8__locals1.<>9__1 = delegate(OBDRequest dummyReq2, byte[] data, bool decodeResult, string responseHeader)
								{
									if (data != null && data.Length != 0)
									{
										try
										{
											List<string> supported = new SupportedPID(dummyReq2.Command).GetSupported(data);
											CS$<>8__locals1.dictHeadersSupportedPids[dummyReq2.Payload].AddRange(supported);
										}
										catch (Exception)
										{
										}
									}
								});
							}
							obdrequest4.ResponseDecoded += responseDecodedDelegate;
							valueTaskAwaiter = App.OBDReader.DecodeData(CS$<>8__locals1.responseData[text], obdrequest3, header.Id).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								num = (num2 = 1);
								valueTaskAwaiter2 = valueTaskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBD2InfoModelReportGenerator.<GetSupported>d__0>(ref valueTaskAwaiter, ref this);
								return;
							}
							goto IL_03A0;
						}
						IL_03D7:
						if (enumerator2.MoveNext())
						{
							header = enumerator2.Current;
							CS$<>8__locals1.dictHeadersSupportedPids.Add(header.Id, new List<string>());
							array = pids;
							k = 0;
							goto IL_03B6;
						}
					}
					finally
					{
						if (num < 0 && enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					enumerator2 = null;
					Dictionary<string, List<string>>.Enumerator enumerator4 = CS$<>8__locals1.dictHeadersSupportedPids.GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							KeyValuePair<string, List<string>> keyValuePair = enumerator4.Current;
							if (keyValuePair.Value.Count == 0 && App.OBDReader.CurrentProtocolNumber > 2)
							{
								keyValuePair.Value.AddRange(new string[] { "0902", "0904", "0906", "090A" });
							}
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator4).Dispose();
						}
					}
					dictionary2 = CS$<>8__locals1.dictHeadersSupportedPids;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					pids = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04C4:
				num2 = -2;
				CS$<>8__locals1 = null;
				pids = null;
				this.<>t__builder.SetResult(dictionary2);
			}

			// Token: 0x06003FB8 RID: 16312 RVA: 0x00332558 File Offset: 0x00330758
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002705 RID: 9989
			public int <>1__state;

			// Token: 0x04002706 RID: 9990
			public AsyncTaskMethodBuilder<Dictionary<string, List<string>>> <>t__builder;

			// Token: 0x04002707 RID: 9991
			private OBD2InfoModelReportGenerator.<>c__DisplayClass0_0 <>8__1;

			// Token: 0x04002708 RID: 9992
			private string[] <pids>5__2;

			// Token: 0x04002709 RID: 9993
			private TaskAwaiter <>u__1;

			// Token: 0x0400270A RID: 9994
			private IEnumerator<ECUHeader> <>7__wrap2;

			// Token: 0x0400270B RID: 9995
			private ECUHeader <header>5__4;

			// Token: 0x0400270C RID: 9996
			private string[] <>7__wrap4;

			// Token: 0x0400270D RID: 9997
			private int <>7__wrap5;

			// Token: 0x0400270E RID: 9998
			private ValueTaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000757 RID: 1879
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetSupportedV2>d__1 : IAsyncStateMachine
		{
			// Token: 0x06003FB9 RID: 16313 RVA: 0x00332568 File Offset: 0x00330768
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				List<string> list;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num == 0)
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0198;
					}
					if (num == 1)
					{
						goto IL_01E8;
					}
					if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
					{
						list = new List<string> { "0902", "0904", "0906", "090A" };
						goto IL_03A5;
					}
					pid_template = "09{0}";
					if (SharedSettings.Current.Mode01Prefix == "22F4")
					{
						pid_template = "22F4{0}";
					}
					finished = false;
					idx = 0;
					pid = null;
					resultPids = new List<string>();
					emptyList = new List<PID>(0);
					IL_00B0:
					CS$<>8__locals1 = new OBD2InfoModelReportGenerator.<>c__DisplayClass1_0();
					string text = string.Format(pid_template, idx.ToString("X2"));
					pid = new PID_SupportedPids(text, emptyList, "");
					req = new OBDRequest(text, false, pid);
					CS$<>8__locals1.data = "";
					req.ResponseReceived += delegate(OBDRequest sender, string response)
					{
						CS$<>8__locals1.data = response;
					};
					App.OBDReader.ReplaceQueue(req);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 0);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBD2InfoModelReportGenerator.<GetSupportedV2>d__1>(ref taskAwaiter, ref this);
						return;
					}
					IL_0198:
					taskAwaiter.GetResult();
					string[] array = OBDDataReader.FilterHexAndNewLineOnly(CS$<>8__locals1.data).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					List<string> headers = App.OBDReader.GetHeaders(req.ResponseMarker, array, ELMFormat.Unknown);
					finished = true;
					enumerator = headers.GetEnumerator();
					IL_01E8:
					try
					{
						if (num != 1)
						{
							goto IL_030B;
						}
						ValueTaskAwaiter<bool> valueTaskAwaiter2;
						ValueTaskAwaiter<bool> valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						num = (num2 = -1);
						IL_0271:
						valueTaskAwaiter.GetResult();
						if (pid.Value[pid.Value.Length - 1])
						{
							finished = false;
						}
						for (int i = 1; i < pid.Value.Length; i++)
						{
							if (pid.Value[i])
							{
								string text2 = string.Format(pid_template, (idx + i + 1).ToString("X2"));
								if (!resultPids.Contains(text2))
								{
									resultPids.Add(text2);
								}
							}
						}
						IL_030B:
						if (enumerator.MoveNext())
						{
							string text3 = enumerator.Current;
							valueTaskAwaiter = App.OBDReader.DecodeData(CS$<>8__locals1.data, req, text3).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								num = (num2 = 1);
								valueTaskAwaiter2 = valueTaskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBD2InfoModelReportGenerator.<GetSupportedV2>d__1>(ref valueTaskAwaiter, ref this);
								return;
							}
							goto IL_0271;
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					enumerator = default(List<string>.Enumerator);
					idx += 32;
					CS$<>8__locals1 = null;
					req = null;
					if (!finished)
					{
						goto IL_00B0;
					}
					list = resultPids;
				}
				catch (Exception ex)
				{
					num2 = -2;
					pid_template = null;
					pid = null;
					resultPids = null;
					emptyList = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_03A5:
				num2 = -2;
				pid_template = null;
				pid = null;
				resultPids = null;
				emptyList = null;
				this.<>t__builder.SetResult(list);
			}

			// Token: 0x06003FBA RID: 16314 RVA: 0x00332980 File Offset: 0x00330B80
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400270F RID: 9999
			public int <>1__state;

			// Token: 0x04002710 RID: 10000
			public AsyncTaskMethodBuilder<List<string>> <>t__builder;

			// Token: 0x04002711 RID: 10001
			private OBD2InfoModelReportGenerator.<>c__DisplayClass1_0 <>8__1;

			// Token: 0x04002712 RID: 10002
			private string <pid_template>5__2;

			// Token: 0x04002713 RID: 10003
			private bool <finished>5__3;

			// Token: 0x04002714 RID: 10004
			private int <idx>5__4;

			// Token: 0x04002715 RID: 10005
			private PID_SupportedPids <pid>5__5;

			// Token: 0x04002716 RID: 10006
			private List<string> <resultPids>5__6;

			// Token: 0x04002717 RID: 10007
			private List<PID> <emptyList>5__7;

			// Token: 0x04002718 RID: 10008
			private OBDRequest <req>5__8;

			// Token: 0x04002719 RID: 10009
			private TaskAwaiter <>u__1;

			// Token: 0x0400271A RID: 10010
			private List<string>.Enumerator <>7__wrap8;

			// Token: 0x0400271B RID: 10011
			private ValueTaskAwaiter<bool> <>u__2;
		}
	}
}
