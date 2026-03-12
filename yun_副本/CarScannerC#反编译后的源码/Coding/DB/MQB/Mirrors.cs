using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B02 RID: 2818
	internal static class Mirrors
	{
		// Token: 0x06005805 RID: 22533 RVA: 0x0041FCC8 File Offset: 0x0041DEC8
		public static ICodingContainer CloseMirrorsHoldingKeylessDoor()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 3, 1, true);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Mirrors, Translate.GetString("codingDB_CloseMirrorsByHoldingDoorKeylessSensor_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x0041FD60 File Offset: 0x0041DF60
		public static ICodingContainer MQB_09_OpenSideMirrorsWithUnlocking()
		{
			MQBAdaptationOption option_unlock = new MQBAdaptationOption(Translate.GetString("codingDB_Unlock_Name"), MQBAdaptationTemplate.EnableOption.Value);
			MQBAdaptationOption option_ignition = new MQBAdaptationOption(Translate.GetString("codingDB_Ignition_Name"), MQBAdaptationTemplate.DisableOption.Value);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == option_ignition.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding coding2)
			{
				if (!BitHelpers.GetBit_0_7(data[1], 0))
				{
					return option_ignition.Title;
				}
				return option_unlock.Title;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == option_ignition.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 5, 5, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 5, 5, true);
				}
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding coding2)
			{
				if (!BitHelpers.GetBit_0_7(data[5], 5))
				{
					return option_ignition.Title;
				}
				return option_unlock.Title;
			});
			return new MQBAlternativeCoding(CodingGroup.Mirrors, Translate.GetString("codingDB_OpenSideMirrorsWhenUnlockingOrIgnitionStarting_Name"), "", "70E", "778", "31347", new MQBAdaptationOption[] { option_unlock, option_ignition })
			{
				InnerDescription = Translate.GetString("codingDB_OpenSideMirrorsWhenUnlockingOrIgnitionStarting_InnerDescription"),
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 }
			};
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x0041FE60 File Offset: 0x0041E060
		public static ICodingContainer MQB_09_RightMirrorGoDownWhenReversWithMemmory()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 2, true);
					BitHelpers.SwitchBitInByte(array2, 2, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 2, 2, false);
					BitHelpers.SwitchBitInByte(array2, 2, 7, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "52", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem2)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 4, 2, true);
					BitHelpers.SwitchBitInByte(array3, 4, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 4, 2, false);
					BitHelpers.SwitchBitInByte(array3, 4, 3, false);
				}
				return array3;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Mirrors, Translate.GetString("codingDB_PutRightMirrorDownWhenReversingMirrorsWithMemoryFunction_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem });
		}

		// Token: 0x06005808 RID: 22536 RVA: 0x0041FF54 File Offset: 0x0041E154
		public static ICodingContainer MQB_09_RightMirrorGoDownWhenReversWithoutMemmory()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A6E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 2, true);
					BitHelpers.SwitchBitInByte(array2, 2, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 2, 2, false);
					BitHelpers.SwitchBitInByte(array2, 2, 7, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "52", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem2)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 4, 2, true);
					BitHelpers.SwitchBitInByte(array3, 4, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 4, 2, false);
					BitHelpers.SwitchBitInByte(array3, 4, 3, false);
				}
				return array3;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Mirrors, Translate.GetString("codingDB_PutRightMirrorDownWhenReversingMirrorsWithoutMemoryFunction_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem });
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x00420048 File Offset: 0x0041E248
		public static ICodingContainer MQB_MirrorLightsWhenFolded()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.Mirrors, "", "", "0600", VagUnitHelper.GetRequestHeaderForMQBUnit("42"), VagUnitHelper.GetResponseHeaderForMQBUnit("42"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.Mirrors, "", "", "0600", VagUnitHelper.GetRequestHeaderForMQBUnit("52"), VagUnitHelper.GetResponseHeaderForMQBUnit("52"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 5, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 5, true);
				}
				return array2;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.Mirrors, Translate.GetString("codingDB_MirrorSideLightActiveWhenMirrorsAreFolded_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x00420150 File Offset: 0x0041E350
		public static ICodingContainer Mirrors_FunkSpiegelanklappen()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.Mirrors, Translate.GetString("codingDB_UnfoldMirrorsFunction_Name"), "Spiegelverstellung-Funk Spiegelanklappen", "0A6E", 0, 3, "0D0D", 3, 0, Array.Empty<TranslationItem>());
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x00420188 File Offset: 0x0041E388
		public static ICodingContainer Mirrors_FoldingOptionsInMMI()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.Mirrors, Translate.GetString("codingDB_MirrorFoldingOptionsInMmi_Name"), "Menuesteuerung Funk Spiegel anklappen", "0A6E", 0, 6, "0D0D", 3, 3, Array.Empty<TranslationItem>());
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x004201C0 File Offset: 0x0041E3C0
		public static ICodingContainer Mirrors_ProfilfunctionForFoldingMirrors()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.Mirrors, Translate.GetString("codingDB_ProfileFunctionForFoldingMirrors_Name"), "Spiegelverstellung-Profilfunktion fuer Anklappung der Aussenspiegel", "0A6E", 0, 7, "0D0D", 5, 4, Array.Empty<TranslationItem>());
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x004201F8 File Offset: 0x0041E3F8
		public static ICodingContainer Mirrors_FoldingMirrorsWhenRepeatLocking()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.Mirrors, Translate.GetString("codingDB_FoldMirrorsWhenRelocking_Name"), "Spiegelverstellung-Spiegelanklappen_bei_Wiederverriegelung", "0A6E", 1, 1, "FFFF", 99, 0, Array.Empty<TranslationItem>());
			mqbalternativeCoding.Alternatives.RemoveAll((MQBAlternativeContainer x) => !(x as MQBAlternativeContainerForUnit09).RequiresNewBCM);
			return mqbalternativeCoding;
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x0042025C File Offset: 0x0041E45C
		public static ICodingContainer Mirrors_SyncMenu()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.Mirrors, "", "Spiegelverstellung-Menuesteuerung synchrone Spiegelverstellung", "0A6E", 0, 5, "0D0D", 3, 2, Array.Empty<TranslationItem>());
			MQBAlternativeCoding mqbalternativeCoding2 = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.Mirrors, "", "Spiegelverstellung-Spiegelverstellung Synchron", "0A6E", 0, 1, "0D0D", 2, 3, Array.Empty<TranslationItem>());
			return new MQBMultipleCoding(CodingGroup.Mirrors, Translate.GetString("codingDB_SynchronousMirrorsAdjustmentMenuInMmi_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding2, mqbalternativeCoding });
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x004202D8 File Offset: 0x0041E4D8
		public static ICodingContainer Mirrors_HeatWithRearWindow()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", VagUnitHelper.GetRequestHeaderForMQBUnit("42"), VagUnitHelper.GetResponseHeaderForMQBUnit("42"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", VagUnitHelper.GetRequestHeaderForMQBUnit("52"), VagUnitHelper.GetResponseHeaderForMQBUnit("52"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 9, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 9, 2, false);
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Mirrors, Translate.GetString("codingDB_ActivateMirrorsHeatersWhenRearWindowHeaterActivated_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x0042039C File Offset: 0x0041E59C
		public static ICodingContainer Mirrors_HeaterAlwasyActive()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", VagUnitHelper.GetRequestHeaderForMQBUnit("42"), VagUnitHelper.GetResponseHeaderForMQBUnit("42"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 9, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 9, 1, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", VagUnitHelper.GetRequestHeaderForMQBUnit("52"), VagUnitHelper.GetResponseHeaderForMQBUnit("52"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 9, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 9, 1, false);
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Mirrors, Translate.GetString("codingDB_MirrorsHeatersAlwaysActive_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x02000B03 RID: 2819
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005811 RID: 22545 RVA: 0x00420460 File Offset: 0x0041E660
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005812 RID: 22546 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005813 RID: 22547 RVA: 0x0042046C File Offset: 0x0041E66C
			internal byte[] <CloseMirrorsHoldingKeylessDoor>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005814 RID: 22548 RVA: 0x004204B8 File Offset: 0x0041E6B8
			internal byte[] <CloseMirrorsHoldingKeylessDoor>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
				}
				return array;
			}

			// Token: 0x06005815 RID: 22549 RVA: 0x00420504 File Offset: 0x0041E704
			internal byte[] <MQB_09_RightMirrorGoDownWhenReversWithMemmory>b__2_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x06005816 RID: 22550 RVA: 0x00420560 File Offset: 0x0041E760
			internal byte[] <MQB_09_RightMirrorGoDownWhenReversWithMemmory>b__2_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
				}
				return array;
			}

			// Token: 0x06005817 RID: 22551 RVA: 0x004205BC File Offset: 0x0041E7BC
			internal byte[] <MQB_09_RightMirrorGoDownWhenReversWithMemmory>b__2_2(byte[] data, string value, MQBEasyCodingItem codingItem2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
					BitHelpers.SwitchBitInByte(array, 4, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
					BitHelpers.SwitchBitInByte(array, 4, 3, false);
				}
				return array;
			}

			// Token: 0x06005818 RID: 22552 RVA: 0x00420618 File Offset: 0x0041E818
			internal byte[] <MQB_09_RightMirrorGoDownWhenReversWithoutMemmory>b__3_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x06005819 RID: 22553 RVA: 0x00420674 File Offset: 0x0041E874
			internal byte[] <MQB_09_RightMirrorGoDownWhenReversWithoutMemmory>b__3_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
				}
				return array;
			}

			// Token: 0x0600581A RID: 22554 RVA: 0x004206D0 File Offset: 0x0041E8D0
			internal byte[] <MQB_09_RightMirrorGoDownWhenReversWithoutMemmory>b__3_2(byte[] data, string value, MQBEasyCodingItem codingItem2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
					BitHelpers.SwitchBitInByte(array, 4, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
					BitHelpers.SwitchBitInByte(array, 4, 3, false);
				}
				return array;
			}

			// Token: 0x0600581B RID: 22555 RVA: 0x0042072C File Offset: 0x0041E92C
			internal byte[] <MQB_MirrorLightsWhenFolded>b__4_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				return array;
			}

			// Token: 0x0600581C RID: 22556 RVA: 0x00420778 File Offset: 0x0041E978
			internal byte[] <MQB_MirrorLightsWhenFolded>b__4_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				return array;
			}

			// Token: 0x0600581D RID: 22557 RVA: 0x004207C1 File Offset: 0x0041E9C1
			internal bool <Mirrors_FoldingMirrorsWhenRepeatLocking>b__8_0(MQBAlternativeContainer x)
			{
				return !(x as MQBAlternativeContainerForUnit09).RequiresNewBCM;
			}

			// Token: 0x0600581E RID: 22558 RVA: 0x004207D4 File Offset: 0x0041E9D4
			internal byte[] <Mirrors_HeatWithRearWindow>b__10_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, false);
				}
				return array;
			}

			// Token: 0x0600581F RID: 22559 RVA: 0x00420820 File Offset: 0x0041EA20
			internal byte[] <Mirrors_HeatWithRearWindow>b__10_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, false);
				}
				return array;
			}

			// Token: 0x06005820 RID: 22560 RVA: 0x0042086C File Offset: 0x0041EA6C
			internal byte[] <Mirrors_HeaterAlwasyActive>b__11_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 9, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 9, 1, false);
				}
				return array;
			}

			// Token: 0x06005821 RID: 22561 RVA: 0x004208B8 File Offset: 0x0041EAB8
			internal byte[] <Mirrors_HeaterAlwasyActive>b__11_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 9, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 9, 1, false);
				}
				return array;
			}

			// Token: 0x0400368E RID: 13966
			public static readonly Mirrors.<>c <>9 = new Mirrors.<>c();

			// Token: 0x0400368F RID: 13967
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x04003690 RID: 13968
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x04003691 RID: 13969
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_0;

			// Token: 0x04003692 RID: 13970
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_1;

			// Token: 0x04003693 RID: 13971
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_2;

			// Token: 0x04003694 RID: 13972
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_0;

			// Token: 0x04003695 RID: 13973
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_1;

			// Token: 0x04003696 RID: 13974
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_2;

			// Token: 0x04003697 RID: 13975
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__4_0;

			// Token: 0x04003698 RID: 13976
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__4_1;

			// Token: 0x04003699 RID: 13977
			public static Predicate<MQBAlternativeContainer> <>9__8_0;

			// Token: 0x0400369A RID: 13978
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__10_0;

			// Token: 0x0400369B RID: 13979
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__10_1;

			// Token: 0x0400369C RID: 13980
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__11_0;

			// Token: 0x0400369D RID: 13981
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__11_1;
		}

		// Token: 0x02000B04 RID: 2820
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06005822 RID: 22562 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06005823 RID: 22563 RVA: 0x00420904 File Offset: 0x0041EB04
			internal byte[] <MQB_09_OpenSideMirrorsWithUnlocking>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.option_ignition.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
				}
				return array;
			}

			// Token: 0x06005824 RID: 22564 RVA: 0x0042094E File Offset: 0x0041EB4E
			internal string <MQB_09_OpenSideMirrorsWithUnlocking>b__1(byte[] data, MQBAlternativeCoding coding2)
			{
				if (!BitHelpers.GetBit_0_7(data[1], 0))
				{
					return this.option_ignition.Title;
				}
				return this.option_unlock.Title;
			}

			// Token: 0x06005825 RID: 22565 RVA: 0x00420974 File Offset: 0x0041EB74
			internal byte[] <MQB_09_OpenSideMirrorsWithUnlocking>b__2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.option_ignition.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 5, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 5, true);
				}
				return array;
			}

			// Token: 0x06005826 RID: 22566 RVA: 0x004209BE File Offset: 0x0041EBBE
			internal string <MQB_09_OpenSideMirrorsWithUnlocking>b__3(byte[] data, MQBAlternativeCoding coding2)
			{
				if (!BitHelpers.GetBit_0_7(data[5], 5))
				{
					return this.option_ignition.Title;
				}
				return this.option_unlock.Title;
			}

			// Token: 0x0400369E RID: 13982
			public MQBAdaptationOption option_ignition;

			// Token: 0x0400369F RID: 13983
			public MQBAdaptationOption option_unlock;
		}
	}
}
