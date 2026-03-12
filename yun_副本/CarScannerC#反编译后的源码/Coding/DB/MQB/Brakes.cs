using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A8D RID: 2701
	internal static class Brakes
	{
		// Token: 0x0600556F RID: 21871 RVA: 0x0040B190 File Offset: 0x00409390
		public static MQBEasyCodingItem MQB_03_ESCMenu()
		{
			return new MQBEasyCodingItem(CodingGroup.Brakes, Translate.GetString("codingDB_EscOffMenu_Name"), Translate.GetString("codingDB_EscOffMenu_Description"), "0600", "713", "77D", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = byte.Parse(value, NumberStyles.HexNumber);
				int num = (int)array[29];
				num &= 240;
				num |= (int)b;
				array[29] = (byte)num;
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				byte b2 = data[29];
				switch (b2 & 15)
				{
				case 1:
					return "No menu";
				case 2:
					return "ESC On/ASR Off";
				case 3:
					return "ESC On/ESC Sport";
				case 4:
					return "ESC On/ESC Off";
				case 5:
					return "ESC On/ASR Off/ESC Off";
				case 6:
					return "ESC On/ESC Sport/ESC Off";
				case 7:
					return "ESC On/ASR Off/ESC Off";
				case 8:
					return "ESC On/ESC Sport/ESC Off";
				case 9:
					return "ESC On/ASR Off/ESC Sport";
				case 10:
					return "ESC On/ASR Off/ESC Sport";
				default:
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
			}, new MQBAdaptationOption[]
			{
				new MQBAdaptationOption("No menu", 1.ToString("X2")),
				new MQBAdaptationOption("ESC On/ASR Off", 2.ToString("X2")),
				new MQBAdaptationOption("ESC On/ESC Sport", 3.ToString("X2")),
				new MQBAdaptationOption("ESC On/ESC Off", 4.ToString("X2")),
				new MQBAdaptationOption("ESC On/ASR Off/ESC Off", 5.ToString("X2")),
				new MQBAdaptationOption("ESC On/ESC Sport/ESC Off", 6.ToString("X2")),
				new MQBAdaptationOption("ESC On/ASR Off/ESC Off", 7.ToString("X2")),
				new MQBAdaptationOption("ESC On/ESC Sport/ESC Off", 8.ToString("X2")),
				new MQBAdaptationOption("ESC On/ASR Off/ESC Sport", 9.ToString("X2")),
				new MQBAdaptationOption("ESC On/ASR Off/ESC Sport", 10.ToString("X2"))
			})
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005570 RID: 21872 RVA: 0x0040B330 File Offset: 0x00409530
		public static MQBEasyCodingItem MQB_03_HillHoldControlActivation()
		{
			return new MQBEasyCodingItem(CodingGroup.Brakes, Translate.GetString("codingDB_HillHoldControlActivation_Name"), Translate.GetString("codingDB_HillHoldControlActivation_Description"), "0600", "713", "77D", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 25, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 25, 0, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x0040B3A8 File Offset: 0x004095A8
		public static ICodingContainer MQB_03_XDS_Activation()
		{
			MQBAdaptationOption off_option = new MQBAdaptationOption(MQBAdaptationTemplate.DisableOption.Title, "00");
			MQBAdaptationOption on1_option = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (engine power <=110 kW/147 hp)", "01");
			MQBAdaptationOption on2_option = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (engine power 110-155 kW/150-210 hp)", "10");
			MQBAdaptationOption on3_option = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (engine power >=162 kW/220 hp)", "11");
			return new MQBEasyCodingItem(CodingGroup.Brakes, Translate.GetString("codingDB_XdsExpandedElectronicDifferentialSystemActivation_Name"), Translate.GetString("codingDB_XdsExpandedElectronicDifferentialSystemActivation_Description"), "0600", "713", "77D", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == off_option.Value)
				{
					BitHelpers.SwitchBitInByte(array, 23, 1, false);
					BitHelpers.SwitchBitInByte(array, 23, 2, false);
				}
				else if (value == on1_option.Value)
				{
					BitHelpers.SwitchBitInByte(array, 23, 1, true);
					BitHelpers.SwitchBitInByte(array, 23, 2, false);
				}
				else if (value == on2_option.Value)
				{
					BitHelpers.SwitchBitInByte(array, 23, 1, false);
					BitHelpers.SwitchBitInByte(array, 23, 2, true);
				}
				else if (value == on3_option.Value)
				{
					BitHelpers.SwitchBitInByte(array, 23, 1, true);
					BitHelpers.SwitchBitInByte(array, 23, 2, true);
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[23], 1) && !BitHelpers.GetBit_0_7(data[23], 2))
				{
					return off_option.Title;
				}
				if (BitHelpers.GetBit_0_7(data[23], 1) && !BitHelpers.GetBit_0_7(data[23], 2))
				{
					return on1_option.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[23], 1) && BitHelpers.GetBit_0_7(data[23], 2))
				{
					return on2_option.Title;
				}
				if (BitHelpers.GetBit_0_7(data[23], 1) && BitHelpers.GetBit_0_7(data[23], 2))
				{
					return on3_option.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}, new MQBAdaptationOption[] { off_option, on1_option, on2_option, on3_option });
		}

		// Token: 0x06005572 RID: 21874 RVA: 0x0040B4B8 File Offset: 0x004096B8
		public static ICodingContainer MQB_ForceEnableAutoHoldWithoutButtonControl()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "03", "24990", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 8, 1, true);
					BitHelpers.SwitchBitInByte(array, 19, 6, true);
					BitHelpers.SwitchBitInByte(array, 23, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 8, 1, false);
					BitHelpers.SwitchBitInByte(array, 19, 6, false);
					BitHelpers.SwitchBitInByte(array, 23, 0, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0E18", "03", "24990", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[0] = 1;
				}
				else
				{
					array2[0] = 0;
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0E17", "03", "24990", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[0] = 2;
				}
				else
				{
					array3[0] = 0;
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0E14", "03", "24990", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[1] = 0;
				}
				else
				{
					array4[1] = 7;
				}
				return array4;
			}, null);
			MQBMultipleCoding mqbmultipleCoding = new MQBMultipleCoding(CodingGroup.Brakes, "Autohold activation for cars WITHOUT Autohold button", "Compatibility depends on your ABS/ESC control unit. Don't use this if you already have Autohold!", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4 });
			TranslationItem translationItem = new TranslationItem("ru", "Активация Autohold для автомобилей БЕЗ кнопки Autohold", "Совместимость зависит от вашего блока ABS/ESC. НИКОГДА не используйте этот пункт, если у вас уже есть функция Autohold!", "");
			mqbmultipleCoding.Translations.Add(translationItem);
			return mqbmultipleCoding;
		}

		// Token: 0x06005573 RID: 21875 RVA: 0x0040B5FC File Offset: 0x004097FC
		public static ICodingContainer BrakePressureSensorBasicAdaptation()
		{
			return new MQBSimpleOperationWith0102StatusCheck("Brake pressure sensor basic setting", "", "", "03", "3101040A", "3102040A", "40168", true)
			{
				Translations = 
				{
					new TranslationItem("ru", "Базовая установка датчика давления в тормозной системе", "", "")
				},
				Group = CodingGroup.Brakes
			};
		}

		// Token: 0x02000A8E RID: 2702
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005574 RID: 21876 RVA: 0x0040B65D File Offset: 0x0040985D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005575 RID: 21877 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005576 RID: 21878 RVA: 0x0040B66C File Offset: 0x0040986C
			internal byte[] <MQB_03_ESCMenu>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = byte.Parse(value, NumberStyles.HexNumber);
				int num = (int)array[29];
				num &= 240;
				num |= (int)b;
				array[29] = (byte)num;
				return array;
			}

			// Token: 0x06005577 RID: 21879 RVA: 0x0040B6B4 File Offset: 0x004098B4
			internal string <MQB_03_ESCMenu>b__0_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				byte b = data[29];
				switch (b & 15)
				{
				case 1:
					return "No menu";
				case 2:
					return "ESC On/ASR Off";
				case 3:
					return "ESC On/ESC Sport";
				case 4:
					return "ESC On/ESC Off";
				case 5:
					return "ESC On/ASR Off/ESC Off";
				case 6:
					return "ESC On/ESC Sport/ESC Off";
				case 7:
					return "ESC On/ASR Off/ESC Off";
				case 8:
					return "ESC On/ESC Sport/ESC Off";
				case 9:
					return "ESC On/ASR Off/ESC Sport";
				case 10:
					return "ESC On/ASR Off/ESC Sport";
				default:
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
			}

			// Token: 0x06005578 RID: 21880 RVA: 0x0040B740 File Offset: 0x00409940
			internal byte[] <MQB_03_HillHoldControlActivation>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 25, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 25, 0, false);
				}
				return array;
			}

			// Token: 0x06005579 RID: 21881 RVA: 0x0040B78C File Offset: 0x0040998C
			internal byte[] <MQB_ForceEnableAutoHoldWithoutButtonControl>b__3_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 8, 1, true);
					BitHelpers.SwitchBitInByte(array, 19, 6, true);
					BitHelpers.SwitchBitInByte(array, 23, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 8, 1, false);
					BitHelpers.SwitchBitInByte(array, 19, 6, false);
					BitHelpers.SwitchBitInByte(array, 23, 0, false);
				}
				return array;
			}

			// Token: 0x0600557A RID: 21882 RVA: 0x0040B800 File Offset: 0x00409A00
			internal byte[] <MQB_ForceEnableAutoHoldWithoutButtonControl>b__3_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 1;
				}
				else
				{
					array[0] = 0;
				}
				return array;
			}

			// Token: 0x0600557B RID: 21883 RVA: 0x0040B840 File Offset: 0x00409A40
			internal byte[] <MQB_ForceEnableAutoHoldWithoutButtonControl>b__3_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 2;
				}
				else
				{
					array[0] = 0;
				}
				return array;
			}

			// Token: 0x0600557C RID: 21884 RVA: 0x0040B880 File Offset: 0x00409A80
			internal byte[] <MQB_ForceEnableAutoHoldWithoutButtonControl>b__3_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[1] = 0;
				}
				else
				{
					array[1] = 7;
				}
				return array;
			}

			// Token: 0x04003476 RID: 13430
			public static readonly Brakes.<>c <>9 = new Brakes.<>c();

			// Token: 0x04003477 RID: 13431
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x04003478 RID: 13432
			public static Func<byte[], MQBEasyCodingItem, string> <>9__0_1;

			// Token: 0x04003479 RID: 13433
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x0400347A RID: 13434
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_0;

			// Token: 0x0400347B RID: 13435
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_1;

			// Token: 0x0400347C RID: 13436
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_2;

			// Token: 0x0400347D RID: 13437
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_3;
		}

		// Token: 0x02000A8F RID: 2703
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600557D RID: 21885 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x0600557E RID: 21886 RVA: 0x0040B8C0 File Offset: 0x00409AC0
			internal byte[] <MQB_03_XDS_Activation>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == this.off_option.Value)
				{
					BitHelpers.SwitchBitInByte(array, 23, 1, false);
					BitHelpers.SwitchBitInByte(array, 23, 2, false);
				}
				else if (value == this.on1_option.Value)
				{
					BitHelpers.SwitchBitInByte(array, 23, 1, true);
					BitHelpers.SwitchBitInByte(array, 23, 2, false);
				}
				else if (value == this.on2_option.Value)
				{
					BitHelpers.SwitchBitInByte(array, 23, 1, false);
					BitHelpers.SwitchBitInByte(array, 23, 2, true);
				}
				else if (value == this.on3_option.Value)
				{
					BitHelpers.SwitchBitInByte(array, 23, 1, true);
					BitHelpers.SwitchBitInByte(array, 23, 2, true);
				}
				return array;
			}

			// Token: 0x0600557F RID: 21887 RVA: 0x0040B988 File Offset: 0x00409B88
			internal string <MQB_03_XDS_Activation>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[23], 1) && !BitHelpers.GetBit_0_7(data[23], 2))
				{
					return this.off_option.Title;
				}
				if (BitHelpers.GetBit_0_7(data[23], 1) && !BitHelpers.GetBit_0_7(data[23], 2))
				{
					return this.on1_option.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[23], 1) && BitHelpers.GetBit_0_7(data[23], 2))
				{
					return this.on2_option.Title;
				}
				if (BitHelpers.GetBit_0_7(data[23], 1) && BitHelpers.GetBit_0_7(data[23], 2))
				{
					return this.on3_option.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x0400347E RID: 13438
			public MQBAdaptationOption off_option;

			// Token: 0x0400347F RID: 13439
			public MQBAdaptationOption on1_option;

			// Token: 0x04003480 RID: 13440
			public MQBAdaptationOption on2_option;

			// Token: 0x04003481 RID: 13441
			public MQBAdaptationOption on3_option;
		}
	}
}
