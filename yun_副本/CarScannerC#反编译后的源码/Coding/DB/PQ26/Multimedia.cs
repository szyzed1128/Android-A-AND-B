using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A62 RID: 2658
	internal static class Multimedia
	{
		// Token: 0x060053FF RID: 21503 RVA: 0x0040025A File Offset: 0x003FE45A
		public static ICodingContainer PQ26_5F_AMRadio()
		{
			return Multimedia.MQB_5F_AMRadio();
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x00400261 File Offset: 0x003FE461
		public static ICodingContainer PQ26_5F_AUXInEnable()
		{
			return Multimedia.MQB_5F_AUXInEnable();
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x00400268 File Offset: 0x003FE468
		public static ICodingContainer PQ26_5F_OffroadModeDisplayInMMI()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, true);
					BitHelpers.SwitchBitInByte(array, 16, 1, true);
					BitHelpers.SwitchBitInByte(array, 16, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, false);
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 16, 0, true);
					BitHelpers.SwitchBitInByte(array2, 16, 1, true);
					BitHelpers.SwitchBitInByte(array2, 16, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 16, 0, false);
					BitHelpers.SwitchBitInByte(array2, 16, 1, false);
					BitHelpers.SwitchBitInByte(array2, 16, 2, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, "Show offroad screen in multimedia system (Swing-3)", "Compatibility: Swing-3", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Меню Offroad в мультимедийной системе (Swing-3)", "Совместимость: Swing-3", "")
				}
			};
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x00400327 File Offset: 0x003FE527
		public static ICodingContainer MQB_5F_BetterSoundBolero()
		{
			return MultimediaSoundQuality.MQB_5F_BetterSoundBolero();
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x00400330 File Offset: 0x003FE530
		public static ICodingContainer PQ26_5F_DrivingSchool()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[34] = 5;
				}
				else
				{
					array[34] = 0;
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[34] = 5;
				}
				else
				{
					array2[34] = 0;
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0B5B", "5F", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[5] = 1;
				}
				else
				{
					array3[5] = 0;
				}
				return array3;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Multimedia, "Display driving school mode in MMI", "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem })
			{
				InnerDescription = "After applying this coding hold power button on MMI for ~20 seconds to restart it.",
				Translations = 
				{
					new TranslationItem("ru", "Отображение режима автошколы (учебный автомобиль) в мультимедийной системе", "", "После применения этой кодировки требуется перезагрузить мультимедийную систему (зажмите кнопку включения на ~20 секунд).")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x00400446 File Offset: 0x003FE646
		public static ICodingContainer PQ26_5F_TripComputerAvailableWithIgnitionOff()
		{
			return Multimedia.MQB_5F_TripComputerAvailableWithIgnitionOff();
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x00400450 File Offset: 0x003FE650
		public static ICodingContainer PQ26_5F_DisplayComfortUsage()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", VagUnitHelper.GetRequestHeaderForMQBUnit("19"), VagUnitHelper.GetResponseHeaderForMQBUnit("19"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 1, true);
					BitHelpers.SwitchBitInByte(array, 13, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 13, 1, false);
					BitHelpers.SwitchBitInByte(array, 13, 4, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("099D", VagUnitHelper.GetRequestHeaderForMQBUnit("19"), VagUnitHelper.GetResponseHeaderForMQBUnit("19"), "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 0, false);
					BitHelpers.SwitchBitInByte(array2, 2, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 2, 0, true);
					BitHelpers.SwitchBitInByte(array2, 2, 1, true);
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0B3C", VagUnitHelper.GetRequestHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 36, 0, true);
					BitHelpers.SwitchBitInByte(array3, 36, 1, true);
					BitHelpers.SwitchBitInByte(array3, 36, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 36, 0, false);
					BitHelpers.SwitchBitInByte(array3, 36, 1, false);
					BitHelpers.SwitchBitInByte(array3, 36, 2, false);
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0B3D", VagUnitHelper.GetRequestHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[35] = 1;
				}
				else
				{
					array4[35] = 0;
				}
				return array4;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Multimedia, "Display comfort systems consumption in MMI", "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Отображение потребителей систем комфорта в мультимедийной системе", "", "")
				}
			};
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x004005D0 File Offset: 0x003FE7D0
		public static MQBEasyCodingItem MQB_5F_RearSpeakersActivation_6_speakers()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, "Rear speakers activation after installation: 4 channels/6 passive speakers", "", "0600", "773", "7DD", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[4] = 190;
				}
				else
				{
					array[4] = 60;
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация задних динамиков после самостоятельной установки: 4 канала / 6 пассивных динамиков", "", "")
				}
			};
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x00400664 File Offset: 0x003FE864
		public static MQBEasyCodingItem MQB_5F_RearSpeakersActivation_8_speakers()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, "Rear speakers activation after installation: 4 channels/8 passive speakers", "", "0600", "773", "7DD", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[4] = byte.MaxValue;
				}
				else
				{
					array[4] = 60;
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация задних динамиков после самостоятельной установки: 4 канала / 8 пассивных динамиков", "", "")
				}
			};
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x00017A6F File Offset: 0x00015C6F
		public static ICodingContainer PQ26_InstallMultiWheel()
		{
			throw new NotImplementedException();
		}

		// Token: 0x02000A63 RID: 2659
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005409 RID: 21513 RVA: 0x004006F5 File Offset: 0x003FE8F5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600540A RID: 21514 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600540B RID: 21515 RVA: 0x00400704 File Offset: 0x003FE904
			internal byte[] <PQ26_5F_OffroadModeDisplayInMMI>b__2_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, true);
					BitHelpers.SwitchBitInByte(array, 16, 1, true);
					BitHelpers.SwitchBitInByte(array, 16, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, false);
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
				}
				return array;
			}

			// Token: 0x0600540C RID: 21516 RVA: 0x00400778 File Offset: 0x003FE978
			internal byte[] <PQ26_5F_OffroadModeDisplayInMMI>b__2_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, true);
					BitHelpers.SwitchBitInByte(array, 16, 1, true);
					BitHelpers.SwitchBitInByte(array, 16, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, false);
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
				}
				return array;
			}

			// Token: 0x0600540D RID: 21517 RVA: 0x004007EC File Offset: 0x003FE9EC
			internal byte[] <PQ26_5F_DrivingSchool>b__4_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[34] = 5;
				}
				else
				{
					array[34] = 0;
				}
				return array;
			}

			// Token: 0x0600540E RID: 21518 RVA: 0x00400830 File Offset: 0x003FEA30
			internal byte[] <PQ26_5F_DrivingSchool>b__4_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[34] = 5;
				}
				else
				{
					array[34] = 0;
				}
				return array;
			}

			// Token: 0x0600540F RID: 21519 RVA: 0x00400874 File Offset: 0x003FEA74
			internal byte[] <PQ26_5F_DrivingSchool>b__4_2(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[5] = 1;
				}
				else
				{
					array[5] = 0;
				}
				return array;
			}

			// Token: 0x06005410 RID: 21520 RVA: 0x004008B4 File Offset: 0x003FEAB4
			internal byte[] <PQ26_5F_DisplayComfortUsage>b__6_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 1, true);
					BitHelpers.SwitchBitInByte(array, 13, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 13, 1, false);
					BitHelpers.SwitchBitInByte(array, 13, 4, false);
				}
				return array;
			}

			// Token: 0x06005411 RID: 21521 RVA: 0x00400914 File Offset: 0x003FEB14
			internal byte[] <PQ26_5F_DisplayComfortUsage>b__6_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
				}
				return array;
			}

			// Token: 0x06005412 RID: 21522 RVA: 0x00400970 File Offset: 0x003FEB70
			internal byte[] <PQ26_5F_DisplayComfortUsage>b__6_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 36, 0, true);
					BitHelpers.SwitchBitInByte(array, 36, 1, true);
					BitHelpers.SwitchBitInByte(array, 36, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 36, 0, false);
					BitHelpers.SwitchBitInByte(array, 36, 1, false);
					BitHelpers.SwitchBitInByte(array, 36, 2, false);
				}
				return array;
			}

			// Token: 0x06005413 RID: 21523 RVA: 0x004009E4 File Offset: 0x003FEBE4
			internal byte[] <PQ26_5F_DisplayComfortUsage>b__6_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[35] = 1;
				}
				else
				{
					array[35] = 0;
				}
				return array;
			}

			// Token: 0x06005414 RID: 21524 RVA: 0x00400A28 File Offset: 0x003FEC28
			internal byte[] <MQB_5F_RearSpeakersActivation_6_speakers>b__7_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[4] = 190;
				}
				else
				{
					array[4] = 60;
				}
				return array;
			}

			// Token: 0x06005415 RID: 21525 RVA: 0x00400A6C File Offset: 0x003FEC6C
			internal byte[] <MQB_5F_RearSpeakersActivation_8_speakers>b__8_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[4] = byte.MaxValue;
				}
				else
				{
					array[4] = 60;
				}
				return array;
			}

			// Token: 0x06005416 RID: 21526 RVA: 0x00400AB0 File Offset: 0x003FECB0
			internal byte[] <PQ26_InstallMultiWheel>b__9_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
				}
				return array;
			}

			// Token: 0x04003352 RID: 13138
			public static readonly Multimedia.<>c <>9 = new Multimedia.<>c();

			// Token: 0x04003353 RID: 13139
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_0;

			// Token: 0x04003354 RID: 13140
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_1;

			// Token: 0x04003355 RID: 13141
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_0;

			// Token: 0x04003356 RID: 13142
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_1;

			// Token: 0x04003357 RID: 13143
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__4_2;

			// Token: 0x04003358 RID: 13144
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_0;

			// Token: 0x04003359 RID: 13145
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_1;

			// Token: 0x0400335A RID: 13146
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_2;

			// Token: 0x0400335B RID: 13147
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_3;

			// Token: 0x0400335C RID: 13148
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__7_0;

			// Token: 0x0400335D RID: 13149
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__8_0;

			// Token: 0x0400335E RID: 13150
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__9_0;
		}
	}
}
