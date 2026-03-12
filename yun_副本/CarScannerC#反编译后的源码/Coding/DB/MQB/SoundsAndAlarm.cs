using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B57 RID: 2903
	internal class SoundsAndAlarm
	{
		// Token: 0x060059AD RID: 22957 RVA: 0x0042C29C File Offset: 0x0042A49C
		public static MQBEasyCodingItem MQB_6D_TailgateSound()
		{
			return new MQBEasyCodingItem(CodingGroup.SoundsAndAlarms, Translate.GetString("codingDB_SoundBeepWhenClosingOpeningTailgateWithElectricActuator_Name"), "By default it's enabled", "0502", "723", "78D", "12345", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x00002050 File Offset: 0x00000250
		public SoundsAndAlarm()
		{
		}

		// Token: 0x02000B58 RID: 2904
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060059AF RID: 22959 RVA: 0x0042C30E File Offset: 0x0042A50E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060059B0 RID: 22960 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060059B1 RID: 22961 RVA: 0x0042C31C File Offset: 0x0042A51C
			internal byte[] <MQB_6D_TailgateSound>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
				}
				return array;
			}

			// Token: 0x04003819 RID: 14361
			public static readonly SoundsAndAlarm.<>c <>9 = new SoundsAndAlarm.<>c();

			// Token: 0x0400381A RID: 14362
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;
		}
	}
}
