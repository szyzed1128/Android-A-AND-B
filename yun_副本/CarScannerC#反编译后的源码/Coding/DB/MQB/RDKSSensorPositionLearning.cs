using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B3A RID: 2874
	internal class RDKSSensorPositionLearning : CustomizableCodingTemplate
	{
		// Token: 0x06005922 RID: 22818 RVA: 0x00428B14 File Offset: 0x00426D14
		public RDKSSensorPositionLearning(int sensorId)
		{
			this.Group = CodingGroup.TPMS;
			base.Name = Translate.GetString("codingDB_RDKSSensorLearning_Name");
			base.Name = string.Format(base.Name, sensorId);
			base.Description = Translate.GetString("codingDB_RDKSSensorLearning_Description");
			switch (sensorId)
			{
			case 1:
				this.ReadModeAndAddress = "2218A0";
				break;
			case 2:
				this.ReadModeAndAddress = "2218A1";
				break;
			case 3:
				this.ReadModeAndAddress = "2218A2";
				break;
			case 4:
				this.ReadModeAndAddress = "2218A3";
				break;
			}
			base.WriteModeAndAddress = "2E091B";
			this.PasswordVisible = true;
			base.MakeChangesToInitialData = false;
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Options.Add(new MQBAdaptationOption(Translate.GetString("PID_FLWHEEL"), "00"));
			this.Options.Add(new MQBAdaptationOption(Translate.GetString("PID_FRWHEEL"), "01"));
			this.Options.Add(new MQBAdaptationOption(Translate.GetString("PID_RLWHEEL"), "02"));
			this.Options.Add(new MQBAdaptationOption(Translate.GetString("PID_RRWHEEL"), "03"));
			this.Options.Add(new MQBAdaptationOption("1", "05"));
			this.Options.Add(new MQBAdaptationOption("2", "06"));
			this.Options.Add(new MQBAdaptationOption("3", "07"));
			this.Options.Add(new MQBAdaptationOption("4", "08"));
			base.OpenSessionCommand = "1003";
			base.PreWriteCommands = "22F198;22F199;2703;2704;2EF198;2EF199;";
			base.PostWriteCommands = "1102";
			base.RequestHeader = "70B";
			base.ResponseHeader = "775";
			base.Protocol = "6";
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x00428D04 File Offset: 0x00426F04
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.sensorid_hex = "";
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
			CodingRequestResult item = tuple.Item2;
			byte[] item2 = tuple.Item1;
			CodingRequestResult codingRequestResult;
			if (item != CodingRequestResult.Success)
			{
				base.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
				codingRequestResult = item;
			}
			else
			{
				try
				{
					int num = (int)item2[0] * 256 * 256 * 256 + (int)item2[1] * 256 * 256 + (int)item2[2] * 256 + (int)item2[3];
					this.sensorid_hex = BitHelpers.ByteArrayToHexString(new byte[]
					{
						item2[0],
						item2[1],
						item2[2],
						item2[3]
					});
					int num2 = (int)item2[4];
					double num3 = (double)item2[6] / 10.0;
					byte b = item2[7];
					string text;
					switch (num2)
					{
					case 0:
						text = Translate.GetString("PID_FLWHEEL");
						goto IL_01C3;
					case 1:
						text = Translate.GetString("PID_FRWHEEL");
						goto IL_01C3;
					case 2:
						text = Translate.GetString("PID_RLWHEEL");
						goto IL_01C3;
					case 3:
						text = Translate.GetString("PID_RRWHEEL");
						goto IL_01C3;
					case 5:
						text = "1";
						goto IL_01C3;
					case 6:
						text = "2";
						goto IL_01C3;
					case 7:
						text = "3";
						goto IL_01C3;
					case 8:
						text = "4";
						goto IL_01C3;
					}
					text = Translate.GetString("coding_StateUnknown");
					IL_01C3:
					base.CurrentState = string.Concat(new string[]
					{
						"ID DEC=",
						num.ToString("000 000 000"),
						"\nID HEX=",
						this.sensorid_hex,
						"\nPosition=",
						text,
						string.Format("\nPressure={0} [{1}]", UnitsHelper.GetValue(num3, UnitsHelper.Units.bar), UnitsHelper.GetCaption(UnitsHelper.Units.bar))
					});
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception)
				{
					codingRequestResult = CodingRequestResult.UnknownError;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x00428D50 File Offset: 0x00426F50
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			string text = this.sensorid_hex + value;
			return await this.WriteDataToECU(password, UserFriendlyValue, progress, null, text);
		}

		// Token: 0x0400379E RID: 14238
		private string sensorid_hex = "";

		// Token: 0x02000B3B RID: 2875
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005925 RID: 22821 RVA: 0x00428DB4 File Offset: 0x00426FB4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RDKSSensorPositionLearning rdkssensorPositionLearning = this;
				CodingRequestResult result;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						string text = rdkssensorPositionLearning.sensorid_hex + value;
						taskAwaiter = rdkssensorPositionLearning.WriteDataToECU(password, UserFriendlyValue, progress, null, text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, RDKSSensorPositionLearning.<Execute>d__3>(ref taskAwaiter, ref this);
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
					result = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x06005926 RID: 22822 RVA: 0x00428E94 File Offset: 0x00427094
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400379F RID: 14239
			public int <>1__state;

			// Token: 0x040037A0 RID: 14240
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040037A1 RID: 14241
			public RDKSSensorPositionLearning <>4__this;

			// Token: 0x040037A2 RID: 14242
			public string value;

			// Token: 0x040037A3 RID: 14243
			public string password;

			// Token: 0x040037A4 RID: 14244
			public string UserFriendlyValue;

			// Token: 0x040037A5 RID: 14245
			public IProgress<string> progress;

			// Token: 0x040037A6 RID: 14246
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000B3C RID: 2876
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005927 RID: 22823 RVA: 0x00428EA4 File Offset: 0x004270A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RDKSSensorPositionLearning rdkssensorPositionLearning = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						rdkssensorPositionLearning.sensorid_hex = "";
						taskAwaiter = rdkssensorPositionLearning.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, RDKSSensorPositionLearning.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
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
						rdkssensorPositionLearning.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						try
						{
							int num3 = (int)item2[0] * 256 * 256 * 256 + (int)item2[1] * 256 * 256 + (int)item2[2] * 256 + (int)item2[3];
							rdkssensorPositionLearning.sensorid_hex = BitHelpers.ByteArrayToHexString(new byte[]
							{
								item2[0],
								item2[1],
								item2[2],
								item2[3]
							});
							int num4 = (int)item2[4];
							double num5 = (double)item2[6] / 10.0;
							byte b = item2[7];
							string text;
							switch (num4)
							{
							case 0:
								text = Translate.GetString("PID_FLWHEEL");
								goto IL_01C3;
							case 1:
								text = Translate.GetString("PID_FRWHEEL");
								goto IL_01C3;
							case 2:
								text = Translate.GetString("PID_RLWHEEL");
								goto IL_01C3;
							case 3:
								text = Translate.GetString("PID_RRWHEEL");
								goto IL_01C3;
							case 5:
								text = "1";
								goto IL_01C3;
							case 6:
								text = "2";
								goto IL_01C3;
							case 7:
								text = "3";
								goto IL_01C3;
							case 8:
								text = "4";
								goto IL_01C3;
							}
							text = Translate.GetString("coding_StateUnknown");
							IL_01C3:
							rdkssensorPositionLearning.CurrentState = string.Concat(new string[]
							{
								"ID DEC=",
								num3.ToString("000 000 000"),
								"\nID HEX=",
								rdkssensorPositionLearning.sensorid_hex,
								"\nPosition=",
								text,
								string.Format("\nPressure={0} [{1}]", UnitsHelper.GetValue(num5, UnitsHelper.Units.bar), UnitsHelper.GetCaption(UnitsHelper.Units.bar))
							});
							codingRequestResult = CodingRequestResult.Success;
						}
						catch (Exception)
						{
							codingRequestResult = CodingRequestResult.UnknownError;
						}
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

			// Token: 0x06005928 RID: 22824 RVA: 0x00429148 File Offset: 0x00427348
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040037A7 RID: 14247
			public int <>1__state;

			// Token: 0x040037A8 RID: 14248
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040037A9 RID: 14249
			public RDKSSensorPositionLearning <>4__this;

			// Token: 0x040037AA RID: 14250
			public string password;

			// Token: 0x040037AB RID: 14251
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
