using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000758 RID: 1880
	public class InformationItem
	{
		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x06003FBB RID: 16315 RVA: 0x0033298E File Offset: 0x00330B8E
		// (set) Token: 0x06003FBC RID: 16316 RVA: 0x00332996 File Offset: 0x00330B96
		public string Title
		{
			[CompilerGenerated]
			get
			{
				return this.<Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Title>k__BackingField = value;
			}
		}

		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x06003FBD RID: 16317 RVA: 0x0033299F File Offset: 0x00330B9F
		// (set) Token: 0x06003FBE RID: 16318 RVA: 0x003329A7 File Offset: 0x00330BA7
		public string Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x06003FBF RID: 16319 RVA: 0x003329B0 File Offset: 0x00330BB0
		// (set) Token: 0x06003FC0 RID: 16320 RVA: 0x003329B8 File Offset: 0x00330BB8
		public string PID
		{
			[CompilerGenerated]
			get
			{
				return this.<PID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PID>k__BackingField = value;
			}
		}

		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x06003FC1 RID: 16321 RVA: 0x003329C1 File Offset: 0x00330BC1
		// (set) Token: 0x06003FC2 RID: 16322 RVA: 0x003329C9 File Offset: 0x00330BC9
		public string Header
		{
			[CompilerGenerated]
			get
			{
				return this.<Header>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Header>k__BackingField = value;
			}
		}

		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x06003FC3 RID: 16323 RVA: 0x003329D2 File Offset: 0x00330BD2
		// (set) Token: 0x06003FC4 RID: 16324 RVA: 0x003329DA File Offset: 0x00330BDA
		public byte[] RawData
		{
			[CompilerGenerated]
			get
			{
				return this.<RawData>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RawData>k__BackingField = value;
			}
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x003329E4 File Offset: 0x00330BE4
		public void Decode()
		{
			string text = this.PID.Substring(this.PID.Length - 2, 2);
			string text2 = this.PID.Substring(0, this.PID.Length - 2);
			if (!(text2 == "09") && !(text2 == "22F8"))
			{
				if (!(text2 == "1A"))
				{
					if (!(text2 == "22F1"))
					{
						return;
					}
					if (text == "90")
					{
						this.Title = "VIN";
						this.Value = PIDWithStringValue.FilterVin(Encoding.ASCII.GetString(this.RawData));
					}
				}
				else if (this.PID == "1A90")
				{
					this.Title = "VIN";
					this.Value = PIDWithStringValue.FilterVin(Encoding.ASCII.GetString(this.RawData));
					return;
				}
				return;
			}
			if (text != null)
			{
				int length = text.Length;
				if (length == 2)
				{
					switch (text[1])
					{
					case '1':
					{
						if (text == "11")
						{
							this.Title = Translate.GetString("PID_0911");
							this.Value = PIDWithStringValue.Mode09TextFormula(this.RawData).Trim();
							return;
						}
						if (!(text == "21"))
						{
							goto IL_1B45;
						}
						this.Title = Translate.GetString("PID_09" + text);
						int num = 8;
						StringBuilder stringBuilder = new StringBuilder(num * 4);
						for (int i = 0; i < num; i++)
						{
							stringBuilder.Append(Translate.GetString("PID_09" + text + "_" + i.ToString()));
							stringBuilder.Append(": ");
							stringBuilder.Append(InformationItem.FourBytesToInt(this.RawData, i * 4).ToString());
							if (i < num)
							{
								stringBuilder.Append("\n");
							}
						}
						this.Value = stringBuilder.ToString();
						return;
					}
					case '2':
					{
						if (text == "02")
						{
							this.Title = "VIN";
							this.Value = PIDWithStringValue.FilterVin(PIDWithStringValue.Mode09TextFormula(this.RawData));
							return;
						}
						if (text == "12")
						{
							this.Title = Translate.GetString("PID_0912");
							this.Value = ((int)this.RawData[0] * 256 + (int)this.RawData[1]).ToString();
							return;
						}
						if (!(text == "22"))
						{
							goto IL_1B45;
						}
						this.Title = Translate.GetString("PID_09" + text);
						int num2 = 6;
						StringBuilder stringBuilder2 = new StringBuilder(num2 * 4);
						for (int j = 0; j < num2; j++)
						{
							stringBuilder2.Append(Translate.GetString("PID_09" + text + "_" + j.ToString()));
							stringBuilder2.Append(": ");
							stringBuilder2.Append(InformationItem.FourBytesToInt(this.RawData, j * 4).ToString());
							if (j < num2)
							{
								stringBuilder2.Append("\n");
							}
						}
						this.Value = stringBuilder2.ToString();
						return;
					}
					case '3':
					{
						if (text == "13")
						{
							this.Title = Translate.GetString("PID_0913");
							this.Value = PIDWithStringValue.Mode09TextFormula(this.RawData);
							return;
						}
						if (!(text == "23"))
						{
							goto IL_1B45;
						}
						this.Title = Translate.GetString("PID_09" + text);
						int num3 = 12;
						StringBuilder stringBuilder3 = new StringBuilder(num3 * 4);
						for (int k = 0; k < num3; k++)
						{
							stringBuilder3.Append(Translate.GetString("PID_09" + text + "_" + k.ToString()));
							stringBuilder3.Append(": ");
							stringBuilder3.Append(InformationItem.FourBytesToInt(this.RawData, k * 4).ToString());
							if (k < num3)
							{
								stringBuilder3.Append("\n");
							}
						}
						this.Value = stringBuilder3.ToString();
						return;
					}
					case '4':
						if (text == "04")
						{
							this.Title = Translate.GetString("PID_0904");
							this.Value = PIDWithStringValue.Mode09TextFormula(this.RawData).Trim();
							return;
						}
						if (text == "14")
						{
							this.Title = Translate.GetString("PID_0914");
							this.Value = UnitsHelper.GetValue((double)((int)this.RawData[0] * 256 + (int)this.RawData[1]), UnitsHelper.Units.kmh).ToString() + " " + UnitsHelper.GetCaption(UnitsHelper.Units.kmh);
							return;
						}
						if (!(text == "24"))
						{
							goto IL_1B45;
						}
						break;
					case '5':
						if (!(text == "25"))
						{
							goto IL_1B45;
						}
						break;
					case '6':
						if (text == "06")
						{
							this.Title = Translate.GetString("PID_0906");
							this.Value = BitHelpers.ByteArrayToHexString(this.RawData).Trim();
							return;
						}
						if (text == "16")
						{
							this.Title = Translate.GetString("PID_0916");
							int num4 = 8;
							StringBuilder stringBuilder4 = new StringBuilder(num4 * 4);
							for (int l = 0; l < num4; l++)
							{
								stringBuilder4.Append(Translate.GetString("PID_0916_" + l.ToString()));
								stringBuilder4.Append(": ");
								stringBuilder4.Append(InformationItem.FourBytesToInt(this.RawData, l * 4).ToString());
								if (l < num4)
								{
									stringBuilder4.Append("\n");
								}
							}
							this.Value = stringBuilder4.ToString();
							return;
						}
						if (!(text == "26"))
						{
							goto IL_1B45;
						}
						break;
					case '7':
						if (text == "17")
						{
							this.Title = Translate.GetString("PID_09" + text);
							StringBuilder stringBuilder5 = new StringBuilder(4 * 4);
							stringBuilder5.Append(Translate.GetString("PID_0917_0"));
							stringBuilder5.Append(": ");
							stringBuilder5.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0) * 0.1).ToString());
							stringBuilder5.Append("\n");
							stringBuilder5.Append(Translate.GetString("PID_0917_1"));
							stringBuilder5.Append(": ");
							stringBuilder5.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4) * 0.1).ToString());
							stringBuilder5.Append("\n");
							stringBuilder5.Append(Translate.GetString("PID_0917_2"));
							stringBuilder5.Append(": ");
							stringBuilder5.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8) * 0.01).ToString());
							stringBuilder5.Append("\n");
							stringBuilder5.Append(Translate.GetString("PID_0917_3"));
							stringBuilder5.Append(": ");
							stringBuilder5.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12) * 0.01).ToString());
							return;
						}
						if (!(text == "27"))
						{
							goto IL_1B45;
						}
						break;
					case '8':
						if (text == "08")
						{
							int num5 = this.RawData.Length / 2;
							if (num5 > 56)
							{
								num5 = 56;
							}
							this.Title = Translate.GetString("PID_0908");
							List<PIDWithFloatValueFormula> list = new List<PIDWithFloatValueFormula>(num5);
							for (int m = 0; m < num5; m += 2)
							{
								try
								{
									list.Add(PIDWithFloatValueFormula.GetPID0908(m));
								}
								catch (Exception)
								{
								}
							}
							StringBuilder stringBuilder6 = new StringBuilder(num5 * 5);
							for (int n = 0; n < list.Count; n++)
							{
								try
								{
									PIDWithFloatValueFormula pidwithFloatValueFormula = list[n];
									pidwithFloatValueFormula.Decode(this.RawData, TimeSpan.Zero, this.Header);
									stringBuilder6.Append(pidwithFloatValueFormula.Name);
									stringBuilder6.Append(": ");
									stringBuilder6.Append(pidwithFloatValueFormula.Value);
									if (n < list.Count - 1)
									{
										stringBuilder6.Append("\n");
									}
								}
								catch (Exception)
								{
								}
							}
							this.Value = stringBuilder6.ToString();
							return;
						}
						if (text == "18")
						{
							this.Title = Translate.GetString("PID_09" + text);
							StringBuilder stringBuilder7 = new StringBuilder(4 * 4);
							stringBuilder7.Append(Translate.GetString("PID_0918_0"));
							stringBuilder7.Append(": ");
							stringBuilder7.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0)).ToString());
							stringBuilder7.Append("\n");
							stringBuilder7.Append(Translate.GetString("PID_0918_1"));
							stringBuilder7.Append(": ");
							stringBuilder7.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4)).ToString());
							stringBuilder7.Append("\n");
							stringBuilder7.Append(Translate.GetString("PID_0918_2"));
							stringBuilder7.Append(": ");
							stringBuilder7.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8) * 0.1).ToString());
							stringBuilder7.Append("\n");
							stringBuilder7.Append(Translate.GetString("PID_0918_3"));
							stringBuilder7.Append(": ");
							stringBuilder7.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12) * 0.1).ToString());
							return;
						}
						if (!(text == "28"))
						{
							goto IL_1B45;
						}
						break;
					case '9':
						if (text == "19")
						{
							this.Title = Translate.GetString("PID_09" + text);
							StringBuilder stringBuilder8 = new StringBuilder(6 * 4);
							stringBuilder8.Append(Translate.GetString("PID_0919_0"));
							stringBuilder8.Append(": ");
							stringBuilder8.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0)).ToString());
							stringBuilder8.Append("\n");
							stringBuilder8.Append(Translate.GetString("PID_0919_1"));
							stringBuilder8.Append(": ");
							stringBuilder8.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4)).ToString());
							stringBuilder8.Append("\n");
							stringBuilder8.Append(Translate.GetString("PID_0919_2"));
							stringBuilder8.Append(": ");
							stringBuilder8.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8)).ToString());
							stringBuilder8.Append("\n");
							stringBuilder8.Append(Translate.GetString("PID_0919_3"));
							stringBuilder8.Append(": ");
							stringBuilder8.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12)).ToString());
							stringBuilder8.Append("\n");
							stringBuilder8.Append(Translate.GetString("PID_0919_4"));
							stringBuilder8.Append(": ");
							stringBuilder8.Append(((double)InformationItem.FourBytesToInt(this.RawData, 16)).ToString());
							stringBuilder8.Append("\n");
							stringBuilder8.Append(Translate.GetString("PID_0919_5"));
							stringBuilder8.Append(": ");
							stringBuilder8.Append(((double)InformationItem.FourBytesToInt(this.RawData, 20)).ToString());
							return;
						}
						if (!(text == "29"))
						{
							goto IL_1B45;
						}
						break;
					case ':':
					case ';':
					case '<':
					case '=':
					case '>':
					case '?':
					case '@':
						goto IL_1B45;
					case 'A':
						if (text == "0A")
						{
							this.Title = Translate.GetString("PID_090A");
							this.Value = PIDWithStringValue.Mode09TextFormula(this.RawData).Trim();
							return;
						}
						if (text == "1A")
						{
							this.Title = Translate.GetString("PID_09" + text);
							StringBuilder stringBuilder9 = new StringBuilder(6 * 4);
							stringBuilder9.Append(Translate.GetString("PID_091A_0"));
							stringBuilder9.Append(": ");
							stringBuilder9.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0) * 0.1).ToString());
							stringBuilder9.Append("\n");
							stringBuilder9.Append(Translate.GetString("PID_091A_1"));
							stringBuilder9.Append(": ");
							stringBuilder9.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4) * 0.1).ToString());
							stringBuilder9.Append("\n");
							stringBuilder9.Append(Translate.GetString("PID_091A_2"));
							stringBuilder9.Append(": ");
							stringBuilder9.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8) * 0.1).ToString());
							stringBuilder9.Append("\n");
							stringBuilder9.Append(Translate.GetString("PID_091A_3"));
							stringBuilder9.Append(": ");
							stringBuilder9.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12) * 0.1).ToString());
							stringBuilder9.Append("\n");
							stringBuilder9.Append(Translate.GetString("PID_091A_4"));
							stringBuilder9.Append(": ");
							stringBuilder9.Append(((double)InformationItem.FourBytesToInt(this.RawData, 16) * 0.1).ToString());
							stringBuilder9.Append("\n");
							stringBuilder9.Append(Translate.GetString("PID_091A_5"));
							stringBuilder9.Append(": ");
							stringBuilder9.Append(((double)InformationItem.FourBytesToInt(this.RawData, 20) * 0.1).ToString());
							return;
						}
						if (!(text == "2A"))
						{
							goto IL_1B45;
						}
						break;
					case 'B':
					{
						if (text == "0B")
						{
							this.Title = Translate.GetString("PID_090B");
							List<PIDWithFloatValueFormula> list2 = new List<PIDWithFloatValueFormula>(40);
							for (int num6 = 0; num6 < 36; num6 += 2)
							{
								try
								{
									list2.Add(PIDWithFloatValueFormula.GetPID090B(num6));
								}
								catch (Exception)
								{
								}
							}
							StringBuilder stringBuilder10 = new StringBuilder(144);
							for (int num7 = 0; num7 < list2.Count; num7++)
							{
								try
								{
									PIDWithFloatValueFormula pidwithFloatValueFormula2 = list2[num7];
									pidwithFloatValueFormula2.Decode(this.RawData, TimeSpan.Zero, this.Header);
									stringBuilder10.Append(pidwithFloatValueFormula2.Name);
									stringBuilder10.Append(": ");
									stringBuilder10.Append(pidwithFloatValueFormula2.Value);
									if (num7 < list2.Count - 1)
									{
										stringBuilder10.Append("\n");
									}
								}
								catch (Exception)
								{
								}
							}
							this.Value = stringBuilder10.ToString();
							return;
						}
						if (!(text == "1B"))
						{
							goto IL_1B45;
						}
						this.Title = Translate.GetString("PID_09" + text);
						StringBuilder stringBuilder11 = new StringBuilder(4 * 4);
						stringBuilder11.Append(Translate.GetString("PID_091B_0"));
						stringBuilder11.Append(": ");
						stringBuilder11.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0) * 0.01).ToString());
						stringBuilder11.Append("\n");
						stringBuilder11.Append(Translate.GetString("PID_091B_1"));
						stringBuilder11.Append(": ");
						stringBuilder11.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4) * 0.01).ToString());
						stringBuilder11.Append("\n");
						stringBuilder11.Append(Translate.GetString("PID_091B_2"));
						stringBuilder11.Append(": ");
						stringBuilder11.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8) * 0.01).ToString());
						stringBuilder11.Append("\n");
						stringBuilder11.Append(Translate.GetString("PID_091B_3"));
						stringBuilder11.Append(": ");
						stringBuilder11.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12) * 0.01).ToString());
						return;
					}
					case 'C':
					{
						if (!(text == "1C"))
						{
							goto IL_1B45;
						}
						this.Title = Translate.GetString("PID_09" + text);
						StringBuilder stringBuilder12 = new StringBuilder(6 * 4);
						stringBuilder12.Append(Translate.GetString("PID_091C_0"));
						stringBuilder12.Append(": ");
						stringBuilder12.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0) * 0.1).ToString());
						stringBuilder12.Append("\n");
						stringBuilder12.Append(Translate.GetString("PID_091C_1"));
						stringBuilder12.Append(": ");
						stringBuilder12.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4) * 0.1).ToString());
						stringBuilder12.Append("\n");
						stringBuilder12.Append(Translate.GetString("PID_091C_2"));
						stringBuilder12.Append(": ");
						stringBuilder12.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8) * 0.1).ToString());
						stringBuilder12.Append("\n");
						stringBuilder12.Append(Translate.GetString("PID_091C_3"));
						stringBuilder12.Append(": ");
						stringBuilder12.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12) * 0.1).ToString());
						stringBuilder12.Append("\n");
						stringBuilder12.Append(Translate.GetString("PID_091C_4"));
						stringBuilder12.Append(": ");
						stringBuilder12.Append(((double)InformationItem.FourBytesToInt(this.RawData, 16) * 0.1).ToString());
						stringBuilder12.Append("\n");
						stringBuilder12.Append(Translate.GetString("PID_091C_5"));
						stringBuilder12.Append(": ");
						stringBuilder12.Append(((double)InformationItem.FourBytesToInt(this.RawData, 20) * 0.1).ToString());
						return;
					}
					case 'D':
					{
						if (text == "0D")
						{
							this.Title = Translate.GetString("PID_090D");
							this.Value = PIDWithStringValue.Mode09TextFormula(this.RawData).Trim();
							return;
						}
						if (!(text == "1D"))
						{
							goto IL_1B45;
						}
						this.Title = Translate.GetString("PID_09" + text);
						StringBuilder stringBuilder13 = new StringBuilder(8 * 4);
						stringBuilder13.Append(Translate.GetString("PID_091D_0"));
						stringBuilder13.Append(": ");
						stringBuilder13.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0) * 1.0).ToString());
						stringBuilder13.Append("\n");
						stringBuilder13.Append(Translate.GetString("PID_091D_1"));
						stringBuilder13.Append(": ");
						stringBuilder13.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4) * 1.0).ToString());
						stringBuilder13.Append("\n");
						stringBuilder13.Append(Translate.GetString("PID_091D_2"));
						stringBuilder13.Append(": ");
						stringBuilder13.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8) * 1.0).ToString());
						stringBuilder13.Append("\n");
						stringBuilder13.Append(Translate.GetString("PID_091C_3"));
						stringBuilder13.Append(": ");
						stringBuilder13.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12) * 1.0).ToString());
						stringBuilder13.Append("\n");
						stringBuilder13.Append(Translate.GetString("PID_091C_4"));
						stringBuilder13.Append(": ");
						stringBuilder13.Append(((double)InformationItem.FourBytesToInt(this.RawData, 16) * 1.0).ToString());
						stringBuilder13.Append("\n");
						stringBuilder13.Append(Translate.GetString("PID_091C_5"));
						stringBuilder13.Append(": ");
						stringBuilder13.Append(((double)InformationItem.FourBytesToInt(this.RawData, 20) * 1.0).ToString());
						stringBuilder13.Append("\n");
						stringBuilder13.Append(Translate.GetString("PID_091C_6"));
						stringBuilder13.Append(": ");
						stringBuilder13.Append(((double)InformationItem.FourBytesToInt(this.RawData, 24) * 1.0).ToString());
						stringBuilder13.Append("\n");
						stringBuilder13.Append(Translate.GetString("PID_091C_7"));
						stringBuilder13.Append(": ");
						stringBuilder13.Append(((double)InformationItem.FourBytesToInt(this.RawData, 28) * 1.0).ToString());
						stringBuilder13.Append("\n");
						return;
					}
					case 'E':
					{
						if (!(text == "1E"))
						{
							goto IL_1B45;
						}
						this.Title = Translate.GetString("PID_09" + text);
						StringBuilder stringBuilder14 = new StringBuilder(4 * 4);
						stringBuilder14.Append(Translate.GetString("PID_091E_0"));
						stringBuilder14.Append(": ");
						stringBuilder14.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0) * 1.0).ToString());
						stringBuilder14.Append("\n");
						stringBuilder14.Append(Translate.GetString("PID_091E_1"));
						stringBuilder14.Append(": ");
						stringBuilder14.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4) * 1.0).ToString());
						stringBuilder14.Append("\n");
						stringBuilder14.Append(Translate.GetString("PID_091E_2"));
						stringBuilder14.Append(": ");
						stringBuilder14.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8) * 1.0).ToString());
						stringBuilder14.Append("\n");
						stringBuilder14.Append(Translate.GetString("PID_091E_3"));
						stringBuilder14.Append(": ");
						stringBuilder14.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12) * 1.0).ToString());
						return;
					}
					case 'F':
					{
						if (text == "0F")
						{
							this.Title = Translate.GetString("PID_090F");
							this.Value = PIDWithStringValue.Mode09TextFormula(this.RawData).Trim();
							return;
						}
						if (!(text == "1F"))
						{
							goto IL_1B45;
						}
						this.Title = Translate.GetString("PID_09" + text);
						StringBuilder stringBuilder15 = new StringBuilder(8 * 4);
						stringBuilder15.Append(Translate.GetString("PID_091F_0"));
						stringBuilder15.Append(": ");
						stringBuilder15.Append(((double)InformationItem.FourBytesToInt(this.RawData, 0) * 1.0).ToString());
						stringBuilder15.Append("\n");
						stringBuilder15.Append(Translate.GetString("PID_091F_1"));
						stringBuilder15.Append(": ");
						stringBuilder15.Append(((double)InformationItem.FourBytesToInt(this.RawData, 4) * 1.0).ToString());
						stringBuilder15.Append("\n");
						stringBuilder15.Append(Translate.GetString("PID_091F_2"));
						stringBuilder15.Append(": ");
						stringBuilder15.Append(((double)InformationItem.FourBytesToInt(this.RawData, 8) * 1.0).ToString());
						stringBuilder15.Append("\n");
						stringBuilder15.Append(Translate.GetString("PID_091F_3"));
						stringBuilder15.Append(": ");
						stringBuilder15.Append(((double)InformationItem.FourBytesToInt(this.RawData, 12) * 1.0).ToString());
						stringBuilder15.Append("\n");
						stringBuilder15.Append(Translate.GetString("PID_091F_4"));
						stringBuilder15.Append(": ");
						stringBuilder15.Append(((double)InformationItem.FourBytesToInt(this.RawData, 16) * 1.0).ToString());
						stringBuilder15.Append("\n");
						stringBuilder15.Append(Translate.GetString("PID_091F_5"));
						stringBuilder15.Append(": ");
						stringBuilder15.Append(((double)InformationItem.FourBytesToInt(this.RawData, 20) * 1.0).ToString());
						stringBuilder15.Append("\n");
						stringBuilder15.Append(Translate.GetString("PID_091F_6"));
						stringBuilder15.Append(": ");
						stringBuilder15.Append(((double)InformationItem.FourBytesToInt(this.RawData, 24) * 1.0).ToString());
						stringBuilder15.Append("\n");
						stringBuilder15.Append(Translate.GetString("PID_091F_7"));
						stringBuilder15.Append(": ");
						stringBuilder15.Append(((double)InformationItem.FourBytesToInt(this.RawData, 28) * 1.0).ToString());
						stringBuilder15.Append("\n");
						return;
					}
					default:
						goto IL_1B45;
					}
					this.Title = Translate.GetString("PID_09" + text);
					int num8 = 4;
					StringBuilder stringBuilder16 = new StringBuilder(num8 * 4);
					for (int num9 = 0; num9 < num8; num9++)
					{
						stringBuilder16.Append(Translate.GetString("PID_09" + text + "_" + num9.ToString()));
						stringBuilder16.Append(": ");
						stringBuilder16.Append(InformationItem.FourBytesToInt(this.RawData, num9 * 4).ToString());
						if (num9 < num8)
						{
							stringBuilder16.Append("\n");
						}
					}
					this.Value = stringBuilder16.ToString();
					return;
				}
			}
			IL_1B45:
			this.Value = "";
			this.Title = "";
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x00002050 File Offset: 0x00000250
		public InformationItem()
		{
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x003345F0 File Offset: 0x003327F0
		// Note: this type is marked as 'beforefieldinit'.
		static InformationItem()
		{
		}

		// Token: 0x0400271C RID: 10012
		[CompilerGenerated]
		private string <Title>k__BackingField;

		// Token: 0x0400271D RID: 10013
		[CompilerGenerated]
		private string <Value>k__BackingField;

		// Token: 0x0400271E RID: 10014
		[CompilerGenerated]
		private string <PID>k__BackingField;

		// Token: 0x0400271F RID: 10015
		[CompilerGenerated]
		private string <Header>k__BackingField;

		// Token: 0x04002720 RID: 10016
		[CompilerGenerated]
		private byte[] <RawData>k__BackingField;

		// Token: 0x04002721 RID: 10017
		private static Func<byte[], int, int> FourBytesToInt = (byte[] data, int startByteId) => (int)data[startByteId] * 16777216 + (int)data[startByteId + 1] * 65536 + (int)data[startByteId + 2] * 256 + (int)data[startByteId + 3];

		// Token: 0x02000759 RID: 1881
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003FC8 RID: 16328 RVA: 0x00334607 File Offset: 0x00332807
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003FC9 RID: 16329 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003FCA RID: 16330 RVA: 0x00334613 File Offset: 0x00332813
			internal int <.cctor>b__23_0(byte[] data, int startByteId)
			{
				return (int)data[startByteId] * 16777216 + (int)data[startByteId + 1] * 65536 + (int)data[startByteId + 2] * 256 + (int)data[startByteId + 3];
			}

			// Token: 0x04002722 RID: 10018
			public static readonly InformationItem.<>c <>9 = new InformationItem.<>c();
		}
	}
}
