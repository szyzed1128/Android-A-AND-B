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
	// Token: 0x02000AFA RID: 2810
	internal class MIM_MIB2 : VIM_MIB2
	{
		// Token: 0x060057F3 RID: 22515 RVA: 0x0041F32C File Offset: 0x0041D52C
		public MIM_MIB2()
		{
			base.Name = Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib2_Name");
			base.Description = Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib2_Description");
			base.InnerDescription = Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib2_InnerDescription");
			base.Translations.Clear();
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x0041F37C File Offset: 0x0041D57C
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
					byte b = item2[18];
					base.CurrentState = b.ToString(CultureInfo.InvariantCulture);
				}
				codingRequestResult = item;
			}
			return codingRequestResult;
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x0041F3D0 File Offset: 0x0041D5D0
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
						array[18] = speed;
						byte[] array2 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array);
						string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
						codingRequestResult = await this.WriteDataToECU(password, value, progress, this.LastReadData, text);
					}
				}
			}
			return codingRequestResult;
		}

		// Token: 0x02000AFB RID: 2811
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060057F6 RID: 22518 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060057F7 RID: 22519 RVA: 0x0041F42C File Offset: 0x0041D62C
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x0400366D RID: 13933
			public bool engineIsRunning;
		}

		// Token: 0x02000AFC RID: 2812
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x060057F8 RID: 22520 RVA: 0x0041F450 File Offset: 0x0041D650
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MIM_MIB2 mim_MIB = this;
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
							goto IL_01DC;
						}
						CS$<>8__locals1 = new MIM_MIB2.<>c__DisplayClass2_0();
						if (mim_MIB.LastReadData == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_0206;
						}
						speed = 0;
						if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out speed))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_0206;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MIM_MIB2.<Execute>d__2>(ref taskAwaiter3, ref this);
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
						goto IL_0206;
					}
					byte[] array = new byte[mim_MIB.LastReadData.Length - 2];
					Array.Copy(mim_MIB.LastReadData, 0, array, 0, array.Length);
					array[18] = speed;
					byte[] array2 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					taskAwaiter = mim_MIB.WriteDataToECU(password, value, progress, mim_MIB.LastReadData, text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MIM_MIB2.<Execute>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_01DC:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0206:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060057F9 RID: 22521 RVA: 0x0041F69C File Offset: 0x0041D89C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400366E RID: 13934
			public int <>1__state;

			// Token: 0x0400366F RID: 13935
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003670 RID: 13936
			public MIM_MIB2 <>4__this;

			// Token: 0x04003671 RID: 13937
			public string value;

			// Token: 0x04003672 RID: 13938
			private MIM_MIB2.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x04003673 RID: 13939
			public string password;

			// Token: 0x04003674 RID: 13940
			public IProgress<string> progress;

			// Token: 0x04003675 RID: 13941
			private byte <speed>5__2;

			// Token: 0x04003676 RID: 13942
			private TaskAwaiter <>u__1;

			// Token: 0x04003677 RID: 13943
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000AFD RID: 2813
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x060057FA RID: 22522 RVA: 0x0041F6AC File Offset: 0x0041D8AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MIM_MIB2 mim_MIB = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(password))
						{
							password = mim_MIB.Password;
						}
						taskAwaiter = mim_MIB.GetCurrentStateRawData(password, progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MIM_MIB2.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
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
						mim_MIB.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						mim_MIB.LastReadData = item2;
						if (item2 != null)
						{
							byte b = item2[18];
							mim_MIB.CurrentState = b.ToString(CultureInfo.InvariantCulture);
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

			// Token: 0x060057FB RID: 22523 RVA: 0x0041F7D4 File Offset: 0x0041D9D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003678 RID: 13944
			public int <>1__state;

			// Token: 0x04003679 RID: 13945
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400367A RID: 13946
			public string password;

			// Token: 0x0400367B RID: 13947
			public MIM_MIB2 <>4__this;

			// Token: 0x0400367C RID: 13948
			public IProgress<string> progress;

			// Token: 0x0400367D RID: 13949
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
