using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BB9 RID: 3001
	internal class KiaBCMSupportedDetector
	{
		// Token: 0x06005AF1 RID: 23281 RVA: 0x004362B4 File Offset: 0x004344B4
		public static async Task<List<CustomizableCodingTemplate>> GetSupportedIds(List<CustomizableCodingTemplate> allCodings)
		{
			OBDRequest obdrequest = new OBDRequest("22B001", "7A0", "1003", "", false);
			List<CustomizableCodingTemplate> result = new List<CustomizableCodingTemplate>(0);
			obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				result = KiaBCMSupportedDetector.ParseResponse(allCodings, data);
			};
			App.OBDReader.ReplaceQueue(obdrequest);
			await App.OBDReader.WaitForCommandQueue();
			return result;
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x004362F8 File Offset: 0x004344F8
		private static List<CustomizableCodingTemplate> ParseResponse(List<CustomizableCodingTemplate> allCodings, byte[] data)
		{
			if (data == null || data.Length < 16)
			{
				return new List<CustomizableCodingTemplate>(0);
			}
			List<CustomizableCodingTemplate> list = new List<CustomizableCodingTemplate>();
			try
			{
				byte[] array = new byte[4];
				Array.Copy(data, 0, array, 0, 4);
				bool[] array2 = new bool[32];
				for (int i = 0; i < array.Length; i++)
				{
					for (int j = 7; j >= 0; j--)
					{
						bool bit_0_ = BitHelpers.GetBit_0_7(array[i], j);
						array2[i * 8 + (7 - j)] = bit_0_;
					}
				}
				for (int k = 0; k < array2.Length; k++)
				{
					if (array2[k])
					{
						string pattern = "12A0" + (k + 1).ToString("X2");
						Func<MQBAdaptationOption, bool> <>9__1;
						CustomizableCodingTemplate[] array3 = allCodings.Where(delegate(CustomizableCodingTemplate coding)
						{
							IEnumerable<MQBAdaptationOption> options = coding.Options;
							Func<MQBAdaptationOption, bool> func;
							if ((func = <>9__1) == null)
							{
								func = (<>9__1 = (MQBAdaptationOption opt) => opt.Value.StartsWith(pattern));
							}
							return options.Any(func);
						}).ToArray<CustomizableCodingTemplate>();
						list.AddRange(array3);
					}
				}
				foreach (CustomizableCodingTemplate customizableCodingTemplate in list)
				{
					customizableCodingTemplate.InnerDescription += SupportedItemsDetectorBase.GetAccessKeyWarning;
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x00002050 File Offset: 0x00000250
		public KiaBCMSupportedDetector()
		{
		}

		// Token: 0x02000BBA RID: 3002
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06005AF4 RID: 23284 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06005AF5 RID: 23285 RVA: 0x00436430 File Offset: 0x00434630
			internal void <GetSupportedIds>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				this.result = KiaBCMSupportedDetector.ParseResponse(this.allCodings, data);
			}

			// Token: 0x04003954 RID: 14676
			public List<CustomizableCodingTemplate> result;

			// Token: 0x04003955 RID: 14677
			public List<CustomizableCodingTemplate> allCodings;
		}

		// Token: 0x02000BBB RID: 3003
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06005AF6 RID: 23286 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06005AF7 RID: 23287 RVA: 0x00436444 File Offset: 0x00434644
			internal bool <ParseResponse>b__0(CustomizableCodingTemplate coding)
			{
				IEnumerable<MQBAdaptationOption> options = coding.Options;
				Func<MQBAdaptationOption, bool> func;
				if ((func = this.<>9__1) == null)
				{
					func = (this.<>9__1 = (MQBAdaptationOption opt) => opt.Value.StartsWith(this.pattern));
				}
				return options.Any(func);
			}

			// Token: 0x06005AF8 RID: 23288 RVA: 0x0043647B File Offset: 0x0043467B
			internal bool <ParseResponse>b__1(MQBAdaptationOption opt)
			{
				return opt.Value.StartsWith(this.pattern);
			}

			// Token: 0x04003956 RID: 14678
			public string pattern;

			// Token: 0x04003957 RID: 14679
			public Func<MQBAdaptationOption, bool> <>9__1;
		}

		// Token: 0x02000BBC RID: 3004
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetSupportedIds>d__0 : IAsyncStateMachine
		{
			// Token: 0x06005AF9 RID: 23289 RVA: 0x00436490 File Offset: 0x00434690
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				List<CustomizableCodingTemplate> result;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new KiaBCMSupportedDetector.<>c__DisplayClass0_0();
						CS$<>8__locals1.allCodings = allCodings;
						OBDRequest obdrequest = new OBDRequest("22B001", "7A0", "1003", "", false);
						CS$<>8__locals1.result = new List<CustomizableCodingTemplate>(0);
						obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							CS$<>8__locals1.result = KiaBCMSupportedDetector.ParseResponse(CS$<>8__locals1.allCodings, data);
						};
						App.OBDReader.ReplaceQueue(obdrequest);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, KiaBCMSupportedDetector.<GetSupportedIds>d__0>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
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

			// Token: 0x06005AFA RID: 23290 RVA: 0x004365CC File Offset: 0x004347CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003958 RID: 14680
			public int <>1__state;

			// Token: 0x04003959 RID: 14681
			public AsyncTaskMethodBuilder<List<CustomizableCodingTemplate>> <>t__builder;

			// Token: 0x0400395A RID: 14682
			public List<CustomizableCodingTemplate> allCodings;

			// Token: 0x0400395B RID: 14683
			private KiaBCMSupportedDetector.<>c__DisplayClass0_0 <>8__1;

			// Token: 0x0400395C RID: 14684
			private TaskAwaiter <>u__1;
		}
	}
}
