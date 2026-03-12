using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B37 RID: 2871
	internal class RDKSSensorIDLearning : CustomizableCodingTemplate
	{
		// Token: 0x0600591B RID: 22811 RVA: 0x0042861C File Offset: 0x0042681C
		public RDKSSensorIDLearning(int position)
		{
			this.Group = CodingGroup.TPMS;
			base.Name = Translate.GetString("codingDB_RDKSSensorIdLearning_Name");
			switch (position)
			{
			case 0:
				base.Name = string.Format(base.Name, Translate.GetString("PID_FLWHEEL"));
				break;
			case 1:
				base.Name = string.Format(base.Name, Translate.GetString("PID_FRWHEEL"));
				break;
			case 2:
				base.Name = string.Format(base.Name, Translate.GetString("PID_RLWHEEL"));
				break;
			case 3:
				base.Name = string.Format(base.Name, Translate.GetString("PID_RRWHEEL"));
				break;
			}
			base.Description = Translate.GetString("codingDB_RDKSSensorIdLearning_Description");
			this.ReadModeAndAddress = "2218A0";
			base.WriteModeAndAddress = "2E091B";
			this.PasswordVisible = true;
			base.MakeChangesToInitialData = false;
			this.ValueType = AdaptationValueTypes.InputValueType;
			base.OpenSessionCommand = "1003";
			base.PreWriteCommands = "22F198;22F199;2703;2704;2EF198;2EF199;";
			base.PostWriteCommands = "1102";
			base.RequestHeader = "70B";
			base.ResponseHeader = "775";
			base.Protocol = "6";
			this.targetPosition = position;
		}

		// Token: 0x0600591C RID: 22812 RVA: 0x0042875C File Offset: 0x0042695C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			int i = 2234528;
			while (i <= 2234531)
			{
				this.ReadModeAndAddress = i.ToString("X6");
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
					if ((int)item2[4] != this.targetPosition)
					{
						i++;
						continue;
					}
					base.CurrentState = ((int)item2[0] * 256 * 256 * 256 + (int)item2[1] * 256 * 256 + (int)item2[2] * 256 + (int)item2[3]).ToString("000000000");
					codingRequestResult = CodingRequestResult.Success;
				}
				return codingRequestResult;
			}
			base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
			return CodingRequestResult.Success;
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x004287A8 File Offset: 0x004269A8
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			if (value != null)
			{
				value = value.Replace(" ", "").Trim();
			}
			uint num;
			CodingRequestResult codingRequestResult;
			if (!uint.TryParse(value, out num))
			{
				codingRequestResult = CodingRequestResult.WrongInputValue;
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(num);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(bytes);
				}
				string text = BitHelpers.ByteArrayToHexString(bytes) + this.targetPosition.ToString("X2");
				codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, null, text);
			}
			return codingRequestResult;
		}

		// Token: 0x0400378F RID: 14223
		private int targetPosition = -1;

		// Token: 0x02000B38 RID: 2872
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600591E RID: 22814 RVA: 0x0042880C File Offset: 0x00426A0C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RDKSSensorIDLearning rdkssensorIDLearning = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (value != null)
						{
							value = value.Replace(" ", "").Trim();
						}
						uint num3;
						if (!uint.TryParse(value, out num3))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_0113;
						}
						byte[] bytes = BitConverter.GetBytes(num3);
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse<byte>(bytes);
						}
						string text = BitHelpers.ByteArrayToHexString(bytes) + rdkssensorIDLearning.targetPosition.ToString("X2");
						taskAwaiter = rdkssensorIDLearning.WriteDataToECU(password, UserFriendlyValue, progress, null, text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, RDKSSensorIDLearning.<Execute>d__3>(ref taskAwaiter, ref this);
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
				IL_0113:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x0600591F RID: 22815 RVA: 0x00428950 File Offset: 0x00426B50
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003790 RID: 14224
			public int <>1__state;

			// Token: 0x04003791 RID: 14225
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003792 RID: 14226
			public string value;

			// Token: 0x04003793 RID: 14227
			public RDKSSensorIDLearning <>4__this;

			// Token: 0x04003794 RID: 14228
			public string password;

			// Token: 0x04003795 RID: 14229
			public string UserFriendlyValue;

			// Token: 0x04003796 RID: 14230
			public IProgress<string> progress;

			// Token: 0x04003797 RID: 14231
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000B39 RID: 2873
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__2 : IAsyncStateMachine
		{
			// Token: 0x06005920 RID: 22816 RVA: 0x00428960 File Offset: 0x00426B60
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RDKSSensorIDLearning rdkssensorIDLearning = this;
				CodingRequestResult codingRequestResult;
				try
				{
					if (num != 0)
					{
						i = 2234528;
						goto IL_012C;
					}
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
					num2 = -1;
					IL_0094:
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						rdkssensorIDLearning.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
						goto IL_0164;
					}
					if ((int)item2[4] == rdkssensorIDLearning.targetPosition)
					{
						rdkssensorIDLearning.CurrentState = ((int)item2[0] * 256 * 256 * 256 + (int)item2[1] * 256 * 256 + (int)item2[2] * 256 + (int)item2[3]).ToString("000000000");
						codingRequestResult = CodingRequestResult.Success;
						goto IL_0164;
					}
					int num3 = i;
					i = num3 + 1;
					IL_012C:
					if (i > 2234531)
					{
						rdkssensorIDLearning.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						rdkssensorIDLearning.ReadModeAndAddress = i.ToString("X6");
						taskAwaiter = rdkssensorIDLearning.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, RDKSSensorIDLearning.<UpdateCurrentState>d__2>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0094;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0164:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005921 RID: 22817 RVA: 0x00428B04 File Offset: 0x00426D04
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003798 RID: 14232
			public int <>1__state;

			// Token: 0x04003799 RID: 14233
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400379A RID: 14234
			public RDKSSensorIDLearning <>4__this;

			// Token: 0x0400379B RID: 14235
			public string password;

			// Token: 0x0400379C RID: 14236
			private int <i>5__2;

			// Token: 0x0400379D RID: 14237
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
