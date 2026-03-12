using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B47 RID: 2887
	internal static class Seats
	{
		// Token: 0x06005979 RID: 22905 RVA: 0x0042A9AC File Offset: 0x00428BAC
		public static ICodingContainer MQB_36_ComfortEntryDriverSide()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Seats, "", "", "0600", "74C", "7B6", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
					BitHelpers.SwitchBitInByte(array, 9, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
					BitHelpers.SwitchBitInByte(array, 9, 6, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 14, 0, true);
					BitHelpers.SwitchBitInByte(array2, 14, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 14, 0, false);
					BitHelpers.SwitchBitInByte(array2, 14, 2, false);
				}
				return array2;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array3, 14, 7, true);
						BitHelpers.SwitchBitInByte(array3, 14, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array3, 14, 7, false);
						BitHelpers.SwitchBitInByte(array3, 14, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 14, 0, true);
					BitHelpers.SwitchBitInByte(array3, 14, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 14, 0, false);
					BitHelpers.SwitchBitInByte(array3, 14, 2, false);
				}
				return array3;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 16, 0, true);
					BitHelpers.SwitchBitInByte(array4, 16, 1, false);
					BitHelpers.SwitchBitInByte(array4, 16, 2, false);
					BitHelpers.SwitchBitInByte(array4, 16, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 16, 0, false);
					BitHelpers.SwitchBitInByte(array4, 16, 1, false);
					BitHelpers.SwitchBitInByte(array4, 16, 2, false);
					BitHelpers.SwitchBitInByte(array4, 16, 3, false);
				}
				return array4;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array5[16] = 99;
					}
					else
					{
						array5[16] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 16, 0, true);
					BitHelpers.SwitchBitInByte(array5, 16, 1, false);
					BitHelpers.SwitchBitInByte(array5, 16, 2, false);
					BitHelpers.SwitchBitInByte(array5, 16, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 16, 0, false);
					BitHelpers.SwitchBitInByte(array5, 16, 1, false);
					BitHelpers.SwitchBitInByte(array5, 16, 2, false);
					BitHelpers.SwitchBitInByte(array5, 16, 3, false);
				}
				return array5;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			return new MQBMultipleCoding(CodingGroup.Seats, Translate.GetString("codingDB_ComfortEntryDriverSeat_Name"), Translate.GetString("codingDB_RequiresDriverSeatWithMemoryFunction_Name"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbalternativeCoding, mqbalternativeCoding2 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x0600597A RID: 22906 RVA: 0x0042AB48 File Offset: 0x00428D48
		public static ICodingContainer MQB_06_ComfortEntryPassengerSide()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Seats, "", "", "0600", "74D", "7B7", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 4, true);
					BitHelpers.SwitchBitInByte(array, 9, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
					BitHelpers.SwitchBitInByte(array, 6, 4, false);
					BitHelpers.SwitchBitInByte(array, 9, 6, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 14, 0, true);
					BitHelpers.SwitchBitInByte(array2, 14, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 14, 0, false);
					BitHelpers.SwitchBitInByte(array2, 14, 2, false);
				}
				return array2;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array3, 14, 7, true);
						BitHelpers.SwitchBitInByte(array3, 14, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array3, 14, 7, false);
						BitHelpers.SwitchBitInByte(array3, 14, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 14, 0, true);
					BitHelpers.SwitchBitInByte(array3, 14, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 14, 0, false);
					BitHelpers.SwitchBitInByte(array3, 14, 2, false);
				}
				return array3;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 32, 0, true);
					BitHelpers.SwitchBitInByte(array4, 32, 1, false);
					BitHelpers.SwitchBitInByte(array4, 32, 2, false);
					BitHelpers.SwitchBitInByte(array4, 32, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 32, 0, false);
					BitHelpers.SwitchBitInByte(array4, 32, 1, false);
					BitHelpers.SwitchBitInByte(array4, 32, 2, false);
					BitHelpers.SwitchBitInByte(array4, 32, 3, false);
				}
				return array4;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array5[32] = 99;
					}
					else
					{
						array5[32] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 32, 0, true);
					BitHelpers.SwitchBitInByte(array5, 32, 1, false);
					BitHelpers.SwitchBitInByte(array5, 32, 2, false);
					BitHelpers.SwitchBitInByte(array5, 32, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 32, 0, false);
					BitHelpers.SwitchBitInByte(array5, 32, 1, false);
					BitHelpers.SwitchBitInByte(array5, 32, 2, false);
					BitHelpers.SwitchBitInByte(array5, 32, 3, false);
				}
				return array5;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.Seats, "Comfort entry driver seat", "Requires driver seat with memory function", "0600", "74C", "7B6", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array6, 6, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array6, 6, 4, false);
				}
				return array6;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.Seats, Translate.GetString("codingDB_ComfortEntryPassengerSeat_Name"), Translate.GetString("codingDB_RequiresDriverSeatWithMemoryFunction_Name"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbalternativeCoding, mqbalternativeCoding2, mqbeasyCodingItem2 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x0600597B RID: 22907 RVA: 0x0042AD4C File Offset: 0x00428F4C
		public static ICodingContainer BasicSettingsDriverSeat()
		{
			return new MQBSimpleOperationWith0102StatusCheck("Basic setting - driver seat", "", "", "36", "31010483040000", "31020483", "", false)
			{
				Group = CodingGroup.Climate,
				Translations = 
				{
					new TranslationItem("ru", "Базовая установка - сиденье водителя", "", "")
				}
			};
		}

		// Token: 0x0600597C RID: 22908 RVA: 0x0042ADB0 File Offset: 0x00428FB0
		public static ICodingContainer BasicSettingsPassengerSeat()
		{
			return new MQBSimpleOperationWith0102StatusCheck("Basic setting - passenger seat", "", "", "06", "31010483040000", "31020483", "", false)
			{
				Group = CodingGroup.Climate,
				Translations = 
				{
					new TranslationItem("ru", "Базовая установка - сиденье пассажира", "", "")
				}
			};
		}

		// Token: 0x02000B48 RID: 2888
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600597D RID: 22909 RVA: 0x0042AE11 File Offset: 0x00429011
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600597E RID: 22910 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600597F RID: 22911 RVA: 0x0042AE20 File Offset: 0x00429020
			internal byte[] <MQB_36_ComfortEntryDriverSide>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
					BitHelpers.SwitchBitInByte(array, 9, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
					BitHelpers.SwitchBitInByte(array, 9, 6, false);
				}
				return array;
			}

			// Token: 0x06005980 RID: 22912 RVA: 0x0042AE80 File Offset: 0x00429080
			internal byte[] <MQB_36_ComfortEntryDriverSide>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 0, true);
					BitHelpers.SwitchBitInByte(array, 14, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 14, 0, false);
					BitHelpers.SwitchBitInByte(array, 14, 2, false);
				}
				return array;
			}

			// Token: 0x06005981 RID: 22913 RVA: 0x0042AEE0 File Offset: 0x004290E0
			internal byte[] <MQB_36_ComfortEntryDriverSide>b__0_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 7, true);
						BitHelpers.SwitchBitInByte(array, 14, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 14, 7, false);
						BitHelpers.SwitchBitInByte(array, 14, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 0, true);
					BitHelpers.SwitchBitInByte(array, 14, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 14, 0, false);
					BitHelpers.SwitchBitInByte(array, 14, 2, false);
				}
				return array;
			}

			// Token: 0x06005982 RID: 22914 RVA: 0x0042AF88 File Offset: 0x00429188
			internal byte[] <MQB_36_ComfortEntryDriverSide>b__0_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, true);
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
					BitHelpers.SwitchBitInByte(array, 16, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, false);
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
					BitHelpers.SwitchBitInByte(array, 16, 3, false);
				}
				return array;
			}

			// Token: 0x06005983 RID: 22915 RVA: 0x0042B010 File Offset: 0x00429210
			internal byte[] <MQB_36_ComfortEntryDriverSide>b__0_4(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[16] = 99;
					}
					else
					{
						array[16] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, true);
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
					BitHelpers.SwitchBitInByte(array, 16, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, false);
					BitHelpers.SwitchBitInByte(array, 16, 1, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
					BitHelpers.SwitchBitInByte(array, 16, 3, false);
				}
				return array;
			}

			// Token: 0x06005984 RID: 22916 RVA: 0x0042B0C0 File Offset: 0x004292C0
			internal byte[] <MQB_06_ComfortEntryPassengerSide>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 4, true);
					BitHelpers.SwitchBitInByte(array, 9, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
					BitHelpers.SwitchBitInByte(array, 6, 4, false);
					BitHelpers.SwitchBitInByte(array, 9, 6, false);
				}
				return array;
			}

			// Token: 0x06005985 RID: 22917 RVA: 0x0042B130 File Offset: 0x00429330
			internal byte[] <MQB_06_ComfortEntryPassengerSide>b__1_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 0, true);
					BitHelpers.SwitchBitInByte(array, 14, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 14, 0, false);
					BitHelpers.SwitchBitInByte(array, 14, 2, false);
				}
				return array;
			}

			// Token: 0x06005986 RID: 22918 RVA: 0x0042B190 File Offset: 0x00429390
			internal byte[] <MQB_06_ComfortEntryPassengerSide>b__1_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 7, true);
						BitHelpers.SwitchBitInByte(array, 14, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 14, 7, false);
						BitHelpers.SwitchBitInByte(array, 14, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 0, true);
					BitHelpers.SwitchBitInByte(array, 14, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 14, 0, false);
					BitHelpers.SwitchBitInByte(array, 14, 2, false);
				}
				return array;
			}

			// Token: 0x06005987 RID: 22919 RVA: 0x0042B238 File Offset: 0x00429438
			internal byte[] <MQB_06_ComfortEntryPassengerSide>b__1_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 32, 0, true);
					BitHelpers.SwitchBitInByte(array, 32, 1, false);
					BitHelpers.SwitchBitInByte(array, 32, 2, false);
					BitHelpers.SwitchBitInByte(array, 32, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 32, 0, false);
					BitHelpers.SwitchBitInByte(array, 32, 1, false);
					BitHelpers.SwitchBitInByte(array, 32, 2, false);
					BitHelpers.SwitchBitInByte(array, 32, 3, false);
				}
				return array;
			}

			// Token: 0x06005988 RID: 22920 RVA: 0x0042B2C0 File Offset: 0x004294C0
			internal byte[] <MQB_06_ComfortEntryPassengerSide>b__1_4(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[32] = 99;
					}
					else
					{
						array[32] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 32, 0, true);
					BitHelpers.SwitchBitInByte(array, 32, 1, false);
					BitHelpers.SwitchBitInByte(array, 32, 2, false);
					BitHelpers.SwitchBitInByte(array, 32, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 32, 0, false);
					BitHelpers.SwitchBitInByte(array, 32, 1, false);
					BitHelpers.SwitchBitInByte(array, 32, 2, false);
					BitHelpers.SwitchBitInByte(array, 32, 3, false);
				}
				return array;
			}

			// Token: 0x06005989 RID: 22921 RVA: 0x0042B370 File Offset: 0x00429570
			internal byte[] <MQB_06_ComfortEntryPassengerSide>b__1_5(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
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

			// Token: 0x040037E0 RID: 14304
			public static readonly Seats.<>c <>9 = new Seats.<>c();

			// Token: 0x040037E1 RID: 14305
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x040037E2 RID: 14306
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x040037E3 RID: 14307
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_2;

			// Token: 0x040037E4 RID: 14308
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_3;

			// Token: 0x040037E5 RID: 14309
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_4;

			// Token: 0x040037E6 RID: 14310
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x040037E7 RID: 14311
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_1;

			// Token: 0x040037E8 RID: 14312
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_2;

			// Token: 0x040037E9 RID: 14313
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_3;

			// Token: 0x040037EA RID: 14314
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_4;

			// Token: 0x040037EB RID: 14315
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_5;
		}
	}
}
