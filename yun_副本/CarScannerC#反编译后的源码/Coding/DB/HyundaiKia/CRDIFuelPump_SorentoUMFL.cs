using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B87 RID: 2951
	internal class CRDIFuelPump_SorentoUMFL : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005A6B RID: 23147 RVA: 0x004323A8 File Offset: 0x004305A8
		public CRDIFuelPump_SorentoUMFL()
		{
			base.Name = "Fuel system air remove (gen.3)";
			base.Description = "Compatibility: Hyundai/KIA with CRDI ~2016 .. ~2018";
			base.InnerDescription = "Use this with ignition turned ON, engine NOT started." + SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.Translations.Add(new TranslationItem("ru", "Удаление воздуха из топливой системы (прокачка топливной системы). Поколение 3", "Совместимость: Hyundai/Kia с дизельными двигателями ~2016 .. ~2018", SupportedItemsDetectorBase.GetAccessKeyWarningRu));
			base.RequestHeader = "7E0";
			base.ResponseHeader = "7E8";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption(Translate.GetString("coding_Start") + " var.1", "01"));
			base.Options.Add(new MQBAdaptationOption(Translate.GetString("coding_Start") + " var.2", "02"));
			base.Options.Add(new MQBAdaptationOption(Translate.GetString("coding_Stop"), "03"));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			base.Group = CodingGroup.EngineAndPowertrain;
			this.HasCurrentState = false;
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x004324F0 File Offset: 0x004306F0
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult result = CodingRequestResult.Success;
			OBDRequest obdrequest = new OBDRequest(this.startPumpCmd, "7E0", "", "", false);
			OBDRequest obdrequest2 = new OBDRequest(this.continuePumpCmd, "7E0", "", "", false);
			OBDRequest obdrequest3 = new OBDRequest("2701", "7E0", "", "", false);
			OBDRequest obdrequest4 = new OBDRequest("2702", "7E0", "", "", false);
			KiaUMCRDI27Generator.SetPasswordDecodeFor2701(obdrequest3);
			obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data == null || data.Contains("NO DATA") || OBDDataReader.FilterHexAndNewLineOnly(data).Contains("037F" + this.continuePumpCmd.Substring(0, 2)))
				{
					result = CodingRequestResult.UnknownError;
					return;
				}
				if (this.stopRequested)
				{
					return;
				}
				if (App.OBDReader.stopwatch.Elapsed > this.targetFinishTime)
				{
					return;
				}
				App.OBDReader.AddRequestToQueue(request);
			};
			CodingRequestResult codingRequestResult;
			if (value == "01" || value == "02")
			{
				if (value == "01")
				{
					this.openSessionCmd = "1081";
				}
				else if (value == "02")
				{
					this.openSessionCmd = "1003";
				}
				OBDRequest obdrequest5 = new OBDRequest(this.openSessionCmd, "7E0", "", "", false);
				this.stopRequested = false;
				OBDRequest[] array = new OBDRequest[] { obdrequest5, obdrequest3, obdrequest4, obdrequest, obdrequest2 };
				this.targetFinishTime = App.OBDReader.stopwatch.Elapsed + TimeSpan.FromSeconds(30.0);
				App.OBDReader.ReplaceQueue(array);
				await App.OBDReader.WaitForCommandQueue();
				codingRequestResult = result;
			}
			else
			{
				if (value == "03")
				{
					this.stopRequested = true;
				}
				codingRequestResult = result;
			}
			return codingRequestResult;
		}

		// Token: 0x040038C1 RID: 14529
		protected volatile bool stopRequested;

		// Token: 0x040038C2 RID: 14530
		protected TimeSpan targetFinishTime = TimeSpan.Zero;

		// Token: 0x040038C3 RID: 14531
		protected string openSessionCmd = "1003";

		// Token: 0x040038C4 RID: 14532
		protected string startPumpCmd = "310102860201";

		// Token: 0x040038C5 RID: 14533
		protected string continuePumpCmd = "3101028601";

		// Token: 0x02000B88 RID: 2952
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06005A6D RID: 23149 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06005A6E RID: 23150 RVA: 0x0043253C File Offset: 0x0043073C
			internal void <Execute>b__0(OBDRequest request, string data)
			{
				if (data == null || data.Contains("NO DATA") || OBDDataReader.FilterHexAndNewLineOnly(data).Contains("037F" + this.<>4__this.continuePumpCmd.Substring(0, 2)))
				{
					this.result = CodingRequestResult.UnknownError;
					return;
				}
				if (this.<>4__this.stopRequested)
				{
					return;
				}
				if (App.OBDReader.stopwatch.Elapsed > this.<>4__this.targetFinishTime)
				{
					return;
				}
				App.OBDReader.AddRequestToQueue(request);
			}

			// Token: 0x040038C6 RID: 14534
			public CRDIFuelPump_SorentoUMFL <>4__this;

			// Token: 0x040038C7 RID: 14535
			public CodingRequestResult result;
		}

		// Token: 0x02000B89 RID: 2953
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__6 : IAsyncStateMachine
		{
			// Token: 0x06005A6F RID: 23151 RVA: 0x004325C8 File Offset: 0x004307C8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CRDIFuelPump_SorentoUMFL crdifuelPump_SorentoUMFL = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new CRDIFuelPump_SorentoUMFL.<>c__DisplayClass6_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.result = CodingRequestResult.Success;
						OBDRequest obdrequest = new OBDRequest(crdifuelPump_SorentoUMFL.startPumpCmd, "7E0", "", "", false);
						OBDRequest obdrequest2 = new OBDRequest(crdifuelPump_SorentoUMFL.continuePumpCmd, "7E0", "", "", false);
						OBDRequest obdrequest3 = new OBDRequest("2701", "7E0", "", "", false);
						OBDRequest obdrequest4 = new OBDRequest("2702", "7E0", "", "", false);
						KiaUMCRDI27Generator.SetPasswordDecodeFor2701(obdrequest3);
						obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data == null || data.Contains("NO DATA") || OBDDataReader.FilterHexAndNewLineOnly(data).Contains("037F" + CS$<>8__locals1.<>4__this.continuePumpCmd.Substring(0, 2)))
							{
								CS$<>8__locals1.result = CodingRequestResult.UnknownError;
								return;
							}
							if (CS$<>8__locals1.<>4__this.stopRequested)
							{
								return;
							}
							if (App.OBDReader.stopwatch.Elapsed > CS$<>8__locals1.<>4__this.targetFinishTime)
							{
								return;
							}
							App.OBDReader.AddRequestToQueue(request);
						};
						if (!(value == "01") && !(value == "02"))
						{
							if (value == "03")
							{
								crdifuelPump_SorentoUMFL.stopRequested = true;
							}
							codingRequestResult = CS$<>8__locals1.result;
							goto IL_0262;
						}
						if (value == "01")
						{
							crdifuelPump_SorentoUMFL.openSessionCmd = "1081";
						}
						else if (value == "02")
						{
							crdifuelPump_SorentoUMFL.openSessionCmd = "1003";
						}
						OBDRequest obdrequest5 = new OBDRequest(crdifuelPump_SorentoUMFL.openSessionCmd, "7E0", "", "", false);
						crdifuelPump_SorentoUMFL.stopRequested = false;
						OBDRequest[] array = new OBDRequest[] { obdrequest5, obdrequest3, obdrequest4, obdrequest, obdrequest2 };
						crdifuelPump_SorentoUMFL.targetFinishTime = App.OBDReader.stopwatch.Elapsed + TimeSpan.FromSeconds(30.0);
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CRDIFuelPump_SorentoUMFL.<Execute>d__6>(ref taskAwaiter, ref this);
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
					codingRequestResult = CS$<>8__locals1.result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0262:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005A70 RID: 23152 RVA: 0x00432870 File Offset: 0x00430A70
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040038C8 RID: 14536
			public int <>1__state;

			// Token: 0x040038C9 RID: 14537
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040038CA RID: 14538
			public CRDIFuelPump_SorentoUMFL <>4__this;

			// Token: 0x040038CB RID: 14539
			public string value;

			// Token: 0x040038CC RID: 14540
			private CRDIFuelPump_SorentoUMFL.<>c__DisplayClass6_0 <>8__1;

			// Token: 0x040038CD RID: 14541
			private TaskAwaiter <>u__1;
		}
	}
}
