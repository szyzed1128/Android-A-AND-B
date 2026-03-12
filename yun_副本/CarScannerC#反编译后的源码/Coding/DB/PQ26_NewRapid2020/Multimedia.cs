using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020
{
	// Token: 0x02000A3C RID: 2620
	internal static class Multimedia
	{
		// Token: 0x06005304 RID: 21252 RVA: 0x003FB930 File Offset: 0x003F9B30
		public static MQBEasyCodingItem MQB_5F_AMRadio()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, "AM radio", "Enable or disable AM radio", "0559", "773", "7DD", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, true);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "AM диапазон в мультимедийной системе", "", "")
				}
			};
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x003FB9C4 File Offset: 0x003F9BC4
		public static MQBEasyCodingItem PQ26_5F_AUXInEnable()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, "AUX input activation", "Enable or disable AUX input for multimedia system", "0557", "773", "7DD", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация аналогового входа AUX-IN", "", "")
				}
			};
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x003FBA58 File Offset: 0x003F9C58
		public static ICodingContainer PQ26_5F_OffroadModeDisplayInMMI()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, "Show offroad screen in multimedia system (Swing-3)", "Compatibility: not tested yet", "0B1B", "773", "7DD", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 7, true);
					BitHelpers.SwitchBitInByte(array, 16, 6, true);
					BitHelpers.SwitchBitInByte(array, 16, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 7, false);
					BitHelpers.SwitchBitInByte(array, 16, 6, false);
					BitHelpers.SwitchBitInByte(array, 16, 5, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "Меню Offroad в мультимедийной системе (Swing-3)", "Совместимость: не проверено", "")
				}
			};
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x003FBAEC File Offset: 0x003F9CEC
		public static ICodingContainer PQ262020_5F_DrivingSchool()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, "Display driving school mode in MMI", "", "0B1B", VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 34, 7, true);
					BitHelpers.SwitchBitInByte(array, 34, 6, false);
					BitHelpers.SwitchBitInByte(array, 34, 5, true);
					BitHelpers.SwitchBitInByte(array, 34, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 34, 7, false);
					BitHelpers.SwitchBitInByte(array, 34, 6, false);
					BitHelpers.SwitchBitInByte(array, 34, 5, false);
					BitHelpers.SwitchBitInByte(array, 34, 4, false);
				}
				return array;
			}, null, Array.Empty<MQBAdaptationOption>())
			{
				InnerDescription = "After applying this coding hold power button on MMI for ~20 seconds to restart it.",
				Translations = 
				{
					new TranslationItem("ru", "Отображение режима автошколы (учебный автомобиль) в мультимедийной системе", "", "После применения этой кодировки требуется перезагрузить мультимедийную систему (зажмите кнопку включения на ~20 секунд).")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x003FBB88 File Offset: 0x003F9D88
		public static ICodingContainer MQB_5F_TripComputerAvailableWithIgnitionOff()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, "Access trip computer in multimedia when ignition turned off", "", "0B1B", "773", "7DD", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 6, false);
				}
				return array;
			}, null, Array.Empty<MQBAdaptationOption>())
			{
				Translations = 
				{
					new TranslationItem("ru", "Доступ в бортовой компьютер в мультимедийной системе при выключенном зажигании", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x003FBC10 File Offset: 0x003F9E10
		public static ICodingContainer PQ26_5F_DisplayComfortUsage()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", VagUnitHelper.GetRequestHeaderForMQBUnit("19"), VagUnitHelper.GetResponseHeaderForMQBUnit("19"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 1, true);
					BitHelpers.SwitchBitInByte(array, 13, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 13, 1, false);
					BitHelpers.SwitchBitInByte(array, 13, 4, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("099D", VagUnitHelper.GetRequestHeaderForMQBUnit("19"), VagUnitHelper.GetResponseHeaderForMQBUnit("19"), "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 0, false);
					BitHelpers.SwitchBitInByte(array2, 2, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 2, 0, true);
					BitHelpers.SwitchBitInByte(array2, 2, 1, true);
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0B1B", VagUnitHelper.GetRequestHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 36, 7, true);
					BitHelpers.SwitchBitInByte(array3, 36, 6, true);
					BitHelpers.SwitchBitInByte(array3, 36, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 36, 7, false);
					BitHelpers.SwitchBitInByte(array3, 36, 6, false);
					BitHelpers.SwitchBitInByte(array3, 36, 5, false);
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0B1D", VagUnitHelper.GetRequestHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[35] = 99;
				}
				else
				{
					array4[35] = 0;
				}
				return array4;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Multimedia, "Display comfort systems consumption in MMI", "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Отображение потребителей систем комфорта в мультимедийной системе", "", "")
				}
			};
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x003FBD90 File Offset: 0x003F9F90
		public static ICodingContainer PQ262020_5F_EcoDriving()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, Translate.GetString("codingDB_EcoDrivingStatisticsDisplayGreenBlueMenu_Name"), "", "0B1B", "773", "7DD", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 35, 7, true);
					BitHelpers.SwitchBitInByte(array, 35, 6, false);
					BitHelpers.SwitchBitInByte(array, 35, 5, true);
					BitHelpers.SwitchBitInByte(array, 35, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 35, 7, false);
					BitHelpers.SwitchBitInByte(array, 35, 6, false);
					BitHelpers.SwitchBitInByte(array, 35, 5, false);
					BitHelpers.SwitchBitInByte(array, 35, 4, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x02000A3D RID: 2621
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600530B RID: 21259 RVA: 0x003FBE02 File Offset: 0x003FA002
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600530C RID: 21260 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600530D RID: 21261 RVA: 0x003FBE10 File Offset: 0x003FA010
			internal byte[] <MQB_5F_AMRadio>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, true);
				}
				return array;
			}

			// Token: 0x0600530E RID: 21262 RVA: 0x003FBE5C File Offset: 0x003FA05C
			internal byte[] <PQ26_5F_AUXInEnable>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
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

			// Token: 0x0600530F RID: 21263 RVA: 0x003FBEA8 File Offset: 0x003FA0A8
			internal byte[] <PQ26_5F_OffroadModeDisplayInMMI>b__2_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 7, true);
					BitHelpers.SwitchBitInByte(array, 16, 6, true);
					BitHelpers.SwitchBitInByte(array, 16, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 7, false);
					BitHelpers.SwitchBitInByte(array, 16, 6, false);
					BitHelpers.SwitchBitInByte(array, 16, 5, false);
				}
				return array;
			}

			// Token: 0x06005310 RID: 21264 RVA: 0x003FBF1C File Offset: 0x003FA11C
			internal byte[] <PQ262020_5F_DrivingSchool>b__3_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 34, 7, true);
					BitHelpers.SwitchBitInByte(array, 34, 6, false);
					BitHelpers.SwitchBitInByte(array, 34, 5, true);
					BitHelpers.SwitchBitInByte(array, 34, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 34, 7, false);
					BitHelpers.SwitchBitInByte(array, 34, 6, false);
					BitHelpers.SwitchBitInByte(array, 34, 5, false);
					BitHelpers.SwitchBitInByte(array, 34, 4, false);
				}
				return array;
			}

			// Token: 0x06005311 RID: 21265 RVA: 0x003FBFA4 File Offset: 0x003FA1A4
			internal byte[] <MQB_5F_TripComputerAvailableWithIgnitionOff>b__4_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 6, false);
				}
				return array;
			}

			// Token: 0x06005312 RID: 21266 RVA: 0x003FBFF0 File Offset: 0x003FA1F0
			internal byte[] <PQ26_5F_DisplayComfortUsage>b__5_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 1, true);
					BitHelpers.SwitchBitInByte(array, 13, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 13, 1, false);
					BitHelpers.SwitchBitInByte(array, 13, 4, false);
				}
				return array;
			}

			// Token: 0x06005313 RID: 21267 RVA: 0x003FC050 File Offset: 0x003FA250
			internal byte[] <PQ26_5F_DisplayComfortUsage>b__5_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
				}
				return array;
			}

			// Token: 0x06005314 RID: 21268 RVA: 0x003FC0AC File Offset: 0x003FA2AC
			internal byte[] <PQ26_5F_DisplayComfortUsage>b__5_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 36, 7, true);
					BitHelpers.SwitchBitInByte(array, 36, 6, true);
					BitHelpers.SwitchBitInByte(array, 36, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 36, 7, false);
					BitHelpers.SwitchBitInByte(array, 36, 6, false);
					BitHelpers.SwitchBitInByte(array, 36, 5, false);
				}
				return array;
			}

			// Token: 0x06005315 RID: 21269 RVA: 0x003FC120 File Offset: 0x003FA320
			internal byte[] <PQ26_5F_DisplayComfortUsage>b__5_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[35] = 99;
				}
				else
				{
					array[35] = 0;
				}
				return array;
			}

			// Token: 0x06005316 RID: 21270 RVA: 0x003FC164 File Offset: 0x003FA364
			internal byte[] <PQ262020_5F_EcoDriving>b__6_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 35, 7, true);
					BitHelpers.SwitchBitInByte(array, 35, 6, false);
					BitHelpers.SwitchBitInByte(array, 35, 5, true);
					BitHelpers.SwitchBitInByte(array, 35, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 35, 7, false);
					BitHelpers.SwitchBitInByte(array, 35, 6, false);
					BitHelpers.SwitchBitInByte(array, 35, 5, false);
					BitHelpers.SwitchBitInByte(array, 35, 4, false);
				}
				return array;
			}

			// Token: 0x040032D3 RID: 13011
			public static readonly Multimedia.<>c <>9 = new Multimedia.<>c();

			// Token: 0x040032D4 RID: 13012
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x040032D5 RID: 13013
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x040032D6 RID: 13014
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x040032D7 RID: 13015
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_0;

			// Token: 0x040032D8 RID: 13016
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__4_0;

			// Token: 0x040032D9 RID: 13017
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_0;

			// Token: 0x040032DA RID: 13018
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_1;

			// Token: 0x040032DB RID: 13019
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_2;

			// Token: 0x040032DC RID: 13020
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_3;

			// Token: 0x040032DD RID: 13021
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__6_0;
		}
	}
}
