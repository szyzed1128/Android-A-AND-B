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
using Xamarin.Forms;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x020009FA RID: 2554
	internal class ESPConfigAutoRestore : CustomizableCodingTemplate
	{
		// Token: 0x060051C6 RID: 20934 RVA: 0x003F4180 File Offset: 0x003F2380
		public ESPConfigAutoRestore()
		{
			this.Group = CodingGroup.Other;
			base.Name = "EPS Configuration automatic resoration";
			base.InnerDescription = "This item supports only a limited number of ESP configurations for automatic restoration. You can use change settings manually if your vehicle configuration is not supported";
			TranslationItem translationItem = new TranslationItem("ru", "Автоматическое восстановление конфигурации EPS (ЭУР)", "", "Этот пункт поддерживает ограниченное число конфигураций автомобилей. Если конфигурация Вашего автомобиля не поддерживается, воспользуйтесь ручным изменением параметров конфигурации EPS (ЭУР)");
			base.Translations.Add(translationItem);
			this.ValueType = AdaptationValueTypes.OptionType;
			base.RequestHeader = "742";
			base.ResponseHeader = "762";
			base.Protocol = "6";
			this.PasswordVisible = false;
			base.MakeChangesToInitialData = false;
			base.OpenSessionCommand = "1003";
			this.ReadModeAndAddress = "22F188";
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x003F4224 File Offset: 0x003F2424
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.Options.Clear();
			base.CurrentState = "";
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(this.ReadModeAndAddress))
			{
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
				CodingRequestResult item = tuple.Item2;
				byte[] item2 = tuple.Item1;
				if (item != CodingRequestResult.Success)
				{
					base.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
					codingRequestResult = item;
				}
				else
				{
					string ident = Encoding.ASCII.GetString(item2);
					ESPConfig espconfig = this.BuildDB().FirstOrDefault((ESPConfig x) => x.Ident == ident);
					if (espconfig != null)
					{
						base.CurrentState = ident;
						this.currentConfig = espconfig;
						this.Options.Add(new MQBAdaptationOption("START", ident));
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						this.Options.Clear();
						base.CurrentState = "Config not supported";
						codingRequestResult = CodingRequestResult.NotSupported;
					}
				}
			}
			return codingRequestResult;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x003F4270 File Offset: 0x003F2470
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			CodingRequestResult codingRequestResult;
			if (this.currentConfig == null)
			{
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				int counter = 1;
				Progress<string> internalProgress = new Progress<string>(delegate(string str)
				{
					string text = Translate.GetString("coding_progress_step");
					text = string.Format(text, counter, this.currentConfig.Data.Count);
					IProgress<string> progress2 = progress;
					if (progress2 == null)
					{
						return;
					}
					progress2.Report(text + "\n" + str);
				});
				foreach (KeyValuePair<string, string> kvp in this.currentConfig.Data)
				{
					CodingRequestResult codingRequestResult2 = await new CustomizableCodingTemplate
					{
						Name = "EPS Config $" + kvp.Key,
						ValueType = AdaptationValueTypes.InputHexDataType,
						Protocol = base.Protocol,
						RequestHeader = base.RequestHeader,
						ResponseHeader = base.ResponseHeader,
						MakeChangesToInitialData = true,
						ReadModeAndAddress = "22" + kvp.Key,
						WriteModeAndAddress = "2E" + kvp.Key,
						OpenSessionCommand = "1003"
					}.Execute("", kvp.Value, kvp.Value, internalProgress, null, false);
					CodingRequestResult step_result = codingRequestResult2;
					if (step_result != CodingRequestResult.Success)
					{
						Page currentPage = App.GetCurrentPage();
						await ((currentPage != null) ? currentPage.DisplayAlert("Error!", "Error in $" + kvp.Value + ": " + MQBAdaptationTemplate.CodingRequestResultToString(step_result), "OK") : null);
						return step_result;
					}
					counter++;
					kvp = default(KeyValuePair<string, string>);
				}
				Dictionary<string, string>.Enumerator enumerator = default(Dictionary<string, string>.Enumerator);
				OBDRequest obdrequest = new OBDRequest("1101", "742", this.BeforeCommands, this.AfterCommands, false);
				App.OBDReader.ReplaceQueue(obdrequest);
				await App.OBDReader.WaitForCommandQueue();
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x003F42BC File Offset: 0x003F24BC
		private List<ESPConfig> BuildDB()
		{
			return new List<ESPConfig>
			{
				new ESPConfig("285043733R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "00"),
					new ValueTuple<string, string>("01D0", "01"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "05"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("85048908R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "00"),
					new ValueTuple<string, string>("01D0", "01"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "05"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("285049726R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "00"),
					new ValueTuple<string, string>("01D0", "01"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "05"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("285043733R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "00"),
					new ValueTuple<string, string>("01D0", "01"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "05"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("285047876R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "02"),
					new ValueTuple<string, string>("01D0", "01"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "05"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("285048594R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "02"),
					new ValueTuple<string, string>("01D0", "01"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "05"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("285042167R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "00"),
					new ValueTuple<string, string>("01D0", "00"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "00"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("285044180R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "00"),
					new ValueTuple<string, string>("01D0", "00"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "00"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("285040084R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "00"),
					new ValueTuple<string, string>("01D0", "00"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "05"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				}),
				new ESPConfig("285048999R", new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("01C7", "00"),
					new ValueTuple<string, string>("01E7", "00"),
					new ValueTuple<string, string>("01C6", "00"),
					new ValueTuple<string, string>("01C3", "01"),
					new ValueTuple<string, string>("017D", "00"),
					new ValueTuple<string, string>("01D0", "00"),
					new ValueTuple<string, string>("01CE", "00"),
					new ValueTuple<string, string>("01CF", "0F"),
					new ValueTuple<string, string>("012A", "01"),
					new ValueTuple<string, string>("01CB", "00"),
					new ValueTuple<string, string>("0168", "05"),
					new ValueTuple<string, string>("C000", "01"),
					new ValueTuple<string, string>("C200", "01"),
					new ValueTuple<string, string>("01CD", "01")
				})
			};
		}

		// Token: 0x040031B3 RID: 12723
		private ESPConfig currentConfig;

		// Token: 0x020009FB RID: 2555
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060051CA RID: 20938 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060051CB RID: 20939 RVA: 0x003F4FF0 File Offset: 0x003F31F0
			internal bool <UpdateCurrentState>b__0(ESPConfig x)
			{
				return x.Ident == this.ident;
			}

			// Token: 0x040031B4 RID: 12724
			public string ident;
		}

		// Token: 0x020009FC RID: 2556
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060051CC RID: 20940 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060051CD RID: 20941 RVA: 0x003F5004 File Offset: 0x003F3204
			internal void <Execute>b__0(string str)
			{
				string text = Translate.GetString("coding_progress_step");
				text = string.Format(text, this.counter, this.<>4__this.currentConfig.Data.Count);
				IProgress<string> progress = this.progress;
				if (progress == null)
				{
					return;
				}
				progress.Report(text + "\n" + str);
			}

			// Token: 0x040031B5 RID: 12725
			public int counter;

			// Token: 0x040031B6 RID: 12726
			public ESPConfigAutoRestore <>4__this;

			// Token: 0x040031B7 RID: 12727
			public IProgress<string> progress;
		}

		// Token: 0x020009FD RID: 2557
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x060051CE RID: 20942 RVA: 0x003F5064 File Offset: 0x003F3264
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ESPConfigAutoRestore espconfigAutoRestore = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter;
					if (num > 1)
					{
						if (num == 2)
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0369;
						}
						CS$<>8__locals1 = new ESPConfigAutoRestore.<>c__DisplayClass3_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						espconfigAutoRestore.BuildDefaultBeforeAndAfterCommands();
						if (espconfigAutoRestore.currentConfig == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_039B;
						}
						CS$<>8__locals1.counter = 1;
						internalProgress = new Progress<string>(delegate(string str)
						{
							string text = Translate.GetString("coding_progress_step");
							text = string.Format(text, CS$<>8__locals1.counter, CS$<>8__locals1.<>4__this.currentConfig.Data.Count);
							IProgress<string> progress = CS$<>8__locals1.progress;
							if (progress == null)
							{
								return;
							}
							progress.Report(text + "\n" + str);
						});
						enumerator = espconfigAutoRestore.currentConfig.Data.GetEnumerator();
					}
					try
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter3;
						if (num != 0)
						{
							if (num != 1)
							{
								goto IL_02B5;
							}
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_027A;
						}
						else
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<CodingRequestResult>);
							num = (num2 = -1);
						}
						IL_01CD:
						CodingRequestResult result = taskAwaiter3.GetResult();
						step_result = result;
						if (step_result == CodingRequestResult.Success)
						{
							int counter = CS$<>8__locals1.counter;
							CS$<>8__locals1.counter = counter + 1;
							kvp = default(KeyValuePair<string, string>);
							goto IL_02B5;
						}
						Page currentPage = App.GetCurrentPage();
						taskAwaiter = ((currentPage != null) ? currentPage.DisplayAlert("Error!", "Error in $" + kvp.Value + ": " + MQBAdaptationTemplate.CodingRequestResultToString(step_result), "OK") : null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ESPConfigAutoRestore.<Execute>d__3>(ref taskAwaiter, ref this);
							return;
						}
						IL_027A:
						taskAwaiter.GetResult();
						codingRequestResult = step_result;
						goto IL_039B;
						IL_02B5:
						if (enumerator.MoveNext())
						{
							kvp = enumerator.Current;
							taskAwaiter3 = new CustomizableCodingTemplate
							{
								Name = "EPS Config $" + kvp.Key,
								ValueType = AdaptationValueTypes.InputHexDataType,
								Protocol = espconfigAutoRestore.Protocol,
								RequestHeader = espconfigAutoRestore.RequestHeader,
								ResponseHeader = espconfigAutoRestore.ResponseHeader,
								MakeChangesToInitialData = true,
								ReadModeAndAddress = "22" + kvp.Key,
								WriteModeAndAddress = "2E" + kvp.Key,
								OpenSessionCommand = "1003"
							}.Execute("", kvp.Value, kvp.Value, internalProgress, null, false).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<CodingRequestResult> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, ESPConfigAutoRestore.<Execute>d__3>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01CD;
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					enumerator = default(Dictionary<string, string>.Enumerator);
					OBDRequest obdrequest = new OBDRequest("1101", "742", espconfigAutoRestore.BeforeCommands, espconfigAutoRestore.AfterCommands, false);
					App.OBDReader.ReplaceQueue(obdrequest);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 2);
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ESPConfigAutoRestore.<Execute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_0369:
					taskAwaiter.GetResult();
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					internalProgress = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_039B:
				num2 = -2;
				CS$<>8__locals1 = null;
				internalProgress = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060051CF RID: 20943 RVA: 0x003F5464 File Offset: 0x003F3664
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040031B8 RID: 12728
			public int <>1__state;

			// Token: 0x040031B9 RID: 12729
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040031BA RID: 12730
			public ESPConfigAutoRestore <>4__this;

			// Token: 0x040031BB RID: 12731
			public IProgress<string> progress;

			// Token: 0x040031BC RID: 12732
			private ESPConfigAutoRestore.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x040031BD RID: 12733
			private Progress<string> <internalProgress>5__2;

			// Token: 0x040031BE RID: 12734
			private Dictionary<string, string>.Enumerator <>7__wrap2;

			// Token: 0x040031BF RID: 12735
			private KeyValuePair<string, string> <kvp>5__4;

			// Token: 0x040031C0 RID: 12736
			private CodingRequestResult <step_result>5__5;

			// Token: 0x040031C1 RID: 12737
			private TaskAwaiter<CodingRequestResult> <>u__1;

			// Token: 0x040031C2 RID: 12738
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020009FE RID: 2558
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x060051D0 RID: 20944 RVA: 0x003F5474 File Offset: 0x003F3674
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ESPConfigAutoRestore espconfigAutoRestore = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new ESPConfigAutoRestore.<>c__DisplayClass2_0();
						espconfigAutoRestore.Options.Clear();
						espconfigAutoRestore.CurrentState = "";
						if (string.IsNullOrEmpty(espconfigAutoRestore.ReadModeAndAddress))
						{
							codingRequestResult = CodingRequestResult.Success;
							goto IL_017F;
						}
						taskAwaiter = espconfigAutoRestore.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, ESPConfigAutoRestore.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						espconfigAutoRestore.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						CS$<>8__locals1.ident = Encoding.ASCII.GetString(item2);
						ESPConfig espconfig = espconfigAutoRestore.BuildDB().FirstOrDefault((ESPConfig x) => x.Ident == CS$<>8__locals1.ident);
						if (espconfig != null)
						{
							espconfigAutoRestore.CurrentState = CS$<>8__locals1.ident;
							espconfigAutoRestore.currentConfig = espconfig;
							espconfigAutoRestore.Options.Add(new MQBAdaptationOption("START", CS$<>8__locals1.ident));
							codingRequestResult = CodingRequestResult.Success;
						}
						else
						{
							espconfigAutoRestore.Options.Clear();
							espconfigAutoRestore.CurrentState = "Config not supported";
							codingRequestResult = CodingRequestResult.NotSupported;
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
				IL_017F:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060051D1 RID: 20945 RVA: 0x003F5638 File Offset: 0x003F3838
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040031C3 RID: 12739
			public int <>1__state;

			// Token: 0x040031C4 RID: 12740
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040031C5 RID: 12741
			public ESPConfigAutoRestore <>4__this;

			// Token: 0x040031C6 RID: 12742
			public string password;

			// Token: 0x040031C7 RID: 12743
			private ESPConfigAutoRestore.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x040031C8 RID: 12744
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
