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
	// Token: 0x02000B64 RID: 2916
	internal class TPMSResetRoutine : MQBServiceProcedure
	{
		// Token: 0x060059EA RID: 23018 RVA: 0x0042DE74 File Offset: 0x0042C074
		public TPMSResetRoutine()
		{
			base.Name = "Reset TPMS adaptation (for TPMS based on ABS sensors)";
			base.InnerDescription = "This is the same as SET button, but some multimedia systems doesn't have it";
			this.Unit = "03";
			this.ResetOption1 = new MQBAdaptationOption("Reset var.1", "31010413", new TranslationItem[]
			{
				new TranslationItem("ru", "Сброс (вариант 1)", "", "")
			});
			this.ResetOption2 = new MQBAdaptationOption("Reset var.2", "31010413040000", new TranslationItem[]
			{
				new TranslationItem("ru", "Сброс  (вариант 2)", "", "")
			});
			base.Group = CodingGroup.TPMS;
			base.Options.Add(this.ResetOption1);
			base.Options.Add(this.ResetOption2);
			base.Translations.Add(new TranslationItem("ru", "Сброс адаптаций системы контроля давления в шинах (для косвенной TPMS, работающей по датчикам ABS)", "", "")
			{
				AdditionalText = "Функция предназначена для тех, у кого в мультимедийной системе нет кнопки SET"
			});
			this.Password = "20103";
			base.PasswordHint = "40168, 20103";
			base.PasswordVisible = true;
		}

		// Token: 0x060059EB RID: 23019 RVA: 0x0042DF8C File Offset: 0x0042C18C
		protected override async Task<CodingRequestResult> OptionExecute(string optionValue, IProgress<string> progress)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
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
			if (optionValue == this.ResetOption1.Value || optionValue == this.ResetOption2.Value)
			{
				OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				OBDRequest obdrequest2 = new OBDRequest("220102", this.RequestHeader, this.BeforeCommands, this.AfterCommands, true);
				obdrequest.ResponseReceived += responseReceivedDelegate;
				obdrequest2.ResponseReceived += responseReceivedDelegate;
				obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data == null || data.Length == 0)
					{
						codingResult = CodingRequestResult.UnknownError;
						semaphore.Release();
						return;
					}
					TPMSResetRoutine.ReportCurrentState(progress, data);
					byte b = data[0];
					if (b == 16 || b == 128)
					{
						codingResult = CodingRequestResult.Success;
						semaphore.Release();
						App.OBDReader.ClearRequestQueue();
					}
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
				await semaphore.WaitAsync();
			}
			return codingResult;
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x0042DFE0 File Offset: 0x0042C1E0
		private static void ReportCurrentState(IProgress<string> progress, byte[] data)
		{
			string text = MQBServiceProcedure.Status0102ByteToString(data[0]);
			progress.Report(text);
		}

		// Token: 0x04003850 RID: 14416
		private MQBAdaptationOption ResetOption1;

		// Token: 0x04003851 RID: 14417
		private MQBAdaptationOption ResetOption2;

		// Token: 0x02000B65 RID: 2917
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060059ED RID: 23021 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060059EE RID: 23022 RVA: 0x0042E000 File Offset: 0x0042C200
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

			// Token: 0x060059EF RID: 23023 RVA: 0x0042E138 File Offset: 0x0042C338
			internal void <OptionExecute>b__1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.codingResult = CodingRequestResult.UnknownError;
					this.semaphore.Release();
					return;
				}
				TPMSResetRoutine.ReportCurrentState(this.progress, data);
				byte b = data[0];
				if (b == 16 || b == 128)
				{
					this.codingResult = CodingRequestResult.Success;
					this.semaphore.Release();
					App.OBDReader.ClearRequestQueue();
				}
			}

			// Token: 0x04003852 RID: 14418
			public CodingRequestResult codingResult;

			// Token: 0x04003853 RID: 14419
			public SemaphoreSlim semaphore;

			// Token: 0x04003854 RID: 14420
			public IProgress<string> progress;
		}

		// Token: 0x02000B66 RID: 2918
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__3 : IAsyncStateMachine
		{
			// Token: 0x060059F0 RID: 23024 RVA: 0x0042E19C File Offset: 0x0042C39C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TPMSResetRoutine tpmsresetRoutine = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new TPMSResetRoutine.<>c__DisplayClass3_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
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
						if (!(optionValue == tpmsresetRoutine.ResetOption1.Value) && !(optionValue == tpmsresetRoutine.ResetOption2.Value))
						{
							goto IL_0179;
						}
						OBDRequest obdrequest = new OBDRequest(optionValue, tpmsresetRoutine.RequestHeader, tpmsresetRoutine.BeforeCommands, tpmsresetRoutine.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("220102", tpmsresetRoutine.RequestHeader, tpmsresetRoutine.BeforeCommands, tpmsresetRoutine.AfterCommands, true);
						obdrequest.ResponseReceived += responseReceivedDelegate;
						obdrequest2.ResponseReceived += responseReceivedDelegate;
						obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								CS$<>8__locals1.semaphore.Release();
								return;
							}
							TPMSResetRoutine.ReportCurrentState(CS$<>8__locals1.progress, data);
							byte b = data[0];
							if (b == 16 || b == 128)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
								CS$<>8__locals1.semaphore.Release();
								App.OBDReader.ClearRequestQueue();
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TPMSResetRoutine.<OptionExecute>d__3>(ref taskAwaiter, ref this);
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
					IL_0179:
					codingResult = CS$<>8__locals1.codingResult;
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
				this.<>t__builder.SetResult(codingResult);
			}

			// Token: 0x060059F1 RID: 23025 RVA: 0x0042E388 File Offset: 0x0042C588
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003855 RID: 14421
			public int <>1__state;

			// Token: 0x04003856 RID: 14422
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003857 RID: 14423
			public IProgress<string> progress;

			// Token: 0x04003858 RID: 14424
			public string optionValue;

			// Token: 0x04003859 RID: 14425
			public TPMSResetRoutine <>4__this;

			// Token: 0x0400385A RID: 14426
			private TPMSResetRoutine.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x0400385B RID: 14427
			private TaskAwaiter <>u__1;
		}
	}
}
