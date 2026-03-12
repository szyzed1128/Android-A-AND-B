using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.VWTP20
{
	// Token: 0x020003A3 RID: 931
	internal class VwTp20Worker
	{
		// Token: 0x0600272B RID: 10027 RVA: 0x001E0708 File Offset: 0x001DE908
		public VwTp20Worker(string channelAddress)
		{
			if (int.TryParse(channelAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out this.i_channelSetupAddress))
			{
				this.channelSetupAddress = channelAddress;
				return;
			}
			throw new ArgumentException("Wrong channel address");
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x001E07C8 File Offset: 0x001DE9C8
		private async Task<ValueTuple<bool, string>> OpenChannelUsingLoopAsync(string dataToSendAfterOpening, bool replaceQueue, bool removeResponseMarker)
		{
			VwTp20Worker.<>c__DisplayClass15_0 CS$<>8__locals1 = new VwTp20Worker.<>c__DisplayClass15_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.dataToSendAfterOpening = dataToSendAfterOpening;
			CS$<>8__locals1.removeResponseMarker = removeResponseMarker;
			CS$<>8__locals1.replaceQueue = replaceQueue;
			CS$<>8__locals1.delay_for_D7 = 5000;
			string text = this.channelSetupAddress + "C000100003011";
			CS$<>8__locals1.recieve_address = (this.i_channelSetupAddress + 512).ToString("X3");
			string text2 = "ATCRA" + CS$<>8__locals1.recieve_address;
			string[] array = new string[] { this.elmprotocolParams, this.elmProtocol, this.openChannelATST, text2 };
			string text3 = "ATSP" + App.OBDReader.CurrentProtocolNumber.ToString("X1");
			string[] array2 = new string[] { text3, "ATSTDEF", "ATAR" };
			CS$<>8__locals1.open_channel_request = new OBDRequest(text, "200", array, array2, false, null)
			{
				DoNotDecode = true,
				ELMFormat = ELMFormat.VwTp20
			};
			CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
			CS$<>8__locals1.open_result = false;
			CS$<>8__locals1.resultContainer = new DataContainer<string>("");
			string text4 = "A00F8AFF32FF";
			CS$<>8__locals1.dataSemaphore = null;
			CS$<>8__locals1.get_channel_parameters_request = new OBDRequest(text4, "", new string[] { "ATST18" }, array2, false, null)
			{
				DoNotDecode = true,
				ELMFormat = ELMFormat.VwTp20
			};
			CS$<>8__locals1.get_channel_parameters_request.ResponseReceived += delegate(OBDRequest get_channel_parameters_request2, string channel_parameters_data)
			{
				try
				{
					if (channel_parameters_data == null || channel_parameters_data.Contains("NO DATA") || channel_parameters_data.Contains("ERROR") || channel_parameters_data.Contains("RESET") || channel_parameters_data == "ELMFAILTP20")
					{
						CS$<>8__locals1.semaphore.Release();
					}
					else
					{
						string[] array3 = OBDDataReader.FilterHexAndNewLineOnly(channel_parameters_data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
						if (array3.Length == 0)
						{
							CS$<>8__locals1.semaphore.Release();
						}
						else
						{
							string text6 = array3.FirstOrDefault((string x) => x.IndexOf("A1") == 3);
							if (text6 == null)
							{
								CS$<>8__locals1.semaphore.Release();
							}
							else
							{
								byte[] array4 = BitHelpers.ConvertHexToBytesX(text6.Substring(3));
								CS$<>8__locals1.<>4__this.channelParametersMaxBlockSize = (int)array4[1];
								CS$<>8__locals1.<>4__this.channelParametersT1 = (int)array4[2];
								CS$<>8__locals1.<>4__this.channelParametersT2 = (int)array4[3];
								CS$<>8__locals1.<>4__this.channelParametersT3 = (int)array4[4];
								CS$<>8__locals1.<>4__this.channelParametersT4 = (int)array4[5];
								CS$<>8__locals1.<>4__this.sendCounter = 0;
								CS$<>8__locals1.<>4__this.lastChannelKeepAliveTime = App.OBDReader.stopwatch.Elapsed;
								App.OBDReader.ClearPendingAfterCommands();
								CS$<>8__locals1.open_result = true;
								if (!string.IsNullOrEmpty(CS$<>8__locals1.dataToSendAfterOpening))
								{
									ValueTuple<OBDRequest, SemaphoreSlim, DataContainer<string>> valueTuple = CS$<>8__locals1.<>4__this.BuildRequestForData(CS$<>8__locals1.dataToSendAfterOpening, CS$<>8__locals1.removeResponseMarker);
									OBDRequest item = valueTuple.Item1;
									CS$<>8__locals1.dataSemaphore = valueTuple.Item2;
									CS$<>8__locals1.resultContainer = valueTuple.Item3;
									int num = VWTPECU.ConvertCanTimingToMsec((byte)CS$<>8__locals1.<>4__this.channelParametersT3) / 4 + 6;
									if (SharedSettings.Current.ShowExperimental)
									{
										num += num / 2;
									}
									string text7 = "ATST" + num.ToString("X2");
									item.BeforeCommands = new string[] { text7 };
									if (CS$<>8__locals1.replaceQueue)
									{
										App.OBDReader.ClearPendingAfterCommands();
										App.OBDReader.ReplaceQueue(new OBDRequest[] { item });
									}
									else
									{
										App.OBDReader.AddRequestToQueue(item);
									}
								}
								CS$<>8__locals1.semaphore.Release();
							}
						}
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					if (CS$<>8__locals1.semaphore.CurrentCount == 0)
					{
						CS$<>8__locals1.semaphore.Release();
					}
				}
			};
			CS$<>8__locals1.open_channel_request.ResponseReceived += delegate(OBDRequest open_channel_request2, string data)
			{
				try
				{
					if (data == null || data.Contains("NO DATA") || data.Contains("ERROR") || data.Contains("RESET") || data == "ELMFAILTP20")
					{
						OBDDataReader obdreader = App.OBDReader;
						Predicate<OBDRequest> predicate;
						if ((predicate = CS$<>8__locals1.<>9__3) == null)
						{
							predicate = (CS$<>8__locals1.<>9__3 = (OBDRequest x) => x == CS$<>8__locals1.get_channel_parameters_request);
						}
						obdreader.RemoveFromQueue(predicate);
						CS$<>8__locals1.semaphore.Release();
					}
					else
					{
						string[] array5 = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
						if (array5.Length == 0)
						{
							OBDDataReader obdreader2 = App.OBDReader;
							Predicate<OBDRequest> predicate2;
							if ((predicate2 = CS$<>8__locals1.<>9__4) == null)
							{
								predicate2 = (CS$<>8__locals1.<>9__4 = (OBDRequest x) => x == CS$<>8__locals1.get_channel_parameters_request);
							}
							obdreader2.RemoveFromQueue(predicate2);
							CS$<>8__locals1.semaphore.Release();
						}
						else
						{
							IEnumerable<string> enumerable = array5;
							Func<string, bool> func;
							if ((func = CS$<>8__locals1.<>9__5) == null)
							{
								func = (CS$<>8__locals1.<>9__5 = (string x) => x.Length == 17 && x.StartsWith(CS$<>8__locals1.recieve_address));
							}
							string text8 = enumerable.Where(func).LastOrDefault<string>();
							if (text8 == null)
							{
								OBDDataReader obdreader3 = App.OBDReader;
								Predicate<OBDRequest> predicate3;
								if ((predicate3 = CS$<>8__locals1.<>9__6) == null)
								{
									predicate3 = (CS$<>8__locals1.<>9__6 = (OBDRequest x) => x == CS$<>8__locals1.get_channel_parameters_request);
								}
								obdreader3.RemoveFromQueue(predicate3);
								CS$<>8__locals1.semaphore.Release();
							}
							else
							{
								string text9 = text8.Substring(3);
								string text10 = text9.Substring(2, 2);
								if (text10 == "D7" && CS$<>8__locals1.delay_for_D7 < 30000)
								{
									App.OBDReader.DebugWriteSync(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds));
									Func<Task> func2;
									if ((func2 = CS$<>8__locals1.<>9__7) == null)
									{
										func2 = (CS$<>8__locals1.<>9__7 = delegate
										{
											VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d <<OpenChannelUsingLoopAsync>b__7>d;
											<<OpenChannelUsingLoopAsync>b__7>d.<>t__builder = AsyncTaskMethodBuilder.Create();
											<<OpenChannelUsingLoopAsync>b__7>d.<>4__this = CS$<>8__locals1;
											<<OpenChannelUsingLoopAsync>b__7>d.<>1__state = -1;
											<<OpenChannelUsingLoopAsync>b__7>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d>(ref <<OpenChannelUsingLoopAsync>b__7>d);
											return <<OpenChannelUsingLoopAsync>b__7>d.<>t__builder.Task;
										});
									}
									Task.Run(func2).Wait();
								}
								else if (text10 == "D0")
								{
									string text11 = text9[7].ToString() + text9[5].ToString() + text9[6].ToString();
									string text12 = text9[11].ToString() + text9[8].ToString() + text9[9].ToString();
									CS$<>8__locals1.<>4__this.channelDataListenToCANId = text11;
									CS$<>8__locals1.<>4__this.channelDataSendToCANId = text12;
									string text13 = "ATCRA" + CS$<>8__locals1.<>4__this.channelDataListenToCANId;
									string[] array6 = new string[] { "ATST32", text13 };
									CS$<>8__locals1.get_channel_parameters_request.BeforeCommands = array6;
									CS$<>8__locals1.get_channel_parameters_request.Header = text12;
									App.OBDReader.ClearPendingAfterCommands();
								}
								else
								{
									OBDDataReader obdreader4 = App.OBDReader;
									Predicate<OBDRequest> predicate4;
									if ((predicate4 = CS$<>8__locals1.<>9__8) == null)
									{
										predicate4 = (CS$<>8__locals1.<>9__8 = (OBDRequest x) => x == CS$<>8__locals1.get_channel_parameters_request);
									}
									obdreader4.RemoveFromQueue(predicate4);
									CS$<>8__locals1.semaphore.Release();
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
			};
			CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
			App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals1.open_channel_request, CS$<>8__locals1.get_channel_parameters_request });
			await CS$<>8__locals1.semaphore.WaitAsync();
			string text5 = "";
			if (CS$<>8__locals1.dataSemaphore != null)
			{
				await CS$<>8__locals1.dataSemaphore.WaitAsync();
				text5 = CS$<>8__locals1.resultContainer.Value;
			}
			return new ValueTuple<bool, string>(CS$<>8__locals1.open_result, text5);
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x001E0824 File Offset: 0x001DEA24
		private bool IsChannelAlive()
		{
			return !(this.lastChannelKeepAliveTime == TimeSpan.Zero) && (App.OBDReader.stopwatch.Elapsed - this.lastChannelKeepAliveTime).TotalSeconds < 1.0;
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x001E0878 File Offset: 0x001DEA78
		public async Task CloseChannel(bool replaceQueue)
		{
			if (this.IsChannelAlive())
			{
				"ATCRA" + this.channelDataListenToCANId;
				string[] array = new string[] { this.elmProtocol };
				string text = "ATSP" + App.OBDReader.CurrentProtocolNumber.ToString("X1");
				string[] array2 = new string[] { text, "ATSTDEF", "ATAR" };
				this.sendCounter.ToString("X1");
				OBDRequest obdrequest = new OBDRequest("A80", this.channelDataSendToCANId, array, array2, false, null)
				{
					ELMFormat = ELMFormat.VwTp20,
					DoNotDecode = true
				};
				SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
				obdrequest.ResponseReceived += delegate(OBDRequest request2, string data)
				{
					semaphore.Release();
				};
				if (replaceQueue)
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
				}
				else
				{
					App.OBDReader.AddRequestToQueue(obdrequest);
				}
				await semaphore.WaitAsync();
			}
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x001E08C4 File Offset: 0x001DEAC4
		public async Task<ValueTuple<bool, string>> SendAndReadDataUsingRequestLoop(string data, bool replaceQueue, bool removeResponseMarker)
		{
			ValueTuple<bool, string> valueTuple2;
			if (!this.IsChannelAlive())
			{
				ValueTuple<bool, string> valueTuple = await this.OpenChannelUsingLoopAsync(data, replaceQueue, removeResponseMarker);
				bool item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				if (item)
				{
					valueTuple2 = new ValueTuple<bool, string>(true, item2);
				}
				else
				{
					valueTuple2 = new ValueTuple<bool, string>(item, "");
				}
			}
			else
			{
				ValueTuple<OBDRequest, SemaphoreSlim, DataContainer<string>> valueTuple3 = this.BuildRequestForData(data, removeResponseMarker);
				OBDRequest item3 = valueTuple3.Item1;
				SemaphoreSlim item4 = valueTuple3.Item2;
				DataContainer<string> resultContainer = valueTuple3.Item3;
				item3.BeforeCommands = new string[0];
				if (replaceQueue)
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[] { item3 });
				}
				else
				{
					App.OBDReader.AddRequestToQueue(item3);
				}
				await item4.WaitAsync();
				valueTuple2 = new ValueTuple<bool, string>(true, resultContainer.Value);
			}
			return valueTuple2;
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x001E0920 File Offset: 0x001DEB20
		private ValueTuple<OBDRequest, SemaphoreSlim, DataContainer<string>> BuildRequestForData(string data, bool removeResponseMarker)
		{
			VwTp20Worker.<>c__DisplayClass21_0 CS$<>8__locals1 = new VwTp20Worker.<>c__DisplayClass21_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.removeResponseMarker = removeResponseMarker;
			string text = "ATST" + (this.channelParametersT3 / 4 + 2).ToString("X2");
			string text2 = "ATCRA" + this.channelDataListenToCANId;
			string[] array = new string[] { this.elmprotocolParams, this.elmProtocol, text, text2 };
			string text3 = "ATSP" + App.OBDReader.CurrentProtocolNumber.ToString("X1");
			string[] array2 = new string[] { text3, "ATSTDEF", "ATAR" };
			string text4 = this.sendCounter.ToString("X1");
			string text5 = "1";
			string text6 = (data.Length / 2).ToString("X4");
			string text7 = text5 + text4 + text6 + data;
			CS$<>8__locals1.result_hex = "";
			CS$<>8__locals1.resultContainer = new DataContainer<string>("");
			this.sendCounter++;
			CS$<>8__locals1.request = new OBDRequest(text7, this.channelDataSendToCANId, array, array2, false, null)
			{
				DoNotDecode = true,
				ELMFormat = ELMFormat.VwTp20
			};
			CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
			CS$<>8__locals1.result_data = new StringBuilder();
			CS$<>8__locals1.data_length = 0;
			CS$<>8__locals1.response_marker = OBDRequest.GetResponseMarkerFromCommand(data);
			CS$<>8__locals1.request.ResponseReceived += delegate(OBDRequest request2, string response_data)
			{
				VwTp20Worker.<>c__DisplayClass21_0.<<BuildRequestForData>b__0>d <<BuildRequestForData>b__0>d;
				<<BuildRequestForData>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<BuildRequestForData>b__0>d.<>4__this = CS$<>8__locals1;
				<<BuildRequestForData>b__0>d.request2 = request2;
				<<BuildRequestForData>b__0>d.response_data = response_data;
				<<BuildRequestForData>b__0>d.<>1__state = -1;
				<<BuildRequestForData>b__0>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass21_0.<<BuildRequestForData>b__0>d>(ref <<BuildRequestForData>b__0>d);
			};
			return new ValueTuple<OBDRequest, SemaphoreSlim, DataContainer<string>>(CS$<>8__locals1.request, CS$<>8__locals1.semaphore, CS$<>8__locals1.resultContainer);
		}

		// Token: 0x04001559 RID: 5465
		private readonly string channelSetupAddress = "";

		// Token: 0x0400155A RID: 5466
		private readonly int i_channelSetupAddress;

		// Token: 0x0400155B RID: 5467
		private string elmprotocolParams = "ATPBC001";

		// Token: 0x0400155C RID: 5468
		private string elmProtocol = "ATSPB";

		// Token: 0x0400155D RID: 5469
		private string openChannelATST = "ATST06";

		// Token: 0x0400155E RID: 5470
		private string channelDataListenToCANId = "";

		// Token: 0x0400155F RID: 5471
		private string channelDataSendToCANId = "";

		// Token: 0x04001560 RID: 5472
		private int channelParametersMaxBlockSize = 15;

		// Token: 0x04001561 RID: 5473
		private int channelParametersT1 = 255;

		// Token: 0x04001562 RID: 5474
		private int channelParametersT2 = 255;

		// Token: 0x04001563 RID: 5475
		private int channelParametersT3 = 255;

		// Token: 0x04001564 RID: 5476
		private int channelParametersT4 = 255;

		// Token: 0x04001565 RID: 5477
		private TimeSpan lastChannelKeepAliveTime = TimeSpan.Zero;

		// Token: 0x04001566 RID: 5478
		private int sendCounter;

		// Token: 0x04001567 RID: 5479
		private const string REQUEST_REPEATED_STRING = "VWTPREPEAT";

		// Token: 0x04001568 RID: 5480
		public const string ELM_FAIL_ON_TP20 = "ELMFAILTP20";

		// Token: 0x020003A4 RID: 932
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002731 RID: 10033 RVA: 0x001E0ABA File Offset: 0x001DECBA
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002732 RID: 10034 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002733 RID: 10035 RVA: 0x001E0AC6 File Offset: 0x001DECC6
			internal bool <OpenChannelUsingLoopAsync>b__15_2(string x)
			{
				return x.IndexOf("A1") == 3;
			}

			// Token: 0x06002734 RID: 10036 RVA: 0x001E0AD6 File Offset: 0x001DECD6
			internal string <BuildRequestForData>b__21_2(string x)
			{
				return x.Substring(3);
			}

			// Token: 0x06002735 RID: 10037 RVA: 0x001E0AE0 File Offset: 0x001DECE0
			internal async Task <BuildRequestForData>b__21_8()
			{
				await App.OBDReader.SendString("A3");
				await App.OBDReader.ReadData(2500, null, -1);
			}

			// Token: 0x04001569 RID: 5481
			public static readonly VwTp20Worker.<>c <>9 = new VwTp20Worker.<>c();

			// Token: 0x0400156A RID: 5482
			public static Func<string, bool> <>9__15_2;

			// Token: 0x0400156B RID: 5483
			public static Func<string, string> <>9__21_2;

			// Token: 0x0400156C RID: 5484
			public static Func<Task> <>9__21_8;

			// Token: 0x020003A5 RID: 933
			[StructLayout(LayoutKind.Auto)]
			private struct <<BuildRequestForData>b__21_8>d : IAsyncStateMachine
			{
				// Token: 0x06002736 RID: 10038 RVA: 0x001E0B1C File Offset: 0x001DED1C
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					try
					{
						TaskAwaiter<string> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter<string> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<string>);
								num2 = -1;
								goto IL_00D1;
							}
							taskAwaiter3 = App.OBDReader.SendString("A3").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c.<<BuildRequestForData>b__21_8>d>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter3.GetResult();
						taskAwaiter = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VwTp20Worker.<>c.<<BuildRequestForData>b__21_8>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_00D1:
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

				// Token: 0x06002737 RID: 10039 RVA: 0x001E0C40 File Offset: 0x001DEE40
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400156D RID: 5485
				public int <>1__state;

				// Token: 0x0400156E RID: 5486
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x0400156F RID: 5487
				private TaskAwaiter <>u__1;

				// Token: 0x04001570 RID: 5488
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x020003A6 RID: 934
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			// Token: 0x06002738 RID: 10040 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_0()
			{
			}

			// Token: 0x06002739 RID: 10041 RVA: 0x001E0C50 File Offset: 0x001DEE50
			internal void <OpenChannelUsingLoopAsync>b__0(OBDRequest get_channel_parameters_request2, string channel_parameters_data)
			{
				try
				{
					if (channel_parameters_data == null || channel_parameters_data.Contains("NO DATA") || channel_parameters_data.Contains("ERROR") || channel_parameters_data.Contains("RESET") || channel_parameters_data == "ELMFAILTP20")
					{
						this.semaphore.Release();
					}
					else
					{
						string[] array = OBDDataReader.FilterHexAndNewLineOnly(channel_parameters_data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
						if (array.Length == 0)
						{
							this.semaphore.Release();
						}
						else
						{
							string text = array.FirstOrDefault((string x) => x.IndexOf("A1") == 3);
							if (text == null)
							{
								this.semaphore.Release();
							}
							else
							{
								byte[] array2 = BitHelpers.ConvertHexToBytesX(text.Substring(3));
								this.<>4__this.channelParametersMaxBlockSize = (int)array2[1];
								this.<>4__this.channelParametersT1 = (int)array2[2];
								this.<>4__this.channelParametersT2 = (int)array2[3];
								this.<>4__this.channelParametersT3 = (int)array2[4];
								this.<>4__this.channelParametersT4 = (int)array2[5];
								this.<>4__this.sendCounter = 0;
								this.<>4__this.lastChannelKeepAliveTime = App.OBDReader.stopwatch.Elapsed;
								App.OBDReader.ClearPendingAfterCommands();
								this.open_result = true;
								if (!string.IsNullOrEmpty(this.dataToSendAfterOpening))
								{
									ValueTuple<OBDRequest, SemaphoreSlim, DataContainer<string>> valueTuple = this.<>4__this.BuildRequestForData(this.dataToSendAfterOpening, this.removeResponseMarker);
									OBDRequest item = valueTuple.Item1;
									this.dataSemaphore = valueTuple.Item2;
									this.resultContainer = valueTuple.Item3;
									int num = VWTPECU.ConvertCanTimingToMsec((byte)this.<>4__this.channelParametersT3) / 4 + 6;
									if (SharedSettings.Current.ShowExperimental)
									{
										num += num / 2;
									}
									string text2 = "ATST" + num.ToString("X2");
									item.BeforeCommands = new string[] { text2 };
									if (this.replaceQueue)
									{
										App.OBDReader.ClearPendingAfterCommands();
										App.OBDReader.ReplaceQueue(new OBDRequest[] { item });
									}
									else
									{
										App.OBDReader.AddRequestToQueue(item);
									}
								}
								this.semaphore.Release();
							}
						}
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					if (this.semaphore.CurrentCount == 0)
					{
						this.semaphore.Release();
					}
				}
			}

			// Token: 0x0600273A RID: 10042 RVA: 0x001E0ED0 File Offset: 0x001DF0D0
			internal void <OpenChannelUsingLoopAsync>b__1(OBDRequest open_channel_request2, string data)
			{
				try
				{
					if (data == null || data.Contains("NO DATA") || data.Contains("ERROR") || data.Contains("RESET") || data == "ELMFAILTP20")
					{
						OBDDataReader obdreader = App.OBDReader;
						Predicate<OBDRequest> predicate;
						if ((predicate = this.<>9__3) == null)
						{
							predicate = (this.<>9__3 = (OBDRequest x) => x == this.get_channel_parameters_request);
						}
						obdreader.RemoveFromQueue(predicate);
						this.semaphore.Release();
					}
					else
					{
						string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
						if (array.Length == 0)
						{
							OBDDataReader obdreader2 = App.OBDReader;
							Predicate<OBDRequest> predicate2;
							if ((predicate2 = this.<>9__4) == null)
							{
								predicate2 = (this.<>9__4 = (OBDRequest x) => x == this.get_channel_parameters_request);
							}
							obdreader2.RemoveFromQueue(predicate2);
							this.semaphore.Release();
						}
						else
						{
							IEnumerable<string> enumerable = array;
							Func<string, bool> func;
							if ((func = this.<>9__5) == null)
							{
								func = (this.<>9__5 = (string x) => x.Length == 17 && x.StartsWith(this.recieve_address));
							}
							string text = enumerable.Where(func).LastOrDefault<string>();
							if (text == null)
							{
								OBDDataReader obdreader3 = App.OBDReader;
								Predicate<OBDRequest> predicate3;
								if ((predicate3 = this.<>9__6) == null)
								{
									predicate3 = (this.<>9__6 = (OBDRequest x) => x == this.get_channel_parameters_request);
								}
								obdreader3.RemoveFromQueue(predicate3);
								this.semaphore.Release();
							}
							else
							{
								string text2 = text.Substring(3);
								string text3 = text2.Substring(2, 2);
								if (text3 == "D7" && this.delay_for_D7 < 30000)
								{
									App.OBDReader.DebugWriteSync(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds));
									Func<Task> func2;
									if ((func2 = this.<>9__7) == null)
									{
										func2 = (this.<>9__7 = async delegate
										{
											await App.OBDReader.SendString("ATPC");
											await App.OBDReader.ReadData(2500, null, -1);
											await App.OBDReader.DebugWrite(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds));
											await Task.Delay(this.delay_for_D7);
											this.delay_for_D7 += 5000;
											await App.OBDReader.DebugWrite(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds));
											App.OBDReader.InsertRequestInQueue(this.open_channel_request);
										});
									}
									Task.Run(func2).Wait();
								}
								else if (text3 == "D0")
								{
									string text4 = text2[7].ToString() + text2[5].ToString() + text2[6].ToString();
									string text5 = text2[11].ToString() + text2[8].ToString() + text2[9].ToString();
									this.<>4__this.channelDataListenToCANId = text4;
									this.<>4__this.channelDataSendToCANId = text5;
									string text6 = "ATCRA" + this.<>4__this.channelDataListenToCANId;
									string[] array2 = new string[] { "ATST32", text6 };
									this.get_channel_parameters_request.BeforeCommands = array2;
									this.get_channel_parameters_request.Header = text5;
									App.OBDReader.ClearPendingAfterCommands();
								}
								else
								{
									OBDDataReader obdreader4 = App.OBDReader;
									Predicate<OBDRequest> predicate4;
									if ((predicate4 = this.<>9__8) == null)
									{
										predicate4 = (this.<>9__8 = (OBDRequest x) => x == this.get_channel_parameters_request);
									}
									obdreader4.RemoveFromQueue(predicate4);
									this.semaphore.Release();
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x0600273B RID: 10043 RVA: 0x001E11EC File Offset: 0x001DF3EC
			internal bool <OpenChannelUsingLoopAsync>b__3(OBDRequest x)
			{
				return x == this.get_channel_parameters_request;
			}

			// Token: 0x0600273C RID: 10044 RVA: 0x001E11EC File Offset: 0x001DF3EC
			internal bool <OpenChannelUsingLoopAsync>b__4(OBDRequest x)
			{
				return x == this.get_channel_parameters_request;
			}

			// Token: 0x0600273D RID: 10045 RVA: 0x001E11F7 File Offset: 0x001DF3F7
			internal bool <OpenChannelUsingLoopAsync>b__5(string x)
			{
				return x.Length == 17 && x.StartsWith(this.recieve_address);
			}

			// Token: 0x0600273E RID: 10046 RVA: 0x001E11EC File Offset: 0x001DF3EC
			internal bool <OpenChannelUsingLoopAsync>b__6(OBDRequest x)
			{
				return x == this.get_channel_parameters_request;
			}

			// Token: 0x0600273F RID: 10047 RVA: 0x001E1214 File Offset: 0x001DF414
			internal async Task <OpenChannelUsingLoopAsync>b__7()
			{
				await App.OBDReader.SendString("ATPC");
				await App.OBDReader.ReadData(2500, null, -1);
				await App.OBDReader.DebugWrite(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds));
				await Task.Delay(this.delay_for_D7);
				this.delay_for_D7 += 5000;
				await App.OBDReader.DebugWrite(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds));
				App.OBDReader.InsertRequestInQueue(this.open_channel_request);
			}

			// Token: 0x06002740 RID: 10048 RVA: 0x001E11EC File Offset: 0x001DF3EC
			internal bool <OpenChannelUsingLoopAsync>b__8(OBDRequest x)
			{
				return x == this.get_channel_parameters_request;
			}

			// Token: 0x04001571 RID: 5489
			public SemaphoreSlim semaphore;

			// Token: 0x04001572 RID: 5490
			public VwTp20Worker <>4__this;

			// Token: 0x04001573 RID: 5491
			public bool open_result;

			// Token: 0x04001574 RID: 5492
			public string dataToSendAfterOpening;

			// Token: 0x04001575 RID: 5493
			public bool removeResponseMarker;

			// Token: 0x04001576 RID: 5494
			public SemaphoreSlim dataSemaphore;

			// Token: 0x04001577 RID: 5495
			public DataContainer<string> resultContainer;

			// Token: 0x04001578 RID: 5496
			public bool replaceQueue;

			// Token: 0x04001579 RID: 5497
			public OBDRequest get_channel_parameters_request;

			// Token: 0x0400157A RID: 5498
			public string recieve_address;

			// Token: 0x0400157B RID: 5499
			public int delay_for_D7;

			// Token: 0x0400157C RID: 5500
			public OBDRequest open_channel_request;

			// Token: 0x0400157D RID: 5501
			public Predicate<OBDRequest> <>9__3;

			// Token: 0x0400157E RID: 5502
			public Predicate<OBDRequest> <>9__4;

			// Token: 0x0400157F RID: 5503
			public Func<string, bool> <>9__5;

			// Token: 0x04001580 RID: 5504
			public Predicate<OBDRequest> <>9__6;

			// Token: 0x04001581 RID: 5505
			public Func<Task> <>9__7;

			// Token: 0x04001582 RID: 5506
			public Predicate<OBDRequest> <>9__8;

			// Token: 0x020003A7 RID: 935
			[StructLayout(LayoutKind.Auto)]
			private struct <<OpenChannelUsingLoopAsync>b__7>d : IAsyncStateMachine
			{
				// Token: 0x06002741 RID: 10049 RVA: 0x001E1258 File Offset: 0x001DF458
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					VwTp20Worker.<>c__DisplayClass15_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter taskAwaiter;
						TaskAwaiter<string> taskAwaiter3;
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
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_00EB;
						}
						case 2:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0172;
						}
						case 3:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_01D2;
						}
						case 4:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0267;
						}
						default:
							taskAwaiter = App.OBDReader.SendString("ATPC").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						taskAwaiter.GetResult();
						taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d>(ref taskAwaiter3, ref this);
							return;
						}
						IL_00EB:
						taskAwaiter3.GetResult();
						taskAwaiter = App.OBDReader.DebugWrite(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_0172:
						taskAwaiter.GetResult();
						taskAwaiter = Task.Delay(CS$<>8__locals1.delay_for_D7).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_01D2:
						taskAwaiter.GetResult();
						CS$<>8__locals1.delay_for_D7 += 5000;
						taskAwaiter = App.OBDReader.DebugWrite(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_0267:
						taskAwaiter.GetResult();
						App.OBDReader.InsertRequestInQueue(CS$<>8__locals1.open_channel_request);
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

				// Token: 0x06002742 RID: 10050 RVA: 0x001E1530 File Offset: 0x001DF730
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04001583 RID: 5507
				public int <>1__state;

				// Token: 0x04001584 RID: 5508
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04001585 RID: 5509
				public VwTp20Worker.<>c__DisplayClass15_0 <>4__this;

				// Token: 0x04001586 RID: 5510
				private TaskAwaiter <>u__1;

				// Token: 0x04001587 RID: 5511
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x020003A8 RID: 936
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x06002743 RID: 10051 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x06002744 RID: 10052 RVA: 0x001E153E File Offset: 0x001DF73E
			internal void <CloseChannel>b__0(OBDRequest request2, string data)
			{
				this.semaphore.Release();
			}

			// Token: 0x04001588 RID: 5512
			public SemaphoreSlim semaphore;
		}

		// Token: 0x020003A9 RID: 937
		[CompilerGenerated]
		private sealed class <>c__DisplayClass21_0
		{
			// Token: 0x06002745 RID: 10053 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass21_0()
			{
			}

			// Token: 0x06002746 RID: 10054 RVA: 0x001E154C File Offset: 0x001DF74C
			internal async void <BuildRequestForData>b__0(OBDRequest request2, string response_data)
			{
				VwTp20Worker.<>c__DisplayClass21_1 CS$<>8__locals1 = new VwTp20Worker.<>c__DisplayClass21_1();
				CS$<>8__locals1.CS$<>8__locals1 = this;
				CS$<>8__locals1.skip_finally = false;
				try
				{
					if (response_data != null && !response_data.Contains("NO DATA") && !response_data.Contains("ERROR") && !response_data.Contains("RESET") && !(response_data == "ELMFAILTP20"))
					{
						string command = request2.Command;
						string[] array = OBDDataReader.FilterHexAndNewLineOnly(response_data).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
						IEnumerable<string> enumerable = array;
						Func<string, bool> func;
						if ((func = this.<>9__1) == null)
						{
							func = (this.<>9__1 = (string x) => x.StartsWith(this.<>4__this.channelDataListenToCANId));
						}
						List<string> response_lines_data = (from x in enumerable.Where(func)
							select x.Substring(3)).ToList<string>();
						List<string> list = response_lines_data;
						Predicate<string> predicate;
						if ((predicate = this.<>9__3) == null)
						{
							predicate = (this.<>9__3 = (string x) => x.IndexOf(this.response_marker) == 6);
						}
						int first_line_idx = list.FindIndex(predicate);
						int num3;
						for (int i = 0; i < response_lines_data.Count; i = num3 + 1)
						{
							string text = response_lines_data[i];
							char c = text[0];
							if (c <= '9')
							{
								switch (c)
								{
								case '0':
								case '1':
								case '2':
								case '3':
									if (i == first_line_idx)
									{
										this.result_data.Append(text.Substring(6));
										string text2 = text.Substring(2, 2);
										string text3 = text.Substring(4, 2);
										this.data_length = int.Parse(text2, NumberStyles.HexNumber) * 256 + int.Parse(text3, NumberStyles.HexNumber);
									}
									else
									{
										this.result_data.Append(text.Substring(2));
									}
									if (text.StartsWith('1'))
									{
										VwTp20Worker.<>c__DisplayClass21_2 CS$<>8__locals2 = new VwTp20Worker.<>c__DisplayClass21_2();
										int num = int.Parse(text[1].ToString(), NumberStyles.HexNumber);
										CS$<>8__locals2.ack = "B" + ((num + 1) & 15).ToString("X1") + "0";
										Task.Run(delegate
										{
											VwTp20Worker.<>c__DisplayClass21_2.<<BuildRequestForData>b__5>d <<BuildRequestForData>b__5>d;
											<<BuildRequestForData>b__5>d.<>t__builder = AsyncTaskMethodBuilder.Create();
											<<BuildRequestForData>b__5>d.<>4__this = CS$<>8__locals2;
											<<BuildRequestForData>b__5>d.<>1__state = -1;
											<<BuildRequestForData>b__5>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass21_2.<<BuildRequestForData>b__5>d>(ref <<BuildRequestForData>b__5>d);
											return <<BuildRequestForData>b__5>d.<>t__builder.Task;
										}).Wait();
									}
									else if (text.StartsWith('0'))
									{
										VwTp20Worker.<>c__DisplayClass21_3 CS$<>8__locals3 = new VwTp20Worker.<>c__DisplayClass21_3();
										int num2 = int.Parse(text[1].ToString(), NumberStyles.HexNumber);
										CS$<>8__locals3.ack = "B" + ((num2 + 1) & 15).ToString("X1");
										CS$<>8__locals3.new_data_portion = "";
										Task.Run(delegate
										{
											VwTp20Worker.<>c__DisplayClass21_3.<<BuildRequestForData>b__6>d <<BuildRequestForData>b__6>d;
											<<BuildRequestForData>b__6>d.<>t__builder = AsyncTaskMethodBuilder.Create();
											<<BuildRequestForData>b__6>d.<>4__this = CS$<>8__locals3;
											<<BuildRequestForData>b__6>d.<>1__state = -1;
											<<BuildRequestForData>b__6>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass21_3.<<BuildRequestForData>b__6>d>(ref <<BuildRequestForData>b__6>d);
											return <<BuildRequestForData>b__6>d.<>t__builder.Task;
										}).Wait();
										IEnumerable<string> enumerable2 = OBDDataReader.FilterHexAndNewLineOnly(response_data).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
										Func<string, bool> func2;
										if ((func2 = this.<>9__7) == null)
										{
											func2 = (this.<>9__7 = (string x) => x.StartsWith(this.<>4__this.channelDataListenToCANId));
										}
										List<string> list2 = enumerable2.Where(func2).ToList<string>();
										if (list2 != null)
										{
											response_lines_data.AddRange(list2);
										}
									}
									if ((text.StartsWith('1') || text.StartsWith('3')) && (App.OBDReader.stopwatch.Elapsed - this.<>4__this.lastChannelKeepAliveTime).TotalSeconds > 0.7)
									{
										Task.Run(async delegate
										{
											await App.OBDReader.SendString("A3");
											await App.OBDReader.ReadData(2500, null, -1);
										}).Wait();
										this.<>4__this.lastChannelKeepAliveTime = App.OBDReader.stopwatch.Elapsed;
									}
									break;
								default:
									if (c == '9')
									{
										if (i == response_lines_data.Count - 1)
										{
											Func<Task> func3;
											if ((func3 = CS$<>8__locals1.<>9__4) == null)
											{
												func3 = (CS$<>8__locals1.<>9__4 = delegate
												{
													VwTp20Worker.<>c__DisplayClass21_1.<<BuildRequestForData>b__4>d <<BuildRequestForData>b__4>d;
													<<BuildRequestForData>b__4>d.<>t__builder = AsyncTaskMethodBuilder.Create();
													<<BuildRequestForData>b__4>d.<>4__this = CS$<>8__locals1;
													<<BuildRequestForData>b__4>d.<>1__state = -1;
													<<BuildRequestForData>b__4>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass21_1.<<BuildRequestForData>b__4>d>(ref <<BuildRequestForData>b__4>d);
													return <<BuildRequestForData>b__4>d.<>t__builder.Task;
												});
											}
											Task.Run(func3).Wait();
											return;
										}
									}
									break;
								}
							}
							else if (c != 'A')
							{
								if (c != 'B')
								{
								}
							}
							else
							{
								string text4 = string.Concat(new string[]
								{
									"A1",
									this.<>4__this.channelParametersMaxBlockSize.ToString("X2"),
									this.<>4__this.channelParametersT1.ToString("X2"),
									this.<>4__this.channelParametersT2.ToString("X2"),
									this.<>4__this.channelParametersT3.ToString("X2"),
									this.<>4__this.channelParametersT4.ToString("X2"),
									"0"
								});
								await App.OBDReader.SendString(text4);
								await App.OBDReader.ReadData(2500, null, -1);
							}
							num3 = i;
						}
						response_lines_data = null;
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					if (!CS$<>8__locals1.skip_finally)
					{
						this.result_hex = this.result_data.ToString();
						if (this.data_length < this.result_hex.Length / 2)
						{
							this.result_hex = this.result_hex.Substring(0, this.data_length * 2);
						}
						if (this.removeResponseMarker && this.result_hex.StartsWith(this.response_marker))
						{
							this.result_hex = this.result_hex.Substring(this.response_marker.Length);
						}
						this.resultContainer.Value = this.result_hex;
						if (this.semaphore.CurrentCount == 0)
						{
							this.semaphore.Release();
						}
					}
				}
			}

			// Token: 0x06002747 RID: 10055 RVA: 0x001E1593 File Offset: 0x001DF793
			internal bool <BuildRequestForData>b__1(string x)
			{
				return x.StartsWith(this.<>4__this.channelDataListenToCANId);
			}

			// Token: 0x06002748 RID: 10056 RVA: 0x001E15A6 File Offset: 0x001DF7A6
			internal bool <BuildRequestForData>b__3(string x)
			{
				return x.IndexOf(this.response_marker) == 6;
			}

			// Token: 0x06002749 RID: 10057 RVA: 0x001E1593 File Offset: 0x001DF793
			internal bool <BuildRequestForData>b__7(string x)
			{
				return x.StartsWith(this.<>4__this.channelDataListenToCANId);
			}

			// Token: 0x04001589 RID: 5513
			public VwTp20Worker <>4__this;

			// Token: 0x0400158A RID: 5514
			public string response_marker;

			// Token: 0x0400158B RID: 5515
			public OBDRequest request;

			// Token: 0x0400158C RID: 5516
			public StringBuilder result_data;

			// Token: 0x0400158D RID: 5517
			public int data_length;

			// Token: 0x0400158E RID: 5518
			public string result_hex;

			// Token: 0x0400158F RID: 5519
			public bool removeResponseMarker;

			// Token: 0x04001590 RID: 5520
			public DataContainer<string> resultContainer;

			// Token: 0x04001591 RID: 5521
			public SemaphoreSlim semaphore;

			// Token: 0x04001592 RID: 5522
			public Func<string, bool> <>9__1;

			// Token: 0x04001593 RID: 5523
			public Predicate<string> <>9__3;

			// Token: 0x04001594 RID: 5524
			public Func<string, bool> <>9__7;

			// Token: 0x020003AA RID: 938
			[StructLayout(LayoutKind.Auto)]
			private struct <<BuildRequestForData>b__0>d : IAsyncStateMachine
			{
				// Token: 0x0600274A RID: 10058 RVA: 0x001E15B8 File Offset: 0x001DF7B8
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					VwTp20Worker.<>c__DisplayClass21_0 CS$<>8__locals2 = this;
					try
					{
						if (num > 1)
						{
							CS$<>8__locals1 = new VwTp20Worker.<>c__DisplayClass21_1();
							CS$<>8__locals1.CS$<>8__locals1 = CS$<>8__locals2;
							CS$<>8__locals1.skip_finally = false;
						}
						try
						{
							TaskAwaiter<string> taskAwaiter;
							TaskAwaiter taskAwaiter3;
							if (num != 0)
							{
								if (num == 1)
								{
									TaskAwaiter<string> taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<string>);
									num = (num2 = -1);
									goto IL_05B6;
								}
								if (response_data == null || response_data.Contains("NO DATA") || response_data.Contains("ERROR") || response_data.Contains("RESET") || response_data == "ELMFAILTP20")
								{
									goto IL_06D2;
								}
								string command = request2.Command;
								string[] array = OBDDataReader.FilterHexAndNewLineOnly(response_data).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
								IEnumerable<string> enumerable = array;
								Func<string, bool> func;
								if ((func = CS$<>8__locals2.<>9__1) == null)
								{
									func = (CS$<>8__locals2.<>9__1 = (string x) => x.StartsWith(CS$<>8__locals2.<>4__this.channelDataListenToCANId));
								}
								response_lines_data = (from x in enumerable.Where(func)
									select x.Substring(3)).ToList<string>();
								List<string> list = response_lines_data;
								Predicate<string> predicate;
								if ((predicate = CS$<>8__locals2.<>9__3) == null)
								{
									predicate = (CS$<>8__locals2.<>9__3 = (string x) => x.IndexOf(CS$<>8__locals2.response_marker) == 6);
								}
								first_line_idx = list.FindIndex(predicate);
								i = 0;
								goto IL_05D0;
							}
							else
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							IL_054D:
							taskAwaiter3.GetResult();
							taskAwaiter = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 1);
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VwTp20Worker.<>c__DisplayClass21_0.<<BuildRequestForData>b__0>d>(ref taskAwaiter, ref this);
								return;
							}
							IL_05B6:
							taskAwaiter.GetResult();
							IL_05BE:
							int num3 = i;
							i = num3 + 1;
							IL_05D0:
							if (i >= response_lines_data.Count)
							{
								response_lines_data = null;
							}
							else
							{
								string text = response_lines_data[i];
								char c = text[0];
								if (c <= '9')
								{
									switch (c)
									{
									case '0':
									case '1':
									case '2':
									case '3':
										if (i == first_line_idx)
										{
											CS$<>8__locals2.result_data.Append(text.Substring(6));
											string text2 = text.Substring(2, 2);
											string text3 = text.Substring(4, 2);
											CS$<>8__locals2.data_length = int.Parse(text2, NumberStyles.HexNumber) * 256 + int.Parse(text3, NumberStyles.HexNumber);
										}
										else
										{
											CS$<>8__locals2.result_data.Append(text.Substring(2));
										}
										if (text.StartsWith('1'))
										{
											VwTp20Worker.<>c__DisplayClass21_2 CS$<>8__locals3 = new VwTp20Worker.<>c__DisplayClass21_2();
											int num4 = int.Parse(text[1].ToString(), NumberStyles.HexNumber);
											CS$<>8__locals3.ack = "B" + ((num4 + 1) & 15).ToString("X1") + "0";
											Task.Run(delegate
											{
												VwTp20Worker.<>c__DisplayClass21_2.<<BuildRequestForData>b__5>d <<BuildRequestForData>b__5>d;
												<<BuildRequestForData>b__5>d.<>t__builder = AsyncTaskMethodBuilder.Create();
												<<BuildRequestForData>b__5>d.<>4__this = CS$<>8__locals3;
												<<BuildRequestForData>b__5>d.<>1__state = -1;
												<<BuildRequestForData>b__5>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass21_2.<<BuildRequestForData>b__5>d>(ref <<BuildRequestForData>b__5>d);
												return <<BuildRequestForData>b__5>d.<>t__builder.Task;
											}).Wait();
										}
										else if (text.StartsWith('0'))
										{
											VwTp20Worker.<>c__DisplayClass21_3 CS$<>8__locals4 = new VwTp20Worker.<>c__DisplayClass21_3();
											int num5 = int.Parse(text[1].ToString(), NumberStyles.HexNumber);
											CS$<>8__locals4.ack = "B" + ((num5 + 1) & 15).ToString("X1");
											CS$<>8__locals4.new_data_portion = "";
											Task.Run(delegate
											{
												VwTp20Worker.<>c__DisplayClass21_3.<<BuildRequestForData>b__6>d <<BuildRequestForData>b__6>d;
												<<BuildRequestForData>b__6>d.<>t__builder = AsyncTaskMethodBuilder.Create();
												<<BuildRequestForData>b__6>d.<>4__this = CS$<>8__locals4;
												<<BuildRequestForData>b__6>d.<>1__state = -1;
												<<BuildRequestForData>b__6>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass21_3.<<BuildRequestForData>b__6>d>(ref <<BuildRequestForData>b__6>d);
												return <<BuildRequestForData>b__6>d.<>t__builder.Task;
											}).Wait();
											IEnumerable<string> enumerable2 = OBDDataReader.FilterHexAndNewLineOnly(response_data).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
											Func<string, bool> func2;
											if ((func2 = CS$<>8__locals2.<>9__7) == null)
											{
												func2 = (CS$<>8__locals2.<>9__7 = (string x) => x.StartsWith(CS$<>8__locals2.<>4__this.channelDataListenToCANId));
											}
											List<string> list2 = enumerable2.Where(func2).ToList<string>();
											if (list2 != null)
											{
												response_lines_data.AddRange(list2);
											}
										}
										if ((text.StartsWith('1') || text.StartsWith('3')) && (App.OBDReader.stopwatch.Elapsed - CS$<>8__locals2.<>4__this.lastChannelKeepAliveTime).TotalSeconds > 0.7)
										{
											Task.Run(async delegate
											{
												await App.OBDReader.SendString("A3");
												await App.OBDReader.ReadData(2500, null, -1);
											}).Wait();
											CS$<>8__locals2.<>4__this.lastChannelKeepAliveTime = App.OBDReader.stopwatch.Elapsed;
											goto IL_05BE;
										}
										goto IL_05BE;
									default:
									{
										if (c != '9')
										{
											goto IL_05BE;
										}
										if (i != response_lines_data.Count - 1)
										{
											goto IL_05BE;
										}
										Func<Task> func3;
										if ((func3 = CS$<>8__locals1.<>9__4) == null)
										{
											func3 = (CS$<>8__locals1.<>9__4 = delegate
											{
												VwTp20Worker.<>c__DisplayClass21_1.<<BuildRequestForData>b__4>d <<BuildRequestForData>b__4>d;
												<<BuildRequestForData>b__4>d.<>t__builder = AsyncTaskMethodBuilder.Create();
												<<BuildRequestForData>b__4>d.<>4__this = CS$<>8__locals1;
												<<BuildRequestForData>b__4>d.<>1__state = -1;
												<<BuildRequestForData>b__4>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass21_1.<<BuildRequestForData>b__4>d>(ref <<BuildRequestForData>b__4>d);
												return <<BuildRequestForData>b__4>d.<>t__builder.Task;
											});
										}
										Task.Run(func3).Wait();
										break;
									}
									}
								}
								else if (c != 'A')
								{
									if (c != 'B')
									{
										goto IL_05BE;
									}
									goto IL_05BE;
								}
								else
								{
									string text4 = string.Concat(new string[]
									{
										"A1",
										CS$<>8__locals2.<>4__this.channelParametersMaxBlockSize.ToString("X2"),
										CS$<>8__locals2.<>4__this.channelParametersT1.ToString("X2"),
										CS$<>8__locals2.<>4__this.channelParametersT2.ToString("X2"),
										CS$<>8__locals2.<>4__this.channelParametersT3.ToString("X2"),
										CS$<>8__locals2.<>4__this.channelParametersT4.ToString("X2"),
										"0"
									});
									taskAwaiter3 = App.OBDReader.SendString(text4).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num = (num2 = 0);
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c__DisplayClass21_0.<<BuildRequestForData>b__0>d>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_054D;
								}
							}
						}
						catch (Exception)
						{
						}
						finally
						{
							if (num < 0 && !CS$<>8__locals1.skip_finally)
							{
								CS$<>8__locals2.result_hex = CS$<>8__locals2.result_data.ToString();
								if (CS$<>8__locals2.data_length < CS$<>8__locals2.result_hex.Length / 2)
								{
									CS$<>8__locals2.result_hex = CS$<>8__locals2.result_hex.Substring(0, CS$<>8__locals2.data_length * 2);
								}
								if (CS$<>8__locals2.removeResponseMarker && CS$<>8__locals2.result_hex.StartsWith(CS$<>8__locals2.response_marker))
								{
									CS$<>8__locals2.result_hex = CS$<>8__locals2.result_hex.Substring(CS$<>8__locals2.response_marker.Length);
								}
								CS$<>8__locals2.resultContainer.Value = CS$<>8__locals2.result_hex;
								if (CS$<>8__locals2.semaphore.CurrentCount == 0)
								{
									CS$<>8__locals2.semaphore.Release();
								}
							}
						}
					}
					catch (Exception ex)
					{
						num2 = -2;
						CS$<>8__locals1 = null;
						this.<>t__builder.SetException(ex);
						return;
					}
					IL_06D2:
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetResult();
				}

				// Token: 0x0600274B RID: 10059 RVA: 0x001E1D00 File Offset: 0x001DFF00
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04001595 RID: 5525
				public int <>1__state;

				// Token: 0x04001596 RID: 5526
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x04001597 RID: 5527
				public VwTp20Worker.<>c__DisplayClass21_0 <>4__this;

				// Token: 0x04001598 RID: 5528
				public string response_data;

				// Token: 0x04001599 RID: 5529
				public OBDRequest request2;

				// Token: 0x0400159A RID: 5530
				private VwTp20Worker.<>c__DisplayClass21_1 <>8__1;

				// Token: 0x0400159B RID: 5531
				private List<string> <response_lines_data>5__2;

				// Token: 0x0400159C RID: 5532
				private int <first_line_idx>5__3;

				// Token: 0x0400159D RID: 5533
				private int <i>5__4;

				// Token: 0x0400159E RID: 5534
				private TaskAwaiter <>u__1;

				// Token: 0x0400159F RID: 5535
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x020003AB RID: 939
		[CompilerGenerated]
		private sealed class <>c__DisplayClass21_1
		{
			// Token: 0x0600274C RID: 10060 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass21_1()
			{
			}

			// Token: 0x0600274D RID: 10061 RVA: 0x001E1D10 File Offset: 0x001DFF10
			internal async Task <BuildRequestForData>b__4()
			{
				await App.OBDReader.SendString("A3");
				string text = await App.OBDReader.ReadData(2500, null, -1);
				if (text != null && text.Contains("A1"))
				{
					this.CS$<>8__locals1.<>4__this.lastChannelKeepAliveTime = App.OBDReader.stopwatch.Elapsed;
				}
				if (this.CS$<>8__locals1.request.Payload != "VWTPREPEAT")
				{
					this.CS$<>8__locals1.request.Payload = "VWTPREPEAT";
					App.OBDReader.InsertRequestInQueue(this.CS$<>8__locals1.request);
					this.skip_finally = true;
				}
			}

			// Token: 0x040015A0 RID: 5536
			public bool skip_finally;

			// Token: 0x040015A1 RID: 5537
			public VwTp20Worker.<>c__DisplayClass21_0 CS$<>8__locals1;

			// Token: 0x040015A2 RID: 5538
			public Func<Task> <>9__4;

			// Token: 0x020003AC RID: 940
			[StructLayout(LayoutKind.Auto)]
			private struct <<BuildRequestForData>b__4>d : IAsyncStateMachine
			{
				// Token: 0x0600274E RID: 10062 RVA: 0x001E1D54 File Offset: 0x001DFF54
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					VwTp20Worker.<>c__DisplayClass21_1 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<string> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter<string> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<string>);
								num2 = -1;
								goto IL_00DE;
							}
							taskAwaiter3 = App.OBDReader.SendString("A3").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c__DisplayClass21_1.<<BuildRequestForData>b__4>d>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter3.GetResult();
						taskAwaiter = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VwTp20Worker.<>c__DisplayClass21_1.<<BuildRequestForData>b__4>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_00DE:
						string result = taskAwaiter.GetResult();
						if (result != null && result.Contains("A1"))
						{
							CS$<>8__locals1.CS$<>8__locals1.<>4__this.lastChannelKeepAliveTime = App.OBDReader.stopwatch.Elapsed;
						}
						if (CS$<>8__locals1.CS$<>8__locals1.request.Payload != "VWTPREPEAT")
						{
							CS$<>8__locals1.CS$<>8__locals1.request.Payload = "VWTPREPEAT";
							App.OBDReader.InsertRequestInQueue(CS$<>8__locals1.CS$<>8__locals1.request);
							CS$<>8__locals1.skip_finally = true;
						}
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

				// Token: 0x0600274F RID: 10063 RVA: 0x001E1F10 File Offset: 0x001E0110
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040015A3 RID: 5539
				public int <>1__state;

				// Token: 0x040015A4 RID: 5540
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040015A5 RID: 5541
				public VwTp20Worker.<>c__DisplayClass21_1 <>4__this;

				// Token: 0x040015A6 RID: 5542
				private TaskAwaiter <>u__1;

				// Token: 0x040015A7 RID: 5543
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x020003AD RID: 941
		[CompilerGenerated]
		private sealed class <>c__DisplayClass21_2
		{
			// Token: 0x06002750 RID: 10064 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass21_2()
			{
			}

			// Token: 0x06002751 RID: 10065 RVA: 0x001E1F20 File Offset: 0x001E0120
			internal async Task <BuildRequestForData>b__5()
			{
				await App.OBDReader.SendString(this.ack);
				await App.OBDReader.ReadData(2500, null, -1);
			}

			// Token: 0x040015A8 RID: 5544
			public string ack;

			// Token: 0x020003AE RID: 942
			[StructLayout(LayoutKind.Auto)]
			private struct <<BuildRequestForData>b__5>d : IAsyncStateMachine
			{
				// Token: 0x06002752 RID: 10066 RVA: 0x001E1F64 File Offset: 0x001E0164
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					VwTp20Worker.<>c__DisplayClass21_2 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<string> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter<string> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<string>);
								num2 = -1;
								goto IL_00D9;
							}
							taskAwaiter3 = App.OBDReader.SendString(CS$<>8__locals1.ack).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c__DisplayClass21_2.<<BuildRequestForData>b__5>d>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter3.GetResult();
						taskAwaiter = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VwTp20Worker.<>c__DisplayClass21_2.<<BuildRequestForData>b__5>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_00D9:
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

				// Token: 0x06002753 RID: 10067 RVA: 0x001E2090 File Offset: 0x001E0290
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040015A9 RID: 5545
				public int <>1__state;

				// Token: 0x040015AA RID: 5546
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040015AB RID: 5547
				public VwTp20Worker.<>c__DisplayClass21_2 <>4__this;

				// Token: 0x040015AC RID: 5548
				private TaskAwaiter <>u__1;

				// Token: 0x040015AD RID: 5549
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x020003AF RID: 943
		[CompilerGenerated]
		private sealed class <>c__DisplayClass21_3
		{
			// Token: 0x06002754 RID: 10068 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass21_3()
			{
			}

			// Token: 0x06002755 RID: 10069 RVA: 0x001E20A0 File Offset: 0x001E02A0
			internal async Task <BuildRequestForData>b__6()
			{
				await App.OBDReader.SendString(this.ack);
				this.new_data_portion = await App.OBDReader.ReadData(2500, null, -1);
			}

			// Token: 0x040015AE RID: 5550
			public string ack;

			// Token: 0x040015AF RID: 5551
			public string new_data_portion;

			// Token: 0x020003B0 RID: 944
			[StructLayout(LayoutKind.Auto)]
			private struct <<BuildRequestForData>b__6>d : IAsyncStateMachine
			{
				// Token: 0x06002756 RID: 10070 RVA: 0x001E20E4 File Offset: 0x001E02E4
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					VwTp20Worker.<>c__DisplayClass21_3 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<string> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter<string> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<string>);
								num2 = -1;
								goto IL_00DC;
							}
							taskAwaiter3 = App.OBDReader.SendString(CS$<>8__locals1.ack).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<>c__DisplayClass21_3.<<BuildRequestForData>b__6>d>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter3.GetResult();
						taskAwaiter = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VwTp20Worker.<>c__DisplayClass21_3.<<BuildRequestForData>b__6>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_00DC:
						string result = taskAwaiter.GetResult();
						CS$<>8__locals1.new_data_portion = result;
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

				// Token: 0x06002757 RID: 10071 RVA: 0x001E221C File Offset: 0x001E041C
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040015B0 RID: 5552
				public int <>1__state;

				// Token: 0x040015B1 RID: 5553
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040015B2 RID: 5554
				public VwTp20Worker.<>c__DisplayClass21_3 <>4__this;

				// Token: 0x040015B3 RID: 5555
				private TaskAwaiter <>u__1;

				// Token: 0x040015B4 RID: 5556
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x020003B1 RID: 945
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CloseChannel>d__17 : IAsyncStateMachine
		{
			// Token: 0x06002758 RID: 10072 RVA: 0x001E222C File Offset: 0x001E042C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VwTp20Worker vwTp20Worker = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!vwTp20Worker.IsChannelAlive())
						{
							goto IL_0185;
						}
						VwTp20Worker.<>c__DisplayClass17_0 CS$<>8__locals1 = new VwTp20Worker.<>c__DisplayClass17_0();
						"ATCRA" + vwTp20Worker.channelDataListenToCANId;
						string[] array = new string[] { vwTp20Worker.elmProtocol };
						string text = "ATSP" + App.OBDReader.CurrentProtocolNumber.ToString("X1");
						string[] array2 = new string[] { text, "ATSTDEF", "ATAR" };
						vwTp20Worker.sendCounter.ToString("X1");
						OBDRequest obdrequest = new OBDRequest("A80", vwTp20Worker.channelDataSendToCANId, array, array2, false, null)
						{
							ELMFormat = ELMFormat.VwTp20,
							DoNotDecode = true
						};
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						obdrequest.ResponseReceived += delegate(OBDRequest request2, string data)
						{
							CS$<>8__locals1.semaphore.Release();
						};
						if (replaceQueue)
						{
							App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						}
						else
						{
							App.OBDReader.AddRequestToQueue(obdrequest);
						}
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<CloseChannel>d__17>(ref taskAwaiter, ref this);
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
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0185:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002759 RID: 10073 RVA: 0x001E23F0 File Offset: 0x001E05F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040015B5 RID: 5557
			public int <>1__state;

			// Token: 0x040015B6 RID: 5558
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040015B7 RID: 5559
			public VwTp20Worker <>4__this;

			// Token: 0x040015B8 RID: 5560
			public bool replaceQueue;

			// Token: 0x040015B9 RID: 5561
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020003B2 RID: 946
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OpenChannelUsingLoopAsync>d__15 : IAsyncStateMachine
		{
			// Token: 0x0600275A RID: 10074 RVA: 0x001E2400 File Offset: 0x001E0600
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VwTp20Worker vwTp20Worker = this;
				ValueTuple<bool, string> valueTuple;
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
							goto IL_0334;
						}
						CS$<>8__locals1 = new VwTp20Worker.<>c__DisplayClass15_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.dataToSendAfterOpening = dataToSendAfterOpening;
						CS$<>8__locals1.removeResponseMarker = removeResponseMarker;
						CS$<>8__locals1.replaceQueue = replaceQueue;
						CS$<>8__locals1.delay_for_D7 = 5000;
						string text = vwTp20Worker.channelSetupAddress + "C000100003011";
						CS$<>8__locals1.recieve_address = (vwTp20Worker.i_channelSetupAddress + 512).ToString("X3");
						string text2 = "ATCRA" + CS$<>8__locals1.recieve_address;
						string[] array = new string[] { vwTp20Worker.elmprotocolParams, vwTp20Worker.elmProtocol, vwTp20Worker.openChannelATST, text2 };
						string text3 = "ATSP" + App.OBDReader.CurrentProtocolNumber.ToString("X1");
						string[] array2 = new string[] { text3, "ATSTDEF", "ATAR" };
						CS$<>8__locals1.open_channel_request = new OBDRequest(text, "200", array, array2, false, null)
						{
							DoNotDecode = true,
							ELMFormat = ELMFormat.VwTp20
						};
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.open_result = false;
						CS$<>8__locals1.resultContainer = new DataContainer<string>("");
						string text4 = "A00F8AFF32FF";
						CS$<>8__locals1.dataSemaphore = null;
						CS$<>8__locals1.get_channel_parameters_request = new OBDRequest(text4, "", new string[] { "ATST18" }, array2, false, null)
						{
							DoNotDecode = true,
							ELMFormat = ELMFormat.VwTp20
						};
						CS$<>8__locals1.get_channel_parameters_request.ResponseReceived += delegate(OBDRequest get_channel_parameters_request2, string channel_parameters_data)
						{
							try
							{
								if (channel_parameters_data == null || channel_parameters_data.Contains("NO DATA") || channel_parameters_data.Contains("ERROR") || channel_parameters_data.Contains("RESET") || channel_parameters_data == "ELMFAILTP20")
								{
									CS$<>8__locals1.semaphore.Release();
								}
								else
								{
									string[] array3 = OBDDataReader.FilterHexAndNewLineOnly(channel_parameters_data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
									if (array3.Length == 0)
									{
										CS$<>8__locals1.semaphore.Release();
									}
									else
									{
										string text6 = array3.FirstOrDefault((string x) => x.IndexOf("A1") == 3);
										if (text6 == null)
										{
											CS$<>8__locals1.semaphore.Release();
										}
										else
										{
											byte[] array4 = BitHelpers.ConvertHexToBytesX(text6.Substring(3));
											CS$<>8__locals1.<>4__this.channelParametersMaxBlockSize = (int)array4[1];
											CS$<>8__locals1.<>4__this.channelParametersT1 = (int)array4[2];
											CS$<>8__locals1.<>4__this.channelParametersT2 = (int)array4[3];
											CS$<>8__locals1.<>4__this.channelParametersT3 = (int)array4[4];
											CS$<>8__locals1.<>4__this.channelParametersT4 = (int)array4[5];
											CS$<>8__locals1.<>4__this.sendCounter = 0;
											CS$<>8__locals1.<>4__this.lastChannelKeepAliveTime = App.OBDReader.stopwatch.Elapsed;
											App.OBDReader.ClearPendingAfterCommands();
											CS$<>8__locals1.open_result = true;
											if (!string.IsNullOrEmpty(CS$<>8__locals1.dataToSendAfterOpening))
											{
												ValueTuple<OBDRequest, SemaphoreSlim, DataContainer<string>> valueTuple2 = CS$<>8__locals1.<>4__this.BuildRequestForData(CS$<>8__locals1.dataToSendAfterOpening, CS$<>8__locals1.removeResponseMarker);
												OBDRequest item = valueTuple2.Item1;
												CS$<>8__locals1.dataSemaphore = valueTuple2.Item2;
												CS$<>8__locals1.resultContainer = valueTuple2.Item3;
												int num3 = VWTPECU.ConvertCanTimingToMsec((byte)CS$<>8__locals1.<>4__this.channelParametersT3) / 4 + 6;
												if (SharedSettings.Current.ShowExperimental)
												{
													num3 += num3 / 2;
												}
												string text7 = "ATST" + num3.ToString("X2");
												item.BeforeCommands = new string[] { text7 };
												if (CS$<>8__locals1.replaceQueue)
												{
													App.OBDReader.ClearPendingAfterCommands();
													App.OBDReader.ReplaceQueue(new OBDRequest[] { item });
												}
												else
												{
													App.OBDReader.AddRequestToQueue(item);
												}
											}
											CS$<>8__locals1.semaphore.Release();
										}
									}
								}
							}
							catch (Exception)
							{
							}
							finally
							{
								if (CS$<>8__locals1.semaphore.CurrentCount == 0)
								{
									CS$<>8__locals1.semaphore.Release();
								}
							}
						};
						CS$<>8__locals1.open_channel_request.ResponseReceived += delegate(OBDRequest open_channel_request2, string data)
						{
							try
							{
								if (data == null || data.Contains("NO DATA") || data.Contains("ERROR") || data.Contains("RESET") || data == "ELMFAILTP20")
								{
									OBDDataReader obdreader = App.OBDReader;
									Predicate<OBDRequest> predicate;
									if ((predicate = CS$<>8__locals1.<>9__3) == null)
									{
										predicate = (CS$<>8__locals1.<>9__3 = (OBDRequest x) => x == CS$<>8__locals1.get_channel_parameters_request);
									}
									obdreader.RemoveFromQueue(predicate);
									CS$<>8__locals1.semaphore.Release();
								}
								else
								{
									string[] array5 = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
									if (array5.Length == 0)
									{
										OBDDataReader obdreader2 = App.OBDReader;
										Predicate<OBDRequest> predicate2;
										if ((predicate2 = CS$<>8__locals1.<>9__4) == null)
										{
											predicate2 = (CS$<>8__locals1.<>9__4 = (OBDRequest x) => x == CS$<>8__locals1.get_channel_parameters_request);
										}
										obdreader2.RemoveFromQueue(predicate2);
										CS$<>8__locals1.semaphore.Release();
									}
									else
									{
										IEnumerable<string> enumerable = array5;
										Func<string, bool> func;
										if ((func = CS$<>8__locals1.<>9__5) == null)
										{
											func = (CS$<>8__locals1.<>9__5 = (string x) => x.Length == 17 && x.StartsWith(CS$<>8__locals1.recieve_address));
										}
										string text8 = enumerable.Where(func).LastOrDefault<string>();
										if (text8 == null)
										{
											OBDDataReader obdreader3 = App.OBDReader;
											Predicate<OBDRequest> predicate3;
											if ((predicate3 = CS$<>8__locals1.<>9__6) == null)
											{
												predicate3 = (CS$<>8__locals1.<>9__6 = (OBDRequest x) => x == CS$<>8__locals1.get_channel_parameters_request);
											}
											obdreader3.RemoveFromQueue(predicate3);
											CS$<>8__locals1.semaphore.Release();
										}
										else
										{
											string text9 = text8.Substring(3);
											string text10 = text9.Substring(2, 2);
											if (text10 == "D7" && CS$<>8__locals1.delay_for_D7 < 30000)
											{
												App.OBDReader.DebugWriteSync(string.Format("\r\n[{0}]\r\n", App.OBDReader.stopwatch.Elapsed.TotalSeconds));
												Func<Task> func2;
												if ((func2 = CS$<>8__locals1.<>9__7) == null)
												{
													func2 = (CS$<>8__locals1.<>9__7 = delegate
													{
														VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d <<OpenChannelUsingLoopAsync>b__7>d;
														<<OpenChannelUsingLoopAsync>b__7>d.<>t__builder = AsyncTaskMethodBuilder.Create();
														<<OpenChannelUsingLoopAsync>b__7>d.<>4__this = CS$<>8__locals1;
														<<OpenChannelUsingLoopAsync>b__7>d.<>1__state = -1;
														<<OpenChannelUsingLoopAsync>b__7>d.<>t__builder.Start<VwTp20Worker.<>c__DisplayClass15_0.<<OpenChannelUsingLoopAsync>b__7>d>(ref <<OpenChannelUsingLoopAsync>b__7>d);
														return <<OpenChannelUsingLoopAsync>b__7>d.<>t__builder.Task;
													});
												}
												Task.Run(func2).Wait();
											}
											else if (text10 == "D0")
											{
												string text11 = text9[7].ToString() + text9[5].ToString() + text9[6].ToString();
												string text12 = text9[11].ToString() + text9[8].ToString() + text9[9].ToString();
												CS$<>8__locals1.<>4__this.channelDataListenToCANId = text11;
												CS$<>8__locals1.<>4__this.channelDataSendToCANId = text12;
												string text13 = "ATCRA" + CS$<>8__locals1.<>4__this.channelDataListenToCANId;
												string[] array6 = new string[] { "ATST32", text13 };
												CS$<>8__locals1.get_channel_parameters_request.BeforeCommands = array6;
												CS$<>8__locals1.get_channel_parameters_request.Header = text12;
												App.OBDReader.ClearPendingAfterCommands();
											}
											else
											{
												OBDDataReader obdreader4 = App.OBDReader;
												Predicate<OBDRequest> predicate4;
												if ((predicate4 = CS$<>8__locals1.<>9__8) == null)
												{
													predicate4 = (CS$<>8__locals1.<>9__8 = (OBDRequest x) => x == CS$<>8__locals1.get_channel_parameters_request);
												}
												obdreader4.RemoveFromQueue(predicate4);
												CS$<>8__locals1.semaphore.Release();
											}
										}
									}
								}
							}
							catch (Exception)
							{
							}
						};
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals1.open_channel_request, CS$<>8__locals1.get_channel_parameters_request });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<OpenChannelUsingLoopAsync>d__15>(ref taskAwaiter, ref this);
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
					string text5 = "";
					if (CS$<>8__locals1.dataSemaphore == null)
					{
						goto IL_034D;
					}
					taskAwaiter = CS$<>8__locals1.dataSemaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<OpenChannelUsingLoopAsync>d__15>(ref taskAwaiter, ref this);
						return;
					}
					IL_0334:
					taskAwaiter.GetResult();
					text5 = CS$<>8__locals1.resultContainer.Value;
					IL_034D:
					valueTuple = new ValueTuple<bool, string>(CS$<>8__locals1.open_result, text5);
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
				this.<>t__builder.SetResult(valueTuple);
			}

			// Token: 0x0600275B RID: 10075 RVA: 0x001E27C8 File Offset: 0x001E09C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040015BA RID: 5562
			public int <>1__state;

			// Token: 0x040015BB RID: 5563
			public AsyncTaskMethodBuilder<ValueTuple<bool, string>> <>t__builder;

			// Token: 0x040015BC RID: 5564
			public VwTp20Worker <>4__this;

			// Token: 0x040015BD RID: 5565
			public string dataToSendAfterOpening;

			// Token: 0x040015BE RID: 5566
			public bool removeResponseMarker;

			// Token: 0x040015BF RID: 5567
			public bool replaceQueue;

			// Token: 0x040015C0 RID: 5568
			private VwTp20Worker.<>c__DisplayClass15_0 <>8__1;

			// Token: 0x040015C1 RID: 5569
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020003B3 RID: 947
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendAndReadDataUsingRequestLoop>d__18 : IAsyncStateMachine
		{
			// Token: 0x0600275C RID: 10076 RVA: 0x001E27D8 File Offset: 0x001E09D8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VwTp20Worker vwTp20Worker = this;
				ValueTuple<bool, string> valueTuple2;
				try
				{
					TaskAwaiter<ValueTuple<bool, string>> taskAwaiter;
					TaskAwaiter<ValueTuple<bool, string>> taskAwaiter2;
					if (num != 0)
					{
						TaskAwaiter taskAwaiter3;
						if (num != 1)
						{
							if (!vwTp20Worker.IsChannelAlive())
							{
								taskAwaiter = vwTp20Worker.OpenChannelUsingLoopAsync(data, replaceQueue, removeResponseMarker).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string>>, VwTp20Worker.<SendAndReadDataUsingRequestLoop>d__18>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_008C;
							}
							else
							{
								ValueTuple<OBDRequest, SemaphoreSlim, DataContainer<string>> valueTuple = vwTp20Worker.BuildRequestForData(data, removeResponseMarker);
								OBDRequest item = valueTuple.Item1;
								SemaphoreSlim item2 = valueTuple.Item2;
								resultContainer = valueTuple.Item3;
								item.BeforeCommands = new string[0];
								if (replaceQueue)
								{
									App.OBDReader.ReplaceQueue(new OBDRequest[] { item });
								}
								else
								{
									App.OBDReader.AddRequestToQueue(item);
								}
								taskAwaiter3 = item2.WaitAsync().GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VwTp20Worker.<SendAndReadDataUsingRequestLoop>d__18>(ref taskAwaiter3, ref this);
									return;
								}
							}
						}
						else
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter3.GetResult();
						valueTuple2 = new ValueTuple<bool, string>(true, resultContainer.Value);
						goto IL_01B7;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
					num2 = -1;
					IL_008C:
					ValueTuple<bool, string> result = taskAwaiter.GetResult();
					bool item3 = result.Item1;
					string item4 = result.Item2;
					if (item3)
					{
						valueTuple2 = new ValueTuple<bool, string>(true, item4);
					}
					else
					{
						valueTuple2 = new ValueTuple<bool, string>(item3, "");
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01B7:
				num2 = -2;
				this.<>t__builder.SetResult(valueTuple2);
			}

			// Token: 0x0600275D RID: 10077 RVA: 0x001E29CC File Offset: 0x001E0BCC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040015C2 RID: 5570
			public int <>1__state;

			// Token: 0x040015C3 RID: 5571
			public AsyncTaskMethodBuilder<ValueTuple<bool, string>> <>t__builder;

			// Token: 0x040015C4 RID: 5572
			public VwTp20Worker <>4__this;

			// Token: 0x040015C5 RID: 5573
			public string data;

			// Token: 0x040015C6 RID: 5574
			public bool replaceQueue;

			// Token: 0x040015C7 RID: 5575
			public bool removeResponseMarker;

			// Token: 0x040015C8 RID: 5576
			private TaskAwaiter<ValueTuple<bool, string>> <>u__1;

			// Token: 0x040015C9 RID: 5577
			private DataContainer<string> <resultContainer>5__2;

			// Token: 0x040015CA RID: 5578
			private TaskAwaiter <>u__2;
		}
	}
}
