using System;
using System.Collections.Generic;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x0200051A RID: 1306
	internal class SubaruCANECU : CAN11bitECU
	{
		// Token: 0x060031BD RID: 12733 RVA: 0x00225318 File Offset: 0x00223518
		public SubaruCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC", "1800FF00", "1802FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			base.IdentsASCII.TryAdd("22F18E", "Part num.");
			base.IdentsASCII.TryAdd("22F189", "Soft ver.");
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x00225404 File Offset: 0x00223604
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME + " #1"),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME + " #1"),
				new ValueTuple<string, string>("7A2", CAN11bitECU.ECU_ENGINE_NAME + " #2"),
				new ValueTuple<string, string>("7A3", CAN11bitECU.ECU_TRANSMISSION_NAME + " #2"),
				new ValueTuple<string, string>("7B0", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string>("752", "BCM"),
				new ValueTuple<string, string>("780", "SRS/Airbag"),
				new ValueTuple<string, string>("7C4", "A/C, Heater), Climate"),
				new ValueTuple<string, string>("783", "Dashboard"),
				new ValueTuple<string, string>("7D5", "Display"),
				new ValueTuple<string, string>("782", "Start-Stop system"),
				new ValueTuple<string, string>("7A2", "Eyesight"),
				new ValueTuple<string, string>("747", "Headlights"),
				new ValueTuple<string, string>("782", "Auto start-stop"),
				new ValueTuple<string, string>("751", "Keyless access"),
				new ValueTuple<string, string>("755", "Light and rain sensor"),
				new ValueTuple<string, string>("756", "Occupant detection"),
				new ValueTuple<string, string>("757", "Power rear gate"),
				new ValueTuple<string, string>("746", "Power steering"),
				new ValueTuple<string, string>("753", "TPMS")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF", "1902AF", "1800FF00", "1802FF00" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Subaru", null);
				SubaruCANECU subaruCANECU = new SubaruCANECU(item2, item, possibleResponseHeader);
				list.Add(subaruCANECU);
			}
			return list;
		}

		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x00225717 File Offset: 0x00223917
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return SubaruCANECU.BuildList();
			}
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x00225720 File Offset: 0x00223920
		public static void FixSubaruDTCItems(List<DTCItemV2> dtcs, OBDRequest request, IECU ecu)
		{
			for (int i = 0; i < dtcs.Count; i++)
			{
				DTCItemV2 dtcitemV = dtcs[i];
				if (dtcitemV.RawCode != null && dtcitemV.RawCode.Length == 6 && dtcitemV.RawCode.StartsWith("00"))
				{
					DTCItemV2 dtcitemV2 = new DTCItemV2(BitHelpers.ConvertHexToBytesX(dtcitemV.RawCode.Substring(2)), request.Header, ecu.ResponseHeader, request.Command, DTCStatusHelper.DTCStatus.uds_bit0_testFailed, "");
					dtcitemV2.Statuses.Clear();
					dtcitemV2.Statuses.AddRange(dtcitemV.Statuses);
					dtcs[i] = dtcitemV2;
				}
			}
		}
	}
}
