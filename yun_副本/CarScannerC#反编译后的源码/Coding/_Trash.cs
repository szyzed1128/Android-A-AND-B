using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000877 RID: 2167
	internal static class _Trash
	{
		// Token: 0x060049FB RID: 18939 RVA: 0x0037C094 File Offset: 0x0037A294
		public static MQBEasyCodingItem MQB_09_ActivateFogLightsWhenRearGearEngaged()
		{
			return new MQBEasyCodingItem(CodingGroup.Washer, "Activate fog lights when rear gear is engaged", "", "0D1D", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[0], 0))
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
					new TranslationItem("ru", "Автоматическое включение заднего стеклоочистителя", "", "")
				},
				RequiresPro = true
			};
		}

		// Token: 0x02000878 RID: 2168
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060049FC RID: 18940 RVA: 0x0037C14A File Offset: 0x0037A34A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060049FD RID: 18941 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060049FE RID: 18942 RVA: 0x0037C158 File Offset: 0x0037A358
			internal byte[] <MQB_09_ActivateFogLightsWhenRearGearEngaged>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
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

			// Token: 0x060049FF RID: 18943 RVA: 0x0037C1A1 File Offset: 0x0037A3A1
			internal string <MQB_09_ActivateFogLightsWhenRearGearEngaged>b__0_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[0], 0))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x04002AD0 RID: 10960
			public static readonly _Trash.<>c <>9 = new _Trash.<>c();

			// Token: 0x04002AD1 RID: 10961
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x04002AD2 RID: 10962
			public static Func<byte[], MQBEasyCodingItem, string> <>9__0_1;
		}
	}
}
