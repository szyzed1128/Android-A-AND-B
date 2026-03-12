using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DTC.VagDTC;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000541 RID: 1345
	public static class DescriptionLoader
	{
		// Token: 0x06003243 RID: 12867 RVA: 0x0022D03C File Offset: 0x0022B23C
		internal static List<BrandAndDescription> GetDescriptionsForCode(string code, string brand)
		{
			code = code.Replace("(", "").Replace(")", "").Replace(" ", "")
				.Replace("-", "");
			List<BrandAndDescription> list = new List<BrandAndDescription>(2);
			string localizedOrDefaultDescriptionFromFile = DescriptionLoader.GetLocalizedOrDefaultDescriptionFromFile(code, "OBDII.db");
			if (!string.IsNullOrEmpty(localizedOrDefaultDescriptionFromFile))
			{
				list.Add(new BrandAndDescription("OBDII", localizedOrDefaultDescriptionFromFile));
			}
			if (!(brand == "Audi") && !(brand == "Volkswagen") && !(brand == "Seat") && !(brand == "Skoda") && !(brand == "Bentley") && !(brand == "Cupra"))
			{
				brand == "Jetta";
			}
			foreach (string text in DescriptionLoader.GetFilesForBrand(brand))
			{
				if (text != null)
				{
					string localizedOrDefaultDescriptionFromFile2 = DescriptionLoader.GetLocalizedOrDefaultDescriptionFromFile(code, text);
					if (!string.IsNullOrEmpty(localizedOrDefaultDescriptionFromFile2))
					{
						list.Add(new BrandAndDescription(brand, localizedOrDefaultDescriptionFromFile2));
					}
				}
			}
			return list;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x0022D150 File Offset: 0x0022B350
		private static string GetLocalizedOrDefaultDescriptionFromFile(string code, string file)
		{
			string text = null;
			try
			{
				string localizedFile = DescriptionLoader.GetLocalizedFile(file);
				if (!string.IsNullOrEmpty(localizedFile))
				{
					text = DescriptionLoader.GetDescriptionForCodeFromFile(code, localizedFile);
					if (string.IsNullOrEmpty(text))
					{
						text = DescriptionLoader.GetDescriptionForCodeFromFile(code, file);
					}
				}
				else
				{
					text = DescriptionLoader.GetDescriptionForCodeFromFile(code, file);
				}
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x0022D1A8 File Offset: 0x0022B3A8
		private static string[] package_files
		{
			get
			{
				if (DescriptionLoader._package_files == null)
				{
					DescriptionLoader._package_files = PackageFileReader.GetFilesInDirectory("dtcdb");
				}
				return DescriptionLoader._package_files;
			}
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x0022D1C8 File Offset: 0x0022B3C8
		private static string GetLocalizedFile(string file)
		{
			string twoLetterISOLanguageName = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
			string text = file + "." + twoLetterISOLanguageName;
			if (DescriptionLoader.package_files.Contains(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x0022D204 File Offset: 0x0022B404
		private static string[] GetFilesForBrand(string brand)
		{
			if (brand != null)
			{
				switch (brand.Length)
				{
				case 2:
				{
					char c = brand[0];
					if (c != 'D')
					{
						if (c != 'M')
						{
							goto IL_0FA9;
						}
						if (!(brand == "MG"))
						{
							goto IL_0FA9;
						}
						return new string[0];
					}
					else
					{
						if (!(brand == "DS"))
						{
							goto IL_0FA9;
						}
						goto IL_0CFD;
					}
					break;
				}
				case 3:
				{
					char c = brand[0];
					if (c <= 'Z')
					{
						switch (c)
						{
						case 'B':
							if (brand == "BAW")
							{
								return new string[0];
							}
							if (brand == "BMW")
							{
								goto IL_0CBB;
							}
							if (!(brand == "BYD"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						case 'C':
						case 'E':
						case 'H':
						case 'I':
							goto IL_0FA9;
						case 'D':
							if (!(brand == "DFM"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						case 'F':
							if (!(brand == "FAW"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						case 'G':
							if (!(brand == "GMC"))
							{
								goto IL_0FA9;
							}
							goto IL_0D5B;
						case 'J':
							if (!(brand == "JAC"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						case 'K':
							if (!(brand == "Kia"))
							{
								goto IL_0FA9;
							}
							goto IL_0DA6;
						default:
							if (c != 'R')
							{
								if (c != 'Z')
								{
									goto IL_0FA9;
								}
								if (!(brand == "ZAZ"))
								{
									goto IL_0FA9;
								}
							}
							else
							{
								if (!(brand == "RAM"))
								{
									goto IL_0FA9;
								}
								goto IL_0CEE;
							}
							break;
						}
					}
					else if (c <= 'Г')
					{
						if (c != 'В')
						{
							if (c != 'Г')
							{
								goto IL_0FA9;
							}
							if (!(brand == "ГАЗ"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						}
						else
						{
							if (!(brand == "ВАЗ"))
							{
								goto IL_0FA9;
							}
							goto IL_0F1C;
						}
					}
					else if (c != 'З')
					{
						if (c != 'У')
						{
							goto IL_0FA9;
						}
						if (!(brand == "УАЗ"))
						{
							goto IL_0FA9;
						}
						return new string[] { "УАЗ_Bosch_ME17.9.71.db" };
					}
					else if (!(brand == "ЗАЗ"))
					{
						goto IL_0FA9;
					}
					return new string[0];
				}
				case 4:
				{
					char c = brand[0];
					if (c <= 'F')
					{
						if (c != 'A')
						{
							if (c != 'F')
							{
								goto IL_0FA9;
							}
							if (brand == "Fiat")
							{
								goto IL_0D36;
							}
							if (!(brand == "Ford"))
							{
								goto IL_0FA9;
							}
							return new string[] { "Ford.db" };
						}
						else
						{
							if (!(brand == "Audi"))
							{
								goto IL_0FA9;
							}
							goto IL_0EF0;
						}
					}
					else
					{
						switch (c)
						{
						case 'J':
							if (!(brand == "Jeep"))
							{
								goto IL_0FA9;
							}
							goto IL_0CEE;
						case 'K':
						case 'N':
						case 'P':
						case 'Q':
						case 'R':
							goto IL_0FA9;
						case 'L':
							if (!(brand == "Lada"))
							{
								goto IL_0FA9;
							}
							goto IL_0F1C;
						case 'M':
							if (!(brand == "Mini"))
							{
								goto IL_0FA9;
							}
							goto IL_0CBB;
						case 'O':
							if (!(brand == "Opel"))
							{
								goto IL_0FA9;
							}
							goto IL_0D5B;
						case 'S':
							if (brand == "Saab")
							{
								return new string[] { "Saab.db" };
							}
							if (!(brand == "Seat"))
							{
								goto IL_0FA9;
							}
							goto IL_0EF0;
						case 'T':
							if (!(brand == "Tata"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						default:
							if (c != 'Л')
							{
								goto IL_0FA9;
							}
							if (!(brand == "Лада"))
							{
								goto IL_0FA9;
							}
							goto IL_0F1C;
						}
					}
					break;
				}
				case 5:
				{
					char c = brand[0];
					switch (c)
					{
					case 'A':
						if (!(brand == "Acura"))
						{
							goto IL_0FA9;
						}
						break;
					case 'B':
						if (!(brand == "Buick"))
						{
							goto IL_0FA9;
						}
						goto IL_0D5B;
					case 'C':
						if (!(brand == "Chery"))
						{
							goto IL_0FA9;
						}
						goto IL_0C9E;
					case 'D':
						if (brand == "Dodge")
						{
							goto IL_0CEE;
						}
						if (!(brand == "Dacia"))
						{
							goto IL_0FA9;
						}
						goto IL_0E8A;
					case 'E':
						if (!(brand == "Exeed"))
						{
							goto IL_0FA9;
						}
						goto IL_0C9E;
					case 'F':
					case 'N':
					case 'P':
					case 'Q':
					case 'T':
					case 'U':
					case 'W':
					case 'Y':
						goto IL_0FA9;
					case 'G':
						if (!(brand == "Geely"))
						{
							goto IL_0FA9;
						}
						return new string[0];
					case 'H':
						if (!(brand == "Honda"))
						{
							if (brand == "Hafei")
							{
								return new string[0];
							}
							if (brand == "Haima")
							{
								return new string[0];
							}
							if (!(brand == "Hover") && !(brand == "Haval"))
							{
								goto IL_0FA9;
							}
							goto IL_0D87;
						}
						break;
					case 'I':
						if (!(brand == "ISUZU"))
						{
							goto IL_0FA9;
						}
						goto IL_0D5B;
					case 'J':
						if (!(brand == "Jietu"))
						{
							goto IL_0FA9;
						}
						goto IL_0C9E;
					case 'K':
						if (!(brand == "Kaiyi"))
						{
							goto IL_0FA9;
						}
						goto IL_0C9E;
					case 'L':
						if (brand == "Lifan")
						{
							return new string[0];
						}
						if (brand == "Lotus")
						{
							return new string[0];
						}
						if (!(brand == "Lexus"))
						{
							goto IL_0FA9;
						}
						goto IL_0ED2;
					case 'M':
						if (brand == "Mando")
						{
							return new string[0];
						}
						if (!(brand == "Mazda"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Mazda.db" };
					case 'O':
						if (!(brand == "Omoda"))
						{
							goto IL_0FA9;
						}
						goto IL_0C9E;
					case 'R':
						if (brand == "Ravon")
						{
							goto IL_0D5B;
						}
						if (!(brand == "Rover"))
						{
							goto IL_0FA9;
						}
						return new string[0];
					case 'S':
						if (brand == "Scion")
						{
							return new string[0];
						}
						if (brand == "Smart")
						{
							return new string[0];
						}
						if (!(brand == "Skoda"))
						{
							goto IL_0FA9;
						}
						goto IL_0EF0;
					case 'V':
						if (!(brand == "Volvo"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Volvo.db" };
					case 'X':
						if (!(brand == "Xcite"))
						{
							goto IL_0FA9;
						}
						goto IL_0C9E;
					case 'Z':
						if (!(brand == "Zotye"))
						{
							goto IL_0FA9;
						}
						return new string[0];
					default:
						if (c != 'Т')
						{
							goto IL_0FA9;
						}
						if (!(brand == "Тайга"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Continental_Synerject_Easy_U.db" };
					}
					return new string[] { "Honda.db" };
				}
				case 6:
				{
					char c = brand[2];
					switch (c)
					{
					case 'b':
						if (!(brand == "Subaru"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Subaru.db" };
					case 'c':
					case 'd':
					case 'f':
					case 'h':
					case 'i':
					case 'j':
					case 'k':
					case 'p':
					case 'q':
						goto IL_0FA9;
					case 'e':
						if (!(brand == "Jaecoo"))
						{
							if (!(brand == "Daewoo"))
							{
								goto IL_0FA9;
							}
							goto IL_0D5B;
						}
						break;
					case 'g':
						if (!(brand == "Jaguar"))
						{
							goto IL_0FA9;
						}
						goto IL_0DCB;
					case 'l':
						if (brand == "Holden")
						{
							goto IL_0D5B;
						}
						if (!(brand == "Delphi"))
						{
							goto IL_0FA9;
						}
						if (SharedSettings.Current.ProfileUpdateAlias == "1a509bc4fd184dcbaafadb74b90e66bf" || SharedSettings.Current.ProfileUpdateAlias == "d0c52754b1594cf3a0a807c8739b746c")
						{
							return new string[] { "delphimt05.db" };
						}
						return new string[0];
					case 'm':
						if (!(brand == "Hummer"))
						{
							goto IL_0FA9;
						}
						goto IL_0D5B;
					case 'n':
						if (!(brand == "Lancia"))
						{
							goto IL_0FA9;
						}
						goto IL_0D36;
					case 'o':
						if (!(brand == "Proton"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Proton.db" };
					case 'r':
						if (!(brand == "Vortex"))
						{
							goto IL_0FA9;
						}
						return new string[0];
					case 's':
						if (!(brand == "Nissan"))
						{
							goto IL_0FA9;
						}
						goto IL_0E4E;
					case 't':
						if (!(brand == "Jetour"))
						{
							if (brand == "Datsun")
							{
								return new string[0];
							}
							if (!(brand == "Saturn"))
							{
								goto IL_0FA9;
							}
							goto IL_0D5B;
						}
						break;
					default:
						if (c != 'y')
						{
							if (c != 'z')
							{
								goto IL_0FA9;
							}
							if (!(brand == "Suzuki"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						}
						else
						{
							if (!(brand == "Toyota"))
							{
								goto IL_0FA9;
							}
							goto IL_0ED2;
						}
						break;
					}
					break;
				}
				case 7:
					switch (brand[4])
					{
					case 'a':
						if (!(brand == "Soueast"))
						{
							if (!(brand == "Ferrari"))
							{
								goto IL_0FA9;
							}
							goto IL_0D36;
						}
						break;
					case 'b':
					case 'f':
					case 'h':
					case 'j':
					case 'k':
					case 'm':
					case 'n':
					case 'p':
					case 'q':
						goto IL_0FA9;
					case 'c':
						if (!(brand == "Porsche"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Porsche.db" };
					case 'd':
						if (!(brand == "Hyundai"))
						{
							goto IL_0FA9;
						}
						goto IL_0DA6;
					case 'e':
						if (!(brand == "Peugeot"))
						{
							goto IL_0FA9;
						}
						goto IL_0CFD;
					case 'g':
						if (!(brand == "Changan"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Changan.db" };
					case 'i':
						if (!(brand == "Pontiac"))
						{
							goto IL_0FA9;
						}
						goto IL_0D5B;
					case 'l':
						if (!(brand == "Bentley"))
						{
							goto IL_0FA9;
						}
						goto IL_0EF0;
					case 'o':
						if (brand == "Citroen")
						{
							goto IL_0CFD;
						}
						if (brand == "Daytona")
						{
							return new string[0];
						}
						if (!(brand == "Lincoln"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Lincoln.db" };
					case 'r':
						if (!(brand == "McLaren"))
						{
							goto IL_0FA9;
						}
						return new string[0];
					case 's':
						if (!(brand == "Genesis"))
						{
							goto IL_0FA9;
						}
						goto IL_0DA6;
					case 't':
						if (!(brand == "Bugatti"))
						{
							goto IL_0FA9;
						}
						return new string[0];
					case 'u':
						if (brand == "Mercury")
						{
							return new string[] { "Mercury.db" };
						}
						if (!(brand == "Renault") && !(brand == "Samsung"))
						{
							goto IL_0FA9;
						}
						goto IL_0E8A;
					default:
						goto IL_0FA9;
					}
					break;
				case 8:
				{
					char c = brand[2];
					if (c <= 'n')
					{
						switch (c)
						{
						case 'd':
							if (!(brand == "Cadillac"))
							{
								goto IL_0FA9;
							}
							goto IL_0D5B;
						case 'e':
						case 'g':
							goto IL_0FA9;
						case 'f':
							if (!(brand == "Infiniti"))
							{
								goto IL_0FA9;
							}
							goto IL_0E4E;
						case 'h':
							if (!(brand == "Mahindra"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						case 'i':
							if (!(brand == "Daihatsu"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						default:
							if (c != 'n')
							{
								goto IL_0FA9;
							}
							if (!(brand == "Dongfeng"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						}
					}
					else
					{
						switch (c)
						{
						case 'r':
							if (!(brand == "Chrysler"))
							{
								goto IL_0FA9;
							}
							goto IL_0CEE;
						case 's':
							if (!(brand == "Maserati"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						case 't':
							goto IL_0FA9;
						case 'u':
							if (!(brand == "Vauxhall"))
							{
								goto IL_0FA9;
							}
							goto IL_0D5B;
						default:
							if (c != 'y')
							{
								goto IL_0FA9;
							}
							if (!(brand == "Plymouth"))
							{
								goto IL_0FA9;
							}
							return new string[] { "Plymouth.db" };
						}
					}
					break;
				}
				case 9:
				{
					char c = brand[0];
					if (c != 'C')
					{
						if (c != 'S')
						{
							goto IL_0FA9;
						}
						if (!(brand == "SsangYong"))
						{
							goto IL_0FA9;
						}
						return new string[0];
					}
					else
					{
						if (!(brand == "Chevrolet"))
						{
							goto IL_0FA9;
						}
						goto IL_0D5B;
					}
					break;
				}
				case 10:
				{
					char c = brand[0];
					if (c <= 'B')
					{
						if (c != 'A')
						{
							if (c != 'B')
							{
								goto IL_0FA9;
							}
							if (!(brand == "Brilliance"))
							{
								goto IL_0FA9;
							}
							return new string[0];
						}
						else
						{
							if (!(brand == "Alfa Romeo"))
							{
								goto IL_0FA9;
							}
							return new string[] { "AlfaRomeo.db" };
						}
					}
					else if (c != 'G')
					{
						switch (c)
						{
						case 'L':
							if (!(brand == "Land Rover"))
							{
								goto IL_0FA9;
							}
							goto IL_0DCB;
						case 'M':
							if (!(brand == "Mitsubishi"))
							{
								goto IL_0FA9;
							}
							return new string[] { "Mitsubishi.db" };
						case 'N':
							goto IL_0FA9;
						case 'O':
							if (!(brand == "Oldsmobile"))
							{
								goto IL_0FA9;
							}
							return new string[] { "Oldsmobile.db" };
						default:
							if (c != 'V')
							{
								goto IL_0FA9;
							}
							if (!(brand == "Volkswagen"))
							{
								goto IL_0FA9;
							}
							goto IL_0EF0;
						}
					}
					else
					{
						if (!(brand == "Great Wall"))
						{
							goto IL_0FA9;
						}
						goto IL_0D87;
					}
					break;
				}
				case 11:
				{
					char c = brand[0];
					if (c != 'L')
					{
						if (c != 'R')
						{
							goto IL_0FA9;
						}
						if (!(brand == "Range Rover"))
						{
							goto IL_0FA9;
						}
						goto IL_0DCB;
					}
					else
					{
						if (!(brand == "Lamborghini"))
						{
							goto IL_0FA9;
						}
						return new string[] { "Lambo.db" };
					}
					break;
				}
				case 12:
					if (!(brand == "Aston Martin"))
					{
						goto IL_0FA9;
					}
					return new string[0];
				case 13:
					if (!(brand == "Mercedes-Benz"))
					{
						goto IL_0FA9;
					}
					return new string[] { "Mercedes_Benz.db" };
				case 14:
					if (!(brand == "General Motors"))
					{
						goto IL_0FA9;
					}
					goto IL_0D5B;
				default:
					goto IL_0FA9;
				}
				IL_0C9E:
				return new string[] { "chery_cvt25.db" };
				IL_0CBB:
				return new string[] { "BMW.db" };
				IL_0CEE:
				return new string[] { "Chrysler.db" };
				IL_0CFD:
				return new string[] { "Peugeot_Citroen.db" };
				IL_0D36:
				return new string[] { "Fiat.db" };
				IL_0D5B:
				return new string[] { "GM.db" };
				IL_0D87:
				return new string[] { "havalh54wd.db", "havalh5bcm.db", "havalh5tod.db" };
				IL_0DA6:
				return new string[] { "Hyundai_Kia.db" };
				IL_0DCB:
				return new string[0];
				IL_0E4E:
				return new string[] { "Nissan.db" };
				IL_0E8A:
				return new string[] { "Renault.db" };
				IL_0ED2:
				return new string[] { "TOYOTA.db" };
				IL_0EF0:
				return new string[] { "VAG.db" };
				IL_0F1C:
				return new string[] { "ВАЗ_Bosch_M7.9.7.db", "ВАЗ_Январь_5.db", "ladavestangpp.db" };
			}
			IL_0FA9:
			return new string[0];
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x0022E1C0 File Offset: 0x0022C3C0
		internal static void ResetCache()
		{
			DescriptionLoader.CacheDictionary.Clear();
			VagDTCDecoder.ClearResources();
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x0022E1D4 File Offset: 0x0022C3D4
		internal static void PreloadDescriptionsForBrand(string brand)
		{
			string text = "OBDII.db";
			DescriptionLoader.LoadDescriptionsFromFile(text);
			string localizedFile = DescriptionLoader.GetLocalizedFile(text);
			if (localizedFile != null)
			{
				DescriptionLoader.LoadDescriptionsFromFile(localizedFile);
			}
			foreach (string text2 in DescriptionLoader.GetFilesForBrand(brand))
			{
				DescriptionLoader.LoadDescriptionsFromFile(text2);
				string localizedFile2 = DescriptionLoader.GetLocalizedFile(text2);
				if (localizedFile2 != null)
				{
					DescriptionLoader.LoadDescriptionsFromFile(localizedFile2);
				}
			}
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x0022E228 File Offset: 0x0022C428
		private static void LoadDescriptionsFromFile(string file)
		{
			try
			{
				Dictionary<string, string> dictionary = PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingStream<Dictionary<string, string>>(DescriptionLoader.dir + file, false);
				if (file.Contains('.'))
				{
					file = file.Substring(0, file.IndexOf('.'));
				}
				if (DescriptionLoader.CacheDictionary.ContainsKey(file))
				{
					Dictionary<string, string> dictionary2 = DescriptionLoader.CacheDictionary[file];
					using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							if (dictionary2.ContainsKey(text))
							{
								dictionary2[text] = dictionary2[text] + "\n" + dictionary[text];
							}
							else
							{
								dictionary2[text] = dictionary[text];
							}
						}
						goto IL_00BB;
					}
				}
				DescriptionLoader.CacheDictionary.Add(file, dictionary);
				IL_00BB:;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x0022E314 File Offset: 0x0022C514
		public static List<BrandAndDescription> GetDescriptionsForCode(string code)
		{
			List<BrandAndDescription> list = new List<BrandAndDescription>();
			string text = code.Replace("(", "").Replace(")", "").Replace(" ", "")
				.Replace("-", "");
			foreach (string text2 in DescriptionLoader.CacheDictionary.Keys)
			{
				Dictionary<string, string> dictionary = DescriptionLoader.CacheDictionary[text2];
				try
				{
					string text3 = null;
					if (dictionary.TryGetValue(text, out text3))
					{
						list.Add(new BrandAndDescription(text2, text3));
					}
					else
					{
						if (text.Length > 5)
						{
							text = text.Substring(0, 5);
						}
						if (dictionary.TryGetValue(text, out text3))
						{
							list.Add(new BrandAndDescription(text2, text3));
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return list;
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x0022E414 File Offset: 0x0022C614
		private static string GetDescriptionForCodeFromFile(string code, string file)
		{
			Dictionary<string, string> dictionary = null;
			if (!DescriptionLoader.CacheDictionary.TryGetValue(file, out dictionary))
			{
				dictionary = PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingStream<Dictionary<string, string>>(DescriptionLoader.dir + file, false);
				DescriptionLoader.CacheDictionary.Add(file, dictionary);
			}
			string text = null;
			string text2;
			try
			{
				if (dictionary.TryGetValue(code, out text))
				{
					text2 = text;
				}
				else
				{
					if (!file.Contains("VAG"))
					{
						if (code.Length > 5)
						{
							code = code.Substring(0, 5);
						}
						if (dictionary.TryGetValue(code, out text))
						{
							return text;
						}
					}
					text2 = null;
				}
			}
			catch (Exception)
			{
				text2 = null;
			}
			return text2;
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x0022E4AC File Offset: 0x0022C6AC
		// Note: this type is marked as 'beforefieldinit'.
		static DescriptionLoader()
		{
		}

		// Token: 0x04001D34 RID: 7476
		private static string dir = "dtcdb.";

		// Token: 0x04001D35 RID: 7477
		private static string[] _package_files = null;

		// Token: 0x04001D36 RID: 7478
		private static Dictionary<string, Dictionary<string, string>> CacheDictionary = new Dictionary<string, Dictionary<string, string>>();
	}
}
