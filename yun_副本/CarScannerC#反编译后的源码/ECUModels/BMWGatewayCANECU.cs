using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004B3 RID: 1203
	internal class BMWGatewayCANECU : CAN11bitECU
	{
		// Token: 0x06002FD9 RID: 12249 RVA: 0x00213CA4 File Offset: 0x00211EA4
		public BMWGatewayCANECU(string name, string extendedAddress)
		{
			this.Name = name;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "3E", "3E00" };
			base.ReadDTCCommands = new List<string> { "1902AC", "1902AF", "1800FFFF", "1802FFFF" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			base.RequestHeader = "6F1";
			base.ResponseHeader = "6" + extendedAddress;
			base.ExtendedAddress = extendedAddress;
			base.TesterAddress = "F1";
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x00213D84 File Offset: 0x00211F84
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("12", CAN11bitECU.ECU_ENGINE_NAME + " #1"),
				new ValueTuple<string, string>("0B", CAN11bitECU.ECU_ENGINE_NAME + " #2"),
				new ValueTuple<string, string>("18", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("01", "SRS/Airbag"),
				new ValueTuple<string, string>("00", "IPDM/Junction box (JBBF)"),
				new ValueTuple<string, string>("29", "DSC/Traction control"),
				new ValueTuple<string, string>("02", "Steering column"),
				new ValueTuple<string, string>("06", "All-round vision camera"),
				new ValueTuple<string, string>("07", "Battery management system"),
				new ValueTuple<string, string>("08", "Lane-change system"),
				new ValueTuple<string, string>("09", "Left B-pillar safety module"),
				new ValueTuple<string, string>("0A", "Infotainment display"),
				new ValueTuple<string, string>("0E", "Center safety unit"),
				new ValueTuple<string, string>("0F", "Rear differential lock"),
				new ValueTuple<string, string>("10", "CAN gateway"),
				new ValueTuple<string, string>("16", "Active steering"),
				new ValueTuple<string, string>("17", "Fuel pump"),
				new ValueTuple<string, string>("19", "Transmission/4WD"),
				new ValueTuple<string, string>("1C", "Suspension/Dynamic management"),
				new ValueTuple<string, string>("20", "TPMS"),
				new ValueTuple<string, string>("21", "Adaptive cruise control"),
				new ValueTuple<string, string>("22", "Adaptive headlights"),
				new ValueTuple<string, string>("23", "Active roll stabilization"),
				new ValueTuple<string, string>("24", "Convertible top"),
				new ValueTuple<string, string>("26", "Infotainment system"),
				new ValueTuple<string, string>("27", "Keyless ignition"),
				new ValueTuple<string, string>("29", "ABS/DSC"),
				new ValueTuple<string, string>("2A", "Parking brake"),
				new ValueTuple<string, string>("2B", "Rear axle slip angle control"),
				new ValueTuple<string, string>("2C", "Parking/Steering assist"),
				new ValueTuple<string, string>("2E", "Power control unit"),
				new ValueTuple<string, string>("30", "Steering"),
				new ValueTuple<string, string>("31", "Multimedia/DVD Changer"),
				new ValueTuple<string, string>("32", "CAN gateway"),
				new ValueTuple<string, string>("35", "Treble bass extender"),
				new ValueTuple<string, string>("36", "Telephone"),
				new ValueTuple<string, string>("37", "Audio amplifier"),
				new ValueTuple<string, string>("38", "Air suspension"),
				new ValueTuple<string, string>("39", "Electronic damper control (EDC)"),
				new ValueTuple<string, string>("3A", "Headphone interface"),
				new ValueTuple<string, string>("3B", "Navigation"),
				new ValueTuple<string, string>("3C", "CD changer"),
				new ValueTuple<string, string>("3D", "Heads-up display"),
				new ValueTuple<string, string>("3F", "Sound system"),
				new ValueTuple<string, string>("40", "Car access system"),
				new ValueTuple<string, string>("41", "Alarm"),
				new ValueTuple<string, string>("42", "Right headlight"),
				new ValueTuple<string, string>("43", "Micro power supply"),
				new ValueTuple<string, string>("44", "Sliding roof"),
				new ValueTuple<string, string>("45", "Rain/light sensor"),
				new ValueTuple<string, string>("47", "Tuner"),
				new ValueTuple<string, string>("48", "Video switch"),
				new ValueTuple<string, string>("49", "Security module 1"),
				new ValueTuple<string, string>("4A", "Security module 2"),
				new ValueTuple<string, string>("4B", "Video"),
				new ValueTuple<string, string>("4D", "Electric motor-driver reel (left)"),
				new ValueTuple<string, string>("4E", "Electric motor-driver reel (right)"),
				new ValueTuple<string, string>("50", "Alarm sensors"),
				new ValueTuple<string, string>("53", "Radio"),
				new ValueTuple<string, string>("54", "Satelite radio"),
				new ValueTuple<string, string>("55", "Bluetooth"),
				new ValueTuple<string, string>("56", "Center roof module"),
				new ValueTuple<string, string>("57", "Night view"),
				new ValueTuple<string, string>("59", "Active backrest (driver)"),
				new ValueTuple<string, string>("5A", "Active backrest (passenger)"),
				new ValueTuple<string, string>("5B", "Digital radio"),
				new ValueTuple<string, string>("5C", "Emergency response unit"),
				new ValueTuple<string, string>("5D", "Camera-based driver assist system"),
				new ValueTuple<string, string>("5E", "Gear selector switch"),
				new ValueTuple<string, string>("5F", "High beam assist"),
				new ValueTuple<string, string>("60", "Dashboard/Instruments cluster"),
				new ValueTuple<string, string>("61", "Flexible bus interface"),
				new ValueTuple<string, string>("62", "Central gateway"),
				new ValueTuple<string, string>("63", "Infotainment system"),
				new ValueTuple<string, string>("64", "Parking sensors"),
				new ValueTuple<string, string>("65", "Center console switches"),
				new ValueTuple<string, string>("67", "Central electronics"),
				new ValueTuple<string, string>("68", "Rear compartment controller"),
				new ValueTuple<string, string>("69", "Rear seat (driver side)"),
				new ValueTuple<string, string>("6A", "Rear seat (passenger side)"),
				new ValueTuple<string, string>("6B", "Tailgate/Rear hatch"),
				new ValueTuple<string, string>("6D", "Driver's seat"),
				new ValueTuple<string, string>("6E", "Front passenger seat"),
				new ValueTuple<string, string>("70", "Light switch"),
				new ValueTuple<string, string>("71", "Trailer"),
				new ValueTuple<string, string>("72", "Central module (in driver's footwell) (FRM)"),
				new ValueTuple<string, string>("73", "Infotainment display"),
				new ValueTuple<string, string>("74", "Infotainment display (rear)"),
				new ValueTuple<string, string>("75", "Rear compartment display 2"),
				new ValueTuple<string, string>("76", "Vertical dynamics management"),
				new ValueTuple<string, string>("78", "Climate/heater/Air conditioning"),
				new ValueTuple<string, string>("79", "Rear heater and A/C"),
				new ValueTuple<string, string>("7A", "Auxiliary heater"),
				new ValueTuple<string, string>("A0", "Right B-pillar safety module"),
				new ValueTuple<string, string>("A1", "Left B-pillar safety module"),
				new ValueTuple<string, string>("A2", "Right B-pillar safety module"),
				new ValueTuple<string, string>("A5", "Damper control (rear left)"),
				new ValueTuple<string, string>("A6", "Damper control (rear right)"),
				new ValueTuple<string, string>("A7", "Damper control (front left)"),
				new ValueTuple<string, string>("A8", "Damper control (front right)"),
				new ValueTuple<string, string>("AD", "Front left door"),
				new ValueTuple<string, string>("AE", "Front right door")
			};
			List<IECU> list = new List<IECU>(array.Length + 3);
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AC", "1902AF", "1800FFFF", "1802FFFF" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFF", "14FFFFFF" }
			});
			OBD2Can11bitECU obd2Can11bitECU = new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "1902AC", "1902AF", "1800FFFF", "1802FFFF" },
				ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14FFFF" },
				OpenSessionCommands = new List<string>(),
				CloseSessionCommands = new List<string>(),
				Protocol = 6,
				RequestHeader = "7E0",
				ResponseHeader = "7E8",
				Name = CAN11bitECU.ECU_ENGINE_NAME
			};
			list.Add(obd2Can11bitECU);
			OBD2Can11bitECU obd2Can11bitECU2 = new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "1902AC", "1902AF", "1800FFFF", "1802FFFF" },
				ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14FFFF" },
				OpenSessionCommands = new List<string>(),
				CloseSessionCommands = new List<string>(),
				Protocol = 6,
				RequestHeader = "7E1",
				ResponseHeader = "7E9",
				Name = CAN11bitECU.ECU_TRANSMISSION_NAME
			};
			list.Add(obd2Can11bitECU2);
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				BMWGatewayCANECU bmwgatewayCANECU = new BMWGatewayCANECU(valueTuple.Item2, item);
				list.Add(bmwgatewayCANECU);
			}
			return list;
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06002FDB RID: 12251 RVA: 0x00214939 File Offset: 0x00212B39
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return BMWGatewayCANECU.BuildList();
			}
		}
	}
}
