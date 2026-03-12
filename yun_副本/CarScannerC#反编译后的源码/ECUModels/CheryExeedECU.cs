using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004CB RID: 1227
	internal class CheryExeedECU : CAN11bitECU
	{
		// Token: 0x06003073 RID: 12403 RVA: 0x00218450 File Offset: 0x00216650
		public CheryExeedECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "1001" };
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x002184E0 File Offset: 0x002166E0
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("7E0", "7E8", CAN11bitECU.ECU_ENGINE_NAME + " #1"),
				new ValueTuple<string, string, string>("700", "710", CAN11bitECU.ECU_ENGINE_NAME + " #2"),
				new ValueTuple<string, string, string>("702", "712", CAN11bitECU.ECU_TRANSMISSION_NAME + "#1 / AWD"),
				new ValueTuple<string, string, string>("7E1", "7E9", CAN11bitECU.ECU_TRANSMISSION_NAME + "#2"),
				new ValueTuple<string, string, string>("703", "713", CAN11bitECU.ECU_TRANSMISSION_NAME + "#3"),
				new ValueTuple<string, string, string>("720", "730", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string, string>("646", "656", "SRS/Airbags"),
				new ValueTuple<string, string, string>("724", "734", "Power steering"),
				new ValueTuple<string, string, string>("640", "650", "BCM"),
				new ValueTuple<string, string, string>("703", "713", "AWD"),
				new ValueTuple<string, string, string>("745", "765", "BCM"),
				new ValueTuple<string, string, string>("682", "692", "Rain light sensor"),
				new ValueTuple<string, string, string>("701", "711", "Fuel pump"),
				new ValueTuple<string, string, string>("66C", "67C", "Power rear gate"),
				new ValueTuple<string, string, string>("663", "673", "Parking sensors"),
				new ValueTuple<string, string, string>("666", "676", "Around view system")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF", "1902AC" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				CheryExeedECU cheryExeedECU = new CheryExeedECU(valueTuple.Item3, item, item2);
				list.Add(cheryExeedECU);
			}
			return list;
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x002187BE File Offset: 0x002169BE
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return CheryExeedECU.BuildList();
			}
		}
	}
}
