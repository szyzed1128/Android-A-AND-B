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
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B9B RID: 2971
	internal class HyundaiDPFRegen : CustomizableCodingTemplate
	{
		// Token: 0x06005A93 RID: 23187 RVA: 0x00433A2C File Offset: 0x00431C2C
		public HyundaiDPFRegen()
		{
			base.Name = "DPF Forced regeneration";
			base.Description = "Experimental function! Compatibility not guaranteed!";
			base.InnerDescription = "WARNING! Function not tested!\r\nPrerequisites:\r\n1) Coolant: >70 C\r\n2) Gearbox: P\r\n3) Engine running idle\r\n4) Turn on consumption related parts (A/C, headlights, etc)\r\n\r\nWARNING! Service regeneration may occur excessive temperature in the engine room. Please open vehicle hood and take off the engine cover to protect engine part.";
			base.RequestHeader = "7E0";
			base.ResponseHeader = "7E8";
			base.ATST = "96";
			base.OpenSessionCommand = "1003";
			base.PreWriteCommands = "22ED04;22ED1F;22ED94;2701;2702;31010272;22ED04;22ED1F;22ED94;22ED1E;22E0F1";
			base.PostWriteCommands = "3101023503;22ED04;22ED1F;22ED94;22ED1E";
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "";
			base.MakeChangesToInitialData = false;
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption("Start", "3101023501");
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption("Stop", "3101023502");
			this.Options.Add(mqbadaptationOption);
			this.Options.Add(mqbadaptationOption2);
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Group = CodingGroup.EngineAndPowertrain;
			this.HasCurrentState = true;
			this.PasswordVisible = false;
			base.Translations.Add(new TranslationItem("ru", "Сервисная регенерация (прожиг) сажевого фильтра", "ВНИМАНИЕ! Экспериментальная функция! Совместимость не гарантируется!", "Условия для запуска:\nТемпература ОЖ >70\nПоложение селектора АКПП: P\nДвигатель работает на холостом ходу\nВключите потребители (кондиционер, свет и т.п.)\nВНИМАНИЕ! Сервисная регенерация может вызвать очень большое тепловыделение в подкапотном пространстве! Пожалуйста, откройте капот и снимите крышку двигателя."));
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x06005A94 RID: 23188 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x06005A95 RID: 23189 RVA: 0x003F0A8F File Offset: 0x003EEC8F
		public override bool HasCurrentState
		{
			get
			{
				return true;
			}
			set
			{
				base.HasCurrentState = value;
			}
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x00433B44 File Offset: 0x00431D44
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			return CodingRequestResult.Success;
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x00433B80 File Offset: 0x00431D80
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			List<CustomPID> list = CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.Header == "7E0" && x.Name != null && x.Name.Contains("DPF") && x.Units != UnitsHelper.Units.km).ToList<CustomPID>();
			OBDRequest[] array = (from x in list.Select((CustomPID x) => x.Command).Distinct<string>().ToList<string>()
				select new OBDRequest(x, base.GetRequestHeaderForELM327(), "", "", true)
				{
					DoNotDecode = false
				}).ToArray<OBDRequest>();
			foreach (IPIDFloatValue ipidfloatValue in this.pids)
			{
				ipidfloatValue.ValueChanged -= this.Pid_ValueChanged;
			}
			this.pids.Clear();
			this.pids.AddRange(list);
			foreach (CustomPID customPID in list)
			{
				customPID.ValueChanged -= this.Pid_ValueChanged;
				customPID.ValueChanged += this.Pid_ValueChanged;
			}
			App.OBDReader.ReplaceQueue(array);
			List<OBDRequest> cycleRequests = new List<OBDRequest>();
			OBDRequest obdrequest = new OBDRequest("3101023503", base.GetRequestHeaderForELM327(), "", "", true)
			{
				DoNotDecode = true
			};
			cycleRequests.Add(obdrequest);
			if (array.Length == 0)
			{
				cycleRequests.AddRange((from x in "22ED1E;22E0F1;22ED04;22ED1F;22ED94".Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					select new OBDRequest(x, base.GetRequestHeaderForELM327(), "", "", true)
					{
						DoNotDecode = true
					}).ToArray<OBDRequest>());
			}
			else
			{
				cycleRequests.AddRange(array);
			}
			CodingRequestResult codingRequestResult = await base.Execute(password, value, UserFriendlyValue, progress, originalData, skipIfTheSameData);
			CodingRequestResult codingRequestResult2;
			if (codingRequestResult == CodingRequestResult.Success)
			{
				if (value == "3101023501")
				{
					App.OBDReader.ReplaceQueue(cycleRequests);
				}
				else
				{
					App.OBDReader.ClearRequestQueue();
				}
				codingRequestResult2 = CodingRequestResult.Success;
			}
			else
			{
				codingRequestResult2 = codingRequestResult;
			}
			return codingRequestResult2;
		}

		// Token: 0x06005A98 RID: 23192 RVA: 0x00433BF8 File Offset: 0x00431DF8
		private void Pid_ValueChanged(object sender, PID e)
		{
			StringBuilder stringBuilder = new StringBuilder(this.pids.Count * 6);
			foreach (IPIDFloatValue ipidfloatValue in this.pids)
			{
				stringBuilder.Append(ipidfloatValue.Name);
				stringBuilder.Append(": ");
				stringBuilder.Append(UnitsHelper.GetValue(ipidfloatValue.Value, ipidfloatValue.Units));
				stringBuilder.Append(' ');
				stringBuilder.Append(UnitsHelper.GetCaption(ipidfloatValue.Units));
				stringBuilder.Append("\r\n");
			}
			string text = stringBuilder.ToString();
			base.CurrentState = text;
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x00433CC0 File Offset: 0x00431EC0
		[CompilerGenerated]
		private OBDRequest <Execute>b__5_2(string x)
		{
			return new OBDRequest(x, base.GetRequestHeaderForELM327(), "", "", true)
			{
				DoNotDecode = false
			};
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x00433CE0 File Offset: 0x00431EE0
		[CompilerGenerated]
		private OBDRequest <Execute>b__5_3(string x)
		{
			return new OBDRequest(x, base.GetRequestHeaderForELM327(), "", "", true)
			{
				DoNotDecode = true
			};
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x00433D00 File Offset: 0x00431F00
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<CodingRequestResult> <>n__0(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			return base.Execute(password, value, UserFriendlyValue, progress, originalData, skipIfTheSameData);
		}

		// Token: 0x040038E9 RID: 14569
		private List<IPIDFloatValue> pids = new List<IPIDFloatValue>();

		// Token: 0x02000B9C RID: 2972
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005A9C RID: 23196 RVA: 0x00433D11 File Offset: 0x00431F11
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005A9D RID: 23197 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005A9E RID: 23198 RVA: 0x00433D1D File Offset: 0x00431F1D
			internal bool <Execute>b__5_0(CustomPID x)
			{
				return x.Header == "7E0" && x.Name != null && x.Name.Contains("DPF") && x.Units != UnitsHelper.Units.km;
			}

			// Token: 0x06005A9F RID: 23199 RVA: 0x001C6E3D File Offset: 0x001C503D
			internal string <Execute>b__5_1(CustomPID x)
			{
				return x.Command;
			}

			// Token: 0x040038EA RID: 14570
			public static readonly HyundaiDPFRegen.<>c <>9 = new HyundaiDPFRegen.<>c();

			// Token: 0x040038EB RID: 14571
			public static Func<CustomPID, bool> <>9__5_0;

			// Token: 0x040038EC RID: 14572
			public static Func<CustomPID, string> <>9__5_1;
		}

		// Token: 0x02000B9D RID: 2973
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__5 : IAsyncStateMachine
		{
			// Token: 0x06005AA0 RID: 23200 RVA: 0x00433D5C File Offset: 0x00431F5C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				HyundaiDPFRegen hyundaiDPFRegen = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						List<CustomPID> list = CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.Header == "7E0" && x.Name != null && x.Name.Contains("DPF") && x.Units != UnitsHelper.Units.km).ToList<CustomPID>();
						OBDRequest[] array = (from x in list.Select((CustomPID x) => x.Command).Distinct<string>().ToList<string>()
							select new OBDRequest(x, base.GetRequestHeaderForELM327(), "", "", true)
							{
								DoNotDecode = false
							}).ToArray<OBDRequest>();
						List<IPIDFloatValue>.Enumerator enumerator = hyundaiDPFRegen.pids.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								IPIDFloatValue ipidfloatValue = enumerator.Current;
								ipidfloatValue.ValueChanged -= hyundaiDPFRegen.Pid_ValueChanged;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						hyundaiDPFRegen.pids.Clear();
						hyundaiDPFRegen.pids.AddRange(list);
						List<CustomPID>.Enumerator enumerator2 = list.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								CustomPID customPID = enumerator2.Current;
								customPID.ValueChanged -= hyundaiDPFRegen.Pid_ValueChanged;
								customPID.ValueChanged += hyundaiDPFRegen.Pid_ValueChanged;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						App.OBDReader.ReplaceQueue(array);
						cycleRequests = new List<OBDRequest>();
						OBDRequest obdrequest = new OBDRequest("3101023503", hyundaiDPFRegen.GetRequestHeaderForELM327(), "", "", true)
						{
							DoNotDecode = true
						};
						cycleRequests.Add(obdrequest);
						if (array.Length == 0)
						{
							cycleRequests.AddRange((from x in "22ED1E;22E0F1;22ED04;22ED1F;22ED94".Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
								select new OBDRequest(x, base.GetRequestHeaderForELM327(), "", "", true)
								{
									DoNotDecode = true
								}).ToArray<OBDRequest>());
						}
						else
						{
							cycleRequests.AddRange(array);
						}
						taskAwaiter = hyundaiDPFRegen.<>n__0(password, value, UserFriendlyValue, progress, originalData, skipIfTheSameData).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, HyundaiDPFRegen.<Execute>d__5>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num = (num2 = -1);
					}
					CodingRequestResult result = taskAwaiter.GetResult();
					if (result == CodingRequestResult.Success)
					{
						if (value == "3101023501")
						{
							App.OBDReader.ReplaceQueue(cycleRequests);
						}
						else
						{
							App.OBDReader.ClearRequestQueue();
						}
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						codingRequestResult = result;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					cycleRequests = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				cycleRequests = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005AA1 RID: 23201 RVA: 0x00434080 File Offset: 0x00432280
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040038ED RID: 14573
			public int <>1__state;

			// Token: 0x040038EE RID: 14574
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040038EF RID: 14575
			public HyundaiDPFRegen <>4__this;

			// Token: 0x040038F0 RID: 14576
			public string password;

			// Token: 0x040038F1 RID: 14577
			public string value;

			// Token: 0x040038F2 RID: 14578
			public string UserFriendlyValue;

			// Token: 0x040038F3 RID: 14579
			public IProgress<string> progress;

			// Token: 0x040038F4 RID: 14580
			public byte[] originalData;

			// Token: 0x040038F5 RID: 14581
			public bool skipIfTheSameData;

			// Token: 0x040038F6 RID: 14582
			private List<OBDRequest> <cycleRequests>5__2;

			// Token: 0x040038F7 RID: 14583
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000B9E RID: 2974
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__4 : IAsyncStateMachine
		{
			// Token: 0x06005AA2 RID: 23202 RVA: 0x00434090 File Offset: 0x00432290
			void IAsyncStateMachine.MoveNext()
			{
				CodingRequestResult codingRequestResult;
				try
				{
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005AA3 RID: 23203 RVA: 0x004340DC File Offset: 0x004322DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040038F8 RID: 14584
			public int <>1__state;

			// Token: 0x040038F9 RID: 14585
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;
		}
	}
}
