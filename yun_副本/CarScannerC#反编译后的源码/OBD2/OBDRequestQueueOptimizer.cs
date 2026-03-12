using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using MoreLinq;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000391 RID: 913
	public static class OBDRequestQueueOptimizer
	{
		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x060026C7 RID: 9927 RVA: 0x001DDD9E File Offset: 0x001DBF9E
		// (set) Token: 0x060026C8 RID: 9928 RVA: 0x001DDDA5 File Offset: 0x001DBFA5
		public static HashSet<string> MainECUSupportedItems
		{
			[CompilerGenerated]
			get
			{
				return OBDRequestQueueOptimizer.<MainECUSupportedItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				OBDRequestQueueOptimizer.<MainECUSupportedItems>k__BackingField = value;
			}
		} = new HashSet<string>();

		// Token: 0x060026C9 RID: 9929 RVA: 0x001DDDB0 File Offset: 0x001DBFB0
		public static void RecordDataLength(string header, string pid, short length)
		{
			if (header == null)
			{
				header = "";
			}
			object obj = OBDRequestQueueOptimizer.record_lock_object;
			lock (obj)
			{
				if (!OBDRequestQueueOptimizer.OptimizationDictionary.ContainsKey(header))
				{
					OBDRequestQueueOptimizer.OptimizationDictionary.TryAdd(header, new Dictionary<string, short>());
				}
				OBDRequestQueueOptimizer.OptimizationDictionary[header][pid] = length;
				Dictionary<string, Dictionary<string, short>> dictionary = new Dictionary<string, Dictionary<string, short>>();
				foreach (string text in OBDRequestQueueOptimizer.OptimizationDictionary.Keys)
				{
					if (text == "")
					{
						Dictionary<string, short> dictionary2 = OBDRequestQueueOptimizer.BuildOBD2Dictionary();
						Dictionary<string, short> dictionary3 = OBDRequestQueueOptimizer.OptimizationDictionary[text];
						Dictionary<string, short> dictionary4 = new Dictionary<string, short>();
						foreach (string text2 in dictionary3.Keys)
						{
							if (!dictionary2.ContainsKey(text2))
							{
								dictionary4[text2] = dictionary3[text2];
							}
						}
						dictionary[""] = dictionary4;
					}
					else
					{
						dictionary[text] = OBDRequestQueueOptimizer.OptimizationDictionary[text];
					}
				}
				SharedSettings.Current.CANOptimizeMode22DataLengthDictionary = JsonConvert.SerializeObject(dictionary);
			}
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x001DDF4C File Offset: 0x001DC14C
		public static void RefreshOBD2Dictionary()
		{
			Dictionary<string, short> dictionary = OBDRequestQueueOptimizer.BuildOBD2Dictionary();
			foreach (string text in dictionary.Keys)
			{
				OBDRequestQueueOptimizer.OptimizationDictionary[""][text] = dictionary[text];
			}
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x001DDFBC File Offset: 0x001DC1BC
		public static IEnumerable<OBDRequest> Optimize(IEnumerable<OBDRequest> queue)
		{
			if (SharedSettings.Current.CANOptimizeRequests && (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit || App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit))
			{
				try
				{
					return OBDRequestQueueOptimizer.OptimizeV2(queue);
				}
				catch (Exception)
				{
					return queue;
				}
			}
			if (queue.Any((OBDRequest x) => x is OBDMultiRequest))
			{
				List<OBDRequest> list = new List<OBDRequest>();
				foreach (OBDRequest obdrequest in queue)
				{
					if (obdrequest is OBDMultiRequest)
					{
						OBDMultiRequest obdmultiRequest = obdrequest as OBDMultiRequest;
						list.AddRange(OBDMultiRequest.Disassemble(obdmultiRequest));
					}
					else
					{
						list.Add(obdrequest);
					}
				}
				return list;
			}
			return queue;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x001DE09C File Offset: 0x001DC29C
		private static IEnumerable<OBDRequest> OptimizeV2(IEnumerable<OBDRequest> queue)
		{
			List<OBDRequest> list = new List<OBDRequest>();
			List<OBDRequest> list2 = new List<OBDRequest>();
			List<OBDRequest> list3 = new List<OBDRequest>();
			new Dictionary<string, int>();
			foreach (OBDRequest obdrequest in queue)
			{
				if (!obdrequest.Repeat)
				{
					list2.Add(obdrequest);
				}
				else if (obdrequest is OBDMultiRequest)
				{
					OBDMultiRequest obdmultiRequest = (OBDMultiRequest)obdrequest;
					list.AddRange(OBDMultiRequest.Disassemble(obdmultiRequest));
				}
				else if (OBDRequestQueueOptimizer.IsOptimizable(obdrequest.Header, obdrequest.Command))
				{
					list.Add(obdrequest);
				}
				else
				{
					list2.Add(obdrequest);
				}
			}
			List<string> list4 = new List<string>();
			foreach (OBDRequest obdrequest2 in list)
			{
				if (!list4.Contains(obdrequest2.Header))
				{
					list4.Add(obdrequest2.Header);
				}
			}
			int num = SharedSettings.Current.CANOptimizeMaxInRequest;
			if (num > 6 && (!SharedSettings.Current.CANRequestSegmentationSTNLevel || !App.OBDReader.STCommandsStupported))
			{
				num = 6;
			}
			int num2 = SharedSettings.Current.CANOptimizeMode22MaxInRequest;
			if (num2 > 3 && (!SharedSettings.Current.CANRequestSegmentationSTNLevel || !App.OBDReader.STCommandsStupported))
			{
				num2 = 3;
			}
			using (List<string>.Enumerator enumerator3 = list4.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					string header = enumerator3.Current;
					List<OBDRequest> list5 = (from x in MoreEnumerable.DistinctBy<OBDRequest, string>(list.Where((OBDRequest x) => x.Header == header), (OBDRequest x) => x.Command)
						orderby x.Command
						select x).ToList<OBDRequest>();
					if (header == "")
					{
						List<OBDRequest> list6 = list5.Where((OBDRequest x) => x.Command.StartsWith("01")).ToList<OBDRequest>();
						List<OBDRequest> list7 = list5.Where((OBDRequest x) => x.Command.StartsWith("22")).ToList<OBDRequest>();
						OBDRequestQueueOptimizer.OptimizeRequestsOfHeader(list3, list6, num, 0);
						OBDRequestQueueOptimizer.OptimizeRequestsOfHeader(list3, list7, num2, 0);
					}
					else
					{
						OBDRequestQueueOptimizer.OptimizeRequestsOfHeader(list3, list5, num2, 0);
					}
				}
			}
			return (from x in list3.Concat(list2)
				orderby x.Header == "" descending, x.Header == "7E0" descending, x.Header == "7E1" descending, x.Header
				select x).ToList<OBDRequest>();
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x001DE404 File Offset: 0x001DC604
		private static void OptimizeRequestsOfHeader(List<OBDRequest> optimized, List<OBDRequest> reqs_of_header, int max_requests, int skipTarget = 0)
		{
			if (reqs_of_header == null || reqs_of_header.Count == 0)
			{
				return;
			}
			if (reqs_of_header.Count > max_requests)
			{
				int firstRequestSkipTarget = reqs_of_header[0].SkipCyclesTarget;
				if (!reqs_of_header.All((OBDRequest x) => x.SkipCyclesTarget == firstRequestSkipTarget))
				{
					int[] array = (from x in reqs_of_header.Select((OBDRequest x) => x.SkipCyclesTarget).Distinct<int>()
						orderby x
						select x).ToArray<int>();
					Dictionary<int, List<OBDRequest>> dictionary = new Dictionary<int, List<OBDRequest>>(array.Length);
					for (int i = array.Length - 1; i >= 0; i--)
					{
						int skip_target = array[i];
						List<OBDRequest> list = reqs_of_header.Where((OBDRequest x) => x.SkipCyclesTarget == skip_target).ToList<OBDRequest>();
						if (i < array.Length - 1 && list.Count < max_requests)
						{
							int num = max_requests - list.Count;
							int num2 = array[i + 1];
							List<OBDRequest> list2 = dictionary[num2];
							if (list2.Count <= num)
							{
								list.AddRange(list2);
								dictionary.Remove(num2);
							}
							else
							{
								IEnumerable<OBDRequest> items_to_transfer = list2.Take(num);
								list.AddRange(items_to_transfer);
								list2.RemoveAll((OBDRequest x) => items_to_transfer.Contains(x));
							}
						}
						dictionary[skip_target] = list;
					}
					foreach (int num3 in dictionary.Keys)
					{
						List<OBDRequest> list3 = dictionary[num3];
						OBDRequestQueueOptimizer.OptimizeRequestsOfHeader(optimized, list3, max_requests, num3);
					}
					return;
				}
			}
			while (reqs_of_header.Count > 0)
			{
				int num4 = reqs_of_header.Count;
				if (num4 == 1)
				{
					optimized.Add(reqs_of_header[0]);
					reqs_of_header.Clear();
				}
				else
				{
					if (num4 > max_requests)
					{
						num4 = max_requests;
					}
					if (num4 > reqs_of_header.Count)
					{
						num4 = reqs_of_header.Count;
					}
					List<OBDRequest> list4 = reqs_of_header.Take(num4).ToList<OBDRequest>();
					int num5 = 1;
					foreach (OBDRequest obdrequest in list4)
					{
						short dataLength = OBDRequestQueueOptimizer.GetDataLength(obdrequest.Command, obdrequest.Header);
						num5 = num5 + (int)dataLength + obdrequest.Command.Length / 2 - 1;
					}
					optimized.Add(new OBDMultiRequest(list4, true, num5)
					{
						SkipCyclesTarget = skipTarget
					});
					reqs_of_header.RemoveRange(0, num4);
				}
			}
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x001DE6CC File Offset: 0x001DC8CC
		private static bool IsOptimizable(string header, string command)
		{
			return (command.StartsWith("01") && header == "" && OBDRequestQueueOptimizer.MainECUSupportedItems.Contains(command) && OBDRequestQueueOptimizer.GetDataLength(command, header) > 0) || (command.StartsWith("22") && OBDRequestQueueOptimizer.GetDataLength(command, header) > 0);
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x001DE726 File Offset: 0x001DC926
		private static Dictionary<string, Dictionary<string, short>> OptimizationDictionary
		{
			get
			{
				if (OBDRequestQueueOptimizer._OptimizationDictionary == null)
				{
					OBDRequestQueueOptimizer.LoadOptimizationDictionary(false);
				}
				return OBDRequestQueueOptimizer._OptimizationDictionary;
			}
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x001DE73C File Offset: 0x001DC93C
		public static short GetDataLength(string command, string header = "")
		{
			short num;
			if ((command.StartsWith("01") || (SharedSettings.Current.CANOptimizeMode22 && command.StartsWith("22"))) && OBDRequestQueueOptimizer.OptimizationDictionary.ContainsKey(header) && OBDRequestQueueOptimizer.OptimizationDictionary[header].TryGetValue(command, out num))
			{
				return num;
			}
			return -1;
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x001DE794 File Offset: 0x001DC994
		private static void LoadOptimizationDictionary(bool forceReload = false)
		{
			if (OBDRequestQueueOptimizer._OptimizationDictionary == null || forceReload)
			{
				OBDRequestQueueOptimizer._OptimizationDictionary = new Dictionary<string, Dictionary<string, short>>();
				Dictionary<string, short> dictionary = OBDRequestQueueOptimizer.BuildOBD2Dictionary();
				OBDRequestQueueOptimizer._OptimizationDictionary.Add("", dictionary);
				try
				{
					Dictionary<string, Dictionary<string, short>> dictionary2 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, short>>>(SharedSettings.Current.CANOptimizeMode22DataLengthDictionary);
					if (dictionary2 != null)
					{
						foreach (string text in dictionary2.Keys)
						{
							if (OBDRequestQueueOptimizer._OptimizationDictionary.ContainsKey(text))
							{
								using (Dictionary<string, short>.KeyCollection.Enumerator enumerator2 = dictionary2[text].Keys.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										string text2 = enumerator2.Current;
										OBDRequestQueueOptimizer._OptimizationDictionary[text][text2] = dictionary2[text][text2];
									}
									continue;
								}
							}
							OBDRequestQueueOptimizer._OptimizationDictionary.Add(text, dictionary2[text]);
						}
					}
				}
				catch (Exception)
				{
					OBDRequestQueueOptimizer._OptimizationDictionary = new Dictionary<string, Dictionary<string, short>>();
				}
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x001DE8C8 File Offset: 0x001DCAC8
		private static Dictionary<string, short> BuildOBD2Dictionary()
		{
			Dictionary<string, short> dictionary = new Dictionary<string, short>();
			dictionary.Add("0100", 4);
			dictionary.Add("0101", 4);
			dictionary.Add("0102", 2);
			dictionary.Add("0103", 2);
			dictionary.Add("0104", 1);
			dictionary.Add("0105", 1);
			dictionary.Add("010A", 1);
			dictionary.Add("010B", 1);
			dictionary.Add("010C", 2);
			if (SharedSettings.Current.SpeedPID2Bytes)
			{
				dictionary.Add("010D", 2);
			}
			else
			{
				dictionary.Add("010D", 1);
			}
			dictionary.Add("010E", 1);
			dictionary.Add("010F", 1);
			dictionary.Add("0110", 2);
			dictionary.Add("0111", 1);
			dictionary.Add("0112", 1);
			dictionary.Add("0113", 1);
			dictionary.Add("0114", 2);
			dictionary.Add("0115", 2);
			dictionary.Add("0116", 2);
			dictionary.Add("0117", 2);
			dictionary.Add("0118", 2);
			dictionary.Add("0119", 2);
			dictionary.Add("011A", 2);
			dictionary.Add("011B", 2);
			dictionary.Add("011C", 1);
			dictionary.Add("011D", 1);
			dictionary.Add("011E", 1);
			dictionary.Add("011F", 2);
			dictionary.Add("0120", 1);
			dictionary.Add("0121", 2);
			dictionary.Add("0122", 2);
			dictionary.Add("0123", 2);
			dictionary.Add("0124", 4);
			dictionary.Add("0125", 4);
			dictionary.Add("0126", 4);
			dictionary.Add("0127", 4);
			dictionary.Add("0128", 4);
			dictionary.Add("0129", 4);
			dictionary.Add("012A", 4);
			dictionary.Add("012B", 4);
			dictionary.Add("012C", 1);
			dictionary.Add("012D", 1);
			dictionary.Add("012E", 1);
			dictionary.Add("012F", 1);
			dictionary.Add("0130", 1);
			dictionary.Add("0131", 2);
			dictionary.Add("0132", 2);
			dictionary.Add("0133", 1);
			dictionary.Add("0134", 4);
			dictionary.Add("0135", 4);
			dictionary.Add("0136", 4);
			dictionary.Add("0137", 4);
			dictionary.Add("0138", 4);
			dictionary.Add("0139", 4);
			dictionary.Add("013A", 4);
			dictionary.Add("013B", 4);
			dictionary.Add("013C", 2);
			dictionary.Add("013D", 2);
			dictionary.Add("013E", 2);
			dictionary.Add("013F", 2);
			dictionary.Add("0140", 1);
			dictionary.Add("0141", 4);
			dictionary.Add("0142", 2);
			dictionary.Add("0143", 2);
			dictionary.Add("0144", 2);
			dictionary.Add("0145", 1);
			dictionary.Add("0146", 1);
			dictionary.Add("0147", 1);
			dictionary.Add("0148", 1);
			dictionary.Add("0149", 1);
			dictionary.Add("014A", 1);
			dictionary.Add("014B", 1);
			dictionary.Add("014C", 1);
			dictionary.Add("014D", 2);
			dictionary.Add("014E", 2);
			dictionary.Add("014F", 4);
			dictionary.Add("0150", 4);
			dictionary.Add("0151", 1);
			dictionary.Add("0152", 1);
			dictionary.Add("0153", 2);
			dictionary.Add("0154", 2);
			dictionary.Add("0159", 2);
			dictionary.Add("015A", 1);
			dictionary.Add("015B", 1);
			dictionary.Add("015C", 1);
			dictionary.Add("015D", 2);
			dictionary.Add("015E", 2);
			dictionary.Add("015F", 1);
			dictionary.Add("0160", 1);
			dictionary.Add("0161", 1);
			dictionary.Add("0162", 1);
			dictionary.Add("0163", 2);
			dictionary.Add("0164", 5);
			dictionary.Add("0165", 2);
			dictionary.Add("017D", 1);
			dictionary.Add("017E", 1);
			dictionary.Add("0180", 1);
			dictionary.Add("0184", 1);
			return dictionary;
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x001DED90 File Offset: 0x001DCF90
		public static async Task ForceLearnOptimizationDictionary()
		{
			PID[] array = LiveDataPIDModel._PIDCollection.Where((PID x) => x != null && x.Command.StartsWith("22", StringComparison.OrdinalIgnoreCase) && x.Command.Length == 6).ToArray<PID>();
			List<PID> list = LiveDataPIDModel._PIDCollection.Where((PID x) => x != null && x.Command.StartsWith("01", StringComparison.OrdinalIgnoreCase) && x.Command.Length == 4 && !OBDRequestQueueOptimizer.IsOptimizable("", x.Command)).ToArray<PID>().Concat(array)
				.ToList<PID>();
			List<OBDRequest> list2 = new List<OBDRequest>();
			foreach (PID pid in list)
			{
				LiveDataPIDModel.GetRequests(pid, list2, null, "");
			}
			bool flag = !SharedSettings.Current.CANOptimizeMode22SelfLearningMode;
			foreach (OBDRequest obdrequest in list2)
			{
				if (flag)
				{
					obdrequest.ResponseDecoded += OBDRequestQueueOptimizer.Req_ResponseDecoded;
				}
				obdrequest.Repeat = false;
			}
			App.OBDReader.ReplaceQueue(list2);
			await Task.Delay(1000);
			await App.OBDReader.WaitForCommandQueue();
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x001DEDCB File Offset: 0x001DCFCB
		public static void ClearOptimizationDictionary()
		{
			SharedSettings.Current.CANOptimizeMode22DataLengthDictionary = "";
			OBDRequestQueueOptimizer.LoadOptimizationDictionary(true);
			OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x001DEDE7 File Offset: 0x001DCFE7
		public static void ClearResponseCounterDictionary()
		{
			OBDRequestQueueOptimizer._KWPResponseCountDictionary = new Dictionary<string, Dictionary<string, int>>();
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x001DEDF4 File Offset: 0x001DCFF4
		private static void Req_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (decodeResult && data.Length != 0)
			{
				if ((request.Header == "6F1" && SharedSettings.Current.SelectedBrand == "BMW") || SharedSettings.Current.SelectedBrand == "Mini")
				{
					OBDRequestQueueOptimizer.RecordDataLength(request.Header, request.Command, -1);
				}
				else
				{
					OBDRequestQueueOptimizer.RecordDataLength(request.Header, request.Command, (short)data.Length);
				}
			}
			request.ResponseDecoded -= OBDRequestQueueOptimizer.Req_ResponseDecoded;
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x001DEE84 File Offset: 0x001DD084
		public static void SetupRequestForLearning(OBDRequest request)
		{
			if (request == null)
			{
				return;
			}
			if (request is OBDMultiRequest)
			{
				return;
			}
			if (request.Command.StartsWith("22") && request.Command.Length == 6)
			{
				request.ResponseDecoded += OBDRequestQueueOptimizer.Req_ResponseDecoded;
				return;
			}
			if (request.Command.StartsWith("01") && request.Command.Length == 4 && !OBDRequestQueueOptimizer.IsOptimizable(request.Header, request.Command))
			{
				request.ResponseDecoded += OBDRequestQueueOptimizer.Req_ResponseDecoded;
				return;
			}
			if (request.Command.StartsWith("10") && request.Command.Length == 4)
			{
				request.ResponseDecoded += OBDRequestQueueOptimizer.Req_ResponseDecoded;
			}
		}

		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x001DEF49 File Offset: 0x001DD149
		public static Dictionary<string, Dictionary<string, int>> KWPResponseCountDictionary
		{
			get
			{
				if (OBDRequestQueueOptimizer._KWPResponseCountDictionary == null)
				{
					OBDRequestQueueOptimizer._KWPResponseCountDictionary = new Dictionary<string, Dictionary<string, int>>();
				}
				return OBDRequestQueueOptimizer._KWPResponseCountDictionary;
			}
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x001DEF64 File Offset: 0x001DD164
		public static string SetExpectedCounterForRequestCommand(OBDRequest request)
		{
			try
			{
				if (!OBDRequestQueueOptimizer.ELMSupportExpectedResponseCount)
				{
					return request.Command;
				}
				if (!SharedSettings.Current.ExpectedResponseCountOptimization)
				{
					return request.Command;
				}
				if (request == null)
				{
					return "";
				}
				if (string.IsNullOrEmpty(request.Command))
				{
					return "";
				}
				if (request.Command.Length < 4)
				{
					return request.Command;
				}
				if (!request.Repeat)
				{
					return request.Command;
				}
				if (request.ELMFormat == ELMFormat.KWP || request.ELMFormat == ELMFormat.CAN11bit || request.ELMFormat == ELMFormat.CAN29bit)
				{
					string text = request.Command.Substring(0, 2);
					if (text == "01" || text == "21" || text == "22" || text == "10")
					{
						if (request.ELMFormat == ELMFormat.KWP && text == "01" && SharedSettings.Current.ExpectedResponseCountOptimizationAlways1ForKWPMode01)
						{
							return request.Command + "1";
						}
						if (OBDRequestQueueOptimizer.KWPResponseCountDictionary.ContainsKey(request.Header))
						{
							int num;
							if (OBDRequestQueueOptimizer.KWPResponseCountDictionary[request.Header].TryGetValue(request.Command, out num))
							{
								if (num >= 1 && num <= 15)
								{
									request.ResponseReceived += OBDRequestQueueOptimizer.ExpectedLengthOptimizedRequestResponseReceived;
									return request.Command + num.ToString("X1");
								}
								return request.Command;
							}
							else
							{
								request.ResponseReceived -= OBDRequestQueueOptimizer.KWPRequest_ResponseReceived;
								request.ResponseReceived += OBDRequestQueueOptimizer.KWPRequest_ResponseReceived;
							}
						}
						else
						{
							OBDRequestQueueOptimizer.KWPResponseCountDictionary.Add(request.Header, new Dictionary<string, int>());
							request.ResponseReceived -= OBDRequestQueueOptimizer.KWPRequest_ResponseReceived;
							request.ResponseReceived += OBDRequestQueueOptimizer.KWPRequest_ResponseReceived;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return request.Command;
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x001DF188 File Offset: 0x001DD388
		private static void ExpectedLengthOptimizedRequestResponseReceived(OBDRequest request, string data)
		{
			request.ResponseReceived -= OBDRequestQueueOptimizer.ExpectedLengthOptimizedRequestResponseReceived;
			if (data.Contains('?'))
			{
				OBDRequestQueueOptimizer.ELMSupportExpectedResponseCount = false;
				Dictionary<string, int> dictionary;
				if (OBDRequestQueueOptimizer.KWPResponseCountDictionary.TryGetValue(request.Header, out dictionary) && dictionary.ContainsKey(request.Command))
				{
					dictionary.Remove(request.Command);
				}
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x001DF1E8 File Offset: 0x001DD3E8
		private static void KWPRequest_ResponseReceived(OBDRequest request, string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return;
			}
			if (data.Contains("NO DATA") || data.Contains("ERROR"))
			{
				request.ResponseReceived -= OBDRequestQueueOptimizer.KWPRequest_ResponseReceived;
				request.ResponseDecoded -= OBDRequestQueueOptimizer.Request_ExpectedDaResponseDecoded;
				return;
			}
			string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			OBDRequestQueueOptimizer.RecordKWPResponsesCounter(request.Header, request.Command, array.Length);
			request.ResponseReceived -= OBDRequestQueueOptimizer.KWPRequest_ResponseReceived;
			request.ResponseDecoded -= OBDRequestQueueOptimizer.Request_ExpectedDaResponseDecoded;
			request.ResponseDecoded += OBDRequestQueueOptimizer.Request_ExpectedDaResponseDecoded;
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x001DF2A4 File Offset: 0x001DD4A4
		private static void Request_ExpectedDaResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (!decodeResult && OBDRequestQueueOptimizer.KWPResponseCountDictionary.ContainsKey(request.Header) && OBDRequestQueueOptimizer.KWPResponseCountDictionary[request.Header].ContainsKey(request.Command))
			{
				OBDRequestQueueOptimizer.KWPResponseCountDictionary[request.Header].Remove(request.Command);
			}
			request.ResponseDecoded -= OBDRequestQueueOptimizer.Request_ExpectedDaResponseDecoded;
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x001DF314 File Offset: 0x001DD514
		public static void RecordKWPResponsesCounter(string header, string pid, int length)
		{
			if (header == null)
			{
				header = "";
			}
			object obj = OBDRequestQueueOptimizer.record_kwp_lock_object;
			lock (obj)
			{
				if (!OBDRequestQueueOptimizer.KWPResponseCountDictionary.ContainsKey(header))
				{
					OBDRequestQueueOptimizer.KWPResponseCountDictionary.TryAdd(header, new Dictionary<string, int>());
				}
				OBDRequestQueueOptimizer.KWPResponseCountDictionary[header][pid] = length;
			}
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x001DF388 File Offset: 0x001DD588
		public static void SetQueueReoptimize(IEnumerable<OBDRequest> queue)
		{
			if (!SharedSettings.Current.AutomaticReoptimization)
			{
				return;
			}
			if (queue == null)
			{
				return;
			}
			OBDRequest obdrequest = queue.LastOrDefault<OBDRequest>();
			if (obdrequest == null)
			{
				return;
			}
			if (queue.All((OBDRequest x) => x is OBDMultiRequest))
			{
				return;
			}
			if (queue.Any((OBDRequest x) => !x.Repeat))
			{
				return;
			}
			if (!obdrequest.Keys.ContainsKey("REOPTIMIZATION_PASSED"))
			{
				obdrequest.ResponseDecoded -= OBDRequestQueueOptimizer.Last_ResponseDecoded;
				obdrequest.ResponseDecoded += OBDRequestQueueOptimizer.Last_ResponseDecoded;
			}
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x001DF438 File Offset: 0x001DD638
		private static void Last_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			request.ResponseDecoded -= OBDRequestQueueOptimizer.Last_ResponseDecoded;
			request.Keys["REOPTIMIZATION_PASSED"] = "1";
			IEnumerable<OBDRequest> enumerable = OBDRequestQueueOptimizer.Optimize(App.OBDReader.GetQueueCopy());
			OBDRequest obdrequest = enumerable.LastOrDefault<OBDRequest>();
			if (obdrequest != null)
			{
				obdrequest.Keys["REOPTIMIZATION_PASSED"] = "1";
			}
			App.OBDReader.ReplaceQueue(enumerable);
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x001DF4A6 File Offset: 0x001DD6A6
		// Note: this type is marked as 'beforefieldinit'.
		static OBDRequestQueueOptimizer()
		{
		}

		// Token: 0x0400151B RID: 5403
		[CompilerGenerated]
		private static HashSet<string> <MainECUSupportedItems>k__BackingField;

		// Token: 0x0400151C RID: 5404
		private static object record_lock_object = new object();

		// Token: 0x0400151D RID: 5405
		private static Dictionary<string, Dictionary<string, short>> _OptimizationDictionary = null;

		// Token: 0x0400151E RID: 5406
		private static Dictionary<string, Dictionary<string, ValueTuple<short, short>>> TemporaryOptimizationDictionary = new Dictionary<string, Dictionary<string, ValueTuple<short, short>>>();

		// Token: 0x0400151F RID: 5407
		private static Dictionary<string, Dictionary<string, int>> _KWPResponseCountDictionary = null;

		// Token: 0x04001520 RID: 5408
		private static bool ELMSupportExpectedResponseCount = true;

		// Token: 0x04001521 RID: 5409
		private static object record_kwp_lock_object = new object();

		// Token: 0x04001522 RID: 5410
		public const string REOPTIMIZATION_PASSED = "REOPTIMIZATION_PASSED";

		// Token: 0x02000392 RID: 914
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060026E1 RID: 9953 RVA: 0x001DF4E2 File Offset: 0x001DD6E2
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060026E2 RID: 9954 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060026E3 RID: 9955 RVA: 0x001DF4EE File Offset: 0x001DD6EE
			internal bool <Optimize>b__7_0(OBDRequest x)
			{
				return x is OBDMultiRequest;
			}

			// Token: 0x060026E4 RID: 9956 RVA: 0x001DF4F9 File Offset: 0x001DD6F9
			internal string <OptimizeV2>b__8_5(OBDRequest x)
			{
				return x.Command;
			}

			// Token: 0x060026E5 RID: 9957 RVA: 0x001DF4F9 File Offset: 0x001DD6F9
			internal string <OptimizeV2>b__8_6(OBDRequest x)
			{
				return x.Command;
			}

			// Token: 0x060026E6 RID: 9958 RVA: 0x001DF501 File Offset: 0x001DD701
			internal bool <OptimizeV2>b__8_7(OBDRequest x)
			{
				return x.Command.StartsWith("01");
			}

			// Token: 0x060026E7 RID: 9959 RVA: 0x001DF513 File Offset: 0x001DD713
			internal bool <OptimizeV2>b__8_8(OBDRequest x)
			{
				return x.Command.StartsWith("22");
			}

			// Token: 0x060026E8 RID: 9960 RVA: 0x001DF525 File Offset: 0x001DD725
			internal bool <OptimizeV2>b__8_0(OBDRequest x)
			{
				return x.Header == "";
			}

			// Token: 0x060026E9 RID: 9961 RVA: 0x001DF537 File Offset: 0x001DD737
			internal bool <OptimizeV2>b__8_1(OBDRequest x)
			{
				return x.Header == "7E0";
			}

			// Token: 0x060026EA RID: 9962 RVA: 0x001DF549 File Offset: 0x001DD749
			internal bool <OptimizeV2>b__8_2(OBDRequest x)
			{
				return x.Header == "7E1";
			}

			// Token: 0x060026EB RID: 9963 RVA: 0x001DF55B File Offset: 0x001DD75B
			internal string <OptimizeV2>b__8_3(OBDRequest x)
			{
				return x.Header;
			}

			// Token: 0x060026EC RID: 9964 RVA: 0x001DF563 File Offset: 0x001DD763
			internal int <OptimizeRequestsOfHeader>b__9_1(OBDRequest x)
			{
				return x.SkipCyclesTarget;
			}

			// Token: 0x060026ED RID: 9965 RVA: 0x00016849 File Offset: 0x00014A49
			internal int <OptimizeRequestsOfHeader>b__9_2(int x)
			{
				return x;
			}

			// Token: 0x060026EE RID: 9966 RVA: 0x001DF56B File Offset: 0x001DD76B
			internal bool <ForceLearnOptimizationDictionary>b__18_0(PID x)
			{
				return x != null && x.Command.StartsWith("22", StringComparison.OrdinalIgnoreCase) && x.Command.Length == 6;
			}

			// Token: 0x060026EF RID: 9967 RVA: 0x001DF593 File Offset: 0x001DD793
			internal bool <ForceLearnOptimizationDictionary>b__18_1(PID x)
			{
				return x != null && x.Command.StartsWith("01", StringComparison.OrdinalIgnoreCase) && x.Command.Length == 4 && !OBDRequestQueueOptimizer.IsOptimizable("", x.Command);
			}

			// Token: 0x060026F0 RID: 9968 RVA: 0x001DF4EE File Offset: 0x001DD6EE
			internal bool <SetQueueReoptimize>b__33_0(OBDRequest x)
			{
				return x is OBDMultiRequest;
			}

			// Token: 0x060026F1 RID: 9969 RVA: 0x001DF5CE File Offset: 0x001DD7CE
			internal bool <SetQueueReoptimize>b__33_1(OBDRequest x)
			{
				return !x.Repeat;
			}

			// Token: 0x04001523 RID: 5411
			public static readonly OBDRequestQueueOptimizer.<>c <>9 = new OBDRequestQueueOptimizer.<>c();

			// Token: 0x04001524 RID: 5412
			public static Func<OBDRequest, bool> <>9__7_0;

			// Token: 0x04001525 RID: 5413
			public static Func<OBDRequest, string> <>9__8_5;

			// Token: 0x04001526 RID: 5414
			public static Func<OBDRequest, string> <>9__8_6;

			// Token: 0x04001527 RID: 5415
			public static Func<OBDRequest, bool> <>9__8_7;

			// Token: 0x04001528 RID: 5416
			public static Func<OBDRequest, bool> <>9__8_8;

			// Token: 0x04001529 RID: 5417
			public static Func<OBDRequest, bool> <>9__8_0;

			// Token: 0x0400152A RID: 5418
			public static Func<OBDRequest, bool> <>9__8_1;

			// Token: 0x0400152B RID: 5419
			public static Func<OBDRequest, bool> <>9__8_2;

			// Token: 0x0400152C RID: 5420
			public static Func<OBDRequest, string> <>9__8_3;

			// Token: 0x0400152D RID: 5421
			public static Func<OBDRequest, int> <>9__9_1;

			// Token: 0x0400152E RID: 5422
			public static Func<int, int> <>9__9_2;

			// Token: 0x0400152F RID: 5423
			public static Func<PID, bool> <>9__18_0;

			// Token: 0x04001530 RID: 5424
			public static Func<PID, bool> <>9__18_1;

			// Token: 0x04001531 RID: 5425
			public static Func<OBDRequest, bool> <>9__33_0;

			// Token: 0x04001532 RID: 5426
			public static Func<OBDRequest, bool> <>9__33_1;
		}

		// Token: 0x02000393 RID: 915
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x060026F2 RID: 9970 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x060026F3 RID: 9971 RVA: 0x001DF5D9 File Offset: 0x001DD7D9
			internal bool <OptimizeV2>b__4(OBDRequest x)
			{
				return x.Header == this.header;
			}

			// Token: 0x04001533 RID: 5427
			public string header;
		}

		// Token: 0x02000394 RID: 916
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x060026F4 RID: 9972 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x060026F5 RID: 9973 RVA: 0x001DF5EC File Offset: 0x001DD7EC
			internal bool <OptimizeRequestsOfHeader>b__0(OBDRequest x)
			{
				return x.SkipCyclesTarget == this.firstRequestSkipTarget;
			}

			// Token: 0x04001534 RID: 5428
			public int firstRequestSkipTarget;
		}

		// Token: 0x02000395 RID: 917
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_1
		{
			// Token: 0x060026F6 RID: 9974 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_1()
			{
			}

			// Token: 0x060026F7 RID: 9975 RVA: 0x001DF5FC File Offset: 0x001DD7FC
			internal bool <OptimizeRequestsOfHeader>b__3(OBDRequest x)
			{
				return x.SkipCyclesTarget == this.skip_target;
			}

			// Token: 0x04001535 RID: 5429
			public int skip_target;
		}

		// Token: 0x02000396 RID: 918
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_2
		{
			// Token: 0x060026F8 RID: 9976 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_2()
			{
			}

			// Token: 0x060026F9 RID: 9977 RVA: 0x001DF60C File Offset: 0x001DD80C
			internal bool <OptimizeRequestsOfHeader>b__4(OBDRequest x)
			{
				return this.items_to_transfer.Contains(x);
			}

			// Token: 0x04001536 RID: 5430
			public IEnumerable<OBDRequest> items_to_transfer;
		}

		// Token: 0x02000397 RID: 919
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ForceLearnOptimizationDictionary>d__18 : IAsyncStateMachine
		{
			// Token: 0x060026FA RID: 9978 RVA: 0x001DF61C File Offset: 0x001DD81C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
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
							goto IL_01DC;
						}
						PID[] array = LiveDataPIDModel._PIDCollection.Where((PID x) => x != null && x.Command.StartsWith("22", StringComparison.OrdinalIgnoreCase) && x.Command.Length == 6).ToArray<PID>();
						List<PID> list = LiveDataPIDModel._PIDCollection.Where((PID x) => x != null && x.Command.StartsWith("01", StringComparison.OrdinalIgnoreCase) && x.Command.Length == 4 && !OBDRequestQueueOptimizer.IsOptimizable("", x.Command)).ToArray<PID>().Concat(array)
							.ToList<PID>();
						List<OBDRequest> list2 = new List<OBDRequest>();
						List<PID>.Enumerator enumerator = list.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								PID pid = enumerator.Current;
								LiveDataPIDModel.GetRequests(pid, list2, null, "");
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						bool flag = !SharedSettings.Current.CANOptimizeMode22SelfLearningMode;
						List<OBDRequest>.Enumerator enumerator2 = list2.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								OBDRequest obdrequest = enumerator2.Current;
								if (flag)
								{
									obdrequest.ResponseDecoded += OBDRequestQueueOptimizer.Req_ResponseDecoded;
								}
								obdrequest.Repeat = false;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						App.OBDReader.ReplaceQueue(list2);
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDRequestQueueOptimizer.<ForceLearnOptimizationDictionary>d__18>(ref taskAwaiter, ref this);
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
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDRequestQueueOptimizer.<ForceLearnOptimizationDictionary>d__18>(ref taskAwaiter, ref this);
						return;
					}
					IL_01DC:
					taskAwaiter.GetResult();
					RequestProducerStatic.UpdateOBDReaderRequests();
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

			// Token: 0x060026FB RID: 9979 RVA: 0x001DF88C File Offset: 0x001DDA8C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001537 RID: 5431
			public int <>1__state;

			// Token: 0x04001538 RID: 5432
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001539 RID: 5433
			private TaskAwaiter <>u__1;
		}
	}
}
