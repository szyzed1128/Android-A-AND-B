using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MLB_A4B9
{
	// Token: 0x02000B81 RID: 2945
	internal static class Multimedia
	{
		// Token: 0x06005A58 RID: 23128 RVA: 0x00431BB0 File Offset: 0x0042FDB0
		internal static ICodingContainer UserSelectionScreen_MMI_MIB3()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0B1D", "5F", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 82, 1, true);
					BitHelpers.SwitchBitInByte(array, 82, 0, false);
					BitHelpers.SwitchBitInByte(array, 82, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 82, 1, false);
					BitHelpers.SwitchBitInByte(array, 82, 0, false);
					BitHelpers.SwitchBitInByte(array, 82, 6, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0B1B", "5F", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 67, 7, true);
					BitHelpers.SwitchBitInByte(array2, 67, 6, true);
					BitHelpers.SwitchBitInByte(array2, 67, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 67, 7, false);
					BitHelpers.SwitchBitInByte(array2, 67, 6, false);
					BitHelpers.SwitchBitInByte(array2, 67, 5, false);
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Multimedia, "MMI User profile selection screen display [MIB3]", "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x00431C54 File Offset: 0x0042FE54
		internal static ICodingContainer PrivacyWarningScreen_MMI_MIB3()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0B1D", "5F", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 55, 1, true);
					BitHelpers.SwitchBitInByte(array, 55, 0, false);
					BitHelpers.SwitchBitInByte(array, 55, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 55, 1, false);
					BitHelpers.SwitchBitInByte(array, 55, 0, false);
					BitHelpers.SwitchBitInByte(array, 55, 6, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0B1B", "5F", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 51, 7, true);
					BitHelpers.SwitchBitInByte(array2, 51, 6, true);
					BitHelpers.SwitchBitInByte(array2, 51, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 51, 7, false);
					BitHelpers.SwitchBitInByte(array2, 51, 6, false);
					BitHelpers.SwitchBitInByte(array2, 51, 5, false);
				}
				return array2;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Multimedia, "MMI Online services privacy warning screen display [MIB3]", "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x00431CF8 File Offset: 0x0042FEF8
		public static ICodingContainer MIB2SoundDataset_audio_parameter_sound_0x3000()
		{
			return new MQBAudioDataSetBase("vag.dataset0x3000mlbevo")
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

		// Token: 0x02000B82 RID: 2946
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005A5B RID: 23131 RVA: 0x00431DB0 File Offset: 0x0042FFB0
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005A5C RID: 23132 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005A5D RID: 23133 RVA: 0x00431DBC File Offset: 0x0042FFBC
			internal byte[] <UserSelectionScreen_MMI_MIB3>b__0_0(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 82, 1, true);
					BitHelpers.SwitchBitInByte(array, 82, 0, false);
					BitHelpers.SwitchBitInByte(array, 82, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 82, 1, false);
					BitHelpers.SwitchBitInByte(array, 82, 0, false);
					BitHelpers.SwitchBitInByte(array, 82, 6, false);
				}
				return array;
			}

			// Token: 0x06005A5E RID: 23134 RVA: 0x00431E30 File Offset: 0x00430030
			internal byte[] <UserSelectionScreen_MMI_MIB3>b__0_1(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 67, 7, true);
					BitHelpers.SwitchBitInByte(array, 67, 6, true);
					BitHelpers.SwitchBitInByte(array, 67, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 67, 7, false);
					BitHelpers.SwitchBitInByte(array, 67, 6, false);
					BitHelpers.SwitchBitInByte(array, 67, 5, false);
				}
				return array;
			}

			// Token: 0x06005A5F RID: 23135 RVA: 0x00431EA4 File Offset: 0x004300A4
			internal byte[] <PrivacyWarningScreen_MMI_MIB3>b__1_0(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 55, 1, true);
					BitHelpers.SwitchBitInByte(array, 55, 0, false);
					BitHelpers.SwitchBitInByte(array, 55, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 55, 1, false);
					BitHelpers.SwitchBitInByte(array, 55, 0, false);
					BitHelpers.SwitchBitInByte(array, 55, 6, false);
				}
				return array;
			}

			// Token: 0x06005A60 RID: 23136 RVA: 0x00431F18 File Offset: 0x00430118
			internal byte[] <PrivacyWarningScreen_MMI_MIB3>b__1_1(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 51, 7, true);
					BitHelpers.SwitchBitInByte(array, 51, 6, true);
					BitHelpers.SwitchBitInByte(array, 51, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 51, 7, false);
					BitHelpers.SwitchBitInByte(array, 51, 6, false);
					BitHelpers.SwitchBitInByte(array, 51, 5, false);
				}
				return array;
			}

			// Token: 0x040038BA RID: 14522
			public static readonly Multimedia.<>c <>9 = new Multimedia.<>c();

			// Token: 0x040038BB RID: 14523
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x040038BC RID: 14524
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_1;

			// Token: 0x040038BD RID: 14525
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x040038BE RID: 14526
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_1;
		}
	}
}
