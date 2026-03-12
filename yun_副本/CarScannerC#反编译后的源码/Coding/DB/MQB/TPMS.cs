using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B62 RID: 2914
	internal static class TPMS
	{
		// Token: 0x060059D9 RID: 23001 RVA: 0x0042D4D0 File Offset: 0x0042B6D0
		public static ICodingContainer MQB_DisableDirectTPMSSystem()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("04A3", "19", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
				}
				return array;
			}, null);
			mqbeasyCodingItem.PostWriteCommands = "1102";
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 7, false);
					BitHelpers.SwitchBitInByte(array2, 11, 2, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 3, 7, true);
					BitHelpers.SwitchBitInByte(array2, 11, 2, true);
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.TPMS, Translate.GetString("codingDB_DisableDirectTpmsSystem_Name"), Translate.GetString("codingDB_DisableDirectTpmsSystem_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = Translate.GetString("codingDB_DisableDirectTpmsSystem_InnerDescription")
			};
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x0042D598 File Offset: 0x0042B798
		public static ICodingContainer MQB_TPMS_Indirect_PatchForParkAssist()
		{
			return new MQBEasyCodingItem(CodingGroup.TPMS, Translate.GetString("codingDB_IndirectTpmsSystemActivationPatchForParkingAssistParkPilot_Name"), Translate.GetString("codingDB_IndirectTpmsSystemActivationPatchForParkingAssistParkPilot_Description"), "0600", "713", "77D", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 27, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 27, 6, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x0042D610 File Offset: 0x0042B810
		public static ICodingContainer MQB_TPMS_Indirect()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Brakes, "03", "", "0600", "713", "77D", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 27, 4, true);
					BitHelpers.SwitchBitInByte(array, 27, 5, true);
					BitHelpers.SwitchBitInByte(array, 28, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 27, 4, false);
					BitHelpers.SwitchBitInByte(array, 27, 5, false);
					BitHelpers.SwitchBitInByte(array, 28, 7, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.Brakes, "17", "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 4, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 4, 0, false);
				}
				return array2;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 7, 0, true);
					BitHelpers.SwitchBitInByte(array3, 7, 1, false);
					BitHelpers.SwitchBitInByte(array3, 7, 2, false);
					BitHelpers.SwitchBitInByte(array3, 7, 3, false);
					BitHelpers.SwitchBitInByte(array3, 7, 4, true);
					BitHelpers.SwitchBitInByte(array3, 7, 5, false);
					BitHelpers.SwitchBitInByte(array3, 7, 6, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 7, 0, false);
					BitHelpers.SwitchBitInByte(array3, 7, 1, false);
					BitHelpers.SwitchBitInByte(array3, 7, 2, false);
					BitHelpers.SwitchBitInByte(array3, 7, 3, false);
					BitHelpers.SwitchBitInByte(array3, 7, 4, true);
					BitHelpers.SwitchBitInByte(array3, 7, 5, false);
					BitHelpers.SwitchBitInByte(array3, 7, 6, false);
				}
				return array3;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array4[7] = 65;
					}
					else
					{
						array4[7] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 7, 0, true);
					BitHelpers.SwitchBitInByte(array4, 7, 1, false);
					BitHelpers.SwitchBitInByte(array4, 7, 2, false);
					BitHelpers.SwitchBitInByte(array4, 7, 3, false);
					BitHelpers.SwitchBitInByte(array4, 7, 4, true);
					BitHelpers.SwitchBitInByte(array4, 7, 5, false);
					BitHelpers.SwitchBitInByte(array4, 7, 6, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 7, 0, false);
					BitHelpers.SwitchBitInByte(array4, 7, 1, false);
					BitHelpers.SwitchBitInByte(array4, 7, 2, false);
					BitHelpers.SwitchBitInByte(array4, 7, 3, false);
					BitHelpers.SwitchBitInByte(array4, 7, 4, true);
					BitHelpers.SwitchBitInByte(array4, 7, 5, false);
					BitHelpers.SwitchBitInByte(array4, 7, 6, false);
				}
				return array4;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("773", "7DD", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array5[11] = 5;
				}
				else
				{
					array5[11] = 4;
				}
				return array5;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array6, 11, 7, true);
						BitHelpers.SwitchBitInByte(array6, 11, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array6, 11, 7, false);
						BitHelpers.SwitchBitInByte(array6, 11, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array6[11] = 5;
				}
				else
				{
					array6[11] = 4;
				}
				return array6;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("773", "7DD", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			return new MQBMultipleCoding(CodingGroup.TPMS, Translate.GetString("codingDB_IndirectTpmsSystemActivation_Name"), Translate.GetString("codingDB_IndirectTpmsSystemActivation_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbalternativeCoding, mqbalternativeCoding2 });
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x0042D800 File Offset: 0x0042BA00
		public static ICodingContainer TPMSDisplayInMMI()
		{
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (Data bus: powertrain)", "05", new TranslationItem[]
			{
				new TranslationItem("ru", MQBAdaptationTemplate.EnableOption.Title + " (шина данных: ходовая часть)", "", "")
			});
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (Data bus: CAN extended)", "0D", new TranslationItem[]
			{
				new TranslationItem("ru", MQBAdaptationTemplate.EnableOption.Title + " (шина данных: CAN Extended)", "", "")
			});
			MQBAdaptationOption mqbadaptationOption3 = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (Data bus: suspension)", "11", new TranslationItem[]
			{
				new TranslationItem("ru", MQBAdaptationTemplate.EnableOption.Title + " (шина данных: подвеска)", "", "")
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = byte.Parse(value, NumberStyles.HexNumber);
				array[7] = b;
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				byte b2 = byte.Parse(value, NumberStyles.HexNumber);
				if (codingItem.IsMIB3())
				{
					if (b2 == 5)
					{
						b2 = 65;
					}
					else if (b2 == 13)
					{
						b2 = 67;
					}
					else if (b2 == 17)
					{
						b2 = 68;
					}
				}
				array2[7] = b2;
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.TPMS, Translate.GetString("codingDB_TpmsMenuInMultimediaSystem_Name"), "", "773", "7DD", "", new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.DisableOption,
				mqbadaptationOption,
				mqbadaptationOption2,
				mqbadaptationOption3
			})
			{
				Alternatives = { mqbalternativeContainer, mqbalternativeContainer2 },
				InnerDescription = Translate.GetString("codingDB_TpmsMenuInMultimediaSystem_InnerDescription")
			};
		}

		// Token: 0x02000B63 RID: 2915
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060059DD RID: 23005 RVA: 0x0042D9BC File Offset: 0x0042BBBC
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060059DE RID: 23006 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060059DF RID: 23007 RVA: 0x0042D9C8 File Offset: 0x0042BBC8
			internal byte[] <MQB_DisableDirectTPMSSystem>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
				}
				return array;
			}

			// Token: 0x060059E0 RID: 23008 RVA: 0x0042DA14 File Offset: 0x0042BC14
			internal byte[] <MQB_DisableDirectTPMSSystem>b__0_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 7, false);
					BitHelpers.SwitchBitInByte(array, 11, 2, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 7, true);
					BitHelpers.SwitchBitInByte(array, 11, 2, true);
				}
				return array;
			}

			// Token: 0x060059E1 RID: 23009 RVA: 0x0042DA74 File Offset: 0x0042BC74
			internal byte[] <MQB_TPMS_Indirect_PatchForParkAssist>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 27, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 27, 6, false);
				}
				return array;
			}

			// Token: 0x060059E2 RID: 23010 RVA: 0x0042DAC0 File Offset: 0x0042BCC0
			internal byte[] <MQB_TPMS_Indirect>b__2_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 27, 4, true);
					BitHelpers.SwitchBitInByte(array, 27, 5, true);
					BitHelpers.SwitchBitInByte(array, 28, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 27, 4, false);
					BitHelpers.SwitchBitInByte(array, 27, 5, false);
					BitHelpers.SwitchBitInByte(array, 28, 7, false);
				}
				return array;
			}

			// Token: 0x060059E3 RID: 23011 RVA: 0x0042DB34 File Offset: 0x0042BD34
			internal byte[] <MQB_TPMS_Indirect>b__2_1(byte[] data, string value, MQBEasyCodingItem codingItem)
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

			// Token: 0x060059E4 RID: 23012 RVA: 0x0042DB80 File Offset: 0x0042BD80
			internal byte[] <MQB_TPMS_Indirect>b__2_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, true);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, false);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					BitHelpers.SwitchBitInByte(array, 7, 4, true);
					BitHelpers.SwitchBitInByte(array, 7, 5, false);
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, false);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, false);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					BitHelpers.SwitchBitInByte(array, 7, 4, true);
					BitHelpers.SwitchBitInByte(array, 7, 5, false);
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				return array;
			}

			// Token: 0x060059E5 RID: 23013 RVA: 0x0042DC38 File Offset: 0x0042BE38
			internal byte[] <MQB_TPMS_Indirect>b__2_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[7] = 65;
					}
					else
					{
						array[7] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, true);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, false);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					BitHelpers.SwitchBitInByte(array, 7, 4, true);
					BitHelpers.SwitchBitInByte(array, 7, 5, false);
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, false);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, false);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					BitHelpers.SwitchBitInByte(array, 7, 4, true);
					BitHelpers.SwitchBitInByte(array, 7, 5, false);
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				return array;
			}

			// Token: 0x060059E6 RID: 23014 RVA: 0x0042DD1C File Offset: 0x0042BF1C
			internal byte[] <MQB_TPMS_Indirect>b__2_4(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[11] = 5;
				}
				else
				{
					array[11] = 4;
				}
				return array;
			}

			// Token: 0x060059E7 RID: 23015 RVA: 0x0042DD60 File Offset: 0x0042BF60
			internal byte[] <MQB_TPMS_Indirect>b__2_5(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 11, 7, true);
						BitHelpers.SwitchBitInByte(array, 11, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 11, 7, false);
						BitHelpers.SwitchBitInByte(array, 11, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[11] = 5;
				}
				else
				{
					array[11] = 4;
				}
				return array;
			}

			// Token: 0x060059E8 RID: 23016 RVA: 0x0042DDE8 File Offset: 0x0042BFE8
			internal byte[] <TPMSDisplayInMMI>b__3_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = byte.Parse(value, NumberStyles.HexNumber);
				array[7] = b;
				return array;
			}

			// Token: 0x060059E9 RID: 23017 RVA: 0x0042DE1C File Offset: 0x0042C01C
			internal byte[] <TPMSDisplayInMMI>b__3_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = byte.Parse(value, NumberStyles.HexNumber);
				if (codingItem.IsMIB3())
				{
					if (b == 5)
					{
						b = 65;
					}
					else if (b == 13)
					{
						b = 67;
					}
					else if (b == 17)
					{
						b = 68;
					}
				}
				array[7] = b;
				return array;
			}

			// Token: 0x04003844 RID: 14404
			public static readonly TPMS.<>c <>9 = new TPMS.<>c();

			// Token: 0x04003845 RID: 14405
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x04003846 RID: 14406
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_1;

			// Token: 0x04003847 RID: 14407
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x04003848 RID: 14408
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x04003849 RID: 14409
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_1;

			// Token: 0x0400384A RID: 14410
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_2;

			// Token: 0x0400384B RID: 14411
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_3;

			// Token: 0x0400384C RID: 14412
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_4;

			// Token: 0x0400384D RID: 14413
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_5;

			// Token: 0x0400384E RID: 14414
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_0;

			// Token: 0x0400384F RID: 14415
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_1;
		}
	}
}
