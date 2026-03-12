using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020
{
	// Token: 0x02000A25 RID: 2597
	internal static class Brakes
	{
		// Token: 0x0600529D RID: 21149 RVA: 0x003F9D8C File Offset: 0x003F7F8C
		public static MQBEasyCodingItem PQ26_2020_HillHoldControlActivation()
		{
			return new MQBEasyCodingItem(CodingGroup.Brakes, "Hill Hold Control activation", "HCC prevents roll-back", "0600", "713", "77D", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[47] = 66;
					array[51] = 66;
				}
				else
				{
					array[47] = 2;
					array[51] = 64;
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
					new TranslationItem("ru", "Ассистент движения на подъеме (активация)", "Предотвращает скатывание автомобиля", "")
				},
				PostWriteCommands = "1102"
			};
		}

		// Token: 0x02000A26 RID: 2598
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600529E RID: 21150 RVA: 0x003F9E27 File Offset: 0x003F8027
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600529F RID: 21151 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060052A0 RID: 21152 RVA: 0x003F9E34 File Offset: 0x003F8034
			internal byte[] <PQ26_2020_HillHoldControlActivation>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[47] = 66;
					array[51] = 66;
				}
				else
				{
					array[47] = 2;
					array[51] = 64;
				}
				return array;
			}

			// Token: 0x0400327F RID: 12927
			public static readonly Brakes.<>c <>9 = new Brakes.<>c();

			// Token: 0x04003280 RID: 12928
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;
		}
	}
}
