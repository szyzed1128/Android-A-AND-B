using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MLB_A4B9
{
	// Token: 0x02000B7B RID: 2939
	internal static class Assistance
	{
		// Token: 0x06005A43 RID: 23107 RVA: 0x004311FC File Offset: 0x0042F3FC
		public static ICodingContainer HighBeamAssistance()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "A5", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 21, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 21, 5, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "09", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 5, 0, true);
					BitHelpers.SwitchBitInByte(array2, 5, 5, true);
					BitHelpers.SwitchBitInByte(array2, 5, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 5, 0, false);
					BitHelpers.SwitchBitInByte(array2, 5, 5, false);
					BitHelpers.SwitchBitInByte(array2, 5, 6, false);
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Assistance, "High beam assistance activation", "Requires assistance camera", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x0043129C File Offset: 0x0042F49C
		public static ICodingContainer LaneAssist()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "A5", "20103", "8W*", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 7, true);
					BitHelpers.SwitchBitInByte(array, 12, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 7, false);
					BitHelpers.SwitchBitInByte(array, 12, 3, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 4, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 4, 6, false);
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0600", "09", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 35, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 35, 0, false);
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0600", "44", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (coding2.ComponentSystem.Contains("EPS_MLBEVO_ZF"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array4, 2, 7, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array4, 2, 7, false);
					}
				}
				else if (coding2.ComponentSystem.Contains("EPS_MLBEVO2BS"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array4, 2, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array4, 2, 0, false);
					}
				}
				return array4;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem5 = new MQBEasyCodingItem("0B3C", "5F", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array5[4] = 5;
				}
				else
				{
					array5[4] = 0;
				}
				return array5;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem6 = new MQBEasyCodingItem("0B3D", "5F", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array6[25] = 25;
				}
				else
				{
					array6[25] = 0;
				}
				return array6;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem7 = new MQBEasyCodingItem("09A4", "A5", "20103", "8W*", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array7, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array7, 0, 7, false);
				}
				return array7;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem8 = new MQBEasyCodingItem("3BB2", "A5", "20103", "8W*", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array8 = new byte[data.Length];
				Array.Copy(data, 0, array8, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array8, 0, 7, true);
					BitHelpers.SwitchBitInByte(array8, 0, 6, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array8, 0, 7, false);
					BitHelpers.SwitchBitInByte(array8, 0, 6, false);
				}
				return array8;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem9 = new MQBEasyCodingItem("09A2", "A5", "20103", "8W*", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array9 = new byte[data.Length];
				Array.Copy(data, 0, array9, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array9, 0, 7, true);
					BitHelpers.SwitchBitInByte(array9, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array9, 0, 7, false);
					BitHelpers.SwitchBitInByte(array9, 0, 6, false);
				}
				return array9;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem10 = new MQBEasyCodingItem("09A1", "A5", "20103", "8W*", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array10 = new byte[data.Length];
				Array.Copy(data, 0, array10, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array10, 0, 7, false);
					BitHelpers.SwitchBitInByte(array10, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array10, 0, 7, false);
					BitHelpers.SwitchBitInByte(array10, 0, 6, false);
				}
				return array10;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Assistance, "Lane assist: activation", "Requires assistance camera installed", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem7, mqbeasyCodingItem3, mqbeasyCodingItem8, mqbeasyCodingItem4, mqbeasyCodingItem9, mqbeasyCodingItem5, mqbeasyCodingItem6, mqbeasyCodingItem10 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Lane assist: активация ассистента удержания в полосе", "Требуется наличие камеры ассистентов", "")
				}
			};
		}

		// Token: 0x02000B7C RID: 2940
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005A45 RID: 23109 RVA: 0x0043155E File Offset: 0x0042F75E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005A46 RID: 23110 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005A47 RID: 23111 RVA: 0x0043156C File Offset: 0x0042F76C
			internal byte[] <HighBeamAssistance>b__0_0(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 21, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 21, 5, false);
				}
				return array;
			}

			// Token: 0x06005A48 RID: 23112 RVA: 0x004315B8 File Offset: 0x0042F7B8
			internal byte[] <HighBeamAssistance>b__0_1(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 0, true);
					BitHelpers.SwitchBitInByte(array, 5, 5, true);
					BitHelpers.SwitchBitInByte(array, 5, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 0, false);
					BitHelpers.SwitchBitInByte(array, 5, 5, false);
					BitHelpers.SwitchBitInByte(array, 5, 6, false);
				}
				return array;
			}

			// Token: 0x06005A49 RID: 23113 RVA: 0x00431628 File Offset: 0x0042F828
			internal byte[] <LaneAssist>b__1_0(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 7, true);
					BitHelpers.SwitchBitInByte(array, 12, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 7, false);
					BitHelpers.SwitchBitInByte(array, 12, 3, false);
				}
				return array;
			}

			// Token: 0x06005A4A RID: 23114 RVA: 0x00431688 File Offset: 0x0042F888
			internal byte[] <LaneAssist>b__1_1(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, false);
				}
				return array;
			}

			// Token: 0x06005A4B RID: 23115 RVA: 0x004316D4 File Offset: 0x0042F8D4
			internal byte[] <LaneAssist>b__1_2(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 35, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 35, 0, false);
				}
				return array;
			}

			// Token: 0x06005A4C RID: 23116 RVA: 0x00431720 File Offset: 0x0042F920
			internal byte[] <LaneAssist>b__1_3(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (coding2.ComponentSystem.Contains("EPS_MLBEVO_ZF"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 2, 7, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 2, 7, false);
					}
				}
				else if (coding2.ComponentSystem.Contains("EPS_MLBEVO2BS"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 2, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 2, 0, false);
					}
				}
				return array;
			}

			// Token: 0x06005A4D RID: 23117 RVA: 0x004317B8 File Offset: 0x0042F9B8
			internal byte[] <LaneAssist>b__1_4(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[4] = 5;
				}
				else
				{
					array[4] = 0;
				}
				return array;
			}

			// Token: 0x06005A4E RID: 23118 RVA: 0x004317F8 File Offset: 0x0042F9F8
			internal byte[] <LaneAssist>b__1_5(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[25] = 25;
				}
				else
				{
					array[25] = 0;
				}
				return array;
			}

			// Token: 0x06005A4F RID: 23119 RVA: 0x0043183C File Offset: 0x0042FA3C
			internal byte[] <LaneAssist>b__1_6(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				return array;
			}

			// Token: 0x06005A50 RID: 23120 RVA: 0x00431888 File Offset: 0x0042FA88
			internal byte[] <LaneAssist>b__1_7(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}

			// Token: 0x06005A51 RID: 23121 RVA: 0x004318E4 File Offset: 0x0042FAE4
			internal byte[] <LaneAssist>b__1_8(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}

			// Token: 0x06005A52 RID: 23122 RVA: 0x00431940 File Offset: 0x0042FB40
			internal byte[] <LaneAssist>b__1_9(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}

			// Token: 0x040038AB RID: 14507
			public static readonly Assistance.<>c <>9 = new Assistance.<>c();

			// Token: 0x040038AC RID: 14508
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x040038AD RID: 14509
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_1;

			// Token: 0x040038AE RID: 14510
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x040038AF RID: 14511
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_1;

			// Token: 0x040038B0 RID: 14512
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_2;

			// Token: 0x040038B1 RID: 14513
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_3;

			// Token: 0x040038B2 RID: 14514
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_4;

			// Token: 0x040038B3 RID: 14515
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_5;

			// Token: 0x040038B4 RID: 14516
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_6;

			// Token: 0x040038B5 RID: 14517
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_7;

			// Token: 0x040038B6 RID: 14518
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_8;

			// Token: 0x040038B7 RID: 14519
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_9;
		}
	}
}
