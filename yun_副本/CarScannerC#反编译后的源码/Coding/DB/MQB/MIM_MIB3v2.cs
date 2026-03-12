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
	// Token: 0x02000AFE RID: 2814
	internal class MIM_MIB3v2 : VIM_MIB3v2
	{
		// Token: 0x060057FC RID: 22524 RVA: 0x0041F7E4 File Offset: 0x0041D9E4
		public MIM_MIB3v2()
		{
			base.Name = Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib2_Name").Replace("MIB2", "MIB3");
			base.Description = Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib2_Description");
			base.InnerDescription = Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib2_InnerDescription") + "\n" + Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib3Warning");
			base.Translations.Clear();
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x0041F858 File Offset: 0x0041DA58
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			if (string.IsNullOrEmpty(password))
			{
				password = this.Password;
			}
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
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
					if (this.LastReadData.Length != 34)
					{
						this.LastReadData = null;
						base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					else
					{
						byte b = item2[22];
						base.CurrentState = b.ToString(CultureInfo.InvariantCulture);
					}
				}
				codingRequestResult = item;
			}
			return codingRequestResult;
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x0041F8A4 File Offset: 0x0041DAA4
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
						byte[] array = new byte[this.LastReadData.Length - 4];
						Array.Copy(this.LastReadData, 0, array, 0, array.Length);
						array[22] = speed;
						byte[] array2 = Crc32.Calculate(array);
						string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
						codingRequestResult = await this.WriteDataToECU(password, value, progress, this.LastReadData, text);
					}
				}
			}
			return codingRequestResult;
		}

		// Token: 0x02000AFF RID: 2815
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060057FF RID: 22527 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06005800 RID: 22528 RVA: 0x0041F900 File Offset: 0x0041DB00
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x0400367E RID: 13950
			public bool engineIsRunning;
		}

		// Token: 0x02000B00 RID: 2816
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005801 RID: 22529 RVA: 0x0041F924 File Offset: 0x0041DB24
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MIM_MIB3v2 mim_MIB3v = this;
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
							goto IL_01D2;
						}
						CS$<>8__locals1 = new MIM_MIB3v2.<>c__DisplayClass2_0();
						if (mim_MIB3v.LastReadData == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_01FC;
						}
						speed = 0;
						if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out speed))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_01FC;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MIM_MIB3v2.<Execute>d__2>(ref taskAwaiter3, ref this);
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
						goto IL_01FC;
					}
					byte[] array = new byte[mim_MIB3v.LastReadData.Length - 4];
					Array.Copy(mim_MIB3v.LastReadData, 0, array, 0, array.Length);
					array[22] = speed;
					byte[] array2 = Crc32.Calculate(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					taskAwaiter = mim_MIB3v.WriteDataToECU(password, value, progress, mim_MIB3v.LastReadData, text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MIM_MIB3v2.<Execute>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_01D2:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01FC:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005802 RID: 22530 RVA: 0x0041FB64 File Offset: 0x0041DD64
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400367F RID: 13951
			public int <>1__state;

			// Token: 0x04003680 RID: 13952
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003681 RID: 13953
			public MIM_MIB3v2 <>4__this;

			// Token: 0x04003682 RID: 13954
			public string value;

			// Token: 0x04003683 RID: 13955
			private MIM_MIB3v2.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x04003684 RID: 13956
			public string password;

			// Token: 0x04003685 RID: 13957
			public IProgress<string> progress;

			// Token: 0x04003686 RID: 13958
			private byte <speed>5__2;

			// Token: 0x04003687 RID: 13959
			private TaskAwaiter <>u__1;

			// Token: 0x04003688 RID: 13960
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000B01 RID: 2817
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005803 RID: 22531 RVA: 0x0041FB74 File Offset: 0x0041DD74
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MIM_MIB3v2 mim_MIB3v = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(password))
						{
							password = mim_MIB3v.Password;
						}
						taskAwaiter = mim_MIB3v.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MIM_MIB3v2.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
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
						mim_MIB3v.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						mim_MIB3v.LastReadData = item2;
						if (item2 != null)
						{
							if (mim_MIB3v.LastReadData.Length != 34)
							{
								mim_MIB3v.LastReadData = null;
								mim_MIB3v.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							}
							else
							{
								byte b = item2[22];
								mim_MIB3v.CurrentState = b.ToString(CultureInfo.InvariantCulture);
							}
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

			// Token: 0x06005804 RID: 22532 RVA: 0x0041FCB8 File Offset: 0x0041DEB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003689 RID: 13961
			public int <>1__state;

			// Token: 0x0400368A RID: 13962
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400368B RID: 13963
			public string password;

			// Token: 0x0400368C RID: 13964
			public MIM_MIB3v2 <>4__this;

			// Token: 0x0400368D RID: 13965
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
