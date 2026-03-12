using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B17 RID: 2839
	internal class MQBMultimediaRestart : MQBServiceProcedure
	{
		// Token: 0x06005859 RID: 22617 RVA: 0x00422658 File Offset: 0x00420858
		public MQBMultimediaRestart()
		{
			base.Name = "Restart mutltimedia system";
			base.Description = "After applying some codings you need to restart your multimedia system for changes to take effect";
			this.Unit = "5F";
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption("Restart", "1102", new TranslationItem[]
			{
				new TranslationItem("ru", "Перезагрузка", "", "")
			});
			base.Translations.Add(new TranslationItem("ru", "Перезагрузить мультимедийную систему", "После применения некоторых кодировок необходимо перезагрузить мультимедийную систему, чтобы изменения вступили в силу.", ""));
			base.Group = CodingGroup.Multimedia;
			base.Options.Add(mqbadaptationOption);
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x004226F8 File Offset: 0x004208F8
		protected override async Task<CodingRequestResult> OptionExecute(string optionValue, IProgress<string> progress)
		{
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			OBDRequest obdrequest = new OBDRequest(optionValue, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (data.Contains("7F1178") || data.Contains("51"))
					{
						progress.Report(MQBAdaptationTemplate.CodingRequestResultToString(CodingRequestResult.Success));
						codingResult = CodingRequestResult.Success;
					}
				}
				semaphore.Release();
			};
			App.OBDReader.AddRequestToQueue(obdrequest);
			await semaphore.WaitAsync();
			return codingResult;
		}

		// Token: 0x02000B18 RID: 2840
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x0600585B RID: 22619 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x0600585C RID: 22620 RVA: 0x0042274C File Offset: 0x0042094C
			internal void <OptionExecute>b__0(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (data.Contains("7F1178") || data.Contains("51"))
					{
						this.progress.Report(MQBAdaptationTemplate.CodingRequestResultToString(CodingRequestResult.Success));
						this.codingResult = CodingRequestResult.Success;
					}
				}
				this.semaphore.Release();
			}

			// Token: 0x040036E1 RID: 14049
			public IProgress<string> progress;

			// Token: 0x040036E2 RID: 14050
			public CodingRequestResult codingResult;

			// Token: 0x040036E3 RID: 14051
			public SemaphoreSlim semaphore;
		}

		// Token: 0x02000B19 RID: 2841
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OptionExecute>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600585D RID: 22621 RVA: 0x004227A4 File Offset: 0x004209A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBMultimediaRestart mqbmultimediaRestart = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBMultimediaRestart.<>c__DisplayClass1_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						OBDRequest obdrequest = new OBDRequest(optionValue, mqbmultimediaRestart.RequestHeader, mqbmultimediaRestart.BeforeCommands, mqbmultimediaRestart.AfterCommands, false);
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data != null)
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								if (data.Contains("7F1178") || data.Contains("51"))
								{
									CS$<>8__locals1.progress.Report(MQBAdaptationTemplate.CodingRequestResultToString(CodingRequestResult.Success));
									CS$<>8__locals1.codingResult = CodingRequestResult.Success;
								}
							}
							CS$<>8__locals1.semaphore.Release();
						};
						App.OBDReader.AddRequestToQueue(obdrequest);
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBMultimediaRestart.<OptionExecute>d__1>(ref taskAwaiter, ref this);
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

			// Token: 0x0600585E RID: 22622 RVA: 0x00422900 File Offset: 0x00420B00
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040036E4 RID: 14052
			public int <>1__state;

			// Token: 0x040036E5 RID: 14053
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040036E6 RID: 14054
			public IProgress<string> progress;

			// Token: 0x040036E7 RID: 14055
			public string optionValue;

			// Token: 0x040036E8 RID: 14056
			public MQBMultimediaRestart <>4__this;

			// Token: 0x040036E9 RID: 14057
			private MQBMultimediaRestart.<>c__DisplayClass1_0 <>8__1;

			// Token: 0x040036EA RID: 14058
			private TaskAwaiter <>u__1;
		}
	}
}
