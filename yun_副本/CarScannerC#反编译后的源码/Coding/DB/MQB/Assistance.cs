using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A86 RID: 2694
	internal static class Assistance
	{
		// Token: 0x060054F1 RID: 21745 RVA: 0x00406398 File Offset: 0x00404598
		public static ICodingContainer MQB_13_SimpleCruiseControlActivation()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "13", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (data.Length >= 24 && (codingItem.CheckDevice("*2Q0907572*") || codingItem.CheckDevice("*5Q0907572*")))
				{
					codingItem.Password = "20103";
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 24, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 24, 4, false);
					}
					return array;
				}
				throw new WrongDeviceException("Unit 13 not supported", ExceptionConsequences.Halt);
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0AEA", "17", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_CruiseControlActivation_Title"), Translate.GetString("codingDB_CruiseControlActivation_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = Translate.GetString("codingDB_CruiseControlActivation_InnerDescription")
			};
		}

		// Token: 0x060054F2 RID: 21746 RVA: 0x00406454 File Offset: 0x00404654
		public static ICodingContainer MQB_13_ACC_OperationMode()
		{
			MQBAdaptationOption opt1kmh = new MQBAdaptationOption("±1/±10 (hold)", MQBAdaptationTemplate.EnableOption.Value);
			MQBAdaptationOption opt10kmh = new MQBAdaptationOption("±10", MQBAdaptationTemplate.DisableOption.Value);
			return new MQBEasyCodingItem(CodingGroup.Assistance, Translate.GetString("codingDB_ACC_OperationMode_Name"), Translate.GetString("codingDB_ACC_OperationMode_Description"), "0600", "757", "7C1", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("2Q0907572*"))
				{
					codingItem.Password = "20103";
					if (value == opt1kmh.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 14, 4, false);
					}
				}
				else if (codingItem.CheckDevice("*5Q0907572*"))
				{
					codingItem.Password = "14117";
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 2, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 2, 6, false);
					}
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("2Q0907572*"))
				{
					codingItem.Password = "20103";
					if (BitHelpers.GetBit_0_7(data[14], 4))
					{
						return opt1kmh.Title;
					}
					return opt10kmh.Title;
				}
				else
				{
					if (!codingItem.CheckDevice("5Q0907572*"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					codingItem.Password = "14117";
					if (BitHelpers.GetBit_0_7(data[2], 6))
					{
						return opt1kmh.Title;
					}
					return opt10kmh.Title;
				}
			}, new MQBAdaptationOption[] { opt1kmh, opt10kmh })
			{
				RequiresPro = true,
				PasswordHint = "20103, 14117"
			};
		}

		// Token: 0x060054F3 RID: 21747 RVA: 0x00406510 File Offset: 0x00404710
		public static ICodingContainer MQB_A5_2Q0LaneAssistTJAActivation()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "A5", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("2Q0980653") || codingItem.CheckDevice("5WA980653"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 10, 6, false);
						BitHelpers.SwitchBitInByte(array, 10, 5, true);
						BitHelpers.SwitchBitInByte(array, 10, 4, false);
						BitHelpers.SwitchBitInByte(array, 17, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 6, false);
						BitHelpers.SwitchBitInByte(array, 17, 5, true);
						BitHelpers.SwitchBitInByte(array, 8, 7, true);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
						BitHelpers.SwitchBitInByte(array, 17, 3, false);
						BitHelpers.SwitchBitInByte(array, 17, 2, false);
						BitHelpers.SwitchBitInByte(array, 17, 1, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 10, 6, false);
						BitHelpers.SwitchBitInByte(array, 10, 5, false);
						BitHelpers.SwitchBitInByte(array, 10, 4, false);
						BitHelpers.SwitchBitInByte(array, 17, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 6, false);
						BitHelpers.SwitchBitInByte(array, 17, 5, false);
						BitHelpers.SwitchBitInByte(array, 8, 7, false);
						BitHelpers.SwitchBitInByte(array, 8, 6, true);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
						BitHelpers.SwitchBitInByte(array, 17, 3, false);
						BitHelpers.SwitchBitInByte(array, 17, 2, false);
						BitHelpers.SwitchBitInByte(array, 17, 1, false);
					}
					return array;
				}
				throw new WrongDeviceException("Unit 13 not supported", ExceptionConsequences.Halt);
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "13", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (data.Length >= 24 && (codingItem.CheckDevice("*2Q0907572*") || codingItem.CheckDevice("*5Q0907572*")))
				{
					codingItem.Password = "20103";
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 4, 2, true);
						BitHelpers.SwitchBitInByte(array2, 11, 0, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 4, 2, false);
						BitHelpers.SwitchBitInByte(array2, 11, 0, true);
					}
					return array2;
				}
				throw new WrongDeviceException("Unit 13 not supported", ExceptionConsequences.SkipIgnore);
			}, null);
			MQBMultipleCoding mqbmultipleCoding = new MQBMultipleCoding(CodingGroup.Assistance, "Lane assist (Traffic Jam Assist) on low speed activation for camera 2Q0", "This option enables lane assist on low speed (<60 km/h).\nRequires: assistance camera (A5) 2Q0980653 and ACC unit 2Q0980653 or 5WA980653", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
			mqbmultipleCoding.InnerDescription = "WARNING! This option wasn't tested and confirmed! If you want to undo, please DON'T use DISABLE option! Use Coding history instead.";
			TranslationItem translationItem = new TranslationItem("ru", "Работа Lane assist на низких скоростях (Traffic Jam Assist) для камеры 2Q0980653", "Активирует работу Lane assist на скоростях ниже 60 км/ч.\nТребования: камера ассистентов 2Q0980653 and ACC unit 2Q0980653 or 5WA980653", "ВНИМАНИЕ! Эта опция не проверена и ее работоспособность не подтверждена! Если вы хотите ее отключить, НЕ используйте пункт отключения. Вместо этого используйте Историю кодирования.");
			mqbmultipleCoding.Translations.Add(translationItem);
			return mqbmultipleCoding;
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x004065E4 File Offset: 0x004047E4
		public static MQBEasyCodingItem MQB_A5_LaneAssistSaveState()
		{
			MQBAdaptationOption lastSettingOpt = new MQBAdaptationOption(Translate.GetString("codingDB_LastSetting"), "10");
			return new MQBEasyCodingItem(CodingGroup.Assistance, Translate.GetString("codingDB_LaneAssistSaveState_Name"), "", "0600", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 3, false);
						BitHelpers.SwitchBitInByte(array, 14, 2, true);
					}
					else if (value == MQBAdaptationTemplate.DisableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 3, false);
						BitHelpers.SwitchBitInByte(array, 14, 2, false);
					}
					else if (value == lastSettingOpt.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 3, true);
						BitHelpers.SwitchBitInByte(array, 14, 2, false);
					}
				}
				if (codingItem.CheckDevice("2Q*980653"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 9, 1, false);
						BitHelpers.SwitchBitInByte(array, 9, 0, true);
					}
					else if (value == MQBAdaptationTemplate.DisableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 9, 1, false);
						BitHelpers.SwitchBitInByte(array, 9, 0, false);
					}
					else if (value == lastSettingOpt.Value)
					{
						BitHelpers.SwitchBitInByte(array, 9, 1, true);
						BitHelpers.SwitchBitInByte(array, 9, 0, false);
					}
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (!BitHelpers.GetBit_0_7(data[14], 3) && BitHelpers.GetBit_0_7(data[14], 2))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					if (!BitHelpers.GetBit_0_7(data[14], 3) && !BitHelpers.GetBit_0_7(data[14], 2))
					{
						return MQBAdaptationTemplate.DisableOption.Title;
					}
					if (BitHelpers.GetBit_0_7(data[14], 3) && !BitHelpers.GetBit_0_7(data[14], 2))
					{
						return lastSettingOpt.Title;
					}
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					if (!BitHelpers.GetBit_0_7(data[9], 1) && BitHelpers.GetBit_0_7(data[9], 0))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					if (!BitHelpers.GetBit_0_7(data[9], 1) && !BitHelpers.GetBit_0_7(data[9], 0))
					{
						return MQBAdaptationTemplate.DisableOption.Title;
					}
					if (BitHelpers.GetBit_0_7(data[9], 1) && !BitHelpers.GetBit_0_7(data[9], 0))
					{
						return lastSettingOpt.Title;
					}
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption,
				lastSettingOpt
			});
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x00406678 File Offset: 0x00404878
		public static MQBEasyCodingItem MQB_13_ACC_WaitTime()
		{
			MQBAdaptationOption optShort = new MQBAdaptationOption(Translate.GetString("codingDB_Short_Name"), "1");
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_opt_Long"), "0");
			return new MQBEasyCodingItem(CodingGroup.Assistance, Translate.GetString("codingDB_ACC_WaitTime_Name"), "", "0600", VagUnitHelper.GetRequestHeaderForMQBUnit("13"), VagUnitHelper.GetResponseHeaderForMQBUnit("13"), "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == optShort.Value)
				{
					BitHelpers.SwitchBitInByte(array, 11, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 11, 0, false);
				}
				return array;
			}, null, new MQBAdaptationOption[] { mqbadaptationOption, optShort })
			{
				InnerDescription = Translate.GetString("codingDB_ACC_WaitTime_InnerDescription")
			};
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x00406724 File Offset: 0x00404924
		public static MQBEasyCodingItem MQB_A5_AdaptavieLaneAssist()
		{
			return new MQBEasyCodingItem(CodingGroup.Assistance, Translate.GetString("codingDB_AdaptiveLaneAssist_Name"), Translate.GetString("codingDB_AdaptiveLaneAssist_Description"), "0600", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 7, true);
						BitHelpers.SwitchBitInByte(array, 14, 6, false);
						BitHelpers.SwitchBitInByte(array, 14, 5, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 14, 7, false);
						BitHelpers.SwitchBitInByte(array, 14, 6, true);
						BitHelpers.SwitchBitInByte(array, 14, 5, false);
					}
				}
				if (codingItem.CheckDevice("2Q*980653") || codingItem.CheckDevice("*5WA980653*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 8, 7, true);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 8, 7, false);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
					}
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (BitHelpers.GetBit_0_7(data[14], 7) && !BitHelpers.GetBit_0_7(data[14], 6) && !BitHelpers.GetBit_0_7(data[14], 5))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653") && !codingItem.CheckDevice("*5WA980653*"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					if (BitHelpers.GetBit_0_7(data[8], 7) && !BitHelpers.GetBit_0_7(data[8], 6) && !BitHelpers.GetBit_0_7(data[8], 5))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				RequiresPro = true
			};
		}

		// Token: 0x060054F7 RID: 21751 RVA: 0x004067C0 File Offset: 0x004049C0
		public static ICodingContainer MQB_A5_AdaptavieLaneAssistPatch()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3D", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[25] = 13;
				}
				else
				{
					array[25] = 0;
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1D", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array2[25] = 67;
					}
					else
					{
						array2[25] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[25] = 49;
				}
				else
				{
					array2[25] = 0;
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "A5 adaptation", "", "09A3", "74F", "7B9", "20103", "3Q*980654", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array3, 0, 6, true);
						BitHelpers.SwitchBitInByte(array3, 0, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array3, 0, 6, false);
						BitHelpers.SwitchBitInByte(array3, 0, 5, true);
					}
				}
				return array3;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "A5 adaptation", "", "09A4", "74F", "7B9", "20103", "3Q*980654", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array4, 0, 7, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array4, 0, 7, true);
					}
				}
				return array4;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_AdaptiveLaneAssistPatchForMenuItemActivation_Name"), Translate.GetString("codingDB_AdaptiveLaneAssistPatchForMenuItemActivation_Description"), false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060054F8 RID: 21752 RVA: 0x0040697C File Offset: 0x00404B7C
		public static ICodingContainer HighBeamAssistantSaveState()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A60", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0C", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 1, true);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Assistance, Translate.GetString("codingDB_SaveStateOfHighBeamAssistAfterTurningOffIgnition_Name"), "", VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x00406A28 File Offset: 0x00404C28
		public static ICodingContainer MQB_A5_HighBeamAssistVariants()
		{
			MQBAdaptationOption opt3Q0_NoLightAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_NoLightAssist"), "BIT:21:7=0,21:6=0,21:5=0");
			MQBAdaptationOption opt3Q0_HighBeamAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_HighBeamAssist"), "BIT:21:7=0,21:6=0,21:5=1");
			MQBAdaptationOption opt3Q0_DynamicLightAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_DynamicLightAssist"), "BIT:21:7=0,21:6=1,21:5=0");
			MQBAdaptationOption opt3Q0_AdaptiveHeadlightRangeControl = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AdaptiveHeadlightRangeControl"), "BIT:21:7=0,21:6=1,21:5=1");
			MQBAdaptationOption opt3Q0_MatrixBeam = new MQBAdaptationOption(Translate.GetString("codingDB_opt_MatrixBeam"), "BIT:21:7=1,21:6=0,21:5=0");
			MQBAdaptationOption opt2Q0_NoLightAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_NoLightAssist"), "BIT:15:7=0,15:6=0,15:5=0");
			MQBAdaptationOption opt2Q0_HighBeamAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_HighBeamAssist"), "BIT:15:7=0,15:6=0,15:5=1");
			MQBAdaptationOption opt2Q0_DynamicLightAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_DynamicLightAssist"), "BIT:15:7=0,15:6=1,15:5=0");
			MQBAdaptationOption opt2Q0_AdaptiveHeadlightRangeControl = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AdaptiveHeadlightRangeControl"), "BIT:15:7=0,15:6=1,15:5=1");
			MQBAdaptationOption opt2Q0_MatrixBeam = new MQBAdaptationOption(Translate.GetString("codingDB_opt_MatrixBeam"), "BIT:15:7=1,15:6=0,15:5=0");
			MQBAdaptationOption opt5Q0_NoLightAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_NoLightAssist"), "BIT:2:2=0,2:1=0,2:0=0");
			MQBAdaptationOption opt5Q0_HighBeamAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_HighBeamAssist"), "BIT:2:2=0,2:1=0,2:0=1");
			MQBAdaptationOption opt5Q0_DynamicLightAssist = new MQBAdaptationOption(Translate.GetString("codingDB_opt_DynamicLightAssist"), "BIT:2:2=0,2:1=1,2:0=0");
			MQBAdaptationOption opt5Q0_AdaptiveHeadlightRangeControl = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AdaptiveHeadlightRangeControl"), "BIT:2:2=0,2:1=1,2:0=1");
			MQBAdaptationOption opt5Q0_MatrixBeam = new MQBAdaptationOption(Translate.GetString("codingDB_opt_MatrixBeam"), "BIT:2:2=1,2:1=0,2:0=0");
			return new MQBEasyCodingItem(CodingGroup.Assistance, Translate.GetString("codingDB_HighBeamAssistStep1_Name"), Translate.GetString("codingDB_HighBeamAssistStep1_Description"), "0600", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				try
				{
					string[] array2 = value.Substring(4).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
						int num = int.Parse(array3[0]);
						int num2 = int.Parse(array3[1]);
						bool flag = int.Parse(array3[2]) == 1;
						BitHelpers.SwitchBitInByte(array, num, num2, flag);
					}
				}
				catch (Exception)
				{
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				codingItem.Options.Clear();
				if (codingItem.CheckDevice("3Q*980654") || codingItem.CheckDevice("3Q*980653"))
				{
					codingItem.Options.Add(opt3Q0_NoLightAssist);
					codingItem.Options.Add(opt3Q0_HighBeamAssist);
					codingItem.Options.Add(opt3Q0_DynamicLightAssist);
					codingItem.Options.Add(opt3Q0_AdaptiveHeadlightRangeControl);
					codingItem.Options.Add(opt3Q0_MatrixBeam);
				}
				else if (codingItem.CheckDevice("5Q0980653"))
				{
					codingItem.Options.Add(opt5Q0_NoLightAssist);
					codingItem.Options.Add(opt5Q0_HighBeamAssist);
					codingItem.Options.Add(opt5Q0_DynamicLightAssist);
					codingItem.Options.Add(opt5Q0_AdaptiveHeadlightRangeControl);
					codingItem.Options.Add(opt5Q0_MatrixBeam);
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					codingItem.Options.Add(opt2Q0_NoLightAssist);
					codingItem.Options.Add(opt2Q0_HighBeamAssist);
					codingItem.Options.Add(opt2Q0_DynamicLightAssist);
					codingItem.Options.Add(opt2Q0_AdaptiveHeadlightRangeControl);
					codingItem.Options.Add(opt2Q0_MatrixBeam);
				}
				return codingItem.ConvertRawDataToOptionType(data);
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				InnerDescription = Translate.GetString("codingDB_FernlichtAssistentErweiterteFernlichtassistent_InnerDescription"),
				PostWriteCommands = "14FFFFFF;ATSH757;ATCRA7C1;1003;14FFFFFF;1102;1003;14FFFFFF;1102;ATSH74F;ATCRA7B9"
			};
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x00406C50 File Offset: 0x00404E50
		public static ICodingContainer MQB_09_HighBeamAssistStep2()
		{
			MQBAdaptationOption opt_old_Basis = new MQBAdaptationOption(Translate.GetString("codingDB_opt_Basis"), "BIT:2:2=0;2:1=0;2:0=0");
			MQBAdaptationOption opt_old_BasisFLA = new MQBAdaptationOption(Translate.GetString("codingDB_opt_BasisFla"), "BIT:2:2=0;2:1=0;2:0=1");
			MQBAdaptationOption opt_old_AfsBcmFernlicht = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsBcmFernlicht"), "BIT:2:2=0;2:1=1;2:0=0");
			MQBAdaptationOption opt_old_AfsFlaBcmFernlicht = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsFlaBcmFernlicht"), "BIT:2:2=0;2:1=1;2:0=1");
			MQBAdaptationOption opt_old_AfsFernlichtUeberAfs = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsFernlichtUeberAfs"), "BIT:2:2=1;2:1=0;2:0=0");
			MQBAdaptationOption opt_old_AfsFlaFernlichtUeberAfs = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsFlaFernlichtUeberAfs"), "BIT:2:2=1;2:1=0;2:0=1");
			MQBAdaptationOption opt_old_AfsFlaFernlichtGlwMdf = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsFlaFernlichtGlwMdf"), "BIT:2:2=1;2:1=1;2:0=0");
			MQBAdaptationOption opt_new_Basis = new MQBAdaptationOption(Translate.GetString("codingDB_opt_Basis"), "BIT:0:2=0;0:1=0;0:0=0");
			MQBAdaptationOption opt_new_BasisFLA = new MQBAdaptationOption(Translate.GetString("codingDB_opt_BasisFla"), "BIT:0:2=0;0:1=0;0:0=1");
			MQBAdaptationOption opt_new_AfsBcmFernlicht = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsBcmFernlicht"), "BIT:0:2=0;0:1=1;0:0=0");
			MQBAdaptationOption opt_new_AfsFlaBcmFernlicht = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsFlaBcmFernlicht"), "BIT:0:2=0;0:1=1;0:0=1");
			MQBAdaptationOption opt_new_AfsFernlichtUeberAfs = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsFernlichtUeberAfs"), "BIT:0:2=1;0:1=0;0:0=0");
			MQBAdaptationOption opt_new_AfsFlaFernlichtUeberAfs = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsFlaFernlichtUeberAfs"), "BIT:0:2=1;0:1=0;0:0=1");
			MQBAdaptationOption opt_new_AfsFlaFernlichtGlwMdf = new MQBAdaptationOption(Translate.GetString("codingDB_opt_AfsFlaFernlichtGlwMdf"), "BIT:0:2=1;0:1=1;0:0=0");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(false, "0600", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				try
				{
					string[] array2 = value.Substring(4).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
						int num = int.Parse(array3[0]);
						int num2 = int.Parse(array3[1]);
						bool flag = int.Parse(array3[2]) == 1;
						BitHelpers.SwitchBitInByte(array, num, num2, flag);
					}
				}
				catch (Exception)
				{
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding coding)
			{
				coding.Options.Clear();
				coding.Options.Add(opt_old_Basis);
				coding.Options.Add(opt_old_BasisFLA);
				coding.Options.Add(opt_old_AfsBcmFernlicht);
				coding.Options.Add(opt_old_AfsFlaBcmFernlicht);
				coding.Options.Add(opt_old_AfsFernlichtUeberAfs);
				coding.Options.Add(opt_old_AfsFlaFernlichtUeberAfs);
				coding.Options.Add(opt_old_AfsFlaFernlichtGlwMdf);
				return coding.ConvertRawDataToOptionType(data);
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(true, "0A60", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				try
				{
					string[] array5 = value.Substring(4).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int j = 0; j < array5.Length; j++)
					{
						string[] array6 = array5[j].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
						int num3 = int.Parse(array6[0]);
						int num4 = int.Parse(array6[1]);
						bool flag2 = int.Parse(array6[2]) == 1;
						BitHelpers.SwitchBitInByte(array4, num3, num4, flag2);
					}
				}
				catch (Exception)
				{
				}
				return array4;
			}, delegate(byte[] data, MQBAlternativeCoding coding)
			{
				coding.Options.Clear();
				coding.Options.Add(opt_new_Basis);
				coding.Options.Add(opt_new_BasisFLA);
				coding.Options.Add(opt_new_AfsBcmFernlicht);
				coding.Options.Add(opt_new_AfsFlaBcmFernlicht);
				coding.Options.Add(opt_new_AfsFernlichtUeberAfs);
				coding.Options.Add(opt_new_AfsFlaFernlichtUeberAfs);
				coding.Options.Add(opt_new_AfsFlaFernlichtGlwMdf);
				return coding.ConvertRawDataToOptionType(data);
			});
			return new MQBAlternativeCoding(CodingGroup.Assistance, Translate.GetString("codingDB_FernlichtAssistentErweiterteFernlichtassistent_Name"), Translate.GetString("codingDB_FernlichtAssistentErweiterteFernlichtassistent_Description"), VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				InnerDescription = Translate.GetString("codingDB_FernlichtAssistentErweiterteFernlichtassistent_InnerDescription"),
				PostWriteCommands = "ATSH773;ATCRA7DD;1003;1102;ATSH70E;ATCRA778"
			};
		}

		// Token: 0x060054FB RID: 21755 RVA: 0x00406E9C File Offset: 0x0040509C
		public static ICodingContainer MQB_09_HighBeamAssistantMenu()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A60", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0C", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
					BitHelpers.SwitchBitInByte(array2, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 1, 2, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Assistance, Translate.GetString("codingDB_HighBeamAssistStep3_Name"), "", VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				PostWriteCommands = "ATSH773;ATCRA7DD;1003;1102;ATSH70E;ATCRA778"
			};
		}

		// Token: 0x060054FC RID: 21756 RVA: 0x00406F54 File Offset: 0x00405154
		public static MQBEasyCodingItem MQB_13_OvertakingRightPrevention()
		{
			return new MQBEasyCodingItem(CodingGroup.Assistance, Translate.GetString("codingDB_OvertakingFromTheRightPreventionWhenUsingAcc_Name"), Translate.GetString("codingDB_OvertakingFromTheRightPreventionWhenUsingAcc_Description"), "0600", "757", "7C1", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				RequiresPro = true
			};
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x00406FD4 File Offset: 0x004051D4
		public static ICodingContainer MQB_17_RoadSignDetectionCamera_5Q0()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "0600", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.CheckDevice("5Q0980653"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 1, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 1, 0, false);
					}
					return array2;
				}
				throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "3B66", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (codingItem.CheckDevice("5Q0980653"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array3, 0, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array3, 0, 0, false);
					}
					return array3;
				}
				throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "3B6B", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.CheckDevice("5Q0980653"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array4[0] = 39;
					}
					else
					{
						array4[0] = 0;
					}
					return array4;
				}
				throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 30, 0, true);
					BitHelpers.SwitchBitInByte(array5, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 30, 0, false);
					BitHelpers.SwitchBitInByte(array5, 30, 2, false);
				}
				return array5;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array6, 30, 7, true);
						BitHelpers.SwitchBitInByte(array6, 30, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array6, 30, 7, false);
						BitHelpers.SwitchBitInByte(array6, 30, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array6, 30, 0, true);
					BitHelpers.SwitchBitInByte(array6, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array6, 30, 0, false);
					BitHelpers.SwitchBitInByte(array6, 30, 2, false);
				}
				return array6;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.Assistance, "5F adapt", "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array7, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array7, 33, 0, false);
				}
				return array7;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array8 = new byte[data.Length];
				Array.Copy(data, 0, array8, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array8, 33, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array8, 33, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array8, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array8, 33, 0, false);
				}
				return array8;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding(CodingGroup.Assistance, "5F adapt", "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "3B63", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array9 = new byte[data.Length];
				Array.Copy(data, 0, array9, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array9[0] = 0;
				}
				else
				{
					array9[0] = 1;
				}
				return array9;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_ActivateRoadSignRecognitionSystemForCamera5Q0980653_Name"), Translate.GetString("codingDB_ActivateRoadSignRecognitionSystemForCamera5Q0980653_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbalternativeCoding, mqbalternativeCoding2, mqbeasyCodingItem3 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x00407310 File Offset: 0x00405510
		public static ICodingContainer MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "0600", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 16, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 16, 4, false);
					}
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
					}
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 12, 3, true);
						BitHelpers.SwitchBitInByte(array2, 12, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 12, 3, false);
						BitHelpers.SwitchBitInByte(array2, 12, 4, false);
					}
				}
				return array2;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 30, 0, true);
					BitHelpers.SwitchBitInByte(array3, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 30, 0, false);
					BitHelpers.SwitchBitInByte(array3, 30, 2, false);
				}
				return array3;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array4, 30, 7, true);
						BitHelpers.SwitchBitInByte(array4, 30, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array4, 30, 7, false);
						BitHelpers.SwitchBitInByte(array4, 30, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 30, 0, true);
					BitHelpers.SwitchBitInByte(array4, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 30, 0, false);
					BitHelpers.SwitchBitInByte(array4, 30, 2, false);
				}
				return array4;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.Assistance, "5F adapt", "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 33, 0, false);
				}
				return array5;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array6, 33, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array6, 33, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array6, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array6, 33, 0, false);
				}
				return array6;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding(CodingGroup.Assistance, "5F adapt", "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "3B63", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array7[0] = 0;
				}
				else
				{
					array7[0] = 1;
				}
				return array7;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_ActivateRoadSignRecognitionSystemCameraNavigationSystem_Name"), Translate.GetString("codingDB_ActivateRoadSignRecognitionSystemCameraNavigationSystem_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbalternativeCoding, mqbalternativeCoding2, mqbeasyCodingItem3 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x0040758C File Offset: 0x0040578C
		public static ICodingContainer MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "0600", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 16, 4, true);
						BitHelpers.SwitchBitInByte(array2, 10, 0, false);
						BitHelpers.SwitchBitInByte(array2, 10, 1, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 16, 4, false);
					}
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
					}
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 12, 3, true);
						BitHelpers.SwitchBitInByte(array2, 12, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 12, 3, false);
						BitHelpers.SwitchBitInByte(array2, 12, 4, false);
					}
				}
				return array2;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 30, 0, true);
					BitHelpers.SwitchBitInByte(array3, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 30, 0, false);
					BitHelpers.SwitchBitInByte(array3, 30, 2, false);
				}
				return array3;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array4, 30, 7, true);
						BitHelpers.SwitchBitInByte(array4, 30, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array4, 30, 7, false);
						BitHelpers.SwitchBitInByte(array4, 30, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 30, 0, true);
					BitHelpers.SwitchBitInByte(array4, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 30, 0, false);
					BitHelpers.SwitchBitInByte(array4, 30, 2, false);
				}
				return array4;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.Assistance, "5F adapt", "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 33, 0, false);
				}
				return array5;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array6, 33, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array6, 33, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array6, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array6, 33, 0, false);
				}
				return array6;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding(CodingGroup.Assistance, "5F adapt", "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem(CodingGroup.Assistance, "", "", "3B63", "74F", "7B9", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array7[0] = 1;
				}
				else
				{
					array7[0] = 0;
				}
				return array7;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_ActivateRoadSignRecognitionSystemUseOnlyCameraNoNavigationSystemInstalled_Name"), Translate.GetString("codingDB_ActivateRoadSignRecognitionSystemUseOnlyCameraNoNavigationSystemInstalled_Description"), false, new ICodingContainer[] { mqbeasyCodingItem2, mqbeasyCodingItem, mqbalternativeCoding, mqbalternativeCoding2, mqbeasyCodingItem3 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x00407814 File Offset: 0x00405A14
		public static ICodingContainer MQB_LaneAssist_3Q0980654()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "17", "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, true);
					BitHelpers.SwitchBitInByte(array, 11, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, false);
					BitHelpers.SwitchBitInByte(array, 11, 1, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "A5 coding", "", "0600", "74F", "7B9", "20103", "3Q*980654", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						if (array2.All((byte x) => x == 0))
						{
							byte[] array3 = BitHelpers.ConvertHexToBytesX("0003070600000401002200448050A10098000E200040");
							Array.Copy(array3, array2, array3.Length);
						}
						BitHelpers.SwitchBitInByte(array2, 16, 7, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 16, 7, false);
					}
				}
				return array2;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				if (BitHelpers.GetBit_0_7(data[16], 7))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "A5 adaptation", "", "09A1", "74F", "7B9", "20103", "3Q*980654", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array4, 0, 7, true);
						BitHelpers.SwitchBitInByte(array4, 0, 6, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array4, 0, 7, false);
						BitHelpers.SwitchBitInByte(array4, 0, 6, false);
					}
				}
				return array4;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				if ((BitHelpers.GetBit_0_7(data[0], 7) && !BitHelpers.GetBit_0_7(data[0], 6)) || (!BitHelpers.GetBit_0_7(data[0], 7) && BitHelpers.GetBit_0_7(data[0], 6)))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "A5 adaptation", "", "09A3", "74F", "7B9", "20103", "3Q*980654", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array5, 0, 6, true);
						BitHelpers.SwitchBitInByte(array5, 0, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array5, 0, 6, false);
						BitHelpers.SwitchBitInByte(array5, 0, 5, true);
					}
				}
				return array5;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem5 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "A5 adaptation", "", "09A4", "74F", "7B9", "20103", "3Q*980654", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array6, 0, 7, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array6, 0, 7, true);
					}
				}
				return array6;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem6 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "A5 adaptation", "", "097E", "74F", "7B9", "20103", "3Q*980654", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array7, 0, 7, true);
						BitHelpers.SwitchBitInByte(array7, 0, 6, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array7, 0, 7, false);
						BitHelpers.SwitchBitInByte(array7, 0, 6, false);
					}
				}
				return array7;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem7 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "44 LC", "", "0600", "712", "77C", "19249", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array8 = new byte[data.Length];
				Array.Copy(data, 0, array8, 0, data.Length);
				if (codingItem.CheckDevice("909144"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array8, 0, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array8, 0, 4, false);
					}
				}
				else if (codingItem.CheckDevice("9*143"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array8, 3, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array8, 3, 0, false);
					}
				}
				return array8;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("909144"))
				{
					if (BitHelpers.GetBit_0_7(data[0], 4))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				else
				{
					if (!codingItem.CheckDevice("909143") && !codingItem.CheckDevice("9*143"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					if (BitHelpers.GetBit_0_7(data[3], 0))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array9 = new byte[data.Length];
				Array.Copy(data, 0, array9, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array9, 25, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array9, 25, 0, false);
				}
				return array9;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array10 = new byte[data.Length];
				Array.Copy(data, 0, array10, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array10, 25, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array10, 25, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array10, 25, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array10, 25, 0, false);
				}
				return array10;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array11 = new byte[data.Length];
				Array.Copy(data, 0, array11, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array11, 4, 0, true);
					BitHelpers.SwitchBitInByte(array11, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array11, 4, 0, false);
					BitHelpers.SwitchBitInByte(array11, 4, 2, false);
				}
				return array11;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array12 = new byte[data.Length];
				Array.Copy(data, 0, array12, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array12, 4, 7, true);
						BitHelpers.SwitchBitInByte(array12, 4, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array12, 4, 7, false);
						BitHelpers.SwitchBitInByte(array12, 4, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array12, 4, 0, true);
					BitHelpers.SwitchBitInByte(array12, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array12, 4, 0, false);
					BitHelpers.SwitchBitInByte(array12, 4, 2, false);
				}
				return array12;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			MQBEasyCodingItem mqbeasyCodingItem8 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "Park assist 2 patch for lane assist activation", "", "0600", "70A", "774", "71679", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("298"))
				{
					byte[] array13 = new byte[data.Length];
					Array.Copy(data, 0, array13, 0, data.Length);
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array13, 3, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array13, 3, 5, false);
					}
					return array13;
				}
				throw new WrongDeviceException("Expected device: 298, but Device=" + codingItem.Device + ", ECU=" + codingItem.ECU, ExceptionConsequences.SkipIgnore);
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("298"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				if (BitHelpers.GetBit_0_7(data[3], 5))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			mqbeasyCodingItem8.SkipOnWrongDevice = true;
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_LaneAssistActivationCamera3Q03Qd_Name"), Translate.GetString("codingDB_LaneAssistActivationCamera3Q03Qd_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem5, mqbeasyCodingItem6, mqbeasyCodingItem7, mqbalternativeCoding, mqbalternativeCoding2, mqbeasyCodingItem8 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x00407D70 File Offset: 0x00405F70
		public static MQBEasyCodingItem MQB_13_ActiveCruiseControlAlgorythmSelection()
		{
			return new MQBEasyCodingItem(CodingGroup.Assistance, Translate.GetString("codingDB_ActiveCruiseControlModeDependingOnDriveMode_Name"), Translate.GetString("codingDB_ActiveCruiseControlModeDependingOnDriveMode_Description"), "0600", "757", "7C1", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 7, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x00407DF0 File Offset: 0x00405FF0
		public static ICodingContainer MQB_LaneAssist_2Q0980654()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "A5", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				if (coding.CheckDevice("2Q*98065*"))
				{
					byte[] array = new byte[data.Length];
					Array.Copy(data, 0, array, 0, data.Length);
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 5, 6, true);
						BitHelpers.SwitchBitInByte(array, 8, 7, true);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, true);
						BitHelpers.SwitchBitInByte(array, 9, 1, true);
						BitHelpers.SwitchBitInByte(array, 9, 0, true);
						BitHelpers.SwitchBitInByte(array, 9, 3, true);
						BitHelpers.SwitchBitInByte(array, 9, 2, true);
						BitHelpers.SwitchBitInByte(array, 9, 7, true);
						BitHelpers.SwitchBitInByte(array, 17, 0, true);
						BitHelpers.SwitchBitInByte(array, 17, 3, false);
						BitHelpers.SwitchBitInByte(array, 17, 2, false);
						BitHelpers.SwitchBitInByte(array, 17, 1, true);
						BitHelpers.SwitchBitInByte(array, 17, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 6, false);
						BitHelpers.SwitchBitInByte(array, 17, 5, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 5, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 7, false);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
						BitHelpers.SwitchBitInByte(array, 9, 1, false);
						BitHelpers.SwitchBitInByte(array, 9, 0, true);
						BitHelpers.SwitchBitInByte(array, 9, 3, false);
						BitHelpers.SwitchBitInByte(array, 9, 2, true);
						BitHelpers.SwitchBitInByte(array, 9, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 3, false);
						BitHelpers.SwitchBitInByte(array, 17, 2, false);
						BitHelpers.SwitchBitInByte(array, 17, 1, false);
						BitHelpers.SwitchBitInByte(array, 17, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 6, false);
						BitHelpers.SwitchBitInByte(array, 17, 5, false);
					}
					return array;
				}
				throw new WrongDeviceException("Expected device: 2Q*98065*", ExceptionConsequences.Halt);
			}, null);
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0B3D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[25] = 13;
				}
				else
				{
					array2[25] = 0;
				}
				return array2;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[25] = 67;
				}
				else
				{
					array3[25] = 0;
				}
				return array3;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.Multimedia, "", "", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB3 = new MQBAlternativeContainerFor5FMIB25("0B3C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 4, 0, true);
					BitHelpers.SwitchBitInByte(array4, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 4, 0, false);
					BitHelpers.SwitchBitInByte(array4, 4, 2, false);
				}
				return array4;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB4 = new MQBAlternativeContainerFor5FMIB3("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 4, 7, true);
					BitHelpers.SwitchBitInByte(array5, 4, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 4, 7, false);
					BitHelpers.SwitchBitInByte(array5, 4, 5, false);
				}
				return array5;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding(CodingGroup.Multimedia, "", "", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB3, mqbalternativeContainerFor5FMIB4 });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array6, 4, 6, true);
					BitHelpers.SwitchBitInByte(array6, 11, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array6, 4, 6, false);
					BitHelpers.SwitchBitInByte(array6, 11, 1, false);
				}
				return array6;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0600", "44", "19249", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array7, 3, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array7, 3, 0, false);
				}
				return array7;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_LaneAssistActivationForNewCamera2Q0980653_Name"), Translate.GetString("codingDB_LaneAssistActivationForNewCamera2Q0980653_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbalternativeCoding, mqbalternativeCoding2, mqbeasyCodingItem2, mqbeasyCodingItem3 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x00407FFC File Offset: 0x004061FC
		public static ICodingContainer MQB_LaneAssist_5Q0980653()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "17", "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, true);
					BitHelpers.SwitchBitInByte(array, 11, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, false);
					BitHelpers.SwitchBitInByte(array, 11, 1, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "A5 coding", "", "0600", "74F", "7B9", "20103", "5Q*98065*", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 0, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 0, 0, false);
					}
				}
				return array2;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("5Q*98065*"))
				{
					return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0544", "A5", "20103", "5Q*98065*", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array3, 0, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array3, 0, 0, false);
					}
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0543", "A5", "20103", "5Q*98065*", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array4[0] = 3;
					}
					else
					{
						array4[0] = 0;
					}
				}
				return array4;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem5 = new MQBEasyCodingItem("0541", "A5", "20103", "5Q*98065*", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array5[0] = 3;
					}
					else
					{
						array5[0] = 0;
					}
				}
				return array5;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem6 = new MQBEasyCodingItem("3BB2", "A5", "20103", "5Q*98065*", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array6[0] = 2;
					}
					else
					{
						array6[0] = 0;
					}
				}
				return array6;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem7 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "44 LC", "", "0600", "712", "77C", "19249", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (codingItem.CheckDevice("909144"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array7, 0, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array7, 0, 4, false);
					}
				}
				else if (codingItem.CheckDevice("9*143"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array7, 3, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array7, 3, 0, false);
					}
				}
				return array7;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("909144"))
				{
					if (BitHelpers.GetBit_0_7(data[0], 4))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				else
				{
					if (!codingItem.CheckDevice("909143") && !codingItem.CheckDevice("9*143"))
					{
						return MQBAdaptationTemplate.DisableOption.Title;
					}
					if (BitHelpers.GetBit_0_7(data[3], 0))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array8 = new byte[data.Length];
				Array.Copy(data, 0, array8, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array8, 25, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array8, 25, 0, false);
				}
				return array8;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array9 = new byte[data.Length];
				Array.Copy(data, 0, array9, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array9, 25, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array9, 25, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array9, 25, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array9, 25, 0, false);
				}
				return array9;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array10 = new byte[data.Length];
				Array.Copy(data, 0, array10, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array10, 4, 0, true);
					BitHelpers.SwitchBitInByte(array10, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array10, 4, 0, false);
					BitHelpers.SwitchBitInByte(array10, 4, 2, false);
				}
				return array10;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array11 = new byte[data.Length];
				Array.Copy(data, 0, array11, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array11, 4, 7, true);
						BitHelpers.SwitchBitInByte(array11, 4, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array11, 4, 7, false);
						BitHelpers.SwitchBitInByte(array11, 4, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array11, 4, 0, true);
					BitHelpers.SwitchBitInByte(array11, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array11, 4, 0, false);
					BitHelpers.SwitchBitInByte(array11, 4, 2, false);
				}
				return array11;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			MQBEasyCodingItem mqbeasyCodingItem8 = new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, "Park assist 2 patch for lane assist activation", "", "0600", "70A", "774", "71679", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("298"))
				{
					byte[] array12 = new byte[data.Length];
					Array.Copy(data, 0, array12, 0, data.Length);
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array12, 3, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array12, 3, 5, false);
					}
					return array12;
				}
				throw new WrongDeviceException("Expected device: 298, but Device=" + codingItem.Device + ", ECU=" + codingItem.ECU, ExceptionConsequences.SkipIgnore);
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("298"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				if (BitHelpers.GetBit_0_7(data[3], 5))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			mqbeasyCodingItem8.SkipOnWrongDevice = true;
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_LaneAssistActivationCamera5Q0_Name"), Translate.GetString("codingDB_LaneAssistActivationCamera5Q0_Description"), false, new ICodingContainer[] { mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem5, mqbeasyCodingItem6, mqbeasyCodingItem, mqbeasyCodingItem7, mqbalternativeCoding, mqbalternativeCoding2, mqbeasyCodingItem8 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005504 RID: 21764 RVA: 0x00408444 File Offset: 0x00406644
		public static ICodingContainer EmergencyAssist()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "13", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
					BitHelpers.SwitchBitInByte(array, 6, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
					BitHelpers.SwitchBitInByte(array, 6, 3, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "A5", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 16, 3, true);
						BitHelpers.SwitchBitInByte(array2, 16, 2, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 16, 3, false);
						BitHelpers.SwitchBitInByte(array2, 16, 2, true);
					}
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
					}
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 10, 6, false);
						BitHelpers.SwitchBitInByte(array2, 10, 5, true);
						BitHelpers.SwitchBitInByte(array2, 10, 4, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 10, 6, false);
						BitHelpers.SwitchBitInByte(array2, 10, 5, false);
						BitHelpers.SwitchBitInByte(array2, 10, 4, false);
					}
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0600", "03", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 29, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 29, 5, false);
				}
				return array3;
			}, null)
			{
				PostWriteCommands = "1102"
			};
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("09B6", "03", "37483", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[0] = 1;
				}
				else
				{
					array4[0] = 0;
				}
				return array4;
			}, null)
			{
				PostWriteCommands = "1102"
			};
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A5E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 0, 0, true);
					BitHelpers.SwitchBitInByte(array5, 2, 0, false);
					BitHelpers.SwitchBitInByte(array5, 2, 2, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 0, 0, false);
					BitHelpers.SwitchBitInByte(array5, 2, 0, false);
					BitHelpers.SwitchBitInByte(array5, 2, 2, false);
				}
				return array5;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit });
			return new MQBMultipleCoding(CodingGroup.Assistance, Translate.GetString("codingDB_EmergencyAssist_Name"), Translate.GetString("codingDB_EmergencyAssist_Description"), false, new ICodingContainer[] { mqbeasyCodingItem2, mqbeasyCodingItem, mqbeasyCodingItem3, mqbeasyCodingItem4, mqbalternativeCoding })
			{
				InnerDescription = Translate.GetString("codingDB_EmergencyAssist_InnerDescription"),
				RequiresPro = true
			};
		}

		// Token: 0x02000A87 RID: 2695
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005505 RID: 21765 RVA: 0x004085F4 File Offset: 0x004067F4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005506 RID: 21766 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005507 RID: 21767 RVA: 0x00408600 File Offset: 0x00406800
			internal byte[] <MQB_13_SimpleCruiseControlActivation>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (data.Length >= 24 && (codingItem.CheckDevice("*2Q0907572*") || codingItem.CheckDevice("*5Q0907572*")))
				{
					codingItem.Password = "20103";
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 24, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 24, 4, false);
					}
					return array;
				}
				throw new WrongDeviceException("Unit 13 not supported", ExceptionConsequences.Halt);
			}

			// Token: 0x06005508 RID: 21768 RVA: 0x00408688 File Offset: 0x00406888
			internal byte[] <MQB_13_SimpleCruiseControlActivation>b__0_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}

			// Token: 0x06005509 RID: 21769 RVA: 0x004086D4 File Offset: 0x004068D4
			internal byte[] <MQB_A5_2Q0LaneAssistTJAActivation>b__2_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("2Q0980653") || codingItem.CheckDevice("5WA980653"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 10, 6, false);
						BitHelpers.SwitchBitInByte(array, 10, 5, true);
						BitHelpers.SwitchBitInByte(array, 10, 4, false);
						BitHelpers.SwitchBitInByte(array, 17, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 6, false);
						BitHelpers.SwitchBitInByte(array, 17, 5, true);
						BitHelpers.SwitchBitInByte(array, 8, 7, true);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
						BitHelpers.SwitchBitInByte(array, 17, 3, false);
						BitHelpers.SwitchBitInByte(array, 17, 2, false);
						BitHelpers.SwitchBitInByte(array, 17, 1, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 10, 6, false);
						BitHelpers.SwitchBitInByte(array, 10, 5, false);
						BitHelpers.SwitchBitInByte(array, 10, 4, false);
						BitHelpers.SwitchBitInByte(array, 17, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 6, false);
						BitHelpers.SwitchBitInByte(array, 17, 5, false);
						BitHelpers.SwitchBitInByte(array, 8, 7, false);
						BitHelpers.SwitchBitInByte(array, 8, 6, true);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
						BitHelpers.SwitchBitInByte(array, 17, 3, false);
						BitHelpers.SwitchBitInByte(array, 17, 2, false);
						BitHelpers.SwitchBitInByte(array, 17, 1, false);
					}
					return array;
				}
				throw new WrongDeviceException("Unit 13 not supported", ExceptionConsequences.Halt);
			}

			// Token: 0x0600550A RID: 21770 RVA: 0x00408824 File Offset: 0x00406A24
			internal byte[] <MQB_A5_2Q0LaneAssistTJAActivation>b__2_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (data.Length >= 24 && (codingItem.CheckDevice("*2Q0907572*") || codingItem.CheckDevice("*5Q0907572*")))
				{
					codingItem.Password = "20103";
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 4, 2, true);
						BitHelpers.SwitchBitInByte(array, 11, 0, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 4, 2, false);
						BitHelpers.SwitchBitInByte(array, 11, 0, true);
					}
					return array;
				}
				throw new WrongDeviceException("Unit 13 not supported", ExceptionConsequences.SkipIgnore);
			}

			// Token: 0x0600550B RID: 21771 RVA: 0x004088BC File Offset: 0x00406ABC
			internal byte[] <MQB_A5_AdaptavieLaneAssist>b__5_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 7, true);
						BitHelpers.SwitchBitInByte(array, 14, 6, false);
						BitHelpers.SwitchBitInByte(array, 14, 5, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 14, 7, false);
						BitHelpers.SwitchBitInByte(array, 14, 6, true);
						BitHelpers.SwitchBitInByte(array, 14, 5, false);
					}
				}
				if (codingItem.CheckDevice("2Q*980653") || codingItem.CheckDevice("*5WA980653*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 8, 7, true);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 8, 7, false);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
					}
				}
				return array;
			}

			// Token: 0x0600550C RID: 21772 RVA: 0x004089A0 File Offset: 0x00406BA0
			internal string <MQB_A5_AdaptavieLaneAssist>b__5_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (BitHelpers.GetBit_0_7(data[14], 7) && !BitHelpers.GetBit_0_7(data[14], 6) && !BitHelpers.GetBit_0_7(data[14], 5))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653") && !codingItem.CheckDevice("*5WA980653*"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					if (BitHelpers.GetBit_0_7(data[8], 7) && !BitHelpers.GetBit_0_7(data[8], 6) && !BitHelpers.GetBit_0_7(data[8], 5))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
			}

			// Token: 0x0600550D RID: 21773 RVA: 0x00408A4C File Offset: 0x00406C4C
			internal byte[] <MQB_A5_AdaptavieLaneAssistPatch>b__6_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[25] = 13;
				}
				else
				{
					array[25] = 0;
				}
				return array;
			}

			// Token: 0x0600550E RID: 21774 RVA: 0x00408A90 File Offset: 0x00406C90
			internal byte[] <MQB_A5_AdaptavieLaneAssistPatch>b__6_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[25] = 67;
					}
					else
					{
						array[25] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[25] = 49;
				}
				else
				{
					array[25] = 0;
				}
				return array;
			}

			// Token: 0x0600550F RID: 21775 RVA: 0x00408AFC File Offset: 0x00406CFC
			internal byte[] <MQB_A5_AdaptavieLaneAssistPatch>b__6_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 0, 6, true);
						BitHelpers.SwitchBitInByte(array, 0, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 6, false);
						BitHelpers.SwitchBitInByte(array, 0, 5, true);
					}
				}
				return array;
			}

			// Token: 0x06005510 RID: 21776 RVA: 0x00408B64 File Offset: 0x00406D64
			internal string <MQB_A5_AdaptavieLaneAssistPatch>b__6_3(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005511 RID: 21777 RVA: 0x00408B88 File Offset: 0x00406D88
			internal byte[] <MQB_A5_AdaptavieLaneAssistPatch>b__6_4(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 0, 7, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 7, true);
					}
				}
				return array;
			}

			// Token: 0x06005512 RID: 21778 RVA: 0x00408B64 File Offset: 0x00406D64
			internal string <MQB_A5_AdaptavieLaneAssistPatch>b__6_5(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005513 RID: 21779 RVA: 0x00408BE0 File Offset: 0x00406DE0
			internal byte[] <HighBeamAssistantSaveState>b__7_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
				}
				return array;
			}

			// Token: 0x06005514 RID: 21780 RVA: 0x00408C2C File Offset: 0x00406E2C
			internal byte[] <HighBeamAssistantSaveState>b__7_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				return array;
			}

			// Token: 0x06005515 RID: 21781 RVA: 0x00408C78 File Offset: 0x00406E78
			internal byte[] <MQB_A5_HighBeamAssistVariants>b__8_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				try
				{
					string[] array2 = value.Substring(4).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
						int num = int.Parse(array3[0]);
						int num2 = int.Parse(array3[1]);
						bool flag = int.Parse(array3[2]) == 1;
						BitHelpers.SwitchBitInByte(array, num, num2, flag);
					}
				}
				catch (Exception)
				{
				}
				return array;
			}

			// Token: 0x06005516 RID: 21782 RVA: 0x00408D20 File Offset: 0x00406F20
			internal byte[] <MQB_09_HighBeamAssistStep2>b__9_0(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				try
				{
					string[] array2 = value.Substring(4).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
						int num = int.Parse(array3[0]);
						int num2 = int.Parse(array3[1]);
						bool flag = int.Parse(array3[2]) == 1;
						BitHelpers.SwitchBitInByte(array, num, num2, flag);
					}
				}
				catch (Exception)
				{
				}
				return array;
			}

			// Token: 0x06005517 RID: 21783 RVA: 0x00408DC8 File Offset: 0x00406FC8
			internal byte[] <MQB_09_HighBeamAssistStep2>b__9_2(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				try
				{
					string[] array2 = value.Substring(4).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
						int num = int.Parse(array3[0]);
						int num2 = int.Parse(array3[1]);
						bool flag = int.Parse(array3[2]) == 1;
						BitHelpers.SwitchBitInByte(array, num, num2, flag);
					}
				}
				catch (Exception)
				{
				}
				return array;
			}

			// Token: 0x06005518 RID: 21784 RVA: 0x00408E70 File Offset: 0x00407070
			internal byte[] <MQB_09_HighBeamAssistantMenu>b__10_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}

			// Token: 0x06005519 RID: 21785 RVA: 0x00408ECC File Offset: 0x004070CC
			internal byte[] <MQB_09_HighBeamAssistantMenu>b__10_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}

			// Token: 0x0600551A RID: 21786 RVA: 0x00408F28 File Offset: 0x00407128
			internal byte[] <MQB_13_OvertakingRightPrevention>b__11_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
				}
				return array;
			}

			// Token: 0x0600551B RID: 21787 RVA: 0x00408F74 File Offset: 0x00407174
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, false);
				}
				return array;
			}

			// Token: 0x0600551C RID: 21788 RVA: 0x00408FC0 File Offset: 0x004071C0
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("5Q0980653"))
				{
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
				throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
			}

			// Token: 0x0600551D RID: 21789 RVA: 0x00409024 File Offset: 0x00407224
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("5Q0980653"))
				{
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
				throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
			}

			// Token: 0x0600551E RID: 21790 RVA: 0x00409088 File Offset: 0x00407288
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("5Q0980653"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[0] = 39;
					}
					else
					{
						array[0] = 0;
					}
					return array;
				}
				throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
			}

			// Token: 0x0600551F RID: 21791 RVA: 0x004090E4 File Offset: 0x004072E4
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_4(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, true);
					BitHelpers.SwitchBitInByte(array, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, false);
					BitHelpers.SwitchBitInByte(array, 30, 2, false);
				}
				return array;
			}

			// Token: 0x06005520 RID: 21792 RVA: 0x00409144 File Offset: 0x00407344
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_5(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 30, 7, true);
						BitHelpers.SwitchBitInByte(array, 30, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 30, 7, false);
						BitHelpers.SwitchBitInByte(array, 30, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, true);
					BitHelpers.SwitchBitInByte(array, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, false);
					BitHelpers.SwitchBitInByte(array, 30, 2, false);
				}
				return array;
			}

			// Token: 0x06005521 RID: 21793 RVA: 0x004091EC File Offset: 0x004073EC
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_6(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, false);
				}
				return array;
			}

			// Token: 0x06005522 RID: 21794 RVA: 0x00409238 File Offset: 0x00407438
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_7(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 33, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 33, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, false);
				}
				return array;
			}

			// Token: 0x06005523 RID: 21795 RVA: 0x004092B8 File Offset: 0x004074B8
			internal byte[] <MQB_17_RoadSignDetectionCamera_5Q0>b__12_8(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 0;
				}
				else
				{
					array[0] = 1;
				}
				return array;
			}

			// Token: 0x06005524 RID: 21796 RVA: 0x004092F8 File Offset: 0x004074F8
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi>b__13_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, false);
				}
				return array;
			}

			// Token: 0x06005525 RID: 21797 RVA: 0x00409344 File Offset: 0x00407544
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi>b__13_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 16, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 16, 4, false);
					}
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
					}
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 12, 3, true);
						BitHelpers.SwitchBitInByte(array, 12, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 12, 3, false);
						BitHelpers.SwitchBitInByte(array, 12, 4, false);
					}
				}
				return array;
			}

			// Token: 0x06005526 RID: 21798 RVA: 0x004093F8 File Offset: 0x004075F8
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi>b__13_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, true);
					BitHelpers.SwitchBitInByte(array, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, false);
					BitHelpers.SwitchBitInByte(array, 30, 2, false);
				}
				return array;
			}

			// Token: 0x06005527 RID: 21799 RVA: 0x00409458 File Offset: 0x00407658
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi>b__13_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 30, 7, true);
						BitHelpers.SwitchBitInByte(array, 30, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 30, 7, false);
						BitHelpers.SwitchBitInByte(array, 30, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, true);
					BitHelpers.SwitchBitInByte(array, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, false);
					BitHelpers.SwitchBitInByte(array, 30, 2, false);
				}
				return array;
			}

			// Token: 0x06005528 RID: 21800 RVA: 0x00409500 File Offset: 0x00407700
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi>b__13_4(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, false);
				}
				return array;
			}

			// Token: 0x06005529 RID: 21801 RVA: 0x0040954C File Offset: 0x0040774C
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi>b__13_5(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 33, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 33, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, false);
				}
				return array;
			}

			// Token: 0x0600552A RID: 21802 RVA: 0x004095CC File Offset: 0x004077CC
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi>b__13_6(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 0;
				}
				else
				{
					array[0] = 1;
				}
				return array;
			}

			// Token: 0x0600552B RID: 21803 RVA: 0x0040960C File Offset: 0x0040780C
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi>b__14_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 2, false);
				}
				return array;
			}

			// Token: 0x0600552C RID: 21804 RVA: 0x00409658 File Offset: 0x00407858
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi>b__14_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 16, 4, true);
						BitHelpers.SwitchBitInByte(array, 10, 0, false);
						BitHelpers.SwitchBitInByte(array, 10, 1, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 16, 4, false);
					}
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
					}
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 12, 3, true);
						BitHelpers.SwitchBitInByte(array, 12, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 12, 3, false);
						BitHelpers.SwitchBitInByte(array, 12, 4, false);
					}
				}
				return array;
			}

			// Token: 0x0600552D RID: 21805 RVA: 0x00409720 File Offset: 0x00407920
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi>b__14_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, true);
					BitHelpers.SwitchBitInByte(array, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, false);
					BitHelpers.SwitchBitInByte(array, 30, 2, false);
				}
				return array;
			}

			// Token: 0x0600552E RID: 21806 RVA: 0x00409780 File Offset: 0x00407980
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi>b__14_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 30, 7, true);
						BitHelpers.SwitchBitInByte(array, 30, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 30, 7, false);
						BitHelpers.SwitchBitInByte(array, 30, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, true);
					BitHelpers.SwitchBitInByte(array, 30, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 30, 0, false);
					BitHelpers.SwitchBitInByte(array, 30, 2, false);
				}
				return array;
			}

			// Token: 0x0600552F RID: 21807 RVA: 0x00409828 File Offset: 0x00407A28
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi>b__14_4(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, false);
				}
				return array;
			}

			// Token: 0x06005530 RID: 21808 RVA: 0x00409874 File Offset: 0x00407A74
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi>b__14_5(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 33, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 33, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 33, 0, false);
				}
				return array;
			}

			// Token: 0x06005531 RID: 21809 RVA: 0x004098F4 File Offset: 0x00407AF4
			internal byte[] <MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi>b__14_6(byte[] data, string value, MQBEasyCodingItem codingItem)
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

			// Token: 0x06005532 RID: 21810 RVA: 0x00409934 File Offset: 0x00407B34
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, true);
					BitHelpers.SwitchBitInByte(array, 11, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, false);
					BitHelpers.SwitchBitInByte(array, 11, 1, false);
				}
				return array;
			}

			// Token: 0x06005533 RID: 21811 RVA: 0x00409994 File Offset: 0x00407B94
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						if (array.All((byte x) => x == 0))
						{
							byte[] array2 = BitHelpers.ConvertHexToBytesX("0003070600000401002200448050A10098000E200040");
							Array.Copy(array2, array, array2.Length);
						}
						BitHelpers.SwitchBitInByte(array, 16, 7, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 16, 7, false);
					}
				}
				return array;
			}

			// Token: 0x06005534 RID: 21812 RVA: 0x0037EDD4 File Offset: 0x0037CFD4
			internal bool <MQB_LaneAssist_3Q0980654>b__15_19(byte x)
			{
				return x == 0;
			}

			// Token: 0x06005535 RID: 21813 RVA: 0x00409A28 File Offset: 0x00407C28
			internal string <MQB_LaneAssist_3Q0980654>b__15_2(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				if (BitHelpers.GetBit_0_7(data[16], 7))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005536 RID: 21814 RVA: 0x00409A64 File Offset: 0x00407C64
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
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
				}
				return array;
			}

			// Token: 0x06005537 RID: 21815 RVA: 0x00409ACC File Offset: 0x00407CCC
			internal string <MQB_LaneAssist_3Q0980654>b__15_4(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				if ((BitHelpers.GetBit_0_7(data[0], 7) && !BitHelpers.GetBit_0_7(data[0], 6)) || (!BitHelpers.GetBit_0_7(data[0], 7) && BitHelpers.GetBit_0_7(data[0], 6)))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005538 RID: 21816 RVA: 0x00409B34 File Offset: 0x00407D34
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_5(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 0, 6, true);
						BitHelpers.SwitchBitInByte(array, 0, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 6, false);
						BitHelpers.SwitchBitInByte(array, 0, 5, true);
					}
				}
				return array;
			}

			// Token: 0x06005539 RID: 21817 RVA: 0x00408B64 File Offset: 0x00406D64
			internal string <MQB_LaneAssist_3Q0980654>b__15_6(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600553A RID: 21818 RVA: 0x00409B9C File Offset: 0x00407D9C
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_7(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 0, 7, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 7, true);
					}
				}
				return array;
			}

			// Token: 0x0600553B RID: 21819 RVA: 0x00408B64 File Offset: 0x00406D64
			internal string <MQB_LaneAssist_3Q0980654>b__15_8(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600553C RID: 21820 RVA: 0x00409BF4 File Offset: 0x00407DF4
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_9(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
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
				}
				return array;
			}

			// Token: 0x0600553D RID: 21821 RVA: 0x00408B64 File Offset: 0x00406D64
			internal string <MQB_LaneAssist_3Q0980654>b__15_10(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600553E RID: 21822 RVA: 0x00409C5C File Offset: 0x00407E5C
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_11(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("909144"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 0, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 4, false);
					}
				}
				else if (codingItem.CheckDevice("9*143"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 3, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 3, 0, false);
					}
				}
				return array;
			}

			// Token: 0x0600553F RID: 21823 RVA: 0x00409CE8 File Offset: 0x00407EE8
			internal string <MQB_LaneAssist_3Q0980654>b__15_12(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("909144"))
				{
					if (BitHelpers.GetBit_0_7(data[0], 4))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				else
				{
					if (!codingItem.CheckDevice("909143") && !codingItem.CheckDevice("9*143"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					if (BitHelpers.GetBit_0_7(data[3], 0))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
			}

			// Token: 0x06005540 RID: 21824 RVA: 0x00409D64 File Offset: 0x00407F64
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_13(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005541 RID: 21825 RVA: 0x00409DB0 File Offset: 0x00407FB0
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_14(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 25, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 25, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 25, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 25, 0, false);
				}
				return array;
			}

			// Token: 0x06005542 RID: 21826 RVA: 0x00409E30 File Offset: 0x00408030
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_15(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
				}
				return array;
			}

			// Token: 0x06005543 RID: 21827 RVA: 0x00409E8C File Offset: 0x0040808C
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_16(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 4, 7, true);
						BitHelpers.SwitchBitInByte(array, 4, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 4, 7, false);
						BitHelpers.SwitchBitInByte(array, 4, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
				}
				return array;
			}

			// Token: 0x06005544 RID: 21828 RVA: 0x00409F2C File Offset: 0x0040812C
			internal byte[] <MQB_LaneAssist_3Q0980654>b__15_17(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("298"))
				{
					byte[] array = new byte[data.Length];
					Array.Copy(data, 0, array, 0, data.Length);
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 3, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 3, 5, false);
					}
					return array;
				}
				throw new WrongDeviceException("Expected device: 298, but Device=" + codingItem.Device + ", ECU=" + codingItem.ECU, ExceptionConsequences.SkipIgnore);
			}

			// Token: 0x06005545 RID: 21829 RVA: 0x00409FA4 File Offset: 0x004081A4
			internal string <MQB_LaneAssist_3Q0980654>b__15_18(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("298"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				if (BitHelpers.GetBit_0_7(data[3], 5))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005546 RID: 21830 RVA: 0x00409FE0 File Offset: 0x004081E0
			internal byte[] <MQB_13_ActiveCruiseControlAlgorythmSelection>b__16_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 7, false);
				}
				return array;
			}

			// Token: 0x06005547 RID: 21831 RVA: 0x0040A02C File Offset: 0x0040822C
			internal byte[] <MQB_LaneAssist_2Q0980654>b__17_0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				if (coding.CheckDevice("2Q*98065*"))
				{
					byte[] array = new byte[data.Length];
					Array.Copy(data, 0, array, 0, data.Length);
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 5, 6, true);
						BitHelpers.SwitchBitInByte(array, 8, 7, true);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, true);
						BitHelpers.SwitchBitInByte(array, 9, 1, true);
						BitHelpers.SwitchBitInByte(array, 9, 0, true);
						BitHelpers.SwitchBitInByte(array, 9, 3, true);
						BitHelpers.SwitchBitInByte(array, 9, 2, true);
						BitHelpers.SwitchBitInByte(array, 9, 7, true);
						BitHelpers.SwitchBitInByte(array, 17, 0, true);
						BitHelpers.SwitchBitInByte(array, 17, 3, false);
						BitHelpers.SwitchBitInByte(array, 17, 2, false);
						BitHelpers.SwitchBitInByte(array, 17, 1, true);
						BitHelpers.SwitchBitInByte(array, 17, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 6, false);
						BitHelpers.SwitchBitInByte(array, 17, 5, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 5, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 7, false);
						BitHelpers.SwitchBitInByte(array, 8, 6, false);
						BitHelpers.SwitchBitInByte(array, 8, 5, false);
						BitHelpers.SwitchBitInByte(array, 9, 1, false);
						BitHelpers.SwitchBitInByte(array, 9, 0, true);
						BitHelpers.SwitchBitInByte(array, 9, 3, false);
						BitHelpers.SwitchBitInByte(array, 9, 2, true);
						BitHelpers.SwitchBitInByte(array, 9, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 3, false);
						BitHelpers.SwitchBitInByte(array, 17, 2, false);
						BitHelpers.SwitchBitInByte(array, 17, 1, false);
						BitHelpers.SwitchBitInByte(array, 17, 7, false);
						BitHelpers.SwitchBitInByte(array, 17, 6, false);
						BitHelpers.SwitchBitInByte(array, 17, 5, false);
					}
					return array;
				}
				throw new WrongDeviceException("Expected device: 2Q*98065*", ExceptionConsequences.Halt);
			}

			// Token: 0x06005548 RID: 21832 RVA: 0x0040A1B4 File Offset: 0x004083B4
			internal byte[] <MQB_LaneAssist_2Q0980654>b__17_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[25] = 13;
				}
				else
				{
					array[25] = 0;
				}
				return array;
			}

			// Token: 0x06005549 RID: 21833 RVA: 0x0040A1F8 File Offset: 0x004083F8
			internal byte[] <MQB_LaneAssist_2Q0980654>b__17_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[25] = 67;
				}
				else
				{
					array[25] = 0;
				}
				return array;
			}

			// Token: 0x0600554A RID: 21834 RVA: 0x0040A23C File Offset: 0x0040843C
			internal byte[] <MQB_LaneAssist_2Q0980654>b__17_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
				}
				return array;
			}

			// Token: 0x0600554B RID: 21835 RVA: 0x0040A298 File Offset: 0x00408498
			internal byte[] <MQB_LaneAssist_2Q0980654>b__17_4(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, true);
					BitHelpers.SwitchBitInByte(array, 4, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, false);
					BitHelpers.SwitchBitInByte(array, 4, 5, false);
				}
				return array;
			}

			// Token: 0x0600554C RID: 21836 RVA: 0x0040A2F4 File Offset: 0x004084F4
			internal byte[] <MQB_LaneAssist_2Q0980654>b__17_5(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, true);
					BitHelpers.SwitchBitInByte(array, 11, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, false);
					BitHelpers.SwitchBitInByte(array, 11, 1, false);
				}
				return array;
			}

			// Token: 0x0600554D RID: 21837 RVA: 0x0040A354 File Offset: 0x00408554
			internal byte[] <MQB_LaneAssist_2Q0980654>b__17_6(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
				}
				return array;
			}

			// Token: 0x0600554E RID: 21838 RVA: 0x0040A3A0 File Offset: 0x004085A0
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, true);
					BitHelpers.SwitchBitInByte(array, 11, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 6, false);
					BitHelpers.SwitchBitInByte(array, 11, 1, false);
				}
				return array;
			}

			// Token: 0x0600554F RID: 21839 RVA: 0x0040A400 File Offset: 0x00408600
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 0, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 0, false);
					}
				}
				return array;
			}

			// Token: 0x06005550 RID: 21840 RVA: 0x0040A456 File Offset: 0x00408656
			internal string <MQB_LaneAssist_5Q0980653>b__18_2(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("5Q*98065*"))
				{
					return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				}
				if (BitHelpers.GetBit_0_7(data[0], 0))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005551 RID: 21841 RVA: 0x0040A48C File Offset: 0x0040868C
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 0, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 0, false);
					}
				}
				return array;
			}

			// Token: 0x06005552 RID: 21842 RVA: 0x0040A4E4 File Offset: 0x004086E4
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_4(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[0] = 3;
					}
					else
					{
						array[0] = 0;
					}
				}
				return array;
			}

			// Token: 0x06005553 RID: 21843 RVA: 0x0040A530 File Offset: 0x00408730
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_5(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[0] = 3;
					}
					else
					{
						array[0] = 0;
					}
				}
				return array;
			}

			// Token: 0x06005554 RID: 21844 RVA: 0x0040A57C File Offset: 0x0040877C
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_6(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("5Q*98065*"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[0] = 2;
					}
					else
					{
						array[0] = 0;
					}
				}
				return array;
			}

			// Token: 0x06005555 RID: 21845 RVA: 0x0040A5C8 File Offset: 0x004087C8
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_7(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("909144"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 0, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 4, false);
					}
				}
				else if (codingItem.CheckDevice("9*143"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 3, 0, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 3, 0, false);
					}
				}
				return array;
			}

			// Token: 0x06005556 RID: 21846 RVA: 0x0040A654 File Offset: 0x00408854
			internal string <MQB_LaneAssist_5Q0980653>b__18_8(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("909144"))
				{
					if (BitHelpers.GetBit_0_7(data[0], 4))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				else
				{
					if (!codingItem.CheckDevice("909143") && !codingItem.CheckDevice("9*143"))
					{
						return MQBAdaptationTemplate.DisableOption.Title;
					}
					if (BitHelpers.GetBit_0_7(data[3], 0))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					return MQBAdaptationTemplate.DisableOption.Title;
				}
			}

			// Token: 0x06005557 RID: 21847 RVA: 0x0040A6D4 File Offset: 0x004088D4
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_9(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005558 RID: 21848 RVA: 0x0040A720 File Offset: 0x00408920
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_10(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 25, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 25, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 25, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 25, 0, false);
				}
				return array;
			}

			// Token: 0x06005559 RID: 21849 RVA: 0x0040A7A0 File Offset: 0x004089A0
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_11(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
				}
				return array;
			}

			// Token: 0x0600555A RID: 21850 RVA: 0x0040A7FC File Offset: 0x004089FC
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_12(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 4, 7, true);
						BitHelpers.SwitchBitInByte(array, 4, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 4, 7, false);
						BitHelpers.SwitchBitInByte(array, 4, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
				}
				return array;
			}

			// Token: 0x0600555B RID: 21851 RVA: 0x0040A89C File Offset: 0x00408A9C
			internal byte[] <MQB_LaneAssist_5Q0980653>b__18_13(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("298"))
				{
					byte[] array = new byte[data.Length];
					Array.Copy(data, 0, array, 0, data.Length);
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 3, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 3, 5, false);
					}
					return array;
				}
				throw new WrongDeviceException("Expected device: 298, but Device=" + codingItem.Device + ", ECU=" + codingItem.ECU, ExceptionConsequences.SkipIgnore);
			}

			// Token: 0x0600555C RID: 21852 RVA: 0x00409FA4 File Offset: 0x004081A4
			internal string <MQB_LaneAssist_5Q0980653>b__18_14(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!codingItem.CheckDevice("298"))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				if (BitHelpers.GetBit_0_7(data[3], 5))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600555D RID: 21853 RVA: 0x0040A914 File Offset: 0x00408B14
			internal byte[] <EmergencyAssist>b__19_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
					BitHelpers.SwitchBitInByte(array, 6, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
					BitHelpers.SwitchBitInByte(array, 6, 3, false);
				}
				return array;
			}

			// Token: 0x0600555E RID: 21854 RVA: 0x0040A970 File Offset: 0x00408B70
			internal byte[] <EmergencyAssist>b__19_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 16, 3, true);
						BitHelpers.SwitchBitInByte(array, 16, 2, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 16, 3, false);
						BitHelpers.SwitchBitInByte(array, 16, 2, true);
					}
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						throw new WrongDeviceException("A5 device not supported", ExceptionConsequences.Halt);
					}
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 10, 6, false);
						BitHelpers.SwitchBitInByte(array, 10, 5, true);
						BitHelpers.SwitchBitInByte(array, 10, 4, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 10, 6, false);
						BitHelpers.SwitchBitInByte(array, 10, 5, false);
						BitHelpers.SwitchBitInByte(array, 10, 4, false);
					}
				}
				return array;
			}

			// Token: 0x0600555F RID: 21855 RVA: 0x0040AA4C File Offset: 0x00408C4C
			internal byte[] <EmergencyAssist>b__19_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 29, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 29, 5, false);
				}
				return array;
			}

			// Token: 0x06005560 RID: 21856 RVA: 0x0040AA98 File Offset: 0x00408C98
			internal byte[] <EmergencyAssist>b__19_4(byte[] data, string value, MQBEasyCodingItem codingItem)
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

			// Token: 0x06005561 RID: 21857 RVA: 0x0040AAD8 File Offset: 0x00408CD8
			internal byte[] <EmergencyAssist>b__19_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
				}
				return array;
			}

			// Token: 0x040033F9 RID: 13305
			public static readonly Assistance.<>c <>9 = new Assistance.<>c();

			// Token: 0x040033FA RID: 13306
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x040033FB RID: 13307
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_1;

			// Token: 0x040033FC RID: 13308
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x040033FD RID: 13309
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_1;

			// Token: 0x040033FE RID: 13310
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_0;

			// Token: 0x040033FF RID: 13311
			public static Func<byte[], MQBEasyCodingItem, string> <>9__5_1;

			// Token: 0x04003400 RID: 13312
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__6_0;

			// Token: 0x04003401 RID: 13313
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__6_1;

			// Token: 0x04003402 RID: 13314
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_2;

			// Token: 0x04003403 RID: 13315
			public static Func<byte[], MQBEasyCodingItem, string> <>9__6_3;

			// Token: 0x04003404 RID: 13316
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_4;

			// Token: 0x04003405 RID: 13317
			public static Func<byte[], MQBEasyCodingItem, string> <>9__6_5;

			// Token: 0x04003406 RID: 13318
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__7_0;

			// Token: 0x04003407 RID: 13319
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__7_1;

			// Token: 0x04003408 RID: 13320
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__8_0;

			// Token: 0x04003409 RID: 13321
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_0;

			// Token: 0x0400340A RID: 13322
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_2;

			// Token: 0x0400340B RID: 13323
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_0;

			// Token: 0x0400340C RID: 13324
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_1;

			// Token: 0x0400340D RID: 13325
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__11_0;

			// Token: 0x0400340E RID: 13326
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__12_0;

			// Token: 0x0400340F RID: 13327
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__12_1;

			// Token: 0x04003410 RID: 13328
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__12_2;

			// Token: 0x04003411 RID: 13329
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__12_3;

			// Token: 0x04003412 RID: 13330
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_4;

			// Token: 0x04003413 RID: 13331
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_5;

			// Token: 0x04003414 RID: 13332
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_6;

			// Token: 0x04003415 RID: 13333
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_7;

			// Token: 0x04003416 RID: 13334
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__12_8;

			// Token: 0x04003417 RID: 13335
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__13_0;

			// Token: 0x04003418 RID: 13336
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__13_1;

			// Token: 0x04003419 RID: 13337
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__13_2;

			// Token: 0x0400341A RID: 13338
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__13_3;

			// Token: 0x0400341B RID: 13339
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__13_4;

			// Token: 0x0400341C RID: 13340
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__13_5;

			// Token: 0x0400341D RID: 13341
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__13_6;

			// Token: 0x0400341E RID: 13342
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_0;

			// Token: 0x0400341F RID: 13343
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_1;

			// Token: 0x04003420 RID: 13344
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__14_2;

			// Token: 0x04003421 RID: 13345
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__14_3;

			// Token: 0x04003422 RID: 13346
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__14_4;

			// Token: 0x04003423 RID: 13347
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__14_5;

			// Token: 0x04003424 RID: 13348
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_6;

			// Token: 0x04003425 RID: 13349
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_0;

			// Token: 0x04003426 RID: 13350
			public static Func<byte, bool> <>9__15_19;

			// Token: 0x04003427 RID: 13351
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_1;

			// Token: 0x04003428 RID: 13352
			public static Func<byte[], MQBEasyCodingItem, string> <>9__15_2;

			// Token: 0x04003429 RID: 13353
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_3;

			// Token: 0x0400342A RID: 13354
			public static Func<byte[], MQBEasyCodingItem, string> <>9__15_4;

			// Token: 0x0400342B RID: 13355
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_5;

			// Token: 0x0400342C RID: 13356
			public static Func<byte[], MQBEasyCodingItem, string> <>9__15_6;

			// Token: 0x0400342D RID: 13357
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_7;

			// Token: 0x0400342E RID: 13358
			public static Func<byte[], MQBEasyCodingItem, string> <>9__15_8;

			// Token: 0x0400342F RID: 13359
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_9;

			// Token: 0x04003430 RID: 13360
			public static Func<byte[], MQBEasyCodingItem, string> <>9__15_10;

			// Token: 0x04003431 RID: 13361
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_11;

			// Token: 0x04003432 RID: 13362
			public static Func<byte[], MQBEasyCodingItem, string> <>9__15_12;

			// Token: 0x04003433 RID: 13363
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_13;

			// Token: 0x04003434 RID: 13364
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_14;

			// Token: 0x04003435 RID: 13365
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_15;

			// Token: 0x04003436 RID: 13366
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_16;

			// Token: 0x04003437 RID: 13367
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_17;

			// Token: 0x04003438 RID: 13368
			public static Func<byte[], MQBEasyCodingItem, string> <>9__15_18;

			// Token: 0x04003439 RID: 13369
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__16_0;

			// Token: 0x0400343A RID: 13370
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__17_0;

			// Token: 0x0400343B RID: 13371
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__17_1;

			// Token: 0x0400343C RID: 13372
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__17_2;

			// Token: 0x0400343D RID: 13373
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__17_3;

			// Token: 0x0400343E RID: 13374
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__17_4;

			// Token: 0x0400343F RID: 13375
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__17_5;

			// Token: 0x04003440 RID: 13376
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__17_6;

			// Token: 0x04003441 RID: 13377
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__18_0;

			// Token: 0x04003442 RID: 13378
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__18_1;

			// Token: 0x04003443 RID: 13379
			public static Func<byte[], MQBEasyCodingItem, string> <>9__18_2;

			// Token: 0x04003444 RID: 13380
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__18_3;

			// Token: 0x04003445 RID: 13381
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__18_4;

			// Token: 0x04003446 RID: 13382
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__18_5;

			// Token: 0x04003447 RID: 13383
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__18_6;

			// Token: 0x04003448 RID: 13384
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__18_7;

			// Token: 0x04003449 RID: 13385
			public static Func<byte[], MQBEasyCodingItem, string> <>9__18_8;

			// Token: 0x0400344A RID: 13386
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__18_9;

			// Token: 0x0400344B RID: 13387
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__18_10;

			// Token: 0x0400344C RID: 13388
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__18_11;

			// Token: 0x0400344D RID: 13389
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__18_12;

			// Token: 0x0400344E RID: 13390
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__18_13;

			// Token: 0x0400344F RID: 13391
			public static Func<byte[], MQBEasyCodingItem, string> <>9__18_14;

			// Token: 0x04003450 RID: 13392
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__19_0;

			// Token: 0x04003451 RID: 13393
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__19_1;

			// Token: 0x04003452 RID: 13394
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__19_3;

			// Token: 0x04003453 RID: 13395
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__19_4;

			// Token: 0x04003454 RID: 13396
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__19_2;
		}

		// Token: 0x02000A88 RID: 2696
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06005562 RID: 21858 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06005563 RID: 21859 RVA: 0x0040AB48 File Offset: 0x00408D48
			internal byte[] <MQB_13_ACC_OperationMode>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("2Q0907572*"))
				{
					codingItem.Password = "20103";
					if (value == this.opt1kmh.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 4, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 14, 4, false);
					}
				}
				else if (codingItem.CheckDevice("*5Q0907572*"))
				{
					codingItem.Password = "14117";
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 2, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 2, 6, false);
					}
				}
				return array;
			}

			// Token: 0x06005564 RID: 21860 RVA: 0x0040ABEC File Offset: 0x00408DEC
			internal string <MQB_13_ACC_OperationMode>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("2Q0907572*"))
				{
					codingItem.Password = "20103";
					if (BitHelpers.GetBit_0_7(data[14], 4))
					{
						return this.opt1kmh.Title;
					}
					return this.opt10kmh.Title;
				}
				else
				{
					if (!codingItem.CheckDevice("5Q0907572*"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					codingItem.Password = "14117";
					if (BitHelpers.GetBit_0_7(data[2], 6))
					{
						return this.opt1kmh.Title;
					}
					return this.opt10kmh.Title;
				}
			}

			// Token: 0x04003455 RID: 13397
			public MQBAdaptationOption opt1kmh;

			// Token: 0x04003456 RID: 13398
			public MQBAdaptationOption opt10kmh;
		}

		// Token: 0x02000A89 RID: 2697
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06005565 RID: 21861 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005566 RID: 21862 RVA: 0x0040AC78 File Offset: 0x00408E78
			internal byte[] <MQB_A5_LaneAssistSaveState>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 3, false);
						BitHelpers.SwitchBitInByte(array, 14, 2, true);
					}
					else if (value == MQBAdaptationTemplate.DisableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 3, false);
						BitHelpers.SwitchBitInByte(array, 14, 2, false);
					}
					else if (value == this.lastSettingOpt.Value)
					{
						BitHelpers.SwitchBitInByte(array, 14, 3, true);
						BitHelpers.SwitchBitInByte(array, 14, 2, false);
					}
				}
				if (codingItem.CheckDevice("2Q*980653"))
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 9, 1, false);
						BitHelpers.SwitchBitInByte(array, 9, 0, true);
					}
					else if (value == MQBAdaptationTemplate.DisableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 9, 1, false);
						BitHelpers.SwitchBitInByte(array, 9, 0, false);
					}
					else if (value == this.lastSettingOpt.Value)
					{
						BitHelpers.SwitchBitInByte(array, 9, 1, true);
						BitHelpers.SwitchBitInByte(array, 9, 0, false);
					}
				}
				return array;
			}

			// Token: 0x06005567 RID: 21863 RVA: 0x0040ADA4 File Offset: 0x00408FA4
			internal string <MQB_A5_LaneAssistSaveState>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (codingItem.CheckDevice("3Q*980654"))
				{
					if (!BitHelpers.GetBit_0_7(data[14], 3) && BitHelpers.GetBit_0_7(data[14], 2))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					if (!BitHelpers.GetBit_0_7(data[14], 3) && !BitHelpers.GetBit_0_7(data[14], 2))
					{
						return MQBAdaptationTemplate.DisableOption.Title;
					}
					if (BitHelpers.GetBit_0_7(data[14], 3) && !BitHelpers.GetBit_0_7(data[14], 2))
					{
						return this.lastSettingOpt.Title;
					}
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					if (!BitHelpers.GetBit_0_7(data[9], 1) && BitHelpers.GetBit_0_7(data[9], 0))
					{
						return MQBAdaptationTemplate.EnableOption.Title;
					}
					if (!BitHelpers.GetBit_0_7(data[9], 1) && !BitHelpers.GetBit_0_7(data[9], 0))
					{
						return MQBAdaptationTemplate.DisableOption.Title;
					}
					if (BitHelpers.GetBit_0_7(data[9], 1) && !BitHelpers.GetBit_0_7(data[9], 0))
					{
						return this.lastSettingOpt.Title;
					}
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
			}

			// Token: 0x04003457 RID: 13399
			public MQBAdaptationOption lastSettingOpt;
		}

		// Token: 0x02000A8A RID: 2698
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06005568 RID: 21864 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06005569 RID: 21865 RVA: 0x0040AEB0 File Offset: 0x004090B0
			internal byte[] <MQB_13_ACC_WaitTime>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == this.optShort.Value)
				{
					BitHelpers.SwitchBitInByte(array, 11, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 11, 0, false);
				}
				return array;
			}

			// Token: 0x04003458 RID: 13400
			public MQBAdaptationOption optShort;
		}

		// Token: 0x02000A8B RID: 2699
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x0600556A RID: 21866 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x0600556B RID: 21867 RVA: 0x0040AEFC File Offset: 0x004090FC
			internal string <MQB_A5_HighBeamAssistVariants>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				codingItem.Options.Clear();
				if (codingItem.CheckDevice("3Q*980654") || codingItem.CheckDevice("3Q*980653"))
				{
					codingItem.Options.Add(this.opt3Q0_NoLightAssist);
					codingItem.Options.Add(this.opt3Q0_HighBeamAssist);
					codingItem.Options.Add(this.opt3Q0_DynamicLightAssist);
					codingItem.Options.Add(this.opt3Q0_AdaptiveHeadlightRangeControl);
					codingItem.Options.Add(this.opt3Q0_MatrixBeam);
				}
				else if (codingItem.CheckDevice("5Q0980653"))
				{
					codingItem.Options.Add(this.opt5Q0_NoLightAssist);
					codingItem.Options.Add(this.opt5Q0_HighBeamAssist);
					codingItem.Options.Add(this.opt5Q0_DynamicLightAssist);
					codingItem.Options.Add(this.opt5Q0_AdaptiveHeadlightRangeControl);
					codingItem.Options.Add(this.opt5Q0_MatrixBeam);
				}
				else
				{
					if (!codingItem.CheckDevice("2Q*980653"))
					{
						return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					codingItem.Options.Add(this.opt2Q0_NoLightAssist);
					codingItem.Options.Add(this.opt2Q0_HighBeamAssist);
					codingItem.Options.Add(this.opt2Q0_DynamicLightAssist);
					codingItem.Options.Add(this.opt2Q0_AdaptiveHeadlightRangeControl);
					codingItem.Options.Add(this.opt2Q0_MatrixBeam);
				}
				return codingItem.ConvertRawDataToOptionType(data);
			}

			// Token: 0x04003459 RID: 13401
			public MQBAdaptationOption opt3Q0_NoLightAssist;

			// Token: 0x0400345A RID: 13402
			public MQBAdaptationOption opt3Q0_HighBeamAssist;

			// Token: 0x0400345B RID: 13403
			public MQBAdaptationOption opt3Q0_DynamicLightAssist;

			// Token: 0x0400345C RID: 13404
			public MQBAdaptationOption opt3Q0_AdaptiveHeadlightRangeControl;

			// Token: 0x0400345D RID: 13405
			public MQBAdaptationOption opt3Q0_MatrixBeam;

			// Token: 0x0400345E RID: 13406
			public MQBAdaptationOption opt5Q0_NoLightAssist;

			// Token: 0x0400345F RID: 13407
			public MQBAdaptationOption opt5Q0_HighBeamAssist;

			// Token: 0x04003460 RID: 13408
			public MQBAdaptationOption opt5Q0_DynamicLightAssist;

			// Token: 0x04003461 RID: 13409
			public MQBAdaptationOption opt5Q0_AdaptiveHeadlightRangeControl;

			// Token: 0x04003462 RID: 13410
			public MQBAdaptationOption opt5Q0_MatrixBeam;

			// Token: 0x04003463 RID: 13411
			public MQBAdaptationOption opt2Q0_NoLightAssist;

			// Token: 0x04003464 RID: 13412
			public MQBAdaptationOption opt2Q0_HighBeamAssist;

			// Token: 0x04003465 RID: 13413
			public MQBAdaptationOption opt2Q0_DynamicLightAssist;

			// Token: 0x04003466 RID: 13414
			public MQBAdaptationOption opt2Q0_AdaptiveHeadlightRangeControl;

			// Token: 0x04003467 RID: 13415
			public MQBAdaptationOption opt2Q0_MatrixBeam;
		}

		// Token: 0x02000A8C RID: 2700
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x0600556C RID: 21868 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x0600556D RID: 21869 RVA: 0x0040B060 File Offset: 0x00409260
			internal string <MQB_09_HighBeamAssistStep2>b__1(byte[] data, MQBAlternativeCoding coding)
			{
				coding.Options.Clear();
				coding.Options.Add(this.opt_old_Basis);
				coding.Options.Add(this.opt_old_BasisFLA);
				coding.Options.Add(this.opt_old_AfsBcmFernlicht);
				coding.Options.Add(this.opt_old_AfsFlaBcmFernlicht);
				coding.Options.Add(this.opt_old_AfsFernlichtUeberAfs);
				coding.Options.Add(this.opt_old_AfsFlaFernlichtUeberAfs);
				coding.Options.Add(this.opt_old_AfsFlaFernlichtGlwMdf);
				return coding.ConvertRawDataToOptionType(data);
			}

			// Token: 0x0600556E RID: 21870 RVA: 0x0040B0F8 File Offset: 0x004092F8
			internal string <MQB_09_HighBeamAssistStep2>b__3(byte[] data, MQBAlternativeCoding coding)
			{
				coding.Options.Clear();
				coding.Options.Add(this.opt_new_Basis);
				coding.Options.Add(this.opt_new_BasisFLA);
				coding.Options.Add(this.opt_new_AfsBcmFernlicht);
				coding.Options.Add(this.opt_new_AfsFlaBcmFernlicht);
				coding.Options.Add(this.opt_new_AfsFernlichtUeberAfs);
				coding.Options.Add(this.opt_new_AfsFlaFernlichtUeberAfs);
				coding.Options.Add(this.opt_new_AfsFlaFernlichtGlwMdf);
				return coding.ConvertRawDataToOptionType(data);
			}

			// Token: 0x04003468 RID: 13416
			public MQBAdaptationOption opt_old_Basis;

			// Token: 0x04003469 RID: 13417
			public MQBAdaptationOption opt_old_BasisFLA;

			// Token: 0x0400346A RID: 13418
			public MQBAdaptationOption opt_old_AfsBcmFernlicht;

			// Token: 0x0400346B RID: 13419
			public MQBAdaptationOption opt_old_AfsFlaBcmFernlicht;

			// Token: 0x0400346C RID: 13420
			public MQBAdaptationOption opt_old_AfsFernlichtUeberAfs;

			// Token: 0x0400346D RID: 13421
			public MQBAdaptationOption opt_old_AfsFlaFernlichtUeberAfs;

			// Token: 0x0400346E RID: 13422
			public MQBAdaptationOption opt_old_AfsFlaFernlichtGlwMdf;

			// Token: 0x0400346F RID: 13423
			public MQBAdaptationOption opt_new_Basis;

			// Token: 0x04003470 RID: 13424
			public MQBAdaptationOption opt_new_BasisFLA;

			// Token: 0x04003471 RID: 13425
			public MQBAdaptationOption opt_new_AfsBcmFernlicht;

			// Token: 0x04003472 RID: 13426
			public MQBAdaptationOption opt_new_AfsFlaBcmFernlicht;

			// Token: 0x04003473 RID: 13427
			public MQBAdaptationOption opt_new_AfsFernlichtUeberAfs;

			// Token: 0x04003474 RID: 13428
			public MQBAdaptationOption opt_new_AfsFlaFernlichtUeberAfs;

			// Token: 0x04003475 RID: 13429
			public MQBAdaptationOption opt_new_AfsFlaFernlichtGlwMdf;
		}
	}
}
