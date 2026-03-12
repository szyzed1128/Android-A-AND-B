using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B45 RID: 2885
	internal class Roof
	{
		// Token: 0x0600596C RID: 22892 RVA: 0x0042A4BC File Offset: 0x004286BC
		public static ICodingContainer AutomaticallyOpenAndCloseSunroofHoldingKeyFobKeys()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A76", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 4, true);
					BitHelpers.SwitchBitInByte(array2, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 4, false);
					BitHelpers.SwitchBitInByte(array2, 1, 5, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Roof, Translate.GetString("codingDB_OpenAndCloseSunroofByHoldingKeyfobKeys_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x0042A568 File Offset: 0x00428768
		public static ICodingContainer EnableRoofInteriorLights()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "50C5", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "50C5", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				throw new WrongDeviceException("not supported", ExceptionConsequences.Halt);
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("09", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0584", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[6] = 5;
				}
				else
				{
					array2[6] = 4;
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0584", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[6] = 5;
				}
				else
				{
					array3[6] = 4;
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0585", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[30] = 11;
					array4[32] = 18;
					array4[33] = 12;
					array4[35] = 23;
					BitHelpers.SwitchBitInByte(array4, 31, 7, true);
					BitHelpers.SwitchBitInByte(array4, 34, 7, true);
				}
				else
				{
					array4[30] = 0;
					array4[32] = 0;
					array4[33] = 0;
					array4[35] = 0;
					BitHelpers.SwitchBitInByte(array4, 31, 7, false);
					BitHelpers.SwitchBitInByte(array4, 34, 7, false);
				}
				return array4;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("09DB", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, array5.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array5[202] = 8;
				}
				else
				{
					array5[202] = 0;
				}
				return array5;
			}, null);
			MQBMultipleCoding mqbmultipleCoding = new MQBMultipleCoding(CodingGroup.Roof, "Roof interior lighting installed", "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4 });
			TranslationItem translationItem = new TranslationItem("ru", "Подсветка панорамной крыши установлена", "", "");
			mqbmultipleCoding.Translations.Add(translationItem);
			mqbmultipleCoding.RequiresPro = false;
			return mqbmultipleCoding;
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x00002050 File Offset: 0x00000250
		public Roof()
		{
		}

		// Token: 0x02000B46 RID: 2886
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600596F RID: 22895 RVA: 0x0042A739 File Offset: 0x00428939
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005970 RID: 22896 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005971 RID: 22897 RVA: 0x0042A748 File Offset: 0x00428948
			internal byte[] <AutomaticallyOpenAndCloseSunroofHoldingKeyFobKeys>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x06005972 RID: 22898 RVA: 0x0042A7A4 File Offset: 0x004289A4
			internal byte[] <AutomaticallyOpenAndCloseSunroofHoldingKeyFobKeys>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				return array;
			}

			// Token: 0x06005973 RID: 22899 RVA: 0x0042A800 File Offset: 0x00428A00
			internal byte[] <EnableRoofInteriorLights>b__1_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}

			// Token: 0x06005974 RID: 22900 RVA: 0x0042A849 File Offset: 0x00428A49
			internal byte[] <EnableRoofInteriorLights>b__1_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				throw new WrongDeviceException("not supported", ExceptionConsequences.Halt);
			}

			// Token: 0x06005975 RID: 22901 RVA: 0x0042A858 File Offset: 0x00428A58
			internal byte[] <EnableRoofInteriorLights>b__1_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[6] = 5;
				}
				else
				{
					array[6] = 4;
				}
				return array;
			}

			// Token: 0x06005976 RID: 22902 RVA: 0x0042A898 File Offset: 0x00428A98
			internal byte[] <EnableRoofInteriorLights>b__1_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[6] = 5;
				}
				else
				{
					array[6] = 4;
				}
				return array;
			}

			// Token: 0x06005977 RID: 22903 RVA: 0x0042A8D8 File Offset: 0x00428AD8
			internal byte[] <EnableRoofInteriorLights>b__1_4(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[30] = 11;
					array[32] = 18;
					array[33] = 12;
					array[35] = 23;
					BitHelpers.SwitchBitInByte(array, 31, 7, true);
					BitHelpers.SwitchBitInByte(array, 34, 7, true);
				}
				else
				{
					array[30] = 0;
					array[32] = 0;
					array[33] = 0;
					array[35] = 0;
					BitHelpers.SwitchBitInByte(array, 31, 7, false);
					BitHelpers.SwitchBitInByte(array, 34, 7, false);
				}
				return array;
			}

			// Token: 0x06005978 RID: 22904 RVA: 0x0042A964 File Offset: 0x00428B64
			internal byte[] <EnableRoofInteriorLights>b__1_5(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[202] = 8;
				}
				else
				{
					array[202] = 0;
				}
				return array;
			}

			// Token: 0x040037D7 RID: 14295
			public static readonly Roof.<>c <>9 = new Roof.<>c();

			// Token: 0x040037D8 RID: 14296
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x040037D9 RID: 14297
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x040037DA RID: 14298
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_0;

			// Token: 0x040037DB RID: 14299
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_1;

			// Token: 0x040037DC RID: 14300
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_2;

			// Token: 0x040037DD RID: 14301
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_3;

			// Token: 0x040037DE RID: 14302
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_4;

			// Token: 0x040037DF RID: 14303
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_5;
		}
	}
}
