using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A44 RID: 2628
	internal static class Dashboard
	{
		// Token: 0x06005328 RID: 21288 RVA: 0x003FC6E3 File Offset: 0x003FA8E3
		public static ICodingContainer PQ26_17_PointerTest()
		{
			return Dashboard.MQB_17_PointerTest();
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x003FC6EA File Offset: 0x003FA8EA
		public static ICodingContainer PQ26_17_Time24HoursFormat()
		{
			return Dashboard.MQB_17_Time24HoursFormat();
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x003FC6F1 File Offset: 0x003FA8F1
		public static ICodingContainer PQ26_17_LapTimerDisplay()
		{
			return Dashboard.MQB_17_LapTimerDisplay();
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x003FC6F8 File Offset: 0x003FA8F8
		public static MQBEasyCodingItem PQ26_RemoveKeyWarning()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, "Remove key warning", "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
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
					new TranslationItem("ru", "Включение напоминания \"Извлеките ключ\"", "Совместимо только с приборными панелями старого образца", "")
				}
			};
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x003FC788 File Offset: 0x003FA988
		public static MQBEasyCodingItem PQ26_ShowECOHints()
		{
			return new MQBEasyCodingItem(CodingGroup.Dashboard, "Display ECO/efficiency hints", "", "0600", "714", "77E", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
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
					new TranslationItem("ru", "Отображение эко-советов", "", "")
				}
			};
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x003FC818 File Offset: 0x003FAA18
		public static MQBEasyCodingItem MQB_17_FreeSpaceInFuelTankDisplay()
		{
			return Dashboard.MQB_17_FreeSpaceInFuelTankDisplay();
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x003FC81F File Offset: 0x003FAA1F
		public static ICodingContainer MQB_WarnAboutRearFogLightsSpeedLimit()
		{
			return Dashboard.MQB_WarnAboutRearFogLightsSpeedLimit();
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x003FC826 File Offset: 0x003FAA26
		public static ICodingContainer MQB_17_OilTemperatureDisplay()
		{
			return Dashboard.MQB_17_OilTemperatureDisplay();
		}

		// Token: 0x02000A45 RID: 2629
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005330 RID: 21296 RVA: 0x003FC82D File Offset: 0x003FAA2D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005331 RID: 21297 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005332 RID: 21298 RVA: 0x003FC83C File Offset: 0x003FAA3C
			internal byte[] <PQ26_RemoveKeyWarning>b__3_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
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

			// Token: 0x06005333 RID: 21299 RVA: 0x003FC888 File Offset: 0x003FAA88
			internal byte[] <PQ26_ShowECOHints>b__4_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
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

			// Token: 0x040032E7 RID: 13031
			public static readonly Dashboard.<>c <>9 = new Dashboard.<>c();

			// Token: 0x040032E8 RID: 13032
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_0;

			// Token: 0x040032E9 RID: 13033
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__4_0;
		}
	}
}
