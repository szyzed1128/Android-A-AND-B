using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B8B RID: 2955
	internal class CretaTPMSCoding : TPMSCodingBase
	{
		// Token: 0x06005A72 RID: 23154 RVA: 0x00432904 File Offset: 0x00430B04
		public CretaTPMSCoding()
		{
			base.Name = "TPMS variant 2: Sensor IDs";
			base.Description = "Compatibility: Hyundai Creta, but not tested";
			base.MakeChangesToInitialData = false;
			this.ReadModeAndAddress = "22C002";
			base.RequestHeader = "7A0";
			base.ResponseHeader = "7A8";
			base.WriteModeAndAddress = "2E0403";
			this.PasswordVisible = false;
			this.HasCurrentState = true;
			this.ID1Title = "Front left wheel";
			this.ID2Title = "Front right wheel";
			this.ID3Title = "Rear left wheel";
			this.ID4Title = "Rear right wheel";
			this.ID5Visible = false;
			base.OpenSessionCommand = "1003";
			base.PreWriteCommands = "2711;2712";
			base.InnerDescription = "This item is compatible only with units, that doesn't request security access code!";
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x004329C4 File Offset: 0x00430BC4
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.ID1 = "";
			base.ID2 = "";
			base.ID3 = "";
			base.ID4 = "";
			base.ID5 = "";
			byte[] array;
			CodingRequestResult codingRequestResult;
			(await this.GetCurrentStateRawData("")).Deconstruct(out array, out codingRequestResult);
			byte[] array2 = array;
			CodingRequestResult codingRequestResult2;
			if (codingRequestResult == CodingRequestResult.Success && array2 != null && array2.Length >= 20)
			{
				base.ID1 = BitHelpers.ByteArrayToHexString(array2.Skip(4).Take(4).ToArray<byte>());
				base.ID2 = BitHelpers.ByteArrayToHexString(array2.Skip(8).Take(4).ToArray<byte>());
				base.ID4 = BitHelpers.ByteArrayToHexString(array2.Skip(12).Take(4).ToArray<byte>());
				base.ID3 = BitHelpers.ByteArrayToHexString(array2.Skip(16).Take(4).ToArray<byte>());
				codingRequestResult2 = CodingRequestResult.Success;
			}
			else
			{
				codingRequestResult2 = CodingRequestResult.InitialDataIncorrect;
			}
			return codingRequestResult2;
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x00432A08 File Offset: 0x00430C08
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (base.ID1.Length != 8 || base.ID2.Length != 8 || base.ID3.Length != 8 || base.ID4.Length != 8)
			{
				codingRequestResult = CodingRequestResult.WrongInputValue;
			}
			else
			{
				string text = base.ID1 + base.ID2 + base.ID4 + base.ID3;
				codingRequestResult = await this.WriteDataToECU(password, string.Concat(new string[] { "FL:", base.ID1, "; FR:", base.ID2, "; RL: ", base.ID3, "; RR: ", base.ID4 }), progress, null, text);
			}
			return codingRequestResult;
		}

		// Token: 0x02000B8C RID: 2956
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005A75 RID: 23157 RVA: 0x00432A5C File Offset: 0x00430C5C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CretaTPMSCoding cretaTPMSCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (cretaTPMSCoding.ID1.Length != 8 || cretaTPMSCoding.ID2.Length != 8 || cretaTPMSCoding.ID3.Length != 8 || cretaTPMSCoding.ID4.Length != 8)
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_0146;
						}
						string text = cretaTPMSCoding.ID1 + cretaTPMSCoding.ID2 + cretaTPMSCoding.ID4 + cretaTPMSCoding.ID3;
						taskAwaiter = cretaTPMSCoding.WriteDataToECU(password, string.Concat(new string[] { "FL:", cretaTPMSCoding.ID1, "; FR:", cretaTPMSCoding.ID2, "; RL: ", cretaTPMSCoding.ID3, "; RR: ", cretaTPMSCoding.ID4 }), progress, null, text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CretaTPMSCoding.<Execute>d__2>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0146:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005A76 RID: 23158 RVA: 0x00432BE0 File Offset: 0x00430DE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040038CE RID: 14542
			public int <>1__state;

			// Token: 0x040038CF RID: 14543
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040038D0 RID: 14544
			public CretaTPMSCoding <>4__this;

			// Token: 0x040038D1 RID: 14545
			public string password;

			// Token: 0x040038D2 RID: 14546
			public IProgress<string> progress;

			// Token: 0x040038D3 RID: 14547
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000B8D RID: 2957
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005A77 RID: 23159 RVA: 0x00432BF0 File Offset: 0x00430DF0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CretaTPMSCoding cretaTPMSCoding = this;
				CodingRequestResult codingRequestResult2;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						cretaTPMSCoding.ID1 = "";
						cretaTPMSCoding.ID2 = "";
						cretaTPMSCoding.ID3 = "";
						cretaTPMSCoding.ID4 = "";
						cretaTPMSCoding.ID5 = "";
						taskAwaiter = cretaTPMSCoding.GetCurrentStateRawData("").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, CretaTPMSCoding.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
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
					byte[] array;
					CodingRequestResult codingRequestResult;
					taskAwaiter.GetResult().Deconstruct(out array, out codingRequestResult);
					byte[] array2 = array;
					if (codingRequestResult == CodingRequestResult.Success && array2 != null && array2.Length >= 20)
					{
						cretaTPMSCoding.ID1 = BitHelpers.ByteArrayToHexString(array2.Skip(4).Take(4).ToArray<byte>());
						cretaTPMSCoding.ID2 = BitHelpers.ByteArrayToHexString(array2.Skip(8).Take(4).ToArray<byte>());
						cretaTPMSCoding.ID4 = BitHelpers.ByteArrayToHexString(array2.Skip(12).Take(4).ToArray<byte>());
						cretaTPMSCoding.ID3 = BitHelpers.ByteArrayToHexString(array2.Skip(16).Take(4).ToArray<byte>());
						codingRequestResult2 = CodingRequestResult.Success;
					}
					else
					{
						codingRequestResult2 = CodingRequestResult.InitialDataIncorrect;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult2);
			}

			// Token: 0x06005A78 RID: 23160 RVA: 0x00432D90 File Offset: 0x00430F90
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040038D4 RID: 14548
			public int <>1__state;

			// Token: 0x040038D5 RID: 14549
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040038D6 RID: 14550
			public CretaTPMSCoding <>4__this;

			// Token: 0x040038D7 RID: 14551
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
