using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AE6 RID: 2790
	internal static class InteriorLights
	{
		// Token: 0x06005760 RID: 22368 RVA: 0x0041AE94 File Offset: 0x00419094
		public static ICodingContainer SmoothButtonsLight()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A64", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D03", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 3, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_SmoothTurnOnButtonsLight_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x0041AF2C File Offset: 0x0041912C
		public static ICodingContainer MQB_09_AmbientLight30Colors_Alt_DashMMI()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list1-10", "", "0A28", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					127, 0, 0, 187, 3, 33, 243, 0, 40, 254,
					30, 30, 254, 64, 0, 254, 101, 30, 254, 131,
					30, 254, 161, 30, 254, 189, 30, 254, 254, 30
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list2-11-30", "", "0586", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				byte[] array4 = new byte[]
				{
					190, byte.MaxValue, 30, 100, byte.MaxValue, 30, 30, byte.MaxValue, 30, 15,
					228, 15, 0, 204, 0, 32, 178, 170, 35, 205,
					195, 15, 216, 224, 0, byte.MaxValue, byte.MaxValue, 1, 192, byte.MaxValue,
					30, 160, byte.MaxValue, 30, 130, byte.MaxValue, 30, 100, byte.MaxValue, 30,
					30, byte.MaxValue, 100, 30, byte.MaxValue, 130, 30, byte.MaxValue, 175, 203,
					byte.MaxValue, 175, 226, 243, 217, 221, 235, 3, 0, 0
				};
				Array.Copy(array4, 0, array3, 0, array4.Length);
				return array3;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			return new MQBMultipleCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_AmbientLightColorSelection30ColorsAlternativeColorSetDashboardAndMultimediaSystem_Name"), "", true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = "Based on: www.drive2.com/l/554598613926281295",
				RequiresPro = false
			};
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x0041B020 File Offset: 0x00419220
		public static ICodingContainer MQB_09_AmbientLight30Colors_Alt_Doors()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list1-10-lin", "", "0B3F", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					58, 244, 4, 29, 254, 153, 254, 9, 2, 201,
					30, 3, 224, 52, 5, 174, 73, 6, 215, 136,
					3, 254, 200, 0, 254, 230, 0, 254, byte.MaxValue, 25
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list2-11-30-lin", "", "0587", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				byte[] array4 = new byte[]
				{
					191, 254, 24, 100, 254, 20, 30, 247, 10, 254,
					9, 2, 58, 245, 4, 32, 178, 170, 254, 9,
					2, 15, 215, 224, 0, 254, 254, 29, 254, 153,
					19, 204, 185, 58, 246, 4, 0, 110, 253, 0,
					40, 253, 70, 120, 205, 106, 141, 182, 175, 200,
					209, 175, 237, 179, 120, 230, 71, 1, 110, 253
				};
				Array.Copy(array4, 0, array3, 0, array4.Length);
				return array3;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			return new MQBMultipleCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_AmbientLightColorSelection30ColorsAlternativeColorSetDoorsLights_Name"), "", true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				RequiresPro = false,
				InnerDescription = "Based on: www.drive2.com/l/554598613926281295"
			};
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x0041B114 File Offset: 0x00419314
		public static ICodingContainer MQB_09_AmbientLight30ColorsVar2_DashMMI()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list1-10", "", "0A28", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					155, 46, byte.MaxValue, 116, 32, byte.MaxValue, 0, 5, byte.MaxValue, 14,
					105, byte.MaxValue, 0, 129, byte.MaxValue, 0, 145, byte.MaxValue, 0, 170,
					byte.MaxValue, 0, 193, byte.MaxValue, 0, 219, byte.MaxValue, 0, byte.MaxValue, 253
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list2-11-30", "", "0586", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				byte[] array4 = new byte[]
				{
					0, byte.MaxValue, 225, 0, byte.MaxValue, 201, 0, byte.MaxValue, 144, 0,
					byte.MaxValue, 0, 93, byte.MaxValue, 0, 150, byte.MaxValue, 0, 206, byte.MaxValue,
					0, 232, 235, 0, byte.MaxValue, 239, 0, byte.MaxValue, 233, 0,
					byte.MaxValue, 200, 0, byte.MaxValue, 180, 0, byte.MaxValue, 161, 0, byte.MaxValue,
					142, 0, byte.MaxValue, 116, 15, byte.MaxValue, 90, 18, byte.MaxValue, 10,
					0, byte.MaxValue, 19, 82, 253, byte.MaxValue, 238, byte.MaxValue, byte.MaxValue, byte.MaxValue
				};
				Array.Copy(array4, 0, array3, 0, array4.Length);
				return array3;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			return new MQBMultipleCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_AmbientLightColorSelection30ColorsDashboardAndMultimediaSystem_Name"), "", true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005764 RID: 22372 RVA: 0x0041B1FC File Offset: 0x004193FC
		public static ICodingContainer MQB_09_AmbientLight30ColorsVar2_Doors()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list1-10-lin", "", "0B3F", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					160, 109, byte.MaxValue, 85, 88, 235, 0, 50, byte.MaxValue, 35,
					190, 245, 45, 235, 224, 161, 245, 183, 15, byte.MaxValue,
					137, 23, byte.MaxValue, 108, 20, byte.MaxValue, 83, 15, byte.MaxValue, 71
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list2-11-30-lin", "", "0587", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				byte[] array4 = new byte[]
				{
					18, byte.MaxValue, 70, 15, byte.MaxValue, 37, 12, byte.MaxValue, 19, 11,
					byte.MaxValue, 10, 45, byte.MaxValue, 0, 72, byte.MaxValue, 0, 120, byte.MaxValue,
					0, 151, byte.MaxValue, 0, 174, byte.MaxValue, 0, 192, byte.MaxValue, 0,
					225, byte.MaxValue, 0, byte.MaxValue, 236, 1, byte.MaxValue, 191, 6, byte.MaxValue,
					156, 9, 250, 133, 9, byte.MaxValue, 90, 9, byte.MaxValue, 0,
					0, 245, 35, 14, 150, 235, 36, 120, byte.MaxValue, 70
				};
				Array.Copy(array4, 0, array3, 0, array4.Length);
				return array3;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			return new MQBMultipleCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_AmbientLightColorSelection30ColorsDoorsLights_Name"), "", true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x0041B2E4 File Offset: 0x004194E4
		public static ICodingContainer MQB_09_AmbientLight10Colors_DashboardAndMMI()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list1-10", "", "0A28", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					217, 221, 235, byte.MaxValue, 172, 5, 253, 108, 55, 242,
					0, 40, 252, 116, 240, 132, 76, 222, 0, 102,
					byte.MaxValue, 1, 192, byte.MaxValue, 0, 204, 0, 182, byte.MaxValue, 57
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list2-11-30", "", "0586", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				byte[] array4 = new byte[60];
				for (int i = 0; i < array4.Length; i++)
				{
					array4[i] = 0;
				}
				Array.Copy(array4, 0, array3, 0, array4.Length);
				return array3;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			return new MQBMultipleCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_AmbientLightColorSelection10DefaultColorsDashboardAndMultimedia_Name"), "", true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x0041B3C8 File Offset: 0x004195C8
		public static ICodingContainer MQB_09_AmbientLight10Colors_Doors()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list1-10-lin", "", "0B3F", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					120, 231, 71, byte.MaxValue, 200, 0, 173, 73, 6, byte.MaxValue,
					7, 2, byte.MaxValue, 134, 106, 106, 140, 182, 0, 110,
					254, 29, byte.MaxValue, 153, 58, 245, 4, 57, 132, 0
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.InteriorLights, "list2-11-30-lin", "", "0587", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				byte[] array4 = new byte[60];
				for (int i = 0; i < array4.Length; i++)
				{
					array4[i] = 0;
				}
				Array.Copy(array4, 0, array3, 0, array4.Length);
				return array3;
			}, null, new MQBAdaptationOption[] { MQBAdaptationTemplate.EnableOption });
			return new MQBMultipleCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_AmbientLightColorSelection10DefaultColorsDoorsLights_Name"), "", true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x0041B4AC File Offset: 0x004196AC
		public static ICodingContainer AmbientLightColorsList1()
		{
			return new MQBAdaptationTemplate(0, Translate.GetString("codingDB_AmbientColorsSetupDashboardAndMmiColors110_Name"), Translate.GetString("codingDB_AmbientColorsSetupDashboardAndMmiColors110_Description"), "31347", "", new TranslationItem[0], AdaptationValueTypes.MQBColorList, "0A28", "70E", "778", 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[0])
			{
				Group = CodingGroup.InteriorLights
			};
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x0041B518 File Offset: 0x00419718
		public static ICodingContainer AmbientLightColorsList1LIN()
		{
			return new MQBAdaptationTemplate(0, Translate.GetString("codingDB_AmbientColorsSetupDoorsColors110_Name"), Translate.GetString("codingDB_AmbientColorsSetupDashboardAndMmiColors110_Description"), "31347", "", new TranslationItem[0], AdaptationValueTypes.MQBColorList, "0B3F", "70E", "778", 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[0])
			{
				Group = CodingGroup.InteriorLights
			};
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x0041B584 File Offset: 0x00419784
		public static ICodingContainer AmbientLightColorsList2()
		{
			return new MQBAdaptationTemplate(0, Translate.GetString("codingDB_AmbientColorsSetupDashboardAndMmiColors1130_Name"), Translate.GetString("codingDB_AmbientColorsSetupDashboardAndMmiColors110_Description"), "31347", "", new TranslationItem[0], AdaptationValueTypes.MQBColorList, "0586", "70E", "778", 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[0])
			{
				Group = CodingGroup.InteriorLights
			};
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x0041B5F0 File Offset: 0x004197F0
		public static ICodingContainer AmbientLightColorsList2LIN()
		{
			return new MQBAdaptationTemplate(0, Translate.GetString("codingDB_AmbientColorsSetupDoorsColors1130_Name"), Translate.GetString("codingDB_AmbientColorsSetupDashboardAndMmiColors110_Description"), "31347", "", new TranslationItem[0], AdaptationValueTypes.MQBColorList, "0587", "70E", "778", 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[0])
			{
				Group = CodingGroup.InteriorLights
			};
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x0041B65C File Offset: 0x0041985C
		public static ICodingContainer StartButtonHeartBit()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0574", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("1304", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 4, true);
					BitHelpers.SwitchBitInByte(array2, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 4, false);
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_StartEngineButtonHeartBeatPulsatingLight_Name"), "", VagUnitHelper.GetRequestHeaderForMQBUnit("B7"), VagUnitHelper.GetResponseHeaderForMQBUnit("B7"), "20103", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x0041B6FC File Offset: 0x004198FC
		public static ICodingContainer DontTurnOnInteriorLightsWhenTailgateOpened()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "50C4", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 13, 5, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D03", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 1, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_TurnOnInteriorLightsWhenTailgateOpened_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x0041B79C File Offset: 0x0041999C
		public static ICodingContainer MQB_AmbientColorWithDriveModeSelectionVar1()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0B3C", "", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[1] = 5;
				}
				else
				{
					array[1] = 0;
				}
				return array;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0B1B", "", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 7, true);
					BitHelpers.SwitchBitInByte(array2, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 7, false);
					BitHelpers.SwitchBitInByte(array2, 1, 5, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("50C4", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 1, 3, true);
					BitHelpers.SwitchBitInByte(array3, 14, 7, false);
					BitHelpers.SwitchBitInByte(array3, 14, 6, true);
					BitHelpers.SwitchBitInByte(array3, 14, 5, true);
					BitHelpers.SwitchBitInByte(array3, 14, 4, true);
					BitHelpers.SwitchBitInByte(array3, 14, 3, true);
					BitHelpers.SwitchBitInByte(array3, 15, 0, false);
					BitHelpers.SwitchBitInByte(array3, 15, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 1, 3, false);
					BitHelpers.SwitchBitInByte(array3, 14, 7, false);
					BitHelpers.SwitchBitInByte(array3, 14, 6, false);
					BitHelpers.SwitchBitInByte(array3, 14, 5, false);
					BitHelpers.SwitchBitInByte(array3, 14, 4, false);
					BitHelpers.SwitchBitInByte(array3, 14, 3, false);
					BitHelpers.SwitchBitInByte(array3, 15, 0, false);
					BitHelpers.SwitchBitInByte(array3, 15, 1, false);
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("50C5", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 1, 5, true);
					BitHelpers.SwitchBitInByte(array4, 1, 3, true);
					BitHelpers.SwitchBitInByte(array4, 1, 4, true);
					array4[2] = 1;
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 1, 5, false);
					BitHelpers.SwitchBitInByte(array4, 1, 3, false);
					BitHelpers.SwitchBitInByte(array4, 1, 4, false);
					array4[2] = 0;
				}
				return array4;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0600", "19", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
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
			mqbeasyCodingItem3.PostWriteCommands = "1102";
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0588", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (data.Take(16).All((byte x) => x == 1 || x == 0))
					{
						array6[0] = 1;
						array6[1] = 7;
						array6[10] = 8;
						array6[11] = 1;
						array6[12] = 1;
						array6[13] = 1;
						array6[14] = 1;
						array6[15] = 1;
						array6[2] = 1;
						array6[3] = 4;
						array6[4] = 3;
						array6[5] = 9;
						array6[6] = 1;
						array6[7] = 1;
						array6[8] = 1;
						array6[9] = 1;
					}
				}
				return array6;
			}, (byte[] data, MQBEasyCodingItem coding2) => MQBAdaptationTemplate.EnableOption.Title);
			MQBEasyCodingItem mqbeasyCodingItem5 = new MQBEasyCodingItem("0A28", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (data.All((byte x) => x == 0))
					{
						byte[] array8 = new byte[]
						{
							217, 221, 235, byte.MaxValue, 172, 5, 253, 108, 55, 242,
							0, 40, 252, 116, 240, 132, 76, 222, 0, 102,
							byte.MaxValue, 1, 192, byte.MaxValue, 0, 204, 0, 182, byte.MaxValue, 57
						};
						Array.Copy(array8, 0, array7, 0, array8.Length);
					}
				}
				return array7;
			}, (byte[] data, MQBEasyCodingItem coding2) => MQBAdaptationTemplate.EnableOption.Title);
			return new MQBMultipleCoding(CodingGroup.InteriorLights, string.Format(Translate.GetString("codingDB_AmbientColorSelectionForMultimediaSystemAndDashboardColorsChangingWithDriveModeSelectionVar1_Name"), "1"), Translate.GetString("codingDB_AmbientColorSelectionForMultimediaSystemAndDashboardColorsChangingWithDriveModeSelectionVar1_Description"), false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem5 })
			{
				InnerDescription = Translate.GetString("codingDB_AmbientColorSelectionForMultimediaSystemAndDashboardColorsChangingWithDriveModeSelectionVar1_InnerDescription")
			};
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x0041B9E8 File Offset: 0x00419BE8
		public static ICodingContainer MQB_AmbientColorWithDriveModeSelectionVar2()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0B3C", "", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[1] = 5;
				}
				else
				{
					array[1] = 0;
				}
				return array;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0B1B", "", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 7, true);
					BitHelpers.SwitchBitInByte(array2, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 7, false);
					BitHelpers.SwitchBitInByte(array2, 1, 5, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("50C4", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 1, 3, true);
					BitHelpers.SwitchBitInByte(array3, 14, 7, false);
					BitHelpers.SwitchBitInByte(array3, 14, 6, true);
					BitHelpers.SwitchBitInByte(array3, 14, 5, true);
					BitHelpers.SwitchBitInByte(array3, 14, 4, true);
					BitHelpers.SwitchBitInByte(array3, 14, 3, true);
					BitHelpers.SwitchBitInByte(array3, 15, 0, false);
					BitHelpers.SwitchBitInByte(array3, 15, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 1, 3, false);
					BitHelpers.SwitchBitInByte(array3, 14, 7, false);
					BitHelpers.SwitchBitInByte(array3, 14, 6, false);
					BitHelpers.SwitchBitInByte(array3, 14, 5, false);
					BitHelpers.SwitchBitInByte(array3, 14, 4, false);
					BitHelpers.SwitchBitInByte(array3, 14, 3, false);
					BitHelpers.SwitchBitInByte(array3, 15, 0, false);
					BitHelpers.SwitchBitInByte(array3, 15, 1, false);
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("50C5", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 1, 5, true);
					BitHelpers.SwitchBitInByte(array4, 1, 3, true);
					BitHelpers.SwitchBitInByte(array4, 1, 4, true);
					array4[2] = 0;
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 1, 5, false);
					BitHelpers.SwitchBitInByte(array4, 1, 3, false);
					BitHelpers.SwitchBitInByte(array4, 1, 4, false);
					array4[2] = 0;
				}
				return array4;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0600", "19", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
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
			mqbeasyCodingItem3.PostWriteCommands = "1102";
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0588", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (data.Take(16).All((byte x) => x == 1 || x == 0))
					{
						array6[0] = 1;
						array6[1] = 7;
						array6[10] = 8;
						array6[11] = 1;
						array6[12] = 1;
						array6[13] = 1;
						array6[14] = 1;
						array6[15] = 1;
						array6[2] = 1;
						array6[3] = 4;
						array6[4] = 3;
						array6[5] = 9;
						array6[6] = 1;
						array6[7] = 1;
						array6[8] = 1;
						array6[9] = 1;
					}
				}
				return array6;
			}, (byte[] data, MQBEasyCodingItem coding2) => MQBAdaptationTemplate.EnableOption.Title);
			MQBEasyCodingItem mqbeasyCodingItem5 = new MQBEasyCodingItem("0A28", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array7 = new byte[data.Length];
				Array.Copy(data, 0, array7, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (data.All((byte x) => x == 0))
					{
						byte[] array8 = new byte[]
						{
							217, 221, 235, byte.MaxValue, 172, 5, 253, 108, 55, 242,
							0, 40, 252, 116, 240, 132, 76, 222, 0, 102,
							byte.MaxValue, 1, 192, byte.MaxValue, 0, 204, 0, 182, byte.MaxValue, 57
						};
						Array.Copy(array8, 0, array7, 0, array8.Length);
					}
				}
				return array7;
			}, (byte[] data, MQBEasyCodingItem coding2) => MQBAdaptationTemplate.EnableOption.Title);
			return new MQBMultipleCoding(CodingGroup.InteriorLights, string.Format(Translate.GetString("codingDB_AmbientColorSelectionForMultimediaSystemAndDashboardColorsChangingWithDriveModeSelectionVar1_Name"), "2"), Translate.GetString("codingDB_AmbientColorSelectionForMultimediaSystemAndDashboardColorsChangingWithDriveModeSelectionVar1_Description"), false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem5 })
			{
				InnerDescription = Translate.GetString("codingDB_AmbientColorSelectionForMultimediaSystemAndDashboardColorsChangingWithDriveModeSelectionVar1_InnerDescription")
			};
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x0041BC34 File Offset: 0x00419E34
		public static ICodingContainer FootwellLightsInstalled()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, "Footwell light installed", "", "50C5", 0, 7, "0600", 17, 3, Array.Empty<TranslationItem>());
			MQB_LightFunction func_off = MQB_LightFunction.FromValue("00");
			MQB_LightFunction func_footwell = MQB_LightFunction.FromValue("2B");
			MQB_LampType lamptype_4x5w = MQB_LampType.FromValue("14");
			MQB_LampType lamptype_none = MQB_LampType.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "056E", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.LampType = lamptype_4x5w;
					mqb_LightConfiguration.LightControlAB = MQB_LightConfiguration.LightControl.Always;
					mqb_LightConfiguration.DimmwertAB = 100;
					mqb_LightConfiguration.FunctionA = func_footwell;
					mqb_LightConfiguration.FehlerortMittleresByteDTC_DFCC = 2;
				}
				else
				{
					mqb_LightConfiguration.LampType = lamptype_none;
					mqb_LightConfiguration.LightControlAB = MQB_LightConfiguration.LightControl.Always;
					mqb_LightConfiguration.DimmwertAB = 0;
					mqb_LightConfiguration.FunctionA = func_off;
					mqb_LightConfiguration.FehlerortMittleresByteDTC_DFCC = 0;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.InteriorLights, Translate.GetString("codingDB_FootwellLightInstalled_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem });
		}

		// Token: 0x02000AE7 RID: 2791
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005770 RID: 22384 RVA: 0x0041BD1E File Offset: 0x00419F1E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005771 RID: 22385 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005772 RID: 22386 RVA: 0x0041BD2C File Offset: 0x00419F2C
			internal byte[] <SmoothButtonsLight>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
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

			// Token: 0x06005773 RID: 22387 RVA: 0x0041BD78 File Offset: 0x00419F78
			internal byte[] <SmoothButtonsLight>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
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

			// Token: 0x06005774 RID: 22388 RVA: 0x0041BDC4 File Offset: 0x00419FC4
			internal byte[] <MQB_09_AmbientLight30Colors_Alt_DashMMI>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					127, 0, 0, 187, 3, 33, 243, 0, 40, 254,
					30, 30, 254, 64, 0, 254, 101, 30, 254, 131,
					30, 254, 161, 30, 254, 189, 30, 254, 254, 30
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x06005775 RID: 22389 RVA: 0x0041BE08 File Offset: 0x0041A008
			internal byte[] <MQB_09_AmbientLight30Colors_Alt_DashMMI>b__1_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					190, byte.MaxValue, 30, 100, byte.MaxValue, 30, 30, byte.MaxValue, 30, 15,
					228, 15, 0, 204, 0, 32, 178, 170, 35, 205,
					195, 15, 216, 224, 0, byte.MaxValue, byte.MaxValue, 1, 192, byte.MaxValue,
					30, 160, byte.MaxValue, 30, 130, byte.MaxValue, 30, 100, byte.MaxValue, 30,
					30, byte.MaxValue, 100, 30, byte.MaxValue, 130, 30, byte.MaxValue, 175, 203,
					byte.MaxValue, 175, 226, 243, 217, 221, 235, 3, 0, 0
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x06005776 RID: 22390 RVA: 0x0041BE4C File Offset: 0x0041A04C
			internal byte[] <MQB_09_AmbientLight30Colors_Alt_Doors>b__2_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					58, 244, 4, 29, 254, 153, 254, 9, 2, 201,
					30, 3, 224, 52, 5, 174, 73, 6, 215, 136,
					3, 254, 200, 0, 254, 230, 0, 254, byte.MaxValue, 25
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x06005777 RID: 22391 RVA: 0x0041BE90 File Offset: 0x0041A090
			internal byte[] <MQB_09_AmbientLight30Colors_Alt_Doors>b__2_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					191, 254, 24, 100, 254, 20, 30, 247, 10, 254,
					9, 2, 58, 245, 4, 32, 178, 170, 254, 9,
					2, 15, 215, 224, 0, 254, 254, 29, 254, 153,
					19, 204, 185, 58, 246, 4, 0, 110, 253, 0,
					40, 253, 70, 120, 205, 106, 141, 182, 175, 200,
					209, 175, 237, 179, 120, 230, 71, 1, 110, 253
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x06005778 RID: 22392 RVA: 0x0041BED4 File Offset: 0x0041A0D4
			internal byte[] <MQB_09_AmbientLight30ColorsVar2_DashMMI>b__3_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					155, 46, byte.MaxValue, 116, 32, byte.MaxValue, 0, 5, byte.MaxValue, 14,
					105, byte.MaxValue, 0, 129, byte.MaxValue, 0, 145, byte.MaxValue, 0, 170,
					byte.MaxValue, 0, 193, byte.MaxValue, 0, 219, byte.MaxValue, 0, byte.MaxValue, 253
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x06005779 RID: 22393 RVA: 0x0041BF18 File Offset: 0x0041A118
			internal byte[] <MQB_09_AmbientLight30ColorsVar2_DashMMI>b__3_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					0, byte.MaxValue, 225, 0, byte.MaxValue, 201, 0, byte.MaxValue, 144, 0,
					byte.MaxValue, 0, 93, byte.MaxValue, 0, 150, byte.MaxValue, 0, 206, byte.MaxValue,
					0, 232, 235, 0, byte.MaxValue, 239, 0, byte.MaxValue, 233, 0,
					byte.MaxValue, 200, 0, byte.MaxValue, 180, 0, byte.MaxValue, 161, 0, byte.MaxValue,
					142, 0, byte.MaxValue, 116, 15, byte.MaxValue, 90, 18, byte.MaxValue, 10,
					0, byte.MaxValue, 19, 82, 253, byte.MaxValue, 238, byte.MaxValue, byte.MaxValue, byte.MaxValue
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x0600577A RID: 22394 RVA: 0x0041BF5C File Offset: 0x0041A15C
			internal byte[] <MQB_09_AmbientLight30ColorsVar2_Doors>b__4_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					160, 109, byte.MaxValue, 85, 88, 235, 0, 50, byte.MaxValue, 35,
					190, 245, 45, 235, 224, 161, 245, 183, 15, byte.MaxValue,
					137, 23, byte.MaxValue, 108, 20, byte.MaxValue, 83, 15, byte.MaxValue, 71
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x0600577B RID: 22395 RVA: 0x0041BFA0 File Offset: 0x0041A1A0
			internal byte[] <MQB_09_AmbientLight30ColorsVar2_Doors>b__4_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					18, byte.MaxValue, 70, 15, byte.MaxValue, 37, 12, byte.MaxValue, 19, 11,
					byte.MaxValue, 10, 45, byte.MaxValue, 0, 72, byte.MaxValue, 0, 120, byte.MaxValue,
					0, 151, byte.MaxValue, 0, 174, byte.MaxValue, 0, 192, byte.MaxValue, 0,
					225, byte.MaxValue, 0, byte.MaxValue, 236, 1, byte.MaxValue, 191, 6, byte.MaxValue,
					156, 9, 250, 133, 9, byte.MaxValue, 90, 9, byte.MaxValue, 0,
					0, 245, 35, 14, 150, 235, 36, 120, byte.MaxValue, 70
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x0600577C RID: 22396 RVA: 0x0041BFE4 File Offset: 0x0041A1E4
			internal byte[] <MQB_09_AmbientLight10Colors_DashboardAndMMI>b__5_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					217, 221, 235, byte.MaxValue, 172, 5, 253, 108, 55, 242,
					0, 40, 252, 116, 240, 132, 76, 222, 0, 102,
					byte.MaxValue, 1, 192, byte.MaxValue, 0, 204, 0, 182, byte.MaxValue, 57
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x0600577D RID: 22397 RVA: 0x0041C028 File Offset: 0x0041A228
			internal byte[] <MQB_09_AmbientLight10Colors_DashboardAndMMI>b__5_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[60];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = 0;
				}
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x0600577E RID: 22398 RVA: 0x0041C074 File Offset: 0x0041A274
			internal byte[] <MQB_09_AmbientLight10Colors_Doors>b__6_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[]
				{
					120, 231, 71, byte.MaxValue, 200, 0, 173, 73, 6, byte.MaxValue,
					7, 2, byte.MaxValue, 134, 106, 106, 140, 182, 0, 110,
					254, 29, byte.MaxValue, 153, 58, 245, 4, 57, 132, 0
				};
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x0600577F RID: 22399 RVA: 0x0041C0B8 File Offset: 0x0041A2B8
			internal byte[] <MQB_09_AmbientLight10Colors_Doors>b__6_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte[] array2 = new byte[60];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = 0;
				}
				Array.Copy(array2, 0, array, 0, array2.Length);
				return array;
			}

			// Token: 0x06005780 RID: 22400 RVA: 0x0041C104 File Offset: 0x0041A304
			internal byte[] <StartButtonHeartBit>b__11_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}

			// Token: 0x06005781 RID: 22401 RVA: 0x0041C160 File Offset: 0x0041A360
			internal byte[] <StartButtonHeartBit>b__11_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}

			// Token: 0x06005782 RID: 22402 RVA: 0x0041C1BC File Offset: 0x0041A3BC
			internal byte[] <DontTurnOnInteriorLightsWhenTailgateOpened>b__12_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 13, 5, false);
				}
				return array;
			}

			// Token: 0x06005783 RID: 22403 RVA: 0x0041C208 File Offset: 0x0041A408
			internal byte[] <DontTurnOnInteriorLightsWhenTailgateOpened>b__12_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				return array;
			}

			// Token: 0x06005784 RID: 22404 RVA: 0x0041C254 File Offset: 0x0041A454
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_0(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[1] = 5;
				}
				else
				{
					array[1] = 0;
				}
				return array;
			}

			// Token: 0x06005785 RID: 22405 RVA: 0x0041C294 File Offset: 0x0041A494
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_1(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, true);
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				return array;
			}

			// Token: 0x06005786 RID: 22406 RVA: 0x0041C2F0 File Offset: 0x0041A4F0
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_2(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 14, 7, false);
					BitHelpers.SwitchBitInByte(array, 14, 6, true);
					BitHelpers.SwitchBitInByte(array, 14, 5, true);
					BitHelpers.SwitchBitInByte(array, 14, 4, true);
					BitHelpers.SwitchBitInByte(array, 14, 3, true);
					BitHelpers.SwitchBitInByte(array, 15, 0, false);
					BitHelpers.SwitchBitInByte(array, 15, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 14, 7, false);
					BitHelpers.SwitchBitInByte(array, 14, 6, false);
					BitHelpers.SwitchBitInByte(array, 14, 5, false);
					BitHelpers.SwitchBitInByte(array, 14, 4, false);
					BitHelpers.SwitchBitInByte(array, 14, 3, false);
					BitHelpers.SwitchBitInByte(array, 15, 0, false);
					BitHelpers.SwitchBitInByte(array, 15, 1, false);
				}
				return array;
			}

			// Token: 0x06005787 RID: 22407 RVA: 0x0041C3C8 File Offset: 0x0041A5C8
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_3(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
					array[2] = 1;
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
					array[2] = 0;
				}
				return array;
			}

			// Token: 0x06005788 RID: 22408 RVA: 0x0041C440 File Offset: 0x0041A640
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_4(byte[] data, string value, MQBEasyCodingItem coding1)
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

			// Token: 0x06005789 RID: 22409 RVA: 0x0041C48C File Offset: 0x0041A68C
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_5(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (data.Take(16).All((byte x) => x == 1 || x == 0))
					{
						array[0] = 1;
						array[1] = 7;
						array[10] = 8;
						array[11] = 1;
						array[12] = 1;
						array[13] = 1;
						array[14] = 1;
						array[15] = 1;
						array[2] = 1;
						array[3] = 4;
						array[4] = 3;
						array[5] = 9;
						array[6] = 1;
						array[7] = 1;
						array[8] = 1;
						array[9] = 1;
					}
				}
				return array;
			}

			// Token: 0x0600578A RID: 22410 RVA: 0x0041C537 File Offset: 0x0041A737
			internal bool <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_9(byte x)
			{
				return x == 1 || x == 0;
			}

			// Token: 0x0600578B RID: 22411 RVA: 0x004142A9 File Offset: 0x004124A9
			internal string <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_6(byte[] data, MQBEasyCodingItem coding2)
			{
				return MQBAdaptationTemplate.EnableOption.Title;
			}

			// Token: 0x0600578C RID: 22412 RVA: 0x0041C544 File Offset: 0x0041A744
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_7(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (data.All((byte x) => x == 0))
					{
						byte[] array2 = new byte[]
						{
							217, 221, 235, byte.MaxValue, 172, 5, 253, 108, 55, 242,
							0, 40, 252, 116, 240, 132, 76, 222, 0, 102,
							byte.MaxValue, 1, 192, byte.MaxValue, 0, 204, 0, 182, byte.MaxValue, 57
						};
						Array.Copy(array2, 0, array, 0, array2.Length);
					}
				}
				return array;
			}

			// Token: 0x0600578D RID: 22413 RVA: 0x0037EDD4 File Offset: 0x0037CFD4
			internal bool <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_10(byte x)
			{
				return x == 0;
			}

			// Token: 0x0600578E RID: 22414 RVA: 0x004142A9 File Offset: 0x004124A9
			internal string <MQB_AmbientColorWithDriveModeSelectionVar1>b__13_8(byte[] data, MQBEasyCodingItem coding2)
			{
				return MQBAdaptationTemplate.EnableOption.Title;
			}

			// Token: 0x0600578F RID: 22415 RVA: 0x0041C5C0 File Offset: 0x0041A7C0
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_0(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[1] = 5;
				}
				else
				{
					array[1] = 0;
				}
				return array;
			}

			// Token: 0x06005790 RID: 22416 RVA: 0x0041C600 File Offset: 0x0041A800
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_1(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, true);
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				return array;
			}

			// Token: 0x06005791 RID: 22417 RVA: 0x0041C65C File Offset: 0x0041A85C
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_2(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 14, 7, false);
					BitHelpers.SwitchBitInByte(array, 14, 6, true);
					BitHelpers.SwitchBitInByte(array, 14, 5, true);
					BitHelpers.SwitchBitInByte(array, 14, 4, true);
					BitHelpers.SwitchBitInByte(array, 14, 3, true);
					BitHelpers.SwitchBitInByte(array, 15, 0, false);
					BitHelpers.SwitchBitInByte(array, 15, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 14, 7, false);
					BitHelpers.SwitchBitInByte(array, 14, 6, false);
					BitHelpers.SwitchBitInByte(array, 14, 5, false);
					BitHelpers.SwitchBitInByte(array, 14, 4, false);
					BitHelpers.SwitchBitInByte(array, 14, 3, false);
					BitHelpers.SwitchBitInByte(array, 15, 0, false);
					BitHelpers.SwitchBitInByte(array, 15, 1, false);
				}
				return array;
			}

			// Token: 0x06005792 RID: 22418 RVA: 0x0041C734 File Offset: 0x0041A934
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_3(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
					array[2] = 0;
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
					array[2] = 0;
				}
				return array;
			}

			// Token: 0x06005793 RID: 22419 RVA: 0x0041C7AC File Offset: 0x0041A9AC
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_4(byte[] data, string value, MQBEasyCodingItem coding1)
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

			// Token: 0x06005794 RID: 22420 RVA: 0x0041C7F8 File Offset: 0x0041A9F8
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_5(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (data.Take(16).All((byte x) => x == 1 || x == 0))
					{
						array[0] = 1;
						array[1] = 7;
						array[10] = 8;
						array[11] = 1;
						array[12] = 1;
						array[13] = 1;
						array[14] = 1;
						array[15] = 1;
						array[2] = 1;
						array[3] = 4;
						array[4] = 3;
						array[5] = 9;
						array[6] = 1;
						array[7] = 1;
						array[8] = 1;
						array[9] = 1;
					}
				}
				return array;
			}

			// Token: 0x06005795 RID: 22421 RVA: 0x0041C537 File Offset: 0x0041A737
			internal bool <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_9(byte x)
			{
				return x == 1 || x == 0;
			}

			// Token: 0x06005796 RID: 22422 RVA: 0x004142A9 File Offset: 0x004124A9
			internal string <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_6(byte[] data, MQBEasyCodingItem coding2)
			{
				return MQBAdaptationTemplate.EnableOption.Title;
			}

			// Token: 0x06005797 RID: 22423 RVA: 0x0041C8A4 File Offset: 0x0041AAA4
			internal byte[] <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_7(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (data.All((byte x) => x == 0))
					{
						byte[] array2 = new byte[]
						{
							217, 221, 235, byte.MaxValue, 172, 5, 253, 108, 55, 242,
							0, 40, 252, 116, 240, 132, 76, 222, 0, 102,
							byte.MaxValue, 1, 192, byte.MaxValue, 0, 204, 0, 182, byte.MaxValue, 57
						};
						Array.Copy(array2, 0, array, 0, array2.Length);
					}
				}
				return array;
			}

			// Token: 0x06005798 RID: 22424 RVA: 0x0037EDD4 File Offset: 0x0037CFD4
			internal bool <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_10(byte x)
			{
				return x == 0;
			}

			// Token: 0x06005799 RID: 22425 RVA: 0x004142A9 File Offset: 0x004124A9
			internal string <MQB_AmbientColorWithDriveModeSelectionVar2>b__14_8(byte[] data, MQBEasyCodingItem coding2)
			{
				return MQBAdaptationTemplate.EnableOption.Title;
			}

			// Token: 0x040035EB RID: 13803
			public static readonly InteriorLights.<>c <>9 = new InteriorLights.<>c();

			// Token: 0x040035EC RID: 13804
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x040035ED RID: 13805
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x040035EE RID: 13806
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x040035EF RID: 13807
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_1;

			// Token: 0x040035F0 RID: 13808
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x040035F1 RID: 13809
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_1;

			// Token: 0x040035F2 RID: 13810
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_0;

			// Token: 0x040035F3 RID: 13811
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_1;

			// Token: 0x040035F4 RID: 13812
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__4_0;

			// Token: 0x040035F5 RID: 13813
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__4_1;

			// Token: 0x040035F6 RID: 13814
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_0;

			// Token: 0x040035F7 RID: 13815
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_1;

			// Token: 0x040035F8 RID: 13816
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_0;

			// Token: 0x040035F9 RID: 13817
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_1;

			// Token: 0x040035FA RID: 13818
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__11_0;

			// Token: 0x040035FB RID: 13819
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__11_1;

			// Token: 0x040035FC RID: 13820
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_0;

			// Token: 0x040035FD RID: 13821
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_1;

			// Token: 0x040035FE RID: 13822
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__13_0;

			// Token: 0x040035FF RID: 13823
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__13_1;

			// Token: 0x04003600 RID: 13824
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__13_2;

			// Token: 0x04003601 RID: 13825
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__13_3;

			// Token: 0x04003602 RID: 13826
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__13_4;

			// Token: 0x04003603 RID: 13827
			public static Func<byte, bool> <>9__13_9;

			// Token: 0x04003604 RID: 13828
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__13_5;

			// Token: 0x04003605 RID: 13829
			public static Func<byte[], MQBEasyCodingItem, string> <>9__13_6;

			// Token: 0x04003606 RID: 13830
			public static Func<byte, bool> <>9__13_10;

			// Token: 0x04003607 RID: 13831
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__13_7;

			// Token: 0x04003608 RID: 13832
			public static Func<byte[], MQBEasyCodingItem, string> <>9__13_8;

			// Token: 0x04003609 RID: 13833
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__14_0;

			// Token: 0x0400360A RID: 13834
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__14_1;

			// Token: 0x0400360B RID: 13835
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_2;

			// Token: 0x0400360C RID: 13836
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_3;

			// Token: 0x0400360D RID: 13837
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_4;

			// Token: 0x0400360E RID: 13838
			public static Func<byte, bool> <>9__14_9;

			// Token: 0x0400360F RID: 13839
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_5;

			// Token: 0x04003610 RID: 13840
			public static Func<byte[], MQBEasyCodingItem, string> <>9__14_6;

			// Token: 0x04003611 RID: 13841
			public static Func<byte, bool> <>9__14_10;

			// Token: 0x04003612 RID: 13842
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_7;

			// Token: 0x04003613 RID: 13843
			public static Func<byte[], MQBEasyCodingItem, string> <>9__14_8;
		}

		// Token: 0x02000AE8 RID: 2792
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			// Token: 0x0600579A RID: 22426 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_0()
			{
			}

			// Token: 0x0600579B RID: 22427 RVA: 0x0041C920 File Offset: 0x0041AB20
			internal byte[] <FootwellLightsInstalled>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.LampType = this.lamptype_4x5w;
					mqb_LightConfiguration.LightControlAB = MQB_LightConfiguration.LightControl.Always;
					mqb_LightConfiguration.DimmwertAB = 100;
					mqb_LightConfiguration.FunctionA = this.func_footwell;
					mqb_LightConfiguration.FehlerortMittleresByteDTC_DFCC = 2;
				}
				else
				{
					mqb_LightConfiguration.LampType = this.lamptype_none;
					mqb_LightConfiguration.LightControlAB = MQB_LightConfiguration.LightControl.Always;
					mqb_LightConfiguration.DimmwertAB = 0;
					mqb_LightConfiguration.FunctionA = this.func_off;
					mqb_LightConfiguration.FehlerortMittleresByteDTC_DFCC = 0;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003614 RID: 13844
			public MQB_LampType lamptype_4x5w;

			// Token: 0x04003615 RID: 13845
			public MQB_LightFunction func_footwell;

			// Token: 0x04003616 RID: 13846
			public MQB_LampType lamptype_none;

			// Token: 0x04003617 RID: 13847
			public MQB_LightFunction func_off;
		}
	}
}
