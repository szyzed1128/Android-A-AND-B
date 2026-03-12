using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Toyota
{
	// Token: 0x020009E3 RID: 2531
	internal class ToyotaTPMS2ACoding5Wheels : TPMSCodingBase
	{
		// Token: 0x06005187 RID: 20871 RVA: 0x003F2178 File Offset: 0x003F0378
		public ToyotaTPMS2ACoding5Wheels()
		{
			base.Name = "TPMS sensors registration (5 wheels)";
			base.Description = "Experimental feature!";
			base.InnerDescription = "";
			this.RequiresPro = false;
			base.RequestHeader = "750";
			base.ResponseHeader = "758";
			base.ExtendedAddress = "2A";
			base.TesterAddress = "2A";
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "3B0E";
			base.Protocol = "6";
			base.ATST = "96";
			base.PreReadCommands = "";
			base.PreWriteCommands = "105F;211B;2109;2106";
			base.MakeChangesToInitialData = false;
			this.ValueType = AdaptationValueTypes.TPMS;
			this.Group = CodingGroup.TPMS;
			this.ID1Title = "Wheel 1";
			this.ID2Title = "Wheel 2";
			this.ID3Title = "Wheel 3";
			this.ID4Title = "Wheel 4";
			this.ID5Title = "Wheel 5";
			this.ID1Visible = true;
			this.ID2Visible = true;
			this.ID3Visible = true;
			this.ID4Visible = true;
			this.ID5Visible = true;
			this.PasswordVisible = false;
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x003F229C File Offset: 0x003F049C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.ID1 = "";
			base.ID2 = "";
			base.ID3 = "";
			base.ID4 = "";
			base.ID5 = "";
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest("210F", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest2 = new OBDRequest("2110", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest3 = new OBDRequest("2111", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest4 = new OBDRequest("2112", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest5 = new OBDRequest("2113", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseDecoded += this.Get_ResponseDecoded;
			obdrequest2.ResponseDecoded += this.Get_ResponseDecoded;
			obdrequest3.ResponseDecoded += this.Get_ResponseDecoded;
			obdrequest4.ResponseDecoded += this.Get_ResponseDecoded;
			obdrequest5.ResponseDecoded += this.Get_ResponseDecoded;
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3, obdrequest4, obdrequest5 });
			await App.OBDReader.WaitForCommandQueue();
			return CodingRequestResult.Success;
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x003F22E0 File Offset: 0x003F04E0
		private void Get_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data == null || data.Length < 4)
			{
				return;
			}
			string ascii = BitHelpers.ByteArrayToHexString(data);
			while (ascii.Length > 7)
			{
				ascii = ascii.Substring(1);
			}
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				string command = request.Command;
				if (command == "210F")
				{
					this.ID1 = ascii;
					return;
				}
				if (command == "2110")
				{
					this.ID2 = ascii;
					return;
				}
				if (command == "2111")
				{
					this.ID3 = ascii;
					return;
				}
				if (command == "2112")
				{
					this.ID4 = ascii;
					return;
				}
				if (!(command == "2113"))
				{
					return;
				}
				this.ID5 = ascii;
			});
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x003F234C File Offset: 0x003F054C
		private string BuildExecuteCommand()
		{
			IEnumerable<byte> enumerable = new byte[] { 0, 5 };
			byte[] array = BitHelpers.ConvertHexToBytesX(base.ID1);
			byte[] array2 = BitHelpers.ConvertHexToBytesX(base.ID2);
			byte[] array3 = BitHelpers.ConvertHexToBytesX(base.ID3);
			byte[] array4 = BitHelpers.ConvertHexToBytesX(base.ID4);
			byte[] array5 = BitHelpers.ConvertHexToBytesX(base.ID5);
			string text = BitHelpers.ByteArrayToHexString(enumerable.Concat(array).Concat(array2).Concat(array3)
				.Concat(array4)
				.Concat(array5)
				.ToArray<byte>());
			if (base.PreWriteDataProcessor != null)
			{
				try
				{
					byte[] array6 = BitHelpers.ConvertHexToBytesX(text);
					text = BitHelpers.ByteArrayToHexString(base.PreWriteDataProcessor.ProcessData(array6));
				}
				catch (Exception)
				{
				}
			}
			return text;
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x003F2408 File Offset: 0x003F0608
		public override Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			string text = this.BuildExecuteCommand();
			return base.Execute(password, text, UserFriendlyValue, progress, originalData, skipIfTheSameData);
		}

		// Token: 0x020009E4 RID: 2532
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600518C RID: 20876 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x0600518D RID: 20877 RVA: 0x003F242C File Offset: 0x003F062C
			internal void <Get_ResponseDecoded>b__0()
			{
				string command = this.request.Command;
				if (command == "210F")
				{
					this.<>4__this.ID1 = this.ascii;
					return;
				}
				if (command == "2110")
				{
					this.<>4__this.ID2 = this.ascii;
					return;
				}
				if (command == "2111")
				{
					this.<>4__this.ID3 = this.ascii;
					return;
				}
				if (command == "2112")
				{
					this.<>4__this.ID4 = this.ascii;
					return;
				}
				if (!(command == "2113"))
				{
					return;
				}
				this.<>4__this.ID5 = this.ascii;
			}

			// Token: 0x04003172 RID: 12658
			public OBDRequest request;

			// Token: 0x04003173 RID: 12659
			public ToyotaTPMS2ACoding5Wheels <>4__this;

			// Token: 0x04003174 RID: 12660
			public string ascii;
		}

		// Token: 0x020009E5 RID: 2533
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600518E RID: 20878 RVA: 0x003F24E0 File Offset: 0x003F06E0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ToyotaTPMS2ACoding5Wheels toyotaTPMS2ACoding5Wheels = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						toyotaTPMS2ACoding5Wheels.ID1 = "";
						toyotaTPMS2ACoding5Wheels.ID2 = "";
						toyotaTPMS2ACoding5Wheels.ID3 = "";
						toyotaTPMS2ACoding5Wheels.ID4 = "";
						toyotaTPMS2ACoding5Wheels.ID5 = "";
						toyotaTPMS2ACoding5Wheels.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest("210F", toyotaTPMS2ACoding5Wheels.GetRequestHeaderForELM327(), toyotaTPMS2ACoding5Wheels.BeforeCommands, toyotaTPMS2ACoding5Wheels.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("2110", toyotaTPMS2ACoding5Wheels.GetRequestHeaderForELM327(), toyotaTPMS2ACoding5Wheels.BeforeCommands, toyotaTPMS2ACoding5Wheels.AfterCommands, false);
						OBDRequest obdrequest3 = new OBDRequest("2111", toyotaTPMS2ACoding5Wheels.GetRequestHeaderForELM327(), toyotaTPMS2ACoding5Wheels.BeforeCommands, toyotaTPMS2ACoding5Wheels.AfterCommands, false);
						OBDRequest obdrequest4 = new OBDRequest("2112", toyotaTPMS2ACoding5Wheels.GetRequestHeaderForELM327(), toyotaTPMS2ACoding5Wheels.BeforeCommands, toyotaTPMS2ACoding5Wheels.AfterCommands, false);
						OBDRequest obdrequest5 = new OBDRequest("2113", toyotaTPMS2ACoding5Wheels.GetRequestHeaderForELM327(), toyotaTPMS2ACoding5Wheels.BeforeCommands, toyotaTPMS2ACoding5Wheels.AfterCommands, false);
						obdrequest.ResponseDecoded += toyotaTPMS2ACoding5Wheels.Get_ResponseDecoded;
						obdrequest2.ResponseDecoded += toyotaTPMS2ACoding5Wheels.Get_ResponseDecoded;
						obdrequest3.ResponseDecoded += toyotaTPMS2ACoding5Wheels.Get_ResponseDecoded;
						obdrequest4.ResponseDecoded += toyotaTPMS2ACoding5Wheels.Get_ResponseDecoded;
						obdrequest5.ResponseDecoded += toyotaTPMS2ACoding5Wheels.Get_ResponseDecoded;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3, obdrequest4, obdrequest5 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ToyotaTPMS2ACoding5Wheels.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
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

			// Token: 0x0600518F RID: 20879 RVA: 0x003F270C File Offset: 0x003F090C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003175 RID: 12661
			public int <>1__state;

			// Token: 0x04003176 RID: 12662
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003177 RID: 12663
			public ToyotaTPMS2ACoding5Wheels <>4__this;

			// Token: 0x04003178 RID: 12664
			private TaskAwaiter <>u__1;
		}
	}
}
