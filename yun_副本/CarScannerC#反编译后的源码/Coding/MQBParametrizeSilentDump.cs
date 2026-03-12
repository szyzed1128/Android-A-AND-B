using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008CA RID: 2250
	internal class MQBParametrizeSilentDump : MQBParametrizeBase
	{
		// Token: 0x06004C2E RID: 19502 RVA: 0x0038797C File Offset: 0x00385B7C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			if (string.IsNullOrEmpty(password))
			{
				password = this.Password;
			}
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password, progress);
			CodingRequestResult item = tuple.Item2;
			byte[] data = tuple.Item1;
			CodingRequestResult codingRequestResult;
			if (item != CodingRequestResult.Success)
			{
				base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
				codingRequestResult = item;
			}
			else
			{
				if (data != null && data.Length != 0)
				{
					if (SharedSettings.Current.DeveloperMode)
					{
						await App.OBDReader.DebugWrite(string.Concat(new string[]
						{
							"\n",
							base.Name,
							": ",
							BitHelpers.ByteArrayToHexString(data),
							"\n"
						}));
					}
					else
					{
						await App.OBDReader.DebugWrite("\nATDSDOK\n");
					}
				}
				if (data != null && data.Length != 0)
				{
					base.CurrentState = BitHelpers.ByteArrayToHexString(data);
				}
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x003879D0 File Offset: 0x00385BD0
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			if (originalData == null)
			{
				TaskAwaiter<CodingRequestResult> taskAwaiter = this.UpdateCurrentState(password, null).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<CodingRequestResult> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
				}
				if (taskAwaiter.GetResult() == CodingRequestResult.Success)
				{
					originalData = BitHelpers.ConvertHexToBytesX(base.CurrentState);
				}
			}
			CodingRequestResult codingRequestResult;
			if (base.RequestHeader == "773")
			{
				byte[] array = BitHelpers.ConvertHexToBytesX(value);
				byte[] array2 = new byte[array.Length - 2];
				Array.Copy(array, 0, array2, 0, array2.Length);
				codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, BitHelpers.ByteArrayToHexString(array2.Concat(new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array2)).ToArray<byte>()));
			}
			else
			{
				codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, value);
			}
			return codingRequestResult;
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x00387A3D File Offset: 0x00385C3D
		public MQBParametrizeSilentDump()
		{
		}

		// Token: 0x020008CB RID: 2251
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__1 : IAsyncStateMachine
		{
			// Token: 0x06004C31 RID: 19505 RVA: 0x00387A48 File Offset: 0x00385C48
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBParametrizeSilentDump mqbparametrizeSilentDump = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter3;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						break;
					case 1:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_016C;
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_01E8;
					default:
						if (originalData != null)
						{
							goto IL_009D;
						}
						taskAwaiter3 = mqbparametrizeSilentDump.UpdateCurrentState(password, null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBParametrizeSilentDump.<Execute>d__1>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (taskAwaiter3.GetResult() == CodingRequestResult.Success)
					{
						originalData = BitHelpers.ConvertHexToBytesX(mqbparametrizeSilentDump.CurrentState);
					}
					IL_009D:
					if (mqbparametrizeSilentDump.RequestHeader == "773")
					{
						byte[] array = BitHelpers.ConvertHexToBytesX(value);
						byte[] array2 = new byte[array.Length - 2];
						Array.Copy(array, 0, array2, 0, array2.Length);
						byte[] array3 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array2);
						string text = BitHelpers.ByteArrayToHexString(array2.Concat(array3).ToArray<byte>());
						taskAwaiter3 = mqbparametrizeSilentDump.WriteDataToECU(password, UserFriendlyValue, progress, originalData, text).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 1;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBParametrizeSilentDump.<Execute>d__1>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = mqbparametrizeSilentDump.WriteDataToECU(password, UserFriendlyValue, progress, originalData, value).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBParametrizeSilentDump.<Execute>d__1>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_01E8;
					}
					IL_016C:
					codingRequestResult = taskAwaiter3.GetResult();
					goto IL_020B;
					IL_01E8:
					codingRequestResult = taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_020B:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004C32 RID: 19506 RVA: 0x00387C90 File Offset: 0x00385E90
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002CDA RID: 11482
			public int <>1__state;

			// Token: 0x04002CDB RID: 11483
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002CDC RID: 11484
			public byte[] originalData;

			// Token: 0x04002CDD RID: 11485
			public MQBParametrizeSilentDump <>4__this;

			// Token: 0x04002CDE RID: 11486
			public string password;

			// Token: 0x04002CDF RID: 11487
			public string value;

			// Token: 0x04002CE0 RID: 11488
			public string UserFriendlyValue;

			// Token: 0x04002CE1 RID: 11489
			public IProgress<string> progress;

			// Token: 0x04002CE2 RID: 11490
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x020008CC RID: 2252
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__0 : IAsyncStateMachine
		{
			// Token: 0x06004C33 RID: 19507 RVA: 0x00387CA0 File Offset: 0x00385EA0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBParametrizeSilentDump mqbparametrizeSilentDump = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_018C;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01F5;
					}
					default:
						if (string.IsNullOrEmpty(password))
						{
							password = mqbparametrizeSilentDump.Password;
						}
						taskAwaiter = mqbparametrizeSilentDump.GetCurrentStateRawData(password, progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBParametrizeSilentDump.<UpdateCurrentState>d__0>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					data = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						mqbparametrizeSilentDump.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
						goto IL_0242;
					}
					if (data == null || data.Length == 0)
					{
						goto IL_01FC;
					}
					if (SharedSettings.Current.DeveloperMode)
					{
						taskAwaiter3 = App.OBDReader.DebugWrite(string.Concat(new string[]
						{
							"\n",
							mqbparametrizeSilentDump.Name,
							": ",
							BitHelpers.ByteArrayToHexString(data),
							"\n"
						})).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBParametrizeSilentDump.<UpdateCurrentState>d__0>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = App.OBDReader.DebugWrite("\nATDSDOK\n").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBParametrizeSilentDump.<UpdateCurrentState>d__0>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_01F5;
					}
					IL_018C:
					taskAwaiter3.GetResult();
					goto IL_01FC;
					IL_01F5:
					taskAwaiter3.GetResult();
					IL_01FC:
					if (data != null && data.Length != 0)
					{
						mqbparametrizeSilentDump.CurrentState = BitHelpers.ByteArrayToHexString(data);
					}
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					num2 = -2;
					data = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0242:
				num2 = -2;
				data = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004C34 RID: 19508 RVA: 0x00387F28 File Offset: 0x00386128
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002CE3 RID: 11491
			public int <>1__state;

			// Token: 0x04002CE4 RID: 11492
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002CE5 RID: 11493
			public string password;

			// Token: 0x04002CE6 RID: 11494
			public MQBParametrizeSilentDump <>4__this;

			// Token: 0x04002CE7 RID: 11495
			public IProgress<string> progress;

			// Token: 0x04002CE8 RID: 11496
			private byte[] <data>5__2;

			// Token: 0x04002CE9 RID: 11497
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;

			// Token: 0x04002CEA RID: 11498
			private TaskAwaiter <>u__2;
		}
	}
}
