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
	// Token: 0x020009E0 RID: 2528
	internal class ToyotaTPMS2ACoding : CustomizableCodingTemplate
	{
		// Token: 0x06005176 RID: 20854 RVA: 0x003F1C10 File Offset: 0x003EFE10
		public ToyotaTPMS2ACoding()
		{
			base.Name = "TPMS sensors registration";
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
			this.ValueType = AdaptationValueTypes.ToyotaTPMSSensor;
			this.Group = CodingGroup.TPMS;
		}

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x06005177 RID: 20855 RVA: 0x003F1CFB File Offset: 0x003EFEFB
		// (set) Token: 0x06005178 RID: 20856 RVA: 0x003F1D03 File Offset: 0x003EFF03
		public string ID1
		{
			get
			{
				return this._ID1;
			}
			set
			{
				this._ID1 = value;
				base.OnPropertyChanged("ID1");
			}
		}

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x06005179 RID: 20857 RVA: 0x003F1D17 File Offset: 0x003EFF17
		// (set) Token: 0x0600517A RID: 20858 RVA: 0x003F1D1F File Offset: 0x003EFF1F
		public string ID2
		{
			get
			{
				return this._ID2;
			}
			set
			{
				this._ID2 = value;
				base.OnPropertyChanged("ID2");
			}
		}

		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x0600517B RID: 20859 RVA: 0x003F1D33 File Offset: 0x003EFF33
		// (set) Token: 0x0600517C RID: 20860 RVA: 0x003F1D3B File Offset: 0x003EFF3B
		public string ID3
		{
			get
			{
				return this._ID3;
			}
			set
			{
				this._ID3 = value;
				base.OnPropertyChanged("ID3");
			}
		}

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x0600517D RID: 20861 RVA: 0x003F1D4F File Offset: 0x003EFF4F
		// (set) Token: 0x0600517E RID: 20862 RVA: 0x003F1D57 File Offset: 0x003EFF57
		public string ID4
		{
			get
			{
				return this._ID4;
			}
			set
			{
				this._ID4 = value;
				base.OnPropertyChanged("ID4");
			}
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x003F1D6C File Offset: 0x003EFF6C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.ID1 = "";
			this.ID2 = "";
			this.ID3 = "";
			this.ID4 = "";
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest("210F", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest2 = new OBDRequest("2110", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest3 = new OBDRequest("2111", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest4 = new OBDRequest("2112", base.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseDecoded += this.Get_ResponseDecoded;
			obdrequest2.ResponseDecoded += this.Get_ResponseDecoded;
			obdrequest3.ResponseDecoded += this.Get_ResponseDecoded;
			obdrequest4.ResponseDecoded += this.Get_ResponseDecoded;
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3, obdrequest4 });
			await App.OBDReader.WaitForCommandQueue();
			return CodingRequestResult.Success;
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x003F1DB0 File Offset: 0x003EFFB0
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
				if (!(command == "2112"))
				{
					return;
				}
				this.ID4 = ascii;
			});
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x003F1E1C File Offset: 0x003F001C
		private string BuildExecuteCommand()
		{
			IEnumerable<byte> enumerable = new byte[] { 0, 4 };
			byte[] array = BitHelpers.ConvertHexToBytesX(this.ID1);
			byte[] array2 = BitHelpers.ConvertHexToBytesX(this.ID2);
			byte[] array3 = BitHelpers.ConvertHexToBytesX(this.ID3);
			byte[] array4 = BitHelpers.ConvertHexToBytesX(this.ID4);
			string text = BitHelpers.ByteArrayToHexString(enumerable.Concat(array).Concat(array2).Concat(array3)
				.Concat(array4)
				.ToArray<byte>());
			if (base.PreWriteDataProcessor != null)
			{
				try
				{
					byte[] array5 = BitHelpers.ConvertHexToBytesX(text);
					text = BitHelpers.ByteArrayToHexString(base.PreWriteDataProcessor.ProcessData(array5));
				}
				catch (Exception)
				{
				}
			}
			return text;
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x003F1EC4 File Offset: 0x003F00C4
		public override Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			string text = this.BuildExecuteCommand();
			return base.Execute(password, text, UserFriendlyValue, progress, originalData, skipIfTheSameData);
		}

		// Token: 0x04003167 RID: 12647
		private string _ID1 = "";

		// Token: 0x04003168 RID: 12648
		private string _ID2 = "";

		// Token: 0x04003169 RID: 12649
		private string _ID3 = "";

		// Token: 0x0400316A RID: 12650
		private string _ID4 = "";

		// Token: 0x020009E1 RID: 2529
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x06005183 RID: 20867 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x06005184 RID: 20868 RVA: 0x003F1EE8 File Offset: 0x003F00E8
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
				if (!(command == "2112"))
				{
					return;
				}
				this.<>4__this.ID4 = this.ascii;
			}

			// Token: 0x0400316B RID: 12651
			public OBDRequest request;

			// Token: 0x0400316C RID: 12652
			public ToyotaTPMS2ACoding <>4__this;

			// Token: 0x0400316D RID: 12653
			public string ascii;
		}

		// Token: 0x020009E2 RID: 2530
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__17 : IAsyncStateMachine
		{
			// Token: 0x06005185 RID: 20869 RVA: 0x003F1F80 File Offset: 0x003F0180
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ToyotaTPMS2ACoding toyotaTPMS2ACoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						toyotaTPMS2ACoding.ID1 = "";
						toyotaTPMS2ACoding.ID2 = "";
						toyotaTPMS2ACoding.ID3 = "";
						toyotaTPMS2ACoding.ID4 = "";
						toyotaTPMS2ACoding.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest("210F", toyotaTPMS2ACoding.GetRequestHeaderForELM327(), toyotaTPMS2ACoding.BeforeCommands, toyotaTPMS2ACoding.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("2110", toyotaTPMS2ACoding.GetRequestHeaderForELM327(), toyotaTPMS2ACoding.BeforeCommands, toyotaTPMS2ACoding.AfterCommands, false);
						OBDRequest obdrequest3 = new OBDRequest("2111", toyotaTPMS2ACoding.GetRequestHeaderForELM327(), toyotaTPMS2ACoding.BeforeCommands, toyotaTPMS2ACoding.AfterCommands, false);
						OBDRequest obdrequest4 = new OBDRequest("2112", toyotaTPMS2ACoding.GetRequestHeaderForELM327(), toyotaTPMS2ACoding.BeforeCommands, toyotaTPMS2ACoding.AfterCommands, false);
						obdrequest.ResponseDecoded += toyotaTPMS2ACoding.Get_ResponseDecoded;
						obdrequest2.ResponseDecoded += toyotaTPMS2ACoding.Get_ResponseDecoded;
						obdrequest3.ResponseDecoded += toyotaTPMS2ACoding.Get_ResponseDecoded;
						obdrequest4.ResponseDecoded += toyotaTPMS2ACoding.Get_ResponseDecoded;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3, obdrequest4 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ToyotaTPMS2ACoding.<UpdateCurrentState>d__17>(ref taskAwaiter, ref this);
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

			// Token: 0x06005186 RID: 20870 RVA: 0x003F2168 File Offset: 0x003F0368
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400316E RID: 12654
			public int <>1__state;

			// Token: 0x0400316F RID: 12655
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003170 RID: 12656
			public ToyotaTPMS2ACoding <>4__this;

			// Token: 0x04003171 RID: 12657
			private TaskAwaiter <>u__1;
		}
	}
}
