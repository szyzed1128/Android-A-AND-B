using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004F9 RID: 1273
	internal class LandRoverECUCAN11bit : VolvoCAN11bitECU
	{
		// Token: 0x06003157 RID: 12631 RVA: 0x0021FA00 File Offset: 0x0021DC00
		public LandRoverECUCAN11bit(string name, string request, string response, bool ms_can)
			: base(name, request, response, ms_can)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC", "190278", "1800FF00", "1802FF00", "17FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x0021FADC File Offset: 0x0021DCDC
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("760", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string>("7E6", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string>("795", "Rear differential"),
				new ValueTuple<string, string>("761", "Transfer case"),
				new ValueTuple<string, string>("792", "Terrain response"),
				new ValueTuple<string, string>("7D3", "Body"),
				new ValueTuple<string, string>("737", "SRS/Airbag"),
				new ValueTuple<string, string>("720", "Dashboard"),
				new ValueTuple<string, string>("726", "BCM"),
				new ValueTuple<string, string>("797", "Steering assist module/SASM"),
				new ValueTuple<string, string>("732", "Gear selection module/GSM"),
				new ValueTuple<string, string>("703", "All wheel drive control module/AWDCM"),
				new ValueTuple<string, string>("433", "All wheel drive control module (B)/AWDB"),
				new ValueTuple<string, string>("751", "TPMS"),
				new ValueTuple<string, string>("706", "Image Processing Module"),
				new ValueTuple<string, string>("710", "Chassis Control Module"),
				new ValueTuple<string, string>("726", "Body Control Module/Gateway Module"),
				new ValueTuple<string, string>("730", "Power Steering Control Module"),
				new ValueTuple<string, string>("731", "Remote function Actuator"),
				new ValueTuple<string, string>("732", "Gear Shift Control Module"),
				new ValueTuple<string, string>("733", "HVAC Control Module"),
				new ValueTuple<string, string>("734", "Headlamp Control Module"),
				new ValueTuple<string, string>("736", "Parking Assist Control Module"),
				new ValueTuple<string, string>("737", "Restraints Control Module"),
				new ValueTuple<string, string>("740", "Driver Door Module"),
				new ValueTuple<string, string>("741", "Passenger Door Module"),
				new ValueTuple<string, string>("742", "Driver Rear Door Module"),
				new ValueTuple<string, string>("743", "Passenger Rear Door Module"),
				new ValueTuple<string, string>("744", "Driver Seat Module"),
				new ValueTuple<string, string>("746", "Rear Electric Power Inverter Converter"),
				new ValueTuple<string, string>("747", "Front Electric Power Inverter Converter"),
				new ValueTuple<string, string>("752", "Occupant Monitoring Model"),
				new ValueTuple<string, string>("753", "Direct Current to Direct Current Converter"),
				new ValueTuple<string, string>("754", "Telematics Control Module"),
				new ValueTuple<string, string>("764", "Adaptative Speed Control Module"),
				new ValueTuple<string, string>("775", "Rear Gate/Trunk Module"),
				new ValueTuple<string, string>("785", "Rear HVAC"),
				new ValueTuple<string, string>("792", "Jaguar Drive Switchpack"),
				new ValueTuple<string, string>("797", "Steering Angle Sensing Module"),
				new ValueTuple<string, string>("7A2", "Interactive Display Module"),
				new ValueTuple<string, string>("7A3", "Passenger Seat Module"),
				new ValueTuple<string, string>("7A4", "Audio Amplifier Module"),
				new ValueTuple<string, string>("7B1", "Camera Module Rear/IPMB"),
				new ValueTuple<string, string>("7B2", "Head-up Display"),
				new ValueTuple<string, string>("7B3", "Infotainment Master Control"),
				new ValueTuple<string, string>("7C3", "Headlight Control Module B"),
				new ValueTuple<string, string>("7C4", "Side Object Detection Control Module - Left"),
				new ValueTuple<string, string>("7C6", "Side Object Detection Control Module - Right"),
				new ValueTuple<string, string>("7E2", "Break Booster Module"),
				new ValueTuple<string, string>("7E4", "Battery Energy Control Module"),
				new ValueTuple<string, string>("7E5", "Battery Charger Control Module"),
				new ValueTuple<string, string>("407", "WDCMB"),
				new ValueTuple<string, string>("4B3", "IGM"),
				new ValueTuple<string, string>("716", "SDLC"),
				new ValueTuple<string, string>("7E3", "HVAC"),
				new ValueTuple<string, string>("7E7", "SGCM (EPICC)"),
				new ValueTuple<string, string>("727", "ACM"),
				new ValueTuple<string, string>("7D0", "APIM"),
				new ValueTuple<string, string>("7D6", "DABM"),
				new ValueTuple<string, string>("7A5", "FCDIM"),
				new ValueTuple<string, string>("7A7", "FCIM"),
				new ValueTuple<string, string>("7A0", "FCIMB"),
				new ValueTuple<string, string>("784", "FEM"),
				new ValueTuple<string, string>("771", "REM"),
				new ValueTuple<string, string>("772", "SPRM"),
				new ValueTuple<string, string>("782", "SRM"),
				new ValueTuple<string, string>("781", "TEL"),
				new ValueTuple<string, string>("707", "TVM")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string>
				{
					"03", "03", "07", "07", "0A", "1902AF", "1902AF", "190278", "1800FF00", "1802FF00",
					"17FF00"
				},
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Land Rover", null);
				LandRoverECUCAN11bit landRoverECUCAN11bit = new LandRoverECUCAN11bit(item2, item, possibleResponseHeader, false);
				list.Add(landRoverECUCAN11bit);
			}
			if (App.OBDReader.STCommandsStupported)
			{
				foreach (ValueTuple<string, string> valueTuple2 in new ValueTuple<string, string>[]
				{
					new ValueTuple<string, string>("7A4", "Audio Amplifier Module"),
					new ValueTuple<string, string>("727", "ACM"),
					new ValueTuple<string, string>("7D0", "APIM"),
					new ValueTuple<string, string>("7D6", "DABM"),
					new ValueTuple<string, string>("740", "Driver Door Module"),
					new ValueTuple<string, string>("744", "Driver Seat Module"),
					new ValueTuple<string, string>("7A7", "FCIM"),
					new ValueTuple<string, string>("7A0", "FCIMB"),
					new ValueTuple<string, string>("784", "FEM"),
					new ValueTuple<string, string>("7C3", "Headlight Control Module B"),
					new ValueTuple<string, string>("733", "HVAC Control Module"),
					new ValueTuple<string, string>("7B1", "Camera Module Rear/IPMB"),
					new ValueTuple<string, string>("731", "Remote function Actuator"),
					new ValueTuple<string, string>("736", "Parking Assist Control Module"),
					new ValueTuple<string, string>("741", "Passenger Door Module"),
					new ValueTuple<string, string>("771", "REM"),
					new ValueTuple<string, string>("772", "SPRM"),
					new ValueTuple<string, string>("782", "SRM"),
					new ValueTuple<string, string>("781", "TEL"),
					new ValueTuple<string, string>("707", "TVM")
				})
				{
					string item3 = valueTuple2.Item1;
					string item4 = valueTuple2.Item2;
					string possibleResponseHeader2 = CAN11bitHelper.GetPossibleResponseHeader(item3, "Land Rover", null);
					LandRoverECUCAN11bit landRoverECUCAN11bit2 = new LandRoverECUCAN11bit(item4 + " (MS-CAN)", item3, possibleResponseHeader2, true);
					list.Add(landRoverECUCAN11bit2);
				}
			}
			return list;
		}

		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06003159 RID: 12633 RVA: 0x00220485 File Offset: 0x0021E685
		public new static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return LandRoverECUCAN11bit.BuildList();
			}
		}
	}
}
