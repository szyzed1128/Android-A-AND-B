using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A64 RID: 2660
	internal static class Other
	{
		// Token: 0x06005417 RID: 21527 RVA: 0x00400AFC File Offset: 0x003FECFC
		public static ICodingContainer PQ26_AutomaticLockUnlockDoorsForLowTrims()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6A", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					BitHelpers.SwitchBitInByte(array2, 0, 3, true);
					BitHelpers.SwitchBitInByte(array2, 1, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
					BitHelpers.SwitchBitInByte(array2, 0, 3, false);
					BitHelpers.SwitchBitInByte(array2, 1, 7, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Other, "Automaticaly lock/unlock doors (for trims without color multimedia system)", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Автоматическая блокировка/разблокировка дверей для комплектаций Entry/Activ без Swing/Bolero", "PQ26: Skoda Rapid", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x00400BBC File Offset: 0x003FEDBC
		public static ICodingContainer PQ26_AutomaticLockkDoorsAt15kmhUnlockKeyRemoved()
		{
			MQBAdaptationOption opt_off = new MQBAdaptationOption(MQBAdaptationTemplate.DisableOption.Title, "OFF");
			MQBAdaptationOption opt_all_doors = new MQBAdaptationOption("All doors", "AD", new TranslationItem[]
			{
				new TranslationItem("ru", "Все двери", "", "")
			});
			MQBAdaptationOption opt_driver_door = new MQBAdaptationOption("Driver door", "DD", new TranslationItem[]
			{
				new TranslationItem("ru", "Дверь водителя", "", "")
			});
			MQBAdaptationOption opt_one_side = new MQBAdaptationOption("All doors on one side", "OS", new TranslationItem[]
			{
				new TranslationItem("ru", "Все двери на одной стороне", "", "")
			});
			MQBAdaptationOption opt_kessy = new MQBAdaptationOption("Selective opening KESSY", "OS", new TranslationItem[]
			{
				new TranslationItem("ru", "Выборочное открытие KESSY", "", "")
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6A", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == opt_all_doors.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == opt_driver_door.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				else if (value == opt_kessy.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 0) && !BitHelpers.GetBit_0_7(data[0], 1))
				{
					return opt_off.Title;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[1], 0) && !BitHelpers.GetBit_0_7(data[1], 1))
				{
					return opt_all_doors.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[1], 0) && !BitHelpers.GetBit_0_7(data[1], 1))
				{
					return opt_driver_door.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[1], 0) && BitHelpers.GetBit_0_7(data[1], 1))
				{
					return opt_one_side.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[1], 0) && BitHelpers.GetBit_0_7(data[1], 1))
				{
					return opt_kessy.Value;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(true, "0D08", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
					BitHelpers.SwitchBitInByte(array2, 0, 3, false);
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
					BitHelpers.SwitchBitInByte(array2, 0, 6, false);
				}
				else if (value == opt_all_doors.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					BitHelpers.SwitchBitInByte(array2, 0, 3, true);
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
					BitHelpers.SwitchBitInByte(array2, 0, 6, false);
				}
				else if (value == opt_driver_door.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					BitHelpers.SwitchBitInByte(array2, 0, 3, true);
					BitHelpers.SwitchBitInByte(array2, 0, 5, true);
					BitHelpers.SwitchBitInByte(array2, 0, 6, false);
				}
				else if (value == opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					BitHelpers.SwitchBitInByte(array2, 0, 3, true);
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
					BitHelpers.SwitchBitInByte(array2, 0, 6, true);
				}
				else if (value == opt_kessy.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					BitHelpers.SwitchBitInByte(array2, 0, 3, true);
					BitHelpers.SwitchBitInByte(array2, 0, 5, true);
					BitHelpers.SwitchBitInByte(array2, 0, 6, true);
				}
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 2) && !BitHelpers.GetBit_0_7(data[0], 3))
				{
					return opt_off.Title;
				}
				if (BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[0], 3) && !BitHelpers.GetBit_0_7(data[0], 5) && !BitHelpers.GetBit_0_7(data[0], 6))
				{
					return opt_all_doors.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[0], 3) && BitHelpers.GetBit_0_7(data[0], 5) && !BitHelpers.GetBit_0_7(data[0], 6))
				{
					return opt_driver_door.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[0], 3) && !BitHelpers.GetBit_0_7(data[0], 5) && BitHelpers.GetBit_0_7(data[0], 6))
				{
					return opt_one_side.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[0], 3) && BitHelpers.GetBit_0_7(data[0], 5) && BitHelpers.GetBit_0_7(data[0], 6))
				{
					return opt_kessy.Value;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			});
			return new MQBAlternativeCoding(CodingGroup.Other, "Automatic close doors at 15 km/h and open them when key removing", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Автоматическое закрытие дверей при наборе скорости 15 км/ч и открытие их по извлечению ключа", "PQ26: Skoda Rapid", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x00400D65 File Offset: 0x003FEF65
		public static ICodingContainer EnableKeyFobWhileEngineRunning()
		{
			return Doors.EnableKeyFobWhileEngineRunning();
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x00400D6C File Offset: 0x003FEF6C
		public static ICodingContainer PQ26_ElectricWindowsWithoutIgnition()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A69", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(true, "0D0D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 5, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Other, "Electric windows working when ignition turned off", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Работа стеклоподъемников при выключенном зажигании", "PQ26: Skoda Rapid", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x00400E2C File Offset: 0x003FF02C
		public static ICodingContainer PQ26_CloseDriverWindowAfterClosingCarMY19()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A69", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 3, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 3, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", (byte[] data, string value, MQBAlternativeCoding codingItem) => data, (byte[] data, MQBAlternativeCoding codingItem) => MQBAdaptationTemplate.UNSUPPORTED_TITLE);
			return new MQBAlternativeCoding(CodingGroup.Other, "Automatically close driver window after locking car (only for MY2019+)", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Автозакрытие стекла двери водителя при закрывании авто (2019+ м.г.)", "PQ26: Skoda Rapid", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x00400F07 File Offset: 0x003FF107
		public static ICodingContainer MQB_DriverElectricWindowTimeAfterIgnitionTurnedOff()
		{
			return Doors.MQB_DriverElectricWindowTimeAfterIgnitionTurnedOff();
		}

		// Token: 0x02000A65 RID: 2661
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600541D RID: 21533 RVA: 0x00400F0E File Offset: 0x003FF10E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600541E RID: 21534 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600541F RID: 21535 RVA: 0x00400F1C File Offset: 0x003FF11C
			internal byte[] <PQ26_AutomaticLockUnlockDoorsForLowTrims>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				return array;
			}

			// Token: 0x06005420 RID: 21536 RVA: 0x00400F8C File Offset: 0x003FF18C
			internal byte[] <PQ26_AutomaticLockUnlockDoorsForLowTrims>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
				}
				return array;
			}

			// Token: 0x06005421 RID: 21537 RVA: 0x00400FFC File Offset: 0x003FF1FC
			internal byte[] <PQ26_ElectricWindowsWithoutIgnition>b__3_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
				}
				return array;
			}

			// Token: 0x06005422 RID: 21538 RVA: 0x00401048 File Offset: 0x003FF248
			internal byte[] <PQ26_ElectricWindowsWithoutIgnition>b__3_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				return array;
			}

			// Token: 0x06005423 RID: 21539 RVA: 0x00401094 File Offset: 0x003FF294
			internal byte[] <PQ26_CloseDriverWindowAfterClosingCarMY19>b__4_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 3, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 3, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				return array;
			}

			// Token: 0x06005424 RID: 21540 RVA: 0x00016849 File Offset: 0x00014A49
			internal byte[] <PQ26_CloseDriverWindowAfterClosingCarMY19>b__4_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				return data;
			}

			// Token: 0x06005425 RID: 21541 RVA: 0x004011A3 File Offset: 0x003FF3A3
			internal string <PQ26_CloseDriverWindowAfterClosingCarMY19>b__4_2(byte[] data, MQBAlternativeCoding codingItem)
			{
				return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
			}

			// Token: 0x0400335F RID: 13151
			public static readonly Other.<>c <>9 = new Other.<>c();

			// Token: 0x04003360 RID: 13152
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x04003361 RID: 13153
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x04003362 RID: 13154
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_0;

			// Token: 0x04003363 RID: 13155
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_1;

			// Token: 0x04003364 RID: 13156
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_0;

			// Token: 0x04003365 RID: 13157
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_1;

			// Token: 0x04003366 RID: 13158
			public static Func<byte[], MQBAlternativeCoding, string> <>9__4_2;
		}

		// Token: 0x02000A66 RID: 2662
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06005426 RID: 21542 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06005427 RID: 21543 RVA: 0x004011AC File Offset: 0x003FF3AC
			internal byte[] <PQ26_AutomaticLockkDoorsAt15kmhUnlockKeyRemoved>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == this.opt_all_doors.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == this.opt_driver_door.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == this.opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				else if (value == this.opt_kessy.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				return array;
			}

			// Token: 0x06005428 RID: 21544 RVA: 0x004012F0 File Offset: 0x003FF4F0
			internal string <PQ26_AutomaticLockkDoorsAt15kmhUnlockKeyRemoved>b__1(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 0) && !BitHelpers.GetBit_0_7(data[0], 1))
				{
					return this.opt_off.Title;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[1], 0) && !BitHelpers.GetBit_0_7(data[1], 1))
				{
					return this.opt_all_doors.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[1], 0) && !BitHelpers.GetBit_0_7(data[1], 1))
				{
					return this.opt_driver_door.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[1], 0) && BitHelpers.GetBit_0_7(data[1], 1))
				{
					return this.opt_one_side.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[1], 0) && BitHelpers.GetBit_0_7(data[1], 1))
				{
					return this.opt_kessy.Value;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x06005429 RID: 21545 RVA: 0x00401404 File Offset: 0x003FF604
			internal byte[] <PQ26_AutomaticLockkDoorsAt15kmhUnlockKeyRemoved>b__2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				else if (value == this.opt_all_doors.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				else if (value == this.opt_driver_door.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				else if (value == this.opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else if (value == this.opt_kessy.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				return array;
			}

			// Token: 0x0600542A RID: 21546 RVA: 0x00401548 File Offset: 0x003FF748
			internal string <PQ26_AutomaticLockkDoorsAt15kmhUnlockKeyRemoved>b__3(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 2) && !BitHelpers.GetBit_0_7(data[0], 3))
				{
					return this.opt_off.Title;
				}
				if (BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[0], 3) && !BitHelpers.GetBit_0_7(data[0], 5) && !BitHelpers.GetBit_0_7(data[0], 6))
				{
					return this.opt_all_doors.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[0], 3) && BitHelpers.GetBit_0_7(data[0], 5) && !BitHelpers.GetBit_0_7(data[0], 6))
				{
					return this.opt_driver_door.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[0], 3) && !BitHelpers.GetBit_0_7(data[0], 5) && BitHelpers.GetBit_0_7(data[0], 6))
				{
					return this.opt_one_side.Value;
				}
				if (BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[0], 3) && BitHelpers.GetBit_0_7(data[0], 5) && BitHelpers.GetBit_0_7(data[0], 6))
				{
					return this.opt_kessy.Value;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x04003367 RID: 13159
			public MQBAdaptationOption opt_off;

			// Token: 0x04003368 RID: 13160
			public MQBAdaptationOption opt_all_doors;

			// Token: 0x04003369 RID: 13161
			public MQBAdaptationOption opt_driver_door;

			// Token: 0x0400336A RID: 13162
			public MQBAdaptationOption opt_one_side;

			// Token: 0x0400336B RID: 13163
			public MQBAdaptationOption opt_kessy;
		}
	}
}
