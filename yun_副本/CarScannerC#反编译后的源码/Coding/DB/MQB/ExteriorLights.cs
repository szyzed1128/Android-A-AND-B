using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.PQ26;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AB2 RID: 2738
	internal static class ExteriorLights
	{
		// Token: 0x06005625 RID: 22053 RVA: 0x00411488 File Offset: 0x0040F688
		public static ICodingContainer MQB_09_RearLightsWhenDayLights()
		{
			string DRL_FUNC = "14";
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0567", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (mqb_LightConfiguration.FunctionC.Value == "00" || mqb_LightConfiguration.FunctionC.Value == DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration.DimmwertCD = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionD.Value == "00" || mqb_LightConfiguration.FunctionD.Value == DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration.DimmwertCD = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionE.Value == "00" || mqb_LightConfiguration.FunctionE.Value == DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration.DimmwertEF = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionF.Value == "00" || mqb_LightConfiguration.FunctionF.Value == DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionF = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration.DimmwertEF = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionG.Value == "00" || mqb_LightConfiguration.FunctionG.Value == DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration.DimmwertGH = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionH.Value == "00" || mqb_LightConfiguration.FunctionH.Value == DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionH = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration.DimmwertGH = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
				}
				else if (mqb_LightConfiguration.FunctionC.Value == DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionD.Value == "00")
					{
						mqb_LightConfiguration.DimmwertCD = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionD.Value == DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionD = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionC.Value == "00")
					{
						mqb_LightConfiguration.DimmwertCD = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionE.Value == DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionF.Value == "00")
					{
						mqb_LightConfiguration.DimmwertEF = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionF.Value == DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionF = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionE.Value == "00")
					{
						mqb_LightConfiguration.DimmwertEF = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionG.Value == DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionH.Value == "00")
					{
						mqb_LightConfiguration.DimmwertGH = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionH.Value == DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionH = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionG.Value == "00")
					{
						mqb_LightConfiguration.DimmwertGH = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(data);
				if (mqb_LightConfiguration2.FunctionC.Value == DRL_FUNC || mqb_LightConfiguration2.FunctionD.Value == DRL_FUNC || mqb_LightConfiguration2.FunctionE.Value == DRL_FUNC || mqb_LightConfiguration2.FunctionF.Value == DRL_FUNC || mqb_LightConfiguration2.FunctionG.Value == DRL_FUNC || mqb_LightConfiguration2.FunctionH.Value == DRL_FUNC)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0568", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration3 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (mqb_LightConfiguration3.FunctionC.Value == "00" || mqb_LightConfiguration3.FunctionC.Value == DRL_FUNC)
					{
						mqb_LightConfiguration3.FunctionC = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration3.DimmwertCD = 70;
						array2 = mqb_LightConfiguration3.ApplyToData();
					}
					else if (mqb_LightConfiguration3.FunctionD.Value == "00" || mqb_LightConfiguration3.FunctionD.Value == DRL_FUNC)
					{
						mqb_LightConfiguration3.FunctionC = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration3.DimmwertCD = 70;
						array2 = mqb_LightConfiguration3.ApplyToData();
					}
					else if (mqb_LightConfiguration3.FunctionE.Value == "00" || mqb_LightConfiguration3.FunctionE.Value == DRL_FUNC)
					{
						mqb_LightConfiguration3.FunctionE = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration3.DimmwertEF = 70;
						array2 = mqb_LightConfiguration3.ApplyToData();
					}
					else if (mqb_LightConfiguration3.FunctionF.Value == "00" || mqb_LightConfiguration3.FunctionF.Value == DRL_FUNC)
					{
						mqb_LightConfiguration3.FunctionF = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration3.DimmwertEF = 70;
						array2 = mqb_LightConfiguration3.ApplyToData();
					}
					else if (mqb_LightConfiguration3.FunctionG.Value == "00" || mqb_LightConfiguration3.FunctionG.Value == DRL_FUNC)
					{
						mqb_LightConfiguration3.FunctionG = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration3.DimmwertGH = 70;
						array2 = mqb_LightConfiguration3.ApplyToData();
					}
					else if (mqb_LightConfiguration3.FunctionH.Value == "00" || mqb_LightConfiguration3.FunctionH.Value == DRL_FUNC)
					{
						mqb_LightConfiguration3.FunctionH = MQB_LightFunction.FromValue(DRL_FUNC);
						mqb_LightConfiguration3.DimmwertGH = 70;
						array2 = mqb_LightConfiguration3.ApplyToData();
					}
				}
				else if (mqb_LightConfiguration3.FunctionC.Value == DRL_FUNC)
				{
					mqb_LightConfiguration3.FunctionC = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration3.FunctionD.Value == "00")
					{
						mqb_LightConfiguration3.DimmwertCD = 0;
					}
					array2 = mqb_LightConfiguration3.ApplyToData();
				}
				else if (mqb_LightConfiguration3.FunctionD.Value == DRL_FUNC)
				{
					mqb_LightConfiguration3.FunctionD = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration3.FunctionC.Value == "00")
					{
						mqb_LightConfiguration3.DimmwertCD = 0;
					}
					array2 = mqb_LightConfiguration3.ApplyToData();
				}
				else if (mqb_LightConfiguration3.FunctionE.Value == DRL_FUNC)
				{
					mqb_LightConfiguration3.FunctionE = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration3.FunctionF.Value == "00")
					{
						mqb_LightConfiguration3.DimmwertEF = 0;
					}
					array2 = mqb_LightConfiguration3.ApplyToData();
				}
				else if (mqb_LightConfiguration3.FunctionF.Value == DRL_FUNC)
				{
					mqb_LightConfiguration3.FunctionF = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration3.FunctionE.Value == "00")
					{
						mqb_LightConfiguration3.DimmwertEF = 0;
					}
					array2 = mqb_LightConfiguration3.ApplyToData();
				}
				else if (mqb_LightConfiguration3.FunctionG.Value == DRL_FUNC)
				{
					mqb_LightConfiguration3.FunctionG = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration3.FunctionH.Value == "00")
					{
						mqb_LightConfiguration3.DimmwertGH = 0;
					}
					array2 = mqb_LightConfiguration3.ApplyToData();
				}
				else if (mqb_LightConfiguration3.FunctionH.Value == DRL_FUNC)
				{
					mqb_LightConfiguration3.FunctionH = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration3.FunctionG.Value == "00")
					{
						mqb_LightConfiguration3.DimmwertGH = 0;
					}
					array2 = mqb_LightConfiguration3.ApplyToData();
				}
				return array2;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				MQB_LightConfiguration mqb_LightConfiguration4 = new MQB_LightConfiguration(data);
				if (mqb_LightConfiguration4.FunctionC.Value == DRL_FUNC || mqb_LightConfiguration4.FunctionD.Value == DRL_FUNC || mqb_LightConfiguration4.FunctionE.Value == DRL_FUNC || mqb_LightConfiguration4.FunctionF.Value == DRL_FUNC || mqb_LightConfiguration4.FunctionG.Value == DRL_FUNC || mqb_LightConfiguration4.FunctionH.Value == DRL_FUNC)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearSideLightsTurnOnWhenDaytimeRunningLightsAreActiveScandiavianDrls_Name") + " (Skoda Kodiaq, etc.)", "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x00411584 File Offset: 0x0040F784
		public static ICodingContainer MQB_09_CornerActivation_NOT_LED()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "055C", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
					mqb_LightConfiguration.LoadFromData(array);
					mqb_LightConfiguration.FunctionB = MQB_LightFunction.FromValue("16");
					array = mqb_LightConfiguration.ApplyToData();
				}
				else
				{
					MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration();
					mqb_LightConfiguration2.LoadFromData(array);
					mqb_LightConfiguration2.FunctionB = MQB_LightFunction.FromValue("00");
					array = mqb_LightConfiguration2.ApplyToData();
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				MQB_LightConfiguration mqb_LightConfiguration3 = new MQB_LightConfiguration();
				mqb_LightConfiguration3.LoadFromData(data);
				if (mqb_LightConfiguration3.FunctionB.Value == "16")
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "055D", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					MQB_LightConfiguration mqb_LightConfiguration4 = new MQB_LightConfiguration();
					mqb_LightConfiguration4.LoadFromData(array2);
					mqb_LightConfiguration4.FunctionB = MQB_LightFunction.FromValue("17");
					array2 = mqb_LightConfiguration4.ApplyToData();
				}
				else
				{
					MQB_LightConfiguration mqb_LightConfiguration5 = new MQB_LightConfiguration();
					mqb_LightConfiguration5.LoadFromData(array2);
					mqb_LightConfiguration5.FunctionB = MQB_LightFunction.FromValue("00");
					array2 = mqb_LightConfiguration5.ApplyToData();
				}
				return array2;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				MQB_LightConfiguration mqb_LightConfiguration6 = new MQB_LightConfiguration();
				mqb_LightConfiguration6.LoadFromData(data);
				if (mqb_LightConfiguration6.FunctionB.Value == "17")
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D1D", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[5] = 128;
					array3[8] = 100;
					BitHelpers.SwitchBitInByte(array3, 0, 0, true);
					array3[2] = 0;
					array3[9] = 200;
					array3[10] = 190;
					array3[7] = 120;
					array3[11] = 160;
					array3[6] = 80;
					array3[3] = 80;
					BitHelpers.SwitchBitInByte(array3, 1, 0, false);
					BitHelpers.SwitchBitInByte(array3, 1, 1, true);
					BitHelpers.SwitchBitInByte(array3, 1, 2, false);
					array3[0] = 31;
				}
				return array3;
			}, (byte[] data, MQBAlternativeCoding coding2) => MQBAdaptationTemplate.EnableOption.Title);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D1D", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[6] = 127;
					array4[9] = 100;
					BitHelpers.SwitchBitInByte(array4, 0, 1, true);
					array4[3] = 0;
					array4[10] = 200;
					array4[11] = 190;
					array4[8] = 120;
					array4[12] = 160;
					array4[7] = 80;
					array4[4] = 80;
					BitHelpers.SwitchBitInByte(array4, 2, 3, false);
					BitHelpers.SwitchBitInByte(array4, 2, 4, true);
					BitHelpers.SwitchBitInByte(array4, 2, 5, false);
				}
				return array4;
			}, (byte[] data, MQBAlternativeCoding coding2) => MQBAdaptationTemplate.EnableOption.Title);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_CornerFunctionActivation_Name"), Translate.GetString("codingDB_CornerFunctionActivation_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbalternativeCoding })
			{
				InnerDescription = Translate.GetString("codingDB_CornerFunctionActivation_InnerDescription")
			};
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x00411790 File Offset: 0x0040F990
		public static ICodingContainer MQB_09_FogLightWithHighBeam_LED_Only()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0552", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 14;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 14)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0554", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[8] = 14;
				}
				else
				{
					array2[8] = 0;
				}
				return array2;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 14)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0553", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[8] = 13;
				}
				else
				{
					array3[8] = 0;
				}
				return array3;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 13)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0555", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[8] = 13;
				}
				else
				{
					array4[8] = 0;
				}
				return array4;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 13)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_FrontFogLightsActivateWithHighBeamSkodaKodiaqKaroqWithLed_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x004119C4 File Offset: 0x0040FBC4
		public static ICodingContainer MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0552", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 15;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 15)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0554", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[8] = 15;
				}
				else
				{
					array2[8] = 0;
				}
				return array2;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 15)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0553", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[8] = 15;
				}
				else
				{
					array3[8] = 0;
				}
				return array3;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 15)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0555", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[8] = 15;
				}
				else
				{
					array4[8] = 0;
				}
				return array4;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 15)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_FrontFogLightsBlinkWithHighBeamBlinkSkodaKodiaqKaroqWithLed_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x00411BF8 File Offset: 0x0040FDF8
		public static ICodingContainer MQB_09_FogLightOnAndBlinkWithHighBeam_LED_Only()
		{
			MQB_LightFunction FUNC_FERNLICHT_LINKS = MQB_LightFunction.FromValue("0D");
			MQB_LightFunction FUNC_FERNLICHT_RECHT = MQB_LightFunction.FromValue("0E");
			MQB_LightFunction FUNC_LICHTUPE_GENEREL = MQB_LightFunction.FromValue("0F");
			MQB_LightFunction FUNC_OFF = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0552", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.DimmwertEF = 100;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionD = FUNC_FERNLICHT_RECHT;
					mqb_LightConfiguration.FunctionE = FUNC_LICHTUPE_GENEREL;
				}
				else
				{
					mqb_LightConfiguration.FunctionD = FUNC_OFF;
					mqb_LightConfiguration.FunctionE = FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0553", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.DimmwertEF = 100;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration2.FunctionD = FUNC_FERNLICHT_LINKS;
					mqb_LightConfiguration2.FunctionE = FUNC_LICHTUPE_GENEREL;
				}
				else
				{
					mqb_LightConfiguration2.FunctionD = FUNC_OFF;
					mqb_LightConfiguration2.FunctionE = FUNC_OFF;
					mqb_LightConfiguration2.DimmwertEF = 0;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0554", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				MQB_LightConfiguration mqb_LightConfiguration3 = new MQB_LightConfiguration(array3);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration3.DimmwertEF = 127;
					mqb_LightConfiguration3.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration3.FunctionD = FUNC_FERNLICHT_RECHT;
					mqb_LightConfiguration3.FunctionE = FUNC_LICHTUPE_GENEREL;
				}
				else
				{
					mqb_LightConfiguration3.FunctionD = FUNC_OFF;
					mqb_LightConfiguration3.FunctionE = FUNC_OFF;
					mqb_LightConfiguration3.DimmwertEF = 0;
					mqb_LightConfiguration3.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration3.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0555", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				MQB_LightConfiguration mqb_LightConfiguration4 = new MQB_LightConfiguration(array4);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration4.DimmwertEF = 127;
					mqb_LightConfiguration4.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration4.FunctionD = FUNC_FERNLICHT_LINKS;
					mqb_LightConfiguration4.FunctionE = FUNC_LICHTUPE_GENEREL;
				}
				else
				{
					mqb_LightConfiguration4.FunctionD = FUNC_OFF;
					mqb_LightConfiguration4.FunctionE = FUNC_OFF;
					mqb_LightConfiguration4.DimmwertEF = 0;
					mqb_LightConfiguration4.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration4.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_FrontFogLightsActivateWithHighBeamBlinkFogLightsWhenHighBeamBlinksSkodaKodiaqKaroqWithLed_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4 });
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x00411D18 File Offset: 0x0040FF18
		public static ICodingContainer BlinkRearTurnLightsWithRearLights()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0567", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 15, 7, true);
					array[13] = 2;
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 15, 7, false);
					array[13] = 0;
				}
				return array;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if ((data[15] & 128) == 128 && data[13] == 2)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0568", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 15, 7, true);
					array2[13] = 4;
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 15, 7, false);
					array2[13] = 0;
				}
				return array2;
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if ((data[15] & 128) == 128 && data[13] == 4)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearSideLightsWinkWithRearTurnLights_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x00411E48 File Offset: 0x00410048
		public static ICodingContainer TurnLightsPoliteBlinks()
		{
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption("2", "2");
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption("3", "3");
			MQBAdaptationOption mqbadaptationOption3 = new MQBAdaptationOption("4", "4");
			MQBAdaptationOption mqbadaptationOption4 = new MQBAdaptationOption("5", "5");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A5E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (!(value == "2"))
				{
					if (!(value == "3"))
					{
						if (!(value == "4"))
						{
							if (value == "5")
							{
								BitHelpers.SwitchBitInByte(array, 0, 1, true);
								BitHelpers.SwitchBitInByte(array, 0, 2, true);
							}
						}
						else
						{
							BitHelpers.SwitchBitInByte(array, 0, 1, false);
							BitHelpers.SwitchBitInByte(array, 0, 2, true);
						}
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 1, true);
						BitHelpers.SwitchBitInByte(array, 0, 2, false);
					}
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				BitHelpers.SwitchBitInByte(array, 0, 4, true);
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "2";
				}
				if (BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "3";
				}
				if (!BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "4";
				}
				if (BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "5";
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D00", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (!(value == "2"))
				{
					if (!(value == "3"))
					{
						if (!(value == "4"))
						{
							if (value == "5")
							{
								BitHelpers.SwitchBitInByte(array2, 0, 1, true);
								BitHelpers.SwitchBitInByte(array2, 0, 2, true);
							}
						}
						else
						{
							BitHelpers.SwitchBitInByte(array2, 0, 1, false);
							BitHelpers.SwitchBitInByte(array2, 0, 2, true);
						}
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 0, 1, true);
						BitHelpers.SwitchBitInByte(array2, 0, 2, false);
					}
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
				}
				BitHelpers.SwitchBitInByte(array2, 0, 7, true);
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "2";
				}
				if (BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "3";
				}
				if (!BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "4";
				}
				if (BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "5";
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			});
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_TurnLightsPoliteBlinksCount_Name"), Translate.GetString("codingDB_TurnLightsPoliteBlinksCount_Description"), "70E", "778", "31347", new MQBAdaptationOption[] { mqbadaptationOption, mqbadaptationOption2, mqbadaptationOption3, mqbadaptationOption4 })
			{
				InnerDescription = Translate.GetString("codingDB_TurnLightsPoliteBlinksCount_InnerDescription"),
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 },
				RequiresPro = true
			};
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x00411F9C File Offset: 0x0041019C
		public static ICodingContainer TurnOffDRLWhenParkingBrakeOn()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A58", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 5, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D02", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 5, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_DrlTurnOffDrlWhenParkingBrakeActivated_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x00412034 File Offset: 0x00410234
		public static ICodingContainer TurnOffDRLWhenLightSwitchOff()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A58", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D02", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 2, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_DrlTurnOffWhenLightSwitchInOffPosition_Name"), "DRL = Daytime running lights", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x004120CC File Offset: 0x004102CC
		public static ICodingContainer DRLSteupInMMI()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A58", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D02", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_DrlDrlMenuItemInMultimediaSystemMmi_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x0041216C File Offset: 0x0041036C
		public static ICodingContainer MirrorLightsWithAreaView()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0A57", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0600", "769", "7D3", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 8, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 8, 2, false);
				}
				return array2;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_SideMirrorLightsTurnOnWith360CameraAreaview_Name"), Translate.GetString("codingDB_SideMirrorLightsTurnOnWith360CameraAreaview_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x00412264 File Offset: 0x00410464
		public static ICodingContainer SkodaKodiaqDRLLigthsWithStripes()
		{
			MQB_LightFunction func_Tagfahrlicht = MQB_LightFunction.FromValue("14");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0560", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = func_Tagfahrlicht;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = off_func;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0561", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionC = func_Tagfahrlicht;
					mqb_LightConfiguration2.DimmwertCD = 127;
					mqb_LightConfiguration2.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionC = off_func;
					mqb_LightConfiguration2.DimmwertCD = 0;
					mqb_LightConfiguration2.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_StripesEyelashesInHeadlightsTurnOnWithDrlLightsSkodaKodiaqKaroq_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005631 RID: 22065 RVA: 0x00412358 File Offset: 0x00410558
		public static ICodingContainer MQB_09_Kodiaq_RearSideLightAudiStyle()
		{
			MQB_LightFunction left_blink_func = MQB_LightFunction.FromValue("3C");
			MQB_LightFunction right_blink_func = MQB_LightFunction.FromValue("3D");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0567", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = left_blink_func;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration.DimmwertAB = 35;
					mqb_LightConfiguration.DimmwertEF = 5;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.DimmwertEF = 0;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0568", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionE = right_blink_func;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration2.DimmwertAB = 35;
					mqb_LightConfiguration2.DimmwertEF = 5;
				}
				else
				{
					mqb_LightConfiguration2.FunctionE = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration2.DimmwertAB = 127;
					mqb_LightConfiguration2.DimmwertEF = 0;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearSideLightsAudiStyleCompatibilitySkodaKodiaq_Name"), Translate.GetString("codingDB_RearSideLightsAudiStyleCompatibilitySkodaKodiaq_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x00412450 File Offset: 0x00410650
		public static ICodingContainer BlinkFrontTurnLightsWithDRL()
		{
			MQB_LightFunction LEFT_FUNC = MQB_LightFunction.FromValue("02");
			MQB_LightFunction RIGHT_FUNC = MQB_LightFunction.FromValue("04");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0552", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = LEFT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0553", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionG = RIGHT_FUNC;
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionG = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_DrlWinkWithTurnLights_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = Translate.GetString("codingDB_DrlWinkWithTurnLights_InnerDescription")
			};
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x00412554 File Offset: 0x00410754
		public static ICodingContainer DimmDRLWhenFrontTurnLight_AudiStyle()
		{
			MQB_LightFunction LEFT_FUNC = MQB_LightFunction.FromValue("06");
			MQB_LightFunction RIGHT_FUNC = MQB_LightFunction.FromValue("07");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0552", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = LEFT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 35;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0553", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionG = RIGHT_FUNC;
					mqb_LightConfiguration2.DimmwertGH = 35;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionG = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_DrlDimmingFrontDaytimeRunningLightsWhenTurnLightsActiveAudiStyle_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x00412648 File Offset: 0x00410848
		public static ICodingContainer CornerUpperSpeedThreshold()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D1D", "31347", 3, 1, 0.5, 0.0, false, false);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D1D", "31347", 4, 1, 0.5, 0.0, false, false);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_CornerFunctionUpperSpeedThreshold_Name"), Translate.GetString("codingDB_CornerFunctionUpperSpeedThreshold_Description"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				ValueType = AdaptationValueTypes.InputValueType
			};
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x004126E4 File Offset: 0x004108E4
		public static ICodingContainer CornerRegulation()
		{
			MQBAdaptationOption off = new MQBAdaptationOption(MQBAdaptationTemplate.DisableOption.Title, "000");
			MQBAdaptationOption ece_r48 = new MQBAdaptationOption("ECE R48", "001");
			MQBAdaptationOption ece_r119 = new MQBAdaptationOption("ECE R119", "010");
			MQBAdaptationOption fmvss_517_108 = new MQBAdaptationOption("FMVSS 517 108", "011");
			MQBAdaptationOption sae_j582 = new MQBAdaptationOption("SAE J582", "100");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D1D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == ece_r48.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == ece_r119.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == ece_r119.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == sae_j582.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				string text = Convert.ToString((int)(data[1] & 7), 2);
				while (text.Length < 3)
				{
					text = "0" + text;
				}
				if (text.EndsWith(off.Value))
				{
					return off.Title;
				}
				if (text.EndsWith(ece_r48.Value))
				{
					return ece_r48.Title;
				}
				if (text.EndsWith(ece_r119.Value))
				{
					return ece_r119.Title;
				}
				if (text.EndsWith(fmvss_517_108.Value))
				{
					return fmvss_517_108.Title;
				}
				if (text.EndsWith(sae_j582.Value))
				{
					return sae_j582.Title;
				}
				return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D1D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == off.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 3, false);
					BitHelpers.SwitchBitInByte(array2, 2, 4, false);
					BitHelpers.SwitchBitInByte(array2, 2, 5, false);
				}
				else if (value == ece_r48.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 3, true);
					BitHelpers.SwitchBitInByte(array2, 2, 4, false);
					BitHelpers.SwitchBitInByte(array2, 2, 5, false);
				}
				else if (value == ece_r119.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 3, false);
					BitHelpers.SwitchBitInByte(array2, 2, 4, true);
					BitHelpers.SwitchBitInByte(array2, 2, 5, false);
				}
				else if (value == ece_r119.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 3, true);
					BitHelpers.SwitchBitInByte(array2, 2, 4, true);
					BitHelpers.SwitchBitInByte(array2, 2, 5, false);
				}
				else if (value == sae_j582.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 3, false);
					BitHelpers.SwitchBitInByte(array2, 2, 4, false);
					BitHelpers.SwitchBitInByte(array2, 2, 5, true);
				}
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				string text2 = Convert.ToString((data[2] >> 3) & 7, 2);
				while (text2.Length < 3)
				{
					text2 = "0" + text2;
				}
				if (text2.EndsWith(off.Value))
				{
					return off.Title;
				}
				if (text2.EndsWith(ece_r48.Value))
				{
					return ece_r48.Title;
				}
				if (text2.EndsWith(ece_r119.Value))
				{
					return ece_r119.Title;
				}
				if (text2.EndsWith(fmvss_517_108.Value))
				{
					return fmvss_517_108.Title;
				}
				if (text2.EndsWith(sae_j582.Value))
				{
					return sae_j582.Title;
				}
				return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
			});
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_CornerFunctionRegulation_Name"), Translate.GetString("codingDB_CornerFunctionRegulation_Description"), VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAdaptationOption[] { off, ece_r48, ece_r119, fmvss_517_108, sae_j582 })
			{
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 }
			};
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x00412838 File Offset: 0x00410A38
		public static ICodingContainer MQB_09_LightSwitchWithAutoMode()
		{
			return new MQBEasyCodingItem(CodingGroup.ExteriorLights, Translate.GetString("codingDB_LightSwitchWithAutoInstalled_Name"), "", "0A57", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, false);
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x004128AC File Offset: 0x00410AAC
		public static ICodingContainer OctaviaA7_SecondsRearFogLight()
		{
			MQB_LightFunction rear_fog_func = MQB_LightFunction.FromValue("1B");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "056A", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionA = rear_fog_func;
					mqb_LightConfiguration.DimmwertAB = 100;
					if (mqb_LightConfiguration.LampType.Value == "00")
					{
						mqb_LightConfiguration.LampType = MQB_LampType.FromValue(9);
					}
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "056B", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionA = rear_fog_func;
					mqb_LightConfiguration2.DimmwertAB = 100;
					if (mqb_LightConfiguration2.LampType.Value == "00")
					{
						mqb_LightConfiguration2.LampType = MQB_LampType.FromValue(9);
					}
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearFogLightsActivateBothRearFogLightOnlySkodaOctaviaA7_Name"), "", true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x00412990 File Offset: 0x00410B90
		public static ICodingContainer EmergencyBrakingLightsWithTurnSignals()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.ExteriorLights, Translate.GetString("codingDB_EmergencyBrakingTurnOnHazardLights_Name"), "", "0A5E", 2, 6, "0D21", 1, 6, Array.Empty<TranslationItem>());
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x004129C8 File Offset: 0x00410BC8
		public static ICodingContainer EmergencyBrakingLightsWithBrakeLights()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.ExteriorLights, Translate.GetString("codingDB_EmergencyBrakingBlinkWithBrakeLights_Name"), "", "0A59", 0, 2, "0D0C", 0, 0, Array.Empty<TranslationItem>());
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x00412A00 File Offset: 0x00410C00
		public static ICodingContainer EmergencyBrakingLightsWithBrakeLightsPhase2()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.ExteriorLights, Translate.GetString("codingDB_EmergencyBrakingBlinkWithBrakeLightsPhase2_Name"), "", "0A5E", 2, 1, "0D21", 1, 2, Array.Empty<TranslationItem>());
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x00412A38 File Offset: 0x00410C38
		public static ICodingContainer KodiaqHalogenDisableFrontSideLightWhenHeadlightsEngaged()
		{
			MQB_LightFunction left_lowbeam = MQB_LightFunction.FromValue("0B");
			MQB_LightFunction left_highbeam = MQB_LightFunction.FromValue("0D");
			MQB_LightFunction right_lowbeam = MQB_LightFunction.FromValue("0C");
			MQB_LightFunction right_highbeam = MQB_LightFunction.FromValue("0E");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0552", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = left_lowbeam;
					mqb_LightConfiguration.FunctionF = left_highbeam;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = off_func;
					mqb_LightConfiguration.FunctionF = off_func;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0553", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionE = right_lowbeam;
					mqb_LightConfiguration2.FunctionF = right_highbeam;
					mqb_LightConfiguration2.DimmwertEF = 0;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionE = off_func;
					mqb_LightConfiguration2.FunctionF = off_func;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_FrontSideLightsDisableWhenHeadlightsOrHighBeamActiveSkodaKodiaqWithHalogenHeadlights_Name"), Translate.GetString("codingDB_FrontSideLightsDisableWhenHeadlightsOrHighBeamActiveSkodaKodiaqWithHalogenHeadlights_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x00412B60 File Offset: 0x00410D60
		public static ICodingContainer KodiaqBlinkStripesWithTurnLights()
		{
			MQB_LightFunction func_left_turn_bright_phase = MQB_LightFunction.FromValue("02");
			MQB_LightFunction func_left_turn_dark_phase = MQB_LightFunction.FromValue("03");
			MQB_LightFunction func_right_turn_bright_phase = MQB_LightFunction.FromValue("04");
			MQB_LightFunction func_right_turn_dark_phase = MQB_LightFunction.FromValue("05");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0561", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = func_left_turn_dark_phase;
					mqb_LightConfiguration.DimmwertEF = 127;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = func_left_turn_bright_phase;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = off_func;
					mqb_LightConfiguration.FunctionG = off_func;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0560", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionE = func_right_turn_dark_phase;
					mqb_LightConfiguration2.DimmwertEF = 127;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration2.FunctionG = func_right_turn_bright_phase;
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionE = off_func;
					mqb_LightConfiguration2.FunctionG = off_func;
					mqb_LightConfiguration2.DimmwertEF = 0;
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_StripesEyelashesInHeadlightsBlinkWithTurnSignalsSkodaKodiaqKaroqWithLedHeadlights_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x00412C84 File Offset: 0x00410E84
		public static ICodingContainer TiguanIIBlinkRearSideLightsWithTurnLights()
		{
			MQB_LightFunction func_left_turn_bright_phase = MQB_LightFunction.FromValue("02");
			MQB_LightFunction.FromValue("03");
			MQB_LightFunction func_right_turn_bright_phase = MQB_LightFunction.FromValue("04");
			MQB_LightFunction.FromValue("05");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0560", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = func_right_turn_bright_phase;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration.DimmwertGH = 0;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = off_func;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0561", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionG = func_left_turn_bright_phase;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration2.DimmwertGH = 0;
				}
				else
				{
					mqb_LightConfiguration2.FunctionG = off_func;
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0567", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				MQB_LightConfiguration mqb_LightConfiguration3 = new MQB_LightConfiguration(array3);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration3.FunctionG = func_left_turn_bright_phase;
					mqb_LightConfiguration3.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration3.DimmwertGH = 0;
				}
				else
				{
					mqb_LightConfiguration3.FunctionG = off_func;
					mqb_LightConfiguration3.DimmwertGH = 0;
					mqb_LightConfiguration3.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration3.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0568", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				MQB_LightConfiguration mqb_LightConfiguration4 = new MQB_LightConfiguration(array4);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration4.FunctionG = func_right_turn_bright_phase;
					mqb_LightConfiguration4.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration4.DimmwertGH = 0;
				}
				else
				{
					mqb_LightConfiguration4.FunctionG = off_func;
					mqb_LightConfiguration4.DimmwertGH = 0;
					mqb_LightConfiguration4.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration4.ApplyToData();
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearSideLightsBlinkWithTurnLightsVwTiguanIi3DLed_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem2, mqbeasyCodingItem })
			{
				InnerDescription = Translate.GetString("codingDB_NotCompatibleWithTiguanIIFL")
			};
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x00412E48 File Offset: 0x00411048
		public static ICodingContainer Tiguan3DLED_RearLightsWhenDayLights()
		{
			MQB_LightFunction DRL_FUNC = MQB_LightFunction.FromValue("14");
			MQB_LightFunction OFF_FUNC = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0567", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0568", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionC = DRL_FUNC;
					mqb_LightConfiguration2.DimmwertCD = 127;
					mqb_LightConfiguration2.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionC = OFF_FUNC;
					mqb_LightConfiguration2.DimmwertCD = 0;
					mqb_LightConfiguration2.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0560", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				MQB_LightConfiguration mqb_LightConfiguration3 = new MQB_LightConfiguration(array3);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration3.FunctionC = DRL_FUNC;
					mqb_LightConfiguration3.DimmwertCD = 127;
					mqb_LightConfiguration3.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration3.FunctionC = OFF_FUNC;
					mqb_LightConfiguration3.DimmwertCD = 0;
					mqb_LightConfiguration3.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration3.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0561", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				MQB_LightConfiguration mqb_LightConfiguration4 = new MQB_LightConfiguration(array4);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration4.FunctionC = DRL_FUNC;
					mqb_LightConfiguration4.DimmwertCD = 127;
					mqb_LightConfiguration4.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration4.FunctionC = OFF_FUNC;
					mqb_LightConfiguration4.DimmwertCD = 0;
					mqb_LightConfiguration4.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration4.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearSideLightsTurnOnWhenDaytimeRunningLightsAreActiveScandiavianDrlsForVwTiguanIiWith3DLed_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = Translate.GetString("codingDB_NotCompatibleWithTiguanIIFL")
			};
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x00412F58 File Offset: 0x00411158
		public static ICodingContainer TiguanBASIC_LED_RearLightsWhenDayLights()
		{
			MQB_LightFunction DRL_FUNC = MQB_LightFunction.FromValue("14");
			MQB_LightFunction OFF_FUNC = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0567", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 35;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0568", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionC = DRL_FUNC;
					mqb_LightConfiguration2.DimmwertCD = 35;
					mqb_LightConfiguration2.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionC = OFF_FUNC;
					mqb_LightConfiguration2.DimmwertCD = 0;
					mqb_LightConfiguration2.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0564", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				MQB_LightConfiguration mqb_LightConfiguration3 = new MQB_LightConfiguration(array3);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration3.FunctionC = DRL_FUNC;
					mqb_LightConfiguration3.DimmwertCD = 35;
					mqb_LightConfiguration3.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration3.FunctionC = OFF_FUNC;
					mqb_LightConfiguration3.DimmwertCD = 0;
					mqb_LightConfiguration3.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration3.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0565", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				MQB_LightConfiguration mqb_LightConfiguration4 = new MQB_LightConfiguration(array4);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration4.FunctionC = DRL_FUNC;
					mqb_LightConfiguration4.DimmwertCD = 35;
					mqb_LightConfiguration4.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration4.FunctionC = OFF_FUNC;
					mqb_LightConfiguration4.DimmwertCD = 0;
					mqb_LightConfiguration4.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration4.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearSideLightsTurnOnWhenDaytimeRunningLightsAreActiveScandiavianDrlsForVwTiguanIiWithLed_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = Translate.GetString("codingDB_NotCompatibleWithTiguanIIFL")
			};
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x00413068 File Offset: 0x00411268
		public static ICodingContainer TurnOnFogLightsWhenReversing()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D1D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D1D", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Other, Translate.GetString("codingDB_FrontFogLightsTurnOnFogWhenReversing_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x00413110 File Offset: 0x00411310
		public static ICodingContainer SkodaSuperB_MK3_2019()
		{
			MQB_LightFunction LEFT_FUNC = MQB_LightFunction.FromValue("3C");
			MQB_LightFunction RIGHT_FUNC = MQB_LightFunction.FromValue("3D");
			MQB_LightFunction OFF_FUNC = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0554", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.DimmwertAB = 100;
					mqb_LightConfiguration.FunctionG = LEFT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 25;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.FunctionG = OFF_FUNC;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0555", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.DimmwertAB = 100;
					mqb_LightConfiguration2.FunctionG = RIGHT_FUNC;
					mqb_LightConfiguration2.DimmwertGH = 25;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration2.DimmwertAB = 127;
					mqb_LightConfiguration2.FunctionG = OFF_FUNC;
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_DrlSmoothTurnOnDrlAfterTurnSignalWasTurnedOffCompatibilitySkodaSuperbMk3_Name"), "Skoda SuperB MY2019", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x004131C8 File Offset: 0x004113C8
		public static ICodingContainer Tiguan3DLED_RearLightsAudiStyle()
		{
			MQB_LightFunction LEFT_FUNC = MQB_LightFunction.FromValue("06");
			MQB_LightFunction RIGHT_FUNC = MQB_LightFunction.FromValue("07");
			MQB_LightFunction OFF_FUNC = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0561", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = LEFT_FUNC;
					mqb_LightConfiguration.DimmwertAB = 126;
					mqb_LightConfiguration.DimmwertCD = 126;
					mqb_LightConfiguration.DimmwertGH = 10;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = OFF_FUNC;
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0567", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionG = LEFT_FUNC;
					mqb_LightConfiguration2.DimmwertAB = 126;
					mqb_LightConfiguration2.DimmwertCD = 126;
					mqb_LightConfiguration2.DimmwertGH = 10;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionG = OFF_FUNC;
					mqb_LightConfiguration2.DimmwertAB = 127;
					mqb_LightConfiguration2.DimmwertCD = 127;
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0560", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				MQB_LightConfiguration mqb_LightConfiguration3 = new MQB_LightConfiguration(array3);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration3.FunctionG = RIGHT_FUNC;
					mqb_LightConfiguration3.DimmwertAB = 126;
					mqb_LightConfiguration3.DimmwertCD = 126;
					mqb_LightConfiguration3.DimmwertGH = 10;
					mqb_LightConfiguration3.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration3.FunctionG = OFF_FUNC;
					mqb_LightConfiguration3.DimmwertAB = 127;
					mqb_LightConfiguration3.DimmwertCD = 127;
					mqb_LightConfiguration3.DimmwertGH = 0;
					mqb_LightConfiguration3.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration3.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0568", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				MQB_LightConfiguration mqb_LightConfiguration4 = new MQB_LightConfiguration(array4);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration4.FunctionG = RIGHT_FUNC;
					mqb_LightConfiguration4.DimmwertAB = 126;
					mqb_LightConfiguration4.DimmwertCD = 126;
					mqb_LightConfiguration4.DimmwertGH = 10;
					mqb_LightConfiguration4.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration4.FunctionG = OFF_FUNC;
					mqb_LightConfiguration4.DimmwertAB = 127;
					mqb_LightConfiguration4.DimmwertCD = 127;
					mqb_LightConfiguration4.DimmwertGH = 0;
					mqb_LightConfiguration4.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration4.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearSideLightsDimmWhileTurnLightsAreActiveAudiStyleForVwTiguanIiWith3DLed_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem3, mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem4 })
			{
				InnerDescription = Translate.GetString("codingDB_NotCompatibleWithTiguanIIFL")
			};
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x004132E8 File Offset: 0x004114E8
		public static ICodingContainer FogLights_ActivateWithHighBeam()
		{
			MQB_LightFunction FUNC_HIGHBEAM_LEFT = MQB_LightFunction.FromValue("0D");
			MQB_LightFunction FUNC_HIGHBEAM_RIGHT = MQB_LightFunction.FromValue("0E");
			MQB_LightFunction.FromValue("0F");
			MQB_LightFunction FUNC_OFF = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("055C", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = FUNC_HIGHBEAM_LEFT;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("055D", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionG = FUNC_HIGHBEAM_RIGHT;
					mqb_LightConfiguration2.DimmwertGH = 100;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionG = FUNC_OFF;
					mqb_LightConfiguration2.DimmwertGH = 100;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_FrontFogLightsActivateWithHighBeam_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Передние противотуманные фары: активация совместно с дальним светом (обычный режим работы)", "", "")
				}
			};
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x004133CC File Offset: 0x004115CC
		public static ICodingContainer FogLights_BlinkWithHighBeam()
		{
			MQB_LightFunction.FromValue("0D");
			MQB_LightFunction.FromValue("0E");
			MQB_LightFunction FUNC_HIGHBEAM_BLINK = MQB_LightFunction.FromValue("0F");
			MQB_LightFunction FUNC_OFF = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("055C", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionH = FUNC_HIGHBEAM_BLINK;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionH = FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("055D", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionH = FUNC_HIGHBEAM_BLINK;
					mqb_LightConfiguration2.DimmwertGH = 100;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionH = FUNC_OFF;
					mqb_LightConfiguration2.DimmwertGH = 100;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_FrontFogLightsBlinkWithHighBeamBlink_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x00413488 File Offset: 0x00411688
		public static ICodingContainer FogLights_ActivateAndBlinkWithHighBeam()
		{
			MQB_LightFunction FUNC_HIGHBEAM_LEFT = MQB_LightFunction.FromValue("0D");
			MQB_LightFunction FUNC_HIGHBEAM_RIGHT = MQB_LightFunction.FromValue("0E");
			MQB_LightFunction FUNC_HIGHBEAM_BLINK = MQB_LightFunction.FromValue("0F");
			MQB_LightFunction FUNC_OFF = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("055C", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = FUNC_HIGHBEAM_LEFT;
					mqb_LightConfiguration.FunctionH = FUNC_HIGHBEAM_BLINK;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = FUNC_OFF;
					mqb_LightConfiguration.FunctionH = FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("055D", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionG = FUNC_HIGHBEAM_RIGHT;
					mqb_LightConfiguration2.FunctionH = FUNC_HIGHBEAM_BLINK;
					mqb_LightConfiguration2.DimmwertGH = 100;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionG = FUNC_OFF;
					mqb_LightConfiguration2.FunctionH = FUNC_OFF;
					mqb_LightConfiguration2.DimmwertGH = 100;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_FrontFogLightsActivateWithHighBeamAndBlinkWithHighBeamBlink_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x0041354D File Offset: 0x0041174D
		public static ICodingContainer LimitMaxFrontLights()
		{
			return ExteriorLight.LimitMaxFrontLights();
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x00413554 File Offset: 0x00411754
		public static ICodingContainer TurnLights_USStyleHalogenOnly()
		{
			MQB_LightFunction FUNC_STANDLICHT_ALLGEMEIN = MQB_LightFunction.FromValue("08");
			MQB_LightFunction FUNC_BLINKEN_LINKS_DUNKEL = MQB_LightFunction.FromValue("03");
			MQB_LightFunction FUNC_BLINKEN_RECHTS_DUNKEL = MQB_LightFunction.FromValue("05");
			MQB_LightFunction FUNC_OFF = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0550", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionD = FUNC_STANDLICHT_ALLGEMEIN;
					mqb_LightConfiguration.DimmwertCD = 30;
					mqb_LightConfiguration.FunctionE = FUNC_BLINKEN_LINKS_DUNKEL;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionD = FUNC_OFF;
					mqb_LightConfiguration.FunctionE = FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0551", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionD = FUNC_STANDLICHT_ALLGEMEIN;
					mqb_LightConfiguration2.DimmwertCD = 30;
					mqb_LightConfiguration2.FunctionE = FUNC_BLINKEN_RECHTS_DUNKEL;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionD = FUNC_OFF;
					mqb_LightConfiguration2.FunctionE = FUNC_OFF;
					mqb_LightConfiguration2.DimmwertEF = 0;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_FrontTurnLightsAlwaysActiveAtHalfPowerUsStyle_Name"), Translate.GetString("codingDB_FrontTurnLightsAlwaysActiveAtHalfPowerUsStyle_Description"), false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x00413620 File Offset: 0x00411820
		public static ICodingContainer ComingHomeLamps()
		{
			MQBAdaptationOption lowbeam = new MQBAdaptationOption(Translate.GetString("codingDB_LowBeam_Name"), MQBAdaptationTemplate.DisableOption.Value);
			MQBAdaptationOption foglight = new MQBAdaptationOption(Translate.GetString("codingDB_FogLights_Name"), MQBAdaptationTemplate.EnableOption.Value);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A57", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, false);
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[4], 7))
				{
					return foglight.Title;
				}
				return lowbeam.Title;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D04", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 2, 0, false);
				}
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[2], 0))
				{
					return foglight.Title;
				}
				return lowbeam.Title;
			});
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_ComingHomeLeavingHomeLights_Name"), "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(lowbeam);
			mqbalternativeCoding.Options.Add(foglight);
			mqbalternativeCoding.RequiresPro = false;
			return mqbalternativeCoding;
		}

		// Token: 0x06005649 RID: 22089 RVA: 0x00413748 File Offset: 0x00411948
		public static ICodingContainer LicensePlate_LampsType()
		{
			MQB_LampType lamp5W = MQB_LampType.FromValue("12");
			MQB_LampType ledLow = MQB_LampType.FromValue("24");
			MQB_LampType ledGeneral = MQB_LampType.FromValue("2B");
			MQBAdaptationOption opt5W = new MQBAdaptationOption("5W", "12");
			MQBAdaptationOption optLedLow = new MQBAdaptationOption("LED (low power)", "24", new TranslationItem[]
			{
				new TranslationItem("ru", "Светодиоды (низкая мощность)", "", "")
			});
			MQBAdaptationOption optLedGen = new MQBAdaptationOption("LED", "2B", new TranslationItem[]
			{
				new TranslationItem("ru", "Светодиоды", "", "")
			});
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, Translate.GetString("codingDB_LicensePlateLightTypeLed5W_Name"), "", "0569", VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == opt5W.Value)
				{
					mqb_LightConfiguration.LampType = lamp5W;
					mqb_LightConfiguration.DimmwertAB = 100;
				}
				else if (value == optLedLow.Value)
				{
					mqb_LightConfiguration.LampType = ledLow;
					mqb_LightConfiguration.DimmwertAB = 127;
				}
				else if (value == optLedGen.Value)
				{
					mqb_LightConfiguration.LampType = ledGeneral;
					mqb_LightConfiguration.DimmwertAB = 127;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null, Array.Empty<MQBAdaptationOption>());
			mqbeasyCodingItem.Options.Clear();
			mqbeasyCodingItem.Options.Add(opt5W);
			mqbeasyCodingItem.Options.Add(optLedGen);
			mqbeasyCodingItem.Options.Add(optLedLow);
			return mqbeasyCodingItem;
		}

		// Token: 0x0600564A RID: 22090 RVA: 0x00413898 File Offset: 0x00411A98
		public static ICodingContainer SeatLeon5F_BlinkRearSideLightWithTurnSignals()
		{
			MQB_LightFunction FUNC_LEFT_HELLPHASE = MQB_LightFunction.FromValue("02");
			MQB_LightFunction FUNC_RIGHT_HELLPHASE = MQB_LightFunction.FromValue("04");
			MQB_LightFunction FUNC_LEFT_DUNKEL = MQB_LightFunction.FromValue("03");
			MQB_LightFunction FUNC_RIGHT_DUNKEL = MQB_LightFunction.FromValue("05");
			MQB_LightFunction FUNC_OFF = MQB_LightFunction.FromValue("00");
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0567", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = FUNC_LEFT_DUNKEL;
					mqb_LightConfiguration.DimmwertEF = 100;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = FUNC_LEFT_HELLPHASE;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0568", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(array2);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration2.FunctionE = FUNC_RIGHT_DUNKEL;
					mqb_LightConfiguration2.DimmwertEF = 100;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration2.FunctionG = FUNC_RIGHT_HELLPHASE;
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration2.FunctionE = FUNC_OFF;
					mqb_LightConfiguration2.DimmwertEF = 0;
					mqb_LightConfiguration2.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration2.FunctionG = FUNC_OFF;
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration2.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("056A", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				MQB_LightConfiguration mqb_LightConfiguration3 = new MQB_LightConfiguration(array3);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration3.FunctionE = FUNC_LEFT_DUNKEL;
					mqb_LightConfiguration3.DimmwertEF = 100;
					mqb_LightConfiguration3.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration3.FunctionG = FUNC_LEFT_HELLPHASE;
					mqb_LightConfiguration3.DimmwertGH = 0;
					mqb_LightConfiguration3.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration3.FunctionE = FUNC_OFF;
					mqb_LightConfiguration3.DimmwertEF = 0;
					mqb_LightConfiguration3.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration3.FunctionG = FUNC_OFF;
					mqb_LightConfiguration3.DimmwertGH = 0;
					mqb_LightConfiguration3.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration3.ApplyToData();
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("056B", "09", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				MQB_LightConfiguration mqb_LightConfiguration4 = new MQB_LightConfiguration(array4);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration4.FunctionE = FUNC_RIGHT_DUNKEL;
					mqb_LightConfiguration4.DimmwertEF = 100;
					mqb_LightConfiguration4.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration4.FunctionG = FUNC_RIGHT_HELLPHASE;
					mqb_LightConfiguration4.DimmwertGH = 0;
					mqb_LightConfiguration4.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration4.FunctionE = FUNC_OFF;
					mqb_LightConfiguration4.DimmwertEF = 0;
					mqb_LightConfiguration4.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration4.FunctionG = FUNC_OFF;
					mqb_LightConfiguration4.DimmwertGH = 0;
					mqb_LightConfiguration4.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration4.ApplyToData();
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_RearSideLightsWinkWithTurnLightsForSeatLeon5F_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4 });
		}

		// Token: 0x0600564B RID: 22091 RVA: 0x004139C8 File Offset: 0x00411BC8
		public static ICodingContainer ComingHomeActivationEvents()
		{
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_IgnitionTurnedOff_Name"), MQBAdaptationTemplate.DisableOption.Value);
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(Translate.GetString("codingDB_DriverDoorOpened_Name"), MQBAdaptationTemplate.EnableOption.Value);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A57", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D04", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 2, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 2, 1, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_ComingHomeAutomaticActivationTrigger_Name"), "", "70E", "778", "31347", new MQBAdaptationOption[] { mqbadaptationOption, mqbadaptationOption2 })
			{
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 },
				RequiresPro = false
			};
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x00413AB4 File Offset: 0x00411CB4
		public static ICodingContainer ComingHomeActivation()
		{
			MQBAdaptationOption opt_off = MQBAdaptationTemplate.DisableOption;
			MQBAdaptationOption opt_manual = new MQBAdaptationOption(Translate.GetString("codingDB_Manual_Name"), "01");
			MQBAdaptationOption opt_auto = new MQBAdaptationOption(Translate.GetString("codingDB_Automatic_Name"), "10");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A57", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 5, false);
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
				}
				else if (value == opt_manual.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 5, false);
					BitHelpers.SwitchBitInByte(array, 4, 4, true);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, true);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					array[9] = 116;
					array[5] = 60;
				}
				else if (value == opt_auto.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 5, true);
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, true);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					array[9] = 116;
					array[5] = 60;
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D04", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
				}
				else if (value == opt_manual.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 0, true);
				}
				else if (value == opt_auto.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_ComingHomeActivationAutomaticManual_Name"), "", "70E", "778", "31347", new MQBAdaptationOption[] { opt_off, opt_manual, opt_auto })
			{
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 },
				RequiresPro = false
			};
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x00413B9C File Offset: 0x00411D9C
		public static ICodingContainer ReduceDRLBrightness()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.ExteriorLights, Translate.GetString("codingDB_DrlReduceBrightnessWhenActiveInDrlSidelightsMode_Name"), "For LED DRL", "0A58", 3, 4, "0D02", 0, 4, Array.Empty<TranslationItem>());
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x00413BD4 File Offset: 0x00411DD4
		public static ICodingContainer LightSensorSensivity()
		{
			MQBAdaptationOption opt_sens = new MQBAdaptationOption(Translate.GetString("codingDB_Sensitive_Name"), "00");
			MQBAdaptationOption opt_norm = new MQBAdaptationOption(Translate.GetString("codingDB_Normal_Name"), "01");
			MQBAdaptationOption opt_nonsens = new MQBAdaptationOption(Translate.GetString("codingDB_NonSensitive_Name"), "10");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D0C", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_sens.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == opt_norm.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == opt_nonsens.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0C", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt_sens.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 0, false);
					BitHelpers.SwitchBitInByte(array2, 3, 1, false);
				}
				else if (value == opt_norm.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 0, true);
					BitHelpers.SwitchBitInByte(array2, 3, 1, false);
				}
				else if (value == opt_nonsens.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 0, false);
					BitHelpers.SwitchBitInByte(array2, 3, 1, true);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_LightSensorSensivity_Name"), "", "70E", "778", "", new MQBAdaptationOption[] { opt_sens, opt_norm, opt_nonsens })
			{
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 },
				RequiresPro = false
			};
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x00413CD8 File Offset: 0x00411ED8
		public static ICodingContainer LightSensorInstalled()
		{
			MQBAdaptationOption opt_none = new MQBAdaptationOption("Not installed", "00", new TranslationItem[]
			{
				new TranslationItem("ru", "Не установлен", "", "")
			});
			MQBAdaptationOption opt_diskret = new MQBAdaptationOption("Diskret", "01", new TranslationItem[]
			{
				new TranslationItem("ru", "Дискретный", "", "")
			});
			MQBAdaptationOption opt_lin = new MQBAdaptationOption("Installed, LIN bus (LIN_REGEN)", "10", new TranslationItem[]
			{
				new TranslationItem("ru", "Установлен, шина LIN (LIN_REGEN)", "", "")
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D0C", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_none.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				else if (value == opt_diskret.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				else if (value == opt_lin.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0600", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt_none.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 6, 0, false);
					BitHelpers.SwitchBitInByte(array2, 6, 1, false);
				}
				else if (value == opt_diskret.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 6, 0, true);
					BitHelpers.SwitchBitInByte(array2, 6, 1, false);
				}
				else if (value == opt_lin.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 6, 0, false);
					BitHelpers.SwitchBitInByte(array2, 6, 1, true);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_LightSensorInstalled_Name"), "", "70E", "778", "", new MQBAdaptationOption[] { opt_none, opt_diskret, opt_lin })
			{
				Alternatives = { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 },
				RequiresPro = false
			};
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x00413E30 File Offset: 0x00412030
		public static ICodingContainer DynamicTurnLights_Tiguan2FL()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A5E", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				return array;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("DinamicTurnLights_Tiguan2FL_Name"), Translate.GetString("DinamicTurnLights_Tiguan2FL_Description"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit });
		}

		// Token: 0x06005651 RID: 22097 RVA: 0x00413E9C File Offset: 0x0041209C
		public static ICodingContainer DRL_Mode()
		{
			MQBAdaptationOption opt1 = MQBAdaptationTemplate.DisableOption;
			MQBAdaptationOption opt2_daytime_running_lamps = new MQBAdaptationOption("Daytime running lamps", "01");
			MQBAdaptationOption opt3_daytime_running_lights = new MQBAdaptationOption("Daytime running lights", "02");
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption("Disabled light (Versehrtenlicht)", "03");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A58", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingInternal)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt1.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				else if (value == opt2_daytime_running_lamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
				}
				else if (value == opt3_daytime_running_lights.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 1, true);
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				else if (value == opt2_daytime_running_lamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D02", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingInternal)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt1.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 1, false);
					BitHelpers.SwitchBitInByte(array2, 1, 0, false);
				}
				else if (value == opt2_daytime_running_lamps.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 1, false);
					BitHelpers.SwitchBitInByte(array2, 1, 0, true);
				}
				else if (value == opt3_daytime_running_lights.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 1, true);
					BitHelpers.SwitchBitInByte(array2, 1, 0, false);
				}
				else if (value == opt2_daytime_running_lamps.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 1, false);
					BitHelpers.SwitchBitInByte(array2, 1, 0, true);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.ExteriorLights, "DRL: Mode", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(opt1);
			mqbalternativeCoding.Options.Add(opt2_daytime_running_lamps);
			mqbalternativeCoding.Options.Add(opt3_daytime_running_lights);
			mqbalternativeCoding.Options.Add(mqbadaptationOption);
			mqbalternativeCoding.Translations.Add(new TranslationItem("ru", "ДХО: Режим работы", "", ""));
			return mqbalternativeCoding;
		}

		// Token: 0x06005652 RID: 22098 RVA: 0x00413FCC File Offset: 0x004121CC
		public static ICodingContainer DrivingLightsByDay()
		{
			MQBAdaptationOption opt_notActive = new MQBAdaptationOption("Not active", "00");
			MQBAdaptationOption opt_DaytimeRunningLamps = new MQBAdaptationOption("Daytime running lamps", "01");
			MQBAdaptationOption opt_DaytimeRunningLights = new MQBAdaptationOption("Daytime running lights", "10");
			MQBAdaptationOption opt_Versehrtenlicht = new MQBAdaptationOption("Disabled light", "11");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A58", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_notActive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
				}
				else if (value == opt_DaytimeRunningLamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
				}
				else if (value == opt_DaytimeRunningLights.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 1, true);
				}
				else if (value == opt_Versehrtenlicht.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 1, true);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D02", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt_notActive.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 0, false);
					BitHelpers.SwitchBitInByte(array2, 1, 1, false);
				}
				else if (value == opt_DaytimeRunningLamps.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 0, true);
					BitHelpers.SwitchBitInByte(array2, 1, 1, false);
				}
				else if (value == opt_DaytimeRunningLights.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 0, false);
					BitHelpers.SwitchBitInByte(array2, 1, 1, true);
				}
				else if (value == opt_Versehrtenlicht.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 0, true);
					BitHelpers.SwitchBitInByte(array2, 1, 1, true);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, Translate.GetString("codingDB_DrivingLightsByDay_Title"), Translate.GetString("codingDB_DrivingLightsByDay_Description"), "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
		}

		// Token: 0x02000AB3 RID: 2739
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005653 RID: 22099 RVA: 0x0041409C File Offset: 0x0041229C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005654 RID: 22100 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005655 RID: 22101 RVA: 0x004140A8 File Offset: 0x004122A8
			internal byte[] <MQB_09_CornerActivation_NOT_LED>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
					mqb_LightConfiguration.LoadFromData(array);
					mqb_LightConfiguration.FunctionB = MQB_LightFunction.FromValue("16");
					array = mqb_LightConfiguration.ApplyToData();
				}
				else
				{
					MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration();
					mqb_LightConfiguration2.LoadFromData(array);
					mqb_LightConfiguration2.FunctionB = MQB_LightFunction.FromValue("00");
					array = mqb_LightConfiguration2.ApplyToData();
				}
				return array;
			}

			// Token: 0x06005656 RID: 22102 RVA: 0x00414123 File Offset: 0x00412323
			internal string <MQB_09_CornerActivation_NOT_LED>b__1_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
				mqb_LightConfiguration.LoadFromData(data);
				if (mqb_LightConfiguration.FunctionB.Value == "16")
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005657 RID: 22103 RVA: 0x0041415C File Offset: 0x0041235C
			internal byte[] <MQB_09_CornerActivation_NOT_LED>b__1_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
					mqb_LightConfiguration.LoadFromData(array);
					mqb_LightConfiguration.FunctionB = MQB_LightFunction.FromValue("17");
					array = mqb_LightConfiguration.ApplyToData();
				}
				else
				{
					MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration();
					mqb_LightConfiguration2.LoadFromData(array);
					mqb_LightConfiguration2.FunctionB = MQB_LightFunction.FromValue("00");
					array = mqb_LightConfiguration2.ApplyToData();
				}
				return array;
			}

			// Token: 0x06005658 RID: 22104 RVA: 0x004141D7 File Offset: 0x004123D7
			internal string <MQB_09_CornerActivation_NOT_LED>b__1_3(byte[] data, MQBEasyCodingItem codingItem)
			{
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
				mqb_LightConfiguration.LoadFromData(data);
				if (mqb_LightConfiguration.FunctionB.Value == "17")
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005659 RID: 22105 RVA: 0x00414210 File Offset: 0x00412410
			internal byte[] <MQB_09_CornerActivation_NOT_LED>b__1_4(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[5] = 128;
					array[8] = 100;
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					array[2] = 0;
					array[9] = 200;
					array[10] = 190;
					array[7] = 120;
					array[11] = 160;
					array[6] = 80;
					array[3] = 80;
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
					array[0] = 31;
				}
				return array;
			}

			// Token: 0x0600565A RID: 22106 RVA: 0x004142A9 File Offset: 0x004124A9
			internal string <MQB_09_CornerActivation_NOT_LED>b__1_5(byte[] data, MQBAlternativeCoding coding2)
			{
				return MQBAdaptationTemplate.EnableOption.Title;
			}

			// Token: 0x0600565B RID: 22107 RVA: 0x004142B8 File Offset: 0x004124B8
			internal byte[] <MQB_09_CornerActivation_NOT_LED>b__1_6(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[6] = 127;
					array[9] = 100;
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					array[3] = 0;
					array[10] = 200;
					array[11] = 190;
					array[8] = 120;
					array[12] = 160;
					array[7] = 80;
					array[4] = 80;
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
				}
				return array;
			}

			// Token: 0x0600565C RID: 22108 RVA: 0x004142A9 File Offset: 0x004124A9
			internal string <MQB_09_CornerActivation_NOT_LED>b__1_7(byte[] data, MQBAlternativeCoding coding2)
			{
				return MQBAdaptationTemplate.EnableOption.Title;
			}

			// Token: 0x0600565D RID: 22109 RVA: 0x0041434C File Offset: 0x0041254C
			internal byte[] <MQB_09_FogLightWithHighBeam_LED_Only>b__2_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 14;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}

			// Token: 0x0600565E RID: 22110 RVA: 0x0041438C File Offset: 0x0041258C
			internal string <MQB_09_FogLightWithHighBeam_LED_Only>b__2_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 14)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600565F RID: 22111 RVA: 0x004143AC File Offset: 0x004125AC
			internal byte[] <MQB_09_FogLightWithHighBeam_LED_Only>b__2_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 14;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}

			// Token: 0x06005660 RID: 22112 RVA: 0x0041438C File Offset: 0x0041258C
			internal string <MQB_09_FogLightWithHighBeam_LED_Only>b__2_3(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 14)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005661 RID: 22113 RVA: 0x004143EC File Offset: 0x004125EC
			internal byte[] <MQB_09_FogLightWithHighBeam_LED_Only>b__2_4(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 13;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}

			// Token: 0x06005662 RID: 22114 RVA: 0x0041442C File Offset: 0x0041262C
			internal string <MQB_09_FogLightWithHighBeam_LED_Only>b__2_5(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 13)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005663 RID: 22115 RVA: 0x0041444C File Offset: 0x0041264C
			internal byte[] <MQB_09_FogLightWithHighBeam_LED_Only>b__2_6(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 13;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}

			// Token: 0x06005664 RID: 22116 RVA: 0x0041442C File Offset: 0x0041262C
			internal string <MQB_09_FogLightWithHighBeam_LED_Only>b__2_7(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 13)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005665 RID: 22117 RVA: 0x0041448C File Offset: 0x0041268C
			internal byte[] <MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only>b__3_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 15;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}

			// Token: 0x06005666 RID: 22118 RVA: 0x004144CC File Offset: 0x004126CC
			internal string <MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only>b__3_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 15)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005667 RID: 22119 RVA: 0x004144EC File Offset: 0x004126EC
			internal byte[] <MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only>b__3_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 15;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}

			// Token: 0x06005668 RID: 22120 RVA: 0x004144CC File Offset: 0x004126CC
			internal string <MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only>b__3_3(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 15)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005669 RID: 22121 RVA: 0x0041452C File Offset: 0x0041272C
			internal byte[] <MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only>b__3_4(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 15;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}

			// Token: 0x0600566A RID: 22122 RVA: 0x004144CC File Offset: 0x004126CC
			internal string <MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only>b__3_5(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 15)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600566B RID: 22123 RVA: 0x0041456C File Offset: 0x0041276C
			internal byte[] <MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only>b__3_6(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[8] = 15;
				}
				else
				{
					array[8] = 0;
				}
				return array;
			}

			// Token: 0x0600566C RID: 22124 RVA: 0x004144CC File Offset: 0x004126CC
			internal string <MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only>b__3_7(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data[8] == 15)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600566D RID: 22125 RVA: 0x004145AC File Offset: 0x004127AC
			internal byte[] <BlinkRearTurnLightsWithRearLights>b__5_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 15, 7, true);
					array[13] = 2;
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 15, 7, false);
					array[13] = 0;
				}
				return array;
			}

			// Token: 0x0600566E RID: 22126 RVA: 0x00414601 File Offset: 0x00412801
			internal string <BlinkRearTurnLightsWithRearLights>b__5_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if ((data[15] & 128) == 128 && data[13] == 2)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600566F RID: 22127 RVA: 0x00414630 File Offset: 0x00412830
			internal byte[] <BlinkRearTurnLightsWithRearLights>b__5_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 15, 7, true);
					array[13] = 4;
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 15, 7, false);
					array[13] = 0;
				}
				return array;
			}

			// Token: 0x06005670 RID: 22128 RVA: 0x00414685 File Offset: 0x00412885
			internal string <BlinkRearTurnLightsWithRearLights>b__5_3(byte[] data, MQBEasyCodingItem codingItem)
			{
				if ((data[15] & 128) == 128 && data[13] == 4)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005671 RID: 22129 RVA: 0x004146B4 File Offset: 0x004128B4
			internal byte[] <TurnLightsPoliteBlinks>b__6_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (!(value == "2"))
				{
					if (!(value == "3"))
					{
						if (!(value == "4"))
						{
							if (value == "5")
							{
								BitHelpers.SwitchBitInByte(array, 0, 1, true);
								BitHelpers.SwitchBitInByte(array, 0, 2, true);
							}
						}
						else
						{
							BitHelpers.SwitchBitInByte(array, 0, 1, false);
							BitHelpers.SwitchBitInByte(array, 0, 2, true);
						}
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 1, true);
						BitHelpers.SwitchBitInByte(array, 0, 2, false);
					}
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				BitHelpers.SwitchBitInByte(array, 0, 4, true);
				return array;
			}

			// Token: 0x06005672 RID: 22130 RVA: 0x00414764 File Offset: 0x00412964
			internal string <TurnLightsPoliteBlinks>b__6_1(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "2";
				}
				if (BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "3";
				}
				if (!BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "4";
				}
				if (BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "5";
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x06005673 RID: 22131 RVA: 0x004147E8 File Offset: 0x004129E8
			internal byte[] <TurnLightsPoliteBlinks>b__6_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (!(value == "2"))
				{
					if (!(value == "3"))
					{
						if (!(value == "4"))
						{
							if (value == "5")
							{
								BitHelpers.SwitchBitInByte(array, 0, 1, true);
								BitHelpers.SwitchBitInByte(array, 0, 2, true);
							}
						}
						else
						{
							BitHelpers.SwitchBitInByte(array, 0, 1, false);
							BitHelpers.SwitchBitInByte(array, 0, 2, true);
						}
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 0, 1, true);
						BitHelpers.SwitchBitInByte(array, 0, 2, false);
					}
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				BitHelpers.SwitchBitInByte(array, 0, 7, true);
				return array;
			}

			// Token: 0x06005674 RID: 22132 RVA: 0x00414898 File Offset: 0x00412A98
			internal string <TurnLightsPoliteBlinks>b__6_3(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "2";
				}
				if (BitHelpers.GetBit_0_7(data[0], 1) && !BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "3";
				}
				if (!BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "4";
				}
				if (BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2))
				{
					return "5";
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x06005675 RID: 22133 RVA: 0x0041491C File Offset: 0x00412B1C
			internal byte[] <TurnOffDRLWhenParkingBrakeOn>b__7_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 5, false);
				}
				return array;
			}

			// Token: 0x06005676 RID: 22134 RVA: 0x00414968 File Offset: 0x00412B68
			internal byte[] <TurnOffDRLWhenParkingBrakeOn>b__7_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
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

			// Token: 0x06005677 RID: 22135 RVA: 0x004149B4 File Offset: 0x00412BB4
			internal byte[] <TurnOffDRLWhenLightSwitchOff>b__8_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 2, false);
				}
				return array;
			}

			// Token: 0x06005678 RID: 22136 RVA: 0x00414A00 File Offset: 0x00412C00
			internal byte[] <TurnOffDRLWhenLightSwitchOff>b__8_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}

			// Token: 0x06005679 RID: 22137 RVA: 0x00414A4C File Offset: 0x00412C4C
			internal byte[] <DRLSteupInMMI>b__9_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				return array;
			}

			// Token: 0x0600567A RID: 22138 RVA: 0x00414A98 File Offset: 0x00412C98
			internal byte[] <DRLSteupInMMI>b__9_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}

			// Token: 0x0600567B RID: 22139 RVA: 0x00414AE4 File Offset: 0x00412CE4
			internal byte[] <MirrorLightsWithAreaView>b__10_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				return array;
			}

			// Token: 0x0600567C RID: 22140 RVA: 0x00414B30 File Offset: 0x00412D30
			internal byte[] <MirrorLightsWithAreaView>b__10_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 8, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 8, 2, false);
				}
				return array;
			}

			// Token: 0x0600567D RID: 22141 RVA: 0x00414B7C File Offset: 0x00412D7C
			internal byte[] <MQB_09_LightSwitchWithAutoMode>b__17_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, false);
				}
				return array;
			}

			// Token: 0x0600567E RID: 22142 RVA: 0x00414BC8 File Offset: 0x00412DC8
			internal byte[] <TurnOnFogLightsWhenReversing>b__27_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
				}
				return array;
			}

			// Token: 0x0600567F RID: 22143 RVA: 0x00414C14 File Offset: 0x00412E14
			internal byte[] <TurnOnFogLightsWhenReversing>b__27_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}

			// Token: 0x06005680 RID: 22144 RVA: 0x00414C60 File Offset: 0x00412E60
			internal byte[] <ComingHomeLamps>b__35_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, false);
				}
				return array;
			}

			// Token: 0x06005681 RID: 22145 RVA: 0x00414CAC File Offset: 0x00412EAC
			internal byte[] <ComingHomeLamps>b__35_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
				}
				return array;
			}

			// Token: 0x06005682 RID: 22146 RVA: 0x00414CF8 File Offset: 0x00412EF8
			internal byte[] <ComingHomeActivationEvents>b__38_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 0, false);
				}
				return array;
			}

			// Token: 0x06005683 RID: 22147 RVA: 0x00414D44 File Offset: 0x00412F44
			internal byte[] <ComingHomeActivationEvents>b__38_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				return array;
			}

			// Token: 0x06005684 RID: 22148 RVA: 0x00414D90 File Offset: 0x00412F90
			internal byte[] <DynamicTurnLights_Tiguan2FL>b__43_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 5, 6, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 5, 6, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 7, false);
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				return array;
			}

			// Token: 0x0400350A RID: 13578
			public static readonly ExteriorLights.<>c <>9 = new ExteriorLights.<>c();

			// Token: 0x0400350B RID: 13579
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x0400350C RID: 13580
			public static Func<byte[], MQBEasyCodingItem, string> <>9__1_1;

			// Token: 0x0400350D RID: 13581
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_2;

			// Token: 0x0400350E RID: 13582
			public static Func<byte[], MQBEasyCodingItem, string> <>9__1_3;

			// Token: 0x0400350F RID: 13583
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_4;

			// Token: 0x04003510 RID: 13584
			public static Func<byte[], MQBAlternativeCoding, string> <>9__1_5;

			// Token: 0x04003511 RID: 13585
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__1_6;

			// Token: 0x04003512 RID: 13586
			public static Func<byte[], MQBAlternativeCoding, string> <>9__1_7;

			// Token: 0x04003513 RID: 13587
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x04003514 RID: 13588
			public static Func<byte[], MQBEasyCodingItem, string> <>9__2_1;

			// Token: 0x04003515 RID: 13589
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_2;

			// Token: 0x04003516 RID: 13590
			public static Func<byte[], MQBEasyCodingItem, string> <>9__2_3;

			// Token: 0x04003517 RID: 13591
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_4;

			// Token: 0x04003518 RID: 13592
			public static Func<byte[], MQBEasyCodingItem, string> <>9__2_5;

			// Token: 0x04003519 RID: 13593
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_6;

			// Token: 0x0400351A RID: 13594
			public static Func<byte[], MQBEasyCodingItem, string> <>9__2_7;

			// Token: 0x0400351B RID: 13595
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_0;

			// Token: 0x0400351C RID: 13596
			public static Func<byte[], MQBEasyCodingItem, string> <>9__3_1;

			// Token: 0x0400351D RID: 13597
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_2;

			// Token: 0x0400351E RID: 13598
			public static Func<byte[], MQBEasyCodingItem, string> <>9__3_3;

			// Token: 0x0400351F RID: 13599
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_4;

			// Token: 0x04003520 RID: 13600
			public static Func<byte[], MQBEasyCodingItem, string> <>9__3_5;

			// Token: 0x04003521 RID: 13601
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__3_6;

			// Token: 0x04003522 RID: 13602
			public static Func<byte[], MQBEasyCodingItem, string> <>9__3_7;

			// Token: 0x04003523 RID: 13603
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_0;

			// Token: 0x04003524 RID: 13604
			public static Func<byte[], MQBEasyCodingItem, string> <>9__5_1;

			// Token: 0x04003525 RID: 13605
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__5_2;

			// Token: 0x04003526 RID: 13606
			public static Func<byte[], MQBEasyCodingItem, string> <>9__5_3;

			// Token: 0x04003527 RID: 13607
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__6_0;

			// Token: 0x04003528 RID: 13608
			public static Func<byte[], MQBAlternativeCoding, string> <>9__6_1;

			// Token: 0x04003529 RID: 13609
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__6_2;

			// Token: 0x0400352A RID: 13610
			public static Func<byte[], MQBAlternativeCoding, string> <>9__6_3;

			// Token: 0x0400352B RID: 13611
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__7_0;

			// Token: 0x0400352C RID: 13612
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__7_1;

			// Token: 0x0400352D RID: 13613
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__8_0;

			// Token: 0x0400352E RID: 13614
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__8_1;

			// Token: 0x0400352F RID: 13615
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_0;

			// Token: 0x04003530 RID: 13616
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_1;

			// Token: 0x04003531 RID: 13617
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__10_0;

			// Token: 0x04003532 RID: 13618
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__10_1;

			// Token: 0x04003533 RID: 13619
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__17_0;

			// Token: 0x04003534 RID: 13620
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__27_0;

			// Token: 0x04003535 RID: 13621
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__27_1;

			// Token: 0x04003536 RID: 13622
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__35_0;

			// Token: 0x04003537 RID: 13623
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__35_2;

			// Token: 0x04003538 RID: 13624
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__38_0;

			// Token: 0x04003539 RID: 13625
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__38_1;

			// Token: 0x0400353A RID: 13626
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__43_0;
		}

		// Token: 0x02000AB4 RID: 2740
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06005685 RID: 22149 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06005686 RID: 22150 RVA: 0x00414E48 File Offset: 0x00413048
			internal byte[] <MQB_09_RearLightsWhenDayLights>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (mqb_LightConfiguration.FunctionC.Value == "00" || mqb_LightConfiguration.FunctionC.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertCD = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionD.Value == "00" || mqb_LightConfiguration.FunctionD.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertCD = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionE.Value == "00" || mqb_LightConfiguration.FunctionE.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertEF = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionF.Value == "00" || mqb_LightConfiguration.FunctionF.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionF = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertEF = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionG.Value == "00" || mqb_LightConfiguration.FunctionG.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertGH = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionH.Value == "00" || mqb_LightConfiguration.FunctionH.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionH = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertGH = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
				}
				else if (mqb_LightConfiguration.FunctionC.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionD.Value == "00")
					{
						mqb_LightConfiguration.DimmwertCD = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionD.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionD = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionC.Value == "00")
					{
						mqb_LightConfiguration.DimmwertCD = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionE.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionF.Value == "00")
					{
						mqb_LightConfiguration.DimmwertEF = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionF.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionF = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionE.Value == "00")
					{
						mqb_LightConfiguration.DimmwertEF = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionG.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionH.Value == "00")
					{
						mqb_LightConfiguration.DimmwertGH = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionH.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionH = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionG.Value == "00")
					{
						mqb_LightConfiguration.DimmwertGH = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				return array;
			}

			// Token: 0x06005687 RID: 22151 RVA: 0x00415250 File Offset: 0x00413450
			internal string <MQB_09_RearLightsWhenDayLights>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(data);
				if (mqb_LightConfiguration.FunctionC.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionD.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionE.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionF.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionG.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionH.Value == this.DRL_FUNC)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005688 RID: 22152 RVA: 0x0041530C File Offset: 0x0041350C
			internal byte[] <MQB_09_RearLightsWhenDayLights>b__2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					if (mqb_LightConfiguration.FunctionC.Value == "00" || mqb_LightConfiguration.FunctionC.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertCD = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionD.Value == "00" || mqb_LightConfiguration.FunctionD.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertCD = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionE.Value == "00" || mqb_LightConfiguration.FunctionE.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertEF = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionF.Value == "00" || mqb_LightConfiguration.FunctionF.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionF = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertEF = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionG.Value == "00" || mqb_LightConfiguration.FunctionG.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertGH = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
					else if (mqb_LightConfiguration.FunctionH.Value == "00" || mqb_LightConfiguration.FunctionH.Value == this.DRL_FUNC)
					{
						mqb_LightConfiguration.FunctionH = MQB_LightFunction.FromValue(this.DRL_FUNC);
						mqb_LightConfiguration.DimmwertGH = 70;
						array = mqb_LightConfiguration.ApplyToData();
					}
				}
				else if (mqb_LightConfiguration.FunctionC.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionC = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionD.Value == "00")
					{
						mqb_LightConfiguration.DimmwertCD = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionD.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionD = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionC.Value == "00")
					{
						mqb_LightConfiguration.DimmwertCD = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionE.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionF.Value == "00")
					{
						mqb_LightConfiguration.DimmwertEF = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionF.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionF = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionE.Value == "00")
					{
						mqb_LightConfiguration.DimmwertEF = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionG.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionH.Value == "00")
					{
						mqb_LightConfiguration.DimmwertGH = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				else if (mqb_LightConfiguration.FunctionH.Value == this.DRL_FUNC)
				{
					mqb_LightConfiguration.FunctionH = MQB_LightFunction.FromValue(0);
					if (mqb_LightConfiguration.FunctionG.Value == "00")
					{
						mqb_LightConfiguration.DimmwertGH = 0;
					}
					array = mqb_LightConfiguration.ApplyToData();
				}
				return array;
			}

			// Token: 0x06005689 RID: 22153 RVA: 0x00415714 File Offset: 0x00413914
			internal string <MQB_09_RearLightsWhenDayLights>b__3(byte[] data, MQBEasyCodingItem codingItem)
			{
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(data);
				if (mqb_LightConfiguration.FunctionC.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionD.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionE.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionF.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionG.Value == this.DRL_FUNC || mqb_LightConfiguration.FunctionH.Value == this.DRL_FUNC)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0400353B RID: 13627
			public string DRL_FUNC;
		}

		// Token: 0x02000AB5 RID: 2741
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x0600568A RID: 22154 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x0600568B RID: 22155 RVA: 0x004157D0 File Offset: 0x004139D0
			internal byte[] <SkodaKodiaqDRLLigthsWithStripes>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.func_Tagfahrlicht;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.off_func;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0600568C RID: 22156 RVA: 0x0041584C File Offset: 0x00413A4C
			internal byte[] <SkodaKodiaqDRLLigthsWithStripes>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.func_Tagfahrlicht;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.off_func;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0400353C RID: 13628
			public MQB_LightFunction func_Tagfahrlicht;

			// Token: 0x0400353D RID: 13629
			public MQB_LightFunction off_func;
		}

		// Token: 0x02000AB6 RID: 2742
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x0600568D RID: 22157 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x0600568E RID: 22158 RVA: 0x004158C8 File Offset: 0x00413AC8
			internal byte[] <MQB_09_Kodiaq_RearSideLightAudiStyle>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.left_blink_func;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration.DimmwertAB = 35;
					mqb_LightConfiguration.DimmwertEF = 5;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.DimmwertEF = 0;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0600568F RID: 22159 RVA: 0x00415958 File Offset: 0x00413B58
			internal byte[] <MQB_09_Kodiaq_RearSideLightAudiStyle>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.right_blink_func;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration.DimmwertAB = 35;
					mqb_LightConfiguration.DimmwertEF = 5;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.DimmwertEF = 0;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0400353E RID: 13630
			public MQB_LightFunction left_blink_func;

			// Token: 0x0400353F RID: 13631
			public MQB_LightFunction right_blink_func;
		}

		// Token: 0x02000AB7 RID: 2743
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x06005690 RID: 22160 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x06005691 RID: 22161 RVA: 0x004159E8 File Offset: 0x00413BE8
			internal byte[] <BlinkFrontTurnLightsWithDRL>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.LEFT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x06005692 RID: 22162 RVA: 0x00415A68 File Offset: 0x00413C68
			internal byte[] <BlinkFrontTurnLightsWithDRL>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.RIGHT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003540 RID: 13632
			public MQB_LightFunction LEFT_FUNC;

			// Token: 0x04003541 RID: 13633
			public MQB_LightFunction RIGHT_FUNC;
		}

		// Token: 0x02000AB8 RID: 2744
		[CompilerGenerated]
		private sealed class <>c__DisplayClass14_0
		{
			// Token: 0x06005693 RID: 22163 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass14_0()
			{
			}

			// Token: 0x06005694 RID: 22164 RVA: 0x00415AE8 File Offset: 0x00413CE8
			internal byte[] <DimmDRLWhenFrontTurnLight_AudiStyle>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.LEFT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 35;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x06005695 RID: 22165 RVA: 0x00415B68 File Offset: 0x00413D68
			internal byte[] <DimmDRLWhenFrontTurnLight_AudiStyle>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.RIGHT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 35;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue("00");
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003542 RID: 13634
			public MQB_LightFunction LEFT_FUNC;

			// Token: 0x04003543 RID: 13635
			public MQB_LightFunction RIGHT_FUNC;
		}

		// Token: 0x02000AB9 RID: 2745
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x06005696 RID: 22166 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x06005697 RID: 22167 RVA: 0x00415BE8 File Offset: 0x00413DE8
			internal byte[] <CornerRegulation>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == this.ece_r48.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == this.ece_r119.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == this.ece_r119.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
				}
				else if (value == this.sae_j582.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
				}
				return array;
			}

			// Token: 0x06005698 RID: 22168 RVA: 0x00415D00 File Offset: 0x00413F00
			internal string <CornerRegulation>b__1(byte[] data, MQBAlternativeCoding codingItem)
			{
				string text = Convert.ToString((int)(data[1] & 7), 2);
				while (text.Length < 3)
				{
					text = "0" + text;
				}
				if (text.EndsWith(this.off.Value))
				{
					return this.off.Title;
				}
				if (text.EndsWith(this.ece_r48.Value))
				{
					return this.ece_r48.Title;
				}
				if (text.EndsWith(this.ece_r119.Value))
				{
					return this.ece_r119.Title;
				}
				if (text.EndsWith(this.fmvss_517_108.Value))
				{
					return this.fmvss_517_108.Title;
				}
				if (text.EndsWith(this.sae_j582.Value))
				{
					return this.sae_j582.Title;
				}
				return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
			}

			// Token: 0x06005699 RID: 22169 RVA: 0x00415DD0 File Offset: 0x00413FD0
			internal byte[] <CornerRegulation>b__2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
				}
				else if (value == this.ece_r48.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
				}
				else if (value == this.ece_r119.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
				}
				else if (value == this.ece_r119.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, true);
					BitHelpers.SwitchBitInByte(array, 2, 5, false);
				}
				else if (value == this.sae_j582.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 3, false);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 5, true);
				}
				return array;
			}

			// Token: 0x0600569A RID: 22170 RVA: 0x00415EE8 File Offset: 0x004140E8
			internal string <CornerRegulation>b__3(byte[] data, MQBAlternativeCoding codingItem)
			{
				string text = Convert.ToString((data[2] >> 3) & 7, 2);
				while (text.Length < 3)
				{
					text = "0" + text;
				}
				if (text.EndsWith(this.off.Value))
				{
					return this.off.Title;
				}
				if (text.EndsWith(this.ece_r48.Value))
				{
					return this.ece_r48.Title;
				}
				if (text.EndsWith(this.ece_r119.Value))
				{
					return this.ece_r119.Title;
				}
				if (text.EndsWith(this.fmvss_517_108.Value))
				{
					return this.fmvss_517_108.Title;
				}
				if (text.EndsWith(this.sae_j582.Value))
				{
					return this.sae_j582.Title;
				}
				return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
			}

			// Token: 0x04003544 RID: 13636
			public MQBAdaptationOption off;

			// Token: 0x04003545 RID: 13637
			public MQBAdaptationOption ece_r48;

			// Token: 0x04003546 RID: 13638
			public MQBAdaptationOption ece_r119;

			// Token: 0x04003547 RID: 13639
			public MQBAdaptationOption sae_j582;

			// Token: 0x04003548 RID: 13640
			public MQBAdaptationOption fmvss_517_108;
		}

		// Token: 0x02000ABA RID: 2746
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x0600569B RID: 22171 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x0600569C RID: 22172 RVA: 0x00415FBC File Offset: 0x004141BC
			internal byte[] <OctaviaA7_SecondsRearFogLight>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionA = this.rear_fog_func;
					mqb_LightConfiguration.DimmwertAB = 100;
					if (mqb_LightConfiguration.LampType.Value == "00")
					{
						mqb_LightConfiguration.LampType = MQB_LampType.FromValue(9);
					}
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0600569D RID: 22173 RVA: 0x00416038 File Offset: 0x00414238
			internal byte[] <OctaviaA7_SecondsRearFogLight>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionA = this.rear_fog_func;
					mqb_LightConfiguration.DimmwertAB = 100;
					if (mqb_LightConfiguration.LampType.Value == "00")
					{
						mqb_LightConfiguration.LampType = MQB_LampType.FromValue(9);
					}
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003549 RID: 13641
			public MQB_LightFunction rear_fog_func;
		}

		// Token: 0x02000ABB RID: 2747
		[CompilerGenerated]
		private sealed class <>c__DisplayClass22_0
		{
			// Token: 0x0600569E RID: 22174 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass22_0()
			{
			}

			// Token: 0x0600569F RID: 22175 RVA: 0x004160B4 File Offset: 0x004142B4
			internal byte[] <KodiaqHalogenDisableFrontSideLightWhenHeadlightsEngaged>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.left_lowbeam;
					mqb_LightConfiguration.FunctionF = this.left_highbeam;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = this.off_func;
					mqb_LightConfiguration.FunctionF = this.off_func;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056A0 RID: 22176 RVA: 0x00416140 File Offset: 0x00414340
			internal byte[] <KodiaqHalogenDisableFrontSideLightWhenHeadlightsEngaged>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.right_lowbeam;
					mqb_LightConfiguration.FunctionF = this.right_highbeam;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = this.off_func;
					mqb_LightConfiguration.FunctionF = this.off_func;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0400354A RID: 13642
			public MQB_LightFunction left_lowbeam;

			// Token: 0x0400354B RID: 13643
			public MQB_LightFunction left_highbeam;

			// Token: 0x0400354C RID: 13644
			public MQB_LightFunction off_func;

			// Token: 0x0400354D RID: 13645
			public MQB_LightFunction right_lowbeam;

			// Token: 0x0400354E RID: 13646
			public MQB_LightFunction right_highbeam;
		}

		// Token: 0x02000ABC RID: 2748
		[CompilerGenerated]
		private sealed class <>c__DisplayClass23_0
		{
			// Token: 0x060056A1 RID: 22177 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass23_0()
			{
			}

			// Token: 0x060056A2 RID: 22178 RVA: 0x004161CC File Offset: 0x004143CC
			internal byte[] <KodiaqBlinkStripesWithTurnLights>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.func_left_turn_dark_phase;
					mqb_LightConfiguration.DimmwertEF = 127;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.func_left_turn_bright_phase;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = this.off_func;
					mqb_LightConfiguration.FunctionG = this.off_func;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056A3 RID: 22179 RVA: 0x0041627C File Offset: 0x0041447C
			internal byte[] <KodiaqBlinkStripesWithTurnLights>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.func_right_turn_dark_phase;
					mqb_LightConfiguration.DimmwertEF = 127;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.func_right_turn_bright_phase;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = this.off_func;
					mqb_LightConfiguration.FunctionG = this.off_func;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0400354F RID: 13647
			public MQB_LightFunction func_left_turn_dark_phase;

			// Token: 0x04003550 RID: 13648
			public MQB_LightFunction func_left_turn_bright_phase;

			// Token: 0x04003551 RID: 13649
			public MQB_LightFunction off_func;

			// Token: 0x04003552 RID: 13650
			public MQB_LightFunction func_right_turn_dark_phase;

			// Token: 0x04003553 RID: 13651
			public MQB_LightFunction func_right_turn_bright_phase;
		}

		// Token: 0x02000ABD RID: 2749
		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_0
		{
			// Token: 0x060056A4 RID: 22180 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass24_0()
			{
			}

			// Token: 0x060056A5 RID: 22181 RVA: 0x0041632C File Offset: 0x0041452C
			internal byte[] <TiguanIIBlinkRearSideLightsWithTurnLights>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.func_right_turn_bright_phase;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration.DimmwertGH = 0;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.off_func;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056A6 RID: 22182 RVA: 0x004163A8 File Offset: 0x004145A8
			internal byte[] <TiguanIIBlinkRearSideLightsWithTurnLights>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.func_left_turn_bright_phase;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration.DimmwertGH = 0;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.off_func;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056A7 RID: 22183 RVA: 0x00416424 File Offset: 0x00414624
			internal byte[] <TiguanIIBlinkRearSideLightsWithTurnLights>b__2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.func_left_turn_bright_phase;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration.DimmwertGH = 0;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.off_func;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056A8 RID: 22184 RVA: 0x004164A0 File Offset: 0x004146A0
			internal byte[] <TiguanIIBlinkRearSideLightsWithTurnLights>b__3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.func_right_turn_bright_phase;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					mqb_LightConfiguration.DimmwertGH = 0;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.off_func;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003554 RID: 13652
			public MQB_LightFunction func_right_turn_bright_phase;

			// Token: 0x04003555 RID: 13653
			public MQB_LightFunction off_func;

			// Token: 0x04003556 RID: 13654
			public MQB_LightFunction func_left_turn_bright_phase;
		}

		// Token: 0x02000ABE RID: 2750
		[CompilerGenerated]
		private sealed class <>c__DisplayClass25_0
		{
			// Token: 0x060056A9 RID: 22185 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass25_0()
			{
			}

			// Token: 0x060056AA RID: 22186 RVA: 0x0041651C File Offset: 0x0041471C
			internal byte[] <Tiguan3DLED_RearLightsWhenDayLights>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056AB RID: 22187 RVA: 0x00416598 File Offset: 0x00414798
			internal byte[] <Tiguan3DLED_RearLightsWhenDayLights>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056AC RID: 22188 RVA: 0x00416614 File Offset: 0x00414814
			internal byte[] <Tiguan3DLED_RearLightsWhenDayLights>b__2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056AD RID: 22189 RVA: 0x00416690 File Offset: 0x00414890
			internal byte[] <Tiguan3DLED_RearLightsWhenDayLights>b__3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003557 RID: 13655
			public MQB_LightFunction DRL_FUNC;

			// Token: 0x04003558 RID: 13656
			public MQB_LightFunction OFF_FUNC;
		}

		// Token: 0x02000ABF RID: 2751
		[CompilerGenerated]
		private sealed class <>c__DisplayClass26_0
		{
			// Token: 0x060056AE RID: 22190 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass26_0()
			{
			}

			// Token: 0x060056AF RID: 22191 RVA: 0x0041670C File Offset: 0x0041490C
			internal byte[] <TiguanBASIC_LED_RearLightsWhenDayLights>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 35;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056B0 RID: 22192 RVA: 0x00416788 File Offset: 0x00414988
			internal byte[] <TiguanBASIC_LED_RearLightsWhenDayLights>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 35;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056B1 RID: 22193 RVA: 0x00416804 File Offset: 0x00414A04
			internal byte[] <TiguanBASIC_LED_RearLightsWhenDayLights>b__2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 35;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056B2 RID: 22194 RVA: 0x00416880 File Offset: 0x00414A80
			internal byte[] <TiguanBASIC_LED_RearLightsWhenDayLights>b__3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionC = this.DRL_FUNC;
					mqb_LightConfiguration.DimmwertCD = 35;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionC = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertCD = 0;
					mqb_LightConfiguration.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003559 RID: 13657
			public MQB_LightFunction DRL_FUNC;

			// Token: 0x0400355A RID: 13658
			public MQB_LightFunction OFF_FUNC;
		}

		// Token: 0x02000AC0 RID: 2752
		[CompilerGenerated]
		private sealed class <>c__DisplayClass28_0
		{
			// Token: 0x060056B3 RID: 22195 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass28_0()
			{
			}

			// Token: 0x060056B4 RID: 22196 RVA: 0x004168FC File Offset: 0x00414AFC
			internal byte[] <SkodaSuperB_MK3_2019>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.DimmwertAB = 100;
					mqb_LightConfiguration.FunctionG = this.LEFT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 25;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.FunctionG = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056B5 RID: 22197 RVA: 0x00416988 File Offset: 0x00414B88
			internal byte[] <SkodaSuperB_MK3_2019>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.DimmwertAB = 100;
					mqb_LightConfiguration.FunctionG = this.RIGHT_FUNC;
					mqb_LightConfiguration.DimmwertGH = 25;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.FunctionG = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0400355B RID: 13659
			public MQB_LightFunction LEFT_FUNC;

			// Token: 0x0400355C RID: 13660
			public MQB_LightFunction OFF_FUNC;

			// Token: 0x0400355D RID: 13661
			public MQB_LightFunction RIGHT_FUNC;
		}

		// Token: 0x02000AC1 RID: 2753
		[CompilerGenerated]
		private sealed class <>c__DisplayClass29_0
		{
			// Token: 0x060056B6 RID: 22198 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass29_0()
			{
			}

			// Token: 0x060056B7 RID: 22199 RVA: 0x00416A14 File Offset: 0x00414C14
			internal byte[] <Tiguan3DLED_RearLightsAudiStyle>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.LEFT_FUNC;
					mqb_LightConfiguration.DimmwertAB = 126;
					mqb_LightConfiguration.DimmwertCD = 126;
					mqb_LightConfiguration.DimmwertGH = 10;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056B8 RID: 22200 RVA: 0x00416AB0 File Offset: 0x00414CB0
			internal byte[] <Tiguan3DLED_RearLightsAudiStyle>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.LEFT_FUNC;
					mqb_LightConfiguration.DimmwertAB = 126;
					mqb_LightConfiguration.DimmwertCD = 126;
					mqb_LightConfiguration.DimmwertGH = 10;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056B9 RID: 22201 RVA: 0x00416B4C File Offset: 0x00414D4C
			internal byte[] <Tiguan3DLED_RearLightsAudiStyle>b__2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.RIGHT_FUNC;
					mqb_LightConfiguration.DimmwertAB = 126;
					mqb_LightConfiguration.DimmwertCD = 126;
					mqb_LightConfiguration.DimmwertGH = 10;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056BA RID: 22202 RVA: 0x00416BE8 File Offset: 0x00414DE8
			internal byte[] <Tiguan3DLED_RearLightsAudiStyle>b__3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.RIGHT_FUNC;
					mqb_LightConfiguration.DimmwertAB = 126;
					mqb_LightConfiguration.DimmwertCD = 126;
					mqb_LightConfiguration.DimmwertGH = 10;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.OFF_FUNC;
					mqb_LightConfiguration.DimmwertAB = 127;
					mqb_LightConfiguration.DimmwertCD = 127;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0400355E RID: 13662
			public MQB_LightFunction LEFT_FUNC;

			// Token: 0x0400355F RID: 13663
			public MQB_LightFunction OFF_FUNC;

			// Token: 0x04003560 RID: 13664
			public MQB_LightFunction RIGHT_FUNC;
		}

		// Token: 0x02000AC2 RID: 2754
		[CompilerGenerated]
		private sealed class <>c__DisplayClass30_0
		{
			// Token: 0x060056BB RID: 22203 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass30_0()
			{
			}

			// Token: 0x060056BC RID: 22204 RVA: 0x00416C84 File Offset: 0x00414E84
			internal byte[] <FogLights_ActivateWithHighBeam>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.FUNC_HIGHBEAM_LEFT;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056BD RID: 22205 RVA: 0x00416D00 File Offset: 0x00414F00
			internal byte[] <FogLights_ActivateWithHighBeam>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.FUNC_HIGHBEAM_RIGHT;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003561 RID: 13665
			public MQB_LightFunction FUNC_HIGHBEAM_LEFT;

			// Token: 0x04003562 RID: 13666
			public MQB_LightFunction FUNC_OFF;

			// Token: 0x04003563 RID: 13667
			public MQB_LightFunction FUNC_HIGHBEAM_RIGHT;
		}

		// Token: 0x02000AC3 RID: 2755
		[CompilerGenerated]
		private sealed class <>c__DisplayClass31_0
		{
			// Token: 0x060056BE RID: 22206 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass31_0()
			{
			}

			// Token: 0x060056BF RID: 22207 RVA: 0x00416D7C File Offset: 0x00414F7C
			internal byte[] <FogLights_BlinkWithHighBeam>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionH = this.FUNC_HIGHBEAM_BLINK;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionH = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056C0 RID: 22208 RVA: 0x00416DF8 File Offset: 0x00414FF8
			internal byte[] <FogLights_BlinkWithHighBeam>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionH = this.FUNC_HIGHBEAM_BLINK;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionH = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003564 RID: 13668
			public MQB_LightFunction FUNC_HIGHBEAM_BLINK;

			// Token: 0x04003565 RID: 13669
			public MQB_LightFunction FUNC_OFF;
		}

		// Token: 0x02000AC4 RID: 2756
		[CompilerGenerated]
		private sealed class <>c__DisplayClass32_0
		{
			// Token: 0x060056C1 RID: 22209 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass32_0()
			{
			}

			// Token: 0x060056C2 RID: 22210 RVA: 0x00416E74 File Offset: 0x00415074
			internal byte[] <FogLights_ActivateAndBlinkWithHighBeam>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.FUNC_HIGHBEAM_LEFT;
					mqb_LightConfiguration.FunctionH = this.FUNC_HIGHBEAM_BLINK;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.FUNC_OFF;
					mqb_LightConfiguration.FunctionH = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056C3 RID: 22211 RVA: 0x00416F08 File Offset: 0x00415108
			internal byte[] <FogLights_ActivateAndBlinkWithHighBeam>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionG = this.FUNC_HIGHBEAM_RIGHT;
					mqb_LightConfiguration.FunctionH = this.FUNC_HIGHBEAM_BLINK;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				else
				{
					mqb_LightConfiguration.FunctionG = this.FUNC_OFF;
					mqb_LightConfiguration.FunctionH = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 100;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003566 RID: 13670
			public MQB_LightFunction FUNC_HIGHBEAM_LEFT;

			// Token: 0x04003567 RID: 13671
			public MQB_LightFunction FUNC_HIGHBEAM_BLINK;

			// Token: 0x04003568 RID: 13672
			public MQB_LightFunction FUNC_OFF;

			// Token: 0x04003569 RID: 13673
			public MQB_LightFunction FUNC_HIGHBEAM_RIGHT;
		}

		// Token: 0x02000AC5 RID: 2757
		[CompilerGenerated]
		private sealed class <>c__DisplayClass34_0
		{
			// Token: 0x060056C4 RID: 22212 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass34_0()
			{
			}

			// Token: 0x060056C5 RID: 22213 RVA: 0x00416F9C File Offset: 0x0041519C
			internal byte[] <TurnLights_USStyleHalogenOnly>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionD = this.FUNC_STANDLICHT_ALLGEMEIN;
					mqb_LightConfiguration.DimmwertCD = 30;
					mqb_LightConfiguration.FunctionE = this.FUNC_BLINKEN_LINKS_DUNKEL;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionD = this.FUNC_OFF;
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056C6 RID: 22214 RVA: 0x00417030 File Offset: 0x00415230
			internal byte[] <TurnLights_USStyleHalogenOnly>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionD = this.FUNC_STANDLICHT_ALLGEMEIN;
					mqb_LightConfiguration.DimmwertCD = 30;
					mqb_LightConfiguration.FunctionE = this.FUNC_BLINKEN_RECHTS_DUNKEL;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionD = this.FUNC_OFF;
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0400356A RID: 13674
			public MQB_LightFunction FUNC_STANDLICHT_ALLGEMEIN;

			// Token: 0x0400356B RID: 13675
			public MQB_LightFunction FUNC_BLINKEN_LINKS_DUNKEL;

			// Token: 0x0400356C RID: 13676
			public MQB_LightFunction FUNC_OFF;

			// Token: 0x0400356D RID: 13677
			public MQB_LightFunction FUNC_BLINKEN_RECHTS_DUNKEL;
		}

		// Token: 0x02000AC6 RID: 2758
		[CompilerGenerated]
		private sealed class <>c__DisplayClass35_0
		{
			// Token: 0x060056C7 RID: 22215 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass35_0()
			{
			}

			// Token: 0x060056C8 RID: 22216 RVA: 0x004170C2 File Offset: 0x004152C2
			internal string <ComingHomeLamps>b__1(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[4], 7))
				{
					return this.foglight.Title;
				}
				return this.lowbeam.Title;
			}

			// Token: 0x060056C9 RID: 22217 RVA: 0x004170E6 File Offset: 0x004152E6
			internal string <ComingHomeLamps>b__3(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[2], 0))
				{
					return this.foglight.Title;
				}
				return this.lowbeam.Title;
			}

			// Token: 0x0400356E RID: 13678
			public MQBAdaptationOption foglight;

			// Token: 0x0400356F RID: 13679
			public MQBAdaptationOption lowbeam;
		}

		// Token: 0x02000AC7 RID: 2759
		[CompilerGenerated]
		private sealed class <>c__DisplayClass36_0
		{
			// Token: 0x060056CA RID: 22218 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass36_0()
			{
			}

			// Token: 0x060056CB RID: 22219 RVA: 0x0041710C File Offset: 0x0041530C
			internal byte[] <LicensePlate_LampsType>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == this.opt5W.Value)
				{
					mqb_LightConfiguration.LampType = this.lamp5W;
					mqb_LightConfiguration.DimmwertAB = 100;
				}
				else if (value == this.optLedLow.Value)
				{
					mqb_LightConfiguration.LampType = this.ledLow;
					mqb_LightConfiguration.DimmwertAB = 127;
				}
				else if (value == this.optLedGen.Value)
				{
					mqb_LightConfiguration.LampType = this.ledGeneral;
					mqb_LightConfiguration.DimmwertAB = 127;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003570 RID: 13680
			public MQBAdaptationOption opt5W;

			// Token: 0x04003571 RID: 13681
			public MQB_LampType lamp5W;

			// Token: 0x04003572 RID: 13682
			public MQBAdaptationOption optLedLow;

			// Token: 0x04003573 RID: 13683
			public MQB_LampType ledLow;

			// Token: 0x04003574 RID: 13684
			public MQBAdaptationOption optLedGen;

			// Token: 0x04003575 RID: 13685
			public MQB_LampType ledGeneral;
		}

		// Token: 0x02000AC8 RID: 2760
		[CompilerGenerated]
		private sealed class <>c__DisplayClass37_0
		{
			// Token: 0x060056CC RID: 22220 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass37_0()
			{
			}

			// Token: 0x060056CD RID: 22221 RVA: 0x004171B8 File Offset: 0x004153B8
			internal byte[] <SeatLeon5F_BlinkRearSideLightWithTurnSignals>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.FUNC_LEFT_DUNKEL;
					mqb_LightConfiguration.DimmwertEF = 100;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.FUNC_LEFT_HELLPHASE;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056CE RID: 22222 RVA: 0x00417268 File Offset: 0x00415468
			internal byte[] <SeatLeon5F_BlinkRearSideLightWithTurnSignals>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.FUNC_RIGHT_DUNKEL;
					mqb_LightConfiguration.DimmwertEF = 100;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.FUNC_RIGHT_HELLPHASE;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056CF RID: 22223 RVA: 0x00417318 File Offset: 0x00415518
			internal byte[] <SeatLeon5F_BlinkRearSideLightWithTurnSignals>b__2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.FUNC_LEFT_DUNKEL;
					mqb_LightConfiguration.DimmwertEF = 100;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.FUNC_LEFT_HELLPHASE;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056D0 RID: 22224 RVA: 0x004173C8 File Offset: 0x004155C8
			internal byte[] <SeatLeon5F_BlinkRearSideLightWithTurnSignals>b__3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.FunctionE = this.FUNC_RIGHT_DUNKEL;
					mqb_LightConfiguration.DimmwertEF = 100;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.FUNC_RIGHT_HELLPHASE;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				}
				else
				{
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionG = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x04003576 RID: 13686
			public MQB_LightFunction FUNC_LEFT_DUNKEL;

			// Token: 0x04003577 RID: 13687
			public MQB_LightFunction FUNC_LEFT_HELLPHASE;

			// Token: 0x04003578 RID: 13688
			public MQB_LightFunction FUNC_OFF;

			// Token: 0x04003579 RID: 13689
			public MQB_LightFunction FUNC_RIGHT_DUNKEL;

			// Token: 0x0400357A RID: 13690
			public MQB_LightFunction FUNC_RIGHT_HELLPHASE;
		}

		// Token: 0x02000AC9 RID: 2761
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_0
		{
			// Token: 0x060056D1 RID: 22225 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_0()
			{
			}

			// Token: 0x060056D2 RID: 22226 RVA: 0x00417478 File Offset: 0x00415678
			internal byte[] <ComingHomeActivation>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 5, false);
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
				}
				else if (value == this.opt_manual.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 5, false);
					BitHelpers.SwitchBitInByte(array, 4, 4, true);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, true);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					array[9] = 116;
					array[5] = 60;
				}
				else if (value == this.opt_auto.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 5, true);
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
					BitHelpers.SwitchBitInByte(array, 7, 1, false);
					BitHelpers.SwitchBitInByte(array, 7, 2, true);
					BitHelpers.SwitchBitInByte(array, 7, 3, false);
					array[9] = 116;
					array[5] = 60;
				}
				return array;
			}

			// Token: 0x060056D3 RID: 22227 RVA: 0x00417560 File Offset: 0x00415760
			internal byte[] <ComingHomeActivation>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_off.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
				}
				else if (value == this.opt_manual.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
				}
				else if (value == this.opt_auto.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
				}
				return array;
			}

			// Token: 0x0400357B RID: 13691
			public MQBAdaptationOption opt_off;

			// Token: 0x0400357C RID: 13692
			public MQBAdaptationOption opt_manual;

			// Token: 0x0400357D RID: 13693
			public MQBAdaptationOption opt_auto;
		}

		// Token: 0x02000ACA RID: 2762
		[CompilerGenerated]
		private sealed class <>c__DisplayClass41_0
		{
			// Token: 0x060056D4 RID: 22228 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass41_0()
			{
			}

			// Token: 0x060056D5 RID: 22229 RVA: 0x004175F8 File Offset: 0x004157F8
			internal byte[] <LightSensorSensivity>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_sens.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == this.opt_norm.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == this.opt_nonsens.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				return array;
			}

			// Token: 0x060056D6 RID: 22230 RVA: 0x00417690 File Offset: 0x00415890
			internal byte[] <LightSensorSensivity>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_sens.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				else if (value == this.opt_norm.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				else if (value == this.opt_nonsens.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
				}
				return array;
			}

			// Token: 0x0400357E RID: 13694
			public MQBAdaptationOption opt_sens;

			// Token: 0x0400357F RID: 13695
			public MQBAdaptationOption opt_norm;

			// Token: 0x04003580 RID: 13696
			public MQBAdaptationOption opt_nonsens;
		}

		// Token: 0x02000ACB RID: 2763
		[CompilerGenerated]
		private sealed class <>c__DisplayClass42_0
		{
			// Token: 0x060056D7 RID: 22231 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass42_0()
			{
			}

			// Token: 0x060056D8 RID: 22232 RVA: 0x00417728 File Offset: 0x00415928
			internal byte[] <LightSensorInstalled>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_none.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				else if (value == this.opt_diskret.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				else if (value == this.opt_lin.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				return array;
			}

			// Token: 0x060056D9 RID: 22233 RVA: 0x004177C0 File Offset: 0x004159C0
			internal byte[] <LightSensorInstalled>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_none.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, false);
				}
				else if (value == this.opt_diskret.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 0, true);
					BitHelpers.SwitchBitInByte(array, 6, 1, false);
				}
				else if (value == this.opt_lin.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, true);
				}
				return array;
			}

			// Token: 0x04003581 RID: 13697
			public MQBAdaptationOption opt_none;

			// Token: 0x04003582 RID: 13698
			public MQBAdaptationOption opt_diskret;

			// Token: 0x04003583 RID: 13699
			public MQBAdaptationOption opt_lin;
		}

		// Token: 0x02000ACC RID: 2764
		[CompilerGenerated]
		private sealed class <>c__DisplayClass44_0
		{
			// Token: 0x060056DA RID: 22234 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass44_0()
			{
			}

			// Token: 0x060056DB RID: 22235 RVA: 0x00417858 File Offset: 0x00415A58
			internal byte[] <DRL_Mode>b__0(byte[] data, string value, MQBAlternativeCoding codingInternal)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt1.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				else if (value == this.opt2_daytime_running_lamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
				}
				else if (value == this.opt3_daytime_running_lights.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 1, true);
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				else if (value == this.opt2_daytime_running_lamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
				}
				return array;
			}

			// Token: 0x060056DC RID: 22236 RVA: 0x00417918 File Offset: 0x00415B18
			internal byte[] <DRL_Mode>b__1(byte[] data, string value, MQBAlternativeCoding codingInternal)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt1.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
				}
				else if (value == this.opt2_daytime_running_lamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
				}
				else if (value == this.opt3_daytime_running_lights.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
				}
				else if (value == this.opt2_daytime_running_lamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
				}
				return array;
			}

			// Token: 0x04003584 RID: 13700
			public MQBAdaptationOption opt1;

			// Token: 0x04003585 RID: 13701
			public MQBAdaptationOption opt2_daytime_running_lamps;

			// Token: 0x04003586 RID: 13702
			public MQBAdaptationOption opt3_daytime_running_lights;
		}

		// Token: 0x02000ACD RID: 2765
		[CompilerGenerated]
		private sealed class <>c__DisplayClass45_0
		{
			// Token: 0x060056DD RID: 22237 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass45_0()
			{
			}

			// Token: 0x060056DE RID: 22238 RVA: 0x004179D8 File Offset: 0x00415BD8
			internal byte[] <DrivingLightsByDay>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_notActive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
				}
				else if (value == this.opt_DaytimeRunningLamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
				}
				else if (value == this.opt_DaytimeRunningLights.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 1, true);
				}
				else if (value == this.opt_Versehrtenlicht.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 1, true);
				}
				return array;
			}

			// Token: 0x060056DF RID: 22239 RVA: 0x00417A98 File Offset: 0x00415C98
			internal byte[] <DrivingLightsByDay>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_notActive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == this.opt_DaytimeRunningLamps.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
				}
				else if (value == this.opt_DaytimeRunningLights.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				else if (value == this.opt_Versehrtenlicht.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
				}
				return array;
			}

			// Token: 0x04003587 RID: 13703
			public MQBAdaptationOption opt_notActive;

			// Token: 0x04003588 RID: 13704
			public MQBAdaptationOption opt_DaytimeRunningLamps;

			// Token: 0x04003589 RID: 13705
			public MQBAdaptationOption opt_DaytimeRunningLights;

			// Token: 0x0400358A RID: 13706
			public MQBAdaptationOption opt_Versehrtenlicht;
		}

		// Token: 0x02000ACE RID: 2766
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x060056E0 RID: 22240 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x060056E1 RID: 22241 RVA: 0x00417B58 File Offset: 0x00415D58
			internal byte[] <MQB_09_FogLightOnAndBlinkWithHighBeam_LED_Only>b__0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.DimmwertEF = 100;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionD = this.FUNC_FERNLICHT_RECHT;
					mqb_LightConfiguration.FunctionE = this.FUNC_LICHTUPE_GENEREL;
				}
				else
				{
					mqb_LightConfiguration.FunctionD = this.FUNC_OFF;
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056E2 RID: 22242 RVA: 0x00417BEC File Offset: 0x00415DEC
			internal byte[] <MQB_09_FogLightOnAndBlinkWithHighBeam_LED_Only>b__1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.DimmwertEF = 100;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionD = this.FUNC_FERNLICHT_LINKS;
					mqb_LightConfiguration.FunctionE = this.FUNC_LICHTUPE_GENEREL;
				}
				else
				{
					mqb_LightConfiguration.FunctionD = this.FUNC_OFF;
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056E3 RID: 22243 RVA: 0x00417C80 File Offset: 0x00415E80
			internal byte[] <MQB_09_FogLightOnAndBlinkWithHighBeam_LED_Only>b__2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.DimmwertEF = 127;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionD = this.FUNC_FERNLICHT_RECHT;
					mqb_LightConfiguration.FunctionE = this.FUNC_LICHTUPE_GENEREL;
				}
				else
				{
					mqb_LightConfiguration.FunctionD = this.FUNC_OFF;
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x060056E4 RID: 22244 RVA: 0x00417D14 File Offset: 0x00415F14
			internal byte[] <MQB_09_FogLightOnAndBlinkWithHighBeam_LED_Only>b__3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					mqb_LightConfiguration.DimmwertEF = 127;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
					mqb_LightConfiguration.FunctionD = this.FUNC_FERNLICHT_LINKS;
					mqb_LightConfiguration.FunctionE = this.FUNC_LICHTUPE_GENEREL;
				}
				else
				{
					mqb_LightConfiguration.FunctionD = this.FUNC_OFF;
					mqb_LightConfiguration.FunctionE = this.FUNC_OFF;
					mqb_LightConfiguration.DimmwertEF = 0;
					mqb_LightConfiguration.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x0400358B RID: 13707
			public MQB_LightFunction FUNC_FERNLICHT_RECHT;

			// Token: 0x0400358C RID: 13708
			public MQB_LightFunction FUNC_LICHTUPE_GENEREL;

			// Token: 0x0400358D RID: 13709
			public MQB_LightFunction FUNC_OFF;

			// Token: 0x0400358E RID: 13710
			public MQB_LightFunction FUNC_FERNLICHT_LINKS;
		}
	}
}
