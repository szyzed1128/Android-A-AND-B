using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A4A RID: 2634
	internal static class Engine
	{
		// Token: 0x06005346 RID: 21318 RVA: 0x003FD0D4 File Offset: 0x003FB2D4
		public static MQBEasyCodingItem MQB_01_AcceleratorPedalSensivity()
		{
			return new MQBEasyCodingItem(CodingGroup.EngineAndPowertrain, "Accelerator pedal high sensivity", "Not recommended for cars with cruise control", "0600", "7E0", "7E8", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
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
					new TranslationItem("ru", "Изменение отклика на педаль акселератора", "Не рекомендуется для автомобилей с круиз-контролем, возможны глюки", "")
				}
			};
		}

		// Token: 0x02000A4B RID: 2635
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005347 RID: 21319 RVA: 0x003FD164 File Offset: 0x003FB364
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005348 RID: 21320 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005349 RID: 21321 RVA: 0x003FD170 File Offset: 0x003FB370
			internal byte[] <MQB_01_AcceleratorPedalSensivity>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}

			// Token: 0x040032F6 RID: 13046
			public static readonly Engine.<>c <>9 = new Engine.<>c();

			// Token: 0x040032F7 RID: 13047
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;
		}
	}
}
