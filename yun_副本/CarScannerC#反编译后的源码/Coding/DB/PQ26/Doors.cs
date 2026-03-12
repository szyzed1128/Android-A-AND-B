using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A46 RID: 2630
	internal static class Doors
	{
		// Token: 0x06005334 RID: 21300 RVA: 0x003FC8D4 File Offset: 0x003FAAD4
		public static ICodingContainer PQ26_ElectricWindowsComfort()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A69", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
					BitHelpers.SwitchBitInByte(array2, 5, 0, true);
					BitHelpers.SwitchBitInByte(array2, 5, 1, true);
					BitHelpers.SwitchBitInByte(array2, 0, 6, true);
					BitHelpers.SwitchBitInByte(array2, 0, 5, true);
					BitHelpers.SwitchBitInByte(array2, 0, 0, true);
					BitHelpers.SwitchBitInByte(array2, 4, 5, true);
					BitHelpers.SwitchBitInByte(array2, 4, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 5, 0, false);
					BitHelpers.SwitchBitInByte(array2, 5, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 6, false);
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
					BitHelpers.SwitchBitInByte(array2, 4, 5, false);
					BitHelpers.SwitchBitInByte(array2, 4, 4, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 7, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 7, 0, false);
				}
				return array3;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 7, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 7, 0, false);
				}
				return array4;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			return new MQBMultipleCoding(CodingGroup.Doors, "Electric window on driver door comfort control using key fob", "", false, new ICodingContainer[] { mqbalternativeCoding, mqbalternativeCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Комфортное управление стеклоподъемником двери водителя со штатного ключа", "PQ26: Skoda Rapid", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x003FCA24 File Offset: 0x003FAC24
		public static ICodingContainer Lock_AutolockRear()
		{
			return Doors.Lock_AutolockRear();
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x003FCA2B File Offset: 0x003FAC2B
		public static ICodingContainer Lock_AutoUnlock()
		{
			return Doors.Lock_AutoUnlock();
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x003FCA32 File Offset: 0x003FAC32
		public static ICodingContainer Lock_AutolockAtSpeed()
		{
			return Doors.Lock_AutolockAtSpeed();
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x003FCA39 File Offset: 0x003FAC39
		public static ICodingContainer Lock_AutoUnlockWhenSelectorInParking_NAR()
		{
			return Doors.Lock_AutoUnlockWhenSelectorInParking_NAR();
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x003FCA40 File Offset: 0x003FAC40
		public static ICodingContainer Lock_LockMenu()
		{
			return Doors.Lock_LockMenu();
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x003FCA48 File Offset: 0x003FAC48
		public static ICodingContainer Lock_UnlockDoorsVariantsNewBCM()
		{
			MQBAdaptationOption opt_all = new MQBAdaptationOption("All doors", "01", new TranslationItem[]
			{
				new TranslationItem("ru", "Все двери", "", "")
			});
			MQBAdaptationOption opt_driver = new MQBAdaptationOption("Driver door only", "10", new TranslationItem[]
			{
				new TranslationItem("ru", "Только водительская дверь", "", "")
			});
			MQBAdaptationOption opt_one_side = new MQBAdaptationOption("Doors on one side", "10", new TranslationItem[]
			{
				new TranslationItem("ru", "Двери на одной стороне", "", "")
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A65", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_all.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				else if (value == opt_driver.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
				}
				else if (value == opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				return array;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, "Unlock doors options (new gen. BCM)", "", "70E", "778", "31347", new MQBAdaptationOption[] { opt_all, opt_driver, opt_one_side })
			{
				Alternatives = { mqbalternativeContainerForUnit },
				Translations = 
				{
					new TranslationItem("ru", "Варианты разблокировки дверей (блок 09 нового поколения)", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x003FCB98 File Offset: 0x003FAD98
		public static ICodingContainer Lock_UnlockDoorsVariantsOldBCM()
		{
			MQBAdaptationOption opt_all = new MQBAdaptationOption("All doors", "01", new TranslationItem[]
			{
				new TranslationItem("ru", "Все двери", "", "")
			});
			MQBAdaptationOption opt_driver = new MQBAdaptationOption("Driver door only", "10", new TranslationItem[]
			{
				new TranslationItem("ru", "Только водительская дверь", "", "")
			});
			MQBAdaptationOption opt_one_side = new MQBAdaptationOption("Doors on one side", "10", new TranslationItem[]
			{
				new TranslationItem("ru", "Двери на одной стороне", "", "")
			});
			MQBAdaptationOption opt_kessy = new MQBAdaptationOption("Kessy opening", "11", new TranslationItem[]
			{
				new TranslationItem("ru", "Открытие через Kessy", "", "")
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_all.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				else if (value == opt_driver.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else if (value == opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				else if (value == opt_kessy.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				return array;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, "Unlock doors options (old gen. BCM)", "", "70E", "778", "31347", new MQBAdaptationOption[] { opt_all, opt_driver, opt_one_side })
			{
				Alternatives = { mqbalternativeContainerForUnit },
				Translations = 
				{
					new TranslationItem("ru", "Варианты разблокировки дверей (блок 09 старого поколения)", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x02000A47 RID: 2631
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600533C RID: 21308 RVA: 0x003FCD1D File Offset: 0x003FAF1D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600533D RID: 21309 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600533E RID: 21310 RVA: 0x003FCD2C File Offset: 0x003FAF2C
			internal byte[] <PQ26_ElectricWindowsComfort>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}

			// Token: 0x0600533F RID: 21311 RVA: 0x003FCE08 File Offset: 0x003FB008
			internal byte[] <PQ26_ElectricWindowsComfort>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 5, 0, true);
					BitHelpers.SwitchBitInByte(array, 5, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 5, true);
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 5, 0, false);
					BitHelpers.SwitchBitInByte(array, 5, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 5, false);
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
				}
				return array;
			}

			// Token: 0x06005340 RID: 21312 RVA: 0x003FCEE4 File Offset: 0x003FB0E4
			internal byte[] <PQ26_ElectricWindowsComfort>b__0_2(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, false);
				}
				return array;
			}

			// Token: 0x06005341 RID: 21313 RVA: 0x003FCF30 File Offset: 0x003FB130
			internal byte[] <PQ26_ElectricWindowsComfort>b__0_3(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, false);
				}
				return array;
			}

			// Token: 0x040032EA RID: 13034
			public static readonly Doors.<>c <>9 = new Doors.<>c();

			// Token: 0x040032EB RID: 13035
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x040032EC RID: 13036
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x040032ED RID: 13037
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_2;

			// Token: 0x040032EE RID: 13038
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_3;
		}

		// Token: 0x02000A48 RID: 2632
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06005342 RID: 21314 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06005343 RID: 21315 RVA: 0x003FCF7C File Offset: 0x003FB17C
			internal byte[] <Lock_UnlockDoorsVariantsNewBCM>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_all.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				else if (value == this.opt_driver.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
				}
				else if (value == this.opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				return array;
			}

			// Token: 0x040032EF RID: 13039
			public MQBAdaptationOption opt_all;

			// Token: 0x040032F0 RID: 13040
			public MQBAdaptationOption opt_driver;

			// Token: 0x040032F1 RID: 13041
			public MQBAdaptationOption opt_one_side;
		}

		// Token: 0x02000A49 RID: 2633
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06005344 RID: 21316 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x06005345 RID: 21317 RVA: 0x003FD014 File Offset: 0x003FB214
			internal byte[] <Lock_UnlockDoorsVariantsOldBCM>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_all.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				else if (value == this.opt_driver.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else if (value == this.opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				else if (value == this.opt_kessy.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				return array;
			}

			// Token: 0x040032F2 RID: 13042
			public MQBAdaptationOption opt_all;

			// Token: 0x040032F3 RID: 13043
			public MQBAdaptationOption opt_driver;

			// Token: 0x040032F4 RID: 13044
			public MQBAdaptationOption opt_one_side;

			// Token: 0x040032F5 RID: 13045
			public MQBAdaptationOption opt_kessy;
		}
	}
}
