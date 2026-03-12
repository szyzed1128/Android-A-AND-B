using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000ACF RID: 2767
	internal static class FeedbackSignals
	{
		// Token: 0x060056E5 RID: 22245 RVA: 0x00417DA8 File Offset: 0x00415FA8
		public static ICodingContainer OpticalFeedbackWhenLocking()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_OpticalFeedbackWhenLocking_Name"), "", "0D0E", 0, 0, "0D0E", 0, 3, Array.Empty<TranslationItem>());
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_Decelerate_Name"), MQBAdaptationTemplate.EnableOption.Value);
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(Translate.GetString("codingDB_Normal_Name"), MQBAdaptationTemplate.DisableOption.Value);
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(mqbadaptationOption);
			mqbalternativeCoding.Options.Add(mqbadaptationOption2);
			return mqbalternativeCoding;
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x00417E38 File Offset: 0x00416038
		public static ICodingContainer AcousticFeedbackWhenUnlocking()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_AcousticFeedbackWhenUnlocking_Name"), "", "0D0E", 0, 4, "0D0E", 0, 0, Array.Empty<TranslationItem>());
		}

		// Token: 0x060056E7 RID: 22247 RVA: 0x00417E70 File Offset: 0x00416070
		public static ICodingContainer AcousticFeedbackGlobal()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_AcousticFeedbackActivationOrDeactivationGlobalSetting_Name"), "", "0D0E", 0, 3, "0D0E", 0, 7, Array.Empty<TranslationItem>());
		}

		// Token: 0x060056E8 RID: 22248 RVA: 0x00417EA8 File Offset: 0x004160A8
		public static ICodingContainer AcousticFeedbackSignalHorn()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_AcousticFeedbackUseSignalHorn_Name"), "", "0D0E", 1, 2, "0D0E", 1, 0, Array.Empty<TranslationItem>());
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x00417EE0 File Offset: 0x004160E0
		public static ICodingContainer AcousticFeedbackLockAcousticFeedback()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_AcousticFeedbackWhenLocking_Name"), "", "0D0E", 0, 6, "0D0E", 0, 1, Array.Empty<TranslationItem>());
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x00417F18 File Offset: 0x00416118
		public static ICodingContainer AcousticFeedbackDurationOfAcousticFeedbackFromSimpleHorn()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_AcousticFeedbackDurationWhenUsingSignalHorn_Name"), "", "0D0E", 0, 6, "0D0E", 0, 1, Array.Empty<TranslationItem>());
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_Short_Name"), MQBAdaptationTemplate.EnableOption.Value);
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(Translate.GetString("codingDB_Normal_Name"), MQBAdaptationTemplate.DisableOption.Value);
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(mqbadaptationOption);
			mqbalternativeCoding.Options.Add(mqbadaptationOption2);
			return mqbalternativeCoding;
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x00417FA8 File Offset: 0x004161A8
		public static ICodingContainer AcousticFeedbackMenu()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_AcousticFeedbackMenuInMmi_Name"), "", "0D0E", 1, 0, "0D0E", 0, 6, Array.Empty<TranslationItem>());
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x00417FE0 File Offset: 0x004161E0
		public static ICodingContainer OpticalFeedback3rdBrakeLight()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_OpticalFeedbackUse3RdBrakeLight_Name"), "", "0D0E", 0, 2, "0D0E", 0, 5, Array.Empty<TranslationItem>());
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x00418018 File Offset: 0x00416218
		public static ICodingContainer OpticalFeedbackComfortClosing()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_OpticalFeedbackWhenComfortClosingWindows_Name"), "", "0D0E", 0, 1, "0D0E", 0, 4, Array.Empty<TranslationItem>());
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x00418050 File Offset: 0x00416250
		public static ICodingContainer AcousticFeedbackForTheSecondCloseCommand()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_AcousticFeedbackWhenSecondCloseCommand_Name"), "Quittierton bei zweitem ZV-ZU-Befehl", "0D0E", 0, 7, "", 9999, 0, Array.Empty<TranslationItem>());
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x0041808C File Offset: 0x0041628C
		public static ICodingContainer MQB_09_SoundConfirmationOfLockAndUnlock()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D0E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, true);
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
					BitHelpers.SwitchBitInByte(array2, 0, 6, true);
					BitHelpers.SwitchBitInByte(array2, 0, 7, true);
					BitHelpers.SwitchBitInByte(array2, 1, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 6, false);
					BitHelpers.SwitchBitInByte(array2, 0, 7, false);
					BitHelpers.SwitchBitInByte(array2, 1, 0, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.FeedbackSignals, Translate.GetString("codingDB_ConfirmationSoundWhenLockingAndUnlockingCentralLock_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x02000AD0 RID: 2768
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060056F0 RID: 22256 RVA: 0x00418124 File Offset: 0x00416324
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060056F1 RID: 22257 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060056F2 RID: 22258 RVA: 0x00418130 File Offset: 0x00416330
			internal byte[] <MQB_09_SoundConfirmationOfLockAndUnlock>b__10_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}

			// Token: 0x060056F3 RID: 22259 RVA: 0x004181C4 File Offset: 0x004163C4
			internal byte[] <MQB_09_SoundConfirmationOfLockAndUnlock>b__10_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
				}
				return array;
			}

			// Token: 0x0400358F RID: 13711
			public static readonly FeedbackSignals.<>c <>9 = new FeedbackSignals.<>c();

			// Token: 0x04003590 RID: 13712
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_0;

			// Token: 0x04003591 RID: 13713
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_1;
		}
	}
}
