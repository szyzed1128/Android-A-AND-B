using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020
{
	// Token: 0x02000A29 RID: 2601
	internal static class ExteriorLights
	{
		// Token: 0x060052A6 RID: 21158 RVA: 0x003FA040 File Offset: 0x003F8240
		public static ICodingContainer PQ26_RearSideLightsAsDRL_Rapid2020()
		{
			MQB_LightFunction drl_func = MQB_LightFunction.FromValue("14");
			MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0567", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = drl_func;
				config.DimmwertEF = 40;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0568", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = drl_func;
				config.DimmwertEF = 40;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding3 = new MQBEasyLightCoding("0564", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = drl_func;
				config.DimmwertEF = 6;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding4 = new MQBEasyLightCoding("0565", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = drl_func;
				config.DimmwertEF = 6;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Rear side lights active with DRL (Skoda Rapid 2020-)", "PQ26: Skoda Rapid 2020-", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2, mqbeasyLightCoding3, mqbeasyLightCoding4 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Включение задних габаритов в режиме ДХО (без подсветки номера и приборки) [Skoda Rapid 2020+]", "PQ26: Skoda Rapid 2020", "")
				}
			};
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x003FA198 File Offset: 0x003F8398
		public static ICodingContainer PQ26_RearSideLightsAsDRL_VWPolo2020()
		{
			MQB_LightFunction drl_func = MQB_LightFunction.FromValue("14");
			MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0567", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = drl_func;
				config.DimmwertEF = 75;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0568", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = drl_func;
				config.DimmwertEF = 75;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding3 = new MQBEasyLightCoding("055C", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = drl_func;
				config.DimmwertEF = 75;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding4 = new MQBEasyLightCoding("055D", delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = drl_func;
				config.DimmwertEF = 75;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Rear side lights active with DRL (Volkswagen Polo Liftback 2020+)", "PQ26: VW Polo 2020-", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2, mqbeasyLightCoding3, mqbeasyLightCoding4 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Включение задних габаритов в режиме ДХО (без подсветки номера и приборки) [Volkswagen Polo 2020+]", "PQ26: VW Polo 2020 (лифтбэк)", "")
				}
			};
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x003FA2F0 File Offset: 0x003F84F0
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

		// Token: 0x060052A9 RID: 21161 RVA: 0x003FA408 File Offset: 0x003F8608
		public static ICodingContainer PQ26_BlinkRearSideLightsWithTurnLight()
		{
			int target_dimming = 0;
			MQB_LightFunction.FromValue("00");
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("02");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("04");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0567", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = left_func;
				config.DimmwertGH = target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0568", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = right_func;
				config.DimmwertGH = target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding3 = new MQBEasyLightCoding("0564", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = left_func;
				config.DimmwertGH = target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding4 = new MQBEasyLightCoding("0565", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = right_func;
				config.DimmwertGH = target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Blink rear side lights with turn lights", "PQ26: Skoda Rapid 2020+", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2, mqbeasyLightCoding3, mqbeasyLightCoding4 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Перемигивание задних габаритов с указателем поворота [Skoda Rapid 2020+]", "PQ26: Skoda Rapid 2020", "")
				}
			};
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x003FA578 File Offset: 0x003F8778
		public static ICodingContainer PQ26_DimmRearSideLightsWithTurnLight()
		{
			int target_dimming = 35;
			MQB_LightFunction.FromValue("00");
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("02");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("04");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0567", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = left_func;
				config.DimmwertGH = target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0568", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = right_func;
				config.DimmwertGH = target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding3 = new MQBEasyLightCoding("0564", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = left_func;
				config.DimmwertGH = target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding4 = new MQBEasyLightCoding("0565", delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = right_func;
				config.DimmwertGH = target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Dimm rear side lights with turn lights (Audi style)", "PQ26: Skoda Rapid 2020+", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2, mqbeasyLightCoding3, mqbeasyLightCoding4 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Притухание задних габаритов с указателем поворота (Audi стиль) [Skoda Rapid 2020+]", "PQ26: Skoda Rapid 2020", "")
				}
			};
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x003FA6E8 File Offset: 0x003F88E8
		public static ICodingContainer PQ26_TurnOnStopSignalWhenDoorOpened()
		{
			MQB_LightFunction left_func = MQB_LightFunction.FromValue("38");
			MQB_LightFunction right_func = MQB_LightFunction.FromValue("39");
			MQB_LightFunction off_func = MQB_LightFunction.FromValue("00");
			MQBEasyLightCoding mqbeasyLightCoding = new MQBEasyLightCoding("0564", delegate(MQB_LightConfiguration config)
			{
				config.FunctionD = left_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionD = off_func;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			MQBEasyLightCoding mqbeasyLightCoding2 = new MQBEasyLightCoding("0565", delegate(MQB_LightConfiguration config)
			{
				config.FunctionD = right_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, delegate(MQB_LightConfiguration config)
			{
				config.FunctionD = off_func;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}, null);
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "Turn on stop lights on the side of opened door", "PQ26: Skoda Rapid", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Включение стоп-сигнала со стороны открытой двери ", "PQ26: Skoda Rapid", "")
				}
			};
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x003FA7B8 File Offset: 0x003F89B8
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
			return new MQBMultipleCoding(CodingGroup.ExteriorLights, "LED DRL dimming when turn signal flashing", "PQ26: Skoda Rapid 2020 (NOT compatible with FULL LED headlights)", false, new ICodingContainer[] { mqbeasyLightCoding, mqbeasyLightCoding2 })
			{
				Translations = 
				{
					new TranslationItem("ru", "Притухание ДХО при включении указателя поворота (на соответствующей стороне)", "PQ26: Skoda Rapid (не подходит для FULL LED фар)", "")
				}
			};
		}

		// Token: 0x02000A2A RID: 2602
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060052AD RID: 21165 RVA: 0x003FA89C File Offset: 0x003F8A9C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060052AE RID: 21166 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060052AF RID: 21167 RVA: 0x003FA8A8 File Offset: 0x003F8AA8
			internal void <PQ26_RearSideLightsAsDRL_Rapid2020>b__0_1(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052B0 RID: 21168 RVA: 0x003FA8A8 File Offset: 0x003F8AA8
			internal void <PQ26_RearSideLightsAsDRL_Rapid2020>b__0_3(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052B1 RID: 21169 RVA: 0x003FA8A8 File Offset: 0x003F8AA8
			internal void <PQ26_RearSideLightsAsDRL_Rapid2020>b__0_5(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052B2 RID: 21170 RVA: 0x003FA8A8 File Offset: 0x003F8AA8
			internal void <PQ26_RearSideLightsAsDRL_Rapid2020>b__0_7(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052B3 RID: 21171 RVA: 0x003FA8A8 File Offset: 0x003F8AA8
			internal void <PQ26_RearSideLightsAsDRL_VWPolo2020>b__1_1(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052B4 RID: 21172 RVA: 0x003FA8A8 File Offset: 0x003F8AA8
			internal void <PQ26_RearSideLightsAsDRL_VWPolo2020>b__1_3(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052B5 RID: 21173 RVA: 0x003FA8A8 File Offset: 0x003F8AA8
			internal void <PQ26_RearSideLightsAsDRL_VWPolo2020>b__1_5(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052B6 RID: 21174 RVA: 0x003FA8A8 File Offset: 0x003F8AA8
			internal void <PQ26_RearSideLightsAsDRL_VWPolo2020>b__1_7(MQB_LightConfiguration config)
			{
				config.FunctionE = MQB_LightFunction.FromValue("00");
				config.DimmwertEF = 0;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052B7 RID: 21175 RVA: 0x003FA8C8 File Offset: 0x003F8AC8
			internal byte[] <PQ26_BlinkTurnLightsWithLEDDRL>b__2_0(byte[] data, string value, MQBEasyCodingItem codingItem)
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

			// Token: 0x060052B8 RID: 21176 RVA: 0x003FA957 File Offset: 0x003F8B57
			internal void <PQ26_BlinkTurnLightsWithLEDDRL>b__2_1(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue(4);
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052B9 RID: 21177 RVA: 0x003FA973 File Offset: 0x003F8B73
			internal void <PQ26_BlinkTurnLightsWithLEDDRL>b__2_2(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue(0);
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052BA RID: 21178 RVA: 0x003FA98F File Offset: 0x003F8B8F
			internal bool <PQ26_BlinkTurnLightsWithLEDDRL>b__2_3(MQB_LightConfiguration config)
			{
				return config.FunctionG.Value == "04" && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052BB RID: 21179 RVA: 0x003FA9BC File Offset: 0x003F8BBC
			internal void <PQ26_BlinkRearSideLightsWithTurnLight>b__3_1(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052BC RID: 21180 RVA: 0x003FA9BC File Offset: 0x003F8BBC
			internal void <PQ26_BlinkRearSideLightsWithTurnLight>b__3_3(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052BD RID: 21181 RVA: 0x003FA9BC File Offset: 0x003F8BBC
			internal void <PQ26_BlinkRearSideLightsWithTurnLight>b__3_5(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052BE RID: 21182 RVA: 0x003FA9BC File Offset: 0x003F8BBC
			internal void <PQ26_BlinkRearSideLightsWithTurnLight>b__3_7(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052BF RID: 21183 RVA: 0x003FA9BC File Offset: 0x003F8BBC
			internal void <PQ26_DimmRearSideLightsWithTurnLight>b__4_1(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052C0 RID: 21184 RVA: 0x003FA9BC File Offset: 0x003F8BBC
			internal void <PQ26_DimmRearSideLightsWithTurnLight>b__4_3(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052C1 RID: 21185 RVA: 0x003FA9BC File Offset: 0x003F8BBC
			internal void <PQ26_DimmRearSideLightsWithTurnLight>b__4_5(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052C2 RID: 21186 RVA: 0x003FA9BC File Offset: 0x003F8BBC
			internal void <PQ26_DimmRearSideLightsWithTurnLight>b__4_7(MQB_LightConfiguration config)
			{
				config.FunctionG = MQB_LightFunction.FromValue("00");
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x04003284 RID: 12932
			public static readonly ExteriorLights.<>c <>9 = new ExteriorLights.<>c();

			// Token: 0x04003285 RID: 12933
			public static Action<MQB_LightConfiguration> <>9__0_1;

			// Token: 0x04003286 RID: 12934
			public static Action<MQB_LightConfiguration> <>9__0_3;

			// Token: 0x04003287 RID: 12935
			public static Action<MQB_LightConfiguration> <>9__0_5;

			// Token: 0x04003288 RID: 12936
			public static Action<MQB_LightConfiguration> <>9__0_7;

			// Token: 0x04003289 RID: 12937
			public static Action<MQB_LightConfiguration> <>9__1_1;

			// Token: 0x0400328A RID: 12938
			public static Action<MQB_LightConfiguration> <>9__1_3;

			// Token: 0x0400328B RID: 12939
			public static Action<MQB_LightConfiguration> <>9__1_5;

			// Token: 0x0400328C RID: 12940
			public static Action<MQB_LightConfiguration> <>9__1_7;

			// Token: 0x0400328D RID: 12941
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__2_0;

			// Token: 0x0400328E RID: 12942
			public static Action<MQB_LightConfiguration> <>9__2_1;

			// Token: 0x0400328F RID: 12943
			public static Action<MQB_LightConfiguration> <>9__2_2;

			// Token: 0x04003290 RID: 12944
			public static Func<MQB_LightConfiguration, bool> <>9__2_3;

			// Token: 0x04003291 RID: 12945
			public static Action<MQB_LightConfiguration> <>9__3_1;

			// Token: 0x04003292 RID: 12946
			public static Action<MQB_LightConfiguration> <>9__3_3;

			// Token: 0x04003293 RID: 12947
			public static Action<MQB_LightConfiguration> <>9__3_5;

			// Token: 0x04003294 RID: 12948
			public static Action<MQB_LightConfiguration> <>9__3_7;

			// Token: 0x04003295 RID: 12949
			public static Action<MQB_LightConfiguration> <>9__4_1;

			// Token: 0x04003296 RID: 12950
			public static Action<MQB_LightConfiguration> <>9__4_3;

			// Token: 0x04003297 RID: 12951
			public static Action<MQB_LightConfiguration> <>9__4_5;

			// Token: 0x04003298 RID: 12952
			public static Action<MQB_LightConfiguration> <>9__4_7;
		}

		// Token: 0x02000A2B RID: 2603
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060052C3 RID: 21187 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060052C4 RID: 21188 RVA: 0x003FA9DC File Offset: 0x003F8BDC
			internal void <PQ26_RearSideLightsAsDRL_Rapid2020>b__0(MQB_LightConfiguration config)
			{
				config.FunctionE = this.drl_func;
				config.DimmwertEF = 40;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052C5 RID: 21189 RVA: 0x003FA9DC File Offset: 0x003F8BDC
			internal void <PQ26_RearSideLightsAsDRL_Rapid2020>b__2(MQB_LightConfiguration config)
			{
				config.FunctionE = this.drl_func;
				config.DimmwertEF = 40;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052C6 RID: 21190 RVA: 0x003FA9F9 File Offset: 0x003F8BF9
			internal void <PQ26_RearSideLightsAsDRL_Rapid2020>b__4(MQB_LightConfiguration config)
			{
				config.FunctionE = this.drl_func;
				config.DimmwertEF = 6;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052C7 RID: 21191 RVA: 0x003FA9F9 File Offset: 0x003F8BF9
			internal void <PQ26_RearSideLightsAsDRL_Rapid2020>b__6(MQB_LightConfiguration config)
			{
				config.FunctionE = this.drl_func;
				config.DimmwertEF = 6;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x04003299 RID: 12953
			public MQB_LightFunction drl_func;
		}

		// Token: 0x02000A2C RID: 2604
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x060052C8 RID: 21192 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x060052C9 RID: 21193 RVA: 0x003FAA15 File Offset: 0x003F8C15
			internal void <PQ26_RearSideLightsAsDRL_VWPolo2020>b__0(MQB_LightConfiguration config)
			{
				config.FunctionE = this.drl_func;
				config.DimmwertEF = 75;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052CA RID: 21194 RVA: 0x003FAA15 File Offset: 0x003F8C15
			internal void <PQ26_RearSideLightsAsDRL_VWPolo2020>b__2(MQB_LightConfiguration config)
			{
				config.FunctionE = this.drl_func;
				config.DimmwertEF = 75;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052CB RID: 21195 RVA: 0x003FAA15 File Offset: 0x003F8C15
			internal void <PQ26_RearSideLightsAsDRL_VWPolo2020>b__4(MQB_LightConfiguration config)
			{
				config.FunctionE = this.drl_func;
				config.DimmwertEF = 75;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052CC RID: 21196 RVA: 0x003FAA15 File Offset: 0x003F8C15
			internal void <PQ26_RearSideLightsAsDRL_VWPolo2020>b__6(MQB_LightConfiguration config)
			{
				config.FunctionE = this.drl_func;
				config.DimmwertEF = 75;
				config.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x0400329A RID: 12954
			public MQB_LightFunction drl_func;
		}

		// Token: 0x02000A2D RID: 2605
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060052CD RID: 21197 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060052CE RID: 21198 RVA: 0x003FAA32 File Offset: 0x003F8C32
			internal void <PQ26_BlinkRearSideLightsWithTurnLight>b__0(MQB_LightConfiguration config)
			{
				config.FunctionG = this.left_func;
				config.DimmwertGH = this.target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052CF RID: 21199 RVA: 0x003FAA53 File Offset: 0x003F8C53
			internal void <PQ26_BlinkRearSideLightsWithTurnLight>b__2(MQB_LightConfiguration config)
			{
				config.FunctionG = this.right_func;
				config.DimmwertGH = this.target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052D0 RID: 21200 RVA: 0x003FAA32 File Offset: 0x003F8C32
			internal void <PQ26_BlinkRearSideLightsWithTurnLight>b__4(MQB_LightConfiguration config)
			{
				config.FunctionG = this.left_func;
				config.DimmwertGH = this.target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052D1 RID: 21201 RVA: 0x003FAA53 File Offset: 0x003F8C53
			internal void <PQ26_BlinkRearSideLightsWithTurnLight>b__6(MQB_LightConfiguration config)
			{
				config.FunctionG = this.right_func;
				config.DimmwertGH = this.target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x0400329B RID: 12955
			public MQB_LightFunction left_func;

			// Token: 0x0400329C RID: 12956
			public int target_dimming;

			// Token: 0x0400329D RID: 12957
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A2E RID: 2606
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x060052D2 RID: 21202 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x060052D3 RID: 21203 RVA: 0x003FAA74 File Offset: 0x003F8C74
			internal void <PQ26_DimmRearSideLightsWithTurnLight>b__0(MQB_LightConfiguration config)
			{
				config.FunctionG = this.left_func;
				config.DimmwertGH = this.target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052D4 RID: 21204 RVA: 0x003FAA95 File Offset: 0x003F8C95
			internal void <PQ26_DimmRearSideLightsWithTurnLight>b__2(MQB_LightConfiguration config)
			{
				config.FunctionG = this.right_func;
				config.DimmwertGH = this.target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052D5 RID: 21205 RVA: 0x003FAA74 File Offset: 0x003F8C74
			internal void <PQ26_DimmRearSideLightsWithTurnLight>b__4(MQB_LightConfiguration config)
			{
				config.FunctionG = this.left_func;
				config.DimmwertGH = this.target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052D6 RID: 21206 RVA: 0x003FAA95 File Offset: 0x003F8C95
			internal void <PQ26_DimmRearSideLightsWithTurnLight>b__6(MQB_LightConfiguration config)
			{
				config.FunctionG = this.right_func;
				config.DimmwertGH = this.target_dimming;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x0400329E RID: 12958
			public MQB_LightFunction left_func;

			// Token: 0x0400329F RID: 12959
			public int target_dimming;

			// Token: 0x040032A0 RID: 12960
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A2F RID: 2607
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x060052D7 RID: 21207 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x060052D8 RID: 21208 RVA: 0x003FAAB6 File Offset: 0x003F8CB6
			internal void <PQ26_TurnOnStopSignalWhenDoorOpened>b__0(MQB_LightConfiguration config)
			{
				config.FunctionD = this.left_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052D9 RID: 21209 RVA: 0x003FAAD3 File Offset: 0x003F8CD3
			internal void <PQ26_TurnOnStopSignalWhenDoorOpened>b__1(MQB_LightConfiguration config)
			{
				config.FunctionD = this.off_func;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052DA RID: 21210 RVA: 0x003FAAE8 File Offset: 0x003F8CE8
			internal void <PQ26_TurnOnStopSignalWhenDoorOpened>b__2(MQB_LightConfiguration config)
			{
				config.FunctionD = this.right_func;
				config.DimmwertCD = 127;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x060052DB RID: 21211 RVA: 0x003FAAD3 File Offset: 0x003F8CD3
			internal void <PQ26_TurnOnStopSignalWhenDoorOpened>b__3(MQB_LightConfiguration config)
			{
				config.FunctionD = this.off_func;
				config.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}

			// Token: 0x040032A1 RID: 12961
			public MQB_LightFunction left_func;

			// Token: 0x040032A2 RID: 12962
			public MQB_LightFunction off_func;

			// Token: 0x040032A3 RID: 12963
			public MQB_LightFunction right_func;
		}

		// Token: 0x02000A30 RID: 2608
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x060052DC RID: 21212 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x060052DD RID: 21213 RVA: 0x003FAB05 File Offset: 0x003F8D05
			internal void <PQ26_DimmingDRLWhenTurnLightsOn>b__0(MQB_LightConfiguration config)
			{
				config.FunctionG = this.left_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052DE RID: 21214 RVA: 0x003FAB21 File Offset: 0x003F8D21
			internal void <PQ26_DimmingDRLWhenTurnLightsOn>b__1(MQB_LightConfiguration config)
			{
				config.FunctionG = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x060052DF RID: 21215 RVA: 0x003FAB36 File Offset: 0x003F8D36
			internal bool <PQ26_DimmingDRLWhenTurnLightsOn>b__2(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052E0 RID: 21216 RVA: 0x003FAB5A File Offset: 0x003F8D5A
			internal void <PQ26_DimmingDRLWhenTurnLightsOn>b__3(MQB_LightConfiguration config)
			{
				config.FunctionG = this.right_func;
				config.DimmwertGH = 0;
				config.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x060052E1 RID: 21217 RVA: 0x003FAB21 File Offset: 0x003F8D21
			internal void <PQ26_DimmingDRLWhenTurnLightsOn>b__4(MQB_LightConfiguration config)
			{
				config.FunctionG = this.off_func;
				config.DimmwertCD = 0;
			}

			// Token: 0x060052E2 RID: 21218 RVA: 0x003FAB36 File Offset: 0x003F8D36
			internal bool <PQ26_DimmingDRLWhenTurnLightsOn>b__5(MQB_LightConfiguration config)
			{
				return config.FunctionG == this.left_func && config.DimmwertGH == 0 && config.DimmingDirectionGH == MQB_LightConfiguration.DimmingDirection.Minimize;
			}

			// Token: 0x040032A4 RID: 12964
			public MQB_LightFunction left_func;

			// Token: 0x040032A5 RID: 12965
			public MQB_LightFunction off_func;

			// Token: 0x040032A6 RID: 12966
			public MQB_LightFunction right_func;
		}
	}
}
