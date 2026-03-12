using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.Coding.DB.Nissan
{
	// Token: 0x02000A6E RID: 2670
	internal class NissanLeafBattery : CustomizableCodingTemplate
	{
		// Token: 0x06005477 RID: 21623 RVA: 0x00402D14 File Offset: 0x00400F14
		public NissanLeafBattery()
		{
			base.Name = Translate.GetString("codingDB_NissanLeafBatteryPairing_Name");
			base.InnerDescription = Translate.GetString("codingDB_NissanLeafBatteryPairing_InnerDescription");
			this.ValueType = AdaptationValueTypes.OptionType;
			base.MakeChangesToInitialData = false;
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "3B1F";
			base.PreWriteCommands = "10C0";
			base.PostWriteCommands = "ATFCSH79B;ATFCSD300000;ATFCSM1;ATCRA7BB;2101";
			this.HasCurrentState = true;
			base.RequestHeader = "797";
			base.ResponseHeader = "79A";
			this.PasswordVisible = false;
			base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
			this.Group = CodingGroup.Battery;
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x06005478 RID: 21624 RVA: 0x00402DD5 File Offset: 0x00400FD5
		// (set) Token: 0x06005479 RID: 21625 RVA: 0x00402DDD File Offset: 0x00400FDD
		public override bool HasCurrentState
		{
			get
			{
				return this._HasCurrentState;
			}
			set
			{
				this._HasCurrentState = value;
				base.OnPropertyChanged("HasCurrentState");
			}
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x00402DF4 File Offset: 0x00400FF4
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.Battery797 = "";
			this.Battery79B = "";
			OBDRequest obdrequest = new OBDRequest("2190", "79B", "ATFCSH79B;ATFCSD300000;ATFCSM1;ATCRA7BB;10C0", "ATAR", false)
			{
				ELMFormat = ELMFormat.CAN11bit,
				CheckLength = true
			};
			obdrequest.ResponseDecoded += this.Req79b_ResponseDecoded;
			OBDRequest obdrequest2 = new OBDRequest("22125C", "797", "ATFCSH797;ATFCSD300000;ATFCSM1;ATCRA79A;10C0", "ATAR", false)
			{
				ELMFormat = ELMFormat.CAN11bit,
				CheckLength = true
			};
			obdrequest2.ResponseDecoded += this.Req797_ResponseDecoded;
			OBDRequest obdrequest3 = new OBDRequest("2101", "79B", "ATFCSH79B;ATFCSD300000;ATFCSM1;ATCRA7BB;10C0", "", false);
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, obdrequest, obdrequest3 });
			await App.OBDReader.WaitForCommandQueue();
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(this.Battery79B) || string.IsNullOrEmpty(this.Battery797))
			{
				base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
				codingRequestResult = CodingRequestResult.UnknownError;
			}
			else
			{
				await MainThread.InvokeOnMainThreadAsync(delegate
				{
					this.Options.Clear();
					this.Options.Add(new MQBAdaptationOption(Translate.GetString("codingDB_opt_Start"), BitHelpers.ByteArrayToHexString(Encoding.ASCII.GetBytes(this.Battery79B))));
					string text = "\nV=" + this.Battery797 + "\nB=" + this.Battery79B;
					if (this.Battery797 == this.Battery79B)
					{
						text = text + "\n" + Translate.GetString("codingDB_PairingStatus") + " OK";
					}
					else
					{
						text = string.Concat(new string[]
						{
							text,
							"\n",
							Translate.GetString("codingDB_PairingStatus"),
							" ",
							Translate.GetString("codingDB_PairingStatus_NotPaired")
						});
					}
					base.CurrentState = text;
				});
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x0600547B RID: 21627 RVA: 0x00402E37 File Offset: 0x00401037
		private void Req79b_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			this.Battery79B = Encoding.ASCII.GetString(data);
		}

		// Token: 0x0600547C RID: 21628 RVA: 0x00402E4A File Offset: 0x0040104A
		private void Req797_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			this.Battery797 = Encoding.ASCII.GetString(data);
		}

		// Token: 0x0600547D RID: 21629 RVA: 0x00402E60 File Offset: 0x00401060
		[CompilerGenerated]
		private void <UpdateCurrentState>b__7_0()
		{
			this.Options.Clear();
			this.Options.Add(new MQBAdaptationOption(Translate.GetString("codingDB_opt_Start"), BitHelpers.ByteArrayToHexString(Encoding.ASCII.GetBytes(this.Battery79B))));
			string text = "\nV=" + this.Battery797 + "\nB=" + this.Battery79B;
			if (this.Battery797 == this.Battery79B)
			{
				text = text + "\n" + Translate.GetString("codingDB_PairingStatus") + " OK";
			}
			else
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					Translate.GetString("codingDB_PairingStatus"),
					" ",
					Translate.GetString("codingDB_PairingStatus_NotPaired")
				});
			}
			base.CurrentState = text;
		}

		// Token: 0x04003395 RID: 13205
		private string Battery79B = "";

		// Token: 0x04003396 RID: 13206
		private string Battery797 = "";

		// Token: 0x04003397 RID: 13207
		private bool _HasCurrentState = true;

		// Token: 0x02000A6F RID: 2671
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__7 : IAsyncStateMachine
		{
			// Token: 0x0600547E RID: 21630 RVA: 0x00402F34 File Offset: 0x00401134
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				NissanLeafBattery nissanLeafBattery = this;
				CodingRequestResult codingRequestResult;
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
							goto IL_01D0;
						}
						nissanLeafBattery.Battery797 = "";
						nissanLeafBattery.Battery79B = "";
						OBDRequest obdrequest = new OBDRequest("2190", "79B", "ATFCSH79B;ATFCSD300000;ATFCSM1;ATCRA7BB;10C0", "ATAR", false)
						{
							ELMFormat = ELMFormat.CAN11bit,
							CheckLength = true
						};
						obdrequest.ResponseDecoded += nissanLeafBattery.Req79b_ResponseDecoded;
						OBDRequest obdrequest2 = new OBDRequest("22125C", "797", "ATFCSH797;ATFCSD300000;ATFCSM1;ATCRA79A;10C0", "ATAR", false)
						{
							ELMFormat = ELMFormat.CAN11bit,
							CheckLength = true
						};
						obdrequest2.ResponseDecoded += nissanLeafBattery.Req797_ResponseDecoded;
						OBDRequest obdrequest3 = new OBDRequest("2101", "79B", "ATFCSH79B;ATFCSD300000;ATFCSM1;ATCRA7BB;10C0", "", false);
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, obdrequest, obdrequest3 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, NissanLeafBattery.<UpdateCurrentState>d__7>(ref taskAwaiter, ref this);
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
					if (string.IsNullOrEmpty(nissanLeafBattery.Battery79B) || string.IsNullOrEmpty(nissanLeafBattery.Battery797))
					{
						nissanLeafBattery.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						codingRequestResult = CodingRequestResult.UnknownError;
						goto IL_01F4;
					}
					taskAwaiter = MainThread.InvokeOnMainThreadAsync(delegate
					{
						nissanLeafBattery.Options.Clear();
						nissanLeafBattery.Options.Add(new MQBAdaptationOption(Translate.GetString("codingDB_opt_Start"), BitHelpers.ByteArrayToHexString(Encoding.ASCII.GetBytes(nissanLeafBattery.Battery79B))));
						string text = "\nV=" + nissanLeafBattery.Battery797 + "\nB=" + nissanLeafBattery.Battery79B;
						if (nissanLeafBattery.Battery797 == nissanLeafBattery.Battery79B)
						{
							text = text + "\n" + Translate.GetString("codingDB_PairingStatus") + " OK";
						}
						else
						{
							text = string.Concat(new string[]
							{
								text,
								"\n",
								Translate.GetString("codingDB_PairingStatus"),
								" ",
								Translate.GetString("codingDB_PairingStatus_NotPaired")
							});
						}
						base.CurrentState = text;
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, NissanLeafBattery.<UpdateCurrentState>d__7>(ref taskAwaiter, ref this);
						return;
					}
					IL_01D0:
					taskAwaiter.GetResult();
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01F4:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x0600547F RID: 21631 RVA: 0x00403168 File Offset: 0x00401368
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003398 RID: 13208
			public int <>1__state;

			// Token: 0x04003399 RID: 13209
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400339A RID: 13210
			public NissanLeafBattery <>4__this;

			// Token: 0x0400339B RID: 13211
			private TaskAwaiter <>u__1;
		}
	}
}
