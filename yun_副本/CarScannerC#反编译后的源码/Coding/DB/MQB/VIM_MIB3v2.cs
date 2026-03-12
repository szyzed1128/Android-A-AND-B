using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B72 RID: 2930
	internal class VIM_MIB3v2 : MQBCustomizableMode22Coding
	{
		// Token: 0x06005A09 RID: 23049 RVA: 0x0042F87C File Offset: 0x0042DA7C
		public VIM_MIB3v2()
		{
			base.Address = "720D";
			base.Group = CodingGroup.Multimedia;
			base.Name = Translate.GetString("codingDB_VideoInMotionSpeedThreshold_Name") + " (MIB3)";
			base.Description = Translate.GetString("codingDB_VideoInMotionSpeedThreshold_Description");
			base.InnerDescription = Translate.GetString("codingDB_MirrorLinkInMotionSpeedThresholdMib2_InnerDescription");
			base.ValueType = AdaptationValueTypes.InputValueType;
			this.Password = "20103";
			base.PreReadCommands = "1003;1040";
			base.PreWriteCommands = "1003;1040;22F1A0;22F1A1;2704;2EF198;2EF199;2EF1A0;2EF1A1;";
			base.PostWriteCommands = "1102;";
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F");
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F");
			base.Translations.Add(new TranslationItem("ru", "Порог скорости отключения видео в движении (MIB3)", "Используйте для активации видео в движении", "Чтобы активировать видео в движении, задайте скорость для отключения 255 (на некоторых автомобилях - 180)\nУсловия:\n1) MIB3\n2) Зажигание включено\n3) Двигатель не запущен"));
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x0042F954 File Offset: 0x0042DB54
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
						byte b = item2[4];
						base.CurrentState = b.ToString(CultureInfo.InvariantCulture);
					}
				}
				codingRequestResult = item;
			}
			return codingRequestResult;
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x0042F9A0 File Offset: 0x0042DBA0
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
						array[4] = speed;
						byte[] array2 = Crc32.Calculate(array);
						string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
						codingRequestResult = await this.WriteDataToECU(password, value, progress, this.LastReadData, text);
					}
				}
			}
			return codingRequestResult;
		}

		// Token: 0x04003883 RID: 14467
		protected byte[] LastReadData;

		// Token: 0x02000B73 RID: 2931
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06005A0C RID: 23052 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005A0D RID: 23053 RVA: 0x0042F9FC File Offset: 0x0042DBFC
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x04003884 RID: 14468
			public bool engineIsRunning;
		}

		// Token: 0x02000B74 RID: 2932
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005A0E RID: 23054 RVA: 0x0042FA20 File Offset: 0x0042DC20
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VIM_MIB3v2 vim_MIB3v = this;
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
							goto IL_01D1;
						}
						CS$<>8__locals1 = new VIM_MIB3v2.<>c__DisplayClass3_0();
						if (vim_MIB3v.LastReadData == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_01FB;
						}
						speed = 0;
						if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out speed))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_01FB;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VIM_MIB3v2.<Execute>d__3>(ref taskAwaiter3, ref this);
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
						goto IL_01FB;
					}
					byte[] array = new byte[vim_MIB3v.LastReadData.Length - 4];
					Array.Copy(vim_MIB3v.LastReadData, 0, array, 0, array.Length);
					array[4] = speed;
					byte[] array2 = Crc32.Calculate(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					taskAwaiter = vim_MIB3v.WriteDataToECU(password, value, progress, vim_MIB3v.LastReadData, text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VIM_MIB3v2.<Execute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_01D1:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01FB:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005A0F RID: 23055 RVA: 0x0042FC60 File Offset: 0x0042DE60
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003885 RID: 14469
			public int <>1__state;

			// Token: 0x04003886 RID: 14470
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003887 RID: 14471
			public VIM_MIB3v2 <>4__this;

			// Token: 0x04003888 RID: 14472
			public string value;

			// Token: 0x04003889 RID: 14473
			private VIM_MIB3v2.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x0400388A RID: 14474
			public string password;

			// Token: 0x0400388B RID: 14475
			public IProgress<string> progress;

			// Token: 0x0400388C RID: 14476
			private byte <speed>5__2;

			// Token: 0x0400388D RID: 14477
			private TaskAwaiter <>u__1;

			// Token: 0x0400388E RID: 14478
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000B75 RID: 2933
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005A10 RID: 23056 RVA: 0x0042FC70 File Offset: 0x0042DE70
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VIM_MIB3v2 vim_MIB3v = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(password))
						{
							password = vim_MIB3v.Password;
						}
						taskAwaiter = vim_MIB3v.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, VIM_MIB3v2.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
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
						vim_MIB3v.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						vim_MIB3v.LastReadData = item2;
						if (item2 != null)
						{
							if (vim_MIB3v.LastReadData.Length != 34)
							{
								vim_MIB3v.LastReadData = null;
								vim_MIB3v.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							}
							else
							{
								byte b = item2[4];
								vim_MIB3v.CurrentState = b.ToString(CultureInfo.InvariantCulture);
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

			// Token: 0x06005A11 RID: 23057 RVA: 0x0042FDB4 File Offset: 0x0042DFB4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400388F RID: 14479
			public int <>1__state;

			// Token: 0x04003890 RID: 14480
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003891 RID: 14481
			public string password;

			// Token: 0x04003892 RID: 14482
			public VIM_MIB3v2 <>4__this;

			// Token: 0x04003893 RID: 14483
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
