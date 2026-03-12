using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004D0 RID: 1232
	internal class FAWCANECU : CAN11bitECU
	{
		// Token: 0x06003083 RID: 12419 RVA: 0x00219520 File Offset: 0x00217720
		public FAWCANECU(string name, string request, string response, string[] readDtcCommands = null, string clearDtcCommands = null, string openSession = "1001")
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "1001" };
			List<string> list;
			if (readDtcCommands != null)
			{
				list = new List<string>(readDtcCommands);
			}
			else
			{
				(list = new List<string>()).Add("190208");
			}
			base.ReadDTCCommands = list;
			List<string> list2;
			if (readDtcCommands != null)
			{
				list2 = new List<string>(readDtcCommands);
			}
			else
			{
				(list2 = new List<string>()).Add("14FFFFFF");
			}
			base.ReadDTCCommands = list2;
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x002195C4 File Offset: 0x002177C4
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("7E0", "7E8", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string, string>("7E1", "7E9", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string, string>("7A4", "7C4", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string, string>("7A2", "7C2", "SRS/Airbag"),
				new ValueTuple<string, string, string>("75E", "77E", "BCM"),
				new ValueTuple<string, string, string>("764", "784", "PEPS"),
				new ValueTuple<string, string, string>("763", "783", "Automatic park assist/Parking distance unit"),
				new ValueTuple<string, string, string>("774", "794", "T-BOX"),
				new ValueTuple<string, string, string>("76B", "78B", "Entertainment system controller"),
				new ValueTuple<string, string, string>("76B", "78B", "Entertainment system controller"),
				new ValueTuple<string, string, string>("7D1", "7D9", "Gearshift Actuation Mechanism Control Unit"),
				new ValueTuple<string, string, string>("7D0", "7D8", "Electronic Gearshift Module"),
				new ValueTuple<string, string, string>("740", "748", "A/C")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string>
				{
					"03", "03", "03", "07", "07", "07", "0A", "17FF00", "1902AF", "1902AC",
					"1800FF00", "1802FF00"
				},
				ClearDTCCommands = new List<string> { "04", "04", "04", "14", "14FF00", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				FAWCANECU fawcanecu = new FAWCANECU(valueTuple.Item3, item, item2, null, null, "1001");
				list.Add(fawcanecu);
			}
			return list;
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06003085 RID: 12421 RVA: 0x00219870 File Offset: 0x00217A70
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return FAWCANECU.BuildList();
			}
		}
	}
}
