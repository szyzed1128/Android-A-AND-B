using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.Volvo
{
	// Token: 0x020009D5 RID: 2517
	internal class VolvoP3SetTimeDashboard : CustomizableCodingTemplate
	{
		// Token: 0x06005145 RID: 20805 RVA: 0x003F08B4 File Offset: 0x003EEAB4
		public VolvoP3SetTimeDashboard()
		{
			base.MakeChangesToInitialData = false;
			base.Name = "Set current time (Volvo P3)";
			base.Description = "WARNING! This procedure requires ELM327 device with ability to access MS-CAN bus and ST commands support (e.g. OBDLink MX+)";
			base.InnerDescription = "There would be no response from the car, so Car Scanner doesn't actually know, if time was set or not";
			base.Translations.Add(new TranslationItem("ru", "Установка времени (Volvo P3)", "Внимание! Для этой процедуры требуется адаптер ELM327 с возможностью доступа к шине MS-CAN и поддержкой ST команд (например OBDLink MX+)", "Автомобиль никак не отвечает на команду установки времени, поэтому Car Scanner не знает, установлено оно или нет."));
			base.RequestHeader = "201";
			base.ResponseHeader = "209";
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Group = CodingGroup.ServiceProcedures;
			this.Options.Add(new MQBAdaptationOption("Set current time (MS-CAN 11 bit)", "MS11", new TranslationItem[]
			{
				new TranslationItem("ru", "Установить текущее время (MS-CAN 11 bit)", "", "")
			}));
			this.Options.Add(new MQBAdaptationOption("Set current time (HS-CAN 11 bit)", "HS11", new TranslationItem[]
			{
				new TranslationItem("ru", "Установить текущее время (HS-CAN 11 bit)", "", "")
			}));
			this.Options.Add(new MQBAdaptationOption("Set current time (MS-CAN 29 bit)", "MS29", new TranslationItem[]
			{
				new TranslationItem("ru", "Установить текущее время (MS-CAN 29 bit)", "", "")
			}));
			this.Options.Add(new MQBAdaptationOption("Set current time (HS-CAN 29 bit)", "HS29", new TranslationItem[]
			{
				new TranslationItem("ru", "Установить текущее время (HS-CAN 29 bit)", "", "")
			}));
			this.beforePing = SharedSettings.Current.AlwaysPingECU;
			this.ReadModeAndAddress = "";
			this.HasCurrentState = false;
			this.PasswordVisible = false;
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x003F0A54 File Offset: 0x003EEC54
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			return CodingRequestResult.Success;
		}

		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x06005147 RID: 20807 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06005148 RID: 20808 RVA: 0x003F0A8F File Offset: 0x003EEC8F
		public override bool HasCurrentState
		{
			get
			{
				return false;
			}
			set
			{
				base.HasCurrentState = value;
			}
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x003F0A98 File Offset: 0x003EEC98
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			DateTime nowSafe = DateTimeNowHelper.NowSafe;
			int hour = nowSafe.Hour;
			int minute = nowSafe.Minute;
			string text = "201";
			string text2 = "STP53";
			string text3 = "209";
			string text4 = "";
			string text5 = "";
			if (!(value == "MS29"))
			{
				if (!(value == "MS11"))
				{
					if (!(value == "HS29"))
					{
						if (value == "HS11")
						{
							text = "201";
							text2 = "ATSP6";
							text3 = "209";
						}
					}
					else
					{
						text = "000201";
						text2 = "ATSP7";
						text3 = "00000209";
						text4 = "ATCP00";
						text5 = "ATCP18";
					}
				}
				else
				{
					text = "201";
					text2 = "STP53";
					text3 = "209";
				}
			}
			else
			{
				text = "000201";
				text2 = "STP54";
				text3 = "00000209";
				text4 = "ATCP00";
				text5 = "ATCP18";
			}
			if (this.beforePing)
			{
				SharedSettings.Current.AlwaysPingECU = false;
			}
			try
			{
				OBDRequest req = new OBDRequest("000134" + minute.ToString("X2") + hour.ToString("X2") + "000000", text, string.Concat(new string[] { text2, ";ATCAF0;ATCFC0;ATCRA", text3, ";ATSH", text, ";", text4 }), "ATCAF1;ATCFC1;ATAR;ATSPDEF;ATSH" + App.OBDReader.GetDefaultHeader() + ";" + text5, false);
				await App.OBDReader.ClearRequestQueue();
				foreach (string text6 in req.BeforeCommands)
				{
					await App.OBDReader.SendString(text6);
					await App.OBDReader.ReadData(2500, null, -1);
				}
				string[] array = null;
				await App.OBDReader.SendString(req.Command);
				await App.OBDReader.ReadData(2500, null, -1);
				await App.OBDReader.SendString(req.Command);
				await App.OBDReader.ReadData(2500, null, -1);
				await App.OBDReader.SendString(req.Command);
				await App.OBDReader.ReadData(2500, null, -1);
				foreach (string text7 in req.AfterCommands)
				{
					await App.OBDReader.SendString(text7);
					await App.OBDReader.ReadData(2500, null, -1);
				}
				array = null;
				req = null;
			}
			catch (Exception)
			{
			}
			SharedSettings.Current.AlwaysPingECU = this.beforePing;
			return CodingRequestResult.Success;
		}

		// Token: 0x04003144 RID: 12612
		private bool beforePing;

		// Token: 0x020009D6 RID: 2518
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600514A RID: 20810 RVA: 0x003F0AE4 File Offset: 0x003EECE4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VolvoP3SetTimeDashboard volvoP3SetTimeDashboard = this;
				CodingRequestResult codingRequestResult;
				try
				{
					int hour;
					int minute;
					string text;
					string text2;
					string text3;
					string text4;
					string text5;
					if (num > 10)
					{
						DateTime nowSafe = DateTimeNowHelper.NowSafe;
						hour = nowSafe.Hour;
						minute = nowSafe.Minute;
						text = "201";
						text2 = "STP53";
						text3 = "209";
						text4 = "";
						text5 = "";
						string text6 = value;
						if (!(text6 == "MS29"))
						{
							if (!(text6 == "MS11"))
							{
								if (!(text6 == "HS29"))
								{
									if (text6 == "HS11")
									{
										text = "201";
										text2 = "ATSP6";
										text3 = "209";
									}
								}
								else
								{
									text = "000201";
									text2 = "ATSP7";
									text3 = "00000209";
									text4 = "ATCP00";
									text5 = "ATCP18";
								}
							}
							else
							{
								text = "201";
								text2 = "STP53";
								text3 = "209";
							}
						}
						else
						{
							text = "000201";
							text2 = "STP54";
							text3 = "00000209";
							text4 = "ATCP00";
							text5 = "ATCP18";
						}
						if (volvoP3SetTimeDashboard.beforePing)
						{
							SharedSettings.Current.AlwaysPingECU = false;
						}
					}
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
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_02C1;
						}
						case 2:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_032A;
						}
						case 3:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_03C0;
						}
						case 4:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_0429;
						}
						case 5:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0497;
						}
						case 6:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_0500;
						}
						case 7:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_056E;
						}
						case 8:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_05D7;
						}
						case 9:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0669;
						}
						case 10:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_06D3;
						}
						default:
							req = new OBDRequest("000134" + minute.ToString("X2") + hour.ToString("X2") + "000000", text, string.Concat(new string[] { text2, ";ATCAF0;ATCFC0;ATCRA", text3, ";ATSH", text, ";", text4 }), "ATCAF1;ATCFC1;ATAR;ATSPDEF;ATSH" + App.OBDReader.GetDefaultHeader() + ";" + text5, false);
							taskAwaiter = App.OBDReader.ClearRequestQueue().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						taskAwaiter.GetResult();
						array = req.BeforeCommands;
						i = 0;
						goto IL_0340;
						IL_02C1:
						taskAwaiter.GetResult();
						taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_032A:
						taskAwaiter3.GetResult();
						i++;
						IL_0340:
						if (i >= array.Length)
						{
							array = null;
							taskAwaiter = App.OBDReader.SendString(req.Command).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 3;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							string text7 = array[i];
							taskAwaiter = App.OBDReader.SendString(text7).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_02C1;
						}
						IL_03C0:
						taskAwaiter.GetResult();
						taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0429:
						taskAwaiter3.GetResult();
						taskAwaiter = App.OBDReader.SendString(req.Command).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter, ref this);
							return;
						}
						IL_0497:
						taskAwaiter.GetResult();
						taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 6;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0500:
						taskAwaiter3.GetResult();
						taskAwaiter = App.OBDReader.SendString(req.Command).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 7;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter, ref this);
							return;
						}
						IL_056E:
						taskAwaiter.GetResult();
						taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 8;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_05D7:
						taskAwaiter3.GetResult();
						array = req.AfterCommands;
						i = 0;
						goto IL_06E9;
						IL_0669:
						taskAwaiter.GetResult();
						taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 10;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_06D3:
						taskAwaiter3.GetResult();
						i++;
						IL_06E9:
						if (i >= array.Length)
						{
							array = null;
							req = null;
						}
						else
						{
							string text8 = array[i];
							taskAwaiter = App.OBDReader.SendString(text8).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 9;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VolvoP3SetTimeDashboard.<Execute>d__6>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0669;
						}
					}
					catch (Exception)
					{
					}
					SharedSettings.Current.AlwaysPingECU = volvoP3SetTimeDashboard.beforePing;
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x0600514B RID: 20811 RVA: 0x003F1278 File Offset: 0x003EF478
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003145 RID: 12613
			public int <>1__state;

			// Token: 0x04003146 RID: 12614
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003147 RID: 12615
			public string value;

			// Token: 0x04003148 RID: 12616
			public VolvoP3SetTimeDashboard <>4__this;

			// Token: 0x04003149 RID: 12617
			private OBDRequest <req>5__2;

			// Token: 0x0400314A RID: 12618
			private TaskAwaiter <>u__1;

			// Token: 0x0400314B RID: 12619
			private string[] <>7__wrap2;

			// Token: 0x0400314C RID: 12620
			private int <>7__wrap3;

			// Token: 0x0400314D RID: 12621
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x020009D7 RID: 2519
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600514C RID: 20812 RVA: 0x003F1288 File Offset: 0x003EF488
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

			// Token: 0x0600514D RID: 20813 RVA: 0x003F12D4 File Offset: 0x003EF4D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400314E RID: 12622
			public int <>1__state;

			// Token: 0x0400314F RID: 12623
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;
		}
	}
}
