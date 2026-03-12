using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A90 RID: 2704
	internal static class Climate
	{
		// Token: 0x06005580 RID: 21888 RVA: 0x0040BA2C File Offset: 0x00409C2C
		public static MQBEasyCodingItem MQB_08_BlowerDisplayInAuto()
		{
			return new MQBEasyCodingItem(CodingGroup.Climate, Translate.GetString("codingDB_DisplayBlowerFanSpeedInAutoMode_Name"), "", "0600", "746", "7B0", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 11, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 11, 6, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x0040BAA0 File Offset: 0x00409CA0
		public static ICodingContainer MQB_08_AutomaticWheelHeat2021PatchOnly()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "08", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 7, true);
					BitHelpers.SwitchBitInByte(array, 14, 6, false);
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 7, false);
					BitHelpers.SwitchBitInByte(array, 14, 6, true);
				}
				return array;
			}, null);
			mqbeasyCodingItem.PostWriteCommands = "1102";
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("6031", "19", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 7, false);
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 7, true);
				}
				return array2;
			}, null);
			mqbeasyCodingItem2.PostWriteCommands = "1102";
			return new MQBMultipleCoding(CodingGroup.Climate, Translate.GetString("codingDB_AutomaticDriveWheelHeater_Name") + ": " + Translate.GetString("codingDB_AutomaticDriveWheelHeater_2021Patch"), "Experimental function!", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = Translate.GetString("codingDB_AutomaticDriveWheelHeater2021My_Description")
			};
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x0040BB80 File Offset: 0x00409D80
		public static MQBEasyCodingItem MQB_08_AutomaticWheelHeat()
		{
			MQBAdaptationOption option_off = new MQBAdaptationOption(MQBAdaptationTemplate.DisableOption.Title, "00");
			MQBAdaptationOption option_inside_temp = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + Translate.GetString("codingDB_UsingCabinTemperature_Name"), "01");
			MQBAdaptationOption option_outside_temp = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + Translate.GetString("codingDB_UsingOutdoorTemperature_Name"), "02");
			return new MQBEasyCodingItem(CodingGroup.Climate, Translate.GetString("codingDB_AutomaticDriveWheelHeater_Name"), "", "0600", "746", "7B0", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == option_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 2, false);
					BitHelpers.SwitchBitInByte(array, 13, 3, false);
				}
				else if (value == option_inside_temp.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 2, true);
					BitHelpers.SwitchBitInByte(array, 13, 3, false);
				}
				else if (value == option_outside_temp.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 2, false);
					BitHelpers.SwitchBitInByte(array, 13, 3, true);
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[13], 2) && !BitHelpers.GetBit_0_7(data[13], 3))
				{
					return option_inside_temp.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[13], 2) && BitHelpers.GetBit_0_7(data[13], 3))
				{
					return option_outside_temp.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[13], 2) && !BitHelpers.GetBit_0_7(data[13], 3))
				{
					return option_off.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}, new MQBAdaptationOption[] { option_off, option_inside_temp, option_outside_temp });
		}

		// Token: 0x06005583 RID: 21891 RVA: 0x0040BC66 File Offset: 0x00409E66
		internal static ICodingContainer FlapAdaptations()
		{
			return new MQBSimpleOperationWith0102StatusCheck(Translate.GetString("codingDB_VentilationFlapsAdaptation_Name"), "", "", "08", "3101034D040000", "3102034D", "", false)
			{
				Group = CodingGroup.Climate
			};
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x0040BCA0 File Offset: 0x00409EA0
		public static ICodingContainer WindshieldHeaterAutoOnThreshold()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D06", "31347", 8, 1, 0.5, -50.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_WindscreenHeaterAutoThreshold"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x02000A91 RID: 2705
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005585 RID: 21893 RVA: 0x0040BD09 File Offset: 0x00409F09
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005586 RID: 21894 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005587 RID: 21895 RVA: 0x0040BD18 File Offset: 0x00409F18
			internal byte[] <MQB_08_BlowerDisplayInAuto>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 11, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 11, 6, false);
				}
				return array;
			}

			// Token: 0x06005588 RID: 21896 RVA: 0x0040BD64 File Offset: 0x00409F64
			internal byte[] <MQB_08_AutomaticWheelHeat2021PatchOnly>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 7, true);
					BitHelpers.SwitchBitInByte(array, 14, 6, false);
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 7, false);
					BitHelpers.SwitchBitInByte(array, 14, 6, true);
				}
				return array;
			}

			// Token: 0x06005589 RID: 21897 RVA: 0x0040BDD8 File Offset: 0x00409FD8
			internal byte[] <MQB_08_AutomaticWheelHeat2021PatchOnly>b__1_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
				}
				return array;
			}

			// Token: 0x04003482 RID: 13442
			public static readonly Climate.<>c <>9 = new Climate.<>c();

			// Token: 0x04003483 RID: 13443
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x04003484 RID: 13444
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x04003485 RID: 13445
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_1;
		}

		// Token: 0x02000A92 RID: 2706
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600558A RID: 21898 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x0600558B RID: 21899 RVA: 0x0040BE34 File Offset: 0x0040A034
			internal byte[] <MQB_08_AutomaticWheelHeat>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == this.option_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 2, false);
					BitHelpers.SwitchBitInByte(array, 13, 3, false);
				}
				else if (value == this.option_inside_temp.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 2, true);
					BitHelpers.SwitchBitInByte(array, 13, 3, false);
				}
				else if (value == this.option_outside_temp.Value)
				{
					BitHelpers.SwitchBitInByte(array, 13, 2, false);
					BitHelpers.SwitchBitInByte(array, 13, 3, true);
				}
				return array;
			}

			// Token: 0x0600558C RID: 21900 RVA: 0x0040BED0 File Offset: 0x0040A0D0
			internal string <MQB_08_AutomaticWheelHeat>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[13], 2) && !BitHelpers.GetBit_0_7(data[13], 3))
				{
					return this.option_inside_temp.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[13], 2) && BitHelpers.GetBit_0_7(data[13], 3))
				{
					return this.option_outside_temp.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[13], 2) && !BitHelpers.GetBit_0_7(data[13], 3))
				{
					return this.option_off.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x04003486 RID: 13446
			public MQBAdaptationOption option_off;

			// Token: 0x04003487 RID: 13447
			public MQBAdaptationOption option_inside_temp;

			// Token: 0x04003488 RID: 13448
			public MQBAdaptationOption option_outside_temp;
		}
	}
}
