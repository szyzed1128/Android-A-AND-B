using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B1A RID: 2842
	internal class MQBSimpleOperationWith0102StatusCheck : MQBServiceProcedure
	{
		// Token: 0x0600585F RID: 22623 RVA: 0x00422910 File Offset: 0x00420B10
		public MQBSimpleOperationWith0102StatusCheck(string Name, string Description, string InnerDectription, string Unit, string startCommand, string stopCommand, string Password, bool PasswordVisible)
		{
			base.Name = Name;
			base.Description = Description;
			base.InnerDescription = base.InnerDescription;
			this.Unit = Unit;
			this.startOption = new MQBAdaptationOption("Start", startCommand, new TranslationItem[]
			{
				new TranslationItem("ru", "Запуск", "", "")
			});
			this.cancelOption = new MQBAdaptationOption("Cancel", stopCommand, new TranslationItem[]
			{
				new TranslationItem("ru", "Прервать", "", "")
			});
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
			this.password = Password;
		}

		// Token: 0x06005860 RID: 22624 RVA: 0x004229F0 File Offset: 0x00420BF0
		protected override async Task<CodingRequestResult> OptionExecute(string optionValue, IProgress<string> progress)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			ResponseReceivedDelegate checkReceivedResponseForNegativeResult = delegate(OBDRequest request, string data)
			{
				if (data == null)
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
					return;
				}
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
				{
					int num = data.IndexOf("7F2E");
					int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
					if (num2 >= 128 || num2 == 34)
					{
						codingResult = CodingRequestResult.WrongConditions;
					}
					else if (num2 == 51)
					{
						codingResult = CodingRequestResult.WrongAccessKey;
					}
					else if (num2 == 49)
					{
						codingResult = CodingRequestResult.NotSupported;
					}
					else
					{
						codingResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
				}
			};
			ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
			{
				OBDRequest obdrequest5 = new OBDRequest(this.cancelOption.Value, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				if (data == null)
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
					semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
					semaphore.Release();
					return;
				}
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
				{
					int num3 = data.IndexOf("7F2E");
					int num4 = int.Parse(data.Substring(num3 + 4, 2), NumberStyles.HexNumber);
					if (num4 >= 128 || num4 == 34)
					{
						codingResult = CodingRequestResult.WrongConditions;
					}
					else if (num4 == 51)
					{
						codingResult = CodingRequestResult.WrongAccessKey;
					}
					else if (num4 == 49)
					{
						codingResult = CodingRequestResult.NotSupported;
					}
					else
					{
						codingResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
					semaphore.Release();
				}
			};
			OBDRequest obdrequest = null;
			OBDRequest req_sendKey = null;
			if (optionValue == this.startOption.Value)
			{
				uint uint_pass = 0U;
				if (!string.IsNullOrEmpty(this.password) && uint.TryParse(this.password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out uint_pass))
				{
					req_sendKey = new OBDRequest("2704", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
					obdrequest = new OBDRequest("2703", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
					obdrequest.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
					{
						try
						{
							if (BitConverter.IsLittleEndian)
							{
								Array.Reverse<byte>(getSeedData);
							}
							string text = (BitConverter.ToUInt32(getSeedData, 0) + uint_pass).ToString("X8");
							req_sendKey.Command = "2704" + text;
						}
						catch (Exception)
						{
						}
					};
					req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if (data != null)
						{
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
						}
						if (data == null || !data.Contains("6704"))
						{
							codingResult = CodingRequestResult.WrongAccessKey;
							App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						}
					};
				}
				OBDRequest obdrequest2 = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest3 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				obdrequest2.ResponseReceived += responseReceivedDelegate;
				obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
				bool status0102_finished = false;
				obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
					progress.Report(this.status_string_0102);
					if (data[0] == 16)
					{
						status0102_finished = true;
						progress.Report(Translate.GetString("coding_OperationFinished"));
						codingResult = CodingRequestResult.Success;
						semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					}
				};
				OBDRequest[] array;
				if (obdrequest != null && req_sendKey != null)
				{
					array = new OBDRequest[] { obdrequest, req_sendKey, obdrequest2, obdrequest3 };
				}
				else
				{
					array = new OBDRequest[] { obdrequest2, obdrequest3 };
				}
				App.OBDReader.ReplaceQueue(array);
				await semaphore.WaitAsync();
			}
			if (optionValue == this.cancelOption.Value)
			{
				OBDRequest obdrequest4 = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest4.ResponseReceived += checkReceivedResponseForNegativeResult;
				obdrequest4.ResponseReceived += delegate(OBDRequest request, string data)
				{
					progress.Report(Translate.GetString("coding_OperationFinished"));
					codingResult = CodingRequestResult.Success;
					semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
				await semaphore.WaitAsync();
			}
			return codingResult;
		}

		// Token: 0x040036EB RID: 14059
		protected MQBAdaptationOption startOption;

		// Token: 0x040036EC RID: 14060
		protected new MQBAdaptationOption cancelOption;

		// Token: 0x040036ED RID: 14061
		private string status_string_0102 = "";

		// Token: 0x040036EE RID: 14062
		private string password = "";

		// Token: 0x02000B1B RID: 2843
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x06005861 RID: 22625 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x06005862 RID: 22626 RVA: 0x00422A44 File Offset: 0x00420C44
			internal void <OptionExecute>b__0(OBDRequest request, string data)
			{
				if (data == null)
				{
					this.codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					this.codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
					return;
				}
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
				{
					int num = data.IndexOf("7F2E");
					int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
					if (num2 >= 128 || num2 == 34)
					{
						this.codingResult = CodingRequestResult.WrongConditions;
					}
					else if (num2 == 51)
					{
						this.codingResult = CodingRequestResult.WrongAccessKey;
					}
					else if (num2 == 49)
					{
						this.codingResult = CodingRequestResult.NotSupported;
					}
					else
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
				}
			}

			// Token: 0x06005863 RID: 22627 RVA: 0x00422B7C File Offset: 0x00420D7C
			internal void <OptionExecute>b__1(OBDRequest request, string data)
			{
				OBDRequest obdrequest = new OBDRequest(this.<>4__this.cancelOption.Value, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false);
				if (data == null)
				{
					this.codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
					this.semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					this.codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
					this.semaphore.Release();
					return;
				}
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
				{
					int num = data.IndexOf("7F2E");
					int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
					if (num2 >= 128 || num2 == 34)
					{
						this.codingResult = CodingRequestResult.WrongConditions;
					}
					else if (num2 == 51)
					{
						this.codingResult = CodingRequestResult.WrongAccessKey;
					}
					else if (num2 == 49)
					{
						this.codingResult = CodingRequestResult.NotSupported;
					}
					else
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
					this.semaphore.Release();
				}
			}

			// Token: 0x06005864 RID: 22628 RVA: 0x00422CF7 File Offset: 0x00420EF7
			internal void <OptionExecute>b__3(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data == null || !data.Contains("6704"))
				{
					this.codingResult = CodingRequestResult.WrongAccessKey;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			}

			// Token: 0x06005865 RID: 22629 RVA: 0x00422D2B File Offset: 0x00420F2B
			internal void <OptionExecute>b__5(OBDRequest request, string data)
			{
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x040036EF RID: 14063
			public CodingRequestResult codingResult;

			// Token: 0x040036F0 RID: 14064
			public SemaphoreSlim semaphore;

			// Token: 0x040036F1 RID: 14065
			public MQBSimpleOperationWith0102StatusCheck <>4__this;

			// Token: 0x040036F2 RID: 14066
			public OBDRequest req_sendKey;

			// Token: 0x040036F3 RID: 14067
			public IProgress<string> progress;
		}

		// Token: 0x02000B1C RID: 2844
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_1
		{
			// Token: 0x06005866 RID: 22630 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_1()
			{
			}

			// Token: 0x06005867 RID: 22631 RVA: 0x00422D60 File Offset: 0x00420F60
			internal void <OptionExecute>b__2(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(getSeedData);
					}
					string text = (BitConverter.ToUInt32(getSeedData, 0) + this.uint_pass).ToString("X8");
					this.CS$<>8__locals1.req_sendKey.Command = "2704" + text;
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x06005868 RID: 22632 RVA: 0x00422DC8 File Offset: 0x00420FC8
			internal void <OptionExecute>b__4(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
					this.CS$<>8__locals1.semaphore.Release();
					return;
				}
				this.CS$<>8__locals1.<>4__this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
				this.CS$<>8__locals1.progress.Report(this.CS$<>8__locals1.<>4__this.status_string_0102);
				if (data[0] == 16)
				{
					this.status0102_finished = true;
					this.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
					this.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
					this.CS$<>8__locals1.semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				}
			}

			// Token: 0x040036F4 RID: 14068
			public uint uint_pass;

			// Token: 0x040036F5 RID: 14069
			public bool status0102_finished;

			// Token: 0x040036F6 RID: 14070
			public MQBSimpleOperationWith0102StatusCheck.<>c__DisplayClass5_0 CS$<>8__locals1;
		}

		// Token: 0x02000B1D RID: 2845
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__5 : IAsyncStateMachine
		{
			// Token: 0x06005869 RID: 22633 RVA: 0x00422E84 File Offset: 0x00421084
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck = this;
				CodingRequestResult codingResult;
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
							goto IL_038F;
						}
						CS$<>8__locals1 = new MQBSimpleOperationWith0102StatusCheck.<>c__DisplayClass5_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						checkReceivedResponseForNegativeResult = delegate(OBDRequest request, string data)
						{
							if (data == null)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							if (data.Contains("NO DATA"))
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
							{
								int num3 = data.IndexOf("7F2E");
								int num4 = int.Parse(data.Substring(num3 + 4, 2), NumberStyles.HexNumber);
								if (num4 >= 128 || num4 == 34)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
								}
								else if (num4 == 51)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
								}
								else if (num4 == 49)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
								}
								else
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								}
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
							}
						};
						ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
						{
							OBDRequest obdrequest5 = new OBDRequest(CS$<>8__locals1.<>4__this.cancelOption.Value, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false);
							if (data == null)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							if (data.Contains("NO DATA"))
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (data.Length >= 11 && data.Contains("037F" + request.Command.Substring(0, 2)) && !data.Contains("037F" + request.Command.Substring(0, 2) + "78"))
							{
								int num5 = data.IndexOf("7F2E");
								int num6 = int.Parse(data.Substring(num5 + 4, 2), NumberStyles.HexNumber);
								if (num6 >= 128 || num6 == 34)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
								}
								else if (num6 == 51)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
								}
								else if (num6 == 49)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
								}
								else
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								}
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest5 });
								CS$<>8__locals1.semaphore.Release();
							}
						};
						OBDRequest obdrequest = null;
						CS$<>8__locals1.req_sendKey = null;
						if (!(optionValue == mqbsimpleOperationWith0102StatusCheck.startOption.Value))
						{
							goto IL_02BC;
						}
						MQBSimpleOperationWith0102StatusCheck.<>c__DisplayClass5_1 CS$<>8__locals2 = new MQBSimpleOperationWith0102StatusCheck.<>c__DisplayClass5_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.uint_pass = 0U;
						if (!string.IsNullOrEmpty(mqbsimpleOperationWith0102StatusCheck.password) && uint.TryParse(mqbsimpleOperationWith0102StatusCheck.password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out CS$<>8__locals2.uint_pass))
						{
							CS$<>8__locals2.CS$<>8__locals1.req_sendKey = new OBDRequest("2704", mqbsimpleOperationWith0102StatusCheck.RequestHeader, mqbsimpleOperationWith0102StatusCheck.BeforeCommands, mqbsimpleOperationWith0102StatusCheck.AfterCommands, false);
							obdrequest = new OBDRequest("2703", mqbsimpleOperationWith0102StatusCheck.RequestHeader, mqbsimpleOperationWith0102StatusCheck.BeforeCommands, mqbsimpleOperationWith0102StatusCheck.AfterCommands, false);
							obdrequest.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
							{
								try
								{
									if (BitConverter.IsLittleEndian)
									{
										Array.Reverse<byte>(getSeedData);
									}
									string text = (BitConverter.ToUInt32(getSeedData, 0) + CS$<>8__locals2.uint_pass).ToString("X8");
									CS$<>8__locals2.CS$<>8__locals1.req_sendKey.Command = "2704" + text;
								}
								catch (Exception)
								{
								}
							};
							CS$<>8__locals2.CS$<>8__locals1.req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
							{
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
								}
								if (data == null || !data.Contains("6704"))
								{
									CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
									App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								}
							};
						}
						OBDRequest obdrequest2 = new OBDRequest(optionValue, mqbsimpleOperationWith0102StatusCheck.RequestHeader, mqbsimpleOperationWith0102StatusCheck.BeforeCommands, mqbsimpleOperationWith0102StatusCheck.AfterCommands, false);
						OBDRequest obdrequest3 = new OBDRequest("220102", mqbsimpleOperationWith0102StatusCheck.RequestHeader, mqbsimpleOperationWith0102StatusCheck.BeforeCommands, mqbsimpleOperationWith0102StatusCheck.AfterCommands, true);
						obdrequest2.ResponseReceived += responseReceivedDelegate;
						obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
						CS$<>8__locals2.status0102_finished = false;
						obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								return;
							}
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
							CS$<>8__locals2.CS$<>8__locals1.progress.Report(CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0102);
							if (data[0] == 16)
							{
								CS$<>8__locals2.status0102_finished = true;
								CS$<>8__locals2.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								App.OBDReader.ClearRequestQueue();
							}
						};
						OBDRequest[] array;
						if (obdrequest != null && CS$<>8__locals2.CS$<>8__locals1.req_sendKey != null)
						{
							array = new OBDRequest[]
							{
								obdrequest,
								CS$<>8__locals2.CS$<>8__locals1.req_sendKey,
								obdrequest2,
								obdrequest3
							};
						}
						else
						{
							array = new OBDRequest[] { obdrequest2, obdrequest3 };
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBSimpleOperationWith0102StatusCheck.<OptionExecute>d__5>(ref taskAwaiter, ref this);
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
					IL_02BC:
					if (!(optionValue == mqbsimpleOperationWith0102StatusCheck.cancelOption.Value))
					{
						goto IL_0396;
					}
					OBDRequest obdrequest4 = new OBDRequest(optionValue, mqbsimpleOperationWith0102StatusCheck.RequestHeader, mqbsimpleOperationWith0102StatusCheck.BeforeCommands, mqbsimpleOperationWith0102StatusCheck.AfterCommands, false);
					obdrequest4.ResponseReceived += checkReceivedResponseForNegativeResult;
					obdrequest4.ResponseReceived += delegate(OBDRequest request, string data)
					{
						CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
						CS$<>8__locals1.codingResult = CodingRequestResult.Success;
						CS$<>8__locals1.semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					};
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4 });
					taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBSimpleOperationWith0102StatusCheck.<OptionExecute>d__5>(ref taskAwaiter, ref this);
						return;
					}
					IL_038F:
					taskAwaiter.GetResult();
					IL_0396:
					codingResult = CS$<>8__locals1.codingResult;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					checkReceivedResponseForNegativeResult = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				checkReceivedResponseForNegativeResult = null;
				this.<>t__builder.SetResult(codingResult);
			}

			// Token: 0x0600586A RID: 22634 RVA: 0x0042329C File Offset: 0x0042149C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040036F7 RID: 14071
			public int <>1__state;

			// Token: 0x040036F8 RID: 14072
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040036F9 RID: 14073
			public MQBSimpleOperationWith0102StatusCheck <>4__this;

			// Token: 0x040036FA RID: 14074
			public IProgress<string> progress;

			// Token: 0x040036FB RID: 14075
			public string optionValue;

			// Token: 0x040036FC RID: 14076
			private MQBSimpleOperationWith0102StatusCheck.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x040036FD RID: 14077
			private ResponseReceivedDelegate <checkReceivedResponseForNegativeResult>5__2;

			// Token: 0x040036FE RID: 14078
			private TaskAwaiter <>u__1;
		}
	}
}
