using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x020002EB RID: 747
	internal static class CAN11bitHelper
	{
		// Token: 0x06002351 RID: 9041 RVA: 0x001AEF30 File Offset: 0x001AD130
		public static string GetPossibleResponseHeader(string requestHeader, string brand, OBDRequest request)
		{
			if (request != null && request.BeforeCommands != null && request.BeforeCommands.Length != 0)
			{
				string text = request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase));
				if (text != null)
				{
					return text.ToUpperInvariant().Replace(" ", "").Replace("ATCRA", "");
				}
			}
			int num = 0;
			if (!int.TryParse(requestHeader, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
			{
				return "";
			}
			if (2016 <= num && num <= 2023)
			{
				return (num + 8).ToString("X3");
			}
			if (brand != null)
			{
				switch (brand.Length)
				{
				case 2:
					if (!(brand == "DS"))
					{
						goto IL_04F7;
					}
					goto IL_048B;
				case 3:
				case 9:
					goto IL_04F7;
				case 4:
				{
					char c = brand[0];
					if (c != 'A')
					{
						if (c != 'S')
						{
							goto IL_04F7;
						}
						if (!(brand == "Seat"))
						{
							goto IL_04F7;
						}
						goto IL_0478;
					}
					else
					{
						if (!(brand == "Audi"))
						{
							goto IL_04F7;
						}
						goto IL_0478;
					}
					break;
				}
				case 5:
				{
					char c = brand[1];
					if (c <= 'i')
					{
						if (c <= 'S')
						{
							if (c != 'P')
							{
								if (c != 'S')
								{
									goto IL_04F7;
								}
								if (!(brand == "ISUZU"))
								{
									goto IL_04F7;
								}
								goto IL_04E1;
							}
							else
							{
								if (!(brand == "XPeng"))
								{
									goto IL_04F7;
								}
								goto IL_04E1;
							}
						}
						else if (c != 'a')
						{
							if (c != 'i')
							{
								goto IL_04F7;
							}
							if (!(brand == "Lifan"))
							{
								goto IL_04F7;
							}
							goto IL_04E1;
						}
						else if (!(brand == "Dacia"))
						{
							if (!(brand == "Haval"))
							{
								goto IL_04F7;
							}
							goto IL_04CE;
						}
					}
					else if (c <= 'o')
					{
						if (c != 'k')
						{
							if (c != 'o')
							{
								goto IL_04F7;
							}
							if (!(brand == "Hover"))
							{
								goto IL_04F7;
							}
							goto IL_04CE;
						}
						else
						{
							if (!(brand == "Skoda"))
							{
								goto IL_04F7;
							}
							goto IL_0478;
						}
					}
					else if (c != 's')
					{
						if (c != 'u')
						{
							goto IL_04F7;
						}
						if (!(brand == "Cupra"))
						{
							goto IL_04F7;
						}
						goto IL_0478;
					}
					else
					{
						if (!(brand == "Isuzu"))
						{
							goto IL_04F7;
						}
						goto IL_04E1;
					}
					break;
				}
				case 6:
				{
					char c = brand[0];
					if (c != 'N')
					{
						if (c != 'S')
						{
							goto IL_04F7;
						}
						if (!(brand == "Suzuki"))
						{
							goto IL_04F7;
						}
						if (requestHeader[0] == '2')
						{
							return "6" + requestHeader.Substring(1, 2);
						}
						return (num + 8).ToString("X3");
					}
					else if (!(brand == "Nissan"))
					{
						goto IL_04F7;
					}
					break;
				}
				case 7:
				{
					char c = brand[0];
					if (c != 'B')
					{
						if (c != 'C')
						{
							switch (c)
							{
							case 'P':
								if (brand == "Porsche")
								{
									goto IL_0478;
								}
								if (!(brand == "Peugeot"))
								{
									goto IL_04F7;
								}
								goto IL_048B;
							case 'Q':
								goto IL_04F7;
							case 'R':
								if (!(brand == "Renault"))
								{
									goto IL_04F7;
								}
								break;
							case 'S':
								if (!(brand == "Samsung"))
								{
									goto IL_04F7;
								}
								break;
							default:
								goto IL_04F7;
							}
						}
						else
						{
							if (!(brand == "Citroen"))
							{
								goto IL_04F7;
							}
							goto IL_048B;
						}
					}
					else
					{
						if (!(brand == "Bentley"))
						{
							goto IL_04F7;
						}
						goto IL_0478;
					}
					break;
				}
				case 8:
				{
					char c = brand[7];
					if (c != 'g')
					{
						if (c != 'i')
						{
							if (c != 'y')
							{
								goto IL_04F7;
							}
							if (!(brand == "Infinity"))
							{
								goto IL_04F7;
							}
						}
						else if (!(brand == "Infiniti"))
						{
							goto IL_04F7;
						}
					}
					else
					{
						if (!(brand == "Dongfeng"))
						{
							goto IL_04F7;
						}
						goto IL_04E1;
					}
					break;
				}
				case 10:
				{
					char c = brand[0];
					if (c != 'G')
					{
						if (c != 'M')
						{
							if (c != 'V')
							{
								goto IL_04F7;
							}
							if (!(brand == "Volkswagen"))
							{
								goto IL_04F7;
							}
							goto IL_0478;
						}
						else
						{
							if (!(brand == "Mitsubishi"))
							{
								goto IL_04F7;
							}
							return (num + 1).ToString("X3");
						}
					}
					else
					{
						if (!(brand == "Great Wall"))
						{
							goto IL_04F7;
						}
						goto IL_04CE;
					}
					break;
				}
				default:
					goto IL_04F7;
				}
				if (1952 <= num && num <= 1959)
				{
					return (num + 8).ToString("X3");
				}
				return (num + 32).ToString("X3");
				IL_0478:
				return (num + 106).ToString("X3");
				IL_048B:
				return (num - 32).ToString("X3");
				IL_04CE:
				return (num + 64).ToString("X3");
				IL_04E1:
				return (num + 128).ToString("X3");
			}
			IL_04F7:
			return (num + 8).ToString("X3");
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x001AF44C File Offset: 0x001AD64C
		public static string GetECUName(string responseHeader, string brand, string extended_address)
		{
			try
			{
				if (responseHeader == "7E8")
				{
					return Translate.GetString("ecu_Engine");
				}
				if (responseHeader == "7E9")
				{
					return Translate.GetString("ecu_Transmission");
				}
				if (brand != null)
				{
					char c;
					int num;
					switch (brand.Length)
					{
					case 2:
						if (!(brand == "DS"))
						{
							goto IL_1BD4;
						}
						goto IL_1B48;
					case 3:
						c = brand[0];
						if (c <= 'K')
						{
							if (c != 'B')
							{
								if (c != 'K')
								{
									goto IL_1BD4;
								}
								if (!(brand == "Kia"))
								{
									goto IL_1BD4;
								}
								goto IL_0B2E;
							}
							else
							{
								if (!(brand == "BMW"))
								{
									goto IL_1BD4;
								}
								goto IL_12F5;
							}
						}
						else if (c != 'V')
						{
							if (c != 'В')
							{
								goto IL_1BD4;
							}
							if (!(brand == "ВАЗ"))
							{
								goto IL_1BD4;
							}
							goto IL_18E4;
						}
						else
						{
							if (!(brand == "VAZ"))
							{
								goto IL_1BD4;
							}
							goto IL_18E4;
						}
						break;
					case 4:
						c = brand[0];
						if (c <= 'L')
						{
							if (c != 'A')
							{
								if (c != 'F')
								{
									if (c != 'L')
									{
										goto IL_1BD4;
									}
									if (!(brand == "Lada"))
									{
										goto IL_1BD4;
									}
									goto IL_18E4;
								}
								else
								{
									if (!(brand == "Ford"))
									{
										goto IL_1BD4;
									}
									goto IL_170F;
								}
							}
							else if (!(brand == "Audi"))
							{
								goto IL_1BD4;
							}
						}
						else if (c != 'M')
						{
							if (c != 'S')
							{
								if (c != 'Л')
								{
									goto IL_1BD4;
								}
								if (!(brand == "Лада"))
								{
									goto IL_1BD4;
								}
								goto IL_18E4;
							}
							else if (!(brand == "Seat"))
							{
								goto IL_1BD4;
							}
						}
						else
						{
							if (!(brand == "Mini"))
							{
								goto IL_1BD4;
							}
							goto IL_12F5;
						}
						break;
					case 5:
						c = brand[0];
						if (c <= 'L')
						{
							if (c != 'C')
							{
								if (c != 'D')
								{
									if (c != 'L')
									{
										goto IL_1BD4;
									}
									if (!(brand == "Lexus"))
									{
										goto IL_1BD4;
									}
									goto IL_0D73;
								}
								else
								{
									if (!(brand == "Dacia"))
									{
										goto IL_1BD4;
									}
									goto IL_1931;
								}
							}
							else if (!(brand == "Cupra"))
							{
								goto IL_1BD4;
							}
						}
						else if (c != 'M')
						{
							if (c != 'S')
							{
								if (c != 'V')
								{
									goto IL_1BD4;
								}
								if (!(brand == "Volvo"))
								{
									goto IL_1BD4;
								}
								if (responseHeader == null)
								{
									goto IL_1BD4;
								}
								num = responseHeader.Length;
								if (num != 3)
								{
									goto IL_1BD4;
								}
								c = responseHeader[1];
								switch (c)
								{
								case '0':
									if (!(responseHeader == "70F"))
									{
										goto IL_1BD4;
									}
									return "TVM";
								case '1':
								case '7':
								case ':':
								case ';':
								case '<':
								case '=':
								case '>':
								case '?':
								case '@':
								case 'B':
									goto IL_1BD4;
								case '2':
									if (responseHeader == "72E")
									{
										return "CEM";
									}
									if (responseHeader == "72F")
									{
										return "IAM";
									}
									if (!(responseHeader == "728"))
									{
										goto IL_1BD4;
									}
									return "DIM";
								case '3':
									if (responseHeader == "739")
									{
										return "KVM";
									}
									if (responseHeader == "73B")
									{
										return "CCM";
									}
									if (responseHeader == "73E")
									{
										return "PAM";
									}
									if (responseHeader == "738")
									{
										return "PSCM";
									}
									if (!(responseHeader == "73F"))
									{
										goto IL_1BD4;
									}
									return "SRS/Airbag";
								case '4':
									if (responseHeader == "748")
									{
										return "DDM";
									}
									if (responseHeader == "749")
									{
										return "PDM";
									}
									if (!(responseHeader == "74C"))
									{
										goto IL_1BD4;
									}
									return "PSM";
								case '5':
									if (responseHeader == "75C")
									{
										return "PHM";
									}
									if (!(responseHeader == "75E"))
									{
										goto IL_1BD4;
									}
									return "PBM";
								case '6':
									if (!(responseHeader == "768"))
									{
										goto IL_1BD4;
									}
									return "BCM";
								case '8':
									if (!(responseHeader == "78C"))
									{
										goto IL_1BD4;
									}
									return "ICM";
								case '9':
									if (responseHeader == "79B")
									{
										return "CVM";
									}
									if (responseHeader == "799")
									{
										return "TRM";
									}
									if (responseHeader == "79C")
									{
										return "PPM";
									}
									if (!(responseHeader == "79F"))
									{
										goto IL_1BD4;
									}
									return "SAS";
								case 'A':
									if (!(responseHeader == "7AC"))
									{
										goto IL_1BD4;
									}
									return "AUD";
								case 'C':
									if (responseHeader == "7CC")
									{
										return "SODL";
									}
									if (responseHeader == "7CE")
									{
										return "SODR";
									}
									if (!(responseHeader == "7C9"))
									{
										goto IL_1BD4;
									}
									return "PAC";
								case 'D':
									if (!(responseHeader == "7DE"))
									{
										goto IL_1BD4;
									}
									return "DABM";
								default:
									if (c != 'S')
									{
										goto IL_1BD4;
									}
									if (!(responseHeader == "FSM"))
									{
										goto IL_1BD4;
									}
									return "76C";
								}
							}
							else if (!(brand == "Skoda"))
							{
								goto IL_1BD4;
							}
						}
						else
						{
							if (!(brand == "Mazda"))
							{
								goto IL_1BD4;
							}
							goto IL_170F;
						}
						break;
					case 6:
						c = brand[0];
						if (c != 'N')
						{
							if (c != 'S')
							{
								if (c != 'T')
								{
									goto IL_1BD4;
								}
								if (!(brand == "Toyota"))
								{
									goto IL_1BD4;
								}
								goto IL_0D73;
							}
							else
							{
								if (!(brand == "Subaru"))
								{
									goto IL_1BD4;
								}
								if (responseHeader == null)
								{
									goto IL_1BD4;
								}
								num = responseHeader.Length;
								if (num != 3)
								{
									goto IL_1BD4;
								}
								c = responseHeader[1];
								if (c != '5')
								{
									if (c != '8')
									{
										switch (c)
										{
										case 'B':
											if (!(responseHeader == "7B8"))
											{
												goto IL_1BD4;
											}
											return "ABS";
										case 'C':
											if (!(responseHeader == "7CC"))
											{
												goto IL_1BD4;
											}
											return "A/C, Heater";
										case 'D':
											if (!(responseHeader == "7DD"))
											{
												goto IL_1BD4;
											}
											return "Display";
										default:
											goto IL_1BD4;
										}
									}
									else
									{
										if (responseHeader == "788")
										{
											return "SRS";
										}
										if (responseHeader == "78B")
										{
											return "Dashboard";
										}
										if (!(responseHeader == "78A"))
										{
											goto IL_1BD4;
										}
										return "Start-Stop";
									}
								}
								else
								{
									if (!(responseHeader == "75A"))
									{
										goto IL_1BD4;
									}
									return "BCM";
								}
							}
						}
						else
						{
							if (!(brand == "Nissan"))
							{
								goto IL_1BD4;
							}
							goto IL_1931;
						}
						break;
					case 7:
						c = brand[3];
						if (c <= 'e')
						{
							if (c != 'a')
							{
								if (c != 'e')
								{
									goto IL_1BD4;
								}
								if (!(brand == "Genesis"))
								{
									goto IL_1BD4;
								}
								goto IL_0B2E;
							}
							else
							{
								if (!(brand == "Renault"))
								{
									goto IL_1BD4;
								}
								goto IL_1931;
							}
						}
						else if (c != 'g')
						{
							switch (c)
							{
							case 'n':
								if (!(brand == "Hyundai"))
								{
									goto IL_1BD4;
								}
								goto IL_0B2E;
							case 'o':
							case 'p':
							case 'q':
								goto IL_1BD4;
							case 'r':
								if (!(brand == "Citroen"))
								{
									goto IL_1BD4;
								}
								goto IL_1B48;
							case 's':
								if (!(brand == "Porsche"))
								{
									goto IL_1BD4;
								}
								break;
							case 't':
								if (!(brand == "Bentley"))
								{
									goto IL_1BD4;
								}
								break;
							default:
								goto IL_1BD4;
							}
						}
						else
						{
							if (!(brand == "Peugeot"))
							{
								goto IL_1BD4;
							}
							goto IL_1B48;
						}
						break;
					case 8:
						c = brand[7];
						if (c != 'i')
						{
							if (c != 'y')
							{
								goto IL_1BD4;
							}
							if (!(brand == "Infinity"))
							{
								goto IL_1BD4;
							}
							goto IL_1931;
						}
						else
						{
							if (!(brand == "Infiniti"))
							{
								goto IL_1BD4;
							}
							goto IL_1931;
						}
						break;
					case 9:
						goto IL_1BD4;
					case 10:
						c = brand[0];
						if (c != 'L')
						{
							if (c != 'M')
							{
								if (c != 'V')
								{
									goto IL_1BD4;
								}
								if (!(brand == "Volkswagen"))
								{
									goto IL_1BD4;
								}
							}
							else
							{
								if (!(brand == "Mitsubishi"))
								{
									goto IL_1BD4;
								}
								if (responseHeader == "785")
								{
									return "ABS";
								}
								if (responseHeader == "7B7")
								{
									return "AWC 4WD";
								}
								if (responseHeader == "689")
								{
									return "A/C";
								}
								if (responseHeader == "6A1")
								{
									return "Dashboard";
								}
								if (!(responseHeader == "774"))
								{
									goto IL_1BD4;
								}
								return "TPMS";
							}
						}
						else
						{
							if (!(brand == "Land Rover"))
							{
								goto IL_1BD4;
							}
							goto IL_1B7E;
						}
						break;
					case 11:
						if (!(brand == "Range Rover"))
						{
							goto IL_1BD4;
						}
						goto IL_1B7E;
					default:
						goto IL_1BD4;
					}
					if (responseHeader == null)
					{
						goto IL_1BD4;
					}
					num = responseHeader.Length;
					if (num != 3)
					{
						goto IL_1BD4;
					}
					switch (responseHeader[2])
					{
					case '0':
						if (responseHeader == "7B0")
						{
							return "Climate";
						}
						if (responseHeader == "780")
						{
							return "Active steering";
						}
						if (!(responseHeader == "850"))
						{
							goto IL_1BD4;
						}
						return "Actuator";
					case '1':
						if (responseHeader == "7C1")
						{
							return "Adaptive cruise control";
						}
						if (responseHeader == "7B1")
						{
							return "Trailer";
						}
						if (!(responseHeader == "7D1"))
						{
							goto IL_1BD4;
						}
						return "Emergency unit";
					case '2':
						if (responseHeader == "792")
						{
							return "Charge control";
						}
						if (!(responseHeader == "722"))
						{
							goto IL_1BD4;
						}
						return "Rear camera";
					case '3':
						if (!(responseHeader == "7D3"))
						{
							goto IL_1BD4;
						}
						return "Rear camera";
					case '4':
						if (responseHeader == "774")
						{
							return "Park assist";
						}
						if (responseHeader == "7D4")
						{
							return "Second heater";
						}
						if (responseHeader == "784")
						{
							return "Rear climate control";
						}
						if (responseHeader == "7B4")
						{
							return "Driver door";
						}
						if (!(responseHeader == "7C4"))
						{
							goto IL_1BD4;
						}
						return "Infotaiment system";
					case '5':
						if (responseHeader == "7A5")
						{
							return "Brake assist";
						}
						if (responseHeader == "7B5")
						{
							return "Front passenger door";
						}
						if (responseHeader == "775")
						{
							return "TPMS";
						}
						if (!(responseHeader == "7D5"))
						{
							goto IL_1BD4;
						}
						return "Phone";
					case '6':
						if (responseHeader == "776")
						{
							return "Steering";
						}
						if (responseHeader == "7B6")
						{
							return "Driver seat";
						}
						if (responseHeader == "7D6")
						{
							return "Navi";
						}
						if (responseHeader == "796")
						{
							return "Special function unit";
						}
						if (!(responseHeader == "726"))
						{
							goto IL_1BD4;
						}
						return "SRS/Airbag";
					case '7':
						if (responseHeader == "7B7")
						{
							return "Front passenger seat";
						}
						if (responseHeader == "797")
						{
							return "Roof";
						}
						if (responseHeader == "7D7")
						{
							return "TV";
						}
						if (!(responseHeader == "787"))
						{
							goto IL_1BD4;
						}
						return "Charge unit";
					case '8':
						if (responseHeader == "778")
						{
							return "BCM";
						}
						if (responseHeader == "788")
						{
							return "Differential lock";
						}
						if (!(responseHeader == "7B8"))
						{
							goto IL_1BD4;
						}
						return "Lane change assist";
					case '9':
						if (responseHeader == "779")
						{
							return "4WD";
						}
						if (responseHeader == "7D9")
						{
							return "Acoustic system";
						}
						if (!(responseHeader == "7B9"))
						{
							goto IL_1BD4;
						}
						return "Front assistance";
					case ':':
					case ';':
					case '<':
					case '=':
					case '>':
					case '?':
					case '@':
						goto IL_1BD4;
					case 'A':
						if (responseHeader == "7DA")
						{
							return "Media 1";
						}
						if (!(responseHeader == "77A"))
						{
							goto IL_1BD4;
						}
						return "CAN gateway";
					case 'B':
						if (!(responseHeader == "77B"))
						{
							goto IL_1BD4;
						}
						return "Immobilizer";
					case 'C':
						if (responseHeader == "77C")
						{
							return "EPS";
						}
						if (responseHeader == "79C")
						{
							return "Start/Access Authorization";
						}
						if (responseHeader == "84C")
						{
							return Translate.GetString("ecu_Engine");
						}
						if (responseHeader == "7BC")
						{
							return "Parking brake";
						}
						if (!(responseHeader == "7CC"))
						{
							goto IL_1BD4;
						}
						return "Brake system sensors";
					case 'D':
						if (responseHeader == "77D")
						{
							return Translate.GetString("ecu_Abs");
						}
						if (responseHeader == "7DD")
						{
							return "Infotaiment system";
						}
						if (responseHeader == "78D")
						{
							return "Trunk electric";
						}
						if (!(responseHeader == "7ED"))
						{
							goto IL_1BD4;
						}
						return "Hybrid battery management";
					case 'E':
						if (responseHeader == "77E")
						{
							return "Dashboard";
						}
						if (responseHeader == "7BE")
						{
							return "Headlight corrector";
						}
						if (responseHeader == "7EE")
						{
							return "Electric drive";
						}
						if (!(responseHeader == "7AE"))
						{
							goto IL_1BD4;
						}
						return "High-voltage battery charger";
					case 'F':
						if (responseHeader == "77F")
						{
							return "SRS/Airbag";
						}
						if (responseHeader == "7BF")
						{
							return "Self-levelling control unit";
						}
						if (responseHeader == "7AF")
						{
							return "Central comfort unit";
						}
						if (!(responseHeader == "7CF"))
						{
							goto IL_1BD4;
						}
						return "High-voltage battery charging management";
					default:
						goto IL_1BD4;
					}
					IL_0B2E:
					if (responseHeader == null)
					{
						goto IL_1BD4;
					}
					num = responseHeader.Length;
					if (num == 3)
					{
						switch (responseHeader[2])
						{
						case '8':
							if (!(responseHeader == "748"))
							{
								if (!(responseHeader == "7A8"))
								{
									goto IL_1BD4;
								}
								goto IL_0C9E;
							}
							break;
						case '9':
							if (!(responseHeader == "7D9"))
							{
								goto IL_1BD4;
							}
							return "ABS";
						case ':':
						case ';':
						case '<':
						case '=':
						case '>':
						case '?':
						case '@':
							goto IL_1BD4;
						case 'A':
							if (responseHeader == "7EA")
							{
								return "VMCU";
							}
							if (!(responseHeader == "7DA"))
							{
								goto IL_1BD4;
							}
							return "SRS/Airbag";
						case 'B':
							if (!(responseHeader == "7BB"))
							{
								goto IL_1BD4;
							}
							return "EV HVAC";
						case 'C':
							if (responseHeader == "7DC")
							{
								return "EPS";
							}
							if (!(responseHeader == "7EC"))
							{
								goto IL_1BD4;
							}
							return "EV BMS";
						case 'D':
							if (!(responseHeader == "7ED"))
							{
								if (!(responseHeader == "7CD"))
								{
									goto IL_1BD4;
								}
								return "LDC";
							}
							break;
						case 'E':
							if (responseHeader == "7DE")
							{
								goto IL_0C9E;
							}
							if (!(responseHeader == "7CE"))
							{
								goto IL_1BD4;
							}
							return "Dashboard";
						case 'F':
							if (!(responseHeader == "7CF"))
							{
								goto IL_1BD4;
							}
							return "ECALL";
						default:
							goto IL_1BD4;
						}
						return "4WD";
						IL_0C9E:
						return "TPMS";
					}
					goto IL_1BD4;
					IL_0D73:
					if (responseHeader == null)
					{
						goto IL_1BD4;
					}
					num = responseHeader.Length;
					if (num == 3)
					{
						c = responseHeader[2];
						if (c != '8')
						{
							if (c != '9')
							{
								switch (c)
								{
								case 'B':
									if (!(responseHeader == "70B"))
									{
										goto IL_1BD4;
									}
									break;
								case 'C':
									if (!(responseHeader == "7CC"))
									{
										goto IL_1BD4;
									}
									return "Climate";
								case 'D':
									if (responseHeader == "70D")
									{
										return "Rear motor generator";
									}
									if (!(responseHeader == "74D"))
									{
										goto IL_1BD4;
									}
									return "Plug-in Control";
								case 'E':
									if (!(responseHeader == "72E"))
									{
										goto IL_1BD4;
									}
									return "Start/stop";
								case 'F':
									if (!(responseHeader == "74F"))
									{
										goto IL_1BD4;
									}
									return "HV Battery";
								default:
									goto IL_1BD4;
								}
							}
							else if (!(responseHeader == "789"))
							{
								if (responseHeader == "799")
								{
									goto IL_0F08;
								}
								if (responseHeader == "7A9")
								{
									return "EMPS";
								}
								if (!(responseHeader == "7B9"))
								{
									goto IL_1BD4;
								}
								goto IL_0F29;
							}
							return "Solar Charging Control";
						}
						if (!(responseHeader == "798"))
						{
							if (responseHeader == "7B8")
							{
								return "ABS";
							}
							if (responseHeader == "7C8")
							{
								goto IL_0F29;
							}
							if (responseHeader == "788")
							{
								return "SRS/Airbag";
							}
							if (!(responseHeader == "758"))
							{
								goto IL_1BD4;
							}
							if (!string.IsNullOrEmpty(extended_address) && extended_address == "40")
							{
								return "BCM";
							}
							return "Gateway";
						}
						IL_0F08:
						return "Radar cruise control";
						IL_0F29:
						return "Dashboard";
					}
					goto IL_1BD4;
					IL_12F5:
					string text = responseHeader.Substring(1, 2).ToUpperInvariant();
					if (text == null)
					{
						goto IL_1BD4;
					}
					num = text.Length;
					if (num != 2)
					{
						goto IL_1BD4;
					}
					c = text[1];
					switch (c)
					{
					case '0':
						if (text == "00")
						{
							return "IPDM";
						}
						if (text == "20")
						{
							return "TPMS";
						}
						if (text == "30")
						{
							return "Steering";
						}
						if (text == "40")
						{
							return "Entry&Start";
						}
						if (!(text == "60"))
						{
							goto IL_1BD4;
						}
						return "Dashboard";
					case '1':
						if (text == "01")
						{
							return "SRS/Airbag";
						}
						if (text == "21")
						{
							return "Adaptive cruise control";
						}
						if (!(text == "31"))
						{
							goto IL_1BD4;
						}
						return "DVD changer";
					case '2':
						if (text == "12")
						{
							return Translate.GetString("ecu_Engine");
						}
						if (!(text == "62"))
						{
							goto IL_1BD4;
						}
						return "Central gateway";
					case '3':
					case '5':
					case '7':
						goto IL_1BD4;
					case '4':
						if (!(text == "64"))
						{
							goto IL_1BD4;
						}
						return "Parktronik";
					case '6':
						if (!(text == "16"))
						{
							goto IL_1BD4;
						}
						return "Active steering";
					case '8':
						if (text == "18")
						{
							return Translate.GetString("ecu_Transmission");
						}
						if (text == "38")
						{
							return "Air suspension";
						}
						if (!(text == "78"))
						{
							goto IL_1BD4;
						}
						return "Climate";
					case '9':
						if (text == "19")
						{
							return "4WD";
						}
						if (text == "29")
						{
							return "DSC";
						}
						if (!(text == "79"))
						{
							goto IL_1BD4;
						}
						return "Rear heater";
					default:
						switch (c)
						{
						case 'A':
							if (!(text == "2A"))
							{
								goto IL_1BD4;
							}
							return "Parking barke";
						case 'B':
							if (!(text == "0B"))
							{
								goto IL_1BD4;
							}
							return Translate.GetString("ecu_Engine");
						case 'C':
							if (!(text == "1C"))
							{
								goto IL_1BD4;
							}
							return "Suspension";
						case 'D':
						case 'E':
							goto IL_1BD4;
						case 'F':
							if (!(text == "0F"))
							{
								goto IL_1BD4;
							}
							return "Differential lock";
						default:
							goto IL_1BD4;
						}
						break;
					}
					IL_170F:
					if (responseHeader == null)
					{
						goto IL_1BD4;
					}
					num = responseHeader.Length;
					if (num != 3)
					{
						goto IL_1BD4;
					}
					c = responseHeader[1];
					switch (c)
					{
					case '2':
						if (responseHeader == "728")
						{
							return "Dashboard";
						}
						if (!(responseHeader == "72E"))
						{
							goto IL_1BD4;
						}
						return "BCM";
					case '3':
						if (responseHeader == "738")
						{
							return "Steering";
						}
						if (responseHeader == "739")
						{
							return "Access system";
						}
						if (responseHeader == "73C")
						{
							return "AFS/ALM";
						}
						if (!(responseHeader == "73F"))
						{
							goto IL_1BD4;
						}
						return "SRS/Airbag";
					case '4':
						if (!(responseHeader == "74C"))
						{
							goto IL_1BD4;
						}
						return "Driver seat";
					case '5':
						if (responseHeader == "75C")
						{
							return "DCM";
						}
						if (!(responseHeader == "75E"))
						{
							goto IL_1BD4;
						}
						return "Parking brake";
					case '6':
						if (!(responseHeader == "768"))
						{
							goto IL_1BD4;
						}
						return "ABS/ESP";
					case '7':
						if (!(responseHeader == "77D"))
						{
							goto IL_1BD4;
						}
						return "Rear door";
					case '8':
						if (!(responseHeader == "78C"))
						{
							goto IL_1BD4;
						}
						return "Multimedia";
					default:
						if (c != 'A')
						{
							goto IL_1BD4;
						}
						if (!(responseHeader == "7AC"))
						{
							goto IL_1BD4;
						}
						return "Amplifier";
					}
					IL_18E4:
					if (responseHeader == "7EB")
					{
						return "ABS";
					}
					if (responseHeader == "7E7")
					{
						return "BCM";
					}
					if (!(responseHeader == "7E5"))
					{
						goto IL_1BD4;
					}
					return "SRS/Airbag";
					IL_1931:
					if (responseHeader == null)
					{
						goto IL_1BD4;
					}
					num = responseHeader.Length;
					if (num != 3)
					{
						goto IL_1BD4;
					}
					c = responseHeader[2];
					switch (c)
					{
					case '0':
						if (!(responseHeader == "760"))
						{
							goto IL_1BD4;
						}
						return "ABS";
					case '1':
					case '6':
					case '7':
						goto IL_1BD4;
					case '2':
						if (responseHeader == "772")
						{
							return "SRS";
						}
						if (!(responseHeader == "762"))
						{
							goto IL_1BD4;
						}
						return "EPS";
					case '3':
						if (!(responseHeader == "763"))
						{
							goto IL_1BD4;
						}
						return "Dashboard";
					case '4':
						if (!(responseHeader == "764"))
						{
							goto IL_1BD4;
						}
						return "A/C, heater, climate";
					case '5':
						if (!(responseHeader == "765"))
						{
							goto IL_1BD4;
						}
						return "BCM";
					case '8':
						if (!(responseHeader == "768"))
						{
							goto IL_1BD4;
						}
						return "AWD";
					case '9':
						if (!(responseHeader == "779"))
						{
							goto IL_1BD4;
						}
						return "Side door";
					default:
						switch (c)
						{
						case 'C':
							if (responseHeader == "7AC")
							{
								return "Differential lock";
							}
							if (responseHeader == "76C")
							{
								return "Intelligent key";
							}
							if (!(responseHeader == "72C"))
							{
								goto IL_1BD4;
							}
							return "Chassis";
						case 'D':
							if (responseHeader == "7ED")
							{
								return "Hybrid battery";
							}
							if (!(responseHeader == "76D"))
							{
								goto IL_1BD4;
							}
							return "IPDM";
						case 'E':
							goto IL_1BD4;
						case 'F':
							if (!(responseHeader == "75F"))
							{
								goto IL_1BD4;
							}
							return "e-4WD";
						default:
							goto IL_1BD4;
						}
						break;
					}
					IL_1B48:
					if (responseHeader == "688")
					{
						return Translate.GetString("ecu_Engine");
					}
					if (!(responseHeader == "689"))
					{
						goto IL_1BD4;
					}
					return Translate.GetString("ecu_Transmission");
					IL_1B7E:
					if (responseHeader == "79D")
					{
						return "Rear differential";
					}
					if (responseHeader == "769")
					{
						return "Transfer case";
					}
					if (responseHeader == "768")
					{
						return "ABS";
					}
					if (responseHeader == "79A")
					{
						return "Terrain response";
					}
				}
				IL_1BD4:;
			}
			catch (Exception)
			{
			}
			return responseHeader;
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x001B1054 File Offset: 0x001AF254
		public static void Request_ResponseReceivedCheckForNegativeResponseAndForCancellation(OBDRequest request, string data)
		{
			if (data == null)
			{
				return;
			}
			string text = OBDDataReader.FilterHexAndNewLineOnly(data);
			string service = request.Command.Substring(0, 2);
			string text2 = "037F" + service + "11";
			string nr78_string = "037F" + service + "78";
			if (text.Contains(text2))
			{
				App.OBDReader.GetQueueCopy().RemoveAll((OBDRequest x) => x.Header == request.Header && x.Command.StartsWith(service));
				return;
			}
			if (text.IndexOf(nr78_string) >= 0)
			{
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
				{
					string[] array = new string[request.BeforeCommands.Length + 2];
					Array.Copy(request.BeforeCommands, array, request.BeforeCommands.Length);
					array[array.Length - 2] = "ATAT0";
					array[array.Length - 1] = "ATSTFF";
					string[] array2 = new string[request.AfterCommands.Length + 2];
					Array.Copy(request.AfterCommands, array2, request.AfterCommands.Length);
					array2[array2.Length - 2] = "ATAT" + SharedSettings.Current.AdaptiveTimings.ToString();
					string text3 = SharedSettings.Current.GetATST();
					if (string.IsNullOrEmpty(text3))
					{
						text3 = "32";
					}
					array2[array2.Length - 1] = "ATST" + text3;
					request.BeforeCommands = array;
					request.AfterCommands = array2;
					request.ResponseReceived -= CAN11bitHelper.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
					request.ResponseReceived += delegate(OBDRequest request2, string response)
					{
						if (response == null || request2.NR78RepeatCounter > 3)
						{
							return;
						}
						bool flag = false;
						bool flag2 = false;
						if (response != null && response.Contains("NO DATA"))
						{
							flag = true;
						}
						if (!flag)
						{
							string text4 = OBDDataReader.FilterHexAndNewLineOnly(response);
							string[] array3 = text4.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
							if (text4.Contains(nr78_string) && array3.Length == 1)
							{
								flag2 = true;
							}
						}
						if (flag || flag2)
						{
							int nr78RepeatCounter = request2.NR78RepeatCounter;
							request2.NR78RepeatCounter = nr78RepeatCounter + 1;
							App.OBDReader.InsertRequestInQueue(request2);
						}
					};
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.Insert(0, request);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x001B1268 File Offset: 0x001AF468
		internal static string GetNegativeResponseCode(OBDRequest request, string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return "";
			}
			if (data.Contains("NO DATA"))
			{
				return "";
			}
			string text = OBDDataReader.FilterHexAndNewLineOnly(data);
			string text2 = request.Command.Substring(0, 2);
			string text3 = "037F" + text2 + "11";
			string text4 = "037F" + text2;
			if (text.Contains(text3))
			{
				return "11";
			}
			if (text.Contains(text4))
			{
				string[] array = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					if (array[i].IndexOf(request.ResponseMarker, 3) >= 0)
					{
						return "0";
					}
					int num = array[i].IndexOf(text4);
					if (num >= 0)
					{
						return array[i].Substring(num + text4.Length, 2);
					}
				}
			}
			return "0";
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x001B1354 File Offset: 0x001AF554
		public static bool IsNoDataOrNegativeResponse(OBDRequest request, string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return true;
			}
			if (data.Contains("NO DATA"))
			{
				return true;
			}
			string negativeResponseCode = CAN11bitHelper.GetNegativeResponseCode(request, data);
			return !(negativeResponseCode == "0") && !(negativeResponseCode == "00");
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x001B13A0 File Offset: 0x001AF5A0
		public static void UpdateCommandsForSTN(OBDRequest req)
		{
			List<string> list = req.BeforeCommands.ToList<string>();
			string header = req.Header;
			string text = list.FirstOrDefault((string x) => x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase));
			string text2 = list.FirstOrDefault((string x) => x.StartsWith("ATCEA", StringComparison.OrdinalIgnoreCase) && x != "ATCEA");
			string text3;
			if (text != null)
			{
				text3 = text.Substring(5);
			}
			else
			{
				text3 = CAN11bitHelper.GetPossibleResponseHeader(header, SharedSettings.Current.SelectedBrand, req);
			}
			string text4;
			if (text2 != null)
			{
				text4 = text2.Substring(5);
			}
			else
			{
				text4 = "";
			}
			string text5 = list.FirstOrDefault((string x) => x.StartsWith("ATTA", StringComparison.OrdinalIgnoreCase));
			string text6;
			if (text5 != null)
			{
				text6 = text5.Substring(4);
			}
			else
			{
				text6 = "";
			}
			string text7 = list.FirstOrDefault((string x) => x.StartsWith("ATFCSH", StringComparison.OrdinalIgnoreCase));
			string text8 = "";
			if (text7 != null)
			{
				text8 = text7.Substring(6);
			}
			if (!string.IsNullOrEmpty(text8))
			{
				string text9 = string.Concat(new string[] { "STCFCPA", text8, text4, ",", text3, text6 });
				list.Add(text9);
			}
			if (string.IsNullOrEmpty(text2))
			{
				string text10 = "STCAF1," + text4;
				list.Add(text10);
				list.Remove(text2);
			}
			List<string> list2 = req.AfterCommands.ToList<string>();
			if (list2.Any((string x) => x == "ATFCSM0"))
			{
				list2.Add("STCFCPC");
			}
			if (list2.Any((string x) => x == "ATCEA"))
			{
				list2.Add("STCAF0");
			}
			req.BeforeCommands = list.ToArray();
			req.AfterCommands = list2.ToArray();
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x001B15C8 File Offset: 0x001AF7C8
		internal static void SetKeysForSTN(OBDRequest req)
		{
			string[] beforeCommands = req.BeforeCommands;
			if (beforeCommands != null && beforeCommands.Length != 0)
			{
				string header = req.Header;
				string text = beforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase));
				string text2 = beforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCEA", StringComparison.OrdinalIgnoreCase) && x != "ATCEA");
				string text3;
				if (text != null)
				{
					text3 = text.Substring(5);
				}
				else
				{
					text3 = CAN11bitHelper.GetPossibleResponseHeader(header, SharedSettings.Current.SelectedBrand, req);
				}
				string text4;
				if (text2 != null)
				{
					text4 = text2.Substring(5);
				}
				else
				{
					text4 = "";
				}
				string text5 = beforeCommands.FirstOrDefault((string x) => x.StartsWith("ATTA", StringComparison.OrdinalIgnoreCase));
				string text6;
				if (text5 != null)
				{
					text6 = text5.Substring(4);
				}
				else
				{
					text6 = "";
				}
				string text7 = beforeCommands.FirstOrDefault((string x) => x.StartsWith("ATFCSH", StringComparison.OrdinalIgnoreCase));
				if (text7 != null)
				{
					string text8 = text7.Substring(6);
					req.Keys["FC_HEADER"] = text8;
					req.Keys["RCV_HEADER"] = text3;
					req.Keys["EXT_ADDR"] = text4;
					req.Keys["EXT_TA"] = text6;
					req.Keys["ST_FC_REQ"] = text8 + text4;
					req.Keys["ST_FC_RES"] = text3 + text6;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					string text9 = "STCAF1," + text4;
					req.Keys[text2] = text9;
				}
			}
		}

		// Token: 0x020002EC RID: 748
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002358 RID: 9048 RVA: 0x001B179F File Offset: 0x001AF99F
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002359 RID: 9049 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600235A RID: 9050 RVA: 0x001B17AB File Offset: 0x001AF9AB
			internal bool <GetPossibleResponseHeader>b__0_0(string x)
			{
				return x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600235B RID: 9051 RVA: 0x001B17AB File Offset: 0x001AF9AB
			internal bool <UpdateCommandsForSTN>b__5_0(string x)
			{
				return x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600235C RID: 9052 RVA: 0x001B17B9 File Offset: 0x001AF9B9
			internal bool <UpdateCommandsForSTN>b__5_1(string x)
			{
				return x.StartsWith("ATCEA", StringComparison.OrdinalIgnoreCase) && x != "ATCEA";
			}

			// Token: 0x0600235D RID: 9053 RVA: 0x001B17D6 File Offset: 0x001AF9D6
			internal bool <UpdateCommandsForSTN>b__5_2(string x)
			{
				return x.StartsWith("ATTA", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600235E RID: 9054 RVA: 0x001B17E4 File Offset: 0x001AF9E4
			internal bool <UpdateCommandsForSTN>b__5_3(string x)
			{
				return x.StartsWith("ATFCSH", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600235F RID: 9055 RVA: 0x001B17F2 File Offset: 0x001AF9F2
			internal bool <UpdateCommandsForSTN>b__5_4(string x)
			{
				return x == "ATFCSM0";
			}

			// Token: 0x06002360 RID: 9056 RVA: 0x001B17FF File Offset: 0x001AF9FF
			internal bool <UpdateCommandsForSTN>b__5_5(string x)
			{
				return x == "ATCEA";
			}

			// Token: 0x06002361 RID: 9057 RVA: 0x001B17AB File Offset: 0x001AF9AB
			internal bool <SetKeysForSTN>b__6_0(string x)
			{
				return x.StartsWith("ATCRA", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06002362 RID: 9058 RVA: 0x001B17B9 File Offset: 0x001AF9B9
			internal bool <SetKeysForSTN>b__6_1(string x)
			{
				return x.StartsWith("ATCEA", StringComparison.OrdinalIgnoreCase) && x != "ATCEA";
			}

			// Token: 0x06002363 RID: 9059 RVA: 0x001B17D6 File Offset: 0x001AF9D6
			internal bool <SetKeysForSTN>b__6_2(string x)
			{
				return x.StartsWith("ATTA", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06002364 RID: 9060 RVA: 0x001B17E4 File Offset: 0x001AF9E4
			internal bool <SetKeysForSTN>b__6_3(string x)
			{
				return x.StartsWith("ATFCSH", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x04001124 RID: 4388
			public static readonly CAN11bitHelper.<>c <>9 = new CAN11bitHelper.<>c();

			// Token: 0x04001125 RID: 4389
			public static Func<string, bool> <>9__0_0;

			// Token: 0x04001126 RID: 4390
			public static Func<string, bool> <>9__5_0;

			// Token: 0x04001127 RID: 4391
			public static Func<string, bool> <>9__5_1;

			// Token: 0x04001128 RID: 4392
			public static Func<string, bool> <>9__5_2;

			// Token: 0x04001129 RID: 4393
			public static Func<string, bool> <>9__5_3;

			// Token: 0x0400112A RID: 4394
			public static Func<string, bool> <>9__5_4;

			// Token: 0x0400112B RID: 4395
			public static Func<string, bool> <>9__5_5;

			// Token: 0x0400112C RID: 4396
			public static Func<string, bool> <>9__6_0;

			// Token: 0x0400112D RID: 4397
			public static Func<string, bool> <>9__6_1;

			// Token: 0x0400112E RID: 4398
			public static Func<string, bool> <>9__6_2;

			// Token: 0x0400112F RID: 4399
			public static Func<string, bool> <>9__6_3;
		}

		// Token: 0x020002ED RID: 749
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x06002365 RID: 9061 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06002366 RID: 9062 RVA: 0x001B180C File Offset: 0x001AFA0C
			internal bool <Request_ResponseReceivedCheckForNegativeResponseAndForCancellation>b__0(OBDRequest x)
			{
				return x.Header == this.request.Header && x.Command.StartsWith(this.service);
			}

			// Token: 0x06002367 RID: 9063 RVA: 0x001B183C File Offset: 0x001AFA3C
			internal void <Request_ResponseReceivedCheckForNegativeResponseAndForCancellation>b__1(OBDRequest request2, string response)
			{
				if (response == null || request2.NR78RepeatCounter > 3)
				{
					return;
				}
				bool flag = false;
				bool flag2 = false;
				if (response != null && response.Contains("NO DATA"))
				{
					flag = true;
				}
				if (!flag)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(response);
					string[] array = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					if (text.Contains(this.nr78_string) && array.Length == 1)
					{
						flag2 = true;
					}
				}
				if (flag || flag2)
				{
					int nr78RepeatCounter = request2.NR78RepeatCounter;
					request2.NR78RepeatCounter = nr78RepeatCounter + 1;
					App.OBDReader.InsertRequestInQueue(request2);
				}
			}

			// Token: 0x04001130 RID: 4400
			public OBDRequest request;

			// Token: 0x04001131 RID: 4401
			public string service;

			// Token: 0x04001132 RID: 4402
			public string nr78_string;
		}
	}
}
