using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26
{
	// Token: 0x02000A4C RID: 2636
	internal static class ExteriorLight
	{
		// Token: 0x0600534A RID: 21322 RVA: 0x003FD1CC File Offset: 0x003FB3CC
		public static ICodingContainer PQ26_DisableLicensePlateLightWhenTailgateIsOpened()
		{
			return new MQBEasyCodingItem(CodingGroup.ExteriorLights, "Disable license plate light when tailgate is opened", "", "0569", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
					mqb_LightConfiguration.LoadFromData(array);
					mqb_LightConfiguration.LightControlAB = MQB_LightConfiguration.LightControl.IfBootIsClosed;
					array = mqb_LightConfiguration.ApplyToData();
				}
				else
				{
					MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration();
					mqb_LightConfiguration2.LoadFromData(array);
					mqb_LightConfiguration2.LightControlAB = MQB_LightConfiguration.LightControl.Always;
					array = mqb_LightConfiguration2.ApplyToData();
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				Translations = 
				{
					new TranslationItem("ru", "Отключение освещения номерного знака при открытии багажника", "", "")
				}
			};
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x003FD25C File Offset: 0x003FB45C
		public static ICodingContainer PQ26_BlinkTurnLightsWithLEDDRL()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem(CodingGroup.ExteriorLights, "", "", "0554", "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
					mqb_LightConfiguration.LoadFromData(array);
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue(2);
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					array = mqb_LightConfiguration.ApplyToData();
				}
				else
				{
					MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration();
					mqb_LightConfiguration2.LoadFromData(array);
					mqb_LightConfiguration2.FunctionG = MQB_LightFunction.FromValue(0);
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
					array = mqb_LightConfiguration2.ApplyToData();
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0555", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue(4);
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue(0);
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionG.Value == "04" && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Blink front DRL with turn lights", "PQ26, Confirmed on Skoda Rapid", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyLightCoding })
			{
				Translations = 
				{
					new TranslationItem("ru", "Перемигивание указателей поворота с LED ДХО", "Совместимость: Skoda Rapid 2016-2020", "")
				}
			};
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x003FD374 File Offset: 0x003FB574
		public static ICodingContainer PQ26_Corner()
		{
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("16");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("17");
			MQB_LightFunction disabled_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("055C", delegate(MQB_LightConfiguration config)
			{
				if (config.FunctionB == disabled_func || config.FunctionB == left_func)
				{
					config.FunctionB = left_func;
					if (config.DimmwertAB < 100)
					{
						config.DimmwertAB = 100;
						return;
					}
				}
				else
				{
					if (config.FunctionC == disabled_func || config.FunctionC == left_func)
					{
						config.FunctionD = left_func;
						if (config.DimmwertCD < 100)
						{
							config.DimmwertCD = 100;
						}
						config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
						return;
					}
					if (config.FunctionD == disabled_func || config.FunctionD == left_func)
					{
						config.FunctionD = left_func;
						if (config.DimmwertCD < 100)
						{
							config.DimmwertCD = 100;
						}
						config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
					}
				}
			}, delegate(MQB_LightConfiguration config)
			{
				if (config.FunctionB == left_func)
				{
					config.FunctionB = disabled_func;
				}
				if (config.FunctionC == left_func)
				{
					config.FunctionC = disabled_func;
					if (config.FunctionD == disabled_func)
					{
						config.DimmwertCD = 0;
					}
				}
				if (config.FunctionD == left_func)
				{
					config.FunctionD = disabled_func;
				}
				config.FunctionD = MQB_LightFunction.FromValue(0);
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("055D", delegate(MQB_LightConfiguration config)
			{
				if (config.FunctionB == disabled_func || config.FunctionB == right_func)
				{
					config.FunctionB = right_func;
					if (config.DimmwertAB < 100)
					{
						config.DimmwertAB = 100;
						return;
					}
				}
				else
				{
					if (config.FunctionC == disabled_func || config.FunctionC == right_func)
					{
						config.FunctionD = right_func;
						if (config.DimmwertCD < 100)
						{
							config.DimmwertCD = 100;
						}
						config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
						return;
					}
					if (config.FunctionD == disabled_func || config.FunctionD == right_func)
					{
						config.FunctionD = right_func;
						if (config.DimmwertCD < 100)
						{
							config.DimmwertCD = 100;
						}
						config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
					}
				}
			}, delegate(MQB_LightConfiguration config)
			{
				if (config.FunctionB == right_func)
				{
					config.FunctionB = disabled_func;
				}
				if (config.FunctionC == right_func)
				{
					config.FunctionC = disabled_func;
					if (config.FunctionD == disabled_func)
					{
						config.DimmwertCD = 0;
					}
				}
				if (config.FunctionD == right_func)
				{
					config.FunctionD = disabled_func;
				}
				config.FunctionD = MQB_LightFunction.FromValue(0);
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Corner function with front fog lights", "PQ26, Confirmed on Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Функция CORNER (подсвечивание поворотов противотуманными фарами)", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x003FD442 File Offset: 0x003FB642
		public static ICodingContainer CornerRegulation()
		{
			return ExteriorLights.CornerRegulation();
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x003FD44C File Offset: 0x003FB64C
		public static ICodingContainer PQ26_RearSideLightsAsDRL()
		{
			MQB_LightFunction drl_func = MQB_LightFunction.FromValue("14");
			MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0567", delegate(MQB_LightConfiguration config)
			{
				config.FunctionD = drl_func;
				config.DimmwertCD = 75;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionD = MQB_LightFunction.FromValue("00");
				config.DimmwertCD = 75;
			}, (MQB_LightConfiguration config) => config.FunctionD == drl_func && config.DimmwertCD == 75);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0568", delegate(MQB_LightConfiguration config)
			{
				config.FunctionD = drl_func;
				config.DimmwertCD = 75;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionD = MQB_LightFunction.FromValue("00");
				config.DimmwertCD = 75;
			}, (MQB_LightConfiguration config) => config.FunctionD == drl_func && config.DimmwertCD == 75);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Rear side lights active with DRL", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Включение задних габаритов в режиме ДХО (без подсветки номера и приборки)", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x003FD544 File Offset: 0x003FB744
		public static ICodingContainer PQ26_StroboscopeEffectFogLightHighBeam()
		{
			MQB_LightFunction headlight_func = MQB_LightFunction.FromValue("0F");
			MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("055C", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = headlight_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionG == headlight_func && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("055D", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = headlight_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionG == headlight_func && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Stroboscope effect when blinking high beam + front fog lights", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Стробоскоп: ПТФ – Дальний свет (при моргании дальним)", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x003FD63C File Offset: 0x003FB83C
		public static ICodingContainer PQ26_FogLightsWithHighBeam()
		{
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("0D");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("0E");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("055C", delegate(MQB_LightConfiguration config)
			{
				config.FunctionH = left_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				config.DimmwertGH = 100;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionH = off_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.DimmwertGH = 0;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("055D", delegate(MQB_LightConfiguration config)
			{
				config.FunctionH = right_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				config.DimmwertGH = 100;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionH = off_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.DimmwertGH = 0;
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Activate fog lights with high beam", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Противотуманные фары включаются вместе с дальним светом", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x003FD70C File Offset: 0x003FB90C
		public static ICodingContainer PQ26_StroboscopeEffectLowBeamHighBeam()
		{
			MQB_LightFunction headlight_func = MQB_LightFunction.FromValue("0F");
			MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0556", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = headlight_func;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionC == headlight_func && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Minimize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0557", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = headlight_func;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionC == headlight_func && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Minimize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Stroboscope effect when blinking high beam + low beam", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Стробоскоп: Ближний свет – Дальний свет (при моргании дальним)", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x003FD804 File Offset: 0x003FBA04
		public static ICodingContainer PQ26_StroboscopeEffectLEDDRLHighBeam()
		{
			MQB_LightFunction headlight_func = MQB_LightFunction.FromValue("0F");
			MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0554", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = headlight_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionG == headlight_func && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0555", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = headlight_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionG == headlight_func && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Stroboscope effect when blinking high beam + LED DRL", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Стробоскоп: LED ДХО – Дальний свет (при моргании дальним)", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x003FD8F9 File Offset: 0x003FBAF9
		public static ICodingContainer CornerUpperSpeedThreshold()
		{
			return ExteriorLights.CornerUpperSpeedThreshold();
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x003FD900 File Offset: 0x003FBB00
		public static ICodingContainer PQ26_LEDDRL_WithLowBeam()
		{
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("0B");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("0C");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0554", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = left_func;
				config.DimmwertCD = 127;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = off_func;
				config.DimmwertCD = 0;
			}, (MQB_LightConfiguration config) => config.FunctionG == left_func && config.DimmwertCD == 127);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0555", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = right_func;
				config.DimmwertCD = 127;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = off_func;
				config.DimmwertCD = 0;
			}, (MQB_LightConfiguration config) => config.FunctionG == right_func && config.DimmwertCD == 127);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "LED DRL working with low beam", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "ДХО (LED) совместно с фарами ближнего света", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x003FD9E4 File Offset: 0x003FBBE4
		public static ICodingContainer PQ26_DimmingDRLWhenTurnLightsOn()
		{
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("3C");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("3D");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0554", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = left_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = off_func;
				config.DimmwertCD = 0;
			}, (MQB_LightConfiguration config) => config.FunctionG == left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0555", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = right_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = off_func;
				config.DimmwertCD = 0;
			}, (MQB_LightConfiguration config) => config.FunctionG == left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "LED DRL dimming when turn signal flashing", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Притухание ДХО при включении указателя поворота (на соответствующей стороне)", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x003FDAC8 File Offset: 0x003FBCC8
		public static ICodingContainer PQ26_BlinkDRLWithTurnLights()
		{
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("03");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("05");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0554", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = left_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = off_func;
				config.DimmwertCD = 0;
			}, (MQB_LightConfiguration config) => config.FunctionG == left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0555", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = right_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = off_func;
				config.DimmwertCD = 0;
			}, (MQB_LightConfiguration config) => config.FunctionG == left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Blink DRL with turn signal", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Мигание ДХО вместе с указателем поворота", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x003FDBAC File Offset: 0x003FBDAC
		public static ICodingContainer PQ26_FrontSideLightDimmingWithTurnLights()
		{
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("3C");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("3D");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0552", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = left_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = off_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionE == left_func && config.DimmwertEF == 0 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Minimize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0553", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = right_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = off_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionE == right_func && config.DimmwertEF == 0 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Minimize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Front side light dimming when turn signal blinking", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Притухание передних габаритов на стороне работающего указателя поворота", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x003FDC90 File Offset: 0x003FBE90
		public static ICodingContainer PQ26_USStandlichts()
		{
			MQB_LightFunction standlicht = MQB_LightFunction.FromValue("08");
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("03");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("05");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0550", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = standlicht;
				config.DimmwertCD = 35;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.FunctionE = left_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.FunctionE = off_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionC == standlicht && config.DimmwertCD == 35 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Maximize && config.FunctionE == left_func && config.DimmwertEF == 0 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Minimize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0551", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = standlicht;
				config.DimmwertCD = 35;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.FunctionE = right_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.FunctionE = off_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionC == standlicht && config.DimmwertCD == 35 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Maximize && config.FunctionE == right_func && config.DimmwertEF == 0 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Minimize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "US-style sidelight with turn signals", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Габаритные огни через указатели поворота в половину накала (американский стиль)", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x003FDD84 File Offset: 0x003FBF84
		public static ICodingContainer PQ26_USStandlichtsOnlyWhenLightInSideLightsPosition()
		{
			MQB_LightFunction standlicht_vorn = MQB_LightFunction.FromValue("1F");
			MQB_LightFunction left_Abblendlicht = MQB_LightFunction.FromValue("0B");
			MQB_LightFunction left_blinken_dunkel = MQB_LightFunction.FromValue("03");
			MQB_LightFunction left_blinken_hell = MQB_LightFunction.FromValue("02");
			MQB_LightFunction right_Abblendlicht = MQB_LightFunction.FromValue("0C");
			MQB_LightFunction right_blinken_dunkel = MQB_LightFunction.FromValue("05");
			MQB_LightFunction right_blinken_hell = MQB_LightFunction.FromValue("04");
			MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0550", delegate(MQB_LightConfiguration config)
			{
				config.FunctionA = standlicht_vorn;
				config.DimmwertAB = 35;
				config.FunctionC = left_Abblendlicht;
				config.FunctionD = left_blinken_dunkel;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
				config.FunctionE = left_blinken_hell;
				config.DimmwertEF = 100;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(BitHelpers.ConvertHexToBytesX("0C340014020064000000000000000000"));
				config.FunctionA = mqb_LightConfiguration.FunctionA;
				config.DimmwertAB = mqb_LightConfiguration.DimmwertAB;
				config.FunctionC = mqb_LightConfiguration.FunctionC;
				config.FunctionD = mqb_LightConfiguration.FunctionD;
				config.DimmwertCD = mqb_LightConfiguration.DimmwertCD;
				config.DimmingDirectionCD = mqb_LightConfiguration.DimmingDirectionCD;
				config.FunctionE = mqb_LightConfiguration.FunctionE;
				config.DimmwertEF = mqb_LightConfiguration.DimmwertEF;
				config.DimmingDirectionEF = mqb_LightConfiguration.DimmingDirectionEF;
			}, (MQB_LightConfiguration config) => config.FunctionA == standlicht_vorn && config.DimmwertAB == 35 && config.FunctionC == left_Abblendlicht && config.FunctionD == left_blinken_dunkel && config.DimmwertCD == 0 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Minimize && config.FunctionE == left_blinken_hell && config.DimmwertEF == 100 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Maximize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0551", delegate(MQB_LightConfiguration config)
			{
				config.FunctionA = standlicht_vorn;
				config.DimmwertAB = 35;
				config.FunctionC = right_Abblendlicht;
				config.FunctionD = right_blinken_dunkel;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
				config.FunctionE = right_blinken_hell;
				config.DimmwertEF = 100;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(BitHelpers.ConvertHexToBytesX("0C3E0015040064000000000000000000"));
				config.FunctionA = mqb_LightConfiguration2.FunctionA;
				config.DimmwertAB = mqb_LightConfiguration2.DimmwertAB;
				config.FunctionC = mqb_LightConfiguration2.FunctionC;
				config.FunctionD = mqb_LightConfiguration2.FunctionD;
				config.DimmwertCD = mqb_LightConfiguration2.DimmwertCD;
				config.DimmingDirectionCD = mqb_LightConfiguration2.DimmingDirectionCD;
				config.FunctionE = mqb_LightConfiguration2.FunctionE;
				config.DimmwertEF = mqb_LightConfiguration2.DimmwertEF;
				config.DimmingDirectionEF = mqb_LightConfiguration2.DimmingDirectionEF;
			}, (MQB_LightConfiguration config) => config.FunctionA == standlicht_vorn && config.DimmwertAB == 35 && config.FunctionC == right_Abblendlicht && config.FunctionD == right_blinken_dunkel && config.DimmwertCD == 0 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Minimize && config.FunctionE == right_blinken_hell && config.DimmwertEF == 100 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Maximize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "US-style sidelight with turn signals (only when light switch in side lights position)", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Габаритные огни через указатели поворота в половину накала (американский стиль, только при положении переключателя света - габаритные огни)", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x003FDEDC File Offset: 0x003FC0DC
		public static ICodingContainer PQ26_BlinkFrontSideLightsWithTurnLights()
		{
			MQB_LightFunction standlicht_vorn = MQB_LightFunction.FromValue("1F");
			MQB_LightFunction left_blinken_dunkel = MQB_LightFunction.FromValue("03");
			MQB_LightFunction blinken_links_aktiv = MQB_LightFunction.FromValue("06");
			MQB_LightFunction right_blinken_dunkel = MQB_LightFunction.FromValue("05");
			MQB_LightFunction blinken_rechts_aktiv = MQB_LightFunction.FromValue("07");
			MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0552", delegate(MQB_LightConfiguration config)
			{
				config.FunctionA = standlicht_vorn;
				config.FunctionE = left_blinken_dunkel;
				config.FunctionF = blinken_rechts_aktiv;
				config.DimmwertEF = 100;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
			}, (MQB_LightConfiguration config) => config.FunctionA == standlicht_vorn && config.FunctionE == left_blinken_dunkel && config.FunctionF == blinken_rechts_aktiv && config.DimmwertEF == 100 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Maximize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0553", delegate(MQB_LightConfiguration config)
			{
				config.FunctionA = standlicht_vorn;
				config.FunctionE = right_blinken_dunkel;
				config.FunctionF = blinken_links_aktiv;
				config.DimmwertEF = 100;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
			}, (MQB_LightConfiguration config) => config.FunctionA == standlicht_vorn && config.FunctionE == right_blinken_dunkel && config.FunctionF == blinken_links_aktiv && config.DimmwertEF == 100 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Maximize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Blink front side lights with turn lights + front side lights turns off when low beam active", "PQ26: Skoda Rapid", true, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Перемигивание передних габаритов в противофазе с указателями поворотов (во время перемигивания на одной стороне — на другой стороне габарит горит) + Гашение передних габаритов при включении ближнего/дальнего света фар", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x003FE014 File Offset: 0x003FC214
		public static ICodingContainer PQ26_TurnOnStopSignalWhenDoorOpened()
		{
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("38");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("39");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0564", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = left_func;
				config.DimmwertCD = 100;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionC == left_func && config.DimmwertCD == 100 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Maximize);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0565", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = right_func;
				config.DimmwertCD = 100;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionC == right_func && config.DimmwertCD == 100 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Maximize);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Turn on stop lights on the side of opened door", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Включение стоп-сигнала со стороны открытой двери ", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x003FE0F8 File Offset: 0x003FC2F8
		public static ICodingContainer PQ26_TurnOnSideTurnlightsWhenTailgateOpened()
		{
			MQB_LightFunction open_func = MQB_LightFunction.FromValue("21");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0580", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = open_func;
				config.DimmwertCD = 50;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
			}, (MQB_LightConfiguration config) => config.FunctionC == open_func && config.DimmwertCD == 50);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0582", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = open_func;
				config.DimmwertCD = 50;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
			}, (MQB_LightConfiguration config) => config.FunctionC == open_func && config.DimmwertCD == 50);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Turning on side direction indicators when opening the boot", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Включение боковых указателей поворота при открытии багажника", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x003FE1CC File Offset: 0x003FC3CC
		public static ICodingContainer TurnOffDRLWhenParkingBrakeOn()
		{
			return ExteriorLights.TurnOffDRLWhenParkingBrakeOn();
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x003FE1D3 File Offset: 0x003FC3D3
		public static ICodingContainer TurnOffDRLWhenLightSwitchOff()
		{
			return ExteriorLights.TurnOffDRLWhenLightSwitchOff();
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x003FE1DA File Offset: 0x003FC3DA
		public static ICodingContainer DRLSteupInMMI()
		{
			return ExteriorLights.DRLSteupInMMI();
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x003FE1E4 File Offset: 0x003FC3E4
		public static ICodingContainer LimitMaxFrontLights()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A58", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[0], 2))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D01", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 0, false);
				}
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[1], 0))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			});
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, "Limit the maximum number of simultaneously burning front lights to 2 types", "This is often used for stroboscope effect", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Ограничить максимальное количество одновременно горящих огней спереди до 2 видов", "Используется для создания эффекта стробоскопа", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x003FE2E0 File Offset: 0x003FC4E0
		public static ICodingContainer ComingHomeWithLightSensor()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A57", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					array[3] = 10;
					BitHelpers.SwitchBitInByte(array, 4, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 2, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[2], 0) && BitHelpers.GetBit_0_7(data[2], 1) && BitHelpers.GetBit_0_7(data[2], 2) && BitHelpers.GetBit_0_7(data[2], 2) && BitHelpers.GetBit_0_7(data[4], 1) && !BitHelpers.GetBit_0_7(data[6], 0) && BitHelpers.GetBit_0_7(data[6], 1) && !BitHelpers.GetBit_0_7(data[6], 2) && data[3] > 0)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D04", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					array2[1] = 10;
					BitHelpers.SwitchBitInByte(array2, 2, 1, true);
					BitHelpers.SwitchBitInByte(array2, 2, 2, false);
					BitHelpers.SwitchBitInByte(array2, 2, 3, true);
					BitHelpers.SwitchBitInByte(array2, 2, 4, false);
					BitHelpers.SwitchBitInByte(array2, 2, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
					array2[1] = 0;
					BitHelpers.SwitchBitInByte(array2, 4, 0, false);
				}
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[2], 0) && data[1] > 0)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			});
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.ExteriorLights, "Coming home activation (with light sensor)", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQB_LightFunction coming_home_func = MQB_LightFunction.FromValue("1E");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0556", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = coming_home_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionB == coming_home_func || config.FunctionC == coming_home_func);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0557", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = coming_home_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionB == coming_home_func || config.FunctionC == coming_home_func);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Coming home activation (trim with light sensors)", "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация функции Coming home (для комплектации с датчиком света)", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x003FE488 File Offset: 0x003FC688
		public static ICodingContainer ComingHomeWithoutLightSensor()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A57", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					array[3] = 10;
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 2, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[2], 0) && !BitHelpers.GetBit_0_7(data[2], 1) && BitHelpers.GetBit_0_7(data[2], 2) && !BitHelpers.GetBit_0_7(data[4], 1) && !BitHelpers.GetBit_0_7(data[6], 0) && BitHelpers.GetBit_0_7(data[6], 1) && !BitHelpers.GetBit_0_7(data[6], 2) && data[3] > 0)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D04", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, true);
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					array2[1] = 10;
					BitHelpers.SwitchBitInByte(array2, 2, 1, false);
					BitHelpers.SwitchBitInByte(array2, 2, 2, false);
					BitHelpers.SwitchBitInByte(array2, 2, 3, true);
					BitHelpers.SwitchBitInByte(array2, 2, 4, false);
					BitHelpers.SwitchBitInByte(array2, 2, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
					array2[3] = 0;
					BitHelpers.SwitchBitInByte(array2, 2, 0, false);
				}
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[0], 0) && !BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2) && !BitHelpers.GetBit_0_7(data[2], 1) && !BitHelpers.GetBit_0_7(data[2], 2) && BitHelpers.GetBit_0_7(data[2], 3) && !BitHelpers.GetBit_0_7(data[2], 4) && data[1] > 0)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			});
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.ExteriorLights, "Coming home activation (with light sensor)", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQB_LightFunction coming_home_func = MQB_LightFunction.FromValue("1E");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0556", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = coming_home_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionB == coming_home_func || config.FunctionC == coming_home_func);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0557", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = coming_home_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, (MQB_LightConfiguration config) => config.FunctionB == coming_home_func || config.FunctionC == coming_home_func);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Coming home activation (trim without light sensors)", "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Активация функции Coming home (для комплектации без датчика света)", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x003FE630 File Offset: 0x003FC830
		public static ICodingContainer PQ26_09_LightSensorSensivity()
		{
			MQBAdaptationOption opt_sensitive = new MQBAdaptationOption("Sensitive", "00", new TranslationItem[]
			{
				new TranslationItem("ru", "Высокая чувствительность", "", "")
			});
			MQBAdaptationOption opt_normal = new MQBAdaptationOption("Normal", "01", new TranslationItem[]
			{
				new TranslationItem("ru", "Нормально", "", "")
			});
			MQBAdaptationOption opt_non_sensitive = new MQBAdaptationOption("Non sensitive", "10", new TranslationItem[]
			{
				new TranslationItem("ru", "Низкая чувствительность", "", "")
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D0C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == opt_sensitive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				else if (value == opt_normal.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				else if (value == opt_non_sensitive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
				}
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[2], 0) && !BitHelpers.GetBit_0_7(data[2], 1))
				{
					return opt_sensitive.Title;
				}
				if (BitHelpers.GetBit_0_7(data[2], 0) && !BitHelpers.GetBit_0_7(data[2], 1))
				{
					return opt_normal.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[2], 0) && BitHelpers.GetBit_0_7(data[2], 1))
				{
					return opt_non_sensitive.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(true, "0D0C", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == opt_sensitive.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 0, false);
					BitHelpers.SwitchBitInByte(array2, 3, 1, false);
				}
				else if (value == opt_normal.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 0, true);
					BitHelpers.SwitchBitInByte(array2, 3, 1, false);
				}
				else if (value == opt_non_sensitive.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 3, 0, false);
					BitHelpers.SwitchBitInByte(array2, 3, 1, true);
				}
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[3], 0) && !BitHelpers.GetBit_0_7(data[3], 1))
				{
					return opt_sensitive.Title;
				}
				if (BitHelpers.GetBit_0_7(data[3], 0) && !BitHelpers.GetBit_0_7(data[3], 1))
				{
					return opt_normal.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[3], 0) && BitHelpers.GetBit_0_7(data[3], 1))
				{
					return opt_non_sensitive.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			});
			return new MQBAlternativeCoding(CodingGroup.Washer, "Light sensor sensivity customization", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Изменение чувствительности датчика света", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x003FE789 File Offset: 0x003FC989
		public static ICodingContainer TurnLightsPoliteBlinks()
		{
			return ExteriorLights.TurnLightsPoliteBlinks();
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x003FE790 File Offset: 0x003FC990
		public static ICodingContainer PQ26_ComingHomeLeavingHomeReverseLight()
		{
			MQB_LightFunction cominghomeFunc = MQB_LightFunction.FromValue("1E");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("056C", delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = cominghomeFunc;
				config.DimmwertCD = 100;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionC = off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Turn on stop reverse light with Coming home or Leaving home functions", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding })
			{
				Translations = 
				{
					new TranslationItem("ru", "Включение лампы заднего хода при работе функций Coming Home/Leaving Home", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x003FE828 File Offset: 0x003FCA28
		public static ICodingContainer ComingHomeLamps()
		{
			MQBAdaptationOption lowbeam = new MQBAdaptationOption("Low beam", MQBAdaptationTemplate.DisableOption.Value, new TranslationItem[]
			{
				new TranslationItem("ru", "Ближний свет", "", "")
			});
			MQBAdaptationOption foglight = new MQBAdaptationOption("Fog lights", MQBAdaptationTemplate.EnableOption.Value, new TranslationItem[]
			{
				new TranslationItem("ru", "Противотуманные фары", "", "")
			});
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A57", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
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
			return new MQBAlternativeCoding(CodingGroup.ExteriorLights, "Coming home/Leaving home lights", "", "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Coming home/Leaving home: выбор фар (ближний свет/ПТФ)", "", "")
				},
				RequiresPro = false
			};
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x003FE97C File Offset: 0x003FCB7C
		public static ICodingContainer EmergencyBrakingLightsWithTurnSignals()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0A59", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D0C", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("09", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit3 = new MQBAlternativeContainerForUnit09(true, "0A59", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 0, 2, false);
				}
				return array3;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit4 = new MQBAlternativeContainerForUnit09(false, "0D21", "31347", delegate(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, array4.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 1, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 1, 2, false);
				}
				return array4;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("09", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit3, mqbalternativeContainerForUnit4 });
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Emergency braking lights", "", false, new ICodingContainer[] { mqbalternativeCoding, mqbalternativeCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Включение аварийной сигнализации при экстренном торможении", "", "")
				}
			};
		}

		// Token: 0x02000A4D RID: 2637
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005368 RID: 21352 RVA: 0x003FEAC6 File Offset: 0x003FCCC6
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005369 RID: 21353 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600536A RID: 21354 RVA: 0x003FEAD4 File Offset: 0x003FCCD4
			internal byte[] <PQ26_DisableLicensePlateLightWhenTailgateIsOpened>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
					mqb_LightConfiguration.LoadFromData(array);
					mqb_LightConfiguration.LightControlAB = MQB_LightConfiguration.LightControl.IfBootIsClosed;
					array = mqb_LightConfiguration.ApplyToData();
				}
				else
				{
					MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration();
					mqb_LightConfiguration2.LoadFromData(array);
					mqb_LightConfiguration2.LightControlAB = MQB_LightConfiguration.LightControl.Always;
					array = mqb_LightConfiguration2.ApplyToData();
				}
				return array;
			}

			// Token: 0x0600536B RID: 21355 RVA: 0x003FEB40 File Offset: 0x003FCD40
			internal byte[] <PQ26_BlinkTurnLightsWithLEDDRL>b__1_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
					mqb_LightConfiguration.LoadFromData(array);
					mqb_LightConfiguration.FunctionG = MQB_LightFunction.FromValue(2);
					mqb_LightConfiguration.DimmwertGH = 0;
					mqb_LightConfiguration.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
					array = mqb_LightConfiguration.ApplyToData();
				}
				else
				{
					MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration();
					mqb_LightConfiguration2.LoadFromData(array);
					mqb_LightConfiguration2.FunctionG = MQB_LightFunction.FromValue(0);
					mqb_LightConfiguration2.DimmwertGH = 0;
					mqb_LightConfiguration2.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
					array = mqb_LightConfiguration2.ApplyToData();
				}
				return array;
			}

			// Token: 0x0600536C RID: 21356 RVA: 0x003FA957 File Offset: 0x003F8B57
			internal void <PQ26_BlinkTurnLightsWithLEDDRL>b__1_1(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue(4);
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x0600536D RID: 21357 RVA: 0x003FA973 File Offset: 0x003F8B73
			internal void <PQ26_BlinkTurnLightsWithLEDDRL>b__1_2(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue(0);
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x0600536E RID: 21358 RVA: 0x003FA98F File Offset: 0x003F8B8F
			internal bool <PQ26_BlinkTurnLightsWithLEDDRL>b__1_3(MQB_LightConfiguration config)
			{
				return config.FunctionG.Value == "04" && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x0600536F RID: 21359 RVA: 0x003FEBCF File Offset: 0x003FCDCF
			internal void <PQ26_RearSideLightsAsDRL>b__4_1(MQB_LightConfiguration config)
			{
				config.FunctionD = MQB_LightFunction.FromValue("00");
				config.DimmwertCD = 75;
			}

			// Token: 0x06005370 RID: 21360 RVA: 0x003FEBCF File Offset: 0x003FCDCF
			internal void <PQ26_RearSideLightsAsDRL>b__4_4(MQB_LightConfiguration config)
			{
				config.FunctionD = MQB_LightFunction.FromValue("00");
				config.DimmwertCD = 75;
			}

			// Token: 0x06005371 RID: 21361 RVA: 0x003FEBE9 File Offset: 0x003FCDE9
			internal void <PQ26_StroboscopeEffectFogLightHighBeam>b__5_1(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x06005372 RID: 21362 RVA: 0x003FEBE9 File Offset: 0x003FCDE9
			internal void <PQ26_StroboscopeEffectFogLightHighBeam>b__5_4(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x06005373 RID: 21363 RVA: 0x003FEC02 File Offset: 0x003FCE02
			internal void <PQ26_StroboscopeEffectLowBeamHighBeam>b__7_1(MQB_LightConfiguration config)
			{
				config.FunctionC = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x06005374 RID: 21364 RVA: 0x003FEC02 File Offset: 0x003FCE02
			internal void <PQ26_StroboscopeEffectLowBeamHighBeam>b__7_4(MQB_LightConfiguration config)
			{
				config.FunctionC = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x06005375 RID: 21365 RVA: 0x003FEBE9 File Offset: 0x003FCDE9
			internal void <PQ26_StroboscopeEffectLEDDRLHighBeam>b__8_1(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x06005376 RID: 21366 RVA: 0x003FEBE9 File Offset: 0x003FCDE9
			internal void <PQ26_StroboscopeEffectLEDDRLHighBeam>b__8_4(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x06005377 RID: 21367 RVA: 0x003FEC1C File Offset: 0x003FCE1C
			internal void <PQ26_USStandlichtsOnlyWhenLightInSideLightsPosition>b__15_1(MQB_LightConfiguration config)
			{
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(BitHelpers.ConvertHexToBytesX("0C340014020064000000000000000000"));
				config.FunctionA = mqb_LightConfiguration.FunctionA;
				config.DimmwertAB = mqb_LightConfiguration.DimmwertAB;
				config.FunctionC = mqb_LightConfiguration.FunctionC;
				config.FunctionD = mqb_LightConfiguration.FunctionD;
				config.DimmwertCD = mqb_LightConfiguration.DimmwertCD;
				config.DimmingDirectionCD = mqb_LightConfiguration.DimmingDirectionCD;
				config.FunctionE = mqb_LightConfiguration.FunctionE;
				config.DimmwertEF = mqb_LightConfiguration.DimmwertEF;
				config.DimmingDirectionEF = mqb_LightConfiguration.DimmingDirectionEF;
			}

			// Token: 0x06005378 RID: 21368 RVA: 0x003FECA8 File Offset: 0x003FCEA8
			internal void <PQ26_USStandlichtsOnlyWhenLightInSideLightsPosition>b__15_4(MQB_LightConfiguration config)
			{
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(BitHelpers.ConvertHexToBytesX("0C3E0015040064000000000000000000"));
				config.FunctionA = mqb_LightConfiguration.FunctionA;
				config.DimmwertAB = mqb_LightConfiguration.DimmwertAB;
				config.FunctionC = mqb_LightConfiguration.FunctionC;
				config.FunctionD = mqb_LightConfiguration.FunctionD;
				config.DimmwertCD = mqb_LightConfiguration.DimmwertCD;
				config.DimmingDirectionCD = mqb_LightConfiguration.DimmingDirectionCD;
				config.FunctionE = mqb_LightConfiguration.FunctionE;
				config.DimmwertEF = mqb_LightConfiguration.DimmwertEF;
				config.DimmingDirectionEF = mqb_LightConfiguration.DimmingDirectionEF;
			}

			// Token: 0x06005379 RID: 21369 RVA: 0x000027D4 File Offset: 0x000009D4
			internal void <PQ26_BlinkFrontSideLightsWithTurnLights>b__16_1(MQB_LightConfiguration config)
			{
			}

			// Token: 0x0600537A RID: 21370 RVA: 0x000027D4 File Offset: 0x000009D4
			internal void <PQ26_BlinkFrontSideLightsWithTurnLights>b__16_4(MQB_LightConfiguration config)
			{
			}

			// Token: 0x0600537B RID: 21371 RVA: 0x003FED34 File Offset: 0x003FCF34
			internal byte[] <LimitMaxFrontLights>b__22_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x0600537C RID: 21372 RVA: 0x003FED7D File Offset: 0x003FCF7D
			internal string <LimitMaxFrontLights>b__22_1(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[0], 2))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600537D RID: 21373 RVA: 0x003FEDA0 File Offset: 0x003FCFA0
			internal byte[] <LimitMaxFrontLights>b__22_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
				}
				return array;
			}

			// Token: 0x0600537E RID: 21374 RVA: 0x003FEDE9 File Offset: 0x003FCFE9
			internal string <LimitMaxFrontLights>b__22_3(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[1], 0))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x0600537F RID: 21375 RVA: 0x003FEE0C File Offset: 0x003FD00C
			internal byte[] <ComingHomeWithLightSensor>b__23_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					array[3] = 10;
					BitHelpers.SwitchBitInByte(array, 4, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 2, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				return array;
			}

			// Token: 0x06005380 RID: 21376 RVA: 0x003FEEB8 File Offset: 0x003FD0B8
			internal string <ComingHomeWithLightSensor>b__23_1(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[2], 0) && BitHelpers.GetBit_0_7(data[2], 1) && BitHelpers.GetBit_0_7(data[2], 2) && BitHelpers.GetBit_0_7(data[2], 2) && BitHelpers.GetBit_0_7(data[4], 1) && !BitHelpers.GetBit_0_7(data[6], 0) && BitHelpers.GetBit_0_7(data[6], 1) && !BitHelpers.GetBit_0_7(data[6], 2) && data[3] > 0)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005381 RID: 21377 RVA: 0x003FEF38 File Offset: 0x003FD138
			internal byte[] <ComingHomeWithLightSensor>b__23_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					array[1] = 10;
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					array[1] = 0;
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				return array;
			}

			// Token: 0x06005382 RID: 21378 RVA: 0x003FEFE4 File Offset: 0x003FD1E4
			internal string <ComingHomeWithLightSensor>b__23_3(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[0], 0) && BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2) && BitHelpers.GetBit_0_7(data[2], 0) && data[1] > 0)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005383 RID: 21379 RVA: 0x003FF038 File Offset: 0x003FD238
			internal byte[] <ComingHomeWithoutLightSensor>b__24_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, true);
					array[3] = 10;
					BitHelpers.SwitchBitInByte(array, 4, 1, false);
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 2, false);
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				return array;
			}

			// Token: 0x06005384 RID: 21380 RVA: 0x003FF0E4 File Offset: 0x003FD2E4
			internal string <ComingHomeWithoutLightSensor>b__24_1(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[2], 0) && !BitHelpers.GetBit_0_7(data[2], 1) && BitHelpers.GetBit_0_7(data[2], 2) && !BitHelpers.GetBit_0_7(data[4], 1) && !BitHelpers.GetBit_0_7(data[6], 0) && BitHelpers.GetBit_0_7(data[6], 1) && !BitHelpers.GetBit_0_7(data[6], 2) && data[3] > 0)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005385 RID: 21381 RVA: 0x003FF15C File Offset: 0x003FD35C
			internal byte[] <ComingHomeWithoutLightSensor>b__24_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					array[1] = 10;
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
					BitHelpers.SwitchBitInByte(array, 2, 2, false);
					BitHelpers.SwitchBitInByte(array, 2, 3, true);
					BitHelpers.SwitchBitInByte(array, 2, 4, false);
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					array[3] = 0;
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
				}
				return array;
			}

			// Token: 0x06005386 RID: 21382 RVA: 0x003FF208 File Offset: 0x003FD408
			internal string <ComingHomeWithoutLightSensor>b__24_3(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[0], 0) && !BitHelpers.GetBit_0_7(data[0], 1) && BitHelpers.GetBit_0_7(data[0], 2) && !BitHelpers.GetBit_0_7(data[2], 1) && !BitHelpers.GetBit_0_7(data[2], 2) && BitHelpers.GetBit_0_7(data[2], 3) && !BitHelpers.GetBit_0_7(data[2], 4) && data[1] > 0)
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x06005387 RID: 21383 RVA: 0x003FF280 File Offset: 0x003FD480
			internal byte[] <ComingHomeLamps>b__28_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 0, false);
				}
				return array;
			}

			// Token: 0x06005388 RID: 21384 RVA: 0x003FF2CC File Offset: 0x003FD4CC
			internal byte[] <ComingHomeLamps>b__28_2(byte[] data, string value, MQBAlternativeCoding codingItem)
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

			// Token: 0x06005389 RID: 21385 RVA: 0x003FF318 File Offset: 0x003FD518
			internal byte[] <EmergencyBrakingLightsWithTurnSignals>b__29_0(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x0600538A RID: 21386 RVA: 0x003FF364 File Offset: 0x003FD564
			internal byte[] <EmergencyBrakingLightsWithTurnSignals>b__29_1(byte[] data, string value, MQBAlternativeCoding coding2)
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

			// Token: 0x0600538B RID: 21387 RVA: 0x003FF3B0 File Offset: 0x003FD5B0
			internal byte[] <EmergencyBrakingLightsWithTurnSignals>b__29_2(byte[] data, string value, MQBAlternativeCoding coding2)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x0600538C RID: 21388 RVA: 0x003FF3FC File Offset: 0x003FD5FC
			internal byte[] <EmergencyBrakingLightsWithTurnSignals>b__29_3(byte[] data, string value, MQBAlternativeCoding coding2)
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

			// Token: 0x040032F8 RID: 13048
			public static readonly ExteriorLight.<>c <>9 = new ExteriorLight.<>c();

			// Token: 0x040032F9 RID: 13049
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x040032FA RID: 13050
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x040032FB RID: 13051
			public static Action<MQB_LightConfiguration> <>9__1_1;

			// Token: 0x040032FC RID: 13052
			public static Action<MQB_LightConfiguration> <>9__1_2;

			// Token: 0x040032FD RID: 13053
			public static Func<MQB_LightConfiguration, bool> <>9__1_3;

			// Token: 0x040032FE RID: 13054
			public static Action<MQB_LightConfiguration> <>9__4_1;

			// Token: 0x040032FF RID: 13055
			public static Action<MQB_LightConfiguration> <>9__4_4;

			// Token: 0x04003300 RID: 13056
			public static Action<MQB_LightConfiguration> <>9__5_1;

			// Token: 0x04003301 RID: 13057
			public static Action<MQB_LightConfiguration> <>9__5_4;

			// Token: 0x04003302 RID: 13058
			public static Action<MQB_LightConfiguration> <>9__7_1;

			// Token: 0x04003303 RID: 13059
			public static Action<MQB_LightConfiguration> <>9__7_4;

			// Token: 0x04003304 RID: 13060
			public static Action<MQB_LightConfiguration> <>9__8_1;

			// Token: 0x04003305 RID: 13061
			public static Action<MQB_LightConfiguration> <>9__8_4;

			// Token: 0x04003306 RID: 13062
			public static Action<MQB_LightConfiguration> <>9__15_1;

			// Token: 0x04003307 RID: 13063
			public static Action<MQB_LightConfiguration> <>9__15_4;

			// Token: 0x04003308 RID: 13064
			public static Action<MQB_LightConfiguration> <>9__16_1;

			// Token: 0x04003309 RID: 13065
			public static Action<MQB_LightConfiguration> <>9__16_4;

			// Token: 0x0400330A RID: 13066
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__22_0;

			// Token: 0x0400330B RID: 13067
			public static Func<byte[], MQBAlternativeCoding, string> <>9__22_1;

			// Token: 0x0400330C RID: 13068
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__22_2;

			// Token: 0x0400330D RID: 13069
			public static Func<byte[], MQBAlternativeCoding, string> <>9__22_3;

			// Token: 0x0400330E RID: 13070
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__23_0;

			// Token: 0x0400330F RID: 13071
			public static Func<byte[], MQBAlternativeCoding, string> <>9__23_1;

			// Token: 0x04003310 RID: 13072
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__23_2;

			// Token: 0x04003311 RID: 13073
			public static Func<byte[], MQBAlternativeCoding, string> <>9__23_3;

			// Token: 0x04003312 RID: 13074
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__24_0;

			// Token: 0x04003313 RID: 13075
			public static Func<byte[], MQBAlternativeCoding, string> <>9__24_1;

			// Token: 0x04003314 RID: 13076
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__24_2;

			// Token: 0x04003315 RID: 13077
			public static Func<byte[], MQBAlternativeCoding, string> <>9__24_3;

			// Token: 0x04003316 RID: 13078
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__28_0;

			// Token: 0x04003317 RID: 13079
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__28_2;

			// Token: 0x04003318 RID: 13080
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__29_0;

			// Token: 0x04003319 RID: 13081
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__29_1;

			// Token: 0x0400331A RID: 13082
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__29_2;

			// Token: 0x0400331B RID: 13083
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__29_3;
		}

		// Token: 0x02000A4E RID: 2638
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_0
		{
			// Token: 0x0600538D RID: 21389 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_0()
			{
			}

			// Token: 0x0600538E RID: 21390 RVA: 0x003FF445 File Offset: 0x003FD645
			internal void <PQ26_LEDDRL_WithLowBeam>b__0(MQB_LightConfiguration config)
			{
				config.FunctionC = this.left_func;
				config.DimmwertCD = 127;
			}

			// Token: 0x0600538F RID: 21391 RVA: 0x003FF45B File Offset: 0x003FD65B
			internal void <PQ26_LEDDRL_WithLowBeam>b__1(MQB_LightConfiguration config)
			{
				config.FunctionG = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x06005390 RID: 21392 RVA: 0x003FF470 File Offset: 0x003FD670
			internal bool <PQ26_LEDDRL_WithLowBeam>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.left_func && config.DimmwertCD == 127;
			}

			// Token: 0x06005391 RID: 21393 RVA: 0x003FF48D File Offset: 0x003FD68D
			internal void <PQ26_LEDDRL_WithLowBeam>b__3(MQB_LightConfiguration config)
			{
				config.FunctionC = this.right_func;
				config.DimmwertCD = 127;
			}

			// Token: 0x06005392 RID: 21394 RVA: 0x003FF45B File Offset: 0x003FD65B
			internal void <PQ26_LEDDRL_WithLowBeam>b__4(MQB_LightConfiguration config)
			{
				config.FunctionG = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x06005393 RID: 21395 RVA: 0x003FF4A3 File Offset: 0x003FD6A3
			internal bool <PQ26_LEDDRL_WithLowBeam>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.right_func && config.DimmwertCD == 127;
			}

			// Token: 0x0400331C RID: 13084
			public MQB_LightFunction left_func;

			// Token: 0x0400331D RID: 13085
			public MQB_LightFunction off_func;

			// Token: 0x0400331E RID: 13086
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A4F RID: 2639
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x06005394 RID: 21396 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x06005395 RID: 21397 RVA: 0x003FF4C0 File Offset: 0x003FD6C0
			internal void <PQ26_DimmingDRLWhenTurnLightsOn>b__0(MQB_LightConfiguration config)
			{
				config.FunctionG = this.left_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x06005396 RID: 21398 RVA: 0x003FF4DC File Offset: 0x003FD6DC
			internal void <PQ26_DimmingDRLWhenTurnLightsOn>b__1(MQB_LightConfiguration config)
			{
				config.FunctionG = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x06005397 RID: 21399 RVA: 0x003FF4F1 File Offset: 0x003FD6F1
			internal bool <PQ26_DimmingDRLWhenTurnLightsOn>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x06005398 RID: 21400 RVA: 0x003FF515 File Offset: 0x003FD715
			internal void <PQ26_DimmingDRLWhenTurnLightsOn>b__3(MQB_LightConfiguration config)
			{
				config.FunctionG = this.right_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x06005399 RID: 21401 RVA: 0x003FF4DC File Offset: 0x003FD6DC
			internal void <PQ26_DimmingDRLWhenTurnLightsOn>b__4(MQB_LightConfiguration config)
			{
				config.FunctionG = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x0600539A RID: 21402 RVA: 0x003FF4F1 File Offset: 0x003FD6F1
			internal bool <PQ26_DimmingDRLWhenTurnLightsOn>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x0400331F RID: 13087
			public MQB_LightFunction left_func;

			// Token: 0x04003320 RID: 13088
			public MQB_LightFunction off_func;

			// Token: 0x04003321 RID: 13089
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A50 RID: 2640
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x0600539B RID: 21403 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x0600539C RID: 21404 RVA: 0x003FF531 File Offset: 0x003FD731
			internal void <PQ26_BlinkDRLWithTurnLights>b__0(MQB_LightConfiguration config)
			{
				config.FunctionG = this.left_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x0600539D RID: 21405 RVA: 0x003FF54D File Offset: 0x003FD74D
			internal void <PQ26_BlinkDRLWithTurnLights>b__1(MQB_LightConfiguration config)
			{
				config.FunctionG = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x0600539E RID: 21406 RVA: 0x003FF562 File Offset: 0x003FD762
			internal bool <PQ26_BlinkDRLWithTurnLights>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x0600539F RID: 21407 RVA: 0x003FF586 File Offset: 0x003FD786
			internal void <PQ26_BlinkDRLWithTurnLights>b__3(MQB_LightConfiguration config)
			{
				config.FunctionG = this.right_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053A0 RID: 21408 RVA: 0x003FF54D File Offset: 0x003FD74D
			internal void <PQ26_BlinkDRLWithTurnLights>b__4(MQB_LightConfiguration config)
			{
				config.FunctionG = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x060053A1 RID: 21409 RVA: 0x003FF562 File Offset: 0x003FD762
			internal bool <PQ26_BlinkDRLWithTurnLights>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x04003322 RID: 13090
			public MQB_LightFunction left_func;

			// Token: 0x04003323 RID: 13091
			public MQB_LightFunction off_func;

			// Token: 0x04003324 RID: 13092
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A51 RID: 2641
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x060053A2 RID: 21410 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x060053A3 RID: 21411 RVA: 0x003FF5A2 File Offset: 0x003FD7A2
			internal void <PQ26_FrontSideLightDimmingWithTurnLights>b__0(MQB_LightConfiguration config)
			{
				config.FunctionE = this.left_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053A4 RID: 21412 RVA: 0x003FF5BE File Offset: 0x003FD7BE
			internal void <PQ26_FrontSideLightDimmingWithTurnLights>b__1(MQB_LightConfiguration config)
			{
				config.FunctionE = this.off_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053A5 RID: 21413 RVA: 0x003FF5DA File Offset: 0x003FD7DA
			internal bool <PQ26_FrontSideLightDimmingWithTurnLights>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionE == this.left_func && config.DimmwertEF == 0 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053A6 RID: 21414 RVA: 0x003FF5FE File Offset: 0x003FD7FE
			internal void <PQ26_FrontSideLightDimmingWithTurnLights>b__3(MQB_LightConfiguration config)
			{
				config.FunctionE = this.right_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053A7 RID: 21415 RVA: 0x003FF5BE File Offset: 0x003FD7BE
			internal void <PQ26_FrontSideLightDimmingWithTurnLights>b__4(MQB_LightConfiguration config)
			{
				config.FunctionE = this.off_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053A8 RID: 21416 RVA: 0x003FF61A File Offset: 0x003FD81A
			internal bool <PQ26_FrontSideLightDimmingWithTurnLights>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionE == this.right_func && config.DimmwertEF == 0 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x04003325 RID: 13093
			public MQB_LightFunction left_func;

			// Token: 0x04003326 RID: 13094
			public MQB_LightFunction off_func;

			// Token: 0x04003327 RID: 13095
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A52 RID: 2642
		[CompilerGenerated]
		private sealed class <>c__DisplayClass14_0
		{
			// Token: 0x060053A9 RID: 21417 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass14_0()
			{
			}

			// Token: 0x060053AA RID: 21418 RVA: 0x003FF63E File Offset: 0x003FD83E
			internal void <PQ26_USStandlichts>b__0(MQB_LightConfiguration config)
			{
				config.FunctionC = this.standlicht;
				config.DimmwertCD = 35;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.FunctionE = this.left_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053AB RID: 21419 RVA: 0x003FF675 File Offset: 0x003FD875
			internal void <PQ26_USStandlichts>b__1(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.FunctionE = this.off_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053AC RID: 21420 RVA: 0x003FF6AC File Offset: 0x003FD8AC
			internal bool <PQ26_USStandlichts>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionC == this.standlicht && config.DimmwertCD == 35 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Maximize && config.FunctionE == this.left_func && config.DimmwertEF == 0 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053AD RID: 21421 RVA: 0x003FF6FB File Offset: 0x003FD8FB
			internal void <PQ26_USStandlichts>b__3(MQB_LightConfiguration config)
			{
				config.FunctionC = this.standlicht;
				config.DimmwertCD = 35;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.FunctionE = this.right_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053AE RID: 21422 RVA: 0x003FF675 File Offset: 0x003FD875
			internal void <PQ26_USStandlichts>b__4(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.FunctionE = this.off_func;
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053AF RID: 21423 RVA: 0x003FF734 File Offset: 0x003FD934
			internal bool <PQ26_USStandlichts>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionC == this.standlicht && config.DimmwertCD == 35 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Maximize && config.FunctionE == this.right_func && config.DimmwertEF == 0 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x04003328 RID: 13096
			public MQB_LightFunction standlicht;

			// Token: 0x04003329 RID: 13097
			public MQB_LightFunction left_func;

			// Token: 0x0400332A RID: 13098
			public MQB_LightFunction off_func;

			// Token: 0x0400332B RID: 13099
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A53 RID: 2643
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			// Token: 0x060053B0 RID: 21424 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_0()
			{
			}

			// Token: 0x060053B1 RID: 21425 RVA: 0x003FF784 File Offset: 0x003FD984
			internal void <PQ26_USStandlichtsOnlyWhenLightInSideLightsPosition>b__0(MQB_LightConfiguration config)
			{
				config.FunctionA = this.standlicht_vorn;
				config.DimmwertAB = 35;
				config.FunctionC = this.left_Abblendlicht;
				config.FunctionD = this.left_blinken_dunkel;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
				config.FunctionE = this.left_blinken_hell;
				config.DimmwertEF = 100;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053B2 RID: 21426 RVA: 0x003FF7E8 File Offset: 0x003FD9E8
			internal bool <PQ26_USStandlichtsOnlyWhenLightInSideLightsPosition>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionA == this.standlicht_vorn && config.DimmwertAB == 35 && config.FunctionC == this.left_Abblendlicht && config.FunctionD == this.left_blinken_dunkel && config.DimmwertCD == 0 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Minimize && config.FunctionE == this.left_blinken_hell && config.DimmwertEF == 100 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053B3 RID: 21427 RVA: 0x003FF860 File Offset: 0x003FDA60
			internal void <PQ26_USStandlichtsOnlyWhenLightInSideLightsPosition>b__3(MQB_LightConfiguration config)
			{
				config.FunctionA = this.standlicht_vorn;
				config.DimmwertAB = 35;
				config.FunctionC = this.right_Abblendlicht;
				config.FunctionD = this.right_blinken_dunkel;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
				config.FunctionE = this.right_blinken_hell;
				config.DimmwertEF = 100;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053B4 RID: 21428 RVA: 0x003FF8C4 File Offset: 0x003FDAC4
			internal bool <PQ26_USStandlichtsOnlyWhenLightInSideLightsPosition>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionA == this.standlicht_vorn && config.DimmwertAB == 35 && config.FunctionC == this.right_Abblendlicht && config.FunctionD == this.right_blinken_dunkel && config.DimmwertCD == 0 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Minimize && config.FunctionE == this.right_blinken_hell && config.DimmwertEF == 100 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x0400332C RID: 13100
			public MQB_LightFunction standlicht_vorn;

			// Token: 0x0400332D RID: 13101
			public MQB_LightFunction left_Abblendlicht;

			// Token: 0x0400332E RID: 13102
			public MQB_LightFunction left_blinken_dunkel;

			// Token: 0x0400332F RID: 13103
			public MQB_LightFunction left_blinken_hell;

			// Token: 0x04003330 RID: 13104
			public MQB_LightFunction right_Abblendlicht;

			// Token: 0x04003331 RID: 13105
			public MQB_LightFunction right_blinken_dunkel;

			// Token: 0x04003332 RID: 13106
			public MQB_LightFunction right_blinken_hell;
		}

		// Token: 0x02000A54 RID: 2644
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x060053B5 RID: 21429 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x060053B6 RID: 21430 RVA: 0x003FF939 File Offset: 0x003FDB39
			internal void <PQ26_BlinkFrontSideLightsWithTurnLights>b__0(MQB_LightConfiguration config)
			{
				config.FunctionA = this.standlicht_vorn;
				config.FunctionE = this.left_blinken_dunkel;
				config.FunctionF = this.blinken_rechts_aktiv;
				config.DimmwertEF = 100;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053B7 RID: 21431 RVA: 0x003FF970 File Offset: 0x003FDB70
			internal bool <PQ26_BlinkFrontSideLightsWithTurnLights>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionA == this.standlicht_vorn && config.FunctionE == this.left_blinken_dunkel && config.FunctionF == this.blinken_rechts_aktiv && config.DimmwertEF == 100 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053B8 RID: 21432 RVA: 0x003FF9BC File Offset: 0x003FDBBC
			internal void <PQ26_BlinkFrontSideLightsWithTurnLights>b__3(MQB_LightConfiguration config)
			{
				config.FunctionA = this.standlicht_vorn;
				config.FunctionE = this.right_blinken_dunkel;
				config.FunctionF = this.blinken_links_aktiv;
				config.DimmwertEF = 100;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053B9 RID: 21433 RVA: 0x003FF9F4 File Offset: 0x003FDBF4
			internal bool <PQ26_BlinkFrontSideLightsWithTurnLights>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionA == this.standlicht_vorn && config.FunctionE == this.right_blinken_dunkel && config.FunctionF == this.blinken_links_aktiv && config.DimmwertEF == 100 && config.DimmingDirectionEF == MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x04003333 RID: 13107
			public MQB_LightFunction standlicht_vorn;

			// Token: 0x04003334 RID: 13108
			public MQB_LightFunction left_blinken_dunkel;

			// Token: 0x04003335 RID: 13109
			public MQB_LightFunction blinken_rechts_aktiv;

			// Token: 0x04003336 RID: 13110
			public MQB_LightFunction right_blinken_dunkel;

			// Token: 0x04003337 RID: 13111
			public MQB_LightFunction blinken_links_aktiv;
		}

		// Token: 0x02000A55 RID: 2645
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x060053BA RID: 21434 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x060053BB RID: 21435 RVA: 0x003FFA40 File Offset: 0x003FDC40
			internal void <PQ26_TurnOnStopSignalWhenDoorOpened>b__0(MQB_LightConfiguration config)
			{
				config.FunctionC = this.left_func;
				config.DimmwertCD = 100;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053BC RID: 21436 RVA: 0x003FFA5D File Offset: 0x003FDC5D
			internal void <PQ26_TurnOnStopSignalWhenDoorOpened>b__1(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053BD RID: 21437 RVA: 0x003FFA79 File Offset: 0x003FDC79
			internal bool <PQ26_TurnOnStopSignalWhenDoorOpened>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionC == this.left_func && config.DimmwertCD == 100 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053BE RID: 21438 RVA: 0x003FFA9E File Offset: 0x003FDC9E
			internal void <PQ26_TurnOnStopSignalWhenDoorOpened>b__3(MQB_LightConfiguration config)
			{
				config.FunctionC = this.right_func;
				config.DimmwertCD = 100;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053BF RID: 21439 RVA: 0x003FFA5D File Offset: 0x003FDC5D
			internal void <PQ26_TurnOnStopSignalWhenDoorOpened>b__4(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053C0 RID: 21440 RVA: 0x003FFABB File Offset: 0x003FDCBB
			internal bool <PQ26_TurnOnStopSignalWhenDoorOpened>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionC == this.right_func && config.DimmwertCD == 100 && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x04003338 RID: 13112
			public MQB_LightFunction left_func;

			// Token: 0x04003339 RID: 13113
			public MQB_LightFunction off_func;

			// Token: 0x0400333A RID: 13114
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A56 RID: 2646
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x060053C1 RID: 21441 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x060053C2 RID: 21442 RVA: 0x003FFAE0 File Offset: 0x003FDCE0
			internal void <PQ26_TurnOnSideTurnlightsWhenTailgateOpened>b__0(MQB_LightConfiguration config)
			{
				config.FunctionC = this.open_func;
				config.DimmwertCD = 50;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053C3 RID: 21443 RVA: 0x003FFAFD File Offset: 0x003FDCFD
			internal void <PQ26_TurnOnSideTurnlightsWhenTailgateOpened>b__1(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x060053C4 RID: 21444 RVA: 0x003FFB12 File Offset: 0x003FDD12
			internal bool <PQ26_TurnOnSideTurnlightsWhenTailgateOpened>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionC == this.open_func && config.DimmwertCD == 50;
			}

			// Token: 0x060053C5 RID: 21445 RVA: 0x003FFAE0 File Offset: 0x003FDCE0
			internal void <PQ26_TurnOnSideTurnlightsWhenTailgateOpened>b__3(MQB_LightConfiguration config)
			{
				config.FunctionC = this.open_func;
				config.DimmwertCD = 50;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053C6 RID: 21446 RVA: 0x003FFAFD File Offset: 0x003FDCFD
			internal void <PQ26_TurnOnSideTurnlightsWhenTailgateOpened>b__4(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x060053C7 RID: 21447 RVA: 0x003FFB12 File Offset: 0x003FDD12
			internal bool <PQ26_TurnOnSideTurnlightsWhenTailgateOpened>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionC == this.open_func && config.DimmwertCD == 50;
			}

			// Token: 0x0400333B RID: 13115
			public MQB_LightFunction open_func;

			// Token: 0x0400333C RID: 13116
			public MQB_LightFunction off_func;
		}

		// Token: 0x02000A57 RID: 2647
		[CompilerGenerated]
		private sealed class <>c__DisplayClass23_0
		{
			// Token: 0x060053C8 RID: 21448 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass23_0()
			{
			}

			// Token: 0x060053C9 RID: 21449 RVA: 0x003FFB2F File Offset: 0x003FDD2F
			internal void <ComingHomeWithLightSensor>b__4(MQB_LightConfiguration config)
			{
				config.FunctionC = this.coming_home_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053CA RID: 21450 RVA: 0x003FFB4C File Offset: 0x003FDD4C
			internal void <ComingHomeWithLightSensor>b__5(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053CB RID: 21451 RVA: 0x003FFB68 File Offset: 0x003FDD68
			internal bool <ComingHomeWithLightSensor>b__6(MQB_LightConfiguration config)
			{
				return config.FunctionB == this.coming_home_func || config.FunctionC == this.coming_home_func;
			}

			// Token: 0x060053CC RID: 21452 RVA: 0x003FFB2F File Offset: 0x003FDD2F
			internal void <ComingHomeWithLightSensor>b__7(MQB_LightConfiguration config)
			{
				config.FunctionC = this.coming_home_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053CD RID: 21453 RVA: 0x003FFB4C File Offset: 0x003FDD4C
			internal void <ComingHomeWithLightSensor>b__8(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053CE RID: 21454 RVA: 0x003FFB68 File Offset: 0x003FDD68
			internal bool <ComingHomeWithLightSensor>b__9(MQB_LightConfiguration config)
			{
				return config.FunctionB == this.coming_home_func || config.FunctionC == this.coming_home_func;
			}

			// Token: 0x0400333D RID: 13117
			public MQB_LightFunction coming_home_func;

			// Token: 0x0400333E RID: 13118
			public MQB_LightFunction off_func;
		}

		// Token: 0x02000A58 RID: 2648
		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_0
		{
			// Token: 0x060053CF RID: 21455 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass24_0()
			{
			}

			// Token: 0x060053D0 RID: 21456 RVA: 0x003FFB89 File Offset: 0x003FDD89
			internal void <ComingHomeWithoutLightSensor>b__4(MQB_LightConfiguration config)
			{
				config.FunctionC = this.coming_home_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053D1 RID: 21457 RVA: 0x003FFBA6 File Offset: 0x003FDDA6
			internal void <ComingHomeWithoutLightSensor>b__5(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053D2 RID: 21458 RVA: 0x003FFBC2 File Offset: 0x003FDDC2
			internal bool <ComingHomeWithoutLightSensor>b__6(MQB_LightConfiguration config)
			{
				return config.FunctionB == this.coming_home_func || config.FunctionC == this.coming_home_func;
			}

			// Token: 0x060053D3 RID: 21459 RVA: 0x003FFB89 File Offset: 0x003FDD89
			internal void <ComingHomeWithoutLightSensor>b__7(MQB_LightConfiguration config)
			{
				config.FunctionC = this.coming_home_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053D4 RID: 21460 RVA: 0x003FFBA6 File Offset: 0x003FDDA6
			internal void <ComingHomeWithoutLightSensor>b__8(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053D5 RID: 21461 RVA: 0x003FFBC2 File Offset: 0x003FDDC2
			internal bool <ComingHomeWithoutLightSensor>b__9(MQB_LightConfiguration config)
			{
				return config.FunctionB == this.coming_home_func || config.FunctionC == this.coming_home_func;
			}

			// Token: 0x0400333F RID: 13119
			public MQB_LightFunction coming_home_func;

			// Token: 0x04003340 RID: 13120
			public MQB_LightFunction off_func;
		}

		// Token: 0x02000A59 RID: 2649
		[CompilerGenerated]
		private sealed class <>c__DisplayClass25_0
		{
			// Token: 0x060053D6 RID: 21462 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass25_0()
			{
			}

			// Token: 0x060053D7 RID: 21463 RVA: 0x003FFBE4 File Offset: 0x003FDDE4
			internal byte[] <PQ26_09_LightSensorSensivity>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_sensitive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				else if (value == this.opt_normal.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, true);
					BitHelpers.SwitchBitInByte(array, 2, 1, false);
				}
				else if (value == this.opt_non_sensitive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 0, false);
					BitHelpers.SwitchBitInByte(array, 2, 1, true);
				}
				return array;
			}

			// Token: 0x060053D8 RID: 21464 RVA: 0x003FFC7C File Offset: 0x003FDE7C
			internal string <PQ26_09_LightSensorSensivity>b__1(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[2], 0) && !BitHelpers.GetBit_0_7(data[2], 1))
				{
					return this.opt_sensitive.Title;
				}
				if (BitHelpers.GetBit_0_7(data[2], 0) && !BitHelpers.GetBit_0_7(data[2], 1))
				{
					return this.opt_normal.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[2], 0) && BitHelpers.GetBit_0_7(data[2], 1))
				{
					return this.opt_non_sensitive.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x060053D9 RID: 21465 RVA: 0x003FFCF4 File Offset: 0x003FDEF4
			internal byte[] <PQ26_09_LightSensorSensivity>b__2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == this.opt_sensitive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				else if (value == this.opt_normal.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
				}
				else if (value == this.opt_non_sensitive.Value)
				{
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
				}
				return array;
			}

			// Token: 0x060053DA RID: 21466 RVA: 0x003FFD8C File Offset: 0x003FDF8C
			internal string <PQ26_09_LightSensorSensivity>b__3(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (!BitHelpers.GetBit_0_7(data[3], 0) && !BitHelpers.GetBit_0_7(data[3], 1))
				{
					return this.opt_sensitive.Title;
				}
				if (BitHelpers.GetBit_0_7(data[3], 0) && !BitHelpers.GetBit_0_7(data[3], 1))
				{
					return this.opt_normal.Title;
				}
				if (!BitHelpers.GetBit_0_7(data[3], 0) && BitHelpers.GetBit_0_7(data[3], 1))
				{
					return this.opt_non_sensitive.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x04003341 RID: 13121
			public MQBAdaptationOption opt_sensitive;

			// Token: 0x04003342 RID: 13122
			public MQBAdaptationOption opt_normal;

			// Token: 0x04003343 RID: 13123
			public MQBAdaptationOption opt_non_sensitive;
		}

		// Token: 0x02000A5A RID: 2650
		[CompilerGenerated]
		private sealed class <>c__DisplayClass27_0
		{
			// Token: 0x060053DB RID: 21467 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass27_0()
			{
			}

			// Token: 0x060053DC RID: 21468 RVA: 0x003FFE04 File Offset: 0x003FE004
			internal void <PQ26_ComingHomeLeavingHomeReverseLight>b__0(MQB_LightConfiguration config)
			{
				config.FunctionC = this.cominghomeFunc;
				config.DimmwertCD = 100;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053DD RID: 21469 RVA: 0x003FFE21 File Offset: 0x003FE021
			internal void <PQ26_ComingHomeLeavingHomeReverseLight>b__1(MQB_LightConfiguration config)
			{
				config.FunctionC = this.off_func;
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x04003344 RID: 13124
			public MQB_LightFunction cominghomeFunc;

			// Token: 0x04003345 RID: 13125
			public MQB_LightFunction off_func;
		}

		// Token: 0x02000A5B RID: 2651
		[CompilerGenerated]
		private sealed class <>c__DisplayClass28_0
		{
			// Token: 0x060053DE RID: 21470 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass28_0()
			{
			}

			// Token: 0x060053DF RID: 21471 RVA: 0x003FFE3D File Offset: 0x003FE03D
			internal string <ComingHomeLamps>b__1(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[4], 7))
				{
					return this.foglight.Title;
				}
				return this.lowbeam.Title;
			}

			// Token: 0x060053E0 RID: 21472 RVA: 0x003FFE61 File Offset: 0x003FE061
			internal string <ComingHomeLamps>b__3(byte[] data, MQBAlternativeCoding codingItem)
			{
				if (BitHelpers.GetBit_0_7(data[2], 0))
				{
					return this.foglight.Title;
				}
				return this.lowbeam.Title;
			}

			// Token: 0x04003346 RID: 13126
			public MQBAdaptationOption foglight;

			// Token: 0x04003347 RID: 13127
			public MQBAdaptationOption lowbeam;
		}

		// Token: 0x02000A5C RID: 2652
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060053E1 RID: 21473 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060053E2 RID: 21474 RVA: 0x003FFE88 File Offset: 0x003FE088
			internal void <PQ26_Corner>b__0(MQB_LightConfiguration config)
			{
				if (config.FunctionB == this.disabled_func || config.FunctionB == this.left_func)
				{
					config.FunctionB = this.left_func;
					if (config.DimmwertAB < 100)
					{
						config.DimmwertAB = 100;
						return;
					}
				}
				else
				{
					if (config.FunctionC == this.disabled_func || config.FunctionC == this.left_func)
					{
						config.FunctionD = this.left_func;
						if (config.DimmwertCD < 100)
						{
							config.DimmwertCD = 100;
						}
						config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
						return;
					}
					if (config.FunctionD == this.disabled_func || config.FunctionD == this.left_func)
					{
						config.FunctionD = this.left_func;
						if (config.DimmwertCD < 100)
						{
							config.DimmwertCD = 100;
						}
						config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
					}
				}
			}

			// Token: 0x060053E3 RID: 21475 RVA: 0x003FFF58 File Offset: 0x003FE158
			internal void <PQ26_Corner>b__1(MQB_LightConfiguration config)
			{
				if (config.FunctionB == this.left_func)
				{
					config.FunctionB = this.disabled_func;
				}
				if (config.FunctionC == this.left_func)
				{
					config.FunctionC = this.disabled_func;
					if (config.FunctionD == this.disabled_func)
					{
						config.DimmwertCD = 0;
					}
				}
				if (config.FunctionD == this.left_func)
				{
					config.FunctionD = this.disabled_func;
				}
				config.FunctionD = MQB_LightFunction.FromValue(0);
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060053E4 RID: 21476 RVA: 0x003FFFE4 File Offset: 0x003FE1E4
			internal void <PQ26_Corner>b__2(MQB_LightConfiguration config)
			{
				if (config.FunctionB == this.disabled_func || config.FunctionB == this.right_func)
				{
					config.FunctionB = this.right_func;
					if (config.DimmwertAB < 100)
					{
						config.DimmwertAB = 100;
						return;
					}
				}
				else
				{
					if (config.FunctionC == this.disabled_func || config.FunctionC == this.right_func)
					{
						config.FunctionD = this.right_func;
						if (config.DimmwertCD < 100)
						{
							config.DimmwertCD = 100;
						}
						config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
						return;
					}
					if (config.FunctionD == this.disabled_func || config.FunctionD == this.right_func)
					{
						config.FunctionD = this.right_func;
						if (config.DimmwertCD < 100)
						{
							config.DimmwertCD = 100;
						}
						config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
					}
				}
			}

			// Token: 0x060053E5 RID: 21477 RVA: 0x004000B4 File Offset: 0x003FE2B4
			internal void <PQ26_Corner>b__3(MQB_LightConfiguration config)
			{
				if (config.FunctionB == this.right_func)
				{
					config.FunctionB = this.disabled_func;
				}
				if (config.FunctionC == this.right_func)
				{
					config.FunctionC = this.disabled_func;
					if (config.FunctionD == this.disabled_func)
					{
						config.DimmwertCD = 0;
					}
				}
				if (config.FunctionD == this.right_func)
				{
					config.FunctionD = this.disabled_func;
				}
				config.FunctionD = MQB_LightFunction.FromValue(0);
				config.DimmwertCD = 0;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x04003348 RID: 13128
			public MQB_LightFunction disabled_func;

			// Token: 0x04003349 RID: 13129
			public MQB_LightFunction left_func;

			// Token: 0x0400334A RID: 13130
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A5D RID: 2653
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x060053E6 RID: 21478 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x060053E7 RID: 21479 RVA: 0x0040013E File Offset: 0x003FE33E
			internal void <PQ26_RearSideLightsAsDRL>b__0(MQB_LightConfiguration config)
			{
				config.FunctionD = this.drl_func;
				config.DimmwertCD = 75;
			}

			// Token: 0x060053E8 RID: 21480 RVA: 0x00400154 File Offset: 0x003FE354
			internal bool <PQ26_RearSideLightsAsDRL>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionD == this.drl_func && config.DimmwertCD == 75;
			}

			// Token: 0x060053E9 RID: 21481 RVA: 0x0040013E File Offset: 0x003FE33E
			internal void <PQ26_RearSideLightsAsDRL>b__3(MQB_LightConfiguration config)
			{
				config.FunctionD = this.drl_func;
				config.DimmwertCD = 75;
			}

			// Token: 0x060053EA RID: 21482 RVA: 0x00400154 File Offset: 0x003FE354
			internal bool <PQ26_RearSideLightsAsDRL>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionD == this.drl_func && config.DimmwertCD == 75;
			}

			// Token: 0x0400334B RID: 13131
			public MQB_LightFunction drl_func;
		}

		// Token: 0x02000A5E RID: 2654
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x060053EB RID: 21483 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x060053EC RID: 21484 RVA: 0x00400171 File Offset: 0x003FE371
			internal void <PQ26_StroboscopeEffectFogLightHighBeam>b__0(MQB_LightConfiguration config)
			{
				config.FunctionG = this.headlight_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053ED RID: 21485 RVA: 0x00400186 File Offset: 0x003FE386
			internal bool <PQ26_StroboscopeEffectFogLightHighBeam>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.headlight_func && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053EE RID: 21486 RVA: 0x00400171 File Offset: 0x003FE371
			internal void <PQ26_StroboscopeEffectFogLightHighBeam>b__3(MQB_LightConfiguration config)
			{
				config.FunctionG = this.headlight_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053EF RID: 21487 RVA: 0x00400186 File Offset: 0x003FE386
			internal bool <PQ26_StroboscopeEffectFogLightHighBeam>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.headlight_func && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x0400334C RID: 13132
			public MQB_LightFunction headlight_func;
		}

		// Token: 0x02000A5F RID: 2655
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x060053F0 RID: 21488 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x060053F1 RID: 21489 RVA: 0x004001A2 File Offset: 0x003FE3A2
			internal void <PQ26_FogLightsWithHighBeam>b__0(MQB_LightConfiguration config)
			{
				config.FunctionH = this.left_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				config.DimmwertGH = 100;
			}

			// Token: 0x060053F2 RID: 21490 RVA: 0x004001BF File Offset: 0x003FE3BF
			internal void <PQ26_FogLightsWithHighBeam>b__1(MQB_LightConfiguration config)
			{
				config.FunctionH = this.off_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.DimmwertGH = 0;
			}

			// Token: 0x060053F3 RID: 21491 RVA: 0x004001DB File Offset: 0x003FE3DB
			internal void <PQ26_FogLightsWithHighBeam>b__2(MQB_LightConfiguration config)
			{
				config.FunctionH = this.right_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
				config.DimmwertGH = 100;
			}

			// Token: 0x060053F4 RID: 21492 RVA: 0x004001BF File Offset: 0x003FE3BF
			internal void <PQ26_FogLightsWithHighBeam>b__3(MQB_LightConfiguration config)
			{
				config.FunctionH = this.off_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
				config.DimmwertGH = 0;
			}

			// Token: 0x0400334D RID: 13133
			public MQB_LightFunction left_func;

			// Token: 0x0400334E RID: 13134
			public MQB_LightFunction off_func;

			// Token: 0x0400334F RID: 13135
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A60 RID: 2656
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x060053F5 RID: 21493 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x060053F6 RID: 21494 RVA: 0x004001F8 File Offset: 0x003FE3F8
			internal void <PQ26_StroboscopeEffectLowBeamHighBeam>b__0(MQB_LightConfiguration config)
			{
				config.FunctionC = this.headlight_func;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053F7 RID: 21495 RVA: 0x0040020D File Offset: 0x003FE40D
			internal bool <PQ26_StroboscopeEffectLowBeamHighBeam>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionC == this.headlight_func && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053F8 RID: 21496 RVA: 0x004001F8 File Offset: 0x003FE3F8
			internal void <PQ26_StroboscopeEffectLowBeamHighBeam>b__3(MQB_LightConfiguration config)
			{
				config.FunctionC = this.headlight_func;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053F9 RID: 21497 RVA: 0x0040020D File Offset: 0x003FE40D
			internal bool <PQ26_StroboscopeEffectLowBeamHighBeam>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionC == this.headlight_func && config.DimmingDirectionCD == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x04003350 RID: 13136
			public MQB_LightFunction headlight_func;
		}

		// Token: 0x02000A61 RID: 2657
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x060053FA RID: 21498 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x060053FB RID: 21499 RVA: 0x00400229 File Offset: 0x003FE429
			internal void <PQ26_StroboscopeEffectLEDDRLHighBeam>b__0(MQB_LightConfiguration config)
			{
				config.FunctionG = this.headlight_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053FC RID: 21500 RVA: 0x0040023E File Offset: 0x003FE43E
			internal bool <PQ26_StroboscopeEffectLEDDRLHighBeam>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.headlight_func && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053FD RID: 21501 RVA: 0x00400229 File Offset: 0x003FE429
			internal void <PQ26_StroboscopeEffectLEDDRLHighBeam>b__3(MQB_LightConfiguration config)
			{
				config.FunctionG = this.headlight_func;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060053FE RID: 21502 RVA: 0x0040023E File Offset: 0x003FE43E
			internal bool <PQ26_StroboscopeEffectLEDDRLHighBeam>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.headlight_func && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x04003351 RID: 13137
			public MQB_LightFunction headlight_func;
		}
	}
}
