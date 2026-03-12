using System;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B67 RID: 2919
	internal static class VagModelDetector
	{
		// Token: 0x060059F2 RID: 23026 RVA: 0x0042E398 File Offset: 0x0042C598
		public static string GetModelFromVin(string vin)
		{
			string text = vin.Substring(6, 2).ToUpper();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1189834229U)
			{
				if (num <= 769702206U)
				{
					if (num <= 518582374U)
					{
						if (num <= 434841374U)
						{
							if (num != 368025088U)
							{
								if (num != 384802707U)
								{
									if (num == 434841374U)
									{
										if (text == "16")
										{
											return "Beetle (2012-on)";
										}
									}
								}
								else if (text == "3C")
								{
									return "Passat 6, 7, 8, CC";
								}
							}
							else if (text == "3B")
							{
								return "Passat 5";
							}
						}
						else if (num != 468690802U)
						{
							if (num != 485174231U)
							{
								if (num == 518582374U)
								{
									if (text == "6K")
									{
										return "Polo Classic, Variant 3";
									}
								}
							}
							else if (text == "11")
							{
								return "Beetle (Brazilian, Mexican, Nigerian)";
							}
						}
						else if (text == "3D")
						{
							return "Phaeton";
						}
					}
					else if (num <= 602470469U)
					{
						if (num != 518729469U)
						{
							if (num != 535801278U)
							{
								if (num == 602470469U)
								{
									if (text == "6N")
									{
										return "Polo 3";
									}
								}
							}
							else if (text == "3H")
							{
								return "Arteon";
							}
						}
						else if (text == "13")
						{
							return "Scirocco 3";
						}
					}
					else if (num != 635854758U)
					{
						if (num != 735387639U)
						{
							if (num == 769702206U)
							{
								if (text == "5Z")
								{
									return "Fox (Europe)";
								}
							}
						}
						else if (text == "AA")
						{
							return "Up!";
						}
					}
					else if (text == "NS")
					{
						return "Kodiaq";
					}
				}
				else if (num <= 955388848U)
				{
					if (num <= 936719067U)
					{
						if (num != 770246659U)
						{
							if (num != 887837087U)
							{
								if (num == 936719067U)
								{
									if (text == "AU")
									{
										return "Golf 7";
									}
								}
							}
							else if (text == "1Y")
							{
								return "New Beetle Cabriolet";
							}
						}
						else if (text == "6X")
						{
							return "Lupo";
						}
					}
					else if (num != 938022849U)
					{
						if (num != 938169944U)
						{
							if (num == 955388848U)
							{
								if (text == "2E")
								{
									return "Crafter 1";
								}
							}
						}
						else if (text == "1T")
						{
							return "Touran";
						}
					}
					else if (text == "6R")
					{
						return "Polo 5";
					}
				}
				else if (num <= 1054921729U)
				{
					if (num != 970274305U)
					{
						if (num != 972166467U)
						{
							if (num == 1054921729U)
							{
								if (text == "5K")
								{
									return "Golf and Jetta 6";
								}
							}
						}
						else if (text == "2D")
						{
							return "LT Transporter 2";
						}
					}
					else if (text == "AW")
					{
						return "Polo 6";
					}
				}
				else if (num <= 1105254586U)
				{
					if (num != 1088476967U)
					{
						if (num == 1105254586U)
						{
							if (text == "5N")
							{
								return "Tiguan 1, 2, Tiguan Allspace";
							}
						}
					}
					else if (text == "5M")
					{
						return "Golf Plus";
					}
				}
				else if (num != 1173497895U)
				{
					if (num == 1189834229U)
					{
						if (text == "1K")
						{
							return "Golf and Jetta 5, 6";
						}
					}
				}
				else if (text == "2H")
				{
					return "Amarok";
				}
			}
			else if (num <= 2246426868U)
			{
				if (num <= 1877319250U)
				{
					if (num <= 1324055181U)
					{
						if (num != 1190275514U)
						{
							if (num != 1240167086U)
							{
								if (num == 1324055181U)
								{
									if (text == "1C")
									{
										return "New Beetle (US market)";
									}
								}
							}
							else if (text == "1F")
							{
								return "Eos";
							}
						}
						else if (text == "2K")
						{
							return "Caddy, Caddy Maxi 3";
						}
					}
					else if (num != 1577225803U)
					{
						if (num != 1761553203U)
						{
							if (num == 1877319250U)
							{
								if (text == "7P")
								{
									return "Touareg 2";
								}
							}
						}
						else if (text == "9U")
						{
							return "Caddy 2 Pickup (ex-Skoda Felicia)";
						}
					}
					else if (text == "SK")
					{
						return "Caddy 4";
					}
				}
				else if (num <= 2130660821U)
				{
					if (num != 1879222945U)
					{
						if (num != 2061113730U)
						{
							if (num == 2130660821U)
							{
								if (text == "9C")
								{
									return "New Beetle";
								}
							}
						}
						else if (text == "CD")
						{
							return "Golf 8";
						}
					}
					else if (text == "SY")
					{
						return "Crafter 2";
					}
				}
				else if (num <= 2212871630U)
				{
					if (num != 2180993678U)
					{
						if (num == 2212871630U)
						{
							if (text == "7L")
							{
								return "Touareg 1";
							}
						}
					}
					else if (text == "9N")
					{
						return "Polo 4";
					}
				}
				else if (num != 2229649249U)
				{
					if (num == 2246426868U)
					{
						if (text == "7J")
						{
							return "T6 Transporter";
						}
					}
				}
				else if (text == "7M")
				{
					return "Sharan";
				}
			}
			else if (num <= 2297598368U)
			{
				if (num <= 2279222777U)
				{
					if (num != 2262910297U)
					{
						if (num != 2264881773U)
						{
							if (num == 2279222777U)
							{
								if (text == "C1")
								{
									return "T-Cross";
								}
							}
						}
						else if (text == "9K")
						{
							return "Caddy 2 Van (ex-SEAT Ibiza)";
						}
					}
					else if (text == "53")
					{
						return "Scirocco 1, 2";
					}
				}
				else if (num != 2279982106U)
				{
					if (num != 2280673654U)
					{
						if (num == 2297598368U)
						{
							if (text == "25")
							{
								return "T3 Transporter Van, Kombi, Bus, Caravelle";
							}
						}
					}
					else if (text == "30")
					{
						return "Fox (US model ex-Brazil)";
					}
				}
				else if (text == "7H")
				{
					return "T5 Transporter, T6.1 Transporter";
				}
			}
			else if (num <= 2348916963U)
			{
				if (num != 2314375987U)
				{
					if (num != 2332139344U)
					{
						if (num == 2348916963U)
						{
							if (text == "86")
							{
								return "Polo and Derby 1 and 2";
							}
						}
					}
					else if (text == "87")
					{
						return "Polo Coupe";
					}
				}
				else if (text == "24")
				{
					return "T3 Transporter Single/Double Cab Pickup";
				}
			}
			else if (num <= 2414203058U)
			{
				if (num != 2363870201U)
				{
					if (num == 2414203058U)
					{
						if (text == "70")
						{
							return "T4 Transporter Vans and Pickups";
						}
					}
				}
				else if (text == "75")
				{
					return "Taro";
				}
			}
			else if (num != 2515707415U)
			{
				if (num == 2614480967U)
				{
					if (text == "A1")
					{
						return "T-Roc";
					}
				}
			}
			else if (text == "28")
			{
				return "LT Transporter 1";
			}
			return "Unknown";
		}
	}
}
