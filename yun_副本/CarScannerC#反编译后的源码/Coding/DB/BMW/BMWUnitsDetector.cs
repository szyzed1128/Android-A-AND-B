using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.ECUModels;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.BMW
{
	// Token: 0x02000BDB RID: 3035
	internal class BMWUnitsDetector
	{
		// Token: 0x06005B4E RID: 23374 RVA: 0x004387FC File Offset: 0x004369FC
		private OBDRequest GetOBDRequestForUnitWithExtAddress(string cmd, string extenededAddress)
		{
			string text = "6" + extenededAddress;
			string text2;
			string text3;
			if (string.IsNullOrEmpty(extenededAddress))
			{
				text2 = "ATFCSH6F1;ATFCSD300010;ATFCSM1;ATAL;ATCRA" + text;
				text3 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF";
			}
			else
			{
				text2 = string.Concat(new string[] { "ATFCSH6F1;ATFCSD", extenededAddress, "300010;ATFCSM1;ATAL;ATCRA", text, ";ATCEA", extenededAddress, ";ATTAF1" });
				text3 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF";
			}
			return new OBDRequest(cmd, "6F1", text2, text3, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
		}

		// Token: 0x06005B4F RID: 23375 RVA: 0x00438890 File Offset: 0x00436A90
		public async Task ReadAllUnits(IProgress<string> progress)
		{
			List<string> headers = BMWGatewayCANECU.ECUs.Select((IECU x) => x.ExtendedAddress).ToList<string>();
			List<OBDRequest> list = new List<OBDRequest>(headers.Count * 3);
			int progressCounter = 1;
			foreach (string text in headers)
			{
				try
				{
					string[] cmds = new string[] { "1A80", "22F101", "22F150", "22F18A", "22F18B" };
					ResponseReceivedDelegate <>9__1;
					foreach (string text2 in cmds)
					{
						OBDRequest obdrequestForUnitWithExtAddress = this.GetOBDRequestForUnitWithExtAddress(text2, text);
						obdrequestForUnitWithExtAddress.ForceManualFlowControl = false;
						OBDRequest obdrequest = obdrequestForUnitWithExtAddress;
						ResponseReceivedDelegate responseReceivedDelegate;
						if ((responseReceivedDelegate = <>9__1) == null)
						{
							responseReceivedDelegate = (<>9__1 = delegate(OBDRequest req, string response)
							{
								IProgress<string> progress2 = progress;
								if (progress2 != null)
								{
									progress2.Report(string.Format("{0}%", progressCounter * 100 / (headers.Count * cmds.Length)));
								}
								int progressCounter2 = progressCounter;
								progressCounter = progressCounter2 + 1;
							});
						}
						obdrequest.ResponseReceived += responseReceivedDelegate;
						list.Add(obdrequestForUnitWithExtAddress);
					}
				}
				catch (Exception)
				{
				}
			}
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x00002050 File Offset: 0x00000250
		public BMWUnitsDetector()
		{
		}

		// Token: 0x02000BDC RID: 3036
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005B51 RID: 23377 RVA: 0x004388DB File Offset: 0x00436ADB
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005B52 RID: 23378 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005B53 RID: 23379 RVA: 0x004388E7 File Offset: 0x00436AE7
			internal string <ReadAllUnits>b__1_0(IECU x)
			{
				return x.ExtendedAddress;
			}

			// Token: 0x0400399F RID: 14751
			public static readonly BMWUnitsDetector.<>c <>9 = new BMWUnitsDetector.<>c();

			// Token: 0x040039A0 RID: 14752
			public static Func<IECU, string> <>9__1_0;
		}

		// Token: 0x02000BDD RID: 3037
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06005B54 RID: 23380 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x040039A1 RID: 14753
			public IProgress<string> progress;

			// Token: 0x040039A2 RID: 14754
			public int progressCounter;

			// Token: 0x040039A3 RID: 14755
			public List<string> headers;
		}

		// Token: 0x02000BDE RID: 3038
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_1
		{
			// Token: 0x06005B55 RID: 23381 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_1()
			{
			}

			// Token: 0x06005B56 RID: 23382 RVA: 0x004388F0 File Offset: 0x00436AF0
			internal void <ReadAllUnits>b__1(OBDRequest req, string response)
			{
				IProgress<string> progress = this.CS$<>8__locals1.progress;
				if (progress != null)
				{
					progress.Report(string.Format("{0}%", this.CS$<>8__locals1.progressCounter * 100 / (this.CS$<>8__locals1.headers.Count * this.cmds.Length)));
				}
				int progressCounter = this.CS$<>8__locals1.progressCounter;
				this.CS$<>8__locals1.progressCounter = progressCounter + 1;
			}

			// Token: 0x040039A4 RID: 14756
			public string[] cmds;

			// Token: 0x040039A5 RID: 14757
			public BMWUnitsDetector.<>c__DisplayClass1_0 CS$<>8__locals1;

			// Token: 0x040039A6 RID: 14758
			public ResponseReceivedDelegate <>9__1;
		}

		// Token: 0x02000BDF RID: 3039
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadAllUnits>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005B57 RID: 23383 RVA: 0x00438964 File Offset: 0x00436B64
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BMWUnitsDetector bmwunitsDetector = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						BMWUnitsDetector.<>c__DisplayClass1_0 CS$<>8__locals1 = new BMWUnitsDetector.<>c__DisplayClass1_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.headers = BMWGatewayCANECU.ECUs.Select((IECU x) => x.ExtendedAddress).ToList<string>();
						List<OBDRequest> list = new List<OBDRequest>(CS$<>8__locals1.headers.Count * 3);
						CS$<>8__locals1.progressCounter = 1;
						List<string>.Enumerator enumerator = CS$<>8__locals1.headers.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								string text = enumerator.Current;
								try
								{
									BMWUnitsDetector.<>c__DisplayClass1_1 CS$<>8__locals2 = new BMWUnitsDetector.<>c__DisplayClass1_1();
									CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
									CS$<>8__locals2.cmds = new string[] { "1A80", "22F101", "22F150", "22F18A", "22F18B" };
									foreach (string text2 in CS$<>8__locals2.cmds)
									{
										OBDRequest obdrequestForUnitWithExtAddress = bmwunitsDetector.GetOBDRequestForUnitWithExtAddress(text2, text);
										obdrequestForUnitWithExtAddress.ForceManualFlowControl = false;
										OBDRequest obdrequest = obdrequestForUnitWithExtAddress;
										ResponseReceivedDelegate responseReceivedDelegate;
										if ((responseReceivedDelegate = CS$<>8__locals2.<>9__1) == null)
										{
											responseReceivedDelegate = (CS$<>8__locals2.<>9__1 = delegate(OBDRequest req, string response)
											{
												IProgress<string> progress = CS$<>8__locals2.CS$<>8__locals1.progress;
												if (progress != null)
												{
													progress.Report(string.Format("{0}%", CS$<>8__locals2.CS$<>8__locals1.progressCounter * 100 / (CS$<>8__locals2.CS$<>8__locals1.headers.Count * CS$<>8__locals2.cmds.Length)));
												}
												int progressCounter = CS$<>8__locals2.CS$<>8__locals1.progressCounter;
												CS$<>8__locals2.CS$<>8__locals1.progressCounter = progressCounter + 1;
											});
										}
										obdrequest.ResponseReceived += responseReceivedDelegate;
										list.Add(obdrequestForUnitWithExtAddress);
									}
								}
								catch (Exception)
								{
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BMWUnitsDetector.<ReadAllUnits>d__1>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
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

			// Token: 0x06005B58 RID: 23384 RVA: 0x00438BB8 File Offset: 0x00436DB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040039A7 RID: 14759
			public int <>1__state;

			// Token: 0x040039A8 RID: 14760
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040039A9 RID: 14761
			public IProgress<string> progress;

			// Token: 0x040039AA RID: 14762
			public BMWUnitsDetector <>4__this;

			// Token: 0x040039AB RID: 14763
			private TaskAwaiter <>u__1;
		}
	}
}
