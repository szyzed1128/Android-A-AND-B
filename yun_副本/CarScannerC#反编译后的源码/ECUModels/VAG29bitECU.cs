using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x0200051F RID: 1311
	internal class VAG29bitECU : CAN29bitECU, IVagECU
	{
		// Token: 0x060031CD RID: 12749 RVA: 0x00226D64 File Offset: 0x00224F64
		public VAG29bitECU(string name, string requestHeader, string responseHeader)
			: base(name, requestHeader, responseHeader)
		{
			this.Protocol = 7;
			base.OpenSessionCommands = new List<string> { "1003" };
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC", "1800FF00", "1802FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			base.IdentsASCII["22F187"] = "VAG Part number";
			base.IdentsASCII["22F191"] = "Hardware Part number";
			base.IdentsASCII["22F19E"] = "ASAM/ODX";
			base.IdentsASCII["22F189"] = "Software version";
			base.IdentsASCII["22F1A3"] = "Hardware version";
			base.IdentsASCII["22F197"] = "System designation";
			base.IdentsASCII["22F1A2"] = "ASAM/ODX version";
			base.IdentsASCII["22F1B1"] = "Application software identification (id/version/valid/plausible/used)";
			base.IdentsHEX["222A2F"] = "Unit ID";
			base.IdentsHEX["22F17B"] = "Date of last coding";
			base.IdentsASCII["22F17C"] = "FAZIT Identification";
			base.IdentsHEX["22F186"] = "Diagnostic mode";
			base.IdentsHEX["22F19B"] = "Date of last adaptation";
			base.IdentsASCII["22F1A0"] = "Parameter set part number";
			base.IdentsASCII["22F1A1"] = "Parameter set version";
			base.IdentsHEX["22F1A9"] = "Parametrization date";
			base.IdentsASCII["22F1AA"] = "System abbreviation";
			if (base.RequestHeader == "17FC0076" || base.RequestHeader == "FC0076")
			{
				base.IdentsASCII["22F1AD"] = "Engine code";
			}
			if (base.RequestHeader == "773")
			{
				base.IdentsASCII.TryAdd("223C0A", "VCRN");
			}
			base.IdentsHEX.TryAdd("220600", "Long Coding");
			if (base.IdentsHEX.ContainsKey("22F191"))
			{
				base.IdentsHEX.Remove("22F191");
			}
			if (base.IdentsASCII.ContainsKey("22F19A"))
			{
				base.IdentsASCII.Remove("22F19A");
			}
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x00227038 File Offset: 0x00225238
		protected override string DecodeAsASCII(string command, byte[] data, bool filterPrintable)
		{
			if (!(command == "22F1B1"))
			{
				return base.DecodeAsASCII(command, data, filterPrintable);
			}
			if (data == null || data.Length == 0 || data.Length < 7)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i += 7)
			{
				string text = data[i].ToString("X2") + data[i + 1].ToString("X2");
				string @string = Encoding.ASCII.GetString(data, i + 2, 4);
				byte b = data[i + 6];
				stringBuilder.Append(string.Concat(new string[]
				{
					text,
					"/",
					@string,
					"/",
					VAGECU.BoolToIntString(BitHelpers.GetBit_0_7(b, 0)),
					"/",
					VAGECU.BoolToIntString(BitHelpers.GetBit_0_7(b, 1)),
					"/",
					VAGECU.BoolToIntString(BitHelpers.GetBit_0_7(b, 2)),
					"; "
				}));
			}
			string text2 = stringBuilder.ToString();
			if (text2[text2.Length - 1] == '\n')
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			return text2;
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x00227170 File Offset: 0x00225370
		private static List<IECU> BuildList()
		{
			return new List<IECU>
			{
				new VAG29bitECU("01. Engine (EVO/29bit)", "17FC0076", "17FE0076"),
				new VAG29bitECU("02. Gearbox (EVO/29bit)", "17FC0077", "17FE0077"),
				new VAG29bitECU("2A. Wireless charging control (29 bit)", "17FC008F", "17FE008F"),
				new VAG29bitECU("43. Brake Electronics 2 (29 bit)", "17FC00C8", "17FE00C8"),
				new VAG29bitECU("46. Comfort unit (29 bit)", "17FC008B", "17FE008B"),
				new VAG29bitECU("4B. Multi-functional unit (29 bit)", "17FC00A9", "17FE00A9"),
				new VAG29bitECU("5B. Seat adjustment, rear passenger side (29 bit)", "17FC0088", "17FE0088"),
				new VAG29bitECU("74. Chassis Control (29 bit)", "17FC0080", "17FE0080"),
				new VAG29bitECU("80. Battery control: additional memory 1 (29 bit)", "17FC00AE", "17FE00AE"),
				new VAG29bitECU("0051. Electro-engine (29 bit)", "17FC007C", "17FE007C"),
				new VAG29bitECU("008C. Battery management system (29 bit)", "17FC007B", "17FE007B"),
				new VAG29bitECU("CA. Sunroof control module (29 bit)", "17FC0084", "17FE0084"),
				new VAG29bitECU("CC. Starter generator (29 bit)", "17FC0085", "17FE0085"),
				new VAG29bitECU("CD. Adaptive cruise control laser (29 bit)", "17FC0089", "17FE0089"),
				new VAG29bitECU("CE. Electric drive 2 (29 bit)", "17FC00B8", "17FE00B8"),
				new VAG29bitECU("CF. Lane change assist 2 (29 bit)", "17FC008A", "17FE008A"),
				new VAG29bitECU("D4. Sway stabilization 1 (29 bit)", "17FC0091", "17FE0091"),
				new VAG29bitECU("D5. Sway stabilization 2 (29 bit)", "17FC0092", "17FE0092"),
				new VAG29bitECU("D6. Headlight control left 2 (29 bit)", "17FC0096", "17FE0096"),
				new VAG29bitECU("D7. Headlight control right 2 (29 bit)", "17FC0097", "17FE0097"),
				new VAG29bitECU("D8. Projection module in left matrix headlamp (29 bit)", "17FC0098", "17FE0098"),
				new VAG29bitECU("D9. Projection module in left matrix headlamp (29 bit)", "17FC0099", "17FE0099"),
				new VAG29bitECU("DB. Front corner radar #1 (29 bit)", "17FC009D", "17FE009D"),
				new VAG29bitECU("DC. Front corner radar #2 (29 bit)", "17FC009E", "17FE009E"),
				new VAG29bitECU("DE. Wireless charging (29 bit)", "17FC00A5", "17FE00A5"),
				new VAG29bitECU("DD. Roof control #2 (29 bit)", "17FC00A0", "17FE00A0"),
				new VAG29bitECU("E0. Auxiliary display/control head 1 (29 bit)", "17FC00AA", "17FE00AA"),
				new VAG29bitECU("0601. Wiper module control (29 bit)", "17FC1601", "17FE1601"),
				new VAG29bitECU("0604. Garage door opener (29 bit)", "17FC1604", "17FE1604"),
				new VAG29bitECU("060A. Left LED headlight control 1 (29 bit)", "17FC160A", "17FE160A"),
				new VAG29bitECU("060B. Right LED headlight control 1 (29 bit)", "17FC160B", "17FE160B"),
				new VAG29bitECU("061B. Fresh air blower control - front (29 bit)", "17FC161B", "17FE161B"),
				new VAG29bitECU("061E. Interior light module (29 bit)", "17FC161E", "17FE161E"),
				new VAG29bitECU("0625. Left tail lamp 1 (29 bit)", "17FC1625", "17FE1625"),
				new VAG29bitECU("0637. Battery monitoring (29 bit)", "17FC1637", "17FE1637"),
				new VAG29bitECU("0649. Adjustable steering column (29 bit)", "17FC1649", "17FE1649"),
				new VAG29bitECU("0651. Rear spoiler control (29 bit)", "17FC1651", "17FE1651"),
				new VAG29bitECU("065D. A/C compressor stub (29 bit)", "17FC165D", "17FE165D"),
				new VAG29bitECU("068B. Bend angle sensor for trailer (29 bit)", "17FC168B", "17FE168B"),
				new VAG29bitECU("0697. Active accelerator pedal (29 bit)", "17FC1697", "17FE1697"),
				new VAG29bitECU("06C0. Coolant heating (29 bit)", "17FC16C0", "17FE16C0"),
				new VAG29bitECU("06D0. Battery interrupt switch (29 bit)", "17FC16D0", "17FE16D0"),
				new VAG29bitECU("06D1.  Battery cell control module 1 (29 bit)", "17FC16D1", "17FE16D1"),
				new VAG29bitECU("06D2.  Battery cell control module 2 (29 bit)", "17FC16D2", "17FE16D2"),
				new VAG29bitECU("06D3.  Battery cell control module 3 (29 bit)", "17FC16D3", "17FE16D3"),
				new VAG29bitECU("06D4.  Battery cell control module 4 (29 bit)", "17FC16D4", "17FE16D4"),
				new VAG29bitECU("06D5.  Battery cell control module 5 (29 bit)", "17FC16D5", "17FE16D5"),
				new VAG29bitECU("06D6.  Battery cell control module 6 (29 bit)", "17FC16D6", "17FE16D6"),
				new VAG29bitECU("06D7.  Battery cell control module 7 (29 bit)", "17FC16D7", "17FE16D7"),
				new VAG29bitECU("06D8.  Battery cell control module 8 (29 bit)", "17FC16D8", "17FE16D8"),
				new VAG29bitECU("06D9.  Battery cell control module 9 (29 bit)", "17FC16D9", "17FE16D9"),
				new VAG29bitECU("06D9.  Battery cell control module 9 (29 bit)", "17FC16D9", "17FE16D9"),
				new VAG29bitECU("06DA.  Battery cell control module 10 (29 bit)", "17FC16DA", "17FE16DA"),
				new VAG29bitECU("06DB.  Battery cell control module 11 (29 bit)", "17FC16DB", "17FE16DB"),
				new VAG29bitECU("06DC.  Battery cell control module 12 (29 bit)", "17FC16DC", "17FE16DC"),
				new VAG29bitECU("06EB.  Short-range communication antenna (29 bit)", "17FC16EB", "17FE16EB"),
				new VAG29bitECU("06F7.  ATF pump (29 bit)", "17FC16F7", "17FE16F7"),
				new VAG29bitECU("06F8.  MTF pump (29 bit)", "17FC16F8", "17FE16F8"),
				new VAG29bitECU("0701.  Rear A/C control head (29 bit)", "17FC1701", "17FE1701"),
				new VAG29bitECU("070B.  Short-range communication antenna #2 (29 bit)", "17FC170B", "17FE170B"),
				new VAG29bitECU("070C.  Clutch position electronics (29 bit)", "17FC170C", "17FE170C"),
				new VAG29bitECU("070D.  Electric compressor (29 bit)", "17FC170D", "17FE170D"),
				new VAG29bitECU("070E.  Left tail lamp 2 (29 bit)", "17FC170E", "17FE170E"),
				new VAG29bitECU("070F.  Right rear tail lamp 1 (29 bit)", "17FC170F", "17FE170F"),
				new VAG29bitECU("0710.  Right rear tail lamp 2 (29 bit)", "17FC1710", "17FE1710"),
				new VAG29bitECU("0722.  Wireless control panel 1 (29 bit)", "17FC1722", "17FE1722"),
				new VAG29bitECU("0722.  Wireless control panel 1 (29 bit)", "17FC1722", "17FE1722"),
				new VAG29bitECU("0724.  Front windshield washer pump (29 bit)", "17FC1724", "17FE1724"),
				new VAG29bitECU("0726.  Fragarance system (29 bit)", "17FC1726", "17FE1726"),
				new VAG29bitECU("0738.  Electrical gear actuator (29 bit)", "17FC1738", "17FE1738"),
				new VAG29bitECU("073E.  Steering wheel touch recognition (29 bit)", "17FC173E", "17FE173E"),
				new VAG29bitECU("0742.  Parking lock (29 bit)", "17FC1742", "17FE1742"),
				new VAG29bitECU("0767.  Dynamic front left turn signal (29 bit)", "17FC1767", "17FE1767"),
				new VAG29bitECU("0768.  Dynamic front right turn signal (29 bit)", "17FC1768", "17FE1768"),
				new VAG29bitECU("07AB.  Gearshift cover (29 bit)", "17FC17AB", "17FE17AB"),
				new VAG29bitECU("8104. High voltage converter (29 bit)", "17FC00B7", "17FE00B7"),
				new VAG29bitECU("8105. DC Charger (29 bit)", "17FC00B9", "17FE00B9"),
				new VAG29bitECU("8107. Antenna module (29 bit)", "17FC00BA", "17FE00BA"),
				new VAG29bitECU("8111. Right exterior rear view mirror (29 bit)", "17FC00C5", "17FE00C5"),
				new VAG29bitECU("8112. Left exterior rear view mirror (29 bit)", "17FC00C4", "17FE00C4"),
				new VAG29bitECU("8113. HV Battery charging unit 2 (29 bit)", "17FC00C6", "17FE00C6"),
				new VAG29bitECU("8114. HV converter 2 (29 bit)", "17FC00C7", "17FE00C7"),
				new VAG29bitECU("81D0. Central control for digital matrix light (29 bit)", "17FC009B", "17FE009B"),
				new VAG29bitECU("BE01. HV Charging cable 1 (29 bit)", "180C00CE", "180E00CE")
			};
		}

		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x00227A0A File Offset: 0x00225C0A
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return VAG29bitECU.BuildList();
			}
		}

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x060031D1 RID: 12753 RVA: 0x00227A11 File Offset: 0x00225C11
		// (set) Token: 0x060031D2 RID: 12754 RVA: 0x00227A19 File Offset: 0x00225C19
		public bool IsDetectedAsExisting
		{
			[CompilerGenerated]
			get
			{
				return this.<IsDetectedAsExisting>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsDetectedAsExisting>k__BackingField = value;
			}
		}

		// Token: 0x04001CC8 RID: 7368
		[CompilerGenerated]
		private bool <IsDetectedAsExisting>k__BackingField;
	}
}
