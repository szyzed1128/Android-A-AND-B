using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007C2 RID: 1986
	public static class CSVLoader
	{
		// Token: 0x06004684 RID: 18052 RVA: 0x0036A5C8 File Offset: 0x003687C8
		public static IEnumerable<CustomPID> LoadFromCSV(string filename)
		{
			IEnumerable<CustomPID> enumerable;
			try
			{
				string text = "";
				using (StreamReader streamReader = new StreamReader(filename))
				{
					text = streamReader.ReadToEnd();
				}
				string[] array = text.Split(new char[] { '\r', '\n' });
				array = array.Where((string x) => !string.IsNullOrEmpty(x)).ToArray<string>();
				List<CustomPID> list = new List<CustomPID>(array.Length - 1);
				for (int i = 1; i < array.Length; i++)
				{
					try
					{
						CustomPID customPID = CSVLoader.CSVLineToPID(array[i], '\0');
						list.Add(customPID);
					}
					catch (Exception)
					{
					}
				}
				enumerable = list;
			}
			catch (Exception)
			{
				enumerable = new List<CustomPID>();
			}
			return enumerable;
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x0036A6A8 File Offset: 0x003688A8
		public static IEnumerable<CustomPID> LoadFromCSV(Stream stream)
		{
			IEnumerable<CustomPID> enumerable;
			try
			{
				string text = "";
				using (StreamReader streamReader = new StreamReader(stream))
				{
					text = streamReader.ReadToEnd();
				}
				string[] array = text.Split(new char[] { '\r', '\n' });
				array = array.Where((string x) => !string.IsNullOrEmpty(x)).ToArray<string>();
				List<CustomPID> list = new List<CustomPID>(array.Length - 1);
				for (int i = 1; i < array.Length; i++)
				{
					try
					{
						CustomPID customPID = CSVLoader.CSVLineToPID(array[i], '\0');
						list.Add(customPID);
					}
					catch (Exception)
					{
					}
				}
				enumerable = list;
			}
			catch (Exception)
			{
				enumerable = new List<CustomPID>();
			}
			return enumerable;
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x0036A788 File Offset: 0x00368988
		public static CustomPID CSVLineToPID(string line, char override_separator = '\0')
		{
			int num = line.Count((char x) => x == ',');
			int num2 = line.Count((char x) => x == ';');
			char c = ((num > num2) ? ',' : ';');
			if (override_separator != '\0')
			{
				c = override_separator;
			}
			string[] array = line.Split(new char[] { c });
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].StartsWith("\"") && array[i].EndsWith("\""))
				{
					array[i] = array[i].Substring(1);
					array[i] = array[i].Substring(0, array[i].Length - 1);
				}
			}
			string text = array[0];
			string text2 = array[1];
			string text3 = array[2].Trim();
			if (text3.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				text3 = text3.Substring(2);
			}
			text3 = text3.ToUpperInvariant();
			string text4 = array[3].Trim().Replace(" ", "").Replace("IF", "if");
			if (!text4.Contains("GetBit"))
			{
				text4 = CSVLoader.FormulaFix(text4);
			}
			array[4] = array[4].Replace(".", Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			array[5] = array[5].Replace(".", Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			double num3 = 0.0;
			try
			{
				num3 = double.Parse(array[4].Trim());
			}
			catch
			{
				num3 = 0.0;
			}
			double num4 = 100.0;
			try
			{
				num4 = double.Parse(array[5].Trim());
			}
			catch
			{
				num4 = 100.0;
			}
			UnitsHelper.Units units = CSVLoader.StringToUnits(array[6]);
			string text5 = "";
			if (array.Length >= 8)
			{
				text5 = array[7].ToUpperInvariant().Trim();
			}
			string text6 = "";
			try
			{
				text6 = array[8];
			}
			catch
			{
			}
			string text7 = "";
			try
			{
				text7 = array[9];
			}
			catch
			{
			}
			return new CustomPID(text, text2, text3, text5, text4, units, num3, num4, text6, text7, false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x0036AA60 File Offset: 0x00368C60
		public static UnitsHelper.Units StringToUnits(string input)
		{
			string l_input = input.ToLowerInvariant();
			KeyValuePair<string, UnitsHelper.Units> keyValuePair = CSVLoader.StringUnitsDictionary.FirstOrDefault((KeyValuePair<string, UnitsHelper.Units> x) => x.Key.ToLowerInvariant() == l_input);
			if (!string.IsNullOrEmpty(keyValuePair.Key))
			{
				return keyValuePair.Value;
			}
			return CSVLoader.TryGetUnitsFromText(input);
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x0036AAB4 File Offset: 0x00368CB4
		public static UnitsHelper.Units TryGetUnitsFromText(string s)
		{
			if (s == null)
			{
				return UnitsHelper.Units.None;
			}
			s = s.ToLowerInvariant().Trim();
			if (string.IsNullOrEmpty(s))
			{
				return UnitsHelper.Units.None;
			}
			int num = UnitsHelper.UnitsCaptionCollection.FindIndex((string x) => x.ToLowerInvariant().Trim() == s);
			if (num < 0)
			{
				num = UnitsHelper.UnitsCaptionCollection.FindIndex((string x) => x.ToLowerInvariant().Trim() == s);
			}
			if (num >= 0)
			{
				return (UnitsHelper.Units)num;
			}
			Console.WriteLine("UNITS NOT FOUND: " + s);
			return UnitsHelper.Units.None;
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x0036AB4B File Offset: 0x00368D4B
		private static string FormulaFix(string s)
		{
			s = CSVLoader.FormulaFix_ReplaceTorqueGetBit(s);
			s = CSVLoader.FormulaFix_ReplaceSigned(s);
			s = CSVLoader.FormulaFix_ReplaceBitOperations(s);
			s = s.Replace(':', ',');
			return s;
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x0036AB74 File Offset: 0x00368D74
		public static string FormulaFix_ReplaceBitOperations(string s)
		{
			s = s.Replace("&", "@&");
			s = s.Replace("<", "@<<");
			s = s.Replace(">", "@>>");
			s = s.Replace("^", "@^");
			return s;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x0036ABCA File Offset: 0x00368DCA
		private static string FormulaFix_ReplaceSigned(string s)
		{
			return s.Replace("signed", "SIGNED");
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x0036ABDC File Offset: 0x00368DDC
		private static string FormulaFix_ReplaceTorqueGetBit(string s)
		{
			int num = s.IndexOf('{');
			if (num < 0)
			{
				return s;
			}
			int num2 = s.IndexOf(':', num);
			if (num2 < 0)
			{
				return s;
			}
			int num3 = s.IndexOf('}', num2);
			if (num3 < 0)
			{
				return s;
			}
			if (num >= 0 && num2 >= 0 && num3 >= 0)
			{
				string text = s.Substring(num + 1, num2 - (num + 1));
				string text2 = s.Substring(num2 + 1, num3 - 1 - num2);
				string text3 = string.Concat(new string[] { "GetBit(", text, ",", text2, ")" });
				if (num > 0)
				{
					text3 = s.Substring(0, num) + text3;
				}
				if (num3 < s.Length - 1)
				{
					text3 += s.Substring(num3 + 1);
				}
				s = CSVLoader.FormulaFix_ReplaceTorqueGetBit(text3);
				return s;
			}
			return s;
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x0036ACB8 File Offset: 0x00368EB8
		public static string FixAndFunc(string input)
		{
			if (input.Contains("@"))
			{
				return input;
			}
			return input.Replace("&", "@&");
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x0036ACDC File Offset: 0x00368EDC
		// Note: this type is marked as 'beforefieldinit'.
		static CSVLoader()
		{
		}

		// Token: 0x040028F9 RID: 10489
		private static Dictionary<string, UnitsHelper.Units> StringUnitsDictionary = new Dictionary<string, UnitsHelper.Units>
		{
			{
				"mg/cyc",
				UnitsHelper.Units.mgpc
			},
			{
				"mg/hub",
				UnitsHelper.Units.mg_stroke
			},
			{
				"hPa",
				UnitsHelper.Units.hPa
			},
			{
				"В°C",
				UnitsHelper.Units.celicium
			},
			{
				"*C",
				UnitsHelper.Units.celicium
			},
			{
				"bar",
				UnitsHelper.Units.bar
			},
			{
				"V",
				UnitsHelper.Units.volts
			},
			{
				"volts",
				UnitsHelper.Units.volts
			},
			{
				"Volts",
				UnitsHelper.Units.volts
			},
			{
				"%",
				UnitsHelper.Units.percent
			},
			{
				"Nm",
				UnitsHelper.Units.Nm
			},
			{
				"mV",
				UnitsHelper.Units.mV
			},
			{
				"мкс",
				UnitsHelper.Units.microseconds
			},
			{
				"мбар",
				UnitsHelper.Units.mbar
			},
			{
				"s",
				UnitsHelper.Units.seconds
			},
			{
				"W",
				UnitsHelper.Units.W
			},
			{
				"Ом",
				UnitsHelper.Units.Ohm
			},
			{
				"km",
				UnitsHelper.Units.km
			},
			{
				"",
				UnitsHelper.Units.None
			},
			{
				"град С",
				UnitsHelper.Units.celicium
			},
			{
				"мм",
				UnitsHelper.Units.mm
			},
			{
				"мин",
				UnitsHelper.Units.minutes
			},
			{
				"*V",
				UnitsHelper.Units.grads
			},
			{
				"км/час",
				UnitsHelper.Units.kmh
			},
			{
				"л.",
				UnitsHelper.Units.liters
			},
			{
				"об/мин",
				UnitsHelper.Units.rpm
			},
			{
				"час",
				UnitsHelper.Units.h
			},
			{
				"м3/ч",
				UnitsHelper.Units.m3_hour
			},
			{
				"mg/cp",
				UnitsHelper.Units.mgpc
			},
			{
				"грамм",
				UnitsHelper.Units.gramms
			},
			{
				"g/s",
				UnitsHelper.Units.grams_sec
			},
			{
				"d*",
				UnitsHelper.Units.grads
			},
			{
				"*/с",
				UnitsHelper.Units.grads
			},
			{
				"м/с?",
				UnitsHelper.Units.m_sec2
			},
			{
				"ДЕНЬ",
				UnitsHelper.Units.days
			},
			{
				"Литр",
				UnitsHelper.Units.liters
			},
			{
				"°C",
				UnitsHelper.Units.celicium
			},
			{
				"°Crk",
				UnitsHelper.Units.grads
			},
			{
				"kg/h",
				UnitsHelper.Units.kg_h
			},
			{
				"mbar",
				UnitsHelper.Units.mbar
			},
			{
				"µs",
				UnitsHelper.Units.microseconds
			},
			{
				"A",
				UnitsHelper.Units.A
			},
			{
				"kWh",
				UnitsHelper.Units.kWh
			},
			{
				"KWh",
				UnitsHelper.Units.kWh
			},
			{
				"Wh",
				UnitsHelper.Units.Wh
			},
			{
				"C",
				UnitsHelper.Units.celicium
			},
			{
				"N-m",
				UnitsHelper.Units.Nm
			},
			{
				"?",
				UnitsHelper.Units.None
			},
			{
				"Volt",
				UnitsHelper.Units.volts
			},
			{
				"deg C",
				UnitsHelper.Units.celicium
			},
			{
				"Watt",
				UnitsHelper.Units.W
			},
			{
				"Mohm",
				UnitsHelper.Units.MOhm
			},
			{
				"F",
				UnitsHelper.Units.fahrengheit
			},
			{
				"g/L",
				UnitsHelper.Units.g_L
			},
			{
				"Mg/Strk",
				UnitsHelper.Units.mg_stroke
			},
			{
				"mm^3",
				UnitsHelper.Units.mm3
			},
			{
				"Deg",
				UnitsHelper.Units.grads
			},
			{
				"degree",
				UnitsHelper.Units.grads
			},
			{
				"volt",
				UnitsHelper.Units.volts
			},
			{
				"m/s2",
				UnitsHelper.Units.m_sec2
			},
			{
				"°F",
				UnitsHelper.Units.fahrengheit
			},
			{
				"°",
				UnitsHelper.Units.grads
			},
			{
				"litre",
				UnitsHelper.Units.liters
			},
			{
				"m3/h",
				UnitsHelper.Units.m3_hour
			},
			{
				"revs",
				UnitsHelper.Units.revs
			},
			{
				"d°",
				UnitsHelper.Units.grads
			},
			{
				"°/s",
				UnitsHelper.Units.grads_sec
			},
			{
				"degrees",
				UnitsHelper.Units.grads
			},
			{
				"degrees/s",
				UnitsHelper.Units.grads_sec
			},
			{
				"gm/sec",
				UnitsHelper.Units.grams_sec
			},
			{
				"grads",
				UnitsHelper.Units.grads
			}
		};

		// Token: 0x020007C3 RID: 1987
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600468F RID: 18063 RVA: 0x0036B07D File Offset: 0x0036927D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004690 RID: 18064 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004691 RID: 18065 RVA: 0x000650BC File Offset: 0x000632BC
			internal bool <LoadFromCSV>b__0_0(string x)
			{
				return !string.IsNullOrEmpty(x);
			}

			// Token: 0x06004692 RID: 18066 RVA: 0x000650BC File Offset: 0x000632BC
			internal bool <LoadFromCSV>b__1_0(string x)
			{
				return !string.IsNullOrEmpty(x);
			}

			// Token: 0x06004693 RID: 18067 RVA: 0x0036B089 File Offset: 0x00369289
			internal bool <CSVLineToPID>b__2_0(char x)
			{
				return x == ',';
			}

			// Token: 0x06004694 RID: 18068 RVA: 0x0036B090 File Offset: 0x00369290
			internal bool <CSVLineToPID>b__2_1(char x)
			{
				return x == ';';
			}

			// Token: 0x040028FA RID: 10490
			public static readonly CSVLoader.<>c <>9 = new CSVLoader.<>c();

			// Token: 0x040028FB RID: 10491
			public static Func<string, bool> <>9__0_0;

			// Token: 0x040028FC RID: 10492
			public static Func<string, bool> <>9__1_0;

			// Token: 0x040028FD RID: 10493
			public static Func<char, bool> <>9__2_0;

			// Token: 0x040028FE RID: 10494
			public static Func<char, bool> <>9__2_1;
		}

		// Token: 0x020007C4 RID: 1988
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06004695 RID: 18069 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06004696 RID: 18070 RVA: 0x0036B097 File Offset: 0x00369297
			internal bool <StringToUnits>b__0(KeyValuePair<string, UnitsHelper.Units> x)
			{
				return x.Key.ToLowerInvariant() == this.l_input;
			}

			// Token: 0x040028FF RID: 10495
			public string l_input;
		}

		// Token: 0x020007C5 RID: 1989
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06004697 RID: 18071 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06004698 RID: 18072 RVA: 0x0036B0B0 File Offset: 0x003692B0
			internal bool <TryGetUnitsFromText>b__0(string x)
			{
				return x.ToLowerInvariant().Trim() == this.s;
			}

			// Token: 0x06004699 RID: 18073 RVA: 0x0036B0B0 File Offset: 0x003692B0
			internal bool <TryGetUnitsFromText>b__1(string x)
			{
				return x.ToLowerInvariant().Trim() == this.s;
			}

			// Token: 0x04002900 RID: 10496
			public string s;
		}
	}
}
