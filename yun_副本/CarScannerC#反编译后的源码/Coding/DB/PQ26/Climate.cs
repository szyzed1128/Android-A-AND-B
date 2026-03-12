using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A42 RID: 2626
	internal static class Climate
	{
		// Token: 0x06005321 RID: 21281 RVA: 0x003FC4E0 File Offset: 0x003FA6E0
		public static MQBEasyCodingItem PQ26_08_RememberLastRecyrculationPositionClimate()
		{
			return new MQBEasyCodingItem(CodingGroup.Climate, "Remember last air recyrculation position (with Climate control)", "", "0600", "746", "7B0", "", "5JA907", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
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
					new TranslationItem("ru", "Запоминание последнего состояния рециркуляции (комплектация с климат-контролем)", "", "")
				}
			};
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x003FC570 File Offset: 0x003FA770
		public static MQBEasyCodingItem PQ26_08_RememberLastRecyrculationPositionAC()
		{
			return new MQBEasyCodingItem(CodingGroup.Climate, "Remember last air recyrculation position (without Climate control)", "", "0600", "746", "7B0", "", "5JA820", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[4], 4))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "Запоминание последнего состояния рециркуляции (комплектация без климат-контроля)", "", "")
				}
			};
		}

		// Token: 0x02000A43 RID: 2627
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005323 RID: 21283 RVA: 0x003FC61E File Offset: 0x003FA81E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005324 RID: 21284 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005325 RID: 21285 RVA: 0x003FC62C File Offset: 0x003FA82C
			internal byte[] <PQ26_08_RememberLastRecyrculationPositionClimate>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				return array;
			}

			// Token: 0x06005326 RID: 21286 RVA: 0x003FC678 File Offset: 0x003FA878
			internal byte[] <PQ26_08_RememberLastRecyrculationPositionAC>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
				}
				return array;
			}

			// Token: 0x06005327 RID: 21287 RVA: 0x003FC6C1 File Offset: 0x003FA8C1
			internal string <PQ26_08_RememberLastRecyrculationPositionAC>b__1_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[4], 4))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x040032E3 RID: 13027
			public static readonly Climate.<>c <>9 = new Climate.<>c();

			// Token: 0x040032E4 RID: 13028
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x040032E5 RID: 13029
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x040032E6 RID: 13030
			public static Func<byte[], MQBEasyCodingItem, string> <>9__1_1;
		}
	}
}
