using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.OBD2.VWTP20
{
	// Token: 0x020003C0 RID: 960
	internal class VWTPManager : IVWTPManager
	{
		// Token: 0x06002793 RID: 10131 RVA: 0x001E6550 File Offset: 0x001E4750
		public void Clear()
		{
			this.ECUs.Clear();
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x001E6560 File Offset: 0x001E4760
		public int GetFreeCRAChannel()
		{
			VWTPECU[] array = this.ECUs.Values.Where((VWTPECU x) => x.CurrentState > VWTPECU.ECUState.Disconnected).ToArray<VWTPECU>();
			for (int i = 768; i < 1023; i++)
			{
				string hex = i.ToString("X3");
				if (!array.Any((VWTPECU x) => x.ResponseHeader == hex))
				{
					return i;
				}
			}
			return 1023;
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x001E65EC File Offset: 0x001E47EC
		public async ValueTask<string> SendCommand(string cmd)
		{
			string[] array = cmd.Split(VWTPManager.CmdSplitter, StringSplitOptions.RemoveEmptyEntries);
			string text;
			if (array.Length != 3)
			{
				text = "NO DATA";
			}
			else if (array[0] != "VWTP")
			{
				text = "NO DATA";
			}
			else
			{
				string text2 = array[1];
				string text3 = array[2];
				VWTPECU vwtpecu;
				if (!this.ECUs.TryGetValue(text2, out vwtpecu))
				{
					vwtpecu = new VWTPECU(text2, this);
					this.ECUs.Add(text2, vwtpecu);
				}
				string text4 = await vwtpecu.SendCommand(text3);
				App.OBDReader.SetELM327_LastSentHeader("000");
				text = text4;
			}
			return text;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x001E6638 File Offset: 0x001E4838
		public static string UnitToCANAddress(string hex)
		{
			if (string.IsNullOrEmpty(hex))
			{
				return "";
			}
			hex = hex.ToUpperInvariant();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(hex);
			if (num <= 1240167086U)
			{
				if (num <= 468690802U)
				{
					if (num <= 417916660U)
					{
						if (num <= 350953279U)
						{
							if (num <= 233362851U)
							{
								if (num != 216585232U)
								{
									if (num == 233362851U)
									{
										if (hex == "68")
										{
											return "32";
										}
									}
								}
								else if (hex == "69")
								{
									return "43";
								}
							}
							else if (num != 334175660U)
							{
								if (num != 350806184U)
								{
									if (num == 350953279U)
									{
										if (hex == "19")
										{
											return "1F";
										}
									}
								}
								else if (hex == "61")
								{
									return "33";
								}
							}
							else if (hex == "18")
							{
								return "2F";
							}
						}
						else if (num <= 384802707U)
						{
							if (num != 384361422U)
							{
								if (num == 384802707U)
								{
									if (hex == "3C")
									{
										return "1C";
									}
								}
							}
							else if (hex == "63")
							{
								return "46";
							}
						}
						else if (num != 401139041U)
						{
							if (num != 401286136U)
							{
								if (num == 417916660U)
								{
									if (hex == "65")
									{
										return "29";
									}
								}
							}
							else if (hex == "14")
							{
								return "0C";
							}
						}
						else if (hex == "62")
						{
							return "24";
						}
					}
					else if (num <= 451471898U)
					{
						if (num <= 418210850U)
						{
							if (num != 418063755U)
							{
								if (num == 418210850U)
								{
									if (hex == "09")
									{
										return "20";
									}
								}
							}
							else if (hex == "15")
							{
								return "05";
							}
						}
						else if (num != 434841374U)
						{
							if (num != 434988469U)
							{
								if (num == 451471898U)
								{
									if (hex == "67")
									{
										return "5C";
									}
								}
							}
							else if (hex == "08")
							{
								return "2C";
							}
						}
						else if (hex == "16")
						{
							return "2A";
						}
					}
					else if (num <= 468249517U)
					{
						if (num != 451618993U)
						{
							if (num != 451766088U)
							{
								if (num == 468249517U)
								{
									if (hex == "66")
									{
										return "36";
									}
								}
							}
							else if (hex == "07")
							{
								return "3F";
							}
						}
						else if (hex == "17")
						{
							return "07";
						}
					}
					else if (num != 468396612U)
					{
						if (num != 468543707U)
						{
							if (num == 468690802U)
							{
								if (hex == "3D")
								{
									return "3A";
								}
							}
						}
						else if (hex == "06")
						{
							return "35";
						}
					}
					else if (hex == "10")
					{
						return "1D";
					}
				}
				else if (num <= 703136183U)
				{
					if (num <= 518729469U)
					{
						if (num <= 485321326U)
						{
							if (num != 485174231U)
							{
								if (num == 485321326U)
								{
									if (hex == "05")
									{
										return "31";
									}
								}
							}
							else if (hex == "11")
							{
								return "15";
							}
						}
						else if (num != 485468421U)
						{
							if (num != 502098945U)
							{
								if (num == 518729469U)
								{
									if (hex == "13")
									{
										return "0B";
									}
								}
							}
							else if (hex == "04")
							{
								return "13";
							}
						}
						else if (hex == "3E")
						{
							return "62";
						}
					}
					else
					{
						if (num <= 552431802U)
						{
							if (num != 518876564U)
							{
								if (num != 535654183U)
								{
									if (num != 552431802U)
									{
										goto IL_0EA4;
									}
									if (!(hex == "01"))
									{
										goto IL_0EA4;
									}
								}
								else if (!(hex == "02"))
								{
									goto IL_0EA4;
								}
							}
							else if (!(hex == "03"))
							{
								goto IL_0EA4;
							}
							return hex;
						}
						if (num != 652803326U)
						{
							if (num != 686358564U)
							{
								if (num == 703136183U)
								{
									if (hex == "6D")
									{
										return "34";
									}
								}
							}
							else if (hex == "6E")
							{
								return "3D";
							}
						}
						else if (hex == "6C")
						{
							return "49";
						}
					}
				}
				else if (num <= 955388848U)
				{
					if (num <= 920700777U)
					{
						if (num != 736691421U)
						{
							if (num == 920700777U)
							{
								if (hex == "5C")
								{
									return "1A";
								}
							}
						}
						else if (hex == "6F")
						{
							return "3C";
						}
					}
					else if (num != 937478396U)
					{
						if (num != 954256015U)
						{
							if (num == 955388848U)
							{
								if (hex == "2E")
								{
									return "54";
								}
							}
						}
						else if (hex == "5E")
						{
							return "5E";
						}
					}
					else if (hex == "5D")
					{
						return "4B";
					}
				}
				else if (num <= 1005721705U)
				{
					if (num != 971033634U)
					{
						if (num != 972166467U)
						{
							if (num == 1005721705U)
							{
								if (hex == "2F")
								{
									return "4C";
								}
							}
						}
						else if (hex == "2D")
						{
							return "59";
						}
					}
					else if (hex == "5F")
					{
						return "4D";
					}
				}
				else if (num != 1206611848U)
				{
					if (num != 1223389467U)
					{
						if (num == 1240167086U)
						{
							if (hex == "1F")
							{
								return "5F";
							}
						}
					}
					else if (hex == "1E")
					{
						return "50";
					}
				}
				else if (hex == "1D")
				{
					return "2B";
				}
			}
			else if (num <= 2330167868U)
			{
				if (num <= 2095428297U)
				{
					if (num <= 1558397394U)
					{
						if (num <= 1324055181U)
						{
							if (num != 1307277562U)
							{
								if (num == 1324055181U)
								{
									if (hex == "1C")
									{
										return "40";
									}
								}
							}
							else if (hex == "1B")
							{
								return "1E";
							}
						}
						else if (num != 1457731680U)
						{
							if (num != 1541619775U)
							{
								if (num == 1558397394U)
								{
									if (hex == "4E")
									{
										return "4C";
									}
								}
							}
							else if (hex == "4F")
							{
								return "28";
							}
						}
						else if (hex == "4C")
						{
							return "08";
						}
					}
					else if (num <= 1844308465U)
					{
						if (num != 1810753227U)
						{
							if (num != 1827530846U)
							{
								if (num == 1844308465U)
								{
									if (hex == "0D")
									{
										return "24";
									}
								}
							}
							else if (hex == "0E")
							{
								return "58";
							}
						}
						else if (hex == "0F")
						{
							return "4F";
						}
					}
					else if (num != 2045095440U)
					{
						if (num != 2078650678U)
						{
							if (num == 2095428297U)
							{
								if (hex == "7E")
								{
									return "48";
								}
							}
						}
						else if (hex == "7D")
						{
							return "2E";
						}
					}
					else if (hex == "7F")
					{
						return "4E";
					}
				}
				else if (num <= 2296465535U)
				{
					if (num <= 2247118416U)
					{
						if (num != 2246132678U)
						{
							if (num == 2247118416U)
							{
								if (hex == "32")
								{
									return "18";
								}
							}
						}
						else if (hex == "52")
						{
							return "23";
						}
					}
					else if (num != 2262910297U)
					{
						if (num != 2279835011U)
						{
							if (num == 2296465535U)
							{
								if (hex == "55")
								{
									return "06";
								}
							}
						}
						else if (hex == "42")
						{
							return "22";
						}
					}
					else if (hex == "53")
					{
						return "19";
					}
				}
				else if (num <= 2313537344U)
				{
					if (num != 2297598368U)
					{
						if (num != 2313243154U)
						{
							if (num == 2313537344U)
							{
								if (hex == "76")
								{
									return "2D";
								}
							}
						}
						else if (hex == "56")
						{
							return "52";
						}
					}
					else if (hex == "25")
					{
						return "14";
					}
				}
				else if (num != 2314228892U)
				{
					if (num != 2330020773U)
					{
						if (num == 2330167868U)
						{
							if (hex == "47")
							{
								return "53";
							}
						}
					}
					else if (hex == "57")
					{
						return "57";
					}
				}
				else if (hex == "36")
				{
					return "26";
				}
			}
			else if (num <= 2380647820U)
			{
				if (num <= 2347092582U)
				{
					if (num <= 2331006511U)
					{
						if (num != 2330314963U)
						{
							if (num == 2331006511U)
							{
								if (hex == "37")
								{
									return "5B";
								}
							}
						}
						else if (hex == "77")
						{
							return "5A";
						}
					}
					else if (num != 2331153606U)
					{
						if (num != 2346945487U)
						{
							if (num == 2347092582U)
							{
								if (hex == "74")
								{
									return "1B";
								}
							}
						}
						else if (hex == "46")
						{
							return "21";
						}
					}
					else if (hex == "27")
					{
						return "3E";
					}
				}
				else if (num <= 2363576011U)
				{
					if (num != 2347784130U)
					{
						if (num != 2347931225U)
						{
							if (num == 2363576011U)
							{
								if (hex == "59")
								{
									return "41";
								}
							}
						}
						else if (hex == "26")
						{
							return "30";
						}
					}
					else if (hex == "34")
					{
						return "04";
					}
				}
				else if (num != 2363870201U)
				{
					if (num != 2380500725U)
					{
						if (num == 2380647820U)
						{
							if (hex == "72")
							{
								return "25";
							}
						}
					}
					else if (hex == "44")
					{
						return "09";
					}
				}
				else if (hex == "75")
				{
					return "55";
				}
			}
			else if (num <= 2415041701U)
			{
				if (num <= 2397425439U)
				{
					if (num != 2381486463U)
					{
						if (num == 2397425439U)
						{
							if (hex == "73")
							{
								return "47";
							}
						}
					}
					else if (hex == "20")
					{
						return "16";
					}
				}
				else if (num != 2398264082U)
				{
					if (num != 2414894606U)
					{
						if (num == 2415041701U)
						{
							if (hex == "22")
							{
								return "0A";
							}
						}
					}
					else if (hex == "38")
					{
						return "27";
					}
				}
				else if (hex == "23")
				{
					return "0D";
				}
			}
			else if (num <= 2447611201U)
			{
				if (num != 2430980677U)
				{
					if (num != 2431672225U)
					{
						if (num == 2447611201U)
						{
							if (hex == "48")
							{
								return "37";
							}
						}
					}
					else if (hex == "39")
					{
						return "39";
					}
				}
				else if (hex == "71")
				{
					return "0E";
				}
			}
			else if (num != 2498929796U)
			{
				if (num != 2515707415U)
				{
					if (num == 2548424010U)
					{
						if (hex == "78")
						{
							return "25";
						}
					}
				}
				else if (hex == "28")
				{
					return "45";
				}
			}
			else if (hex == "29")
			{
				return "38";
			}
			IL_0EA4:
			return "";
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x001E74F0 File Offset: 0x001E56F0
		public static string CANAddressToUnit(string hex)
		{
			if (string.IsNullOrEmpty(hex))
			{
				return "";
			}
			hex = hex.ToUpperInvariant();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(hex);
			if (num <= 1558397394U)
			{
				if (num <= 552431802U)
				{
					if (num <= 434988469U)
					{
						if (num <= 401286136U)
						{
							if (num <= 350953279U)
							{
								if (num != 334175660U)
								{
									if (num == 350953279U)
									{
										if (hex == "19")
										{
											return "53";
										}
									}
								}
								else if (hex == "18")
								{
									return "32";
								}
							}
							else if (num != 384802707U)
							{
								if (num != 401139041U)
								{
									if (num == 401286136U)
									{
										if (hex == "14")
										{
											return "25";
										}
									}
								}
								else if (hex == "62")
								{
									return "3E";
								}
							}
							else if (hex == "3C")
							{
								return "6F";
							}
						}
						else if (num <= 418210850U)
						{
							if (num != 418063755U)
							{
								if (num == 418210850U)
								{
									if (hex == "09")
									{
										return "44";
									}
								}
							}
							else if (hex == "15")
							{
								return "11";
							}
						}
						else if (num != 418357945U)
						{
							if (num != 434841374U)
							{
								if (num == 434988469U)
								{
									if (hex == "08")
									{
										return "4C";
									}
								}
							}
							else if (hex == "16")
							{
								return "20";
							}
						}
						else if (hex == "3A")
						{
							return "3D";
						}
					}
					else if (num <= 485321326U)
					{
						if (num <= 451766088U)
						{
							if (num != 435135564U)
							{
								if (num == 451766088U)
								{
									if (hex == "07")
									{
										return "17";
									}
								}
							}
							else if (hex == "3F")
							{
								return "07";
							}
						}
						else if (num != 468543707U)
						{
							if (num != 468690802U)
							{
								if (num == 485321326U)
								{
									if (hex == "05")
									{
										return "15";
									}
								}
							}
							else if (hex == "3D")
							{
								return "6E";
							}
						}
						else if (hex == "06")
						{
							return "55";
						}
					}
					else
					{
						if (num > 518729469U)
						{
							if (num != 518876564U)
							{
								if (num != 535654183U)
								{
									if (num != 552431802U)
									{
										goto IL_0E32;
									}
									if (!(hex == "01"))
									{
										goto IL_0E32;
									}
								}
								else if (!(hex == "02"))
								{
									goto IL_0E32;
								}
							}
							else if (!(hex == "03"))
							{
								goto IL_0E32;
							}
							return hex;
						}
						if (num != 485468421U)
						{
							if (num != 502098945U)
							{
								if (num == 518729469U)
								{
									if (hex == "13")
									{
										return "04";
									}
								}
							}
							else if (hex == "04")
							{
								return "34";
							}
						}
						else if (hex == "3E")
						{
							return "27";
						}
					}
				}
				else if (num <= 1056054562U)
				{
					if (num <= 955388848U)
					{
						if (num <= 903923158U)
						{
							if (num != 887145539U)
							{
								if (num == 903923158U)
								{
									if (hex == "5B")
									{
										return "37";
									}
								}
							}
							else if (hex == "5A")
							{
								return "77";
							}
						}
						else if (num != 920700777U)
						{
							if (num != 954256015U)
							{
								if (num == 955388848U)
								{
									if (hex == "2E")
									{
										return "7D";
									}
								}
							}
							else if (hex == "5E")
							{
								return "5E";
							}
						}
						else if (hex == "5C")
						{
							return "67";
						}
					}
					else if (num <= 972166467U)
					{
						if (num != 971033634U)
						{
							if (num == 972166467U)
							{
								if (hex == "2D")
								{
									return "76";
								}
							}
						}
						else if (hex == "5F")
						{
							return "1F";
						}
					}
					else if (num != 1005721705U)
					{
						if (num != 1022499324U)
						{
							if (num == 1056054562U)
							{
								if (hex == "2C")
								{
									return "08";
								}
							}
						}
						else if (hex == "2A")
						{
							return "16";
						}
					}
					else if (hex == "2F")
					{
						return "18";
					}
				}
				else if (num <= 1290499943U)
				{
					if (num <= 1206611848U)
					{
						if (num != 1072832181U)
						{
							if (num == 1206611848U)
							{
								if (hex == "1D")
								{
									return "10";
								}
							}
						}
						else if (hex == "2B")
						{
							return "1D";
						}
					}
					else if (num != 1223389467U)
					{
						if (num != 1240167086U)
						{
							if (num == 1290499943U)
							{
								if (hex == "1A")
								{
									return "5C";
								}
							}
						}
						else if (hex == "1F")
						{
							return "19";
						}
					}
					else if (hex == "1E")
					{
						return "1B";
					}
				}
				else if (num <= 1457731680U)
				{
					if (num != 1307277562U)
					{
						if (num != 1324055181U)
						{
							if (num == 1457731680U)
							{
								if (hex == "4C")
								{
									return "2F";
								}
							}
						}
						else if (hex == "1C")
						{
							return "3C";
						}
					}
					else if (hex == "1B")
					{
						return "74";
					}
				}
				else if (num != 1474509299U)
				{
					if (num != 1541619775U)
					{
						if (num == 1558397394U)
						{
							if (hex == "4E")
							{
								return "7F";
							}
						}
					}
					else if (hex == "4F")
					{
						return "0F";
					}
				}
				else if (hex == "4B")
				{
					return "5D";
				}
			}
			else if (num <= 2314375987U)
			{
				if (num <= 2262910297U)
				{
					if (num <= 1877863703U)
					{
						if (num <= 1827530846U)
						{
							if (num != 1575175013U)
							{
								if (num == 1827530846U)
								{
									if (hex == "0E")
									{
										return "71";
									}
								}
							}
							else if (hex == "4D")
							{
								return "5F";
							}
						}
						else if (num != 1844308465U)
						{
							if (num != 1861086084U)
							{
								if (num == 1877863703U)
								{
									if (hex == "0B")
									{
										return "13";
									}
								}
							}
							else if (hex == "0C")
							{
								return "14";
							}
						}
						else if (hex == "0D")
						{
							return "23";
						}
					}
					else if (num <= 2212577440U)
					{
						if (num != 1894641322U)
						{
							if (num == 2212577440U)
							{
								if (hex == "50")
								{
									return "1E";
								}
							}
						}
						else if (hex == "0A")
						{
							return "22";
						}
					}
					else if (num != 2246132678U)
					{
						if (num != 2247118416U)
						{
							if (num == 2262910297U)
							{
								if (hex == "53")
								{
									return "47";
								}
							}
						}
						else if (hex == "32")
						{
							return "68";
						}
					}
					else if (hex == "52")
					{
						return "56";
					}
				}
				else if (num <= 2296465535U)
				{
					if (num <= 2263896035U)
					{
						if (num != 2263057392U)
						{
							if (num == 2263896035U)
							{
								if (hex == "33")
								{
									return "61";
								}
							}
						}
						else if (hex == "43")
						{
							return "69";
						}
					}
					else if (num != 2279687916U)
					{
						if (num != 2280673654U)
						{
							if (num == 2296465535U)
							{
								if (hex == "55")
								{
									return "75";
								}
							}
						}
						else if (hex == "30")
						{
							return "26";
						}
					}
					else if (hex == "54")
					{
						return "2E";
					}
				}
				else if (num <= 2297598368U)
				{
					if (num != 2296612630U)
					{
						if (num != 2297451273U)
						{
							if (num == 2297598368U)
							{
								if (hex == "25")
								{
									return "72";
								}
							}
						}
						else if (hex == "31")
						{
							return "05";
						}
					}
					else if (hex == "41")
					{
						return "59";
					}
				}
				else if (num != 2313390249U)
				{
					if (num != 2314228892U)
					{
						if (num == 2314375987U)
						{
							if (hex == "24")
							{
								return "0D";
							}
						}
					}
					else if (hex == "36")
					{
						return "66";
					}
				}
				else if (hex == "40")
				{
					return "1C";
				}
			}
			else if (num <= 2363723106U)
			{
				if (num <= 2346798392U)
				{
					if (num <= 2330167868U)
					{
						if (num != 2330020773U)
						{
							if (num == 2330167868U)
							{
								if (hex == "47")
								{
									return "73";
								}
							}
						}
						else if (hex == "57")
						{
							return "57";
						}
					}
					else if (num != 2331006511U)
					{
						if (num != 2331153606U)
						{
							if (num == 2346798392U)
							{
								if (hex == "58")
								{
									return "0E";
								}
							}
						}
						else if (hex == "27")
						{
							return "38";
						}
					}
					else if (hex == "37")
					{
						return "48";
					}
				}
				else if (num <= 2347784130U)
				{
					if (num != 2346945487U)
					{
						if (num == 2347784130U)
						{
							if (hex == "34")
							{
								return "6D";
							}
						}
					}
					else if (hex == "46")
					{
						return "63";
					}
				}
				else if (num != 2347931225U)
				{
					if (num != 2363576011U)
					{
						if (num == 2363723106U)
						{
							if (hex == "45")
							{
								return "28";
							}
						}
					}
					else if (hex == "59")
					{
						return "2D";
					}
				}
				else if (hex == "26")
				{
					return "36";
				}
			}
			else if (num <= 2414894606U)
			{
				if (num <= 2364708844U)
				{
					if (num != 2364561749U)
					{
						if (num == 2364708844U)
						{
							if (hex == "21")
							{
								return "46";
							}
						}
					}
					else if (hex == "35")
					{
						return "06";
					}
				}
				else if (num != 2381486463U)
				{
					if (num != 2398264082U)
					{
						if (num == 2414894606U)
						{
							if (hex == "38")
							{
								return "29";
							}
						}
					}
					else if (hex == "23")
					{
						return "52";
					}
				}
				else if (hex == "20")
				{
					return "09";
				}
			}
			else if (num <= 2431672225U)
			{
				if (num != 2415041701U)
				{
					if (num != 2430833582U)
					{
						if (num == 2431672225U)
						{
							if (hex == "39")
							{
								return "39";
							}
						}
					}
					else if (hex == "49")
					{
						return "6C";
					}
				}
				else if (hex == "22")
				{
					return "42";
				}
			}
			else if (num != 2447611201U)
			{
				if (num != 2498929796U)
				{
					if (num == 2515707415U)
					{
						if (hex == "28")
						{
							return "4F";
						}
					}
				}
				else if (hex == "29")
				{
					return "65";
				}
			}
			else if (hex == "48")
			{
				return "7E";
			}
			IL_0E32:
			return "";
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x001E8334 File Offset: 0x001E6534
		public static OBDRequest BuildRequest(string command, string unit, bool repeat)
		{
			return new OBDRequest("VWTP:" + unit + ":" + command, "000", "ATCAF0;ATV1;ATSP6;ATCM000", "ATSPDEF;ATCAF1;ATV0", repeat);
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x001E835C File Offset: 0x001E655C
		public VWTPManager()
		{
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x001E836F File Offset: 0x001E656F
		// Note: this type is marked as 'beforefieldinit'.
		static VWTPManager()
		{
		}

		// Token: 0x0400161B RID: 5659
		public static readonly char[] CmdSplitter = new char[] { ':' };

		// Token: 0x0400161C RID: 5660
		private Dictionary<string, VWTPECU> ECUs = new Dictionary<string, VWTPECU>();

		// Token: 0x020003C1 RID: 961
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600279B RID: 10139 RVA: 0x001E8381 File Offset: 0x001E6581
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600279C RID: 10140 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600279D RID: 10141 RVA: 0x001E838D File Offset: 0x001E658D
			internal bool <GetFreeCRAChannel>b__1_0(VWTPECU x)
			{
				return x.CurrentState > VWTPECU.ECUState.Disconnected;
			}

			// Token: 0x0400161D RID: 5661
			public static readonly VWTPManager.<>c <>9 = new VWTPManager.<>c();

			// Token: 0x0400161E RID: 5662
			public static Func<VWTPECU, bool> <>9__1_0;
		}

		// Token: 0x020003C2 RID: 962
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x0600279E RID: 10142 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x0600279F RID: 10143 RVA: 0x001E8398 File Offset: 0x001E6598
			internal bool <GetFreeCRAChannel>b__1(VWTPECU x)
			{
				return x.ResponseHeader == this.hex;
			}

			// Token: 0x0400161F RID: 5663
			public string hex;
		}

		// Token: 0x020003C3 RID: 963
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendCommand>d__2 : IAsyncStateMachine
		{
			// Token: 0x060027A0 RID: 10144 RVA: 0x001E83AC File Offset: 0x001E65AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPManager vwtpmanager = this;
				string text;
				try
				{
					ValueTaskAwaiter<string> valueTaskAwaiter;
					if (num != 0)
					{
						string[] array = cmd.Split(VWTPManager.CmdSplitter, StringSplitOptions.RemoveEmptyEntries);
						if (array.Length != 3)
						{
							text = "NO DATA";
							goto IL_0112;
						}
						if (array[0] != "VWTP")
						{
							text = "NO DATA";
							goto IL_0112;
						}
						string text2 = array[1];
						string text3 = array[2];
						VWTPECU vwtpecu;
						if (!vwtpmanager.ECUs.TryGetValue(text2, out vwtpecu))
						{
							vwtpecu = new VWTPECU(text2, vwtpmanager);
							vwtpmanager.ECUs.Add(text2, vwtpecu);
						}
						valueTaskAwaiter = vwtpecu.SendCommand(text3).GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num2 = 0;
							ValueTaskAwaiter<string> valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<string>, VWTPManager.<SendCommand>d__2>(ref valueTaskAwaiter, ref this);
							return;
						}
					}
					else
					{
						ValueTaskAwaiter<string> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<string>);
						num2 = -1;
					}
					string result = valueTaskAwaiter.GetResult();
					App.OBDReader.SetELM327_LastSentHeader("000");
					text = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0112:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x060027A1 RID: 10145 RVA: 0x001E84F0 File Offset: 0x001E66F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001620 RID: 5664
			public int <>1__state;

			// Token: 0x04001621 RID: 5665
			public AsyncValueTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001622 RID: 5666
			public string cmd;

			// Token: 0x04001623 RID: 5667
			public VWTPManager <>4__this;

			// Token: 0x04001624 RID: 5668
			private ValueTaskAwaiter<string> <>u__1;
		}
	}
}
