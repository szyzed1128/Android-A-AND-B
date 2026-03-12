using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A97 RID: 2711
	internal static class Dashboard
	{
		// Token: 0x06005597 RID: 21911 RVA: 0x0040C6FC File Offset: 0x0040A8FC
		public static ICodingContainer DisableTurnOnLightWarning_09()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A57", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
				}
				return array;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Dashboard, Translate.GetString("codingDB_DisableTurnOnLightsWarning_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit });
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x0040C764 File Offset: 0x0040A964
		public static MQBEasyCodingItem MQB_17_FreeSpaceInFuelTankDisplay()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_ShowFreeSpaceInFuelTankInTheDashboard_Name"), "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 4, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x0040C7D8 File Offset: 0x0040A9D8
		public static MQBEasyCodingItem MQB_17_PointerTest()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_TestPointersAtEngineStart_Name"), "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x0040C84C File Offset: 0x0040AA4C
		public static MQBEasyCodingItem MQB_17_Time24HoursFormat()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_TimeFormat2412_Name"), "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 7, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 6, 7, true);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				new MQBAdaptationOption("24", MQBAdaptationTemplate.EnableOption.Value),
				new MQBAdaptationOption("12", MQBAdaptationTemplate.DisableOption.Value)
			});
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x0040C8DC File Offset: 0x0040AADC
		public static MQBEasyCodingItem MQB_17_LapTimerDisplay()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_ShowLapTimer_Name"), "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
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
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x0040C950 File Offset: 0x0040AB50
		public static MQBEasyCodingItem MQB_17_OilTemperatureDisplay()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_DisplayOilTemperature_Name"), "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x0040C9C4 File Offset: 0x0040ABC4
		public static MQBEasyCodingItem MQB_17_AccelerationDisplay()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_ShowAcceleration_Name"), "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 2, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x0040CA38 File Offset: 0x0040AC38
		public static MQBEasyCodingItem MQB_17_OutsideTemperatureDisplay()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_ShowAmbientTemperature_Name"), "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 3, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x0040CAAC File Offset: 0x0040ACAC
		public static MQBEasyCodingItem MQB_17_WarningWhen120kmh()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_ShowWarningWhenSpeed120KmH75Mph_Name"), "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
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
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x0040CB20 File Offset: 0x0040AD20
		public static ICodingContainer MQB_WarnAboutRearFogLightsSpeedLimit()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A59", "31347", 1, 1, 1.0, 0.0, false, false);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(true, "0D05", "31347", 13, 1, 1.0, 0.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.Washer, Translate.GetString("codingDB_RearFogLightsTurnedOnWarningWhenReachingSpeedLimit_Name"), Translate.GetString("codingDB_RearFogLightsTurnedOnWarningWhenReachingSpeedLimit_Description"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x0040CBBC File Offset: 0x0040ADBC
		public static MQBEasyCodingItem MQB_17_DisplaySoC()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, Translate.GetString("codingDB_DisplayStateOfChargeSoc_Name"), Translate.GetString("codingDB_DisplayStateOfChargeSoc_Description"), "22DC", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
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
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				PasswordHint = "20103",
				InnerDescription = Translate.GetString("codingDB_DisplayStateOfChargeSoc_InnerDescription"),
				Translations = 
				{
					new TranslationItem("ru", "Отображение заряда аккумулятора на экране панели приборов", "Не совместимо с цифровой панелью приборов", "При выключенном зажигании зажмите кнопку RESET/SET (сброс). В момент, когда на экране будет указано Battery status отпустите кнопку")
				}
			};
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x0040CC74 File Offset: 0x0040AE74
		public static ICodingContainer DigitalDashboardConfigurationInMMI()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("09F2", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[5] = 1;
				}
				else
				{
					array[5] = 0;
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 56, 7, true);
						BitHelpers.SwitchBitInByte(array2, 56, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 56, 7, false);
						BitHelpers.SwitchBitInByte(array2, 56, 5, false);
					}
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Dashboard, Translate.GetString("codingDB_DigitalLcdDashboardConfigurationMenuInMultimediaSystem_Name"), "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 })
			{
				RequiresPro = false,
				InnerDescription = Translate.GetString("codingDB_DigitalLcdDashboardConfigurationMenuInMultimediaSystem_InnerDescription")
			};
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x0040CD2C File Offset: 0x0040AF2C
		internal static ICodingContainer AudiVC1GenSportLayout()
		{
			return new CustomizableCodingTemplate
			{
				RequestHeader = "714",
				ResponseHeader = "77E",
				Name = "Audi Sport Layout (FPK Gen.1 / VC Gen.1 only AUDI)",
				Description = "Only AUDI, only Virtual Cockpit gen.1!",
				Password = "20103",
				PasswordHint = "20103",
				ReadModeAndAddress = "231403003C0901",
				WriteModeAndAddress = "3D1403003C0901",
				MakeChangesToInitialData = false,
				ValueType = AdaptationValueTypes.OptionType,
				OpenSessionCommand = "104F",
				PreReadCommands = "22F198;22F199;2703;2704;2EF198;2EF199",
				PreWriteCommands = "22F198;22F199;2703;2704;2EF198;2EF199",
				PostWriteCommands = "1102",
				Group = CodingGroup.Dashboard,
				Protocol = "6",
				Options = 
				{
					new MQBAdaptationOption(MQBAdaptationTemplate.DisableOption.Title, "00", "00", Array.Empty<TranslationItem>()),
					new MQBAdaptationOption("S Style", "01", "01", Array.Empty<TranslationItem>()),
					new MQBAdaptationOption("RS Style", "03", "03", Array.Empty<TranslationItem>())
				}
			};
		}

		// Token: 0x02000A98 RID: 2712
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060055A4 RID: 21924 RVA: 0x0040CE53 File Offset: 0x0040B053
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060055A5 RID: 21925 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060055A6 RID: 21926 RVA: 0x0040CE60 File Offset: 0x0040B060
			internal byte[] <DisableTurnOnLightWarning_09>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
				}
				return array;
			}

			// Token: 0x060055A7 RID: 21927 RVA: 0x0040CEBC File Offset: 0x0040B0BC
			internal byte[] <MQB_17_FreeSpaceInFuelTankDisplay>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 4, false);
				}
				return array;
			}

			// Token: 0x060055A8 RID: 21928 RVA: 0x0040CF08 File Offset: 0x0040B108
			internal byte[] <MQB_17_PointerTest>b__2_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
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

			// Token: 0x060055A9 RID: 21929 RVA: 0x0040CF54 File Offset: 0x0040B154
			internal byte[] <MQB_17_Time24HoursFormat>b__3_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 7, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 6, 7, true);
				}
				return array;
			}

			// Token: 0x060055AA RID: 21930 RVA: 0x0040CFA0 File Offset: 0x0040B1A0
			internal byte[] <MQB_17_LapTimerDisplay>b__4_0(byte[] data, string value, MQBEasyCodingItem codingItem)
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

			// Token: 0x060055AB RID: 21931 RVA: 0x0040CFEC File Offset: 0x0040B1EC
			internal byte[] <MQB_17_OilTemperatureDisplay>b__5_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, false);
				}
				return array;
			}

			// Token: 0x060055AC RID: 21932 RVA: 0x0040D038 File Offset: 0x0040B238
			internal byte[] <MQB_17_AccelerationDisplay>b__6_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 2, false);
				}
				return array;
			}

			// Token: 0x060055AD RID: 21933 RVA: 0x0040D084 File Offset: 0x0040B284
			internal byte[] <MQB_17_OutsideTemperatureDisplay>b__7_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 3, false);
				}
				return array;
			}

			// Token: 0x060055AE RID: 21934 RVA: 0x0040D0D0 File Offset: 0x0040B2D0
			internal byte[] <MQB_17_WarningWhen120kmh>b__8_0(byte[] data, string value, MQBEasyCodingItem codingItem)
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

			// Token: 0x060055AF RID: 21935 RVA: 0x0040D11C File Offset: 0x0040B31C
			internal byte[] <MQB_17_DisplaySoC>b__10_0(byte[] data, string value, MQBEasyCodingItem codingItem)
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

			// Token: 0x060055B0 RID: 21936 RVA: 0x0040D15C File Offset: 0x0040B35C
			internal byte[] <DigitalDashboardConfigurationInMMI>b__11_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[5] = 1;
				}
				else
				{
					array[5] = 0;
				}
				return array;
			}

			// Token: 0x060055B1 RID: 21937 RVA: 0x0040D19C File Offset: 0x0040B39C
			internal byte[] <DigitalDashboardConfigurationInMMI>b__11_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 56, 7, true);
						BitHelpers.SwitchBitInByte(array, 56, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 56, 7, false);
						BitHelpers.SwitchBitInByte(array, 56, 5, false);
					}
				}
				return array;
			}

			// Token: 0x04003498 RID: 13464
			public static readonly Dashboard.<>c <>9 = new Dashboard.<>c();

			// Token: 0x04003499 RID: 13465
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x0400349A RID: 13466
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x0400349B RID: 13467
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x0400349C RID: 13468
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_0;

			// Token: 0x0400349D RID: 13469
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__4_0;

			// Token: 0x0400349E RID: 13470
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_0;

			// Token: 0x0400349F RID: 13471
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_0;

			// Token: 0x040034A0 RID: 13472
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__7_0;

			// Token: 0x040034A1 RID: 13473
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__8_0;

			// Token: 0x040034A2 RID: 13474
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__10_0;

			// Token: 0x040034A3 RID: 13475
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__11_0;

			// Token: 0x040034A4 RID: 13476
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__11_1;
		}
	}
}
