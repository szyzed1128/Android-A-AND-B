using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020
{
	// Token: 0x02000A23 RID: 2595
	internal static class Assistance
	{
		// Token: 0x06005298 RID: 21144 RVA: 0x003F9C08 File Offset: 0x003F7E08
		public static ICodingContainer PQ26_SpeedLimiter()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0AEA", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}, null);
			mqbeasyCodingItem.PostWriteCommands = "ATST16";
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "01", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 6, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 6, 2, false);
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Assistance, "Speed limiter", "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация ограничителя скорости", "", "")
				}
			};
		}

		// Token: 0x02000A24 RID: 2596
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005299 RID: 21145 RVA: 0x003F9CD7 File Offset: 0x003F7ED7
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600529A RID: 21146 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600529B RID: 21147 RVA: 0x003F9CE4 File Offset: 0x003F7EE4
			internal byte[] <PQ26_SpeedLimiter>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}

			// Token: 0x0600529C RID: 21148 RVA: 0x003F9D40 File Offset: 0x003F7F40
			internal byte[] <PQ26_SpeedLimiter>b__0_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 6, 2, false);
				}
				return array;
			}

			// Token: 0x0400327C RID: 12924
			public static readonly Assistance.<>c <>9 = new Assistance.<>c();

			// Token: 0x0400327D RID: 12925
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x0400327E RID: 12926
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_1;
		}
	}
}
