using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B1E RID: 2846
	internal class MQBSwapActivation : CustomizableCodingTemplate
	{
		// Token: 0x0600586B RID: 22635 RVA: 0x004232AC File Offset: 0x004214AC
		public MQBSwapActivation()
		{
			base.Name = "SWaP code: transfer code";
			base.Description = "You shoould have VALID SWaP code or unlocked MIB.";
			base.Translations.Add(new TranslationItem("ru", "SWaP code: запись кода", "Вам потребуется действительный SWaP код, либо взломанная мультимедийная система, принимающая недействительные коды.", ""));
			base.RequestHeader = "773";
			base.ResponseHeader = "7DD";
			this.PasswordVisible = true;
			this.Password = "20103";
			base.PasswordHint = "20103";
			base.MakeChangesToInitialData = false;
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "2E3C01";
			base.OpenSessionCommand = "1040";
			base.PreWriteCommands = "22F198;22F199;22F1A0;22F1A1;2703;2704;2EF198;2EF199;2EF1A0;2EF1A1";
			base.PostWriteCommands = "3101C001";
			this.ValueType = AdaptationValueTypes.InputTextType;
			this.Group = CodingGroup.Multimedia;
			this.HasCurrentState = true;
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x004233A4 File Offset: 0x004215A4
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.vinHex = "";
			this.vcrnHex = "";
			this.vinASCII = "";
			OBDRequest obdrequest = new OBDRequest("0902", false);
			obdrequest.ResponseDecoded += this.VinReq_ResponseDecoded;
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest2 = new OBDRequest("223C0A", "773", this.BeforeCommands + ";ATSTFE", this.AfterCommands + ";ATSTDEF", false);
			obdrequest2.ResponseDecoded += this.VcrnReq_ResponseDecoded;
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
			await App.OBDReader.WaitForCommandQueue();
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(this.vinHex) && string.IsNullOrEmpty(this.vcrnHex))
			{
				base.CurrentState = "VIN=" + this.vinASCII + " VCRN=" + this.vcrnHex;
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				codingRequestResult = CodingRequestResult.UnknownError;
			}
			return codingRequestResult;
		}

		// Token: 0x0600586D RID: 22637 RVA: 0x004233E7 File Offset: 0x004215E7
		private void VcrnReq_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				this.vcrnHex = BitHelpers.ByteArrayToHexString(data);
			}
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x004233FC File Offset: 0x004215FC
		private void VinReq_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				this.vinHex = BitHelpers.ByteArrayToHexString(data.Skip(1).ToArray<byte>());
				this.vinASCII = Encoding.ASCII.GetString(data, 1, data.Length - 1);
			}
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x00423434 File Offset: 0x00421634
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(value))
			{
				codingRequestResult = CodingRequestResult.WrongInputValue;
			}
			else
			{
				string checked_value = value.Trim().Replace(" ", "");
				TaskAwaiter<bool> taskAwaiter2;
				if (!string.IsNullOrEmpty(this.vinHex) && !this.IsVINCorrect(checked_value))
				{
					TaskAwaiter<bool> taskAwaiter = App.GetCurrentPage().DisplayAlert("WARNING!", "Your SWaP code contains VIN code, that is different from your car VIN.\nWould you like Car Scanner to put your car VIN code in the SWaP code?\nFixed SWaP code:\n" + this.ApplyVINToSWAP(checked_value), "Yes", "No, continue with my code").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						checked_value = this.ApplyVINToSWAP(checked_value);
					}
				}
				if (!string.IsNullOrEmpty(this.vcrnHex) && !this.IsVCRNCorrect(checked_value))
				{
					TaskAwaiter<bool> taskAwaiter = App.GetCurrentPage().DisplayAlert("WARNING!", "Your SWaP code contains VCRN code, that is different from your MMI unit VCRN.\nWould you like Car Scanner to put correct VCRN code in the SWaP code?\nFixed SWaP code:\n" + this.ApplyVCRNToSWAP(checked_value), "Yes", "No, continue with my code").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						checked_value = this.ApplyVCRNToSWAP(checked_value);
					}
				}
				try
				{
					BitHelpers.ConvertHexToBytesX(checked_value);
				}
				catch (Exception)
				{
					return CodingRequestResult.WrongInputValue;
				}
				codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value);
			}
			return codingRequestResult;
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x004234A4 File Offset: 0x004216A4
		private string ApplyVCRNToSWAP(string swapcode)
		{
			string text3;
			try
			{
				string text = swapcode.Substring(0, 14);
				string text2 = swapcode.Substring(24);
				text3 = text + this.vcrnHex + text2;
			}
			catch (Exception)
			{
				text3 = swapcode;
			}
			return text3;
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x004234E8 File Offset: 0x004216E8
		private bool IsVCRNCorrect(string swapcode)
		{
			return this.ApplyVCRNToSWAP(swapcode).Equals(swapcode);
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x004234FC File Offset: 0x004216FC
		private string ApplyVINToSWAP(string swapcode)
		{
			string text3;
			try
			{
				string text = swapcode.Substring(0, 24);
				string text2 = swapcode.Substring(58);
				text3 = text + this.vinHex + text2;
			}
			catch (Exception)
			{
				text3 = swapcode;
			}
			return text3;
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x00423540 File Offset: 0x00421740
		private bool IsVINCorrect(string swapcode)
		{
			return this.ApplyVINToSWAP(swapcode).Equals(swapcode);
		}

		// Token: 0x040036FF RID: 14079
		private string vinHex = "";

		// Token: 0x04003700 RID: 14080
		private string vcrnHex = "";

		// Token: 0x04003701 RID: 14081
		private string vinASCII = "";

		// Token: 0x02000B1F RID: 2847
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__7 : IAsyncStateMachine
		{
			// Token: 0x06005874 RID: 22644 RVA: 0x00423554 File Offset: 0x00421754
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBSwapActivation mqbswapActivation = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter<CodingRequestResult> taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01AC;
					case 2:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0251;
					}
					default:
						if (string.IsNullOrEmpty(value))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_027B;
						}
						checked_value = value.Trim().Replace(" ", "");
						if (string.IsNullOrEmpty(mqbswapActivation.vinHex) || mqbswapActivation.IsVINCorrect(checked_value))
						{
							goto IL_010E;
						}
						taskAwaiter3 = App.GetCurrentPage().DisplayAlert("WARNING!", "Your SWaP code contains VIN code, that is different from your car VIN.\nWould you like Car Scanner to put your car VIN code in the SWaP code?\nFixed SWaP code:\n" + mqbswapActivation.ApplyVINToSWAP(checked_value), "Yes", "No, continue with my code").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MQBSwapActivation.<Execute>d__7>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (taskAwaiter3.GetResult())
					{
						checked_value = mqbswapActivation.ApplyVINToSWAP(checked_value);
					}
					IL_010E:
					if (string.IsNullOrEmpty(mqbswapActivation.vcrnHex) || mqbswapActivation.IsVCRNCorrect(checked_value))
					{
						goto IL_01C7;
					}
					taskAwaiter3 = App.GetCurrentPage().DisplayAlert("WARNING!", "Your SWaP code contains VCRN code, that is different from your MMI unit VCRN.\nWould you like Car Scanner to put correct VCRN code in the SWaP code?\nFixed SWaP code:\n" + mqbswapActivation.ApplyVCRNToSWAP(checked_value), "Yes", "No, continue with my code").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MQBSwapActivation.<Execute>d__7>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01AC:
					if (taskAwaiter3.GetResult())
					{
						checked_value = mqbswapActivation.ApplyVCRNToSWAP(checked_value);
					}
					IL_01C7:
					try
					{
						BitHelpers.ConvertHexToBytesX(checked_value);
					}
					catch (Exception)
					{
						codingRequestResult = CodingRequestResult.WrongInputValue;
						goto IL_027B;
					}
					taskAwaiter4 = mqbswapActivation.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<CodingRequestResult> taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBSwapActivation.<Execute>d__7>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0251:
					codingRequestResult = taskAwaiter4.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					checked_value = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_027B:
				num2 = -2;
				checked_value = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005875 RID: 22645 RVA: 0x0042382C File Offset: 0x00421A2C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003702 RID: 14082
			public int <>1__state;

			// Token: 0x04003703 RID: 14083
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003704 RID: 14084
			public string value;

			// Token: 0x04003705 RID: 14085
			public MQBSwapActivation <>4__this;

			// Token: 0x04003706 RID: 14086
			public string password;

			// Token: 0x04003707 RID: 14087
			public string UserFriendlyValue;

			// Token: 0x04003708 RID: 14088
			public IProgress<string> progress;

			// Token: 0x04003709 RID: 14089
			public byte[] originalData;

			// Token: 0x0400370A RID: 14090
			private string <checked_value>5__2;

			// Token: 0x0400370B RID: 14091
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x0400370C RID: 14092
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000B20 RID: 2848
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__4 : IAsyncStateMachine
		{
			// Token: 0x06005876 RID: 22646 RVA: 0x0042383C File Offset: 0x00421A3C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBSwapActivation mqbswapActivation = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						mqbswapActivation.vinHex = "";
						mqbswapActivation.vcrnHex = "";
						mqbswapActivation.vinASCII = "";
						OBDRequest obdrequest = new OBDRequest("0902", false);
						obdrequest.ResponseDecoded += mqbswapActivation.VinReq_ResponseDecoded;
						mqbswapActivation.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest2 = new OBDRequest("223C0A", "773", mqbswapActivation.BeforeCommands + ";ATSTFE", mqbswapActivation.AfterCommands + ";ATSTDEF", false);
						obdrequest2.ResponseDecoded += mqbswapActivation.VcrnReq_ResponseDecoded;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBSwapActivation.<UpdateCurrentState>d__4>(ref taskAwaiter, ref this);
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
					if (string.IsNullOrEmpty(mqbswapActivation.vinHex) && string.IsNullOrEmpty(mqbswapActivation.vcrnHex))
					{
						mqbswapActivation.CurrentState = "VIN=" + mqbswapActivation.vinASCII + " VCRN=" + mqbswapActivation.vcrnHex;
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						codingRequestResult = CodingRequestResult.UnknownError;
					}
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

			// Token: 0x06005877 RID: 22647 RVA: 0x004239F0 File Offset: 0x00421BF0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400370D RID: 14093
			public int <>1__state;

			// Token: 0x0400370E RID: 14094
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400370F RID: 14095
			public MQBSwapActivation <>4__this;

			// Token: 0x04003710 RID: 14096
			private TaskAwaiter <>u__1;
		}
	}
}
