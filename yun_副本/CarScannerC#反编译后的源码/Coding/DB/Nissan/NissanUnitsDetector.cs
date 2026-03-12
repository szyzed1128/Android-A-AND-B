using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.ECUModels;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Nissan
{
	// Token: 0x02000A72 RID: 2674
	internal class NissanUnitsDetector
	{
		// Token: 0x0600548D RID: 21645 RVA: 0x004034A4 File Offset: 0x004016A4
		private OBDRequest GetOBDRequestForUnit(string cmd, string requestHeader, string responseHeader)
		{
			string text = "ATFCSH" + requestHeader + ";ATFCSD300010;ATFCSM1;ATAL;ATCRA" + responseHeader;
			string text2 = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF";
			return new OBDRequest(cmd, requestHeader, text, text2, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x004034E8 File Offset: 0x004016E8
		public async Task ReadAllUnits(IProgress<string> progress)
		{
			List<KeyValuePair<string, string>> headers = NissanCANECU.ECUs.Select((IECU x) => new KeyValuePair<string, string>(x.RequestHeader, x.ResponseHeader)).ToList<KeyValuePair<string, string>>();
			List<OBDRequest> list = new List<OBDRequest>(headers.Count * 3);
			int progressCounter = 1;
			foreach (KeyValuePair<string, string> keyValuePair in headers)
			{
				try
				{
					string[] cmds = new string[] { "10C0", "2183", "22F150", "22F18A", "22F18B" };
					ResponseReceivedDelegate <>9__1;
					foreach (string text in cmds)
					{
						OBDRequest obdrequestForUnit = this.GetOBDRequestForUnit(text, keyValuePair.Key, keyValuePair.Value);
						obdrequestForUnit.ForceManualFlowControl = false;
						OBDRequest obdrequest = obdrequestForUnit;
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
						list.Add(obdrequestForUnit);
					}
				}
				catch (Exception)
				{
				}
			}
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
		}

		// Token: 0x0600548F RID: 21647 RVA: 0x00002050 File Offset: 0x00000250
		public NissanUnitsDetector()
		{
		}

		// Token: 0x02000A73 RID: 2675
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005490 RID: 21648 RVA: 0x00403533 File Offset: 0x00401733
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005491 RID: 21649 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005492 RID: 21650 RVA: 0x0040353F File Offset: 0x0040173F
			internal KeyValuePair<string, string> <ReadAllUnits>b__1_0(IECU x)
			{
				return new KeyValuePair<string, string>(x.RequestHeader, x.ResponseHeader);
			}

			// Token: 0x040033A0 RID: 13216
			public static readonly NissanUnitsDetector.<>c <>9 = new NissanUnitsDetector.<>c();

			// Token: 0x040033A1 RID: 13217
			public static Func<IECU, KeyValuePair<string, string>> <>9__1_0;
		}

		// Token: 0x02000A74 RID: 2676
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06005493 RID: 21651 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x040033A2 RID: 13218
			public IProgress<string> progress;

			// Token: 0x040033A3 RID: 13219
			public int progressCounter;

			// Token: 0x040033A4 RID: 13220
			public List<KeyValuePair<string, string>> headers;
		}

		// Token: 0x02000A75 RID: 2677
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_1
		{
			// Token: 0x06005494 RID: 21652 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_1()
			{
			}

			// Token: 0x06005495 RID: 21653 RVA: 0x00403554 File Offset: 0x00401754
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

			// Token: 0x040033A5 RID: 13221
			public string[] cmds;

			// Token: 0x040033A6 RID: 13222
			public NissanUnitsDetector.<>c__DisplayClass1_0 CS$<>8__locals1;

			// Token: 0x040033A7 RID: 13223
			public ResponseReceivedDelegate <>9__1;
		}

		// Token: 0x02000A76 RID: 2678
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadAllUnits>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005496 RID: 21654 RVA: 0x004035C8 File Offset: 0x004017C8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				NissanUnitsDetector nissanUnitsDetector = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						NissanUnitsDetector.<>c__DisplayClass1_0 CS$<>8__locals1 = new NissanUnitsDetector.<>c__DisplayClass1_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.headers = NissanCANECU.ECUs.Select((IECU x) => new KeyValuePair<string, string>(x.RequestHeader, x.ResponseHeader)).ToList<KeyValuePair<string, string>>();
						List<OBDRequest> list = new List<OBDRequest>(CS$<>8__locals1.headers.Count * 3);
						CS$<>8__locals1.progressCounter = 1;
						List<KeyValuePair<string, string>>.Enumerator enumerator = CS$<>8__locals1.headers.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair = enumerator.Current;
								try
								{
									NissanUnitsDetector.<>c__DisplayClass1_1 CS$<>8__locals2 = new NissanUnitsDetector.<>c__DisplayClass1_1();
									CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
									CS$<>8__locals2.cmds = new string[] { "10C0", "2183", "22F150", "22F18A", "22F18B" };
									foreach (string text in CS$<>8__locals2.cmds)
									{
										OBDRequest obdrequestForUnit = nissanUnitsDetector.GetOBDRequestForUnit(text, keyValuePair.Key, keyValuePair.Value);
										obdrequestForUnit.ForceManualFlowControl = false;
										OBDRequest obdrequest = obdrequestForUnit;
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
										list.Add(obdrequestForUnit);
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, NissanUnitsDetector.<ReadAllUnits>d__1>(ref taskAwaiter, ref this);
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

			// Token: 0x06005497 RID: 21655 RVA: 0x00403828 File Offset: 0x00401A28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040033A8 RID: 13224
			public int <>1__state;

			// Token: 0x040033A9 RID: 13225
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040033AA RID: 13226
			public IProgress<string> progress;

			// Token: 0x040033AB RID: 13227
			public NissanUnitsDetector <>4__this;

			// Token: 0x040033AC RID: 13228
			private TaskAwaiter <>u__1;
		}
	}
}
