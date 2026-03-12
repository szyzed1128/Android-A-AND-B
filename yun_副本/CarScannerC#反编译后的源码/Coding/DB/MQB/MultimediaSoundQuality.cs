using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B2B RID: 2859
	internal static class MultimediaSoundQuality
	{
		// Token: 0x060058EF RID: 22767 RVA: 0x00426C74 File Offset: 0x00424E74
		public static ICodingContainer MIB2SoundDataset_audio_parameter_sound_0x3000()
		{
			return new MQBAudioDataSetBase("vag.dataset0x3000")
			{
				Address = 12288,
				DataLength = 1330,
				DataLengthFormatLength = 3,
				AddressFormatLength = 3,
				Group = CodingGroup.MultimediaSoundQuality,
				Name = Translate.GetString("codingDB_SoundProcessingPreset_Name") + " (MIB2)",
				Description = Translate.GetString("codingDB_SoundProcessingPreset_Description"),
				InnerDescription = Translate.GetString("codingDB_SoundProcessingPreset_InnerDescription") + "\n" + Translate.GetString("codingDB_SoundProcessingPreset_InnerDescriptionWarningMIB2"),
				ValueType = AdaptationValueTypes.OptionType,
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F")
			};
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x00426D2C File Offset: 0x00424F2C
		public static ICodingContainer MIB1SoundDataset_audio_parameter_sound_0x1000()
		{
			MQBAudioDataSetBase mqbaudioDataSetBase = new MQBAudioDataSetBase("vag.dataset0x1000")
			{
				Address = 4096,
				DataLength = 1404,
				DataLengthFormatLength = 3,
				AddressFormatLength = 3,
				Group = CodingGroup.MultimediaSoundQuality,
				Name = Translate.GetString("codingDB_SoundProcessingPreset_Name") + " (MIB1 High)",
				Description = Translate.GetString("codingDB_SoundProcessingPreset_Description"),
				InnerDescription = Translate.GetString("codingDB_SoundProcessingPreset_InnerDescription") + "\n" + Translate.GetString("codingDB_SoundProcessingPreset_InnerDescriptionWarningMIB2"),
				ValueType = AdaptationValueTypes.OptionType,
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F")
			};
			foreach (MQBAdaptationOption mqbadaptationOption in mqbaudioDataSetBase.Options)
			{
				string text = mqbadaptationOption.Value;
				text = text.Substring(0, text.Length - 4);
				int num = text.LastIndexOf('.');
				string text2 = text.Substring(num + 1);
				mqbadaptationOption.Title = text2;
			}
			return mqbaudioDataSetBase;
		}

		// Token: 0x060058F1 RID: 22769 RVA: 0x00426E50 File Offset: 0x00425050
		public static ICodingContainer MIB2SoundDataset_audio_parameter_individual_sound_processing_0x0700()
		{
			string name = MultimediaSoundQuality.MQB_5F_BetterSoundBolero().Name;
			return new MQBAudioDataSetBase("vag.dataset0x0700")
			{
				Address = 1792,
				DataLength = 1024,
				DataLengthFormatLength = 3,
				AddressFormatLength = 3,
				Group = CodingGroup.MultimediaSoundQuality,
				Name = Translate.GetString("codingDB_SkodaOnlyIndividualSoundProcessingPreset_Name") + " (MIB2 Entry/Swing 2 before 2017 with SW. ver. 6xxx, MIB2 STD)",
				Description = Translate.GetString("codingDB_SkodaOnlyIndividualSoundProcessingPreset_Description"),
				InnerDescription = Translate.GetString("codingDB_SoundProcessingPreset_InnerDescription") + "\n" + Translate.GetString("codingDB_SoundProcessingPreset_InnerDescriptionWarningMIB2"),
				ValueType = AdaptationValueTypes.OptionType,
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F")
			};
		}

		// Token: 0x060058F2 RID: 22770 RVA: 0x00426F14 File Offset: 0x00425114
		public static ICodingContainer MIB2SoundDataset_audio_parameter_individual_sound_processing_0x7100()
		{
			string name = MultimediaSoundQuality.MQB_5F_BetterSoundBolero().Name;
			return new MQBAudioDataSetBase("vag.dataset0x7100")
			{
				Address = 28928,
				DataLength = 2048,
				DataLengthFormatLength = 3,
				AddressFormatLength = 3,
				Group = CodingGroup.MultimediaSoundQuality,
				Name = Translate.GetString("codingDB_SkodaOnlyIndividualSoundProcessingPreset_Name") + " (MIB2 Entry/Swing2 after 2017, GP/Swing3 with SW. ver. 7xxx, 8xxx)",
				Description = Translate.GetString("codingDB_SkodaOnlyIndividualSoundProcessingPreset_Description"),
				InnerDescription = Translate.GetString("codingDB_SoundProcessingPreset_InnerDescription") + "\n" + Translate.GetString("codingDB_SoundProcessingPreset_InnerDescriptionWarningMIB2"),
				ValueType = AdaptationValueTypes.OptionType,
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F")
			};
		}

		// Token: 0x060058F3 RID: 22771 RVA: 0x00426FD8 File Offset: 0x004251D8
		public static ICodingContainer MQB_5F_BetterSoundBolero()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0600", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[11] = 4;
				}
				else
				{
					array[11] = 1;
				}
				return array;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.MultimediaSoundQuality, Translate.GetString("codingDB_ActivateSkodaSkodaSuroundVirtualSubwooferNotCompatibleWithCanton_Name"), Translate.GetString("codingDB_ActivateSkodaSkodaSuroundVirtualSubwooferNotCompatibleWithCanton_Description"), "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB })
			{
				InnerDescription = Translate.GetString("codingDB_ActivateSkodaSkodaSuroundVirtualSubwooferNotCompatibleWithCanton_InnerDescription")
			};
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x00427054 File Offset: 0x00425254
		public static ICodingContainer MQB_47_AcousticSystemCoding()
		{
			MQBAdaptationOption[] options = new MQBAdaptationOption[]
			{
				new MQBAdaptationOption("Audi/Default", "01"),
				new MQBAdaptationOption("Audi/Dynaudio", "11"),
				new MQBAdaptationOption("Audi/Fender", "21"),
				new MQBAdaptationOption("Audi/No brand sound", "31"),
				new MQBAdaptationOption("Volkswagen/Default", "02"),
				new MQBAdaptationOption("Volkswagen/Dynaudio", "12"),
				new MQBAdaptationOption("Volkswagen/Fender", "22"),
				new MQBAdaptationOption("Volkswagen/No brand sound", "32"),
				new MQBAdaptationOption("Skoda/Default", "03"),
				new MQBAdaptationOption("Skoda/Dynaudio", "13"),
				new MQBAdaptationOption("Skoda/Fender", "23"),
				new MQBAdaptationOption("Skoda/No brand sound", "33"),
				new MQBAdaptationOption("Seat/Default", "04"),
				new MQBAdaptationOption("Seat/Dynaudio", "14"),
				new MQBAdaptationOption("Seat/Fender", "24"),
				new MQBAdaptationOption("Seat/No brand sound", "34"),
				new MQBAdaptationOption("Bentley/Default", "05"),
				new MQBAdaptationOption("Bentley/Dynaudio", "15"),
				new MQBAdaptationOption("Bentley/Fender", "25"),
				new MQBAdaptationOption("Bentley/No brand sound", "35"),
				new MQBAdaptationOption("Porsche/Default", "07"),
				new MQBAdaptationOption("Porsche/Dynaudio", "17"),
				new MQBAdaptationOption("Porsche/Fender", "27"),
				new MQBAdaptationOption("Porsche/No brand sound", "37"),
				new MQBAdaptationOption("Lamborghini/Default", "08"),
				new MQBAdaptationOption("Lamborghini/Dynaudio", "18"),
				new MQBAdaptationOption("Lamborghini/Fender", "28"),
				new MQBAdaptationOption("Lamborghini/No brand sound", "38")
			};
			return new MQBEasyCodingItem(CodingGroup.MultimediaSoundQuality, Translate.GetString("codingDB_ChangeAcousticSystemCodingPresetMib2_Name"), Translate.GetString("codingDB_ChangeAcousticSystemCodingPresetMib2_Description"), "0600", "76F", "7D9", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = byte.Parse(value, NumberStyles.HexNumber);
				array[0] = b;
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				string hex = data[0].ToString("X2");
				MQBAdaptationOption mqbadaptationOption = options.FirstOrDefault((MQBAdaptationOption x) => x.Value == hex);
				if (mqbadaptationOption != null)
				{
					return mqbadaptationOption.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}, options);
		}

		// Token: 0x02000B2C RID: 2860
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060058F5 RID: 22773 RVA: 0x004272E4 File Offset: 0x004254E4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060058F6 RID: 22774 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060058F7 RID: 22775 RVA: 0x004272F0 File Offset: 0x004254F0
			internal byte[] <MQB_5F_BetterSoundBolero>b__4_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[11] = 4;
				}
				else
				{
					array[11] = 1;
				}
				return array;
			}

			// Token: 0x060058F8 RID: 22776 RVA: 0x00427334 File Offset: 0x00425534
			internal byte[] <MQB_47_AcousticSystemCoding>b__5_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = byte.Parse(value, NumberStyles.HexNumber);
				array[0] = b;
				return array;
			}

			// Token: 0x04003769 RID: 14185
			public static readonly MultimediaSoundQuality.<>c <>9 = new MultimediaSoundQuality.<>c();

			// Token: 0x0400376A RID: 14186
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_0;

			// Token: 0x0400376B RID: 14187
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_0;
		}

		// Token: 0x02000B2D RID: 2861
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x060058F9 RID: 22777 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x060058FA RID: 22778 RVA: 0x00427368 File Offset: 0x00425568
			internal string <MQB_47_AcousticSystemCoding>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				MultimediaSoundQuality.<>c__DisplayClass5_1 CS$<>8__locals1 = new MultimediaSoundQuality.<>c__DisplayClass5_1();
				CS$<>8__locals1.hex = data[0].ToString("X2");
				MQBAdaptationOption mqbadaptationOption = this.options.FirstOrDefault((MQBAdaptationOption x) => x.Value == CS$<>8__locals1.hex);
				if (mqbadaptationOption != null)
				{
					return mqbadaptationOption.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x0400376C RID: 14188
			public MQBAdaptationOption[] options;
		}

		// Token: 0x02000B2E RID: 2862
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_1
		{
			// Token: 0x060058FB RID: 22779 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_1()
			{
			}

			// Token: 0x060058FC RID: 22780 RVA: 0x004273B9 File Offset: 0x004255B9
			internal bool <MQB_47_AcousticSystemCoding>b__2(MQBAdaptationOption x)
			{
				return x.Value == this.hex;
			}

			// Token: 0x0400376D RID: 14189
			public string hex;
		}
	}
}
