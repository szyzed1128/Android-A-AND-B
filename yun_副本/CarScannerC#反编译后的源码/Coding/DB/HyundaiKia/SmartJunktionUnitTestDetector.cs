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
	// Token: 0x02000BC6 RID: 3014
	internal class SmartJunktionUnitTestDetector
	{
		// Token: 0x06005B13 RID: 23315 RVA: 0x00436D6C File Offset: 0x00434F6C
		public static async Task<List<CustomizableCodingTemplate>> GetSupportedActuators()
		{
			List<CustomizableCodingTemplate> result = new List<CustomizableCodingTemplate>();
			List<ValueItemWithTranslation> highList = PackageFileReader.DeserilzeFromEmbeddedFile<List<ValueItemWithTranslation>>("kiasmartjunction.db");
			OBDRequest obdrequest = new OBDRequest("1003", "771", "ATFCSH771;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
			OBDRequest obdrequest2 = new OBDRequest("2FBC0002", "771", "ATFCSH771;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
			OBDRequest obdrequest3 = new OBDRequest("2FBC2002", "771", "ATFCSH771;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
			bool req1_decoded = false;
			bool req2_decoded = false;
			obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					req1_decoded = true;
					foreach (int num in SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte x) => x.ReverseByte()).ToArray<byte>()))
					{
						string hexId = num.ToString("X2");
						ValueItemWithTranslation valueItemWithTranslation = highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == hexId);
						if (valueItemWithTranslation != null)
						{
							CustomizableCodingTemplate customizableCodingTemplate = SmartJunktionUnitTestDetector.CreateActuatorTestUDS(num, valueItemWithTranslation.Title);
							result.Add(customizableCodingTemplate);
						}
					}
				}
			};
			obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					req2_decoded = true;
					int[] array = (from x in SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte x) => x.ReverseByte()).ToArray<byte>())
						select x += 32).ToArray<int>();
					for (int i = 0; i < array.Length; i++)
					{
						int num2 = array[i];
						string hexId = num2.ToString("X2");
						ValueItemWithTranslation valueItemWithTranslation2 = highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == hexId);
						if (valueItemWithTranslation2 != null)
						{
							CustomizableCodingTemplate customizableCodingTemplate2 = SmartJunktionUnitTestDetector.CreateActuatorTestUDS(num2, valueItemWithTranslation2.Title);
							result.Add(customizableCodingTemplate2);
						}
					}
				}
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
			await App.OBDReader.WaitForCommandQueue();
			return result;
		}

		// Token: 0x06005B14 RID: 23316 RVA: 0x00436DA7 File Offset: 0x00434FA7
		public static CustomizableCodingTemplate CreateActuatorTestUDS(int id, string title)
		{
			return SupportedItemsDetectorBase.CreateActuatorTest(id, Translate.GetString("coding_ActuatorTest") + " - " + title, "771", "BC{0}03", "BC{0}00", "2F", "1003", CodingGroup.Other);
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x00002050 File Offset: 0x00000250
		public SmartJunktionUnitTestDetector()
		{
		}

		// Token: 0x02000BC7 RID: 3015
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005B16 RID: 23318 RVA: 0x00436DDE File Offset: 0x00434FDE
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005B17 RID: 23319 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005B18 RID: 23320 RVA: 0x004333D9 File Offset: 0x004315D9
			internal byte <GetSupportedActuators>b__0_2(byte x)
			{
				return x.ReverseByte();
			}

			// Token: 0x06005B19 RID: 23321 RVA: 0x004333D9 File Offset: 0x004315D9
			internal byte <GetSupportedActuators>b__0_4(byte x)
			{
				return x.ReverseByte();
			}

			// Token: 0x06005B1A RID: 23322 RVA: 0x00436DEA File Offset: 0x00434FEA
			internal int <GetSupportedActuators>b__0_5(int x)
			{
				return x += 32;
			}

			// Token: 0x0400396B RID: 14699
			public static readonly SmartJunktionUnitTestDetector.<>c <>9 = new SmartJunktionUnitTestDetector.<>c();

			// Token: 0x0400396C RID: 14700
			public static Func<byte, byte> <>9__0_2;

			// Token: 0x0400396D RID: 14701
			public static Func<byte, byte> <>9__0_4;

			// Token: 0x0400396E RID: 14702
			public static Func<int, int> <>9__0_5;
		}

		// Token: 0x02000BC8 RID: 3016
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06005B1B RID: 23323 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06005B1C RID: 23324 RVA: 0x00436DF4 File Offset: 0x00434FF4
			internal void <GetSupportedActuators>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.req1_decoded = true;
					foreach (int num in SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte x) => x.ReverseByte()).ToArray<byte>()))
					{
						SmartJunktionUnitTestDetector.<>c__DisplayClass0_1 CS$<>8__locals1 = new SmartJunktionUnitTestDetector.<>c__DisplayClass0_1();
						CS$<>8__locals1.hexId = num.ToString("X2");
						ValueItemWithTranslation valueItemWithTranslation = this.highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals1.hexId);
						if (valueItemWithTranslation != null)
						{
							CustomizableCodingTemplate customizableCodingTemplate = SmartJunktionUnitTestDetector.CreateActuatorTestUDS(num, valueItemWithTranslation.Title);
							this.result.Add(customizableCodingTemplate);
						}
					}
				}
			}

			// Token: 0x06005B1D RID: 23325 RVA: 0x00436ECC File Offset: 0x004350CC
			internal void <GetSupportedActuators>b__1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.req2_decoded = true;
					foreach (int num in (from x in SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte x) => x.ReverseByte()).ToArray<byte>())
						select x += 32).ToArray<int>())
					{
						SmartJunktionUnitTestDetector.<>c__DisplayClass0_2 CS$<>8__locals1 = new SmartJunktionUnitTestDetector.<>c__DisplayClass0_2();
						CS$<>8__locals1.hexId = num.ToString("X2");
						ValueItemWithTranslation valueItemWithTranslation = this.highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals1.hexId);
						if (valueItemWithTranslation != null)
						{
							CustomizableCodingTemplate customizableCodingTemplate = SmartJunktionUnitTestDetector.CreateActuatorTestUDS(num, valueItemWithTranslation.Title);
							this.result.Add(customizableCodingTemplate);
						}
					}
				}
			}

			// Token: 0x0400396F RID: 14703
			public bool req1_decoded;

			// Token: 0x04003970 RID: 14704
			public List<ValueItemWithTranslation> highList;

			// Token: 0x04003971 RID: 14705
			public List<CustomizableCodingTemplate> result;

			// Token: 0x04003972 RID: 14706
			public bool req2_decoded;
		}

		// Token: 0x02000BC9 RID: 3017
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_1
		{
			// Token: 0x06005B1E RID: 23326 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_1()
			{
			}

			// Token: 0x06005B1F RID: 23327 RVA: 0x00436FA9 File Offset: 0x004351A9
			internal bool <GetSupportedActuators>b__3(ValueItemWithTranslation x)
			{
				return x.Value == this.hexId;
			}

			// Token: 0x04003973 RID: 14707
			public string hexId;
		}

		// Token: 0x02000BCA RID: 3018
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_2
		{
			// Token: 0x06005B20 RID: 23328 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_2()
			{
			}

			// Token: 0x06005B21 RID: 23329 RVA: 0x00436FBC File Offset: 0x004351BC
			internal bool <GetSupportedActuators>b__6(ValueItemWithTranslation x)
			{
				return x.Value == this.hexId;
			}

			// Token: 0x04003974 RID: 14708
			public string hexId;
		}

		// Token: 0x02000BCB RID: 3019
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetSupportedActuators>d__0 : IAsyncStateMachine
		{
			// Token: 0x06005B22 RID: 23330 RVA: 0x00436FD0 File Offset: 0x004351D0
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
						CS$<>8__locals1 = new SmartJunktionUnitTestDetector.<>c__DisplayClass0_0();
						CS$<>8__locals1.result = new List<CustomizableCodingTemplate>();
						CS$<>8__locals1.highList = PackageFileReader.DeserilzeFromEmbeddedFile<List<ValueItemWithTranslation>>("kiasmartjunction.db");
						OBDRequest obdrequest = new OBDRequest("1003", "771", "ATFCSH771;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
						OBDRequest obdrequest2 = new OBDRequest("2FBC0002", "771", "ATFCSH771;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
						OBDRequest obdrequest3 = new OBDRequest("2FBC2002", "771", "ATFCSH771;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
						CS$<>8__locals1.req1_decoded = false;
						CS$<>8__locals1.req2_decoded = false;
						obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								SmartJunktionUnitTestDetector.<>c__DisplayClass0_1 CS$<>8__locals1;
								CS$<>8__locals1.req1_decoded = true;
								foreach (int num3 in SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte x) => x.ReverseByte()).ToArray<byte>()))
								{
									CS$<>8__locals1 = new SmartJunktionUnitTestDetector.<>c__DisplayClass0_1();
									CS$<>8__locals1.hexId = num3.ToString("X2");
									ValueItemWithTranslation valueItemWithTranslation = CS$<>8__locals1.highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals1.hexId);
									if (valueItemWithTranslation != null)
									{
										CustomizableCodingTemplate customizableCodingTemplate = SmartJunktionUnitTestDetector.CreateActuatorTestUDS(num3, valueItemWithTranslation.Title);
										CS$<>8__locals1.result.Add(customizableCodingTemplate);
									}
								}
							}
						};
						obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								CS$<>8__locals1.req2_decoded = true;
								foreach (int num4 in (from x in SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte x) => x.ReverseByte()).ToArray<byte>())
									select x += 32).ToArray<int>())
								{
									SmartJunktionUnitTestDetector.<>c__DisplayClass0_2 CS$<>8__locals2 = new SmartJunktionUnitTestDetector.<>c__DisplayClass0_2();
									CS$<>8__locals2.hexId = num4.ToString("X2");
									ValueItemWithTranslation valueItemWithTranslation2 = CS$<>8__locals1.highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals2.hexId);
									if (valueItemWithTranslation2 != null)
									{
										CustomizableCodingTemplate customizableCodingTemplate2 = SmartJunktionUnitTestDetector.CreateActuatorTestUDS(num4, valueItemWithTranslation2.Title);
										CS$<>8__locals1.result.Add(customizableCodingTemplate2);
									}
								}
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SmartJunktionUnitTestDetector.<GetSupportedActuators>d__0>(ref taskAwaiter, ref this);
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

			// Token: 0x06005B23 RID: 23331 RVA: 0x00437194 File Offset: 0x00435394
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003975 RID: 14709
			public int <>1__state;

			// Token: 0x04003976 RID: 14710
			public AsyncTaskMethodBuilder<List<CustomizableCodingTemplate>> <>t__builder;

			// Token: 0x04003977 RID: 14711
			private SmartJunktionUnitTestDetector.<>c__DisplayClass0_0 <>8__1;

			// Token: 0x04003978 RID: 14712
			private TaskAwaiter <>u__1;
		}
	}
}
