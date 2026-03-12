using System;
using System.Collections.Generic;
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
	// Token: 0x02000AF6 RID: 2806
	internal class MIB2PowerManagement : VIM_MIB2
	{
		// Token: 0x060057E9 RID: 22505 RVA: 0x0041EC98 File Offset: 0x0041CE98
		public MIB2PowerManagement(string name, string description, int byteId, int dataLength)
		{
			base.Address = 1088;
			base.DataLength = 18;
			base.DataLengthFormatLength = 3;
			base.AddressFormatLength = 3;
			base.Group = CodingGroup.Multimedia;
			base.Name = name;
			base.Description = description;
			base.InnerDescription = "This item makes changes to power management dataset in MIB2";
			this.byteId = byteId;
			this.parameterLengthBytes = dataLength;
		}

		// Token: 0x060057EA RID: 22506 RVA: 0x0041ECFC File Offset: 0x0041CEFC
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
					if (this.parameterLengthBytes == 1)
					{
						byte b = item2[this.byteId];
						base.CurrentState = b.ToString(CultureInfo.InvariantCulture);
					}
					else if (this.parameterLengthBytes == 2)
					{
						base.CurrentState = ((int)item2[this.byteId] * 256 + (int)item2[this.byteId + 1]).ToString(CultureInfo.InvariantCulture);
					}
				}
				codingRequestResult = item;
			}
			return codingRequestResult;
		}

		// Token: 0x060057EB RID: 22507 RVA: 0x0041ED50 File Offset: 0x0041CF50
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (this.LastReadData == null)
			{
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				int userInput = 0;
				if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out userInput))
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
						if (this.parameterLengthBytes == 1)
						{
							array[this.byteId] = (byte)userInput;
						}
						else if (this.parameterLengthBytes == 2)
						{
							int num = (userInput >> 8) & 255;
							int num2 = userInput & 255;
							array[this.byteId] = (byte)num;
							array[this.byteId + 1] = (byte)num2;
						}
						byte[] array2 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array);
						string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
						codingRequestResult = await this.WriteDataToECU(password, value, progress, this.LastReadData, text);
					}
				}
			}
			return codingRequestResult;
		}

		// Token: 0x060057EC RID: 22508 RVA: 0x0041EDAC File Offset: 0x0041CFAC
		public static List<ICodingContainer> GetMIB2PowerManagementCollection()
		{
			string text = "MIB2 power management: ";
			return new List<ICodingContainer>
			{
				new MIB2PowerManagement(text + "t_mmi_off_1", "0-254", 0, 1),
				new MIB2PowerManagement(text + "t_mmi_off_2", "0-254", 1, 1),
				new MIB2PowerManagement(text + "t_mmi_standby", "0-254", 2, 1),
				new MIB2PowerManagement(text + "t_pdd", "0-254", 3, 1),
				new MIB2PowerManagement(text + "t_mmi_on_customer_swdl", "0-254", 4, 1),
				new MIB2PowerManagement(text + "t_mmi_standby_pwr_save", "0-254", 5, 1),
				new MIB2PowerManagement(text + "t_bem_shutdown_suspend", "60-300", 6, 2),
				new MIB2PowerManagement(text + "t_sleep", "0-720", 8, 2),
				new MIB2PowerManagement(text + "t_mmi_on_tel", "0-120", 10, 2)
			};
		}

		// Token: 0x0400365A RID: 13914
		protected int byteId;

		// Token: 0x0400365B RID: 13915
		protected int parameterLengthBytes;

		// Token: 0x02000AF7 RID: 2807
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x060057ED RID: 22509 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x060057EE RID: 22510 RVA: 0x0041EECA File Offset: 0x0041D0CA
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x0400365C RID: 13916
			public bool engineIsRunning;
		}

		// Token: 0x02000AF8 RID: 2808
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__4 : IAsyncStateMachine
		{
			// Token: 0x060057EF RID: 22511 RVA: 0x0041EEF0 File Offset: 0x0041D0F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MIB2PowerManagement mib2PowerManagement = this;
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
							goto IL_022D;
						}
						CS$<>8__locals1 = new MIB2PowerManagement.<>c__DisplayClass4_0();
						if (mib2PowerManagement.LastReadData == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_0257;
						}
						userInput = 0;
						if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out userInput))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_0257;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MIB2PowerManagement.<Execute>d__4>(ref taskAwaiter3, ref this);
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
						goto IL_0257;
					}
					byte[] array = new byte[mib2PowerManagement.LastReadData.Length - 2];
					Array.Copy(mib2PowerManagement.LastReadData, 0, array, 0, array.Length);
					if (mib2PowerManagement.parameterLengthBytes == 1)
					{
						array[mib2PowerManagement.byteId] = (byte)userInput;
					}
					else if (mib2PowerManagement.parameterLengthBytes == 2)
					{
						int num3 = (userInput >> 8) & 255;
						int num4 = userInput & 255;
						array[mib2PowerManagement.byteId] = (byte)num3;
						array[mib2PowerManagement.byteId + 1] = (byte)num4;
					}
					byte[] array2 = new Crc16Ccitt(InitialCrcValue.NonZero1).ComputeChecksumBytes(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					taskAwaiter = mib2PowerManagement.WriteDataToECU(password, value, progress, mib2PowerManagement.LastReadData, text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MIB2PowerManagement.<Execute>d__4>(ref taskAwaiter, ref this);
						return;
					}
					IL_022D:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0257:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060057F0 RID: 22512 RVA: 0x0041F18C File Offset: 0x0041D38C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400365D RID: 13917
			public int <>1__state;

			// Token: 0x0400365E RID: 13918
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400365F RID: 13919
			public MIB2PowerManagement <>4__this;

			// Token: 0x04003660 RID: 13920
			public string value;

			// Token: 0x04003661 RID: 13921
			private MIB2PowerManagement.<>c__DisplayClass4_0 <>8__1;

			// Token: 0x04003662 RID: 13922
			public string password;

			// Token: 0x04003663 RID: 13923
			public IProgress<string> progress;

			// Token: 0x04003664 RID: 13924
			private int <userInput>5__2;

			// Token: 0x04003665 RID: 13925
			private TaskAwaiter <>u__1;

			// Token: 0x04003666 RID: 13926
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000AF9 RID: 2809
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__3 : IAsyncStateMachine
		{
			// Token: 0x060057F1 RID: 22513 RVA: 0x0041F19C File Offset: 0x0041D39C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MIB2PowerManagement mib2PowerManagement = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(password))
						{
							password = mib2PowerManagement.Password;
						}
						taskAwaiter = mib2PowerManagement.GetCurrentStateRawData(password, progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MIB2PowerManagement.<UpdateCurrentState>d__3>(ref taskAwaiter, ref this);
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
						mib2PowerManagement.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						mib2PowerManagement.LastReadData = item2;
						if (item2 != null)
						{
							if (mib2PowerManagement.parameterLengthBytes == 1)
							{
								byte b = item2[mib2PowerManagement.byteId];
								mib2PowerManagement.CurrentState = b.ToString(CultureInfo.InvariantCulture);
							}
							else if (mib2PowerManagement.parameterLengthBytes == 2)
							{
								mib2PowerManagement.CurrentState = ((int)item2[mib2PowerManagement.byteId] * 256 + (int)item2[mib2PowerManagement.byteId + 1]).ToString(CultureInfo.InvariantCulture);
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

			// Token: 0x060057F2 RID: 22514 RVA: 0x0041F31C File Offset: 0x0041D51C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003667 RID: 13927
			public int <>1__state;

			// Token: 0x04003668 RID: 13928
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003669 RID: 13929
			public string password;

			// Token: 0x0400366A RID: 13930
			public MIB2PowerManagement <>4__this;

			// Token: 0x0400366B RID: 13931
			public IProgress<string> progress;

			// Token: 0x0400366C RID: 13932
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
