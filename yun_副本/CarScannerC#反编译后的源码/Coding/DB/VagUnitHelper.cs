using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB
{
	// Token: 0x020009CD RID: 2509
	internal static class VagUnitHelper
	{
		// Token: 0x06005126 RID: 20774 RVA: 0x003EE9E0 File Offset: 0x003ECBE0
		public static bool IsVag(string brand)
		{
			return brand == "Audi" || brand == "Bentley" || brand == "Bugatti" || brand == "Jetta" || brand == "Porsche" || brand == "Seat" || brand == "Skoda" || brand == "Volkswagen" || brand == "Jetta" || brand == "Cupra";
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x003EEA72 File Offset: 0x003ECC72
		public static string BrandToVAG(string brand)
		{
			if (VagUnitHelper.IsVag(brand))
			{
				return "VAG";
			}
			return brand;
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x003EEA83 File Offset: 0x003ECC83
		public static string GetResponseHeaderForMQBUnit(string unit)
		{
			unit = unit.ToUpperInvariant();
			if (unit.Length == 3)
			{
				return VagUnitHelper.GetResponseHeaderForMQBUnitFromRequestHeader(unit);
			}
			return VagUnitHelper.GetResponseHeaderForMQBUnitFromRequestHeader(VagUnitHelper.GetRequestHeaderForMQBUnit(unit));
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x003EEAA8 File Offset: 0x003ECCA8
		private static string GetResponseHeaderForMQBUnitFromRequestHeader(string requestHeader)
		{
			uint num;
			if (requestHeader.Length == 8 && uint.TryParse(requestHeader, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
			{
				return (num + 131072U).ToString("X8");
			}
			return CAN11bitHelper.GetPossibleResponseHeader(requestHeader, "Audi", null);
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x003EEAF4 File Offset: 0x003ECCF4
		public static string GetRequestHeaderForMQBUnit(string unit)
		{
			unit = unit.ToUpperInvariant();
			if (unit.Length < 2)
			{
				unit = "0" + unit;
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(unit);
			if (num <= 971033634U)
			{
				if (num <= 451618993U)
				{
					if (num <= 350953279U)
					{
						if (num <= 150901779U)
						{
							if (num != 81951608U)
							{
								if (num != 134124160U)
								{
									if (num == 150901779U)
									{
										if (unit == "95")
										{
											return "784";
										}
									}
								}
								else if (unit == "94")
								{
									return "6BC";
								}
							}
							else if (unit == "6С")
							{
								return "6B8";
							}
						}
						else if (num <= 216585232U)
						{
							if (num != 165493046U)
							{
								if (num == 216585232U)
								{
									if (unit == "69")
									{
										return "747";
									}
								}
							}
							else if (unit == "B7")
							{
								return "732";
							}
						}
						else if (num != 334175660U)
						{
							if (num == 350953279U)
							{
								if (unit == "19")
								{
									return "710";
								}
							}
						}
						else if (unit == "18")
						{
							return "76A";
						}
					}
					else if (num <= 418063755U)
					{
						if (num <= 401286136U)
						{
							if (num != 384802707U)
							{
								if (num == 401286136U)
								{
									if (unit == "14")
									{
										return "772";
									}
								}
							}
							else if (unit == "3C")
							{
								return "74E";
							}
						}
						else if (num != 417916660U)
						{
							if (num == 418063755U)
							{
								if (unit == "15")
								{
									return "715";
								}
							}
						}
						else if (unit == "65")
						{
							return "70B";
						}
					}
					else if (num <= 434841374U)
					{
						if (num != 418210850U)
						{
							if (num == 434841374U)
							{
								if (unit == "16")
								{
									return "70C";
								}
							}
						}
						else if (unit == "09")
						{
							return "70E";
						}
					}
					else if (num != 434988469U)
					{
						if (num == 451618993U)
						{
							if (unit == "17")
							{
								return "714";
							}
						}
					}
					else if (unit == "08")
					{
						return "746";
					}
				}
				else if (num <= 517823045U)
				{
					if (num <= 485174231U)
					{
						if (num <= 468543707U)
						{
							if (num != 468396612U)
							{
								if (num == 468543707U)
								{
									if (unit == "06")
									{
										return "74D";
									}
								}
							}
							else if (unit == "10")
							{
								return "70A";
							}
						}
						else if (num != 468690802U)
						{
							if (num == 485174231U)
							{
								if (unit == "11")
								{
									return "7E2";
								}
							}
						}
						else if (unit == "3D")
						{
							return "72C";
						}
					}
					else if (num <= 501045426U)
					{
						if (num != 485321326U)
						{
							if (num == 501045426U)
							{
								if (unit == "BC")
								{
									return "73F";
								}
							}
						}
						else if (unit == "05")
						{
							return "732";
						}
					}
					else if (num != 502098945U)
					{
						if (num == 517823045U)
						{
							if (unit == "BB")
							{
								return "73E";
							}
						}
					}
					else if (unit == "04")
					{
						return "712";
					}
				}
				else if (num <= 552431802U)
				{
					if (num <= 518876564U)
					{
						if (num != 518729469U)
						{
							if (num == 518876564U)
							{
								if (unit == "03")
								{
									return "713";
								}
							}
						}
						else if (unit == "13")
						{
							return "757";
						}
					}
					else if (num != 535654183U)
					{
						if (num == 552431802U)
						{
							if (unit == "01")
							{
								return "7E0";
							}
						}
					}
					else if (unit == "02")
					{
						return "7E1";
					}
				}
				else if (num <= 703136183U)
				{
					if (num != 652803326U)
					{
						if (num == 703136183U)
						{
							if (unit == "6D")
							{
								return "723";
							}
						}
					}
					else if (unit == "6C")
					{
						return "769";
					}
				}
				else if (num != 736691421U)
				{
					if (num == 971033634U)
					{
						if (unit == "5F")
						{
							return "773";
						}
					}
				}
				else if (unit == "6F")
				{
					return "745";
				}
			}
			else if (num <= 2330167868U)
			{
				if (num <= 2247118416U)
				{
					if (num <= 1827530846U)
					{
						if (num != 1307277562U)
						{
							if (num != 1474509299U)
							{
								if (num == 1827530846U)
								{
									if (unit == "0E")
									{
										return "770";
									}
								}
							}
							else if (unit == "4B")
							{
								return "17FC00A9";
							}
						}
						else if (unit == "1B")
						{
							return "716";
						}
					}
					else if (num <= 2229355059U)
					{
						if (num != 2045095440U)
						{
							if (num == 2229355059U)
							{
								if (unit == "51")
								{
									return "7E6";
								}
							}
						}
						else if (unit == "7F")
						{
							return "75A";
						}
					}
					else if (num != 2246132678U)
					{
						if (num == 2247118416U)
						{
							if (unit == "32")
							{
								return "71E";
							}
						}
					}
					else if (unit == "52")
					{
						return "74B";
					}
				}
				else if (num <= 2297598368U)
				{
					if (num <= 2279835011U)
					{
						if (num != 2262910297U)
						{
							if (num == 2279835011U)
							{
								if (unit == "42")
								{
									return "74A";
								}
							}
						}
						else if (unit == "53")
						{
							return "752";
						}
					}
					else if (num != 2296465535U)
					{
						if (num == 2297598368U)
						{
							if (unit == "25")
							{
								return "711";
							}
						}
					}
					else if (unit == "55")
					{
						return "754";
					}
				}
				else if (num <= 2314228892U)
				{
					if (num != 2313537344U)
					{
						if (num == 2314228892U)
						{
							if (unit == "36")
							{
								return "74C";
							}
						}
					}
					else if (unit == "76")
					{
						return "70A";
					}
				}
				else if (num != 2330020773U)
				{
					if (num == 2330167868U)
					{
						if (unit == "47")
						{
							return "76F";
						}
					}
				}
				else if (unit == "57")
				{
					return "76D";
				}
			}
			else if (num <= 2364708844U)
			{
				if (num <= 2347092582U)
				{
					if (num <= 2331006511U)
					{
						if (num != 2330314963U)
						{
							if (num == 2331006511U)
							{
								if (unit == "37")
								{
									return "76C";
								}
							}
						}
						else if (unit == "77")
						{
							return "76B";
						}
					}
					else if (num != 2346945487U)
					{
						if (num == 2347092582U)
						{
							if (unit == "74")
							{
								return "17FC0080";
							}
						}
					}
					else if (unit == "46")
					{
						return "17FC008B";
					}
				}
				else if (num <= 2347931225U)
				{
					if (num != 2347784130U)
					{
						if (num == 2347931225U)
						{
							if (unit == "26")
							{
								return "72D";
							}
						}
					}
					else if (unit == "34")
					{
						return "755";
					}
				}
				else if (num != 2363870201U)
				{
					if (num == 2364708844U)
					{
						if (unit == "21")
						{
							return "728";
						}
					}
				}
				else if (unit == "75")
				{
					return "767";
				}
			}
			else if (num <= 2415041701U)
			{
				if (num <= 2381486463U)
				{
					if (num != 2380500725U)
					{
						if (num == 2381486463U)
						{
							if (unit == "20")
							{
								return "730";
							}
						}
					}
					else if (unit == "44")
					{
						return "712";
					}
				}
				else if (num != 2398264082U)
				{
					if (num == 2415041701U)
					{
						if (unit == "22")
						{
							return "70F";
						}
					}
				}
				else if (unit == "23")
				{
					return "73B";
				}
			}
			else if (num <= 2430980677U)
			{
				if (num != 2416027439U)
				{
					if (num == 2430980677U)
					{
						if (unit == "71")
						{
							return "71D";
						}
					}
				}
				else if (unit == "82")
				{
					return "71B";
				}
			}
			else if (num != 2515707415U)
			{
				if (num == 2547370491U)
				{
					if (unit == "A5")
					{
						return "74F";
					}
				}
			}
			else if (unit == "28")
			{
				return "71A";
			}
			throw new Exception("Unknown VAG unit " + unit);
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x003EF5F8 File Offset: 0x003ED7F8
		internal static string GetUnitIdFromRequestHeader(string requestHeader)
		{
			if (requestHeader != null)
			{
				int length = requestHeader.Length;
				if (length != 3)
				{
					if (length == 8)
					{
						char c = requestHeader[7];
						if (c != '0')
						{
							if (c != '9')
							{
								if (c == 'B')
								{
									if (requestHeader == "17FC008B")
									{
										return "46";
									}
								}
							}
							else if (requestHeader == "17FC00A9")
							{
								return "4B";
							}
						}
						else if (requestHeader == "17FC0080")
						{
							return "74";
						}
					}
				}
				else
				{
					switch (requestHeader[2])
					{
					case '0':
						if (requestHeader == "7E0")
						{
							return "01";
						}
						if (requestHeader == "770")
						{
							return "0E";
						}
						if (requestHeader == "710")
						{
							return "19";
						}
						if (requestHeader == "730")
						{
							return "20";
						}
						break;
					case '1':
						if (requestHeader == "7E1")
						{
							return "02";
						}
						if (requestHeader == "711")
						{
							return "25";
						}
						break;
					case '2':
						if (requestHeader == "712")
						{
							return "44";
						}
						if (requestHeader == "732")
						{
							return "05";
						}
						if (requestHeader == "7E2")
						{
							return "11";
						}
						if (requestHeader == "772")
						{
							return "14";
						}
						if (requestHeader == "752")
						{
							return "53";
						}
						break;
					case '3':
						if (requestHeader == "713")
						{
							return "03";
						}
						if (requestHeader == "773")
						{
							return "5F";
						}
						if (requestHeader == "723")
						{
							return "6D";
						}
						break;
					case '4':
						if (requestHeader == "714")
						{
							return "17";
						}
						if (requestHeader == "754")
						{
							return "55";
						}
						if (requestHeader == "784")
						{
							return "95";
						}
						break;
					case '5':
						if (requestHeader == "715")
						{
							return "15";
						}
						if (requestHeader == "755")
						{
							return "34";
						}
						if (requestHeader == "745")
						{
							return "6F";
						}
						break;
					case '6':
						if (requestHeader == "746")
						{
							return "08";
						}
						if (requestHeader == "716")
						{
							return "1B";
						}
						if (requestHeader == "7E6")
						{
							return "51";
						}
						break;
					case '7':
						if (requestHeader == "757")
						{
							return "13";
						}
						if (requestHeader == "747")
						{
							return "69";
						}
						if (requestHeader == "767")
						{
							return "75";
						}
						break;
					case '8':
						if (requestHeader == "728")
						{
							return "21";
						}
						if (requestHeader == "6B8")
						{
							return "6С";
						}
						break;
					case '9':
						if (requestHeader == "769")
						{
							return "6C";
						}
						break;
					case 'A':
						if (requestHeader == "70A")
						{
							return "10";
						}
						if (requestHeader == "76A")
						{
							return "18";
						}
						if (requestHeader == "71A")
						{
							return "28";
						}
						if (requestHeader == "74A")
						{
							return "42";
						}
						if (requestHeader == "75A")
						{
							return "7F";
						}
						break;
					case 'B':
						if (requestHeader == "73B")
						{
							return "23";
						}
						if (requestHeader == "74B")
						{
							return "52";
						}
						if (requestHeader == "70B")
						{
							return "65";
						}
						if (requestHeader == "76B")
						{
							return "77";
						}
						if (requestHeader == "71B")
						{
							return "82";
						}
						break;
					case 'C':
						if (requestHeader == "70C")
						{
							return "16";
						}
						if (requestHeader == "74C")
						{
							return "36";
						}
						if (requestHeader == "76C")
						{
							return "37";
						}
						if (requestHeader == "72C")
						{
							return "3D";
						}
						if (requestHeader == "6BC")
						{
							return "94";
						}
						break;
					case 'D':
						if (requestHeader == "74D")
						{
							return "06";
						}
						if (requestHeader == "72D")
						{
							return "26";
						}
						if (requestHeader == "76D")
						{
							return "57";
						}
						if (requestHeader == "71D")
						{
							return "71";
						}
						break;
					case 'E':
						if (requestHeader == "70E")
						{
							return "09";
						}
						if (requestHeader == "71E")
						{
							return "32";
						}
						if (requestHeader == "74E")
						{
							return "3C";
						}
						if (requestHeader == "73E")
						{
							return "BB";
						}
						break;
					case 'F':
						if (requestHeader == "70F")
						{
							return "22";
						}
						if (requestHeader == "76F")
						{
							return "47";
						}
						if (requestHeader == "74F")
						{
							return "A5";
						}
						if (requestHeader == "73F")
						{
							return "BC";
						}
						break;
					}
				}
			}
			return "";
		}
	}
}
