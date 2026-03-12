using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B76 RID: 2934
	internal static class Washer
	{
		// Token: 0x06005A12 RID: 23058 RVA: 0x0042FDC4 File Offset: 0x0042DFC4
		public static ICodingContainer ReduceWasherDelay()
		{
			return new MQBEasyCodingItem(CodingGroup.Washer, Translate.GetString("codingDB_ReduceWasherDelay_Name"), "", "0A72", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
					BitHelpers.SwitchBitInByte(array, 2, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
				}
				else
				{
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
					BitHelpers.SwitchBitInByte(array, 2, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x0042FE38 File Offset: 0x0042E038
		public static ICodingContainer DropsTear_FrontWiper()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
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
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Washer, "", "", "6001", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 2, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 2, 5, false);
				}
				return array3;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.Washer, Translate.GetString("codingDB_TeardropModeAdditionalWipersPassOnTheWindshield_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem });
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x0042FF48 File Offset: 0x0042E148
		public static ICodingContainer DropsTear_RearWiper()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D20", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
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
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D20", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
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
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Washer, "", "", "6001", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 0, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 0, 3, false);
				}
				return array3;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.Washer, Translate.GetString("codingDB_TeardropModeAdditionalWipersPassOnTheRearGlass_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem });
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x00430058 File Offset: 0x0042E258
		public static ICodingContainer MQB_09_ComfortRearWiper()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D20", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D20", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
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
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_RearWiperComfortFunction_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x004300F8 File Offset: 0x0042E2F8
		public static ICodingContainer MQB_09_AutoRearWiper()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D20", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D20", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_RearWiperAutomaticFunction_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x00430198 File Offset: 0x0042E398
		public static ICodingContainer ParkWipersAfterIgnitionTurnedOff()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 6, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 6, 3, true);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_ParkWindshieldWipersAfterIgnitionTurnedOff_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x00430230 File Offset: 0x0042E430
		public static ICodingContainer MQB_HeadlightWasherInterval()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A73", "31347", 1, 1, 1.0, 0.0, false, false);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", "31347", 0, 1, 1.0, 0.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_HeadlightWasherInterval_Name"), Translate.GetString("codingDB_HeadlightWasherInterval_Description"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x004302CC File Offset: 0x0042E4CC
		public static ICodingContainer MQB_DelayBeforeHeadlightWasher()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A73", "31347", 2, 1, 10.0, 0.0, false, false);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", "31347", 1, 1, 10.0, 0.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_TurnOnHeadlightWasherAfterHoldingWasherLeverForXxMs_Name"), Translate.GetString("codingDB_MinimumStep10Ms_Name"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x00430368 File Offset: 0x0042E568
		public static ICodingContainer MQB_HeadlightWasherDutyDuration()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A73", "31347", 3, 1, 10.0, 0.0, false, false);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", "31347", 2, 1, 10.0, 0.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_HeadlightWasherDutyDuration_Name"), Translate.GetString("codingDB_MinimumStep10Ms_Name"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x00430404 File Offset: 0x0042E604
		public static ICodingContainer MQB_HeadlightWasher_DelayBetweenFirstAndSecondWash()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A73", "31347", 4, 1, 10.0, 0.0, false, false);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", "31347", 3, 1, 10.0, 0.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_MQB_HeadlightWasher_DelayBetweenFirstAndSecondWash"), Translate.GetString("codingDB_MinimumStep10Ms_Name"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x06005A1C RID: 23068 RVA: 0x004304A0 File Offset: 0x0042E6A0
		public static ICodingContainer MQB_HeadlightWasher_SecondWashDuration()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A73", "31347", 5, 1, 10.0, 0.0, false, false);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D05", "31347", 4, 1, 10.0, 0.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_MQB_HeadlightWasher_SecondWashDuration"), Translate.GetString("codingDB_MinimumStep10Ms_Name"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x06005A1D RID: 23069 RVA: 0x0043053C File Offset: 0x0042E73C
		public static ICodingContainer MQB_HeadlightWasher_UseWasherWithLowLevel()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.Washer, Translate.GetString("codingDB_MQB_HeadlightWasher_DisableWasherWithLowLevel"), Translate.GetString("codingDB_MinimumStep10Ms_Name"), "0A73", 0, 1, "0D05", 6, 0, Array.Empty<TranslationItem>());
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x00430578 File Offset: 0x0042E778
		public static ICodingContainer MQB_StopWiperWhenHoodOpened()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_StopWipersWhenOpeningHood_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x00430618 File Offset: 0x0042E818
		public static ICodingContainer MQB_ParkWiperWhenHoodOpened()
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
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_ParkWipersWhenOpeningHood_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x004306B8 File Offset: 0x0042E8B8
		public static ICodingContainer WipersServicePositionMenu()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A72", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
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
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_WindshieldWiperServicePositionActivationThroughMenuOption_Name"), Translate.GetString("codingDB_WindshieldWiperServicePositionActivationThroughMenuOption_Description"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x00430758 File Offset: 0x0042E958
		public static ICodingContainer MIB3_DisplayWiperMenu()
		{
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB3("0B1B", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[12] = 160;
				}
				else
				{
					array[12] = 0;
				}
				return array;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.Washer, "", "", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB });
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0B1D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[12] = 99;
				}
				else
				{
					array2[12] = 0;
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding(CodingGroup.Washer, "", "", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB2 });
			return new MQBMultipleCoding(CodingGroup.Washer, Translate.GetString("codingDB_WipersMenuInMultimediaSystemMib3Only_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding, mqbalternativeCoding2 });
		}

		// Token: 0x06005A22 RID: 23074 RVA: 0x00430838 File Offset: 0x0042EA38
		public static ICodingContainer MQB_09_RearCameraWashHMI()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "11EA", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_RearCameraWasherButton_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit })
			{
				RequiresPro = false
			};
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x004308A8 File Offset: 0x0042EAA8
		public static ICodingContainer LowWasherLevelSensorInstalled()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.Washer, Translate.GetString("codingDB_LowWasherLevelSensorInstalled_Name"), Translate.GetString("codingDB_LowWasherLevelSensorInstalled_ShortName"), "0A55", 0, 5, "0600", 14, 2, Array.Empty<TranslationItem>());
		}

		// Token: 0x02000B77 RID: 2935
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005A24 RID: 23076 RVA: 0x004308E4 File Offset: 0x0042EAE4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005A25 RID: 23077 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005A26 RID: 23078 RVA: 0x004308F0 File Offset: 0x0042EAF0
			internal byte[] <ReduceWasherDelay>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
					BitHelpers.SwitchBitInByte(array, 2, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
				}
				else
				{
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
					BitHelpers.SwitchBitInByte(array, 2, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
				}
				return array;
			}

			// Token: 0x06005A27 RID: 23079 RVA: 0x0043098C File Offset: 0x0042EB8C
			internal byte[] <DropsTear_FrontWiper>b__1_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}

			// Token: 0x06005A28 RID: 23080 RVA: 0x004309D8 File Offset: 0x0042EBD8
			internal byte[] <DropsTear_FrontWiper>b__1_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
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

			// Token: 0x06005A29 RID: 23081 RVA: 0x00430A24 File Offset: 0x0042EC24
			internal byte[] <DropsTear_FrontWiper>b__1_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
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

			// Token: 0x06005A2A RID: 23082 RVA: 0x00430A70 File Offset: 0x0042EC70
			internal byte[] <DropsTear_RearWiper>b__2_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
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

			// Token: 0x06005A2B RID: 23083 RVA: 0x00430ABC File Offset: 0x0042ECBC
			internal byte[] <DropsTear_RearWiper>b__2_1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A2C RID: 23084 RVA: 0x00430B08 File Offset: 0x0042ED08
			internal byte[] <DropsTear_RearWiper>b__2_2(byte[] data, string value, MQBEasyCodingItem codingItem)
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

			// Token: 0x06005A2D RID: 23085 RVA: 0x00430B54 File Offset: 0x0042ED54
			internal byte[] <MQB_09_ComfortRearWiper>b__3_0(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A2E RID: 23086 RVA: 0x00430BA0 File Offset: 0x0042EDA0
			internal byte[] <MQB_09_ComfortRearWiper>b__3_1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A2F RID: 23087 RVA: 0x00430BEC File Offset: 0x0042EDEC
			internal byte[] <MQB_09_AutoRearWiper>b__4_0(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A30 RID: 23088 RVA: 0x00430C38 File Offset: 0x0042EE38
			internal byte[] <MQB_09_AutoRearWiper>b__4_1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A31 RID: 23089 RVA: 0x00430C84 File Offset: 0x0042EE84
			internal byte[] <ParkWipersAfterIgnitionTurnedOff>b__5_0(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				return array;
			}

			// Token: 0x06005A32 RID: 23090 RVA: 0x00430CD0 File Offset: 0x0042EED0
			internal byte[] <ParkWipersAfterIgnitionTurnedOff>b__5_1(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 6, 3, true);
				}
				return array;
			}

			// Token: 0x06005A33 RID: 23091 RVA: 0x00430D1C File Offset: 0x0042EF1C
			internal byte[] <MQB_StopWiperWhenHoodOpened>b__12_0(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A34 RID: 23092 RVA: 0x00430D68 File Offset: 0x0042EF68
			internal byte[] <MQB_StopWiperWhenHoodOpened>b__12_1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A35 RID: 23093 RVA: 0x00430DB4 File Offset: 0x0042EFB4
			internal byte[] <MQB_ParkWiperWhenHoodOpened>b__13_0(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A36 RID: 23094 RVA: 0x00430E00 File Offset: 0x0042F000
			internal byte[] <MQB_ParkWiperWhenHoodOpened>b__13_1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005A37 RID: 23095 RVA: 0x00430E4C File Offset: 0x0042F04C
			internal byte[] <WipersServicePositionMenu>b__14_0(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
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

			// Token: 0x06005A38 RID: 23096 RVA: 0x00430E98 File Offset: 0x0042F098
			internal byte[] <WipersServicePositionMenu>b__14_1(byte[] data, string value, MQBAlternativeCoding coding1)
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

			// Token: 0x06005A39 RID: 23097 RVA: 0x00430EE4 File Offset: 0x0042F0E4
			internal byte[] <MIB3_DisplayWiperMenu>b__15_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[12] = 160;
				}
				else
				{
					array[12] = 0;
				}
				return array;
			}

			// Token: 0x06005A3A RID: 23098 RVA: 0x00430F2C File Offset: 0x0042F12C
			internal byte[] <MIB3_DisplayWiperMenu>b__15_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[12] = 99;
				}
				else
				{
					array[12] = 0;
				}
				return array;
			}

			// Token: 0x06005A3B RID: 23099 RVA: 0x00430F70 File Offset: 0x0042F170
			internal byte[] <MQB_09_RearCameraWashHMI>b__16_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}

			// Token: 0x04003894 RID: 14484
			public static readonly Washer.<>c <>9 = new Washer.<>c();

			// Token: 0x04003895 RID: 14485
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x04003896 RID: 14486
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_0;

			// Token: 0x04003897 RID: 14487
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_1;

			// Token: 0x04003898 RID: 14488
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_2;

			// Token: 0x04003899 RID: 14489
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_0;

			// Token: 0x0400389A RID: 14490
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_1;

			// Token: 0x0400389B RID: 14491
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_2;

			// Token: 0x0400389C RID: 14492
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_0;

			// Token: 0x0400389D RID: 14493
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_1;

			// Token: 0x0400389E RID: 14494
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_0;

			// Token: 0x0400389F RID: 14495
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_1;

			// Token: 0x040038A0 RID: 14496
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__5_0;

			// Token: 0x040038A1 RID: 14497
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__5_1;

			// Token: 0x040038A2 RID: 14498
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_0;

			// Token: 0x040038A3 RID: 14499
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_1;

			// Token: 0x040038A4 RID: 14500
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__13_0;

			// Token: 0x040038A5 RID: 14501
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__13_1;

			// Token: 0x040038A6 RID: 14502
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__14_0;

			// Token: 0x040038A7 RID: 14503
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__14_1;

			// Token: 0x040038A8 RID: 14504
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_0;

			// Token: 0x040038A9 RID: 14505
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_1;

			// Token: 0x040038AA RID: 14506
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__16_0;
		}
	}
}
