using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x0200051D RID: 1309
	internal class ToyotaCANGatewayECU : CAN11bitECU
	{
		// Token: 0x060031C7 RID: 12743 RVA: 0x00226218 File Offset: 0x00224418
		public ToyotaCANGatewayECU(string name, string extendedAddress)
		{
			this.Name = name;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "13", "1381", "13FF00", "13B0", "1380" };
			base.ClearDTCCommands = new List<string> { "04", "14", "14FF00", "14FFFF", "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			base.RequestHeader = "750";
			base.ResponseHeader = "758";
			base.ExtendedAddress = extendedAddress;
			base.TesterAddress = extendedAddress;
			base.IdentsASCII.TryAdd("1A81", "Part num.");
			base.IdentsASCII.TryAdd("1A8881", "Calibration ID");
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x00226324 File Offset: 0x00224524
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("02", "LKA/LDA"),
				new ValueTuple<string, string>("0F", "Front radar sensor"),
				new ValueTuple<string, string>("26", "Windshield wipers"),
				new ValueTuple<string, string>("2A", "TPMS"),
				new ValueTuple<string, string>("2B", "Trailer brake"),
				new ValueTuple<string, string>("2C", "Electric parking brake"),
				new ValueTuple<string, string>("30", "Pedestrian protection"),
				new ValueTuple<string, string>("36", "Air suspension/Auto-leveling suspension #1"),
				new ValueTuple<string, string>("38", "Air suspension/Auto-leveling suspension #2"),
				new ValueTuple<string, string>("39", "Front stabilizer"),
				new ValueTuple<string, string>("3A", "Rear stabilizer"),
				new ValueTuple<string, string>("3B", "Adaptive suspension"),
				new ValueTuple<string, string>("40", "BCM #1/Convenience systems"),
				new ValueTuple<string, string>("41", "Blind spot monitor"),
				new ValueTuple<string, string>("42", "Blind spot monitor (slave)"),
				new ValueTuple<string, string>("43", "BCM #2"),
				new ValueTuple<string, string>("44", "Pre-collision sensor"),
				new ValueTuple<string, string>("4C", "BCM #3"),
				new ValueTuple<string, string>("4F", "BCM #4/Rear electronics"),
				new ValueTuple<string, string>("51", "BCM #5/Headlights"),
				new ValueTuple<string, string>("52", "CAN gateway"),
				new ValueTuple<string, string>("54", "ECM gateway"),
				new ValueTuple<string, string>("55", "ECB gateway"),
				new ValueTuple<string, string>("56", "WIL gateway"),
				new ValueTuple<string, string>("57", "PM1 gateway"),
				new ValueTuple<string, string>("5B", "Occupant detection"),
				new ValueTuple<string, string>("5F", "Central gateway"),
				new ValueTuple<string, string>("60", "Night view"),
				new ValueTuple<string, string>("67", "Parking sensors"),
				new ValueTuple<string, string>("68", "Night view system"),
				new ValueTuple<string, string>("69", "Head up display"),
				new ValueTuple<string, string>("6D", "Single-lens camera sensor"),
				new ValueTuple<string, string>("70", "Adaptive Front light System (AFS)"),
				new ValueTuple<string, string>("79", "Road sign assist"),
				new ValueTuple<string, string>("7A", "LKA/LDA #2"),
				new ValueTuple<string, string>("7B", "All-round vision camera"),
				new ValueTuple<string, string>("7C", "High beam assist"),
				new ValueTuple<string, string>("80", "Passenger seat"),
				new ValueTuple<string, string>("81", "Front corner radar master"),
				new ValueTuple<string, string>("82", "Front corner radar slave"),
				new ValueTuple<string, string>("83", "Rear right seat"),
				new ValueTuple<string, string>("85", "Rear left seat"),
				new ValueTuple<string, string>("8A", "Front right seat A/C"),
				new ValueTuple<string, string>("8B", "Front left seat A/C"),
				new ValueTuple<string, string>("8C", "Rear right seat A/C"),
				new ValueTuple<string, string>("8D", "Rear left seat A/C"),
				new ValueTuple<string, string>("90", "Electric window/Driver door"),
				new ValueTuple<string, string>("91", "Electric window/Passenger door"),
				new ValueTuple<string, string>("92", "Electric window/Rear right door"),
				new ValueTuple<string, string>("93", "Electric window/Rear left door"),
				new ValueTuple<string, string>("A0", "Ceiling illumination"),
				new ValueTuple<string, string>("A1", "Driver's door"),
				new ValueTuple<string, string>("A2", "Front passenger's door"),
				new ValueTuple<string, string>("A4", "Rear right door electronics"),
				new ValueTuple<string, string>("A5", "Right mirror"),
				new ValueTuple<string, string>("A6", "Left mirror"),
				new ValueTuple<string, string>("A7", "Rear left door electronics"),
				new ValueTuple<string, string>("A8", "Steering column"),
				new ValueTuple<string, string>("AA", "Sliding roof sunshade (rear)"),
				new ValueTuple<string, string>("AB", "Driver seat"),
				new ValueTuple<string, string>("AC", "Sliding roof sunshade"),
				new ValueTuple<string, string>("AE", "Active noise control"),
				new ValueTuple<string, string>("AD", "Sunroof/Sliding roof"),
				new ValueTuple<string, string>("C8", "Accessory gateway"),
				new ValueTuple<string, string>("B0", "Rain sensor"),
				new ValueTuple<string, string>("B5", "Entry & start / Smart key"),
				new ValueTuple<string, string>("B6", "Remote engine starter"),
				new ValueTuple<string, string>("B8", "Tailgate/Rear hatch"),
				new ValueTuple<string, string>("B9", "Tailgate/Rear hatch window motor"),
				new ValueTuple<string, string>("E0", "Theft deterrent"),
				new ValueTuple<string, string>("E9", "Power source control"),
				new ValueTuple<string, string>("EB", "Grill shutters"),
				new ValueTuple<string, string>("EC", "Electric windows main switch/control"),
				new ValueTuple<string, string>("C7", "Telematics"),
				new ValueTuple<string, string>("C8", "Accessory network gateway"),
				new ValueTuple<string, string>("D3", "Special vehicle assist"),
				new ValueTuple<string, string>("D4", "Sub battery"),
				new ValueTuple<string, string>("DA", "Steering wheel"),
				new ValueTuple<string, string>("DD", "Steering column stalks/switches"),
				new ValueTuple<string, string>("DC", "Driver seat switches"),
				new ValueTuple<string, string>("DD", "Center console switches"),
				new ValueTuple<string, string>("DE", "Rear console switches"),
				new ValueTuple<string, string>("F3", "Rear spoiler")
			};
			List<IECU> list = new List<IECU>();
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				ToyotaCANGatewayECU toyotaCANGatewayECU = new ToyotaCANGatewayECU(valueTuple.Item2, item);
				list.Add(toyotaCANGatewayECU);
			}
			return list;
		}

		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x00226ADD File Offset: 0x00224CDD
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return ToyotaCANGatewayECU.BuildList();
			}
		}
	}
}
