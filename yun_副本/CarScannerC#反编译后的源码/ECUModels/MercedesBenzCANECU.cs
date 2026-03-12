using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004FD RID: 1277
	internal class MercedesBenzCANECU : CAN11bitECU
	{
		// Token: 0x06003163 RID: 12643 RVA: 0x00220FA0 File Offset: 0x0021F1A0
		public MercedesBenzCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AC", "190208", "19020D", "1800FF00", "1802FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x0022106C File Offset: 0x0021F26C
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("7E2", CAN11bitECU.ECU_ABS_NAME)
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Mercedes-Benz", null);
				MercedesBenzCANECU mercedesBenzCANECU = new MercedesBenzCANECU(item2, item, possibleResponseHeader);
				list.Add(mercedesBenzCANECU);
			}
			foreach (ValueTuple<string, string, string> valueTuple2 in new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("692", "492", "Adaptive damping system"),
				new ValueTuple<string, string, string>("632", "486", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string, string>("6B2", "496", "Steering"),
				new ValueTuple<string, string, string>("7E5", "7ED", "PTCU"),
				new ValueTuple<string, string, string>("662", "5E2", "SG-EM"),
				new ValueTuple<string, string, string>("624", "5A4", "EM1")
			})
			{
				string item3 = valueTuple2.Item1;
				string item4 = valueTuple2.Item2;
				MercedesBenzCANECU mercedesBenzCANECU2 = new MercedesBenzCANECU(valueTuple2.Item3, item3, item4);
				list.Add(mercedesBenzCANECU2);
			}
			return list;
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x002212B3 File Offset: 0x0021F4B3
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return MercedesBenzCANECU.BuildList();
			}
		}
	}
}
