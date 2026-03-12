using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B2F RID: 2863
	internal static class Other
	{
		// Token: 0x060058FD RID: 22781 RVA: 0x004273CC File Offset: 0x004255CC
		public static ICodingContainer CloseWindowsWhenRainy()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A69", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0D", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 0, true);
					BitHelpers.SwitchBitInByte(array2, 2, 1, true);
					BitHelpers.SwitchBitInByte(array2, 3, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 2, 0, false);
					BitHelpers.SwitchBitInByte(array2, 2, 1, false);
					BitHelpers.SwitchBitInByte(array2, 3, 6, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Other, Translate.GetString("codingDB_AutomaticallyCloseWindowsWhenRainy_Name"), Translate.GetString("codingDB_AutomaticallyCloseWindowsWhenRainy_Description"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x060058FE RID: 22782 RVA: 0x00427468 File Offset: 0x00425668
		public static ICodingContainer LightsSwitchWithFogLightsInstalled()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A57", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0600", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 7, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 7, 3, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Other, Translate.GetString("codingDB_LightSwitchWithFogLightsSwitchInstalled_Name"), "", "70E", "778", "", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060058FF RID: 22783 RVA: 0x00427510 File Offset: 0x00425710
		public static ICodingContainer PersonalizationActivation()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("16F0", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0600", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 10, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 10, 0, false);
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0600", "08", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 15, 4, false);
					BitHelpers.SwitchBitInByte(array3, 15, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 15, 4, false);
					BitHelpers.SwitchBitInByte(array3, 15, 3, false);
				}
				return array3;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Other, Translate.GetString("codingDB_PersonalizationActivation_Name"), Translate.GetString("codingDB_PersonalizationActivation_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3 })
			{
				InnerDescription = string.Format(Translate.GetString("codingDB_PersonalizationActivation_InnerDescription"), Translate.GetString("codingDB_PersonalizationPatchForDriverSeatWithMemoryFunction_Name"))
			};
		}

		// Token: 0x06005900 RID: 22784 RVA: 0x00427618 File Offset: 0x00425818
		public static ICodingContainer PersonalizationSeatsPatch()
		{
			return new MQBEasyCodingItem(CodingGroup.Other, Translate.GetString("codingDB_PersonalizationPatchForDriverSeatWithMemoryFunction_Name"), "", "0600", VagUnitHelper.GetRequestHeaderForMQBUnit("36"), VagUnitHelper.GetResponseHeaderForMQBUnit("36"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, false);
					BitHelpers.SwitchBitInByte(array, 9, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, false);
					BitHelpers.SwitchBitInByte(array, 9, 1, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x02000B30 RID: 2864
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005901 RID: 22785 RVA: 0x00427693 File Offset: 0x00425893
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005902 RID: 22786 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005903 RID: 22787 RVA: 0x004276A0 File Offset: 0x004258A0
			internal byte[] <CloseWindowsWhenRainy>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
				}
				return array;
			}

			// Token: 0x06005904 RID: 22788 RVA: 0x00427710 File Offset: 0x00425910
			internal byte[] <CloseWindowsWhenRainy>b__0_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
					BitHelpers.SwitchBitInByte(array, 3, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 3, 6, false);
				}
				return array;
			}

			// Token: 0x06005905 RID: 22789 RVA: 0x00427780 File Offset: 0x00425980
			internal byte[] <LightsSwitchWithFogLightsInstalled>b__1_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				return array;
			}

			// Token: 0x06005906 RID: 22790 RVA: 0x004277CC File Offset: 0x004259CC
			internal byte[] <LightsSwitchWithFogLightsInstalled>b__1_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
				}
				return array;
			}

			// Token: 0x06005907 RID: 22791 RVA: 0x00427818 File Offset: 0x00425A18
			internal byte[] <PersonalizationActivation>b__2_0(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
				}
				return array;
			}

			// Token: 0x06005908 RID: 22792 RVA: 0x00427978 File Offset: 0x00425B78
			internal byte[] <PersonalizationActivation>b__2_1(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 0, false);
				}
				return array;
			}

			// Token: 0x06005909 RID: 22793 RVA: 0x004279C4 File Offset: 0x00425BC4
			internal byte[] <PersonalizationActivation>b__2_2(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 15, 4, false);
					BitHelpers.SwitchBitInByte(array, 15, 3, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 15, 4, false);
					BitHelpers.SwitchBitInByte(array, 15, 3, false);
				}
				return array;
			}

			// Token: 0x0600590A RID: 22794 RVA: 0x00427A24 File Offset: 0x00425C24
			internal byte[] <PersonalizationSeatsPatch>b__3_0(byte[] data, string value, MQBEasyCodingItem coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, false);
					BitHelpers.SwitchBitInByte(array, 9, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 9, 2, false);
					BitHelpers.SwitchBitInByte(array, 9, 1, false);
				}
				return array;
			}

			// Token: 0x0400376E RID: 14190
			public static readonly Other.<>c <>9 = new Other.<>c();

			// Token: 0x0400376F RID: 14191
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x04003770 RID: 14192
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_1;

			// Token: 0x04003771 RID: 14193
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_0;

			// Token: 0x04003772 RID: 14194
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_1;

			// Token: 0x04003773 RID: 14195
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x04003774 RID: 14196
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_1;

			// Token: 0x04003775 RID: 14197
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_2;

			// Token: 0x04003776 RID: 14198
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_0;
		}
	}
}
