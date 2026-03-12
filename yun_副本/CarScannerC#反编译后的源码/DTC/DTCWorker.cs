using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000562 RID: 1378
	internal static class DTCWorker
	{
		// Token: 0x060032F7 RID: 13047 RVA: 0x0023A7F4 File Offset: 0x002389F4
		public static OBDRequest[] GetRequestsFromStringSequence(IEnumerable<string> sequence)
		{
			List<string> list = new List<string>(sequence);
			List<PID> list2 = new List<PID>();
			List<OBDRequest> list3 = new List<OBDRequest>(sequence.Count<string>());
			for (int i = 0; i < list.Count; i++)
			{
				bool flag = false;
				string text = list[i];
				string text2 = text;
				if (text.StartsWith("_", StringComparison.Ordinal))
				{
					text2 = text.Substring(1);
					flag = true;
				}
				list3.Add(new OBDRequest(text2, false, list2)
				{
					DoNotDecode = !flag
				});
			}
			return list3.ToArray();
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x0023A87C File Offset: 0x00238A7C
		public static async Task Start(IEnumerable<OBDRequest> sequence, bool do_decode, IProgress<string> progress)
		{
			await Task.Run(delegate
			{
				DescriptionLoader.ResetCache();
				DescriptionLoader.PreloadDescriptionsForBrand(string.IsNullOrEmpty(SharedSettings.Current.BrandForDTC) ? SharedSettings.Current.SelectedBrand : SharedSettings.Current.BrandForDTC);
			});
			progress.Report("Clearing queue...");
			await App.OBDReader.ClearRequestQueue();
			progress.Report("Checking connection to ECU...");
			App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
			TaskAwaiter<bool> taskAwaiter = App.OBDReader.CheckECUConnectionWhileRunning(false).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				await App.OBDReader.Stop("DTCWorker Start, No ECU connection");
				progress.Report("No ECU connection, trying to reconnect...");
				await App.OBDReader.ReinitializeConnectionToECU("DTCWorker:Start #65");
				progress.Report("ECU connected...");
			}
			App.OBDReader.CurrentMode = OBDDataReader.OBDModes.ReadDTC;
			progress.Report("Replacing queue...");
			DTCWorker.SetCheckLengthForRequests(sequence);
			App.OBDReader.ReplaceQueue(sequence);
			progress.Report("Replaced queue...");
			await Task.Delay(300);
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x0023A8C8 File Offset: 0x00238AC8
		private static void SetCheckLengthForRequests(IEnumerable<OBDRequest> sequence)
		{
			foreach (OBDRequest obdrequest in sequence)
			{
				if (obdrequest.Command != null && !obdrequest.Command.ToUpperInvariant().StartsWith("AT"))
				{
					obdrequest.CheckLength = true;
				}
			}
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x0023A930 File Offset: 0x00238B30
		public static OBDRequest[] GetProfileSequence(string sequence, bool read)
		{
			if (string.IsNullOrEmpty(sequence))
			{
				return DTCWorker.GetBrandSequence(read);
			}
			OBDRequest[] array;
			try
			{
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && sequence.Contains("[CAN11]") && sequence.Contains("[/CAN11]"))
				{
					sequence = DTCWorker.GetSubstringBetweenTwoStrings(sequence, "[CAN11]", "[/CAN11]");
				}
				else if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit && sequence.Contains("[CAN29]") && sequence.Contains("[/CAN29]"))
				{
					sequence = DTCWorker.GetSubstringBetweenTwoStrings(sequence, "[CAN29START]", "[CAN29END]");
				}
				else if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && sequence.Contains("[KWP]") && sequence.Contains("[/KWP]"))
				{
					sequence = DTCWorker.GetSubstringBetweenTwoStrings(sequence, "[KWP]", "[/KWP]");
				}
				else if (sequence.Contains("[CAN11]") || sequence.Contains("[CAN29]") || sequence.Contains("[KWP]"))
				{
					sequence = "";
				}
				array = DTCWorker.GetRequestsFromStringSequence(sequence.Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries));
			}
			catch (Exception)
			{
				array = new OBDRequest[0];
			}
			return array;
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x0023AA78 File Offset: 0x00238C78
		private static OBDRequest[] GetRequestsFromPattern(string before_pattern, string after_pattern, string[] commands, string[] headers, bool do_decode)
		{
			List<PID> list = new List<PID>(0);
			string[] array = new string[0];
			List<OBDRequest> list2 = new List<OBDRequest>();
			foreach (string text in headers)
			{
				string[] array2;
				if (string.IsNullOrEmpty(before_pattern))
				{
					array2 = new string[0];
				}
				else if (before_pattern.Contains("{0}"))
				{
					array2 = string.Format(before_pattern, text).Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				else
				{
					array2 = before_pattern.Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				string[] array3;
				if (string.IsNullOrEmpty(after_pattern))
				{
					array3 = new string[0];
				}
				else if (after_pattern.Contains("{0}"))
				{
					array3 = string.Format(after_pattern, text).Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				else
				{
					array3 = after_pattern.Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				for (int j = 0; j < commands.Length; j++)
				{
					if (j == 0 && commands.Length == 1)
					{
						OBDRequest obdrequest = new OBDRequest(commands[j], text, array2, array3, false, list)
						{
							DoNotDecode = !do_decode
						};
						list2.Add(obdrequest);
					}
					else if (j == 0)
					{
						OBDRequest obdrequest2 = new OBDRequest(commands[j], text, array2, array, false, list)
						{
							DoNotDecode = !do_decode
						};
						list2.Add(obdrequest2);
					}
					else if (j == commands.Length - 1)
					{
						OBDRequest obdrequest3 = new OBDRequest(commands[j], text, array, array3, false, list)
						{
							DoNotDecode = !do_decode
						};
						list2.Add(obdrequest3);
					}
					else
					{
						OBDRequest obdrequest4 = new OBDRequest(commands[j], text, array, array, false, list)
						{
							DoNotDecode = !do_decode
						};
						list2.Add(obdrequest4);
					}
				}
			}
			return list2.ToArray();
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x0023AC44 File Offset: 0x00238E44
		private static OBDRequest[] GetRequestsFromPatternWithCRA(string before_pattern, string after_pattern, string[] commands, string[] headers, bool do_decode, Func<string, string> responseHeaderFunction)
		{
			List<PID> list = new List<PID>(0);
			new string[0];
			List<OBDRequest> list2 = new List<OBDRequest>();
			foreach (string text in headers)
			{
				string text2 = responseHeaderFunction(text);
				string[] array;
				if (string.IsNullOrEmpty(before_pattern))
				{
					array = new string[0];
				}
				else if (before_pattern.Contains("{0}") && before_pattern.Contains("{1}"))
				{
					array = string.Format(before_pattern, text, text2).Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				else if (before_pattern.Contains("{0}"))
				{
					array = string.Format(before_pattern, text).Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				else
				{
					array = before_pattern.Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				string[] array2;
				if (string.IsNullOrEmpty(after_pattern))
				{
					array2 = new string[0];
				}
				else if (before_pattern.Contains("{0}") && before_pattern.Contains("{1}"))
				{
					array2 = string.Format(after_pattern, text, text2).Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				else if (after_pattern.Contains("{0}"))
				{
					array2 = string.Format(after_pattern, text).Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				else
				{
					array2 = after_pattern.Split(new char[] { ';', '\\' }, StringSplitOptions.RemoveEmptyEntries);
				}
				for (int j = 0; j < commands.Length; j++)
				{
					OBDRequest obdrequest = new OBDRequest(commands[j], text, array, array2, false, list)
					{
						DoNotDecode = !do_decode,
						ELMFormat = ELMFormat.CAN11bit
					};
					list2.Add(obdrequest);
				}
			}
			return list2.ToArray();
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x0023AE0C File Offset: 0x0023900C
		public static void Request_ResponseReceivedCheckForNR78(OBDRequest request, string data)
		{
			try
			{
				string text = "7F" + request.Command.Substring(0, 2) + "78";
				if (data.IndexOf(text) >= 0)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
					{
						string[] array = new string[request.BeforeCommands.Length + 2];
						Array.Copy(request.BeforeCommands, array, request.BeforeCommands.Length);
						array[array.Length - 2] = "ATAT0";
						array[array.Length - 1] = "ATSTFF";
						string[] array2 = new string[request.AfterCommands.Length + 2];
						Array.Copy(request.AfterCommands, array2, request.AfterCommands.Length);
						array2[array2.Length - 2] = "ATAT" + SharedSettings.Current.AdaptiveTimings.ToString();
						string text2 = SharedSettings.Current.GetATST();
						if (string.IsNullOrEmpty(text2))
						{
							text2 = "32";
						}
						array2[array2.Length - 1] = "ATST" + text2;
						OBDRequest obdrequest = new OBDRequest(request.Command, request.Header, array, array2, false)
						{
							DoNotDecode = request.DoNotDecode,
							ELMFormat = request.ELMFormat,
							Payload = request.Payload,
							FailCounter = request.FailCounter,
							OBDMode = OBDDataReader.OBDModes.ReadDTC
						};
						List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
						queueCopy.Insert(0, obdrequest);
						App.OBDReader.ReplaceQueue(queueCopy);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x0023AFAC File Offset: 0x002391AC
		private static OBDRequest[] GetBrandSequence(bool read)
		{
			try
			{
				List<PID> empty_pids = new List<PID>(0);
				string selectedBrand = SharedSettings.Current.SelectedBrand;
				if (selectedBrand != null)
				{
					switch (selectedBrand.Length)
					{
					case 2:
						if (!(selectedBrand == "DS"))
						{
							goto IL_3EEA;
						}
						goto IL_1EEA;
					case 3:
					{
						char c = selectedBrand[0];
						if (c <= 'K')
						{
							if (c != 'B')
							{
								if (c != 'K')
								{
									goto IL_3EEA;
								}
								if (!(selectedBrand == "Kia"))
								{
									goto IL_3EEA;
								}
							}
							else
							{
								if (!(selectedBrand == "BMW"))
								{
									goto IL_3EEA;
								}
								goto IL_2D82;
							}
						}
						else if (c != 'V')
						{
							if (c != 'В')
							{
								goto IL_3EEA;
							}
							if (!(selectedBrand == "ВАЗ"))
							{
								goto IL_3EEA;
							}
							goto IL_351B;
						}
						else
						{
							if (!(selectedBrand == "VAZ"))
							{
								goto IL_3EEA;
							}
							goto IL_351B;
						}
						break;
					}
					case 4:
					{
						char c = selectedBrand[0];
						if (c <= 'F')
						{
							if (c != 'A')
							{
								if (c != 'F')
								{
									goto IL_3EEA;
								}
								if (!(selectedBrand == "Ford"))
								{
									goto IL_3EEA;
								}
								goto IL_2333;
							}
							else
							{
								if (!(selectedBrand == "Audi"))
								{
									goto IL_3EEA;
								}
								goto IL_21EA;
							}
						}
						else
						{
							switch (c)
							{
							case 'J':
								if (!(selectedBrand == "Jeep"))
								{
									goto IL_3EEA;
								}
								goto IL_2A9E;
							case 'K':
								goto IL_3EEA;
							case 'L':
								if (!(selectedBrand == "Lada"))
								{
									goto IL_3EEA;
								}
								goto IL_351B;
							case 'M':
								if (!(selectedBrand == "Mini"))
								{
									goto IL_3EEA;
								}
								goto IL_2D82;
							default:
								if (c != 'S')
								{
									if (c != 'Л')
									{
										goto IL_3EEA;
									}
									if (!(selectedBrand == "Лада"))
									{
										goto IL_3EEA;
									}
									goto IL_351B;
								}
								else
								{
									if (!(selectedBrand == "Seat"))
									{
										goto IL_3EEA;
									}
									goto IL_21EA;
								}
								break;
							}
						}
						break;
					}
					case 5:
					{
						char c = selectedBrand[2];
						if (c <= 'd')
						{
							if (c != 'c')
							{
								if (c != 'd')
								{
									goto IL_3EEA;
								}
								if (!(selectedBrand == "Dodge"))
								{
									goto IL_3EEA;
								}
								goto IL_2A9E;
							}
							else
							{
								if (!(selectedBrand == "Dacia"))
								{
									goto IL_3EEA;
								}
								goto IL_1E07;
							}
						}
						else
						{
							switch (c)
							{
							case 'l':
							{
								if (!(selectedBrand == "Volvo"))
								{
									goto IL_3EEA;
								}
								if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
								{
									goto IL_4320;
								}
								if (read)
								{
									string text = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
									string text2 = "";
									string[] array = new string[]
									{
										"7E0", "7E1", "7E3", "7D6", "7C6", "7C4", "7C1", "7A4", "797", "794",
										"793", "791", "784", "764", "760", "756", "754", "744", "741", "740",
										"737", "736", "733", "731", "730", "727", "726", "720", "707"
									};
									string[] array2 = new string[] { "1902AC", "19028D", "190220", "190208", "190FAC" };
									OBDRequest[] requestsFromPatternWithCRA = DTCWorker.GetRequestsFromPatternWithCRA(text, text2, array2, array, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
									List<OBDRequest> list = new List<OBDRequest>();
									list.Add(new OBDRequest("ATSH7DF", false, empty_pids));
									list.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
									{
										DoNotDecode = !read
									}));
									list.AddRange(array2.Select((string x) => new OBDRequest(x, false, empty_pids)
									{
										DoNotDecode = !read
									}));
									list.Add(DTCWorker.ATSHTest(6));
									list.AddRange(requestsFromPatternWithCRA);
									list.AddRange(DTCWorker.GetRestoreConnectionCommands());
									return list.ToArray();
								}
								string text3 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
								string text4 = "";
								string[] array3 = new string[]
								{
									"7E0", "7E1", "7E3", "7D6", "7C6", "7C4", "7C1", "7A4", "797", "794",
									"793", "791", "784", "764", "760", "756", "754", "744", "741", "740",
									"737", "736", "733", "731", "730", "727", "726", "720", "707"
								};
								string[] array4 = new string[] { "14FFFFFF", "14FF00" };
								OBDRequest[] requestsFromPatternWithCRA2 = DTCWorker.GetRequestsFromPatternWithCRA(text3, text4, array4, array3, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
								List<OBDRequest> list2 = new List<OBDRequest>();
								list2.Add(new OBDRequest("ATSH7DF", false, empty_pids));
								list2.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list2.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list2.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list2.Add(new OBDRequest("14", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list2.Add(new OBDRequest("14FFFFFF", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list2.Add(DTCWorker.ATSHTest(6));
								list2.AddRange(requestsFromPatternWithCRA2);
								list2.AddRange(DTCWorker.GetRestoreConnectionCommands());
								return list2.ToArray();
							}
							case 'm':
							case 'n':
								goto IL_3EEA;
							case 'o':
								if (!(selectedBrand == "Skoda"))
								{
									goto IL_3EEA;
								}
								goto IL_21EA;
							case 'p':
								if (!(selectedBrand == "Cupra"))
								{
									goto IL_3EEA;
								}
								goto IL_21EA;
							default:
								if (c != 'x')
								{
									if (c != 'z')
									{
										goto IL_3EEA;
									}
									if (!(selectedBrand == "Mazda"))
									{
										goto IL_3EEA;
									}
									if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
									{
										goto IL_4320;
									}
									if (read)
									{
										string text5 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
										string text6 = "";
										string[] array5 = new string[]
										{
											"7E0", "7E1", "7E2", "7E3", "720", "726", "730", "731", "734", "737",
											"744", "754", "756", "760", "761", "775", "784", "7A4", "7D2"
										};
										string[] array6 = new string[] { "1800FF00", "1802FF00", "19028F", "1902AC" };
										OBDRequest[] requestsFromPatternWithCRA3 = DTCWorker.GetRequestsFromPatternWithCRA(text5, text6, array6, array5, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
										List<OBDRequest> list3 = new List<OBDRequest>();
										list3.Add(new OBDRequest("ATSH7DF", false, empty_pids));
										list3.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false, empty_pids)
										{
											DoNotDecode = !read
										}));
										list3.AddRange(DTCWorker.DTC_PIDS.Select((string x) => new OBDRequest(x.Substring(1), false, empty_pids)
										{
											DoNotDecode = !read
										}));
										list3.Add(DTCWorker.ATSHTest(6));
										list3.AddRange(requestsFromPatternWithCRA3);
										list3.AddRange(DTCWorker.GetRestoreConnectionCommands());
										return list3.ToArray();
									}
									string text7 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
									string text8 = "";
									string[] array7 = new string[] { "7E0", "7E1", "7E2" };
									string[] array8 = new string[] { "14", "14FF00", "14FFFF", "14FFFFFF" };
									OBDRequest[] requestsFromPatternWithCRA4 = DTCWorker.GetRequestsFromPatternWithCRA(text7, text8, array8, array7, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
									List<OBDRequest> list4 = new List<OBDRequest>();
									list4.Add(new OBDRequest("ATSH7DF", false, empty_pids));
									list4.Add(new OBDRequest("04", false, empty_pids)
									{
										DoNotDecode = !read
									});
									list4.Add(new OBDRequest("04", false, empty_pids)
									{
										DoNotDecode = !read
									});
									list4.Add(new OBDRequest("04", false, empty_pids)
									{
										DoNotDecode = !read
									});
									list4.Add(new OBDRequest("14", false, empty_pids)
									{
										DoNotDecode = !read
									});
									list4.Add(new OBDRequest("14FF00", false, empty_pids)
									{
										DoNotDecode = !read
									});
									list4.Add(new OBDRequest("14FFFF", false, empty_pids)
									{
										DoNotDecode = !read
									});
									list4.Add(new OBDRequest("14FFFFFF", false, empty_pids)
									{
										DoNotDecode = !read
									});
									list4.Add(DTCWorker.ATSHTest(6));
									list4.AddRange(requestsFromPatternWithCRA4);
									list4.AddRange(DTCWorker.GetRestoreConnectionCommands());
									return list4.ToArray();
								}
								else
								{
									if (!(selectedBrand == "Lexus"))
									{
										goto IL_3EEA;
									}
									goto IL_0E6A;
								}
								break;
							}
						}
						break;
					}
					case 6:
					{
						char c = selectedBrand[0];
						if (c != 'N')
						{
							if (c != 'S')
							{
								if (c != 'T')
								{
									goto IL_3EEA;
								}
								if (!(selectedBrand == "Toyota"))
								{
									goto IL_3EEA;
								}
								goto IL_0E6A;
							}
							else
							{
								if (!(selectedBrand == "Subaru"))
								{
									goto IL_3EEA;
								}
								if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
								{
									goto IL_4320;
								}
								if (read)
								{
									string text9 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
									string text10 = "";
									string[] array9 = new string[] { "7E0", "7E1", "7B0", "752", "780", "7C4", "783", "7D5", "782" };
									string[] array10 = new string[] { "1902AF", "1902AF", "1800FF00", "1802FF00" };
									OBDRequest[] requestsFromPatternWithCRA5 = DTCWorker.GetRequestsFromPatternWithCRA(text9, text10, array10, array9, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
									List<OBDRequest> list5 = new List<OBDRequest>();
									list5.Add(new OBDRequest("ATSH7DF", false, empty_pids));
									list5.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
									{
										DoNotDecode = !read
									}));
									list5.AddRange(array10.Select((string x) => new OBDRequest(x, false, empty_pids)
									{
										DoNotDecode = !read
									}));
									list5.Add(DTCWorker.ATSHTest(6));
									list5.AddRange(requestsFromPatternWithCRA5);
									list5.AddRange(DTCWorker.GetRestoreConnectionCommands());
									return list5.ToArray();
								}
								string text11 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
								string text12 = "";
								string[] array11 = new string[] { "7E0", "7E1", "7B0" };
								string[] array12 = new string[] { "14FFFFFF", "14FF00" };
								OBDRequest[] requestsFromPatternWithCRA6 = DTCWorker.GetRequestsFromPatternWithCRA(text11, text12, array12, array11, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
								List<OBDRequest> list6 = new List<OBDRequest>();
								list6.Add(new OBDRequest("ATSH7DF", false, empty_pids));
								list6.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list6.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list6.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list6.Add(new OBDRequest("14", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list6.Add(new OBDRequest("14FFFFFF", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list6.Add(DTCWorker.ATSHTest(6));
								list6.AddRange(requestsFromPatternWithCRA6);
								list6.AddRange(DTCWorker.GetRestoreConnectionCommands());
								return list6.ToArray();
							}
						}
						else
						{
							if (!(selectedBrand == "Nissan"))
							{
								goto IL_3EEA;
							}
							goto IL_3815;
						}
						break;
					}
					case 7:
					{
						char c = selectedBrand[0];
						if (c <= 'M')
						{
							if (c != 'C')
							{
								switch (c)
								{
								case 'G':
									if (!(selectedBrand == "Genesis"))
									{
										goto IL_3EEA;
									}
									break;
								case 'H':
									if (!(selectedBrand == "Hyundai"))
									{
										goto IL_3EEA;
									}
									break;
								case 'I':
								case 'J':
								case 'K':
									goto IL_3EEA;
								case 'L':
									if (!(selectedBrand == "Lincoln"))
									{
										goto IL_3EEA;
									}
									goto IL_2333;
								case 'M':
									if (!(selectedBrand == "Mercury"))
									{
										goto IL_3EEA;
									}
									goto IL_2333;
								default:
									goto IL_3EEA;
								}
							}
							else
							{
								if (!(selectedBrand == "Citroen"))
								{
									goto IL_3EEA;
								}
								goto IL_1EEA;
							}
						}
						else if (c != 'P')
						{
							if (c != 'R')
							{
								goto IL_3EEA;
							}
							if (!(selectedBrand == "Renault"))
							{
								goto IL_3EEA;
							}
							goto IL_1E07;
						}
						else
						{
							if (selectedBrand == "Peugeot")
							{
								goto IL_1EEA;
							}
							if (!(selectedBrand == "Porsche"))
							{
								goto IL_3EEA;
							}
							goto IL_21EA;
						}
						break;
					}
					case 8:
					{
						char c = selectedBrand[7];
						if (c != 'i')
						{
							if (c != 'r')
							{
								if (c != 'y')
								{
									goto IL_3EEA;
								}
								if (!(selectedBrand == "Infinity"))
								{
									goto IL_3EEA;
								}
								goto IL_3815;
							}
							else
							{
								if (!(selectedBrand == "Chrysler"))
								{
									goto IL_3EEA;
								}
								goto IL_2A9E;
							}
						}
						else
						{
							if (!(selectedBrand == "Infiniti"))
							{
								goto IL_3EEA;
							}
							goto IL_3815;
						}
						break;
					}
					case 9:
						goto IL_3EEA;
					case 10:
					{
						char c = selectedBrand[0];
						if (c != 'L')
						{
							if (c != 'M')
							{
								if (c != 'V')
								{
									goto IL_3EEA;
								}
								if (!(selectedBrand == "Volkswagen"))
								{
									goto IL_3EEA;
								}
								goto IL_21EA;
							}
							else
							{
								if (!(selectedBrand == "Mitsubishi"))
								{
									goto IL_3EEA;
								}
								if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
								{
									goto IL_4320;
								}
								if (read)
								{
									string text13 = "ATFCSH{0};ATFCSD300005;ATFCSM1;1092";
									string text14 = "ATFCSH{0};ATFCSD300005;ATFCSM1;ATCRA{1};1092";
									string text15 = "1081";
									string[] array13 = new string[]
									{
										"718", "71A", "724", "72A", "73A", "773", "784", "786", "78A", "78C",
										"790", "792", "79E", "7A0", "7A2", "7A4", "7A6", "7AA", "7B5", "7B6",
										"688", "6A0", "773"
									};
									string[] array14 = new string[] { "7E0", "7E1" };
									string[] array15 = new string[] { "1800FF00" };
									OBDRequest[] requestsFromPattern = DTCWorker.GetRequestsFromPattern(text13, text15, array15, array14, read);
									OBDRequest[] requestsFromPatternWithCRA7 = DTCWorker.GetRequestsFromPatternWithCRA(text14, text15, array15, array13, read, delegate(string req_header)
									{
										int num;
										if (int.TryParse(req_header, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
										{
											return (num + 1).ToString("X3");
										}
										return "";
									});
									List<OBDRequest> list7 = new List<OBDRequest>();
									list7.Add(new OBDRequest("ATSH7DF", false, null)
									{
										DoNotDecode = !read
									});
									list7.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
									{
										DoNotDecode = !read
									}));
									list7.AddRange(array15.Select((string x) => new OBDRequest(x, false, empty_pids)
									{
										DoNotDecode = !read
									}));
									list7.AddRange(requestsFromPattern);
									list7.Add(DTCWorker.ATFCTestRequest(6));
									list7.AddRange(requestsFromPatternWithCRA7);
									list7.AddRange(DTCWorker.GetRestoreConnectionCommands());
									return list7.ToArray();
								}
								string text16 = "ATFCSH{0};ATFCSD300005;ATFCSM1;ATCRA{1};1092";
								string text17 = "1081";
								string[] array16 = new string[]
								{
									"7E0", "7E1", "718", "71A", "724", "72A", "73A", "773", "784", "786",
									"78A", "78C", "790", "792", "79E", "7A0", "7A2", "7A4", "7A6", "7AA",
									"7B5", "7B6"
								};
								string[] array17 = new string[] { "14FF00", "14FFFF", "14FFFFFF" };
								OBDRequest[] requestsFromPatternWithCRA8 = DTCWorker.GetRequestsFromPatternWithCRA(text16, text17, array17, array16, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
								List<OBDRequest> list8 = new List<OBDRequest>();
								list8.Add(new OBDRequest("ATSH7DF", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list8.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list8.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list8.Add(new OBDRequest("04", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list8.Add(new OBDRequest("14", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list8.Add(new OBDRequest("14FF00", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list8.Add(new OBDRequest("14FFFF", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list8.Add(new OBDRequest("14FFFFFF", false, empty_pids)
								{
									DoNotDecode = !read
								});
								list8.AddRange(requestsFromPatternWithCRA8);
								list8.AddRange(DTCWorker.GetRestoreConnectionCommands());
								return list8.ToArray();
							}
						}
						else
						{
							if (!(selectedBrand == "Land Rover"))
							{
								goto IL_3EEA;
							}
							goto IL_3BDD;
						}
						break;
					}
					case 11:
						if (!(selectedBrand == "Range Rover"))
						{
							goto IL_3EEA;
						}
						goto IL_3BDD;
					default:
						goto IL_3EEA;
					}
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					if (read)
					{
						string text18 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1};1003;1090";
						string text19 = "20";
						string[] array18 = new string[]
						{
							"7E0", "7E1", "7E3", "7E4", "7E5", "7E7", "740", "7D0", "7D1", "7D2",
							"7D3", "7D4", "7D5", "7D6", "7D7", "7A0", "7A1", "7A2", "7A3", "7A4",
							"7A5", "7A6", "7A7", "7B3", "7B7", "7C4", "7C6", "7C7", "771"
						};
						string[] array19 = new string[] { "1800FF00", "1800FF00", "1802FF00", "190208", "1902AC", "1902AF" };
						OBDRequest[] requestsFromPatternWithCRA9 = DTCWorker.GetRequestsFromPatternWithCRA(text18, text19, array19, array18, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
						List<OBDRequest> list9 = new List<OBDRequest>();
						list9.Add(new OBDRequest("ATSH7DF", false, empty_pids)
						{
							DoNotDecode = true
						});
						list9.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
						{
							DoNotDecode = !read
						}));
						list9.AddRange(array19.Select((string x) => new OBDRequest(x, false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list9.Add(DTCWorker.ATSHTest(6));
						list9.AddRange(requestsFromPatternWithCRA9);
						list9.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list9.ToArray();
					}
					string text20 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1};1003;1090";
					string text21 = "20";
					string[] array20 = new string[] { "7E0", "7E1", "7E3", "7E4", "7E5", "7E7", "740", "7D6", "7A0", "7D1" };
					string[] array21 = new string[] { "14", "14FF00", "14FFFF", "14FFFFFF" };
					OBDRequest[] requestsFromPatternWithCRA10 = DTCWorker.GetRequestsFromPatternWithCRA(text20, text21, array21, array20, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
					List<OBDRequest> list10 = new List<OBDRequest>();
					list10.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list10.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list10.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list10.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list10.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list10.Add(new OBDRequest("14FF00", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list10.Add(new OBDRequest("14FFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list10.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list10.Add(DTCWorker.ATSHTest(6));
					list10.AddRange(requestsFromPatternWithCRA10);
					list10.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list10.ToArray();
					IL_0E6A:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					string[] array22 = new string[]
					{
						"40", "2A", "B5", "AD", "67", "E9", "B6", "41", "42", "E0",
						"90", "91", "93", "B9", "A1", "A2", "A4", "A7", "B8", "AB",
						"80", "83", "85", "8A", "8B", "8C", "8D", "DC", "DD", "DE",
						"C7", "70", "7C", "38", "36", "39", "3A", "2A", "44", "EC",
						"A5", "A6", "A8", "DB", "DA", "3B", "60", "02", "2C", "51",
						"4F", "B0", "5B", "52", "C8", "7A", "AE", "7B", "AC", "69",
						"26", "5F", "D3", "EB", "6D", "0F", "79", "F3", "0C"
					};
					string[] array26;
					if (read)
					{
						string text22 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
						string text23 = "";
						string[] array23 = new string[]
						{
							"7E0", "7E1", "7E2", "7E3", "760", "720", "700", "7D2", "7C0", "724",
							"726", "705", "747", "745", "703", "781", "790", "791", "7A1", "7B0",
							"7C0", "7B1", "7C4", "741", "780", "792", "7A2", "7A3", "7B4", "7D0"
						};
						string[] array24 = new string[]
						{
							"3E", "1800FF00", "1802FF00", "18FF00", "17FF00", "13FF00", "13FFFF", "13", "1381", "1902AC",
							"190278", "190208", "190FAC"
						};
						OBDRequest[] requestsFromPatternWithCRA11 = DTCWorker.GetRequestsFromPatternWithCRA(text22, text23, array24, array23, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
						foreach (OBDRequest obdrequest in requestsFromPatternWithCRA11.Where((OBDRequest x) => x.Command == "3E" && !x.Header.StartsWith("7E")).ToList<OBDRequest>())
						{
							obdrequest.DoNotDecode = true;
							obdrequest.ResponseReceived += delegate(OBDRequest req, string data)
							{
								if (data == null || !data.Contains("7E"))
								{
									List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
									queueCopy.RemoveAll((OBDRequest x) => x.Header == req.Header);
									App.OBDReader.ReplaceQueue(queueCopy);
								}
							};
						}
						string[] array25 = new string[] { "13FF00", "13FFFF", "13", "1381" };
						List<OBDRequest> list11 = new List<OBDRequest>(array22.Length * (array25.Length + 1));
						List<PID> list12 = new List<PID>(0);
						array26 = array22;
						for (int i = 0; i < array26.Length; i++)
						{
							string text24 = array26[i];
							List<OBDRequest> gate_header_requests2 = new List<OBDRequest>(array25.Length);
							string[] array27 = array25;
							for (int j = 0; j < array27.Length; j++)
							{
								OBDRequest obdrequest2 = new OBDRequest(array27[j], "750", string.Concat(new string[] { "ATCRA758;ATCEA", text24, ";ATTA", text24, ";ATFCSH750;ATFCSD", text24, "300005;ATFCSM1" }), "ATFCSM0;ATCEA;ATAR", false, list12);
								obdrequest2.Payload = text24;
								gate_header_requests2.Add(obdrequest2);
							}
							OBDRequest obdrequest3 = new OBDRequest("3E", "750", string.Concat(new string[] { "ATCRA758;ATCEA", text24, ";ATTA", text24, ";ATFCSH750;ATFCSD", text24, "300005;ATFCSM1" }), "ATFCSM0;ATCEA;ATAR", false, list12);
							obdrequest3.DoNotDecode = true;
							obdrequest3.ResponseReceived += delegate(OBDRequest request, string data)
							{
								if (data == null || !data.Contains("7E"))
								{
									List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
									foreach (OBDRequest obdrequest14 in gate_header_requests2)
									{
										queueCopy2.Remove(obdrequest14);
									}
									App.OBDReader.ReplaceQueue(queueCopy2);
								}
							};
							list11.Add(obdrequest3);
							list11.AddRange(gate_header_requests2);
						}
						List<OBDRequest> list13 = new List<OBDRequest>();
						list13.Add(new OBDRequest("ATSH7DF", false, empty_pids));
						list13.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
						{
							DoNotDecode = !read
						}));
						list13.AddRange(array24.Select((string x) => new OBDRequest(x, false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list13.Add(DTCWorker.ATSHTest(6));
						list13.Add(DTCWorker.ATFCTestRequest(6));
						list13.AddRange(requestsFromPatternWithCRA11);
						list13.AddRange(list11);
						list13.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list13.ToArray();
					}
					string text25 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
					string text26 = "";
					string[] array28 = new string[] { "7E0", "7E1", "7E2", "7E3", "760", "720", "705", "747", "7B0" };
					string[] array29 = new string[] { "3E", "14", "14FF00", "14FFFF", "14FFFFFF" };
					OBDRequest[] requestsFromPatternWithCRA12 = DTCWorker.GetRequestsFromPatternWithCRA(text25, text26, array29, array28, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
					foreach (OBDRequest obdrequest4 in requestsFromPatternWithCRA12.Where((OBDRequest x) => x.Command == "3E" && !x.Header.StartsWith("7E")).ToList<OBDRequest>())
					{
						obdrequest4.DoNotDecode = true;
						obdrequest4.ResponseReceived += delegate(OBDRequest req, string data)
						{
							if (data == null || !data.Contains("7E"))
							{
								List<OBDRequest> queueCopy3 = App.OBDReader.GetQueueCopy();
								queueCopy3.RemoveAll((OBDRequest x) => x.Header == req.Header);
								App.OBDReader.ReplaceQueue(queueCopy3);
							}
						};
					}
					List<OBDRequest> list14 = new List<OBDRequest>(array22.Length * array29.Length);
					List<PID> list15 = new List<PID>(0);
					array26 = array22;
					for (int i = 0; i < array26.Length; i++)
					{
						string text27 = array26[i];
						List<OBDRequest> gate_header_requests = new List<OBDRequest>(array29.Length);
						for (int k = 1; k < array29.Length; k++)
						{
							OBDRequest obdrequest5 = new OBDRequest(array29[k], "750", string.Concat(new string[] { "ATCRA758;ATCEA", text27, ";ATTA", text27, ";ATFCSH750;ATFCSD", text27, "300005;ATFCSM1" }), "ATFCSM0;ATCEA;ATAR", false, list15);
							obdrequest5.Payload = text27;
							gate_header_requests.Add(obdrequest5);
						}
						OBDRequest obdrequest6 = new OBDRequest("3E", "750", string.Concat(new string[] { "ATCRA758;ATCEA", text27, ";ATTA", text27, ";ATFCSH750;ATFCSD", text27, "300005;ATFCSM1" }), "ATFCSM0;ATCEA;ATAR", false, list15);
						obdrequest6.DoNotDecode = true;
						obdrequest6.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data == null || !data.Contains("7E"))
							{
								List<OBDRequest> queueCopy4 = App.OBDReader.GetQueueCopy();
								foreach (OBDRequest obdrequest15 in gate_header_requests)
								{
									queueCopy4.Remove(obdrequest15);
								}
								App.OBDReader.ReplaceQueue(queueCopy4);
							}
						};
						list14.Add(obdrequest6);
						list14.AddRange(gate_header_requests);
					}
					List<OBDRequest> list16 = new List<OBDRequest>();
					list16.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list16.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list16.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list16.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list16.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list16.Add(new OBDRequest("14FF00", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list16.Add(new OBDRequest("14FFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list16.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list16.Add(DTCWorker.ATSHTest(6));
					list16.AddRange(requestsFromPatternWithCRA12);
					list16.AddRange(list14);
					list16.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list16.ToArray();
					IL_1E07:
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
					{
						return DTCWorker.GetRenaultCANRequests(read);
					}
					if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
					{
						OBDRequest[] classicCommands = DTCWorker.GetClassicCommands(3, read);
						List<OBDRequest> list17 = DTCWorker.GetRenaultCANRequests(read).ToList<OBDRequest>();
						list17.RemoveAll((OBDRequest x) => x.Command == "03" || x.Command == "07" || x.Command == "0A" || x.Command == "04" || x.Header == "");
						List<OBDRequest> list18 = new List<OBDRequest>(classicCommands.Length + 2 + list17.Count);
						list18.AddRange(classicCommands);
						if (read)
						{
							list18.Add(DTCWorker.ATFCTestRequest(6));
						}
						else
						{
							list18.Add(DTCWorker.ATSHTest(6));
						}
						list18.AddRange(list17);
						return list18.ToArray();
					}
					goto IL_4320;
					IL_1EEA:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					if (read)
					{
						string text28 = "ATFCSH{0};ATFCSD300005;ATFCSM1;81;10C0";
						string text29 = "";
						string[] array30 = new string[] { "7E0", "7E1", "6A8", "6A9" };
						string[] array31 = new string[] { "17FF00", "1902AF", "1902AF" };
						OBDRequest[] requestsFromPatternWithCRA13 = DTCWorker.GetRequestsFromPatternWithCRA(text28, text29, array31, array30, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
						List<OBDRequest> list19 = new List<OBDRequest>();
						list19.Add(new OBDRequest("ATSH7DF", false, empty_pids));
						list19.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
						{
							DoNotDecode = !read
						}));
						list19.AddRange(array31.Select((string x) => new OBDRequest(x, false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list19.Add(DTCWorker.ATFCTestRequest(6));
						list19.AddRange(requestsFromPatternWithCRA13);
						list19.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list19.ToArray();
					}
					string text30 = "ATFCSH{0};ATFCSD300005;ATFCSM1;81;10C0";
					string text31 = "";
					string[] array32 = new string[] { "7E0", "7E1", "6A8", "6A9" };
					string[] array33 = new string[] { "14FFFFFF", "14FF00" };
					OBDRequest[] requestsFromPatternWithCRA14 = DTCWorker.GetRequestsFromPatternWithCRA(text30, text31, array33, array32, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
					List<OBDRequest> list20 = new List<OBDRequest>();
					list20.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list20.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list20.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list20.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list20.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list20.Add(new OBDRequest("14FF00", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list20.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list20.Add(DTCWorker.ATSHTest(6));
					list20.AddRange(requestsFromPatternWithCRA14);
					list20.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list20.ToArray();
					IL_21EA:
					if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
					{
						OBDRequest[] vagkwprequests = DTCWorker.GetVAGKWPRequests(read);
						List<OBDRequest> can_requests = DTCWorker.GetVAGCanRequests(read).ToList<OBDRequest>();
						can_requests.RemoveAll((OBDRequest x) => x.Command == "03" || x.Command == "07" || x.Command == "0A" || x.Command == "04" || x.Header == "" || x.Header == "7E0" || x.Header == "7E1");
						Predicate<OBDRequest> <>9__31;
						ResponseReceivedDelegate <>9__30;
						foreach (OBDRequest obdrequest7 in can_requests)
						{
							obdrequest7.ELMFormat = ELMFormat.CAN11bit;
							ResponseReceivedDelegate responseReceivedDelegate;
							if ((responseReceivedDelegate = <>9__30) == null)
							{
								responseReceivedDelegate = (<>9__30 = delegate(OBDRequest request, string data)
								{
									if (data.Contains("CAN ERROR"))
									{
										List<OBDRequest> queueCopy5 = App.OBDReader.GetQueueCopy();
										List<OBDRequest> list36 = queueCopy5;
										Predicate<OBDRequest> predicate;
										if ((predicate = <>9__31) == null)
										{
											predicate = (<>9__31 = (OBDRequest x) => can_requests.Contains(x));
										}
										list36.RemoveAll(predicate);
										App.OBDReader.ReplaceQueue(queueCopy5);
									}
								});
							}
							obdrequest7.ResponseReceived += responseReceivedDelegate;
						}
						List<OBDRequest> list21 = new List<OBDRequest>(vagkwprequests.Length + can_requests.Count + 3);
						list21.AddRange(vagkwprequests);
						list21.Add(new OBDRequest("ATSP6", false)
						{
							DoNotDecode = true
						});
						list21.AddRange(can_requests);
						return list21.ToArray();
					}
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
					{
						return DTCWorker.GetVAGCanRequests(read);
					}
					goto IL_4320;
					IL_2333:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					if (read)
					{
						string text32 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
						string text33 = "";
						string[] array34 = new string[] { "7E0", "7E1", "7E2", "7E3", "760", "720" };
						string[] array35 = new string[]
						{
							"1800FF00", "1802FF00", "18FF00", "17FF00", "13FF00", "1902AF", "1902AC", "19028D", "190223", "190278",
							"190208", "190FAC", "190F8D", "190F23"
						};
						OBDRequest[] requestsFromPatternWithCRA15 = DTCWorker.GetRequestsFromPatternWithCRA(text32, text33, array35, array34, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
						List<OBDRequest> list22 = new List<OBDRequest>();
						list22.Add(new OBDRequest("ATSH7DF", false, empty_pids));
						list22.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list22.AddRange(DTCWorker.DTC_PIDS.Select((string x) => new OBDRequest(x.Substring(1), false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list22.Add(DTCWorker.ATSHTest(6));
						list22.AddRange(requestsFromPatternWithCRA15);
						list22.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list22.ToArray();
					}
					string text34 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
					string text35 = "";
					string[] array36 = new string[] { "7E0", "7E1", "7E2", "7E3", "720" };
					string[] array37 = new string[] { "14", "14FF00", "14FFFF", "14FFFFFF" };
					OBDRequest[] requestsFromPatternWithCRA16 = DTCWorker.GetRequestsFromPatternWithCRA(text34, text35, array37, array36, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
					List<OBDRequest> list23 = new List<OBDRequest>();
					list23.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list23.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list23.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list23.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list23.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list23.Add(new OBDRequest("14FF00", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list23.Add(new OBDRequest("14FFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list23.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list23.Add(DTCWorker.ATSHTest(6));
					list23.AddRange(requestsFromPatternWithCRA16);
					list23.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list23.ToArray();
					IL_2A9E:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					if (read)
					{
						string text36 = "";
						string text37 = "";
						string[] array38 = new string[] { "7E0", "7E1" };
						string[] array39 = new string[] { "1800FF00", "190208", "1902AC" };
						OBDRequest[] requestsFromPattern2 = DTCWorker.GetRequestsFromPattern(text36, text37, array39, array38, read);
						List<OBDRequest> list24 = new List<OBDRequest>();
						list24.Add(new OBDRequest("ATSH7DF", false, empty_pids)
						{
							DoNotDecode = true
						});
						list24.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
						{
							DoNotDecode = !read
						}));
						list24.AddRange(array39.Select((string x) => new OBDRequest(x, false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list24.Add(DTCWorker.ATSHTest(6));
						list24.AddRange(requestsFromPattern2);
						list24.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list24.ToArray();
					}
					string text38 = "";
					string text39 = "";
					string[] array40 = new string[] { "7E0", "7E1" };
					string[] array41 = new string[] { "14", "14FF00", "14FFFF", "14FFFFFF" };
					OBDRequest[] requestsFromPattern3 = DTCWorker.GetRequestsFromPattern(text38, text39, array41, array40, read);
					List<OBDRequest> list25 = new List<OBDRequest>();
					list25.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list25.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list25.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list25.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list25.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list25.Add(new OBDRequest("14FF00", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list25.Add(new OBDRequest("14FFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list25.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list25.Add(DTCWorker.ATSHTest(6));
					list25.AddRange(requestsFromPattern3);
					list25.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list25.ToArray();
					IL_2D82:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					if (read)
					{
						string[] array42 = new int[]
						{
							0, 1, 6, 8, 11, 13, 15, 16, 18, 19,
							22, 24, 25, 27, 30, 28, 32, 33, 34, 36,
							38, 39, 41, 42, 44, 43, 48, 49, 50, 53,
							54, 55, 56, 58, 59, 60, 61, 63, 64, 66,
							67, 68, 69, 71, 72, 73, 74, 75, 78, 77,
							83, 84, 85, 86, 87, 89, 90, 91, 92, 93,
							94, 95, 96, 97, 98, 99, 100, 104, 105, 106,
							107, 109, 110, 114, 115, 116, 117, 118, 120, 121,
							146, 160, 173, 174, 239
						}.Select((int x) => x.ToString("X2")).ToArray<string>();
						List<OBDRequest> list26 = new List<OBDRequest>();
						list26.Add(new OBDRequest("03", false));
						list26.Add(new OBDRequest("07", false));
						list26.Add(new OBDRequest("0A", false));
						list26.AddRange(DTCWorker.DTC_PIDS.Select((string x) => new OBDRequest(x.Substring(1), false)).ToArray<OBDRequest>());
						list26.Add(new OBDRequest("ATTAF1", false)
						{
							DoNotDecode = true
						});
						list26.Add(new OBDRequest("ATFCSH6F1", false)
						{
							DoNotDecode = true
						});
						list26.Add(new OBDRequest("ATFCSD00300010", false)
						{
							DoNotDecode = true
						});
						list26.Add(new OBDRequest("ATFCSM1", false)
						{
							DoNotDecode = true
						});
						OBDRequest obdrequest8 = new OBDRequest("ATCEA", false)
						{
							DoNotDecode = true
						};
						OBDRequest obdrequest9 = DTCWorker.ATFCTestRequest(6);
						list26.Add(obdrequest9);
						string[] array43 = new string[] { "1902AC", "1902AF", "1800FFFF", "1802FFFF" };
						foreach (string text40 in array42)
						{
							string[] array44 = new string[]
							{
								"ATPBC101",
								"ATSPB",
								"ATCRA6" + text40,
								"ATCEA" + text40,
								"ATFCSD" + text40 + "300010",
								"ATFCSH6F1",
								"ATFCSM1"
							};
							string[] array27 = array43;
							for (int j = 0; j < array27.Length; j++)
							{
								OBDRequest obdrequest10 = new OBDRequest(array27[j], "6F1", array44, new string[0], false, null);
								list26.Add(obdrequest10);
							}
						}
						list26.Add(new OBDRequest("ATFCSM0", "", "ATSTDEF", "ATSTDEF", false)
						{
							DoNotDecode = true
						});
						list26.Add(obdrequest8);
						return list26.ToArray();
					}
					string[] array45 = new int[] { 11, 18, 24, 25, 27, 30, 28, 32, 41, 42 }.Select((int x) => x.ToString("X2")).ToArray<string>();
					List<OBDRequest> list27 = new List<OBDRequest>();
					list27.Add(new OBDRequest("04", false));
					list27.Add(new OBDRequest("04", false));
					list27.Add(new OBDRequest("04", false));
					list27.AddRange(DTCWorker.CLEAR_DTC.Select((string x) => new OBDRequest(x, false)).ToArray<OBDRequest>());
					list27.Add(new OBDRequest("ATTAF1", false)
					{
						DoNotDecode = true
					});
					list27.Add(new OBDRequest("ATFCSH6F1", false)
					{
						DoNotDecode = true
					});
					list27.Add(new OBDRequest("ATFCSD00300010", false)
					{
						DoNotDecode = true
					});
					list27.Add(new OBDRequest("ATFCSM1", false)
					{
						DoNotDecode = true
					});
					OBDRequest obdrequest11 = new OBDRequest("ATCEA", false)
					{
						DoNotDecode = true
					};
					OBDRequest obdrequest12 = DTCWorker.ATFCTestRequest(6);
					list27.Add(obdrequest12);
					string[] array46 = new string[] { "14FFFFFF" };
					foreach (string text41 in array45)
					{
						string[] array47 = new string[]
						{
							"ATCRA6" + text41,
							"ATCEA" + text41,
							"ATFCSD" + text41 + "300010",
							"ATFCSH6F1",
							"ATFCSM1"
						};
						string[] array27 = array46;
						for (int j = 0; j < array27.Length; j++)
						{
							OBDRequest obdrequest13 = new OBDRequest(array27[j], "6F1", array47, new string[0], false, null);
							list27.Add(obdrequest13);
						}
					}
					list27.Add(new OBDRequest("ATFCSM0", false)
					{
						DoNotDecode = true
					});
					list27.Add(obdrequest11);
					list27.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list27.ToArray();
					IL_351B:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					if (read)
					{
						string text42 = "ATFCSH{0};ATFCSD300000;ATFCSM1";
						string text43 = "";
						string[] array48 = new string[]
						{
							"7E0", "7E1", "7E2", "7E3", "7E4", "7E5", "7E6", "7E7", "752", "740",
							"748"
						};
						string[] array49 = new string[] { "1902AF", "190209", "19020B", "1802FF00", "1800FF00" };
						OBDRequest[] requestsFromPattern4 = DTCWorker.GetRequestsFromPattern(text42, text43, array49, array48, read);
						List<OBDRequest> list28 = new List<OBDRequest>();
						list28.Add(new OBDRequest("ATSH7DF", false, empty_pids));
						list28.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
						{
							DoNotDecode = !read
						}));
						list28.AddRange(array49.Select((string x) => new OBDRequest(x, false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list28.Add(DTCWorker.ATSHTest(6));
						list28.AddRange(requestsFromPattern4);
						list28.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list28.ToArray();
					}
					string text44 = "";
					string text45 = "";
					string[] array50 = new string[] { "7E0", "7E1", "7E3", "7E5", "752", "740", "748" };
					string[] array51 = new string[] { "14FFFFFF", "14FF00" };
					OBDRequest[] requestsFromPattern5 = DTCWorker.GetRequestsFromPattern(text44, text45, array51, array50, read);
					List<OBDRequest> list29 = new List<OBDRequest>();
					list29.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list29.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list29.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list29.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list29.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list29.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list29.Add(DTCWorker.ATSHTest(6));
					list29.AddRange(requestsFromPattern5);
					list29.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list29.ToArray();
					IL_3815:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					if (read)
					{
						string text46 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1};10C0";
						string text47 = "";
						string[] array52 = new string[]
						{
							"7E0", "7E1", "7E5", "7A4", "752", "759", "75F", "740", "742", "743",
							"744", "745", "747", "748", "74D", "74C", "74F", "70C", "758", "75B"
						};
						string[] array53 = new string[] { "17FF00", "1902AF", "1902AF" };
						OBDRequest[] requestsFromPatternWithCRA17 = DTCWorker.GetRequestsFromPatternWithCRA(text46, text47, array53, array52, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
						List<OBDRequest> list30 = new List<OBDRequest>();
						list30.Add(new OBDRequest("ATSH7DF", false, empty_pids));
						list30.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
						{
							DoNotDecode = !read
						}));
						list30.AddRange(array53.Select((string x) => new OBDRequest(x, false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list30.Add(DTCWorker.ATFCTestRequest(6));
						list30.AddRange(requestsFromPatternWithCRA17);
						list30.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list30.ToArray();
					}
					string text48 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1};10C0";
					string text49 = "";
					string[] array54 = new string[]
					{
						"7E0", "7E1", "7E5", "7A4", "752", "759", "75F", "740", "742", "747",
						"748", "74F", "70C", "758", "75B"
					};
					string[] array55 = new string[] { "14FFFFFF", "14FF00", "14" };
					OBDRequest[] requestsFromPatternWithCRA18 = DTCWorker.GetRequestsFromPatternWithCRA(text48, text49, array55, array54, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
					List<OBDRequest> list31 = new List<OBDRequest>();
					list31.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list31.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list31.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list31.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list31.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list31.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list31.Add(DTCWorker.ATSHTest(6));
					list31.AddRange(requestsFromPatternWithCRA18);
					list31.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list31.ToArray();
					IL_3BDD:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_4320;
					}
					if (read)
					{
						string text50 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
						string text51 = "";
						string[] array56 = new string[] { "7E0", "7E1", "795", "761", "760", "792", "7D3" };
						string[] array57 = new string[] { "17FF00", "1902AF", "1902AF" };
						OBDRequest[] requestsFromPatternWithCRA19 = DTCWorker.GetRequestsFromPatternWithCRA(text50, text51, array57, array56, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
						List<OBDRequest> list32 = new List<OBDRequest>();
						list32.Add(new OBDRequest("ATSH7DF", false, empty_pids));
						list32.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
						{
							DoNotDecode = !read
						}));
						list32.AddRange(array57.Select((string x) => new OBDRequest(x, false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list32.Add(DTCWorker.ATSHTest(6));
						list32.AddRange(requestsFromPatternWithCRA19);
						list32.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list32.ToArray();
					}
					string text52 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1}";
					string text53 = "";
					string[] array58 = new string[] { "7E0", "7E1", "795", "761", "760", "792", "7D3" };
					string[] array59 = new string[] { "14FFFFFF", "14FF00", "14" };
					OBDRequest[] requestsFromPatternWithCRA20 = DTCWorker.GetRequestsFromPatternWithCRA(text52, text53, array59, array58, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
					List<OBDRequest> list33 = new List<OBDRequest>();
					list33.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list33.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list33.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list33.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list33.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list33.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list33.Add(DTCWorker.ATSHTest(6));
					list33.AddRange(requestsFromPatternWithCRA20);
					list33.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list33.ToArray();
				}
				IL_3EEA:
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					if (read)
					{
						string text54 = "ATFCSH{0};ATFCSD300000;ATFCSM1";
						string text55 = "";
						string[] array60 = new string[] { "7E0", "7E1", "7E2", "7E3", "760", "720" };
						string[] array61 = new string[]
						{
							"1800FF00", "1802FF00", "18FF00", "17FF00", "13FF00", "1902AF", "1902AC", "19028D", "190223", "190278",
							"190208", "190FAC", "190F8D", "190F23"
						};
						OBDRequest[] requestsFromPattern6 = DTCWorker.GetRequestsFromPattern(text54, text55, array61, array60, read);
						List<OBDRequest> list34 = new List<OBDRequest>();
						list34.Add(new OBDRequest("ATSH7DF", false, empty_pids));
						list34.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list34.AddRange(DTCWorker.DTC_PIDS.Select((string x) => new OBDRequest(x.Substring(1), false, empty_pids)
						{
							DoNotDecode = !read
						}));
						list34.Add(new OBDRequest("03", "7E0", "", "", false));
						list34.Add(new OBDRequest("07", "7E0", "", "", false));
						list34.Add(new OBDRequest("03", "7E1", "", "", false));
						list34.Add(new OBDRequest("07", "7E1", "", "", false));
						list34.Add(new OBDRequest("03", "7E2", "", "", false));
						list34.Add(new OBDRequest("07", "7E2", "", "", false));
						list34.Add(DTCWorker.ATSHTest(6));
						list34.AddRange(requestsFromPattern6);
						list34.AddRange(DTCWorker.GetRestoreConnectionCommands());
						return list34.ToArray();
					}
					string text56 = "";
					string text57 = "";
					string[] array62 = new string[] { "7E0", "7E1", "7E2", "7E3", "760", "720" };
					string[] array63 = new string[] { "14", "14FF00", "14FFFF", "14FFFFFF" };
					OBDRequest[] requestsFromPattern7 = DTCWorker.GetRequestsFromPattern(text56, text57, array63, array62, read);
					List<OBDRequest> list35 = new List<OBDRequest>();
					list35.Add(new OBDRequest("ATSH7DF", false, empty_pids));
					list35.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list35.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list35.Add(new OBDRequest("04", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list35.Add(new OBDRequest("14", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list35.Add(new OBDRequest("14FF00", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list35.Add(new OBDRequest("14FFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list35.Add(new OBDRequest("14FFFFFF", false, empty_pids)
					{
						DoNotDecode = !read
					});
					list35.Add(DTCWorker.ATSHTest(6));
					list35.AddRange(requestsFromPattern7);
					list35.AddRange(DTCWorker.GetRestoreConnectionCommands());
					return list35.ToArray();
				}
				IL_4320:;
			}
			catch
			{
			}
			return new OBDRequest[0];
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x0023F34C File Offset: 0x0023D54C
		public static OBDRequest[] GetRestoreConnectionCommands()
		{
			return new OBDRequest[]
			{
				new OBDRequest("ATSH" + App.OBDReader.GetDefaultHeader(), false)
				{
					DoNotDecode = true
				},
				new OBDRequest("ATAR", false)
				{
					DoNotDecode = true
				},
				new OBDRequest("ATFCSM0", false)
				{
					DoNotDecode = true
				},
				new OBDRequest("ATCAF1", false)
				{
					DoNotDecode = true
				},
				new OBDRequest("ATCEA", false)
				{
					DoNotDecode = true
				},
				new OBDRequest("ATST" + SharedSettings.Current.GetATST(), false)
				{
					DoNotDecode = true
				}
			};
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x0023F3FC File Offset: 0x0023D5FC
		private static OBDRequest[] GetRenaultCANRequests(bool read)
		{
			List<PID> empty_pids = new List<PID>(0);
			if (read)
			{
				string text = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1};10C0";
				string text2 = "";
				string[] array = new string[] { "7E0", "7E1", "752", "740", "748" };
				string[] array2 = new string[] { "17FF00", "1902AF", "1902AF" };
				OBDRequest[] requestsFromPatternWithCRA = DTCWorker.GetRequestsFromPatternWithCRA(text, text2, array2, array, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
				List<OBDRequest> list = new List<OBDRequest>();
				list.Add(new OBDRequest("ATSH7DF", false, empty_pids));
				list.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !read
				}));
				list.AddRange(array2.Select((string x) => new OBDRequest(x, false, empty_pids)
				{
					DoNotDecode = !read
				}));
				list.Add(DTCWorker.ATFCTestRequest(6));
				list.AddRange(requestsFromPatternWithCRA);
				list.AddRange(DTCWorker.GetRestoreConnectionCommands());
				return list.ToArray();
			}
			string text3 = "ATFCSH{0};ATFCSD300000;ATFCSM1;ATCRA{1};10C0";
			string text4 = "";
			string[] array3 = new string[] { "7E0", "7E1", "740", "748", "752" };
			string[] array4 = new string[] { "14FFFFFF", "14FF00" };
			OBDRequest[] requestsFromPatternWithCRA2 = DTCWorker.GetRequestsFromPatternWithCRA(text3, text4, array4, array3, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
			List<OBDRequest> list2 = new List<OBDRequest>();
			list2.Add(new OBDRequest("ATSH7DF", false, empty_pids));
			list2.Add(new OBDRequest("04", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("04", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("04", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("14", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("14FF00", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("14FFFFFF", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(DTCWorker.ATSHTest(6));
			list2.AddRange(requestsFromPatternWithCRA2);
			list2.AddRange(DTCWorker.GetRestoreConnectionCommands());
			return list2.ToArray();
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x0023F6E0 File Offset: 0x0023D8E0
		private static OBDRequest[] GetVAGKWPRequests(bool read)
		{
			List<PID> list = new List<PID>(0);
			string[] commonHeaders = DTCWorker.GetCommonHeaders();
			string[] array;
			if (read)
			{
				array = new string[] { "1800FF00", "1802FF00", "1802FFFF", "1800FFFF", "18FF00" };
			}
			else
			{
				array = new string[] { "14", "14FF00", "14FFFF" };
			}
			List<OBDRequest> list2 = new List<OBDRequest>();
			foreach (string text in commonHeaders)
			{
				if (text == "")
				{
					if (read)
					{
						list2.Add(new OBDRequest("03", "", "", "", false, list));
						list2.Add(new OBDRequest("03", "", "", "", false, list));
						list2.Add(new OBDRequest("07", "", "", "", false, list));
						list2.Add(new OBDRequest("07", "", "", "", false, list));
						list2.Add(new OBDRequest("0A", "", "", "", false, list));
					}
					else
					{
						list2.Add(new OBDRequest("04", "", "", "", false, list)
						{
							DoNotDecode = true
						});
						list2.Add(new OBDRequest("04", "", "", "", false, list)
						{
							DoNotDecode = true
						});
						list2.Add(new OBDRequest("04", "", "", "", false, list)
						{
							DoNotDecode = true
						});
					}
				}
				foreach (string text2 in array)
				{
					list2.Add(new OBDRequest(text2, text, "", "", false, list)
					{
						DoNotDecode = !read
					});
				}
			}
			return list2.ToArray();
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x0023F8F4 File Offset: 0x0023DAF4
		private static OBDRequest ATFCTestRequest(int protocol = 6)
		{
			OBDRequest obdrequest = new OBDRequest("ATFCSH7E0", "", new string[] { "ATSP" + protocol.ToString("X1") }, new string[0], false);
			obdrequest.DoNotDecode = true;
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.BeforeCommands.Any((string cmd) => cmd.StartsWith("ATFCSH")));
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			};
			return obdrequest;
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x0023F964 File Offset: 0x0023DB64
		private static OBDRequest ATSHTest(int protocol = 6)
		{
			OBDRequest obdrequest = new OBDRequest("ATSH" + App.OBDReader.GetDefaultHeader(), "", new string[] { "ATSP" + protocol.ToString("X1") }, new string[0], false);
			obdrequest.DoNotDecode = true;
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.Header != "");
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			};
			return obdrequest;
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x0023F9E4 File Offset: 0x0023DBE4
		private static OBDRequest[] GetVAGCanRequests(bool read)
		{
			List<PID> empty_pids = new List<PID>(0);
			if (read)
			{
				string text = "ATFCSH{0};ATFCSD300005;ATFCSM1;ATCRA{1};1003";
				if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
				{
					text = "ATSP6;" + text;
				}
				string[] array = new string[]
				{
					"7E0", "7E1", "713", "732", "74D", "746", "70E", "770", "70A", "7E2",
					"772", "715", "70C", "714", "76A", "716", "728", "70F", "73B", "711",
					"72D", "71A", "71E", "755", "74C", "76C", "74E", "72C", "74A", "712",
					"76F", "74B", "754", "76D", "7E6", "752", "773", "70B", "747", "769",
					"6B8", "723", "745", "71D", "767", "70A", "76B", "75A", "74F", "6BC",
					"784", "710", "757", "7E5", "762", "765", "744"
				};
				string[] array2;
				if (SharedSettings.Current.HideArchiveDTC)
				{
					array2 = new string[] { "3E", "1902AF", "1902AF", "1800FF00", "1800FFFF" };
				}
				else
				{
					array2 = new string[] { "3E", "1902AF", "1902AF", "190FAF", "190FAF", "1800FF00", "1800FFFF" };
				}
				OBDRequest[] requestsFromPatternWithCRA = DTCWorker.GetRequestsFromPatternWithCRA(text, "", array2, array, read, delegate(string req_header)
				{
					if (req_header.StartsWith("7E", StringComparison.OrdinalIgnoreCase))
					{
						int num;
						if (int.TryParse(req_header, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
						{
							return (num + 8).ToString("X3");
						}
						return "";
					}
					else
					{
						int num2;
						if (int.TryParse(req_header, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2))
						{
							return (num2 + 106).ToString("X3");
						}
						return "";
					}
				});
				foreach (OBDRequest obdrequest in requestsFromPatternWithCRA)
				{
					if (obdrequest.Command == "3E")
					{
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data == null || data.Contains("NO DATA"))
							{
								List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
								queueCopy.RemoveAll((OBDRequest x) => x.Header == request.Header);
								App.OBDReader.ReplaceQueue(queueCopy);
							}
						};
					}
					if (obdrequest.Command.StartsWith("19"))
					{
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data != null && data.Contains("59"))
							{
								List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
								queueCopy2.RemoveAll((OBDRequest x) => x.Header == request.Header && x.Command.StartsWith("18"));
								App.OBDReader.ReplaceQueue(queueCopy2);
							}
						};
					}
				}
				List<OBDRequest> list = new List<OBDRequest>();
				list.Add(new OBDRequest("ATSH7DF", false, empty_pids));
				list.AddRange(DTCWorker.OBD_DTC.Select((string x) => new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !read
				}));
				list.AddRange(array2.Select((string x) => new OBDRequest(x, false, empty_pids)
				{
					DoNotDecode = !read
				}));
				list.Add(DTCWorker.ATFCTestRequest(6));
				list.AddRange(requestsFromPatternWithCRA);
				list.AddRange(DTCWorker.GetRestoreConnectionCommands());
				return list.ToArray();
			}
			string text2 = "ATFCSH{0};ATFCSD300010;ATFCSM1;ATCRA{1};1003";
			string[] array4 = new string[]
			{
				"7E0",
				"7E1",
				VagUnitHelper.GetRequestHeaderForMQBUnit("03"),
				VagUnitHelper.GetRequestHeaderForMQBUnit("5F"),
				VagUnitHelper.GetRequestHeaderForMQBUnit("08"),
				VagUnitHelper.GetRequestHeaderForMQBUnit("05"),
				VagUnitHelper.GetRequestHeaderForMQBUnit("17")
			};
			string[] array5 = new string[] { "3E", "14FFFFFF", "14FFFF" };
			OBDRequest[] requestsFromPatternWithCRA2 = DTCWorker.GetRequestsFromPatternWithCRA(text2, "", array5, array4, read, (string requestHeader) => CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null));
			foreach (OBDRequest obdrequest2 in requestsFromPatternWithCRA2)
			{
				if (obdrequest2.Command == "3E")
				{
					obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if (data == null || data.Contains("NO DATA"))
						{
							List<OBDRequest> queueCopy3 = App.OBDReader.GetQueueCopy();
							queueCopy3.RemoveAll((OBDRequest x) => x.Header == request.Header);
							App.OBDReader.ReplaceQueue(queueCopy3);
						}
					};
				}
				if (obdrequest2.Command == "14FFFFFF")
				{
					obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if (data != null)
						{
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (data.Contains("7F1478") || data.Contains("54"))
							{
								List<OBDRequest> queueCopy4 = App.OBDReader.GetQueueCopy();
								queueCopy4.RemoveAll((OBDRequest x) => x.Header == request.Header && x.Command.StartsWith("18"));
								App.OBDReader.ReplaceQueue(queueCopy4);
							}
						}
					};
				}
			}
			List<OBDRequest> list2 = new List<OBDRequest>();
			list2.Add(new OBDRequest("ATSH7DF", false, empty_pids));
			list2.Add(new OBDRequest("04", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("04", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("04", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("14", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("14FF00", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(new OBDRequest("14FFFFFF", false, empty_pids)
			{
				DoNotDecode = !read
			});
			list2.Add(DTCWorker.ATSHTest(6));
			list2.AddRange(requestsFromPatternWithCRA2);
			list2.Add(new OBDRequest("ATFCSM0", false, empty_pids)
			{
				DoNotDecode = true
			});
			list2.Add(new OBDRequest("ATSH7DF", false, empty_pids)
			{
				DoNotDecode = true
			});
			return list2.ToArray();
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x00240090 File Offset: 0x0023E290
		private static string GetSubstringBetweenTwoStrings(string input, string substring1, string substring2)
		{
			int num = input.IndexOf(substring1, StringComparison.Ordinal);
			int num2 = input.IndexOf(substring2, StringComparison.Ordinal);
			if (num < 0 || num2 < 0)
			{
				throw new ArgumentException("substring not found");
			}
			if (num2 < num)
			{
				num = num2;
			}
			string text = input.Substring(num + substring1.Length);
			num2 = text.IndexOf(substring2, StringComparison.Ordinal);
			return text.Substring(0, num2);
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x002400E8 File Offset: 0x0023E2E8
		public static string[] GetStartDiagnosticsCommands()
		{
			string text = SharedSettings.Current.SelectedBrand;
			if (string.IsNullOrEmpty(text))
			{
				text = SharedSettings.Current.BrandAndProfile;
			}
			text = text.ToLowerInvariant();
			if (text.Contains("hyundai") || text.Contains("kia"))
			{
				return new string[] { "1003", "1090" };
			}
			if (text.Contains("mitsubishi"))
			{
				return new string[] { "1092" };
			}
			if (text.Contains("nissan") || text.Contains("renault") || text.Contains("dacia") || text.Contains("samsung") || text.Contains("infinit"))
			{
				return new string[] { "10C0" };
			}
			return new string[0];
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x002401C0 File Offset: 0x0023E3C0
		public static string[] GetStopDiagnosticsCommand()
		{
			string text = SharedSettings.Current.SelectedBrand;
			text = text.ToLowerInvariant();
			if (text.Contains("hyundai") || text.Contains("kia"))
			{
				return new string[] { "20" };
			}
			return new string[0];
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x00240210 File Offset: 0x0023E410
		public static bool GetShouldSetFlowControl()
		{
			string text = SharedSettings.Current.SelectedBrand;
			text = text.ToLowerInvariant();
			return text.Contains("nissan") || text.Contains("renault") || text.Contains("dacia") || text.Contains("samsung") || text.Contains("infinit") || text.Contains("mitsubishi");
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x00240280 File Offset: 0x0023E480
		public static string[] GetFlowControlCommandsForHeader(string header)
		{
			return new string[]
			{
				"ATFCSH" + header,
				"ATFCSD300000",
				"ATFCSM1"
			};
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x002402A8 File Offset: 0x0023E4A8
		private static string[] GetCommonHeaders()
		{
			string[] array = new string[] { "" };
			if (SharedSettings.Current.ProtocolNumber <= 11)
			{
				switch (App.OBDReader.CurrentProtocolNumber)
				{
				case 1:
					array = new string[] { "", "6158F1", "616AF1", "6110F1", "6118F1", "616AF1" };
					break;
				case 2:
					array = new string[] { "", "6C10F1", "6C58F1", "6C40F1", "6C1AF1", "6818F1", "6810F1", "6858F1", "6828F1", "686AF1" };
					break;
				case 3:
					array = new string[] { "", "6828F1", "6858F1", "6818F1", "686AF1", "6810F1", "6812F1" };
					break;
				case 4:
				case 5:
					array = new string[]
					{
						"", "8110F1", "8111F1", "8112F1", "8113F1", "8114F1", "8658F1", "C241F1", "C218F1", "C230F1",
						"C258F1", "C228F1", "C233F1", "8116F1", "8118F1", "811AF1"
					};
					break;
				case 6:
				case 8:
				{
					string text = SharedSettings.Current.BrandAndProfile.ToLowerInvariant();
					if (text.Contains("kia") || text.Contains("hyundai"))
					{
						array = new string[] { "", "7E0", "7E1", "7A5", "7B3", "7C6", "7D1", "7D2", "7E5", "740" };
					}
					else if (text.Contains("toyota") || text.Contains("lexus"))
					{
						array = new string[]
						{
							"", "7E0", "7E1", "780", "7A1", "7B0", "7C0", "7C4", "7D0", "7E3",
							"7E5", "7A4", "75F", "759", "752", "74D", "74C", "748", "745", "744",
							"742", "740", "710", "70C"
						};
					}
					else if (text.Contains("nissan"))
					{
						array = new string[] { "", "7E0", "7E1", "765", "764" };
					}
					else if (text.Contains("Mercedes"))
					{
						array = new string[] { "", "7E0", "7E1", "7E2", "7E3", "720" };
					}
					else
					{
						array = new string[] { "", "760", "720", "7E0", "7E1", "7E2", "7E3" };
					}
					break;
				}
				case 7:
				case 9:
					array = new string[]
					{
						"", "DB18F1", "DB10F1", "DB12F1", "DB14F1", "DB16F1", "DB18F1", "DB28F1", "DB33F1", "DA18F1",
						"DA10F1", "DA12F1", "DA14F1", "DA16F1", "DA18F1", "DA28F1", "DA33F1"
					};
					break;
				default:
					array = new string[] { "" };
					break;
				}
			}
			else if (SharedSettings.Current.ProtocolNumber > 11)
			{
				switch (SharedSettings.Current.ProtocolNumber)
				{
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 21:
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
				case 40:
				case 41:
					array = new string[] { "", "8658F1", "C241F1", "C218F1", "C230F1", "C258F1", "C228F1", "C233F1" };
					break;
				case 22:
				case 23:
				case 24:
				case 25:
				case 26:
				case 42:
					array = new string[] { "", "6828F1", "6858F1", "6818F1", "686AF1" };
					break;
				}
			}
			IEnumerable<string> enumerable = from x in CustomPIDViewModel.CurrentProfile.PidCollection
				where x != null && !string.IsNullOrEmpty(x.Header)
				select x.Header;
			List<string> list = new List<string>();
			foreach (string text2 in enumerable)
			{
				if (!array.Contains(text2) && !list.Contains(text2))
				{
					list.Add(text2);
				}
			}
			foreach (string text3 in from x in CustomPIDViewModel.CurrentCustom.PidCollection
				where x != null && !string.IsNullOrEmpty(x.Header)
				select x.Header)
			{
				if (!array.Contains(text3) && !list.Contains(text3))
				{
					list.Add(text3);
				}
			}
			int currentProtocolNumber = App.OBDReader.CurrentProtocolNumber;
			if (currentProtocolNumber == 6 || currentProtocolNumber == 8)
			{
				foreach (ECUHeader ecuheader in App.OBDReader.ECUHeaders)
				{
					int num = BitHelpers.ConvertHexToInt(ecuheader.Id);
					string text4 = (num - 8).ToString("X3");
					if (!array.Contains(text4) && !list.Contains(text4))
					{
						list.Add(text4);
					}
				}
			}
			if (list.Count > 0)
			{
				array = array.Concat(list).Distinct<string>().ToArray<string>();
			}
			return array;
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x00240A98 File Offset: 0x0023EC98
		private static string[] GetExtendedHeadersList(bool FullRange)
		{
			List<string> list = new List<string>();
			list.Add("");
			switch (App.OBDReader.CurrentProtocolNumber)
			{
			case 6:
			case 8:
			{
				int num;
				int num2;
				if (FullRange)
				{
					num = 2032;
					num2 = 0;
				}
				else
				{
					num = 2032;
					num2 = 1536;
				}
				for (int i = num; i >= num2; i -= 16)
				{
					for (int j = i; j <= i + 15; j++)
					{
						list.Add(j.ToString("X3"));
					}
				}
				goto IL_0125;
			}
			case 7:
			case 9:
			{
				int num3;
				int num4;
				if (FullRange)
				{
					num3 = 0;
					num4 = 255;
				}
				else
				{
					num3 = 0;
					num4 = 50;
				}
				for (int k = num3; k <= num4; k++)
				{
					string text = "DA" + k.ToString("X2") + "F1";
					list.Add(text);
				}
				goto IL_0125;
			}
			}
			int num5;
			int num6;
			if (FullRange)
			{
				num5 = 0;
				num6 = 255;
			}
			else
			{
				num5 = 0;
				num6 = 50;
			}
			string[] wideRangeHeadersForKWP = DTCWorker.GetWideRangeHeadersForKWP(App.OBDReader.GetDefaultHeader(), num5, num6);
			list.AddRange(wideRangeHeadersForKWP);
			IL_0125:
			return list.ToArray();
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x00240BD0 File Offset: 0x0023EDD0
		private static string[] GetWideRangeHeadersForKWP(string default_header, int start, int end)
		{
			string[] array;
			try
			{
				List<string> list = new List<string>();
				list.Add(default_header);
				string text = default_header.Substring(0, 2);
				default_header.Substring(2, 2);
				string text2 = default_header.Substring(4, 2);
				for (int i = start; i <= end; i++)
				{
					string text3 = text + i.ToString("X2") + text2;
					list.Add(text3);
				}
				array = list.ToArray();
			}
			catch (Exception ex)
			{
				App.OBDReader.DebugWriteSync(string.Concat(new string[]
				{
					"GetWideRangeHeadersForKWP:",
					default_header,
					",",
					start.ToString(),
					",",
					end.ToString(),
					"\n",
					ex.ToString()
				}));
				array = DTCWorker.GetCommonHeaders();
			}
			return array;
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x00240CAC File Offset: 0x0023EEAC
		private static string[] SkipHeaders(string[] headers)
		{
			if (!SharedSettings.Current.ShowExperimental)
			{
				return headers;
			}
			if (string.IsNullOrEmpty(SharedSettings.Current.SkipHeadersDTC.Trim()))
			{
				return headers;
			}
			string[] skip = SharedSettings.Current.SkipHeadersDTC.Split(new char[] { ';' });
			if (skip.Length == 0)
			{
				return headers;
			}
			List<string> list = headers.ToList<string>();
			int i;
			Predicate<string> <>9__0;
			int j;
			for (i = 0; i < skip.Length; i = j + 1)
			{
				skip[i] = skip[i].ToUpperInvariant();
				List<string> list2 = list;
				Predicate<string> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (string x) => x == skip[i]);
				}
				int num = list2.FindIndex(predicate);
				if (num >= 0)
				{
					list.RemoveAt(num);
				}
				j = i;
			}
			return list.ToArray();
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x00240D9C File Offset: 0x0023EF9C
		public static OBDRequest[] GetClassicCommands(int mode, bool IsReading)
		{
			bool flag = false;
			string[] array = new string[0];
			string[] array2 = new string[0];
			switch (mode)
			{
			case 0:
				if (App.OBDReader.IsNissanConsult2Protocol && App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
				{
					flag = false;
					if (IsReading)
					{
						array = new string[] { "_A3" };
					}
					else
					{
						array = new string[] { "14" };
					}
					array2 = new string[] { "" };
				}
				else
				{
					flag = false;
					if (IsReading)
					{
						array = DTCWorker.OBD_DTC;
					}
					else
					{
						array = DTCWorker.OBD_CLEAR;
					}
					array2 = new string[] { "" };
				}
				break;
			case 1:
				flag = false;
				if (IsReading)
				{
					array = DTCWorker.DTC_PIDS;
				}
				else
				{
					array = DTCWorker.CLEAR_DTC;
				}
				array2 = new string[] { "" };
				break;
			case 2:
				flag = true;
				if (IsReading)
				{
					array = DTCWorker.DTC_PIDS;
				}
				else
				{
					array = DTCWorker.CLEAR_DTC;
				}
				array2 = new string[] { "" };
				break;
			case 3:
				flag = false;
				if (IsReading)
				{
					array = DTCWorker.DTC_PIDS;
				}
				else
				{
					array = DTCWorker.CLEAR_DTC;
				}
				array2 = DTCWorker.GetCommonHeaders();
				break;
			case 4:
				flag = true;
				if (IsReading)
				{
					array = DTCWorker.DTC_PIDS;
				}
				else
				{
					array = DTCWorker.CLEAR_DTC;
				}
				array2 = DTCWorker.GetCommonHeaders();
				break;
			case 5:
				flag = false;
				if (IsReading)
				{
					array = DTCWorker.DTC_PIDS;
				}
				else
				{
					array = DTCWorker.CLEAR_DTC;
				}
				array2 = DTCWorker.GetExtendedHeadersList(false);
				break;
			case 6:
				flag = true;
				if (IsReading)
				{
					array = DTCWorker.DTC_PIDS;
				}
				else
				{
					array = DTCWorker.CLEAR_DTC;
				}
				array2 = DTCWorker.GetExtendedHeadersList(false);
				break;
			case 7:
				flag = false;
				if (IsReading)
				{
					array = DTCWorker.DTC_PIDS;
				}
				else
				{
					array = DTCWorker.CLEAR_DTC;
				}
				array2 = DTCWorker.GetExtendedHeadersList(true);
				break;
			case 8:
				flag = true;
				if (IsReading)
				{
					array = DTCWorker.DTC_PIDS;
				}
				else
				{
					array = DTCWorker.CLEAR_DTC;
				}
				array2 = DTCWorker.GetExtendedHeadersList(true);
				break;
			}
			string selectedBrand = SharedSettings.Current.SelectedBrand;
			if (selectedBrand != null)
			{
				switch (selectedBrand.Length)
				{
				case 3:
					if (!(selectedBrand == "GMC"))
					{
						goto IL_03BB;
					}
					break;
				case 4:
				{
					char c = selectedBrand[0];
					if (c != 'O')
					{
						if (c != 'S')
						{
							goto IL_03BB;
						}
						if (!(selectedBrand == "Saab"))
						{
							goto IL_03BB;
						}
					}
					else if (!(selectedBrand == "Opel"))
					{
						goto IL_03BB;
					}
					break;
				}
				case 5:
				{
					char c = selectedBrand[0];
					if (c != 'B')
					{
						if (c != 'R')
						{
							goto IL_03BB;
						}
						if (!(selectedBrand == "Ravon"))
						{
							goto IL_03BB;
						}
					}
					else if (!(selectedBrand == "Buick"))
					{
						goto IL_03BB;
					}
					break;
				}
				case 6:
				{
					char c = selectedBrand[2];
					if (c <= 'l')
					{
						if (c != 'e')
						{
							if (c != 'l')
							{
								goto IL_03BB;
							}
							if (!(selectedBrand == "Holden"))
							{
								goto IL_03BB;
							}
						}
						else if (!(selectedBrand == "Daewoo"))
						{
							goto IL_03BB;
						}
					}
					else if (c != 'm')
					{
						if (c != 't')
						{
							goto IL_03BB;
						}
						if (!(selectedBrand == "Saturn"))
						{
							goto IL_03BB;
						}
					}
					else if (!(selectedBrand == "Hummer"))
					{
						goto IL_03BB;
					}
					break;
				}
				case 7:
					if (!(selectedBrand == "Pontiac"))
					{
						goto IL_03BB;
					}
					break;
				case 8:
				{
					char c = selectedBrand[0];
					if (c != 'C')
					{
						if (c != 'V')
						{
							goto IL_03BB;
						}
						if (!(selectedBrand == "Vauxhall"))
						{
							goto IL_03BB;
						}
					}
					else if (!(selectedBrand == "Cadillac"))
					{
						goto IL_03BB;
					}
					break;
				}
				case 9:
					if (!(selectedBrand == "Chevrolet"))
					{
						goto IL_03BB;
					}
					break;
				case 10:
				case 11:
				case 12:
				case 13:
					goto IL_03BB;
				case 14:
					if (!(selectedBrand == "General Motors"))
					{
						goto IL_03BB;
					}
					break;
				default:
					goto IL_03BB;
				}
				if (array == DTCWorker.DTC_PIDS)
				{
					array = new string[] { "1201" }.Concat(DTCWorker.DTC_PIDS).ToArray<string>();
				}
			}
			IL_03BB:
			array2 = DTCWorker.SkipHeaders(array2);
			string[] startDiagnosticsCommands = DTCWorker.GetStartDiagnosticsCommands();
			string[] stopDiagnosticsCommand = DTCWorker.GetStopDiagnosticsCommand();
			bool flag2 = true;
			List<string> list = new List<string>(array2.Length * array.Length);
			if (SharedSettings.Current.UseOBD2)
			{
				if (IsReading)
				{
					list.AddRange(DTCWorker.OBD_DTC);
				}
				else
				{
					list.AddRange(DTCWorker.OBD_CLEAR);
				}
			}
			foreach (string text in array2)
			{
				if (text != "")
				{
					list.Add("ATSH" + text);
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && flag2)
					{
						string[] flowControlCommandsForHeader = DTCWorker.GetFlowControlCommandsForHeader(text);
						list.AddRange(flowControlCommandsForHeader);
					}
					if (flag)
					{
						list.AddRange(startDiagnosticsCommands);
					}
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && (text == "7E0" || text == "7E1"))
					{
						if (IsReading)
						{
							list.Add("03");
							list.Add("07");
						}
						else
						{
							list.Add("04");
						}
					}
					list.AddRange(array);
					if (flag)
					{
						list.AddRange(stopDiagnosticsCommand);
					}
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && flag2)
					{
						list.Add("ATFCSM0");
					}
				}
				else
				{
					list.AddRange(array);
				}
			}
			return DTCWorker.GetRequestsFromStringSequence(list);
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x002412C4 File Offset: 0x0023F4C4
		public static void GoogleForDTC(string code)
		{
			if (code.Length == 7)
			{
				code = code.Substring(0, 5);
			}
			string selectedBrand = SharedSettings.Current.SelectedBrand;
			if (!string.IsNullOrEmpty(selectedBrand) && selectedBrand != Translate.GetString("ios_Other"))
			{
				Launcher.TryOpenAsync("https://www.google.com/search?q=DTC+" + code + "+" + selectedBrand);
				return;
			}
			Launcher.TryOpenAsync("https://www.google.com/search?q=DTC+" + code);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x00241334 File Offset: 0x0023F534
		// Note: this type is marked as 'beforefieldinit'.
		static DTCWorker()
		{
		}

		// Token: 0x04001DEE RID: 7662
		internal static string[] OBD_DTC = new string[] { "_03", "_03", "_07", "_07", "_0A" };

		// Token: 0x04001DEF RID: 7663
		internal static readonly string[] NC2_DTC = new string[] { "_A3" };

		// Token: 0x04001DF0 RID: 7664
		internal static string[] OBD_CLEAR = new string[] { "04", "04", "04" };

		// Token: 0x04001DF1 RID: 7665
		internal static string[] DTC_PIDS = new string[]
		{
			"_1800FF00", "_1802FF00", "_1802FFFF", "_1800FFFF", "_18FF00", "_17FF00", "_13FF00", "_1902AF", "_1902AC", "_19028D",
			"_190223", "_190278", "_190208", "_190FAC", "_190F8D", "_190F23", "_19D2FF00"
		};

		// Token: 0x04001DF2 RID: 7666
		internal static string[] CLEAR_DTC = new string[] { "14", "14FF00", "14FFFF", "14FFFFFF" };

		// Token: 0x02000563 RID: 1379
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003311 RID: 13073 RVA: 0x00241471 File Offset: 0x0023F671
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003312 RID: 13074 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003313 RID: 13075 RVA: 0x0024147D File Offset: 0x0023F67D
			internal void <Start>b__1_0()
			{
				DescriptionLoader.ResetCache();
				DescriptionLoader.PreloadDescriptionsForBrand(string.IsNullOrEmpty(SharedSettings.Current.BrandForDTC) ? SharedSettings.Current.SelectedBrand : SharedSettings.Current.BrandForDTC);
			}

			// Token: 0x06003314 RID: 13076 RVA: 0x002414B0 File Offset: 0x0023F6B0
			internal string <GetBrandSequence>b__7_0(string req_header)
			{
				int num;
				if (int.TryParse(req_header, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
				{
					return (num + 1).ToString("X3");
				}
				return "";
			}

			// Token: 0x06003315 RID: 13077 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_3(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003316 RID: 13078 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_4(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003317 RID: 13079 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_7(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003318 RID: 13080 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_8(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003319 RID: 13081 RVA: 0x002414FA File Offset: 0x0023F6FA
			internal bool <GetBrandSequence>b__7_9(OBDRequest x)
			{
				return x.Command == "3E" && !x.Header.StartsWith("7E");
			}

			// Token: 0x0600331A RID: 13082 RVA: 0x00241524 File Offset: 0x0023F724
			internal void <GetBrandSequence>b__7_12(OBDRequest req, string data)
			{
				DTCWorker.<>c__DisplayClass7_2 CS$<>8__locals1 = new DTCWorker.<>c__DisplayClass7_2();
				CS$<>8__locals1.req = req;
				if (data == null || !data.Contains("7E"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.Header == CS$<>8__locals1.req.Header);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x0600331B RID: 13083 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_15(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x0600331C RID: 13084 RVA: 0x002414FA File Offset: 0x0023F6FA
			internal bool <GetBrandSequence>b__7_16(OBDRequest x)
			{
				return x.Command == "3E" && !x.Header.StartsWith("7E");
			}

			// Token: 0x0600331D RID: 13085 RVA: 0x00241578 File Offset: 0x0023F778
			internal void <GetBrandSequence>b__7_17(OBDRequest req, string data)
			{
				DTCWorker.<>c__DisplayClass7_4 CS$<>8__locals1 = new DTCWorker.<>c__DisplayClass7_4();
				CS$<>8__locals1.req = req;
				if (data == null || !data.Contains("7E"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.Header == CS$<>8__locals1.req.Header);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x0600331E RID: 13086 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_20(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x0600331F RID: 13087 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_23(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003320 RID: 13088 RVA: 0x002415CC File Offset: 0x0023F7CC
			internal bool <GetBrandSequence>b__7_24(OBDRequest x)
			{
				return x.Command == "03" || x.Command == "07" || x.Command == "0A" || x.Command == "04" || x.Header == "";
			}

			// Token: 0x06003321 RID: 13089 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_25(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003322 RID: 13090 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_28(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003323 RID: 13091 RVA: 0x00241634 File Offset: 0x0023F834
			internal bool <GetBrandSequence>b__7_29(OBDRequest x)
			{
				return x.Command == "03" || x.Command == "07" || x.Command == "0A" || x.Command == "04" || x.Header == "" || x.Header == "7E0" || x.Header == "7E1";
			}

			// Token: 0x06003324 RID: 13092 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_32(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003325 RID: 13093 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_35(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003326 RID: 13094 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_36(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003327 RID: 13095 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_39(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003328 RID: 13096 RVA: 0x002416BF File Offset: 0x0023F8BF
			internal string <GetBrandSequence>b__7_42(int x)
			{
				return x.ToString("X2");
			}

			// Token: 0x06003329 RID: 13097 RVA: 0x002416CD File Offset: 0x0023F8CD
			internal OBDRequest <GetBrandSequence>b__7_43(string x)
			{
				return new OBDRequest(x.Substring(1), false);
			}

			// Token: 0x0600332A RID: 13098 RVA: 0x002416BF File Offset: 0x0023F8BF
			internal string <GetBrandSequence>b__7_44(int x)
			{
				return x.ToString("X2");
			}

			// Token: 0x0600332B RID: 13099 RVA: 0x002416DC File Offset: 0x0023F8DC
			internal OBDRequest <GetBrandSequence>b__7_45(string x)
			{
				return new OBDRequest(x, false);
			}

			// Token: 0x0600332C RID: 13100 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_46(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x0600332D RID: 13101 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_49(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x0600332E RID: 13102 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_52(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x0600332F RID: 13103 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_55(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003330 RID: 13104 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_56(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003331 RID: 13105 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetBrandSequence>b__7_59(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003332 RID: 13106 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetRenaultCANRequests>b__9_0(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003333 RID: 13107 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetRenaultCANRequests>b__9_3(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x06003334 RID: 13108 RVA: 0x002416E8 File Offset: 0x0023F8E8
			internal void <ATFCTestRequest>b__11_0(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.BeforeCommands.Any((string cmd) => cmd.StartsWith("ATFCSH")));
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x06003335 RID: 13109 RVA: 0x0024173E File Offset: 0x0023F93E
			internal bool <ATFCTestRequest>b__11_1(OBDRequest x)
			{
				return x.BeforeCommands.Any((string cmd) => cmd.StartsWith("ATFCSH"));
			}

			// Token: 0x06003336 RID: 13110 RVA: 0x001DDD6A File Offset: 0x001DBF6A
			internal bool <ATFCTestRequest>b__11_2(string cmd)
			{
				return cmd.StartsWith("ATFCSH");
			}

			// Token: 0x06003337 RID: 13111 RVA: 0x0024176C File Offset: 0x0023F96C
			internal void <ATSHTest>b__12_0(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.Header != "");
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x06003338 RID: 13112 RVA: 0x002417C2 File Offset: 0x0023F9C2
			internal bool <ATSHTest>b__12_1(OBDRequest x)
			{
				return x.Header != "";
			}

			// Token: 0x06003339 RID: 13113 RVA: 0x002417D4 File Offset: 0x0023F9D4
			internal string <GetVAGCanRequests>b__13_0(string req_header)
			{
				if (req_header.StartsWith("7E", StringComparison.OrdinalIgnoreCase))
				{
					int num;
					if (int.TryParse(req_header, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
					{
						return (num + 8).ToString("X3");
					}
					return "";
				}
				else
				{
					int num2;
					if (int.TryParse(req_header, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2))
					{
						return (num2 + 106).ToString("X3");
					}
					return "";
				}
			}

			// Token: 0x0600333A RID: 13114 RVA: 0x00241848 File Offset: 0x0023FA48
			internal void <GetVAGCanRequests>b__13_3(OBDRequest request, string data)
			{
				DTCWorker.<>c__DisplayClass13_1 CS$<>8__locals1 = new DTCWorker.<>c__DisplayClass13_1();
				CS$<>8__locals1.request = request;
				if (data == null || data.Contains("NO DATA"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x0600333B RID: 13115 RVA: 0x0024189C File Offset: 0x0023FA9C
			internal void <GetVAGCanRequests>b__13_4(OBDRequest request, string data)
			{
				DTCWorker.<>c__DisplayClass13_2 CS$<>8__locals1 = new DTCWorker.<>c__DisplayClass13_2();
				CS$<>8__locals1.request = request;
				if (data != null && data.Contains("59"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header && x.Command.StartsWith("18"));
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x0600333C RID: 13116 RVA: 0x002414E7 File Offset: 0x0023F6E7
			internal string <GetVAGCanRequests>b__13_7(string requestHeader)
			{
				return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, SharedSettings.Current.SelectedBrand, null);
			}

			// Token: 0x0600333D RID: 13117 RVA: 0x002418F0 File Offset: 0x0023FAF0
			internal void <GetVAGCanRequests>b__13_8(OBDRequest request, string data)
			{
				DTCWorker.<>c__DisplayClass13_3 CS$<>8__locals1 = new DTCWorker.<>c__DisplayClass13_3();
				CS$<>8__locals1.request = request;
				if (data == null || data.Contains("NO DATA"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.RemoveAll((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x0600333E RID: 13118 RVA: 0x00241944 File Offset: 0x0023FB44
			internal void <GetVAGCanRequests>b__13_9(OBDRequest request, string data)
			{
				DTCWorker.<>c__DisplayClass13_4 CS$<>8__locals1 = new DTCWorker.<>c__DisplayClass13_4();
				CS$<>8__locals1.request = request;
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (data.Contains("7F1478") || data.Contains("54"))
					{
						List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
						queueCopy.RemoveAll((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header && x.Command.StartsWith("18"));
						App.OBDReader.ReplaceQueue(queueCopy);
					}
				}
			}

			// Token: 0x0600333F RID: 13119 RVA: 0x002419AC File Offset: 0x0023FBAC
			internal bool <GetCommonHeaders>b__24_0(CustomPID x)
			{
				return x != null && !string.IsNullOrEmpty(x.Header);
			}

			// Token: 0x06003340 RID: 13120 RVA: 0x002419C1 File Offset: 0x0023FBC1
			internal string <GetCommonHeaders>b__24_1(CustomPID x)
			{
				return x.Header;
			}

			// Token: 0x06003341 RID: 13121 RVA: 0x002419AC File Offset: 0x0023FBAC
			internal bool <GetCommonHeaders>b__24_2(CustomPID x)
			{
				return x != null && !string.IsNullOrEmpty(x.Header);
			}

			// Token: 0x06003342 RID: 13122 RVA: 0x002419C1 File Offset: 0x0023FBC1
			internal string <GetCommonHeaders>b__24_3(CustomPID x)
			{
				return x.Header;
			}

			// Token: 0x04001DF3 RID: 7667
			public static readonly DTCWorker.<>c <>9 = new DTCWorker.<>c();

			// Token: 0x04001DF4 RID: 7668
			public static Action <>9__1_0;

			// Token: 0x04001DF5 RID: 7669
			public static Func<string, string> <>9__7_0;

			// Token: 0x04001DF6 RID: 7670
			public static Func<string, string> <>9__7_3;

			// Token: 0x04001DF7 RID: 7671
			public static Func<string, string> <>9__7_4;

			// Token: 0x04001DF8 RID: 7672
			public static Func<string, string> <>9__7_7;

			// Token: 0x04001DF9 RID: 7673
			public static Func<string, string> <>9__7_8;

			// Token: 0x04001DFA RID: 7674
			public static Func<OBDRequest, bool> <>9__7_9;

			// Token: 0x04001DFB RID: 7675
			public static ResponseReceivedDelegate <>9__7_12;

			// Token: 0x04001DFC RID: 7676
			public static Func<string, string> <>9__7_15;

			// Token: 0x04001DFD RID: 7677
			public static Func<OBDRequest, bool> <>9__7_16;

			// Token: 0x04001DFE RID: 7678
			public static ResponseReceivedDelegate <>9__7_17;

			// Token: 0x04001DFF RID: 7679
			public static Func<string, string> <>9__7_20;

			// Token: 0x04001E00 RID: 7680
			public static Func<string, string> <>9__7_23;

			// Token: 0x04001E01 RID: 7681
			public static Predicate<OBDRequest> <>9__7_24;

			// Token: 0x04001E02 RID: 7682
			public static Func<string, string> <>9__7_25;

			// Token: 0x04001E03 RID: 7683
			public static Func<string, string> <>9__7_28;

			// Token: 0x04001E04 RID: 7684
			public static Predicate<OBDRequest> <>9__7_29;

			// Token: 0x04001E05 RID: 7685
			public static Func<string, string> <>9__7_32;

			// Token: 0x04001E06 RID: 7686
			public static Func<string, string> <>9__7_35;

			// Token: 0x04001E07 RID: 7687
			public static Func<string, string> <>9__7_36;

			// Token: 0x04001E08 RID: 7688
			public static Func<string, string> <>9__7_39;

			// Token: 0x04001E09 RID: 7689
			public static Func<int, string> <>9__7_42;

			// Token: 0x04001E0A RID: 7690
			public static Func<string, OBDRequest> <>9__7_43;

			// Token: 0x04001E0B RID: 7691
			public static Func<int, string> <>9__7_44;

			// Token: 0x04001E0C RID: 7692
			public static Func<string, OBDRequest> <>9__7_45;

			// Token: 0x04001E0D RID: 7693
			public static Func<string, string> <>9__7_46;

			// Token: 0x04001E0E RID: 7694
			public static Func<string, string> <>9__7_49;

			// Token: 0x04001E0F RID: 7695
			public static Func<string, string> <>9__7_52;

			// Token: 0x04001E10 RID: 7696
			public static Func<string, string> <>9__7_55;

			// Token: 0x04001E11 RID: 7697
			public static Func<string, string> <>9__7_56;

			// Token: 0x04001E12 RID: 7698
			public static Func<string, string> <>9__7_59;

			// Token: 0x04001E13 RID: 7699
			public static Func<string, string> <>9__9_0;

			// Token: 0x04001E14 RID: 7700
			public static Func<string, string> <>9__9_3;

			// Token: 0x04001E15 RID: 7701
			public static Func<string, bool> <>9__11_2;

			// Token: 0x04001E16 RID: 7702
			public static Predicate<OBDRequest> <>9__11_1;

			// Token: 0x04001E17 RID: 7703
			public static ResponseReceivedDelegate <>9__11_0;

			// Token: 0x04001E18 RID: 7704
			public static Predicate<OBDRequest> <>9__12_1;

			// Token: 0x04001E19 RID: 7705
			public static ResponseReceivedDelegate <>9__12_0;

			// Token: 0x04001E1A RID: 7706
			public static Func<string, string> <>9__13_0;

			// Token: 0x04001E1B RID: 7707
			public static ResponseReceivedDelegate <>9__13_3;

			// Token: 0x04001E1C RID: 7708
			public static ResponseReceivedDelegate <>9__13_4;

			// Token: 0x04001E1D RID: 7709
			public static Func<string, string> <>9__13_7;

			// Token: 0x04001E1E RID: 7710
			public static ResponseReceivedDelegate <>9__13_8;

			// Token: 0x04001E1F RID: 7711
			public static ResponseReceivedDelegate <>9__13_9;

			// Token: 0x04001E20 RID: 7712
			public static Func<CustomPID, bool> <>9__24_0;

			// Token: 0x04001E21 RID: 7713
			public static Func<CustomPID, string> <>9__24_1;

			// Token: 0x04001E22 RID: 7714
			public static Func<CustomPID, bool> <>9__24_2;

			// Token: 0x04001E23 RID: 7715
			public static Func<CustomPID, string> <>9__24_3;
		}

		// Token: 0x02000564 RID: 1380
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x06003343 RID: 13123 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x06003344 RID: 13124 RVA: 0x002419C9 File Offset: 0x0023FBC9
			internal OBDRequest <GetVAGCanRequests>b__1(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003345 RID: 13125 RVA: 0x002419E7 File Offset: 0x0023FBE7
			internal OBDRequest <GetVAGCanRequests>b__2(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x04001E24 RID: 7716
			public bool read;

			// Token: 0x04001E25 RID: 7717
			public List<PID> empty_pids;
		}

		// Token: 0x02000565 RID: 1381
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_1
		{
			// Token: 0x06003346 RID: 13126 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_1()
			{
			}

			// Token: 0x06003347 RID: 13127 RVA: 0x00241A05 File Offset: 0x0023FC05
			internal bool <GetVAGCanRequests>b__5(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04001E26 RID: 7718
			public OBDRequest request;
		}

		// Token: 0x02000566 RID: 1382
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_2
		{
			// Token: 0x06003348 RID: 13128 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_2()
			{
			}

			// Token: 0x06003349 RID: 13129 RVA: 0x00241A1D File Offset: 0x0023FC1D
			internal bool <GetVAGCanRequests>b__6(OBDRequest x)
			{
				return x.Header == this.request.Header && x.Command.StartsWith("18");
			}

			// Token: 0x04001E27 RID: 7719
			public OBDRequest request;
		}

		// Token: 0x02000567 RID: 1383
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_3
		{
			// Token: 0x0600334A RID: 13130 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_3()
			{
			}

			// Token: 0x0600334B RID: 13131 RVA: 0x00241A49 File Offset: 0x0023FC49
			internal bool <GetVAGCanRequests>b__10(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04001E28 RID: 7720
			public OBDRequest request;
		}

		// Token: 0x02000568 RID: 1384
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_4
		{
			// Token: 0x0600334C RID: 13132 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_4()
			{
			}

			// Token: 0x0600334D RID: 13133 RVA: 0x00241A61 File Offset: 0x0023FC61
			internal bool <GetVAGCanRequests>b__11(OBDRequest x)
			{
				return x.Header == this.request.Header && x.Command.StartsWith("18");
			}

			// Token: 0x04001E29 RID: 7721
			public OBDRequest request;
		}

		// Token: 0x02000569 RID: 1385
		[CompilerGenerated]
		private sealed class <>c__DisplayClass27_0
		{
			// Token: 0x0600334E RID: 13134 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass27_0()
			{
			}

			// Token: 0x0600334F RID: 13135 RVA: 0x00241A8D File Offset: 0x0023FC8D
			internal bool <SkipHeaders>b__0(string x)
			{
				return x == this.skip[this.i];
			}

			// Token: 0x04001E2A RID: 7722
			public string[] skip;

			// Token: 0x04001E2B RID: 7723
			public int i;

			// Token: 0x04001E2C RID: 7724
			public Predicate<string> <>9__0;
		}

		// Token: 0x0200056A RID: 1386
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06003350 RID: 13136 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x06003351 RID: 13137 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__1(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003352 RID: 13138 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__5(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003353 RID: 13139 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__10(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003354 RID: 13140 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__21(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003355 RID: 13141 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__26(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003356 RID: 13142 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__40(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003357 RID: 13143 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__47(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003358 RID: 13144 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__50(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003359 RID: 13145 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__53(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x0600335A RID: 13146 RVA: 0x00241AA2 File Offset: 0x0023FCA2
			internal OBDRequest <GetBrandSequence>b__57(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x04001E2D RID: 7725
			public bool read;
		}

		// Token: 0x0200056B RID: 1387
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_1
		{
			// Token: 0x0600335B RID: 13147 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_1()
			{
			}

			// Token: 0x0600335C RID: 13148 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__2(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x0600335D RID: 13149 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__6(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x0600335E RID: 13150 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__11(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x0600335F RID: 13151 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__22(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003360 RID: 13152 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__27(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003361 RID: 13153 RVA: 0x00241AE3 File Offset: 0x0023FCE3
			internal OBDRequest <GetBrandSequence>b__33(string x)
			{
				return new OBDRequest(x.Substring(1), false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003362 RID: 13154 RVA: 0x00241AE3 File Offset: 0x0023FCE3
			internal OBDRequest <GetBrandSequence>b__34(string x)
			{
				return new OBDRequest(x.Substring(1), false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003363 RID: 13155 RVA: 0x00241AE3 File Offset: 0x0023FCE3
			internal OBDRequest <GetBrandSequence>b__37(string x)
			{
				return new OBDRequest(x.Substring(1), false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003364 RID: 13156 RVA: 0x00241AE3 File Offset: 0x0023FCE3
			internal OBDRequest <GetBrandSequence>b__38(string x)
			{
				return new OBDRequest(x.Substring(1), false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003365 RID: 13157 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__41(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003366 RID: 13158 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__48(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003367 RID: 13159 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__51(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003368 RID: 13160 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__54(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x06003369 RID: 13161 RVA: 0x00241AC0 File Offset: 0x0023FCC0
			internal OBDRequest <GetBrandSequence>b__58(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x0600336A RID: 13162 RVA: 0x00241AE3 File Offset: 0x0023FCE3
			internal OBDRequest <GetBrandSequence>b__60(string x)
			{
				return new OBDRequest(x.Substring(1), false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x0600336B RID: 13163 RVA: 0x00241AE3 File Offset: 0x0023FCE3
			internal OBDRequest <GetBrandSequence>b__61(string x)
			{
				return new OBDRequest(x.Substring(1), false, this.empty_pids)
				{
					DoNotDecode = !this.CS$<>8__locals1.read
				};
			}

			// Token: 0x04001E2E RID: 7726
			public List<PID> empty_pids;

			// Token: 0x04001E2F RID: 7727
			public DTCWorker.<>c__DisplayClass7_0 CS$<>8__locals1;
		}

		// Token: 0x0200056C RID: 1388
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_2
		{
			// Token: 0x0600336C RID: 13164 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_2()
			{
			}

			// Token: 0x0600336D RID: 13165 RVA: 0x00241B0C File Offset: 0x0023FD0C
			internal bool <GetBrandSequence>b__13(OBDRequest x)
			{
				return x.Header == this.req.Header;
			}

			// Token: 0x04001E30 RID: 7728
			public OBDRequest req;
		}

		// Token: 0x0200056D RID: 1389
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_3
		{
			// Token: 0x0600336E RID: 13166 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_3()
			{
			}

			// Token: 0x0600336F RID: 13167 RVA: 0x00241B24 File Offset: 0x0023FD24
			internal void <GetBrandSequence>b__14(OBDRequest request, string data)
			{
				if (data == null || !data.Contains("7E"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					foreach (OBDRequest obdrequest in this.gate_header_requests)
					{
						queueCopy.Remove(obdrequest);
					}
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x04001E31 RID: 7729
			public List<OBDRequest> gate_header_requests;
		}

		// Token: 0x0200056E RID: 1390
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_4
		{
			// Token: 0x06003370 RID: 13168 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_4()
			{
			}

			// Token: 0x06003371 RID: 13169 RVA: 0x00241BA0 File Offset: 0x0023FDA0
			internal bool <GetBrandSequence>b__18(OBDRequest x)
			{
				return x.Header == this.req.Header;
			}

			// Token: 0x04001E32 RID: 7730
			public OBDRequest req;
		}

		// Token: 0x0200056F RID: 1391
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_5
		{
			// Token: 0x06003372 RID: 13170 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_5()
			{
			}

			// Token: 0x06003373 RID: 13171 RVA: 0x00241BB8 File Offset: 0x0023FDB8
			internal void <GetBrandSequence>b__19(OBDRequest request, string data)
			{
				if (data == null || !data.Contains("7E"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					foreach (OBDRequest obdrequest in this.gate_header_requests)
					{
						queueCopy.Remove(obdrequest);
					}
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x04001E33 RID: 7731
			public List<OBDRequest> gate_header_requests;
		}

		// Token: 0x02000570 RID: 1392
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_6
		{
			// Token: 0x06003374 RID: 13172 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_6()
			{
			}

			// Token: 0x06003375 RID: 13173 RVA: 0x00241C34 File Offset: 0x0023FE34
			internal void <GetBrandSequence>b__30(OBDRequest request, string data)
			{
				if (data.Contains("CAN ERROR"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					List<OBDRequest> list = queueCopy;
					Predicate<OBDRequest> predicate;
					if ((predicate = this.<>9__31) == null)
					{
						predicate = (this.<>9__31 = (OBDRequest x) => this.can_requests.Contains(x));
					}
					list.RemoveAll(predicate);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x06003376 RID: 13174 RVA: 0x00241C8A File Offset: 0x0023FE8A
			internal bool <GetBrandSequence>b__31(OBDRequest x)
			{
				return this.can_requests.Contains(x);
			}

			// Token: 0x04001E34 RID: 7732
			public List<OBDRequest> can_requests;

			// Token: 0x04001E35 RID: 7733
			public Predicate<OBDRequest> <>9__31;

			// Token: 0x04001E36 RID: 7734
			public ResponseReceivedDelegate <>9__30;
		}

		// Token: 0x02000571 RID: 1393
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x06003377 RID: 13175 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x06003378 RID: 13176 RVA: 0x00241C98 File Offset: 0x0023FE98
			internal OBDRequest <GetRenaultCANRequests>b__1(string x)
			{
				return new OBDRequest(x.Substring(1), false)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x06003379 RID: 13177 RVA: 0x00241CB6 File Offset: 0x0023FEB6
			internal OBDRequest <GetRenaultCANRequests>b__2(string x)
			{
				return new OBDRequest(x, false, this.empty_pids)
				{
					DoNotDecode = !this.read
				};
			}

			// Token: 0x04001E37 RID: 7735
			public bool read;

			// Token: 0x04001E38 RID: 7736
			public List<PID> empty_pids;
		}

		// Token: 0x02000572 RID: 1394
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Start>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600337A RID: 13178 RVA: 0x00241CD4 File Offset: 0x0023FED4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0106;
					}
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0181;
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01EA;
					}
					case 4:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_025E;
					case 5:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0311;
					}
					default:
						taskAwaiter3 = Task.Run(delegate
						{
							DescriptionLoader.ResetCache();
							DescriptionLoader.PreloadDescriptionsForBrand(string.IsNullOrEmpty(SharedSettings.Current.BrandForDTC) ? SharedSettings.Current.SelectedBrand : SharedSettings.Current.BrandForDTC);
						}).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWorker.<Start>d__1>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					progress.Report("Clearing queue...");
					taskAwaiter3 = App.OBDReader.ClearRequestQueue().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWorker.<Start>d__1>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0106:
					taskAwaiter3.GetResult();
					progress.Report("Checking connection to ECU...");
					App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
					taskAwaiter5 = App.OBDReader.CheckECUConnectionWhileRunning(false).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 2;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWorker.<Start>d__1>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0181:
					if (taskAwaiter5.GetResult())
					{
						goto IL_0276;
					}
					taskAwaiter3 = App.OBDReader.Stop("DTCWorker Start, No ECU connection").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWorker.<Start>d__1>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01EA:
					taskAwaiter3.GetResult();
					progress.Report("No ECU connection, trying to reconnect...");
					taskAwaiter5 = App.OBDReader.ReinitializeConnectionToECU("DTCWorker:Start #65").GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 4;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWorker.<Start>d__1>(ref taskAwaiter5, ref this);
						return;
					}
					IL_025E:
					taskAwaiter5.GetResult();
					progress.Report("ECU connected...");
					IL_0276:
					App.OBDReader.CurrentMode = OBDDataReader.OBDModes.ReadDTC;
					progress.Report("Replacing queue...");
					DTCWorker.SetCheckLengthForRequests(sequence);
					App.OBDReader.ReplaceQueue(sequence);
					progress.Report("Replaced queue...");
					taskAwaiter3 = Task.Delay(300).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWorker.<Start>d__1>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0311:
					taskAwaiter3.GetResult();
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

			// Token: 0x0600337B RID: 13179 RVA: 0x00242044 File Offset: 0x00240244
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001E39 RID: 7737
			public int <>1__state;

			// Token: 0x04001E3A RID: 7738
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001E3B RID: 7739
			public IProgress<string> progress;

			// Token: 0x04001E3C RID: 7740
			public IEnumerable<OBDRequest> sequence;

			// Token: 0x04001E3D RID: 7741
			private TaskAwaiter <>u__1;

			// Token: 0x04001E3E RID: 7742
			private TaskAwaiter<bool> <>u__2;
		}
	}
}
