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
	// Token: 0x02000B93 RID: 2963
	internal class EngineActuatorTestsDetector
	{
		// Token: 0x06005A7E RID: 23166 RVA: 0x00433324 File Offset: 0x00431524
		public static async Task<List<CustomizableCodingTemplate>> GetSupportedActuators()
		{
			List<CustomizableCodingTemplate> result = new List<CustomizableCodingTemplate>();
			List<ValueItemWithTranslation> highList = PackageFileReader.DeserilzeFromEmbeddedFile<List<ValueItemWithTranslation>>("kiaengine2124actuators.db");
			bool has2124response = false;
			OBDRequest obdrequest = new OBDRequest("1090", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
			OBDRequest obdrequest2 = new OBDRequest("2124", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
			obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				foreach (int num in SupportedItemsDetectorBase.GetSupportedIds(data))
				{
					string hexId = num.ToString("X2");
					ValueItemWithTranslation valueItemWithTranslation = highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == hexId);
					if (valueItemWithTranslation != null)
					{
						CustomizableCodingTemplate customizableCodingTemplate = EngineActuatorTestsDetector.CreateActuatorTestFor2124(num, valueItemWithTranslation.Title);
						result.Add(customizableCodingTemplate);
					}
				}
				has2124response = true;
			};
			OBDRequest obdrequest3 = new OBDRequest("22E024", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
			obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				foreach (int num2 in SupportedItemsDetectorBase.GetSupportedIds(data))
				{
					string hexId = num2.ToString("X2");
					ValueItemWithTranslation valueItemWithTranslation2 = highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == hexId);
					if (valueItemWithTranslation2 != null)
					{
						CustomizableCodingTemplate customizableCodingTemplate2 = EngineActuatorTestsDetector.CreateActuatorTestFor22E024(num2, valueItemWithTranslation2.Title);
						result.Add(customizableCodingTemplate2);
					}
				}
				has2124response = true;
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
			await App.OBDReader.WaitForCommandQueue();
			if (!has2124response)
			{
				List<ValueItemWithTranslation> lowList = PackageFileReader.DeserilzeFromEmbeddedFile<List<ValueItemWithTranslation>>("kiaenginelowactuators.db");
				OBDRequest obdrequest4 = new OBDRequest("1090", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
				int counter = 0;
				OBDRequest obdrequest5 = new OBDRequest("30000A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
				OBDRequest obdrequest6 = new OBDRequest("30200A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
				OBDRequest obdrequest7 = new OBDRequest("30400A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
				OBDRequest obdrequest8 = new OBDRequest("30600A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
				OBDRequest obdrequest9 = new OBDRequest("30800A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
				OBDRequest obdrequest10 = new OBDRequest("30A00A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
				bool hasReqData = false;
				Func<int, int> <>9__6;
				ResponseDecodedDelegate responseDecodedDelegate = delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data != null && data.Length != 0)
					{
						IEnumerable<int> supportedIds = SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte b) => b.ReverseByte()).ToArray<byte>());
						Func<int, int> func;
						if ((func = <>9__6) == null)
						{
							func = (<>9__6 = (int x) => x + counter);
						}
						foreach (int num3 in supportedIds.Select(func))
						{
							string hexId = num3.ToString("X2");
							ValueItemWithTranslation valueItemWithTranslation3 = lowList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == hexId);
							if (valueItemWithTranslation3 != null)
							{
								CustomizableCodingTemplate customizableCodingTemplate3 = EngineActuatorTestsDetector.CreateActuatorTestFor2124(num3, valueItemWithTranslation3.Title);
								result.Add(customizableCodingTemplate3);
							}
						}
						hasReqData = true;
					}
					counter += 32;
				};
				obdrequest5.ResponseDecoded += responseDecodedDelegate;
				obdrequest6.ResponseDecoded += responseDecodedDelegate;
				obdrequest7.ResponseDecoded += responseDecodedDelegate;
				obdrequest8.ResponseDecoded += responseDecodedDelegate;
				obdrequest9.ResponseDecoded += responseDecodedDelegate;
				obdrequest10.ResponseDecoded += responseDecodedDelegate;
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4, obdrequest5, obdrequest6, obdrequest7, obdrequest8, obdrequest9, obdrequest10 });
				await App.OBDReader.WaitForCommandQueue();
			}
			return result;
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x0043335F File Offset: 0x0043155F
		public static CustomizableCodingTemplate CreateActuatorTestFor2124(int id, string title)
		{
			return SupportedItemsDetectorBase.CreateActuatorTest(id, Translate.GetString("coding_ActuatorTest") + " - " + title, "7E0", "{0}07FF", "{0}0700", "30", "1090", CodingGroup.EngineAndPowertrain);
		}

		// Token: 0x06005A80 RID: 23168 RVA: 0x00433396 File Offset: 0x00431596
		public static CustomizableCodingTemplate CreateActuatorTestFor22E024(int id, string title)
		{
			return SupportedItemsDetectorBase.CreateActuatorTest(id, Translate.GetString("coding_ActuatorTest") + " - " + title, "7E0", "F0{0}03FF", "F0{0}0300", "2F", "1090", CodingGroup.EngineAndPowertrain);
		}

		// Token: 0x06005A81 RID: 23169 RVA: 0x00002050 File Offset: 0x00000250
		public EngineActuatorTestsDetector()
		{
		}

		// Token: 0x02000B94 RID: 2964
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005A82 RID: 23170 RVA: 0x004333CD File Offset: 0x004315CD
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005A83 RID: 23171 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005A84 RID: 23172 RVA: 0x004333D9 File Offset: 0x004315D9
			internal byte <GetSupportedActuators>b__0_5(byte b)
			{
				return b.ReverseByte();
			}

			// Token: 0x040038D8 RID: 14552
			public static readonly EngineActuatorTestsDetector.<>c <>9 = new EngineActuatorTestsDetector.<>c();

			// Token: 0x040038D9 RID: 14553
			public static Func<byte, byte> <>9__0_5;
		}

		// Token: 0x02000B95 RID: 2965
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06005A85 RID: 23173 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06005A86 RID: 23174 RVA: 0x004333E4 File Offset: 0x004315E4
			internal void <GetSupportedActuators>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				foreach (int num in SupportedItemsDetectorBase.GetSupportedIds(data))
				{
					EngineActuatorTestsDetector.<>c__DisplayClass0_1 CS$<>8__locals1 = new EngineActuatorTestsDetector.<>c__DisplayClass0_1();
					CS$<>8__locals1.hexId = num.ToString("X2");
					ValueItemWithTranslation valueItemWithTranslation = this.highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals1.hexId);
					if (valueItemWithTranslation != null)
					{
						CustomizableCodingTemplate customizableCodingTemplate = EngineActuatorTestsDetector.CreateActuatorTestFor2124(num, valueItemWithTranslation.Title);
						this.result.Add(customizableCodingTemplate);
					}
				}
				this.has2124response = true;
			}

			// Token: 0x06005A87 RID: 23175 RVA: 0x00433488 File Offset: 0x00431688
			internal void <GetSupportedActuators>b__1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				foreach (int num in SupportedItemsDetectorBase.GetSupportedIds(data))
				{
					EngineActuatorTestsDetector.<>c__DisplayClass0_2 CS$<>8__locals1 = new EngineActuatorTestsDetector.<>c__DisplayClass0_2();
					CS$<>8__locals1.hexId = num.ToString("X2");
					ValueItemWithTranslation valueItemWithTranslation = this.highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals1.hexId);
					if (valueItemWithTranslation != null)
					{
						CustomizableCodingTemplate customizableCodingTemplate = EngineActuatorTestsDetector.CreateActuatorTestFor22E024(num, valueItemWithTranslation.Title);
						this.result.Add(customizableCodingTemplate);
					}
				}
				this.has2124response = true;
			}

			// Token: 0x040038DA RID: 14554
			public List<ValueItemWithTranslation> highList;

			// Token: 0x040038DB RID: 14555
			public List<CustomizableCodingTemplate> result;

			// Token: 0x040038DC RID: 14556
			public bool has2124response;
		}

		// Token: 0x02000B96 RID: 2966
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_1
		{
			// Token: 0x06005A88 RID: 23176 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_1()
			{
			}

			// Token: 0x06005A89 RID: 23177 RVA: 0x0043352C File Offset: 0x0043172C
			internal bool <GetSupportedActuators>b__2(ValueItemWithTranslation x)
			{
				return x.Value == this.hexId;
			}

			// Token: 0x040038DD RID: 14557
			public string hexId;
		}

		// Token: 0x02000B97 RID: 2967
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_2
		{
			// Token: 0x06005A8A RID: 23178 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_2()
			{
			}

			// Token: 0x06005A8B RID: 23179 RVA: 0x0043353F File Offset: 0x0043173F
			internal bool <GetSupportedActuators>b__3(ValueItemWithTranslation x)
			{
				return x.Value == this.hexId;
			}

			// Token: 0x040038DE RID: 14558
			public string hexId;
		}

		// Token: 0x02000B98 RID: 2968
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_3
		{
			// Token: 0x06005A8C RID: 23180 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_3()
			{
			}

			// Token: 0x06005A8D RID: 23181 RVA: 0x00433554 File Offset: 0x00431754
			internal void <GetSupportedActuators>b__4(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					IEnumerable<int> supportedIds = SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte b) => b.ReverseByte()).ToArray<byte>());
					Func<int, int> func;
					if ((func = this.<>9__6) == null)
					{
						func = (this.<>9__6 = (int x) => x + this.counter);
					}
					foreach (int num in supportedIds.Select(func))
					{
						EngineActuatorTestsDetector.<>c__DisplayClass0_4 CS$<>8__locals1 = new EngineActuatorTestsDetector.<>c__DisplayClass0_4();
						CS$<>8__locals1.hexId = num.ToString("X2");
						ValueItemWithTranslation valueItemWithTranslation = this.lowList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals1.hexId);
						if (valueItemWithTranslation != null)
						{
							CustomizableCodingTemplate customizableCodingTemplate = EngineActuatorTestsDetector.CreateActuatorTestFor2124(num, valueItemWithTranslation.Title);
							this.CS$<>8__locals1.result.Add(customizableCodingTemplate);
						}
					}
					this.hasReqData = true;
				}
				this.counter += 32;
			}

			// Token: 0x06005A8E RID: 23182 RVA: 0x00433660 File Offset: 0x00431860
			internal int <GetSupportedActuators>b__6(int x)
			{
				return x + this.counter;
			}

			// Token: 0x040038DF RID: 14559
			public int counter;

			// Token: 0x040038E0 RID: 14560
			public List<ValueItemWithTranslation> lowList;

			// Token: 0x040038E1 RID: 14561
			public bool hasReqData;

			// Token: 0x040038E2 RID: 14562
			public EngineActuatorTestsDetector.<>c__DisplayClass0_0 CS$<>8__locals1;

			// Token: 0x040038E3 RID: 14563
			public Func<int, int> <>9__6;
		}

		// Token: 0x02000B99 RID: 2969
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_4
		{
			// Token: 0x06005A8F RID: 23183 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_4()
			{
			}

			// Token: 0x06005A90 RID: 23184 RVA: 0x0043366A File Offset: 0x0043186A
			internal bool <GetSupportedActuators>b__7(ValueItemWithTranslation x)
			{
				return x.Value == this.hexId;
			}

			// Token: 0x040038E4 RID: 14564
			public string hexId;
		}

		// Token: 0x02000B9A RID: 2970
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetSupportedActuators>d__0 : IAsyncStateMachine
		{
			// Token: 0x06005A91 RID: 23185 RVA: 0x00433680 File Offset: 0x00431880
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
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0321;
						}
						CS$<>8__locals1 = new EngineActuatorTestsDetector.<>c__DisplayClass0_0();
						CS$<>8__locals1.result = new List<CustomizableCodingTemplate>();
						CS$<>8__locals1.highList = PackageFileReader.DeserilzeFromEmbeddedFile<List<ValueItemWithTranslation>>("kiaengine2124actuators.db");
						CS$<>8__locals1.has2124response = false;
						OBDRequest obdrequest = new OBDRequest("1090", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
						OBDRequest obdrequest2 = new OBDRequest("2124", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
						obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							foreach (int num3 in SupportedItemsDetectorBase.GetSupportedIds(data))
							{
								EngineActuatorTestsDetector.<>c__DisplayClass0_1 CS$<>8__locals3 = new EngineActuatorTestsDetector.<>c__DisplayClass0_1();
								CS$<>8__locals3.hexId = num3.ToString("X2");
								ValueItemWithTranslation valueItemWithTranslation = CS$<>8__locals1.highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals3.hexId);
								if (valueItemWithTranslation != null)
								{
									CustomizableCodingTemplate customizableCodingTemplate = EngineActuatorTestsDetector.CreateActuatorTestFor2124(num3, valueItemWithTranslation.Title);
									CS$<>8__locals1.result.Add(customizableCodingTemplate);
								}
							}
							CS$<>8__locals1.has2124response = true;
						};
						OBDRequest obdrequest3 = new OBDRequest("22E024", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
						obdrequest3.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							foreach (int num4 in SupportedItemsDetectorBase.GetSupportedIds(data))
							{
								EngineActuatorTestsDetector.<>c__DisplayClass0_2 CS$<>8__locals4 = new EngineActuatorTestsDetector.<>c__DisplayClass0_2();
								CS$<>8__locals4.hexId = num4.ToString("X2");
								ValueItemWithTranslation valueItemWithTranslation2 = CS$<>8__locals1.highList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals4.hexId);
								if (valueItemWithTranslation2 != null)
								{
									CustomizableCodingTemplate customizableCodingTemplate2 = EngineActuatorTestsDetector.CreateActuatorTestFor22E024(num4, valueItemWithTranslation2.Title);
									CS$<>8__locals1.result.Add(customizableCodingTemplate2);
								}
							}
							CS$<>8__locals1.has2124response = true;
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, EngineActuatorTestsDetector.<GetSupportedActuators>d__0>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.has2124response)
					{
						goto IL_0328;
					}
					EngineActuatorTestsDetector.<>c__DisplayClass0_3 CS$<>8__locals2 = new EngineActuatorTestsDetector.<>c__DisplayClass0_3();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.lowList = PackageFileReader.DeserilzeFromEmbeddedFile<List<ValueItemWithTranslation>>("kiaenginelowactuators.db");
					OBDRequest obdrequest4 = new OBDRequest("1090", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
					CS$<>8__locals2.counter = 0;
					OBDRequest obdrequest5 = new OBDRequest("30000A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
					OBDRequest obdrequest6 = new OBDRequest("30200A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
					OBDRequest obdrequest7 = new OBDRequest("30400A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
					OBDRequest obdrequest8 = new OBDRequest("30600A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
					OBDRequest obdrequest9 = new OBDRequest("30800A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
					OBDRequest obdrequest10 = new OBDRequest("30A00A", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1", "ATFCSM0", false);
					CS$<>8__locals2.hasReqData = false;
					ResponseDecodedDelegate responseDecodedDelegate = delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length != 0)
						{
							IEnumerable<int> supportedIds = SupportedItemsDetectorBase.GetSupportedIds(data.Select((byte b) => b.ReverseByte()).ToArray<byte>());
							Func<int, int> func;
							if ((func = CS$<>8__locals2.<>9__6) == null)
							{
								func = (CS$<>8__locals2.<>9__6 = (int x) => x + CS$<>8__locals2.counter);
							}
							foreach (int num5 in supportedIds.Select(func))
							{
								EngineActuatorTestsDetector.<>c__DisplayClass0_4 CS$<>8__locals5 = new EngineActuatorTestsDetector.<>c__DisplayClass0_4();
								CS$<>8__locals5.hexId = num5.ToString("X2");
								ValueItemWithTranslation valueItemWithTranslation3 = CS$<>8__locals2.lowList.FirstOrDefault((ValueItemWithTranslation x) => x.Value == CS$<>8__locals5.hexId);
								if (valueItemWithTranslation3 != null)
								{
									CustomizableCodingTemplate customizableCodingTemplate3 = EngineActuatorTestsDetector.CreateActuatorTestFor2124(num5, valueItemWithTranslation3.Title);
									CS$<>8__locals2.CS$<>8__locals1.result.Add(customizableCodingTemplate3);
								}
							}
							CS$<>8__locals2.hasReqData = true;
						}
						CS$<>8__locals2.counter += 32;
					};
					obdrequest5.ResponseDecoded += responseDecodedDelegate;
					obdrequest6.ResponseDecoded += responseDecodedDelegate;
					obdrequest7.ResponseDecoded += responseDecodedDelegate;
					obdrequest8.ResponseDecoded += responseDecodedDelegate;
					obdrequest9.ResponseDecoded += responseDecodedDelegate;
					obdrequest10.ResponseDecoded += responseDecodedDelegate;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest4, obdrequest5, obdrequest6, obdrequest7, obdrequest8, obdrequest9, obdrequest10 });
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, EngineActuatorTestsDetector.<GetSupportedActuators>d__0>(ref taskAwaiter, ref this);
						return;
					}
					IL_0321:
					taskAwaiter.GetResult();
					IL_0328:
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

			// Token: 0x06005A92 RID: 23186 RVA: 0x00433A1C File Offset: 0x00431C1C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040038E5 RID: 14565
			public int <>1__state;

			// Token: 0x040038E6 RID: 14566
			public AsyncTaskMethodBuilder<List<CustomizableCodingTemplate>> <>t__builder;

			// Token: 0x040038E7 RID: 14567
			private EngineActuatorTestsDetector.<>c__DisplayClass0_0 <>8__1;

			// Token: 0x040038E8 RID: 14568
			private TaskAwaiter <>u__1;
		}
	}
}
