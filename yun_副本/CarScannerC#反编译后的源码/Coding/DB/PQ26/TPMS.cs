using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A69 RID: 2665
	internal class TPMS
	{
		// Token: 0x06005434 RID: 21556 RVA: 0x004019D0 File Offset: 0x003FFBD0
		public static ICodingContainer TPMS_ActivationStep1()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "03", "40168", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 1, true);
					BitHelpers.SwitchBitInByte(array, 16, 2, true);
					BitHelpers.SwitchBitInByte(array, 19, 7, true);
					BitHelpers.SwitchBitInByte(array, 15, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
					BitHelpers.SwitchBitInByte(array, 19, 7, false);
					BitHelpers.SwitchBitInByte(array, 15, 3, false);
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 7, 0, true);
					BitHelpers.SwitchBitInByte(array2, 7, 1, false);
					BitHelpers.SwitchBitInByte(array2, 7, 2, false);
					BitHelpers.SwitchBitInByte(array2, 7, 3, false);
					BitHelpers.SwitchBitInByte(array2, 7, 4, false);
					BitHelpers.SwitchBitInByte(array2, 7, 5, false);
					BitHelpers.SwitchBitInByte(array2, 7, 6, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 7, 0, false);
					BitHelpers.SwitchBitInByte(array2, 7, 1, false);
					BitHelpers.SwitchBitInByte(array2, 7, 2, false);
					BitHelpers.SwitchBitInByte(array2, 7, 2, false);
					BitHelpers.SwitchBitInByte(array2, 7, 3, false);
					BitHelpers.SwitchBitInByte(array2, 7, 4, false);
					BitHelpers.SwitchBitInByte(array2, 7, 5, false);
					BitHelpers.SwitchBitInByte(array2, 7, 6, false);
				}
				return array2;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[7] = 99;
				}
				else
				{
					array3[7] = 0;
				}
				return array3;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "20103", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[11] = 5;
				}
				else
				{
					array4[11] = 0;
				}
				return array4;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 11, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 11, 7, false);
				}
				return array5;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "20103", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "17", "", "5JA920", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array6, 4, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array6, 4, 0, false);
				}
				return array6;
			}, null);
			return new MQBMultipleCoding(CodingGroup.TPMS, "Indirect TPMS system activation (part #1)", "Uses ABS sensors to warn about low pressure in tires.", false, new ICodingContainer[] { mqbeasyCodingItem, mqbalternativeCoding, mqbalternativeCoding2, mqbeasyCodingItem2 })
			{
				InnerDescription = "After applying first part, turn off your car, lock it and wait for 10 minutes before applying second part.\nOn most of cars there's no need to apply part 2.",
				Translations = 
				{
					new TranslationItem("ru", "Активация системы TPMS по датчикам ABS (часть 1)", "TPMS = система мониторинга давления в шинах.", "После применения первой части следует выключить автомобиль, поставить его на охрану и подождать 10 минут.\nНа многих автомобилях система TPMS работает и без применения второй части.")
				}
			};
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x00401BB8 File Offset: 0x003FFDB8
		public static ICodingContainer TPMS_ActivationStep2()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0E00", "03", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 2;
				}
				else
				{
					array[0] = 2;
				}
				return array;
			}, (byte[] data, MQBEasyCodingItem coding) => MQBAdaptationTemplate.EnableOption.Value);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0E26", "03", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[0] = 2;
				}
				else
				{
					array2[0] = 2;
				}
				return array2;
			}, (byte[] data, MQBEasyCodingItem coding) => MQBAdaptationTemplate.EnableOption.Value);
			return new MQBMultipleCoding(CodingGroup.TPMS, "Indirect TPMS system activation (part #2)", "Uses ABS sensors to warn about low pressure in tires.\nThis part should be applied only after a cycle of restarting all modules on your car.", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = "After applying first part, turn off your car, lock it and wait for 10 minutes before applying second part.\nOn most of cars there's no need to apply part 2.",
				Translations = 
				{
					new TranslationItem("ru", "Активация системы TPMS по датчикам ABS (часть 2)", "TPMS = система мониторинга давления в шинах.", "После применения первой части следует выключить автомобиль, поставить его на охрану и подождать 10 минут.\nНа многих автомобилях система TPMS работает и без применения второй части.")
				}
			};
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x00002050 File Offset: 0x00000250
		public TPMS()
		{
		}

		// Token: 0x02000A6A RID: 2666
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005437 RID: 21559 RVA: 0x00401CC4 File Offset: 0x003FFEC4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005438 RID: 21560 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005439 RID: 21561 RVA: 0x00401CD0 File Offset: 0x003FFED0
			internal byte[] <TPMS_ActivationStep1>b__0_0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 1, true);
					BitHelpers.SwitchBitInByte(array, 16, 2, true);
					BitHelpers.SwitchBitInByte(array, 19, 7, true);
					BitHelpers.SwitchBitInByte(array, 15, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
					BitHelpers.SwitchBitInByte(array, 19, 7, false);
					BitHelpers.SwitchBitInByte(array, 15, 3, false);
				}
				return array;
			}

			// Token: 0x0600543A RID: 21562 RVA: 0x00401D58 File Offset: 0x003FFF58
			internal byte[] <TPMS_ActivationStep1>b__0_1(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, true);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, false);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					BitHelpers.SwitchBitInByte(array, 7, 4, false);
					BitHelpers.SwitchBitInByte(array, 7, 5, false);
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, false);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, false);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					BitHelpers.SwitchBitInByte(array, 7, 4, false);
					BitHelpers.SwitchBitInByte(array, 7, 5, false);
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				return array;
			}

			// Token: 0x0600543B RID: 21563 RVA: 0x00401E18 File Offset: 0x00400018
			internal byte[] <TPMS_ActivationStep1>b__0_2(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[7] = 99;
				}
				else
				{
					array[7] = 0;
				}
				return array;
			}

			// Token: 0x0600543C RID: 21564 RVA: 0x00401E58 File Offset: 0x00400058
			internal byte[] <TPMS_ActivationStep1>b__0_3(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[11] = 5;
				}
				else
				{
					array[11] = 0;
				}
				return array;
			}

			// Token: 0x0600543D RID: 21565 RVA: 0x00401E9C File Offset: 0x0040009C
			internal byte[] <TPMS_ActivationStep1>b__0_4(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 11, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 11, 7, false);
				}
				return array;
			}

			// Token: 0x0600543E RID: 21566 RVA: 0x00401EE8 File Offset: 0x004000E8
			internal byte[] <TPMS_ActivationStep1>b__0_5(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				return array;
			}

			// Token: 0x0600543F RID: 21567 RVA: 0x00401F34 File Offset: 0x00400134
			internal byte[] <TPMS_ActivationStep2>b__1_0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 2;
				}
				else
				{
					array[0] = 2;
				}
				return array;
			}

			// Token: 0x06005440 RID: 21568 RVA: 0x00401F73 File Offset: 0x00400173
			internal string <TPMS_ActivationStep2>b__1_1(byte[] data, MQBEasyCodingItem coding)
			{
				return MQBAdaptationTemplate.EnableOption.Value;
			}

			// Token: 0x06005441 RID: 21569 RVA: 0x00401F80 File Offset: 0x00400180
			internal byte[] <TPMS_ActivationStep2>b__1_2(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 2;
				}
				else
				{
					array[0] = 2;
				}
				return array;
			}

			// Token: 0x06005442 RID: 21570 RVA: 0x00401F73 File Offset: 0x00400173
			internal string <TPMS_ActivationStep2>b__1_3(byte[] data, MQBEasyCodingItem coding)
			{
				return MQBAdaptationTemplate.EnableOption.Value;
			}

			// Token: 0x04003372 RID: 13170
			public static readonly TPMS.<>c <>9 = new TPMS.<>c();

			// Token: 0x04003373 RID: 13171
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x04003374 RID: 13172
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x04003375 RID: 13173
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_2;

			// Token: 0x04003376 RID: 13174
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_3;

			// Token: 0x04003377 RID: 13175
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_4;

			// Token: 0x04003378 RID: 13176
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_5;

			// Token: 0x04003379 RID: 13177
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x0400337A RID: 13178
			public static Func<byte[], MQBEasyCodingItem, string> <>9__1_1;

			// Token: 0x0400337B RID: 13179
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_2;

			// Token: 0x0400337C RID: 13180
			public static Func<byte[], MQBEasyCodingItem, string> <>9__1_3;
		}
	}
}
