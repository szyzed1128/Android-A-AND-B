using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AAB RID: 2731
	internal static class EngineAndPowertrain
	{
		// Token: 0x0600560D RID: 22029 RVA: 0x00410538 File Offset: 0x0040E738
		public static MQBEasyCodingItem MQB_44_TorqueSteeringCompensation()
		{
			MQBAdaptationOption ActiveWithLearnedValue = new MQBAdaptationOption(Translate.GetString("codingDB_ActiveWithLearnedValue_Name"), "04");
			MQBAdaptationOption ActiveWithoutLearnedValue = new MQBAdaptationOption(Translate.GetString("codingDB_ActiveWithoutLearnedValue_Name"), "08");
			return new MQBEasyCodingItem(CodingGroup.EngineAndPowertrain, Translate.GetString("codingDB_TorqueSteeringCompensationTsc_Name"), "", "0600", "712", "77C", "19249", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				else if (value == ActiveWithLearnedValue.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				else if (value == ActiveWithoutLearnedValue.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 3) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[0], 3) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return ActiveWithLearnedValue.Title;
				}
				if (BitHelpers.GetBit_0_7(data[0], 3) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return ActiveWithoutLearnedValue.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация системы компенсации бокового увода (TSC)", "Не проверено!", "")
				}
			};
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x00410600 File Offset: 0x0040E800
		public static ICodingContainer IdleRPMAdaptationDiesel()
		{
			return new MQBAdaptationTemplate(0, "Idle RPM correction (Diesel)", "", "27971", "27971, 12233, 00008, 00009, 00010, 00007, 00001, 00002, 00003, 00004", new TranslationItem[]
			{
				new TranslationItem("ru", "Корректировка оборотов холостого хода (дизель)", "", "Шаг = 1 об/мин, мин = -100, макс = 155")
			}, AdaptationValueTypes.InputValueType, "0310", "7E0", "7E8", 0, 1, 1.0, -100.0, false, false, false, new MQBAdaptationOption[0])
			{
				Group = CodingGroup.EngineAndPowertrain,
				PreWriteCommands = "1040",
				InnerDescription = "Step = 1 rpm, min = -100, max = 155"
			};
		}

		// Token: 0x02000AAC RID: 2732
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x0600560F RID: 22031 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06005610 RID: 22032 RVA: 0x00410698 File Offset: 0x0040E898
			internal byte[] <MQB_44_TorqueSteeringCompensation>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				else if (value == this.ActiveWithLearnedValue.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
				}
				else if (value == this.ActiveWithoutLearnedValue.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
				}
				return array;
			}

			// Token: 0x06005611 RID: 22033 RVA: 0x00410730 File Offset: 0x0040E930
			internal string <MQB_44_TorqueSteeringCompensation>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 3) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return MQBAdaptationTemplate.DisableOption.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[0], 3) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return this.ActiveWithLearnedValue.Title;
				}
				if (BitHelpers.GetBit_0_7(data[0], 3) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return this.ActiveWithoutLearnedValue.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x040034F0 RID: 13552
			public MQBAdaptationOption ActiveWithLearnedValue;

			// Token: 0x040034F1 RID: 13553
			public MQBAdaptationOption ActiveWithoutLearnedValue;
		}
	}
}
