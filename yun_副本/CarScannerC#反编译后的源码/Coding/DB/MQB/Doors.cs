using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A9B RID: 2715
	internal static class Doors
	{
		// Token: 0x060055B9 RID: 21945 RVA: 0x0040D7C4 File Offset: 0x0040B9C4
		public static ICodingContainer EnableKeyFobWhileEngineRunning()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A65", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_EnableKeyOpenCloseFunctionsWhileEngineRunning_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x060055BA RID: 21946 RVA: 0x0040D870 File Offset: 0x0040BA70
		public static ICodingContainer EnableEasyOpenPart1()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A68", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(true, "0D0D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 5, 6, true);
					BitHelpers.SwitchBitInByte(array2, 5, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 5, 6, false);
					BitHelpers.SwitchBitInByte(array2, 5, 7, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			string text = "20103";
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0D57", "05", text, "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[0] = 34;
				}
				else
				{
					array3[0] = 254;
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0D57", "05", text, "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[0] = 34;
				}
				else
				{
					array4[0] = 254;
				}
				return array4;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0579", "05", text, "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 0, 0, true);
					BitHelpers.SwitchBitInByte(array5, 0, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 0, 0, false);
					BitHelpers.SwitchBitInByte(array5, 0, 4, false);
				}
				return array5;
			}, null);
			mqbeasyCodingItem3.PostWriteCommands = "2F040503FFFFFFFF;1102";
			return new MQBMultipleCoding(CodingGroup.Doors, Translate.GetString("codingDB_ActivateEasyOpenPart1_Name"), Translate.GetString("codingDB_ActivateEasyOpenPart1_Description"), false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3 })
			{
				InnerDescription = Translate.GetString("codingDB_ActivateEasyOpenPart1_InnerDescription")
			};
		}

		// Token: 0x060055BB RID: 21947 RVA: 0x0040DA08 File Offset: 0x0040BC08
		public static ICodingContainer EnableEasyOpenPart2()
		{
			string text = "20103";
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0D30", "05", text, "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 69;
				}
				else
				{
					array[0] = 5;
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0D3C", "05", text, "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[0] = 9;
				}
				else
				{
					array2[0] = 0;
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0D3D", "05", text, "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[0] = 34;
				}
				else
				{
					array3[0] = 226;
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0D53", "05", text, "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[0] = 34;
				}
				else
				{
					array4[0] = byte.MaxValue;
				}
				return array4;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem5 = new MQBEasyCodingItem("0D54", "05", text, "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array5[0] = 24;
				}
				else
				{
					array5[0] = 108;
				}
				return array5;
			}, null);
			mqbeasyCodingItem5.PostWriteCommands = "2F040503FFFFFFFF";
			return new MQBMultipleCoding(CodingGroup.Doors, Translate.GetString("codingDB_ActivateEasyOpenPart2_Name"), Translate.GetString("codingDB_ActivateEasyOpenPart1_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem5 })
			{
				InnerDescription = Translate.GetString("codingDB_ActivateEasyOpenPart1_InnerDescription")
			};
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x0040DB80 File Offset: 0x0040BD80
		public static ICodingContainer EasyOpenPart3_Kodiaq()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0D4C", "B7", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 0;
					array[1] = 30;
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0D57", "B7", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[0] = 54;
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Doors, Translate.GetString("codingDB_ActivateEasyOpenPart3Kodiaq_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x060055BD RID: 21949 RVA: 0x0040DC28 File Offset: 0x0040BE28
		public static ICodingContainer MQB_DriverElectricWindowTimeAfterIgnitionTurnedOff()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A69", "31347", 1, 1, 5.0, 0.0, false, false);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", "31347", 4, 1, 5.0, 0.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_DriverDoorPowerWindowShutOffDelayAfterIgnitionWasTurnedOffSeconds_Name"), Translate.GetString("codingDB_DriverDoorPowerWindowShutOffDelayAfterIgnitionWasTurnedOffSeconds_Description"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x060055BE RID: 21950 RVA: 0x0040DCC4 File Offset: 0x0040BEC4
		public static MQBEasyCodingItem MQB_05_DisableKessyFrontDoors()
		{
			return new MQBEasyCodingItem(CodingGroup.Doors, Translate.GetString("codingDB_PreventFrontDoorOpeningWithKessy_Name"), Translate.GetString("codingDB_PreventFrontDoorOpeningWithKessy_Description"), "0600", "732", "79C", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x060055BF RID: 21951 RVA: 0x0040DD3C File Offset: 0x0040BF3C
		public static MQBEasyCodingItem MQB_05_DisableKessyAllDoors()
		{
			return new MQBEasyCodingItem(CodingGroup.Doors, Translate.GetString("codingDB_PreventAllDoorOpeningWithKessy_Name"), Translate.GetString("codingDB_PreventFrontDoorOpeningWithKessy_Description"), "0600", "732", "79C", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x060055C0 RID: 21952 RVA: 0x0040DDB4 File Offset: 0x0040BFB4
		public static ICodingContainer MQB_05_AutomaticallyLockCarDoors()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0570", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0600", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 2, 4, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_AutomaticallyLockCarDoorsWhenKeyIsOutsideOfTheCarAndAllDoorsAreClosed_Name"), "", "732", "79C", "20103", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x0040DE54 File Offset: 0x0040C054
		public static ICodingContainer Lock_AutolockRear()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6A", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 7, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_AutolockRearPartOfTheVehicle_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x0040DF00 File Offset: 0x0040C100
		public static ICodingContainer Lock_AutoUnlock()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6A", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 3, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_AutoUnlockDoors_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x0040DFAC File Offset: 0x0040C1AC
		public static ICodingContainer Lock_AutolockAtSpeed()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6A", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_AutoLockDoorsAfterSpeedThresholdWasReached_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060055C4 RID: 21956 RVA: 0x0040E058 File Offset: 0x0040C258
		public static ICodingContainer Lock_AutoUnlockWhenSelectorInParking_NAR()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6A", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 3, 3, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_AutoUnlockDoorsWhenSelectorInParking_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060055C5 RID: 21957 RVA: 0x0040E104 File Offset: 0x0040C304
		public static ICodingContainer Lock_LockMenu()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A65", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 3, 0, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_LockMenuInMmi_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060055C6 RID: 21958 RVA: 0x0040E1B0 File Offset: 0x0040C3B0
		public static ICodingContainer Lock_Unlockmenu()
		{
			MQBAdaptationOption opt_off = MQBAdaptationTemplate.DisableOption;
			MQBAdaptationOption opt_vis = new MQBAdaptationOption(Translate.GetString("codingDB_VisibleOnly_Name"), "01");
			MQBAdaptationOption opt_adj = new MQBAdaptationOption(Translate.GetString("codingDB_Adjustable_Name"), "10");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6A", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				else if (value == opt_vis.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else if (value == opt_adj.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 2, false);
					BitHelpers.SwitchBitInByte(array2, 3, 1, false);
				}
				else if (value == opt_vis.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 2, false);
					BitHelpers.SwitchBitInByte(array2, 3, 1, true);
				}
				else if (value == opt_adj.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 2, true);
					BitHelpers.SwitchBitInByte(array2, 3, 1, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_UnlockMenuInMmi_Name"), "", "70E", "778", "31347", new MQBAdaptationOption[] { opt_off, opt_vis, opt_adj })
			{
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 },
				RequiresPro = false
			};
		}

		// Token: 0x060055C7 RID: 21959 RVA: 0x0040E2A4 File Offset: 0x0040C4A4
		public static ICodingContainer Lock_UnlockDoorsVariants()
		{
			MQBAdaptationOption opt_all = new MQBAdaptationOption(Translate.GetString("codingDB_AllDoors_Name"), "01");
			MQBAdaptationOption opt_driver = new MQBAdaptationOption(Translate.GetString("codingDB_DriverDoorOnly_Name"), "10");
			MQBAdaptationOption opt_one_side = new MQBAdaptationOption(Translate.GetString("codingDB_DoorsOnOneSide_Name"), "10");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A65", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_all.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == opt_driver.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else if (value == opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D08", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt_all.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
					BitHelpers.SwitchBitInByte(array2, 0, 4, false);
				}
				else if (value == opt_driver.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
					BitHelpers.SwitchBitInByte(array2, 0, 4, true);
				}
				else if (value == opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 5, true);
					BitHelpers.SwitchBitInByte(array2, 0, 4, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Doors, Translate.GetString("codingDB_UnlockDoorsOptions_Name"), "", "70E", "778", "31347", new MQBAdaptationOption[] { opt_all, opt_driver, opt_one_side })
			{
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 },
				RequiresPro = false
			};
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x0040E3A8 File Offset: 0x0040C5A8
		public static ICodingContainer RaiseAndLowerWindowsWithKeyFob()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A69", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					BitHelpers.SwitchBitInByte(array2, 0, 6, true);
					BitHelpers.SwitchBitInByte(array2, 0, 5, true);
					BitHelpers.SwitchBitInByte(array2, 3, 5, true);
					BitHelpers.SwitchBitInByte(array2, 3, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 6, false);
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
					BitHelpers.SwitchBitInByte(array2, 3, 5, false);
					BitHelpers.SwitchBitInByte(array2, 3, 4, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit3 = new MQBAlternativeContainerForUnit09(true, "0A9A", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 0, 3, true);
					BitHelpers.SwitchBitInByte(array3, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 0, 3, false);
					BitHelpers.SwitchBitInByte(array3, 0, 2, false);
				}
				return array3;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit4 = new MQBAlternativeContainerForUnit09(false, "0D0D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 2, 5, true);
					BitHelpers.SwitchBitInByte(array4, 2, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 2, 5, false);
					BitHelpers.SwitchBitInByte(array4, 2, 4, false);
				}
				return array4;
			}, null);
			new MQBAlternativeCoding("70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit3, mqbalternativeContainerForUnit4 });
			return new MQBMultipleCoding(CodingGroup.Doors, Translate.GetString("codingDB_OpenAndCloseWindowsHoldingKeyFobButtons_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding });
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x0040E4E4 File Offset: 0x0040C6E4
		public static ICodingContainer EasyOpen_Unit09_Part()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A68", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(true, "0D0D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 5, 6, true);
					BitHelpers.SwitchBitInByte(array2, 5, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 5, 6, false);
					BitHelpers.SwitchBitInByte(array2, 5, 7, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Name = "Easy open: Unit 09: Virtual pedal",
				InnerDescription = "Verdecksteuergeraet:\r\n- Virtuelles_Pedal_HMI_einstellba\r\n- Virtuelles_Pedal_Verbau\r\n- Virtuelles_Pedal",
				Group = CodingGroup.Boot
			};
		}

		// Token: 0x02000A9C RID: 2716
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060055CA RID: 21962 RVA: 0x0040E59C File Offset: 0x0040C79C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060055CB RID: 21963 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060055CC RID: 21964 RVA: 0x0040E5A8 File Offset: 0x0040C7A8
			internal byte[] <EnableKeyFobWhileEngineRunning>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				return array;
			}

			// Token: 0x060055CD RID: 21965 RVA: 0x0040E5F4 File Offset: 0x0040C7F4
			internal byte[] <EnableKeyFobWhileEngineRunning>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x060055CE RID: 21966 RVA: 0x0040E640 File Offset: 0x0040C840
			internal byte[] <EnableEasyOpenPart1>b__1_0(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
				}
				return array;
			}

			// Token: 0x060055CF RID: 21967 RVA: 0x0040E6C0 File Offset: 0x0040C8C0
			internal byte[] <EnableEasyOpenPart1>b__1_1(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 6, true);
					BitHelpers.SwitchBitInByte(array, 5, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 6, false);
					BitHelpers.SwitchBitInByte(array, 5, 7, false);
				}
				return array;
			}

			// Token: 0x060055D0 RID: 21968 RVA: 0x0040E71C File Offset: 0x0040C91C
			internal byte[] <EnableEasyOpenPart1>b__1_2(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 34;
				}
				else
				{
					array[0] = 254;
				}
				return array;
			}

			// Token: 0x060055D1 RID: 21969 RVA: 0x0040E760 File Offset: 0x0040C960
			internal byte[] <EnableEasyOpenPart1>b__1_3(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 34;
				}
				else
				{
					array[0] = 254;
				}
				return array;
			}

			// Token: 0x060055D2 RID: 21970 RVA: 0x0040E7A4 File Offset: 0x0040C9A4
			internal byte[] <EnableEasyOpenPart1>b__1_4(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				return array;
			}

			// Token: 0x060055D3 RID: 21971 RVA: 0x0040E800 File Offset: 0x0040CA00
			internal byte[] <EnableEasyOpenPart2>b__2_0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 69;
				}
				else
				{
					array[0] = 5;
				}
				return array;
			}

			// Token: 0x060055D4 RID: 21972 RVA: 0x0040E840 File Offset: 0x0040CA40
			internal byte[] <EnableEasyOpenPart2>b__2_1(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 9;
				}
				else
				{
					array[0] = 0;
				}
				return array;
			}

			// Token: 0x060055D5 RID: 21973 RVA: 0x0040E880 File Offset: 0x0040CA80
			internal byte[] <EnableEasyOpenPart2>b__2_2(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 34;
				}
				else
				{
					array[0] = 226;
				}
				return array;
			}

			// Token: 0x060055D6 RID: 21974 RVA: 0x0040E8C4 File Offset: 0x0040CAC4
			internal byte[] <EnableEasyOpenPart2>b__2_3(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 34;
				}
				else
				{
					array[0] = byte.MaxValue;
				}
				return array;
			}

			// Token: 0x060055D7 RID: 21975 RVA: 0x0040E908 File Offset: 0x0040CB08
			internal byte[] <EnableEasyOpenPart2>b__2_4(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 24;
				}
				else
				{
					array[0] = 108;
				}
				return array;
			}

			// Token: 0x060055D8 RID: 21976 RVA: 0x0040E94C File Offset: 0x0040CB4C
			internal byte[] <EasyOpenPart3_Kodiaq>b__3_0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 0;
					array[1] = 30;
				}
				return array;
			}

			// Token: 0x060055D9 RID: 21977 RVA: 0x0040E98C File Offset: 0x0040CB8C
			internal byte[] <EasyOpenPart3_Kodiaq>b__3_1(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 54;
				}
				return array;
			}

			// Token: 0x060055DA RID: 21978 RVA: 0x0040E9C8 File Offset: 0x0040CBC8
			internal byte[] <MQB_05_DisableKessyFrontDoors>b__5_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				return array;
			}

			// Token: 0x060055DB RID: 21979 RVA: 0x0040EA24 File Offset: 0x0040CC24
			internal byte[] <MQB_05_DisableKessyAllDoors>b__6_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
				}
				return array;
			}

			// Token: 0x060055DC RID: 21980 RVA: 0x0040EAA4 File Offset: 0x0040CCA4
			internal byte[] <MQB_05_AutomaticallyLockCarDoors>b__7_0(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x060055DD RID: 21981 RVA: 0x0040EAF0 File Offset: 0x0040CCF0
			internal byte[] <MQB_05_AutomaticallyLockCarDoors>b__7_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}

			// Token: 0x060055DE RID: 21982 RVA: 0x0040EB3C File Offset: 0x0040CD3C
			internal byte[] <Lock_AutolockRear>b__8_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				return array;
			}

			// Token: 0x060055DF RID: 21983 RVA: 0x0040EB88 File Offset: 0x0040CD88
			internal byte[] <Lock_AutolockRear>b__8_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
				}
				return array;
			}

			// Token: 0x060055E0 RID: 21984 RVA: 0x0040EBD4 File Offset: 0x0040CDD4
			internal byte[] <Lock_AutoUnlock>b__9_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
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

			// Token: 0x060055E1 RID: 21985 RVA: 0x0040EC20 File Offset: 0x0040CE20
			internal byte[] <Lock_AutoUnlock>b__9_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				return array;
			}

			// Token: 0x060055E2 RID: 21986 RVA: 0x0040EC6C File Offset: 0x0040CE6C
			internal byte[] <Lock_AutolockAtSpeed>b__10_0(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x060055E3 RID: 21987 RVA: 0x0040ECB8 File Offset: 0x0040CEB8
			internal byte[] <Lock_AutolockAtSpeed>b__10_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x060055E4 RID: 21988 RVA: 0x0040ED04 File Offset: 0x0040CF04
			internal byte[] <Lock_AutoUnlockWhenSelectorInParking_NAR>b__11_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x060055E5 RID: 21989 RVA: 0x0040ED50 File Offset: 0x0040CF50
			internal byte[] <Lock_AutoUnlockWhenSelectorInParking_NAR>b__11_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 3, false);
				}
				return array;
			}

			// Token: 0x060055E6 RID: 21990 RVA: 0x0040ED9C File Offset: 0x0040CF9C
			internal byte[] <Lock_LockMenu>b__12_0(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x060055E7 RID: 21991 RVA: 0x0040EDE8 File Offset: 0x0040CFE8
			internal byte[] <Lock_LockMenu>b__12_1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x060055E8 RID: 21992 RVA: 0x0040EE34 File Offset: 0x0040D034
			internal byte[] <RaiseAndLowerWindowsWithKeyFob>b__15_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}

			// Token: 0x060055E9 RID: 21993 RVA: 0x0040EEC8 File Offset: 0x0040D0C8
			internal byte[] <RaiseAndLowerWindowsWithKeyFob>b__15_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 3, 5, true);
					BitHelpers.SwitchBitInByte(array, 3, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 3, 5, false);
					BitHelpers.SwitchBitInByte(array, 3, 4, false);
				}
				return array;
			}

			// Token: 0x060055EA RID: 21994 RVA: 0x0040EF5C File Offset: 0x0040D15C
			internal byte[] <RaiseAndLowerWindowsWithKeyFob>b__15_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x060055EB RID: 21995 RVA: 0x0040EFB8 File Offset: 0x0040D1B8
			internal byte[] <RaiseAndLowerWindowsWithKeyFob>b__15_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}

			// Token: 0x060055EC RID: 21996 RVA: 0x0040F014 File Offset: 0x0040D214
			internal byte[] <EasyOpen_Unit09_Part>b__16_0(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
				}
				return array;
			}

			// Token: 0x060055ED RID: 21997 RVA: 0x0040F094 File Offset: 0x0040D294
			internal byte[] <EasyOpen_Unit09_Part>b__16_1(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 6, true);
					BitHelpers.SwitchBitInByte(array, 5, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 6, false);
					BitHelpers.SwitchBitInByte(array, 5, 7, false);
				}
				return array;
			}

			// Token: 0x040034A5 RID: 13477
			public static readonly Doors.<>c <>9 = new Doors.<>c();

			// Token: 0x040034A6 RID: 13478
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x040034A7 RID: 13479
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x040034A8 RID: 13480
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_0;

			// Token: 0x040034A9 RID: 13481
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_1;

			// Token: 0x040034AA RID: 13482
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_2;

			// Token: 0x040034AB RID: 13483
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_3;

			// Token: 0x040034AC RID: 13484
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_4;

			// Token: 0x040034AD RID: 13485
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x040034AE RID: 13486
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_1;

			// Token: 0x040034AF RID: 13487
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_2;

			// Token: 0x040034B0 RID: 13488
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_3;

			// Token: 0x040034B1 RID: 13489
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_4;

			// Token: 0x040034B2 RID: 13490
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_0;

			// Token: 0x040034B3 RID: 13491
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_1;

			// Token: 0x040034B4 RID: 13492
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_0;

			// Token: 0x040034B5 RID: 13493
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_0;

			// Token: 0x040034B6 RID: 13494
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__7_0;

			// Token: 0x040034B7 RID: 13495
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__7_1;

			// Token: 0x040034B8 RID: 13496
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__8_0;

			// Token: 0x040034B9 RID: 13497
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__8_1;

			// Token: 0x040034BA RID: 13498
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_0;

			// Token: 0x040034BB RID: 13499
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_1;

			// Token: 0x040034BC RID: 13500
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_0;

			// Token: 0x040034BD RID: 13501
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_1;

			// Token: 0x040034BE RID: 13502
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__11_0;

			// Token: 0x040034BF RID: 13503
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__11_1;

			// Token: 0x040034C0 RID: 13504
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_0;

			// Token: 0x040034C1 RID: 13505
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_1;

			// Token: 0x040034C2 RID: 13506
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_0;

			// Token: 0x040034C3 RID: 13507
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_1;

			// Token: 0x040034C4 RID: 13508
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_2;

			// Token: 0x040034C5 RID: 13509
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_3;

			// Token: 0x040034C6 RID: 13510
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__16_0;

			// Token: 0x040034C7 RID: 13511
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__16_1;
		}

		// Token: 0x02000A9D RID: 2717
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x060055EE RID: 21998 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x060055EF RID: 21999 RVA: 0x0040F0F0 File Offset: 0x0040D2F0
			internal byte[] <Lock_Unlockmenu>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				else if (value == this.opt_vis.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else if (value == this.opt_adj.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}

			// Token: 0x060055F0 RID: 22000 RVA: 0x0040F188 File Offset: 0x0040D388
			internal byte[] <Lock_Unlockmenu>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 2, false);
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				else if (value == this.opt_vis.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 2, false);
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
				}
				else if (value == this.opt_adj.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 2, true);
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				return array;
			}

			// Token: 0x040034C8 RID: 13512
			public MQBAdaptationOption opt_off;

			// Token: 0x040034C9 RID: 13513
			public MQBAdaptationOption opt_vis;

			// Token: 0x040034CA RID: 13514
			public MQBAdaptationOption opt_adj;
		}

		// Token: 0x02000A9E RID: 2718
		[CompilerGenerated]
		private sealed class <>c__DisplayClass14_0
		{
			// Token: 0x060055F1 RID: 22001 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass14_0()
			{
			}

			// Token: 0x060055F2 RID: 22002 RVA: 0x0040F220 File Offset: 0x0040D420
			internal byte[] <Lock_UnlockDoorsVariants>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_all.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == this.opt_driver.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else if (value == this.opt_one_side.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}

			// Token: 0x060055F3 RID: 22003 RVA: 0x0040F2B8 File Offset: 0x0040D4B8
			internal byte[] <Lock_UnlockDoorsVariants>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x040034CB RID: 13515
			public MQBAdaptationOption opt_all;

			// Token: 0x040034CC RID: 13516
			public MQBAdaptationOption opt_driver;

			// Token: 0x040034CD RID: 13517
			public MQBAdaptationOption opt_one_side;
		}
	}
}
