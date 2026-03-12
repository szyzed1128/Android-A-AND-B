using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB.BMW;
using CarScannerXamarinForms.Coding.DB.Nissan;
using CarScannerXamarinForms.Coding.DB.Toyota;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x0200013B RID: 315
	[XamlFilePath("Pages\\TerminalPage.xaml")]
	public class TerminalPage : ContentPage
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00060770 File Offset: 0x0005E970
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x00060777 File Offset: 0x0005E977
		public static TerminalPage Instance
		{
			[CompilerGenerated]
			get
			{
				return TerminalPage.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				TerminalPage.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00060780 File Offset: 0x0005E980
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
			this.LayoutRoot.HeightRequest = base.Height - base.Padding.Top - base.Padding.Bottom;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000607BC File Offset: 0x0005E9BC
		public TerminalPage()
		{
			TerminalPage.Instance = this;
			this.turnOnECUPingAfterFinish = SharedSettings.Current.AlwaysPingECU;
			this.InitializeComponent();
			if (SharedSettings.Current.ShowExperimental)
			{
				this.btnPIDScanner.IsVisible = true;
			}
			this.DefaultViewModel.Add(new TerminalElement
			{
				ElementType = TerminalElementTypes.Response,
				Text = Translate.GetString("ios_TerminalInfoText"),
				LineFinished = true
			});
			this.lvTerm.ItemsSource = this.DefaultViewModel;
			App.OBDReader.ClearRequestQueue();
			App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Terminal;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00060870 File Offset: 0x0005EA70
		public async void OnResponseReceived(string data)
		{
			foreach (string text in data.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries))
			{
				this.DefaultViewModel.Add(new TerminalElement
				{
					ElementType = TerminalElementTypes.Response,
					Text = text
				});
			}
			try
			{
				this.lvTerm.ScrollTo(this.DefaultViewModel.LastOrDefault<TerminalElement>(), 3, true);
			}
			catch
			{
			}
			if (this.next_commands.Count > 0)
			{
				string text2 = this.next_commands[0];
				this.next_commands.RemoveAt(0);
				await this.SendCommand(text2);
			}
			else
			{
				this.btnSendCmd.IsEnabled = true;
				if (this.turnOnECUPingAfterFinish)
				{
					SharedSettings.Current.AlwaysPingECU = true;
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x000608AF File Offset: 0x0005EAAF
		public ObservableCollection<TerminalElement> DefaultViewModel
		{
			get
			{
				return this.defaultViewModel;
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000608B8 File Offset: 0x0005EAB8
		public async void btnBack_Clicked(object sender, EventArgs e)
		{
			App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
			App.OBDReader.ClearRequestQueue();
			await base.Navigation.PopAsync(true);
			TerminalPage.Instance = null;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000608EF File Offset: 0x0005EAEF
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnBack_Clicked(this, null);
			}
			return true;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0006091C File Offset: 0x0005EB1C
		private async void btnSendCmd_Click(object sender, EventArgs e)
		{
			try
			{
				this.btnSendCmd.IsEnabled = false;
				this.turnOnECUPingAfterFinish = SharedSettings.Current.AlwaysPingECU;
				SharedSettings.Current.AlwaysPingECU = false;
				string cmd = this.tbCommand.Text.Trim();
				if (cmd.ToLowerInvariant().StartsWith("www:"))
				{
					try
					{
						string text = cmd.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).First<string>();
						string text2 = await HttpDownloader.Get("https://www.carscanner.info/terminal/" + text, 10);
						if (!string.IsNullOrEmpty(text2))
						{
							cmd = text2;
						}
					}
					catch (Exception)
					{
					}
				}
				if (cmd == "[TOYOTAFULL]")
				{
					Progress<string> progress = new Progress<string>(delegate(string s)
					{
						MainThread.BeginInvokeOnMainThread(delegate
						{
							this.OnResponseReceived(s);
						});
					});
					await new ToyotaUnitsDetector().ReadAllUnits(progress);
					this.btnSendCmd.IsEnabled = true;
				}
				else if (cmd == "[NISSANFULL]")
				{
					Progress<string> progress2 = new Progress<string>(delegate(string s)
					{
						MainThread.BeginInvokeOnMainThread(delegate
						{
							this.OnResponseReceived(s);
						});
					});
					await new NissanUnitsDetector().ReadAllUnits(progress2);
					this.btnSendCmd.IsEnabled = true;
				}
				else if (cmd == "[BMWFULL]")
				{
					Progress<string> progress3 = new Progress<string>(delegate(string s)
					{
						MainThread.BeginInvokeOnMainThread(delegate
						{
							this.OnResponseReceived(s);
						});
					});
					await new BMWUnitsDetector().ReadAllUnits(progress3);
					this.btnSendCmd.IsEnabled = true;
				}
				else if (cmd.StartsWith('['))
				{
					this.next_commands = this.GetBuiltInSequence(cmd);
					cmd = this.next_commands[0];
					this.next_commands.RemoveAt(0);
					await this.SendCommand(cmd);
				}
				else if (cmd.Contains(';'))
				{
					this.next_commands = (from x in cmd.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						where !string.IsNullOrEmpty(x)
						select x).ToList<string>();
					cmd = this.next_commands[0];
					this.next_commands.RemoveAt(0);
					await this.SendCommand(cmd);
				}
				else
				{
					await this.SendCommand(cmd);
				}
				this.tbCommand.Text = "";
				cmd = null;
			}
			catch
			{
				this.btnSendCmd.IsEnabled = true;
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00060954 File Offset: 0x0005EB54
		private string[] GetVagBlocksScan(string header)
		{
			string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(header, "Audi", null);
			return string.Concat(new string[] { "ATSTFF;ATAT0;ATSH", header, ";ATFCSH", header, ";ATFCSD300000;ATFCSM1;ATCRA", possibleResponseHeader, ";1003;0902;222A2A;222A2E;22F19E;22F1A2;22F187;22F189;22F191;22F1A3;22F1A5;22F1DF;22F197;220601;220600;220606;220607;220608;22F1A0;22F1A1;22F1A3;22F1A4;22F1AA;22F190;22F17C;22F18C;22F1E0;22F17B;22F182;22F1A0;22F1A1;22F1AA;22F190;22F17C;22F18C;22F1AD;22F1A4;22F1E0;22F17B;22F1B4;22F182;" }).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000609B8 File Offset: 0x0005EBB8
		private List<string> GetBuiltInSequence(string cmd)
		{
			if (cmd.StartsWith("[VAGBLOCKS") && cmd.EndsWith(']'))
			{
				string[] array = new string[0];
				if (cmd.Contains("FULL]"))
				{
					array = new string[]
					{
						"7E0", "7E1", "7E2", "7E3", "7E4", "7E5", "7E6", "7E7", "713", "712",
						"732", "74D", "746", "70E", "770", "70A", "7E2", "757", "772", "715",
						"70C", "714", "76A", "716", "728", "70F", "73B", "711", "72D", "71A",
						"71E", "755", "74C", "76C", "74E", "72C", "74A", "712", "76F", "74B",
						"754", "76D", "7E6", "752", "773", "70B", "747", "769", "6B8", "723",
						"745", "71D", "767", "70A", "76B", "75A", "74F", "6BC", "784", "710",
						"765", "762", "744"
					};
				}
				else
				{
					array = (from x in cmd.Replace("[VAGBLOCKS", "").Replace("]", "").Replace(" ", "")
							.Trim()
							.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						where x.Length == 3
						select x).ToArray<string>();
				}
				List<string> list = new List<string>();
				foreach (string text in array)
				{
					string[] vagBlocksScan = this.GetVagBlocksScan(text);
					list.AddRange(vagBlocksScan);
				}
				return list;
			}
			if (cmd != null)
			{
				switch (cmd.Length)
				{
				case 6:
					if (cmd == "[BUFF]")
					{
						return "ATSH773;ATFCSH773;ATFCSD300000;ATFCSM1;ATCRA7DD;ATAT0;ATST96;223C05".Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
					}
					break;
				case 7:
				{
					char c = cmd[1];
					if (c != 'G')
					{
						if (c != 'H')
						{
							if (c == 'V')
							{
								if (cmd == "[VAG3E]")
								{
									IEnumerable<string> enumerable = new string[]
									{
										"7E0", "7E1", "713", "712", "732", "74D", "746", "70E", "770", "70A",
										"7E2", "772", "715", "70C", "714", "76A", "716", "728", "70F", "73B",
										"711", "72D", "71A", "71E", "755", "74C", "76C", "74E", "72C", "74A",
										"712", "76F", "74B", "754", "76D", "7E6", "752", "773", "70B", "747",
										"769", "6B8", "723", "745", "71D", "767", "70A", "76B", "75A", "74F",
										"6BC", "784", "710", "757", "7E5", "762", "765", "744"
									}.Select((string header) => string.Concat(new string[]
									{
										"ATSH",
										header,
										";ATFCSH",
										header,
										";ATFCSD300000;ATFCSM1;ATCRA",
										CAN11bitHelper.GetPossibleResponseHeader(header, "Audi", null),
										";3E"
									}));
									List<string> list2 = new List<string>();
									foreach (string text2 in enumerable)
									{
										string[] array3 = text2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
										list2.AddRange(array3);
									}
									return list2;
								}
							}
						}
						else if (cmd == "[HKCAN]")
						{
							App.OBDReader.DebugWrite("\r\n[HKCAN]\r\n");
							return "ATSH7DF;0100;010C;0105;ATSH7E0;ATFCSH7E0;ATFCSD300000;ATFCSM1;2100;2101;2102;2103;2104;2105;2106;2107;2108;2109;210A;210B;210C;210D;210E;210F;2110;2111;2112;2113;2114;2115;2116;2117;2118;2119;211A;211B;211D;211F;2120;2121;2122;2123;2124;2125;2126;2127;2128;2129;21A0;2190;1A90;2172;21948001;21B0;21B3;21C0;22ED03;22E0F1;22ED94;22E000;22E001;22E002;22E003;22E004;22E005;22E006;22E007;22E008;22E009;22E017;22E019;22E021;22E00A;22E00D;22E00E;22E014;228520;22ED01;22ED02;22ED03;22ED04;22ED05;22ED06;22ED07;22ED27;22ED1F;22E0F2;22F190;22EDA2;22ED01;22ED02;22ED03;22ED04;22ED05;22ED06;22ED07;22ED08;22ED09;22ED0A;22ED0B;22ED0C;22ED0D;22ED0F;22ED11;22ED12;22ED13;22ED15;22ED17;22ED18;22ED19;22ED1A;22ED1B;22ED1C;22ED1D;22ED1E;22E010;22E011;22E012;22E013;22E016;22E01B;22E01D;22E01F;22E020;22E022;22E023;22E025;22E026;22E027;22E028;22E029;22E00B;22E00C;22E00F;22E015;22E02A;22E02C;22E02D;22ED10;22ED11;22ED14;22ED18;22E0F1;22ED94;22ED29;22E0F2;228520;228530;22851A;2285A2;1003;22E000;22E001;22E002;22E003;22E004;22E005;22E006;22E007;22E008;22E009;22E017;22E019;22E021;22E00A;22E00D;22E00E;22E014;228520;228520;228530;22851A;2285A2;ATSH7E1;ATFCSH7E1;ATFCSD300000;ATFCSM1;21A0;21A1;2201A0;2201A4;222081;222089;2201A4;1003;21A0;21A1;2201A0;2201A4;222081;222089;2201A4;2201A5;2201A6;ATSH7E5;ATFCSH7E5;ATFCSD300000;ATFCSM1;21C0;2201A1;1090;21C0;2201A1;ATSH7D4;ATFCSH7D4;ATFCSD300000;ATFCSM1;2101;220101;ATSH7D1;ATFCSH7D1;ATFCSD300000;ATFCSM1;2101;2102;22C101;22C104;22C105;22C102;22C103;22C100;224000;224001;224002;224003;220104;220201;ATSH7A0;ATFCSH7A0;ATFCSD300000;ATFCSM1;22C00B;1003;22C00B;22C00C;22C001;22C002;22C00D;22C00E;22C00F;ATSH7D6;ATFCSH7D6;ATFCSD300000;ATFCSM1;2106;ATSH740;ATFCSH740;ATFCSD300000;ATFCSM1;21C0;2201A1;1090;21C0;2201A1;2201A2;2201A3;ATSH7E4;ATFCSH7E4;ATFCSD300000;ATFCSM1;2100;2101;2102;2103;2104;2105;2106;2120;220101;220105;220106;220102;220103;22010A;22010C;1081;1003;1001;220104;220107;220108;22010B;220111;220114;220130;220131;220132;220133;220134;ATSH7C5;2101;ATSH794;2102;ATSH7E2;2100;2102;ATSH7E7;ATFCSH7E7;ATFCSD300000;ATFCSM1;2101;2102;2103;2104;2105;2106;2107;2108;2109;2110;2111;ATSH7C6;ATFCSH7C6;ATFCSD300000;ATFCSM1;22B002;2102;22B003;22B001;2101;212A;ATSH7B3;ATFCSH7B3;ATFCSD300000;ATFCSM1;220100;ATSH7B2;ATFCSH7B2;ATFCSD300000;ATFCSM1;2100;2101;220100".Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
						}
					}
					else if (cmd == "[GMDTC]")
					{
						return "ATAT0;ATSTFF;ATCF7E8;ATCM7FF;A98112;A98112;ATSH7DF;ATCF7E8;ATCM7E0;A98112;A98112;ATSH101;A98112;A98112;ATCF643;A98112;A98112;ATCF640;A98112;A98112;ATCM7E0;ATFCSH7E0;ATFCSD300000;ATFCSM1;ATCF7E8;A98112;A98112;ATCM5FF;A98112;A98112;ATSH7E0;A98112;A98112;ATFCSH243;ATSH243;ATCF643;ATCM4FF;A98112;A98112;ATFCSH245;ATSH245;ATCF645;ATCM4FF;A98112;A98112;ATFCSH241;ATSH241;ATCF641;ATCM4FF;A98112;A98112;ATFCSH245;ATSH245;ATCF645;ATCM4FF;A98112;A98112;ATFCSH242;ATSH242;ATCF642;ATCM4FF;A98112;A98112;ATFCSH7E1;ATSH7E1;ATCF7E9;ATCM5FF;A98112;A98112;ATFCSH7E2;ATSH7E2;ATCF7EA;ATCM5FF;A98112;A98112;".Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
					}
					break;
				}
				case 8:
				{
					char c = cmd[1];
					if (c != 'M')
					{
						if (c == 'V')
						{
							if (cmd == "[VWTP02]")
							{
								return "ATAT0;ATPBC001;ATSPB;ATSH200;ATCRA202;02C00010000301;ATSH760;ATCRA300;A00F8AFF32FF;1000021089;B1;1100021A9B;B1;12000322F19E;13000322F1A2;14000322F187;15000322F189;16000322F191;17000322F1A3;18000322F1A5;19000322F1DF;ATD;ATSP6;ATE0;ATH1;0100".Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
							}
						}
					}
					else if (cmd == "[MBFULL]")
					{
						return new string[]
						{
							"ATSP6", "ATH1", "ATE1", "ATSH7E0", "ATFCSH7E0", "ATCRA7E8", "ATFCSD300828", "ATFCSM1", "10032", "10C02",
							"1A90", "220009", "22000B", "2104", "2103", "210A", "2105", "301601", "2102", "210D",
							"210E", "2108", "304F01", "2106", "2127", "2113", "2111", "2109", "306501", "306401",
							"2101", "302301", "302401", "302501", "302601", "2115", "301501", "302201", "302701", "210C",
							"304201", "304001", "21A3", "22B540", "22B542", "22B544", "22B548", "222044", "22202B", "22D044",
							"22D093", "222051", "22D010", "222018", "226430", "226432", "222027", "226250", "22640E", "222038",
							"222037", "226254", "226255", "22C001", "22C220", "22C003", "22C006", "22C007", "226221", "226220",
							"22622A", "222019", "222028", "224014", "226232", "22624B", "226242", "226241", "226233", "222030",
							"222032", "222040", "222039", "226162", "22D062", "226041", "226042", "226043", "226044", "226040",
							"22D049", "309201", "309001", "210F", "309601", "309701", "2126", "2121", "220201", "220220",
							"220200", "220400", "220410", "220420", "220163", "220164", "220165", "220166", "220168", "22016A",
							"22016B", "22016C", "22016D", "22016F", "220170", "220171", "220172", "220173", "220174", "220176",
							"22017A", "22017B", "22017E", "22017F", "220181", "220182", "220184", "220188", "220189", "22018A",
							"22018F", "220198", "22019A", "22019F", "2201A0", "2201A2", "2201A3", "2201A4", "2201A5", "2201A6",
							"2201C9", "2201D1", "2201D2", "2201DE", "2201E0", "2201E1", "2201E2", "2201EF", "2201F0", "2201F2",
							"2201F4", "2201F5", "2201FA", "2201FF", "220202", "220203", "220205", "220209", "22020A", "22020B",
							"220211", "220212", "220225", "220226", "220227", "22022B", "22022F", "220294", "220296", "220298",
							"220299", "22029A", "22029B", "221004", "22D011", "22D012", "22D013", "22D019", "22D01A", "22D01B",
							"22D053", "22D058", "22D05C", "22D087", "22D0A0", "22D0A1", "22D0A2", "22D0A3", "220330", "220332",
							"220364", "220365", "220366", "220368", "220369", "22036A", "22036B", "2203A1", "2203A2", "2203A3",
							"2203A5", "2203A8", "2201A7", "2201CF", "2201D7", "2201FB", "22020C", "22020D", "22020E", "22020F",
							"220210", "220292", "22D051", "2203BD", "2203C2", "22D05D", "220412", "220413", "221002", "222006",
							"222013", "222014", "222015", "22201C", "222024", "222026", "222029", "22202A", "22202C", "22202E",
							"22202F", "222035", "22203D", "22203E", "222046", "22400A", "22400D", "22400E", "224010", "22500A",
							"22500B", "22500D", "22500F", "225014", "225024", "225025", "225028", "22502B", "225030", "226008",
							"22600D", "22600E", "22601A", "22601F", "226038", "226051", "21A2", "30A601", "21A1", "21A5",
							"22D054", "22633D", "22633C", "226335", "226334", "222067", "22633B", "22633A", "222066", "222016",
							"225031", "222022", "226450", "22D022", "22D023", "224002", "224003", "22D052", "226088", "226087",
							"226428", "226056", "22605E", "226065", "2260A5", "2260A8", "2260BF", "2260C0", "2260DC", "2260F3",
							"2260F6", "2260F7", "22611D", "226149", "226152", "226163", "226170", "226172", "226175", "22619A",
							"2261FE", "226214", "226217", "226218", "226219", "22621A", "22621C", "22621D", "22625D", "22625E",
							"226262", "22630A", "22630E", "22631B", "22631C", "22631D", "226320", "226321", "226322", "226323",
							"226324", "226327", "226328", "22632F", "226331", "226332", "226333", "22635E", "226384", "226551",
							"226575", "226576", "226591", "226594", "226596", "226597", "226598", "2265A2", "2265A3", "2265A4",
							"2265A5", "2265A6", "2265A7", "2265A8", "2265A9", "2265AA", "2265AB", "2265AC", "2265AD", "2265AE",
							"2265AF", "2265B0", "2265B1", "2265B2", "2265B3", "2265B4", "2265B5", "2265B6", "2265B7", "2265B8",
							"2265B9", "2265BA", "2265BB", "2265BC", "2265BE", "2265BF", "2265C0", "2265C1", "2265C2", "2265C3",
							"2265C4", "2265C5", "2265C6", "2265C8", "2265CA", "2265CB", "2265CC", "226601", "226605", "226606",
							"22670A", "226713", "22D050", "3103032A", "22C330", "22C33B", "22C34E", "22C350", "22C351", "22C352",
							"22C353", "22C354", "22C356", "22C357", "22C358", "22C359", "22C35A", "22C35C", "22C35F", "22C360",
							"22C361", "22C363", "22C364", "22C365", "22C366", "22C367", "22C368", "22C36B", "22C36E", "22C382",
							"22C38C", "22C3DC", "22CD07", "22CD22", "22CD28", "22CD2A", "22CD2B", "22D04D", "22D04E", "22D05B",
							"22D060", "22D061", "22D081", "22D082", "310303A7", "222011", "220301", "220319", "220321", "220401",
							"220404", "221600", "221601", "221602", "221606", "221650", "221652", "221653", "221656", "222096",
							"222021", "224000", "224001", "224050", "224051", "224052", "224053", "226105", "226103", "225004",
							"225022", "222100", "222101", "222102", "222103", "222071", "222070", "222098", "224073", "22D031",
							"222023", "222077", "226351", "226132", "226353", "22D006", "22D034", "222079", "22D057", "222000",
							"222001", "223017", "226355", "22301E", "22301F", "224012", "226181", "226183", "222012", "222009",
							"222034", "222095", "222025", "22D066", "22C200", "22D048", "226402", "226382", "222053", "224040",
							"226509", "226504", "226505", "226506", "226503", "225002", "225001", "22D027", "225021", "225027",
							"226021", "226022", "226023", "226024", "226025", "226026", "222017", "222063", "222061", "22D005",
							"225029", "224005", "222007", "226000", "226001", "224067", "224065", "224066", "224064", "22D036",
							"22D037", "22D038", "22D039", "226182", "226184", "222062", "222049", "222010", "222008", "226271",
							"222045", "22201E", "2146", "217C", "2157", "2156", "2110", "2166", "216E", "2114",
							"2165", "217D", "2158", "2107", "215A", "215B", "2159", "2116", "2117", "2118",
							"2112", "210B", "217B", "22114601", "22112501", "22112601", "22110301", "22114001", "22111001", "22116701",
							"22110A01", "22110D01", "22110E01", "22115B01", "22115A01", "22115C01", "22111D01", "22111E01", "2211D101", "22111401",
							"22111301", "22114301", "22111701", "22111801", "22112B01", "22112C01", "22112D01", "22112E01", "22112F01", "22113001",
							"22113101", "22113201", "22116B01", "22116C01", "22111B01", "22111C01", "22112101", "22111F01", "22112201", "22112001",
							"22110601", "22116801", "22116901", "22110701", "22116A01", "22113301", "22113401", "22113501", "22113601", "22113701",
							"22113801", "22110C01", "22110401", "22110501", "22110201", "22110101", "22113F01", "22117301", "22117401", "22117201",
							"", "22111501", "22111601", "22116F01", "22111101", "22114201", "22111201", "22110B01", "216F", "305501",
							"2139", "213A", "2163", "217E", "305701", "214B", "214A", "305201", "215D", "211A",
							"2119", "2167", "2171", "211B", "2172", "2173", "215C", "211C", "226429", "226303",
							"226319", "226311", "226304", "226312", "226305", "226313", "226306", "226314", "226307", "226315",
							"226308", "226316", "22206F", "222069", "226329", "22207A", "222076", "226030", "226031", "226032",
							"226033", "226034", "226035", "222081", "22207C", "222080", "22207B", "222078", "22626B", "22626A",
							"22626E", "224039", "225120", "22D073", "222050", "225122", "22D111", "224011", "224013", "226154",
							"226153", "222135", "305601", "222004", "222005", "222020", "223013", "224022", "225015", "225045",
							"225051", "225058", "226068", "226090", "226091", "226496", "226702", "226711", "226724", "226726",
							"301001", "301101", "301201", "2142", "306801", "301B01", "2162", "22D094", "22D095", "2201A1",
							"301301", "2180", "2169", "2144", "216A", "303001", "303201", "222134", "222137", "222136",
							"222131", "222130", "222133", "222132", "222041", "226285", "226287", "226289", "222056", "222055",
							"22205A", "222059", "222064", "222058", "222057", "226472", "226474", "226470", "222123", "22C201",
							"222122", "222121", "22201D", "22201F", "226278", "225032", "226451", "226452", "226453", "226454",
							"226455", "226299", "22D001", "225105", "225101", "225100", "225103", "225102", "224015", "224021",
							"224016", "224019", "224033", "224017", "22D056", "226257", "226302", "22606D", "222119", "226089",
							"226425", "226427", "226109", "225011", "22D029", "22D026", "22D028", "226245", "222120", "222068",
							"22D03B", "22D108", "22205C", "22205B", "22D06F", "22D003", "226279", "226280", "226405", "226252",
							"22626C", "226275", "226273", "226274", "226272", "226277", "226276", "226431", "22D035", "224070",
							"226139", "222036", "22204C", "22D043", "22204A", "220101", "220100", "22B547", "22B541", "22B543",
							"22B545", "22B549", "ATFSCM1", "ATD", "ATE0", "ATH1", "ATS0"
						}.ToList<string>();
					}
					break;
				}
				case 9:
					if (!(cmd == "[TOYOTA1]"))
					{
					}
					break;
				case 11:
					if (cmd == "[TOYOTACAN]")
					{
						List<string> list3 = new List<string>();
						string[] array4 = new string[] { "7E0", "7E1", "7C0", "7C4", "791" };
						string[] array5 = new string[]
						{
							"40", "2A", "B5", "AD", "67", "E9", "B6", "41", "42", "E0",
							"90", "91", "93", "B9", "A1", "A2", "A4", "A7", "B8", "AB",
							"80", "83", "85", "8A", "8B", "8C", "8D", "DC", "DD", "DE",
							"C7", "70", "7C", "38", "36", "39", "3A", "2A", "44", "EC",
							"A5", "A6", "A8", "DB", "DA", "3B", "60", "02", "2C", "51",
							"4F", "B0", "5B", "52", "C8", "7A", "AE", "7B", "AC", "69",
							"26", "5F", "D3", "EB", "6D", "0F", "79", "F3", "0C"
						};
						foreach (string text3 in array4)
						{
							string[] array6 = string.Concat(new string[]
							{
								"ATAL;ATSH",
								text3,
								";ATCRA",
								CAN11bitHelper.GetPossibleResponseHeader(text3, "Toyota", null),
								";ATFCSH",
								text3,
								";ATFCSD300010;ATFCSM1;3E;A803;ATCAF0;3000100000000000;3000100000000000;3000100000000000;ATCAF1;2100;1A80;1A81;1A8881;1A8800;"
							}).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
							list3.AddRange(array6);
						}
						foreach (string text4 in array5)
						{
							string[] array7 = string.Concat(new string[]
							{
								"ATAL;ATSH750;ATCRA758;ATFCSH750;ATFCSD", text4, "300010;ATFCSM1;ATCEA", text4, ";ATTA", text4, "3E;A803;ATCAF0;ATAL;", text4, "30001000000000;", text4,
								"30001000000000;", text4, "30001000000000;ATCAF1;2100;1A80;1A81;1A8881;1A8800;"
							}).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
							list3.AddRange(array7);
						}
						return list3;
					}
					break;
				}
			}
			return new List<string> { "0100" };
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00063990 File Offset: 0x00061B90
		private void Handle_Completed(object sender, EventArgs e)
		{
			this.btnSendCmd_Click(null, null);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0006399C File Offset: 0x00061B9C
		private async Task SendCommand(string cmd)
		{
			if (cmd.StartsWith("delay=", StringComparison.InvariantCultureIgnoreCase))
			{
				string text = cmd.Substring(6);
				int num = 0;
				this.DefaultViewModel.Add(new TerminalElement
				{
					ElementType = TerminalElementTypes.Command,
					Text = cmd
				});
				if (int.TryParse(text, out num))
				{
					await Task.Delay(num);
				}
				this.DefaultViewModel.Add(new TerminalElement
				{
					ElementType = TerminalElementTypes.Response,
					Text = cmd
				});
				if (this.next_commands.Count > 0)
				{
					cmd = this.next_commands[0];
					this.next_commands.RemoveAt(0);
					await this.SendCommand(cmd);
				}
				else
				{
					this.btnSendCmd.IsEnabled = true;
				}
			}
			else if (!OBDReaderSimulator.Current.IsActive && (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU))
			{
				this.DefaultViewModel.Add(new TerminalElement
				{
					ElementType = TerminalElementTypes.Command,
					Text = cmd
				});
				OBDRequest obdrequest = new OBDRequest(cmd, "", "", "", false, null);
				if (!obdrequest.Command.ToUpperInvariant().StartsWith("AT"))
				{
					obdrequest.CheckLength = true;
				}
				App.OBDReader.AddRequestToQueue(obdrequest);
			}
			else if (OBDReaderSimulator.Current.IsActive)
			{
				this.DefaultViewModel.Add(new TerminalElement
				{
					ElementType = TerminalElementTypes.Command,
					Text = cmd
				});
				this.OnResponseReceived(">>:" + cmd);
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000639E7 File Offset: 0x00061BE7
		private void btnPIDScanner_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new PIDScannerPage());
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000639FC File Offset: 0x00061BFC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(TerminalPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/TerminalPage.xaml",
				Instance = this
			}))
			{
				this.__InitComponentRuntime();
				return;
			}
			if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(base.GetType()) != null)
			{
				this.__InitComponentRuntime();
				return;
			}
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 22);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 22);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 22);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 21);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 26);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 18);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 21);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 21);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("LayoutRoot", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("lvTerm", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvTerm";
			}
			nameScope.RegisterName("tbCommand", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "tbCommand";
			}
			nameScope.RegisterName("btnSendCmd", linkButton2);
			if (linkButton2.StyleId == null)
			{
				linkButton2.StyleId = "btnSendCmd";
			}
			nameScope.RegisterName("btnPIDScanner", linkButton3);
			if (linkButton3.StyleId == null)
			{
				linkButton3.StyleId = "btnPIDScanner";
			}
			this.gridButtons = grid;
			this.LayoutRoot = grid2;
			this.lvTerm = listView;
			this.tbCommand = entry;
			this.btnSendCmd = linkButton2;
			this.btnPIDScanner = linkButton3;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, false);
			this.Appearing += this.Handle_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(TerminalPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Handle_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			on.Platform = new List<string>(2) { "Android", "WinPhone" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "5,20,5,5";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			grid.SetValue(Grid.RowProperty, 0);
			grid.SetValue(Grid.ColumnProperty, 0);
			grid.SetValue(Grid.ColumnSpanProperty, 2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = linkButton;
			array2[1] = grid;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(TerminalPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Back";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(TerminalPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			dynamicResourceExtension3.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = nonScalableLabel;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(TerminalPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_MainPage_TileTerminal";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = nonScalableLabel;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(TerminalPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.Text = obj7;
			grid.Children.Add(nonScalableLabel);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(Grid.ColumnProperty, 0);
			listView.SetValue(Grid.ColumnSpanProperty, 2);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			bindingExtension.Path = "DefaultViewModel";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			listView.SetValue(ListView.SeparatorVisibilityProperty, 1);
			IDataTemplate dataTemplate2 = dataTemplate;
			TerminalPage.<InitializeComponent>_anonXamlCDataTemplate_77 <InitializeComponent>_anonXamlCDataTemplate_ = new TerminalPage.<InitializeComponent>_anonXamlCDataTemplate_77();
			object[] array6 = new object[0 + 5];
			array6[0] = dataTemplate;
			array6[1] = listView;
			array6[2] = grid2;
			array6[3] = scrollView;
			array6[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array6;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			entry.SetValue(Grid.RowProperty, 2);
			entry.SetValue(Grid.ColumnProperty, 0);
			entry.Completed += this.Handle_Completed;
			entry.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Text"));
			grid2.Children.Add(entry);
			linkButton2.SetValue(Grid.RowProperty, 2);
			linkButton2.SetValue(Grid.ColumnProperty, 1);
			linkButton2.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton2.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton2.Clicked += this.btnSendCmd_Click;
			translate3.Text = "ios_Terminal_Send";
			IMarkupExtension markupExtension6 = translate3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = linkButton2;
			array7[1] = grid2;
			array7[2] = scrollView;
			array7[3] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array7, Button.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(TerminalPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(110, 21)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			linkButton2.Text = obj9;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = linkButton2;
			array8[1] = grid2;
			array8[2] = scrollView;
			array8[3] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array8, Button.TextColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(TerminalPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 21)));
			DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.SetDynamicResource(Button.TextColorProperty, dynamicResource4.Key);
			grid2.Children.Add(linkButton2);
			linkButton3.SetValue(Grid.RowProperty, 3);
			linkButton3.SetValue(Grid.ColumnProperty, 0);
			linkButton3.SetValue(Grid.ColumnSpanProperty, 2);
			linkButton3.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton3.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton3.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton3.Clicked += this.btnPIDScanner_Clicked;
			linkButton3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			linkButton3.SetValue(Button.TextProperty, "PID scanner");
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = linkButton3;
			array9[1] = grid2;
			array9[2] = scrollView;
			array9[3] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array9, Button.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(TerminalPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 21)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton3.SetDynamicResource(Button.TextColorProperty, dynamicResource5.Key);
			grid2.Children.Add(linkButton3);
			scrollView.Content = grid2;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00064FBA File Offset: 0x000631BA
		[CompilerGenerated]
		private void <btnSendCmd_Click>b__16_1(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.OnResponseReceived(s);
			});
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00064FDF File Offset: 0x000631DF
		[CompilerGenerated]
		private void <btnSendCmd_Click>b__16_3(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.OnResponseReceived(s);
			});
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00065004 File Offset: 0x00063204
		[CompilerGenerated]
		private void <btnSendCmd_Click>b__16_5(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.OnResponseReceived(s);
			});
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0006502C File Offset: 0x0006322C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<TerminalPage>(this, typeof(TerminalPage));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.lvTerm = NameScopeExtensions.FindByName<ListView>(this, "lvTerm");
			this.tbCommand = NameScopeExtensions.FindByName<Entry>(this, "tbCommand");
			this.btnSendCmd = NameScopeExtensions.FindByName<LinkButton>(this, "btnSendCmd");
			this.btnPIDScanner = NameScopeExtensions.FindByName<LinkButton>(this, "btnPIDScanner");
		}

		// Token: 0x040004CE RID: 1230
		[CompilerGenerated]
		private static TerminalPage <Instance>k__BackingField;

		// Token: 0x040004CF RID: 1231
		private bool turnOnECUPingAfterFinish;

		// Token: 0x040004D0 RID: 1232
		private List<string> next_commands = new List<string>();

		// Token: 0x040004D1 RID: 1233
		private ObservableCollection<TerminalElement> defaultViewModel = new ObservableCollection<TerminalElement>();

		// Token: 0x040004D2 RID: 1234
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x040004D3 RID: 1235
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x040004D4 RID: 1236
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvTerm;

		// Token: 0x040004D5 RID: 1237
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry tbCommand;

		// Token: 0x040004D6 RID: 1238
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnSendCmd;

		// Token: 0x040004D7 RID: 1239
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPIDScanner;

		// Token: 0x0200013C RID: 316
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06000600 RID: 1536 RVA: 0x000650B0 File Offset: 0x000632B0
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06000602 RID: 1538 RVA: 0x000650BC File Offset: 0x000632BC
			internal bool <btnSendCmd_Click>b__16_0(string x)
			{
				return !string.IsNullOrEmpty(x);
			}

			// Token: 0x06000603 RID: 1539 RVA: 0x000650C7 File Offset: 0x000632C7
			internal bool <GetBuiltInSequence>b__18_0(string x)
			{
				return x.Length == 3;
			}

			// Token: 0x06000604 RID: 1540 RVA: 0x000650D4 File Offset: 0x000632D4
			internal string <GetBuiltInSequence>b__18_1(string header)
			{
				return string.Concat(new string[]
				{
					"ATSH",
					header,
					";ATFCSH",
					header,
					";ATFCSD300000;ATFCSM1;ATCRA",
					CAN11bitHelper.GetPossibleResponseHeader(header, "Audi", null),
					";3E"
				});
			}

			// Token: 0x040004D8 RID: 1240
			public static readonly TerminalPage.<>c <>9 = new TerminalPage.<>c();

			// Token: 0x040004D9 RID: 1241
			public static Func<string, bool> <>9__16_0;

			// Token: 0x040004DA RID: 1242
			public static Func<string, bool> <>9__18_0;

			// Token: 0x040004DB RID: 1243
			public static Func<string, string> <>9__18_1;
		}

		// Token: 0x0200013D RID: 317
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x06000605 RID: 1541 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x06000606 RID: 1542 RVA: 0x00065123 File Offset: 0x00063323
			internal void <btnSendCmd_Click>b__2()
			{
				this.<>4__this.OnResponseReceived(this.s);
			}

			// Token: 0x040004DC RID: 1244
			public string s;

			// Token: 0x040004DD RID: 1245
			public TerminalPage <>4__this;
		}

		// Token: 0x0200013E RID: 318
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_1
		{
			// Token: 0x06000607 RID: 1543 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_1()
			{
			}

			// Token: 0x06000608 RID: 1544 RVA: 0x00065136 File Offset: 0x00063336
			internal void <btnSendCmd_Click>b__4()
			{
				this.<>4__this.OnResponseReceived(this.s);
			}

			// Token: 0x040004DE RID: 1246
			public string s;

			// Token: 0x040004DF RID: 1247
			public TerminalPage <>4__this;
		}

		// Token: 0x0200013F RID: 319
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_2
		{
			// Token: 0x06000609 RID: 1545 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_2()
			{
			}

			// Token: 0x0600060A RID: 1546 RVA: 0x00065149 File Offset: 0x00063349
			internal void <btnSendCmd_Click>b__6()
			{
				this.<>4__this.OnResponseReceived(this.s);
			}

			// Token: 0x040004E0 RID: 1248
			public string s;

			// Token: 0x040004E1 RID: 1249
			public TerminalPage <>4__this;
		}

		// Token: 0x02000140 RID: 320
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnResponseReceived>d__8 : IAsyncStateMachine
		{
			// Token: 0x0600060B RID: 1547 RVA: 0x0006515C File Offset: 0x0006335C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TerminalPage terminalPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						foreach (string text in data.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries))
						{
							terminalPage.DefaultViewModel.Add(new TerminalElement
							{
								ElementType = TerminalElementTypes.Response,
								Text = text
							});
						}
						try
						{
							terminalPage.lvTerm.ScrollTo(terminalPage.DefaultViewModel.LastOrDefault<TerminalElement>(), 3, true);
						}
						catch
						{
						}
						if (terminalPage.next_commands.Count > 0)
						{
							string text2 = terminalPage.next_commands[0];
							terminalPage.next_commands.RemoveAt(0);
							taskAwaiter = terminalPage.SendCommand(text2).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<OnResponseReceived>d__8>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							terminalPage.btnSendCmd.IsEnabled = true;
							if (terminalPage.turnOnECUPingAfterFinish)
							{
								SharedSettings.Current.AlwaysPingECU = true;
								goto IL_0122;
							}
							goto IL_0122;
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
					IL_0122:;
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

			// Token: 0x0600060C RID: 1548 RVA: 0x000652F0 File Offset: 0x000634F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040004E2 RID: 1250
			public int <>1__state;

			// Token: 0x040004E3 RID: 1251
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040004E4 RID: 1252
			public string data;

			// Token: 0x040004E5 RID: 1253
			public TerminalPage <>4__this;

			// Token: 0x040004E6 RID: 1254
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000141 RID: 321
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendCommand>d__20 : IAsyncStateMachine
		{
			// Token: 0x0600060D RID: 1549 RVA: 0x00065300 File Offset: 0x00063500
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TerminalPage terminalPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0172;
						}
						if (cmd.StartsWith("delay=", StringComparison.InvariantCultureIgnoreCase))
						{
							string text = cmd.Substring(6);
							int num3 = 0;
							terminalPage.DefaultViewModel.Add(new TerminalElement
							{
								ElementType = TerminalElementTypes.Command,
								Text = cmd
							});
							if (!int.TryParse(text, out num3))
							{
								goto IL_00C6;
							}
							taskAwaiter = Task.Delay(num3).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<SendCommand>d__20>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							if (!OBDReaderSimulator.Current.IsActive && (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU))
							{
								terminalPage.DefaultViewModel.Add(new TerminalElement
								{
									ElementType = TerminalElementTypes.Command,
									Text = cmd
								});
								OBDRequest obdrequest = new OBDRequest(cmd, "", "", "", false, null);
								if (!obdrequest.Command.ToUpperInvariant().StartsWith("AT"))
								{
									obdrequest.CheckLength = true;
								}
								App.OBDReader.AddRequestToQueue(obdrequest);
								goto IL_026C;
							}
							if (OBDReaderSimulator.Current.IsActive)
							{
								terminalPage.DefaultViewModel.Add(new TerminalElement
								{
									ElementType = TerminalElementTypes.Command,
									Text = cmd
								});
								terminalPage.OnResponseReceived(">>:" + cmd);
								goto IL_026C;
							}
							goto IL_026C;
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
					IL_00C6:
					terminalPage.DefaultViewModel.Add(new TerminalElement
					{
						ElementType = TerminalElementTypes.Response,
						Text = cmd
					});
					if (terminalPage.next_commands.Count <= 0)
					{
						terminalPage.btnSendCmd.IsEnabled = true;
						goto IL_026C;
					}
					cmd = terminalPage.next_commands[0];
					terminalPage.next_commands.RemoveAt(0);
					taskAwaiter = terminalPage.SendCommand(cmd).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<SendCommand>d__20>(ref taskAwaiter, ref this);
						return;
					}
					IL_0172:
					taskAwaiter.GetResult();
					IL_026C:;
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

			// Token: 0x0600060E RID: 1550 RVA: 0x000655C4 File Offset: 0x000637C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040004E7 RID: 1255
			public int <>1__state;

			// Token: 0x040004E8 RID: 1256
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040004E9 RID: 1257
			public string cmd;

			// Token: 0x040004EA RID: 1258
			public TerminalPage <>4__this;

			// Token: 0x040004EB RID: 1259
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000142 RID: 322
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__14 : IAsyncStateMachine
		{
			// Token: 0x0600060F RID: 1551 RVA: 0x000655D4 File Offset: 0x000637D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TerminalPage terminalPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						App.OBDReader.ClearRequestQueue();
						taskAwaiter = terminalPage.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, TerminalPage.<btnBack_Clicked>d__14>(ref taskAwaiter, ref this);
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
					TerminalPage.Instance = null;
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

			// Token: 0x06000610 RID: 1552 RVA: 0x000656AC File Offset: 0x000638AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040004EC RID: 1260
			public int <>1__state;

			// Token: 0x040004ED RID: 1261
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040004EE RID: 1262
			public TerminalPage <>4__this;

			// Token: 0x040004EF RID: 1263
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000143 RID: 323
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSendCmd_Click>d__16 : IAsyncStateMachine
		{
			// Token: 0x06000611 RID: 1553 RVA: 0x000656BC File Offset: 0x000638BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TerminalPage terminalPage = this;
				try
				{
					try
					{
						TaskAwaiter taskAwaiter;
						switch (num)
						{
						case 0:
							break;
						case 1:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_01B7;
						}
						case 2:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0254;
						}
						case 3:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_02F1;
						}
						case 4:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_03A8;
						}
						case 5:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0487;
						}
						case 6:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_04EA;
						}
						default:
							terminalPage.btnSendCmd.IsEnabled = false;
							terminalPage.turnOnECUPingAfterFinish = SharedSettings.Current.AlwaysPingECU;
							SharedSettings.Current.AlwaysPingECU = false;
							cmd = terminalPage.tbCommand.Text.Trim();
							if (!cmd.ToLowerInvariant().StartsWith("www:"))
							{
								goto IL_0132;
							}
							break;
						}
						try
						{
							TaskAwaiter<string> taskAwaiter3;
							if (num != 0)
							{
								string text = cmd.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).First<string>();
								taskAwaiter3 = HttpDownloader.Get("https://www.carscanner.info/terminal/" + text, 10).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, TerminalPage.<btnSendCmd_Click>d__16>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num2 = -1;
							}
							string result = taskAwaiter3.GetResult();
							if (!string.IsNullOrEmpty(result))
							{
								cmd = result;
							}
						}
						catch (Exception)
						{
						}
						IL_0132:
						if (cmd == "[TOYOTAFULL]")
						{
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								MainThread.BeginInvokeOnMainThread(new Action(new TerminalPage.<>c__DisplayClass16_0
								{
									<>4__this = terminalPage,
									s = s
								}.<btnSendCmd_Click>b__2));
							});
							taskAwaiter = new ToyotaUnitsDetector().ReadAllUnits(progress).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<btnSendCmd_Click>d__16>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (cmd == "[NISSANFULL]")
						{
							Progress<string> progress2 = new Progress<string>(delegate(string s)
							{
								MainThread.BeginInvokeOnMainThread(new Action(new TerminalPage.<>c__DisplayClass16_1
								{
									<>4__this = terminalPage,
									s = s
								}.<btnSendCmd_Click>b__4));
							});
							taskAwaiter = new NissanUnitsDetector().ReadAllUnits(progress2).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<btnSendCmd_Click>d__16>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0254;
						}
						else if (cmd == "[BMWFULL]")
						{
							Progress<string> progress3 = new Progress<string>(delegate(string s)
							{
								MainThread.BeginInvokeOnMainThread(new Action(new TerminalPage.<>c__DisplayClass16_2
								{
									<>4__this = terminalPage,
									s = s
								}.<btnSendCmd_Click>b__6));
							});
							taskAwaiter = new BMWUnitsDetector().ReadAllUnits(progress3).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 3;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<btnSendCmd_Click>d__16>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_02F1;
						}
						else if (cmd.StartsWith('['))
						{
							terminalPage.next_commands = terminalPage.GetBuiltInSequence(cmd);
							cmd = terminalPage.next_commands[0];
							terminalPage.next_commands.RemoveAt(0);
							taskAwaiter = terminalPage.SendCommand(cmd).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<btnSendCmd_Click>d__16>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_03A8;
						}
						else if (cmd.Contains(';'))
						{
							terminalPage.next_commands = (from x in cmd.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
								where !string.IsNullOrEmpty(x)
								select x).ToList<string>();
							cmd = terminalPage.next_commands[0];
							terminalPage.next_commands.RemoveAt(0);
							taskAwaiter = terminalPage.SendCommand(cmd).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 5;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<btnSendCmd_Click>d__16>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0487;
						}
						else
						{
							taskAwaiter = terminalPage.SendCommand(cmd).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 6;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TerminalPage.<btnSendCmd_Click>d__16>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_04EA;
						}
						IL_01B7:
						taskAwaiter.GetResult();
						terminalPage.btnSendCmd.IsEnabled = true;
						goto IL_04F1;
						IL_0254:
						taskAwaiter.GetResult();
						terminalPage.btnSendCmd.IsEnabled = true;
						goto IL_04F1;
						IL_02F1:
						taskAwaiter.GetResult();
						terminalPage.btnSendCmd.IsEnabled = true;
						goto IL_04F1;
						IL_03A8:
						taskAwaiter.GetResult();
						goto IL_04F1;
						IL_0487:
						taskAwaiter.GetResult();
						goto IL_04F1;
						IL_04EA:
						taskAwaiter.GetResult();
						IL_04F1:
						terminalPage.tbCommand.Text = "";
						cmd = null;
					}
					catch
					{
						terminalPage.btnSendCmd.IsEnabled = true;
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

			// Token: 0x06000612 RID: 1554 RVA: 0x00065C5C File Offset: 0x00063E5C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040004F0 RID: 1264
			public int <>1__state;

			// Token: 0x040004F1 RID: 1265
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040004F2 RID: 1266
			public TerminalPage <>4__this;

			// Token: 0x040004F3 RID: 1267
			private string <cmd>5__2;

			// Token: 0x040004F4 RID: 1268
			private TaskAwaiter<string> <>u__1;

			// Token: 0x040004F5 RID: 1269
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000144 RID: 324
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_77
		{
			// Token: 0x06000613 RID: 1555 RVA: 0x00065C6C File Offset: 0x00063E6C
			public <InitializeComponent>_anonXamlCDataTemplate_77()
			{
			}

			// Token: 0x06000614 RID: 1556 RVA: 0x00065C80 File Offset: 0x00063E80
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 37);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\TerminalPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				dynamicResourceExtension.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(TerminalPage.<InitializeComponent>_anonXamlCDataTemplate_77).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 37)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label.SetValue(Label.LineBreakModeProperty, 2);
				bindingExtension.Path = "Text";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				bindingExtension2.Path = "Color";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label.SetBinding(Label.TextColorProperty, bindingBase2);
				viewCell.View = label;
				return viewCell;
			}

			// Token: 0x040004F6 RID: 1270
			internal object[] parentValues;

			// Token: 0x040004F7 RID: 1271
			internal TerminalPage root;
		}
	}
}
