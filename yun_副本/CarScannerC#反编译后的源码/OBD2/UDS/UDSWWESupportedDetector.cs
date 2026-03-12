using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.ECUModels;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.UDS
{
	// Token: 0x020003C4 RID: 964
	internal class UDSWWESupportedDetector
	{
		// Token: 0x060027A2 RID: 10146 RVA: 0x00002050 File Offset: 0x00000250
		public UDSWWESupportedDetector(IECU ecu)
		{
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x001E8500 File Offset: 0x001E6700
		public List<string> DecodeSupported(string command, byte[] data)
		{
			int num = BitHelpers.ConvertHexToInt(command) + 1;
			bool[] array = new bool[32];
			for (int i = 0; i < 4; i++)
			{
				BitArrayReverse bitArrayReverse = new BitArrayReverse(new BitArray(new byte[] { data[i] }));
				bool[] array2 = new bool[8];
				for (int j = 0; j < 8; j++)
				{
					array2[j] = bitArrayReverse[j];
				}
				Array.Copy(array2, 0, array, i * 8, 8);
			}
			List<string> list = new List<string>();
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k])
				{
					list.Add((num + k).ToString("X6"));
				}
			}
			return list;
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x001E85AC File Offset: 0x001E67AC
		public static async Task<List<string>> DetectSupportedIdsForHeader(string pattern, string header, string beforeCommands, string afterCommands)
		{
			int counter = 0;
			List<string> result = new List<string>();
			bool nextPartIsSupported = false;
			ResponseDecodedDelegate <>9__0;
			do
			{
				nextPartIsSupported = false;
				OBDRequest obdrequest = new OBDRequest(string.Format(pattern, counter.ToString("X2")), header, beforeCommands, afterCommands, false);
				OBDRequest obdrequest2 = obdrequest;
				ResponseDecodedDelegate responseDecodedDelegate;
				if ((responseDecodedDelegate = <>9__0) == null)
				{
					responseDecodedDelegate = (<>9__0 = delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data.Length >= 4)
						{
							bool[] array = new bool[32];
							for (int i = 0; i < 4; i++)
							{
								BitArrayReverse bitArrayReverse = new BitArrayReverse(new BitArray(new byte[] { data[i] }));
								bool[] array2 = new bool[8];
								for (int j = 0; j < 8; j++)
								{
									array2[j] = bitArrayReverse[j];
								}
								Array.Copy(array2, 0, array, i * 8, 8);
							}
							for (int k = counter + 1; k < array.Length - 1; k++)
							{
								if (array[k])
								{
									result.Add(k.ToString("X2"));
								}
							}
							nextPartIsSupported = array[array.Length - 1];
						}
					});
				}
				obdrequest2.ResponseDecoded += responseDecodedDelegate;
				App.OBDReader.ReplaceQueue(obdrequest);
				await App.OBDReader.WaitForCommandQueue();
				counter += 32;
			}
			while (nextPartIsSupported);
			return result;
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x001E8608 File Offset: 0x001E6808
		public static List<CustomPID> BuildPidsFromSupportedList(List<string> supportedList, string pattern, string header, string beforeCommands, string afterCommands)
		{
			List<CustomPID> list = new List<CustomPID>();
			int num = 10000;
			if (header.Length == 3)
			{
				num = int.Parse(header, NumberStyles.HexNumber);
			}
			else if (header.Length > 3)
			{
				num = int.Parse(new string(header.TakeLast(4).ToArray<char>()), NumberStyles.HexNumber);
			}
			for (int i = 0; i < supportedList.Count; i++)
			{
				string text = supportedList[i];
				string mode01cmd = SharedSettings.Current.Mode01Prefix + text;
				string text2 = string.Format(pattern, text);
				foreach (PIDWithFloatValueFormula pidwithFloatValueFormula in from x in App.OBDReader.CurrentCarData.LiveDataPIDs
					where x is PIDWithFloatValueFormula && x.Command == mode01cmd
					select (PIDWithFloatValueFormula)x)
				{
					CustomPID customPID = new CustomPID(pidwithFloatValueFormula.Name, pidwithFloatValueFormula.ShortName, text2, header, "", pidwithFloatValueFormula.Units, pidwithFloatValueFormula.Minimum, pidwithFloatValueFormula.Maximum, beforeCommands, afterCommands, false, pidwithFloatValueFormula.Role, CustomPIDType.InternalOBD2, 0, 0, 0.0, 0.0, 0.0, false, true, true, null);
					customPID.SetInternalFormual(pidwithFloatValueFormula.GetFormula());
					list.Add(customPID);
					customPID.Id = 10000 + num + i;
				}
			}
			return list;
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x001E87B0 File Offset: 0x001E69B0
		public static async Task<List<CustomPID>> DetectSupportedPidsForHeader(string pattern, string header, string beforeCommands, string afterCommands)
		{
			return UDSWWESupportedDetector.BuildPidsFromSupportedList(await UDSWWESupportedDetector.DetectSupportedIdsForHeader(pattern, header, beforeCommands, afterCommands), pattern, header, beforeCommands, afterCommands);
		}

		// Token: 0x020003C5 RID: 965
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060027A7 RID: 10151 RVA: 0x001E880B File Offset: 0x001E6A0B
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060027A8 RID: 10152 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060027A9 RID: 10153 RVA: 0x001E8817 File Offset: 0x001E6A17
			internal PIDWithFloatValueFormula <BuildPidsFromSupportedList>b__3_1(PID x)
			{
				return (PIDWithFloatValueFormula)x;
			}

			// Token: 0x04001625 RID: 5669
			public static readonly UDSWWESupportedDetector.<>c <>9 = new UDSWWESupportedDetector.<>c();

			// Token: 0x04001626 RID: 5670
			public static Func<PID, PIDWithFloatValueFormula> <>9__3_1;
		}

		// Token: 0x020003C6 RID: 966
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060027AA RID: 10154 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060027AB RID: 10155 RVA: 0x001E8820 File Offset: 0x001E6A20
			internal void <DetectSupportedIdsForHeader>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data.Length >= 4)
				{
					bool[] array = new bool[32];
					for (int i = 0; i < 4; i++)
					{
						BitArrayReverse bitArrayReverse = new BitArrayReverse(new BitArray(new byte[] { data[i] }));
						bool[] array2 = new bool[8];
						for (int j = 0; j < 8; j++)
						{
							array2[j] = bitArrayReverse[j];
						}
						Array.Copy(array2, 0, array, i * 8, 8);
					}
					for (int k = this.counter + 1; k < array.Length - 1; k++)
					{
						if (array[k])
						{
							this.result.Add(k.ToString("X2"));
						}
					}
					this.nextPartIsSupported = array[array.Length - 1];
				}
			}

			// Token: 0x04001627 RID: 5671
			public int counter;

			// Token: 0x04001628 RID: 5672
			public List<string> result;

			// Token: 0x04001629 RID: 5673
			public bool nextPartIsSupported;

			// Token: 0x0400162A RID: 5674
			public ResponseDecodedDelegate <>9__0;
		}

		// Token: 0x020003C7 RID: 967
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060027AC RID: 10156 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060027AD RID: 10157 RVA: 0x001E88D5 File Offset: 0x001E6AD5
			internal bool <BuildPidsFromSupportedList>b__0(PID x)
			{
				return x is PIDWithFloatValueFormula && x.Command == this.mode01cmd;
			}

			// Token: 0x0400162B RID: 5675
			public string mode01cmd;
		}

		// Token: 0x020003C8 RID: 968
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DetectSupportedIdsForHeader>d__2 : IAsyncStateMachine
		{
			// Token: 0x060027AE RID: 10158 RVA: 0x001E88F4 File Offset: 0x001E6AF4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				List<string> result;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num == 0)
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_011F;
					}
					CS$<>8__locals1 = new UDSWWESupportedDetector.<>c__DisplayClass2_0();
					CS$<>8__locals1.counter = 0;
					CS$<>8__locals1.result = new List<string>();
					CS$<>8__locals1.nextPartIsSupported = false;
					IL_0040:
					CS$<>8__locals1.nextPartIsSupported = false;
					OBDRequest obdrequest = new OBDRequest(string.Format(pattern, CS$<>8__locals1.counter.ToString("X2")), header, beforeCommands, afterCommands, false);
					OBDRequest obdrequest2 = obdrequest;
					ResponseDecodedDelegate responseDecodedDelegate;
					if ((responseDecodedDelegate = CS$<>8__locals1.<>9__0) == null)
					{
						responseDecodedDelegate = (CS$<>8__locals1.<>9__0 = delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data.Length >= 4)
							{
								bool[] array = new bool[32];
								for (int i = 0; i < 4; i++)
								{
									BitArrayReverse bitArrayReverse = new BitArrayReverse(new BitArray(new byte[] { data[i] }));
									bool[] array2 = new bool[8];
									for (int j = 0; j < 8; j++)
									{
										array2[j] = bitArrayReverse[j];
									}
									Array.Copy(array2, 0, array, i * 8, 8);
								}
								for (int k = CS$<>8__locals1.counter + 1; k < array.Length - 1; k++)
								{
									if (array[k])
									{
										CS$<>8__locals1.result.Add(k.ToString("X2"));
									}
								}
								CS$<>8__locals1.nextPartIsSupported = array[array.Length - 1];
							}
						});
					}
					obdrequest2.ResponseDecoded += responseDecodedDelegate;
					App.OBDReader.ReplaceQueue(obdrequest);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 0;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UDSWWESupportedDetector.<DetectSupportedIdsForHeader>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_011F:
					taskAwaiter.GetResult();
					CS$<>8__locals1.counter += 32;
					if (CS$<>8__locals1.nextPartIsSupported)
					{
						goto IL_0040;
					}
					result = CS$<>8__locals1.result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x060027AF RID: 10159 RVA: 0x001E8AB8 File Offset: 0x001E6CB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400162C RID: 5676
			public int <>1__state;

			// Token: 0x0400162D RID: 5677
			public AsyncTaskMethodBuilder<List<string>> <>t__builder;

			// Token: 0x0400162E RID: 5678
			private UDSWWESupportedDetector.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x0400162F RID: 5679
			public string pattern;

			// Token: 0x04001630 RID: 5680
			public string header;

			// Token: 0x04001631 RID: 5681
			public string beforeCommands;

			// Token: 0x04001632 RID: 5682
			public string afterCommands;

			// Token: 0x04001633 RID: 5683
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020003C9 RID: 969
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DetectSupportedPidsForHeader>d__4 : IAsyncStateMachine
		{
			// Token: 0x060027B0 RID: 10160 RVA: 0x001E8AC8 File Offset: 0x001E6CC8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				List<CustomPID> list;
				try
				{
					TaskAwaiter<List<string>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = UDSWWESupportedDetector.DetectSupportedIdsForHeader(pattern, header, beforeCommands, afterCommands).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<List<string>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<string>>, UDSWWESupportedDetector.<DetectSupportedPidsForHeader>d__4>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<List<string>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<List<string>>);
						num2 = -1;
					}
					list = UDSWWESupportedDetector.BuildPidsFromSupportedList(taskAwaiter.GetResult(), pattern, header, beforeCommands, afterCommands);
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(list);
			}

			// Token: 0x060027B1 RID: 10161 RVA: 0x001E8BAC File Offset: 0x001E6DAC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001634 RID: 5684
			public int <>1__state;

			// Token: 0x04001635 RID: 5685
			public AsyncTaskMethodBuilder<List<CustomPID>> <>t__builder;

			// Token: 0x04001636 RID: 5686
			public string pattern;

			// Token: 0x04001637 RID: 5687
			public string header;

			// Token: 0x04001638 RID: 5688
			public string beforeCommands;

			// Token: 0x04001639 RID: 5689
			public string afterCommands;

			// Token: 0x0400163A RID: 5690
			private TaskAwaiter<List<string>> <>u__1;
		}
	}
}
