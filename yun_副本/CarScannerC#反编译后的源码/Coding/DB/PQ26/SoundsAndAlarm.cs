using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A67 RID: 2663
	internal static class SoundsAndAlarm
	{
		// Token: 0x0600542B RID: 21547 RVA: 0x0040165C File Offset: 0x003FF85C
		public static ICodingContainer PQ26_SignalWhenIgnitionOff()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A55", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D10", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
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
			return new MQBAlternativeCoding(CodingGroup.SoundsAndAlarms, "Horn working when ignition turned off", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Работа сигнала при выключенном зажигании", "PQ26: Skoda Rapid", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x0040171C File Offset: 0x003FF91C
		public static ICodingContainer ActivateAntiTheftAlarm()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D07", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D07", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 7, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("09", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit3 = new MQBAlternativeContainerForUnit09(false, "0600", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding3)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 12, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 12, 0, false);
				}
				return array3;
			}, null);
			return new MQBMultipleCoding(CodingGroup.SoundsAndAlarms, "Activate anti-theft alarm system", "", false, new ICodingContainer[]
			{
				mqbalternativeCoding,
				new MQBAlternativeCoding("09", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit3 })
				{
					CodingResultWhenNoSuitableAlternativeFound = CodingRequestResult.Success
				}
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация заводской охранной системы", "", "")
				}
			};
		}

		// Token: 0x02000A68 RID: 2664
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600542D RID: 21549 RVA: 0x00401838 File Offset: 0x003FFA38
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600542E RID: 21550 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600542F RID: 21551 RVA: 0x00401844 File Offset: 0x003FFA44
			internal byte[] <PQ26_SignalWhenIgnitionOff>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				return array;
			}

			// Token: 0x06005430 RID: 21552 RVA: 0x00401890 File Offset: 0x003FFA90
			internal byte[] <PQ26_SignalWhenIgnitionOff>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005431 RID: 21553 RVA: 0x004018DC File Offset: 0x003FFADC
			internal byte[] <ActivateAntiTheftAlarm>b__1_0(byte[] data, string value, MQBAlternativeCoding coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}

			// Token: 0x06005432 RID: 21554 RVA: 0x00401938 File Offset: 0x003FFB38
			internal byte[] <ActivateAntiTheftAlarm>b__1_1(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				return array;
			}

			// Token: 0x06005433 RID: 21555 RVA: 0x00401984 File Offset: 0x003FFB84
			internal byte[] <ActivateAntiTheftAlarm>b__1_2(byte[] data, string value, MQBAlternativeCoding coding3)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 12, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 12, 0, false);
				}
				return array;
			}

			// Token: 0x0400336C RID: 13164
			public static readonly SoundsAndAlarm.<>c <>9 = new SoundsAndAlarm.<>c();

			// Token: 0x0400336D RID: 13165
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x0400336E RID: 13166
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x0400336F RID: 13167
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_0;

			// Token: 0x04003370 RID: 13168
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_1;

			// Token: 0x04003371 RID: 13169
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_2;
		}
	}
}
