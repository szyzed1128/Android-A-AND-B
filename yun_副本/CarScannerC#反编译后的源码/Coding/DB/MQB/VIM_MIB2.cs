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
	// Token: 0x02000B6A RID: 2922
	internal class VIM_MIB2 : MQBParametrizeBase
	{
		// Token: 0x060059F7 RID: 23031 RVA: 0x0042EEA4 File Offset: 0x0042D0A4
		public VIM_MIB2()
		{
			base.Address = 576;
			base.DataLength = 30;
			base.DataLengthFormatLength = 3;
			base.AddressFormatLength = 3;
			base.Group = CodingGroup.Multimedia;
			base.Name = Translate.GetString("codingDB_VideoInMotionSpeedThreshold_Name") + " (MIB2)";
			base.Description = Translate.GetString("codingDB_VideoInMotionSpeedThreshold_Description");
			base.InnerDescription = Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib2_InnerDescription");
			base.ValueType = AdaptationValueTypes.InputValueType;
			this.Password = "20103";
			base.PreReadCommands = "1003;1040;2704;";
			base.PreWriteCommands = "700:1083;1003;1040;22F1A0;700:3E80;22F1A1;700:3E80;22F1A4;700:3E80;2704;2EF198;700:3E80;2EF199;700:3E80;31010300030100;700:3E80;31030300;700:3E80;";
			base.PostWriteCommands = "700:3E80;310102EF030100;700:3E80;310302EF;700:3E80;2EF1A0;700:3E80;2EF1A1;700:3E80;2EF1A4;700:3E80;1102;1003;14FFFFFF";
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F");
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F");
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x0042EF70 File Offset: 0x0042D170
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
					byte b = item2[0];
					base.CurrentState = b.ToString(CultureInfo.InvariantCulture);
				}
				codingRequestResult = item;
			}
			return codingRequestResult;
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x0042EFC4 File Offset: 0x0042D1C4
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
						array[0] = speed;
						byte[] array2 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array);
						string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
						codingRequestResult = await this.WriteDataToECU(password, value, progress, this.LastReadData, text);
					}
				}
			}
			return codingRequestResult;
		}

		// Token: 0x0400385F RID: 14431
		protected byte[] LastReadData;

		// Token: 0x02000B6B RID: 2923
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060059FA RID: 23034 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060059FB RID: 23035 RVA: 0x0042F020 File Offset: 0x0042D220
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x04003860 RID: 14432
			public bool engineIsRunning;
		}

		// Token: 0x02000B6C RID: 2924
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x060059FC RID: 23036 RVA: 0x0042F044 File Offset: 0x0042D244
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VIM_MIB2 vim_MIB = this;
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
							goto IL_01DB;
						}
						CS$<>8__locals1 = new VIM_MIB2.<>c__DisplayClass3_0();
						if (vim_MIB.LastReadData == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_0205;
						}
						speed = 0;
						if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out speed))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_0205;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VIM_MIB2.<Execute>d__3>(ref taskAwaiter3, ref this);
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
						goto IL_0205;
					}
					byte[] array = new byte[vim_MIB.LastReadData.Length - 2];
					Array.Copy(vim_MIB.LastReadData, 0, array, 0, array.Length);
					array[0] = speed;
					byte[] array2 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					taskAwaiter = vim_MIB.WriteDataToECU(password, value, progress, vim_MIB.LastReadData, text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VIM_MIB2.<Execute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_01DB:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0205:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060059FD RID: 23037 RVA: 0x0042F290 File Offset: 0x0042D490
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003861 RID: 14433
			public int <>1__state;

			// Token: 0x04003862 RID: 14434
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003863 RID: 14435
			public VIM_MIB2 <>4__this;

			// Token: 0x04003864 RID: 14436
			public string value;

			// Token: 0x04003865 RID: 14437
			private VIM_MIB2.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04003866 RID: 14438
			public string password;

			// Token: 0x04003867 RID: 14439
			public IProgress<string> progress;

			// Token: 0x04003868 RID: 14440
			private byte <speed>5__2;

			// Token: 0x04003869 RID: 14441
			private TaskAwaiter <>u__1;

			// Token: 0x0400386A RID: 14442
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000B6D RID: 2925
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x060059FE RID: 23038 RVA: 0x0042F2A0 File Offset: 0x0042D4A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VIM_MIB2 vim_MIB = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(password))
						{
							password = vim_MIB.Password;
						}
						taskAwaiter = vim_MIB.GetCurrentStateRawData(password, progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, VIM_MIB2.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
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
						vim_MIB.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						vim_MIB.LastReadData = item2;
						if (item2 != null)
						{
							byte b = item2[0];
							vim_MIB.CurrentState = b.ToString(CultureInfo.InvariantCulture);
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

			// Token: 0x060059FF RID: 23039 RVA: 0x0042F3C8 File Offset: 0x0042D5C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400386B RID: 14443
			public int <>1__state;

			// Token: 0x0400386C RID: 14444
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400386D RID: 14445
			public string password;

			// Token: 0x0400386E RID: 14446
			public VIM_MIB2 <>4__this;

			// Token: 0x0400386F RID: 14447
			public IProgress<string> progress;

			// Token: 0x04003870 RID: 14448
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
