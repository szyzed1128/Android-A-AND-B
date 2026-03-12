using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.DTC.VagDTC;
using CarScannerXamarinForms.ECUModels;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.DTCv2
{
	// Token: 0x02000590 RID: 1424
	public class DTCv2Model : INotifyPropertyChanged
	{
		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x00244538 File Offset: 0x00242738
		public static bool IsDTCv2Available
		{
			get
			{
				string text = SharedSettings.Current.BrandForDTC;
				if (string.IsNullOrEmpty(text))
				{
					text = SharedSettings.Current.SelectedBrand;
				}
				switch (App.OBDReader.CurrentELMFormat)
				{
				case ELMFormat.KWP:
					if (SharedSettings.Current.DTCReadingModeV2 == DTCModeV2.ReplaceAuto && !string.IsNullOrEmpty(SharedSettings.Current.DTCReadingSequence))
					{
						return false;
					}
					if (SharedSettings.Current.DTCClearingModeV2 == DTCModeV2.ReplaceAuto && !string.IsNullOrEmpty(SharedSettings.Current.DTCClearingSequence))
					{
						return false;
					}
					if (text != null)
					{
						switch (text.Length)
						{
						case 3:
							if (!(text == "BMW"))
							{
								return true;
							}
							return false;
						case 4:
						{
							char c = text[0];
							if (c != 'A')
							{
								if (c != 'S')
								{
									return true;
								}
								if (!(text == "Seat"))
								{
									return true;
								}
							}
							else if (!(text == "Audi"))
							{
								return true;
							}
							break;
						}
						case 5:
						{
							char c = text[0];
							if (c != 'J')
							{
								if (c != 'S')
								{
									return true;
								}
								if (!(text == "Skoda"))
								{
									return true;
								}
							}
							else if (!(text == "Jetta"))
							{
								return true;
							}
							break;
						}
						case 6:
						{
							char c = text[0];
							if (c != 'D')
							{
								if (c != 'N')
								{
									if (c != 'S')
									{
										return true;
									}
									if (!(text == "Suzuki"))
									{
										return true;
									}
									return false;
								}
								else
								{
									if (!(text == "Nissan"))
									{
										return true;
									}
									return false;
								}
							}
							else if (!(text == "Delphi"))
							{
								return true;
							}
							break;
						}
						case 7:
							if (!(text == "Porsche"))
							{
								return true;
							}
							break;
						case 8:
						case 9:
							return true;
						case 10:
							if (!(text == "Volkswagen"))
							{
								return true;
							}
							break;
						default:
							return true;
						}
						return true;
					}
					return true;
				case ELMFormat.CAN11bit:
					if (text != null)
					{
						switch (text.Length)
						{
						case 2:
						{
							char c = text[0];
							if (c != 'D')
							{
								if (c != 'M')
								{
									return true;
								}
								if (!(text == "MG"))
								{
									return true;
								}
							}
							else if (!(text == "DS"))
							{
								return true;
							}
							break;
						}
						case 3:
						{
							char c = text[0];
							if (c > 'K')
							{
								switch (c)
								{
								case 'O':
									if (!(text == "Ora"))
									{
										return true;
									}
									return true;
								case 'P':
								case 'Q':
								case 'S':
								case 'T':
									return true;
								case 'R':
									if (!(text == "RAM"))
									{
										return true;
									}
									return true;
								case 'U':
									if (!(text == "UAZ"))
									{
										return true;
									}
									break;
								case 'V':
									if (!(text == "VAZ"))
									{
										return true;
									}
									return true;
								case 'W':
									if (!(text == "WEY"))
									{
										return true;
									}
									return true;
								default:
									if (c != 'В')
									{
										if (c != 'У')
										{
											return true;
										}
										if (!(text == "УАЗ"))
										{
											return true;
										}
									}
									else
									{
										if (!(text == "ВАЗ"))
										{
											return true;
										}
										return true;
									}
									break;
								}
								return true;
							}
							if (c != 'B')
							{
								switch (c)
								{
								case 'F':
									if (!(text == "FAW"))
									{
										return true;
									}
									break;
								case 'G':
									if (!(text == "GMC"))
									{
										if (!(text == "GAC"))
										{
											return true;
										}
										return true;
									}
									break;
								case 'H':
								case 'I':
									return true;
								case 'J':
									if (!(text == "JAC"))
									{
										return true;
									}
									break;
								case 'K':
									if (!(text == "Kia") && !(text == "KTM"))
									{
										return true;
									}
									break;
								default:
									return true;
								}
							}
							else if (!(text == "BMW") && !(text == "BYD") && !(text == "Byd"))
							{
								return true;
							}
							break;
						}
						case 4:
						{
							char c = text[3];
							if (c <= 'd')
							{
								if (c <= 'K')
								{
									if (c != 'G')
									{
										if (c != 'K')
										{
											return true;
										}
										if (!(text == "DFSK"))
										{
											return true;
										}
										return true;
									}
									else
									{
										if (!(text == "DFFG"))
										{
											return true;
										}
										return true;
									}
								}
								else if (c != 'O')
								{
									switch (c)
									{
									case 'a':
										if (!(text == "Lada"))
										{
											return true;
										}
										break;
									case 'b':
										if (!(text == "Saab"))
										{
											return true;
										}
										break;
									case 'c':
										return true;
									case 'd':
										if (!(text == "Ford"))
										{
											return true;
										}
										break;
									default:
										return true;
									}
								}
								else
								{
									if (!(text == "AITO"))
									{
										return true;
									}
									return true;
								}
							}
							else if (c <= 'l')
							{
								if (c != 'i')
								{
									if (c != 'l')
									{
										return true;
									}
									if (!(text == "Opel"))
									{
										return true;
									}
								}
								else if (!(text == "Audi") && !(text == "Mini"))
								{
									return true;
								}
							}
							else if (c != 'p')
							{
								if (c != 't')
								{
									if (c != 'а')
									{
										return true;
									}
									if (!(text == "Лада"))
									{
										return true;
									}
								}
								else if (!(text == "Seat"))
								{
									return true;
								}
							}
							else if (!(text == "Jeep"))
							{
								return true;
							}
							break;
						}
						case 5:
						{
							char c = text[2];
							if (c != 'U')
							{
								switch (c)
								{
								case 'c':
									if (!(text == "Dacia"))
									{
										return true;
									}
									break;
								case 'd':
									if (!(text == "Dodge"))
									{
										return true;
									}
									break;
								case 'e':
									if (!(text == "Chery") && !(text == "Exeed") && !(text == "Chevy") && !(text == "Geely"))
									{
										return true;
									}
									break;
								case 'f':
									if (!(text == "Lifan"))
									{
										return true;
									}
									break;
								case 'g':
								case 'j':
								case 'k':
								case 'm':
								case 'n':
								case 'q':
								case 's':
								case 'w':
									return true;
								case 'h':
									if (!(text == "Sehol"))
									{
										return true;
									}
									break;
								case 'i':
									if (!(text == "Kaiyi") && !(text == "Buick"))
									{
										return true;
									}
									break;
								case 'l':
									if (!(text == "Volvo") && !(text == "Euler"))
									{
										return true;
									}
									break;
								case 'o':
									if (!(text == "Skoda") && !(text == "Omoda"))
									{
										return true;
									}
									break;
								case 'p':
									if (!(text == "Cupra"))
									{
										return true;
									}
									break;
								case 'r':
									if (!(text == "Seres"))
									{
										return true;
									}
									return true;
								case 't':
									if (!(text == "Jetta"))
									{
										return true;
									}
									break;
								case 'u':
									if (!(text == "Isuzu"))
									{
										return true;
									}
									break;
								case 'v':
									if (!(text == "Ravon") && !(text == "Haval") && !(text == "Hover"))
									{
										return true;
									}
									break;
								case 'x':
									if (!(text == "Lexus"))
									{
										return true;
									}
									break;
								case 'y':
									if (!(text == "Voyah"))
									{
										return true;
									}
									return true;
								case 'z':
									if (!(text == "Mazda"))
									{
										return true;
									}
									break;
								default:
									return true;
								}
							}
							else if (!(text == "ISUZU"))
							{
								return true;
							}
							break;
						}
						case 6:
						{
							char c = text[2];
							if (c <= 'e')
							{
								if (c != 'b')
								{
									if (c != 'e')
									{
										return true;
									}
									if (!(text == "Daewoo"))
									{
										return true;
									}
								}
								else if (!(text == "Subaru"))
								{
									return true;
								}
							}
							else
							{
								switch (c)
								{
								case 'l':
									if (!(text == "Holden"))
									{
										return true;
									}
									break;
								case 'm':
									if (!(text == "Hummer"))
									{
										if (!(text == "Yamaha"))
										{
											return true;
										}
										return true;
									}
									break;
								case 'n':
									if (!(text == "Hongqi"))
									{
										return true;
									}
									break;
								case 'o':
								case 'p':
								case 'q':
								case 'r':
									return true;
								case 's':
									if (!(text == "Nissan"))
									{
										return true;
									}
									break;
								case 't':
									if (!(text == "Saturn") && !(text == "Jetour"))
									{
										return true;
									}
									break;
								default:
									if (c != 'y')
									{
										if (c != 'z')
										{
											return true;
										}
										if (!(text == "Suzuki"))
										{
											return true;
										}
									}
									else if (!(text == "Toyota"))
									{
										return true;
									}
									break;
								}
							}
							break;
						}
						case 7:
						{
							char c = text[4];
							switch (c)
							{
							case 'c':
								if (!(text == "Porsche"))
								{
									return true;
								}
								break;
							case 'd':
								if (!(text == "Hyundai"))
								{
									return true;
								}
								break;
							case 'e':
								if (!(text == "Peugeot"))
								{
									return true;
								}
								break;
							case 'f':
							case 'h':
							case 'j':
							case 'k':
							case 'm':
							case 'n':
							case 'p':
							case 'q':
							case 'r':
								return true;
							case 'g':
								if (!(text == "Changan"))
								{
									return true;
								}
								break;
							case 'i':
								if (!(text == "Pontiac"))
								{
									return true;
								}
								break;
							case 'l':
								if (!(text == "Bentley"))
								{
									return true;
								}
								break;
							case 'o':
								if (!(text == "Citroen") && !(text == "Lincoln"))
								{
									return true;
								}
								break;
							case 's':
								if (!(text == "Genesis"))
								{
									return true;
								}
								break;
							case 't':
								if (!(text == "Bugatti"))
								{
									return true;
								}
								break;
							case 'u':
								if (!(text == "Renault") && !(text == "Mercury"))
								{
									if (!(text == "Evolute"))
									{
										return true;
									}
									return true;
								}
								break;
							default:
								if (c != 'в')
								{
									return true;
								}
								if (!(text == "Москвич"))
								{
									return true;
								}
								break;
							}
							break;
						}
						case 8:
						{
							char c = text[4];
							if (c <= 'f')
							{
								if (c != 'F')
								{
									if (c != 'f')
									{
										return true;
									}
									if (!(text == "Dongfeng"))
									{
										return true;
									}
									return true;
								}
								else
								{
									if (!(text == "DongFeng"))
									{
										return true;
									}
									return true;
								}
							}
							else if (c != 'h')
							{
								switch (c)
								{
								case 'l':
									if (!(text == "Cadillac"))
									{
										return true;
									}
									break;
								case 'm':
								case 'o':
									return true;
								case 'n':
									if (!(text == "Infiniti") && !(text == "Infinity"))
									{
										return true;
									}
									break;
								case 'p':
									if (!(text == "Trumpchi"))
									{
										return true;
									}
									return true;
								default:
									if (c != 's')
									{
										return true;
									}
									if (!(text == "Chrysler"))
									{
										return true;
									}
									break;
								}
							}
							else if (!(text == "Vauxhall"))
							{
								return true;
							}
							break;
						}
						case 9:
						{
							char c = text[0];
							if (c != 'C')
							{
								if (c != 'S')
								{
									return true;
								}
								if (!(text == "SsangYong"))
								{
									return true;
								}
							}
							else if (!(text == "Chevrolet"))
							{
								return true;
							}
							break;
						}
						case 10:
						{
							char c = text[0];
							if (c <= 'L')
							{
								if (c != 'G')
								{
									if (c != 'L')
									{
										return true;
									}
									if (!(text == "Land Rover"))
									{
										return true;
									}
								}
								else if (!(text == "Great Wall"))
								{
									return true;
								}
							}
							else if (c != 'M')
							{
								if (c != 'V')
								{
									return true;
								}
								if (!(text == "Volkswagen"))
								{
									return true;
								}
							}
							else if (!(text == "Mitsubishi"))
							{
								return true;
							}
							break;
						}
						case 11:
							if (!(text == "Range Rover"))
							{
								return true;
							}
							break;
						case 12:
							return true;
						case 13:
							if (!(text == "Mercedes-Benz"))
							{
								return true;
							}
							break;
						case 14:
							text == "General Motors";
							break;
						default:
							return true;
						}
						return true;
					}
					return true;
				case ELMFormat.CAN29bit:
					if (text != null)
					{
						switch (text.Length)
						{
						case 4:
						{
							char c = text[0];
							if (c <= 'F')
							{
								if (c != 'A')
								{
									if (c != 'F')
									{
										return false;
									}
									if (!(text == "Fiat"))
									{
										return false;
									}
								}
								else if (!(text == "Audi"))
								{
									return false;
								}
							}
							else if (c != 'J')
							{
								if (c != 'S')
								{
									return false;
								}
								if (!(text == "Seat"))
								{
									return false;
								}
							}
							else if (!(text == "Jeep"))
							{
								return false;
							}
							break;
						}
						case 5:
						{
							char c = text[2];
							switch (c)
							{
							case 'c':
								if (!(text == "Dacia"))
								{
									return false;
								}
								break;
							case 'd':
								if (!(text == "Dodge"))
								{
									return false;
								}
								break;
							case 'e':
								if (!(text == "Geely"))
								{
									return false;
								}
								break;
							default:
								switch (c)
								{
								case 'l':
									if (!(text == "Volvo"))
									{
										return false;
									}
									break;
								case 'm':
									return false;
								case 'n':
									if (!(text == "Honda"))
									{
										return false;
									}
									break;
								case 'o':
									if (!(text == "Skoda"))
									{
										return false;
									}
									break;
								case 'p':
									if (!(text == "Cupra"))
									{
										return false;
									}
									break;
								default:
									if (c != 'u')
									{
										return false;
									}
									if (!(text == "Acura"))
									{
										return false;
									}
									break;
								}
								break;
							}
							break;
						}
						case 6:
							if (!(text == "Nissan"))
							{
								return false;
							}
							break;
						case 7:
						{
							char c = text[0];
							if (c != 'P')
							{
								if (c != 'R')
								{
									return false;
								}
								if (!(text == "Renault"))
								{
									return false;
								}
							}
							else if (!(text == "Porsche"))
							{
								return false;
							}
							break;
						}
						case 8:
							return false;
						case 9:
							if (!(text == "Lynk & Co"))
							{
								return false;
							}
							break;
						case 10:
							if (!(text == "Volkswagen"))
							{
								return false;
							}
							break;
						default:
							return false;
						}
						return true;
					}
					break;
				}
				return false;
			}
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x002453E4 File Offset: 0x002435E4
		public DTCv2Model()
		{
			string text = SharedSettings.Current.BrandForDTC;
			if (string.IsNullOrEmpty(text))
			{
				text = SharedSettings.Current.SelectedBrand;
			}
			this.ECUListFound = new ObservableCollection<IECU>();
			this.ReadDTCCommand = new Command(delegate
			{
				this.ReadDTC(null);
			});
			this.CancelCommand = new Command(new Action(this.Cancel));
			this.BackToSetupCommand = new Command(new Action(this.BackToSetup));
			this.BackToMainScreenCommand = new Command(new Action(this.BackToMainScreen));
			this.cancellationTokenSource = new CancellationTokenSource();
			SharedSettings.Current.PropertyChanged += this.SharedSettings_PropertyChanged;
			this.Initialize(text);
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x002454F0 File Offset: 0x002436F0
		private async Task Initialize(string brand)
		{
			switch (App.OBDReader.CurrentELMFormat)
			{
			case ELMFormat.KWP:
				if (brand != null)
				{
					switch (brand.Length)
					{
					case 4:
					{
						char c = brand[0];
						if (c != 'A')
						{
							if (c != 'S')
							{
								goto IL_164D;
							}
							if (!(brand == "Seat"))
							{
								goto IL_164D;
							}
						}
						else if (!(brand == "Audi"))
						{
							goto IL_164D;
						}
						break;
					}
					case 5:
					{
						char c = brand[0];
						if (c != 'D')
						{
							if (c != 'J')
							{
								if (c != 'S')
								{
									goto IL_164D;
								}
								if (!(brand == "Skoda"))
								{
									goto IL_164D;
								}
							}
							else if (!(brand == "Jetta"))
							{
								goto IL_164D;
							}
						}
						else
						{
							if (!(brand == "Dacia"))
							{
								goto IL_164D;
							}
							goto IL_125B;
						}
						break;
					}
					case 6:
						if (!(brand == "Delphi"))
						{
							goto IL_164D;
						}
						this.ECUListFull = new ObservableCollection<IECU>(DelphiKWP_ECU.ECUs);
						goto IL_164D;
					case 7:
					{
						char c = brand[0];
						if (c != 'P')
						{
							if (c != 'R')
							{
								goto IL_164D;
							}
							if (!(brand == "Renault"))
							{
								goto IL_164D;
							}
							goto IL_125B;
						}
						else if (!(brand == "Porsche"))
						{
							goto IL_164D;
						}
						break;
					}
					case 8:
					case 9:
						goto IL_164D;
					case 10:
						if (!(brand == "Volkswagen"))
						{
							goto IL_164D;
						}
						break;
					default:
						goto IL_164D;
					}
					List<IECU> list = VAGECU.ECUs.ToList<IECU>();
					list.RemoveAll((IECU x) => x is OBD2Can11bitECU);
					list.Insert(0, new OBD2KWPECU());
					this.ECUListFull = new ObservableCollection<IECU>(list);
					break;
					IL_125B:
					List<IECU> list2 = new List<IECU>();
					list2.Add(new OBD2KWPECU());
					List<KWPECU> list3 = KWPECU.BuildDetectedECUs();
					list2.AddRange(list3);
					list2.AddRange(RenaultKWPECU.ECUs);
					List<IECU> list4 = RenaultDaciaCAN11bitECU.ECUs.ToList<IECU>();
					list4.RemoveAll((IECU x) => x is OBD2Can11bitECU);
					list2.AddRange(list4);
					this.ECUListFull = new ObservableCollection<IECU>(list2);
				}
				break;
			case ELMFormat.CAN11bit:
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
								goto IL_1089;
							}
							if (!(brand == "MG"))
							{
								goto IL_1089;
							}
							this.ECUListFull = new ObservableCollection<IECU>(MGZSEV.ECUs);
							goto IL_164D;
						}
						else
						{
							if (!(brand == "DS"))
							{
								goto IL_1089;
							}
							goto IL_0C22;
						}
						break;
					}
					case 3:
					{
						char c = brand[0];
						if (c > 'K')
						{
							switch (c)
							{
							case 'O':
								if (!(brand == "Ora"))
								{
									goto IL_1089;
								}
								goto IL_0F8D;
							case 'P':
							case 'Q':
							case 'S':
							case 'T':
								goto IL_1089;
							case 'R':
								if (!(brand == "RAM"))
								{
									goto IL_1089;
								}
								goto IL_0C4C;
							case 'U':
								if (!(brand == "UAZ"))
								{
									goto IL_1089;
								}
								break;
							case 'V':
								if (!(brand == "VAZ"))
								{
									goto IL_1089;
								}
								goto IL_0C61;
							case 'W':
								if (!(brand == "WEY"))
								{
									goto IL_1089;
								}
								goto IL_0F8D;
							default:
								if (c != 'В')
								{
									if (c != 'У')
									{
										goto IL_1089;
									}
									if (!(brand == "УАЗ"))
									{
										goto IL_1089;
									}
								}
								else
								{
									if (!(brand == "ВАЗ"))
									{
										goto IL_1089;
									}
									goto IL_0C61;
								}
								break;
							}
							this.ECUListFull = new ObservableCollection<IECU>(UAZCAN11bit.ECUs);
							goto IL_164D;
						}
						if (c != 'B')
						{
							switch (c)
							{
							case 'F':
								if (!(brand == "FAW"))
								{
									goto IL_1089;
								}
								this.ECUListFull = new ObservableCollection<IECU>(FAWCANECU.ECUs);
								goto IL_164D;
							case 'G':
								if (brand == "GMC")
								{
									goto IL_0F4E;
								}
								if (!(brand == "GAC"))
								{
									goto IL_1089;
								}
								goto IL_1035;
							case 'H':
							case 'I':
								goto IL_1089;
							case 'J':
								if (!(brand == "JAC"))
								{
									goto IL_1089;
								}
								goto IL_0FE1;
							case 'K':
								if (brand == "Kia")
								{
									goto IL_0BE3;
								}
								if (!(brand == "KTM"))
								{
									goto IL_1089;
								}
								this.ECUListFull = new ObservableCollection<IECU>(KTMCANECU.ECUs);
								goto IL_164D;
							default:
								goto IL_1089;
							}
						}
						else
						{
							if (brand == "BMW")
							{
								goto IL_0C0D;
							}
							if (!(brand == "BYD") && !(brand == "Byd"))
							{
								goto IL_1089;
							}
							this.ECUListFull = new ObservableCollection<IECU>(BydCANECU.ECUs);
							goto IL_164D;
						}
						break;
					}
					case 4:
					{
						char c = brand[3];
						if (c <= 'i')
						{
							if (c <= 'K')
							{
								if (c != 'G')
								{
									if (c != 'K')
									{
										goto IL_1089;
									}
									if (!(brand == "DFSK"))
									{
										goto IL_1089;
									}
									goto IL_104A;
								}
								else
								{
									if (!(brand == "DFFG"))
									{
										goto IL_1089;
									}
									goto IL_104A;
								}
							}
							else
							{
								switch (c)
								{
								case 'a':
									if (!(brand == "Lada"))
									{
										goto IL_1089;
									}
									goto IL_0C61;
								case 'b':
									if (!(brand == "Saab"))
									{
										goto IL_1089;
									}
									goto IL_0F4E;
								case 'c':
									goto IL_1089;
								case 'd':
									if (!(brand == "Ford"))
									{
										goto IL_1089;
									}
									goto IL_0C37;
								default:
									if (c != 'i')
									{
										goto IL_1089;
									}
									if (!(brand == "Audi"))
									{
										if (!(brand == "Mini"))
										{
											goto IL_1089;
										}
										goto IL_0C0D;
									}
									break;
								}
							}
						}
						else if (c <= 'p')
						{
							if (c != 'l')
							{
								if (c != 'p')
								{
									goto IL_1089;
								}
								if (!(brand == "Jeep"))
								{
									goto IL_1089;
								}
								goto IL_0C4C;
							}
							else
							{
								if (!(brand == "Opel"))
								{
									goto IL_1089;
								}
								goto IL_0F4E;
							}
						}
						else if (c != 't')
						{
							if (c != 'а')
							{
								goto IL_1089;
							}
							if (!(brand == "Лада"))
							{
								goto IL_1089;
							}
							goto IL_0C61;
						}
						else if (!(brand == "Seat"))
						{
							goto IL_1089;
						}
						break;
					}
					case 5:
					{
						char c = brand[2];
						if (c != 'U')
						{
							switch (c)
							{
							case 'c':
								if (!(brand == "Dacia"))
								{
									goto IL_1089;
								}
								goto IL_0C8B;
							case 'd':
								if (!(brand == "Dodge"))
								{
									goto IL_1089;
								}
								goto IL_0C4C;
							case 'e':
								if (brand == "Chery" || brand == "Exeed")
								{
									goto IL_0F0F;
								}
								if (brand == "Geely")
								{
									this.ECUListFull = new ObservableCollection<IECU>(GeelyCANECU.ECUs);
									goto IL_164D;
								}
								if (!(brand == "Chevy"))
								{
									goto IL_1089;
								}
								goto IL_0F4E;
							case 'f':
								if (!(brand == "Lifan"))
								{
									goto IL_1089;
								}
								this.ECUListFull = new ObservableCollection<IECU>(LifanECUCAN11bit.ECUs);
								goto IL_164D;
							case 'g':
							case 'j':
							case 'k':
							case 'm':
							case 'n':
							case 'q':
							case 'r':
							case 's':
							case 'w':
							case 'y':
								goto IL_1089;
							case 'h':
								if (!(brand == "Sehol"))
								{
									goto IL_1089;
								}
								goto IL_0FE1;
							case 'i':
								if (brand == "Kaiyi")
								{
									goto IL_0F0F;
								}
								if (!(brand == "Buick"))
								{
									goto IL_1089;
								}
								goto IL_0F4E;
							case 'l':
								if (brand == "Volvo")
								{
									string profileUpdateAlias = SharedSettings.Current.ProfileUpdateAlias;
									if (profileUpdateAlias != null)
									{
										int length = profileUpdateAlias.Length;
										if (length <= 11)
										{
											if (length != 8)
											{
												if (length != 11)
												{
													goto IL_0EB1;
												}
												if (!(profileUpdateAlias == "Volvo 2010+"))
												{
													goto IL_0EB1;
												}
											}
											else if (!(profileUpdateAlias == "D2 (1.6)"))
											{
												goto IL_0EB1;
											}
										}
										else
										{
											if (length == 32)
											{
												c = profileUpdateAlias[5];
												if (c <= '6')
												{
													if (c != ' ')
													{
														switch (c)
														{
														case '0':
															if (!(profileUpdateAlias == "7747e05956024118aaba6db5b89100fa"))
															{
																goto IL_0EB1;
															}
															goto IL_0E87;
														case '1':
															if (!(profileUpdateAlias == "7a29617cc40446609b8396baf0f3af53"))
															{
																goto IL_0EB1;
															}
															goto IL_0E87;
														case '2':
															if (!(profileUpdateAlias == "d039520dffb647318b17a78daffbea5d"))
															{
																goto IL_0EB1;
															}
															break;
														case '3':
															if (!(profileUpdateAlias == "4b6d63d2e23f438eb11a6f5ff7267a93") && !(profileUpdateAlias == "83ffc37f09394359aba236bc11854e14"))
															{
																goto IL_0EB1;
															}
															goto IL_0E87;
														case '4':
															if (!(profileUpdateAlias == "ead764bfaeb7421fa210abc9f19a59fc"))
															{
																goto IL_0EB1;
															}
															break;
														case '5':
															goto IL_0EB1;
														case '6':
															if (!(profileUpdateAlias == "27fbf62a5f9845e791a413f387e060d3"))
															{
																goto IL_0EB1;
															}
															break;
														default:
															goto IL_0EB1;
														}
													}
													else
													{
														if (!(profileUpdateAlias == "Volvo 2014+ VEA engine (not SPA)"))
														{
															goto IL_0EB1;
														}
														goto IL_0E87;
													}
												}
												else if (c != 'd')
												{
													if (c != 'f')
													{
														goto IL_0EB1;
													}
													if (!(profileUpdateAlias == "6c80ff6e03e846a7a16922fc7a4a0359"))
													{
														goto IL_0EB1;
													}
												}
												else if (!(profileUpdateAlias == "76d64d01184541249adb0da64f0d421a"))
												{
													goto IL_0EB1;
												}
												this.ECUListFull = new ObservableCollection<IECU>(VolvoSPA29bitECU.ECUs);
												goto IL_164D;
											}
											if (length != 35)
											{
												goto IL_0EB1;
											}
											if (!(profileUpdateAlias == "Volvo S80-V70 with 2.0D Ford engine"))
											{
												goto IL_0EB1;
											}
										}
										IL_0E87:
										this.ECUListFull = new ObservableCollection<IECU>(VolvoCAN11bitECU.ECUs);
										goto IL_164D;
									}
									IL_0EB1:
									this.ECUListFull = new ObservableCollection<IECU>(VolvoCAN11bitECU.ECUs.Concat(VolvoSPA29bitECU.ECUs));
									goto IL_164D;
								}
								if (!(brand == "Euler"))
								{
									goto IL_1089;
								}
								goto IL_0F8D;
							case 'o':
								if (brand == "Skoda")
								{
									goto IL_0B99;
								}
								if (!(brand == "Omoda"))
								{
									goto IL_1089;
								}
								goto IL_0F0F;
							case 'p':
								if (!(brand == "Cupra"))
								{
									goto IL_1089;
								}
								goto IL_0B99;
							case 't':
								if (!(brand == "Jetta"))
								{
									goto IL_1089;
								}
								goto IL_0B99;
							case 'u':
								if (!(brand == "Isuzu"))
								{
									goto IL_1089;
								}
								break;
							case 'v':
								if (brand == "Ravon")
								{
									goto IL_0F4E;
								}
								if (!(brand == "Haval") && !(brand == "Hover"))
								{
									goto IL_1089;
								}
								goto IL_0F8D;
							case 'x':
								if (!(brand == "Lexus"))
								{
									goto IL_1089;
								}
								goto IL_0BF8;
							case 'z':
								if (!(brand == "Mazda"))
								{
									goto IL_1089;
								}
								this.ECUListFull = new ObservableCollection<IECU>(MazdaECUCAN11bit.ECUs);
								goto IL_164D;
							default:
								goto IL_1089;
							}
						}
						else if (!(brand == "ISUZU"))
						{
							goto IL_1089;
						}
						this.ECUListFull = new ObservableCollection<IECU>(ISUZUCAN11bitECU.ECUs);
						goto IL_164D;
					}
					case 6:
					{
						char c = brand[2];
						if (c <= 'g')
						{
							if (c != 'b')
							{
								if (c != 'e')
								{
									if (c != 'g')
									{
										goto IL_1089;
									}
									if (!(brand == "Jaguar"))
									{
										goto IL_1089;
									}
									goto IL_0EE5;
								}
								else
								{
									if (!(brand == "Daewoo"))
									{
										goto IL_1089;
									}
									goto IL_0F4E;
								}
							}
							else
							{
								if (!(brand == "Subaru"))
								{
									goto IL_1089;
								}
								this.ECUListFull = new ObservableCollection<IECU>(SubaruCANECU.ECUs);
								goto IL_164D;
							}
						}
						else
						{
							switch (c)
							{
							case 'l':
								if (!(brand == "Holden"))
								{
									goto IL_1089;
								}
								goto IL_0F4E;
							case 'm':
								if (brand == "Hummer")
								{
									goto IL_0F4E;
								}
								if (!(brand == "Yamaha"))
								{
									goto IL_1089;
								}
								this.ECUListFull = new ObservableCollection<IECU>(YamahaCANECU.ECUs);
								goto IL_164D;
							case 'n':
								if (!(brand == "Hongqi"))
								{
									goto IL_1089;
								}
								this.ECUListFull = new ObservableCollection<IECU>(HongqiCANECU.ECUs);
								goto IL_164D;
							case 'o':
							case 'p':
							case 'q':
							case 'r':
								goto IL_1089;
							case 's':
								if (!(brand == "Nissan"))
								{
									goto IL_1089;
								}
								goto IL_0BB9;
							case 't':
								if (brand == "Jetour")
								{
									goto IL_0F0F;
								}
								if (!(brand == "Saturn"))
								{
									goto IL_1089;
								}
								goto IL_0F4E;
							default:
								if (c != 'y')
								{
									if (c != 'z')
									{
										goto IL_1089;
									}
									if (!(brand == "Suzuki"))
									{
										goto IL_1089;
									}
									this.ECUListFull = new ObservableCollection<IECU>(SuzukiCAN.ECUs);
									goto IL_164D;
								}
								else
								{
									if (!(brand == "Toyota"))
									{
										goto IL_1089;
									}
									goto IL_0BF8;
								}
								break;
							}
						}
						break;
					}
					case 7:
					{
						char c = brand[4];
						switch (c)
						{
						case 'c':
							if (!(brand == "Porsche"))
							{
								goto IL_1089;
							}
							break;
						case 'd':
							if (!(brand == "Hyundai"))
							{
								goto IL_1089;
							}
							goto IL_0BE3;
						case 'e':
							if (!(brand == "Peugeot"))
							{
								goto IL_1089;
							}
							goto IL_0C22;
						case 'f':
						case 'h':
						case 'j':
						case 'k':
						case 'm':
						case 'n':
						case 'p':
						case 'q':
						case 'r':
							goto IL_1089;
						case 'g':
							if (!(brand == "Changan"))
							{
								goto IL_1089;
							}
							this.ECUListFull = new ObservableCollection<IECU>(ChanganCANECU.ECUs);
							goto IL_164D;
						case 'i':
							if (!(brand == "Pontiac"))
							{
								goto IL_1089;
							}
							goto IL_0F4E;
						case 'l':
							if (!(brand == "Bentley"))
							{
								goto IL_1089;
							}
							break;
						case 'o':
							if (brand == "Citroen")
							{
								goto IL_0C22;
							}
							if (!(brand == "Lincoln"))
							{
								goto IL_1089;
							}
							goto IL_0C37;
						case 's':
							if (!(brand == "Genesis"))
							{
								goto IL_1089;
							}
							goto IL_0BE3;
						case 't':
							if (!(brand == "Bugatti"))
							{
								goto IL_1089;
							}
							break;
						case 'u':
							if (brand == "Mercury")
							{
								goto IL_0C37;
							}
							if (brand == "Renault")
							{
								goto IL_0C8B;
							}
							if (!(brand == "Evolute"))
							{
								goto IL_1089;
							}
							goto IL_104A;
						default:
							if (c != 'в')
							{
								goto IL_1089;
							}
							if (!(brand == "Москвич"))
							{
								goto IL_1089;
							}
							goto IL_0FE1;
						}
						break;
					}
					case 8:
					{
						char c = brand[4];
						if (c <= 'h')
						{
							if (c != 'f')
							{
								if (c != 'h')
								{
									goto IL_1089;
								}
								if (!(brand == "Vauxhall"))
								{
									goto IL_1089;
								}
								goto IL_0F4E;
							}
							else
							{
								if (!(brand == "Dongfeng"))
								{
									goto IL_1089;
								}
								goto IL_104A;
							}
						}
						else
						{
							switch (c)
							{
							case 'l':
								if (!(brand == "Cadillac"))
								{
									goto IL_1089;
								}
								goto IL_0F4E;
							case 'm':
							case 'o':
								goto IL_1089;
							case 'n':
								if (!(brand == "Infiniti") && !(brand == "Infinity"))
								{
									goto IL_1089;
								}
								goto IL_0BB9;
							case 'p':
								if (!(brand == "Trumpchi"))
								{
									goto IL_1089;
								}
								goto IL_1035;
							default:
								if (c != 's')
								{
									goto IL_1089;
								}
								if (!(brand == "Chrysler"))
								{
									goto IL_1089;
								}
								goto IL_0C4C;
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
								goto IL_1089;
							}
							if (!(brand == "SsangYong"))
							{
								goto IL_1089;
							}
							this.ECUListFull = new ObservableCollection<IECU>(SsangYongCANECU.ECUs);
							goto IL_164D;
						}
						else
						{
							if (!(brand == "Chevrolet"))
							{
								goto IL_1089;
							}
							goto IL_0F4E;
						}
						break;
					}
					case 10:
					{
						char c = brand[0];
						if (c <= 'L')
						{
							if (c != 'G')
							{
								if (c != 'L')
								{
									goto IL_1089;
								}
								if (!(brand == "Land Rover"))
								{
									goto IL_1089;
								}
								goto IL_0EE5;
							}
							else
							{
								if (!(brand == "Great Wall"))
								{
									goto IL_1089;
								}
								goto IL_0F8D;
							}
						}
						else if (c != 'M')
						{
							if (c != 'V')
							{
								goto IL_1089;
							}
							if (!(brand == "Volkswagen"))
							{
								goto IL_1089;
							}
						}
						else
						{
							if (!(brand == "Mitsubishi"))
							{
								goto IL_1089;
							}
							this.ECUListFull = new ObservableCollection<IECU>(MitsubishiCANECU.ECUs);
							goto IL_164D;
						}
						break;
					}
					case 11:
						if (!(brand == "Range Rover"))
						{
							goto IL_1089;
						}
						goto IL_0EE5;
					case 12:
						goto IL_1089;
					case 13:
						if (!(brand == "Mercedes-Benz"))
						{
							goto IL_1089;
						}
						this.ECUListFull = new ObservableCollection<IECU>(MercedesBenzCANECU.ECUs);
						goto IL_164D;
					case 14:
						if (!(brand == "General Motors"))
						{
							goto IL_1089;
						}
						goto IL_0F4E;
					default:
						goto IL_1089;
					}
					IL_0B99:
					this.ECUListFull = new ObservableCollection<IECU>(VAGECU.ECUs);
					this.SupportedUnitsDetector = new VagUDSSupportedUnitsDetector();
					break;
					IL_0BB9:
					this.ECUListFull = new ObservableCollection<IECU>(NissanCANECU.ECUs);
					break;
					IL_0BE3:
					this.ECUListFull = new ObservableCollection<IECU>(HyundaiKiaCANECU.ECUs);
					break;
					IL_0BF8:
					this.ECUListFull = new ObservableCollection<IECU>(ToyotaCANECU.ECUs);
					break;
					IL_0C0D:
					this.ECUListFull = new ObservableCollection<IECU>(BMWGatewayCANECU.ECUs);
					break;
					IL_0C22:
					this.ECUListFull = new ObservableCollection<IECU>(CitroenPeugeotCAN11bit.ECUs);
					break;
					IL_0C37:
					this.ECUListFull = new ObservableCollection<IECU>(FordCANECU.ECUs);
					break;
					IL_0C4C:
					this.ECUListFull = new ObservableCollection<IECU>(JeepChryslerDodgeCANECU.ECUs);
					break;
					IL_0C61:
					this.ECUListFull = new ObservableCollection<IECU>(LadaECUCAN.ECUs);
					break;
					IL_0C8B:
					List<IECU> list5 = Renault29bitCANECU.ECUs.ToList<IECU>();
					IReadOnlyList<IECU> ecus = RenaultDaciaCAN11bitECU.ECUs;
					list5.RemoveAll((IECU x) => x is OBD2Can29bitECU);
					this.ECUListFull = new ObservableCollection<IECU>(ecus.Concat(list5));
					break;
					IL_0EE5:
					this.ECUListFull = new ObservableCollection<IECU>(LandRoverECUCAN11bit.ECUs);
					break;
					IL_0F0F:
					this.ECUListFull = new ObservableCollection<IECU>(CheryExeedECU.ECUs);
					break;
					IL_0F4E:
					this.ECUListFull = new ObservableCollection<IECU>(GMCANECU.ECUs);
					break;
					IL_0F8D:
					this.ECUListFull = new ObservableCollection<IECU>(HavalCANECU.ECUs);
					break;
					IL_0FE1:
					this.ECUListFull = new ObservableCollection<IECU>(JACCAN11bitECU.ECUs);
					break;
					IL_1035:
					this.ECUListFull = new ObservableCollection<IECU>(GACCAN11bitECU.ECUs);
					break;
					IL_104A:
					this.ECUListFull = new ObservableCollection<IECU>(DongFengCAN11bitECU.ECUs);
					break;
				}
				IL_1089:
				this.ECUListFull = new ObservableCollection<IECU>(GenericCAN11bitBrand.ECUs);
				break;
			case ELMFormat.CAN29bit:
				if (brand != null)
				{
					switch (brand.Length)
					{
					case 4:
					{
						char c = brand[0];
						if (c <= 'F')
						{
							if (c != 'A')
							{
								if (c != 'F')
								{
									goto IL_164D;
								}
								if (!(brand == "Fiat"))
								{
									goto IL_164D;
								}
								goto IL_1588;
							}
							else if (!(brand == "Audi"))
							{
								goto IL_164D;
							}
						}
						else if (c != 'J')
						{
							if (c != 'S')
							{
								goto IL_164D;
							}
							if (!(brand == "Seat"))
							{
								goto IL_164D;
							}
						}
						else
						{
							if (!(brand == "Jeep"))
							{
								goto IL_164D;
							}
							goto IL_1588;
						}
						break;
					}
					case 5:
					{
						char c = brand[2];
						switch (c)
						{
						case 'c':
							if (!(brand == "Dacia"))
							{
								goto IL_164D;
							}
							goto IL_159D;
						case 'd':
							if (!(brand == "Dodge"))
							{
								goto IL_164D;
							}
							goto IL_1588;
						case 'e':
							if (!(brand == "Geely"))
							{
								goto IL_164D;
							}
							goto IL_163D;
						default:
							switch (c)
							{
							case 'l':
								if (!(brand == "Volvo"))
								{
									goto IL_164D;
								}
								goto IL_163D;
							case 'm':
							case 'q':
							case 'r':
							case 's':
								goto IL_164D;
							case 'n':
								if (!(brand == "Honda"))
								{
									goto IL_164D;
								}
								break;
							case 'o':
								if (!(brand == "Skoda"))
								{
									goto IL_164D;
								}
								goto IL_1568;
							case 'p':
								if (!(brand == "Cupra"))
								{
									goto IL_164D;
								}
								goto IL_1568;
							case 't':
								if (!(brand == "Jetta"))
								{
									goto IL_164D;
								}
								goto IL_1568;
							case 'u':
								if (!(brand == "Acura"))
								{
									goto IL_164D;
								}
								break;
							default:
								goto IL_164D;
							}
							this.ECUListFull = new ObservableCollection<IECU>(HondaECU29bit.ECUs);
							goto IL_164D;
						}
						break;
					}
					case 6:
					{
						if (!(brand == "Nissan"))
						{
							goto IL_164D;
						}
						IReadOnlyList<IECU> ecus2 = Renault29bitCANECU.ECUs;
						List<IECU> list6 = NissanCANECU.ECUs.ToList<IECU>();
						list6.RemoveAll((IECU x) => x is OBD2Can11bitECU);
						this.ECUListFull = new ObservableCollection<IECU>(ecus2.Concat(list6));
						goto IL_164D;
					}
					case 7:
					{
						char c = brand[0];
						if (c != 'P')
						{
							if (c != 'R')
							{
								goto IL_164D;
							}
							if (!(brand == "Renault"))
							{
								goto IL_164D;
							}
							goto IL_159D;
						}
						else if (!(brand == "Porsche"))
						{
							goto IL_164D;
						}
						break;
					}
					case 8:
						goto IL_164D;
					case 9:
						if (!(brand == "Lynk & Co"))
						{
							goto IL_164D;
						}
						goto IL_163D;
					case 10:
						if (!(brand == "Volkswagen"))
						{
							goto IL_164D;
						}
						break;
					default:
						goto IL_164D;
					}
					IL_1568:
					this.ECUListFull = new ObservableCollection<IECU>(VAGECU.ECUs);
					this.SupportedUnitsDetector = new VagUDSSupportedUnitsDetector();
					break;
					IL_1588:
					this.ECUListFull = new ObservableCollection<IECU>(FiatCAN29bitECU.ECUs);
					break;
					IL_159D:
					IReadOnlyList<IECU> ecus3 = Renault29bitCANECU.ECUs;
					List<IECU> list7 = RenaultDaciaCAN11bitECU.ECUs.ToList<IECU>();
					list7.RemoveAll((IECU x) => x is OBD2Can11bitECU);
					this.ECUListFull = new ObservableCollection<IECU>(ecus3.Concat(list7));
					break;
					IL_163D:
					this.ECUListFull = new ObservableCollection<IECU>(VolvoSPA29bitECU.ECUs);
				}
				break;
			}
			IL_164D:
			if (this.ECUListFull == null)
			{
				this.ECUListFull = new ObservableCollection<IECU>();
				if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
				{
					this.ECUListFull.Add(new OBD2KWPECU());
					foreach (KWPECU kwpecu in KWPECU.BuildDetectedECUs())
					{
						this.ECUListFull.Add(kwpecu);
					}
					string[] array = new string[0];
					switch (App.OBDReader.CurrentProtocolNumber)
					{
					case 1:
						array = new string[] { "6158F1", "616AF1", "6110F1", "6118F1", "616AF1" };
						break;
					case 2:
						array = new string[] { "6C10F1", "6C58F1", "6C40F1", "6C1AF1", "6818F1", "6810F1", "6858F1", "6828F1", "686AF1" };
						break;
					case 3:
						array = new string[] { "6828F1", "6858F1", "6818F1", "686AF1", "6810F1", "6812F1", "687AF1" };
						break;
					case 4:
					case 5:
						array = new string[]
						{
							"8110F1", "8111F1", "8112F1", "8113F1", "8114F1", "8658F1", "C241F1", "C218F1", "C230F1", "C258F1",
							"C228F1", "C233F1", "8116F1", "8118F1", "811AF1", "807AF1"
						};
						break;
					}
					List<string> readDTCCommands = new List<string>
					{
						"03", "03", "07", "07", "0A", "1800FF00", "1802FF00", "1802FFFF", "1800FFFF", "18FF00",
						"17FF00", "13FF00", "1902AF", "1902AC", "19028D", "190223", "190278", "190208", "190FAC", "190F8D",
						"190F23", "19D2FF00"
					};
					List<string> clearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFFFF", "140000" };
					using (List<KWPECU>.Enumerator enumerator = array.Select((string header) => new KWPECU
					{
						Name = "ECU Address $" + header.Substring(2, 2),
						RequestHeader = header,
						ReadDTCCommands = readDTCCommands,
						ClearDTCCommands = clearDTCCommands,
						TestIfEcuExists = false
					}).ToList<KWPECU>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KWPECU kwpecu2 = enumerator.Current;
							string ecuAddress = kwpecu2.RequestHeader.Substring(2, 2);
							if (!this.ECUListFull.Any((IECU x) => x.RequestHeader != null && x.RequestHeader.Length == 6 && x.RequestHeader.Substring(2, 2) == ecuAddress))
							{
								this.ECUListFull.Add(kwpecu2);
							}
						}
						goto IL_1A74;
					}
				}
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					this.ECUListFull.Add(new OBD2Can11bitECU());
				}
				else if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
				{
					this.ECUListFull.Add(new OBD2Can29bitECU());
				}
			}
			IL_1A74:
			await this.DetectSupportedUnits();
			string dtcfoundECUs = SharedSettings.Current.DTCFoundECUs;
			foreach (IECU iecu in this.ECUListFull)
			{
				iecu.IsSelected = false;
			}
			if (string.IsNullOrEmpty(dtcfoundECUs) || dtcfoundECUs == "[]")
			{
				using (IEnumerator<IECU> enumerator2 = this.ECUListToDisplay.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						IECU iecu2 = enumerator2.Current;
						iecu2.IsSelected = true;
					}
					return;
				}
			}
			try
			{
				int[] array2 = JsonConvert.DeserializeObject<int[]>(dtcfoundECUs);
				foreach (IECU iecu3 in this.ECUListToDisplay)
				{
					if (array2.Contains(iecu3.GetHashCode()))
					{
						iecu3.IsSelected = true;
					}
					else
					{
						iecu3.IsSelected = false;
					}
				}
			}
			catch (Exception)
			{
				IEnumerator<IECU> enumerator2 = this.ECUListToDisplay.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						IECU iecu4 = enumerator2.Current;
						iecu4.IsSelected = true;
					}
				}
				finally
				{
					int num;
					if (num < 0 && enumerator2 != null)
					{
						enumerator2.Dispose();
					}
				}
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x0024553C File Offset: 0x0024373C
		public async Task DetectSupportedUnits()
		{
			this.IsBusy = true;
			try
			{
				List<IECU> list = await this.SupportedUnitsDetector.DetectSupported(this.ECUListFull);
				this.ECUListDetected.Clear();
				foreach (IECU iecu in list)
				{
					this.ECUListDetected.Add(iecu);
				}
			}
			catch (Exception)
			{
				this.ECUListDetected = this.ECUListFull;
			}
			finally
			{
				this.IsBusy = false;
			}
			if (this.ECUListDetected.Count > 1)
			{
				this.DisplayDetected = true;
			}
			else
			{
				this.DisplayDetected = false;
			}
			this.OnPropertyChanged("ECUListToDisplay");
			this.OnPropertyChanged("DisplayAllOrDetectedSelector");
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x0024557F File Offset: 0x0024377F
		private void SharedSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "HideArchiveDTC" || e.PropertyName == "HideDTCWithUncomplitedTests")
			{
				this.UpdateCollections();
			}
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x002455AC File Offset: 0x002437AC
		private void UpdateCollections()
		{
			foreach (IECU iecu in this.ECUListFull)
			{
				iecu.UpdateCollection();
			}
			IECU[] array = this.ECUListFull.Where((IECU x) => x.Count<DTCItemV2>() > 0).ToArray<IECU>();
			this.ECUListFound.Clear();
			foreach (IECU iecu2 in array)
			{
				this.ECUListFound.Add(iecu2);
			}
			if (this.ECUListFound.Count == 0)
			{
				this.HasDTCToDisplay = false;
				return;
			}
			this.HasDTCToDisplay = true;
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060033F5 RID: 13301 RVA: 0x0024566C File Offset: 0x0024386C
		// (remove) Token: 0x060033F6 RID: 13302 RVA: 0x002456A4 File Offset: 0x002438A4
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x002456D9 File Offset: 0x002438D9
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x060033F8 RID: 13304 RVA: 0x002456F2 File Offset: 0x002438F2
		// (set) Token: 0x060033F9 RID: 13305 RVA: 0x002456FA File Offset: 0x002438FA
		public bool IsSetup
		{
			get
			{
				return this._IsSetup;
			}
			private set
			{
				this._IsSetup = value;
				this.OnPropertyChanged("IsSetup");
			}
		}

		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x060033FA RID: 13306 RVA: 0x0024570E File Offset: 0x0024390E
		// (set) Token: 0x060033FB RID: 13307 RVA: 0x00245716 File Offset: 0x00243916
		public bool IsResults
		{
			get
			{
				return this._IsResults;
			}
			private set
			{
				if (value && !this._IsResults)
				{
					this.IsCancelButtonEnabled = true;
				}
				this._IsResults = value;
				this.OnPropertyChanged("IsResults");
			}
		}

		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x060033FC RID: 13308 RVA: 0x0024573C File Offset: 0x0024393C
		// (set) Token: 0x060033FD RID: 13309 RVA: 0x00245744 File Offset: 0x00243944
		public bool IsCancelButtonEnabled
		{
			get
			{
				return this._IsCancelButtonEnabled;
			}
			set
			{
				this._IsCancelButtonEnabled = value;
				this.OnPropertyChanged("IsCancelButtonEnabled");
			}
		}

		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x060033FE RID: 13310 RVA: 0x00245758 File Offset: 0x00243958
		// (set) Token: 0x060033FF RID: 13311 RVA: 0x00245760 File Offset: 0x00243960
		public ObservableCollection<IECU> ECUListFull
		{
			get
			{
				return this._ECUListFull;
			}
			private set
			{
				this._ECUListFull = value;
				this.OnPropertyChanged("ECUListFull");
				ObservableCollection<IECU> eculistDetected = this.ECUListDetected;
				if (eculistDetected != null)
				{
					eculistDetected.Clear();
				}
				this.OnPropertyChanged("ECUListDetected");
				this.OnPropertyChanged("ECUListToDisplay");
			}
		}

		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06003400 RID: 13312 RVA: 0x0024579B File Offset: 0x0024399B
		// (set) Token: 0x06003401 RID: 13313 RVA: 0x002457A3 File Offset: 0x002439A3
		public ObservableCollection<IECU> ECUListDetected
		{
			[CompilerGenerated]
			get
			{
				return this.<ECUListDetected>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ECUListDetected>k__BackingField = value;
			}
		} = new ObservableCollection<IECU>();

		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06003402 RID: 13314 RVA: 0x002457AC File Offset: 0x002439AC
		// (set) Token: 0x06003403 RID: 13315 RVA: 0x002457B4 File Offset: 0x002439B4
		public ObservableCollection<IECU> ECUListFound
		{
			[CompilerGenerated]
			get
			{
				return this.<ECUListFound>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ECUListFound>k__BackingField = value;
			}
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06003404 RID: 13316 RVA: 0x002457BD File Offset: 0x002439BD
		public ObservableCollection<IECU> ECUListToDisplay
		{
			get
			{
				if (this.SupportedUnitsDetector == null || this.SupportedUnitsDetector is BaseSupportedUnitsDetector)
				{
					return this.ECUListFull;
				}
				if (this.DisplayDetected)
				{
					return this.ECUListDetected;
				}
				return this.ECUListFull;
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x002457F0 File Offset: 0x002439F0
		public bool DisplayAllOrDetectedSelector
		{
			get
			{
				return this.SupportedUnitsDetector != null && !(this.SupportedUnitsDetector is BaseSupportedUnitsDetector) && this.ECUListDetected != this.ECUListFull && this.ECUListDetected.Count >= 2;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x06003406 RID: 13318 RVA: 0x00245828 File Offset: 0x00243A28
		// (set) Token: 0x06003407 RID: 13319 RVA: 0x00245830 File Offset: 0x00243A30
		public bool DisplayDetected
		{
			get
			{
				return this._DisplayDetected;
			}
			set
			{
				if (this._DisplayDetected != value)
				{
					this._DisplayDetected = value;
					this.OnPropertyChanged("DisplayDetected");
					this.OnPropertyChanged("ECUListToDisplay");
				}
			}
		}

		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06003408 RID: 13320 RVA: 0x00245858 File Offset: 0x00243A58
		public Command ShareCommand
		{
			get
			{
				return new Command(async delegate
				{
					this.ECUListFound.SelectMany((IECU x) => x.DTCCollection).ToList<DTCItemV2>();
					string text = ReportGenerator.CreateReport(this.ECUListFull.Where((IECU x) => x.IsSelected).ToList<IECU>());
					try
					{
						await Share.RequestAsync(text);
					}
					catch (Exception)
					{
					}
				});
			}
		}

		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x06003409 RID: 13321 RVA: 0x0024586B File Offset: 0x00243A6B
		// (set) Token: 0x0600340A RID: 13322 RVA: 0x00245873 File Offset: 0x00243A73
		public Command ReadDTCCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<ReadDTCCommand>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ReadDTCCommand>k__BackingField = value;
			}
		}

		// Token: 0x17001335 RID: 4917
		// (get) Token: 0x0600340B RID: 13323 RVA: 0x0024587C File Offset: 0x00243A7C
		public Command ClearDTCCommand
		{
			get
			{
				return new Command(async delegate
				{
					if (this.IsResults)
					{
						if (this.HasDTCToDisplay)
						{
							action_clear_found = Translate.GetString("dtc_ClearFoundDTC");
							action_clear_full = Translate.GetString("dtc_ClearFullMode");
							string @string = Translate.GetString("dtc_ClearFoundsQuestion");
							cancel = Translate.GetString("btnCancel.Content");
							mass_clear = Translate.GetString("codingDB_DtcClearInMostOfUnits_Name");
							string[] array = new string[] { action_clear_found, action_clear_full };
							string selectedBrand = SharedSettings.Current.SelectedBrand;
							if ((selectedBrand == "Audi" || selectedBrand == "Seat" || selectedBrand == "Skoda" || selectedBrand == "Volkswagen" || selectedBrand == "Cupra") && !VAGECU.HasVWTP20Support)
							{
								array = new string[] { action_clear_found, action_clear_full, mass_clear };
							}
							Page currentPage = App.GetCurrentPage();
							if (currentPage is IDTCvXPage)
							{
								string text = await currentPage.DisplayActionSheetCustom(@string, cancel, null, array);
								if (!(text == cancel))
								{
									if (text == action_clear_full)
									{
										this.IsSetup = true;
										this.IsResults = false;
										this.HasDTCToDisplay = true;
									}
									else if (text == action_clear_found)
									{
										TaskAwaiter<bool> taskAwaiter3 = this.AskForDTCClearing().GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											await taskAwaiter3;
											taskAwaiter3 = taskAwaiter2;
											taskAwaiter2 = default(TaskAwaiter<bool>);
										}
										if (!taskAwaiter3.GetResult())
										{
											return;
										}
										List<IECU> list = this.ECUListFound.ToList<IECU>();
										foreach (IECU iecu in list)
										{
											iecu.IsSelected = true;
										}
										await this.ClearDTC(list);
									}
									else if (text == mass_clear)
									{
										await this.VagMassClear();
									}
									action_clear_found = null;
									action_clear_full = null;
									cancel = null;
									mass_clear = null;
								}
							}
						}
						else
						{
							this.IsSetup = true;
							this.IsResults = false;
							this.HasDTCToDisplay = true;
						}
					}
					else if (this.IsSetup)
					{
						Page currentPage2 = App.GetCurrentPage();
						if (currentPage2 is IDTCvXPage)
						{
							if (!this.ECUListFull.Any((IECU x) => x.IsSelected))
							{
								try
								{
									await currentPage2.DisplayAlert(Translate.GetString("dtc_NoECUSelected_Title"), Translate.GetString("dtc_NoECUSelected_Text"), "OK");
								}
								catch (Exception)
								{
								}
							}
							else
							{
								TaskAwaiter<bool> taskAwaiter3 = this.AskForDTCClearing().GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									await taskAwaiter3;
									taskAwaiter3 = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<bool>);
								}
								if (taskAwaiter3.GetResult())
								{
									await this.ClearDTC(null);
								}
							}
						}
					}
				});
			}
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x00245890 File Offset: 0x00243A90
		private async Task VagMassClear()
		{
			if (!this.IsBusy)
			{
				this.IsResults = true;
				this.IsSetup = false;
				this.BadELMDetected = false;
				this.ECUListFound.Clear();
				await App.OBDReader.DebugWrite("\r\n[DTCv2VagMassClear]\r\n");
				await this.CheckAndRestoreECUConnection();
				this.IsBusy = true;
				this.IsCancelButtonEnabled = true;
				this.cancellationTokenSource = new CancellationTokenSource();
				CancellationToken token = this.cancellationTokenSource.Token;
				foreach (IECU iecu in this.ECUListFull)
				{
					iecu.Reset();
				}
				OBDRequest obdrequest = new OBDRequest("3E80", "700", "", "", false);
				OBDRequest obdrequest2 = new OBDRequest("04", "700", "", "", false);
				OBDRequest obdrequest3 = new OBDRequest("14FFFFFF", "700", "", "", false);
				App.OBDReader.ReplaceQueue(new OBDRequest[]
				{
					obdrequest2, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest,
					obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest3
				});
				await App.OBDReader.WaitForCommandQueue();
				Page currentPage = App.GetCurrentPage();
				if (currentPage is IDTCvXPage)
				{
					await currentPage.DisplayAlert(Translate.GetString("ios_DTCClearFinished_Title"), Translate.GetString("ios_DTCClearFinished_Text"), "OK");
				}
				this.IsSetup = true;
				this.IsCancelButtonEnabled = true;
				this.IsResults = false;
				this.IsBusy = false;
			}
		}

		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x002458D3 File Offset: 0x00243AD3
		// (set) Token: 0x0600340E RID: 13326 RVA: 0x002458DB File Offset: 0x00243ADB
		public Command CancelCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<CancelCommand>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CancelCommand>k__BackingField = value;
			}
		}

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x002458E4 File Offset: 0x00243AE4
		// (set) Token: 0x06003410 RID: 13328 RVA: 0x002458EC File Offset: 0x00243AEC
		public Command BackToSetupCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<BackToSetupCommand>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<BackToSetupCommand>k__BackingField = value;
			}
		}

		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06003411 RID: 13329 RVA: 0x002458F5 File Offset: 0x00243AF5
		// (set) Token: 0x06003412 RID: 13330 RVA: 0x002458FD File Offset: 0x00243AFD
		public Command BackToMainScreenCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<BackToMainScreenCommand>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<BackToMainScreenCommand>k__BackingField = value;
			}
		}

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06003413 RID: 13331 RVA: 0x00245906 File Offset: 0x00243B06
		public Command CheckAllCommand
		{
			get
			{
				return new Command(delegate
				{
					foreach (IECU iecu in this.ECUListFull)
					{
						iecu.IsSelected = true;
					}
				});
			}
		}

		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06003414 RID: 13332 RVA: 0x00245919 File Offset: 0x00243B19
		public Command CheckNoneCommand
		{
			get
			{
				return new Command(delegate
				{
					foreach (IECU iecu in this.ECUListFull)
					{
						iecu.IsSelected = false;
					}
				});
			}
		}

		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x06003415 RID: 13333 RVA: 0x0024592C File Offset: 0x00243B2C
		// (set) Token: 0x06003416 RID: 13334 RVA: 0x00245934 File Offset: 0x00243B34
		public bool IsFilterVisible
		{
			get
			{
				return this._IsFilterVisible;
			}
			set
			{
				this._IsFilterVisible = value;
				this.OnPropertyChanged("IsFilterVisible");
			}
		}

		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x06003417 RID: 13335 RVA: 0x00245948 File Offset: 0x00243B48
		public Command FilterSwitchCommand
		{
			get
			{
				return new Command(delegate
				{
					this.IsFilterVisible = !this.IsFilterVisible;
				});
			}
		}

		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x06003418 RID: 13336 RVA: 0x0024595B File Offset: 0x00243B5B
		public Command BackButtonCommand
		{
			get
			{
				return new Command(delegate
				{
					if (this.IsSetup)
					{
						this.BackToMainScreenCommand.Execute(null);
						return;
					}
					if (this.IsBusy)
					{
						this.CancelCommand.Execute(null);
						return;
					}
					this.BackToSetupCommand.Execute(null);
				});
			}
		}

		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x0024596E File Offset: 0x00243B6E
		// (set) Token: 0x0600341A RID: 13338 RVA: 0x00245976 File Offset: 0x00243B76
		public string StatusText
		{
			get
			{
				return this._StatusText;
			}
			set
			{
				this._StatusText = value;
				this.OnPropertyChanged("StatusText");
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x0024598A File Offset: 0x00243B8A
		// (set) Token: 0x0600341C RID: 13340 RVA: 0x00245992 File Offset: 0x00243B92
		public bool IsBusy
		{
			get
			{
				return this._IsBusy;
			}
			set
			{
				this._IsBusy = value;
				this.OnPropertyChanged("IsBusy");
			}
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x002459A6 File Offset: 0x00243BA6
		// (set) Token: 0x0600341E RID: 13342 RVA: 0x002459AE File Offset: 0x00243BAE
		public bool HasDTCToDisplay
		{
			get
			{
				return this._HasDTCToDisplay;
			}
			set
			{
				this._HasDTCToDisplay = value;
				this.OnPropertyChanged("HasDTCToDisplay");
			}
		}

		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x0600341F RID: 13343 RVA: 0x002459C2 File Offset: 0x00243BC2
		// (set) Token: 0x06003420 RID: 13344 RVA: 0x002459CA File Offset: 0x00243BCA
		public bool BadELMDetected
		{
			get
			{
				return this._BadELMDetected;
			}
			set
			{
				this._BadELMDetected = value;
				this.OnPropertyChanged("BadELMDetected");
			}
		}

		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x002459DE File Offset: 0x00243BDE
		// (set) Token: 0x06003422 RID: 13346 RVA: 0x002459E6 File Offset: 0x00243BE6
		public FormattedString ECUInfoReport
		{
			get
			{
				return this._ECUInfoReport;
			}
			set
			{
				this._ECUInfoReport = value;
				this.OnPropertyChanged("ECUInfoReport");
			}
		}

		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x002459FA File Offset: 0x00243BFA
		public ICommand ReadECUInfoCommand
		{
			get
			{
				return new Command(delegate
				{
					this.ReadECUInfo(null);
				});
			}
		}

		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x00245A0D File Offset: 0x00243C0D
		public ICommand SharedECUInfoCommand
		{
			get
			{
				return new Command(delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (Span span in this.ECUInfoReport.Spans)
					{
						stringBuilder.Append(span.Text);
					}
					string text = stringBuilder.ToString();
					try
					{
						Share.RequestAsync(text);
					}
					catch (Exception)
					{
					}
				});
			}
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x00245A20 File Offset: 0x00243C20
		private Task ReadECUInfo(List<IECU> selectedECUs)
		{
			DTCv2Model.<ReadECUInfo>d__102 <ReadECUInfo>d__;
			<ReadECUInfo>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ReadECUInfo>d__.<>4__this = this;
			<ReadECUInfo>d__.selectedECUs = selectedECUs;
			<ReadECUInfo>d__.<>1__state = -1;
			<ReadECUInfo>d__.<>t__builder.Start<DTCv2Model.<ReadECUInfo>d__102>(ref <ReadECUInfo>d__);
			return <ReadECUInfo>d__.<>t__builder.Task;
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x00245A6C File Offset: 0x00243C6C
		private Task ReadDTC(List<IECU> selectedECUs)
		{
			DTCv2Model.<ReadDTC>d__103 <ReadDTC>d__;
			<ReadDTC>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ReadDTC>d__.<>4__this = this;
			<ReadDTC>d__.selectedECUs = selectedECUs;
			<ReadDTC>d__.<>1__state = -1;
			<ReadDTC>d__.<>t__builder.Start<DTCv2Model.<ReadDTC>d__103>(ref <ReadDTC>d__);
			return <ReadDTC>d__.<>t__builder.Task;
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x00245AB8 File Offset: 0x00243CB8
		private Task ClearDTC(List<IECU> selectedECUs)
		{
			DTCv2Model.<ClearDTC>d__104 <ClearDTC>d__;
			<ClearDTC>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ClearDTC>d__.<>4__this = this;
			<ClearDTC>d__.selectedECUs = selectedECUs;
			<ClearDTC>d__.<>1__state = -1;
			<ClearDTC>d__.<>t__builder.Start<DTCv2Model.<ClearDTC>d__104>(ref <ClearDTC>d__);
			return <ClearDTC>d__.<>t__builder.Task;
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x00245B04 File Offset: 0x00243D04
		public async Task ClearDTCForOneECU(IECU ecu)
		{
			if (!this.IsBusy)
			{
				if (ecu != null)
				{
					Page currentPage = App.GetCurrentPage();
					if (currentPage is IDTCvXPage)
					{
						TaskAwaiter<bool> taskAwaiter = currentPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_ClearOneECUDTC"), ecu.Name), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (taskAwaiter.GetResult())
						{
							this.IsResults = true;
							this.IsSetup = false;
							this.BadELMDetected = false;
							await App.OBDReader.DebugWrite("\r\n[DTCv2ClearSingleECU]\r\n");
							await this.CheckAndRestoreECUConnection();
							this.IsBusy = true;
							this.cancellationTokenSource = new CancellationTokenSource();
							CancellationToken token = this.cancellationTokenSource.Token;
							this.StatusText = string.Format("Clearing {0}/{1}", 1, 1) + "\n" + ecu.Name;
							ecu.Reset();
							if (!string.IsNullOrEmpty(ecu.RequestHeader))
							{
								ecu.TestELMDevice = true;
							}
							await ecu.ClearDTCAsync(new IECU[0], delegate
							{
								this.BadELMDetected = true;
							}, token);
							if (this.ECUListFound.Contains(ecu))
							{
								this.ECUListFound.Remove(ecu);
							}
							Page currentPage2 = App.GetCurrentPage();
							if (currentPage2 is IDTCvXPage)
							{
								await currentPage2.DisplayAlert(Translate.GetString("ios_DTCClearFinished_Title"), Translate.GetString("ios_DTCClearFinished_Text"), "OK");
							}
							this.IsCancelButtonEnabled = true;
							this.IsBusy = false;
						}
					}
				}
			}
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x00245B50 File Offset: 0x00243D50
		private async Task<bool> AskForDTCClearing()
		{
			Page currentPage = App.GetCurrentPage();
			if (currentPage is IDTCvXPage)
			{
				try
				{
					return await currentPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("DtcPage_CleanCodes_Text"), "OK", Translate.GetString("btnCancel.Content"));
				}
				catch (Exception)
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x00245B8C File Offset: 0x00243D8C
		private async void Cancel()
		{
			App.OBDReader.DebugWrite("[DTCv2Model: Cancel requested]");
			this.IsCancelButtonEnabled = false;
			CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			int counter = 0;
			while ((App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU || counter > 5) && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				await Task.Delay(1000);
				counter++;
			}
			this.IsBusy = false;
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x00245BC4 File Offset: 0x00243DC4
		private async void BackToSetup()
		{
			this.IsSetup = true;
			this.IsResults = false;
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x00245BFC File Offset: 0x00243DFC
		private async void BackToMainScreen()
		{
			Page currentPage = App.GetCurrentPage();
			if (currentPage != null && (currentPage is IDTCvXPage || currentPage is EcuInfoPageV2 || currentPage is DTCv3Page))
			{
				try
				{
					await currentPage.Navigation.PopAsync();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x00245C2C File Offset: 0x00243E2C
		private async Task CheckAndRestoreECUConnection()
		{
			this.IsBusy = true;
			this.StatusText = "Checking connection...";
			TaskAwaiter<bool> taskAwaiter = App.OBDReader.CheckECUConnectionWhileRunning(false).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				this.StatusText = "ECU disconnected, trying to reconnect...";
				this.IsCancelButtonEnabled = false;
				await App.OBDReader.Stop("From DTCv2Mode");
				await App.OBDReader.ReinitializeConnectionToECU("DTCResults finishing queue #163");
				this.IsCancelButtonEnabled = true;
				this.StatusText = "ECU connected!";
			}
			else
			{
				this.StatusText = ActivityFrame.PLEASE_WAIT_TEXT;
			}
			this.IsBusy = false;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x00245C6F File Offset: 0x00243E6F
		[CompilerGenerated]
		private void <.ctor>b__2_0()
		{
			this.ReadDTC(null);
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x00245C7C File Offset: 0x00243E7C
		[CompilerGenerated]
		private async void <get_ShareCommand>b__45_0()
		{
			this.ECUListFound.SelectMany((IECU x) => x.DTCCollection).ToList<DTCItemV2>();
			string text = ReportGenerator.CreateReport(this.ECUListFull.Where((IECU x) => x.IsSelected).ToList<IECU>());
			try
			{
				await Share.RequestAsync(text);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x00245CB4 File Offset: 0x00243EB4
		[CompilerGenerated]
		private async void <get_ClearDTCCommand>b__51_0()
		{
			if (this.IsResults)
			{
				if (this.HasDTCToDisplay)
				{
					string action_clear_found = Translate.GetString("dtc_ClearFoundDTC");
					string action_clear_full = Translate.GetString("dtc_ClearFullMode");
					string @string = Translate.GetString("dtc_ClearFoundsQuestion");
					string cancel = Translate.GetString("btnCancel.Content");
					string mass_clear = Translate.GetString("codingDB_DtcClearInMostOfUnits_Name");
					string[] array = new string[] { action_clear_found, action_clear_full };
					string selectedBrand = SharedSettings.Current.SelectedBrand;
					if ((selectedBrand == "Audi" || selectedBrand == "Seat" || selectedBrand == "Skoda" || selectedBrand == "Volkswagen" || selectedBrand == "Cupra") && !VAGECU.HasVWTP20Support)
					{
						array = new string[] { action_clear_found, action_clear_full, mass_clear };
					}
					Page currentPage = App.GetCurrentPage();
					if (currentPage is IDTCvXPage)
					{
						string text = await currentPage.DisplayActionSheetCustom(@string, cancel, null, array);
						if (!(text == cancel))
						{
							if (text == action_clear_full)
							{
								this.IsSetup = true;
								this.IsResults = false;
								this.HasDTCToDisplay = true;
							}
							else if (text == action_clear_found)
							{
								TaskAwaiter<bool> taskAwaiter = this.AskForDTCClearing().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									await taskAwaiter;
									TaskAwaiter<bool> taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<bool>);
								}
								if (!taskAwaiter.GetResult())
								{
									return;
								}
								List<IECU> list = this.ECUListFound.ToList<IECU>();
								foreach (IECU iecu in list)
								{
									iecu.IsSelected = true;
								}
								await this.ClearDTC(list);
							}
							else if (text == mass_clear)
							{
								await this.VagMassClear();
							}
							action_clear_found = null;
							action_clear_full = null;
							cancel = null;
							mass_clear = null;
						}
					}
				}
				else
				{
					this.IsSetup = true;
					this.IsResults = false;
					this.HasDTCToDisplay = true;
				}
			}
			else if (this.IsSetup)
			{
				Page currentPage2 = App.GetCurrentPage();
				if (currentPage2 is IDTCvXPage)
				{
					if (!this.ECUListFull.Any((IECU x) => x.IsSelected))
					{
						try
						{
							await currentPage2.DisplayAlert(Translate.GetString("dtc_NoECUSelected_Title"), Translate.GetString("dtc_NoECUSelected_Text"), "OK");
						}
						catch (Exception)
						{
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter = this.AskForDTCClearing().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (taskAwaiter.GetResult())
						{
							await this.ClearDTC(null);
						}
					}
				}
			}
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x00245CEC File Offset: 0x00243EEC
		[CompilerGenerated]
		private void <get_CheckAllCommand>b__66_0()
		{
			foreach (IECU iecu in this.ECUListFull)
			{
				iecu.IsSelected = true;
			}
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x00245D38 File Offset: 0x00243F38
		[CompilerGenerated]
		private void <get_CheckNoneCommand>b__68_0()
		{
			foreach (IECU iecu in this.ECUListFull)
			{
				iecu.IsSelected = false;
			}
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x00245D84 File Offset: 0x00243F84
		[CompilerGenerated]
		private void <get_FilterSwitchCommand>b__74_0()
		{
			this.IsFilterVisible = !this.IsFilterVisible;
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x00245D95 File Offset: 0x00243F95
		[CompilerGenerated]
		private void <get_BackButtonCommand>b__76_0()
		{
			if (this.IsSetup)
			{
				this.BackToMainScreenCommand.Execute(null);
				return;
			}
			if (this.IsBusy)
			{
				this.CancelCommand.Execute(null);
				return;
			}
			this.BackToSetupCommand.Execute(null);
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x00245DCD File Offset: 0x00243FCD
		[CompilerGenerated]
		private void <get_ReadECUInfoCommand>b__99_0()
		{
			this.ReadECUInfo(null);
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x00245DD8 File Offset: 0x00243FD8
		[CompilerGenerated]
		private void <get_SharedECUInfoCommand>b__101_0()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Span span in this.ECUInfoReport.Spans)
			{
				stringBuilder.Append(span.Text);
			}
			string text = stringBuilder.ToString();
			try
			{
				Share.RequestAsync(text);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x00245E58 File Offset: 0x00244058
		[CompilerGenerated]
		private void <ReadECUInfo>b__102_3()
		{
			this.BadELMDetected = true;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x00245E61 File Offset: 0x00244061
		[CompilerGenerated]
		private void <ReadECUInfo>b__102_4()
		{
			this.OnPropertyChanged("ECUInfoReport");
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x00245E58 File Offset: 0x00244058
		[CompilerGenerated]
		private void <ReadDTC>b__103_3()
		{
			this.BadELMDetected = true;
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x00245E58 File Offset: 0x00244058
		[CompilerGenerated]
		private void <ClearDTC>b__104_2()
		{
			this.BadELMDetected = true;
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x00245E58 File Offset: 0x00244058
		[CompilerGenerated]
		private void <ClearDTCForOneECU>b__105_0()
		{
			this.BadELMDetected = true;
		}

		// Token: 0x04001E91 RID: 7825
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04001E92 RID: 7826
		private bool _IsSetup = true;

		// Token: 0x04001E93 RID: 7827
		private bool _IsResults;

		// Token: 0x04001E94 RID: 7828
		private bool _IsCancelButtonEnabled = true;

		// Token: 0x04001E95 RID: 7829
		private ObservableCollection<IECU> _ECUListFull;

		// Token: 0x04001E96 RID: 7830
		[CompilerGenerated]
		private ObservableCollection<IECU> <ECUListDetected>k__BackingField;

		// Token: 0x04001E97 RID: 7831
		[CompilerGenerated]
		private ObservableCollection<IECU> <ECUListFound>k__BackingField;

		// Token: 0x04001E98 RID: 7832
		private bool _DisplayDetected = true;

		// Token: 0x04001E99 RID: 7833
		internal ISupportedUnitsDetector SupportedUnitsDetector = new BaseSupportedUnitsDetector();

		// Token: 0x04001E9A RID: 7834
		[CompilerGenerated]
		private Command <ReadDTCCommand>k__BackingField;

		// Token: 0x04001E9B RID: 7835
		[CompilerGenerated]
		private Command <CancelCommand>k__BackingField;

		// Token: 0x04001E9C RID: 7836
		[CompilerGenerated]
		private Command <BackToSetupCommand>k__BackingField;

		// Token: 0x04001E9D RID: 7837
		[CompilerGenerated]
		private Command <BackToMainScreenCommand>k__BackingField;

		// Token: 0x04001E9E RID: 7838
		private bool _IsFilterVisible;

		// Token: 0x04001E9F RID: 7839
		private string _StatusText = ActivityFrame.PLEASE_WAIT_TEXT;

		// Token: 0x04001EA0 RID: 7840
		private bool _IsBusy;

		// Token: 0x04001EA1 RID: 7841
		private bool _HasDTCToDisplay = true;

		// Token: 0x04001EA2 RID: 7842
		private bool _BadELMDetected;

		// Token: 0x04001EA3 RID: 7843
		private CancellationTokenSource cancellationTokenSource;

		// Token: 0x04001EA4 RID: 7844
		private FormattedString _ECUInfoReport = new FormattedString();

		// Token: 0x02000591 RID: 1425
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_ClearDTCCommand>b__51_0>d : IAsyncStateMachine
		{
			// Token: 0x0600343C RID: 13372 RVA: 0x00245E70 File Offset: 0x00244070
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Model dtcv2Model = this;
				try
				{
					TaskAwaiter<string> taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					TaskAwaiter taskAwaiter6;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						break;
					}
					case 1:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_024A;
					case 2:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_02F3;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0362;
					}
					case 4:
						goto IL_03F3;
					case 5:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_04D1;
					case 6:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0531;
					}
					default:
						if (dtcv2Model.IsResults)
						{
							if (!dtcv2Model.HasDTCToDisplay)
							{
								dtcv2Model.IsSetup = true;
								dtcv2Model.IsResults = false;
								dtcv2Model.HasDTCToDisplay = true;
								goto IL_0538;
							}
							action_clear_found = Translate.GetString("dtc_ClearFoundDTC");
							action_clear_full = Translate.GetString("dtc_ClearFullMode");
							string @string = Translate.GetString("dtc_ClearFoundsQuestion");
							cancel = Translate.GetString("btnCancel.Content");
							mass_clear = Translate.GetString("codingDB_DtcClearInMostOfUnits_Name");
							string[] array = new string[] { action_clear_found, action_clear_full };
							string selectedBrand = SharedSettings.Current.SelectedBrand;
							if ((selectedBrand == "Audi" || selectedBrand == "Seat" || selectedBrand == "Skoda" || selectedBrand == "Volkswagen" || selectedBrand == "Cupra") && !VAGECU.HasVWTP20Support)
							{
								array = new string[] { action_clear_found, action_clear_full, mass_clear };
							}
							Page currentPage = App.GetCurrentPage();
							if (!(currentPage is IDTCvXPage))
							{
								goto IL_0553;
							}
							taskAwaiter3 = currentPage.DisplayActionSheetCustom(@string, cancel, null, array).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, DTCv2Model.<<get_ClearDTCCommand>b__51_0>d>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							if (!dtcv2Model.IsSetup)
							{
								goto IL_0538;
							}
							Page currentPage2 = App.GetCurrentPage();
							if (!(currentPage2 is IDTCvXPage))
							{
								goto IL_0553;
							}
							if (!dtcv2Model.ECUListFull.Any((IECU x) => x.IsSelected))
							{
								goto IL_03F3;
							}
							taskAwaiter5 = dtcv2Model.AskForDTCClearing().GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num = (num2 = 5);
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCv2Model.<<get_ClearDTCCommand>b__51_0>d>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_04D1;
						}
						break;
					}
					string result = taskAwaiter3.GetResult();
					if (result == cancel)
					{
						goto IL_0553;
					}
					if (result == action_clear_full)
					{
						dtcv2Model.IsSetup = true;
						dtcv2Model.IsResults = false;
						dtcv2Model.HasDTCToDisplay = true;
						goto IL_0369;
					}
					if (result == action_clear_found)
					{
						taskAwaiter5 = dtcv2Model.AskForDTCClearing().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num = (num2 = 1);
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCv2Model.<<get_ClearDTCCommand>b__51_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						if (!(result == mass_clear))
						{
							goto IL_0369;
						}
						taskAwaiter6 = dtcv2Model.VagMassClear().GetAwaiter();
						if (!taskAwaiter6.IsCompleted)
						{
							num = (num2 = 3);
							TaskAwaiter taskAwaiter7 = taskAwaiter6;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<<get_ClearDTCCommand>b__51_0>d>(ref taskAwaiter6, ref this);
							return;
						}
						goto IL_0362;
					}
					IL_024A:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0553;
					}
					List<IECU> list = dtcv2Model.ECUListFound.ToList<IECU>();
					List<IECU>.Enumerator enumerator = list.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IECU iecu = enumerator.Current;
							iecu.IsSelected = true;
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					taskAwaiter6 = dtcv2Model.ClearDTC(list).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<<get_ClearDTCCommand>b__51_0>d>(ref taskAwaiter6, ref this);
						return;
					}
					IL_02F3:
					taskAwaiter6.GetResult();
					goto IL_0369;
					IL_0362:
					taskAwaiter6.GetResult();
					IL_0369:
					action_clear_found = null;
					action_clear_full = null;
					cancel = null;
					mass_clear = null;
					goto IL_0538;
					IL_03F3:
					try
					{
						if (num != 4)
						{
							Page currentPage2;
							taskAwaiter6 = currentPage2.DisplayAlert(Translate.GetString("dtc_NoECUSelected_Title"), Translate.GetString("dtc_NoECUSelected_Text"), "OK").GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num = (num2 = 4);
								TaskAwaiter taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<<get_ClearDTCCommand>b__51_0>d>(ref taskAwaiter6, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter7;
							taskAwaiter6 = taskAwaiter7;
							taskAwaiter7 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter6.GetResult();
					}
					catch (Exception)
					{
					}
					goto IL_0553;
					IL_04D1:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0553;
					}
					taskAwaiter6 = dtcv2Model.ClearDTC(null).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num = (num2 = 6);
						TaskAwaiter taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<<get_ClearDTCCommand>b__51_0>d>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0531:
					taskAwaiter6.GetResult();
					IL_0538:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0553:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600343D RID: 13373 RVA: 0x00246430 File Offset: 0x00244630
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001EA5 RID: 7845
			public int <>1__state;

			// Token: 0x04001EA6 RID: 7846
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001EA7 RID: 7847
			public DTCv2Model <>4__this;

			// Token: 0x04001EA8 RID: 7848
			private string <action_clear_found>5__2;

			// Token: 0x04001EA9 RID: 7849
			private string <action_clear_full>5__3;

			// Token: 0x04001EAA RID: 7850
			private string <cancel>5__4;

			// Token: 0x04001EAB RID: 7851
			private string <mass_clear>5__5;

			// Token: 0x04001EAC RID: 7852
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04001EAD RID: 7853
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04001EAE RID: 7854
			private TaskAwaiter <>u__3;
		}

		// Token: 0x02000592 RID: 1426
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_ShareCommand>b__45_0>d : IAsyncStateMachine
		{
			// Token: 0x0600343E RID: 13374 RVA: 0x00246440 File Offset: 0x00244640
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Model dtcv2Model = this;
				try
				{
					string text;
					if (num != 0)
					{
						dtcv2Model.ECUListFound.SelectMany((IECU x) => x.DTCCollection).ToList<DTCItemV2>();
						text = ReportGenerator.CreateReport(dtcv2Model.ECUListFull.Where((IECU x) => x.IsSelected).ToList<IECU>());
					}
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = Share.RequestAsync(text).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<<get_ShareCommand>b__45_0>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600343F RID: 13375 RVA: 0x00246570 File Offset: 0x00244770
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001EAF RID: 7855
			public int <>1__state;

			// Token: 0x04001EB0 RID: 7856
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001EB1 RID: 7857
			public DTCv2Model <>4__this;

			// Token: 0x04001EB2 RID: 7858
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000593 RID: 1427
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003440 RID: 13376 RVA: 0x0024657E File Offset: 0x0024477E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003441 RID: 13377 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003442 RID: 13378 RVA: 0x0024658A File Offset: 0x0024478A
			internal bool <Initialize>b__3_0(IECU x)
			{
				return x is OBD2Can29bitECU;
			}

			// Token: 0x06003443 RID: 13379 RVA: 0x00246595 File Offset: 0x00244795
			internal bool <Initialize>b__3_1(IECU x)
			{
				return x is OBD2Can11bitECU;
			}

			// Token: 0x06003444 RID: 13380 RVA: 0x00246595 File Offset: 0x00244795
			internal bool <Initialize>b__3_2(IECU x)
			{
				return x is OBD2Can11bitECU;
			}

			// Token: 0x06003445 RID: 13381 RVA: 0x00246595 File Offset: 0x00244795
			internal bool <Initialize>b__3_3(IECU x)
			{
				return x is OBD2Can11bitECU;
			}

			// Token: 0x06003446 RID: 13382 RVA: 0x00246595 File Offset: 0x00244795
			internal bool <Initialize>b__3_4(IECU x)
			{
				return x is OBD2Can11bitECU;
			}

			// Token: 0x06003447 RID: 13383 RVA: 0x002465A0 File Offset: 0x002447A0
			internal bool <UpdateCollections>b__6_0(IECU x)
			{
				return x.Count<DTCItemV2>() > 0;
			}

			// Token: 0x06003448 RID: 13384 RVA: 0x002465AB File Offset: 0x002447AB
			internal IEnumerable<DTCItemV2> <get_ShareCommand>b__45_1(IECU x)
			{
				return x.DTCCollection;
			}

			// Token: 0x06003449 RID: 13385 RVA: 0x002465B3 File Offset: 0x002447B3
			internal bool <get_ShareCommand>b__45_2(IECU x)
			{
				return x.IsSelected;
			}

			// Token: 0x0600344A RID: 13386 RVA: 0x002465B3 File Offset: 0x002447B3
			internal bool <get_ClearDTCCommand>b__51_1(IECU x)
			{
				return x.IsSelected;
			}

			// Token: 0x0600344B RID: 13387 RVA: 0x002465B3 File Offset: 0x002447B3
			internal bool <ReadECUInfo>b__102_0(IECU x)
			{
				return x.IsSelected;
			}

			// Token: 0x0600344C RID: 13388 RVA: 0x002465B3 File Offset: 0x002447B3
			internal bool <ReadECUInfo>b__102_1(IECU x)
			{
				return x.IsSelected;
			}

			// Token: 0x0600344D RID: 13389 RVA: 0x002465BB File Offset: 0x002447BB
			internal bool <ReadECUInfo>b__102_2(IECU x)
			{
				return !string.IsNullOrEmpty(x.RequestHeader);
			}

			// Token: 0x0600344E RID: 13390 RVA: 0x002465CB File Offset: 0x002447CB
			internal bool <ReadECUInfo>b__102_5(IECU x)
			{
				return x.ECUExists;
			}

			// Token: 0x0600344F RID: 13391 RVA: 0x002465D3 File Offset: 0x002447D3
			internal int <ReadECUInfo>b__102_6(IECU x)
			{
				return x.GetHashCode();
			}

			// Token: 0x06003450 RID: 13392 RVA: 0x002465B3 File Offset: 0x002447B3
			internal bool <ReadDTC>b__103_0(IECU x)
			{
				return x.IsSelected;
			}

			// Token: 0x06003451 RID: 13393 RVA: 0x002465B3 File Offset: 0x002447B3
			internal bool <ReadDTC>b__103_1(IECU x)
			{
				return x.IsSelected;
			}

			// Token: 0x06003452 RID: 13394 RVA: 0x002465BB File Offset: 0x002447BB
			internal bool <ReadDTC>b__103_2(IECU x)
			{
				return !string.IsNullOrEmpty(x.RequestHeader);
			}

			// Token: 0x06003453 RID: 13395 RVA: 0x002465CB File Offset: 0x002447CB
			internal bool <ReadDTC>b__103_4(IECU x)
			{
				return x.ECUExists;
			}

			// Token: 0x06003454 RID: 13396 RVA: 0x002465D3 File Offset: 0x002447D3
			internal int <ReadDTC>b__103_5(IECU x)
			{
				return x.GetHashCode();
			}

			// Token: 0x06003455 RID: 13397 RVA: 0x002465B3 File Offset: 0x002447B3
			internal bool <ClearDTC>b__104_0(IECU x)
			{
				return x.IsSelected;
			}

			// Token: 0x06003456 RID: 13398 RVA: 0x002465BB File Offset: 0x002447BB
			internal bool <ClearDTC>b__104_1(IECU x)
			{
				return !string.IsNullOrEmpty(x.RequestHeader);
			}

			// Token: 0x04001EB3 RID: 7859
			public static readonly DTCv2Model.<>c <>9 = new DTCv2Model.<>c();

			// Token: 0x04001EB4 RID: 7860
			public static Predicate<IECU> <>9__3_0;

			// Token: 0x04001EB5 RID: 7861
			public static Predicate<IECU> <>9__3_1;

			// Token: 0x04001EB6 RID: 7862
			public static Predicate<IECU> <>9__3_2;

			// Token: 0x04001EB7 RID: 7863
			public static Predicate<IECU> <>9__3_3;

			// Token: 0x04001EB8 RID: 7864
			public static Predicate<IECU> <>9__3_4;

			// Token: 0x04001EB9 RID: 7865
			public static Func<IECU, bool> <>9__6_0;

			// Token: 0x04001EBA RID: 7866
			public static Func<IECU, IEnumerable<DTCItemV2>> <>9__45_1;

			// Token: 0x04001EBB RID: 7867
			public static Func<IECU, bool> <>9__45_2;

			// Token: 0x04001EBC RID: 7868
			public static Func<IECU, bool> <>9__51_1;

			// Token: 0x04001EBD RID: 7869
			public static Func<IECU, bool> <>9__102_0;

			// Token: 0x04001EBE RID: 7870
			public static Func<IECU, bool> <>9__102_1;

			// Token: 0x04001EBF RID: 7871
			public static Func<IECU, bool> <>9__102_2;

			// Token: 0x04001EC0 RID: 7872
			public static Func<IECU, bool> <>9__102_5;

			// Token: 0x04001EC1 RID: 7873
			public static Func<IECU, int> <>9__102_6;

			// Token: 0x04001EC2 RID: 7874
			public static Func<IECU, bool> <>9__103_0;

			// Token: 0x04001EC3 RID: 7875
			public static Func<IECU, bool> <>9__103_1;

			// Token: 0x04001EC4 RID: 7876
			public static Func<IECU, bool> <>9__103_2;

			// Token: 0x04001EC5 RID: 7877
			public static Func<IECU, bool> <>9__103_4;

			// Token: 0x04001EC6 RID: 7878
			public static Func<IECU, int> <>9__103_5;

			// Token: 0x04001EC7 RID: 7879
			public static Func<IECU, bool> <>9__104_0;

			// Token: 0x04001EC8 RID: 7880
			public static Func<IECU, bool> <>9__104_1;
		}

		// Token: 0x02000594 RID: 1428
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06003457 RID: 13399 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06003458 RID: 13400 RVA: 0x002465DC File Offset: 0x002447DC
			internal KWPECU <Initialize>b__5(string header)
			{
				return new KWPECU
				{
					Name = "ECU Address $" + header.Substring(2, 2),
					RequestHeader = header,
					ReadDTCCommands = this.readDTCCommands,
					ClearDTCCommands = this.clearDTCCommands,
					TestIfEcuExists = false
				};
			}

			// Token: 0x04001EC9 RID: 7881
			public List<string> readDTCCommands;

			// Token: 0x04001ECA RID: 7882
			public List<string> clearDTCCommands;
		}

		// Token: 0x02000595 RID: 1429
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_1
		{
			// Token: 0x06003459 RID: 13401 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_1()
			{
			}

			// Token: 0x0600345A RID: 13402 RVA: 0x0024662C File Offset: 0x0024482C
			internal bool <Initialize>b__6(IECU x)
			{
				return x.RequestHeader != null && x.RequestHeader.Length == 6 && x.RequestHeader.Substring(2, 2) == this.ecuAddress;
			}

			// Token: 0x04001ECB RID: 7883
			public string ecuAddress;
		}

		// Token: 0x02000596 RID: 1430
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AskForDTCClearing>d__106 : IAsyncStateMachine
		{
			// Token: 0x0600345B RID: 13403 RVA: 0x00246660 File Offset: 0x00244860
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					Page currentPage;
					if (num != 0)
					{
						currentPage = App.GetCurrentPage();
						if (!(currentPage is IDTCvXPage))
						{
							goto IL_00A2;
						}
					}
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = currentPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("DtcPage_CleanCodes_Text"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCv2Model.<AskForDTCClearing>d__106>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						flag = taskAwaiter.GetResult();
						goto IL_00BF;
					}
					catch (Exception)
					{
						flag = false;
						goto IL_00BF;
					}
					IL_00A2:
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00BF:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600345C RID: 13404 RVA: 0x0024675C File Offset: 0x0024495C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001ECC RID: 7884
			public int <>1__state;

			// Token: 0x04001ECD RID: 7885
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001ECE RID: 7886
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000597 RID: 1431
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BackToMainScreen>d__109 : IAsyncStateMachine
		{
			// Token: 0x0600345D RID: 13405 RVA: 0x0024676C File Offset: 0x0024496C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					Page currentPage;
					if (num != 0)
					{
						currentPage = App.GetCurrentPage();
						if (currentPage == null || (!(currentPage is IDTCvXPage) && !(currentPage is EcuInfoPageV2) && !(currentPage is DTCv3Page)))
						{
							goto IL_0092;
						}
					}
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = currentPage.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DTCv2Model.<BackToMainScreen>d__109>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<Page> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Page>);
							num2 = -1;
						}
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
					}
					IL_0092:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600345E RID: 13406 RVA: 0x00246854 File Offset: 0x00244A54
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001ECF RID: 7887
			public int <>1__state;

			// Token: 0x04001ED0 RID: 7888
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001ED1 RID: 7889
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000598 RID: 1432
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BackToSetup>d__108 : IAsyncStateMachine
		{
			// Token: 0x0600345F RID: 13407 RVA: 0x00246864 File Offset: 0x00244A64
			void IAsyncStateMachine.MoveNext()
			{
				DTCv2Model dtcv2Model = this;
				try
				{
					dtcv2Model.IsSetup = true;
					dtcv2Model.IsResults = false;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003460 RID: 13408 RVA: 0x002468C4 File Offset: 0x00244AC4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001ED2 RID: 7890
			public int <>1__state;

			// Token: 0x04001ED3 RID: 7891
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001ED4 RID: 7892
			public DTCv2Model <>4__this;
		}

		// Token: 0x02000599 RID: 1433
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Cancel>d__107 : IAsyncStateMachine
		{
			// Token: 0x06003461 RID: 13409 RVA: 0x002468D4 File Offset: 0x00244AD4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Model dtcv2Model = this;
				try
				{
					if (num != 0)
					{
						App.OBDReader.DebugWrite("[DTCv2Model: Cancel requested]");
						dtcv2Model.IsCancelButtonEnabled = false;
						CancellationTokenSource cancellationTokenSource = dtcv2Model.cancellationTokenSource;
						if (cancellationTokenSource != null)
						{
							cancellationTokenSource.Cancel();
						}
						counter = 0;
						goto IL_00C1;
					}
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_00AA:
					taskAwaiter.GetResult();
					int num3 = counter;
					counter = num3 + 1;
					IL_00C1:
					if ((App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU || counter > 5) && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<Cancel>d__107>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_00AA;
					}
					else
					{
						dtcv2Model.IsBusy = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003462 RID: 13410 RVA: 0x00246A04 File Offset: 0x00244C04
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001ED5 RID: 7893
			public int <>1__state;

			// Token: 0x04001ED6 RID: 7894
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001ED7 RID: 7895
			public DTCv2Model <>4__this;

			// Token: 0x04001ED8 RID: 7896
			private int <counter>5__2;

			// Token: 0x04001ED9 RID: 7897
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200059A RID: 1434
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckAndRestoreECUConnection>d__110 : IAsyncStateMachine
		{
			// Token: 0x06003463 RID: 13411 RVA: 0x00246A14 File Offset: 0x00244C14
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Model dtcv2Model = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0106;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0167;
					default:
						dtcv2Model.IsBusy = true;
						dtcv2Model.StatusText = "Checking connection...";
						taskAwaiter3 = App.OBDReader.CheckECUConnectionWhileRunning(false).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCv2Model.<CheckAndRestoreECUConnection>d__110>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (taskAwaiter3.GetResult())
					{
						dtcv2Model.StatusText = ActivityFrame.PLEASE_WAIT_TEXT;
						goto IL_018E;
					}
					dtcv2Model.StatusText = "ECU disconnected, trying to reconnect...";
					dtcv2Model.IsCancelButtonEnabled = false;
					taskAwaiter4 = App.OBDReader.Stop("From DTCv2Mode").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<CheckAndRestoreECUConnection>d__110>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0106:
					taskAwaiter4.GetResult();
					taskAwaiter3 = App.OBDReader.ReinitializeConnectionToECU("DTCResults finishing queue #163").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCv2Model.<CheckAndRestoreECUConnection>d__110>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0167:
					taskAwaiter3.GetResult();
					dtcv2Model.IsCancelButtonEnabled = true;
					dtcv2Model.StatusText = "ECU connected!";
					IL_018E:
					dtcv2Model.IsBusy = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003464 RID: 13412 RVA: 0x00246C00 File Offset: 0x00244E00
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001EDA RID: 7898
			public int <>1__state;

			// Token: 0x04001EDB RID: 7899
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001EDC RID: 7900
			public DTCv2Model <>4__this;

			// Token: 0x04001EDD RID: 7901
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001EDE RID: 7902
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200059B RID: 1435
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearDTC>d__104 : IAsyncStateMachine
		{
			// Token: 0x06003465 RID: 13413 RVA: 0x00246C10 File Offset: 0x00244E10
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				DTCv2Model dtcv2Model = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						break;
					case 1:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_0113;
					case 2:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_0259;
					case 3:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_03CB;
					case 4:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_04C1;
					default:
						if (dtcv2Model.IsBusy)
						{
							goto IL_0512;
						}
						dtcv2Model.IsResults = true;
						dtcv2Model.IsSetup = false;
						dtcv2Model.BadELMDetected = false;
						dtcv2Model.ECUListFound.Clear();
						taskAwaiter = App.OBDReader.DebugWrite("\r\n[DTCv2Clear]\r\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (this.<>1__state = 0);
							this.<>u__1 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTC>d__104>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					taskAwaiter = dtcv2Model.CheckAndRestoreECUConnection().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (this.<>1__state = 1);
						this.<>u__1 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTC>d__104>(ref taskAwaiter, ref this);
						return;
					}
					IL_0113:
					taskAwaiter.GetResult();
					dtcv2Model.IsBusy = true;
					dtcv2Model.IsCancelButtonEnabled = true;
					dtcv2Model.cancellationTokenSource = new CancellationTokenSource();
					this.<token>5__2 = dtcv2Model.cancellationTokenSource.Token;
					if (selectedECUs == null)
					{
						selectedECUs = dtcv2Model.ECUListToDisplay.Where((IECU x) => x.IsSelected).ToList<IECU>();
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("\r\n[Selected:");
					List<IECU>.Enumerator enumerator = selectedECUs.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IECU iecu = enumerator.Current;
							stringBuilder.Append(iecu.Name);
							stringBuilder.Append(",");
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					stringBuilder.Append("]\r\n");
					taskAwaiter = App.OBDReader.DebugWrite(Encoding.UTF8.GetBytes(stringBuilder.ToString())).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (this.<>1__state = 2);
						this.<>u__1 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTC>d__104>(ref taskAwaiter, ref this);
						return;
					}
					IL_0259:
					taskAwaiter.GetResult();
					IEnumerator<IECU> enumerator2 = dtcv2Model.ECUListToDisplay.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IECU iecu2 = enumerator2.Current;
							iecu2.Reset();
						}
					}
					finally
					{
						if (num < 0 && enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					IECU iecu3 = selectedECUs.FirstOrDefault((IECU x) => !string.IsNullOrEmpty(x.RequestHeader));
					if (iecu3 != null)
					{
						iecu3.TestELMDevice = true;
					}
					this.<badELMDetectedAction>5__3 = delegate
					{
						base.BadELMDetected = true;
					};
					this.<i>5__4 = 0;
					goto IL_042A;
					IL_03CB:
					taskAwaiter.GetResult();
					if (dtcv2Model.ECUListFound.Contains(this.<ecu>5__5))
					{
						dtcv2Model.ECUListFound.Remove(this.<ecu>5__5);
					}
					if (dtcv2Model.cancellationTokenSource.IsCancellationRequested || this.<token>5__2.IsCancellationRequested)
					{
						goto IL_0440;
					}
					this.<ecu>5__5 = null;
					IL_0418:
					int num2 = this.<i>5__4;
					this.<i>5__4 = num2 + 1;
					IL_042A:
					if (this.<i>5__4 < selectedECUs.Count)
					{
						this.<ecu>5__5 = selectedECUs[this.<i>5__4];
						if (!this.<ecu>5__5.IsSelected)
						{
							goto IL_0418;
						}
						dtcv2Model.StatusText = string.Format("{0}/{1}", this.<i>5__4 + 1, selectedECUs.Count) + "\n" + this.<ecu>5__5.Name;
						this.<ecu>5__5.Reset();
						taskAwaiter = this.<ecu>5__5.ClearDTCAsync(selectedECUs, this.<badELMDetectedAction>5__3, this.<token>5__2).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (this.<>1__state = 3);
							this.<>u__1 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTC>d__104>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_03CB;
					}
					IL_0440:
					Page currentPage = App.GetCurrentPage();
					if (!(currentPage is IDTCvXPage))
					{
						goto IL_04C8;
					}
					taskAwaiter = currentPage.DisplayAlert(Translate.GetString("ios_DTCClearFinished_Title"), Translate.GetString("ios_DTCClearFinished_Text"), "OK").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (this.<>1__state = 4);
						this.<>u__1 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTC>d__104>(ref taskAwaiter, ref this);
						return;
					}
					IL_04C1:
					taskAwaiter.GetResult();
					IL_04C8:
					dtcv2Model.IsSetup = true;
					dtcv2Model.IsCancelButtonEnabled = true;
					dtcv2Model.IsResults = false;
					dtcv2Model.IsBusy = false;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<token>5__2 = default(CancellationToken);
					this.<badELMDetectedAction>5__3 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0512:
				this.<>1__state = -2;
				this.<token>5__2 = default(CancellationToken);
				this.<badELMDetectedAction>5__3 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003466 RID: 13414 RVA: 0x002471A4 File Offset: 0x002453A4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001EDF RID: 7903
			public int <>1__state;

			// Token: 0x04001EE0 RID: 7904
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001EE1 RID: 7905
			public DTCv2Model <>4__this;

			// Token: 0x04001EE2 RID: 7906
			public List<IECU> selectedECUs;

			// Token: 0x04001EE3 RID: 7907
			private CancellationToken <token>5__2;

			// Token: 0x04001EE4 RID: 7908
			private Action <badELMDetectedAction>5__3;

			// Token: 0x04001EE5 RID: 7909
			private TaskAwaiter <>u__1;

			// Token: 0x04001EE6 RID: 7910
			private int <i>5__4;

			// Token: 0x04001EE7 RID: 7911
			private IECU <ecu>5__5;
		}

		// Token: 0x0200059C RID: 1436
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearDTCForOneECU>d__105 : IAsyncStateMachine
		{
			// Token: 0x06003467 RID: 13415 RVA: 0x002471B4 File Offset: 0x002453B4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Model dtcv2Model = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0162;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01C0;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02AE;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0358;
					}
					default:
					{
						if (dtcv2Model.IsBusy)
						{
							goto IL_0388;
						}
						if (ecu == null)
						{
							goto IL_0388;
						}
						Page currentPage = App.GetCurrentPage();
						if (!(currentPage is IDTCvXPage))
						{
							goto IL_0388;
						}
						taskAwaiter3 = currentPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_ClearOneECUDTC"), ecu.Name), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCv2Model.<ClearDTCForOneECU>d__105>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0388;
					}
					dtcv2Model.IsResults = true;
					dtcv2Model.IsSetup = false;
					dtcv2Model.BadELMDetected = false;
					taskAwaiter4 = App.OBDReader.DebugWrite("\r\n[DTCv2ClearSingleECU]\r\n").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTCForOneECU>d__105>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0162:
					taskAwaiter4.GetResult();
					taskAwaiter4 = dtcv2Model.CheckAndRestoreECUConnection().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTCForOneECU>d__105>(ref taskAwaiter4, ref this);
						return;
					}
					IL_01C0:
					taskAwaiter4.GetResult();
					dtcv2Model.IsBusy = true;
					dtcv2Model.cancellationTokenSource = new CancellationTokenSource();
					CancellationToken token = dtcv2Model.cancellationTokenSource.Token;
					dtcv2Model.StatusText = string.Format("Clearing {0}/{1}", 1, 1) + "\n" + ecu.Name;
					ecu.Reset();
					if (!string.IsNullOrEmpty(ecu.RequestHeader))
					{
						ecu.TestELMDevice = true;
					}
					taskAwaiter4 = ecu.ClearDTCAsync(new IECU[0], delegate
					{
						base.BadELMDetected = true;
					}, token).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTCForOneECU>d__105>(ref taskAwaiter4, ref this);
						return;
					}
					IL_02AE:
					taskAwaiter4.GetResult();
					if (dtcv2Model.ECUListFound.Contains(ecu))
					{
						dtcv2Model.ECUListFound.Remove(ecu);
					}
					Page currentPage2 = App.GetCurrentPage();
					if (!(currentPage2 is IDTCvXPage))
					{
						goto IL_035F;
					}
					taskAwaiter4 = currentPage2.DisplayAlert(Translate.GetString("ios_DTCClearFinished_Title"), Translate.GetString("ios_DTCClearFinished_Text"), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ClearDTCForOneECU>d__105>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0358:
					taskAwaiter4.GetResult();
					IL_035F:
					dtcv2Model.IsCancelButtonEnabled = true;
					dtcv2Model.IsBusy = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0388:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003468 RID: 13416 RVA: 0x00247578 File Offset: 0x00245778
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001EE8 RID: 7912
			public int <>1__state;

			// Token: 0x04001EE9 RID: 7913
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001EEA RID: 7914
			public DTCv2Model <>4__this;

			// Token: 0x04001EEB RID: 7915
			public IECU ecu;

			// Token: 0x04001EEC RID: 7916
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001EED RID: 7917
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200059D RID: 1437
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DetectSupportedUnits>d__4 : IAsyncStateMachine
		{
			// Token: 0x06003469 RID: 13417 RVA: 0x00247588 File Offset: 0x00245788
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Model dtcv2Model = this;
				try
				{
					if (num != 0)
					{
						dtcv2Model.IsBusy = true;
					}
					try
					{
						TaskAwaiter<List<IECU>> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = dtcv2Model.SupportedUnitsDetector.DetectSupported(dtcv2Model.ECUListFull).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<List<IECU>> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<IECU>>, DTCv2Model.<DetectSupportedUnits>d__4>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<List<IECU>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<List<IECU>>);
							num = (num2 = -1);
						}
						List<IECU> result = taskAwaiter.GetResult();
						dtcv2Model.ECUListDetected.Clear();
						List<IECU>.Enumerator enumerator = result.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								IECU iecu = enumerator.Current;
								dtcv2Model.ECUListDetected.Add(iecu);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
					}
					catch (Exception)
					{
						dtcv2Model.ECUListDetected = dtcv2Model.ECUListFull;
					}
					finally
					{
						if (num < 0)
						{
							dtcv2Model.IsBusy = false;
						}
					}
					if (dtcv2Model.ECUListDetected.Count > 1)
					{
						dtcv2Model.DisplayDetected = true;
					}
					else
					{
						dtcv2Model.DisplayDetected = false;
					}
					dtcv2Model.OnPropertyChanged("ECUListToDisplay");
					dtcv2Model.OnPropertyChanged("DisplayAllOrDetectedSelector");
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600346A RID: 13418 RVA: 0x00247744 File Offset: 0x00245944
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001EEE RID: 7918
			public int <>1__state;

			// Token: 0x04001EEF RID: 7919
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001EF0 RID: 7920
			public DTCv2Model <>4__this;

			// Token: 0x04001EF1 RID: 7921
			private TaskAwaiter<List<IECU>> <>u__1;
		}

		// Token: 0x0200059E RID: 1438
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Initialize>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600346B RID: 13419 RVA: 0x00247754 File Offset: 0x00245954
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Model dtcv2Model = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						switch (App.OBDReader.CurrentELMFormat)
						{
						case ELMFormat.KWP:
						{
							string text = brand;
							if (text != null)
							{
								switch (text.Length)
								{
								case 4:
								{
									char c = text[0];
									if (c != 'A')
									{
										if (c != 'S')
										{
											goto IL_164D;
										}
										if (!(text == "Seat"))
										{
											goto IL_164D;
										}
									}
									else if (!(text == "Audi"))
									{
										goto IL_164D;
									}
									break;
								}
								case 5:
								{
									char c = text[0];
									if (c != 'D')
									{
										if (c != 'J')
										{
											if (c != 'S')
											{
												goto IL_164D;
											}
											if (!(text == "Skoda"))
											{
												goto IL_164D;
											}
										}
										else if (!(text == "Jetta"))
										{
											goto IL_164D;
										}
									}
									else
									{
										if (!(text == "Dacia"))
										{
											goto IL_164D;
										}
										goto IL_125B;
									}
									break;
								}
								case 6:
									if (!(text == "Delphi"))
									{
										goto IL_164D;
									}
									dtcv2Model.ECUListFull = new ObservableCollection<IECU>(DelphiKWP_ECU.ECUs);
									goto IL_164D;
								case 7:
								{
									char c = text[0];
									if (c != 'P')
									{
										if (c != 'R')
										{
											goto IL_164D;
										}
										if (!(text == "Renault"))
										{
											goto IL_164D;
										}
										goto IL_125B;
									}
									else if (!(text == "Porsche"))
									{
										goto IL_164D;
									}
									break;
								}
								case 8:
								case 9:
									goto IL_164D;
								case 10:
									if (!(text == "Volkswagen"))
									{
										goto IL_164D;
									}
									break;
								default:
									goto IL_164D;
								}
								List<IECU> list = VAGECU.ECUs.ToList<IECU>();
								list.RemoveAll((IECU x) => x is OBD2Can11bitECU);
								list.Insert(0, new OBD2KWPECU());
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(list);
								break;
								IL_125B:
								List<IECU> list2 = new List<IECU>();
								list2.Add(new OBD2KWPECU());
								List<KWPECU> list3 = KWPECU.BuildDetectedECUs();
								list2.AddRange(list3);
								list2.AddRange(RenaultKWPECU.ECUs);
								List<IECU> list4 = RenaultDaciaCAN11bitECU.ECUs.ToList<IECU>();
								list4.RemoveAll((IECU x) => x is OBD2Can11bitECU);
								list2.AddRange(list4);
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(list2);
							}
							break;
						}
						case ELMFormat.CAN11bit:
						{
							string text = brand;
							if (text != null)
							{
								switch (text.Length)
								{
								case 2:
								{
									char c = text[0];
									if (c != 'D')
									{
										if (c != 'M')
										{
											goto IL_1089;
										}
										if (!(text == "MG"))
										{
											goto IL_1089;
										}
										dtcv2Model.ECUListFull = new ObservableCollection<IECU>(MGZSEV.ECUs);
										goto IL_164D;
									}
									else
									{
										if (!(text == "DS"))
										{
											goto IL_1089;
										}
										goto IL_0C22;
									}
									break;
								}
								case 3:
								{
									char c = text[0];
									if (c > 'K')
									{
										switch (c)
										{
										case 'O':
											if (!(text == "Ora"))
											{
												goto IL_1089;
											}
											goto IL_0F8D;
										case 'P':
										case 'Q':
										case 'S':
										case 'T':
											goto IL_1089;
										case 'R':
											if (!(text == "RAM"))
											{
												goto IL_1089;
											}
											goto IL_0C4C;
										case 'U':
											if (!(text == "UAZ"))
											{
												goto IL_1089;
											}
											break;
										case 'V':
											if (!(text == "VAZ"))
											{
												goto IL_1089;
											}
											goto IL_0C61;
										case 'W':
											if (!(text == "WEY"))
											{
												goto IL_1089;
											}
											goto IL_0F8D;
										default:
											if (c != 'В')
											{
												if (c != 'У')
												{
													goto IL_1089;
												}
												if (!(text == "УАЗ"))
												{
													goto IL_1089;
												}
											}
											else
											{
												if (!(text == "ВАЗ"))
												{
													goto IL_1089;
												}
												goto IL_0C61;
											}
											break;
										}
										dtcv2Model.ECUListFull = new ObservableCollection<IECU>(UAZCAN11bit.ECUs);
										goto IL_164D;
									}
									if (c != 'B')
									{
										switch (c)
										{
										case 'F':
											if (!(text == "FAW"))
											{
												goto IL_1089;
											}
											dtcv2Model.ECUListFull = new ObservableCollection<IECU>(FAWCANECU.ECUs);
											goto IL_164D;
										case 'G':
											if (text == "GMC")
											{
												goto IL_0F4E;
											}
											if (!(text == "GAC"))
											{
												goto IL_1089;
											}
											goto IL_1035;
										case 'H':
										case 'I':
											goto IL_1089;
										case 'J':
											if (!(text == "JAC"))
											{
												goto IL_1089;
											}
											goto IL_0FE1;
										case 'K':
											if (text == "Kia")
											{
												goto IL_0BE3;
											}
											if (!(text == "KTM"))
											{
												goto IL_1089;
											}
											dtcv2Model.ECUListFull = new ObservableCollection<IECU>(KTMCANECU.ECUs);
											goto IL_164D;
										default:
											goto IL_1089;
										}
									}
									else
									{
										if (text == "BMW")
										{
											goto IL_0C0D;
										}
										if (!(text == "BYD") && !(text == "Byd"))
										{
											goto IL_1089;
										}
										dtcv2Model.ECUListFull = new ObservableCollection<IECU>(BydCANECU.ECUs);
										goto IL_164D;
									}
									break;
								}
								case 4:
								{
									char c = text[3];
									if (c <= 'i')
									{
										if (c <= 'K')
										{
											if (c != 'G')
											{
												if (c != 'K')
												{
													goto IL_1089;
												}
												if (!(text == "DFSK"))
												{
													goto IL_1089;
												}
												goto IL_104A;
											}
											else
											{
												if (!(text == "DFFG"))
												{
													goto IL_1089;
												}
												goto IL_104A;
											}
										}
										else
										{
											switch (c)
											{
											case 'a':
												if (!(text == "Lada"))
												{
													goto IL_1089;
												}
												goto IL_0C61;
											case 'b':
												if (!(text == "Saab"))
												{
													goto IL_1089;
												}
												goto IL_0F4E;
											case 'c':
												goto IL_1089;
											case 'd':
												if (!(text == "Ford"))
												{
													goto IL_1089;
												}
												goto IL_0C37;
											default:
												if (c != 'i')
												{
													goto IL_1089;
												}
												if (!(text == "Audi"))
												{
													if (!(text == "Mini"))
													{
														goto IL_1089;
													}
													goto IL_0C0D;
												}
												break;
											}
										}
									}
									else if (c <= 'p')
									{
										if (c != 'l')
										{
											if (c != 'p')
											{
												goto IL_1089;
											}
											if (!(text == "Jeep"))
											{
												goto IL_1089;
											}
											goto IL_0C4C;
										}
										else
										{
											if (!(text == "Opel"))
											{
												goto IL_1089;
											}
											goto IL_0F4E;
										}
									}
									else if (c != 't')
									{
										if (c != 'а')
										{
											goto IL_1089;
										}
										if (!(text == "Лада"))
										{
											goto IL_1089;
										}
										goto IL_0C61;
									}
									else if (!(text == "Seat"))
									{
										goto IL_1089;
									}
									break;
								}
								case 5:
								{
									char c = text[2];
									if (c != 'U')
									{
										switch (c)
										{
										case 'c':
											if (!(text == "Dacia"))
											{
												goto IL_1089;
											}
											goto IL_0C8B;
										case 'd':
											if (!(text == "Dodge"))
											{
												goto IL_1089;
											}
											goto IL_0C4C;
										case 'e':
											if (text == "Chery" || text == "Exeed")
											{
												goto IL_0F0F;
											}
											if (text == "Geely")
											{
												dtcv2Model.ECUListFull = new ObservableCollection<IECU>(GeelyCANECU.ECUs);
												goto IL_164D;
											}
											if (!(text == "Chevy"))
											{
												goto IL_1089;
											}
											goto IL_0F4E;
										case 'f':
											if (!(text == "Lifan"))
											{
												goto IL_1089;
											}
											dtcv2Model.ECUListFull = new ObservableCollection<IECU>(LifanECUCAN11bit.ECUs);
											goto IL_164D;
										case 'g':
										case 'j':
										case 'k':
										case 'm':
										case 'n':
										case 'q':
										case 'r':
										case 's':
										case 'w':
										case 'y':
											goto IL_1089;
										case 'h':
											if (!(text == "Sehol"))
											{
												goto IL_1089;
											}
											goto IL_0FE1;
										case 'i':
											if (text == "Kaiyi")
											{
												goto IL_0F0F;
											}
											if (!(text == "Buick"))
											{
												goto IL_1089;
											}
											goto IL_0F4E;
										case 'l':
											if (text == "Volvo")
											{
												text = SharedSettings.Current.ProfileUpdateAlias;
												if (text != null)
												{
													int length = text.Length;
													if (length <= 11)
													{
														if (length != 8)
														{
															if (length != 11)
															{
																goto IL_0EB1;
															}
															if (!(text == "Volvo 2010+"))
															{
																goto IL_0EB1;
															}
														}
														else if (!(text == "D2 (1.6)"))
														{
															goto IL_0EB1;
														}
													}
													else
													{
														if (length == 32)
														{
															c = text[5];
															if (c <= '6')
															{
																if (c != ' ')
																{
																	switch (c)
																	{
																	case '0':
																		if (!(text == "7747e05956024118aaba6db5b89100fa"))
																		{
																			goto IL_0EB1;
																		}
																		goto IL_0E87;
																	case '1':
																		if (!(text == "7a29617cc40446609b8396baf0f3af53"))
																		{
																			goto IL_0EB1;
																		}
																		goto IL_0E87;
																	case '2':
																		if (!(text == "d039520dffb647318b17a78daffbea5d"))
																		{
																			goto IL_0EB1;
																		}
																		break;
																	case '3':
																		if (!(text == "4b6d63d2e23f438eb11a6f5ff7267a93") && !(text == "83ffc37f09394359aba236bc11854e14"))
																		{
																			goto IL_0EB1;
																		}
																		goto IL_0E87;
																	case '4':
																		if (!(text == "ead764bfaeb7421fa210abc9f19a59fc"))
																		{
																			goto IL_0EB1;
																		}
																		break;
																	case '5':
																		goto IL_0EB1;
																	case '6':
																		if (!(text == "27fbf62a5f9845e791a413f387e060d3"))
																		{
																			goto IL_0EB1;
																		}
																		break;
																	default:
																		goto IL_0EB1;
																	}
																}
																else
																{
																	if (!(text == "Volvo 2014+ VEA engine (not SPA)"))
																	{
																		goto IL_0EB1;
																	}
																	goto IL_0E87;
																}
															}
															else if (c != 'd')
															{
																if (c != 'f')
																{
																	goto IL_0EB1;
																}
																if (!(text == "6c80ff6e03e846a7a16922fc7a4a0359"))
																{
																	goto IL_0EB1;
																}
															}
															else if (!(text == "76d64d01184541249adb0da64f0d421a"))
															{
																goto IL_0EB1;
															}
															dtcv2Model.ECUListFull = new ObservableCollection<IECU>(VolvoSPA29bitECU.ECUs);
															goto IL_164D;
														}
														if (length != 35)
														{
															goto IL_0EB1;
														}
														if (!(text == "Volvo S80-V70 with 2.0D Ford engine"))
														{
															goto IL_0EB1;
														}
													}
													IL_0E87:
													dtcv2Model.ECUListFull = new ObservableCollection<IECU>(VolvoCAN11bitECU.ECUs);
													goto IL_164D;
												}
												IL_0EB1:
												dtcv2Model.ECUListFull = new ObservableCollection<IECU>(VolvoCAN11bitECU.ECUs.Concat(VolvoSPA29bitECU.ECUs));
												goto IL_164D;
											}
											if (!(text == "Euler"))
											{
												goto IL_1089;
											}
											goto IL_0F8D;
										case 'o':
											if (text == "Skoda")
											{
												goto IL_0B99;
											}
											if (!(text == "Omoda"))
											{
												goto IL_1089;
											}
											goto IL_0F0F;
										case 'p':
											if (!(text == "Cupra"))
											{
												goto IL_1089;
											}
											goto IL_0B99;
										case 't':
											if (!(text == "Jetta"))
											{
												goto IL_1089;
											}
											goto IL_0B99;
										case 'u':
											if (!(text == "Isuzu"))
											{
												goto IL_1089;
											}
											break;
										case 'v':
											if (text == "Ravon")
											{
												goto IL_0F4E;
											}
											if (!(text == "Haval") && !(text == "Hover"))
											{
												goto IL_1089;
											}
											goto IL_0F8D;
										case 'x':
											if (!(text == "Lexus"))
											{
												goto IL_1089;
											}
											goto IL_0BF8;
										case 'z':
											if (!(text == "Mazda"))
											{
												goto IL_1089;
											}
											dtcv2Model.ECUListFull = new ObservableCollection<IECU>(MazdaECUCAN11bit.ECUs);
											goto IL_164D;
										default:
											goto IL_1089;
										}
									}
									else if (!(text == "ISUZU"))
									{
										goto IL_1089;
									}
									dtcv2Model.ECUListFull = new ObservableCollection<IECU>(ISUZUCAN11bitECU.ECUs);
									goto IL_164D;
								}
								case 6:
								{
									char c = text[2];
									if (c <= 'g')
									{
										if (c != 'b')
										{
											if (c != 'e')
											{
												if (c != 'g')
												{
													goto IL_1089;
												}
												if (!(text == "Jaguar"))
												{
													goto IL_1089;
												}
												goto IL_0EE5;
											}
											else
											{
												if (!(text == "Daewoo"))
												{
													goto IL_1089;
												}
												goto IL_0F4E;
											}
										}
										else
										{
											if (!(text == "Subaru"))
											{
												goto IL_1089;
											}
											dtcv2Model.ECUListFull = new ObservableCollection<IECU>(SubaruCANECU.ECUs);
											goto IL_164D;
										}
									}
									else
									{
										switch (c)
										{
										case 'l':
											if (!(text == "Holden"))
											{
												goto IL_1089;
											}
											goto IL_0F4E;
										case 'm':
											if (text == "Hummer")
											{
												goto IL_0F4E;
											}
											if (!(text == "Yamaha"))
											{
												goto IL_1089;
											}
											dtcv2Model.ECUListFull = new ObservableCollection<IECU>(YamahaCANECU.ECUs);
											goto IL_164D;
										case 'n':
											if (!(text == "Hongqi"))
											{
												goto IL_1089;
											}
											dtcv2Model.ECUListFull = new ObservableCollection<IECU>(HongqiCANECU.ECUs);
											goto IL_164D;
										case 'o':
										case 'p':
										case 'q':
										case 'r':
											goto IL_1089;
										case 's':
											if (!(text == "Nissan"))
											{
												goto IL_1089;
											}
											goto IL_0BB9;
										case 't':
											if (text == "Jetour")
											{
												goto IL_0F0F;
											}
											if (!(text == "Saturn"))
											{
												goto IL_1089;
											}
											goto IL_0F4E;
										default:
											if (c != 'y')
											{
												if (c != 'z')
												{
													goto IL_1089;
												}
												if (!(text == "Suzuki"))
												{
													goto IL_1089;
												}
												dtcv2Model.ECUListFull = new ObservableCollection<IECU>(SuzukiCAN.ECUs);
												goto IL_164D;
											}
											else
											{
												if (!(text == "Toyota"))
												{
													goto IL_1089;
												}
												goto IL_0BF8;
											}
											break;
										}
									}
									break;
								}
								case 7:
								{
									char c = text[4];
									switch (c)
									{
									case 'c':
										if (!(text == "Porsche"))
										{
											goto IL_1089;
										}
										break;
									case 'd':
										if (!(text == "Hyundai"))
										{
											goto IL_1089;
										}
										goto IL_0BE3;
									case 'e':
										if (!(text == "Peugeot"))
										{
											goto IL_1089;
										}
										goto IL_0C22;
									case 'f':
									case 'h':
									case 'j':
									case 'k':
									case 'm':
									case 'n':
									case 'p':
									case 'q':
									case 'r':
										goto IL_1089;
									case 'g':
										if (!(text == "Changan"))
										{
											goto IL_1089;
										}
										dtcv2Model.ECUListFull = new ObservableCollection<IECU>(ChanganCANECU.ECUs);
										goto IL_164D;
									case 'i':
										if (!(text == "Pontiac"))
										{
											goto IL_1089;
										}
										goto IL_0F4E;
									case 'l':
										if (!(text == "Bentley"))
										{
											goto IL_1089;
										}
										break;
									case 'o':
										if (text == "Citroen")
										{
											goto IL_0C22;
										}
										if (!(text == "Lincoln"))
										{
											goto IL_1089;
										}
										goto IL_0C37;
									case 's':
										if (!(text == "Genesis"))
										{
											goto IL_1089;
										}
										goto IL_0BE3;
									case 't':
										if (!(text == "Bugatti"))
										{
											goto IL_1089;
										}
										break;
									case 'u':
										if (text == "Mercury")
										{
											goto IL_0C37;
										}
										if (text == "Renault")
										{
											goto IL_0C8B;
										}
										if (!(text == "Evolute"))
										{
											goto IL_1089;
										}
										goto IL_104A;
									default:
										if (c != 'в')
										{
											goto IL_1089;
										}
										if (!(text == "Москвич"))
										{
											goto IL_1089;
										}
										goto IL_0FE1;
									}
									break;
								}
								case 8:
								{
									char c = text[4];
									if (c <= 'h')
									{
										if (c != 'f')
										{
											if (c != 'h')
											{
												goto IL_1089;
											}
											if (!(text == "Vauxhall"))
											{
												goto IL_1089;
											}
											goto IL_0F4E;
										}
										else
										{
											if (!(text == "Dongfeng"))
											{
												goto IL_1089;
											}
											goto IL_104A;
										}
									}
									else
									{
										switch (c)
										{
										case 'l':
											if (!(text == "Cadillac"))
											{
												goto IL_1089;
											}
											goto IL_0F4E;
										case 'm':
										case 'o':
											goto IL_1089;
										case 'n':
											if (!(text == "Infiniti") && !(text == "Infinity"))
											{
												goto IL_1089;
											}
											goto IL_0BB9;
										case 'p':
											if (!(text == "Trumpchi"))
											{
												goto IL_1089;
											}
											goto IL_1035;
										default:
											if (c != 's')
											{
												goto IL_1089;
											}
											if (!(text == "Chrysler"))
											{
												goto IL_1089;
											}
											goto IL_0C4C;
										}
									}
									break;
								}
								case 9:
								{
									char c = text[0];
									if (c != 'C')
									{
										if (c != 'S')
										{
											goto IL_1089;
										}
										if (!(text == "SsangYong"))
										{
											goto IL_1089;
										}
										dtcv2Model.ECUListFull = new ObservableCollection<IECU>(SsangYongCANECU.ECUs);
										goto IL_164D;
									}
									else
									{
										if (!(text == "Chevrolet"))
										{
											goto IL_1089;
										}
										goto IL_0F4E;
									}
									break;
								}
								case 10:
								{
									char c = text[0];
									if (c <= 'L')
									{
										if (c != 'G')
										{
											if (c != 'L')
											{
												goto IL_1089;
											}
											if (!(text == "Land Rover"))
											{
												goto IL_1089;
											}
											goto IL_0EE5;
										}
										else
										{
											if (!(text == "Great Wall"))
											{
												goto IL_1089;
											}
											goto IL_0F8D;
										}
									}
									else if (c != 'M')
									{
										if (c != 'V')
										{
											goto IL_1089;
										}
										if (!(text == "Volkswagen"))
										{
											goto IL_1089;
										}
									}
									else
									{
										if (!(text == "Mitsubishi"))
										{
											goto IL_1089;
										}
										dtcv2Model.ECUListFull = new ObservableCollection<IECU>(MitsubishiCANECU.ECUs);
										goto IL_164D;
									}
									break;
								}
								case 11:
									if (!(text == "Range Rover"))
									{
										goto IL_1089;
									}
									goto IL_0EE5;
								case 12:
									goto IL_1089;
								case 13:
									if (!(text == "Mercedes-Benz"))
									{
										goto IL_1089;
									}
									dtcv2Model.ECUListFull = new ObservableCollection<IECU>(MercedesBenzCANECU.ECUs);
									goto IL_164D;
								case 14:
									if (!(text == "General Motors"))
									{
										goto IL_1089;
									}
									goto IL_0F4E;
								default:
									goto IL_1089;
								}
								IL_0B99:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(VAGECU.ECUs);
								dtcv2Model.SupportedUnitsDetector = new VagUDSSupportedUnitsDetector();
								break;
								IL_0BB9:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(NissanCANECU.ECUs);
								break;
								IL_0BE3:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(HyundaiKiaCANECU.ECUs);
								break;
								IL_0BF8:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(ToyotaCANECU.ECUs);
								break;
								IL_0C0D:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(BMWGatewayCANECU.ECUs);
								break;
								IL_0C22:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(CitroenPeugeotCAN11bit.ECUs);
								break;
								IL_0C37:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(FordCANECU.ECUs);
								break;
								IL_0C4C:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(JeepChryslerDodgeCANECU.ECUs);
								break;
								IL_0C61:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(LadaECUCAN.ECUs);
								break;
								IL_0C8B:
								List<IECU> list5 = Renault29bitCANECU.ECUs.ToList<IECU>();
								IReadOnlyList<IECU> ecus = RenaultDaciaCAN11bitECU.ECUs;
								list5.RemoveAll((IECU x) => x is OBD2Can29bitECU);
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(ecus.Concat(list5));
								break;
								IL_0EE5:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(LandRoverECUCAN11bit.ECUs);
								break;
								IL_0F0F:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(CheryExeedECU.ECUs);
								break;
								IL_0F4E:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(GMCANECU.ECUs);
								break;
								IL_0F8D:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(HavalCANECU.ECUs);
								break;
								IL_0FE1:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(JACCAN11bitECU.ECUs);
								break;
								IL_1035:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(GACCAN11bitECU.ECUs);
								break;
								IL_104A:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(DongFengCAN11bitECU.ECUs);
								break;
							}
							IL_1089:
							dtcv2Model.ECUListFull = new ObservableCollection<IECU>(GenericCAN11bitBrand.ECUs);
							break;
						}
						case ELMFormat.CAN29bit:
						{
							string text = brand;
							if (text != null)
							{
								switch (text.Length)
								{
								case 4:
								{
									char c = text[0];
									if (c <= 'F')
									{
										if (c != 'A')
										{
											if (c != 'F')
											{
												goto IL_164D;
											}
											if (!(text == "Fiat"))
											{
												goto IL_164D;
											}
											goto IL_1588;
										}
										else if (!(text == "Audi"))
										{
											goto IL_164D;
										}
									}
									else if (c != 'J')
									{
										if (c != 'S')
										{
											goto IL_164D;
										}
										if (!(text == "Seat"))
										{
											goto IL_164D;
										}
									}
									else
									{
										if (!(text == "Jeep"))
										{
											goto IL_164D;
										}
										goto IL_1588;
									}
									break;
								}
								case 5:
								{
									char c = text[2];
									switch (c)
									{
									case 'c':
										if (!(text == "Dacia"))
										{
											goto IL_164D;
										}
										goto IL_159D;
									case 'd':
										if (!(text == "Dodge"))
										{
											goto IL_164D;
										}
										goto IL_1588;
									case 'e':
										if (!(text == "Geely"))
										{
											goto IL_164D;
										}
										goto IL_163D;
									default:
										switch (c)
										{
										case 'l':
											if (!(text == "Volvo"))
											{
												goto IL_164D;
											}
											goto IL_163D;
										case 'm':
										case 'q':
										case 'r':
										case 's':
											goto IL_164D;
										case 'n':
											if (!(text == "Honda"))
											{
												goto IL_164D;
											}
											break;
										case 'o':
											if (!(text == "Skoda"))
											{
												goto IL_164D;
											}
											goto IL_1568;
										case 'p':
											if (!(text == "Cupra"))
											{
												goto IL_164D;
											}
											goto IL_1568;
										case 't':
											if (!(text == "Jetta"))
											{
												goto IL_164D;
											}
											goto IL_1568;
										case 'u':
											if (!(text == "Acura"))
											{
												goto IL_164D;
											}
											break;
										default:
											goto IL_164D;
										}
										dtcv2Model.ECUListFull = new ObservableCollection<IECU>(HondaECU29bit.ECUs);
										goto IL_164D;
									}
									break;
								}
								case 6:
								{
									if (!(text == "Nissan"))
									{
										goto IL_164D;
									}
									IReadOnlyList<IECU> ecus2 = Renault29bitCANECU.ECUs;
									List<IECU> list6 = NissanCANECU.ECUs.ToList<IECU>();
									list6.RemoveAll((IECU x) => x is OBD2Can11bitECU);
									dtcv2Model.ECUListFull = new ObservableCollection<IECU>(ecus2.Concat(list6));
									goto IL_164D;
								}
								case 7:
								{
									char c = text[0];
									if (c != 'P')
									{
										if (c != 'R')
										{
											goto IL_164D;
										}
										if (!(text == "Renault"))
										{
											goto IL_164D;
										}
										goto IL_159D;
									}
									else if (!(text == "Porsche"))
									{
										goto IL_164D;
									}
									break;
								}
								case 8:
									goto IL_164D;
								case 9:
									if (!(text == "Lynk & Co"))
									{
										goto IL_164D;
									}
									goto IL_163D;
								case 10:
									if (!(text == "Volkswagen"))
									{
										goto IL_164D;
									}
									break;
								default:
									goto IL_164D;
								}
								IL_1568:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(VAGECU.ECUs);
								dtcv2Model.SupportedUnitsDetector = new VagUDSSupportedUnitsDetector();
								break;
								IL_1588:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(FiatCAN29bitECU.ECUs);
								break;
								IL_159D:
								IReadOnlyList<IECU> ecus3 = Renault29bitCANECU.ECUs;
								List<IECU> list7 = RenaultDaciaCAN11bitECU.ECUs.ToList<IECU>();
								list7.RemoveAll((IECU x) => x is OBD2Can11bitECU);
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(ecus3.Concat(list7));
								break;
								IL_163D:
								dtcv2Model.ECUListFull = new ObservableCollection<IECU>(VolvoSPA29bitECU.ECUs);
							}
							break;
						}
						}
						IL_164D:
						if (dtcv2Model.ECUListFull == null)
						{
							dtcv2Model.ECUListFull = new ObservableCollection<IECU>();
							if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
							{
								DTCv2Model.<>c__DisplayClass3_0 CS$<>8__locals1 = new DTCv2Model.<>c__DisplayClass3_0();
								dtcv2Model.ECUListFull.Add(new OBD2KWPECU());
								List<KWPECU>.Enumerator enumerator = KWPECU.BuildDetectedECUs().GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										KWPECU kwpecu = enumerator.Current;
										dtcv2Model.ECUListFull.Add(kwpecu);
									}
								}
								finally
								{
									if (num < 0)
									{
										((IDisposable)enumerator).Dispose();
									}
								}
								string[] array = new string[0];
								switch (App.OBDReader.CurrentProtocolNumber)
								{
								case 1:
									array = new string[] { "6158F1", "616AF1", "6110F1", "6118F1", "616AF1" };
									break;
								case 2:
									array = new string[] { "6C10F1", "6C58F1", "6C40F1", "6C1AF1", "6818F1", "6810F1", "6858F1", "6828F1", "686AF1" };
									break;
								case 3:
									array = new string[] { "6828F1", "6858F1", "6818F1", "686AF1", "6810F1", "6812F1", "687AF1" };
									break;
								case 4:
								case 5:
									array = new string[]
									{
										"8110F1", "8111F1", "8112F1", "8113F1", "8114F1", "8658F1", "C241F1", "C218F1", "C230F1", "C258F1",
										"C228F1", "C233F1", "8116F1", "8118F1", "811AF1", "807AF1"
									};
									break;
								}
								CS$<>8__locals1.readDTCCommands = new List<string>
								{
									"03", "03", "07", "07", "0A", "1800FF00", "1802FF00", "1802FFFF", "1800FFFF", "18FF00",
									"17FF00", "13FF00", "1902AF", "1902AC", "19028D", "190223", "190278", "190208", "190FAC", "190F8D",
									"190F23", "19D2FF00"
								};
								CS$<>8__locals1.clearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFFFF", "140000" };
								enumerator = array.Select((string header) => new KWPECU
								{
									Name = "ECU Address $" + header.Substring(2, 2),
									RequestHeader = header,
									ReadDTCCommands = CS$<>8__locals1.readDTCCommands,
									ClearDTCCommands = CS$<>8__locals1.clearDTCCommands,
									TestIfEcuExists = false
								}).ToList<KWPECU>().GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										KWPECU kwpecu2 = enumerator.Current;
										DTCv2Model.<>c__DisplayClass3_1 CS$<>8__locals2 = new DTCv2Model.<>c__DisplayClass3_1();
										CS$<>8__locals2.ecuAddress = kwpecu2.RequestHeader.Substring(2, 2);
										if (!dtcv2Model.ECUListFull.Any((IECU x) => x.RequestHeader != null && x.RequestHeader.Length == 6 && x.RequestHeader.Substring(2, 2) == CS$<>8__locals2.ecuAddress))
										{
											dtcv2Model.ECUListFull.Add(kwpecu2);
										}
									}
									goto IL_1A74;
								}
								finally
								{
									if (num < 0)
									{
										((IDisposable)enumerator).Dispose();
									}
								}
							}
							if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
							{
								dtcv2Model.ECUListFull.Add(new OBD2Can11bitECU());
							}
							else if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
							{
								dtcv2Model.ECUListFull.Add(new OBD2Can29bitECU());
							}
						}
						IL_1A74:
						taskAwaiter = dtcv2Model.DetectSupportedUnits().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<Initialize>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					string dtcfoundECUs = SharedSettings.Current.DTCFoundECUs;
					IEnumerator<IECU> enumerator2 = dtcv2Model.ECUListFull.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IECU iecu = enumerator2.Current;
							iecu.IsSelected = false;
						}
					}
					finally
					{
						if (num < 0 && enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					if (string.IsNullOrEmpty(dtcfoundECUs) || dtcfoundECUs == "[]")
					{
						enumerator2 = dtcv2Model.ECUListToDisplay.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								IECU iecu2 = enumerator2.Current;
								iecu2.IsSelected = true;
							}
							goto IL_1BFD;
						}
						finally
						{
							if (num < 0 && enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					try
					{
						int[] array2 = JsonConvert.DeserializeObject<int[]>(dtcfoundECUs);
						enumerator2 = dtcv2Model.ECUListToDisplay.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								IECU iecu3 = enumerator2.Current;
								if (array2.Contains(iecu3.GetHashCode()))
								{
									iecu3.IsSelected = true;
								}
								else
								{
									iecu3.IsSelected = false;
								}
							}
						}
						finally
						{
							if (num < 0 && enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					catch (Exception)
					{
						enumerator2 = dtcv2Model.ECUListToDisplay.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								IECU iecu4 = enumerator2.Current;
								iecu4.IsSelected = true;
							}
						}
						finally
						{
							if (num < 0 && enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					IL_1BFD:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600346C RID: 13420 RVA: 0x00249450 File Offset: 0x00247650
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001EF2 RID: 7922
			public int <>1__state;

			// Token: 0x04001EF3 RID: 7923
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001EF4 RID: 7924
			public string brand;

			// Token: 0x04001EF5 RID: 7925
			public DTCv2Model <>4__this;

			// Token: 0x04001EF6 RID: 7926
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200059F RID: 1439
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadDTC>d__103 : IAsyncStateMachine
		{
			// Token: 0x0600346D RID: 13421 RVA: 0x00249460 File Offset: 0x00247660
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				DTCv2Model dtcv2Model = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						break;
					case 1:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_0185;
					case 2:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_01E3;
					case 3:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_0322;
					case 4:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_05AA;
					default:
						if (dtcv2Model.IsBusy)
						{
							goto IL_0746;
						}
						if (!dtcv2Model.ECUListFull.Any((IECU x) => x.IsSelected))
						{
							Page currentPage = App.GetCurrentPage();
							if (!(currentPage is IDTCvXPage))
							{
								goto IL_00EC;
							}
							taskAwaiter = currentPage.DisplayAlert(Translate.GetString("dtc_NoECUSelected_Title"), Translate.GetString("dtc_NoECUSelected_Text"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (this.<>1__state = 0);
								this.<>u__1 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadDTC>d__103>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							bool isMainThread = MainThread.IsMainThread;
							dtcv2Model.IsCancelButtonEnabled = true;
							dtcv2Model.ECUListFound.Clear();
							dtcv2Model.HasDTCToDisplay = true;
							dtcv2Model.IsResults = true;
							dtcv2Model.BadELMDetected = false;
							dtcv2Model.IsSetup = false;
							taskAwaiter = App.OBDReader.DebugWrite("\r\n[DTCv2Read]\r\n").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (this.<>1__state = 1);
								this.<>u__1 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadDTC>d__103>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0185;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_00EC:
					goto IL_0746;
					IL_0185:
					taskAwaiter.GetResult();
					taskAwaiter = dtcv2Model.CheckAndRestoreECUConnection().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (this.<>1__state = 2);
						this.<>u__1 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadDTC>d__103>(ref taskAwaiter, ref this);
						return;
					}
					IL_01E3:
					taskAwaiter.GetResult();
					dtcv2Model.IsBusy = true;
					dtcv2Model.cancellationTokenSource = new CancellationTokenSource();
					this.<token>5__2 = dtcv2Model.cancellationTokenSource.Token;
					if (selectedECUs == null)
					{
						selectedECUs = dtcv2Model.ECUListToDisplay.Where((IECU x) => x.IsSelected).ToList<IECU>();
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("\r\n[Selected:");
					List<IECU>.Enumerator enumerator = selectedECUs.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IECU iecu = enumerator.Current;
							stringBuilder.Append(iecu.Name);
							stringBuilder.Append(",");
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					stringBuilder.Append("]\r\n");
					taskAwaiter = App.OBDReader.DebugWrite(Encoding.UTF8.GetBytes(stringBuilder.ToString())).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (this.<>1__state = 3);
						this.<>u__1 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadDTC>d__103>(ref taskAwaiter, ref this);
						return;
					}
					IL_0322:
					taskAwaiter.GetResult();
					IEnumerator<IECU> enumerator2 = dtcv2Model.ECUListToDisplay.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IECU iecu2 = enumerator2.Current;
							iecu2.Reset();
						}
					}
					finally
					{
						if (num < 0 && enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					IECU iecu3 = selectedECUs.FirstOrDefault((IECU x) => !string.IsNullOrEmpty(x.RequestHeader));
					if (iecu3 != null)
					{
						iecu3.TestELMDevice = true;
					}
					this.<badELMDetectedAction>5__3 = delegate
					{
						base.BadELMDetected = true;
					};
					string selectedBrand = SharedSettings.Current.SelectedBrand;
					if (selectedBrand != null)
					{
						switch (selectedBrand.Length)
						{
						case 4:
						{
							char c = selectedBrand[0];
							if (c != 'A')
							{
								if (c != 'S')
								{
									goto IL_04C7;
								}
								if (!(selectedBrand == "Seat"))
								{
									goto IL_04C7;
								}
							}
							else if (!(selectedBrand == "Audi"))
							{
								goto IL_04C7;
							}
							break;
						}
						case 5:
						{
							char c = selectedBrand[0];
							if (c != 'C')
							{
								if (c != 'S')
								{
									goto IL_04C7;
								}
								if (!(selectedBrand == "Skoda"))
								{
									goto IL_04C7;
								}
							}
							else if (!(selectedBrand == "Cupra"))
							{
								goto IL_04C7;
							}
							break;
						}
						case 6:
						case 8:
						case 9:
							goto IL_04C7;
						case 7:
						{
							char c = selectedBrand[1];
							if (c != 'e')
							{
								if (c != 'o')
								{
									if (c != 'u')
									{
										goto IL_04C7;
									}
									if (!(selectedBrand == "Bugatti"))
									{
										goto IL_04C7;
									}
								}
								else if (!(selectedBrand == "Porsche"))
								{
									goto IL_04C7;
								}
							}
							else if (!(selectedBrand == "Bentley"))
							{
								goto IL_04C7;
							}
							break;
						}
						case 10:
							if (!(selectedBrand == "Volkswagen"))
							{
								goto IL_04C7;
							}
							break;
						default:
							goto IL_04C7;
						}
						VagDTCDecoder.UnpackContainers(false);
					}
					IL_04C7:
					this.<i>5__4 = 0;
					goto IL_0613;
					IL_05AA:
					taskAwaiter.GetResult();
					this.<ecu>5__5.UpdateCollection();
					try
					{
						if (this.<ecu>5__5.Count<DTCItemV2>() > 0)
						{
							dtcv2Model.ECUListFound.Add(this.<ecu>5__5);
						}
					}
					catch (Exception)
					{
					}
					if (dtcv2Model.cancellationTokenSource.IsCancellationRequested || this.<token>5__2.IsCancellationRequested)
					{
						goto IL_0629;
					}
					this.<ecu>5__5 = null;
					IL_0601:
					int num2 = this.<i>5__4;
					this.<i>5__4 = num2 + 1;
					IL_0613:
					if (this.<i>5__4 < selectedECUs.Count)
					{
						this.<ecu>5__5 = selectedECUs[this.<i>5__4];
						if (!this.<ecu>5__5.IsSelected)
						{
							goto IL_0601;
						}
						dtcv2Model.StatusText = string.Format("{0}/{1}", this.<i>5__4 + 1, selectedECUs.Count) + "\n" + this.<ecu>5__5.Name;
						taskAwaiter = this.<ecu>5__5.ReadDTCAsync(selectedECUs, this.<badELMDetectedAction>5__3, this.<token>5__2).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (this.<>1__state = 4);
							this.<>u__1 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadDTC>d__103>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_05AA;
					}
					IL_0629:
					bool isMainThread2 = MainThread.IsMainThread;
					dtcv2Model.IsCancelButtonEnabled = true;
					if (!this.<token>5__2.IsCancellationRequested)
					{
						List<IECU> list = dtcv2Model.ECUListFull.Where((IECU x) => x.ECUExists).ToList<IECU>();
						int[] array = list.Select((IECU x) => x.GetHashCode()).ToArray<int>();
						SharedSettings.Current.DTCFoundECUs = JsonConvert.SerializeObject(array);
						enumerator2 = dtcv2Model.ECUListToDisplay.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								IECU iecu4 = enumerator2.Current;
								iecu4.IsSelected = list.Contains(iecu4);
							}
						}
						finally
						{
							if (num < 0 && enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					if (dtcv2Model.ECUListFound.Count == 0)
					{
						dtcv2Model.HasDTCToDisplay = false;
					}
					VagDTCDecoder.ClearResources();
					dtcv2Model.IsBusy = false;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<token>5__2 = default(CancellationToken);
					this.<badELMDetectedAction>5__3 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0746:
				this.<>1__state = -2;
				this.<token>5__2 = default(CancellationToken);
				this.<badELMDetectedAction>5__3 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600346E RID: 13422 RVA: 0x00249C58 File Offset: 0x00247E58
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001EF7 RID: 7927
			public int <>1__state;

			// Token: 0x04001EF8 RID: 7928
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001EF9 RID: 7929
			public DTCv2Model <>4__this;

			// Token: 0x04001EFA RID: 7930
			public List<IECU> selectedECUs;

			// Token: 0x04001EFB RID: 7931
			private CancellationToken <token>5__2;

			// Token: 0x04001EFC RID: 7932
			private Action <badELMDetectedAction>5__3;

			// Token: 0x04001EFD RID: 7933
			private TaskAwaiter <>u__1;

			// Token: 0x04001EFE RID: 7934
			private int <i>5__4;

			// Token: 0x04001EFF RID: 7935
			private IECU <ecu>5__5;
		}

		// Token: 0x020005A0 RID: 1440
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadECUInfo>d__102 : IAsyncStateMachine
		{
			// Token: 0x0600346F RID: 13423 RVA: 0x00249C68 File Offset: 0x00247E68
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				DTCv2Model dtcv2Model = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> awaiter;
					switch (num)
					{
					case 0:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						break;
					case 1:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_01A4;
					case 2:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_0202;
					case 3:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_0341;
					case 4:
						awaiter = this.<>u__2;
						this.<>u__2 = default(TaskAwaiter<string>);
						num = (this.<>1__state = -1);
						goto IL_048A;
					case 5:
						taskAwaiter = this.<>u__1;
						this.<>u__1 = default(TaskAwaiter);
						num = (this.<>1__state = -1);
						goto IL_056D;
					default:
						if (dtcv2Model.IsBusy)
						{
							goto IL_06C1;
						}
						if (!dtcv2Model.ECUListToDisplay.Any((IECU x) => x.IsSelected))
						{
							Page currentPage = App.GetCurrentPage();
							if (!(currentPage is IDTCvXPage))
							{
								goto IL_00F0;
							}
							taskAwaiter = currentPage.DisplayAlert(Translate.GetString("dtc_NoECUSelected_Title"), Translate.GetString("dtc_NoECUSelected_Text"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (this.<>1__state = 0);
								this.<>u__1 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadECUInfo>d__102>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							dtcv2Model.ECUInfoReport.Spans.Clear();
							dtcv2Model.OnPropertyChanged("ECUInfoReport");
							bool isMainThread = MainThread.IsMainThread;
							dtcv2Model.IsCancelButtonEnabled = true;
							dtcv2Model.ECUListFound.Clear();
							dtcv2Model.HasDTCToDisplay = true;
							dtcv2Model.IsResults = true;
							dtcv2Model.BadELMDetected = false;
							dtcv2Model.IsSetup = false;
							taskAwaiter = App.OBDReader.DebugWrite("\r\n[ECUInfov2Read]\r\n").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (this.<>1__state = 1);
								this.<>u__1 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadECUInfo>d__102>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01A4;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_00F0:
					goto IL_06C1;
					IL_01A4:
					taskAwaiter.GetResult();
					taskAwaiter = dtcv2Model.CheckAndRestoreECUConnection().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (this.<>1__state = 2);
						this.<>u__1 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadECUInfo>d__102>(ref taskAwaiter, ref this);
						return;
					}
					IL_0202:
					taskAwaiter.GetResult();
					dtcv2Model.IsBusy = true;
					dtcv2Model.cancellationTokenSource = new CancellationTokenSource();
					this.<token>5__2 = dtcv2Model.cancellationTokenSource.Token;
					if (selectedECUs == null)
					{
						selectedECUs = dtcv2Model.ECUListToDisplay.Where((IECU x) => x.IsSelected).ToList<IECU>();
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("\r\n[Selected:");
					List<IECU>.Enumerator enumerator = selectedECUs.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IECU iecu = enumerator.Current;
							stringBuilder.Append(iecu.Name);
							stringBuilder.Append(",");
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					stringBuilder.Append("]\r\n");
					taskAwaiter = App.OBDReader.DebugWrite(Encoding.UTF8.GetBytes(stringBuilder.ToString())).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (this.<>1__state = 3);
						this.<>u__1 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadECUInfo>d__102>(ref taskAwaiter, ref this);
						return;
					}
					IL_0341:
					taskAwaiter.GetResult();
					IEnumerator<IECU> enumerator2 = dtcv2Model.ECUListToDisplay.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IECU iecu2 = enumerator2.Current;
							iecu2.Reset();
						}
					}
					finally
					{
						if (num < 0 && enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					IECU iecu3 = selectedECUs.FirstOrDefault((IECU x) => !string.IsNullOrEmpty(x.RequestHeader));
					if (iecu3 != null)
					{
						iecu3.TestELMDevice = true;
					}
					this.<i>5__3 = 0;
					goto IL_059A;
					IL_048A:
					string result = awaiter.GetResult();
					if (!string.IsNullOrEmpty(result))
					{
						dtcv2Model.ECUInfoReport.Spans.Add(new Span
						{
							Text = this.<ecu>5__4.Name + "\n",
							FontAttributes = 1
						});
						dtcv2Model.ECUInfoReport.Spans.Add(new Span
						{
							Text = result + "\n"
						});
						dtcv2Model.ECUListFound.Add(this.<ecu>5__4);
					}
					taskAwaiter = MainThread.InvokeOnMainThreadAsync(delegate
					{
						base.OnPropertyChanged("ECUInfoReport");
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (this.<>1__state = 5);
						this.<>u__1 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<ReadECUInfo>d__102>(ref taskAwaiter, ref this);
						return;
					}
					IL_056D:
					taskAwaiter.GetResult();
					if (this.<token>5__2.IsCancellationRequested)
					{
						goto IL_05B0;
					}
					this.<ecu>5__4 = null;
					IL_0588:
					int num2 = this.<i>5__3;
					this.<i>5__3 = num2 + 1;
					IL_059A:
					if (this.<i>5__3 < selectedECUs.Count)
					{
						this.<ecu>5__4 = selectedECUs[this.<i>5__3];
						if (!this.<ecu>5__4.IsSelected)
						{
							goto IL_0588;
						}
						dtcv2Model.StatusText = string.Format("{0}/{1}", this.<i>5__3 + 1, selectedECUs.Count) + "\n" + this.<ecu>5__4.Name;
						awaiter = this.<ecu>5__4.GetECUInformationReportAsync(this.<token>5__2).GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (this.<>1__state = 4);
							this.<>u__2 = awaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, DTCv2Model.<ReadECUInfo>d__102>(ref awaiter, ref this);
							return;
						}
						goto IL_048A;
					}
					IL_05B0:
					bool isMainThread2 = MainThread.IsMainThread;
					dtcv2Model.IsCancelButtonEnabled = true;
					if (!this.<token>5__2.IsCancellationRequested)
					{
						List<IECU> list = dtcv2Model.ECUListToDisplay.Where((IECU x) => x.ECUExists).ToList<IECU>();
						int[] array = list.Select((IECU x) => x.GetHashCode()).ToArray<int>();
						SharedSettings.Current.DTCFoundECUs = JsonConvert.SerializeObject(array);
						enumerator2 = dtcv2Model.ECUListFull.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								IECU iecu4 = enumerator2.Current;
								iecu4.IsSelected = list.Contains(iecu4);
							}
						}
						finally
						{
							if (num < 0 && enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					if (dtcv2Model.ECUListFound.Count == 0)
					{
						dtcv2Model.HasDTCToDisplay = false;
					}
					dtcv2Model.IsBusy = false;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<token>5__2 = default(CancellationToken);
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_06C1:
				this.<>1__state = -2;
				this.<token>5__2 = default(CancellationToken);
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003470 RID: 13424 RVA: 0x0024A3BC File Offset: 0x002485BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F00 RID: 7936
			public int <>1__state;

			// Token: 0x04001F01 RID: 7937
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001F02 RID: 7938
			public DTCv2Model <>4__this;

			// Token: 0x04001F03 RID: 7939
			public List<IECU> selectedECUs;

			// Token: 0x04001F04 RID: 7940
			private CancellationToken <token>5__2;

			// Token: 0x04001F05 RID: 7941
			private TaskAwaiter <>u__1;

			// Token: 0x04001F06 RID: 7942
			private int <i>5__3;

			// Token: 0x04001F07 RID: 7943
			private IECU <ecu>5__4;

			// Token: 0x04001F08 RID: 7944
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x020005A1 RID: 1441
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <VagMassClear>d__52 : IAsyncStateMachine
		{
			// Token: 0x06003471 RID: 13425 RVA: 0x0024A3CC File Offset: 0x002485CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Model dtcv2Model = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_010F;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0286;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_030B;
					}
					default:
						if (dtcv2Model.IsBusy)
						{
							goto IL_0349;
						}
						dtcv2Model.IsResults = true;
						dtcv2Model.IsSetup = false;
						dtcv2Model.BadELMDetected = false;
						dtcv2Model.ECUListFound.Clear();
						taskAwaiter = App.OBDReader.DebugWrite("\r\n[DTCv2VagMassClear]\r\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<VagMassClear>d__52>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					taskAwaiter = dtcv2Model.CheckAndRestoreECUConnection().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<VagMassClear>d__52>(ref taskAwaiter, ref this);
						return;
					}
					IL_010F:
					taskAwaiter.GetResult();
					dtcv2Model.IsBusy = true;
					dtcv2Model.IsCancelButtonEnabled = true;
					dtcv2Model.cancellationTokenSource = new CancellationTokenSource();
					CancellationToken token = dtcv2Model.cancellationTokenSource.Token;
					IEnumerator<IECU> enumerator = dtcv2Model.ECUListFull.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IECU iecu = enumerator.Current;
							iecu.Reset();
						}
					}
					finally
					{
						if (num < 0 && enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					OBDRequest obdrequest = new OBDRequest("3E80", "700", "", "", false);
					OBDRequest obdrequest2 = new OBDRequest("04", "700", "", "", false);
					OBDRequest obdrequest3 = new OBDRequest("14FFFFFF", "700", "", "", false);
					App.OBDReader.ReplaceQueue(new OBDRequest[]
					{
						obdrequest2, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest,
						obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest, obdrequest3
					});
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<VagMassClear>d__52>(ref taskAwaiter, ref this);
						return;
					}
					IL_0286:
					taskAwaiter.GetResult();
					Page currentPage = App.GetCurrentPage();
					if (!(currentPage is IDTCvXPage))
					{
						goto IL_0312;
					}
					taskAwaiter = currentPage.DisplayAlert(Translate.GetString("ios_DTCClearFinished_Title"), Translate.GetString("ios_DTCClearFinished_Text"), "OK").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 3);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Model.<VagMassClear>d__52>(ref taskAwaiter, ref this);
						return;
					}
					IL_030B:
					taskAwaiter.GetResult();
					IL_0312:
					dtcv2Model.IsSetup = true;
					dtcv2Model.IsCancelButtonEnabled = true;
					dtcv2Model.IsResults = false;
					dtcv2Model.IsBusy = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0349:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003472 RID: 13426 RVA: 0x0024A76C File Offset: 0x0024896C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F09 RID: 7945
			public int <>1__state;

			// Token: 0x04001F0A RID: 7946
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001F0B RID: 7947
			public DTCv2Model <>4__this;

			// Token: 0x04001F0C RID: 7948
			private TaskAwaiter <>u__1;
		}
	}
}
