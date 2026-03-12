using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x0200051C RID: 1308
	internal class ToyotaCANECU : CAN11bitECU
	{
		// Token: 0x060031C4 RID: 12740 RVA: 0x00225AB4 File Offset: 0x00223CB4
		public ToyotaCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string>
			{
				"1800FF00", "1802FF00", "18FF00", "17FF00", "13FF00", "13FFFF", "13", "1381", "1902AC", "190278",
				"190208", "190FAC"
			};
			base.ClearDTCCommands = new List<string> { "04", "14", "14FF00", "14FFFF", "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			base.IdentsASCII.TryAdd("1A81", "Part num.");
			base.IdentsASCII.TryAdd("1A8881", "Calibration ID");
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x00225BF8 File Offset: 0x00223DF8
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME + " #1"),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME + " #1"),
				new ValueTuple<string, string>("700", CAN11bitECU.ECU_ENGINE_NAME + " #2"),
				new ValueTuple<string, string>("701", CAN11bitECU.ECU_TRANSMISSION_NAME + " #2"),
				new ValueTuple<string, string>("7A2", CAN11bitECU.ECU_ENGINE_NAME + " #3 OR Self parking system"),
				new ValueTuple<string, string>("7A3", CAN11bitECU.ECU_TRANSMISSION_NAME + " #2 OR Steering (VGRS)"),
				new ValueTuple<string, string>("760", CAN11bitECU.ECU_ABS_NAME + " #1"),
				new ValueTuple<string, string>("7B0", CAN11bitECU.ECU_ABS_NAME + " #2"),
				new ValueTuple<string, string>("780", "SRS/Airbag #1"),
				new ValueTuple<string, string>("737", "SRS/Airbag #2"),
				new ValueTuple<string, string>("7E2", "Hybrid engine system"),
				new ValueTuple<string, string>("7E3", "HV battery"),
				new ValueTuple<string, string>("7E7", "Plug-in control"),
				new ValueTuple<string, string>("703", "Solar Charging Control"),
				new ValueTuple<string, string>("705", "Rear motor generator"),
				new ValueTuple<string, string>("723", "Active engine mount"),
				new ValueTuple<string, string>("724", "Motor generator"),
				new ValueTuple<string, string>("726", "Start and stop/BCM"),
				new ValueTuple<string, string>("727", "Gear selector switch"),
				new ValueTuple<string, string>("730", "Power steering"),
				new ValueTuple<string, string>("731", "Smart start"),
				new ValueTuple<string, string>("733", "Electronic air temperature control"),
				new ValueTuple<string, string>("734", "Air suspension/Auto-leveling suspension"),
				new ValueTuple<string, string>("741", "4WD"),
				new ValueTuple<string, string>("744", "Dynamic rear steering"),
				new ValueTuple<string, string>("745", "Plug-in Control"),
				new ValueTuple<string, string>("746", "Diagnostic recorder"),
				new ValueTuple<string, string>("747", "HV Battery"),
				new ValueTuple<string, string>("750", "Gateway"),
				new ValueTuple<string, string>("781", "Pre-collision safety"),
				new ValueTuple<string, string>("784", "Telematics"),
				new ValueTuple<string, string>("790", "Adaptive cruise control"),
				new ValueTuple<string, string>("791", "Radar cruise control/Pre-collision safety"),
				new ValueTuple<string, string>("792", "LKA/LDA"),
				new ValueTuple<string, string>("793", "Smart city brake support"),
				new ValueTuple<string, string>("7A1", "Steering (EMPS/EHPS/PPS)"),
				new ValueTuple<string, string>("7AA", "Parking assist"),
				new ValueTuple<string, string>("7B1", "4WD"),
				new ValueTuple<string, string>("7B2", "Charging control"),
				new ValueTuple<string, string>("7B3", "Steering angle"),
				new ValueTuple<string, string>("7B4", "Starter electronics"),
				new ValueTuple<string, string>("7B7", "Electric supply"),
				new ValueTuple<string, string>("7C0", "Dashboard/Instrument cluster #1"),
				new ValueTuple<string, string>("7C1", "Electric power control"),
				new ValueTuple<string, string>("7C4", "Climate/Heater"),
				new ValueTuple<string, string>("7D0", "Multimedia/Navigation"),
				new ValueTuple<string, string>("7D2", "Electric propulsion control")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string>
				{
					"03", "03", "07", "07", "0A", "1800FF00", "1802FF00", "18FF00", "17FF00", "13FF00",
					"13FFFF", "13", "1381", "1902AC", "190278", "190208", "190FAC"
				},
				ClearDTCCommands = new List<string> { "04", "04", "04", "14", "14FF00", "14FFFF", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Toyota", null);
				ToyotaCANECU toyotaCANECU = new ToyotaCANECU(item2, item, possibleResponseHeader);
				list.Add(toyotaCANECU);
			}
			list.AddRange(ToyotaCANGatewayECU.ECUs);
			return list;
		}

		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x0022620E File Offset: 0x0022440E
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return ToyotaCANECU.BuildList();
			}
		}
	}
}
