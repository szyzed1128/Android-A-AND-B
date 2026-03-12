using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004E6 RID: 1254
	internal class KTMCANECU : CAN11bitECU
	{
		// Token: 0x060030E1 RID: 12513 RVA: 0x0021D82C File Offset: 0x0021BA2C
		public KTMCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC", "190278", "1800FF00" };
			base.ClearDTCCommands = new List<string> { "14", "14FF00", "14FFFF", "14FFFFFF", "144000" };
			base.CloseSessionCommands = new List<string>();
			base.IdentsASCII.TryAdd("22F113", "Assembly ver.");
			base.IdentsASCII.TryAdd("22F188", "ECU Software Number.");
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x0021D924 File Offset: 0x0021BB24
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("7E0", "7E8", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string, string>("7E1", "7E9", "7E1/7E9 (ABS)"),
				new ValueTuple<string, string, string>("780", "781", "780/781")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00", "144000" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				FordCANECU fordCANECU = new FordCANECU(valueTuple.Item3, item, item2);
				if (item == "7E1")
				{
					fordCANECU.ReadDTCCommands.Clear();
					fordCANECU.ReadDTCCommands.AddRange(new string[] { "19004000", "190208", "190204", "1800FF00" });
					fordCANECU.ClearDTCCommands.Clear();
					fordCANECU.ClearDTCCommands.AddRange(new string[] { "144000", "14FFFFFF" });
				}
				if (item == "7E8")
				{
					fordCANECU.ReadDTCCommands.Clear();
					fordCANECU.ReadDTCCommands.AddRange(new string[] { "190208", "190204", "190202", "190201", "190280", "190220" });
				}
				if (item == "780")
				{
					fordCANECU.ReadDTCCommands.Clear();
					fordCANECU.ReadDTCCommands.AddRange(new string[] { "1800FF00", "190208" });
				}
				list.Add(fordCANECU);
			}
			return list;
		}

		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x060030E3 RID: 12515 RVA: 0x0021DB81 File Offset: 0x0021BD81
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return KTMCANECU.BuildList();
			}
		}
	}
}
