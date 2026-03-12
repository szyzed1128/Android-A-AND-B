using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AAD RID: 2733
	internal class EngineAutomaticTestsProcedure : MQBServiceProcedure
	{
		// Token: 0x06005612 RID: 22034 RVA: 0x004107A8 File Offset: 0x0040E9A8
		public EngineAutomaticTestsProcedure()
		{
			base.Name = Translate.GetString("coding_mqb_engine_automatic_tests_Title");
			base.Description = Translate.GetString("coding_mqb_engine_automatic_tests_Description");
			base.InnerDescription = Translate.GetString("coding_mqb_engine_automatic_tests_InnerDescription");
			this.Unit = "01";
			this.startOption = new MQBAdaptationOption(Translate.GetString("codingDB_opt_Start"), "31010311040000");
			this.cancelOption = new MQBAdaptationOption(Translate.GetString("btnCancel.Content"), "31020311");
			base.Options.Add(this.startOption);
			base.Options.Add(this.cancelOption);
			this.status_string_0102 = "";
			this.status_string_0104 = "";
			this.status_string_0106 = "";
			this.Password = "27971";
			base.PasswordVisible = true;
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x00410880 File Offset: 0x0040EA80
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
				OBDRequest obdrequest9 = new OBDRequest(this.cancelOption.Value, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				if (data == null)
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest9 });
					semaphore.Release();
					return;
				}
				if (data.Contains("NO DATA"))
				{
					codingResult = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest9 });
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
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest9 });
					semaphore.Release();
				}
			};
			if (optionValue == this.startOption.Value)
			{
				OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest2 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				OBDRequest obdrequest3 = new OBDRequest("220104", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				OBDRequest obdrequest4 = new OBDRequest("220106", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				obdrequest.ResponseReceived += responseReceivedDelegate;
				obdrequest2.ResponseReceived += checkReceivedResponseForNegativeResult;
				obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
				bool status0102_finished = false;
				obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
					progress.Report(string.Concat(new string[] { this.status_string_0102, "\n", this.status_string_0104, "\nTest steps to be perfomed: ", this.status_string_0106 }));
					if (data[0] == 16)
					{
						status0102_finished = true;
						progress.Report(Translate.GetString("coding_OperationFinished"));
						codingResult = CodingRequestResult.Success;
						semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					}
				};
				obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					this.status_string_0104 = this.StatusFrom0104ToString(data);
					progress.Report(string.Concat(new string[] { this.status_string_0102, "\n", this.status_string_0104, "\nTest steps to be perfomed: ", this.status_string_0106 }));
				};
				obdrequest4.ResponseDecoded += delegate(OBDRequest check_result_request_0106_2, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data != null && data.Length != 0)
					{
						this.status_string_0106 = data[0].ToString();
					}
				};
				OBDRequest req_setDate = new OBDRequest(string.Format("2EF199{0}{1}{2}", DateTimeNowHelper.NowSafe.Year - 2000, DateTimeNowHelper.NowSafe.Month.ToString("00"), DateTimeNowHelper.NowSafe.Day.ToString("00")), this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				req_setDate.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if (data != null && !data.Contains("6EF199"))
					{
						App.OBDReader.DebugWrite("\n2EF199_error\n");
					}
				};
				OBDRequest obdrequest5 = new OBDRequest("22F199", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
				{
					ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
				};
				obdrequest5.ResponseDecoded += delegate(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
				{
					if (getDateData != null && getDateData.Length >= 3)
					{
						string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
						req_setDate.Command = text;
					}
				};
				OBDRequest req_setCodingSequence = new OBDRequest("2EF198", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				req_setCodingSequence.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if (data != null && data.Contains("6EF198"))
					{
						IProgress<string> progress2 = progress;
						if (progress2 == null)
						{
							return;
						}
						progress2.Report(Translate.GetString("coding_progress_ApplyingNewData"));
						return;
					}
					else
					{
						App.OBDReader.DebugWrite("\n2EF198_error\n");
						IProgress<string> progress3 = progress;
						if (progress3 == null)
						{
							return;
						}
						progress3.Report(Translate.GetString("coding_progress_ApplyingNewData"));
						return;
					}
				};
				OBDRequest obdrequest6 = new OBDRequest("22F1A5", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
				{
					ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations,
					CheckLength = true
				};
				obdrequest6.ResponseDecoded += delegate(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
				{
					if (getCodingData != null && getCodingData.Length != 0)
					{
						string text2 = BitHelpers.ByteArrayToHexString(getCodingData);
						if (getCodingData.All((byte x) => x == 0))
						{
							text2 = "0181C8F63039";
						}
						if (text2.Length == 6 && req_getCoding2.ForceManualFlowControl)
						{
							req_getCoding2.ForceManualFlowControl = false;
							App.OBDReader.InsertRequestInQueue(req_getCoding2);
							text2 = "0181C8F63039";
						}
						if (text2.Length == 6)
						{
							text2 = "0181C8F63039";
						}
						string text3 = "2EF198" + text2;
						req_setCodingSequence.Command = text3;
						return;
					}
					string text4 = "2EF1980181C8F63039";
					req_setCodingSequence.Command = text4;
					if (req_getCoding2.ForceManualFlowControl == SharedSettings.Current.ForceUseManualFlowControlForCodingOperations)
					{
						req_getCoding2.ForceManualFlowControl = !SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
						App.OBDReader.InsertRequestInQueue(req_getCoding2);
					}
				};
				OBDRequest obdrequest7 = new OBDRequest("1003", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest7, obdrequest5, obdrequest6, req_setDate, req_setCodingSequence, obdrequest, obdrequest2, obdrequest3, obdrequest4 });
				await semaphore.WaitAsync();
			}
			if (optionValue == this.cancelOption.Value)
			{
				OBDRequest obdrequest8 = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest8.ResponseReceived += checkReceivedResponseForNegativeResult;
				obdrequest8.ResponseReceived += delegate(OBDRequest request, string data)
				{
					progress.Report(Translate.GetString("coding_OperationFinished"));
					codingResult = CodingRequestResult.Success;
					semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest8 });
				await semaphore.WaitAsync();
			}
			return codingResult;
		}

		// Token: 0x040034F2 RID: 13554
		protected MQBAdaptationOption startOption;

		// Token: 0x040034F3 RID: 13555
		protected new MQBAdaptationOption cancelOption;

		// Token: 0x040034F4 RID: 13556
		private string status_string_0102;

		// Token: 0x040034F5 RID: 13557
		private string status_string_0104;

		// Token: 0x040034F6 RID: 13558
		private string status_string_0106;

		// Token: 0x02000AAE RID: 2734
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005614 RID: 22036 RVA: 0x004108D3 File Offset: 0x0040EAD3
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005615 RID: 22037 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005616 RID: 22038 RVA: 0x00385EA9 File Offset: 0x003840A9
			internal void <OptionExecute>b__6_5(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && !data.Contains("6EF199"))
				{
					App.OBDReader.DebugWrite("\n2EF199_error\n");
				}
			}

			// Token: 0x06005617 RID: 22039 RVA: 0x0037EDD4 File Offset: 0x0037CFD4
			internal bool <OptionExecute>b__6_9(byte x)
			{
				return x == 0;
			}

			// Token: 0x040034F7 RID: 13559
			public static readonly EngineAutomaticTestsProcedure.<>c <>9 = new EngineAutomaticTestsProcedure.<>c();

			// Token: 0x040034F8 RID: 13560
			public static ResponseReceivedDelegate <>9__6_5;

			// Token: 0x040034F9 RID: 13561
			public static Func<byte, bool> <>9__6_9;
		}

		// Token: 0x02000AAF RID: 2735
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06005618 RID: 22040 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06005619 RID: 22041 RVA: 0x004108E0 File Offset: 0x0040EAE0
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

			// Token: 0x0600561A RID: 22042 RVA: 0x00410A18 File Offset: 0x0040EC18
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

			// Token: 0x0600561B RID: 22043 RVA: 0x00410B94 File Offset: 0x0040ED94
			internal void <OptionExecute>b__3(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.codingResult = CodingRequestResult.UnknownError;
					this.semaphore.Release();
					return;
				}
				this.<>4__this.status_string_0104 = this.<>4__this.StatusFrom0104ToString(data);
				this.progress.Report(string.Concat(new string[]
				{
					this.<>4__this.status_string_0102,
					"\n",
					this.<>4__this.status_string_0104,
					"\nTest steps to be perfomed: ",
					this.<>4__this.status_string_0106
				}));
			}

			// Token: 0x0600561C RID: 22044 RVA: 0x00410C23 File Offset: 0x0040EE23
			internal void <OptionExecute>b__4(OBDRequest check_result_request_0106_2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.<>4__this.status_string_0106 = data[0].ToString();
				}
			}

			// Token: 0x0600561D RID: 22045 RVA: 0x00410C44 File Offset: 0x0040EE44
			internal void <OptionExecute>b__7(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && data.Contains("6EF198"))
				{
					IProgress<string> progress = this.progress;
					if (progress == null)
					{
						return;
					}
					progress.Report(Translate.GetString("coding_progress_ApplyingNewData"));
					return;
				}
				else
				{
					App.OBDReader.DebugWrite("\n2EF198_error\n");
					IProgress<string> progress2 = this.progress;
					if (progress2 == null)
					{
						return;
					}
					progress2.Report(Translate.GetString("coding_progress_ApplyingNewData"));
					return;
				}
			}

			// Token: 0x0600561E RID: 22046 RVA: 0x00410CB1 File Offset: 0x0040EEB1
			internal void <OptionExecute>b__10(OBDRequest request, string data)
			{
				this.progress.Report(Translate.GetString("coding_OperationFinished"));
				this.codingResult = CodingRequestResult.Success;
				this.semaphore.Release();
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x040034FA RID: 13562
			public CodingRequestResult codingResult;

			// Token: 0x040034FB RID: 13563
			public SemaphoreSlim semaphore;

			// Token: 0x040034FC RID: 13564
			public EngineAutomaticTestsProcedure <>4__this;

			// Token: 0x040034FD RID: 13565
			public IProgress<string> progress;
		}

		// Token: 0x02000AB0 RID: 2736
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_1
		{
			// Token: 0x0600561F RID: 22047 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_1()
			{
			}

			// Token: 0x06005620 RID: 22048 RVA: 0x00410CE8 File Offset: 0x0040EEE8
			internal void <OptionExecute>b__2(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
					this.CS$<>8__locals1.semaphore.Release();
					return;
				}
				this.CS$<>8__locals1.<>4__this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
				this.CS$<>8__locals1.progress.Report(string.Concat(new string[]
				{
					this.CS$<>8__locals1.<>4__this.status_string_0102,
					"\n",
					this.CS$<>8__locals1.<>4__this.status_string_0104,
					"\nTest steps to be perfomed: ",
					this.CS$<>8__locals1.<>4__this.status_string_0106
				}));
				if (data[0] == 16)
				{
					this.status0102_finished = true;
					this.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
					this.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
					this.CS$<>8__locals1.semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				}
			}

			// Token: 0x06005621 RID: 22049 RVA: 0x00410DE8 File Offset: 0x0040EFE8
			internal void <OptionExecute>b__6(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
			{
				if (getDateData != null && getDateData.Length >= 3)
				{
					string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
					this.req_setDate.Command = text;
				}
			}

			// Token: 0x06005622 RID: 22050 RVA: 0x00410E48 File Offset: 0x0040F048
			internal void <OptionExecute>b__8(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
			{
				if (getCodingData != null && getCodingData.Length != 0)
				{
					string text = BitHelpers.ByteArrayToHexString(getCodingData);
					if (getCodingData.All((byte x) => x == 0))
					{
						text = "0181C8F63039";
					}
					if (text.Length == 6 && req_getCoding2.ForceManualFlowControl)
					{
						req_getCoding2.ForceManualFlowControl = false;
						App.OBDReader.InsertRequestInQueue(req_getCoding2);
						text = "0181C8F63039";
					}
					if (text.Length == 6)
					{
						text = "0181C8F63039";
					}
					string text2 = "2EF198" + text;
					this.req_setCodingSequence.Command = text2;
					return;
				}
				string text3 = "2EF1980181C8F63039";
				this.req_setCodingSequence.Command = text3;
				if (req_getCoding2.ForceManualFlowControl == SharedSettings.Current.ForceUseManualFlowControlForCodingOperations)
				{
					req_getCoding2.ForceManualFlowControl = !SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
					App.OBDReader.InsertRequestInQueue(req_getCoding2);
				}
			}

			// Token: 0x040034FE RID: 13566
			public bool status0102_finished;

			// Token: 0x040034FF RID: 13567
			public OBDRequest req_setDate;

			// Token: 0x04003500 RID: 13568
			public OBDRequest req_setCodingSequence;

			// Token: 0x04003501 RID: 13569
			public EngineAutomaticTestsProcedure.<>c__DisplayClass6_0 CS$<>8__locals1;
		}

		// Token: 0x02000AB1 RID: 2737
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__6 : IAsyncStateMachine
		{
			// Token: 0x06005623 RID: 22051 RVA: 0x00410F2C File Offset: 0x0040F12C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				EngineAutomaticTestsProcedure engineAutomaticTestsProcedure = this;
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
							goto IL_04C2;
						}
						CS$<>8__locals1 = new EngineAutomaticTestsProcedure.<>c__DisplayClass6_0();
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
							OBDRequest obdrequest9 = new OBDRequest(CS$<>8__locals1.<>4__this.cancelOption.Value, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false);
							if (data == null)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest9 });
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							if (data.Contains("NO DATA"))
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest9 });
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
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest9 });
								CS$<>8__locals1.semaphore.Release();
							}
						};
						if (!(optionValue == engineAutomaticTestsProcedure.startOption.Value))
						{
							goto IL_03EF;
						}
						EngineAutomaticTestsProcedure.<>c__DisplayClass6_1 CS$<>8__locals2 = new EngineAutomaticTestsProcedure.<>c__DisplayClass6_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						OBDRequest obdrequest = new OBDRequest(optionValue, engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("220102", engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, true);
						OBDRequest obdrequest3 = new OBDRequest("220104", engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, true);
						OBDRequest obdrequest4 = new OBDRequest("220106", engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, true);
						obdrequest.ResponseReceived += responseReceivedDelegate;
						obdrequest2.ResponseReceived += checkReceivedResponseForNegativeResult;
						obdrequest3.ResponseReceived += checkReceivedResponseForNegativeResult;
						CS$<>8__locals2.status0102_finished = false;
						obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								return;
							}
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0102 = MQBServiceProcedure.Status0102ByteToString(data[0]);
							CS$<>8__locals2.CS$<>8__locals1.progress.Report(string.Concat(new string[]
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0102,
								"\n",
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0104,
								"\nTest steps to be perfomed: ",
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0106
							}));
							if (data[0] == 16)
							{
								CS$<>8__locals2.status0102_finished = true;
								CS$<>8__locals2.CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.Success;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								App.OBDReader.ClearRequestQueue();
							}
						};
						obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals2.CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								CS$<>8__locals2.CS$<>8__locals1.semaphore.Release();
								return;
							}
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0104 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.StatusFrom0104ToString(data);
							CS$<>8__locals2.CS$<>8__locals1.progress.Report(string.Concat(new string[]
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0102,
								"\n",
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0104,
								"\nTest steps to be perfomed: ",
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0106
							}));
						};
						obdrequest4.ResponseDecoded += delegate(OBDRequest check_result_request_0106_2, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.status_string_0106 = data[0].ToString();
							}
						};
						CS$<>8__locals2.req_setDate = new OBDRequest(string.Format("2EF199{0}{1}{2}", DateTimeNowHelper.NowSafe.Year - 2000, DateTimeNowHelper.NowSafe.Month.ToString("00"), DateTimeNowHelper.NowSafe.Day.ToString("00")), engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, false);
						CS$<>8__locals2.req_setDate.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data != null)
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
							}
							if (data != null && !data.Contains("6EF199"))
							{
								App.OBDReader.DebugWrite("\n2EF199_error\n");
							}
						};
						OBDRequest obdrequest5 = new OBDRequest("22F199", engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, false)
						{
							ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
						};
						obdrequest5.ResponseDecoded += delegate(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
						{
							if (getDateData != null && getDateData.Length >= 3)
							{
								string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
								CS$<>8__locals2.req_setDate.Command = text;
							}
						};
						CS$<>8__locals2.req_setCodingSequence = new OBDRequest("2EF198", engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, false);
						CS$<>8__locals2.req_setCodingSequence.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data != null)
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
							}
							if (data != null && data.Contains("6EF198"))
							{
								IProgress<string> progress = CS$<>8__locals2.CS$<>8__locals1.progress;
								if (progress == null)
								{
									return;
								}
								progress.Report(Translate.GetString("coding_progress_ApplyingNewData"));
								return;
							}
							else
							{
								App.OBDReader.DebugWrite("\n2EF198_error\n");
								IProgress<string> progress2 = CS$<>8__locals2.CS$<>8__locals1.progress;
								if (progress2 == null)
								{
									return;
								}
								progress2.Report(Translate.GetString("coding_progress_ApplyingNewData"));
								return;
							}
						};
						OBDRequest obdrequest6 = new OBDRequest("22F1A5", engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, false)
						{
							ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations,
							CheckLength = true
						};
						obdrequest6.ResponseDecoded += delegate(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
						{
							if (getCodingData != null && getCodingData.Length != 0)
							{
								string text2 = BitHelpers.ByteArrayToHexString(getCodingData);
								if (getCodingData.All((byte x) => x == 0))
								{
									text2 = "0181C8F63039";
								}
								if (text2.Length == 6 && req_getCoding2.ForceManualFlowControl)
								{
									req_getCoding2.ForceManualFlowControl = false;
									App.OBDReader.InsertRequestInQueue(req_getCoding2);
									text2 = "0181C8F63039";
								}
								if (text2.Length == 6)
								{
									text2 = "0181C8F63039";
								}
								string text3 = "2EF198" + text2;
								CS$<>8__locals2.req_setCodingSequence.Command = text3;
								return;
							}
							string text4 = "2EF1980181C8F63039";
							CS$<>8__locals2.req_setCodingSequence.Command = text4;
							if (req_getCoding2.ForceManualFlowControl == SharedSettings.Current.ForceUseManualFlowControlForCodingOperations)
							{
								req_getCoding2.ForceManualFlowControl = !SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
								App.OBDReader.InsertRequestInQueue(req_getCoding2);
							}
						};
						OBDRequest obdrequest7 = new OBDRequest("1003", engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, false);
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest7, obdrequest5, obdrequest6, CS$<>8__locals2.req_setDate, CS$<>8__locals2.req_setCodingSequence, obdrequest, obdrequest2, obdrequest3, obdrequest4 });
						taskAwaiter = CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, EngineAutomaticTestsProcedure.<OptionExecute>d__6>(ref taskAwaiter, ref this);
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
					IL_03EF:
					if (!(optionValue == engineAutomaticTestsProcedure.cancelOption.Value))
					{
						goto IL_04C9;
					}
					OBDRequest obdrequest8 = new OBDRequest(optionValue, engineAutomaticTestsProcedure.RequestHeader, engineAutomaticTestsProcedure.BeforeCommands, engineAutomaticTestsProcedure.AfterCommands, false);
					obdrequest8.ResponseReceived += checkReceivedResponseForNegativeResult;
					obdrequest8.ResponseReceived += delegate(OBDRequest request, string data)
					{
						CS$<>8__locals1.progress.Report(Translate.GetString("coding_OperationFinished"));
						CS$<>8__locals1.codingResult = CodingRequestResult.Success;
						CS$<>8__locals1.semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					};
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest8 });
					taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, EngineAutomaticTestsProcedure.<OptionExecute>d__6>(ref taskAwaiter, ref this);
						return;
					}
					IL_04C2:
					taskAwaiter.GetResult();
					IL_04C9:
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

			// Token: 0x06005624 RID: 22052 RVA: 0x00411478 File Offset: 0x0040F678
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003502 RID: 13570
			public int <>1__state;

			// Token: 0x04003503 RID: 13571
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003504 RID: 13572
			public EngineAutomaticTestsProcedure <>4__this;

			// Token: 0x04003505 RID: 13573
			public IProgress<string> progress;

			// Token: 0x04003506 RID: 13574
			public string optionValue;

			// Token: 0x04003507 RID: 13575
			private EngineAutomaticTestsProcedure.<>c__DisplayClass6_0 <>8__1;

			// Token: 0x04003508 RID: 13576
			private ResponseReceivedDelegate <checkReceivedResponseForNegativeResult>5__2;

			// Token: 0x04003509 RID: 13577
			private TaskAwaiter <>u__1;
		}
	}
}
