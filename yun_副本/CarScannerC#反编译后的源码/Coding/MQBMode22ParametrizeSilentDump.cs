using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008B0 RID: 2224
	internal class MQBMode22ParametrizeSilentDump : MQBCustomizableMode22Coding
	{
		// Token: 0x06004B80 RID: 19328 RVA: 0x003841B0 File Offset: 0x003823B0
		public MQBMode22ParametrizeSilentDump(string unit, string address)
		{
			base.Address = address;
			base.Group = CodingGroup.DatasetDump;
			base.Name = "Dataset " + unit + " 0x" + address;
			base.ValueType = AdaptationValueTypes.InputValueType;
			this.Password = "20103";
			base.PreReadCommands = "1003;1040;2704";
			base.PreWriteCommands = "1003;1040;22F1A0;22F1A1;2704;2EF198;2EF199;2EF1A0;2EF1A1;";
			base.PostWriteCommands = "1102;";
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit(unit);
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit(unit);
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x00384234 File Offset: 0x00382434
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			if (string.IsNullOrEmpty(password))
			{
				password = this.Password;
			}
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
			CodingRequestResult requestResult = tuple.Item2;
			byte[] item = tuple.Item1;
			CodingRequestResult codingRequestResult;
			if (requestResult != CodingRequestResult.Success)
			{
				base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(requestResult);
				codingRequestResult = requestResult;
			}
			else
			{
				this.LastReadData = item;
				string hex = BitHelpers.ByteArrayToHexString(item);
				await App.OBDReader.DebugWrite(string.Concat(new string[] { "\n", base.Name, ": ", hex, "\n" }));
				base.CurrentState = hex;
				codingRequestResult = requestResult;
			}
			return codingRequestResult;
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x00384280 File Offset: 0x00382480
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			if (this.LastReadData == null)
			{
				MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1 CS$<>8__locals2 = new MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1();
				CS$<>8__locals2.shouldWriteWithoutReading = false;
				await MainThread.InvokeOnMainThreadAsync(delegate
				{
					MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1.<<Execute>b__1>d <<Execute>b__1>d;
					<<Execute>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<Execute>b__1>d.<>4__this = CS$<>8__locals2;
					<<Execute>b__1>d.<>1__state = -1;
					<<Execute>b__1>d.<>t__builder.Start<MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1.<<Execute>b__1>d>(ref <<Execute>b__1>d);
					return <<Execute>b__1>d.<>t__builder.Task;
				});
				if (!CS$<>8__locals2.shouldWriteWithoutReading)
				{
					return CodingRequestResult.NotSupported;
				}
				CS$<>8__locals2 = null;
			}
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
			CodingRequestResult codingRequestResult;
			if (engineIsRunning)
			{
				codingRequestResult = CodingRequestResult.WrongConditions;
			}
			else
			{
				codingRequestResult = await this.WriteDataToECU(password, value, progress, this.LastReadData, value);
			}
			return codingRequestResult;
		}

		// Token: 0x04002C2A RID: 11306
		protected byte[] LastReadData;

		// Token: 0x020008B1 RID: 2225
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06004B83 RID: 19331 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06004B84 RID: 19332 RVA: 0x003842DC File Offset: 0x003824DC
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x04002C2B RID: 11307
			public bool engineIsRunning;
		}

		// Token: 0x020008B2 RID: 2226
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_1
		{
			// Token: 0x06004B85 RID: 19333 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_1()
			{
			}

			// Token: 0x06004B86 RID: 19334 RVA: 0x00384300 File Offset: 0x00382500
			internal async Task <Execute>b__1()
			{
				bool flag = await App.GetCurrentPage().DisplayAlert("WARNING!", "IMPORTANT! Cannot make a backup of the dataset, if you continue, you will overwrite original dataset and will not be able to restore it. Do you understand the consequences and really want to continue?", "OK", "Cancel");
				this.shouldWriteWithoutReading = flag;
			}

			// Token: 0x04002C2C RID: 11308
			public bool shouldWriteWithoutReading;

			// Token: 0x020008B3 RID: 2227
			[StructLayout(LayoutKind.Auto)]
			private struct <<Execute>b__1>d : IAsyncStateMachine
			{
				// Token: 0x06004B87 RID: 19335 RVA: 0x00384344 File Offset: 0x00382544
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = App.GetCurrentPage().DisplayAlert("WARNING!", "IMPORTANT! Cannot make a backup of the dataset, if you continue, you will overwrite original dataset and will not be able to restore it. Do you understand the consequences and really want to continue?", "OK", "Cancel").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1.<<Execute>b__1>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = taskAwaiter.GetResult();
						CS$<>8__locals1.shouldWriteWithoutReading = result;
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

				// Token: 0x06004B88 RID: 19336 RVA: 0x00384418 File Offset: 0x00382618
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04002C2D RID: 11309
				public int <>1__state;

				// Token: 0x04002C2E RID: 11310
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04002C2F RID: 11311
				public MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1 <>4__this;

				// Token: 0x04002C30 RID: 11312
				private TaskAwaiter<bool> <>u__1;
			}
		}

		// Token: 0x020008B4 RID: 2228
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004B89 RID: 19337 RVA: 0x00384428 File Offset: 0x00382628
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBMode22ParametrizeSilentDump mqbmode22ParametrizeSilentDump = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<CodingRequestResult> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0175;
					}
					case 2:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0202;
					}
					default:
						CS$<>8__locals1 = new MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_0();
						if (mqbmode22ParametrizeSilentDump.LastReadData != null)
						{
							goto IL_00D7;
						}
						CS$<>8__locals2 = new MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1();
						CS$<>8__locals2.shouldWriteWithoutReading = false;
						taskAwaiter = MainThread.InvokeOnMainThreadAsync(delegate
						{
							MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1.<<Execute>b__1>d <<Execute>b__1>d;
							<<Execute>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<Execute>b__1>d.<>4__this = CS$<>8__locals2;
							<<Execute>b__1>d.<>1__state = -1;
							<<Execute>b__1>d.<>t__builder.Start<MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1.<<Execute>b__1>d>(ref <<Execute>b__1>d);
							return <<Execute>b__1>d.<>t__builder.Task;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBMode22ParametrizeSilentDump.<Execute>d__3>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					if (!CS$<>8__locals2.shouldWriteWithoutReading)
					{
						codingRequestResult = CodingRequestResult.NotSupported;
						goto IL_022C;
					}
					CS$<>8__locals2 = null;
					IL_00D7:
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
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBMode22ParametrizeSilentDump.<Execute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_0175:
					taskAwaiter.GetResult();
					if (CS$<>8__locals1.engineIsRunning)
					{
						codingRequestResult = CodingRequestResult.WrongConditions;
						goto IL_022C;
					}
					taskAwaiter3 = mqbmode22ParametrizeSilentDump.WriteDataToECU(password, value, progress, mqbmode22ParametrizeSilentDump.LastReadData, value).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<CodingRequestResult> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBMode22ParametrizeSilentDump.<Execute>d__3>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0202:
					codingRequestResult = taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_022C:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004B8A RID: 19338 RVA: 0x00384698 File Offset: 0x00382898
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002C31 RID: 11313
			public int <>1__state;

			// Token: 0x04002C32 RID: 11314
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002C33 RID: 11315
			public MQBMode22ParametrizeSilentDump <>4__this;

			// Token: 0x04002C34 RID: 11316
			private MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_1 <>8__1;

			// Token: 0x04002C35 RID: 11317
			private MQBMode22ParametrizeSilentDump.<>c__DisplayClass3_0 <>8__2;

			// Token: 0x04002C36 RID: 11318
			public string password;

			// Token: 0x04002C37 RID: 11319
			public string value;

			// Token: 0x04002C38 RID: 11320
			public IProgress<string> progress;

			// Token: 0x04002C39 RID: 11321
			private TaskAwaiter <>u__1;

			// Token: 0x04002C3A RID: 11322
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x020008B5 RID: 2229
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004B8B RID: 19339 RVA: 0x003846A8 File Offset: 0x003828A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBMode22ParametrizeSilentDump mqbmode22ParametrizeSilentDump = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0171;
						}
						if (string.IsNullOrEmpty(password))
						{
							password = mqbmode22ParametrizeSilentDump.Password;
						}
						taskAwaiter3 = mqbmode22ParametrizeSilentDump.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBMode22ParametrizeSilentDump.<UpdateCurrentState>d__2>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter3.GetResult();
					requestResult = result.Item2;
					byte[] item = result.Item1;
					if (requestResult != CodingRequestResult.Success)
					{
						mqbmode22ParametrizeSilentDump.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(requestResult);
						codingRequestResult = requestResult;
						goto IL_01AD;
					}
					mqbmode22ParametrizeSilentDump.LastReadData = item;
					hex = BitHelpers.ByteArrayToHexString(item);
					taskAwaiter = App.OBDReader.DebugWrite(string.Concat(new string[] { "\n", mqbmode22ParametrizeSilentDump.Name, ": ", hex, "\n" })).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBMode22ParametrizeSilentDump.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_0171:
					taskAwaiter.GetResult();
					mqbmode22ParametrizeSilentDump.CurrentState = hex;
					codingRequestResult = requestResult;
				}
				catch (Exception ex)
				{
					num2 = -2;
					hex = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01AD:
				num2 = -2;
				hex = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004B8C RID: 19340 RVA: 0x0038489C File Offset: 0x00382A9C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002C3B RID: 11323
			public int <>1__state;

			// Token: 0x04002C3C RID: 11324
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002C3D RID: 11325
			public string password;

			// Token: 0x04002C3E RID: 11326
			public MQBMode22ParametrizeSilentDump <>4__this;

			// Token: 0x04002C3F RID: 11327
			private CodingRequestResult <requestResult>5__2;

			// Token: 0x04002C40 RID: 11328
			private string <hex>5__3;

			// Token: 0x04002C41 RID: 11329
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;

			// Token: 0x04002C42 RID: 11330
			private TaskAwaiter <>u__2;
		}
	}
}
