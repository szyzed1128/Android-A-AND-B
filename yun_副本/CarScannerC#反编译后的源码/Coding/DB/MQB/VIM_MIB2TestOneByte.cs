using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B6E RID: 2926
	internal class VIM_MIB2TestOneByte : VIM_MIB2
	{
		// Token: 0x06005A00 RID: 23040 RVA: 0x0042F3D6 File Offset: 0x0042D5D6
		public VIM_MIB2TestOneByte(int byteIdx)
		{
			this.byteIdx = byteIdx;
			base.Name = "MIB2 HMI Byte " + byteIdx.ToString("00");
			base.Translations.Clear();
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x0042F40C File Offset: 0x0042D60C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			if (string.IsNullOrEmpty(password))
			{
				password = this.Password;
			}
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password, progress);
			CodingRequestResult item = tuple.Item2;
			byte[] item2 = tuple.Item1;
			CodingRequestResult codingRequestResult;
			if (item != CodingRequestResult.Success)
			{
				base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
				codingRequestResult = item;
			}
			else
			{
				this.LastReadData = item2;
				if (item2 != null)
				{
					base.CurrentState = item2[this.byteIdx].ToString(CultureInfo.InvariantCulture);
				}
				codingRequestResult = item;
			}
			return codingRequestResult;
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x0042F460 File Offset: 0x0042D660
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (this.LastReadData == null)
			{
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				byte speed = 0;
				if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out speed))
				{
					codingRequestResult = CodingRequestResult.WrongInputValue;
				}
				else
				{
					OBDRequest obdrequest = new OBDRequest("010C", false);
					bool engineIsRunning = false;
					obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
						{
							engineIsRunning = true;
						}
					};
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
					await App.OBDReader.WaitForCommandQueue();
					if (engineIsRunning)
					{
						codingRequestResult = CodingRequestResult.WrongConditions;
					}
					else
					{
						byte[] array = new byte[this.LastReadData.Length - 2];
						Array.Copy(this.LastReadData, 0, array, 0, array.Length);
						array[this.byteIdx] = speed;
						byte[] array2 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array);
						string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
						codingRequestResult = await this.WriteDataToECU(password, value, progress, this.LastReadData, text);
					}
				}
			}
			return codingRequestResult;
		}

		// Token: 0x04003871 RID: 14449
		private int byteIdx;

		// Token: 0x02000B6F RID: 2927
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06005A03 RID: 23043 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005A04 RID: 23044 RVA: 0x0042F4BC File Offset: 0x0042D6BC
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x04003872 RID: 14450
			public bool engineIsRunning;
		}

		// Token: 0x02000B70 RID: 2928
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005A05 RID: 23045 RVA: 0x0042F4E0 File Offset: 0x0042D6E0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VIM_MIB2TestOneByte vim_MIB2TestOneByte = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
							goto IL_01E0;
						}
						CS$<>8__locals1 = new VIM_MIB2TestOneByte.<>c__DisplayClass3_0();
						if (vim_MIB2TestOneByte.LastReadData == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_020A;
						}
						speed = 0;
						if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out speed))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_020A;
						}
						OBDRequest obdrequest = new OBDRequest("010C", false);
						CS$<>8__locals1.engineIsRunning = false;
						obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
							{
								CS$<>8__locals1.engineIsRunning = true;
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VIM_MIB2TestOneByte.<Execute>d__3>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					if (CS$<>8__locals1.engineIsRunning)
					{
						codingRequestResult = CodingRequestResult.WrongConditions;
						goto IL_020A;
					}
					byte[] array = new byte[vim_MIB2TestOneByte.LastReadData.Length - 2];
					Array.Copy(vim_MIB2TestOneByte.LastReadData, 0, array, 0, array.Length);
					array[vim_MIB2TestOneByte.byteIdx] = speed;
					byte[] array2 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					taskAwaiter = vim_MIB2TestOneByte.WriteDataToECU(password, value, progress, vim_MIB2TestOneByte.LastReadData, text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VIM_MIB2TestOneByte.<Execute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_01E0:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_020A:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005A06 RID: 23046 RVA: 0x0042F730 File Offset: 0x0042D930
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003873 RID: 14451
			public int <>1__state;

			// Token: 0x04003874 RID: 14452
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003875 RID: 14453
			public VIM_MIB2TestOneByte <>4__this;

			// Token: 0x04003876 RID: 14454
			public string value;

			// Token: 0x04003877 RID: 14455
			private VIM_MIB2TestOneByte.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04003878 RID: 14456
			public string password;

			// Token: 0x04003879 RID: 14457
			public IProgress<string> progress;

			// Token: 0x0400387A RID: 14458
			private byte <speed>5__2;

			// Token: 0x0400387B RID: 14459
			private TaskAwaiter <>u__1;

			// Token: 0x0400387C RID: 14460
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000B71 RID: 2929
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005A07 RID: 23047 RVA: 0x0042F740 File Offset: 0x0042D940
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VIM_MIB2TestOneByte vim_MIB2TestOneByte = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(password))
						{
							password = vim_MIB2TestOneByte.Password;
						}
						taskAwaiter = vim_MIB2TestOneByte.GetCurrentStateRawData(password, progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, VIM_MIB2TestOneByte.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
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
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						vim_MIB2TestOneByte.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						vim_MIB2TestOneByte.LastReadData = item2;
						if (item2 != null)
						{
							vim_MIB2TestOneByte.CurrentState = item2[vim_MIB2TestOneByte.byteIdx].ToString(CultureInfo.InvariantCulture);
						}
						codingRequestResult = item;
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

			// Token: 0x06005A08 RID: 23048 RVA: 0x0042F86C File Offset: 0x0042DA6C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400387D RID: 14461
			public int <>1__state;

			// Token: 0x0400387E RID: 14462
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400387F RID: 14463
			public string password;

			// Token: 0x04003880 RID: 14464
			public VIM_MIB2TestOneByte <>4__this;

			// Token: 0x04003881 RID: 14465
			public IProgress<string> progress;

			// Token: 0x04003882 RID: 14466
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
