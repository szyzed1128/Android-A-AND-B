using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A6B RID: 2667
	internal static class Washers
	{
		// Token: 0x06005443 RID: 21571 RVA: 0x00401FC0 File Offset: 0x004001C0
		public static ICodingContainer WipersServicePositionMenu()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
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
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 6, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 6, 6, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Washer, "Windshield wiper service position activation through menu option", "Compatibility depends on installed MMI device.", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация сервисного положения дворников через меню", "Совместимо не со всеми мультимедийными устройствами!", "")
				}
			};
		}

		// Token: 0x06005444 RID: 21572 RVA: 0x00402077 File Offset: 0x00400277
		public static ICodingContainer ParkWipersAfterIgnitionTurnedOff()
		{
			return Washer.ParkWipersAfterIgnitionTurnedOff();
		}

		// Token: 0x06005445 RID: 21573 RVA: 0x00402080 File Offset: 0x00400280
		public static ICodingContainer TearDropWindshield()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 7, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 7, 2, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Washer, "Teardrop mode (additional wipers pass) on the windshield", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Режим дотирки капель для лобового стекла", "", "")
				}
			};
		}

		// Token: 0x06005446 RID: 21574 RVA: 0x00402138 File Offset: 0x00400338
		public static ICodingContainer TearDropRearGlass()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D20", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D20", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 4, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Washer, "Teardrop mode (additional wipers pass) on the rear glass", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Режим дотирки капель для заднего стекла", "", "")
				}
			};
		}

		// Token: 0x06005447 RID: 21575 RVA: 0x004021EF File Offset: 0x004003EF
		public static ICodingContainer MQB_09_ComfortRearWiper()
		{
			return Washer.MQB_09_ComfortRearWiper();
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x004021F6 File Offset: 0x004003F6
		public static ICodingContainer MQB_09_AutoRearWiper()
		{
			return Washer.MQB_09_AutoRearWiper();
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x004021FD File Offset: 0x004003FD
		public static ICodingContainer MQB_HeadlightWasherInterval()
		{
			return Washer.MQB_HeadlightWasherInterval();
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x00402204 File Offset: 0x00400404
		public static ICodingContainer MQB_DelayBeforeHeadlightWasher()
		{
			return Washer.MQB_DelayBeforeHeadlightWasher();
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x0040220B File Offset: 0x0040040B
		public static ICodingContainer MQB_HeadlightWasherDutyDuration()
		{
			return Washer.MQB_HeadlightWasherDutyDuration();
		}

		// Token: 0x0600544C RID: 21580 RVA: 0x00402214 File Offset: 0x00400414
		public static ICodingContainer PQ26_WipersDefrostPosition()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 6, true);
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 6, false);
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 8, 7, false);
					BitHelpers.SwitchBitInByte(array2, 8, 6, true);
					BitHelpers.SwitchBitInByte(array2, 8, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 8, 7, false);
					BitHelpers.SwitchBitInByte(array2, 8, 6, false);
					BitHelpers.SwitchBitInByte(array2, 8, 5, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Washer, "Windshield wipers defrost position", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Положение размораживания дворников", "", "")
				}
			};
		}

		// Token: 0x0600544D RID: 21581 RVA: 0x004022CC File Offset: 0x004004CC
		public static ICodingContainer PQ26_StopWiperWhenHoodOpened()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 8, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 8, 2, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Washer, "Stop wipers when opening hood", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Остановка стеклоочистителей при открытии капота", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x0600544E RID: 21582 RVA: 0x0040238C File Offset: 0x0040058C
		public static ICodingContainer PQ26_ParkWiperWhenHoodOpened()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 6, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 6, 4, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Washer, "Stop wipers when opening hood", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Остановка стеклоочистителей при открытии капота", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x02000A6C RID: 2668
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600544F RID: 21583 RVA: 0x0040244A File Offset: 0x0040064A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005450 RID: 21584 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005451 RID: 21585 RVA: 0x00402458 File Offset: 0x00400658
			internal byte[] <WipersServicePositionMenu>b__0_0(byte[] data, string value, MQBAlternativeCoding coding1)
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

			// Token: 0x06005452 RID: 21586 RVA: 0x004024A4 File Offset: 0x004006A4
			internal byte[] <WipersServicePositionMenu>b__0_1(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 6, 6, false);
				}
				return array;
			}

			// Token: 0x06005453 RID: 21587 RVA: 0x004024F0 File Offset: 0x004006F0
			internal byte[] <TearDropWindshield>b__2_0(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
				}
				return array;
			}

			// Token: 0x06005454 RID: 21588 RVA: 0x0040253C File Offset: 0x0040073C
			internal byte[] <TearDropWindshield>b__2_1(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 2, false);
				}
				return array;
			}

			// Token: 0x06005455 RID: 21589 RVA: 0x00402588 File Offset: 0x00400788
			internal byte[] <TearDropRearGlass>b__3_0(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}

			// Token: 0x06005456 RID: 21590 RVA: 0x004025D4 File Offset: 0x004007D4
			internal byte[] <TearDropRearGlass>b__3_1(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				return array;
			}

			// Token: 0x06005457 RID: 21591 RVA: 0x00402620 File Offset: 0x00400820
			internal byte[] <PQ26_WipersDefrostPosition>b__9_0(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 6, true);
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 6, false);
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				return array;
			}

			// Token: 0x06005458 RID: 21592 RVA: 0x00402690 File Offset: 0x00400890
			internal byte[] <PQ26_WipersDefrostPosition>b__9_1(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 8, 7, false);
					BitHelpers.SwitchBitInByte(array, 8, 6, true);
					BitHelpers.SwitchBitInByte(array, 8, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 8, 7, false);
					BitHelpers.SwitchBitInByte(array, 8, 6, false);
					BitHelpers.SwitchBitInByte(array, 8, 5, false);
				}
				return array;
			}

			// Token: 0x06005459 RID: 21593 RVA: 0x00402700 File Offset: 0x00400900
			internal byte[] <PQ26_StopWiperWhenHoodOpened>b__10_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				return array;
			}

			// Token: 0x0600545A RID: 21594 RVA: 0x0040274C File Offset: 0x0040094C
			internal byte[] <PQ26_StopWiperWhenHoodOpened>b__10_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 8, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 8, 2, false);
				}
				return array;
			}

			// Token: 0x0600545B RID: 21595 RVA: 0x00402798 File Offset: 0x00400998
			internal byte[] <PQ26_ParkWiperWhenHoodOpened>b__11_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				return array;
			}

			// Token: 0x0600545C RID: 21596 RVA: 0x004027E4 File Offset: 0x004009E4
			internal byte[] <PQ26_ParkWiperWhenHoodOpened>b__11_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 6, 4, false);
				}
				return array;
			}

			// Token: 0x0400337D RID: 13181
			public static readonly Washers.<>c <>9 = new Washers.<>c();

			// Token: 0x0400337E RID: 13182
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x0400337F RID: 13183
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x04003380 RID: 13184
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_0;

			// Token: 0x04003381 RID: 13185
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_1;

			// Token: 0x04003382 RID: 13186
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_0;

			// Token: 0x04003383 RID: 13187
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_1;

			// Token: 0x04003384 RID: 13188
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_0;

			// Token: 0x04003385 RID: 13189
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_1;

			// Token: 0x04003386 RID: 13190
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_0;

			// Token: 0x04003387 RID: 13191
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_1;

			// Token: 0x04003388 RID: 13192
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__11_0;

			// Token: 0x04003389 RID: 13193
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__11_1;
		}
	}
}
