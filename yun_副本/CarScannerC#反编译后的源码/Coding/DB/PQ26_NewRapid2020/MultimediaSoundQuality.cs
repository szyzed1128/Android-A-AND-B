using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020
{
	// Token: 0x02000A3E RID: 2622
	internal static class MultimediaSoundQuality
	{
		// Token: 0x06005317 RID: 21271 RVA: 0x003FC1EC File Offset: 0x003FA3EC
		public static ICodingContainer PQ26AudioDatasets()
		{
			Func<byte[], MQBDataSet2EWriteOnly, string> func = delegate(byte[] data, MQBDataSet2EWriteOnly coding)
			{
				if (data == null || data.Length < 6)
				{
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
				try
				{
					int i = 0;
					while (i < data.Length)
					{
						byte[] array = new byte[7];
						Array.Copy(data, i, array, 0, array.Length);
						string text = BitHelpers.ByteArrayToHexString(array);
						if (text.StartsWith(coding.Address))
						{
							string version = text.Substring(4);
							version = version.Substring(0, version.Length - 2);
							MQBAdaptationOption mqbadaptationOption = coding.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value.StartsWith(version));
							if (mqbadaptationOption != null)
							{
								return mqbadaptationOption.Title;
							}
							break;
						}
						else
						{
							i += 7;
						}
					}
				}
				catch (Exception)
				{
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			};
			return new MQBDataSet2EWriteOnly("7201", CodingGroup.MultimediaSoundQuality, "Sound processing preset (MIB3/Bolero/7201)", "WARNING! THIS FUNCTION IS EXPERIMENTAL! DON'T USE IT!\nThis feature changes sound configuration to improve sound quality.", "Attention! Some variants doesn't support equlizer. It would dissappear after multimedia unit restart. To bring it back - apply option with equalizer and restart your multimedia system.\nWARNING! THIS FUNCTION IS EXPERIMENTAL! DON'T USE IT! There's no way to revert this operation. You can't use coding history to revert it.\nOnce you have applied sound processing preset, you can only switch between other presets, but you can't clear it.\nThis function requires high quality ELM327 device!", "20103", "1003;1040", "1003;1040;22F1A0;22F1A1;2704;2EF198;2EF199;2EF1A0;2EF1A1;", "", "5F", "vag.mib3soundpq26", "F1B1", func)
			{
				Translations = 
				{
					new TranslationItem("ru", "Предустановки обработки звука (MIB3/Bolero/7201)", "Этот пункт изменяет предустановки, ответственные за обработку звука (\"параметрию\")\nВНИМАНИЕ! ЭКСПЕРИМЕНТАЛЬНАЯ ФУНКЦИЯ! НЕ ИСПОЛЬЗУЙТЕ ЕЕ!", "\nВНИМАНИЕ! Эту операцию невозможно отменить! ФУНКЦИЯ ЭКСПЕРИМЕНТАЛЬНАЯ! НЕ ИСПОЛЬЗУЙТЕ ЕЕ! ПОМНИТЕ, ЧТО ВСЕ НА ВАШ СТРАХ И РИСК!\nВы сможете изменять варианты предустановок, но не сможете возвращаться к первоначальным через историю кодирования.\nПредупреждение! С некоторыми вариантами после перезагрузки или выключения мультимедийной системы пропадет эквалайзер. Эквалайзер можно вернуть, если применить вариант с поддержкой эквалайзера и перезагрузить мультимедиа.\nДля корректной работы этого пункта необходим качественный адаптер ELM327!")
				}
			};
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x003FC27C File Offset: 0x003FA47C
		public static ICodingContainer Rapid2020SoundAnn7202()
		{
			Func<byte[], MQBDataSet2EWriteOnly, string> func = delegate(byte[] data, MQBDataSet2EWriteOnly coding)
			{
				if (data == null || data.Length < 6)
				{
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
				try
				{
					int i = 0;
					while (i < data.Length)
					{
						byte[] array = new byte[7];
						Array.Copy(data, i, array, 0, array.Length);
						string text = BitHelpers.ByteArrayToHexString(array);
						if (text.StartsWith(coding.Address))
						{
							string version = text.Substring(4);
							version = version.Substring(0, version.Length - 2);
							MQBAdaptationOption mqbadaptationOption = coding.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value.StartsWith(version));
							if (mqbadaptationOption != null)
							{
								return mqbadaptationOption.Title;
							}
							break;
						}
						else
						{
							i += 7;
						}
					}
				}
				catch (Exception)
				{
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			};
			return new MQBDataSet2EWriteOnly("7202", CodingGroup.MultimediaSoundQuality, "Speakers preset (MIB3/7202)", "WARNING! THIS FUNCTION IS EXPERIMENTAL! DON'T USE IT!\nThis feature is a part of speakers configuration change", "\nWARNING! THIS FUNCTION IS EXPERIMENTAL! DON'T USE IT! There's no way to revert this operation. You can't use coding history to revert it.\nThis function requires high quality ELM327 device!", "20103", "1003;1040", "1003;1040;22F1A0;22F1A1;2704;2EF198;2EF199;2EF1A0;2EF1A1;", "", "5F", "vag.rapid2020_7202_sound_ann", "F1B1", func)
			{
				Translations = 
				{
					new TranslationItem("ru", "Предустановки колонок (MIB3/7202)", "Изменение части параметрии, ответственной за колонки на стандартную с 6 динамиками (4 впереди + 2 сзади)\nВНИМАНИЕ! ЭКСПЕРИМЕНТАЛЬНАЯ ФУНКЦИЯ! НЕ ИСПОЛЬЗУЙТЕ ЕЕ!", "\nВНИМАНИЕ! Эту операцию невозможно отменить! ФУНКЦИЯ ЭКСПЕРИМЕНТАЛЬНАЯ! НЕ ИСПОЛЬЗУЙТЕ ЕЕ! ПОМНИТЕ, ЧТО ВСЕ НА ВАШ СТРАХ И РИСК!\nДля корректной работы этого пункта необходим качественный адаптер ELM327!")
				}
			};
		}

		// Token: 0x02000A3F RID: 2623
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005319 RID: 21273 RVA: 0x003FC30C File Offset: 0x003FA50C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600531A RID: 21274 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600531B RID: 21275 RVA: 0x003FC318 File Offset: 0x003FA518
			internal string <PQ26AudioDatasets>b__0_0(byte[] data, MQBDataSet2EWriteOnly coding)
			{
				if (data == null || data.Length < 6)
				{
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
				try
				{
					int i = 0;
					while (i < data.Length)
					{
						byte[] array = new byte[7];
						Array.Copy(data, i, array, 0, array.Length);
						string text = BitHelpers.ByteArrayToHexString(array);
						if (text.StartsWith(coding.Address))
						{
							MultimediaSoundQuality.<>c__DisplayClass0_0 CS$<>8__locals1 = new MultimediaSoundQuality.<>c__DisplayClass0_0();
							CS$<>8__locals1.version = text.Substring(4);
							CS$<>8__locals1.version = CS$<>8__locals1.version.Substring(0, CS$<>8__locals1.version.Length - 2);
							MQBAdaptationOption mqbadaptationOption = coding.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value.StartsWith(CS$<>8__locals1.version));
							if (mqbadaptationOption != null)
							{
								return mqbadaptationOption.Title;
							}
							break;
						}
						else
						{
							i += 7;
						}
					}
				}
				catch (Exception)
				{
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x0600531C RID: 21276 RVA: 0x003FC3E8 File Offset: 0x003FA5E8
			internal string <Rapid2020SoundAnn7202>b__1_0(byte[] data, MQBDataSet2EWriteOnly coding)
			{
				if (data == null || data.Length < 6)
				{
					return MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
				try
				{
					int i = 0;
					while (i < data.Length)
					{
						byte[] array = new byte[7];
						Array.Copy(data, i, array, 0, array.Length);
						string text = BitHelpers.ByteArrayToHexString(array);
						if (text.StartsWith(coding.Address))
						{
							MultimediaSoundQuality.<>c__DisplayClass1_0 CS$<>8__locals1 = new MultimediaSoundQuality.<>c__DisplayClass1_0();
							CS$<>8__locals1.version = text.Substring(4);
							CS$<>8__locals1.version = CS$<>8__locals1.version.Substring(0, CS$<>8__locals1.version.Length - 2);
							MQBAdaptationOption mqbadaptationOption = coding.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value.StartsWith(CS$<>8__locals1.version));
							if (mqbadaptationOption != null)
							{
								return mqbadaptationOption.Title;
							}
							break;
						}
						else
						{
							i += 7;
						}
					}
				}
				catch (Exception)
				{
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x040032DE RID: 13022
			public static readonly MultimediaSoundQuality.<>c <>9 = new MultimediaSoundQuality.<>c();

			// Token: 0x040032DF RID: 13023
			public static Func<byte[], MQBDataSet2EWriteOnly, string> <>9__0_0;

			// Token: 0x040032E0 RID: 13024
			public static Func<byte[], MQBDataSet2EWriteOnly, string> <>9__1_0;
		}

		// Token: 0x02000A40 RID: 2624
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x0600531D RID: 21277 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x0600531E RID: 21278 RVA: 0x003FC4B8 File Offset: 0x003FA6B8
			internal bool <PQ26AudioDatasets>b__1(MQBAdaptationOption x)
			{
				return x.Value.StartsWith(this.version);
			}

			// Token: 0x040032E1 RID: 13025
			public string version;
		}

		// Token: 0x02000A41 RID: 2625
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x0600531F RID: 21279 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06005320 RID: 21280 RVA: 0x003FC4CB File Offset: 0x003FA6CB
			internal bool <Rapid2020SoundAnn7202>b__1(MQBAdaptationOption x)
			{
				return x.Value.StartsWith(this.version);
			}

			// Token: 0x040032E2 RID: 13026
			public string version;
		}
	}
}
